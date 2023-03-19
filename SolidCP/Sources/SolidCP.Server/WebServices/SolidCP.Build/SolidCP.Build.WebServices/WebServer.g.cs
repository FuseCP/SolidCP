#if !Client
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Server.Utils;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
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
    public interface IWebServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeSiteState(string siteId, ServerState state);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ServerState GetSiteState(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetSiteId(string siteName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetSitesAccounts(string[] siteIds);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SiteExists(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetSites();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebSite GetSite(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ServerBinding[] GetSiteBindings(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreateSite(WebSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateSite(WebSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteSite(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeAppPoolState(string siteId, AppPoolState state);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        AppPoolState GetAppPoolState(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool VirtualDirectoryExists(string siteId, string directoryName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebVirtualDirectory[] GetVirtualDirectories(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteVirtualDirectory(string siteId, string directoryName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AppVirtualDirectoryExists(string siteId, string directoryName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteAppVirtualDirectory(string siteId, string directoryName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsFrontPageSystemInstalled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsFrontPageInstalled(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool InstallFrontPage(string siteId, string username, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UninstallFrontPage(string siteId, string username);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeFrontPagePassword(string username, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsColdFusionSystemInstalled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void InstallSecuredFolders(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UninstallSecuredFolders(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<WebFolder> GetFolders(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebFolder GetFolder(string siteId, string folderPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateFolder(string siteId, WebFolder folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteFolder(string siteId, string folderPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<WebUser> GetUsers(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebUser GetUser(string siteId, string userName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateUser(string siteId, WebUser user);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteUser(string siteId, string userName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<WebGroup> GetGroups(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebGroup GetGroup(string siteId, string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateGroup(string siteId, WebGroup group);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteGroup(string siteId, string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        HeliconApeStatus GetHeliconApeStatus(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void InstallHeliconApe(string ServiceId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void EnableHeliconApe(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DisableHeliconApe(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<HtaccessFolder> GetHeliconApeFolders(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        HtaccessFolder GetHeliconApeHttpdFolder();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateHeliconApeHttpdFolder(HtaccessFolder folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteHeliconApeFolder(string siteId, string folderPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<HtaccessUser> GetHeliconApeUsers(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        HtaccessUser GetHeliconApeUser(string siteId, string userName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateHeliconApeUser(string siteId, HtaccessUser user);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteHeliconApeUser(string siteId, string userName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<WebGroup> GetHeliconApeGroups(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebGroup GetHeliconApeGroup(string siteId, string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateHeliconApeGroup(string siteId, WebGroup group);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RevokeWebDeployPublishingAccess(string siteId, string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteHeliconApeGroup(string siteId, string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebAppVirtualDirectory[] GetZooApplications(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        StringResultObject SetZooConsoleEnabled(string siteId, string appName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        StringResultObject SetZooConsoleDisabled(string siteId, string appName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckLoadUserProfile();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void EnableLoadUserProfile();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void InitFeeds(int UserId, string[] feeds);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetResourceLanguage(int UserId, string resourceLanguage);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        GalleryLanguagesResult GetGalleryLanguages(int UserId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        GalleryCategoriesResult GetGalleryCategories(int UserId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsMsDeployInstalled();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        GalleryApplicationResult GetGalleryApplication(int UserId, string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckWebManagementAccountExists(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject CheckWebManagementPasswordComplexity(string accountPassword);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void GrantWebManagementAccess(string siteId, string accountName, string accountPassword);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RevokeWebManagementAccess(string siteId, string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeWebManagementAccessPassword(string accountName, string accountPassword);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SSLCertificate generateCSR(SSLCertificate certificate);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SSLCertificate generateRenewalCSR(SSLCertificate certificate);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SSLCertificate getCertificate(WebSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SSLCertificate installCertificate(SSLCertificate certificate, WebSite website);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        String LEinstallCertificate(WebSite website, string email);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SSLCertificate installPFX(byte[] certificate, string password, WebSite website);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] exportCertificate(string serialNumber, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<SSLCertificate> getServerCertificates();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SSLCertificate ImportCertificate(WebSite website);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckCertificate(WebSite webSite);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool GetDirectoryBrowseEnabled(string siteId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetDirectoryBrowseEnabled(string siteId, bool enabled);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class WebServer : SolidCP.Server.WebServer, IWebServer
    {
    }
}
#endif