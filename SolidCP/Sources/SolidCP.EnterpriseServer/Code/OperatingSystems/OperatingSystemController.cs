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
using OS = SolidCP.Providers.OS;
using System.Collections;


namespace SolidCP.EnterpriseServer
{
    public class OperatingSystemController : IImportController, IBackupController
    {
        private const int FILE_BUFFER_LENGTH = 5000000; // ~5MB

        private static OS.OperatingSystem GetOS(int serviceId)
        {
            OS.OperatingSystem os = new OS.OperatingSystem();
            ServiceProviderProxy.Init(os, serviceId);
            return os;
        }

        #region ODBC DSNs
        public static DataSet GetRawOdbcSourcesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, ResourceGroups.Os, typeof(SystemDSN),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<SystemDSN> GetOdbcSources(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, ResourceGroups.Os, typeof(SystemDSN), recursive);

            return items.ConvertAll<SystemDSN>(
                new Converter<ServiceProviderItem, SystemDSN>(ConvertItemToSystemDSN));
        }

        private static SystemDSN ConvertItemToSystemDSN(ServiceProviderItem item)
        {
            return (SystemDSN)item;
        }

        public static string[] GetInstalledOdbcDrivers(int packageId)
        {
            // load service item
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Os);
            OS.OperatingSystem os = GetOS(serviceId);
            return os.GetInstalledOdbcDrivers();
        }

        public static SystemDSN GetOdbcSource(int itemId)
        {
            // load meta item
            SystemDSN item = (SystemDSN)PackageController.GetPackageItem(itemId);

            // load service item
            OS.OperatingSystem os = GetOS(item.ServiceId);
            SystemDSN dsn = os.GetDSN(item.Name);

            // add common properties
            dsn.Id = item.Id;
            dsn.PackageId = item.PackageId;
            dsn.ServiceId = item.ServiceId;

            if(dsn.Driver == "MsAccess" || dsn.Driver == "Excel" || dsn.Driver == "Text")
                dsn.DatabaseName = FilesController.GetVirtualPackagePath(item.PackageId, dsn.DatabaseName);

            return dsn;
        }

        public static int AddOdbcSource(SystemDSN item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.OS_ODBC);
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_OS_DSN_RESOURCE_QUOTA_LIMIT;

            // check if mail resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Os);
            if (serviceId == 0)
                return BusinessErrorCodes.ERROR_OS_RESOURCE_UNAVAILABLE;

            // check package items
            if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(SystemDSN)) != null)
                return BusinessErrorCodes.ERROR_OS_DSN_PACKAGE_ITEM_EXISTS;

            // place log record
            TaskManager.StartTask("ODBC_DSN", "ADD", item.Name);

            try
            {
                // check service items
                OS.OperatingSystem os = GetOS(serviceId);
                if (os.GetDSN(item.Name) != null)
                    return BusinessErrorCodes.ERROR_OS_DSN_SERVICE_ITEM_EXISTS;

                string[] dbNameParts = item.DatabaseName.Split('|');
                string groupName = null;
                if (dbNameParts.Length > 1)
                {
                    item.DatabaseName = dbNameParts[0];
                    groupName = dbNameParts[1];
                }

                // get database server address
                item.DatabaseServer = GetDatabaseServerName(groupName, item.PackageId);

                if (item.Driver == "MsAccess" || item.Driver == "Excel" || item.Driver == "Text")
                    item.DatabaseName = FilesController.GetFullPackagePath(item.PackageId, item.DatabaseName);

                // add service item
                os.CreateDSN(item);

                // save item
                item.DatabasePassword = CryptoUtils.Encrypt(item.DatabasePassword);
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

        public static int UpdateOdbcSource(SystemDSN item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SystemDSN origItem = (SystemDSN)PackageController.GetPackageItem(item.Id);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_OS_DSN_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("ODBC_DSN", "UPDATE", origItem.Name);
            TaskManager.ItemId = item.Id;

            try
            {
                // get service
                OS.OperatingSystem os = GetOS(origItem.ServiceId);

                // password
                item.Driver = origItem.Driver;
                item.Name = origItem.Name;

                if (item.DatabasePassword == "")
                    item.DatabasePassword = CryptoUtils.Decrypt(origItem.DatabasePassword);

                string[] dbNameParts = item.DatabaseName.Split('|');
                string groupName = null;
                if (dbNameParts.Length > 1)
                {
                    item.DatabaseName = dbNameParts[0];
                    groupName = dbNameParts[1];
                }

                // get database server address
                item.DatabaseServer = GetDatabaseServerName(groupName, item.PackageId);

                if (item.Driver == "MsAccess" || item.Driver == "Excel" || item.Driver == "Text")
                    item.DatabaseName = FilesController.GetFullPackagePath(origItem.PackageId, item.DatabaseName);

                // update service item
                os.UpdateDSN(item);

                // update meta item
                if (item.DatabasePassword != "")
                {
                    item.DatabasePassword = CryptoUtils.Encrypt(item.DatabasePassword);
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

        private static string GetDatabaseServerName(string groupName, int packageId)
        {
            int sqlServiceId = PackageController.GetPackageServiceId(packageId, groupName);
            if (sqlServiceId > 0)
            {
                StringDictionary sqlSettings = ServerController.GetServiceSettings(sqlServiceId);
                return sqlSettings["ExternalAddress"];
            }
            return "";
        }

        public static int DeleteOdbcSource(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            SystemDSN origItem = (SystemDSN)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_OS_DSN_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("ODBC_DSN", "DELETE", origItem.Name);
            TaskManager.ItemId = itemId;

            try
            {
                // get service
                OS.OperatingSystem os = GetOS(origItem.ServiceId);

                // delete service item
                os.DeleteDSN(origItem.Name);

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

        private static WindowsServer GetServerService(int serverId)
        {
            WindowsServer winServer = new WindowsServer();
            ServiceProviderProxy.ServerInit(winServer, serverId);
            return winServer;
        }

        #region Terminal Services Sessions
        public static TerminalSession[] GetTerminalServicesSessions(int serverId)
	    {
            return GetServerService(serverId).GetTerminalServicesSessions();
	    }

        public static int CloseTerminalServicesSession(int serverId, int sessionId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load server info
            ServerInfo server = ServerController.GetServerById(serverId);

            // place log record
            TaskManager.StartTask("SERVER", "RESET_TERMINAL_SESSION", sessionId);
            TaskManager.ItemId = serverId;

            try
            {
                GetServerService(serverId).CloseTerminalServicesSession(sessionId);
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

        #region Windows Processes
        public static WindowsProcess[] GetWindowsProcesses(int serverId)
        {
            return GetServerService(serverId).GetWindowsProcesses();
        }

        public static int TerminateWindowsProcess(int serverId, int pid)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load server info
            ServerInfo server = ServerController.GetServerById(serverId);

            // place log record
            TaskManager.StartTask("SERVER", "TERMINATE_SYSTEM_PROCESS", pid);
            TaskManager.ItemId = serverId;

            try
            {
                GetServerService(serverId).TerminateWindowsProcess(pid);
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

        #region Windows Services
        public static WindowsService[] GetWindowsServices(int serverId)
        {
            return GetServerService(serverId).GetWindowsServices();
        }

        public static int ChangeWindowsServiceStatus(int serverId, string id, WindowsServiceStatus status)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load server info
            ServerInfo server = ServerController.GetServerById(serverId);

            // place log record
            TaskManager.StartTask("SERVER", "CHANGE_WINDOWS_SERVICE_STATUS", id);
            TaskManager.ItemId = serverId;
            TaskManager.WriteParameter("New Status", status);

            try
            {
                GetServerService(serverId).ChangeWindowsServiceStatus(id, status);
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

        // Check If FSRM Role services were installed
        public static bool CheckFileServicesInstallation(int serviceId)
        {
            OS.OperatingSystem os = GetOS(serviceId);
            return os.CheckFileServicesInstallation();

        }

        #endregion

        #region Web Platform Installer

        public static bool CheckLoadUserProfile(int serverId)
        {
            int serviceId = getWebServiceId(serverId);
            if (serviceId != -1)
            {
                return WebServerController.GetWebServer(serviceId).CheckLoadUserProfile();
            }

            return false;
        }

        private static int getWebServiceId(int serverId)
        {
            DataSet dsServices = ServerController.GetRawServicesByServerId(serverId);

            int webGroup = -1;
            
            if (dsServices.Tables.Count < 1) return -1;

            foreach (DataRow r in dsServices.Tables[0].Rows)
            {
                if (r["GroupName"].ToString() == "Web")
                {
                    webGroup = (int)r["GroupID"];
                    break;
                }
            }

            if (webGroup == -1) return -1;

            foreach (DataRow r in dsServices.Tables[1].Rows)
            {
                if ((int)r["GroupID"] == webGroup)
                {
                    return (int)r["ServiceID"];
                }
            }

            return -1;
        }



        public static void EnableLoadUserProfile(int serverId)
        {
            int serviceId = getWebServiceId(serverId);
            if (serviceId != -1)
            {
                WebServerController.GetWebServer(serviceId).EnableLoadUserProfile();
            }
        }

        

        public static void InitWPIFeeds(int serverId, string feedUrls)
        {
            GetServerService(serverId).InitWPIFeeds(feedUrls);
        }

        public static WPITab[] GetWPITabs(int serverId)
        {
            return GetServerService(serverId).GetWPITabs();
        }

        public static WPIKeyword[] GetWPIKeywords(int serverId)
        {
            return GetServerService(serverId).GetWPIKeywords();
        }

        public static WPIProduct[] GetWPIProducts(int serverId, string tabId, string keywordId)
        {
            return GetServerService(serverId).GetWPIProducts(tabId, keywordId);
        }

        public static WPIProduct[] GetWPIProductsFiltered(int serverId, string keywordId)
        {
            return GetServerService(serverId).GetWPIProductsFiltered(keywordId);
        }

        public static WPIProduct GetWPIProductById(int serverId, string productdId)
        {
            return GetServerService(serverId).GetWPIProductById(productdId);
        }

        

        public static WPIProduct[] GetWPIProductsWithDependencies(int serverId, string[] products)
        {
            return GetServerService(serverId).GetWPIProductsWithDependencies(products);
        }
        public static void InstallWPIProducts(int serverId, string[] products)
        {
            GetServerService(serverId).InstallWPIProducts(products);
        }

        public static void CancelInstallWPIProducts(int serverId)
        {
            GetServerService(serverId).CancelInstallWPIProducts();
        }

        public static string GetWPIStatus(int serverId)
        {
            return GetServerService(serverId).GetWPIStatus();
        }

        public static string WpiGetLogFileDirectory(int serverId)
        {
            return GetServerService(serverId).WpiGetLogFileDirectory();
        }

        public static SettingPair[] WpiGetLogsInDirectory(int serverId, string Path)
        {
            return GetServerService(serverId).WpiGetLogsInDirectory(Path);
        }
        

        #endregion

        #region Event Viewer
        public static string[] GetLogNames(int serverId)
        {
            return GetServerService(serverId).GetLogNames();
        }

        public static SystemLogEntry[] GetLogEntries(int serverId, string logName)
        {
            return GetServerService(serverId).GetLogEntries(logName);
        }

        public static SystemLogEntriesPaged GetLogEntriesPaged(int serverId, string logName, int startRow, int maximumRows)
        {
            return GetServerService(serverId).GetLogEntriesPaged(logName, startRow, maximumRows);
        }

        public static int ClearLog(int serverId, string logName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("SERVER", "CLEAR_EVENT_LOG", logName);
            TaskManager.ItemId = serverId;

            try
            {
                GetServerService(serverId).ClearLog(logName);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return 0;
        }
        #endregion

        #region Server Reboot
        public static int RebootSystem(int serverId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load server info
            ServerInfo server = ServerController.GetServerById(serverId);

            // place log record
            TaskManager.StartTask("SERVER", "REBOOT");
            TaskManager.ItemId = serverId;

            try
            {
                GetServerService(serverId).RebootSystem();
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

        public List<string> GetImportableItems(int packageId, int itemTypeId, Type itemType,
			ResourceGroupInfo group)
        {
            List<string> items = new List<string>();

            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return items;

            OS.OperatingSystem os = GetOS(serviceId);
            if (itemType == typeof(SystemDSN))
                items.AddRange(os.GetDSNNames());

            return items;
        }

        public void ImportItem(int packageId, int itemTypeId, Type itemType,
			ResourceGroupInfo group, string itemName)
        {
            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return;

            if (itemType == typeof(SystemDSN))
            {
                // save DSN info
                SystemDSN dsn = new SystemDSN();
                dsn.Name = itemName;
                dsn.ServiceId = serviceId;
                dsn.PackageId = packageId;
                PackageController.AddPackageItem(dsn);
            }
        }

        #endregion

        #region IBackupController Members

        public int BackupItem(string tempFolder, System.Xml.XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
        {
            if (item is HomeFolder)
            {
                // backup home folder files
                string backupName = String.Format("SpaceFiles_{0}_{1}.zip",
                    item.Id, DateTime.Now.Ticks);
                
                // get the list of remote files
                List<SystemFile> files = FilesController.GetFiles(item.PackageId, "\\", true);

                string[] zipFiles = new string[files.Count];
                for(int i = 0; i < zipFiles.Length; i++)
                    zipFiles[i] = files[i].Name;

                // zip remote files
                FilesController.ZipFiles(item.PackageId, zipFiles, backupName);

                // download zipped file
                string localBackupPath = Path.Combine(tempFolder, backupName);

                byte[] buffer = null;
                FileStream stream = new FileStream(localBackupPath, FileMode.Create, FileAccess.Write);

                int offset = 0;
                long length = 0;
                do
                {
                    // read remote content
                    buffer = FilesController.GetFileBinaryChunk(item.PackageId, backupName, offset, FILE_BUFFER_LENGTH);

                    // write remote content
                    stream.Write(buffer, 0, buffer.Length);

                    length += buffer.Length;
                    offset += FILE_BUFFER_LENGTH;
                }
                while (buffer.Length == FILE_BUFFER_LENGTH);
                stream.Close();

				// delete zipped file
				if (FilesController.FileExists(item.PackageId, backupName))
					FilesController.DeleteFiles(item.PackageId, new string[] { backupName });

                // add file pointer
                BackupController.WriteFileElement(writer, "SpaceFiles", backupName, length);

                // store meta item
                XmlSerializer serializer = new XmlSerializer(typeof(HomeFolder));
                serializer.Serialize(writer, item);
            }
            else if (item is SystemDSN)
            {
                // backup ODBC DSN
                OS.OperatingSystem os = GetOS(item.ServiceId);

                // read DSN info
                SystemDSN itemDsn = item as SystemDSN;
                SystemDSN dsn = os.GetDSN(item.Name);
                dsn.DatabasePassword = itemDsn.DatabasePassword;

                XmlSerializer serializer = new XmlSerializer(typeof(SystemDSN));
                serializer.Serialize(writer, dsn);
            }

            return 0;
        }

        public int RestoreItem(string tempFolder, System.Xml.XmlNode itemNode, int itemId, Type itemType, string itemName, int packageId, int serviceId, ResourceGroupInfo group)
        {
            if (itemType == typeof(HomeFolder))
            {
                OS.OperatingSystem os = GetOS(serviceId);
                
                // extract meta item
                XmlSerializer serializer = new XmlSerializer(typeof(HomeFolder));
                HomeFolder homeFolder = (HomeFolder)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("HomeFolder")));

                // create home folder if required
                if (!os.DirectoryExists(homeFolder.Name))
                {
                    os.CreatePackageFolder(homeFolder.Name);
                }

                // copy database backup to remote server
                XmlNode fileNode = itemNode.SelectSingleNode("File[@name='SpaceFiles']");
                string backupFileName = fileNode.Attributes["path"].Value;
                long backupFileLength = Int64.Parse(fileNode.Attributes["size"].Value);
                string localBackupFilePath = Path.Combine(tempFolder, backupFileName);

                if (new FileInfo(localBackupFilePath).Length != backupFileLength)
                    return -3;

                FileStream stream = new FileStream(localBackupFilePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[FILE_BUFFER_LENGTH];

                int readBytes = 0;
                long length = 0;
                string remoteBackupPath = Path.Combine(homeFolder.Name, backupFileName);
                do
                {
                    // read package file
                    readBytes = stream.Read(buffer, 0, FILE_BUFFER_LENGTH);
                    length += readBytes;

                    if (readBytes < FILE_BUFFER_LENGTH)
                        // resize buffer
                        Array.Resize<byte>(ref buffer, readBytes);

                    // write remote backup file
                    os.AppendFileBinaryContent(remoteBackupPath, buffer);
                }
                while (readBytes == FILE_BUFFER_LENGTH);
                stream.Close();

                // unzip files
                os.UnzipFiles(remoteBackupPath, homeFolder.Name);

				// delete archive
				if (os.FileExists(remoteBackupPath))
					os.DeleteFile(remoteBackupPath);

                // add meta-item if required
                if (PackageController.GetPackageItemByName(packageId, itemName, typeof(HomeFolder)) == null)
                {
                    homeFolder.PackageId = packageId;
                    homeFolder.ServiceId = serviceId;
                    PackageController.AddPackageItem(homeFolder);
                }
            }
            else if (itemType == typeof(SystemDSN))
            {
                OS.OperatingSystem os = GetOS(serviceId);

                // extract meta item
                XmlSerializer serializer = new XmlSerializer(typeof(SystemDSN));
                SystemDSN dsn = (SystemDSN)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("SystemDSN")));

                // create DSN if required
                if (os.GetDSN(itemName) == null)
                {
                    dsn.DatabasePassword = CryptoUtils.Decrypt(dsn.DatabasePassword);
                    os.CreateDSN(dsn);

                    // restore password
                    dsn.DatabasePassword = CryptoUtils.Encrypt(dsn.DatabasePassword);
                }

                // add meta-item if required
                if (PackageController.GetPackageItemByName(packageId, itemName, typeof(SystemDSN)) == null)
                {
                    dsn.PackageId = packageId;
                    dsn.ServiceId = serviceId;
                    PackageController.AddPackageItem(dsn);
                }
            }

            return 0;
        }

      
       
        #endregion
    }
}
