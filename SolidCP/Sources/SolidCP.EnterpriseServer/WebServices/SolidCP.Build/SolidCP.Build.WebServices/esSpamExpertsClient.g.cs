#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesSpamExperts", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesSpamExperts
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilterResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilter(SolidCP.EnterpriseServer.Base.SpamExpertsRoute route);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAsync(SolidCP.EnterpriseServer.Base.SpamExpertsRoute route);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilterResponse")]
        void DeleteDomainFilter(SolidCP.EnterpriseServer.DomainInfo id);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilterResponse")]
        System.Threading.Tasks.Task DeleteDomainFilterAsync(SolidCP.EnterpriseServer.DomainInfo id);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilterAliasResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilterAlias(SolidCP.EnterpriseServer.DomainInfo domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddDomainFilterAliasResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAliasAsync(SolidCP.EnterpriseServer.DomainInfo domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilterAliasResponse")]
        void DeleteDomainFilterAlias(SolidCP.EnterpriseServer.DomainInfo domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteDomainFilterAliasResponse")]
        System.Threading.Tasks.Task DeleteDomainFilterAliasAsync(SolidCP.EnterpriseServer.DomainInfo domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddEmailFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddEmailFilterResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult AddEmailFilter(int packageId, string username, string password, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddEmailFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/AddEmailFilterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddEmailFilterAsync(int packageId, string username, string password, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteEmailFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteEmailFilterResponse")]
        void DeleteEmailFilter(int packageId, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteEmailFilter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/DeleteEmailFilterResponse")]
        System.Threading.Tasks.Task DeleteEmailFilterAsync(int packageId, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/SetEmailFilterPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/SetEmailFilterPasswordResponse")]
        void SetEmailFilterPassword(int packageId, string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/SetEmailFilterPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/SetEmailFilterPasswordResponse")]
        System.Threading.Tasks.Task SetEmailFilterPasswordAsync(int packageId, string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/IsSpamExpertsEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/IsSpamExpertsEnabledResponse")]
        bool IsSpamExpertsEnabled(int packageId, string group);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/IsSpamExpertsEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSpamExperts/IsSpamExpertsEnabledResponse")]
        System.Threading.Tasks.Task<bool> IsSpamExpertsEnabledAsync(int packageId, string group);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSpamExpertsAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesSpamExperts
    {
        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilter(SolidCP.EnterpriseServer.Base.SpamExpertsRoute route)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.EnterpriseServer.esSpamExperts", "AddDomainFilter", route);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAsync(SolidCP.EnterpriseServer.Base.SpamExpertsRoute route)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.EnterpriseServer.esSpamExperts", "AddDomainFilter", route);
        }

        public void DeleteDomainFilter(SolidCP.EnterpriseServer.DomainInfo id)
        {
            Invoke("SolidCP.EnterpriseServer.esSpamExperts", "DeleteDomainFilter", id);
        }

        public async System.Threading.Tasks.Task DeleteDomainFilterAsync(SolidCP.EnterpriseServer.DomainInfo id)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esSpamExperts", "DeleteDomainFilter", id);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilterAlias(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.EnterpriseServer.esSpamExperts", "AddDomainFilterAlias", domain, alias);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAliasAsync(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.EnterpriseServer.esSpamExperts", "AddDomainFilterAlias", domain, alias);
        }

        public void DeleteDomainFilterAlias(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            Invoke("SolidCP.EnterpriseServer.esSpamExperts", "DeleteDomainFilterAlias", domain, alias);
        }

        public async System.Threading.Tasks.Task DeleteDomainFilterAliasAsync(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esSpamExperts", "DeleteDomainFilterAlias", domain, alias);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddEmailFilter(int packageId, string username, string password, string domain)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.EnterpriseServer.esSpamExperts", "AddEmailFilter", packageId, username, password, domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddEmailFilterAsync(int packageId, string username, string password, string domain)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.EnterpriseServer.esSpamExperts", "AddEmailFilter", packageId, username, password, domain);
        }

        public void DeleteEmailFilter(int packageId, string email)
        {
            Invoke("SolidCP.EnterpriseServer.esSpamExperts", "DeleteEmailFilter", packageId, email);
        }

        public async System.Threading.Tasks.Task DeleteEmailFilterAsync(int packageId, string email)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esSpamExperts", "DeleteEmailFilter", packageId, email);
        }

        public void SetEmailFilterPassword(int packageId, string email, string password)
        {
            Invoke("SolidCP.EnterpriseServer.esSpamExperts", "SetEmailFilterPassword", packageId, email, password);
        }

        public async System.Threading.Tasks.Task SetEmailFilterPasswordAsync(int packageId, string email, string password)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esSpamExperts", "SetEmailFilterPassword", packageId, email, password);
        }

        public bool IsSpamExpertsEnabled(int packageId, string group)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esSpamExperts", "IsSpamExpertsEnabled", packageId, group);
        }

        public async System.Threading.Tasks.Task<bool> IsSpamExpertsEnabledAsync(int packageId, string group)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esSpamExperts", "IsSpamExpertsEnabled", packageId, group);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSpamExperts : SolidCP.Web.Client.ClientBase<IesSpamExperts, esSpamExpertsAssemblyClient>, IesSpamExperts
    {
        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilter(SolidCP.EnterpriseServer.Base.SpamExpertsRoute route)
        {
            return base.Client.AddDomainFilter(route);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAsync(SolidCP.EnterpriseServer.Base.SpamExpertsRoute route)
        {
            return await base.Client.AddDomainFilterAsync(route);
        }

        public void DeleteDomainFilter(SolidCP.EnterpriseServer.DomainInfo id)
        {
            base.Client.DeleteDomainFilter(id);
        }

        public async System.Threading.Tasks.Task DeleteDomainFilterAsync(SolidCP.EnterpriseServer.DomainInfo id)
        {
            await base.Client.DeleteDomainFilterAsync(id);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilterAlias(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            return base.Client.AddDomainFilterAlias(domain, alias);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAliasAsync(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            return await base.Client.AddDomainFilterAliasAsync(domain, alias);
        }

        public void DeleteDomainFilterAlias(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            base.Client.DeleteDomainFilterAlias(domain, alias);
        }

        public async System.Threading.Tasks.Task DeleteDomainFilterAliasAsync(SolidCP.EnterpriseServer.DomainInfo domain, string alias)
        {
            await base.Client.DeleteDomainFilterAliasAsync(domain, alias);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddEmailFilter(int packageId, string username, string password, string domain)
        {
            return base.Client.AddEmailFilter(packageId, username, password, domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddEmailFilterAsync(int packageId, string username, string password, string domain)
        {
            return await base.Client.AddEmailFilterAsync(packageId, username, password, domain);
        }

        public void DeleteEmailFilter(int packageId, string email)
        {
            base.Client.DeleteEmailFilter(packageId, email);
        }

        public async System.Threading.Tasks.Task DeleteEmailFilterAsync(int packageId, string email)
        {
            await base.Client.DeleteEmailFilterAsync(packageId, email);
        }

        public void SetEmailFilterPassword(int packageId, string email, string password)
        {
            base.Client.SetEmailFilterPassword(packageId, email, password);
        }

        public async System.Threading.Tasks.Task SetEmailFilterPasswordAsync(int packageId, string email, string password)
        {
            await base.Client.SetEmailFilterPasswordAsync(packageId, email, password);
        }

        public bool IsSpamExpertsEnabled(int packageId, string group)
        {
            return base.Client.IsSpamExpertsEnabled(packageId, group);
        }

        public async System.Threading.Tasks.Task<bool> IsSpamExpertsEnabledAsync(int packageId, string group)
        {
            return await base.Client.IsSpamExpertsEnabledAsync(packageId, group);
        }
    }
}
#endif