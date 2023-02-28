#if !Client
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

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
    public interface IHostedSharePointServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int[] GetSupportedLanguages();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointSiteCollection[] GetSiteCollections();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointSiteCollection GetSiteCollection(string url);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateSiteCollection(SharePointSiteCollection siteCollection);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateQuotas(string url, long maxSize, long warningSize);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointSiteDiskSpace[] CalculateSiteCollectionsDiskSpace(string[] urls);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteSiteCollection(SharePointSiteCollection siteCollection);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string BackupSiteCollection(string url, string filename, bool zip);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RestoreSiteCollection(SharePointSiteCollection siteCollection, string filename);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        long GetSiteCollectionSize(string url);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetPeoplePickerOu(string site, string ou);
    }

    // wcf service
    public class HostedSharePointServerService : HostedSharePointServer, IHostedSharePointServer
    {
        public new int[] GetSupportedLanguages()
        {
            return base.GetSupportedLanguages();
        }

        public new SharePointSiteCollection[] GetSiteCollections()
        {
            return base.GetSiteCollections();
        }

        public new SharePointSiteCollection GetSiteCollection(string url)
        {
            return base.GetSiteCollection(url);
        }

        public new void CreateSiteCollection(SharePointSiteCollection siteCollection)
        {
            base.CreateSiteCollection(siteCollection);
        }

        public new void UpdateQuotas(string url, long maxSize, long warningSize)
        {
            base.UpdateQuotas(url, maxSize, warningSize);
        }

        public new SharePointSiteDiskSpace[] CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return base.CalculateSiteCollectionsDiskSpace(urls);
        }

        public new void DeleteSiteCollection(SharePointSiteCollection siteCollection)
        {
            base.DeleteSiteCollection(siteCollection);
        }

        public new string BackupSiteCollection(string url, string filename, bool zip)
        {
            return base.BackupSiteCollection(url, filename, zip);
        }

        public new void RestoreSiteCollection(SharePointSiteCollection siteCollection, string filename)
        {
            base.RestoreSiteCollection(siteCollection, filename);
        }

        public new byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.GetTempFileBinaryChunk(path, offset, length);
        }

        public new string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public new long GetSiteCollectionSize(string url)
        {
            return base.GetSiteCollectionSize(url);
        }

        public new void SetPeoplePickerOu(string site, string ou)
        {
            base.SetPeoplePickerOu(site, ou);
        }
    }
}
#endif