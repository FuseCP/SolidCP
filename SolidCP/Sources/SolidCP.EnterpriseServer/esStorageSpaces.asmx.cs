using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;
using Microsoft.Web.Services3;
using SolidCP.Providers.Common;
using SolidCP.Providers.OS;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esSystem
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esStorageSpaces : System.Web.Services.WebService
    {
        [WebMethod]
        public StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return StorageSpacesController.GetStorageSpaceLevelsPaged(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public StorageSpaceLevel GetStorageSpaceLevelById(int id)
        {
            return StorageSpacesController.GetStorageSpaceLevelById(id);
        }

        [WebMethod]
        public bool CheckIsStorageSpacePathInUse(int serverId, string path, int currentServiceId)
        {
            return StorageSpacesController.CheckIsStorageSpacePathInUse(serverId, path, currentServiceId);
        }

        [WebMethod]
        public IntResult SaveStorageSpaceLevel(StorageSpaceLevel level, List<ResourceGroupInfo> groups )
        {
            return StorageSpacesController.SaveStorageSpaceLevel(level, groups);
        }

        [WebMethod]
        public List<StorageSpaceFolder> GetStorageSpaceFoldersByStorageSpaceId(int id)
        {
            return StorageSpacesController.GetStorageSpaceFoldersByStorageSpaceId(id);
        }

        [WebMethod]
        public List<ResourceGroupInfo> GetLevelResourceGroups(int id)
        {
            return StorageSpacesController.GetLevelResourceGroups(id);
        }

        [WebMethod]
        public ResultObject SaveLevelResourceGroups(int levelId, List<ResourceGroupInfo> newGroups)
        {
            return StorageSpacesController.SaveLevelResourceGroups(levelId, newGroups);
        }

        [WebMethod]
        public ResultObject RemoveStorageSpaceLevel(int id)
        {
            return StorageSpacesController.RemoveStorageSpaceLevel(id);
        }

        [WebMethod]
        public StorageSpacesPaged GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return StorageSpacesController.GetStorageSpacesPaged(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<StorageSpace> GetStorageSpacesByLevelId(int levelId)
        {
            return StorageSpacesController.GetStorageSpacesByLevelId(levelId);
        }

        [WebMethod]
        public StorageSpace GetStorageSpaceById(int id)
        {
            return StorageSpacesController.GetStorageSpaceById(id);
        }

        [WebMethod]
        public IntResult SaveStorageSpace(StorageSpace space)
        {
            return StorageSpacesController.SaveStorageSpace(space);
        }

        [WebMethod]
        public ResultObject RemoveStorageSpace(int id)
        {
            return StorageSpacesController.RemoveStorageSpace(id);
        }

        [WebMethod]
        public SystemFile[] GetDriveLetters(int serviceId)
        {
            return StorageSpacesController.GetDriveLetters(serviceId);
        }

        [WebMethod]
        public SystemFile[] GetSystemSubFolders(int serviceId, string path)
        {
            return StorageSpacesController.GetSystemSubFolders(serviceId, path);
        }

        [WebMethod]
        public void SetStorageSpaceFolderAbeStatus(int storageSpaceFolderId, bool enabled)
        {
            StorageSpacesController.SetStorageSpaceFolderAbeStatus(storageSpaceFolderId, enabled);
        }

        [WebMethod]
        public bool GetStorageSpaceFolderAbeStatus(int storageSpaceFolderId)
        {
           return StorageSpacesController.GetStorageSpaceFolderAbeStatus(storageSpaceFolderId);
        }

        [WebMethod]
        public void SetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId, bool enabled)
        {
            StorageSpacesController.SetStorageSpaceFolderEncryptDataAccessStatus(storageSpaceFolderId, enabled);
        }

        [WebMethod]
        public bool GetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId)
        {
            return StorageSpacesController.GetStorageSpaceFolderEncryptDataAccessStatus(storageSpaceFolderId);
        }
    }
}
