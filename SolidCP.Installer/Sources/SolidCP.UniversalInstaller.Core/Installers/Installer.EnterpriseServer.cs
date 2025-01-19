using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.EnterpriseServer.Data;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml.Linq;

namespace SolidCP.UniversalInstaller
{
	public abstract partial class Installer
	{
		public virtual void InstallEnterpriseServerPrerequisites() { }
		public virtual void RemoveEnterpriseServerPrerequisites() { }
		public virtual void SetEnterpriseServerFilePermissions() => SetFilePermissions(EnterpriseServerFolder);
		public virtual void SetEnterpriseServerFileOwner() => SetFileOwner(EnterpriseServerFolder, Settings.EnterpriseServer.Username, SolidCP.ToLower());
		public virtual void InstallEnterpriseServer()
		{
			ResetEstimatedOutputLines();
			CountInstallDatabaseStatements();
			InstallEnterpriseServerPrerequisites();
			CopyEnterpriseServer();
			SetEnterpriseServerFilePermissions();
			SetEnterpriseServerFileOwner();
			InstallDatabase();
			InstallEnterpriseServerWebsite();
			ConfigureEnterpriseServer();
		}
		public virtual void UpdateEnterpriseServer()
		{
			ResetEstimatedOutputLines();
			CountUpdateDatabaseStatements();
			InstallEnterpriseServerPrerequisites();
			CopyEnterpriseServer(ConfigAndSetupFilter);
			SetEnterpriseServerFilePermissions();
			SetEnterpriseServerFileOwner();
			UpdateEnterpriseServerConfig();
			ConfigureEnterpriseServer();
			UpdateDatabase();
			InstallEnterpriseServerWebsite();
		}
		public virtual void UpdateEnterpriseServerConfig() { }

		public virtual string DefaultDatabaseUser => SolidCP;
		public virtual void InstallDatabase()
		{
			Info("Install Database...");
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;
			if (string.IsNullOrEmpty(connstr) ||
				!DatabaseUtils.CheckSqlConnection(connstr)) throw new DataException("Unable to connect to database.");
			if (string.IsNullOrEmpty(settings.DatabaseUser))
			{
				settings.DatabaseUser = DefaultDatabaseUser;
				settings.DatabasePassword = Utils.GetRandomString(32);
			}
			var user = settings.DatabaseUser;
			var password = settings.DatabasePassword;
			var db = settings.DatabaseName;

			DatabaseUtils.InstallFreshDatabase(connstr, db, user, password, progress => Log.WriteLine("."));
			InstallLog("Installed Database");
		}


		public virtual void UpdateDatabase() {
			Info("Update Database");
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;
			InstallLog("Updated Database");
		}
		public virtual void DeleteDatabase() {
			Info("Delete Database");
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;
			DatabaseUtils.DeleteDatabase(connstr, settings.DatabaseName);
			InstallLog("Deleted Database");
		}
		public virtual void CountInstallDatabaseStatements()
		{
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;
			var user = settings.DatabaseUser;
			var password = settings.DatabasePassword;
			var db = settings.DatabaseName;
			DatabaseUtils.InstallFreshDatabase(connstr, db, user, password, null,
				count => DatabaseStatements = count, null, "", true);
		}
		public virtual void CountUpdateDatabaseStatements()
		{
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;
			var user = settings.DatabaseUser;
			var password = settings.DatabasePassword;
			var db = settings.DatabaseName;
			DatabaseUtils.UpdateDatabase(connstr, db, user, password, null,
				count => DatabaseStatements = count, null, "", true);
		}
		public virtual void ResetEstimatedOutputLines(Func<int> calculateEstimatedOutputLines = null)
		{
			DatabaseStatements = 0;
			estimatedOutputLines = null;
			CalculateEstimateOutputLines = calculateEstimatedOutputLines;
		}
		public virtual void InstallEnterpriseServerWebsite()
		{
			InstallWebsite($"{SolidCP}EnterpriseServer",
				Path.Combine(InstallWebRootPath, EnterpriseServerFolder),
				Settings.EnterpriseServer.Urls ?? "",
				"", "");
		}
		public virtual void RemoveEnterpriseServerWebsite() { }
		public virtual void RemoveEnterpriseServerFolder()
		{
			Directory.Delete(Path.Combine(InstallWebRootPath, EnterpriseServerFolder), true);
		}

		public virtual void RemoveEnterpriseServer()
		{
			RemoveEnterpriseServerWebsite();
			RemoveEnterpriseServerFolder();
			DeleteDatabase();
		}
		public virtual void ReadEnterpriseServerConfiguration() { }

		public virtual void ConfigureEnterpriseServer() { }

		public virtual void CopyEnterpriseServer(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, EnterpriseServerFolder);
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}
		public virtual int InstallEnterpriseServerMaxProgress => 100;
		public virtual int UninstallEnterpriseServerMaxProgress => 100;
		public virtual int SetupEnterpriseServerMaxProgress => 100;
		public virtual int UpdateEnterpriseServerMaxProgress => 100;
	}
}