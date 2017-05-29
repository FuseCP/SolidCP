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
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Globalization;

using System.Management.Automation;
using System.Management.Automation.Runspaces;

using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.OS;
using System.Reflection;

namespace SolidCP.Providers.HostedSolution
{
    public class OrganizationProvider : HostingServiceProviderBase, IOrganization
    {
        #region Constants

        private const string GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE = @"\\{0}\SYSVOL\{0}\Policies\{1}\User\Preferences\Drives";
        private const string GROUP_POLICY_MAPPED_DRIVES_ROOT_PATH_TEMPLATE = @"\\{0}\SYSVOL\{0}\Policies\{1}";
        private const string DRIVES_CLSID = "{8FDDCC1A-0C3C-43cd-A6B4-71A6DF20DA8C}";
        private const string DRIVE_CLSID = "{935D1B74-9CB8-4e3c-9914-7DD559B7A417}";
        private const string gPCUserExtensionNames = "[{00000000-0000-0000-0000-000000000000}{2EA1A81B-48E5-45E9-8BB7-A6E3AC170006}][{5794DAFD-BE60-433F-88A2-1A31939AC01F}{2EA1A81B-48E5-45E9-8BB7-A6E3AC170006}]";

        #endregion

        #region Properties

        private string RootOU
        {
            get { return ProviderSettings["RootOU"]; }
        }

        private string RootDomain
        {
            get { return ServerSettings.ADRootDomain; }
        }

        private string PrimaryDomainController
        {
            get { return ProviderSettings["PrimaryDomainController"]; }
        }

        #endregion



        #region Helpers

        private string GetObjectTargetAccountName(string accountName, string domain)
        {
            string part = domain.Split('.')[0].ToUpperInvariant();

            return string.Format("{0}\\{1}", part, accountName);
        }

        private string GetOrganizationTargetPath(string organizationId)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetOrganizationPath(string organizationId)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetUserPath(string organizationId, string loginName)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, loginName);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetGroupPath(string organizationId)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, organizationId);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetObjectPath(string organizationId, string objName)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, objName);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetGroupPath(string organizationId, string groupName)
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendCNPath(sb, groupName);
            AppendOUPath(sb, organizationId);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private string GetRootOU()
        {
            StringBuilder sb = new StringBuilder();
            // append provider
            AppendProtocol(sb);
            AppendDomainController(sb);
            AppendOUPath(sb, RootOU);
            AppendDomainPath(sb, RootDomain);

            return sb.ToString();
        }

        private void AppendDomainController(StringBuilder sb)
        {
            sb.Append(PrimaryDomainController + "/");
        }

        private static void AppendCNPath(StringBuilder sb, string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
                return;

            sb.Append("CN=").Append(organizationId).Append(",");
        }

        private static void AppendProtocol(StringBuilder sb)
        {
            sb.Append("LDAP://");
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

        #endregion



        #region Organizations

        public bool OrganizationExists(string organizationId)
        {
            return OrganizationExistsInternal(organizationId);
        }

        internal bool OrganizationExistsInternal(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            string orgPath = GetOrganizationPath(organizationId);
            return ActiveDirectoryUtils.AdObjectExists(orgPath);
        }

        public Organization CreateOrganization(string organizationId)
        {
            return CreateOrganizationInternal(organizationId);
        }

        internal Organization CreateOrganizationInternal(string organizationId)
        {
            HostedSolutionLog.LogStart("CreateOrganizationInternal");
            HostedSolutionLog.DebugInfo("OrganizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            bool ouCreated = false;
            bool groupCreated = false;

            Organization org;
            try
            {
                string parentPath = GetRootOU();
                string orgPath = GetOrganizationPath(organizationId);

                //Create OU
                ActiveDirectoryUtils.CreateOrganizationalUnit(organizationId, parentPath);
                ouCreated = true;

                //Create security group
                ActiveDirectoryUtils.CreateGroup(orgPath, organizationId);
                groupCreated = true;
            
                org = new Organization();
                org.OrganizationId = organizationId;
                org.DistinguishedName = ActiveDirectoryUtils.RemoveADPrefix(orgPath);
                org.SecurityGroup = ActiveDirectoryUtils.RemoveADPrefix(GetGroupPath(organizationId));

                org.GroupName = organizationId;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                try
                {
                    if (groupCreated)
                    {
                        string groupPath = GetGroupPath(organizationId);
                        ActiveDirectoryUtils.DeleteADObject(groupPath);
                    }
                }
                catch (Exception e)
                {
                    HostedSolutionLog.LogError(e);
                }

                try
                {
                    if (ouCreated)
                    {
                        string orgPath = GetOrganizationPath(organizationId);
                        ActiveDirectoryUtils.DeleteADObject(orgPath);
                    }
                }
                catch (Exception e)
                {
                    HostedSolutionLog.LogError(e);
                }

                throw;
            }

            HostedSolutionLog.LogEnd("CreateOrganizationInternal");

            return org;
        }

        public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {

            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is Organization)
                    {
                        Organization org = item as Organization;
                        ChangeOrganizationState(org, enabled);
                    }
                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError(
                        String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }
        }

        private void ChangeOrganizationState(Organization org, bool enabled)
        {
            string path = GetOrganizationPath(org.OrganizationId);
            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

            string filter =
                string.Format(CultureInfo.InvariantCulture, "(&(objectClass=user)(!{0}=disabled))",
                              ADAttributes.CustomAttribute2);
            using (DirectorySearcher searcher = new DirectorySearcher(entry, filter))
            {
                SearchResultCollection resCollection = searcher.FindAll();
                foreach (SearchResult res in resCollection)
                {
                    DirectoryEntry de = res.GetDirectoryEntry();
                    de.InvokeSet("AccountDisabled", !enabled);
                    de.CommitChanges();
                }
            }
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is Organization)
                    {
                        Organization org = item as Organization;
                        DeleteOrganizationInternal(org.OrganizationId);
                    }

                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }

        }

        public void DeleteOrganization(string organizationId)
        {
            DeleteOrganizationInternal(organizationId);
        }

        internal void DeleteOrganizationInternal(string organizationId)
        {
            HostedSolutionLog.LogStart("DeleteOrganizationInternal");
            HostedSolutionLog.DebugInfo("OrganizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            string groupPath = GetGroupPath(organizationId);
            string psoName = FormOrganizationPSOName(organizationId);
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                if (FineGrainedPasswordPolicyExist(runspace, psoName))
                {
                    RemoveFineGrainedPasswordPolicy(runspace, psoName);
                }

                ActiveDirectoryUtils.DeleteADObject(groupPath);
            }
            catch
            {
                /* skip */
            }
            finally
            {
                CloseRunspace(runspace);
            }

            string path = GetOrganizationPath(organizationId);
            ActiveDirectoryUtils.DeleteADObject(path, true);

            HostedSolutionLog.LogEnd("DeleteOrganizationInternal");
        }

        #endregion


        #region Users

        public int CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            return CreateUserInternal(organizationId, loginName, displayName, upn, password, enabled);
        }

        internal int CreateUserInternal(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            HostedSolutionLog.LogStart("CreateUserInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);
            HostedSolutionLog.DebugInfo("loginName : {0}", loginName);
            HostedSolutionLog.DebugInfo("displayName : {0}", displayName);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentNullException("loginName");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            bool userCreated = false;
            string userPath = null;
            try
            {
                string path = GetOrganizationPath(organizationId);
                userPath = GetUserPath(organizationId, loginName);
                if (!ActiveDirectoryUtils.AdObjectExists(userPath))
                {
                    userPath = ActiveDirectoryUtils.CreateUser(path, null, loginName, displayName, password, enabled);
                    DirectoryEntry entry = new DirectoryEntry(userPath);
                    ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.UserPrincipalName, upn);
                    entry.CommitChanges();
                    userCreated = true;
                    HostedSolutionLog.DebugInfo("User created: {0}", displayName);
                }
                else
                {
                    HostedSolutionLog.DebugInfo("AD_OBJECT_ALREADY_EXISTS: {0}", userPath);
                    HostedSolutionLog.LogEnd("CreateUserInternal");
                    return Errors.AD_OBJECT_ALREADY_EXISTS;
                }

                string groupPath = GetGroupPath(organizationId);
                HostedSolutionLog.DebugInfo("Group retrieved: {0}", groupPath);

                ActiveDirectoryUtils.AddObjectToGroup(userPath, groupPath);
                HostedSolutionLog.DebugInfo("Added to group: {0}", groupPath);
            }
            catch (Exception e)
            {
                HostedSolutionLog.LogError(e);
                try
                {
                    if (userCreated)
                        ActiveDirectoryUtils.DeleteADObject(userPath);
                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError(ex);
                }

                return Errors.AD_OBJECT_ALREADY_EXISTS;
            }

            HostedSolutionLog.LogEnd("CreateUserInternal");
            return Errors.OK;
        }

        public List<OrganizationUser> GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration)
        {
            return GetOrganizationUsersWithExpiredPasswordInternal(organizationId, daysBeforeExpiration);
        }

        internal List<OrganizationUser> GetOrganizationUsersWithExpiredPasswordInternal(string organizationId, int daysBeforeExpiration)
        {
            var result = new List<OrganizationUser>();

            if (string.IsNullOrEmpty(organizationId))
            {
                return result;
            }

            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                var psoName = FormOrganizationPSOName(organizationId);

                var maxPasswordAgeSpan = GetMaxPasswordAge(runspace, psoName);

                var searchRoot = new DirectoryEntry(GetOrganizationPath(organizationId));

                var search = new DirectorySearcher(searchRoot)
                {
                    SearchScope = SearchScope.Subtree,
                    Filter = "(objectClass=user)"
                };

                search.PropertiesToLoad.Add("pwdLastSet");
                search.PropertiesToLoad.Add("sAMAccountName");

                SearchResultCollection searchResults = search.FindAll();

                foreach (SearchResult searchResult in searchResults)
                {
                    var pwdLastSetTicks = (long) searchResult.Properties["pwdLastSet"][0];

                    var pwdLastSetDate = DateTime.FromFileTimeUtc(pwdLastSetTicks);

                    var expirationDate = maxPasswordAgeSpan == TimeSpan.MaxValue ? DateTime.MaxValue : pwdLastSetDate.AddDays(maxPasswordAgeSpan.Days);

                    if (expirationDate.AddDays(-daysBeforeExpiration) < DateTime.Now)
                    {
                        var user = new OrganizationUser();

                        user.PasswordExpirationDateTime = expirationDate;
                        user.SamAccountName = (string) searchResult.Properties["sAMAccountName"][0];

                        result.Add(user);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }

            return result;
        }

        internal TimeSpan GetMaxPasswordAge(Runspace runspace, string psoName)
        {
            if (FineGrainedPasswordPolicyExist(runspace, psoName))
            {
                var psoObject = GetFineGrainedPasswordPolicy(runspace, psoName);

                var span = GetPSObjectProperty(psoObject, "MaxPasswordAge") as TimeSpan?;

                if (span != null)
                {
                    if (span.Value.Duration() == new TimeSpan().Duration())
                    {
                        return TimeSpan.MaxValue;
                    }

                    return span.Value;
                }
            }


            using (Domain d = Domain.GetCurrentDomain())
            {
                using (DirectoryEntry domain = d.GetDirectoryEntry())
                {
                    DirectorySearcher ds = new DirectorySearcher(
                        domain,
                        "(objectClass=*)",
                        null,
                        SearchScope.Base
                        );

                    SearchResult sr = ds.FindOne();

                    if (sr != null && sr.Properties.Contains("maxPwdAge"))
                    {
                        try
                        {
                            return TimeSpan.FromTicks((long)sr.Properties["maxPwdAge"][0]).Duration();

                        }
                        catch (Exception)
                        {
                            return TimeSpan.MaxValue;
                        }
                    }

                    throw new Exception("'maxPwdAge' property not found.");
                }
            }
        }

        public bool CheckPhoneNumberIsInUse(string phoneNumber, string userPrincipalName = null)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            phoneNumber = phoneNumber.Replace("+", "");

            var userExcludeQuery = string.IsNullOrEmpty(userPrincipalName) ? string.Empty : string.Format("(!(UserPrincipalName={0}))", userPrincipalName);

            string query = string.Format("(&" +
                                             "(objectClass=user)" +
                                             "(|" +
                                                "(|(facsimileTelephoneNumber=+{0})(facsimileTelephoneNumber={0}))" +
                                                "(|(homePhone=+{0})(homePhone={0}))" +
                                                "(|(mobile=+{0})(mobile={0}))" +
                                                "(|(pager=+{0})(pager={0}))" +
                                                "(|(telephoneNumber=+{0})(telephoneNumber={0}))" +
                                             ")" +
                                             "{1}" +
                                         ")", phoneNumber, userExcludeQuery);

            using (Domain d = Domain.GetCurrentDomain())
            {
                using (DirectoryEntry domain = d.GetDirectoryEntry())
                {

                    var search = new DirectorySearcher(domain)
                    {
                        SearchScope = SearchScope.Subtree,
                        Filter = query
                    };

                    search.PropertiesToLoad.Add(ADAttributes.Fax);
                    search.PropertiesToLoad.Add(ADAttributes.HomePhone);
                    search.PropertiesToLoad.Add(ADAttributes.MobilePhone);
                    search.PropertiesToLoad.Add(ADAttributes.Pager);
                    search.PropertiesToLoad.Add(ADAttributes.BusinessPhone);

                    SearchResult result = search.FindOne();

                    if (result != null)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }

        public void ApplyPasswordSettings(string organizationId, OrganizationPasswordSettings settings)
        {
            HostedSolutionLog.LogStart("ApplyPasswordPolicy");

            Runspace runspace = null;

            var psoName = FormOrganizationPSOName(organizationId);

            try
            {
                runspace = OpenRunspace();

                if (!FineGrainedPasswordPolicyExist(runspace, psoName))
                {
                    CreateFineGrainedPasswordPolicy(runspace, organizationId, psoName, settings);
                }
                else
                {
                    UpdateFineGrainedPasswordPolicy(runspace, psoName, settings);
                }

                string groupPath = GetGroupPath(organizationId);

                SetFineGrainedPasswordPolicySubject(runspace, groupPath, psoName);

                if (settings.MaxPasswordAge == 0)
                {
                    SetPasswordNeverExpiresInFineGrainedPasswordPolicy(runspace, psoName);
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
                HostedSolutionLog.LogEnd("ApplyPasswordPolicy");
            }
        }

        private string FormOrganizationPSOName(string organizationId)
        {
            return string.Format("{0}-PSO", organizationId);
        }

        private void SetPasswordNeverExpiresInFineGrainedPasswordPolicy(Runspace runspace, string psoName)
        {
            var psoObject = GetFineGrainedPasswordPolicy(runspace, psoName);

            var distinguishedName = GetPSObjectProperty(psoObject, "DistinguishedName") as string;

            var cmd = new Command("Set-ADObject");
            cmd.Parameters.Add("Identity", distinguishedName);

            var hashTable = new Hashtable();

            hashTable.Add("msDS-MaximumPasswordAge", "-9223372036854775808");

            cmd.Parameters.Add("Replace", hashTable);

            ExecuteShellCommand(runspace, cmd);
        }

        private bool FineGrainedPasswordPolicyExist(Runspace runspace, string psoName)
        {
            try
            {
                var cmd = new Command("Get-ADFineGrainedPasswordPolicy");
                cmd.Parameters.Add("Identity", psoName);

                var result = ExecuteShellCommand(runspace, cmd);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private PSObject GetFineGrainedPasswordPolicy(Runspace runspace, string psoName)
        {
            var cmd = new Command("Get-ADFineGrainedPasswordPolicy");
            cmd.Parameters.Add("Identity", psoName);

            return ExecuteShellCommand(runspace, cmd).FirstOrDefault();
        }

        private void CreateFineGrainedPasswordPolicy(Runspace runspace, string organizationId, string psoName, OrganizationPasswordSettings settings)
        {
            var cmd = new Command("New-ADFineGrainedPasswordPolicy");
            cmd.Parameters.Add("Name", psoName);
            cmd.Parameters.Add("Description", string.Format("The {0} Password Policy", organizationId));
            cmd.Parameters.Add("Precedence", 50);
            cmd.Parameters.Add("MinPasswordLength", settings.MinimumLength);
            cmd.Parameters.Add("PasswordHistoryCount", settings.EnforcePasswordHistory);
            cmd.Parameters.Add("ComplexityEnabled", false);
            cmd.Parameters.Add("ReversibleEncryptionEnabled", false);
            cmd.Parameters.Add("MaxPasswordAge", new TimeSpan(settings.MaxPasswordAge * 24, 0, 0));
            
            if (settings.LockoutSettingsEnabled)
            {
                cmd.Parameters.Add("LockoutDuration", new TimeSpan(0, settings.AccountLockoutDuration, 0));
                cmd.Parameters.Add("LockoutThreshold", settings.AccountLockoutThreshold);
                cmd.Parameters.Add("LockoutObservationWindow", new TimeSpan(0, settings.ResetAccountLockoutCounterAfter, 0));
            }

            ExecuteShellCommand(runspace, cmd);
        }

        private void SetFineGrainedPasswordPolicySubject(Runspace runspace, string subjectPath, string psoName)
        {
            var entry = new DirectoryEntry(subjectPath);

            var cmd = new Command("Add-ADFineGrainedPasswordPolicySubject");
            cmd.Parameters.Add("Identity", psoName);
            cmd.Parameters.Add("Subjects", entry.Properties[ADAttributes.DistinguishedName].Value.ToString());

            ExecuteShellCommand(runspace, cmd);

            cmd = new Command("Set-ADGroup");
            cmd.Parameters.Add("Identity", entry.Properties[ADAttributes.DistinguishedName].Value.ToString());
            cmd.Parameters.Add("GroupScope", "Global");

            ExecuteShellCommand(runspace, cmd);
        }

        private void UpdateFineGrainedPasswordPolicy(Runspace runspace, string psoName, OrganizationPasswordSettings settings)
        {
            var cmd = new Command("Set-ADFineGrainedPasswordPolicy");
            cmd.Parameters.Add("Identity", psoName);
            cmd.Parameters.Add("MinPasswordLength", settings.MinimumLength);
            cmd.Parameters.Add("PasswordHistoryCount", settings.EnforcePasswordHistory);
            cmd.Parameters.Add("ComplexityEnabled", false);
            cmd.Parameters.Add("ReversibleEncryptionEnabled", false);
            cmd.Parameters.Add("MaxPasswordAge", new TimeSpan(settings.MaxPasswordAge*24, 0, 0));

            if (settings.LockoutSettingsEnabled)
            {
                cmd.Parameters.Add("LockoutDuration", new TimeSpan(0, settings.AccountLockoutDuration, 0));
                cmd.Parameters.Add("LockoutThreshold", settings.AccountLockoutThreshold);
                cmd.Parameters.Add("LockoutObservationWindow", new TimeSpan(0, settings.ResetAccountLockoutCounterAfter, 0));
            }
            else
            {
                cmd.Parameters.Add("LockoutDuration", new TimeSpan(0));
                cmd.Parameters.Add("LockoutThreshold", 0);
                cmd.Parameters.Add("LockoutObservationWindow", 0);
            }

            ExecuteShellCommand(runspace, cmd);
        }

        private void RemoveFineGrainedPasswordPolicy(Runspace runspace, string psoName)
        {
            var cmd = new Command("Remove-ADFineGrainedPasswordPolicy");
            cmd.Parameters.Add("Identity", psoName);

            ExecuteShellCommand(runspace, cmd);
        }

        public PasswordPolicyResult GetPasswordPolicy()
        {
            return GetPasswordPolicyInternal();
        }

        internal PasswordPolicyResult GetPasswordPolicyInternal()
        {
            HostedSolutionLog.LogStart("GetPasswordPolicyInternal");

            PasswordPolicyResult res = new PasswordPolicyResult { IsSuccess = true };

            string[] policyAttributes = new[] {"minPwdLength", 
                                               "pwdProperties", 
                                               "objectClass"};
            try
            {
                DirectoryEntry domainRoot = new DirectoryEntry(ActiveDirectoryUtils.ConvertDomainName(RootDomain));

                DirectorySearcher ds = new DirectorySearcher(
                    domainRoot,
                    "(objectClass=domainDNS)",
                    policyAttributes,
                    SearchScope.Base
                    );


                SearchResult result = ds.FindOne();

                PasswordPolicy ret = new PasswordPolicy
                {
                    MinLength = ((int)result.Properties["minPwdLength"][0]),
                    IsComplexityEnable = ((int)result.Properties["pwdProperties"][0] == 1)
                };
                res.Value = ret;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                res.IsSuccess = false;
                res.ErrorCodes.Add(ErrorCodes.CANNOT_GET_PASSWORD_COMPLEXITY);
            }

            HostedSolutionLog.LogEnd("GetPasswordPolicyInternal");
            return res;
        }

        public void DisableUser(string loginName, string organizationId)
        {
            DisableUserInternal(loginName, organizationId);
        }

        public void DisableUserInternal(string loginName, string organizationId)
        {
            HostedSolutionLog.LogStart("DisableUserInternal");
            HostedSolutionLog.DebugInfo("loginName : {0}", loginName);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentNullException("loginName");

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            string path = GetUserPath(organizationId, loginName);

            if (ActiveDirectoryUtils.AdObjectExists(path))
            {
                DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

                entry.InvokeSet(ADAttributes.AccountDisabled, true);
                
                entry.CommitChanges();
            }

            HostedSolutionLog.LogEnd("DisableUserInternal");
        }

        public void DeleteUser(string loginName, string organizationId)
        {
            DeleteUserInternal(loginName, organizationId);
        }

        internal void DeleteUserInternal(string loginName, string organizationId)
        {
            HostedSolutionLog.LogStart("DeleteUserInternal");
            HostedSolutionLog.DebugInfo("loginName : {0}", loginName);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentNullException("loginName");

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            string path = GetUserPath(organizationId, loginName);
            if (ActiveDirectoryUtils.AdObjectExists(path))
                ActiveDirectoryUtils.DeleteADObject(path, true);

            HostedSolutionLog.LogEnd("DeleteUserInternal");
        }

        public OrganizationUser GetUserGeneralSettings(string loginName, string organizationId)
        {
            return GetUserGeneralSettingsInternal(loginName, organizationId);
        }

        internal OrganizationUser GetUserGeneralSettingsInternal(string loginName, string organizationId)
        {
            HostedSolutionLog.LogStart("GetUserGeneralSettingsInternal");
            HostedSolutionLog.DebugInfo("loginName : {0}", loginName);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentNullException("loginName");

            string path = GetUserPath(organizationId, loginName);

            OrganizationUser retUser = GetUser(organizationId, path);

            HostedSolutionLog.LogEnd("GetUserGeneralSettingsInternal");
            return retUser;
        }

        private static Int64 ConvertADSLargeIntegerToInt64(object adsLargeInteger)
        {
            var highPart = (Int32)adsLargeInteger.GetType().InvokeMember("HighPart", System.Reflection.BindingFlags.GetProperty, null, adsLargeInteger, null);
            var lowPart = (Int32)adsLargeInteger.GetType().InvokeMember("LowPart", System.Reflection.BindingFlags.GetProperty, null, adsLargeInteger, null);
            return highPart * ((Int64)UInt32.MaxValue + 1) + lowPart;
        }

        private bool GetUserMustChangePassword(DirectoryEntry user)
        {
            Int64 pls;
            int uac;

            if (user.Properties[ADAttributes.PwdLastSet] != null && user.Properties[ADAttributes.PwdLastSet].Value != null)
                pls = ConvertADSLargeIntegerToInt64(user.Properties[ADAttributes.PwdLastSet].Value);
            else
                return false;

            if (user.Properties[ADAttributes.UserAccountControl] != null && user.Properties[ADAttributes.UserAccountControl].Value != null)
                uac = (int)user.Properties[ADAttributes.UserAccountControl].Value;
            else
                return false;

            return (pls == 0) && ((uac & 0x00010000) == 0);
        }

        private void SetUserMustChangePassword(DirectoryEntry user, bool userMustChangePassword)
        {
            Int64 pls;
            int uac;

            if (user.Properties[ADAttributes.PwdLastSet] != null && user.Properties[ADAttributes.PwdLastSet].Value != null)
                pls = ConvertADSLargeIntegerToInt64(user.Properties[ADAttributes.PwdLastSet].Value);
            else
                return;

            if (user.Properties[ADAttributes.UserAccountControl] != null && user.Properties[ADAttributes.UserAccountControl].Value != null)
                uac = (int)user.Properties[ADAttributes.UserAccountControl].Value;
            else
                return;

            if ((uac & 0x00010000) != 0) return;

            if ((pls == 0) == userMustChangePassword) return;

            user.Properties[ADAttributes.PwdLastSet].Value = userMustChangePassword ? 0 : -1;
        }

        private OrganizationUser GetUser(string organizationId, string path)
        {
            OrganizationUser retUser = new OrganizationUser();

            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

            retUser.FirstName = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.FirstName);
            retUser.LastName = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.LastName);
            retUser.DisplayName = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.DisplayName);
            retUser.Initials = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Initials);
            retUser.JobTitle = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.JobTitle);
            retUser.Company = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Company);
            retUser.Department = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Department);
            retUser.Office = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Office);
            retUser.BusinessPhone = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.BusinessPhone);
            retUser.Fax = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Fax);
            retUser.HomePhone = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.HomePhone);
            retUser.MobilePhone = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.MobilePhone);
            retUser.Pager = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Pager);
            retUser.WebPage = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.WebPage);
            retUser.Address = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Address);
            retUser.City = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.City);
            retUser.State = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.State);
            retUser.Zip = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Zip);
            retUser.Country = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Country);
            retUser.Notes = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Notes);
            retUser.ExternalEmail = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.ExternalEmail);
            retUser.Disabled = (bool)entry.InvokeGet(ADAttributes.AccountDisabled);
            retUser.Manager = GetManager(entry, ADAttributes.Manager);
            retUser.SamAccountName = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.SAMAccountName);
            retUser.DomainUserName = GetDomainName(ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.SAMAccountName));
            retUser.DistinguishedName = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.DistinguishedName);
            retUser.Locked = (bool)entry.InvokeGet(ADAttributes.AccountLocked);
            retUser.UserPrincipalName = (string)entry.InvokeGet(ADAttributes.UserPrincipalName);
            retUser.UserMustChangePassword = GetUserMustChangePassword(entry);

            return retUser;
        }

        public OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId)
        {
            HostedSolutionLog.LogStart("GetOrganizationUserWithExtraData");
            HostedSolutionLog.DebugInfo("loginName : {0}", loginName);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentNullException("loginName");

            var psoName = FormOrganizationPSOName(organizationId);

            string path = GetUserPath(organizationId, loginName);

            OrganizationUser retUser = GetUser(organizationId, path);

            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

            retUser.PasswordExpirationDateTime = GetPasswordExpirationDate(psoName, entry);

            HostedSolutionLog.LogEnd("GetOrganizationUserWithExtraData");

            return retUser;
        }

        private DateTime GetPasswordExpirationDate(string psoName, DirectoryEntry entry)
        {
            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                var maxPasswordAgeSpan = GetMaxPasswordAge(runspace, psoName);

                var pwdLastSetTicks = ConvertADSLargeIntegerToInt64(entry.Properties[ADAttributes.PwdLastSet].Value);

                var pwdLastSetDate = DateTime.FromFileTimeUtc(pwdLastSetTicks);

                if (maxPasswordAgeSpan == TimeSpan.MaxValue)
                {
                    return DateTime.MaxValue;
                }

                return pwdLastSetDate.AddDays(maxPasswordAgeSpan.Days);
            }
            finally
            {
                CloseRunspace(runspace);
            }

        }

        private string GetDomainName(string username)
        {
            string domain = ActiveDirectoryUtils.GetNETBIOSDomainName(RootDomain);
            string ret = string.Format(@"{0}\{1}", domain, username);
            return ret;
        }

        private OrganizationUser GetManager(DirectoryEntry entry, string adAttribute)
        {
            OrganizationUser retUser = null;
            string path = ActiveDirectoryUtils.GetADObjectStringProperty(entry, adAttribute);
            if (!string.IsNullOrEmpty(path))
            {
                path = ActiveDirectoryUtils.AddADPrefix(path, PrimaryDomainController);
                if (ActiveDirectoryUtils.AdObjectExists(path))
                {
                    DirectoryEntry user = ActiveDirectoryUtils.GetADObject(path);
                    retUser = new OrganizationUser();
                    retUser.DisplayName = ActiveDirectoryUtils.GetADObjectStringProperty(user, ADAttributes.DisplayName);

                    retUser.AccountName = ActiveDirectoryUtils.GetADObjectStringProperty(user, ADAttributes.Name);
                }
            }

            return retUser;
        }

        public void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password,
            bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName,
            string address, string city, string state, string zip, string country, string jobTitle,
            string company, string department, string office, string managerAccountName,
            string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, string externalEmail, bool userMustChangePassword)
        {
            SetUserGeneralSettingsInternal(organizationId, accountName, displayName, password, hideFromAddressBook,
                disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle,
                company, department, office, managerAccountName, businessPhone, fax, homePhone,
                mobilePhone, pager, webPage, notes, externalEmail, userMustChangePassword);
        }

        internal void SetUserGeneralSettingsInternal(string organizationId, string accountName, string displayName, string password,
            bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName,
            string address, string city, string state, string zip, string country, string jobTitle,
            string company, string department, string office, string managerAccountName,
            string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, string externalEmail, bool userMustChangePassword)
        {
            string path = GetUserPath(organizationId, accountName);
            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);


            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.FirstName, firstName);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.LastName, lastName);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.DisplayName, displayName);

            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Initials, initials);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.JobTitle, jobTitle);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Company, company);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Department, department);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Office, office);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.BusinessPhone, businessPhone);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Fax, fax);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.HomePhone, homePhone);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.MobilePhone, mobilePhone);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Pager, pager);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.WebPage, webPage);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Address, address);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.City, city);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.State, state);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Zip, zip);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Country, country);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Notes, notes);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.ExternalEmail, externalEmail);
            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.CustomAttribute2, (disabled ? "disabled" : null));


            string manager = string.Empty;
            if (!string.IsNullOrEmpty(managerAccountName))
            {
                string managerPath = GetUserPath(organizationId, managerAccountName);
                manager = ActiveDirectoryUtils.AdObjectExists(managerPath) ? managerPath : string.Empty;
            }

            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Manager, ActiveDirectoryUtils.RemoveADPrefix(manager));

            entry.InvokeSet(ADAttributes.AccountDisabled, disabled);
            if (!string.IsNullOrEmpty(password))
                entry.Invoke(ADAttributes.SetPassword, password);

            if (!locked)
            {
                bool isLoked = (bool)entry.InvokeGet(ADAttributes.AccountLocked);
                if (isLoked)
                    entry.InvokeSet(ADAttributes.AccountLocked, locked);

            }

            SetUserMustChangePassword(entry, userMustChangePassword);

            entry.CommitChanges();
        }

        public void SetUserPassword(string organizationId, string accountName, string password)
        {
            SetUserPasswordInternal(organizationId, accountName, password);
        }

        internal void SetUserPasswordInternal(string organizationId, string accountName, string password)
        {
            string path = GetUserPath(organizationId, accountName);
            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

            if (!string.IsNullOrEmpty(password))
                entry.Invoke(ADAttributes.SetPassword, password);

            entry.CommitChanges();
        }


        public void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName)
        {
            SetUserPrincipalNameInternal(organizationId, accountName, userPrincipalName);
        }

        internal void SetUserPrincipalNameInternal(string organizationId, string accountName, string userPrincipalName)
        {
            string path = GetUserPath(organizationId, accountName);
            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.UserPrincipalName, userPrincipalName);

            entry.CommitChanges();
        }

        public string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName)
        {
            return GetSamAccountNameByUserPrincipalNameInternal(organizationId, userPrincipalName);
        }

        private string GetSamAccountNameByUserPrincipalNameInternal(string organizationId, string userPrincipalName)
        {
            HostedSolutionLog.LogStart("GetSamAccountNameByUserPrincipalNameInternal");
            HostedSolutionLog.DebugInfo("userPrincipalName : {0}", userPrincipalName);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            string accountName = string.Empty;

            try
            {

                string path = GetOrganizationPath(organizationId);
                DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.PropertiesToLoad.Add("userPrincipalName");
                searcher.PropertiesToLoad.Add("sAMAccountName");
                searcher.Filter = "(userPrincipalName=" + userPrincipalName + ")";
                searcher.SearchScope = SearchScope.Subtree;

                SearchResult resCollection = searcher.FindOne();
                if (resCollection != null)
                {
                    accountName = resCollection.Properties["samaccountname"][0].ToString();
                }

                HostedSolutionLog.LogEnd("GetSamAccountNameByUserPrincipalNameInternal");
            }
            catch (Exception e)
            {
                HostedSolutionLog.DebugInfo("Failed : {0}", e.Message);
            }

            return accountName;
        }


        public bool DoesSamAccountNameExist(string accountName)
        {
            return DoesSamAccountNameExistInternal(accountName);
        }


        private bool DoesSamAccountNameExistInternal(string accountName)
        {
            HostedSolutionLog.LogStart("DoesSamAccountNameExistInternal");
            HostedSolutionLog.DebugInfo("sAMAccountName : {0}", accountName);
            bool bFound = false;

            try
            {

                string path = GetRootOU();
                HostedSolutionLog.DebugInfo("Search path : {0}", path);
                DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.PropertiesToLoad.Add("sAMAccountName");
                searcher.Filter = "(sAMAccountName=" + accountName + ")";
                searcher.SearchScope = SearchScope.Subtree;

                SearchResult resCollection = searcher.FindOne();
                if (resCollection != null)
                {
                    if (resCollection.Properties["samaccountname"] != null)
                        bFound = true;
                }
            }
            catch (Exception e)
            {
                HostedSolutionLog.DebugInfo("Failed : {0}", e.Message);
            }

            HostedSolutionLog.DebugInfo("DoesSamAccountNameExistInternal Result: {0}", bFound);
            HostedSolutionLog.LogEnd("DoesSamAccountNameExistInternal");

            return bFound;
        }



        #endregion

        #region Domains

        public void CreateOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            CreateOrganizationDomainInternal(organizationDistinguishedName, domain);
        }

        /// <summary>
        /// Creates organization domain
        /// </summary>
        /// <param name="organizationDistinguishedName"></param>
        /// <param name="domain"></param>
        private void CreateOrganizationDomainInternal(string organizationDistinguishedName, string domain)
        {
            HostedSolutionLog.LogStart("CreateOrganizationDomainInternal");

            string path = ActiveDirectoryUtils.AddADPrefix(organizationDistinguishedName, PrimaryDomainController);
            ActiveDirectoryUtils.AddUPNSuffix(path, domain);
            HostedSolutionLog.LogEnd("CreateOrganizationDomainInternal");
        }


        public void DeleteOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            DeleteOrganizationDomainInternal(organizationDistinguishedName, domain);
        }

        /// <summary>
        /// Deletes organization domain
        /// </summary>
        /// <param name="organizationDistinguishedName"></param>
        /// <param name="domain"></param>
        private void DeleteOrganizationDomainInternal(string organizationDistinguishedName, string domain)
        {
            HostedSolutionLog.LogStart("DeleteOrganizationDomainInternal");

            //Remove UPN Suffix
            string path = ActiveDirectoryUtils.AddADPrefix(organizationDistinguishedName, PrimaryDomainController);
            ActiveDirectoryUtils.RemoveUPNSuffix(path, domain);

            HostedSolutionLog.LogEnd("DeleteOrganizationDomainInternal");
        }
        #endregion

        #region Security Groups

        public int CreateSecurityGroup(string organizationId, string groupName)
        {
            return CreateSecurityGroupInternal(organizationId, groupName);
        }

        internal int CreateSecurityGroupInternal(string organizationId, string groupName)
        {
            HostedSolutionLog.LogStart("CreateSecurityGroupInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);
            HostedSolutionLog.DebugInfo("groupName : {0}", groupName);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException("groupName");

            bool groupCreated = false;
            string groupPath = null;
            try
            {
                string path = GetOrganizationPath(organizationId);
                groupPath = GetGroupPath(organizationId, groupName);

                if (!ActiveDirectoryUtils.AdObjectExists(groupPath))
                {
                    ActiveDirectoryUtils.CreateGroup(path, groupName);

                    groupCreated = true;

                    HostedSolutionLog.DebugInfo("Security Group created: {0}", groupName);
                }
                else
                {
                    HostedSolutionLog.DebugInfo("AD_OBJECT_ALREADY_EXISTS: {0}", groupPath);
                    HostedSolutionLog.LogEnd("CreateSecurityGroupInternal");

                    return Errors.AD_OBJECT_ALREADY_EXISTS;
                }
            }
            catch (Exception e)
            {
                HostedSolutionLog.LogError(e);
                try
                {
                    if (groupCreated)
                        ActiveDirectoryUtils.DeleteADObject(groupPath);
                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError(ex);
                }

                return Errors.AD_OBJECT_ALREADY_EXISTS;
            }

            HostedSolutionLog.LogEnd("CreateSecurityGroupInternal");

            return Errors.OK;
        }

        public OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId)
        {
            return GetSecurityGroupGeneralSettingsInternal(groupName, organizationId);
        }

        internal OrganizationSecurityGroup GetSecurityGroupGeneralSettingsInternal(string groupName, string organizationId)
        {
            HostedSolutionLog.LogStart("GetSecurityGroupGeneralSettingsInternal");
            HostedSolutionLog.DebugInfo("groupName : {0}", groupName);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException("groupName");

            string path = GetGroupPath(organizationId, groupName);
            string organizationPath = GetOrganizationPath(organizationId);

            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);
            DirectoryEntry organizationEntry = ActiveDirectoryUtils.GetADObject(organizationPath);


            OrganizationSecurityGroup securityGroup = new OrganizationSecurityGroup();

            securityGroup.Notes = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.Notes);

            string samAccountName = ActiveDirectoryUtils.GetADObjectStringProperty(entry, ADAttributes.SAMAccountName);

            securityGroup.AccountName = samAccountName;
            securityGroup.SAMAccountName = samAccountName;

            List<ExchangeAccount> members = new List<ExchangeAccount>();

            foreach (string userPath in ActiveDirectoryUtils.GetGroupObjects(groupName, "user", organizationEntry))
            {
                OrganizationUser tmpUser = GetUser(organizationId, userPath);

                members.Add(new ExchangeAccount
                {
                    AccountName = ActiveDirectoryUtils.GetCNFromADPath(userPath),
                    SamAccountName = tmpUser.SamAccountName
                });
            }

            foreach (string groupPath in ActiveDirectoryUtils.GetGroupObjects(groupName, "group", organizationEntry))
            {
                DirectoryEntry groupEntry = ActiveDirectoryUtils.GetADObject(groupPath);

                string tmpSamAccountName = ActiveDirectoryUtils.GetADObjectStringProperty(groupEntry, ADAttributes.SAMAccountName);

                members.Add(new ExchangeAccount
                {
                    AccountName =  tmpSamAccountName,
                    SamAccountName =  tmpSamAccountName
                });
            }

            securityGroup.MembersAccounts = members.ToArray();

            HostedSolutionLog.LogEnd("GetSecurityGroupGeneralSettingsInternal");

            return securityGroup;
        }

        public void DeleteSecurityGroup(string groupName, string organizationId)
        {
            DeleteSecurityGroupInternal(groupName, organizationId);
        }

        internal void DeleteSecurityGroupInternal(string groupName, string organizationId)
        {
            HostedSolutionLog.LogStart("DeleteSecurityGroupInternal");
            HostedSolutionLog.DebugInfo("groupName : {0}", groupName);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException("groupName");

            string path = GetGroupPath(organizationId, groupName);

            if (ActiveDirectoryUtils.AdObjectExists(path))
                ActiveDirectoryUtils.DeleteADObject(path, true);

            HostedSolutionLog.LogEnd("DeleteSecurityGroupInternal");
        }

        public void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes)
        {

            SetSecurityGroupGeneralSettingsInternal(organizationId, groupName, memberAccounts, notes);
        }

        internal void SetSecurityGroupGeneralSettingsInternal(string organizationId, string groupName, string[] memberAccounts, string notes)
        {
            HostedSolutionLog.LogStart("SetSecurityGroupGeneralSettingsInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);
            HostedSolutionLog.DebugInfo("groupName : {0}", groupName);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException("groupName");

            string path = GetGroupPath(organizationId, groupName);

            DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

            ActiveDirectoryUtils.SetADObjectProperty(entry, ADAttributes.Notes, notes);

            entry.CommitChanges();

            string orgPath = GetOrganizationPath(organizationId);

            DirectoryEntry orgEntry = ActiveDirectoryUtils.GetADObject(orgPath);

            foreach (string userPath in ActiveDirectoryUtils.GetGroupObjects(groupName, "user", orgEntry))
            {
                ActiveDirectoryUtils.RemoveObjectFromGroup(userPath, path);
            }

            foreach (string groupPath in ActiveDirectoryUtils.GetGroupObjects(groupName, "group", orgEntry))
            {
                ActiveDirectoryUtils.RemoveObjectFromGroup(groupPath, path);
            }

            foreach (string obj in memberAccounts)
            {
                string objPath = GetObjectPath(organizationId, obj);
                ActiveDirectoryUtils.AddObjectToGroup(objPath, path);
            }   
        }

        public void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName)
        {
            AddObjectToSecurityGroupInternal(organizationId, accountName, groupName);
        }

        internal void AddObjectToSecurityGroupInternal(string organizationId, string accountName, string groupName)
        {
            HostedSolutionLog.LogStart("AddUserToSecurityGroupInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);
            HostedSolutionLog.DebugInfo("accountName : {0}", accountName);
            HostedSolutionLog.DebugInfo("groupName : {0}", groupName);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(accountName))
                throw new ArgumentNullException("loginName");

            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException("groupName");

            string objectPath = GetObjectPath(organizationId, accountName);

            string groupPath = GetGroupPath(organizationId, groupName);

            ActiveDirectoryUtils.AddObjectToGroup(objectPath, groupPath);
        }

        public void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName)
        {
            DeleteObjectFromSecurityGroupInternal(organizationId, accountName, groupName);
        }

        internal void DeleteObjectFromSecurityGroupInternal(string organizationId, string accountName, string groupName)
        {
            HostedSolutionLog.LogStart("AddUserToSecurityGroupInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);
            HostedSolutionLog.DebugInfo("accountName : {0}", accountName);
            HostedSolutionLog.DebugInfo("groupName : {0}", groupName);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(accountName))
                throw new ArgumentNullException("loginName");

            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException("groupName");

            string objectPath = GetObjectPath(organizationId, accountName);

            string groupPath = GetGroupPath(organizationId, groupName);

            ActiveDirectoryUtils.RemoveObjectFromGroup(objectPath, groupPath);
        }

        #endregion

        #region Drive Mapping

        public MappedDrive[] GetDriveMaps(string organizationId)
        {
            return GetDriveMapsInternal(organizationId, false);
        }

        internal MappedDrive[] GetDriveMapsInternal(string organizationId, bool newDrive)
        {
            HostedSolutionLog.LogStart("GetDriveMapsInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            ArrayList items = new ArrayList();

            string gpoId;

            if (!CheckMappedDriveGpoExists(organizationId, out gpoId))
            {
                HostedSolutionLog.LogEnd("GetDriveMapsInternal");

                return (MappedDrive[])items.ToArray(typeof(MappedDrive));
            }
            else if (!string.IsNullOrEmpty(gpoId))
            {
                var xml = GetOrCreateDrivesFile(gpoId);

                MappedDrive[] drives = GetDrivesFromXML(xml, items);

                if (drives.Length == 0 && !newDrive)
                {
                    DeleteMappedDrivesGPO(organizationId);
                }

                HostedSolutionLog.LogEnd("GetDriveMapsInternal");

                return drives;
            }

            HostedSolutionLog.LogEnd("GetDriveMapsInternal");

            return (MappedDrive[])items.ToArray(typeof(MappedDrive));
        }

       
        public int CreateMappedDrive(string organizationId, string drive, string labelAs, string path)
        {
            return CreateMappedDriveInternal(organizationId, drive, labelAs, path);
        }

        internal int CreateMappedDriveInternal(string organizationId, string drive, string labelAs, string path)
        {
            HostedSolutionLog.LogStart("CreateMappedDriveInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);
            HostedSolutionLog.DebugInfo("driveLetter : {0}:", drive);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(drive))
                throw new ArgumentNullException("driveLetter");

            string gpoId;

            if (!CheckMappedDriveGpoExists(organizationId, out gpoId))
            {
                CreateAndLinkMappedDrivesGPO(organizationId, out gpoId);
            }

            if (CheckMappedDriveExists(organizationId, path, true))
            {
                return Errors.MAPPED_DRIVE_ALREADY_EXISTS;
            }

            if (!string.IsNullOrEmpty(gpoId))
            {
                string drivesXmlPath = string.Format("{0}\\{1}",
                                                     string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId), 
                                                     "Drives.xml");
                var xml = GetOrCreateDrivesFile(gpoId);

                XmlNode drivesNode = xml.SelectSingleNode("(/*)");
                XmlNode driveNode = CreateDriveNode(xml, drive, labelAs, path);

                drivesNode.AppendChild(driveNode);

                xml.Save(drivesXmlPath);

                IncrementGPOVersion(organizationId, gpoId);
            }

            HostedSolutionLog.LogEnd("CreateMappedDriveInternal");

            return Errors.OK;
        }

        public void DeleteMappedDriveByPath(string organizationId, string path)
        {
            DeleteMappedDriveByPathInternal(organizationId, path);
        }

        internal void DeleteMappedDriveByPathInternal(string organizationId, string path)
        {
                HostedSolutionLog.LogStart("DeleteMappedDriveInternal");
                HostedSolutionLog.DebugInfo("path : {0}:", path);
                HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

                if (string.IsNullOrEmpty(organizationId))
                    throw new ArgumentNullException("organizationId");

                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("path");

                string gpoId;

                if (!CheckMappedDriveGpoExists(organizationId, out gpoId))
                {
                    CreateAndLinkMappedDrivesGPO(organizationId, out gpoId);
                }

                if (!string.IsNullOrEmpty(gpoId))
                {
                    string filePath = string.Format("{0}\\{1}",
                                                string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId),
                                                "Drives.xml");

                    var xml = GetOrCreateDrivesFile(gpoId);

                    XmlNode drive = xml.SelectSingleNode(string.Format("./Drives/Drive[contains(Properties/@path,'{0}')]", path));

                    if (drive != null)
                    {
                        drive.ParentNode.RemoveChild(drive);
                    }

                    xml.Save(filePath);

                    IncrementGPOVersion(organizationId, gpoId);
                }

                HostedSolutionLog.LogEnd("DeleteMappedDriveInternal");
        }

        public void DeleteMappedDrive(string organizationId, string drive)
        {
            DeleteMappedDriveInternal(organizationId, drive);
        }

        internal void DeleteMappedDriveInternal(string organizationId, string drive)
        {
            HostedSolutionLog.LogStart("DeleteMappedDriveInternal");
            HostedSolutionLog.DebugInfo("driveLetter : {0}:", drive);
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(drive))
                throw new ArgumentNullException("driveLetter");

            string gpoId;

            if (!CheckMappedDriveGpoExists(organizationId, out gpoId))
            {
                CreateAndLinkMappedDrivesGPO(organizationId, out gpoId);
            }

            if (!string.IsNullOrEmpty(gpoId))
            {
                string path = string.Format("{0}\\{1}",
                                            string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId),
                                            "Drives.xml");

                var xml = GetOrCreateDrivesFile(gpoId);

                XmlNode x = xml.SelectSingleNode(string.Format("/Drives/Drive[@name='{0}:']", drive));
                if (x != null)
                {
                    x.ParentNode.RemoveChild(x);
                }

                xml.Save(path);

                IncrementGPOVersion(organizationId, gpoId);
            }

            HostedSolutionLog.LogEnd("DeleteMappedDriveInternal");
        }

        public void DeleteMappedDrivesGPO(string organizationId)
        {
            DeleteMappedDrivesGPOInternal(organizationId);
        }

        internal void DeleteMappedDrivesGPOInternal(string organizationId)
        {
            HostedSolutionLog.LogStart("DeleteMappedDrivesGPOInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            Runspace runSpace = null;

            try
            {
                runSpace = OpenRunspace();

                string gpoName = string.Format("{0}-mapped-drives", organizationId);

                //create new gpo 
                Command cmd = new Command("Remove-GPO");
                cmd.Parameters.Add("Name", gpoName);

                ExecuteShellCommand(runSpace, cmd);
            }
            catch (Exception)
            {
                CloseRunspace(runSpace);

                throw;
            }
            finally
            {
                CloseRunspace(runSpace);

                HostedSolutionLog.LogEnd("DeleteMappedDrivesGPOInternal");
            }
        }

        public void SetDriveMapsTargetingFilter(string organizationId, ExchangeAccount[] accounts, string path)
        {
            SetDriveMapsTargetingFilterInternal(organizationId, accounts, path);
        }

        internal void SetDriveMapsTargetingFilterInternal(string organizationId, ExchangeAccount[] accounts, string locationPath)
        {
            HostedSolutionLog.LogStart("SetDriveMapsTargetingFilterInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);
            HostedSolutionLog.DebugInfo("folderName : {0}", locationPath);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            if (string.IsNullOrEmpty(locationPath))
                throw new ArgumentNullException("folderName");

             try
             {
                 Dictionary<string, ExchangeAccount> sidAccountPairs = new Dictionary<string, ExchangeAccount>();

                 foreach (var account in accounts)
                 {
                     string path = IsGroup(account) ? GetGroupPath(organizationId, account.AccountName) : GetUserPath(organizationId, account.AccountName);

                     DirectoryEntry entry = ActiveDirectoryUtils.GetADObject(path);

                     byte[] sidByteArr = (byte[])ActiveDirectoryUtils.GetADObjectProperty(entry, ADAttributes.SID);

                     string sid = new System.Security.Principal.SecurityIdentifier(sidByteArr, 0).ToString();

                     sidAccountPairs.Add(sid, account);
                 }

                 string gpoId;

                 if (!CheckMappedDriveGpoExists(organizationId, out gpoId))
                 {
                     CreateAndLinkMappedDrivesGPO(organizationId, out gpoId);
                 }

                 if (!string.IsNullOrEmpty(gpoId))
                 {
                     string drivesXmlPath = string.Format("{0}\\{1}",
                                                          string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId),
                                                          "Drives.xml");
                     // open xml document
                     var xml = GetOrCreateDrivesFile(gpoId);

                     XmlNodeList drives = xml.SelectNodes(string.Format("./Drives/Drive[contains(Properties/@path,'{0}')]", locationPath));

                     foreach (XmlNode driveNode in drives)
                     {
                         XmlNodeList childNodes = driveNode.ChildNodes;

                         if (childNodes.Count > 1)
                         {
                             //delete existing filters
                             driveNode.LastChild.ParentNode.RemoveChild(driveNode.LastChild);
                         }

                         XmlNode newFiltersNode = CreateFiltersNode(xml, sidAccountPairs);

                         driveNode.AppendChild(newFiltersNode);
                     }

                     xml.Save(drivesXmlPath);

                     IncrementGPOVersion(organizationId, gpoId);
                 }
             }
             catch (Exception)
             {
                 throw;
             }
             finally
             {
                 HostedSolutionLog.LogEnd("SetDriveMapsTargetingFilterInternal");
             }
        }

        public void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder)
        {
            ChangeDriveMapFolderPathInternal(organizationId, oldFolder, newFolder);
        }

        internal void ChangeDriveMapFolderPathInternal(string organizationId, string oldFolder, string newFolder)
        {
            HostedSolutionLog.LogStart("ChangeDriveMapFolderPathInternal");
            HostedSolutionLog.DebugInfo("organizationId : {0}", organizationId);

            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            Runspace runSpace = null;

            try
            {
                runSpace = OpenRunspace();

                string gpoId;

                 if (!CheckMappedDriveGpoExists(organizationId, out gpoId))
                 {
                     CreateAndLinkMappedDrivesGPO(organizationId, out gpoId);
                 }

                 if (!string.IsNullOrEmpty(gpoId))
                 {
                     string drivesXmlPath = string.Format("{0}\\{1}",
                                                          string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId),
                                                          "Drives.xml");
                     // open xml document
                     var xml = GetOrCreateDrivesFile(gpoId);

                     XmlNodeList drives = xml.SelectNodes(string.Format("./Drives/Drive[contains(Properties/@path,'{0}')]", oldFolder));

                     foreach (XmlNode driveNode in drives)
                     {
                         if (driveNode.ChildNodes.Count > 1)
                         {
                             string oldPath = driveNode.FirstChild.Attributes["path"].Value;

                             driveNode.FirstChild.Attributes["path"].Value = oldPath.Replace(oldFolder, newFolder);
                         }
                     }

                     xml.Save(drivesXmlPath);

                     IncrementGPOVersion(organizationId, gpoId);
                 }
            }
            catch (Exception)
            {
                CloseRunspace(runSpace);

                throw;
            }
            finally
            {
                CloseRunspace(runSpace);

                HostedSolutionLog.LogEnd("ChangeDriveMapFolderPathInternal");
            }
        }

        private void CreateAndLinkMappedDrivesGPO(string organizationId, out string gpoId)
        {
            Runspace runSpace = null;

            try
            {
                runSpace = OpenRunspace();

                string gpoName = string.Format("{0}-mapped-drives", organizationId);
                string pathOU = GetOrganizationTargetPath(organizationId);

                //create new gpo 
                Command cmd = new Command("New-GPO");
                cmd.Parameters.Add("Name", gpoName);

                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

                gpoId = null;

                if (result != null && result.Count > 0)
                {
                    PSObject gpo = result[0];
                    //get gpo id
                    gpoId = ((Guid)GetPSObjectProperty(gpo, "Id")).ToString("B");

                }

                //create gpo link
                cmd = new Command("New-GPLink");
                cmd.Parameters.Add("Name", gpoName);
                cmd.Parameters.Add("Target", pathOU);

                ExecuteShellCommand(runSpace, cmd);

                //create empty drives.xml file for for gpo drives mapping
                CreateDrivesXmlEmpty(string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId), "Drives.xml");
            }
            catch (Exception)
            {
                gpoId = null;
                CloseRunspace(runSpace);

                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }
        }

        private bool CheckMappedDriveGpoExists(string organizationId, out string gpoId)
        {
            Runspace runSpace = null;

            try
            {
                runSpace = OpenRunspace();

                string gpoName = string.Format("{0}-mapped-drives", organizationId);

                Command cmd = new Command("Get-GPO");
                cmd.Parameters.Add("Name", gpoName);

                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

                gpoId = null;

                if (result != null && result.Count > 0)
                {
                    PSObject gpo = result[0];
                    gpoId = ((Guid)GetPSObjectProperty(gpo, "Id")).ToString("B");
                }
            }
            catch (Exception)
            {
                gpoId = null;
                CloseRunspace(runSpace);

                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            return string.IsNullOrEmpty(gpoId) ? false : true;
        }

        private bool CheckMappedDriveExists(string organizationId, string path, bool newDrive)
        {
            bool exists = false;

            MappedDrive[] drives = GetDriveMapsInternal(organizationId, newDrive);

            foreach (var item in drives)
            {
                if (item.Path == path)
                {
                    exists = true;
                    break;
                }
            }

            if (drives.Length == 0 && newDrive)
            {
                SetGPCUserExtensionNames(organizationId);
            }

            return exists;
        }

        private void SetGPCUserExtensionNames(string organizationId)
        {
            string gpoName = string.Format("{0}-mapped-drives", organizationId);

            DirectoryEntry de = ActiveDirectoryUtils.GetGroupPolicyContainer(gpoName);

            ActiveDirectoryUtils.SetADObjectProperty(de, "gPCUserExtensionNames", gPCUserExtensionNames);

            de.CommitChanges();
        }

        private void SetGPCVersionNumber(string organizationId, int version)
        {
            string gpoName = string.Format("{0}-mapped-drives", organizationId);

            DirectoryEntry de = ActiveDirectoryUtils.GetGroupPolicyContainer(gpoName);

            ActiveDirectoryUtils.SetADObjectProperty(de, "versionNumber", version.ToString());

            de.CommitChanges();
        }

        private void IncrementGPOVersion(string organizationId, string gpoId)
        {
            string path = string.Format("{0}\\{1}",
                                           string.Format(GROUP_POLICY_MAPPED_DRIVES_ROOT_PATH_TEMPLATE, RootDomain, gpoId),
                                           "GPT.ini");

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                int version = int.Parse(lines.Where(x => x.Contains("Version=")).FirstOrDefault().Replace("Version=", ""));

                string hexVersionValue = version.ToString("X");

                int userVersion = (version == 0) ? 0 : int.Parse(hexVersionValue.Substring(0, hexVersionValue.Length - 4), System.Globalization.NumberStyles.HexNumber);

                userVersion++;

                string userHexVersionValue = userVersion.ToString("X");
                string conputerHexVersion = (version == 0) ? "0000" : hexVersionValue.Substring(hexVersionValue.Length - 4, 4);

                hexVersionValue = userHexVersionValue + conputerHexVersion;

                int newVersion = int.Parse(hexVersionValue, System.Globalization.NumberStyles.HexNumber);

                lines[1] = string.Format("Version={0}", newVersion);

                File.WriteAllLines(path, lines);

                SetGPCVersionNumber(organizationId, newVersion);
            }
        }

        #region Drive Mapping Helpers

        private void CreateDrivesXmlEmpty(string path, string fileName)
        {
            DirectoryInfo drivesDirectory = new DirectoryInfo(path);

            if (!drivesDirectory.Exists)
            {
                drivesDirectory.Create();
            }

            XmlDocument doc = new XmlDocument();

            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode drivesNode = doc.CreateElement("Drives");

            XmlAttribute drivesAttribute = doc.CreateAttribute("clsid");
            drivesAttribute.Value = DRIVES_CLSID;
            drivesNode.Attributes.Append(drivesAttribute);

            doc.AppendChild(drivesNode);

            doc.Save(string.Format("{0}\\{1}", path, fileName));
        }

        private XmlNode CreateDriveNode(XmlDocument xml, string drive, string labelAs, string path)
        {
            XmlNode driveNode = xml.CreateElement("Drive");
            XmlNode propertiesNode = xml.CreateElement("Properties");;

            XmlAttribute clsidAttr = xml.CreateAttribute("clsid");
            XmlAttribute nameAttr = xml.CreateAttribute("name");
            XmlAttribute statusAttr = xml.CreateAttribute("status");
            XmlAttribute imageAttr = xml.CreateAttribute("image");
            XmlAttribute changedAttr = xml.CreateAttribute("changed");
            XmlAttribute uidAttr = xml.CreateAttribute("uid");
            XmlAttribute bypassErrorsAttr = xml.CreateAttribute("bypassErrors");

            XmlAttribute actionPropAttr = xml.CreateAttribute("action");
            XmlAttribute thisDrivePropAttr = xml.CreateAttribute("thisDrive");
            XmlAttribute allDrivesPropAttr = xml.CreateAttribute("allDrives");
            XmlAttribute userNamePropAttr = xml.CreateAttribute("userName");
            XmlAttribute pathPropAttr = xml.CreateAttribute("path");
            XmlAttribute labelPropAttr = xml.CreateAttribute("label");
            XmlAttribute persistentPropAttr = xml.CreateAttribute("persistent");
            XmlAttribute useLetterPropAttr = xml.CreateAttribute("useLetter");
            XmlAttribute letterPropAttr = xml.CreateAttribute("letter");

            clsidAttr.Value = DRIVE_CLSID;
            nameAttr.Value = string.Format("{0}:", drive);
            statusAttr.Value = string.Format("{0}:", drive);
            imageAttr.Value = (1).ToString();
            changedAttr.Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            uidAttr.Value = Guid.NewGuid().ToString("B");
            bypassErrorsAttr.Value = (1).ToString();

            actionPropAttr.Value = "C";
            thisDrivePropAttr.Value = "NOCHANGE";
            allDrivesPropAttr.Value = "NOCHANGE";
            userNamePropAttr.Value = string.Empty;
            pathPropAttr.Value = path;
            labelPropAttr.Value = labelAs;
            persistentPropAttr.Value = (1).ToString();
            useLetterPropAttr.Value = (1).ToString();
            letterPropAttr.Value = drive;

            driveNode.Attributes.Append(clsidAttr);
            driveNode.Attributes.Append(nameAttr);
            driveNode.Attributes.Append(statusAttr);
            driveNode.Attributes.Append(imageAttr);
            driveNode.Attributes.Append(changedAttr);
            driveNode.Attributes.Append(uidAttr);
            driveNode.Attributes.Append(bypassErrorsAttr);

            propertiesNode.Attributes.Append(actionPropAttr);
            propertiesNode.Attributes.Append(thisDrivePropAttr);
            propertiesNode.Attributes.Append(allDrivesPropAttr);
            propertiesNode.Attributes.Append(userNamePropAttr);
            propertiesNode.Attributes.Append(pathPropAttr);
            propertiesNode.Attributes.Append(labelPropAttr);
            propertiesNode.Attributes.Append(persistentPropAttr);
            propertiesNode.Attributes.Append(useLetterPropAttr);
            propertiesNode.Attributes.Append(letterPropAttr);

            driveNode.AppendChild(propertiesNode);

            return driveNode;
        }

        private XmlNode CreateFiltersNode(XmlDocument xml, Dictionary<string, ExchangeAccount> accounts)
        {
            XmlNode filtersNode = xml.CreateElement("Filters");

            foreach (var pair in accounts)
            {
                XmlAttribute boolAttr = xml.CreateAttribute("bool");
                XmlAttribute notAttr = xml.CreateAttribute("not");
                XmlAttribute userContextAttr = xml.CreateAttribute("userContext");
                XmlAttribute primaryGroupAttr = xml.CreateAttribute("primaryGroup");
                XmlAttribute localGroupAttr = xml.CreateAttribute("localGroup");
                XmlAttribute nameAttr = xml.CreateAttribute("name");
                XmlAttribute sidAttr = xml.CreateAttribute("sid");

                boolAttr.Value = "OR";
                notAttr.Value = (0).ToString();
                userContextAttr.Value = (1).ToString();
                primaryGroupAttr.Value = (0).ToString();
                localGroupAttr.Value = (0).ToString();

                if (IsGroup(pair.Value))
                {
                    XmlNode filterGroupNode = xml.CreateElement("FilterGroup");

                    nameAttr.Value = GetObjectTargetAccountName(pair.Value.AccountName, RootDomain);
                    sidAttr.Value = pair.Key;

                    filterGroupNode.Attributes.Append(boolAttr);
                    filterGroupNode.Attributes.Append(notAttr);
                    filterGroupNode.Attributes.Append(nameAttr);
                    filterGroupNode.Attributes.Append(sidAttr);
                    filterGroupNode.Attributes.Append(userContextAttr);
                    filterGroupNode.Attributes.Append(primaryGroupAttr);
                    filterGroupNode.Attributes.Append(localGroupAttr);

                    filtersNode.AppendChild(filterGroupNode);
                }
                else
                {
                    XmlNode filterUserNode = xml.CreateElement("FilterUser");

                    nameAttr.Value = GetObjectTargetAccountName(pair.Value.AccountName, RootDomain);
                    sidAttr.Value = pair.Key;

                    filterUserNode.Attributes.Append(boolAttr);
                    filterUserNode.Attributes.Append(notAttr);
                    filterUserNode.Attributes.Append(nameAttr);
                    filterUserNode.Attributes.Append(sidAttr);

                    filtersNode.AppendChild(filterUserNode);
                }
            }

            return filtersNode;
        }

        private MappedDrive[] GetDrivesFromXML(XmlDocument xml, ArrayList items)
        {
            XmlNodeList drives = xml.SelectNodes("./Drives/Drive");

            foreach (XmlNode driveNode in drives)
            {
                XmlNode props = driveNode.ChildNodes[0];

                MappedDrive item = new MappedDrive(props.Attributes["path"].Value,
                                                   props.Attributes["label"].Value,
                                                   props.Attributes["letter"].Value);
                items.Add(item);
            }

            return (MappedDrive[])items.ToArray(typeof(MappedDrive));
        }

        private bool IsGroup(ExchangeAccount account)
        {
            return (account.AccountType == ExchangeAccountType.SecurityGroup || account.AccountType == ExchangeAccountType.DefaultSecurityGroup);
        }

        private XmlDocument GetOrCreateDrivesFile(string gpoId)
        {
            string path = string.Format("{0}\\{1}",
                string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId),
                "Drives.xml");

            if (!File.Exists(path))
            {
                CreateDrivesXmlEmpty(string.Format(GROUP_POLICY_MAPPED_DRIVES_FILE_PATH_TEMPLATE, RootDomain, gpoId), "Drives.xml");
            }

            // open xml document
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            return xml;
        }

        #endregion

        #endregion

        #region PowerShell integration

        internal void ImportGroupPolicyMolude(Runspace runSpace)
        {
            Command cmd = new Command("Import-Module");
            cmd.Parameters.Add("Name", "grouppolicy");

            ExecuteShellCommand(runSpace, cmd);
        }

        private static RunspaceConfiguration runspaceConfiguration = null;

        internal virtual Runspace OpenRunspace()
        {
            HostedSolutionLog.LogStart("OpenRunspace");

            if (runspaceConfiguration == null)
            {
                runspaceConfiguration = RunspaceConfiguration.Create();
            }

            Runspace runSpace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            //
            runSpace.Open();
            //
            runSpace.SessionStateProxy.SetVariable("ConfirmPreference", "none");

            ImportGroupPolicyMolude(runSpace);

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
                    runspaceConfiguration = null;
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("Runspace error", ex);
            }
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd)
        {
            return ExecuteShellCommand(runSpace, cmd, false);
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

        #endregion

        public override bool IsInstalled()
        {
            return Environment.UserDomainName != Environment.MachineName;
        }
    }
}
