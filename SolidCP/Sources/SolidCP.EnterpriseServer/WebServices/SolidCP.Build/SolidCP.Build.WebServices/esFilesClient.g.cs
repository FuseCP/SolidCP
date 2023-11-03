#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesFiles", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesFiles
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileManagerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileManagerSettingsResponse")]
        SolidCP.EnterpriseServer.SystemSettings GetFileManagerSettings();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileManagerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileManagerSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetFileManagerSettingsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetHomeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetHomeFolderResponse")]
        string GetHomeFolder(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetHomeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetHomeFolderResponse")]
        System.Threading.Tasks.Task<string> GetHomeFolderAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilesResponse")]
        SolidCP.Providers.OS.SystemFile[] /*List*/ GetFiles(int packageId, string path, bool includeFiles);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesAsync(int packageId, string path, bool includeFiles);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilesByMask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilesByMaskResponse")]
        SolidCP.Providers.OS.SystemFile[] /*List*/ GetFilesByMask(int packageId, string path, string filesMask);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilesByMask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilesByMaskResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesByMaskAsync(int packageId, string path, string filesMask);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContentResponse")]
        byte[] GetFileBinaryContent(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContentResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileBinaryContentAsync(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContentUsingEncodingResponse")]
        byte[] GetFileBinaryContentUsingEncoding(int packageId, string path, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryContentUsingEncodingResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileBinaryContentUsingEncodingAsync(int packageId, string path, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContentResponse")]
        int UpdateFileBinaryContent(int packageId, string path, byte[] content);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContent", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContentResponse")]
        System.Threading.Tasks.Task<int> UpdateFileBinaryContentAsync(int packageId, string path, byte[] content);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContentUsingEncodingResponse")]
        int UpdateFileBinaryContentUsingEncoding(int packageId, string path, byte[] content, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContentUsingEncoding", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UpdateFileBinaryContentUsingEncodingResponse")]
        System.Threading.Tasks.Task<int> UpdateFileBinaryContentUsingEncodingAsync(int packageId, string path, byte[] content, string encoding);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryChunkResponse")]
        byte[] GetFileBinaryChunk(int packageId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(int packageId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/AppendFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/AppendFileBinaryChunkResponse")]
        int AppendFileBinaryChunk(int packageId, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/AppendFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/AppendFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<int> AppendFileBinaryChunkAsync(int packageId, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/DeleteFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/DeleteFilesResponse")]
        int DeleteFiles(int packageId, string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/DeleteFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/DeleteFilesResponse")]
        System.Threading.Tasks.Task<int> DeleteFilesAsync(int packageId, string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFileResponse")]
        int CreateFile(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFileResponse")]
        System.Threading.Tasks.Task<int> CreateFileAsync(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFolderResponse")]
        int CreateFolder(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateFolderResponse")]
        System.Threading.Tasks.Task<int> CreateFolderAsync(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CopyFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CopyFilesResponse")]
        int CopyFiles(int packageId, string[] files, string destFolder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CopyFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CopyFilesResponse")]
        System.Threading.Tasks.Task<int> CopyFilesAsync(int packageId, string[] files, string destFolder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/MoveFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/MoveFilesResponse")]
        int MoveFiles(int packageId, string[] files, string destFolder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/MoveFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/MoveFilesResponse")]
        System.Threading.Tasks.Task<int> MoveFilesAsync(int packageId, string[] files, string destFolder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/RenameFile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/RenameFileResponse")]
        int RenameFile(int packageId, string oldPath, string newPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/RenameFile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/RenameFileResponse")]
        System.Threading.Tasks.Task<int> RenameFileAsync(int packageId, string oldPath, string newPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UnzipFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UnzipFilesResponse")]
        void UnzipFiles(int packageId, string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UnzipFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/UnzipFilesResponse")]
        System.Threading.Tasks.Task UnzipFilesAsync(int packageId, string[] files);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipFilesResponse")]
        int ZipFiles(int packageId, string[] files, string archivePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipFilesResponse")]
        System.Threading.Tasks.Task<int> ZipFilesAsync(int packageId, string[] files, string archivePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipRemoteFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipRemoteFilesResponse")]
        int ZipRemoteFiles(int packageId, string rootFolder, string[] files, string archivePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipRemoteFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ZipRemoteFilesResponse")]
        System.Threading.Tasks.Task<int> ZipRemoteFilesAsync(int packageId, string rootFolder, string[] files, string archivePath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateAccessDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateAccessDatabaseResponse")]
        int CreateAccessDatabase(int packageId, string dbPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateAccessDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CreateAccessDatabaseResponse")]
        System.Threading.Tasks.Task<int> CreateAccessDatabaseAsync(int packageId, string dbPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CalculatePackageDiskspace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CalculatePackageDiskspaceResponse")]
        int CalculatePackageDiskspace(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CalculatePackageDiskspace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/CalculatePackageDiskspaceResponse")]
        System.Threading.Tasks.Task<int> CalculatePackageDiskspaceAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilePermissionsResponse")]
        SolidCP.Providers.OS.UserPermission[] GetFilePermissions(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFilePermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.UserPermission[]> GetFilePermissionsAsync(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/SetFilePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/SetFilePermissionsResponse")]
        int SetFilePermissions(int packageId, string path, SolidCP.Providers.OS.UserPermission[] users, bool resetChildPermissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/SetFilePermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/SetFilePermissionsResponse")]
        System.Threading.Tasks.Task<int> SetFilePermissionsAsync(int packageId, string path, SolidCP.Providers.OS.UserPermission[] users, bool resetChildPermissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFolderGraph", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFolderGraphResponse")]
        SolidCP.Providers.OS.FolderGraph GetFolderGraph(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFolderGraph", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/GetFolderGraphResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.FolderGraph> GetFolderGraphAsync(int packageId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ExecuteSyncActions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ExecuteSyncActionsResponse")]
        void ExecuteSyncActions(int packageId, SolidCP.Providers.OS.FileSyncAction[] actions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ExecuteSyncActions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ExecuteSyncActionsResponse")]
        System.Threading.Tasks.Task ExecuteSyncActionsAsync(int packageId, SolidCP.Providers.OS.FileSyncAction[] actions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ApplyEnableHardQuotaFeature", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ApplyEnableHardQuotaFeatureResponse")]
        int ApplyEnableHardQuotaFeature(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ApplyEnableHardQuotaFeature", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesFiles/ApplyEnableHardQuotaFeatureResponse")]
        System.Threading.Tasks.Task<int> ApplyEnableHardQuotaFeatureAsync(int packageId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esFilesAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesFiles
    {
        public SolidCP.EnterpriseServer.SystemSettings GetFileManagerSettings()
        {
            return Invoke<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esFiles", "GetFileManagerSettings");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetFileManagerSettingsAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esFiles", "GetFileManagerSettings");
        }

        public string GetHomeFolder(int packageId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esFiles", "GetHomeFolder", packageId);
        }

        public async System.Threading.Tasks.Task<string> GetHomeFolderAsync(int packageId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esFiles", "GetHomeFolder", packageId);
        }

        public SolidCP.Providers.OS.SystemFile[] /*List*/ GetFiles(int packageId, string path, bool includeFiles)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[], SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esFiles", "GetFiles", packageId, path, includeFiles);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesAsync(int packageId, string path, bool includeFiles)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[], SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esFiles", "GetFiles", packageId, path, includeFiles);
        }

        public SolidCP.Providers.OS.SystemFile[] /*List*/ GetFilesByMask(int packageId, string path, string filesMask)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[], SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esFiles", "GetFilesByMask", packageId, path, filesMask);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesByMaskAsync(int packageId, string path, string filesMask)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[], SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esFiles", "GetFilesByMask", packageId, path, filesMask);
        }

        public byte[] GetFileBinaryContent(int packageId, string path)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esFiles", "GetFileBinaryContent", packageId, path);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentAsync(int packageId, string path)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esFiles", "GetFileBinaryContent", packageId, path);
        }

        public byte[] GetFileBinaryContentUsingEncoding(int packageId, string path, string encoding)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esFiles", "GetFileBinaryContentUsingEncoding", packageId, path, encoding);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentUsingEncodingAsync(int packageId, string path, string encoding)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esFiles", "GetFileBinaryContentUsingEncoding", packageId, path, encoding);
        }

        public int UpdateFileBinaryContent(int packageId, string path, byte[] content)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "UpdateFileBinaryContent", packageId, path, content);
        }

        public async System.Threading.Tasks.Task<int> UpdateFileBinaryContentAsync(int packageId, string path, byte[] content)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "UpdateFileBinaryContent", packageId, path, content);
        }

        public int UpdateFileBinaryContentUsingEncoding(int packageId, string path, byte[] content, string encoding)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "UpdateFileBinaryContentUsingEncoding", packageId, path, content, encoding);
        }

        public async System.Threading.Tasks.Task<int> UpdateFileBinaryContentUsingEncodingAsync(int packageId, string path, byte[] content, string encoding)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "UpdateFileBinaryContentUsingEncoding", packageId, path, content, encoding);
        }

        public byte[] GetFileBinaryChunk(int packageId, string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esFiles", "GetFileBinaryChunk", packageId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(int packageId, string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esFiles", "GetFileBinaryChunk", packageId, path, offset, length);
        }

        public int AppendFileBinaryChunk(int packageId, string path, byte[] chunk)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "AppendFileBinaryChunk", packageId, path, chunk);
        }

        public async System.Threading.Tasks.Task<int> AppendFileBinaryChunkAsync(int packageId, string path, byte[] chunk)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "AppendFileBinaryChunk", packageId, path, chunk);
        }

        public int DeleteFiles(int packageId, string[] files)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "DeleteFiles", packageId, files);
        }

        public async System.Threading.Tasks.Task<int> DeleteFilesAsync(int packageId, string[] files)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "DeleteFiles", packageId, files);
        }

        public int CreateFile(int packageId, string path)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "CreateFile", packageId, path);
        }

        public async System.Threading.Tasks.Task<int> CreateFileAsync(int packageId, string path)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "CreateFile", packageId, path);
        }

        public int CreateFolder(int packageId, string path)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "CreateFolder", packageId, path);
        }

        public async System.Threading.Tasks.Task<int> CreateFolderAsync(int packageId, string path)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "CreateFolder", packageId, path);
        }

        public int CopyFiles(int packageId, string[] files, string destFolder)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "CopyFiles", packageId, files, destFolder);
        }

        public async System.Threading.Tasks.Task<int> CopyFilesAsync(int packageId, string[] files, string destFolder)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "CopyFiles", packageId, files, destFolder);
        }

        public int MoveFiles(int packageId, string[] files, string destFolder)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "MoveFiles", packageId, files, destFolder);
        }

        public async System.Threading.Tasks.Task<int> MoveFilesAsync(int packageId, string[] files, string destFolder)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "MoveFiles", packageId, files, destFolder);
        }

        public int RenameFile(int packageId, string oldPath, string newPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "RenameFile", packageId, oldPath, newPath);
        }

        public async System.Threading.Tasks.Task<int> RenameFileAsync(int packageId, string oldPath, string newPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "RenameFile", packageId, oldPath, newPath);
        }

        public void UnzipFiles(int packageId, string[] files)
        {
            Invoke("SolidCP.EnterpriseServer.esFiles", "UnzipFiles", packageId, files);
        }

        public async System.Threading.Tasks.Task UnzipFilesAsync(int packageId, string[] files)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esFiles", "UnzipFiles", packageId, files);
        }

        public int ZipFiles(int packageId, string[] files, string archivePath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "ZipFiles", packageId, files, archivePath);
        }

        public async System.Threading.Tasks.Task<int> ZipFilesAsync(int packageId, string[] files, string archivePath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "ZipFiles", packageId, files, archivePath);
        }

        public int ZipRemoteFiles(int packageId, string rootFolder, string[] files, string archivePath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "ZipRemoteFiles", packageId, rootFolder, files, archivePath);
        }

        public async System.Threading.Tasks.Task<int> ZipRemoteFilesAsync(int packageId, string rootFolder, string[] files, string archivePath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "ZipRemoteFiles", packageId, rootFolder, files, archivePath);
        }

        public int CreateAccessDatabase(int packageId, string dbPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "CreateAccessDatabase", packageId, dbPath);
        }

        public async System.Threading.Tasks.Task<int> CreateAccessDatabaseAsync(int packageId, string dbPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "CreateAccessDatabase", packageId, dbPath);
        }

        public int CalculatePackageDiskspace(int packageId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "CalculatePackageDiskspace", packageId);
        }

        public async System.Threading.Tasks.Task<int> CalculatePackageDiskspaceAsync(int packageId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "CalculatePackageDiskspace", packageId);
        }

        public SolidCP.Providers.OS.UserPermission[] GetFilePermissions(int packageId, string path)
        {
            return Invoke<SolidCP.Providers.OS.UserPermission[]>("SolidCP.EnterpriseServer.esFiles", "GetFilePermissions", packageId, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.UserPermission[]> GetFilePermissionsAsync(int packageId, string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.UserPermission[]>("SolidCP.EnterpriseServer.esFiles", "GetFilePermissions", packageId, path);
        }

        public int SetFilePermissions(int packageId, string path, SolidCP.Providers.OS.UserPermission[] users, bool resetChildPermissions)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "SetFilePermissions", packageId, path, users, resetChildPermissions);
        }

        public async System.Threading.Tasks.Task<int> SetFilePermissionsAsync(int packageId, string path, SolidCP.Providers.OS.UserPermission[] users, bool resetChildPermissions)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "SetFilePermissions", packageId, path, users, resetChildPermissions);
        }

        public SolidCP.Providers.OS.FolderGraph GetFolderGraph(int packageId, string path)
        {
            return Invoke<SolidCP.Providers.OS.FolderGraph>("SolidCP.EnterpriseServer.esFiles", "GetFolderGraph", packageId, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.FolderGraph> GetFolderGraphAsync(int packageId, string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.FolderGraph>("SolidCP.EnterpriseServer.esFiles", "GetFolderGraph", packageId, path);
        }

        public void ExecuteSyncActions(int packageId, SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            Invoke("SolidCP.EnterpriseServer.esFiles", "ExecuteSyncActions", packageId, actions);
        }

        public async System.Threading.Tasks.Task ExecuteSyncActionsAsync(int packageId, SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esFiles", "ExecuteSyncActions", packageId, actions);
        }

        public int ApplyEnableHardQuotaFeature(int packageId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esFiles", "ApplyEnableHardQuotaFeature", packageId);
        }

        public async System.Threading.Tasks.Task<int> ApplyEnableHardQuotaFeatureAsync(int packageId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esFiles", "ApplyEnableHardQuotaFeature", packageId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esFiles : SolidCP.Web.Client.ClientBase<IesFiles, esFilesAssemblyClient>, IesFiles
    {
        public SolidCP.EnterpriseServer.SystemSettings GetFileManagerSettings()
        {
            return base.Client.GetFileManagerSettings();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetFileManagerSettingsAsync()
        {
            return await base.Client.GetFileManagerSettingsAsync();
        }

        public string GetHomeFolder(int packageId)
        {
            return base.Client.GetHomeFolder(packageId);
        }

        public async System.Threading.Tasks.Task<string> GetHomeFolderAsync(int packageId)
        {
            return await base.Client.GetHomeFolderAsync(packageId);
        }

        public SolidCP.Providers.OS.SystemFile[] /*List*/ GetFiles(int packageId, string path, bool includeFiles)
        {
            return base.Client.GetFiles(packageId, path, includeFiles);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesAsync(int packageId, string path, bool includeFiles)
        {
            return await base.Client.GetFilesAsync(packageId, path, includeFiles);
        }

        public SolidCP.Providers.OS.SystemFile[] /*List*/ GetFilesByMask(int packageId, string path, string filesMask)
        {
            return base.Client.GetFilesByMask(packageId, path, filesMask);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFilesByMaskAsync(int packageId, string path, string filesMask)
        {
            return await base.Client.GetFilesByMaskAsync(packageId, path, filesMask);
        }

        public byte[] GetFileBinaryContent(int packageId, string path)
        {
            return base.Client.GetFileBinaryContent(packageId, path);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentAsync(int packageId, string path)
        {
            return await base.Client.GetFileBinaryContentAsync(packageId, path);
        }

        public byte[] GetFileBinaryContentUsingEncoding(int packageId, string path, string encoding)
        {
            return base.Client.GetFileBinaryContentUsingEncoding(packageId, path, encoding);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryContentUsingEncodingAsync(int packageId, string path, string encoding)
        {
            return await base.Client.GetFileBinaryContentUsingEncodingAsync(packageId, path, encoding);
        }

        public int UpdateFileBinaryContent(int packageId, string path, byte[] content)
        {
            return base.Client.UpdateFileBinaryContent(packageId, path, content);
        }

        public async System.Threading.Tasks.Task<int> UpdateFileBinaryContentAsync(int packageId, string path, byte[] content)
        {
            return await base.Client.UpdateFileBinaryContentAsync(packageId, path, content);
        }

        public int UpdateFileBinaryContentUsingEncoding(int packageId, string path, byte[] content, string encoding)
        {
            return base.Client.UpdateFileBinaryContentUsingEncoding(packageId, path, content, encoding);
        }

        public async System.Threading.Tasks.Task<int> UpdateFileBinaryContentUsingEncodingAsync(int packageId, string path, byte[] content, string encoding)
        {
            return await base.Client.UpdateFileBinaryContentUsingEncodingAsync(packageId, path, content, encoding);
        }

        public byte[] GetFileBinaryChunk(int packageId, string path, int offset, int length)
        {
            return base.Client.GetFileBinaryChunk(packageId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(int packageId, string path, int offset, int length)
        {
            return await base.Client.GetFileBinaryChunkAsync(packageId, path, offset, length);
        }

        public int AppendFileBinaryChunk(int packageId, string path, byte[] chunk)
        {
            return base.Client.AppendFileBinaryChunk(packageId, path, chunk);
        }

        public async System.Threading.Tasks.Task<int> AppendFileBinaryChunkAsync(int packageId, string path, byte[] chunk)
        {
            return await base.Client.AppendFileBinaryChunkAsync(packageId, path, chunk);
        }

        public int DeleteFiles(int packageId, string[] files)
        {
            return base.Client.DeleteFiles(packageId, files);
        }

        public async System.Threading.Tasks.Task<int> DeleteFilesAsync(int packageId, string[] files)
        {
            return await base.Client.DeleteFilesAsync(packageId, files);
        }

        public int CreateFile(int packageId, string path)
        {
            return base.Client.CreateFile(packageId, path);
        }

        public async System.Threading.Tasks.Task<int> CreateFileAsync(int packageId, string path)
        {
            return await base.Client.CreateFileAsync(packageId, path);
        }

        public int CreateFolder(int packageId, string path)
        {
            return base.Client.CreateFolder(packageId, path);
        }

        public async System.Threading.Tasks.Task<int> CreateFolderAsync(int packageId, string path)
        {
            return await base.Client.CreateFolderAsync(packageId, path);
        }

        public int CopyFiles(int packageId, string[] files, string destFolder)
        {
            return base.Client.CopyFiles(packageId, files, destFolder);
        }

        public async System.Threading.Tasks.Task<int> CopyFilesAsync(int packageId, string[] files, string destFolder)
        {
            return await base.Client.CopyFilesAsync(packageId, files, destFolder);
        }

        public int MoveFiles(int packageId, string[] files, string destFolder)
        {
            return base.Client.MoveFiles(packageId, files, destFolder);
        }

        public async System.Threading.Tasks.Task<int> MoveFilesAsync(int packageId, string[] files, string destFolder)
        {
            return await base.Client.MoveFilesAsync(packageId, files, destFolder);
        }

        public int RenameFile(int packageId, string oldPath, string newPath)
        {
            return base.Client.RenameFile(packageId, oldPath, newPath);
        }

        public async System.Threading.Tasks.Task<int> RenameFileAsync(int packageId, string oldPath, string newPath)
        {
            return await base.Client.RenameFileAsync(packageId, oldPath, newPath);
        }

        public void UnzipFiles(int packageId, string[] files)
        {
            base.Client.UnzipFiles(packageId, files);
        }

        public async System.Threading.Tasks.Task UnzipFilesAsync(int packageId, string[] files)
        {
            await base.Client.UnzipFilesAsync(packageId, files);
        }

        public int ZipFiles(int packageId, string[] files, string archivePath)
        {
            return base.Client.ZipFiles(packageId, files, archivePath);
        }

        public async System.Threading.Tasks.Task<int> ZipFilesAsync(int packageId, string[] files, string archivePath)
        {
            return await base.Client.ZipFilesAsync(packageId, files, archivePath);
        }

        public int ZipRemoteFiles(int packageId, string rootFolder, string[] files, string archivePath)
        {
            return base.Client.ZipRemoteFiles(packageId, rootFolder, files, archivePath);
        }

        public async System.Threading.Tasks.Task<int> ZipRemoteFilesAsync(int packageId, string rootFolder, string[] files, string archivePath)
        {
            return await base.Client.ZipRemoteFilesAsync(packageId, rootFolder, files, archivePath);
        }

        public int CreateAccessDatabase(int packageId, string dbPath)
        {
            return base.Client.CreateAccessDatabase(packageId, dbPath);
        }

        public async System.Threading.Tasks.Task<int> CreateAccessDatabaseAsync(int packageId, string dbPath)
        {
            return await base.Client.CreateAccessDatabaseAsync(packageId, dbPath);
        }

        public int CalculatePackageDiskspace(int packageId)
        {
            return base.Client.CalculatePackageDiskspace(packageId);
        }

        public async System.Threading.Tasks.Task<int> CalculatePackageDiskspaceAsync(int packageId)
        {
            return await base.Client.CalculatePackageDiskspaceAsync(packageId);
        }

        public SolidCP.Providers.OS.UserPermission[] GetFilePermissions(int packageId, string path)
        {
            return base.Client.GetFilePermissions(packageId, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.UserPermission[]> GetFilePermissionsAsync(int packageId, string path)
        {
            return await base.Client.GetFilePermissionsAsync(packageId, path);
        }

        public int SetFilePermissions(int packageId, string path, SolidCP.Providers.OS.UserPermission[] users, bool resetChildPermissions)
        {
            return base.Client.SetFilePermissions(packageId, path, users, resetChildPermissions);
        }

        public async System.Threading.Tasks.Task<int> SetFilePermissionsAsync(int packageId, string path, SolidCP.Providers.OS.UserPermission[] users, bool resetChildPermissions)
        {
            return await base.Client.SetFilePermissionsAsync(packageId, path, users, resetChildPermissions);
        }

        public SolidCP.Providers.OS.FolderGraph GetFolderGraph(int packageId, string path)
        {
            return base.Client.GetFolderGraph(packageId, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.FolderGraph> GetFolderGraphAsync(int packageId, string path)
        {
            return await base.Client.GetFolderGraphAsync(packageId, path);
        }

        public void ExecuteSyncActions(int packageId, SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            base.Client.ExecuteSyncActions(packageId, actions);
        }

        public async System.Threading.Tasks.Task ExecuteSyncActionsAsync(int packageId, SolidCP.Providers.OS.FileSyncAction[] actions)
        {
            await base.Client.ExecuteSyncActionsAsync(packageId, actions);
        }

        public int ApplyEnableHardQuotaFeature(int packageId)
        {
            return base.Client.ApplyEnableHardQuotaFeature(packageId);
        }

        public async System.Threading.Tasks.Task<int> ApplyEnableHardQuotaFeatureAsync(int packageId)
        {
            return await base.Client.ApplyEnableHardQuotaFeatureAsync(packageId);
        }
    }
}
#endif