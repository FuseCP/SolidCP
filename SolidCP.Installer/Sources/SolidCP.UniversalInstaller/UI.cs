namespace SolidCP.Installer {

    [Flags]
    public enum Packages = {Server, EnterpriseServer, WebPortal }

    public class UI {

        public abstract ServerSettings GetServerSettings();
        public abstract EnterpriseServerSettings GetEnterpriseServerSettings();
        public abstract WebPortalSettings GetWebPortalSettings();
        public abstract Packages GetPackagesToInstall();
        public abstract ShowInstallationProgress();

    }
}