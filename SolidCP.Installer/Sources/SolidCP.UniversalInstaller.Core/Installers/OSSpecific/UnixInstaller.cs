using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using SolidCP.EnterpriseServer;

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
	public UnixInstaller() : base() { }

	public override void InstallWebsite(string name, string path, CommonSettings settings,
		string group, string dll, string description, string serviceId)
	{
		if (!File.Exists(dll) && !Debugger.IsAttached)
		{
			throw new FileNotFoundException($"The service executable {dll} was not found.");
		}

		AddUnixUser(serviceId, SolidCPUnixGroup);

		var service = new SystemdServiceDescription()
		{
			ServiceId = serviceId,
			Directory = Path.GetDirectoryName(dll),
			Description = description,
			Executable = $"dotnet {dll}",
			DependsOn = new List<string>() { "network-online.target" },
			EnvironmentVariables = new Dictionary<string, string>(),
			Restart = "on-failure",
			RestartSec = "1s",
			StartLimitBurst = "5",
			StartLimitIntervalSec = "500",
			User = settings.Username ?? "",
			Group = group,
			SyslogIdentifier = serviceId
		};
		service.EnvironmentVariables.Add("ASPNETCORE_ENVIRONMENT", "Production");

		InstallService(service);

		OpenFirewall(settings.Urls ?? "");
	}
	public virtual void AddUnixUser(string user, string group)
	{
		Shell.Exec($"useradd --home /home/{user} --gid {group} -m --shell /bin/false {user}");

		InstallLog($"Added System User {user}.");
	}
	public override void RemoveWebsite(string serviceId, string username, string urls)
	{
		var service = ServiceController[serviceId];

		if (service.Info != null)
		{
			service.Stop();
			service.Disable();
			service.Remove();

			InstallLog($"Removed {serviceId} service");

			RemoveFirewallRule(urls);
		}
	}
	public override void RemoveServerWebsite()
	{
		RemoveWebsite(UnixServerServiceId, UnixServerServiceId, Settings.Server.Urls);

		//InstallLog($"Removed {UnixServerServiceId} service & website.");
	}

	public override void RemoveEnterpriseServerWebsite()
	{
		RemoveWebsite(UnixEnterpriseServerServiceId, UnixEnterpriseServerServiceId, Settings.EnterpriseServer.Urls);

		//InstallLog($"Removed {UnixEnterpriseServerServiceId} service & website.");
	}

	public override void RemoveWebPortalWebsite()
	{
		RemoveWebsite(UnixPortalServiceId, UnixPortalServiceId, Settings.WebPortal.Urls);

		//InstallLog($"Removed {UnixPortalServiceId} service & website.");
	}
	public override void OpenFirewall(int port)
	{
		if (Shell.Default.Find("ufw") != null)
		{
			Shell.Default.Exec($"ufw allow {port}/tcp");

			//InstallLog($"Opened firewall on port {port}.");
		}
	}

	public override void RemoveFirewallRule(int port)
	{
		if (Shell.Default.Find("ufw") != null)
		{
			Shell.Default.Exec($"ufw delete allow {port}/tcp");

			//InstallLog($"Removed firewall rule for port {port}.");
		}
	}

	public override void ReadServerConfiguration()
	{
		var appsettingsfile = Path.Combine(InstallWebRootPath, ServerFolder, "bin_dotnet", "appsettings.json");
		if (File.Exists(appsettingsfile))
		{
			var appsettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(appsettingsfile),
				new VersionConverter(), new StringEnumConverter()) ?? new AppSettings();
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
	//public override void ConfigureEnterpriseServerNetFX() { }
	//public override void ConfigureServerNetFX() { }
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

	public override string StandardInstallFilter(string file) => Net8Filter(base.StandardInstallFilter(file));
	public override string StandardUpdateFilter(string file) => Net8Filter(base.StandardUpdateFilter(file));

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
