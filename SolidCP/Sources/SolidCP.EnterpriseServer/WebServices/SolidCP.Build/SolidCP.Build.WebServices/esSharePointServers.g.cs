#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.OS;
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
    public interface IesSharePointServers
    {
        [WebMethod]
        [OperationContract]
        DataSet GetRawSharePointSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<SharePointSite> GetSharePointSites(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        SharePointSite GetSharePointSite(int itemId);
        [WebMethod]
        [OperationContract]
        int AddSharePointSite(SharePointSite item);
        [WebMethod]
        [OperationContract]
        int DeleteSharePointSite(int itemId);
        [WebMethod]
        [OperationContract]
        string BackupVirtualServer(int itemId, string fileName, bool zipBackup, bool download, string folderName);
        [WebMethod]
        [OperationContract]
        byte[] GetSharePointBackupBinaryChunk(int itemId, string path, int offset, int length);
        [WebMethod]
        [OperationContract]
        string AppendSharePointBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [WebMethod]
        [OperationContract]
        int RestoreVirtualServer(int itemId, string uploadedFile, string packageFile);
        [WebMethod]
        [OperationContract]
        string[] GetInstalledWebParts(int itemId);
        [WebMethod]
        [OperationContract]
        int InstallWebPartsPackage(int itemId, string uploadedFile, string packageFile);
        [WebMethod]
        [OperationContract]
        int DeleteWebPartsPackage(int itemId, string packageName);
        [WebMethod]
        [OperationContract]
        DataSet GetRawSharePointUsersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<SystemUser> GetSharePointUsers(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        SystemUser GetSharePointUser(int itemId);
        [WebMethod]
        [OperationContract]
        int AddSharePointUser(SystemUser item);
        [WebMethod]
        [OperationContract]
        int UpdateSharePointUser(SystemUser item);
        [WebMethod]
        [OperationContract]
        int DeleteSharePointUser(int itemId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawSharePointGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<SystemGroup> GetSharePointGroups(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        SystemGroup GetSharePointGroup(int itemId);
        [WebMethod]
        [OperationContract]
        int AddSharePointGroup(SystemGroup item);
        [WebMethod]
        [OperationContract]
        int UpdateSharePointGroup(SystemGroup item);
        [WebMethod]
        [OperationContract]
        int DeleteSharePointGroup(int itemId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esSharePointServers : SolidCP.EnterpriseServer.esSharePointServers, IesSharePointServers
    {
    }
}
#endif