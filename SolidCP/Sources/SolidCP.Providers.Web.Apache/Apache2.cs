using System;
using System.Collections.Generic;
using System.Text;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Web;
using SolidCP.Providers.WebAppGallery;

namespace SolidCP.Providers.Web
{
	public class Apache2 : IWebServer
	{
		public bool AppVirtualDirectoryExists(string siteId, string directoryName)
		{
			throw new NotImplementedException();
		}

		public void ChangeAppPoolState(string siteId, AppPoolState state)
		{
			throw new NotImplementedException();
		}

		public void ChangeFrontPagePassword(string username, string password)
		{
			throw new NotImplementedException();
		}

		public void ChangeSiteState(string siteId, ServerState state)
		{
			throw new NotImplementedException();
		}

		public void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
		{
			throw new NotImplementedException();
		}

		public bool CheckCertificate(WebSite webSite)
		{
			throw new NotImplementedException();
		}

		public bool CheckLoadUserProfile()
		{
			throw new NotImplementedException();
		}

		public bool CheckWebManagementAccountExists(string accountName)
		{
			throw new NotImplementedException();
		}

		public ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
		{
			throw new NotImplementedException();
		}

		public void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			throw new NotImplementedException();
		}

		public void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			throw new NotImplementedException();
		}

		public string CreateSite(WebSite site)
		{
			throw new NotImplementedException();
		}

		public void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
		{
			throw new NotImplementedException();
		}

		public void DeleteAppVirtualDirectory(string siteId, string directoryName)
		{
			throw new NotImplementedException();
		}

		public ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
		{
			throw new NotImplementedException();
		}

		public void DeleteFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public void DeleteGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public void DeleteHeliconApeFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public void DeleteHeliconApeGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public void DeleteHeliconApeUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public void DeleteSite(string siteId)
		{
			throw new NotImplementedException();
		}

		public void DeleteUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public void DeleteVirtualDirectory(string siteId, string directoryName)
		{
			throw new NotImplementedException();
		}

		public void DisableHeliconApe(string siteId)
		{
			throw new NotImplementedException();
		}

		public GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public void EnableHeliconApe(string siteId)
		{
			throw new NotImplementedException();
		}

		public void EnableLoadUserProfile()
		{
			throw new NotImplementedException();
		}

		public byte[] ExportCertificate(string serialNumber, string password)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate GenerateCSR(SSLCertificate certificate)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate generateRenewalCSR(SSLCertificate certificate)
		{
			throw new NotImplementedException();
		}

		public AppPoolState GetAppPoolState(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate getCertificate(WebSite site)
		{
			throw new NotImplementedException();
		}

		public bool GetDirectoryBrowseEnabled(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebFolder GetFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public List<WebFolder> GetFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public GalleryApplicationResult GetGalleryApplication(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
		{
			throw new NotImplementedException();
		}

		public GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
		{
			throw new NotImplementedException();
		}

		public GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public GalleryCategoriesResult GetGalleryCategories(int UserId)
		{
			throw new NotImplementedException();
		}

		public GalleryLanguagesResult GetGalleryLanguages(int UserId)
		{
			throw new NotImplementedException();
		}

		public WebGroup GetGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public List<WebGroup> GetGroups(string siteId)
		{
			throw new NotImplementedException();
		}

		public HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public List<HtaccessFolder> GetHeliconApeFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebGroup GetHeliconApeGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public List<WebGroup> GetHeliconApeGroups(string siteId)
		{
			throw new NotImplementedException();
		}

		public HtaccessFolder GetHeliconApeHttpdFolder()
		{
			throw new NotImplementedException();
		}

		public HeliconApeStatus GetHeliconApeStatus(string siteId)
		{
			throw new NotImplementedException();
		}

		public HtaccessUser GetHeliconApeUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public List<HtaccessUser> GetHeliconApeUsers(string siteId)
		{
			throw new NotImplementedException();
		}

		public List<SSLCertificate> GetServerCertificates()
		{
			throw new NotImplementedException();
		}

		public WebSite GetSite(string siteId)
		{
			throw new NotImplementedException();
		}

		public ServerBinding[] GetSiteBindings(string siteId)
		{
			throw new NotImplementedException();
		}

		public string GetSiteId(string siteName)
		{
			throw new NotImplementedException();
		}

		public string[] GetSites()
		{
			throw new NotImplementedException();
		}

		public string[] GetSitesAccounts(string[] siteIds)
		{
			throw new NotImplementedException();
		}

		public ServerState GetSiteState(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebUser GetUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public List<WebUser> GetUsers(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebVirtualDirectory[] GetVirtualDirectories(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
		{
			throw new NotImplementedException();
		}

		public WebAppVirtualDirectory[] GetZooApplications(string siteId)
		{
			throw new NotImplementedException();
		}

		public void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword)
		{
			throw new NotImplementedException();
		}

		public void GrantWebManagementAccess(string siteId, string accountName, string accountPassword)
		{
			throw new NotImplementedException();
		}

		public void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate ImportCertificate(WebSite website)
		{
			throw new NotImplementedException();
		}

		public void InitFeeds(int UserId, string[] feeds)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate InstallCertificate(SSLCertificate certificate, WebSite website)
		{
			throw new NotImplementedException();
		}

		public bool InstallFrontPage(string siteId, string username, string password)
		{
			throw new NotImplementedException();
		}

		public StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
		{
			throw new NotImplementedException();
		}

		public void InstallHeliconApe(string ServiceId)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate InstallPFX(byte[] certificate, string password, WebSite website)
		{
			throw new NotImplementedException();
		}

		public void InstallSecuredFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public bool IsColdFusionSystemInstalled()
		{
			throw new NotImplementedException();
		}

		public bool IsFrontPageInstalled(string siteId)
		{
			throw new NotImplementedException();
		}

		public bool IsFrontPageSystemInstalled()
		{
			throw new NotImplementedException();
		}

		public bool IsMsDeployInstalled()
		{
			throw new NotImplementedException();
		}

		public string LEInstallCertificate(WebSite website, string email)
		{
			throw new NotImplementedException();
		}

		public void RevokeWebDeployPublishingAccess(string siteId, string accountName)
		{
			throw new NotImplementedException();
		}

		public void RevokeWebManagementAccess(string siteId, string accountName)
		{
			throw new NotImplementedException();
		}

		public void SetDirectoryBrowseEnabled(string siteId, bool enabled)
		{
			throw new NotImplementedException();
		}

		public void SetResourceLanguage(int UserId, string resourceLanguage)
		{
			throw new NotImplementedException();
		}

		public StringResultObject SetZooConsoleDisabled(string siteId, string appName)
		{
			throw new NotImplementedException();
		}

		public StringResultObject SetZooConsoleEnabled(string siteId, string appName)
		{
			throw new NotImplementedException();
		}

		public StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
		{
			throw new NotImplementedException();
		}

		public bool SiteExists(string siteId)
		{
			throw new NotImplementedException();
		}

		public void UninstallFrontPage(string siteId, string username)
		{
			throw new NotImplementedException();
		}

		public void UninstallSecuredFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			throw new NotImplementedException();
		}

		public void UpdateFolder(string siteId, WebFolder folder)
		{
			throw new NotImplementedException();
		}

		public void UpdateGroup(string siteId, WebGroup group)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeGroup(string siteId, WebGroup group)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeHttpdFolder(HtaccessFolder folder)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeUser(string siteId, HtaccessUser user)
		{
			throw new NotImplementedException();
		}

		public void UpdateSite(WebSite site)
		{
			throw new NotImplementedException();
		}

		public void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
		{
			throw new NotImplementedException();
		}

		public void UpdateUser(string siteId, WebUser user)
		{
			throw new NotImplementedException();
		}

		public void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
		{
			throw new NotImplementedException();
		}

		public bool VirtualDirectoryExists(string siteId, string directoryName)
		{
			throw new NotImplementedException();
		}
	}
}
