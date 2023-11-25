using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using Ionic.Zip;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;

namespace SolidCP.UniversalInstaller
{


	public abstract class Installer
	{
		public virtual string SolidCP => "SolidCP";
		public virtual string ServerFolder => "Server";
		public virtual string EnterpriseServerFolder => "EnterpriseServer";
		public virtual string PortalFolder => "Portal";
		public virtual string ServerUser => $"{SolidCP}Server";
		public virtual string EnterpriseServerUser => $"{SolidCP}EnterpriseServer";
		public virtual string WebPortalUser => $"{SolidCP}Portal";
		public virtual bool CanInstallServer => true;
		public virtual bool CanInstallEnterpriseServer => OSInfo.IsWindows;
		public virtual bool CanInstallPortal => OSInfo.IsWindows;
		static bool? hasDotnet = false;
		public virtual bool HasDotnet => hasDotnet ?? (hasDotnet = Shell.Find("dotnet") != null).Value;
		public bool Net8RuntimeAllreadyInstalled = true;
		public bool Net8AspRuntimeAllreadyInstalled = true;
		public virtual string? InstallWebRootPath { get; set; } = null;
		public virtual string? InstallExeRootPath { get; set; } = null;
		public abstract string WebsiteLogsPath { get; }

		public ServerSettings? ServerSettings { get; set; } = null;
		public EnterpriseServerSettings? EnterpriseServerSettings { get; set; } = null;
		public WebPortalSettings? WebPortalSettings { get; set; } = null;

		public Shell Shell { get; set; } = OSInfo.Current.DefaultShell.Clone;
		public Providers.OS.Installer OSInstaller => OSInfo.Current.DefaultInstaller;
		public IWebServer WebServer => OSInfo.Current.WebServer;
		public ServiceController ServiceController => OSInfo.Current.ServiceController;
		public UI UI => UI.Current;

		public bool CheckNet8RuntimeInstalled()
		{
			if (HasDotnet)
			{
				var output = Shell.Exec("dotnet --info").Output().Result ?? "";
				Net8AspRuntimeAllreadyInstalled = output.Contains("Microsoft.AspNetCore.App 8.");
				Net8RuntimeAllreadyInstalled = output.Contains("Microsoft.NETCore.App 8.");
				return Net8RuntimeAllreadyInstalled && Net8AspRuntimeAllreadyInstalled;
			}
			else return Net8RuntimeAllreadyInstalled = Net8AspRuntimeAllreadyInstalled = false;
		}

		public abstract void InstallNet8Runtime();
		public abstract void RemoveNet8Runtime();

		public virtual void InstallServerPrerequisites() { }
		public virtual void InstallEnterpriseServerPrerequisites() { }
		public virtual void InstallPortalPrerequisites() { }
		public virtual void RemoveServerPrerequisites() { }
		public virtual void RemoveEnterpriseServerPrerequisites() { }
		public virtual void RemovePortalPrerequisites() { }


		public virtual void SetFilePermissions(string folder)
		{
			if (!Path.IsPathRooted(folder)) folder = Path.Combine(InstallWebRootPath, folder);

			throw new CultureNotFoundException();
		}
		public virtual void SetServerFilePermissions() => SetFilePermissions(ServerFolder);
		public virtual void SetEnterpriseServerFilePermissions() => SetFilePermissions(EnterpriseServerFolder);
		public virtual void SetPortalFilePermissions() => SetFilePermissions(PortalFolder);

		public virtual void ConfigureServer()
		{
		}

		public virtual void InstallServer()
		{
			InstallServerPrerequisites();
			var settings = ReadServerConfiguration();
			UnzipServer();
			InstallServerWebsite();
			SetServerFilePermissions();
			ConfigureServer(settings);
		}


		public virtual void InstallWebsite(string name, string path, string urls)
		{
			var site = new WebSite()
			{
				ContentPath = path,
				AspNetInstalled = "",
				ApplicationPool = "",
				DedicatedApplicationPool = true,
				EnableAnonymousAccess = true,
				EnableBasicAuthentication = true,
				EnableDynamicCompression = false,
				EnableWritePermissions = false,
				Name = name,
				LogsPath = WebsiteLogsPath,
			};
			site.Bindings = urls
				.Split(';')
				.Select(url =>
				{
					url = url.Trim();
					var uri = new Uri(url);
					string ip = uri.Host;

					return new ServerBinding(uri.Scheme, "0.0.0.0", uri.Port.ToString(), uri.Host);
				})
				.ToArray();

			WebServer.CreateSite(site);
		}
		public virtual void InstallServerUser() { }
		public virtual void InstallServerApplicationPool() { }
		public virtual void InstallServerWebsite() { }
		public virtual void InstallEnterpriseServer()
		{
			InstallEnterpriseServerPrerequisites();
			var settings = ReadEnterpriseServerConfiguration();
			UnzipEnterpriseServer();
			InstallEnterpriseServerWebsite();
			SetEnterpriseServerFilePermissions();
		}
		public virtual void InstallEnterpriseServerWebsite()
		{
			InstallWebsite($"{SolidCP}EnterpriseServer", Path.Combine(InstallWebRootPath, "EnterpriseServer"), EnterpriseServerSettings.Urls);
		}
		public virtual void InstallWebPortal() { }
		public ServerSettings ReadServerConfiguration()
		{
			return new ServerSettings();
		}
		public EnterpriseServerSettings ReadEnterpriseServerConfiguration()
		{
			return new EnterpriseServerSettings();
		}
		public WebPortalSettings ReadWebPortalConfiguration()
		{
			return new WebPortalSettings();
		}

		public void ConfigureServer(ServerSettings settings)
		{
		}
		public void ConfigureEnterpriseServer(EnterpriseServerSettings settings)
		{

		}
		public void ConfigureWebPortal(WebPortalSettings settings)
		{

		}
		public virtual void UnzipServer()
		{
			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
			if (!Directory.Exists(websitePath)) Directory.CreateDirectory(websitePath);
			UnzipFromResource("SolidCP.Server.zip", websitePath);
		}

		public virtual void UnzipEnterpriseServer()
		{
			var websitePath = Path.Combine(InstallWebRootPath, EnterpriseServerFolder);
			if (!Directory.Exists(websitePath)) Directory.CreateDirectory(websitePath);
			UnzipFromResource("SolidCP.EnterpriseServer.zip", websitePath);
		}

		public virtual void UnzipPortal()
		{
			var websitePath = Path.Combine(InstallWebRootPath, PortalFolder);
			if (!Directory.Exists(websitePath)) Directory.CreateDirectory(websitePath);
			UnzipFromResource("SolidCP.WebPortal.zip", websitePath);
		}

		public async Task<string> DownloadFileAsync(string url)
		{
			var web = new HttpClient();
			var tmp = Path.GetTempFileName();
			tmp = Path.ChangeExtension(tmp, Path.GetExtension(url));
			using (HttpResponseMessage response = await web.GetAsync(url))
			{
				if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception($"Could not download file {url}. Status code: {response.StatusCode}");

				using (Stream responseStream = await response.Content.ReadAsStreamAsync())
				using (var file = new FileStream(tmp, FileMode.Create, FileAccess.Write))
				{
					await responseStream.CopyToAsync(file);
				}
			}
			return tmp;
		}
		public string DownloadFile(string url) => DownloadFileAsync(url).Result;

		public void UnzipFromResource(string resourcePath, string destinationPath)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = assembly.GetManifestResourceNames()
				.FirstOrDefault(res => res.EndsWith(resourcePath));

			if (resourceName == null) throw new NotSupportedException($"Cannot find {resourcePath} in resources.");

			using (var resource = assembly.GetManifestResourceStream(resourceName))
			using (var zip = ZipFile.Read(resource))
			{
				foreach (var zipEntry in zip)
				{
					Shell.Log?.Invoke($"Exracting {zipEntry.FileName}{Environment.NewLine}");
					zipEntry.Extract(destinationPath);
				}
			}
		}

		public virtual bool CheckIsRoot() => true;
		public virtual void RestartAsRoot(string password) { }

		public void InstallAll()
		{
			Shell.Log += msg => File.AppendAllText("SolidCP.Installer.log", msg);

			if (!CheckIsRoot())
			{
				var password = UI.GetRootPassword();
				RestartAsRoot(password);
				return;
			}

			var packages = UI.GetPackagesToInstall();

			try
			{
				if (CanInstallServer && (packages & Packages.Server) != 0)
				{
					ServerSettings = ReadServerConfiguration();
					ServerSettings = UI.GetServerSettings();
					InstallServer();
				}

				if (CanInstallEnterpriseServer && (packages & Packages.EnterpriseServer) != 0)
				{
					EnterpriseServerSettings = ReadEnterpriseServerConfiguration();
					EnterpriseServerSettings = UI.GetEnterpriseServerSettings();
					UI.ShowInstallationProgress();
					InstallEnterpriseServer();
					UI.CloseInstallationProgress();
				}

				if (CanInstallPortal && (packages & Packages.WebPortal) != 0)
				{
					WebPortalSettings = ReadWebPortalConfiguration();
					WebPortalSettings = UI.GetWebPortalSettings();
					InstallWebPortal();
				}
			} catch (Exception ex)
			{
				Shell.Log($"Exception: {ex}");
			}
		}

		static Installer current;
		public static Installer Current
		{
			get
			{
				if (current == null)
				{
					switch (OSInfo.OSFlavor)
					{
						case OSFlavor.Debian: current = new DebianInstaller(); break;
						// TODO support for Ubuntu variants
						// case OSFlavor.Mint:
						case OSFlavor.Ubuntu: current = new UbuntuInstaller(); break;
						case OSFlavor.RedHat: current = new RedHatInstaller(); break;
						case OSFlavor.CentOS: current = new CentOSInstaller(); break;
						case OSFlavor.Fedora: current = new FedoraInstaller(); break;
						case OSFlavor.Mac: current = new MacInstaller(); break;
						case OSFlavor.Windows: current = new WindowsInstaller(); break;
						case OSFlavor.Alpine: current = new AlpineInstaller(); break;
						case OSFlavor.SUSE: current = new SuseInstaller(); break;
						default: throw new PlatformNotSupportedException("This OS is not supported by the installer.");
					}
				}
				return current;
			}
		}
	}
}