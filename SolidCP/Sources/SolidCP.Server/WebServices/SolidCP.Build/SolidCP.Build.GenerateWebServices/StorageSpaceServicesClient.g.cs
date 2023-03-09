#if Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.OS;
using SolidCP.Providers.StorageSpaces;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IStorageSpaceServices", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IStorageSpaceServices
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetAllDriveLetters", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetAllDriveLettersResponse")]
        List<SystemFile> GetAllDriveLetters();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetAllDriveLetters", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetAllDriveLettersResponse")]
        System.Threading.Tasks.Task<List<SystemFile>> GetAllDriveLettersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetSystemSubFolders", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetSystemSubFoldersResponse")]
        List<SystemFile> GetSystemSubFolders(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetSystemSubFolders", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetSystemSubFoldersResponse")]
        System.Threading.Tasks.Task<List<SystemFile>> GetSystemSubFoldersAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateStorageSettings", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateStorageSettingsResponse")]
        void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateStorageSettings", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateStorageSettingsResponse")]
        System.Threading.Tasks.Task UpdateStorageSettingsAsync(string fullPath, long qouteSizeBytes, QuotaType type);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ClearStorageSettings", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ClearStorageSettingsResponse")]
        void ClearStorageSettings(string fullPath, string uncPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ClearStorageSettings", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ClearStorageSettingsResponse")]
        System.Threading.Tasks.Task ClearStorageSettingsAsync(string fullPath, string uncPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateFolderQuota", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateFolderQuotaResponse")]
        void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateFolderQuota", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/UpdateFolderQuotaResponse")]
        System.Threading.Tasks.Task UpdateFolderQuotaAsync(string fullPath, long qouteSizeBytes, QuotaType type);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/CreateFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/CreateFolderResponse")]
        void CreateFolder(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/CreateFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/CreateFolderResponse")]
        System.Threading.Tasks.Task CreateFolderAsync(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareFolderResponse")]
        StorageSpaceFolderShare ShareFolder(string fullPath, string shareName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareFolderResponse")]
        System.Threading.Tasks.Task<StorageSpaceFolderShare> ShareFolderAsync(string fullPath, string shareName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFolderQuota", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFolderQuotaResponse")]
        Quota GetFolderQuota(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFolderQuota", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFolderQuotaResponse")]
        System.Threading.Tasks.Task<Quota> GetFolderQuotaAsync(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/DeleteFolderResponse")]
        void DeleteFolder(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/DeleteFolderResponse")]
        System.Threading.Tasks.Task DeleteFolderAsync(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/RenameFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/RenameFolderResponse")]
        bool RenameFolder(string originalPath, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/RenameFolder", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/RenameFolderResponse")]
        System.Threading.Tasks.Task<bool> RenameFolderAsync(string originalPath, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/FileOrDirectoryExist", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/FileOrDirectoryExistResponse")]
        bool FileOrDirectoryExist(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/FileOrDirectoryExist", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/FileOrDirectoryExistResponse")]
        System.Threading.Tasks.Task<bool> FileOrDirectoryExistAsync(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/SetFolderNtfsPermissions", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/SetFolderNtfsPermissionsResponse")]
        void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/SetFolderNtfsPermissions", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/SetFolderNtfsPermissionsResponse")]
        System.Threading.Tasks.Task SetFolderNtfsPermissionsAsync(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/Search", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/SearchResponse")]
        SystemFile[] Search(string[] searchPaths, string searchText, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/Search", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/SearchResponse")]
        System.Threading.Tasks.Task<SystemFile[]> SearchAsync(string[] searchPaths, string searchText, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFileBinaryChunkResponse")]
        byte[] GetFileBinaryChunk(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/GetFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/RemoveShare", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/RemoveShareResponse")]
        void RemoveShare(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/RemoveShare", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/RemoveShareResponse")]
        System.Threading.Tasks.Task RemoveShareAsync(string fullPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetAbeState", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetAbeStateResponse")]
        void ShareSetAbeState(string path, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetAbeState", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetAbeStateResponse")]
        System.Threading.Tasks.Task ShareSetAbeStateAsync(string path, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetEncyptDataAccess", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetEncyptDataAccessResponse")]
        void ShareSetEncyptDataAccess(string path, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetEncyptDataAccess", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareSetEncyptDataAccessResponse")]
        System.Threading.Tasks.Task ShareSetEncyptDataAccessAsync(string path, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetEncyptDataAccessStatus", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetEncyptDataAccessStatusResponse")]
        bool ShareGetEncyptDataAccessStatus(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetEncyptDataAccessStatus", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetEncyptDataAccessStatusResponse")]
        System.Threading.Tasks.Task<bool> ShareGetEncyptDataAccessStatusAsync(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetAbeState", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetAbeStateResponse")]
        bool ShareGetAbeState(string path);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetAbeState", ReplyAction = "http://smbsaas/solidcp/server/IStorageSpaceServices/ShareGetAbeStateResponse")]
        System.Threading.Tasks.Task<bool> ShareGetAbeStateAsync(string path);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class StorageSpaceServicesAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IStorageSpaceServices
    {
        public List<SystemFile> GetAllDriveLetters()
        {
            return (List<SystemFile>)Invoke("SolidCP.Server.StorageSpaceServices", "GetAllDriveLetters");
        }

        public async System.Threading.Tasks.Task<List<SystemFile>> GetAllDriveLettersAsync()
        {
            return await InvokeAsync<List<SystemFile>>("SolidCP.Server.StorageSpaceServices", "GetAllDriveLetters");
        }

        public List<SystemFile> GetSystemSubFolders(string path)
        {
            return (List<SystemFile>)Invoke("SolidCP.Server.StorageSpaceServices", "GetSystemSubFolders", path);
        }

        public async System.Threading.Tasks.Task<List<SystemFile>> GetSystemSubFoldersAsync(string path)
        {
            return await InvokeAsync<List<SystemFile>>("SolidCP.Server.StorageSpaceServices", "GetSystemSubFolders", path);
        }

        public void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "UpdateStorageSettings", fullPath, qouteSizeBytes, type);
        }

        public async System.Threading.Tasks.Task UpdateStorageSettingsAsync(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "UpdateStorageSettings", fullPath, qouteSizeBytes, type);
        }

        public void ClearStorageSettings(string fullPath, string uncPath)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "ClearStorageSettings", fullPath, uncPath);
        }

        public async System.Threading.Tasks.Task ClearStorageSettingsAsync(string fullPath, string uncPath)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "ClearStorageSettings", fullPath, uncPath);
        }

        public void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "UpdateFolderQuota", fullPath, qouteSizeBytes, type);
        }

        public async System.Threading.Tasks.Task UpdateFolderQuotaAsync(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "UpdateFolderQuota", fullPath, qouteSizeBytes, type);
        }

        public void CreateFolder(string fullPath)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "CreateFolder", fullPath);
        }

        public async System.Threading.Tasks.Task CreateFolderAsync(string fullPath)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "CreateFolder", fullPath);
        }

        public StorageSpaceFolderShare ShareFolder(string fullPath, string shareName)
        {
            return (StorageSpaceFolderShare)Invoke("SolidCP.Server.StorageSpaceServices", "ShareFolder", fullPath, shareName);
        }

        public async System.Threading.Tasks.Task<StorageSpaceFolderShare> ShareFolderAsync(string fullPath, string shareName)
        {
            return await InvokeAsync<StorageSpaceFolderShare>("SolidCP.Server.StorageSpaceServices", "ShareFolder", fullPath, shareName);
        }

        public Quota GetFolderQuota(string fullPath)
        {
            return (Quota)Invoke("SolidCP.Server.StorageSpaceServices", "GetFolderQuota", fullPath);
        }

        public async System.Threading.Tasks.Task<Quota> GetFolderQuotaAsync(string fullPath)
        {
            return await InvokeAsync<Quota>("SolidCP.Server.StorageSpaceServices", "GetFolderQuota", fullPath);
        }

        public void DeleteFolder(string fullPath)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "DeleteFolder", fullPath);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string fullPath)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "DeleteFolder", fullPath);
        }

        public bool RenameFolder(string originalPath, string newName)
        {
            return (bool)Invoke("SolidCP.Server.StorageSpaceServices", "RenameFolder", originalPath, newName);
        }

        public async System.Threading.Tasks.Task<bool> RenameFolderAsync(string originalPath, string newName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.StorageSpaceServices", "RenameFolder", originalPath, newName);
        }

        public bool FileOrDirectoryExist(string fullPath)
        {
            return (bool)Invoke("SolidCP.Server.StorageSpaceServices", "FileOrDirectoryExist", fullPath);
        }

        public async System.Threading.Tasks.Task<bool> FileOrDirectoryExistAsync(string fullPath)
        {
            return await InvokeAsync<bool>("SolidCP.Server.StorageSpaceServices", "FileOrDirectoryExist", fullPath);
        }

        public void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "SetFolderNtfsPermissions", fullPath, permissions, isProtected, preserveInheritance);
        }

        public async System.Threading.Tasks.Task SetFolderNtfsPermissionsAsync(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "SetFolderNtfsPermissions", fullPath, permissions, isProtected, preserveInheritance);
        }

        public SystemFile[] Search(string[] searchPaths, string searchText, bool recursive)
        {
            return (SystemFile[])Invoke("SolidCP.Server.StorageSpaceServices", "Search", searchPaths, searchText, recursive);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> SearchAsync(string[] searchPaths, string searchText, bool recursive)
        {
            return await InvokeAsync<SystemFile[]>("SolidCP.Server.StorageSpaceServices", "Search", searchPaths, searchText, recursive);
        }

        public byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return (byte[])Invoke("SolidCP.Server.StorageSpaceServices", "GetFileBinaryChunk", path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.StorageSpaceServices", "GetFileBinaryChunk", path, offset, length);
        }

        public void RemoveShare(string fullPath)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "RemoveShare", fullPath);
        }

        public async System.Threading.Tasks.Task RemoveShareAsync(string fullPath)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "RemoveShare", fullPath);
        }

        public void ShareSetAbeState(string path, bool enabled)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "ShareSetAbeState", path, enabled);
        }

        public async System.Threading.Tasks.Task ShareSetAbeStateAsync(string path, bool enabled)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "ShareSetAbeState", path, enabled);
        }

        public void ShareSetEncyptDataAccess(string path, bool enabled)
        {
            Invoke("SolidCP.Server.StorageSpaceServices", "ShareSetEncyptDataAccess", path, enabled);
        }

        public async System.Threading.Tasks.Task ShareSetEncyptDataAccessAsync(string path, bool enabled)
        {
            await InvokeAsync("SolidCP.Server.StorageSpaceServices", "ShareSetEncyptDataAccess", path, enabled);
        }

        public bool ShareGetEncyptDataAccessStatus(string path)
        {
            return (bool)Invoke("SolidCP.Server.StorageSpaceServices", "ShareGetEncyptDataAccessStatus", path);
        }

        public async System.Threading.Tasks.Task<bool> ShareGetEncyptDataAccessStatusAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.StorageSpaceServices", "ShareGetEncyptDataAccessStatus", path);
        }

        public bool ShareGetAbeState(string path)
        {
            return (bool)Invoke("SolidCP.Server.StorageSpaceServices", "ShareGetAbeState", path);
        }

        public async System.Threading.Tasks.Task<bool> ShareGetAbeStateAsync(string path)
        {
            return await InvokeAsync<bool>("SolidCP.Server.StorageSpaceServices", "ShareGetAbeState", path);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class StorageSpaceServices : SolidCP.Web.Client.ClientBase<IStorageSpaceServices, StorageSpaceServicesAssemblyClient>, IStorageSpaceServices
    {
        public List<SystemFile> GetAllDriveLetters()
        {
            return base.Client.GetAllDriveLetters();
        }

        public async System.Threading.Tasks.Task<List<SystemFile>> GetAllDriveLettersAsync()
        {
            return await base.Client.GetAllDriveLettersAsync();
        }

        public List<SystemFile> GetSystemSubFolders(string path)
        {
            return base.Client.GetSystemSubFolders(path);
        }

        public async System.Threading.Tasks.Task<List<SystemFile>> GetSystemSubFoldersAsync(string path)
        {
            return await base.Client.GetSystemSubFoldersAsync(path);
        }

        public void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            base.Client.UpdateStorageSettings(fullPath, qouteSizeBytes, type);
        }

        public async System.Threading.Tasks.Task UpdateStorageSettingsAsync(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            await base.Client.UpdateStorageSettingsAsync(fullPath, qouteSizeBytes, type);
        }

        public void ClearStorageSettings(string fullPath, string uncPath)
        {
            base.Client.ClearStorageSettings(fullPath, uncPath);
        }

        public async System.Threading.Tasks.Task ClearStorageSettingsAsync(string fullPath, string uncPath)
        {
            await base.Client.ClearStorageSettingsAsync(fullPath, uncPath);
        }

        public void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            base.Client.UpdateFolderQuota(fullPath, qouteSizeBytes, type);
        }

        public async System.Threading.Tasks.Task UpdateFolderQuotaAsync(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            await base.Client.UpdateFolderQuotaAsync(fullPath, qouteSizeBytes, type);
        }

        public void CreateFolder(string fullPath)
        {
            base.Client.CreateFolder(fullPath);
        }

        public async System.Threading.Tasks.Task CreateFolderAsync(string fullPath)
        {
            await base.Client.CreateFolderAsync(fullPath);
        }

        public StorageSpaceFolderShare ShareFolder(string fullPath, string shareName)
        {
            return base.Client.ShareFolder(fullPath, shareName);
        }

        public async System.Threading.Tasks.Task<StorageSpaceFolderShare> ShareFolderAsync(string fullPath, string shareName)
        {
            return await base.Client.ShareFolderAsync(fullPath, shareName);
        }

        public Quota GetFolderQuota(string fullPath)
        {
            return base.Client.GetFolderQuota(fullPath);
        }

        public async System.Threading.Tasks.Task<Quota> GetFolderQuotaAsync(string fullPath)
        {
            return await base.Client.GetFolderQuotaAsync(fullPath);
        }

        public void DeleteFolder(string fullPath)
        {
            base.Client.DeleteFolder(fullPath);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string fullPath)
        {
            await base.Client.DeleteFolderAsync(fullPath);
        }

        public bool RenameFolder(string originalPath, string newName)
        {
            return base.Client.RenameFolder(originalPath, newName);
        }

        public async System.Threading.Tasks.Task<bool> RenameFolderAsync(string originalPath, string newName)
        {
            return await base.Client.RenameFolderAsync(originalPath, newName);
        }

        public bool FileOrDirectoryExist(string fullPath)
        {
            return base.Client.FileOrDirectoryExist(fullPath);
        }

        public async System.Threading.Tasks.Task<bool> FileOrDirectoryExistAsync(string fullPath)
        {
            return await base.Client.FileOrDirectoryExistAsync(fullPath);
        }

        public void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            base.Client.SetFolderNtfsPermissions(fullPath, permissions, isProtected, preserveInheritance);
        }

        public async System.Threading.Tasks.Task SetFolderNtfsPermissionsAsync(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            await base.Client.SetFolderNtfsPermissionsAsync(fullPath, permissions, isProtected, preserveInheritance);
        }

        public SystemFile[] Search(string[] searchPaths, string searchText, bool recursive)
        {
            return base.Client.Search(searchPaths, searchText, recursive);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> SearchAsync(string[] searchPaths, string searchText, bool recursive)
        {
            return await base.Client.SearchAsync(searchPaths, searchText, recursive);
        }

        public byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return base.Client.GetFileBinaryChunk(path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await base.Client.GetFileBinaryChunkAsync(path, offset, length);
        }

        public void RemoveShare(string fullPath)
        {
            base.Client.RemoveShare(fullPath);
        }

        public async System.Threading.Tasks.Task RemoveShareAsync(string fullPath)
        {
            await base.Client.RemoveShareAsync(fullPath);
        }

        public void ShareSetAbeState(string path, bool enabled)
        {
            base.Client.ShareSetAbeState(path, enabled);
        }

        public async System.Threading.Tasks.Task ShareSetAbeStateAsync(string path, bool enabled)
        {
            await base.Client.ShareSetAbeStateAsync(path, enabled);
        }

        public void ShareSetEncyptDataAccess(string path, bool enabled)
        {
            base.Client.ShareSetEncyptDataAccess(path, enabled);
        }

        public async System.Threading.Tasks.Task ShareSetEncyptDataAccessAsync(string path, bool enabled)
        {
            await base.Client.ShareSetEncyptDataAccessAsync(path, enabled);
        }

        public bool ShareGetEncyptDataAccessStatus(string path)
        {
            return base.Client.ShareGetEncyptDataAccessStatus(path);
        }

        public async System.Threading.Tasks.Task<bool> ShareGetEncyptDataAccessStatusAsync(string path)
        {
            return await base.Client.ShareGetEncyptDataAccessStatusAsync(path);
        }

        public bool ShareGetAbeState(string path)
        {
            return base.Client.ShareGetAbeState(path);
        }

        public async System.Threading.Tasks.Task<bool> ShareGetAbeStateAsync(string path)
        {
            return await base.Client.ShareGetAbeStateAsync(path);
        }
    }
}
#endif