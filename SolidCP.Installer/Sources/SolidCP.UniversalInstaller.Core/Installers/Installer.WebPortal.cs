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
	public virtual void SetWebPortalFilePermissions() => SetFilePermissions(Settings.WebPortal.InstallPath);
	public virtual void SetWebPortalFileOwner() => SetFileOwner(Settings.WebPortal.InstallPath, Settings.WebPortal.Username, SolidCPGroup);
	public virtual void InstallWebPortalWebsite()
	{
		var web = Settings.WebPortal.InstallPath;
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

	public void AddAppSettings(XElement appSettings, string name, string value)
	{
		var setting = appSettings.Elements("add").FirstOrDefault(e => e.Attribute("key")?.Value == name);
		if (setting == null)
		{
			setting = new XElement("add", new XAttribute("key", name));
			appSettings.Add(setting);
		}
		setting.SetAttributeValue("value", value);
	}
	public virtual void UpdateWebPortalConfig()
	{
		var configFile = Path.Combine(Settings.EnterpriseServer.InstallPath, "Web.config");
		var config = XElement.Load(configFile);
		var sections = config.Element("configSections");

		// Remove WSE3
		sections
			?.Elements("section")
			.FirstOrDefault(e => e.Attribute("name")?.Value == "microsoft.web.services3")
			?.Remove();
		config
			.Elements("microsoft.web.services3")
			.FirstOrDefault()
			?.Remove();

		// Add SwaggerWCF
		sections.Add(new XElement("section",
			new XAttribute("name", "swaggerwcf"),
			new XAttribute("type", "SwaggerWcf.Configuration.SwaggerWcfSection, SwaggerWcf")));
		var swaggerwcf = XElement.Parse(@"<swaggerwcf>
		<settings>
			<setting name=""InfoDescription"" value=""SolidCP EnterpriseServer Service"" />
			<setting name=""InfoVersion"" value=""2.0.0"" />
			<setting name=""InfoTermsOfService"" value=""Terms of Service"" />
			<setting name=""InfoTitle"" value=""SolidCP EnterpriseServer Service"" />
			<setting name=""InfoContactName"" value=""SolidCP"" />
			<setting name=""InfoContactUrl"" value=""http://solidcp.com/forum"" />
			<setting name=""InfoContactEmail"" value=""support@solidcp.com"" />
			<setting name=""InfoLicenseUrl"" value=""https://github.com/FuseCP/SolidCP/blob/master/LICENSE.txt"" />
			<setting name=""InfoLicenseName"" value=""Creative Commons Share-alike"" />
		</settings>
	</swaggerwcf>");
		config.Add(swaggerwcf);

		// Adjust httpRuntime
		var systemWeb = config.Element("system.web");
		var httpRuntime = systemWeb?.Element("httpRuntime");
		if (httpRuntime == null) systemWeb.Add(httpRuntime = new XElement("httpRuntime"));
		var requestLength = httpRuntime.Attribute("maxRequestLength");
		if (requestLength == null)
		{
			httpRuntime.Add(requestLength = new XAttribute("maxRequestLength", 134217728));
		}
		else requestLength.SetValue(134217728);
		var pages = systemWeb?.Element("pages");
		var asyncTimeout = pages.Attribute("asyncTimeout");
		if (asyncTimeout == null) pages.Add(asyncTimeout = new XAttribute("asyncTimeout", "120"));
		asyncTimeout.SetValue("120");

		systemWeb?.Element("webServices")?.Remove();

		// Block bin_dotnet
		var location = XElement.Parse(@"<location path=""bin_dotnet"">
        <system.webServer>
            <handlers>
                <add name=""DisallowServe"" path=""*.*"" verb=""*"" type=""System.Web.HttpNotFoundHandler"" />
                <!-- Return 404 instead of 403 -->
            </handlers>
        </system.webServer>
    </location>");
		config.Add(location);

		// Add system.webServers
		var systemWebServer = XElement.Parse(@"<system.webServer>
        <validation validateIntegratedModeConfiguration=""false""/>
        <modules runAllManagedModulesForAllRequests=""true""/>
        <handlers>
            <add name=""ReportViewerWebControlHandler"" preCondition=""integratedMode"" verb=""*"" path=""Reserved.ReportViewerWebControl.axd"" type=""Microsoft.Reporting.WebForms.HttpHandler, Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a""/>
            <add name=""ChartImg"" path=""ChartImg.axd"" verb=""GET,HEAD,POST"" type=""System.Web.UI.DataVisualization.Charting.ChartHttpHandler, System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"" resourceType=""Unspecified"" preCondition=""integratedMode""/>
            <add name=""LessAssetHandler"" path=""*.less"" verb=""GET"" type=""BundleTransformer.Less.HttpHandlers.LessAssetHandler, BundleTransformer.Less"" resourceType=""File"" preCondition=""""/>
        </handlers>
        <staticContent>
            <remove fileExtension="".woff""/>
            <remove fileExtension="".woff2""/>
            <mimeMap fileExtension="".woff"" mimeType=""application/x-font-woff""/>
            <mimeMap fileExtension="".woff2"" mimeType=""application/font-woff2""/>
        </staticContent>
        <!-- hide the bin_dotnet folder -->
        <security>
            <requestFiltering>
                <hiddenSegments>
                    <add segment=""bin_dotnet""/>
                </hiddenSegments>
                <requestLimits maxAllowedContentLength=""3000000000""/>
            </requestFiltering>
        </security>
        <httpErrors errorMode=""Detailed""/>
    </system.webServer>");
		if (config.Element("system.webServer") == null) config.Add(systemWebServer);
		else config.Element("system.webServer").ReplaceWith(systemWebServer);

		// Add WCF certificate
		var systemServiceModel = config.Element("system.serviceModel");
		var environment = XElement.Parse(@"<serviceHostingEnvironment aspNetCompatibilityEnabled=""true"" />");
		if (systemServiceModel.Element("serviceHostingEnvironment") == null) systemServiceModel.Add(environment);
		var behaviors = systemServiceModel?.Element("behaviors");
		if (behaviors == null)
		{
			systemServiceModel.Add(behaviors = new XElement("behaviors"));
		}
		var serviceBehaviors = XElement.Parse(@"<serviceBehaviors>
				<behavior>
					<serviceCredentials>
						<serviceCertificate storeLocation=""LocalMachine"" storeName=""My"" x509FindType=""FindBySubjectName"" findValue=""localhost"" />
					</serviceCredentials>
				</behavior>
			</serviceBehaviors>");
		behaviors?.Add(serviceBehaviors);

		// Runtime
		var runtime = config.Element("runtime");
		if (runtime == null) config.Add(runtime = new XElement("runtime"));

		runtime.ReplaceWith(XElement.Parse(@"<runtime>
        <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
            <probing privatePath=""bin/Lazy""/>
            <dependentAssembly>
                <assemblyIdentity name=""JavaScriptEngineSwitcher.Core"" publicKeyToken=""C608B2A8CC9E4472"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-3.19.0.0"" newVersion=""3.19.0.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""MsieJavaScriptEngine"" publicKeyToken=""A3A2846A37AC0D3E"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-2.2.4.0"" newVersion=""2.2.4.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""BundleTransformer.Core"" publicKeyToken=""973C344C93AAC60D"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-1.9.171.0"" newVersion=""1.9.171.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""Newtonsoft.Json"" publicKeyToken=""30ad4fe6b2a6aeed"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-13.0.0.0"" newVersion=""13.0.0.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""WebGrease"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-1.6.5135.21930"" newVersion=""1.6.5135.21930""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""Antlr3.Runtime"" publicKeyToken=""eb42632606e9261f"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-3.5.0.2"" newVersion=""3.5.0.2""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""Microsoft.Web.Infrastructure"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-2.0.0.0"" newVersion=""2.0.0.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""JavaScriptEngineSwitcher.Core"" publicKeyToken=""c608b2a8cc9e4472"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-3.19.0.0"" newVersion=""3.19.0.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""Microsoft.IdentityModel.Abstractions"" publicKeyToken=""31bf3856ad364e35"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-8.8.0.0"" newVersion=""8.8.0.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Runtime.CompilerServices.Unsafe"" publicKeyToken=""b03f5f7f11d50a3a"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-6.0.3.0"" newVersion=""6.0.3.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Security.Cryptography.ProtectedData"" publicKeyToken=""b03f5f7f11d50a3a"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-9.0.0.3"" newVersion=""9.0.0.4""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Security.AccessControl"" publicKeyToken=""b03f5f7f11d50a3a"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-6.0.0.0"" newVersion=""6.0.0.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Threading.Tasks.Extensions"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-4.2.0.0"" newVersion=""4.2.0.1""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Text.Encodings.Web"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-9.0.0.3"" newVersion=""9.0.0.4""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.ValueTuple"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-4.0.2.0"" newVersion=""4.0.3.0""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Buffers"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral"" />
                <bindingRedirect oldVersion=""0.0.0.0-4.0.4.0"" newVersion=""4.0.5.0"" />
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Text.Json"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-9.0.0.3"" newVersion=""9.0.0.4""/>
            </dependentAssembly>
            <dependentAssembly>
                <assemblyIdentity name=""System.Diagnostics.DiagnosticSource"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral""/>
                <bindingRedirect oldVersion=""0.0.0.0-9.0.0.3"" newVersion=""9.0.0.4""/>
            </dependentAssembly>
                    <dependentAssembly>
                <assemblyIdentity name=""System.Memory"" publicKeyToken=""cc7b13ffcd2ddd51"" culture=""neutral"" />
                <bindingRedirect oldVersion=""0.0.0.0-4.0.4.0"" newVersion=""4.0.5.0"" />
            </dependentAssembly>
        </assemblyBinding>
    </runtime>"));

		File.WriteAllText(configFile, config.ToString());
	}
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
		var dir = Settings.WebPortal.InstallPath;
		if (Directory.Exists(dir)) Directory.Delete(dir, true);
		InstallLog("Removed Portal files");
	}
	public virtual void ReadWebPortalConfiguration()
	{
		var confFile = Path.Combine(Settings.WebPortal.InstallPath, "App_Data", "SiteSettings.config");
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
		var confFile = Path.Combine(Settings.WebPortal.InstallPath, "App_Data", "SiteSettings.config");
		var conf = XElement.Load(confFile);
		var enterpriseServer = conf.Element("EnterpriseServer");
		enterpriseServer.Value = settings.EmbedEnterpriseServer ? "assembly://SolidCP.EnterpriseServer" : settings.EnterpriseServerUrl;
		conf.Save(confFile);

		confFile = Path.Combine(Settings.WebPortal.InstallPath, "Web.config");
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
		var websitePath = Settings.WebPortal.InstallPath;
		InstallWebRootPath = Path.GetDirectoryName(websitePath);
		CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
	}
}