using System.Collections.Generic;
using System.IO;
using SolidCP.Providers.OS;

namespace SolidCP.Providers.StorageSpaces
{
    public interface IStorageSpace
    {
        List<SystemFile> GetAllDriveLetters();
        List<SystemFile> GetSystemSubFolders(string path);
        void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type);
        void ClearStorageSettings(string fullPath, string uncPath);
        void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type);
        Quota GetFolderQuota(string fullPath);
        void CreateFolder(string fullPath);
        void DeleteFolder(string fullPath);
        bool RenameFolder(string originalPath, string newName);
        bool FileOrDirectoryExist(string fullPath);
        void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance);
        StorageSpaceFolderShare ShareFolder(string fullPath, string shareName);
        SystemFile[] Search(string[] searchPaths, string searchText, bool recursive);
        byte[] GetFileBinaryChunk(string path, int offset, int length);
        void RemoveShare(string fullPath);
        void ShareSetAbeState(string path, bool enabled);
        bool ShareGetAbeState(string path);
        bool ShareGetEncyptDataAccessStatus(string path);
        void ShareSetEncyptDataAccess(string path, bool enabled);
    }
}