﻿// Copyright (c) 2016, SolidCP
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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;
using System.Diagnostics;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.IO;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using SolidCP.Providers.OS;
using SolidCP.Providers;
using System.Reflection;
using System.Data.Common;

namespace SolidCP.Providers.Database
{
	public class MariaDB101 : HostingServiceProviderBase, IDatabaseServer
	{

		#region Properties
		protected string BackupTempFolder
		{
			get { return Path.GetTempPath(); }
		}

		protected string MariaDBBinFolder
		{
			get { return Path.Combine(InstallFolder, "Bin"); }
		}

		protected int ServerPort
		{
			get
			{
				string addr = ProviderSettings["InternalAddress"];
				if (String.IsNullOrEmpty(addr))
					return 3306;

				int idx = addr.IndexOfAny(new char[] { ':', ',' });
				if (idx == -1)
					return 3306;

				return Int32.Parse(addr.Substring(idx + 1));
			}
		}

		protected string ServerName
		{
			get
			{
				string addr = ProviderSettings["InternalAddress"];
				if (String.IsNullOrEmpty(addr))
					return addr;

				int idx = addr.IndexOfAny(new char[] { ':', ',' });
				if (idx == -1)
					return addr;

				return addr.Substring(0, idx);
			}
		}

		protected string InstallFolder
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["InstallFolder"]); }
		}

		protected string RootLogin
		{
			get { return ProviderSettings["RootLogin"]; }
		}

		protected string RootPassword
		{
			get { return ProviderSettings["RootPassword"]; }
		}

		protected bool OldPassword
		{
			get
			{
				return !String.IsNullOrEmpty(ProviderSettings["OldPassword"])
			 ? Boolean.Parse(ProviderSettings["OldPassword"]) : false;
			}
		}

		public string ConnectionString
		{
			get
			{
				return String.Format("server={0};port={1};database=mysql;uid={2};password={3}",
					 ServerName, ServerPort, RootLogin, RootPassword);
			}
		}

		#endregion

		#region Static ctor

		static MariaDB101()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			// We are able to use the MySQL .NET connector for MariaDB

			//
			if (!args.Name.Contains("MySql.Data"))
				return null;

			if (args.Name.Contains("MySql.Data.resources"))
				return null;


			string connectorKeyName = "SOFTWARE\\MySQL AB\\MySQL Connector/Net";
			string connectorVersion = String.Empty;
			//
			if (PInvoke.RegistryHive.HKLM.SubKeyExists_x86(connectorKeyName))
			{
				connectorVersion = PInvoke.RegistryHive.HKLM.GetSubKeyValue_x86(connectorKeyName, "Version");
			}



			string assemblyFullName = string.Format("MySql.Data, Version={0}.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d", connectorVersion);

			if (assemblyFullName == args.Name)
			{
				return null; //avoid of stack overflow
			}


			return Assembly.Load(assemblyFullName);

		}

		#endregion

		#region Databases
		private string GetSafeConnectionString(string databaseName, string username, string password)
		{
			return String.Format("server={0};port={1};database={2};uid={3};password={4}",
				 ServerName, ServerPort, databaseName, username, password);
		}

		public virtual bool CheckConnectivity(string databaseName, string username, string password)
		{
			MySqlConnection conn = new MySqlConnection(
					  String.Format("server={0};port={1};database={2};uid={3};password={4}",
									  ServerName,
									  ServerPort,
									  databaseName,
									  username,
									  password)
					  );
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

		public virtual DataSet ExecuteSqlQuery(string databaseName, string commandText)
		{
			commandText = "USE `" + databaseName + "`; " + commandText;
			return ExecuteQueryDataSet(commandText);
		}

		public virtual void ExecuteSqlNonQuery(string databaseName, string commandText)
		{
			commandText = "USE `" + databaseName + "`;\n" + commandText;
			ExecuteNonQuery(commandText);
		}

		public virtual DataSet ExecuteSqlQuerySafe(string databaseName, string username, string password, string commandText)
		{
			commandText = "USE `" + databaseName + "`; " + commandText;
			return ExecuteQueryDataSet(commandText, GetSafeConnectionString(databaseName, username, password));
		}

		public virtual void ExecuteSqlNonQuerySafe(string databaseName, string username, string password, string commandText)
		{
			commandText = "USE `" + databaseName + "`;\n" + commandText;
			ExecuteNonQuery(commandText, GetSafeConnectionString(databaseName, username, password));
		}

		public virtual bool DatabaseExists(string databaseName)
		{
			DataTable dvDatabases = ExecuteQuery("SHOW DATABASES");
			DataView dvDatabase = new DataView(dvDatabases, String.Format("Database='{0}'",
				 databaseName), "", DataViewRowState.CurrentRows);
			return (dvDatabase.Count > 0);
		}

		public virtual string[] GetDatabases()
		{
			DataTable dt = ExecuteQuery("SHOW DATABASES");
			List<string> databases = new List<string>();
			foreach (DataRow dr in dt.Rows)
			{
				if (!Convert.IsDBNull(dr["Database"]))
				{
					databases.Add(Convert.ToString(dr["Database"]));
				}
			}
			return databases.ToArray();
		}

		public virtual SqlDatabase GetDatabase(string databaseName)
		{
			if (!DatabaseExists(databaseName))
				return null;

			SqlDatabase database = new SqlDatabase();

			database.Name = databaseName;

			// calculate database size
			DataView dvTables = ExecuteQuery(String.Format("SHOW TABLE STATUS FROM `{0}`", databaseName)).DefaultView;
			long data = 0;
			long index = 0;

			foreach (DataRowView drTable in dvTables)
			{
				//
				if (!Convert.IsDBNull(drTable["Data_length"]))
				{
					data += Convert.ToInt64(drTable["Data_length"]);
				}
				//
				if (!Convert.IsDBNull(drTable["Index_length"]))
				{
					index += Convert.ToInt64(drTable["Index_length"]);
				}
			}

			//size in KB
			database.DataSize = (int)(data + index) / 1024;

			// get database uzers
			database.Users = GetDatabaseUsers(databaseName);

			return database;
		}

		public virtual void CreateDatabase(SqlDatabase database)
		{
			if (database.Users == null)
				database.Users = new string[0];

			/*if (!((Regex.IsMatch(database.Name, @"[^\w\.-]")) && (database.Name.Length > 40)))
			{
				 Exception ex = new Exception("INVALID_DATABASE_NAME");
				 throw ex;
			}
			 */

			// create database
			ExecuteNonQuery(String.Format("CREATE DATABASE IF NOT EXISTS `{0}`;", database.Name));

			// grant users access
			foreach (string user in database.Users)
				AddUserToDatabase(database.Name, user);
		}

		public virtual void UpdateDatabase(SqlDatabase database)
		{
			if (database.Users == null)
				database.Users = new string[0];

			// remove all users from database
			string[] users = GetDatabaseUsers(database.Name);
			foreach (string user in users)
				RemoveUserFromDatabase(database.Name, user);

			// grant users access
			foreach (string user in database.Users)
				AddUserToDatabase(database.Name, user);
		}

		public virtual void DeleteDatabase(string databaseName)
		{
			if (!DatabaseExists(databaseName))
				return;

			// remove all users from database
			string[] users = GetDatabaseUsers(databaseName);
			foreach (string user in users)
				RemoveUserFromDatabase(databaseName, user);

			// close all connection
			CloseDatabaseConnections(databaseName);

			// drop database
			ExecuteNonQuery(String.Format("DROP DATABASE IF EXISTS `{0}`", databaseName));
		}

		#endregion

		#region Users
		public virtual bool UserExists(string username)
		{
			return (ExecuteQuery(String.Format("SELECT user FROM user WHERE user = '{0}'",
			username)).DefaultView.Count > 0);
		}

		public virtual string[] GetUsers()
		{
			DataTable dt = ExecuteQuery("select user from user");
			List<string> users = new List<string>();
			foreach (DataRow dr in dt.Rows)
				users.Add(dr["user"].ToString());
			return users.ToArray();
		}

		public virtual SqlUser GetUser(string username, string[] databases)
		{
			// get user information
			SqlUser user = new SqlUser();
			user.Name = username;

			// get user databases
			user.Databases = GetUserDatabases(username);

			return user;
		}

		public virtual void CreateUser(SqlUser user, string password)
		{
			if (user.Databases == null)
				user.Databases = new string[0];

			/*if (!((Regex.IsMatch(user.Name, @"[^\w\.-]")) && (user.Name.Length > 16)))
			{
				 Exception ex = new Exception("INVALID_USERNAME");
				 throw ex;
			}
			*/
			ExecuteNonQuery(String.Format(
									  "GRANT USAGE ON mysql.* TO '{0}'@'%' IDENTIFIED BY '{1}'",
									  user.Name, password));

			if (OldPassword)
				ChangeUserPassword(user.Name, password);

			// add access to databases
			foreach (string database in user.Databases)
				AddUserToDatabase(database, user.Name);
		}

		public virtual void UpdateUser(SqlUser user, string[] allDatabases)
		{
			if (user.Databases == null)
				user.Databases = new string[0];

			// update user databases access
			string[] databases = GetUserDatabases(user.Name);
			foreach (string database in databases)
				RemoveUserFromDatabase(database, user.Name);

			foreach (string database in user.Databases)
				AddUserToDatabase(database, user.Name);

			// change user password if required
			if (!String.IsNullOrEmpty(user.Password))
				ChangeUserPassword(user.Name, user.Password);
		}

		public virtual void DeleteUser(string username, string[] databases)
		{
			ExecuteNonQuery(String.Format(
				 @"DELETE FROM mysql.user WHERE User='{0}' AND Host = '%';
			DELETE FROM mysql.db WHERE User='{0}' AND Host = '%';
			DELETE FROM mysql.tables_priv WHERE User='{0}' AND Host = '%';
			DELETE FROM mysql.columns_priv WHERE User='{0}' AND Host = '%';
			FLUSH PRIVILEGES;", username));
		}

		public virtual void ChangeUserPassword(string username, string password)
		{
			string pswMode = OldPassword ? "OLD_" : "";
			ExecuteNonQuery(String.Format("SET PASSWORD FOR '{0}'@'%' = {1}PASSWORD('{2}')",
				 username, pswMode, password));
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

		public virtual string BackupDatabase(string databaseName, string backupName, bool zipBackup)
		{
			string bakFile = BackupDatabase(databaseName, (zipBackup ? null : backupName));

			// zip database files
			if (zipBackup)
			{
				string zipFile = Path.Combine(BackupTempFolder, backupName);

				FileUtils.ZipFiles(zipFile, Path.GetDirectoryName(bakFile), new string[] { Path.GetFileName(bakFile) });

				// delete data files
				if (String.Compare(bakFile, zipFile, true) != 0)
					FileUtils.DeleteFile(bakFile);

				bakFile = zipFile;
			}

			return bakFile;
		}

		private string BackupDatabase(string databaseName, string backupName)
		{
			if (backupName == null) backupName = databaseName + ".sql";

			var exe = OSInfo.IsWindows ? "mysqldump.exe" : "mysqldump";

			string cmd = Path.Combine(MariaDBBinFolder, exe);
			if (!File.Exists(cmd)) cmd = Path.Combine(InstallFolder, exe);
			if (!File.Exists(cmd)) cmd = Shell.Default.Find(exe);
			if (cmd == null) throw new FileNotFoundException($"mysqldump executable not found.");
			
			string bakFile = Path.Combine(BackupTempFolder, backupName);

			cmd = $"\"{cmd}\" --host={ServerName} --port={ServerPort} --user={RootLogin} --password={RootPassword} --opt --skip-extended-insert --skip-quick --skip-comments --result-file=\"{bakFile}\" {databaseName}";

			// backup database
			Shell.Default.Exec(cmd);

			return bakFile;
		}

		public virtual void TruncateDatabase(string databaseName)
		{
			if (databaseName.Contains(" ") || databaseName.Contains("\n")) throw new NotSupportedException("databaseName must not contain whitespace");

			string zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			FileUtils.CreateDirectory(zipPath);

			var exe = OSInfo.IsWindows ? "mysql.exe" : "mysql";

			string mysqlexe = Path.Combine(MariaDBBinFolder, exe);
			if (!File.Exists(mysqlexe)) mysqlexe = Path.Combine(InstallFolder, exe);
			if (!File.Exists(mysqlexe)) mysqlexe = Shell.Default.Find(exe);
			if (mysqlexe == null) throw new FileNotFoundException($"mysql executable not found.");

			var script = $"\"{mysqlexe}\" --host={ServerName} --port={ServerPort} --user={RootLogin} --password={RootPassword} -N -e " +
				$"\"SELECT CONCAT('OPTIMIZE TABLE ', table_name, ';') FROM information_schema.tables WHERE table_schema = {databaseName} AND data_free/1024/1024 > 5;\" " +
				$"| \"{mysqlexe}\" --host={ServerName} --port={ServerPort} --user={RootLogin} --password={RootPassword} {databaseName}";

			var cdir = Environment.CurrentDirectory;
			Environment.CurrentDirectory = zipPath;
			Shell.Default.ExecScript(script);
			Environment.CurrentDirectory = cdir;

			FileUtils.DeleteDirectoryAdvanced(zipPath);

		}

		#endregion

		#region Restore database
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
					if (!FileUtils.DirectoryExists(zipPath))
						FileUtils.CreateDirectory(zipPath);
					string newfile = Path.Combine(zipPath, Path.GetFileName(file));
					FileUtils.MoveFile(file, newfile);
					expandedFiles.Add(newfile);
				}
			}

			files = new string[expandedFiles.Count];
			expandedFiles.CopyTo(files, 0);

			if (files.Length == 0)
				throw new ApplicationException("No backup files were uploaded"); // error: no backup files were uploaded

			if (files.Length > 1)
				throw new ApplicationException("Too many files were uploaded"); // error: too many files were uploaded

			// analyze uploaded files
			bool fromDump = true;
			foreach (string file in files)
			{
				if (Path.GetExtension(file).ToLower() != ".sql")
				{
					fromDump = false;
					break;
				}
			}

			if (fromDump)
			{
				// restore database

				// Check executable
				string MariaDBEXEPath = null;
				if (File.Exists(Path.Combine(MariaDBBinFolder, "mysql.exe")))
				{
					MariaDBEXEPath = MariaDBBinFolder;
					Log.WriteInfo("MariaDBEXE Path: {0}", MariaDBEXEPath);
				}
				else
				{
					MariaDBEXEPath = InstallFolder;
					Log.WriteInfo("MariaDBEXE Path: {0}", MariaDBEXEPath);
				}

				// create temporary batchfile
				string batchfilename = Path.Combine(zipPath, "MariaDB_Restore.bat");
				StreamWriter file = new StreamWriter(batchfilename);
				file.WriteLine("@ECHO OFF");
				file.WriteLine("cls");
				file.WriteLine("set host=%1%");
				file.WriteLine("set port=%2%");
				file.WriteLine("set user=%3%");
				file.WriteLine("set password=%4%");
				file.WriteLine("set dbname=%5%");
				file.WriteLine("set dumpfile=%6%");
				file.WriteLine("\"" + Path.Combine(MariaDBEXEPath, "mysql") + "\" --host=%host% --port=%port% --user=%user% --password=%password% %dbname% < %dumpfile%");
				file.Close();
				// restore from .SQL file
				CloseDatabaseConnections(database.Name);
				try
				{
					string sqlFile = files[0];
					string args = string.Format(" {0} {1} {2} {3} {4} {5}",
						 ServerName, ServerPort,
						 RootLogin, RootPassword, database.Name, Path.GetFileName(sqlFile));

					FileUtils.ExecuteSystemCommand(batchfilename, args);
				}
				finally
				{
					// delete uploaded files
					FileUtils.DeleteFiles(files);
				}
			}
			else
			{
				// do nothing
			}
			//delete temporary folder for zip contents
			if (FileUtils.DirectoryExists(zipPath))
			{
				FileUtils.DeleteDirectoryAdvanced(zipPath);
			}
		}
		#endregion

		#region private helper methods

		private int ExecuteNonQuery(string commandText)
		{
			return ExecuteNonQuery(commandText, ConnectionString);
		}

		private int ExecuteNonQuery(string commandText, string connectionString)
		{
			MySqlConnection conn = new MySqlConnection(connectionString);
			MySqlCommand cmd = new MySqlCommand(commandText, conn);
			conn.Open();
			int ret = cmd.ExecuteNonQuery();
			conn.Close();
			return ret;
		}

		private DataTable ExecuteQuery(string commandText, string connectionString)
		{
			return ExecuteQueryDataSet(commandText, connectionString).Tables[0];
		}

		private DataTable ExecuteQuery(string commandText)
		{
			return ExecuteQueryDataSet(commandText).Tables[0];
		}

		private DataSet ExecuteQueryDataSet(string commandText)
		{
			return ExecuteQueryDataSet(commandText, ConnectionString);
		}

		private DataSet ExecuteQueryDataSet(string commandText, string connectionString)
		{
			MySqlConnection conn = new MySqlConnection(connectionString);
			MySqlDataAdapter adapter = new MySqlDataAdapter(commandText, conn);
			DataSet ds = new DataSet();
			adapter.Fill(ds);
			return ds;
		}

		private string[] GetDatabaseUsers(string databaseName)
		{
			DataTable dtResult = ExecuteQuery(String.Format("SELECT User FROM db WHERE Db='{0}' AND Host='%' AND " +
				 "Select_priv = 'Y' AND " +
				 "Insert_priv = 'Y' AND " +
				 "Update_priv = 'Y' AND  " +
				 "Delete_priv = 'Y' AND  " +
				 "Index_priv = 'Y' AND  " +
				 "Alter_priv = 'Y' AND  " +
				 "Create_priv = 'Y' AND  " +
				 "Drop_priv = 'Y' AND  " +
				 "Create_tmp_table_priv = 'Y' AND  " +
				 "Lock_tables_priv = 'Y'", databaseName.ToLower()));
			//
			List<string> users = new List<string>();
			//
			if (dtResult != null)
			{
				if (dtResult.DefaultView != null)
				{
					DataView dvUsers = dtResult.DefaultView;
					//
					foreach (DataRowView drUser in dvUsers)
					{
						if (!Convert.IsDBNull(drUser["user"]))
						{
							users.Add(Convert.ToString(drUser["user"]));
						}
					}
				}
			}
			//
			return users.ToArray();
		}

		private string[] GetUserDatabases(string username)
		{
			DataTable dtResult = ExecuteQuery(String.Format("SELECT Db FROM db WHERE LOWER(User)='{0}' AND Host='%' AND " +
				 "Select_priv = 'Y' AND " +
				 "Insert_priv = 'Y' AND " +
				 "Update_priv = 'Y' AND  " +
				 "Delete_priv = 'Y' AND  " +
				 "Index_priv = 'Y' AND  " +
				 "Alter_priv = 'Y' AND  " +
				 "Create_priv = 'Y' AND  " +
				 "Drop_priv = 'Y' AND  " +
				 "Create_tmp_table_priv = 'Y' AND  " +
				 "Lock_tables_priv = 'Y'", username.ToLower()));
			//
			List<string> databases = new List<string>();
			//
			//
			if (dtResult != null)
			{
				if (dtResult.DefaultView != null)
				{
					DataView dvDatabases = dtResult.DefaultView;
					//
					foreach (DataRowView drDatabase in dvDatabases)
					{
						if (!Convert.IsDBNull(drDatabase["db"]))
						{
							databases.Add(Convert.ToString(drDatabase["db"]));
						}
					}
				}
			}
			//
			return databases.ToArray();
		}

		private void AddUserToDatabase(string databaseName, string user)
		{
			// grant database access
			ExecuteNonQuery(String.Format("GRANT ALL PRIVILEGES ON `{0}`.* TO '{1}'@'%'",
					  databaseName, user));
		}

		private void RemoveUserFromDatabase(string databaseName, string user)
		{
			// revoke db access
			ExecuteNonQuery(String.Format("REVOKE ALL PRIVILEGES ON `{0}`.* FROM '{1}'@'%'",
					  databaseName, user));
		}

		private void CloseDatabaseConnections(string database)
		{
			DataTable dtProcesses = ExecuteQuery("SHOW PROCESSLIST");
			//
			string filter = String.Format("db = '{0}'", database);
			//
			if (dtProcesses.Columns["db"].DataType == typeof(System.Byte[]))
				filter = String.Format("Convert(db, 'System.String') = '{0}'", database);

			DataView dvProcesses = new DataView(dtProcesses);
			foreach (DataRowView rowSid in dvProcesses)
			{
				string cmdText = String.Format("KILL {0}", rowSid["Id"]);
				try
				{
					ExecuteNonQuery(cmdText);
				}
				catch (Exception ex)
				{
					Log.WriteError("Cannot drop MariaDB connection: " + cmdText, ex);
				}
			}
		}

		public virtual long CalculateDatabaseSize(string database)
		{
			// read mySQL INI file
			string dataPath = null;
			string iniPath = Path.Combine(InstallFolder, "my.ini");
			if (File.Exists(iniPath))
			{
				string[] lines = File.ReadAllLines(iniPath);
				Regex re = new Regex(@"^datadir\s?=\s?\""?(?<path>[^\""\n]*)", RegexOptions.IgnoreCase);
				foreach (string line in lines)
				{
					Match m = re.Match(line);
					if (m.Success)
					{
						dataPath = m.Groups["path"].Value.Trim();
						break;
					}
				}
			}

			if (String.IsNullOrEmpty(dataPath))
				dataPath = Path.Combine(InstallFolder, "data");

			string dbFolder = Path.Combine(dataPath, database);

			Log.WriteStart("Database path: " + dbFolder);

			if (Directory.Exists(dbFolder))
			{
				Log.WriteStart("Data folder exists");
				return FileUtils.CalculateFolderSize(dbFolder);
			}
			return 0;
		}

		#endregion

		#region IHostingServiceProvier methods
		public override string[] Install()
		{
			List<string> messages = new List<string>();

			// check connectivity
			MySqlConnection conn = new MySqlConnection(ConnectionString);
			try
			{
				conn.Open();
			}
			catch (Exception ex)
			{
				messages.Add("Could not connect to the specified MariaDB Server: " + ex.Message);
			}
			finally
			{
				if (conn.State == ConnectionState.Open)
					conn.Close();
			}

			// check installation folder existance
			if (String.IsNullOrEmpty(InstallFolder) ||
				 !FileUtils.DirectoryExists(InstallFolder))
			{
				messages.Add(String.Format("Installation folder '{0}' could not be found", InstallFolder));
			}

			return messages.ToArray();
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
						Log.WriteError(String.Format("Error deleting '{0}' MariaDB Database", item.Name), ex);
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
						Log.WriteError(String.Format("Error deleting '{0}' MariaDB User", item.Name), ex);
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

						// calculate disk space
						ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
						diskspace.ItemId = item.Id;
						diskspace.DiskSpace = CalculateDatabaseSize(item.Name);
						itemsDiskspace.Add(diskspace);

						Log.WriteEnd(String.Format("Calculating '{0}' database size", item.Name));
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error calculating '{0}' MariaDB database size", item.Name), ex);
					}
				}
			}

			return itemsDiskspace.ToArray();
		}
		#endregion

		protected virtual bool IsInstalledWindows(string version)
		{
			RegistryKey HKLM = Registry.LocalMachine;

			RegistryKey key = HKLM.OpenSubKey(@$"SOFTWARE\Monty Program AB\MariaDB {version} (x64)");

			if (key == null)
			{
				key = HKLM.OpenSubKey(@$"SOFTWARE\Wow6432Node\Monty Program AB\MariaDB {version}");
				if (key == null) return false;
			}

			return true;
		}

		protected virtual bool IsInstalledUnix(string version)
		{
			var processes = Process.GetProcessesByName("mysqld")
				.Concat(Process.GetProcessesByName("mariadbd"))
				.Select(p => p.ExecutableFile())
				.Concat(new string[] { Shell.Default.Find("mysqld"), Shell.Default.Find("mariadbd") })
				.Where(exe => exe != null)
				.Distinct();
			foreach (var exe in processes)
			{
				if (File.Exists(exe))
				{
					try
					{
						var output = Shell.Default.Exec($"\"{exe}\" --version").Output().Result;
						var match = Regex.Match(output, @"(?<version>[0-9][0-9.]+)(?=.*MariaDB)", RegexOptions.IgnoreCase);
						if (match.Success)
						{
							var ver = match.Groups["version"].Value;
							if (ver.StartsWith(version)) return true;
						}
					}
					catch { }
				}
			}
			return false;
		}

		protected virtual bool IsInstalled(string version)
		{
			if (OS.OSInfo.IsWindows) return IsInstalledWindows(version);
			else return IsInstalledUnix(version);
		}

		public override bool IsInstalled() => IsInstalled("10.1");
	}
}
