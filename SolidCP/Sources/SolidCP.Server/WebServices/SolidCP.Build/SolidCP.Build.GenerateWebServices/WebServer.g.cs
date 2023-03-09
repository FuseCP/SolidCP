#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Server.Utils;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;
using SolidCP.Server;
using System.ServiceModel;
using System.ServiceModel.Activation;

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
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WebServer : SolidCP.Server.WebServer, IWebServer
    {
        public new void ChangeSiteState(string siteId, ServerState state)
        {
            base.ChangeSiteState(siteId, state);
        }

        public new ServerState GetSiteState(string siteId)
        {
            return base.GetSiteState(siteId);
        }

        public new string GetSiteId(string siteName)
        {
            return base.GetSiteId(siteName);
        }

        public new string[] GetSitesAccounts(string[] siteIds)
        {
            return base.GetSitesAccounts(siteIds);
        }

        public new bool SiteExists(string siteId)
        {
            return base.SiteExists(siteId);
        }

        public new string[] GetSites()
        {
            return base.GetSites();
        }

        public new WebSite GetSite(string siteId)
        {
            return base.GetSite(siteId);
        }

        public new ServerBinding[] GetSiteBindings(string siteId)
        {
            return base.GetSiteBindings(siteId);
        }

        public new string CreateSite(WebSite site)
        {
            return base.CreateSite(site);
        }

        public new void UpdateSite(WebSite site)
        {
            base.UpdateSite(site);
        }

        public new void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            base.UpdateSiteBindings(siteId, bindings, emptyBindingsAllowed);
        }

        public new void DeleteSite(string siteId)
        {
            base.DeleteSite(siteId);
        }

        public new void ChangeAppPoolState(string siteId, AppPoolState state)
        {
            base.ChangeAppPoolState(siteId, state);
        }

        public new AppPoolState GetAppPoolState(string siteId)
        {
            return base.GetAppPoolState(siteId);
        }

        public new bool VirtualDirectoryExists(string siteId, string directoryName)
        {
            return base.VirtualDirectoryExists(siteId, directoryName);
        }

        public new WebVirtualDirectory[] GetVirtualDirectories(string siteId)
        {
            return base.GetVirtualDirectories(siteId);
        }

        public new WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            return base.GetVirtualDirectory(siteId, directoryName);
        }

        public new void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            base.CreateVirtualDirectory(siteId, directory);
        }

        public new void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            base.UpdateVirtualDirectory(siteId, directory);
        }

        public new void DeleteVirtualDirectory(string siteId, string directoryName)
        {
            base.DeleteVirtualDirectory(siteId, directoryName);
        }

        public new bool AppVirtualDirectoryExists(string siteId, string directoryName)
        {
            return base.AppVirtualDirectoryExists(siteId, directoryName);
        }

        public new WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
        {
            return base.GetAppVirtualDirectories(siteId);
        }

        public new WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
        {
            return base.GetAppVirtualDirectory(siteId, directoryName);
        }

        public new void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            base.CreateAppVirtualDirectory(siteId, directory);
        }

        public new void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            base.CreateEnterpriseStorageAppVirtualDirectory(siteId, directory);
        }

        public new void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            base.UpdateAppVirtualDirectory(siteId, directory);
        }

        public new void DeleteAppVirtualDirectory(string siteId, string directoryName)
        {
            base.DeleteAppVirtualDirectory(siteId, directoryName);
        }

        public new bool IsFrontPageSystemInstalled()
        {
            return base.IsFrontPageSystemInstalled();
        }

        public new bool IsFrontPageInstalled(string siteId)
        {
            return base.IsFrontPageInstalled(siteId);
        }

        public new bool InstallFrontPage(string siteId, string username, string password)
        {
            return base.InstallFrontPage(siteId, username, password);
        }

        public new void UninstallFrontPage(string siteId, string username)
        {
            base.UninstallFrontPage(siteId, username);
        }

        public new void ChangeFrontPagePassword(string username, string password)
        {
            base.ChangeFrontPagePassword(username, password);
        }

        public new bool IsColdFusionSystemInstalled()
        {
            return base.IsColdFusionSystemInstalled();
        }

        public new void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
        {
            base.GrantWebSiteAccess(path, siteId, permission);
        }

        public new void InstallSecuredFolders(string siteId)
        {
            base.InstallSecuredFolders(siteId);
        }

        public new void UninstallSecuredFolders(string siteId)
        {
            base.UninstallSecuredFolders(siteId);
        }

        public new List<WebFolder> GetFolders(string siteId)
        {
            return base.GetFolders(siteId);
        }

        public new WebFolder GetFolder(string siteId, string folderPath)
        {
            return base.GetFolder(siteId, folderPath);
        }

        public new void UpdateFolder(string siteId, WebFolder folder)
        {
            base.UpdateFolder(siteId, folder);
        }

        public new void DeleteFolder(string siteId, string folderPath)
        {
            base.DeleteFolder(siteId, folderPath);
        }

        public new List<WebUser> GetUsers(string siteId)
        {
            return base.GetUsers(siteId);
        }

        public new WebUser GetUser(string siteId, string userName)
        {
            return base.GetUser(siteId, userName);
        }

        public new void UpdateUser(string siteId, WebUser user)
        {
            base.UpdateUser(siteId, user);
        }

        public new void DeleteUser(string siteId, string userName)
        {
            base.DeleteUser(siteId, userName);
        }

        public new List<WebGroup> GetGroups(string siteId)
        {
            return base.GetGroups(siteId);
        }

        public new WebGroup GetGroup(string siteId, string groupName)
        {
            return base.GetGroup(siteId, groupName);
        }

        public new void UpdateGroup(string siteId, WebGroup group)
        {
            base.UpdateGroup(siteId, group);
        }

        public new void DeleteGroup(string siteId, string groupName)
        {
            base.DeleteGroup(siteId, groupName);
        }

        public new HeliconApeStatus GetHeliconApeStatus(string siteId)
        {
            return base.GetHeliconApeStatus(siteId);
        }

        public new void InstallHeliconApe(string ServiceId)
        {
            base.InstallHeliconApe(ServiceId);
        }

        public new void EnableHeliconApe(string siteId)
        {
            base.EnableHeliconApe(siteId);
        }

        public new void DisableHeliconApe(string siteId)
        {
            base.DisableHeliconApe(siteId);
        }

        public new List<HtaccessFolder> GetHeliconApeFolders(string siteId)
        {
            return base.GetHeliconApeFolders(siteId);
        }

        public new HtaccessFolder GetHeliconApeHttpdFolder()
        {
            return base.GetHeliconApeHttpdFolder();
        }

        public new HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
        {
            return base.GetHeliconApeFolder(siteId, folderPath);
        }

        public new void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder)
        {
            base.UpdateHeliconApeFolder(siteId, folder);
        }

        public new void UpdateHeliconApeHttpdFolder(HtaccessFolder folder)
        {
            base.UpdateHeliconApeHttpdFolder(folder);
        }

        public new void DeleteHeliconApeFolder(string siteId, string folderPath)
        {
            base.DeleteHeliconApeFolder(siteId, folderPath);
        }

        public new List<HtaccessUser> GetHeliconApeUsers(string siteId)
        {
            return base.GetHeliconApeUsers(siteId);
        }

        public new HtaccessUser GetHeliconApeUser(string siteId, string userName)
        {
            return base.GetHeliconApeUser(siteId, userName);
        }

        public new void UpdateHeliconApeUser(string siteId, HtaccessUser user)
        {
            base.UpdateHeliconApeUser(siteId, user);
        }

        public new void DeleteHeliconApeUser(string siteId, string userName)
        {
            base.DeleteHeliconApeUser(siteId, userName);
        }

        public new List<WebGroup> GetHeliconApeGroups(string siteId)
        {
            return base.GetHeliconApeGroups(siteId);
        }

        public new WebGroup GetHeliconApeGroup(string siteId, string groupName)
        {
            return base.GetHeliconApeGroup(siteId, groupName);
        }

        public new void UpdateHeliconApeGroup(string siteId, WebGroup group)
        {
            base.UpdateHeliconApeGroup(siteId, group);
        }

        public new void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword)
        {
            base.GrantWebDeployPublishingAccess(siteId, accountName, accountPassword);
        }

        public new void RevokeWebDeployPublishingAccess(string siteId, string accountName)
        {
            base.RevokeWebDeployPublishingAccess(siteId, accountName);
        }

        public new void DeleteHeliconApeGroup(string siteId, string groupName)
        {
            base.DeleteHeliconApeGroup(siteId, groupName);
        }

        public new WebAppVirtualDirectory[] GetZooApplications(string siteId)
        {
            return base.GetZooApplications(siteId);
        }

        public new StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
        {
            return base.SetZooEnvironmentVariable(siteId, appName, envName, envValue);
        }

        public new StringResultObject SetZooConsoleEnabled(string siteId, string appName)
        {
            return base.SetZooConsoleEnabled(siteId, appName);
        }

        public new StringResultObject SetZooConsoleDisabled(string siteId, string appName)
        {
            return base.SetZooConsoleDisabled(siteId, appName);
        }

        public new bool CheckLoadUserProfile()
        {
            return base.CheckLoadUserProfile();
        }

        public new void EnableLoadUserProfile()
        {
            base.EnableLoadUserProfile();
        }

        public new void InitFeeds(int UserId, string[] feeds)
        {
            base.InitFeeds(UserId, feeds);
        }

        public new void SetResourceLanguage(int UserId, string resourceLanguage)
        {
            base.SetResourceLanguage(UserId, resourceLanguage);
        }

        public new GalleryLanguagesResult GetGalleryLanguages(int UserId)
        {
            return base.GetGalleryLanguages(UserId);
        }

        public new GalleryCategoriesResult GetGalleryCategories(int UserId)
        {
            return base.GetGalleryCategories(UserId);
        }

        public new GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
        {
            return base.GetGalleryApplications(UserId, categoryId);
        }

        public new GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            return base.GetGalleryApplicationsFiltered(UserId, pattern);
        }

        public new bool IsMsDeployInstalled()
        {
            return base.IsMsDeployInstalled();
        }

        public new GalleryApplicationResult GetGalleryApplication(int UserId, string id)
        {
            return base.GetGalleryApplication(UserId, id);
        }

        public new GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
        {
            return base.GetGalleryApplicationStatus(UserId, id);
        }

        public new GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
        {
            return base.DownloadGalleryApplication(UserId, id);
        }

        public new DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
        {
            return base.GetGalleryApplicationParameters(UserId, id);
        }

        public new StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
        {
            return base.InstallGalleryApplication(UserId, id, updatedValues, languageId);
        }

        public new bool CheckWebManagementAccountExists(string accountName)
        {
            return base.CheckWebManagementAccountExists(accountName);
        }

        public new ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
        {
            return base.CheckWebManagementPasswordComplexity(accountPassword);
        }

        public new void GrantWebManagementAccess(string siteId, string accountName, string accountPassword)
        {
            base.GrantWebManagementAccess(siteId, accountName, accountPassword);
        }

        public new void RevokeWebManagementAccess(string siteId, string accountName)
        {
            base.RevokeWebManagementAccess(siteId, accountName);
        }

        public new void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
        {
            base.ChangeWebManagementAccessPassword(accountName, accountPassword);
        }

        public new SSLCertificate generateCSR(SSLCertificate certificate)
        {
            return base.generateCSR(certificate);
        }

        public new SSLCertificate generateRenewalCSR(SSLCertificate certificate)
        {
            return base.generateRenewalCSR(certificate);
        }

        public new SSLCertificate getCertificate(WebSite site)
        {
            return base.getCertificate(site);
        }

        public new SSLCertificate installCertificate(SSLCertificate certificate, WebSite website)
        {
            return base.installCertificate(certificate, website);
        }

        public new String LEinstallCertificate(WebSite website, string email)
        {
            return base.LEinstallCertificate(website, email);
        }

        public new SSLCertificate installPFX(byte[] certificate, string password, WebSite website)
        {
            return base.installPFX(certificate, password, website);
        }

        public new byte[] exportCertificate(string serialNumber, string password)
        {
            return base.exportCertificate(serialNumber, password);
        }

        public new List<SSLCertificate> getServerCertificates()
        {
            return base.getServerCertificates();
        }

        public new ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
        {
            return base.DeleteCertificate(certificate, website);
        }

        public new SSLCertificate ImportCertificate(WebSite website)
        {
            return base.ImportCertificate(website);
        }

        public new bool CheckCertificate(WebSite webSite)
        {
            return base.CheckCertificate(webSite);
        }

        public new bool GetDirectoryBrowseEnabled(string siteId)
        {
            return base.GetDirectoryBrowseEnabled(siteId);
        }

        public new void SetDirectoryBrowseEnabled(string siteId, bool enabled)
        {
            base.SetDirectoryBrowseEnabled(siteId, enabled);
        }
    }
}
#endif