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
using SolidCP.Providers.Common;

namespace SolidCP.UniversalInstaller;

public abstract partial class Installer
{
	public virtual string EnterpriseServerSiteId => $"{SolidCP}EnterpriseServer";
	public virtual string UnixEnterpriseServerServiceId => "solidcp-enterpriseserver";
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
		InstallSchedulerService();
		InstallEnterpriseServerWebsite();
	}
	public virtual void UpdateEnterpriseServer()
	{
		ResetEstimatedOutputLines();
		CountUpdateDatabaseStatements();
		InstallEnterpriseServerPrerequisites();
		RemoveSchedulerService();
		DisableEnterpriseServerWebsite();
		CopyEnterpriseServer(StandardUpdateFilter);
		SetEnterpriseServerFilePermissions();
		SetEnterpriseServerFileOwner();
		UpdateEnterpriseServerConfig();
		ConfigureEnterpriseServer();
		UpdateDatabase();
		EnableEnterpriseServerWebsite();
		InstallSchedulerService();
	}
	public void SetupEnterpriseServer()
	{
		ResetEstimatedOutputLines();
		RemoveEnterpriseServerWebsite();
		ConfigureEnterpriseServer();
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
			if (settings.DatabaseWindowsAuthentication)
			{
				settings.DatabaseUser = settings.DatabaseName;
				settings.DatabasePassword = Utils.GetRandomString(16);
				settings.DatabaseWindowsAuthentication = false;
			}
			var user = settings.DatabaseUser;
			var password = settings.DatabasePassword;
			var db = settings.DatabaseName;

			DatabaseUtils.InstallFreshDatabase(connstr, db, user, password, progress => Log.WriteLine("."));

			var cryptor = new Cryptor(settings.CryptoKey);
			DatabaseUtils.SetServerAdminPassword(connstr, db, cryptor.Encrypt(settings.ServerAdminPassword));

			InstallLog("Installed Database");
		})
			.WithRollback(DeleteDatabase);
	}


	public virtual void UpdateDatabase()
	{
		Info("Update Database");
		var settings = Settings.EnterpriseServer;
		var connstr = settings.DbInstallConnectionString;
		InstallLog("Updated Database");
	}
	public virtual void DeleteDatabase()
	{
		Info("Delete Database");
		var settings = Settings.EnterpriseServer;
		var connstr = settings.DbInstallConnectionString;
		DatabaseUtils.DeleteDatabase(connstr, settings.DatabaseName);
		if (string.IsNullOrEmpty(settings.DatabaseUser))
		{
			settings.DatabaseUser = settings.DatabaseName;
		}
		DatabaseUtils.DeleteUser(connstr, settings.DatabaseUser);
		DatabaseUtils.DeleteLogin(connstr, settings.DatabaseUser);
		InstallLog("Deleted Database");
	}
	public virtual void CountInstallDatabaseStatements()
	{
		var settings = Settings.EnterpriseServer;
		var connstr = settings.DbInstallConnectionString;
		if (string.IsNullOrEmpty(settings.DatabaseUser))
		{
			settings.DatabaseUser = DefaultDatabaseUser;
			settings.DatabasePassword = Utils.GetRandomString(32);
		}
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
		var web = Path.Combine(InstallWebRootPath, EnterpriseServerFolder);
		var dll = Path.Combine(web, "bin_dotnet", "SolidCP.EnterpriseServer.dll");
		InstallWebsite(EnterpriseServerSiteId,
			web,
			Settings.EnterpriseServer,
			UnixEnterpriseServerServiceId,
			dll,
			"SolidCP.EnterpriseServer service, the EnterpriseServer for the SolidCP control panel.",
			UnixEnterpriseServerServiceId);
	}

	public virtual void EnableEnterpriseServerWebsite() { }
	public virtual void DisableEnterpriseServerWebsite() { }
	public virtual void InstallSchedulerService() { }
	public virtual void RemoveSchedulerService() { }
	public virtual void RemoveEnterpriseServerWebsite() {
		RemoveWebsite(EnterpriseServerSiteId, Settings.EnterpriseServer);
	}
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
			Settings.EnterpriseServer.CertificateFindType = cert.Attribute("x509FindType")?.Value;
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

	public virtual void ConfigureEnterpriseServerNetFX(bool webPortalEmbedded = false)
	{
		var settings = Settings.EnterpriseServer;

		var confFile = webPortalEmbedded ? Path.Combine(InstallWebRootPath, WebPortalFolder, "web.config") :
			Path.Combine(InstallWebRootPath, EnterpriseServerFolder, "web.config");

		if (!File.Exists(confFile)) return;

		var configuration = XElement.Load(confFile);

		// server certificate
		if (!webPortalEmbedded) ConfigureCertificateNetFX(settings, configuration);

		var appSettings = configuration.Element("appSettings");

		if (webPortalEmbedded)
		{
			// read CryptoKey
			var esConfFile = Path.GetFullPath(Path.Combine(InstallWebRootPath, WebPortalFolder, Settings.WebPortal.EnterpriseServerPath, "web.config"));
			if (File.Exists(esConfFile))
			{
				var esConf = XElement.Load(esConfFile);
				var esAppSettings = esConf.Element("appSettings");


				var esCryptoKey = esAppSettings?.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "SolidCP.CryptoKey");
				if (esCryptoKey != null)
				{
					settings.CryptoKey = esCryptoKey.Attribute("value")?.Value;
				}

				foreach (var esSetting in esAppSettings.Elements("add"))
				{
					var key = esSetting.Attribute("key")?.Value;
					var setting = appSettings.Elements("add").FirstOrDefault(s => s.Attribute("key")?.Value == key);
					if (setting == null)
					{
						appSettings.Add(esSetting);
					}
					else
					{
						setting.Attribute("value").SetValue(esSetting.Attribute("value")?.Value);
					}
				}

				var esConStrings = esConf.Element("connectionStrings");
				if (esConStrings != null)
				{
					var esConString = esConStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
					var esCstring = esConString?.Attribute("value")?.Value;
					if (esCstring != null)
					{
						var csb = new ConnectionStringBuilder(esCstring);
						if ((csb["DbType"] as string)?.Contains("Sqlite") == true)
						{
							if (csb["Data Source"] != null)
							{
								csb["Data Source"] = Path.Combine(Settings.WebPortal.EnterpriseServerPath, (string)csb["Data Source"]);
							}
							csb["Data Source"] = Path.Combine(Settings.WebPortal.EnterpriseServerPath, (string)csb["Data Source"]);
							esConString.Attribute("value").SetValue(csb.ConnectionString);
						}
					}
				}
				var conStrings = configuration.Element("connectionStrings");
				conStrings.ReplaceWith(esConStrings);

				var esSwagger = esConf.Element("swaggerwcf");
				var swagger = configuration.Element("swaggerwcf");
				swagger.ReplaceWith(esSwagger);
			}
		}

		// CryptoKey
		if (string.IsNullOrEmpty(settings.CryptoKey))
		{
			// generate random crypto key
			settings.CryptoKey = CryptoUtils.GetRandomString(20);
		}

		var cryptoKey = appSettings.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "SolidCP.CryptoKey");
		if (cryptoKey == null)
		{
			cryptoKey = new XElement("add", new XAttribute("key", "SolidCP.CryptoKey"), new XAttribute("value", settings.CryptoKey));
			appSettings.Add(cryptoKey);
		}
		else
		{
			cryptoKey.Attribute("value").SetValue(settings.CryptoKey);
		}

		if (!webPortalEmbedded)
		{
			// Connection String
			var connectionStrings = configuration.Element("connectionStrings");
			var cstring = connectionStrings.Elements("add").FirstOrDefault(e => e.Attribute("name")?.Value == "EnterpriseServer");
			var connectionString = DatabaseUtils.BuildConnectionString(settings.DatabaseType, settings.DatabaseServer,
				settings.DatabasePort, settings.DatabaseName, settings.DatabaseUser, settings.DatabasePassword, Settings.WebPortal.EnterpriseServerPath, webPortalEmbedded && Settings.WebPortal.EmbedEnterpriseServer);
			cstring.Attribute("connectionString").SetValue(connectionString);

			// Swagger Version
			var swaggerwcf = configuration.Element("swaggerwcf");
			var swagsetting = swaggerwcf?.Element("settings");
			var versionInfo = swagsetting?.Elements("setting").FirstOrDefault(e => e.Attribute("name")?.Value == "InfoVersion");
			if (versionInfo != null)
			{
				versionInfo.Attribute("value").SetValue(Settings.EnterpriseServer.Version.ToString());
			}
		}

		configuration.Save(confFile);
	}
	public virtual void ConfigureEnterpriseServerNetCore() => ConfigureAppsettings(Settings.EnterpriseServer);
	public virtual void ConfigureEnterpriseServer()
	{
		ConfigureEnterpriseServerNetCore();
		ConfigureEnterpriseServerNetFX();
		InstallLog("Configured Enterprise Server.");
	}
	public virtual void CopyEnterpriseServer(Func<string, string> filter = null)
	{
		filter ??= SetupFilter;
		var websitePath = Path.Combine(InstallWebRootPath, EnterpriseServerFolder);
		CopyFiles(ComponentTempPath, websitePath, filter);
	}
}