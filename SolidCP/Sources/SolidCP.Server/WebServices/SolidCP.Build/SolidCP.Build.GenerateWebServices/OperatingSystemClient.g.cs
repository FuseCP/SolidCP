#if Client
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

namespace SolidCP.Server.Client
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
}
#endif