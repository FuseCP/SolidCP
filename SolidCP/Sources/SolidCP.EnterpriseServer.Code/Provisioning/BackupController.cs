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
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

using SolidCP.Providers;
using OS = SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;

namespace SolidCP.EnterpriseServer
{
    public class BackupController
    {
        public const string BACKUP_CATALOG_FILE_NAME = "BackupCatalog.xml";
        private const int FILE_BUFFER_LENGTH = 5000000; // ~5MB
        private const string RSA_SETTINGS_KEY = "RSA_KEY";

        public static int Backup(bool async, string taskId, int userId, int packageId, int serviceId, int serverId,
            string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder,
            bool deleteTempBackup)
        {
            // check demo account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check admin account
            accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin);
            if ((serviceId > 0 || serverId > 0) && accountCheck < 0) return accountCheck;

			if (accountCheck < 0)
				deleteTempBackup = true;

            // check if backup temp folder is available
			string tempFolder = GetTempBackupFolder();
			if (!FolderWriteAccessible(tempFolder))
				return BusinessErrorCodes.ERROR_BACKUP_TEMP_FOLDER_UNAVAILABLE;

            // check destination folder if required
            if (!String.IsNullOrEmpty(storePackageFolder) &&
                !RemoteServerFolderWriteAccessible(storePackageId, storePackageFolder))
                return BusinessErrorCodes.ERROR_BACKUP_DEST_FOLDER_UNAVAILABLE;

            // check server folder if required
            if (!String.IsNullOrEmpty(storeServerFolder) &&
                !FolderWriteAccessible(storeServerFolder))
                return BusinessErrorCodes.ERROR_BACKUP_SERVER_FOLDER_UNAVAILABLE;

			// check/reset delete flag
			accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin);
			if (accountCheck < 0 && !deleteTempBackup)
				deleteTempBackup = true;

			if (async)
			{
				BackupAsyncWorker worker = new BackupAsyncWorker();
				worker.threadUserId = SecurityContext.User.UserId;
				worker.taskId = taskId;
				worker.userId = userId;
				worker.packageId = packageId;
				worker.serviceId = serviceId;
				worker.serverId = serverId;
				worker.backupFileName = backupFileName;
				worker.storePackageId = storePackageId;
				worker.storePackageFolder = storePackageFolder;
				worker.storeServerFolder = storeServerFolder;
				worker.deleteTempBackup = deleteTempBackup;

				// run
				worker.BackupAsync();

				return 0;
			}
			else
			{
				return BackupInternal(taskId, userId, packageId, serviceId, serverId,
					backupFileName, storePackageId, storePackageFolder, storeServerFolder,
					deleteTempBackup);
			}
        }

		public static int BackupInternal(string taskId, int userId, int packageId, int serviceId, int serverId,
			string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder,
			bool deleteTempBackup)
		{
			try
			{
                TaskManager.StartTask(taskId, "BACKUP", "BACKUP", backupFileName, SecurityContext.User.UserId);

                // get the list of items to backup
				TaskManager.Write("Calculate items to backup");
				List<ServiceProviderItem> items = GetBackupItems(userId, packageId, serviceId, serverId);

				if (items.Count == 0)
					return 0;

				// group items by item types
				Dictionary<int, List<ServiceProviderItem>> groupedItems = new Dictionary<int, List<ServiceProviderItem>>();

				// sort by groups
				foreach (ServiceProviderItem item in items)
				{
					// add to group
					if (!groupedItems.ContainsKey(item.TypeId))
						groupedItems[item.TypeId] = new List<ServiceProviderItem>();

					groupedItems[item.TypeId].Add(item);
				}

				// temp backup folder
				string tempFolder = GetTempBackupFolder();

				// create backup catalog file
				StringWriter sw = new StringWriter();
				XmlTextWriter writer = new XmlTextWriter(sw);

				// write backup file header
				writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
				writer.WriteStartElement("Backup");
				writer.WriteStartElement("Info");
				writer.WriteElementString("Name", backupFileName);
				writer.WriteElementString("Created", DateTime.Now.ToString("r"));
				writer.WriteElementString("User", GetLoggedUsername());
				writer.WriteEndElement(); // Info

				// determine the number of items to backup
				int totalItems = 0;
				foreach (int itemTypeId in groupedItems.Keys)
				{
					// load item type
					ServiceProviderItemType itemType = PackageController.GetServiceItemType(itemTypeId);
					if (!itemType.Backupable)
						continue;

					totalItems += groupedItems[itemTypeId].Count;
				}

				TaskManager.IndicatorMaximum = totalItems + 2;
				TaskManager.IndicatorCurrent = 0;

				// backup grouped items
				writer.WriteStartElement("Items");
				foreach (int itemTypeId in groupedItems.Keys)
				{
					// load item type
					ServiceProviderItemType itemType = PackageController.GetServiceItemType(itemTypeId);
					if (!itemType.Backupable)
						continue;

					// load group
					ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);

					// instantiate controller
					IBackupController controller = null;
					try
					{
                        if (group.GroupController != null)
						    controller = Activator.CreateInstance(Type.GetType(group.GroupController)) as IBackupController;
						if (controller != null)
						{
							// backup items
							foreach (ServiceProviderItem item in groupedItems[itemTypeId])
							{
								TaskManager.Write(String.Format("Backup {0} of {1} - {2} '{3}'",
									TaskManager.IndicatorCurrent + 1,
									totalItems,
									itemType.DisplayName,
									item.Name));

								try
								{
									int backupResult = BackupItem(tempFolder, writer, item, group, controller);
								}
								catch (Exception ex)
								{
									TaskManager.WriteError(ex, "Can't backup item");
								}

								// increment progress
								TaskManager.IndicatorCurrent += 1;
    						}
						}
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex);
					}
				}
				writer.WriteEndElement(); // Items

				// close catalog writer
				writer.WriteEndElement(); // Backup
				writer.Close();

				// convert to Xml document
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(sw.ToString());

				// sign XML document
				//SignXmlDocument(doc);

				// save signed doc to file
				try
				{
					doc.Save(Path.Combine(tempFolder, BACKUP_CATALOG_FILE_NAME));
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Can't save backup catalog file: "
						+ Path.Combine(tempFolder, BACKUP_CATALOG_FILE_NAME));
					return 0;
				}

				TaskManager.Write("Packaging backup...");

				// compress backup files
				string[] zipFiles = Directory.GetFiles(tempFolder);
				string[] zipFileNames = new string[zipFiles.Length];
				for (int i = 0; i < zipFiles.Length; i++)
					zipFileNames[i] = Path.GetFileName(zipFiles[i]);

				string backupFileNamePath = Path.Combine(tempFolder, backupFileName);

				try
				{
					FileUtils.ZipFiles(backupFileNamePath, tempFolder, zipFileNames);

					// delete packed files
					foreach (string zipFile in zipFiles)
						File.Delete(zipFile);
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Can't zip backed up files");
					return 0;
				}

				TaskManager.IndicatorCurrent += 1;

				TaskManager.Write("Copying backup...");
				// move/copy backup file
				if (!String.IsNullOrEmpty(storeServerFolder))
				{
					// copy to local folder or UNC
					try
					{
						string destFile = Path.Combine(storeServerFolder, backupFileName);
						File.Copy(backupFileNamePath, destFile, true);
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex, "Can't copy backup to destination location");
						return 0;
					}
				}
				else if (storePackageId > 0)
				{
					try
					{
						// copy to space folder
						int osServiceId = PackageController.GetPackageServiceId(storePackageId, ResourceGroups.Os);
						if (osServiceId > 0)
						{
							OS.OperatingSystem os = new OS.OperatingSystem();
							ServiceProviderProxy.Init(os, osServiceId);

							string remoteBackupPath = FilesController.GetFullPackagePath(storePackageId,
								Path.Combine(storePackageFolder, backupFileName));

							FileStream stream = new FileStream(backupFileNamePath, FileMode.Open, FileAccess.Read);
							byte[] buffer = new byte[FILE_BUFFER_LENGTH];

							int readBytes = 0;
							do
							{
								// read package file
								readBytes = stream.Read(buffer, 0, FILE_BUFFER_LENGTH);

								if (readBytes < FILE_BUFFER_LENGTH)
									// resize buffer
									Array.Resize<byte>(ref buffer, readBytes);

								// write remote backup file
								os.AppendFileBinaryContent(remoteBackupPath, buffer);
							}
							while (readBytes == FILE_BUFFER_LENGTH);
							stream.Close();
						}
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex, "Can't copy backup to destination hosting space");
						return 0;
					}
				}

				TaskManager.IndicatorCurrent += 1;

				// delete backup file if required
				if (deleteTempBackup)
				{
					try
					{
						// delete backup folder and all its contents
						Directory.Delete(tempFolder, true);
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex, "Can't delete temporary backup folder");
						return 0;
					}
				}

                BackgroundTask topTask = TaskManager.TopTask;

                topTask.IndicatorCurrent = topTask.IndicatorMaximum;

                TaskController.UpdateTask(topTask);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}

			return 0;
		}

        public static List<ServiceProviderItem> GetBackupItems(int userId,
            int packageId, int serviceId, int serverId)
        {
			List<ServiceProviderItem> items = new List<ServiceProviderItem>();

			if (packageId > 0)
			{
				items.AddRange(PackageController.GetPackageItems(packageId));
			}
			else if (serviceId > 0)
			{
				items.AddRange(PackageController.GetServiceItems(serviceId));
			}
			else if (serverId > 0)
			{
				// get server services
				List<ServiceInfo> services = ServerController.GetServicesByServerId(serverId);
				foreach (ServiceInfo service in services)
					items.AddRange(PackageController.GetServiceItems(service.ServiceId));
			}
			else if (userId > 0)
			{
				List<PackageInfo> packages = new List<PackageInfo>();

				// get own spaces
				packages.AddRange(PackageController.GetMyPackages(userId));

				// get user spaces
				packages.AddRange(PackageController.GetPackages(userId));

				// build collection
				foreach (PackageInfo package in packages)
					items.AddRange(PackageController.GetPackageItems(package.PackageId));
			}
			return items;
		}

		public static KeyValueBunch GetBackupContentSummary(int userId, int packageId,
			int serviceId, int serverId)
		{
			Dictionary<string, List<string>> summary = new Dictionary<string, List<string>>();
			// Get backup items
			List<ServiceProviderItem> items = GetBackupItems(userId, packageId, serviceId, serverId);
			// Prepare filter for in-loop sort
			ServiceProviderItemType[] itemTypes = PackageController.GetServiceItemTypes().ToArray();
			// Group service items by type id
			foreach (ServiceProviderItem si in items)
			{
				ServiceProviderItemType itemType = Array.Find<ServiceProviderItemType>(itemTypes, 
					x => x.ItemTypeId == si.TypeId && x.Backupable);
				// Sort out item types without backup capabilities
				if (itemType != null)
				{
					// Mimic a grouping sort
					if (!summary.ContainsKey(itemType.DisplayName))
						summary.Add(itemType.DisplayName, new List<string>());
					//
					summary[itemType.DisplayName].Add(si.Name);
				}
			}
			//
			KeyValueBunch result = new KeyValueBunch();
			// Convert grouped data into serializable format
			foreach (string groupName in summary.Keys)
				result[groupName] = String.Join(",", summary[groupName].ToArray());
			//
			return result;
		}

        public static int Restore(bool async, string taskId, int userId, int packageId, int serviceId, int serverId,
            int storePackageId, string storePackageBackupPath, string storeServerBackupPath)
        {
            // check demo account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // check admin account
            accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin);
            if ((serviceId > 0 || serverId > 0) && accountCheck < 0) return accountCheck;

            // check if backup temp folder is available
			string tempFolder = GetTempBackupFolder();
			if (!FolderWriteAccessible(tempFolder))
				return BusinessErrorCodes.ERROR_BACKUP_TEMP_FOLDER_UNAVAILABLE;

			// check server source path if required
			if (!String.IsNullOrEmpty(storeServerBackupPath))
			{
				try
				{
					if (!File.Exists(storeServerBackupPath))
						return BusinessErrorCodes.ERROR_RESTORE_BACKUP_SOURCE_NOT_FOUND;
				}
				catch
				{
					return BusinessErrorCodes.ERROR_RESTORE_BACKUP_SOURCE_UNAVAILABLE;
				}
			}

			if (async)
			{
				BackupAsyncWorker worker = new BackupAsyncWorker();
				worker.threadUserId = SecurityContext.User.UserId;
				worker.taskId = taskId;
				worker.userId = userId;
				worker.packageId = packageId;
				worker.serviceId = serviceId;
				worker.serverId = serverId;
				worker.storePackageId = storePackageId;
				worker.storePackageBackupPath = storePackageBackupPath;
				worker.storeServerBackupPath = storeServerBackupPath;

				// run
				worker.RestoreAsync();

				return 0;
			}
			else
			{
				return RestoreInternal(taskId, userId, packageId, serviceId, serverId,
					storePackageId, storePackageBackupPath, storeServerBackupPath);
			}
        }

		public static int RestoreInternal(string taskId, int userId, int packageId, int serviceId, int serverId,
			int storePackageId, string storePackageBackupPath, string storeServerBackupPath)
		{
			try
			{
				// copy backup from remote or local server
				string backupFileName = (storePackageId > 0)
					? Path.GetFileName(storePackageBackupPath) : Path.GetFileName(storeServerBackupPath);

                TaskManager.StartTask(taskId, "BACKUP", "RESTORE", backupFileName, SecurityContext.User.UserId);

				// create temp folder
				string tempFolder = GetTempBackupFolder();

				string backupFileNamePath = Path.Combine(tempFolder, backupFileName);
				if (storePackageId > 0)
				{
					try
					{
						int osServiceId = PackageController.GetPackageServiceId(storePackageId, ResourceGroups.Os);
						if (osServiceId > 0)
						{
							OS.OperatingSystem os = new OS.OperatingSystem();
							ServiceProviderProxy.Init(os, osServiceId);

							string remoteBackupPath = FilesController.GetFullPackagePath(storePackageId,
								storePackageBackupPath);

							FileStream stream = new FileStream(backupFileNamePath, FileMode.Create, FileAccess.Write);

							byte[] buffer = new byte[FILE_BUFFER_LENGTH];
							int offset = 0;
							do
							{
								// read remote content
								buffer = os.GetFileBinaryChunk(remoteBackupPath, offset, FILE_BUFFER_LENGTH);

								// write remote content
								stream.Write(buffer, 0, buffer.Length);

								offset += FILE_BUFFER_LENGTH;
							}
							while (buffer.Length == FILE_BUFFER_LENGTH);
							stream.Close();

						}
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex, "Can't copy source backup set");
						return 0;
					}
				}
				else
				{
					backupFileNamePath = storeServerBackupPath;
				}

				try
				{
					// unpack archive
					FileUtils.UnzipFiles(backupFileNamePath, tempFolder);
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Can't unzip backup set");
					return 0;
				}

				// load backup catalog
				XmlDocument doc = new XmlDocument();

				try
				{
					doc.Load(Path.Combine(tempFolder, BACKUP_CATALOG_FILE_NAME));
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Can't find/open backup catalog file");
					return 0;
				}

				// validate XML document
				//if (!ValidateXmlDocument(doc))
				//{
				//    TaskManager.WriteError("Corrupted or altered backup catalog file has been read");
				//    return 0;
				//}

				// get the list of items to restore
				string condition = "";
				if (userId > 0)
				{
					// get user spaces
					List<PackageInfo> packages = new List<PackageInfo>();
					packages.AddRange(PackageController.GetMyPackages(userId));
					packages.AddRange(PackageController.GetPackages(userId));
					List<string> parts = new List<string>();
					foreach (PackageInfo package in packages)
						parts.Add("@packageId = " + package.PackageId.ToString());
					condition = "[" + String.Join(" or ", parts.ToArray()) + "]";
				}
				else if (packageId > 0)
				{
					condition = "[@packageId = " + packageId + "]";
				}
				else if (serviceId > 0)
				{
					condition = "[@serviceId = " + serviceId + "]";
				}
				else if (serverId > 0)
				{
					// get server services
					List<ServiceInfo> services = ServerController.GetServicesByServerId(serverId);
					List<string> parts = new List<string>();
					foreach (ServiceInfo service in services)
						parts.Add("@serviceId = " + service.ServiceId.ToString());
					condition = "[" + String.Join(" or ", parts.ToArray()) + "]";
				}

				XmlNodeList itemNodes = doc.SelectNodes("Backup/Items/Item" + condition);

				TaskManager.IndicatorMaximum = itemNodes.Count;
				TaskManager.IndicatorCurrent = 0;

				// group items by item types
				Dictionary<int, List<XmlNode>> groupedItems = new Dictionary<int, List<XmlNode>>();

				// sort by groups
				foreach (XmlNode itemNode in itemNodes)
				{
					int itemTypeId = Utils.ParseInt(itemNode.Attributes["itemTypeId"].Value, 0);
					// add to group
					if (!groupedItems.ContainsKey(itemTypeId))
						groupedItems[itemTypeId] = new List<XmlNode>();

					groupedItems[itemTypeId].Add(itemNode);
				}

				// restore grouped items
				foreach (int itemTypeId in groupedItems.Keys)
				{
					// load item type
					ServiceProviderItemType itemTypeInfo = PackageController.GetServiceItemType(itemTypeId);
					if (!itemTypeInfo.Backupable)
						continue;

					Type itemType = Type.GetType(itemTypeInfo.TypeName);

					// load group
					ResourceGroupInfo group = ServerController.GetResourceGroup(itemTypeInfo.GroupId);

					// instantiate controller
					IBackupController controller = null;
					try
					{
						controller = Activator.CreateInstance(Type.GetType(group.GroupController)) as IBackupController;
						if (controller != null)
						{
							// backup items
							foreach (XmlNode itemNode in groupedItems[itemTypeId])
							{
								int itemId = Utils.ParseInt(itemNode.Attributes["itemId"].Value, 0);
								string itemName = itemNode.Attributes["itemName"].Value;
								int itemPackageId = Utils.ParseInt(itemNode.Attributes["packageId"].Value, 0);
								int itemServiceId = Utils.ParseInt(itemNode.Attributes["serviceId"].Value, 0);

								TaskManager.Write(String.Format("Restore {0} '{1}'",
									itemTypeInfo.DisplayName, itemName));

								try
								{
									int restoreResult = controller.RestoreItem(tempFolder, itemNode,
										itemId, itemType, itemName, itemPackageId, itemServiceId, group);
								}
								catch (Exception ex)
								{
									TaskManager.WriteError(ex, "Can't restore item");
								}

								TaskManager.IndicatorCurrent++;
							}
						}
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex);
					}
				}

				// delete backup folder and all its contents
				try
				{
					Directory.Delete(tempFolder, true);
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Can't delete temporary backup folder");
					return 0;
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}

			return 0;
		}

        public static int BackupItem(string tempFolder, XmlWriter writer, ServiceProviderItem item)
        {
            // load item type
            ServiceProviderItemType itemType = PackageController.GetServiceItemType(item.TypeId);
            if (!itemType.Backupable)
                return -1;

            // load group
            ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);

            // create controller
            IBackupController controller = null;
            try
            {
                controller = Activator.CreateInstance(Type.GetType(group.GroupController)) as IBackupController;
                if (controller != null)
                {
                    return BackupItem(tempFolder, writer, item, group, controller);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }

            return -2;
        }

        private static int BackupItem(string tempFolder, XmlWriter writer,
            ServiceProviderItem item, ResourceGroupInfo group, IBackupController controller)
        {
            writer.WriteStartElement("Item");
            writer.WriteAttributeString("itemId", item.Id.ToString());
            writer.WriteAttributeString("itemTypeId", item.TypeId.ToString());
            writer.WriteAttributeString("itemName", item.Name);
            writer.WriteAttributeString("packageId", item.PackageId.ToString());
            writer.WriteAttributeString("serviceId", item.ServiceId.ToString());

            try
            {
                return controller.BackupItem(tempFolder, writer, item, group);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
            finally
            {
                writer.WriteEndElement(); // Item
            }

            return 0;
        }

        public static int RestoreItem()
        {

            return 0;
        }

		public static void WriteFileElement(XmlWriter writer, string fileName, string filePath, long size)
		{
			writer.WriteStartElement("File");
			writer.WriteAttributeString("name", fileName);
			writer.WriteAttributeString("path", filePath);
            writer.WriteAttributeString("size", size.ToString());
			writer.WriteEndElement();
		}

        #region Utility Methods
		public static bool FolderWriteAccessible(string path)
		{
			try
			{
				string tempFile = Path.Combine(path, "check");
				StreamWriter writer = File.CreateText(tempFile);
				writer.Close();
				File.Delete(tempFile);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool RemoteServerFolderWriteAccessible(int packageId, string path)
		{
			try
			{
				// copy to space folder
				int osServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Os);
				if (osServiceId > 0)
				{
					OS.OperatingSystem os = new OS.OperatingSystem();
					ServiceProviderProxy.Init(os, osServiceId);

					string remoteServerPathCheck = FilesController.GetFullPackagePath(packageId,
						Path.Combine(path, "check.txt"));

					//
					os.CreateFile(remoteServerPathCheck);
					os.AppendFileBinaryContent(remoteServerPathCheck, Encoding.UTF8.GetBytes(remoteServerPathCheck));
					os.DeleteFile(remoteServerPathCheck);
				}
				//
				return true;
			}
			catch
            {				//
				return false;
			}
		}

        public static void SignXmlDocument(XmlDocument doc)
        {
            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(doc);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = GetUserRSAKey();

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
        }

        public static bool ValidateXmlDocument(XmlDocument doc)
        {
            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(doc);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for
            // the entire XML document.  Throw an exception 
            // if more than one signature was found.
            if (nodeList.Count >= 2)
            {
                throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.  
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature(GetUserRSAKey());
        }

        public static RSA GetUserRSAKey()
        {
            int userId = SecurityContext.User.UserId;
            if(SecurityContext.User.IsPeer)
                userId = SecurityContext.User.OwnerId;

            UserSettings settings = UserController.GetUserSettings(userId, RSA_SETTINGS_KEY);
            string keyXml = settings[RSA_SETTINGS_KEY];
            RSA rsa = RSA.Create();
            if (String.IsNullOrEmpty(keyXml) || settings.UserId != userId)
            {
                // generate new key
                keyXml = rsa.ToXmlString(true);

                // store to settings
                settings[RSA_SETTINGS_KEY] = keyXml;
                settings.UserId = userId;
                UserController.UpdateUserSettings(settings);
            }
            else
            {
                rsa.FromXmlString(keyXml);
            }
            return rsa;
        }

        private static string GetLoggedUsername()
        {
            string username = SecurityContext.User.Identity.Name;
            UserInfo user = UserController.GetUser(SecurityContext.User.UserId);
            if (user != null)
                username = user.Username;
            return username;
        }

        public static string GetTempBackupFolder()
        {
            string timeStamp = DateTime.Now.Ticks.ToString();
            string tempFolder = Path.Combine(ConfigSettings.BackupsPath, GetLoggedUsername() + "_" + timeStamp);

            // create folder
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            return tempFolder;
        }


        #endregion
    }
}
