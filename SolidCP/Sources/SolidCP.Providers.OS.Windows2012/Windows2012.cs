// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Globalization;

using Microsoft.Win32;

using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

using SolidCP.Providers.Common;

using System.Runtime.InteropServices;
using System.Linq;
using SolidCP.Providers.DomainLookup;
using SolidCP.Providers.DNS;


namespace SolidCP.Providers.OS
{

    public class Windows2012 : Windows2003
    {
        #region Properties
        internal string PrimaryDomainController
        {
            get { return ProviderSettings["PrimaryDomainController"]; }
        }

        #endregion Properties
        
        public override bool IsInstalled()
        {
            Server.Utils.OS.WindowsVersion version = SolidCP.Server.Utils.OS.GetVersion();
            return version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012
                || version == SolidCP.Server.Utils.OS.WindowsVersion.Windows8
                || version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012R2
                || version == SolidCP.Server.Utils.OS.WindowsVersion.Windows81;
        }
        
        public override void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            Log.WriteStart("SetQuotaLimitOnFolder");
            Log.WriteInfo("FolderPath : {0}", folderPath);
            Log.WriteInfo("QuotaLimit : {0}", quotaLimit);

            string path = folderPath;

            if (shareNameDrive != null)
                path = Path.Combine(shareNameDrive + @":\", folderPath);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                {
                    if (!FileUtils.DirectoryExists(path))
                        FileUtils.CreateDirectory(path);

                    if (quotaLimit.Contains("-"))
                    {
                        RemoveOldQuotaOnFolder(runSpace, path);
                    }
                    else
                    {
                        var quota = CalculateQuota(quotaLimit);

                        switch (mode)
                        {
                            //deleting old quota and creating new one
                            case 0:
                                {
                                    RemoveOldQuotaOnFolder(runSpace, path);
                                    ChangeQuotaOnFolder(runSpace, "New-FsrmQuota", path, quotaType, quota);
                                    break;
                                }
                            //modifying folder quota
                            case 1:
                                {
                                    ChangeQuotaOnFolder(runSpace, "Set-FsrmQuota", path, quotaType, quota);
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("SetQuotaLimitOnFolder", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            Log.WriteEnd("SetQuotaLimitOnFolder");
        }

        public override Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
        {
            Log.WriteStart("GetQuotaLimitOnFolder");
            Log.WriteInfo("FolderPath : {0}", folderPath);
            
            
            Runspace runSpace = null;
            Quota quota = new Quota();

            try
            {
                runSpace = OpenRunspace();

                if (folderPath.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                {
                    Command cmd = new Command("Get-FsrmQuota");
                    cmd.Parameters.Add("Path", folderPath);
                    var result = ExecuteShellCommand(runSpace, cmd, false);

                    if (result.Count > 0)
                    {
                        quota.Size = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(result[0], "Size")));
                        quota.QuotaType = Convert.ToBoolean(GetPSObjectProperty(result[0], "SoftLimit")) ? QuotaType.Soft : QuotaType.Hard;
                        quota.Usage = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(result[0], "usage")));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("GetQuotaLimitOnFolder", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            Log.WriteEnd("GetQuotaLimitOnFolder");

            return quota;
        }

        public override Dictionary<string, Quota> GetQuotasForOrganization(string folderPath, string wmiUserName, string wmiPassword)
        {
            Log.WriteStart("GetQuotasLimitsForOrganization");

            // 05.09.2015 roland.breitschaft@x-company.de
            // New: Add LogInfo
            Log.WriteInfo("FolderPath : {0}", folderPath);

            Runspace runSpace = null;
            Quota quota = null;
            var quotas = new Dictionary<string, Quota>();

            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-FsrmQuota");

                cmd.Parameters.Add("Path", folderPath + "\\*");
                var result = ExecuteShellCommand(runSpace, cmd, false);                

                if (result.Count > 0)
                {
                    foreach (var element in result)
                    {
                        quota = new Quota();

                        quota.Size = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(element, "Size")));
                        quota.QuotaType = Convert.ToBoolean(GetPSObjectProperty(element, "SoftLimit")) ? QuotaType.Soft : QuotaType.Hard;
                        quota.Usage = ConvertBytesToMB(Convert.ToInt64(GetPSObjectProperty(element, "usage")));

                        quotas.Add(Convert.ToString(GetPSObjectProperty(element, "Path")), quota);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("GetQuotasLimitsForOrganization", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            Log.WriteEnd("GetQuotasLimitsForOrganization");

            return quotas;
        }

        public UInt64 CalculateQuota(string quota)
        {
            UInt64 OneKb = 1024;
            UInt64 OneMb = OneKb * 1024;
            UInt64 OneGb = OneMb * 1024;

            UInt64 result = 0;

            // Quota Unit
            if (quota.ToLower().Contains("gb"))
            {
                result = UInt64.Parse(quota.ToLower().Replace("gb", "")) * OneGb;
            }
            else if (quota.ToLower().Contains("mb"))
            {
                result = UInt64.Parse(quota.ToLower().Replace("mb", "")) * OneMb;
            }
            else
            {
                result = UInt64.Parse(quota.ToLower().Replace("kb", "")) * OneKb;
            }

            return result;
        }

        public int ConvertMegaBytesToGB(int megabytes)
        {
            int OneGb = 1024;

            if (megabytes == -1)
                return megabytes;

            return (int)(megabytes/ OneGb);
        }

        public int ConvertBytesToMB(long bytes)
        {
            int OneKb = 1024;
            int OneMb = OneKb * 1024;

            if (bytes == 0)
                return 0;

            return (int)(bytes / OneMb);
        }

        public void RemoveOldQuotaOnFolder(Runspace runSpace, string path)
        {
            try
            {
                runSpace = OpenRunspace();
                if (!string.IsNullOrEmpty(path))
                {
                    Command cmd = new Command("Remove-FsrmQuota");
                    cmd.Parameters.Add("Path", path);
                    ExecuteShellCommand(runSpace, cmd, false);
                }
            }
            catch { /* do nothing */ }
        }

        public void ChangeQuotaOnFolder(Runspace runSpace, string command, string path, QuotaType quotaType, UInt64 quota)
        {
            Command cmd = new Command(command);
            cmd.Parameters.Add("Path", path);
            cmd.Parameters.Add("Size", quota);

            if (quotaType == QuotaType.Soft)
            {
                cmd.Parameters.Add("SoftLimit", true);
            }

            ExecuteShellCommand(runSpace, cmd, false);
        }

        public override bool InstallFsrmService()
        {
            Log.WriteStart("InstallFsrmService");

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Install-WindowsFeature");
                cmd.Parameters.Add("Name", "FS-Resource-Manager");
                cmd.Parameters.Add("IncludeManagementTools", true);

                ExecuteShellCommand(runSpace, cmd, false);
            }
            catch (Exception ex)
            {
                Log.WriteError("InstallFsrmService", ex);

                return false;
            }
            finally
            {
                Log.WriteEnd("InstallFsrmService");

                CloseRunspace(runSpace);
            }

            return true;
        }

        #region PowerShell integration
        private static InitialSessionState session = null;

        protected virtual Runspace OpenRunspace()
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

        protected void CloseRunspace(Runspace runspace)
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

        protected Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd)
        {
            return ExecuteShellCommand(runSpace, cmd, true);
        }

        protected Collection<PSObject> ExecuteLocalScript(Runspace runSpace,  List<string> scripts, out object[] errors, params string[] moduleImports)
        {
            return ExecuteRemoteScript(runSpace, null ,scripts, out errors, moduleImports);
        }

        protected Collection<PSObject> ExecuteRemoteScript(Runspace runSpace, string hostName, List<string> scripts, out object[] errors, params string[] moduleImports)
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

            return ExecuteShellCommand(runSpace, invokeCommand, false, out errors);
        }

        protected Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, bool useDomainController)
        {
            object[] errors;
            return ExecuteShellCommand(runSpace, cmd, useDomainController, out errors);
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, out object[] errors)
        {
            return ExecuteShellCommand(runSpace, cmd, true, out errors);
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, bool useDomainController, out object[] errors)
        {
            Log.WriteStart("ExecuteShellCommand");

            // 05.09.2015 roland.breitschaft@x-company.de
            // New: Add LogInfo
            Log.WriteInfo("Command              : {0}", cmd.CommandText);
            foreach (var par in cmd.Parameters)            
                Log.WriteInfo("Parameter            : Name {0}, Value {1}", par.Name, par.Value);
            Log.WriteInfo("UseDomainController  : {0}", useDomainController);

            List<object> errorList = new List<object>();

            if (useDomainController)
            {
                CommandParameter dc = new CommandParameter("DomainController", PrimaryDomainController);
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

        protected object GetPSObjectProperty(PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }

        /// <summary>
        /// Returns the identity of the object from the shell execution result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal string GetResultObjectIdentity(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectIdentity");
            if (result == null)
                throw new ArgumentNullException("result", "Execution result is not specified");

            if (result.Count < 1)
                throw new ArgumentException("Execution result is empty", "result");

            if (result.Count > 1)
                throw new ArgumentException("Execution result contains more than one object", "result");

            PSMemberInfo info = result[0].Members["Identity"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain Identity property", "result");

            string ret = info.Value.ToString();
            Log.WriteEnd("GetResultObjectIdentity");
            return ret;
        }

        internal string GetResultObjectDN(Collection<PSObject> result)
        {
            Log.WriteStart("GetResultObjectDN");
            if (result == null)
                throw new ArgumentNullException("result", "Execution result is not specified");

            if (result.Count < 1)
                throw new ArgumentException("Execution result does not contain any object");

            if (result.Count > 1)
                throw new ArgumentException("Execution result contains more than one object");

            PSMemberInfo info = result[0].Members["DistinguishedName"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain DistinguishedName property", "result");

            string ret = info.Value.ToString();
            Log.WriteEnd("GetResultObjectDN");
            return ret;
        }


        #endregion

    }
}
