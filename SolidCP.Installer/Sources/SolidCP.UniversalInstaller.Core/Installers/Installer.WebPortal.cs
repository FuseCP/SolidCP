using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml.Linq;

namespace SolidCP.UniversalInstaller;

public abstract partial class Installer
{
	public string WebPortalSiteId => $"{SolidCP}WebPortal";
	public virtual string UnixPortalServiceId => "solidcp-portal";
	public virtual void InstallWebPortalPrerequisites() { }
	public virtual void RemoveWebPortalPrerequisites() { }
	public virtual void CreateWebPortalUser() => CreateUser(Settings.WebPortal);
	public virtual void RemoveWebPortalUser() => RemoveUser(Settings.WebPortal.Username);
	public virtual void SetWebPortalFilePermissions() => SetFilePermissions(Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder));
	public virtual void SetWebPortalFileOwner() => SetFileOwner(Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder), Settings.WebPortal.Username, SolidCPGroup);
	public virtual void InstallWebPortalWebsite()
	{
		var web = Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder);
		var dll = Path.Combine(web, "bin_dotnet", "SolidCP.WebPortal.dll");
		InstallWebsite(WebPortalSiteId,
			web,
			Settings.WebPortal,
			SolidCPUnixGroup,
			dll,
			"SolidCP.WebPortal service, the portal service for the SolidCP control panel.",
			UnixPortalServiceId);
	}
	public virtual string WebPortalInstallFilter(string file) => SetupFilter(file);
	public virtual string WebPortalSetupFilter(string file) => ConfigAndSetupFilter(file);

	public virtual void InstallWebPortal()
	{
		InstallWebPortalPrerequisites();
		ReadWebPortalConfiguration();
		CopyWebPortal(true, WebPortalInstallFilter);
		CreateWebPortalUser();
		SetWebPortalFilePermissions();
		SetWebPortalFileOwner();
		ConfigureWebPortal();
		InstallWebPortalWebsite();
	}
	public virtual void RemoveWebPortalWebsite() {
		RemoveWebsite(WebPortalSiteId, Settings.WebPortal);
	}
	public virtual void UpdateWebPortalConfig() { }
	public virtual void UpdateWebPortal() {
		InstallWebPortalPrerequisites();
		ReadWebPortalConfiguration();
		CopyWebPortal(true, WebPortalSetupFilter);
		SetWebPortalFilePermissions();
		SetWebPortalFileOwner();
		UpdateWebPortalConfig();
		ConfigureWebPortal();
		InstallWebPortalWebsite();
	}

	public virtual void SetupWebPortal()
	{
		RemoveWebPortalWebsite();
		ConfigureWebPortal();
		InstallWebPortalWebsite();
	}
	public virtual void RemoveWebPortal()
	{
		RemoveWebPortalWebsite();
		RemoveWebPortalFolder();
		RemoveWebPortalUser();
	}
	public virtual void RemoveWebPortalFolder()
	{
		var dir = Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder);
		if (Directory.Exists(dir)) Directory.Delete(dir, true);
		InstallLog("Removed Portal files");
	}
	public virtual void ReadWebPortalConfiguration()
	{
		var confFile = Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder, "App_Data", "SiteSettings.config");
		if (File.Exists(confFile))
		{
			var conf = XElement.Load(confFile);
			var enterpriseServer = conf.Element("EnterpriseServer");
			Settings.WebPortal.EnterpriseServerUrl = enterpriseServer?.Value ?? "http://localhost:9002";
		}
	}
	public virtual void ConfigureWebPortal()
	{
		var settings = Settings.WebPortal;
		var confFile = Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder, "App_Data", "SiteSettings.config");
		var conf = XElement.Load(confFile);
		var enterpriseServer = conf.Element("EnterpriseServer");
		enterpriseServer.Value = settings.EmbedEnterpriseServer ? "assembly://SolidCP.EnterpriseServer" : settings.EnterpriseServerUrl;
		conf.Save(confFile);

		confFile = Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder, "Web.config");
		conf = XElement.Load(confFile);

		ConfigureCertificateNetFX(settings, conf);

		conf.Save(confFile);

		ConfigureAppsettings(Settings.WebPortal);

		if (settings.EmbedEnterpriseServer)
		{
			if (settings.EnterpriseServerPath.EndsWith("\\") ||
				settings.EnterpriseServerPath.EndsWith("/"))
			{
				settings.EnterpriseServerPath = settings.EnterpriseServerPath.Substring(0, settings.EnterpriseServerPath.Length - 1);
			}

			ConfigureEnterpriseServerNetFX(true);

			conf = XElement.Load(confFile);

			// add external probing paths
			var appSettings = conf.Element("appSettings");
			var paths = appSettings.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "ExternalProbingPaths");
			if (paths == null)
			{
				paths = new XElement("add", new XAttribute("key", "ExternalProbingPaths"));
				appSettings.Add(paths);
			}

			paths.Attribute("value").SetValue($@"{settings.EnterpriseServerPath}\bin;{settings.EnterpriseServerPath}\bin\Code;{settings.EnterpriseServerPath}\bin\netstandard");

			var exposews = appSettings.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == "ExposeWebServices");
			if (exposews == null)
			{
				exposews = new XElement("add", new XAttribute("key", "ExposeWebServices"));
				appSettings.Add(exposews);
			}
			exposews.Attribute("value").SetValue(settings.ExposeEnterpriseServerWebServices ? "EnterpriseServer" : "false");
		}

		conf.Save(confFile);

		InstallLog("Configured Web Portal.");
	}
	public virtual void CopyWebPortal(bool clearDestination = false, Func<string, string> filter = null)
	{
		filter ??= SetupFilter;
		InstallWebRootPath = Settings.WebPortal.InstallFolder;
		var websitePath = Path.Combine(Settings.WebPortal.InstallFolder, WebPortalFolder);
		CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
	}
}