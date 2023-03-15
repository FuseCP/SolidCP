#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ICRM", Namespace = "http://smbsaas/solidcp/server/")]
    public interface ICRM
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateOrganizationResponse")]
        SolidCP.Providers.Common.OrganizationResult CreateOrganization(System.Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.OrganizationResult> CreateOrganizationAsync(System.Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNames", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNamesResponse")]
        string[] GetSupportedCollationNames();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNames", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetSupportedCollationNamesResponse")]
        System.Threading.Tasks.Task<string[]> GetSupportedCollationNamesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCurrencyList", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCurrencyListResponse")]
        SolidCP.Providers.HostedSolution.Currency[] GetCurrencyList();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCurrencyList", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCurrencyListResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Currency[]> GetCurrencyListAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacks", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacksResponse")]
        int[] GetInstalledLanguagePacks();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacks", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetInstalledLanguagePacksResponse")]
        System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/DeleteOrganizationResponse")]
        SolidCP.Providers.Common.ResultObject DeleteOrganization(System.Guid orgId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ICRM/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteOrganizationAsync(System.Guid orgId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateCRMUser", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateCRMUserResponse")]
        SolidCP.Providers.ResultObjects.UserResult CreateCRMUser(SolidCP.Providers.HostedSolution.OrganizationUser user, string orgName, System.Guid organizationId, System.Guid baseUnitId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/CreateCRMUser", ReplyAction = "http://smbsaas/solidcp/server/ICRM/CreateCRMUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.UserResult> CreateCRMUserAsync(SolidCP.Providers.HostedSolution.OrganizationUser user, string orgName, System.Guid organizationId, System.Guid baseUnitId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnits", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnitsResponse")]
        SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult GetOrganizationBusinessUnits(System.Guid organizationId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnits", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetOrganizationBusinessUnitsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult> GetOrganizationBusinessUnitsAsync(System.Guid organizationId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRolesResponse")]
        SolidCP.Providers.ResultObjects.CrmRolesResult GetAllCrmRoles(string orgName, System.Guid businessUnitId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetAllCrmRolesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetAllCrmRolesAsync(string orgName, System.Guid businessUnitId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRolesResponse")]
        SolidCP.Providers.ResultObjects.CrmRolesResult GetCrmUserRoles(string orgName, System.Guid userId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserRolesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetCrmUserRolesAsync(string orgName, System.Guid userId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserRolesResponse")]
        SolidCP.Providers.Common.ResultObject SetUserRoles(string orgName, System.Guid userId, System.Guid[] roles);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserRoles", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserRolesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserRolesAsync(string orgName, System.Guid userId, System.Guid[] roles);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserCALType", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserCALTypeResponse")]
        SolidCP.Providers.Common.ResultObject SetUserCALType(string orgName, System.Guid userId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetUserCALType", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetUserCALTypeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserCALTypeAsync(string orgName, System.Guid userId, int CALType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainName", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainNameResponse")]
        SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUserByDomainName(string domainName, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainName", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByDomainNameResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserByDomainNameAsync(string domainName, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserById", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByIdResponse")]
        SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUserById(System.Guid crmUserId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetCrmUserById", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetCrmUserByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserByIdAsync(System.Guid crmUserId, string orgName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/ChangeUserState", ReplyAction = "http://smbsaas/solidcp/server/ICRM/ChangeUserStateResponse")]
        SolidCP.Providers.Common.ResultObject ChangeUserState(bool disable, string orgName, System.Guid crmUserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/ChangeUserState", ReplyAction = "http://smbsaas/solidcp/server/ICRM/ChangeUserStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeUserStateAsync(bool disable, string orgName, System.Guid crmUserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetDBSizeResponse")]
        long GetDBSize(System.Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetDBSizeResponse")]
        System.Threading.Tasks.Task<long> GetDBSizeAsync(System.Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSizeResponse")]
        long GetMaxDBSize(System.Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/GetMaxDBSizeResponse")]
        System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(System.Guid organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSizeResponse")]
        SolidCP.Providers.Common.ResultObject SetMaxDBSize(System.Guid organizationId, long maxSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSize", ReplyAction = "http://smbsaas/solidcp/server/ICRM/SetMaxDBSizeResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMaxDBSizeAsync(System.Guid organizationId, long maxSize);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class CRMAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ICRM
    {
        public SolidCP.Providers.Common.OrganizationResult CreateOrganization(System.Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return Invoke<SolidCP.Providers.Common.OrganizationResult>("SolidCP.Server.CRM", "CreateOrganization", organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.OrganizationResult> CreateOrganizationAsync(System.Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return await InvokeAsync<SolidCP.Providers.Common.OrganizationResult>("SolidCP.Server.CRM", "CreateOrganization", organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public string[] GetSupportedCollationNames()
        {
            return Invoke<string[]>("SolidCP.Server.CRM", "GetSupportedCollationNames");
        }

        public async System.Threading.Tasks.Task<string[]> GetSupportedCollationNamesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.CRM", "GetSupportedCollationNames");
        }

        public SolidCP.Providers.HostedSolution.Currency[] GetCurrencyList()
        {
            return Invoke<SolidCP.Providers.HostedSolution.Currency[]>("SolidCP.Server.CRM", "GetCurrencyList");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Currency[]> GetCurrencyListAsync()
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Currency[]>("SolidCP.Server.CRM", "GetCurrencyList");
        }

        public int[] GetInstalledLanguagePacks()
        {
            return Invoke<int[]>("SolidCP.Server.CRM", "GetInstalledLanguagePacks");
        }

        public async System.Threading.Tasks.Task<int[]> GetInstalledLanguagePacksAsync()
        {
            return await InvokeAsync<int[]>("SolidCP.Server.CRM", "GetInstalledLanguagePacks");
        }

        public SolidCP.Providers.Common.ResultObject DeleteOrganization(System.Guid orgId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "DeleteOrganization", orgId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteOrganizationAsync(System.Guid orgId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "DeleteOrganization", orgId);
        }

        public SolidCP.Providers.ResultObjects.UserResult CreateCRMUser(SolidCP.Providers.HostedSolution.OrganizationUser user, string orgName, System.Guid organizationId, System.Guid baseUnitId, int CALType)
        {
            return Invoke<SolidCP.Providers.ResultObjects.UserResult>("SolidCP.Server.CRM", "CreateCRMUser", user, orgName, organizationId, baseUnitId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.UserResult> CreateCRMUserAsync(SolidCP.Providers.HostedSolution.OrganizationUser user, string orgName, System.Guid organizationId, System.Guid baseUnitId, int CALType)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.UserResult>("SolidCP.Server.CRM", "CreateCRMUser", user, orgName, organizationId, baseUnitId, CALType);
        }

        public SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult GetOrganizationBusinessUnits(System.Guid organizationId, string orgName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult>("SolidCP.Server.CRM", "GetOrganizationBusinessUnits", organizationId, orgName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult> GetOrganizationBusinessUnitsAsync(System.Guid organizationId, string orgName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult>("SolidCP.Server.CRM", "GetOrganizationBusinessUnits", organizationId, orgName);
        }

        public SolidCP.Providers.ResultObjects.CrmRolesResult GetAllCrmRoles(string orgName, System.Guid businessUnitId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CrmRolesResult>("SolidCP.Server.CRM", "GetAllCrmRoles", orgName, businessUnitId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetAllCrmRolesAsync(string orgName, System.Guid businessUnitId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CrmRolesResult>("SolidCP.Server.CRM", "GetAllCrmRoles", orgName, businessUnitId);
        }

        public SolidCP.Providers.ResultObjects.CrmRolesResult GetCrmUserRoles(string orgName, System.Guid userId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CrmRolesResult>("SolidCP.Server.CRM", "GetCrmUserRoles", orgName, userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetCrmUserRolesAsync(string orgName, System.Guid userId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CrmRolesResult>("SolidCP.Server.CRM", "GetCrmUserRoles", orgName, userId);
        }

        public SolidCP.Providers.Common.ResultObject SetUserRoles(string orgName, System.Guid userId, System.Guid[] roles)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "SetUserRoles", orgName, userId, roles);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserRolesAsync(string orgName, System.Guid userId, System.Guid[] roles)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "SetUserRoles", orgName, userId, roles);
        }

        public SolidCP.Providers.Common.ResultObject SetUserCALType(string orgName, System.Guid userId, int CALType)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "SetUserCALType", orgName, userId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserCALTypeAsync(string orgName, System.Guid userId, int CALType)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "SetUserCALType", orgName, userId, CALType);
        }

        public SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CrmUserResult>("SolidCP.Server.CRM", "GetCrmUserByDomainName", domainName, orgName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserByDomainNameAsync(string domainName, string orgName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CrmUserResult>("SolidCP.Server.CRM", "GetCrmUserByDomainName", domainName, orgName);
        }

        public SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUserById(System.Guid crmUserId, string orgName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.CrmUserResult>("SolidCP.Server.CRM", "GetCrmUserById", crmUserId, orgName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserByIdAsync(System.Guid crmUserId, string orgName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.CrmUserResult>("SolidCP.Server.CRM", "GetCrmUserById", crmUserId, orgName);
        }

        public SolidCP.Providers.Common.ResultObject ChangeUserState(bool disable, string orgName, System.Guid crmUserId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "ChangeUserState", disable, orgName, crmUserId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeUserStateAsync(bool disable, string orgName, System.Guid crmUserId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "ChangeUserState", disable, orgName, crmUserId);
        }

        public long GetDBSize(System.Guid organizationId)
        {
            return Invoke<long>("SolidCP.Server.CRM", "GetDBSize", organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetDBSizeAsync(System.Guid organizationId)
        {
            return await InvokeAsync<long>("SolidCP.Server.CRM", "GetDBSize", organizationId);
        }

        public long GetMaxDBSize(System.Guid organizationId)
        {
            return Invoke<long>("SolidCP.Server.CRM", "GetMaxDBSize", organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(System.Guid organizationId)
        {
            return await InvokeAsync<long>("SolidCP.Server.CRM", "GetMaxDBSize", organizationId);
        }

        public SolidCP.Providers.Common.ResultObject SetMaxDBSize(System.Guid organizationId, long maxSize)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "SetMaxDBSize", organizationId, maxSize);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMaxDBSizeAsync(System.Guid organizationId, long maxSize)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.CRM", "SetMaxDBSize", organizationId, maxSize);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class CRM : SolidCP.Web.Client.ClientBase<ICRM, CRMAssemblyClient>, ICRM
    {
        public SolidCP.Providers.Common.OrganizationResult CreateOrganization(System.Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return base.Client.CreateOrganization(organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.OrganizationResult> CreateOrganizationAsync(System.Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
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

        public SolidCP.Providers.HostedSolution.Currency[] GetCurrencyList()
        {
            return base.Client.GetCurrencyList();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Currency[]> GetCurrencyListAsync()
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

        public SolidCP.Providers.Common.ResultObject DeleteOrganization(System.Guid orgId)
        {
            return base.Client.DeleteOrganization(orgId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteOrganizationAsync(System.Guid orgId)
        {
            return await base.Client.DeleteOrganizationAsync(orgId);
        }

        public SolidCP.Providers.ResultObjects.UserResult CreateCRMUser(SolidCP.Providers.HostedSolution.OrganizationUser user, string orgName, System.Guid organizationId, System.Guid baseUnitId, int CALType)
        {
            return base.Client.CreateCRMUser(user, orgName, organizationId, baseUnitId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.UserResult> CreateCRMUserAsync(SolidCP.Providers.HostedSolution.OrganizationUser user, string orgName, System.Guid organizationId, System.Guid baseUnitId, int CALType)
        {
            return await base.Client.CreateCRMUserAsync(user, orgName, organizationId, baseUnitId, CALType);
        }

        public SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult GetOrganizationBusinessUnits(System.Guid organizationId, string orgName)
        {
            return base.Client.GetOrganizationBusinessUnits(organizationId, orgName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CRMBusinessUnitsResult> GetOrganizationBusinessUnitsAsync(System.Guid organizationId, string orgName)
        {
            return await base.Client.GetOrganizationBusinessUnitsAsync(organizationId, orgName);
        }

        public SolidCP.Providers.ResultObjects.CrmRolesResult GetAllCrmRoles(string orgName, System.Guid businessUnitId)
        {
            return base.Client.GetAllCrmRoles(orgName, businessUnitId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetAllCrmRolesAsync(string orgName, System.Guid businessUnitId)
        {
            return await base.Client.GetAllCrmRolesAsync(orgName, businessUnitId);
        }

        public SolidCP.Providers.ResultObjects.CrmRolesResult GetCrmUserRoles(string orgName, System.Guid userId)
        {
            return base.Client.GetCrmUserRoles(orgName, userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmRolesResult> GetCrmUserRolesAsync(string orgName, System.Guid userId)
        {
            return await base.Client.GetCrmUserRolesAsync(orgName, userId);
        }

        public SolidCP.Providers.Common.ResultObject SetUserRoles(string orgName, System.Guid userId, System.Guid[] roles)
        {
            return base.Client.SetUserRoles(orgName, userId, roles);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserRolesAsync(string orgName, System.Guid userId, System.Guid[] roles)
        {
            return await base.Client.SetUserRolesAsync(orgName, userId, roles);
        }

        public SolidCP.Providers.Common.ResultObject SetUserCALType(string orgName, System.Guid userId, int CALType)
        {
            return base.Client.SetUserCALType(orgName, userId, CALType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUserCALTypeAsync(string orgName, System.Guid userId, int CALType)
        {
            return await base.Client.SetUserCALTypeAsync(orgName, userId, CALType);
        }

        public SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return base.Client.GetCrmUserByDomainName(domainName, orgName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserByDomainNameAsync(string domainName, string orgName)
        {
            return await base.Client.GetCrmUserByDomainNameAsync(domainName, orgName);
        }

        public SolidCP.Providers.ResultObjects.CrmUserResult GetCrmUserById(System.Guid crmUserId, string orgName)
        {
            return base.Client.GetCrmUserById(crmUserId, orgName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.CrmUserResult> GetCrmUserByIdAsync(System.Guid crmUserId, string orgName)
        {
            return await base.Client.GetCrmUserByIdAsync(crmUserId, orgName);
        }

        public SolidCP.Providers.Common.ResultObject ChangeUserState(bool disable, string orgName, System.Guid crmUserId)
        {
            return base.Client.ChangeUserState(disable, orgName, crmUserId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeUserStateAsync(bool disable, string orgName, System.Guid crmUserId)
        {
            return await base.Client.ChangeUserStateAsync(disable, orgName, crmUserId);
        }

        public long GetDBSize(System.Guid organizationId)
        {
            return base.Client.GetDBSize(organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetDBSizeAsync(System.Guid organizationId)
        {
            return await base.Client.GetDBSizeAsync(organizationId);
        }

        public long GetMaxDBSize(System.Guid organizationId)
        {
            return base.Client.GetMaxDBSize(organizationId);
        }

        public async System.Threading.Tasks.Task<long> GetMaxDBSizeAsync(System.Guid organizationId)
        {
            return await base.Client.GetMaxDBSizeAsync(organizationId);
        }

        public SolidCP.Providers.Common.ResultObject SetMaxDBSize(System.Guid organizationId, long maxSize)
        {
            return base.Client.SetMaxDBSize(organizationId, maxSize);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMaxDBSizeAsync(System.Guid organizationId, long maxSize)
        {
            return await base.Client.SetMaxDBSizeAsync(organizationId, maxSize);
        }
    }
}
#endif