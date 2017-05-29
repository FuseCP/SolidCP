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
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using SolidCP.Server;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.Web;
using SolidCP.Providers.Database;
using SolidCP.Providers.SharePoint;
using OS = SolidCP.Providers.OS;

namespace SolidCP.EnterpriseServer
{
    public class SharePointServerController : IImportController, IBackupController
    {
        private const int FILE_BUFFER_LENGTH = 5000000; // ~5MB

        private static SharePointServer GetSharePoint(int serviceId)
        {
            SharePointServer sps = new SharePointServer();
            ServiceProviderProxy.Init(sps, serviceId);
            return sps;
        }

        #region Sites
        public static DataSet GetRawSharePointSitesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, typeof(SharePointSite),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<SharePointSite> GetSharePointSites(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(SharePointSite), recursive);

            return items.ConvertAll<SharePointSite>(
                new Converter<ServiceProviderItem, SharePointSite>(ConvertItemToSharePointSiteItem));
        }

        private static SharePointSite ConvertItemToSharePointSiteItem(ServiceProviderItem item)
        {
            return (SharePointSite)item;
        }

        public static SharePointSite GetSite(int itemId)
        {
            // load meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            return item;
        }

        public static int AddSite(SharePointSite item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.SHAREPOINT_SITES);
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_SHAREPOINT_RESOURCE_QUOTA_LIMIT;

            // check if stats resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.SharePoint);
            if (serviceId == 0)
                return BusinessErrorCodes.ERROR_SHAREPOINT_RESOURCE_UNAVAILABLE;

            // check package items
            if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(SharePointSite)) != null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_EXISTS;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "ADD_SITE", item.Name);
            TaskManager.WriteParameter("Database group", item.DatabaseGroupName);
            TaskManager.WriteParameter("Database name", item.DatabaseName);
            TaskManager.WriteParameter("Database user", item.DatabaseUser);

            int databaseItemId = 0;
            int databaseUserItemId = 0;

            try
            {
                // load web site
                WebSite siteItem = (WebSite)PackageController.GetPackageItemByName(item.PackageId,
                    item.Name, typeof(WebSite));

                if (siteItem == null)
                    return BusinessErrorCodes.ERROR_WEB_SITE_SERVICE_UNAVAILABLE;

                // get service web site
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                WebSite site = web.GetSite(siteItem.SiteId);

                /////////////////////////////////////////
                //
                //  PREPARE SHAREPOINT SITE INSTALLATION
                //

				ServiceInfo wssService = ServerController.GetServiceInfo(serviceId);
				bool wss30 = (wssService.ProviderId == 23); // WSS 3.0

				// remember original web site bindings
				ServerBinding[] bindings = site.Bindings;

				// set application pool and root folder
				item.ApplicationPool = site.ApplicationPool;
				item.RootFolder = site.ContentPath;

                // change web site .NET framework if required
				bool siteUpdated = false;
                if (!wss30 && (site.AspNetInstalled != "1"
                    || site.DedicatedApplicationPool))
                {
                    site.AspNetInstalled = "1";
                    site.DedicatedApplicationPool = false;
                    web.UpdateSite(site);

					siteUpdated = true;
                }

				if (wss30 && site.AspNetInstalled != "2")
				{
					site.AspNetInstalled = "2";
					web.UpdateSite(site);

					siteUpdated = true;
				}

				if (siteUpdated)
				{
					site = web.GetSite(siteItem.SiteId);
					item.ApplicationPool = site.ApplicationPool;
				}

                if (site.FrontPageInstalled)
                {
                    // remove FrontPage
                    web.UninstallFrontPage(siteItem.SiteId, siteItem.FrontPageAccount);
                }
                
                // create SQL database
                SqlDatabase database = new SqlDatabase();
                database.PackageId = item.PackageId;
                database.Name = item.DatabaseName;
                databaseItemId = DatabaseServerController.AddSqlDatabase(database, item.DatabaseGroupName);
                if (databaseItemId < 0)
                    return databaseItemId;

                // create SQL user
                SqlUser dbUser = new SqlUser();
                dbUser.PackageId = item.PackageId;
                dbUser.Name = item.DatabaseUser;
                dbUser.Password = item.DatabasePassword;
                databaseUserItemId = DatabaseServerController.AddSqlUser(dbUser, item.DatabaseGroupName);
                if (databaseUserItemId < 0)
                    return databaseUserItemId;

                // delete SQL database from service
                // and change database user role
                int sqlServiceId = PackageController.GetPackageServiceId(item.PackageId, item.DatabaseGroupName);
                if (sqlServiceId < 0)
                    return BusinessErrorCodes.ERROR_MSSQL_RESOURCE_UNAVAILABLE;

                // load server settings
                StringDictionary sqlSettings = ServerController.GetServiceSettings(sqlServiceId);
                item.DatabaseServer = sqlSettings["ExternalAddress"];

                DatabaseServer dbServer = new DatabaseServer();
                ServiceProviderProxy.Init(dbServer, sqlServiceId);

                // delete database from service
                dbServer.DeleteDatabase(database.Name);

                // give SQL user "db_creator" role
                dbServer.ExecuteSqlNonQuery("master", String.Format(
                    "sp_addsrvrolemember '{0}', 'dbcreator'\nGO", dbUser.Name));

                // install SharePoint site
                SharePointServer sps = GetSharePoint(serviceId);
                sps.ExtendVirtualServer(item);

                // remove SQL user from "db_creator" role
                dbServer.ExecuteSqlNonQuery("master", String.Format(
                    "sp_dropsrvrolemember '{0}', 'dbcreator'\nGO", dbUser.Name));

				// restore original web site bindings
				web.UpdateSiteBindings(site.SiteId, bindings, false);

                // save statistics item
                item.ServiceId = serviceId;
                int itemId = PackageController.AddPackageItem(item);

                TaskManager.ItemId = itemId;

                return itemId;
            }
            catch (Exception ex)
            {
                // delete database if required
                if (databaseItemId > 0)
                    DatabaseServerController.DeleteSqlDatabase(databaseItemId);

                // delete user if required
                if (databaseUserItemId > 0)
                    DatabaseServerController.DeleteSqlUser(databaseUserItemId);

                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteSite(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SharePointSite origItem = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "DELETE_SITE", origItem.Name, itemId);

            try
            {
                // get service
                SharePointServer sps = GetSharePoint(origItem.ServiceId);

                // delete service item
                sps.UnextendVirtualServer(origItem.Name, true);

                try
                {
                    // delete database
                    ServiceProviderItem dbItem = PackageController.GetPackageItemByName(origItem.PackageId, origItem.DatabaseGroupName,
                        origItem.DatabaseName, typeof(SqlDatabase));
                    if (dbItem != null)
                        DatabaseServerController.DeleteSqlDatabase(dbItem.Id);

                    // delete database user
                    dbItem = PackageController.GetPackageItemByName(origItem.PackageId, origItem.DatabaseGroupName,
                        origItem.DatabaseUser, typeof(SqlUser));
                    if (dbItem != null)
                        DatabaseServerController.DeleteSqlUser(dbItem.Id);
                }
                catch (Exception ex2)
                {
                    TaskManager.WriteError(ex2);
                }

                // delete meta item
                PackageController.DeletePackageItem(origItem.Id);

				try
				{
					// start web site
					WebSite site = WebServerController.GetWebSite(origItem.PackageId, origItem.Name);
					if(site != null)
						WebServerController.ChangeSiteState(site.Id, ServerState.Started);
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex);
				}

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
        #endregion

        #region Backup/Restore
        public static string BackupVirtualServer(int itemId, string fileName,
            bool zipBackup, bool download, string folderName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return null;

            // load original meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "BACKUP_SITE", item.Name, itemId);

            try
            {
                SharePointServer sps = GetSharePoint(item.ServiceId);
                string backFile = sps.BackupVirtualServer(item.Name, fileName, zipBackup);

                if (!download)
                {
                    // copy backup files to space folder
                    string relFolderName = FilesController.CorrectRelativePath(folderName);
                    if (!relFolderName.EndsWith("\\"))
                        relFolderName = relFolderName + "\\";

                    // create backup folder if not exists
                    if (!FilesController.DirectoryExists(item.PackageId, relFolderName))
                        FilesController.CreateFolder(item.PackageId, relFolderName);

                    string packageFile = relFolderName + Path.GetFileName(backFile);

                    // delete destination file if exists
                    if (FilesController.FileExists(item.PackageId, packageFile))
                        FilesController.DeleteFiles(item.PackageId, new string[] { packageFile });

                    byte[] buffer = null;

                    int offset = 0;
                    do
                    {
                        // read remote content
                        buffer = sps.GetTempFileBinaryChunk(backFile, offset, FILE_BUFFER_LENGTH);

                        // write remote content
                        FilesController.AppendFileBinaryChunk(item.PackageId, packageFile, buffer);

                        offset += FILE_BUFFER_LENGTH;
                    }
                    while (buffer.Length == FILE_BUFFER_LENGTH);

                }

                return backFile;
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

        public static byte[] GetSharePointBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            // load original meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            SharePointServer sps = GetSharePoint(item.ServiceId);
            return sps.GetTempFileBinaryChunk(path, offset, length);
        }

        public static string AppendSharePointBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            // load original meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            SharePointServer sps = GetSharePoint(item.ServiceId);
            return sps.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public static int RestoreVirtualServer(int itemId, string uploadedFile, string packageFile)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "RESTORE_SITE", item.Name, itemId);

            try
            {

                SharePointServer sps = GetSharePoint(item.ServiceId);

                string backupFile = null;
                if (!String.IsNullOrEmpty(packageFile))
                {
                    // copy package files to the remote SharePoint Server
                    string path = null;
                    byte[] buffer = null;

                    int offset = 0;
                    do
                    {
                        // read package file
                        buffer = FilesController.GetFileBinaryChunk(item.PackageId, packageFile, offset, FILE_BUFFER_LENGTH);

                        // write remote backup file
                        string tempPath = sps.AppendTempFileBinaryChunk(Path.GetFileName(packageFile), path, buffer);
                        if (path == null)
                        {
                            path = tempPath;
                            backupFile = path;
                        }

                        offset += FILE_BUFFER_LENGTH;
                    }
                    while (buffer.Length == FILE_BUFFER_LENGTH);
                }
                else if (!String.IsNullOrEmpty(uploadedFile))
                {
                    // upladed files
                    backupFile = uploadedFile;
                }

                // restore
                if (!String.IsNullOrEmpty(backupFile))
                    sps.RestoreVirtualServer(item.Name, backupFile);

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
        #endregion

        #region Web Parts
        public static string[] GetInstalledWebParts(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return null;

            // load original meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            SharePointServer sps = GetSharePoint(item.ServiceId);
            return sps.GetInstalledWebParts(item.Name);
        }

        public static int InstallWebPartsPackage(int itemId, string uploadedFile, string packageFile)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "INSTALL_WEBPARTS", item.Name, itemId);

            TaskManager.WriteParameter("Package file", packageFile);

            try
            {

                SharePointServer sps = GetSharePoint(item.ServiceId);

                string backupFile = null;
                if (!String.IsNullOrEmpty(packageFile))
                {
                    // copy package files to the remote SharePoint Server
                    string path = null;
                    byte[] buffer = null;

                    int offset = 0;
                    do
                    {
                        // read package file
                        buffer = FilesController.GetFileBinaryChunk(item.PackageId, packageFile, offset, FILE_BUFFER_LENGTH);

                        // write remote backup file
                        string tempPath = sps.AppendTempFileBinaryChunk(Path.GetFileName(packageFile), path, buffer);
                        if (path == null)
                        {
                            path = tempPath;
                            backupFile = path;
                        }

                        offset += FILE_BUFFER_LENGTH;
                    }
                    while (buffer.Length == FILE_BUFFER_LENGTH);
                }
                else if (!String.IsNullOrEmpty(uploadedFile))
                {
                    // upladed files
                    backupFile = uploadedFile;
                }

                // restore
                if (!String.IsNullOrEmpty(backupFile))
                    sps.InstallWebPartsPackage(item.Name, backupFile);

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

        public static int DeleteWebPartsPackage(int itemId, string packageName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SharePointSite item = (SharePointSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "UNINSTALL_WEBPARTS", item.Name, itemId);

            TaskManager.WriteParameter("Package name", packageName);

            try
            {

                SharePointServer sps = GetSharePoint(item.ServiceId);

                // uninstall webparts
                if (!String.IsNullOrEmpty(packageName))
                    sps.DeleteWebPartsPackage(item.Name, packageName);

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
        #endregion

        #region Users
        public static DataSet GetRawSharePointUsersPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, ResourceGroups.SharePoint, typeof(SystemUser),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<SystemUser> GetSharePointUsers(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, ResourceGroups.SharePoint, typeof(SystemUser), recursive);

            return items.ConvertAll<SystemUser>(
                new Converter<ServiceProviderItem, SystemUser>(ConvertItemToSharePointUser));
        }

        private static SystemUser ConvertItemToSharePointUser(ServiceProviderItem item)
        {
            return (SystemUser)item;
        }

        public static SystemUser GetSharePointUser(int itemId)
        {
            // load meta item
            SystemUser item = (SystemUser)PackageController.GetPackageItem(itemId);

            // load service item
            SharePointServer sps = GetSharePoint(item.ServiceId);
            SystemUser user = sps.GetUser(item.Name);

            // add common properties
            user.Id = item.Id;
            user.PackageId = item.PackageId;
            user.ServiceId = item.ServiceId;

            return user;
        }

        public static int AddSharePointUser(SystemUser item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.SHAREPOINT_USERS);
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_SHAREPOINT_USERS_RESOURCE_QUOTA_LIMIT;

            // check if mail resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.SharePoint);
            if (serviceId == 0)
                return BusinessErrorCodes.ERROR_SHAREPOINT_RESOURCE_UNAVAILABLE;

            // check package items
            if (PackageController.GetPackageItemByName(item.PackageId, ResourceGroups.SharePoint, item.Name, typeof(SystemUser)) != null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_USERS_PACKAGE_ITEM_EXISTS;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "ADD_USER", item.Name);

            try
            {
                // check service items
                SharePointServer sps = GetSharePoint(serviceId);
                if (sps.UserExists(item.Name))
                    return BusinessErrorCodes.ERROR_SHAREPOINT_USERS_SERVICE_ITEM_EXISTS;

                // create service item
                item.FullName = item.Name;
                item.Description = "SolidCP System Account";
                item.AccountDisabled = false;
                item.PasswordCantChange = true;
                item.PasswordNeverExpires = true;

                // add service item
                sps.CreateUser(item);

                // save item
                item.Password = CryptoUtils.Encrypt(item.Password);
                item.ServiceId = serviceId;
                int itemId = PackageController.AddPackageItem(item);

                TaskManager.ItemId = itemId;

                return itemId;
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

        public static int UpdateSharePointUser(SystemUser item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SystemUser origItem = (SystemUser)PackageController.GetPackageItem(item.Id);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_USERS_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "UPDATE_USER", origItem.Name, item.Id);

            try
            {
                // get service
                SharePointServer sps = GetSharePoint(origItem.ServiceId);

                item.Name = origItem.Name;
                item.FullName = origItem.Name;
                item.Description = "SolidCP System Account";
                item.AccountDisabled = false;
                item.PasswordCantChange = true;
                item.PasswordNeverExpires = true;

                // update service item
                sps.UpdateUser(item);

                // update meta item
                if (item.Password != "")
                {
                    item.Password = CryptoUtils.Encrypt(item.Password);
                    PackageController.UpdatePackageItem(item);
                }

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

        public static int DeleteSharePointUser(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SystemUser origItem = (SystemUser)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_USERS_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "DELETE_USER", origItem.Name, itemId);

            try
            {
                // get service
                SharePointServer sps = GetSharePoint(origItem.ServiceId);

                // delete service item
                sps.DeleteUser(origItem.Name);

                // delete meta item
                PackageController.DeletePackageItem(origItem.Id);

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

        #endregion

        #region Groups
        public static DataSet GetRawSharePointGroupsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, ResourceGroups.SharePoint, typeof(SystemGroup),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<SystemGroup> GetSharePointGroups(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, ResourceGroups.SharePoint, typeof(SystemGroup), recursive);

            return items.ConvertAll<SystemGroup>(
                new Converter<ServiceProviderItem, SystemGroup>(ConvertItemToSharePointGroup));
        }

        private static SystemGroup ConvertItemToSharePointGroup(ServiceProviderItem item)
        {
            return (SystemGroup)item;
        }

        public static SystemGroup GetSharePointGroup(int itemId)
        {
            // load meta item
            SystemGroup item = (SystemGroup)PackageController.GetPackageItem(itemId);

            // load service item
            SharePointServer sps = GetSharePoint(item.ServiceId);
            SystemGroup group = sps.GetGroup(item.Name);

            // add common properties
            group.Id = item.Id;
            group.PackageId = item.PackageId;
            group.ServiceId = item.ServiceId;

            return group;
        }

        public static int AddSharePointGroup(SystemGroup item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.SHAREPOINT_GROUPS);
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_SHAREPOINT_GROUPS_RESOURCE_QUOTA_LIMIT;

            // check if mail resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.SharePoint);
            if (serviceId == 0)
                return BusinessErrorCodes.ERROR_SHAREPOINT_RESOURCE_UNAVAILABLE;

            // check package items
            if (PackageController.GetPackageItemByName(item.PackageId, ResourceGroups.SharePoint, item.Name, typeof(SystemGroup)) != null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_GROUPS_PACKAGE_ITEM_EXISTS;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "ADD_GROUP", item.Name);

            try
            {
                // check service items
                SharePointServer sps = GetSharePoint(serviceId);
                if (sps.GroupExists(item.Name))
                    return BusinessErrorCodes.ERROR_SHAREPOINT_GROUPS_SERVICE_ITEM_EXISTS;

                item.Description = "SolidCP System Group";

                // add service item
                sps.CreateGroup(item);

                // save item
                item.ServiceId = serviceId;
                int itemId = PackageController.AddPackageItem(item);

                TaskManager.ItemId = itemId;

                return itemId;
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

        public static int UpdateSharePointGroup(SystemGroup item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SystemGroup origItem = (SystemGroup)PackageController.GetPackageItem(item.Id);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_GROUPS_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "UPDATE_GROUP", origItem.Name, item.Id);

            try
            {
                // get service
                SharePointServer sps = GetSharePoint(origItem.ServiceId);

                item.Description = "SolidCP System Group";

                // update service item
                sps.UpdateGroup(item);

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

        public static int DeleteSharePointGroup(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SystemGroup origItem = (SystemGroup)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_SHAREPOINT_GROUPS_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("SHAREPOINT", "DELETE_GROUP", origItem.Name, itemId);

            try
            {
                // get service
                SharePointServer sps = GetSharePoint(origItem.ServiceId);

                // delete service item
                sps.DeleteGroup(origItem.Name);

                // delete meta item
                PackageController.DeletePackageItem(origItem.Id);

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
        #endregion

        #region IImportController Members

        public List<string> GetImportableItems(int packageId, int itemTypeId, Type itemType, ResourceGroupInfo group)
        {
            List<string> items = new List<string>();

            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return items;

            SharePointServer sps = GetSharePoint(serviceId);
            if (itemType == typeof(SystemUser))
                items.AddRange(sps.GetUsers());
            else if (itemType == typeof(SystemGroup))
                items.AddRange(sps.GetGroups());

            return items;
        }

        public void ImportItem(int packageId, int itemTypeId, Type itemType,
			ResourceGroupInfo group, string itemName)
        {
            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return;

            if (itemType == typeof(SystemUser))
            {
                // import user
                SystemUser user = new SystemUser();
                user.ServiceId = serviceId;
                user.PackageId = packageId;
                user.Name = itemName;
                user.Password = "";
                user.GroupName = group.GroupName;
                PackageController.AddPackageItem(user);
            }
            else if (itemType == typeof(SystemGroup))
            {
                // import group
                SystemGroup spGroup = new SystemGroup();
                spGroup.ServiceId = serviceId;
                spGroup.PackageId = packageId;
                spGroup.Name = itemName;
                spGroup.GroupName = group.GroupName;
                PackageController.AddPackageItem(spGroup);
            }
        }

        #endregion

        #region IBackupController Members

        public int BackupItem(string tempFolder, System.Xml.XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
        {
            if (item is SystemUser)
            {
                // backup system user
                SharePointServer sps = GetSharePoint(item.ServiceId);

                // read user info
                SystemUser user = sps.GetUser(item.Name);
                user.Password = ((SystemUser)item).Password;

                XmlSerializer serializer = new XmlSerializer(typeof(SystemUser));
                serializer.Serialize(writer, user);
            }
            else if (item is SystemGroup)
            {
                // backup system group
                SharePointServer sps = GetSharePoint(item.ServiceId);

                // read site info
                SystemGroup sysGroup = sps.GetGroup(item.Name);

                XmlSerializer serializer = new XmlSerializer(typeof(SystemGroup));
                serializer.Serialize(writer, group);
            }
            return 0;
        }

        public int RestoreItem(string tempFolder, System.Xml.XmlNode itemNode, int itemId, Type itemType, string itemName, int packageId, int serviceId, ResourceGroupInfo group)
        {
            if (itemType == typeof(SystemUser))
            {
                SharePointServer sps = GetSharePoint(serviceId);

                // extract meta item
                XmlSerializer serializer = new XmlSerializer(typeof(SystemUser));
                SystemUser user = (SystemUser)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("SystemUser")));

                // create user if required
                if (!sps.UserExists(itemName))
                {
                    user.Password = CryptoUtils.Decrypt(user.Password);
                    sps.CreateUser(user);

                    // restore password
                    user.Password = CryptoUtils.Encrypt(user.Password);
                }

                // add meta-item if required
                if (PackageController.GetPackageItemByName(packageId, itemName, typeof(SystemUser)) == null)
                {
                    user.PackageId = packageId;
                    user.ServiceId = serviceId;
                    PackageController.AddPackageItem(user);
                }
            }
            else if (itemType == typeof(SystemGroup))
            {
                SharePointServer sps = GetSharePoint(serviceId);

                // extract meta item
                XmlSerializer serializer = new XmlSerializer(typeof(SystemGroup));
                SystemGroup sysGroup = (SystemGroup)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("SystemGroup")));

                // create user if required
                if (!sps.GroupExists(itemName))
                {
                    sps.CreateGroup(sysGroup);
                }

                // add meta-item if required
                if (PackageController.GetPackageItemByName(packageId, itemName, typeof(SystemGroup)) == null)
                {
                    sysGroup.PackageId = packageId;
                    sysGroup.ServiceId = serviceId;
                    PackageController.AddPackageItem(sysGroup);
                }
            }

            return 0;
        }

        #endregion
    }
}
