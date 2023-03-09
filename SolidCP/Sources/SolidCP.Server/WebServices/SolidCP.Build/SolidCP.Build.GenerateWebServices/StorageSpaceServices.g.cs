#if !Client
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
using System.ServiceModel.Activation;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IStorageSpaceServices
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<SystemFile> GetAllDriveLetters();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<SystemFile> GetSystemSubFolders(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ClearStorageSettings(string fullPath, string uncPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateFolder(string fullPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        StorageSpaceFolderShare ShareFolder(string fullPath, string shareName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Quota GetFolderQuota(string fullPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteFolder(string fullPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool RenameFolder(string originalPath, string newName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool FileOrDirectoryExist(string fullPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] Search(string[] searchPaths, string searchText, bool recursive);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetFileBinaryChunk(string path, int offset, int length);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RemoveShare(string fullPath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ShareSetAbeState(string path, bool enabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ShareSetEncyptDataAccess(string path, bool enabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool ShareGetEncyptDataAccessStatus(string path);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool ShareGetAbeState(string path);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StorageSpaceServices : SolidCP.Server.StorageSpaceServices, IStorageSpaceServices
    {
        public new List<SystemFile> GetAllDriveLetters()
        {
            return base.GetAllDriveLetters();
        }

        public new List<SystemFile> GetSystemSubFolders(string path)
        {
            return base.GetSystemSubFolders(path);
        }

        public new void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            base.UpdateStorageSettings(fullPath, qouteSizeBytes, type);
        }

        public new void ClearStorageSettings(string fullPath, string uncPath)
        {
            base.ClearStorageSettings(fullPath, uncPath);
        }

        public new void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            base.UpdateFolderQuota(fullPath, qouteSizeBytes, type);
        }

        public new void CreateFolder(string fullPath)
        {
            base.CreateFolder(fullPath);
        }

        public new StorageSpaceFolderShare ShareFolder(string fullPath, string shareName)
        {
            return base.ShareFolder(fullPath, shareName);
        }

        public new Quota GetFolderQuota(string fullPath)
        {
            return base.GetFolderQuota(fullPath);
        }

        public new void DeleteFolder(string fullPath)
        {
            base.DeleteFolder(fullPath);
        }

        public new bool RenameFolder(string originalPath, string newName)
        {
            return base.RenameFolder(originalPath, newName);
        }

        public new bool FileOrDirectoryExist(string fullPath)
        {
            return base.FileOrDirectoryExist(fullPath);
        }

        public new void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            base.SetFolderNtfsPermissions(fullPath, permissions, isProtected, preserveInheritance);
        }

        public new SystemFile[] Search(string[] searchPaths, string searchText, bool recursive)
        {
            return base.Search(searchPaths, searchText, recursive);
        }

        public new byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return base.GetFileBinaryChunk(path, offset, length);
        }

        public new void RemoveShare(string fullPath)
        {
            base.RemoveShare(fullPath);
        }

        public new void ShareSetAbeState(string path, bool enabled)
        {
            base.ShareSetAbeState(path, enabled);
        }

        public new void ShareSetEncyptDataAccess(string path, bool enabled)
        {
            base.ShareSetEncyptDataAccess(path, enabled);
        }

        public new bool ShareGetEncyptDataAccessStatus(string path)
        {
            return base.ShareGetEncyptDataAccessStatus(path);
        }

        public new bool ShareGetAbeState(string path)
        {
            return base.ShareGetAbeState(path);
        }
    }
}
#endif