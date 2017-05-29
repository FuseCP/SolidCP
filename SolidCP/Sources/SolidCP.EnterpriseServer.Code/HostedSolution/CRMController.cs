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
using System.Collections.Specialized;
using System.Data;
using SolidCP.Providers.Common;
using SolidCP.Providers.CRM;
using SolidCP.Providers.DNS;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using System.Text.RegularExpressions;

namespace SolidCP.EnterpriseServer
{
    public class CRMController
    {
        private static CRM GetCRMProxy(int packageId)
        {
            int crmServiceId = GetCRMServiceId(packageId);
            CRM ws = new CRM();
            ServiceProviderProxy.Init(ws, crmServiceId);
            return ws;
        }

        private static int GetCRMServiceId(int packageId)
        {
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedCRM2013);
            if (serviceId == 0)
                serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedCRM);
            return serviceId;
        }

        private static int GetCRMServiceId(int packageId, ResultObject res)
        {
            int serviceId = GetCRMServiceId(packageId);
            if (serviceId == 0)
                CompleteTask(res, CrmErrorCodes.CRM_IS_NOT_SELECTED_IN_HOSTING_PLAN, null,
                             "CRM is not selected in hosting plan.");
            return serviceId;
        }


		private static ResultObject CreateDnsZoneRecord(int domainId, string recordName, string ip)
		{
			ResultObject ret = StartTask<ResultObject>("CRM", "CREATE_DNS_ZONE");

			try
			{
				//check for existing empty A record
				DnsRecord[] records = ServerController.GetDnsZoneRecords(domainId);
				foreach (DnsRecord record in records)
				{
					if ((record.RecordType == DnsRecordType.A || record.RecordType == DnsRecordType.AAAA) && (String.Compare(recordName, record.RecordName, true) == 0))
					{
						CompleteTask(ret, CrmErrorCodes.CANNOT_CREATE_DNS_ZONE, null,
							string.Format("DNS record already exists. DomainId={0}, RecordName={1}", domainId, recordName));
						
						return ret;
					}
				}
				var type = ip.Contains(":") ? DnsRecordType.AAAA : DnsRecordType.A;
                int res = ServerController.AddDnsZoneRecord(domainId, recordName, type, ip, 0, 0, 0, 0);
				if (res != 0)
				{
					CompleteTask(ret, CrmErrorCodes.CANNOT_CREATE_DNS_ZONE, null,
						string.Format("Cannot create dns record. DomainId={0}, IP={1}, ErrorCode={2}, RecordName={3}", domainId, ip, res, recordName));

					return ret;
				}
			}
			catch (Exception ex)
			{
				CompleteTask(ret, CrmErrorCodes.CANNOT_CREATE_DNS_ZONE, ex);
				return ret;
			}

			CompleteTask();
			return ret;
		}
        
        private static ValueResultObject<DomainInfo> CreateOrganizationDomain(Organization org)
        {            
            ValueResultObject<DomainInfo> ret = StartTask<ValueResultObject<DomainInfo>>("CRM", "CREATE_ORGANIZATIO_DOMAIN");
            try
            {
                int dnsServiceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.Dns);
                if (dnsServiceId <= 0)
                {
                    CompleteTask(ret, CrmErrorCodes.DNS_SERVER_IS_NOT_SELECTED_IN_HOSTIN_PLAN, null,
                        string.Format("DNS Server is not selected in hosting plan."));
                    
                    return ret;
                }

                int crmServiceId = GetCRMServiceId(org.PackageId);
                StringDictionary serviceSettings = ServerController.GetServiceSettings(crmServiceId);

                string strDomainName = string.Format("{0}.{1}", org.OrganizationId,
                                  serviceSettings[Constants.IFDWebApplicationRootDomain]);

                DomainInfo domain = ServerController.GetDomain(strDomainName);

                if (domain == null)
                {
                    domain = new DomainInfo();
                    domain.PackageId = org.PackageId;
                    domain.DomainName = strDomainName;

                    domain.IsInstantAlias = false;
                    domain.IsSubDomain = false;
                    domain.DomainId = ServerController.AddDomain(domain);

                }

                ret.Value = domain;

                if (domain.DomainId <= 0)
                {                                        
                    CompleteTask(ret, CrmErrorCodes.CANNOT_CREATE_CRM_ORGANIZATION_DOMAIN, null,
                        string.Format("Crm organization domain cannot be created. DomainId {0}", domain.DomainId));                    
                    return ret;
                }

                ResultObject resAddDnsZoneRecord = CreateDnsZoneRecord(domain.DomainId, string.Empty, serviceSettings[Constants.CRMWebsiteIP]);
                ret.ErrorCodes.AddRange(resAddDnsZoneRecord.ErrorCodes);

            }
            catch (Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.CREATE_CRM_ORGANIZATION_DOMAIN_GENERAL_ERROR, ex);
                return ret;
            }
            
            CompleteTask();
            return ret;
        }

        private static void CompleteTask(ResultObject res, string errorCode, Exception ex, string errorMessage)
        {
            if (res != null)
            {
                res.IsSuccess = false;

                if (!string.IsNullOrEmpty(errorCode))
                    res.ErrorCodes.Add(errorCode);
            }
           
            if (ex != null)
                TaskManager.WriteError(ex);

            if (!string.IsNullOrEmpty(errorMessage))
                TaskManager.WriteError(errorMessage);

            //LogRecord.
            TaskManager.CompleteTask();


        }        

        private static void CompleteTask(ResultObject res, string errorCode, Exception ex)
        {
            CompleteTask(res, errorCode, ex, null);
        }

        private static void CompleteTask(ResultObject res, string errorCode)
        {
            CompleteTask(res, errorCode, null, null);
        }

        private static void CompleteTask(ResultObject res)
        {
            CompleteTask(res, null);
        }

        private static void CompleteTask()
        {
            CompleteTask(null);
        }

        private static T StartTask<T>(string source, string taskName) where T : ResultObject, new()
        {
            TaskManager.StartTask(source, taskName);
            T res = new T();
            res.IsSuccess = true;
            return res;
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
                if (props.Length == 2)
                {
                    if (props[0].ToLower() == property)
                    {
                        value = props[1];
                        break;
                    }
                }
            }

            return value;
        }

        public static string GetOrganizationCRMUniqueName(string orgName)
        {
            return Regex.Replace(orgName, @"[^\dA-Za-z]", "-", RegexOptions.Compiled);
        }


        public static OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName,  int userId, string collation, int baseLanguageCode)
        {
            OrganizationResult res = StartTask<OrganizationResult>("CRM", "CREATE_ORGANIZATION");

            try
            {
                Organization org = OrganizationController.GetOrganization(organizationId);

                try
                {
                    if (!CheckQuota(org.PackageId))
                    {
                        CompleteTask(res, CrmErrorCodes.QUOTA_HAS_BEEN_REACHED);
                        return res;
                    }
                }
                catch (Exception ex)
                {
                    CompleteTask(res, CrmErrorCodes.CANNOT_CHECK_QUOTAS, ex);
                    return res;
                }

                CRM crm = GetCRMProxy(org.PackageId);
                Guid orgId = Guid.NewGuid();

                OrganizationUser user = OrganizationController.GetUserGeneralSettings(organizationId, userId);                

                if (string.IsNullOrEmpty(user.FirstName))
                {
                    CompleteTask(res, CrmErrorCodes.FIRST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }

                if (string.IsNullOrEmpty(user.LastName))
                {
                    CompleteTask(res, CrmErrorCodes.LAST_NAME_IS_NOT_SPECIFIED);
                    return res;
                }

                int serviceid = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.HostedOrganizations);
                string rootOU = GetProviderProperty(serviceid, "rootou");

                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                string maxDBSizeQuotaName = "";
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013)) maxDBSizeQuotaName = Quotas.CRM2013_MAXDATABASESIZE;
                else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM)) maxDBSizeQuotaName = Quotas.CRM_MAXDATABASESIZE;

                long maxDBSize = cntx.Quotas[maxDBSizeQuotaName].QuotaAllocatedValue;
                if (maxDBSize != -1) maxDBSize = maxDBSize * 1024 * 1024;

                org.CrmAdministratorId = user.AccountId;
                org.CrmCurrency =
                    string.Join("|", new[] {baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName});
                org.CrmCollation = collation;
                org.CrmOrgState = (int) CrmOrganizationState.Enabled;
                org.CrmLanguadgeCode = baseLanguageCode.ToString();
                org.CrmOrganizationId = orgId;

                OrganizationResult serverRes =
                    crm.CreateOrganization(orgId, org.OrganizationId, org.Name,
                                           baseLanguageCode,
                                           org.OrganizationId + "." + rootOU,
                                           baseCurrencyCode, baseCurrencyName,
                                           baseCurrencySymbol, user.SamAccountName, user.FirstName, user.LastName, user.PrimaryEmailAddress,
                                           collation, maxDBSize);

                if (!serverRes.IsSuccess)
                {
                    res.ErrorCodes.AddRange(serverRes.ErrorCodes);
                    CompleteTask(res);
                    return res;
                }

                ValueResultObject<DomainInfo> resDomain = CreateOrganizationDomain(org);
                res.ErrorCodes.AddRange(resDomain.ErrorCodes);


                int crmServiceId = GetCRMServiceId(org.PackageId);

                StringDictionary serviceSettings = ServerController.GetServiceSettings(crmServiceId);
                string port = serviceSettings[Constants.Port];
                string schema = serviceSettings[Constants.UrlSchema];
                if (port == "80" && schema == "http")
                    port = string.Empty;

                if (port == "443" && schema == "https")
                    port = string.Empty;

                if (port != string.Empty)
                    port = ":" + port;

                string strDomainName = string.Format("{0}.{1}", GetOrganizationCRMUniqueName(org.OrganizationId),
                                                     serviceSettings[Constants.IFDWebApplicationRootDomain]);
                org.CrmUrl = string.Format("{0}://{1}{2}", schema, strDomainName, port);

                PackageController.UpdatePackageItem(org);

                CrmUserResult crmUser = crm.GetCrmUserByDomainName(user.DomainUserName, org.OrganizationId);
                res.ErrorCodes.AddRange(crmUser.ErrorCodes);
                if (crmUser.IsSuccess)
                {
                    try
                    {
                        DataProvider.CreateCRMUser(userId, crmUser.Value.CRMUserId, crmUser.Value.BusinessUnitId, 0);
                    }
                    catch (Exception ex)
                    {
                        CompleteTask(res, CrmErrorCodes.CANNOT_ADD_ORGANIZATION_OWNER_TO_ORGANIZATIO_USER, ex, "Cannot add organization owner to organization users");                        
                    }
                }

                res.Value = org;
            }
            catch (Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CREATE_CRM_ORGANIZATION_GENERAL_ERROR, ex);
                return res;
            }
           
            CompleteTask();
            return res;
        }

        public static StringArrayResultObject GetCollationByServiceId(int serviceId)
        {
            StringArrayResultObject ret = StartTask<StringArrayResultObject>("CRM", "GET_COLLATION_NAMES");
            ret.IsSuccess = true;
            try
            {
                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);
                ret.Value = crm.GetSupportedCollationNames();
            }
            catch (Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.CANNOT_GET_COLLATION_NAMES, ex);
                return ret;
            }
            CompleteTask();
            return ret;
        }

        public static StringArrayResultObject GetCollation(int packageId)
        {                        
            StringArrayResultObject ret = StartTask<StringArrayResultObject>("CRM", "GET_COLLATION_NAMES");
            ret.IsSuccess = true;

            try
            {
                int serviceId =  GetCRMServiceId(packageId, ret);
                if (serviceId == 0)
                    return ret;
            
                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);
                ret.Value = crm.GetSupportedCollationNames();
            
            }
            catch(Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.CANNOT_GET_COLLATION_NAMES, ex);
                return ret;
            }
            
            CompleteTask();
            return ret;
        }

        public static CurrencyArrayResultObject GetCurrencyByServiceId(int serviceId)
        {
            CurrencyArrayResultObject ret = StartTask<CurrencyArrayResultObject>("CRM", "GET_CURRENCY");
            ret.IsSuccess = true;
            try
            {
                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);
                ret.Value = crm.GetCurrencyList();
            }
            catch (Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.CANNOT_GET_CURRENCY_LIST, ex);
                return ret;
            }
            CompleteTask();

            return ret;
        }

        public static CurrencyArrayResultObject GetCurrency(int packageId)
        {            
            CurrencyArrayResultObject ret = StartTask<CurrencyArrayResultObject>("CRM", "GET_CURRENCY");
            ret.IsSuccess = true;
            try
            {
                int serviceId = GetCRMServiceId(packageId, ret);
                if (serviceId == 0)
                    return ret;
            
                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);
                ret.Value = crm.GetCurrencyList();
                            
            }
            catch(Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.CANNOT_GET_CURRENCY_LIST, ex);
                return ret;
            }
            CompleteTask();
           
            return ret;
        }
        
      
        private static ResultObject DeleteDnsRecord(int domainId, string recordName, string ip)
        {
            ResultObject ret = StartTask<ResultObject>("CRM", "DELETE_DNS_RECORD");
            
            try
            {
				var type = ip.Contains(":") ? DnsRecordType.AAAA : DnsRecordType.A;
				int res = ServerController.DeleteDnsZoneRecord(domainId, recordName, type, ip);

                if (res != 0)
                {
                    
                    CompleteTask(ret, CrmErrorCodes.DELETE_DNS_RECORD_ERROR, null,
                        string.Format("Cannot delete dns record. DomainId={0}, DnsRecord={1}, ErrorCode={2}", domainId, DnsRecordType.A, res));
                    
                    return ret;
                }
            }
            catch (Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.CANNOT_DELETE_DNS_RECORD, ex);
                return ret;                
            }
            
            CompleteTask();
            return ret;
 
        }

        private static ResultObject DeleteOrganizationDomain(Organization org)
        {
            ResultObject ret = StartTask<ResultObject>("CRM", "DELETE_ORGANIZATION_DOMAIN");            

            try
            {
                int dnsServiceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.Dns);
                int crmServiceId = GetCRMServiceId(org.PackageId);
                if (dnsServiceId <= 0)
                {
                    TaskManager.WriteError("DNS Server is not selected in hosting plan.");
                    CompleteTask(ret, CrmErrorCodes.DNS_SERVER_IS_NOT_SELECTED_IN_HOSTIN_PLAN);
                    return ret;
                }

                StringDictionary serviceSettings = ServerController.GetServiceSettings(crmServiceId);

                string domainName =
                    string.Format("{0}.{1}", org.OrganizationId, serviceSettings[Constants.IFDWebApplicationRootDomain]);

                DomainInfo domainInfo = ServerController.GetDomain(domainName);

                if (domainInfo == null)
                {                    
                    CompleteTask(ret, CrmErrorCodes.CANNOT_GET_CRM_ORGANIZATION_DOMAIN, null, 
                        string.Format("Cannot get crm organization domain {0}", domainName));
                    
                    return ret;
                }

                ResultObject resultDeleteDnsRecord = DeleteDnsRecord(domainInfo.DomainId, string.Empty, serviceSettings[Constants.CRMWebsiteIP]);                
                ret.ErrorCodes.AddRange(resultDeleteDnsRecord.ErrorCodes);                
                if (!resultDeleteDnsRecord.IsSuccess)
                {                                        
                    CompleteTask(ret);
                    return ret;
                }

                /*try
                {
                    ServerController.DeleteDomain(domainInfo.DomainId);
                }
                catch (Exception ex)
                {
                    CompleteTask(ret, CrmErrorCodes.CANNOT_DELETE_CRM_ORGANIZATIO_DOMAIN, ex);
                    return ret;
                }*/
            }
            catch(Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.DELETE_CRM_ORGANIZATION_DOMAIN_GENERAL_ERROR, ex);
                return ret;
            }
            
            CompleteTask();
            return ret;
        }

        public static ResultObject DeleteOrganization(int organizationId)
        {
            ResultObject res = StartTask<ResultObject>("CRM", "DELETE_ORGANIZATION");
            
            try
            {
                Organization org = OrganizationController.GetOrganization(organizationId);
                CRM crm = GetCRMProxy(org.PackageId);
                
                try
                {
                    DataProvider.DeleteCrmOrganization(organizationId);
                }
                catch(Exception ex)
                {
                    CompleteTask(res, CrmErrorCodes.CANNOT_DELETE_CRM_ORGANIZATION_METADATA, ex);
                    return res;
                }
                
                ResultObject resDeleteDomain = DeleteOrganizationDomain(org);                             
                res.ErrorCodes.AddRange(resDeleteDomain.ErrorCodes);
                                
                
                ResultObject resDeleteOrganization = crm.DeleteOrganization(org.CrmOrganizationId);
                res.ErrorCodes.AddRange(resDeleteOrganization.ErrorCodes);                
                if (!resDeleteOrganization.IsSuccess)
                {                    
                    CompleteTask(res);
                    return res;
                }
                                    
                org.CrmAdministratorId = 0;
                org.CrmCollation = string.Empty;
                org.CrmCurrency = string.Empty;
                org.CrmOrganizationId = Guid.Empty;
                org.CrmOrgState = (int) CrmOrganizationState.Disabled;
                org.CrmUrl = string.Empty;
                
                PackageController.UpdatePackageItem(org);
            }
            catch(Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.DELETE_CRM_ORGANIZATION_GENERAL_ERROR, ex);
                return res;
            }
            
            CompleteTask();
            return res;
        }

        private static bool CheckQuota(int packageId)
        {
            TaskManager.StartTask("CRM", "CHECK_QUOTA");
            bool res = false;
            try
            {
                // check account
                int errorCode = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (errorCode < 0) return false;


                PackageContext cntx = PackageController.GetPackageContext(packageId);

                string quotaName = "";
                if      (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013)) quotaName = Quotas.CRM2013_ORGANIZATION;
                else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM)) quotaName = Quotas.CRM_ORGANIZATION;

                return (cntx.Quotas[quotaName] != null && !cntx.Quotas[quotaName].QuotaExhausted);
                
            }
            catch(Exception ex)
            {
                TaskManager.WriteError(ex);
                res = false;

            }
            finally
            {
                TaskManager.CompleteTask();
            }
            return res;                        
        }


        public static IntResult GetCRMUsersCount(int itemId, string name, string email, int CALType)
        {
            IntResult res = StartTask<IntResult>("CRM", "GET_CRM_USERS_COUNT");
            try
            {
                res.Value = DataProvider.GetCRMUsersCount(itemId, name, email, CALType);
            }
            catch(Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.GET_CRM_USER_COUNT, ex);
                return res;
            }

            CompleteTask();
            return res;
        }


        public static List<OrganizationUser> GetCRMOrganizationUsers(int itemId)
        {
            IDataReader reader =
                DataProvider.GetCRMOrganizationUsers(itemId);
            List<OrganizationUser> accounts = new List<OrganizationUser>();
            ObjectUtils.FillCollectionFromDataReader(accounts, reader);


            return accounts;
        }


        public static OrganizationUsersPagedResult GetCRMUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            OrganizationUsersPagedResult res = StartTask<OrganizationUsersPagedResult>("CRM", "GET_CRM_USERS");
            
            try
            {
                IDataReader reader =
                    DataProvider.GetCrmUsers(itemId, sortColumn, sortDirection, name, email, startRow, count);
                List<OrganizationUser> accounts = new List<OrganizationUser>();
                ObjectUtils.FillCollectionFromDataReader(accounts, reader);
                res.Value = new OrganizationUsersPaged();
                res.Value.PageUsers = accounts.ToArray();                                
                
            }
            catch(Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.GET_CRM_USERS, ex);
                return res;
            }

            IntResult intRes = GetCRMUsersCount(itemId, name, email, -1);
            res.ErrorCodes.AddRange(intRes.ErrorCodes);
            if (!intRes.IsSuccess)
            {
                CompleteTask(res);
                return res;
            }
            res.Value.RecordsCount = intRes.Value;
            
            CompleteTask();
            return res;
        }


        public static UserResult CreateCRMUser(OrganizationUser user, int packageId, int itemId, Guid businessUnitId, int CALType)
        {
            UserResult ret = StartTask<UserResult>("CRM", "CREATE_CRM_USER");

            try
            {
                if (user == null)
                    throw new ArgumentNullException("user");

                
                if (businessUnitId == Guid.Empty)
                    throw new ArgumentNullException("businessUnitId");

                if (string.IsNullOrEmpty(user.FirstName))
                {
                    CompleteTask(ret, CrmErrorCodes.FIRST_NAME_IS_NOT_SPECIFIED, null, "First name is not specified.");
                    return ret;
                }

                if (string.IsNullOrEmpty(user.LastName))
                {
                    CompleteTask(ret, CrmErrorCodes.LAST_NAME_IS_NOT_SPECIFIED, null, "Last name is not specified.");
                    return ret;
                }

                Guid crmUserId = GetCrmUserId(user.AccountId);                                

                if (crmUserId != Guid.Empty)
                {
                    CompleteTask(ret, CrmErrorCodes.CRM_USER_ALREADY_EXISTS, null, "CRM user already exists.");
                    return ret;
                }

                BoolResult quotaRes = CheckQuota(packageId, itemId, CALType);
                ret.ErrorCodes.AddRange(quotaRes.ErrorCodes);
                if (!quotaRes.IsSuccess)
                {
                    CompleteTask(ret);
                    return ret;
                }

                if (!quotaRes.Value)
                {
                    string errorCode = CrmErrorCodes.USER_QUOTA_HAS_BEEN_REACHED;
                    PackageContext cntx = PackageController.GetPackageContext(packageId);
                    if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                        errorCode += "2013_";
                    errorCode += CALType.ToString();

                    CompleteTask(ret, errorCode , null, "CRM user quota has been reached.");
                    return ret;
                }
                
                Guid crmId;
                try
                {
                    int serviceId = GetCRMServiceId(packageId, ret);
                    if (serviceId == 0)
                        return ret;

                    Organization org = OrganizationController.GetOrganization(itemId);
                    
                    CRM crm = new CRM();
                    ServiceProviderProxy.Init(crm, serviceId);

                    UserResult res = crm.CreateCRMUser(user, org.OrganizationId, org.CrmOrganizationId, businessUnitId, CALType);
                    ret.ErrorCodes.AddRange(res.ErrorCodes);

                    if (!res.IsSuccess)
                    {
                        CompleteTask(res);
                        return ret;
                    }
                    crmId = res.Value.CrmUserId;
                }
                catch (Exception ex)
                {
                    CompleteTask(ret, CrmErrorCodes.CANNOT_CREATE_CRM_USER_GENERAL_ERROR, ex);
                    return ret;
                }

                try
                {
                    DataProvider.CreateCRMUser(user.AccountId, crmId, businessUnitId, CALType);
                }
                catch (Exception ex)
                {
                    CompleteTask(ret, CrmErrorCodes.CANNOT_CREATE_CRM_USER_IN_DATABASE, ex);
                    return ret;
                }
            }
            catch(Exception ex)
            {
                CompleteTask(ret, CrmErrorCodes.CREATE_CRM_USER_GENERAL_ERROR, ex);
                return ret;
            }
            CompleteTask();
            return ret;
        }

        public static CRMBusinessUnitsResult GetCRMBusinessUnits(int itemId, int packageId)
        {
            CRMBusinessUnitsResult res = StartTask<CRMBusinessUnitsResult>("CRM", "GET_CRM_BUSINESS_UNITS");
            try
            {
                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return res;

                Organization org;
                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                    if (org.CrmOrganizationId == Guid.Empty)
                        throw new ApplicationException("Current organization is not CRM organization.");

                }
                catch(Exception ex)
                {
                    CompleteTask(res, CrmErrorCodes.CANNOT_GET_CRM_ORGANIZATION, ex);
                    return res;
                }
                
                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);
                CRMBusinessUnitsResult providerRes = crm.GetOrganizationBusinessUnits(org.CrmOrganizationId, org.OrganizationId);
                res.ErrorCodes.AddRange(providerRes.ErrorCodes);
                if (!providerRes.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }
                res.Value = providerRes.Value;
                
            }
            catch(Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_GET_BUSINESS_UNITS_GENERAL_ERROR, ex);
                return res;
            }
            
            CompleteTask();
            return res;
        }

        public static CrmRolesResult GetCRMRoles(int itemId, int accountId, int packageId)
        {
            CrmRolesResult res = StartTask<CrmRolesResult>("CRM", "GET_CRM_ROLES");

            try
            {
                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return res;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                Organization org = OrganizationController.GetOrganization(itemId);

                CrmUserResult crmUserResult = GetCrmUser(itemId, accountId);
                res.ErrorCodes.AddRange(crmUserResult.ErrorCodes);
                if (!crmUserResult.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }                
                
                CrmUser crmUser = crmUserResult.Value;
                CrmRolesResult crmRoles = crm.GetAllCrmRoles(org.OrganizationId, crmUser.BusinessUnitId);
                res.ErrorCodes.AddRange(crmRoles.ErrorCodes);
                if (!crmRoles.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }
                Guid userId = crmUser.CRMUserId;
                CrmRolesResult crmUserRoles = crm.GetCrmUserRoles(org.OrganizationId, userId);

                res.ErrorCodes.AddRange(crmUserRoles.ErrorCodes);
                if (!crmUserRoles.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }

                foreach (CrmRole role in crmUserRoles.Value)
                {
                    CrmRole retRole = crmRoles.Value.Find(delegate(CrmRole currentRole)
                                                              {
                                                                  return currentRole.RoleId == role.RoleId;
                                                              });
                    if (retRole != null)
                        retRole.IsCurrentUserRole = true;
                }

                res.Value = crmRoles.Value;
            }
            catch(Exception ex)
            {
                  CompleteTask(res, CrmErrorCodes.CANNOT_GET_CRM_ROLES_GENERAL_ERROR, ex);    
                  return res;
            }
            
            CompleteTask();
            return res;
        }

        
        public static Guid GetCrmUserId(int accountId)
        {
            IDataReader reader = DataProvider.GetCrmUser(accountId);
            CrmUser user = ObjectUtils.FillObjectFromDataReader<CrmUser>(reader);
            return user == null ? Guid.Empty :user.CRMUserId;
                  
        }

        public static CrmUserResult GetCrmUser(int itemId, int accountId)
        {
            CrmUserResult res = StartTask<CrmUserResult>("CRM", "GET_CRM_USER");
            
            try
            {
                Guid crmUserId = GetCrmUserId(accountId);

                Organization org;

                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                }
                catch (Exception ex)
                {
                    CompleteTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                    return res;
                }


                int serviceId = GetCRMServiceId(org.PackageId, res);
                if (serviceId == 0)
                    return res;

                try
                {
                    CRM crm = new CRM();
                    ServiceProviderProxy.Init(crm, serviceId);
                    CrmUserResult user = crm.GetCrmUserById(crmUserId, org.OrganizationId);
                    res.ErrorCodes.AddRange(user.ErrorCodes);
                    if (!user.IsSuccess)
                    {
                        CompleteTask(res);
                        return res;
                    }
                    res.Value = user.Value;
                }
                catch (Exception ex)
                {
                    CompleteTask(res, CrmErrorCodes.CANONT_GET_CRM_USER_BY_ID, ex);
                    return res;
                }
            }
            catch(Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANONT_GET_CRM_USER_GENERAL_ERROR, ex);
                return res;
            }
            CompleteTask();
            return res;
        }


        
        public static ResultObject SetUserRoles(int itemId, int accountId, int packageId, Guid[] roles)
        {
            CrmRolesResult res = StartTask<CrmRolesResult>("CRM", "GET_CRM_ROLES");

            try
            {

                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return res;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                Organization org;

                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                    if (org == null)
                        throw new ApplicationException("Org is null.");
                }
                catch (Exception ex)
                {
                    CompleteTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                    return res;
                }

                Guid crmUserId = GetCrmUserId(accountId);

                ResultObject rolesRes = crm.SetUserRoles(org.OrganizationId, crmUserId, roles);
                res.ErrorCodes.AddRange(rolesRes.ErrorCodes);
                if (!rolesRes.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }
            }
            catch(Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_SET_CRM_USER_ROLES_GENERAL_ERROR, ex);
                return res;
            }
            CompleteTask();
            return res;

        }

        public static ResultObject SetUserCALType(int itemId, int accountId, int packageId, int CALType)
        {
            ResultObject res = StartTask<CrmRolesResult>("CRM", "SET_CRM_CALTYPE");

            try
            {
                CrmUserResult user = GetCrmUser(itemId, accountId);
                if (user.Value.CALType + ((int)user.Value.ClientAccessMode)*10 == CALType)
                {
                    res.IsSuccess = true;
                    CompleteTask();
                    return res;
                }

                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return res;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                Organization org;

                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                    if (org == null)
                        throw new ApplicationException("Org is null.");
                }
                catch (Exception ex)
                {
                    CompleteTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                    return res;
                }

                BoolResult quotaRes = CheckQuota(packageId, itemId, CALType);
                res.ErrorCodes.AddRange(quotaRes.ErrorCodes);
                if (!quotaRes.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }
                if (!quotaRes.Value)
                {
                    string errorCode = CrmErrorCodes.USER_QUOTA_HAS_BEEN_REACHED;
                    PackageContext cntx = PackageController.GetPackageContext(packageId);
                    if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                        errorCode += "2013_";
                    errorCode += CALType.ToString();

                    CompleteTask(res, errorCode, null, "CRM user quota has been reached.");
                    return res;
                }

                Guid crmUserId = GetCrmUserId(accountId);

                ResultObject rolesRes = crm.SetUserCALType(org.Name, crmUserId, CALType);

                DataProvider.UpdateCRMUser(accountId, CALType);

                res.ErrorCodes.AddRange(rolesRes.ErrorCodes);
                if (!rolesRes.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_SET_CRM_USER_ROLES_GENERAL_ERROR, ex);
                return res;
            }
            CompleteTask();
            return res;

        }
        
        public static ResultObject ChangeUserState(int itemId, int accountId, bool disable)
        {
            CrmRolesResult res = StartTask<CrmRolesResult>("CRM", "CHANGER_USER_STATE");

            Organization org;
            try
            {
                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                    if (org == null)
                        throw new ApplicationException("Org is null.");
                }
                catch (Exception ex)
                {
                    CompleteTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                    return res;
                }

                int serviceId = GetCRMServiceId(org.PackageId, res);
                if (serviceId == 0)
                    return res;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                Guid crmUserId = GetCrmUserId(accountId);
                
                ResultObject changeStateRes = crm.ChangeUserState(disable, org.OrganizationId, crmUserId);
                
                if (!changeStateRes.IsSuccess)
                    res.IsSuccess = false;

                res.ErrorCodes.AddRange(changeStateRes.ErrorCodes);                
                
            }
            catch(Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_CHANGE_USER_STATE_GENERAL_ERROR, ex);
                return res;
            }
            
            CompleteTask();
            return res;
        }

        
        private static BoolResult CheckQuota(int packageId, int itemId, int CALType)
        {
            BoolResult res = StartTask<BoolResult>("CRM", "CHECK_QUOTA");
            try
            {
                PackageContext cntx = PackageController.GetPackageContext(packageId);

                IntResult tmp = GetCRMUsersCount(itemId, string.Empty, string.Empty, CALType);
                res.ErrorCodes.AddRange(tmp.ErrorCodes);
                if (!tmp.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }

                string quotaName = "";

                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                {
                    switch (CALType)
                    {
                        case CRMUserLycenseTypes.PROFESSIONAL:
                            quotaName = Quotas.CRM2013_PROFESSIONALUSERS;
                            break;
                        case CRMUserLycenseTypes.BASIC:
                            quotaName = Quotas.CRM2013_BASICUSERS;
                            break;
                        case CRMUserLycenseTypes.ESSENTIAL:
                            quotaName = Quotas.CRM2013_ESSENTIALUSERS;
                            break;
                    }
                }
                else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM))
                {
                    switch (CALType)
                    {
                        case CRMUserLycenseTypes.FULL:
                            quotaName = Quotas.CRM_USERS;
                            break;
                        case CRMUserLycenseTypes.LIMITED:
                            quotaName = Quotas.CRM_LIMITEDUSERS;
                            break;
                        case CRMUserLycenseTypes.ESS:
                            quotaName = Quotas.CRM_ESSUSERS;
                            break;
                    }
                }

                int allocatedCrmUsers = cntx.Quotas[quotaName].QuotaAllocatedValuePerOrganization;
                res.Value = allocatedCrmUsers == -1 || allocatedCrmUsers > tmp.Value;
                
            }
            catch(Exception ex)
            {
                 CompleteTask(res, CrmErrorCodes.CHECK_QUOTA, ex);
                 return res;               
            }
            
            CompleteTask();
            return res;
        }


        public static ResultObject SetMaxDBSize(int itemId, int packageId, long maxSize)
        {
            ResultObject res = StartTask<ResultObject>("CRM", "SET_CRM_MAXDBSIZE");

            try
            {

                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return res;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                Organization org;

                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                    if (org == null)
                        throw new ApplicationException("Org is null.");
                }
                catch (Exception ex)
                {
                    CompleteTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                    return res;
                }

                PackageContext cntx = PackageController.GetPackageContext(packageId);

                string quotaName = "";
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013)) quotaName = Quotas.CRM2013_MAXDATABASESIZE;
                else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM)) quotaName = Quotas.CRM_MAXDATABASESIZE;

                long limitSize = cntx.Quotas[quotaName].QuotaAllocatedValue;

                if (limitSize != -1)
                {
                    limitSize = limitSize * 1024 * 1024;
                    if (maxSize == -1) maxSize = limitSize;
                    if (maxSize > limitSize) maxSize = limitSize;
                }

                res = crm.SetMaxDBSize(org.CrmOrganizationId, maxSize);

                res.ErrorCodes.AddRange(res.ErrorCodes);
                if (!res.IsSuccess)
                {
                    CompleteTask(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_CONFIGURE_CRM_ORGANIZATION, ex);
                return res;
            }
            CompleteTask();
            return res;

        }

        public static long GetDBSize(int itemId, int packageId)
        {
            ResultObject res = StartTask<ResultObject>("CRM", "GET_CRM_DBSIZE");
            long size = -1;

            try
            {

                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return -1;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                Organization org;

                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                    if (org == null)
                        throw new ApplicationException("Org is null.");
                }
                catch (Exception ex)
                {
                    CompleteTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                    return -1;
                }

                size = crm.GetDBSize(org.CrmOrganizationId);
                
            }
            catch (Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_CONFIGURE_CRM_ORGANIZATION, ex);
                return -1;
            }
            CompleteTask();
            return size;
        }

        public static long GetMaxDBSize(int itemId, int packageId)
        {
            ResultObject res = StartTask<ResultObject>("CRM", "GET_CRM_MAXDBSIZE");
            long size = -1;

            try
            {

                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return -1;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                Organization org;

                try
                {
                    org = OrganizationController.GetOrganization(itemId);
                    if (org == null)
                        throw new ApplicationException("Org is null.");
                }
                catch (Exception ex)
                {
                    CompleteTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                    return -1;
                }

                size = crm.GetMaxDBSize(org.CrmOrganizationId);

            }
            catch (Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_CONFIGURE_CRM_ORGANIZATION, ex);
                return -1;
            }
            CompleteTask();
            return size;
        }


        public static int[] GetInstalledLanguagePacks(int packageId)
        {
            ResultObject res = StartTask<ResultObject>("CRM", "GET_CRM_MAXDBSIZE");
            int[] ret = null;

            try
            {
                int serviceId = GetCRMServiceId(packageId, res);
                if (serviceId == 0)
                    return null;

                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                ret = crm.GetInstalledLanguagePacks();
            }
            catch (Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_CONFIGURE_CRM_ORGANIZATION, ex);
                return null;
            }
            CompleteTask();
            return ret;
        }

        public static int[] GetInstalledLanguagePacksByServiceId(int serviceId)
        {
            ResultObject res = StartTask<ResultObject>("CRM", "GET_CRM_MAXDBSIZE");
            int[] ret = null;

            try
            {
                CRM crm = new CRM();
                ServiceProviderProxy.Init(crm, serviceId);

                ret = crm.GetInstalledLanguagePacks();
            }
            catch (Exception ex)
            {
                CompleteTask(res, CrmErrorCodes.CANNOT_CONFIGURE_CRM_ORGANIZATION, ex);
                return null;
            }
            CompleteTask();
            return ret;
        }

    }
}
