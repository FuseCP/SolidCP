#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Server.Utils;
using SolidCP.Providers;
using SolidCP.Providers.Database;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

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
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class DatabaseServer : SolidCP.Server.DatabaseServer, IDatabaseServer
    {
    }
}
#endif