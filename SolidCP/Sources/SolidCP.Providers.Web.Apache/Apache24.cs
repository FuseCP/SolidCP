using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;
using System.IO;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Web;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Web.Apache;

namespace SolidCP.Providers.Web
{
	public class Apache24 : HostingServiceProviderBase, IWebServer
	{
		public OS.Shell Shell => OS.Shell.Default;

		#region Properties
		public string ApacheConfigPath => ProviderSettings["ConfigPath"]; // /etc/apache2

		public string ApacheConfigFile => ProviderSettings["ConfigFile"]; // /etc/apache2/apache2.conf

		public string ApacheBinPath => ProviderSettings["BinPath"];

		string ApacheCmd(string cmd)
		{
			var exe = Path.Combine(ApacheBinPath, cmd);
			if (OSInfo.IsWindows) exe += ".exe";
			if (string.IsNullOrEmpty(ApacheBinPath) || !File.Exists(exe)) exe = Shell.Default.Find(cmd);
			if (exe.Contains(' ')) exe = $"\"{exe}\"";
			return exe;
		}

		string apachectl => ApacheCmd(nameof(apachectl));
		string a2ensite => ApacheCmd(nameof(a2ensite));
		string a2dissite => ApacheCmd(nameof(a2dissite));
		#endregion

		ConfigFile Config(string siteId)
		{
			var path = Path.Combine(ApacheConfigPath, "sites-available", $"{siteId}.conf");
			return ConfigFile.Load(path);
		}

		ConfigFile GlobalConfig
		{
			get
			{
				var conf = ConfigFile.Load(ApacheConfigFile);
				conf.Include();
				return conf;
			}
		}

		void ReloadApache()
		{
			var output = Shell.Exec($"{apachectl} start");
			Shell.Exec($"{apachectl} graceful");

		}

		public bool AppVirtualDirectoryExists(string siteId, string directoryName)
		{
			var conf = Config(siteId);
			return conf.Sections.Any(s => s is Location && s.Argument == directoryName);
		}

		public void ChangeAppPoolState(string siteId, AppPoolState state)
		{
			throw new NotImplementedException();
		}

		public void ChangeFrontPagePassword(string username, string password)
		{
			throw new NotImplementedException();
		}

		public void ChangeSiteState(string siteId, ServerState state)
		{
			switch (state)
			{
				case ServerState.Started:
				case ServerState.Starting:
				case ServerState.Continuing:
					Shell.Exec($"{a2ensite} {siteId}");
					ReloadApache();
					break;
				case ServerState.Paused:
				case ServerState.Pausing:
				case ServerState.Stopped:
				case ServerState.Stopping:
					Shell.Exec($"{a2dissite} {siteId}");
					ReloadApache();
					break;
				default: break;
			}
		}

		public void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
		{
			throw new NotImplementedException();
		}

		public bool CheckCertificate(WebSite webSite)
		{
			throw new NotImplementedException();
		}

		public bool CheckLoadUserProfile()
		{
			throw new NotImplementedException();
		}

		public bool CheckWebManagementAccountExists(string accountName)
		{
			throw new NotImplementedException();
		}

		public ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
		{
			throw new NotImplementedException();
		}

		public void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			var conf = Config(siteId);
			var dir = new Location()
			{
				Url = directory.VirtualPath,
				DocumentRoot = directory.ContentPath
				//TODO config location
			};
			var vhosts = conf.Sections.OfType<VirtualHost>();
			foreach (var vhost in vhosts) vhost.Add(dir);
			conf.Save();
			ReloadApache();
		}

		public void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			throw new NotImplementedException();
		}

		public string CreateSite(WebSite site)
		{
			site.SiteId = site.Name;

			var conf = Config(site.SiteId);
			var bindings = site.Bindings.Select(b => new
			{
				Address = $"{((string.IsNullOrEmpty(b.IP) || b.IP == "0.0.0.0" || b.IP == "::" || b.IP == "*") ? "*" : b.IP)}:{b.Port}",
				Listen = ((string.IsNullOrEmpty(b.IP) || b.IP == "0.0.0.0" || b.IP == "::" || b.IP == "*") ? b.Port : $"{b.IP}:{b.Port}") +
					((b.Protocol == "https" && b.Port != "443") ? " https" : ""),
				Host = (!string.IsNullOrEmpty(b.Host) && b.Host != "*") ? b.Host : null
			});
			var bindingsByAddress = bindings.GroupBy(b => b.Address);

			var globalConfig = new ConfigFile[] { GlobalConfig }
				.Concat(GlobalConfig.Descendants.OfType<IncludeFile>());
			var globalNameVirtualHosts = globalConfig
				.SelectMany(file => file.GetAll("NameVirtualHosts"));
			var allNameVirtualHosts = globalNameVirtualHosts.Any(vhost => vhost == "*");
			var globalListen = globalConfig
				.SelectMany(file => file.GetAll("Listen"));
			var nameVirtualHosts = bindingsByAddress
				.Select(b => b.Key)
				.Except(globalNameVirtualHosts);
			var listen = bindings
				.Select(b => b.Listen)
				.Distinct()
				.Except(globalListen);

			if (!allNameVirtualHosts)
			{
				foreach (var nvhost in nameVirtualHosts) conf.NameVirtualHost = nvhost;
			}

			foreach (var addr in listen) conf.Listen = addr;

			var vhosts = bindingsByAddress.Select(b =>
			{
				var hosts = b.Where(b2 => b2.Host != null);
				var name = hosts.FirstOrDefault()?.Host;
				var alias = string.Join(" ", hosts.Skip(1).Select(b2 => b2.Host).ToArray());
				if (string.IsNullOrEmpty(alias)) alias = null;
				var vhost = new VirtualHost()
				{
					Hosts = b.Key,
					ServerName = name,
					ServerAlias = alias,
					DocumentRoot = site.ContentPath
					//TODO config site
				};
				// config site
				if (site.EnableDirectoryBrowsing) vhost.Options = "Indexes";
				if (!string.IsNullOrEmpty(site.DefaultDocs))
				{
					vhost.DirectoryIndex = Regex.Replace(site.DefaultDocs, @"\s*(?:[,;]|\r?\n)\s*", " ", RegexOptions.Multiline);
				}
				
				site.CreatedDate = conf.Created;
				site.Apache = true;

				if (site.EnableDynamicCompression)
				{
					//TODO enable dynamic compression
				}
				if (site.EnableStaticCompression)
				{
					//TODO enable static compression
				}
				if (site.EnableWritePermissions)
				{
					//TODO enable write permissions
				}
				if (site.EnableAnonymousAccess)
				{
					//TODO enable anonymous access
				}
				if (site.EnableBasicAuthentication)
				{
					//TODO enable basic authentication
				}
				//TODO configure php, perl, phyton & mono
				return vhost;
			});

			conf.Add(new Comment("This file is auto generated by SolidCP. Do not edit."));
			conf.AddRange(vhosts);
			conf.Save();
			ChangeSiteState(site.SiteId, ServerState.Started);

			return site.SiteId;
		}

		public void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
		{
			var conf = Config(siteId);
			var dir = new Location()
			{
				Url = directory.VirtualPath,
				DocumentRoot = directory.ContentPath
				//TODO config location
			};
			var vhosts = conf.Sections.OfType<VirtualHost>();
			foreach (var vhost in vhosts) vhost.Add(dir);
			conf.Save();
			ReloadApache();
		}

		public void DeleteAppVirtualDirectory(string siteId, string directoryName)
		{
			var conf = Config(siteId);
			var locations = conf.Descendants.OfType<Location>().Where(loc => loc.Url == directoryName);
			foreach (var location in locations) location.Remove();
			conf.Save();
			ReloadApache();
		}

		public ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
		{
			throw new NotImplementedException();
		}

		public void DeleteFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public void DeleteGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public void DeleteHeliconApeFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public void DeleteHeliconApeGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public void DeleteHeliconApeUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public void DeleteSite(string siteId)
		{
			ChangeSiteState(siteId, ServerState.Stopped);
			var conf = Config(siteId);
			conf.Remove();
		}

		public void DeleteUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public void DeleteVirtualDirectory(string siteId, string directoryName)
		{
			var conf = Config(siteId);
			var url = "/" + directoryName.TrimStart('/');
			var locations = conf.Descendants
				.OfType<Location>()
				.Where(loc => loc.Url == url);
			foreach (var location in locations) location.Remove();
			conf.Save();
			ReloadApache();
		}

		public void DisableHeliconApe(string siteId)
		{
			throw new NotImplementedException();
		}

		public GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public void EnableHeliconApe(string siteId)
		{
			throw new NotImplementedException();
		}

		public void EnableLoadUserProfile()
		{
			throw new NotImplementedException();
		}

		public byte[] ExportCertificate(string serialNumber, string password)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate GenerateCSR(SSLCertificate certificate)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate GenerateRenewalCSR(SSLCertificate certificate)
		{
			throw new NotImplementedException();
		}

		public AppPoolState GetAppPoolState(string siteId)
		{
			throw new NotImplementedException();
		}

		WebAppVirtualDirectory GetAppVirtualDirectory(Location loc)
		{
			return new WebAppVirtualDirectory()
			{
				Name = loc.Url.TrimStart('/'),
				ContentPath = loc.DocumentRoot,
				//TODO read virtual directory
			};
		}
		public WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
		{
			var conf = Config(siteId);
			var locations = conf.Descendants.OfType<Location>();
			return locations
				.Select(loc => GetAppVirtualDirectory(loc))
				.ToArray();
		}

		public WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
		{
			var conf = Config(siteId);
			var url = "/" + directoryName.TrimStart('/');
			var location = conf.Descendants.OfType<Location>().FirstOrDefault(loc => loc.Url == url);
			if (location != null)
			{
				return GetAppVirtualDirectory(location);
			}
			else return null;
		}

		public SSLCertificate GetCertificate(WebSite site)
		{
			throw new NotImplementedException();
		}

		public bool GetDirectoryBrowseEnabled(string siteId)
		{
			var conf = Config(siteId);
			var vhost = conf.Sections.OfType<VirtualHost>().FirstOrDefault();
			if (vhost != null)
			{
				return vhost.Options.Contains("Indexes");
			}
			return false;
		}

		public WebFolder GetFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public List<WebFolder> GetFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public GalleryApplicationResult GetGalleryApplication(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
		{
			throw new NotImplementedException();
		}

		public GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
		{
			throw new NotImplementedException();
		}

		public GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
		{
			throw new NotImplementedException();
		}

		public GalleryCategoriesResult GetGalleryCategories(int UserId)
		{
			throw new NotImplementedException();
		}

		public GalleryLanguagesResult GetGalleryLanguages(int UserId)
		{
			throw new NotImplementedException();
		}

		public WebGroup GetGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public List<WebGroup> GetGroups(string siteId)
		{
			throw new NotImplementedException();
		}

		public HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public List<HtaccessFolder> GetHeliconApeFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebGroup GetHeliconApeGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public List<WebGroup> GetHeliconApeGroups(string siteId)
		{
			throw new NotImplementedException();
		}

		public HtaccessFolder GetHeliconApeHttpdFolder()
		{
			throw new NotImplementedException();
		}

		public HeliconApeStatus GetHeliconApeStatus(string siteId)
		{
			throw new NotImplementedException();
		}

		public HtaccessUser GetHeliconApeUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public List<HtaccessUser> GetHeliconApeUsers(string siteId)
		{
			throw new NotImplementedException();
		}

		public List<SSLCertificate> GetServerCertificates()
		{
			throw new NotImplementedException();
		}

		public WebSite GetSite(string siteId)
		{
			var conf = Config(siteId);
			var vhost = conf.Sections.OfType<VirtualHost>().FirstOrDefault();
			var enabledFile = Path.Combine(ApacheConfigPath, "sites-enabled", $"{siteId}.conf");
			var enabled = File.Exists(enabledFile);

			return new WebSite()
			{
				SiteId = siteId,
				Name = siteId,
				Bindings = GetSiteBindings(conf),
				AspInstalled = false,
				AspNetInstalled = "",
				CgiBinInstalled = false,
				ApplicationPool = "",
				ColdFusionAvailable = false,
				ColdFusionInstalled = false,
				ContentPath = vhost?.DocumentRoot,
				CreatedDate = conf.Created,
				DedicatedApplicationPool = false,
				DefaultDocs = vhost?.DirectoryIndex,
				EnableDirectoryBrowsing = vhost?.Options.Contains("Indexes") ?? false,
				FrontPageAvailable = false,
				FrontPageInstalled = false,
				HeliconApeEnabled = false,
				HeliconApeInstalled = false,
				Id = 0,
				IIs7 = false,
				IsDedicatedIP = false,
				LogsPath = "",
				MimeMaps = new MimeMap[0],
				Php5VersionsInstalled = "",
				ParentSiteName = "",
				PerlInstalled = false,
				PhpInstalled = "",
				PythonInstalled = false,
				SecuredFoldersInstalled = false,
				SharePointInstalled = false,
				SiteState = enabled ? ServerState.Started : ServerState.Stopped,
				WebDeployPublishingAvailable = false,
				WebDeploySitePublishingEnabled = false
			};

		}
		public ServerBinding[] GetSiteBindings(string siteId) => GetSiteBindings(Config(siteId));

		ServerBinding[] GetSiteBindings(ConfigFile conf)
		{
			var vhosts = conf.Sections.OfType<VirtualHost>();
			var listens = new ConfigFile[] { GlobalConfig }
				.Concat(GlobalConfig.Descendants.OfType<IncludeFile>())
				.SelectMany(file => file.GetAll("Listen"))
				.ToArray();

			return vhosts
				.SelectMany(vhost => vhost.Hosts.Split(' ')
					.Select(host => new
					{
						VirtualHost = vhost,
						Address = host
					}))
				.SelectMany(vhost =>
				{
					var match = Regex.Match(vhost.Address, @"(?:(?<adr>[0-9.]+|\[[0-9a-fA-F:]+\]|\*)|(?<domain>[0-9a-zA-Z_\-.]+)):(?<port>[0-9]+)");
					var ip = match.Groups["adr"].Success ? match.Groups["adr"].Value : "";
					if (ip == "*" || ip == "0.0.0.0" || ip == "::") ip = "";
					var port = match.Groups["port"].Value;
					var domain = match.Groups["domain"].Success ? match.Groups["domain"].Value : "";
					var listen = $"{(ip == "" ? port : $"{ip}:{port}")} https";
					var listenv4 = $"{(ip == "" ? $"0.0.0.0:{port}" : listen)} https";
					var listenv6 = $"{(ip == "" ? $"[::]:{port}" : listen)} https";
					var ishttps = port == "443" || listens.Any(lst => lst == listen || lst == listenv4 || lst == listenv6);
					var protocol = ishttps ? "https" : "http";
					var hosts = new string[] { vhost.VirtualHost.ServerName }
						.Concat(vhost.VirtualHost.ServerAlias.Split(' '));
					return hosts.Select(host => new ServerBinding()
					{
						IP = ip,
						Host = host,
						Port = port,
						Protocol = protocol
					});
				})
				.ToArray();
		}

		public string GetSiteId(string siteName)
		{
			return siteName.Replace(' ', '-');
		}

		public string[] GetSites()
		{
			var path = Path.Combine(ApacheConfigPath, "sites-available");
			var files = System.IO.Directory.EnumerateFiles(path, "*.conf");
			var sites = files.Select(file => Path.GetFileNameWithoutExtension(file));
			return sites.ToArray();
		}

		public string[] GetSitesAccounts(string[] siteIds)
		{
			throw new NotImplementedException();
		}

		public ServerState GetSiteState(string siteId)
		{
			var enabledFile = Path.Combine(ApacheConfigPath, "sites-enabled", $"{siteId}.conf");
			var enabled = File.Exists(enabledFile);
			return enabled ? ServerState.Started : ServerState.Stopped;
		}

		public WebUser GetUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public List<WebUser> GetUsers(string siteId)
		{
			throw new NotImplementedException();
		}

		public WebVirtualDirectory[] GetVirtualDirectories(string siteId)
		{
			var conf = Config(siteId);
			var locations = conf.Descendants.OfType<Location>();
			return locations.Select(loc => GetVirtualDirectory(loc)).ToArray();
		}

		public WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
		{
			var conf = Config(siteId);
			var location = conf.Descendants.OfType<Location>().FirstOrDefault(loc => loc.Url == directoryName);
			return GetVirtualDirectory(location);
		}

		public WebVirtualDirectory GetVirtualDirectory(Location loc)
		{
			if (loc == null) return null;

			var dir = new WebVirtualDirectory()
			{
				Name = loc.Url,
				CreatedDate = loc.Root.Created,
				ContentPath = loc.DocumentRoot,
				IIs7 = false,
				Apache = true
			};
			return dir;
		}
		public WebAppVirtualDirectory[] GetZooApplications(string siteId)
		{
			throw new NotImplementedException();
		}

		public void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword)
		{
			throw new NotImplementedException();
		}

		public void GrantWebManagementAccess(string siteId, string accountName, string accountPassword)
		{
			throw new NotImplementedException();
		}

		public void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate ImportCertificate(WebSite website)
		{
			throw new NotImplementedException();
		}

		public void InitFeeds(int UserId, string[] feeds)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate InstallCertificate(SSLCertificate certificate, WebSite website)
		{
			throw new NotImplementedException();
		}

		public bool InstallFrontPage(string siteId, string username, string password)
		{
			throw new NotImplementedException();
		}

		public StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
		{
			throw new NotImplementedException();
		}

		public void InstallHeliconApe(string ServiceId)
		{
			throw new NotImplementedException();
		}

		public SSLCertificate InstallPFX(byte[] certificate, string password, WebSite website)
		{
			throw new NotImplementedException();
		}

		public void InstallSecuredFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public bool IsColdFusionSystemInstalled()
		{
			throw new NotImplementedException();
		}

		public bool IsFrontPageInstalled(string siteId)
		{
			throw new NotImplementedException();
		}

		public bool IsFrontPageSystemInstalled()
		{
			throw new NotImplementedException();
		}

		public bool IsMsDeployInstalled()
		{
			throw new NotImplementedException();
		}

		public string LEInstallCertificate(WebSite website, string email)
		{
			throw new NotImplementedException();
		}

		public void RevokeWebDeployPublishingAccess(string siteId, string accountName)
		{
			throw new NotImplementedException();
		}

		public void RevokeWebManagementAccess(string siteId, string accountName)
		{
			throw new NotImplementedException();
		}

		public void SetDirectoryBrowseEnabled(string siteId, bool enabled)
		{
			var conf = Config(siteId);
			var vhosts = conf.Sections.OfType<VirtualHost>();
			foreach (var vhost in vhosts)
			{
				if (enabled)
				{
					if (!vhost.Options.Contains("Indexes")) vhost.Options = vhost.Options + " Indexes";
				}
				else
				{
					vhost.Options = vhost.Options.Replace("Indexes", "").Replace("  ", " ");
				}
			}
		}

		public void SetResourceLanguage(int UserId, string resourceLanguage)
		{
			throw new NotImplementedException();
		}

		public StringResultObject SetZooConsoleDisabled(string siteId, string appName)
		{
			throw new NotImplementedException();
		}

		public StringResultObject SetZooConsoleEnabled(string siteId, string appName)
		{
			throw new NotImplementedException();
		}

		public StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
		{
			throw new NotImplementedException();
		}

		public bool SiteExists(string siteId)
		{
			var path = Path.Combine(ApacheConfigPath, "sites-available", $"{siteId}.conf");
			return File.Exists(path);
		}

		public void UninstallFrontPage(string siteId, string username)
		{
			throw new NotImplementedException();
		}

		public void UninstallSecuredFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			var conf = Config(siteId);
			var location = conf.Descendants.OfType<Location>().FirstOrDefault(loc => loc.Url == directory.VirtualPath);
			if (location != null)
			{

			}
			conf.Save();
			ReloadApache();
		}

		public void UpdateFolder(string siteId, WebFolder folder)
		{
			throw new NotImplementedException();
		}

		public void UpdateGroup(string siteId, WebGroup group)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeGroup(string siteId, WebGroup group)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeHttpdFolder(HtaccessFolder folder)
		{
			throw new NotImplementedException();
		}

		public void UpdateHeliconApeUser(string siteId, HtaccessUser user)
		{
			throw new NotImplementedException();
		}

		public void UpdateSite(WebSite site)
		{
			var conf = Config(site.Name);
		}

		public void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
		{
			throw new NotImplementedException();
		}

		public void UpdateUser(string siteId, WebUser user)
		{
			throw new NotImplementedException();
		}

		public void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
		{
			throw new NotImplementedException();
		}

		public bool VirtualDirectoryExists(string siteId, string directoryName)
		{
			var conf = Config(siteId);
			var url = "/" + directoryName.TrimStart('/');
			return conf.Descendants.OfType<Location>().Any(loc => loc.Url == url);
		}

		protected bool IsInstalled(string version)
		{
			var processes = Process.GetProcessesByName("httpd")
				.Concat(Process.GetProcessesByName("apache2"))
				.Select(p => p.ExecutableFile())
				.Concat(new string[] { Shell.Default.Find("httpd"), Shell.Default.Find("apache2") })
				.Where(exe => exe != null)
				.Distinct();
			foreach (var exe in processes)
			{
				if (File.Exists(exe))
				{
					try
					{
						var output = Shell.Exec($"\"{exe}\" -V").Output().Result;
						var match = Regex.Match(output, @"^Server [Vv]ersion:\s*[A-Za-z]*/(?<version>[0-9][0-9.]+)", RegexOptions.Multiline);
						if (match.Success)
						{
							var ver = match.Groups["version"].Value;
							if (ver.StartsWith(version)) return true;
						}
					}
					catch { }
				}
			}
			return false;
		}

		public override bool IsInstalled() => IsInstalled("2.4");
	}
}