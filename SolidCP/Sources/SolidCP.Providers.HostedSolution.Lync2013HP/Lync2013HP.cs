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
using System.DirectoryServices;

using Microsoft.Win32;

using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

//using Microsoft.Rtc.Management.Hosted;
using Microsoft.Rtc.Management.WritableConfig.Settings.Edge;
using Microsoft.Rtc.Management.WritableConfig.Settings.SimpleUrl;


namespace SolidCP.Providers.HostedSolution
{
    public class Lync2013HP : HostingServiceProviderBase, ILyncServer
    {

        #region Static constructor
        static Lync2013HP()
        {
            LyncRegistryPath = "SOFTWARE\\Microsoft\\Real-Time Communications";
        }

        public static string LyncRegistryPath
        {
            get;
            set;
        }

        #endregion


        #region Properties

        /// <summary>
        /// Pool FQDN
        /// </summary>
        private string PoolFQDN
        {
            get { return ProviderSettings[LyncConstants.PoolFQDN]; }
        }

        private string SimpleUrlRoot
        {
            get { return ProviderSettings[LyncConstants.SimpleUrlRoot]; }
        }

        internal string PrimaryDomainController
        {
            get { return ProviderSettings["PrimaryDomainController"]; }
        }

        private string RootOU
        {
            get { return ProviderSettings["RootOU"]; }
        }

        private string RootDomain
        {
            get { return ServerSettings.ADRootDomain; }
        }


        #endregion

        #region ILyncServer implementation

        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return CreateOrganizationInternal(organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public string GetOrganizationTenantId(string organizationId)
        {
            return GetOrganizationTenantIdInternal(organizationId);
        }

        public bool DeleteOrganization(string organizationId, string sipDomain)
        {
            return DeleteOrganizationInternal(organizationId, sipDomain);
        }

        public bool CreateUser(string organizationId, string userUpn, LyncUserPlan plan)
        {
            return CreateUserInternal(organizationId, userUpn, plan);
        }

        public LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn)
        {
            return GetLyncUserGeneralSettingsInternal(organizationId, userUpn);
        }

        public bool SetLyncUserGeneralSettings(string organizationId, string userUpn, LyncUser lyncUser)
        {
            return SetLyncUserGeneralSettingsInternal(organizationId, userUpn, lyncUser);
        }


        public bool SetLyncUserPlan(string organizationId, string userUpn, LyncUserPlan plan)
        {
            return SetLyncUserPlanInternal(organizationId, userUpn, plan, null);
        }

        public bool DeleteUser(string userUpn)
        {
            return DeleteUserInternal(userUpn);
        }

        public LyncFederationDomain[] GetFederationDomains(string organizationId)
        {
            return GetFederationDomainsInternal(organizationId);
        }

        public bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            return AddFederationDomainInternal(organizationId, domainName, proxyFqdn);
        }

        public bool RemoveFederationDomain(string organizationId, string domainName)
        {
            return RemoveFederationDomainInternal(organizationId, domainName);
        }

        public void ReloadConfiguration()
        {
            ReloadConfigurationInternal();
        }

        public string[] GetPolicyList(LyncPolicyType type, string name)
        {
            return GetPolicyListInternal(type, name);
        }

        #endregion

        #region organization
        private string CreateOrganizationInternal(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            sipDomain = sipDomain.ToLower();
            HostedSolutionLog.LogStart("CreateOrganizationInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("sipDomain: {0}", sipDomain);

            string TenantId = string.Empty;

            LyncTransaction transaction = StartTransaction();

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                // create sip domain
                Command cmd = new Command("New-CsSipDomain");
                cmd.Parameters.Add("Identity", sipDomain);
                ExecuteShellCommand(runSpace, cmd, false);

                transaction.RegisterNewSipDomain(sipDomain);

                //set the msRTCSIP-Domains, TenantID, ObjectID
                Guid id = Guid.NewGuid();

                string path = AddADPrefix(GetOrganizationPath(organizationId));
                DirectoryEntry ou = ActiveDirectoryUtils.GetADObject(path);
                ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "msRTCSIP-Domains", sipDomain);
                ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "msRTCSIP-TenantId", id);
                ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "msRTCSIP-ObjectId", id);
                ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "msRTCSIP-DomainUrlMap", sipDomain + "#" + SimpleUrlRoot+sipDomain);
                ou.CommitChanges();

                //Create simpleurls
                CreateSimpleUrl(runSpace, sipDomain, id);
                transaction.RegisterNewSimpleUrl(sipDomain, id.ToString());

                //create conferencing policy
                cmd = new Command("New-CsConferencingPolicy");
                cmd.Parameters.Add("Identity", organizationId);

                cmd.Parameters.Add("MaxMeetingSize", ((maxConferenceSize == -1) | (maxConferenceSize > 250)) ? 250 : maxConferenceSize);
                cmd.Parameters.Add("AllowIPVideo", enableConferencingVideo);
                ExecuteShellCommand(runSpace, cmd, false);
                transaction.RegisterNewConferencingPolicy(organizationId);

                //create external access policy
                cmd = new Command("New-CsExternalAccessPolicy");
                cmd.Parameters.Add("Identity", organizationId);
                cmd.Parameters.Add("EnableFederationAccess", true);
                cmd.Parameters.Add("EnableOutsideAccess", true);
                cmd.Parameters.Add("EnablePublicCloudAccess", false);
                cmd.Parameters.Add("EnablePublicCloudAudioVideoAccess", false);
                ExecuteShellCommand(runSpace, cmd, false);
                transaction.RegisterNewCsExternalAccessPolicy(organizationId);

                //Enable for federation
                AllowList allowList = new AllowList();
                DomainPattern domain = new DomainPattern(sipDomain);
                allowList.AllowedDomain.Add(domain);

                cmd = new Command("Set-CsTenantFederationConfiguration");
                cmd.Parameters.Add("Tenant", id);
                cmd.Parameters.Add("AllowFederatedUsers", true);
                cmd.Parameters.Add("AllowedDomains", allowList);
                ExecuteShellCommand(runSpace, cmd, false);

                //create mobility policy
                cmd = new Command("New-CsMobilityPolicy");
                cmd.Parameters.Add("Identity", organizationId + " EnableOutSideVoice");
                cmd.Parameters.Add("EnableMobility", true);
                cmd.Parameters.Add("EnableOutsideVoice", true);
                ExecuteShellCommand(runSpace, cmd, false);
                transaction.RegisterNewCsMobilityPolicy(organizationId + " EnableOutSideVoice");

                cmd = new Command("New-CsMobilityPolicy");
                cmd.Parameters.Add("Identity", organizationId + " DisableOutSideVoice");
                cmd.Parameters.Add("EnableMobility", true);
                cmd.Parameters.Add("EnableOutsideVoice", false);
                ExecuteShellCommand(runSpace, cmd, false);
                transaction.RegisterNewCsMobilityPolicy(organizationId + " DisableOutSideVoice");

                cmd = new Command("Invoke-CsManagementStoreReplication");
                ExecuteShellCommand(runSpace, cmd, false);


                TenantId = id.ToString();
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("CreateOrganizationInternal", ex);
                RollbackTransaction(transaction);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }

            HostedSolutionLog.LogEnd("CreateOrganizationInternal");

            return TenantId;
        }

        private string GetOrganizationTenantIdInternal(string organizationId)
        {
            HostedSolutionLog.LogStart("GetOrganizationTenantIdInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);

            string tenantIdStr = string.Empty;

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-CsTenant");
                cmd.Parameters.Add("Identity", GetOrganizationPath(organizationId));
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                if ((result != null) && (result.Count > 0))
                {
                    Guid tenantId = (Guid)GetPSObjectProperty(result[0], "TenantId");

                    tenantIdStr = tenantId.ToString();
                }

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetOrganizationTenantIdInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("GetOrganizationTenantIdnInternal");
            return tenantIdStr;
        }

        private bool DeleteOrganizationInternal(string organizationId, string sipDomain)
        {
            HostedSolutionLog.LogStart("DeleteOrganizationInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("sipDomain: {0}", sipDomain);

            bool ret = true;

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-CsTenant");
                cmd.Parameters.Add("Identity", GetOrganizationPath(organizationId));
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                if ((result != null) && (result.Count > 0))
                {
                    Guid tenantId = (Guid)GetPSObjectProperty(result[0], "TenantId");

                    // create sip domain
                    string path = AddADPrefix(GetOrganizationPath(organizationId));
                    DirectoryEntry ou = ActiveDirectoryUtils.GetADObject(path);
                    string[] sipDs = (string[])ActiveDirectoryUtils.GetADObjectPropertyMultiValue(ou, "msRTCSIP-Domains");

                    foreach (string sipD in sipDs)
                        DeleteSipDomain(runSpace, sipD);

                    //clear the msRTCSIP-Domains, TenantID, ObjectID
                    ActiveDirectoryUtils.ClearADObjectPropertyValue(ou, "msRTCSIP-Domains");
                    ActiveDirectoryUtils.ClearADObjectPropertyValue(ou, "msRTCSIP-TenantId");
                    ActiveDirectoryUtils.ClearADObjectPropertyValue(ou, "msRTCSIP-ObjectId");
                    ou.CommitChanges();

                    try
                    {
                        DeleteConferencingPolicy(runSpace, organizationId);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        DeleteExternalAccessPolicy(runSpace, organizationId);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        DeleteMobilityPolicy(runSpace, organizationId + " EnableOutSideVoice");
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        DeleteMobilityPolicy(runSpace, organizationId + " DisableOutSideVoice");
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        DeleteSimpleUrl(runSpace, sipDomain, tenantId);
                    }
                    catch (Exception)
                    {
                    }
                }

                cmd = new Command("Invoke-CsManagementStoreReplication");
                ExecuteShellCommand(runSpace, cmd, false);

            }
            catch (Exception ex)
            {
                ret = false;
                HostedSolutionLog.LogError("DeleteOrganizationInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("DeleteOrganizationInternal");
            return ret;
        }
        #endregion

        #region Users
        private bool CreateUserInternal(string organizationId, string userUpn, LyncUserPlan plan)
        {
            HostedSolutionLog.LogStart("CreateUserInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);

            bool ret = true;
            Guid tenantId = Guid.Empty;

            LyncTransaction transaction = StartTransaction();

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-CsTenant");
                cmd.Parameters.Add("Identity", GetOrganizationPath(organizationId));
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                if ((result != null) && (result.Count > 0))
                {
                    tenantId = (Guid)GetPSObjectProperty(result[0], "TenantId");

                    string[] tmp = userUpn.Split('@');
                    if (tmp.Length < 2) return false;

                    // Get SipDomains and verify existence
                    bool bSipDomainExists = false;
                    cmd = new Command("Get-CsSipDomain");
                    Collection<PSObject> sipDomains = ExecuteShellCommand(runSpace, cmd, false);

                    foreach (PSObject domain in sipDomains)
                    {
                        string d = (string)GetPSObjectProperty(domain, "Name");
                        if (d.ToLower() == tmp[1].ToLower())
                        {
                            bSipDomainExists = true;
                            break;
                        }
                    }

                    string path = string.Empty;

                    if (!bSipDomainExists)
                    {
                        // Create Sip Domain
                        cmd = new Command("New-CsSipDomain");
                        cmd.Parameters.Add("Identity", tmp[1].ToLower());
                        ExecuteShellCommand(runSpace, cmd, false);

                        transaction.RegisterNewSipDomain(tmp[1].ToLower());


                        path = AddADPrefix(GetOrganizationPath(organizationId));
                        DirectoryEntry ou = ActiveDirectoryUtils.GetADObject(path);
                        string[] sipDs = (string[])ActiveDirectoryUtils.GetADObjectPropertyMultiValue(ou, "msRTCSIP-Domains");
                        List<string> listSipDs = new List<string>();
                        listSipDs.AddRange(sipDs);
                        listSipDs.Add(tmp[1]);

                        ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "msRTCSIP-Domains", listSipDs.ToArray());
                        ou.CommitChanges();

                        //Create simpleurls
                        CreateSimpleUrl(runSpace, tmp[1].ToLower(), tenantId);
                        transaction.RegisterNewSimpleUrl(tmp[1].ToLower(), tenantId.ToString());
                    }

                    //enable for lync
                    cmd = new Command("Enable-CsUser");
                    cmd.Parameters.Add("Identity", userUpn);
                    cmd.Parameters.Add("RegistrarPool", PoolFQDN);
                    cmd.Parameters.Add("SipAddressType", "UserPrincipalName");
                    ExecuteShellCommand(runSpace, cmd);

                    transaction.RegisterNewCsUser(userUpn);

                    //set groupingID and tenantID
                    cmd = new Command("Get-CsAdUser");
                    cmd.Parameters.Add("Identity", userUpn);
                    result = ExecuteShellCommand(runSpace, cmd);

                    path = AddADPrefix(GetResultObjectDN(result));
                    DirectoryEntry user = ActiveDirectoryUtils.GetADObject(path);
                    ActiveDirectoryUtils.SetADObjectPropertyValue(user, "msRTCSIP-GroupingID", tenantId);
                    ActiveDirectoryUtils.SetADObjectPropertyValue(user, "msRTCSIP-TenantId", tenantId);

                    if (tmp.Length > 0)
                    {
                        string Url = SimpleUrlRoot + tmp[1];
                        ActiveDirectoryUtils.SetADObjectPropertyValue(user, "msRTCSIP-BaseSimpleUrl", Url.ToLower());
                    }
                    user.CommitChanges();

                    int trySleep = 2000; int tryMaxCount = 10; bool PlanSet = false;
                    for (int tryCount = 0; (tryCount < tryMaxCount) && (!PlanSet); tryCount++)
                    {
                        try
                        {
                            PlanSet = SetLyncUserPlanInternal(organizationId, userUpn, plan, runSpace);
                        }
                        catch { }
                        if (!PlanSet) System.Threading.Thread.Sleep(trySleep);
                    }

                    //initiate addressbook generation
                    cmd = new Command("Update-CsAddressBook");
                    ExecuteShellCommand(runSpace, cmd, false);

                    //initiate user database replication
                    cmd = new Command("Update-CsUserDatabase");
                    ExecuteShellCommand(runSpace, cmd, false);



                }
                else
                {
                    ret = false;
                    HostedSolutionLog.LogError("Failed to retrieve tenantID", null);
                }
            }
            catch (Exception ex)
            {
                ret = false;
                HostedSolutionLog.LogError("CreateUserInternal", ex);
                RollbackTransaction(transaction);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("CreateUserInternal");
            return ret;
        }

        private LyncUser GetLyncUserGeneralSettingsInternal(string organizationId, string userUpn)
        {
            HostedSolutionLog.LogStart("GetLyncUserGeneralSettingsInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);

            LyncUser lyncUser = null;
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-CsUser");
                cmd.Parameters.Add("Identity", userUpn);
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

                if ((result != null) && (result.Count > 0))
                {
                    PSObject user = result[0];

                    lyncUser = new LyncUser();

                    lyncUser.DisplayName = (string)GetPSObjectProperty(user, "DisplayName");
                    lyncUser.SipAddress = (string)GetPSObjectProperty(user, "SipAddress");
                    lyncUser.LineUri = (string)GetPSObjectProperty(user, "LineURI");

                    lyncUser.SipAddress = lyncUser.SipAddress.ToLower().Replace("sip:", "");
                    lyncUser.LineUri = lyncUser.LineUri.ToLower().Replace("tel:+", "");
                    lyncUser.LineUri = lyncUser.LineUri.ToLower().Replace("tel:", "");
                }
                else
                    HostedSolutionLog.LogInfo("GetLyncUserGeneralSettingsInternal: No info found");

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetLyncUserGeneralSettingsInternal", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("GetLyncUserGeneralSettingsInternal");
            return lyncUser;
        }

        private bool SetLyncUserGeneralSettingsInternal(string organizationId, string userUpn, LyncUser lyncUser)
        {
            HostedSolutionLog.LogStart("SetLyncUserGeneralSettingsInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);

            bool ret = true;
            Runspace runSpace = null;
            Guid tenantId = Guid.Empty;
            LyncTransaction transaction = StartTransaction();

            try
            {
                runSpace = OpenRunspace();
                Command cmd = new Command("Get-CsTenant");
                cmd.Parameters.Add("Identity", GetOrganizationPath(organizationId));
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                if ((result != null) && (result.Count > 0))
                {
                    tenantId = (Guid)GetPSObjectProperty(result[0], "TenantId");

                    string[] tmp = userUpn.Split('@');
                    if (tmp.Length < 2) return false;

                    // Get SipDomains and verify existence
                    bool bSipDomainExists = false;
                    cmd = new Command("Get-CsSipDomain");
                    Collection<PSObject> sipDomains = ExecuteShellCommand(runSpace, cmd, false);

                    foreach (PSObject domain in sipDomains)
                    {
                        string d = (string)GetPSObjectProperty(domain, "Name");
                        if (d.ToLower() == tmp[1].ToLower())
                        {
                            bSipDomainExists = true;
                            break;
                        }
                    }

                    string path = string.Empty;

                    if (!bSipDomainExists)
                    {
                        // Create Sip Domain
                        cmd = new Command("New-CsSipDomain");
                        cmd.Parameters.Add("Identity", tmp[1].ToLower());
                        ExecuteShellCommand(runSpace, cmd, false);

                        transaction.RegisterNewSipDomain(tmp[1].ToLower());


                        path = AddADPrefix(GetOrganizationPath(organizationId));
                        DirectoryEntry ou = ActiveDirectoryUtils.GetADObject(path);
                        string[] sipDs = (string[])ActiveDirectoryUtils.GetADObjectPropertyMultiValue(ou, "msRTCSIP-Domains");
                        List<string> listSipDs = new List<string>();
                        listSipDs.AddRange(sipDs);
                        listSipDs.Add(tmp[1]);

                        ActiveDirectoryUtils.SetADObjectPropertyValue(ou, "msRTCSIP-Domains", listSipDs.ToArray());
                        ou.CommitChanges();

                        //Create simpleurls
                        CreateSimpleUrl(runSpace, tmp[1].ToLower(), tenantId);
                        transaction.RegisterNewSimpleUrl(tmp[1].ToLower(), tenantId.ToString());

                        path = AddADPrefix(GetResultObjectDN(result));
                        DirectoryEntry user = ActiveDirectoryUtils.GetADObject(path);

                        if (tmp.Length > 0)
                        {
                            string Url = SimpleUrlRoot + tmp[1];
                            ActiveDirectoryUtils.SetADObjectPropertyValue(user, "msRTCSIP-BaseSimpleUrl", Url.ToLower());
                        }
                        user.CommitChanges();
                    }
                }

                cmd = new Command("Set-CsUser");
                cmd.Parameters.Add("Identity", userUpn);
                if (!string.IsNullOrEmpty(lyncUser.SipAddress)) cmd.Parameters.Add("SipAddress", "SIP:" + lyncUser.SipAddress);
                if (!string.IsNullOrEmpty(lyncUser.LineUri)) cmd.Parameters.Add("LineUri", "TEL:+" + lyncUser.LineUri);
                else cmd.Parameters.Add("LineUri", null);
                ExecuteShellCommand(runSpace, cmd, false);

                if (!String.IsNullOrEmpty(lyncUser.PIN))
                {
                    cmd = new Command("Set-CsClientPin");
                    cmd.Parameters.Add("Identity", userUpn);
                    cmd.Parameters.Add("Pin", lyncUser.PIN);
                    ExecuteShellCommand(runSpace, cmd, false);
                }


                //initiate addressbook generation
                cmd = new Command("Update-CsAddressBook");
                ExecuteShellCommand(runSpace, cmd, false);

                //initiate user database replication
                cmd = new Command("Update-CsUserDatabase");
                ExecuteShellCommand(runSpace, cmd, false);

            }
            catch (Exception ex)
            {
                ret = false;
                HostedSolutionLog.LogError("SetLyncUserGeneralSettingsInternal", ex);
                RollbackTransaction(transaction);
            }
            finally
            {
                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("SetLyncUserGeneralSettingsInternal");
            return ret;
        }



        private bool SetLyncUserPlanInternal(string organizationId, string userUpn, LyncUserPlan plan, Runspace runSpace)
        {
            HostedSolutionLog.LogStart("SetLyncUserPlanInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);
            bool ret = true;
            bool bCloseRunSpace = false;

            try
            {
                if (runSpace == null)
                {
                    runSpace = OpenRunspace();
                    bCloseRunSpace = true;
                }

                // EnterpriseVoice
                Command cmd = new Command("Set-CsUser");
                cmd.Parameters.Add("Identity", userUpn);
                cmd.Parameters.Add("EnterpriseVoiceEnabled", plan.EnterpriseVoice);
                ExecuteShellCommand(runSpace, cmd, false);

                //CsExternalAccessPolicy
                cmd = new Command("Grant-CsExternalAccessPolicy");
                cmd.Parameters.Add("Identity", userUpn);
                cmd.Parameters.Add("PolicyName", plan.Federation ? organizationId : null);
                ExecuteShellCommand(runSpace, cmd);

                //CsConferencingPolicy
                cmd = new Command("Grant-CsConferencingPolicy");
                cmd.Parameters.Add("Identity", userUpn);
                cmd.Parameters.Add("PolicyName", plan.Federation ? organizationId : null);
                ExecuteShellCommand(runSpace, cmd);

                //CsMobilityPolicy
                cmd = new Command("Grant-CsMobilityPolicy");
                cmd.Parameters.Add("Identity", userUpn);
                if (plan.Mobility)
                    cmd.Parameters.Add("PolicyName", plan.MobilityEnableOutsideVoice ? organizationId + " EnableOutSideVoice" : organizationId + " DisableOutSideVoice");
                else
                    cmd.Parameters.Add("PolicyName", null);
                ExecuteShellCommand(runSpace, cmd);

                // ArchivePolicy
                cmd = new Command("Grant-CsArchivingPolicy");
                cmd.Parameters.Add("Identity", userUpn);
                cmd.Parameters.Add("PolicyName", string.IsNullOrEmpty(plan.ArchivePolicy) ? null : plan.ArchivePolicy);
                ExecuteShellCommand(runSpace, cmd);

                // DialPlan
                cmd = new Command("Grant-CsDialPlan");
                cmd.Parameters.Add("Identity", userUpn);
                cmd.Parameters.Add("PolicyName", string.IsNullOrEmpty(plan.TelephonyDialPlanPolicy) ? null : plan.TelephonyDialPlanPolicy);
                ExecuteShellCommand(runSpace, cmd);

                // VoicePolicy
                cmd = new Command("Grant-CsVoicePolicy");
                cmd.Parameters.Add("Identity", userUpn);
                cmd.Parameters.Add("PolicyName", string.IsNullOrEmpty(plan.TelephonyVoicePolicy) ? null : plan.TelephonyVoicePolicy);
                ExecuteShellCommand(runSpace, cmd);

                //initiate user database replication
                cmd = new Command("Update-CsUserDatabase");
                ExecuteShellCommand(runSpace, cmd, false);
            }
            catch (Exception ex)
            {
                ret = false;
                HostedSolutionLog.LogError("SetLyncUserPlanInternal", ex);
                throw;
            }
            finally
            {

                if (bCloseRunSpace) CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("SetLyncUserPlanInternal");
            return ret;
        }


        private bool DeleteUserInternal(string userUpn)
        {
            HostedSolutionLog.LogStart("DeleteUserInternal");
            HostedSolutionLog.DebugInfo("userUpn: {0}", userUpn);

            bool ret = true;

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                //Delete User
                DeleteUser(runSpace, userUpn);

                //Clear groupingID and tenantID
                Command cmd = new Command("Get-CsAdUser");
                cmd.Parameters.Add("Identity", userUpn);
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

                string path = AddADPrefix(GetResultObjectDN(result));
                DirectoryEntry user = ActiveDirectoryUtils.GetADObject(path);
                ActiveDirectoryUtils.ClearADObjectPropertyValue(user, "msRTCSIP-GroupingID");
                ActiveDirectoryUtils.ClearADObjectPropertyValue(user, "msRTCSIP-TenantId");
                ActiveDirectoryUtils.ClearADObjectPropertyValue(user, "msRTCSIP-BaseSimpleUrl");
                user.CommitChanges();

                //initiate addressbook generation
                cmd = new Command("Update-CsAddressBook");
                ExecuteShellCommand(runSpace, cmd, false);

                //initiate user database replication
                cmd = new Command("Update-CsUserDatabase");
                ExecuteShellCommand(runSpace, cmd, false);
            }
            catch (Exception ex)
            {
                ret = false;
                HostedSolutionLog.LogError("DeleteUserInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("DeleteUserInternal");
            return ret;
        }

        internal void DeleteUser(Runspace runSpace, string userUpn)
        {
            HostedSolutionLog.LogStart("DeleteUser");
            HostedSolutionLog.DebugInfo("userUpn : {0}", userUpn);
            Command cmd = new Command("Disable-CsUser");
            cmd.Parameters.Add("Identity", userUpn);
            cmd.Parameters.Add("Confirm", false);
            ExecuteShellCommand(runSpace, cmd, false);
            HostedSolutionLog.LogEnd("DeleteUser");
        }

        #endregion

        #region SipDomains
        internal void DeleteSipDomain(Runspace runSpace, string id)
        {
            HostedSolutionLog.LogStart("DeleteSipDomain");
            HostedSolutionLog.DebugInfo("SipDomain : {0}", id);
            Command cmd = new Command("Remove-CsSipDomain");
            cmd.Parameters.Add("Identity", id);
            cmd.Parameters.Add("Confirm", false);
            cmd.Parameters.Add("Force", true);
            ExecuteShellCommand(runSpace, cmd, false);
            HostedSolutionLog.LogEnd("DeleteSipDomain");
        }

        private void CreateSimpleUrl(Runspace runSpace, string sipDomain, Guid id)
        {
            //Create the simpleUrlEntry
            Command cmd = new Command("Get-CsSipDomain");
            Collection<PSObject> sipDomains = ExecuteShellCommand(runSpace, cmd, false);

            IList<PSObject> SimpleUrls = new List<PSObject>();


            foreach (PSObject domain in sipDomains)
            {
                string d = (string)GetPSObjectProperty(domain, "Name");
                string Url = SimpleUrlRoot + d;


                //Create the simpleUrlEntry
                cmd = new Command("New-CsSimpleUrlEntry");
                cmd.Parameters.Add("Url", Url);
                Collection<PSObject> simpleUrlEntry = ExecuteShellCommand(runSpace, cmd, false);

                //Create the simpleUrl
                cmd = new Command("New-CsSimpleUrl");
                cmd.Parameters.Add("Component", "meet");
                cmd.Parameters.Add("Domain", d);
                cmd.Parameters.Add("SimpleUrl", simpleUrlEntry[0]);
                cmd.Parameters.Add("ActiveUrl", Url);
                Collection<PSObject> simpleUrl = ExecuteShellCommand(runSpace, cmd, false);

                SimpleUrls.Add(simpleUrl[0]);
            }

            //PSListModifier
            cmd = new Command("Set-CsSimpleUrlConfiguration");
            cmd.Parameters.Add("Identity", "Global");
            cmd.Parameters.Add("Tenant", id);
            cmd.Parameters.Add("SimpleUrl", SimpleUrls);
            ExecuteShellCommand(runSpace, cmd, false);
        }


        internal void DeleteSimpleUrl(Runspace runSpace, string sipDomain, Guid id)
        {
            /*
            //build the url
            string Url = SimpleUrlRoot + sipDomain;

            //Create the simpleUrlEntry
            Command cmd = new Command("New-CsSimpleUrlEntry");
            cmd.Parameters.Add("Url", Url);
            Collection<PSObject> simpleUrlEntry = ExecuteShellCommand(runSpace, cmd, false);

            //Create the simpleUrl
            cmd = new Command("New-CsSimpleUrl");
            cmd.Parameters.Add("Component", "meet");
            cmd.Parameters.Add("Domain", sipDomain);
            cmd.Parameters.Add("SimpleUrl", simpleUrlEntry[0]);
            cmd.Parameters.Add("ActiveUrl", Url);
            Collection<PSObject> simpleUrl = ExecuteShellCommand(runSpace, cmd, false);

            //PSListModifier
            
            Hashtable properties = new Hashtable();
            properties.Add("Remove", simpleUrl[0]);
                        
            cmd = new Command("Set-CsSimpleUrlConfiguration");
            cmd.Parameters.Add("Identity", "Global");
            cmd.Parameters.Add("Tenant", id);
            cmd.Parameters.Add("SimpleUrl", properties);
            ExecuteShellCommand(runSpace, cmd, false);
            */
        }


        #endregion

        #region Policies

        internal void DeleteConferencingPolicy(Runspace runSpace, string policyName)
        {
            HostedSolutionLog.LogStart("DeleteConferencingPolicy");
            HostedSolutionLog.DebugInfo("policyName : {0}", policyName);
            Command cmd = new Command("Remove-CsConferencingPolicy");
            cmd.Parameters.Add("Identity", policyName);
            cmd.Parameters.Add("Confirm", false);
            cmd.Parameters.Add("Force", true);
            ExecuteShellCommand(runSpace, cmd, false);
            HostedSolutionLog.LogEnd("DeleteConferencingPolicy");
        }

        internal void DeleteExternalAccessPolicy(Runspace runSpace, string policyName)
        {
            HostedSolutionLog.LogStart("DeleteExternalAccessPolicy");
            HostedSolutionLog.DebugInfo("policyName : {0}", policyName);
            Command cmd = new Command("Remove-CsExternalAccessPolicy");
            cmd.Parameters.Add("Identity", policyName);
            cmd.Parameters.Add("Confirm", false);
            cmd.Parameters.Add("Force", true);
            ExecuteShellCommand(runSpace, cmd, false);
            HostedSolutionLog.LogEnd("DeleteExternalAccessPolicy");
        }

        internal void DeleteMobilityPolicy(Runspace runSpace, string policyName)
        {
            HostedSolutionLog.LogStart("DeleteMobilityPolicy");
            HostedSolutionLog.DebugInfo("policyName : {0}", policyName);
            Command cmd = new Command("Remove-CsMobilityPolicy");
            cmd.Parameters.Add("Identity", policyName);
            cmd.Parameters.Add("Confirm", false);
            cmd.Parameters.Add("Force", true);
            ExecuteShellCommand(runSpace, cmd, false);
            HostedSolutionLog.LogEnd("DeleteMobilityPolicy");
        }

        internal string[] GetPolicyListInternal(LyncPolicyType type, string name)
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

        #region Sytsem Related Methods
        private void ReloadConfigurationInternal()
        {
            HostedSolutionLog.LogStart("ReloadConfigurationInternal");
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Enable-CsComputer");
                ExecuteShellCommand(runSpace, cmd, false);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("ReloadConfigurationInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("ReloadConfigurationInternal");
        }


        #endregion

        #region Federation Domains
        private LyncFederationDomain[] GetFederationDomainsInternal(string organizationId)
        {

            HostedSolutionLog.LogStart("GetFederationDomainsInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);


            LyncFederationDomain[] domains = null;

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Guid tenantId = Guid.Empty;

                domains = GetFederationDomainsInternal(runSpace, organizationId, ref tenantId);

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetFederationDomainsInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("GetFederationDomainsInternal");

            return domains;
        }

        private LyncFederationDomain[] GetFederationDomainsInternal(Runspace runSpace, string organizationId, ref Guid tenantId)
        {
            //Get TenantID
            List<LyncFederationDomain> domains = null;
            Command cmd = new Command("Get-CsTenant");
            cmd.Parameters.Add("Identity", GetOrganizationPath(organizationId));
            Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
            if ((result != null) && (result.Count > 0))
            {
                tenantId = (Guid)GetPSObjectProperty(result[0], "TenantId");

                //Get-CSTenantFederationConfiguration (AllowedDomains)
                cmd = new Command("Get-CsTenantFederationConfiguration");
                cmd.Parameters.Add("Tenant", tenantId);
                result = ExecuteShellCommand(runSpace, cmd, false);

                if ((result != null) && (result.Count > 0))
                {
                    domains = new List<LyncFederationDomain>();

                    if (GetPSObjectProperty(result[0], "AllowedDomains").GetType().ToString() == "Microsoft.Rtc.Management.WritableConfig.Settings.Edge.AllowList")
                    {
                        AllowList allowList = (AllowList)GetPSObjectProperty(result[0], "AllowedDomains");

                        foreach (DomainPattern d in allowList.AllowedDomain)
                        {
                            LyncFederationDomain domain = new LyncFederationDomain();
                            domain.DomainName = d.Domain.ToString();
                            domains.Add(domain);
                        }
                    }
                }
            }

            if (domains != null)
                return domains.ToArray();
            else
                return null;
        }


        private bool AddFederationDomainInternal(string organizationId, string domainName, string proxyFqdn)
        {
            HostedSolutionLog.LogStart("AddFederationDomainInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("domainName: {0}", domainName);

            domainName = domainName.ToLower();
            proxyFqdn = proxyFqdn.ToLower();

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Guid tenantId = Guid.Empty;
                Command cmd = new Command("Get-CsTenant");
                cmd.Parameters.Add("Identity", GetOrganizationPath(organizationId));
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                if ((result != null) && (result.Count > 0))
                {
                    tenantId = (Guid)GetPSObjectProperty(result[0], "TenantId");

                    //Get-CSTenantFederationConfiguration (AllowedDomains)
                    cmd = new Command("Get-CsTenantFederationConfiguration");
                    cmd.Parameters.Add("Tenant", tenantId);
                    result = ExecuteShellCommand(runSpace, cmd, false);

                    if ((result != null) && (result.Count > 0))
                    {
                        AllowList allowList = null;

                        if (GetPSObjectProperty(result[0], "AllowedDomains").GetType().ToString() == "Microsoft.Rtc.Management.WritableConfig.Settings.Edge.AllowList")
                        {
                            allowList = (AllowList)GetPSObjectProperty(result[0], "AllowedDomains");
                            DomainPattern domain = new DomainPattern(domainName);
                            allowList.AllowedDomain.Add(domain);
                        }
                        else
                        {
                            allowList = new AllowList();
                            DomainPattern domain = new DomainPattern(domainName);
                            allowList.AllowedDomain.Add(domain);
                        }

                        cmd = new Command("Set-CsTenantFederationConfiguration");
                        cmd.Parameters.Add("Tenant", tenantId);
                        cmd.Parameters.Add("AllowedDomains", allowList);
                        ExecuteShellCommand(runSpace, cmd, false);
                    }
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("AddFederationDomainInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("AddFederationDomainInternal");

            return true;
        }

        private bool RemoveFederationDomainInternal(string organizationId, string domainName)
        {
            HostedSolutionLog.LogStart("RemoveFederationDomainInternal");
            HostedSolutionLog.DebugInfo("organizationId: {0}", organizationId);
            HostedSolutionLog.DebugInfo("domainName: {0}", domainName);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Guid tenantId = Guid.Empty;
                Command cmd = new Command("Get-CsTenant");
                cmd.Parameters.Add("Identity", GetOrganizationPath(organizationId));
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, false);
                if ((result != null) && (result.Count > 0))
                {
                    tenantId = (Guid)GetPSObjectProperty(result[0], "TenantId");

                    //Get-CSTenantFederationConfiguration (AllowedDomains)
                    cmd = new Command("Get-CsTenantFederationConfiguration");
                    cmd.Parameters.Add("Tenant", tenantId);
                    result = ExecuteShellCommand(runSpace, cmd, false);

                    if ((result != null) && (result.Count > 0))
                    {
                        AllowList allowList = null;

                        if (GetPSObjectProperty(result[0], "AllowedDomains").GetType().ToString() == "Microsoft.Rtc.Management.WritableConfig.Settings.Edge.AllowList")
                        {
                            HostedSolutionLog.DebugInfo("Remove DomainName: {0}", domainName);
                            allowList = (AllowList)GetPSObjectProperty(result[0], "AllowedDomains");
                            DomainPattern domain = null;
                            foreach (DomainPattern d in allowList.AllowedDomain)
                            {
                                if (d.Domain.ToLower() == domainName.ToLower())
                                {
                                    domain = d;
                                    break;
                                }
                            }
                            if (domain != null)
                                allowList.AllowedDomain.Remove(domain);
                        }

                        cmd = new Command("Set-CsTenantFederationConfiguration");
                        cmd.Parameters.Add("Tenant", tenantId);
                        cmd.Parameters.Add("AllowedDomains", allowList);
                        ExecuteShellCommand(runSpace, cmd, false);
                    }
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("RemoveFederationDomainInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("RemoveFederationDomainInternal");

            return true;
        }




        #endregion

        #region PowerShell integration
        private static InitialSessionState session = null;

        internal virtual Runspace OpenRunspace()
        {
            HostedSolutionLog.LogStart("OpenRunspace");

            if (session == null)
            {
                session = InitialSessionState.CreateDefault();
                session.ImportPSModule(new string[] { "ActiveDirectory", "Lync", "LyncOnline" });
            }
            Runspace runSpace = RunspaceFactory.CreateRunspace(session);
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

            if (useDomainController)
            {
                CommandParameter dc = new CommandParameter("DomainController", PrimaryDomainController);
                if (!cmd.Parameters.Contains(dc))
                {
                    cmd.Parameters.Add(dc);
                }
            }

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

        internal object GetPSObjectProperty(PSObject obj, string name)
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


        #endregion

        #region Transactions

        internal LyncTransaction StartTransaction()
        {
            return new LyncTransaction();
        }

        internal void RollbackTransaction(LyncTransaction transaction)
        {
            HostedSolutionLog.LogStart("RollbackTransaction");
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();


                for (int i = transaction.Actions.Count - 1; i > -1; i--)
                {
                    //reverse order
                    try
                    {
                        RollbackAction(transaction.Actions[i], runSpace);
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

                CloseRunspace(runSpace);
            }
            HostedSolutionLog.LogEnd("RollbackTransaction");
        }

        private void RollbackAction(TransactionAction action, Runspace runspace)
        {
            HostedSolutionLog.LogInfo("Rollback action: {0}", action.ActionType);
            switch (action.ActionType)
            {
                case TransactionAction.TransactionActionTypes.LyncNewSipDomain:
                    DeleteSipDomain(runspace, action.Id);
                    break;
                //case TransactionAction.TransactionActionTypes.LyncNewSimpleUrl:
                //DeleteSimpleUrl(runspace, action.Id);
                //break;
                case TransactionAction.TransactionActionTypes.LyncNewUser:
                    DeleteUser(runspace, action.Id);
                    break;
                case TransactionAction.TransactionActionTypes.LyncNewConferencingPolicy:
                    DeleteConferencingPolicy(runspace, action.Id);
                    break;
                case TransactionAction.TransactionActionTypes.LyncNewExternalAccessPolicy:
                    DeleteExternalAccessPolicy(runspace, action.Id);
                    break;
                case TransactionAction.TransactionActionTypes.LyncNewMobilityPolicy:
                    DeleteMobilityPolicy(runspace, action.Id);
                    break;
            }
        }

        #endregion

        #region helpers
        private string GetOrganizationPath(string organizationId)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private static void AppendOUPath(StringBuilder sb, string ou)
        {
            if (string.IsNullOrEmpty(ou))
                return;

            string path = ou.Replace("/", "\\");
            string[] parts = path.Split('\\');
            for (int i = parts.Length - 1; i != -1; i--)
                sb.Append("OU=").Append(parts[i]).Append(",");
        }

        private static void AppendDomainPath(StringBuilder sb, string domain)
        {
            if (string.IsNullOrEmpty(domain))
                return;

            string[] parts = domain.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append("DC=").Append(parts[i]);

                if (i < (parts.Length - 1))
                    sb.Append(",");
            }
        }

        internal string AddADPrefix(string path)
        {
            string dn = path;
            if (!dn.ToUpper().StartsWith("LDAP://"))
            {
                dn = string.Format("LDAP://{0}/{1}", PrimaryDomainController, dn);
            }
            return dn;
        }

        #endregion

        public override bool IsInstalled()
        {
            string value = "";
            bool bResult = false;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey(LyncRegistryPath);
            if (rk != null)
            {
                value = (string)rk.GetValue("ProductVersion", null);
                if (value == "5.0.8308.0")
                    bResult = true;

                rk.Close();
            }
            return bResult;
        }
    }
}
