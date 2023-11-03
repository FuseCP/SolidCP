#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesCRM", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesCRM
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateOrganizationResponse")]
        SolidCP.Providers.Common.OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.OrganizationResult> CreateOrganizationAsync(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollation", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollationResponse")]
        SolidCP.Providers.ResultObjects.StringArrayResultObject GetCollation(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollation", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringArrayResultObject> GetCollationAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollationByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollationByServiceIdResponse")]
        SolidCP.Providers.ResultObjects.StringArrayResultObject GetCollationByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollationByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCollationByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringArrayResultObject> GetCollationByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrency", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrencyResponse")]
        SolidCP.Providers.ResultObjects.CurrencyArrayResultObject GetCurrency(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrency", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrencyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject> GetCurrencyAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrencyByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrencyByServiceIdResponse")]
        SolidCP.Providers.ResultObjects.CurrencyArrayResultObject GetCurrencyByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrencyByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCurrencyByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject> GetCurrencyByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/DeleteCRMOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/DeleteCRMOrganizationResponse")]
        SolidCP.Providers.Common.ResultObject DeleteCRMOrganization(int organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/DeleteCRMOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/DeleteCRMOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCRMOrganizationAsync(int organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUsersPagedResponse")]
        SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult GetCRMUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUsersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult> GetCRMUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUserCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUserCountResponse")]
        SolidCP.Providers.ResultObjects.IntResult GetCRMUserCount(int itemId, string name, string email, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUserCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCRMUserCountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetCRMUserCountAsync(int itemId, string name, string email, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateCRMUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateCRMUserResponse")]
        SolidCP.Providers.ResultObjects.UserResult CreateCRMUser(SolidCP.Providers.HostedSolution.OrganizationUser user, int packageId, int itemId, System.Guid businessUnitOrgId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateCRMUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/CreateCRMUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.UserResult> CreateCRMUserAsync(SolidCP.Providers.HostedSolution.OrganizationUser user, int packageId, int itemId, System.Guid businessUnitOrgId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetBusinessUnits", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetBusinessUnitsResponse")]
        SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult GetBusinessUnits(int itemId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetBusinessUnits", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetBusinessUnitsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult> GetBusinessUnitsAsync(int itemId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmRoles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmRolesResponse")]
        SolidCP.Providers.ResultObjects.CrmRolesResult GetCrmRoles(int itemId, int accountId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmRoles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmRolesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetCrmRolesAsync(int itemId, int accountId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserRoles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserRolesResponse")]
        SolidCP.Providers.Common.ResultObject SetUserRoles(int itemId, int accountId, int packageId, System.Guid[] roles);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserRoles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserRolesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserRolesAsync(int itemId, int accountId, int packageId, System.Guid[] roles);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserCALType", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserCALTypeResponse")]
        SolidCP.Providers.Common.ResultObject SetUserCALType(int itemId, int accountId, int packageId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserCALType", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetUserCALTypeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserCALTypeAsync(int itemId, int accountId, int packageId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/ChangeUserState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/ChangeUserStateResponse")]
        SolidCP.Providers.Common.ResultObject ChangeUserState(int itemId, int accountId, bool disable);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/ChangeUserState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/ChangeUserStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeUserStateAsync(int itemId, int accountId, bool disable);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmUserResponse")]
        SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUser(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetCrmUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetMaxDBSizeResponse")]
        SolidCP.Providers.Common.ResultObject SetMaxDBSize(int itemId, int packageId, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/SetMaxDBSizeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMaxDBSizeAsync(int itemId, int packageId, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetDBSize", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetDBSizeResponse")]
        long GetDBSize(int itemId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetDBSize", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetDBSizeResponse")]
        System.Threading.Tasks.Task<long> GetDBSizeAsync(int itemId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetMaxDBSizeResponse")]
        long GetMaxDBSize(int itemId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetMaxDBSizeResponse")]
        System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(int itemId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacksResponse")]
        int[] GetInstalledLanguagePacks(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacksResponse")]
        System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacksByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacksByServiceIdResponse")]
        int[] GetInstalledLanguagePacksByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacksByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesCRM/GetInstalledLanguagePacksByServiceIdResponse")]
        System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksByServiceIdAsync(int serviceId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esCRMAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesCRM
    {
        public SolidCP.Providers.Common.OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode)
        {
            return Invoke<SolidCP.Providers.Common.OrganizationResult>("SolidCP.EnterpriseServer.esCRM", "CreateOrganization", organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName, userId, collation, baseLanguageCode);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.OrganizationResult> CreateOrganizationAsync(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode)
        {
            return await InvokeAsync<SolidCP.Providers.Common.OrganizationResult>("SolidCP.EnterpriseServer.esCRM", "CreateOrganization", organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName, userId, collation, baseLanguageCode);
        }

        public SolidCP.Providers.ResultObjects.StringArrayResultObject GetCollation(int packageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCollation", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringArrayResultObject> GetCollationAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCollation", packageId);
        }

        public SolidCP.Providers.ResultObjects.StringArrayResultObject GetCollationByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCollationByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringArrayResultObject> GetCollationByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCollationByServiceId", serviceId);
        }

        public SolidCP.Providers.ResultObjects.CurrencyArrayResultObject GetCurrency(int packageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCurrency", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject> GetCurrencyAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCurrency", packageId);
        }

        public SolidCP.Providers.ResultObjects.CurrencyArrayResultObject GetCurrencyByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCurrencyByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject> GetCurrencyByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject>("SolidCP.EnterpriseServer.esCRM", "GetCurrencyByServiceId", serviceId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteCRMOrganization(int organizationId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "DeleteCRMOrganization", organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCRMOrganizationAsync(int organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "DeleteCRMOrganization", organizationId);
        }

        public SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult GetCRMUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult>("SolidCP.EnterpriseServer.esCRM", "GetCRMUsersPaged", itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult> GetCRMUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult>("SolidCP.EnterpriseServer.esCRM", "GetCRMUsersPaged", itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetCRMUserCount(int itemId, string name, string email, int CALType)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esCRM", "GetCRMUserCount", itemId, name, email, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetCRMUserCountAsync(int itemId, string name, string email, int CALType)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esCRM", "GetCRMUserCount", itemId, name, email, CALType);
        }

        public SolidCP.Providers.ResultObjects.UserResult CreateCRMUser(SolidCP.Providers.HostedSolution.OrganizationUser user, int packageId, int itemId, System.Guid businessUnitOrgId, int CALType)
        {
            return Invoke<SolidCP.Providers.ResultObjects.UserResult>("SolidCP.EnterpriseServer.esCRM", "CreateCRMUser", user, packageId, itemId, businessUnitOrgId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.UserResult> CreateCRMUserAsync(SolidCP.Providers.HostedSolution.OrganizationUser user, int packageId, int itemId, System.Guid businessUnitOrgId, int CALType)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.UserResult>("SolidCP.EnterpriseServer.esCRM", "CreateCRMUser", user, packageId, itemId, businessUnitOrgId, CALType);
        }

        public SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult GetBusinessUnits(int itemId, int packageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult>("SolidCP.EnterpriseServer.esCRM", "GetBusinessUnits", itemId, packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult> GetBusinessUnitsAsync(int itemId, int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult>("SolidCP.EnterpriseServer.esCRM", "GetBusinessUnits", itemId, packageId);
        }

        public SolidCP.Providers.ResultObjects.CrmRolesResult GetCrmRoles(int itemId, int accountId, int packageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CrmRolesResult>("SolidCP.EnterpriseServer.esCRM", "GetCrmRoles", itemId, accountId, packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetCrmRolesAsync(int itemId, int accountId, int packageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CrmRolesResult>("SolidCP.EnterpriseServer.esCRM", "GetCrmRoles", itemId, accountId, packageId);
        }

        public SolidCP.Providers.Common.ResultObject SetUserRoles(int itemId, int accountId, int packageId, System.Guid[] roles)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "SetUserRoles", itemId, accountId, packageId, roles);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserRolesAsync(int itemId, int accountId, int packageId, System.Guid[] roles)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "SetUserRoles", itemId, accountId, packageId, roles);
        }

        public SolidCP.Providers.Common.ResultObject SetUserCALType(int itemId, int accountId, int packageId, int CALType)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "SetUserCALType", itemId, accountId, packageId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserCALTypeAsync(int itemId, int accountId, int packageId, int CALType)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "SetUserCALType", itemId, accountId, packageId, CALType);
        }

        public SolidCP.Providers.Common.ResultObject ChangeUserState(int itemId, int accountId, bool disable)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "ChangeUserState", itemId, accountId, disable);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeUserStateAsync(int itemId, int accountId, bool disable)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "ChangeUserState", itemId, accountId, disable);
        }

        public SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUser(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CrmUserResult>("SolidCP.EnterpriseServer.esCRM", "GetCrmUser", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CrmUserResult>("SolidCP.EnterpriseServer.esCRM", "GetCrmUser", itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SetMaxDBSize(int itemId, int packageId, long maxSize)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "SetMaxDBSize", itemId, packageId, maxSize);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMaxDBSizeAsync(int itemId, int packageId, long maxSize)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esCRM", "SetMaxDBSize", itemId, packageId, maxSize);
        }

        public long GetDBSize(int itemId, int packageId)
        {
            return Invoke<long>("SolidCP.EnterpriseServer.esCRM", "GetDBSize", itemId, packageId);
        }

        public async System.Threading.Tasks.Task<long> GetDBSizeAsync(int itemId, int packageId)
        {
            return await InvokeAsync<long>("SolidCP.EnterpriseServer.esCRM", "GetDBSize", itemId, packageId);
        }

        public long GetMaxDBSize(int itemId, int packageId)
        {
            return Invoke<long>("SolidCP.EnterpriseServer.esCRM", "GetMaxDBSize", itemId, packageId);
        }

        public async System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(int itemId, int packageId)
        {
            return await InvokeAsync<long>("SolidCP.EnterpriseServer.esCRM", "GetMaxDBSize", itemId, packageId);
        }

        public int[] GetInstalledLanguagePacks(int packageId)
        {
            return Invoke<int[]>("SolidCP.EnterpriseServer.esCRM", "GetInstalledLanguagePacks", packageId);
        }

        public async System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync(int packageId)
        {
            return await InvokeAsync<int[]>("SolidCP.EnterpriseServer.esCRM", "GetInstalledLanguagePacks", packageId);
        }

        public int[] GetInstalledLanguagePacksByServiceId(int serviceId)
        {
            return Invoke<int[]>("SolidCP.EnterpriseServer.esCRM", "GetInstalledLanguagePacksByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<int[]>("SolidCP.EnterpriseServer.esCRM", "GetInstalledLanguagePacksByServiceId", serviceId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esCRM : SolidCP.Web.Client.ClientBase<IesCRM, esCRMAssemblyClient>, IesCRM
    {
        public SolidCP.Providers.Common.OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode)
        {
            return base.Client.CreateOrganization(organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName, userId, collation, baseLanguageCode);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.OrganizationResult> CreateOrganizationAsync(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode)
        {
            return await base.Client.CreateOrganizationAsync(organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName, userId, collation, baseLanguageCode);
        }

        public SolidCP.Providers.ResultObjects.StringArrayResultObject GetCollation(int packageId)
        {
            return base.Client.GetCollation(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringArrayResultObject> GetCollationAsync(int packageId)
        {
            return await base.Client.GetCollationAsync(packageId);
        }

        public SolidCP.Providers.ResultObjects.StringArrayResultObject GetCollationByServiceId(int serviceId)
        {
            return base.Client.GetCollationByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringArrayResultObject> GetCollationByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetCollationByServiceIdAsync(serviceId);
        }

        public SolidCP.Providers.ResultObjects.CurrencyArrayResultObject GetCurrency(int packageId)
        {
            return base.Client.GetCurrency(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject> GetCurrencyAsync(int packageId)
        {
            return await base.Client.GetCurrencyAsync(packageId);
        }

        public SolidCP.Providers.ResultObjects.CurrencyArrayResultObject GetCurrencyByServiceId(int serviceId)
        {
            return base.Client.GetCurrencyByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CurrencyArrayResultObject> GetCurrencyByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetCurrencyByServiceIdAsync(serviceId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteCRMOrganization(int organizationId)
        {
            return base.Client.DeleteCRMOrganization(organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCRMOrganizationAsync(int organizationId)
        {
            return await base.Client.DeleteCRMOrganizationAsync(organizationId);
        }

        public SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult GetCRMUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return base.Client.GetCRMUsersPaged(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.OrganizationUsersPagedResult> GetCRMUsersPagedAsync(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows)
        {
            return await base.Client.GetCRMUsersPagedAsync(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetCRMUserCount(int itemId, string name, string email, int CALType)
        {
            return base.Client.GetCRMUserCount(itemId, name, email, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetCRMUserCountAsync(int itemId, string name, string email, int CALType)
        {
            return await base.Client.GetCRMUserCountAsync(itemId, name, email, CALType);
        }

        public SolidCP.Providers.ResultObjects.UserResult CreateCRMUser(SolidCP.Providers.HostedSolution.OrganizationUser user, int packageId, int itemId, System.Guid businessUnitOrgId, int CALType)
        {
            return base.Client.CreateCRMUser(user, packageId, itemId, businessUnitOrgId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.UserResult> CreateCRMUserAsync(SolidCP.Providers.HostedSolution.OrganizationUser user, int packageId, int itemId, System.Guid businessUnitOrgId, int CALType)
        {
            return await base.Client.CreateCRMUserAsync(user, packageId, itemId, businessUnitOrgId, CALType);
        }

        public SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult GetBusinessUnits(int itemId, int packageId)
        {
            return base.Client.GetBusinessUnits(itemId, packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult> GetBusinessUnitsAsync(int itemId, int packageId)
        {
            return await base.Client.GetBusinessUnitsAsync(itemId, packageId);
        }

        public SolidCP.Providers.ResultObjects.CrmRolesResult GetCrmRoles(int itemId, int accountId, int packageId)
        {
            return base.Client.GetCrmRoles(itemId, accountId, packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetCrmRolesAsync(int itemId, int accountId, int packageId)
        {
            return await base.Client.GetCrmRolesAsync(itemId, accountId, packageId);
        }

        public SolidCP.Providers.Common.ResultObject SetUserRoles(int itemId, int accountId, int packageId, System.Guid[] roles)
        {
            return base.Client.SetUserRoles(itemId, accountId, packageId, roles);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserRolesAsync(int itemId, int accountId, int packageId, System.Guid[] roles)
        {
            return await base.Client.SetUserRolesAsync(itemId, accountId, packageId, roles);
        }

        public SolidCP.Providers.Common.ResultObject SetUserCALType(int itemId, int accountId, int packageId, int CALType)
        {
            return base.Client.SetUserCALType(itemId, accountId, packageId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserCALTypeAsync(int itemId, int accountId, int packageId, int CALType)
        {
            return await base.Client.SetUserCALTypeAsync(itemId, accountId, packageId, CALType);
        }

        public SolidCP.Providers.Common.ResultObject ChangeUserState(int itemId, int accountId, bool disable)
        {
            return base.Client.ChangeUserState(itemId, accountId, disable);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeUserStateAsync(int itemId, int accountId, bool disable)
        {
            return await base.Client.ChangeUserStateAsync(itemId, accountId, disable);
        }

        public SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUser(int itemId, int accountId)
        {
            return base.Client.GetCrmUser(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserAsync(int itemId, int accountId)
        {
            return await base.Client.GetCrmUserAsync(itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SetMaxDBSize(int itemId, int packageId, long maxSize)
        {
            return base.Client.SetMaxDBSize(itemId, packageId, maxSize);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMaxDBSizeAsync(int itemId, int packageId, long maxSize)
        {
            return await base.Client.SetMaxDBSizeAsync(itemId, packageId, maxSize);
        }

        public long GetDBSize(int itemId, int packageId)
        {
            return base.Client.GetDBSize(itemId, packageId);
        }

        public async System.Threading.Tasks.Task<long> GetDBSizeAsync(int itemId, int packageId)
        {
            return await base.Client.GetDBSizeAsync(itemId, packageId);
        }

        public long GetMaxDBSize(int itemId, int packageId)
        {
            return base.Client.GetMaxDBSize(itemId, packageId);
        }

        public async System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(int itemId, int packageId)
        {
            return await base.Client.GetMaxDBSizeAsync(itemId, packageId);
        }

        public int[] GetInstalledLanguagePacks(int packageId)
        {
            return base.Client.GetInstalledLanguagePacks(packageId);
        }

        public async System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync(int packageId)
        {
            return await base.Client.GetInstalledLanguagePacksAsync(packageId);
        }

        public int[] GetInstalledLanguagePacksByServiceId(int serviceId)
        {
            return base.Client.GetInstalledLanguagePacksByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetInstalledLanguagePacksByServiceIdAsync(serviceId);
        }
    }
}
#endif