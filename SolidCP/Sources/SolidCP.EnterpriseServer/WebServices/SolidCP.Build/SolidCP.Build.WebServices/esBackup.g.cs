#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
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
    public interface IesBackup
    {
        [WebMethod]
        [OperationContract]
        KeyValueBunch GetBackupContentSummary(int userId, int packageId, int serviceId, int serverId);
        [WebMethod]
        [OperationContract]
        int Backup(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder, bool deleteTempBackup);
        [WebMethod]
        [OperationContract]
        int Restore(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, int storePackageId, string storePackageBackupPath, string storeServerBackupPath);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esBackup : SolidCP.EnterpriseServer.esBackup, IesBackup
    {
    }
}
#endif