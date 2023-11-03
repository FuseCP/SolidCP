#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesBackup", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesBackup
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBackup/GetBackupContentSummary", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBackup/GetBackupContentSummaryResponse")]
        SolidCP.EnterpriseServer.KeyValueBunch GetBackupContentSummary(int userId, int packageId, int serviceId, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBackup/GetBackupContentSummary", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBackup/GetBackupContentSummaryResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.KeyValueBunch> GetBackupContentSummaryAsync(int userId, int packageId, int serviceId, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBackup/Backup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBackup/BackupResponse")]
        int Backup(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder, bool deleteTempBackup);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBackup/Backup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBackup/BackupResponse")]
        System.Threading.Tasks.Task<int> BackupAsync(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder, bool deleteTempBackup);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBackup/Restore", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBackup/RestoreResponse")]
        int Restore(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, int storePackageId, string storePackageBackupPath, string storeServerBackupPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesBackup/Restore", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesBackup/RestoreResponse")]
        System.Threading.Tasks.Task<int> RestoreAsync(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, int storePackageId, string storePackageBackupPath, string storeServerBackupPath);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esBackupAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesBackup
    {
        public SolidCP.EnterpriseServer.KeyValueBunch GetBackupContentSummary(int userId, int packageId, int serviceId, int serverId)
        {
            return Invoke<SolidCP.EnterpriseServer.KeyValueBunch>("SolidCP.EnterpriseServer.esBackup", "GetBackupContentSummary", userId, packageId, serviceId, serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.KeyValueBunch> GetBackupContentSummaryAsync(int userId, int packageId, int serviceId, int serverId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.KeyValueBunch>("SolidCP.EnterpriseServer.esBackup", "GetBackupContentSummary", userId, packageId, serviceId, serverId);
        }

        public int Backup(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder, bool deleteTempBackup)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esBackup", "Backup", async, taskId, userId, packageId, serviceId, serverId, backupFileName, storePackageId, storePackageFolder, storeServerFolder, deleteTempBackup);
        }

        public async System.Threading.Tasks.Task<int> BackupAsync(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder, bool deleteTempBackup)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esBackup", "Backup", async, taskId, userId, packageId, serviceId, serverId, backupFileName, storePackageId, storePackageFolder, storeServerFolder, deleteTempBackup);
        }

        public int Restore(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, int storePackageId, string storePackageBackupPath, string storeServerBackupPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esBackup", "Restore", async, taskId, userId, packageId, serviceId, serverId, storePackageId, storePackageBackupPath, storeServerBackupPath);
        }

        public async System.Threading.Tasks.Task<int> RestoreAsync(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, int storePackageId, string storePackageBackupPath, string storeServerBackupPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esBackup", "Restore", async, taskId, userId, packageId, serviceId, serverId, storePackageId, storePackageBackupPath, storeServerBackupPath);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esBackup : SolidCP.Web.Client.ClientBase<IesBackup, esBackupAssemblyClient>, IesBackup
    {
        public SolidCP.EnterpriseServer.KeyValueBunch GetBackupContentSummary(int userId, int packageId, int serviceId, int serverId)
        {
            return base.Client.GetBackupContentSummary(userId, packageId, serviceId, serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.KeyValueBunch> GetBackupContentSummaryAsync(int userId, int packageId, int serviceId, int serverId)
        {
            return await base.Client.GetBackupContentSummaryAsync(userId, packageId, serviceId, serverId);
        }

        public int Backup(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder, bool deleteTempBackup)
        {
            return base.Client.Backup(async, taskId, userId, packageId, serviceId, serverId, backupFileName, storePackageId, storePackageFolder, storeServerFolder, deleteTempBackup);
        }

        public async System.Threading.Tasks.Task<int> BackupAsync(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, string backupFileName, int storePackageId, string storePackageFolder, string storeServerFolder, bool deleteTempBackup)
        {
            return await base.Client.BackupAsync(async, taskId, userId, packageId, serviceId, serverId, backupFileName, storePackageId, storePackageFolder, storeServerFolder, deleteTempBackup);
        }

        public int Restore(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, int storePackageId, string storePackageBackupPath, string storeServerBackupPath)
        {
            return base.Client.Restore(async, taskId, userId, packageId, serviceId, serverId, storePackageId, storePackageBackupPath, storeServerBackupPath);
        }

        public async System.Threading.Tasks.Task<int> RestoreAsync(bool async, string taskId, int userId, int packageId, int serviceId, int serverId, int storePackageId, string storePackageBackupPath, string storeServerBackupPath)
        {
            return await base.Client.RestoreAsync(async, taskId, userId, packageId, serviceId, serverId, storePackageId, storePackageBackupPath, storeServerBackupPath);
        }
    }
}
#endif