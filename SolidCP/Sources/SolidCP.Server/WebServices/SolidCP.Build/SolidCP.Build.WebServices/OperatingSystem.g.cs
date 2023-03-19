#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.DNS;
using SolidCP.Providers.DomainLookup;
using System.Collections.Generic;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
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
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        UnixFileMode GetUnixPermissions(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void GrantUnixPermissions(string path, UnixFileMode mode, bool resetChildPermissions = false);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        TerminalSession[] GetTerminalServicesSessions();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CloseTerminalServicesSession(int sessionId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<string> GetLogNames();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<SystemLogEntry> GetLogEntries(string logName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ClearLog(string logName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        OSProcess[] GetOSProcesses();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void TerminateOSProcess(int pid);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        OSService[] GetOSServices();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeOSServiceStatus(string id, OSServiceStatus status);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RebootSystem();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Memory GetMemory();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string ExecuteSystemCommand(string path, string args);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WPIProduct[] GetWPIProducts(string tabId, string keywordId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WPIProduct[] GetWPIProductsFiltered(string filter);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WPIProduct GetWPIProductById(string productdId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WPITab[] GetWPITabs();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void InitWPIFeeds(string feedUrls);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WPIKeyword[] GetWPIKeywords();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WPIProduct[] GetWPIProductsWithDependencies(string[] products);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void InstallWPIProducts(string[] products);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CancelInstallWPIProducts();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetWPIStatus();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string WpiGetLogFileDirectory();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SettingPair[] WpiGetLogsInDirectory(string Path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool IsUnix();
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class OperatingSystem : SolidCP.Server.OperatingSystem, IOperatingSystem
    {
    }
}
#endif