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
using System.IO;
using System.Reflection;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using SolidCP.Providers.Common;
using System.Text;
using Data = SolidCP.EnterpriseServer.Data;

namespace SolidCP.UniversalInstaller.Core
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

		public static string BuildMsSqlServerMasterConnectionString(string dbServer, string dbLogin, string dbPassw)
		{
			return BuildMsSqlServerConnectionString(dbServer, "master", dbLogin, dbPassw);
		}

		public static string BuildMySqlServerMasterConnectionString(string dbServer, int port, string dbLogin, string dbPassw)
		{
			return BuildMySqlServerConnectionString(dbServer, port, dbLogin, dbPassw, "mysql");
		}

		public static string BuildMsSqlServerConnectionString(string dbServer, string dbName, string dbLogin, string dbPassw)
		{
			if (String.IsNullOrEmpty(dbLogin) && String.IsNullOrEmpty(dbPassw))
			{
				return String.Format("DbType=MsSql;Server={0};Database={1};Integrated Security=SSPI;", dbServer, dbName);
			}
			else
			{
				return String.Format("DbType=MsSql;Server={0};Database={1};User id={2};Password={3};", dbServer, dbName, dbLogin, dbPassw);
			}
		}

		public static string BuildMySqlServerConnectionString(string server, int port, string user, string password, string database)
		{
			return $"DbType=MySql;Server={server};Port={port};User={user};Password={password};Database={database}";
		}

		public static string BuildSqliteMasterConnectionString(string database, string installationFolder,
			string enterpriseServerPath = null, bool integrated = false)
		{
			if (!Path.IsPathRooted(database))
			{
				if (!database.Contains(Path.DirectorySeparatorChar.ToString()))
				{
					if (!database.Contains('.')) database = $"{database}.sqlite";
					if (integrated) database = Path.Combine(enterpriseServerPath, "App_Data", database);
					else database = Path.Combine("App_Data", database);
				}
				database = Path.GetFullPath(Path.Combine(installationFolder, database));
			}
			return $"DbType=Sqlite;Data Source={database}";
		}

		public static string BuildSqliteConnectionString(string database, string enterpriseServerPath = null, bool integrated = false)
		{
			if (!Path.IsPathRooted(database) && !database.Contains(Path.DirectorySeparatorChar))
			{
				if (!database.Contains('.')) database = $"{database}.sqlite";
				//var integrated = vars.EmbedEnterpriseServer;
				if (integrated) database = Path.Combine(enterpriseServerPath, "App_Data", database);
				else database = Path.Combine("App_Data", database);
			}
			return $"DbType=Sqlite;Data Source={database}";
		}

		public static string BuildConnectionString(Data.DbType type, string server, int port, string database, string user, string password,
			string enterpriseServerPath = null, bool integrated = false)
		{
			switch (type)
			{
				case Data.DbType.SqlServer:
					return BuildMsSqlServerConnectionString(server, database, user, password);
				case Data.DbType.MariaDb:
				case Data.DbType.MySql:
					return BuildMySqlServerConnectionString(server, port, user, password, database);
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					return BuildSqliteConnectionString(database, enterpriseServerPath, integrated);
				default: throw new NotSupportedException($"DbType {type} not supported.");
			}
		}

		public static bool CheckSqlConnection(string connectionString)
		{
			Data.DbType dbtype;
			ParseConnectionString(connectionString, out dbtype, out connectionString);
			switch (dbtype)
			{
				case Data.DbType.SqlServer: return CheckMsSqlConnection(connectionString);
				case Data.DbType.MySql:
				case Data.DbType.MariaDb: return CheckMySqlConnection(connectionString);
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX: return CheckSqliteConnection(connectionString);
				default: return false;
			}
		}

		/// <summary>
		/// Check sql connection.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <returns>True if connecion is valid, otherwise false.</returns>
		public static bool CheckMsSqlConnection(string connectionString)
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

		public static bool CheckMySqlConnection(string connectionString)
		{
			var csb = new ConnectionStringBuilder(connectionString);
			csb["database"] = "mysql";
			connectionString = csb.ToString();
			MySqlConnection conn = new MySqlConnection(connectionString);
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

		public static bool CheckSqliteConnection(string connectionString)
		{
			/*var csb = new ConnectionStringBuilder(connectionString);
			var dbFile = (string)(csb["data source"] ?? "");
			var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			if (!Path.IsPathRooted(dbFile)) dbFile = Path.GetFullPath(Path.Combine(assemblyPath, "..", dbFile));
			var dir = new DirectoryInfo(Path.GetDirectoryName(dbFile)).Exists;*/
			return true;
		}
		/// <summary>
		/// Gets the version of SQL Server instance.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <returns>True if connecion is valid, otherwise false.</returns>
		public static string GetMsSqlServerVersion(string connectionString)
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
		public static int GetMsSqlServerSecurityMode(string connectionString)
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
		public static string[] GetDatabases(string connectionString)
		{
			DataSet ds = ExecuteQuery(connectionString, "SELECT Name FROM master..sysdatabases ORDER BY Name");
			string[] ret = new string[ds.Tables[0].Rows.Count];
			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				ret[i] = (string)ds.Tables[0].Rows[i]["Name"];
			}
			return ret;
		}

		public static int ExecuteStoredProcedure(string connectionString, string name, params SqlParameter[] commandParameters)
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
			Data.DbType dbType;
			string nativeConnectionString;
			ParseConnectionString(connectionString, out dbType, out nativeConnectionString);
			switch (dbType)
			{
				case Data.DbType.SqlServer: return ExecuteNonQueryMsSql(nativeConnectionString, commandText);
				case Data.DbType.MySql:
				case Data.DbType.MariaDb: return ExecuteNonQueryMySql(nativeConnectionString, commandText);
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX: return ExecuteNonQuerySqlite(nativeConnectionString, commandText);
				default: return 0;
			}
		}
		private static int ExecuteNonQueryMsSql(string connectionString, string commandText)
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

		private static int ExecuteNonQueryMySql(string connectionString, string commandText)
		{
			MySqlConnection conn = null;
			try
			{
				conn = new MySqlConnection(connectionString);
				MySqlCommand cmd = new MySqlCommand(commandText, conn);
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

		private static int ExecuteNonQuerySqlite(string connectionString, string commandText)
		{
			SQLiteConnection conn = null;
			try
			{
				conn = new SQLiteConnection(connectionString);
				SQLiteCommand cmd = new SQLiteCommand(commandText, conn);
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

		public static void ParseConnectionString(string connectionString, out Data.DbType dbType, out string nativeConnectionString)
		{
			var csb = new ConnectionStringBuilder(connectionString);
			var dbTypeName = (string)(csb["dbtype"] ?? "");
			csb.Remove("dbtype");
			Enum.TryParse(dbTypeName, out dbType);
			nativeConnectionString = csb.ToString();
		}
		public static DataSet ExecuteQuery(string connectionString, string commandText)
		{
			Data.DbType dbType;
			string nativeConnectionString;
			ParseConnectionString(connectionString, out dbType, out nativeConnectionString);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					return ExecuteQueryMsSql(nativeConnectionString, commandText);
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					return ExecuteQueryMySql(nativeConnectionString, commandText);
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					return ExecuteQuerySqlite(nativeConnectionString, commandText);
				default: return null;
			}
		}
		public static DataSet ExecuteQueryMsSql(string connectionString, string commandText)
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

		public static DataSet ExecuteQueryMySql(string connectionString, string commandText)
		{
			MySqlConnection conn = null;
			try
			{
				conn = new MySqlConnection(connectionString);
				MySqlDataAdapter adapter = new MySqlDataAdapter(commandText, conn);
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

		public static DataSet ExecuteQuerySqlite(string connectionString, string commandText)
		{
			SQLiteConnection conn = null;
			try
			{
				conn = new SQLiteConnection(connectionString);
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(commandText, conn);
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

		public static string ChangeDb(string connectionString, string database)
		{
			var csb = new ConnectionStringBuilder(connectionString);
			csb["database"] = database;
			return csb.ToString();
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
			Data.DbType dbType;
			string nativeConnectionString;
			ParseConnectionString(connectionString, out dbType, out nativeConnectionString);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					return (ExecuteQuery(connectionString,
						String.Format("select name from master..sysdatabases where name = '{0}'", databaseName)).Tables[0].Rows.Count > 0);
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					return (ExecuteQuery(connectionString,
						$"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'").Tables[0].Rows.Count > 0);
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					var csb = new ConnectionStringBuilder(connectionString);
					var dbFile = (string)(csb["data source"] ?? "");
					var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
					if (!Path.IsPathRooted(dbFile)) dbFile = Path.GetFullPath(Path.Combine(assemblyPath, "..", dbFile));
					return File.Exists(dbFile);
				default: return false;
			}
		}
		public static bool IsEmptyDatabase(string ConnStr, string DbName)
		{
			Data.DbType dbType;
			int Tmp;
			ParseConnectionString(ConnStr, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					Tmp = (int)ExecuteSql(ConnStr, string.Format("use [{0}]; select case when exists(select * from information_schema.tables) then 1 else 0 end;", DbName)).Tables[0].Rows[0][0];
					return Tmp == 0;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					Tmp = (int)ExecuteSql(ConnStr, $"SELECT COUNT(DISTINCT `TABLE_NAME`) AS anyAliasName FROM `INFORMATION_SCHEMA`. `COLUMNS` WHERE `table_schema` = '{DbName}';").Tables[0].Rows[0][0];
					return Tmp == 0;
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					var csb = new ConnectionStringBuilder(ConnStr);
					var dbFile = (string)(csb["data source"] ?? "");
					var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
					if (!Path.IsPathRooted(dbFile)) dbFile = Path.GetFullPath(Path.Combine(assemblyPath, "..", dbFile));
					csb["data source"] = dbFile;
					ConnStr = csb.ToString();
					Tmp = (int)ExecuteSql(ConnStr, $"SELECT COUNT(name) FROM sqlite_master").Tables[0].Rows[0][0];
					return Tmp == 0;
				default: return false;
			}
		}

		/// <summary>
		/// Creates database.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="databaseName">Database name.</param>
		public static void CreateDatabase(string connectionString, string databaseName)
		{
			Data.DbType dbType;
			string ConnStr, commandText;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					// create database in the default location
					commandText = string.Format("CREATE DATABASE [{0}] COLLATE Latin1_General_CI_AS;", databaseName);
					// create database
					ExecuteNonQuery(connectionString, commandText);
					// grant users access
					//UpdateDatabaseUsers(database.Name, database.Users);
					break;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					// create database in the default location
					commandText = $"CREATE DATABASE {databaseName};";
					// create database
					ExecuteNonQuery(connectionString, commandText);
					break;
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					var csb = new ConnectionStringBuilder(ConnStr);
					var dbFile = (string)(csb["data source"] ?? "");
					var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
					if (!Path.IsPathRooted(dbFile)) dbFile = Path.GetFullPath(Path.Combine(assemblyPath, "..", dbFile));
					CreateDatabaseSqlite(dbFile);
					break;
				default: break;
			}
		}

		public static void CreateDatabaseSqlite(string dbFile)
		{
			var path = Path.GetDirectoryName(dbFile);
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			SQLiteConnection.CreateFile(dbFile);
		}
		/// <summary>
		/// Creates database user.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="userName">User name</param>
		/// <param name="password">Password.</param>
		/// <param name="database">Default database.</param>
		public static bool CreateUser(string connectionString, string userName, string password, string database)
		{
			Data.DbType dbType;
			string ConnStr, server;
			var csb = new ConnectionStringBuilder(connectionString);
			server = csb["server"] as string;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			bool userCreated = false;
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					if (!UserExists(connectionString, userName))
					{
						ExecuteNonQuery(connectionString,
							String.Format("CREATE LOGIN {0} WITH PASSWORD='{1}', DEFAULT_DATABASE={2}, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF",
							userName, password, database));
						userCreated = true;
					}
					AddUserToDatabase(connectionString, database, userName);
					return userCreated;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					if (!UserExists(connectionString, userName))
					{
						// create user
						var host = server == "localhost" ? server : Environment.MachineName;
						ExecuteNonQuery(connectionString, $"CREATE USER '{userName}'@'{host}' IDENTIFIED BY '{password}';");
						ExecuteNonQuery(connectionString, $"CREATE USER '{userName}'@'%' IDENTIFIED BY '{password}';");
						userCreated = true;
					}
					AddUserToDatabase(connectionString, database, userName);
					return userCreated;
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX: return true;
				default: return false;
			}
		}

		private static void AddUserToDatabase(string connectionString, string databaseName, string user)
		{
			Data.DbType dbType;
			string ConnStr, server;
			var csb = new ConnectionStringBuilder(connectionString);
			server = csb["server"] as string;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
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
					break;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					var host = server == "localhost" ? server : Environment.MachineName;
					ExecuteNonQuery(connectionString, $"GRANT ALL ON {databaseName}.* TO '{user}'@'{host}';");
					ExecuteNonQuery(connectionString, $"GRANT ALL ON {databaseName}.* TO '{user}'@'%';");
					ExecuteNonQuery(connectionString, "FLUSH PRIVILEGES;");
					break;
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					break;
				default: break;
			}
		}

		/// <summary>
		/// Checks whether specified user exists.
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="username">User name.</param>
		/// <returns>True if specified user exists, otherwise false.</returns>
		public static bool UserExists(string connectionString, string username)
		{
			Data.DbType dbType;
			string ConnStr, server;
			var csb = new ConnectionStringBuilder(connectionString);
			server = csb["server"] as string;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					return (ExecuteQuery(connectionString,
						string.Format("select name from master..syslogins where name = '{0}'", username)).Tables[0].Rows.Count > 0);
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					var host = server == "localhost" ? server : Environment.MachineName;
					return 1 == (long)ExecuteQuery(connectionString, $"SELECT EXISTS(SELECT 1 FROM mysql.user WHERE user = '{username}' AND host = '{host}')").Tables[0].Rows[0][0];
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX: return true;
				default: return false;
			}
		}

		/// <summary>
		/// Checks whether specified login exists.
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="loginName">Login name.</param>
		/// <returns>True if specified login exists, otherwise false.</returns>
		public static bool LoginExists(string connectionString, string loginName)
		{
			Data.DbType dbType;
			string ConnStr, server;
			var csb = new ConnectionStringBuilder(connectionString);
			server = csb["server"] as string;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					return (ExecuteQuery(connectionString,
						string.Format("SELECT * FROM sys.server_principals WHERE name = N'{0}'", loginName)).Tables[0].Rows.Count > 0);
				default: return UserExists(connectionString, loginName);
			}
		}

		/// <summary>
		/// Deletes login
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="loginName">Login name</param>
		public static void DeleteLogin(string connectionString, string loginName)
		{
			Data.DbType dbType;
			string ConnStr, server;
			var csb = new ConnectionStringBuilder(connectionString);
			server = csb["server"] as string;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					// drop login
					if (LoginExists(connectionString, loginName))
						ExecuteNonQuery(connectionString, String.Format("DROP LOGIN [{0}]", loginName));
					break;
				default: DeleteUser(connectionString, loginName); break;
			}
		}

		/// <summary>
		/// Deletes database user
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="username">Username</param>
		public static void DeleteUser(string connectionString, string username)
		{
			Data.DbType dbType;
			string ConnStr, server;
			var csb = new ConnectionStringBuilder(connectionString);
			server = csb["server"] as string;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
					// remove user from databases
					string[] userDatabases = GetUserDatabases(connectionString, username);
					foreach (string database in userDatabases)
						RemoveUserFromDatabase(connectionString, database, username);

					// close all user connection
					CloseUserConnections(connectionString, username);

					// drop login
					ExecuteNonQuery(connectionString, String.Format("EXEC sp_droplogin '{0}'", username));
					break;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					var host = server == "localhost" ? server : Environment.MachineName;
					ExecuteNonQuery(connectionString, $"DROP USER '{username}'@'{host}';");
					ExecuteNonQuery(connectionString, $"DROP USER '{username}'@'%';");
					break;
				default: break;
			}
		}

		/// <summary>
		/// Deletes database
		/// </summary>
		/// <param name="connectionString">Connection string</param>
		/// <param name="databaseName">Database name</param>
		public static void DeleteDatabase(string connectionString, string databaseName)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			switch (dbType)
			{
				case Data.DbType.SqlServer:
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
					break;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					// drop database
					ExecuteNonQuery(connectionString,
						String.Format("DROP DATABASE {0}", databaseName));
					break;
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					SQLiteConnection.ClearAllPools();
					var csb = new ConnectionStringBuilder(connectionString);
					var dbFile = (string)(csb["data source"] ?? "");
					var assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
					if (!Path.IsPathRooted(dbFile)) dbFile = Path.GetFullPath(Path.Combine(assemblyPath, "..", dbFile));
					File.Delete(dbFile);
					break;
				default: break;
			}
		}

		private static string[] GetDatabaseUsers(string connectionString, string databaseName)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);

			if (dbType == Data.DbType.SqlServer)
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
			return new string[0];
		}

		private static string[] GetUserDatabaseObjects(string connectionString, string databaseName, string user)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);

			if (dbType == Data.DbType.SqlServer)
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
			return new string[0];
		}

		private static string[] GetUserDatabases(string connectionString, string username)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);

			if (dbType == Data.DbType.SqlServer)
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
			return new string[0];
		}

		private static void CloseDatabaseConnections(string connectionString, string databaseName)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);

			if (dbType == Data.DbType.SqlServer)
			{
				DataView dv = ExecuteQuery(connectionString,
					String.Format(@"SELECT spid FROM master..sysprocesses WHERE dbid = DB_ID('{0}') AND spid > 50 AND spid <> @@spid", databaseName)).Tables[0].DefaultView;

				// kill processes
				for (int i = 0; i < dv.Count; i++)
				{
					KillProcess(connectionString, (short)(dv[i]["spid"]));
				}
			}
		}

		private static void CloseUserConnections(string connectionString, string userName)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);

			if (dbType == Data.DbType.SqlServer)
			{
				DataView dv = ExecuteQuery(connectionString,
					String.Format(@"SELECT spid FROM master..sysprocesses WHERE loginame = '{0}'", userName)).Tables[0].DefaultView;

				// kill processes
				for (int i = 0; i < dv.Count; i++)
				{
					KillProcess(connectionString, (short)(dv[i]["spid"]));
				}
			}
		}

		private static void KillProcess(string connectionString, short spid)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);

			if (dbType == Data.DbType.SqlServer)
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
		}

		private static void RemoveUserFromDatabase(string connectionString, string databaseName, string user)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);

			if (dbType == Data.DbType.SqlServer)
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
		}

		public static bool IsValidDatabaseName(string name)
		{
			if (name == null || name.Trim().Length == 0 || name.Length > 128)
				return false;

			return Regex.IsMatch(name, "^[0-9A-z_@#$]+$", RegexOptions.Singleline);
		}

		public static string BackupDatabase(string connectionString, string databaseName)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			if (dbType == Data.DbType.SqlServer)
			{
				string bakFile = databaseName + ".bak";
				// backup database
				ExecuteNonQuery(connectionString,
					String.Format(@"BACKUP DATABASE [{0}] TO DISK = N'{1}'", // WITH INIT, NAME = '{2}'
					databaseName, bakFile));
				return bakFile;
			}
			return null;
		}

		public static void BackupDatabase(string connectionString, string databaseName, out string bakFile, out string position)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			bakFile = databaseName + ".bak";
			position = "1";
			if (dbType == Data.DbType.SqlServer)
			{
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
		}

		public static void RestoreDatabase(string connectionString, string databaseName, string bakFile)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			if (dbType == Data.DbType.SqlServer)
			{
				// close current database connections
				CloseDatabaseConnections(connectionString, databaseName);

				// restore database
				ExecuteNonQuery(connectionString,
					String.Format(@"RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE",
						databaseName, bakFile));
			}
		}

		public static void RestoreDatabase(string connectionString, string databaseName, string bakFile, string position)
		{
			Data.DbType dbType;
			string ConnStr;
			ParseConnectionString(connectionString, out dbType, out ConnStr);
			if (dbType == Data.DbType.SqlServer)
			{
				// close current database connections
				CloseDatabaseConnections(connectionString, databaseName);

				// restore database
				ExecuteNonQuery(connectionString,
					String.Format(@"RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH FILE = {2}, REPLACE",
						databaseName, bakFile, position));
			}
		}
		public class Script : IDisposable
		{
			public bool IsMySql => DbType == Data.DbType.MySql || DbType == Data.DbType.MariaDb;
			public bool IsSqlite => DbType == Data.DbType.Sqlite || DbType == Data.DbType.SqliteFX;
			public bool IsMsSql => DbType == Data.DbType.SqlServer;
			public Data.DbType DbType = Data.DbType.Unknown;
			public string Delimiter = null;
			public Stream Source;
			public StreamReader Reader;
			public string File = "";

			public Script(Stream stream, string connectionString)
			{
				Source = stream;
				Data.DbType dbtype;
				ParseConnectionString(connectionString, out dbtype, out connectionString);
				DbType = dbtype;
				Reset();
			}
			public Script(string fileName, string connectionString) : this(new FileStream(fileName, FileMode.Open, FileAccess.Read), connectionString) { }

			public void Dispose()
			{
				Source?.Dispose();
				Source = null;
				Reader?.Dispose();
				Reader = null;
			}
			public void Reset()
			{
				Source.Seek(0, SeekOrigin.Begin);
				Reader = new StreamReader(Source);
				if (IsMySql) Delimiter = ";";
			}

			public bool HasDelimiter(string line)
			{
				if (IsMySql)
				{
					var delimiter = Delimiter;
					var match = Regex.Match(line, @"^DELIMITER\s+(.*?)\s*$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
					if (match.Success) Delimiter = match.Groups[1].Value;
					return line.EndsWith(delimiter);
				}
				else if (IsSqlite) return line.EndsWith(";");
				else
				{
					return Regex.IsMatch(line,
						@"^\s*GO\s*$",
						RegexOptions.Singleline | RegexOptions.IgnoreCase);
				}
			}
			public bool IsDelimiter(string cmd) => IsMsSql && HasDelimiter(cmd);

			public string ReadNextStatement()
			{
				StringBuilder sb = new StringBuilder();
				string lineOfText;

				while (true)
				{
					lineOfText = Reader.ReadLine();
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

					if (HasDelimiter(lineOfText))
					{
						if (!IsDelimiter(lineOfText)) sb.Append(lineOfText + Environment.NewLine);
						break;
					}

					sb.Append(lineOfText + Environment.NewLine);
				}

				return sb.ToString();
			}
		}
		public static System.Data.Common.DbConnection MsSqlConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}
		public static System.Data.Common.DbConnection MySqlConnection(string connectionString)
		{
			return new MySqlConnection(connectionString);
		}
		public static System.Data.Common.DbConnection SqliteConnection(string connectionString)
		{
			return new SQLiteConnection(connectionString);
		}
		public static System.Data.Common.DbCommand MsSqlCommand() => new SqlCommand();
		public static System.Data.Common.DbCommand MySqlCommand() => new MySqlCommand();
		public static System.Data.Common.DbCommand SqliteCommand() => new SQLiteCommand();

		public static System.Data.Common.DbConnection DbConnection(string connectionString)
		{
			Data.DbType dbtype;
			ParseConnectionString(connectionString, out dbtype, out connectionString);
			switch (dbtype)
			{
				case Data.DbType.SqlServer: return MsSqlConnection(connectionString);
				case Data.DbType.MySql:
				case Data.DbType.MariaDb: return MySqlConnection(connectionString);
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX: return SqliteConnection(connectionString);
				default: throw new NotSupportedException();
			}
		}

		public static System.Data.Common.DbCommand DbCommand(string connectionString)
		{
			Data.DbType dbtype;
			ParseConnectionString(connectionString, out dbtype, out connectionString);
			switch (dbtype)
			{
				case Data.DbType.SqlServer: return MsSqlCommand();
				case Data.DbType.MySql:
				case Data.DbType.MariaDb: return MySqlCommand();
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX: return SqliteCommand();
				default: throw new NotSupportedException();
			}
		}

		static void ProcessMySqlScript(System.Data.Common.DbConnection connection, string connectionString, Script script, int commandCount = 0,
			Action<int> OnProgressChange = null, Func<string, string> ProcessInstallVariables = null)
		{
			// iterate through delimited command text
			var command = new MySqlScript(connectionString);
			command.Connection = (MySqlConnection)connection;
			command.Delimiter = ";";
			int i = 0;
			command.Query = ProcessInstallVariables?.Invoke(script.Reader.ReadToEnd());
			command.StatementExecuted += (sender, args) =>
				OnProgressChange?.Invoke(Convert.ToInt32(++i * 100 / commandCount));
			command.Error += (sender, args) =>
				throw new Exception("Error executing SQL command: " + args.StatementText, args.Exception);
			command.Execute();
		}

		public static void RunSqlScript(string connectionString, Script script, int commandCount = 0,
			Action<int> OnProgressChange = null, Func<string, string> ProcessInstallVariables = null,
			string database = null)
		{

			var connection = DbConnection(connectionString);
			connection.Open();
			string sql;
			int i = 0;

			try
			{
				// iterate through delimited command text
				var command = DbCommand(connectionString);
				command.Connection = connection;
				command.CommandType = System.Data.CommandType.Text;
				command.CommandTimeout = 600;

				if (!string.IsNullOrEmpty(database))
				{
					if (script.IsMsSql)
					{
						command.CommandText = $"USE [{database}]";
						command.ExecuteNonQuery();
					}
					else if (script.IsMySql)
					{
						command.CommandText = $"USE {database}";
						command.ExecuteNonQuery();
					}
				}

				if (!script.IsMySql)
				{
					while (null != (sql = script.ReadNextStatement()))
					{
						sql = ProcessInstallVariables?.Invoke(sql);
						command.CommandText = sql;
						try
						{
							command.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							throw new Exception("Error executing SQL command: " + sql, ex);
						}

						i++;
						if (commandCount != 0)
						{
							OnProgressChange?.Invoke(Convert.ToInt32(i * 100 / commandCount));
						}
					}
				}
				else
				{
					ProcessMySqlScript(connection, connectionString, script, commandCount, OnProgressChange, ProcessInstallVariables);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Can't run SQL script " + script.File, ex);
			}
			finally
			{
				connection.Close();
			}
		}

		public static void RunSqlScript(string connectionString, Stream sql, Action<int> OnProgressChange = null,
			Action<int> SetCommandCount = null, Func<string, string> ProcessInstallVariables = null, string scriptFile = "",
			string database = null)
		{
			using (var script = new Script(sql, connectionString) { File = scriptFile })
			{
				int commandCount = 0;
				try
				{
					while (null != script.ReadNextStatement())
					{
						commandCount++;
					}
				}
				catch (Exception ex)
				{
					throw new Exception("Can't read SQL script " + script.File, ex);
				}

				SetCommandCount?.Invoke(commandCount);

				script.Reset();

				RunSqlScript(connectionString, script, commandCount, OnProgressChange, ProcessInstallVariables, database);
			}
		}
	}
}
