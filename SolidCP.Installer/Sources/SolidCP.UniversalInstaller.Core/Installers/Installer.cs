using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using System.IO.Compression;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.Web.Administration;
using SolidCP.UniversalInstaller.Core;

namespace SolidCP.UniversalInstaller;

public abstract partial class Installer
{
	public const bool RunAsAdmin = true;
	public virtual string SolidCP => "SolidCP";
	public virtual string ServerFolder => "Server";
	public virtual string EnterpriseServerFolder { get; set; } = "EnterpriseServer";
	public virtual string WebPortalFolder => "Portal";
	public virtual string WebDavPortalFolder => "WebDavPortal";
	public virtual string ServerUser => $"{SolidCP}Server";
	public virtual string EnterpriseServerUser => $"{SolidCP}EnterpriseServer";
	public virtual string WebPortalUser => $"{SolidCP}Portal";
	public virtual string SolidCPWebUsersGroup => "SCP_IUSRS";
	public virtual string UnixAppRootPath => "/usr";
	public virtual string NewLine => Environment.NewLine;
	public virtual bool CanInstallServer => true;
	public virtual bool CanInstallEnterpriseServer => true;
	public virtual bool CanInstallPortal => true;
	public virtual string InstallerSettingsFile => "installer.settings.json";
	public virtual string CertificateFolder => "Certificates";
	public virtual string TempPath => Settings.Installer.TempPath;
	public virtual string BackupPath => Path.Combine(TempPath, "Backup");
	public virtual string ComponentTempPath => Path.Combine(TempPath, "Component");
	public virtual bool IsWindows => OSInfo.IsWindows;
	public virtual bool IsUnix => OSInfo.IsUnix;
	public virtual bool IsInstallAction => Settings.Installer.Action == SetupActions.Install;
	public virtual bool IsUpdateAction => Settings.Installer.Action == SetupActions.Update;
	public virtual bool IsSetupAction => Settings.Installer.Action == SetupActions.Setup;
	public virtual bool IsUninstallAction => Settings.Installer.Action == SetupActions.Uninstall;
	public virtual string SolidCPUnixGroup => "solidcp";
	public Action OnExit { get; set; }
	public Action<Exception> OnError { get; set; }

	static bool? hasDotnet = null;
	public virtual bool HasDotnet
	{
		get
		{
			if (hasDotnet == null)
			{
				var find = Shell.Find("dotnet");
				hasDotnet = find != null && !Regex.IsMatch(find, "^/mnt/[a-zA-Z]/");
			}
			return hasDotnet.Value;
		}
		set => hasDotnet = value;
	}
	public void ResetHasDotnet() => hasDotnet = null;
	public bool NeedRemoveNet8Runtime = false;
	public bool NeedRemoveNet8AspRuntime = false;
	public virtual string InstallWebRootPath { get; set; } = null;
	public virtual string InstallExeRootPath { get; set; } = null;
	public abstract string WebsiteLogsPath { get; }

	int? estimatedOutputLines = null;
	public virtual Func<int> CalculateEstimateOutputLines { get; set; } = null;
	public virtual int EstimatedOutputLines
	{
		get => estimatedOutputLines ??= CalculateEstimateOutputLines != null ?
			CalculateEstimateOutputLines() : Files + DatabaseStatements + 50;
		set => estimatedOutputLines = value;
	}
	public int Files
	{
		get => Settings.Installer.Files;
		set => Settings.Installer.Files = value;
	}
	public int DatabaseStatements { get; set; } = 0;

	public InstallerSettings Settings { get; set; } = new InstallerSettings()
	{
		Server = new ServerSettings(),
		EnterpriseServer = new EnterpriseServerSettings(),
		WebPortal = new WebPortalSettings(),
		Installer = new InstallerSpecificSettings(),
		WebDavPortal = new WebDavPortalSettings(),
		Standalone = new StandaloneSettings()
	};

	public Shell Shell { get; set; } = Shell.Standard.Clone;
	public Providers.OS.Installer OSInstaller => OSInfo.Current.DefaultInstaller;
	public IWebServer WebServer => OSInfo.Current.WebServer;
	public ServiceController ServiceController => OSInfo.Current.ServiceController;
	public UI UI => UI.Current;
	LogWriter log = null;
	public virtual LogWriter Log => log ??= new LogWriter();

	public List<string> InstallLogs { get; private set; } = new List<string>();
	public void InstallLog(string msg)
	{
		InstallLogs.Add(msg);
		Log.WriteLine(msg);
	}
	public Action<string> OnInfo { get; set; }
	public void Info(string message)
	{
		OnInfo?.Invoke(message);
		Log.WriteLine(message);
	}
	public virtual bool IsSetup => !Assembly.GetEntryAssembly().GetName().Name.Contains("Installer");
	public virtual Version Version
	{
		get
		{
			if (!IsSetup) return Assembly.GetEntryAssembly().GetName().Version;
			else return Settings.Installer.Version;
		}
	}
	public void LoadSettings()
	{
		var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, InstallerSettingsFile);
		if (File.Exists(path))
		{
			var settings = JsonConvert.DeserializeObject<InstallerSettings>(
				File.ReadAllText(path), new VersionConverter(), new StringEnumConverter());
			settings.Installer.Version = Version;
			settings.Installer.TempPath = FileUtils.GetTempDirectory();
			settings.Installer.UI = UI.Current.GetType().Name;
			Settings = settings;
		}
		else SaveSettings();
	}
	public void SaveSettings()
	{
		var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, InstallerSettingsFile);
		var tempPath = TempPath;
		Settings.Installer.TempPath = null;
		Settings.Installer.Version = null;
		Settings.Installer.UI = null;
		var json = JsonConvert.SerializeObject(Settings, Formatting.Indented,
			new VersionConverter(), new StringEnumConverter());
		Settings.Installer.TempPath = tempPath;
		Settings.Installer.UI = UI.Current.GetType().Name;
		Settings.Installer.Version = Version;
		File.WriteAllText(path, json);
	}
	public void UpdateSettings()
	{
		if (HasError) return;

		if (Settings.Installer.Component != null)
		{
			ComponentInfo component;
			switch (Settings.Installer.Action)
			{
				case SetupActions.Install:
					Settings.Installer.InstalledComponents.Add(Settings.Installer.Component);
					break;
				case SetupActions.Uninstall:
					component = Settings.Installer.InstalledComponents
						.FirstOrDefault(c => c.ComponentCode == Settings.Installer.Component.ComponentCode);
					if (component != null) Settings.Installer.InstalledComponents.Remove(component);
					break;
				case SetupActions.Setup:
					break;
				case SetupActions.Update:
					component = Settings.Installer.InstalledComponents
						.FirstOrDefault(c => c.ComponentCode == Settings.Installer.Component.ComponentCode);
					if (component != null) Settings.Installer.InstalledComponents.Remove(component);
					Settings.Installer.InstalledComponents.Add(Settings.Installer.Component);
					break;
			}
			Settings.Installer.Component = null;
			SaveSettings();
		}
	}

	public virtual void Cleanup()
	{
		if (Directory.Exists(BackupPath)) Directory.Delete(BackupPath, true);
		Settings.Installer.Component = null;
		Error = null;
		Cancel = new CancellationTokenSource();
	}

	public bool RunSetup(ComponentInfo info, SetupActions action)
	{
		Settings.Installer.Component = info;
		Settings.Installer.Action = action;

		info.FullFilePath = info.FullFilePath?.Replace('\\', Path.DirectorySeparatorChar);
		info.UpgradeFilePath = info.UpgradeFilePath?.Replace('\\', Path.DirectorySeparatorChar);
		info.InstallerPath = info.InstallerPath?.Replace('\\', Path.DirectorySeparatorChar);

		RemoteFile file = new RemoteFile(info, action != SetupActions.Update);
		string installerPath = info.InstallerPath;
		string installerType = info.InstallerType;

		if (action == SetupActions.Install && info.IsInstalled)
		{
			UI.Current.ShowWarning(Global.Messages.ComponentIsAlreadyInstalled);
			return false;
		}
		else if (action != SetupActions.Install && !info.IsInstalled)
		{
			UI.Current.ShowWarning(Global.Messages.ComponentIsNotInstalled);
			return false;
		}
		try
		{
			// download installer
			var tmpFolder = ComponentTempPath;

			if (UI.Current.DownloadSetup(file, action == SetupActions.Uninstall))
			{
				UI.Current.ShowWaitCursor();
				string path = Path.Combine(tmpFolder, installerPath);
				string method;
				switch (Settings.Installer.Action)
				{
					default:
					case SetupActions.Install: method = "Install"; break;
					case SetupActions.Uninstall: method = "Uninstall"; break;
					case SetupActions.Setup: method = "Setup"; break;
					case SetupActions.Update: method = "Update"; break;
				}
				Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", installerType, method, path));

				var json = JsonConvert.SerializeObject(Settings, new VersionConverter(), new StringEnumConverter());

				var hashtable = new Hashtable();
				hashtable["ParametersJson"] = json;
				UI.PassArguments(hashtable);

				//run installer
				var res = (Result)LoadContext.Execute(path, installerType, method, new object[] { hashtable }) == Result.OK;

				FileUtils.DeleteTempDirectory();

				LoadSettings();

				UI.EndWaitCursor();

				Log.WriteInfo(string.Format("Installer returned {0}", res));
				Log.WriteEnd("Installer finished");

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
	public bool Install(ComponentInfo info) => RunSetup(info, SetupActions.Install);
	public bool Uninstall(ComponentInfo info) => RunSetup(info, SetupActions.Uninstall);
	public bool Setup(ComponentInfo info) => RunSetup(info, SetupActions.Setup);
	public bool Update(ComponentInfo info) => RunSetup(info, SetupActions.Update);
	protected bool? Net8RuntimeInstalled { get; set; }
	public bool CheckNet8RuntimeInstalled()
	{
		if (Net8RuntimeInstalled != null) return Net8RuntimeInstalled.Value;

		if (HasDotnet)
		{
			var output = Shell.Exec("dotnet --info").Output().Result ?? "";
			NeedRemoveNet8AspRuntime = !output.Contains("Microsoft.AspNetCore.App 8.");
			NeedRemoveNet8Runtime = !output.Contains("Microsoft.NETCore.App 8.");
			var installed = !NeedRemoveNet8Runtime && !NeedRemoveNet8AspRuntime;
			Net8RuntimeInstalled = installed;
			return installed;
		}
		else
		{
			NeedRemoveNet8Runtime = NeedRemoveNet8AspRuntime = true;
			Net8RuntimeInstalled = false;
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

		if (NeedRemoveNet8Runtime || NeedRemoveNet8AspRuntime)
			InstallLog("Removed .NET 8 Runtime");
	}

	public virtual void SetFilePermissions(string folder)
	{
		if (!Path.IsPathRooted(folder)) folder = Path.Combine(InstallWebRootPath, folder);

		if (!OSInfo.IsWindows)
		{
			Info("Grant file permissions...");
			OSInfo.Unix.GrantUnixPermissions(folder,
				UnixFileMode.UserWrite | UnixFileMode.GroupWrite |
				UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead |
				UnixFileMode.UserExecute | UnixFileMode.GroupExecute, true);
			InstallLog($"Granted file permissions on {folder}.");
		}
	}
	public virtual void SetFileOwner(string folder, string owner, string group)
	{
		if (!Path.IsPathRooted(folder)) folder = Path.Combine(InstallWebRootPath, folder);

		if (!OSInfo.IsWindows)
		{
			Info("Set file owner...");
			OSInfo.Unix.ChangeUnixFileOwner(folder, owner, group, true);
			InstallLog($"Changed file owner in {folder}.");
		}
	}

	public IEnumerable<int> ExternalPortsFromUrls(string urls)
	{
		return (urls ?? "").Split(',', ';')
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
		if (ports.Any())
		{
			Info("Configure firewall...");
			foreach (int port in ports) OpenFirewall(port);
			InstallLog($"Opened firewall on {string.Join(",", ports.OfType<object>().ToArray())}");
		}
	}
	public virtual void RemoveFirewallRule(params IEnumerable<int> ports)
	{
		if (ports.Any())
		{
			Info("Configure firewall...");
			foreach (int port in ports) RemoveFirewallRule(port);
			InstallLog($"Closed firewall on {string.Join(",", ports.OfType<object>().ToArray())}");
		}
	}

	protected bool IsIis7 => OSInfo.IsWindows && OSInfo.Windows.WebServer.Version.Major >= 7;
	protected bool IsHttps(CommonSettings settings)
	{
		if (!string.IsNullOrEmpty(settings.Urls))
		{
			return settings.Urls.Split(';', ',')
				.Select(url => new Uri(url))
				.Any(uri => uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase));
		}
		return (IsIis7 || !OSInfo.IsWindows) && Utils.IsHttpsAndNotWindows(settings.WebSiteIp, settings.WebSiteDomain);
	}
	public virtual string GetUrls(CommonSettings settings)
	{
		if (!string.IsNullOrEmpty(settings.Urls)) return settings.Urls;
		else
		{
			var isHttps = IsHttps(settings);
			return $"http{(isHttps ? "s" : "")}://{(!string.IsNullOrEmpty(settings.WebSiteDomain) ? settings.WebSiteDomain : settings.WebSiteIp)}:{settings.WebSitePort}";
		}
	}

	public virtual void InstallWebsite(string name, string path, CommonSettings settings,
		string group, string dll, string description, string serviceId)
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
			AspNetInstalled = "4I",
			AnonymousUsername = settings.Username ?? "",
			AnonymousUserPassword = settings.Password ?? "",
			ApplicationPool = name,
			DedicatedApplicationPool = true,
			EnableAnonymousAccess = true,
			EnableBasicAuthentication = true,
			EnableDynamicCompression = false,
			EnableWritePermissions = true,
			Name = name,
			LogsPath = WebsiteLogsPath,
		};
		if (!string.IsNullOrEmpty(settings.Urls))
		{
			site.Bindings = (settings.Urls ?? "")
				.Split(';')
				.Select(url =>
				{
					url = url.Trim();
					var uri = new Uri(url);
					IPAddress ip;
					string host;
					if (!IPAddress.TryParse(uri.Host, out ip))
					{
						ip = null;
						host = uri.Host;
					}
					else
					{
						host = "";
					}
					return new ServerBinding(uri.Scheme, ip?.ToString(), uri.Port.ToString(), host);
				})
				.ToArray();
		}
		else
		{
			bool isHttps = IsHttps(settings);

			site.Bindings = new[] {
					new ServerBinding(isHttps ? "https" : "http", settings.WebSiteIp, settings.WebSitePort.ToString(), settings.WebSiteDomain)
				};
		}
		Transaction(() =>
		{
			Info($"Install Website {name}");

			((HostingServiceProviderBase)WebServer).ProviderSettings.Settings.Add("WebGroupName", SolidCPWebUsersGroup);

			WebServer.CreateSite(site);
			InstallLog($"Installed {name} website, listening on the url(s):" +
				$"{string.Join(NewLine, (GetUrls(settings) ?? "").Split(',', ';')
					.Select(url => "  " + url))}");
		})
		.WithRollback(() => WebServer.DeleteSite(site.SiteId));

		int p = 0;
		var ports = site.Bindings
			.Where(binding => binding.IP != "127.0.0.1" && binding.IP != "::1" && binding.Host != "localhost")
			.Select(binding => int.TryParse(binding.Port, out p) ? p : 0);
		OpenFirewall(ports);
	}

	public virtual void RemoveUser(string username)
	{
		if (!string.IsNullOrEmpty(username) && OSInfo.IsWindows)
		{
			var bslash = username.IndexOf('\\');
			string machine, user;
			if (bslash >= 0)
			{
				machine = username.Substring(0, bslash);
				if (bslash < username.Length - 1) user = username.Substring(bslash, username.Length - bslash - 1);
				else user = "";
			}
			else
			{
				machine = "";
				user = username;
			}
			if (SecurityUtils.UserExists(machine, user))
			{
				SecurityUtils.DeleteUser(machine, user);
				InstallLog($"Removed user {username}");
			}
		}
	}
	public virtual void RemoveWebsite(string siteId, CommonSettings setting)
	{
		RemoveFirewallRule(GetUrls(setting));

		Info($"Delete website {siteId}");
		WebServer.DeleteSite(siteId);
		RemoveUser(setting.Username);
		InstallLog($"Removed website {siteId}");
	}

	public void InstallService(ServiceDescription description)
	{
		Transaction(() =>
		{
			Info($"Install service {description.ServiceId}");
			var service = ServiceController.Install(description);
			service.Enable();
			var status = service.Info;
			if (status != null && status.Status == OSServiceStatus.Running) service.Stop();
			service.Start();
			InstallLog($"Installed service {description.ServiceId}");
		})
		.WithRollback(() => RemoveService(description.ServiceId));
	}
	public void RemoveService(string serviceId)
	{
		Info($"Remove service {serviceId}");
		var service = ServiceController[serviceId];
		service.Stop();
		service.Disable();
		service.Remove();
		InstallLog($"Removed service {serviceId}.");
	}
	public virtual Func<string, string> UnzipFilter => null;

	public async Task<string> DownloadFileAsync(string url)
	{
		Info($"Download {url}.");
		var web = new HttpClient();
		var tmp = Path.GetTempFileName();
		tmp = Path.ChangeExtension(tmp, Path.GetExtension(url));
		using (HttpResponseMessage response = await web.GetAsync(url))
		{
			if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new Exception($"Could not download file {url}. Status code: {response.StatusCode}");

			using (var file = new FileStream(tmp, FileMode.Create, FileAccess.Write))
			{
				await response.Content.CopyToAsync(file);
			}
		}
		var name = Regex.Replace(url, @"(.*?/)|(?:\?.*$)", "", RegexOptions.Singleline);
		InstallLog($"Downloaded file {name}");
		return tmp;
	}
	public string DownloadFile(string url) => DownloadFileAsync(url).Result;
	public string SetupFilter(string file) => file != null && !file.StartsWith("Setup/") ? file : null;
	public string Net48Filter(string file)
	{
		file = SetupFilter(file);
		return (file != null && !file.StartsWith("bin_dotnet/") && !Regex.IsMatch(file, "(?:^|/)appsettings.json$", RegexOptions.IgnoreCase)) ? file : null;
	}
	public virtual string Net8Filter(string file)
	{
		file = SetupFilter(file);
		return (file != null && (!file.StartsWith("bin/") || file.StartsWith("bin/netstandard/")) &&
			!Regex.IsMatch(file, "(?:^|/)web.config", RegexOptions.IgnoreCase) &&
			!file.EndsWith(".aspx") && !file.EndsWith(".asax") && !file.EndsWith(".asmx")) ? file : null;
	}
	public string ConfigAndSetupFilter(string file)
	{
		file = SetupFilter(file);
		return (file != null && !file.EndsWith("/web.config", StringComparison.OrdinalIgnoreCase) &&
			!Regex.IsMatch(file, "(?:^|/)appsettings.json$", RegexOptions.IgnoreCase)) ? file : null;
	}
	public virtual string StandardInstallFilter(string file) => SetupFilter(file);
	public virtual string StandardUpdateFilter(string file) => ConfigAndSetupFilter(file);

	public virtual void CopyFile(string source, string destination, bool settings = false)
	{
		if (File.Exists(source))
		{
			File.Copy(source, destination, true);
			if (settings)
			{
				if (IsUnix)
				{
					var owner = OSInfo.Unix.GetUnixFileOwner(source);
					OSInfo.Unix.ChangeUnixFileOwner(destination, owner.Owner, owner.Group);
					var mode = OSInfo.Unix.GetUnixPermissions(source);
					OSInfo.Unix.GrantUnixPermissions(destination, mode);
				}
				else if (IsWindows)
				{
					var sourceInfo = new FileInfo(source);
					var acl = sourceInfo.GetAccessControl();
					acl.SetAccessRuleProtection(true, true);
					var destInfo = new FileInfo(destination);
					destInfo.SetAccessControl(acl);
				}
			}
			Log.ProgressOne();
		}
	}

	public void CopyFiles(string source, string destination,
		Func<string, string> filter = null, string root = null, string destroot = null, bool settings = false)
	{
		if (root == null && destroot == null)
		{
			Info("Copy files...");
			if (IsUpdateAction)
			{
				// Create backup
				if (Directory.Exists(BackupPath)) Directory.Delete(BackupPath, true);
				CopyFiles(destination, BackupPath, null, destination, BackupPath, true);
			}
			Transaction(() =>
			{
				CopyFiles(source, destination, filter, source, destination);
				InstallLog("Unzipped & installed distribution files");
			})
				.WithRollback(() =>
				{
					Directory.Delete(destination, true);
					if (IsUpdateAction)
					{
						// Restore backup
						CopyFiles(BackupPath, destination, null, source, BackupPath, true);
					}
				});
		}
		else
		{
			if (filter != null && source.StartsWith(root))
			{
				int len = root.Length;
				if (!root.EndsWith(Path.DirectorySeparatorChar.ToString()) && len < source.Length) len++;
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
				if (settings)
				{ // Copy security settings
					if (IsUnix)
					{
						var owner = OSInfo.Unix.GetUnixFileOwner(source);
						OSInfo.Unix.ChangeUnixFileOwner(destination, owner.Owner, owner.Group);
						var mode = OSInfo.Unix.GetUnixPermissions(source);
						OSInfo.Unix.GrantUnixPermissions(destination, mode);
					}
					else if (IsWindows)
					{
						var sourceInfo = new DirectoryInfo(source);
						var acl = sourceInfo.GetAccessControl();
						acl.SetAccessRuleProtection(true, true);
						var destInfo = new DirectoryInfo(destination);
						destInfo.SetAccessControl(acl);
					}
				}
				foreach (var file in Directory.GetFiles(source))
				{
					var name = Path.GetFileName(file);
					File.Copy(file, Path.Combine(destination, name), true);
					Log.ProgressOne();
				}
				foreach (var dir in Directory.GetDirectories(source))
				{
					CopyFiles(dir, Path.Combine(destination, Path.GetFileName(dir)), filter, root, destroot);
				}
			}
			else if (File.Exists(source))
			{
				File.Copy(source, destination, true);
				if (settings)
				{
					if (IsUnix)
					{
						var mode = OSInfo.Unix.GetUnixPermissions(source);
						OSInfo.Unix.GrantUnixPermissions(destination, mode);
					}
				}
				Log.ProgressOne();
			}
		}
	}

	public void UnzipFromStream(Stream resource, string destinationPath, Func<string, string> filter = null)
	{
		Transaction(() =>
		{
			Info($"Unizp {Path.GetFileName(destinationPath)}");

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

	public virtual void ConfigureAppsettings(CommonSettings settings)
	{
		AppSettings appsettings = null;
		string folder;
		WebPortalSettings webSettings = null;
		bool embedEnterpriseServer = false;
		if (settings is ServerSettings) folder = ServerFolder;
		else if (settings is EnterpriseServerSettings) folder = EnterpriseServerFolder;
		else if (settings is WebDavPortalSettings) folder = WebDavPortalFolder;
		else if (settings is WebPortalSettings)
		{
			folder = WebPortalFolder;
			webSettings = (WebPortalSettings)settings;
			embedEnterpriseServer = webSettings.EmbedEnterpriseServer;
		}
		else throw new NotSupportedException();

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

		var urls = settings.Urls;
		if (string.IsNullOrEmpty(urls)) urls = GetUrls(settings);

		appsettings.applicationUrls = urls;
		var allowedHosts = (urls ?? "").Split(',', ';')
			.Select(url => new Uri(url.Trim()).Host)
			.ToList();
		if (allowedHosts.Any(host => host == "localhost"))
		{
			allowedHosts.Add("127.0.0.1");
			allowedHosts.Add("::1");
		}
		if (allowedHosts.Any(host => host == "*")) appsettings.AllowedHosts = null;
		else appsettings.AllowedHosts = string.Join(";", allowedHosts.Distinct());

		if (settings is ServerSettings srvSettings && 
			(!string.IsNullOrEmpty(srvSettings.ServerPassword) || !string.IsNullOrEmpty(srvSettings.ServerPasswordSHA)))
		{
			string pwsha;
			if (!string.IsNullOrEmpty(srvSettings.ServerPassword))
			{
				pwsha = CryptoUtils.ComputeSHAServerPassword(srvSettings.ServerPassword);
			}
			else
			{
				pwsha = srvSettings.ServerPasswordSHA;
			}
			appsettings.Server = new AppSettings.ServerSetting() { Password = pwsha };
		}

		if (!string.IsNullOrEmpty(settings.LetsEncryptCertificateEmail) && !string.IsNullOrEmpty(settings.LetsEncryptCertificateDomains))
		{
			appsettings.LettuceEncrypt = new AppSettings.LettuceEncryptSetting()
			{
				AcceptTermOfService = true,
				EmailAddress = settings.LetsEncryptCertificateEmail,
				DomainNames = settings.LetsEncryptCertificateDomains
					?.Split(',', ';')
					.Select(domain => domain.Trim())
					.ToArray() ?? new string[0]
			};
		}
		else if (!string.IsNullOrEmpty(settings.CertificateFile) && !string.IsNullOrEmpty(settings.CertificatePassword))
		{
			// create a local copy of the certificate file
			var certFile = settings.CertificateFile;
			var certFolder = Path.Combine(InstallWebRootPath, CertificateFolder);
			if (!Directory.Exists(certFolder)) Directory.CreateDirectory(certFolder);
			var shadowFileName = $"{Guid.NewGuid()}.{Path.GetFileName(certFile)}";
			var shadowFile = Path.Combine(certFolder, shadowFileName);
			File.Copy(certFile, shadowFile);
			OSInfo.Unix.GrantUnixPermissions(shadowFile, UnixFileMode.UserRead | UnixFileMode.UserWrite);

			appsettings.Certificate = new AppSettings.CertificateSetting()
			{
				File = shadowFile,
				Password = settings.CertificatePassword
			};
		}
		else if (!string.IsNullOrEmpty(settings.CertificateStoreLocation) && !string.IsNullOrEmpty(settings.CertificateStoreName))
		{
			appsettings.Certificate = new AppSettings.CertificateSetting()
			{
				FindValue = settings.CertificateFindValue
			};
			Enum.TryParse<StoreLocation>(settings.CertificateStoreLocation, out appsettings.Certificate.StoreLocation);
			Enum.TryParse<StoreName>(settings.CertificateStoreName, out appsettings.Certificate.StoreName);
			Enum.TryParse<X509FindType>(settings.CertificateFindType, out appsettings.Certificate.FindType);
		}

		if (string.IsNullOrEmpty(appsettings.probingPaths)) appsettings.probingPaths = "..\\bin\\netstandard";

		if (settings is WebPortalSettings)
		{
			if (webSettings.EmbedEnterpriseServer)
			{
				appsettings.probingPaths = @$"..\bin\netstandard;{webSettings.EnterpriseServerPath}\bin_dotnet;{webSettings.EnterpriseServerPath}\bin\netstandard";
				var esJsonFile = Path.Combine(InstallWebRootPath, EnterpriseServerFolder, "bin_dotnet", "appsettings.json");
				if (File.Exists(esJsonFile))
				{
					var esSettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(esJsonFile),
						new VersionConverter(), new StringEnumConverter()) ?? new AppSettings();
					appsettings.EnterpriseServer = esSettings.EnterpriseServer;
				}
			}
		}

		var path = Path.GetDirectoryName(appsettingsfile);
		if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		File.WriteAllText(appsettingsfile, JsonConvert.SerializeObject(appsettings, Formatting.Indented, new JsonSerializerSettings()
		{
			ContractResolver = new AppSettings.IgnoreAllowedHostsResolver(),
			Converters = new List<JsonConverter>() { new VersionConverter(), new StringEnumConverter() }
		}));
	}

	public void ConfigureCertificateNetFX(CommonSettings settings, XElement configuration)
	{
		// server certificate
		if (!settings.ConfigureCertificateManually)
		{
			if (!string.IsNullOrEmpty(settings.CertificateStoreLocation) &&
				!string.IsNullOrEmpty(settings.CertificateStoreName) &&
				!string.IsNullOrEmpty(settings.CertificateFindType) &&
				!string.IsNullOrEmpty(settings.CertificateFindValue))
			{
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
					new XAttribute("x509FindType", settings.CertificateFindType),
					new XAttribute("findValue", settings.CertificateFindValue));
				serviceCredentials.Add(cert);
			}
			else throw new PlatformNotSupportedException("Certificate file or Let's Encrypt not supported on Windows.");
		}
	}
	public virtual string GetUninstallLog(ComponentSettings settings) => "";
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

	bool exitCalled = false;
	public virtual void Exit(int errorCode = 0)
	{
		if (!exitCalled)
		{
			exitCalled = true;
			SaveSettings();
			OnExit?.Invoke();
			UI.Exit();
			Environment.Exit(errorCode);
		}
	}
	protected void ParseConnectionString(string connectionString, out EnterpriseServer.Data.DbType dbType, out string server, out string user, out string password,
		out bool windowsAuthentication, out bool trustCertificate)
	{
		server = user = password = "";
		windowsAuthentication = false;
		dbType = EnterpriseServer.Data.DbType.Unknown;

		var csb = new System.Data.Common.DbConnectionStringBuilder();
		csb.ConnectionString = connectionString;

		string get(string key) => csb.ContainsKey(key) ? csb[key] as string : null;

		string type = get("dbtype");
		Enum.TryParse<EnterpriseServer.Data.DbType>(type, out dbType);
		server = get("server");
		user = get("uid") ?? get("user id") ?? get("username") ?? get("user");
		password = get("pwd") ?? get("password");
		var security = get("integrated sceurity");
		windowsAuthentication = security.Equals("true", StringComparison.OrdinalIgnoreCase) ||
			security.Equals("sspi");
		var tc = get("trustservercertificate");
		trustCertificate = tc != null && tc.Equals("true", StringComparison.OrdinalIgnoreCase);
	}
	public abstract bool CheckOSSupported();
	public abstract bool CheckSystemdSupported();
	public abstract bool CheckIISVersionSupported();
	public abstract bool CheckNetVersionSupported();

	public virtual void WaitForDownloadToComplete()
	{
		var progressFile = Path.Combine(TempPath, SetupLoader.DownloadProgressFile);
		var nofFilesFile = Path.Combine(TempPath, SetupLoader.NofFilesFile);
		int n = 0;
		var info = new FileInfo(progressFile);
		while (info.Exists)
		{
			while (info.Exists && n++ < info.Length)
			{
				Log.ProgressOne();
			}
			Thread.Sleep(50);
			info = new FileInfo(progressFile);
		}

		if (File.Exists(nofFilesFile))
		{
			var fileTxt = File.ReadAllText(nofFilesFile);
			Installer.Current.Files = int.Parse(fileTxt);
			File.Delete(nofFilesFile);
		}
	}
	public virtual ILoadContext LoadContext
	{
		get
		{
			if (OSInfo.IsCore) return Activator.CreateInstance(GetType("SolidCP.UniversalInstaller.LoadContextImplementation, SolidCP.UniversalInstaller.Runtime.NetCore")) as ILoadContext;
			else return Activator.CreateInstance(GetType("SolidCP.UniversalInstaller.LoadContextImplementation, SolidCP.UniversalInstaller.Runtime.NetFX")) as ILoadContext;
		}
	}

	public virtual SecurityUtils SecurityUtils
	{
		get
		{
			if (OSInfo.IsCore) return Activator.CreateInstance(GetType("SolidCP.UniversalInstaller.Runtime.SecurityUtils, SolidCP.UniversalInstaller.Runtime.NetCore")) as SecurityUtils;
			else return Activator.CreateInstance(GetType("SolidCP.UniversalInstaller.Runtime.SecurityUtils, SolidCP.UniversalInstaller.Runtime.NetFX")) as SecurityUtils;
		}
	}

	public virtual WebUtils WebUtils
	{
		get
		{
			if (OSInfo.IsCore) return Activator.CreateInstance(GetType("SolidCP.UniversalInstaller.Runtime.WebUtils, SolidCP.UniversalInstaller.Runtime.NetCore")) as WebUtils;
			else return Activator.CreateInstance(GetType("SolidCP.UniversalInstaller.Runtime.WebUtils, SolidCP.UniversalInstaller.Runtime.NetFX")) as WebUtils;
		}
	}

	// Gets a Type by name. On NET Core it uses the custom AssemblyLoadContext.
	public virtual Type GetType(string name)
	{
		if (OSInfo.IsCore)
		{
			var ctxType = Type.GetType("System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader");
			var ctx = ctxType.GetMethod("GetLoadContext").Invoke(null, new object[] { Assembly.GetExecutingAssembly() });
			if (ctx.GetType().Name == "SetupAssemblyLoadContext")
			{
				var defaultCtx = ctxType.GetProperty("Default").GetValue(null);
				var loadCustom = ctx.GetType().GetMethod("Load", BindingFlags.Instance | BindingFlags.NonPublic);
				var loadDefault = defaultCtx.GetType().GetMethod("Load", BindingFlags.Instance | BindingFlags.NonPublic);
				return Type.GetType(name, asmName =>
					(loadCustom.Invoke(ctx, new object[] { asmName }) ??
						loadDefault.Invoke(defaultCtx, new object[] { asmName })) as Assembly,
					(Func<Assembly, string, bool, Type>)((asm, type, ignoreCase) =>
						asm.GetType(type, true, ignoreCase)));
			}
		}
		return Type.GetType(name);
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

	public bool CheckForInstallerUpdate(out ComponentUpdateInfo component)
	{
		bool ret = false;
		Log.WriteStart("Checking for a new version");
		//
		UI.ShowWaitCursor();
		var webService = Installer.Current.InstallerWebService;
		component = webService.GetLatestComponentUpdate(Global.InstallerProductCode);
		UI.EndWaitCursor();
		//
		Log.WriteEnd("Checked for a new version");
		if (component != null)
		{
			Version currentVersion = GetType().Assembly.GetName().Version;
			Version newVersion = null;
			if (component.Version != default) newVersion = component.Version;
			else
			{
				Log.WriteError("Version error");
				return false;
			}
			if (newVersion > currentVersion)
			{
				ret = true;
				Log.WriteInfo(string.Format("Version {0} is available for download", newVersion));
			}
		}
		return ret;
	}

	public bool DownloadInstallerUpdate(ComponentUpdateInfo component)
	{
		Log.WriteStart("Starting updater");
		string entry = Assembly.GetEntryAssembly().Location;
		string tmpFile = Path.ChangeExtension(Path.GetTempFileName(), Path.GetExtension(entry));
		
		File.Copy(entry, tmpFile);

		//
		string url = Installer.Current.Settings.Installer.WebServiceUrl;
		//
		string proxyServer = string.Empty;
		string user = string.Empty;
		string password = string.Empty;

		// check if we need to add a proxy to access Internet
		bool useProxy = Installer.Current.Settings.Installer.Proxy != null;
		if (useProxy)
		{
			proxyServer = Installer.Current.Settings.Installer.Proxy.Address;
			user = Installer.Current.Settings.Installer.Proxy.Username;
			password = Installer.Current.Settings.Installer.Proxy.Password;
		}

		//prepare command line args
		StringBuilder sb = new StringBuilder();

		ProcessStartInfo info = new ProcessStartInfo();
		var isExe = Path.GetExtension(tmpFile).Equals(".exe", StringComparison.OrdinalIgnoreCase);
		var winconsole = UI.IsConsole && OSInfo.IsWindows;
		if (isExe) info.FileName = tmpFile;
		else
		{
			var runtime = Path.ChangeExtension(entry, ".runtimeconfig.json");
			var runtimedest = Path.ChangeExtension(tmpFile, ".runtimeconfig.json");
			File.Copy(runtime, runtimedest);
			info.FileName = Shell.Find(OSInfo.IsWindows ? "dotnet.exe" : "dotnet");
			sb.Append($"\"{tmpFile}\" ");
		}
		sb.Append("-update ");
		sb.Append($"-ui={UI.Current.GetType().Name.Replace("UI", "").ToLower()} ");
		sb.AppendFormat("-url:\"{0}\" ", url);
		sb.AppendFormat("-target:\"{0}\" ", entry);
		sb.AppendFormat("-file:\"{0}\" ", component.UpgradeFilePath);
		sb.AppendFormat("-proxy:\"{0}\" ", proxyServer);
		sb.AppendFormat("-user:\"{0}\" ", user);
		sb.AppendFormat("-password:\"{0}\" ", password);
		info.Arguments = sb.ToString();
		info.UseShellExecute = winconsole;
		info.CreateNoWindow = !winconsole;

		Process process = Process.Start(info);
		if (process.Handle != IntPtr.Zero)
		{
			User32.SetForegroundWindow(process.Handle);
		}
		Log.WriteEnd("Updater started");

		if (UI.IsConsole && !Providers.OS.OSInfo.IsWindows)
		{
			UI.ShowWaitCursor();
			process.WaitForExit();
			UI.EndWaitCursor();

			Console.WriteLine("HostPanelPro Installer has been updated. Please restart it...");
			Installer.Current.Exit();
		}

		return (process.Handle != IntPtr.Zero);
	}
}