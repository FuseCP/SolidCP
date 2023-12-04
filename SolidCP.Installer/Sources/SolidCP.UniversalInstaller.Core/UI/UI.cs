using System.Reflection;

namespace SolidCP.UniversalInstaller {

    [Flags]
    public enum Packages { Server, EnterpriseServer, WebPortal }

    public abstract class UI {

        static UI? current;
        public static UI Current
        {
            get
            {
                if (current == null)
                {
                    current = new ConsoleUI();
                }
                return current;
            }
            protected set
            {
                current = value;
            }
        }

        public Installer Installer => Installer.Current;
        public ServerSettings ServerSettings => Installer.ServerSettings;
        public EnterpriseServerSettings EnterpriseServerSettings => Installer.EnterpriseServerSettings;
        public WebPortalSettings WebPortalSettings => Installer.WebPortalSettings;
        public abstract string GetRootPassword();
        public abstract ServerSettings GetServerSettings();
        public abstract EnterpriseServerSettings GetEnterpriseServerSettings();
        public abstract WebPortalSettings GetWebPortalSettings();
        public abstract void GetCommonSettings(CommonSettings settings);
        public abstract Packages GetPackagesToInstall();
        public abstract void ShowInstallationProgress();
        public abstract void CloseInstallationProgress();
        public abstract void ShowError(Exception ex);
        public abstract void ShowInstallationSuccess(Packages packages);
        public virtual void PrintInstallerVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            Installer.Log($"SolidCP Installer {version}");
        }

    }
}