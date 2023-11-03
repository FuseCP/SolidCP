#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.EnterpriseServer.Code.SharePoint;
using SolidCP.Providers.SharePoint;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesHostedSharePointServersEnt
    {
        [WebMethod]
        [OperationContract]
        SharePointEnterpriseSiteCollectionListPaged Enterprise_GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        int[] Enterprise_GetSupportedLanguages(int packageId);
        [WebMethod]
        [OperationContract]
        List<SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollections(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        int Enterprise_SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections);
        [WebMethod]
        [OperationContract]
        SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(int itemId);
        [WebMethod]
        [OperationContract]
        SharePointEnterpriseSiteCollection Enterprise_GetSiteCollectionByDomain(int organizationId, string domain);
        [WebMethod]
        [OperationContract]
        int Enterprise_AddSiteCollection(SharePointEnterpriseSiteCollection item);
        [WebMethod]
        [OperationContract]
        int Enterprise_DeleteSiteCollection(int itemId);
        [WebMethod]
        [OperationContract]
        int Enterprise_DeleteSiteCollections(int organizationId);
        [WebMethod]
        [OperationContract]
        string Enterprise_BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [WebMethod]
        [OperationContract]
        int Enterprise_RestoreSiteCollection(int itemId, string uploadedFile, string packageFile);
        [WebMethod]
        [OperationContract]
        byte[] Enterprise_GetBackupBinaryChunk(int itemId, string path, int offset, int length);
        [WebMethod]
        [OperationContract]
        string Enterprise_AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [WebMethod]
        [OperationContract]
        SharePointSiteDiskSpace[] Enterprise_CalculateSharePointSitesDiskSpace(int itemId, out int errorCode);
        [WebMethod]
        [OperationContract]
        void Enterprise_UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esHostedSharePointServersEnt : SolidCP.EnterpriseServer.esHostedSharePointServersEnt, IesHostedSharePointServersEnt
    {
    }
}
#endif