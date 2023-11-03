#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.FTP;
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
    public interface IesFtpServers
    {
        [WebMethod]
        [OperationContract]
        FtpSite[] GetFtpSites(int serviceId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawFtpAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<FtpAccount> GetFtpAccounts(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        FtpAccount GetFtpAccount(int itemId);
        [WebMethod]
        [OperationContract]
        int AddFtpAccount(FtpAccount item);
        [WebMethod]
        [OperationContract]
        int UpdateFtpAccount(FtpAccount item);
        [WebMethod]
        [OperationContract]
        int DeleteFtpAccount(int itemId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esFtpServers : SolidCP.EnterpriseServer.esFtpServers, IesFtpServers
    {
    }
}
#endif