#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesStorageSpaces", Namespace = "http://tempuri.org/")]
    public interface IesStorageSpaces
    {
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelsPaged", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelsPagedResponse")]
        SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelsPaged", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged> GetStorageSpaceLevelsPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelById", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelByIdResponse")]
        SolidCP.Providers.StorageSpaces.StorageSpaceLevel GetStorageSpaceLevelById(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelById", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceLevelByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceLevel> GetStorageSpaceLevelByIdAsync(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/CheckIsStorageSpacePathInUse", ReplyAction = "http://tempuri.org/IesStorageSpaces/CheckIsStorageSpacePathInUseResponse")]
        bool CheckIsStorageSpacePathInUse(int serverId, string path, int currentServiceId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/CheckIsStorageSpacePathInUse", ReplyAction = "http://tempuri.org/IesStorageSpaces/CheckIsStorageSpacePathInUseResponse")]
        System.Threading.Tasks.Task<bool> CheckIsStorageSpacePathInUseAsync(int serverId, string path, int currentServiceId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SaveStorageSpaceLevel", ReplyAction = "http://tempuri.org/IesStorageSpaces/SaveStorageSpaceLevelResponse")]
        SolidCP.Providers.ResultObjects.IntResult SaveStorageSpaceLevel(SolidCP.Providers.StorageSpaces.StorageSpaceLevel level, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ groups);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SaveStorageSpaceLevel", ReplyAction = "http://tempuri.org/IesStorageSpaces/SaveStorageSpaceLevelResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> SaveStorageSpaceLevelAsync(SolidCP.Providers.StorageSpaces.StorageSpaceLevel level, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ groups);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFoldersByStorageSpaceId", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFoldersByStorageSpaceIdResponse")]
        SolidCP.Providers.StorageSpaces.StorageSpaceFolder[] /*List*/ GetStorageSpaceFoldersByStorageSpaceId(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFoldersByStorageSpaceId", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFoldersByStorageSpaceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceFolder[]> GetStorageSpaceFoldersByStorageSpaceIdAsync(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetLevelResourceGroups", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetLevelResourceGroupsResponse")]
        SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ GetLevelResourceGroups(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetLevelResourceGroups", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetLevelResourceGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo[]> GetLevelResourceGroupsAsync(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SaveLevelResourceGroups", ReplyAction = "http://tempuri.org/IesStorageSpaces/SaveLevelResourceGroupsResponse")]
        SolidCP.Providers.Common.ResultObject SaveLevelResourceGroups(int levelId, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ newGroups);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SaveLevelResourceGroups", ReplyAction = "http://tempuri.org/IesStorageSpaces/SaveLevelResourceGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveLevelResourceGroupsAsync(int levelId, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ newGroups);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpaceLevel", ReplyAction = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpaceLevelResponse")]
        SolidCP.Providers.Common.ResultObject RemoveStorageSpaceLevel(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpaceLevel", ReplyAction = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpaceLevelResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveStorageSpaceLevelAsync(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesPaged", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesPagedResponse")]
        SolidCP.Providers.StorageSpaces.StorageSpacesPaged GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesPaged", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpacesPaged> GetStorageSpacesPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesByLevelId", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesByLevelIdResponse")]
        SolidCP.Providers.StorageSpaces.StorageSpace[] /*List*/ GetStorageSpacesByLevelId(int levelId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesByLevelId", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpacesByLevelIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpace[]> GetStorageSpacesByLevelIdAsync(int levelId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceById", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceByIdResponse")]
        SolidCP.Providers.StorageSpaces.StorageSpace GetStorageSpaceById(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceById", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpace> GetStorageSpaceByIdAsync(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SaveStorageSpace", ReplyAction = "http://tempuri.org/IesStorageSpaces/SaveStorageSpaceResponse")]
        SolidCP.Providers.ResultObjects.IntResult SaveStorageSpace(SolidCP.Providers.StorageSpaces.StorageSpace space);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SaveStorageSpace", ReplyAction = "http://tempuri.org/IesStorageSpaces/SaveStorageSpaceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> SaveStorageSpaceAsync(SolidCP.Providers.StorageSpaces.StorageSpace space);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpace", ReplyAction = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpaceResponse")]
        SolidCP.Providers.Common.ResultObject RemoveStorageSpace(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpace", ReplyAction = "http://tempuri.org/IesStorageSpaces/RemoveStorageSpaceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveStorageSpaceAsync(int id);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetDriveLetters", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetDriveLettersResponse")]
        SolidCP.Providers.OS.SystemFile[] GetDriveLetters(int serviceId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetDriveLetters", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetDriveLettersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetDriveLettersAsync(int serviceId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetSystemSubFolders", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetSystemSubFoldersResponse")]
        SolidCP.Providers.OS.SystemFile[] GetSystemSubFolders(int serviceId, string path);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetSystemSubFolders", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetSystemSubFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetSystemSubFoldersAsync(int serviceId, string path);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderAbeStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderAbeStatusResponse")]
        void SetStorageSpaceFolderAbeStatus(int storageSpaceFolderId, bool enabled);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderAbeStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderAbeStatusResponse")]
        System.Threading.Tasks.Task SetStorageSpaceFolderAbeStatusAsync(int storageSpaceFolderId, bool enabled);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderAbeStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderAbeStatusResponse")]
        bool GetStorageSpaceFolderAbeStatus(int storageSpaceFolderId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderAbeStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderAbeStatusResponse")]
        System.Threading.Tasks.Task<bool> GetStorageSpaceFolderAbeStatusAsync(int storageSpaceFolderId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderEncryptDataAccessStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderEncryptDataAccessStatusResponse")]
        void SetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId, bool enabled);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderEncryptDataAccessStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/SetStorageSpaceFolderEncryptDataAccessStatusResponse")]
        System.Threading.Tasks.Task SetStorageSpaceFolderEncryptDataAccessStatusAsync(int storageSpaceFolderId, bool enabled);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderEncryptDataAccessStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderEncryptDataAccessStatusResponse")]
        bool GetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId);
        [OperationContract(Action = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderEncryptDataAccessStatus", ReplyAction = "http://tempuri.org/IesStorageSpaces/GetStorageSpaceFolderEncryptDataAccessStatusResponse")]
        System.Threading.Tasks.Task<bool> GetStorageSpaceFolderEncryptDataAccessStatusAsync(int storageSpaceFolderId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esStorageSpacesAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesStorageSpaces
    {
        public SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceLevelsPaged", filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged> GetStorageSpaceLevelsPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceLevelsPaged", filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpaceLevel GetStorageSpaceLevelById(int id)
        {
            return Invoke<SolidCP.Providers.StorageSpaces.StorageSpaceLevel>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceLevelById", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceLevel> GetStorageSpaceLevelByIdAsync(int id)
        {
            return await InvokeAsync<SolidCP.Providers.StorageSpaces.StorageSpaceLevel>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceLevelById", id);
        }

        public bool CheckIsStorageSpacePathInUse(int serverId, string path, int currentServiceId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esStorageSpaces", "CheckIsStorageSpacePathInUse", serverId, path, currentServiceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckIsStorageSpacePathInUseAsync(int serverId, string path, int currentServiceId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esStorageSpaces", "CheckIsStorageSpacePathInUse", serverId, path, currentServiceId);
        }

        public SolidCP.Providers.ResultObjects.IntResult SaveStorageSpaceLevel(SolidCP.Providers.StorageSpaces.StorageSpaceLevel level, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ groups)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esStorageSpaces", "SaveStorageSpaceLevel", level, groups.ToList());
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> SaveStorageSpaceLevelAsync(SolidCP.Providers.StorageSpaces.StorageSpaceLevel level, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ groups)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esStorageSpaces", "SaveStorageSpaceLevel", level, groups);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpaceFolder[] /*List*/ GetStorageSpaceFoldersByStorageSpaceId(int id)
        {
            return Invoke<SolidCP.Providers.StorageSpaces.StorageSpaceFolder[], SolidCP.Providers.StorageSpaces.StorageSpaceFolder>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceFoldersByStorageSpaceId", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceFolder[]> GetStorageSpaceFoldersByStorageSpaceIdAsync(int id)
        {
            return await InvokeAsync<SolidCP.Providers.StorageSpaces.StorageSpaceFolder[], SolidCP.Providers.StorageSpaces.StorageSpaceFolder>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceFoldersByStorageSpaceId", id);
        }

        public SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ GetLevelResourceGroups(int id)
        {
            return Invoke<SolidCP.EnterpriseServer.ResourceGroupInfo[], SolidCP.EnterpriseServer.ResourceGroupInfo>("SolidCP.EnterpriseServer.esStorageSpaces", "GetLevelResourceGroups", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo[]> GetLevelResourceGroupsAsync(int id)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ResourceGroupInfo[], SolidCP.EnterpriseServer.ResourceGroupInfo>("SolidCP.EnterpriseServer.esStorageSpaces", "GetLevelResourceGroups", id);
        }

        public SolidCP.Providers.Common.ResultObject SaveLevelResourceGroups(int levelId, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ newGroups)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esStorageSpaces", "SaveLevelResourceGroups", levelId, newGroups.ToList());
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveLevelResourceGroupsAsync(int levelId, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ newGroups)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esStorageSpaces", "SaveLevelResourceGroups", levelId, newGroups);
        }

        public SolidCP.Providers.Common.ResultObject RemoveStorageSpaceLevel(int id)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esStorageSpaces", "RemoveStorageSpaceLevel", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveStorageSpaceLevelAsync(int id)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esStorageSpaces", "RemoveStorageSpaceLevel", id);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpacesPaged GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.StorageSpaces.StorageSpacesPaged>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpacesPaged", filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpacesPaged> GetStorageSpacesPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.StorageSpaces.StorageSpacesPaged>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpacesPaged", filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpace[] /*List*/ GetStorageSpacesByLevelId(int levelId)
        {
            return Invoke<SolidCP.Providers.StorageSpaces.StorageSpace[], SolidCP.Providers.StorageSpaces.StorageSpace>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpacesByLevelId", levelId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpace[]> GetStorageSpacesByLevelIdAsync(int levelId)
        {
            return await InvokeAsync<SolidCP.Providers.StorageSpaces.StorageSpace[], SolidCP.Providers.StorageSpaces.StorageSpace>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpacesByLevelId", levelId);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpace GetStorageSpaceById(int id)
        {
            return Invoke<SolidCP.Providers.StorageSpaces.StorageSpace>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceById", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpace> GetStorageSpaceByIdAsync(int id)
        {
            return await InvokeAsync<SolidCP.Providers.StorageSpaces.StorageSpace>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceById", id);
        }

        public SolidCP.Providers.ResultObjects.IntResult SaveStorageSpace(SolidCP.Providers.StorageSpaces.StorageSpace space)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esStorageSpaces", "SaveStorageSpace", space);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> SaveStorageSpaceAsync(SolidCP.Providers.StorageSpaces.StorageSpace space)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esStorageSpaces", "SaveStorageSpace", space);
        }

        public SolidCP.Providers.Common.ResultObject RemoveStorageSpace(int id)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esStorageSpaces", "RemoveStorageSpace", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveStorageSpaceAsync(int id)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esStorageSpaces", "RemoveStorageSpace", id);
        }

        public SolidCP.Providers.OS.SystemFile[] GetDriveLetters(int serviceId)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esStorageSpaces", "GetDriveLetters", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetDriveLettersAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esStorageSpaces", "GetDriveLetters", serviceId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetSystemSubFolders(int serviceId, string path)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esStorageSpaces", "GetSystemSubFolders", serviceId, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetSystemSubFoldersAsync(int serviceId, string path)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esStorageSpaces", "GetSystemSubFolders", serviceId, path);
        }

        public void SetStorageSpaceFolderAbeStatus(int storageSpaceFolderId, bool enabled)
        {
            Invoke("SolidCP.EnterpriseServer.esStorageSpaces", "SetStorageSpaceFolderAbeStatus", storageSpaceFolderId, enabled);
        }

        public async System.Threading.Tasks.Task SetStorageSpaceFolderAbeStatusAsync(int storageSpaceFolderId, bool enabled)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esStorageSpaces", "SetStorageSpaceFolderAbeStatus", storageSpaceFolderId, enabled);
        }

        public bool GetStorageSpaceFolderAbeStatus(int storageSpaceFolderId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceFolderAbeStatus", storageSpaceFolderId);
        }

        public async System.Threading.Tasks.Task<bool> GetStorageSpaceFolderAbeStatusAsync(int storageSpaceFolderId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceFolderAbeStatus", storageSpaceFolderId);
        }

        public void SetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId, bool enabled)
        {
            Invoke("SolidCP.EnterpriseServer.esStorageSpaces", "SetStorageSpaceFolderEncryptDataAccessStatus", storageSpaceFolderId, enabled);
        }

        public async System.Threading.Tasks.Task SetStorageSpaceFolderEncryptDataAccessStatusAsync(int storageSpaceFolderId, bool enabled)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esStorageSpaces", "SetStorageSpaceFolderEncryptDataAccessStatus", storageSpaceFolderId, enabled);
        }

        public bool GetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceFolderEncryptDataAccessStatus", storageSpaceFolderId);
        }

        public async System.Threading.Tasks.Task<bool> GetStorageSpaceFolderEncryptDataAccessStatusAsync(int storageSpaceFolderId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esStorageSpaces", "GetStorageSpaceFolderEncryptDataAccessStatus", storageSpaceFolderId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esStorageSpaces : SolidCP.Web.Client.ClientBase<IesStorageSpaces, esStorageSpacesAssemblyClient>, IesStorageSpaces
    {
        public SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetStorageSpaceLevelsPaged(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceLevelPaged> GetStorageSpaceLevelsPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetStorageSpaceLevelsPagedAsync(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpaceLevel GetStorageSpaceLevelById(int id)
        {
            return base.Client.GetStorageSpaceLevelById(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceLevel> GetStorageSpaceLevelByIdAsync(int id)
        {
            return await base.Client.GetStorageSpaceLevelByIdAsync(id);
        }

        public bool CheckIsStorageSpacePathInUse(int serverId, string path, int currentServiceId)
        {
            return base.Client.CheckIsStorageSpacePathInUse(serverId, path, currentServiceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckIsStorageSpacePathInUseAsync(int serverId, string path, int currentServiceId)
        {
            return await base.Client.CheckIsStorageSpacePathInUseAsync(serverId, path, currentServiceId);
        }

        public SolidCP.Providers.ResultObjects.IntResult SaveStorageSpaceLevel(SolidCP.Providers.StorageSpaces.StorageSpaceLevel level, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ groups)
        {
            return base.Client.SaveStorageSpaceLevel(level, groups);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> SaveStorageSpaceLevelAsync(SolidCP.Providers.StorageSpaces.StorageSpaceLevel level, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ groups)
        {
            return await base.Client.SaveStorageSpaceLevelAsync(level, groups);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpaceFolder[] /*List*/ GetStorageSpaceFoldersByStorageSpaceId(int id)
        {
            return base.Client.GetStorageSpaceFoldersByStorageSpaceId(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpaceFolder[]> GetStorageSpaceFoldersByStorageSpaceIdAsync(int id)
        {
            return await base.Client.GetStorageSpaceFoldersByStorageSpaceIdAsync(id);
        }

        public SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ GetLevelResourceGroups(int id)
        {
            return base.Client.GetLevelResourceGroups(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ResourceGroupInfo[]> GetLevelResourceGroupsAsync(int id)
        {
            return await base.Client.GetLevelResourceGroupsAsync(id);
        }

        public SolidCP.Providers.Common.ResultObject SaveLevelResourceGroups(int levelId, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ newGroups)
        {
            return base.Client.SaveLevelResourceGroups(levelId, newGroups);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveLevelResourceGroupsAsync(int levelId, SolidCP.EnterpriseServer.ResourceGroupInfo[] /*List*/ newGroups)
        {
            return await base.Client.SaveLevelResourceGroupsAsync(levelId, newGroups);
        }

        public SolidCP.Providers.Common.ResultObject RemoveStorageSpaceLevel(int id)
        {
            return base.Client.RemoveStorageSpaceLevel(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveStorageSpaceLevelAsync(int id)
        {
            return await base.Client.RemoveStorageSpaceLevelAsync(id);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpacesPaged GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetStorageSpacesPaged(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpacesPaged> GetStorageSpacesPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetStorageSpacesPagedAsync(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpace[] /*List*/ GetStorageSpacesByLevelId(int levelId)
        {
            return base.Client.GetStorageSpacesByLevelId(levelId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpace[]> GetStorageSpacesByLevelIdAsync(int levelId)
        {
            return await base.Client.GetStorageSpacesByLevelIdAsync(levelId);
        }

        public SolidCP.Providers.StorageSpaces.StorageSpace GetStorageSpaceById(int id)
        {
            return base.Client.GetStorageSpaceById(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.StorageSpaces.StorageSpace> GetStorageSpaceByIdAsync(int id)
        {
            return await base.Client.GetStorageSpaceByIdAsync(id);
        }

        public SolidCP.Providers.ResultObjects.IntResult SaveStorageSpace(SolidCP.Providers.StorageSpaces.StorageSpace space)
        {
            return base.Client.SaveStorageSpace(space);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> SaveStorageSpaceAsync(SolidCP.Providers.StorageSpaces.StorageSpace space)
        {
            return await base.Client.SaveStorageSpaceAsync(space);
        }

        public SolidCP.Providers.Common.ResultObject RemoveStorageSpace(int id)
        {
            return base.Client.RemoveStorageSpace(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveStorageSpaceAsync(int id)
        {
            return await base.Client.RemoveStorageSpaceAsync(id);
        }

        public SolidCP.Providers.OS.SystemFile[] GetDriveLetters(int serviceId)
        {
            return base.Client.GetDriveLetters(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetDriveLettersAsync(int serviceId)
        {
            return await base.Client.GetDriveLettersAsync(serviceId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetSystemSubFolders(int serviceId, string path)
        {
            return base.Client.GetSystemSubFolders(serviceId, path);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetSystemSubFoldersAsync(int serviceId, string path)
        {
            return await base.Client.GetSystemSubFoldersAsync(serviceId, path);
        }

        public void SetStorageSpaceFolderAbeStatus(int storageSpaceFolderId, bool enabled)
        {
            base.Client.SetStorageSpaceFolderAbeStatus(storageSpaceFolderId, enabled);
        }

        public async System.Threading.Tasks.Task SetStorageSpaceFolderAbeStatusAsync(int storageSpaceFolderId, bool enabled)
        {
            await base.Client.SetStorageSpaceFolderAbeStatusAsync(storageSpaceFolderId, enabled);
        }

        public bool GetStorageSpaceFolderAbeStatus(int storageSpaceFolderId)
        {
            return base.Client.GetStorageSpaceFolderAbeStatus(storageSpaceFolderId);
        }

        public async System.Threading.Tasks.Task<bool> GetStorageSpaceFolderAbeStatusAsync(int storageSpaceFolderId)
        {
            return await base.Client.GetStorageSpaceFolderAbeStatusAsync(storageSpaceFolderId);
        }

        public void SetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId, bool enabled)
        {
            base.Client.SetStorageSpaceFolderEncryptDataAccessStatus(storageSpaceFolderId, enabled);
        }

        public async System.Threading.Tasks.Task SetStorageSpaceFolderEncryptDataAccessStatusAsync(int storageSpaceFolderId, bool enabled)
        {
            await base.Client.SetStorageSpaceFolderEncryptDataAccessStatusAsync(storageSpaceFolderId, enabled);
        }

        public bool GetStorageSpaceFolderEncryptDataAccessStatus(int storageSpaceFolderId)
        {
            return base.Client.GetStorageSpaceFolderEncryptDataAccessStatus(storageSpaceFolderId);
        }

        public async System.Threading.Tasks.Task<bool> GetStorageSpaceFolderEncryptDataAccessStatusAsync(int storageSpaceFolderId)
        {
            return await base.Client.GetStorageSpaceFolderEncryptDataAccessStatusAsync(storageSpaceFolderId);
        }
    }
}
#endif