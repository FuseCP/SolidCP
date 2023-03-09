#if Client
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Filters;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ISpamExperts", Namespace = "http://smbsaas/solidcp/server/")]
    public interface ISpamExperts
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterResponse")]
        SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> AddDomainFilterAsync(string domain, string password, string email, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilterResponse")]
        SpamExpertsResult AddEmailFilter(string name, string domain, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddEmailFilterResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> AddEmailFilterAsync(string name, string domain, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterResponse")]
        SpamExpertsResult DeleteDomainFilter(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> DeleteDomainFilterAsync(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilterResponse")]
        SpamExpertsResult DeleteEmailFilter(string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilter", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteEmailFilterResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> DeleteEmailFilterAsync(string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinations", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinationsResponse")]
        SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinations", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterDestinationsResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterDestinationsAsync(string name, string[] destinations);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUser", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserResponse")]
        SpamExpertsResult SetDomainFilterUser(string domain, string password, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUser", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterUserAsync(string domain, string password, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPasswordResponse")]
        SpamExpertsResult SetDomainFilterUserPassword(string name, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetDomainFilterUserPasswordResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterUserPasswordAsync(string name, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPasswordResponse")]
        SpamExpertsResult SetEmailFilterUserPassword(string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/SetEmailFilterUserPasswordResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> SetEmailFilterUserPasswordAsync(string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAliasResponse")]
        SpamExpertsResult AddDomainFilterAlias(string domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/AddDomainFilterAliasResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> AddDomainFilterAliasAsync(string domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAliasResponse")]
        SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAlias", ReplyAction = "http://smbsaas/solidcp/server/ISpamExperts/DeleteDomainFilterAliasResponse")]
        System.Threading.Tasks.Task<SpamExpertsResult> DeleteDomainFilterAliasAsync(string domain, string alias);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SpamExpertsAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ISpamExperts
    {
        public SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "AddDomainFilter", domain, password, email, destinations);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> AddDomainFilterAsync(string domain, string password, string email, string[] destinations)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddDomainFilter", domain, password, email, destinations);
        }

        public SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "AddEmailFilter", name, domain, password);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> AddEmailFilterAsync(string name, string domain, string password)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddEmailFilter", name, domain, password);
        }

        public SpamExpertsResult DeleteDomainFilter(string domain)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "DeleteDomainFilter", domain);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> DeleteDomainFilterAsync(string domain)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteDomainFilter", domain);
        }

        public SpamExpertsResult DeleteEmailFilter(string email)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "DeleteEmailFilter", email);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> DeleteEmailFilterAsync(string email)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteEmailFilter", email);
        }

        public SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "SetDomainFilterDestinations", name, destinations);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterDestinationsAsync(string name, string[] destinations)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterDestinations", name, destinations);
        }

        public SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "SetDomainFilterUser", domain, password, email);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterUserAsync(string domain, string password, string email)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterUser", domain, password, email);
        }

        public SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "SetDomainFilterUserPassword", name, password);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterUserPasswordAsync(string name, string password)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetDomainFilterUserPassword", name, password);
        }

        public SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "SetEmailFilterUserPassword", email, password);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetEmailFilterUserPasswordAsync(string email, string password)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "SetEmailFilterUserPassword", email, password);
        }

        public SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "AddDomainFilterAlias", domain, alias);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> AddDomainFilterAliasAsync(string domain, string alias)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "AddDomainFilterAlias", domain, alias);
        }

        public SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            return (SpamExpertsResult)Invoke("SolidCP.Server.SpamExperts", "DeleteDomainFilterAlias", domain, alias);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> DeleteDomainFilterAliasAsync(string domain, string alias)
        {
            return await InvokeAsync<SpamExpertsResult>("SolidCP.Server.SpamExperts", "DeleteDomainFilterAlias", domain, alias);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SpamExperts : SolidCP.Web.Client.ClientBase<ISpamExperts, SpamExpertsAssemblyClient>, ISpamExperts
    {
        public SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            return base.Client.AddDomainFilter(domain, password, email, destinations);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> AddDomainFilterAsync(string domain, string password, string email, string[] destinations)
        {
            return await base.Client.AddDomainFilterAsync(domain, password, email, destinations);
        }

        public SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            return base.Client.AddEmailFilter(name, domain, password);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> AddEmailFilterAsync(string name, string domain, string password)
        {
            return await base.Client.AddEmailFilterAsync(name, domain, password);
        }

        public SpamExpertsResult DeleteDomainFilter(string domain)
        {
            return base.Client.DeleteDomainFilter(domain);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> DeleteDomainFilterAsync(string domain)
        {
            return await base.Client.DeleteDomainFilterAsync(domain);
        }

        public SpamExpertsResult DeleteEmailFilter(string email)
        {
            return base.Client.DeleteEmailFilter(email);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> DeleteEmailFilterAsync(string email)
        {
            return await base.Client.DeleteEmailFilterAsync(email);
        }

        public SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            return base.Client.SetDomainFilterDestinations(name, destinations);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterDestinationsAsync(string name, string[] destinations)
        {
            return await base.Client.SetDomainFilterDestinationsAsync(name, destinations);
        }

        public SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            return base.Client.SetDomainFilterUser(domain, password, email);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterUserAsync(string domain, string password, string email)
        {
            return await base.Client.SetDomainFilterUserAsync(domain, password, email);
        }

        public SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            return base.Client.SetDomainFilterUserPassword(name, password);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetDomainFilterUserPasswordAsync(string name, string password)
        {
            return await base.Client.SetDomainFilterUserPasswordAsync(name, password);
        }

        public SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            return base.Client.SetEmailFilterUserPassword(email, password);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> SetEmailFilterUserPasswordAsync(string email, string password)
        {
            return await base.Client.SetEmailFilterUserPasswordAsync(email, password);
        }

        public SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            return base.Client.AddDomainFilterAlias(domain, alias);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> AddDomainFilterAliasAsync(string domain, string alias)
        {
            return await base.Client.AddDomainFilterAliasAsync(domain, alias);
        }

        public SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            return base.Client.DeleteDomainFilterAlias(domain, alias);
        }

        public async System.Threading.Tasks.Task<SpamExpertsResult> DeleteDomainFilterAliasAsync(string domain, string alias)
        {
            return await base.Client.DeleteDomainFilterAliasAsync(domain, alias);
        }
    }
}
#endif