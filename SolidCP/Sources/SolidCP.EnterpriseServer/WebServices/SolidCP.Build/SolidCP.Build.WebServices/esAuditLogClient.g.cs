#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesAuditLog", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesAuditLog
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecordsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecordsPagedResponse")]
        System.Data.DataSet GetAuditLogRecordsPaged(int userId, int packageId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecordsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecordsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogRecordsPagedAsync(int userId, int packageId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogSources", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogSourcesResponse")]
        System.Data.DataSet GetAuditLogSources();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogSources", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogSourcesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogSourcesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogTasksResponse")]
        System.Data.DataSet GetAuditLogTasks(string sourceName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogTasks", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogTasksResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogTasksAsync(string sourceName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecordResponse")]
        SolidCP.EnterpriseServer.LogRecord GetAuditLogRecord(string recordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecord", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/GetAuditLogRecordResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord> GetAuditLogRecordAsync(string recordId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecordsResponse")]
        int DeleteAuditLogRecords(int userId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecords", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecordsResponse")]
        System.Threading.Tasks.Task<int> DeleteAuditLogRecordsAsync(int userId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecordsComplete", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecordsCompleteResponse")]
        int DeleteAuditLogRecordsComplete();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecordsComplete", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesAuditLog/DeleteAuditLogRecordsCompleteResponse")]
        System.Threading.Tasks.Task<int> DeleteAuditLogRecordsCompleteAsync();
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esAuditLogAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesAuditLog
    {
        public System.Data.DataSet GetAuditLogRecordsPaged(int userId, int packageId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogRecordsPaged", userId, packageId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogRecordsPagedAsync(int userId, int packageId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogRecordsPaged", userId, packageId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetAuditLogSources()
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogSources");
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogSourcesAsync()
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogSources");
        }

        public System.Data.DataSet GetAuditLogTasks(string sourceName)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogTasks", sourceName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogTasksAsync(string sourceName)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogTasks", sourceName);
        }

        public SolidCP.EnterpriseServer.LogRecord GetAuditLogRecord(string recordId)
        {
            return Invoke<SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogRecord", recordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord> GetAuditLogRecordAsync(string recordId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.LogRecord>("SolidCP.EnterpriseServer.esAuditLog", "GetAuditLogRecord", recordId);
        }

        public int DeleteAuditLogRecords(int userId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esAuditLog", "DeleteAuditLogRecords", userId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName);
        }

        public async System.Threading.Tasks.Task<int> DeleteAuditLogRecordsAsync(int userId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esAuditLog", "DeleteAuditLogRecords", userId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName);
        }

        public int DeleteAuditLogRecordsComplete()
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esAuditLog", "DeleteAuditLogRecordsComplete");
        }

        public async System.Threading.Tasks.Task<int> DeleteAuditLogRecordsCompleteAsync()
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esAuditLog", "DeleteAuditLogRecordsComplete");
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esAuditLog : SolidCP.Web.Client.ClientBase<IesAuditLog, esAuditLogAssemblyClient>, IesAuditLog
    {
        public System.Data.DataSet GetAuditLogRecordsPaged(int userId, int packageId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetAuditLogRecordsPaged(userId, packageId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogRecordsPagedAsync(int userId, int packageId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetAuditLogRecordsPagedAsync(userId, packageId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetAuditLogSources()
        {
            return base.Client.GetAuditLogSources();
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogSourcesAsync()
        {
            return await base.Client.GetAuditLogSourcesAsync();
        }

        public System.Data.DataSet GetAuditLogTasks(string sourceName)
        {
            return base.Client.GetAuditLogTasks(sourceName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetAuditLogTasksAsync(string sourceName)
        {
            return await base.Client.GetAuditLogTasksAsync(sourceName);
        }

        public SolidCP.EnterpriseServer.LogRecord GetAuditLogRecord(string recordId)
        {
            return base.Client.GetAuditLogRecord(recordId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.LogRecord> GetAuditLogRecordAsync(string recordId)
        {
            return await base.Client.GetAuditLogRecordAsync(recordId);
        }

        public int DeleteAuditLogRecords(int userId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName)
        {
            return base.Client.DeleteAuditLogRecords(userId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName);
        }

        public async System.Threading.Tasks.Task<int> DeleteAuditLogRecordsAsync(int userId, int itemId, string itemName, System.DateTime startDate, System.DateTime endDate, int severityId, string sourceName, string taskName)
        {
            return await base.Client.DeleteAuditLogRecordsAsync(userId, itemId, itemName, startDate, endDate, severityId, sourceName, taskName);
        }

        public int DeleteAuditLogRecordsComplete()
        {
            return base.Client.DeleteAuditLogRecordsComplete();
        }

        public async System.Threading.Tasks.Task<int> DeleteAuditLogRecordsCompleteAsync()
        {
            return await base.Client.DeleteAuditLogRecordsCompleteAsync();
        }
    }
}
#endif