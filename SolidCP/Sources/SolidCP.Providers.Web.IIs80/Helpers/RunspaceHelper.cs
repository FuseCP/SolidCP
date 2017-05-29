using SolidCP.Server.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace SolidCP.Providers.Web
{
    public class RunspaceHelper
    {
        private static InitialSessionState session;

        static RunspaceHelper()
        {
        }

        public RunspaceHelper()
        {
        }

        public bool CheckWindowsFeatureInstallation(string featureName)
        {
            bool pSObjectProperty = false;
            Runspace runspace = null;
            try
            {
                runspace = this.OpenRunspace(new string[0]);
                Command command = new Command("Get-WindowsFeature");
                command.Parameters.Add("Name", featureName);
                PSObject pSObject = this.ExecuteShellCommand(runspace, command, false).FirstOrDefault<PSObject>();
                if (pSObject != null)
                {
                    pSObjectProperty = (bool)this.GetPSObjectProperty(pSObject, "Installed");
                }
            }
            finally
            {
                this.CloseRunspace(runspace);
            }
            return pSObjectProperty;
        }

        public void CloseRunspace(Runspace runspace)
        {
            try
            {
                if (runspace != null && runspace.RunspaceStateInfo.State == RunspaceState.Opened)
                {
                    runspace.Close();
                }
            }
            catch (Exception exception)
            {
                Log.WriteError("Runspace error", exception);
            }
        }

        public Collection<PSObject> ExecuteLocalScript(Runspace runSpace, List<string> scripts, out object[] errors, params string[] moduleImports)
        {
            return this.ExecuteRemoteScript(runSpace, null, scripts, out errors, moduleImports);
        }

        public Collection<PSObject> ExecuteRemoteScript(Runspace runSpace, string hostName, List<string> scripts, out object[] errors, params string[] moduleImports)
        {
            Command command = new Command("Invoke-Command");
            if (!string.IsNullOrEmpty(hostName))
            {
                command.Parameters.Add("ComputerName", hostName);
            }
            RunspaceInvoke runspaceInvoke = new RunspaceInvoke();
            string str = (moduleImports.Any<string>() ? string.Format("import-module {0};", string.Join(",", moduleImports)) : string.Empty);
            str = string.Format("{0};{1}", str, string.Join(";", scripts.ToArray()));
            ScriptBlock baseObject = runspaceInvoke.Invoke(string.Format("{{{0}}}", str))[0].BaseObject as ScriptBlock;
            command.Parameters.Add("ScriptBlock", baseObject);
            return this.ExecuteShellCommand(runSpace, command, false, out errors);
        }

        public Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd)
        {
            return this.ExecuteShellCommand(runSpace, cmd, true);
        }

        public Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, bool useDomainController)
        {
            object[] objArray;
            return this.ExecuteShellCommand(runSpace, cmd, useDomainController, out objArray);
        }

        public Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, out object[] errors)
        {
            return this.ExecuteShellCommand(runSpace, cmd, true, out errors);
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, bool useDomainController, out object[] errors)
        {
            Log.WriteStart("ExecuteShellCommand", new object[0]);
            List<object> objs = new List<object>();
            Collection<PSObject> pSObjects = null;
            Pipeline pipeline = runSpace.CreatePipeline();
            using (pipeline)
            {
                pipeline.Commands.Add(cmd);
                pSObjects = pipeline.Invoke();
                if (pipeline.Error != null && pipeline.Error.Count > 0)
                {
                    foreach (object end in pipeline.Error.ReadToEnd())
                    {
                        objs.Add(end);
                        Log.WriteWarning(string.Format("Invoke error: {0}", end), new object[0]);
                    }
                }
            }
            pipeline = null;
            errors = objs.ToArray();
            Log.WriteEnd("ExecuteShellCommand", new object[0]);
            return pSObjects;
        }

        public object GetPSObjectProperty(PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }

        internal string GetResultObjectDN(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectDN", new object[0]);
            if (result == null)
            {
                throw new ArgumentNullException("result", "Execution result is not specified");
            }
            if (result.Count < 1)
            {
                throw new ArgumentException("Execution result does not contain any object");
            }
            if (result.Count > 1)
            {
                throw new ArgumentException("Execution result contains more than one object");
            }
            PSMemberInfo item = result[0].Members["DistinguishedName"];
            if (item == null)
            {
                throw new ArgumentException("Execution result does not contain DistinguishedName property", "result");
            }
            string str = item.Value.ToString();
            Log.WriteEnd("GetResultObjectDN", new object[0]);
            return str;
        }

        internal string GetResultObjectIdentity(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectIdentity", new object[0]);
            if (result == null)
            {
                throw new ArgumentNullException("result", "Execution result is not specified");
            }
            if (result.Count < 1)
            {
                throw new ArgumentException("Execution result is empty", "result");
            }
            if (result.Count > 1)
            {
                throw new ArgumentException("Execution result contains more than one object", "result");
            }
            PSMemberInfo item = result[0].Members["Identity"];
            if (item == null)
            {
                throw new ArgumentException("Execution result does not contain Identity property", "result");
            }
            string str = item.Value.ToString();
            Log.WriteEnd("GetResultObjectIdentity", new object[0]);
            return str;
        }

        public bool InstallWindowsFeature(string featureName)
        {
            bool flag;
            Log.WriteStart("InstallWindowsFeature  {0}", new object[] { featureName });
            Runspace runspace = null;
            try
            {
                try
                {
                    runspace = this.OpenRunspace(new string[0]);
                    Command command = new Command("Install-WindowsFeature");
                    command.Parameters.Add("Name", featureName);
                    command.Parameters.Add("IncludeManagementTools", true);
                    this.ExecuteShellCommand(runspace, command, false);
                    return true;
                }
                catch (Exception exception)
                {
                    Log.WriteError(string.Format("InstallWindowsFeature  {0}", featureName), exception);
                    flag = false;
                }
            }
            finally
            {
                Log.WriteEnd("InstallWindowsFeature  {0}", new object[] { featureName });
                this.CloseRunspace(runspace);
            }
            return flag;
        }

        protected virtual Runspace OpenRunspace(string[] importModules)
        {
            Log.WriteStart("OpenRunspace", new object[0]);
            if (RunspaceHelper.session == null)
            {
                RunspaceHelper.session = InitialSessionState.CreateDefault();
                RunspaceHelper.session.ImportPSModule(importModules);
            }
            Runspace runspace = RunspaceFactory.CreateRunspace(RunspaceHelper.session);
            runspace.Open();
            runspace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            Log.WriteEnd("OpenRunspace", new object[0]);
            return runspace;
        }
    }
}
