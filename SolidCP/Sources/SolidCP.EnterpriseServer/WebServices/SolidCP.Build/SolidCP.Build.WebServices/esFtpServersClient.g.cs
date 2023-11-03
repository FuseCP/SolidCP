#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesFtpServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesFtpServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpSitesResponse")]
        SolidCP.Providers.FTP.FtpSite[] GetFtpSites(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpSitesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite[]> GetFtpSitesAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetRawFtpAccountsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetRawFtpAccountsPagedResponse")]
        System.Data.DataSet GetRawFtpAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetRawFtpAccountsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetRawFtpAccountsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawFtpAccountsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccountsResponse")]
        SolidCP.Providers.FTP.FtpAccount[] /*List*/ GetFtpAccounts(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount[]> GetFtpAccountsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccountResponse")]
        SolidCP.Providers.FTP.FtpAccount GetFtpAccount(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/GetFtpAccountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount> GetFtpAccountAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/AddFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/AddFtpAccountResponse")]
        int AddFtpAccount(SolidCP.Providers.FTP.FtpAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/AddFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/AddFtpAccountResponse")]
        System.Threading.Tasks.Task<int> AddFtpAccountAsync(SolidCP.Providers.FTP.FtpAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/UpdateFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/UpdateFtpAccountResponse")]
        int UpdateFtpAccount(SolidCP.Providers.FTP.FtpAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/UpdateFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/UpdateFtpAccountResponse")]
        System.Threading.Tasks.Task<int> UpdateFtpAccountAsync(SolidCP.Providers.FTP.FtpAccount item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/DeleteFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/DeleteFtpAccountResponse")]
        int DeleteFtpAccount(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/DeleteFtpAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFtpServers/DeleteFtpAccountResponse")]
        System.Threading.Tasks.Task<int> DeleteFtpAccountAsync(int itemId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esFtpServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesFtpServers
    {
        public SolidCP.Providers.FTP.FtpSite[] GetFtpSites(int serviceId)
        {
            return Invoke<SolidCP.Providers.FTP.FtpSite[]>("SolidCP.EnterpriseServer.esFtpServers", "GetFtpSites", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite[]> GetFtpSitesAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.FTP.FtpSite[]>("SolidCP.EnterpriseServer.esFtpServers", "GetFtpSites", serviceId);
        }

        public System.Data.DataSet GetRawFtpAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esFtpServers", "GetRawFtpAccountsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawFtpAccountsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esFtpServers", "GetRawFtpAccountsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.FTP.FtpAccount[] /*List*/ GetFtpAccounts(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.FTP.FtpAccount[], SolidCP.Providers.FTP.FtpAccount>("SolidCP.EnterpriseServer.esFtpServers", "GetFtpAccounts", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount[]> GetFtpAccountsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.FTP.FtpAccount[], SolidCP.Providers.FTP.FtpAccount>("SolidCP.EnterpriseServer.esFtpServers", "GetFtpAccounts", packageId, recursive);
        }

        public SolidCP.Providers.FTP.FtpAccount GetFtpAccount(int itemId)
        {
            return Invoke<SolidCP.Providers.FTP.FtpAccount>("SolidCP.EnterpriseServer.esFtpServers", "GetFtpAccount", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount> GetFtpAccountAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.FTP.FtpAccount>("SolidCP.EnterpriseServer.esFtpServers", "GetFtpAccount", itemId);
        }

        public int AddFtpAccount(SolidCP.Providers.FTP.FtpAccount item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFtpServers", "AddFtpAccount", item);
        }

        public async System.Threading.Tasks.Task<int> AddFtpAccountAsync(SolidCP.Providers.FTP.FtpAccount item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFtpServers", "AddFtpAccount", item);
        }

        public int UpdateFtpAccount(SolidCP.Providers.FTP.FtpAccount item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFtpServers", "UpdateFtpAccount", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateFtpAccountAsync(SolidCP.Providers.FTP.FtpAccount item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFtpServers", "UpdateFtpAccount", item);
        }

        public int DeleteFtpAccount(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFtpServers", "DeleteFtpAccount", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteFtpAccountAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFtpServers", "DeleteFtpAccount", itemId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esFtpServers : SolidCP.Web.Client.ClientBase<IesFtpServers, esFtpServersAssemblyClient>, IesFtpServers
    {
        public SolidCP.Providers.FTP.FtpSite[] GetFtpSites(int serviceId)
        {
            return base.Client.GetFtpSites(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite[]> GetFtpSitesAsync(int serviceId)
        {
            return await base.Client.GetFtpSitesAsync(serviceId);
        }

        public System.Data.DataSet GetRawFtpAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawFtpAccountsPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawFtpAccountsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawFtpAccountsPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.FTP.FtpAccount[] /*List*/ GetFtpAccounts(int packageId, bool recursive)
        {
            return base.Client.GetFtpAccounts(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount[]> GetFtpAccountsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetFtpAccountsAsync(packageId, recursive);
        }

        public SolidCP.Providers.FTP.FtpAccount GetFtpAccount(int itemId)
        {
            return base.Client.GetFtpAccount(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount> GetFtpAccountAsync(int itemId)
        {
            return await base.Client.GetFtpAccountAsync(itemId);
        }

        public int AddFtpAccount(SolidCP.Providers.FTP.FtpAccount item)
        {
            return base.Client.AddFtpAccount(item);
        }

        public async System.Threading.Tasks.Task<int> AddFtpAccountAsync(SolidCP.Providers.FTP.FtpAccount item)
        {
            return await base.Client.AddFtpAccountAsync(item);
        }

        public int UpdateFtpAccount(SolidCP.Providers.FTP.FtpAccount item)
        {
            return base.Client.UpdateFtpAccount(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateFtpAccountAsync(SolidCP.Providers.FTP.FtpAccount item)
        {
            return await base.Client.UpdateFtpAccountAsync(item);
        }

        public int DeleteFtpAccount(int itemId)
        {
            return base.Client.DeleteFtpAccount(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteFtpAccountAsync(int itemId)
        {
            return await base.Client.DeleteFtpAccountAsync(itemId);
        }
    }
}
#endif