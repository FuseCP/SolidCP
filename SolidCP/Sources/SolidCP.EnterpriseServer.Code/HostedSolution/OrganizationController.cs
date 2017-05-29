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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using SolidCP.EnterpriseServer.Base;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.EnterpriseServer.Code.SharePoint;
using SolidCP.EnterpriseServer.Extensions;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.SharePoint;
using SolidCP.Providers.Common;
using SolidCP.Providers.DNS;
using SolidCP.Providers.OCS;
using System.Linq;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.OS;
using System.Text.RegularExpressions;
using SolidCP.Server.Client;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
    public class OrganizationController
    {
        public const string TemporyDomainName = "TempDomain";
        public const string UseStorageSpaces = "UseStorageSpaces";
        private static readonly OrganizationFoldersManager _foldersManager;

        static OrganizationController()
        {
            _foldersManager = new OrganizationFoldersManager();
        }

        private static bool CheckUserQuota(int orgId, out int errorCode)
        {
            errorCode = 0;
            OrganizationStatistics stats = GetOrganizationStatisticsByOrganization(orgId);


            if (stats.AllocatedUsers != -1 && (stats.CreatedUsers >= stats.AllocatedUsers))
            {
                errorCode = BusinessErrorCodes.ERROR_USERS_RESOURCE_QUOTA_LIMIT;
                return false;
            }

            return true;
        }

        private static bool CheckDeletedUserQuota(int orgId, out int errorCode)
        {
            errorCode = 0;
            OrganizationStatistics stats = GetOrganizationStatisticsByOrganization(orgId);


            if (stats.AllocatedDeletedUsers != -1 && (stats.DeletedUsers >= stats.AllocatedDeletedUsers))
            {
                errorCode = BusinessErrorCodes.ERROR_DELETED_USERS_RESOURCE_QUOTA_LIMIT;
                return false;
            }

            return true;
        }

        private static string EvaluateMailboxTemplate(int itemId, int accountId,
            bool pmm, bool emailMode, bool signup, string template)
        {
            Hashtable items = new Hashtable();

            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return null;
            // add organization
            items["Organization"] = org;
            OrganizationUser user = GetAccount(itemId, accountId);

            items["account"] = user;


            // evaluate template
            return PackageController.EvaluateTemplate(template, items);
        }

        public static string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return null;

            // load user info
            UserInfo user = PackageController.GetPackageOwner(org.PackageId);

            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.ORGANIZATION_USER_SUMMARY_LETTER);

            string settingName = user.HtmlMail ? "HtmlBody" : "TextBody";
            string body = settings[settingName];
            if (String.IsNullOrEmpty(body))
                return null;

            string result = EvaluateMailboxTemplate(itemId, accountId, pmm, false, false, body);
            return user.HtmlMail ? result : result.Replace("\n", "<br/>");
        }

        public static int SendSummaryLetter(int itemId, int accountId, bool signup, string to, string cc)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return -1;

            // load user info
            UserInfo user = PackageController.GetPackageOwner(org.PackageId);

            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.ORGANIZATION_USER_SUMMARY_LETTER);

            string from = settings["From"];
            if (cc == null)
                cc = settings["CC"];
            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;
            if (!String.IsNullOrEmpty(settings["Priority"]))
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

            if (String.IsNullOrEmpty(body))
                return 0;// BusinessErrorCodes.ERROR_SETTINGS_ACCOUNT_LETTER_EMPTY_BODY;

            // load user info
            if (to == null)
                to = user.Email;

            subject = EvaluateMailboxTemplate(itemId, accountId, false, true, signup, subject);
            body = EvaluateMailboxTemplate(itemId, accountId, false, true, signup, body);

            // send message
            return MailHelper.SendMessage(from, to, cc, subject, body, priority, isHtml);
        }

        private static bool CheckQuotas(int packageId, out int errorCode)
        {

            // check account
            errorCode = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (errorCode < 0) return false;

            // check package
            errorCode = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (errorCode < 0) return false;

            // check organizations quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(packageId, Quotas.ORGANIZATIONS);
            if (quota.QuotaExhausted)
            {
                errorCode = BusinessErrorCodes.ERROR_ORGS_RESOURCE_QUOTA_LIMIT;
                return false;
            }


            // check sub-domains quota (for temporary domain)
            quota = PackageController.GetPackageQuota(packageId, Quotas.OS_SUBDOMAINS);
            if (quota.QuotaExhausted)
            {
                errorCode = BusinessErrorCodes.ERROR_SUBDOMAIN_QUOTA_LIMIT;
                return false;
            }


            return true;
        }

        private static string CreateTemporyDomainName(int serviceId, string organizationId)
        {
            // load service settings
            StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);

            string tempDomain = serviceSettings[TemporyDomainName];
            return String.IsNullOrEmpty(tempDomain) ? null : organizationId + "." + tempDomain;
        }

        private static DomainInfo CreateNewDomain(int packageId, string domainName)
        {
            // new domain
            DomainInfo domain = new DomainInfo();
            domain.PackageId = packageId;
            domain.DomainName = domainName;
            domain.IsInstantAlias = true;
            domain.IsSubDomain = true;

            return domain;
        }

        private static int CreateDomain(string domainName, int packageId, out bool domainCreated)
        {
            // trying to locate (register) temp domain
            DomainInfo domain = null;
            int domainId = 0;
            domainCreated = false;

            // check if the domain already exists
            int checkResult = ServerController.CheckDomain(domainName);
            if (checkResult == BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS)
            {
                // domain exists
                // check if it belongs to the same space
                domain = ServerController.GetDomain(domainName);
                if (domain == null)
                    return checkResult;

                if (domain.PackageId != packageId)
                    return checkResult;

                if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
                    return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;

                domainId = domain.DomainId;
            }
            else if (checkResult < 0)
            {
                return checkResult;
            }

            // create domain if required
            if (domain == null)
            {
                domain = CreateNewDomain(packageId, domainName);
                // add SolidCP domain
                domainId = ServerController.AddDomain(domain);

                if (domainId < 0)
                    return domainId;

                domainCreated = true;
            }

            return domainId;
        }

        private static int AddOrganizationToPackageItems(Organization org, int serviceId, int packageId, string organizationName, string organizationId, string domainName)
        {
            org.ServiceId = serviceId;
            org.PackageId = packageId;
            org.Name = organizationName;
            org.OrganizationId = organizationId;
            org.DefaultDomain = domainName;

            int itemId = PackageController.AddPackageItem(org);


            return itemId;
        }

        public static bool OrganizationIdentifierExists(string organizationId)
        {
            return DataProvider.ExchangeOrganizationExists(organizationId);
        }

        private static void RollbackOrganization(int packageId, string organizationId)
        {
            try
            {
                int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedOrganizations);
                Organizations orgProxy = GetOrganizationProxy(serviceId);
                orgProxy.DeleteOrganization(organizationId);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
        }

        public static int CreateOrganization(int packageId, string organizationId, string organizationName, string domainName)
        {
            int itemId = 0;
            int errorCode;
            if (!CheckQuotas(packageId, out errorCode))
                return errorCode;

            // place log record
            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            parameters.Add(new BackgroundTaskParameter("Organization ID", organizationId));
            parameters.Add(new BackgroundTaskParameter("DomainName", domainName));

            TaskManager.StartTask("ORGANIZATION", "CREATE_ORG", organizationName, parameters);

            try
            {
                // Check if organization exitsts.                
                if (OrganizationIdentifierExists(organizationId))
                    return BusinessErrorCodes.ERROR_ORG_ID_EXISTS;

                // Create Organization Unit
                int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedOrganizations);

                Organizations orgProxy = GetOrganizationProxy(serviceId);
                Organization org = null;
                if (!orgProxy.OrganizationExists(organizationId))
                {
                    org = orgProxy.CreateOrganization(organizationId);
                }
                else
                    return BusinessErrorCodes.ERROR_ORG_ID_EXISTS;

                //create temporary domain name;
                if (string.IsNullOrEmpty(domainName))
                {
                    string tmpDomainName = CreateTemporyDomainName(serviceId, organizationId);

                    if (!string.IsNullOrEmpty(tmpDomainName)) domainName = tmpDomainName;
                }

                if (string.IsNullOrEmpty(domainName))
                {
                    domainName = organizationName;
                    //RollbackOrganization(packageId, organizationId);
                    //return BusinessErrorCodes.ERROR_ORGANIZATION_TEMP_DOMAIN_IS_NOT_SPECIFIED;
                }


                bool domainCreated;
                int domainId = CreateDomain(domainName, packageId, out domainCreated);
                //create domain 
                if (domainId < 0)
                {
                    RollbackOrganization(packageId, organizationId);
                    return domainId;
                }

                DomainInfo domain = ServerController.GetDomain(domainId);
                if (domain != null)
                {
                    if (domain.ZoneItemId != 0)
                    {
                        ServerController.AddServiceDNSRecords(org.PackageId, ResourceGroups.HostedOrganizations, domain, "");
                        ServerController.AddServiceDNSRecords(org.PackageId, ResourceGroups.HostedCRM, domain, "");
                    }
                }


                PackageContext cntx = PackageController.GetPackageContext(packageId);

                if (cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE] != null)
                    org.MaxSharePointStorage = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE].QuotaAllocatedValue;


                if (cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE] != null)
                    org.WarningSharePointStorage = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE].QuotaAllocatedValue;

                if (cntx.Quotas[Quotas.HOSTED_SHAREPOINT_ENTERPRISE_STORAGE_SIZE] != null)
                    org.MaxSharePointEnterpriseStorage = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_ENTERPRISE_STORAGE_SIZE].QuotaAllocatedValue;


                if (cntx.Quotas[Quotas.HOSTED_SHAREPOINT_ENTERPRISE_STORAGE_SIZE] != null)
                    org.WarningSharePointEnterpriseStorage = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_ENTERPRISE_STORAGE_SIZE].QuotaAllocatedValue;


                //add organization to package items                
                itemId = AddOrganizationToPackageItems(org, serviceId, packageId, organizationName, organizationId, domainName);

                // register org ID
                DataProvider.AddExchangeOrganization(itemId, organizationId);

                // register domain                
                DataProvider.AddExchangeOrganizationDomain(itemId, domainId, true);

                //add to exchangeAcounts
                AddAccount(itemId, ExchangeAccountType.DefaultSecurityGroup, org.GroupName,
                                org.GroupName, null, false,
                                0, org.GroupName, null, 0, null);


                // load user info
                UserInfo user = PackageController.GetPackageOwner(packageId);

                // get letter settings
                UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.EXCHANGE_POLICY);

                bool enableAdditionalGroup = false;
                try
                {
                    // parse settings
                    enableAdditionalGroup = Utils.ParseBool(settings["OrgPolicy"], false);
                }
                catch { /* skip */ }

                if (enableAdditionalGroup)
                {
                    foreach (AdditionalGroup additionalGroup in GetAdditionalGroups(settings.UserId))
                    {
                        string additionalGroupName = BuildAccountNameEx(org, additionalGroup.GroupName.Replace(" ", ""));

                        if (orgProxy.CreateSecurityGroup(org.OrganizationId, additionalGroupName) == 0)
                        {
                            OrganizationSecurityGroup retSecurityGroup = orgProxy.GetSecurityGroupGeneralSettings(additionalGroupName, org.OrganizationId);

                            AddAccount(itemId, ExchangeAccountType.SecurityGroup, additionalGroupName,
                                additionalGroup.GroupName, null, false, 0, retSecurityGroup.SAMAccountName, null, 0, null);
                        }
                    }
                }

                // register organization domain service item
                OrganizationDomain orgDomain = new OrganizationDomain
                {
                    Name = domainName,
                    PackageId = packageId,
                    ServiceId = serviceId
                };

                PackageController.AddPackageItem(orgDomain);

                ExchangeServerController.CreateOrganizationRootPublicFolder(itemId);

            }
            catch (Exception ex)
            {
                //rollback organization
                try
                {
                    if (itemId <= 0)
                        RollbackOrganization(packageId, organizationId);
                    else
                        DeleteOrganization(itemId);
                }
                catch (Exception rollbackException)
                {
                    TaskManager.WriteError(rollbackException);
                }

                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return itemId;
        }

        public static int DeleteOrganizationDomain(int itemId, int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "DELETE_DOMAIN", itemId, new BackgroundTaskParameter("Domain ID", domainId));

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // load domain
                DomainInfo domain = ServerController.GetDomain(domainId);
                if (domain == null)
                    return -1;
                
                // Log Extension
                LogExtension.SetItemName(domain.DomainName);
                LogExtension.WriteObject(domain);

                if (!string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    if (DataProvider.CheckDomainUsedByHostedOrganization(domain.DomainName) == 1)
                    {
                        return -1;
                    }
                }

                // unregister domain
                DataProvider.DeleteExchangeOrganizationDomain(itemId, domainId);

                // remove service item
                ServiceProviderItem itemDomain = PackageController.GetPackageItemByName(
                    org.PackageId, domain.DomainName, typeof(OrganizationDomain));
                if (itemDomain != null)
                    PackageController.DeletePackageItem(itemDomain.Id);


                /*Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                orgProxy.CreateOrganizationDomain(org.DistinguishedName, domain.DomainName);*/
                if (!string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExchangeServerController.DeleteAuthoritativeDomain(itemId, domainId);
                }

                if (org.IsOCSOrganization)
                {
                    OCSController.DeleteDomain(itemId, domain.DomainName);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static bool CheckDomainUsedByHostedOrganization(int itemId, int domainId)
        {
            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain == null)
                return false;

            return (DataProvider.CheckDomainUsedByHostedOrganization(domain.DomainName) == 1);
        }

        private static void DeleteOCSUsers(int itemId, ref bool successful)
        {
            try
            {
                OCSUsersPagedResult res = OCSController.GetOCSUsers(itemId, string.Empty, string.Empty, string.Empty,
                                                        string.Empty, 0, int.MaxValue);
                if (res.IsSuccess)
                {
                    foreach (OCSUser user in res.Value.PageUsers)
                    {
                        try
                        {
                            ResultObject delUserResult = OCSController.DeleteOCSUser(itemId, user.InstanceId);
                            if (!delUserResult.IsSuccess)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (string str in delUserResult.ErrorCodes)
                                {
                                    sb.Append(str);
                                    sb.Append('\n');
                                }

                                throw new ApplicationException(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            successful = false;
                            TaskManager.WriteError(ex);
                        }
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string str in res.ErrorCodes)
                    {
                        sb.Append(str);
                        sb.Append('\n');
                    }

                    throw new ApplicationException(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                successful = false;
                TaskManager.WriteError(ex);
            }
        }


        private static bool DeleteLyncUsers(int itemId)
        {
            bool successful = false;

            try
            {
                LyncUsersPagedResult res = LyncController.GetLyncUsers(itemId);

                if (res.IsSuccess)
                {
                    successful = true;
                    foreach (LyncUser user in res.Value.PageUsers)
                    {
                        try
                        {
                            ResultObject delUserResult = LyncController.DeleteLyncUser(itemId, user.AccountID);
                            if (!delUserResult.IsSuccess)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (string str in delUserResult.ErrorCodes)
                                {
                                    sb.Append(str);
                                    sb.Append('\n');
                                }

                                throw new ApplicationException(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            successful = false;
                            TaskManager.WriteError(ex);
                        }
                    }

                    return successful;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string str in res.ErrorCodes)
                    {
                        sb.Append(str);
                        sb.Append('\n');
                    }

                    throw new ApplicationException(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                successful = false;
                TaskManager.WriteError(ex);
            }

            return successful;
        }

        private static bool DeleteSfBUsers(int itemId)
        {
            bool successful = false;

            try
            {
                SfBUsersPagedResult res = SfBController.GetSfBUsers(itemId);

                if (res.IsSuccess)
                {
                    successful = true;
                    foreach (SfBUser user in res.Value.PageUsers)
                    {
                        try
                        {
                            ResultObject delUserResult = SfBController.DeleteSfBUser(itemId, user.AccountID);
                            if (!delUserResult.IsSuccess)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (string str in delUserResult.ErrorCodes)
                                {
                                    sb.Append(str);
                                    sb.Append('\n');
                                }

                                throw new ApplicationException(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            successful = false;
                            TaskManager.WriteError(ex);
                        }
                    }

                    return successful;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string str in res.ErrorCodes)
                    {
                        sb.Append(str);
                        sb.Append('\n');
                    }

                    throw new ApplicationException(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                successful = false;
                TaskManager.WriteError(ex);
            }

            return successful;
        }

        public static int DeleteOrganization(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "DELETE_ORG", itemId);

            try
            {
                bool successful = true;
                Organization org = GetOrganization(itemId);

                try
                {
                    HostedSharePointServerController.DeleteSiteCollections(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                try
                {
                    HostedSharePointServerEntController.DeleteSiteCollections(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                if (org.IsOCSOrganization)
                {
                    DeleteOCSUsers(itemId, ref successful);
                }

                try
                {
                    if (org.CrmOrganizationId != Guid.Empty)
                        CRMController.DeleteOrganization(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                try
                {
                    OrganizationUsersPagedResult res = BlackBerryController.GetBlackBerryUsers(itemId, string.Empty, string.Empty, string.Empty,
                                                            string.Empty, 0, int.MaxValue);
                    if (res.IsSuccess)
                    {
                        foreach (OrganizationUser user in res.Value.PageUsers)
                        {
                            try
                            {
                                ResultObject delUserResult = BlackBerryController.DeleteBlackBerryUser(itemId, user.AccountId);
                                if (!delUserResult.IsSuccess)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    foreach (string str in delUserResult.ErrorCodes)
                                    {
                                        sb.Append(str);
                                        sb.Append('\n');
                                    }

                                    throw new ApplicationException(sb.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                successful = false;
                                TaskManager.WriteError(ex);
                            }
                        }
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (string str in res.ErrorCodes)
                        {
                            sb.Append(str);
                            sb.Append('\n');
                        }

                        throw new ApplicationException(sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                //Cleanup Lync
                try
                {
                    if (!string.IsNullOrEmpty(org.LyncTenantId))
                        if (DeleteLyncUsers(itemId))
                            LyncController.DeleteOrganization(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                //Cleanup SfB
                try
                {
                    if (!string.IsNullOrEmpty(org.SfBTenantId))
                        if (DeleteSfBUsers(itemId))
                            SfBController.DeleteOrganization(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                //Cleanup Enterprise storage

                if (EnterpriseStorageController.DeleteEnterpriseStorage(org.PackageId, itemId).IsSuccess == false)
                {
                    successful = false;
                }

                //Cleanup RDS

                if (RemoteDesktopServicesController.DeleteRemoteDesktopService(itemId).IsSuccess == false)
                {
                    successful = false;
                }

                //Cleanup Exchange
                try
                {
                    if (!string.IsNullOrEmpty(org.GlobalAddressList))
                        ExchangeServerController.DeleteOrganization(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                //Cleanup OrganizationFolders
                try
                {
                    if (_foldersManager.DeleteFolders(itemId).IsSuccess == false)
                    {
                        successful = false;
                    }
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }


                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                try
                {
                    orgProxy.DeleteOrganization(org.OrganizationId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                // delete organization domains
                List<OrganizationDomainName> domains = GetOrganizationDomains(itemId);
                foreach (OrganizationDomainName domain in domains)
                {
                    try
                    {
                        DeleteOrganizationDomain(itemId, domain.DomainId);
                    }
                    catch (Exception ex)
                    {
                        successful = false;
                        TaskManager.WriteError(ex);
                    }

                }

                DataProvider.DeleteOrganizationUser(itemId);

                // delete meta-item
                PackageController.DeletePackageItem(itemId);

                return successful ? 0 : BusinessErrorCodes.ERROR_ORGANIZATION_DELETE_SOME_PROBLEMS;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }


        public static Organizations GetOrganizationProxy(int serviceId)
        {
            Organizations ws = new Organizations();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }

        public static List<Organization> GetOrganizations(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(Organization), recursive);

            return items.ConvertAll<Organization>(
                delegate(ServiceProviderItem item) { return (Organization)item; });
        }

        public static DataSet GetRawOrganizationsPaged(int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                DataSet ds = new DataSet();

                // total records
                DataTable dtTotal = ds.Tables.Add();
                dtTotal.Columns.Add("Records", typeof(int));
                dtTotal.Rows.Add(2);

                // organizations
                DataTable dtItems = ds.Tables.Add();
                dtItems.Columns.Add("ItemID", typeof(int));
                dtItems.Columns.Add("OrganizationID", typeof(string));
                dtItems.Columns.Add("ItemName", typeof(string));
                dtItems.Columns.Add("PackageName", typeof(string));
                dtItems.Columns.Add("PackageID", typeof(int));
                dtItems.Columns.Add("Username", typeof(string));
                dtItems.Columns.Add("UserID", typeof(int));
                dtItems.Rows.Add(1, "fabrikam", "Fabrikam Inc", "Hosted Exchange", 1, "Customer", 1);


                dtItems.Rows.Add(2, "Contoso", "Contoso Ltd", "Hosted Exchange", 2, "Customer", 2);


                return ds;
            }
            #endregion


            return PackageController.GetRawPackageItemsPaged(
                   packageId, ResourceGroups.HostedOrganizations, typeof(Organization),
                   recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static Organization GetOrganizationById(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            int itemId = DataProvider.GetItemIdByOrganizationId(organizationId);

            Organization org = GetOrganization(itemId);

            return org;
        }

        public static Organization GetOrganization(int itemId, bool withLog = true)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                // load package by user
                Organization orgDemo = new Organization();
                orgDemo.PackageId = 0;
                orgDemo.Id = 1;
                orgDemo.OrganizationId = "fabrikam";
                orgDemo.Name = "Fabrikam Inc";
                orgDemo.KeepDeletedItemsDays = 14;
                orgDemo.GlobalAddressList = "FabrikamGAL";

                // Log Extension
                if (withLog)
                    LogExtension.WriteObject(orgDemo);

                return orgDemo;
            }
            #endregion

            var org = (Organization)PackageController.GetPackageItem(itemId);

            // Log Extension
            if (withLog)
                LogExtension.WriteObject(org);

            return org;
        }

        public static OrganizationStatistics GetOrganizationStatistics(int itemId)
        {
            return GetOrganizationStatisticsInternal(itemId, false);
        }

        public static OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId)
        {
            return GetOrganizationStatisticsInternal(itemId, true);
        }


        private static OrganizationStatistics GetOrganizationStatisticsInternal(int itemId, bool byOrganization)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                OrganizationStatistics stats = new OrganizationStatistics();
                stats.AllocatedMailboxes = 4;
                stats.CreatedMailboxes = 3;
                stats.AllocatedContacts = 4;
                stats.CreatedContacts = 2;
                stats.AllocatedDistributionLists = 5;
                stats.CreatedDistributionLists = 1;
                stats.AllocatedPublicFolders = 40;
                stats.CreatedPublicFolders = 4;
                stats.AllocatedDomains = 5;
                stats.CreatedDomains = 2;
                stats.AllocatedDiskSpace = 200;
                stats.UsedDiskSpace = 70;
                stats.CreatedUsers = 5;
                stats.AllocatedUsers = 10;
                stats.CreatedSharePointSiteCollections = 1;
                stats.CreatedSharePointEnterpriseSiteCollections = 1;
                stats.AllocatedSharePointSiteCollections = 5;
                stats.AllocatedSharePointEnterpriseSiteCollections = 5;
                return stats;
            }
            #endregion

            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_ORG_STATS", itemId);

            try
            {
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return null;

                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                OrganizationStatistics stats = new OrganizationStatistics();
                if (byOrganization)
                {
                    OrganizationStatistics tempStats = ObjectUtils.FillObjectFromDataReader<OrganizationStatistics>(DataProvider.GetOrganizationStatistics(org.Id));

                    stats.CreatedUsers = tempStats.CreatedUsers;
                    stats.CreatedDomains = tempStats.CreatedDomains;
                    stats.CreatedGroups = tempStats.CreatedGroups;
                    stats.DeletedUsers = tempStats.DeletedUsers;

                    PackageContext cntxTmp = PackageController.GetPackageContext(org.PackageId);

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
                    {
                        SharePointSiteCollectionListPaged sharePointStats = HostedSharePointServerController.GetSiteCollectionsPaged(org.PackageId, org.Id, string.Empty, string.Empty, string.Empty, 0, 0);
                        stats.CreatedSharePointSiteCollections = sharePointStats.TotalRowCount;
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
                    {
                        SharePointEnterpriseSiteCollectionListPaged sharePointStats = HostedSharePointServerEntController.GetSiteCollectionsPaged(org.PackageId, org.Id, string.Empty, string.Empty, string.Empty, 0, 0);
                        stats.CreatedSharePointEnterpriseSiteCollections = sharePointStats.TotalRowCount;
                    }


                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.HostedCRM))
                    {
                        stats.CreatedCRMUsers = CRMController.GetCRMUsersCount(org.Id, string.Empty, string.Empty, CRMUserLycenseTypes.FULL).Value;
                        stats.CreatedLimitedCRMUsers = CRMController.GetCRMUsersCount(org.Id, string.Empty, string.Empty, CRMUserLycenseTypes.LIMITED).Value;
                        stats.CreatedESSCRMUsers = CRMController.GetCRMUsersCount(org.Id, string.Empty, string.Empty, CRMUserLycenseTypes.ESS).Value;
                        stats.UsedCRMDiskSpace = CRMController.GetDBSize(org.Id, org.PackageId);
                        stats.AllocatedCRMDiskSpace = CRMController.GetMaxDBSize(org.Id, org.PackageId);
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                    {
                        stats.CreatedProfessionalCRMUsers = CRMController.GetCRMUsersCount(org.Id, string.Empty, string.Empty, CRMUserLycenseTypes.PROFESSIONAL).Value;
                        stats.CreatedBasicCRMUsers = CRMController.GetCRMUsersCount(org.Id, string.Empty, string.Empty, CRMUserLycenseTypes.BASIC).Value;
                        stats.CreatedEssentialCRMUsers = CRMController.GetCRMUsersCount(org.Id, string.Empty, string.Empty, CRMUserLycenseTypes.ESSENTIAL).Value;
                        stats.UsedCRMDiskSpace = CRMController.GetDBSize(org.Id, org.PackageId);
                        stats.AllocatedCRMDiskSpace = CRMController.GetMaxDBSize(org.Id, org.PackageId);
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.BlackBerry))
                    {
                        stats.CreatedBlackBerryUsers = BlackBerryController.GetBlackBerryUsersCount(org.Id, string.Empty, string.Empty).Value;
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.OCS))
                    {
                        stats.CreatedOCSUsers = OCSController.GetOCSUsersCount(org.Id, string.Empty, string.Empty).Value;
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.Lync))
                    {
                        stats.CreatedLyncUsers = LyncController.GetLyncUsersCount(org.Id).Value;
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.SfB))
                    {
                        stats.CreatedSfBUsers = SfBController.GetSfBUsersCount(org.Id).Value;
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.EnterpriseStorage))
                    {
                        SystemFile[] folders = EnterpriseStorageController.GetFolders(itemId);

                        stats.CreatedEnterpriseStorageFolders = folders.Count();

                        stats.UsedEnterpriseStorageSpace = folders.Where(x => x.FRSMQuotaMB != -1).Sum(x => x.FRSMQuotaMB);
                    }

                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.RDS))
                    {
                        stats.CreatedRdsUsers = RemoteDesktopServicesController.GetOrganizationRdsUsersCount(org.Id);
                        stats.CreatedRdsCollections = RemoteDesktopServicesController.GetOrganizationRdsCollectionsCount(org.Id);
                        stats.CreatedRdsServers = RemoteDesktopServicesController.GetOrganizationRdsServersCount(org.Id);
                    }

                    stats.ServiceLevels = GetServiceLevelQuotas(cntx, org.Id);
                }
                else
                {
                    UserInfo user = ObjectUtils.FillObjectFromDataReader<UserInfo>(DataProvider.GetUserByExchangeOrganizationIdInternally(org.Id));

                    List<PackageInfo> Packages = PackageController.GetPackages(user.UserId);

                    if ((Packages != null) & (Packages.Count > 0))
                    {
                        foreach (PackageInfo Package in Packages)
                        {
                            List<Organization> orgs = null;

                            orgs = ExchangeServerController.GetExchangeOrganizations(Package.PackageId, false);

                            if ((orgs != null) & (orgs.Count > 0))
                            {
                                foreach (Organization o in orgs)
                                {
                                    OrganizationStatistics tempStats = ObjectUtils.FillObjectFromDataReader<OrganizationStatistics>(DataProvider.GetOrganizationStatistics(o.Id));

                                    stats.CreatedUsers += tempStats.CreatedUsers;
                                    stats.CreatedDomains += tempStats.CreatedDomains;
                                    stats.CreatedGroups += tempStats.CreatedGroups;

                                    PackageContext cntxTmp = PackageController.GetPackageContext(org.PackageId);

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
                                    {
                                        SharePointSiteCollectionListPaged sharePointStats = HostedSharePointServerController.GetSiteCollectionsPaged(org.PackageId, o.Id, string.Empty, string.Empty, string.Empty, 0, 0);
                                        stats.CreatedSharePointSiteCollections += sharePointStats.TotalRowCount;
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
                                    {
                                        SharePointEnterpriseSiteCollectionListPaged sharePointStats = HostedSharePointServerEntController.GetSiteCollectionsPaged(org.PackageId, o.Id, string.Empty, string.Empty, string.Empty, 0, 0);
                                        stats.CreatedSharePointEnterpriseSiteCollections += sharePointStats.TotalRowCount;
                                    }                                    


                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.HostedCRM))
                                    {
                                        stats.CreatedCRMUsers += CRMController.GetCRMUsersCount(o.Id, string.Empty, string.Empty, CRMUserLycenseTypes.FULL ).Value;
                                        stats.CreatedLimitedCRMUsers += CRMController.GetCRMUsersCount(o.Id, string.Empty, string.Empty, CRMUserLycenseTypes.LIMITED).Value;
                                        stats.CreatedESSCRMUsers += CRMController.GetCRMUsersCount(o.Id, string.Empty, string.Empty, CRMUserLycenseTypes.ESS).Value;
                                        stats.UsedCRMDiskSpace += CRMController.GetDBSize(o.Id, o.PackageId);
                                        stats.AllocatedCRMDiskSpace += CRMController.GetMaxDBSize(o.Id, o.PackageId);
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                                    {
                                        stats.CreatedProfessionalCRMUsers += CRMController.GetCRMUsersCount(o.Id, string.Empty, string.Empty, CRMUserLycenseTypes.PROFESSIONAL).Value;
                                        stats.CreatedBasicCRMUsers += CRMController.GetCRMUsersCount(o.Id, string.Empty, string.Empty, CRMUserLycenseTypes.BASIC).Value;
                                        stats.CreatedEssentialCRMUsers += CRMController.GetCRMUsersCount(o.Id, string.Empty, string.Empty, CRMUserLycenseTypes.ESSENTIAL).Value;
                                        stats.UsedCRMDiskSpace += CRMController.GetDBSize(o.Id, o.PackageId);
                                        stats.AllocatedCRMDiskSpace += CRMController.GetMaxDBSize(o.Id, o.PackageId);
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.BlackBerry))
                                    {
                                        stats.CreatedBlackBerryUsers += BlackBerryController.GetBlackBerryUsersCount(o.Id, string.Empty, string.Empty).Value;
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.OCS))
                                    {
                                        stats.CreatedOCSUsers += OCSController.GetOCSUsersCount(o.Id, string.Empty, string.Empty).Value;
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.Lync))
                                    {
                                        stats.CreatedLyncUsers += LyncController.GetLyncUsersCount(o.Id).Value;
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.SfB))
                                    {
                                        stats.CreatedSfBUsers += SfBController.GetSfBUsersCount(o.Id).Value;
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.EnterpriseStorage))
                                    {
                                        SystemFile[] folders = EnterpriseStorageController.GetFolders(o.Id);

                                        stats.CreatedEnterpriseStorageFolders += folders.Count();

                                        stats.UsedEnterpriseStorageSpace += folders.Where(x => x.FRSMQuotaMB != -1).Sum(x => x.FRSMQuotaMB);
                                    }

                                    if (cntxTmp.Groups.ContainsKey(ResourceGroups.RDS))
                                    {
                                        stats.CreatedRdsUsers += RemoteDesktopServicesController.GetOrganizationRdsUsersCount(o.Id);
                                        stats.CreatedRdsCollections += RemoteDesktopServicesController.GetOrganizationRdsCollectionsCount(o.Id);
                                        stats.CreatedRdsServers += RemoteDesktopServicesController.GetOrganizationRdsServersCount(o.Id);
                                    }

                                    if (stats.ServiceLevels == null)
                                    {
                                        stats.ServiceLevels = GetServiceLevelQuotas(cntx, org.Id);
                                    }
                                    else
                                    {
                                        var orgServiceLevels = GetServiceLevelQuotas(cntx, org.Id);

                                        foreach (var totalQuota in stats.ServiceLevels)
                                        {
                                            var orgQuota = orgServiceLevels.FirstOrDefault(q => q.QuotaName == totalQuota.QuotaName);

                                            if (orgQuota != null)
                                            {
                                                totalQuota.QuotaAllocatedValue += orgQuota.QuotaAllocatedValue;
                                                totalQuota.QuotaUsedValue += orgQuota.QuotaUsedValue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // disk space               
                // allocated quotas                               

                stats.AllocatedUsers = cntx.Quotas[Quotas.ORGANIZATION_USERS].GetQuotaAllocatedValue(byOrganization);
                stats.AllocatedDeletedUsers = cntx.Quotas[Quotas.ORGANIZATION_DELETED_USERS].GetQuotaAllocatedValue(byOrganization);
                stats.AllocatedDomains = cntx.Quotas[Quotas.ORGANIZATION_DOMAINS].GetQuotaAllocatedValue(byOrganization);
                stats.AllocatedGroups = cntx.Quotas[Quotas.ORGANIZATION_SECURITYGROUPS].GetQuotaAllocatedValue(byOrganization);

                if (cntx.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
                {
                    stats.AllocatedSharePointSiteCollections = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_SITES].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
                {
                    stats.AllocatedSharePointEnterpriseSiteCollections = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_ENTERPRISE_SITES].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM))
                {
                    stats.AllocatedCRMUsers = cntx.Quotas[Quotas.CRM_USERS].GetQuotaAllocatedValue(byOrganization);
                    stats.AllocatedLimitedCRMUsers = cntx.Quotas[Quotas.CRM_LIMITEDUSERS].GetQuotaAllocatedValue(byOrganization);
                    stats.AllocatedESSCRMUsers = cntx.Quotas[Quotas.CRM_ESSUSERS].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                {
                    stats.AllocatedProfessionalCRMUsers = cntx.Quotas[Quotas.CRM2013_PROFESSIONALUSERS].GetQuotaAllocatedValue(byOrganization);
                    stats.AllocatedBasicCRMUsers = cntx.Quotas[Quotas.CRM2013_BASICUSERS].GetQuotaAllocatedValue(byOrganization);
                    stats.AllocatedEssentialCRMUsers = cntx.Quotas[Quotas.CRM2013_ESSENTIALUSERS].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.BlackBerry))
                {
                    stats.AllocatedBlackBerryUsers = cntx.Quotas[Quotas.BLACKBERRY_USERS].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.OCS))
                {
                    stats.AllocatedOCSUsers = cntx.Quotas[Quotas.OCS_USERS].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.Lync))
                {
                    stats.AllocatedLyncUsers = cntx.Quotas[Quotas.LYNC_USERS].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.SfB))
                {
                    stats.AllocatedSfBUsers = cntx.Quotas[Quotas.SFB_USERS].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.EnterpriseStorage))
                {
                    stats.AllocatedEnterpriseStorageFolders = cntx.Quotas[Quotas.ENTERPRISESTORAGE_FOLDERS].GetQuotaAllocatedValue(byOrganization);
                    stats.AllocatedEnterpriseStorageSpace = cntx.Quotas[Quotas.ENTERPRISESTORAGE_DISKSTORAGESPACE].GetQuotaAllocatedValue(byOrganization);
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.RDS))
                {
                    stats.AllocatedRdsServers = cntx.Quotas[Quotas.RDS_SERVERS].GetQuotaAllocatedValue(byOrganization);
                    stats.AllocatedRdsCollections = cntx.Quotas[Quotas.RDS_COLLECTIONS].GetQuotaAllocatedValue(byOrganization);
                    stats.AllocatedRdsUsers = cntx.Quotas[Quotas.RDS_USERS].GetQuotaAllocatedValue(byOrganization);
                }


                return stats;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static List<QuotaValueInfo> GetServiceLevelQuotas(PackageContext cntx, int orgItemId)
        {
            ServiceLevel[] serviceLevels = GetSupportServiceLevels();
            List<OrganizationUser> accounts = SearchAccounts(orgItemId, "", "", "", true);

            var quotas = Array.FindAll(cntx.QuotasArray, x => x.QuotaName.Contains(Quotas.SERVICE_LEVELS));
            foreach (var quota in quotas)
            {
               int levelId = serviceLevels.First(x => x.LevelName == quota.QuotaName.Replace(Quotas.SERVICE_LEVELS, "")).LevelId;
                int usedInOrgCount = accounts.Count(x => x.LevelId == levelId);

                quota.QuotaUsedValue = usedInOrgCount;
            }

            return quotas.ToList();
        }

        public static int ChangeOrganizationDomainType(int itemId, int domainId, ExchangeAcceptedDomainType newDomainType)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "CHANGE_DOMAIN_TYPE", domainId, itemId);

            try
            {
                // change accepted domain type on Exchange
                int checkResult = ExchangeServerController.ChangeAcceptedDomainType(itemId, domainId, newDomainType);


                // change accepted domain type in DB
                int domainTypeId = (int)newDomainType;
                DataProvider.ChangeExchangeAcceptedDomainType(itemId, domainId, domainTypeId);

                return checkResult;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int AddOrganizationDomain(int itemId, string domainName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check domains quota
            OrganizationStatistics orgStats = GetOrganizationStatisticsByOrganization(itemId);
            if (orgStats.AllocatedDomains > -1
                && orgStats.CreatedDomains >= orgStats.AllocatedDomains)
                return BusinessErrorCodes.ERROR_EXCHANGE_DOMAINS_QUOTA_LIMIT;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "ADD_DOMAIN", domainName, itemId);

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                DomainInfo domain = null;

                // check if the domain already exists
                int checkResult = ServerController.CheckDomain(domainName);
                if (checkResult == BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS)
                {
                    // domain exists
                    // check if it belongs to the same space
                    domain = ServerController.GetDomain(domainName);
                    if (domain == null)
                        return checkResult;

                    if (domain.PackageId != org.PackageId)
                        return checkResult;

                    if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
                        return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;
                }
                else if (checkResult == BusinessErrorCodes.ERROR_RESTRICTED_DOMAIN)
                {
                    return checkResult;
                }

                // create domain if required
                if (domain == null)
                {
                    domain = new DomainInfo();
                    domain.PackageId = org.PackageId;
                    domain.DomainName = domainName;
                    domain.IsInstantAlias = false;
                    domain.IsSubDomain = false;

                    int domainId = ServerController.AddDomain(domain);
                    if (domainId < 0)
                        return domainId;

                    // add domain
                    domain.DomainId = domainId;
                }
                
                // Log Extension
                LogExtension.WriteObject(domain);

                // register domain
                DataProvider.AddExchangeOrganizationDomain(itemId, domain.DomainId, false);

                // register service item
                OrganizationDomain exchDomain = new OrganizationDomain();
                exchDomain.Name = domainName;
                exchDomain.PackageId = org.PackageId;
                exchDomain.ServiceId = org.ServiceId;
                PackageController.AddPackageItem(exchDomain);


                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                orgProxy.CreateOrganizationDomain(org.DistinguishedName, domainName);
                if (!string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExchangeServerController.AddAuthoritativeDomain(itemId, domain.DomainId);
                }

                OrganizationStatistics orgStatsExchange = ExchangeServerController.GetOrganizationStatisticsByOrganization(itemId);

                if (orgStatsExchange.AllocatedMailboxes == 0)
                {
                    ExchangeAcceptedDomainType newDomainType = ExchangeAcceptedDomainType.InternalRelay;
                    ChangeOrganizationDomainType(org.ServiceId, domain.DomainId, newDomainType);
                }

                if (org.IsOCSOrganization)
                {
                    OCSController.AddDomain(domain.DomainName, itemId);
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int SetOrganizationDefaultDomain(int itemId, int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return -1;

            // update default domain
            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain == null)
                return 0;

            org.DefaultDomain = domain.DomainName;

            // save organization
            PackageController.UpdatePackageItem(org);

            return 0;
        }

        public static void SetDefaultOrganization(int newDefaultOrganizationId, int currentDefaultOrganizationId)
        {
            // place log record
            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            parameters.Add(new BackgroundTaskParameter("ItemID", newDefaultOrganizationId));

            TaskManager.StartTask("ORGANIZATION", "SET_DEFAULT_ORG", parameters);

            try
            {
                if (currentDefaultOrganizationId > 0)
                {
                    // load current default organization
                    Organization currentDefaultOrg = GetOrganization(currentDefaultOrganizationId);

                    currentDefaultOrg.IsDefault = false;

                    // save changes
                    PackageController.UpdatePackageItem(currentDefaultOrg);
                }

                // load organization
                Organization newDefaultOrg = GetOrganization(newDefaultOrganizationId);
                
                newDefaultOrg.IsDefault = true;
                // save changes
                PackageController.UpdatePackageItem(newDefaultOrg);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        #region Users

        public static List<OrganizationDeletedUser> GetOrganizationDeletedUsers(int itemId)
        {
            var result = new List<OrganizationDeletedUser>();

            var orgDeletedUsers = ObjectUtils.CreateListFromDataReader<OrganizationUser>(
                DataProvider.GetExchangeAccounts(itemId, (int)ExchangeAccountType.DeletedUser));

            foreach (var orgDeletedUser in orgDeletedUsers)
            {
                OrganizationDeletedUser deletedUser = GetDeletedUser(orgDeletedUser.AccountId);

                if (deletedUser == null)
                    continue;

                deletedUser.User = orgDeletedUser;

                result.Add(deletedUser);
            }

            return result;
        }

        public static OrganizationDeletedUsersPaged GetOrganizationDeletedUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {
            DataSet ds =
                DataProvider.GetExchangeAccountsPaged(SecurityContext.User.UserId, itemId, ((int)ExchangeAccountType.DeletedUser).ToString(),
                filterColumn, filterValue, sortColumn, startRow, maximumRows, false);

            OrganizationDeletedUsersPaged result = new OrganizationDeletedUsersPaged();
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<OrganizationUser> Tmpaccounts = new List<OrganizationUser>();
            ObjectUtils.FillCollectionFromDataView(Tmpaccounts, ds.Tables[1].DefaultView);

            List<OrganizationDeletedUser> deletedUsers = new List<OrganizationDeletedUser>();

            foreach (OrganizationUser user in Tmpaccounts.ToArray())
            {
                OrganizationDeletedUser deletedUser = GetDeletedUser(user.AccountId);

                if (deletedUser == null)
                    continue;

                OrganizationUser tmpUser = GetUserGeneralSettings(itemId, user.AccountId);

                if (tmpUser != null)
                {
                    deletedUser.User = tmpUser;

                    deletedUsers.Add(deletedUser);
                }
            }

            result.PageDeletedUsers = deletedUsers.ToArray();

            return result;
        }

        public static OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {

            #region Demo Mode
            if (IsDemoMode)
            {
                OrganizationUsersPaged res = new OrganizationUsersPaged();
                List<OrganizationUser> demoAccounts = SearchAccounts(1, "", "", "", true);

                OrganizationUser r1 = new OrganizationUser();
                r1.AccountId = 20;
                r1.AccountName = "room1_fabrikam";
                r1.AccountType = ExchangeAccountType.Room;
                r1.DisplayName = "Meeting Room 1";
                r1.PrimaryEmailAddress = "room1@fabrikam.net";
                demoAccounts.Add(r1);

                OrganizationUser e1 = new OrganizationUser();
                e1.AccountId = 21;
                e1.AccountName = "projector_fabrikam";
                e1.AccountType = ExchangeAccountType.Equipment;
                e1.DisplayName = "Projector 1";
                e1.PrimaryEmailAddress = "projector@fabrikam.net";
                demoAccounts.Add(e1);
                res.PageUsers = demoAccounts.ToArray();
                res.RecordsCount = res.PageUsers.Length;
                return res;
            }
            #endregion

            string accountTypes = string.Format("{0}, {1}, {2}, {3}", ((int)ExchangeAccountType.User), ((int)ExchangeAccountType.Mailbox), ((int)ExchangeAccountType.Room), ((int)ExchangeAccountType.Equipment));


            DataSet ds =
                DataProvider.GetExchangeAccountsPaged(SecurityContext.User.UserId, itemId, accountTypes, filterColumn,
                                                      filterValue, sortColumn, startRow, maximumRows, false);

            OrganizationUsersPaged result = new OrganizationUsersPaged();
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<OrganizationUser> Tmpaccounts = new List<OrganizationUser>();
            ObjectUtils.FillCollectionFromDataView(Tmpaccounts, ds.Tables[1].DefaultView);
            result.PageUsers = Tmpaccounts.ToArray();

            List<OrganizationUser> accounts = new List<OrganizationUser>();

            foreach (OrganizationUser user in Tmpaccounts.ToArray())
            {
                OrganizationUser tmpUser = GetUserGeneralSettings(itemId, user.AccountId);

                if (tmpUser != null)
                    accounts.Add(tmpUser);
            }

            result.PageUsers = accounts.ToArray();
            return result;
        }

        public static List<OrganizationUser> GetOrganizationUsersWithExpiredPassword(int itemId, int daysBeforeExpiration)
        {
            // load organization
            Organization org = GetOrganization(itemId);

            if (org == null)
            {
                return null;
            }

            Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

            var expiredUsersAd = orgProxy.GetOrganizationUsersWithExpiredPassword(org.OrganizationId, daysBeforeExpiration);

            var expiredUsersDb = GetOrganizationUsersPaged(itemId, null, null, null, 0, int.MaxValue).PageUsers.Where(x => expiredUsersAd.Any(z => z.SamAccountName == x.SamAccountName)).ToList();

            foreach (var user in expiredUsersDb)
            {
                var adUser = expiredUsersAd.First(x => x.SamAccountName == user.SamAccountName);

                user.PasswordExpirationDateTime = adUser.PasswordExpirationDateTime;
            }

            return expiredUsersDb;
        }

        public static ResultObject SendResetUserPasswordLinkSms(int itemId, int accountId, string reason,
            string phoneTo = null)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "SEND_USER_PASSWORD_RESET_SMS",
                itemId);

            try
            {

                // load organization
                Organization org = GetOrganization(itemId);

                if (org == null)
                {
                    throw new Exception(string.Format("Organization not found (ItemId = {0})", itemId));
                }

                UserInfo owner = PackageController.GetPackageOwner(org.PackageId);
                OrganizationUser user = OrganizationController.GetUserGeneralSettingsWithExtraData(itemId, accountId);

                user.ItemId = itemId;

                if (string.IsNullOrEmpty(phoneTo))
                {
                    phoneTo = user.MobilePhone;
                }

                UserSettings settings = UserController.GetUserSettings(owner.UserId,
                    UserSettings.USER_PASSWORD_RESET_LETTER);


                string body = settings["PasswordResetLinkSmsBody"];

                var pincode = GeneratePincode();
                Guid token;

                var items = new Hashtable();

                items["passwordResetLink"] = GenerateUserPasswordResetLink(user.ItemId, user.AccountId, out token, pincode);

                body = PackageController.EvaluateTemplate(body, items);

                TaskManager.Write("Organization ID : " + user.ItemId);
                TaskManager.Write("Account : " + user.DisplayName);
                TaskManager.Write("Reason : " + reason);
                TaskManager.Write("SmsTo : " + phoneTo);

                // send Sms message
                var response = SendSms(phoneTo, body);

                if (response.RestException != null)
                {
                    throw new Exception(response.RestException.Message);
                }

                SetAccessTokenResponse(token, pincode);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                TaskManager.CompleteResultTask(result);
                result.AddError("", ex);
                return result;
            }

            TaskManager.CompleteResultTask();
            return result;
        }

        public static ResultObject SendUserPasswordRequestSms(int itemId, int accountId, string reason, string phoneTo = null)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "SEND_USER_PASSWORD_REQUEST_SMS",
                itemId);

            try
            {

                // load organization
                Organization org = GetOrganization(itemId);

                if (org == null)
                {
                    throw new Exception(string.Format("Organization not found (ItemId = {0})", itemId));
                }

                UserInfo owner = PackageController.GetPackageOwner(org.PackageId);
                OrganizationUser user = OrganizationController.GetUserGeneralSettingsWithExtraData(itemId, accountId);

                user.ItemId = itemId;

                if (string.IsNullOrEmpty(phoneTo))
                {
                    phoneTo = user.MobilePhone;
                }

                UserSettings settings = UserController.GetUserSettings(owner.UserId, UserSettings.USER_PASSWORD_REQUEST_LETTER);


                string body = settings["SMSBody"];

                var pincode = GeneratePincode();
                Guid token;

                var items = new Hashtable();

                items["passwordResetLink"] = GenerateUserPasswordResetLink(user.ItemId, user.AccountId, out token, pincode);

                body = PackageController.EvaluateTemplate(body, items);

                TaskManager.Write("Organization ID : " + user.ItemId);
                TaskManager.Write("Account : " + user.DisplayName);
                TaskManager.Write("Reason : " + reason);
                TaskManager.Write("SmsTo : " + phoneTo);

                // send Sms message
                var response = SendSms(phoneTo, body);

                if (response.RestException != null)
                {
                    throw new Exception(response.RestException.Message);
                }

                SetAccessTokenResponse(token, pincode);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                TaskManager.CompleteResultTask(result);
                result.AddError("", ex);
                return result;
            }

            TaskManager.CompleteResultTask();
            return result;
        }

        public static ResultObject SendResetUserPasswordPincodeSms(Guid token, string phoneTo = null)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "SEND_USER_PASSWORD_RESET_SMS_PINCODE");

            try
            {
                var accessToken = OrganizationController.GetAccessToken(token, AccessTokenTypes.PasswrodReset);

                if (accessToken == null)
                {
                    throw new Exception(string.Format("Access token not found"));
                }

                // load organization
                Organization org = GetOrganization(accessToken.ItemId);

                if (org == null)
                {
                    throw new Exception(string.Format("Organization not found"));
                }

                UserInfo owner = PackageController.GetPackageOwner(org.PackageId);
                OrganizationUser user = OrganizationController.GetUserGeneralSettingsWithExtraData(accessToken.ItemId, accessToken.AccountId);

                if (string.IsNullOrEmpty(phoneTo))
                {
                    phoneTo = user.MobilePhone;
                }

                UserSettings settings = UserController.GetUserSettings(owner.UserId, UserSettings.USER_PASSWORD_RESET_PINCODE_LETTER);

                string body = settings["PasswordResetPincodeSmsBody"];

                var items = new Hashtable();

                var pincode = GeneratePincode();

                items["passwordResetPincode"] = pincode;

                body = PackageController.EvaluateTemplate(body, items);

                TaskManager.Write("Organization ID : " + user.ItemId);
                TaskManager.Write("Account : " + user.DisplayName);
                TaskManager.Write("SmsTo : " + phoneTo);

                // send Sms message
                var response = SendSms(phoneTo, body);

                if (response.RestException != null)
                {
                    throw new Exception(response.RestException.Message);
                }

                SetAccessTokenResponse(token, pincode);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                TaskManager.CompleteResultTask(result);
                result.AddError("", ex);
                return result;
            }

            TaskManager.CompleteResultTask();
            return result;
        }

        public static ResultObject SendResetUserPasswordPincodeEmail(Guid token, string mailTo = null)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "SEND_USER_PASSWORD_RESET_EMAIL_PINCODE");

            try
            {
                var accessToken = OrganizationController.GetAccessToken(token, AccessTokenTypes.PasswrodReset);

                if (accessToken == null)
                {
                    throw new Exception(string.Format("Access token not found"));
                }

                // load organization
                Organization org = GetOrganization(accessToken.ItemId);

                if (org == null)
                {
                    throw new Exception(string.Format("Organization not found"));
                }

                UserInfo owner = PackageController.GetPackageOwner(org.PackageId);
                OrganizationUser user = OrganizationController.GetUserGeneralSettingsWithExtraData(accessToken.ItemId, accessToken.AccountId);

                if (string.IsNullOrEmpty(mailTo))
                {
                    mailTo = user.PrimaryEmailAddress;
                }

                UserSettings settings = UserController.GetUserSettings(owner.UserId, UserSettings.USER_PASSWORD_RESET_PINCODE_LETTER);

                var generalSettings = OrganizationController.GetOrganizationGeneralSettings(accessToken.ItemId);

                var logoUrl = generalSettings != null ? generalSettings.OrganizationLogoUrl : string.Empty;

                if (string.IsNullOrEmpty(logoUrl))
                {
                    logoUrl = settings["LogoUrl"];
                }

                string from = settings["From"];

                string subject = settings["Subject"];
                string body = owner.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
                bool isHtml = owner.HtmlMail;

                MailPriority priority = MailPriority.Normal;

                if (!String.IsNullOrEmpty(settings["Priority"]))
                {
                    priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);
                }

                string pincode = GeneratePincode() ;

                Hashtable items = new Hashtable();

                items["user"] = user;
                items["logoUrl"] = logoUrl;
                items["passwordResetPincode"] = pincode;

                body = PackageController.EvaluateTemplate(body, items);

                    SetAccessTokenResponse(token, pincode);

                TaskManager.Write("Organization ID : " + user.ItemId);
                TaskManager.Write("Account : " + user.DisplayName);
                TaskManager.Write("MailTo : " + mailTo);

                // send mail message
                MailHelper.SendMessage(from, mailTo, null, subject, body, priority, isHtml);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                TaskManager.CompleteResultTask(result);
                result.AddError("", ex);
                return result;
            }

            TaskManager.CompleteResultTask();
            return result;
        }

        private static string GeneratePincode()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(10000, 99999).ToString(CultureInfo.InvariantCulture);
        }

        private static SMSMessage SendSms(string to, string body)
        {
            SystemSettings settings = SystemController.GetSystemSettingsInternal(SystemSettings.TWILIO_SETTINGS, false);

            if (settings == null)
            {
                throw new Exception("Twilio settings are not set");
            }

            string accountSid = settings.GetValueOrDefault(SystemSettings.TWILIO_ACCOUNTSID_KEY, string.Empty);
            string authToken = settings.GetValueOrDefault(SystemSettings.TWILIO_AUTHTOKEN_KEY, string.Empty);
            string phoneFrom = settings.GetValueOrDefault(SystemSettings.TWILIO_PHONEFROM_KEY, string.Empty);

            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(accountSid))
            {
                throw new Exception("Twilio settings are not set (System settings)");
            }

            var client = new TwilioRestClient(accountSid, authToken);

            return client.SendSmsMessage(phoneFrom, to, body);
        }

        /// <summary>
        /// Send reset user password email
        /// </summary>
        /// <param name="itemId">Organization Id</param>
        /// <param name="accountId">User Id</param>
        /// <param name="reason">Reason why reset email is sent.</param>
        /// <param name="mailTo">Optional, if null accountID user PrimaryEmailAddress will be used</param>
        /// <param name="finalStep">Url direction</param>
        public static void SendResetUserPasswordEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            // load organization
            Organization org = GetOrganization(itemId);

            if (org == null)
            {
                throw new Exception(string.Format("Organization not found (ItemId = {0})", itemId));
            }

            Organizations orgProxy = GetOrganizationProxy(org.ServiceId);


            UserInfo owner = PackageController.GetPackageOwner(org.PackageId);
            OrganizationUser user = OrganizationController.GetUserGeneralSettingsWithExtraData(itemId, accountId);

            user.ItemId = itemId;

            if (string.IsNullOrEmpty(mailTo))
            {
                mailTo = user.PrimaryEmailAddress;
            }

            var generalSettings = OrganizationController.GetOrganizationGeneralSettings(itemId);

            var logoUrl = generalSettings != null ? generalSettings.OrganizationLogoUrl : string.Empty;

            SendUserPasswordEmail(owner, user, reason, mailTo, logoUrl, UserSettings.USER_PASSWORD_RESET_LETTER, "USER_PASSWORD_RESET_LETTER", finalStep);
        }

        public static void SendUserPasswordRequestEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            // load organization
            Organization org = GetOrganization(itemId);

            if (org == null)
            {
                throw new Exception(string.Format("Organization not found (ItemId = {0})", itemId));
            }

            UserInfo owner = PackageController.GetPackageOwner(org.PackageId);
            OrganizationUser user = OrganizationController.GetUserGeneralSettingsWithExtraData(itemId, accountId);

            user.ItemId = itemId;

            if (string.IsNullOrEmpty(mailTo))
            {
                mailTo = user.PrimaryEmailAddress;
            }

            var generalSettings = OrganizationController.GetOrganizationGeneralSettings(itemId);

            var logoUrl = generalSettings != null ? generalSettings.OrganizationLogoUrl : string.Empty;

            SendUserPasswordEmail(owner, user, reason, mailTo, logoUrl, UserSettings.USER_PASSWORD_REQUEST_LETTER, "USER_PASSWORD_REQUEST_LETTER", finalStep);
        }

        public static void SendUserExpirationPasswordEmail(UserInfo owner, OrganizationUser user, string reason, string mailTo, string logoUrl)
        {
            SendUserPasswordEmail(owner, user, reason, user.PrimaryEmailAddress, logoUrl, UserSettings.USER_PASSWORD_EXPIRATION_LETTER, "USER_PASSWORD_EXPIRATION_LETTER", true);
        }

        public static void SendUserPasswordEmail(UserInfo owner, OrganizationUser user, string reason, string mailTo, string logoUrl, string settingsName, string taskName, bool finalStep)
        {
            UserSettings settings = UserController.GetUserSettings(owner.UserId,
                settingsName);

            TaskManager.StartTask("ORGANIZATION", "SEND_" + taskName, user.ItemId);

            try
            {
                if (string.IsNullOrEmpty(logoUrl))
                {
                    logoUrl = settings["LogoUrl"];
                }

                string from = settings["From"];

                string subject = settings["Subject"];
                string body = owner.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
                bool isHtml = owner.HtmlMail;

                MailPriority priority = MailPriority.Normal;

                if (!String.IsNullOrEmpty(settings["Priority"]))
                {
                    priority = (MailPriority) Enum.Parse(typeof (MailPriority), settings["Priority"], true);
                }

                Guid token;

                string pincode = finalStep ? GeneratePincode() :  null;

                Hashtable items = new Hashtable();

                items["user"] = user;
                items["logoUrl"] = logoUrl;
                items["passwordResetLink"] = GenerateUserPasswordResetLink(user.ItemId, user.AccountId, out token, pincode);

                body = PackageController.EvaluateTemplate(body, items);

                if (finalStep)
                {
                    SetAccessTokenResponse(token, pincode);
                }

                TaskManager.Write("Organization ID : " + user.ItemId);
                TaskManager.Write("Account : " + user.DisplayName);
                TaskManager.Write("Reason : " + reason);
                TaskManager.Write("MailTo : " + mailTo);

                // send mail message
                MailHelper.SendMessage(from, mailTo, null, subject, body, priority, isHtml);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static AccessToken GetAccessToken(Guid accessToken, AccessTokenTypes type)
        {
            return ObjectUtils.FillObjectFromDataReader<AccessToken>(DataProvider.GetAccessTokenByAccessToken(accessToken, type));
        }

        public static void DeleteAccessToken(Guid accessToken, AccessTokenTypes type)
        {
            DataProvider.DeleteAccessToken(accessToken, type);
        }

        public static void DeleteAllExpiredTokens()
        {
            DataProvider.DeleteExpiredAccessTokens();
        }

        public static SystemSettings GetWebDavSystemSettings()
        {
            return SystemController.GetSystemSettingsInternal(SystemSettings.WEBDAV_PORTAL_SETTINGS, false);
        }

        public static string GenerateUserPasswordResetLink(int itemId, int accountId, out Guid tokenGuid, string pincode = null, string resetUrl = null)
        {
            var settings = GetWebDavSystemSettings();
            tokenGuid = new Guid();

            if (settings == null || !settings.GetValueOrDefault(SystemSettings.WEBDAV_PASSWORD_RESET_ENABLED_KEY, false) || !settings.Contains("WebdavPortalUrl"))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(resetUrl) == false)
            {
                return resetUrl;
            }

            string passwordResetUrlFormat = string.IsNullOrEmpty(pincode) ? "account/password-reset/step-2" : "account/password-reset/step-final";

            var webdavPortalUrl = new Uri(settings["WebdavPortalUrl"]);

            var hours = settings.GetValueOrDefault(SystemSettings.WEBDAV_PASSWORD_RESET_LINK_LIFE_SPAN, 1);

            var token = CreateAccessToken(itemId, accountId, AccessTokenTypes.PasswrodReset, hours);

            tokenGuid = token.AccessTokenGuid;

            var resultUrl = webdavPortalUrl.Append(passwordResetUrlFormat)
                .Append(token.AccessTokenGuid.ToString("n"));

            if (string.IsNullOrEmpty(pincode) == false)
            {
                resultUrl = resultUrl.Append(pincode);
            }

            return resultUrl.ToString();
        }

        public static AccessToken CreatePasswordResetAccessToken(int itemId, int accountId)
        {
            var settings = GetWebDavSystemSettings();

            if (settings == null || !settings.GetValueOrDefault(SystemSettings.WEBDAV_PASSWORD_RESET_ENABLED_KEY, false))
            {
                return null;
            }

            var hours = settings.GetValueOrDefault(SystemSettings.WEBDAV_PASSWORD_RESET_LINK_LIFE_SPAN, 1);

            return CreateAccessToken(itemId, accountId, AccessTokenTypes.PasswrodReset, hours);
        }

        private static AccessToken CreateAccessToken(int itemId, int accountId, AccessTokenTypes type, int hours)
        {
            var token = new AccessToken
            {
                AccessTokenGuid = Guid.NewGuid(),
                ItemId = itemId,
                AccountId = accountId,
                TokenType = type,
                ExpirationDate = DateTime.Now.AddHours(hours)
            };

            token.Id = DataProvider.AddAccessToken(token);

            return token;
        }

        public static void SetAccessTokenResponse(Guid accessToken, string response)
        {
            DataProvider.SetAccessTokenResponseMessage(accessToken, response);
        }

        public static bool CheckPhoneNumberIsInUse(int itemId, string phoneNumber, string userSamAccountName = null)
        {
            // load organization
            Organization org = GetOrganization(itemId);

            if (org == null)
            {
                throw new Exception(string.Format("Organization with id '{0}' not found", itemId));
            }

            Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

            return orgProxy.CheckPhoneNumberIsInUse(phoneNumber, userSamAccountName);
        }

        public static void UpdateOrganizationPasswordSettings(int itemId, OrganizationPasswordSettings settings)
        {
            TaskManager.StartTask("ORGANIZATION", "UPDATE_PASSWORD_SETTINGS");

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);

                if (org == null)
                {
                    TaskManager.WriteWarning("Organization with itemId '{0}' not found", itemId.ToString());
                    return;
                }

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                orgProxy.ApplyPasswordSettings(org.OrganizationId, settings);

                var xml = ObjectUtils.Serialize(settings);

                DataProvider.UpdateOrganizationSettings(itemId, OrganizationSettings.PasswordSettings, xml);

                // Log Extension
                LogExtension.WriteVariable("Password Settings", xml);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static OrganizationPasswordSettings GetOrganizationPasswordSettings(int itemId)
        {
            var passwordSettings = GetOrganizationSettings<OrganizationPasswordSettings>(itemId, OrganizationSettings.PasswordSettings);

            if (passwordSettings == null)
            {
                Organization org = GetOrganization(itemId);

                if (org == null)
                {
                    throw new Exception(string.Format("Organization not found (ItemId = {0})", itemId));
                }

                var package = PackageController.GetPackage(org.PackageId);

                UserSettings userSettings = UserController.GetUserSettings(package.UserId, UserSettings.EXCHANGE_POLICY);

                if (userSettings != null)
                {
                    string policyValue = userSettings["MailboxPasswordPolicy"];

                    if (policyValue != null)
                    {
                        string[] parts = policyValue.Split(';');

                        passwordSettings = new OrganizationPasswordSettings
                        {
                            MinimumLength = GetValueSafe(parts, 1, 0),
                            MaximumLength = GetValueSafe(parts, 2, 0),
                            UppercaseLettersCount = GetValueSafe(parts, 3, 0),
                            NumbersCount = GetValueSafe(parts, 4, 0),
                            SymbolsCount = GetValueSafe(parts, 5, 0),
                            AccountLockoutThreshold = GetValueSafe(parts, 7, 0),

                            EnforcePasswordHistory = GetValueSafe(parts, 8, 0),
                            AccountLockoutDuration = GetValueSafe(parts, 9, 0),
                            ResetAccountLockoutCounterAfter = GetValueSafe(parts, 10, 0),
                            LockoutSettingsEnabled = GetValueSafe(parts, 11, false),
                            PasswordComplexityEnabled = GetValueSafe(parts, 12, true),
                            MaxPasswordAge = GetValueSafe(parts, 13, 42),
                        };


                        PasswordPolicyResult passwordPolicy = GetPasswordPolicy(itemId);

                        if (passwordPolicy.IsSuccess)
                        {
                            passwordSettings.MinimumLength = passwordPolicy.Value.MinLength;
                            if (passwordPolicy.Value.IsComplexityEnable)
                            {
                                passwordSettings.NumbersCount = 1;
                                passwordSettings.SymbolsCount = 1;
                                passwordSettings.UppercaseLettersCount = 1;
                            }
                        }
                    }
                }
            }

            return passwordSettings;
        }

        public static T GetValueSafe<T>(string[] array, int index, T defaultValue)
        {
            if (array.Length > index)
            {
                if (string.IsNullOrEmpty(array[index]))
                {
                    return defaultValue;
                }

                return (T)Convert.ChangeType(array[index], typeof(T));
            }

            return defaultValue;
        }

        public static void UpdateOrganizationGeneralSettings(int itemId, OrganizationGeneralSettings settings)
        {
            TaskManager.StartTask("ORGANIZATION", "UPDATE_GENERAL_SETTINGS");

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);

                if (org == null)
                {
                    TaskManager.WriteWarning("Organization with itemId '{0}' not found", itemId.ToString());
                    return;
                }

                var xml = ObjectUtils.Serialize(settings);

                DataProvider.UpdateOrganizationSettings(itemId, OrganizationSettings.GeneralSettings, xml);

                // Log Extension
                LogExtension.WriteVariable("General Settings", xml);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static OrganizationGeneralSettings GetOrganizationGeneralSettings(int itemId)
        {
            return GetOrganizationSettings<OrganizationGeneralSettings>(itemId, OrganizationSettings.GeneralSettings);
        }

        private static T GetOrganizationSettings<T>(int itemId, string settingsName)
        {
            var entity = ObjectUtils.FillObjectFromDataReader<OrganizationSettingsEntity>(DataProvider.GetOrganizationSettings(itemId, settingsName));

            if (entity == null)
            {
                return default(T);
            }

            return ObjectUtils.Deserialize<T>(entity.Xml);
        }

        private static bool EmailAddressExists(string emailAddress)
        {
            return DataProvider.ExchangeAccountEmailAddressExists(emailAddress);
        }


        private static int AddOrganizationUser(int itemId, string accountName, string displayName, string email, string sAMAccountName, string accountPassword, string subscriberNumber)
        {
            return DataProvider.AddExchangeAccount(itemId, (int)ExchangeAccountType.User, accountName, displayName, email, false, string.Empty,
                                            sAMAccountName, 0, subscriberNumber.Trim());

        }

        public static string GetAccountName(string loginName)
        {
            //string []parts = loginName.Split('@');
            //return parts != null && parts.Length > 1 ? parts[0] : loginName;
            return loginName;

        }

        public static int CreateUser(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool enabled, bool sendNotification, string to, out string accountName)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException("displayName");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(domain))
                throw new ArgumentNullException("domain");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            accountName = string.Empty;

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;


            // place log record
            TaskManager.StartTask("ORGANIZATION", "CREATE_USER", displayName, itemId);
            
            // Log Extension
            LogExtension.WriteVariables(new {name, domain, subscriberNumber});
            
            int userId = -1;

            try
            {
                displayName = displayName.Trim();
                name = name.Trim();
                domain = domain.Trim();

                // e-mail
                string email = name + "@" + domain;

                if (EmailAddressExists(email))
                    return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                StringDictionary serviceSettings = ServerController.GetServiceSettings(org.ServiceId);

                if (serviceSettings == null)
                {
                    return -1;
                }

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                int errorCode;
                if (!CheckUserQuota(org.Id, out errorCode))
                    return errorCode;

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                string upn = string.Format("{0}@{1}", name, domain);
                string SamAccountName = BuildAccountNameEx(org, name);

                // Log Extension
                LogExtension.WriteVariable("Account Name", SamAccountName);
                LogExtension.WriteVariables(new {upn});

                if (orgProxy.CreateUser(org.OrganizationId, SamAccountName, displayName, upn, password, enabled) == 0)
                {
                    accountName = SamAccountName;
                    OrganizationUser retUser = orgProxy.GetUserGeneralSettings(SamAccountName, org.OrganizationId);
                    
                    // Log Extension
                    LogExtension.WriteVariable("sAMAccountName", retUser.DomainUserName);

                    userId = AddOrganizationUser(itemId, SamAccountName, displayName, email, retUser.DomainUserName, password, subscriberNumber);

                    // register email address
                    AddAccountEmailAddress(userId, email);

                    if (sendNotification)
                    {
                        SendSummaryLetter(org.Id, userId, true, to, "");
                    }
                }
                else
                {
                    TaskManager.WriteError("Failed to create user");
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return userId;
        }


        public static string BuildAccountNameEx(Organization org, string name)
        {
            StringDictionary serviceSettings = ServerController.GetServiceSettings(org.ServiceId);
            
            return AppendOrgId(serviceSettings) ? BuildAccountNameWithOrgId(org.OrganizationId, name, org.ServiceId) : BuildAccountName(org.OrganizationId, name, org.ServiceId);
        }


        /// <summary> Checks should or not user name include organization id. </summary>
        /// <param name="serviceSettings"> The service settings. </param>
        /// <returns> True - if organization id should be appended. </returns>
        private static bool AppendOrgId(StringDictionary serviceSettings)
        {
            if (!serviceSettings.ContainsKey("usernameformat"))
            {
                return false;
            }

            if (!serviceSettings["usernameformat"].Equals("Append OrgId", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public static int ImportUser(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber)
        {
            if (string.IsNullOrEmpty(accountName))
                throw new ArgumentNullException("accountName");

            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException("displayName");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(domain))
                throw new ArgumentNullException("domain");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");


            // place log record
            TaskManager.StartTask("ORGANIZATION", "IMPORT_USER", itemId);

            TaskManager.Write("Organization ID :" + itemId);
            TaskManager.Write("account :" + accountName);
            TaskManager.Write("name :" + name);
            TaskManager.Write("domain :" + domain);

            int userId = -1;

            try
            {
                accountName = accountName.Trim();
                displayName = displayName.Trim();
                name = name.Trim();
                domain = domain.Trim();

                // e-mail
                string email = name + "@" + domain;

                if (EmailAddressExists(email))
                    return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                int errorCode;
                if (!CheckUserQuota(org.Id, out errorCode))
                    return errorCode;

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                string upn = string.Format("{0}@{1}", name, domain);
                TaskManager.Write("upn :" + upn);

                OrganizationUser retUser = orgProxy.GetUserGeneralSettings(accountName, org.OrganizationId);

                TaskManager.Write("sAMAccountName :" + retUser.DomainUserName);

                userId = AddOrganizationUser(itemId, retUser.SamAccountName, displayName, email, retUser.DomainUserName, password, subscriberNumber);

                AddAccountEmailAddress(userId, email);

            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return userId;
        }


        private static void AddAccountEmailAddress(int accountId, string emailAddress)
        {
            DataProvider.AddExchangeAccountEmailAddress(accountId, emailAddress);
        }

        private static string BuildAccountName(string orgId, string name, int ServiceId)
        {
            string accountName = name = name.Replace(" ", "");
            int counter = 0;
            bool bFound = false;

            do
            {
                accountName = genSamLogin(name, counter.ToString("d5"));

                if (!AccountExists(accountName, ServiceId)) bFound = true;

                counter++;
            }
            while (!bFound);

            return accountName;
        }

        /// <summary> Building account name with organization Id. </summary>
        /// <param name="orgId"> The organization identifier. </param>
        /// <param name="name"> The name. </param>
        /// <param name="serviceId"> The service identifier. </param>
        /// <returns> The account name with organization Id. </returns>
        public static string BuildAccountNameWithOrgId(string orgId, string name, int serviceId)
        {
            name = ((orgId.Length + name.Length) > 19 && name.Length > 9) ? name.Substring(0, (19 - orgId.Length) < 10 ? 10 : 19 - orgId.Length) : name;

            orgId = (orgId.Length + name.Length) > 19 ? orgId.Substring(0, 19 - name.Length) : orgId;

            int maxLen = 19 - orgId.Length;

            if (!string.IsNullOrEmpty(orgId))
            {
                orgId = orgId.TrimEnd(' ', '.');
            }

            // try to choose name
            int i = 0;

            while (true)
            {
                string num = i > 0 ? i.ToString() : "";
                int len = maxLen - num.Length;

                if (name.Length > len)
                {
                    name = name.Substring(0, len);
                }

                string accountName = name + num + "_" + orgId;

                // check if already exists
                if (!AccountExists(accountName, serviceId))
                {
                    return accountName;
                }

                i++;
            }
        }


        private static string genSamLogin(string login, string strCounter)
        {
            int maxLogin = 20;
            int fullLen = login.Length + strCounter.Length;
            if (fullLen <= maxLogin)
                return login + strCounter;
            else
            {
                if (login.Length - (fullLen - maxLogin) > 0)
                    return login.Substring(0, login.Length - (fullLen - maxLogin)) + strCounter;
                else return strCounter; // ????
            }

        }


        private static bool AccountExists(string accountName, int ServiceId)
        {
            if (!DataProvider.ExchangeAccountExists(accountName))
            {
                Organizations orgProxy = GetOrganizationProxy(ServiceId);


                return orgProxy.DoesSamAccountNameExist(accountName);
            }
            else
                return true;
        }

        #region Deleted Users

        public static int SetDeletedUser(int itemId, int accountId, bool enableForceArchive)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "SET_DELETED_USER", itemId);
            
            // Log Extension
            LogExtension.WriteVariables(new {enableForceArchive});

            try
            {
                Guid crmUserId = CRMController.GetCrmUserId(accountId);
                if (crmUserId != Guid.Empty)
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_CRM_USER;
                }

                if (DataProvider.CheckOCSUserExists(accountId))
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_OCS_USER;
                }

                if (DataProvider.CheckLyncUserExists(accountId))
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_LYNC_USER;
                }

                if (DataProvider.CheckSfBUserExists(accountId))
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_SFB_USER;
                }


                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                int errorCode;
                if (!CheckDeletedUserQuota(org.Id, out errorCode))
                    return errorCode;

                // load account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);
                
                // Log Extension
                LogExtension.SetItemName(account.DisplayName);

                string accountName = GetAccountName(account.AccountName);

                var deletedUser = new OrganizationDeletedUser
                {
                    AccountId = account.AccountId,
                    OriginAT = account.AccountType,
                    ExpirationDate = DateTime.UtcNow.AddHours(1)
                };

                if (account.AccountType == ExchangeAccountType.User)
                {
                    Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                    //Disable user in AD
                    orgProxy.DisableUser(accountName, org.OrganizationId);
                }
                else
                {
                    if (enableForceArchive)
                    {
                        var serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.HostedOrganizations);

                        if (serviceId != 0)
                        {
                            string path = string.Empty;
                            QuotaValueInfo diskSpaceQuota = PackageController.GetPackageQuota(org.PackageId, Quotas.ORGANIZATION_DELETED_USERS_BACKUP_STORAGE_SPACE);

                            deletedUser.FileName = string.Format("{0}.pst", account.UserPrincipalName);

                            if (UsingStorageSpaces(serviceId))
                            {
                                var folder = _foldersManager.GetFolder(itemId, StorageSpaceFolderTypes.DeletedUsersData.ToString())
                                    ?? _foldersManager.CreateFolder(org.OrganizationId, itemId, StorageSpaceFolderTypes.DeletedUsersData.ToString(),
                                        StorageSpacesController.GetFsrmQuotaInBytes(diskSpaceQuota), QuotaType.Hard);

                                deletedUser.StoragePath = StorageSpacesController.GetParentUnc(folder.UncPath);

                                deletedUser.FolderName = Path.GetFileName(folder.UncPath);

                                path = Path.Combine(folder.UncPath, deletedUser.FileName);
                            }
                            else
                            {
                                var settings = ServerController.GetServiceSettings(serviceId);

                                deletedUser.StoragePath = settings["ArchiveStoragePath"];
                                deletedUser.FolderName = org.OrganizationId;

                                if (!CheckFolderExists(org.PackageId, deletedUser.StoragePath, deletedUser.FolderName))
                                {
                                    CreateFolder(org.PackageId, deletedUser.StoragePath, deletedUser.FolderName);
                                }

                                if (diskSpaceQuota.QuotaAllocatedValuePerOrganization != -1)
                                {
                                    SetFRSMQuotaOnFolder(org.PackageId, deletedUser.StoragePath, org.OrganizationId, diskSpaceQuota, QuotaType.Hard);
                                }

                                path = FilesController.ConvertToUncPath(serviceId, Path.Combine(GetDirectory(deletedUser.StoragePath), deletedUser.FolderName, deletedUser.FileName));
                            }

                            ExchangeServerController.ExportMailBox(itemId, accountId, path);
                        }
                    }

                    //Set Deleted Mailbox
                    ExchangeServerController.SetDeletedMailbox(itemId, accountId);
                }

                AddDeletedUser(deletedUser);

                account.AccountType = ExchangeAccountType.DeletedUser;

                UpdateAccount(account);

                var taskId = "SCHEDULE_TASK_DELETE_EXCHANGE_ACCOUNTS";

                if (!CheckScheduleTaskRun(org.PackageId, taskId))
                {
                    AddScheduleTask(org.PackageId, taskId, "Auto Delete Exchange Account");
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static byte[] GetArchiveFileBinaryChunk(int packageId, int itemId, int deleteAccountId, int offset, int length)
        {
            var user = GetDeletedUser(deleteAccountId);

            var path = Path.Combine(user.StoragePath, user.FolderName, user.FileName);

            var os = GetOS(packageId);

            if (os != null)
            {
                return os.GetFileBinaryChunk(path, offset, length);
            }

            var folder = _foldersManager.GetFolder(itemId, StorageSpaceFolderTypes.DeletedUsersData.ToString());

            if (folder == null)
            {
                return null;
            }

            return StorageSpacesController.GetFileBinaryChunk(folder.StorageSpaceId, path, offset, length);
        }

        private static bool UsingStorageSpaces(int serviceId)
        {
            var settings = ServerController.GetServiceSettings(serviceId);

            if (settings == null)
            {
                return false;
            }

            if (!settings.ContainsKey(UseStorageSpaces))
            {
                return false;
            }

            if (string.IsNullOrEmpty(settings[UseStorageSpaces]))
            {
                return false;
            }

            return Convert.ToBoolean(settings[UseStorageSpaces]);
        }

        private static bool CheckScheduleTaskRun(int packageId, string taskId)
        {
            var schedules = new List<ScheduleInfo>();

            ObjectUtils.FillCollectionFromDataSet(schedules, SchedulerController.GetSchedules(packageId));

            foreach(var schedule in schedules)
            {
                if (schedule.TaskId == taskId)
                {
                    return true;
                }
            }

            return false;
        }

        private static int AddScheduleTask(int packageId, string taskId, string taskName)
        {
            return SchedulerController.AddSchedule(new ScheduleInfo
                {
                    PackageId = packageId,
                    TaskId = taskId,
                    ScheduleName = taskName,
                    ScheduleTypeId = "Daily",
                    FromTime = new DateTime(2000, 1, 1, 0, 0, 0),
                    ToTime = new DateTime(2000, 1, 1, 23, 59, 59),
                    Interval = 3600,
                    StartTime = new DateTime(2000, 01, 01, 0, 30, 0),
                    MaxExecutionTime = 3600,
                    PriorityId = "Normal",
                    Enabled = true,
                    WeekMonthDay = 1,
                    HistoriesNumber = 0
                });
        }

        private static bool CheckFolderExists(int packageId, string path, string folderName)
        {
            var os = GetOS(packageId);

            if (os != null && os.CheckFileServicesInstallation())
            {
                return os.DirectoryExists(Path.Combine(path, folderName));
            }

            return false;
        }

        private static void CreateFolder(int packageId, string path, string folderName)
        {
            var os = GetOS(packageId);

            if (os != null && os.CheckFileServicesInstallation())
            {
                os.CreateDirectory(Path.Combine(path, folderName));
            }
        }

        private static void RemoveArchive(int packageId, string path, string folderName, string fileName)
        {
            var os = GetOS(packageId);

            if (os != null && os.CheckFileServicesInstallation())
            {
                os.DeleteFile(Path.Combine(path, folderName, fileName));
            }
        }

        private static string GetLocationDrive(string path)
        {
            var drive = System.IO.Path.GetPathRoot(path);

            return drive.Split(':')[0];
        }

        private static string GetDirectory(string path)
        {
            var drive = System.IO.Path.GetPathRoot(path);
            
            return path.Replace(drive, string.Empty);
        }

        private static void SetFRSMQuotaOnFolder(int packageId, string path, string folderName, QuotaValueInfo quotaInfo, QuotaType quotaType)
        {
            var os = GetOS(packageId);

            if (os != null && os.CheckFileServicesInstallation())
            {
                #region figure Quota Unit

                // Quota Unit
                string unit = string.Empty;
                if (quotaInfo.QuotaDescription.ToLower().Contains("gb"))
                    unit = "GB";
                else if (quotaInfo.QuotaDescription.ToLower().Contains("mb"))
                    unit = "MB";
                else
                    unit = "KB";

                #endregion

                os.SetQuotaLimitOnFolder(
                    Path.Combine(GetDirectory(path), folderName),
                    GetLocationDrive(path), quotaType,
                    quotaInfo.QuotaAllocatedValuePerOrganization.ToString() + unit,
                    0, String.Empty, String.Empty);
            }
        }

        #endregion

        public static int DeleteUser(int itemId, int accountId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "REMOVE_USER", itemId);

            try
            {
                Guid crmUserId = CRMController.GetCrmUserId(accountId);
                if (crmUserId != Guid.Empty)
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_CRM_USER;
                }

                if (DataProvider.CheckOCSUserExists(accountId))
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_OCS_USER;
                }

                if (DataProvider.CheckLyncUserExists(accountId))
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_LYNC_USER;
                }

                if (DataProvider.CheckSfBUserExists(accountId))
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_SFB_USER;
                }

                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // load account
                ExchangeAccount user = ExchangeServerController.GetAccount(itemId, accountId);
                
                // Log Extension
                LogExtension.SetItemName(user.DisplayName);

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                string account = GetAccountName(user.AccountName);

                var accountType = user.AccountType;

                if (accountType == ExchangeAccountType.DeletedUser)
                {
                    var deletedUser = GetDeletedUser(user.AccountId);

                    if (deletedUser != null)
                    {
                        accountType = deletedUser.OriginAT;

                        if (!deletedUser.IsArchiveEmpty)
                        {
                            RemoveArchive(org.PackageId, deletedUser.StoragePath, deletedUser.FolderName, deletedUser.FileName);
                        }

                        RemoveDeletedUser(deletedUser.Id);
                    }
                }

                if (user.AccountType == ExchangeAccountType.User )
                {
                    //Delete user from AD
                    orgProxy.DeleteUser(account, org.OrganizationId);
                    // unregister account
                    DeleteUserFromMetabase(itemId, accountId);
                }
                else
                {
                    //Delete mailbox with AD user
                    ExchangeServerController.DeleteMailbox(itemId, accountId);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static OrganizationDeletedUser GetDeletedUser(int accountId)
        {
            OrganizationDeletedUser deletedUser = ObjectUtils.FillObjectFromDataReader<OrganizationDeletedUser>(
                DataProvider.GetOrganizationDeletedUser(accountId));

            if (deletedUser == null)
                return null;

            deletedUser.IsArchiveEmpty = string.IsNullOrEmpty(deletedUser.FileName);

            return deletedUser;
        }

        private static int AddDeletedUser(OrganizationDeletedUser deletedUser)
        {
            return DataProvider.AddOrganizationDeletedUser(
                deletedUser.AccountId, (int)deletedUser.OriginAT, deletedUser.StoragePath, deletedUser.FolderName, deletedUser.FileName, deletedUser.ExpirationDate);
        }

        private static void RemoveDeletedUser(int id)
        {
            DataProvider.DeleteOrganizationDeletedUser(id);
        }

        public static OrganizationUser GetAccount(int itemId, int userId, bool withLog = true)
        {
            OrganizationUser account = ObjectUtils.FillObjectFromDataReader<OrganizationUser>(
                DataProvider.GetExchangeAccount(itemId, userId));

            if (account == null)
                return null;

            // Log Extension
            if (withLog)
                LogExtension.WriteObject(account);

            return account;
        }

        public static OrganizationUser GetAccountByAccountName(int itemId, string AccountName)
        {
            OrganizationUser account = ObjectUtils.FillObjectFromDataReader<OrganizationUser>(
                DataProvider.GetExchangeAccountByAccountName(itemId, AccountName));

            if (account == null)
                return null;

            return account;
        }


        private static void DeleteUserFromMetabase(int itemId, int accountId)
        {
            // try to get organization
            if (GetOrganization(itemId, false) == null)
                return;

            DataProvider.DeleteExchangeAccount(itemId, accountId);
        }

        public static OrganizationUser GetUserGeneralSettings(int itemId, int accountId)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                return GetDemoUserGeneralSettings();
            }
            #endregion

            // place log record
            //TaskManager.StartTask("ORGANIZATION", "GET_USER_GENERAL", itemId);

            OrganizationUser account = null;
            Organization org = null;

            try
            {
                // load organization
                org = GetOrganization(itemId, false);
                if (org == null)
                    return null;

                // load account
                account = GetAccount(itemId, accountId, false);
            }
            catch (Exception) { }

            try
            {

                // get mailbox settings
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                string accountName = GetAccountName(account.AccountName);


                OrganizationUser retUser = orgProxy.GetUserGeneralSettings(accountName, org.OrganizationId);
                retUser.AccountId = accountId;
                retUser.AccountName = account.AccountName;
                retUser.PrimaryEmailAddress = account.PrimaryEmailAddress;
                retUser.AccountType = account.AccountType;
                retUser.CrmUserId = CRMController.GetCrmUserId(accountId);
                retUser.IsOCSUser = DataProvider.CheckOCSUserExists(accountId);
                retUser.IsLyncUser = DataProvider.CheckLyncUserExists(accountId);
                retUser.IsSfBUser = DataProvider.CheckSfBUserExists(accountId);
                retUser.IsBlackBerryUser = BlackBerryController.CheckBlackBerryUserExists(accountId);
                retUser.SubscriberNumber = account.SubscriberNumber;
                retUser.LevelId = account.LevelId;
                retUser.IsVIP = account.IsVIP;

                return retUser;
            }
            catch { }
            finally
            {
                //TaskManager.CompleteTask();
            }

            return (account);
        }

        public static OrganizationUser GetUserGeneralSettingsWithExtraData(int itemId, int accountId)
        {
            OrganizationUser account = null;
            Organization org = null;

            try
            {
                // load organization
                org = GetOrganization(itemId);
                if (org == null)
                    return null;

                // load account
                account = GetAccount(itemId, accountId);
            }
            catch (Exception) { }

            try
            {
                // get mailbox settings
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                string accountName = GetAccountName(account.AccountName);


                OrganizationUser retUser = orgProxy.GetOrganizationUserWithExtraData(accountName, org.OrganizationId);
                retUser.AccountId = accountId;
                retUser.ItemId = itemId;
                retUser.AccountName = account.AccountName;
                retUser.PrimaryEmailAddress = account.PrimaryEmailAddress;
                retUser.AccountType = account.AccountType;
                retUser.CrmUserId = CRMController.GetCrmUserId(accountId);
                retUser.IsOCSUser = DataProvider.CheckOCSUserExists(accountId);
                retUser.IsLyncUser = DataProvider.CheckLyncUserExists(accountId);
                retUser.IsSfBUser = DataProvider.CheckSfBUserExists(accountId);
                retUser.IsBlackBerryUser = BlackBerryController.CheckBlackBerryUserExists(accountId);
                retUser.SubscriberNumber = account.SubscriberNumber;
                retUser.LevelId = account.LevelId;
                retUser.IsVIP = account.IsVIP;

                return retUser;
            }
            catch { }

            return (account);
        }

        public static int SetUserGeneralSettings(int itemId, int accountId, string displayName,
            string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials,
            string lastName, string address, string city, string state, string zip, string country,
            string jobTitle, string company, string department, string office, string managerAccountName,
            string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP,
            bool userMustChangePassword)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "UPDATE_USER_GENERAL", displayName, itemId);
            
            try
            {
                displayName = displayName.Trim();
                firstName = firstName.Trim();
                lastName = lastName.Trim();

                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                // load account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);

                // Log Extension
                LogExtension.WriteVariables(new {notes});

                string accountName = GetAccountName(account.AccountName);
                // get mailbox settings
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                // external email
                string externalEmailAddress = (account.AccountType == ExchangeAccountType.User) ? externalEmail : account.PrimaryEmailAddress;

                var oldOrgUser = orgProxy.GetUserGeneralSettings(accountName, org.OrganizationId);

                orgProxy.SetUserGeneralSettings(
                    org.OrganizationId,
                    accountName,
                    displayName,
                    password,
                    hideAddressBook,
                    disabled,
                    locked,
                    firstName,
                    initials,
                    lastName,
                    address,
                    city,
                    state,
                    zip,
                    country,
                    jobTitle,
                    company,
                    department,
                    office,
                    managerAccountName,
                    businessPhone,
                    fax,
                    homePhone,
                    mobilePhone,
                    pager,
                    webPage,
                    notes,
                    externalEmailAddress,
                    userMustChangePassword);

                var newOrgUser = orgProxy.GetUserGeneralSettings(accountName, org.OrganizationId);

                // Log Extension
                account.LogPropertyIfChanged(a => a.DisplayName, displayName);
                account.LogPropertyIfChanged(a => a.SubscriberNumber, subscriberNumber);
                account.LogPropertyIfChanged(a => a.LevelId, levelId);
                account.LogPropertyIfChanged(a => a.IsVIP, isVIP);
                LogExtension.LogPropertiesIfChanged(oldOrgUser, newOrgUser);

                // update account
                account.DisplayName = displayName;
                account.SubscriberNumber = subscriberNumber;
                account.LevelId = levelId;
                account.IsVIP = isVIP;

                UpdateAccount(account);
                UpdateAccountServiceLevelSettings(account);


                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }


        public static int SetUserPrincipalName(int itemId, int accountId, string userPrincipalName, bool inherit)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;


            // place log record
            TaskManager.StartTask("ORGANIZATION", "SET_USER_USERPRINCIPALNAME", itemId);

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                // load account
                OrganizationUser user = GetUserGeneralSettings(itemId, accountId);
               
                // Log Extension
                LogExtension.SetItemName(user.DisplayName);
                LogExtension.WriteObject(user);
                LogExtension.WriteVariables(new {inherit});
                user.LogPropertyIfChanged(u => u.UserPrincipalName, userPrincipalName);

                if (user.UserPrincipalName != userPrincipalName)
                {
                    bool userPrincipalNameOwned = false;
                    ExchangeEmailAddress[] emails = ExchangeServerController.GetMailboxEmailAddresses(itemId, accountId);

                    foreach (ExchangeEmailAddress mail in emails)
                    {
                        if (mail.EmailAddress == userPrincipalName)
                        {
                            userPrincipalNameOwned = true;
                            break;
                        }
                    }

                    if (!userPrincipalNameOwned)
                    {
                        if (EmailAddressExists(userPrincipalName))
                            return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;
                    }
                }

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                orgProxy.SetUserPrincipalName(org.OrganizationId,
                                            user.AccountName,
                                            userPrincipalName.ToLower());

                DataProvider.UpdateExchangeAccountUserPrincipalName(accountId, userPrincipalName.ToLower());

                if (inherit)
                {
                    if (user.AccountType == ExchangeAccountType.Mailbox)
                    {
                        ExchangeServerController.AddMailboxEmailAddress(itemId, accountId, userPrincipalName.ToLower());
                        ExchangeServerController.SetMailboxPrimaryEmailAddress(itemId, accountId, userPrincipalName.ToLower());
                    }
                    else
                    {
                        if (user.IsLyncUser)
                        {
                            if (!DataProvider.LyncUserExists(accountId, userPrincipalName.ToLower()))
                            {
                                LyncController.SetLyncUserGeneralSettings(itemId, accountId, userPrincipalName.ToLower(), null);
                            }
                        }
                        if (user.IsSfBUser)
                        {
                            if (!DataProvider.SfBUserExists(accountId, userPrincipalName.ToLower()))
                            {
                                SfBController.SetSfBUserGeneralSettings(itemId, accountId, userPrincipalName.ToLower(), null);
                            }
                        }
                        else
                        {
                            if (user.IsOCSUser)
                            {
                                OCSServer ocs = GetOCSProxy(itemId);
                                string instanceId = DataProvider.GetOCSUserInstanceID(user.AccountId);
                                ocs.SetUserPrimaryUri(instanceId, userPrincipalName.ToLower());
                            }
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }


        }


        public static int SetUserPassword(int itemId, int accountId, string password)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "SET_USER_PASSWORD", itemId);

            TaskManager.Write("ItemId: {0}", itemId.ToString());
            TaskManager.Write("AccountId: {0}", accountId.ToString());

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                // load account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);

                string accountName = GetAccountName(account.AccountName);

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                orgProxy.SetUserPassword(org.OrganizationId,
                                            accountName,
                                            password);

                //account.

                UpdateAccount(account);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static void UpdateAccountServiceLevelSettings(ExchangeAccount account)
        {
            DataProvider.UpdateExchangeAccountServiceLevelSettings(account.AccountId, account.LevelId, account.IsVIP);
        }

        private static void UpdateAccount(ExchangeAccount account)
        {
            DataProvider.UpdateExchangeAccount(account.AccountId, account.AccountName, account.AccountType, account.DisplayName,
                account.PrimaryEmailAddress, account.MailEnabledPublicFolder,
                account.MailboxManagerActions.ToString(), account.SamAccountName, account.MailboxPlanId, account.ArchivingMailboxPlanId,
                (string.IsNullOrEmpty(account.SubscriberNumber) ? null : account.SubscriberNumber.Trim()),
                account.EnableArchiving);
        }


        public static List<OrganizationUser> SearchAccounts(int itemId,

            string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                List<OrganizationUser> demoAccounts = new List<OrganizationUser>();


                OrganizationUser m1 = new OrganizationUser();
                m1.AccountId = 1;
                m1.AccountName = "john_fabrikam";
                m1.AccountType = ExchangeAccountType.Mailbox;
                m1.DisplayName = "John Smith";
                m1.PrimaryEmailAddress = "john@fabrikam.net";

                if (includeMailboxes)
                    demoAccounts.Add(m1);

                OrganizationUser m2 = new OrganizationUser();
                m2.AccountId = 2;
                m2.AccountName = "jack_fabrikam";
                m2.AccountType = ExchangeAccountType.User;
                m2.DisplayName = "Jack Brown";
                m2.PrimaryEmailAddress = "jack@fabrikam.net";
                demoAccounts.Add(m2);

                OrganizationUser m3 = new OrganizationUser();
                m3.AccountId = 3;
                m3.AccountName = "marry_fabrikam";
                m3.AccountType = ExchangeAccountType.Mailbox;
                m3.DisplayName = "Marry Smith";
                m3.PrimaryEmailAddress = "marry@fabrikam.net";

                if (includeMailboxes)
                    demoAccounts.Add(m3);


                return demoAccounts;
            }
            #endregion

            List<OrganizationUser> Tmpaccounts = ObjectUtils.CreateListFromDataReader<OrganizationUser>(
                                                  DataProvider.SearchOrganizationAccounts(SecurityContext.User.UserId, itemId,
                                                  filterColumn, filterValue, sortColumn, includeMailboxes));

            return Tmpaccounts;

            // on large lists is very slow
            /*
            List<OrganizationUser> Accounts = new List<OrganizationUser>();

            foreach (OrganizationUser user in Tmpaccounts.ToArray())
            {
                Accounts.Add(GetUserGeneralSettings(itemId, user.AccountId));
            }

            return Accounts;
             */
        }

        public static int GetAccountIdByUserPrincipalName(int itemId, string userPrincipalName)
        {
            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_ACCOUNT_BYUPN", itemId);

            int accounId = -1;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return 0;

                // get samaccountName
                //Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                //string accountName = orgProxy.GetSamAccountNameByUserPrincipalName(org.OrganizationId, userPrincipalName);

                // load account
                OrganizationUser account = GetAccountByAccountName(itemId, userPrincipalName);

                if (account != null)
                    accounId = account.AccountId;

                return accounId;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }


        #endregion

        public static List<OrganizationDomainName> GetOrganizationDomains(int itemId)
        {

            #region Demo Mode
            if (IsDemoMode)
            {
                List<OrganizationDomainName> demoDomains = new List<OrganizationDomainName>();
                OrganizationDomainName d1 = new OrganizationDomainName();
                d1.DomainId = 1;
                d1.DomainName = "fabrikam.hosted-exchange.com";
                d1.IsDefault = false;
                d1.IsHost = true;
                demoDomains.Add(d1);

                OrganizationDomainName d2 = new OrganizationDomainName();
                d2.DomainId = 2;
                d2.DomainName = "fabrikam.net";
                d2.IsDefault = true;
                d2.IsHost = false;
                demoDomains.Add(d2);

                return demoDomains;
            }
            #endregion


            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return null;

            // load all domains
            List<OrganizationDomainName> domains = ObjectUtils.CreateListFromDataReader<OrganizationDomainName>(
                DataProvider.GetExchangeOrganizationDomains(itemId));

            // set default domain
            foreach (OrganizationDomainName domain in domains)
            {
                if (String.Compare(domain.DomainName, org.DefaultDomain, true) == 0)
                {
                    domain.IsDefault = true;
                    break;
                }
            }

            return domains;
        }

        private static OrganizationUser GetDemoUserGeneralSettings()
        {
            OrganizationUser user = new OrganizationUser();
            user.DisplayName = "John Smith";
            user.AccountName = "john_fabrikam";
            user.FirstName = "John";
            user.LastName = "Smith";
            user.AccountType = ExchangeAccountType.Mailbox;
            return user;
        }

        private static bool IsDemoMode
        {
            get
            {
                return (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);
            }
        }


        public static PasswordPolicyResult GetPasswordPolicy(int itemId)
        {
            PasswordPolicyResult res = new PasswordPolicyResult { IsSuccess = true };
            try
            {
                Organization org = GetOrganization(itemId);
                if (org == null)
                {
                    res.IsSuccess = false;
                    res.ErrorCodes.Add(ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID);
                    return res;
                }

                Organizations orgProxy;
                try
                {
                    orgProxy = GetOrganizationProxy(org.ServiceId);
                }
                catch (Exception ex)
                {
                    res.IsSuccess = false;
                    res.ErrorCodes.Add(ErrorCodes.CANNOT_GET_ORGANIZATION_PROXY);
                    TaskManager.WriteError(ex);
                    return res;
                }

                PasswordPolicyResult policyRes = orgProxy.GetPasswordPolicy();
                res.ErrorCodes.AddRange(policyRes.ErrorCodes);
                if (!policyRes.IsSuccess)
                {
                    res.IsSuccess = false;
                    return res;
                }

                res.Value = policyRes.Value;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                res.IsSuccess = false;
            }
            return res;
        }

        private static OCSServer GetOCSProxy(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);

            OCSServer ocs = new OCSServer();
            ServiceProviderProxy.Init(ocs, serviceId);


            return ocs;
        }

        private static int AddAccount(int itemId, ExchangeAccountType accountType,
            string accountName, string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            MailboxManagerActions mailboxManagerActions, string samAccountName, string accountPassword, int mailboxPlanId, string subscriberNumber)
        {
            return DataProvider.AddExchangeAccount(itemId, (int)accountType,
                accountName, displayName, primaryEmailAddress, mailEnabledPublicFolder,
                mailboxManagerActions.ToString(), samAccountName, mailboxPlanId, (string.IsNullOrEmpty(subscriberNumber) ? null : subscriberNumber.Trim()));
        }

        #region Additional Default Groups

        public static List<AdditionalGroup> GetAdditionalGroups(int userId)
        {
            List<AdditionalGroup> additionalGroups = new List<AdditionalGroup>();

            IDataReader reader = DataProvider.GetAdditionalGroups(userId);

            while (reader.Read())
            {
                additionalGroups.Add(new AdditionalGroup
                {
                    GroupId = (int)reader["ID"],
                    GroupName = (string)reader["GroupName"]
                });
            }

            reader.Close();

            return additionalGroups;
        }

        public static void UpdateAdditionalGroup(int groupId, string groupName)
        {
            DataProvider.UpdateAdditionalGroup(groupId, groupName);
        }

        public static void DeleteAdditionalGroup(int groupId)
        {
            DataProvider.DeleteAdditionalGroup(groupId);
        }

        public static int AddAdditionalGroup(int userId, string groupName)
        {
            return DataProvider.AddAdditionalGroup(userId, groupName);
        }

        #endregion

        public static int CreateSecurityGroup(int itemId, string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException("displayName");

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;


            // place log record
            TaskManager.StartTask("ORGANIZATION", "CREATE_SECURITY_GROUP", displayName, itemId);
            
            // Log Extension
            LogExtension.WriteVariables(new {displayName});

            int securityGroupId = -1;

            try
            {
                displayName = displayName.Trim();

                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                {
                    return -1;
                }

                StringDictionary serviceSettings = ServerController.GetServiceSettings(org.ServiceId);

                if (serviceSettings == null)
                {
                    return -1;
                }

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                string groupName = BuildAccountNameEx(org, displayName.Replace(" ", ""));
                
                // Log Extension
                LogExtension.WriteVariables(new {groupName});

                if (orgProxy.CreateSecurityGroup(org.OrganizationId, groupName) == 0)
                {
                    OrganizationSecurityGroup retSecurityGroup = orgProxy.GetSecurityGroupGeneralSettings(groupName, org.OrganizationId);
                    
                    // Log Extension
                    LogExtension.WriteObject(retSecurityGroup);

                    securityGroupId = AddAccount(itemId, ExchangeAccountType.SecurityGroup, groupName,
                                                    displayName, null, false,
                                                    0, retSecurityGroup.SAMAccountName, null, 0, null);
                }
                else
                {
                    TaskManager.WriteError("Failed to create securitygroup");
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return securityGroupId;
        }

        private static OrganizationSecurityGroup GetDemoSecurityGroupGeneralSettings()
        {
            OrganizationSecurityGroup c = new OrganizationSecurityGroup();
            c.DisplayName = "Fabrikam Sales";
            c.AccountName = "sales_fabrikam";

            return c;
        }

        public static OrganizationSecurityGroup GetSecurityGroupGeneralSettings(int itemId, int accountId)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                return GetDemoSecurityGroupGeneralSettings();
            }
            #endregion

            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_SECURITY_GROUP_GENERAL", itemId);

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return null;

                OrganizationUser account = GetAccount(itemId, accountId);

                // get mailbox settings
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                //OrganizationSecurityGroup securityGroup = orgProxy.GetSecurityGroupGeneralSettings(account.AccountName, org.OrganizationId);
                string groupName;
                string[] parts = account.SamAccountName.Split('\\');
                if (parts.Length == 2) 
                    groupName = parts[1];
                else 
                    groupName = account.SamAccountName;

                OrganizationSecurityGroup securityGroup = orgProxy.GetSecurityGroupGeneralSettings(groupName, org.OrganizationId);

                securityGroup.DisplayName = account.DisplayName;

                securityGroup.IsDefault = account.AccountType == ExchangeAccountType.DefaultSecurityGroup;

                List<ExchangeAccount> members = new List<ExchangeAccount>();

                foreach (ExchangeAccount user in securityGroup.MembersAccounts)
                {
                    OrganizationUser userAccount = GetAccountByAccountName(itemId, user.AccountName);

                    if (userAccount != null)
                    {
                        user.AccountId = userAccount.AccountId;
                        user.AccountName = userAccount.AccountName;
                        user.DisplayName = userAccount.DisplayName;
                        user.PrimaryEmailAddress = userAccount.PrimaryEmailAddress;
                        user.AccountType = userAccount.AccountType;

                        members.Add(user);
                    }
                }

                securityGroup.MembersAccounts = members.ToArray();

                return securityGroup;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteSecurityGroup(int itemId, int accountId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "DELETE_SECURITY_GROUP", itemId);

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // load account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);
                
                // Log Extension
                LogExtension.SetItemName(account.DisplayName);

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                orgProxy.DeleteSecurityGroup(account.AccountName, org.OrganizationId);

                DeleteUserFromMetabase(itemId, accountId);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int SetSecurityGroupGeneralSettings(int itemId, int accountId, string displayName, string[] memberAccounts, string notes)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "UPDATE_SECURITY_GROUP_GENERAL", displayName, itemId);
            
            try
            {
                displayName = displayName.Trim();

                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                // load account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);

                // Log Extension
                LogExtension.WriteVariables(new {notes});

                string accountName = GetAccountName(account.AccountName);
                // get mailbox settings
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                // external email

                var oldObj = orgProxy.GetSecurityGroupGeneralSettings(accountName, org.OrganizationId);
                
                orgProxy.SetSecurityGroupGeneralSettings(
                    org.OrganizationId,
                    accountName,
                    memberAccounts,
                    notes);

                var newObj = orgProxy.GetSecurityGroupGeneralSettings(accountName, org.OrganizationId);

                // Log Extension
                account.LogPropertyIfChanged(a => a.DisplayName, displayName);
                LogExtension.LogPropertiesIfChanged(oldObj, newObj);
                
                // update account
                account.DisplayName = displayName;

                UpdateAccount(account);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ExchangeAccountsPaged GetOrganizationSecurityGroupsPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
            int startRow, int maximumRows)
        {

            #region Demo Mode
            if (IsDemoMode)
            {
                ExchangeAccountsPaged res = new ExchangeAccountsPaged();
                List<ExchangeAccount> demoSecurityGroups = new List<ExchangeAccount>();

                ExchangeAccount r1 = new ExchangeAccount();
                r1.AccountId = 20;
                r1.AccountName = "group1_fabrikam";
                r1.AccountType = ExchangeAccountType.SecurityGroup;
                r1.DisplayName = "Group 1";
                demoSecurityGroups.Add(r1);

                ExchangeAccount r2 = new ExchangeAccount();
                r1.AccountId = 21;
                r1.AccountName = "group2_fabrikam";
                r1.AccountType = ExchangeAccountType.SecurityGroup;
                r1.DisplayName = "Group 2";
                demoSecurityGroups.Add(r2);


                res.PageItems = demoSecurityGroups.ToArray();
                res.RecordsCount = res.PageItems.Length;

                return res;
            }
            #endregion

            string accountTypes = string.Format("{0}, {1}", ((int)ExchangeAccountType.SecurityGroup), ((int)ExchangeAccountType.DefaultSecurityGroup));

            DataSet ds =
                DataProvider.GetExchangeAccountsPaged(SecurityContext.User.UserId, itemId, accountTypes, filterColumn,
                                                      filterValue, sortColumn, startRow, maximumRows, false);

            ExchangeAccountsPaged result = new ExchangeAccountsPaged();
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<ExchangeAccount> Tmpaccounts = new List<ExchangeAccount>();
            ObjectUtils.FillCollectionFromDataView(Tmpaccounts, ds.Tables[1].DefaultView);
            result.PageItems = Tmpaccounts.ToArray();

            List<ExchangeAccount> accounts = new List<ExchangeAccount>();

            foreach (ExchangeAccount account in Tmpaccounts.ToArray())
            {
                OrganizationSecurityGroup tmpSecurityGroup = GetSecurityGroupGeneralSettings(itemId, account.AccountId);

                if (tmpSecurityGroup != null)
                {
                    account.Notes = tmpSecurityGroup.Notes;
                    accounts.Add(account);
                }
            }

            result.PageItems = accounts.ToArray();

            return result;
        }

        public static int AddObjectToSecurityGroup(int itemId, int accountId, string groupName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "ADD_USER_TO_SECURITY_GROUP", itemId);

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // load user account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);

                // Log Extension
                LogExtension.SetItemName(account.PrimaryEmailAddress);
                LogExtension.WriteVariables(new { groupName });

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                orgProxy.AddObjectToSecurityGroup(org.OrganizationId, account.AccountName, groupName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteObjectFromSecurityGroup(int itemId, int accountId, string groupName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "DELETE_USER_FROM_SECURITY_GROUP", itemId);
            
            // Log Extension
            LogExtension.WriteVariables(new {groupName});

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // load user account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);
                
                // Log Extension
                LogExtension.SetItemName(account.DisplayName);

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                orgProxy.DeleteObjectFromSecurityGroup(org.OrganizationId, account.AccountName, groupName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ExchangeAccount[] GetSecurityGroupsByMember(int itemId, int accountId)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                return null;
            }
            #endregion

            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_SECURITY_GROUPS_BYMEMBER");
            TaskManager.ItemId = itemId;

            List<ExchangeAccount> ret = new List<ExchangeAccount>();

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return null;

                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                // load account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);

                List<ExchangeAccount> securytyGroups = ExchangeServerController.GetAccounts(itemId, ExchangeAccountType.SecurityGroup);

                //load default group
                securytyGroups.AddRange(ExchangeServerController.GetAccounts(itemId, ExchangeAccountType.DefaultSecurityGroup));

                foreach (ExchangeAccount securityGroupAccount in securytyGroups)
                {
                    OrganizationSecurityGroup securityGroup = GetSecurityGroupGeneralSettings(itemId, securityGroupAccount.AccountId);

                    foreach (ExchangeAccount member in securityGroup.MembersAccounts)
                    {
                        if (member.AccountName == account.AccountName)
                        {
                            ret.Add(securityGroupAccount);
                            break;
                        }

                    }
                }

                return ret.ToArray();
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static List<ExchangeAccount> SearchOrganizationAccounts(int itemId, string filterColumn, string filterValue,
            string sortColumn, bool includeOnlySecurityGroups)
        {
            #region Demo Mode

            if (IsDemoMode)
            {
                List<ExchangeAccount> demoAccounts = new List<ExchangeAccount>();

                ExchangeAccount m1 = new ExchangeAccount();
                m1.AccountId = 1;
                m1.AccountName = "john_fabrikam";
                m1.AccountType = ExchangeAccountType.Mailbox;
                m1.DisplayName = "John Smith";
                m1.PrimaryEmailAddress = "john@fabrikam.net";
                demoAccounts.Add(m1);

                ExchangeAccount m2 = new ExchangeAccount();
                m2.AccountId = 2;
                m2.AccountName = "jack_fabrikam";
                m2.AccountType = ExchangeAccountType.User;
                m2.DisplayName = "Jack Brown";
                m2.PrimaryEmailAddress = "jack@fabrikam.net";
                demoAccounts.Add(m2);

                ExchangeAccount m3 = new ExchangeAccount();
                m3.AccountId = 3;
                m3.AccountName = "marry_fabrikam";
                m3.AccountType = ExchangeAccountType.Mailbox;
                m3.DisplayName = "Marry Smith";
                m3.PrimaryEmailAddress = "marry@fabrikam.net";
                demoAccounts.Add(m3);

                ExchangeAccount r1 = new ExchangeAccount();
                r1.AccountId = 20;
                r1.AccountName = "group1_fabrikam";
                r1.AccountType = ExchangeAccountType.SecurityGroup;
                r1.DisplayName = "Group 1";
                demoAccounts.Add(r1);

                ExchangeAccount r2 = new ExchangeAccount();
                r1.AccountId = 21;
                r1.AccountName = "group2_fabrikam";
                r1.AccountType = ExchangeAccountType.SecurityGroup;
                r1.DisplayName = "Group 2";
                demoAccounts.Add(r2);

                return demoAccounts;
            }

            #endregion

            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return null;

            string accountTypes = string.Format("{0}", ((int)ExchangeAccountType.SecurityGroup));

            if (!includeOnlySecurityGroups)
            {
                accountTypes = string.Format("{0}, {1}", accountTypes, ((int)ExchangeAccountType.User));

                int exchangeServiceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.Exchange);

                if (exchangeServiceId != 0)
                {
                    accountTypes = string.Format("{0}, {1}, {2}, {3}, {4}", accountTypes, ((int)ExchangeAccountType.Mailbox),
                    ((int)ExchangeAccountType.Room), ((int)ExchangeAccountType.Equipment), ((int)ExchangeAccountType.DistributionList));
                }
            }

            List<ExchangeAccount> tmpAccounts = ObjectUtils.CreateListFromDataReader<ExchangeAccount>(
                                                  DataProvider.SearchExchangeAccountsByTypes(SecurityContext.User.UserId, itemId,
                                                  accountTypes, filterColumn, filterValue, sortColumn));

            return tmpAccounts;

            // on large lists is very slow

            //List<ExchangeAccount> accounts = new List<ExchangeAccount>();

            //foreach (ExchangeAccount tmpAccount in tmpAccounts.ToArray())
            //{
            //    bool bSuccess = false;

            //    switch (tmpAccount.AccountType)
            //    {
            //        case ExchangeAccountType.SecurityGroup:
            //            bSuccess = GetSecurityGroupGeneralSettings(itemId, tmpAccount.AccountId) != null;
            //            break;
            //        case ExchangeAccountType.DistributionList:
            //            bSuccess = ExchangeServerController.GetDistributionListGeneralSettings(itemId, tmpAccount.AccountId) != null;
            //            break;
            //        default:
            //            bSuccess = GetUserGeneralSettings(itemId, tmpAccount.AccountId) != null;
            //            break;
            //    }

            //    if (bSuccess)
            //    {
            //        accounts.Add(tmpAccount);
            //    }
            //}

            //return accounts;
        }


        #region Service Levels

        public static int AddSupportServiceLevel(string levelName, string levelDescription)
        {
            if (string.IsNullOrEmpty(levelName))
                throw new ArgumentNullException("levelName");

            // place log record
            TaskManager.StartTask("ORGANIZATION", "ADD_SUPPORT_SERVICE_LEVEL");

            int levelID = 0;

            try
            {
                levelID = DataProvider.AddSupportServiceLevel(levelName, levelDescription);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return levelID;
        }

        public static ResultObject DeleteSupportServiceLevel(int levelId)
        {
            ResultObject res = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "DELETE_SUPPORT_SERVICE_LEVEL", levelId);

            try
            {
                if (CheckServiceLevelUsage(levelId)) res.AddError("SERVICE_LEVEL_IN_USE", new ApplicationException("Service Level is being used"));

                if (res.IsSuccess)
                DataProvider.DeleteSupportServiceLevel(levelId);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                TaskManager.CompleteResultTask(res);
                res.AddError("", ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription)
        {
            // place log record
            TaskManager.StartTask("ORGANIZATION", "UPDATE_SUPPORT_SERVICE_LEVEL", levelID);

            try
            {
                // Log Extension
                LogExtension.WriteVariables(new { levelID, levelName, levelDescription }); 
                
                DataProvider.UpdateSupportServiceLevel(levelID, levelName, levelDescription);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ServiceLevel[] GetSupportServiceLevels()
        {
            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_SUPPORT_SERVICE_LEVELS");

            try
            {
                return ObjectUtils.CreateListFromDataReader<ServiceLevel>(DataProvider.GetSupportServiceLevels()).ToArray();
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ServiceLevel GetSupportServiceLevel(int levelID)
        {
            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_SUPPORT_SERVICE_LEVEL", levelID);

            try
            {
                return ObjectUtils.FillObjectFromDataReader<ServiceLevel>(
                    DataProvider.GetSupportServiceLevel(levelID));
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static bool CheckServiceLevelUsage(int levelID)
        {
            return DataProvider.CheckServiceLevelUsage(levelID);
        }

        #endregion

        #region OS

        private static SolidCP.Providers.OS.OperatingSystem GetOS(int packageId)
        {
            int sid = PackageController.GetPackageServiceId(packageId, ResourceGroups.Os);
            if (sid <= 0)
                return null;

            var os = new SolidCP.Providers.OS.OperatingSystem();
            ServiceProviderProxy.Init(os, sid);

            return os;
        }

        #endregion

        public static DataSet GetOrganizationObjectsByDomain(int itemId, string domainName)
        {
            return DataProvider.GetOrganizationObjectsByDomain(itemId, domainName);
        }
    }
}
