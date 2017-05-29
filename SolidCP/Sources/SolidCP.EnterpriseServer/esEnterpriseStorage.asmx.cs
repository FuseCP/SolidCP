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
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;

using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.OS;
using SolidCP.Providers.Web;
using SolidCP.EnterpriseServer.Base.HostedSolution;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esEnterpriseStorage
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esEnterpriseStorage : WebService
    {
        [WebMethod]
        public int AddWebDavAccessToken(WebDavAccessToken accessToken)
        {
           return EnterpriseStorageController.AddWebDavAccessToken(accessToken);
        }

        [WebMethod]
        public void DeleteExpiredWebDavAccessTokens()
        {
            EnterpriseStorageController.DeleteExpiredWebDavAccessTokens();
        }

        [WebMethod]
        public WebDavAccessToken GetWebDavAccessTokenById(int id)
        {
            return EnterpriseStorageController.GetWebDavAccessTokenById(id);
        }

        [WebMethod]
        public WebDavAccessToken GetWebDavAccessTokenByAccessToken(Guid accessToken)
        {
            return EnterpriseStorageController.GetWebDavAccessTokenByAccessToken(accessToken);
        }

        [WebMethod]
        public bool CheckFileServicesInstallation(int serviceId)
        {
            return EnterpriseStorageController.CheckFileServicesInstallation(serviceId);
        }

        [WebMethod]
        public SystemFile[] GetEnterpriseFolders(int itemId)
        {
            return EnterpriseStorageController.GetFolders(itemId);
        }

        [WebMethod]
        public SystemFile[] GetUserRootFolders(int itemId, int accountId, string userName, string displayName)
        {
            return EnterpriseStorageController.GetUserRootFolders(itemId, accountId, userName, displayName);
        }

        [WebMethod]
        public SystemFile GetEnterpriseFolder(int itemId, string folderName)
        {
            return EnterpriseStorageController.GetFolder(itemId, folderName);
        }

        [WebMethod]
        public SystemFile GetEnterpriseFolderWithExtraData(int itemId, string folderName, bool loadDriveMapInfo)
        {
            return EnterpriseStorageController.GetFolder(itemId, folderName, loadDriveMapInfo);
        }

        [WebMethod]
        public ResultObject CreateEnterpriseFolder(int itemId, string folderName, int quota, QuotaType quotaType, bool addDefaultGroup)
        {
            return EnterpriseStorageController.CreateFolder(itemId, folderName, quota, quotaType, addDefaultGroup);
        }

        [WebMethod]
        public ResultObject DeleteEnterpriseFolder(int itemId, string folderName)
        {
            return EnterpriseStorageController.DeleteFolder(itemId, folderName);
        }

        [WebMethod]
        public ESPermission[] GetEnterpriseFolderPermissions(int itemId, string folderName)
        {
            return EnterpriseStorageController.GetFolderPermission(itemId, folderName);
        }

        [WebMethod]
        public ResultObject SetEnterpriseFolderPermissions(int itemId, string folderName, ESPermission[] permission)
        {
            return EnterpriseStorageController.SetFolderPermission(itemId, folderName, permission);
        }

        [WebMethod]
        public List<ExchangeAccount> SearchESAccounts(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            return EnterpriseStorageController.SearchESAccounts(itemId, filterColumn, filterValue, sortColumn);
        }

        [WebMethod]
        public SystemFilesPaged GetEnterpriseFoldersPaged(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return EnterpriseStorageController.GetEnterpriseFoldersPaged(itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public SystemFile RenameEnterpriseFolder(int itemId, string oldName, string newName)
        {
            return EnterpriseStorageController.RenameFolder(itemId, oldName, newName);
        }

        [WebMethod]
        public ResultObject CreateEnterpriseStorage(int packageId, int itemId)
        {
            return EnterpriseStorageController.CreateEnterpriseStorage(packageId, itemId);
        }

        [WebMethod]
        public bool CheckEnterpriseStorageInitialization(int packageId, int itemId)
        {
            return EnterpriseStorageController.CheckEnterpriseStorageInitialization(packageId, itemId);
        }

        [WebMethod]
        public bool CheckUsersDomainExists(int itemId)
        {
            return EnterpriseStorageController.CheckUsersDomainExists(itemId);
        }

        [WebMethod]
        public string GetWebDavPortalUserSettingsByAccountId(int accountId)
        {
            return EnterpriseStorageController.GetWebDavPortalUserSettingsByAccountId(accountId);
        }

        [WebMethod]
        public void UpdateWebDavPortalUserSettings(int accountId, string settings)
        {
            EnterpriseStorageController.UpdateUserSettings(accountId,settings);
        }

        [WebMethod]
        public SystemFile[] SearchFiles(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
           return EnterpriseStorageController.SearchFiles(itemId, searchPaths, searchText, userPrincipalName, recursive);
        }

        [WebMethod]
        public int GetEnterpriseStorageServiceId(int itemId)
        {
            return EnterpriseStorageController.GetEnterpriseStorageServiceId(itemId);
        }

        [WebMethod]
        public void SetEsFolderShareSettings(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
            EnterpriseStorageController.SetEsFolderShareSettings(itemId, folderName, abeIsEnabled, edaIsEnabled);
        }

        #region Directory Browsing

        [WebMethod]
        public bool GetDirectoryBrowseEnabled(int itemId, string site)
        {
            return EnterpriseStorageController.GetDirectoryBrowseEnabled(itemId, site);
        }

        [WebMethod]
        public void SetDirectoryBrowseEnabled(int itemId, string site, bool enabled)
        {
            EnterpriseStorageController.SetDirectoryBrowseEnabled(itemId, site, enabled);
        }

        [WebMethod]
        public void SetEnterpriseFolderSettings(int itemId, SystemFile folder, ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            EnterpriseStorageController.StartSetEnterpriseFolderSettingsBackgroundTask(itemId, folder, permissions, directoyBrowsingEnabled, quota, quotaType);
        }

        [WebMethod]
        public void SetEnterpriseFolderGeneralSettings(int itemId, SystemFile folder, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            EnterpriseStorageController.SetESGeneralSettings(itemId, folder, directoyBrowsingEnabled, quota, quotaType);
        }

        [WebMethod]
        public void SetEnterpriseFolderPermissionSettings(int itemId, SystemFile folder, ESPermission[] permissions)
        {
            EnterpriseStorageController.SetESFolderPermissionSettings(itemId, folder, permissions);
        }

        [WebMethod]
        public OrganizationUser[] GetFolderOwaAccounts(int itemId, SystemFile folder)
        {
           return  EnterpriseStorageController.GetFolderOwaAccounts(itemId, folder.Name);
        }

        [WebMethod]
        public void SetFolderOwaAccounts(int itemId, SystemFile folder, OrganizationUser[] users)
        {
            EnterpriseStorageController.SetFolderOwaAccounts(itemId, folder.Name, users);
        }

        [WebMethod]
        public List<string> GetUserEnterpriseFolderWithOwaEditPermission(int itemId, List<int> accountIds)
        {
            return EnterpriseStorageController.GetUserEnterpriseFolderWithOwaEditPermission(itemId, accountIds);
        }

        [WebMethod]
        public ResultObject MoveToStorageSpace(int itemId, string folderName)
        {
           return EnterpriseStorageController.MoveToStorageSpace(itemId, folderName);
        }

        #endregion

        #region Statistics

        [WebMethod]
        public OrganizationStatistics GetStatistics(int itemId)
        {
            return EnterpriseStorageController.GetStatistics(itemId);
        }

        [WebMethod]
        public OrganizationStatistics GetStatisticsByOrganization(int itemId)
        {
            return EnterpriseStorageController.GetStatisticsByOrganization(itemId);
        }

        #endregion

        #region Drive Mapping

        [WebMethod]
        public ResultObject CreateMappedDrive(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            return EnterpriseStorageController.CreateMappedDrive(packageId, itemId, driveLetter, labelAs, folderName);
        }

        [WebMethod]
        public ResultObject DeleteMappedDrive(int itemId, string driveLetter)
        {
            return EnterpriseStorageController.DeleteMappedDrive(itemId, driveLetter);
        }

        [WebMethod]
        public MappedDrivesPaged GetDriveMapsPaged(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return EnterpriseStorageController.GetDriveMapsPaged(itemId, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public string[] GetUsedDriveLetters(int itemId)
        {
            return EnterpriseStorageController.GetUsedDriveLetters(itemId);
        }

        [WebMethod]
        public SystemFile[] GetNotMappedEnterpriseFolders(int itemId)
        {
            return EnterpriseStorageController.GetNotMappedEnterpriseFolders(itemId);
        }

        #endregion
    }
}
