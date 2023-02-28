#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.DNS;
using SolidCP.Providers.DomainLookup;
using System.Collections.Generic;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
    public interface IOperatingSystem
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreatePackageFolder(string initialPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool FileExists(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DirectoryExists(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile GetFile(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] GetFiles(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] GetDirectoriesRecursive(string rootFolder, string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] GetFilesRecursive(string rootFolder, string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetFileBinaryContent(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetFileBinaryContentUsingEncoding(string path, string encoding);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetFileBinaryChunk(string path, int offset, int length);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetFileTextContent(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateFile(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateDirectory(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteFile(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteFiles(string[] files);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteEmptyDirectories(string[] directories);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateFileBinaryContent(string path, byte[] content);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AppendFileBinaryContent(string path, byte[] chunk);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateFileTextContent(string path, string content);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void MoveFile(string sourcePath, string destinationPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CopyFile(string sourcePath, string destinationPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ZipFiles(string zipFile, string rootPath, string[] files);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] UnzipFiles(string zipFile, string destFolder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateBackupZip(string zipFile, string rootPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateAccessDatabase(string databasePath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteDirectoryRecursive(string rootPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckFileServicesInstallation();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool InstallFsrmService();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        FolderGraph GetFolderGraph(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ExecuteSyncActions(FileSyncAction[] actions);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetInstalledOdbcDrivers();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetDSNNames();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemDSN GetDSN(string dsnName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateDSN(SystemDSN dsn);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateDSN(SystemDSN dsn);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteDSN(string dsnName);
    }

    // wcf service
    public class OperatingSystemService : OperatingSystem, IOperatingSystem
    {
        public new string CreatePackageFolder(string initialPath)
        {
            return base.CreatePackageFolder(initialPath);
        }

        public new bool FileExists(string path)
        {
            return base.FileExists(path);
        }

        public new bool DirectoryExists(string path)
        {
            return base.DirectoryExists(path);
        }

        public new SystemFile GetFile(string path)
        {
            return base.GetFile(path);
        }

        public new SystemFile[] GetFiles(string path)
        {
            return base.GetFiles(path);
        }

        public new SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            return base.GetDirectoriesRecursive(rootFolder, path);
        }

        public new SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            return base.GetFilesRecursive(rootFolder, path);
        }

        public new SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            return base.GetFilesRecursiveByPattern(rootFolder, path, pattern);
        }

        public new byte[] GetFileBinaryContent(string path)
        {
            return base.GetFileBinaryContent(path);
        }

        public new byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
        {
            return base.GetFileBinaryContentUsingEncoding(path, encoding);
        }

        public new byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return base.GetFileBinaryChunk(path, offset, length);
        }

        public new string GetFileTextContent(string path)
        {
            return base.GetFileTextContent(path);
        }

        public new void CreateFile(string path)
        {
            base.CreateFile(path);
        }

        public new void CreateDirectory(string path)
        {
            base.CreateDirectory(path);
        }

        public new void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime)
        {
            base.ChangeFileAttributes(path, createdTime, changedTime);
        }

        public new void DeleteFile(string path)
        {
            base.DeleteFile(path);
        }

        public new void DeleteFiles(string[] files)
        {
            base.DeleteFiles(files);
        }

        public new void DeleteEmptyDirectories(string[] directories)
        {
            base.DeleteEmptyDirectories(directories);
        }

        public new void UpdateFileBinaryContent(string path, byte[] content)
        {
            base.UpdateFileBinaryContent(path, content);
        }

        public new void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
        {
            base.UpdateFileBinaryContentUsingEncoding(path, content, encoding);
        }

        public new void AppendFileBinaryContent(string path, byte[] chunk)
        {
            base.AppendFileBinaryContent(path, chunk);
        }

        public new void UpdateFileTextContent(string path, string content)
        {
            base.UpdateFileTextContent(path, content);
        }

        public new void MoveFile(string sourcePath, string destinationPath)
        {
            base.MoveFile(sourcePath, destinationPath);
        }

        public new void CopyFile(string sourcePath, string destinationPath)
        {
            base.CopyFile(sourcePath, destinationPath);
        }

        public new void ZipFiles(string zipFile, string rootPath, string[] files)
        {
            base.ZipFiles(zipFile, rootPath, files);
        }

        public new string[] UnzipFiles(string zipFile, string destFolder)
        {
            return base.UnzipFiles(zipFile, destFolder);
        }

        public new void CreateBackupZip(string zipFile, string rootPath)
        {
            base.CreateBackupZip(zipFile, rootPath);
        }

        public new void CreateAccessDatabase(string databasePath)
        {
            base.CreateAccessDatabase(databasePath);
        }

        public new UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU)
        {
            return base.GetGroupNtfsPermissions(path, users, usersOU);
        }

        public new void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions)
        {
            base.GrantGroupNtfsPermissions(path, users, usersOU, resetChildPermissions);
        }

        public new void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            base.SetQuotaLimitOnFolder(folderPath, shareNameDrive, quotaType, quotaLimit, mode, wmiUserName, wmiPassword);
        }

        public new Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
        {
            return base.GetQuotaOnFolder(folderPath, wmiUserName, wmiPassword);
        }

        public new void DeleteDirectoryRecursive(string rootPath)
        {
            base.DeleteDirectoryRecursive(rootPath);
        }

        public new bool CheckFileServicesInstallation()
        {
            return base.CheckFileServicesInstallation();
        }

        public new bool InstallFsrmService()
        {
            return base.InstallFsrmService();
        }

        public new FolderGraph GetFolderGraph(string path)
        {
            return base.GetFolderGraph(path);
        }

        public new void ExecuteSyncActions(FileSyncAction[] actions)
        {
            base.ExecuteSyncActions(actions);
        }

        public new string[] GetInstalledOdbcDrivers()
        {
            return base.GetInstalledOdbcDrivers();
        }

        public new string[] GetDSNNames()
        {
            return base.GetDSNNames();
        }

        public new SystemDSN GetDSN(string dsnName)
        {
            return base.GetDSN(dsnName);
        }

        public new void CreateDSN(SystemDSN dsn)
        {
            base.CreateDSN(dsn);
        }

        public new void UpdateDSN(SystemDSN dsn)
        {
            base.UpdateDSN(dsn);
        }

        public new void DeleteDSN(string dsnName)
        {
            base.DeleteDSN(dsnName);
        }
    }
}
#endif