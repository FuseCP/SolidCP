#if !Client
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

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
    public interface IFTPServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeSiteState(string siteId, ServerState state);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ServerState GetSiteState(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SiteExists(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        FtpSite[] GetSites();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        FtpSite GetSite(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreateSite(FtpSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateSite(FtpSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteSite(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AccountExists(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        FtpAccount[] GetAccounts();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        FtpAccount GetAccount(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateAccount(FtpAccount account);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateAccount(FtpAccount account);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteAccount(string accountName);
    }

    // wcf service
    public class FTPServerService : FTPServer, IFTPServer
    {
        public new void ChangeSiteState(string siteId, ServerState state)
        {
            base.ChangeSiteState(siteId, state);
        }

        public new ServerState GetSiteState(string siteId)
        {
            return base.GetSiteState(siteId);
        }

        public new bool SiteExists(string siteId)
        {
            return base.SiteExists(siteId);
        }

        public new FtpSite[] GetSites()
        {
            return base.GetSites();
        }

        public new FtpSite GetSite(string siteId)
        {
            return base.GetSite(siteId);
        }

        public new string CreateSite(FtpSite site)
        {
            return base.CreateSite(site);
        }

        public new void UpdateSite(FtpSite site)
        {
            base.UpdateSite(site);
        }

        public new void DeleteSite(string siteId)
        {
            base.DeleteSite(siteId);
        }

        public new bool AccountExists(string accountName)
        {
            return base.AccountExists(accountName);
        }

        public new FtpAccount[] GetAccounts()
        {
            return base.GetAccounts();
        }

        public new FtpAccount GetAccount(string accountName)
        {
            return base.GetAccount(accountName);
        }

        public new void CreateAccount(FtpAccount account)
        {
            base.CreateAccount(account);
        }

        public new void UpdateAccount(FtpAccount account)
        {
            base.UpdateAccount(account);
        }

        public new void DeleteAccount(string accountName)
        {
            base.DeleteAccount(accountName);
        }
    }
}
#endif