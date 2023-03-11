#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Server.Utils;
using SolidCP.Providers;
using SolidCP.Providers.Database;
using SolidCP.Server;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IDatabaseServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckConnectivity(string databaseName, string username, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        DataSet ExecuteSqlQuery(string databaseName, string commandText);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ExecuteSqlNonQuery(string databaseName, string commandText);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DatabaseExists(string databaseName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetDatabases();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SqlDatabase GetDatabase(string databaseName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateDatabase(SqlDatabase database);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateDatabase(SqlDatabase database);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteDatabase(string databaseName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void TruncateDatabase(string databaseName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string BackupDatabase(string databaseName, string backupName, bool zipBackup);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RestoreDatabase(string databaseName, string[] fileNames);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool UserExists(string userName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetUsers();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SqlUser GetUser(string username, string[] databases);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateUser(SqlUser user, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateUser(SqlUser user, string[] databases);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteUser(string username, string[] databases);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeUserPassword(string username, string password);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DatabaseServer : SolidCP.Server.DatabaseServer, IDatabaseServer
    {
        public new bool CheckConnectivity(string databaseName, string username, string password)
        {
            return base.CheckConnectivity(databaseName, username, password);
        }

        public new DataSet ExecuteSqlQuery(string databaseName, string commandText)
        {
            return base.ExecuteSqlQuery(databaseName, commandText);
        }

        public new void ExecuteSqlNonQuery(string databaseName, string commandText)
        {
            base.ExecuteSqlNonQuery(databaseName, commandText);
        }

        public new DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText)
        {
            return base.ExecuteSqlQuerySafe(databaseName, username, password, commandText);
        }

        public new void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText)
        {
            base.ExecuteSqlNonQuerySafe(databaseName, username, password, commandText);
        }

        public new bool DatabaseExists(string databaseName)
        {
            return base.DatabaseExists(databaseName);
        }

        public new string[] GetDatabases()
        {
            return base.GetDatabases();
        }

        public new SqlDatabase GetDatabase(string databaseName)
        {
            return base.GetDatabase(databaseName);
        }

        public new void CreateDatabase(SqlDatabase database)
        {
            base.CreateDatabase(database);
        }

        public new void UpdateDatabase(SqlDatabase database)
        {
            base.UpdateDatabase(database);
        }

        public new void DeleteDatabase(string databaseName)
        {
            base.DeleteDatabase(databaseName);
        }

        public new void TruncateDatabase(string databaseName)
        {
            base.TruncateDatabase(databaseName);
        }

        public new byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.GetTempFileBinaryChunk(path, offset, length);
        }

        public new string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public new string BackupDatabase(string databaseName, string backupName, bool zipBackup)
        {
            return base.BackupDatabase(databaseName, backupName, zipBackup);
        }

        public new void RestoreDatabase(string databaseName, string[] fileNames)
        {
            base.RestoreDatabase(databaseName, fileNames);
        }

        public new bool UserExists(string userName)
        {
            return base.UserExists(userName);
        }

        public new string[] GetUsers()
        {
            return base.GetUsers();
        }

        public new SqlUser GetUser(string username, string[] databases)
        {
            return base.GetUser(username, databases);
        }

        public new void CreateUser(SqlUser user, string password)
        {
            base.CreateUser(user, password);
        }

        public new void UpdateUser(SqlUser user, string[] databases)
        {
            base.UpdateUser(user, databases);
        }

        public new void DeleteUser(string username, string[] databases)
        {
            base.DeleteUser(username, databases);
        }

        public new void ChangeUserPassword(string username, string password)
        {
            base.ChangeUserPassword(username, password);
        }
    }
}
#endif