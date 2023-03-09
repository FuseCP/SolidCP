#if Client
using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ISfBServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface ISfBServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/CreateOrganizationResponse")]
        string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/CreateOrganization", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/CreateOrganizationResponse")]
        System.Threading.Tasks.Task<string> CreateOrganizationAsync(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetOrganizationTenantId", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetOrganizationTenantIdResponse")]
        string GetOrganizationTenantId(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetOrganizationTenantId", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetOrganizationTenantIdResponse")]
        System.Threading.Tasks.Task<string> GetOrganizationTenantIdAsync(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/DeleteOrganizationResponse")]
        bool DeleteOrganization(string organizationId, string sipDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string sipDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/CreateUserResponse")]
        bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/CreateUserResponse")]
        System.Threading.Tasks.Task<bool> CreateUserAsync(string organizationId, string userUpn, SfBUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetSfBUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetSfBUserGeneralSettingsResponse")]
        SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetSfBUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetSfBUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SfBUser> GetSfBUserGeneralSettingsAsync(string organizationId, string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserGeneralSettingsResponse")]
        bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<bool> SetSfBUserGeneralSettingsAsync(string organizationId, string userUpn, SfBUser sfbUser);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserPlan", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserPlanResponse")]
        bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserPlan", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/SetSfBUserPlanResponse")]
        System.Threading.Tasks.Task<bool> SetSfBUserPlanAsync(string organizationId, string userUpn, SfBUserPlan plan);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/DeleteUserResponse")]
        bool DeleteUser(string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/DeleteUserResponse")]
        System.Threading.Tasks.Task<bool> DeleteUserAsync(string userUpn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetFederationDomains", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetFederationDomainsResponse")]
        SfBFederationDomain[] GetFederationDomains(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetFederationDomains", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetFederationDomainsResponse")]
        System.Threading.Tasks.Task<SfBFederationDomain[]> GetFederationDomainsAsync(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/AddFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/AddFederationDomainResponse")]
        bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/AddFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/AddFederationDomainResponse")]
        System.Threading.Tasks.Task<bool> AddFederationDomainAsync(string organizationId, string domainName, string proxyFqdn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/RemoveFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/RemoveFederationDomainResponse")]
        bool RemoveFederationDomain(string organizationId, string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/RemoveFederationDomain", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/RemoveFederationDomainResponse")]
        System.Threading.Tasks.Task<bool> RemoveFederationDomainAsync(string organizationId, string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/ReloadConfiguration", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/ReloadConfigurationResponse")]
        void ReloadConfiguration();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/ReloadConfiguration", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/ReloadConfigurationResponse")]
        System.Threading.Tasks.Task ReloadConfigurationAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetPolicyList", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetPolicyListResponse")]
        string[] GetPolicyList(SfBPolicyType type, string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISfBServer/GetPolicyList", ReplyAction = "http://smbsaas/solidcp/server/ISfBServer/GetPolicyListResponse")]
        System.Threading.Tasks.Task<string[]> GetPolicyListAsync(SfBPolicyType type, string name);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SfBServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ISfBServer
    {
        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return (string)Invoke("SolidCP.Server.SfBServer", "CreateOrganization", organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public async System.Threading.Tasks.Task<string> CreateOrganizationAsync(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return await InvokeAsync<string>("SolidCP.Server.SfBServer", "CreateOrganization", organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public string GetOrganizationTenantId(string organizationId)
        {
            return (string)Invoke("SolidCP.Server.SfBServer", "GetOrganizationTenantId", organizationId);
        }

        public async System.Threading.Tasks.Task<string> GetOrganizationTenantIdAsync(string organizationId)
        {
            return await InvokeAsync<string>("SolidCP.Server.SfBServer", "GetOrganizationTenantId", organizationId);
        }

        public bool DeleteOrganization(string organizationId, string sipDomain)
        {
            return (bool)Invoke("SolidCP.Server.SfBServer", "DeleteOrganization", organizationId, sipDomain);
        }

        public async System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string sipDomain)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SfBServer", "DeleteOrganization", organizationId, sipDomain);
        }

        public bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return (bool)Invoke("SolidCP.Server.SfBServer", "CreateUser", organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> CreateUserAsync(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SfBServer", "CreateUser", organizationId, userUpn, plan);
        }

        public SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn)
        {
            return (SfBUser)Invoke("SolidCP.Server.SfBServer", "GetSfBUserGeneralSettings", organizationId, userUpn);
        }

        public async System.Threading.Tasks.Task<SfBUser> GetSfBUserGeneralSettingsAsync(string organizationId, string userUpn)
        {
            return await InvokeAsync<SfBUser>("SolidCP.Server.SfBServer", "GetSfBUserGeneralSettings", organizationId, userUpn);
        }

        public bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser)
        {
            return (bool)Invoke("SolidCP.Server.SfBServer", "SetSfBUserGeneralSettings", organizationId, userUpn, sfbUser);
        }

        public async System.Threading.Tasks.Task<bool> SetSfBUserGeneralSettingsAsync(string organizationId, string userUpn, SfBUser sfbUser)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SfBServer", "SetSfBUserGeneralSettings", organizationId, userUpn, sfbUser);
        }

        public bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return (bool)Invoke("SolidCP.Server.SfBServer", "SetSfBUserPlan", organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> SetSfBUserPlanAsync(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SfBServer", "SetSfBUserPlan", organizationId, userUpn, plan);
        }

        public bool DeleteUser(string userUpn)
        {
            return (bool)Invoke("SolidCP.Server.SfBServer", "DeleteUser", userUpn);
        }

        public async System.Threading.Tasks.Task<bool> DeleteUserAsync(string userUpn)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SfBServer", "DeleteUser", userUpn);
        }

        public SfBFederationDomain[] GetFederationDomains(string organizationId)
        {
            return (SfBFederationDomain[])Invoke("SolidCP.Server.SfBServer", "GetFederationDomains", organizationId);
        }

        public async System.Threading.Tasks.Task<SfBFederationDomain[]> GetFederationDomainsAsync(string organizationId)
        {
            return await InvokeAsync<SfBFederationDomain[]>("SolidCP.Server.SfBServer", "GetFederationDomains", organizationId);
        }

        public bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            return (bool)Invoke("SolidCP.Server.SfBServer", "AddFederationDomain", organizationId, domainName, proxyFqdn);
        }

        public async System.Threading.Tasks.Task<bool> AddFederationDomainAsync(string organizationId, string domainName, string proxyFqdn)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SfBServer", "AddFederationDomain", organizationId, domainName, proxyFqdn);
        }

        public bool RemoveFederationDomain(string organizationId, string domainName)
        {
            return (bool)Invoke("SolidCP.Server.SfBServer", "RemoveFederationDomain", organizationId, domainName);
        }

        public async System.Threading.Tasks.Task<bool> RemoveFederationDomainAsync(string organizationId, string domainName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SfBServer", "RemoveFederationDomain", organizationId, domainName);
        }

        public void ReloadConfiguration()
        {
            Invoke("SolidCP.Server.SfBServer", "ReloadConfiguration");
        }

        public async System.Threading.Tasks.Task ReloadConfigurationAsync()
        {
            await InvokeAsync("SolidCP.Server.SfBServer", "ReloadConfiguration");
        }

        public string[] GetPolicyList(SfBPolicyType type, string name)
        {
            return (string[])Invoke("SolidCP.Server.SfBServer", "GetPolicyList", type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(SfBPolicyType type, string name)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.SfBServer", "GetPolicyList", type, name);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SfBServer : SolidCP.Web.Client.ClientBase<ISfBServer, SfBServerAssemblyClient>, ISfBServer
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

        public bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return base.Client.CreateUser(organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> CreateUserAsync(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return await base.Client.CreateUserAsync(organizationId, userUpn, plan);
        }

        public SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn)
        {
            return base.Client.GetSfBUserGeneralSettings(organizationId, userUpn);
        }

        public async System.Threading.Tasks.Task<SfBUser> GetSfBUserGeneralSettingsAsync(string organizationId, string userUpn)
        {
            return await base.Client.GetSfBUserGeneralSettingsAsync(organizationId, userUpn);
        }

        public bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser)
        {
            return base.Client.SetSfBUserGeneralSettings(organizationId, userUpn, sfbUser);
        }

        public async System.Threading.Tasks.Task<bool> SetSfBUserGeneralSettingsAsync(string organizationId, string userUpn, SfBUser sfbUser)
        {
            return await base.Client.SetSfBUserGeneralSettingsAsync(organizationId, userUpn, sfbUser);
        }

        public bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return base.Client.SetSfBUserPlan(organizationId, userUpn, plan);
        }

        public async System.Threading.Tasks.Task<bool> SetSfBUserPlanAsync(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return await base.Client.SetSfBUserPlanAsync(organizationId, userUpn, plan);
        }

        public bool DeleteUser(string userUpn)
        {
            return base.Client.DeleteUser(userUpn);
        }

        public async System.Threading.Tasks.Task<bool> DeleteUserAsync(string userUpn)
        {
            return await base.Client.DeleteUserAsync(userUpn);
        }

        public SfBFederationDomain[] GetFederationDomains(string organizationId)
        {
            return base.Client.GetFederationDomains(organizationId);
        }

        public async System.Threading.Tasks.Task<SfBFederationDomain[]> GetFederationDomainsAsync(string organizationId)
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

        public string[] GetPolicyList(SfBPolicyType type, string name)
        {
            return base.Client.GetPolicyList(type, name);
        }

        public async System.Threading.Tasks.Task<string[]> GetPolicyListAsync(SfBPolicyType type, string name)
        {
            return await base.Client.GetPolicyListAsync(type, name);
        }
    }
}
#endif