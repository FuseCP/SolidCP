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

namespace SolidCP.UniversalInstaller;

public abstract partial class Installer
{
	public const bool RunAsAdmin = true;
	public virtual string SolidCP => "SolidCP";
	public virtual string ServerFolder => "Server";
	public virtual string EnterpriseServerFolder => "EnterpriseServer";
	public virtual string WebPortalFolder => "Portal";
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
	public virtual string CertificateFolder => "Certificates";
	public virtual bool IsWindows => OSInfo.IsWindows;
	public virtual bool IsUnix => OSInfo.IsUnix;
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
	public int Files {
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

	public virtual string Version => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyVersionAttribute>()?.Version;
	public void LoadSettings()
	{
		var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, InstallerSettingsFile);
		if (File.Exists(path))
		{
			var settings = JsonConvert.DeserializeObject<InstallerSettings>(
				File.ReadAllText(path), new VersionConverter(), new StringEnumConverter());
			settings.Installer.Version = null;
			settings.Installer.TempPath = null;
			Settings = settings;
		}
		else SaveSettings();
	}
	public void SaveSettings()
	{
		var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, InstallerSettingsFile);
		var tempPath = Settings.Installer.TempPath;
		Settings.Installer.TempPath = null;
		Settings.Installer.Version = null;
		var json = JsonConvert.SerializeObject(Settings, Formatting.Indented,
			new VersionConverter(), new StringEnumConverter());
		Settings.Installer.TempPath = tempPath;
		File.WriteAllText(path, json);
	}
	public void UpdateSettings()
	{
		if (Settings.Installer.Component != null)
		{
			if (Settings.Installer.Action != SetupActions.Uninstall &&
				Settings.Installer.Action != SetupActions.Setup)
			{
				if (Error == null) Settings.Installer.InstalledComponents.Add(Settings.Installer.Component);
			}
			else if (Settings.Installer.Action == SetupActions.Uninstall)
			{
				var component = Settings.Installer.InstalledComponents
					.FirstOrDefault(c => c.ComponentCode == Settings.Installer.Component.ComponentCode);
				if (component != null) Settings.Installer.InstalledComponents.Remove(component);
			} else
			{
				var component = Settings.Installer.InstalledComponents
					.FirstOrDefault(c => c.ComponentCode == Settings.Installer.Component.ComponentCode);
				var index = Settings.Installer.InstalledComponents.IndexOf(component);
				if (index >= 0)
				{
					Settings.Installer.InstalledComponents.RemoveAt(index);
					Settings.Installer.InstalledComponents.Insert(index, Settings.Installer.Component);
				}
			}
			Settings.Installer.Component = null;
			Error = null;
			SaveSettings();
		}
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

		if (Settings.Installer.Action == SetupActions.Install && info.IsInstalled)
		{
			UI.Current.ShowWarning(Global.Messages.ComponentIsAlreadyInstalled);
			return false;
		} else if (Settings.Installer.Action != SetupActions.Install && !info.IsInstalled)
		{
			UI.Current.ShowWarning(Global.Messages.ComponentIsNotInstalled);
			return false;
		}
		try
		{
			// download installer
			var tmpFolder = Settings.Installer.TempPath = FileUtils.GetTempDirectory();

			if (UI.Current.DownloadSetup(file))
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
		else {
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
			InstallLog($"Chaned file owner in {folder}.");
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
		Info("Configure firewall...");
		foreach (int port in ports) OpenFirewall(port);
		InstallLog($"Opened firewall on {string.Join(",", ports.OfType<object>().ToArray())}");
	}
	public virtual void RemoveFirewallRule(params IEnumerable<int> ports)
	{
		Info("Configure firewall...");
		foreach (int port in ports) RemoveFirewallRule(port);
		InstallLog($"Closed firewall on {string.Join(",", ports.OfType<object>().ToArray())}");
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
			AspNetInstalled = "4I",
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
			Info($"Install Website {name}");

			((HostingServiceProviderBase)WebServer).ProviderSettings.Settings.Add("WebGroupName", SolidCPWebUsersGroup);

			WebServer.CreateSite(site);
			InstallLog($"Installed Website {name}");
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

			using (var file = new FileStream(tmp, FileMode.Create, FileAccess.Write)) {
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
	public void CopyFiles(string source, string destination,
		Func<string, string> filter = null, string root = null, string destroot = null)
	{
		if (root == null && destroot == null)
		{
			Info("Copy files...");
			Transaction(() => CopyFiles(source, destination, filter, source, destination))
				.WithRollback(() => Directory.Delete(destination, true));
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
				File.Copy(source, destination);
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

	public virtual ILoadContext LoadContext {
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
}