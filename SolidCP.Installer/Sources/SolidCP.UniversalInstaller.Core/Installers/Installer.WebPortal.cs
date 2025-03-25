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
	public virtual void SetWebPortalFilePermissions() => SetFilePermissions(WebPortalFolder);
	public virtual void SetWebPortalFileOwner() => SetFileOwner(WebPortalFolder, Settings.WebPortal.Username, SolidCP.ToLower());
	public virtual void InstallWebPortalWebsite()
	{
		var web = Path.Combine(InstallWebRootPath, WebPortalFolder);
		var dll = Path.Combine(web, "bin_dotnet", "SolidCP.WebPortal.dll");
		InstallWebsite(WebPortalSiteId,
			web,
			Settings.WebPortal,
			UnixPortalServiceId,
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
		CopyWebPortal(WebPortalInstallFilter);
		SetWebPortalFilePermissions();
		SetWebPortalFileOwner();
		ConfigureWebPortal();
		InstallWebPortalWebsite();
	}
	public virtual void RemoveWebPortalWebsite() {
		RemoveWebsite(WebPortalSiteId, Settings.WebPortal);
	}
	public virtual void UpdateWebPortal() {
		InstallWebPortalPrerequisites();
		ReadWebPortalConfiguration();
		CopyWebPortal(WebPortalSetupFilter);
		SetWebPortalFilePermissions();
		SetWebPortalFileOwner();
		UpdateWebPortalConfig();
		ConfigureWebPortal();
		InstallWebPortalWebsite();
	}
	public virtual void RemoveWebPortal()
	{
		RemoveWebPortalWebsite();
		RemoveWebPortalFolder();
	}
	public virtual void RemoveWebPortalFolder()
	{
		Directory.Delete(Path.Combine(InstallWebRootPath, WebPortalFolder), true);
	}
	public virtual void ReadWebPortalConfiguration()
	{
		var confFile = Path.Combine(InstallWebRootPath, WebPortalFolder, "App_Data", "SiteSettings.config");
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
		var confFile = Path.Combine(InstallWebRootPath, WebPortalFolder, "App_Data", "SiteSettings.config");
		var conf = XElement.Load(confFile);
		var enterpriseServer = conf.Element("EnterpriseServer");
		enterpriseServer.Value = settings.EmbedEnterpriseServer ? "assembly://SolidCP.EnterpriseServer" : settings.EnterpriseServerUrl;
		conf.Save(confFile);

		confFile = Path.Combine(InstallWebRootPath, WebPortalFolder, "web.config");
		conf = XElement.Load(confFile);

		ConfigureCertificateNetFX(settings, conf);

		ConfigureAppsettings(Settings.WebPortal);

		if (settings.EmbedEnterpriseServer)
		{
			ConfigureEnterpriseServerNetFX(true);

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
	public virtual void UpdateWebPortalConfig() { }
	public virtual void CopyWebPortal(Func<string, string> filter = null)
	{
		filter ??= SetupFilter;
		var websitePath = Path.Combine(InstallWebRootPath, WebPortalFolder);
		CopyFiles(ComponentTempPath, websitePath, filter);
	}
}