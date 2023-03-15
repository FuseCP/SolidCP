#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "ISharePointServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface ISharePointServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/ExtendVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/ExtendVirtualServerResponse")]
        void ExtendVirtualServer(SolidCP.Providers.SharePoint.SharePointSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/ExtendVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/ExtendVirtualServerResponse")]
        System.Threading.Tasks.Task ExtendVirtualServerAsync(SolidCP.Providers.SharePoint.SharePointSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UnextendVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UnextendVirtualServerResponse")]
        void UnextendVirtualServer(string url, bool deleteContent);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UnextendVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UnextendVirtualServerResponse")]
        System.Threading.Tasks.Task UnextendVirtualServerAsync(string url, bool deleteContent);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/BackupVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/BackupVirtualServerResponse")]
        string BackupVirtualServer(string url, string fileName, bool zipBackup);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/BackupVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/BackupVirtualServerResponse")]
        System.Threading.Tasks.Task<string> BackupVirtualServerAsync(string url, string fileName, bool zipBackup);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/RestoreVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/RestoreVirtualServerResponse")]
        void RestoreVirtualServer(string url, string fileName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/RestoreVirtualServer", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/RestoreVirtualServerResponse")]
        System.Threading.Tasks.Task RestoreVirtualServerAsync(string url, string fileName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetTempFileBinaryChunkResponse")]
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/AppendTempFileBinaryChunkResponse")]
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/AppendTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetInstalledWebParts", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetInstalledWebPartsResponse")]
        string[] GetInstalledWebParts(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetInstalledWebParts", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetInstalledWebPartsResponse")]
        System.Threading.Tasks.Task<string[]> GetInstalledWebPartsAsync(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/InstallWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/InstallWebPartsPackageResponse")]
        void InstallWebPartsPackage(string url, string packageName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/InstallWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/InstallWebPartsPackageResponse")]
        System.Threading.Tasks.Task InstallWebPartsPackageAsync(string url, string packageName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/DeleteWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/DeleteWebPartsPackageResponse")]
        void DeleteWebPartsPackage(string url, string packageName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/DeleteWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/DeleteWebPartsPackageResponse")]
        System.Threading.Tasks.Task DeleteWebPartsPackageAsync(string url, string packageName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UserExists", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UserExistsResponse")]
        bool UserExists(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UserExists", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UserExistsResponse")]
        System.Threading.Tasks.Task<bool> UserExistsAsync(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetUsersResponse")]
        string[] GetUsers();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetUsersResponse")]
        System.Threading.Tasks.Task<string[]> GetUsersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetUserResponse")]
        SolidCP.Providers.OS.SystemUser GetUser(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser> GetUserAsync(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/CreateUserResponse")]
        void CreateUser(SolidCP.Providers.OS.SystemUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/CreateUserResponse")]
        System.Threading.Tasks.Task CreateUserAsync(SolidCP.Providers.OS.SystemUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UpdateUserResponse")]
        void UpdateUser(SolidCP.Providers.OS.SystemUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UpdateUserResponse")]
        System.Threading.Tasks.Task UpdateUserAsync(SolidCP.Providers.OS.SystemUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/ChangeUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/ChangeUserPasswordResponse")]
        void ChangeUserPassword(string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/ChangeUserPassword", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/ChangeUserPasswordResponse")]
        System.Threading.Tasks.Task ChangeUserPasswordAsync(string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/DeleteUserResponse")]
        void DeleteUser(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/DeleteUserResponse")]
        System.Threading.Tasks.Task DeleteUserAsync(string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GroupExists", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GroupExistsResponse")]
        bool GroupExists(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GroupExists", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GroupExistsResponse")]
        System.Threading.Tasks.Task<bool> GroupExistsAsync(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetGroupsResponse")]
        string[] GetGroups();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetGroupsResponse")]
        System.Threading.Tasks.Task<string[]> GetGroupsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetGroupResponse")]
        SolidCP.Providers.OS.SystemGroup GetGroup(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/GetGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup> GetGroupAsync(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/CreateGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/CreateGroupResponse")]
        void CreateGroup(SolidCP.Providers.OS.SystemGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/CreateGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/CreateGroupResponse")]
        System.Threading.Tasks.Task CreateGroupAsync(SolidCP.Providers.OS.SystemGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UpdateGroupResponse")]
        void UpdateGroup(SolidCP.Providers.OS.SystemGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/UpdateGroupResponse")]
        System.Threading.Tasks.Task UpdateGroupAsync(SolidCP.Providers.OS.SystemGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/DeleteGroupResponse")]
        void DeleteGroup(string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/ISharePointServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/ISharePointServer/DeleteGroupResponse")]
        System.Threading.Tasks.Task DeleteGroupAsync(string groupName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SharePointServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, ISharePointServer
    {
        public void ExtendVirtualServer(SolidCP.Providers.SharePoint.SharePointSite site)
        {
            Invoke("SolidCP.Server.SharePointServer", "ExtendVirtualServer", site);
        }

        public async System.Threading.Tasks.Task ExtendVirtualServerAsync(SolidCP.Providers.SharePoint.SharePointSite site)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "ExtendVirtualServer", site);
        }

        public void UnextendVirtualServer(string url, bool deleteContent)
        {
            Invoke("SolidCP.Server.SharePointServer", "UnextendVirtualServer", url, deleteContent);
        }

        public async System.Threading.Tasks.Task UnextendVirtualServerAsync(string url, bool deleteContent)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "UnextendVirtualServer", url, deleteContent);
        }

        public string BackupVirtualServer(string url, string fileName, bool zipBackup)
        {
            return Invoke<string>("SolidCP.Server.SharePointServer", "BackupVirtualServer", url, fileName, zipBackup);
        }

        public async System.Threading.Tasks.Task<string> BackupVirtualServerAsync(string url, string fileName, bool zipBackup)
        {
            return await InvokeAsync<string>("SolidCP.Server.SharePointServer", "BackupVirtualServer", url, fileName, zipBackup);
        }

        public void RestoreVirtualServer(string url, string fileName)
        {
            Invoke("SolidCP.Server.SharePointServer", "RestoreVirtualServer", url, fileName);
        }

        public async System.Threading.Tasks.Task RestoreVirtualServerAsync(string url, string fileName)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "RestoreVirtualServer", url, fileName);
        }

        public byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.Server.SharePointServer", "GetTempFileBinaryChunk", path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.SharePointServer", "GetTempFileBinaryChunk", path, offset, length);
        }

        public string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return Invoke<string>("SolidCP.Server.SharePointServer", "AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.Server.SharePointServer", "AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public string[] GetInstalledWebParts(string url)
        {
            return Invoke<string[]>("SolidCP.Server.SharePointServer", "GetInstalledWebParts", url);
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledWebPartsAsync(string url)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.SharePointServer", "GetInstalledWebParts", url);
        }

        public void InstallWebPartsPackage(string url, string packageName)
        {
            Invoke("SolidCP.Server.SharePointServer", "InstallWebPartsPackage", url, packageName);
        }

        public async System.Threading.Tasks.Task InstallWebPartsPackageAsync(string url, string packageName)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "InstallWebPartsPackage", url, packageName);
        }

        public void DeleteWebPartsPackage(string url, string packageName)
        {
            Invoke("SolidCP.Server.SharePointServer", "DeleteWebPartsPackage", url, packageName);
        }

        public async System.Threading.Tasks.Task DeleteWebPartsPackageAsync(string url, string packageName)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "DeleteWebPartsPackage", url, packageName);
        }

        public bool UserExists(string username)
        {
            return Invoke<bool>("SolidCP.Server.SharePointServer", "UserExists", username);
        }

        public async System.Threading.Tasks.Task<bool> UserExistsAsync(string username)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SharePointServer", "UserExists", username);
        }

        public string[] GetUsers()
        {
            return Invoke<string[]>("SolidCP.Server.SharePointServer", "GetUsers");
        }

        public async System.Threading.Tasks.Task<string[]> GetUsersAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.SharePointServer", "GetUsers");
        }

        public SolidCP.Providers.OS.SystemUser GetUser(string username)
        {
            return Invoke<SolidCP.Providers.OS.SystemUser>("SolidCP.Server.SharePointServer", "GetUser", username);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser> GetUserAsync(string username)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemUser>("SolidCP.Server.SharePointServer", "GetUser", username);
        }

        public void CreateUser(SolidCP.Providers.OS.SystemUser user)
        {
            Invoke("SolidCP.Server.SharePointServer", "CreateUser", user);
        }

        public async System.Threading.Tasks.Task CreateUserAsync(SolidCP.Providers.OS.SystemUser user)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "CreateUser", user);
        }

        public void UpdateUser(SolidCP.Providers.OS.SystemUser user)
        {
            Invoke("SolidCP.Server.SharePointServer", "UpdateUser", user);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(SolidCP.Providers.OS.SystemUser user)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "UpdateUser", user);
        }

        public void ChangeUserPassword(string username, string password)
        {
            Invoke("SolidCP.Server.SharePointServer", "ChangeUserPassword", username, password);
        }

        public async System.Threading.Tasks.Task ChangeUserPasswordAsync(string username, string password)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "ChangeUserPassword", username, password);
        }

        public void DeleteUser(string username)
        {
            Invoke("SolidCP.Server.SharePointServer", "DeleteUser", username);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string username)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "DeleteUser", username);
        }

        public bool GroupExists(string groupName)
        {
            return Invoke<bool>("SolidCP.Server.SharePointServer", "GroupExists", groupName);
        }

        public async System.Threading.Tasks.Task<bool> GroupExistsAsync(string groupName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.SharePointServer", "GroupExists", groupName);
        }

        public string[] GetGroups()
        {
            return Invoke<string[]>("SolidCP.Server.SharePointServer", "GetGroups");
        }

        public async System.Threading.Tasks.Task<string[]> GetGroupsAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.SharePointServer", "GetGroups");
        }

        public SolidCP.Providers.OS.SystemGroup GetGroup(string groupName)
        {
            return Invoke<SolidCP.Providers.OS.SystemGroup>("SolidCP.Server.SharePointServer", "GetGroup", groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup> GetGroupAsync(string groupName)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemGroup>("SolidCP.Server.SharePointServer", "GetGroup", groupName);
        }

        public void CreateGroup(SolidCP.Providers.OS.SystemGroup group)
        {
            Invoke("SolidCP.Server.SharePointServer", "CreateGroup", group);
        }

        public async System.Threading.Tasks.Task CreateGroupAsync(SolidCP.Providers.OS.SystemGroup group)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "CreateGroup", group);
        }

        public void UpdateGroup(SolidCP.Providers.OS.SystemGroup group)
        {
            Invoke("SolidCP.Server.SharePointServer", "UpdateGroup", group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(SolidCP.Providers.OS.SystemGroup group)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "UpdateGroup", group);
        }

        public void DeleteGroup(string groupName)
        {
            Invoke("SolidCP.Server.SharePointServer", "DeleteGroup", groupName);
        }

        public async System.Threading.Tasks.Task DeleteGroupAsync(string groupName)
        {
            await InvokeAsync("SolidCP.Server.SharePointServer", "DeleteGroup", groupName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class SharePointServer : SolidCP.Web.Client.ClientBase<ISharePointServer, SharePointServerAssemblyClient>, ISharePointServer
    {
        public void ExtendVirtualServer(SolidCP.Providers.SharePoint.SharePointSite site)
        {
            base.Client.ExtendVirtualServer(site);
        }

        public async System.Threading.Tasks.Task ExtendVirtualServerAsync(SolidCP.Providers.SharePoint.SharePointSite site)
        {
            await base.Client.ExtendVirtualServerAsync(site);
        }

        public void UnextendVirtualServer(string url, bool deleteContent)
        {
            base.Client.UnextendVirtualServer(url, deleteContent);
        }

        public async System.Threading.Tasks.Task UnextendVirtualServerAsync(string url, bool deleteContent)
        {
            await base.Client.UnextendVirtualServerAsync(url, deleteContent);
        }

        public string BackupVirtualServer(string url, string fileName, bool zipBackup)
        {
            return base.Client.BackupVirtualServer(url, fileName, zipBackup);
        }

        public async System.Threading.Tasks.Task<string> BackupVirtualServerAsync(string url, string fileName, bool zipBackup)
        {
            return await base.Client.BackupVirtualServerAsync(url, fileName, zipBackup);
        }

        public void RestoreVirtualServer(string url, string fileName)
        {
            base.Client.RestoreVirtualServer(url, fileName);
        }

        public async System.Threading.Tasks.Task RestoreVirtualServerAsync(string url, string fileName)
        {
            await base.Client.RestoreVirtualServerAsync(url, fileName);
        }

        public byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.Client.GetTempFileBinaryChunk(path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await base.Client.GetTempFileBinaryChunkAsync(path, offset, length);
        }

        public string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.Client.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk)
        {
            return await base.Client.AppendTempFileBinaryChunkAsync(fileName, path, chunk);
        }

        public string[] GetInstalledWebParts(string url)
        {
            return base.Client.GetInstalledWebParts(url);
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledWebPartsAsync(string url)
        {
            return await base.Client.GetInstalledWebPartsAsync(url);
        }

        public void InstallWebPartsPackage(string url, string packageName)
        {
            base.Client.InstallWebPartsPackage(url, packageName);
        }

        public async System.Threading.Tasks.Task InstallWebPartsPackageAsync(string url, string packageName)
        {
            await base.Client.InstallWebPartsPackageAsync(url, packageName);
        }

        public void DeleteWebPartsPackage(string url, string packageName)
        {
            base.Client.DeleteWebPartsPackage(url, packageName);
        }

        public async System.Threading.Tasks.Task DeleteWebPartsPackageAsync(string url, string packageName)
        {
            await base.Client.DeleteWebPartsPackageAsync(url, packageName);
        }

        public bool UserExists(string username)
        {
            return base.Client.UserExists(username);
        }

        public async System.Threading.Tasks.Task<bool> UserExistsAsync(string username)
        {
            return await base.Client.UserExistsAsync(username);
        }

        public string[] GetUsers()
        {
            return base.Client.GetUsers();
        }

        public async System.Threading.Tasks.Task<string[]> GetUsersAsync()
        {
            return await base.Client.GetUsersAsync();
        }

        public SolidCP.Providers.OS.SystemUser GetUser(string username)
        {
            return base.Client.GetUser(username);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser> GetUserAsync(string username)
        {
            return await base.Client.GetUserAsync(username);
        }

        public void CreateUser(SolidCP.Providers.OS.SystemUser user)
        {
            base.Client.CreateUser(user);
        }

        public async System.Threading.Tasks.Task CreateUserAsync(SolidCP.Providers.OS.SystemUser user)
        {
            await base.Client.CreateUserAsync(user);
        }

        public void UpdateUser(SolidCP.Providers.OS.SystemUser user)
        {
            base.Client.UpdateUser(user);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(SolidCP.Providers.OS.SystemUser user)
        {
            await base.Client.UpdateUserAsync(user);
        }

        public void ChangeUserPassword(string username, string password)
        {
            base.Client.ChangeUserPassword(username, password);
        }

        public async System.Threading.Tasks.Task ChangeUserPasswordAsync(string username, string password)
        {
            await base.Client.ChangeUserPasswordAsync(username, password);
        }

        public void DeleteUser(string username)
        {
            base.Client.DeleteUser(username);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string username)
        {
            await base.Client.DeleteUserAsync(username);
        }

        public bool GroupExists(string groupName)
        {
            return base.Client.GroupExists(groupName);
        }

        public async System.Threading.Tasks.Task<bool> GroupExistsAsync(string groupName)
        {
            return await base.Client.GroupExistsAsync(groupName);
        }

        public string[] GetGroups()
        {
            return base.Client.GetGroups();
        }

        public async System.Threading.Tasks.Task<string[]> GetGroupsAsync()
        {
            return await base.Client.GetGroupsAsync();
        }

        public SolidCP.Providers.OS.SystemGroup GetGroup(string groupName)
        {
            return base.Client.GetGroup(groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup> GetGroupAsync(string groupName)
        {
            return await base.Client.GetGroupAsync(groupName);
        }

        public void CreateGroup(SolidCP.Providers.OS.SystemGroup group)
        {
            base.Client.CreateGroup(group);
        }

        public async System.Threading.Tasks.Task CreateGroupAsync(SolidCP.Providers.OS.SystemGroup group)
        {
            await base.Client.CreateGroupAsync(group);
        }

        public void UpdateGroup(SolidCP.Providers.OS.SystemGroup group)
        {
            base.Client.UpdateGroup(group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(SolidCP.Providers.OS.SystemGroup group)
        {
            await base.Client.UpdateGroupAsync(group);
        }

        public void DeleteGroup(string groupName)
        {
            base.Client.DeleteGroup(groupName);
        }

        public async System.Threading.Tasks.Task DeleteGroupAsync(string groupName)
        {
            await base.Client.DeleteGroupAsync(groupName);
        }
    }
}
#endif