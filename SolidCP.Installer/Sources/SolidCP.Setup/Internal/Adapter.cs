using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Install;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Setup.Actions;
using SolidCP.Setup.Common;
using SolidCP.Setup.Web;
using SolidCP.Setup.Windows;
using SolidCP.Providers.OS;
using Data = SolidCP.EnterpriseServer.Data;
using SolidCP.UniversalInstaller.Core;
using System.Windows.Forms;

namespace SolidCP.Setup.Internal
{
    public enum ModeExtension: byte { Normal, Backup, Restore }
	public class WiXSetupException : ApplicationException
	{
        public WiXSetupException():base()
		{

		}
        public WiXSetupException(string Msg):base(Msg)
		{

		}
        public WiXSetupException(string Msg, Exception Inner):base(Msg, Inner)
		{

		}
        public WiXSetupException(SerializationInfo Info, StreamingContext Context):base(Info, Context)
		{

		}
	}
	public static class DictionaryExtension
	{
        public static IDictionary ToNonGenericDictionary(this IDictionary<string,string> Src)
		{
			var Result = new Hashtable();
			foreach (var Pair in Src)
				Result.Add(Pair.Key, Pair.Value);
			return Result;
		}
	}
	public sealed class Adapter
	{
		public static string DefaultConfigFile { get { return "SolidCP.config"; } }
		private Adapter() { }
		public static CheckStatuses CheckASPNET(SetupVariables setupVariables, out string Msg)
		{
			return ConfigurationCheckPage.CheckASPNET(setupVariables, out Msg);
		}
		public static CheckStatuses CheckIIS(SetupVariables setupVariables, out string Msg)
		{
			return ConfigurationCheckPage.CheckIISVersion(setupVariables, out Msg);
		}
		public static CheckStatuses CheckOS(SetupVariables setupVariables, out string Msg)
		{
			return ConfigurationCheckPage.CheckOS(setupVariables, out Msg);
		}
		public static CheckStatuses CheckSql(SetupVariables setupVariables, out string Msg)
		{
			var Result = CheckStatuses.Error;
			try
			{
				var MsgBuilder = new StringBuilder();
				var MsgStr = string.Empty;
				var ConnStr = setupVariables.InstallConnectionString;
				if (CheckConnectionInfo(ConnStr, out MsgStr))
				{
					Data.DbType dbtype;
					string nativeConnectionString;
					Data.DatabaseUtils.ParseConnectionString(ConnStr, out dbtype, out nativeConnectionString);
					if (dbtype == Data.DbType.SqlServer)
					{
						string V = Data.DatabaseUtils.GetSqlServerVersion(ConnStr);
						var Valid = new string[] { "9.", "10.", "11.", "12.", "13.", "14.", "15.", "16." }.Any(x => V.StartsWith(x));
						if (Valid)
							if (Data.DatabaseUtils.GetSqlServerSecurityMode(ConnStr) == 0)
							{
								MsgBuilder.AppendLine("Good connection.");
								Result = CheckStatuses.Success;
							}
							else
								MsgBuilder.AppendLine("Please switch SQL Server authentication to mixed SQL Server and Windows Authentication mode.");
						else
							MsgBuilder.AppendLine("This program can be installed on SQL Server 2005/2008/2012/2014/2016/2017/2019/2022 only.");
					} else
					{
						MsgBuilder.AppendLine("Good connection.");
						Result = CheckStatuses.Success;
					}
				}
				else
				{
					MsgBuilder.AppendLine("SQL Server does not exist or access denied");
					MsgBuilder.AppendLine(MsgStr);
				}
				Msg = MsgBuilder.ToString();
			}
            catch(Exception ex)
			{
				Msg = "Unable to configure the database server." + ex.Message;
			}
			return Result;
		}
		public static bool CheckConnectionInfo(string ConnStr, out string Info)
		{
			Info = string.Empty;
			bool Result = false;
			using (var Conn = new SqlConnection(ConnStr))
			{
				try
				{
					Conn.Open();
					Result = true;
				}
				catch (Exception ex)
				{
					Info = ex.Message;
				}
			}
			return Result;
		}
		public static bool IsAdministrator()
		{
			WindowsIdentity user = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(user);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
		public static bool CheckSecurity()
		{
			try
			{
				PermissionSet set = new PermissionSet(PermissionState.Unrestricted);
				set.Demand();
			}
			catch
			{
				return false;
			}
			return true;
		}
		public static bool SkipCreateDb(string ConnStr, string DbName)
		{
			return Data.DatabaseUtils.DatabaseExists(ConnStr, DbName) && !Data.DatabaseUtils.IsEmptyDatabase(ConnStr, DbName);
		}
	}
	public interface IWiXSetup
	{
		void Run();
	}
	public abstract class WiXSetup : IWiXSetup
	{
		private SetupVariables m_Ctx;
		public WiXSetup(SetupVariables SessionVariables, ModeExtension Ext = ModeExtension.Normal)
		{
			m_Ctx = SessionVariables;
			ModeExtension = Ext;
		}
		public SetupVariables Context { get { return m_Ctx; } }
		public ModeExtension ModeExtension { get; private set; }
		public virtual void Run()
		{
			switch (Context.SetupAction)
			{
				case SetupActions.Install:
					Install();
					break;
				case SetupActions.Uninstall:
					Uninstall();
					break;
				case SetupActions.Setup:
					Maintenance();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		protected abstract void Install();
		protected abstract void Uninstall();
		protected abstract void Maintenance();

		/// <summary>
		/// LoadSetupVariablesFromParameters.
		/// </summary>
		/// <param name="Src"></param>
		/// <param name="Dst"></param>
		public static void FillFromSession(IDictionary<string, string> Src, SetupVariables Dst)
		{
			if (Src == null)
				throw new NullReferenceException("Src");
			var Hash = Src.ToNonGenericDictionary() as Hashtable;
			if (Hash == null)
				throw new NullReferenceException("Hash");
			Dst.ApplicationName = Utils.GetStringSetupParameter(Hash, "ApplicationName");
			Dst.ComponentName = Utils.GetStringSetupParameter(Hash, "ComponentName");
			Dst.ComponentCode = Utils.GetStringSetupParameter(Hash, "ComponentCode");
			Dst.ComponentDescription = Utils.GetStringSetupParameter(Hash, "ComponentDescription");
			Dst.Version = Utils.GetStringSetupParameter(Hash, "Version");

			Dst.InstallationFolder = Utils.GetStringSetupParameter(Hash, "InstallationFolder");

			Dst.InstallerFolder = Utils.GetStringSetupParameter(Hash, "InstallerFolder");
			Dst.Installer = Utils.GetStringSetupParameter(Hash, "Installer");
			Dst.InstallerType = Utils.GetStringSetupParameter(Hash, "InstallerType");
			Dst.InstallerPath = Utils.GetStringSetupParameter(Hash, "InstallerPath");
			//Dst.IISVersion = Utils.GetVersionSetupParameter(Hash, "IISVersion");
			Dst.SetupXml = Utils.GetStringSetupParameter(Hash, "SetupXml");
			Dst.ServerPassword = Utils.GetStringSetupParameter(Hash, Global.Parameters.ServerPassword);
			Dst.UpdateServerPassword = !string.IsNullOrWhiteSpace(Dst.ServerPassword);

			Dst.WebSiteIP = Utils.GetStringSetupParameter(Hash, Global.Parameters.WebSiteIP);
			Dst.WebSitePort = Utils.GetStringSetupParameter(Hash, Global.Parameters.WebSitePort);
			Dst.WebSiteDomain = Utils.GetStringSetupParameter(Hash, Global.Parameters.WebSiteDomain);
			Dst.UserDomain = Utils.GetStringSetupParameter(Hash, Global.Parameters.UserDomain);
			Dst.UserAccount = Utils.GetStringSetupParameter(Hash, Global.Parameters.UserAccount);
			Dst.UserPassword = Utils.GetStringSetupParameter(Hash, Global.Parameters.UserPassword);

			// From portal base install.
			Dst.ConfigurationFile = "Web.config";
			Dst.NewWebSite = true;
			Dst.NewVirtualDirectory = false;
			Dst.EnterpriseServerURL = Utils.GetStringSetupParameter(Hash, Global.Parameters.EnterpriseServerUrl);

			// From ent server base install.
			Dst.ConnectionString = Global.EntServer.AspNetConnectionStringFormat;
			Dst.DatabaseServer = Utils.GetStringSetupParameter(Hash, Global.Parameters.DatabaseServer);
			Dst.Database = Utils.GetStringSetupParameter(Hash, Global.Parameters.DatabaseName);
			Dst.CreateDatabase = false; // Done by WiX itself.
			Dst.NewDatabaseUser = true;
			Dst.ServerAdminPassword = Utils.GetStringSetupParameter(Hash, Global.Parameters.ServerAdminPassword);
			Dst.UpdateServerAdminPassword = true;
			Data.DbType dbType = Data.DbType.Unknown;
			Enum.TryParse(Utils.GetStringSetupParameter(Hash, Global.Parameters.DatabaseType), out dbType);
			Dst.DatabaseType = dbType;
			Dst.DatabasePort = (int)(Utils.GetSetupParameter(Hash, Global.Parameters.DatabasePort) ?? 0);

			switch (Dst.DatabaseType)
			{
				case Data.DbType.SqlServer:
					// DB_LOGIN, DB_PASSWORD.
					bool WinAuth = Utils.GetStringSetupParameter(Hash, "DbAuth").ToLowerInvariant().Equals("Windows Authentication".ToLowerInvariant());
					Dst.DbInstallConnectionString = Data.DatabaseUtils.BuildSqlServerMasterConnectionString(
												Dst.DatabaseServer,
												WinAuth ? null : Utils.GetStringSetupParameter(Hash, Global.Parameters.DbServerAdmin),
												WinAuth ? null : Utils.GetStringSetupParameter(Hash, Global.Parameters.DbServerAdminPassword));
					break;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					Dst.DbInstallConnectionString = Data.DatabaseUtils.BuildMySqlMasterConnectionString(
												Dst.DatabaseServer,
												Dst.DatabasePort,
												Utils.GetStringSetupParameter(Hash, Global.Parameters.DbServerAdmin),
												Utils.GetStringSetupParameter(Hash, Global.Parameters.DbServerAdminPassword));
					break;
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					Dst.DbInstallConnectionString = Data.DatabaseUtils.BuildSqliteMasterConnectionString(Dst.Database,
						Dst.InstallationFolder, Dst.EnterpriseServerPath, Dst.EmbedEnterpriseServer);
					break;
				default: throw new NotSupportedException("This database type is not supported.");
			}

			Dst.BaseDirectory = Utils.GetStringSetupParameter(Hash, Global.Parameters.BaseDirectory);
			Dst.ComponentId = Utils.GetStringSetupParameter(Hash, Global.Parameters.ComponentId);
			Dst.ComponentExists = string.IsNullOrWhiteSpace(Dst.ComponentId) ? false : true;

			Dst.UpdateVersion = Utils.GetStringSetupParameter(Hash, Global.Parameters.Version);
			Dst.DatabaseUser = Utils.GetStringSetupParameter(Hash, Global.Parameters.DatabaseUser);
			Dst.DatabaseUserPassword = Utils.GetStringSetupParameter(Hash, Global.Parameters.DatabaseUserPassword);
			Dst.CryptoKey = Utils.GetStringSetupParameter(Hash, Global.Parameters.CryptoKey);
			Dst.SessionVariables = Src;
		}
		public static string GetFullConfigPath(SetupVariables Ctx)
		{
			return Path.Combine(Ctx.InstallerFolder, Adapter.DefaultConfigFile);
		}
		public static string GetComponentID(SetupVariables Ctx)
		{
			return GetComponentID(GetFullConfigPath(Ctx), Ctx.ComponentCode);
		}
		public static string GetComponentID(string Cfg, string ComponentCode)
		{
			var XmlPath = string.Format("//component[.//add/@key='ComponentCode' and .//add/@value='{0}']", ComponentCode);
			var Xml = new XmlDocument();
			Xml.Load(Cfg);
			var Node = Xml.SelectSingleNode(XmlPath) as XmlElement;
			return Node == null ? null : Node.GetAttribute("id");
		}
		public static void InstallLogListener(object o)
		{
			if (o == null)
				throw new NullReferenceException("log listener");
			if (o is TraceListener)
				Log.Listeners.Add(o as TraceListener);
		}
		public static void InstallFailed()
		{
			throw new WiXSetupException("Installation failed.");
		}
		public static ModeExtension GetModeExtension(IDictionary<string, string> Src)
		{
			var mup = "MODE_UP";
			var mrup = "MODE_RUP";
			var Result = ModeExtension.Normal;
			if ((Src.Keys.Contains(mup) && !string.IsNullOrWhiteSpace(Src[mup])) || (Src.Keys.Contains("ComponentId") && !string.IsNullOrWhiteSpace(Src["ComponentId"])))
				Result = ModeExtension.Restore;
			else if (Src.Keys.Contains(mrup) && !string.IsNullOrWhiteSpace(Src[mrup]))
				Result = ModeExtension.Backup;
			return Result;
		}
	}
	public abstract class SetupScript // ExpressInstallPage, UninstallPage etc
	{
		private int m_Progress;
		private List<InstallAction> m_Actions;
		private SetupVariables m_Ctx;
		public SetupScript(SetupVariables SessionVariables)
		{
			m_Progress = 0;
			m_Actions = new List<InstallAction>();
			m_Ctx = SessionVariables;
		}
		public SetupVariables Context { get { return m_Ctx; } }
		public List<InstallAction> Actions { get { return m_Actions; } }
		public void Run()
		{
			string ComponentName = m_Ctx.ComponentFullName;
			string ComponentID = m_Ctx.ComponentId;
			Version IisVersion = m_Ctx.IISVersion;
			bool iis7 = (IisVersion.Major >= 7);
			var ExecuteActions = GetActions(ComponentID);
			ExecuteActions.AddRange(Actions);
			foreach (var Execute in ExecuteActions)
			{
				try
				{
					switch (Execute.ActionType)
					{
						case ActionTypes.DeleteRegistryKey:
							DeleteRegistryKey(Execute.Key, Execute.Empty);
							break;
						case ActionTypes.DeleteDirectory:
							DeleteDirectory(Execute.Path);
							break;
						case ActionTypes.DeleteDatabase:
							DeleteDatabase(
								Execute.ConnectionString,
								Execute.Name);
							break;
						case ActionTypes.DeleteDatabaseUser:
							DeleteDatabaseUser(
								Execute.ConnectionString,
								Execute.UserName);
							break;
						case ActionTypes.DeleteDatabaseLogin:
							DeleteDatabaseLogin(
								Execute.ConnectionString,
								Execute.UserName);
							break;
						case ActionTypes.DeleteWebSite:
							if (OSInfo.IsWindows)
							{
								if (iis7)
									DeleteIIS7WebSite(Execute.SiteId);
								else
									DeleteWebSite(Execute.SiteId);
							} else
							{
								var a = (IUninstallAction)new InstallServerUnixAction();
								a.Run(Execute.SetupVariables);
							}
							break;
						case ActionTypes.DeleteVirtualDirectory:
							if (OSInfo.IsWindows)
							{
								DeleteVirtualDirectory(
									Execute.SiteId,
									Execute.Name);
							}
							break;
						case ActionTypes.DeleteUserMembership:
							DeleteUserMembership(Execute.Domain, Execute.Name, Execute.Membership);
							break;
						case ActionTypes.DeleteUserAccount:
							DeleteUserAccount(Execute.Domain, Execute.Name);
							break;
						case ActionTypes.DeleteApplicationPool:
							if (OSInfo.IsWindows)
							{
								if (iis7)
									DeleteIIS7ApplicationPool(Execute.Name);
								else
									DeleteApplicationPool(Execute.Name);
							}
							break;
						case ActionTypes.UpdateConfig:
							if (string.IsNullOrWhiteSpace(Execute.Key))
								UpdateSystemConfiguration();
							else
								UpdateSystemConfiguration(Execute.Key);
							break;
						case ActionTypes.DeleteShortcuts:
							DeleteShortcuts(Execute.Name);
							break;
						case ActionTypes.UnregisterWindowsService:
							UnregisterWindowsService(Execute.Path, Execute.Name);
							break;
						// case ActionTypes.UnregisterUnixService:
						//	 UnregisterUnixService(Execute.Path, Execute.Name);
						// break;
						case ActionTypes.SwitchWebPortal2AspNet40:
							SwitchWebPortal2AspNet40(Execute, Context);
							break;
						case ActionTypes.SwitchEntServer2AspNet40:
							SwitchEntServer2AspNet40(Execute, Context);
							break;
						case ActionTypes.SwitchServer2AspNet40:
							SwitchServer2AspNet40(Execute, Context);
							break;
						case ActionTypes.CopyFiles:
							CopyFiles(
								Context.InstallerFolder,
								Context.InstallationFolder);
							break;
						case ActionTypes.CreateWebSite:
							if (OSInfo.IsWindows) CreateWebSite();
							else
							{
								var a = (IInstallAction)new InstallServerUnixAction();
								a.Run(Execute.SetupVariables);
							}
							break;
						case ActionTypes.ConfigureLetsEncrypt:
							if (!ConfigureLetsEncrypt(!string.IsNullOrEmpty(m_Ctx.SetupXml))) {
								Execute.Log = "Failed to install Let's Encrypt certificate. Check the error log for details.";
							}
							break;
						case ActionTypes.CryptoKey:
							SetCryptoKey();
							break;
						case ActionTypes.ServerPassword:
							SetServerPassword();
							break;
						case ActionTypes.UpdateServerPassword:
							UpdateServerPassword();
							break;
						//case ActionTypes.UpdateConfig:
						//    UpdateSystemConfiguration();
						//    break;
						case ActionTypes.CreateDatabase:
							CreateDatabase();
							break;
						case ActionTypes.CreateDatabaseUser:
							CreateDatabaseUser();
							break;
						case ActionTypes.ExecuteSql:
							ExecuteSqlScript(Execute.Path);
							break;
						case ActionTypes.UpdateWebSite:
							UpdateWebSiteBindings();
							break;
						case ActionTypes.Backup:
							Backup();
							break;
						case ActionTypes.DeleteFiles:
							DeleteFiles(Execute.Path);
							break;
						case ActionTypes.UpdateEnterpriseServerUrl:
							UpdateEnterpriseServerUrl();
							break;
						case ActionTypes.CreateShortcuts:
							CreateShortcuts();
							break;
						case ActionTypes.UpdateServers:
							UpdateServers();
							break;
						case ActionTypes.CopyWebConfig:
							CopyWebConfig();
							break;
						case ActionTypes.UpdateWebConfigNamespaces:
							UpdateWebConfigNamespaces();
							break;
						case ActionTypes.StopApplicationPool:
							StopApplicationPool();
							break;
						case ActionTypes.StartApplicationPool:
							StartApplicationPool();
							break;
						case ActionTypes.UpdatePortal2811:
							UpdatePortal2811();
							break;
						case ActionTypes.UpdateEnterpriseServer2810:
						case ActionTypes.UpdateServer2810:
							UpdateWseSecuritySettings();
							break;
						case ActionTypes.CreateUserAccount:
							CreateAccount(Execute.Name);
							break;
						case ActionTypes.ServiceSettings:
							SetServiceSettings();
							break;
						case ActionTypes.RegisterWindowsService:
							RegisterWindowsService();
							break;
						//case ActionTypes.RegisterUnixService:
						//	RegisterUnixService();
						//	break;
						case ActionTypes.StartWindowsService:
							StartWindowsService();
							break;
						case ActionTypes.StopWindowsService:
							StopWindowsService();
							break;
						//case ActionTypes.InitSetupVariables:
						//    InitSetupVaribles(Execute.SetupVariables);
						//    break;
						case ActionTypes.UpdateServerAdminPassword:
							UpdateServerAdminPassword();
							break;
						case ActionTypes.UpdateLicenseInformation:
							UpdateLicenseInformation();
							break;
						case ActionTypes.ConfigureStandaloneServerData:
							ConfigureStandaloneServer(Execute.Url);
							break;
						case ActionTypes.CreateSCPServerLogin:
							CreateSCPServerLogin();
							break;
						case ActionTypes.FolderPermissions:
							ConfigureFolderPermissions();
							break;
						case ActionTypes.AddCustomErrorsPage:
							AddCustomErrorsPage();
							break;
						case ActionTypes.ConfigureSecureSessionModuleInWebConfig:
							ConfigureSecureSessionModuleInWebConfig();
							break;
						case ActionTypes.RestoreConfig:
							RestoreXmlConfigs(Execute.SetupVariables);
							break;
						case ActionTypes.UpdateXml:
							UpdateXml(Execute.Path, Execute.SetupVariables.XmlData);
							break;
						case ActionTypes.SaveConfig:
							SaveComponentConfig(Context);
							break;
						case ActionTypes.DeleteDirectoryFiles:
							DeleteDirectoryFiles(Execute.Path, Execute.FileFilter);
							break;
					}
				}
				catch (Exception ex)
				{
					ProcessError(ex);
					// TODO: Add Rollback
				}
			}
		}
		protected virtual List<InstallAction> GetActions(string ComponentID)
		{
			return new List<InstallAction>();
		}
		protected virtual void ProcessError(Exception ex)
		{

		}
		protected virtual object GetProgressObject()
		{
			return null;
		}
		#region Action Implementations
		#region Uninstall
		private void UnregisterWindowsService(string path, string serviceName)
		{
			try
			{
				Log.WriteStart(string.Format("Removing \"{0}\" Windows service", serviceName));
				Log.WriteStart(string.Format("Stopping \"{0}\" Windows service", serviceName));
				try
				{
					Utils.StopService(serviceName);
					Log.WriteEnd("Stopped Windows service");
				}
				catch (Exception ex)
				{
					if (!Utils.IsThreadAbortException(ex))
						Log.WriteError("Windows service stop error", ex);
				}

				try
				{
					ManagedInstallerClass.InstallHelper(new[] { "/u", path, "/LogFile=" });
				}
				catch
				{
					Log.WriteError(string.Format("Unable to remove \"{0}\" Windows service.", serviceName), null);
					InstallLog.AppendLine(string.Format("- Failed to remove \"{0}\" Windows service", serviceName));
					throw;
				}

				Log.WriteEnd("Removed Windows service");
				InstallLog.AppendLine(string.Format("- Removed \"{0}\" Windows service", serviceName));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Windows service error", ex);
				InstallLog.AppendLine(string.Format("- Failed to remove \"{0}\" Windows service", serviceName));
				throw;
			}
		}

		private void DeleteShortcuts(string fileName)
		{
			try
			{
				Log.WriteStart("Deleting menu shortcut");
				string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
				string path = Path.Combine(programs, "SolidCP Software");
				path = Path.Combine(path, fileName);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				Log.WriteEnd("Deleted menu shortcut");

				Log.WriteStart("Deleting desktop shortcut");
				string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				path = Path.Combine(desktop, fileName);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				Log.WriteEnd("Deleted desktop shortcut");
				InstallLog.AppendLine("- Deleted application shortcuts");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Delete shortcut error", ex);
				InstallLog.AppendLine("- Failed to delete application shortcuts");
				throw;
			}
		}

		private void DeleteDirectory(string path)
		{
			try
			{
				Log.WriteStart("Deleting folder");
				Log.WriteInfo(string.Format("Deleting \"{0}\" folder", path));
				if (FileUtils.DirectoryExists(path))
				{
					FileUtils.DeleteDirectory(path);
					Log.WriteEnd("Deleted folder");
				}
				InstallLog.AppendLine(string.Format("- Deleted \"{0}\" folder", path));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("I/O error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" folder", path));
			}
		}

		private void DeleteDirectoryFiles(string Dir, Func<string, bool> Predicate)
		{
			try
			{
				Log.WriteStart("Deleting directory files");
				Log.WriteInfo(string.Format("Looking in \"{0}\" folder", Dir));
				if (FileUtils.DirectoryExists(Dir))
				{
					FileUtils.DeleteDirectoryFiles(Dir, Predicate);
					Log.WriteEnd("Done");
				}
				InstallLog.AppendLine(string.Format("- Done \"{0}\" folder", Dir));
			}
			catch (Exception ex)
			{
				Log.WriteError("I/O error", ex);
				InstallLog.AppendLine(string.Format("- Failed in \"{0}\" folder", Dir));
			}
		}

		private void DeleteRegistryKey(string subkey, bool deleteEmptyOnly)
		{
			try
			{
				Log.WriteStart("Deleting registry key");
				if (RegistryUtils.RegistryKeyExist(subkey))
				{
					if (deleteEmptyOnly && RegistryUtils.GetSubKeyCount(subkey) != 0)
					{
						Log.WriteEnd(string.Format("Registry key \"{0}\" is not empty", subkey));
						return;
					}
					Log.WriteInfo(string.Format("Deleting registry key \"{0}\"", subkey));
					RegistryUtils.DeleteRegistryKey(subkey);
					Log.WriteEnd("Deleted registry key");
					InstallLog.AppendLine(string.Format("- Deleted registry key \"{0}\"", subkey));
				}
				else
				{
					Log.WriteEnd(string.Format("Registry key \"{0}\" not found", subkey));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Registry key delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete registry key \"{0}\"", subkey));
				throw;
			}
		}

		private void UpdateSystemConfiguration(string componentId)
		{
			try
			{
				Log.WriteStart("Updating system configuration");
				string componentName = AppConfig.GetComponentSettingStringValue(componentId, "ComponentName");
				Log.WriteInfo(string.Format("Deleting \"{0}\" component settings ", componentName));
				XmlUtils.RemoveXmlNode(AppConfig.GetComponentConfig(componentId));
				Log.WriteInfo("Saving system configuration");
				AppConfig.SaveConfiguration();
				Log.WriteEnd("Updated system configuration");
				InstallLog.AppendLine("- Updated system configuration");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Config error", ex);
				InstallLog.AppendLine("- Failed to update system configuration");
				throw;
			}
		}

		private void DeleteDatabase(string connectionString, string database)
		{
			try
			{
				Log.WriteStart("Deleting database");
				Log.WriteInfo(string.Format("Deleting \"{0}\" database", database));
				if (Data.DatabaseUtils.DatabaseExists(connectionString, database))
				{
					Data.DatabaseUtils.DeleteDatabase(connectionString, database);
					Log.WriteEnd("Deleted database");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" database ", database));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" SQL server database ", database));
				throw;
			}
		}

		private void DeleteDatabaseUser(string connectionString, string username)
		{
			try
			{
				Log.WriteStart("Deleting database user");
				Log.WriteInfo(string.Format("Deleting \"{0}\" database user", username));
				if (Data.DatabaseUtils.UserExists(connectionString, username))
				{
					Data.DatabaseUtils.DeleteUser(connectionString, username);
					Log.WriteEnd("Deleted database user");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" database user ", username));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database user delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" SQL server user ", username));
				throw;
			}
		}

		private void DeleteDatabaseLogin(string connectionString, string loginName)
		{
			try
			{
				Log.WriteStart("Deleting SQL server login");
				Log.WriteInfo(string.Format("Deleting \"{0}\" SQL server login", loginName));
				if (Data.DatabaseUtils.LoginExists(connectionString, loginName))
				{
					Data.DatabaseUtils.DeleteLogin(connectionString, loginName);
					Log.WriteEnd("Deleted SQL server login");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" SQL server login ", loginName));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database login delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" SQL server login ", loginName));
				throw;
			}
		}
		private void DeleteUserMembership(string domain, string username, string[] membership)
		{
			try
			{
				Log.WriteStart("Removing user membership");
				if (SecurityUtils.UserExists(domain, username))
				{
					Log.WriteInfo(string.Format("Removing user \"{0}\" membership", username));
					SecurityUtils.RemoveUserFromGroups(domain, username, membership);
					Log.WriteEnd("Removed user membership");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("User membership delete error", ex);
				throw;
			}
		}

		private void DeleteUserAccount(string domain, string username)
		{
			try
			{
				Log.WriteStart("Deleting user account");
				Log.WriteInfo(string.Format("Deleting \"{0}\" user account", username));
				if (SecurityUtils.UserExists(domain, username))
				{
					SecurityUtils.DeleteUser(domain, username);
					Log.WriteEnd("Deleted user account");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" user account ", username));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("User account delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" user account ", username));
				throw;
			}
		}

		private void DeleteApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Deleting application pool");
				Log.WriteInfo(string.Format("Deleting \"{0}\" application pool", name));
				if (WebUtils.ApplicationPoolExists(name))
				{
					int count = WebUtils.GetApplicationPoolSitesCount(name);
					if (count > 0)
					{
						Log.WriteEnd("Application pool is not empty");
					}
					else
					{
						WebUtils.DeleteApplicationPool(name);
						Log.WriteEnd("Deleted  application pool");
						InstallLog.AppendLine(string.Format("- Deleted \"{0}\" application pool ", name));
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" application pool ", name));
				throw;
			}
		}

		private void DeleteIIS7ApplicationPool(string name)
		{
			try
			{
				Log.WriteStart("Deleting application pool");
				Log.WriteInfo(string.Format("Deleting \"{0}\" application pool", name));
				if (WebUtils.IIS7ApplicationPoolExists(name))
				{
					int count = WebUtils.GetIIS7ApplicationPoolSitesCount(name);
					if (count > 0)
					{
						Log.WriteEnd("Application pool is not empty");
					}
					else
					{
						WebUtils.DeleteIIS7ApplicationPool(name);
						Log.WriteEnd("Deleted  application pool");
						InstallLog.AppendLine(string.Format("- Deleted \"{0}\" application pool ", name));
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Application pool delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" application pool ", name));
				throw;
			}
		}

		private void DeleteWebSite(string siteId)
		{
			try
			{
				Log.WriteStart("Deleting web site");
				Log.WriteInfo(string.Format("Deleting \"{0}\" web site", siteId));
				if (WebUtils.SiteIdExists(siteId))
				{
					WebUtils.DeleteSite(siteId);
					Log.WriteEnd("Deleted web site");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" web site ", siteId));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" web site ", siteId));
				throw;
			}
		}

		private void DeleteIIS7WebSite(string siteId)
		{
			try
			{
				Log.WriteStart("Deleting web site");
				Log.WriteInfo(string.Format("Deleting \"{0}\" web site", siteId));
				if (WebUtils.IIS7SiteExists(siteId))
				{
					WebUtils.DeleteIIS7Site(siteId);
					Log.WriteEnd("Deleted web site");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" web site ", siteId));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" web site ", siteId));

				throw;
			}
		}

		private void DeleteVirtualDirectory(string siteId, string name)
		{
			try
			{
				Log.WriteStart("Deleting virtual directory");
				Log.WriteInfo(string.Format("Deleting virtual directory \"{0}\" for the site \"{1}\"", name, siteId));
				if (WebUtils.VirtualDirectoryExists(siteId, name))
				{
					WebUtils.DeleteVirtualDirectory(siteId, name);
					Log.WriteEnd("Deleted  virtual directory");
					InstallLog.AppendLine(string.Format("- Deleted \"{0}\" virtual directory ", name));
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Virtual directory delete error", ex);
				InstallLog.AppendLine(string.Format("- Failed to delete \"{0}\" virtual directory ", name));
				throw;
			}
		}
		#endregion
		#region Express 

		private void ConfigureSecureSessionModuleInWebConfig()
		{
			try
			{
				string webConfigPath = Path.Combine(Context.InstallationFolder, "web.config");
				Log.WriteStart("Web.config file is being updated");
				// Ensure the web.config exists
				if (!File.Exists(webConfigPath))
				{
					Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
					return;
				}
				// Load web.config
				var doc = new XmlDocument();
				doc.Load(webConfigPath);

				// add node:
				//<system.webServer>
				//  <modules>
				//    <add name="SecureSession" type="SolidCP.WebPortal.SecureSessionModule" />
				//  </modules>
				//</system.webServer>
				//
				//  ... or for IIS 6:
				//
				//<system.web>
				//  <httpModules>
				//    <add name="SecureSession" type="SolidCP.WebPortal.SecureSessionModule" />
				//  </httpModules>
				//</system.web>
				bool iis6 = false;
				XmlElement webServer = doc.SelectSingleNode("configuration/system.webServer") as XmlElement;
				if (webServer == null)
				{
					// this is IIS 6
					webServer = doc.SelectSingleNode("configuration/system.web") as XmlElement;
					iis6 = true;
				}

				if (webServer != null)
				{
					string modulesNodeName = iis6 ? "httpModules" : "modules";
					if (webServer.SelectSingleNode(modulesNodeName + "/add[@name='SecureSession']") == null)
					{
						var modules = doc.CreateElement(modulesNodeName);
						webServer.AppendChild(modules);
						var sessionModule = doc.CreateElement("add");
						sessionModule.SetAttribute("name", "SecureSession");
						sessionModule.SetAttribute("type", "SolidCP.WebPortal.SecureSessionModule");
						modules.AppendChild(sessionModule);
					}
				}

				// update /system.web/httpRuntime element
				var httpRuntime = doc.SelectSingleNode("configuration/system.web/httpRuntime") as XmlElement;
				if (httpRuntime != null)
					httpRuntime.SetAttribute("enableVersionHeader", "false");

				// add:
				//<appSettings>
				//    <add key="SessionValidationKey" value="XXXXXX" />
				//</appSettings>
				var appSettings = doc.SelectSingleNode("configuration/appSettings");
				if (appSettings != null && appSettings.SelectSingleNode("add[@key='SessionValidationKey']") == null)
				{
					var sessionKey = doc.CreateElement("add");
					sessionKey.SetAttribute("key", "SessionValidationKey");
					sessionKey.SetAttribute("value", StringUtils.GenerateRandomString(16));
					appSettings.AppendChild(sessionKey);
				}

				// save changes have been made
				doc.Save(webConfigPath);
				//
				Log.WriteEnd("Web.config has been updated");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Could not update web.config file", ex);
				throw;
			}
		}

		private void SwitchWebPortal2AspNet40(InstallAction action, Setup.SetupVariables setupVariables)
		{
			var sam = new WebPortalActionManager(setupVariables);
			sam.AddAction(new RegisterAspNet40Action());
			sam.AddAction(new EnableAspNetWebExtensionAction());
			sam.AddAction(new MigrateWebPortalWebConfigAction());
			sam.AddAction(new SwitchAppPoolAspNetVersion());
			sam.AddAction(new CleanupSolidCPModulesListAction());
			//
			sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
			{
				throw e.OriginalException;
			});
			//
			sam.Start();
		}

		private void SwitchEntServer2AspNet40(InstallAction action, Setup.SetupVariables setupVariables)
		{
			var sam = new EntServerActionManager(setupVariables);
			sam.AddAction(new RegisterAspNet40Action());
			sam.AddAction(new EnableAspNetWebExtensionAction());
			sam.AddAction(new MigrateEntServerWebConfigAction());
			sam.AddAction(new AdjustHttpRuntimeRequestLengthAction());
			sam.AddAction(new SwitchAppPoolAspNetVersion());
			//
			sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
			{
				throw e.OriginalException;
			});
			//
			sam.Start();
		}

		private void SwitchServer2AspNet40(InstallAction action, Setup.SetupVariables setupVariables)
		{
			var sam = new ServerActionManager(setupVariables);
			sam.AddAction(new RegisterAspNet40Action());
			sam.AddAction(new EnableAspNetWebExtensionAction());
			sam.AddAction(new MigrateServerWebConfigAction());
			sam.AddAction(new AdjustHttpRuntimeRequestLengthAction());
			sam.AddAction(new SwitchAppPoolAspNetVersion());
			//
			sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
			{
				throw e.OriginalException;
			});
			//
			sam.Start();
		}

		private void MigrateServerWebConfigFile(Setup.SetupVariables setupVariables)
		{
			// Migrate web.config
			// IIS 6
			if (setupVariables.IISVersion.Major == 6)
			{
			}
			// IIS 7
			else
			{

			}
		}

		private void UpdatePortal2811()
		{
			try
			{
				string webConfigPath = Path.Combine(Context.InstallationFolder, "web.config");
				Log.WriteStart("Web.config file is being updated");
				// Ensure the web.config exists
				if (!File.Exists(webConfigPath))
				{
					Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
					return;
				}
				// Load web.config
				var doc = new XmlDocument();
				doc.Load(webConfigPath);
				// do Windows 2008 platform-specific changes
				bool iis7 = (Context.IISVersion.Major >= 7);
				//
				#region Do IIS 7 and IIS 6 specific web.config file changes
				if (iis7)
				{
					// remove existing node:
					//<system.webServer>
					//	<handlers>
					//		<add name="WebChart" path="WebChart.axd" verb="GET,HEAD" type="blong.WebControls.WebChartImageHandler, WebChart" preCondition="integratedMode,runtimeVersionv2.0" />
					//	</handlers>
					//</system.webServer>
					XmlElement webChartWebServerHandler = doc.SelectSingleNode("configuration/system.webServer/handlers/add[@path='WebChart.axd']") as XmlElement;
					// ensure node is found
					if (webChartWebServerHandler != null)
					{
						var parentNode = webChartWebServerHandler.ParentNode;
						parentNode.RemoveChild(webChartWebServerHandler);
					}
				}
				else
				{
					// remove existing node:
					//<system.web>
					//	<httpHandlers>
					//		<add verb="GET,HEAD" path="WebChart.axd" type="blong.WebControls.WebChartImageHandler, WebChart" validate="false"/>
					//	</httpHandlers>
					//</system.web>
					XmlElement webChartWebServerHandler = doc.SelectSingleNode("configuration/system.web/httpHandlers/add[@path='WebChart.axd']") as XmlElement;
					// ensure node is found
					if (webChartWebServerHandler != null)
					{
						var parentNode = webChartWebServerHandler.ParentNode;
						parentNode.RemoveChild(webChartWebServerHandler);
					}
				}
				#endregion

				#region CompareValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.CompareValidator" mappedTagType="Sample.Web.UI.Compatibility.CompareValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement compareValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.CompareValidator']") as XmlElement;
				// ensure node is found
				if (compareValidatorMapping != null)
				{
					var parentNode = compareValidatorMapping.ParentNode;
					parentNode.RemoveChild(compareValidatorMapping);
				}
				#endregion

				#region CustomValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.CustomValidator" mappedTagType="Sample.Web.UI.Compatibility.CustomValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement customValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.CustomValidator']") as XmlElement;
				// ensure node is found
				if (customValidatorMapping != null)
				{
					var parentNode = customValidatorMapping.ParentNode;
					parentNode.RemoveChild(customValidatorMapping);
				}
				#endregion

				#region RangeValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.RangeValidator" mappedTagType="Sample.Web.UI.Compatibility.RangeValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement rangeValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.RangeValidator']") as XmlElement;
				// ensure node is found
				if (rangeValidatorMapping != null)
				{
					var parentNode = rangeValidatorMapping.ParentNode;
					parentNode.RemoveChild(rangeValidatorMapping);
				}
				#endregion

				#region RegularExpressionValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.RegularExpressionValidator" mappedTagType="Sample.Web.UI.Compatibility.RegularExpressionValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement regExpValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.RegularExpressionValidator']") as XmlElement;
				// ensure node is found
				if (regExpValidatorMapping != null)
				{
					var parentNode = regExpValidatorMapping.ParentNode;
					parentNode.RemoveChild(regExpValidatorMapping);
				}
				#endregion

				#region RequiredFieldValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.RequiredFieldValidator" mappedTagType="Sample.Web.UI.Compatibility.RequiredFieldValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement requiredFieldValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.RequiredFieldValidator']") as XmlElement;
				// ensure node is found
				if (requiredFieldValidatorMapping != null)
				{
					var parentNode = requiredFieldValidatorMapping.ParentNode;
					parentNode.RemoveChild(requiredFieldValidatorMapping);
				}
				#endregion

				#region ValidationSummary
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.ValidationSummary" mappedTagType="Sample.Web.UI.Compatibility.ValidationSummary, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement validationSummaryMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.ValidationSummary']") as XmlElement;
				// ensure node is found
				if (validationSummaryMapping != null)
				{
					var parentNode = validationSummaryMapping.ParentNode;
					parentNode.RemoveChild(validationSummaryMapping);
				}
				#endregion

				#region tagMapping
				// remove existing node only if it does not have any siblings:
				//<system.web>
				//	<pages>
				//		<tagMapping />
				//	</pages>
				//</system.webServer>
				XmlElement tagMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping") as XmlElement;
				// ensure node is found
				if (tagMapping != null && !tagMapping.HasChildNodes)
				{
					var parentNode = tagMapping.ParentNode;
					parentNode.RemoveChild(tagMapping);
				}
				#endregion

				// save changes have been made
				doc.Save(webConfigPath);
				//
				Log.WriteEnd("Web.config has been updated");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Could not update web.config file", ex);
				throw;
			}
		}

		private void UpdateLicenseInformation()
		{
			try
			{
				if (string.IsNullOrEmpty(Context.LicenseKey))
					return;

				Log.WriteStart("Updating license information");

				string path = Path.Combine(Context.InstallationFolder, Context.ConfigurationFile);
				string licenseKey = Context.LicenseKey;

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				string connectionString = GetConnectionString(path);
				if (string.IsNullOrEmpty(connectionString))
				{
					Log.WriteError("Connection string setting not found");
					return;
				}

				string cryptoKey = GetCryptoKey(path);
				if (string.IsNullOrEmpty(cryptoKey))
				{
					Log.WriteError("CryptoKey setting not found");
					return;
				}

				bool encryptionEnabled = IsEncryptionEnabled(path);
				//encrypt password
				if (encryptionEnabled)
				{
					licenseKey = Utils.Encrypt(cryptoKey, licenseKey);
				}

				string query = string.Format("INSERT INTO Licenses ( SerialNumber ) VALUES ('{0}')", licenseKey);
				Data.DatabaseUtils.ExecuteQuery(connectionString, query);

				Log.WriteEnd("Updated license information");
				InstallLog.AppendLine("- Updated license information");

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update error", ex);
				throw;
			}
		}

		private string GetConnectionString(string webConfigPath)
		{
			string ret = null;
			var doc = new XmlDocument();
			doc.Load(webConfigPath);
			//connection string
			string xPath = "configuration/connectionStrings/add[@name=\"EnterpriseServer\"]";
			XmlElement connectionStringNode = doc.SelectSingleNode(xPath) as XmlElement;
			if (connectionStringNode != null)
			{
				ret = connectionStringNode.GetAttribute("connectionString");
			}
			return ret;
		}

		private string GetCryptoKey(string webConfigPath)
		{
			string ret = null;
			var doc = new XmlDocument();
			doc.Load(webConfigPath);
			//crypto key
			string xPath = "configuration/appSettings/add[@key=\"SolidCP.CryptoKey\"]";
			XmlElement keyNode = doc.SelectSingleNode(xPath) as XmlElement;
			if (keyNode != null)
			{
				ret = keyNode.GetAttribute("value"); ;
			}
			return ret;
		}

		private bool IsEncryptionEnabled(string webConfigPath)
		{
			var doc = new XmlDocument();
			doc.Load(webConfigPath);
			//encryption enabled
			string xPath = "configuration/appSettings/add[@key=\"SolidCP.EncryptionEnabled\"]";
			XmlElement encryptionNode = doc.SelectSingleNode(xPath) as XmlElement;
			bool encryptionEnabled = false;
			if (encryptionNode != null)
			{
				bool.TryParse(encryptionNode.GetAttribute("value"), out encryptionEnabled);
			}
			return encryptionEnabled;
		}
		#region SolidCP providioning
		private void ConfigureStandaloneServer(string enterpriseServerUrl)
		{
			try
			{
				Log.WriteStart("Configuring SolidCP");
				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolvePortalAssembly);

				SetProgressText("Configuring SolidCP...");
				if (!ConnectToEnterpriseServer(enterpriseServerUrl, "serveradmin", Context.ServerAdminPassword))
				{
					Log.WriteError("Enterprise Server connection error");
					return;
				}
				SetProgressValue(10);

				//Add server
				int serverId = AddServer(Context.RemoteServerUrl, "My Server", Context.RemoteServerPassword);
				if (serverId < 0)
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serverId));
					return;
				}
				//Add IP address
				string portalIP = AppConfig.GetComponentSettingStringValue(Context.PortalComponentId, "WebSiteIP");
				int ipAddressId = AddIpAddress(portalIP, serverId);
				SetProgressValue(20);
				//Add OS service
				int osServiceId = AddOSService(serverId);
				SetProgressValue(30);
				//Add Web service
				int webServiceId = AddWebService(serverId, ipAddressId);
				SetProgressValue(40);
				//Add Sql service
				int sqlServiceId = AddSqlService(serverId);
				SetProgressValue(50);
				//Add Dns service
				int dnsServiceId = AddDnsService(serverId, ipAddressId);
				SetProgressValue(60);
				//Add virtual server
				int virtualServerId = AddVirtualServer("My Server Resources", serverId, new int[] { osServiceId, webServiceId, sqlServiceId, dnsServiceId });
				SetProgressValue(70);
				//Add user
				int userId = AddUser("admin", Context.ServerAdminPassword, "Server", "Administrator", "admin@myhosting.com");
				SetProgressValue(80);
				//Add plan
				int planId = -1;
				if (virtualServerId > 0)
				{
					planId = AddHostingPlan("My Server", virtualServerId);
				}
				SetProgressValue(90);
				//Add package
				if (userId > 0 && planId > 0)
				{
					int packageId = AddPackage("My Server", userId, planId);
				}
				SetProgressValue(95);
				ConfigureWebPolicy(1);
				SetProgressValue(100);
				Log.WriteEnd("Server configured");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Server configuration error", ex);
			}
		}

		private void SetProgressText(string p) // Do nothing.
		{
			//throw new NotImplementedException();
		}

		protected void SetProgressValue(int p)
		{
			m_Progress = p;
			Log.WriteInfo(string.Format("Current progress is {0}%.", m_Progress));
		}

		private void ConfigureWebPolicy(int userId)
		{
			try
			{
				Log.WriteStart("Configuring Web Policy");
				UserSettings settings = ES.Services.Users.GetUserSettings(userId, "WebPolicy");
				settings["AspNetInstalled"] = "2I";
				if (Context.IISVersion.Major == 6)
					settings["AspNetInstalled"] = "2";
				ES.Services.Users.UpdateUserSettings(settings);
				Log.WriteEnd("Configured Web Policy");
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Web policy configuration error", ex);
			}
		}

		private int AddOSService(int serverId)
		{
			try
			{
				Log.WriteStart("Adding OS service");
				ServiceInfo serviceInfo = new ServiceInfo();
				serviceInfo.ServerId = serverId;
				serviceInfo.ServiceName = "OS";
				serviceInfo.Comments = string.Empty;

				//check OS version
				OS.WindowsVersion version = OS.GetVersion();
				if (version == OS.WindowsVersion.WindowsServer2003)
				{
					serviceInfo.ProviderId = 1;
				}
				else if (version == OS.WindowsVersion.WindowsServer2008)
				{
					serviceInfo.ProviderId = 100;
				}
				int serviceId = ES.Services.Servers.AddService(serviceInfo);
				if (serviceId > 0)
				{
					InstallService(serviceId);
					Log.WriteEnd("Added OS service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("OS service configuration error", ex);
				return -1;
			}
		}

		private int AddWebService(int serverId, int ipAddressId)
		{
			try
			{
				Log.WriteStart("Adding Web service");
				ServiceInfo serviceInfo = new ServiceInfo();
				serviceInfo.ServerId = serverId;
				serviceInfo.ServiceName = "Web";
				serviceInfo.Comments = string.Empty;

				//check IIS version
				if (Context.IISVersion.Major >= 7)
				{
					serviceInfo.ProviderId = 101;
				}
				else if (Context.IISVersion.Major == 6)
				{
					serviceInfo.ProviderId = 2;
				}
				int serviceId = ES.Services.Servers.AddService(serviceInfo);
				if (serviceId > 0)
				{
					StringDictionary settings = GetServiceSettings(serviceId);
					if (settings != null)
					{
						// set ip address						
						if (ipAddressId > 0)
							settings["sharedip"] = ipAddressId.ToString();

						// settings for win2003 x64
						if (Context.IISVersion.Major == 6 &&
							Utils.IsWin64() && !Utils.IIS32Enabled())
						{
							settings["AspNet20Path"] = @"%SYSTEMROOT%\Microsoft.NET\Framework64\v2.0.50727\aspnet_isapi.dll";
							settings["Php4Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php.exe";
							settings["Php5Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
						}
						// settings for win2008 x64
						if (Context.IISVersion.Major > 6 &&
							Utils.IsWin64())
						{
							settings["Php4Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php.exe";
							settings["Php5Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
							settings["phppath"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
						}

						UpdateServiceSettings(serviceId, settings);
					}
					InstallService(serviceId);
					Log.WriteEnd("Added Web service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Web service configuration error", ex);
				return -1;
			}
		}

		private int AddSqlService(int serverId)
		{
			int serviceId = -1;
			try
			{
				Data.DbType dbtype;
				string nativeConnectionString;
				Data.DatabaseUtils.ParseConnectionString(Context.DbInstallConnectionString, out dbtype, out nativeConnectionString);
				
				Log.WriteStart("Adding Sql service");

				SqlServerItem item = ParseConnectionString(Context.DbInstallConnectionString);
				string serverName = item.Server.ToLower();
				if ((serverName.StartsWith("(local)") ||
					serverName.StartsWith("localhost") ||
					serverName.StartsWith(System.Environment.MachineName.ToLower())) &&
					(dbtype == Data.DbType.SqlServer 
					/* TODO || dbtype == Data.DbType.MySql || dbtype == Data.DbType.MariaDb */))
				{
					ServiceInfo serviceInfo = new ServiceInfo();
					serviceInfo.ServerId = serverId;
					if (dbtype == Data.DbType.SqlServer) serviceInfo.ServiceName = "SQL Server";
					else if (dbtype == Data.DbType.MySql) serviceInfo.ServiceName = "MySQL Server";
					else if (dbtype == Data.DbType.MariaDb) serviceInfo.ServiceName = "MariaDB Server";
					serviceInfo.Comments = string.Empty;

					string connectionString = Context.DbInstallConnectionString;
					//check SQL version
					if (Data.DatabaseUtils.CheckSqlConnection(connectionString))
					{
						if (dbtype == Data.DbType.SqlServer)
						{
							// check SQL server version
							string sqlVersion = Data.DatabaseUtils.GetSqlServerVersion(connectionString);
							if (sqlVersion.StartsWith("9."))
							{
								serviceInfo.ProviderId = 16;
							}
							else if (sqlVersion.StartsWith("10."))
							{
								serviceInfo.ProviderId = 202;
							}
							else if (sqlVersion.StartsWith("11."))
							{
								serviceInfo.ProviderId = 209;
							}
							else if (sqlVersion.StartsWith("12."))
							{
								serviceInfo.ProviderId = 1203;
							}
							else if (sqlVersion.StartsWith("13."))
							{
								serviceInfo.ProviderId = 1701;
							}
							else if (sqlVersion.StartsWith("14."))
							{
								serviceInfo.ProviderId = 1704;
							}
							else if (sqlVersion.StartsWith("15."))
							{
								serviceInfo.ProviderId = 1705;
							}
							else if (sqlVersion.StartsWith("16."))
							{
								serviceInfo.ProviderId = 1706;
							}
							serviceId = ES.Services.Servers.AddService(serviceInfo);
						}
					}
					else
						Log.WriteInfo("SQL Server connection error");
					//configure service
					if (serviceId > 0)
					{
						StringDictionary settings = GetServiceSettings(serviceId);
						if (settings != null)
						{
							settings["InternalAddress"] = item.Server;
							settings["ExternalAddress"] = string.Empty;
							settings["UseTrustedConnection"] = item.WindowsAuthentication.ToString();
							settings["SaLogin"] = item.User;
							settings["SaPassword"] = item.Password;
							UpdateServiceSettings(serviceId, settings);
						}
						InstallService(serviceId);
						Log.WriteEnd("Added Sql service");
					}
					else
					{
						Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
					}
				}
				else
				{
					Log.WriteError("Microsoft SQL Server was not found");
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Sql service configuration error", ex);
				return -1;
			}
		}

		private int AddDnsService(int serverId, int ipAddressId)
		{
			try
			{
				Log.WriteStart("Adding DNS service");
				int providerId = 7;
				int serviceId = -1;
				BoolResult result = ES.Services.Servers.IsInstalled(serverId, providerId);
				if (result.IsSuccess && result.Value)
				{
					ServiceInfo serviceInfo = new ServiceInfo();
					serviceInfo.ServerId = serverId;
					serviceInfo.ServiceName = "DNS";
					serviceInfo.Comments = string.Empty;
					serviceInfo.ProviderId = providerId;
					serviceId = ES.Services.Servers.AddService(serviceInfo);
				}
				else
				{
					Log.WriteInfo("Microsoft DNS was not found");
					return -1;
				}

				if (serviceId > 0)
				{
					StringDictionary settings = GetServiceSettings(serviceId);
					if (settings != null)
					{
						if (ipAddressId > 0)
							settings["listeningipaddresses"] = ipAddressId.ToString();
						UpdateServiceSettings(serviceId, settings);
					}
					InstallService(serviceId);
					Log.WriteEnd("Added DNS service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("DNS service configuration error", ex);
				return -1;
			}
		}

		private int AddServer(string url, string name, string password)
		{
			try
			{
				Log.WriteStart("Adding server");
				ServerInfo serverInfo = new ServerInfo()
				{
					ADAuthenticationType = null,
					ADPassword = null,
					ADEnabled = false,
					ADRootDomain = null,
					ADUsername = null,
					Comments = string.Empty,
					Password = password,
					ServerName = name,
					ServerUrl = url,
					VirtualServer = false
				};

				int serverId = ES.Services.Servers.AddServer(serverInfo, false);
				if (serverId > 0)
				{
					Log.WriteEnd("Added server");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serverId));
				}
				return serverId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Server configuration error", ex);
				return -1;
			}
		}

		private int AddIpAddress(string ip, int serverId)
		{
			try
			{
				Log.WriteStart("Adding IP address");
				IntResult res = ES.Services.Servers.AddIPAddress(IPAddressPool.General, serverId, ip, string.Empty, string.Empty, string.Empty, string.Empty, 0);
				if (res.IsSuccess && res.Value > 0)
				{
					Log.WriteEnd("Added IP address");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", res.Value));
				}
				return res.Value;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("IP address configuration error", ex);
				return -1;
			}
		}

		private int AddVirtualServer(string name, int serverId, int[] services)
		{
			Log.WriteStart("Adding virtual server");
			ServerInfo serverInfo = new ServerInfo()
			{
				Comments = string.Empty,
				ServerName = name,
				VirtualServer = true
			};

			int virtualServerId = ES.Services.Servers.AddServer(serverInfo, false);
			if (virtualServerId > 0)
			{
				List<int> allServices = new List<int>(services);
				List<int> validServices = new List<int>();
				foreach (int serviceId in allServices)
				{
					if (serviceId > 0)
						validServices.Add(serviceId);
				}
				ES.Services.Servers.AddVirtualServices(virtualServerId, validServices.ToArray());
				Log.WriteEnd("Added virtual server");
			}
			else
			{
				Log.WriteError(string.Format("Enterprise Server error: {0}", virtualServerId));
			}

			return virtualServerId;
		}

		private int AddUser(string loginName, string password, string firstName, string lastName, string email)
		{
			try
			{
				Log.WriteStart("Adding user account");
				UserInfo user = new UserInfo();
				user.UserId = 0;
				user.Role = UserRole.User;
				user.StatusId = 1;
				user.OwnerId = 1;
				user.IsDemo = false;
				user.IsPeer = false;
				user.HtmlMail = true;
				user.Username = loginName;
				user.FirstName = firstName;
				user.LastName = lastName;
				user.Email = email;

				int userId = ES.Services.Users.AddUser(user, false, password, new string[0]);
				if (userId > 0)
				{
					Log.WriteEnd("Added user account");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", userId));
				}
				return userId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("User configuration error", ex);
				return -1;
			}
		}

		private int AddHostingPlan(string name, int serverId)
		{
			try
			{
				Log.WriteStart("Adding hosting plan");
				// gather form info
				HostingPlanInfo plan = new HostingPlanInfo();
				plan.UserId = 1;
				plan.PlanId = 0;
				plan.IsAddon = false;
				plan.PlanName = name;
				plan.PlanDescription = "";
				plan.Available = true; // always available

				plan.SetupPrice = 0;
				plan.RecurringPrice = 0;
				plan.RecurrenceLength = 1;
				plan.RecurrenceUnit = 2; // month

				plan.PackageId = 0;
				plan.ServerId = serverId;
				List<HostingPlanGroupInfo> groups = new List<HostingPlanGroupInfo>();
				List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();

				DataSet ds = ES.Services.Packages.GetHostingPlanQuotas(-1, 0, serverId);

				foreach (DataRow groupRow in ds.Tables[0].Rows)
				{
					bool enabled = (bool)groupRow["ParentEnabled"];
					if (!enabled)
						continue; // disabled group

					int groupId = (int)groupRow["GroupId"]; ;

					HostingPlanGroupInfo group = new HostingPlanGroupInfo();
					group.GroupId = groupId;
					group.Enabled = true;
					group.CalculateDiskSpace = (bool)groupRow["CalculateDiskSpace"];
					group.CalculateBandwidth = (bool)groupRow["CalculateBandwidth"];
					groups.Add(group);

					DataView dvQuotas = new DataView(ds.Tables[1], "GroupID=" + group.GroupId.ToString(), "", DataViewRowState.CurrentRows);
					List<HostingPlanQuotaInfo> groupQuotas = GetGroupQuotas(groupId, dvQuotas);
					quotas.AddRange(groupQuotas);

				}

				plan.Groups = groups.ToArray();
				plan.Quotas = quotas.ToArray();

				int planId = ES.Services.Packages.AddHostingPlan(plan);
				if (planId > 0)
				{
					Log.WriteEnd("Added hosting plan");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", planId));
				}
				return planId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Hosting plan configuration error", ex);
				return -1;
			}
		}

		private List<HostingPlanQuotaInfo> GetGroupQuotas(int groupId, DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			//OS quotas
			if (groupId == 1)
				quotas = GetOSQuotas(dvQuotas);
			//Web quotas
			else if (groupId == 2)
				quotas = GetWebQuotas(dvQuotas);
			else
			{
				foreach (DataRowView quotaRow in dvQuotas)
				{
					int quotaTypeId = (int)quotaRow["QuotaTypeID"];
					HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
					quota.QuotaId = (int)quotaRow["QuotaID"];
					quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
					quotas.Add(quota);
				}
			}
			return quotas;
		}

		private List<HostingPlanQuotaInfo> GetOSQuotas(DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			foreach (DataRowView quotaRow in dvQuotas)
			{
				int quotaTypeId = (int)quotaRow["QuotaTypeID"];
				string quotaName = (string)quotaRow["QuotaName"];
				HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
				quota.QuotaId = (int)quotaRow["QuotaID"];
				quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
				if (quotaName == "OS.AppInstaller" ||
					quotaName == "OS.ExtraApplications")
					quota.QuotaValue = 0;

				quotas.Add(quota);
			}
			return quotas;
		}

		private List<HostingPlanQuotaInfo> GetWebQuotas(DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			foreach (DataRowView quotaRow in dvQuotas)
			{
				int quotaTypeId = (int)quotaRow["QuotaTypeID"];
				string quotaName = (string)quotaRow["QuotaName"];
				HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
				quota.QuotaId = (int)quotaRow["QuotaID"];
				quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
				if (quotaName == "Web.Asp" ||
					quotaName == "Web.AspNet11" ||
					quotaName == "Web.Php4" ||
					quotaName == "Web.Perl" ||
					quotaName == "Web.CgiBin" ||
					quotaName == "Web.SecuredFolders" ||
					quotaName == "Web.SharedSSL" ||
					quotaName == "Web.Python" ||
					quotaName == "Web.AppPools" ||
					quotaName == "Web.IPAddresses" ||
					quotaName == "Web.ColdFusion" ||
					quotaName == "Web.CFVirtualDirectories" ||
					quotaName == "Web.RemoteManagement")
					quota.QuotaValue = 0;

				quotas.Add(quota);
			}
			return quotas;
		}

		private int AddPackage(string name, int userId, int planId)
		{
			try
			{
				Log.WriteStart("Adding hosting space");
				// gather form info
				PackageResult res = ES.Services.Packages.AddPackageWithResources(userId, planId,
					name, 1, false, false, string.Empty, false, false, false, null, false, string.Empty);
				if (res.Result > 0)
					Log.WriteEnd("Added hosting space");
				else
					Log.WriteError(string.Format("Enterprise Server error: {0}", planId));
				return res.Result;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Hosting space configuration error", ex);
				return -1;
			}
		}

		private bool ConnectToEnterpriseServer(string url, string username, string password)
		{
			return ES.Connect(url, username, password);
		}

		private void UpdateServiceSettings(int serviceId, StringDictionary settings)
		{
			if (serviceId < 0 || settings == null)
				return;

			try
			{
				// save settings
				int result = ES.Services.Servers.UpdateServiceSettings(serviceId,
					ConvertDictionaryToArray(settings));

				if (result < 0)
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", result));
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Update service settings error", ex);
			}
		}

		private void InstallService(int serviceId)
		{
			if (serviceId < 0)
				return;

			string[] installResults = null;

			try
			{
				installResults = ES.Services.Servers.InstallService(serviceId);
				foreach (string result in installResults)
				{
					Log.WriteInfo(result);
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Install service error", ex);
			}
		}

		private StringDictionary GetServiceSettings(int serviceId)
		{
			StringDictionary ret = null;
			try
			{
				if (serviceId > 0)
				{
					// load service properties and bind them
					string[] settings = ES.Services.Servers.GetServiceSettings(serviceId);
					ret = ConvertArrayToDictionary(settings);
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Get service settings error", ex);
			}
			return ret;
		}

		private string[] ConvertDictionaryToArray(StringDictionary settings)
		{
			List<string> r = new List<string>();
			foreach (string key in settings.Keys)
				r.Add(key + "=" + settings[key]);
			return r.ToArray();
		}

		private StringDictionary ConvertArrayToDictionary(string[] settings)
		{
			StringDictionary r = new StringDictionary();
			foreach (string setting in settings)
			{
				int idx = setting.IndexOf('=');
				r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
			}
			return r;
		}

		private SqlServerItem ParseConnectionString(string connectionString)
		{
			SqlServerItem ret = new SqlServerItem();

			ret.WindowsAuthentication = false;
			string[] pairs = connectionString.Split(';');
			foreach (string pair in pairs)
			{
				string[] keyValue = pair.Split('=');
				if (keyValue.Length == 2)
				{
					string key = keyValue[0].Trim().ToLower();
					string value = keyValue[1];
					switch (key)
					{
						case "server":
							ret.Server = value;
							break;
						case "database":
							ret.Database = value;
							break;
						case "integrated security":
							if (value.Trim().ToLower() == "sspi")
								ret.WindowsAuthentication = true;
							break;
						case "user":
						case "user id":
							ret.User = value;
							break;
						case "password":
							ret.Password = value;
							break;
						case "port":
							int port;
							if (!int.TryParse(value, out port)) port = 3306;
							ret.Port = port;
							break;
						case "dbtype":
							SolidCP.EnterpriseServer.Data.DbType dbtype;
							if (!Enum.TryParse(value, out dbtype)) dbtype = Data.DbType.Unknown;
							ret.DatabaseType = dbtype;
							break;
					}
				}
			}
			return ret;
		}

		private Assembly ResolvePortalAssembly(object sender, ResolveEventArgs args)
		{
			Assembly ret = null;

			string portalPath = AppConfig.GetComponentSettingStringValue(Context.PortalComponentId, "InstallFolder");
			string binPath = Path.Combine(portalPath, "bin");
			string path = Path.Combine(binPath, args.Name.Split(',')[0] + ".dll");
			Log.WriteInfo("Assembly to resolve: " + path);
			if (File.Exists(path))
			{
				ret = Assembly.LoadFrom(path);
				Log.WriteInfo("Assembly resolved: " + path);
			}
			return ret;
		}
		#endregion

		private void CreateSCPServerLogin()
		{
			try
			{
				Log.WriteStart("Creating SolidCP login");
				string query = string.Empty;

				string connectionString = AppConfig.GetComponentSettingStringValue(
					Context.EnterpriseServerComponentId,
					"InstallConnectionString");

				SqlServerItem item = ParseConnectionString(connectionString);
				string serverName = item.Server.ToLower();
				if (serverName.StartsWith("(local)") ||
					serverName.StartsWith("localhost") ||
					serverName.StartsWith(System.Environment.MachineName.ToLower()))
				{

					string domain = AppConfig.GetComponentSettingStringValue(
						Context.ServerComponentId,
						"Domain");
					if (string.IsNullOrEmpty(domain))
						domain = System.Environment.MachineName;

					string userAccount = AppConfig.GetComponentSettingStringValue(
							Context.ServerComponentId,
							"UserAccount");

					string loginName = string.Format("{0}\\{1}", domain, userAccount);

					if (!Data.DatabaseUtils.LoginExists(connectionString, loginName))
					{
						query = string.Format("CREATE LOGIN [{0}] FROM WINDOWS WITH DEFAULT_DATABASE=[master]", loginName);
						Data.DatabaseUtils.ExecuteQuery(connectionString, query);
					}
					query = string.Format("EXEC master..sp_addsrvrolemember @loginame = N'{0}', @rolename = N'sysadmin'", loginName);
					Data.DatabaseUtils.ExecuteQuery(connectionString, query);

					AppConfig.SetComponentSettingStringValue(Context.EnterpriseServerComponentId, "DatabaseLogin", loginName);

					Log.WriteEnd("Created SolidCP login");
				}
				else
				{
					Log.WriteInfo("Microsoft SQL Server is not located on the local server.");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("SQL error", ex);
			}
		}

		private void UpdateServerAdminPassword()
		{
			try
			{
				if (!Context.UpdateServerAdminPassword)
					return;

				Log.WriteStart("Updating serveradmin password");

				string path = Path.Combine(Context.InstallationFolder, Context.ConfigurationFile);
				string password = Context.ServerAdminPassword;
				string database = Context.Database;

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				string connectionString = GetConnectionString(path);
				if (string.IsNullOrEmpty(connectionString))
				{
					Log.WriteError("Connection string setting not found");
					return;
				}

				string cryptoKey = GetCryptoKey(path);
				if (string.IsNullOrEmpty(cryptoKey))
				{
					Log.WriteError("CryptoKey setting not found");
					return;
				}

				bool encryptionEnabled = IsEncryptionEnabled(path);
				//encrypt password
				if (encryptionEnabled)
				{
					password = Utils.Encrypt(cryptoKey, password);
				}

				Data.DatabaseUtils.SetServerAdminPassword(connectionString, database, password);

				Log.WriteEnd("Updated serveradmin password");
				InstallLog.AppendLine("- Updated password for the serveradmin account");

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update error", ex);
				throw;
			}
		}

		private void RegisterWindowsService()
		{
			try
			{
				string componentId = Context.ComponentId;
				string path = Context.ServiceFile; // FullFileName.
				string service = Context.ServiceName;

				Log.WriteStart(string.Format("Registering \"{0}\" windows service", service));

				if (!File.Exists(path))
				{
					Log.WriteError(string.Format("File {0} not found", path), null);
					return;
				}
				if (System.ServiceProcess.ServiceController.GetServices().Any(s => s.DisplayName.Equals(service, StringComparison.CurrentCultureIgnoreCase)))
				{
					var Msg = string.Format("Service \"{0}\" already installed.", service);
					Log.WriteEnd(Msg);
					InstallLog.AppendLine(Msg);
				}
				else
				{
					try
					{
						string domain = Context.UserDomain;
						if (string.IsNullOrEmpty(domain))
							domain = ".";

						string arguments = string.Empty;
						if (Context.UseUserCredentials)
							arguments = string.Format("/i /LogFile=\"\" /user={0}\\{1} /password={2}", domain, Context.UserAccount, Context.UserPassword);
						else
							arguments = "/i /LogFile= ''";

						ManagedInstallerClass.InstallHelper(new[] { arguments, path });
						//add rollback action
						RollBack.RegisterWindowsService(path, service);
						var Msg = string.Format("Registered \"{0}\" Windows service ", service);
						//update log
						Log.WriteEnd(Msg);
						//update install log
						InstallLog.AppendLine(Msg);

						// update config setings
						AppConfig.EnsureComponentConfig(componentId);
						AppConfig.SetComponentSettingStringValue(componentId, "ServiceName", service);
						AppConfig.SetComponentSettingStringValue(componentId, "ServiceFile", path);
						AppConfig.SaveConfiguration();
					}
					catch
					{
						Log.WriteError(string.Format("Unable to register \"{0}\" Windows service.", service), null);
						InstallLog.AppendLine(string.Format("- Failed to register \"{0}\" windows service ", service));
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Windows service error", ex);
			}
		}

		private void StartWindowsService()
		{
			try
			{
				string service = Context.ServiceName;
				Log.WriteStart(string.Format("Starting \"{0}\" Windows service", service));
				Utils.StartService(service);
				//update log
				Log.WriteEnd("Started Windows service");
				InstallLog.AppendLine(string.Format("- Started \"{0}\" Windows service ", service));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Windows service start error", ex);
			}
		}

		private void StopWindowsService()
		{
			try
			{
				string service = Context.ServiceName;
				Log.WriteStart(string.Format("Stopping \"{0}\" Windows service", service));
				Utils.StopService(service);
				//update log
				Log.WriteEnd("Stopped Windows service");
				InstallLog.AppendLine(string.Format("- Stopped \"{0}\" Windows service ", service));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Windows service stop error", ex);
			}
		}

		private void StopApplicationPool()
		{
			try
			{
				string componentId = Context.ComponentId;
				string appPool = AppConfig.GetComponentSettingStringValue(componentId, "ApplicationPool");
				if (string.IsNullOrEmpty(appPool))
					return;

				Version iisVersion = Context.IISVersion;
				bool iis7 = (iisVersion.Major >= 7);

				Log.WriteStart("Stopping IIS Application Pool");
				Log.WriteInfo(string.Format("Stopping \"{0}\"", appPool));
				if (iis7)
					WebUtils.StopIIS7ApplicationPool(appPool);
				else
					WebUtils.StopApplicationPool(appPool);

				Log.WriteEnd("Stopped IIS Application Pool");
				// rollback
				if (iis7)
					RollBack.RegisterStopIIS7ApplicationPool(appPool);
				else
					RollBack.RegisterStopApplicationPool(appPool);

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Application Pool stop error", ex);
			}
		}

		private void StartApplicationPool()
		{
			try
			{
				Log.WriteStart("Starting IIS Application Pool");
				string componentId = Context.ComponentId;
				string appPool = AppConfig.GetComponentSettingStringValue(componentId, "ApplicationPool");
				if (string.IsNullOrEmpty(appPool))
				{
					Log.WriteInfo("ApplicatonPool name is empty string value.");
					return;
				}

				Version iisVersion = Context.IISVersion;
				bool iis7 = (iisVersion.Major >= 7);

				Log.WriteInfo(string.Format("Starting \"{0}\"", appPool));
				if (iis7)
					WebUtils.StartIIS7ApplicationPool(appPool);
				else
					WebUtils.StartApplicationPool(appPool);

				Log.WriteEnd("Started IIS Application Pool");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Application Pool start error", ex);
			}
		}

		private void UpdateServers()
		{
			try
			{
				if (Context.SQLServers == null)
					return;

				string path = Path.Combine(Context.InstallationFolder, "config.xml");

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Updating config.xml file");
				var doc = new XmlDocument();
				doc.Load(path);

				XmlNode serversNode = doc.SelectSingleNode("//myLittleAdmin/sqlservers");
				if (serversNode == null)
				{
					Log.WriteInfo("sql server setting not found");
					return;
				}

				if (serversNode.HasChildNodes)
					serversNode.RemoveAll();

				foreach (ServerItem item in Context.SQLServers)
				{
					XmlElement serverNode = doc.CreateElement("sqlserver");
					serverNode.SetAttribute("address", item.Server);
					serverNode.SetAttribute("name", item.Name);
					serversNode.AppendChild(serverNode);
				}
				doc.Save(path);
				Log.WriteEnd("Updated config.xml file");
				InstallLog.AppendLine("- Updated config.xml file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update config.xml error", ex);
				throw;
			}
		}

		private void CreateShortcuts()
		{
			try
			{
				string ip = Context.WebSiteIP;
				string domain = Context.WebSiteDomain;
				string port = Context.WebSitePort;

				string[] urls = GetApplicationUrls(ip, domain, port, null);
				string url = null;
				if (urls.Length > 0)
					url = "http://" + urls[0];
				else
				{
					Log.WriteInfo("Application url not found");
					return;
				}

				Log.WriteStart("Creating menu shortcut");
				string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
				string fileName = "Login to SolidCP.url";
				string path = Path.Combine(programs, "SolidCP Software");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				path = Path.Combine(path, fileName);
				using (StreamWriter sw = File.CreateText(path))
				{
					WriteShortcutData(url, sw);
				}
				Log.WriteEnd("Created menu shortcut");

				Log.WriteStart("Creating desktop shortcut");
				string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				path = Path.Combine(desktop, fileName);
				using (StreamWriter sw = File.CreateText(path))
				{
					WriteShortcutData(url, sw);
				}
				Log.WriteEnd("Created desktop shortcut");

				InstallLog.AppendLine("- Created application shortcuts");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Create shortcut error", ex);
				//throw;
			}
		}

		private static void WriteShortcutData(string url, StreamWriter sw)
		{
			sw.WriteLine("[InternetShortcut]");
			sw.WriteLine("URL=" + url);
			string iconFile = Path.Combine(Environment.SystemDirectory, "url.dll");
			sw.WriteLine("IconFile=" + iconFile);
			sw.WriteLine("IconIndex=0");
			sw.WriteLine("HotKey=0");
			Log.WriteInfo(string.Format("Shortcut url: {0}", url));
		}

		/// <summary>
		/// Tighten WSE security for Server
		/// </summary>
		private void UpdateWseSecuritySettings()
		{
			try
			{
				string webConfigPath = Path.Combine(Context.InstallationFolder, "web.config");
				Log.WriteStart("Web.config file is being updated");
				// Ensure the web.config exists
				if (!File.Exists(webConfigPath))
				{
					Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
					return;
				}
				// Load web.config
				var doc = new XmlDocument();
				doc.Load(webConfigPath);

				// Tighten WSE security on local machine
				XmlElement httpPostLocalhost = doc.SelectSingleNode("configuration/system.web/webServices/protocols/remove[@name='HttpPostLocalhost']") as XmlElement;
				// ensure node is found
				if (httpPostLocalhost == null)
				{
					var protocolsNode = doc.SelectSingleNode("configuration/system.web/webServices/protocols");
					//
					if (protocolsNode != null)
					{
						protocolsNode.InnerXml += "<remove name=\"HttpPostLocalhost\"/>";
					}
				}

				// save changes have been made
				doc.Save(webConfigPath);
				//
				Log.WriteEnd("Web.config has been updated");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Could not update web.config file", ex);
				throw;
			}
		}

		private void CopyWebConfig()
		{
			if (!OSInfo.IsWindows) return;

			try
			{
				Log.WriteStart("Copying web.config");
				string configPath = Path.Combine(Context.InstallationFolder, "web.config");
				string config6Path = Path.Combine(Context.InstallationFolder, "web6.config");

				bool iis6 = (Context.IISVersion.Major == 6);
				if (!File.Exists(config6Path))
				{
					Log.WriteInfo(string.Format("File {0} not found", config6Path));
					return;
				}

				if (iis6)
				{
					if (!File.Exists(configPath))
					{
						Log.WriteInfo(string.Format("File {0} not found", configPath));
						return;
					}

					FileUtils.DeleteFile(configPath);
					File.Move(config6Path, configPath);
				}
				else
				{
					FileUtils.DeleteFile(config6Path);
				}
				Log.WriteEnd("Copied web.config");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Copy web.config error", ex);
				throw;
			}
		}

		private void LoadPortal153Settings()
		{
			try
			{
				string path = Path.Combine(Context.InstallationFolder, "web.config");

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Loading portal settings");
				var doc = new XmlDocument();
				doc.Load(path);

				string xPath = "configuration/connectionStrings/add[@name=\"SiteSqlServer\"]";
				XmlElement connectionNode = doc.SelectSingleNode(xPath) as XmlElement;
				if (connectionNode != null)
				{
					string connectionString = connectionNode.GetAttribute("connectionString");
					Context.ConnectionString = connectionString;
					Log.WriteInfo(string.Format("Connection string loaded: {0}", connectionString));
				}
				else
				{
					Context.ConnectionString = null;
					Log.WriteError("Connection string not found!", null);
				}

				Log.WriteEnd("Loaded portal settings");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Loading portal settings error", ex);
				throw;
			}
		}

		private void UpdateWebConfigNamespaces()
		{
			if (!OSInfo.IsWindows) return;

			try
			{
				// find all .config files in the installation directory
				string[] configFiles = Directory.GetFiles(Context.InstallationFolder,
					"*.config", SearchOption.TopDirectoryOnly);

				if (configFiles != null && configFiles.Length > 0)
				{
					foreach (string path in configFiles)
					{
						try
						{
							Log.WriteStart(String.Format("Updating '{0}' file", path));

							// load configuration file in memory
							string content = File.ReadAllText(path);

							// replace DotNetPark. to empty strings
							content = Regex.Replace(content, "dotnetpark\\.", "", RegexOptions.IgnoreCase);

							// save updated config
							File.WriteAllText(path, content);

							Log.WriteEnd(String.Format("Updated '{0}' file", path));
							InstallLog.AppendLine(String.Format("- Updated {0} file", path));
						}
						catch (Exception ex)
						{
							if (Utils.IsThreadAbortException(ex))
								return;
							Log.WriteError(String.Format("Error updating '{0}' file", path), ex);
							throw;
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Error listing *.config files", ex);
				throw;
			}
		}

		private void UpdateEnterpriseServerUrl()
		{
			try
			{
				string url = Context.EnterpriseServerURL;
				string installFolder = Context.InstallationFolder;
				string file = @"App_Data\SiteSettings.config";

				string path = Path.Combine(installFolder, file);

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Updating site settings");
				var doc = new XmlDocument();
				doc.Load(path);

				XmlElement urlNode = doc.SelectSingleNode("SiteSettings/EnterpriseServer") as XmlElement;
				if (urlNode == null)
				{
					Log.WriteInfo("EnterpriseServer setting not found");
					return;
				}

				urlNode.InnerText = url;
				doc.Save(path);
				Log.WriteEnd("Updated site settings");
				InstallLog.AppendLine("- Updated site settings");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Site settigs error", ex);
				throw;
			}
		}

		private void DeleteFiles(string file)
		{
			try
			{
				string component = Context.ComponentFullName;
				string installerFolder = Context.InstallerFolder;
				string installFolder = Context.InstallationFolder;

				//file with list of files to delete
				string path = Path.Combine(installerFolder, file);

				if (!File.Exists(path))
					return;

				Log.WriteStart("Deleting files");
				long count = 0;

				using (StreamReader reader = new StreamReader(path))
				{
					string fileName;
					string filePath;
					// Read and display lines from the file until the end of the file is reached.
					while ((fileName = reader.ReadLine()) != null)
					{
						if (!string.IsNullOrEmpty(fileName))
						{
							filePath = Path.Combine(installFolder, fileName);
							if (Directory.Exists(filePath))
							{
								FileUtils.DeleteDirectory(filePath);
								count++;
							}
							else if (File.Exists(filePath))
							{
								FileUtils.DeleteFile(filePath);
								count++;
							}
						}
					}
				}
				Log.WriteEnd(string.Format("Deleted {0} files", count));
				InstallLog.AppendLine(string.Format("- Deleted {0} files", count));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("File delete error", ex);
				throw;
			}
		}

		private void Backup()
		{
			try
			{
				string componentId = Context.ComponentId;
				string componentName = Context.ComponentFullName;
				string version = Context.Version;
				List<InstallAction> actions = GenerateBackupActions(componentId);

				Log.WriteStart("Creating backup directory");
				string backupDirectory = Path.Combine(Context.BaseDirectory, "Backup");
				if (!Directory.Exists(backupDirectory))
				{
					Directory.CreateDirectory(backupDirectory);
				}

				string destinationDirectory = Path.Combine(backupDirectory,
					string.Format("{0}{1}{2} {3}",
					DateTime.Now.ToString("yyyy-MM-dd"),
					Path.DirectorySeparatorChar,
					componentName,
					version));

				if (Directory.Exists(destinationDirectory))
				{
					//clear existing dir
					FileUtils.DeleteDirectory(destinationDirectory);
				}

				Directory.CreateDirectory(destinationDirectory);
				Log.WriteEnd("Created backup directory");

				for (int i = 0; i < actions.Count; i++)
				{
					InstallAction action = actions[i];
					SetProgressText(action.Description);
					SetProgressValue(i * 100 / actions.Count);

					switch (action.ActionType)
					{
						case ActionTypes.BackupDirectory:
							BackupDirectory(action.Path, destinationDirectory);
							break;
						case ActionTypes.BackupDatabase:
							BackupDatabase(action.ConnectionString, action.Name);
							break;
						case ActionTypes.BackupConfig:
							BackupConfig(action.Path, destinationDirectory, action.SetupVariables != null ? action.SetupVariables.FileNameMap : null);
							break;
					}
				}
				//this.progressBar.Value = 100;
				this.SetProgressValue(100);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}

		private void BackupConfig(string path, string backupDirectory, IDictionary<string, string> NameMap = null)
		{
			try
			{
				Log.WriteStart("Backing up system configuration");

				string destination = Path.Combine(backupDirectory, "Config");
				if (!Directory.Exists(destination))
				{
					Log.WriteStart(string.Format("Creating directory {0}", destination));
					Directory.CreateDirectory(destination);
					Log.WriteEnd("Created directory");
				}

				string[] files = Directory.GetFiles(path, "*.config", SearchOption.TopDirectoryOnly);
				foreach (string file in files)
				{
					FileUtils.CopyFileToFolder(file, destination, GetMappedFileName(file, NameMap));
				}
				Log.WriteEnd("Backed up system configuration");
				InstallLog.AppendLine("- Backed up system configuration");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}

		private string GetMappedFileName(string FullFileName, IDictionary<string, string> Map)
		{
			if (Map == null)
				return "";
			string Key = new FileInfo(FullFileName).Name;
			if (Map.Keys.Contains(Key))
				return Map[Key];
			else
				return "";
		}

		private void BackupDatabase(string connectionString, string database)
		{
			try
			{
				Log.WriteStart(string.Format("Backing up database \"{0}\"", database));
				string bakFile;
				string position;
				Data.DatabaseUtils.BackupDatabase(connectionString, database, out bakFile, out position);
				Log.WriteEnd("Backed up database");
				InstallLog.AppendLine(string.Format("- Backed up {0} database", database));
				RollBack.RegisterDatabaseBackupAction(connectionString, database, bakFile, position);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}

		private void BackupDirectory(string source, string backupDirectory)
		{
			try
			{
				string componentName = Context.ComponentFullName;
				string destination = Path.Combine(backupDirectory, "App");

				if (Directory.Exists(destination))
				{
					try
					{
						Log.WriteStart(string.Format("Deleting directory {0}", destination));
						FileUtils.DeleteDirectory(destination);
						Log.WriteEnd("Deleted directory");
					}
					catch (Exception ex)
					{
						Log.WriteError("Backup error", ex);
					}
				}

				if (!Directory.Exists(destination))
				{
					Log.WriteStart(string.Format("Creating directory {0}", destination));
					Directory.CreateDirectory(destination);
					Log.WriteEnd("Created directory");
				}
				string zipFile = Path.Combine(destination, "app.zip");

				Log.WriteStart("Backing up files");
				Log.WriteInfo(string.Format("Zipping files from \"{0}\" to \"{1}\"", source, zipFile));
				//showing process
				ZipIndicator process = new ZipIndicator(GetProgressObject(), source, zipFile);
				//CopyProcess process = new CopyProcess(progressBar, source, destination);
				process.Start();
				Log.WriteEnd("Backed up files");
				InstallLog.AppendLine(string.Format("- Backed up {0} files", componentName));
				RollBack.RegisterDirectoryBackupAction(source, zipFile);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}
		protected virtual List<InstallAction> GenerateBackupActions(string componentId)
		{
			List<InstallAction> list = new List<InstallAction>();
			InstallAction action = null;
			//database
			string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
			if (!String.IsNullOrEmpty(connectionString))
			{
				string database = AppConfig.GetComponentSettingStringValue(componentId, "Database");
				action = new InstallAction(ActionTypes.BackupDatabase);
				action.ConnectionString = connectionString;
				action.Name = database;
				action.Description = string.Format("Backing up database {0}...", database);
				list.Add(action);
			}
			//directory
			string path = AppConfig.GetComponentSettingStringValue(componentId, "InstallFolder");
			if (!string.IsNullOrEmpty(path))
			{
				action = new InstallAction(ActionTypes.BackupDirectory);
				action.Path = path;
				action.Description = string.Format("Backing up directory {0}...", path);
				list.Add(action);
			}
			//config
			action = new InstallAction(ActionTypes.BackupConfig);
			action.Path = Context.BaseDirectory;
			action.Description = "Backing up configuration settings...";
			if (!string.IsNullOrWhiteSpace(Context.SpecialBaseDirectory))
			{
				action.Path = Context.SpecialBaseDirectory;
				action.SetupVariables = Context;
			}
			list.Add(action);
			return list;
		}
		private bool ConfigureLetsEncrypt(bool isUnattended)
		{
			string ip = Context.WebSiteIP;
			string siteId = Context.WebSiteId;
			string domain = Context.WebSiteDomain;
			string email = Context.LetsEncryptEmail;
			var componentId = Context.ComponentId;
			bool updateWCF = componentId == "enterpriseserver" || componentId == "server";
			var iisVersion = Context.IISVersion;
			var iis7 = (iisVersion.Major >= 7);
			bool success = true;

			if ((iis7 || !OSInfo.IsWindows) && Utils.IsHttps(ip, domain) && !string.IsNullOrEmpty(email))
			{
				if (OSInfo.IsWindows)
				{
					success = WebUtils.LEInstallCertificate(siteId, domain, email, updateWCF);
				}
			}
			if (!success)
			{
				Log.WriteError("Error creating Let's Encrypt certificate. Check the error log for details.");
				if (!isUnattended) MessageBox.Show("Error creating Let's Encrypt certificate. Check the error log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return success;
		}
		private void UpdateWebSiteBindings()
		{
			if (!OSInfo.IsWindows)
			{
				UpdateWebSiteBindingsUnix();
				return;
			}

			string componentId = Context.ComponentId;
			string component = Context.ComponentFullName;
			string siteId = Context.WebSiteId;
			string ip = Context.WebSiteIP;
			string port = Context.WebSitePort;
			string domain = Context.WebSiteDomain;
			bool update = Context.UpdateWebSite;
			Version iisVersion = Context.IISVersion;
			bool iis7 = (iisVersion.Major >= 7);

			if (!update)
				return;

			//updating web site
			try
			{
				Log.WriteStart("Updating web site");
				Log.WriteInfo(string.Format("Updating web site \"{0}\" ( IP: {1}, Port: {2}, Domain: {3} )", siteId, ip, port, domain));

				//check for existing site
				var oldSiteId = iis7 ? WebUtils.GetIIS7SiteIdByBinding(ip, port, domain) : WebUtils.GetSiteIdByBinding(ip, port, domain);
				// We found out that other web site has this combination of {IP:Port:Host Header} already assigned
				if (oldSiteId != null && !oldSiteId.Equals(Context.WebSiteId))
				{
					// get site name
					string oldSiteName = iis7 ? oldSiteId : WebUtils.GetSite(oldSiteId).Name;
					throw new Exception(
						String.Format("'{0}' web site already has server binding ( IP: {1}, Port: {2}, Domain: {3} )",
						oldSiteName, ip, port, domain));
				}

				// Assign the binding only if is not defined
				if (String.IsNullOrEmpty(oldSiteId))
				{
					ServerBinding newBinding = new ServerBinding(ip, port, domain, null, componentId);
					if (iis7)
					{
						var bindings = new ServerBinding[] { newBinding };
						WebUtils.UpdateIIS7SiteBindings(siteId, bindings);
					}
					else
					{
						WebUtils.UpdateSiteBindings(siteId, new ServerBinding[] { newBinding });
					}
				}

				// update config setings
				AppConfig.SetComponentSettingStringValue(componentId, "WebSiteIP", ip);
				AppConfig.SetComponentSettingStringValue(componentId, "WebSitePort", port);
				AppConfig.SetComponentSettingStringValue(componentId, "WebSiteDomain", domain);

				//update log
				Log.WriteEnd("Updated web site");

				//update install log
				InstallLog.AppendLine("- Updated web site");
				InstallLog.AppendLine("  You can access the application by the following URLs:");
				string[] urls = GetApplicationUrls(ip, domain, port, null);
				//
				foreach (string url in urls)
				{
					InstallLog.AppendLine("  http://" + url);
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Update web site error", ex);
				throw;
			}

			//opening windows firewall ports
			try
			{
				Utils.OpenFirewallPort(component, port, Context.IISVersion);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Open windows firewall port error", ex);
			}
		}

		private void UpdateWebSiteBindingsUnix()
		{
			try
			{
				var ip = Context.WebSiteIP;
				var port = Context.WebSitePort;
				var domain = Context.WebSiteDomain;
				var urls = Utils.GetApplicationUrls(ip, domain, port, null)
					.Select(url => Utils.IsHttps(ip, domain) ? "https://" + url : "http://" + url);

				Log.WriteStart("Updating configuration file (web site bindings)");
				var installer = UniversalInstaller.Installer.Current;
				installer.InstallWebRootPath = Context.InstallationFolder;
				installer.ReadServerConfiguration();
				installer.ServerSettings.Urls = string.Join(";", urls.ToArray());
				installer.ConfigureServer();
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated password in the configuration file");

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Update web site bindings error", ex);
			}
		}

			private void CreateDatabaseUser()
		{
			try
			{
				string connectionString = Context.DbInstallConnectionString;
				string database = Context.Database;
				string component = Context.ComponentFullName;
				//user name should be the same as database
				string userName = Context.Database;
				string password = Utils.GetRandomString(20);

				CreateDbUser(connectionString, database, userName, password);
				UpdateWebConfigConnection(userName, password);

				InstallLog.AppendLine(string.Format("- Created database user \"{0}\"", userName));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create db user error", ex);
				throw;
			}
		}

		private void CreateDbUser(string connectionString, string database, string userName, string password)
		{
			Log.WriteStart(string.Format("Creating database user {0}", userName));

			if (Data.DatabaseUtils.UserExists(connectionString, userName))
				throw new Exception(string.Format("Database user {0} already exists", userName));

			bool userCreated = Data.DatabaseUtils.CreateUser(connectionString, userName, password, database);

			// save user details
			string componentId = Context.ComponentId;
			AppConfig.SetComponentSettingStringValue(componentId, "DatabaseUser", userName);
			AppConfig.SetComponentSettingBooleanValue(componentId, "NewDatabaseUser", userCreated);

			// roll-back support
			if (userCreated)
				RollBack.RegisterDatabaseUserAction(connectionString, userName);

			Log.WriteEnd("Created database user");
		}

		private void UpdateWebConfigConnection(string userName, string password)
		{
			Log.WriteStart("Updating web.config file (connection string)");

			string file = Path.Combine(Context.InstallationFolder, "web.config");

			string content = string.Empty;
			// load file
			using (StreamReader reader = new StreamReader(file))
			{
				content = reader.ReadToEnd();
			}

			string connectionString = string.Format("server={0};database={1};uid={2};pwd={3};",
					Context.DatabaseServer, Context.Database, userName, password);

			// expand variables
			content = Utils.ReplaceScriptVariable(content, "installer.connectionstring", connectionString);

			// save file
			using (StreamWriter writer = new StreamWriter(file))
			{
				writer.Write(content);
			}
			Log.WriteEnd("Updated web.config file");

			//update settings
			string componentId = Context.ComponentId;
			AppConfig.SetComponentSettingStringValue(componentId, "ConnectionString", connectionString);
		}

		private void ExecuteSqlScript(string file)
		{
			try
			{
				string component = Context.ComponentFullName;
				string componentId = Context.ComponentId;

				string path = Path.Combine(Context.InstallationFolder, file);
				if (Context.SetupAction == SetupActions.Update)
				{
					path = Path.Combine(Context.InstallerFolder, file);
					Context.DbInstallConnectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
					Context.Database = AppConfig.GetComponentSettingStringValue(componentId, "Database");
				}

				if (!FileUtils.FileExists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				string connectionString = Context.DbInstallConnectionString;
				string database = Context.Database;

				//if (Context.SetupAction == SetupActions.Install)
				//{
				//    UpdateSqlScript(path);
				//}
				RunSqlScript(connectionString, database, path);

				InstallLog.AppendLine(string.Format("- Installed {0} database objects", component));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Run sql error", ex);
				throw;
			}
		}

		private void CreateDatabase()
		{
			try
			{
				string connectionString = Context.DbInstallConnectionString;
				string database = Context.Database;

				Log.WriteStart("Creating database");
				Log.WriteInfo(string.Format("Creating database \"{0}\"", database));
				if (Data.DatabaseUtils.DatabaseExists(connectionString, database))
				{
					throw new Exception(string.Format("Database \"{0}\" already exists", database));
				}
				Data.DatabaseUtils.CreateDatabase(connectionString, database);
				Log.WriteEnd("Created database");

				// rollback
				RollBack.RegisterDatabaseAction(connectionString, database);

				string componentId = Context.ComponentId;
				AppConfig.SetComponentSettingStringValue(componentId, "Database", database);
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewDatabase", true);

				InstallLog.AppendLine(string.Format("- Created a new SQL Server database \"{0}\"", database));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Create database error", ex);
				throw;
			}
		}

		private void SetServerPassword()
		{
			if (!OSInfo.IsWindows)
			{
				SetServerPasswordUnix();
				return;
			}
			try
			{
				Log.WriteStart("Updating configuration file (server password)");

				string file = Path.Combine(Context.InstallationFolder, Context.ConfigurationFile);
				string hash = Context.ServerPassword;

				// load file
				string content = string.Empty;
				using (StreamReader reader = new StreamReader(file))
				{
					content = reader.ReadToEnd();
				}

				// expand variables
				content = Utils.ReplaceScriptVariable(content, "installer.server.password", hash);

				// save file
				using (StreamWriter writer = new StreamWriter(file))
				{
					writer.Write(content);
				}
				//update log
				Log.WriteEnd("Updated configuration file");

				//string component = Context.ComponentFullName;
				//InstallLog.AppendLine(string.Format("- Updated {0} web.config file", component));

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void SetServerPasswordUnix()
		{
			try
			{
				Log.WriteStart("Updating configuration file (server password)");
				var installer = UniversalInstaller.Installer.Current;
				installer.InstallWebRootPath = Context.InstallationFolder;
				installer.ReadServerConfiguration();
				installer.ServerSettings.ServerPasswordSHA = Context.ServerPassword;
				installer.ConfigureServer();
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated password in the configuration file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void UpdateServerPassword()
		{
			if (!OSInfo.IsWindows)
			{
				UpdateServerPasswordUnix();
				return;
			}
			try
			{
				if (!Context.UpdateServerPassword)
					return;

				string path = Path.Combine(Context.InstallationFolder, Context.ConfigurationFile);
				string hash = Utils.ComputeSHAServerPassword(Context.ServerPassword);

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Updating configuration file (server password)");
				var doc = new XmlDocument();
				doc.Load(path);

				XmlElement passwordNode = doc.SelectSingleNode("//SolidCP.server/security/password") as XmlElement;
				if (passwordNode == null)
				{
					Log.WriteInfo("server password setting not found");
					return;
				}

				passwordNode.SetAttribute("value", hash);
				doc.Save(path);
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated password in the configuration file");

				//string component = Context.ComponentFullName;
				//InstallLog.AppendLine(string.Format("- Updated {0} web.config file", component));

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void UpdateServerPasswordUnix()
		{
			try
			{
				Log.WriteStart("Updating configuration file (server password)");
				var installer = UniversalInstaller.Installer.Current;
				installer.InstallWebRootPath = Context.InstallationFolder;
				installer.ReadServerConfiguration();
				installer.ServerSettings.ServerPasswordSHA = Utils.ComputeSHAServerPassword(Context.ServerPassword);
				installer.ConfigureServer();
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated password in the configuration file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}
		private void SetServiceSettings()
		{
			try
			{
				string path = Path.Combine(Context.InstallationFolder, Context.ConfigurationFile);
				string ip = Context.ServiceIP;
				string port = Context.ServicePort;

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Updating configuration file (service settings)");
				var doc = new XmlDocument();
				doc.Load(path);

				XmlElement ipNode = doc.SelectSingleNode("//configuration/appSettings/add[@key='SolidCP.HostIP']") as XmlElement;
				if (ipNode == null)
				{
					Log.WriteInfo("Service host IP setting not found");
					return;
				}
				ipNode.SetAttribute("value", ip);
				XmlElement portNode = doc.SelectSingleNode("//configuration/appSettings/add[@key='SolidCP.HostPort']") as XmlElement;
				if (portNode == null)
				{
					Log.WriteInfo("Service host port setting not found");
					return;
				}
				portNode.SetAttribute("value", port);
				doc.Save(path);
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated service settings in the configuration file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void SetCryptoKey()
		{
			try
			{
				Log.WriteStart("Updating web.config file (crypto key)");

				string file = Path.Combine(Context.InstallationFolder, "web.config");
				string cryptoKey = Utils.GetRandomString(20);

				// load file
				string content = string.Empty;
				using (StreamReader reader = new StreamReader(file))
				{
					content = reader.ReadToEnd();
				}

				// expand variables
				content = Utils.ReplaceScriptVariable(content, "installer.cryptokey", cryptoKey);

				// save file
				using (StreamWriter writer = new StreamWriter(file))
				{
					writer.Write(content);
				}
				//update log
				Log.WriteEnd("Updated web.config file");

				string componentId = Context.ComponentId;
				Context.CryptoKey = cryptoKey;
				AppConfig.SetComponentSettingStringValue(componentId, "CryptoKey", cryptoKey);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update web.config error", ex);
				throw;
			}
		}

		private void CopyFiles(string source, string destination)
		{
			try
			{
				string component = Context.ComponentFullName;
				Log.WriteStart("Copying files");
				Log.WriteInfo(string.Format("Copying files from \"{0}\" to \"{1}\"", source, destination));
				//showing copy process
				CopyProcess process = new CopyProcess(GetProgressObject(), source, destination);
				process.Run();
				Log.WriteEnd("Copied files");
				InstallLog.AppendLine(string.Format("- Copied {0} files", component));
				// rollback
				RollBack.RegisterDirectoryAction(destination);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Copy error", ex);
				throw;
			}
		}

		private void CreateWebSite()
		{
			string component = Context.ComponentFullName;
			string ip = Context.WebSiteIP;
			string port = Context.WebSitePort;
			string domain = Context.WebSiteDomain;
			string contentPath = Context.InstallationFolder;

			//creating user account
			string userName = Context.UserAccount;
			string password = Context.UserPassword;
			string userDomain = Context.UserDomain;
			string userDescription = component + " account for anonymous access to Internet Information Services";
			string[] memberOf = Context.UserMembership;
			string identity = userName;
			string netbiosDomain = userDomain;
			Version iisVersion = Context.IISVersion;
			bool iis7 = (iisVersion.Major >= 7);

			try
			{
				CreateUserAccount(userDomain, userName, password, userDescription, memberOf);
				if (!string.IsNullOrEmpty(userDomain))
				{
					netbiosDomain = SecurityUtils.GetNETBIOSDomainName(userDomain);
					if (iis7)
					{
						//for iis7 we use fqdn\user
						identity = string.Format("{0}\\{1}", userDomain, userName);
					}
					else
					{
						//for iis6 we use netbiosdomain\user
						identity = string.Format("{0}\\{1}", netbiosDomain, userName);
					}
				}
				WebUtils.SetWebFolderPermissions(contentPath, netbiosDomain, userName);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create user account error", ex);
				throw;
			}
			//progressBar.Value = 30;
			this.SetProgressValue(30);

			//creating app pool
			string appPool = component + " Pool";
			try
			{
				CreateAppPool(appPool, identity, password);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError(string.Format("Create application pool \"{0}\" error", appPool), ex);
				throw;
			}
			//progressBar.Value = 60;
			this.SetProgressValue(60);

			//creating web site
			string siteName = component;
			try
			{
				CreateSite(siteName, ip, port, domain, contentPath, identity, password, appPool);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create web site error", ex);
				throw;
			}

			//progressBar.Value = 90;
			this.SetProgressValue(90);

			//opening windows firewall ports
			try
			{
				Utils.OpenFirewallPort(component, port, Context.IISVersion);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Open windows firewall port error", ex);
			}

		}

		private void ConfigureFolderPermissions()
		{
			if (!OSInfo.IsWindows) return;

			try
			{
				string path;
				if (Context.IISVersion.Major == 6)
				{
					// IIS_WPG -> C:\WINDOWS\Temp
					path = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine);
					SetFolderPermission(path, "IIS_WPG", NtfsPermission.Modify);

					// IIS_WPG - > C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Temporary ASP.NET Files
					path = Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(),
						"Temporary ASP.NET Files");
					if (Utils.IsWin64() && Utils.IIS32Enabled())
						path = path.Replace("Framework64", "Framework");
					SetFolderPermission(path, "IIS_WPG", NtfsPermission.Modify);
				}
				// NETWORK_SERVICE -> C:\WINDOWS\Temp
				path = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine);
				SetFolderPermissionBySid(path, SystemSID.NETWORK_SERVICE, NtfsPermission.Modify);

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}

		private void SetFolderPermission(string path, string account, NtfsPermission permission)
		{
			try
			{
				if (!FileUtils.DirectoryExists(path))
				{
					FileUtils.CreateDirectory(path);
					Log.WriteInfo(string.Format("Created {0} folder", path));
				}

				Log.WriteStart(string.Format("Setting '{0}' permission for '{1}' folder for '{2}' account", permission, path, account));
				SecurityUtils.GrantNtfsPermissions(path, null, account, permission, true, true);
				Log.WriteEnd("Set security permissions");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}

		private void SetFolderPermissionBySid(string path, string account, NtfsPermission permission)
		{
			try
			{
				if (!FileUtils.DirectoryExists(path))
				{
					FileUtils.CreateDirectory(path);
					Log.WriteInfo(string.Format("Created {0} folder", path));
				}

				Log.WriteStart(string.Format("Setting '{0}' permission for '{1}' folder for '{2}' account", permission, path, account));
				SecurityUtils.GrantNtfsPermissionsBySid(path, account, permission, true, true);
				Log.WriteEnd("Set security permissions");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}

		private void CreateSite(string siteName, string ip, string port, string domain, string contentPath, string userName, string userPassword, string appPool)
		{
			SetProgressText("Creating web site...");

			Log.WriteStart("Creating web site");
			Log.WriteInfo(string.Format("Creating web site \"{0}\" ( IP: {1}, Port: {2}, Domain: {3} )", siteName, ip, port, domain));
			Version iisVersion = Context.IISVersion;
			var componentId = Context.ComponentId;
			bool iis7 = (iisVersion.Major >= 7);

			//check for existing site
			string oldSiteId = iis7 ? WebUtils.GetIIS7SiteIdByBinding(ip, port, domain) : WebUtils.GetSiteIdByBinding(ip, port, domain);
			if (oldSiteId != null)
			{
				// get site name
				string oldSiteName = iis7 ? oldSiteId : WebUtils.GetSite(oldSiteId).Name;
				throw new Exception(
					String.Format("'{0}' web site already has server binding ( IP: {1}, Port: {2}, Domain: {3} )",
					oldSiteName, ip, port, domain));
			}

			// create site
			WebSiteItem site = new WebSiteItem();
			site.Name = siteName;
			site.SiteIPAddress = ip;

			site.ContentPath = contentPath;
			//site.LogFileDirectory = logsPath;

			//set bindings
			ServerBinding binding = new ServerBinding(ip, port, domain, null, componentId);
			site.Bindings = new ServerBinding[] { binding };

			// set other properties
			site.AllowExecuteAccess = false;
			site.AllowScriptAccess = true;
			site.AllowSourceAccess = false;
			site.AllowReadAccess = true;
			site.AllowWriteAccess = false;
			site.AnonymousUsername = userName;
			site.AnonymousUserPassword = userPassword;
			site.AllowDirectoryBrowsingAccess = false;

			site.AuthAnonymous = true;
			site.AuthWindows = true;

			site.DefaultDocs = null; // inherit from service
			site.HttpRedirect = "";

			site.InstalledDotNetFramework = AspNetVersion.AspNet20;
			site.ApplicationPool = appPool;

			// create site
			string newSiteId = iis7 ? WebUtils.CreateIIS7Site(site) : WebUtils.CreateSite(site);

			//add rollback action
			if (iis7)
				RollBack.RegisterIIS7WebSiteAction(newSiteId);
			else
				RollBack.RegisterWebSiteAction(newSiteId);


			// update config setings
			AppConfig.SetComponentSettingStringValue(componentId, "WebSiteId", newSiteId);
			AppConfig.SetComponentSettingStringValue(componentId, "WebSiteIP", ip);
			AppConfig.SetComponentSettingStringValue(componentId, "WebSitePort", port);
			AppConfig.SetComponentSettingStringValue(componentId, "WebSiteDomain", domain);
			AppConfig.SetComponentSettingStringValue(componentId, "VirtualDirectory", string.Empty);
			AppConfig.SetComponentSettingBooleanValue(componentId, "NewWebSite", true);
			AppConfig.SetComponentSettingBooleanValue(componentId, "NewVirtualDirectory", false);

			// update setup variables
			Context.WebSiteId = newSiteId;

			//update log
			Log.WriteEnd("Created web site");

			//update install log
			InstallLog.AppendLine(string.Format("- Created a new web site named \"{0}\" ({1})", siteName, newSiteId));
			InstallLog.AppendLine("  You can access the application by the following URLs:");
			string[] urls = GetApplicationUrls(ip, domain, port, null);
			foreach (string url in urls)
			{
				InstallLog.AppendLine("  http://" + url);
			}
		}

		private void CreateAccount(string description)
		{
			string component = Context.ComponentFullName;

			//creating user account
			string userName = Context.UserAccount;
			string password = Context.UserPassword;
			string userDomain = Context.UserDomain;
			string userDescription = description;
			string[] memberOf = Context.UserMembership;

			try
			{
				CreateUserAccount(userDomain, userName, password, userDescription, memberOf);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create user account error", ex);
				throw;
			}
		}

		private void CreateUserAccount(string domain, string userName, string password, string description, string[] memberOf)
		{
			SetProgressText("Creating windows user account...");
			this.Update();
			string componentId = Context.ComponentId;

			if (!SecurityUtils.UserExists(domain, userName))
			{
				Log.WriteStart("Creating Windows user account");
				Log.WriteInfo(string.Format("Creating Windows user account \"{0}\"", userName));

				// create account
				SystemUserItem user = new SystemUserItem();
				user.Domain = domain;
				user.Name = userName;
				user.FullName = userName;
				user.Description = description;
				user.MemberOf = memberOf;
				user.Password = password;
				user.PasswordCantChange = true;
				user.PasswordNeverExpires = true;
				user.AccountDisabled = false;
				user.System = true;
				SecurityUtils.CreateUser(user);

				//add rollback action
				RollBack.RegisterUserAccountAction(domain, userName);

				// update config setings
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewUserAccount", true);
				AppConfig.SetComponentSettingStringValue(componentId, "UserAccount", userName);
				AppConfig.SetComponentSettingStringValue(componentId, "Domain", domain);

				//update log
				Log.WriteEnd("Created windows user account");

				//update install log
				if (string.IsNullOrEmpty(domain))
					InstallLog.AppendLine(string.Format("- Created a new windows user account \"{0}\"", userName));
				else
					InstallLog.AppendLine(string.Format("- Created a new windows user account \"{0}\" in \"{1}\" domain", userName, domain));
			}
			else
			{
				throw new Exception("Account already exists");
			}
		}

		protected void Update() // Do nothing.
		{
			//throw new NotImplementedException();
		}

		private void CreateAppPool(string name, string userName, string userPassword)
		{
			SetProgressText("Creating local account...");
			string componentId = Context.ComponentId;
			Version iisVersion = Context.IISVersion;
			bool poolExists = (iisVersion.Major >= 7) ?
				WebUtils.IIS7ApplicationPoolExists(name) :
				WebUtils.ApplicationPoolExists(name);

			if (poolExists)
			{
				//update app pool
				Log.WriteStart("Updating application pool");
				Log.WriteInfo(string.Format("Updating application pool \"{0}\"", name));
				if (iisVersion.Major >= 7)
					WebUtils.UpdateIIS7ApplicationPool(name, userName, userPassword);
				else
					WebUtils.UpdateApplicationPool(name, userName, userPassword);

				// update config setings
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewApplicationPool", false);
				AppConfig.SetComponentSettingStringValue(componentId, "ApplicationPool", name);

				//update log
				Log.WriteEnd("Updated application pool");

				//update install log
				InstallLog.AppendLine(string.Format("- Updated application pool named \"{0}\"", name));
			}
			else
			{
				// create app pool
				Log.WriteStart("Creating application pool");
				Log.WriteInfo(string.Format("Creating application pool \"{0}\"", name));
				if (iisVersion.Major >= 7)
					WebUtils.CreateIIS7ApplicationPool(name, userName, userPassword);
				else
					WebUtils.CreateApplicationPool(name, userName, userPassword);

				//register rollback action
				if (iisVersion.Major >= 7)
					RollBack.RegisterIIS7ApplicationPool(name);
				else
					RollBack.RegisterApplicationPool(name);


				// update config setings
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewApplicationPool", true);
				AppConfig.SetComponentSettingStringValue(componentId, "ApplicationPool", name);

				//update log
				Log.WriteEnd("Created application pool");

				//update install log
				InstallLog.AppendLine(string.Format("- Created a new application pool named \"{0}\"", name));
			}
		}

		/// <summary>
		/// Returns the list of all possible application URLs
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="domain"></param>
		/// <param name="port"></param>
		/// <param name="virtualDir"></param>
		/// <returns></returns>
		protected string[] GetApplicationUrls(string ip, string domain, string port, string virtualDir)
		{
			List<string> urls = new List<string>();

			// IP address, [port] and [virtualDir]
			string url = ip;
			if (String.IsNullOrEmpty(domain))
			{
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			// domain, [port] and [virtualDir]
			if (!String.IsNullOrEmpty(domain))
			{
				url = domain;
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			// add "localhost" by default
			/*
            url = "localhost";
            if(port != "" && port != "80" && port != null)
                url += ":" + port;
            if(virtualDir != "" && virtualDir != null)
                url += "/" + virtualDir;
            urls.Add(url);
             * */

			return urls.ToArray();
		}

		private void UpdateSystemConfiguration()
		{
			try
			{
				Log.WriteStart("Updating system configuration");
				string componentId = Context.ComponentId;
				if (Context.SetupAction == SetupActions.Update)
				{
					//update settings
					AppConfig.SetComponentSettingStringValue(componentId, "Release", Context.UpdateVersion);
					AppConfig.SetComponentSettingStringValue(componentId, "Installer", Context.SessionVariables["Installer"]);
					AppConfig.SetComponentSettingStringValue(componentId, "InstallerType", Context.SessionVariables["InstallerType"]);
					AppConfig.SetComponentSettingStringValue(componentId, "InstallerPath", Context.SessionVariables["InstallerPath"]);
				}

				Log.WriteInfo("Saving system configuration");
				//save
				AppConfig.SaveConfiguration();
				Log.WriteEnd("Updated system configuration");
				InstallLog.AppendLine("- Updated system configuration");

				if (Context.SetupAction == SetupActions.Install)
				{
					RollBack.RegisterConfigAction(Context.ComponentId, Context.ComponentName);
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Config error", ex);
				throw;
			}
		}
		private void UpdateSqlScript(string file)
		{
			// get ES crypto key from the registry
			string cryptoKey = Context.CryptoKey;
			if (String.IsNullOrEmpty(cryptoKey))
				return;

			Log.WriteStart("Updating SQL script file");
			Log.WriteInfo(string.Format("Updating SQL script file \"{0}\"", file));

			//update 'Users' table
			string text = GetServerAdminPasswordScript(cryptoKey);

			using (StreamWriter sw = File.AppendText(file))
			{
				sw.Write(text);
			}
			Log.WriteEnd("Updated SQL script file");
		}

		private string GetServerAdminPasswordScript(string cryptoKey)
		{
			// encrypt default password
			string password = Utils.Encrypt(cryptoKey, "serveradmin");

			// build script
			StringBuilder sb = new StringBuilder();

			sb.Append("\n\nIF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[ResourceGroupDnsRecords]') AND type in (N'U'))\n");
			sb.Append("BEGIN\n\n");

			sb.Append("\nUPDATE Users SET Password = '");
			sb.Append(password).Append("' WHERE Username = 'serveradmin'\n");

			sb.Append("END\nGO\n\n");

			return sb.ToString();
		}

		/// <summary>
		/// Runs sql script.
		/// </summary>
		/// <param name="connectionString">Sql server connection string.</param>
		/// <param name="database">Sql server database name.</param>
		/// <param name="fileName">Sql script file</param>
		private void RunSqlScript(string connectionString, string database, string fileName)
		{
			if (!File.Exists(fileName))
			{
				Log.WriteInfo(string.Format("File {0} not found", fileName));
				return;
			}

			Log.WriteStart("Installing database objects");

			//showing process
			SqlProcess process = new SqlProcess(fileName, connectionString, database);
			// Update progress change
			process.ProgressChange += new EventHandler<ActionProgressEventArgs<int>>((object sender, ActionProgressEventArgs<int> e) =>
			{
				//this.progressBar.Value = e.EventData;
				this.SetProgressValue(e.EventData);
			});
			//
			process.Run();

			Log.WriteEnd("Installed database objects");
		}
		/// <summary>
		/// Add custom error page to the web.config (Microsoft Security Advisory (2416728))
		/// </summary>
		private void AddCustomErrorsPage()
		{
			try
			{
				string webConfigPath = Path.Combine(Context.InstallationFolder, "web.config");
				Log.WriteStart("Web.config file is being updated");
				// Ensure the web.config exists
				if (!File.Exists(webConfigPath))
				{
					Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
					return;
				}
				// Load web.config
				var doc = new XmlDocument();
				doc.Load(webConfigPath);

				// replace existing node:
				// <system.web>
				//	 <customErrors mode="Off" />
				// </system.web>
				// with:
				// <system.web>
				//	 <customErrors mode="RemoteOnly" defaultRedirect="~/error.htm" />
				// </system.web>
				//
				XmlElement customErrors = doc.SelectSingleNode("configuration/system.web/customErrors[@mode='Off']") as XmlElement;
				// ensure node is found
				if (customErrors != null)
				{
					XmlUtils.SetXmlAttribute(customErrors, "mode", "RemoteOnly");
					XmlUtils.SetXmlAttribute(customErrors, "defaultRedirect", "~/error.htm");
				}
				// save changes have been made
				doc.Save(webConfigPath);
				//
				Log.WriteEnd("Web.config has been updated");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Could not update web.config file", ex);
				throw;
			}
		}
		#endregion
		#endregion
		private void RestoreXmlConfigs(SetupVariables Ctx)
		{
			try
			{
				XmlDocumentMerge.KeyAttributes = new List<string> { "name", "id", "key", "pageID", "localName", "xmlns", "privatePath", "moduleDefinitionID", "ref", "verb;path", "controlRenderingCompatibilityVersion;clientIDMode" };
				XmlDocumentMerge.FrozenAttributes = new List<XmlDocumentMerge.FrozenAttrTag>
				{
					new XmlDocumentMerge.FrozenAttrTag() { Path="configuration/microsoft.web.services3/security/securityTokenManager/add", Attributes = new List<string>() {"type"} },
					new XmlDocumentMerge.FrozenAttrTag(true) { Path="compilation", Attributes = new List<string>() {"targetFramework"} },
					new XmlDocumentMerge.FrozenAttrTag() { Path="configuration/startup/supportedRuntime", Attributes = new List<string>() {"version", "sku" } }
				};
				Log.WriteStart("RestoreXmlConfigs");
				var Backup = BackupRestore.Find(Ctx.InstallerFolder, Global.DefaultProductName, Ctx.ComponentName);
                switch(Ctx.ComponentCode)
				{
					case Global.Server.ComponentCode:
						{
							Backup.XmlFiles.Add("Web.config");
						}
						break;
					case Global.EntServer.ComponentCode:
						{
							Backup.XmlFiles.Add("Web.config");
						}
						break;
					case Global.WebPortal.ComponentCode:
						{
							Backup.XmlFiles.Add("Web.config");
							Backup.XmlFiles.Add(@"App_Data\SiteSettings.config");
							Backup.XmlFiles.Add(@"App_Data\SolidCP_Pages.config");
						}
						break;
						//case Global.Scheduler.ComponentCode:
						//{
						//   Backup.XmlFiles.Add("SolidCP.SchedulerService.exe.config");
						//}
						//break;
				}
				var MainCfg = Path.Combine(Ctx.InstallerFolder, BackupRestore.MainConfig);
				if (string.IsNullOrWhiteSpace(WiXSetup.GetComponentID(MainCfg, Ctx.ComponentCode)))
				{
					try
					{
						Log.WriteInfo(string.Format("Restoring main config section for '{0}' component...", Ctx.ComponentCode));
						var Current = new XmlDocument();
						Current.Load(MainCfg);
						var Components = Current.SelectSingleNode("//components");
						var XmlPath = string.Format("//component[.//add/@key='ComponentCode' and .//add/@value='{0}']", Ctx.ComponentCode);
						var Previous = new XmlDocument();
						Previous.Load(Backup.BackupMainConfigFile);
						var PreviousComponent = Previous.SelectSingleNode(XmlPath);
						Components.CreateNavigator().AppendChild(PreviousComponent.CloneNode(true).CreateNavigator());
						Current.Save(MainCfg);
						Context.ComponentId = WiXSetup.GetComponentID(Ctx);
						AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = MainCfg });
						AppConfig.LoadComponentSettings(Ctx);
					}
					catch (Exception ex)
					{
						Log.WriteError("Error in main or backup xml config files.", ex);
					}
				}
				Log.WriteInfo(string.Format("Restoring xml config for component - {0}.", Ctx.ComponentFullName));
				Backup.Restore();
				Log.WriteEnd("RestoreXmlConfigs");
			}
			catch (Exception ex)
			{
				Log.WriteError("RestoreXmlConfigs", ex);
				throw;
			}
		}
		private void UpdateXml(string FullFileName, IDictionary<string, string[]> Data)
		{
			var Msg = "Update xml files.";
			try
			{
				Log.WriteStart(Msg);
				if (!File.Exists(FullFileName))
					throw new FileNotFoundException(FullFileName);
				var Doc = new XmlDocument();
				Doc.Load(FullFileName);
                foreach(var Key in Data.Keys)
				{
					var Node = Doc.SelectSingleNode(Key) as XmlElement;
					if (Node == null)
					{
						Log.WriteInfo(string.Format("XPath \"{0}\" not found.", Key));
					}
					else
					{
						var Value = Data[Key];
						switch (Value.Length)
						{
							case 1:
								Node.Value = Value[0];
								break;
							case 2:
								Node.SetAttribute(Value[0], Value[1]);
								break;
							default:
								Log.WriteError(string.Format("Bad xml value for \"{0}\".", Key));
								break;
						}
					}
				}
				Doc.Save(FullFileName);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex.ToString());
				throw;
			}
			finally
			{
				Log.WriteEnd(Msg);
			}
		}
		private void SaveComponentConfig(SetupVariables CompCtx)
		{
			const string Msg = "Updating system configuration...";
			Log.WriteStart(Msg);
			AppConfig.EnsureComponentConfig(CompCtx.ComponentId);
			var TypeInfo = CompCtx.GetType();
			foreach (var Field in CompCtx.SysFields)
			{
				var TypeField = TypeInfo.GetProperty(Field);
				if (TypeField.PropertyType.Equals(typeof(string)))
					AppConfig.SetComponentSettingStringValue(CompCtx.ComponentId, Field, Convert.ToString(TypeField.GetValue(CompCtx, null)));
				else if (TypeField.PropertyType.Equals(typeof(bool)))
					AppConfig.SetComponentSettingBooleanValue(CompCtx.ComponentId, Field, Convert.ToBoolean(TypeField.GetValue(CompCtx, null)));
				else
					Log.WriteError(string.Format("Unknown type '{1}' for field '{0}' in system configuration of '{2}'.", Field, TypeField.PropertyType.FullName, CompCtx.ComponentCode));
			}
			AppConfig.SaveConfiguration();
			Log.WriteEnd(Msg);
		}
	}
	public class UninstallScript : SetupScript // UninstallPage
	{
        public UninstallScript(SetupVariables SessionVariables):base(SessionVariables)
		{

		}
		protected override List<InstallAction> GetActions(string componentId)
		{
			var list = base.GetActions(componentId);
			InstallAction action = null;
			//windows service
			string serviceName = AppConfig.GetComponentSettingStringValue(componentId, "ServiceName");
			string serviceFile = AppConfig.GetComponentSettingStringValue(componentId, "ServiceFile");
			string installFolder = AppConfig.GetComponentSettingStringValue(componentId, "InstallFolder");
			if (!string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(serviceFile))
			{
				action = new InstallAction(ActionTypes.UnregisterWindowsService);
				action.Path = serviceFile; // FullFileName.
				action.Name = serviceName;
				action.Description = "Removing Windows service...";
				action.Log = string.Format("- Remove {0} Windows service", serviceName);
				list.Add(action);
			}
			//database
			bool deleteDatabase = AppConfig.GetComponentSettingBooleanValue(componentId, "NewDatabase");
			bool allowDelete = true;
			if (Context.InstallerType.ToLowerInvariant().Equals("msi")) // DB handled by MSI (WiX) by default.
				allowDelete = false;
			if (deleteDatabase && allowDelete)
			{
				string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
				string database = AppConfig.GetComponentSettingStringValue(componentId, "Database");
				action = new InstallAction(ActionTypes.DeleteDatabase);
				action.ConnectionString = connectionString;
				action.Name = database;
				action.Description = "Deleting database...";
				action.Log = string.Format("- Delete {0} database", database);
				list.Add(action);
			}
			//database user
			bool deleteDatabaseUser = AppConfig.GetComponentSettingBooleanValue(componentId, "NewDatabaseUser");
			if (deleteDatabaseUser)
			{
				string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
				string username = AppConfig.GetComponentSettingStringValue(componentId, "DatabaseUser");
				action = new InstallAction(ActionTypes.DeleteDatabaseUser);
				action.ConnectionString = connectionString;
				action.UserName = username;
				action.Description = "Deleting database user...";
				action.Log = string.Format("- Delete {0} database user", username);
				list.Add(action);
			}
			//database login (from standalone setup)
			string loginName = AppConfig.GetComponentSettingStringValue(componentId, "DatabaseLogin");
			if (!string.IsNullOrEmpty(loginName))
			{
				string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
				action = new InstallAction(ActionTypes.DeleteDatabaseLogin);
				action.ConnectionString = connectionString;
				action.UserName = loginName;
				action.Description = "Deleting database login...";
				action.Log = string.Format("- Delete {0} database login", loginName);
				list.Add(action);
			}
			//virtual directory
			bool deleteVirtualDirectory = AppConfig.GetComponentSettingBooleanValue(componentId, "NewVirtualDirectory");
			if (deleteVirtualDirectory)
			{
				string virtualDirectory = AppConfig.GetComponentSettingStringValue(componentId, "VirtualDirectory");
				string virtualDirectorySiteId = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteId");
				action = new InstallAction(ActionTypes.DeleteVirtualDirectory);
				action.SiteId = virtualDirectorySiteId;
				action.Name = virtualDirectory;
				action.Description = "Deleting virtual directory...";
				action.Log = string.Format("- Delete {0} virtual directory...", virtualDirectory);
				list.Add(action);
			}
			//web site
			bool deleteWebSite = AppConfig.GetComponentSettingBooleanValue(componentId, "NewWebSite");
			if (deleteWebSite)
			{
				string siteId = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteId");
				action = new InstallAction(ActionTypes.DeleteWebSite);
				action.SiteId = siteId;
				action.Description = "Deleting web site...";
				action.Log = string.Format("- Delete {0} web site", siteId);
				list.Add(action);
			}
			//application pool
			bool deleteAppPool = AppConfig.GetComponentSettingBooleanValue(componentId, "NewApplicationPool");
			if (deleteAppPool)
			{
				string appPoolName = AppConfig.GetComponentSettingStringValue(componentId, "ApplicationPool");
				if (string.IsNullOrEmpty(appPoolName))
					appPoolName = WebUtils.SolidCP_ADMIN_POOL;
				action = new InstallAction(ActionTypes.DeleteApplicationPool);
				action.Name = appPoolName;
				action.Description = "Deleting application pool...";
				action.Log = string.Format("- Delete {0} application pool", appPoolName);
				list.Add(action);
			}
			//user account
			bool deleteUserAccount = AppConfig.GetComponentSettingBooleanValue(componentId, "NewUserAccount");
			if (deleteUserAccount)
			{
				string username = AppConfig.GetComponentSettingStringValue(componentId, "UserAccount");
				string domain = AppConfig.GetComponentSettingStringValue(componentId, "Domain");
				//membership
				if (Context.UserMembership != null && Context.UserMembership.Length > 0)
				{
					action = new InstallAction(ActionTypes.DeleteUserMembership);
					action.Name = username;
					action.Domain = domain;
					action.Membership = Context.UserMembership;
					action.Description = "Removing user account membership...";
					action.Log = string.Format("- Remove {0} user account membership", username);
					list.Add(action);
				}
				action = new InstallAction(ActionTypes.DeleteUserAccount);
				action.Name = username;
				action.Domain = domain;
				action.Description = "Deleting user account...";
				action.Log = string.Format("- Delete {0} user account", username);
				list.Add(action);
			}

			// TODO: WiX does not need it by default.
			//directory
			//string path = AppConfig.GetComponentSettingStringValue(componentId, "InstallFolder");
			//if (!string.IsNullOrEmpty(path))
			//{
			//    action = new InstallAction(ActionTypes.DeleteDirectory);
			//    action.Path = path;
			//    action.Description = "Deleting application folder...";
			//    action.Log = string.Format("- Delete {0} folder", path);
			//    list.Add(action);
			//}

			//config
			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Key = componentId;
			action.Description = "Updating configuration settings...";
			action.Log = "- Update configuration settings";
			list.Add(action);
			return list;
		}
		protected override void ProcessError(Exception ex)
		{
			Log.WriteError("Uninstall error", ex);
		}
	}
	public class ExpressScript : SetupScript // ExpressInstallPage
	{
		public ExpressScript(SetupVariables SessionVariables)
			: base(SessionVariables)
		{

		}
		protected override void ProcessError(Exception ex)
		{
			var Msg = "An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.";
			Log.WriteError(Msg, ex);
		}
	}
	public class BackupScript : ExpressScript
	{
		public BackupScript(SetupVariables SessionVariables)
			: base(SessionVariables)
		{
			Context.SetupAction = SetupActions.Update;
		}
		protected override List<InstallAction> GetActions(string ComponentID)
		{
			var Scenario = base.GetActions(ComponentID);
			var Act = new InstallAction(ActionTypes.StopApplicationPool);
			Act.Description = "Stopping IIS Application Pool...";
			Scenario.Add(Act);
			Act = new InstallAction(ActionTypes.Backup);
			Act.Description = "Backing up...";
			Scenario.Add(Act);
			return Scenario;
		}
	}
	public class BackupSchedulerScript : ExpressScript
	{
		public BackupSchedulerScript(SetupVariables SessionVariables)
			: base(SessionVariables)
		{
			Context.SetupAction = SetupActions.Update;
		}

		protected override List<InstallAction> GenerateBackupActions(string componentId)
		{
			List<InstallAction> list = new List<InstallAction>();
			InstallAction action = null;
			string path = Directory.GetParent(Context.ServiceFile).FullName;
			if (!string.IsNullOrEmpty(path))
			{
				action = new InstallAction(ActionTypes.BackupDirectory);
				action.Path = path;
				action.Description = string.Format("Backing up directory {0}...", path);
				list.Add(action);
			}
			return list;
		}
	}
	public class RestoreScript : ExpressScript
	{
		public RestoreScript(SetupVariables SessionVariables)
			: base(SessionVariables)
		{
			Context.SetupAction = SetupActions.Update;
		}
		protected override List<InstallAction> GetActions(string ComponentID)
		{
			var Scenario = base.GetActions(ComponentID);
			Scenario.Add(new InstallAction(ActionTypes.RestoreConfig) { SetupVariables = Context, Description = "Restoring xml configuration files..." });
			Scenario.Add(new InstallAction(ActionTypes.UpdateConfig) { Description = "Updating system configuration..." });
			Scenario.Add(new InstallAction(ActionTypes.StartApplicationPool) { Description = "Starting IIS Application Pool..." });
			return Scenario;
		}
	}
    public class MaintenanceScript: ExpressScript
	{
        public MaintenanceScript(SetupVariables SessionVariables):base(SessionVariables)
		{
			Context.SetupAction = SetupActions.Setup;
		}
		protected override List<InstallAction> GetActions(string ComponentID)
		{
			var Scenario = base.GetActions(ComponentID);
			Scenario.Add(new InstallAction(ActionTypes.UpdateConfig) { Description = "Updating system configuration..." });
			return Scenario;
		}
	}
	public class ServerSetup : WiXSetup
	{
		public ServerSetup(SetupVariables Ctx, ModeExtension Ext)
			: base(Ctx, Ext)
		{

		}
		public static IWiXSetup Create(IDictionary<string, string> Ctx, SetupActions Action)
		{
			var SetupVars = new SetupVariables();
			FillFromSession(Ctx, SetupVars);
			SetupVars.SetupAction = Action;
			SetupVars.IISVersion = Global.IISVersion;
			AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = GetFullConfigPath(SetupVars) });
			return new ServerSetup(SetupVars, GetModeExtension(Ctx));
		}
		protected override void Install()
		{
			bool WiXThrow = default(bool);
			if (ModeExtension == ModeExtension.Normal)
			{
				Context.ComponentId = Guid.NewGuid().ToString();
				Context.Instance = String.Empty;
				var sam = new WiXServerActionManager(Context);
				sam.PrepareDistributiveDefaults();
				try
				{
					sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Log.WriteError(e.ErrorMessage);
						WiXThrow = true;
					});
					sam.Start();
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to install the component", ex);
				}
				if (WiXThrow)
					InstallFailed();
			}
			else if (ModeExtension == ModeExtension.Restore)
			{
				Context.ComponentId = GetComponentID(Context);
				AppConfig.LoadComponentSettings(Context);
				new RestoreScript(Context).Run();
			}
			else
				throw new NotImplementedException("Install " + ModeExtension.ToString());
		}
		protected override void Uninstall()
		{
			Context.ComponentId = GetComponentID(Context);
			AppConfig.LoadComponentSettings(Context);
			SetupScript Script = null;
			switch (ModeExtension)
			{
				case ModeExtension.Normal:
					Script = new UninstallScript(Context);
					break;
				case ModeExtension.Backup:
					Script = new BackupScript(Context);
					break;
				default:
					throw new NotImplementedException("Uninstall " + ModeExtension.ToString());
			}
			Script.Run();
		}
		protected override void Maintenance()
		{
			Context.ComponentId = GetComponentID(Context);
			AppConfig.LoadComponentSettings(Context);
			SetupScript Script = new MaintenanceScript(Context);
			Script.Actions.Add(new InstallAction(ActionTypes.UpdateServerPassword) { Description = "Updating server password..." });
			Script.Run();
		}
	}
	public class EServerSetup : WiXSetup
	{
		public EServerSetup(SetupVariables Ctx, ModeExtension Ext)
			: base(Ctx, Ext)
		{

		}
		public static IWiXSetup Create(IDictionary<string, string> Ctx, SetupActions Action)
		{
			var SetupVars = new SetupVariables();
			FillFromSession(Ctx, SetupVars);
			SetupVars.SetupAction = Action;
			SetupVars.IISVersion = Global.IISVersion;
			AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = GetFullConfigPath(SetupVars) });
			return new EServerSetup(SetupVars, GetModeExtension(Ctx));
		}
		protected override void Install()
		{
			if (ModeExtension == ModeExtension.Normal)
			{
				bool WiXThrow = default(bool);
				Context.ComponentId = Guid.NewGuid().ToString();
				Context.Instance = String.Empty;
				var sam = new WiXEnterpriseServerActionManager(Context);
				sam.PrepareDistributiveDefaults();
				try
				{
					sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Log.WriteError(e.ErrorMessage);
						WiXThrow = true;
					});
					sam.Start();
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to install the component", ex);
				}
				if (WiXThrow)
					InstallFailed();
			}
			else if (ModeExtension == ModeExtension.Restore)
			{
				Context.ComponentId = GetComponentID(Context);
				Context.UseUserCredentials = false;
				AppConfig.LoadComponentSettings(Context);
				SetupScript Script = new RestoreScript(Context);
				Script.Run();
			}
			else
				throw new NotImplementedException("Install " + ModeExtension.ToString());
		}
		protected override void Uninstall()
		{
			Context.ComponentId = GetComponentID(Context);
			AppConfig.LoadComponentSettings(Context);
			SetupScript Script = null;
			switch (ModeExtension)
			{
				case ModeExtension.Normal:
					Script = new UninstallScript(Context);
					break;
				case ModeExtension.Backup:
					Script = new BackupScript(Context);
					Script.Actions.Add(new InstallAction(ActionTypes.StopWindowsService));
					break;
				default:
					throw new NotImplementedException("Uninstall " + ModeExtension.ToString());
			}
			Script.Run();
		}
		protected override void Maintenance()
		{
			Context.ComponentId = GetComponentID(Context);
			AppConfig.LoadComponentSettings(Context);
			SetupScript Script = new MaintenanceScript(Context);
			Script.Actions.Add(new InstallAction(ActionTypes.UpdateServerAdminPassword) { Description = "Updating serveradmin password..." });
			Script.Run();
		}
	}
	public class PortalSetup : WiXSetup
	{
		public PortalSetup(SetupVariables Ctx, ModeExtension Ext)
			: base(Ctx, Ext)
		{

		}
		public static IWiXSetup Create(IDictionary<string, string> Ctx, SetupActions Action)
		{
			var SetupVars = new SetupVariables();
			FillFromSession(Ctx, SetupVars);
			SetupVars.SetupAction = Action;
			SetupVars.IISVersion = Global.IISVersion;
			AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = GetFullConfigPath(SetupVars) });
			return new PortalSetup(SetupVars, GetModeExtension(Ctx));
		}
		protected override void Install()
		{
			if (ModeExtension == ModeExtension.Normal)
			{
				bool WiXThrow = default(bool);
				Context.ComponentId = Guid.NewGuid().ToString();
				Context.Instance = String.Empty;
				var sam = new WiXPortalActionManager(Context);
				sam.PrepareDistributiveDefaults();
				try
				{
					sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Log.WriteError(e.ErrorMessage);
						WiXThrow = true;
					});
					sam.Start();
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to install the component", ex);
				}
				if (WiXThrow)
					InstallFailed();
			}
			else if (ModeExtension == ModeExtension.Restore)
			{
				Context.ComponentId = GetComponentID(Context);
				AppConfig.LoadComponentSettings(Context);
				new RestoreScript(Context).Run();
			}
			else
				throw new NotImplementedException("Install " + ModeExtension.ToString());
		}
		protected override void Uninstall()
		{
			Context.ComponentId = GetComponentID(Context);
			AppConfig.LoadComponentSettings(Context);
			SetupScript Script = null;
			switch (ModeExtension)
			{
				case ModeExtension.Normal:
					{
						Script = new UninstallScript(Context);
						var Act = new InstallAction(ActionTypes.DeleteShortcuts);
						Act.Description = "Deleting shortcuts...";
						Act.Log = "- Delete shortcuts";
						Act.Name = "Login to SolidCP.url";
						Script.Actions.Add(Act);
					}
					break;
				case ModeExtension.Backup:
					Script = new BackupScript(Context);
					break;
				default:
					throw new NotImplementedException("Uninstall " + ModeExtension.ToString());
			}
			Script.Run();
		}
		protected override void Maintenance()
		{
			Context.ComponentId = GetComponentID(Context);
			AppConfig.LoadComponentSettings(Context);
			SetupScript Script = new MaintenanceScript(Context);
			Script.Actions.Add(new InstallAction(ActionTypes.UpdateEnterpriseServerUrl) { Description = "Updating site settings..." });
			Script.Run();
		}
	}
	public class SchedulerSetup : WiXSetup
	{
		public SchedulerSetup(SetupVariables Ctx, ModeExtension Ext)
			: base(Ctx, Ext)
		{
			Ctx.SysFields = new string[]
			{
				"ApplicationName",
				"ComponentCode",
				"ComponentName",
				"ComponentDescription",
				"Release",
				"Instance",
				"InstallFolder",
				"Installer",
				"InstallerType",
				"InstallerPath",
				"ConnectionString",
				"CryptoKey",
				"ServiceName",
				"ServiceFile"
			};
		}
		public static IWiXSetup Create(IDictionary<string, string> Ctx, SetupActions Action)
		{
			var SetupVars = new SetupVariables();
			FillFromSession(Ctx, SetupVars);
			SetupVars.SetupAction = Action;
			SetupVars.IISVersion = Global.IISVersion;
			AppConfig.LoadConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = GetFullConfigPath(SetupVars) });
			return new SchedulerSetup(SetupVars, GetModeExtension(Ctx));
		}
		protected override void Install()
		{
			if (ModeExtension == ModeExtension.Normal)
			{
				Context.ComponentId = Guid.NewGuid().ToString();
				Context.Instance = String.Empty;
				try
				{
					if (new string[] { Context.DatabaseServer, Context.Database, Context.DatabaseUser, Context.DatabaseUserPassword, Context.CryptoKey }.All(x => string.IsNullOrWhiteSpace(x)))
					{
						var ESVars = new SetupVariables() { ComponentCode = Global.EntServer.ComponentCode, InstallerFolder = Context.InstallerFolder };
						ESVars.ComponentId = WiXSetup.GetComponentID(ESVars);
						AppConfig.LoadComponentSettings(ESVars);
						Context.ConnectionString = ESVars.ConnectionString;
						Context.CryptoKey = ESVars.CryptoKey;
					}
					else
					{
						Context.ConnectionString = Data.DatabaseUtils.BuildConnectionString(Context.DatabaseType,
							Context.DatabaseServer, Context.DatabasePort, Context.Database, Context.Database,
							Context.DatabaseUserPassword, Context.EnterpriseServerPath, Context.EmbedEnterpriseServer);
						//Context.ConnectionString = string.Format(Context.ConnectionString, Context.DatabaseServer, Context.Database, Context.DatabaseUser, Context.DatabaseUserPassword);
					}
					if (string.IsNullOrWhiteSpace(Context.ServiceName) || string.IsNullOrWhiteSpace(Context.ServiceFile))
					{
						Context.ServiceName = Global.Parameters.SchedulerServiceName;
						Context.ServiceFile = Path.Combine(Context.InstallationFolder, Global.Parameters.SchedulerServiceFileName);
					}
					var XmlUp = new Dictionary<string, string[]>();
					XmlUp.Add("configuration/connectionStrings/add[@name='EnterpriseServer']", new string[] { "connectionString", Context.ConnectionString });
					XmlUp.Add("configuration/appSettings/add[@key='SolidCP.CryptoKey']", new string[] { "value", Context.CryptoKey });
					Context.XmlData = XmlUp;
					var Script = new ExpressScript(Context);
					Script.Actions.Add(new InstallAction(ActionTypes.UpdateXml) { SetupVariables = Context, Path = string.Format("{0}.config", Context.ServiceFile) });
					Script.Actions.Add(new InstallAction(ActionTypes.RegisterWindowsService));
					Script.Actions.Add(new InstallAction(ActionTypes.StartWindowsService));
					Script.Actions.Add(new InstallAction(ActionTypes.SaveConfig));
					Script.Run();
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to install the component", ex);
				}
			}
			else if (ModeExtension == ModeExtension.Restore)
			{
				try
				{
					Context.ComponentId = GetComponentID(Context);
					bool SaveConfig = false;
					if (string.IsNullOrWhiteSpace(Context.ComponentId))
					{
						Context.ComponentId = Context.ComponentId = Guid.NewGuid().ToString();
						SaveConfig = true;
					}
					if (string.IsNullOrWhiteSpace(Context.ServiceName) || string.IsNullOrWhiteSpace(Context.ServiceFile))
					{
						Context.ServiceName = Global.Parameters.SchedulerServiceName;
						Context.ServiceFile = Path.Combine(Context.InstallationFolder, Global.Parameters.SchedulerServiceFileName);
					}
					AppConfig.LoadComponentSettings(Context);
					var Script = new ExpressScript(Context);
					Script.Actions.Add(new InstallAction(ActionTypes.RestoreConfig) { SetupVariables = Context, Description = "Restoring xml configuration files..." });
					Script.Actions.Add(new InstallAction(ActionTypes.RegisterWindowsService));
					Script.Actions.Add(new InstallAction(ActionTypes.StartWindowsService));
                    if(SaveConfig)
						Script.Actions.Add(new InstallAction(ActionTypes.SaveConfig));
					Script.Actions.Add(new InstallAction(ActionTypes.UpdateConfig) { Description = "Updating system configuration..." });
					Script.Run();
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to restore the component", ex);
				}
			}
			else
				throw new NotImplementedException("Install " + ModeExtension.ToString());
		}
		protected override void Uninstall()
		{
			const string LogExt = ".InstallLog";
			Context.ComponentId = GetComponentID(Context);
			AppConfig.LoadComponentSettings(Context);
			SetupScript Script = null;
			var ServiceName = AppConfig.GetComponentSettingStringValue(Context.ComponentId, "ServiceName");
			var ServiceFile = AppConfig.GetComponentSettingStringValue(Context.ComponentId, "ServiceFile");
			switch (ModeExtension)
			{
				case ModeExtension.Normal:
					{
						Script = new ExpressScript(Context);
						Script.Actions.Add(new InstallAction(ActionTypes.UnregisterWindowsService)
						{
							Name = ServiceName,
							Path = ServiceFile,
							Description = "Removing Windows service...",
							Log = string.Format("- Remove {0} Windows service", ServiceName)
						});
						Script.Actions.Add(new InstallAction(ActionTypes.UpdateConfig)
						{
							Key = Context.ComponentId,
							Description = "Updating configuration settings...",
							Log = "- Update configuration settings"
						});
						Script.Actions.Add(new InstallAction(ActionTypes.DeleteDirectoryFiles)
						{
							Path = Context.InstallFolder,
							FileFilter = x => x.ToLowerInvariant().EndsWith(LogExt.ToLowerInvariant())
						});
					}
					break;
				case ModeExtension.Backup:
					{
						Script = new ExpressScript(Context);
						Script.Actions.Add(new InstallAction(ActionTypes.Backup) { SetupVariables = Script.Context });
						Script.Actions.Add(new InstallAction(ActionTypes.UnregisterWindowsService)
						{
							Name = ServiceName,
							Path = ServiceFile,
							Description = "Removing Windows service...",
							Log = string.Format("- Remove {0} Windows service", ServiceName)
						});
						Script.Actions.Add(new InstallAction(ActionTypes.DeleteDirectoryFiles)
						{
							Path = Context.InstallFolder,
							FileFilter = x => x.ToLowerInvariant().EndsWith(LogExt.ToLowerInvariant())
						});
					}
					break;
				default:
					throw new NotImplementedException("Uninstall " + ModeExtension.ToString());
			}
			Script.Run();
		}
		protected override void Maintenance()
		{
			throw new NotImplementedException("Maintenance");
		}
	}
	#region WiXActionManagers
	public class WiXServerActionManager : BaseActionManager
	{
		public static readonly List<SolidCP.Setup.Actions.Action> InstallScenario = new List<SolidCP.Setup.Actions.Action>
		{
			new SetCommonDistributiveParamsAction(),
			new SetServerDefaultInstallationSettingsAction(),
			new EnsureServiceAccntSecured(),
			//new CopyFilesAction(),
			new SetServerPasswordAction(),
			new CreateWindowsAccountAction(),
			new ConfigureAspNetTempFolderPermissionsAction(),
			new SetNtfsPermissionsAction(),
			new CreateWebApplicationPoolAction(),
			new CreateWebSiteAction(),
			new SwitchAppPoolAspNetVersion(),
			new SaveComponentConfigSettingsAction()
		};

		public WiXServerActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(WiXServerActionManager_Initialize);
		}

		void WiXServerActionManager_Initialize(object sender, EventArgs e)
		{
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install:
					LoadInstallationScenario();
					break;
				default:
					break;
			}
		}

		protected virtual void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}

	public class WiXPortalActionManager : BaseActionManager
	{
		public static readonly List<SolidCP.Setup.Actions.Action> InstallScenario = new List<SolidCP.Setup.Actions.Action>
		{
			new SetCommonDistributiveParamsAction(),
			new SetWebPortalWebSettingsAction(),
			new EnsureServiceAccntSecured(),
			//new CopyFilesAction(),
			new CopyWebConfigAction(),
			new ConfigureEmbeddedEnterpriseServerAction(),
			new CreateWindowsAccountAction(),
			new ConfigureAspNetTempFolderPermissionsAction(),
			new SetNtfsPermissionsAction(),
			new CreateWebApplicationPoolAction(),
			new CreateWebSiteAction(),
			new SwitchAppPoolAspNetVersion(),
			new UpdateEnterpriseServerUrlAction(),
			new GenerateSessionValidationKeyAction(),
			new SaveComponentConfigSettingsAction(),
			new CreateDesktopShortcutsAction()
		};
		public WiXPortalActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(WiXPortalActionManager_Initialize);
		}
		void WiXPortalActionManager_Initialize(object sender, EventArgs e)
		{
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install:
					LoadInstallationScenario();
					break;
				default:
					break;
			}
		}
		protected virtual void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}
	public class WiXEnterpriseServerActionManager : BaseActionManager
	{
		public static readonly List<SolidCP.Setup.Actions.Action> InstallScenario = new List<SolidCP.Setup.Actions.Action>
		{
			new SetCommonDistributiveParamsAction(),
			new SetEntServerWebSettingsAction(),
			new EnsureServiceAccntSecured(),
			//new CopyFilesAction(),
			new SetEntServerCryptoKeyAction(),
			new CreateWindowsAccountAction(),
			new ConfigureAspNetTempFolderPermissionsAction(),
			new SetNtfsPermissionsAction(),
			new CreateWebApplicationPoolAction(),
			new CreateWebSiteAction(),
			new SwitchAppPoolAspNetVersion(),
			//new CreateDatabaseAction(),
			new CreateDatabaseUserAction(),
			//new ExecuteInstallSqlAction(),
			new UpdateServeradminPasswAction(),
			new SaveAspNetDbConnectionStringAction(),
			new SaveComponentConfigSettingsAction(),
			new SaveEntServerConfigSettingsAction(),
            // new SaveSchedulerServiceConnectionStringAction(),
            // new SaveSchedulerServiceCryptoKeyAction(),
            // new InstallSchedulerServiceAction()
		};
		public WiXEnterpriseServerActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(WiXEnterpriseServerActionManager_Initialize);
		}
		void WiXEnterpriseServerActionManager_Initialize(object sender, EventArgs e)
		{
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install:
					LoadInstallationScenario();
					break;
				default:
					break;
			}
		}
		protected virtual void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}
	#endregion
}
