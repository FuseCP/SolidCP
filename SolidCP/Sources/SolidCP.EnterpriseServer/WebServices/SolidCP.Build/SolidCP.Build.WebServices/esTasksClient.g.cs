#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesTasks", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesTasks
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTaskResponse")]
        SolidCP.EnterpriseServer.BackgroundTask GetTask(string taskId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTaskResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask> GetTaskAsync(string taskId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTaskWithLogRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTaskWithLogRecordsResponse")]
        SolidCP.EnterpriseServer.BackgroundTask GetTaskWithLogRecords(string taskId, System.DateTime startLogTime);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTaskWithLogRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTaskWithLogRecordsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask> GetTaskWithLogRecordsAsync(string taskId, System.DateTime startLogTime);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTasksNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTasksNumberResponse")]
        int GetTasksNumber();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTasksNumber", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetTasksNumberResponse")]
        System.Threading.Tasks.Task<int> GetTasksNumberAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserTasksResponse")]
        SolidCP.EnterpriseServer.BackgroundTask[] /*List*/ GetUserTasks(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserTasksResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask[]> GetUserTasksAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserCompletedTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserCompletedTasksResponse")]
        SolidCP.EnterpriseServer.BackgroundTask[] /*List*/ GetUserCompletedTasks(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserCompletedTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/GetUserCompletedTasksResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask[]> GetUserCompletedTasksAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/SetTaskNotifyOnComplete", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/SetTaskNotifyOnCompleteResponse")]
        void SetTaskNotifyOnComplete(string taskId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/SetTaskNotifyOnComplete", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/SetTaskNotifyOnCompleteResponse")]
        System.Threading.Tasks.Task SetTaskNotifyOnCompleteAsync(string taskId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/StopTask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/StopTaskResponse")]
        void StopTask(string taskId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesTasks/StopTask", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesTasks/StopTaskResponse")]
        System.Threading.Tasks.Task StopTaskAsync(string taskId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esTasksAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesTasks
    {
        public SolidCP.EnterpriseServer.BackgroundTask GetTask(string taskId)
        {
            return Invoke<SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetTask", taskId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask> GetTaskAsync(string taskId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetTask", taskId);
        }

        public SolidCP.EnterpriseServer.BackgroundTask GetTaskWithLogRecords(string taskId, System.DateTime startLogTime)
        {
            return Invoke<SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetTaskWithLogRecords", taskId, startLogTime);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask> GetTaskWithLogRecordsAsync(string taskId, System.DateTime startLogTime)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetTaskWithLogRecords", taskId, startLogTime);
        }

        public int GetTasksNumber()
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esTasks", "GetTasksNumber");
        }

        public async System.Threading.Tasks.Task<int> GetTasksNumberAsync()
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esTasks", "GetTasksNumber");
        }

        public SolidCP.EnterpriseServer.BackgroundTask[] /*List*/ GetUserTasks(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.BackgroundTask[], SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetUserTasks", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask[]> GetUserTasksAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.BackgroundTask[], SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetUserTasks", userId);
        }

        public SolidCP.EnterpriseServer.BackgroundTask[] /*List*/ GetUserCompletedTasks(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.BackgroundTask[], SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetUserCompletedTasks", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask[]> GetUserCompletedTasksAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.BackgroundTask[], SolidCP.EnterpriseServer.BackgroundTask>("SolidCP.EnterpriseServer.esTasks", "GetUserCompletedTasks", userId);
        }

        public void SetTaskNotifyOnComplete(string taskId)
        {
            Invoke("SolidCP.EnterpriseServer.esTasks", "SetTaskNotifyOnComplete", taskId);
        }

        public async System.Threading.Tasks.Task SetTaskNotifyOnCompleteAsync(string taskId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esTasks", "SetTaskNotifyOnComplete", taskId);
        }

        public void StopTask(string taskId)
        {
            Invoke("SolidCP.EnterpriseServer.esTasks", "StopTask", taskId);
        }

        public async System.Threading.Tasks.Task StopTaskAsync(string taskId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esTasks", "StopTask", taskId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esTasks : SolidCP.Web.Client.ClientBase<IesTasks, esTasksAssemblyClient>, IesTasks
    {
        public SolidCP.EnterpriseServer.BackgroundTask GetTask(string taskId)
        {
            return base.Client.GetTask(taskId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask> GetTaskAsync(string taskId)
        {
            return await base.Client.GetTaskAsync(taskId);
        }

        public SolidCP.EnterpriseServer.BackgroundTask GetTaskWithLogRecords(string taskId, System.DateTime startLogTime)
        {
            return base.Client.GetTaskWithLogRecords(taskId, startLogTime);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask> GetTaskWithLogRecordsAsync(string taskId, System.DateTime startLogTime)
        {
            return await base.Client.GetTaskWithLogRecordsAsync(taskId, startLogTime);
        }

        public int GetTasksNumber()
        {
            return base.Client.GetTasksNumber();
        }

        public async System.Threading.Tasks.Task<int> GetTasksNumberAsync()
        {
            return await base.Client.GetTasksNumberAsync();
        }

        public SolidCP.EnterpriseServer.BackgroundTask[] /*List*/ GetUserTasks(int userId)
        {
            return base.Client.GetUserTasks(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask[]> GetUserTasksAsync(int userId)
        {
            return await base.Client.GetUserTasksAsync(userId);
        }

        public SolidCP.EnterpriseServer.BackgroundTask[] /*List*/ GetUserCompletedTasks(int userId)
        {
            return base.Client.GetUserCompletedTasks(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.BackgroundTask[]> GetUserCompletedTasksAsync(int userId)
        {
            return await base.Client.GetUserCompletedTasksAsync(userId);
        }

        public void SetTaskNotifyOnComplete(string taskId)
        {
            base.Client.SetTaskNotifyOnComplete(taskId);
        }

        public async System.Threading.Tasks.Task SetTaskNotifyOnCompleteAsync(string taskId)
        {
            await base.Client.SetTaskNotifyOnCompleteAsync(taskId);
        }

        public void StopTask(string taskId)
        {
            base.Client.StopTask(taskId);
        }

        public async System.Threading.Tasks.Task StopTaskAsync(string taskId)
        {
            await base.Client.StopTaskAsync(taskId);
        }
    }
}
#endif