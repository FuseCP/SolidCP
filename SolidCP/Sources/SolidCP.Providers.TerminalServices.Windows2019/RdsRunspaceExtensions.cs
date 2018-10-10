using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public static class RdsRunspaceExtensions
    {
        private static InitialSessionState session = null;

        public static Runspace OpenRunspace()
        {
            Log.WriteStart("OpenRunspace");

            if (session == null)
            {
                session = InitialSessionState.CreateDefault();
                session.ImportPSModule(new string[] { "ServerManager", "RemoteDesktop", "RemoteDesktopServices" });
            }
            Runspace runSpace = RunspaceFactory.CreateRunspace(session);            
            runSpace.Open();            
            runSpace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            Log.WriteEnd("OpenRunspace");
            return runSpace;
        }

        public static void CloseRunspace(this Runspace runspace)
        {
            try
            {
                if (runspace != null && runspace.RunspaceStateInfo.State == RunspaceState.Opened)
                {
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Runspace error", ex);
            }
        }     

        public static RdsCollection GetCollection(this Runspace runspace, string collectionName, string connectionBroker, string primaryDomainController)
        {
            RdsCollection collection = null;
            Command cmd = new Command("Get-RDSessionCollection");
            cmd.Parameters.Add("CollectionName", collectionName);
            cmd.Parameters.Add("ConnectionBroker", connectionBroker);

            var collectionPs = ExecuteShellCommand(runspace, cmd, false, primaryDomainController).FirstOrDefault();

            if (collectionPs != null)
            {
                collection = new RdsCollection();
                collection.Name = Convert.ToString(GetPSObjectProperty(collectionPs, "CollectionName"));
                collection.Description = Convert.ToString(GetPSObjectProperty(collectionPs, "CollectionDescription"));
            }

            return collection;
        }

        public static List<RdsCollectionSetting> GetCollectionSettings(this Runspace runspace, string collectionName, string connectionBroker, string primaryDomainController, out object[] errors)
        {
            var result = new List<RdsCollectionSetting>();
            var errorsList = new List<object>();

            result.AddRange(GetCollectionSettings(runspace, collectionName, connectionBroker, primaryDomainController, "Connection", out errors));
            errorsList.AddRange(errors);
            result.AddRange(GetCollectionSettings(runspace, collectionName, connectionBroker, primaryDomainController, "UserProfileDisk", out errors));
            errorsList.AddRange(errors);
            result.AddRange(GetCollectionSettings(runspace, collectionName, connectionBroker, primaryDomainController, "Security", out errors));
            errorsList.AddRange(errors);
            result.AddRange(GetCollectionSettings(runspace, collectionName, connectionBroker, primaryDomainController, "LoadBalancing", out errors));
            errorsList.AddRange(errors);
            result.AddRange(GetCollectionSettings(runspace, collectionName, connectionBroker, primaryDomainController, "Client", out errors));
            errorsList.AddRange(errors);
            errors = errorsList.ToArray();

            return result;
        }

        public static List<RdsCollectionSetting> GetCollectionUserGroups(this Runspace runspace, string collectionName, string connectionBroker, string primaryDomainController, out object[] errors)
        {
            return GetCollectionSettings(runspace, collectionName, connectionBroker, primaryDomainController, "UserGroup", out errors);
        }

        public static List<string> GetSessionHosts(this Runspace runspace, string collectionName, string connectionBroker, string primaryDomainController, out object[] errors)
        {
            Command cmd = new Command("Get-RDSessionHost");
            cmd.Parameters.Add("CollectionName", collectionName);
            cmd.Parameters.Add("ConnectionBroker", connectionBroker);

            var psObjects = ExecuteShellCommand(runspace, cmd, false, primaryDomainController, out errors);
            var rdsServers = new List<string>();

            if (psObjects != null)
            {
                foreach(var psObject in psObjects)
                {
                    rdsServers.Add(GetPSObjectProperty(psObject, "SessionHost").ToString());
                }
            }

            return rdsServers;
        }

        private static List<RdsCollectionSetting> GetCollectionSettings(this Runspace runspace, string collectionName, string connectionBroker, string primaryDomainController, string param, out object[] errors)
        {
            Command cmd = new Command("Get-RDSessionCollectionConfiguration");
            cmd.Parameters.Add("CollectionName", collectionName);
            cmd.Parameters.Add("ConnectionBroker", connectionBroker);

            if (!string.IsNullOrEmpty(param))
            {
                cmd.Parameters.Add(param, true);
            }

            var psObject = ExecuteShellCommand(runspace, cmd, false, primaryDomainController, out errors).FirstOrDefault();

            var properties = typeof(RdsCollectionSettings).GetProperties().Select(p => p.Name.ToLower());
            var collectionSettings = new RdsCollectionSettings();
            var result = new List<RdsCollectionSetting>();

            if (psObject != null)
            {
                foreach (var prop in psObject.Properties)
                {                    
                    if (prop.Name.ToLower() != "id" && prop.Name.ToLower() != "rdscollectionid")
                    {
                        result.Add(new RdsCollectionSetting
                        {
                            PropertyName = prop.Name,
                            PropertyValue = prop.Value
                        });
                    }
                }
            }

            return result;
        }           
    
        public static Collection<PSObject> ExecuteRemoteShellCommand(this Runspace runSpace, string hostName, Command cmd, string primaryDomainController, params string[] moduleImports)
        {
            object[] errors;
            return ExecuteRemoteShellCommand(runSpace, hostName, cmd, primaryDomainController, out errors, moduleImports);
        }

        public static Collection<PSObject> ExecuteRemoteShellCommand(this Runspace runSpace, string hostName, Command cmd, string primaryDomainController, out object[] errors, params string[] moduleImports)
        {
            Command invokeCommand = new Command("Invoke-Command");
            invokeCommand.Parameters.Add("ComputerName", hostName);

            RunspaceInvoke invoke = new RunspaceInvoke();

            string commandString = moduleImports.Any() ? string.Format("import-module {0};", string.Join(",", moduleImports)) : string.Empty;

            commandString += cmd.CommandText;

            if (cmd.Parameters != null && cmd.Parameters.Any())
            {
                commandString += " " +
                                 string.Join(" ",
                                     cmd.Parameters.Select(x => string.Format("-{0} {1}", x.Name, x.Value)).ToArray());
            }

            ScriptBlock sb = invoke.Invoke(string.Format("{{{0}}}", commandString))[0].BaseObject as ScriptBlock;

            invokeCommand.Parameters.Add("ScriptBlock", sb);            

            return ExecuteShellCommand(runSpace, invokeCommand, false, primaryDomainController, out errors);
        }

        public static Collection<PSObject> ExecuteRemoteShellCommand(this Runspace runSpace, string hostName, List<string> scripts, string primaryDomainController, out object[] errors, params string[] moduleImports)
        {
            Command invokeCommand = new Command("Invoke-Command");
            invokeCommand.Parameters.Add("ComputerName", hostName);

            RunspaceInvoke invoke = new RunspaceInvoke();
            string commandString = moduleImports.Any() ? string.Format("import-module {0};", string.Join(",", moduleImports)) : string.Empty;

            commandString = string.Format("{0};{1}", commandString, string.Join(";", scripts.ToArray()));            

            ScriptBlock sb = invoke.Invoke(string.Format("{{{0}}}", commandString))[0].BaseObject as ScriptBlock;

            invokeCommand.Parameters.Add("ScriptBlock", sb);

            return ExecuteShellCommand(runSpace, invokeCommand, false, primaryDomainController, out errors);
        }        

        public static Collection<PSObject> ExecuteShellCommand(this Runspace runspace, List<string> scripts, out object[] errors)
        {
            Log.WriteStart("ExecuteShellCommand");
            var errorList = new List<object>();
            Collection<PSObject> results;

            using (Pipeline pipeLine = runspace.CreatePipeline())
            {
                foreach (string script in scripts)
                {
                    pipeLine.Commands.AddScript(script);
                }
                
                results = pipeLine.Invoke();

                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Add(item);
                        string errorMessage = string.Format("Invoke error: {0}", item);
                        Log.WriteWarning(errorMessage);
                    }
                }
            }

            errors = errorList.ToArray();
            Log.WriteEnd("ExecuteShellCommand");

            return results;
        }

        public static Collection<PSObject> ExecuteShellCommand(this Runspace runSpace, Command cmd, bool useDomainController, string primaryDomainController)
        {
            object[] errors;
            return ExecuteShellCommand(runSpace, cmd, useDomainController, primaryDomainController, out errors);
        } 

        public static Collection<PSObject> ExecuteShellCommand(this Runspace runSpace, Command cmd, bool useDomainController, string primaryDomainController,
            out object[] errors)
        {
            Log.WriteStart("ExecuteShellCommand");
            List<object> errorList = new List<object>();

            if (useDomainController)
            {
                CommandParameter dc = new CommandParameter("DomainController", primaryDomainController);
                if (!cmd.Parameters.Contains(dc))
                {
                    cmd.Parameters.Add(dc);
                }
            }

            Collection<PSObject> results = null;
            // Create a pipeline
            Pipeline pipeLine = runSpace.CreatePipeline();
            using (pipeLine)
            {
                // Add the command
                pipeLine.Commands.Add(cmd);
                // Execute the pipeline and save the objects returned.
                results = pipeLine.Invoke();

                // Log out any errors in the pipeline execution
                // NOTE: These errors are NOT thrown as exceptions! 
                // Be sure to check this to ensure that no errors 
                // happened while executing the command.
                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Add(item);
                        string errorMessage = string.Format("Invoke error: {0}", item);
                        Log.WriteWarning(errorMessage);
                    }
                }
            }
            pipeLine = null;
            errors = errorList.ToArray();
            Log.WriteEnd("ExecuteShellCommand");
            return results;
        }

        public static object GetPSObjectProperty(PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }
    }
}
