#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IFTPServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IFTPServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteStateResponse")]
        void ChangeSiteState(string siteId, SolidCP.Providers.ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteStateResponse")]
        System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, SolidCP.Providers.ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteStateResponse")]
        SolidCP.Providers.ServerState GetSiteState(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/SiteExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/SiteExistsResponse")]
        bool SiteExists(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/SiteExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/SiteExistsResponse")]
        System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSitesResponse")]
        SolidCP.Providers.FTP.FtpSite[] GetSites();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSitesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite[]> GetSitesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteResponse")]
        SolidCP.Providers.FTP.FtpSite GetSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite> GetSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateSiteResponse")]
        string CreateSite(SolidCP.Providers.FTP.FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateSiteResponse")]
        System.Threading.Tasks.Task<string> CreateSiteAsync(SolidCP.Providers.FTP.FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateSiteResponse")]
        void UpdateSite(SolidCP.Providers.FTP.FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateSiteResponse")]
        System.Threading.Tasks.Task UpdateSiteAsync(SolidCP.Providers.FTP.FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteSiteResponse")]
        void DeleteSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteSiteResponse")]
        System.Threading.Tasks.Task DeleteSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/AccountExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/AccountExistsResponse")]
        bool AccountExists(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/AccountExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/AccountExistsResponse")]
        System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountsResponse")]
        SolidCP.Providers.FTP.FtpAccount[] GetAccounts();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount[]> GetAccountsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountResponse")]
        SolidCP.Providers.FTP.FtpAccount GetAccount(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount> GetAccountAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateAccountResponse")]
        void CreateAccount(SolidCP.Providers.FTP.FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateAccountResponse")]
        System.Threading.Tasks.Task CreateAccountAsync(SolidCP.Providers.FTP.FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccountResponse")]
        void UpdateAccount(SolidCP.Providers.FTP.FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccountResponse")]
        System.Threading.Tasks.Task UpdateAccountAsync(SolidCP.Providers.FTP.FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccountResponse")]
        void DeleteAccount(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccountResponse")]
        System.Threading.Tasks.Task DeleteAccountAsync(string accountName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class FTPServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IFTPServer
    {
        public void ChangeSiteState(string siteId, SolidCP.Providers.ServerState state)
        {
            Invoke("SolidCP.Server.FTPServer", "ChangeSiteState", siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, SolidCP.Providers.ServerState state)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "ChangeSiteState", siteId, state);
        }

        public SolidCP.Providers.ServerState GetSiteState(string siteId)
        {
            return Invoke<SolidCP.Providers.ServerState>("SolidCP.Server.FTPServer", "GetSiteState", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.ServerState>("SolidCP.Server.FTPServer", "GetSiteState", siteId);
        }

        public bool SiteExists(string siteId)
        {
            return Invoke<bool>("SolidCP.Server.FTPServer", "SiteExists", siteId);
        }

        public async System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.FTPServer", "SiteExists", siteId);
        }

        public SolidCP.Providers.FTP.FtpSite[] GetSites()
        {
            return Invoke<SolidCP.Providers.FTP.FtpSite[]>("SolidCP.Server.FTPServer", "GetSites");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite[]> GetSitesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.FTP.FtpSite[]>("SolidCP.Server.FTPServer", "GetSites");
        }

        public SolidCP.Providers.FTP.FtpSite GetSite(string siteId)
        {
            return Invoke<SolidCP.Providers.FTP.FtpSite>("SolidCP.Server.FTPServer", "GetSite", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite> GetSiteAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.FTP.FtpSite>("SolidCP.Server.FTPServer", "GetSite", siteId);
        }

        public string CreateSite(SolidCP.Providers.FTP.FtpSite site)
        {
            return Invoke<string>("SolidCP.Server.FTPServer", "CreateSite", site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(SolidCP.Providers.FTP.FtpSite site)
        {
            return await InvokeAsync<string>("SolidCP.Server.FTPServer", "CreateSite", site);
        }

        public void UpdateSite(SolidCP.Providers.FTP.FtpSite site)
        {
            Invoke("SolidCP.Server.FTPServer", "UpdateSite", site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(SolidCP.Providers.FTP.FtpSite site)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "UpdateSite", site);
        }

        public void DeleteSite(string siteId)
        {
            Invoke("SolidCP.Server.FTPServer", "DeleteSite", siteId);
        }

        public async System.Threading.Tasks.Task DeleteSiteAsync(string siteId)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "DeleteSite", siteId);
        }

        public bool AccountExists(string accountName)
        {
            return Invoke<bool>("SolidCP.Server.FTPServer", "AccountExists", accountName);
        }

        public async System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.FTPServer", "AccountExists", accountName);
        }

        public SolidCP.Providers.FTP.FtpAccount[] GetAccounts()
        {
            return Invoke<SolidCP.Providers.FTP.FtpAccount[]>("SolidCP.Server.FTPServer", "GetAccounts");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount[]> GetAccountsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.FTP.FtpAccount[]>("SolidCP.Server.FTPServer", "GetAccounts");
        }

        public SolidCP.Providers.FTP.FtpAccount GetAccount(string accountName)
        {
            return Invoke<SolidCP.Providers.FTP.FtpAccount>("SolidCP.Server.FTPServer", "GetAccount", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount> GetAccountAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.FTP.FtpAccount>("SolidCP.Server.FTPServer", "GetAccount", accountName);
        }

        public void CreateAccount(SolidCP.Providers.FTP.FtpAccount account)
        {
            Invoke("SolidCP.Server.FTPServer", "CreateAccount", account);
        }

        public async System.Threading.Tasks.Task CreateAccountAsync(SolidCP.Providers.FTP.FtpAccount account)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "CreateAccount", account);
        }

        public void UpdateAccount(SolidCP.Providers.FTP.FtpAccount account)
        {
            Invoke("SolidCP.Server.FTPServer", "UpdateAccount", account);
        }

        public async System.Threading.Tasks.Task UpdateAccountAsync(SolidCP.Providers.FTP.FtpAccount account)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "UpdateAccount", account);
        }

        public void DeleteAccount(string accountName)
        {
            Invoke("SolidCP.Server.FTPServer", "DeleteAccount", accountName);
        }

        public async System.Threading.Tasks.Task DeleteAccountAsync(string accountName)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "DeleteAccount", accountName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class FTPServer : SolidCP.Web.Client.ClientBase<IFTPServer, FTPServerAssemblyClient>, IFTPServer
    {
        public void ChangeSiteState(string siteId, SolidCP.Providers.ServerState state)
        {
            base.Client.ChangeSiteState(siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, SolidCP.Providers.ServerState state)
        {
            await base.Client.ChangeSiteStateAsync(siteId, state);
        }

        public SolidCP.Providers.ServerState GetSiteState(string siteId)
        {
            return base.Client.GetSiteState(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(string siteId)
        {
            return await base.Client.GetSiteStateAsync(siteId);
        }

        public bool SiteExists(string siteId)
        {
            return base.Client.SiteExists(siteId);
        }

        public async System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId)
        {
            return await base.Client.SiteExistsAsync(siteId);
        }

        public SolidCP.Providers.FTP.FtpSite[] GetSites()
        {
            return base.Client.GetSites();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite[]> GetSitesAsync()
        {
            return await base.Client.GetSitesAsync();
        }

        public SolidCP.Providers.FTP.FtpSite GetSite(string siteId)
        {
            return base.Client.GetSite(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpSite> GetSiteAsync(string siteId)
        {
            return await base.Client.GetSiteAsync(siteId);
        }

        public string CreateSite(SolidCP.Providers.FTP.FtpSite site)
        {
            return base.Client.CreateSite(site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(SolidCP.Providers.FTP.FtpSite site)
        {
            return await base.Client.CreateSiteAsync(site);
        }

        public void UpdateSite(SolidCP.Providers.FTP.FtpSite site)
        {
            base.Client.UpdateSite(site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(SolidCP.Providers.FTP.FtpSite site)
        {
            await base.Client.UpdateSiteAsync(site);
        }

        public void DeleteSite(string siteId)
        {
            base.Client.DeleteSite(siteId);
        }

        public async System.Threading.Tasks.Task DeleteSiteAsync(string siteId)
        {
            await base.Client.DeleteSiteAsync(siteId);
        }

        public bool AccountExists(string accountName)
        {
            return base.Client.AccountExists(accountName);
        }

        public async System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName)
        {
            return await base.Client.AccountExistsAsync(accountName);
        }

        public SolidCP.Providers.FTP.FtpAccount[] GetAccounts()
        {
            return base.Client.GetAccounts();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount[]> GetAccountsAsync()
        {
            return await base.Client.GetAccountsAsync();
        }

        public SolidCP.Providers.FTP.FtpAccount GetAccount(string accountName)
        {
            return base.Client.GetAccount(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.FTP.FtpAccount> GetAccountAsync(string accountName)
        {
            return await base.Client.GetAccountAsync(accountName);
        }

        public void CreateAccount(SolidCP.Providers.FTP.FtpAccount account)
        {
            base.Client.CreateAccount(account);
        }

        public async System.Threading.Tasks.Task CreateAccountAsync(SolidCP.Providers.FTP.FtpAccount account)
        {
            await base.Client.CreateAccountAsync(account);
        }

        public void UpdateAccount(SolidCP.Providers.FTP.FtpAccount account)
        {
            base.Client.UpdateAccount(account);
        }

        public async System.Threading.Tasks.Task UpdateAccountAsync(SolidCP.Providers.FTP.FtpAccount account)
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
    }
}
#endif