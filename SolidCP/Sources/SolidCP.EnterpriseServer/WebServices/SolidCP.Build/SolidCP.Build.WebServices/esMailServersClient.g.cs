#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesMailServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesMailServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailAccountsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailAccountsPagedResponse")]
        System.Data.DataSet GetRawMailAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailAccountsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailAccountsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailAccountsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccountsResponse")]
        SolidCP.Providers.Mail.MailAccount[] /*List*/ GetMailAccounts(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount[]> GetMailAccountsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccountResponse")]
        SolidCP.Providers.Mail.MailAccount GetMailAccount(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailAccountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount> GetMailAccountAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailAccountResponse")]
        int AddMailAccount(SolidCP.Providers.Mail.MailAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailAccountResponse")]
        System.Threading.Tasks.Task<int> AddMailAccountAsync(SolidCP.Providers.Mail.MailAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailAccountResponse")]
        int UpdateMailAccount(SolidCP.Providers.Mail.MailAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailAccountResponse")]
        System.Threading.Tasks.Task<int> UpdateMailAccountAsync(SolidCP.Providers.Mail.MailAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailAccountResponse")]
        int DeleteMailAccount(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailAccountResponse")]
        System.Threading.Tasks.Task<int> DeleteMailAccountAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailForwardingsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailForwardingsPagedResponse")]
        System.Data.DataSet GetRawMailForwardingsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailForwardingsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailForwardingsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailForwardingsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwardings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwardingsResponse")]
        SolidCP.Providers.Mail.MailAlias[] /*List*/ GetMailForwardings(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwardings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwardingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias[]> GetMailForwardingsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwardingResponse")]
        SolidCP.Providers.Mail.MailAlias GetMailForwarding(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailForwardingResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias> GetMailForwardingAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailForwardingResponse")]
        int AddMailForwarding(SolidCP.Providers.Mail.MailAlias item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailForwardingResponse")]
        System.Threading.Tasks.Task<int> AddMailForwardingAsync(SolidCP.Providers.Mail.MailAlias item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailForwardingResponse")]
        int UpdateMailForwarding(SolidCP.Providers.Mail.MailAlias item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailForwardingResponse")]
        System.Threading.Tasks.Task<int> UpdateMailForwardingAsync(SolidCP.Providers.Mail.MailAlias item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailForwardingResponse")]
        int DeleteMailForwarding(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailForwarding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailForwardingResponse")]
        System.Threading.Tasks.Task<int> DeleteMailForwardingAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailGroupsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailGroupsPagedResponse")]
        System.Data.DataSet GetRawMailGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailGroupsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailGroupsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailGroupsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroupsResponse")]
        SolidCP.Providers.Mail.MailGroup[] /*List*/ GetMailGroups(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup[]> GetMailGroupsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroupResponse")]
        SolidCP.Providers.Mail.MailGroup GetMailGroup(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup> GetMailGroupAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailGroupResponse")]
        int AddMailGroup(SolidCP.Providers.Mail.MailGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailGroupResponse")]
        System.Threading.Tasks.Task<int> AddMailGroupAsync(SolidCP.Providers.Mail.MailGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailGroupResponse")]
        int UpdateMailGroup(SolidCP.Providers.Mail.MailGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailGroupResponse")]
        System.Threading.Tasks.Task<int> UpdateMailGroupAsync(SolidCP.Providers.Mail.MailGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailGroupResponse")]
        int DeleteMailGroup(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailGroupResponse")]
        System.Threading.Tasks.Task<int> DeleteMailGroupAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailListsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailListsPagedResponse")]
        System.Data.DataSet GetRawMailListsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailListsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailListsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailListsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailLists", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailListsResponse")]
        SolidCP.Providers.Mail.MailList[] /*List*/ GetMailLists(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailLists", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailListsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList[]> GetMailListsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailListResponse")]
        SolidCP.Providers.Mail.MailList GetMailList(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailListResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList> GetMailListAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailListResponse")]
        int AddMailList(SolidCP.Providers.Mail.MailList item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailListResponse")]
        System.Threading.Tasks.Task<int> AddMailListAsync(SolidCP.Providers.Mail.MailList item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailListResponse")]
        int UpdateMailList(SolidCP.Providers.Mail.MailList item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailListResponse")]
        System.Threading.Tasks.Task<int> UpdateMailListAsync(SolidCP.Providers.Mail.MailList item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailListResponse")]
        int DeleteMailList(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailListResponse")]
        System.Threading.Tasks.Task<int> DeleteMailListAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailDomainsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailDomainsPagedResponse")]
        System.Data.DataSet GetRawMailDomainsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailDomainsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetRawMailDomainsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailDomainsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainsResponse")]
        SolidCP.Providers.Mail.MailDomain[] /*List*/ GetMailDomains(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain[]> GetMailDomainsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainPointers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainPointersResponse")]
        SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetMailDomainPointers(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainPointers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainPointersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetMailDomainPointersAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainResponse")]
        SolidCP.Providers.Mail.MailDomain GetMailDomain(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/GetMailDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain> GetMailDomainAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomainResponse")]
        int AddMailDomain(SolidCP.Providers.Mail.MailDomain item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomainResponse")]
        System.Threading.Tasks.Task<int> AddMailDomainAsync(SolidCP.Providers.Mail.MailDomain item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailDomainResponse")]
        int UpdateMailDomain(SolidCP.Providers.Mail.MailDomain item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/UpdateMailDomainResponse")]
        System.Threading.Tasks.Task<int> UpdateMailDomainAsync(SolidCP.Providers.Mail.MailDomain item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomainResponse")]
        int DeleteMailDomain(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomainResponse")]
        System.Threading.Tasks.Task<int> DeleteMailDomainAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomainPointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomainPointerResponse")]
        int AddMailDomainPointer(int itemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomainPointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/AddMailDomainPointerResponse")]
        System.Threading.Tasks.Task<int> AddMailDomainPointerAsync(int itemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomainPointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomainPointerResponse")]
        int DeleteMailDomainPointer(int itemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomainPointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesMailServers/DeleteMailDomainPointerResponse")]
        System.Threading.Tasks.Task<int> DeleteMailDomainPointerAsync(int itemId, int domainId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esMailServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesMailServers
    {
        public System.Data.DataSet GetRawMailAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailAccountsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailAccountsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailAccountsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailAccount[] /*List*/ GetMailAccounts(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Mail.MailAccount[], SolidCP.Providers.Mail.MailAccount>("SolidCP.EnterpriseServer.esMailServers", "GetMailAccounts", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount[]> GetMailAccountsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAccount[], SolidCP.Providers.Mail.MailAccount>("SolidCP.EnterpriseServer.esMailServers", "GetMailAccounts", packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailAccount GetMailAccount(int itemId)
        {
            return Invoke<SolidCP.Providers.Mail.MailAccount>("SolidCP.EnterpriseServer.esMailServers", "GetMailAccount", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount> GetMailAccountAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAccount>("SolidCP.EnterpriseServer.esMailServers", "GetMailAccount", itemId);
        }

        public int AddMailAccount(SolidCP.Providers.Mail.MailAccount item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailAccount", item);
        }

        public async System.Threading.Tasks.Task<int> AddMailAccountAsync(SolidCP.Providers.Mail.MailAccount item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailAccount", item);
        }

        public int UpdateMailAccount(SolidCP.Providers.Mail.MailAccount item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailAccount", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailAccountAsync(SolidCP.Providers.Mail.MailAccount item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailAccount", item);
        }

        public int DeleteMailAccount(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailAccount", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailAccountAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailAccount", itemId);
        }

        public System.Data.DataSet GetRawMailForwardingsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailForwardingsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailForwardingsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailForwardingsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailAlias[] /*List*/ GetMailForwardings(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Mail.MailAlias[], SolidCP.Providers.Mail.MailAlias>("SolidCP.EnterpriseServer.esMailServers", "GetMailForwardings", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias[]> GetMailForwardingsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAlias[], SolidCP.Providers.Mail.MailAlias>("SolidCP.EnterpriseServer.esMailServers", "GetMailForwardings", packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailAlias GetMailForwarding(int itemId)
        {
            return Invoke<SolidCP.Providers.Mail.MailAlias>("SolidCP.EnterpriseServer.esMailServers", "GetMailForwarding", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias> GetMailForwardingAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailAlias>("SolidCP.EnterpriseServer.esMailServers", "GetMailForwarding", itemId);
        }

        public int AddMailForwarding(SolidCP.Providers.Mail.MailAlias item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailForwarding", item);
        }

        public async System.Threading.Tasks.Task<int> AddMailForwardingAsync(SolidCP.Providers.Mail.MailAlias item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailForwarding", item);
        }

        public int UpdateMailForwarding(SolidCP.Providers.Mail.MailAlias item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailForwarding", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailForwardingAsync(SolidCP.Providers.Mail.MailAlias item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailForwarding", item);
        }

        public int DeleteMailForwarding(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailForwarding", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailForwardingAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailForwarding", itemId);
        }

        public System.Data.DataSet GetRawMailGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailGroupsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailGroupsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailGroupsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailGroup[] /*List*/ GetMailGroups(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Mail.MailGroup[], SolidCP.Providers.Mail.MailGroup>("SolidCP.EnterpriseServer.esMailServers", "GetMailGroups", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup[]> GetMailGroupsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailGroup[], SolidCP.Providers.Mail.MailGroup>("SolidCP.EnterpriseServer.esMailServers", "GetMailGroups", packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailGroup GetMailGroup(int itemId)
        {
            return Invoke<SolidCP.Providers.Mail.MailGroup>("SolidCP.EnterpriseServer.esMailServers", "GetMailGroup", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup> GetMailGroupAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailGroup>("SolidCP.EnterpriseServer.esMailServers", "GetMailGroup", itemId);
        }

        public int AddMailGroup(SolidCP.Providers.Mail.MailGroup item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailGroup", item);
        }

        public async System.Threading.Tasks.Task<int> AddMailGroupAsync(SolidCP.Providers.Mail.MailGroup item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailGroup", item);
        }

        public int UpdateMailGroup(SolidCP.Providers.Mail.MailGroup item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailGroup", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailGroupAsync(SolidCP.Providers.Mail.MailGroup item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailGroup", item);
        }

        public int DeleteMailGroup(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailGroup", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailGroupAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailGroup", itemId);
        }

        public System.Data.DataSet GetRawMailListsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailListsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailListsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailListsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailList[] /*List*/ GetMailLists(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Mail.MailList[], SolidCP.Providers.Mail.MailList>("SolidCP.EnterpriseServer.esMailServers", "GetMailLists", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList[]> GetMailListsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailList[], SolidCP.Providers.Mail.MailList>("SolidCP.EnterpriseServer.esMailServers", "GetMailLists", packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailList GetMailList(int itemId)
        {
            return Invoke<SolidCP.Providers.Mail.MailList>("SolidCP.EnterpriseServer.esMailServers", "GetMailList", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList> GetMailListAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailList>("SolidCP.EnterpriseServer.esMailServers", "GetMailList", itemId);
        }

        public int AddMailList(SolidCP.Providers.Mail.MailList item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailList", item);
        }

        public async System.Threading.Tasks.Task<int> AddMailListAsync(SolidCP.Providers.Mail.MailList item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailList", item);
        }

        public int UpdateMailList(SolidCP.Providers.Mail.MailList item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailList", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailListAsync(SolidCP.Providers.Mail.MailList item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailList", item);
        }

        public int DeleteMailList(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailList", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailListAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailList", itemId);
        }

        public System.Data.DataSet GetRawMailDomainsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailDomainsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailDomainsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esMailServers", "GetRawMailDomainsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailDomain[] /*List*/ GetMailDomains(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Mail.MailDomain[], SolidCP.Providers.Mail.MailDomain>("SolidCP.EnterpriseServer.esMailServers", "GetMailDomains", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain[]> GetMailDomainsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailDomain[], SolidCP.Providers.Mail.MailDomain>("SolidCP.EnterpriseServer.esMailServers", "GetMailDomains", packageId, recursive);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetMailDomainPointers(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esMailServers", "GetMailDomainPointers", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetMailDomainPointersAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esMailServers", "GetMailDomainPointers", itemId);
        }

        public SolidCP.Providers.Mail.MailDomain GetMailDomain(int itemId)
        {
            return Invoke<SolidCP.Providers.Mail.MailDomain>("SolidCP.EnterpriseServer.esMailServers", "GetMailDomain", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain> GetMailDomainAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Mail.MailDomain>("SolidCP.EnterpriseServer.esMailServers", "GetMailDomain", itemId);
        }

        public int AddMailDomain(SolidCP.Providers.Mail.MailDomain item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailDomain", item);
        }

        public async System.Threading.Tasks.Task<int> AddMailDomainAsync(SolidCP.Providers.Mail.MailDomain item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailDomain", item);
        }

        public int UpdateMailDomain(SolidCP.Providers.Mail.MailDomain item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailDomain", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailDomainAsync(SolidCP.Providers.Mail.MailDomain item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "UpdateMailDomain", item);
        }

        public int DeleteMailDomain(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailDomain", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailDomainAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailDomain", itemId);
        }

        public int AddMailDomainPointer(int itemId, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailDomainPointer", itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> AddMailDomainPointerAsync(int itemId, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "AddMailDomainPointer", itemId, domainId);
        }

        public int DeleteMailDomainPointer(int itemId, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailDomainPointer", itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailDomainPointerAsync(int itemId, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esMailServers", "DeleteMailDomainPointer", itemId, domainId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esMailServers : SolidCP.Web.Client.ClientBase<IesMailServers, esMailServersAssemblyClient>, IesMailServers
    {
        public System.Data.DataSet GetRawMailAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawMailAccountsPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailAccountsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawMailAccountsPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailAccount[] /*List*/ GetMailAccounts(int packageId, bool recursive)
        {
            return base.Client.GetMailAccounts(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount[]> GetMailAccountsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetMailAccountsAsync(packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailAccount GetMailAccount(int itemId)
        {
            return base.Client.GetMailAccount(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAccount> GetMailAccountAsync(int itemId)
        {
            return await base.Client.GetMailAccountAsync(itemId);
        }

        public int AddMailAccount(SolidCP.Providers.Mail.MailAccount item)
        {
            return base.Client.AddMailAccount(item);
        }

        public async System.Threading.Tasks.Task<int> AddMailAccountAsync(SolidCP.Providers.Mail.MailAccount item)
        {
            return await base.Client.AddMailAccountAsync(item);
        }

        public int UpdateMailAccount(SolidCP.Providers.Mail.MailAccount item)
        {
            return base.Client.UpdateMailAccount(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailAccountAsync(SolidCP.Providers.Mail.MailAccount item)
        {
            return await base.Client.UpdateMailAccountAsync(item);
        }

        public int DeleteMailAccount(int itemId)
        {
            return base.Client.DeleteMailAccount(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailAccountAsync(int itemId)
        {
            return await base.Client.DeleteMailAccountAsync(itemId);
        }

        public System.Data.DataSet GetRawMailForwardingsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawMailForwardingsPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailForwardingsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawMailForwardingsPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailAlias[] /*List*/ GetMailForwardings(int packageId, bool recursive)
        {
            return base.Client.GetMailForwardings(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias[]> GetMailForwardingsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetMailForwardingsAsync(packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailAlias GetMailForwarding(int itemId)
        {
            return base.Client.GetMailForwarding(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailAlias> GetMailForwardingAsync(int itemId)
        {
            return await base.Client.GetMailForwardingAsync(itemId);
        }

        public int AddMailForwarding(SolidCP.Providers.Mail.MailAlias item)
        {
            return base.Client.AddMailForwarding(item);
        }

        public async System.Threading.Tasks.Task<int> AddMailForwardingAsync(SolidCP.Providers.Mail.MailAlias item)
        {
            return await base.Client.AddMailForwardingAsync(item);
        }

        public int UpdateMailForwarding(SolidCP.Providers.Mail.MailAlias item)
        {
            return base.Client.UpdateMailForwarding(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailForwardingAsync(SolidCP.Providers.Mail.MailAlias item)
        {
            return await base.Client.UpdateMailForwardingAsync(item);
        }

        public int DeleteMailForwarding(int itemId)
        {
            return base.Client.DeleteMailForwarding(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailForwardingAsync(int itemId)
        {
            return await base.Client.DeleteMailForwardingAsync(itemId);
        }

        public System.Data.DataSet GetRawMailGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawMailGroupsPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailGroupsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawMailGroupsPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailGroup[] /*List*/ GetMailGroups(int packageId, bool recursive)
        {
            return base.Client.GetMailGroups(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup[]> GetMailGroupsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetMailGroupsAsync(packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailGroup GetMailGroup(int itemId)
        {
            return base.Client.GetMailGroup(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailGroup> GetMailGroupAsync(int itemId)
        {
            return await base.Client.GetMailGroupAsync(itemId);
        }

        public int AddMailGroup(SolidCP.Providers.Mail.MailGroup item)
        {
            return base.Client.AddMailGroup(item);
        }

        public async System.Threading.Tasks.Task<int> AddMailGroupAsync(SolidCP.Providers.Mail.MailGroup item)
        {
            return await base.Client.AddMailGroupAsync(item);
        }

        public int UpdateMailGroup(SolidCP.Providers.Mail.MailGroup item)
        {
            return base.Client.UpdateMailGroup(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailGroupAsync(SolidCP.Providers.Mail.MailGroup item)
        {
            return await base.Client.UpdateMailGroupAsync(item);
        }

        public int DeleteMailGroup(int itemId)
        {
            return base.Client.DeleteMailGroup(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailGroupAsync(int itemId)
        {
            return await base.Client.DeleteMailGroupAsync(itemId);
        }

        public System.Data.DataSet GetRawMailListsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawMailListsPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailListsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawMailListsPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailList[] /*List*/ GetMailLists(int packageId, bool recursive)
        {
            return base.Client.GetMailLists(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList[]> GetMailListsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetMailListsAsync(packageId, recursive);
        }

        public SolidCP.Providers.Mail.MailList GetMailList(int itemId)
        {
            return base.Client.GetMailList(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailList> GetMailListAsync(int itemId)
        {
            return await base.Client.GetMailListAsync(itemId);
        }

        public int AddMailList(SolidCP.Providers.Mail.MailList item)
        {
            return base.Client.AddMailList(item);
        }

        public async System.Threading.Tasks.Task<int> AddMailListAsync(SolidCP.Providers.Mail.MailList item)
        {
            return await base.Client.AddMailListAsync(item);
        }

        public int UpdateMailList(SolidCP.Providers.Mail.MailList item)
        {
            return base.Client.UpdateMailList(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailListAsync(SolidCP.Providers.Mail.MailList item)
        {
            return await base.Client.UpdateMailListAsync(item);
        }

        public int DeleteMailList(int itemId)
        {
            return base.Client.DeleteMailList(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailListAsync(int itemId)
        {
            return await base.Client.DeleteMailListAsync(itemId);
        }

        public System.Data.DataSet GetRawMailDomainsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawMailDomainsPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMailDomainsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawMailDomainsPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Mail.MailDomain[] /*List*/ GetMailDomains(int packageId, bool recursive)
        {
            return base.Client.GetMailDomains(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain[]> GetMailDomainsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetMailDomainsAsync(packageId, recursive);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetMailDomainPointers(int itemId)
        {
            return base.Client.GetMailDomainPointers(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetMailDomainPointersAsync(int itemId)
        {
            return await base.Client.GetMailDomainPointersAsync(itemId);
        }

        public SolidCP.Providers.Mail.MailDomain GetMailDomain(int itemId)
        {
            return base.Client.GetMailDomain(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Mail.MailDomain> GetMailDomainAsync(int itemId)
        {
            return await base.Client.GetMailDomainAsync(itemId);
        }

        public int AddMailDomain(SolidCP.Providers.Mail.MailDomain item)
        {
            return base.Client.AddMailDomain(item);
        }

        public async System.Threading.Tasks.Task<int> AddMailDomainAsync(SolidCP.Providers.Mail.MailDomain item)
        {
            return await base.Client.AddMailDomainAsync(item);
        }

        public int UpdateMailDomain(SolidCP.Providers.Mail.MailDomain item)
        {
            return base.Client.UpdateMailDomain(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateMailDomainAsync(SolidCP.Providers.Mail.MailDomain item)
        {
            return await base.Client.UpdateMailDomainAsync(item);
        }

        public int DeleteMailDomain(int itemId)
        {
            return base.Client.DeleteMailDomain(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailDomainAsync(int itemId)
        {
            return await base.Client.DeleteMailDomainAsync(itemId);
        }

        public int AddMailDomainPointer(int itemId, int domainId)
        {
            return base.Client.AddMailDomainPointer(itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> AddMailDomainPointerAsync(int itemId, int domainId)
        {
            return await base.Client.AddMailDomainPointerAsync(itemId, domainId);
        }

        public int DeleteMailDomainPointer(int itemId, int domainId)
        {
            return base.Client.DeleteMailDomainPointer(itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailDomainPointerAsync(int itemId, int domainId)
        {
            return await base.Client.DeleteMailDomainPointerAsync(itemId, domainId);
        }
    }
}
#endif