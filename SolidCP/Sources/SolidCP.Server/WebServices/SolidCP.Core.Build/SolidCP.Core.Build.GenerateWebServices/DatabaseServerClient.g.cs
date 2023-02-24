#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Server.Utils;
using SolidCP.Providers;
using SolidCP.Providers.Database;
using SolidCP.Server;
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for DatabaseServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class DatabaseServer
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif