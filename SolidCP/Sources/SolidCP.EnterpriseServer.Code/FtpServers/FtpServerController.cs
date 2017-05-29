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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using SolidCP.Providers;
using SolidCP.Providers.FTP;

namespace SolidCP.EnterpriseServer
{
    public class FtpServerController : IImportController, IBackupController
    {
        public static FTPServer GetFTPServer(int serviceId)
        {
            FTPServer ftp = new FTPServer();
            ServiceProviderProxy.Init(ftp, serviceId);
            return ftp;
        }

        public static FtpSite[] GetFtpSites(int serviceId)
        {
            FTPServer ftp = new FTPServer();
            ServiceProviderProxy.Init(ftp, serviceId);
            return ftp.GetSites();
        }

        public static DataSet GetRawFtpAccountsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, typeof(FtpAccount),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<FtpAccount> GetFtpAccounts(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(FtpAccount), recursive);

            return items.ConvertAll<FtpAccount>(
                new Converter<ServiceProviderItem, FtpAccount>(ConvertItemToFtpAccount));
        }

        private static FtpAccount ConvertItemToFtpAccount(ServiceProviderItem item)
        {
            FtpAccount account = (FtpAccount)item;

            string homeFolder = FilesController.GetHomeFolder(account.PackageId);
            if(!String.IsNullOrEmpty(account.Folder) && account.Folder.IndexOf(":") != -1)
                account.Folder = account.Folder.Substring(homeFolder.Length);
            account.Folder = (account.Folder == "") ? "\\" : account.Folder;

            // decode password
            account.Password = CryptoUtils.Decrypt(account.Password);

            return account;
        }

        public static bool FtpAccountExists(string name)
        {
            return PackageController.CheckServiceItemExists(name, typeof(FtpAccount));
        }

        public static bool FtpAccountExists(int serviceId, string name)
        {
            return PackageController.CheckServiceItemExists(serviceId, name, typeof(FtpAccount));
        }

        public static FtpAccount GetFtpAccount(int itemId)
        {
            // load meta item
            FtpAccount item = (FtpAccount)PackageController.GetPackageItem(itemId);
            
            // load service item
            FTPServer ftp = new FTPServer();
            ServiceProviderProxy.Init(ftp, item.ServiceId);
            FtpAccount account = ftp.GetAccount(item.Name);

            // truncate home folder
            account.Folder = FilesController.GetVirtualPackagePath(item.PackageId, account.Folder);

            account.Id = item.Id;
            account.PackageId = item.PackageId;
            account.ServiceId = item.ServiceId;
            account.Password = CryptoUtils.Decrypt(item.Password);

            return account;
        }

        public static int AddFtpAccount(FtpAccount item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.FTP_ACCOUNTS);
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_FTP_RESOURCE_QUOTA_LIMIT;

            // check if FTP account already exists
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Ftp);
            if(serviceId == 0)
                return BusinessErrorCodes.ERROR_FTP_RESOURCE_UNAVAILABLE;

            // place log record
            TaskManager.StartTask("FTP_ACCOUNT", "ADD", item.Name);
            TaskManager.WriteParameter("Folder", item.Folder);
            TaskManager.WriteParameter("CanRead", item.CanRead);
            TaskManager.WriteParameter("CanWrite", item.CanWrite);

            
            try
            {
                // check package items
                if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(FtpAccount)) != null)
                    return BusinessErrorCodes.ERROR_FTP_PACKAGE_ITEM_EXISTS;

                // check service items
                FTPServer ftp = new FTPServer();
                ServiceProviderProxy.Init(ftp, serviceId);
                if (ftp.AccountExists(item.Name))
                    return BusinessErrorCodes.ERROR_FTP_SERVICE_ITEM_EXISTS;

				// store original path
				string origFolder = item.Folder;

				// convert folder
				StringDictionary ftpSettings = ServerController.GetServiceSettings(serviceId);
				if (Utils.ParseBool(ftpSettings["BuildUncFilesPath"], false))
				{
					// UNC
					// get OS service
					int osServiceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Os);
					item.Folder = FilesController.GetFullUncPackagePath(item.PackageId, osServiceId, item.Folder);
				}
				else
				{
					// Absolute
					item.Folder = FilesController.GetFullPackagePath(item.PackageId, item.Folder);
				}

                item.Enabled = true;

                // add service item
                ftp.CreateAccount(item);

                // save item
                item.Password = CryptoUtils.Encrypt(item.Password);
                item.ServiceId = serviceId;
                item.Folder = (origFolder == "") ? "\\" : origFolder;
                int itemId = PackageController.AddPackageItem(item);
                TaskManager.ItemId = itemId;
                return itemId;
                
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                throw;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int UpdateFtpAccount(FtpAccount item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            FtpAccount origItem = (FtpAccount)PackageController.GetPackageItem(item.Id);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_FTP_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FTP_ACCOUNT", "UPDATE", origItem.Name, item.Id);

            TaskManager.WriteParameter("Folder", item.Folder);
            TaskManager.WriteParameter("CanRead", item.CanRead);
            TaskManager.WriteParameter("CanWrite", item.CanWrite);

            try
            {
                // get service
                FTPServer ftp = new FTPServer();
                ServiceProviderProxy.Init(ftp, origItem.ServiceId);

                // store original folder
                string origFolder = item.Folder;

				// convert folder
				StringDictionary ftpSettings = ServerController.GetServiceSettings(origItem.ServiceId);
				if (Utils.ParseBool(ftpSettings["BuildUncFilesPath"], false))
				{
					// UNC
					// get OS service
					int osServiceId = PackageController.GetPackageServiceId(origItem.PackageId, ResourceGroups.Os);
					item.Folder = FilesController.GetFullUncPackagePath(origItem.PackageId, osServiceId, item.Folder);
				}
				else
				{
					// Absolute
					item.Folder = FilesController.GetFullPackagePath(origItem.PackageId, item.Folder);
				}

                item.Enabled = true;

                // add service item
                ftp.UpdateAccount(item);

                // save item
                item.Password = String.IsNullOrEmpty(item.Password) ? origItem.Password : CryptoUtils.Encrypt(item.Password);
                item.Folder = (origFolder == "") ? "\\" : origFolder;
                PackageController.UpdatePackageItem(item);

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

        public static int DeleteFtpAccount(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            FtpAccount origItem = (FtpAccount)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_FTP_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("FTP_ACCOUNT", "DELETE", origItem.Name, itemId);

            try
            {
                // get service
                FTPServer ftp = new FTPServer();
                ServiceProviderProxy.Init(ftp, origItem.ServiceId);

                // delete service item
                ftp.DeleteAccount(origItem.Name);

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

        #region IImportController Members

        public List<string> GetImportableItems(int packageId, int itemTypeId, Type itemType, ResourceGroupInfo group)
        {
            List<string> items = new List<string>();

            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return items;

            // FTP provider
            FTPServer ftp = new FTPServer();
            ServiceProviderProxy.Init(ftp, serviceId);

            FtpAccount[] accounts = ftp.GetAccounts();

            foreach (FtpAccount account in accounts)
                items.Add(account.Name);

            return items;
        }

        public void ImportItem(int packageId, int itemTypeId,
            Type itemType, ResourceGroupInfo group, string itemName)
        {
            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return;

            // FTP provider
            FTPServer ftp = new FTPServer();
            ServiceProviderProxy.Init(ftp, serviceId);

            if (itemType == typeof(FtpAccount))
            {
                    // load FTP account
                    FtpAccount account = ftp.GetAccount(itemName);
                    account.Folder = FilesController.GetFullPackagePath(packageId, "\\"); // root space folder

                    // update FTP account
                    ftp.UpdateAccount(account);

                    // save account
                    account.ServiceId = serviceId;
                    account.PackageId = packageId;
                    PackageController.AddPackageItem(account);
            }
        }

        #endregion

        #region IBackupController Members

        public int BackupItem(string tempFolder, System.Xml.XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
        {
            if (item is FtpAccount)
            {
                // backup FTP account
                FTPServer ftp = GetFTPServer(item.ServiceId);

                // read FTP account info
                FtpAccount account = ftp.GetAccount(item.Name);
                account.Password = ((FtpAccount)item).Password;

                XmlSerializer serializer = new XmlSerializer(typeof(FtpAccount));
                serializer.Serialize(writer, account);
            }
            return 0;
        }

        public int RestoreItem(string tempFolder, System.Xml.XmlNode itemNode, int itemId, Type itemType, string itemName, int packageId, int serviceId, ResourceGroupInfo group)
        {
            if (itemType == typeof(FtpAccount))
            {
                FTPServer ftp = GetFTPServer(serviceId);

                // extract meta item
                XmlSerializer serializer = new XmlSerializer(typeof(FtpAccount));
                FtpAccount account = (FtpAccount)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("FtpAccount")));

                // create DSN if required
                if (!ftp.AccountExists(itemName))
                {
                    account.Password = CryptoUtils.Decrypt(account.Password);
                    ftp.CreateAccount(account);

                    // restore password
                    account.Password = CryptoUtils.Encrypt(account.Password);
                }

                // add meta-item if required
                if (PackageController.GetPackageItemByName(packageId, itemName, typeof(FtpAccount)) == null)
                {
                    account.PackageId = packageId;
                    account.ServiceId = serviceId;
                    PackageController.AddPackageItem(account);
                }
            }

            return 0;
        }

        #endregion
    }
}
