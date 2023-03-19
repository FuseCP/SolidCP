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

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for WebServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class WebServer : HostingServiceProviderWebService, IWebServer
    {
        private IWebServer WebProvider
        {
            get { return (IWebServer)Provider; }
        }

        #region Web Sites
        [WebMethod, SoapHeader("settings")]
        public void ChangeSiteState(string siteId, ServerState state)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeSiteState", ProviderSettings.ProviderName);
                WebProvider.ChangeSiteState(siteId, state);
                Log.WriteEnd("'{0}' ChangeSiteState", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeSiteState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ServerState GetSiteState(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSiteState", ProviderSettings.ProviderName);
                ServerState result = WebProvider.GetSiteState(siteId);
                Log.WriteEnd("'{0}' GetSiteState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSiteState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string GetSiteId(string siteName)
        {
            try
            {
                Log.WriteStart("'{0}' GetSiteId", ProviderSettings.ProviderName);
                string result = WebProvider.GetSiteId(siteName);
                Log.WriteEnd("'{0}' GetSiteId", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSiteId", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetSitesAccounts(string[] siteIds)
        {
            try
            {
                Log.WriteStart("'{0}' GetSitesAccounts", ProviderSettings.ProviderName);
                string[] result = WebProvider.GetSitesAccounts(siteIds);
                Log.WriteEnd("'{0}' GetSitesAccounts", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSitesAccounts", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SiteExists(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' SiteIdExists", ProviderSettings.ProviderName);
                bool result = WebProvider.SiteExists(siteId);
                Log.WriteEnd("'{0}' SiteIdExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SiteIdExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetSites()
        {
            try
            {
                Log.WriteStart("'{0}' GetSites", ProviderSettings.ProviderName);
                string[] result = WebProvider.GetSites();
                Log.WriteEnd("'{0}' GetSites", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSites", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebSite GetSite(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSite", ProviderSettings.ProviderName);
                WebSite result = WebProvider.GetSite(siteId);
                Log.WriteEnd("'{0}' GetSite", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ServerBinding[] GetSiteBindings(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSiteBindings", ProviderSettings.ProviderName);
                ServerBinding[] result = WebProvider.GetSiteBindings(siteId);
                Log.WriteEnd("'{0}' GetSiteBindings", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSiteBindings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string CreateSite(WebSite site)
        {
            try
            {
                Log.WriteStart("'{0}' CreateSite", ProviderSettings.ProviderName);
                string result = WebProvider.CreateSite(site);
                Log.WriteEnd("'{0}' CreateSite", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateSite(WebSite site)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateSite", ProviderSettings.ProviderName);
                WebProvider.UpdateSite(site);
                Log.WriteEnd("'{0}' UpdateSite", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateSiteBindings", ProviderSettings.ProviderName);
                WebProvider.UpdateSiteBindings(siteId, bindings, emptyBindingsAllowed);
                Log.WriteEnd("'{0}' UpdateSiteBindings", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateSiteBindings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteSite(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSite", ProviderSettings.ProviderName);
                WebProvider.DeleteSite(siteId);
                Log.WriteEnd("'{0}' DeleteSite", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        // AppPool
        [WebMethod, SoapHeader("settings")]
        public void ChangeAppPoolState(string siteId, AppPoolState state)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeAppPoolState", ProviderSettings.ProviderName);
                WebProvider.ChangeAppPoolState(siteId, state);
                Log.WriteEnd("'{0}' ChangeAppPoolState", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeAppPoolState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public AppPoolState GetAppPoolState(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetAppPoolState", ProviderSettings.ProviderName);
                AppPoolState result = WebProvider.GetAppPoolState(siteId);
                Log.WriteEnd("'{0}' GetAppPoolState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAppPoolState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Virtual Directories
        [WebMethod, SoapHeader("settings")]
        public bool VirtualDirectoryExists(string siteId, string directoryName)
        {
            try
            {
                Log.WriteStart("'{0}' VirtualDirectoryExists", ProviderSettings.ProviderName);
                bool result = WebProvider.VirtualDirectoryExists(siteId, directoryName);
                Log.WriteEnd("'{0}' VirtualDirectoryExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' VirtualDirectoryExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebVirtualDirectory[] GetVirtualDirectories(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualDirectories", ProviderSettings.ProviderName);
                WebVirtualDirectory[] result = WebProvider.GetVirtualDirectories(siteId);
                Log.WriteEnd("'{0}' GetVirtualDirectories", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualDirectories", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualDirectory", ProviderSettings.ProviderName);
                WebVirtualDirectory result = WebProvider.GetVirtualDirectory(siteId, directoryName);
                Log.WriteEnd("'{0}' GetVirtualDirectory", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            try
            {
                Log.WriteStart("'{0}' CreateVirtualDirectory", ProviderSettings.ProviderName);
                WebProvider.CreateVirtualDirectory(siteId, directory);
                Log.WriteEnd("'{0}' CreateVirtualDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }



        [WebMethod, SoapHeader("settings")]
        public void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateVirtualDirectory", ProviderSettings.ProviderName);
                WebProvider.UpdateVirtualDirectory(siteId, directory);
                Log.WriteEnd("'{0}' UpdateVirtualDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteVirtualDirectory(string siteId, string directoryName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteVirtualDirectory", ProviderSettings.ProviderName);
                WebProvider.DeleteVirtualDirectory(siteId, directoryName);
                Log.WriteEnd("'{0}' DeleteVirtualDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }






        [WebMethod, SoapHeader("settings")]
        public bool AppVirtualDirectoryExists(string siteId, string directoryName)
        {
            try
            {
                Log.WriteStart("'{0}' AppVirtualDirectoryExists", ProviderSettings.ProviderName);
                bool result = WebProvider.AppVirtualDirectoryExists(siteId, directoryName);
                Log.WriteEnd("'{0}' AppVirtualDirectoryExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AppVirtualDirectoryExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetAppVirtualDirectories", ProviderSettings.ProviderName);
                WebAppVirtualDirectory[] result = WebProvider.GetAppVirtualDirectories(siteId);
                Log.WriteEnd("'{0}' GetAppVirtualDirectories", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAppVirtualDirectories", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
        {
            try
            {
                Log.WriteStart("'{0}' GetAppVirtualDirectory", ProviderSettings.ProviderName);
                WebAppVirtualDirectory result = WebProvider.GetAppVirtualDirectory(siteId, directoryName);
                Log.WriteEnd("'{0}' GetAppVirtualDirectory", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAppVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            try
            {
                Log.WriteStart("'{0}' CreateAppVirtualDirectory", ProviderSettings.ProviderName);
                WebProvider.CreateAppVirtualDirectory(siteId, directory);
                Log.WriteEnd("'{0}' CreateAppVirtualDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateAppVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            try
            {
                Log.WriteStart("'{0}' CreateEnterpriseStorageAppVirtualDirectory", ProviderSettings.ProviderName);
                WebProvider.CreateEnterpriseStorageAppVirtualDirectory(siteId, directory);
                Log.WriteEnd("'{0}' CreateEnterpriseStorageAppVirtualDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateEnterpriseStorageAppVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateAppVirtualDirectory", ProviderSettings.ProviderName);
                WebProvider.UpdateAppVirtualDirectory(siteId, directory);
                Log.WriteEnd("'{0}' UpdateAppVirtualDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateAppVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteAppVirtualDirectory(string siteId, string directoryName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteAppVirtualDirectory", ProviderSettings.ProviderName);
                WebProvider.DeleteAppVirtualDirectory(siteId, directoryName);
                Log.WriteEnd("'{0}' DeleteAppVirtualDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteAppVirtualDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region FrontPage
        [WebMethod, SoapHeader("settings")]
        public bool IsFrontPageSystemInstalled()
        {
            try
            {
                Log.WriteStart("'{0}' IsFrontPageSystemInstalled", ProviderSettings.ProviderName);
                bool result = WebProvider.IsFrontPageSystemInstalled();
                Log.WriteEnd("'{0}' IsFrontPageSystemInstalled", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' IsFrontPageSystemInstalled", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool IsFrontPageInstalled(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' IsFrontPageInstalled", ProviderSettings.ProviderName);
                bool result = WebProvider.IsFrontPageInstalled(siteId);
                Log.WriteEnd("'{0}' IsFrontPageInstalled", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' IsFrontPageInstalled", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool InstallFrontPage(string siteId, string username, string password)
        {
            try
            {
                Log.WriteStart("'{0}' InstallFrontPage", ProviderSettings.ProviderName);
                bool result = WebProvider.InstallFrontPage(siteId, username, password);
                Log.WriteEnd("'{0}' InstallFrontPage", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InstallFrontPage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UninstallFrontPage(string siteId, string username)
        {
            try
            {
                Log.WriteStart("'{0}' UninstallFrontPage", ProviderSettings.ProviderName);
                WebProvider.UninstallFrontPage(siteId, username);
                Log.WriteEnd("'{0}' UninstallFrontPage", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UninstallFrontPage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ChangeFrontPagePassword(string username, string password)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeFrontPagePassword", ProviderSettings.ProviderName);
                WebProvider.ChangeFrontPagePassword(username, password);
                Log.WriteEnd("'{0}' ChangeFrontPagePassword", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeFrontPagePassword", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region ColdFusion
        [WebMethod, SoapHeader("settings")]
        public bool IsColdFusionSystemInstalled()
        {
            try
            {
                Log.WriteStart("'{0}' IsColdFusionSystemInstalled", ProviderSettings.ProviderName);
                bool result = WebProvider.IsColdFusionSystemInstalled();
                Log.WriteEnd("'{0}' IsColdFusionSystemInstalled", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' IsColdFusionSystemInstalled", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Permissions
        [WebMethod, SoapHeader("settings")]
        public void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
        {
            try
            {
                Log.WriteStart("'{0}' GrantWebSiteAccess", ProviderSettings.ProviderName);
                WebProvider.GrantWebSiteAccess(path, siteId, permission);
                Log.WriteEnd("'{0}' GrantWebSiteAccess", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GrantWebSiteAccess", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Secured Folders
        [WebMethod, SoapHeader("settings")]
        public void InstallSecuredFolders(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' InstallSecuredFolders", ProviderSettings.ProviderName);
                WebProvider.InstallSecuredFolders(siteId);
                Log.WriteEnd("'{0}' InstallSecuredFolders", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InstallSecuredFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UninstallSecuredFolders(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' UninstallSecuredFolders", ProviderSettings.ProviderName);
                WebProvider.UninstallSecuredFolders(siteId);
                Log.WriteEnd("'{0}' UninstallSecuredFolders", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UninstallSecuredFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<WebFolder> GetFolders(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolders", ProviderSettings.ProviderName);
                List<WebFolder> result = WebProvider.GetFolders(siteId);
                Log.WriteEnd("'{0}' GetFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebFolder GetFolder(string siteId, string folderPath)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolder", ProviderSettings.ProviderName);
                WebFolder result = WebProvider.GetFolder(siteId, folderPath);
                Log.WriteEnd("'{0}' GetFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateFolder(string siteId, WebFolder folder)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateFolder", ProviderSettings.ProviderName);
                WebProvider.UpdateFolder(siteId, folder);
                Log.WriteEnd("'{0}' UpdateFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteFolder(string siteId, string folderPath)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteFolder", ProviderSettings.ProviderName);
                WebProvider.DeleteFolder(siteId, folderPath);
                Log.WriteEnd("'{0}' DeleteFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Secured Users
        [WebMethod, SoapHeader("settings")]
        public List<WebUser> GetUsers(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetUsers", ProviderSettings.ProviderName);
                List<WebUser> result = WebProvider.GetUsers(siteId);
                Log.WriteEnd("'{0}' GetUsers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetUsers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebUser GetUser(string siteId, string userName)
        {
            try
            {
                Log.WriteStart("'{0}' GetUser", ProviderSettings.ProviderName);
                WebUser result = WebProvider.GetUser(siteId, userName);
                Log.WriteEnd("'{0}' GetUser", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateUser(string siteId, WebUser user)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateUser", ProviderSettings.ProviderName);
                WebProvider.UpdateUser(siteId, user);
                Log.WriteEnd("'{0}' UpdateUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteUser(string siteId, string userName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteUser", ProviderSettings.ProviderName);
                WebProvider.DeleteUser(siteId, userName);
                Log.WriteEnd("'{0}' DeleteUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Secured Groups
        [WebMethod, SoapHeader("settings")]
        public List<WebGroup> GetGroups(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetGroups", ProviderSettings.ProviderName);
                List<WebGroup> result = WebProvider.GetGroups(siteId);
                Log.WriteEnd("'{0}' GetGroups", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGroups", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebGroup GetGroup(string siteId, string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' GetGroup", ProviderSettings.ProviderName);
                WebGroup result = WebProvider.GetGroup(siteId, groupName);
                Log.WriteEnd("'{0}' GetGroup", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateGroup(string siteId, WebGroup group)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateGroup", ProviderSettings.ProviderName);
                WebProvider.UpdateGroup(siteId, group);
                Log.WriteEnd("'{0}' UpdateGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteGroup(string siteId, string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteGroup", ProviderSettings.ProviderName);
                WebProvider.DeleteGroup(siteId, groupName);
                Log.WriteEnd("'{0}' DeleteGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Helicon Ape
        [WebMethod, SoapHeader("settings")]
        public HeliconApeStatus GetHeliconApeStatus(string siteId)
        {
            HeliconApeStatus status;
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeStatus", ProviderSettings.ProviderName);
                status = WebProvider.GetHeliconApeStatus(siteId);
                Log.WriteEnd("'{0}' GetHeliconApeStatus", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHeliconApeStatus", ProviderSettings.ProviderName), ex);
                throw;
            }

            return status;
        }


        [WebMethod, SoapHeader("settings")]
        public void InstallHeliconApe(string ServiceId)
        {
            try
            {
                Log.WriteStart("'{0}' InstallHeliconApe", ProviderSettings.ProviderName);
                WebProvider.InstallHeliconApe(ServiceId);
                Log.WriteEnd("'{0}' InstallHeliconApe", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InstallHeliconApe", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public void EnableHeliconApe(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' EnableHeliconApe", ProviderSettings.ProviderName);
                WebProvider.EnableHeliconApe(siteId);
                Log.WriteEnd("'{0}' EnableHeliconApe", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' EnableHeliconApe", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DisableHeliconApe(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' DisableHeliconApe", ProviderSettings.ProviderName);
                WebProvider.DisableHeliconApe(siteId);
                Log.WriteEnd("'{0}' DisableHeliconApe", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DisableHeliconApe", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<HtaccessFolder> GetHeliconApeFolders(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeFolders", ProviderSettings.ProviderName);
                List<HtaccessFolder> result = WebProvider.GetHeliconApeFolders(siteId);
                Log.WriteEnd("'{0}' GetHeliconApeFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHeliconApeFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public HtaccessFolder GetHeliconApeHttpdFolder()
        {
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeFolder", ProviderSettings.ProviderName);
                HtaccessFolder result = WebProvider.GetHeliconApeHttpdFolder();
                Log.WriteEnd("'{0}' GetHeliconApeFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHeliconApeFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
        {
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeFolder", ProviderSettings.ProviderName);
                HtaccessFolder result = WebProvider.GetHeliconApeFolder(siteId, folderPath);
                Log.WriteEnd("'{0}' GetHeliconApeFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHeliconApeFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateHeliconApeFolder", ProviderSettings.ProviderName);
                WebProvider.UpdateHeliconApeFolder(siteId, folder);
                Log.WriteEnd("'{0}' UpdateHeliconApeFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateHeliconApeFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateHeliconApeHttpdFolder(HtaccessFolder folder)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateHeliconApeHttpdFolder", ProviderSettings.ProviderName);
                WebProvider.UpdateHeliconApeHttpdFolder(folder);
                Log.WriteEnd("'{0}' UpdateHeliconApeHttpdFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateHeliconApeHttpdFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteHeliconApeFolder(string siteId, string folderPath)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteHeliconApeFolder", ProviderSettings.ProviderName);
                WebProvider.DeleteHeliconApeFolder(siteId, folderPath);
                Log.WriteEnd("'{0}' DeleteHeliconApeFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteHeliconApeFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Helicon Ape Users
        [WebMethod, SoapHeader("settings")]
        public List<HtaccessUser> GetHeliconApeUsers(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeUsers", ProviderSettings.ProviderName);
                List<HtaccessUser> result = WebProvider.GetHeliconApeUsers(siteId);
                Log.WriteEnd("'{0}' GetHeliconApeUsers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GeHeliconApetUsers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public HtaccessUser GetHeliconApeUser(string siteId, string userName)
        {
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeUser", ProviderSettings.ProviderName);
                HtaccessUser result = WebProvider.GetHeliconApeUser(siteId, userName);
                Log.WriteEnd("'{0}' GetHeliconApeUser", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHeliconApeUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateHeliconApeUser(string siteId, HtaccessUser user)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateHeliconApeUser", ProviderSettings.ProviderName);
                WebProvider.UpdateHeliconApeUser(siteId, user);
                Log.WriteEnd("'{0}' UpdateHeliconApeUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateHeliconApeUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteHeliconApeUser(string siteId, string userName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteHeliconApeUser", ProviderSettings.ProviderName);
                WebProvider.DeleteHeliconApeUser(siteId, userName);
                Log.WriteEnd("'{0}' DeleteHeliconApeUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteHeliconApeUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Helicon Ape Groups
        [WebMethod, SoapHeader("settings")]
        public List<WebGroup> GetHeliconApeGroups(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeGroups", ProviderSettings.ProviderName);
                List<WebGroup> result = WebProvider.GetHeliconApeGroups(siteId);
                Log.WriteEnd("'{0}' GetHeliconApeGroups", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHeliconApeGroups", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebGroup GetHeliconApeGroup(string siteId, string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' GetHeliconApeGroup", ProviderSettings.ProviderName);
                WebGroup result = WebProvider.GetHeliconApeGroup(siteId, groupName);
                Log.WriteEnd("'{0}' GetHeliconApeGroup", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetHeliconApeGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateHeliconApeGroup(string siteId, WebGroup group)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateHeliconApeGroup", ProviderSettings.ProviderName);
                WebProvider.UpdateHeliconApeGroup(siteId, group);
                Log.WriteEnd("'{0}' UpdateHeliconApeGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateHeliconApeGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

		[WebMethod, SoapHeader("settings")]
		public void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword)
		{
			try
			{
				Log.WriteStart("'{0}' GrantWebDeployPublishingAccess", ProviderSettings.ProviderName);
				WebProvider.GrantWebDeployPublishingAccess(siteId, accountName, accountPassword);
				Log.WriteEnd("'{0}' GrantWebDeployPublishingAccess", ProviderSettings.ProviderName);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GrantWebDeployPublishingAccess", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void RevokeWebDeployPublishingAccess(string siteId, string accountName)
		{
			try
			{
				Log.WriteStart("'{0}' RevokeWebDeployPublishingAccess", ProviderSettings.ProviderName);
				WebProvider.RevokeWebDeployPublishingAccess(siteId, accountName);
				Log.WriteEnd("'{0}' RevokeWebDeployPublishingAccess", ProviderSettings.ProviderName);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' RevokeWebDeployPublishingAccess", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public void DeleteHeliconApeGroup(string siteId, string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteHeliconApeGroup", ProviderSettings.ProviderName);
                WebProvider.DeleteHeliconApeGroup(siteId, groupName);
                Log.WriteEnd("'{0}' DeleteHeliconApeGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteHeliconApeGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

    

        #endregion

        #region Helicon Zoo

        [WebMethod, SoapHeader("settings")]
        public WebAppVirtualDirectory[] GetZooApplications(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetZooApplications", ProviderSettings.ProviderName);
                WebAppVirtualDirectory[] result = WebProvider.GetZooApplications(siteId);
                Log.WriteEnd("'{0}' GetZooApplications", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetZooApplications", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
        {
            try
            {
                Log.WriteStart("'{0}' SetZooEnvironmentVariable", ProviderSettings.ProviderName);
                StringResultObject result = WebProvider.SetZooEnvironmentVariable(siteId, appName, envName, envValue);
                Log.WriteEnd("'{0}' SetZooEnvironmentVariable", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetZooEnvironmentVariable", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public StringResultObject SetZooConsoleEnabled(string siteId, string appName)
        {
            try
            {
                Log.WriteStart("'{0}' SetZooConsoleEnabled", ProviderSettings.ProviderName);
                StringResultObject result = WebProvider.SetZooConsoleEnabled(siteId, appName);
                Log.WriteEnd("'{0}' SetZooConsoleEnabled", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetZooConsoleEnabled", ProviderSettings.ProviderName), ex);
                throw;
            }
            
        }

        [WebMethod, SoapHeader("settings")]
        public StringResultObject SetZooConsoleDisabled(string siteId, string appName)
        {
            try
            {
                Log.WriteStart("'{0}' SetZooConsoleDisabled", ProviderSettings.ProviderName);
                StringResultObject result = WebProvider.SetZooConsoleDisabled(siteId, appName);
                Log.WriteEnd("'{0}' SetZooConsoleDisabled", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetZooConsoleDisabled", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Web Application Gallery

        [WebMethod, SoapHeader("settings")]
        public bool CheckLoadUserProfile()
        {
            try
            {
                Log.WriteStart("CheckLoadUserProfile");

                bool bResult =  WebProvider.CheckLoadUserProfile();

                Log.WriteEnd("CheckLoadUserProfile");

                return bResult;
            }
            catch (Exception ex)
            {
                Log.WriteError("CheckLoadUserProfile", ex);
                throw;
            }
        }
        
        [WebMethod, SoapHeader("settings")]
        public void EnableLoadUserProfile()
        {
            try
            {
                Log.WriteStart("EnableLoadUserProfile");

                WebProvider.EnableLoadUserProfile();

                Log.WriteEnd("EnableLoadUserProfile");
            }
            catch (Exception ex)
            {
                Log.WriteError("EnableLoadUserProfile", ex);
                throw;
            }
        }
        [WebMethod, SoapHeader("settings")]
        public void InitFeeds(int UserId, string[] feeds)
        {
            try
            {
                Log.WriteStart("'{0}' InitFeeds", ProviderSettings.ProviderName);
                WebProvider.InitFeeds(UserId, feeds);
                Log.WriteEnd("'{0}' InitFeeds", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InitFeeds", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetResourceLanguage(int UserId, string resourceLanguage)
        {
            try
            {
                Log.WriteStart("'{0}' SetResourceLanguage", ProviderSettings.ProviderName);
                WebProvider.SetResourceLanguage(UserId,resourceLanguage);
                Log.WriteEnd("'{0}' SetResourceLanguage", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetResourceLanguage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public GalleryLanguagesResult GetGalleryLanguages(int UserId)
        {
            try
            {
                Log.WriteStart("'{0}' GalleryLanguagesResult", ProviderSettings.ProviderName);
                GalleryLanguagesResult result = WebProvider.GetGalleryLanguages(UserId);
                Log.WriteEnd("'{0}' GalleryLanguagesResult", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GalleryLanguagesResult", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
		public GalleryCategoriesResult GetGalleryCategories(int UserId)
		{
			try
			{
				Log.WriteStart("'{0}' GalleryCategoriesResult", ProviderSettings.ProviderName);
                GalleryCategoriesResult result = WebProvider.GetGalleryCategories(UserId);
				Log.WriteEnd("'{0}' GalleryCategoriesResult", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GalleryCategoriesResult", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
		{
			try
			{
				Log.WriteStart("'{0}' GetGalleryApplications", ProviderSettings.ProviderName);
                GalleryApplicationsResult result = WebProvider.GetGalleryApplications(UserId,categoryId);
				Log.WriteEnd("'{0}' GetGalleryApplications", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GetGalleryApplications", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            try
            {
                Log.WriteStart("'{0}' GetGalleryApplicationsFiltered", ProviderSettings.ProviderName);
                GalleryApplicationsResult result = WebProvider.GetGalleryApplicationsFiltered(UserId,pattern);
                Log.WriteEnd("'{0}' GetGalleryApplicationsFiltered", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGalleryApplicationsFiltered", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
		public bool IsMsDeployInstalled()
		{
			try
			{
				Log.WriteStart("'{0}' IsMsDeployInstalled", ProviderSettings.ProviderName);
				bool result = WebProvider.IsMsDeployInstalled();
				Log.WriteEnd("'{0}' IsMsDeployInstalled", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' IsMsDeployInstalled", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public GalleryApplicationResult GetGalleryApplication(int UserId, string id)
		{
			try
			{
				Log.WriteStart("'{0}' GetGalleryApplication", ProviderSettings.ProviderName);
                GalleryApplicationResult result = WebProvider.GetGalleryApplication(UserId,id);
				Log.WriteEnd("'{0}' GetGalleryApplication", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GetGalleryApplication", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
		{
			try
			{
				Log.WriteStart("'{0}' GetGalleryApplicationStatus", ProviderSettings.ProviderName);
                GalleryWebAppStatus result = WebProvider.GetGalleryApplicationStatus(UserId,id);
				Log.WriteEnd("'{0}' GetGalleryApplicationStatus", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GetGalleryApplicationStatus", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
		{
			try
			{
				Log.WriteStart("'{0}' DownloadGalleryApplication", ProviderSettings.ProviderName);
                GalleryWebAppStatus result = WebProvider.DownloadGalleryApplication(UserId,id);
				Log.WriteEnd("'{0}' DownloadGalleryApplication", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' DownloadGalleryApplication", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
		{
			try
			{
				Log.WriteStart("'{0}' GetGalleryApplicationParameters", ProviderSettings.ProviderName);
                DeploymentParametersResult result = WebProvider.GetGalleryApplicationParameters(UserId,id);
				Log.WriteEnd("'{0}' GetGalleryApplicationParameters", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GetGalleryApplicationParameters", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
		{
			try
			{
				Log.WriteStart("'{0}' InstallGalleryApplication", ProviderSettings.ProviderName);
                StringResultObject result = WebProvider.InstallGalleryApplication(UserId,id, updatedValues, languageId);
				Log.WriteEnd("'{0}' InstallGalleryApplication", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' InstallGalleryApplication", ProviderSettings.ProviderName), ex);
				throw;
			}
		}
	
		#endregion

		#region WebManagement Access

		[WebMethod, SoapHeader("settings")]
		public bool CheckWebManagementAccountExists(string accountName)
		{
			try
			{
				bool accountExists;
				//
				Log.WriteStart("'{0}' CheckWebManagementAccountExtsts", ProviderSettings.ProviderName);
				//
				accountExists = WebProvider.CheckWebManagementAccountExists(accountName);
				//
				Log.WriteEnd("'{0}' CheckWebManagementAccountExtsts", ProviderSettings.ProviderName);
				//
				return accountExists;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' CheckWebManagementAccountExtsts", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
		{
			try
			{
				ResultObject result;

				Log.WriteStart("'{0}' CheckWebManagementPasswordComplexity", ProviderSettings.ProviderName);
				
				result = WebProvider.CheckWebManagementPasswordComplexity(accountPassword);
				
				Log.WriteEnd("'{0}' CheckWebManagementPasswordComplexity", ProviderSettings.ProviderName);
				//
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' CheckWebManagementPasswordComplexity", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void GrantWebManagementAccess(string siteId, string accountName, string accountPassword)
		{
			try
			{
				Log.WriteStart("'{0}' GrantWebManagementAccess", ProviderSettings.ProviderName);
				WebProvider.GrantWebManagementAccess(siteId, accountName, accountPassword);
				Log.WriteEnd("'{0}' GrantWebManagementAccess", ProviderSettings.ProviderName);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GrantWebManagementAccess", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void RevokeWebManagementAccess(string siteId, string accountName)
		{
			try
			{
				Log.WriteStart("'{0}' RevokeWebManagementAccess", ProviderSettings.ProviderName);
				WebProvider.RevokeWebManagementAccess(siteId, accountName);
				Log.WriteEnd("'{0}' RevokeWebManagementAccess", ProviderSettings.ProviderName);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' RevokeWebManagementAccess", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
		{
			try
			{
				Log.WriteStart("'{0}' ChangeWebManagementAccessPassword", ProviderSettings.ProviderName);
				WebProvider.ChangeWebManagementAccessPassword(accountName, accountPassword);
				Log.WriteEnd("'{0}' ChangeWebManagementAccessPassword", ProviderSettings.ProviderName);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' ChangeWebManagementAccessPassword", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        #endregion

        #region SSL Management
        [WebMethod, SoapHeader("settings")]
        public SSLCertificate generateCSR(SSLCertificate certificate)
        {
            try
            {
                Log.WriteStart("'{0}' generateCSR", ProviderSettings.ProviderName);
                certificate = WebProvider.generateCSR(certificate);
                Log.WriteEnd("'{0}' generateCSR", ProviderSettings.ProviderName);
                return certificate;

            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' generateCSR", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        [WebMethod, SoapHeader("settings")]
        public SSLCertificate generateRenewalCSR(SSLCertificate certificate)
        {
            try
            {
                Log.WriteStart("'{0}' generateCSR", ProviderSettings.ProviderName);
                certificate = WebProvider.generateCSR(certificate);
                Log.WriteEnd("'{0}' generateCSR", ProviderSettings.ProviderName);
                return certificate;

            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' generateCSR", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SSLCertificate getCertificate(WebSite site)
        {
            throw new NotImplementedException();
        }

        [WebMethod, SoapHeader("settings")]
        public SSLCertificate installCertificate(SSLCertificate certificate, WebSite website)
        {
            try
            {
                Log.WriteStart("'{0}' installCertificate", ProviderSettings.ProviderName);
                SSLCertificate result = WebProvider.installCertificate(certificate, website);
                Log.WriteEnd("'{0}' installCertificate", ProviderSettings.ProviderName);
                return result;

            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' generateCSR", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public String LEinstallCertificate(WebSite website, string email)
        {
            try
            {
                Log.WriteStart("'{0}' LEinstallCertificate", ProviderSettings.ProviderName);
                string result = WebProvider.LEinstallCertificate(website, email);
                Log.WriteEnd("'{0}' LEinstallCertificate", ProviderSettings.ProviderName);
                return result;

            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' LEinstallCertificate", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SSLCertificate installPFX(byte[] certificate, string password, WebSite website)
        {
            try
            {
                Log.WriteStart("'{0}' installPFX", ProviderSettings.ProviderName);
                SSLCertificate response = WebProvider.installPFX(certificate, password, website);

                if (response.Hash == null)
                {
                    Log.WriteError(String.Format("'{0}' installPFX", ProviderSettings.ProviderName), null);
                }
                else
                {
                    Log.WriteEnd("'{0}' installPFX", ProviderSettings.ProviderName);
                }
                return response;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' installPFX", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] exportCertificate(string serialNumber, string password)
        {
            return WebProvider.exportCertificate(serialNumber, password);
        }
        [WebMethod, SoapHeader("settings")]
        public List<SSLCertificate> getServerCertificates()
        {
            return WebProvider.getServerCertificates();
        }
        [WebMethod, SoapHeader("settings")]
        public ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
        {
            return WebProvider.DeleteCertificate(certificate, website);
        }
        [WebMethod, SoapHeader("settings")]
        public SSLCertificate ImportCertificate(WebSite website)
        {
            return WebProvider.ImportCertificate(website);
        }
        [WebMethod, SoapHeader("settings")]
        public bool CheckCertificate(WebSite webSite)
        {
            return WebProvider.CheckCertificate(webSite);
        }
        #endregion

        #region Directory Browsing

        [WebMethod, SoapHeader("settings")]
        public bool GetDirectoryBrowseEnabled(string siteId)
        {
            return WebProvider.GetDirectoryBrowseEnabled(siteId);
        }

        [WebMethod, SoapHeader("settings")]
        public  void SetDirectoryBrowseEnabled(string siteId, bool enabled)
        {
            WebProvider.SetDirectoryBrowseEnabled(siteId, enabled);
        }

        #endregion
    }
}
