#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesLync", Namespace = "http://tempuri.org/")]
    public interface IesLync
    {
        [OperationContract(Action = "http://tempuri.org/IesLync/CreateLyncUser", ReplyAction = "http://tempuri.org/IesLync/CreateLyncUserResponse")]
        SolidCP.Providers.ResultObjects.LyncUserResult CreateLyncUser(int itemId, int accountId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/CreateLyncUser", ReplyAction = "http://tempuri.org/IesLync/CreateLyncUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> CreateLyncUserAsync(int itemId, int accountId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/DeleteLyncUser", ReplyAction = "http://tempuri.org/IesLync/DeleteLyncUserResponse")]
        SolidCP.Providers.Common.ResultObject DeleteLyncUser(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesLync/DeleteLyncUser", ReplyAction = "http://tempuri.org/IesLync/DeleteLyncUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteLyncUserAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUsersPaged", ReplyAction = "http://tempuri.org/IesLync/GetLyncUsersPagedResponse")]
        SolidCP.Providers.ResultObjects.LyncUsersPagedResult GetLyncUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUsersPaged", ReplyAction = "http://tempuri.org/IesLync/GetLyncUsersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUsersPagedResult> GetLyncUsersPagedAsync(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUsersByPlanId", ReplyAction = "http://tempuri.org/IesLync/GetLyncUsersByPlanIdResponse")]
        SolidCP.Providers.HostedSolution.LyncUser[] /*List*/ GetLyncUsersByPlanId(int itemId, int planId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUsersByPlanId", ReplyAction = "http://tempuri.org/IesLync/GetLyncUsersByPlanIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser[]> GetLyncUsersByPlanIdAsync(int itemId, int planId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserCount", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserCountResponse")]
        SolidCP.Providers.ResultObjects.IntResult GetLyncUserCount(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserCount", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserCountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetLyncUserCountAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserPlans", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserPlansResponse")]
        SolidCP.Providers.HostedSolution.LyncUserPlan[] /*List*/ GetLyncUserPlans(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserPlans", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserPlansResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUserPlan[]> GetLyncUserPlansAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserPlanResponse")]
        SolidCP.Providers.HostedSolution.LyncUserPlan GetLyncUserPlan(int itemId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserPlanResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUserPlan> GetLyncUserPlanAsync(int itemId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/AddLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/AddLyncUserPlanResponse")]
        int AddLyncUserPlan(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesLync/AddLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/AddLyncUserPlanResponse")]
        System.Threading.Tasks.Task<int> AddLyncUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesLync/UpdateLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/UpdateLyncUserPlanResponse")]
        int UpdateLyncUserPlan(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesLync/UpdateLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/UpdateLyncUserPlanResponse")]
        System.Threading.Tasks.Task<int> UpdateLyncUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan);
        [OperationContract(Action = "http://tempuri.org/IesLync/DeleteLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/DeleteLyncUserPlanResponse")]
        int DeleteLyncUserPlan(int itemId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/DeleteLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/DeleteLyncUserPlanResponse")]
        System.Threading.Tasks.Task<int> DeleteLyncUserPlanAsync(int itemId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/SetOrganizationDefaultLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/SetOrganizationDefaultLyncUserPlanResponse")]
        int SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/SetOrganizationDefaultLyncUserPlan", ReplyAction = "http://tempuri.org/IesLync/SetOrganizationDefaultLyncUserPlanResponse")]
        System.Threading.Tasks.Task<int> SetOrganizationDefaultLyncUserPlanAsync(int itemId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserGeneralSettings", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.LyncUser GetLyncUserGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetLyncUserGeneralSettings", ReplyAction = "http://tempuri.org/IesLync/GetLyncUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser> GetLyncUserGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesLync/SetLyncUserGeneralSettings", ReplyAction = "http://tempuri.org/IesLync/SetLyncUserGeneralSettingsResponse")]
        SolidCP.Providers.ResultObjects.LyncUserResult SetLyncUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri);
        [OperationContract(Action = "http://tempuri.org/IesLync/SetLyncUserGeneralSettings", ReplyAction = "http://tempuri.org/IesLync/SetLyncUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> SetLyncUserGeneralSettingsAsync(int itemId, int accountId, string sipAddress, string lineUri);
        [OperationContract(Action = "http://tempuri.org/IesLync/SetUserLyncPlan", ReplyAction = "http://tempuri.org/IesLync/SetUserLyncPlanResponse")]
        SolidCP.Providers.ResultObjects.LyncUserResult SetUserLyncPlan(int itemId, int accountId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/SetUserLyncPlan", ReplyAction = "http://tempuri.org/IesLync/SetUserLyncPlanResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> SetUserLyncPlanAsync(int itemId, int accountId, int lyncUserPlanId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetFederationDomains", ReplyAction = "http://tempuri.org/IesLync/GetFederationDomainsResponse")]
        SolidCP.Providers.HostedSolution.LyncFederationDomain[] GetFederationDomains(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetFederationDomains", ReplyAction = "http://tempuri.org/IesLync/GetFederationDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncFederationDomain[]> GetFederationDomainsAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesLync/AddFederationDomain", ReplyAction = "http://tempuri.org/IesLync/AddFederationDomainResponse")]
        SolidCP.Providers.ResultObjects.LyncUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://tempuri.org/IesLync/AddFederationDomain", ReplyAction = "http://tempuri.org/IesLync/AddFederationDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> AddFederationDomainAsync(int itemId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://tempuri.org/IesLync/RemoveFederationDomain", ReplyAction = "http://tempuri.org/IesLync/RemoveFederationDomainResponse")]
        SolidCP.Providers.ResultObjects.LyncUserResult RemoveFederationDomain(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesLync/RemoveFederationDomain", ReplyAction = "http://tempuri.org/IesLync/RemoveFederationDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> RemoveFederationDomainAsync(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetPolicyList", ReplyAction = "http://tempuri.org/IesLync/GetPolicyListResponse")]
        string[] GetPolicyList(int itemId, SolidCP.Providers.HostedSolution.LyncPolicyType type, string name);
        [OperationContract(Action = "http://tempuri.org/IesLync/GetPolicyList", ReplyAction = "http://tempuri.org/IesLync/GetPolicyListResponse")]
        System.Threading.Tasks.Task<string[]> GetPolicyListAsync(int itemId, SolidCP.Providers.HostedSolution.LyncPolicyType type, string name);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esLyncAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesLync
    {
        public SolidCP.Providers.ResultObjects.LyncUserResult CreateLyncUser(int itemId, int accountId, int lyncUserPlanId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "CreateLyncUser", itemId, accountId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> CreateLyncUserAsync(int itemId, int accountId, int lyncUserPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "CreateLyncUser", itemId, accountId, lyncUserPlanId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteLyncUser(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esLync", "DeleteLyncUser", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteLyncUserAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esLync", "DeleteLyncUser", itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.LyncUsersPagedResult GetLyncUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.ResultObjects.LyncUsersPagedResult>("SolidCP.EnterpriseServer.esLync", "GetLyncUsersPaged", itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUsersPagedResult> GetLyncUsersPagedAsync(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.LyncUsersPagedResult>("SolidCP.EnterpriseServer.esLync", "GetLyncUsersPaged", itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.LyncUser[] /*List*/ GetLyncUsersByPlanId(int itemId, int planId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.LyncUser[], SolidCP.Providers.HostedSolution.LyncUser>("SolidCP.EnterpriseServer.esLync", "GetLyncUsersByPlanId", itemId, planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser[]> GetLyncUsersByPlanIdAsync(int itemId, int planId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.LyncUser[], SolidCP.Providers.HostedSolution.LyncUser>("SolidCP.EnterpriseServer.esLync", "GetLyncUsersByPlanId", itemId, planId);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetLyncUserCount(int itemId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esLync", "GetLyncUserCount", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetLyncUserCountAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esLync", "GetLyncUserCount", itemId);
        }

        public SolidCP.Providers.HostedSolution.LyncUserPlan[] /*List*/ GetLyncUserPlans(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.LyncUserPlan[], SolidCP.Providers.HostedSolution.LyncUserPlan>("SolidCP.EnterpriseServer.esLync", "GetLyncUserPlans", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUserPlan[]> GetLyncUserPlansAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.LyncUserPlan[], SolidCP.Providers.HostedSolution.LyncUserPlan>("SolidCP.EnterpriseServer.esLync", "GetLyncUserPlans", itemId);
        }

        public SolidCP.Providers.HostedSolution.LyncUserPlan GetLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.LyncUserPlan>("SolidCP.EnterpriseServer.esLync", "GetLyncUserPlan", itemId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUserPlan> GetLyncUserPlanAsync(int itemId, int lyncUserPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.LyncUserPlan>("SolidCP.EnterpriseServer.esLync", "GetLyncUserPlan", itemId, lyncUserPlanId);
        }

        public int AddLyncUserPlan(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esLync", "AddLyncUserPlan", itemId, lyncUserPlan);
        }

        public async System.Threading.Tasks.Task<int> AddLyncUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esLync", "AddLyncUserPlan", itemId, lyncUserPlan);
        }

        public int UpdateLyncUserPlan(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esLync", "UpdateLyncUserPlan", itemId, lyncUserPlan);
        }

        public async System.Threading.Tasks.Task<int> UpdateLyncUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esLync", "UpdateLyncUserPlan", itemId, lyncUserPlan);
        }

        public int DeleteLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esLync", "DeleteLyncUserPlan", itemId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> DeleteLyncUserPlanAsync(int itemId, int lyncUserPlanId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esLync", "DeleteLyncUserPlan", itemId, lyncUserPlanId);
        }

        public int SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esLync", "SetOrganizationDefaultLyncUserPlan", itemId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationDefaultLyncUserPlanAsync(int itemId, int lyncUserPlanId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esLync", "SetOrganizationDefaultLyncUserPlan", itemId, lyncUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.LyncUser GetLyncUserGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.LyncUser>("SolidCP.EnterpriseServer.esLync", "GetLyncUserGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser> GetLyncUserGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.LyncUser>("SolidCP.EnterpriseServer.esLync", "GetLyncUserGeneralSettings", itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult SetLyncUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return Invoke<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "SetLyncUserGeneralSettings", itemId, accountId, sipAddress, lineUri);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> SetLyncUserGeneralSettingsAsync(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "SetLyncUserGeneralSettings", itemId, accountId, sipAddress, lineUri);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult SetUserLyncPlan(int itemId, int accountId, int lyncUserPlanId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "SetUserLyncPlan", itemId, accountId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> SetUserLyncPlanAsync(int itemId, int accountId, int lyncUserPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "SetUserLyncPlan", itemId, accountId, lyncUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.LyncFederationDomain[] GetFederationDomains(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.LyncFederationDomain[]>("SolidCP.EnterpriseServer.esLync", "GetFederationDomains", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncFederationDomain[]> GetFederationDomainsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.LyncFederationDomain[]>("SolidCP.EnterpriseServer.esLync", "GetFederationDomains", itemId);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return Invoke<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "AddFederationDomain", itemId, domainName, proxyFqdn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> AddFederationDomainAsync(int itemId, string domainName, string proxyFqdn)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "AddFederationDomain", itemId, domainName, proxyFqdn);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "RemoveFederationDomain", itemId, domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> RemoveFederationDomainAsync(int itemId, string domainName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.LyncUserResult>("SolidCP.EnterpriseServer.esLync", "RemoveFederationDomain", itemId, domainName);
        }

        public string[] GetPolicyList(int itemId, SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esLync", "GetPolicyList", itemId, type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(int itemId, SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esLync", "GetPolicyList", itemId, type, name);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esLync : SolidCP.Web.Client.ClientBase<IesLync, esLyncAssemblyClient>, IesLync
    {
        public SolidCP.Providers.ResultObjects.LyncUserResult CreateLyncUser(int itemId, int accountId, int lyncUserPlanId)
        {
            return base.Client.CreateLyncUser(itemId, accountId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> CreateLyncUserAsync(int itemId, int accountId, int lyncUserPlanId)
        {
            return await base.Client.CreateLyncUserAsync(itemId, accountId, lyncUserPlanId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteLyncUser(int itemId, int accountId)
        {
            return base.Client.DeleteLyncUser(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteLyncUserAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteLyncUserAsync(itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.LyncUsersPagedResult GetLyncUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return base.Client.GetLyncUsersPaged(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUsersPagedResult> GetLyncUsersPagedAsync(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows)
        {
            return await base.Client.GetLyncUsersPagedAsync(itemId, sortColumn, sortDirection, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.LyncUser[] /*List*/ GetLyncUsersByPlanId(int itemId, int planId)
        {
            return base.Client.GetLyncUsersByPlanId(itemId, planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser[]> GetLyncUsersByPlanIdAsync(int itemId, int planId)
        {
            return await base.Client.GetLyncUsersByPlanIdAsync(itemId, planId);
        }

        public SolidCP.Providers.ResultObjects.IntResult GetLyncUserCount(int itemId)
        {
            return base.Client.GetLyncUserCount(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> GetLyncUserCountAsync(int itemId)
        {
            return await base.Client.GetLyncUserCountAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.LyncUserPlan[] /*List*/ GetLyncUserPlans(int itemId)
        {
            return base.Client.GetLyncUserPlans(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUserPlan[]> GetLyncUserPlansAsync(int itemId)
        {
            return await base.Client.GetLyncUserPlansAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.LyncUserPlan GetLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return base.Client.GetLyncUserPlan(itemId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUserPlan> GetLyncUserPlanAsync(int itemId, int lyncUserPlanId)
        {
            return await base.Client.GetLyncUserPlanAsync(itemId, lyncUserPlanId);
        }

        public int AddLyncUserPlan(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return base.Client.AddLyncUserPlan(itemId, lyncUserPlan);
        }

        public async System.Threading.Tasks.Task<int> AddLyncUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return await base.Client.AddLyncUserPlanAsync(itemId, lyncUserPlan);
        }

        public int UpdateLyncUserPlan(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return base.Client.UpdateLyncUserPlan(itemId, lyncUserPlan);
        }

        public async System.Threading.Tasks.Task<int> UpdateLyncUserPlanAsync(int itemId, SolidCP.Providers.HostedSolution.LyncUserPlan lyncUserPlan)
        {
            return await base.Client.UpdateLyncUserPlanAsync(itemId, lyncUserPlan);
        }

        public int DeleteLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return base.Client.DeleteLyncUserPlan(itemId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> DeleteLyncUserPlanAsync(int itemId, int lyncUserPlanId)
        {
            return await base.Client.DeleteLyncUserPlanAsync(itemId, lyncUserPlanId);
        }

        public int SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            return base.Client.SetOrganizationDefaultLyncUserPlan(itemId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationDefaultLyncUserPlanAsync(int itemId, int lyncUserPlanId)
        {
            return await base.Client.SetOrganizationDefaultLyncUserPlanAsync(itemId, lyncUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.LyncUser GetLyncUserGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetLyncUserGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser> GetLyncUserGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetLyncUserGeneralSettingsAsync(itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult SetLyncUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return base.Client.SetLyncUserGeneralSettings(itemId, accountId, sipAddress, lineUri);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> SetLyncUserGeneralSettingsAsync(int itemId, int accountId, string sipAddress, string lineUri)
        {
            return await base.Client.SetLyncUserGeneralSettingsAsync(itemId, accountId, sipAddress, lineUri);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult SetUserLyncPlan(int itemId, int accountId, int lyncUserPlanId)
        {
            return base.Client.SetUserLyncPlan(itemId, accountId, lyncUserPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> SetUserLyncPlanAsync(int itemId, int accountId, int lyncUserPlanId)
        {
            return await base.Client.SetUserLyncPlanAsync(itemId, accountId, lyncUserPlanId);
        }

        public SolidCP.Providers.HostedSolution.LyncFederationDomain[] GetFederationDomains(int itemId)
        {
            return base.Client.GetFederationDomains(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncFederationDomain[]> GetFederationDomainsAsync(int itemId)
        {
            return await base.Client.GetFederationDomainsAsync(itemId);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn)
        {
            return base.Client.AddFederationDomain(itemId, domainName, proxyFqdn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> AddFederationDomainAsync(int itemId, string domainName, string proxyFqdn)
        {
            return await base.Client.AddFederationDomainAsync(itemId, domainName, proxyFqdn);
        }

        public SolidCP.Providers.ResultObjects.LyncUserResult RemoveFederationDomain(int itemId, string domainName)
        {
            return base.Client.RemoveFederationDomain(itemId, domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.LyncUserResult> RemoveFederationDomainAsync(int itemId, string domainName)
        {
            return await base.Client.RemoveFederationDomainAsync(itemId, domainName);
        }

        public string[] GetPolicyList(int itemId, SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return base.Client.GetPolicyList(itemId, type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(int itemId, SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return await base.Client.GetPolicyListAsync(itemId, type, name);
        }
    }
}
#endif