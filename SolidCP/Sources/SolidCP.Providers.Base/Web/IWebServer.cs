// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace SolidCP.Providers.Web
{
	/// <summary>
	/// Summary description for IWebServer.
	/// </summary>
	public interface IWebServer
	{
		// sites
		void ChangeSiteState(string siteId, ServerState state);
        ServerState GetSiteState(string siteId);
		bool SiteExists(string siteId);
		string[] GetSites();
        string GetSiteId(string siteName);
		WebSite GetSite(string siteId);
        string[] GetSitesAccounts(string[] siteIds);
        ServerBinding[] GetSiteBindings(string siteId);
		string CreateSite(WebSite site);
		void UpdateSite(WebSite site);
        void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed);
		void DeleteSite(string siteId);

        // AppPool
        void ChangeAppPoolState(string siteId, AppPoolState state);
        AppPoolState GetAppPoolState(string siteId);

        // virtual directories
        bool VirtualDirectoryExists(string siteId, string directoryName);
        WebVirtualDirectory[] GetVirtualDirectories(string siteId);
        WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName);
        void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory);
        void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory);
        void DeleteVirtualDirectory(string siteId, string directoryName);

        // virtual directories
        bool AppVirtualDirectoryExists(string siteId, string directoryName);
		WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId);
		WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName);
		void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
        void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
		void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
		void DeleteAppVirtualDirectory(string siteId, string directoryName);

		// FrontPage
		bool IsFrontPageSystemInstalled();
		bool IsFrontPageInstalled(string siteId);
		bool InstallFrontPage(string siteId, string username, string password);
		void UninstallFrontPage(string siteId, string username);
        void ChangeFrontPagePassword(string username, string password);

        //ColdFusion
	    bool IsColdFusionSystemInstalled();
	    //bool IsColdFusionEnabled(string siteId);

        // permissions
        void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission);

        // Secured folders
        void InstallSecuredFolders(string siteId);
        void UninstallSecuredFolders(string siteId);

        List<WebFolder> GetFolders(string siteId);
        WebFolder GetFolder(string siteId, string folderPath);
        void UpdateFolder(string siteId, WebFolder folder);
        void DeleteFolder(string siteId, string folderPath);

        List<WebUser> GetUsers(string siteId);
        WebUser GetUser(string siteId, string userName);
        void UpdateUser(string siteId, WebUser user);
        void DeleteUser(string siteId, string userName);

        List<WebGroup> GetGroups(string siteId);
        WebGroup GetGroup(string siteId, string groupName);
        void UpdateGroup(string siteId, WebGroup group);
        void DeleteGroup(string siteId, string groupName);

        // Helicon Ape
        HeliconApeStatus GetHeliconApeStatus(string siteId);
        void InstallHeliconApe(string ServiceId);
        void EnableHeliconApe(string siteId);
        void DisableHeliconApe(string siteId);
        List<HtaccessFolder> GetHeliconApeFolders(string siteId);
        HtaccessFolder GetHeliconApeHttpdFolder();
        HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath);
        void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder);
        void UpdateHeliconApeHttpdFolder(HtaccessFolder folder);
        void DeleteHeliconApeFolder(string siteId, string folderPath);

        List<HtaccessUser> GetHeliconApeUsers(string siteId);
        HtaccessUser GetHeliconApeUser(string siteId, string userName);
        void UpdateHeliconApeUser(string siteId, HtaccessUser user);
        void DeleteHeliconApeUser(string siteId, string userName);

        List<WebGroup> GetHeliconApeGroups(string siteId);
        WebGroup GetHeliconApeGroup(string siteId, string groupName);
        void UpdateHeliconApeGroup(string siteId, WebGroup group);
        void DeleteHeliconApeGroup(string siteId, string groupName);

        // Helicon Zoo
	    WebAppVirtualDirectory[] GetZooApplications(string siteId);
        StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue);
        StringResultObject SetZooConsoleEnabled(string siteId, string appName);
        StringResultObject SetZooConsoleDisabled(string siteId, string appName);


		// web app gallery
        bool CheckLoadUserProfile();
        void EnableLoadUserProfile();
        void InitFeeds(int UserId, string[] feeds);
        void SetResourceLanguage(int UserId, string resourceLanguage);
		bool IsMsDeployInstalled();
        GalleryLanguagesResult GetGalleryLanguages(int UserId);
        GalleryCategoriesResult GetGalleryCategories(int UserId);
        GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId);
        GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern);
        GalleryApplicationResult GetGalleryApplication(int UserId, string id);
        GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id);
        GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id);
        DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id);
        StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId);

		//
		void GrantWebManagementAccess(string siteId, string accountName, string accountPassword);
		void RevokeWebManagementAccess(string siteId, string accountName);
		void ChangeWebManagementAccessPassword(string accountName, string accountPassword);
		bool CheckWebManagementAccountExists(string accountName);
		ResultObject CheckWebManagementPasswordComplexity(string accountPassword);
		// Web Deploy Publishing Access
		void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword);
		void RevokeWebDeployPublishingAccess(string siteId, string accountName);

        //SSL
        SSLCertificate generateCSR(SSLCertificate certificate);
        SSLCertificate generateRenewalCSR(SSLCertificate certificate);
        SSLCertificate installCertificate(SSLCertificate certificate, WebSite website);
        SSLCertificate getCertificate(WebSite site);
        SSLCertificate installPFX(byte[] certificate, string password, WebSite website);
        byte[] exportCertificate(string serialNumber, string password);
        List<SSLCertificate> getServerCertificates();
        ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website);
        SSLCertificate ImportCertificate(WebSite website);
        bool CheckCertificate(WebSite webSite);
        string LEinstallCertificate(WebSite website, string email);

        //Directory Browseing
        bool GetDirectoryBrowseEnabled(string siteId);
        void SetDirectoryBrowseEnabled(string siteId, bool enabled);
    }
}
