using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SolidCP.Providers.Common;
using SolidCP.Providers.OS;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
    public class StorageSpacesController
    {
        #region Storage Spaces Levels
        public static StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetStorageSpaceLevelsPagedInternal(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        private static StorageSpaceLevelPaged GetStorageSpaceLevelsPagedInternal(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetStorageSpaceLevelsPaged(filterColumn, string.Format("%{0}%", filterValue), sortColumn, startRow, maximumRows);

            var result = new StorageSpaceLevelPaged
            {
                RecordsCount = (int)ds.Tables[0].Rows[0][0]
            };

            var tmpLevels = new List<StorageSpaceLevel>();

            ObjectUtils.FillCollectionFromDataView(tmpLevels, ds.Tables[1].DefaultView);

            result.Levels = tmpLevels.ToArray();

            return result;
        }

        public static StorageSpaceLevel GetStorageSpaceLevelById(int levelId)
        {
            return GetStorageSpaceLevelByIdInternal(levelId);
        }

        private static StorageSpaceLevel GetStorageSpaceLevelByIdInternal(int levelId)
        {
            return ObjectUtils.FillObjectFromDataReader<StorageSpaceLevel>(DataProvider.GetStorageSpaceLevelById(levelId));
        }

        public static IntResult SaveStorageSpaceLevel(StorageSpaceLevel level, List<ResourceGroupInfo> groups)
        {
            return SaveStorageSpaceLevelInternal(level, groups);
        }

        private static IntResult SaveStorageSpaceLevelInternal(StorageSpaceLevel level, List<ResourceGroupInfo> groups)
        {

            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "SAVE_STORAGE_SPACE_LEVEL");

            try
            {
                if (level == null)
                {
                    throw new ArgumentNullException("level");
                }

                if (level.Id > 0)
                {
                    DataProvider.UpdateStorageSpaceLevel(level);

                    TaskManager.Write("Updating Storage Space Level with id = {0}",
                        level.Id.ToString(CultureInfo.InvariantCulture));

                    result.Value = level.Id;
                }
                else
                {
                    result.Value = DataProvider.InsertStorageSpaceLevel(level);
                    TaskManager.Write("Inserting new Storage Space Level, obtained id = {0}",
                        level.Id.ToString(CultureInfo.InvariantCulture));

                    level.Id = result.Value;
                }

                var resultGroup = SaveLevelResourceGroups(result.Value, groups);

                if (!resultGroup.IsSuccess)
                {
                    throw new Exception("Error saving resource groups");
                }
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error saving Storage Space Level", exception);
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

        public static ResultObject RemoveStorageSpaceLevel(int id)
        {
            return RemoveStorageSpaceLevelInternal(id);
        }

        private static ResultObject RemoveStorageSpaceLevelInternal(int id)
        {

            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "REMOVE_STORAGE_SPACE_LEVEL");

            try
            {
                if (id < 1)
                {
                    throw new ArgumentException("Id must be greater than 0");
                }

                DataProvider.RemoveStorageSpaceLevel(id);

            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error removing Storage Space Level", exception);
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


        public static List<ResourceGroupInfo> GetLevelResourceGroups(int levelId)
        {
            return GetLevelResourceGroupsInternal(levelId);
        }

        private static List<ResourceGroupInfo> GetLevelResourceGroupsInternal(int levelId)
        {
            return ObjectUtils.CreateListFromDataReader<ResourceGroupInfo>(DataProvider.GetStorageSpaceLevelResourceGroups(levelId)).ToList();
        }

        public static ResultObject SaveLevelResourceGroups(int levelId, List<ResourceGroupInfo> newGroups)
        {
            return SaveLevelResourceGroupsInternal(levelId, newGroups);
        }

        private static ResultObject SaveLevelResourceGroupsInternal(int levelId, IEnumerable<ResourceGroupInfo> newGroups)
        {
            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "REMOVE_STORAGE_SPACE_LEVEL");

            try
            {
                if (levelId < 1)
                {
                    throw new ArgumentException("Level Id must be greater than 0");
                }

                DataProvider.RemoveStorageSpaceLevelResourceGroups(levelId);

                if (newGroups != null)
                {
                    foreach (var newGroup in newGroups)
                    {
                        DataProvider.AddStorageSpaceLevelResourceGroup(levelId, newGroup.GroupId);
                    }
                }

            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error saving Storage Space Level Resource Groups", exception);
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
        #endregion

        #region Storage Spaces

        public static StorageSpacesPaged GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetStorageSpacePagedInternal(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        private static StorageSpacesPaged GetStorageSpacePagedInternal(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetStorageSpacesPaged(filterColumn, string.Format("%{0}%", filterValue), sortColumn, startRow, maximumRows);

            var result = new StorageSpacesPaged
            {
                RecordsCount = (int)ds.Tables[0].Rows[0][0]
            };

            var spaces = new List<StorageSpace>();

            ObjectUtils.FillCollectionFromDataView(spaces, ds.Tables[1].DefaultView);

            result.Spaces = spaces.ToArray();

            GetStorageSpacesUsage(result.Spaces);

            return result;
        }

        private static void GetStorageSpacesUsage(IEnumerable<StorageSpace> spaces)
        {
            var tasks = new List<Task>();


            foreach (var space in spaces)
            {
                var closureSpace = space;
                var task = new Task(() =>
                {
                    var quota = GetFolderQuota(closureSpace.Path, closureSpace.Id);

                    closureSpace.ActuallyUsedInBytes = ConvertMbToBytes(quota.Usage);
                    closureSpace.DiskFreeSpaceInBytes = quota.DiskFreeSpaceInBytes;
                });

                task.Start();

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static List<StorageSpace> GetStorageSpacesByLevelId(int levelId)
        {
            return GetStorageSpacesByLevelIdInternal(levelId);
        }

        private static List<StorageSpace> GetStorageSpacesByLevelIdInternal(int levelId)
        {
            DataSet ds = DataProvider.GetStorageSpacesByLevelId(levelId);

            var spaces = new List<StorageSpace>();

            ObjectUtils.FillCollectionFromDataView(spaces, ds.Tables[0].DefaultView);

            return spaces;
        }

        public static StorageSpace GetStorageSpaceById(int id)
        {
            return GetStorageSpaceByIdInternal(id);
        }

        private static StorageSpace GetStorageSpaceByIdInternal(int id)
        {
            return ObjectUtils.FillObjectFromDataReader<StorageSpace>(DataProvider.GetStorageSpaceById(id));
        }

        public static bool CheckIsStorageSpacePathInUse(int serverId, string path, int currentServiceId)
        {
            return CheckIsPathInUseInternal(serverId, path, currentServiceId);
        }

        private static bool CheckIsPathInUseInternal(int serverId, string path, int currentStorageSpaceId)
        {
            var storage = ObjectUtils.FillObjectFromDataReader<StorageSpace>(DataProvider.GetStorageSpaceByServiceAndPath(serverId, path));

            return storage != null && storage.Id != currentStorageSpaceId;
        }

        public static IntResult SaveStorageSpace(StorageSpace space)
        {
            return SaveStorageSpaceInternal(space);
        }

        private static IntResult SaveStorageSpaceInternal(StorageSpace space, bool isShared = false)
        {

            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "SAVE_STORAGE_SPACE");

            try
            {
                if (space == null)
                {
                    throw new ArgumentNullException("space");
                }

                var ss = GetStorageSpaceService(space.ServiceId);

                if (isShared)
                {
                    var share = ShareStorageSpaceFolderInternal(space.Id, space.Path, space.Name);

                    space.IsShared = true;
                    space.UncPath = share.UncPath;
                }

                if (space.Id > 0)
                {
                    DataProvider.UpdateStorageSpace(space);

                    TaskManager.Write("Updating Storage Space with id = {0}", space.Id.ToString(CultureInfo.InvariantCulture));

                    result.Value = space.Id;
                }
                else
                {
                    result.Value = DataProvider.InsertStorageSpace(space);
                    TaskManager.Write("Inserting new Storage Space, obtained id = {0}", space.Id.ToString(CultureInfo.InvariantCulture));

                    space.Id = result.Value;
                }

                ss.UpdateStorageSettings(space.Path, space.FsrmQuotaSizeBytes, space.FsrmQuotaType);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error saving Storage Space", exception);
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

        public static ResultObject RemoveStorageSpace(int id)
        {
            return RemoveStorageSpaceInternal(id);
        }

        private static ResultObject RemoveStorageSpaceInternal(int id)
        {

            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "REMOVE_STORAGE_SPACE");

            try
            {
                if (id < 1)
                {
                    throw new ArgumentException("Id must be greater than 0");
                }

                var storage = GetStorageSpaceByIdInternal(id);

                var ss = GetStorageSpaceService(storage.ServiceId);

                ss.ClearStorageSettings(storage.Path, storage.UncPath);

                DataProvider.RemoveStorageSpace(id);

            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error removing Storage Space", exception);
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

        public static IntResult FindBestStorageSpaceService(IStorageSpaceSelector selector, string groupName, long quotaSizeBytes)
        {
            return FindBestStorageSpaceServiceInternal(selector, groupName, quotaSizeBytes);
        }

        private static IntResult FindBestStorageSpaceServiceInternal(IStorageSpaceSelector selector, string groupName, long quotaSizeBytes)
        {
            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "FIND_BEST_STORAGE_SPACE_SERVICE");

            try
            {
                var bestStorage = selector.FindBest(groupName, quotaSizeBytes);

                result.Value = bestStorage.Id;
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error finding best Storage Space", exception);
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

        #endregion

        #region Storage Space Folders

        public static IntResult CreateStorageSpaceFolder(int storageSpaceId, string groupName, string organizationId, string folderName, long quotaInBytes, QuotaType quotaType)
        {
            return CreateStorageSpaceFolderInternal(storageSpaceId, groupName, organizationId, folderName, quotaInBytes, quotaType);
        }

        private static IntResult CreateStorageSpaceFolderInternal(int storageSpaceId, string groupName, string organizationId, string folderName, long quotaInBytes, QuotaType quotaType)
        {
            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "CREATE_STORAGE_SPACE_FOLDER");

            try
            {
                var storageSpace = StorageSpacesController.GetStorageSpaceById(storageSpaceId);

                if (storageSpace == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }

                var ss = GetStorageSpaceService(storageSpace.ServiceId);

                var fullPath = CreateFilePath(storageSpace.Path, organizationId, groupName, folderName);

                ss.CreateFolder(fullPath);

                var shareName = GenerateShareName(organizationId, folderName);

                var share = ShareStorageSpaceFolderInternal(storageSpace.Id, fullPath, shareName);

                ss.UpdateFolderQuota(fullPath, quotaInBytes, quotaType);

                result.Value = DataProvider.CreateStorageSpaceFolder(folderName, storageSpace.Id, fullPath, share.UncPath, true, quotaType, quotaInBytes);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error creating Storage Space folder", exception);

                if (result.Value > 0)
                {
                    DataProvider.RemoveStorageSpaceFolder(result.Value);
                }
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

        private static string GenerateShareName(string organizationId, string folderName)
        {
            var hiddenfolderName = folderName + "$";
            return string.Join("-", organizationId, hiddenfolderName);
        }

        public static ResultObject UpdateStorageSpaceFolder(int storageSpaceId, int storageSpaceFolderId, string organizationId, string groupName, string folderName, string uncPath, long quotaInBytes, QuotaType quotaType)
        {
            return UpdateStorageSpaceFolderInternal(storageSpaceId, storageSpaceFolderId, organizationId, groupName, folderName, uncPath, quotaInBytes, quotaType);
        }

        private static ResultObject UpdateStorageSpaceFolderInternal(int storageSpaceId, int storageSpaceFolderId, string organizationId, string groupName, string folderName,string uncPath, long quotaInBytes, QuotaType quotaType)
        {
            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "UPDATE_STORAGE_SPACE_FOLDER");

            try
            {
                var storageSpace = StorageSpacesController.GetStorageSpaceById(storageSpaceId);

                if (storageSpace == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }

                var ss = StorageSpacesController.GetStorageSpaceService(storageSpace.ServiceId);

                var fullPath = CreateFilePath(storageSpace.Path, organizationId, groupName, folderName);

                if (quotaInBytes > 0)
                {
                    ss.UpdateFolderQuota(fullPath, quotaInBytes, quotaType);
                }

                DataProvider.UpdateStorageSpaceFolder(storageSpaceFolderId, folderName, storageSpace.Id, fullPath, uncPath, false, quotaType, quotaInBytes);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error removing Storage Space", exception);
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

        public static StorageSpaceFolderShare ShareStorageSpaceFolder(int storageSpaceId, string fullPath, string shareName)
        {
            return ShareStorageSpaceFolderInternal(storageSpaceId, fullPath, shareName);
        }

        private static StorageSpaceFolderShare ShareStorageSpaceFolderInternal(int storageSpaceId, string fullPath, string shareName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "SHARE_STORAGE_SPACE_FOLDER");

            try
            {
                var storageSpace = StorageSpacesController.GetStorageSpaceById(storageSpaceId);

                if (storageSpace == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }

                var ss = StorageSpacesController.GetStorageSpaceService(storageSpace.ServiceId);

                var share = ss.ShareFolder(fullPath, shareName);

                if (share == null)
                {
                    throw new Exception("Error sharing folder");
                }

                return share;
            }
            catch (Exception exception)
            {
                result.AddError("Error sharing Storage Space folder", exception);

               throw TaskManager.WriteError(exception);
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
        }

        private static string CreateFilePath(string path, string organizationId, string groupName, string folderName)
        {
            // 06.09.2015 roland.breitschaft@x-company.de
            // Problem: The folder will not created in the Storage-Space
            // Fix: Remove groupName from Path-Creation
            // return Path.Combine(path, groupName, organizationId, folderName);
            return Path.Combine(path, organizationId, folderName);
        }

        public static ResultObject DeleteStorageSpaceFolder(int storageSpaceId, int storageSpaceFolderId)
        {
            return DeleteStorageSpaceFolderInternal(storageSpaceId, storageSpaceFolderId);
        }

        private static ResultObject DeleteStorageSpaceFolderInternal(int storageSpaceId, int storageSpaceFolderId)
        {
            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "DELETE_STORAGE_SPACE_FOLDER");

            try
            {
                if (storageSpaceId < 0)
                {
                    throw new ArgumentException("Storage Space iD must be greater than 0");
                }

                var storage = GetStorageSpaceById(storageSpaceId);

                if (storage == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }

                var storageFolder = GetStorageSpaceFolderById(storageSpaceFolderId);

                if (storageFolder == null)
                {
                    throw new Exception(string.Format("Storage Space folder with id={0} not found", storageSpaceFolderId));
                }

                var ss = GetStorageSpaceService(storage.ServiceId);

                if (storageFolder.IsShared)
                {
                    ss.RemoveShare(storageFolder.Path);
                }

                ss.DeleteFolder(storageFolder.Path);

                DataProvider.RemoveStorageSpaceFolder(storageSpaceFolderId);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error removing Storage Space folder", exception);

                if (result.Value > 0)
                {
                    DataProvider.RemoveStorageSpaceFolder(result.Value);
                }
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

        public static ResultObject SetStorageSpaceFolderQuota(int storageSpaceId, int storageSpaceFolderId, long quotaInBytes, QuotaType quotaType)
        {
            return SetStorageSpaceFolderQuotaInternal(storageSpaceId, storageSpaceFolderId, quotaInBytes, quotaType);
        }

        private static ResultObject SetStorageSpaceFolderQuotaInternal(int storageSpaceId, int storageSpaceFolderId, long quotaInBytes, QuotaType quotaType)
        {
            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "SET_STORAGE_SPACE_FOLDER_QUOTA");

            try
            {
                if (storageSpaceId < 0)
                {
                    throw new ArgumentException("Storage Space iD must be greater than 0");
                }

                var storage = GetStorageSpaceById(storageSpaceId);

                if (storage == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }

                var storageFolder = GetStorageSpaceFolderById(storageSpaceFolderId);

                if (storageFolder == null)
                {
                    throw new Exception(string.Format("Storage Space folder with id={0} not found", storageSpaceFolderId));
                }

                SetFolderQuota(storageSpaceId, storageFolder.Path, quotaInBytes, quotaType);

                DataProvider.UpdateStorageSpaceFolder(storageSpaceFolderId, storageFolder.Name, storageSpaceId, storageFolder.Path, storageFolder.UncPath, storageFolder.IsShared, quotaType, quotaInBytes);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error removing Storage Space folder", exception);

                if (result.Value > 0)
                {
                    DataProvider.RemoveStorageSpaceFolder(result.Value);
                }
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

        public static List<StorageSpaceFolder> GetStorageSpaceFoldersByStorageSpaceId(int storageSpaceId)
        {
            return GetStorageSpaceFoldersByStorageSpaceIdInternal(storageSpaceId);
        }

        private static List<StorageSpaceFolder> GetStorageSpaceFoldersByStorageSpaceIdInternal(int storageSpaceId)
        {
            var folders = ObjectUtils.CreateListFromDataReader<StorageSpaceFolder>(DataProvider.GetStorageSpaceFoldersByStorageSpaceId(storageSpaceId));

            return folders;
        }

        public static ResultObject SetFolderNtfsPermissions(int storageSpaceId, string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            return SetFolderNtfsPermissionsInternal(storageSpaceId, fullPath, permissions, isProtected, preserveInheritance);
        }

        private static ResultObject SetFolderNtfsPermissionsInternal(int storageSpaceId, string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            var result = TaskManager.StartResultTask<IntResult>("STORAGE_SPACES", "SET_NTFS_PERMISSIONS_ON_FOLDER");

            try
            {
                if (storageSpaceId < 0)
                {
                    throw new ArgumentException("Storage Space iD must be greater than 0");
                }

                var storage = GetStorageSpaceById(storageSpaceId);

                if (storage == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }


                var ss = GetStorageSpaceService(storage.ServiceId);

                ss.SetFolderNtfsPermissions(fullPath, permissions, isProtected, preserveInheritance);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error setting NTFS permissions on Storage Space folder", exception);

                if (result.Value > 0)
                {
                    DataProvider.RemoveStorageSpaceFolder(result.Value);
                }
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

        public static BoolResult StorageSpaceFileOrFolderExist(int storageSpaceId, string fullPath)
        {
            return StorageSpaceFileOrFolderExistInternal(storageSpaceId, fullPath);
        }

        private static BoolResult StorageSpaceFileOrFolderExistInternal(int storageSpaceId, string fullPath)
        {
            var result = TaskManager.StartResultTask<BoolResult>("STORAGE_SPACES", "FOLDER_OR_FOLDER_EXIST");

            try
            {
                if (storageSpaceId < 0)
                {
                    throw new ArgumentException("Storage Space iD must be greater than 0");
                }

                var storage = GetStorageSpaceById(storageSpaceId);

                if (storage == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }

                var ss = GetStorageSpaceService(storage.ServiceId);

                result.Value = ss.FileOrDirectoryExist(fullPath);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error during folder exist check", exception);
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

        public static ResultObject RenameStorageSpaceFolder(int storageSpaceId, int folderId, string organizationId, string group, string fullPath, string newName)
        {
            return RenameFolderInternal(storageSpaceId, folderId, organizationId, group, fullPath, newName);
        }

        private static ResultObject RenameFolderInternal(int storageSpaceId, int folderId, string organizationId, string group, string fullPath, string newName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("STORAGE_SPACES", "RENAME_FOLDER");

            try
            {
                if (storageSpaceId < 0)
                {
                    throw new ArgumentException("Storage Space iD must be greater than 0");
                }

                var storage = GetStorageSpaceById(storageSpaceId);

                if (storage == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
                }

                var ss = GetStorageSpaceService(storage.ServiceId);

                ss.RemoveShare(fullPath);

                ss.RenameFolder(fullPath, newName);

                var newPath = Path.Combine(Directory.GetParent(fullPath).ToString(), newName);

                var shareName = GenerateShareName(organizationId, newName);

                var share = ShareStorageSpaceFolderInternal(storageSpaceId, newPath, shareName);

                var folder = GetStorageSpaceFolderById(folderId);

                StorageSpacesController.UpdateStorageSpaceFolder(storageSpaceId, folderId, organizationId, group, newName, share.UncPath, folder.FsrmQuotaSizeBytes, folder.FsrmQuotaType);

            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
                result.AddError("Error during folder rename", exception);
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

        public static StorageSpaceFolder GetStorageSpaceFolderById(int id)
        {
            return GetStorageSpaceFolderByIdInternal(id);
        }

        private static StorageSpaceFolder GetStorageSpaceFolderByIdInternal(int id)
        {
            return ObjectUtils.FillObjectFromDataReader<StorageSpaceFolder>(DataProvider.GetStorageSpaceFolderById(id));
        }

        public static Quota GetFolderQuota(string fullPath, int storageSpaceid)
        {
            var space = GetStorageSpaceById(storageSpaceid);

            var ss = GetStorageSpaceService(space.ServiceId);

            return ss.GetFolderQuota(fullPath);
        }

        public static void SetFolderQuota(int storageSpaceid, string fullPath, long quotaSizeBytes, QuotaType type)
        {
            var space = GetStorageSpaceById(storageSpaceid);

            var ss = GetStorageSpaceService(space.ServiceId);

            ss.UpdateFolderQuota(fullPath, quotaSizeBytes,type);
        }

        public static List<Task<IEnumerable<SystemFile>>> SearchInStorageSpaceFolders(IEnumerable<StorageSpaceFolderSearchRequest> requests)
        {
            var tasks = new List<Task<IEnumerable<SystemFile>>>();

            foreach (var request in requests)
            {
                StorageSpaceFolderSearchRequest closure = request;

                var task = new Task<IEnumerable<SystemFile>>(() =>
                {
                    return SearchInStorageSpaceFolderInternal(closure.StorageSpaceId, closure.StorageSpaceFolderId, closure.SearchPath, closure.SearchValue);
                });

                task.Start();

                tasks.Add(task);
            }

            return tasks;
        }

        public static List<SystemFile> SearchInStorageSpaceFolder(int storageSpaceId, int storageSpaceFolderId, string searchPath, string searchValue)
        {
            return SearchInStorageSpaceFolderInternal(storageSpaceId, storageSpaceFolderId, searchPath, searchValue);
        }

        private static List<SystemFile> SearchInStorageSpaceFolderInternal(int storageSpaceId, int storageSpaceFolderId, string searchPath, string searchValue)
        {
            var storageSpace = StorageSpacesController.GetStorageSpaceById(storageSpaceId);

            if (storageSpace == null)
            {
                throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
            }

            var storageSpaceFolder = StorageSpacesController.GetStorageSpaceFolderById(storageSpaceFolderId);

            if (storageSpaceFolder == null)
            {
                throw new Exception(string.Format("Storage space folder with id={0} not found", storageSpaceFolderId));
            }

            var origSearchPath = searchPath;
            var ss = GetStorageSpaceService(storageSpace.ServiceId);

            int index = searchPath.IndexOf(Path.DirectorySeparatorChar);


            if (index >= 0)
            {
                searchPath = searchPath.Substring(index + 1);

                searchPath = Path.Combine(storageSpaceFolder.Path, searchPath);
            }
            else
            {
                searchPath = storageSpaceFolder.Path;
            }

            var searchResults = ss.Search(new[] {searchPath}, searchValue, true).ToList();

            foreach (var result in searchResults)
            {
                result.RelativeUrl = result.FullName.Replace(storageSpaceFolder.Path, Path.GetFileName(storageSpaceFolder.Path));
            }

            return searchResults;
        }



        public static void SetStorageSpaceFolderAbeStatus(int storageSpaceFolderId, bool enabled)
        {
            SetStorageSpaceFolderAbeStatusInternal(storageSpaceFolderId, enabled);
        }

        private static void SetStorageSpaceFolderAbeStatusInternal(int storageSpaceFolderId, bool enabled)
        {
            TaskManager.StartTask("STORAGE_SPACES", "SET_ABE_STATUS");

            try
            {
                var folder = GetStorageSpaceFolderById(storageSpaceFolderId);

                if (folder == null)
                {
                    throw new Exception(string.Format("Storage space folder with id={0} not found", storageSpaceFolderId));
                }

                var storageSpace = StorageSpacesController.GetStorageSpaceById(folder.StorageSpaceId);

                if (storageSpace == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", folder.StorageSpaceId));
                }

                var ss = GetStorageSpaceService(storageSpace.ServiceId);

                ss.ShareSetAbeState(folder.Path, enabled);
            }
            catch (Exception exception)
            {
                TaskManager.WriteError(exception);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static bool GetStorageSpaceFolderAbeStatus(int storageSpaceFolderId)
        {
            return GetStorageSpaceFolderAbeStatusInternal(storageSpaceFolderId);
        }

        private static bool GetStorageSpaceFolderAbeStatusInternal(int storageSpaceFolderId)
        {

            try
            {
                var folder = GetStorageSpaceFolderById(storageSpaceFolderId);

                if (folder == null)
                {
                    throw new Exception(string.Format("Storage space folder with id={0} not found", storageSpaceFolderId));
                }

                var storageSpace = StorageSpacesController.GetStorageSpaceById(folder.StorageSpaceId);

                if (storageSpace == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", folder.StorageSpaceId));
                }

                var ss = GetStorageSpaceService(storageSpace.ServiceId);

                return ss.ShareGetAbeState(folder.Path);
            }
            catch (Exception exception)
            {
                throw TaskManager.WriteError(exception);
            }
        }

        public static void SetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId, bool enabled)
        {
            SetStorageSpaceFolderEncryptDataAccessStatusInternal(storageSpaceFolderId, enabled);
        }

        private static void SetStorageSpaceFolderEncryptDataAccessStatusInternal(int storageSpaceFolderId, bool enabled)
        {
            TaskManager.StartTask("STORAGE_SPACES", "SET_ENCRYPT_DATA_ACCESS_STATUS");

            try
            {
                var folder = GetStorageSpaceFolderById(storageSpaceFolderId);

                if (folder == null)
                {
                    throw new Exception(string.Format("Storage space folder with id={0} not found", storageSpaceFolderId));
                }

                var storageSpace = StorageSpacesController.GetStorageSpaceById(folder.StorageSpaceId);

                if (storageSpace == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", folder.StorageSpaceId));
                }

                var ss = GetStorageSpaceService(storageSpace.ServiceId);

                ss.ShareSetEncyptDataAccess(folder.Path, enabled);
            }
            catch (Exception exception)
            {
                throw TaskManager.WriteError(exception);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static bool GetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId)
        {
            return GetStorageSpaceFolderEncryptDataAccessStatusInternal(storageSpaceFolderId);
        }

        private static bool GetStorageSpaceFolderEncryptDataAccessStatusInternal(int storageSpaceFolderId)
        {
            try
            {
                var folder = GetStorageSpaceFolderById(storageSpaceFolderId);

                if (folder == null)
                {
                    throw new Exception(string.Format("Storage space folder with id={0} not found", storageSpaceFolderId));
                }

                var storageSpace = StorageSpacesController.GetStorageSpaceById(folder.StorageSpaceId);

                if (storageSpace == null)
                {
                    throw new Exception(string.Format("Storage space with id={0} not found", folder.StorageSpaceId));
                }

                var ss = GetStorageSpaceService(storageSpace.ServiceId);

                return ss.ShareGetEncyptDataAccessStatus(folder.Path);
            }
            catch (Exception exception)
            {
                throw TaskManager.WriteError(exception);
            }
        }

        #endregion

        #region Storage Spaces TreeView

        public static SystemFile[] GetDriveLetters(int serviceId)
        {
            return GetDriveLettersInternal(serviceId);
        }

        private static SystemFile[] GetDriveLettersInternal(int serviceId)
        {
            
            var ss = GetStorageSpaceService(serviceId);

            return ss.GetAllDriveLetters();
        }

        public static SystemFile[] GetSystemSubFolders(int serviceId, string path)
        {
            return GetSystemSubFoldersInternal(serviceId, path);
        }

        private static SystemFile[] GetSystemSubFoldersInternal(int serviceId, string path)
        {
            var ss = GetStorageSpaceService(serviceId);

            return ss.GetSystemSubFolders(path);
        }

        #endregion

        public static StorageSpaceServices GetStorageSpaceService(int serviceId)
        {
            var ss = new StorageSpaceServices();
            ServiceProviderProxy.Init(ss, serviceId);

            return ss;
        }

        public static long GetFsrmQuotaInBytes(QuotaValueInfo quotaInfo)
        {
            if (quotaInfo.QuotaAllocatedValue == -1)
            {
                return -1;
            }

            if (quotaInfo.QuotaDescription.ToLower().Contains("gb"))
            {
                return quotaInfo.QuotaAllocatedValue*1024*1024*1024;
            }

            if (quotaInfo.QuotaDescription.ToLower().Contains("mb"))
            {
                return quotaInfo.QuotaAllocatedValue * 1024 * 1024;
            }

            return quotaInfo.QuotaAllocatedValue * 1024 ;
        }

        private static long ConvertMbToBytes(long mBytes)
        {
            return mBytes*1024*1024;
        }

        public static byte[] GetFileBinaryChunk(int storageSpaceId, string path, int offset, int length)
        {
            var storageSpace = StorageSpacesController.GetStorageSpaceById(storageSpaceId);

            if (storageSpace == null)
            {
                throw new Exception(string.Format("Storage space with id={0} not found", storageSpaceId));
            }

            var ss = GetStorageSpaceService(storageSpace.ServiceId);

            return ss.GetFileBinaryChunk(path, offset, length);
        }

        public static string GetParentUnc(string uncPath)
        {
            var uri = new Uri(uncPath);

            if (uri.Segments.Length == 2)
            {
                return string.Format("\\\\{0}", uri.Host);
            }

            return Directory.GetParent(uncPath).ToString();
        }
    }
}