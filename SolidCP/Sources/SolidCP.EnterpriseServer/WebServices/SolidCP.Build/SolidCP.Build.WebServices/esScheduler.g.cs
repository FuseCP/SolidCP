#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.EnterpriseServer.Base.Scheduling;
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
    public interface IesScheduler
    {
        [WebMethod]
        [OperationContract]
        DateTime GetSchedulerTime();
        [WebMethod]
        [OperationContract]
        List<ScheduleTaskInfo> GetScheduleTasks();
        [WebMethod]
        [OperationContract]
        DataSet GetSchedules(int userId);
        [WebMethod]
        [OperationContract]
        DataSet GetSchedulesPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        ScheduleInfo GetSchedule(int scheduleId);
        [WebMethod]
        [OperationContract]
        List<ScheduleTaskParameterInfo> GetScheduleParameters(string taskId, int scheduleId);
        [WebMethod]
        [OperationContract]
        List<ScheduleTaskViewConfiguration> GetScheduleTaskViewConfigurations(string taskId);
        [WebMethod]
        [OperationContract]
        ScheduleTaskViewConfiguration GetScheduleTaskViewConfiguration(string taskId, string environment);
        [WebMethod]
        [OperationContract]
        int StartSchedule(int scheduleId);
        [WebMethod]
        [OperationContract]
        int StopSchedule(int scheduleId);
        [WebMethod]
        [OperationContract]
        int AddSchedule(ScheduleInfo schedule);
        [WebMethod]
        [OperationContract]
        int UpdateSchedule(ScheduleInfo schedule);
        [WebMethod]
        [OperationContract]
        int DeleteSchedule(int scheduleId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esScheduler : SolidCP.EnterpriseServer.esScheduler, IesScheduler
    {
    }
}
#endif