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
using System.Data;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.EnterpriseServer.Code.HostedSolution
{
    public class BlackBerryController
    {
        private static bool CheckQuota(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

            IntResult userCount = GetBlackBerryUsersCount(itemId, string.Empty, string.Empty);

            int allocatedBlackBerryUsers = cntx.Quotas[Quotas.BLACKBERRY_USERS].QuotaAllocatedValuePerOrganization;
            
            return allocatedBlackBerryUsers == -1 || allocatedBlackBerryUsers > userCount.Value;                        
        }
        
        private static BlackBerry GetBlackBerryProxy(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.BlackBerry);

            BlackBerry blackBerry = new BlackBerry();
            ServiceProviderProxy.Init(blackBerry, serviceId);

            return blackBerry;
        }

        internal static bool CheckBlackBerryUserExists(int accountId)
        {
            return DataProvider.CheckBlackBerryUserExists(accountId);
        }
        
        public static ResultObject CreateBlackBerryUser(int itemId, int accountId)
        {
            ResultObject res = TaskManager.StartResultTask<ResultObject>("BLACBERRY", "CREATE_BLACKBERRY_USER");
            
            
            bool isBlackBerryUser;

            try
            {
                isBlackBerryUser = DataProvider.CheckBlackBerryUserExists(accountId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_CHECK_IF_BLACKBERRY_USER_EXISTS, ex);
                return res;
            }
            
            if (isBlackBerryUser)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.USER_IS_ALREADY_BLAKBERRY_USER);
                return res;
            }
            
            OrganizationUser user;
            try
            {
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user == null)
                    throw new ApplicationException(
                        string.Format("User is null. ItemId={0}, AccountId={1}", itemId,
                                      accountId));                
                
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            
            if (user.AccountType != ExchangeAccountType.Mailbox)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.ACCOUNT_TYPE_IS_NOT_MAILBOX);
                return res;
            }

            try
            {
                user = OrganizationController.GetUserGeneralSettings(itemId, accountId);
                if (string.IsNullOrEmpty(user.FirstName))
                {
                    TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.USER_FIRST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }
                if (string.IsNullOrEmpty(user.LastName))
                {
                    TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.USER_LAST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_USER_GENERAL_SETTINGS, ex);
                return res;
            }
            
            BlackBerry blackBerry;
            
            try
            {
                blackBerry = GetBlackBerryProxy(itemId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_BLACKBERRY_PROXY, ex);
                return res;
            }

            try
            {
                bool quota = CheckQuota(itemId);
                if (!quota)
                {
                    TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.USER_QUOTA_HAS_BEEN_REACHED);
                    return res;
                }
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_CHECK_QUOTA, ex);
                return res;
            }
            
            try
            {
               ResultObject userRes = blackBerry.CreateBlackBerryUser(user.PrimaryEmailAddress);
               res.ErrorCodes.AddRange(userRes.ErrorCodes);
               if (!userRes.IsSuccess)
               {
                   TaskManager.CompleteResultTask(res);
                   return res;
               }
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_ADD_BLACKBERRY_USER, ex);
                return res;
            }

            try
            {
                DataProvider.AddBlackBerryUser(accountId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_ADD_BLACKBERRY_USER_TO_DATABASE, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
            
        }

        public static OrganizationUsersPagedResult GetBlackBerryUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            OrganizationUsersPagedResult res = TaskManager.StartResultTask<OrganizationUsersPagedResult>("BLACKBERRY", "GET_BLACKBERRY_USERS");

            try
            {
                IDataReader reader =
                    DataProvider.GetBlackBerryUsers(itemId, sortColumn, sortDirection, name, email, startRow, count);
                List<OrganizationUser> accounts = new List<OrganizationUser>();
                ObjectUtils.FillCollectionFromDataReader(accounts, reader);
                res.Value = new OrganizationUsersPaged {PageUsers = accounts.ToArray()};
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, CrmErrorCodes.GET_CRM_USERS, ex);
                return res;
            }

            IntResult intRes = GetBlackBerryUsersCount(itemId, name, email);
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

        public static IntResult GetBlackBerryUsersCount(int itemId, string name, string email)
        {
            IntResult res = TaskManager.StartResultTask<IntResult>("BLACKBERRY", "GET_BLACKBERRY_USERS_COUNT");
            try
            {
                res.Value = DataProvider.GetBlackBerryUsersCount(itemId, name, email);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, CrmErrorCodes.GET_CRM_USER_COUNT, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject DeleteBlackBerryUser(int itemId, int accountId)
        {
            ResultObject res = TaskManager.StartResultTask<ResultObject>("BLACKBERRY", "DELETE_BLACKBERRY_USER");

            BlackBerry blackBerry;

            try
            {
                blackBerry = GetBlackBerryProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_BLACKBERRY_PROXY, ex);
                return res;
            }

            OrganizationUser user;
            try
            {
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user == null)
                    throw new ApplicationException(
                        string.Format("User is null. ItemId={0}, AccountId={1}", itemId,
                                      accountId));


            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            try
            {
                ResultObject bbRes = blackBerry.DeleteBlackBerryUser(user.PrimaryEmailAddress);
                res.ErrorCodes.AddRange(bbRes.ErrorCodes);
                if (!bbRes.IsSuccess)
                {
                    TaskManager.CompleteResultTask(res);
                    return res;
                }
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_DELETE_BLACKBERRY_USER, ex);
                return res;
            }

            try
            {
                DataProvider.DeleteBlackBerryUser(accountId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_DELETE_BLACKBERRY_USER_FROM_METADATA, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        public static BlackBerryUserStatsResult GetBlackBerryUserStats(int itemId, int accountId)
        {
            BlackBerryUserStatsResult res = TaskManager.StartResultTask<BlackBerryUserStatsResult>("BLACKBERRY",
                                                                                                   "DELETE_BLACKBERRY_USER");
            BlackBerry blackBerry;

            try
            {
                blackBerry = GetBlackBerryProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_BLACKBERRY_PROXY, ex);
                return res;
            }

            OrganizationUser user;
            try
            {
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user == null)
                    throw new ApplicationException(
                        string.Format("User is null. ItemId={0}, AccountId={1}", itemId,
                                      accountId));


            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            try
            {
                BlackBerryUserStatsResult tmp = blackBerry.GetBlackBerryUserStats(user.PrimaryEmailAddress);
                res.ErrorCodes.AddRange(tmp.ErrorCodes);
                if (!tmp.IsSuccess)
                {
                    TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_USER_STATS);
                    return res;
                }
                
                res.Value = tmp.Value;
                
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_USER_STATS, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        
        public static ResultObject SetActivationPasswordWithExpirationTime(int itemId, int accountId, string password, int time)
        {
            BlackBerryUserStatsResult res = TaskManager.StartResultTask<BlackBerryUserStatsResult>("BLACKBERRY",
                                                                                                   "DELETE_BLACKBERRY_USER");
            BlackBerry blackBerry;

            try
            {
                blackBerry = GetBlackBerryProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_BLACKBERRY_PROXY, ex);
                return res;
            }

            OrganizationUser user;
            try
            {
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user == null)
                    throw new ApplicationException(
                        string.Format("User is null. ItemId={0}, AccountId={1}", itemId,
                                      accountId));


            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            try
            {
                ResultObject tmp = blackBerry.SetActivationPasswordWithExpirationTime(user.PrimaryEmailAddress, password, time);
                res.ErrorCodes.AddRange(tmp.ErrorCodes);
                if (!tmp.IsSuccess)
                {
                    TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_SET_ACTIVATION_PASSWORD);
                    return res;
                }                

            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_SET_ACTIVATION_PASSWORD, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        public static ResultObject SetEmailActivationPassword(int itemId, int accountId)
        {
            ResultObject res = TaskManager.StartResultTask<BlackBerryUserStatsResult>("BLACKBERRY",
                                                                                                   "DELETE_BLACKBERRY_USER");
            BlackBerry blackBerry;

            try
            {
                blackBerry = GetBlackBerryProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_BLACKBERRY_PROXY, ex);
                return res;
            }

            OrganizationUser user;
            try
            {
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user == null)
                    throw new ApplicationException(
                        string.Format("User is null. ItemId={0}, AccountId={1}", itemId,
                                      accountId));


            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            try
            {
                ResultObject tmp = blackBerry.SetEmailActivationPassword(user.PrimaryEmailAddress);
                res.ErrorCodes.AddRange(tmp.ErrorCodes);
                if (!tmp.IsSuccess)
                {
                    TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_SET_EMAIL_ACTIVATION_PASSWORD);
                    return res;
                }                

            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_SET_EMAIL_ACTIVATION_PASSWORD, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }


        public static ResultObject DeleteDataFromBlackBerryDevice(int itemId, int accountId)
        {
            ResultObject res = TaskManager.StartResultTask<BlackBerryUserStatsResult>("BLACKBERRY",
                                                                                                   "DELETE_BLACKBERRY_USER");
            BlackBerry blackBerry;

            try
            {
                blackBerry = GetBlackBerryProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_GET_BLACKBERRY_PROXY, ex);
                return res;
            }

            OrganizationUser user;
            try
            {
                user = OrganizationController.GetAccount(itemId, accountId);

                if (user == null)
                    throw new ApplicationException(
                        string.Format("User is null. ItemId={0}, AccountId={1}", itemId,
                                      accountId));


            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            try
            {
                ResultObject tmp = blackBerry.DeleteDataFromBlackBerryDevice(user.PrimaryEmailAddress);
                res.ErrorCodes.AddRange(tmp.ErrorCodes);
                if (!tmp.IsSuccess)
                {
                    TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_DELETE_DATA_FROM_BLACKBERRY_DEVICE);
                    return res;
                }
                

            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, BlackBerryErrorsCodes.CANNOT_DELETE_DATA_FROM_BLACKBERRY_DEVICE, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

       
    }
}
