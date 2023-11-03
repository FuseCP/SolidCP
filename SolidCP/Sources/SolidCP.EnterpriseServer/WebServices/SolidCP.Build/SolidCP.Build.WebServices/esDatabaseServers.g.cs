#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.Database;
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
    public interface IesDatabaseServers
    {
        [WebMethod]
        [OperationContract]
        DataSet GetRawSqlDatabasesPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<SqlDatabase> GetSqlDatabases(int packageId, string groupName, bool recursive);
        [WebMethod]
        [OperationContract]
        SqlDatabase GetSqlDatabase(int itemId);
        [WebMethod]
        [OperationContract]
        int AddSqlDatabase(SqlDatabase item, string groupName);
        [WebMethod]
        [OperationContract]
        int UpdateSqlDatabase(SqlDatabase item);
        [WebMethod]
        [OperationContract]
        int DeleteSqlDatabase(int itemId);
        [WebMethod]
        [OperationContract]
        string BackupSqlDatabase(int itemId, string backupName, bool zipBackup, bool download, string folderName);
        [WebMethod]
        [OperationContract]
        byte[] GetSqlBackupBinaryChunk(int itemId, string path, int offset, int length);
        [WebMethod]
        [OperationContract]
        string AppendSqlBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk);
        [WebMethod]
        [OperationContract]
        int RestoreSqlDatabase(int itemId, string[] uploadedFiles, string[] packageFiles);
        [WebMethod]
        [OperationContract]
        int TruncateSqlDatabase(int itemId);
        [WebMethod]
        [OperationContract]
        DatabaseBrowserConfiguration GetDatabaseBrowserConfiguration(int packageId, string groupName);
        [WebMethod]
        [OperationContract]
        DatabaseBrowserConfiguration GetDatabaseBrowserLogonScript(int packageId, string groupName, string username);
        [WebMethod]
        [OperationContract]
        DataSet GetRawSqlUsersPaged(int packageId, string groupName, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<SqlUser> GetSqlUsers(int packageId, string groupName, bool recursive);
        [WebMethod]
        [OperationContract]
        SqlUser GetSqlUser(int itemId);
        [WebMethod]
        [OperationContract]
        int AddSqlUser(SqlUser item, string groupName);
        [WebMethod]
        [OperationContract]
        int UpdateSqlUser(SqlUser item);
        [WebMethod]
        [OperationContract]
        int DeleteSqlUser(int itemId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esDatabaseServers : SolidCP.EnterpriseServer.esDatabaseServers, IesDatabaseServers
    {
    }
}
#endif