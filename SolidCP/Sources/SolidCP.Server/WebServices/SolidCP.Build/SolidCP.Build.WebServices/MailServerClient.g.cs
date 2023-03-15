#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IMailServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IMailServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DomainExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DomainExistsResponse")]
        bool DomainExists(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DomainExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DomainExistsResponse")]
        System.Threading.Tasks.Task<bool> DomainExistsAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetDomainResponse")]
        SolidCP.Providers.Mail.MailDomain GetDomain(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain> GetDomainAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetDomains", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetDomainsResponse")]
        string[] GetDomains();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetDomains", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetDomainsResponse")]
        System.Threading.Tasks.Task<string[]> GetDomainsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateDomainResponse")]
        void CreateDomain(SolidCP.Providers.Mail.MailDomain domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateDomainResponse")]
        System.Threading.Tasks.Task CreateDomainAsync(SolidCP.Providers.Mail.MailDomain domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateDomainResponse")]
        void UpdateDomain(SolidCP.Providers.Mail.MailDomain domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateDomainResponse")]
        System.Threading.Tasks.Task UpdateDomainAsync(SolidCP.Providers.Mail.MailDomain domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteDomainResponse")]
        void DeleteDomain(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteDomain", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteDomainResponse")]
        System.Threading.Tasks.Task DeleteDomainAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DomainAliasExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DomainAliasExistsResponse")]
        bool DomainAliasExists(string domainName, string aliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DomainAliasExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DomainAliasExistsResponse")]
        System.Threading.Tasks.Task<bool> DomainAliasExistsAsync(string domainName, string aliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetDomainAliases", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetDomainAliasesResponse")]
        string[] GetDomainAliases(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetDomainAliases", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetDomainAliasesResponse")]
        System.Threading.Tasks.Task<string[]> GetDomainAliasesAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/AddDomainAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/AddDomainAliasResponse")]
        void AddDomainAlias(string domainName, string aliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/AddDomainAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/AddDomainAliasResponse")]
        System.Threading.Tasks.Task AddDomainAliasAsync(string domainName, string aliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteDomainAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteDomainAliasResponse")]
        void DeleteDomainAlias(string domainName, string aliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteDomainAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteDomainAliasResponse")]
        System.Threading.Tasks.Task DeleteDomainAliasAsync(string domainName, string aliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/AccountExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/AccountExistsResponse")]
        bool AccountExists(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/AccountExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/AccountExistsResponse")]
        System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetAccountsResponse")]
        SolidCP.Providers.Mail.MailAccount[] GetAccounts(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount[]> GetAccountsAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetAccountResponse")]
        SolidCP.Providers.Mail.MailAccount GetAccount(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetAccountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount> GetAccountAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateAccountResponse")]
        void CreateAccount(SolidCP.Providers.Mail.MailAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateAccountResponse")]
        System.Threading.Tasks.Task CreateAccountAsync(SolidCP.Providers.Mail.MailAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateAccountResponse")]
        void UpdateAccount(SolidCP.Providers.Mail.MailAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateAccountResponse")]
        System.Threading.Tasks.Task UpdateAccountAsync(SolidCP.Providers.Mail.MailAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteAccountResponse")]
        void DeleteAccount(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteAccount", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteAccountResponse")]
        System.Threading.Tasks.Task DeleteAccountAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/MailAliasExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/MailAliasExistsResponse")]
        bool MailAliasExists(string mailAliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/MailAliasExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/MailAliasExistsResponse")]
        System.Threading.Tasks.Task<bool> MailAliasExistsAsync(string mailAliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetMailAliases", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetMailAliasesResponse")]
        SolidCP.Providers.Mail.MailAlias[] GetMailAliases(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetMailAliases", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetMailAliasesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias[]> GetMailAliasesAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetMailAliasResponse")]
        SolidCP.Providers.Mail.MailAlias GetMailAlias(string mailAliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetMailAliasResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias> GetMailAliasAsync(string mailAliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateMailAliasResponse")]
        void CreateMailAlias(SolidCP.Providers.Mail.MailAlias mailAlias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateMailAliasResponse")]
        System.Threading.Tasks.Task CreateMailAliasAsync(SolidCP.Providers.Mail.MailAlias mailAlias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateMailAliasResponse")]
        void UpdateMailAlias(SolidCP.Providers.Mail.MailAlias mailAlias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateMailAliasResponse")]
        System.Threading.Tasks.Task UpdateMailAliasAsync(SolidCP.Providers.Mail.MailAlias mailAlias);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteMailAliasResponse")]
        void DeleteMailAlias(string mailAliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteMailAlias", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteMailAliasResponse")]
        System.Threading.Tasks.Task DeleteMailAliasAsync(string mailAliasName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GroupExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GroupExistsResponse")]
        bool GroupExists(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GroupExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GroupExistsResponse")]
        System.Threading.Tasks.Task<bool> GroupExistsAsync(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetGroupsResponse")]
        SolidCP.Providers.Mail.MailGroup[] GetGroups(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup[]> GetGroupsAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetGroupResponse")]
        SolidCP.Providers.Mail.MailGroup GetGroup(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup> GetGroupAsync(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateGroupResponse")]
        void CreateGroup(SolidCP.Providers.Mail.MailGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateGroupResponse")]
        System.Threading.Tasks.Task CreateGroupAsync(SolidCP.Providers.Mail.MailGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateGroupResponse")]
        void UpdateGroup(SolidCP.Providers.Mail.MailGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateGroupResponse")]
        System.Threading.Tasks.Task UpdateGroupAsync(SolidCP.Providers.Mail.MailGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteGroupResponse")]
        void DeleteGroup(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteGroupResponse")]
        System.Threading.Tasks.Task DeleteGroupAsync(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/ListExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/ListExistsResponse")]
        bool ListExists(string listName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/ListExists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/ListExistsResponse")]
        System.Threading.Tasks.Task<bool> ListExistsAsync(string listName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetLists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetListsResponse")]
        SolidCP.Providers.Mail.MailList[] GetLists(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetLists", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetListsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList[]> GetListsAsync(string domainName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetListResponse")]
        SolidCP.Providers.Mail.MailList GetList(string listName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/GetList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/GetListResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList> GetListAsync(string listName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateListResponse")]
        void CreateList(SolidCP.Providers.Mail.MailList list);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/CreateList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/CreateListResponse")]
        System.Threading.Tasks.Task CreateListAsync(SolidCP.Providers.Mail.MailList list);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateListResponse")]
        void UpdateList(SolidCP.Providers.Mail.MailList list);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/UpdateList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/UpdateListResponse")]
        System.Threading.Tasks.Task UpdateListAsync(SolidCP.Providers.Mail.MailList list);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteListResponse")]
        void DeleteList(string listName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IMailServer/DeleteList", ReplyAction = "http://smbsaas/solidcp/server/IMailServer/DeleteListResponse")]
        System.Threading.Tasks.Task DeleteListAsync(string listName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class MailServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IMailServer
    {
        public bool DomainExists(string domainName)
        {
            return Invoke<bool>("SolidCP.Server.MailServer", "DomainExists", domainName);
        }

        public async System.Threading.Tasks.Task<bool> DomainExistsAsync(string domainName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.MailServer", "DomainExists", domainName);
        }

        public SolidCP.Providers.Mail.MailDomain GetDomain(string domainName)
        {
            return Invoke<SolidCP.Providers.Mail.MailDomain>("SolidCP.Server.MailServer", "GetDomain", domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain> GetDomainAsync(string domainName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailDomain>("SolidCP.Server.MailServer", "GetDomain", domainName);
        }

        public string[] GetDomains()
        {
            return Invoke<string[]>("SolidCP.Server.MailServer", "GetDomains");
        }

        public async System.Threading.Tasks.Task<string[]> GetDomainsAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.MailServer", "GetDomains");
        }

        public void CreateDomain(SolidCP.Providers.Mail.MailDomain domain)
        {
            Invoke("SolidCP.Server.MailServer", "CreateDomain", domain);
        }

        public async System.Threading.Tasks.Task CreateDomainAsync(SolidCP.Providers.Mail.MailDomain domain)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "CreateDomain", domain);
        }

        public void UpdateDomain(SolidCP.Providers.Mail.MailDomain domain)
        {
            Invoke("SolidCP.Server.MailServer", "UpdateDomain", domain);
        }

        public async System.Threading.Tasks.Task UpdateDomainAsync(SolidCP.Providers.Mail.MailDomain domain)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "UpdateDomain", domain);
        }

        public void DeleteDomain(string domainName)
        {
            Invoke("SolidCP.Server.MailServer", "DeleteDomain", domainName);
        }

        public async System.Threading.Tasks.Task DeleteDomainAsync(string domainName)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "DeleteDomain", domainName);
        }

        public bool DomainAliasExists(string domainName, string aliasName)
        {
            return Invoke<bool>("SolidCP.Server.MailServer", "DomainAliasExists", domainName, aliasName);
        }

        public async System.Threading.Tasks.Task<bool> DomainAliasExistsAsync(string domainName, string aliasName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.MailServer", "DomainAliasExists", domainName, aliasName);
        }

        public string[] GetDomainAliases(string domainName)
        {
            return Invoke<string[]>("SolidCP.Server.MailServer", "GetDomainAliases", domainName);
        }

        public async System.Threading.Tasks.Task<string[]> GetDomainAliasesAsync(string domainName)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.MailServer", "GetDomainAliases", domainName);
        }

        public void AddDomainAlias(string domainName, string aliasName)
        {
            Invoke("SolidCP.Server.MailServer", "AddDomainAlias", domainName, aliasName);
        }

        public async System.Threading.Tasks.Task AddDomainAliasAsync(string domainName, string aliasName)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "AddDomainAlias", domainName, aliasName);
        }

        public void DeleteDomainAlias(string domainName, string aliasName)
        {
            Invoke("SolidCP.Server.MailServer", "DeleteDomainAlias", domainName, aliasName);
        }

        public async System.Threading.Tasks.Task DeleteDomainAliasAsync(string domainName, string aliasName)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "DeleteDomainAlias", domainName, aliasName);
        }

        public bool AccountExists(string accountName)
        {
            return Invoke<bool>("SolidCP.Server.MailServer", "AccountExists", accountName);
        }

        public async System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.MailServer", "AccountExists", accountName);
        }

        public SolidCP.Providers.Mail.MailAccount[] GetAccounts(string domainName)
        {
            return Invoke<SolidCP.Providers.Mail.MailAccount[]>("SolidCP.Server.MailServer", "GetAccounts", domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount[]> GetAccountsAsync(string domainName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAccount[]>("SolidCP.Server.MailServer", "GetAccounts", domainName);
        }

        public SolidCP.Providers.Mail.MailAccount GetAccount(string accountName)
        {
            return Invoke<SolidCP.Providers.Mail.MailAccount>("SolidCP.Server.MailServer", "GetAccount", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount> GetAccountAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAccount>("SolidCP.Server.MailServer", "GetAccount", accountName);
        }

        public void CreateAccount(SolidCP.Providers.Mail.MailAccount account)
        {
            Invoke("SolidCP.Server.MailServer", "CreateAccount", account);
        }

        public async System.Threading.Tasks.Task CreateAccountAsync(SolidCP.Providers.Mail.MailAccount account)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "CreateAccount", account);
        }

        public void UpdateAccount(SolidCP.Providers.Mail.MailAccount account)
        {
            Invoke("SolidCP.Server.MailServer", "UpdateAccount", account);
        }

        public async System.Threading.Tasks.Task UpdateAccountAsync(SolidCP.Providers.Mail.MailAccount account)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "UpdateAccount", account);
        }

        public void DeleteAccount(string accountName)
        {
            Invoke("SolidCP.Server.MailServer", "DeleteAccount", accountName);
        }

        public async System.Threading.Tasks.Task DeleteAccountAsync(string accountName)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "DeleteAccount", accountName);
        }

        public bool MailAliasExists(string mailAliasName)
        {
            return Invoke<bool>("SolidCP.Server.MailServer", "MailAliasExists", mailAliasName);
        }

        public async System.Threading.Tasks.Task<bool> MailAliasExistsAsync(string mailAliasName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.MailServer", "MailAliasExists", mailAliasName);
        }

        public SolidCP.Providers.Mail.MailAlias[] GetMailAliases(string domainName)
        {
            return Invoke<SolidCP.Providers.Mail.MailAlias[]>("SolidCP.Server.MailServer", "GetMailAliases", domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias[]> GetMailAliasesAsync(string domainName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAlias[]>("SolidCP.Server.MailServer", "GetMailAliases", domainName);
        }

        public SolidCP.Providers.Mail.MailAlias GetMailAlias(string mailAliasName)
        {
            return Invoke<SolidCP.Providers.Mail.MailAlias>("SolidCP.Server.MailServer", "GetMailAlias", mailAliasName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias> GetMailAliasAsync(string mailAliasName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAlias>("SolidCP.Server.MailServer", "GetMailAlias", mailAliasName);
        }

        public void CreateMailAlias(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            Invoke("SolidCP.Server.MailServer", "CreateMailAlias", mailAlias);
        }

        public async System.Threading.Tasks.Task CreateMailAliasAsync(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "CreateMailAlias", mailAlias);
        }

        public void UpdateMailAlias(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            Invoke("SolidCP.Server.MailServer", "UpdateMailAlias", mailAlias);
        }

        public async System.Threading.Tasks.Task UpdateMailAliasAsync(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "UpdateMailAlias", mailAlias);
        }

        public void DeleteMailAlias(string mailAliasName)
        {
            Invoke("SolidCP.Server.MailServer", "DeleteMailAlias", mailAliasName);
        }

        public async System.Threading.Tasks.Task DeleteMailAliasAsync(string mailAliasName)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "DeleteMailAlias", mailAliasName);
        }

        public bool GroupExists(string groupName)
        {
            return Invoke<bool>("SolidCP.Server.MailServer", "GroupExists", groupName);
        }

        public async System.Threading.Tasks.Task<bool> GroupExistsAsync(string groupName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.MailServer", "GroupExists", groupName);
        }

        public SolidCP.Providers.Mail.MailGroup[] GetGroups(string domainName)
        {
            return Invoke<SolidCP.Providers.Mail.MailGroup[]>("SolidCP.Server.MailServer", "GetGroups", domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup[]> GetGroupsAsync(string domainName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailGroup[]>("SolidCP.Server.MailServer", "GetGroups", domainName);
        }

        public SolidCP.Providers.Mail.MailGroup GetGroup(string groupName)
        {
            return Invoke<SolidCP.Providers.Mail.MailGroup>("SolidCP.Server.MailServer", "GetGroup", groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup> GetGroupAsync(string groupName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailGroup>("SolidCP.Server.MailServer", "GetGroup", groupName);
        }

        public void CreateGroup(SolidCP.Providers.Mail.MailGroup group)
        {
            Invoke("SolidCP.Server.MailServer", "CreateGroup", group);
        }

        public async System.Threading.Tasks.Task CreateGroupAsync(SolidCP.Providers.Mail.MailGroup group)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "CreateGroup", group);
        }

        public void UpdateGroup(SolidCP.Providers.Mail.MailGroup group)
        {
            Invoke("SolidCP.Server.MailServer", "UpdateGroup", group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(SolidCP.Providers.Mail.MailGroup group)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "UpdateGroup", group);
        }

        public void DeleteGroup(string groupName)
        {
            Invoke("SolidCP.Server.MailServer", "DeleteGroup", groupName);
        }

        public async System.Threading.Tasks.Task DeleteGroupAsync(string groupName)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "DeleteGroup", groupName);
        }

        public bool ListExists(string listName)
        {
            return Invoke<bool>("SolidCP.Server.MailServer", "ListExists", listName);
        }

        public async System.Threading.Tasks.Task<bool> ListExistsAsync(string listName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.MailServer", "ListExists", listName);
        }

        public SolidCP.Providers.Mail.MailList[] GetLists(string domainName)
        {
            return Invoke<SolidCP.Providers.Mail.MailList[]>("SolidCP.Server.MailServer", "GetLists", domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList[]> GetListsAsync(string domainName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailList[]>("SolidCP.Server.MailServer", "GetLists", domainName);
        }

        public SolidCP.Providers.Mail.MailList GetList(string listName)
        {
            return Invoke<SolidCP.Providers.Mail.MailList>("SolidCP.Server.MailServer", "GetList", listName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList> GetListAsync(string listName)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailList>("SolidCP.Server.MailServer", "GetList", listName);
        }

        public void CreateList(SolidCP.Providers.Mail.MailList list)
        {
            Invoke("SolidCP.Server.MailServer", "CreateList", list);
        }

        public async System.Threading.Tasks.Task CreateListAsync(SolidCP.Providers.Mail.MailList list)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "CreateList", list);
        }

        public void UpdateList(SolidCP.Providers.Mail.MailList list)
        {
            Invoke("SolidCP.Server.MailServer", "UpdateList", list);
        }

        public async System.Threading.Tasks.Task UpdateListAsync(SolidCP.Providers.Mail.MailList list)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "UpdateList", list);
        }

        public void DeleteList(string listName)
        {
            Invoke("SolidCP.Server.MailServer", "DeleteList", listName);
        }

        public async System.Threading.Tasks.Task DeleteListAsync(string listName)
        {
            await InvokeAsync("SolidCP.Server.MailServer", "DeleteList", listName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class MailServer : SolidCP.Web.Client.ClientBase<IMailServer, MailServerAssemblyClient>, IMailServer
    {
        public bool DomainExists(string domainName)
        {
            return base.Client.DomainExists(domainName);
        }

        public async System.Threading.Tasks.Task<bool> DomainExistsAsync(string domainName)
        {
            return await base.Client.DomainExistsAsync(domainName);
        }

        public SolidCP.Providers.Mail.MailDomain GetDomain(string domainName)
        {
            return base.Client.GetDomain(domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain> GetDomainAsync(string domainName)
        {
            return await base.Client.GetDomainAsync(domainName);
        }

        public string[] GetDomains()
        {
            return base.Client.GetDomains();
        }

        public async System.Threading.Tasks.Task<string[]> GetDomainsAsync()
        {
            return await base.Client.GetDomainsAsync();
        }

        public void CreateDomain(SolidCP.Providers.Mail.MailDomain domain)
        {
            base.Client.CreateDomain(domain);
        }

        public async System.Threading.Tasks.Task CreateDomainAsync(SolidCP.Providers.Mail.MailDomain domain)
        {
            await base.Client.CreateDomainAsync(domain);
        }

        public void UpdateDomain(SolidCP.Providers.Mail.MailDomain domain)
        {
            base.Client.UpdateDomain(domain);
        }

        public async System.Threading.Tasks.Task UpdateDomainAsync(SolidCP.Providers.Mail.MailDomain domain)
        {
            await base.Client.UpdateDomainAsync(domain);
        }

        public void DeleteDomain(string domainName)
        {
            base.Client.DeleteDomain(domainName);
        }

        public async System.Threading.Tasks.Task DeleteDomainAsync(string domainName)
        {
            await base.Client.DeleteDomainAsync(domainName);
        }

        public bool DomainAliasExists(string domainName, string aliasName)
        {
            return base.Client.DomainAliasExists(domainName, aliasName);
        }

        public async System.Threading.Tasks.Task<bool> DomainAliasExistsAsync(string domainName, string aliasName)
        {
            return await base.Client.DomainAliasExistsAsync(domainName, aliasName);
        }

        public string[] GetDomainAliases(string domainName)
        {
            return base.Client.GetDomainAliases(domainName);
        }

        public async System.Threading.Tasks.Task<string[]> GetDomainAliasesAsync(string domainName)
        {
            return await base.Client.GetDomainAliasesAsync(domainName);
        }

        public void AddDomainAlias(string domainName, string aliasName)
        {
            base.Client.AddDomainAlias(domainName, aliasName);
        }

        public async System.Threading.Tasks.Task AddDomainAliasAsync(string domainName, string aliasName)
        {
            await base.Client.AddDomainAliasAsync(domainName, aliasName);
        }

        public void DeleteDomainAlias(string domainName, string aliasName)
        {
            base.Client.DeleteDomainAlias(domainName, aliasName);
        }

        public async System.Threading.Tasks.Task DeleteDomainAliasAsync(string domainName, string aliasName)
        {
            await base.Client.DeleteDomainAliasAsync(domainName, aliasName);
        }

        public bool AccountExists(string accountName)
        {
            return base.Client.AccountExists(accountName);
        }

        public async System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName)
        {
            return await base.Client.AccountExistsAsync(accountName);
        }

        public SolidCP.Providers.Mail.MailAccount[] GetAccounts(string domainName)
        {
            return base.Client.GetAccounts(domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount[]> GetAccountsAsync(string domainName)
        {
            return await base.Client.GetAccountsAsync(domainName);
        }

        public SolidCP.Providers.Mail.MailAccount GetAccount(string accountName)
        {
            return base.Client.GetAccount(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount> GetAccountAsync(string accountName)
        {
            return await base.Client.GetAccountAsync(accountName);
        }

        public void CreateAccount(SolidCP.Providers.Mail.MailAccount account)
        {
            base.Client.CreateAccount(account);
        }

        public async System.Threading.Tasks.Task CreateAccountAsync(SolidCP.Providers.Mail.MailAccount account)
        {
            await base.Client.CreateAccountAsync(account);
        }

        public void UpdateAccount(SolidCP.Providers.Mail.MailAccount account)
        {
            base.Client.UpdateAccount(account);
        }

        public async System.Threading.Tasks.Task UpdateAccountAsync(SolidCP.Providers.Mail.MailAccount account)
        {
            await base.Client.UpdateAccountAsync(account);
        }

        public void DeleteAccount(string accountName)
        {
            base.Client.DeleteAccount(accountName);
        }

        public async System.Threading.Tasks.Task DeleteAccountAsync(string accountName)
        {
            await base.Client.DeleteAccountAsync(accountName);
        }

        public bool MailAliasExists(string mailAliasName)
        {
            return base.Client.MailAliasExists(mailAliasName);
        }

        public async System.Threading.Tasks.Task<bool> MailAliasExistsAsync(string mailAliasName)
        {
            return await base.Client.MailAliasExistsAsync(mailAliasName);
        }

        public SolidCP.Providers.Mail.MailAlias[] GetMailAliases(string domainName)
        {
            return base.Client.GetMailAliases(domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias[]> GetMailAliasesAsync(string domainName)
        {
            return await base.Client.GetMailAliasesAsync(domainName);
        }

        public SolidCP.Providers.Mail.MailAlias GetMailAlias(string mailAliasName)
        {
            return base.Client.GetMailAlias(mailAliasName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias> GetMailAliasAsync(string mailAliasName)
        {
            return await base.Client.GetMailAliasAsync(mailAliasName);
        }

        public void CreateMailAlias(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            base.Client.CreateMailAlias(mailAlias);
        }

        public async System.Threading.Tasks.Task CreateMailAliasAsync(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            await base.Client.CreateMailAliasAsync(mailAlias);
        }

        public void UpdateMailAlias(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            base.Client.UpdateMailAlias(mailAlias);
        }

        public async System.Threading.Tasks.Task UpdateMailAliasAsync(SolidCP.Providers.Mail.MailAlias mailAlias)
        {
            await base.Client.UpdateMailAliasAsync(mailAlias);
        }

        public void DeleteMailAlias(string mailAliasName)
        {
            base.Client.DeleteMailAlias(mailAliasName);
        }

        public async System.Threading.Tasks.Task DeleteMailAliasAsync(string mailAliasName)
        {
            await base.Client.DeleteMailAliasAsync(mailAliasName);
        }

        public bool GroupExists(string groupName)
        {
            return base.Client.GroupExists(groupName);
        }

        public async System.Threading.Tasks.Task<bool> GroupExistsAsync(string groupName)
        {
            return await base.Client.GroupExistsAsync(groupName);
        }

        public SolidCP.Providers.Mail.MailGroup[] GetGroups(string domainName)
        {
            return base.Client.GetGroups(domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup[]> GetGroupsAsync(string domainName)
        {
            return await base.Client.GetGroupsAsync(domainName);
        }

        public SolidCP.Providers.Mail.MailGroup GetGroup(string groupName)
        {
            return base.Client.GetGroup(groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup> GetGroupAsync(string groupName)
        {
            return await base.Client.GetGroupAsync(groupName);
        }

        public void CreateGroup(SolidCP.Providers.Mail.MailGroup group)
        {
            base.Client.CreateGroup(group);
        }

        public async System.Threading.Tasks.Task CreateGroupAsync(SolidCP.Providers.Mail.MailGroup group)
        {
            await base.Client.CreateGroupAsync(group);
        }

        public void UpdateGroup(SolidCP.Providers.Mail.MailGroup group)
        {
            base.Client.UpdateGroup(group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(SolidCP.Providers.Mail.MailGroup group)
        {
            await base.Client.UpdateGroupAsync(group);
        }

        public void DeleteGroup(string groupName)
        {
            base.Client.DeleteGroup(groupName);
        }

        public async System.Threading.Tasks.Task DeleteGroupAsync(string groupName)
        {
            await base.Client.DeleteGroupAsync(groupName);
        }

        public bool ListExists(string listName)
        {
            return base.Client.ListExists(listName);
        }

        public async System.Threading.Tasks.Task<bool> ListExistsAsync(string listName)
        {
            return await base.Client.ListExistsAsync(listName);
        }

        public SolidCP.Providers.Mail.MailList[] GetLists(string domainName)
        {
            return base.Client.GetLists(domainName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList[]> GetListsAsync(string domainName)
        {
            return await base.Client.GetListsAsync(domainName);
        }

        public SolidCP.Providers.Mail.MailList GetList(string listName)
        {
            return base.Client.GetList(listName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList> GetListAsync(string listName)
        {
            return await base.Client.GetListAsync(listName);
        }

        public void CreateList(SolidCP.Providers.Mail.MailList list)
        {
            base.Client.CreateList(list);
        }

        public async System.Threading.Tasks.Task CreateListAsync(SolidCP.Providers.Mail.MailList list)
        {
            await base.Client.CreateListAsync(list);
        }

        public void UpdateList(SolidCP.Providers.Mail.MailList list)
        {
            base.Client.UpdateList(list);
        }

        public async System.Threading.Tasks.Task UpdateListAsync(SolidCP.Providers.Mail.MailList list)
        {
            await base.Client.UpdateListAsync(list);
        }

        public void DeleteList(string listName)
        {
            base.Client.DeleteList(listName);
        }

        public async System.Threading.Tasks.Task DeleteListAsync(string listName)
        {
            await base.Client.DeleteListAsync(listName);
        }
    }
}
#endif