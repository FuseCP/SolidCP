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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Serialization;
using System.Web;

using SolidCP.Providers;
using SolidCP.Providers.Database;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.EnterpriseServer
{
    public class DatabaseServerController : IImportController, IBackupController
    {
        private const int FILE_BUFFER_LENGTH = 5000000; // ~5MB

        public static DatabaseServer GetDatabaseServer(int serviceId)
        {
            DatabaseServer db = new DatabaseServer();
            ServiceProviderProxy.Init(db, serviceId);
            return db;
        }

        #region Databases
        public static DataSet GetRawSqlDatabasesPaged(int packageId,
            string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, groupName, typeof(SqlDatabase),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<SqlDatabase> GetSqlDatabases(int packageId, string groupName, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, groupName, typeof(SqlDatabase), recursive);

            return items.ConvertAll<SqlDatabase>(
                new Converter<ServiceProviderItem, SqlDatabase>(ConvertItemToSqlDatabase));
        }

        private static SqlDatabase ConvertItemToSqlDatabase(ServiceProviderItem item)
        {
            return (SqlDatabase)item;
        }

        public static SqlDatabase GetSqlDatabase(int itemId)
        {
            // load meta item
            SqlDatabase item = (SqlDatabase)PackageController.GetPackageItem(itemId);

            // load service item
            DatabaseServer sql = GetDatabaseServer(item.ServiceId);
            SqlDatabase database = sql.GetDatabase(item.Name);

            if (database == null)
                return item;

            // add common properties
            database.Id = item.Id;
            database.PackageId = item.PackageId;
            database.ServiceId = item.ServiceId;
            database.GroupName = item.GroupName;

            StringDictionary settings = ServerController.GetServiceSettings(item.ServiceId);


            if (settings["InternalAddress"] != null) database.InternalServerName = settings["InternalAddress"];
            if (settings["ExternalAddress"] != null) database.ExternalServerName = settings["ExternalAddress"];

            return database;
        }

        public static int AddSqlDatabase(SqlDatabase item, string groupName)
        {
			// check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, groupName + ".Databases");
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_MSSQL_DATABASES_RESOURCE_QUOTA_LIMIT;

            // check if mail resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, groupName);
            if (serviceId == 0)
                return BusinessErrorCodes.ERROR_MSSQL_RESOURCE_UNAVAILABLE;

            // check service items
            if (PackageController.GetServiceItemsCountByNameAndServiceId(serviceId, groupName, item.Name, typeof(SqlDatabase)) > 0)
                return BusinessErrorCodes.ERROR_MSSQL_DATABASES_PACKAGE_ITEM_EXISTS;

            // place log record
            TaskManager.StartTask("SQL_DATABASE", "ADD", item.Name);
            TaskManager.WriteParameter("Provider", groupName);

            int itemId = default(int);
			//
            try
            {
                // check service items
                DatabaseServer sql = GetDatabaseServer(serviceId);
                if (sql.DatabaseExists(item.Name))
                    return BusinessErrorCodes.ERROR_MSSQL_DATABASES_SERVICE_ITEM_EXISTS;

                // calculate database location
                StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                UserInfo user = PackageController.GetPackageOwner(item.PackageId);
                if (settings["UseDefaultDatabaseLocation"] != null &&
                    !Utils.ParseBool(settings["UseDefaultDatabaseLocation"], false))
                {
                    item.Location = Utils.ReplaceStringVariable(settings["DatabaseLocation"], "user_name", user.Username);
                }

                // set database size
                item.DataSize = GetMaxDatabaseSize(item.PackageId, groupName);

                
                // set log size
                item.LogSize = GetMaxLogSize(item.PackageId, groupName);

                // add service item
                sql.CreateDatabase(item);

                // save item
                item.ServiceId = serviceId;
                itemId = PackageController.AddPackageItem(item);

                TaskManager.ItemId = itemId;
                                
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
				//
                if (ex.Message.Contains("INVALID_DATABASE_NAME"))
                    return BusinessErrorCodes.ERROR_MYSQL_INVALID_DATABASE_NAME;
				// Return a generic error instead of default(int)
				itemId = BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
            return itemId;
        }

        public static int UpdateSqlDatabase(SqlDatabase item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SqlDatabase origItem = (SqlDatabase)PackageController.GetPackageItem(item.Id);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_MSSQL_DATABASES_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SQL_DATABASE", "UPDATE", origItem.Name, item.Id);

            TaskManager.WriteParameter("Provider", origItem.GroupName);

            try
            {
                // get service
                DatabaseServer sql = GetDatabaseServer(origItem.ServiceId);

                // update service item
                sql.UpdateDatabase(item);
                return 0;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
				// Return a generic error instead of re-throwing an exception
				return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteSqlDatabase(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SqlDatabase origItem = (SqlDatabase)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_MSSQL_DATABASES_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("SQL_DATABASE", "DELETE", origItem.Name, itemId, new BackgroundTaskParameter("Provider", origItem.GroupName));

            try
            {
                // get service
                DatabaseServer sql = GetDatabaseServer(origItem.ServiceId);

                // delete service item
                sql.DeleteDatabase(origItem.Name);

                // delete meta item
                PackageController.DeletePackageItem(origItem.Id);

                return 0;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
				// Return a generic error instead of re-throwing an exception
				return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        
        private static int GetMaxSize(int packageId, string groupName, string prefix)
        {
            // load package context
            int maxSize = 0; // unlimited
            string quotaName = groupName + prefix;
            PackageContext cntx = PackageController.GetPackageContext(packageId);
            if (cntx != null && cntx.Quotas.ContainsKey(quotaName))
            {
                maxSize = cntx.Quotas[quotaName].QuotaAllocatedValue;
                if (maxSize == -1)
                    maxSize = 0;
            }
            return maxSize;
        }
        
        private static int GetMaxDatabaseSize(int packageId, string groupName)
        {
            return GetMaxSize(packageId, groupName, ".MaxDatabaseSize");            
        }

        private static int GetMaxLogSize(int packageId, string groupName)
        {
            return GetMaxSize(packageId, groupName, ".MaxLogSize");
        }

        public static string BackupSqlDatabase(int itemId, string backupName,
            bool zipBackup, bool download, string folderName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return null;

            // load original meta item
            SqlDatabase item = (SqlDatabase)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            // place log record
            TaskManager.StartTask("SQL_DATABASE", "BACKUP", item.Name, itemId);

            try
            {
                DatabaseServer sql = GetDatabaseServer(item.ServiceId);
                string backFile = sql.BackupDatabase(item.Name, backupName, zipBackup);

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
                        buffer = sql.GetTempFileBinaryChunk(backFile, offset, FILE_BUFFER_LENGTH);

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

        public static byte[] GetSqlBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            // load original meta item
            SqlDatabase item = (SqlDatabase)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            DatabaseServer sql = GetDatabaseServer(item.ServiceId);

            return sql.GetTempFileBinaryChunk(path, offset, length);
        }

        public static string AppendSqlBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            // load original meta item
            SqlDatabase item = (SqlDatabase)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            DatabaseServer sql = GetDatabaseServer(item.ServiceId);
            return sql.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public static int RestoreSqlDatabase(int itemId, string[] uploadedFiles, string[] packageFiles)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SqlDatabase item = (SqlDatabase)PackageController.GetPackageItem(itemId);
            if (item == null)
                return BusinessErrorCodes.ERROR_MSSQL_DATABASES_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SQL_DATABASE", "RESTORE", item.Name, itemId);

            try
            {

                DatabaseServer sql = GetDatabaseServer(item.ServiceId);

                List<string> backupFiles = new List<string>();
                if (packageFiles != null && packageFiles.Length > 0)
                {
                    // copy package files to the remote SQL Server
                    foreach (string packageFile in packageFiles)
                    {
                        string path = null;
                        byte[] buffer = null;

                        int offset = 0;
                        do
                        {
                            // read package file
                            buffer = FilesController.GetFileBinaryChunk(item.PackageId, packageFile, offset, FILE_BUFFER_LENGTH);

                            // write remote backup file
                            string tempPath = sql.AppendTempFileBinaryChunk(Path.GetFileName(packageFile), path, buffer);
                            if (path == null)
                            {
                                path = tempPath;
                                backupFiles.Add(path);
                            }

                            offset += FILE_BUFFER_LENGTH;
                        }
                        while (buffer.Length == FILE_BUFFER_LENGTH);
                    }
                }
                else if (uploadedFiles != null && uploadedFiles.Length > 0)
                {
                    // upladed files
                    backupFiles.AddRange(uploadedFiles);
                }

                // restore
                if (backupFiles.Count > 0)
                    sql.RestoreDatabase(item.Name, backupFiles.ToArray());

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

        public static int TruncateSqlDatabase(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SqlDatabase origItem = (SqlDatabase)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_MSSQL_DATABASES_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("SQL_DATABASE", "TRUNCATE", origItem.Name, itemId);

            try
            {
                // get service
                DatabaseServer sql = GetDatabaseServer(origItem.ServiceId);

                // truncate database
                sql.TruncateDatabase(origItem.Name);

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

        private static string[] GetSqlDatabasesArray(int packageId, string groupName)
        {
            List<SqlDatabase> databases = GetSqlDatabases(packageId, groupName, false);
            string[] arr = new string[databases.Count];
            for (int i = 0; i < databases.Count; i++)
                arr[i] = databases[i].Name;

            return arr;
        }

		public static DatabaseBrowserConfiguration GetDatabaseBrowserConfiguration(int packageId, string groupName)
		{
            DatabaseBrowserConfiguration config = new DatabaseBrowserConfiguration();

			int serviceId = PackageController.GetPackageServiceId(packageId, groupName);
            if (serviceId == 0)
                return config;

			StringDictionary settings = ServerController.GetServiceSettings(serviceId);
			config.Enabled = !String.IsNullOrEmpty(settings["BrowseURL"]);

			return config;
		}

		public static DatabaseBrowserConfiguration GetDatabaseBrowserLogonScript(int packageId,
			string groupName, string username)
		{
			int serviceId = PackageController.GetPackageServiceId(packageId, groupName);
			if (serviceId == 0)
				return null;

			StringDictionary settings = ServerController.GetServiceSettings(serviceId);
			string url = settings["BrowseURL"];
			string method = settings["BrowseMethod"];
			string prms = settings["BrowseParameters"];

			DatabaseBrowserConfiguration config = new DatabaseBrowserConfiguration();
			config.Enabled = !String.IsNullOrEmpty(url);
			config.Method = method;

			prms = Utils.ReplaceStringVariable(prms, "server", settings["InternalAddress"]);

			// load database user
			SqlUser user = (SqlUser)PackageController.GetPackageItemByName(packageId, groupName, username, typeof(SqlUser));
			if (user != null)
			{
				prms = Utils.ReplaceStringVariable(prms, "user", username);
				prms = Utils.ReplaceStringVariable(prms, "password", CryptoUtils.Decrypt(user.Password));

				string[] lines = Utils.ParseDelimitedString(prms, '\n', '\r');

				StringBuilder sb = new StringBuilder();
				if (String.Compare(method, "get", true) == 0)
				{
					// GET
					sb.Append(url).Append("?");
					foreach (string line in lines)
						sb.Append(line).Append("&");

					config.GetData = sb.ToString();
				}
				else
				{
					// POST
					sb.Append("<html><body>");
					sb.Append("<form id=\"AspForm\" method=\"POST\" action=\"").Append(url).Append("\">");

					foreach (string line in lines)
					{
						//
						var indexOfSplit = line.IndexOf('=');
						//
						if (indexOfSplit == -1)
							continue;
						//
						sb.Append("<input type=\"hidden\" name=\"").Append(line.Substring(0, indexOfSplit))
							.Append("\" value=\"").Append(line.Substring(indexOfSplit + 1)).Append("\"></input>");
					}

					sb.Append("</form><script language=\"javascript\">document.getElementById(\"AspForm\").submit();</script>");
					sb.Append("</body></html>");

					config.PostData = sb.ToString();
				}
			}

			return config;
		}

        #endregion

        #region Users
        public static DataSet GetRawSqlUsersPaged(int packageId,
            string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, groupName, typeof(SqlUser),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<SqlUser> GetSqlUsers(int packageId, string groupName, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, groupName, typeof(SqlUser), recursive);

            return items.ConvertAll<SqlUser>(
                new Converter<ServiceProviderItem, SqlUser>(ConvertItemToSqlUser));
        }

        private static SqlUser ConvertItemToSqlUser(ServiceProviderItem item)
        {
            return (SqlUser)item;
        }

        public static SqlUser GetSqlUser(int itemId)
        {
            // load meta item
            SqlUser item = (SqlUser)PackageController.GetPackageItem(itemId);

            // load service item
            DatabaseServer sql = GetDatabaseServer(item.ServiceId);
            SqlUser user = sql.GetUser(item.Name, GetSqlDatabasesArray(item.PackageId, item.GroupName));

            // add common properties
            user.Id = item.Id;
            user.PackageId = item.PackageId;
            user.ServiceId = item.ServiceId;
            user.Password = CryptoUtils.Decrypt(item.Password);
            user.GroupName = item.GroupName;

            return user;
        }

        public static int AddSqlUser(SqlUser item, string groupName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, groupName + ".Users");
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_MSSQL_USERS_RESOURCE_QUOTA_LIMIT;

            // check if mail resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, groupName);
            if (serviceId == 0)
                return BusinessErrorCodes.ERROR_MSSQL_RESOURCE_UNAVAILABLE;

            // check package items
            if (PackageController.GetPackageItemByName(item.PackageId, groupName, item.Name, typeof(SqlUser)) != null)
                return BusinessErrorCodes.ERROR_MSSQL_USERS_PACKAGE_ITEM_EXISTS;

            // place log record
            TaskManager.StartTask("SQL_USER", "ADD", item.Name);
            TaskManager.WriteParameter("Provider", groupName);

            int itemId = default(int); 
            try
            {
                // check service items
                DatabaseServer sql = GetDatabaseServer(serviceId);
                if (sql.UserExists(item.Name))
                    return BusinessErrorCodes.ERROR_MSSQL_USERS_SERVICE_ITEM_EXISTS;

                // add service item
                sql.CreateUser(item, item.Password);

                // save item
                item.Password = CryptoUtils.Encrypt(item.Password);
                item.ServiceId = serviceId;
                itemId = PackageController.AddPackageItem(item);

                TaskManager.ItemId = itemId;                
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
				//
                if (ex.Message.Contains("INVALID_USERNAME"))
                    return BusinessErrorCodes.ERROR_MYSQL_INVALID_USER_NAME;
				// Return a generic error instead of default(int)
				itemId = BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
            return itemId;
        }

        public static int UpdateSqlUser(SqlUser item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SqlUser origItem = (SqlUser)PackageController.GetPackageItem(item.Id);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_MSSQL_USERS_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("SQL_USER", "UPDATE", origItem.Name, item.Id);

            try
            {
                // get service
                DatabaseServer sql = GetDatabaseServer(origItem.ServiceId);

                // update service item
                sql.UpdateUser(item, GetSqlDatabasesArray(origItem.PackageId, origItem.GroupName));

                // update meta item
                if (item.Password == "")
                    item.Password = CryptoUtils.Decrypt(origItem.Password);

                item.Password = CryptoUtils.Encrypt(item.Password);
                PackageController.UpdatePackageItem(item);
                return 0;
                
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);

				// Return a generic error instead of re-throwing an exception
				return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
         }

        public static int DeleteSqlUser(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SqlUser origItem = (SqlUser)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_MSSQL_USERS_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("SQL_USER", "DELETE", origItem.Name, itemId);

            try
            {
                // get service
                DatabaseServer sql = GetDatabaseServer(origItem.ServiceId);

                // delete service item
                sql.DeleteUser(origItem.Name, GetSqlDatabasesArray(origItem.PackageId, origItem.GroupName));

                // delete meta item
                PackageController.DeletePackageItem(origItem.Id);

                return 0;
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
				// Return a generic error instead of re-throwing an exception
				return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region IImportController Members

        public List<string> GetImportableItems(int packageId, int itemTypeId,
            Type itemType, ResourceGroupInfo group)
        {
            List<string> items = new List<string>();

            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return items;

            DatabaseServer db = GetDatabaseServer(serviceId);
            if (itemType == typeof(SqlDatabase))
                items.AddRange(db.GetDatabases());
            else if (itemType == typeof(SqlUser))
                items.AddRange(db.GetUsers());

            return items;
        }

        public void ImportItem(int packageId, int itemTypeId,
            Type itemType, ResourceGroupInfo group, string itemName)
        {
            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return;

            if (itemType == typeof(SqlDatabase))
            {
                // import database
                SqlDatabase db = new SqlDatabase();
                db.ServiceId = serviceId;
                db.PackageId = packageId;
                db.Name = itemName;
                db.GroupName = group.GroupName;
                PackageController.AddPackageItem(db);
            }
            else if (itemType == typeof(SqlUser))
            {
                // import user
                SqlUser user = new SqlUser();
                user.ServiceId = serviceId;
                user.PackageId = packageId;
                user.Name = itemName;
                user.GroupName = group.GroupName;
                user.Password = "";
                PackageController.AddPackageItem(user);
            }
        }

        #endregion

		#region IBackupController Members

		public int BackupItem(string tempFolder, XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
		{
			if (item is SqlDatabase)
			{
				// backup database
				DatabaseServer sql = GetDatabaseServer(item.ServiceId);

				string backupName = String.Format("DatabaseBackup_{0}.zip", item.Id);
				string remoteBackupFile = sql.BackupDatabase(item.Name, backupName, true);

				// download remote backup
				string localBackupPath = Path.Combine(tempFolder, backupName);

				byte[] buffer = null;
				FileStream stream = new FileStream(localBackupPath, FileMode.Create, FileAccess.Write);

				int offset = 0;
                long length = 0;
				do
				{
					// read remote content
					buffer = sql.GetTempFileBinaryChunk(remoteBackupFile, offset, FILE_BUFFER_LENGTH);

					// write remote content
					stream.Write(buffer, 0, buffer.Length);

                    length += buffer.Length;
					offset += FILE_BUFFER_LENGTH;
				}
				while (buffer.Length == FILE_BUFFER_LENGTH);
				stream.Close();

				// add file pointer
                
				BackupController.WriteFileElement(writer, "DatabaseBackup", backupName, length);

				// store meta item
				SqlDatabase database = sql.GetDatabase(item.Name);
				XmlSerializer serializer = new XmlSerializer(typeof(SqlDatabase));
				serializer.Serialize(writer, database);
			}
			else if (item is SqlUser)
			{
				// backup user
				DatabaseServer sql = GetDatabaseServer(item.ServiceId);

				SqlUser userItem = item as SqlUser;

				// store user info
				SqlUser user = sql.GetUser(item.Name, GetSqlDatabasesArray(item.PackageId, item.GroupName));
				user.Password = userItem.Password;

				XmlSerializer serializer = new XmlSerializer(typeof(SqlUser));
				serializer.Serialize(writer, user);
			}

			return 0;
		}

		public int RestoreItem(string tempFolder, XmlNode itemNode, int itemId, Type itemType, string itemName, int packageId, int serviceId, ResourceGroupInfo group)
		{
			if (itemType == typeof(SqlDatabase))
			{
				DatabaseServer sql = GetDatabaseServer(serviceId);

				// extract meta item
				XmlSerializer serializer = new XmlSerializer(typeof(SqlDatabase));
				SqlDatabase db = (SqlDatabase)serializer.Deserialize(
					new XmlNodeReader(itemNode.SelectSingleNode("SqlDatabase")));

				// create database if required
				if (!sql.DatabaseExists(itemName))
				{
					sql.CreateDatabase(db);
				}

				// copy database backup to remote server
				XmlNode fileNode = itemNode.SelectSingleNode("File[@name='DatabaseBackup']");
				string backupFileName = fileNode.Attributes["path"].Value;
                long backupFileLength = Int64.Parse(fileNode.Attributes["size"].Value);
				string localBackupFilePath = Path.Combine(tempFolder, backupFileName);

                if (new FileInfo(localBackupFilePath).Length != backupFileLength)
                    return -3;

				FileStream stream = new FileStream(localBackupFilePath, FileMode.Open, FileAccess.Read);
				byte[] buffer = new byte[FILE_BUFFER_LENGTH];

				int readBytes = 0;
                long length = 0;
				string remoteBackupPath = null;
				do
				{
					// read package file
					readBytes = stream.Read(buffer, 0, FILE_BUFFER_LENGTH);
                    length += readBytes;

					if (readBytes < FILE_BUFFER_LENGTH)
						// resize buffer
						Array.Resize<byte>(ref buffer, readBytes);

					// write remote backup file
					string tempPath = sql.AppendTempFileBinaryChunk(backupFileName, remoteBackupPath, buffer);
					if (remoteBackupPath == null)
						remoteBackupPath = tempPath;
				}
				while (readBytes == FILE_BUFFER_LENGTH);
				stream.Close();

				// restore database
				sql.RestoreDatabase(itemName, new string[] { remoteBackupPath });

				// add meta-item if required
				if (PackageController.GetPackageItemByName(packageId, group.GroupName,
					itemName, typeof(SqlDatabase)) == null)
				{
					db.PackageId = packageId;
					db.ServiceId = serviceId;
					db.GroupName = group.GroupName;
					PackageController.AddPackageItem(db);
				}
			}
			else if (itemType == typeof(SqlUser))
			{
				DatabaseServer sql = GetDatabaseServer(serviceId);

				// extract meta item
				XmlSerializer serializer = new XmlSerializer(typeof(SqlUser));
				SqlUser user = (SqlUser)serializer.Deserialize(
					new XmlNodeReader(itemNode.SelectSingleNode("SqlUser")));

				// create user if required
				if (!sql.UserExists(itemName))
				{
					sql.CreateUser(user, CryptoUtils.Decrypt(user.Password));
				}

				// add meta-item if required
				if (PackageController.GetPackageItemByName(packageId, group.GroupName,
					itemName, typeof(SqlUser)) == null)
				{
					user.PackageId = packageId;
					user.ServiceId = serviceId;
					user.GroupName = group.GroupName;
					PackageController.AddPackageItem(user);
				}
			}

			return 0;
		}

		#endregion
	}
}
