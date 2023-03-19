#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.OS;
using SolidCP.Providers.StorageSpaces;
using SolidCP.Server.Utils;
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
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class StorageSpaceServices : SolidCP.Server.StorageSpaceServices, IStorageSpaceServices
    {
    }
}
#endif