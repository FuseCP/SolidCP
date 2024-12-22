using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
//using Ionic.Zip;
using System.IO.Compression;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.Net.Mime.MediaTypeNames;
using SolidCP.UniversalInstaller.Core;
using System.Collections;

namespace SolidCP.UniversalInstaller
{

	public abstract partial class Installer
	{
		public const bool RunAsAdmin = true;
		public virtual string SolidCP => "SolidCP";
		public virtual string ServerFolder => "Server";
		public virtual string EnterpriseServerFolder => "EnterpriseServer";
		public virtual string PortalFolder => "Portal";
		public virtual string WebDavPortalFolder => "WebDavPortal";
		public virtual string ServerUser => $"{SolidCP}Server";
		public virtual string EnterpriseServerUser => $"{SolidCP}EnterpriseServer";
		public virtual string WebPortalUser => $"{SolidCP}Portal";
		public virtual string SolidCPWebUsersGroup => "SCP_IUSRS";
		public virtual string UnixAppRootPath => "/usr";
		public virtual string NewLine => Environment.NewLine;
		public virtual bool CanInstallServer => true;
		public virtual bool CanInstallEnterpriseServer => OSInfo.IsWindows;
		public virtual bool CanInstallPortal => OSInfo.IsWindows;
		public virtual string InstallerSettingsFile => "installer.settings.json";
		public virtual bool IsWindows => OSInfo.IsWindows;
		public virtual bool IsUnix => OSInfo.IsUnix;

		static bool? hasDotnet = null;
		public virtual bool HasDotnet
		{
			get => hasDotnet ?? (hasDotnet = Shell.Find("dotnet") != null).Value;
			set => hasDotnet = value;
		}
		public void ResetHasDotnet() => hasDotnet = null;
		public bool NeedRemoveNet8Runtime = false;
		public bool NeedRemoveNet8AspRuntime = false;
		public virtual string InstallWebRootPath { get; set; } = null;
		public virtual string InstallExeRootPath { get; set; } = null;
		public abstract string WebsiteLogsPath { get; }
		public int EstimatedOutputLines = 0;
		public InstallerSettings Settings { get; set; } = new InstallerSettings()
		{
			Server = new ServerSettings(),
			EnterpriseServer = new EnterpriseServerSettings(),
			WebPortal = new WebPortalSettings(),
			Installer = new InstallerSpecificSettings(),
			WebDavPortal = new CommonSettings()
		};

		public Shell Shell { get; set; } = Shell.Standard.Clone;
		public Providers.OS.Installer OSInstaller => OSInfo.Current.DefaultInstaller;
		public IWebServer WebServer => OSInfo.Current.WebServer;
		public ServiceController ServiceController => OSInfo.Current.ServiceController;
		public UI UI => UI.Current;
		LogWriter log = null;
		public virtual LogWriter Log => log ??= new LogWriter();
		/* public virtual void Log(string msg)
		{
			Debug.WriteLine(msg);
			Trace.WriteLine(msg);
			Shell.Log?.Invoke(msg);
			LogWriter?.WriteLine(msg);
		} */

		public List<string> InstallLogs { get; private set; } = new List<string>();
		public void InstallLog(string msg) => InstallLogs.Add(msg);

		public void LoadSettings()
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, InstallerSettingsFile);
			if (File.Exists(path))
			{
				var settings = JsonConvert.DeserializeObject<InstallerSettings>(
						File.ReadAllText(path)
					);
				Settings = settings;
			}
			else SaveSettings();
		}
		public void SaveSettings()
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, InstallerSettingsFile);
			var json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
			File.WriteAllText(path, json);
		}

		public bool Install(ComponentInfo info)
		{
			info.FullFilePath = info.FullFilePath.Replace('\\', Path.DirectorySeparatorChar);
			info.InstallerPath = info.InstallerPath.Replace('\\', Path.DirectorySeparatorChar);
			Settings.Installer.Component = info;

			string fileName = info.FullFilePath;
			string installerPath = info.InstallerPath;
			string installerType = info.InstallerType;

			if (info.CheckForInstalledComponent())
			{
				UI.Current.ShowWarning(Global.Messages.ComponentIsAlreadyInstalled);
				return false;
			}
			try
			{
				// download installer
				var tmpFolder = Settings.Installer.TempPath = FileUtils.GetTempDirectory();

				if (UI.Current.DownloadSetup(fileName)) {
					string path = Path.Combine(tmpFolder, installerPath);
					string method = "Install";
					Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", installerType, method, path));

					var json = JsonConvert.SerializeObject(Settings, new VersionConverter());

					//run installer
					var res = (bool)AssemblyLoader.Execute(path, installerType, method, new object[] { json });
					FileUtils.DeleteTempDirectory();

					return res;
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
				UI.Current.ShowError(ex);
			}

			return false;
		}

		bool firstCheck = true;
		public bool CheckNet8RuntimeInstalled()
		{
			if (!firstCheck) return true;
			firstCheck = false;

			if (HasDotnet)
			{
				var output = Shell.Exec("dotnet --info").Output().Result ?? "";
				NeedRemoveNet8AspRuntime = !output.Contains("Microsoft.AspNetCore.App 8.");
				NeedRemoveNet8Runtime = !output.Contains("Microsoft.NETCore.App 8.");
				return !NeedRemoveNet8Runtime && !NeedRemoveNet8AspRuntime;
			}
			else {
				NeedRemoveNet8Runtime = NeedRemoveNet8AspRuntime = true;
				return false;
			}
		}
		public abstract void InstallNet8Runtime();
		public abstract void RemoveNet8AspRuntime();
		public abstract void RemoveNet8NetRuntime();
		public virtual void RemoveNet8Runtime()
		{
			if (NeedRemoveNet8Runtime) RemoveNet8NetRuntime();
			if (NeedRemoveNet8AspRuntime) RemoveNet8AspRuntime();
		}

		public virtual void SetFilePermissions(string folder)
		{
			if (!Path.IsPathRooted(folder)) folder = Path.Combine(InstallWebRootPath, folder);

			if (!OSInfo.IsWindows)
			{
				OSInfo.Unix.GrantUnixPermissions(folder,
					UnixFileMode.UserWrite | UnixFileMode.GroupWrite |
					UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead |
					UnixFileMode.UserExecute | UnixFileMode.GroupExecute, true);
			}
		}
		public virtual void SetFileOwner(string folder, string owner, string group)
		{
			if (!Path.IsPathRooted(folder)) folder = Path.Combine(InstallWebRootPath, folder);

			if (!OSInfo.IsWindows)
			{
				OSInfo.Unix.ChangeUnixFileOwner(folder, owner, group, true);
			}
		}

		public IEnumerable<int> ExternalPortsFromUrls(string urls)
		{
			return urls.Split(',', ';')
				.Select(url => new Uri(url))
				.Where(uri => uri.Host != "localhost" && uri.Host != "127.0.0.1" && uri.Host != "::1")
				.Select(uri => uri.Port);
		}
		public virtual void OpenFirewall(string urls) => OpenFirewall(ExternalPortsFromUrls(urls));
		public virtual void RemoveFirewallRule(string urls) => RemoveFirewallRule(ExternalPortsFromUrls(urls));
		public virtual void OpenFirewall(int port) { }
		public virtual void RemoveFirewallRule(int port) { }
		public virtual void OpenFirewall(params IEnumerable<int> ports)
		{
			foreach (int port in ports) OpenFirewall(port);
		}
		public virtual void RemoveFirewallRule(params IEnumerable<int> ports)
		{
			foreach (int port in ports) RemoveFirewallRule(port);
		}

		public virtual void InstallWebsite(string name, string path, string urls, string username, string password)
		{
				/* var remoteServerSettings = new RemoteServerSettings() { ADEnabled = false };
				// Create web users group
				if (!SecurityUtils.GroupExists(SolidCPWebUsersGroup, remoteServerSettings, ""))
				{
					var group = new SystemGroup() { GroupName = SolidCPWebUsersGroup, Name = SolidCPWebUsersGroup, Description = "SolidCP Website User Group" };
					SecurityUtils.CreateGroup(group, remoteServerSettings, "", "");
				} */

			var site = new WebSite()
			{
				ContentPath = path,
				GroupName = SolidCPWebUsersGroup,
				AspNetInstalled = "v4.0",
				AnonymousUsername = username,
				AnonymousUserPassword = password,
				ApplicationPool = name,
				DedicatedApplicationPool = true,
				EnableAnonymousAccess = true,
				EnableBasicAuthentication = true,
				EnableDynamicCompression = false,
				EnableWritePermissions = true,
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

			Transaction(() =>
			{
				((HostingServiceProviderBase)WebServer).ProviderSettings.Settings.Add("WebGroupName", SolidCPWebUsersGroup);

				WebServer.CreateSite(site);
			})
			.WithRollback(() => WebServer.DeleteSite(site.SiteId));
			
			int p = 0;
			var ports = site.Bindings
				.Where(binding => binding.IP != "127.0.0.1" && binding.IP != "::1" && binding.Host != "localhost")
				.Select(binding => int.TryParse(binding.Port, out p) ? p : 0);
			OpenFirewall(ports);
		}

		public void InstallService(ServiceDescription description)
		{
			Transaction(() =>
			{
				var service = ServiceController.Install(description);
				service.Enable();
				var status = service.Info;
				if (status != null && status.Status == OSServiceStatus.Running) service.Stop();
				service.Start();
			})
			.WithRollback(() => RemoveService(description.ServiceId));
		}
		public void RemoveService(string serviceId)
		{
			var service = ServiceController[serviceId];
			service.Stop();
			service.Disable();
			service.Remove();
		}
		public virtual Func<string, string> UnzipFilter => null;

		public async Task<string> DownloadFileAsync(string url)
		{
			var web = new HttpClient();
			var tmp = Path.GetTempFileName();
			tmp = Path.ChangeExtension(tmp, Path.GetExtension(url));
			using (HttpResponseMessage response = await web.GetAsync(url))
			{
				if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception($"Could not download file {url}. Status code: {response.StatusCode}");

				using (var file = new FileStream(tmp, FileMode.Create, FileAccess.Write)) {
					await response.Content.CopyToAsync(file);
				}
			}
			var name = Regex.Replace(url, @"(.*?/)|(?:\?.*$)", "", RegexOptions.Singleline);
			Log.WriteLine($"Downloaded file {name}");
			return tmp;
		}
		public string DownloadFile(string url) => DownloadFileAsync(url).Result;
		public string Net48Filter(string file)
		{
			return (!file.StartsWith("Setup/") && !file.StartsWith("bin_dotnet/") && !Regex.IsMatch(file, "(?:^|/)appsettings.json$")) ? file : null;
		}
		public string Net8Filter(string file)
		{
			return (!file.StartsWith("Setup/") && (!file.StartsWith("bin/") || file.StartsWith("bin/netstandard/")) &&
				!file.EndsWith(".config", StringComparison.OrdinalIgnoreCase) && file != "appsettings.json" &&
				!file.EndsWith(".aspx") && !file.EndsWith(".asax") && !file.EndsWith(".asmx")) ? file : null;
		}

		public string ConfigAndSetupFilter(string file)
		{
			return !file.StartsWith("Setup/") && !file.EndsWith("/web.config", StringComparison.OrdinalIgnoreCase) &&
				!file.EndsWith("/appsettings.json", StringComparison.OrdinalIgnoreCase) ? file : null;
		}
		public string SetupFilter(string file)
		{
			return !file.StartsWith("Setup/") ? file : null;
		}

		public void CopyFiles(string source, string destination,
			Func<string, string> filter = null, string root = null, string destroot = null)
		{
			if (root == null && destroot == null)
			{
				Transaction(() => CopyFiles(source, destination, filter, source, destination))
					.WithRollback(() => Directory.Delete(destination, true));
			}
			else
			{
				if (filter != null && source.StartsWith(root))
				{
					int len = root.Length;
					if (!root.EndsWith(Path.DirectorySeparatorChar.ToString())) len++;
					var file = source.Substring(len)
						.Replace(Path.DirectorySeparatorChar, '/');
					var dest = filter(file);
					if (dest == null) return;
					destination = Path.Combine(destroot, dest.Replace('/', Path.DirectorySeparatorChar));
				}
				if (root == null) root = source;
				if (destroot == null) destroot = destination;
				if (Directory.Exists(source))
				{
					if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
					foreach (var file in Directory.GetFiles(source))
					{
						var name = Path.GetFileName(file);
						File.Copy(file, Path.Combine(destination, name));
						Shell.Log?.Invoke($"Copied {name}{NewLine}");
					}
					foreach (var dir in Directory.GetDirectories(source))
					{
						CopyFiles(dir, Path.Combine(destination, Path.GetFileName(dir)), filter, root, destroot);
					}
				}
				else if (File.Exists(source))
				{
					File.Copy(source, destination);
					Shell.Log?.Invoke($"Copied {Path.GetFileName(source)}{NewLine}");
				}
			}
		}

		public void UnzipFromStream(Stream resource, string destinationPath, Func<string, string> filter = null)
		{
			Transaction(() =>
			{
				Directory.CreateDirectory(destinationPath);

				if (filter == null) filter = name => name;

				var dirs = new HashSet<string>();
				using (var zip = new ZipArchive(resource))
				{
					foreach (var zipEntry in zip.Entries)
					{
						var name = filter?.Invoke(zipEntry.FullName);

						if (name != null)
						{
							var fullName = Path.GetFullPath(Path.Combine(destinationPath, name.Replace('/', Path.DirectorySeparatorChar)));

							if (string.IsNullOrEmpty(zipEntry.Name))
								Directory.CreateDirectory(fullName);
							else
								zipEntry.ExtractToFile(fullName);

							Shell.Log?.Invoke($"Extracted {name}{NewLine}");
						}
					}
				}
			})
			.WithRollback(() => Directory.Delete(destinationPath, true));
		}
		
		public void UnzipFromResource(string resourcePath, string destinationPath, Func<string, string> filter = null)
		{
			Transaction(() =>
			{
				var assembly = Assembly.GetEntryAssembly();
				var resourceName = assembly.GetManifestResourceNames()
					.FirstOrDefault(res => res.EndsWith(resourcePath, StringComparison.OrdinalIgnoreCase));

				if (resourceName == null) throw new NotSupportedException($"Cannot find {resourcePath} in resources.");

				using (var resource = assembly.GetManifestResourceStream(resourceName))
				{
					UnzipFromStream(resource, destinationPath, filter);
				}
			})
			.WithRollback(() => Directory.Delete(destinationPath));
		}
		public virtual void InstallWinAcme() => Shell.Exec("dotnet tool install win-acme --global");
		public virtual bool IsRunningAsAdmin => true;
		public virtual void RestartAsAdmin() { }
		public void InstallAll()
		{
			const int EstimatedOutputLinesPerSite = 200;

			Shell.LogFile = "SolidCP.Installer.log";

			if (!IsRunningAsAdmin && !Debugger.IsAttached) RestartAsAdmin();

			UI.CheckPrerequisites();

			var packages = UI.GetPackagesToInstall();

			bool installServer = false, installEnterpriseServer = false, installPortal = false;

			try
			{
				if (CanInstallServer && packages.HasFlag(Packages.Server))
				{
					ReadServerConfiguration();
					Settings.Server = UI.GetServerSettings();
					EstimatedOutputLines += EstimatedOutputLinesPerSite;
					installServer = true;
				}

				if (CanInstallEnterpriseServer && packages.HasFlag(Packages.EnterpriseServer))
				{
					ReadEnterpriseServerConfiguration();
					Settings.EnterpriseServer = UI.GetEnterpriseServerSettings();
					EstimatedOutputLines += EstimatedOutputLinesPerSite;
					installEnterpriseServer = true;
				}
				if (CanInstallPortal && packages.HasFlag(Packages.WebPortal))
				{
					ReadWebPortalConfiguration();
					Settings.WebPortal = UI.GetWebPortalSettings();
					EstimatedOutputLines += EstimatedOutputLinesPerSite;
					installPortal = true;
				}

				if (installServer || installPortal || installEnterpriseServer) UI.ShowInstallationProgress();

				if (installServer) InstallServer();
				if (installEnterpriseServer) InstallEnterpriseServer();
				if (installPortal) InstallWebPortal();

				if (installServer || installPortal || installEnterpriseServer) UI.CloseInstallationProgress();

				UI.ShowInstallationSuccess(packages);
			}
			catch (Exception ex)
			{
				UI.ShowError(ex);

				Shell.Log($"Exception: {ex}");

				Console.WriteLine("Press any key to exit...");
				Console.Read();
			}
		}

		protected void ParseConnectionString(string connectionString, out string server, out string user, out string password,
			out bool windowsAuthentication)
		{
			server = user = password = "";
			windowsAuthentication = false;
			
			var matches = Regex.Matches(connectionString, @"(?:Server\s*=\s*(?<server>.*?)\s*(?:;|$))|(?:(?:uid|User ID)\s*=\s*(?<user>.*?)\s*(?:;|$))|(?:(?:pwd|Password)\s*=\s*(?<password>.*?)\s*(?:;|$))|(?:Integrated Security\s*=\s*(?<windowsAuth>.*?)\s*(?:;|$))", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			foreach (Match match in matches)
			{
				if (match.Groups["server"].Success) server = match.Groups["server"].Value;
				if (match.Groups["user"].Success) user = match.Groups["user"].Value;
				if (match.Groups["password"].Success) password = match.Groups["password"].Value;
				if (match.Groups["windowsAuth"].Success)
				{
					var win = match.Groups["windowsAuth"].Value;
					windowsAuthentication = string.Equals(win, "true", StringComparison.OrdinalIgnoreCase) ||
						string.Equals(win, "SSPI", StringComparison.OrdinalIgnoreCase);
				}
			}
		}

		public abstract bool CheckOSSupported();
		public abstract bool CheckSystemdSupported();
		public abstract bool CheckIISVersionSupported();
		public abstract bool CheckNetVersionSupported();

		static Installer current;
		public static Installer Current
		{
			get
			{
				if (current == null)
				{
					switch (OSInfo.OSFlavor)
					{
						// TODO support for Ubuntu variants
						case OSFlavor.Windows: current = new WindowsInstaller(); break;
						case OSFlavor.Mac: current = new MacInstaller(); break;
						case OSFlavor.Debian: current = new DebianInstaller(); break;
						case OSFlavor.Ubuntu: current = new UbuntuInstaller(); break;
						case OSFlavor.Mint: current = new MintInstaller(); break;
						case OSFlavor.Kali: current = new KaliInstaller(); break;
						case OSFlavor.Fedora: current = new FedoraInstaller(); break;
						case OSFlavor.RedHat: current = new RedHatInstaller(); break;
						case OSFlavor.CentOS: current = new CentOSInstaller(); break;
						case OSFlavor.Oracle: current = new OracleInstaller(); break;
						case OSFlavor.SUSE: current = new SuseInstaller(); break;
						case OSFlavor.Arch: current = new ArchInstaller(); break;
						case OSFlavor.Alpine: current = new AlpineInstaller(); break;
						default: throw new PlatformNotSupportedException("This OS is not supported by the installer.");
					}
				}
				return current;
			}
		}
	}
}