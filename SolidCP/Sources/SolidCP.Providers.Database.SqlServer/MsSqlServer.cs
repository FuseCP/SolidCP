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
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using Microsoft.Win32;

namespace SolidCP.Providers.Database
{
    public class MsSqlServer : HostingServiceProviderBase, IDatabaseServer
    {
        #region Properties
        protected string ServerName
        {
            get { return ProviderSettings["InternalAddress"]; }
        }

		protected string DatabaseCollation
		{
			get { return ProviderSettings["DatabaseCollation"]; }
		}

        protected string SaLogin
        {
            get { return ProviderSettings["SaLogin"]; }
        }

        protected string SaPassword
        {
            get { return ProviderSettings["SaPassword"]; }
        }

        protected bool UseTrustedConnection
        {
            get { return ProviderSettings.GetBool("UseTrustedConnection"); }
        }

        protected string ConnectionString
        {
            get
            {
                string connectionString = String.Format("Server={0};User id={1};Password={2};Database=master;",
                    ServerName, SaLogin, SaPassword);

                if (UseTrustedConnection)
                    connectionString = String.Format("Server={0};Integrated security=SSPI;Database=master;",
                    ServerName);

                return connectionString;
            }
        }
        #endregion

        #region Databases
        private string GetSafeConnectionString(string databaseName, string username, string password)
        {
            return String.Format("Server={0};User id={1};Password={2};Database={3};",
                                ServerName, username, password, databaseName);
        }

        public virtual bool CheckConnectivity(string databaseName, string username, string password)
        {
            SqlConnection conn = new SqlConnection(String.Format("Server={0};Database={1};User id={2};Password={3};",
                ServerName, databaseName, username, password));
            try
            {
                conn.Open();
            }
            catch
            {
                return false;
            }
            conn.Close();
            return true;
        }

        public virtual DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText)
        {
            return ExecuteSqlQuery(databaseName, commandText,
                GetSafeConnectionString(databaseName, username, password));
        }

        public virtual void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText)
        {
            ExecuteSqlNonQuery(databaseName, commandText,
                GetSafeConnectionString(databaseName, username, password));
        }

        public virtual DataSet ExecuteSqlQuery(string databaseName, string commandText)
        {
            return ExecuteSqlQuery(databaseName, commandText, ConnectionString);
        }

        public virtual DataSet ExecuteSqlQuery(string databaseName, string commandText, string connectionString)
        {
            commandText = "USE [" + databaseName + "]; " + commandText;
            return ExecuteQuery(commandText, connectionString);
        }

        public virtual void ExecuteSqlNonQuery(string databaseName, string commandText)
        {
            ExecuteSqlNonQuery(databaseName, commandText, ConnectionString);
        }

        public virtual void ExecuteSqlNonQuery(string databaseName, string commandText, string connectionString)
        {
            commandText = "USE [" + databaseName + "]\nGO\n" + commandText;

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                // iterate through "GO" delimited command text
                StringReader reader = new StringReader(commandText);

                SqlCommand command = new SqlCommand();

                connection.Open();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandTimeout = 600; // 10 minutes

                string sql = "";
                while (null != (sql = ReadNextStatementFromStream(reader)))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't run SQL script", ex);
            }

            connection.Close();
        }

        private string ReadNextStatementFromStream(StringReader reader)
        {
            StringBuilder sb = new StringBuilder();
            string lineOfText;

            while (true)
            {
                lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                    {
                        return sb.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                {
                    break;
                }

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        public virtual bool DatabaseExists(string databaseName)
        {
            return (ExecuteQuery(
                String.Format("select name from master..sysdatabases where name = '{0}'", EscapeSql(databaseName))).Tables[0].Rows.Count > 0);
        }

        public virtual string[] GetDatabases()
        {
            DataTable dt = ExecuteQuery("select name from master..sysdatabases where name not in ('master', 'tempdb', 'model', 'msdb')").Tables[0];
            List<string> databases = new List<string>();
            foreach (DataRow dr in dt.Rows)
                databases.Add(dr["name"].ToString());
            return databases.ToArray();
        }

        public virtual SqlDatabase GetDatabase(string databaseName)
        {
            if (!DatabaseExists(databaseName))
                return null;

            SqlDatabase database = new SqlDatabase();
            database.Name = databaseName;

            // get database size
            DataView dvFiles = ExecuteQuery(String.Format(
                "SELECT Status, (Size * 8) AS DbSize, Name, FileName FROM [{0}]..sysfiles", databaseName)).Tables[0].DefaultView;

            foreach (DataRowView drFile in dvFiles)
            {
                int status = (int)drFile["Status"];
                if ((status & 64) == 0)
                {
                    // data file
                    database.DataName = ((string)drFile["Name"]).Trim();
                    database.DataPath = ((string)drFile["FileName"]).Trim();
                    database.DataSize = (int)drFile["DbSize"];
                }
                else
                {
                    // log file
                    database.LogName = ((string)drFile["Name"]).Trim();
                    database.LogPath = ((string)drFile["FileName"]).Trim();
                    database.LogSize = (int)drFile["DbSize"];
                }
            }

            // get database uzers
            database.Users = GetDatabaseUsers(databaseName);
            return database;
        }

        private string CreateFileNameString(string fileName, int fileSize)
        {
            string str = fileSize == 0 ? string.Format(" FILENAME = '{0}' ", fileName) :
                string.Format(" FILENAME = '{0}', MAXSIZE = {1} ", EscapeSql(fileName), fileSize);

            return str;
        }
        
        public virtual void CreateDatabase(SqlDatabase database)
        {
            if (database.Users == null)
                database.Users = new string[0];

            string commandText = "";
            if (String.IsNullOrEmpty(database.Location))
            {
                // load default location
                SqlDatabase dbMaster = GetDatabase("master");
                database.Location = Path.GetDirectoryName(dbMaster.DataPath);
            }
            else
            {
                // subst vars
                database.Location = FileUtils.EvaluateSystemVariables(database.Location);

                // verify folder exists
                if (!Directory.Exists(database.Location))
                    Directory.CreateDirectory(database.Location);
            }

			string collation = String.IsNullOrEmpty(DatabaseCollation) ? "" : " COLLATE " + DatabaseCollation;

            // create command
            string dataFile = Path.Combine(database.Location, database.Name) + "_data.mdf";
            string logFile = Path.Combine(database.Location, database.Name) + "_log.ldf";

            
            commandText = string.Format("CREATE DATABASE [{0}]" +
                    " ON ( NAME = '{1}_data', {2})" +
                    " LOG ON ( NAME = '{3}_log', {4}){5};",
                    database.Name,
                    EscapeSql(database.Name), 
                    CreateFileNameString(dataFile, database.DataSize),
                    EscapeSql(database.Name), 
                    CreateFileNameString(logFile, database.LogSize),
                    collation);
            

            // create database
            ExecuteNonQuery(commandText);

            // grant users access
            UpdateDatabaseUsers(database.Name, database.Users);
        }

        public virtual void UpdateDatabase(SqlDatabase database)
        {
            if (database.Users == null)
                database.Users = new string[0];

            // grant users access
            UpdateDatabaseUsers(database.Name, database.Users);
        }

        public virtual void DeleteDatabase(string databaseName)
        {
            if (!DatabaseExists(databaseName))
                return;

            // get database details
            SqlDatabase db = GetDatabase(databaseName);

            // remove all users from database
            try
            {
                string[] users = GetDatabaseUsers(databaseName);
                foreach (string user in users)
                    RemoveUserFromDatabase(databaseName, user);
            }
            catch
            {
                // ignore user deletion
            }

            // close all connection
            CloseDatabaseConnections(databaseName);

            // drop database
            ExecuteNonQuery(String.Format("DROP DATABASE [{0}]", databaseName));

            // drop database folder if empty
            string dbFolder = Path.GetDirectoryName(db.DataPath);
            try
            {
                if (Directory.GetFileSystemEntries(dbFolder).Length == 0)
                    Directory.Delete(dbFolder);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error deleting '{0}' database folder", dbFolder), ex);
            }
        }

        #endregion

        #region Users
        public virtual bool UserExists(string username)
        {
            return (ExecuteQuery(
                String.Format("select name from master..syslogins where name = '{0}'", EscapeSql(username))).Tables[0].Rows.Count > 0);
        }

        public virtual string[] GetUsers()
        {
            DataTable dt = ExecuteQuery("select name from master..syslogins where isntname = 0 and sysadmin = 0 and password is not null").Tables[0];
            List<string> users = new List<string>();
            foreach (DataRow dr in dt.Rows)
                users.Add(dr["name"].ToString());
            return users.ToArray();
        }

        public virtual SqlUser GetUser(string username, string[] allDatabases)
        {
            // get user information
            SqlUser user = new SqlUser();

            DataView dvUser = ExecuteQuery(String.Format("select dbname from master..syslogins where name = '{0}'",
                EscapeSql(username))).Tables[0].DefaultView;

            user.Name = username;
            user.DefaultDatabase = "";
            if (dvUser.Count > 0)
            {
                object dbname = dvUser[0]["dbname"];
                user.DefaultDatabase = (dbname != null && dbname != DBNull.Value) ? (string)dbname : "";
            }

            // get user databases
            user.Databases = GetUserDatabases(username, allDatabases);

            return user;
        }

        public virtual void CreateUser(SqlUser user, string password)
        {
            if (user.Databases == null)
                user.Databases = new string[0];

            // create user account
            if (user.DefaultDatabase == null || user.DefaultDatabase == "")
                user.DefaultDatabase = "master";

            //ExecuteNonQuery(String.Format("EXEC sp_addlogin '{0}', '{1}', '{2}'",
            //    user.Name, password, user.DefaultDatabase));
            //Fixed create login with "Enforce password policy" disabled.
            ExecuteNonQuery(
                String.Format("CREATE LOGIN [{0}] WITH PASSWORD='{1}', DEFAULT_DATABASE=[{2}], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF",
                    user.Name, EscapeSql(password), user.DefaultDatabase));

            // add access to databases
            foreach (string database in user.Databases)
                AddUserToDatabase(database, user.Name);
        }


        public virtual void UpdateUser(SqlUser user, string[] allDatabases)
        {
            if (user.Databases == null)
                user.Databases = new string[0];

            // update user's default database
            if (user.DefaultDatabase == null || user.DefaultDatabase == "")
                user.DefaultDatabase = "master";

            ExecuteNonQuery(String.Format("EXEC sp_defaultdb '{0}', '{1}'",
                EscapeSql(user.Name), EscapeSql(user.DefaultDatabase)));

            
            // update user databases access
            UpdateUserDatabases(user.Name, user.Databases, allDatabases);

            // change user password if required
            if (user.Password != "")
                ChangeUserPassword(user.Name, user.Password);
        }
        public virtual void DeleteUser(string username, string[] allDatabases)
        {
            // remove user from databases
            string[] userDatabases = GetUserDatabases(username, allDatabases);
            foreach (string database in userDatabases)
                RemoveUserFromDatabase(database, username);

            // close all user connection
            CloseUserConnections(username);

            // drop login
            ExecuteNonQuery(String.Format("EXEC sp_droplogin '{0}'", EscapeSql(username)));
        }

        public virtual void ChangeUserPassword(string username, string password)
        {
            // change user password
            ExecuteNonQuery(String.Format("EXEC sp_password @new='{0}', @loginame='{1}'",
                EscapeSql(password), EscapeSql(username)));
        }

        #endregion

        #region Database log routines
        public virtual void TruncateDatabase(string databaseName)
        {
            SqlDatabase database = GetDatabase(databaseName);
            ExecuteNonQuery(String.Format(@"USE [{0}];BACKUP LOG [{1}] WITH TRUNCATE_ONLY;DBCC SHRINKFILE ('{2}', 1, TRUNCATEONLY);",
                databaseName, databaseName, database.LogName));
        }
        #endregion

        #region Backup databases
        public virtual byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            CheckTempPath(path);

            byte[] buffer = FileUtils.GetFileBinaryChunk(path, offset, length);

            // delete temp file
            if (buffer.Length < length)
                FileUtils.DeleteFile(path);
            return buffer;
        }

        public virtual string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            if (path == null)
            {
                path = Path.Combine(Path.GetTempPath(), fileName);
                if (FileUtils.FileExists(path))
                    FileUtils.DeleteFile(path);
            }
            else
            {
                CheckTempPath(path);
            }

            FileUtils.AppendFileBinaryContent(path, chunk);

            return path;
        }

        public virtual string BackupDatabase(string databaseName, string backupFileName, bool zipBackupFile)
        {
            string bakFile = BackupBak(databaseName, (zipBackupFile ? null : backupFileName));

            /*
            if (createBackup)
            {
                files = BackupBak(databaseName, (zipBackup ? null : backupName));
            }
            else
            {
                files = BackupMdf(databaseName);
            }
             * */

            // zip backup file
            if (zipBackupFile)
            {
                string zipFile = Path.Combine(Path.GetTempPath(), backupFileName);
                string zipRoot = Path.GetDirectoryName(bakFile);

                // zip files
                FileUtils.ZipFiles(zipFile, zipRoot, new string[] { Path.GetFileName(bakFile) });

                // delete data files
                if(String.Compare(bakFile, zipFile, true) != 0)
                    FileUtils.DeleteFile(bakFile);

                bakFile = zipFile;
            }

            return bakFile;
        }

        private string BackupBak(string databaseName, string backupName)
        {
            string tempPath = Path.GetTempPath();
            if (backupName == null)
                backupName = databaseName + ".bak";
            string bakFile = Path.Combine(tempPath, backupName);
            //string backupName = databaseName + " Database Backup";

            // backup database
            ExecuteNonQuery(String.Format(@"BACKUP DATABASE [{0}] TO DISK = N'{1}'", // WITH INIT, NAME = '{2}'
                databaseName, EscapeSql(bakFile)/*, backupName*/));

            return bakFile;
        }

        private string[] BackupMdf(string databaseName)
        {
            string tempPath = Path.GetTempPath();

            // get database files
            SqlDatabase db = GetDatabase(databaseName);
            string[] files = new string[] { db.DataPath, db.LogPath };

            // close current database connections
            CloseDatabaseConnections(databaseName);

            // Detach database
            DetachDatabase(databaseName);

            try
            {

                // copy database files
                string[] destFiles = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    destFiles[i] = Path.Combine(tempPath, Path.GetFileName(files[i]));
                    FileUtils.CopyFile(files[i], destFiles[i]);
                }
                return destFiles;
            }
            catch (Exception ex)
            {
                throw new Exception("Can't detach/copy database", ex);
            }
            finally
            {
                // Attach Database
                AttachDatabase(databaseName, files);
            }
        }

        private void DetachDatabase(string databaseName)
        {
            ExecuteNonQuery(String.Format("sp_detach_db '{0}'", EscapeSql(databaseName)));
        }

        private void AttachDatabase(string databaseName, string[] files)
        {
            string[] sqlFiles = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
                sqlFiles[i] = "N'" + EscapeSql(files[i]) + "'";

            // create command
            string cmdText = String.Format("sp_attach_db N'{0}', {1}",
                EscapeSql(databaseName), String.Join(",", sqlFiles));

            ExecuteNonQuery(cmdText);
        }

        private void AttachSingleFileDatabase(string databaseName, string file)
        {
            // execute command
            ExecuteNonQuery(String.Format("sp_attach_single_file_db @dbname='{0}', @physname=N'{1}'",
                EscapeSql(databaseName), EscapeSql(file)));
        }
        #endregion

        #region Restore databases
        public virtual void RestoreDatabase(string databaseName, string[] files)
        {
            string tempPath = Path.GetTempPath();
            
            //create folder with unique name to avoid getting all files from temp directory
            string zipPath = Path.Combine(tempPath, Guid.NewGuid().ToString());
            
            // store original database information
            SqlDatabase database = GetDatabase(databaseName);

            // unzip uploaded files if required
            List<string> expandedFiles = new List<string>();
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".zip")
                {
                    // unpack file
                    expandedFiles.AddRange(FileUtils.UnzipFiles(file, zipPath));

                    // delete zip archive
                    FileUtils.DeleteFile(file);
                }
                else
                {
                    // just add file to the collection
                    expandedFiles.Add(file);
                }
            }

            files = new string[expandedFiles.Count];
            expandedFiles.CopyTo(files, 0);

            // analyze uploaded files
            bool fromBackup = true;
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".mdf")
                {
                    fromBackup = false;
                    break;
                }
            }

            // restore database
            if (fromBackup)
            {
                // restore from .BAK file
                RestoreFromBackup(database, files);
                //delete temporary folder for zip contents
                if (FileUtils.DirectoryExists(zipPath))
                {
                    FileUtils.DeleteDirectoryAdvanced(zipPath);
                }
            }
            else
            {
                // restore from .MDF, .LDF
                //RestoreFromDataFiles(database, files);
            }
        }

        private void RestoreFromBackup(SqlDatabase database, string[] files)
        {
            // close current database connections
            CloseDatabaseConnections(database.Name);

            if (files.Length == 0)
                throw new ApplicationException("No backup files were uploaded"); // error: no backup files were uploaded

            if (files.Length > 1)
                throw new ApplicationException("Too many files were uploaded"); // error: too many files were uploaded

            string bakFile = files[0];

            try
            {
                // restore database
                // get file list from backup file
                string[][] backupFiles = GetBackupFiles(bakFile);

                if (backupFiles.Length < 1)
                    throw new ApplicationException("Backup set should contain at least 1 logical file");

                // map backup files to existing ones
                string[] movings = new string[backupFiles.Length];
                for (int i = 0; i < backupFiles.Length; i++)
                {
                    string name = backupFiles[i][0];
                    string path = backupFiles[i][1];
                    if (Path.GetExtension(path).ToLower() == ".mdf")
                        path = database.DataPath;
                    else
                        path = database.LogPath;

                    movings[i] = String.Format("MOVE '{0}' TO '{1}'", EscapeSql(name), EscapeSql(path));
                }

                // restore database
                ExecuteNonQuery(String.Format(@"RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE, {2}",
                    database.Name, EscapeSql(bakFile), String.Join(", ", movings)));


                // restore original database users
                UpdateDatabaseUsers(database.Name, database.Users);
            }
            finally
            {
                // delete uploaded files
                FileUtils.DeleteFiles(files);
                
            }
        }

        private void RestoreFromDataFiles(SqlDatabase database, string[] files)
        {
            // close current database connections
            CloseDatabaseConnections(database.Name);

            // detach database
            DetachDatabase(database.Name);

            string[] originalFiles = new string[] { database.DataPath, database.LogPath };

            try
            {
                // backup (rename) original database files
                BackupFiles(originalFiles);
            }
            catch (Exception ex)
            {
                AttachDatabase(database.Name, files);
                throw new Exception("Can't restore database", ex);
            }

            try
            {
                // replace original database files with uploaded ones
                for (int i = 0; i < files.Length; i++)
                {
                    if (Path.GetExtension(files[i]).ToLower() == ".mdf")
                        FileUtils.MoveFile(files[i], database.DataPath);
                    else
                        FileUtils.MoveFile(files[i], database.LogPath);
                }

                // attach database
                if (files.Length == 1)
                {
                    AttachSingleFileDatabase(database.Name, originalFiles[0]);
                }
                else
                {
                    AttachDatabase(database.Name, originalFiles);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number != 5105)
                    throw new ApplicationException("Can't attach database!", ex);

            }
            catch (Exception ex)
            {
                // restore original database files
                RollbackFiles(originalFiles);

                // attach old database
                AttachDatabase(database.Name, files);
                throw new Exception("Can't rollback original database files", ex);
            }
            finally
            {
                // restore original database users
                UpdateDatabaseUsers(database.Name, database.Users);

                // remove old backed up files
                for (int i = 0; i < originalFiles.Length; i++)
                    FileUtils.DeleteFile(originalFiles[i] + "_bak");
            }
        }

        private void BackupFiles(string[] files)
        {
            // just rename old database files
            foreach (string file in files)
                FileUtils.MoveFile(file, file + "_bak");
        }

        private void RollbackFiles(string[] files)
        {
            // just rename old files back
            foreach (string file in files)
                FileUtils.MoveFile(file + "_bak", file);
        }

        private string[][] GetBackupFiles(string file)
        {
            DataView dvFiles = ExecuteQuery(
                String.Format("RESTORE FILELISTONLY FROM DISK = '{0}'", EscapeSql(file))).Tables[0].DefaultView;

            string[][] files = new string[dvFiles.Count][];
            for (int i = 0; i < dvFiles.Count; i++)
            {
                files[i] = new string[]{
										   (string)dvFiles[i]["LogicalName"],
										   (string)dvFiles[i]["PhysicalName"]
									   };
            }
            return files;
        }

        #endregion

        #region private helper methods

        protected int ExecuteNonQuery(string commandText)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand(commandText, conn);
                cmd.CommandTimeout = 300;
                conn.Open();
                int ret = cmd.ExecuteNonQuery();
                conn.Close();
                return ret;
            }
            finally
            {
                // close connection if required
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private DataSet ExecuteQuery(string commandText)
        {
            return ExecuteQuery(commandText, ConnectionString);
        }

        private DataSet ExecuteQuery(string commandText, string connectionString)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);
                adapter.SelectCommand.CommandTimeout = 600; // 10 minutes
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            finally
            {
                // close connection if required
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void UpdateDatabaseUsers(string databaseName, string[] users)
        {
            // current users
            string[] arrCurrentUsers = GetDatabaseUsers(databaseName);
            StringDictionary currentUsers = new StringDictionary();
            foreach (string user in arrCurrentUsers)
                currentUsers.Add(user, user);

            // new users
            StringDictionary newUsers = new StringDictionary();
            foreach (string user in users)
                newUsers.Add(user, user);

            // users to add
            List<string> addedUsers = new List<string>();
            foreach (string user in users)
            {
                if (currentUsers[user] == null)
                    addedUsers.Add(user);
            }

            // users to remove
            List<string> removedUsers = new List<string>();
            foreach (string user in arrCurrentUsers)
            {
                if (newUsers[user] == null)
                    removedUsers.Add(user);
            }

            // grant/revoke DB access
            AddUsersToDatabase(databaseName, addedUsers);
            RemoveUsersFromDatabase(databaseName, removedUsers);
        }

        private void UpdateUserDatabases(string username, string[] databases, string[] allDatabases)
        {
            // current databases
            string[] arrCurrentDatabases = GetUserDatabases(username, allDatabases);
            StringDictionary currentDatabases = new StringDictionary();
            foreach (string database in arrCurrentDatabases)
                currentDatabases.Add(database, database);

            // new databases
            StringDictionary newDatabases = new StringDictionary();
            foreach (string database in databases)
                newDatabases.Add(database, database);

            // databases to add
            StringCollection addedDatabases = new StringCollection();
            foreach (string database in databases)
            {
                if (currentDatabases[database] == null)
                    addedDatabases.Add(database);
            }

            // databases to remove
            StringCollection removedDatabases = new StringCollection();
            foreach (string database in arrCurrentDatabases)
            {
                if (newDatabases[database] == null)
                    removedDatabases.Add(database);
            }

            // grant/revoke DB access
            foreach (string database in addedDatabases)
                AddUserToDatabase(database, username);
            foreach (string database in removedDatabases)
                RemoveUserFromDatabase(database, username);
        }

        private string[] GetDatabaseUsers(string databaseName)
        {
            string cmdText = String.Format(@"
				select su.name FROM [{0}]..sysusers as su
				inner JOIN master..syslogins as sl on su.sid = sl.sid
				where su.hasdbaccess = 1 AND su.islogin = 1 AND su.issqluser = 1 AND su.name <> 'dbo'",
                    databaseName);
            DataView dvUsers = ExecuteQuery(cmdText).Tables[0].DefaultView;

            string[] users = new string[dvUsers.Count];
            for (int i = 0; i < dvUsers.Count; i++)
            {
                users[i] = (string)dvUsers[i]["Name"];
            }
            return users;
        }

        private string[] GetUserDatabases(string username, string[] allDatabases)
        {
            string filter = "";
            if (allDatabases != null)
            {
                if (allDatabases.Length == 0)
                    return new string[] { };

                for (int i = 0; i < allDatabases.Length; i++)
                    allDatabases[i] = "'" + allDatabases[i] + "'";

                filter = String.Format(" AND name IN ({0})", String.Join(", ", allDatabases));
            }

            string cmdText = String.Format(@"
					DECLARE @Username varchar(100)
					SET @Username = '{0}'

					CREATE TABLE #UserDatabases
					(
						Name nvarchar(100) collate database_default
					)

					DECLARE @DbName nvarchar(100)
					DECLARE DatabasesCursor CURSOR FOR
					SELECT name FROM master..sysdatabases
					WHERE (status & 256) = 0 AND (status & 512) = 0 {1}

					OPEN DatabasesCursor

					WHILE (10 = 10)
					BEGIN    --LOOP 10: thru Databases
						FETCH NEXT FROM DatabasesCursor
						INTO @DbName

					--print @DbName

						IF (@@fetch_status <> 0)
						BEGIN
							DEALLOCATE DatabasesCursor
							BREAK
						END

						DECLARE @sql nvarchar(1000)
                SET @sql = 'if exists (select ''' + @DbName + ''' from [' + @DbName + ']..sysusers where name = ''' + @Username + ''') insert into #UserDatabases (Name) values (''' + @DbName + ''')'

						EXECUTE(@sql)

					END

					SELECT Name FROM #UserDatabases

					DROP TABLE #UserDatabases
					", username, filter);
            DataView dvDatabases = ExecuteQuery(cmdText).Tables[0].DefaultView;

            string[] databases = new string[dvDatabases.Count];
            for (int i = 0; i < dvDatabases.Count; i++)
                databases[i] = (string)dvDatabases[i]["Name"];
            return databases;
        }

        private void AddUsersToDatabase(string databaseName, List<string> users)
        {
            foreach (string user in users)
                AddUserToDatabase(databaseName, user);
        }

        private void AddUserToDatabase(string databaseName, string user)
        {
            // grant database access
            try
            {
                ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_grantdbaccess '{1}';",
                    databaseName, EscapeSql(user)));
            }
            catch (SqlException ex)
            {
                if (ex.Number == 15023)
                {
                    // the user already exists in the database
                    // so, try to auto fix his login in the database
                    ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_change_users_login 'Auto_Fix', '{1}';",
                        databaseName, EscapeSql(user)));
                }
                else
                {
                    throw new Exception("Can't add user to database", ex);
                }
            }

            // add database owner
            ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_addrolemember 'db_owner', '{1}';",
                databaseName, EscapeSql(user)));
        }

        private void RemoveUsersFromDatabase(string databaseName, List<string> users)
        {
            foreach (string user in users)
                RemoveUserFromDatabase(databaseName, user);
        }

        private void RemoveUserFromDatabase(string databaseName, string user)
        {
            // change ownership of user's objects
            string[] userObjects = GetUserDatabaseObjects(databaseName, user);
            foreach (string userObject in userObjects)
            {
                try
                {
                    ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_changeobjectowner '{1}.{2}', 'dbo'",
                        databaseName, EscapeSql(user), EscapeSql(userObject)));
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 15505)
                    {
                        // Cannot change owner of object 'user.ObjectName' or one of its child objects because
                        // the new owner 'dbo' already has an object with the same name.

                        // try to rename object before changing owner
                        string renamedObject = user + DateTime.Now.Ticks + "_" + userObject;
                        ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_rename '{1}.{2}', '{3}'",
                            databaseName, EscapeSql(user), EscapeSql(userObject), EscapeSql(renamedObject)));

                        // change owner
                        ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_changeobjectowner '{1}.{2}', 'dbo'",
                            databaseName, EscapeSql(user), EscapeSql(renamedObject)));
                    }
                    else
                    {
                        throw new Exception("Can't change database object owner", ex);
                    }
                }
            }

            // revoke db access
            ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_revokedbaccess '{1}';",
                databaseName, EscapeSql(user)));
        }

        private string[] GetUserDatabaseObjects(string databaseName, string user)
        {
            DataView dvObjects = ExecuteQuery(String.Format("select so.name from [{0}]..sysobjects as so" +
                " inner join [{1}]..sysusers as su on so.uid = su.uid" +
                " where su.name = '{2}'", databaseName, databaseName, EscapeSql(user))).Tables[0].DefaultView;
            string[] objects = new string[dvObjects.Count];
            for (int i = 0; i < dvObjects.Count; i++)
            {
                objects[i] = (string)dvObjects[i]["Name"];
            }
            return objects;
        }

        private void CloseDatabaseConnections(string databaseName)
        {
            DataView dv = ExecuteQuery(
                String.Format(@"SELECT spid FROM master..sysprocesses WHERE dbid = DB_ID('{0}')", EscapeSql(databaseName))).Tables[0].DefaultView;

            // kill processes
            for (int i = 0; i < dv.Count; i++)
                KillProcess((short)(dv[i]["spid"]));
        }

        private void CloseUserConnections(string userName)
        {
            DataView dv = ExecuteQuery(
                String.Format(@"SELECT spid FROM master..sysprocesses WHERE loginame = '{0}'", EscapeSql(userName))).Tables[0].DefaultView;

            // kill processes
            for (int i = 0; i < dv.Count; i++)
                KillProcess((short)(dv[i]["spid"]));
        }

        private void KillProcess(short spid)
        {
            ExecuteNonQuery(String.Format("KILL {0}", spid));
        }

        private string EscapeSql(string s)
        {
            return (s != null) ? s.Replace("'", "''") : null;
        }

        #endregion

        #region IHostingServiceProvier methods
        public override string[] Install()
        {
            List<string> messages = new List<string>();

            // check connectivity
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                messages.Add("Could not connect to the specified SQL Server: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return messages.ToArray();
        }

        public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is SqlUser)
                    {
                        // enable/disable user access to all his databases
                        string[] databases = GetUserDatabases(item.Name, null);
                        foreach (string database in databases)
                        {
                            if (enabled)
                            {
                                // enable access
                                ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_addrolemember 'db_owner', '{1}';",
                                    database, item.Name));
                                ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_droprolemember 'db_denydatareader', '{1}';",
                                    database, item.Name));
                                ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_droprolemember 'db_denydatawriter', '{1}';",
                                    database, item.Name));
                            }
                            else
                            {
                                // disable access
                                ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_droprolemember 'db_owner', '{1}';",
                                    database, item.Name));
                                ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_addrolemember 'db_denydatareader', '{1}';",
                                    database, item.Name));
                                ExecuteNonQuery(String.Format("USE [{0}];EXEC sp_addrolemember 'db_denydatawriter', '{1}';",
                                    database, item.Name));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error switching '{0}' MS SQL database", item.Name), ex);
                }
            }
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is SqlDatabase)
                {
                    try
                    {
                        // delete database
                        DeleteDatabase(item.Name);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' MS SQL Database", item.Name), ex);
                    }
                }
                else if (item is SqlUser)
                {
                    try
                    {
                        // delete user
                        DeleteUser(item.Name, null);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' MS SQL User", item.Name), ex);
                    }
                }
            }
        }

        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)        
        {
            List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();

            // update items with diskspace
            foreach (ServiceProviderItem item in items)
            {
                if (item is SqlDatabase)
                {
                    try
                    {
                        // get database details

                        Log.WriteStart(String.Format("Calculating '{0}' database size", item.Name));

                        SqlDatabase db = GetDatabase(item.Name);

                        // calculate disk space
                        ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
                        diskspace.ItemId = item.Id;
                        diskspace.DiskSpace = (((long)db.DataSize) * 1024) + (((long)db.LogSize) * 1024);
                        itemsDiskspace.Add(diskspace);

                        Log.WriteEnd(String.Format("Calculating '{0}' database size", item.Name));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error calculating '{0}' SQL Server database size", item.Name), ex);
                    }
                }
            }

            return itemsDiskspace.ToArray();
        }

        protected bool IsStringRegistryValueStartWith(string path, string paramName, string paramValue)
        {
            string value = string.Empty;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey(path);
            if (rk != null)
            {
                value = (string)rk.GetValue(paramName, null);
            }

            bool res = !string.IsNullOrEmpty(value) && value.StartsWith(paramValue);

            return res;
        }
        
        protected bool CheckVersion(string version)
        {
            bool res = IsStringRegistryValueStartWith("SOFTWARE\\Microsoft\\MSSQLServer\\MSSQLServer\\CurrentVersion", "CurrentVersion", version);

            if (!res)
                res = IsStringRegistryValueStartWith("SOFTWARE\\Wow6432Node\\Microsoft\\MSSQLServer\\MSSQLServer\\CurrentVersion", "CurrentVersion", version);
            
            //Check instances
            if (!res)
            {
                RegistryKey root = Registry.LocalMachine;
                RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server");
                if (rk == null)
                    rk = root.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Microsoft SQL Server");
                
                if (rk != null)
                {
                    string[] instances = null;
                    object value = rk.GetValue("InstalledInstances");
                    if (value != null && value is string[])
                    {
                        instances = (string[])rk.GetValue("InstalledInstances");
                    }

                    if (instances != null)
                    {
                        foreach (string instance in instances)
                        {
                            string registryPath =
                                string.Format(
                                    "SOFTWARE\\Microsoft\\Microsoft SQL Server\\{0}\\MSSQLServer\\CurrentVersion",
                                    instance);

                            res = IsStringRegistryValueStartWith(registryPath, "CurrentVersion", version);

                            if (!res)
                            {
                                registryPath =
                                    string.Format(
                                        "SOFTWARE\\Wow6432Node\\Microsoft\\Microsoft SQL Server\\{0}\\MSSQLServer\\CurrentVersion",
                                        instance);
                                res = IsStringRegistryValueStartWith(registryPath, "CurrentVersion", version);

                            }

                            if (res)
                                break;
                        }
                    }
                }
            }
            return res;            

        }
        
        public override bool IsInstalled()
        {
            return CheckVersion("8.");
        }
        #endregion
    }
}
