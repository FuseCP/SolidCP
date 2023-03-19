// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Server.Utils;
using SolidCP.Providers;
using SolidCP.Providers.Database;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for DatabaseServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class DatabaseServer : HostingServiceProviderWebService, IDatabaseServer
    {
        private IDatabaseServer DatabaseProvider
        {
            get { return (IDatabaseServer)Provider; }
        }

        #region General methods
        [WebMethod, SoapHeader("settings")]
        public bool CheckConnectivity(string databaseName, string username, string password)
        {
            try
            {
                Log.WriteStart("'{0}' CheckConnectivity", ProviderSettings.ProviderName);
                bool result = DatabaseProvider.CheckConnectivity(databaseName, username, password);
                Log.WriteEnd("'{0}' CheckConnectivity", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckConnectivity", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public DataSet ExecuteSqlQuery(string databaseName, string commandText)
        {
            try
            {
                Log.WriteStart("'{0}' ExecuteSqlQuery", ProviderSettings.ProviderName);
                DataSet result = DatabaseProvider.ExecuteSqlQuery(databaseName, commandText);
                Log.WriteEnd("'{0}' ExecuteSqlQuery", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExecuteSqlQuery", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ExecuteSqlNonQuery(string databaseName, string commandText)
        {
            try
            {
                Log.WriteStart("'{0}' ExecuteSqlNonQuery", ProviderSettings.ProviderName);
                DatabaseProvider.ExecuteSqlNonQuery(databaseName, commandText);
                Log.WriteEnd("'{0}' ExecuteSqlNonQuery", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExecuteSqlNonQuery", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText)
        {
            try
            {
                Log.WriteStart("'{0}' ExecuteSqlQuerySafe", ProviderSettings.ProviderName);
                DataSet result = DatabaseProvider.ExecuteSqlQuerySafe(databaseName, username, password, commandText);
                Log.WriteEnd("'{0}' ExecuteSqlQuerySafe", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExecuteSqlQuerySafe", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText)
        {
            try
            {
                Log.WriteStart("'{0}' ExecuteSqlNonQuerySafe", ProviderSettings.ProviderName);
                DatabaseProvider.ExecuteSqlNonQuerySafe(databaseName, username, password, commandText);
                Log.WriteEnd("'{0}' ExecuteSqlNonQuerySafe", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExecuteSqlNonQuerySafe", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Databases
        [WebMethod, SoapHeader("settings")]
        public bool DatabaseExists(string databaseName)
        {
            try
            {
                Log.WriteStart("'{0}' DatabaseExists", ProviderSettings.ProviderName);
                bool result = DatabaseProvider.DatabaseExists(databaseName);
                Log.WriteEnd("'{0}' DatabaseExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DatabaseExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetDatabases()
        {
            try
            {
                Log.WriteStart("'{0}' GetDatabases", ProviderSettings.ProviderName);
                string[] result = DatabaseProvider.GetDatabases();
                Log.WriteEnd("'{0}' GetDatabases", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetDatabases", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SqlDatabase GetDatabase(string databaseName)
        {
            try
            {
                Log.WriteStart("'{0}' GetDatabase", ProviderSettings.ProviderName);
                SqlDatabase result = DatabaseProvider.GetDatabase(databaseName);
                Log.WriteEnd("'{0}' GetDatabase", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateDatabase(SqlDatabase database)
        {
            try
            {
                Log.WriteStart("'{0}' CreateDatabase", ProviderSettings.ProviderName);
                DatabaseProvider.CreateDatabase(database);
                Log.WriteEnd("'{0}' CreateDatabase", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateDatabase(SqlDatabase database)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateDatabase", ProviderSettings.ProviderName);
                DatabaseProvider.UpdateDatabase(database);
                Log.WriteEnd("'{0}' UpdateDatabase", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteDatabase(string databaseName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteDatabase", ProviderSettings.ProviderName);
                DatabaseProvider.DeleteDatabase(databaseName);
                Log.WriteEnd("'{0}' DeleteDatabase", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void TruncateDatabase(string databaseName)
        {
            try
            {
                Log.WriteStart("'{0}' TruncateDatabase", ProviderSettings.ProviderName);
                DatabaseProvider.TruncateDatabase(databaseName);
                Log.WriteEnd("'{0}' TruncateDatabase", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' TruncateDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            try
            {
                Log.WriteStart("'{0}' GetTempFileBinaryChunk", ProviderSettings.ProviderName);
                byte[] result = DatabaseProvider.GetTempFileBinaryChunk(path, offset, length);
                Log.WriteEnd("'{0}' GetTempFileBinaryChunk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetTempFileBinaryChunk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            try
            {
                Log.WriteStart("'{0}' AppendTempFileBinaryChunk", ProviderSettings.ProviderName);
                string result = DatabaseProvider.AppendTempFileBinaryChunk(fileName, path, chunk);
                Log.WriteEnd("'{0}' AppendTempFileBinaryChunk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AppendTempFileBinaryChunk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string BackupDatabase(string databaseName, string backupName, bool zipBackup)
        {
            try
            {
                Log.WriteStart("'{0}' BackupDatabase", ProviderSettings.ProviderName);
                string result = DatabaseProvider.BackupDatabase(databaseName, backupName, zipBackup);
                Log.WriteEnd("'{0}' BackupDatabase", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' BackupDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RestoreDatabase(string databaseName, string[] fileNames)
        {
            try
            {
                Log.WriteStart("'{0}' RestoreDatabase", ProviderSettings.ProviderName);
                DatabaseProvider.RestoreDatabase(databaseName, fileNames);
                Log.WriteEnd("'{0}' RestoreDatabase", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RestoreDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Users
        [WebMethod, SoapHeader("settings")]
        public bool UserExists(string userName)
        {
            try
            {
                Log.WriteStart("'{0}' UserExists", ProviderSettings.ProviderName);
                bool result = DatabaseProvider.UserExists(userName);
                Log.WriteEnd("'{0}' UserExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UserExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetUsers()
        {
            try
            {
                Log.WriteStart("'{0}' GetUsers", ProviderSettings.ProviderName);
                string[] result = DatabaseProvider.GetUsers();
                Log.WriteEnd("'{0}' GetUsers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetUsers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SqlUser GetUser(string username, string[] databases)
        {
            try
            {
                Log.WriteStart("'{0}' GetUser", ProviderSettings.ProviderName);
                SqlUser result = DatabaseProvider.GetUser(username, databases);
                Log.WriteEnd("'{0}' GetUser", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateUser(SqlUser user, string password)
        {
            try
            {
                Log.WriteStart("'{0}' CreateUser", ProviderSettings.ProviderName);
                DatabaseProvider.CreateUser(user, password);
                Log.WriteEnd("'{0}' CreateUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateUser(SqlUser user, string[] databases)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateUser", ProviderSettings.ProviderName);
                DatabaseProvider.UpdateUser(user, databases);
                Log.WriteEnd("'{0}' UpdateUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteUser(string username, string[] databases)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteUser", ProviderSettings.ProviderName);
                DatabaseProvider.DeleteUser(username, databases);
                Log.WriteEnd("'{0}' DeleteUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ChangeUserPassword(string username, string password)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeUserPassword", ProviderSettings.ProviderName);
                DatabaseProvider.ChangeUserPassword(username, password);
                Log.WriteEnd("'{0}' ChangeUserPassword", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeUserPassword", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        public long CalculateDatabaseSize(string database)
        {
            return DatabaseProvider.CalculateDatabaseSize(database);
        }
        #endregion
    }
}
