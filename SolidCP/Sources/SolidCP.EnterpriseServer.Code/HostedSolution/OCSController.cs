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
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.OCS;

namespace SolidCP.EnterpriseServer.Code.HostedSolution
{
    public class OCSController
    {

        private static OCSServer GetOCSProxy(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);

            OCSServer ocs = new OCSServer();
            ServiceProviderProxy.Init(ocs, serviceId);
            

            return ocs;
        }
        
        private static bool CheckQuota(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

            IntResult userCount = GetOCSUsersCount(itemId, string.Empty, string.Empty);

            int allocatedBlackBerryUsers = cntx.Quotas[Quotas.OCS_USERS].QuotaAllocatedValuePerOrganization;

            return allocatedBlackBerryUsers == -1 || allocatedBlackBerryUsers > userCount.Value;
        }

        private static void SetUserGeneralSettingsByDefault(int itemId, string instanceId, OCSServer ocs)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

            

            ocs.SetUserGeneralSettings(instanceId, !cntx.Quotas[Quotas.OCS_FederationByDefault].QuotaExhausted,
                !cntx.Quotas[Quotas.OCS_PublicIMConnectivityByDefault].QuotaExhausted, 
                !cntx.Quotas[Quotas.OCS_ArchiveIMConversationByDefault].QuotaExhausted,
                !cntx.Quotas[Quotas.OCS_ArchiveFederatedIMConversationByDefault].QuotaExhausted,
                !cntx.Quotas[Quotas.OCS_PresenceAllowedByDefault].QuotaExhausted);

    
        }


        private static OCSEdgeServer[] GetEdgeServers(string edgeServices)
        {
            List<OCSEdgeServer> list = new List<OCSEdgeServer>();
            if (!string.IsNullOrEmpty(edgeServices))
            {
                string[] services = edgeServices.Split(';');
                foreach (string current in services)
                {
                    string[] data = current.Split(',');
                    try
                    {
                        int serviceId = int.Parse(data[1]);
                        OCSEdgeServer ocs = new OCSEdgeServer();
                        ServiceProviderProxy.Init(ocs, serviceId);
                        list.Add(ocs);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex);
                    }
                }
            }

            return list.ToArray();
        }

        public static void DeleteDomain(int itemId, string domainName)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            if (org.IsOCSOrganization)
            {
                int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                string edgeServersData = settings[OCSConstants.EDGEServicesData];
                OCSEdgeServer[] edgeServers = GetEdgeServers(edgeServersData);
                DeleteDomain(domainName, edgeServers);
            }
        }

        public static void DeleteDomain(string domainName, OCSEdgeServer[] edgeServers)
        {
            foreach (OCSEdgeServer currentEdgeServer in edgeServers)
            {
                try
                {
                    currentEdgeServer.DeleteDomain(domainName);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
        }

        public static void AddDomain(string domainName, OCSEdgeServer[] edgeServers)
        {
            foreach (OCSEdgeServer currentEdgeServer in edgeServers)
            {
                try
                {
                    currentEdgeServer.AddDomain(domainName);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
        }

        public static void  AddDomain(string domainName, int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            if (!org.IsOCSOrganization)
            {
                int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                string edgeServersData = settings[OCSConstants.EDGEServicesData];
                OCSEdgeServer[] edgeServers = GetEdgeServers(edgeServersData);
                AddDomain(domainName, edgeServers);
            }
        }

        private static void CreateOCSDomains(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            if (!org.IsOCSOrganization)
            {
                List<OrganizationDomainName> domains = OrganizationController.GetOrganizationDomains(itemId);
                int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                string edgeServersData = settings[OCSConstants.EDGEServicesData];
                OCSEdgeServer[] edgeServers = GetEdgeServers(edgeServersData);
                foreach (OrganizationDomainName currentDomain in domains)
                {
                    AddDomain(currentDomain.DomainName, edgeServers);
                }

                org.IsOCSOrganization = true;

                PackageController.UpdatePackageItem(org);
            }
        }


        public static OCSUserResult CreateOCSUser(int itemId, int accountId)
        {
            OCSUserResult res = TaskManager.StartResultTask<OCSUserResult>("OCS", "CREATE_OCS_USER");

            OCSUser retOCSUser = new OCSUser();
            bool isOCSUser;

            try
            {
                isOCSUser = DataProvider.CheckOCSUserExists(accountId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_CHECK_IF_OCS_USER_EXISTS, ex);
                return res;
            }

            if (isOCSUser)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_IS_ALREADY_OCS_USER);
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
                user = OrganizationController.GetUserGeneralSettings(itemId, accountId);
                if (string.IsNullOrEmpty(user.FirstName))
                {
                    TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_FIRST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }
                
                if (string.IsNullOrEmpty(user.LastName))
                {
                    TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_LAST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_GET_USER_GENERAL_SETTINGS, ex);
                return res;
            }


            try
            {
                bool quota = CheckQuota(itemId);
                if (!quota)
                {
                    TaskManager.CompleteResultTask(res, OCSErrorCodes.USER_QUOTA_HAS_BEEN_REACHED);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_CHECK_QUOTA, ex);
                return res;
            }


           OCSServer ocs;

           try
            {
                ocs = GetOCSProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_GET_OCS_PROXY, ex);
                return res;
            }

            string instanceId;
            try
            {
                CreateOCSDomains(itemId);

                instanceId = ocs.CreateUser(user.PrimaryEmailAddress, user.DistinguishedName);
                retOCSUser.InstanceId = instanceId;
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_ADD_OCS_USER, ex);
                return res;
            }


            try
            {
                SetUserGeneralSettingsByDefault(itemId, instanceId, ocs);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_SET_DEFAULT_SETTINGS, ex);
                return res;
                
            }
            
            try
            {
                DataProvider.AddOCSUser(accountId, instanceId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_ADD_OCS_USER_TO_DATABASE, ex);
                return res;
            }

            res.Value = retOCSUser;
            TaskManager.CompleteResultTask();
            return res;

        }

        
        public static OCSUsersPagedResult GetOCSUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            OCSUsersPagedResult res = TaskManager.StartResultTask<OCSUsersPagedResult>("OCS", "GET_OCS_USERS");

            try
            {
                IDataReader reader =
                    DataProvider.GetOCSUsers(itemId, sortColumn, sortDirection, name, email, startRow, count);
                List<OCSUser> accounts = new List<OCSUser>();
                ObjectUtils.FillCollectionFromDataReader(accounts, reader);
                res.Value = new OCSUsersPaged { PageUsers = accounts.ToArray() };
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.GET_OCS_USERS, ex);
                return res;
            }

            IntResult intRes = GetOCSUsersCount(itemId, name, email);
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

        public static IntResult GetOCSUsersCount(int itemId, string name, string email)
        {
            IntResult res = TaskManager.StartResultTask<IntResult>("OCS", "GET_OCS_USERS_COUNT");
            try
            {
                res.Value = DataProvider.GetOCSUsersCount(itemId, name, email);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.GET_OCS_USER_COUNT, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject DeleteOCSUser(int itemId, string instanceId)
        {
            ResultObject res = TaskManager.StartResultTask<ResultObject>("OCS", "DELETE_OCS_USER");

            OCSServer ocsServer;

            try
            {
                ocsServer = GetOCSProxy(itemId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_GET_OCS_PROXY, ex);
                return res;
            }

            
            try
            {
                ocsServer.DeleteUser(instanceId);                                
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_DELETE_OCS_USER, ex);
                return res;
            }

            try
            {
                DataProvider.DeleteOCSUser(instanceId);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, OCSErrorCodes.CANNOT_DELETE_OCS_USER_FROM_METADATA, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static OCSUser GetUserGeneralSettings(int itemId, string instanceId)
        {
            TaskManager.StartTask("OCS", "GET_OCS_USER_GENERAL_SETTINGS");

            OCSUser user;

            try
            {
                OCSServer ocs = GetOCSProxy(itemId);
                user = ocs.GetUserGeneralSettings(instanceId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
                
            }
            TaskManager.CompleteTask();
            return user;

        }

        public static void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            TaskManager.StartTask("OCS", "SET_OCS_USER_GENERAL_SETTINGS");
            try
            {
                OCSServer ocs = GetOCSProxy(itemId);
                ocs.SetUserGeneralSettings(instanceId, enabledForFederation, enabledForPublicIMConnectivity,
                                           archiveInternalCommunications, archiveFederatedCommunications,
                                           enabledForEnhancedPresence);
            }
            catch(Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            TaskManager.CompleteTask();

        }
    }
}
