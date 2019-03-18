// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using Microsoft.Win32;

namespace SolidCP.Providers.HostedSolution
{
    public class SfBBase : HostingServiceProviderBase, ISfBServer
    {
        #region Fields

        private static InitialSessionState session;

        #endregion

        #region Properties

        internal static string SfBRegistryPath { get; set; }
        internal static string SfBVersion { get; set; }

        internal string PoolFQDN
        {
            get { return ProviderSettings[SfBConstants.PoolFQDN]; }
        }

        internal string SimpleUrlRoot
        {
            get { return ProviderSettings[SfBConstants.SimpleUrlRoot]; }
        }

        internal string PrimaryDomainController
        {
            get { return ProviderSettings["PrimaryDomainController"]; }
        }

        internal string RootOU
        {
            get { return ProviderSettings["RootOU"]; }
        }

        internal string RootDomain
        {
            get { return ServerSettings.ADRootDomain; }
        }

        #endregion

        #region Methods

        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return CreateOrganizationInternal(organizationId, sipDomain, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public virtual string GetOrganizationTenantId(string organizationId)
        {
            return "NoHostingPack";
        }

        public virtual bool DeleteOrganization(string organizationId, string sipDomain)
        {
            return DeleteOrganizationInternal(organizationId, sipDomain);
        }

        public virtual bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return CreateUserInternal(organizationId, userUpn, plan);
        }

        public virtual SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn)
        {
            return GetSfBUserGeneralSettingsInternal(organizationId, userUpn);
        }

        public virtual bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser)
        {
            return SetSfBUserGeneralSettingsInternal(organizationId, userUpn, sfbUser);
        }

        public virtual bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return SetSfBUserPlanInternal(organizationId, userUpn, plan, null);
        }

        public virtual bool DeleteUser(string userUpn)
        {
            return DeleteUserInternal(userUpn);
        }

        public virtual SfBFederationDomain[] GetFederationDomains(string organizationId)
        {
            return GetFederationDomainsInternal(organizationId);
        }

        public virtual bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            return AddFederationDomainInternal(organizationId, domainName, proxyFqdn);
        }

        public virtual bool RemoveFederationDomain(string organizationId, string domainName)
        {
            return RemoveFederationDomainInternal(organizationId, domainName);
        }

        public virtual void ReloadConfiguration()
        {
            ReloadConfigurationInternal();
        }

        public virtual string[] GetPolicyList(SfBPolicyType type, string name)
        {
            return GetPolicyListInternal(type, name);
        }

        public override bool IsInstalled()
        {
            bool bResult = false;
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(SfBRegistryPath);

            if (registryKey != null)
            {
                var value = (string) registryKey.GetValue("ProductVersion", null);

                if (value.StartsWith(SfBVersion))
                {
                    bResult = true;
                }

                registryKey.Close();
            }

            return bResult;
        }

        internal virtual string CreateOrganizationInternal(string organizationId, string sipDomain, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            throw new NotImplementedException();
        }

        internal virtual bool DeleteOrganizationInternal(string organizationId, string sipDomain)
        {
            throw new NotImplementedException();
        }

        internal virtual bool CreateUserInternal(string organizationId, string userUpn, SfBUserPlan plan)
        {
            throw new NotImplementedException();
        }

        internal virtual SfBUser GetSfBUserGeneralSettingsInternal(string organizationId, string userUpn)
        {
            throw new NotImplementedException();
        }

        internal virtual bool SetSfBUserGeneralSettingsInternal(string organizationId, string userUpn, SfBUser sfbUser)
        {
            throw new NotImplementedException();
        }

        internal virtual bool SetSfBUserPlanInternal(string organizationId, string userUpn, SfBUserPlan plan, Runspace runspace)
        {
            throw new NotImplementedException();
        }

        internal virtual bool DeleteUserInternal(string userUpn)
        {
            throw new NotImplementedException();
        }

        internal virtual SfBFederationDomain[] GetFederationDomainsInternal(string organizationId)
        {
            throw new NotImplementedException();
        }

        internal virtual bool AddFederationDomainInternal(string organizationId, string domainName, string proxyFqdn)
        {
            throw new NotImplementedException();
        }

        internal virtual bool RemoveFederationDomainInternal(string organizationId, string domainName)
        {
            throw new NotImplementedException();
        }

        internal virtual void ReloadConfigurationInternal()
        {
            throw new NotImplementedException();
        }

        internal virtual string[] GetPolicyListInternal(SfBPolicyType type, string name)
        {
            throw new NotImplementedException();
        }

        #region PowerShell integration

        /// <summary> Opens runspace.</summary>
        /// <returns> The runspace.</returns>
        internal Runspace OpenRunspace()
        {
            HostedSolutionLog.LogStart("OpenRunspace");

            if (session == null)
            {
                session = InitialSessionState.CreateDefault();
                session.ImportPSModule(new[] {"ActiveDirectory", "SkypeForBusiness" });
            }

            Runspace runspace = RunspaceFactory.CreateRunspace(session);
            runspace.Open();
            runspace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            HostedSolutionLog.LogEnd("OpenRunspace");

            return runspace;
        }

        /// <summary> Closes runspace.</summary>
        /// <param name="runspace"> The runspace.</param>
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

        /// <summary> Executes shell command.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="scripts"> Scripts list.</param>
        /// <returns> The result.</returns>
        internal Collection<PSObject> ExecuteShellCommand(Runspace runspace, List<string> scripts)
        {
            object[] errors;
            return ExecuteShellCommand(runspace, scripts, out errors);
        }

        /// <summary> Executes shell command.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="command"> The command.</param>
        /// <param name="useDomainController"> True - if domain controller should be used.</param>
        /// <returns> The result.</returns>
        internal Collection<PSObject> ExecuteShellCommand(Runspace runspace, Command command, bool useDomainController)
        {
            object[] errors;
            return ExecuteShellCommand(runspace, command, useDomainController, out errors);
        }

        /// <summary> Executes shell command.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="command"> The command.</param>
        /// <param name="errors"> Errors list.</param>
        /// <returns> The result.</returns>
        internal Collection<PSObject> ExecuteShellCommand(Runspace runspace, Command command, out object[] errors)
        {
            return ExecuteShellCommand(runspace, command, true, out errors);
        }

        /// <summary> Executes shell command.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="command"> The command.</param>
        /// <param name="useDomainController"> True - if domain controller should be used.</param>
        /// <param name="errors"> Errors list.</param>
        /// <returns> The result.</returns>
        internal Collection<PSObject> ExecuteShellCommand(Runspace runspace, Command command, bool useDomainController, out object[] errors)
        {
            HostedSolutionLog.LogStart("ExecuteShellCommand");
            var errorList = new List<object>();

            if (useDomainController)
            {
                var dc = new CommandParameter("DomainController", PrimaryDomainController);
                if (!command.Parameters.Contains(dc))
                {
                    command.Parameters.Add(dc);
                }
            }

            HostedSolutionLog.DebugCommand(command);
            Collection<PSObject> results;
            Pipeline pipeLine = runspace.CreatePipeline();

            using (pipeLine)
            {
                pipeLine.Commands.Add(command);
                results = pipeLine.Invoke();

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

            errors = errorList.ToArray();
            HostedSolutionLog.LogEnd("ExecuteShellCommand");

            return results;
        }

        /// <summary> Executes shell command.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="scripts"> Scripts list.</param>
        /// <param name="errors"> Errors list.</param>
        /// <returns> The result.</returns>
        internal Collection<PSObject> ExecuteShellCommand(Runspace runspace, List<string> scripts, out object[] errors)
        {
            HostedSolutionLog.LogStart("ExecuteShellCommand");
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
                        HostedSolutionLog.LogWarning(errorMessage);

                        throw new ArgumentException(scripts.First());
                    }
                }
            }

            errors = errorList.ToArray();
            HostedSolutionLog.LogEnd("ExecuteShellCommand");

            return results;
        }

        /// <summary> Gets PSObject property value.</summary>
        /// <param name="obj"> The object.</param>
        /// <param name="name"> The property name.</param>
        /// <returns> The property value.</returns>
        internal object GetPSObjectProperty(PSObject obj, string name)
        {
            return obj.Members[name].Value;
        }          

        #endregion

        #region Transactions

        /// <summary> Starts the transaction.</summary>
        /// <returns> The transaction.</returns>
        internal SfBTransaction StartTransaction()
        {
            return new SfBTransaction();
        }

        /// <summary> Rollbacks the transaction.</summary>
        /// <param name="transaction"> The transaction.</param>
        internal void RollbackTransaction(SfBTransaction transaction)
        {
            HostedSolutionLog.LogStart("RollbackTransaction");
            Runspace runspace = null;
            try
            {
                runspace = OpenRunspace();

                for (int i = transaction.Actions.Count - 1; i > -1; i--)
                {
                    try
                    {
                        RollbackAction(transaction.Actions[i], runspace);
                    }
                    catch (Exception ex)
                    {
                        HostedSolutionLog.LogError("Rollback error", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("Rollback error", ex);
            }
            finally
            {
                CloseRunspace(runspace);
            }
            HostedSolutionLog.LogEnd("RollbackTransaction");
        }

        /// <summary> Rollbacks sfb action.</summary>
        /// <param name="action"> The action.</param>
        /// <param name="runspace"> The runspace.</param>
        private void RollbackAction(TransactionAction action, Runspace runspace)
        {
            HostedSolutionLog.LogInfo("Rollback action: {0}", action.ActionType);
            switch (action.ActionType)
            {
                case TransactionAction.TransactionActionTypes.SfBNewSipDomain:
                    DeleteSipDomain(runspace, action.Id);
                    break;
                case TransactionAction.TransactionActionTypes.SfBNewUser:
                    DeleteUser(runspace, action.Id);
                    break;
                case TransactionAction.TransactionActionTypes.SfBNewConferencingPolicy:
                    DeleteConferencingPolicy(runspace, action.Id);
                    break;
                case TransactionAction.TransactionActionTypes.SfBNewExternalAccessPolicy:
                    DeleteExternalAccessPolicy(runspace, action.Id);
                    break;
                case TransactionAction.TransactionActionTypes.SfBNewMobilityPolicy:
                    DeleteMobilityPolicy(runspace, action.Id);
                    break;
            }
        }

        #endregion

        #region Helpers

        /// <summary> Gets organizations AD path. </summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <returns> The organization AD path.</returns>
        internal string GetOrganizationPath(string organizationId)
        {
            var sb = new StringBuilder();
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        /// <summary> Appends organizational unit path.</summary>
        /// <param name="sb"> The string builder.</param>
        /// <param name="ou"> The organizational unit.</param>
        internal static void AppendOUPath(StringBuilder sb, string ou)
        {
            if (string.IsNullOrEmpty(ou))
            {
                return;
            }

            string path = ou.Replace("/", "\\");
            string[] parts = path.Split('\\');
            for (int i = parts.Length - 1; i != -1; i--)
            {
                sb.Append("OU=").Append(parts[i]).Append(",");
            }
        }

        /// <summary> Appends domain path.</summary>
        /// <param name="sb"> The string builder.</param>
        /// <param name="domain"> The domain name.</param>
        internal static void AppendDomainPath(StringBuilder sb, string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                return;
            }

            string[] parts = domain.Split('.');

            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append("DC=").Append(parts[i]);

                if (i < (parts.Length - 1))
                {
                    sb.Append(",");
                }
            }
        }

        /// <summary> Adds AD prefix.</summary>
        /// <param name="path"> The path.</param>
        /// <returns> The result.</returns>
        internal string AddADPrefix(string path)
        {
            string dn = path;
            if (!dn.ToUpper().StartsWith("LDAP://"))
            {
                dn = string.Format("LDAP://{0}/{1}", PrimaryDomainController, dn);
            }
            return dn;
        }

        /// <summary> Deletes sip domain.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="id"> The identifier.</param>
        internal void DeleteSipDomain(Runspace runspace, string id)
        {
            HostedSolutionLog.LogStart("DeleteSipDomain");
            HostedSolutionLog.DebugInfo("SipDomain : {0}", id);
            var command = new Command("Remove-CsSipDomain");
            command.Parameters.Add("Identity", id);
            command.Parameters.Add("Confirm", false);
            command.Parameters.Add("Force", true);
            ExecuteShellCommand(runspace, command, false);
            HostedSolutionLog.LogEnd("DeleteSipDomain");
        }

        /// <summary> Deletes user.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="userUpn"> The user UPN.</param>
        internal void DeleteUser(Runspace runspace, string userUpn)
        {
            HostedSolutionLog.LogStart("DeleteUser");
            HostedSolutionLog.DebugInfo("userUpn : {0}", userUpn);

            var command = new Command("Disable-CsUser");
            command.Parameters.Add("Identity", userUpn);
            command.Parameters.Add("Confirm", false);

            ExecuteShellCommand(runspace, command, false);
            HostedSolutionLog.LogEnd("DeleteUser");
        }

        /// <summary> Deletes conferencing policy.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="policyName"> The policy name.</param>
        internal void DeleteConferencingPolicy(Runspace runspace, string policyName)
        {
            HostedSolutionLog.LogStart("DeleteConferencingPolicy");
            HostedSolutionLog.DebugInfo("policyName : {0}", policyName);

            var command = new Command("Remove-CsConferencingPolicy");
            command.Parameters.Add("Identity", policyName);
            command.Parameters.Add("Confirm", false);
            command.Parameters.Add("Force", true);

            ExecuteShellCommand(runspace, command, false);
            HostedSolutionLog.LogEnd("DeleteConferencingPolicy");
        }

        /// <summary> Deletes external access policy.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="policyName"> The policy name.</param>
        internal void DeleteExternalAccessPolicy(Runspace runspace, string policyName)
        {
            HostedSolutionLog.LogStart("DeleteExternalAccessPolicy");
            HostedSolutionLog.DebugInfo("policyName : {0}", policyName);

            var command = new Command("Remove-CsExternalAccessPolicy");
            command.Parameters.Add("Identity", policyName);
            command.Parameters.Add("Confirm", false);
            command.Parameters.Add("Force", true);

            ExecuteShellCommand(runspace, command, false);
            HostedSolutionLog.LogEnd("DeleteExternalAccessPolicy");
        }

        /// <summary> Deletes mobility policy.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="policyName"> The policy name.</param>
        internal void DeleteMobilityPolicy(Runspace runspace, string policyName)
        {
            HostedSolutionLog.LogStart("DeleteMobilityPolicy");
            HostedSolutionLog.DebugInfo("policyName : {0}", policyName);

            var command = new Command("Remove-CsMobilityPolicy");
            command.Parameters.Add("Identity", policyName);
            command.Parameters.Add("Confirm", false);
            command.Parameters.Add("Force", true);

            ExecuteShellCommand(runspace, command, false);
            HostedSolutionLog.LogEnd("DeleteMobilityPolicy");
        }

        /// <summary> Creates simple url.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="id"> The identifier.</param>
        internal void CreateSimpleUrl(Runspace runspace, Guid id)
        {
            IList<PSObject> SimpleUrls = new List<PSObject>();

            // Set the Dialin URL
            string DialinUrl = SimpleUrlRoot + "dialin";

            var command = new Command("New-CsSimpleUrlEntry");
            command.Parameters.Add("Url", DialinUrl);
            Collection<PSObject> simpleDialinUrlEntry = ExecuteShellCommand(runspace, command, false);

            command = new Command("New-CsSimpleUrl");
            command.Parameters.Add("Component", "Dialin");
            command.Parameters.Add("Domain", '*');
            command.Parameters.Add("SimpleUrl", simpleDialinUrlEntry);
            command.Parameters.Add("ActiveUrl", DialinUrl);
            Collection<PSObject> simpleDialinUrl = ExecuteShellCommand(runspace, command, false);

            SimpleUrls.Add(simpleDialinUrl[0]);

            // Set Metting URL
            command = new Command("Get-CsSipDomain");
            Collection<PSObject> sipDomains = ExecuteShellCommand(runspace, command, false);

            foreach (PSObject domain in sipDomains)
            {
                var d = (string) GetPSObjectProperty(domain, "Name");
                string Url = SimpleUrlRoot + d;

                command = new Command("New-CsSimpleUrlEntry");
                command.Parameters.Add("Url", Url);
                Collection<PSObject> simpleUrlEntry = ExecuteShellCommand(runspace, command, false);

                command = new Command("New-CsSimpleUrl");
                command.Parameters.Add("Component", "meet");
                command.Parameters.Add("Domain", d);
                command.Parameters.Add("SimpleUrl", simpleUrlEntry[0]);
                command.Parameters.Add("ActiveUrl", Url);
                Collection<PSObject> simpleUrl = ExecuteShellCommand(runspace, command, false);

                SimpleUrls.Add(simpleUrl[0]);
            }

            command = new Command("Set-CsSimpleUrlConfiguration");
            command.Parameters.Add("Identity", "Global");
            //command.Parameters.Add("Tenant", id);
            command.Parameters.Add("SimpleUrl", SimpleUrls);
            ExecuteShellCommand(runspace, command, false);
        }

        #endregion

        #endregion
    }
}
