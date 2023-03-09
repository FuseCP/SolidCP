#if Client
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
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
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class OperatingSystemAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IOperatingSystem
    {
        public string CreatePackageFolder(string initialPath)
        {
            return (string)Invoke("SolidCP.Server.OperatingSystem", "CreatePackageFolder", initialPath);
        }

        public async System.Threading.Tasks.Task<string> CreatePackageFolderAsync(string initialPath)
        {
            return await InvokeAsync<string>("SolidCP.Server.OperatingSystem", "CreatePackageFolder", initialPath);
        }

        public bool FileExists(string path)
        {
            return (bool)Invoke("SolidCP.Server.OperatingSystem", "FileExists", path);
        }

        public async System.Threading.Tasks.Task<bool> FileExistsAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "FileExists", path);
        }

        public bool DirectoryExists(string path)
        {
            return (bool)Invoke("SolidCP.Server.OperatingSystem", "DirectoryExists", path);
        }

        public async System.Threading.Tasks.Task<bool> DirectoryExistsAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "DirectoryExists", path);
        }

        public SolidCP.Providers.OS.SystemFile GetFile(string path)
        {
            return (SolidCP.Providers.OS.SystemFile)Invoke("SolidCP.Server.OperatingSystem", "GetFile", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetFileAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile>("SolidCP.Server.OperatingSystem", "GetFile", path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFiles(string path)
        {
            return (SolidCP.Providers.OS.SystemFile[])Invoke("SolidCP.Server.OperatingSystem", "GetFiles", path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesAsync(string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFiles", path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            return (SolidCP.Providers.OS.SystemFile[])Invoke("SolidCP.Server.OperatingSystem", "GetDirectoriesRecursive", rootFolder, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetDirectoriesRecursiveAsync(string rootFolder, string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetDirectoriesRecursive", rootFolder, path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            return (SolidCP.Providers.OS.SystemFile[])Invoke("SolidCP.Server.OperatingSystem", "GetFilesRecursive", rootFolder, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveAsync(string rootFolder, string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFilesRecursive", rootFolder, path);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            return (SolidCP.Providers.OS.SystemFile[])Invoke("SolidCP.Server.OperatingSystem", "GetFilesRecursiveByPattern", rootFolder, path, pattern);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesRecursiveByPatternAsync(string rootFolder, string path, string pattern)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.OperatingSystem", "GetFilesRecursiveByPattern", rootFolder, path, pattern);
        }

        public byte[] GetFileBinaryContent(string path)
        {
            return (byte[])Invoke("SolidCP.Server.OperatingSystem", "GetFileBinaryContent", path);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentAsync(string path)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryContent", path);
        }

        public byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
        {
            return (byte[])Invoke("SolidCP.Server.OperatingSystem", "GetFileBinaryContentUsingEncoding", path, encoding);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentUsingEncodingAsync(string path, string encoding)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryContentUsingEncoding", path, encoding);
        }

        public byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return (byte[])Invoke("SolidCP.Server.OperatingSystem", "GetFileBinaryChunk", path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.OperatingSystem", "GetFileBinaryChunk", path, offset, length);
        }

        public string GetFileTextContent(string path)
        {
            return (string)Invoke("SolidCP.Server.OperatingSystem", "GetFileTextContent", path);
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
            return (string[])Invoke("SolidCP.Server.OperatingSystem", "UnzipFiles", zipFile, destFolder);
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
            return (SolidCP.Providers.OS.UserPermission[])Invoke("SolidCP.Server.OperatingSystem", "GetGroupNtfsPermissions", path, users, usersOU);
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
            return (SolidCP.Providers.OS.Quota)Invoke("SolidCP.Server.OperatingSystem", "GetQuotaOnFolder", folderPath, wmiUserName, wmiPassword);
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
            return (bool)Invoke("SolidCP.Server.OperatingSystem", "CheckFileServicesInstallation");
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "CheckFileServicesInstallation");
        }

        public bool InstallFsrmService()
        {
            return (bool)Invoke("SolidCP.Server.OperatingSystem", "InstallFsrmService");
        }

        public async System.Threading.Tasks.Task<bool> InstallFsrmServiceAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.OperatingSystem", "InstallFsrmService");
        }

        public SolidCP.Providers.OS.FolderGraph GetFolderGraph(string path)
        {
            return (SolidCP.Providers.OS.FolderGraph)Invoke("SolidCP.Server.OperatingSystem", "GetFolderGraph", path);
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
            return (string[])Invoke("SolidCP.Server.OperatingSystem", "GetInstalledOdbcDrivers");
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledOdbcDriversAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.OperatingSystem", "GetInstalledOdbcDrivers");
        }

        public string[] GetDSNNames()
        {
            return (string[])Invoke("SolidCP.Server.OperatingSystem", "GetDSNNames");
        }

        public async System.Threading.Tasks.Task<string[]> GetDSNNamesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.OperatingSystem", "GetDSNNames");
        }

        public SolidCP.Providers.OS.SystemDSN GetDSN(string dsnName)
        {
            return (SolidCP.Providers.OS.SystemDSN)Invoke("SolidCP.Server.OperatingSystem", "GetDSN", dsnName);
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
    }
}
#endif