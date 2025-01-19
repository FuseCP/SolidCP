using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.UniversalInstaller
{
	public class InstallerSettings
	{
		public ServerSettings Server { get; set; }
		public EnterpriseServerSettings EnterpriseServer { get; set; }
		public WebPortalSettings WebPortal { get; set; }
		public StandaloneSettings Standalone { get; set; }
		public WebDavPortalSettings WebDavPortal { get; set; } 
		public InstallerSpecificSettings Installer { get; set; }
	}

	public class ComponentSettings
	{
		private string installFolder = null;
		public string InstallFolder
		{
			get => installFolder ??= Path.Combine(OSInfo.IsWindows ?
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) :
				Installer.Current.UnixAppRootPath, Installer.Current.SolidCP);
			set => installFolder = value;
		}

		public virtual string ComponentCode => null;
		public virtual string ComponentName => null;		
	}

	public class CommonSettings: ComponentSettings
	{
		public string Urls { get; set; }
		public string WebSiteIp { get; set; }
		public string WebSiteDomain { get; set; }
		public int WebSitePort { get; set; }
		public bool UpdateWebSite { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool UseActiveDirectory { get; set; }
		public string CertificateStoreName { get; set; }
		public string CertificateStoreLocation { get; set; }
		public string CertificateFindValue { get; set; }
		public string CertificateFindType { get; set; }
		public string CertificateFile { get; set; }
		public string CertificatePassword { get; set; }
		public string LetsEncryptCertificateDomains { get; set; }
		public string LetsEncryptCertificateEmail { get; set; }
		public bool ConfigureCertificateManually { get; set; }
	}
	public class ServerSettings: CommonSettings
	{
		public ServerSettings()
		{
			Urls = "http://localhost:9003";
			WebSitePort = 9003;
		}
		public string ServerPassword { get; set; }
		public string ServerPasswordSHA { get; set; }
		public bool UpdateServerPassword { get; set; }
		public override string ComponentCode => Global.Server.ComponentCode;
		public override string ComponentName => Global.Server.ComponentName;
	}

	public class EnterpriseServerSettings: CommonSettings
	{
		public EnterpriseServerSettings()
		{
			Urls = "http://localhost:9002";
			WebSitePort = 9002;
		}
		public DbType DatabaseType { get; set; } = DbType.Unknown;
		public string DatabaseServer { get; set; } = "localhost";
		public int DatabasePort { get; set; }
		public string DatabaseUser { get; set; }
		public string DatabasePassword { get; set; }
		public string DatabaseName { get; set; }
		public string DbInstallConnectionString { get; set; }
		public string ServerAdminPassword { get; set; }
		public bool UpdateServerAdminPassword { get; set; }
		public bool TrustDatabaseServerCertificate { get; set; } = false;
		public bool WindowsAuthentication { get; set; } = false;
		public string CryptoKey { get; set; }
		public override string ComponentCode => Global.EntServer.ComponentCode;
		public override string ComponentName => Global.EntServer.ComponentName;
	}

	public class WebPortalSettings: CommonSettings
	{
		public WebPortalSettings()
		{
			Urls = "http://localhost:9001";
			WebSitePort = 9001;
		}
		public string EnterpriseServerUrl { get; set; } = "assembly://SolidCP.EnterpriseServer";
		public string EnterpriseServerPath { get; set; } = "..\\EnterpriseServer";
		public bool EmbedEnterpriseServer { get; set; } = true;
		public bool ExposeEnterpriseServerWebServices { get; set; } = true;
		public override string ComponentCode => Global.WebPortal.ComponentCode;
		public override string ComponentName => Global.WebPortal.ComponentName;
	}

	public class WebDavPortalSettings: CommonSettings
	{
		public WebDavPortalSettings()
		{
			Urls = "http://localhost:9004";
			WebSitePort = 9004;
		}
		public override string ComponentCode => Global.WebDavPortal.ComponentCode;
		public override string ComponentName => Global.WebDavPortal.ComponentName;
	}
	public class StandaloneSettings: ComponentSettings
	{
		public override string ComponentCode => Global.StandaloneServer.ComponentCode;
		public override string ComponentName => Global.StandaloneServer.ComponentName;
	}
	public class ProxySettings
	{
		public string Address { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

	}

	public class InstallerSpecificSettings
	{
		public List<ComponentInfo> InstalledComponents { get; set; } = new List<ComponentInfo>();
		public string ComponentSettingsXml { get; set; }
		public ComponentInfo Component { get; set; }
		public SetupActions Action { get; set; }
		public string TempPath { get; set; }
		public int Files { get; set; } = 0;
		public bool IsUnattended { get; set; }

		Version? installerVersion = null;
		public Version Version {
			get => installerVersion ??= Assembly.GetEntryAssembly().GetName().Version;
			set => installerVersion = value;
		}
		public string WebServiceUrl { get; set; }
		public string GitHubUrl { get; set; }
		public bool CheckForUpdate { get; set; }
		public ProxySettings Proxy { get; set; }
		public string UI
		{
			get => UniversalInstaller.UI.Current.GetType().Name;
			set => UniversalInstaller.UI.SetCurrent(value);
		}
	}
}
