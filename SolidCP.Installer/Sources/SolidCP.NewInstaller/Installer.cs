namespace SolidCP.Installer {


    public class ServerSettings {

    }

    public class EnterpriseServerSettings {

    }

    public class WebPortalSettings {

    }

    public abstract class Installer {

        public ServerSettings ServerSettings { get; set; }
        public EnterpriseServerSettings EnterpriseServerSettings { get; set; }
        public WebPortalSettings WebPortalSettings { get; set; }

        bool globalPrerequisitesAreInstalled = false;

        Providers.OS.Installer OSInstaller => Server.Utils.OS.Current.DefaultInstaller;
        WebServer WebServer => Server.Utils.OS.Current.WebServer;

        public abstract void InstallGlobalPrerequisites();

        void InstallGlobalPrerequisitesConditionally()
        {
            if (globalPrerequisitesAreInstalled) {
                InstallGlobalPrerequisites();
                globalPrerequisitesAreInstalled = false;
            }
        }

        public virtual void InstallServerPrerequisites() { }

        public virtual void InstallServer() {
            InstallGlobalPrerequisitesConditionally();
            InstallServerPrerequisites();
            InstallServerWebsite();
            InstallServerUser();
            SetServerFilePermissions();
            ConfigureServer();
        };
        public virtual void InstallEnterpriseServer() {
            InstallGlobalPrerequisitesConditionally();
            InstallEnterpriseServerPrerequisites();
            InstallEnterpriseServerWebsite();
        };
        public virtual void InstallWebPortal();

        public ServerSettings ReadServerConfiguration();
        public EnterpriseServerSettings ReadEnterpriseServerConfiguration();
        public WebPortalServerSettings ReadWebPortalConfiguration();

        public void ConfigureServer(ServerSettings settings) {
        }
        public void ConfigureEnterpriseServer(EnterpriseServerSettings settings) {

        }
        public ConfigureWebPortal(WebPortalSettings settings) {

        }

        public virtual InstallAll() {
            InstallServer();
            if (OS.IsWindows) InstallEnterpriseServer();
            if (OS.IsWindows) InstallWebPortal();
        }
    }
}