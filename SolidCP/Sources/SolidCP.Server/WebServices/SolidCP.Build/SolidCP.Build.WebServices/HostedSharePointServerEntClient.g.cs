#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IHostedSharePointServerEnt", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IHostedSharePointServerEnt
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSupportedLanguagesResponse")]
        int[] Enterprise_GetSupportedLanguages();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSupportedLanguagesResponse")]
        System.Threading.Tasks.Task<int[]> Enterprise_GetSupportedLanguagesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionsResponse")]
        SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]> Enterprise_GetSiteCollectionsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionResponse")]
        SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionAsync(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CreateSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CreateSiteCollectionResponse")]
        void Enterprise_CreateSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CreateSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CreateSiteCollectionResponse")]
        System.Threading.Tasks.Task Enterprise_CreateSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_UpdateQuotas", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_UpdateQuotasResponse")]
        void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_UpdateQuotas", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_UpdateQuotasResponse")]
        System.Threading.Tasks.Task Enterprise_UpdateQuotasAsync(string url, long maxSize, long warningSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CalculateSiteCollectionsDiskSpace", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CalculateSiteCollectionsDiskSpaceResponse")]
        SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CalculateSiteCollectionsDiskSpace", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_CalculateSiteCollectionsDiskSpaceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[]> Enterprise_CalculateSiteCollectionsDiskSpaceAsync(string[] urls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_DeleteSiteCollectionResponse")]
        void Enterprise_DeleteSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_DeleteSiteCollectionResponse")]
        System.Threading.Tasks.Task Enterprise_DeleteSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_BackupSiteCollectionResponse")]
        string Enterprise_BackupSiteCollection(string url, string filename, bool zip);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_BackupSiteCollectionResponse")]
        System.Threading.Tasks.Task<string> Enterprise_BackupSiteCollectionAsync(string url, string filename, bool zip);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_RestoreSiteCollectionResponse")]
        void Enterprise_RestoreSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection, string filename);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_RestoreSiteCollectionResponse")]
        System.Threading.Tasks.Task Enterprise_RestoreSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection, string filename);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetTempFileBinaryChunkResponse")]
        byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> Enterprise_GetTempFileBinaryChunkAsync(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_AppendTempFileBinaryChunkResponse")]
        string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_AppendTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> Enterprise_AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionSize", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionSizeResponse")]
        long Enterprise_GetSiteCollectionSize(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionSize", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_GetSiteCollectionSizeResponse")]
        System.Threading.Tasks.Task<long> Enterprise_GetSiteCollectionSizeAsync(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_SetPeoplePickerOu", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_SetPeoplePickerOuResponse")]
        void Enterprise_SetPeoplePickerOu(string site, string ou);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_SetPeoplePickerOu", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServerEnt/Enterprise_SetPeoplePickerOuResponse")]
        System.Threading.Tasks.Task Enterprise_SetPeoplePickerOuAsync(string site, string ou);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class HostedSharePointServerEntAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IHostedSharePointServerEnt
    {
        public int[] Enterprise_GetSupportedLanguages()
        {
            return Invoke<int[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSupportedLanguages");
        }

        public async System.Threading.Tasks.Task<int[]> Enterprise_GetSupportedLanguagesAsync()
        {
            return await InvokeAsync<int[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSupportedLanguages");
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections()
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSiteCollections");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]> Enterprise_GetSiteCollectionsAsync()
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSiteCollections");
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSiteCollection", url);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionAsync(string url)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSiteCollection", url);
        }

        public void Enterprise_CreateSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            Invoke("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_CreateSiteCollection", siteCollection);
        }

        public async System.Threading.Tasks.Task Enterprise_CreateSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_CreateSiteCollection", siteCollection);
        }

        public void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize)
        {
            Invoke("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_UpdateQuotas", url, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task Enterprise_UpdateQuotasAsync(string url, long maxSize, long warningSize)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_UpdateQuotas", url, maxSize, warningSize);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return Invoke<SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_CalculateSiteCollectionsDiskSpace", urls);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[]> Enterprise_CalculateSiteCollectionsDiskSpaceAsync(string[] urls)
        {
            return await InvokeAsync<SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_CalculateSiteCollectionsDiskSpace", urls);
        }

        public void Enterprise_DeleteSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            Invoke("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_DeleteSiteCollection", siteCollection);
        }

        public async System.Threading.Tasks.Task Enterprise_DeleteSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_DeleteSiteCollection", siteCollection);
        }

        public string Enterprise_BackupSiteCollection(string url, string filename, bool zip)
        {
            return Invoke<string>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_BackupSiteCollection", url, filename, zip);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_BackupSiteCollectionAsync(string url, string filename, bool zip)
        {
            return await InvokeAsync<string>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_BackupSiteCollection", url, filename, zip);
        }

        public void Enterprise_RestoreSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            Invoke("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_RestoreSiteCollection", siteCollection, filename);
        }

        public async System.Threading.Tasks.Task Enterprise_RestoreSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_RestoreSiteCollection", siteCollection, filename);
        }

        public byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetTempFileBinaryChunk", path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> Enterprise_GetTempFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetTempFileBinaryChunk", path, offset, length);
        }

        public string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return Invoke<string>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public long Enterprise_GetSiteCollectionSize(string url)
        {
            return Invoke<long>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSiteCollectionSize", url);
        }

        public async System.Threading.Tasks.Task<long> Enterprise_GetSiteCollectionSizeAsync(string url)
        {
            return await InvokeAsync<long>("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_GetSiteCollectionSize", url);
        }

        public void Enterprise_SetPeoplePickerOu(string site, string ou)
        {
            Invoke("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_SetPeoplePickerOu", site, ou);
        }

        public async System.Threading.Tasks.Task Enterprise_SetPeoplePickerOuAsync(string site, string ou)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServerEnt", "Enterprise_SetPeoplePickerOu", site, ou);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class HostedSharePointServerEnt : SolidCP.Web.Client.ClientBase<IHostedSharePointServerEnt, HostedSharePointServerEntAssemblyClient>, IHostedSharePointServerEnt
    {
        public int[] Enterprise_GetSupportedLanguages()
        {
            return base.Client.Enterprise_GetSupportedLanguages();
        }

        public async System.Threading.Tasks.Task<int[]> Enterprise_GetSupportedLanguagesAsync()
        {
            return await base.Client.Enterprise_GetSupportedLanguagesAsync();
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections()
        {
            return base.Client.Enterprise_GetSiteCollections();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection[]> Enterprise_GetSiteCollectionsAsync()
        {
            return await base.Client.Enterprise_GetSiteCollectionsAsync();
        }

        public SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url)
        {
            return base.Client.Enterprise_GetSiteCollection(url);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollectionAsync(string url)
        {
            return await base.Client.Enterprise_GetSiteCollectionAsync(url);
        }

        public void Enterprise_CreateSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            base.Client.Enterprise_CreateSiteCollection(siteCollection);
        }

        public async System.Threading.Tasks.Task Enterprise_CreateSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            await base.Client.Enterprise_CreateSiteCollectionAsync(siteCollection);
        }

        public void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize)
        {
            base.Client.Enterprise_UpdateQuotas(url, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task Enterprise_UpdateQuotasAsync(string url, long maxSize, long warningSize)
        {
            await base.Client.Enterprise_UpdateQuotasAsync(url, maxSize, warningSize);
        }

        public SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return base.Client.Enterprise_CalculateSiteCollectionsDiskSpace(urls);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.SharePoint.SharePointSiteDiskSpace[]> Enterprise_CalculateSiteCollectionsDiskSpaceAsync(string[] urls)
        {
            return await base.Client.Enterprise_CalculateSiteCollectionsDiskSpaceAsync(urls);
        }

        public void Enterprise_DeleteSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            base.Client.Enterprise_DeleteSiteCollection(siteCollection);
        }

        public async System.Threading.Tasks.Task Enterprise_DeleteSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection)
        {
            await base.Client.Enterprise_DeleteSiteCollectionAsync(siteCollection);
        }

        public string Enterprise_BackupSiteCollection(string url, string filename, bool zip)
        {
            return base.Client.Enterprise_BackupSiteCollection(url, filename, zip);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_BackupSiteCollectionAsync(string url, string filename, bool zip)
        {
            return await base.Client.Enterprise_BackupSiteCollectionAsync(url, filename, zip);
        }

        public void Enterprise_RestoreSiteCollection(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            base.Client.Enterprise_RestoreSiteCollection(siteCollection, filename);
        }

        public async System.Threading.Tasks.Task Enterprise_RestoreSiteCollectionAsync(SolidCP.Providers.SharePoint.SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            await base.Client.Enterprise_RestoreSiteCollectionAsync(siteCollection, filename);
        }

        public byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.Client.Enterprise_GetTempFileBinaryChunk(path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> Enterprise_GetTempFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await base.Client.Enterprise_GetTempFileBinaryChunkAsync(path, offset, length);
        }

        public string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.Client.Enterprise_AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> Enterprise_AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk)
        {
            return await base.Client.Enterprise_AppendTempFileBinaryChunkAsync(fileName, path, chunk);
        }

        public long Enterprise_GetSiteCollectionSize(string url)
        {
            return base.Client.Enterprise_GetSiteCollectionSize(url);
        }

        public async System.Threading.Tasks.Task<long> Enterprise_GetSiteCollectionSizeAsync(string url)
        {
            return await base.Client.Enterprise_GetSiteCollectionSizeAsync(url);
        }

        public void Enterprise_SetPeoplePickerOu(string site, string ou)
        {
            base.Client.Enterprise_SetPeoplePickerOu(site, ou);
        }

        public async System.Threading.Tasks.Task Enterprise_SetPeoplePickerOuAsync(string site, string ou)
        {
            await base.Client.Enterprise_SetPeoplePickerOuAsync(site, ou);
        }
    }
}
#endif