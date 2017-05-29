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
using SolidCP.Providers.Lync;

namespace SolidCP.EnterpriseServer.Code.HostedSolution
{
    public class LyncController
    {


        public static LyncServer GetLyncServer(int lyncServiceId, int organizationServiceId)
        {
            LyncServer ws = new LyncServer();

            ServiceProviderProxy.Init(ws, lyncServiceId);

            string[] lyncSettings = ws.ServiceProviderSettingsSoapHeaderValue.Settings;

            List<string> resSettings = new List<string>(lyncSettings);

            if (organizationServiceId != -1)
            {
                ExtendLyncSettings(resSettings, "primarydomaincontroller", GetProviderProperty(organizationServiceId, "primarydomaincontroller"));
                ExtendLyncSettings(resSettings, "rootou", GetProviderProperty(organizationServiceId, "rootou"));
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

        private static void ExtendLyncSettings(List<string> lyncSettings, string property, string value)
        {
            bool isAdded = false;
            for (int i = 0; i < lyncSettings.Count; i++)
            {
                string[] props = lyncSettings[i].Split('=');
                if (props[0].ToLower() == property)
                {
                    lyncSettings[i] = value;
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                lyncSettings.Add(value);
            }
        }

        private static int GetLyncServiceID(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.Lync);
        }


        private static bool CheckQuota(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

            IntResult userCount = GetLyncUsersCount(itemId);

            int allocatedUsers = cntx.Quotas[Quotas.LYNC_USERS].QuotaAllocatedValuePerOrganization;

            return allocatedUsers == -1 || allocatedUsers > userCount.Value;
        }


        public static LyncUserResult CreateLyncUser(int itemId, int accountId, int lyncUserPlanId)
        {
            LyncUserResult res = TaskManager.StartResultTask<LyncUserResult>("LYNC", "CREATE_LYNC_USER");

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.NOT_AUTHORIZED);
                return res;
            }


            LyncUser retLyncUser = new LyncUser();
            bool isLyncUser;

            isLyncUser = DataProvider.CheckLyncUserExists(accountId);
            if (isLyncUser)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.USER_IS_ALREADY_LYNC_USER);
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
                TaskManager.CompleteResultTask(res, LyncErrorCodes.USER_FIRST_NAME_IS_NOT_SPECIFIED);
                return res;
            }

            if (string.IsNullOrEmpty(user.LastName))
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.USER_LAST_NAME_IS_NOT_SPECIFIED);
                return res;
            }

            bool quota = CheckQuota(itemId);
            if (!quota)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.USER_QUOTA_HAS_BEEN_REACHED);
                return res;
            }


            LyncServer lync;

            try
            {

                bool bReloadConfiguration = false;

                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                lync = GetLyncServer(lyncServiceId, org.ServiceId);

                if (string.IsNullOrEmpty(org.LyncTenantId))
                {
                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    org.LyncTenantId = lync.CreateOrganization(org.OrganizationId,
                                                                org.DefaultDomain,
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_CONFERENCING].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ALLOWVIDEO].QuotaAllocatedValue),
                                                                Convert.ToInt32(cntx.Quotas[Quotas.LYNC_MAXPARTICIPANTS].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_FEDERATION].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ENTERPRISEVOICE].QuotaAllocatedValue));

                    if (string.IsNullOrEmpty(org.LyncTenantId))
                    {
                        TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ENABLE_ORG);
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
                                ServerController.AddServiceDNSRecords(org.PackageId, ResourceGroups.Lync, domain, "");
                            }
                        }
                        
                        PackageController.UpdatePackageItem(org);

                        bReloadConfiguration = true;
                    }
                }

                if (lync.GetOrganizationTenantId(org.OrganizationId) == string.Empty)
                {
                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    org.LyncTenantId = lync.CreateOrganization(org.OrganizationId,
                                                                org.DefaultDomain,
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_CONFERENCING].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ALLOWVIDEO].QuotaAllocatedValue),
                                                                Convert.ToInt32(cntx.Quotas[Quotas.LYNC_MAXPARTICIPANTS].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_FEDERATION].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ENTERPRISEVOICE].QuotaAllocatedValue));

                    if (string.IsNullOrEmpty(org.LyncTenantId))
                    {
                        TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ENABLE_ORG);
                        return res;
                    }
                    else
                    {
                        PackageController.UpdatePackageItem(org);

                        bReloadConfiguration = true;
                    }


                }


                LyncUserPlan plan = GetLyncUserPlan(itemId, lyncUserPlanId);

                if (!lync.CreateUser(org.OrganizationId, user.UserPrincipalName, plan))
                {
                    TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ADD_LYNC_USER);
                    return res;
                }

                if (bReloadConfiguration)
                {
                    LyncControllerAsync userWorker = new LyncControllerAsync();
                    userWorker.LyncServiceId = lyncServiceId;
                    userWorker.OrganizationServiceId = org.ServiceId;
                    userWorker.Enable_CsComputerAsync();
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ADD_LYNC_USER, ex);
                return res;
            }

            try
            {
                DataProvider.AddLyncUser(accountId, lyncUserPlanId, user.UserPrincipalName);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ADD_LYNC_USER_TO_DATABASE, ex);
                return res;
            }

            res.IsSuccess = true;
            TaskManager.CompleteResultTask();
            return res;

        }


        private static int[] ParseMultiSetting(int lyncServiceId, string settingName)
        {
            List<int> retIds = new List<int>();
            StringDictionary settings = ServerController.GetServiceSettings(lyncServiceId);
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
                retIds.Add(lyncServiceId);

            return retIds.ToArray();

        }


        public static void GetLyncServices(int lyncServiceId, out int[] lyncServiceIds)
        {
            lyncServiceIds = ParseMultiSetting(lyncServiceId, "LyncServersServiceID");
        }



        public static LyncUser GetLyncUserGeneralSettings(int itemId, int accountId)
        {
            TaskManager.StartTask("LYNC", "GET_LYNC_USER_GENERAL_SETTINGS");

            LyncUser user = null;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                OrganizationUser usr;
                usr = OrganizationController.GetAccount(itemId, accountId);

                if (usr != null)
                    user = lync.GetLyncUserGeneralSettings(org.OrganizationId, usr.UserPrincipalName);

                if (user != null)
                {
                    LyncUserPlan plan = ObjectUtils.FillObjectFromDataReader<LyncUserPlan>(DataProvider.GetLyncUserPlanByAccountId(accountId));

                    if (plan != null)
                    {
                        user.LyncUserPlanId = plan.LyncUserPlanId;
                        user.LyncUserPlanName = plan.LyncUserPlanName;
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

        public static LyncUserResult SetLyncUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            LyncUserResult res = TaskManager.StartResultTask<LyncUserResult>("LYNC", "SET_LYNC_USER_GENERAL_SETTINGS");

            string PIN = "";

            string[] uriAndPin = ("" + lineUri).Split(':');

            if (uriAndPin.Length > 0) lineUri = uriAndPin[0];
            if (uriAndPin.Length > 1) PIN = uriAndPin[1];

            LyncUser user = null;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                OrganizationUser usr;
                usr = OrganizationController.GetAccount(itemId, accountId);

                if (usr != null)
                    user = lync.GetLyncUserGeneralSettings(org.OrganizationId, usr.UserPrincipalName);

                if (user != null)
                {
                    LyncUserPlan plan = ObjectUtils.FillObjectFromDataReader<LyncUserPlan>(DataProvider.GetLyncUserPlanByAccountId(accountId));

                    if (plan != null)
                    {
                        user.LyncUserPlanId = plan.LyncUserPlanId;
                        user.LyncUserPlanName = plan.LyncUserPlanName;
                    }


                    if (!string.IsNullOrEmpty(sipAddress))
                    {
                        if (user.SipAddress != sipAddress)
                        {
                            if (sipAddress != usr.UserPrincipalName)
                            {
                                if (DataProvider.LyncUserExists(accountId, sipAddress))
                                {
                                    TaskManager.CompleteResultTask(res, LyncErrorCodes.ADDRESS_ALREADY_USED);
                                    return res;
                                }
                            }
                            user.SipAddress = sipAddress;
                        }
                    }

                    user.LineUri = lineUri;
                    user.PIN = PIN;

                    lync.SetLyncUserGeneralSettings(org.OrganizationId, usr.UserPrincipalName, user);

                    DataProvider.UpdateLyncUser(accountId, sipAddress);
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.FAILED_SET_SETTINGS, ex);
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
            TaskManager.StartTask("LYNC", "DELETE_ORG", itemId);

            try
            {
                // delete organization in Exchange
                //System.Threading.Thread.Sleep(5000);
                Organization org = (Organization)PackageController.GetPackageItem(itemId);

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                bool successful = lync.DeleteOrganization(org.OrganizationId, org.DefaultDomain);

                return successful ? 0 : BusinessErrorCodes.ERROR_LYNC_DELETE_SOME_PROBLEMS;
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


        public static LyncUserResult SetUserLyncPlan(int itemId, int accountId, int lyncUserPlanId)
        {
            LyncUserResult res = TaskManager.StartResultTask<LyncUserResult>("LYNC", "SET_LYNC_USER_LYNCPLAN");

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.NOT_AUTHORIZED);
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

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                LyncUserPlan plan = GetLyncUserPlan(itemId, lyncUserPlanId);

                OrganizationUser user;
                user = OrganizationController.GetAccount(itemId, accountId);

                if (!lync.SetLyncUserPlan(org.OrganizationId, user.UserPrincipalName, plan))
                {
                    TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ADD_LYNC_USER);
                    return res;
                }

                try
                {
                    DataProvider.SetLyncUserLyncUserplan(accountId, lyncUserPlanId);
                }
                catch (Exception ex)
                {
                    TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ADD_LYNC_USER_TO_DATABASE, ex);
                    return res;
                }

                res.IsSuccess = true;
                TaskManager.CompleteResultTask();
                return res;
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_UPDATE_LYNC_USER, ex);
                return res;
            }

        }

        public static LyncUsersPagedResult GetLyncUsers(int itemId)
        {
            return GetLyncUsersPaged(itemId, string.Empty, string.Empty, 0, int.MaxValue);
        }

        public static LyncUsersPagedResult GetLyncUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int count)
        {
            LyncUsersPagedResult res = TaskManager.StartResultTask<LyncUsersPagedResult>("LYNC", "GET_LYNC_USERS");

            try
            {
                IDataReader reader =
                    DataProvider.GetLyncUsers(itemId, sortColumn, sortDirection, startRow, count);
                List<LyncUser> accounts = new List<LyncUser>();
                ObjectUtils.FillCollectionFromDataReader(accounts, reader);
                res.Value = new LyncUsersPaged { PageUsers = accounts.ToArray() };
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.GET_LYNC_USERS, ex);
                return res;
            }

            IntResult intRes = GetLyncUsersCount(itemId);
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

        public static List<LyncUser> GetLyncUsersByPlanId(int itemId, int planId)
        {
            return ObjectUtils.CreateListFromDataReader<LyncUser>(DataProvider.GetLyncUsersByPlanId(itemId, planId));
        }

        public static IntResult GetLyncUsersCount(int itemId)
        {
            IntResult res = TaskManager.StartResultTask<IntResult>("LYNC", "GET_LYNC_USERS_COUNT");
            try
            {
                res.Value = DataProvider.GetLyncUsersCount(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.GET_LYNC_USER_COUNT, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static LyncUserResult DeleteLyncUser(int itemId, int accountId)
        {
            LyncUserResult res = TaskManager.StartResultTask<LyncUserResult>("LYNC", "DELETE_LYNC_USER");
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);

            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.NOT_AUTHORIZED);
                return res;
            }

            LyncServer lync;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                {
                    throw new ApplicationException(
                        string.Format("Organization is null. ItemId={0}", itemId));
                }

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                lync = GetLyncServer(lyncServiceId, org.ServiceId);

                OrganizationUser user;
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user != null)
                    lync.DeleteUser(user.UserPrincipalName);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_DELETE_LYNC_USER, ex);
                return res;
            }

            try
            {
                DataProvider.DeleteLyncUser(accountId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_DELETE_LYNC_USER_FROM_METADATA, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        public static Organization GetOrganization(int itemId)
        {
            return (Organization)PackageController.GetPackageItem(itemId);
        }


        #region Lync Plans
        public static List<LyncUserPlan> GetLyncUserPlans(int itemId)
        {
            // place log record
            TaskManager.StartTask("LYNC", "GET_LYNC_LYNCUSERPLANS", itemId);

            try
            {
                List<LyncUserPlan> plans = new List<LyncUserPlan>();

                UserInfo user = ObjectUtils.FillObjectFromDataReader<UserInfo>(DataProvider.GetUserByExchangeOrganizationIdInternally(itemId));
                
                if (user.Role == UserRole.User)
                    LyncController.GetLyncUserPlansByUser(itemId, user, ref plans);
                else
                    LyncController.GetLyncUserPlansByUser(0, user, ref plans);


                ExchangeOrganization ExchangeOrg = ObjectUtils.FillObjectFromDataReader<ExchangeOrganization>(DataProvider.GetExchangeOrganization(itemId));

                if (ExchangeOrg != null)
                {
                    foreach (LyncUserPlan p in plans)
                    {
                        p.IsDefault = (p.LyncUserPlanId == ExchangeOrg.LyncUserPlanID);
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

        private static void GetLyncUserPlansByUser(int itemId, UserInfo user, ref List<LyncUserPlan> plans)
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
                    List<LyncUserPlan> Plans = ObjectUtils.CreateListFromDataReader<LyncUserPlan>(DataProvider.GetLyncUserPlans(OrgId));

                    foreach (LyncUserPlan p in Plans)
                    {
                        plans.Add(p);
                    }
                }

                UserInfo owner = UserController.GetUserInternally(user.OwnerId);

                GetLyncUserPlansByUser(0, owner, ref plans);
            }
        }


        public static LyncUserPlan GetLyncUserPlan(int itemID, int lyncUserPlanId)
        {

            // place log record
            TaskManager.StartTask("LYNC", "GET_LYNC_LYNCUSERPLAN", lyncUserPlanId);

            try
            {
                return ObjectUtils.FillObjectFromDataReader<LyncUserPlan>(
                    DataProvider.GetLyncUserPlan(lyncUserPlanId));
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

        public static int AddLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("LYNC", "ADD_LYNC_LYNCUSERPLAN", itemID);

            try
            {
                Organization org = GetOrganization(itemID);
                if (org == null)
                    return -1;

                // load package context
                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                lyncUserPlan.Conferencing = lyncUserPlan.Conferencing & Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_CONFERENCING].QuotaAllocatedValue);
                lyncUserPlan.EnterpriseVoice = lyncUserPlan.EnterpriseVoice & Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ENTERPRISEVOICE].QuotaAllocatedValue);
                if (!lyncUserPlan.EnterpriseVoice)
                    lyncUserPlan.VoicePolicy = LyncVoicePolicyType.None;
                lyncUserPlan.IM = true;

                return DataProvider.AddLyncUserPlan(itemID, lyncUserPlan);
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





        public static int UpdateLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("LYNC", "ADD_LYNC_LYNCUSERPLAN", itemID);

            try
            {
                Organization org = GetOrganization(itemID);
                if (org == null)
                    return -1;

                // load package context
                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                lyncUserPlan.Conferencing = lyncUserPlan.Conferencing & Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_CONFERENCING].QuotaAllocatedValue);
                lyncUserPlan.EnterpriseVoice = lyncUserPlan.EnterpriseVoice & Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ENTERPRISEVOICE].QuotaAllocatedValue);
                if (!lyncUserPlan.EnterpriseVoice)
                    lyncUserPlan.VoicePolicy = LyncVoicePolicyType.None;
                lyncUserPlan.IM = true;

                DataProvider.UpdateLyncUserPlan(itemID, lyncUserPlan);
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


        public static int DeleteLyncUserPlan(int itemID, int lyncUserPlanId)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("LYNC", "DELETE_LYNC_LYNCPLAN", itemID);

            try
            {
                DataProvider.DeleteLyncUserPlan(lyncUserPlanId);

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

        public static int SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("LYNC", "SET_LYNC_LYNCUSERPLAN", itemId);

            try
            {
                DataProvider.SetOrganizationDefaultLyncUserPlan(itemId, lyncUserPlanId);

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
        public static LyncFederationDomain[] GetFederationDomains(int itemId)
        {
            // place log record
            TaskManager.StartTask("LYNC", "GET_LYNC_FEDERATIONDOMAINS", itemId);

            LyncFederationDomain[] lyncFederationDomains = null;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                lyncFederationDomains = lync.GetFederationDomains(org.OrganizationId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return lyncFederationDomains;
        }

        public static LyncUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            parameters.Add(new BackgroundTaskParameter("domainName", domainName));
            parameters.Add(new BackgroundTaskParameter("proxyFqdn", proxyFqdn));

            LyncUserResult res = TaskManager.StartResultTask<LyncUserResult>("LYNC", "ADD_LYNC_FEDERATIONDOMAIN", itemId, parameters);

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);

            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.NOT_AUTHORIZED);
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

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                if (string.IsNullOrEmpty(org.LyncTenantId))
                {
                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    org.LyncTenantId = lync.CreateOrganization(org.OrganizationId,
                                                                org.DefaultDomain,
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_CONFERENCING].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ALLOWVIDEO].QuotaAllocatedValue),
                                                                Convert.ToInt32(cntx.Quotas[Quotas.LYNC_MAXPARTICIPANTS].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_FEDERATION].QuotaAllocatedValue),
                                                                Convert.ToBoolean(cntx.Quotas[Quotas.LYNC_ENTERPRISEVOICE].QuotaAllocatedValue));

                    if (string.IsNullOrEmpty(org.LyncTenantId))
                    {
                        TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ENABLE_ORG);
                        return res;
                    }
                    else
                        PackageController.UpdatePackageItem(org);
                }

                lync = GetLyncServer(lyncServiceId, org.ServiceId);

                bool bDomainExists = false;
                LyncFederationDomain[] domains = GetFederationDomains(itemId);
                foreach (LyncFederationDomain d in domains)
                {
                    if (d.DomainName.ToLower() == domainName.ToLower())
                    {
                        bDomainExists = true;
                        break;
                    }

                }
                
                if (!bDomainExists)
                    lync.AddFederationDomain(org.OrganizationId, domainName.ToLower(), proxyFqdn);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_ADD_LYNC_FEDERATIONDOMAIN, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static LyncUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            LyncUserResult res = TaskManager.StartResultTask<LyncUserResult>("LYNC", "REMOVE_LYNC_FEDERATIONDOMAIN", itemId, new BackgroundTaskParameter("domainName", domainName));

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);

            if (accountCheck < 0)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.NOT_AUTHORIZED);
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

                int lyncServiceId = GetLyncServiceID(org.PackageId);
                LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                if (org.OrganizationId.ToLower() == domainName.ToLower())
                {
                    TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_REMOVE_LYNC_FEDERATIONDOMAIN);
                    return res;
                }

                lync.RemoveFederationDomain(org.OrganizationId, domainName);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, LyncErrorCodes.CANNOT_REMOVE_LYNC_FEDERATIONDOMAIN, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        #endregion

        public static string[] GetPolicyList(int itemId, LyncPolicyType type, string name)
        {
            string[] ret = null;
            try
            {
                if (itemId == -1)
                {
                    // policy list in all lync servers
                    List<string> allpolicylist = new List<string>();
                    List<ServerInfo> servers = ServerController.GetAllServers();
                    foreach (ServerInfo server in servers)
                    {
                        List<ServiceInfo> services = ServerController.GetServicesByServerIdGroupName(server.ServerId, ResourceGroups.Lync);
                        foreach (ServiceInfo service in services)
                        {
                            LyncServer lync = GetLyncServer(service.ServiceId, -1);
                            string[] values = lync.GetPolicyList(type, name);
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

                    int lyncServiceId = GetLyncServiceID(org.PackageId);
                    LyncServer lync = GetLyncServer(lyncServiceId, org.ServiceId);

                    ret = lync.GetPolicyList(type, name);
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
