#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IDatabaseServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IDatabaseServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/CheckConnectivity", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/CheckConnectivityResponse")]
        bool CheckConnectivity(string databaseName, string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/CheckConnectivity", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/CheckConnectivityResponse")]
        System.Threading.Tasks.Task<bool> CheckConnectivityAsync(string databaseName, string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQuery", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQueryResponse")]
        System.Data.DataSet ExecuteSqlQuery(string databaseName, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQuery", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQueryResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> ExecuteSqlQueryAsync(string databaseName, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQuery", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQueryResponse")]
        void ExecuteSqlNonQuery(string databaseName, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQuery", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQueryResponse")]
        System.Threading.Tasks.Task ExecuteSqlNonQueryAsync(string databaseName, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQuerySafe", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQuerySafeResponse")]
        System.Data.DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQuerySafe", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlQuerySafeResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> ExecuteSqlQuerySafeAsync(string databaseName, string username, string password, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQuerySafe", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQuerySafeResponse")]
        void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQuerySafe", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ExecuteSqlNonQuerySafeResponse")]
        System.Threading.Tasks.Task ExecuteSqlNonQuerySafeAsync(string databaseName, string username, string password, string commandText);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/DatabaseExists", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/DatabaseExistsResponse")]
        bool DatabaseExists(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/DatabaseExists", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/DatabaseExistsResponse")]
        System.Threading.Tasks.Task<bool> DatabaseExistsAsync(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabases", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabasesResponse")]
        string[] GetDatabases();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabases", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabasesResponse")]
        System.Threading.Tasks.Task<string[]> GetDatabasesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabaseResponse")]
        SolidCP.Providers.Database.SqlDatabase GetDatabase(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetDatabaseResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase> GetDatabaseAsync(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/CreateDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/CreateDatabaseResponse")]
        void CreateDatabase(SolidCP.Providers.Database.SqlDatabase database);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/CreateDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/CreateDatabaseResponse")]
        System.Threading.Tasks.Task CreateDatabaseAsync(SolidCP.Providers.Database.SqlDatabase database);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateDatabaseResponse")]
        void UpdateDatabase(SolidCP.Providers.Database.SqlDatabase database);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateDatabaseResponse")]
        System.Threading.Tasks.Task UpdateDatabaseAsync(SolidCP.Providers.Database.SqlDatabase database);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteDatabaseResponse")]
        void DeleteDatabase(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteDatabaseResponse")]
        System.Threading.Tasks.Task DeleteDatabaseAsync(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/TruncateDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/TruncateDatabaseResponse")]
        void TruncateDatabase(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/TruncateDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/TruncateDatabaseResponse")]
        System.Threading.Tasks.Task TruncateDatabaseAsync(string databaseName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetTempFileBinaryChunkResponse")]
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/AppendTempFileBinaryChunkResponse")]
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/AppendTempFileBinaryChunk", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/AppendTempFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/BackupDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/BackupDatabaseResponse")]
        string BackupDatabase(string databaseName, string backupName, bool zipBackup);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/BackupDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/BackupDatabaseResponse")]
        System.Threading.Tasks.Task<string> BackupDatabaseAsync(string databaseName, string backupName, bool zipBackup);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/RestoreDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/RestoreDatabaseResponse")]
        void RestoreDatabase(string databaseName, string[] fileNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/RestoreDatabase", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/RestoreDatabaseResponse")]
        System.Threading.Tasks.Task RestoreDatabaseAsync(string databaseName, string[] fileNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/UserExists", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/UserExistsResponse")]
        bool UserExists(string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/UserExists", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/UserExistsResponse")]
        System.Threading.Tasks.Task<bool> UserExistsAsync(string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetUsersResponse")]
        string[] GetUsers();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetUsersResponse")]
        System.Threading.Tasks.Task<string[]> GetUsersAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetUserResponse")]
        SolidCP.Providers.Database.SqlUser GetUser(string username, string[] databases);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/GetUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser> GetUserAsync(string username, string[] databases);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/CreateUserResponse")]
        void CreateUser(SolidCP.Providers.Database.SqlUser user, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/CreateUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/CreateUserResponse")]
        System.Threading.Tasks.Task CreateUserAsync(SolidCP.Providers.Database.SqlUser user, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateUserResponse")]
        void UpdateUser(SolidCP.Providers.Database.SqlUser user, string[] databases);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/UpdateUserResponse")]
        System.Threading.Tasks.Task UpdateUserAsync(SolidCP.Providers.Database.SqlUser user, string[] databases);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteUserResponse")]
        void DeleteUser(string username, string[] databases);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/DeleteUserResponse")]
        System.Threading.Tasks.Task DeleteUserAsync(string username, string[] databases);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ChangeUserPassword", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ChangeUserPasswordResponse")]
        void ChangeUserPassword(string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDatabaseServer/ChangeUserPassword", ReplyAction = "http://smbsaas/solidcp/server/IDatabaseServer/ChangeUserPasswordResponse")]
        System.Threading.Tasks.Task ChangeUserPasswordAsync(string username, string password);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class DatabaseServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IDatabaseServer
    {
        public bool CheckConnectivity(string databaseName, string username, string password)
        {
            return Invoke<bool>("SolidCP.Server.DatabaseServer", "CheckConnectivity", databaseName, username, password);
        }

        public async System.Threading.Tasks.Task<bool> CheckConnectivityAsync(string databaseName, string username, string password)
        {
            return await InvokeAsync<bool>("SolidCP.Server.DatabaseServer", "CheckConnectivity", databaseName, username, password);
        }

        public System.Data.DataSet ExecuteSqlQuery(string databaseName, string commandText)
        {
            return Invoke<System.Data.DataSet>("SolidCP.Server.DatabaseServer", "ExecuteSqlQuery", databaseName, commandText);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> ExecuteSqlQueryAsync(string databaseName, string commandText)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.Server.DatabaseServer", "ExecuteSqlQuery", databaseName, commandText);
        }

        public void ExecuteSqlNonQuery(string databaseName, string commandText)
        {
            Invoke("SolidCP.Server.DatabaseServer", "ExecuteSqlNonQuery", databaseName, commandText);
        }

        public async System.Threading.Tasks.Task ExecuteSqlNonQueryAsync(string databaseName, string commandText)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "ExecuteSqlNonQuery", databaseName, commandText);
        }

        public System.Data.DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText)
        {
            return Invoke<System.Data.DataSet>("SolidCP.Server.DatabaseServer", "ExecuteSqlQuerySafe", databaseName, username, password, commandText);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> ExecuteSqlQuerySafeAsync(string databaseName, string username, string password, string commandText)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.Server.DatabaseServer", "ExecuteSqlQuerySafe", databaseName, username, password, commandText);
        }

        public void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText)
        {
            Invoke("SolidCP.Server.DatabaseServer", "ExecuteSqlNonQuerySafe", databaseName, username, password, commandText);
        }

        public async System.Threading.Tasks.Task ExecuteSqlNonQuerySafeAsync(string databaseName, string username, string password, string commandText)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "ExecuteSqlNonQuerySafe", databaseName, username, password, commandText);
        }

        public bool DatabaseExists(string databaseName)
        {
            return Invoke<bool>("SolidCP.Server.DatabaseServer", "DatabaseExists", databaseName);
        }

        public async System.Threading.Tasks.Task<bool> DatabaseExistsAsync(string databaseName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.DatabaseServer", "DatabaseExists", databaseName);
        }

        public string[] GetDatabases()
        {
            return Invoke<string[]>("SolidCP.Server.DatabaseServer", "GetDatabases");
        }

        public async System.Threading.Tasks.Task<string[]> GetDatabasesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.DatabaseServer", "GetDatabases");
        }

        public SolidCP.Providers.Database.SqlDatabase GetDatabase(string databaseName)
        {
            return Invoke<SolidCP.Providers.Database.SqlDatabase>("SolidCP.Server.DatabaseServer", "GetDatabase", databaseName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase> GetDatabaseAsync(string databaseName)
        {
            return await InvokeAsync<SolidCP.Providers.Database.SqlDatabase>("SolidCP.Server.DatabaseServer", "GetDatabase", databaseName);
        }

        public void CreateDatabase(SolidCP.Providers.Database.SqlDatabase database)
        {
            Invoke("SolidCP.Server.DatabaseServer", "CreateDatabase", database);
        }

        public async System.Threading.Tasks.Task CreateDatabaseAsync(SolidCP.Providers.Database.SqlDatabase database)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "CreateDatabase", database);
        }

        public void UpdateDatabase(SolidCP.Providers.Database.SqlDatabase database)
        {
            Invoke("SolidCP.Server.DatabaseServer", "UpdateDatabase", database);
        }

        public async System.Threading.Tasks.Task UpdateDatabaseAsync(SolidCP.Providers.Database.SqlDatabase database)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "UpdateDatabase", database);
        }

        public void DeleteDatabase(string databaseName)
        {
            Invoke("SolidCP.Server.DatabaseServer", "DeleteDatabase", databaseName);
        }

        public async System.Threading.Tasks.Task DeleteDatabaseAsync(string databaseName)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "DeleteDatabase", databaseName);
        }

        public void TruncateDatabase(string databaseName)
        {
            Invoke("SolidCP.Server.DatabaseServer", "TruncateDatabase", databaseName);
        }

        public async System.Threading.Tasks.Task TruncateDatabaseAsync(string databaseName)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "TruncateDatabase", databaseName);
        }

        public byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.Server.DatabaseServer", "GetTempFileBinaryChunk", path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.DatabaseServer", "GetTempFileBinaryChunk", path, offset, length);
        }

        public string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return Invoke<string>("SolidCP.Server.DatabaseServer", "AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk)
        {
            return await InvokeAsync<string>("SolidCP.Server.DatabaseServer", "AppendTempFileBinaryChunk", fileName, path, chunk);
        }

        public string BackupDatabase(string databaseName, string backupName, bool zipBackup)
        {
            return Invoke<string>("SolidCP.Server.DatabaseServer", "BackupDatabase", databaseName, backupName, zipBackup);
        }

        public async System.Threading.Tasks.Task<string> BackupDatabaseAsync(string databaseName, string backupName, bool zipBackup)
        {
            return await InvokeAsync<string>("SolidCP.Server.DatabaseServer", "BackupDatabase", databaseName, backupName, zipBackup);
        }

        public void RestoreDatabase(string databaseName, string[] fileNames)
        {
            Invoke("SolidCP.Server.DatabaseServer", "RestoreDatabase", databaseName, fileNames);
        }

        public async System.Threading.Tasks.Task RestoreDatabaseAsync(string databaseName, string[] fileNames)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "RestoreDatabase", databaseName, fileNames);
        }

        public bool UserExists(string userName)
        {
            return Invoke<bool>("SolidCP.Server.DatabaseServer", "UserExists", userName);
        }

        public async System.Threading.Tasks.Task<bool> UserExistsAsync(string userName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.DatabaseServer", "UserExists", userName);
        }

        public string[] GetUsers()
        {
            return Invoke<string[]>("SolidCP.Server.DatabaseServer", "GetUsers");
        }

        public async System.Threading.Tasks.Task<string[]> GetUsersAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.DatabaseServer", "GetUsers");
        }

        public SolidCP.Providers.Database.SqlUser GetUser(string username, string[] databases)
        {
            return Invoke<SolidCP.Providers.Database.SqlUser>("SolidCP.Server.DatabaseServer", "GetUser", username, databases);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser> GetUserAsync(string username, string[] databases)
        {
            return await InvokeAsync<SolidCP.Providers.Database.SqlUser>("SolidCP.Server.DatabaseServer", "GetUser", username, databases);
        }

        public void CreateUser(SolidCP.Providers.Database.SqlUser user, string password)
        {
            Invoke("SolidCP.Server.DatabaseServer", "CreateUser", user, password);
        }

        public async System.Threading.Tasks.Task CreateUserAsync(SolidCP.Providers.Database.SqlUser user, string password)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "CreateUser", user, password);
        }

        public void UpdateUser(SolidCP.Providers.Database.SqlUser user, string[] databases)
        {
            Invoke("SolidCP.Server.DatabaseServer", "UpdateUser", user, databases);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(SolidCP.Providers.Database.SqlUser user, string[] databases)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "UpdateUser", user, databases);
        }

        public void DeleteUser(string username, string[] databases)
        {
            Invoke("SolidCP.Server.DatabaseServer", "DeleteUser", username, databases);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string username, string[] databases)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "DeleteUser", username, databases);
        }

        public void ChangeUserPassword(string username, string password)
        {
            Invoke("SolidCP.Server.DatabaseServer", "ChangeUserPassword", username, password);
        }

        public async System.Threading.Tasks.Task ChangeUserPasswordAsync(string username, string password)
        {
            await InvokeAsync("SolidCP.Server.DatabaseServer", "ChangeUserPassword", username, password);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class DatabaseServer : SolidCP.Web.Client.ClientBase<IDatabaseServer, DatabaseServerAssemblyClient>, IDatabaseServer
    {
        public bool CheckConnectivity(string databaseName, string username, string password)
        {
            return base.Client.CheckConnectivity(databaseName, username, password);
        }

        public async System.Threading.Tasks.Task<bool> CheckConnectivityAsync(string databaseName, string username, string password)
        {
            return await base.Client.CheckConnectivityAsync(databaseName, username, password);
        }

        public System.Data.DataSet ExecuteSqlQuery(string databaseName, string commandText)
        {
            return base.Client.ExecuteSqlQuery(databaseName, commandText);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> ExecuteSqlQueryAsync(string databaseName, string commandText)
        {
            return await base.Client.ExecuteSqlQueryAsync(databaseName, commandText);
        }

        public void ExecuteSqlNonQuery(string databaseName, string commandText)
        {
            base.Client.ExecuteSqlNonQuery(databaseName, commandText);
        }

        public async System.Threading.Tasks.Task ExecuteSqlNonQueryAsync(string databaseName, string commandText)
        {
            await base.Client.ExecuteSqlNonQueryAsync(databaseName, commandText);
        }

        public System.Data.DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText)
        {
            return base.Client.ExecuteSqlQuerySafe(databaseName, username, password, commandText);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> ExecuteSqlQuerySafeAsync(string databaseName, string username, string password, string commandText)
        {
            return await base.Client.ExecuteSqlQuerySafeAsync(databaseName, username, password, commandText);
        }

        public void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText)
        {
            base.Client.ExecuteSqlNonQuerySafe(databaseName, username, password, commandText);
        }

        public async System.Threading.Tasks.Task ExecuteSqlNonQuerySafeAsync(string databaseName, string username, string password, string commandText)
        {
            await base.Client.ExecuteSqlNonQuerySafeAsync(databaseName, username, password, commandText);
        }

        public bool DatabaseExists(string databaseName)
        {
            return base.Client.DatabaseExists(databaseName);
        }

        public async System.Threading.Tasks.Task<bool> DatabaseExistsAsync(string databaseName)
        {
            return await base.Client.DatabaseExistsAsync(databaseName);
        }

        public string[] GetDatabases()
        {
            return base.Client.GetDatabases();
        }

        public async System.Threading.Tasks.Task<string[]> GetDatabasesAsync()
        {
            return await base.Client.GetDatabasesAsync();
        }

        public SolidCP.Providers.Database.SqlDatabase GetDatabase(string databaseName)
        {
            return base.Client.GetDatabase(databaseName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlDatabase> GetDatabaseAsync(string databaseName)
        {
            return await base.Client.GetDatabaseAsync(databaseName);
        }

        public void CreateDatabase(SolidCP.Providers.Database.SqlDatabase database)
        {
            base.Client.CreateDatabase(database);
        }

        public async System.Threading.Tasks.Task CreateDatabaseAsync(SolidCP.Providers.Database.SqlDatabase database)
        {
            await base.Client.CreateDatabaseAsync(database);
        }

        public void UpdateDatabase(SolidCP.Providers.Database.SqlDatabase database)
        {
            base.Client.UpdateDatabase(database);
        }

        public async System.Threading.Tasks.Task UpdateDatabaseAsync(SolidCP.Providers.Database.SqlDatabase database)
        {
            await base.Client.UpdateDatabaseAsync(database);
        }

        public void DeleteDatabase(string databaseName)
        {
            base.Client.DeleteDatabase(databaseName);
        }

        public async System.Threading.Tasks.Task DeleteDatabaseAsync(string databaseName)
        {
            await base.Client.DeleteDatabaseAsync(databaseName);
        }

        public void TruncateDatabase(string databaseName)
        {
            base.Client.TruncateDatabase(databaseName);
        }

        public async System.Threading.Tasks.Task TruncateDatabaseAsync(string databaseName)
        {
            await base.Client.TruncateDatabaseAsync(databaseName);
        }

        public byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.Client.GetTempFileBinaryChunk(path, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetTempFileBinaryChunkAsync(string path, int offset, int length)
        {
            return await base.Client.GetTempFileBinaryChunkAsync(path, offset, length);
        }

        public string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.Client.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public async System.Threading.Tasks.Task<string> AppendTempFileBinaryChunkAsync(string fileName, string path, byte[] chunk)
        {
            return await base.Client.AppendTempFileBinaryChunkAsync(fileName, path, chunk);
        }

        public string BackupDatabase(string databaseName, string backupName, bool zipBackup)
        {
            return base.Client.BackupDatabase(databaseName, backupName, zipBackup);
        }

        public async System.Threading.Tasks.Task<string> BackupDatabaseAsync(string databaseName, string backupName, bool zipBackup)
        {
            return await base.Client.BackupDatabaseAsync(databaseName, backupName, zipBackup);
        }

        public void RestoreDatabase(string databaseName, string[] fileNames)
        {
            base.Client.RestoreDatabase(databaseName, fileNames);
        }

        public async System.Threading.Tasks.Task RestoreDatabaseAsync(string databaseName, string[] fileNames)
        {
            await base.Client.RestoreDatabaseAsync(databaseName, fileNames);
        }

        public bool UserExists(string userName)
        {
            return base.Client.UserExists(userName);
        }

        public async System.Threading.Tasks.Task<bool> UserExistsAsync(string userName)
        {
            return await base.Client.UserExistsAsync(userName);
        }

        public string[] GetUsers()
        {
            return base.Client.GetUsers();
        }

        public async System.Threading.Tasks.Task<string[]> GetUsersAsync()
        {
            return await base.Client.GetUsersAsync();
        }

        public SolidCP.Providers.Database.SqlUser GetUser(string username, string[] databases)
        {
            return base.Client.GetUser(username, databases);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Database.SqlUser> GetUserAsync(string username, string[] databases)
        {
            return await base.Client.GetUserAsync(username, databases);
        }

        public void CreateUser(SolidCP.Providers.Database.SqlUser user, string password)
        {
            base.Client.CreateUser(user, password);
        }

        public async System.Threading.Tasks.Task CreateUserAsync(SolidCP.Providers.Database.SqlUser user, string password)
        {
            await base.Client.CreateUserAsync(user, password);
        }

        public void UpdateUser(SolidCP.Providers.Database.SqlUser user, string[] databases)
        {
            base.Client.UpdateUser(user, databases);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(SolidCP.Providers.Database.SqlUser user, string[] databases)
        {
            await base.Client.UpdateUserAsync(user, databases);
        }

        public void DeleteUser(string username, string[] databases)
        {
            base.Client.DeleteUser(username, databases);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string username, string[] databases)
        {
            await base.Client.DeleteUserAsync(username, databases);
        }

        public void ChangeUserPassword(string username, string password)
        {
            base.Client.ChangeUserPassword(username, password);
        }

        public async System.Threading.Tasks.Task ChangeUserPasswordAsync(string username, string password)
        {
            await base.Client.ChangeUserPasswordAsync(username, password);
        }
    }
}
#endif