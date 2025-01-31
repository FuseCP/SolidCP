using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Converters;

namespace SolidCP.UniversalInstaller
{

	public abstract partial class Installer
	{
		public virtual void InstallServerPrerequisites() { }
		public virtual void RemoveServerPrerequisites() { }

		public virtual void SetServerFilePermissions() => SetFilePermissions(ServerFolder);
		public virtual void SetServerFileOwner() => SetFileOwner(ServerFolder, Settings.Server.Username, SolidCP.ToLower());
		public virtual void InstallServer()
		{
			InstallServerPrerequisites();
			CopyServer(StandardInstallFilter);
			SetServerFilePermissions();
			SetServerFileOwner();
			ConfigureServer();
			InstallServerWebsite();
			UpdateSettings();
		}
		public virtual void UpdateServer()
		{
			InstallServerPrerequisites();
			CopyServer(StandardUpdateFilter);
			SetServerFilePermissions();
			SetServerFileOwner();
			UpdateServerConfig();
			ConfigureServer();
			InstallServerWebsite();
			UpdateSettings();
		}
		public virtual void SetupServer()
		{
			ConfigureServer();
			UpdateSettings();
		}
		public virtual void RemoveServer()
		{
			//RemoveServerPrerequisites();
			RemoveServerWebsite();
			RemoveServerFolder();
			UpdateSettings();
		}

		public virtual void InstallServerUser() { }
		public virtual void InstallServerApplicationPool() { }
		public virtual void InstallServerWebsite() { }
		public virtual void RemoveServerWebsite() { }
		public virtual void RemoveServerFolder() {
			Directory.Delete(Path.Combine(InstallWebRootPath, ServerFolder), true);
		}
		public virtual void RemoveServerUser() { }
		public virtual void RemoveServerApplicationPool() { }
		public virtual void ReadServerConfigurationNetFX()
		{
			// Read Server configuration from Server web.config

			Settings.Server = new ServerSettings();

			var confFile = Path.Combine(InstallWebRootPath, ServerFolder, "bin", "web.config");

			if (File.Exists(confFile))
			{
				var webconf = XElement.Load(confFile);
				var configuration = webconf.Element("configuration");

				// server certificate
				var cert = configuration?.Element("system.serviceModel/behaviors/serviceBehaviors/behavior/serviceCredentials/serviceCertificate");
				if (cert != null)
				{
					Settings.Server.CertificateStoreLocation = cert.Attribute("storeLocation")?.Value;
					Settings.Server.CertificateStoreName = cert.Attribute("storeName")?.Value;
					Settings.Server.CertificateFindType = cert.Attribute("X509FindType")?.Value;
					Settings.Server.CertificateFindValue = cert.Attribute("findValue")?.Value;
				}

				Settings.Server.ServerPasswordSHA = Settings.Server.ServerPassword = "";
				// server password
				var password = configuration?.Element("SolidCP.server/security/password");
				if (password != null)
				{
					Settings.Server.ServerPasswordSHA = password.Attribute("value")?.Value;
				}
			}
		}
		public virtual void ReadServerConfiguration() => ReadEnterpriseServerConfigurationNetFX();
		public virtual void UpdateServerConfig() { }

		public virtual void ConfigureServerNetCore(bool standalone = false)
		{
			AppSettings appsettings = null;
			string folder;
			if (!standalone) folder = ServerFolder;
			else folder = WebPortalFolder;

			var appsettingsfile = Path.Combine(InstallWebRootPath, folder, "bin_dotnet", "appsettings.json");
			if (File.Exists(appsettingsfile))
			{
				appsettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(appsettingsfile),
					new VersionConverter(), new StringEnumConverter()) ?? new AppSettings();
			}
			else
			{
				appsettings = new AppSettings();
			}

			appsettings.applicationUrls = Settings.Server.Urls;
			var allowedHosts = (Settings.Server?.Urls ?? "").Split(',', ';')
				.Select(url => new Uri(url.Trim()).Host)
				.ToList();
			if (allowedHosts.Any(host => host == "localhost"))
			{
				allowedHosts.Add("127.0.0.1");
				allowedHosts.Add("::1");
			}
			if (allowedHosts.Any(host => host == "*")) appsettings.AllowedHosts = null;
			else appsettings.AllowedHosts = string.Join(";", allowedHosts.Distinct());

			if (!string.IsNullOrEmpty(Settings.Server.ServerPassword) || !string.IsNullOrEmpty(Settings.Server.ServerPasswordSHA))
			{
				string pwsha;
				if (!string.IsNullOrEmpty(Settings.Server.ServerPassword))
				{
					pwsha = CryptoUtils.ComputeSHAServerPassword(Settings.Server.ServerPassword);
				}
				else
				{
					pwsha = Settings.Server.ServerPasswordSHA;
				}
				appsettings.Server = new AppSettings.ServerSetting() { Password = pwsha };
			}

			if (!string.IsNullOrEmpty(Settings.Server.LetsEncryptCertificateEmail) && !string.IsNullOrEmpty(Settings.Server.LetsEncryptCertificateDomains))
			{
				appsettings.LettuceEncrypt = new AppSettings.LettuceEncryptSetting()
				{
					AcceptTermOfService = true,
					EmailAddress = Settings.Server.LetsEncryptCertificateEmail,
					DomainNames = Settings.Server.LetsEncryptCertificateDomains
						?.Split(',', ';')
						.Select(domain => domain.Trim())
						.ToArray() ?? new string[0]
				};
			}
			else if (!string.IsNullOrEmpty(Settings.Server.CertificateFile) && !string.IsNullOrEmpty(Settings.Server.CertificatePassword))
			{
				// create a local copy of the certificate file
				var certFile = Settings.Server.CertificateFile;
				var certFolder = Path.Combine(InstallWebRootPath, CertificateFolder);
				if (!Directory.Exists(certFolder)) Directory.CreateDirectory(certFolder);
				var shadowFileName = $"{Guid.NewGuid()}.{Path.GetFileName(certFile)}";
				var shadowFile = Path.Combine(certFolder, shadowFileName);
				File.Copy(certFile, shadowFile);
				OSInfo.Unix.GrantUnixPermissions(shadowFile, UnixFileMode.UserRead | UnixFileMode.UserWrite);

				appsettings.Certificate = new AppSettings.CertificateSetting()
				{
					File = shadowFile,
					Password = Settings.Server.CertificatePassword
				};
			}
			else if (!string.IsNullOrEmpty(Settings.Server.CertificateStoreLocation) && !string.IsNullOrEmpty(Settings.Server.CertificateStoreName))
			{
				appsettings.Certificate = new AppSettings.CertificateSetting()
				{
					FindValue = Settings.Server.CertificateFindValue
				};
				Enum.TryParse<StoreLocation>(Settings.Server.CertificateStoreLocation, out appsettings.Certificate.StoreLocation);
				Enum.TryParse<StoreName>(Settings.Server.CertificateStoreName, out appsettings.Certificate.StoreName);
				Enum.TryParse<X509FindType>(Settings.Server.CertificateFindType, out appsettings.Certificate.FindType);
			}

			if (string.IsNullOrEmpty(appsettings.probingPaths)) appsettings.probingPaths = "..\\bin\\netstandard";

			var path = Path.GetDirectoryName(appsettingsfile);
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			File.WriteAllText(appsettingsfile, JsonConvert.SerializeObject(appsettings, Formatting.Indented, new JsonSerializerSettings()
			{
				ContractResolver = new AppSettings.IgnoreAllowedHostsResolver(),
				Converters = new List<JsonConverter>() { new VersionConverter(), new StringEnumConverter() }
			}));

		}

		public virtual void ConfigureServerNetFX()
		{
			var settings = Settings.Server;
			var confFile = Path.Combine(InstallWebRootPath, ServerFolder, "web.config");
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

			// Server password
			var server = configuration.Element("SolidCP.server");
			var security = server?.Element("security");
			var password = security?.Element("password");
			var pwsha = string.IsNullOrEmpty(settings.ServerPassword) ? settings.ServerPasswordSHA : CryptoUtils.ComputeSHAServerPassword(settings.ServerPassword);
			password.Attribute("value").SetValue(pwsha);

			// Swagger Version
			var swaggerwcf = configuration.Element("swaggerwcf");
			var swagsetting = swaggerwcf?.Element("settings");
			var versionInfo = swagsetting?.Elements("setting").FirstOrDefault(e => e.Attribute("name")?.Value == "InfoVersion");
			if (versionInfo != null)
			{
				var version = Settings.Installer.Component.Version;
				versionInfo.Attribute("value").SetValue(version);
			}

			configuration.Save(confFile);
		}

		public virtual void ConfigureServer(bool standalone = false) {
			ConfigureServerNetFX();
			ConfigureServerNetCore();

			InstallLog("Configured Server.");
		}
		public virtual void CopyServer(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
			CopyFiles(ComponentTempPath, websitePath, filter);
		}
		public virtual int InstallServerMaxProgress => 100;
		public virtual int UninstallServerMaxProgress => 100;
		public virtual int SetupServerMaxProgress => 100;
		public virtual int UpdateServerMaxProgress => 100;
	}
}