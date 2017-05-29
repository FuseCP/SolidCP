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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.Rtc.Management.WritableConfig.Settings.Edge;

namespace SolidCP.Providers.HostedSolution
{
    public class Lync2013 : LyncBase
    {
        #region Constructor

        static Lync2013()
        {
            LyncRegistryPath = "SOFTWARE\\Microsoft\\Real-Time Communications";
            LyncVersion = "5";
        }

        #endregion

        #region Methods

        #region Organizations

        /// <summary> Creates organization. </summary>
        /// <param name="organizationId"> The organization identifier. </param>
        /// <param name="sipDomain"> The sip domain. </param>
        /// <param name="enableConferencingVideo"> True - if conferencing video should be enabled.</param>
        /// <param name="maxConferenceSize"> The max conference size.</param>
        /// <param name="enabledFederation"> True - if federations should be enabled.</param>
        /// <param name="enabledEnterpriseVoice"> True - if enterprise voice should be enabled.</param>
        /// <returns> The tenant identifier. </returns>
        internal override string CreateOrganizationInternal(string organizationId, string sipDomain, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            sipDomain = sipDomain.ToLower();
            HostedSolutionLog.LogStart("CreateOrganizationInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("sipDomain: {0}", sipDomain);
            string tenantId;
            LyncTransaction transaction = StartTransaction();
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                var command = new Command("New-CsSipDomain");
                command.Parameters.Add("Identity", sipDomain);
                ExecuteShellCommand(runspace, command, false);
                transaction.RegisterNewSipDomain(sipDomain);
                Guid id = Guid.NewGuid();

                AddAdDomainName(organizationId, sipDomain);

                CreateSimpleUrl(runspace, id);
                transaction.RegisterNewSimpleUrl(sipDomain, id.ToString());

                command = new Command("New-CsConferencingPolicy");
                command.Parameters.Add("Identity", organizationId);
                command.Parameters.Add("MaxMeetingSize", ((maxConferenceSize == -1) | (maxConferenceSize > 250)) ? 250 : maxConferenceSize);
                command.Parameters.Add("AllowIPVideo", enableConferencingVideo);
                ExecuteShellCommand(runspace, command, false);
                transaction.RegisterNewConferencingPolicy(organizationId);

                command = new Command("New-CsExternalAccessPolicy");
                command.Parameters.Add("Identity", organizationId);
                command.Parameters.Add("EnableFederationAccess", true);
                command.Parameters.Add("EnableOutsideAccess", true);
                command.Parameters.Add("EnablePublicCloudAccess", false);
                command.Parameters.Add("EnablePublicCloudAudioVideoAccess", false);
                ExecuteShellCommand(runspace, command, false);
                transaction.RegisterNewCsExternalAccessPolicy(organizationId);

                var allowList = new AllowList();
                var domain = new DomainPattern(sipDomain);
                allowList.AllowedDomain.Add(domain);

                AddFederationDomainInternal("", domain.Domain, PoolFQDN);

                command = new Command("New-CsMobilityPolicy");
                command.Parameters.Add("Identity", organizationId + " EnableOutSideVoice");
                command.Parameters.Add("EnableMobility", true);
                command.Parameters.Add("EnableOutsideVoice", true);
                ExecuteShellCommand(runspace, command, false);
                transaction.RegisterNewCsMobilityPolicy(organizationId + " EnableOutSideVoice");

                command = new Command("New-CsMobilityPolicy");
                command.Parameters.Add("Identity", organizationId + " DisableOutSideVoice");
                command.Parameters.Add("EnableMobility", true);
                command.Parameters.Add("EnableOutsideVoice", false);
                ExecuteShellCommand(runspace, command, false);
                transaction.RegisterNewCsMobilityPolicy(organizationId + " DisableOutSideVoice");

                command = new Command("Invoke-CsManagementStoreReplication");
                ExecuteShellCommand(runspace, command, false);

                tenantId = id.ToString();
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("CreateOrganizationInternal", ex);
                RollbackTransaction(transaction);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("CreateOrganizationInternal");

            return tenantId;
        }

        /// <summary> Deletes organization.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="sipDomain"> The sip domain.</param>
        /// <returns> The result.</returns>
        internal override bool DeleteOrganizationInternal(string organizationId, string sipDomain)
        {
            HostedSolutionLog.LogStart("DeleteOrganizationInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("sipDomain: {0}", sipDomain);
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                string path = AddADPrefix(GetOrganizationPath(organizationId));
                DirectoryEntry ou = ActiveDirectoryUtils.GetADObject(path);
                string[] sipDs = ActiveDirectoryUtils.GetADObjectPropertyMultiValue(ou, "Url");

                foreach (string sipD in sipDs)
                {
                    DeleteSipDomain(runspace, sipD);
                }

                try
                {
                    DeleteConferencingPolicy(runspace, organizationId);
                }
                catch (Exception)
                {
                }

                try
                {
                    DeleteExternalAccessPolicy(runspace, organizationId);
                }
                catch (Exception)
                {
                }

                try
                {
                    DeleteMobilityPolicy(runspace, organizationId + " EnableOutSideVoice");
                }
                catch (Exception)
                {
                }

                try
                {
                    DeleteMobilityPolicy(runspace, organizationId + " DisableOutSideVoice");
                }
                catch (Exception)
                {
                }

                var command = new Command("Invoke-CsManagementStoreReplication");
                ExecuteShellCommand(runspace, command, false);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("DeleteOrganizationInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("DeleteOrganizationInternal");

            return true;
        }

        #endregion

        #region Users

        /// <summary> Creates the user.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="userUpn"> The user UPN.</param>
        /// <param name="plan"> The Lync user plan.</param>
        /// <returns> The result.</returns>
        internal override bool CreateUserInternal(string organizationId, string userUpn, LyncUserPlan plan)
        {
            HostedSolutionLog.LogStart("CreateUserInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);
            LyncTransaction transaction = StartTransaction();
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                Guid guid = GetObjectGuid(organizationId, runspace);
                string[] tmp = userUpn.Split('@');

                if (tmp.Length < 2)
                {
                    return false;
                }

                var command = new Command("Get-CsSipDomain");
                Collection<PSObject> sipDomains = ExecuteShellCommand(runspace, command, false);
                bool bSipDomainExists = sipDomains.Select(domain => (string) GetPSObjectProperty(domain, "Name")).Any(d => d.ToLower() == tmp[1].ToLower());

                string path = string.Empty;
                if (!bSipDomainExists)
                {
                    command = new Command("New-CsSipDomain");
                    command.Parameters.Add("Identity", tmp[1].ToLower());
                    ExecuteShellCommand(runspace, command, false);
                    transaction.RegisterNewSipDomain(tmp[1].ToLower());
                    AddAdDomainName(organizationId, tmp[1].ToLower());
                    CreateSimpleUrl(runspace, guid);
                    transaction.RegisterNewSimpleUrl(tmp[1].ToLower(), guid.ToString());
                }

                command = new Command("Enable-CsUser");
                command.Parameters.Add("Identity", userUpn);
                command.Parameters.Add("RegistrarPool", PoolFQDN);
                command.Parameters.Add("SipAddressType", "UserPrincipalName");
                ExecuteShellCommand(runspace, command, false);
                transaction.RegisterNewCsUser(userUpn);

                command = new Command("Get-CsAdUser");
                command.Parameters.Add("Identity", userUpn);
                Collection<PSObject> result = ExecuteShellCommand(runspace, command, false);

                //set groupingID
                path = AddADPrefix(GetResultObjectDN(result));
                DirectoryEntry user = ActiveDirectoryUtils.GetADObject(path);
                ActiveDirectoryUtils.SetADObjectPropertyValue(user, "msRTCSIP-GroupingID", guid);
                user.CommitChanges();

                command = new Command("Update-CsAddressBook");
                ExecuteShellCommand(runspace, command, false);
                command = new Command("Update-CsUserDatabase");
                ExecuteShellCommand(runspace, command, false);

                int trySleep = 2000; int tryMaxCount = 10; bool PlanSet = false;
                for (int tryCount = 0; (tryCount < tryMaxCount) && (!PlanSet); tryCount++ )
                {
                    try
                    {
                        PlanSet = SetLyncUserPlanInternal(organizationId, userUpn, plan, runspace);
                    }
                    catch { }
                    if (!PlanSet) System.Threading.Thread.Sleep(trySleep);
                }

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("CreateUserInternal", ex);
                RollbackTransaction(transaction);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("CreateUserInternal");

            return true;
        }

        /// <summary> Gets users general settings.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="userUpn"> The user UPN.</param>
        /// <returns> User settings.</returns>
        internal override LyncUser GetLyncUserGeneralSettingsInternal(string organizationId, string userUpn)
        {
            HostedSolutionLog.LogStart("GetLyncUserGeneralSettingsInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);
            var lyncUser = new LyncUser();
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                var command = new Command("Get-CsUser");
                command.Parameters.Add("Identity", userUpn);
                Collection<PSObject> result = ExecuteShellCommand(runspace, command, false);
                PSObject user = result[0];

                lyncUser.DisplayName = (string) GetPSObjectProperty(user, "DisplayName");
                lyncUser.SipAddress = (string) GetPSObjectProperty(user, "SipAddress");
                lyncUser.LineUri = (string) GetPSObjectProperty(user, "LineURI");

                lyncUser.SipAddress = lyncUser.SipAddress.ToLower().Replace("sip:", "");
                lyncUser.LineUri = lyncUser.LineUri.ToLower().Replace("tel:+", "");
                lyncUser.LineUri = lyncUser.LineUri.ToLower().Replace("tel:", "");
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetLyncUserGeneralSettingsInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("GetLyncUserGeneralSettingsInternal");

            return lyncUser;
        }

        /// <summary> Sets users general settings.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="userUpn"> The user UPN.</param>
        /// <param name="lyncUser"> The lync user settings.</param>
        /// <returns> The result.</returns>
        internal override bool SetLyncUserGeneralSettingsInternal(string organizationId, string userUpn, LyncUser lyncUser)
        {
            HostedSolutionLog.LogStart("SetLyncUserGeneralSettingsInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);

            bool ret = true;
            Runspace runspace = null;
            LyncTransaction transaction = StartTransaction();

            try
            {
                runspace = OpenRunspace();
                Guid tenantId = GetObjectGuid(organizationId, runspace);
                string[] tmp = userUpn.Split('@');

                if (tmp.Length < 2)
                {
                    return false;
                }

                var command = new Command("Get-CsSipDomain");
                Collection<PSObject> sipDomains = ExecuteShellCommand(runspace, command, false);
                bool bSipDomainExists = sipDomains.Select(domain => (string) GetPSObjectProperty(domain, "Name")).Any(d => d.ToLower() == tmp[1].ToLower());

                if (!bSipDomainExists)
                {
                    command = new Command("New-CsSipDomain");
                    command.Parameters.Add("Identity", tmp[1].ToLower());
                    ExecuteShellCommand(runspace, command, false);

                    transaction.RegisterNewSipDomain(tmp[1].ToLower());

                    string path = AddADPrefix(GetOrganizationPath(organizationId));
                    DirectoryEntry ou = ActiveDirectoryUtils.GetADObject(path);
                    string[] sipDs = ActiveDirectoryUtils.GetADObjectPropertyMultiValue(ou, "Url");
                    var listSipDs = new List<string>();
                    listSipDs.AddRange(sipDs);
                    listSipDs.Add(tmp[1]);

                    ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "Url", listSipDs.ToArray());
                    ou.CommitChanges();

                    CreateSimpleUrl(runspace, tenantId);
                    transaction.RegisterNewSimpleUrl(tmp[1].ToLower(), tenantId.ToString());

                    path = AddADPrefix(GetResultObjectDN(organizationId, runspace));
                    DirectoryEntry user = ActiveDirectoryUtils.GetADObject(path);

                    if (tmp.Length > 0)
                    {
                        string Url = SimpleUrlRoot + tmp[1];
                        ActiveDirectoryUtils.SetADObjectPropertyValue(user, "msRTCSIP-BaseSimpleUrl", Url.ToLower());
                    }

                    user.CommitChanges();
                }

                command = new Command("Set-CsUser");
                command.Parameters.Add("Identity", userUpn);

                if (!string.IsNullOrEmpty(lyncUser.SipAddress))
                {
                    command.Parameters.Add("SipAddress", "SIP:" + lyncUser.SipAddress);
                }

                if (!string.IsNullOrEmpty(lyncUser.LineUri))
                {
                    command.Parameters.Add("LineUri", "TEL:+" + lyncUser.LineUri);
                }
                else
                {
                    command.Parameters.Add("LineUri", null);
                }

                ExecuteShellCommand(runspace, command, false);

                if (!String.IsNullOrEmpty(lyncUser.PIN))
                {
                    command = new Command("Set-CsClientPin");
                    command.Parameters.Add("Identity", userUpn);
                    command.Parameters.Add("Pin", lyncUser.PIN);
                    ExecuteShellCommand(runspace, command, false);
                }

                command = new Command("Update-CsAddressBook");
                ExecuteShellCommand(runspace, command, false);

                command = new Command("Update-CsUserDatabase");
                ExecuteShellCommand(runspace, command, false);
            }
            catch (Exception ex)
            {
                ret = false;
                HostedSolutionLog.LogError("SetLyncUserGeneralSettingsInternal", ex);
                RollbackTransaction(transaction);
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("SetLyncUserGeneralSettingsInternal");

            return ret;
        }

        /// <summary> Sets users lync plan.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="userUpn"> The user UPN.</param>
        /// <param name="plan"> The lync plan.</param>
        /// <param name="runspace"> The runspace.</param>
        /// <returns> The result.</returns>
        internal override bool SetLyncUserPlanInternal(string organizationId, string userUpn, LyncUserPlan plan, Runspace runspace)
        {
            HostedSolutionLog.LogStart("SetLyncUserPlanInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);
            bool bCloseRunSpace = false;
            bool ret = true;

            try
            {
                if (runspace == null)
                {
                    runspace = OpenRunspace();
                    bCloseRunSpace = true;
                }

                // EnterpriseVoice
                var command = new Command("Set-CsUser");
                command.Parameters.Add("Identity", userUpn);
                command.Parameters.Add("EnterpriseVoiceEnabled", plan.EnterpriseVoice);
                ExecuteShellCommand(runspace, command, false);

                command = new Command("Grant-CsExternalAccessPolicy");
                command.Parameters.Add("Identity", userUpn);
                command.Parameters.Add("PolicyName", plan.Federation ? organizationId : null);
                ExecuteShellCommand(runspace, command, false);

                command = new Command("Grant-CsConferencingPolicy");
                command.Parameters.Add("Identity", userUpn);
                command.Parameters.Add("PolicyName", plan.Federation ? organizationId : null);
                ExecuteShellCommand(runspace, command, false);

                command = new Command("Grant-CsMobilityPolicy");
                command.Parameters.Add("Identity", userUpn);
                if (plan.Mobility)
                {
                    command.Parameters.Add("PolicyName", plan.MobilityEnableOutsideVoice ? organizationId + " EnableOutSideVoice" : organizationId + " DisableOutSideVoice");
                }
                else
                {
                    command.Parameters.Add("PolicyName", null);
                }
                ExecuteShellCommand(runspace, command, false);

                // ArchivePolicy
                command = new Command("Grant-CsArchivingPolicy");
                command.Parameters.Add("Identity", userUpn);
                command.Parameters.Add("PolicyName", string.IsNullOrEmpty(plan.ArchivePolicy) ? null : plan.ArchivePolicy);
                ExecuteShellCommand(runspace, command, false);

                // DialPlan
                command = new Command("Grant-CsDialPlan");
                command.Parameters.Add("Identity", userUpn);
                command.Parameters.Add("PolicyName", string.IsNullOrEmpty(plan.TelephonyDialPlanPolicy) ? null : plan.TelephonyDialPlanPolicy);
                ExecuteShellCommand(runspace, command, false);

                // VoicePolicy
                command = new Command("Grant-CsVoicePolicy");
                command.Parameters.Add("Identity", userUpn);
                command.Parameters.Add("PolicyName", string.IsNullOrEmpty(plan.TelephonyVoicePolicy) ? null : plan.TelephonyVoicePolicy);
                ExecuteShellCommand(runspace, command, false);

                command = new Command("Update-CsUserDatabase");
                ExecuteShellCommand(runspace, command, false);

                ret = false;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("SetLyncUserPlanInternal", ex);
                throw;
            }
            finally
            {
                if (bCloseRunSpace)
                {
                    CloseRunspace(runspace);
                }
            }

            HostedSolutionLog.LogEnd("SetLyncUserPlanInternal");

            return ret;
        }

        /// <summary> Deletes user.</summary>
        /// <param name="userUpn"> The user UPN.</param>
        /// <returns> The result.</returns>
        internal override bool DeleteUserInternal(string userUpn)
        {
            HostedSolutionLog.LogStart("DeleteUserInternal");
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                DeleteUser(runspace, userUpn);

                var command = new Command("Get-CsAdUser");
                command.Parameters.Add("Identity", userUpn);
                ExecuteShellCommand(runspace, command, false);

                command = new Command("Update-CsAddressBook");
                ExecuteShellCommand(runspace, command, false);

                command = new Command("Update-CsUserDatabase");
                ExecuteShellCommand(runspace, command, false);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("DeleteUserInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("DeleteUserInternal");

            return true;
        }

        #endregion

        #region Sytsem Related Methods

        /// <summary> Refreshes configuration.</summary>
        internal override void ReloadConfigurationInternal()
        {
            HostedSolutionLog.LogStart("ReloadConfigurationInternal");
            Runspace runspace = null;
            try
            {
                runspace = OpenRunspace();

                var command = new Command("Enable-CsComputer");
                ExecuteShellCommand(runspace, command, false);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("ReloadConfigurationInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }
            HostedSolutionLog.LogEnd("ReloadConfigurationInternal");
        }

        #endregion

        #region Federation Domains

        /// <summary> Gets allowed domains.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <returns> Allowed domains.</returns>
        internal override LyncFederationDomain[] GetFederationDomainsInternal(string organizationId)
        {
            HostedSolutionLog.LogStart("GetFederationDomainsInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            LyncFederationDomain[] domains;
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                domains = GetFederationDomainsInternal(runspace);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetFederationDomainsInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("GetFederationDomainsInternal");

            return domains;
        }

        /// <summary> Gets allowed domains. </summary>
        /// <param name="runspace">The runspace.</param>
        /// <returns> Allowed domains.</returns>
        private LyncFederationDomain[] GetFederationDomainsInternal(Runspace runspace)
        {
            var domains = new List<LyncFederationDomain>();
            var command = new Command("Get-CsAllowedDomain");
            Collection<PSObject> result = ExecuteShellCommand(runspace, command, false);

            if ((result != null) && (result.Count > 0))
            {
                domains = result.Select(psObject => new LyncFederationDomain {DomainName = psObject.Properties["Domain"].Value.ToString()}).ToList();
            }

            return domains.ToArray();
        }

        /// <summary> Adds domain to allowed list.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="domainName"> The domain name.</param>
        /// <param name="proxyFqdn"> The ProxyFQDN.</param>
        /// <returns> The result.</returns>
        internal override bool AddFederationDomainInternal(string organizationId, string domainName, string proxyFqdn)
        {
            domainName = domainName.ToLower();
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                var command = new Command("Get-CsAllowedDomain");
                command.Parameters.Add("Identity", domainName);
                Collection<PSObject> result = ExecuteShellCommand(runspace, command, false);

                if (result != null && !result.Any())
                {
                    command = new Command("New-CsAllowedDomain");
                    command.Parameters.Add("Identity", domainName);
                    ExecuteShellCommand(runspace, command, false);

                    command = new Command("Set-CsAllowedDomain");
                    command.Parameters.Add("Identity", domainName);
                    command.Parameters.Add("ProxyFQDN", PoolFQDN);
                    ExecuteShellCommand(runspace, command, false);
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("AddFederationDomainInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("AddFederationDomainInternal");

            return true;
        }

        /// <summary> Removes domain from allowed list.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="domainName"> The domain name.</param>
        /// <returns> The result.</returns>
        internal override bool RemoveFederationDomainInternal(string organizationId, string domainName)
        {
            HostedSolutionLog.LogStart("RemoveFederationDomainInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("domainName: {0}", domainName);
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                var command = new Command("Remove-CsAllowedDomain");
                command.Parameters.Add("Identity", domainName);
                ExecuteShellCommand(runspace, command, false);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("RemoveFederationDomainInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("RemoveFederationDomainInternal");

            return true;
        }

        /// <summary> Gets organization lync identifier.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="runspace"> The runspace.</param>
        /// <returns> Organization lync identifier.</returns>
        private Guid GetObjectGuid(string organizationId, Runspace runspace)
        {
            string path = GetOrganizationPath(organizationId);
            var scripts = new List<string> {string.Format("Get-ADOrganizationalUnit -Identity \"{0}\"", path)};
            Collection<PSObject> result = ExecuteShellCommand(runspace, scripts);

            if (result != null && result.Any())
            {
                return new Guid(result.First().Properties["ObjectGuid"].Value.ToString());
            }

            return Guid.NewGuid();
        }

        /// <summary> Gets organization distinguished name.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="runspace"> The runspace.</param>
        /// <returns> The distinguished name.</returns>
        private string GetResultObjectDN(string organizationId, Runspace runspace)
        {
            HostedSolutionLog.LogStart("GetResultObjectDN");

            string path = GetOrganizationPath(organizationId);
            var scripts = new List<string> {string.Format("Get-ADOrganizationalUnit -Identity \"{0}\"", path)};
            Collection<PSObject> result = ExecuteShellCommand(runspace, scripts);

            if (result != null && result.Any())
            {
                return result.First().Properties["DistinguishedName"].Value.ToString();
            }

            throw new ArgumentException("Execution result does not contain DistinguishedName property");
        }

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

        /// <summary> Adds domain to AD.</summary>
        /// <param name="organizationId"> The organization identifier.</param>
        /// <param name="domainName"> The domain name.</param>
        private void AddAdDomainName(string organizationId, string domainName)
        {
            string path = AddADPrefix(GetOrganizationPath(organizationId));
            DirectoryEntry ou = ActiveDirectoryUtils.GetADObject(path);
            ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "Url", new[] {domainName});
            ou.CommitChanges();
        }

        #endregion

        #region Policy

        internal override string[] GetPolicyListInternal(LyncPolicyType type, string name)
        {
            List<string> ret = new List<string>();

            switch (type)
            {
                case LyncPolicyType.Archiving:
                    {
                        Runspace runSpace = OpenRunspace();
                        Command cmd = new Command("Get-CsArchivingPolicy");
                        Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                        if ((result != null) && (result.Count > 0))
                        {
                            foreach (PSObject res in result)
                            {
                                string Identity = GetPSObjectProperty(res, "Identity").ToString();
                                ret.Add(Identity);
                            }
                        }
                    }
                    break;
                case LyncPolicyType.DialPlan:
                    {
                        Runspace runSpace = OpenRunspace();
                        Command cmd = new Command("Get-CsDialPlan");
                        Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                        if ((result != null) && (result.Count > 0))
                        {
                            foreach (PSObject res in result)
                            {
                                string Identity = GetPSObjectProperty(res, "Identity").ToString();
                                string Description = "" + (string)GetPSObjectProperty(res, "Description");
                                if (Description.ToLower().IndexOf(name.ToLower()) == -1) continue;
                                ret.Add(Identity);
                            }


                        }
                    }
                    break;
                case LyncPolicyType.Voice:
                    {
                        Runspace runSpace = OpenRunspace();
                        Command cmd = new Command("Get-CsVoicePolicy");
                        Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                        if ((result != null) && (result.Count > 0))
                        {
                            foreach (PSObject res in result)
                            {
                                string Identity = GetPSObjectProperty(res, "Identity").ToString();
                                string Description = "" + (string)GetPSObjectProperty(res, "Description");
                                if (Description.ToLower().IndexOf(name.ToLower()) == -1) continue;

                                ret.Add(Identity);
                            }


                        }
                    }
                    break;
                case LyncPolicyType.Pin:
                    {
                        Runspace runSpace = OpenRunspace();
                        Command cmd = new Command("Get-CsPinPolicy");
                        Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                        if ((result != null) && (result.Count > 0))
                        {
                            foreach (PSObject res in result)
                            {
                                string Identity = GetPSObjectProperty(res, "Identity").ToString();
                                string str = "" + GetPSObjectProperty(res, name);
                                ret.Add(str);
                            }
                        }
                    }
                    break;

            }

            return ret.ToArray();
        }

        #endregion

        #endregion
    }
}
