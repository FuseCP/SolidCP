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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for StorageSpace
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class StorageSpaceServices
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif