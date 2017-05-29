using System;
using System.Collections.Generic;
using System.Linq;
using SolidCP.Providers.Common;
using SolidCP.Providers.OS;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
    public class OrganizationFoldersManager
    {
        public List<StorageSpaceFolder> GetFolders(int itemId, string type)
        {
            var folders = new List<StorageSpaceFolder>();

            ObjectUtils.FillCollectionFromDataReader(folders, DataProvider.GetOrganizationStoragSpacesFolderByType(itemId, type));

            return folders;
        }

        public StorageSpaceFolder GetFolder(int itemId, string type)
        {
            var folders = new List<StorageSpaceFolder>();

            ObjectUtils.FillCollectionFromDataReader(folders, DataProvider.GetOrganizationStoragSpacesFolderByType(itemId, type));

            return folders.FirstOrDefault();
        }

        public StorageSpaceFolder CreateFolder(string organizationId, int itemId, string type, long quotaInBytes, QuotaType qoutaType)
        {
            var storageId = StorageSpacesController.FindBestStorageSpaceService(new DefaultStorageSpaceSelector(), ResourceGroups.HostedOrganizations, quotaInBytes);

            if (!storageId.IsSuccess)
            {
                throw new Exception(storageId.ErrorCodes.First());
            }

            var folder = StorageSpacesController.CreateStorageSpaceFolder(storageId.Value, ResourceGroups.HostedOrganizations, organizationId, type, quotaInBytes, qoutaType);

            if (!folder.IsSuccess)
            {
                throw new Exception(string.Join("---------------------------------------", folder.ErrorCodes));
            }

            DataProvider.AddOrganizationStoragSpacesFolder(itemId, type, folder.Value);

            return StorageSpacesController.GetStorageSpaceFolderById(folder.Value);
        }

        public ResultObject DeleteFolder(int itemId, int folderId)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION_FOLDERS", "DELETE_FOLDER");

            try
            {
                var folder = StorageSpacesController.GetStorageSpaceFolderById(folderId);

                if (folder == null)
                {
                    throw new Exception("Folder not found");
                }

                DataProvider.DeleteOrganizationStoragSpacesFolder(folderId);

                var deletionResult = StorageSpacesController.DeleteStorageSpaceFolder(folder.StorageSpaceId, folder.Id);

                if (deletionResult.IsSuccess == false)
                {
                    throw new Exception(string.Join(";",deletionResult.ErrorCodes));
                }
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error deleting organization folder", exception);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        public ResultObject DeleteFolders(int itemId)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION_FOLDERS", "DELETE_ALL_FOLDERS");

            try
            {
                foreach (var storageSpaceFolderType in Enum.GetValues(typeof(StorageSpaceFolderTypes)))
                {
                    DeleteFolders(itemId, storageSpaceFolderType.ToString());
                }
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error deleting organization folders", exception);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        public ResultObject DeleteFolders(int itemId, string type)
        {
            var result = TaskManager.StartResultTask<ResultObject>("ORGANIZATION_FOLDERS", "DELETE_FOLDERS_BY_TYPE");

            try
            {
                var folders = GetFolders(itemId, type);

                foreach (var folder in folders)
                {
                    DeleteFolder(itemId, folder.Id);
                }
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error deleting organization folders", exception);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }
    }
}