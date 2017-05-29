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
using System.Text.RegularExpressions;
using System.Linq;

namespace SolidCP.Providers.OS
{
    public class Windows2003 : HostingServiceProviderBase, IOperatingSystem
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
            return SolidCP.Server.Utils.OS.GetVersion() == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2003;                        
        }

        public virtual bool CheckFileServicesInstallation()
        {
            return SolidCP.Server.Utils.OS.CheckFileServicesInstallation();

        }

        public virtual bool InstallFsrmService()
        {
            return true;
        }
    }
}
