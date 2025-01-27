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
			CopyEnterpriseServer(StandardInstallFilter);
			SetEnterpriseServerFilePermissions();
			SetEnterpriseServerFileOwner();
			InstallDatabase();
			ConfigureEnterpriseServer();
			InstallEnterpriseServerWebsite();
		}
		public virtual void UpdateEnterpriseServer()
		{
			ResetEstimatedOutputLines();
			CountUpdateDatabaseStatements();
			InstallEnterpriseServerPrerequisites();
			CopyEnterpriseServer(StandardUpdateFilter);
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
			Transaction(() =>
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
			})
				.WithRollback(DeleteDatabase);
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
		public virtual void ReadEnterpriseServerConfiguration() => ReadEnterpriseServerConfigurationNetFX();

		public virtual void ReadEnterpriseServerConfigurationNetFX()
		{

			var confFile = Path.Combine(InstallWebRootPath, EnterpriseServerFolder, "bin", "web.config");

			if (!File.Exists(confFile)) return;

			Settings.EnterpriseServer = new EnterpriseServerSettings();

			var webconf = XElement.Load(confFile);
			var configuration = webconf.Element("configuration");

			// server certificate
			var cert = configuration?.Element("system.serviceModel/behaviors/serviceBehaviors/behavior/serviceCredentials/serviceCertificate");
			if (cert != null)
			{
				Settings.EnterpriseServer.CertificateStoreLocation = cert.Attribute("storeLocation")?.Value;
				Settings.EnterpriseServer.CertificateStoreName = cert.Attribute("storeName")?.Value;
				Settings.EnterpriseServer.CertificateFindType = cert.Attribute("X509FindType")?.Value;
				Settings.EnterpriseServer.CertificateFindValue = cert.Attribute("findValue")?.Value;
			}

			// connection string
			var cstring = configuration?.Element("connectionStrings").Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");

			string server, user, password;
			bool windowsAuthentication, trustCertificate;
			EnterpriseServer.Data.DbType dbtype;
			ParseConnectionString(cstring?.Attribute("value")?.Value, out dbtype, out server, out user, out password, out windowsAuthentication,
				out trustCertificate);

			Settings.EnterpriseServer.DatabaseType = dbtype;
			Settings.EnterpriseServer.DatabaseServer = server;
			Settings.EnterpriseServer.DatabaseUser = user;
			Settings.EnterpriseServer.DatabasePassword = password;
			Settings.EnterpriseServer.DatabaseWindowsAuthentication = windowsAuthentication;
			Settings.EnterpriseServer.DatabaseTrustServerCertificate = trustCertificate;

			// CryptoKey
			var cryptoKey = configuration?.Elements("appSettings/add").FirstOrDefault(e => e.Attribute("key")?.Value == "CryptoKey");
			Settings.EnterpriseServer.CryptoKey = cryptoKey?.Attribute("value")?.Value;
		}

		public virtual void ConfigureEnterpriseServerNetFX()
		{
			var settings = Settings.EnterpriseServer;
			var confFile = Path.Combine(InstallWebRootPath, EnterpriseServerFolder, "web.config");

			if (!File.Exists(confFile)) return;
			
			var configuration = XElement.Load(confFile);

			// server certificate
			var serviceModel = configuration.Element("system.serviceModel");
			if (serviceModel == null)
			{
				serviceModel = new XElement("system.serviceModel");
				configuration.Add(serviceModel);
			}
			var behaviors = serviceModel.Element("behaviors");
			if (behaviors == null)
			{
				behaviors = new XElement("behaviors");
				serviceModel.Add(behaviors);
			}
			var serviceBehaiors = behaviors.Element("serviceBehaviors");
			if (serviceBehaiors == null)
			{
				serviceBehaiors = new XElement("serviceBehaviors");
				behaviors.Add(serviceBehaiors);
			}
			var behavior = serviceBehaiors.Element("behavior");
			if (behavior == null)
			{
				behavior = new XElement("behavior");
				serviceBehaiors.Add(behavior);
			}
			var serviceCredentials = behavior.Element("serviceCredentials");
			if (serviceCredentials == null)
			{
				serviceCredentials = new XElement("serviceCredentials");
				behavior.Add(serviceCredentials);
			}
			var cert = serviceCredentials.Element("serviceCertificate");
			if (cert != null) cert.Remove();
			cert = new XElement("serviceCertificate", new XAttribute("storeName", settings.CertificateStoreName),
				new XAttribute("storeLocation", settings.CertificateStoreLocation),
				new XAttribute("X509FindType", settings.CertificateFindType),
				new XAttribute("findValue", settings.CertificateFindValue));
			serviceCredentials.Add(cert);

			// CryptoKey
			if (string.IsNullOrEmpty(settings.CryptoKey))
			{
				// generate random crypto key
				settings.CryptoKey = CryptoUtils.GetRandomString(20);
			}
			var appSettings = configuration.Element("appSettings");
			var cryptoKey = appSettings.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "CryptoKey");
			if (cryptoKey == null)
			{
				cryptoKey = new XElement("add", new XAttribute("key", "CryptoKey"), new XAttribute("value", settings.CryptoKey));
				appSettings.Add(cryptoKey);
			}
			else
			{
				cryptoKey.Attribute("CryptoKey").SetValue(settings.CryptoKey);
			}

			// Connection String
			var connectionStrings = configuration.Element("connectionStrings");
			var cstring = connectionStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
			var authentication = settings.DatabaseWindowsAuthentication ? "Integrated Security=true" : $"User ID={settings.DatabaseUser};Password={settings.DatabasePassword}";
			cstring.Attribute("connectionString").SetValue($"Server={settings.DatabaseServer};Database=SolidCP;{authentication}");

			// Swagger Version
			var swaggerwcf = configuration.Element("swaggerWcf");
			var swagsetting = swaggerwcf?.Element("settings");
			var versionInfo = swagsetting?.Elements("setting").FirstOrDefault(e => e.Attribute("name")?.Value == "InfoVersion");
			if (versionInfo != null)
			{
				versionInfo.Attribute("value").SetValue(Settings.EnterpriseServer.Version.ToString());
			}

			configuration.Save(confFile);

			InstallLog("Configured Enterprise Server.");
		}
		public virtual void ConfigureEnterpriseServer(bool standalone = false) {
			ConfigureEnterpriseServerNetFX();
		}
		public virtual void CopyEnterpriseServer(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, EnterpriseServerFolder);
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}
	}
}