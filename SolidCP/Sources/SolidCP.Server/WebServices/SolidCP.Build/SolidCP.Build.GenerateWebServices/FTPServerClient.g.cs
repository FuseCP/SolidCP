#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.FTP;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IFTPServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IFTPServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteStateResponse")]
        void ChangeSiteState(string siteId, ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/ChangeSiteStateResponse")]
        System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteStateResponse")]
        ServerState GetSiteState(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteStateResponse")]
        System.Threading.Tasks.Task<ServerState> GetSiteStateAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/SiteExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/SiteExistsResponse")]
        bool SiteExists(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/SiteExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/SiteExistsResponse")]
        System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSitesResponse")]
        FtpSite[] GetSites();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSitesResponse")]
        System.Threading.Tasks.Task<FtpSite[]> GetSitesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteResponse")]
        FtpSite GetSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetSiteResponse")]
        System.Threading.Tasks.Task<FtpSite> GetSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateSiteResponse")]
        string CreateSite(FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateSiteResponse")]
        System.Threading.Tasks.Task<string> CreateSiteAsync(FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateSiteResponse")]
        void UpdateSite(FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateSiteResponse")]
        System.Threading.Tasks.Task UpdateSiteAsync(FtpSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteSiteResponse")]
        void DeleteSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteSiteResponse")]
        System.Threading.Tasks.Task DeleteSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/AccountExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/AccountExistsResponse")]
        bool AccountExists(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/AccountExists", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/AccountExistsResponse")]
        System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountsResponse")]
        FtpAccount[] GetAccounts();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountsResponse")]
        System.Threading.Tasks.Task<FtpAccount[]> GetAccountsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountResponse")]
        FtpAccount GetAccount(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/GetAccountResponse")]
        System.Threading.Tasks.Task<FtpAccount> GetAccountAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateAccountResponse")]
        void CreateAccount(FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/CreateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/CreateAccountResponse")]
        System.Threading.Tasks.Task CreateAccountAsync(FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccountResponse")]
        void UpdateAccount(FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/UpdateAccountResponse")]
        System.Threading.Tasks.Task UpdateAccountAsync(FtpAccount account);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccountResponse")]
        void DeleteAccount(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccount", ReplyAction = "http://smbsaas/solidcp/server/IFTPServer/DeleteAccountResponse")]
        System.Threading.Tasks.Task DeleteAccountAsync(string accountName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class FTPServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IFTPServer
    {
        public void ChangeSiteState(string siteId, ServerState state)
        {
            Invoke("SolidCP.Server.FTPServer", "ChangeSiteState", siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, ServerState state)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "ChangeSiteState", siteId, state);
        }

        public ServerState GetSiteState(string siteId)
        {
            return (ServerState)Invoke("SolidCP.Server.FTPServer", "GetSiteState", siteId);
        }

        public async System.Threading.Tasks.Task<ServerState> GetSiteStateAsync(string siteId)
        {
            return await InvokeAsync<ServerState>("SolidCP.Server.FTPServer", "GetSiteState", siteId);
        }

        public bool SiteExists(string siteId)
        {
            return (bool)Invoke("SolidCP.Server.FTPServer", "SiteExists", siteId);
        }

        public async System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.FTPServer", "SiteExists", siteId);
        }

        public FtpSite[] GetSites()
        {
            return (FtpSite[])Invoke("SolidCP.Server.FTPServer", "GetSites");
        }

        public async System.Threading.Tasks.Task<FtpSite[]> GetSitesAsync()
        {
            return await InvokeAsync<FtpSite[]>("SolidCP.Server.FTPServer", "GetSites");
        }

        public FtpSite GetSite(string siteId)
        {
            return (FtpSite)Invoke("SolidCP.Server.FTPServer", "GetSite", siteId);
        }

        public async System.Threading.Tasks.Task<FtpSite> GetSiteAsync(string siteId)
        {
            return await InvokeAsync<FtpSite>("SolidCP.Server.FTPServer", "GetSite", siteId);
        }

        public string CreateSite(FtpSite site)
        {
            return (string)Invoke("SolidCP.Server.FTPServer", "CreateSite", site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(FtpSite site)
        {
            return await InvokeAsync<string>("SolidCP.Server.FTPServer", "CreateSite", site);
        }

        public void UpdateSite(FtpSite site)
        {
            Invoke("SolidCP.Server.FTPServer", "UpdateSite", site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(FtpSite site)
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
            return (bool)Invoke("SolidCP.Server.FTPServer", "AccountExists", accountName);
        }

        public async System.Threading.Tasks.Task<bool> AccountExistsAsync(string accountName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.FTPServer", "AccountExists", accountName);
        }

        public FtpAccount[] GetAccounts()
        {
            return (FtpAccount[])Invoke("SolidCP.Server.FTPServer", "GetAccounts");
        }

        public async System.Threading.Tasks.Task<FtpAccount[]> GetAccountsAsync()
        {
            return await InvokeAsync<FtpAccount[]>("SolidCP.Server.FTPServer", "GetAccounts");
        }

        public FtpAccount GetAccount(string accountName)
        {
            return (FtpAccount)Invoke("SolidCP.Server.FTPServer", "GetAccount", accountName);
        }

        public async System.Threading.Tasks.Task<FtpAccount> GetAccountAsync(string accountName)
        {
            return await InvokeAsync<FtpAccount>("SolidCP.Server.FTPServer", "GetAccount", accountName);
        }

        public void CreateAccount(FtpAccount account)
        {
            Invoke("SolidCP.Server.FTPServer", "CreateAccount", account);
        }

        public async System.Threading.Tasks.Task CreateAccountAsync(FtpAccount account)
        {
            await InvokeAsync("SolidCP.Server.FTPServer", "CreateAccount", account);
        }

        public void UpdateAccount(FtpAccount account)
        {
            Invoke("SolidCP.Server.FTPServer", "UpdateAccount", account);
        }

        public async System.Threading.Tasks.Task UpdateAccountAsync(FtpAccount account)
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
        public void ChangeSiteState(string siteId, ServerState state)
        {
            base.Client.ChangeSiteState(siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, ServerState state)
        {
            await base.Client.ChangeSiteStateAsync(siteId, state);
        }

        public ServerState GetSiteState(string siteId)
        {
            return base.Client.GetSiteState(siteId);
        }

        public async System.Threading.Tasks.Task<ServerState> GetSiteStateAsync(string siteId)
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

        public FtpSite[] GetSites()
        {
            return base.Client.GetSites();
        }

        public async System.Threading.Tasks.Task<FtpSite[]> GetSitesAsync()
        {
            return await base.Client.GetSitesAsync();
        }

        public FtpSite GetSite(string siteId)
        {
            return base.Client.GetSite(siteId);
        }

        public async System.Threading.Tasks.Task<FtpSite> GetSiteAsync(string siteId)
        {
            return await base.Client.GetSiteAsync(siteId);
        }

        public string CreateSite(FtpSite site)
        {
            return base.Client.CreateSite(site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(FtpSite site)
        {
            return await base.Client.CreateSiteAsync(site);
        }

        public void UpdateSite(FtpSite site)
        {
            base.Client.UpdateSite(site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(FtpSite site)
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

        public FtpAccount[] GetAccounts()
        {
            return base.Client.GetAccounts();
        }

        public async System.Threading.Tasks.Task<FtpAccount[]> GetAccountsAsync()
        {
            return await base.Client.GetAccountsAsync();
        }

        public FtpAccount GetAccount(string accountName)
        {
            return base.Client.GetAccount(accountName);
        }

        public async System.Threading.Tasks.Task<FtpAccount> GetAccountAsync(string accountName)
        {
            return await base.Client.GetAccountAsync(accountName);
        }

        public void CreateAccount(FtpAccount account)
        {
            base.Client.CreateAccount(account);
        }

        public async System.Threading.Tasks.Task CreateAccountAsync(FtpAccount account)
        {
            await base.Client.CreateAccountAsync(account);
        }

        public void UpdateAccount(FtpAccount account)
        {
            base.Client.UpdateAccount(account);
        }

        public async System.Threading.Tasks.Task UpdateAccountAsync(FtpAccount account)
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