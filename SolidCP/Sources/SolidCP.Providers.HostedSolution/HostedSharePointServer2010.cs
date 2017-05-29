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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.Management.Automation;
using System.Management.Automation.Runspaces;

using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

using SolidCP.Providers.SharePoint;


namespace SolidCP.Providers.HostedSolution
{
    public class HostedSharePointServer2010 : HostedSharePointServer
    {
        public HostedSharePointServer2010()
        {
            this.Wss3RegistryKey = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\14.0";
            this.Wss3Registry32Key = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\14.0";
            this.LanguagePacksPath = @"%commonprogramfiles%\microsoft shared\Web Server Extensions\14\HCCab\";
        }

        #region PowerShell integration
        private static RunspaceConfiguration runspaceConfiguration = null;

        internal virtual string SharepointSnapInName
        {
            get { return "Microsoft.SharePoint.Powershell"; }
        }

        internal virtual Runspace OpenRunspace()
        {
            HostedSolutionLog.LogStart("OpenRunspace");

            if (runspaceConfiguration == null)
            {
                runspaceConfiguration = RunspaceConfiguration.Create();
                PSSnapInException exception = null;

                PSSnapInInfo info = runspaceConfiguration.AddPSSnapIn(SharepointSnapInName, out exception);
                HostedSolutionLog.LogInfo("Sharepoint snapin loaded");

                if (exception != null)
                {
                    HostedSolutionLog.LogWarning("SnapIn error", exception);
                }
            }
            Runspace runSpace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            //
            runSpace.Open();
            //
            runSpace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            HostedSolutionLog.LogEnd("OpenRunspace");
            return runSpace;
        }

        internal void CloseRunspace(Runspace runspace)
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
                HostedSolutionLog.LogError("Runspace error", ex);
            }
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd)
        {
            return ExecuteShellCommand(runSpace, cmd, true);
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, bool useDomainController)
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
            HostedSolutionLog.LogStart("ExecuteShellCommand");
            List<object> errorList = new List<object>();

            HostedSolutionLog.DebugCommand(cmd);
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
                        HostedSolutionLog.LogWarning(errorMessage);
                    }
                }
            }
            pipeLine = null;
            errors = errorList.ToArray();
            HostedSolutionLog.LogEnd("ExecuteShellCommand");
            return results;
        }

        /// <summary>
        /// Returns the distinguished name of the object from the shell execution result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal string GetResultObjectDN(Collection<PSObject> result)
        {
            HostedSolutionLog.LogStart("GetResultObjectDN");
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
            HostedSolutionLog.LogEnd("GetResultObjectDN");
            return ret;
        }

        /// <summary>
        /// Checks the object from the shell execution result.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Distinguished name of the object if object exists or null otherwise.</returns>
        internal string CheckResultObjectDN(Collection<PSObject> result)
        {
            HostedSolutionLog.LogStart("CheckResultObjectDN");

            if (result == null)
                return null;

            if (result.Count < 1)
                return null;

            PSMemberInfo info = result[0].Members["DistinguishedName"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain DistinguishedName property", "result");

            string ret = info.Value.ToString();
            HostedSolutionLog.LogEnd("CheckResultObjectDN");
            return ret;
        }

        /// <summary>
        /// Returns the identity of the object from the shell execution result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal string GetResultObjectIdentity(Collection<PSObject> result)
        {
            HostedSolutionLog.LogStart("GetResultObjectIdentity");
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
            HostedSolutionLog.LogEnd("GetResultObjectIdentity");
            return ret;
        }

        /// <summary>
        /// Returns the identity of the PS object 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal string GetPSObjectIdentity(PSObject obj)
        {
            HostedSolutionLog.LogStart("GetPSObjectIdentity");
            if (obj == null)
                throw new ArgumentNullException("obj", "PSObject is not specified");


            PSMemberInfo info = obj.Members["Identity"];
            if (info == null)
                throw new ArgumentException("PSObject does not contain Identity property", "obj");

            string ret = info.Value.ToString();
            HostedSolutionLog.LogEnd("GetPSObjectIdentity");
            return ret;
        }

        internal object GetPSObjectProperty(PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }

        #endregion

        public override void SetPeoplePickerOu(string site, string ou)
        {
            HostedSolutionLog.LogStart("SetPeoplePickerOu");
            HostedSolutionLog.LogInfo("  Site: {0}", site);
            HostedSolutionLog.LogInfo("  OU: {0}", ou);

            Runspace runSpace = null;
            try
            {
                List<SharePointSiteCollection> siteCollections = new List<SharePointSiteCollection>();

                runSpace = OpenRunspace();
                Command cmd = new Command("Set-SPSite");
                cmd.Parameters.Add("Identity", site);
                cmd.Parameters.Add("UserAccountDirectoryPath", ou);
                ExecuteShellCommand(runSpace, cmd);

            }
            finally
            {

                CloseRunspace(runSpace);
            }

            HostedSolutionLog.LogEnd("SetPeoplePickerOu");
        }


    }
}
