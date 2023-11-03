#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesSfB", Namespace = "http://tempuri.org/")]
    public interface IesSfB
    {
        [OperationContract(Action = "http://tempuri.org/IesSfB/CreateSfBUser", ReplyAction = "http://tempuri.org/IesSfB/CreateSfBUserResponse")]
        SolidCP.Providers.ResultObjects.SfBUserResult CreateSfBUser(int itemId, int accountId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/CreateSfBUser", ReplyAction = "http://tempuri.org/IesSfB/CreateSfBUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> CreateSfBUserAsync(int itemId, int accountId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/DeleteSfBUser", ReplyAction = "http://tempuri.org/IesSfB/DeleteSfBUserResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSfBUser(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/DeleteSfBUser", ReplyAction = "http://tempuri.org/IesSfB/DeleteSfBUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSfBUserAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUsersPaged", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUsersPagedResponse")]
        SolidCP.Providers.ResultObjects.SfBUsersPagedResult GetSfBUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUsersPaged", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUsersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUsersPagedResult> GetSfBUsersPagedAsync(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUsersByPlanId", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUsersByPlanIdResponse")]
        SolidCP.Providers.HostedSolution.SfBUser[] /*List*/ GetSfBUsersByPlanId(int itemId, int planId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUsersByPlanId", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUsersByPlanIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUser[]> GetSfBUsersByPlanIdAsync(int itemId, int planId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserCount", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserCountResponse")]
        SolidCP.Providers.ResultObjects.IntResult GetSfBUserCount(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserCount", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserCountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetSfBUserCountAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserPlans", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserPlansResponse")]
        SolidCP.Providers.HostedSolution.SfBUserPlan[] /*List*/ GetSfBUserPlans(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserPlans", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserPlansResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUserPlan[]> GetSfBUserPlansAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserPlanResponse")]
        SolidCP.Providers.HostedSolution.SfBUserPlan GetSfBUserPlan(int itemId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserPlanResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUserPlan> GetSfBUserPlanAsync(int itemId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/AddSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/AddSfBUserPlanResponse")]
        int AddSfBUserPlan(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesSfB/AddSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/AddSfBUserPlanResponse")]
        System.Threading.Tasks.Task<int> AddSfBUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesSfB/UpdateSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/UpdateSfBUserPlanResponse")]
        int UpdateSfBUserPlan(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesSfB/UpdateSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/UpdateSfBUserPlanResponse")]
        System.Threading.Tasks.Task<int> UpdateSfBUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesSfB/DeleteSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/DeleteSfBUserPlanResponse")]
        int DeleteSfBUserPlan(int itemId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/DeleteSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/DeleteSfBUserPlanResponse")]
        System.Threading.Tasks.Task<int> DeleteSfBUserPlanAsync(int itemId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/SetOrganizationDefaultSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/SetOrganizationDefaultSfBUserPlanResponse")]
        int SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/SetOrganizationDefaultSfBUserPlan", ReplyAction = "http://tempuri.org/IesSfB/SetOrganizationDefaultSfBUserPlanResponse")]
        System.Threading.Tasks.Task<int> SetOrganizationDefaultSfBUserPlanAsync(int itemId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserGeneralSettings", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.SfBUser GetSfBUserGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetSfBUserGeneralSettings", ReplyAction = "http://tempuri.org/IesSfB/GetSfBUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUser> GetSfBUserGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/SetSfBUserGeneralSettings", ReplyAction = "http://tempuri.org/IesSfB/SetSfBUserGeneralSettingsResponse")]
        SolidCP.Providers.ResultObjects.SfBUserResult SetSfBUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri);
        [OperationContract(Action = "http://tempuri.org/IesSfB/SetSfBUserGeneralSettings", ReplyAction = "http://tempuri.org/IesSfB/SetSfBUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> SetSfBUserGeneralSettingsAsync(int itemId, int accountId, string sipAddress, string lineUri);
        [OperationContract(Action = "http://tempuri.org/IesSfB/SetUserSfBPlan", ReplyAction = "http://tempuri.org/IesSfB/SetUserSfBPlanResponse")]
        SolidCP.Providers.ResultObjects.SfBUserResult SetUserSfBPlan(int itemId, int accountId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/SetUserSfBPlan", ReplyAction = "http://tempuri.org/IesSfB/SetUserSfBPlanResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> SetUserSfBPlanAsync(int itemId, int accountId, int sfbUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetFederationDomains", ReplyAction = "http://tempuri.org/IesSfB/GetFederationDomainsResponse")]
        SolidCP.Providers.HostedSolution.SfBFederationDomain[] GetFederationDomains(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetFederationDomains", ReplyAction = "http://tempuri.org/IesSfB/GetFederationDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBFederationDomain[]> GetFederationDomainsAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesSfB/AddFederationDomain", ReplyAction = "http://tempuri.org/IesSfB/AddFederationDomainResponse")]
        SolidCP.Providers.ResultObjects.SfBUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://tempuri.org/IesSfB/AddFederationDomain", ReplyAction = "http://tempuri.org/IesSfB/AddFederationDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> AddFederationDomainAsync(int itemId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://tempuri.org/IesSfB/RemoveFederationDomain", ReplyAction = "http://tempuri.org/IesSfB/RemoveFederationDomainResponse")]
        SolidCP.Providers.ResultObjects.SfBUserResult RemoveFederationDomain(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesSfB/RemoveFederationDomain", ReplyAction = "http://tempuri.org/IesSfB/RemoveFederationDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> RemoveFederationDomainAsync(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetPolicyList", ReplyAction = "http://tempuri.org/IesSfB/GetPolicyListResponse")]
        string[] GetPolicyList(int itemId, SolidCP.Providers.HostedSolution.SfBPolicyType type, string name);
        [OperationContract(Action = "http://tempuri.org/IesSfB/GetPolicyList", ReplyAction = "http://tempuri.org/IesSfB/GetPolicyListResponse")]
        System.Threading.Tasks.Task<string[]> GetPolicyListAsync(int itemId, SolidCP.Providers.HostedSolution.SfBPolicyType type, string name);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSfBAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesSfB
    {
        public SolidCP.Providers.ResultObjects.SfBUserResult CreateSfBUser(int itemId, int accountId, int sfbUserPlanId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "CreateSfBUser", itemId, accountId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> CreateSfBUserAsync(int itemId, int accountId, int sfbUserPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "CreateSfBUser", itemId, accountId, sfbUserPlanId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSfBUser(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esSfB", "DeleteSfBUser", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSfBUserAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esSfB", "DeleteSfBUser", itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.SfBUsersPagedResult GetSfBUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.ResultObjects.SfBUsersPagedResult>("SolidCP.EnterpriseServer.esSfB", "GetSfBUsersPaged", itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUsersPagedResult> GetSfBUsersPagedAsync(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.SfBUsersPagedResult>("SolidCP.EnterpriseServer.esSfB", "GetSfBUsersPaged", itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.SfBUser[] /*List*/ GetSfBUsersByPlanId(int itemId, int planId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.SfBUser[], SolidCP.Providers.HostedSolution.SfBUser>("SolidCP.EnterpriseServer.esSfB", "GetSfBUsersByPlanId", itemId, planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUser[]> GetSfBUsersByPlanIdAsync(int itemId, int planId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.SfBUser[], SolidCP.Providers.HostedSolution.SfBUser>("SolidCP.EnterpriseServer.esSfB", "GetSfBUsersByPlanId", itemId, planId);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetSfBUserCount(int itemId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserCount", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetSfBUserCountAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserCount", itemId);
        }

        public SolidCP.Providers.HostedSolution.SfBUserPlan[] /*List*/ GetSfBUserPlans(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.SfBUserPlan[], SolidCP.Providers.HostedSolution.SfBUserPlan>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserPlans", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUserPlan[]> GetSfBUserPlansAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.SfBUserPlan[], SolidCP.Providers.HostedSolution.SfBUserPlan>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserPlans", itemId);
        }

        public SolidCP.Providers.HostedSolution.SfBUserPlan GetSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.SfBUserPlan>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserPlan", itemId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUserPlan> GetSfBUserPlanAsync(int itemId, int sfbUserPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.SfBUserPlan>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserPlan", itemId, sfbUserPlanId);
        }

        public int AddSfBUserPlan(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSfB", "AddSfBUserPlan", itemId, sfbUserPlan);
        }

        public async System.Threading.Tasks.Task<int> AddSfBUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSfB", "AddSfBUserPlan", itemId, sfbUserPlan);
        }

        public int UpdateSfBUserPlan(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSfB", "UpdateSfBUserPlan", itemId, sfbUserPlan);
        }

        public async System.Threading.Tasks.Task<int> UpdateSfBUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSfB", "UpdateSfBUserPlan", itemId, sfbUserPlan);
        }

        public int DeleteSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSfB", "DeleteSfBUserPlan", itemId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSfBUserPlanAsync(int itemId, int sfbUserPlanId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSfB", "DeleteSfBUserPlan", itemId, sfbUserPlanId);
        }

        public int SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSfB", "SetOrganizationDefaultSfBUserPlan", itemId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationDefaultSfBUserPlanAsync(int itemId, int sfbUserPlanId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSfB", "SetOrganizationDefaultSfBUserPlan", itemId, sfbUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.SfBUser GetSfBUserGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.SfBUser>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUser> GetSfBUserGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.SfBUser>("SolidCP.EnterpriseServer.esSfB", "GetSfBUserGeneralSettings", itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult SetSfBUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return Invoke<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "SetSfBUserGeneralSettings", itemId, accountId, sipAddress, lineUri);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> SetSfBUserGeneralSettingsAsync(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "SetSfBUserGeneralSettings", itemId, accountId, sipAddress, lineUri);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult SetUserSfBPlan(int itemId, int accountId, int sfbUserPlanId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "SetUserSfBPlan", itemId, accountId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> SetUserSfBPlanAsync(int itemId, int accountId, int sfbUserPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "SetUserSfBPlan", itemId, accountId, sfbUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.SfBFederationDomain[] GetFederationDomains(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.SfBFederationDomain[]>("SolidCP.EnterpriseServer.esSfB", "GetFederationDomains", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBFederationDomain[]> GetFederationDomainsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.SfBFederationDomain[]>("SolidCP.EnterpriseServer.esSfB", "GetFederationDomains", itemId);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return Invoke<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "AddFederationDomain", itemId, domainName, proxyFqdn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> AddFederationDomainAsync(int itemId, string domainName, string proxyFqdn)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "AddFederationDomain", itemId, domainName, proxyFqdn);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "RemoveFederationDomain", itemId, domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> RemoveFederationDomainAsync(int itemId, string domainName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.SfBUserResult>("SolidCP.EnterpriseServer.esSfB", "RemoveFederationDomain", itemId, domainName);
        }

        public string[] GetPolicyList(int itemId, SolidCP.Providers.HostedSolution.SfBPolicyType type, string name)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esSfB", "GetPolicyList", itemId, type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(int itemId, SolidCP.Providers.HostedSolution.SfBPolicyType type, string name)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esSfB", "GetPolicyList", itemId, type, name);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSfB : SolidCP.Web.Client.ClientBase<IesSfB, esSfBAssemblyClient>, IesSfB
    {
        public SolidCP.Providers.ResultObjects.SfBUserResult CreateSfBUser(int itemId, int accountId, int sfbUserPlanId)
        {
            return base.Client.CreateSfBUser(itemId, accountId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> CreateSfBUserAsync(int itemId, int accountId, int sfbUserPlanId)
        {
            return await base.Client.CreateSfBUserAsync(itemId, accountId, sfbUserPlanId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSfBUser(int itemId, int accountId)
        {
            return base.Client.DeleteSfBUser(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSfBUserAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteSfBUserAsync(itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.SfBUsersPagedResult GetSfBUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return base.Client.GetSfBUsersPaged(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUsersPagedResult> GetSfBUsersPagedAsync(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return await base.Client.GetSfBUsersPagedAsync(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.SfBUser[] /*List*/ GetSfBUsersByPlanId(int itemId, int planId)
        {
            return base.Client.GetSfBUsersByPlanId(itemId, planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUser[]> GetSfBUsersByPlanIdAsync(int itemId, int planId)
        {
            return await base.Client.GetSfBUsersByPlanIdAsync(itemId, planId);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetSfBUserCount(int itemId)
        {
            return base.Client.GetSfBUserCount(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetSfBUserCountAsync(int itemId)
        {
            return await base.Client.GetSfBUserCountAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.SfBUserPlan[] /*List*/ GetSfBUserPlans(int itemId)
        {
            return base.Client.GetSfBUserPlans(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUserPlan[]> GetSfBUserPlansAsync(int itemId)
        {
            return await base.Client.GetSfBUserPlansAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.SfBUserPlan GetSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return base.Client.GetSfBUserPlan(itemId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUserPlan> GetSfBUserPlanAsync(int itemId, int sfbUserPlanId)
        {
            return await base.Client.GetSfBUserPlanAsync(itemId, sfbUserPlanId);
        }

        public int AddSfBUserPlan(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return base.Client.AddSfBUserPlan(itemId, sfbUserPlan);
        }

        public async System.Threading.Tasks.Task<int> AddSfBUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return await base.Client.AddSfBUserPlanAsync(itemId, sfbUserPlan);
        }

        public int UpdateSfBUserPlan(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return base.Client.UpdateSfBUserPlan(itemId, sfbUserPlan);
        }

        public async System.Threading.Tasks.Task<int> UpdateSfBUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.SfBUserPlan sfbUserPlan)
        {
            return await base.Client.UpdateSfBUserPlanAsync(itemId, sfbUserPlan);
        }

        public int DeleteSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return base.Client.DeleteSfBUserPlan(itemId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSfBUserPlanAsync(int itemId, int sfbUserPlanId)
        {
            return await base.Client.DeleteSfBUserPlanAsync(itemId, sfbUserPlanId);
        }

        public int SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            return base.Client.SetOrganizationDefaultSfBUserPlan(itemId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationDefaultSfBUserPlanAsync(int itemId, int sfbUserPlanId)
        {
            return await base.Client.SetOrganizationDefaultSfBUserPlanAsync(itemId, sfbUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.SfBUser GetSfBUserGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetSfBUserGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBUser> GetSfBUserGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetSfBUserGeneralSettingsAsync(itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult SetSfBUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return base.Client.SetSfBUserGeneralSettings(itemId, accountId, sipAddress, lineUri);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> SetSfBUserGeneralSettingsAsync(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return await base.Client.SetSfBUserGeneralSettingsAsync(itemId, accountId, sipAddress, lineUri);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult SetUserSfBPlan(int itemId, int accountId, int sfbUserPlanId)
        {
            return base.Client.SetUserSfBPlan(itemId, accountId, sfbUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> SetUserSfBPlanAsync(int itemId, int accountId, int sfbUserPlanId)
        {
            return await base.Client.SetUserSfBPlanAsync(itemId, accountId, sfbUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.SfBFederationDomain[] GetFederationDomains(int itemId)
        {
            return base.Client.GetFederationDomains(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.SfBFederationDomain[]> GetFederationDomainsAsync(int itemId)
        {
            return await base.Client.GetFederationDomainsAsync(itemId);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return base.Client.AddFederationDomain(itemId, domainName, proxyFqdn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> AddFederationDomainAsync(int itemId, string domainName, string proxyFqdn)
        {
            return await base.Client.AddFederationDomainAsync(itemId, domainName, proxyFqdn);
        }

        public SolidCP.Providers.ResultObjects.SfBUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return base.Client.RemoveFederationDomain(itemId, domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.SfBUserResult> RemoveFederationDomainAsync(int itemId, string domainName)
        {
            return await base.Client.RemoveFederationDomainAsync(itemId, domainName);
        }

        public string[] GetPolicyList(int itemId, SolidCP.Providers.HostedSolution.SfBPolicyType type, string name)
        {
            return base.Client.GetPolicyList(itemId, type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(int itemId, SolidCP.Providers.HostedSolution.SfBPolicyType type, string name)
        {
            return await base.Client.GetPolicyListAsync(itemId, type, name);
        }
    }
}
#endif