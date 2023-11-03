#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.Statistics;
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
    public interface IesStatisticsServers
    {
        [WebMethod]
        [OperationContract]
        DataSet GetRawStatisticsSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<StatsSite> GetStatisticsSites(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        StatsServer[] GetServers(int serviceId);
        [WebMethod]
        [OperationContract]
        StatsSite GetSite(int itemId);
        [WebMethod]
        [OperationContract]
        int AddSite(StatsSite item);
        [WebMethod]
        [OperationContract]
        int UpdateSite(StatsSite item);
        [WebMethod]
        [OperationContract]
        int DeleteSite(int itemId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esStatisticsServers : SolidCP.EnterpriseServer.esStatisticsServers, IesStatisticsServers
    {
    }
}
#endif