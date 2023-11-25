namespace SolidCP.UniversalInstaller {

    [Flags]
    public enum Packages { Server, EnterpriseServer, WebPortal }

    public abstract class UI {

        static UI current;
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

        public static Installer Installer { get; }
        public abstract string GetRootPassword();
        public abstract ServerSettings GetServerSettings();
        public abstract EnterpriseServerSettings GetEnterpriseServerSettings();
        public abstract WebPortalSettings GetWebPortalSettings();
        public abstract Packages GetPackagesToInstall();
        public abstract void ShowInstallationProgress();
        public abstract void CloseInstallationProgress();

    }
}