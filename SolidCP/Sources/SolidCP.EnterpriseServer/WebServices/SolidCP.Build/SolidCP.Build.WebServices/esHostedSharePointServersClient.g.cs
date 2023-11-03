#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesHostedSharePointServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesHostedSharePointServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionsPagedResponse")]
        SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged> GetSiteCollectionsPagedAsync(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSupportedLanguagesResponse")]
        int[] GetSupportedLanguages(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSupportedLanguagesResponse")]
        System.Threading.Tasks.Task<int[]> GetSupportedLanguagesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionsResponse")]
        SolidCP.Providers.SharePoint.SharePointSiteCollection[] /*List*/ GetSiteCollections(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection[]> GetSiteCollectionsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/SetStorageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/SetStorageSettingsResponse")]
        int SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/SetStorageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/SetStorageSettingsResponse")]
        System.Threading.Tasks.Task<int> SetStorageSettingsAsync(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionResponse")]
        SolidCP.Providers.SharePoint.SharePointSiteCollection GetSiteCollection(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection> GetSiteCollectionAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionByDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionByDomainResponse")]
        SolidCP.Providers.SharePoint.SharePointSiteCollection GetSiteCollectionByDomain(int organizationId, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionByDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetSiteCollectionByDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection> GetSiteCollectionByDomainAsync(int organizationId, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AddSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AddSiteCollectionResponse")]
        int AddSiteCollection(SolidCP.Providers.SharePoint.SharePointSiteCollection item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AddSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AddSiteCollectionResponse")]
        System.Threading.Tasks.Task<int> AddSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointSiteCollection item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollectionResponse")]
        int DeleteSiteCollection(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollectionResponse")]
        System.Threading.Tasks.Task<int> DeleteSiteCollectionAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollectionsResponse")]
        int DeleteSiteCollections(int organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/DeleteSiteCollectionsResponse")]
        System.Threading.Tasks.Task<int> DeleteSiteCollectionsAsync(int organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/BackupSiteCollectionResponse")]
        string BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/BackupSiteCollectionResponse")]
        System.Threading.Tasks.Task<string> BackupSiteCollectionAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/RestoreSiteCollectionResponse")]
        int RestoreSiteCollection(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/RestoreSiteCollectionResponse")]
        System.Threading.Tasks.Task<int> RestoreSiteCollectionAsync(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetBackupBinaryChunkResponse")]
        byte[] GetBackupBinaryChunk(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/GetBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetBackupBinaryChunkAsync(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AppendBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AppendBackupBinaryChunkResponse")]
        string AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AppendBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/AppendBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> AppendBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/CalculateSharePointSitesDiskSpace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/CalculateSharePointSitesDiskSpaceResponse")]
        SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] CalculateSharePointSitesDiskSpace(int itemId, out int errorCode);
        // No async method, because method has ref, in or out parameters.
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/UpdateQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/UpdateQuotaResponse")]
        void UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/UpdateQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServers/UpdateQuotaResponse")]
        System.Threading.Tasks.Task UpdateQuotaAsync(int itemId, int siteCollectionId, int maxSize, int warningSize);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esHostedSharePointServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesHostedSharePointServers
    {
        public SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollectionsPaged", packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged> GetSiteCollectionsPagedAsync(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollectionsPaged", packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public int[] GetSupportedLanguages(int packageId)
        {
            return Invoke<int[]>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSupportedLanguages", packageId);
        }

        public async System.Threading.Tasks.Task<int[]> GetSupportedLanguagesAsync(int packageId)
        {
            return await InvokeAsync<int[]>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSupportedLanguages", packageId);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteCollection[] /*List*/ GetSiteCollections(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointSiteCollection[], SolidCP.Providers.SharePoint.SharePointSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollections", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection[]> GetSiteCollectionsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointSiteCollection[], SolidCP.Providers.SharePoint.SharePointSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollections", packageId, recursive);
        }

        public int SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "SetStorageSettings", itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public async System.Threading.Tasks.Task<int> SetStorageSettingsAsync(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "SetStorageSettings", itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteCollection GetSiteCollection(int itemId)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollection", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection> GetSiteCollectionAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollection", itemId);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteCollection GetSiteCollectionByDomain(int organizationId, string domain)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollectionByDomain", organizationId, domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection> GetSiteCollectionByDomainAsync(int organizationId, string domain)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetSiteCollectionByDomain", organizationId, domain);
        }

        public int AddSiteCollection(SolidCP.Providers.SharePoint.SharePointSiteCollection item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "AddSiteCollection", item);
        }

        public async System.Threading.Tasks.Task<int> AddSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointSiteCollection item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "AddSiteCollection", item);
        }

        public int DeleteSiteCollection(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "DeleteSiteCollection", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSiteCollectionAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "DeleteSiteCollection", itemId);
        }

        public int DeleteSiteCollections(int organizationId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "DeleteSiteCollections", organizationId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSiteCollectionsAsync(int organizationId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "DeleteSiteCollections", organizationId);
        }

        public string BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esHostedSharePointServers", "BackupSiteCollection", itemId, fileName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> BackupSiteCollectionAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esHostedSharePointServers", "BackupSiteCollection", itemId, fileName, zipBackup, download, folderName);
        }

        public int RestoreSiteCollection(int itemId, string uploadedFile, string packageFile)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "RestoreSiteCollection", itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> RestoreSiteCollectionAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServers", "RestoreSiteCollection", itemId, uploadedFile, packageFile);
        }

        public byte[] GetBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetBackupBinaryChunk", itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esHostedSharePointServers", "GetBackupBinaryChunk", itemId, path, offset, length);
        }

        public string AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esHostedSharePointServers", "AppendBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esHostedSharePointServers", "AppendBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] CalculateSharePointSitesDiskSpace(int itemId, out int errorCode)
        {
            var _params = new object[]
            {
                itemId,
                null
            };
            var _result = Invoke<SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[]>("SolidCP.EnterpriseServer.esHostedSharePointServers", "CalculateSharePointSitesDiskSpace", _params);
            errorCode = (int)_params[0];
            return _result;
        }

        // No async method since asnyc methods cannot contain ref, in or out parameters.
        public void UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            Invoke("SolidCP.EnterpriseServer.esHostedSharePointServers", "UpdateQuota", itemId, siteCollectionId, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task UpdateQuotaAsync(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esHostedSharePointServers", "UpdateQuota", itemId, siteCollectionId, maxSize, warningSize);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esHostedSharePointServers : SolidCP.Web.Client.ClientBase<IesHostedSharePointServers, esHostedSharePointServersAssemblyClient>, IesHostedSharePointServers
    {
        public SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetSiteCollectionsPaged(packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollectionListPaged> GetSiteCollectionsPagedAsync(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetSiteCollectionsPagedAsync(packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public int[] GetSupportedLanguages(int packageId)
        {
            return base.Client.GetSupportedLanguages(packageId);
        }

        public async System.Threading.Tasks.Task<int[]> GetSupportedLanguagesAsync(int packageId)
        {
            return await base.Client.GetSupportedLanguagesAsync(packageId);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteCollection[] /*List*/ GetSiteCollections(int packageId, bool recursive)
        {
            return base.Client.GetSiteCollections(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection[]> GetSiteCollectionsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetSiteCollectionsAsync(packageId, recursive);
        }

        public int SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return base.Client.SetStorageSettings(itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public async System.Threading.Tasks.Task<int> SetStorageSettingsAsync(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return await base.Client.SetStorageSettingsAsync(itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteCollection GetSiteCollection(int itemId)
        {
            return base.Client.GetSiteCollection(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection> GetSiteCollectionAsync(int itemId)
        {
            return await base.Client.GetSiteCollectionAsync(itemId);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteCollection GetSiteCollectionByDomain(int organizationId, string domain)
        {
            return base.Client.GetSiteCollectionByDomain(organizationId, domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteCollection> GetSiteCollectionByDomainAsync(int organizationId, string domain)
        {
            return await base.Client.GetSiteCollectionByDomainAsync(organizationId, domain);
        }

        public int AddSiteCollection(SolidCP.Providers.SharePoint.SharePointSiteCollection item)
        {
            return base.Client.AddSiteCollection(item);
        }

        public async System.Threading.Tasks.Task<int> AddSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointSiteCollection item)
        {
            return await base.Client.AddSiteCollectionAsync(item);
        }

        public int DeleteSiteCollection(int itemId)
        {
            return base.Client.DeleteSiteCollection(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSiteCollectionAsync(int itemId)
        {
            return await base.Client.DeleteSiteCollectionAsync(itemId);
        }

        public int DeleteSiteCollections(int organizationId)
        {
            return base.Client.DeleteSiteCollections(organizationId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSiteCollectionsAsync(int organizationId)
        {
            return await base.Client.DeleteSiteCollectionsAsync(organizationId);
        }

        public string BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return base.Client.BackupSiteCollection(itemId, fileName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> BackupSiteCollectionAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return await base.Client.BackupSiteCollectionAsync(itemId, fileName, zipBackup, download, folderName);
        }

        public int RestoreSiteCollection(int itemId, string uploadedFile, string packageFile)
        {
            return base.Client.RestoreSiteCollection(itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> RestoreSiteCollectionAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await base.Client.RestoreSiteCollectionAsync(itemId, uploadedFile, packageFile);
        }

        public byte[] GetBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return base.Client.GetBackupBinaryChunk(itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await base.Client.GetBackupBinaryChunkAsync(itemId, path, offset, length);
        }

        public string AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return base.Client.AppendBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await base.Client.AppendBackupBinaryChunkAsync(itemId, fileName, path, chunk);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] CalculateSharePointSitesDiskSpace(int itemId, out int errorCode)
        {
            return base.Client.CalculateSharePointSitesDiskSpace(itemId, out errorCode);
        }

        //No async method, because the method has ref, in or out parameters.
        public void UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            base.Client.UpdateQuota(itemId, siteCollectionId, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task UpdateQuotaAsync(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            await base.Client.UpdateQuotaAsync(itemId, siteCollectionId, maxSize, warningSize);
        }
    }
}
#endif