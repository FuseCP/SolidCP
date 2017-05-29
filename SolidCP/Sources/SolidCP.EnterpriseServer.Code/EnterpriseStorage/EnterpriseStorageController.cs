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
using System.Collections.Concurrent;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using SolidCP.Providers.StorageSpaces;
using SolidCP.Server;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.EnterpriseStorage;
using System.Collections;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Web;
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Server.Client;
using System.Text.RegularExpressions;
using System.Threading;

namespace SolidCP.EnterpriseServer
{
    public class EnterpriseStorageController
    {
        public const string UseStorageSpaces = "UseStorageSpaces";

        #region Public Methods

        public static bool CheckEnterpriseStorageInitialization(int packageId, int itemId)
        {
            return CheckEnterpriseStorageInitializationInternal(packageId, itemId);
        }

        public static ResultObject CreateEnterpriseStorage(int packageId, int itemId)
        {
            return CreateEnterpriseStorageInternal(packageId, itemId);
        }

        public static ResultObject DeleteEnterpriseStorage(int packageId, int itemId)
        {
            return DeleteEnterpriseStorageInternal(packageId, itemId);
        }

        public static SystemFile[] GetFolders(int itemId)
        {
            return GetFoldersInternal(itemId);
        }

        public static SystemFile[] GetUserRootFolders(int itemId, int accountId, string userName, string displayName)
        {
            return GetUserRootFoldersInternal(itemId, accountId, userName, displayName);
        }

        public static SystemFile GetFolder(int itemId, string folderName, bool loadMappedDriveInfo = false)
        {
            return GetFolderInternal(itemId, folderName, loadMappedDriveInfo);
        }

        public static SystemFile GetFolder(int itemId)
        {
            return GetFolder(itemId, string.Empty);
        }

        public static ResultObject CreateFolder(int itemId, bool isRootFolder = false)
        {
            return CreateFolder(itemId, string.Empty, 0, QuotaType.Soft, false, isRootFolder);
        }

        public static ResultObject CreateFolder(int itemId, string folderName, int quota, QuotaType quotaType, bool addDefaultGroup, bool isRootFolder = false)
        {
            return CreateFolderInternal(itemId, folderName, quota, quotaType, addDefaultGroup, isRootFolder);
        }

        public static ResultObject DeleteFolder(int itemId)
        {
            return DeleteFolder(itemId, string.Empty);
        }

        public static ResultObject DeleteFolder(int itemId, string folderName)
        {
            return DeleteFolderInternal(itemId, folderName);
        }

        public static List<ExchangeAccount> SearchESAccounts(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            return SearchESAccountsInternal(itemId, filterColumn, filterValue, sortColumn);
        }

        public static SystemFilesPaged GetEnterpriseFoldersPaged(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetEnterpriseFoldersPagedInternal(itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, filterValue, sortColumn, startRow, maximumRows);
        }

        public static IEnumerable<SystemFile> GetEnterpriseFolders(int itemId, bool loadUsagesData = false, bool loadWebdavRules = false, bool loadMappedDrives = false)
        {
            return GetEnterpriseFoldersPaged(itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, "", "", 0, int.MaxValue).PageItems;
        }

        public static ResultObject SetFolderPermission(int itemId, string folder, ESPermission[] permission)
        {
            return SetFolderWebDavRulesInternal(itemId, folder, permission);
        }

        public static ESPermission[] GetFolderPermission(int itemId, string folder)
        {
            return ConvertToESPermission(itemId, GetFolderWebDavRulesInternal(itemId, folder));
        }

        public static bool CheckFileServicesInstallation(int serviceId)
        {
            EnterpriseStorage es = GetEnterpriseStorage(serviceId);
            return es.CheckFileServicesInstallation();
        }

        public static SystemFile RenameFolder(int itemId, string oldFolder, string newFolder)
        {
            return RenameFolderInternal(itemId, oldFolder, newFolder);
        }

        public static bool CheckUsersDomainExists(int itemId)
        {
            return CheckUsersDomainExistsInternal(itemId);
        }

        public static void SetFRSMQuotaOnFolder(int itemId, string folderName, int quota, QuotaType quotaType)
        {
            SetFRSMQuotaOnFolderInternal(itemId, folderName, quota, quotaType);
        }

        public static void StartSetEnterpriseFolderSettingsBackgroundTask(int itemId, SystemFile folder, ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            StartESBackgroundTaskInternal("SET_ENTERPRISE_FOLDER_SETTINGS", itemId, folder, permissions, directoyBrowsingEnabled, quota, quotaType);
        }

        public static void SetESGeneralSettings(int itemId, SystemFile folder, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            SetESGeneralSettingsInternal("SET_ENTERPRISE_FOLDER_GENERAL_SETTINGS", itemId, folder, directoyBrowsingEnabled, quota, quotaType);
        }

        public static void SetESFolderPermissionSettings(int itemId, SystemFile folder, ESPermission[] permissions)
        {
            SetESFolderPermissionSettingsInternal("SET_ENTERPRISE_FOLDER_GENERAL_SETTINGS", itemId, folder, permissions);
        }

        public static int AddWebDavAccessToken(WebDavAccessToken accessToken)
        {
            return DataProvider.AddWebDavAccessToken(accessToken);
        }

        public static void DeleteExpiredWebDavAccessTokens()
        {
            DataProvider.DeleteExpiredWebDavAccessTokens();
        }

        public static WebDavAccessToken GetWebDavAccessTokenById(int id)
        {
            return ObjectUtils.FillObjectFromDataReader<WebDavAccessToken>(DataProvider.GetWebDavAccessTokenById(id));
        }

        public static WebDavAccessToken GetWebDavAccessTokenByAccessToken(Guid accessToken)
        {
            return ObjectUtils.FillObjectFromDataReader<WebDavAccessToken>(DataProvider.GetWebDavAccessTokenByAccessToken(accessToken));
        }

        public static SystemFile[] SearchFiles(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return new SystemFile[0];
                }

                int serviceId = GetEnterpriseStorageServiceID(org.PackageId);

                if (serviceId == 0)
                {
                    return new SystemFile[0];
                }

                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));

                DataSet ds = DataProvider.GetEnterpriseFoldersPaged(itemId, "FolderName", "", "", 0, int.MaxValue);

                var esFolders = new List<EsFolder>();

                ObjectUtils.FillCollectionFromDataView(esFolders, ds.Tables[1].DefaultView);

                var searchRequests = new List<StorageSpaceFolderSearchRequest>();

                foreach (var searchPath in searchPaths.Where(x => !string.IsNullOrEmpty(x)))
                {
                    var rootFolder = esFolders.First(
                        x => string.Equals(searchPath.Split('\\').FirstOrDefault(),
                            x.FolderName,
                            StringComparison.InvariantCultureIgnoreCase));

                    if (rootFolder.StorageSpaceFolderId == null)
                    {
                        continue;
                    }

                    var searchRequest = new StorageSpaceFolderSearchRequest
                    {
                        SearchPath = searchPath,
                        SearchValue = searchText,
                        StorageSpaceFolderId = rootFolder.StorageSpaceFolderId.Value,
                        StorageSpaceId = rootFolder.StorageSpaceId
                    };

                    searchRequests.Add(searchRequest);
                }

                var tasks = new List<Task<IEnumerable<SystemFile>>>();

                tasks.AddRange(StorageSpacesController.SearchInStorageSpaceFolders(searchRequests));

                var task = new Task<IEnumerable<SystemFile>>(() =>
                {
                    var locEs = GetEnterpriseStorage(serviceId);

                    return locEs.Search(org.OrganizationId, searchPaths, searchText, userPrincipalName, recursive);
                });

                task.Start();

                tasks.Add(task);

                Task.WaitAll(tasks.ToArray());

                return tasks.SelectMany(x=>x.Result).ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Directory Browsing

        public static bool GetDirectoryBrowseEnabled(int itemId, string siteId)
        {
            return GetDirectoryBrowseEnabledInternal(itemId, siteId);
        }

        public static void SetDirectoryBrowseEnabled(int itemId, string siteId, bool enabled)
        {
            SetDirectoryBrowseEnabledInternal(itemId, siteId, enabled);
        }

        #endregion

        #region WebDav

        public static int AddWebDavDirectory(int packageId, string site, string vdirName, string contentpath)
        {
            return AddWebDavDirectoryInternal(packageId, site, vdirName, contentpath);
        }

        public static int DeleteWebDavDirectory(int packageId, string site, string vdirName)
        {
            return DeleteWebDavDirectoryInternal(packageId, site, vdirName);
        }

        #endregion

        #endregion

        private static IEnumerable<SystemFile> GetRootFolders(string userPrincipalName)
        {
            var rootFolders = new List<SystemFile>();

            var account = ExchangeServerController.GetAccountByAccountName(userPrincipalName);

            var userGroups = OrganizationController.GetSecurityGroupsByMember(account.ItemId, account.AccountId);

            foreach (var folder in GetFolders(account.ItemId))
            {
                var permissions = GetFolderPermission(account.ItemId, folder.Name);

                foreach (var permission in permissions)
                {
                    if ((!permission.IsGroup
                            && (permission.DisplayName == account.UserPrincipalName || permission.DisplayName == account.DisplayName))
                        || (permission.IsGroup && userGroups.Any(x => x.DisplayName == permission.DisplayName)))
                    {
                        rootFolders.Add(folder);
                        break;
                    }
                }
            }

            return rootFolders;
        }

        protected static void SetESGeneralSettingsInternal(string taskName, int itemId, SystemFile folder, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            // load organization
            var org = OrganizationController.GetOrganization(itemId);

            try
            {
                TaskManager.StartTask("ENTERPRISE_STORAGE", taskName, org.PackageId);

                var esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, folder.Name));

                if (esFolder.StorageSpaceFolderId == null)
                {
                    EnterpriseStorageController.SetFRSMQuotaOnFolder(itemId, folder.Name, quota, quotaType);
                }
                else
                {
                    StorageSpacesController.SetStorageSpaceFolderQuota(esFolder.StorageSpaceId, esFolder.StorageSpaceFolderId.Value, (long)quota * 1024 * 1024, quotaType);

                    DataProvider.UpdateEnterpriseFolder(itemId, folder.Name, folder.Name, quota);
                }
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex, "Error executing cloud folders background task");
            }
            finally
            {
                // complete task
                try
                {
                    TaskManager.CompleteTask();
                }
                catch (Exception)
                {
                }
            }
        }

        protected static void SetESFolderPermissionSettingsInternal(string taskName, int itemId, SystemFile folder, ESPermission[] permissions)
        {
            // load organization
            var org = OrganizationController.GetOrganization(itemId);

            new Thread(() =>
            {
                try
                {
                    TaskManager.StartTask("ENTERPRISE_STORAGE", taskName, org.PackageId);

                    EnterpriseStorageController.SetFolderPermission(itemId, folder.Name, permissions);


                    var esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, folder.Name));

                    if (esFolder.StorageSpaceFolderId != null)
                    {
                        StorageSpacesController.SetFolderNtfsPermissions(esFolder.StorageSpaceId, esFolder.Path, ConvertToUserPermissions(itemId, permissions.ToArray()), true, false);
                    }
                }
                catch (Exception ex)
                {
                    // log error
                    TaskManager.WriteError(ex, "Error executing Cloud Folders background task");
                }
                finally
                {
                    // complete task
                    try
                    {
                        TaskManager.CompleteTask();
                    }
                    catch (Exception)
                    {
                    }
                }

            }).Start();
        }

        protected static void StartESBackgroundTaskInternal(string taskName, int itemId, SystemFile folder, ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, QuotaType quotaType)
        {
            // load organization
            var org = OrganizationController.GetOrganization(itemId);

            new Thread(() =>
            {
                try
                {
                    TaskManager.StartTask("ENTERPRISE_STORAGE", taskName, org.PackageId);

                    EnterpriseStorageController.SetFRSMQuotaOnFolder(itemId, folder.Name, quota, quotaType);
                    EnterpriseStorageController.SetFolderPermission(itemId, folder.Name, permissions);
                }
                catch (Exception ex)
                {
                    // log error
                    TaskManager.WriteError(ex, "Error executing Cloud Folders background task");
                }
                finally
                {
                    // complete task
                    try
                    {
                        TaskManager.CompleteTask();
                    }
                    catch (Exception)
                    {
                    }
                }

            }).Start();
        }

        protected static bool CheckUsersDomainExistsInternal(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);

            if (org == null)
            {
                return false;
            }

            return CheckUsersDomainExistsInternal(itemId, org.PackageId);
        }

        protected static bool CheckUsersDomainExistsInternal(int itemId, int packageId)
        {
            var web = GetWebServer(packageId);

            if (web != null)
            {
                var esServiceId = GetEnterpriseStorageServiceID(packageId);

                StringDictionary esSesstings = ServerController.GetServiceSettings(esServiceId);

                string usersDomain = esSesstings["UsersDomain"];

                if (web.SiteExists(usersDomain))
                    return true;
            }

            return false;
        }

        protected static bool CheckEnterpriseStorageInitializationInternal(int packageId, int itemId)
        {
            bool checkResult = true;

            var esServiceId = GetEnterpriseStorageServiceID(packageId);
            int webServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);

            Organization org = OrganizationController.GetOrganization(itemId);

            if (org == null)
            {
                return false;
            }

            //root folder not created
            if (GetFolder(itemId) == null)
            {
                checkResult = false;
            }

            //checking if virtual directory is created
            StringDictionary esSesstings = ServerController.GetServiceSettings(esServiceId);

            string usersDomain = esSesstings["UsersDomain"];

            WebServer web = GetWebServer(packageId);

            if (!web.AppVirtualDirectoryExists(usersDomain, org.OrganizationId))
            {
                checkResult = false;
            }


            return checkResult;
        }

        protected static ResultObject CreateEnterpriseStorageInternal(int packageId, int itemId)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "CREATE_ORGANIZATION_ENTERPRISE_STORAGE", itemId, packageId);

            try
            {
                int esServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);

                if (esServiceId != 0)
                {
                    StringDictionary esSesstings = ServerController.GetServiceSettings(esServiceId);

                    Organization org = OrganizationController.GetOrganization(itemId);

                    string usersHome = esSesstings["UsersHome"];
                    string usersDomain = esSesstings["UsersDomain"];
                    string locationDrive = esSesstings["LocationDrive"];

                    string homePath = string.Format("{0}:\\{1}", locationDrive, usersHome);

                    EnterpriseStorageController.CreateFolder(itemId, true);

                    EnterpriseStorageController.AddWebDavDirectory(packageId, usersDomain, org.OrganizationId, Path.Combine(homePath, org.OrganizationId));
                }
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_CREATE_FOLDER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        protected static ResultObject CreateEnterpriseStorageVirtualFolderInternal(int packageId, int itemId, string folderName, string uncPath)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "CREATE_ORGANIZATION_ENTERPRISE_STORAGE_VIRTUAL_DIRECTORY", itemId, packageId);

            try
            {
                int esServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);

                if (esServiceId != 0)
                {
                    StringDictionary esSesstings = ServerController.GetServiceSettings(esServiceId);

                    string usersDomain = esSesstings["UsersDomain"];

                    Organization org = OrganizationController.GetOrganization(itemId);

                    EnterpriseStorageController.AddWebDavDirectory(packageId, usersDomain, Path.Combine(org.OrganizationId, folderName), uncPath);
                }
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_CREATE_FOLDER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        protected static ResultObject DeleteEnterpriseStorageInternal(int packageId, int itemId)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION", "CLEANUP_ORGANIZATION_ENTERPRISE_STORAGE", itemId, packageId);

            try
            {
                int esId = PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);

                Organization org = OrganizationController.GetOrganization(itemId);

                if (esId != 0)
                {
                    StringDictionary esSesstings = ServerController.GetServiceSettings(esId);

                    string usersDomain = esSesstings["UsersDomain"];

                    var folders = GetEnterpriseFoldersPaged(itemId, false,false,false, string.Empty,string.Empty, 0, int.MaxValue);

                    foreach (var folder in folders.PageItems)
                    {
                        DeleteFolder(itemId, folder.Name);
                    }

                    EnterpriseStorageController.DeleteWebDavDirectory(packageId, usersDomain, org.OrganizationId);
                    EnterpriseStorageController.DeleteFolder(itemId);
                    EnterpriseStorageController.DeleteMappedDrivesGPO(itemId);
                }
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_CLEANUP", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static EnterpriseStorage GetEnterpriseStorage(int serviceId)
        {
            EnterpriseStorage es = new EnterpriseStorage();
            ServiceProviderProxy.Init(es, serviceId);
            return es;
        }

        protected static SystemFile[] GetFoldersInternal(int itemId)
        {
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return new SystemFile[0];
                }

                int serviceId = GetEnterpriseStorageServiceID(org.PackageId);

                if (serviceId == 0)
                {
                    return new SystemFile[0];
                }

                EnterpriseStorage es = GetEnterpriseStorage(serviceId);

                var webDavSettings = ObjectUtils.CreateListFromDataReader<WebDavSetting>(
                    DataProvider.GetEnterpriseFolders(itemId)).ToArray();

                return es.GetFolders(org.OrganizationId, webDavSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected static SystemFile[] GetUserRootFoldersInternal(int itemId, int accountId, string userName, string displayName)
        {
            try
            {
                var rootFolders = new List<SystemFile>();

                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return new SystemFile[0];
                }

                int serviceId = GetEnterpriseStorageServiceID(org.PackageId);

                if (serviceId == 0)
                {
                    return new SystemFile[0];
                }

                EnterpriseStorage es = GetEnterpriseStorage(serviceId);

                var webDavSettings = ObjectUtils.CreateListFromDataReader<WebDavSetting>(
                    DataProvider.GetEnterpriseFolders(itemId)).ToArray();

                var userGroups = OrganizationController.GetSecurityGroupsByMember(itemId, accountId);

                foreach (var folder in es.GetFoldersWithoutFrsm(org.OrganizationId, webDavSettings))
                {
                    var permissions = ConvertToESPermission(itemId, folder.Rules);

                    foreach (var permission in permissions)
                    {
                        if ((!permission.IsGroup
                                && (permission.DisplayName == userName || permission.DisplayName == displayName))
                            || (permission.IsGroup && userGroups.Any(x => x.DisplayName == permission.DisplayName)))
                        {
                            rootFolders.Add(folder);
                            break;
                        }
                    }
                }

                return rootFolders.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected static SystemFile GetFolderInternal(int itemId, string folderName, bool loadDriveMapInfo = false)
        {
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return null;
                }

                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));

                var esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, folderName));

                if (esFolder == null)
                {
                    return null;
                }

                if (esFolder.StorageSpaceFolderId == null)
                {
                    var folder = es.GetFolder(org.OrganizationId, folderName, new WebDavSetting(esFolder.LocationDrive, esFolder.HomeFolder, esFolder.Domain));

                    if (folder == null)
                    {
                        return folder;
                    }

                    if (loadDriveMapInfo)
                    {
                        Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                        List<MappedDrive> mappedDrives = orgProxy.GetDriveMaps(org.OrganizationId).ToList();

                        var drive = GetFolderMappedDrive(mappedDrives, folder);

                        if (drive != null)
                        {
                            folder.DriveLetter = drive.DriveLetter;
                        }
                    }

                    return folder;
                }
                else
                {
                    var folder = ConvertToSystemFile(esFolder, org.OrganizationId);

                    if (esFolder.StorageSpaceFolderId != null)
                    {
                        var quota = StorageSpacesController.GetFolderQuota(esFolder.Path, esFolder.StorageSpaceId);

                        if (quota != null)
                        {
                            folder.Size = quota.Usage;
                        }

                        folder.FsrmQuotaType = esFolder.FsrmQuotaType;

                        var ssFolder = StorageSpacesController.GetStorageSpaceFolderById(esFolder.StorageSpaceFolderId.Value);

                        if (ssFolder != null)
                        {
                            folder.UncPath = ssFolder.UncPath;
                        }
                    }

                    if (loadDriveMapInfo)
                    {
                        Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                        List<MappedDrive> mappedDrives = orgProxy.GetDriveMaps(org.OrganizationId).ToList();

                        var drive = GetFolderMappedDrive(mappedDrives, folder);

                        if (drive != null)
                        {
                            folder.DriveLetter = drive.DriveLetter;
                        }
                    }

                    folder.Rules = GetFolderWebDavRulesInternal(itemId, folder.Name);

                    folder.StorageSpaceFolderId = esFolder.StorageSpaceFolderId;

                    return folder;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected static SystemFile RenameFolderInternal(int itemId, string oldFolder, string newFolder)
        {
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return null;
                }

                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));
                int esId =  GetEnterpriseStorageServiceID(org.PackageId);

                var esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, oldFolder));
                var targetFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, newFolder));

                if (targetFolder == null)
                {
                    var folder = GetFolder(itemId, oldFolder);

                    if (folder == null)
                    {
                        throw new Exception("Old folder not found");
                    }

                    var rules = folder.Rules;

                    if (esFolder.StorageSpaceFolderId == null)
                    {
                        es.RenameFolder(org.OrganizationId, oldFolder, newFolder, new WebDavSetting(esFolder.LocationDrive, esFolder.HomeFolder, esFolder.Domain));
                    
                        DataProvider.UpdateEnterpriseFolder(itemId, oldFolder, newFolder, ConvertMegaBytesToGB(ConvertBytesToMB(esFolder.FsrmQuotaSizeBytes)));
                    }
                    else
                    {
                        var result = StorageSpacesController.RenameStorageSpaceFolder(esFolder.StorageSpaceId, esFolder.StorageSpaceFolderId.Value,org.OrganizationId, ResourceGroups.EnterpriseStorage, esFolder.Path, newFolder);

                        if (!result.IsSuccess)
                        {
                            throw new Exception(result.ErrorCodes.First());
                        }

                        esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, oldFolder));

                        DeleteWebDavDirectory(org.PackageId, esFolder.Domain, string.Format("{0}/{1}", org.OrganizationId, esFolder.FolderName));

                        CreateEnterpriseStorageVirtualFolderInternal(org.PackageId, itemId, newFolder, CheckIfSsAndEsOnSameServer(esId,esFolder.StorageSpaceId) ? esFolder.Path : esFolder.UncPath);

                        DataProvider.UpdateEnterpriseFolder(itemId, oldFolder, newFolder, ConvertBytesToMB(esFolder.FsrmQuotaSizeBytes));

                        SetFolderWebDavRulesInternal(itemId, newFolder, ConvertToESPermission(itemId, rules));

                        StorageSpacesController.SetFolderNtfsPermissions(esFolder.StorageSpaceId, esFolder.Path, ConvertToUserPermissions(rules), true, false);
                    }

                    UpdateFolderDriveMapPath(itemId, folder,newFolder );

                    return GetFolder(itemId, newFolder);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckIfSsAndEsOnSameServer(int esId, int ssId)
        {
            var storage = StorageSpacesController.GetStorageSpaceById(ssId);
            var esService = ServerController.GetServiceInfo(esId);

            return storage.ServerId == esService.ServerId;
        }

        protected static ResultObject CreateFolderInternal(int itemId, string folderName, int quota, QuotaType quotaType, bool addDefaultGroup, bool rootFolder = false)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "CREATE_FOLDER");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                long quotaInBytses = ((long)quota) * 1024 * 1024;

                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));

                var webDavSetting = ObjectUtils.FillObjectFromDataReader<WebDavSetting>(
                    DataProvider.GetEnterpriseFolder(itemId, folderName));

                if (webDavSetting == null)
                {
                    int esId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.EnterpriseStorage);

                    StringDictionary esSesstings = ServerController.GetServiceSettings(esId);
                    
                    webDavSetting = new WebDavSetting(esSesstings["LocationDrive"], esSesstings["UsersHome"], esSesstings["UsersDomain"]);

                    StorageSpaceFolder folder = null;

                    if (UsingStorageSpaces(esId) && !rootFolder)
                    {
                        var storageSpaceId = StorageSpacesController.FindBestStorageSpaceService(new EnterpriseStorageSpaceSelector(esId), 
                            ResourceGroups.EnterpriseStorage, quotaInBytses);

                        if (!storageSpaceId.IsSuccess)
                        {
                            throw new Exception(storageSpaceId.ErrorCodes.First());
                        }

                        var storageSpaceFolderResult =
                            StorageSpacesController.CreateStorageSpaceFolder(storageSpaceId.Value, ResourceGroups.EnterpriseStorage,
                                org.OrganizationId, folderName, quotaInBytses, quotaType);

                        if (!storageSpaceFolderResult.IsSuccess)
                        {
                            foreach (var errorCode in storageSpaceFolderResult.ErrorCodes)
                            {
                                result.ErrorCodes.Add(errorCode);
                            }

                            throw new Exception("Error creating cloud folder");
                        }

                        folder = StorageSpacesController.GetStorageSpaceFolderById(storageSpaceFolderResult.Value);

                        DataProvider.AddEntepriseFolder(itemId, folderName, quota, null, null, esSesstings["UsersDomain"], storageSpaceFolderResult.Value);

                        CreateEnterpriseStorageVirtualFolderInternal(org.PackageId, itemId, folderName, CheckIfSsAndEsOnSameServer(esId, folder.StorageSpaceId) ? folder.Path : folder.UncPath);

                        StorageSpacesController.SetStorageSpaceFolderAbeStatus(folder.Id, true);
                    }
                    else
                    {
                        es.CreateFolder(org.OrganizationId, folderName, webDavSetting);

                        DataProvider.AddEntepriseFolder(itemId, folderName, quota, webDavSetting.LocationDrive, webDavSetting.HomeFolder, webDavSetting.Domain, null);

                        SetFolderQuota(org.PackageId, org.OrganizationId, folderName, quota, quotaType, webDavSetting);

                        DataProvider.UpdateEnterpriseFolder(itemId, folderName, folderName, quota);
                    }

                    if (addDefaultGroup)
                    {
                        var groupName = string.Format("{0} Folder Users", folderName);

                        var account = ObjectUtils.CreateListFromDataReader<ExchangeAccount>(
                            DataProvider.GetOrganizationGroupsByDisplayName(itemId, groupName)).FirstOrDefault();

                        var accountId = account == null
                            ? OrganizationController.CreateSecurityGroup(itemId, groupName)
                            : account.AccountId;


                        var securityGroup = OrganizationController.GetSecurityGroupGeneralSettings(itemId, accountId);

                        var rules = new List<WebDavFolderRule>() {
                            new WebDavFolderRule
                            {
                                Roles = new List<string>() { securityGroup.AccountName },
                                Read = true,
                                Write = true,
                                Source = true,
                                Pathes = new List<string>() { "*" }
                           }
                        };

                        es.SetFolderWebDavRules(org.OrganizationId, folderName, null, rules.ToArray());

                        if (UsingStorageSpaces(esId) && folder != null)
                        {
                            StorageSpacesController.SetFolderNtfsPermissions(folder.StorageSpaceId, folder.Path, ConvertToUserPermissions(rules.ToArray()), true, false);
                        }
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.AddError("Cloud Folders", new Exception("Folder already exist"));
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_CREATE_FOLDER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        protected static void ChangeDriveMapFolderPath(int itemId, string oldPath, string newPath )
        {
            // load organization
            Organization org = OrganizationController.GetOrganization(itemId);
            if (org == null)
            {
                return;
            }

            Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

            orgProxy.ChangeDriveMapFolderPath(org.OrganizationId, oldPath, newPath);
        }

        private static void UpdateFolderDriveMapPath(int itemId, SystemFile folder, string newFolder)
        {
            // load organization
            Organization org = OrganizationController.GetOrganization(itemId);
            if (org == null)
            {
                return;
            }

            Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

            var mappedDrive = GetFolderMappedDrive(orgProxy.GetDriveMaps(org.OrganizationId), folder);

            if (mappedDrive != null)
            {
                var oldFolderDriveMapPath = mappedDrive.Path;
                var newPath = GetDriveMapPath(itemId, org.OrganizationId, newFolder);

                ChangeDriveMapFolderPath(itemId, oldFolderDriveMapPath, newPath);
            }
        }


        protected static void SetFRSMQuotaOnFolderInternal(int itemId, string folderName, int quota, QuotaType quotaType)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "SET_FRSM_QUOTA");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return;
                }

                // check if it's not root folder
                if (!string.IsNullOrEmpty(folderName))
                {
                    var webDavSetting = ObjectUtils.FillObjectFromDataReader<WebDavSetting>(
                        DataProvider.GetEnterpriseFolder(itemId, folderName));

                    SetFolderQuota(org.PackageId, org.OrganizationId, folderName, quota, quotaType, webDavSetting);

                    DataProvider.UpdateEnterpriseFolder(itemId, folderName, folderName, quota);
                }
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_SET_FRSM_QUOTA", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }
        }

        protected static ResultObject DeleteFolderInternal(int itemId, string folderName)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "DELETE_FOLDER");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return null;
                }

                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));

                var esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, folderName));

                DeleteMappedDriveInternal(itemId, folderName);

                if (esFolder.StorageSpaceFolderId == null)
                {
                    es.DeleteFolder(org.OrganizationId, folderName, new WebDavSetting(esFolder.LocationDrive,esFolder.HomeFolder,esFolder.Domain));
                }
                else
                {
                    EnterpriseStorageController.DeleteWebDavDirectory(org.PackageId, esFolder.Domain, string.Format("{0}/{1}", org.OrganizationId, esFolder.FolderName));

                    StorageSpacesController.DeleteStorageSpaceFolder(esFolder.StorageSpaceId, esFolder.StorageSpaceFolderId.Value);
                }

                DataProvider.DeleteEnterpriseFolder(itemId, folderName);
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_DELETE_FOLDER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        protected static List<ExchangeAccount> SearchESAccountsInternal(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            // load organization
            Organization org = (Organization)PackageController.GetPackageItem(itemId);
            if (org == null)
                return null;

            string accountTypes = string.Format("{0}, {1}, {2}", ((int)ExchangeAccountType.SecurityGroup),
                (int)ExchangeAccountType.DefaultSecurityGroup, ((int)ExchangeAccountType.User));

            if (PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.Exchange) != 0)
            {
                accountTypes = string.Format("{0}, {1}, {2}, {3}", accountTypes, ((int)ExchangeAccountType.Mailbox),
                ((int)ExchangeAccountType.Room), ((int)ExchangeAccountType.Equipment));
            }

            List<ExchangeAccount> tmpAccounts = ObjectUtils.CreateListFromDataReader<ExchangeAccount>(
                                                  DataProvider.SearchExchangeAccountsByTypes(SecurityContext.User.UserId, itemId,
                                                  accountTypes, filterColumn, filterValue, sortColumn));

            return tmpAccounts;

            // on large lists is very slow

            //List<ExchangeAccount> exAccounts = new List<ExchangeAccount>();

            //foreach (ExchangeAccount tmpAccount in tmpAccounts.ToArray())
            //{
            //    if (tmpAccount.AccountType == ExchangeAccountType.SecurityGroup || tmpAccount.AccountType == ExchangeAccountType.DefaultSecurityGroup
            //            ? OrganizationController.GetSecurityGroupGeneralSettings(itemId, tmpAccount.AccountId) == null
            //            : OrganizationController.GetUserGeneralSettings(itemId, tmpAccount.AccountId) == null)
            //        continue;

            //    exAccounts.Add(tmpAccount);
            //}

            //return exAccounts;
        }

        protected static SystemFilesPaged GetEnterpriseFoldersPagedInternal(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            SystemFilesPaged result = new SystemFilesPaged();

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return null;
                }

                if (CheckUsersDomainExistsInternal(itemId, org.PackageId))
                {
                    EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));

                    DataSet ds = DataProvider.GetEnterpriseFoldersPaged(itemId, "FolderName", string.Format("%{0}%", filterValue), sortColumn, startRow, maximumRows);

                    var esFolders = new List<EsFolder>();

                    ObjectUtils.FillCollectionFromDataView(esFolders, ds.Tables[1].DefaultView);

                    var folders = FillEsFolderEntity(esFolders.Where(x => !string.IsNullOrEmpty(x.FolderName)), org.OrganizationId, org.PackageId, loadUsagesData, loadWebdavRules);

                    if (loadMappedDrives)
                    {
                        Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                        List<MappedDrive> mappedDrives = orgProxy.GetDriveMaps(org.OrganizationId).ToList();

                        foreach (SystemFile folder in folders)
                        {
                            var drive = GetFolderMappedDrive(mappedDrives, folder);

                            if (drive != null)
                            {
                                folder.DriveLetter = drive.DriveLetter;
                            }
                        }
                    }

                    result.RecordsCount = (int)ds.Tables[0].Rows[0][0];
                    result.PageItems = folders.Skip(startRow).Take(maximumRows).ToArray();
                }
            }
            catch(Exception e) 
            { /*skip exception*/}

            return result;
        }

        public static ResultObject MoveToStorageSpace(int itemId, string folderName)
        {
            return MoveToStorageSpaceInternal(itemId, folderName);
        }

        private static ResultObject MoveToStorageSpaceInternal(int itemId, string folderName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "MOVE_TO_STORAGE_SPACE");

            var virDirectoryResult = new ResultObject { IsSuccess = false};
            StorageSpaceFolder storageFolder = null;
            Organization org = null;
            EsFolder esFolder = null;

            try
            {
                // load organization
                org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var esId = GetEnterpriseStorageServiceID(org.PackageId);

                EnterpriseStorage es = GetEnterpriseStorage(esId);

                esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, folderName));

                if (esFolder == null)
                {
                    throw new Exception("Folder not found");
                }

                if (esFolder.StorageSpaceFolderId != null)
                {
                    throw new Exception("Folder is already on Storage Spaces");
                }

                var systemFile = GetFolderInternal(itemId, folderName);

                long quotaInBytses = ((long)systemFile.FRSMQuotaMB) * 1024 * 1024;

                var storageSpaceId = StorageSpacesController.FindBestStorageSpaceService(new EnterpriseStorageSpaceSelector(esId),
                           ResourceGroups.EnterpriseStorage, quotaInBytses);

                if (!storageSpaceId.IsSuccess)
                {
                    throw new Exception(storageSpaceId.ErrorCodes.First());
                }

                var storageFolderResult =
                    StorageSpacesController.CreateStorageSpaceFolder(storageSpaceId.Value, ResourceGroups.EnterpriseStorage,
                        org.OrganizationId, folderName, quotaInBytses, systemFile.FsrmQuotaType);

                if (!storageFolderResult.IsSuccess)
                {
                    foreach (var errorCode in storageFolderResult.ErrorCodes)
                    {
                        result.ErrorCodes.Add(errorCode);
                    }

                    throw new Exception("Error creating storage space folder");
                }

                storageFolder = StorageSpacesController.GetStorageSpaceFolderById(storageFolderResult.Value);

                virDirectoryResult = CreateEnterpriseStorageVirtualFolderInternal(org.PackageId, itemId, folderName, CheckIfSsAndEsOnSameServer(esId, storageFolder.StorageSpaceId) ? storageFolder.Path : storageFolder.UncPath);

                if (!virDirectoryResult.IsSuccess)
                {
                    foreach (var errorCode in virDirectoryResult.ErrorCodes)
                    {
                        result.ErrorCodes.Add(errorCode);
                    }

                    throw new Exception("Error creating virtual folder");
                }

                var webDavResult = es.SetFolderWebDavRules(org.OrganizationId, folderName, null, systemFile.Rules.ToArray());

                if (!webDavResult)
                {
                    throw new Exception("Error updating webdav rules");
                }

                var ntfsResult = StorageSpacesController.SetFolderNtfsPermissions(storageFolder.StorageSpaceId, storageFolder.Path, ConvertToUserPermissions(systemFile.Rules.ToArray()), true, false);

                if (!ntfsResult.IsSuccess)
                {
                    foreach (var errorCode in ntfsResult.ErrorCodes)
                    {
                        result.ErrorCodes.Add(errorCode);
                    }

                    throw new Exception("Error updating NTFS permissions");
                }

                //es.MoveFolder(systemFile.FullName, storageFolder.UncPath);

                DataProvider.UpdateEntepriseFolderStorageSpaceFolder(itemId, folderName, storageFolderResult.Value);

                UpdateFolderDriveMapPath(itemId, systemFile, folderName);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error moving to Storage Space", exception);

                if (storageFolder != null)
                {
                    StorageSpacesController.DeleteStorageSpaceFolder(storageFolder.StorageSpaceId, storageFolder.Id);
                }

                if (virDirectoryResult.IsSuccess && org != null && esFolder != null)
                {
                    EnterpriseStorageController.DeleteWebDavDirectory(org.PackageId, esFolder.Domain, string.Format("{0}/{1}", org.OrganizationId, esFolder.FolderName));
                }
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        public static int GetEnterpriseStorageServiceId(int itemId)
        {
            return GetEnterpriseStorageServiceIdInternal(itemId);
        }

        private static int GetEnterpriseStorageServiceIdInternal(int itemId)
        {
            // load organization
            Organization org = OrganizationController.GetOrganization(itemId);
            if (org == null)
            {
                throw new NullReferenceException("Organization not found");
            }

            int esId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.EnterpriseStorage);

            return esId;
        }

        public static void SetEsFolderShareSettings(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
            SetEsFolderShareSettingsInternal(itemId, folderName, abeIsEnabled, edaIsEnabled);
        }

        private static void SetEsFolderShareSettingsInternal(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
           TaskManager.StartTask("ENTERPRISE_STORAGE", "SET_ES_FOLDER_SHARE_SETTINGS");

            try
            {
                var  esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, folderName));

                if (esFolder == null)
                {
                    throw new Exception("Folder not found");
                }

                if (esFolder.StorageSpaceFolderId == null)
                {
                    throw new Exception("Folder is not Storage Space folder");
                }

                StorageSpacesController.SetStorageSpaceFolderAbeStatus(esFolder.StorageSpaceFolderId.Value, abeIsEnabled);
                StorageSpacesController.SetStorageSpaceFolderEncryptDataAccessStatus(esFolder.StorageSpaceFolderId.Value, edaIsEnabled);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
            }
            finally
            {
               TaskManager.CompleteTask();
            }
        }

        #region WebDav

        protected static int AddWebDavDirectoryInternal(int packageId, string site, string vdirName, string contentpath)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ENTERPRISE_STORAGE", "ADD_VDIR", vdirName);

            TaskManager.WriteParameter("enterprise storage", site);

            try
            {
                // create virtual directory
                WebAppVirtualDirectory dir = new WebAppVirtualDirectory();
                dir.Name = (vdirName ?? string.Empty).Replace("\\","/");
                dir.ContentPath = contentpath;

                dir.EnableAnonymousAccess = false;
                dir.EnableWindowsAuthentication = false;
                dir.EnableBasicAuthentication = false;

                //dir.InstalledDotNetFramework = aspNet;

                dir.DefaultDocs = null; // inherit from service
                dir.HttpRedirect = "";
                dir.HttpErrors = null;
                dir.MimeMaps = null;

                // create directory

                WebServer web = GetWebServer(packageId);
                if (web.AppVirtualDirectoryExists(site, vdirName))
                    return BusinessErrorCodes.ERROR_VDIR_ALREADY_EXISTS;

                web.CreateEnterpriseStorageAppVirtualDirectory(site, dir);

                return 0;

            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        protected static int DeleteWebDavDirectoryInternal(int packageId, string site, string vdirName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ENTERPRISE_STORAGE", "DELETE_VDIR", vdirName);

            TaskManager.WriteParameter("enterprise storage", site);

            try
            {
                int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);

                if (serviceId == -1)
                    return serviceId;

                // create directory
                WebServer web = GetWebServer(packageId);
                if (web.AppVirtualDirectoryExists(site, vdirName))
                    web.DeleteAppVirtualDirectory(site, vdirName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        protected static ResultObject SetFolderWebDavRulesInternal(int itemId, string folder, ESPermission[] permission)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "SET_WEBDAV_FOLDER_RULES");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return null;
                }

                var rules = ConvertToWebDavRule(itemId, permission);

                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));

                var webDavSetting = ObjectUtils.FillObjectFromDataReader<WebDavSetting>(
                    DataProvider.GetEnterpriseFolder(itemId, folder));

                es.SetFolderWebDavRules(org.OrganizationId, folder, webDavSetting, rules);

                var path = GetDriveMapPath(itemId, org.OrganizationId, folder);

                EnterpriseStorageController.SetDriveMapsTargetingFilter(org, permission, path);
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_SET_WEBDAV_FOLDER_RULES", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        protected static WebDavFolderRule[] GetFolderWebDavRulesInternal(int itemId, string folder)
        {
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return null;
                }

                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(org.PackageId));

                var webDavSetting = ObjectUtils.FillObjectFromDataReader<WebDavSetting>(
                    DataProvider.GetEnterpriseFolder(itemId, folder));

                return es.GetFolderWebDavRules(org.OrganizationId, folder, webDavSetting);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Directory Browsing

        private static bool GetDirectoryBrowseEnabledInternal(int itemId, string siteId)
        {
            // load organization
            var org = OrganizationController.GetOrganization(itemId);

            if (org == null)
                return false;

            var webServer = GetWebServer(org.PackageId);

            return webServer.GetDirectoryBrowseEnabled(siteId);
        }

        private static void SetDirectoryBrowseEnabledInternal(int itemId, string siteId, bool enabled)
        {
            // load organization
            var org = OrganizationController.GetOrganization(itemId);

            if (org == null)
                return;


            var webServer = GetWebServer(org.PackageId);

            webServer.SetDirectoryBrowseEnabled(siteId, enabled);
        }

        #endregion

        private static List<SystemFile> FillEsFolderEntity(IEnumerable<EsFolder> esFolders, string organizationId,
            int packageId, bool loadUsedSpace = true, bool loadWebdavRules = true)
        {
            var result = new List<SystemFile>();

            foreach (var esfolder in esFolders)
            {
                var folder = ConvertToSystemFile(esfolder, organizationId);

                if (loadUsedSpace && esfolder.StorageSpaceFolderId != null)
                {
                    var quota = StorageSpacesController.GetFolderQuota(esfolder.Path, esfolder.StorageSpaceId);

                    if (quota != null)
                    {
                        folder.Size = quota.Usage;
                    }

                    folder.FsrmQuotaType = esfolder.FsrmQuotaType;
                }

                result.Add(folder);
            }

            if (loadUsedSpace)
            {
                EnterpriseStorage es = GetEnterpriseStorage(GetEnterpriseStorageServiceID(packageId));
                result = es.GetQuotasForOrganization(result.ToArray()).ToList();
            }

            return result;
        }

        private static SystemFile ConvertToSystemFile(EsFolder esfolder, string organizationId)
        {
            string fullName = esfolder.StorageSpaceFolderId == null
                   ? System.IO.Path.Combine(string.Format("{0}:\\{1}\\{2}", esfolder.LocationDrive, esfolder.HomeFolder, organizationId), esfolder.FolderName)
                   : esfolder.Path;

            var folder = new SystemFile();

            folder.Name = esfolder.FolderName;
            folder.FullName = fullName;
            folder.IsDirectory = true;
            folder.Url = string.Format("https://{0}/{1}/{2}", esfolder.Domain, organizationId, esfolder.FolderName);
            folder.FRSMQuotaMB = esfolder.FolderQuota;
            folder.UncPath = esfolder.UncPath;
            folder.FRSMQuotaGB = ConvertMegaBytesToGB(esfolder.FolderQuota);
            folder.StorageSpaceFolderId = esfolder.StorageSpaceFolderId;

            return folder;
        }

        private static int GetEnterpriseStorageServiceID(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);
        }

        private static EnterpriseStorage GetEnterpriseStorageByPackageId(int packageId)
        {
            var serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);

            return GetEnterpriseStorage(serviceId);
        }

        private static WebDavFolderRule[] ConvertToWebDavRule(int itemId, ESPermission[] permissions)
        {
            var rules = new List<WebDavFolderRule>();

            foreach (var permission in permissions)
            {
                var rule = new WebDavFolderRule();

                var account = ObjectUtils.FillObjectFromDataReader<ExchangeAccount>(DataProvider.GetExchangeAccountByAccountName(itemId, permission.Account));

                if (account.AccountType == ExchangeAccountType.SecurityGroup
                    || account.AccountType == ExchangeAccountType.DefaultSecurityGroup)
                {
                    rule.Roles.Add(permission.Account);
                    permission.IsGroup = true;
                }
                else
                {
                    rule.Users.Add(permission.Account);
                }

                if (permission.Access.ToLower().Contains("read-only"))
                {
                    rule.Read = true;
                }

                if (permission.Access.ToLower().Contains("read-write"))
                {
                    rule.Write = true;
                    rule.Read = true;
                    rule.Source = true;
                }

                rule.Source = true;

                rule.Pathes.Add("*");

                rules.Add(rule);
            }

            return rules.ToArray();
        }

        private static ESPermission[] ConvertToESPermission(int itemId, WebDavFolderRule[] rules)
        {
            var permissions = new List<ESPermission>();

            foreach (var rule in rules)
            {
                var permission = new ESPermission();

                permission.Account = rule.Users.Any() ? rule.Users[0] : rule.Roles[0];

                permission.IsGroup = rule.Roles.Any();

                var orgObj = OrganizationController.GetAccountByAccountName(itemId, permission.Account);

                if (orgObj == null)
                    continue;

                if (permission.IsGroup)
                {
                    var secGroupObj = OrganizationController.GetSecurityGroupGeneralSettings(itemId, orgObj.AccountId);

                    if (secGroupObj == null)
                        continue;

                    permission.DisplayName = secGroupObj.DisplayName;

                }
                else
                {
                    var userObj = OrganizationController.GetUserGeneralSettings(itemId, orgObj.AccountId);

                    if (userObj == null)
                        continue;

                    permission.DisplayName = userObj.DisplayName;
                }

                if (rule.Read && !rule.Write)
                {
                    permission.Access = "Read-Only";
                }
                if (rule.Write)
                {
                    permission.Access = "Read-Write";
                }

                permissions.Add(permission);
            }

            return permissions.ToArray();

        }


        private static UserPermission[] ConvertToUserPermissions(WebDavFolderRule[] rules)
        {
            var users = new List<UserPermission>();

            foreach (var rule in rules)
            {
                foreach (var user in rule.Users)
                {
                    users.Add(new UserPermission
                    {
                        AccountName = user,
                        Read = rule.Read,
                        Write = rule.Write
                    });
                }

                foreach (var user in rule.Roles)
                {
                    users.Add(new UserPermission
                    {
                        AccountName = user,
                        Read = rule.Read,
                        Write = rule.Write
                    });
                }
            }

            return users.ToArray();
        }

        private static UserPermission[] ConvertToUserPermissions(int itemId, ESPermission[] permissions)
        {
            var rules = ConvertToWebDavRule(itemId, permissions);

            return ConvertToUserPermissions(rules);
        }

        private static void SetFolderQuota(int packageId, string orgId, string folderName, int quotaSize, QuotaType quotaType, WebDavSetting setting)
        {
            if (quotaSize == 0)
                return;

            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0)
                return;

            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0)
                return;

            int esServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);

            if (esServiceId != 0)
            {
                var curSetting = setting;

                if (curSetting == null || curSetting.IsEmpty())
                {
                    StringDictionary esSesstings = ServerController.GetServiceSettings(esServiceId);

                    curSetting = new WebDavSetting(esSesstings["LocationDrive"], esSesstings["UsersHome"], esSesstings["UsersDomain"]);
                }

                var orgFolder = Path.Combine(curSetting.HomeFolder, orgId, folderName);

                var os = GetOS(packageId);

                if (os != null && os.CheckFileServicesInstallation())
                {
                    TaskManager.StartTask("FILES", "SET_QUOTA_ON_FOLDER", orgFolder, packageId);

                    try
                    {
                        QuotaValueInfo diskSpaceQuota = PackageController.GetPackageQuota(packageId, Quotas.ENTERPRISESTORAGE_DISKSTORAGESPACE);

                        #region figure Quota Unit

                        // Quota Unit
                        string unit = string.Empty;
                        if (diskSpaceQuota.QuotaDescription.ToLower().Contains("gb"))
                            unit = "GB";
                        else if (diskSpaceQuota.QuotaDescription.ToLower().Contains("mb"))
                            unit = "MB";
                        else
                            unit = "KB";

                        #endregion

                        os.SetQuotaLimitOnFolder(orgFolder, curSetting.LocationDrive, quotaType, quotaSize.ToString() + unit, 0, String.Empty, String.Empty);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex);
                    }
                    finally
                    {
                        TaskManager.CompleteTask();
                    }
                }
            }
        }

        /// <summary>
        /// Get webserver (IIS) installed on server connected with packageId
        /// </summary>
        /// <param name="packageId">packageId parametr</param>
        /// <returns>Configurated webserver or null</returns>
        private static WebServer GetWebServer(int packageId)
        {
            try
            {
                var webGroup = ServerController.GetResourceGroupByName(ResourceGroups.Web);
                var webProviders = ServerController.GetProvidersByGroupID(webGroup.GroupId);
                var esServiceInfo = ServerController.GetServiceInfo(GetEnterpriseStorageServiceID(packageId));

                var serverId = esServiceInfo.ServerId;

                foreach (var webProvider in webProviders)
                {
                    BoolResult result = ServerController.IsInstalled(serverId, webProvider.ProviderId);

                    if (result.IsSuccess && result.Value)
                    {
                        WebServer web = new WebServer();
                        ServerProxyConfigurator cnfg = new ServerProxyConfigurator();

                        cnfg.ProviderSettings.ProviderGroupID = webProvider.GroupId;
                        cnfg.ProviderSettings.ProviderCode = webProvider.ProviderName;
                        cnfg.ProviderSettings.ProviderName = webProvider.DisplayName;
                        cnfg.ProviderSettings.ProviderType = webProvider.ProviderType;

                        //// set service settings
                        //StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);
                        //foreach (string key in serviceSettings.Keys)
                        //    cnfg.ProviderSettings.Settings[key] = serviceSettings[key];
                        cnfg.ProviderSettings.Settings["aspnet40path"] = @"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll";
                        cnfg.ProviderSettings.Settings["aspnet40x64path"] = @"%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll";

                        ServiceProviderProxy.ServerInit(web, cnfg, serverId);

                        return web;
                    }
                }
            }
            catch { /*something wrong*/ }

            return null;
        }

        private static SolidCP.Providers.OS.OperatingSystem GetOS(int packageId)
        {
            var esServiceInfo = ServerController.GetServiceInfo(GetEnterpriseStorageServiceID(packageId));
            var esProviderInfo = ServerController.GetProvider(esServiceInfo.ProviderId);

            var osGroups = ServerController.GetResourceGroupByName(ResourceGroups.Os);
            var osProviders = ServerController.GetProvidersByGroupID(osGroups.GroupId);

            var regexResult = Regex.Match(esProviderInfo.ProviderType, "Windows([0-9]+)");

            if (regexResult.Success)
            {
                foreach (var osProvider in osProviders)
                {
                    BoolResult result = ServerController.IsInstalled(esServiceInfo.ServerId, osProvider.ProviderId);

                    if (result.IsSuccess && result.Value)
                    {
                        var os = new SolidCP.Providers.OS.OperatingSystem();
                        ServerProxyConfigurator cnfg = new ServerProxyConfigurator();

                        cnfg.ProviderSettings.ProviderGroupID = osProvider.GroupId;
                        cnfg.ProviderSettings.ProviderCode = osProvider.ProviderName;
                        cnfg.ProviderSettings.ProviderName = osProvider.DisplayName;
                        cnfg.ProviderSettings.ProviderType = osProvider.ProviderType;

                        ServiceProviderProxy.ServerInit(os, cnfg, esServiceInfo.ServerId);

                        return os;
                    }
                }
            }

            return null;
        }

        public static OrganizationUser[] GetFolderOwaAccounts(int itemId, string folderName)
        {
            try
            {
                var folderId = GetFolderId(itemId, folderName);

                var users = ObjectUtils.CreateListFromDataReader<OrganizationUser>(DataProvider.GetEnterpriseFolderOwaUsers(itemId, folderId));

                return users.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetFolderOwaAccounts(int itemId, string folderName, OrganizationUser[] users)
        {
            try
            {
                var folderId = GetFolderId(itemId, folderName);

                DataProvider.DeleteAllEnterpriseFolderOwaUsers(itemId, folderId);

                foreach (var user in users)
                {
                    DataProvider.AddEnterpriseFolderOwaUser(itemId, folderId, user.AccountId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected static int GetFolderId(int itemId, string folderName)
        {
            try
            {
                GetFolder(itemId, folderName);

                var dataReader = DataProvider.GetEnterpriseFolderId(itemId, folderName);

                while (dataReader.Read())
                {
                    return (int)dataReader[0];
                }

                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> GetUserEnterpriseFolderWithOwaEditPermission(int itemId, List<int> accountIds)
        {
            try
            {
                var result = new List<string>();


                foreach (var accountId in accountIds)
                {
                    var reader = DataProvider.GetUserEnterpriseFolderWithOwaEditPermission(itemId, accountId);

                    while (reader.Read())
                    {
                        result.Add(Convert.ToString(reader["FolderName"]));
                    }
                }

                return result.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region WebDav portal

        public static string GetWebDavPortalUserSettingsByAccountId(int accountId)
        {
            var dataReader = DataProvider.GetWebDavPortalUserSettingsByAccountId(accountId);

            while (dataReader.Read())
            {
                return (string)dataReader["Settings"];
            }

            return null;
        }

        public static void UpdateUserSettings(int accountId, string settings)
        {
            var oldSettings = GetWebDavPortalUserSettingsByAccountId(accountId);

            if (string.IsNullOrEmpty(oldSettings))
            {
                DataProvider.AddWebDavPortalUsersSettings(accountId, settings);
            }
            else
            {
                DataProvider.UpdateWebDavPortalUsersSettings(accountId, settings);
            }
        }

        #endregion


        #region Statistics

        public static OrganizationStatistics GetStatistics(int itemId)
        {
            return GetStatisticsInternal(itemId, false);
        }

        public static OrganizationStatistics GetStatisticsByOrganization(int itemId)
        {
            return GetStatisticsInternal(itemId, true);
        }

        private static OrganizationStatistics GetStatisticsInternal(int itemId, bool byOrganization)
        {
            // place log record
            TaskManager.StartTask("ENTERPRISE_STORAGE", "GET_ORG_STATS", itemId);

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return null;

                OrganizationStatistics stats = new OrganizationStatistics();

                if (byOrganization)
                {
                    SystemFile[] folders = GetEnterpriseFoldersPaged(itemId, false, false, false, "", "", 0, int.MaxValue).PageItems;

                    stats.CreatedEnterpriseStorageFolders = folders.Count();

                    stats.UsedEnterpriseStorageSpace = folders.Where(x => x.FRSMQuotaMB != -1).Sum(x => x.FRSMQuotaMB);
                }
                else
                {
                    UserInfo user = ObjectUtils.FillObjectFromDataReader<UserInfo>(DataProvider.GetUserByExchangeOrganizationIdInternally(org.Id));
                    List<PackageInfo> Packages = PackageController.GetPackages(user.UserId);

                    if ((Packages != null) & (Packages.Count > 0))
                    {
                        foreach (PackageInfo Package in Packages)
                        {
                            List<Organization> orgs = null;

                            orgs = ExchangeServerController.GetExchangeOrganizations(Package.PackageId, false);

                            if ((orgs != null) & (orgs.Count > 0))
                            {
                                foreach (Organization o in orgs)
                                {
                                    SystemFile[] folders = GetEnterpriseFoldersPaged(o.Id, true, false, false, "", "", 0, int.MaxValue).PageItems;

                                    stats.CreatedEnterpriseStorageFolders += folders.Count();

                                    stats.UsedEnterpriseStorageSpace += folders.Where(x => x.FRSMQuotaMB != -1).Sum(x => x.FRSMQuotaMB);
                                }
                            }
                        }
                    }
                }

                // allocated quotas
                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);
                stats.AllocatedEnterpriseStorageSpace = cntx.Quotas[Quotas.ENTERPRISESTORAGE_DISKSTORAGESPACE].GetQuotaAllocatedValue(byOrganization);
                stats.AllocatedEnterpriseStorageFolders = cntx.Quotas[Quotas.ENTERPRISESTORAGE_FOLDERS].GetQuotaAllocatedValue(byOrganization);

                return stats;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        #endregion

        #region Drive Mapping

        public static ResultObject CreateMappedDrive(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            return CreateMappedDriveInternal(packageId, itemId, driveLetter, labelAs, folderName);
        }

        protected static ResultObject CreateMappedDriveInternal(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "CREATE_MAPPED_DRIVE", itemId);

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                int esServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.EnterpriseStorage);

                if (esServiceId != 0)
                {
                    StringDictionary esSesstings = ServerController.GetServiceSettings(esServiceId);

                    Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                    var path = GetDriveMapPath(itemId, org.OrganizationId,folderName);

                    if (orgProxy.CreateMappedDrive(org.OrganizationId, driveLetter, labelAs, path) == 0)
                    {
                        var folder = GetFolder(itemId, folderName);

                        EnterpriseStorageController.SetDriveMapsTargetingFilter(org, ConvertToESPermission(itemId, folder.Rules), path);
                    }
                    else
                    {
                        result.AddError("", new Exception("Organization already has mapped drive for this folder"));
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_CREATE_MAPPED_DRIVE", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static string GetDriveMapPath(int itemId, string organizatinoId, string folderName)
        {
            var esFolder = ObjectUtils.FillObjectFromDataReader<EsFolder>(DataProvider.GetEnterpriseFolder(itemId, folderName));

            return esFolder.StorageSpaceFolderId == null
                ? string.Format(@"\\{0}\{1}\{2}", esFolder.Domain.Split('.')[0], organizatinoId, folderName)
                : esFolder.UncPath;
        }

        public static ResultObject DeleteMappedDrive(int itemId, string folderName)
        {
            return DeleteMappedDriveInternal(itemId, folderName);
        }

        protected static ResultObject DeleteMappedDriveInternal(int itemId, string folderName)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "DELETE_MAPPED_DRIVE", itemId);

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                string path = GetDriveMapPath(itemId, org.OrganizationId, folderName);

                Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                orgProxy.DeleteMappedDriveByPath(org.OrganizationId, path);
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_DELETE_MAPPED_DRIVE", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        public static MappedDrivesPaged GetDriveMapsPaged(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetDriveMapsPagedInternal(itemId, filterValue, sortColumn, startRow, maximumRows);
        }

        protected static MappedDrivesPaged GetDriveMapsPagedInternal(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            MappedDrivesPaged result = new MappedDrivesPaged();

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    return null;
                }

                List<SystemFile> folders = GetEnterpriseFolders(itemId).ToList();

                Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                List<MappedDrive> mappedDrives = orgProxy.GetDriveMaps(org.OrganizationId).Where(x => x.LabelAs.Contains(filterValue)).ToList();
                var resultItems = new List<MappedDrive>();

                foreach (var folder in folders)
                {
                    var drive = GetFolderMappedDrive(mappedDrives, folder);

                    if (drive != null)
                    {
                        resultItems.Add(drive);
                    }
                }

                mappedDrives = resultItems;

                switch (sortColumn)
                {
                    case "Label":
                        mappedDrives = mappedDrives.OrderBy(x => x.LabelAs).ToList();
                        break;
                    default:
                        mappedDrives = mappedDrives.OrderBy(x => x.DriveLetter).ToList();
                        break;
                }

                result.RecordsCount = mappedDrives.Count;
                result.PageItems = mappedDrives.Skip(startRow).Take(maximumRows).ToArray();

            }
            catch (Exception ex) { throw ex; }

            return result;
        }

        private static MappedDrive GetFolderMappedDrive(IEnumerable<MappedDrive> drives, SystemFile folder)
        {
            foreach (MappedDrive drive in drives)
            {
                var name = folder.StorageSpaceFolderId == null ? folder.Name : folder.UncPath.Split('\\').Last();

                if (drive.Path.Split('\\').Last() == name)
                {
                    drive.Folder = folder;

                    return drive;
                }
            }

            return null;
        }

        public static string[] GetUsedDriveLetters(int itemId)
        {
            return GetUsedDriveLettersInternal(itemId);
        }

        protected static string[] GetUsedDriveLettersInternal(int itemId)
        {
            List<string> driveLetters = new List<string>();

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    return null;
                }

                Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                driveLetters = orgProxy.GetDriveMaps(org.OrganizationId).Select(x => x.DriveLetter).ToList();
            }
            catch (Exception ex) { throw ex; }

            return driveLetters.ToArray();

        }

        public static SystemFile[] GetNotMappedEnterpriseFolders(int itemId)
        {
            return GetNotMappedEnterpriseFoldersInternal(itemId);
        }

        protected static SystemFile[] GetNotMappedEnterpriseFoldersInternal(int itemId)
        {
            List<SystemFile> folders = new List<SystemFile>();
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    return null;
                }

                if (CheckUsersDomainExistsInternal(itemId, org.PackageId))
                {
                    folders = GetEnterpriseFoldersPaged(itemId, false,false,true,"","",0,int.MaxValue).PageItems.ToList();

                    Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                    List<MappedDrive> drives = orgProxy.GetDriveMaps(org.OrganizationId).ToList();

                    folders = folders.Where(x => GetFolderMappedDrive(drives, x) == null).ToList();
                }
            }
            catch (Exception ex) { throw ex; }

            return folders.ToArray();
        }

        private static ResultObject SetDriveMapsTargetingFilter(Organization org, ESPermission[] permissions, string folderName)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "SET_MAPPED_DRIVE_TARGETING_FILTER");

            try
            {
                Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                List<ExchangeAccount> accounts = new List<ExchangeAccount>();

                foreach (var permission in permissions)
                {
                    accounts.Add(ObjectUtils.FillObjectFromDataReader<ExchangeAccount>(DataProvider.GetExchangeAccountByAccountName(org.Id, permission.Account)));
                }

                orgProxy.SetDriveMapsTargetingFilter(org.OrganizationId, accounts.ToArray(), folderName);
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_SET_MAPPED_DRIVE_TARGETING_FILTER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject DeleteMappedDrivesGPO(int itemId)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("ENTERPRISE_STORAGE", "DELETE_MAPPED_DRIVES_GPO", itemId);

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                Organizations orgProxy = OrganizationController.GetOrganizationProxy(org.ServiceId);

                orgProxy.DeleteMappedDrivesGPO(org.OrganizationId);
            }
            catch (Exception ex)
            {
                result.AddError("ENTERPRISE_STORAGE_DELETE_MAPPED_DRIVES_GPO", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        #endregion


        public static int ConvertMegaBytesToGB(int megabytes)
        {
            int OneGb = 1024;

            if (megabytes == -1)
                return megabytes;

            return (int)(megabytes / OneGb);
        }

        public static int ConvertBytesToMB(long bytes)
        {
            int OneKb = 1024;
            int OneMb = OneKb * 1024;

            if (bytes == 0)
                return 0;

            return (int)(bytes / OneMb);
        }

        private static bool UsingStorageSpaces(int serviceId)
        {
            var settings = ServerController.GetServiceSettings(serviceId);

            if (settings == null)
            {
                return false;
            }

            if (!settings.ContainsKey(UseStorageSpaces))
            {
                return false;
            }

            if (string.IsNullOrEmpty(settings[UseStorageSpaces]))
            {
                return false;
            }

            return Convert.ToBoolean(settings[UseStorageSpaces]);
        }
    }
}
