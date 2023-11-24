namespace SolidCP.Installer {

    [Flags]
    public enum Packages { Server, EnterpriseServer, WebPortal }

    public class UI {

        static UI current;
        public static UI Current
        {
            get
            {
                if (current == null)
                {
                    current = new WinFormsUI();
                }
                return current;
            }
            protected set
            {
                current = value;
            }
        }

        public static Installer Installer { get; }

        public abstract ServerSettings GetServerSettings();
        public abstract EnterpriseServerSettings GetEnterpriseServerSettings();
        public abstract WebPortalSettings GetWebPortalSettings();
        public abstract Packages GetPackagesToInstall();
        public abstract void ShowInstallationProgress();

    }
}