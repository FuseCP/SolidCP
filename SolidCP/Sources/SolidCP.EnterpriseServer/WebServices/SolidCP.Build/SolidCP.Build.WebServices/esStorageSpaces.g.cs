#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SolidCP.Web.Services;
using SolidCP.Providers.Common;
using SolidCP.Providers.OS;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.StorageSpaces;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IesStorageSpaces
    {
        [WebMethod]
        [OperationContract]
        StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        StorageSpaceLevel GetStorageSpaceLevelById(int id);
        [WebMethod]
        [OperationContract]
        bool CheckIsStorageSpacePathInUse(int serverId, string path, int currentServiceId);
        [WebMethod]
        [OperationContract]
        IntResult SaveStorageSpaceLevel(StorageSpaceLevel level, List<ResourceGroupInfo> groups);
        [WebMethod]
        [OperationContract]
        List<StorageSpaceFolder> GetStorageSpaceFoldersByStorageSpaceId(int id);
        [WebMethod]
        [OperationContract]
        List<ResourceGroupInfo> GetLevelResourceGroups(int id);
        [WebMethod]
        [OperationContract]
        ResultObject SaveLevelResourceGroups(int levelId, List<ResourceGroupInfo> newGroups);
        [WebMethod]
        [OperationContract]
        ResultObject RemoveStorageSpaceLevel(int id);
        [WebMethod]
        [OperationContract]
        StorageSpacesPaged GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<StorageSpace> GetStorageSpacesByLevelId(int levelId);
        [WebMethod]
        [OperationContract]
        StorageSpace GetStorageSpaceById(int id);
        [WebMethod]
        [OperationContract]
        IntResult SaveStorageSpace(StorageSpace space);
        [WebMethod]
        [OperationContract]
        ResultObject RemoveStorageSpace(int id);
        [WebMethod]
        [OperationContract]
        SystemFile[] GetDriveLetters(int serviceId);
        [WebMethod]
        [OperationContract]
        SystemFile[] GetSystemSubFolders(int serviceId, string path);
        [WebMethod]
        [OperationContract]
        void SetStorageSpaceFolderAbeStatus(int storageSpaceFolderId, bool enabled);
        [WebMethod]
        [OperationContract]
        bool GetStorageSpaceFolderAbeStatus(int storageSpaceFolderId);
        [WebMethod]
        [OperationContract]
        void SetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId, bool enabled);
        [WebMethod]
        [OperationContract]
        bool GetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esStorageSpaces : SolidCP.EnterpriseServer.esStorageSpaces, IesStorageSpaces
    {
    }
}
#endif