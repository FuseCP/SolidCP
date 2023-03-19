#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IOperatingSystem", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IOperatingSystem
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreatePackageFolder", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreatePackageFolderResponse")]
        string CreatePackageFolder(string initialPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreatePackageFolder", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreatePackageFolderResponse")]
        System.Threading.Tasks.Task<string> CreatePackageFolderAsync(string initialPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/FileExists", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/FileExistsResponse")]
        bool FileExists(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/FileExists", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/FileExistsResponse")]
        System.Threading.Tasks.Task<bool> FileExistsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DirectoryExistsResponse")]
        bool DirectoryExists(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DirectoryExistsResponse")]
        System.Threading.Tasks.Task<bool> DirectoryExistsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileResponse")]
        SolidCP.Providers.OS.SystemFile GetFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesResponse")]
        SolidCP.Providers.OS.SystemFile[] GetFiles(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetDirectoriesRecursive", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetDirectoriesRecursiveResponse")]
        SolidCP.Providers.OS.SystemFile[] GetDirectoriesRecursive(string rootFolder, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetDirectoriesRecursive", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetDirectoriesRecursiveResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetDirectoriesRecursiveAsync(string rootFolder, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursive", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursiveResponse")]
        SolidCP.Providers.OS.SystemFile[] GetFilesRecursive(string rootFolder, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursive", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursiveResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveAsync(string rootFolder, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursiveByPattern", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursiveByPatternResponse")]
        SolidCP.Providers.OS.SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursiveByPattern", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFilesRecursiveByPatternResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveByPatternAsync(string rootFolder, string path, string pattern);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContentResponse")]
        byte[] GetFileBinaryContent(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContentResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileBinaryContentAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContentUsingEncodingResponse")]
        byte[] GetFileBinaryContentUsingEncoding(string path, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryContentUsingEncodingResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileBinaryContentUsingEncodingAsync(string path, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryChunkResponse")]
        byte[] GetFileBinaryChunk(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileTextContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileTextContentResponse")]
        string GetFileTextContent(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileTextContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFileTextContentResponse")]
        System.Threading.Tasks.Task<string> GetFileTextContentAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateFileResponse")]
        void CreateFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateFileResponse")]
        System.Threading.Tasks.Task CreateFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDirectory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDirectoryResponse")]
        void CreateDirectory(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDirectory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDirectoryResponse")]
        System.Threading.Tasks.Task CreateDirectoryAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeFileAttributes", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeFileAttributesResponse")]
        void ChangeFileAttributes(string path, System.DateTime createdTime, System.DateTime changedTime);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeFileAttributes", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeFileAttributesResponse")]
        System.Threading.Tasks.Task ChangeFileAttributesAsync(string path, System.DateTime createdTime, System.DateTime changedTime);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFileResponse")]
        void DeleteFile(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFileResponse")]
        System.Threading.Tasks.Task DeleteFileAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFilesResponse")]
        void DeleteFiles(string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteFilesResponse")]
        System.Threading.Tasks.Task DeleteFilesAsync(string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteEmptyDirectories", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteEmptyDirectoriesResponse")]
        void DeleteEmptyDirectories(string[] directories);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteEmptyDirectories", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteEmptyDirectoriesResponse")]
        System.Threading.Tasks.Task DeleteEmptyDirectoriesAsync(string[] directories);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContentResponse")]
        void UpdateFileBinaryContent(string path, byte[] content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContentResponse")]
        System.Threading.Tasks.Task UpdateFileBinaryContentAsync(string path, byte[] content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContentUsingEncodingResponse")]
        void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileBinaryContentUsingEncodingResponse")]
        System.Threading.Tasks.Task UpdateFileBinaryContentUsingEncodingAsync(string path, byte[] content, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/AppendFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/AppendFileBinaryContentResponse")]
        void AppendFileBinaryContent(string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/AppendFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/AppendFileBinaryContentResponse")]
        System.Threading.Tasks.Task AppendFileBinaryContentAsync(string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileTextContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileTextContentResponse")]
        void UpdateFileTextContent(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileTextContent", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateFileTextContentResponse")]
        System.Threading.Tasks.Task UpdateFileTextContentAsync(string path, string content);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/MoveFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/MoveFileResponse")]
        void MoveFile(string sourcePath, string destinationPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/MoveFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/MoveFileResponse")]
        System.Threading.Tasks.Task MoveFileAsync(string sourcePath, string destinationPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CopyFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CopyFileResponse")]
        void CopyFile(string sourcePath, string destinationPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CopyFile", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CopyFileResponse")]
        System.Threading.Tasks.Task CopyFileAsync(string sourcePath, string destinationPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ZipFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ZipFilesResponse")]
        void ZipFiles(string zipFile, string rootPath, string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ZipFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ZipFilesResponse")]
        System.Threading.Tasks.Task ZipFilesAsync(string zipFile, string rootPath, string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UnzipFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UnzipFilesResponse")]
        string[] UnzipFiles(string zipFile, string destFolder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UnzipFiles", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UnzipFilesResponse")]
        System.Threading.Tasks.Task<string[]> UnzipFilesAsync(string zipFile, string destFolder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateBackupZip", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateBackupZipResponse")]
        void CreateBackupZip(string zipFile, string rootPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateBackupZip", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateBackupZipResponse")]
        System.Threading.Tasks.Task CreateBackupZipAsync(string zipFile, string rootPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateAccessDatabase", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateAccessDatabaseResponse")]
        void CreateAccessDatabase(string databasePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateAccessDatabase", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateAccessDatabaseResponse")]
        System.Threading.Tasks.Task CreateAccessDatabaseAsync(string databasePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetGroupNtfsPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetGroupNtfsPermissionsResponse")]
        SolidCP.Providers.OS.UserPermission[] GetGroupNtfsPermissions(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetGroupNtfsPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetGroupNtfsPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.UserPermission[]> GetGroupNtfsPermissionsAsync(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GrantGroupNtfsPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GrantGroupNtfsPermissionsResponse")]
        void GrantGroupNtfsPermissions(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU, bool resetChildPermissions);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GrantGroupNtfsPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GrantGroupNtfsPermissionsResponse")]
        System.Threading.Tasks.Task GrantGroupNtfsPermissionsAsync(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU, bool resetChildPermissions);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/SetQuotaLimitOnFolder", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/SetQuotaLimitOnFolderResponse")]
        void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, SolidCP.Providers.OS.QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/SetQuotaLimitOnFolder", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/SetQuotaLimitOnFolderResponse")]
        System.Threading.Tasks.Task SetQuotaLimitOnFolderAsync(string folderPath, string shareNameDrive, SolidCP.Providers.OS.QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetQuotaOnFolder", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetQuotaOnFolderResponse")]
        SolidCP.Providers.OS.Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetQuotaOnFolder", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetQuotaOnFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.Quota> GetQuotaOnFolderAsync(string folderPath, string wmiUserName, string wmiPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDirectoryRecursive", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDirectoryRecursiveResponse")]
        void DeleteDirectoryRecursive(string rootPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDirectoryRecursive", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDirectoryRecursiveResponse")]
        System.Threading.Tasks.Task DeleteDirectoryRecursiveAsync(string rootPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CheckFileServicesInstallationResponse")]
        bool CheckFileServicesInstallation();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CheckFileServicesInstallationResponse")]
        System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/InstallFsrmService", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/InstallFsrmServiceResponse")]
        bool InstallFsrmService();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/InstallFsrmService", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/InstallFsrmServiceResponse")]
        System.Threading.Tasks.Task<bool> InstallFsrmServiceAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFolderGraph", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFolderGraphResponse")]
        SolidCP.Providers.OS.FolderGraph GetFolderGraph(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetFolderGraph", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetFolderGraphResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.FolderGraph> GetFolderGraphAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSyncActions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSyncActionsResponse")]
        void ExecuteSyncActions(SolidCP.Providers.OS.FileSyncAction[] actions);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSyncActions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSyncActionsResponse")]
        System.Threading.Tasks.Task ExecuteSyncActionsAsync(SolidCP.Providers.OS.FileSyncAction[] actions);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetInstalledOdbcDrivers", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetInstalledOdbcDriversResponse")]
        string[] GetInstalledOdbcDrivers();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetInstalledOdbcDrivers", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetInstalledOdbcDriversResponse")]
        System.Threading.Tasks.Task<string[]> GetInstalledOdbcDriversAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSNNames", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSNNamesResponse")]
        string[] GetDSNNames();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSNNames", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSNNamesResponse")]
        System.Threading.Tasks.Task<string[]> GetDSNNamesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSNResponse")]
        SolidCP.Providers.OS.SystemDSN GetDSN(string dsnName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetDSNResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN> GetDSNAsync(string dsnName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDSNResponse")]
        void CreateDSN(SolidCP.Providers.OS.SystemDSN dsn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CreateDSNResponse")]
        System.Threading.Tasks.Task CreateDSNAsync(SolidCP.Providers.OS.SystemDSN dsn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateDSNResponse")]
        void UpdateDSN(SolidCP.Providers.OS.SystemDSN dsn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/UpdateDSNResponse")]
        System.Threading.Tasks.Task UpdateDSNAsync(SolidCP.Providers.OS.SystemDSN dsn);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDSNResponse")]
        void DeleteDSN(string dsnName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDSN", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/DeleteDSNResponse")]
        System.Threading.Tasks.Task DeleteDSNAsync(string dsnName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetUnixPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetUnixPermissionsResponse")]
        SolidCP.Providers.OS.UnixFileMode GetUnixPermissions(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetUnixPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetUnixPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.UnixFileMode> GetUnixPermissionsAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GrantUnixPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GrantUnixPermissionsResponse")]
        void GrantUnixPermissions(string path, SolidCP.Providers.OS.UnixFileMode mode, bool resetChildPermissions = false);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GrantUnixPermissions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GrantUnixPermissionsResponse")]
        System.Threading.Tasks.Task GrantUnixPermissionsAsync(string path, SolidCP.Providers.OS.UnixFileMode mode, bool resetChildPermissions = false);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetTerminalServicesSessionsResponse")]
        SolidCP.Providers.OS.TerminalSession[] GetTerminalServicesSessions();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetTerminalServicesSessions", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetTerminalServicesSessionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.TerminalSession[]> GetTerminalServicesSessionsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CloseTerminalServicesSessionResponse")]
        void CloseTerminalServicesSession(int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CloseTerminalServicesSession", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CloseTerminalServicesSessionResponse")]
        System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogNames", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogNamesResponse")]
        string[] /*List*/ GetLogNames();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogNames", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogNamesResponse")]
        System.Threading.Tasks.Task<string[]> GetLogNamesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntriesResponse")]
        SolidCP.Providers.OS.SystemLogEntry[] /*List*/ GetLogEntries(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntries", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntry[]> GetLogEntriesAsync(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntriesPagedResponse")]
        SolidCP.Providers.OS.SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntriesPaged", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetLogEntriesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ClearLog", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ClearLogResponse")]
        void ClearLog(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ClearLog", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ClearLogResponse")]
        System.Threading.Tasks.Task ClearLogAsync(string logName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSProcesses", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSProcessesResponse")]
        SolidCP.Providers.OS.OSProcess[] GetOSProcesses();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSProcesses", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSProcessesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.OSProcess[]> GetOSProcessesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/TerminateOSProcess", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/TerminateOSProcessResponse")]
        void TerminateOSProcess(int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/TerminateOSProcess", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/TerminateOSProcessResponse")]
        System.Threading.Tasks.Task TerminateOSProcessAsync(int pid);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSServices", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSServicesResponse")]
        SolidCP.Providers.OS.OSService[] GetOSServices();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSServices", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetOSServicesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.OSService[]> GetOSServicesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeOSServiceStatus", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeOSServiceStatusResponse")]
        void ChangeOSServiceStatus(string id, SolidCP.Providers.OS.OSServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeOSServiceStatus", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ChangeOSServiceStatusResponse")]
        System.Threading.Tasks.Task ChangeOSServiceStatusAsync(string id, SolidCP.Providers.OS.OSServiceStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/RebootSystem", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/RebootSystemResponse")]
        void RebootSystem();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/RebootSystem", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/RebootSystemResponse")]
        System.Threading.Tasks.Task RebootSystemAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetMemory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetMemoryResponse")]
        SolidCP.Providers.OS.Memory GetMemory();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetMemory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetMemoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSystemCommand", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSystemCommandResponse")]
        string ExecuteSystemCommand(string path, string args);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSystemCommand", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/ExecuteSystemCommandResponse")]
        System.Threading.Tasks.Task<string> ExecuteSystemCommandAsync(string path, string args);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProducts(string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsFilteredResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(string filter);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsFilteredResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(string filter);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductByIdResponse")]
        SolidCP.Server.WPIProduct GetWPIProductById(string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductById", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(string productdId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPITabsResponse")]
        SolidCP.Server.WPITab[] GetWPITabs();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPITabs", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPITabsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/InitWPIFeedsResponse")]
        void InitWPIFeeds(string feedUrls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/InitWPIFeeds", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/InitWPIFeedsResponse")]
        System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIKeywordsResponse")]
        SolidCP.Server.WPIKeyword[] GetWPIKeywords();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIKeywords", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIKeywordsResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsWithDependenciesResponse")]
        SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsWithDependencies", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIProductsWithDependenciesResponse")]
        System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/InstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/InstallWPIProductsResponse")]
        void InstallWPIProducts(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/InstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/InstallWPIProductsResponse")]
        System.Threading.Tasks.Task InstallWPIProductsAsync(string[] products);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CancelInstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CancelInstallWPIProductsResponse")]
        void CancelInstallWPIProducts();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/CancelInstallWPIProducts", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/CancelInstallWPIProductsResponse")]
        System.Threading.Tasks.Task CancelInstallWPIProductsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIStatus", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIStatusResponse")]
        string GetWPIStatus();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIStatus", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/GetWPIStatusResponse")]
        System.Threading.Tasks.Task<string> GetWPIStatusAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogFileDirectory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogFileDirectoryResponse")]
        string WpiGetLogFileDirectory();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogFileDirectory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogFileDirectoryResponse")]
        System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogsInDirectory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogsInDirectoryResponse")]
        SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogsInDirectory", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/WpiGetLogsInDirectoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(string Path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/IsUnix", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/IsUnixResponse")]
        bool IsUnix();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IOperatingSystem/IsUnix", ReplyAction = "http://smbsaas/solidcp/server/IOperatingSystem/IsUnixResponse")]
        System.Threading.Tasks.Task<bool> IsUnixAsync();
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OperatingSystemAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IOperatingSystem
    {
        public string CreatePackageFolder(string initialPath)
        {
            return Invoke<string>("SolidCP.Server.OperatingSystem", "CreatePackageFolder", initialPath);
        }

        public async System.Threading.Tasks.Task<string> CreatePackageFolderAsync(string initialPath)
        {
            return await InvokeAsync<string>("SolidCP.Server.OperatingSystem", "CreatePackageFolder", initialPath);
        }

        public bool FileExists(string path)
        {
            return Invoke<bool>("SolidCP.Server.OperatingSystem", "FileExists", path);
        }

        public async System.Threading.Tasks.Task<bool> FileExistsAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "FileExists", path);
        }

        public bool DirectoryExists(string path)
        {
            return Invoke<bool>("SolidCP.Server.OperatingSystem", "DirectoryExists", path);
        }

        public async System.Threading.Tasks.Task<bool> DirectoryExistsAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "DirectoryExists", path);
        }

        public SolidCP.Providers.OS.SystemFile GetFile(string path)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile>("SolidCP.Server.OperatingSystem", "GetFile", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetFileAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile>("SolidCP.Server.OperatingSystem", "GetFile", path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFiles(string path)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFiles", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFiles", path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetDirectoriesRecursive", rootFolder, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetDirectoriesRecursiveAsync(string rootFolder, string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetDirectoriesRecursive", rootFolder, path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFilesRecursive", rootFolder, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveAsync(string rootFolder, string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFilesRecursive", rootFolder, path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFilesRecursiveByPattern", rootFolder, path, pattern);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveByPatternAsync(string rootFolder, string path, string pattern)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFilesRecursiveByPattern", rootFolder, path, pattern);
        }

        public byte[] GetFileBinaryContent(string path)
        {
            return Invoke<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryContent", path);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentAsync(string path)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryContent", path);
        }

        public byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
        {
            return Invoke<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryContentUsingEncoding", path, encoding);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentUsingEncodingAsync(string path, string encoding)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryContentUsingEncoding", path, encoding);
        }

        public byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryChunk", path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryChunk", path, offset, length);
        }

        public string GetFileTextContent(string path)
        {
            return Invoke<string>("SolidCP.Server.OperatingSystem", "GetFileTextContent", path);
        }

        public async System.Threading.Tasks.Task<string> GetFileTextContentAsync(string path)
        {
            return await InvokeAsync<string>("SolidCP.Server.OperatingSystem", "GetFileTextContent", path);
        }

        public void CreateFile(string path)
        {
            Invoke("SolidCP.Server.OperatingSystem", "CreateFile", path);
        }

        public async System.Threading.Tasks.Task CreateFileAsync(string path)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CreateFile", path);
        }

        public void CreateDirectory(string path)
        {
            Invoke("SolidCP.Server.OperatingSystem", "CreateDirectory", path);
        }

        public async System.Threading.Tasks.Task CreateDirectoryAsync(string path)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CreateDirectory", path);
        }

        public void ChangeFileAttributes(string path, System.DateTime createdTime, System.DateTime changedTime)
        {
            Invoke("SolidCP.Server.OperatingSystem", "ChangeFileAttributes", path, createdTime, changedTime);
        }

        public async System.Threading.Tasks.Task ChangeFileAttributesAsync(string path, System.DateTime createdTime, System.DateTime changedTime)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "ChangeFileAttributes", path, createdTime, changedTime);
        }

        public void DeleteFile(string path)
        {
            Invoke("SolidCP.Server.OperatingSystem", "DeleteFile", path);
        }

        public async System.Threading.Tasks.Task DeleteFileAsync(string path)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "DeleteFile", path);
        }

        public void DeleteFiles(string[] files)
        {
            Invoke("SolidCP.Server.OperatingSystem", "DeleteFiles", files);
        }

        public async System.Threading.Tasks.Task DeleteFilesAsync(string[] files)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "DeleteFiles", files);
        }

        public void DeleteEmptyDirectories(string[] directories)
        {
            Invoke("SolidCP.Server.OperatingSystem", "DeleteEmptyDirectories", directories);
        }

        public async System.Threading.Tasks.Task DeleteEmptyDirectoriesAsync(string[] directories)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "DeleteEmptyDirectories", directories);
        }

        public void UpdateFileBinaryContent(string path, byte[] content)
        {
            Invoke("SolidCP.Server.OperatingSystem", "UpdateFileBinaryContent", path, content);
        }

        public async System.Threading.Tasks.Task UpdateFileBinaryContentAsync(string path, byte[] content)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "UpdateFileBinaryContent", path, content);
        }

        public void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
        {
            Invoke("SolidCP.Server.OperatingSystem", "UpdateFileBinaryContentUsingEncoding", path, content, encoding);
        }

        public async System.Threading.Tasks.Task UpdateFileBinaryContentUsingEncodingAsync(string path, byte[] content, string encoding)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "UpdateFileBinaryContentUsingEncoding", path, content, encoding);
        }

        public void AppendFileBinaryContent(string path, byte[] chunk)
        {
            Invoke("SolidCP.Server.OperatingSystem", "AppendFileBinaryContent", path, chunk);
        }

        public async System.Threading.Tasks.Task AppendFileBinaryContentAsync(string path, byte[] chunk)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "AppendFileBinaryContent", path, chunk);
        }

        public void UpdateFileTextContent(string path, string content)
        {
            Invoke("SolidCP.Server.OperatingSystem", "UpdateFileTextContent", path, content);
        }

        public async System.Threading.Tasks.Task UpdateFileTextContentAsync(string path, string content)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "UpdateFileTextContent", path, content);
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            Invoke("SolidCP.Server.OperatingSystem", "MoveFile", sourcePath, destinationPath);
        }

        public async System.Threading.Tasks.Task MoveFileAsync(string sourcePath, string destinationPath)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "MoveFile", sourcePath, destinationPath);
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            Invoke("SolidCP.Server.OperatingSystem", "CopyFile", sourcePath, destinationPath);
        }

        public async System.Threading.Tasks.Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CopyFile", sourcePath, destinationPath);
        }

        public void ZipFiles(string zipFile, string rootPath, string[] files)
        {
            Invoke("SolidCP.Server.OperatingSystem", "ZipFiles", zipFile, rootPath, files);
        }

        public async System.Threading.Tasks.Task ZipFilesAsync(string zipFile, string rootPath, string[] files)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "ZipFiles", zipFile, rootPath, files);
        }

        public string[] UnzipFiles(string zipFile, string destFolder)
        {
            return Invoke<string[]>("SolidCP.Server.OperatingSystem", "UnzipFiles", zipFile, destFolder);
        }

        public async System.Threading.Tasks.Task<string[]> UnzipFilesAsync(string zipFile, string destFolder)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.OperatingSystem", "UnzipFiles", zipFile, destFolder);
        }

        public void CreateBackupZip(string zipFile, string rootPath)
        {
            Invoke("SolidCP.Server.OperatingSystem", "CreateBackupZip", zipFile, rootPath);
        }

        public async System.Threading.Tasks.Task CreateBackupZipAsync(string zipFile, string rootPath)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CreateBackupZip", zipFile, rootPath);
        }

        public void CreateAccessDatabase(string databasePath)
        {
            Invoke("SolidCP.Server.OperatingSystem", "CreateAccessDatabase", databasePath);
        }

        public async System.Threading.Tasks.Task CreateAccessDatabaseAsync(string databasePath)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CreateAccessDatabase", databasePath);
        }

        public SolidCP.Providers.OS.UserPermission[] GetGroupNtfsPermissions(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU)
        {
            return Invoke<SolidCP.Providers.OS.UserPermission[]>("SolidCP.Server.OperatingSystem", "GetGroupNtfsPermissions", path, users, usersOU);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.UserPermission[]> GetGroupNtfsPermissionsAsync(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU)
        {
            return await InvokeAsync<SolidCP.Providers.OS.UserPermission[]>("SolidCP.Server.OperatingSystem", "GetGroupNtfsPermissions", path, users, usersOU);
        }

        public void GrantGroupNtfsPermissions(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU, bool resetChildPermissions)
        {
            Invoke("SolidCP.Server.OperatingSystem", "GrantGroupNtfsPermissions", path, users, usersOU, resetChildPermissions);
        }

        public async System.Threading.Tasks.Task GrantGroupNtfsPermissionsAsync(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU, bool resetChildPermissions)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "GrantGroupNtfsPermissions", path, users, usersOU, resetChildPermissions);
        }

        public void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, SolidCP.Providers.OS.QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            Invoke("SolidCP.Server.OperatingSystem", "SetQuotaLimitOnFolder", folderPath, shareNameDrive, quotaType, quotaLimit, mode, wmiUserName, wmiPassword);
        }

        public async System.Threading.Tasks.Task SetQuotaLimitOnFolderAsync(string folderPath, string shareNameDrive, SolidCP.Providers.OS.QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "SetQuotaLimitOnFolder", folderPath, shareNameDrive, quotaType, quotaLimit, mode, wmiUserName, wmiPassword);
        }

        public SolidCP.Providers.OS.Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
        {
            return Invoke<SolidCP.Providers.OS.Quota>("SolidCP.Server.OperatingSystem", "GetQuotaOnFolder", folderPath, wmiUserName, wmiPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Quota> GetQuotaOnFolderAsync(string folderPath, string wmiUserName, string wmiPassword)
        {
            return await InvokeAsync<SolidCP.Providers.OS.Quota>("SolidCP.Server.OperatingSystem", "GetQuotaOnFolder", folderPath, wmiUserName, wmiPassword);
        }

        public void DeleteDirectoryRecursive(string rootPath)
        {
            Invoke("SolidCP.Server.OperatingSystem", "DeleteDirectoryRecursive", rootPath);
        }

        public async System.Threading.Tasks.Task DeleteDirectoryRecursiveAsync(string rootPath)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "DeleteDirectoryRecursive", rootPath);
        }

        public bool CheckFileServicesInstallation()
        {
            return Invoke<bool>("SolidCP.Server.OperatingSystem", "CheckFileServicesInstallation");
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "CheckFileServicesInstallation");
        }

        public bool InstallFsrmService()
        {
            return Invoke<bool>("SolidCP.Server.OperatingSystem", "InstallFsrmService");
        }

        public async System.Threading.Tasks.Task<bool> InstallFsrmServiceAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "InstallFsrmService");
        }

        public SolidCP.Providers.OS.FolderGraph GetFolderGraph(string path)
        {
            return Invoke<SolidCP.Providers.OS.FolderGraph>("SolidCP.Server.OperatingSystem", "GetFolderGraph", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.FolderGraph> GetFolderGraphAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.FolderGraph>("SolidCP.Server.OperatingSystem", "GetFolderGraph", path);
        }

        public void ExecuteSyncActions(SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            Invoke("SolidCP.Server.OperatingSystem", "ExecuteSyncActions", actions);
        }

        public async System.Threading.Tasks.Task ExecuteSyncActionsAsync(SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "ExecuteSyncActions", actions);
        }

        public string[] GetInstalledOdbcDrivers()
        {
            return Invoke<string[]>("SolidCP.Server.OperatingSystem", "GetInstalledOdbcDrivers");
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledOdbcDriversAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.OperatingSystem", "GetInstalledOdbcDrivers");
        }

        public string[] GetDSNNames()
        {
            return Invoke<string[]>("SolidCP.Server.OperatingSystem", "GetDSNNames");
        }

        public async System.Threading.Tasks.Task<string[]> GetDSNNamesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.OperatingSystem", "GetDSNNames");
        }

        public SolidCP.Providers.OS.SystemDSN GetDSN(string dsnName)
        {
            return Invoke<SolidCP.Providers.OS.SystemDSN>("SolidCP.Server.OperatingSystem", "GetDSN", dsnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN> GetDSNAsync(string dsnName)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemDSN>("SolidCP.Server.OperatingSystem", "GetDSN", dsnName);
        }

        public void CreateDSN(SolidCP.Providers.OS.SystemDSN dsn)
        {
            Invoke("SolidCP.Server.OperatingSystem", "CreateDSN", dsn);
        }

        public async System.Threading.Tasks.Task CreateDSNAsync(SolidCP.Providers.OS.SystemDSN dsn)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CreateDSN", dsn);
        }

        public void UpdateDSN(SolidCP.Providers.OS.SystemDSN dsn)
        {
            Invoke("SolidCP.Server.OperatingSystem", "UpdateDSN", dsn);
        }

        public async System.Threading.Tasks.Task UpdateDSNAsync(SolidCP.Providers.OS.SystemDSN dsn)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "UpdateDSN", dsn);
        }

        public void DeleteDSN(string dsnName)
        {
            Invoke("SolidCP.Server.OperatingSystem", "DeleteDSN", dsnName);
        }

        public async System.Threading.Tasks.Task DeleteDSNAsync(string dsnName)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "DeleteDSN", dsnName);
        }

        public SolidCP.Providers.OS.UnixFileMode GetUnixPermissions(string path)
        {
            return Invoke<SolidCP.Providers.OS.UnixFileMode>("SolidCP.Server.OperatingSystem", "GetUnixPermissions", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.UnixFileMode> GetUnixPermissionsAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.UnixFileMode>("SolidCP.Server.OperatingSystem", "GetUnixPermissions", path);
        }

        public void GrantUnixPermissions(string path, SolidCP.Providers.OS.UnixFileMode mode, bool resetChildPermissions = false)
        {
            Invoke("SolidCP.Server.OperatingSystem", "GrantUnixPermissions", path, mode, resetChildPermissions);
        }

        public async System.Threading.Tasks.Task GrantUnixPermissionsAsync(string path, SolidCP.Providers.OS.UnixFileMode mode, bool resetChildPermissions = false)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "GrantUnixPermissions", path, mode, resetChildPermissions);
        }

        public SolidCP.Providers.OS.TerminalSession[] GetTerminalServicesSessions()
        {
            return Invoke<SolidCP.Providers.OS.TerminalSession[]>("SolidCP.Server.OperatingSystem", "GetTerminalServicesSessions");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.TerminalSession[]> GetTerminalServicesSessionsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.OS.TerminalSession[]>("SolidCP.Server.OperatingSystem", "GetTerminalServicesSessions");
        }

        public void CloseTerminalServicesSession(int sessionId)
        {
            Invoke("SolidCP.Server.OperatingSystem", "CloseTerminalServicesSession", sessionId);
        }

        public async System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CloseTerminalServicesSession", sessionId);
        }

        public string[] /*List*/ GetLogNames()
        {
            return Invoke<string[], string>("SolidCP.Server.OperatingSystem", "GetLogNames");
        }

        public async System.Threading.Tasks.Task<string[]> GetLogNamesAsync()
        {
            return await InvokeAsync<string[], string>("SolidCP.Server.OperatingSystem", "GetLogNames");
        }

        public SolidCP.Providers.OS.SystemLogEntry[] /*List*/ GetLogEntries(string logName)
        {
            return Invoke<SolidCP.Providers.OS.SystemLogEntry[], SolidCP.Providers.OS.SystemLogEntry>("SolidCP.Server.OperatingSystem", "GetLogEntries", logName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntry[]> GetLogEntriesAsync(string logName)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemLogEntry[], SolidCP.Providers.OS.SystemLogEntry>("SolidCP.Server.OperatingSystem", "GetLogEntries", logName);
        }

        public SolidCP.Providers.OS.SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.OS.SystemLogEntriesPaged>("SolidCP.Server.OperatingSystem", "GetLogEntriesPaged", logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemLogEntriesPaged>("SolidCP.Server.OperatingSystem", "GetLogEntriesPaged", logName, startRow, maximumRows);
        }

        public void ClearLog(string logName)
        {
            Invoke("SolidCP.Server.OperatingSystem", "ClearLog", logName);
        }

        public async System.Threading.Tasks.Task ClearLogAsync(string logName)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "ClearLog", logName);
        }

        public SolidCP.Providers.OS.OSProcess[] GetOSProcesses()
        {
            return Invoke<SolidCP.Providers.OS.OSProcess[]>("SolidCP.Server.OperatingSystem", "GetOSProcesses");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSProcess[]> GetOSProcessesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.OS.OSProcess[]>("SolidCP.Server.OperatingSystem", "GetOSProcesses");
        }

        public void TerminateOSProcess(int pid)
        {
            Invoke("SolidCP.Server.OperatingSystem", "TerminateOSProcess", pid);
        }

        public async System.Threading.Tasks.Task TerminateOSProcessAsync(int pid)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "TerminateOSProcess", pid);
        }

        public SolidCP.Providers.OS.OSService[] GetOSServices()
        {
            return Invoke<SolidCP.Providers.OS.OSService[]>("SolidCP.Server.OperatingSystem", "GetOSServices");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSService[]> GetOSServicesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.OS.OSService[]>("SolidCP.Server.OperatingSystem", "GetOSServices");
        }

        public void ChangeOSServiceStatus(string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            Invoke("SolidCP.Server.OperatingSystem", "ChangeOSServiceStatus", id, status);
        }

        public async System.Threading.Tasks.Task ChangeOSServiceStatusAsync(string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "ChangeOSServiceStatus", id, status);
        }

        public void RebootSystem()
        {
            Invoke("SolidCP.Server.OperatingSystem", "RebootSystem");
        }

        public async System.Threading.Tasks.Task RebootSystemAsync()
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "RebootSystem");
        }

        public SolidCP.Providers.OS.Memory GetMemory()
        {
            return Invoke<SolidCP.Providers.OS.Memory>("SolidCP.Server.OperatingSystem", "GetMemory");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryAsync()
        {
            return await InvokeAsync<SolidCP.Providers.OS.Memory>("SolidCP.Server.OperatingSystem", "GetMemory");
        }

        public string ExecuteSystemCommand(string path, string args)
        {
            return Invoke<string>("SolidCP.Server.OperatingSystem", "ExecuteSystemCommand", path, args);
        }

        public async System.Threading.Tasks.Task<string> ExecuteSystemCommandAsync(string path, string args)
        {
            return await InvokeAsync<string>("SolidCP.Server.OperatingSystem", "ExecuteSystemCommand", path, args);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {
            return Invoke<SolidCP.Server.WPIProduct[]>("SolidCP.Server.OperatingSystem", "GetWPIProducts", tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.Server.OperatingSystem", "GetWPIProducts", tabId, keywordId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(string filter)
        {
            return Invoke<SolidCP.Server.WPIProduct[]>("SolidCP.Server.OperatingSystem", "GetWPIProductsFiltered", filter);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(string filter)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.Server.OperatingSystem", "GetWPIProductsFiltered", filter);
        }

        public SolidCP.Server.WPIProduct GetWPIProductById(string productdId)
        {
            return Invoke<SolidCP.Server.WPIProduct>("SolidCP.Server.OperatingSystem", "GetWPIProductById", productdId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(string productdId)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct>("SolidCP.Server.OperatingSystem", "GetWPIProductById", productdId);
        }

        public SolidCP.Server.WPITab[] GetWPITabs()
        {
            return Invoke<SolidCP.Server.WPITab[]>("SolidCP.Server.OperatingSystem", "GetWPITabs");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync()
        {
            return await InvokeAsync<SolidCP.Server.WPITab[]>("SolidCP.Server.OperatingSystem", "GetWPITabs");
        }

        public void InitWPIFeeds(string feedUrls)
        {
            Invoke("SolidCP.Server.OperatingSystem", "InitWPIFeeds", feedUrls);
        }

        public async System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "InitWPIFeeds", feedUrls);
        }

        public SolidCP.Server.WPIKeyword[] GetWPIKeywords()
        {
            return Invoke<SolidCP.Server.WPIKeyword[]>("SolidCP.Server.OperatingSystem", "GetWPIKeywords");
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync()
        {
            return await InvokeAsync<SolidCP.Server.WPIKeyword[]>("SolidCP.Server.OperatingSystem", "GetWPIKeywords");
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            return Invoke<SolidCP.Server.WPIProduct[]>("SolidCP.Server.OperatingSystem", "GetWPIProductsWithDependencies", products);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products)
        {
            return await InvokeAsync<SolidCP.Server.WPIProduct[]>("SolidCP.Server.OperatingSystem", "GetWPIProductsWithDependencies", products);
        }

        public void InstallWPIProducts(string[] products)
        {
            Invoke("SolidCP.Server.OperatingSystem", "InstallWPIProducts", products);
        }

        public async System.Threading.Tasks.Task InstallWPIProductsAsync(string[] products)
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "InstallWPIProducts", products);
        }

        public void CancelInstallWPIProducts()
        {
            Invoke("SolidCP.Server.OperatingSystem", "CancelInstallWPIProducts");
        }

        public async System.Threading.Tasks.Task CancelInstallWPIProductsAsync()
        {
            await InvokeAsync("SolidCP.Server.OperatingSystem", "CancelInstallWPIProducts");
        }

        public string GetWPIStatus()
        {
            return Invoke<string>("SolidCP.Server.OperatingSystem", "GetWPIStatus");
        }

        public async System.Threading.Tasks.Task<string> GetWPIStatusAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.OperatingSystem", "GetWPIStatus");
        }

        public string WpiGetLogFileDirectory()
        {
            return Invoke<string>("SolidCP.Server.OperatingSystem", "WpiGetLogFileDirectory");
        }

        public async System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.OperatingSystem", "WpiGetLogFileDirectory");
        }

        public SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            return Invoke<SolidCP.Providers.SettingPair[]>("SolidCP.Server.OperatingSystem", "WpiGetLogsInDirectory", Path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(string Path)
        {
            return await InvokeAsync<SolidCP.Providers.SettingPair[]>("SolidCP.Server.OperatingSystem", "WpiGetLogsInDirectory", Path);
        }

        public bool IsUnix()
        {
            return Invoke<bool>("SolidCP.Server.OperatingSystem", "IsUnix");
        }

        public async System.Threading.Tasks.Task<bool> IsUnixAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "IsUnix");
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OperatingSystem : SolidCP.Web.Client.ClientBase<IOperatingSystem, OperatingSystemAssemblyClient>, IOperatingSystem
    {
        public string CreatePackageFolder(string initialPath)
        {
            return base.Client.CreatePackageFolder(initialPath);
        }

        public async System.Threading.Tasks.Task<string> CreatePackageFolderAsync(string initialPath)
        {
            return await base.Client.CreatePackageFolderAsync(initialPath);
        }

        public bool FileExists(string path)
        {
            return base.Client.FileExists(path);
        }

        public async System.Threading.Tasks.Task<bool> FileExistsAsync(string path)
        {
            return await base.Client.FileExistsAsync(path);
        }

        public bool DirectoryExists(string path)
        {
            return base.Client.DirectoryExists(path);
        }

        public async System.Threading.Tasks.Task<bool> DirectoryExistsAsync(string path)
        {
            return await base.Client.DirectoryExistsAsync(path);
        }

        public SolidCP.Providers.OS.SystemFile GetFile(string path)
        {
            return base.Client.GetFile(path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetFileAsync(string path)
        {
            return await base.Client.GetFileAsync(path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFiles(string path)
        {
            return base.Client.GetFiles(path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesAsync(string path)
        {
            return await base.Client.GetFilesAsync(path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            return base.Client.GetDirectoriesRecursive(rootFolder, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetDirectoriesRecursiveAsync(string rootFolder, string path)
        {
            return await base.Client.GetDirectoriesRecursiveAsync(rootFolder, path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            return base.Client.GetFilesRecursive(rootFolder, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveAsync(string rootFolder, string path)
        {
            return await base.Client.GetFilesRecursiveAsync(rootFolder, path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            return base.Client.GetFilesRecursiveByPattern(rootFolder, path, pattern);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveByPatternAsync(string rootFolder, string path, string pattern)
        {
            return await base.Client.GetFilesRecursiveByPatternAsync(rootFolder, path, pattern);
        }

        public byte[] GetFileBinaryContent(string path)
        {
            return base.Client.GetFileBinaryContent(path);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentAsync(string path)
        {
            return await base.Client.GetFileBinaryContentAsync(path);
        }

        public byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
        {
            return base.Client.GetFileBinaryContentUsingEncoding(path, encoding);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentUsingEncodingAsync(string path, string encoding)
        {
            return await base.Client.GetFileBinaryContentUsingEncodingAsync(path, encoding);
        }

        public byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return base.Client.GetFileBinaryChunk(path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await base.Client.GetFileBinaryChunkAsync(path, offset, length);
        }

        public string GetFileTextContent(string path)
        {
            return base.Client.GetFileTextContent(path);
        }

        public async System.Threading.Tasks.Task<string> GetFileTextContentAsync(string path)
        {
            return await base.Client.GetFileTextContentAsync(path);
        }

        public void CreateFile(string path)
        {
            base.Client.CreateFile(path);
        }

        public async System.Threading.Tasks.Task CreateFileAsync(string path)
        {
            await base.Client.CreateFileAsync(path);
        }

        public void CreateDirectory(string path)
        {
            base.Client.CreateDirectory(path);
        }

        public async System.Threading.Tasks.Task CreateDirectoryAsync(string path)
        {
            await base.Client.CreateDirectoryAsync(path);
        }

        public void ChangeFileAttributes(string path, System.DateTime createdTime, System.DateTime changedTime)
        {
            base.Client.ChangeFileAttributes(path, createdTime, changedTime);
        }

        public async System.Threading.Tasks.Task ChangeFileAttributesAsync(string path, System.DateTime createdTime, System.DateTime changedTime)
        {
            await base.Client.ChangeFileAttributesAsync(path, createdTime, changedTime);
        }

        public void DeleteFile(string path)
        {
            base.Client.DeleteFile(path);
        }

        public async System.Threading.Tasks.Task DeleteFileAsync(string path)
        {
            await base.Client.DeleteFileAsync(path);
        }

        public void DeleteFiles(string[] files)
        {
            base.Client.DeleteFiles(files);
        }

        public async System.Threading.Tasks.Task DeleteFilesAsync(string[] files)
        {
            await base.Client.DeleteFilesAsync(files);
        }

        public void DeleteEmptyDirectories(string[] directories)
        {
            base.Client.DeleteEmptyDirectories(directories);
        }

        public async System.Threading.Tasks.Task DeleteEmptyDirectoriesAsync(string[] directories)
        {
            await base.Client.DeleteEmptyDirectoriesAsync(directories);
        }

        public void UpdateFileBinaryContent(string path, byte[] content)
        {
            base.Client.UpdateFileBinaryContent(path, content);
        }

        public async System.Threading.Tasks.Task UpdateFileBinaryContentAsync(string path, byte[] content)
        {
            await base.Client.UpdateFileBinaryContentAsync(path, content);
        }

        public void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
        {
            base.Client.UpdateFileBinaryContentUsingEncoding(path, content, encoding);
        }

        public async System.Threading.Tasks.Task UpdateFileBinaryContentUsingEncodingAsync(string path, byte[] content, string encoding)
        {
            await base.Client.UpdateFileBinaryContentUsingEncodingAsync(path, content, encoding);
        }

        public void AppendFileBinaryContent(string path, byte[] chunk)
        {
            base.Client.AppendFileBinaryContent(path, chunk);
        }

        public async System.Threading.Tasks.Task AppendFileBinaryContentAsync(string path, byte[] chunk)
        {
            await base.Client.AppendFileBinaryContentAsync(path, chunk);
        }

        public void UpdateFileTextContent(string path, string content)
        {
            base.Client.UpdateFileTextContent(path, content);
        }

        public async System.Threading.Tasks.Task UpdateFileTextContentAsync(string path, string content)
        {
            await base.Client.UpdateFileTextContentAsync(path, content);
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            base.Client.MoveFile(sourcePath, destinationPath);
        }

        public async System.Threading.Tasks.Task MoveFileAsync(string sourcePath, string destinationPath)
        {
            await base.Client.MoveFileAsync(sourcePath, destinationPath);
        }

        public void CopyFile(string sourcePath, string destinationPath)
        {
            base.Client.CopyFile(sourcePath, destinationPath);
        }

        public async System.Threading.Tasks.Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            await base.Client.CopyFileAsync(sourcePath, destinationPath);
        }

        public void ZipFiles(string zipFile, string rootPath, string[] files)
        {
            base.Client.ZipFiles(zipFile, rootPath, files);
        }

        public async System.Threading.Tasks.Task ZipFilesAsync(string zipFile, string rootPath, string[] files)
        {
            await base.Client.ZipFilesAsync(zipFile, rootPath, files);
        }

        public string[] UnzipFiles(string zipFile, string destFolder)
        {
            return base.Client.UnzipFiles(zipFile, destFolder);
        }

        public async System.Threading.Tasks.Task<string[]> UnzipFilesAsync(string zipFile, string destFolder)
        {
            return await base.Client.UnzipFilesAsync(zipFile, destFolder);
        }

        public void CreateBackupZip(string zipFile, string rootPath)
        {
            base.Client.CreateBackupZip(zipFile, rootPath);
        }

        public async System.Threading.Tasks.Task CreateBackupZipAsync(string zipFile, string rootPath)
        {
            await base.Client.CreateBackupZipAsync(zipFile, rootPath);
        }

        public void CreateAccessDatabase(string databasePath)
        {
            base.Client.CreateAccessDatabase(databasePath);
        }

        public async System.Threading.Tasks.Task CreateAccessDatabaseAsync(string databasePath)
        {
            await base.Client.CreateAccessDatabaseAsync(databasePath);
        }

        public SolidCP.Providers.OS.UserPermission[] GetGroupNtfsPermissions(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU)
        {
            return base.Client.GetGroupNtfsPermissions(path, users, usersOU);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.UserPermission[]> GetGroupNtfsPermissionsAsync(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU)
        {
            return await base.Client.GetGroupNtfsPermissionsAsync(path, users, usersOU);
        }

        public void GrantGroupNtfsPermissions(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU, bool resetChildPermissions)
        {
            base.Client.GrantGroupNtfsPermissions(path, users, usersOU, resetChildPermissions);
        }

        public async System.Threading.Tasks.Task GrantGroupNtfsPermissionsAsync(string path, SolidCP.Providers.OS.UserPermission[] users, string usersOU, bool resetChildPermissions)
        {
            await base.Client.GrantGroupNtfsPermissionsAsync(path, users, usersOU, resetChildPermissions);
        }

        public void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, SolidCP.Providers.OS.QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            base.Client.SetQuotaLimitOnFolder(folderPath, shareNameDrive, quotaType, quotaLimit, mode, wmiUserName, wmiPassword);
        }

        public async System.Threading.Tasks.Task SetQuotaLimitOnFolderAsync(string folderPath, string shareNameDrive, SolidCP.Providers.OS.QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            await base.Client.SetQuotaLimitOnFolderAsync(folderPath, shareNameDrive, quotaType, quotaLimit, mode, wmiUserName, wmiPassword);
        }

        public SolidCP.Providers.OS.Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
        {
            return base.Client.GetQuotaOnFolder(folderPath, wmiUserName, wmiPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Quota> GetQuotaOnFolderAsync(string folderPath, string wmiUserName, string wmiPassword)
        {
            return await base.Client.GetQuotaOnFolderAsync(folderPath, wmiUserName, wmiPassword);
        }

        public void DeleteDirectoryRecursive(string rootPath)
        {
            base.Client.DeleteDirectoryRecursive(rootPath);
        }

        public async System.Threading.Tasks.Task DeleteDirectoryRecursiveAsync(string rootPath)
        {
            await base.Client.DeleteDirectoryRecursiveAsync(rootPath);
        }

        public bool CheckFileServicesInstallation()
        {
            return base.Client.CheckFileServicesInstallation();
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync()
        {
            return await base.Client.CheckFileServicesInstallationAsync();
        }

        public bool InstallFsrmService()
        {
            return base.Client.InstallFsrmService();
        }

        public async System.Threading.Tasks.Task<bool> InstallFsrmServiceAsync()
        {
            return await base.Client.InstallFsrmServiceAsync();
        }

        public SolidCP.Providers.OS.FolderGraph GetFolderGraph(string path)
        {
            return base.Client.GetFolderGraph(path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.FolderGraph> GetFolderGraphAsync(string path)
        {
            return await base.Client.GetFolderGraphAsync(path);
        }

        public void ExecuteSyncActions(SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            base.Client.ExecuteSyncActions(actions);
        }

        public async System.Threading.Tasks.Task ExecuteSyncActionsAsync(SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            await base.Client.ExecuteSyncActionsAsync(actions);
        }

        public string[] GetInstalledOdbcDrivers()
        {
            return base.Client.GetInstalledOdbcDrivers();
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledOdbcDriversAsync()
        {
            return await base.Client.GetInstalledOdbcDriversAsync();
        }

        public string[] GetDSNNames()
        {
            return base.Client.GetDSNNames();
        }

        public async System.Threading.Tasks.Task<string[]> GetDSNNamesAsync()
        {
            return await base.Client.GetDSNNamesAsync();
        }

        public SolidCP.Providers.OS.SystemDSN GetDSN(string dsnName)
        {
            return base.Client.GetDSN(dsnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemDSN> GetDSNAsync(string dsnName)
        {
            return await base.Client.GetDSNAsync(dsnName);
        }

        public void CreateDSN(SolidCP.Providers.OS.SystemDSN dsn)
        {
            base.Client.CreateDSN(dsn);
        }

        public async System.Threading.Tasks.Task CreateDSNAsync(SolidCP.Providers.OS.SystemDSN dsn)
        {
            await base.Client.CreateDSNAsync(dsn);
        }

        public void UpdateDSN(SolidCP.Providers.OS.SystemDSN dsn)
        {
            base.Client.UpdateDSN(dsn);
        }

        public async System.Threading.Tasks.Task UpdateDSNAsync(SolidCP.Providers.OS.SystemDSN dsn)
        {
            await base.Client.UpdateDSNAsync(dsn);
        }

        public void DeleteDSN(string dsnName)
        {
            base.Client.DeleteDSN(dsnName);
        }

        public async System.Threading.Tasks.Task DeleteDSNAsync(string dsnName)
        {
            await base.Client.DeleteDSNAsync(dsnName);
        }

        public SolidCP.Providers.OS.UnixFileMode GetUnixPermissions(string path)
        {
            return base.Client.GetUnixPermissions(path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.UnixFileMode> GetUnixPermissionsAsync(string path)
        {
            return await base.Client.GetUnixPermissionsAsync(path);
        }

        public void GrantUnixPermissions(string path, SolidCP.Providers.OS.UnixFileMode mode, bool resetChildPermissions = false)
        {
            base.Client.GrantUnixPermissions(path, mode, resetChildPermissions);
        }

        public async System.Threading.Tasks.Task GrantUnixPermissionsAsync(string path, SolidCP.Providers.OS.UnixFileMode mode, bool resetChildPermissions = false)
        {
            await base.Client.GrantUnixPermissionsAsync(path, mode, resetChildPermissions);
        }

        public SolidCP.Providers.OS.TerminalSession[] GetTerminalServicesSessions()
        {
            return base.Client.GetTerminalServicesSessions();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.TerminalSession[]> GetTerminalServicesSessionsAsync()
        {
            return await base.Client.GetTerminalServicesSessionsAsync();
        }

        public void CloseTerminalServicesSession(int sessionId)
        {
            base.Client.CloseTerminalServicesSession(sessionId);
        }

        public async System.Threading.Tasks.Task CloseTerminalServicesSessionAsync(int sessionId)
        {
            await base.Client.CloseTerminalServicesSessionAsync(sessionId);
        }

        public string[] /*List*/ GetLogNames()
        {
            return base.Client.GetLogNames();
        }

        public async System.Threading.Tasks.Task<string[]> GetLogNamesAsync()
        {
            return await base.Client.GetLogNamesAsync();
        }

        public SolidCP.Providers.OS.SystemLogEntry[] /*List*/ GetLogEntries(string logName)
        {
            return base.Client.GetLogEntries(logName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntry[]> GetLogEntriesAsync(string logName)
        {
            return await base.Client.GetLogEntriesAsync(logName);
        }

        public SolidCP.Providers.OS.SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            return base.Client.GetLogEntriesPaged(logName, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemLogEntriesPaged> GetLogEntriesPagedAsync(string logName, int startRow, int maximumRows)
        {
            return await base.Client.GetLogEntriesPagedAsync(logName, startRow, maximumRows);
        }

        public void ClearLog(string logName)
        {
            base.Client.ClearLog(logName);
        }

        public async System.Threading.Tasks.Task ClearLogAsync(string logName)
        {
            await base.Client.ClearLogAsync(logName);
        }

        public SolidCP.Providers.OS.OSProcess[] GetOSProcesses()
        {
            return base.Client.GetOSProcesses();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSProcess[]> GetOSProcessesAsync()
        {
            return await base.Client.GetOSProcessesAsync();
        }

        public void TerminateOSProcess(int pid)
        {
            base.Client.TerminateOSProcess(pid);
        }

        public async System.Threading.Tasks.Task TerminateOSProcessAsync(int pid)
        {
            await base.Client.TerminateOSProcessAsync(pid);
        }

        public SolidCP.Providers.OS.OSService[] GetOSServices()
        {
            return base.Client.GetOSServices();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.OSService[]> GetOSServicesAsync()
        {
            return await base.Client.GetOSServicesAsync();
        }

        public void ChangeOSServiceStatus(string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            base.Client.ChangeOSServiceStatus(id, status);
        }

        public async System.Threading.Tasks.Task ChangeOSServiceStatusAsync(string id, SolidCP.Providers.OS.OSServiceStatus status)
        {
            await base.Client.ChangeOSServiceStatusAsync(id, status);
        }

        public void RebootSystem()
        {
            base.Client.RebootSystem();
        }

        public async System.Threading.Tasks.Task RebootSystemAsync()
        {
            await base.Client.RebootSystemAsync();
        }

        public SolidCP.Providers.OS.Memory GetMemory()
        {
            return base.Client.GetMemory();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.Memory> GetMemoryAsync()
        {
            return await base.Client.GetMemoryAsync();
        }

        public string ExecuteSystemCommand(string path, string args)
        {
            return base.Client.ExecuteSystemCommand(path, args);
        }

        public async System.Threading.Tasks.Task<string> ExecuteSystemCommandAsync(string path, string args)
        {
            return await base.Client.ExecuteSystemCommandAsync(path, args);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProducts(string tabId, string keywordId)
        {
            return base.Client.GetWPIProducts(tabId, keywordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsAsync(string tabId, string keywordId)
        {
            return await base.Client.GetWPIProductsAsync(tabId, keywordId);
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsFiltered(string filter)
        {
            return base.Client.GetWPIProductsFiltered(filter);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsFilteredAsync(string filter)
        {
            return await base.Client.GetWPIProductsFilteredAsync(filter);
        }

        public SolidCP.Server.WPIProduct GetWPIProductById(string productdId)
        {
            return base.Client.GetWPIProductById(productdId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct> GetWPIProductByIdAsync(string productdId)
        {
            return await base.Client.GetWPIProductByIdAsync(productdId);
        }

        public SolidCP.Server.WPITab[] GetWPITabs()
        {
            return base.Client.GetWPITabs();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPITab[]> GetWPITabsAsync()
        {
            return await base.Client.GetWPITabsAsync();
        }

        public void InitWPIFeeds(string feedUrls)
        {
            base.Client.InitWPIFeeds(feedUrls);
        }

        public async System.Threading.Tasks.Task InitWPIFeedsAsync(string feedUrls)
        {
            await base.Client.InitWPIFeedsAsync(feedUrls);
        }

        public SolidCP.Server.WPIKeyword[] GetWPIKeywords()
        {
            return base.Client.GetWPIKeywords();
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIKeyword[]> GetWPIKeywordsAsync()
        {
            return await base.Client.GetWPIKeywordsAsync();
        }

        public SolidCP.Server.WPIProduct[] GetWPIProductsWithDependencies(string[] products)
        {
            return base.Client.GetWPIProductsWithDependencies(products);
        }

        public async System.Threading.Tasks.Task<SolidCP.Server.WPIProduct[]> GetWPIProductsWithDependenciesAsync(string[] products)
        {
            return await base.Client.GetWPIProductsWithDependenciesAsync(products);
        }

        public void InstallWPIProducts(string[] products)
        {
            base.Client.InstallWPIProducts(products);
        }

        public async System.Threading.Tasks.Task InstallWPIProductsAsync(string[] products)
        {
            await base.Client.InstallWPIProductsAsync(products);
        }

        public void CancelInstallWPIProducts()
        {
            base.Client.CancelInstallWPIProducts();
        }

        public async System.Threading.Tasks.Task CancelInstallWPIProductsAsync()
        {
            await base.Client.CancelInstallWPIProductsAsync();
        }

        public string GetWPIStatus()
        {
            return base.Client.GetWPIStatus();
        }

        public async System.Threading.Tasks.Task<string> GetWPIStatusAsync()
        {
            return await base.Client.GetWPIStatusAsync();
        }

        public string WpiGetLogFileDirectory()
        {
            return base.Client.WpiGetLogFileDirectory();
        }

        public async System.Threading.Tasks.Task<string> WpiGetLogFileDirectoryAsync()
        {
            return await base.Client.WpiGetLogFileDirectoryAsync();
        }

        public SolidCP.Providers.SettingPair[] WpiGetLogsInDirectory(string Path)
        {
            return base.Client.WpiGetLogsInDirectory(Path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SettingPair[]> WpiGetLogsInDirectoryAsync(string Path)
        {
            return await base.Client.WpiGetLogsInDirectoryAsync(Path);
        }

        public bool IsUnix()
        {
            return base.Client.IsUnix();
        }

        public async System.Threading.Tasks.Task<bool> IsUnixAsync()
        {
            return await base.Client.IsUnixAsync();
        }
    }
}
#endif