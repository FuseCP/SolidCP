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

namespace SolidCP.UniversalInstaller
{
	public abstract class UnixInstaller : Installer
	{
		public override string? InstallExeRootPath { get => base.InstallExeRootPath ?? $"/user/local/{SolidCP}/bin"; set => base.InstallExeRootPath = value; }
		public override string? InstallWebRootPath { get => base.InstallWebRootPath ?? $"/var/www/{SolidCP}"; set => base.InstallWebRootPath = value; }
		public override string WebsiteLogsPath => $"/var/log/{SolidCP}";
		public UnixInstaller() : base() { }

		public override void InstallServerWebsite()
		{
			// Run SolidCP.Server as a service on Unix

			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder, "bin_dotnet");
			var dll = Path.Combine(websitePath, "SolidCP.Server.dll");

			var service = new ServiceDescription()
			{
				ServiceId = "SolidCPServer",
				Directory = websitePath,
				Description = "SolidCP.Server service, the server management service for the SolidCP control panel.",
				Executable = $"dotnet {dll}",
				DependsOn = new List<string>() { "network-online.target" },
				EnvironmentVariables = new Dictionary<string, string>(),
				Restart = "on-failure",
				RestartSec = "1s",
				StartLimitBurst = "5",
				StartLimitIntervalSec = "500",
				SyslogIdentifier = "SolidCPServer"
			};
			service.EnvironmentVariables.Add("ASPNETCORE_ENVIRONMENT", "Production");

			ServiceController.Install(service);
			ServiceController.Enable(service.ServiceId);
			ServiceController.Start(service.ServiceId);
		}

		public class AppSettings
		{
			public class ServerSetting
			{
				public string? Password { get; set; }
			}

			public class CertificateSetting
			{
				public StoreLocation StoreLocation;
				public StoreName StoreName;
				public X509FindType FindType;
				public string? FindValue { get; set; }
				public string? File { get; set; }
				public string? Password { get; set; }
			}

			public class LettuceEncryptSetting
			{
				public bool AcceptTermOfService { get; set; }
				public string[] DomainNames { get; set; }
				public string? EmailAddress { get; set; }
			}
			public class IgnoreAllowedHostsResolver : DefaultContractResolver
			{
				protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
				{
					JsonProperty property = base.CreateProperty(member, memberSerialization);
					if (property.PropertyName == "AllowedHosts" || property.PropertyName == "Certificate" ||
						property.PropertyName == "Server" || property.PropertyName == "LettuceEncrypt")
					{
						property.NullValueHandling = NullValueHandling.Ignore;
					}
					return property;
				}
			}

			public string? applicationUrls { get; set; } = null;
			public string? AllowedHosts { get; set; } = null;
			public ServerSetting? Server { get; set; }
			public CertificateSetting? Certificate { get; set; }
			public LettuceEncryptSetting? LettuceEncrypt { get; set; }
		}
		public override void ReadServerConfiguration()
		{
			var appsettingsfile = Path.Combine(InstallWebRootPath, ServerFolder, "bin_dotnet", "appsettings.json");
			var appsettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(appsettingsfile)) ?? new AppSettings();
			CryptoUtils.CryptoKey = ServerSettings.CryptoKey;
			ServerSettings.Urls = appsettings.applicationUrls;
			ServerSettings.ServerPassword = CryptoUtils.Decrypt(appsettings.Server?.Password ?? "");
			ServerSettings.LetsEncryptCertificateEmail = appsettings.LettuceEncrypt?.EmailAddress;
			ServerSettings.LetsEncryptCertificateDomains = (appsettings.LettuceEncrypt != null && appsettings.LettuceEncrypt.DomainNames != null) ? string.Join(", ", appsettings.LettuceEncrypt.DomainNames) : "";
			ServerSettings.CertificateFile = appsettings.Certificate?.File;
			ServerSettings.CertificatePassword = appsettings.Certificate?.Password;
			ServerSettings.CertificateStoreLocation = appsettings.Certificate?.StoreLocation.ToString();
			ServerSettings.CertificateStoreName = appsettings.Certificate?.StoreName.ToString();
			ServerSettings.CertificateFindType = appsettings.Certificate?.FindType.ToString();
			ServerSettings.CertificateFindValue = appsettings.Certificate?.FindValue;
		}

		public override void ConfigureServer()
		{
			var appsettings = new AppSettings();
			appsettings.applicationUrls = ServerSettings.Urls;
			var allowedHosts = (ServerSettings?.Urls ?? "").Split(',', ';')
				.Select(url => new Uri(url.Trim()).Host)
				.ToList();
			if (allowedHosts.Any(host => host == "localhost"))
			{
				allowedHosts.Add("127.0.0.1");
				allowedHosts.Add("::1");
			}
			if (allowedHosts.Any(host => host == "*")) appsettings.AllowedHosts = null;
			else appsettings.AllowedHosts = string.Join(";", allowedHosts.Distinct());

			if (!string.IsNullOrEmpty(ServerSettings.ServerPassword))
			{
				CryptoUtils.CryptoKey = ServerSettings.CryptoKey;
				appsettings.Server = new AppSettings.ServerSetting() { Password = CryptoUtils.Encrypt(ServerSettings.ServerPassword ?? "") };
			}

			if (!string.IsNullOrEmpty(ServerSettings.LetsEncryptCertificateEmail) && !string.IsNullOrEmpty(ServerSettings.LetsEncryptCertificateDomains))
			{
				appsettings.LettuceEncrypt = new AppSettings.LettuceEncryptSetting()
				{
					AcceptTermOfService = true,
					EmailAddress = ServerSettings.LetsEncryptCertificateEmail,
					DomainNames = ServerSettings.LetsEncryptCertificateDomains
						?.Split(',', ';')
						.Select(domain => domain.Trim())
						.ToArray() ?? new string[0]
				};
			}
			else if (!string.IsNullOrEmpty(ServerSettings.CertificateFile) && !string.IsNullOrEmpty(ServerSettings.CertificatePassword))
			{
				appsettings.Certificate = new AppSettings.CertificateSetting()
				{
					File = ServerSettings.CertificateFile,
					Password = ServerSettings.CertificatePassword
				};
			}
			else if (!string.IsNullOrEmpty(ServerSettings.CertificateStoreLocation) && !string.IsNullOrEmpty(ServerSettings.CertificateStoreName))
			{
				appsettings.Certificate = new AppSettings.CertificateSetting()
				{
					FindValue = ServerSettings.CertificateFindValue
				};
				Enum.TryParse<StoreLocation>(ServerSettings.CertificateStoreLocation, out appsettings.Certificate.StoreLocation);
				Enum.TryParse<StoreName>(ServerSettings.CertificateStoreName, out appsettings.Certificate.StoreName);
				Enum.TryParse<X509FindType>(ServerSettings.CertificateFindType, out appsettings.Certificate.FindType);
			}

			var appsettingsfile = Path.Combine(InstallWebRootPath, ServerFolder, "bin_dotnet", "appsettings.json");
			File.WriteAllText(appsettingsfile, JsonConvert.SerializeObject(appsettings, Formatting.Indented, new JsonSerializerSettings()
			{
				ContractResolver = new AppSettings.IgnoreAllowedHostsResolver()
			}));
		}
		public override void InstallServerPrerequisites()
		{
			InstallNet8Runtime();
		}
		public override Func<string, string?>? UnzipFilter => Net8UnzipFilter;
		public override bool IsRunningAsAdmin()
		{
			//var uid = Mono.Posix.Syscall.getuid();
			var euid = Mono.Unix.Native.Syscall.geteuid();
			return euid == 0;
		}

		public override void RestartAsAdmin()
		{
			var password = UI.GetRootPassword();
			var assembly = Assembly.GetEntryAssembly()?.Location;
			string? arguments = null;
			try
			{
				arguments = Process.GetCurrentProcess().StartInfo.Arguments;
			}
			catch { }
			arguments = arguments ?? assembly;
			if (OSInfo.IsMono) Shell.ExecScript($"echo {password} | sudo -S mono {arguments}");
			else if (OSInfo.IsCore) Shell.ExecScript($"echo {password} | sudo -S dotnet {arguments}");

			Environment.Exit(Shell.Process?.ExitCode ?? -1);
		}
	}
}
