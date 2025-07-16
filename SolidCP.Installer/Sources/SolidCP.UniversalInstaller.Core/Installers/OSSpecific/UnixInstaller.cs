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
using AspNetCoreSharedServer;

namespace SolidCP.UniversalInstaller;

public abstract class UnixInstaller : Installer
{
	public override string InstallExeRootPath { get => base.InstallExeRootPath ?? $"/usr/share/{SolidCP.ToLower()}"; set => base.InstallExeRootPath = value; }
	public override string InstallWebRootPath { get => base.InstallWebRootPath ?? $"/var/www/{SolidCP.ToLower()}"; set => base.InstallWebRootPath = value; }
	public override string WebsiteLogsPath => $"/var/log/{SolidCP.ToLower()}";
	public override string UnixServerServiceId => "solidcp-server";
	public override string UnixEnterpriseServerServiceId => "solidcp-enterpriseserver";
	public override string UnixPortalServiceId => "solidcp-portal";
	public override string SolidCPGroup => SolidCP.ToLower();
	public override string SolidCPUnixGroup => SolidCPGroup;
	public UnixInstaller() : base() { }
	bool installedAspNetCoreSharedServer = false;
	public void InstallAspNetCoreSharedServer()
	{
		Log.WriteStart("Install AspNetCoreSharedServer");

		const string AspNetCoreSharedServerVersion = "1.1.2";

		if (installedAspNetCoreSharedServer) return;
		installedAspNetCoreSharedServer = true;

		Shell.Exec($"dotnet tool install AspNetCoreSharedServer -g --version {AspNetCoreSharedServerVersion}");

		AddUnixGroup("www-data");
		AddUnixUser("www-data", "www-data", Utils.GetRandomString(16));

		var conf = Configuration.Current;
		using (var mutex = new Configuration.NamedMutex())
		{
			conf.Load();
			conf.EnableHttp3 = false;
			conf.User = "www-data";
			conf.Group = null;
			conf.Save();
		}

		const string ServiceId = "aspnetcore-shared-server";
		const string Description = "ASP.NET Core Shared Server support for shared hosting of ASP.NET Core applications";
		const string Command = "/root/.dotnet/tools/AspNetCoreSharedServer";
		string Directory = Path.GetDirectoryName(Command);

		ServiceDescription service;
		if (IsSystemd)
		{
			service = new SystemdServiceDescription()
			{
				ServiceId = ServiceId,
				Description = Description,
				Executable = Command,
				Directory = Directory,
				DependsOn = new List<string>() { "network-online.target" },
				Environment = new Dictionary<string, string>()
				{
					{ "ASPNETCORE_ENVIRONMENT", "Production" }
				},
				Restart = "on-failure",
				RestartSec = "1s",
				StartLimitBurst = "5",
				StartLimitIntervalSec = "500",
				User = "root",
				Group = "root",
				SyslogIdentifier = ServiceId
			};
		}
		else if (IsOpenRC)
		{
			var rcservice = new OpenRCServiceDescription()
			{
				ServiceId = ServiceId,
				Description = Description,
				Environment = new Dictionary<string, string>()
				{
					{ "ASPNETCORE_ENVIRONMENT", "Production" }
				},
				CommandUser = "root",
				Command = Command,
				CommandBackground = true,
				WorkingDirectory = Directory,
				PidFile = $"/run/{ServiceId}.pid",
				StopTimeout = 30
			};
			if (!OSInfo.IsWSL) rcservice.Need = "net";
			service = rcservice;
		}
		else if (OSInfo.IsMac)
		{
			var log = Path.Combine(WebsiteLogsPath, $"{ServiceId}.log");
			service = new LaunchdServiceDescription()
			{
				Label = ServiceId,
				Executable = Command,
				WorkingDirectory = Directory,
				Environment = new Dictionary<string, string>()
				{
					{ "ASPNETCORE_ENVIRONMENT", "Production" }
				},
				ExitTimeout = 30,
				KeepAlive = true,
				RunAtLoad = true,
				StandardOutPath = log,
				StandardErrorPath = log,
				StartOnMount = true
			};
		}
		else throw new NotSupportedException("Only SystemD, OpenRC and Launchd are supported.");

		InstallService(service);

		Log.WriteEnd("Installed AspNetCoreSharedServer");
	}
	public override void InstallWebsite(string name, string path, CommonSettings settings,
		string group, string dll, string description, string serviceId)
	{
		Info($"Creating Website {name}");

		Log.WriteStart(string.Format("Creating web site \"{0}\" ( Urls: {1} )", name, settings.Urls));

		InstallAspNetCoreSharedServer();

		if (!File.Exists(dll) && !Debugger.IsAttached)
		{
			throw new FileNotFoundException($"The service executable {dll} was not found.");
		}

		var app = new Application()
		{
			Name = serviceId,
			Assembly = dll,
			User = settings.Username ?? "root",
			Group = group,
			Urls = settings.Urls,
			ListenUrls = settings.Urls,
			EnableHttp3 = false,
			Environment = new Dictionary<string, string>() {
				{ "ASPNETCORE_ENVIRONMENT", "Production" }
			},
		};

		var conf = Configuration.Current;
		conf.Add(app);

		Thread.Sleep(1000);

		var error = Configuration.Current.ReadError();
		if (error != null) throw new InvalidOperationException(error);

		InstallLog($"Installed {name} website, listening on the url(s):{NewLine}" +
			$"{string.Join(NewLine, (GetUrls(settings) ?? "").Split(',', ';')
			.Select(url => "  " + url))}");

		OpenFirewall(settings.Urls ?? "");

		Log.WriteEnd($"Created web site \"{name}\"");
	}

	public virtual void AddUnixGroup(string group) => Shell.Exec($"groupadd {group}");
	public virtual void AddUnixUser(string user, string group, string password)
	{
		if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password)) return;

		Shell.Exec($"useradd --home /home/{user} --gid {group} -m --shell /bin/false {user}");

		var shell = Shell.ExecAsync($"passwd {user}");
		shell.Input.WriteLine(password);
		shell.Input.WriteLine(password);
		var output = shell.Output().Result;
		Log.WriteLine(output);
		
		InstallLog($"Added System User {user}.");
	}
	public override void CreateUser(CommonSettings settings)
	{
		Transaction(() =>
		{
			AddUnixGroup(SolidCPUnixGroup);
			AddUnixUser(settings.Username, SolidCPUnixGroup, settings.Password);
		}).WithRollback(() => RemoveUser(settings.Username));
	}
	public override void RemoveUser(string username) => Shell.Standard.Exec($"userdel {username}");
	/*public override void RemoveWebsite(string serviceId, CommonSettings settings)
	{
		var service = ServiceController[serviceId];

		if (service.Info != null)
		{
			service.Stop();
			service.Disable();
			service.Remove();

			InstallLog($"Removed {serviceId} service");

			RemoveFirewallRule(GetUrls(settings));
		}
	}*/
	public override void RemoveWebsite(string serviceId, CommonSettings settings)
	{
		Configuration.Current.Load();
		var app = Configuration.Current.Applications.FirstOrDefault(app => app.Name == serviceId);
		if (app != null)
		{
			Configuration.Current.Remove(app);
			InstallLog($"Removed {serviceId} service");

			RemoveFirewallRule(GetUrls(settings));
		}
	}

	public override void RemoveServerWebsite()
	{
		RemoveWebsite(UnixServerServiceId, Settings.Server);

		//InstallLog($"Removed {UnixServerServiceId} service & website.");
	}

	public override void RemoveEnterpriseServerWebsite()
	{
		RemoveWebsite(UnixEnterpriseServerServiceId, Settings.EnterpriseServer);

		//InstallLog($"Removed {UnixEnterpriseServerServiceId} service & website.");
	}

	public override void RemoveWebPortalWebsite()
	{
		RemoveWebsite(UnixPortalServiceId, Settings.WebPortal);

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
		var appsettingsfile = Path.Combine(Settings.Server.InstallPath, "bin_dotnet", "appsettings.json");
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
	public override bool CheckOSSupported() => CheckInitSystemSupported();
	public override bool CheckIISVersionSupported() => false;
	public override bool CheckInitSystemSupported() => IsSystemd || IsOpenRC || IsMac;
	public override bool CheckNetVersionSupported() => OSInfo.IsMono || OSInfo.IsCore && int.Parse(Regex.Match(OSInfo.FrameworkDescription, "[0-9]+").Value) >= 8;
	public override void RestartAsAdmin()
	{
		var password = UI.GetRootPassword();
		var assembly = GetEntryAssembly()?.Location;
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

	public override void CreateWebDavPortalUser() { }
	public override void RemoveWebDavPortalUser() { }
	public override void InstallWebDavPortalWebsite() { }
	public override void RemoveWebDavPortalWebsite() { }
	public override void RemoveWebPortalFolder() { }
	public override string[] UserIsMemeberOf(CommonSettings settings)
	{
		return new string[] { SolidCPUnixGroup };
	}
}
