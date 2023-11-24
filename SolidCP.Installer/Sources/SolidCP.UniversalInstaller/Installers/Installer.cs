using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using Ionic.Zip;
using System.Globalization;
using System.Security.Policy;

namespace SolidCP.UniversalInstaller
{


    public class ServerSettings
    {
        public string Urls { get; set; }
        public string ServerUser { get; set; }
        public string ServerUserPassword { get; set; }
        public string ServerPassword { get; set; }

    }

    public class EnterpriseServerSettings
    {

    }

    public class WebPortalSettings
    {

    }

    public abstract class Installer
    {
        public virtual string ServerFolder => "Server";
        public virtual string EnterpriseServerFolder => "EnterpriseServer";
        public virtual string WebPortalFolder => "Portal";
        public virtual string ServerUser => "Server";
        public virtual string EnterpriseServerUser => "EnterpriseServer";
        public virtual string WebPortalUser => "Portal";

        public bool Net8RuntimeAllreadyInstalled = false;
		public virtual string InstallWebRootPath { get; set; }
        public virtual string InstallExeRootPath { get; set; }

        public ServerSettings ServerSettings { get; set; }
        public EnterpriseServerSettings EnterpriseServerSettings { get; set; }
        public WebPortalSettings WebPortalSettings { get; set; }

        public Shell Shell { get; set; } = OSInfo.Current.DefaultShell.Clone;
        public Providers.OS.Installer OSInstaller => OSInfo.Current.DefaultInstaller;
        public IWebServer WebServer => OSInfo.Current.WebServer;
        public ServiceController ServiceController => OSInfo.Current.ServiceController;

        public abstract void InstallGlobalPrerequisites();
        public abstract bool IsGlobalPrerequisitesInstalled();
        public virtual bool GlobalPrerequisitesWereInstalled { get; set; }
        public abstract void InstallNet8Runtime();
        public abstract void RemoveNet8Runtime();
        void InstallGlobalPrerequisitesConditionally()
        {
            GlobalPrerequisitesWereInstalled = IsGlobalPrerequisitesInstalled();
            if (!GlobalPrerequisitesWereInstalled) InstallGlobalPrerequisites();
        }

        public virtual void InstallServerPrerequisites() { }

        public virtual void InstallServer()
        {
            InstallGlobalPrerequisitesConditionally();
            InstallServerPrerequisitesConditionally();
            UnzipServer();
            InstallServerWebsite();
            InstallServerUser();
            InstallSetServerFilePermissions();
            ConfigureServer();
        }

        public abstract string WebsiteLogsPath { get; }
        public virtual void InstallServerUser()
        {
        }
        public virtual void InstallServerWebsite()
        {
            if (OSInfo.IsWindows)
            {
                var site = new WebSite()
                {
                    ContentPath = Path.Combine(InstallWebRootPath, ServerFolder),
                    AspNetInstalled = "",
                    ApplicationPool = "",
                    DedicatedApplicationPool = true,
                    EnableAnonymousAccess = true,
                    EnableBasicAuthentication = true,
                    EnableDynamicCompression = false,
                    EnableWritePermissions = false,
                    Name = "SolidCP.Server",
                    LogsPath = WebsiteLogsPath,
                };
                site.Bindings = ServerSettings.Urls
                    .Split(';')
                    .Select(url =>
                    {
                        var uri = new Uri(url);
                        string ip = uri.Host;

                        return new ServerBinding(uri.Scheme, "0.0.0.0", uri.Port.ToString(), uri.Host);
                    })
                    .ToArray();
                WebServer.CreateSite(site);
            }
            else
            {
                // run SolidCP.Server as a service on Unix
                var service = new ServiceDescription()
                {
                    ServiceId = "SolidCP.Server",
                    Description = "SolidCP.Server service",
                    Executable = "dotnet SolidCP.Server.dll"
                };
                ServiceController.Install(service);
                ServiceController.Enable(service.ServiceId);
                ServiceController.Start(service.ServiceId);
            }
        }
        public virtual void InstallEnterpriseServer()
        {
            InstallGlobalPrerequisitesConditionally();
            InstallEnterpriseServerPrerequisites();
            InstallEnterpriseServerWebsite();
        }
        public virtual void InstallWebPortal();

        public ServerSettings ReadServerConfiguration();
        public EnterpriseServerSettings ReadEnterpriseServerConfiguration();
        public WebPortalServerSettings ReadWebPortalConfiguration();

        public void ConfigureServer(ServerSettings settings)
        {
        }
        public void ConfigureEnterpriseServer(EnterpriseServerSettings settings)
        {

        }
        public void ConfigureWebPortal(WebPortalSettings settings)
        {

        }

        public virtual void InstallAll()
        {

        }


        public virtual void UnzipServer()
        {
            UnzipFromResource("SolidCP.Server.zip", Path.Combine(InstallWebRootPath, ServerFolder));
        }

        public virtual void UnzipEnterpriseServer()
        {
            UnzipFromResource("SolidCP.EnterpriseServer.zip", Path.Combine(InstallWebRootPath, EnterpriseServerFolder));
        }

        public virtual void UnzipPortal()
        {
            UnzipFromResource("SolidCP.WebPortal.zip", Path.Combine(InstallWebRootPath, WebPortalFolder));
        }

        public void UnzipFromResource(string resourcePath, string destinationPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var resource = assembly.GetManifestResourceStream(resourcePath))
            using (var zip = ZipFile.Read(resource))
            {
                foreach (var zipEntry in zip)
                {
                    zipEntry.Extract(destinationPath);
                }
            }
        }
    }
}