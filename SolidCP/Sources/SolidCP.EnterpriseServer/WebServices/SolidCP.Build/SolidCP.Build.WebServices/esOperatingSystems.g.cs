#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.OS;
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
    public interface IesOperatingSystems
    {
        [WebMethod]
        [OperationContract]
        DataSet GetRawOdbcSourcesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        string[] GetInstalledOdbcDrivers(int packageId);
        [WebMethod]
        [OperationContract]
        List<SystemDSN> GetOdbcSources(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        SystemDSN GetOdbcSource(int itemId);
        [WebMethod]
        [OperationContract]
        int AddOdbcSource(SystemDSN item);
        [WebMethod]
        [OperationContract]
        int UpdateOdbcSource(SystemDSN item);
        [WebMethod]
        [OperationContract]
        int DeleteOdbcSource(int itemId);
        [WebMethod]
        [OperationContract]
        bool CheckFileServicesInstallation(int serviceId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esOperatingSystems : SolidCP.EnterpriseServer.esOperatingSystems, IesOperatingSystems
    {
    }
}
#endif