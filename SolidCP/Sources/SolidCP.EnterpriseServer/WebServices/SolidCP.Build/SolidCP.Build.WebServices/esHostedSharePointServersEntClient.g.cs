#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesHostedSharePointServersEnt", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesHostedSharePointServersEnt
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionsPagedResponse")]
        SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged Enterprise_GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged> Enterprise_GetSiteCollectionsPagedAsync(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSupportedLanguagesResponse")]
        int[] Enterprise_GetSupportedLanguages(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSupportedLanguagesResponse")]
        System.Threading.Tasks.Task<int[]> Enterprise_GetSupportedLanguagesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionsResponse")]
        SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[] /*List*/ Enterprise_GetSiteCollections(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]> Enterprise_GetSiteCollectionsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_SetStorageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_SetStorageSettingsResponse")]
        int Enterprise_SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_SetStorageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_SetStorageSettingsResponse")]
        System.Threading.Tasks.Task<int> Enterprise_SetStorageSettingsAsync(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionResponse")]
        SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionByDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionByDomainResponse")]
        SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollectionByDomain(int organizationId, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionByDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetSiteCollectionByDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionByDomainAsync(int organizationId, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AddSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AddSiteCollectionResponse")]
        int Enterprise_AddSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AddSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AddSiteCollectionResponse")]
        System.Threading.Tasks.Task<int> Enterprise_AddSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollectionResponse")]
        int Enterprise_DeleteSiteCollection(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollectionResponse")]
        System.Threading.Tasks.Task<int> Enterprise_DeleteSiteCollectionAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollectionsResponse")]
        int Enterprise_DeleteSiteCollections(int organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_DeleteSiteCollectionsResponse")]
        System.Threading.Tasks.Task<int> Enterprise_DeleteSiteCollectionsAsync(int organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_BackupSiteCollectionResponse")]
        string Enterprise_BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_BackupSiteCollectionResponse")]
        System.Threading.Tasks.Task<string> Enterprise_BackupSiteCollectionAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_RestoreSiteCollectionResponse")]
        int Enterprise_RestoreSiteCollection(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_RestoreSiteCollectionResponse")]
        System.Threading.Tasks.Task<int> Enterprise_RestoreSiteCollectionAsync(int itemId, string uploadedFile, string packageFile);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetBackupBinaryChunkResponse")]
        byte[] Enterprise_GetBackupBinaryChunk(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_GetBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> Enterprise_GetBackupBinaryChunkAsync(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AppendBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AppendBackupBinaryChunkResponse")]
        string Enterprise_AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AppendBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_AppendBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> Enterprise_AppendBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_CalculateSharePointSitesDiskSpace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_CalculateSharePointSitesDiskSpaceResponse")]
        SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] Enterprise_CalculateSharePointSitesDiskSpace(int itemId, out int errorCode);
        // No async method, because method has ref, in or out parameters.
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_UpdateQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_UpdateQuotaResponse")]
        void Enterprise_UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_UpdateQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesHostedSharePointServersEnt/Enterprise_UpdateQuotaResponse")]
        System.Threading.Tasks.Task Enterprise_UpdateQuotaAsync(int itemId, int siteCollectionId, int maxSize, int warningSize);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esHostedSharePointServersEntAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesHostedSharePointServersEnt
    {
        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged Enterprise_GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollectionsPaged", packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged> Enterprise_GetSiteCollectionsPagedAsync(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollectionsPaged", packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public int[] Enterprise_GetSupportedLanguages(int packageId)
        {
            return Invoke<int[]>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSupportedLanguages", packageId);
        }

        public async System.Threading.Tasks.Task<int[]> Enterprise_GetSupportedLanguagesAsync(int packageId)
        {
            return await InvokeAsync<int[]>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSupportedLanguages", packageId);
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[] /*List*/ Enterprise_GetSiteCollections(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[], SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollections", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]> Enterprise_GetSiteCollectionsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[], SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollections", packageId, recursive);
        }

        public int Enterprise_SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_SetStorageSettings", itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_SetStorageSettingsAsync(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_SetStorageSettings", itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(int itemId)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollection", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollection", itemId);
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollectionByDomain(int organizationId, string domain)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollectionByDomain", organizationId, domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionByDomainAsync(int organizationId, string domain)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetSiteCollectionByDomain", organizationId, domain);
        }

        public int Enterprise_AddSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_AddSiteCollection", item);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_AddSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_AddSiteCollection", item);
        }

        public int Enterprise_DeleteSiteCollection(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_DeleteSiteCollection", itemId);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_DeleteSiteCollectionAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_DeleteSiteCollection", itemId);
        }

        public int Enterprise_DeleteSiteCollections(int organizationId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_DeleteSiteCollections", organizationId);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_DeleteSiteCollectionsAsync(int organizationId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_DeleteSiteCollections", organizationId);
        }

        public string Enterprise_BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_BackupSiteCollection", itemId, fileName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_BackupSiteCollectionAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_BackupSiteCollection", itemId, fileName, zipBackup, download, folderName);
        }

        public int Enterprise_RestoreSiteCollection(int itemId, string uploadedFile, string packageFile)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_RestoreSiteCollection", itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_RestoreSiteCollectionAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_RestoreSiteCollection", itemId, uploadedFile, packageFile);
        }

        public byte[] Enterprise_GetBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetBackupBinaryChunk", itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> Enterprise_GetBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_GetBackupBinaryChunk", itemId, path, offset, length);
        }

        public string Enterprise_AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_AppendBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_AppendBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_AppendBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] Enterprise_CalculateSharePointSitesDiskSpace(int itemId, out int errorCode)
        {
            var _params = new object[]
            {
                itemId,
                null
            };
            var _result = Invoke<SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[]>("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_CalculateSharePointSitesDiskSpace", _params);
            errorCode = (int)_params[0];
            return _result;
        }

        // No async method since asnyc methods cannot contain ref, in or out parameters.
        public void Enterprise_UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            Invoke("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_UpdateQuota", itemId, siteCollectionId, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task Enterprise_UpdateQuotaAsync(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esHostedSharePointServersEnt", "Enterprise_UpdateQuota", itemId, siteCollectionId, maxSize, warningSize);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esHostedSharePointServersEnt : SolidCP.Web.Client.ClientBase<IesHostedSharePointServersEnt, esHostedSharePointServersEntAssemblyClient>, IesHostedSharePointServersEnt
    {
        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged Enterprise_GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.Enterprise_GetSiteCollectionsPaged(packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollectionListPaged> Enterprise_GetSiteCollectionsPagedAsync(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.Enterprise_GetSiteCollectionsPagedAsync(packageId, organizationId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public int[] Enterprise_GetSupportedLanguages(int packageId)
        {
            return base.Client.Enterprise_GetSupportedLanguages(packageId);
        }

        public async System.Threading.Tasks.Task<int[]> Enterprise_GetSupportedLanguagesAsync(int packageId)
        {
            return await base.Client.Enterprise_GetSupportedLanguagesAsync(packageId);
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[] /*List*/ Enterprise_GetSiteCollections(int packageId, bool recursive)
        {
            return base.Client.Enterprise_GetSiteCollections(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]> Enterprise_GetSiteCollectionsAsync(int packageId, bool recursive)
        {
            return await base.Client.Enterprise_GetSiteCollectionsAsync(packageId, recursive);
        }

        public int Enterprise_SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return base.Client.Enterprise_SetStorageSettings(itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_SetStorageSettingsAsync(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return await base.Client.Enterprise_SetStorageSettingsAsync(itemId, maxStorage, warningStorage, applyToSiteCollections);
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(int itemId)
        {
            return base.Client.Enterprise_GetSiteCollection(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionAsync(int itemId)
        {
            return await base.Client.Enterprise_GetSiteCollectionAsync(itemId);
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollectionByDomain(int organizationId, string domain)
        {
            return base.Client.Enterprise_GetSiteCollectionByDomain(organizationId, domain);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionByDomainAsync(int organizationId, string domain)
        {
            return await base.Client.Enterprise_GetSiteCollectionByDomainAsync(organizationId, domain);
        }

        public int Enterprise_AddSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection item)
        {
            return base.Client.Enterprise_AddSiteCollection(item);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_AddSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection item)
        {
            return await base.Client.Enterprise_AddSiteCollectionAsync(item);
        }

        public int Enterprise_DeleteSiteCollection(int itemId)
        {
            return base.Client.Enterprise_DeleteSiteCollection(itemId);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_DeleteSiteCollectionAsync(int itemId)
        {
            return await base.Client.Enterprise_DeleteSiteCollectionAsync(itemId);
        }

        public int Enterprise_DeleteSiteCollections(int organizationId)
        {
            return base.Client.Enterprise_DeleteSiteCollections(organizationId);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_DeleteSiteCollectionsAsync(int organizationId)
        {
            return await base.Client.Enterprise_DeleteSiteCollectionsAsync(organizationId);
        }

        public string Enterprise_BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return base.Client.Enterprise_BackupSiteCollection(itemId, fileName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_BackupSiteCollectionAsync(int itemId, string fileName, bool zipBackup, bool download, string folderName)
        {
            return await base.Client.Enterprise_BackupSiteCollectionAsync(itemId, fileName, zipBackup, download, folderName);
        }

        public int Enterprise_RestoreSiteCollection(int itemId, string uploadedFile, string packageFile)
        {
            return base.Client.Enterprise_RestoreSiteCollection(itemId, uploadedFile, packageFile);
        }

        public async System.Threading.Tasks.Task<int> Enterprise_RestoreSiteCollectionAsync(int itemId, string uploadedFile, string packageFile)
        {
            return await base.Client.Enterprise_RestoreSiteCollectionAsync(itemId, uploadedFile, packageFile);
        }

        public byte[] Enterprise_GetBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return base.Client.Enterprise_GetBackupBinaryChunk(itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> Enterprise_GetBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await base.Client.Enterprise_GetBackupBinaryChunkAsync(itemId, path, offset, length);
        }

        public string Enterprise_AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return base.Client.Enterprise_AppendBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_AppendBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await base.Client.Enterprise_AppendBackupBinaryChunkAsync(itemId, fileName, path, chunk);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] Enterprise_CalculateSharePointSitesDiskSpace(int itemId, out int errorCode)
        {
            return base.Client.Enterprise_CalculateSharePointSitesDiskSpace(itemId, out errorCode);
        }

        //No async method, because the method has ref, in or out parameters.
        public void Enterprise_UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            base.Client.Enterprise_UpdateQuota(itemId, siteCollectionId, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task Enterprise_UpdateQuotaAsync(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            await base.Client.Enterprise_UpdateQuotaAsync(itemId, siteCollectionId, maxSize, warningSize);
        }
    }
}
#endif