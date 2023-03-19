#if !Client
using System;
using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.SharePoint;
using SolidCP.Server.Utils;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class HostedSharePointServer : SolidCP.Server.HostedSharePointServer, IHostedSharePointServer
    {
    }
}
#endif