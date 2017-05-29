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
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace SolidCP.Setup
{
	/// <summary>
	/// Sql utils class.
	/// </summary>
	public sealed class SqlUtils
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private SqlUtils()
		{
		}

		public static string BuildDbServerMasterConnectionString(string dbServer, string dbLogin, string dbPassw)
		{
			return BuildDbServerConnectionString(dbServer, "master", dbLogin, dbPassw);
		}

		public static string BuildDbServerConnectionString(string dbServer, string dbName, string dbLogin, string dbPassw)
		{
			if (String.IsNullOrEmpty(dbLogin) && String.IsNullOrEmpty(dbPassw))
			{
				return String.Format("Server={0};Database={1};Integrated Security=SSPI;", dbServer, dbName);
			}
			else
			{
				return String.Format("Server={0};Database={1};User id={2};Password={3};", dbServer, dbName, dbLogin, dbPassw);
			}
		}

		/// <summary>
		/// Check sql connection.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <returns>True if connecion is valid, otherwise false.</returns>
		internal static bool CheckSqlConnection(string connectionString)
		{
			SqlConnection conn = new SqlConnection(connectionString);
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

        /// <summary>
        /// Gets the version of SQL Server instance.
        /// </summary>
		/// <param name="connectionString">Connection string.</param>
        /// <returns>True if connecion is valid, otherwise false.</returns>
        internal static string GetSqlServerVersion(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT SERVERPROPERTY('productversion')", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                string version = "unknown";
                if (reader.HasRows)
                {
                    reader.Read();
                    version = reader[0].ToString();
                    reader.Close();
                }
                return version;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

		/// <summary>
		/// Gets the security mode of SQL Server.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <returns>1 - Windows Authentication mode, 0 - Mixed mode.</returns>
		internal static int GetSqlServerSecurityMode(string connectionString)
		{
			SqlConnection conn = new SqlConnection(connectionString);
			int mode = 0;
			try
			{
				SqlCommand cmd = new SqlCommand("SELECT SERVERPROPERTY('IsIntegratedSecurityOnly')", conn);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

				if (reader.HasRows)
				{
					reader.Read();
					mode = Convert.ToInt32(reader[0]);
					reader.Close();
				}
			}
			catch
			{
			}
			finally
			{
				if (conn != null && conn.State == ConnectionState.Open)
					conn.Close();
			}
			return mode;
		}

		/// <summary>
		/// Get databases.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <returns>Databases.</returns>
		internal static string[] GetDatabases(string connectionString)
		{
			DataSet ds = ExecuteQuery(connectionString, "SELECT Name FROM master..sysdatabases ORDER BY Name");
			string[] ret = new string[ds.Tables[0].Rows.Count];
			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				ret[i] = (string)ds.Tables[0].Rows[i]["Name"];
			}
			return ret;
		}

		internal static int ExecuteStoredProcedure(string connectionString, string name, params SqlParameter[] commandParameters)
        {
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand cmd = new SqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = name;
				cmd.Connection = connection;
				cmd.CommandTimeout = 300;
				//attach parameters
				if (commandParameters != null)
				{
					foreach (SqlParameter p in commandParameters)
					{
						if (p != null)
						{
							// Check for derived output value with no value assigned
							if ((p.Direction == ParameterDirection.InputOutput ||
								p.Direction == ParameterDirection.Input) &&
								(p.Value == null))
							{
								p.Value = DBNull.Value;
							}
							cmd.Parameters.Add(p);
						}
					}
				}
				int ret = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				connection.Close();
				return ret;
			}
        }

		private static int ExecuteNonQuery(string connectionString, string commandText)
		{
			SqlConnection conn = null;
			try
			{
				conn = new SqlConnection(connectionString);
				SqlCommand cmd = new SqlCommand(commandText, conn);
				cmd.CommandTimeout = 300;
				conn.Open();
				int ret = cmd.ExecuteNonQuery();
				return ret;
			}
			finally
			{
				// close connection if required
				if (conn != null && conn.State == ConnectionState.Open)
					conn.Close();
			}
		}

		internal static DataSet ExecuteQuery(string connectionString, string commandText)
		{
			SqlConnection conn = null;
			try
			{
				conn = new SqlConnection(connectionString);
				SqlDataAdapter adapter = new SqlDataAdapter(commandText, conn);
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

        public static DataSet ExecuteSql(string Conn, string Query)
        {
            return ExecuteQuery(Conn, Query);
        }

		/// <summary>
		/// Checks if the database exists.
		/// </summary>
		/// <param name="databaseName">Database name.</param>
		/// <param name="connectionString">Connection string.</param>
		/// <returns>Returns True if the database exists.</returns>
		public static bool DatabaseExists(string connectionString, string databaseName)
		{
			return (ExecuteQuery(connectionString,
				String.Format("select name from master..sysdatabases where name = '{0}'", databaseName)).Tables[0].Rows.Count > 0);
		}
        public static bool IsEmptyDatabase(string ConnStr, string DbName)
        {
            var Tmp = (int)ExecuteSql(ConnStr, string.Format("use [{0}]; select case when exists(select * from information_schema.tables) then 1 else 0 end;", DbName)).Tables[0].Rows[0][0];
            return Tmp == 0;
        }

		/// <summary>
		/// Creates database.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="databaseName">Database name.</param>
		internal static void CreateDatabase(string connectionString, string databaseName)
		{
				// create database in the default location
			string commandText = string.Format("CREATE DATABASE [{0}] COLLATE Latin1_General_CI_AS;", databaseName);
			// create database
			ExecuteNonQuery(connectionString, commandText);
			// grant users access
			//UpdateDatabaseUsers(database.Name, database.Users);
		}

		/// <summary>
		/// Creates database user.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="userName">User name</param>
		/// <param name="password">Password.</param>
		/// <param name="database">Default database.</param>
		internal static bool CreateUser(string connectionString, string userName, string password, string database)
		{
            bool userCreated = false;
			if (!UserExists(connectionString, userName))
			{
				ExecuteNonQuery(connectionString,
					String.Format("CREATE LOGIN {0} WITH PASSWORD='{1}', DEFAULT_DATABASE={2}, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF",
					userName, password, database));
                userCreated = true;
			}
			AddUserToDatabase(connectionString, database, userName);
            return userCreated;
		}

		private static void AddUserToDatabase(string connectionString, string databaseName, string user)
		{
			if (user == "sa")
				return;

			// grant database access
			try
			{
				ExecuteNonQuery(connectionString, 
					string.Format("USE {0};EXEC sp_grantdbaccess '{1}';", databaseName, user));
			}
			catch (SqlException ex)
			{
				if (ex.Number == 15023)
				{
					// the user already exists in the database
					// so, try to auto fix his login in the database
					ExecuteNonQuery(connectionString,
						string.Format("USE {0};EXEC sp_change_users_login 'Auto_Fix', '{1}';", databaseName, user));
				}
				else
				{
					throw new Exception("Can't add user to database", ex);
				}
			}

			// add database owner
			ExecuteNonQuery(connectionString,
				string.Format("USE {0};EXEC sp_addrolemember 'db_owner', '{1}';", databaseName, user));
		}

		/// <summary>
		/// Checks whether specified user exists.
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="username">User name.</param>
		/// <returns>True if specified user exists, otherwise false.</returns>
		internal static bool UserExists(string connectionString, string username)
		{
			return (ExecuteQuery(connectionString,
				string.Format("select name from master..syslogins where name = '{0}'", username)).Tables[0].Rows.Count > 0);
		}

		/// <summary>
		/// Checks whether specified login exists.
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="loginName">Login name.</param>
		/// <returns>True if specified login exists, otherwise false.</returns>
		internal static bool LoginExists(string connectionString, string loginName)
		{
			return (ExecuteQuery(connectionString,
				string.Format("SELECT * FROM sys.server_principals WHERE name = N'{0}'", loginName)).Tables[0].Rows.Count > 0);
		}

		/// <summary>
		/// Deletes login
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="loginName">Login name</param>
		internal static void DeleteLogin(string connectionString, string loginName)
		{
			// drop login
			if (LoginExists(connectionString, loginName))
				ExecuteNonQuery(connectionString, String.Format("DROP LOGIN [{0}]", loginName));
		}

        /// <summary>
        /// Deletes database user
        /// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="username">Username</param>
        internal static void DeleteUser(string connectionString, string username)
        {
            // remove user from databases
            string[] userDatabases = GetUserDatabases(connectionString, username);
            foreach (string database in userDatabases)
                RemoveUserFromDatabase(connectionString, database, username);

            // close all user connection
            CloseUserConnections(connectionString, username);

            // drop login
            ExecuteNonQuery(connectionString, String.Format("EXEC sp_droplogin '{0}'", username));
        }

		/// <summary>
		/// Deletes database
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="databaseName">Database name</param>
		internal static void DeleteDatabase(string connectionString, string databaseName)
		{
			// remove all users from database
			string[] users = GetDatabaseUsers(connectionString, databaseName);
			foreach (string user in users)
			{
				RemoveUserFromDatabase(connectionString, databaseName, user);
			}

			// close all connection
			CloseDatabaseConnections(connectionString, databaseName);

			// drop database
			ExecuteNonQuery(connectionString,
				String.Format("DROP DATABASE {0}", databaseName));
		}

		private static string[] GetDatabaseUsers(string connectionString, string databaseName)
		{
			string cmdText = String.Format(@"
				select su.name FROM {0}..sysusers as su
				inner JOIN master..syslogins as sl on su.sid = sl.sid
				where su.hasdbaccess = 1 AND su.islogin = 1 AND su.issqluser = 1 AND su.name <> 'dbo'",
				databaseName);
			DataView dvUsers = ExecuteQuery(connectionString, cmdText).Tables[0].DefaultView;

			string[] users = new string[dvUsers.Count];
			for (int i = 0; i < dvUsers.Count; i++)
			{
				users[i] = (string)dvUsers[i]["Name"];
			}
			return users;
		}

		private static string[] GetUserDatabaseObjects(string connectionString, string databaseName, string user)
		{
			DataView dvObjects = ExecuteQuery(connectionString,
				String.Format("select so.name from {0}..sysobjects as so" +
				" inner join {1}..sysusers as su on so.uid = su.uid" + 
				" where su.name = '{2}'", databaseName, databaseName, user)).Tables[0].DefaultView;
			string[] objects = new string[dvObjects.Count];
			for (int i = 0; i < dvObjects.Count; i++)
			{
				objects[i] = (string)dvObjects[i]["Name"];
			}
			return objects;
		}

        private static string[] GetUserDatabases(string connectionString, string username)
        {
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
					WHERE (status & 256) = 0 AND (status & 512) = 0

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
					", username);
            DataView dvDatabases = ExecuteQuery(connectionString, cmdText).Tables[0].DefaultView;

            string[] databases = new string[dvDatabases.Count];
            for (int i = 0; i < dvDatabases.Count; i++)
                databases[i] = (string)dvDatabases[i]["Name"];
            return databases;
        }

		private static void CloseDatabaseConnections(string connectionString, string databaseName)
		{
			DataView dv = ExecuteQuery(connectionString,
				String.Format(@"SELECT spid FROM master..sysprocesses WHERE dbid = DB_ID('{0}') AND spid > 50 AND spid <> @@spid", databaseName)).Tables[0].DefaultView;

			// kill processes
			for (int i = 0; i < dv.Count; i++)
			{
				KillProcess(connectionString, (short)(dv[i]["spid"]));
			}
		}

        private static void CloseUserConnections(string connectionString, string userName)
        {
            DataView dv = ExecuteQuery(connectionString,
                String.Format(@"SELECT spid FROM master..sysprocesses WHERE loginame = '{0}'", userName)).Tables[0].DefaultView;

            // kill processes
			for (int i = 0; i < dv.Count; i++)
			{
				KillProcess(connectionString, (short)(dv[i]["spid"]));
			}
        }

		private static void KillProcess(string connectionString, short spid)
		{
			try
			{
				ExecuteNonQuery(connectionString,
					String.Format("KILL {0}", spid));
			}
			catch (SqlException)
			{
			}
		}

		private static void RemoveUserFromDatabase(string connectionString, string databaseName, string user)
		{
			// change ownership of user's objects
			string[] userObjects = GetUserDatabaseObjects(connectionString, databaseName, user);
			foreach (string userObject in userObjects)
			{
				try
				{
					ExecuteNonQuery(connectionString,
						String.Format("USE {0};EXEC sp_changeobjectowner '{1}.{2}', 'dbo'",
						databaseName, user, userObject));
				}
				catch (SqlException ex)
				{
					if (ex.Number == 15505)
					{
						// Cannot change owner of object 'user.ObjectName' or one of its child objects because
						// the new owner 'dbo' already has an object with the same name.

						// try to rename object before changing owner
						string renamedObject = user + DateTime.Now.Ticks + "_" + userObject;
						ExecuteNonQuery(connectionString,
							String.Format("USE {0};EXEC sp_rename '{1}.{2}', '{3}'",
							databaseName, user, userObject, renamedObject));

						// change owner
						ExecuteNonQuery(connectionString,
							String.Format("USE {0};EXEC sp_changeobjectowner '{1}.{2}', 'dbo'",
							databaseName, user, renamedObject));
					}
					else
					{
						throw new Exception("Can't change database object owner", ex);
					}
				}
			}

			// revoke db access
			ExecuteNonQuery(connectionString,
				String.Format("USE {0};EXEC sp_revokedbaccess '{1}';",
				databaseName, user));
		}

		internal static bool IsValidDatabaseName(string name)
		{
			if (name == null || name.Trim().Length == 0 || name.Length > 128)
				return false; 

			return Regex.IsMatch(name, "^[0-9A-z_@#$]+$", RegexOptions.Singleline);
		}

		internal static string BackupDatabase(string connectionString, string databaseName)
		{
			string bakFile = databaseName + ".bak";
			// backup database
			ExecuteNonQuery(connectionString,
				String.Format(@"BACKUP DATABASE [{0}] TO DISK = N'{1}'", // WITH INIT, NAME = '{2}'
				databaseName, bakFile));
			return bakFile;
		}

		internal static void BackupDatabase(string connectionString, string databaseName, out string bakFile, out string position)
		{
			bakFile = databaseName + ".bak";
			position = "1";
			string backupName = "Backup " + DateTime.Now.ToString("yyyyMMddHHmmss");
			// backup database
			ExecuteNonQuery(connectionString,
				String.Format(@"BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH NAME = '{2}'", // WITH INIT, NAME = '{2}'
				databaseName, bakFile, backupName));

			//define last position in backup set
			string query = string.Format("RESTORE HEADERONLY FROM DISK = N'{0}'", bakFile);
			DataSet ds = ExecuteQuery(connectionString, query);
			query = string.Format("BackupName = '{0}'", backupName);
			DataRow[] rows = ds.Tables[0].Select(query, "Position DESC");
			if (rows != null && rows.Length > 0)
				position = rows[0]["Position"].ToString();
		}

		internal static void RestoreDatabase(string connectionString, string databaseName, string bakFile)
		{
			// close current database connections
			CloseDatabaseConnections(connectionString, databaseName);

			// restore database
			ExecuteNonQuery(connectionString,
				String.Format(@"RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE",
					databaseName, bakFile));

		}

		internal static void RestoreDatabase(string connectionString, string databaseName, string bakFile, string position)
		{
			// close current database connections
			CloseDatabaseConnections(connectionString, databaseName);

			// restore database
			ExecuteNonQuery(connectionString,
				String.Format(@"RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH FILE = {2}, REPLACE",
					databaseName, bakFile, position));

		}
	}
}
