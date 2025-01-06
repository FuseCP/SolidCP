using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.Http;
using Newtonsoft.Json.Bson;
using SolidCP.Providers.Web;
using SolidCP.Providers;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Security.Policy;
using System.Security.Principal;

namespace SolidCP.UniversalInstaller
{
	public class WindowsInstaller : Installer
	{
		const bool Net8RuntimeNeededOnWindows = false;

		public override string InstallExeRootPath
		{
			get => base.InstallExeRootPath ??
				(base.InstallExeRootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), SolidCP));
			set => base.InstallExeRootPath = value;
		}
		public override string InstallWebRootPath { get => base.InstallWebRootPath ?? InstallExeRootPath; set => base.InstallWebRootPath = value; }
		public override string WebsiteLogsPath => InstallExeRootPath ?? "";

		WinGet WinGet => (WinGet)((IWindowsOperatingSystem)OSInfo.Current).WinGet;
		public override Func<string, string> UnzipFilter => Net48Filter;

		public override void InstallNet8Runtime()
		{
			if (Net8RuntimeNeededOnWindows)
			{
				if (CheckNet8RuntimeInstalled()) return;

				var ver = OSInfo.WindowsVersion;
				if (!(OSInfo.IsWindowsServer && ver >= WindowsVersion.WindowsServer2012 ||
					!OSInfo.IsWindowsServer && ver >= WindowsVersion.Windows10))
					throw new PlatformNotSupportedException("NET 8 is not supported on this OS.");

				WinGet.Install("Microsoft.DotNet.AspNetCore.8;Microsoft.DotNet.Runtime.8");

				InstallLog("Installed .NET 8 Runtime.");

				ResetHasDotnet();
			}
		}

		public override void RemoveNet8AspRuntime()
		{
			WinGet.Remove("Microsoft.DotNet.AspNetCore.8");

			ResetHasDotnet();
		}
		public override void RemoveNet8NetRuntime()
		{
			WinGet.Remove("Microsoft.DotNet.Runtime.8");

			ResetHasDotnet();
		}

		public void InstallNet48()
		{
			if (!OSInfo.IsNet48)
			{

				Log.WriteLine("Installing NET Framework 4.8");

				var file = DownloadFile("https://download.visualstudio.microsoft.com/ndp48-web.exe");
				if (file != null)
				{
					Shell.Exec($"\"{file}\" /q");
				}

				InstallLog("Installed .NET Framework 4.8.");
			}
		}

		public override void InstallServerPrerequisites()
		{			
			InstallNet8Runtime(); 
			InstallNet48();
		}

		public override void InstallServerWebsite()
		{
			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
			InstallWebsite($"{SolidCP}Server", websitePath,
				Settings.Server.Urls ?? "",
				Settings.Server.Username ?? $"{SolidCP}Server",
				Settings.Server.Password ?? "");

			InstallLog("Installed Server website, listening on the url(s):" +
				$"{string.Join(NewLine, Settings.Server.Urls.Split(',', ';')
					.Select(url => "  " + url))}");
		}

		public override void ReadServerConfiguration()
		{
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

		public override void ConfigureServer()
		{
			var settings = Settings.Server;
			var confFile = Path.Combine(InstallWebRootPath, ServerFolder, "bin", "web.config");
			var webconf = XElement.Load(confFile);
			var configuration = webconf.Element("configuration");

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
			var pwsha1 = string.IsNullOrEmpty(settings.ServerPassword) ? settings.ServerPasswordSHA : CryptoUtils.ComputeSHAServerPassword(settings.ServerPassword);
			password.Attribute("value").SetValue(pwsha1);

			// Swagger Version
			var swaggerwcf = configuration.Element("swaggerWcf");
			var swagsetting = swaggerwcf?.Element("settings");
			var versionInfo = swagsetting?.Elements("setting").FirstOrDefault(e => e.Attribute("name")?.Value == "InfoVersion");
			if (versionInfo != null)
			{
				var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyVersionAttribute>()?.Version;
				versionInfo.Attribute("value").SetValue(version);
			}

			webconf.Save(confFile);

			InstallLog("Configured Server.");
		}

		public override void ReadEnterpriseServerConfiguration()
		{
			Settings.EnterpriseServer = new EnterpriseServerSettings();

			var confFile = Path.Combine(InstallWebRootPath, EnterpriseServerFolder, "bin", "web.config");
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
			bool windowsAuthentication;
			ParseConnectionString(cstring?.Attribute("value")?.Value, out server, out user, out password, out windowsAuthentication);

			Settings.EnterpriseServer.DatabaseServer = server;
			Settings.EnterpriseServer.DatabaseUser = user;
			Settings.EnterpriseServer.DatabasePassword = password;
			Settings.EnterpriseServer.WindowsAuthentication = windowsAuthentication;

			// CryptoKey
			var cryptoKey = configuration?.Elements("appSettings/add").FirstOrDefault(e => e.Attribute("key")?.Value == "CryptoKey");
			Settings.EnterpriseServer.CryptoKey = cryptoKey?.Attribute("value")?.Value;
		}

		public override void ConfigureEnterpriseServer()
		{
			var settings = Settings.EnterpriseServer;
			var confFile = Path.Combine(InstallWebRootPath, EnterpriseServerFolder, "bin", "web.config");
			var webconf = XElement.Load(confFile);
			var configuration = webconf.Element("configuration");

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
			var authentication = settings.WindowsAuthentication ? "Integrated Security=true" : $"User ID={settings.DatabaseUser};Password={settings.DatabasePassword}";
			cstring.Attribute("connectionString").SetValue($"Server={settings.DatabaseServer};Database=SolidCP;{authentication}");

			// Swagger Version
			var swaggerwcf = configuration.Element("swaggerWcf");
			var swagsetting = swaggerwcf?.Element("settings");
			var versionInfo = swagsetting?.Elements("setting").FirstOrDefault(e => e.Attribute("name")?.Value == "InfoVersion");
			if (versionInfo != null)
			{
				var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyVersionAttribute>()?.Version;
				versionInfo.Attribute("value").SetValue(version);
			}

			webconf.Save(confFile);

			InstallLog("Configured Enterprise Server.");
		}

		public override void ReadWebPortalConfiguration()
		{
			Settings.WebPortal = new WebPortalSettings();

			var confFile = Path.Combine(InstallWebRootPath, WebPortalFolder, "App_Data", "SiteSettings.config");
			var conf = XElement.Load(confFile);
			var enterpriseServer = conf.Element("SiteSettings/EnterpriseServer");
			Settings.WebPortal.EnterpriseServerUrl = enterpriseServer.Value;
		}

		public override void ConfigureWebPortal()
		{
			var settings = Settings.WebPortal;
			var confFile = Path.Combine(InstallWebRootPath, WebPortalFolder, "App_Data", "SiteSettings.config");
			var conf = XElement.Load(confFile);
			var enterpriseServer = conf.Element("SiteSettings/EnterpriseServer");
			enterpriseServer.Value = settings.EmbedEnterpriseServer ? "assembly://SolidCP.EnterpriseServer" : settings.EnterpriseServerUrl;
			conf.Save(confFile);

			InstallLog("Configured Web Portal.");
		}

		public override bool IsRunningAsAdmin
			=> new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

		public override void ShowLogFile()
		{
			try
			{
				var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Log.File);
				Shell.Standard.Exec($"notepad.exe \"{path}\"");
			}
			catch { }
		}

		public override bool CheckOSSupported() => OSInfo.WindowsVersion >= WindowsVersion.WindowsServer2003;

		public override bool CheckIISVersionSupported() => CheckOSSupported();

		public override bool CheckSystemdSupported() => false;

		public override bool CheckNetVersionSupported() => OSInfo.IsNet48 || OSInfo.IsCore && int.Parse(Regex.Match(OSInfo.FrameworkDescription, "[0-9]+").Value) >= 8;

		public override void RestartAsAdmin()
		{
			if (RunAsAdmin)
			{
				var currentp = Process.GetCurrentProcess();
				ProcessStartInfo procInfo = new ProcessStartInfo();
				procInfo.UseShellExecute = true;
				var assemblyFile = Assembly.GetEntryAssembly().Location;
				if (OSInfo.IsMono) procInfo.FileName = "mono";
				else if (assemblyFile.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) procInfo.FileName = assemblyFile;
				else if (OSInfo.IsCore) procInfo.FileName = "dotnet";
				procInfo.WorkingDirectory = Environment.CurrentDirectory;
				procInfo.Arguments = currentp.StartInfo.Arguments;
				procInfo.Verb = "runas";
				try
				{
					var p = Process.Start(procInfo);
					p.WaitForExit();
					Environment.Exit(p.ExitCode);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
					Console.Read();
					Environment.Exit(-1);
				}
			}
		}
	}
}