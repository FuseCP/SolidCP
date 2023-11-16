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
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using SolidCP.Providers.DomainLookup;
using SolidCP.Providers.DNS;
using SolidCP.Server.Code;
using SolidCP.Server.WPIService;
using System.Text.RegularExpressions;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;
using SolidCP.Server;
using System.Diagnostics;
using System.Collections;
using System.Security;
using System.Web;
using System.Management;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using Microsoft.Web.PlatformInstaller;

namespace SolidCP.Providers.OS
{
    public class Windows2003 : HostingServiceProviderBase, IWindowsOperatingSystem
	{
		#region Constants
		private const string ODBC_SOURCES_KEY = @"SOFTWARE\ODBC\ODBC.INI";
		private const string ODBC_NAMES_KEY = @"SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources";
		private const string ODBC_SOURCES_KEY_NAME = @"ODBC Data Sources";
		private const string ODBC_INST_KEY = @"SOFTWARE\ODBC\ODBCINST.INI\";

		private const string DSN_DESCRIPTION = @"SolidCP Data Source";

		private const string MSSQL_DRIVER = "SQL Server";
		private const string MSSQL_NATIVE_DRIVER = "SQL Native Client";
		private const string MYSQL_DRIVER = "MySQL ODBC";// 3.51 Driver";
		private const string MARIADB_DRIVER = "MariaDB ODBC";
		private const string MSACCESS_DRIVER = "Microsoft Access Driver (*.mdb)";
		private const string MSACCESS2010_DRIVER = "Microsoft Access Driver (*.mdb, *.accdb)";
		private const string MSEXCEL_DRIVER = "Microsoft Excel Driver (*.xls)";
		private const string MSEXCEL2010_DRIVER = "Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)";
		private const string TEXT_DRIVER = "Microsoft Text Driver (*.txt; *.csv)";

		#endregion

		#region Properties
		protected string UsersHome
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["UsersHome"]); }
		}
		#endregion

		#region Files
		public virtual string CreatePackageFolder(string initialPath)
		{
			return FileUtils.CreatePackageFolder(initialPath);
		}

		public virtual bool FileExists(string path)
		{
			return FileUtils.FileExists(path);
		}

		public virtual bool DirectoryExists(string path)
		{
			return FileUtils.DirectoryExists(path);
		}

		public virtual SystemFile GetFile(string path)
		{
			return FileUtils.GetFile(path);
		}

		public virtual SystemFile[] GetFiles(string path)
		{
			return FileUtils.GetFiles(path);
		}

		public virtual SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
		{
			return FileUtils.GetDirectoriesRecursive(rootFolder, path);
		}

		public virtual SystemFile[] GetFilesRecursive(string rootFolder, string path)
		{
			return FileUtils.GetFilesRecursive(rootFolder, path);
		}

		public virtual SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
		{
			return FileUtils.GetFilesRecursiveByPattern(rootFolder, path, pattern);
		}

		public virtual byte[] GetFileBinaryContent(string path)
		{
			return FileUtils.GetFileBinaryContent(path);
		}

		public virtual byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
		{
			return FileUtils.GetFileBinaryContent(path, encoding);
		}

		public virtual byte[] GetFileBinaryChunk(string path, int offset, int length)
		{
			return FileUtils.GetFileBinaryChunk(path, offset, length);
		}

		public virtual string GetFileTextContent(string path)
		{
			return FileUtils.GetFileTextContent(path);
		}

		public virtual void CreateFile(string path)
		{
			FileUtils.CreateFile(path);
		}

		public virtual void CreateDirectory(string path)
		{
			FileUtils.CreateDirectory(path);
		}

		public virtual void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime)
		{
			FileUtils.ChangeFileAttributes(path, createdTime, changedTime);
		}

		public virtual void DeleteFile(string path)
		{
			FileUtils.DeleteFile(path);
		}

		public virtual void DeleteFiles(string[] files)
		{
			FileUtils.DeleteFiles(files);
		}

		public virtual void DeleteEmptyDirectories(string[] directories)
		{
			FileUtils.DeleteEmptyDirectories(directories);
		}

		public virtual void UpdateFileBinaryContent(string path, byte[] content)
		{
			FileUtils.UpdateFileBinaryContent(path, content);
		}

		public virtual void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
		{
			FileUtils.UpdateFileBinaryContent(path, content, encoding);
		}

		public virtual void AppendFileBinaryContent(string path, byte[] chunk)
		{
			FileUtils.AppendFileBinaryContent(path, chunk);
		}

		public virtual void UpdateFileTextContent(string path, string content)
		{
			FileUtils.UpdateFileTextContent(path, content);
		}

		public virtual void MoveFile(string sourcePath, string destinationPath)
		{
			FileUtils.MoveFile(sourcePath, destinationPath);
		}

		public virtual void CopyFile(string sourcePath, string destinationPath)
		{
			FileUtils.CopyFile(sourcePath, destinationPath);
		}

		public virtual void ZipFiles(string zipFile, string rootPath, string[] files)
		{
			FileUtils.ZipFiles(zipFile, rootPath, files);
		}

		public virtual string[] UnzipFiles(string zipFile, string destFolder)
		{
			return FileUtils.UnzipFiles(zipFile, destFolder);
		}

		public virtual void CreateBackupZip(string zipFile, string rootPath)
		{
			FileUtils.CreateBackupZip(zipFile, rootPath);
		}

		public virtual void CreateAccessDatabase(string databasePath)
		{
			FileUtils.CreateAccessDatabase(databasePath);
		}

		public UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU)
		{
			return SecurityUtils.GetGroupNtfsPermissions(path, users,
				 ServerSettings, usersOU, null);
		}

		public void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions)
		{
			SecurityUtils.GrantGroupNtfsPermissions(path, users, resetChildPermissions,
				 ServerSettings, usersOU, null);
		}

		public virtual void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
		{
			FileUtils.SetQuotaLimitOnFolder(folderPath, shareNameDrive, quotaLimit, mode, wmiUserName, wmiPassword);
		}

		public virtual Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
		{
			throw new NotImplementedException();
		}

		public virtual Dictionary<string, Quota> GetQuotasForOrganization(string folderPath, string wmiUserName, string wmiPassword)
		{
			throw new NotImplementedException();
		}

		public virtual void DeleteDirectoryRecursive(string rootPath)
		{
			FileUtils.DeleteDirectoryRecursive(rootPath);
		}
		#endregion

		#region ODBC DSNs
		public virtual string[] GetInstalledOdbcDrivers()
		{
			List<string> drivers = new List<string>();
			if (IsDriverInstalled(MSSQL_DRIVER)) drivers.Add("MsSql");
			if (IsDriverInstalled(MSSQL_NATIVE_DRIVER)) drivers.Add("MsSqlNative");
			if (IsDriverInstalled(GetDriverName(MYSQL_DRIVER))) drivers.Add("MySql");
			if (IsDriverInstalled(GetDriverName(MARIADB_DRIVER))) drivers.Add("MariaDB");
			if (IsDriverInstalled(MSACCESS_DRIVER)) drivers.Add("MsAccess");
			if (IsDriverInstalled(MSACCESS2010_DRIVER)) drivers.Add("MsAccess2010");
			if (IsDriverInstalled(MSEXCEL_DRIVER)) drivers.Add("Excel");
			if (IsDriverInstalled(MSEXCEL2010_DRIVER)) drivers.Add("Excel2010");
			if (IsDriverInstalled(TEXT_DRIVER)) drivers.Add("Text");
			return drivers.ToArray();
		}

		public virtual string[] GetDSNNames()
		{
			// check DSN name
			RegistryKey keyNames = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY);
			if (keyNames == null)
				return new string[0];

			// open DSNs tree
			RegistryKey keyDsn = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY);
			if (keyDsn == null)
				return new string[0];

			return keyDsn.GetSubKeyNames();
		}

		public virtual SystemDSN GetDSN(string dsnName)
		{
			// check DSN name
			RegistryKey keyNames = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY);
			if (keyNames == null)
				return null;

			string driverName = (string)keyNames.GetValue(dsnName);
			if (driverName == null)
				return null;

			// open DSN tree
			RegistryKey keyDsn = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY + "\\" + dsnName);
			if (keyDsn == null)
				return null;

			SystemDSN dsn = new SystemDSN();
			dsn.Name = dsnName;
			if (driverName == MSSQL_DRIVER || driverName == MSSQL_NATIVE_DRIVER)
			{
				dsn.Driver = (driverName == MSSQL_DRIVER) ? "MsSql" : "MsSqlNative";
				dsn.DatabaseServer = (string)keyDsn.GetValue("Server");
				dsn.DatabaseName = (string)keyDsn.GetValue("Database");
				dsn.DatabaseUser = (string)keyDsn.GetValue("LastUser");
			}
			else if (driverName.ToLower().StartsWith(MYSQL_DRIVER.ToLower()))
			{
				dsn.Driver = "MySql";
				dsn.DatabaseServer = (string)keyDsn.GetValue("SERVER");
				dsn.DatabaseName = (string)keyDsn.GetValue("DATABASE");
				dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
				dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
			}
			else if (driverName.ToLower().StartsWith(MARIADB_DRIVER.ToLower()))
			{
				dsn.Driver = "MariaDB";
				dsn.DatabaseServer = (string)keyDsn.GetValue("SERVER");
				dsn.DatabaseName = (string)keyDsn.GetValue("DATABASE");
				dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
				dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
			}
			else if (driverName == MSACCESS_DRIVER)
			{
				dsn.Driver = "MsAccess";
				dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
				dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
				dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
			}
			else if (driverName == MSACCESS2010_DRIVER)
			{
				dsn.Driver = "MsAccess2010";
				dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
				dsn.DatabaseUser = (string)keyDsn.GetValue("UID");
				dsn.DatabasePassword = (string)keyDsn.GetValue("PWD");
			}
			else if (driverName == MSEXCEL_DRIVER)
			{
				dsn.Driver = "Excel";
				dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
			}
			else if (driverName == MSEXCEL2010_DRIVER)
			{
				dsn.Driver = "Excel2010";
				dsn.DatabaseName = (string)keyDsn.GetValue("DBQ");
			}
			else if (driverName == TEXT_DRIVER)
			{
				dsn.Driver = "Text";
				dsn.DatabaseName = (string)keyDsn.GetValue("DefaultDir");
			}

			return dsn;
		}

		public virtual void CreateDSN(SystemDSN dsn)
		{
			switch (dsn.Driver.ToLower())
			{
				case "mssql":
					CreateMsSqlDsn(dsn, MSSQL_DRIVER);
					break;
				case "mssqlnative":
					CreateMsSqlDsn(dsn, MSSQL_NATIVE_DRIVER);
					break;
				case "mysql":
					CreateMySqlDsn(dsn);
					break;
				case "mariadb":
					CreateMariaDBDsn(dsn);
					break;
				case "msaccess":
					CreateMsAccessDsn(dsn);
					break;
				case "msaccess2010":
					CreateMsAccess2010Dsn(dsn);
					break;
				case "excel":
					CreateExcelDsn(dsn);
					break;
				case "excel2010":
					CreateExcel2010Dsn(dsn);
					break;
				case "text":
					CreateTextDsn(dsn);
					break;
			}
		}

		public virtual void UpdateDSN(SystemDSN dsn)
		{
			// delete DSN
			DeleteDSN(dsn.Name);

			// create again
			CreateDSN(dsn);
		}

		public virtual void DeleteDSN(string dsnName)
		{
			// delete ODBC name
			RegistryKey list = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY, true);
			list.DeleteValue(dsnName);
			list.Close();

			// delete from ODBC tree
			RegistryKey root = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY, true);
			root.DeleteSubKeyTree(dsnName);
			root.Close();
		}
		#endregion

		#region Synchronizing
		public FolderGraph GetFolderGraph(string path)
		{
			if (!path.EndsWith("\\"))
				path += "\\";

			FolderGraph graph = new FolderGraph();
			graph.Hash = CalculateFileHash(path, path, graph.CheckSums);

			// copy hash to arrays
			graph.CheckSumKeys = new uint[graph.CheckSums.Count];
			graph.CheckSumValues = new FileHash[graph.CheckSums.Count];
			graph.CheckSums.Keys.CopyTo(graph.CheckSumKeys, 0);
			graph.CheckSums.Values.CopyTo(graph.CheckSumValues, 0);

			return graph;
		}

		public void ExecuteSyncActions(FileSyncAction[] actions)
		{
			// perform all operations but not delete ones
			foreach (FileSyncAction action in actions)
			{
				if (action.ActionType == SyncActionType.Create)
				{
					FileUtils.CreateDirectory(action.DestPath);
					continue;
				}
				else if (action.ActionType == SyncActionType.Copy)
				{
					FileUtils.CopyFile(action.SrcPath, action.DestPath);
				}
				else if (action.ActionType == SyncActionType.Move)
				{
					FileUtils.MoveFile(action.SrcPath, action.DestPath);
				}
			}

			// unzip file
			// ...after delete

			// delete files
			foreach (FileSyncAction action in actions)
			{
				if (action.ActionType == SyncActionType.Delete)
				{
					FileUtils.DeleteFile(action.DestPath);
				}
			}
		}

		private FileHash CalculateFileHash(string rootFolder, string path, Dictionary<uint, FileHash> checkSums)
		{
			CRC32 crc32 = new CRC32();

			// check if this is a folder
			if (Directory.Exists(path))
			{
				FileHash folder = new FileHash();
				folder.IsFolder = true;
				folder.Name = Path.GetFileName(path);
				folder.FullName = path.Substring(rootFolder.Length - 1);

				// process child folders and files
				List<string> childFiles = new List<string>();
				childFiles.AddRange(Directory.GetDirectories(path));
				childFiles.AddRange(Directory.GetFiles(path));

				foreach (string childFile in childFiles)
				{
					FileHash childHash = CalculateFileHash(rootFolder, childFile, checkSums);
					folder.Files.Add(childHash);

					// check sum
					folder.CheckSum += childHash.CheckSum;
					folder.CheckSum += ConvertCheckSumToInt(crc32.ComputeHash(Encoding.UTF8.GetBytes(childHash.Name)));

					//Debug.WriteLine(folder.CheckSum + " : " + folder.FullName);
				}

				// move list to array
				folder.FilesArray = folder.Files.ToArray();

				if (!checkSums.ContainsKey(folder.CheckSum))
					checkSums.Add(folder.CheckSum, folder);

				return folder;
			}

			FileHash file = new FileHash();
			file.Name = Path.GetFileName(path);
			file.FullName = path.Substring(rootFolder.Length - 1);

			// calculate CRC32
			using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				file.CheckSum = ConvertCheckSumToInt(
					crc32.ComputeHash(fs));
			}

			if (!checkSums.ContainsKey(file.CheckSum))
				checkSums.Add(file.CheckSum, file);

			//Debug.WriteLine(file.CheckSum + " : " + file.FullName);

			return file;
		}

		private uint ConvertCheckSumToInt(byte[] sumBytes)
		{
			uint checkSum = (uint)sumBytes[0] << 24;
			checkSum |= (uint)sumBytes[1] << 16;
			checkSum |= (uint)sumBytes[2] << 8;
			checkSum |= (uint)sumBytes[3] << 0;
			return checkSum;
		}
		#endregion

		#region HostingServiceProvider methods
		public override string[] Install()
		{
			List<string> messages = new List<string>();

			// create folder if it not exists
			try
			{
				if (!FileUtils.DirectoryExists(UsersHome))
				{
					FileUtils.CreateDirectory(UsersHome);
				}
			}
			catch (Exception ex)
			{
				messages.Add(String.Format("Folder '{0}' could not be created: {1}",
					 UsersHome, ex.Message));
			}
			return messages.ToArray();
		}

		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				try
				{
					if (item is HomeFolder)
						// delete home folder
						DeleteFile(item.Name);
					else if (item is SystemDSN)
						// delete DSN
						DeleteDSN(item.Name);
				}
				catch (Exception ex)
				{
					Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
				}
			}
		}

		public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
		{
			List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
			foreach (ServiceProviderItem item in items)
			{
				if (item is HomeFolder)
				{
					try
					{
						string path = item.Name;

						Log.WriteStart(String.Format("Calculating '{0}' folder size", path));

						// calculate disk space
						ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
						diskspace.ItemId = item.Id;
						diskspace.DiskSpace = FileUtils.CalculateFolderSize(path);
						itemsDiskspace.Add(diskspace);

						Log.WriteEnd(String.Format("Calculating '{0}' folder size", path));
					}
					catch (Exception ex)
					{
						Log.WriteError(ex);
					}
				}
			}
			return itemsDiskspace.ToArray();
		}
		#endregion

		#region Private Helpers
		private void CreateMsSqlDsn(SystemDSN dsn, string driverName)
		{
			// get driver path
			string driver = GetDriverPath(driverName);

			// add ODBC name
			RegisterDSN(dsn.Name, driverName);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("Server", dsn.DatabaseServer);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("Description", DSN_DESCRIPTION);
			keyDsn.SetValue("Database", dsn.DatabaseName);
			keyDsn.SetValue("LastUser", dsn.DatabaseUser);
			keyDsn.Close();
		}

		private void CreateMySqlDsn(SystemDSN dsn)
		{
			// get driver path
			string driver = GetDriverPath(MYSQL_DRIVER);

			// add ODBC name
			RegisterDSN(dsn.Name, MYSQL_DRIVER);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("SERVER", dsn.DatabaseServer);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("DESCRIPTION", DSN_DESCRIPTION);
			keyDsn.SetValue("DATABASE", dsn.DatabaseName);
			keyDsn.SetValue("UID", dsn.DatabaseUser);
			keyDsn.SetValue("PWD", dsn.DatabasePassword);
			keyDsn.Close();
		}

		private void CreateMariaDBDsn(SystemDSN dsn)
		{
			// get driver path
			string driver = GetDriverPath(MARIADB_DRIVER);

			// add ODBC name
			RegisterDSN(dsn.Name, MARIADB_DRIVER);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("SERVER", dsn.DatabaseServer);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("DESCRIPTION", DSN_DESCRIPTION);
			keyDsn.SetValue("DATABASE", dsn.DatabaseName);
			keyDsn.SetValue("UID", dsn.DatabaseUser);
			keyDsn.SetValue("PWD", dsn.DatabasePassword);
			keyDsn.Close();
		}

		private void CreateMsAccessDsn(SystemDSN dsn)
		{
			// get driver path
			string driver = GetDriverPath(MSACCESS_DRIVER);

			// add ODBC name
			RegisterDSN(dsn.Name, MSACCESS_DRIVER);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("Description", DSN_DESCRIPTION);
			keyDsn.SetValue("DBQ", dsn.DatabaseName);
			keyDsn.SetValue("DriverId", 25);
			keyDsn.SetValue("FIL", "MS Access;");
			keyDsn.SetValue("SafeTransactions", 0);
			keyDsn.SetValue("UID", dsn.DatabaseUser);
			keyDsn.SetValue("PWD", dsn.DatabasePassword);

			// add "Engines/Jet" subkey
			RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
			RegistryKey keyJet = keyEngines.CreateSubKey("Jet");
			keyJet.SetValue("ImplicitCommitSync", "");
			keyJet.SetValue("MaxBufferSize", 2048);
			keyJet.SetValue("PageTimeout", 5);
			keyJet.SetValue("Threads", 3);
			keyJet.SetValue("UserCommitSync", "Yes");

			keyJet.Close();
			keyEngines.Close();
			keyDsn.Close();
		}

		private void CreateMsAccess2010Dsn(SystemDSN dsn)
		{
			// get driver path
			string driver = GetDriverPath(MSACCESS2010_DRIVER);

			// add ODBC name
			RegisterDSN(dsn.Name, MSACCESS2010_DRIVER);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("Description", DSN_DESCRIPTION);
			keyDsn.SetValue("DBQ", dsn.DatabaseName);
			keyDsn.SetValue("DriverId", 25);
			keyDsn.SetValue("FIL", "MS Access;");
			keyDsn.SetValue("SafeTransactions", 0);
			keyDsn.SetValue("UID", dsn.DatabaseUser);
			keyDsn.SetValue("PWD", dsn.DatabasePassword);

			// add "Engines/Jet" subkey
			RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
			RegistryKey keyJet = keyEngines.CreateSubKey("Jet");
			keyJet.SetValue("ImplicitCommitSync", "");
			keyJet.SetValue("MaxBufferSize", 2048);
			keyJet.SetValue("PageTimeout", 5);
			keyJet.SetValue("Threads", 3);
			keyJet.SetValue("UserCommitSync", "Yes");

			keyJet.Close();
			keyEngines.Close();
			keyDsn.Close();
		}

		private void CreateExcelDsn(SystemDSN dsn)
		{
			// get driver path
			string driver = GetDriverPath(MSEXCEL_DRIVER);

			// add ODBC name
			RegisterDSN(dsn.Name, MSEXCEL_DRIVER);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("Description", DSN_DESCRIPTION);
			keyDsn.SetValue("DBQ", dsn.DatabaseName);
			keyDsn.SetValue("DefaultDir", Path.GetDirectoryName(dsn.DatabaseName));
			keyDsn.SetValue("DriverId", 790);
			keyDsn.SetValue("FIL", "excel 8.0;");
			keyDsn.SetValue("SafeTransactions", 0);
			keyDsn.SetValue("UID", "");
			keyDsn.SetValue("ReadOnly", new byte[] { 1 });

			// add "Engines/Excel" subkey
			RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
			RegistryKey keyExcel = keyEngines.CreateSubKey("Excel");
			keyExcel.SetValue("ImplicitCommitSync", "");
			keyExcel.SetValue("MaxScanRows", 8);
			keyExcel.SetValue("FirstRowHasNames", new byte[] { 1 });
			keyExcel.SetValue("Threads", 3);
			keyExcel.SetValue("UserCommitSync", "Yes");

			keyExcel.Close();
			keyEngines.Close();
			keyDsn.Close();
		}

		private void CreateExcel2010Dsn(SystemDSN dsn)
		{
			// get driver path
			string driver = GetDriverPath(MSEXCEL2010_DRIVER);

			// add ODBC name
			RegisterDSN(dsn.Name, MSEXCEL2010_DRIVER);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("Description", DSN_DESCRIPTION);
			keyDsn.SetValue("DBQ", dsn.DatabaseName);
			keyDsn.SetValue("DefaultDir", Path.GetDirectoryName(dsn.DatabaseName));
			keyDsn.SetValue("DriverId", 790);
			keyDsn.SetValue("FIL", "excel 8.0;");
			keyDsn.SetValue("SafeTransactions", 0);
			keyDsn.SetValue("UID", "");
			keyDsn.SetValue("ReadOnly", new byte[] { 1 });

			// add "Engines/Excel" subkey
			RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
			RegistryKey keyExcel = keyEngines.CreateSubKey("Excel");
			keyExcel.SetValue("ImplicitCommitSync", "");
			keyExcel.SetValue("MaxScanRows", 8);
			keyExcel.SetValue("FirstRowHasNames", new byte[] { 1 });
			keyExcel.SetValue("Threads", 3);
			keyExcel.SetValue("UserCommitSync", "Yes");

			keyExcel.Close();
			keyEngines.Close();
			keyDsn.Close();
		}

		private void CreateTextDsn(SystemDSN dsn)
		{
			// get driver path
			string driver = GetDriverPath(TEXT_DRIVER);

			// add ODBC name
			RegisterDSN(dsn.Name, TEXT_DRIVER);

			// add ODBC tree
			RegistryKey keyDsn = CreateDSNNode(dsn.Name);
			keyDsn.SetValue("Driver", driver);
			keyDsn.SetValue("Description", DSN_DESCRIPTION);
			keyDsn.SetValue("DefaultDir", dsn.DatabaseName);
			keyDsn.SetValue("DriverId", 27);
			keyDsn.SetValue("FIL", "text;");
			keyDsn.SetValue("SafeTransactions", 0);
			keyDsn.SetValue("UID", "");

			// add "Engines/Text" subkey
			RegistryKey keyEngines = keyDsn.CreateSubKey("Engines");
			RegistryKey keyText = keyEngines.CreateSubKey("Text");
			keyText.SetValue("ImplicitCommitSync", "");
			keyText.SetValue("Threads", 3);
			keyText.SetValue("UserCommitSync", "Yes");

			keyText.Close();
			keyEngines.Close();
			keyDsn.Close();
		}

		private void RegisterDSN(string dsnName, string driverName)
		{
			RegistryKey list = Registry.LocalMachine.OpenSubKey(ODBC_NAMES_KEY, true);

			if (list == null)
			{
				// create "SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources" node
				RegistryKey keyOdbc = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY, true);
				list = keyOdbc.CreateSubKey(ODBC_SOURCES_KEY_NAME);
			}

			list.SetValue(dsnName, GetDriverName(driverName));
			list.Close();
		}

		private RegistryKey CreateDSNNode(string dsnName)
		{
			RegistryKey root = Registry.LocalMachine.OpenSubKey(ODBC_SOURCES_KEY, true);
			return root.CreateSubKey(dsnName);
		}

		private string GetDriverName(string driverName)
		{
			// get driver path
			string[] keyDrivers = Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY).GetSubKeyNames();

			foreach (string keyDriver in keyDrivers)
			{
				if (keyDriver.ToLower().StartsWith(driverName.ToLower()))
					return keyDriver;
			}
			return null;
		}

		private string GetDriverPath(string driverName)
		{
			// get driver path
			string[] keyDrivers = Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY).GetSubKeyNames();

			foreach (string keyDriver in keyDrivers)
			{
				if (keyDriver.ToLower().StartsWith(driverName.ToLower()))
					return (string)Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY + keyDriver).GetValue("Driver");
			}

			throw new Exception(String.Format("'{0}' driver is not installed on the system", driverName));
		}

		private bool IsDriverInstalled(string driverName)
		{
			if (String.IsNullOrEmpty(driverName))
				return false;

			RegistryKey keyDriver = Registry.LocalMachine.OpenSubKey(ODBC_INST_KEY + driverName);
			return (keyDriver != null);
		}
		#endregion

		public override bool IsInstalled()
		{
			return OSInfo.WindowsVersion == WindowsVersion.WindowsServer2003;
		}

		public virtual bool CheckFileServicesInstallation()
		{
			return SolidCP.Providers.OS.WindowsOSInfo.CheckFileServicesInstallation();

		}

		public virtual bool InstallFsrmService()
		{
			return true;
		}

		public void RebootSystem()
		{
			try
			{
				WmiHelper wmi = new WmiHelper("root\\cimv2");
				ManagementObjectCollection objOses = wmi.ExecuteQuery("SELECT * FROM Win32_OperatingSystem");
				foreach (ManagementObject objOs in objOses)
				{
					objOs.Scope.Options.EnablePrivileges = true;
					objOs.InvokeMethod("Reboot", null);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region Web Platform Installer
		private string Linkify(string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			//" qweqwe http://www.helicontech.com/zoo/feed/  asdasdasd"
			Regex link = new Regex("(http[^\\s,]+)(?<![.,])");

			return link.Replace(value, "<a href=\"$1\" target=\"_blank\">$1</a>");
		}


		private WPIProduct ProductToWPIProduct(Product product)
		{
			WPIProduct p = new WPIProduct();
			p.ProductId = product.ProductId;
			p.Summary = product.Summary;
			p.LongDescription = Linkify(product.LongDescription);
			p.Published = product.Published;
			p.Author = product.Author;
			p.AuthorUri = (product.AuthorUri != null) ? product.AuthorUri.ToString() : "";
			p.Title = product.Title;
			p.Link = (product.Link != null) ? product.Link.ToString() : "";
			p.Version = product.Version;

			if (product.Installers.Count > 0)
			{
				if (product.Installers[0].EulaUrl != null)
				{
					p.EulaUrl = product.Installers[0].EulaUrl.ToString();

				}

				if (product.Installers[0].InstallerFile != null)
				{
					if (product.Installers[0].InstallerFile.InstallerUrl != null)
					{
						p.DownloadedLocation = product.Installers[0].InstallerFile.InstallerUrl.ToString();
					}
					p.FileSize = product.Installers[0].InstallerFile.FileSize;
				}

			}

			if (product.IconUrl != null)
			{
				p.Logo = product.IconUrl.ToString();
			}

			p.IsInstalled = product.IsInstalled(true);

			return p;
		}

		private void CheckHostingPackagesUpgrades(IList<WPIProduct> products)
		{
			foreach (WPIProduct product in products)
			{
				string hostingPackageName = product.ProductId;
				CheckProductForUpdate(hostingPackageName, product);
			}
		}

		private void CheckProductForUpdate(string hostingPackageName, WPIProduct product)
		{
			string installedHostingPackageVersion = GetInstalledHostingPackageVersion(hostingPackageName);
			if (!string.IsNullOrEmpty(installedHostingPackageVersion))
			{
				//6
				//3.0.90.383
				if (CompareVersions(product.Version, installedHostingPackageVersion) > 0)
				{
					product.IsUpgrade = true;
					product.IsInstalled = false;
				}

			}
		}

		private decimal CompareVersions(string newVersion, string oldVersion)
		{
			try
			{
				var version1 = new Version(newVersion);
				var version2 = new Version(oldVersion);

				return version1.CompareTo(version2);

			}
			catch (ArgumentException)
			{
				return String.Compare(newVersion, oldVersion, StringComparison.Ordinal);

			}
		}

		private string GetInstalledHostingPackageVersion(string hostingPackageName)
		{
			string installedVersion = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Helicon\\Zoo", hostingPackageName + "Version", string.Empty) as string;
			if (string.IsNullOrEmpty(installedVersion))
			{
				installedVersion = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Helicon\\Zoo", hostingPackageName + "Version", string.Empty) as string;
			}

			return installedVersion;
		}

		public virtual WPIProduct[] GetWPIProducts(string tabId, string keywordId)
		{


			try
			{
				List<WPIProduct> wpiProducts = new List<WPIProduct>();


				WpiHelper wpi = GetWpiFeed();

				string feedLocation = null;
				if (tabId != null)
				{
					Tab tab = wpi.GetTab(tabId);
					ICollection<string> feeds = tab.FeedList;
					feedLocation = feeds.GetEnumerator().Current;
				}

				List<Product> products = wpi.GetProductsToInstall(feedLocation, keywordId);

				if (products != null)
				{


					foreach (Product product in products)
					{
						if (null != product && !product.IsApplication)
						{
							wpiProducts.Add(ProductToWPIProduct(product));

						}
					}

					// check upgrades for Hosting Packages (ZooPackage keyword)
					if (WPIKeyword.HOSTING_PACKAGE_KEYWORD == keywordId)
					{
						CheckHostingPackagesUpgrades(wpiProducts);
					}


				}
				return wpiProducts.ToArray();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public virtual WPIProduct[] GetWPIProductsFiltered(string filter)
		{


			try
			{
				List<WPIProduct> wpiProducts = new List<WPIProduct>();

				WpiHelper wpi = GetWpiFeed();

				List<Product> products = wpi.GetProductsFiltered(filter);

				if (products != null)
				{


					foreach (Product product in products)
					{
						if (null != product && !product.IsApplication)
						{
							wpiProducts.Add(ProductToWPIProduct(product));

						}
					}

				}
				return wpiProducts.ToArray();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public virtual WPIProduct GetWPIProductById(string productdId)
		{
			try
			{
				WpiHelper wpi = GetWpiFeed();

				Product product = wpi.GetWPIProductById(productdId);
				WPIProduct wpiProduct = ProductToWPIProduct(product);

				if (wpiProduct.ProductId == "HeliconZooModule")
				{
					/*null string = HeliconZooModule in registry*/
					CheckProductForUpdate("", wpiProduct);
				}

				return wpiProduct;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public virtual WPITab[] GetWPITabs()
		{
			try
			{
				WpiHelper wpi = GetWpiFeed();

				List<WPITab> result = new List<WPITab>();

				foreach (Tab tab in wpi.GetTabs())
				{
					result.Add(new WPITab(tab.Id, tab.Name));
				}
				return result.ToArray();
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		static private string[] _feeds = new string[] { };

		public virtual void InitWPIFeeds(string feedUrls)
		{
			if (string.IsNullOrEmpty(feedUrls))
			{
				throw new Exception("Empty feed list");
			}

			string[] newFeeds = feedUrls.Split(';');

			if (newFeeds.Length == 0)
			{
				throw new Exception("Empty feed list");
			}
			if (!ArraysEqual<string>(newFeeds, _feeds))
			{
				//Feeds settings have been channged
				_feeds = newFeeds;
				wpi = null;

			}
		}


		static bool ArraysEqual<T>(T[] a1, T[] a2)
		{
			if (ReferenceEquals(a1, a2))
				return true;

			if (a1 == null || a2 == null)
				return false;

			if (a1.Length != a2.Length)
				return false;

			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			for (int i = 0; i < a1.Length; i++)
			{
				if (!comparer.Equals(a1[i], a2[i])) return false;
			}
			return true;
		}

		public virtual WPIKeyword[] GetWPIKeywords()
		{
			try
			{
				WpiHelper wpi = GetWpiFeed();

				List<WPIKeyword> result = new List<WPIKeyword>();

				result.Add(new WPIKeyword("", "All"));

				foreach (Keyword keyword in wpi.GetKeywords())
				{
					if (!wpi.IsKeywordApplication(keyword))
					{
						result.Add(new WPIKeyword(keyword.Id, keyword.Text));
					}

				}
				return result.ToArray();
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		public virtual WPIProduct[] GetWPIProductsWithDependencies(string[] products)
		{
			try
			{
				WpiHelper wpi = GetWpiFeed();

				List<WPIProduct> result = new List<WPIProduct>();
				foreach (Product product in wpi.GetProductsToInstallWithDependencies(products))
				{
					result.Add(ProductToWPIProduct(product));
				}
				return result.ToArray();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		static Process _WpiServiceExe = null;
		public virtual void InstallWPIProducts(string[] products)
		{
			try
			{
				StartWpiService();

				RegisterWpiService();

				WPIServiceContract client = new WPIServiceContract();

				client.Initialize(_feeds);
				client.BeginInstallation(products);
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		private void StartWpiService()
		{
			string binFolder = HttpContext.Current.Server.MapPath("/bin/");
			string workingDirectory = Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "Temp\\zoo.wpi");

			//string newUserProfile = Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "Temp\\zoo.wpi");
			//string newAppData = Path.Combine(newUserProfile, "Roaming");
			//string newLocalAppData = Path.Combine(newUserProfile, "Local");
			//try
			//{
			//    Directory.CreateDirectory(newUserProfile);
			//    Directory.CreateDirectory(newAppData);
			//    Directory.CreateDirectory(newLocalAppData);
			//}
			//catch (Exception)
			//{
			//    //throw;
			//}


			Process wpiServiceExe = new Process();
			wpiServiceExe.StartInfo = new ProcessStartInfo(Path.Combine(binFolder, "SolidCP.Server.WPIService.exe"));
			wpiServiceExe.StartInfo.WorkingDirectory = workingDirectory;
			wpiServiceExe.StartInfo.UseShellExecute = false;
			wpiServiceExe.StartInfo.LoadUserProfile = true;
			wpiServiceExe.StartInfo.EnvironmentVariables["MySqlPassword"] = "";
			//wpiServiceExe.StartInfo.EnvironmentVariables["UserProfile"] = newUserProfile;
			//wpiServiceExe.StartInfo.EnvironmentVariables["LocalAppData"] = newLocalAppData;
			//wpiServiceExe.StartInfo.EnvironmentVariables["AppData"] = newAppData;
			if (wpiServiceExe.Start())
			{
				_WpiServiceExe = wpiServiceExe;
			}
		}

		public virtual void CancelInstallWPIProducts()
		{
			try
			{
				Log.WriteStart("CancelInstallWPIProducts");

				KillWpiService();


				Log.WriteEnd("CancelInstallWPIProducts");
			}
			catch (Exception ex)
			{
				Log.WriteError("CancelInstallWPIProducts", ex);
				throw;
			}
		}

		private void KillWpiService()
		{
			//kill own service
			if (_WpiServiceExe != null && !_WpiServiceExe.HasExited)
			{
				_WpiServiceExe.Kill();
				_WpiServiceExe = null;
			}
			else
			{
				//find SolidCP.Server.WPIService.exe
				Process[] wpiservices = Process.GetProcessesByName("SolidCP.Server.WPIService");
				foreach (Process p in wpiservices)
				{
					p.Kill();
				}
			}
		}

		public virtual string GetWPIStatus()
		{
			try
			{
				RegisterWpiService();

				WPIServiceContract client = new WPIServiceContract();

				string status = client.GetStatus();

				return status; //OK
			}
			catch (Exception ex)
			{
				// done or error

				if (_WpiServiceExe == null || _WpiServiceExe.HasExited)
				{
					// reset WpiHelper for refresh status
					wpi = null;
					return ""; //OK
				}
				return ex.ToString();
			}
		}

		public virtual string WpiGetLogFileDirectory()
		{
			try
			{
				RegisterWpiService();

				WPIServiceContract client = new WPIServiceContract();

				string result = client.GetLogFileDirectory();

				return result; //OK
			}
			catch (Exception ex)
			{
				//throw;
				return string.Empty;
			}
		}

		public virtual SettingPair[] WpiGetLogsInDirectory(string Path)
		{
			try
			{
				ArrayList result = new ArrayList();

				string[] filePaths = Directory.GetFiles(Path);
				foreach (string filePath in filePaths)
				{
					using (StreamReader streamReader = new StreamReader(filePath))
					{
						string fileContent = SecurityElement.Escape(StringUtils.CleanupASCIIControlCharacters(streamReader.ReadToEnd()));
						result.Add(new SettingPair(filePath, fileContent));
					}

				}
				return (SettingPair[])result.ToArray(typeof(SettingPair)); //OK
			}
			catch (Exception ex)
			{
				//throw;
				return null;
			}
		}





		static WpiHelper wpi = null;
		WpiHelper GetWpiFeed()
		{
			if (_feeds.Length == 0)
			{
				throw new Exception("Empty feed list");
			}

			if (null == wpi)
			{
				wpi = new WpiHelper(_feeds);
			}
			return wpi;
		}

		private static object _lockRegisterWpiService = new object();
		private void RegisterWpiService()
		{
			lock (_lockRegisterWpiService)
			{


				try
				{
					ChannelServices.RegisterChannel(new TcpChannel(), true);
				}
				catch (System.Exception)
				{
					//ignor
				}

				if (null == RemotingConfiguration.IsWellKnownClientType(typeof(WPIServiceContract)))
				{
					RemotingConfiguration.RegisterWellKnownClientType(typeof(WPIServiceContract), string.Format("tcp://localhost:{0}/WPIServiceContract", WPIServiceContract.PORT));
				}

				try
				{
					WPIServiceContract client = new WPIServiceContract();
					client.Ping();
				}
				catch (Exception)
				{
					//unable to connect 
					//try to restart service
					KillWpiService();
					//StartWpiService();
				}
			}
		}
		#endregion Web Platform Installer


		#region Event Viewer
		public virtual List<string> GetLogNames()
		{
			List<string> logs = new List<string>();
			EventLog[] eventLogs = EventLog.GetEventLogs();
			foreach (EventLog eventLog in eventLogs)
			{
				logs.Add(eventLog.Log);
			}
			return logs;
		}

		public virtual List<SystemLogEntry> GetLogEntries(string logName)
		{
			SystemLogEntriesPaged result = new SystemLogEntriesPaged();
			List<SystemLogEntry> entries = new List<SystemLogEntry>();

			if (String.IsNullOrEmpty(logName))
				return entries;

			EventLog log = new EventLog(logName);
			EventLogEntryCollection logEntries = log.Entries;
			int count = logEntries.Count;

			// iterate in reverse order
			for (int i = count - 1; i >= 0; i--)
				entries.Add(CreateLogEntry(logEntries[i], false));

			return entries;
		}

		public SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
		{
			SystemLogEntriesPaged result = new SystemLogEntriesPaged();
			List<SystemLogEntry> entries = new List<SystemLogEntry>();

			if (String.IsNullOrEmpty(logName))
			{
				result.Count = 0;
				result.Entries = new SystemLogEntry[] { };
				return result;
			}

			EventLog log = new EventLog(logName);
			EventLogEntryCollection logEntries = log.Entries;
			int count = logEntries.Count;
			result.Count = count;

			// iterate in reverse order
			startRow = count - 1 - startRow;
			int endRow = startRow - maximumRows + 1;
			if (endRow < 0)
				endRow = 0;

			for (int i = startRow; i >= endRow; i--)
				entries.Add(CreateLogEntry(logEntries[i], true));

			result.Entries = entries.ToArray();

			return result;
		}

		public void ClearLog(string logName)
		{
			EventLog log = new EventLog(logName);
			log.Clear();
		}

		private SystemLogEntry CreateLogEntry(EventLogEntry logEntry, bool includeMessage)
		{
			SystemLogEntry entry = new SystemLogEntry();
			switch (logEntry.EntryType)
			{
				case EventLogEntryType.Error: entry.EntryType = SystemLogEntryType.Error; break;
				case EventLogEntryType.Warning: entry.EntryType = SystemLogEntryType.Warning; break;
				case EventLogEntryType.Information: entry.EntryType = SystemLogEntryType.Information; break;
				case EventLogEntryType.SuccessAudit: entry.EntryType = SystemLogEntryType.SuccessAudit; break;
				case EventLogEntryType.FailureAudit: entry.EntryType = SystemLogEntryType.FailureAudit; break;
			}

			entry.Created = logEntry.TimeGenerated;
			entry.Source = logEntry.Source;
			entry.Category = logEntry.Category;
			entry.EventID = logEntry.InstanceId;
			entry.UserName = logEntry.UserName;
			entry.MachineName = logEntry.MachineName;

			if (includeMessage)
				entry.Message = logEntry.Message;

			return entry;
		}
		#endregion

		#region Terminal connections
		public TerminalSession[] GetTerminalServicesSessions()
		{
			try
			{
				Log.WriteStart("GetTerminalServicesSessions");
				List<TerminalSession> sessions = new List<TerminalSession>();
				string ret = FileUtils.ExecuteSystemCommand("qwinsta", "");

				// parse returned string
				StringReader reader = new StringReader(ret);
				string line = null;
				int lineIndex = 0;
				while ((line = reader.ReadLine()) != null)
				{
					/*if (line.IndexOf("USERNAME") != -1 )
                        continue;*/
					//
					if (lineIndex == 0)
					{
						lineIndex++;
						continue;
					}

					Regex re = new Regex(@"(\S+)\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
					MatchCollection matches = re.Matches(line);

					// add row to the table
					string username = matches[1].Value.Trim();
					if (Regex.IsMatch(username, "^[0-9]*$"))
					{
						username = "";
					}

					if (username != "")
					{
						TerminalSession session = new TerminalSession();
						//
						session.SessionId = Int32.Parse(matches[2].Value.Trim());
						session.Username = username;
						session.Status = matches[3].Value.Trim();

						sessions.Add(session);
					}
					//
					lineIndex++;
				}
				reader.Close();

				Log.WriteEnd("GetTerminalServicesSessions");
				return sessions.ToArray();
			}
			catch (Exception ex)
			{
				Log.WriteError("GetTerminalServicesSessions", ex);
				throw;
			}
		}

		public void CloseTerminalServicesSession(int sessionId)
		{
			try
			{
				Log.WriteStart("CloseTerminalServicesSession");
				FileUtils.ExecuteSystemCommand("rwinsta", sessionId.ToString());
				Log.WriteEnd("CloseTerminalServicesSession");
			}
			catch (Exception ex)
			{
				Log.WriteError("CloseTerminalServicesSession", ex);
				throw;
			}
		}
		#endregion

		#region Windows Processes
		public OSProcess[] GetOSProcesses()
		{
			try
			{
				List<OSProcess> winProcesses = new List<OSProcess>();

				WmiHelper wmi = new WmiHelper("root\\cimv2");
				ManagementObjectCollection objProcesses = wmi.ExecuteQuery(
					"SELECT * FROM Win32_Process");

				foreach (ManagementObject objProcess in objProcesses)
				{
					int pid = Int32.Parse(objProcess["ProcessID"].ToString());
					string name = objProcess["Name"].ToString();

					// get user info
					string[] methodParams = new String[2];
					objProcess.InvokeMethod("GetOwner", (object[])methodParams);
					string username = methodParams[0];

					OSProcess winProcess = new OSProcess();
					winProcess.Pid = pid;
					winProcess.Name = name;
					winProcess.Username = username;
					winProcess.MemUsage = Int64.Parse(objProcess["WorkingSetSize"].ToString());

					winProcesses.Add(winProcess);
				}

				return winProcesses.ToArray();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void TerminateOSProcess(int pid)
		{
			try
			{
				Process[] processes = Process.GetProcesses();
				foreach (Process process in processes)
				{
					if (process.Id == pid)
						process.Kill();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion

		#region Windows Services
		public OSService[] GetOSServices()
		{
			try
			{
				List<OSService> winServices = new List<OSService>();

				System.ServiceProcess.ServiceController[] services = System.ServiceProcess.ServiceController.GetServices();
				foreach (var service in services)
				{
					OSService winService = new OSService();
					winService.Id = service.ServiceName;
					winService.Name = service.DisplayName;
					winService.CanStop = service.CanStop;
					winService.CanPauseAndContinue = service.CanPauseAndContinue;

					OSServiceStatus status = OSServiceStatus.ContinuePending;
					switch (service.Status)
					{
						case ServiceControllerStatus.ContinuePending: status = OSServiceStatus.ContinuePending; break;
						case ServiceControllerStatus.Paused: status = OSServiceStatus.Paused; break;
						case ServiceControllerStatus.PausePending: status = OSServiceStatus.PausePending; break;
						case ServiceControllerStatus.Running: status = OSServiceStatus.Running; break;
						case ServiceControllerStatus.StartPending: status = OSServiceStatus.StartPending; break;
						case ServiceControllerStatus.Stopped: status = OSServiceStatus.Stopped; break;
						case ServiceControllerStatus.StopPending: status = OSServiceStatus.StopPending; break;
					}
					winService.Status = status;

					winServices.Add(winService);
				}

				return winServices.ToArray();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public void ChangeOSServiceStatus(string id, OSServiceStatus status)
		{
			try
			{
				// get all services
				System.ServiceProcess.ServiceController[] services = System.ServiceProcess.ServiceController.GetServices();

				// find required service
				foreach (var service in services)
				{
					if (String.Compare(service.ServiceName, id, true) == 0)
					{
						if (status == OSServiceStatus.Paused
							&& service.Status == ServiceControllerStatus.Running)
							service.Pause();
						else if (status == OSServiceStatus.Running
							&& service.Status == ServiceControllerStatus.Stopped)
							service.Start();
						else if (status == OSServiceStatus.Stopped
							&& ((service.Status == ServiceControllerStatus.Running) ||
								(service.Status == ServiceControllerStatus.Paused)))
							service.Stop();
						else if (status == OSServiceStatus.ContinuePending
							&& service.Status == ServiceControllerStatus.Paused)
							service.Continue();
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion

		#region OS informations
		public Memory GetMemory()
		{
			try
			{
				Memory memory = new Memory();

				WmiHelper wmi = new WmiHelper("root\\cimv2");
				ManagementObjectCollection objOses = wmi.ExecuteQuery("SELECT * FROM Win32_OperatingSystem");
				foreach (ManagementObject objOs in objOses)
				{
					memory.FreePhysicalMemoryKB = UInt64.Parse(objOs["FreePhysicalMemory"].ToString());
					memory.TotalVisibleMemorySizeKB = UInt64.Parse(objOs["TotalVisibleMemorySize"].ToString());
					memory.TotalVirtualMemorySizeKB = UInt64.Parse(objOs["TotalVirtualMemorySize"].ToString());
					memory.FreeVirtualMemoryKB = UInt64.Parse(objOs["FreeVirtualMemory"].ToString());
				}
				return memory;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public virtual bool IsUnix() => false;
		#endregion

		#region System Commands
		public string ExecuteSystemCommand(string path, string args)
		{
			try
			{
				string result = FileUtils.ExecuteSystemCommand(path, args);
				return result;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion

		Shell cmd, powershell;
		Installer winget, chocolatey;

		public virtual Installer WinGet => winget != null ? winget : winget = new WinGet();
		public virtual Installer Chocolatey => chocolatey != null ? chocolatey : chocolatey = new Chocolatey();

		public Shell Cmd => cmd != null ? cmd : cmd = new Cmd();

		public Shell PowerShell => powershell != null ? powershell : powershell = new PowerShell();

		public Shell DefaultShell => Cmd;

		public Installer DefaultInstaller => WinGet;

		public void GetOSPlatform(out OSPlatform platform, out bool IsCore)
		{
			platform = OSInfo.OSPlatform;
			IsCore = OSInfo.IsCore;
		}

		protected Web.IWebServer webServer = null;
		public virtual Web.IWebServer WebServer =>
			webServer != null ? webServer :
			webServer = (Web.IWebServer)Activator.CreateInstance(Type.GetType("SolidCP.Providers.Web.IIs60, SolidCP.Providers.Web.IIs60"));
		public virtual ServiceController ServiceController => throw new NotImplementedException();

	}
}
