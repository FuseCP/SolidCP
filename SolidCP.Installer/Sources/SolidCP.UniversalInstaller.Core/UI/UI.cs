using System.Diagnostics;
using System.Reflection;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{

	[Flags]
	public enum Packages { Server = 1, EnterpriseServer = 2, WebPortal = 4, WebDavPortal = 8 }

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
					current = Activator.CreateInstance(new WinFormsUI();
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
		public abstract bool IsAvailable { get; }
		public static UI WinFormsUI
		{
			get
			{
				var type = Type.GetType("SolidCP.UniversalInstaller.WinFormsUI, SolidCP.UniversalInstaller.UI.WinForms");
				if (type != null) return Activator.CreateInstance(type) as UI;
				else return new NotAvailableUI();
			}
		}
		public static UI ConsoleUI => new ConsoleUI();
		public static UI AvaloniaUI {
			get {
				var type = Type.GetType("SolidCP.UniversalInstaller.AvaloniaUI, SolidCP.UniversalInstaller.UI.Avalonia");
				if (type != null) return Activator.CreateInstance(type) as UI;
				else return new NotAvailableUI();
			}
		}
		public void ShowRunningInstance()
		{
			if (!(this is ConsoleUI) && OSInfo.IsWindows)
			{
				Process currentProcess = Process.GetCurrentProcess();
				foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
				{
					if (process.Id != currentProcess.Id)
					{
						//set focus
						User32.SetForegroundWindow(process.MainWindowHandle);
						break;
					}
				}
			}
		}

		public abstract class SetupWizard
		{
			public UI UI { get; protected set; }
			public Installer Installer => UI.Installer;
			public InstallerSettings Settings => Installer.Settings;

			public SetupWizard(UI ui) => UI = ui;

			public virtual SetupWizard BannerWizard() => this;
			public virtual SetupWizard Certificate() => this;
			public virtual SetupWizard ConfigurationCheck() => this;
			public virtual SetupWizard ConfirmUninstall() => this;
			public virtual SetupWizard Database() => this;
			public virtual SetupWizard EmbeddEnterpriseServer() => this;
			public virtual SetupWizard Progress() => this;
			public virtual SetupWizard Download() => this;
			public virtual SetupWizard Finish() => this;
			public virtual SetupWizard InsecureHttpWarning() => this;
			public virtual SetupWizard InstallFolder() => this;
			public virtual SetupWizard Introduction() => this;
			public virtual SetupWizard LicenseAgreement() => this;
			public virtual SetupWizard MarginWizards() => this;
			public virtual SetupWizard Rollback() => this;
			public virtual SetupWizard ServerAdminPassword() => this;
			public virtual SetupWizard ServerPassword() => this;
			public virtual SetupWizard ServiceAddress() => this;
			public virtual SetupWizard SetupComplete() => this;
			public virtual SetupWizard SQLServers() => this;
			public virtual SetupWizard Uninstall() => this;
			public virtual SetupWizard Url() => this;
			public virtual SetupWizard UserAccount() => this;
			public virtual SetupWizard Web() => this;
			public abstract void Show();
		}

		public Installer Installer => Installer.Current;
		public InstallerSettings Settings => Installer.Settings;
		public abstract SetupWizard Wizard { get; }
		public abstract void Init();
		public abstract void Exit();
		public abstract void RunMainUI();
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
		public abstract void ShowLogFile();
		public virtual void PrintInstallerVersion()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var version = assembly.GetName().Version;
			Log.WriteLine($"SolidCP UniversalInstaller {version}");
		}

		public abstract void CheckPrerequisites();
	}
}