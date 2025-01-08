using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SolidCP.EnterpriseServer;
using System.Text.RegularExpressions;

namespace SolidCP.UniversalInstaller;

public abstract class UnixInstaller : Installer
{
	public override string InstallExeRootPath { get => base.InstallExeRootPath ?? $"/usr/local/{SolidCP}"; set => base.InstallExeRootPath = value; }
	public override string InstallWebRootPath { get => base.InstallWebRootPath ?? $"/var/www/{SolidCP}"; set => base.InstallWebRootPath = value; }
	public override string WebsiteLogsPath => $"/var/log/{SolidCP}";
	public virtual string UnixServerServiceId => "solidcp-server";
	public virtual string UnixEnterpriseServerServiceId => "solidcp-enterpriseserver";
	public virtual string UnixPortalServiceId => "solidcp-portal";
	public virtual string SolidCPUnixGroup => "solidcp";
	public virtual string CertificateFolder => "Certificates";
	public UnixInstaller() : base() { }

	public void InstallWebsite(string dll, string serviceId, string urls, string user, string group, string description)
	{
		if (!File.Exists(dll) && !Debugger.IsAttached)
		{
			throw new FileNotFoundException($"The service executable {dll} was not found.");
		}

		var service = new SystemdServiceDescription()
		{
			ServiceId = UnixServerServiceId,
			Directory = Path.GetDirectoryName(dll),
			Description = "SolidCP.Server service, the server management service for the SolidCP control panel.",
			Executable = $"dotnet {dll}",
			DependsOn = new List<string>() { "network-online.target" },
			EnvironmentVariables = new Dictionary<string, string>(),
			Restart = "on-failure",
			RestartSec = "1s",
			StartLimitBurst = "5",
			StartLimitIntervalSec = "500",
			User = user,
			Group = group,
			SyslogIdentifier = UnixServerServiceId
		};
		service.EnvironmentVariables.Add("ASPNETCORE_ENVIRONMENT", "Production");

		InstallService(service);

		OpenFirewall(urls);
	}
	public override void InstallServerWebsite()
	{
		var dll = Path.Combine(InstallWebRootPath, ServerFolder, "bin_dotnet", "SolidCP.Server.dll");

		InstallWebsite(dll, UnixServerServiceId, Settings.Server.Urls, "root", SolidCPUnixGroup,
			"SolidCP.Server service, the server management service for the SolidCP control panel.");

		InstallLog($"Installed {UnixServerServiceId} service runnig the Server website.");
	}
	public virtual void AddUnixUser(string user, string group)
	{
		Shell.Exec($"useradd --home /home/{user} --gid {group} -m --shell /bin/false {user}");

		InstallLog($"Added System User {user}.");
	}
	public override void InstallEnterpriseServerWebsite()
	{
		var dll = Path.Combine(InstallWebRootPath, EnterpriseServerFolder, "bin_dotnet", "SolidCP.EnterpriseServer.dll");

		AddUnixUser(UnixEnterpriseServerServiceId, SolidCPUnixGroup);

		InstallWebsite(dll, UnixEnterpriseServerServiceId, Settings.Server.Urls, UnixEnterpriseServerServiceId, SolidCPUnixGroup,
			"SolidCP.Server service, the server management service for the SolidCP control panel.");

		InstallLog($"Installed {UnixEnterpriseServerServiceId} service runnig the Enterprise Server website.");
	}

	public override void InstallWebPortalWebsite()
	{
		var dll = Path.Combine(InstallWebRootPath, WebPortalFolder, "bin_dotnet", "SolidCP.WebPortal.dll");

		AddUnixUser(UnixPortalServiceId, SolidCPUnixGroup);

		InstallWebsite(dll, UnixPortalServiceId, Settings.WebPortal.Urls, UnixPortalServiceId, SolidCPUnixGroup,
			"SolidCP.Server service, the server management service for the SolidCP control panel.");

		InstallLog($"Installed {UnixPortalServiceId} service runnig the WebPortal website.");
	}
	public virtual void RemoveWebsite(string serviceId, string urls)
	{
		var service = ServiceController[serviceId];

		if (service.Info != null)
		{
			service.Stop();
			service.Disable();
			service.Remove();

			RemoveFirewallRule(urls);
		}
	}
	public override void RemoveServerWebsite()
	{
		RemoveWebsite(UnixServerServiceId, Settings.Server.Urls);

		InstallLog($"Removed {UnixServerServiceId} service & website.");
	}

	public override void RemoveEnterpriseServerWebsite()
	{
		RemoveWebsite(UnixEnterpriseServerServiceId, Settings.EnterpriseServer.Urls);

		InstallLog($"Removed {UnixEnterpriseServerServiceId} service & website.");
	}

	public override void RemoveWebPortalWebsite()
	{
		RemoveWebsite(UnixPortalServiceId, Settings.WebPortal.Urls);

		InstallLog($"Removed {UnixPortalServiceId} service & website.");
	}
	public override void OpenFirewall(int port)
	{
		if (Shell.Default.Find("ufw") != null)
		{
			Shell.Default.Exec($"ufw allow {port}/tcp");

			InstallLog($"Opened firewall on port {port}.");
		}
	}

	public override void RemoveFirewallRule(int port)
	{
		if (Shell.Default.Find("ufw") != null)
		{
			Shell.Default.Exec($"ufw delete allow {port}/tcp");

			InstallLog($"Removed firewall rule for port {port}.");
		}
	}

	public override void ReadServerConfiguration()
	{
		var appsettingsfile = Path.Combine(InstallWebRootPath, ServerFolder, "bin_dotnet", "appsettings.json");
		if (File.Exists(appsettingsfile))
		{
			var appsettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(appsettingsfile)) ?? new AppSettings();
			Settings.Server.Urls = appsettings.applicationUrls;
			Settings.Server.ServerPasswordSHA = appsettings.Server?.Password ?? "";
			Settings.Server.ServerPassword = "";
			Settings.Server.LetsEncryptCertificateEmail = appsettings.LettuceEncrypt?.EmailAddress;
			Settings.Server.LetsEncryptCertificateDomains = (appsettings.LettuceEncrypt != null && appsettings.LettuceEncrypt.DomainNames != null) ? string.Join(", ", appsettings.LettuceEncrypt.DomainNames) : "";
			Settings.Server.CertificateFile = appsettings.Certificate?.File;
			Settings.Server.CertificatePassword = appsettings.Certificate?.Password;
			Settings.Server.CertificateStoreLocation = appsettings.Certificate?.StoreLocation.ToString();
			Settings.Server.CertificateStoreName = appsettings.Certificate?.StoreName.ToString();
			Settings.Server.CertificateFindType = appsettings.Certificate?.FindType.ToString();
			Settings.Server.CertificateFindValue = appsettings.Certificate?.FindValue;
		}
	}

	public override void ConfigureServer()
	{
		AppSettings appsettings = null;
            var appsettingsfile = Path.Combine(InstallWebRootPath, ServerFolder, "bin_dotnet", "appsettings.json");
		if (File.Exists(appsettingsfile))
		{
			appsettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(appsettingsfile)) ?? new AppSettings();
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
			string pwsha1;
			if (!string.IsNullOrEmpty(Settings.Server.ServerPassword))
			{
				pwsha1 = CryptoUtils.ComputeSHAServerPassword(Settings.Server.ServerPassword);
			} else
			{
				pwsha1 = Settings.Server.ServerPasswordSHA;
			}
			appsettings.Server = new AppSettings.ServerSetting() { Password = pwsha1 };
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
			ContractResolver = new AppSettings.IgnoreAllowedHostsResolver()
		}));

		InstallLog("Configured Server.");
	}
	public override void InstallServerPrerequisites()
	{
		InstallNet8Runtime();
	}
	public override Func<string, string> UnzipFilter => Net8Filter;
	public override bool IsRunningAsAdmin
	{
		get
		{
			//var uid = Mono.Posix.Syscall.getuid();
			var euid = Mono.Unix.Native.Syscall.geteuid();
			return euid == 0;
		}
	}

	public override bool CheckOSSupported() => CheckSystemdSupported();

	public override bool CheckIISVersionSupported() => false;

	public override bool CheckSystemdSupported() => new SystemdServiceController().IsInstalled;

	public override bool CheckNetVersionSupported() => OSInfo.IsMono || OSInfo.IsCore && int.Parse(Regex.Match(OSInfo.FrameworkDescription, "[0-9]+").Value) >= 8;

	public override void RestartAsAdmin()
	{
		var password = UI.GetRootPassword();
		var assembly = Assembly.GetEntryAssembly()?.Location;
		string arguments = null;
		arguments = Environment.CommandLine;
		Shell shell = null;
		arguments = string.IsNullOrEmpty(arguments) ? assembly : arguments;
		if (OSInfo.IsMono) shell = Shell.ExecScript($"echo {password} | sudo -S mono --debug {arguments}");
		else if (OSInfo.IsCore) shell = Shell.ExecScript($"echo {password} | sudo -S dotnet {arguments}");
		else throw new NotSupportedException();
		Environment.Exit(shell.ExitCode().Result -1);
	}

	public override void ShowLogFile()
	{
		try
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Log.File);
			if (Shell.Find("gedit") != null) Shell.Standard.Exec($"gedit \"{path}\"");
			else UI.ShowLogFile();
		}
		catch { }
	}

	public override string GetUninstallLog(ComponentSettings settings)
	{
		switch (settings.ComponentCode)
		{
			case Global.Server.ComponentCode:
				return
@"- Remove SolidCP Server website
- Delete SolidCP Server folder.
- Remove firewall rule.";
			case Global.EntServer.ComponentCode:
				return
@"- Remove SolidCP EnterpriseServer website.
- Delete SolidCP EnterpriseServer folder.
- Remove firewall rule.";
			case Global.WebPortal.ComponentCode:
				return
@"- Remove SolidCP WebPortal website.
- Delete SolidCP WebPortal folder.
- Remove firewall rule.";
			case Global.WebDavPortal.ComponentCode:
				return
@"- Remove SolidCP EnterpriseServer website.
- Delete SolidCP EnterpriseServer folder.
- Remove firewall rule.";
			case Global.StandaloneServer.ComponentCode:
				return
@"- Remove SolidCP WebPortal website.
- Delete SolidCP WebPortal, EnterpriseServer & Server folder.
- Remove firewall rule.";
			default: throw new NotSupportedException();
		}
	}
}
