using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.EnterpriseStorage
{
    /// <summary>
    /// Helper Class für Workfolderservice in Windows Server 2012
    /// 
    /// Prerequisites:
    /// - Workfolder Feature must installed
    /// - DNS must set to workfolders.domain.tld
    /// - Change Binding in C:\Windows\System32\SyncShareSvc.config to DNS-Setting
    /// - Add SSL Cert (below is an Example)
    ///     # Determine the Hash vor the Cert
    ///     Get-ChildItem -Path Cert:\LocalMachine\WebHosting
    ///     # Add Hash (Remarks! appid is everytime the same)
    ///     netsh http add sslcert ipport=0.0.0.0:443 certhash=82A4055498EA2get-7EF57AA7FA53DA9E822BEF59628 appid={CE66697B-3AA0-49D1-BDBD-A25C8359FD5D} certstorename=WebHosting
    /// </summary>
    internal class SyncShareService
    {
        internal SyncShareService()
        {

        }


        private bool CheckWindowsFeatureInstallation(string featureName)
        {
            bool isInstalled = false;

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-WindowsFeature");
                cmd.Parameters.Add("Name", featureName);

                var feature = ExecuteShellCommand(runSpace, cmd).FirstOrDefault();

                if (feature != null)
                {
                    isInstalled = (bool)GetPSObjectProperty(feature, "Installed");
                }
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            return isInstalled;
        }

        private bool InstallWindwosFeature(string featureName)
        {
            Log.WriteStart("InstallWindowsFeature  {0}", featureName);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Install-WindowsFeature");
                cmd.Parameters.Add("Name", featureName);
                cmd.Parameters.Add("IncludeManagementTools", true);

                ExecuteShellCommand(runSpace, cmd);
            }
            catch (Exception ex)
            {
                Log.WriteError(string.Format("InstallWindowsFeature  {0}", featureName), ex);

                return false;
            }
            finally
            {
                Log.WriteEnd("InstallWindowsFeature  {0}", featureName);

                CloseRunspace(runSpace);
            }

            return true;
        }

        #region PowerShell integration
        private static InitialSessionState session = null;

        private Runspace OpenRunspace()
        {
            Log.WriteStart("OpenRunspace");

            if (session == null)
            {
                session = InitialSessionState.CreateDefault();
                session.ImportPSModule(new string[] { "FileServerResourceManager" });
            }
            Runspace runSpace = RunspaceFactory.CreateRunspace(session);
            //
            runSpace.Open();
            //
            runSpace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            Log.WriteEnd("OpenRunspace");
            return runSpace;
        }

        private void CloseRunspace(Runspace runspace)
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

        private Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd)
        {
            object[] errors;
            return ExecuteShellCommand(runSpace, cmd, out errors);
        }

        private Collection<PSObject> ExecuteLocalScript(Runspace runSpace, List<string> scripts, out object[] errors, params string[] moduleImports)
        {
            return ExecuteRemoteScript(runSpace, null, scripts, out errors, moduleImports);
        }

        private Collection<PSObject> ExecuteRemoteScript(Runspace runSpace, string hostName, List<string> scripts, out object[] errors, params string[] moduleImports)
        {
            Command invokeCommand = new Command("Invoke-Command");

            if (!string.IsNullOrEmpty(hostName))
            {
                invokeCommand.Parameters.Add("ComputerName", hostName);
            }

            RunspaceInvoke invoke = new RunspaceInvoke();
            string commandString = moduleImports.Any() ? string.Format("import-module {0};", string.Join(",", moduleImports)) : string.Empty;

            commandString = string.Format("{0};{1}", commandString, string.Join(";", scripts.ToArray()));

            ScriptBlock sb = invoke.Invoke(string.Format("{{{0}}}", commandString))[0].BaseObject as ScriptBlock;

            invokeCommand.Parameters.Add("ScriptBlock", sb);

            return ExecuteShellCommand(runSpace, invokeCommand, out errors);
        }

        private Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, out object[] errors)
        {            
            Log.WriteStart("ExecuteShellCommand");

            // 05.09.2015 roland.breitschaft@x-company.de
            // New: Add LogInfo
            Log.WriteInfo("Command              : {0}", cmd.CommandText);
            foreach (var par in cmd.Parameters)
                Log.WriteInfo("Parameter            : Name {0}, Value {1}", par.Name, par.Value);

            List<object> errorList = new List<object>();

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

        private object GetPSObjectProperty(PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }

        #endregion

        #region FS-SyncShareService

        internal bool IsSyncShareInstalled()
        {
            return CheckWindowsFeatureInstallation("FS-SyncShareService");
        }

        internal bool InstallSyncShare()
        {
            return InstallWindwosFeature("FS-SyncShareService");
        }

        internal void CreateSyncShare(string name, string path, string user)
        {
            Log.WriteStart("CreateSyncShare");

            Runspace runSpace = null;            

            try
            {
                runSpace = OpenRunspace();

                // ToDo: Add the correct parameters

                Command cmd = new Command("New-SyncShare");
                cmd.Parameters.Add("Name", name);
                cmd.Parameters.Add("Path", path);
                cmd.Parameters.Add("user", user);
                var result = ExecuteShellCommand(runSpace, cmd);

                if (result.Count > 0)
                {
                
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("CreateSyncShare", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            Log.WriteEnd("CreateSyncShare");
        }

        internal bool ExistSyncShare(string shareName)
        {
            throw new NotImplementedException();
        }

        internal bool UpdateSyncSahre()
        {
            throw new NotImplementedException();
        }

        internal bool RemoveSyncShare()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
