#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ISpamExperts", Namespace = "http://smbsaas/solidcp/server/")]
    public interface ISpamExperts
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAsync(string domain, string password, string email, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilterResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult AddEmailFilter(string name, string domain, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddEmailFilterAsync(string name, string domain, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult DeleteDomainFilter(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteDomainFilterAsync(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilterResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult DeleteEmailFilter(string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilterResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteEmailFilterAsync(string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinations", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinationsResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinations", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterDestinationsAsync(string name, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUser", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterUser(string domain, string password, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUser", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterUserAsync(string domain, string password, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPasswordResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterUserPassword(string name, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterUserPasswordAsync(string name, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPasswordResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult SetEmailFilterUserPassword(string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetEmailFilterUserPasswordAsync(string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAliasResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilterAlias(string domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAliasResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAliasAsync(string domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAliasResponse")]
        SolidCP.Providers.Filters.SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAliasResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteDomainFilterAliasAsync(string domain, string alias);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SpamExpertsAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ISpamExperts
    {
        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddDomainFilter", domain, password, email, destinations);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAsync(string domain, string password, string email, string[] destinations)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddDomainFilter", domain, password, email, destinations);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddEmailFilter", name, domain, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddEmailFilterAsync(string name, string domain, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddEmailFilter", name, domain, password);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult DeleteDomainFilter(string domain)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteDomainFilter", domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteDomainFilterAsync(string domain)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteDomainFilter", domain);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult DeleteEmailFilter(string email)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteEmailFilter", email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteEmailFilterAsync(string email)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteEmailFilter", email);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterDestinations", name, destinations);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterDestinationsAsync(string name, string[] destinations)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterDestinations", name, destinations);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterUser", domain, password, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterUserAsync(string domain, string password, string email)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterUser", domain, password, email);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterUserPassword", name, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterUserPasswordAsync(string name, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterUserPassword", name, password);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetEmailFilterUserPassword", email, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetEmailFilterUserPasswordAsync(string email, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetEmailFilterUserPassword", email, password);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddDomainFilterAlias", domain, alias);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAliasAsync(string domain, string alias)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddDomainFilterAlias", domain, alias);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            return Invoke<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteDomainFilterAlias", domain, alias);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteDomainFilterAliasAsync(string domain, string alias)
        {
            return await InvokeAsync<SolidCP.Providers.Filters.SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteDomainFilterAlias", domain, alias);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SpamExperts : SolidCP.Web.Client.ClientBase<ISpamExperts, SpamExpertsAssemblyClient>, ISpamExperts
    {
        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            return base.Client.AddDomainFilter(domain, password, email, destinations);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAsync(string domain, string password, string email, string[] destinations)
        {
            return await base.Client.AddDomainFilterAsync(domain, password, email, destinations);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            return base.Client.AddEmailFilter(name, domain, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddEmailFilterAsync(string name, string domain, string password)
        {
            return await base.Client.AddEmailFilterAsync(name, domain, password);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult DeleteDomainFilter(string domain)
        {
            return base.Client.DeleteDomainFilter(domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteDomainFilterAsync(string domain)
        {
            return await base.Client.DeleteDomainFilterAsync(domain);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult DeleteEmailFilter(string email)
        {
            return base.Client.DeleteEmailFilter(email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteEmailFilterAsync(string email)
        {
            return await base.Client.DeleteEmailFilterAsync(email);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            return base.Client.SetDomainFilterDestinations(name, destinations);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterDestinationsAsync(string name, string[] destinations)
        {
            return await base.Client.SetDomainFilterDestinationsAsync(name, destinations);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            return base.Client.SetDomainFilterUser(domain, password, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterUserAsync(string domain, string password, string email)
        {
            return await base.Client.SetDomainFilterUserAsync(domain, password, email);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            return base.Client.SetDomainFilterUserPassword(name, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetDomainFilterUserPasswordAsync(string name, string password)
        {
            return await base.Client.SetDomainFilterUserPasswordAsync(name, password);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            return base.Client.SetEmailFilterUserPassword(email, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> SetEmailFilterUserPasswordAsync(string email, string password)
        {
            return await base.Client.SetEmailFilterUserPasswordAsync(email, password);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            return base.Client.AddDomainFilterAlias(domain, alias);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> AddDomainFilterAliasAsync(string domain, string alias)
        {
            return await base.Client.AddDomainFilterAliasAsync(domain, alias);
        }

        public SolidCP.Providers.Filters.SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            return base.Client.DeleteDomainFilterAlias(domain, alias);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Filters.SpamExpertsResult> DeleteDomainFilterAliasAsync(string domain, string alias)
        {
            return await base.Client.DeleteDomainFilterAliasAsync(domain, alias);
        }
    }
}
#endif