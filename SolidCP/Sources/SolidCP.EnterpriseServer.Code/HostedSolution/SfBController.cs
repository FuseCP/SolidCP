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

﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Xml;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.SfB;

namespace SolidCP.EnterpriseServer.Code.HostedSolution
{
    public class SfBController
    {


        public static SfBServer GetSfBServer(int sfbServiceId, int organizationServiceId)
        {
            SfBServer ws = new SfBServer();

            ServiceProviderProxy.Init(ws, sfbServiceId);

            string[] sfbSettings = ws.ServiceProviderSettingsSoapHeaderValue.Settings;

            List<string> resSettings = new List<string>(sfbSettings);

            if (organizationServiceId != -1)
            {
                ExtendSfBSettings(resSettings, "primarydomaincontroller", GetProviderProperty(organizationServiceId, "primarydomaincontroller"));
                ExtendSfBSettings(resSettings, "rootou", GetProviderProperty(organizationServiceId, "rootou"));
            }
            ws.ServiceProviderSettingsSoapHeaderValue.Settings = resSettings.ToArray();
            return ws;
        }

        private static string GetProviderProperty(int organizationServiceId, string property)
        {

            Organizations orgProxy = new Organizations();

            ServiceProviderProxy.Init(orgProxy, organizationServiceId);

            string[] organizationSettings = orgProxy.ServiceProviderSettingsSoapHeaderValue.Settings;

            string value = string.Empty;
            foreach (string str in organizationSettings)
            {
                string[] props = str.Split('=');
                if (props[0].ToLower() == property)
                {
                    value = str;
                    break;
                }
            }

            return value;
        }

        private static void ExtendSfBSettings(List<string> sfbSettings, string property, string value)
        {
            bool isAdded = false;
            for (int i = 0; i < sfbSettings.Count; i++)
            {
                string[] props = sfbSettings[i].Split('=');
                if (props[0].ToLower() == property)
                {
                    sfbSettings[i] = value;
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                sfbSettings.Add(value);
            }
        }

        private static int GetSfBServiceID(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.SfB);
        }


        private static bool CheckQuota(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

            IntResult userCount = GetSfBUsersCount(itemId);

            int allocatedUsers = cntx.Quotas[Quotas.SFB_USERS].QuotaAllocatedValuePerOrganization;

            return allocatedUsers == -1 || allocatedUsers > userCount.Value;
        }


        public static SfBUserResult CreateSfBUser(int itemId, int accountId, int sfbUserPlanId)
        {
            SfBUserResult res = TaskManager.StartResultTask<SfBUserResult>("SFB", "CREATE_SFB_USER");

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.NOT_AUTHORIZED);
                return res;
            }


            SfBUser retSfBUser = new SfBUser();
            bool isSfBUser;

            isSfBUser = DataProvider.CheckSfBUserExists(accountId);
            if (isSfBUser)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.USER_IS_ALREADY_SFB_USER);
                return res;
            }

            OrganizationUser user;
            user = OrganizationController.GetAccount(itemId, accountId);
            if (user == null)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT);
                return res;
            }

            user = OrganizationController.GetUserGeneralSettings(itemId, accountId);
            if (string.IsNullOrEmpty(user.FirstName))
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.USER_FIRST_NAME_IS_NOT_SPECIFIED);
                return res;
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.USER_LAST_NAME_IS_NOT_SPECIFIED);
                return res;
            }

            bool quota = CheckQuota(itemId);
            if (!quota)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.USER_QUOTA_HAS_BEEN_REACHED);
                return res;
            }


            SfBServer sfb;

            try
            {

                bool bReloadConfiguration = false;

                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                if (string.IsNullOrEmpty(org.SfBTenantId))
                {
                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    org.SfBTenantId = sfb.CreateOrganization(org.OrganizationId,
                                                                org.DefaultDomain,
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_CONFERENCING].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ALLOWVIDEO].QuotaAllocatedValue),
                                                                Convert.ToInt32(cntx.Quotas[Quotas.SFB_MAXPARTICIPANTS].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_FEDERATION].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ENTERPRISEVOICE].QuotaAllocatedValue));

                    if (string.IsNullOrEmpty(org.SfBTenantId))
                    {
                        TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ENABLE_ORG);
                        return res;
                    }
                    else
                    {

                        DomainInfo domain = ServerController.GetDomain(org.DefaultDomain);

                        //Add the service records
                        if (domain != null)
                        {
                            if (domain.ZoneItemId != 0)
                            {
                                ServerController.AddServiceDNSRecords(org.PackageId, ResourceGroups.SfB, domain, "");
                            }
                        }
                        
                        PackageController.UpdatePackageItem(org);

                        bReloadConfiguration = true;
                    }
                }

                if (sfb.GetOrganizationTenantId(org.OrganizationId) == string.Empty)
                {
                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    org.SfBTenantId = sfb.CreateOrganization(org.OrganizationId,
                                                                org.DefaultDomain,
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_CONFERENCING].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ALLOWVIDEO].QuotaAllocatedValue),
                                                                Convert.ToInt32(cntx.Quotas[Quotas.SFB_MAXPARTICIPANTS].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_FEDERATION].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ENTERPRISEVOICE].QuotaAllocatedValue));

                    if (string.IsNullOrEmpty(org.SfBTenantId))
                    {
                        TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ENABLE_ORG);
                        return res;
                    }
                    else
                    {
                        PackageController.UpdatePackageItem(org);

                        bReloadConfiguration = true;
                    }


                }


                SfBUserPlan plan = GetSfBUserPlan(itemId, sfbUserPlanId);

                if (!sfb.CreateUser(org.OrganizationId, user.UserPrincipalName, plan))
                {
                    TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ADD_SFB_USER);
                    return res;
                }

                if (bReloadConfiguration)
                {
                    SfBControllerAsync userWorker = new SfBControllerAsync();
                    userWorker.SfBServiceId = sfbServiceId;
                    userWorker.OrganizationServiceId = org.ServiceId;
                    userWorker.Enable_CsComputerAsync();
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ADD_SFB_USER, ex);
                return res;
            }

            try
            {
                DataProvider.AddSfBUser(accountId, sfbUserPlanId, user.UserPrincipalName);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ADD_SFB_USER_TO_DATABASE, ex);
                return res;
            }

            res.IsSuccess = true;
            TaskManager.CompleteResultTask();
            return res;

        }


        private static int[] ParseMultiSetting(int sfbServiceId, string settingName)
        {
            List<int> retIds = new List<int>();
            StringDictionary settings = ServerController.GetServiceSettings(sfbServiceId);
            if (!String.IsNullOrEmpty(settings[settingName]))
            {
                string[] ids = settings[settingName].Split(',');

                int res;
                foreach (string id in ids)
                {
                    if (int.TryParse(id, out res))
                        retIds.Add(res);
                }
            }

            if (retIds.Count == 0)
                retIds.Add(sfbServiceId);

            return retIds.ToArray();

        }


        public static void GetSfBServices(int sfbServiceId, out int[] sfbServiceIds)
        {
            sfbServiceIds = ParseMultiSetting(sfbServiceId, "SfBServersServiceID");
        }



        public static SfBUser GetSfBUserGeneralSettings(int itemId, int accountId)
        {
            TaskManager.StartTask("SFB", "GET_SFB_USER_GENERAL_SETTINGS");

            SfBUser user = null;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                OrganizationUser usr;
                usr = OrganizationController.GetAccount(itemId, accountId);

                if (usr != null)
                    user = sfb.GetSfBUserGeneralSettings(org.OrganizationId, usr.UserPrincipalName);

                if (user != null)
                {
                    SfBUserPlan plan = ObjectUtils.FillObjectFromDataReader<SfBUserPlan>(DataProvider.GetSfBUserPlanByAccountId(accountId));

                    if (plan != null)
                    {
                        user.SfBUserPlanId = plan.SfBUserPlanId;
                        user.SfBUserPlanName = plan.SfBUserPlanName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);

            }
            TaskManager.CompleteTask();
            return user;

        }

        public static SfBUserResult SetSfBUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            SfBUserResult res = TaskManager.StartResultTask<SfBUserResult>("SFB", "SET_SFB_USER_GENERAL_SETTINGS");

            string PIN = "";

            string[] uriAndPin = ("" + lineUri).Split(':');

            if (uriAndPin.Length > 0) lineUri = uriAndPin[0];
            if (uriAndPin.Length > 1) PIN = uriAndPin[1];

            SfBUser user = null;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                OrganizationUser usr;
                usr = OrganizationController.GetAccount(itemId, accountId);

                if (usr != null)
                    user = sfb.GetSfBUserGeneralSettings(org.OrganizationId, usr.UserPrincipalName);

                if (user != null)
                {
                    SfBUserPlan plan = ObjectUtils.FillObjectFromDataReader<SfBUserPlan>(DataProvider.GetSfBUserPlanByAccountId(accountId));

                    if (plan != null)
                    {
                        user.SfBUserPlanId = plan.SfBUserPlanId;
                        user.SfBUserPlanName = plan.SfBUserPlanName;
                    }


                    if (!string.IsNullOrEmpty(sipAddress))
                    {
                        if (user.SipAddress != sipAddress)
                        {
                            if (sipAddress != usr.UserPrincipalName)
                            {
                                if (DataProvider.SfBUserExists(accountId, sipAddress))
                                {
                                    TaskManager.CompleteResultTask(res, SfBErrorCodes.ADDRESS_ALREADY_USED);
                                    return res;
                                }
                            }
                            user.SipAddress = sipAddress;
                        }
                    }

                    user.LineUri = lineUri;
                    user.PIN = PIN;

                    sfb.SetSfBUserGeneralSettings(org.OrganizationId, usr.UserPrincipalName, user);

                    DataProvider.UpdateSfBUser(accountId, sipAddress);
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.FAILED_SET_SETTINGS, ex);
                return res;
            }

            res.IsSuccess = true;
            TaskManager.CompleteResultTask();
            return res;

        }



        public static int DeleteOrganization(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("SFB", "DELETE_ORG", itemId);

            try
            {
                // delete organization in Exchange
                //System.Threading.Thread.Sleep(5000);
                Organization org = (Organization)PackageController.GetPackageItem(itemId);

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                bool successful = sfb.DeleteOrganization(org.OrganizationId, org.DefaultDomain);

                return successful ? 0 : BusinessErrorCodes.ERROR_SFB_DELETE_SOME_PROBLEMS;
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


        public static SfBUserResult SetUserSfBPlan(int itemId, int accountId, int sfbUserPlanId)
        {
            SfBUserResult res = TaskManager.StartResultTask<SfBUserResult>("SFB", "SET_SFB_USER_SFBPLAN");

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.NOT_AUTHORIZED);
                return res;
            }

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                SfBUserPlan plan = GetSfBUserPlan(itemId, sfbUserPlanId);

                OrganizationUser user;
                user = OrganizationController.GetAccount(itemId, accountId);

                if (!sfb.SetSfBUserPlan(org.OrganizationId, user.UserPrincipalName, plan))
                {
                    TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ADD_SFB_USER);
                    return res;
                }

                try
                {
                    DataProvider.SetSfBUserSfBUserplan(accountId, sfbUserPlanId);
                }
                catch (Exception ex)
                {
                    TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ADD_SFB_USER_TO_DATABASE, ex);
                    return res;
                }

                res.IsSuccess = true;
                TaskManager.CompleteResultTask();
                return res;
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_UPDATE_SFB_USER, ex);
                return res;
            }

        }

        public static SfBUsersPagedResult GetSfBUsers(int itemId)
        {
            return GetSfBUsersPaged(itemId, string.Empty, string.Empty, 0, int.MaxValue);
        }

        public static SfBUsersPagedResult GetSfBUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int count)
        {
            SfBUsersPagedResult res = TaskManager.StartResultTask<SfBUsersPagedResult>("SFB", "GET_SFB_USERS");

            try
            {
                IDataReader reader =
                    DataProvider.GetSfBUsers(itemId, sortColumn, sortDirection, startRow, count);
                List<SfBUser> accounts = new List<SfBUser>();
                ObjectUtils.FillCollectionFromDataReader(accounts, reader);
                res.Value = new SfBUsersPaged { PageUsers = accounts.ToArray() };
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.GET_SFB_USERS, ex);
                return res;
            }

            IntResult intRes = GetSfBUsersCount(itemId);
            res.ErrorCodes.AddRange(intRes.ErrorCodes);
            if (!intRes.IsSuccess)
            {
                TaskManager.CompleteResultTask(res);
                return res;
            }
            res.Value.RecordsCount = intRes.Value;

            TaskManager.CompleteResultTask();
            return res;
        }

        public static List<SfBUser> GetSfBUsersByPlanId(int itemId, int planId)
        {
            return ObjectUtils.CreateListFromDataReader<SfBUser>(DataProvider.GetSfBUsersByPlanId(itemId, planId));
        }

        public static IntResult GetSfBUsersCount(int itemId)
        {
            IntResult res = TaskManager.StartResultTask<IntResult>("SFB", "GET_SFB_USERS_COUNT");
            try
            {
                res.Value = DataProvider.GetSfBUsersCount(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.GET_SFB_USER_COUNT, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static SfBUserResult DeleteSfBUser(int itemId, int accountId)
        {
            SfBUserResult res = TaskManager.StartResultTask<SfBUserResult>("SFB", "DELETE_SFB_USER");
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);

            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.NOT_AUTHORIZED);
                return res;
            }

            SfBServer sfb;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                OrganizationUser user;
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user != null)
                    sfb.DeleteUser(user.UserPrincipalName);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_DELETE_SFB_USER, ex);
                return res;
            }

            try
            {
                DataProvider.DeleteSfBUser(accountId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_DELETE_SFB_USER_FROM_METADATA, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        public static Organization GetOrganization(int itemId)
        {
            return (Organization)PackageController.GetPackageItem(itemId);
        }


        #region SfB Plans
        public static List<SfBUserPlan> GetSfBUserPlans(int itemId)
        {
            // place log record
            TaskManager.StartTask("SFB", "GET_SFB_SFBUSERPLANS", itemId);

            try
            {
                List<SfBUserPlan> plans = new List<SfBUserPlan>();

                UserInfo user = ObjectUtils.FillObjectFromDataReader<UserInfo>(DataProvider.GetUserByExchangeOrganizationIdInternally(itemId));
                
                if (user.Role == UserRole.User)
                    SfBController.GetSfBUserPlansByUser(itemId, user, ref plans);
                else
                    SfBController.GetSfBUserPlansByUser(0, user, ref plans);


                ExchangeOrganization ExchangeOrg = ObjectUtils.FillObjectFromDataReader<ExchangeOrganization>(DataProvider.GetExchangeOrganization(itemId));

                if (ExchangeOrg != null)
                {
                    foreach (SfBUserPlan p in plans)
                    {
                        p.IsDefault = (p.SfBUserPlanId == ExchangeOrg.SfBUserPlanID);
                    }
                }


                return plans;
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

        private static void GetSfBUserPlansByUser(int itemId, UserInfo user, ref List<SfBUserPlan> plans)
        {
            if ((user != null))
            {
                List<Organization> orgs = null;

                if (user.UserId != 1)
                {
                    List<PackageInfo> Packages = PackageController.GetPackages(user.UserId);

                    if ((Packages != null) & (Packages.Count > 0))
                    {
                        orgs = ExchangeServerController.GetExchangeOrganizationsInternal(Packages[0].PackageId, false);
                    }
                }
                else
                {
                    orgs = ExchangeServerController.GetExchangeOrganizationsInternal(1, false);
                }

                int OrgId = -1;
                if (itemId > 0) OrgId = itemId;
                else if ((orgs != null) & (orgs.Count > 0)) OrgId = orgs[0].Id;

                if (OrgId != -1)
                {
                    List<SfBUserPlan> Plans = ObjectUtils.CreateListFromDataReader<SfBUserPlan>(DataProvider.GetSfBUserPlans(OrgId));

                    foreach (SfBUserPlan p in Plans)
                    {
                        plans.Add(p);
                    }
                }

                UserInfo owner = UserController.GetUserInternally(user.OwnerId);

                GetSfBUserPlansByUser(0, owner, ref plans);
            }
        }


        public static SfBUserPlan GetSfBUserPlan(int itemID, int sfbUserPlanId)
        {

            // place log record
            TaskManager.StartTask("SFB", "GET_SFB_SFBUSERPLAN", sfbUserPlanId);

            try
            {
                return ObjectUtils.FillObjectFromDataReader<SfBUserPlan>(
                    DataProvider.GetSfBUserPlan(sfbUserPlanId));
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

        public static int AddSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("SFB", "ADD_SFB_SFBUSERPLAN", itemID);

            try
            {
                Organization org = GetOrganization(itemID);
                if (org == null)
                    return -1;

                // load package context
                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                sfbUserPlan.Conferencing = sfbUserPlan.Conferencing & Convert.ToBoolean(cntx.Quotas[Quotas.SFB_CONFERENCING].QuotaAllocatedValue);
                sfbUserPlan.EnterpriseVoice = sfbUserPlan.EnterpriseVoice & Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ENTERPRISEVOICE].QuotaAllocatedValue);
                if (!sfbUserPlan.EnterpriseVoice)
                    sfbUserPlan.VoicePolicy = SfBVoicePolicyType.None;
                sfbUserPlan.IM = true;

                return DataProvider.AddSfBUserPlan(itemID, sfbUserPlan);
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





        public static int UpdateSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("SFB", "ADD_SFB_SFBUSERPLAN", itemID);

            try
            {
                Organization org = GetOrganization(itemID);
                if (org == null)
                    return -1;

                // load package context
                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                sfbUserPlan.Conferencing = sfbUserPlan.Conferencing & Convert.ToBoolean(cntx.Quotas[Quotas.SFB_CONFERENCING].QuotaAllocatedValue);
                sfbUserPlan.EnterpriseVoice = sfbUserPlan.EnterpriseVoice & Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ENTERPRISEVOICE].QuotaAllocatedValue);
                if (!sfbUserPlan.EnterpriseVoice)
                    sfbUserPlan.VoicePolicy = SfBVoicePolicyType.None;
                sfbUserPlan.IM = true;

                DataProvider.UpdateSfBUserPlan(itemID, sfbUserPlan);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }


            return 0;
        }


        public static int DeleteSfBUserPlan(int itemID, int sfbUserPlanId)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("SFB", "DELETE_SFB_SFBPLAN", itemID);

            try
            {
                DataProvider.DeleteSfBUserPlan(sfbUserPlanId);

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

        public static int SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("SFB", "SET_SFB_SFBUSERPLAN", itemId);

            try
            {
                DataProvider.SetOrganizationDefaultSfBUserPlan(itemId, sfbUserPlanId);

            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return 1;

        }

        #endregion

        #region Federation Domains
        public static SfBFederationDomain[] GetFederationDomains(int itemId)
        {
            // place log record
            TaskManager.StartTask("SFB", "GET_SFB_FEDERATIONDOMAINS", itemId);

            SfBFederationDomain[] sfbFederationDomains = null;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                sfbFederationDomains = sfb.GetFederationDomains(org.OrganizationId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return sfbFederationDomains;
        }

        public static SfBUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            parameters.Add(new BackgroundTaskParameter("domainName", domainName));
            parameters.Add(new BackgroundTaskParameter("proxyFqdn", proxyFqdn));

            SfBUserResult res = TaskManager.StartResultTask<SfBUserResult>("SFB", "ADD_SFB_FEDERATIONDOMAIN", itemId, parameters);

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);

            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.NOT_AUTHORIZED);
                return res;
            }


            try
            {

                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                if (string.IsNullOrEmpty(org.SfBTenantId))
                {
                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    org.SfBTenantId = sfb.CreateOrganization(org.OrganizationId,
                                                                org.DefaultDomain,
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_CONFERENCING].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ALLOWVIDEO].QuotaAllocatedValue),
                                                                Convert.ToInt32(cntx.Quotas[Quotas.SFB_MAXPARTICIPANTS].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_FEDERATION].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.SFB_ENTERPRISEVOICE].QuotaAllocatedValue));

                    if (string.IsNullOrEmpty(org.SfBTenantId))
                    {
                        TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ENABLE_ORG);
                        return res;
                    }
                    else
                        PackageController.UpdatePackageItem(org);
                }

                sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                bool bDomainExists = false;
                SfBFederationDomain[] domains = GetFederationDomains(itemId);
                foreach (SfBFederationDomain d in domains)
                {
                    if (d.DomainName.ToLower() == domainName.ToLower())
                    {
                        bDomainExists = true;
                        break;
                    }

                }
                
                if (!bDomainExists)
                    sfb.AddFederationDomain(org.OrganizationId, domainName.ToLower(), proxyFqdn);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_ADD_SFB_FEDERATIONDOMAIN, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static SfBUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            SfBUserResult res = TaskManager.StartResultTask<SfBUserResult>("SFB", "REMOVE_SFB_FEDERATIONDOMAIN", itemId, new BackgroundTaskParameter("domainName", domainName));

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);

            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.NOT_AUTHORIZED);
                return res;
            }

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int sfbServiceId = GetSfBServiceID(org.PackageId);
                SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                if (org.OrganizationId.ToLower() == domainName.ToLower())
                {
                    TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_REMOVE_SFB_FEDERATIONDOMAIN);
                    return res;
                }

                sfb.RemoveFederationDomain(org.OrganizationId, domainName);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, SfBErrorCodes.CANNOT_REMOVE_SFB_FEDERATIONDOMAIN, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        #endregion

        public static string[] GetPolicyList(int itemId, SfBPolicyType type, string name)
        {
            string[] ret = null;
            try
            {
                if (itemId == -1)
                {
                    // policy list in all sfb servers
                    List<string> allpolicylist = new List<string>();
                    List<ServerInfo> servers = ServerController.GetAllServers();
                    foreach (ServerInfo server in servers)
                    {
                        List<ServiceInfo> services = ServerController.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.SfB);
                        foreach (ServiceInfo service in services)
                        {
                            SfBServer sfb = GetSfBServer(service.ServiceId, -1);
                            string[] values = sfb.GetPolicyList(type, name);
                            foreach (string val in values)
                                if (allpolicylist.IndexOf(val) == -1)
                                    allpolicylist.Add(val);
                        }

                    }
                    ret = allpolicylist.ToArray();
                }
                else
                {

                    Organization org = (Organization)PackageController.GetPackageItem(itemId);

                    int sfbServiceId = GetSfBServiceID(org.PackageId);
                    SfBServer sfb = GetSfBServer(sfbServiceId, org.ServiceId);

                    ret = sfb.GetPolicyList(type, name);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
            }

            return ret;
        }

        #region Private methods
        public static UInt64 ConvertPhoneNumberToLong(string ip)
        {
            return Convert.ToUInt64(ip);
        }

        public static string ConvertLongToPhoneNumber(UInt64 ip)
        {
            if (ip == 0)
                return "";

            return ip.ToString();
        }
        #endregion


    }
}
