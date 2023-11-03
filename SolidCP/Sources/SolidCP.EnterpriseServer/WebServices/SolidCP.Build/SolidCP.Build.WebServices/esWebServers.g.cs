#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesWebServers
    {
        [WebMethod]
        [OperationContract]
        DataSet GetRawWebSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<WebSite> GetWebSites(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        WebSite GetWebSite(int siteItemId);
        [WebMethod]
        [OperationContract]
        List<DomainInfo> GetWebSitePointers(int siteItemId);
        [WebMethod]
        [OperationContract]
        int AddWebSitePointer(int siteItemId, string hostName, int domainId);
        [WebMethod]
        [OperationContract]
        int DeleteWebSitePointer(int siteItemId, int domainId);
        [WebMethod]
        [OperationContract]
        int AddWebSite(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone);
        [WebMethod]
        [OperationContract]
        int UpdateWebSite(WebSite site);
        [WebMethod]
        [OperationContract]
        int InstallFrontPage(int siteItemId, string username, string password);
        [WebMethod]
        [OperationContract]
        int UninstallFrontPage(int siteItemId);
        [WebMethod]
        [OperationContract]
        int ChangeFrontPagePassword(int siteItemId, string password);
        [WebMethod]
        [OperationContract]
        int RepairWebSite(int siteItemId);
        [WebMethod]
        [OperationContract]
        int DeleteWebSite(int siteItemId, bool deleteWebsiteDirectory);
        [WebMethod]
        [OperationContract]
        int SwitchWebSiteToDedicatedIP(int siteItemId, int ipAddressId);
        [WebMethod]
        [OperationContract]
        int SwitchWebSiteToSharedIP(int siteItemId);
        [WebMethod]
        [OperationContract]
        int AddVirtualDirectory(int siteItemId, string vdirName, string vdirPath);
        [WebMethod]
        [OperationContract]
        List<WebVirtualDirectory> GetVirtualDirectories(int siteItemId);
        [WebMethod]
        [OperationContract]
        WebVirtualDirectory GetVirtualDirectory(int siteItemId, string vdirName);
        [WebMethod]
        [OperationContract]
        int UpdateVirtualDirectory(int siteItemId, WebVirtualDirectory vdir);
        [WebMethod]
        [OperationContract]
        int DeleteVirtualDirectory(int siteItemId, string vdirName);
        [WebMethod]
        [OperationContract]
        int AddAppVirtualDirectory(int siteItemId, string vdirName, string vdirPath, string aspNetVersion);
        [WebMethod]
        [OperationContract]
        List<WebAppVirtualDirectory> GetAppVirtualDirectories(int siteItemId);
        [WebMethod]
        [OperationContract]
        WebAppVirtualDirectory GetAppVirtualDirectory(int siteItemId, string vdirName);
        [WebMethod]
        [OperationContract]
        int UpdateAppVirtualDirectory(int siteItemId, WebAppVirtualDirectory vdir);
        [WebMethod]
        [OperationContract]
        int DeleteAppVirtualDirectory(int siteItemId, string vdirName);
        [WebMethod]
        [OperationContract]
        int ChangeSiteState(int siteItemId, ServerState state);
        [WebMethod]
        [OperationContract]
        int ChangeAppPoolState(int siteItemId, AppPoolState state);
        [WebMethod]
        [OperationContract]
        AppPoolState GetAppPoolState(int siteItemId);
        [WebMethod]
        [OperationContract]
        ServerState GetSiteState(int siteItemId);
        [WebMethod]
        [OperationContract]
        List<string> GetSharedSSLDomains(int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawSSLFoldersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<SharedSSLFolder> GetSharedSSLFolders(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        SharedSSLFolder GetSharedSSLFolder(int itemId);
        [WebMethod]
        [OperationContract]
        int AddSharedSSLFolder(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath);
        [WebMethod]
        [OperationContract]
        int UpdateSharedSSLFolder(SharedSSLFolder vdir);
        [WebMethod]
        [OperationContract]
        int DeleteSharedSSLFolder(int itemId);
        [WebMethod]
        [OperationContract]
        int InstallSecuredFolders(int siteItemId);
        [WebMethod]
        [OperationContract]
        int UninstallSecuredFolders(int siteItemId);
        [WebMethod]
        [OperationContract]
        WebFolder[] GetSecuredFolders(int siteItemId);
        [WebMethod]
        [OperationContract]
        WebFolder GetSecuredFolder(int siteItemId, string folderPath);
        [WebMethod]
        [OperationContract]
        int UpdateSecuredFolder(int siteItemId, WebFolder folder);
        [WebMethod]
        [OperationContract]
        int DeleteSecuredFolder(int siteItemId, string folderPath);
        [WebMethod]
        [OperationContract]
        WebUser[] GetSecuredUsers(int siteItemId);
        [WebMethod]
        [OperationContract]
        WebUser GetSecuredUser(int siteItemId, string userName);
        [WebMethod]
        [OperationContract]
        int UpdateSecuredUser(int siteItemId, WebUser user);
        [WebMethod]
        [OperationContract]
        int DeleteSecuredUser(int siteItemId, string userName);
        [WebMethod]
        [OperationContract]
        WebGroup[] GetSecuredGroups(int siteItemId);
        [WebMethod]
        [OperationContract]
        WebGroup GetSecuredGroup(int siteItemId, string groupName);
        [WebMethod]
        [OperationContract]
        int UpdateSecuredGroup(int siteItemId, WebGroup group);
        [WebMethod]
        [OperationContract]
        int DeleteSecuredGroup(int siteItemId, string groupName);
        [WebMethod]
        [OperationContract]
        ResultObject GrantWebDeployPublishingAccess(int siteItemId, string accountName, string accountPassword);
        [WebMethod]
        [OperationContract]
        ResultObject SaveWebDeployPublishingProfile(int siteItemId, int[] serviceItemIds);
        [WebMethod]
        [OperationContract]
        void RevokeWebDeployPublishingAccess(int siteItemId);
        [WebMethod]
        [OperationContract]
        BytesResult GetWebDeployPublishingProfile(int siteItemId);
        [WebMethod]
        [OperationContract]
        ResultObject ChangeWebDeployPublishingPassword(int siteItemId, string newAccountPassword);
        [WebMethod]
        [OperationContract]
        HeliconApeStatus GetHeliconApeStatus(int siteItemId);
        [WebMethod]
        [OperationContract]
        void InstallHeliconApe(int siteItemId);
        [WebMethod]
        [OperationContract]
        int EnableHeliconApe(int siteItemId);
        [WebMethod]
        [OperationContract]
        int DisableHeliconApe(int siteItemId);
        [WebMethod]
        [OperationContract]
        int EnableHeliconApeGlobally(int serviceId);
        [WebMethod]
        [OperationContract]
        int DisableHeliconApeGlobally(int serviceId);
        [WebMethod]
        [OperationContract]
        HtaccessFolder[] GetHeliconApeFolders(int siteItemId);
        [WebMethod]
        [OperationContract]
        HtaccessFolder GetHeliconApeHttpdFolder(int serviceId);
        [WebMethod]
        [OperationContract]
        HtaccessFolder GetHeliconApeFolder(int siteItemId, string folderPath);
        [WebMethod]
        [OperationContract]
        int UpdateHeliconApeFolder(int siteItemId, HtaccessFolder folder);
        [WebMethod]
        [OperationContract]
        int UpdateHeliconApeHttpdFolder(int serviceId, HtaccessFolder folder);
        [WebMethod]
        [OperationContract]
        int DeleteHeliconApeFolder(int siteItemId, string folderPath);
        [WebMethod]
        [OperationContract]
        HtaccessUser[] GetHeliconApeUsers(int siteItemId);
        [WebMethod]
        [OperationContract]
        HtaccessUser GetHeliconApeUser(int siteItemId, string userName);
        [WebMethod]
        [OperationContract]
        int UpdateHeliconApeUser(int siteItemId, HtaccessUser user);
        [WebMethod]
        [OperationContract]
        int DeleteHeliconApeUser(int siteItemId, string userName);
        [WebMethod]
        [OperationContract]
        WebGroup[] GetHeliconApeGroups(int siteItemId);
        [WebMethod]
        [OperationContract]
        WebGroup GetHeliconApeGroup(int siteItemId, string groupName);
        [WebMethod]
        [OperationContract]
        int UpdateHeliconApeGroup(int siteItemId, WebGroup group);
        [WebMethod]
        [OperationContract]
        int DeleteHeliconApeGroup(int siteItemId, string groupName);
        [WebMethod]
        [OperationContract]
        List<WebAppVirtualDirectory> GetZooApplications(int siteItemId);
        [WebMethod]
        [OperationContract]
        StringResultObject SetZooEnvironmentVariable(int siteItemId, string appName, string envName, string envValue);
        [WebMethod]
        [OperationContract]
        StringResultObject SetZooConsoleEnabled(int siteItemId, string appName);
        [WebMethod]
        [OperationContract]
        StringResultObject SetZooConsoleDisabled(int siteItemId, string appName);
        [WebMethod]
        [OperationContract]
        ResultObject GrantWebManagementAccess(int siteItemId, string accountName, string accountPassword);
        [WebMethod]
        [OperationContract]
        void RevokeWebManagementAccess(int siteItemId);
        [WebMethod]
        [OperationContract]
        ResultObject ChangeWebManagementAccessPassword(int siteItemId, string accountPassword);
        [WebMethod]
        [OperationContract]
        SSLCertificate CertificateRequest(SSLCertificate certificate, int siteItemId);
        [WebMethod]
        [OperationContract]
        ResultObject InstallCertificate(SSLCertificate certificate, int siteItemId);
        [WebMethod]
        [OperationContract]
        ResultObject LEInstallCertificate(int siteItemId, string email);
        [WebMethod]
        [OperationContract]
        ResultObject InstallPfx(byte[] certificate, int siteItemId, string password);
        [WebMethod]
        [OperationContract]
        List<SSLCertificate> GetPendingCertificates(int siteItemId);
        [WebMethod]
        [OperationContract]
        SSLCertificate GetSSLCertificateByID(int Id);
        [WebMethod]
        [OperationContract]
        SSLCertificate GetSiteCert(int siteID);
        [WebMethod]
        [OperationContract]
        int CheckSSLForWebsite(int siteID, bool renewal);
        [WebMethod]
        [OperationContract]
        ResultObject CheckSSLForDomain(string domain, int siteID);
        [WebMethod]
        [OperationContract]
        byte[] ExportCertificate(int siteId, string serialNumber, string password);
        [WebMethod]
        [OperationContract]
        List<SSLCertificate> GetCertificatesForSite(int siteId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteCertificate(int siteId, SSLCertificate certificate);
        [WebMethod]
        [OperationContract]
        ResultObject ImportCertificate(int siteId);
        [WebMethod]
        [OperationContract]
        ResultObject CheckCertificate(int siteId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteCertificateRequest(int siteId, int csrID);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esWebServers : SolidCP.EnterpriseServer.esWebServers, IesWebServers
    {
    }
}
#endif