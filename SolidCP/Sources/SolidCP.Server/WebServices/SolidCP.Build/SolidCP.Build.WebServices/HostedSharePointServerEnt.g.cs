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
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class HostedSharePointServerEnt : SolidCP.Server.HostedSharePointServerEnt, IHostedSharePointServerEnt
    {
    }
}
#endif