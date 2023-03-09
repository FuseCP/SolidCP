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
using System.ServiceModel.Activation;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IHostedSharePointServerEnt
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int[] Enterprise_GetSupportedLanguages();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_CreateSiteCollection(SharePointEnterpriseSiteCollection siteCollection);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection siteCollection);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string Enterprise_BackupSiteCollection(string url, string filename, bool zip);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_RestoreSiteCollection(SharePointEnterpriseSiteCollection siteCollection, string filename);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        long Enterprise_GetSiteCollectionSize(string url);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void Enterprise_SetPeoplePickerOu(string site, string ou);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HostedSharePointServerEnt : SolidCP.Server.HostedSharePointServerEnt, IHostedSharePointServerEnt
    {
        public new int[] Enterprise_GetSupportedLanguages()
        {
            return base.Enterprise_GetSupportedLanguages();
        }

        public new SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections()
        {
            return base.Enterprise_GetSiteCollections();
        }

        public new SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url)
        {
            return base.Enterprise_GetSiteCollection(url);
        }

        public new void Enterprise_CreateSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            base.Enterprise_CreateSiteCollection(siteCollection);
        }

        public new void Enterprise_UpdateQuotas(string url, long maxSize, long warningSize)
        {
            base.Enterprise_UpdateQuotas(url, maxSize, warningSize);
        }

        public new SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return base.Enterprise_CalculateSiteCollectionsDiskSpace(urls);
        }

        public new void Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            base.Enterprise_DeleteSiteCollection(siteCollection);
        }

        public new string Enterprise_BackupSiteCollection(string url, string filename, bool zip)
        {
            return base.Enterprise_BackupSiteCollection(url, filename, zip);
        }

        public new void Enterprise_RestoreSiteCollection(SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            base.Enterprise_RestoreSiteCollection(siteCollection, filename);
        }

        public new byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.Enterprise_GetTempFileBinaryChunk(path, offset, length);
        }

        public new string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.Enterprise_AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public new long Enterprise_GetSiteCollectionSize(string url)
        {
            return base.Enterprise_GetSiteCollectionSize(url);
        }

        public new void Enterprise_SetPeoplePickerOu(string site, string ou)
        {
            base.Enterprise_SetPeoplePickerOu(site, ou);
        }
    }
}
#endif