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
			InstallEnterpriseServerPrerequisites();
			ReadEnterpriseServerConfiguration();
			CopyEnterpriseServer();
			SetEnterpriseServerFilePermissions();
			SetEnterpriseServerFileOwner();
			InstallDatabase();
			InstallEnterpriseServerWebsite();
			ConfigureEnterpriseServer();
		}
		public virtual void UpdateEnterpriseServer()
		{
			InstallEnterpriseServerPrerequisites();
			ReadEnterpriseServerConfiguration();
			CopyEnterpriseServer(ConfigAndSetupFilter);
			SetEnterpriseServerFilePermissions();
			SetEnterpriseServerFileOwner();
			UpdateEnterpriseServerConfig();
			ConfigureEnterpriseServer();
			InstallEnterpriseServerWebsite();
		}
		public virtual void UpdateEnterpriseServerConfig() { }
		public virtual void InstallDatabase()
		{
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;
			if (string.IsNullOrEmpty(connstr) ||
				!DatabaseUtils.CheckSqlConnection(connstr)) throw new DataException("Unable to connect to database.");
			if (string.IsNullOrEmpty(settings.DatabaseUser))
			{
				settings.DatabaseUser = "SolidCP";
				settings.DatabasePassword = Utils.GetRandomString(32);
			}
			var user = settings.DatabaseUser;
			var password = settings.DatabasePassword;
			var db = settings.DatabaseName;

			DatabaseUtils.InstallFreshDatabase(connstr, db, user, password, progress => Log.WriteLine("."));
		}
		public virtual void UpdateDatabase() {
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;

		}
		public virtual void DeleteDatabase() {
			var settings = Settings.EnterpriseServer;
			var connstr = settings.DbInstallConnectionString;
			DatabaseUtils.DeleteDatabase(connstr, settings.DatabaseName);
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