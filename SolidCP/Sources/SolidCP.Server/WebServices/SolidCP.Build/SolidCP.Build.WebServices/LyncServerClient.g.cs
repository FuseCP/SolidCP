#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ILyncServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface ILyncServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/CreateOrganizationResponse")]
        string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/CreateOrganizationResponse")]
        System.Threading.Tasks.Task<string> CreateOrganizationAsync(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetOrganizationTenantId", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetOrganizationTenantIdResponse")]
        string GetOrganizationTenantId(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetOrganizationTenantId", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetOrganizationTenantIdResponse")]
        System.Threading.Tasks.Task<string> GetOrganizationTenantIdAsync(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/DeleteOrganizationResponse")]
        bool DeleteOrganization(string organizationId, string sipDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string sipDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/CreateUserResponse")]
        bool CreateUser(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/CreateUserResponse")]
        System.Threading.Tasks.Task<bool> CreateUserAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetLyncUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetLyncUserGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetLyncUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetLyncUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser> GetLyncUserGeneralSettingsAsync(string organizationId, string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserGeneralSettingsResponse")]
        bool SetLyncUserGeneralSettings(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUser lyncUser);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<bool> SetLyncUserGeneralSettingsAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUser lyncUser);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserPlan", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserPlanResponse")]
        bool SetLyncUserPlan(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserPlan", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/SetLyncUserPlanResponse")]
        System.Threading.Tasks.Task<bool> SetLyncUserPlanAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/DeleteUserResponse")]
        bool DeleteUser(string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/DeleteUserResponse")]
        System.Threading.Tasks.Task<bool> DeleteUserAsync(string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetFederationDomains", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetFederationDomainsResponse")]
        SolidCP.Providers.HostedSolution.LyncFederationDomain[] GetFederationDomains(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetFederationDomains", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetFederationDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncFederationDomain[]> GetFederationDomainsAsync(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/AddFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/AddFederationDomainResponse")]
        bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/AddFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/AddFederationDomainResponse")]
        System.Threading.Tasks.Task<bool> AddFederationDomainAsync(string organizationId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/RemoveFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/RemoveFederationDomainResponse")]
        bool RemoveFederationDomain(string organizationId, string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/RemoveFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/RemoveFederationDomainResponse")]
        System.Threading.Tasks.Task<bool> RemoveFederationDomainAsync(string organizationId, string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/ReloadConfiguration", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/ReloadConfigurationResponse")]
        void ReloadConfiguration();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/ReloadConfiguration", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/ReloadConfigurationResponse")]
        System.Threading.Tasks.Task ReloadConfigurationAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetPolicyList", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetPolicyListResponse")]
        string[] GetPolicyList(SolidCP.Providers.HostedSolution.LyncPolicyType type, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ILyncServer/GetPolicyList", ReplyAction = "http://smbsaas/solidcp/server/ILyncServer/GetPolicyListResponse")]
        System.Threading.Tasks.Task<string[]> GetPolicyListAsync(SolidCP.Providers.HostedSolution.LyncPolicyType type, string name);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class LyncServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ILyncServer
    {
        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return Invoke<string>("SolidCP.Server.LyncServer", "CreateOrganization", organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public async System.Threading.Tasks.Task<string> CreateOrganizationAsync(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return await InvokeAsync<string>("SolidCP.Server.LyncServer", "CreateOrganization", organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public string GetOrganizationTenantId(string organizationId)
        {
            return Invoke<string>("SolidCP.Server.LyncServer", "GetOrganizationTenantId", organizationId);
        }

        public async System.Threading.Tasks.Task<string> GetOrganizationTenantIdAsync(string organizationId)
        {
            return await InvokeAsync<string>("SolidCP.Server.LyncServer", "GetOrganizationTenantId", organizationId);
        }

        public bool DeleteOrganization(string organizationId, string sipDomain)
        {
            return Invoke<bool>("SolidCP.Server.LyncServer", "DeleteOrganization", organizationId, sipDomain);
        }

        public async System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string sipDomain)
        {
            return await InvokeAsync<bool>("SolidCP.Server.LyncServer", "DeleteOrganization", organizationId, sipDomain);
        }

        public bool CreateUser(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return Invoke<bool>("SolidCP.Server.LyncServer", "CreateUser", organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> CreateUserAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return await InvokeAsync<bool>("SolidCP.Server.LyncServer", "CreateUser", organizationId, userUpn, plan);
        }

        public SolidCP.Providers.HostedSolution.LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn)
        {
            return Invoke<SolidCP.Providers.HostedSolution.LyncUser>("SolidCP.Server.LyncServer", "GetLyncUserGeneralSettings", organizationId, userUpn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser> GetLyncUserGeneralSettingsAsync(string organizationId, string userUpn)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.LyncUser>("SolidCP.Server.LyncServer", "GetLyncUserGeneralSettings", organizationId, userUpn);
        }

        public bool SetLyncUserGeneralSettings(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUser lyncUser)
        {
            return Invoke<bool>("SolidCP.Server.LyncServer", "SetLyncUserGeneralSettings", organizationId, userUpn, lyncUser);
        }

        public async System.Threading.Tasks.Task<bool> SetLyncUserGeneralSettingsAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUser lyncUser)
        {
            return await InvokeAsync<bool>("SolidCP.Server.LyncServer", "SetLyncUserGeneralSettings", organizationId, userUpn, lyncUser);
        }

        public bool SetLyncUserPlan(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return Invoke<bool>("SolidCP.Server.LyncServer", "SetLyncUserPlan", organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> SetLyncUserPlanAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return await InvokeAsync<bool>("SolidCP.Server.LyncServer", "SetLyncUserPlan", organizationId, userUpn, plan);
        }

        public bool DeleteUser(string userUpn)
        {
            return Invoke<bool>("SolidCP.Server.LyncServer", "DeleteUser", userUpn);
        }

        public async System.Threading.Tasks.Task<bool> DeleteUserAsync(string userUpn)
        {
            return await InvokeAsync<bool>("SolidCP.Server.LyncServer", "DeleteUser", userUpn);
        }

        public SolidCP.Providers.HostedSolution.LyncFederationDomain[] GetFederationDomains(string organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.LyncFederationDomain[]>("SolidCP.Server.LyncServer", "GetFederationDomains", organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncFederationDomain[]> GetFederationDomainsAsync(string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.LyncFederationDomain[]>("SolidCP.Server.LyncServer", "GetFederationDomains", organizationId);
        }

        public bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            return Invoke<bool>("SolidCP.Server.LyncServer", "AddFederationDomain", organizationId, domainName, proxyFqdn);
        }

        public async System.Threading.Tasks.Task<bool> AddFederationDomainAsync(string organizationId, string domainName, string proxyFqdn)
        {
            return await InvokeAsync<bool>("SolidCP.Server.LyncServer", "AddFederationDomain", organizationId, domainName, proxyFqdn);
        }

        public bool RemoveFederationDomain(string organizationId, string domainName)
        {
            return Invoke<bool>("SolidCP.Server.LyncServer", "RemoveFederationDomain", organizationId, domainName);
        }

        public async System.Threading.Tasks.Task<bool> RemoveFederationDomainAsync(string organizationId, string domainName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.LyncServer", "RemoveFederationDomain", organizationId, domainName);
        }

        public void ReloadConfiguration()
        {
            Invoke("SolidCP.Server.LyncServer", "ReloadConfiguration");
        }

        public async System.Threading.Tasks.Task ReloadConfigurationAsync()
        {
            await InvokeAsync("SolidCP.Server.LyncServer", "ReloadConfiguration");
        }

        public string[] GetPolicyList(SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return Invoke<string[]>("SolidCP.Server.LyncServer", "GetPolicyList", type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.LyncServer", "GetPolicyList", type, name);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class LyncServer : SolidCP.Web.Client.ClientBase<ILyncServer, LyncServerAssemblyClient>, ILyncServer
    {
        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return base.Client.CreateOrganization(organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public async System.Threading.Tasks.Task<string> CreateOrganizationAsync(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return await base.Client.CreateOrganizationAsync(organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public string GetOrganizationTenantId(string organizationId)
        {
            return base.Client.GetOrganizationTenantId(organizationId);
        }

        public async System.Threading.Tasks.Task<string> GetOrganizationTenantIdAsync(string organizationId)
        {
            return await base.Client.GetOrganizationTenantIdAsync(organizationId);
        }

        public bool DeleteOrganization(string organizationId, string sipDomain)
        {
            return base.Client.DeleteOrganization(organizationId, sipDomain);
        }

        public async System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string sipDomain)
        {
            return await base.Client.DeleteOrganizationAsync(organizationId, sipDomain);
        }

        public bool CreateUser(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return base.Client.CreateUser(organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> CreateUserAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return await base.Client.CreateUserAsync(organizationId, userUpn, plan);
        }

        public SolidCP.Providers.HostedSolution.LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn)
        {
            return base.Client.GetLyncUserGeneralSettings(organizationId, userUpn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncUser> GetLyncUserGeneralSettingsAsync(string organizationId, string userUpn)
        {
            return await base.Client.GetLyncUserGeneralSettingsAsync(organizationId, userUpn);
        }

        public bool SetLyncUserGeneralSettings(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUser lyncUser)
        {
            return base.Client.SetLyncUserGeneralSettings(organizationId, userUpn, lyncUser);
        }

        public async System.Threading.Tasks.Task<bool> SetLyncUserGeneralSettingsAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUser lyncUser)
        {
            return await base.Client.SetLyncUserGeneralSettingsAsync(organizationId, userUpn, lyncUser);
        }

        public bool SetLyncUserPlan(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return base.Client.SetLyncUserPlan(organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> SetLyncUserPlanAsync(string organizationId, string userUpn, SolidCP.Providers.HostedSolution.LyncUserPlan plan)
        {
            return await base.Client.SetLyncUserPlanAsync(organizationId, userUpn, plan);
        }

        public bool DeleteUser(string userUpn)
        {
            return base.Client.DeleteUser(userUpn);
        }

        public async System.Threading.Tasks.Task<bool> DeleteUserAsync(string userUpn)
        {
            return await base.Client.DeleteUserAsync(userUpn);
        }

        public SolidCP.Providers.HostedSolution.LyncFederationDomain[] GetFederationDomains(string organizationId)
        {
            return base.Client.GetFederationDomains(organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.LyncFederationDomain[]> GetFederationDomainsAsync(string organizationId)
        {
            return await base.Client.GetFederationDomainsAsync(organizationId);
        }

        public bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            return base.Client.AddFederationDomain(organizationId, domainName, proxyFqdn);
        }

        public async System.Threading.Tasks.Task<bool> AddFederationDomainAsync(string organizationId, string domainName, string proxyFqdn)
        {
            return await base.Client.AddFederationDomainAsync(organizationId, domainName, proxyFqdn);
        }

        public bool RemoveFederationDomain(string organizationId, string domainName)
        {
            return base.Client.RemoveFederationDomain(organizationId, domainName);
        }

        public async System.Threading.Tasks.Task<bool> RemoveFederationDomainAsync(string organizationId, string domainName)
        {
            return await base.Client.RemoveFederationDomainAsync(organizationId, domainName);
        }

        public void ReloadConfiguration()
        {
            base.Client.ReloadConfiguration();
        }

        public async System.Threading.Tasks.Task ReloadConfigurationAsync()
        {
            await base.Client.ReloadConfigurationAsync();
        }

        public string[] GetPolicyList(SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return base.Client.GetPolicyList(type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(SolidCP.Providers.HostedSolution.LyncPolicyType type, string name)
        {
            return await base.Client.GetPolicyListAsync(type, name);
        }
    }
}
#endif