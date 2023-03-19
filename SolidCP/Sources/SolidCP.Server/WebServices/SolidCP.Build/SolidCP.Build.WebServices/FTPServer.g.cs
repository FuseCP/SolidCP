#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.FTP;
using SolidCP.Server.Utils;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
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

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class FTPServer : SolidCP.Server.FTPServer, IFTPServer
    {
    }
}
#endif