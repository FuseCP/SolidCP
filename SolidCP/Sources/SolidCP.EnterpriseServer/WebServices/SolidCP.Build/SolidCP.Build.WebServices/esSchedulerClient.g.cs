#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesScheduler", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesScheduler
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulerTime", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulerTimeResponse")]
        System.DateTime GetSchedulerTime();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulerTime", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulerTimeResponse")]
        System.Threading.Tasks.Task<System.DateTime> GetSchedulerTimeAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTasksResponse")]
        SolidCP.EnterpriseServer.ScheduleTaskInfo[] /*List*/ GetScheduleTasks();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTasksResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleTaskInfo[]> GetScheduleTasksAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedules", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulesResponse")]
        System.Data.DataSet GetSchedules(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedules", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetSchedulesAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulesPagedResponse")]
        System.Data.DataSet GetSchedulesPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedulesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetSchedulesPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleResponse")]
        SolidCP.EnterpriseServer.ScheduleInfo GetSchedule(int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleInfo> GetScheduleAsync(int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleParameters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleParametersResponse")]
        SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[] /*List*/ GetScheduleParameters(string taskId, int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleParameters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleParametersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[]> GetScheduleParametersAsync(string taskId, int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfigurations", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfigurationsResponse")]
        SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[] /*List*/ GetScheduleTaskViewConfigurations(string taskId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfigurations", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfigurationsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[]> GetScheduleTaskViewConfigurationsAsync(string taskId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfigurationResponse")]
        SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration GetScheduleTaskViewConfiguration(string taskId, string environment);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/GetScheduleTaskViewConfigurationResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration> GetScheduleTaskViewConfigurationAsync(string taskId, string environment);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StartSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StartScheduleResponse")]
        int StartSchedule(int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StartSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StartScheduleResponse")]
        System.Threading.Tasks.Task<int> StartScheduleAsync(int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StopSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StopScheduleResponse")]
        int StopSchedule(int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StopSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/StopScheduleResponse")]
        System.Threading.Tasks.Task<int> StopScheduleAsync(int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/AddSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/AddScheduleResponse")]
        int AddSchedule(SolidCP.EnterpriseServer.ScheduleInfo schedule);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/AddSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/AddScheduleResponse")]
        System.Threading.Tasks.Task<int> AddScheduleAsync(SolidCP.EnterpriseServer.ScheduleInfo schedule);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/UpdateSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/UpdateScheduleResponse")]
        int UpdateSchedule(SolidCP.EnterpriseServer.ScheduleInfo schedule);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/UpdateSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/UpdateScheduleResponse")]
        System.Threading.Tasks.Task<int> UpdateScheduleAsync(SolidCP.EnterpriseServer.ScheduleInfo schedule);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/DeleteSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/DeleteScheduleResponse")]
        int DeleteSchedule(int scheduleId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/DeleteSchedule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesScheduler/DeleteScheduleResponse")]
        System.Threading.Tasks.Task<int> DeleteScheduleAsync(int scheduleId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esSchedulerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesScheduler
    {
        public System.DateTime GetSchedulerTime()
        {
            return Invoke<System.DateTime>("SolidCP.EnterpriseServer.esScheduler", "GetSchedulerTime");
        }

        public async System.Threading.Tasks.Task<System.DateTime> GetSchedulerTimeAsync()
        {
            return await InvokeAsync<System.DateTime>("SolidCP.EnterpriseServer.esScheduler", "GetSchedulerTime");
        }

        public SolidCP.EnterpriseServer.ScheduleTaskInfo[] /*List*/ GetScheduleTasks()
        {
            return Invoke<SolidCP.EnterpriseServer.ScheduleTaskInfo[], SolidCP.EnterpriseServer.ScheduleTaskInfo>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleTasks");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleTaskInfo[]> GetScheduleTasksAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ScheduleTaskInfo[], SolidCP.EnterpriseServer.ScheduleTaskInfo>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleTasks");
        }

        public System.Data.DataSet GetSchedules(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esScheduler", "GetSchedules", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSchedulesAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esScheduler", "GetSchedules", userId);
        }

        public System.Data.DataSet GetSchedulesPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esScheduler", "GetSchedulesPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSchedulesPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esScheduler", "GetSchedulesPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.ScheduleInfo GetSchedule(int scheduleId)
        {
            return Invoke<SolidCP.EnterpriseServer.ScheduleInfo>("SolidCP.EnterpriseServer.esScheduler", "GetSchedule", scheduleId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleInfo> GetScheduleAsync(int scheduleId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ScheduleInfo>("SolidCP.EnterpriseServer.esScheduler", "GetSchedule", scheduleId);
        }

        public SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[] /*List*/ GetScheduleParameters(string taskId, int scheduleId)
        {
            return Invoke<SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[], SolidCP.EnterpriseServer.ScheduleTaskParameterInfo>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleParameters", taskId, scheduleId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[]> GetScheduleParametersAsync(string taskId, int scheduleId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[], SolidCP.EnterpriseServer.ScheduleTaskParameterInfo>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleParameters", taskId, scheduleId);
        }

        public SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[] /*List*/ GetScheduleTaskViewConfigurations(string taskId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[], SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleTaskViewConfigurations", taskId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[]> GetScheduleTaskViewConfigurationsAsync(string taskId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[], SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleTaskViewConfigurations", taskId);
        }

        public SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration GetScheduleTaskViewConfiguration(string taskId, string environment)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleTaskViewConfiguration", taskId, environment);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration> GetScheduleTaskViewConfigurationAsync(string taskId, string environment)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration>("SolidCP.EnterpriseServer.esScheduler", "GetScheduleTaskViewConfiguration", taskId, environment);
        }

        public int StartSchedule(int scheduleId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esScheduler", "StartSchedule", scheduleId);
        }

        public async System.Threading.Tasks.Task<int> StartScheduleAsync(int scheduleId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esScheduler", "StartSchedule", scheduleId);
        }

        public int StopSchedule(int scheduleId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esScheduler", "StopSchedule", scheduleId);
        }

        public async System.Threading.Tasks.Task<int> StopScheduleAsync(int scheduleId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esScheduler", "StopSchedule", scheduleId);
        }

        public int AddSchedule(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esScheduler", "AddSchedule", schedule);
        }

        public async System.Threading.Tasks.Task<int> AddScheduleAsync(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esScheduler", "AddSchedule", schedule);
        }

        public int UpdateSchedule(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esScheduler", "UpdateSchedule", schedule);
        }

        public async System.Threading.Tasks.Task<int> UpdateScheduleAsync(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esScheduler", "UpdateSchedule", schedule);
        }

        public int DeleteSchedule(int scheduleId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esScheduler", "DeleteSchedule", scheduleId);
        }

        public async System.Threading.Tasks.Task<int> DeleteScheduleAsync(int scheduleId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esScheduler", "DeleteSchedule", scheduleId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esScheduler : SolidCP.Web.Client.ClientBase<IesScheduler, esSchedulerAssemblyClient>, IesScheduler
    {
        public System.DateTime GetSchedulerTime()
        {
            return base.Client.GetSchedulerTime();
        }

        public async System.Threading.Tasks.Task<System.DateTime> GetSchedulerTimeAsync()
        {
            return await base.Client.GetSchedulerTimeAsync();
        }

        public SolidCP.EnterpriseServer.ScheduleTaskInfo[] /*List*/ GetScheduleTasks()
        {
            return base.Client.GetScheduleTasks();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleTaskInfo[]> GetScheduleTasksAsync()
        {
            return await base.Client.GetScheduleTasksAsync();
        }

        public System.Data.DataSet GetSchedules(int userId)
        {
            return base.Client.GetSchedules(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSchedulesAsync(int userId)
        {
            return await base.Client.GetSchedulesAsync(userId);
        }

        public System.Data.DataSet GetSchedulesPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetSchedulesPaged(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSchedulesPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetSchedulesPagedAsync(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.ScheduleInfo GetSchedule(int scheduleId)
        {
            return base.Client.GetSchedule(scheduleId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleInfo> GetScheduleAsync(int scheduleId)
        {
            return await base.Client.GetScheduleAsync(scheduleId);
        }

        public SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[] /*List*/ GetScheduleParameters(string taskId, int scheduleId)
        {
            return base.Client.GetScheduleParameters(taskId, scheduleId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ScheduleTaskParameterInfo[]> GetScheduleParametersAsync(string taskId, int scheduleId)
        {
            return await base.Client.GetScheduleParametersAsync(taskId, scheduleId);
        }

        public SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[] /*List*/ GetScheduleTaskViewConfigurations(string taskId)
        {
            return base.Client.GetScheduleTaskViewConfigurations(taskId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration[]> GetScheduleTaskViewConfigurationsAsync(string taskId)
        {
            return await base.Client.GetScheduleTaskViewConfigurationsAsync(taskId);
        }

        public SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration GetScheduleTaskViewConfiguration(string taskId, string environment)
        {
            return base.Client.GetScheduleTaskViewConfiguration(taskId, environment);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.Scheduling.ScheduleTaskViewConfiguration> GetScheduleTaskViewConfigurationAsync(string taskId, string environment)
        {
            return await base.Client.GetScheduleTaskViewConfigurationAsync(taskId, environment);
        }

        public int StartSchedule(int scheduleId)
        {
            return base.Client.StartSchedule(scheduleId);
        }

        public async System.Threading.Tasks.Task<int> StartScheduleAsync(int scheduleId)
        {
            return await base.Client.StartScheduleAsync(scheduleId);
        }

        public int StopSchedule(int scheduleId)
        {
            return base.Client.StopSchedule(scheduleId);
        }

        public async System.Threading.Tasks.Task<int> StopScheduleAsync(int scheduleId)
        {
            return await base.Client.StopScheduleAsync(scheduleId);
        }

        public int AddSchedule(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return base.Client.AddSchedule(schedule);
        }

        public async System.Threading.Tasks.Task<int> AddScheduleAsync(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return await base.Client.AddScheduleAsync(schedule);
        }

        public int UpdateSchedule(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return base.Client.UpdateSchedule(schedule);
        }

        public async System.Threading.Tasks.Task<int> UpdateScheduleAsync(SolidCP.EnterpriseServer.ScheduleInfo schedule)
        {
            return await base.Client.UpdateScheduleAsync(schedule);
        }

        public int DeleteSchedule(int scheduleId)
        {
            return base.Client.DeleteSchedule(scheduleId);
        }

        public async System.Threading.Tasks.Task<int> DeleteScheduleAsync(int scheduleId)
        {
            return await base.Client.DeleteScheduleAsync(scheduleId);
        }
    }
}
#endif