#if Client
using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.SharePoint;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IHostedSharePointServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IHostedSharePointServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSupportedLanguagesResponse")]
        int[] GetSupportedLanguages();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSupportedLanguages", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSupportedLanguagesResponse")]
        System.Threading.Tasks.Task<int[]> GetSupportedLanguagesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionsResponse")]
        SharePointSiteCollection[] GetSiteCollections();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollections", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionsResponse")]
        System.Threading.Tasks.Task<SharePointSiteCollection[]> GetSiteCollectionsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionResponse")]
        SharePointSiteCollection GetSiteCollection(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionResponse")]
        System.Threading.Tasks.Task<SharePointSiteCollection> GetSiteCollectionAsync(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/CreateSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/CreateSiteCollectionResponse")]
        void CreateSiteCollection(SharePointSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/CreateSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/CreateSiteCollectionResponse")]
        System.Threading.Tasks.Task CreateSiteCollectionAsync(SharePointSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/UpdateQuotas", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/UpdateQuotasResponse")]
        void UpdateQuotas(string url, long maxSize, long warningSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/UpdateQuotas", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/UpdateQuotasResponse")]
        System.Threading.Tasks.Task UpdateQuotasAsync(string url, long maxSize, long warningSize);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/CalculateSiteCollectionsDiskSpace", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/CalculateSiteCollectionsDiskSpaceResponse")]
        SharePointSiteDiskSpace[] CalculateSiteCollectionsDiskSpace(string[] urls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/CalculateSiteCollectionsDiskSpace", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/CalculateSiteCollectionsDiskSpaceResponse")]
        System.Threading.Tasks.Task<SharePointSiteDiskSpace[]> CalculateSiteCollectionsDiskSpaceAsync(string[] urls);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/DeleteSiteCollectionResponse")]
        void DeleteSiteCollection(SharePointSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/DeleteSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/DeleteSiteCollectionResponse")]
        System.Threading.Tasks.Task DeleteSiteCollectionAsync(SharePointSiteCollection siteCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/BackupSiteCollectionResponse")]
        string BackupSiteCollection(string url, string filename, bool zip);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/BackupSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/BackupSiteCollectionResponse")]
        System.Threading.Tasks.Task<string> BackupSiteCollectionAsync(string url, string filename, bool zip);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/RestoreSiteCollectionResponse")]
        void RestoreSiteCollection(SharePointSiteCollection siteCollection, string filename);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/RestoreSiteCollection", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/RestoreSiteCollectionResponse")]
        System.Threading.Tasks.Task RestoreSiteCollectionAsync(SharePointSiteCollection siteCollection, string filename);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetTempFileBinaryChunkResponse")]
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/AppendTempFileBinaryChunkResponse")]
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/AppendTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionSize", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionSizeResponse")]
        long GetSiteCollectionSize(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionSize", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/GetSiteCollectionSizeResponse")]
        System.Threading.Tasks.Task<long> GetSiteCollectionSizeAsync(string url);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/SetPeoplePickerOu", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/SetPeoplePickerOuResponse")]
        void SetPeoplePickerOu(string site, string ou);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IHostedSharePointServer/SetPeoplePickerOu", ReplyAction = "http://smbsaas/solidcp/server/IHostedSharePointServer/SetPeoplePickerOuResponse")]
        System.Threading.Tasks.Task SetPeoplePickerOuAsync(string site, string ou);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class HostedSharePointServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IHostedSharePointServer
    {
        public int[] GetSupportedLanguages()
        {
            return (int[])Invoke("SolidCP.Server.HostedSharePointServer", "GetSupportedLanguages");
        }

        public async System.Threading.Tasks.Task<int[]> GetSupportedLanguagesAsync()
        {
            return await InvokeAsync<int[]>("SolidCP.Server.HostedSharePointServer", "GetSupportedLanguages");
        }

        public SharePointSiteCollection[] GetSiteCollections()
        {
            return (SharePointSiteCollection[])Invoke("SolidCP.Server.HostedSharePointServer", "GetSiteCollections");
        }

        public async System.Threading.Tasks.Task<SharePointSiteCollection[]> GetSiteCollectionsAsync()
        {
            return await InvokeAsync<SharePointSiteCollection[]>("SolidCP.Server.HostedSharePointServer", "GetSiteCollections");
        }

        public SharePointSiteCollection GetSiteCollection(string url)
        {
            return (SharePointSiteCollection)Invoke("SolidCP.Server.HostedSharePointServer", "GetSiteCollection", url);
        }

        public async System.Threading.Tasks.Task<SharePointSiteCollection> GetSiteCollectionAsync(string url)
        {
            return await InvokeAsync<SharePointSiteCollection>("SolidCP.Server.HostedSharePointServer", "GetSiteCollection", url);
        }

        public void CreateSiteCollection(SharePointSiteCollection siteCollection)
        {
            Invoke("SolidCP.Server.HostedSharePointServer", "CreateSiteCollection", siteCollection);
        }

        public async System.Threading.Tasks.Task CreateSiteCollectionAsync(SharePointSiteCollection siteCollection)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServer", "CreateSiteCollection", siteCollection);
        }

        public void UpdateQuotas(string url, long maxSize, long warningSize)
        {
            Invoke("SolidCP.Server.HostedSharePointServer", "UpdateQuotas", url, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task UpdateQuotasAsync(string url, long maxSize, long warningSize)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServer", "UpdateQuotas", url, maxSize, warningSize);
        }

        public SharePointSiteDiskSpace[] CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return (SharePointSiteDiskSpace[])Invoke("SolidCP.Server.HostedSharePointServer", "CalculateSiteCollectionsDiskSpace", urls);
        }

        public async System.Threading.Tasks.Task<SharePointSiteDiskSpace[]> CalculateSiteCollectionsDiskSpaceAsync(string[] urls)
        {
            return await InvokeAsync<SharePointSiteDiskSpace[]>("SolidCP.Server.HostedSharePointServer", "CalculateSiteCollectionsDiskSpace", urls);
        }

        public void DeleteSiteCollection(SharePointSiteCollection siteCollection)
        {
            Invoke("SolidCP.Server.HostedSharePointServer", "DeleteSiteCollection", siteCollection);
        }

        public async System.Threading.Tasks.Task DeleteSiteCollectionAsync(SharePointSiteCollection siteCollection)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServer", "DeleteSiteCollection", siteCollection);
        }

        public string BackupSiteCollection(string url, string filename, bool zip)
        {
            return (string)Invoke("SolidCP.Server.HostedSharePointServer", "BackupSiteCollection", url, filename, zip);
        }

        public async System.Threading.Tasks.Task<string> BackupSiteCollectionAsync(string url, string filename, bool zip)
        {
            return await InvokeAsync<string>("SolidCP.Server.HostedSharePointServer", "BackupSiteCollection", url, filename, zip);
        }

        public void RestoreSiteCollection(SharePointSiteCollection siteCollection, string filename)
        {
            Invoke("SolidCP.Server.HostedSharePointServer", "RestoreSiteCollection", siteCollection, filename);
        }

        public async System.Threading.Tasks.Task RestoreSiteCollectionAsync(SharePointSiteCollection siteCollection, string filename)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServer", "RestoreSiteCollection", siteCollection, filename);
        }

        public byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return (byte[])Invoke("SolidCP.Server.HostedSharePointServer", "GetTempFileBinaryChunk", path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.HostedSharePointServer", "GetTempFileBinaryChunk", path, offset, length);
        }

        public string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return (string)Invoke("SolidCP.Server.HostedSharePointServer", "AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.Server.HostedSharePointServer", "AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public long GetSiteCollectionSize(string url)
        {
            return (long)Invoke("SolidCP.Server.HostedSharePointServer", "GetSiteCollectionSize", url);
        }

        public async System.Threading.Tasks.Task<long> GetSiteCollectionSizeAsync(string url)
        {
            return await InvokeAsync<long>("SolidCP.Server.HostedSharePointServer", "GetSiteCollectionSize", url);
        }

        public void SetPeoplePickerOu(string site, string ou)
        {
            Invoke("SolidCP.Server.HostedSharePointServer", "SetPeoplePickerOu", site, ou);
        }

        public async System.Threading.Tasks.Task SetPeoplePickerOuAsync(string site, string ou)
        {
            await InvokeAsync("SolidCP.Server.HostedSharePointServer", "SetPeoplePickerOu", site, ou);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class HostedSharePointServer : SolidCP.Web.Client.ClientBase<IHostedSharePointServer, HostedSharePointServerAssemblyClient>, IHostedSharePointServer
    {
        public int[] GetSupportedLanguages()
        {
            return base.Client.GetSupportedLanguages();
        }

        public async System.Threading.Tasks.Task<int[]> GetSupportedLanguagesAsync()
        {
            return await base.Client.GetSupportedLanguagesAsync();
        }

        public SharePointSiteCollection[] GetSiteCollections()
        {
            return base.Client.GetSiteCollections();
        }

        public async System.Threading.Tasks.Task<SharePointSiteCollection[]> GetSiteCollectionsAsync()
        {
            return await base.Client.GetSiteCollectionsAsync();
        }

        public SharePointSiteCollection GetSiteCollection(string url)
        {
            return base.Client.GetSiteCollection(url);
        }

        public async System.Threading.Tasks.Task<SharePointSiteCollection> GetSiteCollectionAsync(string url)
        {
            return await base.Client.GetSiteCollectionAsync(url);
        }

        public void CreateSiteCollection(SharePointSiteCollection siteCollection)
        {
            base.Client.CreateSiteCollection(siteCollection);
        }

        public async System.Threading.Tasks.Task CreateSiteCollectionAsync(SharePointSiteCollection siteCollection)
        {
            await base.Client.CreateSiteCollectionAsync(siteCollection);
        }

        public void UpdateQuotas(string url, long maxSize, long warningSize)
        {
            base.Client.UpdateQuotas(url, maxSize, warningSize);
        }

        public async System.Threading.Tasks.Task UpdateQuotasAsync(string url, long maxSize, long warningSize)
        {
            await base.Client.UpdateQuotasAsync(url, maxSize, warningSize);
        }

        public SharePointSiteDiskSpace[] CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return base.Client.CalculateSiteCollectionsDiskSpace(urls);
        }

        public async System.Threading.Tasks.Task<SharePointSiteDiskSpace[]> CalculateSiteCollectionsDiskSpaceAsync(string[] urls)
        {
            return await base.Client.CalculateSiteCollectionsDiskSpaceAsync(urls);
        }

        public void DeleteSiteCollection(SharePointSiteCollection siteCollection)
        {
            base.Client.DeleteSiteCollection(siteCollection);
        }

        public async System.Threading.Tasks.Task DeleteSiteCollectionAsync(SharePointSiteCollection siteCollection)
        {
            await base.Client.DeleteSiteCollectionAsync(siteCollection);
        }

        public string BackupSiteCollection(string url, string filename, bool zip)
        {
            return base.Client.BackupSiteCollection(url, filename, zip);
        }

        public async System.Threading.Tasks.Task<string> BackupSiteCollectionAsync(string url, string filename, bool zip)
        {
            return await base.Client.BackupSiteCollectionAsync(url, filename, zip);
        }

        public void RestoreSiteCollection(SharePointSiteCollection siteCollection, string filename)
        {
            base.Client.RestoreSiteCollection(siteCollection, filename);
        }

        public async System.Threading.Tasks.Task RestoreSiteCollectionAsync(SharePointSiteCollection siteCollection, string filename)
        {
            await base.Client.RestoreSiteCollectionAsync(siteCollection, filename);
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

        public long GetSiteCollectionSize(string url)
        {
            return base.Client.GetSiteCollectionSize(url);
        }

        public async System.Threading.Tasks.Task<long> GetSiteCollectionSizeAsync(string url)
        {
            return await base.Client.GetSiteCollectionSizeAsync(url);
        }

        public void SetPeoplePickerOu(string site, string ou)
        {
            base.Client.SetPeoplePickerOu(site, ou);
        }

        public async System.Threading.Tasks.Task SetPeoplePickerOuAsync(string site, string ou)
        {
            await base.Client.SetPeoplePickerOuAsync(site, ou);
        }
    }
}
#endif