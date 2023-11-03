#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesSharePointServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesSharePointServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointSitesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointSitesPagedResponse")]
        System.Data.DataSet GetRawSharePointSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointSitesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointSitesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSitesResponse")]
        SolidCP.Providers.SharePoint.SharePointSite[] /*List*/ GetSharePointSites(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSitesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSite[]> GetSharePointSitesAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSiteResponse")]
        SolidCP.Providers.SharePoint.SharePointSite GetSharePointSite(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointSiteResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSite> GetSharePointSiteAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointSiteResponse")]
        int AddSharePointSite(SolidCP.Providers.SharePoint.SharePointSite item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointSiteResponse")]
        System.Threading.Tasks.Task<int> AddSharePointSiteAsync(SolidCP.Providers.SharePoint.SharePointSite item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointSiteResponse")]
        int DeleteSharePointSite(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointSiteResponse")]
        System.Threading.Tasks.Task<int> DeleteSharePointSiteAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/BackupVirtualServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/BackupVirtualServerResponse")]
        string BackupVirtualServer(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/BackupVirtualServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/BackupVirtualServerResponse")]
        System.Threading.Tasks.Task<string> BackupVirtualServerAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointBackupBinaryChunkResponse")]
        byte[] GetSharePointBackupBinaryChunk(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetSharePointBackupBinaryChunkAsync(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AppendSharePointBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AppendSharePointBackupBinaryChunkResponse")]
        string AppendSharePointBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AppendSharePointBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AppendSharePointBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> AppendSharePointBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/RestoreVirtualServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/RestoreVirtualServerResponse")]
        int RestoreVirtualServer(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/RestoreVirtualServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/RestoreVirtualServerResponse")]
        System.Threading.Tasks.Task<int> RestoreVirtualServerAsync(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetInstalledWebParts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetInstalledWebPartsResponse")]
        string[] GetInstalledWebParts(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetInstalledWebParts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetInstalledWebPartsResponse")]
        System.Threading.Tasks.Task<string[]> GetInstalledWebPartsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/InstallWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/InstallWebPartsPackageResponse")]
        int InstallWebPartsPackage(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/InstallWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/InstallWebPartsPackageResponse")]
        System.Threading.Tasks.Task<int> InstallWebPartsPackageAsync(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteWebPartsPackageResponse")]
        int DeleteWebPartsPackage(int itemId, string packageName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteWebPartsPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteWebPartsPackageResponse")]
        System.Threading.Tasks.Task<int> DeleteWebPartsPackageAsync(int itemId, string packageName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointUsersPagedResponse")]
        System.Data.DataSet GetRawSharePointUsersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointUsersPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointUsersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUsersResponse")]
        SolidCP.Providers.OS.SystemUser[] /*List*/ GetSharePointUsers(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser[]> GetSharePointUsersAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUserResponse")]
        SolidCP.Providers.OS.SystemUser GetSharePointUser(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser> GetSharePointUserAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointUserResponse")]
        int AddSharePointUser(SolidCP.Providers.OS.SystemUser item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointUserResponse")]
        System.Threading.Tasks.Task<int> AddSharePointUserAsync(SolidCP.Providers.OS.SystemUser item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointUserResponse")]
        int UpdateSharePointUser(SolidCP.Providers.OS.SystemUser item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointUserResponse")]
        System.Threading.Tasks.Task<int> UpdateSharePointUserAsync(SolidCP.Providers.OS.SystemUser item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointUserResponse")]
        int DeleteSharePointUser(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointUserResponse")]
        System.Threading.Tasks.Task<int> DeleteSharePointUserAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointGroupsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointGroupsPagedResponse")]
        System.Data.DataSet GetRawSharePointGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointGroupsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetRawSharePointGroupsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointGroupsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroupsResponse")]
        SolidCP.Providers.OS.SystemGroup[] /*List*/ GetSharePointGroups(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup[]> GetSharePointGroupsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroupResponse")]
        SolidCP.Providers.OS.SystemGroup GetSharePointGroup(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/GetSharePointGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup> GetSharePointGroupAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointGroupResponse")]
        int AddSharePointGroup(SolidCP.Providers.OS.SystemGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/AddSharePointGroupResponse")]
        System.Threading.Tasks.Task<int> AddSharePointGroupAsync(SolidCP.Providers.OS.SystemGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointGroupResponse")]
        int UpdateSharePointGroup(SolidCP.Providers.OS.SystemGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/UpdateSharePointGroupResponse")]
        System.Threading.Tasks.Task<int> UpdateSharePointGroupAsync(SolidCP.Providers.OS.SystemGroup item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointGroupResponse")]
        int DeleteSharePointGroup(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesSharePointServers/DeleteSharePointGroupResponse")]
        System.Threading.Tasks.Task<int> DeleteSharePointGroupAsync(int itemId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSharePointServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesSharePointServers
    {
        public System.Data.DataSet GetRawSharePointSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esSharePointServers", "GetRawSharePointSitesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esSharePointServers", "GetRawSharePointSitesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.SharePoint.SharePointSite[] /*List*/ GetSharePointSites(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointSite[], SolidCP.Providers.SharePoint.SharePointSite>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointSites", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSite[]> GetSharePointSitesAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointSite[], SolidCP.Providers.SharePoint.SharePointSite>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointSites", packageId, recursive);
        }

        public SolidCP.Providers.SharePoint.SharePointSite GetSharePointSite(int itemId)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointSite>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointSite", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSite> GetSharePointSiteAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointSite>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointSite", itemId);
        }

        public int AddSharePointSite(SolidCP.Providers.SharePoint.SharePointSite item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "AddSharePointSite", item);
        }

        public async System.Threading.Tasks.Task<int> AddSharePointSiteAsync(SolidCP.Providers.SharePoint.SharePointSite item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "AddSharePointSite", item);
        }

        public int DeleteSharePointSite(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteSharePointSite", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharePointSiteAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteSharePointSite", itemId);
        }

        public string BackupVirtualServer(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esSharePointServers", "BackupVirtualServer", itemId, fileName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> BackupVirtualServerAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esSharePointServers", "BackupVirtualServer", itemId, fileName, zipBackup, download, folderName);
        }

        public byte[] GetSharePointBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointBackupBinaryChunk", itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSharePointBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointBackupBinaryChunk", itemId, path, offset, length);
        }

        public string AppendSharePointBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esSharePointServers", "AppendSharePointBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendSharePointBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esSharePointServers", "AppendSharePointBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public int RestoreVirtualServer(int itemId, string uploadedFile, string packageFile)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "RestoreVirtualServer", itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> RestoreVirtualServerAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "RestoreVirtualServer", itemId, uploadedFile, packageFile);
        }

        public string[] GetInstalledWebParts(int itemId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esSharePointServers", "GetInstalledWebParts", itemId);
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledWebPartsAsync(int itemId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esSharePointServers", "GetInstalledWebParts", itemId);
        }

        public int InstallWebPartsPackage(int itemId, string uploadedFile, string packageFile)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "InstallWebPartsPackage", itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> InstallWebPartsPackageAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "InstallWebPartsPackage", itemId, uploadedFile, packageFile);
        }

        public int DeleteWebPartsPackage(int itemId, string packageName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteWebPartsPackage", itemId, packageName);
        }

        public async System.Threading.Tasks.Task<int> DeleteWebPartsPackageAsync(int itemId, string packageName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteWebPartsPackage", itemId, packageName);
        }

        public System.Data.DataSet GetRawSharePointUsersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esSharePointServers", "GetRawSharePointUsersPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointUsersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esSharePointServers", "GetRawSharePointUsersPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.OS.SystemUser[] /*List*/ GetSharePointUsers(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.OS.SystemUser[], SolidCP.Providers.OS.SystemUser>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointUsers", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser[]> GetSharePointUsersAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemUser[], SolidCP.Providers.OS.SystemUser>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointUsers", packageId, recursive);
        }

        public SolidCP.Providers.OS.SystemUser GetSharePointUser(int itemId)
        {
            return Invoke<SolidCP.Providers.OS.SystemUser>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointUser", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser> GetSharePointUserAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemUser>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointUser", itemId);
        }

        public int AddSharePointUser(SolidCP.Providers.OS.SystemUser item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "AddSharePointUser", item);
        }

        public async System.Threading.Tasks.Task<int> AddSharePointUserAsync(SolidCP.Providers.OS.SystemUser item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "AddSharePointUser", item);
        }

        public int UpdateSharePointUser(SolidCP.Providers.OS.SystemUser item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "UpdateSharePointUser", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSharePointUserAsync(SolidCP.Providers.OS.SystemUser item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "UpdateSharePointUser", item);
        }

        public int DeleteSharePointUser(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteSharePointUser", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharePointUserAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteSharePointUser", itemId);
        }

        public System.Data.DataSet GetRawSharePointGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esSharePointServers", "GetRawSharePointGroupsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointGroupsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esSharePointServers", "GetRawSharePointGroupsPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.OS.SystemGroup[] /*List*/ GetSharePointGroups(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.OS.SystemGroup[], SolidCP.Providers.OS.SystemGroup>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointGroups", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup[]> GetSharePointGroupsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemGroup[], SolidCP.Providers.OS.SystemGroup>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointGroups", packageId, recursive);
        }

        public SolidCP.Providers.OS.SystemGroup GetSharePointGroup(int itemId)
        {
            return Invoke<SolidCP.Providers.OS.SystemGroup>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointGroup", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup> GetSharePointGroupAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemGroup>("SolidCP.EnterpriseServer.esSharePointServers", "GetSharePointGroup", itemId);
        }

        public int AddSharePointGroup(SolidCP.Providers.OS.SystemGroup item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "AddSharePointGroup", item);
        }

        public async System.Threading.Tasks.Task<int> AddSharePointGroupAsync(SolidCP.Providers.OS.SystemGroup item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "AddSharePointGroup", item);
        }

        public int UpdateSharePointGroup(SolidCP.Providers.OS.SystemGroup item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "UpdateSharePointGroup", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSharePointGroupAsync(SolidCP.Providers.OS.SystemGroup item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "UpdateSharePointGroup", item);
        }

        public int DeleteSharePointGroup(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteSharePointGroup", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharePointGroupAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esSharePointServers", "DeleteSharePointGroup", itemId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSharePointServers : SolidCP.Web.Client.ClientBase<IesSharePointServers, esSharePointServersAssemblyClient>, IesSharePointServers
    {
        public System.Data.DataSet GetRawSharePointSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawSharePointSitesPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawSharePointSitesPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.SharePoint.SharePointSite[] /*List*/ GetSharePointSites(int packageId, bool recursive)
        {
            return base.Client.GetSharePointSites(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSite[]> GetSharePointSitesAsync(int packageId, bool recursive)
        {
            return await base.Client.GetSharePointSitesAsync(packageId, recursive);
        }

        public SolidCP.Providers.SharePoint.SharePointSite GetSharePointSite(int itemId)
        {
            return base.Client.GetSharePointSite(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSite> GetSharePointSiteAsync(int itemId)
        {
            return await base.Client.GetSharePointSiteAsync(itemId);
        }

        public int AddSharePointSite(SolidCP.Providers.SharePoint.SharePointSite item)
        {
            return base.Client.AddSharePointSite(item);
        }

        public async System.Threading.Tasks.Task<int> AddSharePointSiteAsync(SolidCP.Providers.SharePoint.SharePointSite item)
        {
            return await base.Client.AddSharePointSiteAsync(item);
        }

        public int DeleteSharePointSite(int itemId)
        {
            return base.Client.DeleteSharePointSite(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharePointSiteAsync(int itemId)
        {
            return await base.Client.DeleteSharePointSiteAsync(itemId);
        }

        public string BackupVirtualServer(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return base.Client.BackupVirtualServer(itemId, fileName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> BackupVirtualServerAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return await base.Client.BackupVirtualServerAsync(itemId, fileName, zipBackup, download, folderName);
        }

        public byte[] GetSharePointBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return base.Client.GetSharePointBackupBinaryChunk(itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSharePointBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await base.Client.GetSharePointBackupBinaryChunkAsync(itemId, path, offset, length);
        }

        public string AppendSharePointBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return base.Client.AppendSharePointBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendSharePointBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await base.Client.AppendSharePointBackupBinaryChunkAsync(itemId, fileName, path, chunk);
        }

        public int RestoreVirtualServer(int itemId, string uploadedFile, string packageFile)
        {
            return base.Client.RestoreVirtualServer(itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> RestoreVirtualServerAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await base.Client.RestoreVirtualServerAsync(itemId, uploadedFile, packageFile);
        }

        public string[] GetInstalledWebParts(int itemId)
        {
            return base.Client.GetInstalledWebParts(itemId);
        }

        public async System.Threading.Tasks.Task<string[]> GetInstalledWebPartsAsync(int itemId)
        {
            return await base.Client.GetInstalledWebPartsAsync(itemId);
        }

        public int InstallWebPartsPackage(int itemId, string uploadedFile, string packageFile)
        {
            return base.Client.InstallWebPartsPackage(itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> InstallWebPartsPackageAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await base.Client.InstallWebPartsPackageAsync(itemId, uploadedFile, packageFile);
        }

        public int DeleteWebPartsPackage(int itemId, string packageName)
        {
            return base.Client.DeleteWebPartsPackage(itemId, packageName);
        }

        public async System.Threading.Tasks.Task<int> DeleteWebPartsPackageAsync(int itemId, string packageName)
        {
            return await base.Client.DeleteWebPartsPackageAsync(itemId, packageName);
        }

        public System.Data.DataSet GetRawSharePointUsersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawSharePointUsersPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointUsersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawSharePointUsersPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.OS.SystemUser[] /*List*/ GetSharePointUsers(int packageId, bool recursive)
        {
            return base.Client.GetSharePointUsers(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser[]> GetSharePointUsersAsync(int packageId, bool recursive)
        {
            return await base.Client.GetSharePointUsersAsync(packageId, recursive);
        }

        public SolidCP.Providers.OS.SystemUser GetSharePointUser(int itemId)
        {
            return base.Client.GetSharePointUser(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemUser> GetSharePointUserAsync(int itemId)
        {
            return await base.Client.GetSharePointUserAsync(itemId);
        }

        public int AddSharePointUser(SolidCP.Providers.OS.SystemUser item)
        {
            return base.Client.AddSharePointUser(item);
        }

        public async System.Threading.Tasks.Task<int> AddSharePointUserAsync(SolidCP.Providers.OS.SystemUser item)
        {
            return await base.Client.AddSharePointUserAsync(item);
        }

        public int UpdateSharePointUser(SolidCP.Providers.OS.SystemUser item)
        {
            return base.Client.UpdateSharePointUser(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSharePointUserAsync(SolidCP.Providers.OS.SystemUser item)
        {
            return await base.Client.UpdateSharePointUserAsync(item);
        }

        public int DeleteSharePointUser(int itemId)
        {
            return base.Client.DeleteSharePointUser(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharePointUserAsync(int itemId)
        {
            return await base.Client.DeleteSharePointUserAsync(itemId);
        }

        public System.Data.DataSet GetRawSharePointGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawSharePointGroupsPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSharePointGroupsPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawSharePointGroupsPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.OS.SystemGroup[] /*List*/ GetSharePointGroups(int packageId, bool recursive)
        {
            return base.Client.GetSharePointGroups(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup[]> GetSharePointGroupsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetSharePointGroupsAsync(packageId, recursive);
        }

        public SolidCP.Providers.OS.SystemGroup GetSharePointGroup(int itemId)
        {
            return base.Client.GetSharePointGroup(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemGroup> GetSharePointGroupAsync(int itemId)
        {
            return await base.Client.GetSharePointGroupAsync(itemId);
        }

        public int AddSharePointGroup(SolidCP.Providers.OS.SystemGroup item)
        {
            return base.Client.AddSharePointGroup(item);
        }

        public async System.Threading.Tasks.Task<int> AddSharePointGroupAsync(SolidCP.Providers.OS.SystemGroup item)
        {
            return await base.Client.AddSharePointGroupAsync(item);
        }

        public int UpdateSharePointGroup(SolidCP.Providers.OS.SystemGroup item)
        {
            return base.Client.UpdateSharePointGroup(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSharePointGroupAsync(SolidCP.Providers.OS.SystemGroup item)
        {
            return await base.Client.UpdateSharePointGroupAsync(item);
        }

        public int DeleteSharePointGroup(int itemId)
        {
            return base.Client.DeleteSharePointGroup(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharePointGroupAsync(int itemId)
        {
            return await base.Client.DeleteSharePointGroupAsync(itemId);
        }
    }
}
#endif