using System.Reflection;

namespace SolidCP.UniversalInstaller
{

	[Flags]
	public enum Packages { Server = 1, EnterpriseServer = 2, WebPortal = 4 }

	public abstract class UI
	{

		static UI current;
		public static UI Current
		{
			get
			{
				if (current == null)
				{
#if NETFRAMEWORK
               current = new WinFormsUI();
#else
					current = new ConsoleUI();
#endif
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
		public abstract void Init();
		public abstract void Exit();
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
			Installer.Log($"SolidCP UniversalInstaller {version}");
		}

		public abstract void CheckPrerequisites();
	}
}