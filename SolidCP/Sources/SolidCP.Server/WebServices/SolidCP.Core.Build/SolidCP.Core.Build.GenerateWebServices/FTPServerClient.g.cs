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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for FTPServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
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

    public class FTPServer
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif