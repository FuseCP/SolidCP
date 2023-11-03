#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesDatabaseServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesDatabaseServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlDatabasesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlDatabasesPagedResponse")]
        System.Data.DataSet GetRawSqlDatabasesPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlDatabasesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlDatabasesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawSqlDatabasesPagedAsync(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabases", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabasesResponse")]
        SolidCP.Providers.Database.SqlDatabase[] /*List*/ GetSqlDatabases(int packageId, string groupName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabases", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabasesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase[]> GetSqlDatabasesAsync(int packageId, string groupName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabaseResponse")]
        SolidCP.Providers.Database.SqlDatabase GetSqlDatabase(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlDatabaseResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase> GetSqlDatabaseAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlDatabaseResponse")]
        int AddSqlDatabase(SolidCP.Providers.Database.SqlDatabase item, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlDatabaseResponse")]
        System.Threading.Tasks.Task<int> AddSqlDatabaseAsync(SolidCP.Providers.Database.SqlDatabase item, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlDatabaseResponse")]
        int UpdateSqlDatabase(SolidCP.Providers.Database.SqlDatabase item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlDatabaseResponse")]
        System.Threading.Tasks.Task<int> UpdateSqlDatabaseAsync(SolidCP.Providers.Database.SqlDatabase item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlDatabaseResponse")]
        int DeleteSqlDatabase(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlDatabaseResponse")]
        System.Threading.Tasks.Task<int> DeleteSqlDatabaseAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/BackupSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/BackupSqlDatabaseResponse")]
        string BackupSqlDatabase(int itemId, string backupName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/BackupSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/BackupSqlDatabaseResponse")]
        System.Threading.Tasks.Task<string> BackupSqlDatabaseAsync(int itemId, string backupName, bool zipBackup, bool download, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlBackupBinaryChunkResponse")]
        byte[] GetSqlBackupBinaryChunk(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetSqlBackupBinaryChunkAsync(int itemId, string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AppendSqlBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AppendSqlBackupBinaryChunkResponse")]
        string AppendSqlBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AppendSqlBackupBinaryChunk", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AppendSqlBackupBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> AppendSqlBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/RestoreSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/RestoreSqlDatabaseResponse")]
        int RestoreSqlDatabase(int itemId, string[] uploadedFiles, string[] packageFiles);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/RestoreSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/RestoreSqlDatabaseResponse")]
        System.Threading.Tasks.Task<int> RestoreSqlDatabaseAsync(int itemId, string[] uploadedFiles, string[] packageFiles);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/TruncateSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/TruncateSqlDatabaseResponse")]
        int TruncateSqlDatabase(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/TruncateSqlDatabase", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/TruncateSqlDatabaseResponse")]
        System.Threading.Tasks.Task<int> TruncateSqlDatabaseAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserConfigurationResponse")]
        SolidCP.EnterpriseServer.DatabaseBrowserConfiguration GetDatabaseBrowserConfiguration(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserConfiguration", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserConfigurationResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration> GetDatabaseBrowserConfigurationAsync(int packageId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserLogonScript", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserLogonScriptResponse")]
        SolidCP.EnterpriseServer.DatabaseBrowserConfiguration GetDatabaseBrowserLogonScript(int packageId, string groupName, string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserLogonScript", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetDatabaseBrowserLogonScriptResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration> GetDatabaseBrowserLogonScriptAsync(int packageId, string groupName, string username);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlUsersPagedResponse")]
        System.Data.DataSet GetRawSqlUsersPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlUsersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetRawSqlUsersPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawSqlUsersPagedAsync(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUsersResponse")]
        SolidCP.Providers.Database.SqlUser[] /*List*/ GetSqlUsers(int packageId, string groupName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser[]> GetSqlUsersAsync(int packageId, string groupName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUserResponse")]
        SolidCP.Providers.Database.SqlUser GetSqlUser(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/GetSqlUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser> GetSqlUserAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlUserResponse")]
        int AddSqlUser(SolidCP.Providers.Database.SqlUser item, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/AddSqlUserResponse")]
        System.Threading.Tasks.Task<int> AddSqlUserAsync(SolidCP.Providers.Database.SqlUser item, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlUserResponse")]
        int UpdateSqlUser(SolidCP.Providers.Database.SqlUser item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/UpdateSqlUserResponse")]
        System.Threading.Tasks.Task<int> UpdateSqlUserAsync(SolidCP.Providers.Database.SqlUser item);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlUserResponse")]
        int DeleteSqlUser(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesDatabaseServers/DeleteSqlUserResponse")]
        System.Threading.Tasks.Task<int> DeleteSqlUserAsync(int itemId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esDatabaseServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesDatabaseServers
    {
        public System.Data.DataSet GetRawSqlDatabasesPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esDatabaseServers", "GetRawSqlDatabasesPaged", packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSqlDatabasesPagedAsync(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esDatabaseServers", "GetRawSqlDatabasesPaged", packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Database.SqlDatabase[] /*List*/ GetSqlDatabases(int packageId, string groupName, bool recursive)
        {
            return Invoke<SolidCP.Providers.Database.SqlDatabase[], SolidCP.Providers.Database.SqlDatabase>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlDatabases", packageId, groupName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase[]> GetSqlDatabasesAsync(int packageId, string groupName, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Database.SqlDatabase[], SolidCP.Providers.Database.SqlDatabase>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlDatabases", packageId, groupName, recursive);
        }

        public SolidCP.Providers.Database.SqlDatabase GetSqlDatabase(int itemId)
        {
            return Invoke<SolidCP.Providers.Database.SqlDatabase>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlDatabase", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase> GetSqlDatabaseAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Database.SqlDatabase>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlDatabase", itemId);
        }

        public int AddSqlDatabase(SolidCP.Providers.Database.SqlDatabase item, string groupName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "AddSqlDatabase", item, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddSqlDatabaseAsync(SolidCP.Providers.Database.SqlDatabase item, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "AddSqlDatabase", item, groupName);
        }

        public int UpdateSqlDatabase(SolidCP.Providers.Database.SqlDatabase item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "UpdateSqlDatabase", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSqlDatabaseAsync(SolidCP.Providers.Database.SqlDatabase item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "UpdateSqlDatabase", item);
        }

        public int DeleteSqlDatabase(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "DeleteSqlDatabase", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSqlDatabaseAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "DeleteSqlDatabase", itemId);
        }

        public string BackupSqlDatabase(int itemId, string backupName, bool zipBackup, bool download, string folderName)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esDatabaseServers", "BackupSqlDatabase", itemId, backupName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> BackupSqlDatabaseAsync(int itemId, string backupName, bool zipBackup, bool download, string folderName)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esDatabaseServers", "BackupSqlDatabase", itemId, backupName, zipBackup, download, folderName);
        }

        public byte[] GetSqlBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlBackupBinaryChunk", itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSqlBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlBackupBinaryChunk", itemId, path, offset, length);
        }

        public string AppendSqlBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esDatabaseServers", "AppendSqlBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendSqlBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esDatabaseServers", "AppendSqlBackupBinaryChunk", itemId, fileName, path, chunk);
        }

        public int RestoreSqlDatabase(int itemId, string[] uploadedFiles, string[] packageFiles)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "RestoreSqlDatabase", itemId, uploadedFiles, packageFiles);
        }

        public async System.Threading.Tasks.Task<int> RestoreSqlDatabaseAsync(int itemId, string[] uploadedFiles, string[] packageFiles)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "RestoreSqlDatabase", itemId, uploadedFiles, packageFiles);
        }

        public int TruncateSqlDatabase(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "TruncateSqlDatabase", itemId);
        }

        public async System.Threading.Tasks.Task<int> TruncateSqlDatabaseAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "TruncateSqlDatabase", itemId);
        }

        public SolidCP.EnterpriseServer.DatabaseBrowserConfiguration GetDatabaseBrowserConfiguration(int packageId, string groupName)
        {
            return Invoke<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration>("SolidCP.EnterpriseServer.esDatabaseServers", "GetDatabaseBrowserConfiguration", packageId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration> GetDatabaseBrowserConfigurationAsync(int packageId, string groupName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration>("SolidCP.EnterpriseServer.esDatabaseServers", "GetDatabaseBrowserConfiguration", packageId, groupName);
        }

        public SolidCP.EnterpriseServer.DatabaseBrowserConfiguration GetDatabaseBrowserLogonScript(int packageId, string groupName, string username)
        {
            return Invoke<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration>("SolidCP.EnterpriseServer.esDatabaseServers", "GetDatabaseBrowserLogonScript", packageId, groupName, username);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration> GetDatabaseBrowserLogonScriptAsync(int packageId, string groupName, string username)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration>("SolidCP.EnterpriseServer.esDatabaseServers", "GetDatabaseBrowserLogonScript", packageId, groupName, username);
        }

        public System.Data.DataSet GetRawSqlUsersPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esDatabaseServers", "GetRawSqlUsersPaged", packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSqlUsersPagedAsync(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esDatabaseServers", "GetRawSqlUsersPaged", packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Database.SqlUser[] /*List*/ GetSqlUsers(int packageId, string groupName, bool recursive)
        {
            return Invoke<SolidCP.Providers.Database.SqlUser[], SolidCP.Providers.Database.SqlUser>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlUsers", packageId, groupName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser[]> GetSqlUsersAsync(int packageId, string groupName, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Database.SqlUser[], SolidCP.Providers.Database.SqlUser>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlUsers", packageId, groupName, recursive);
        }

        public SolidCP.Providers.Database.SqlUser GetSqlUser(int itemId)
        {
            return Invoke<SolidCP.Providers.Database.SqlUser>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlUser", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser> GetSqlUserAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Database.SqlUser>("SolidCP.EnterpriseServer.esDatabaseServers", "GetSqlUser", itemId);
        }

        public int AddSqlUser(SolidCP.Providers.Database.SqlUser item, string groupName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "AddSqlUser", item, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddSqlUserAsync(SolidCP.Providers.Database.SqlUser item, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "AddSqlUser", item, groupName);
        }

        public int UpdateSqlUser(SolidCP.Providers.Database.SqlUser item)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "UpdateSqlUser", item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSqlUserAsync(SolidCP.Providers.Database.SqlUser item)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "UpdateSqlUser", item);
        }

        public int DeleteSqlUser(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esDatabaseServers", "DeleteSqlUser", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSqlUserAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esDatabaseServers", "DeleteSqlUser", itemId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esDatabaseServers : SolidCP.Web.Client.ClientBase<IesDatabaseServers, esDatabaseServersAssemblyClient>, IesDatabaseServers
    {
        public System.Data.DataSet GetRawSqlDatabasesPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawSqlDatabasesPaged(packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSqlDatabasesPagedAsync(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawSqlDatabasesPagedAsync(packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Database.SqlDatabase[] /*List*/ GetSqlDatabases(int packageId, string groupName, bool recursive)
        {
            return base.Client.GetSqlDatabases(packageId, groupName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase[]> GetSqlDatabasesAsync(int packageId, string groupName, bool recursive)
        {
            return await base.Client.GetSqlDatabasesAsync(packageId, groupName, recursive);
        }

        public SolidCP.Providers.Database.SqlDatabase GetSqlDatabase(int itemId)
        {
            return base.Client.GetSqlDatabase(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase> GetSqlDatabaseAsync(int itemId)
        {
            return await base.Client.GetSqlDatabaseAsync(itemId);
        }

        public int AddSqlDatabase(SolidCP.Providers.Database.SqlDatabase item, string groupName)
        {
            return base.Client.AddSqlDatabase(item, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddSqlDatabaseAsync(SolidCP.Providers.Database.SqlDatabase item, string groupName)
        {
            return await base.Client.AddSqlDatabaseAsync(item, groupName);
        }

        public int UpdateSqlDatabase(SolidCP.Providers.Database.SqlDatabase item)
        {
            return base.Client.UpdateSqlDatabase(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSqlDatabaseAsync(SolidCP.Providers.Database.SqlDatabase item)
        {
            return await base.Client.UpdateSqlDatabaseAsync(item);
        }

        public int DeleteSqlDatabase(int itemId)
        {
            return base.Client.DeleteSqlDatabase(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSqlDatabaseAsync(int itemId)
        {
            return await base.Client.DeleteSqlDatabaseAsync(itemId);
        }

        public string BackupSqlDatabase(int itemId, string backupName, bool zipBackup, bool download, string folderName)
        {
            return base.Client.BackupSqlDatabase(itemId, backupName, zipBackup, download, folderName);
        }

        public async System.Threading.Tasks.Task<string> BackupSqlDatabaseAsync(int itemId, string backupName, bool zipBackup, bool download, string folderName)
        {
            return await base.Client.BackupSqlDatabaseAsync(itemId, backupName, zipBackup, download, folderName);
        }

        public byte[] GetSqlBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return base.Client.GetSqlBackupBinaryChunk(itemId, path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetSqlBackupBinaryChunkAsync(int itemId, string path, int offset, int length)
        {
            return await base.Client.GetSqlBackupBinaryChunkAsync(itemId, path, offset, length);
        }

        public string AppendSqlBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return base.Client.AppendSqlBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendSqlBackupBinaryChunkAsync(int itemId, string fileName, string path, byte[] chunk)
        {
            return await base.Client.AppendSqlBackupBinaryChunkAsync(itemId, fileName, path, chunk);
        }

        public int RestoreSqlDatabase(int itemId, string[] uploadedFiles, string[] packageFiles)
        {
            return base.Client.RestoreSqlDatabase(itemId, uploadedFiles, packageFiles);
        }

        public async System.Threading.Tasks.Task<int> RestoreSqlDatabaseAsync(int itemId, string[] uploadedFiles, string[] packageFiles)
        {
            return await base.Client.RestoreSqlDatabaseAsync(itemId, uploadedFiles, packageFiles);
        }

        public int TruncateSqlDatabase(int itemId)
        {
            return base.Client.TruncateSqlDatabase(itemId);
        }

        public async System.Threading.Tasks.Task<int> TruncateSqlDatabaseAsync(int itemId)
        {
            return await base.Client.TruncateSqlDatabaseAsync(itemId);
        }

        public SolidCP.EnterpriseServer.DatabaseBrowserConfiguration GetDatabaseBrowserConfiguration(int packageId, string groupName)
        {
            return base.Client.GetDatabaseBrowserConfiguration(packageId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration> GetDatabaseBrowserConfigurationAsync(int packageId, string groupName)
        {
            return await base.Client.GetDatabaseBrowserConfigurationAsync(packageId, groupName);
        }

        public SolidCP.EnterpriseServer.DatabaseBrowserConfiguration GetDatabaseBrowserLogonScript(int packageId, string groupName, string username)
        {
            return base.Client.GetDatabaseBrowserLogonScript(packageId, groupName, username);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DatabaseBrowserConfiguration> GetDatabaseBrowserLogonScriptAsync(int packageId, string groupName, string username)
        {
            return await base.Client.GetDatabaseBrowserLogonScriptAsync(packageId, groupName, username);
        }

        public System.Data.DataSet GetRawSqlUsersPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawSqlUsersPaged(packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSqlUsersPagedAsync(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawSqlUsersPagedAsync(packageId, groupName, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Database.SqlUser[] /*List*/ GetSqlUsers(int packageId, string groupName, bool recursive)
        {
            return base.Client.GetSqlUsers(packageId, groupName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser[]> GetSqlUsersAsync(int packageId, string groupName, bool recursive)
        {
            return await base.Client.GetSqlUsersAsync(packageId, groupName, recursive);
        }

        public SolidCP.Providers.Database.SqlUser GetSqlUser(int itemId)
        {
            return base.Client.GetSqlUser(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser> GetSqlUserAsync(int itemId)
        {
            return await base.Client.GetSqlUserAsync(itemId);
        }

        public int AddSqlUser(SolidCP.Providers.Database.SqlUser item, string groupName)
        {
            return base.Client.AddSqlUser(item, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddSqlUserAsync(SolidCP.Providers.Database.SqlUser item, string groupName)
        {
            return await base.Client.AddSqlUserAsync(item, groupName);
        }

        public int UpdateSqlUser(SolidCP.Providers.Database.SqlUser item)
        {
            return base.Client.UpdateSqlUser(item);
        }

        public async System.Threading.Tasks.Task<int> UpdateSqlUserAsync(SolidCP.Providers.Database.SqlUser item)
        {
            return await base.Client.UpdateSqlUserAsync(item);
        }

        public int DeleteSqlUser(int itemId)
        {
            return base.Client.DeleteSqlUser(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSqlUserAsync(int itemId)
        {
            return await base.Client.DeleteSqlUserAsync(itemId);
        }
    }
}
#endif