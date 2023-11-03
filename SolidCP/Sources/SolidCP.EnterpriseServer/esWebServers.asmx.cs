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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    public class esWebServers
    {
        [WebMethod]
        public DataSet GetRawWebSitesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return WebServerController.GetRawWebSitesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<WebSite> GetWebSites(int packageId, bool recursive)
        {
            return WebServerController.GetWebSites(packageId, recursive);
        }

        [WebMethod]
        public WebSite GetWebSite(int siteItemId)
        {
            return WebServerController.GetWebSite(siteItemId);
        }

        [WebMethod]
        public List<DomainInfo> GetWebSitePointers(int siteItemId)
        {
            return WebServerController.GetWebSitePointers(siteItemId);
        }

        [WebMethod]
        public int AddWebSitePointer(int siteItemId, string hostName, int domainId)
        {
            return WebServerController.AddWebSitePointer(siteItemId, hostName, domainId);
        }

        [WebMethod]
        public int DeleteWebSitePointer(int siteItemId, int domainId)
        {
            return WebServerController.DeleteWebSitePointer(siteItemId, domainId);
        }

        [WebMethod]
        public int AddWebSite(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone)
        {
            return WebServerController.AddWebSite(packageId, hostName, domainId, ipAddressId, true, ignoreGlobalDNSZone);
        }

        [WebMethod]
        public int UpdateWebSite(WebSite site)
        {
            return WebServerController.UpdateWebSite(site);
        }

        [WebMethod]
        public int InstallFrontPage(int siteItemId, string username, string password)
        {
            return WebServerController.InstallFrontPage(siteItemId, username, password);
        }

        [WebMethod]
        public int UninstallFrontPage(int siteItemId)
        {
            return WebServerController.UninstallFrontPage(siteItemId);
        }

        [WebMethod]
        public int ChangeFrontPagePassword(int siteItemId, string password)
        {
            return WebServerController.ChangeFrontPagePassword(siteItemId, password);
        }

        [WebMethod]
        public int RepairWebSite(int siteItemId)
        {
            return WebServerController.RepairWebSite(siteItemId);
        }

        [WebMethod]
        public int DeleteWebSite(int siteItemId, bool deleteWebsiteDirectory)
        {
            return WebServerController.DeleteWebSite(siteItemId, deleteWebsiteDirectory);
        }

        [WebMethod]
        public int SwitchWebSiteToDedicatedIP(int siteItemId, int ipAddressId)
        {
            return WebServerController.SwitchWebSiteToDedicatedIP(siteItemId, ipAddressId);
        }

        [WebMethod]
        public int SwitchWebSiteToSharedIP(int siteItemId)
        {
            return WebServerController.SwitchWebSiteToSharedIP(siteItemId);
        }

        #region Virtual Directory
        [WebMethod]
        public int AddVirtualDirectory(int siteItemId, string vdirName, string vdirPath)
        {
            return WebServerController.AddVirtualDirectory(siteItemId, vdirName, vdirPath);
        }

        [WebMethod]
        public List<WebVirtualDirectory> GetVirtualDirectories(int siteItemId)
        {
            return WebServerController.GetVirtualDirectories(siteItemId);
        }

        [WebMethod]
        public WebVirtualDirectory GetVirtualDirectory(int siteItemId, string vdirName)
        {
            return WebServerController.GetVirtualDirectory(siteItemId, vdirName);
        }

        [WebMethod]
        public int UpdateVirtualDirectory(int siteItemId, WebVirtualDirectory vdir)
        {
            return WebServerController.UpdateVirtualDirectory(siteItemId, vdir);
        }

        [WebMethod]
        public int DeleteVirtualDirectory(int siteItemId, string vdirName)
        {
            return WebServerController.DeleteVirtualDirectory(siteItemId, vdirName);
        }

        [WebMethod]
        public int AddAppVirtualDirectory(int siteItemId, string vdirName, string vdirPath, string aspNetVersion)
        {
            return WebServerController.AddAppVirtualDirectory(siteItemId, vdirName, vdirPath);
        }

        [WebMethod]
        public List<WebAppVirtualDirectory> GetAppVirtualDirectories(int siteItemId)
        {
            return WebServerController.GetAppVirtualDirectories(siteItemId);
        }

        [WebMethod]
        public WebAppVirtualDirectory GetAppVirtualDirectory(int siteItemId, string vdirName)
        {
            return WebServerController.GetAppVirtualDirectory(siteItemId, vdirName);
        }

        [WebMethod]
        public int UpdateAppVirtualDirectory(int siteItemId, WebAppVirtualDirectory vdir)
        {
            return WebServerController.UpdateAppVirtualDirectory(siteItemId, vdir);
        }

        [WebMethod]
        public int DeleteAppVirtualDirectory(int siteItemId, string vdirName)
        {
            return WebServerController.DeleteAppVirtualDirectory(siteItemId, vdirName);
        }
        #endregion

        [WebMethod]
        public int ChangeSiteState(int siteItemId, ServerState state)
        {
            return WebServerController.ChangeSiteState(siteItemId, state);
        }

        // AppPool
        [WebMethod]
        public int ChangeAppPoolState(int siteItemId, AppPoolState state)
        {
            return WebServerController.ChangeAppPoolState(siteItemId, state);
        }

        [WebMethod]
        public AppPoolState GetAppPoolState(int siteItemId)
        {
            return WebServerController.GetAppPoolState(siteItemId);
        }

        // RB ADDED TO TRACK SITE STATUS
        [WebMethod]
        public ServerState GetSiteState(int siteItemId)
        {
            return WebServerController.GetSiteState(siteItemId);
        }


        #region Shared SSL Folders
        [WebMethod]
        public List<string> GetSharedSSLDomains(int packageId)
        {
            return WebServerController.GetSharedSSLDomains(packageId);
        }

        [WebMethod]
        public DataSet GetRawSSLFoldersPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return WebServerController.GetRawSSLFoldersPaged(packageId, filterColumn, filterValue, sortColumn,
                startRow, maximumRows);
        }

        [WebMethod]
        public List<SharedSSLFolder> GetSharedSSLFolders(int packageId, bool recursive)
        {
            return WebServerController.GetSharedSSLFolders(packageId, recursive);
        }

        [WebMethod]
        public SharedSSLFolder GetSharedSSLFolder(int itemId)
        {
            return WebServerController.GetSharedSSLFolder(itemId);
        }

        [WebMethod]
        public int AddSharedSSLFolder(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath)
        {
            return WebServerController.AddSharedSSLFolder(packageId, sslDomain, siteId, vdirName, vdirPath);
        }

        [WebMethod]
        public int UpdateSharedSSLFolder(SharedSSLFolder vdir)
        {
            return WebServerController.UpdateSharedSSLFolder(vdir);
        }

        [WebMethod]
        public int DeleteSharedSSLFolder(int itemId)
        {
            return WebServerController.DeleteSharedSSLFolder(itemId);
        }
        #endregion

        #region Secured Folders
        [WebMethod]
        public int InstallSecuredFolders(int siteItemId)
        {
            return WebServerController.InstallSecuredFolders(siteItemId);
        }

        [WebMethod]
        public int UninstallSecuredFolders(int siteItemId)
        {
            return WebServerController.UninstallSecuredFolders(siteItemId);
        }

        [WebMethod]
        public WebFolder[] GetSecuredFolders(int siteItemId)
        {
            return WebServerController.GetFolders(siteItemId);
        }

        [WebMethod]
        public WebFolder GetSecuredFolder(int siteItemId, string folderPath)
        {
            return WebServerController.GetFolder(siteItemId, folderPath);
        }

        [WebMethod]
        public int UpdateSecuredFolder(int siteItemId, WebFolder folder)
        {
            return WebServerController.UpdateFolder(siteItemId, folder);
        }

        [WebMethod]
        public int DeleteSecuredFolder(int siteItemId, string folderPath)
        {
            return WebServerController.DeleteFolder(siteItemId, folderPath);
        }
        #endregion

        #region Secured Users
        [WebMethod]
        public WebUser[] GetSecuredUsers(int siteItemId)
        {
            return WebServerController.GetUsers(siteItemId);
        }

        [WebMethod]
        public WebUser GetSecuredUser(int siteItemId, string userName)
        {
            return WebServerController.GetUser(siteItemId, userName);
        }

        [WebMethod]
        public int UpdateSecuredUser(int siteItemId, WebUser user)
        {
            return WebServerController.UpdateUser(siteItemId, user);
        }

        [WebMethod]
        public int DeleteSecuredUser(int siteItemId, string userName)
        {
            return WebServerController.DeleteUser(siteItemId, userName);
        }
        #endregion

        #region Secured Groups
        [WebMethod]
        public WebGroup[] GetSecuredGroups(int siteItemId)
        {
            return WebServerController.GetGroups(siteItemId);
        }

        [WebMethod]
        public WebGroup GetSecuredGroup(int siteItemId, string groupName)
        {
            return WebServerController.GetGroup(siteItemId, groupName);
        }

        [WebMethod]
        public int UpdateSecuredGroup(int siteItemId, WebGroup group)
        {
            return WebServerController.UpdateGroup(siteItemId, group);
        }

        [WebMethod]
        public int DeleteSecuredGroup(int siteItemId, string groupName)
        {
            return WebServerController.DeleteGroup(siteItemId, groupName);
        }
        #endregion

        #region Web Deploy Publishing Access

        [WebMethod]
        public ResultObject GrantWebDeployPublishingAccess(int siteItemId, string accountName, string accountPassword)
        {
            return WebServerController.GrantWebDeployPublishingAccess(siteItemId, accountName, accountPassword);
        }

        [WebMethod]
        public ResultObject SaveWebDeployPublishingProfile(int siteItemId, int[] serviceItemIds)
        {
            return WebServerController.SaveWebDeployPublishingProfile(siteItemId, serviceItemIds);
        }

        [WebMethod]
        public void RevokeWebDeployPublishingAccess(int siteItemId)
        {
            WebServerController.RevokeWebDeployPublishingAccess(siteItemId);
        }

        [WebMethod]
        public BytesResult GetWebDeployPublishingProfile(int siteItemId)
        {
            return WebServerController.GetWebDeployPublishingProfile(siteItemId);
        }

        [WebMethod]
        public ResultObject ChangeWebDeployPublishingPassword(int siteItemId, string newAccountPassword)
        {
            return WebServerController.ChangeWebDeployPublishingPassword(siteItemId, newAccountPassword);
        }

        #endregion

        #region Helicon Ape

        [WebMethod]
        public HeliconApeStatus GetHeliconApeStatus(int siteItemId)
        {
            return WebServerController.GetHeliconApeStatus(siteItemId);
        }

        [WebMethod]
        public void InstallHeliconApe(int siteItemId)
        {
            WebServerController.InstallHeliconApe(siteItemId);
        }

        [WebMethod]
        public int EnableHeliconApe(int siteItemId)
        {
            return WebServerController.EnableHeliconApe(siteItemId);
        }

        [WebMethod]
        public int DisableHeliconApe(int siteItemId)
        {
            return WebServerController.DisableHeliconApe(siteItemId);
        }

        [WebMethod]
        public int EnableHeliconApeGlobally(int serviceId)
        {
            return WebServerController.EnableHeliconApeGlobally(serviceId);
        }

        [WebMethod]
        public int DisableHeliconApeGlobally(int serviceId)
        {
            return WebServerController.DisableHeliconApeGlobally(serviceId);
        }

        [WebMethod]
        public HtaccessFolder[] GetHeliconApeFolders(int siteItemId)
        {
            return WebServerController.GetHeliconApeFolders(siteItemId);
        }

        [WebMethod]
        public HtaccessFolder GetHeliconApeHttpdFolder(int serviceId)
        {
            return WebServerController.GetHeliconApeHttpdFolder(serviceId);
        }


        [WebMethod]
        public HtaccessFolder GetHeliconApeFolder(int siteItemId, string folderPath)
        {
            return WebServerController.GetHeliconApeFolder(siteItemId, folderPath);
        }

        [WebMethod]
        public int UpdateHeliconApeFolder(int siteItemId, HtaccessFolder folder)
        {
            return WebServerController.UpdateHeliconApeFolder(siteItemId, folder);
        }

        [WebMethod]
        public int UpdateHeliconApeHttpdFolder(int serviceId, HtaccessFolder folder)
        {
            return WebServerController.UpdateHeliconApeHttpdFolder(serviceId, folder);
        }

        [WebMethod]
        public int DeleteHeliconApeFolder(int siteItemId, string folderPath)
        {
            return WebServerController.DeleteHeliconApeFolder(siteItemId, folderPath);
        }

        #endregion

        #region Helicon Ape Users
        [WebMethod]
        public HtaccessUser[] GetHeliconApeUsers(int siteItemId)
        {
            return WebServerController.GetHeliconApeUsers(siteItemId);
        }

        [WebMethod]
        public HtaccessUser GetHeliconApeUser(int siteItemId, string userName)
        {
            return WebServerController.GetHeliconApeUser(siteItemId, userName);
        }

        [WebMethod]
        public int UpdateHeliconApeUser(int siteItemId, HtaccessUser user)
        {
            return WebServerController.UpdateHeliconApeUser(siteItemId, user);
        }

        [WebMethod]
        public int DeleteHeliconApeUser(int siteItemId, string userName)
        {
            return WebServerController.DeleteHeliconApeUser(siteItemId, userName);
        }
        #endregion

        #region Helicon Ape Groups
        [WebMethod]
        public WebGroup[] GetHeliconApeGroups(int siteItemId)
        {
            return WebServerController.GetHeliconApeGroups(siteItemId);
        }

        [WebMethod]
        public WebGroup GetHeliconApeGroup(int siteItemId, string groupName)
        {
            return WebServerController.GetHeliconApeGroup(siteItemId, groupName);
        }

        [WebMethod]
        public int UpdateHeliconApeGroup(int siteItemId, WebGroup group)
        {
            return WebServerController.UpdateHeliconApeGroup(siteItemId, group);
        }

        [WebMethod]
        public int DeleteHeliconApeGroup(int siteItemId, string groupName)
        {
            return WebServerController.DeleteHeliconApeGroup(siteItemId, groupName);
        }
        #endregion

        #region Helicon Zoo

        [WebMethod]
        public List<WebAppVirtualDirectory> GetZooApplications(int siteItemId)
        {
            return WebServerController.GetZooApplications(siteItemId);
        }

        [WebMethod]
        public StringResultObject SetZooEnvironmentVariable(int siteItemId, string appName, string envName, string envValue)
        {
            return WebServerController.SetZooEnvironmentVariable(siteItemId, appName, envName, envValue);
        }


        [WebMethod]
        public StringResultObject SetZooConsoleEnabled(int siteItemId, string appName)
        {
            return WebServerController.SetZooConsoleEnabled(siteItemId, appName);
        }

        [WebMethod]
        public StringResultObject SetZooConsoleDisabled(int siteItemId, string appName)
        {
            return WebServerController.SetZooConsoleDisabled(siteItemId, appName);
        }



        #endregion

        #region WebManagement Access

        [WebMethod]
        public ResultObject GrantWebManagementAccess(int siteItemId, string accountName, string accountPassword)
        {
            return WebServerController.GrantWebManagementAccess(siteItemId, accountName, accountPassword);
        }

        [WebMethod]
        public void RevokeWebManagementAccess(int siteItemId)
        {
            WebServerController.RevokeWebManagementAccess(siteItemId);
        }

        [WebMethod]
        public ResultObject ChangeWebManagementAccessPassword(int siteItemId, string accountPassword)
        {
            return WebServerController.ChangeWebManagementAccessPassword(siteItemId, accountPassword);
        }

        #endregion

        #region SSL
        [WebMethod]
        public SSLCertificate CertificateRequest(SSLCertificate certificate, int siteItemId)
        {
            return WebServerController.CertificateRequest(certificate, siteItemId);
        }
        [WebMethod]
        public ResultObject InstallCertificate(SSLCertificate certificate, int siteItemId)
        {
            return WebServerController.InstallCertificate(certificate, siteItemId);
        }
        [WebMethod]
        public ResultObject LEInstallCertificate(int siteItemId, string email)
        {
            return WebServerController.LEInstallCertificate(siteItemId, email);
        }
        [WebMethod]
        public ResultObject InstallPfx(byte[] certificate, int siteItemId, string password)
        {
            return WebServerController.InstallPfx(certificate, siteItemId, password);
        }
        [WebMethod]
        public List<SSLCertificate> GetPendingCertificates(int siteItemId)
        {
            return WebServerController.GetPendingCertificates(siteItemId);
        }
        [WebMethod]
        public SSLCertificate GetSSLCertificateByID(int Id)
        {
            return WebServerController.GetSslCertificateById(Id);
        }
        [WebMethod]
        public SSLCertificate GetSiteCert(int siteID)
        {
            return WebServerController.GetSiteCert(siteID);
        }
        [WebMethod]
        public int CheckSSLForWebsite(int siteID, bool renewal)
        {
            return WebServerController.CheckSSL(siteID, renewal);
        }
        [WebMethod]
        public ResultObject CheckSSLForDomain(string domain, int siteID)
        {
            return WebServerController.CheckSSLForDomain(domain, siteID);
        }
        [WebMethod]
        public byte[] ExportCertificate(int siteId, string serialNumber, string password)
        {
            return WebServerController.ExportCertificate(siteId, serialNumber, password);
        }
        [WebMethod]
        public List<SSLCertificate> GetCertificatesForSite(int siteId)
        {
            return WebServerController.GetCertificatesForSite(siteId);
        }
        [WebMethod]
        public ResultObject DeleteCertificate(int siteId, SSLCertificate certificate)
        {
            return WebServerController.DeleteCertificate(siteId, certificate);
        }
        [WebMethod]
        public ResultObject ImportCertificate(int siteId)
        {
            return WebServerController.ImportCertificate(siteId);
        }
        [WebMethod]
        public ResultObject CheckCertificate(int siteId)
        {
            return WebServerController.CheckCertificate(siteId);
        }
        [WebMethod]
        public ResultObject DeleteCertificateRequest(int siteId, int csrID)
        {
            return WebServerController.DeleteCertificateRequest(siteId, csrID);
        }

        #endregion

    }
}
