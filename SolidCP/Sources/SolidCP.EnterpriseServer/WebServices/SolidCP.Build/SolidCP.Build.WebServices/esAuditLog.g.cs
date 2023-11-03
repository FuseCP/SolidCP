#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
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
    public interface IesAuditLog
    {
        [WebMethod]
        [OperationContract]
        DataSet GetAuditLogRecordsPaged(int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate, int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetAuditLogSources();
        [WebMethod]
        [OperationContract]
        DataSet GetAuditLogTasks(string sourceName);
        [WebMethod]
        [OperationContract]
        LogRecord GetAuditLogRecord(string recordId);
        [WebMethod]
        [OperationContract]
        int DeleteAuditLogRecords(int userId, int itemId, string itemName, DateTime startDate, DateTime endDate, int severityId, string sourceName, string taskName);
        [WebMethod]
        [OperationContract]
        int DeleteAuditLogRecordsComplete();
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esAuditLog : SolidCP.EnterpriseServer.esAuditLog, IesAuditLog
    {
    }
}
#endif