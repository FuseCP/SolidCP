#if Client
using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using Microsoft.Web.Services3;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ICRM", Namespace = "http://smbsaas/solidcp/server/")]
    public interface ICRM
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateOrganizationResponse")]
        OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateOrganizationResponse")]
        System.Threading.Tasks.Task<OrganizationResult> CreateOrganizationAsync(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNames", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNamesResponse")]
        string[] GetSupportedCollationNames();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNames", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNamesResponse")]
        System.Threading.Tasks.Task<string[]> GetSupportedCollationNamesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCurrencyList", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCurrencyListResponse")]
        Currency[] GetCurrencyList();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCurrencyList", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCurrencyListResponse")]
        System.Threading.Tasks.Task<Currency[]> GetCurrencyListAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacks", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacksResponse")]
        int[] GetInstalledLanguagePacks();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacks", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacksResponse")]
        System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/DeleteOrganizationResponse")]
        ResultObject DeleteOrganization(Guid orgId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task<ResultObject> DeleteOrganizationAsync(Guid orgId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateCRMUser", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateCRMUserResponse")]
        UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateCRMUser", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateCRMUserResponse")]
        System.Threading.Tasks.Task<UserResult> CreateCRMUserAsync(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnits", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnitsResponse")]
        CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnits", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnitsResponse")]
        System.Threading.Tasks.Task<CRMBusinessUnitsResult> GetOrganizationBusinessUnitsAsync(Guid organizationId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRolesResponse")]
        CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRolesResponse")]
        System.Threading.Tasks.Task<CrmRolesResult> GetAllCrmRolesAsync(string orgName, Guid businessUnitId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRolesResponse")]
        CrmRolesResult GetCrmUserRoles(string orgName, Guid userId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRolesResponse")]
        System.Threading.Tasks.Task<CrmRolesResult> GetCrmUserRolesAsync(string orgName, Guid userId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserRolesResponse")]
        ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserRolesResponse")]
        System.Threading.Tasks.Task<ResultObject> SetUserRolesAsync(string orgName, Guid userId, Guid[] roles);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserCALType", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserCALTypeResponse")]
        ResultObject SetUserCALType(string orgName, Guid userId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserCALType", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserCALTypeResponse")]
        System.Threading.Tasks.Task<ResultObject> SetUserCALTypeAsync(string orgName, Guid userId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainName", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainNameResponse")]
        CrmUserResult GetCrmUserByDomainName(string domainName, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainName", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainNameResponse")]
        System.Threading.Tasks.Task<CrmUserResult> GetCrmUserByDomainNameAsync(string domainName, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserById", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByIdResponse")]
        CrmUserResult GetCrmUserById(Guid crmUserId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserById", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByIdResponse")]
        System.Threading.Tasks.Task<CrmUserResult> GetCrmUserByIdAsync(Guid crmUserId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/ChangeUserState", ReplyAction = "http://smbsaas/solidcp/server/ICRM/ChangeUserStateResponse")]
        ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/ChangeUserState", ReplyAction = "http://smbsaas/solidcp/server/ICRM/ChangeUserStateResponse")]
        System.Threading.Tasks.Task<ResultObject> ChangeUserStateAsync(bool disable, string orgName, Guid crmUserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetDBSizeResponse")]
        long GetDBSize(Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetDBSizeResponse")]
        System.Threading.Tasks.Task<long> GetDBSizeAsync(Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSizeResponse")]
        long GetMaxDBSize(Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSizeResponse")]
        System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSizeResponse")]
        ResultObject SetMaxDBSize(Guid organizationId, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSizeResponse")]
        System.Threading.Tasks.Task<ResultObject> SetMaxDBSizeAsync(Guid organizationId, long maxSize);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class CRMAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ICRM
    {
        public OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return (OrganizationResult)Invoke("SolidCP.Server.CRM", "CreateOrganization", organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public async System.Threading.Tasks.Task<OrganizationResult> CreateOrganizationAsync(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return await InvokeAsync<OrganizationResult>("SolidCP.Server.CRM", "CreateOrganization", organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public string[] GetSupportedCollationNames()
        {
            return (string[])Invoke("SolidCP.Server.CRM", "GetSupportedCollationNames");
        }

        public async System.Threading.Tasks.Task<string[]> GetSupportedCollationNamesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.CRM", "GetSupportedCollationNames");
        }

        public Currency[] GetCurrencyList()
        {
            return (Currency[])Invoke("SolidCP.Server.CRM", "GetCurrencyList");
        }

        public async System.Threading.Tasks.Task<Currency[]> GetCurrencyListAsync()
        {
            return await InvokeAsync<Currency[]>("SolidCP.Server.CRM", "GetCurrencyList");
        }

        public int[] GetInstalledLanguagePacks()
        {
            return (int[])Invoke("SolidCP.Server.CRM", "GetInstalledLanguagePacks");
        }

        public async System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync()
        {
            return await InvokeAsync<int[]>("SolidCP.Server.CRM", "GetInstalledLanguagePacks");
        }

        public ResultObject DeleteOrganization(Guid orgId)
        {
            return (ResultObject)Invoke("SolidCP.Server.CRM", "DeleteOrganization", orgId);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteOrganizationAsync(Guid orgId)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.CRM", "DeleteOrganization", orgId);
        }

        public UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return (UserResult)Invoke("SolidCP.Server.CRM", "CreateCRMUser", user, orgName, organizationId, baseUnitId, CALType);
        }

        public async System.Threading.Tasks.Task<UserResult> CreateCRMUserAsync(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return await InvokeAsync<UserResult>("SolidCP.Server.CRM", "CreateCRMUser", user, orgName, organizationId, baseUnitId, CALType);
        }

        public CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName)
        {
            return (CRMBusinessUnitsResult)Invoke("SolidCP.Server.CRM", "GetOrganizationBusinessUnits", organizationId, orgName);
        }

        public async System.Threading.Tasks.Task<CRMBusinessUnitsResult> GetOrganizationBusinessUnitsAsync(Guid organizationId, string orgName)
        {
            return await InvokeAsync<CRMBusinessUnitsResult>("SolidCP.Server.CRM", "GetOrganizationBusinessUnits", organizationId, orgName);
        }

        public CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId)
        {
            return (CrmRolesResult)Invoke("SolidCP.Server.CRM", "GetAllCrmRoles", orgName, businessUnitId);
        }

        public async System.Threading.Tasks.Task<CrmRolesResult> GetAllCrmRolesAsync(string orgName, Guid businessUnitId)
        {
            return await InvokeAsync<CrmRolesResult>("SolidCP.Server.CRM", "GetAllCrmRoles", orgName, businessUnitId);
        }

        public CrmRolesResult GetCrmUserRoles(string orgName, Guid userId)
        {
            return (CrmRolesResult)Invoke("SolidCP.Server.CRM", "GetCrmUserRoles", orgName, userId);
        }

        public async System.Threading.Tasks.Task<CrmRolesResult> GetCrmUserRolesAsync(string orgName, Guid userId)
        {
            return await InvokeAsync<CrmRolesResult>("SolidCP.Server.CRM", "GetCrmUserRoles", orgName, userId);
        }

        public ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles)
        {
            return (ResultObject)Invoke("SolidCP.Server.CRM", "SetUserRoles", orgName, userId, roles);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetUserRolesAsync(string orgName, Guid userId, Guid[] roles)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.CRM", "SetUserRoles", orgName, userId, roles);
        }

        public ResultObject SetUserCALType(string orgName, Guid userId, int CALType)
        {
            return (ResultObject)Invoke("SolidCP.Server.CRM", "SetUserCALType", orgName, userId, CALType);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetUserCALTypeAsync(string orgName, Guid userId, int CALType)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.CRM", "SetUserCALType", orgName, userId, CALType);
        }

        public CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return (CrmUserResult)Invoke("SolidCP.Server.CRM", "GetCrmUserByDomainName", domainName, orgName);
        }

        public async System.Threading.Tasks.Task<CrmUserResult> GetCrmUserByDomainNameAsync(string domainName, string orgName)
        {
            return await InvokeAsync<CrmUserResult>("SolidCP.Server.CRM", "GetCrmUserByDomainName", domainName, orgName);
        }

        public CrmUserResult GetCrmUserById(Guid crmUserId, string orgName)
        {
            return (CrmUserResult)Invoke("SolidCP.Server.CRM", "GetCrmUserById", crmUserId, orgName);
        }

        public async System.Threading.Tasks.Task<CrmUserResult> GetCrmUserByIdAsync(Guid crmUserId, string orgName)
        {
            return await InvokeAsync<CrmUserResult>("SolidCP.Server.CRM", "GetCrmUserById", crmUserId, orgName);
        }

        public ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId)
        {
            return (ResultObject)Invoke("SolidCP.Server.CRM", "ChangeUserState", disable, orgName, crmUserId);
        }

        public async System.Threading.Tasks.Task<ResultObject> ChangeUserStateAsync(bool disable, string orgName, Guid crmUserId)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.CRM", "ChangeUserState", disable, orgName, crmUserId);
        }

        public long GetDBSize(Guid organizationId)
        {
            return (long)Invoke("SolidCP.Server.CRM", "GetDBSize", organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetDBSizeAsync(Guid organizationId)
        {
            return await InvokeAsync<long>("SolidCP.Server.CRM", "GetDBSize", organizationId);
        }

        public long GetMaxDBSize(Guid organizationId)
        {
            return (long)Invoke("SolidCP.Server.CRM", "GetMaxDBSize", organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(Guid organizationId)
        {
            return await InvokeAsync<long>("SolidCP.Server.CRM", "GetMaxDBSize", organizationId);
        }

        public ResultObject SetMaxDBSize(Guid organizationId, long maxSize)
        {
            return (ResultObject)Invoke("SolidCP.Server.CRM", "SetMaxDBSize", organizationId, maxSize);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetMaxDBSizeAsync(Guid organizationId, long maxSize)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.CRM", "SetMaxDBSize", organizationId, maxSize);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class CRM : SolidCP.Web.Client.ClientBase<ICRM, CRMAssemblyClient>, ICRM
    {
        public OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return base.Client.CreateOrganization(organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public async System.Threading.Tasks.Task<OrganizationResult> CreateOrganizationAsync(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return await base.Client.CreateOrganizationAsync(organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public string[] GetSupportedCollationNames()
        {
            return base.Client.GetSupportedCollationNames();
        }

        public async System.Threading.Tasks.Task<string[]> GetSupportedCollationNamesAsync()
        {
            return await base.Client.GetSupportedCollationNamesAsync();
        }

        public Currency[] GetCurrencyList()
        {
            return base.Client.GetCurrencyList();
        }

        public async System.Threading.Tasks.Task<Currency[]> GetCurrencyListAsync()
        {
            return await base.Client.GetCurrencyListAsync();
        }

        public int[] GetInstalledLanguagePacks()
        {
            return base.Client.GetInstalledLanguagePacks();
        }

        public async System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync()
        {
            return await base.Client.GetInstalledLanguagePacksAsync();
        }

        public ResultObject DeleteOrganization(Guid orgId)
        {
            return base.Client.DeleteOrganization(orgId);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteOrganizationAsync(Guid orgId)
        {
            return await base.Client.DeleteOrganizationAsync(orgId);
        }

        public UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return base.Client.CreateCRMUser(user, orgName, organizationId, baseUnitId, CALType);
        }

        public async System.Threading.Tasks.Task<UserResult> CreateCRMUserAsync(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return await base.Client.CreateCRMUserAsync(user, orgName, organizationId, baseUnitId, CALType);
        }

        public CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName)
        {
            return base.Client.GetOrganizationBusinessUnits(organizationId, orgName);
        }

        public async System.Threading.Tasks.Task<CRMBusinessUnitsResult> GetOrganizationBusinessUnitsAsync(Guid organizationId, string orgName)
        {
            return await base.Client.GetOrganizationBusinessUnitsAsync(organizationId, orgName);
        }

        public CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId)
        {
            return base.Client.GetAllCrmRoles(orgName, businessUnitId);
        }

        public async System.Threading.Tasks.Task<CrmRolesResult> GetAllCrmRolesAsync(string orgName, Guid businessUnitId)
        {
            return await base.Client.GetAllCrmRolesAsync(orgName, businessUnitId);
        }

        public CrmRolesResult GetCrmUserRoles(string orgName, Guid userId)
        {
            return base.Client.GetCrmUserRoles(orgName, userId);
        }

        public async System.Threading.Tasks.Task<CrmRolesResult> GetCrmUserRolesAsync(string orgName, Guid userId)
        {
            return await base.Client.GetCrmUserRolesAsync(orgName, userId);
        }

        public ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles)
        {
            return base.Client.SetUserRoles(orgName, userId, roles);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetUserRolesAsync(string orgName, Guid userId, Guid[] roles)
        {
            return await base.Client.SetUserRolesAsync(orgName, userId, roles);
        }

        public ResultObject SetUserCALType(string orgName, Guid userId, int CALType)
        {
            return base.Client.SetUserCALType(orgName, userId, CALType);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetUserCALTypeAsync(string orgName, Guid userId, int CALType)
        {
            return await base.Client.SetUserCALTypeAsync(orgName, userId, CALType);
        }

        public CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return base.Client.GetCrmUserByDomainName(domainName, orgName);
        }

        public async System.Threading.Tasks.Task<CrmUserResult> GetCrmUserByDomainNameAsync(string domainName, string orgName)
        {
            return await base.Client.GetCrmUserByDomainNameAsync(domainName, orgName);
        }

        public CrmUserResult GetCrmUserById(Guid crmUserId, string orgName)
        {
            return base.Client.GetCrmUserById(crmUserId, orgName);
        }

        public async System.Threading.Tasks.Task<CrmUserResult> GetCrmUserByIdAsync(Guid crmUserId, string orgName)
        {
            return await base.Client.GetCrmUserByIdAsync(crmUserId, orgName);
        }

        public ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId)
        {
            return base.Client.ChangeUserState(disable, orgName, crmUserId);
        }

        public async System.Threading.Tasks.Task<ResultObject> ChangeUserStateAsync(bool disable, string orgName, Guid crmUserId)
        {
            return await base.Client.ChangeUserStateAsync(disable, orgName, crmUserId);
        }

        public long GetDBSize(Guid organizationId)
        {
            return base.Client.GetDBSize(organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetDBSizeAsync(Guid organizationId)
        {
            return await base.Client.GetDBSizeAsync(organizationId);
        }

        public long GetMaxDBSize(Guid organizationId)
        {
            return base.Client.GetMaxDBSize(organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(Guid organizationId)
        {
            return await base.Client.GetMaxDBSizeAsync(organizationId);
        }

        public ResultObject SetMaxDBSize(Guid organizationId, long maxSize)
        {
            return base.Client.SetMaxDBSize(organizationId, maxSize);
        }

        public async System.Threading.Tasks.Task<ResultObject> SetMaxDBSizeAsync(Guid organizationId, long maxSize)
        {
            return await base.Client.SetMaxDBSizeAsync(organizationId, maxSize);
        }
    }
}
#endif