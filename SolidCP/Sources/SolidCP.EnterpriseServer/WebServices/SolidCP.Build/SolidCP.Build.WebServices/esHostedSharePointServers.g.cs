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
    public interface IesHostedSharePointServers
    {
        [WebMethod]
        [OperationContract]
        SharePointSiteCollectionListPaged GetSiteCollectionsPaged(int packageId, int organizationId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        int[] GetSupportedLanguages(int packageId);
        [WebMethod]
        [OperationContract]
        List<SharePointSiteCollection> GetSiteCollections(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        int SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections);
        [WebMethod]
        [OperationContract]
        SharePointSiteCollection GetSiteCollection(int itemId);
        [WebMethod]
        [OperationContract]
        SharePointSiteCollection GetSiteCollectionByDomain(int organizationId, string domain);
        [WebMethod]
        [OperationContract]
        int AddSiteCollection(SharePointSiteCollection item);
        [WebMethod]
        [OperationContract]
        int DeleteSiteCollection(int itemId);
        [WebMethod]
        [OperationContract]
        int DeleteSiteCollections(int organizationId);
        [WebMethod]
        [OperationContract]
        string BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [WebMethod]
        [OperationContract]
        int RestoreSiteCollection(int itemId, string uploadedFile, string packageFile);
        [WebMethod]
        [OperationContract]
        byte[] GetBackupBinaryChunk(int itemId, string path, int offset, int length);
        [WebMethod]
        [OperationContract]
        string AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [WebMethod]
        [OperationContract]
        SharePointSiteDiskSpace[] CalculateSharePointSitesDiskSpace(int itemId, out int errorCode);
        [WebMethod]
        [OperationContract]
        void UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esHostedSharePointServers : SolidCP.EnterpriseServer.esHostedSharePointServers, IesHostedSharePointServers
    {
    }
}
#endif