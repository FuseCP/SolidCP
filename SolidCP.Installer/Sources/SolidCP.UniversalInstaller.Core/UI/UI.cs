using System.Diagnostics;
using System.Reflection;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{

	[Flags]
	public enum Packages { None = 0, Server = 1, EnterpriseServer = 2, WebPortal = 4, WebDavPortal = 8, All = 15 }

	public abstract class UI
	{
		static UI current;
		public static UI Current
		{
			get
			{
				if (current == null)
				{
					var naUI = new NotAvailableUI();
					UI ui = AvaloniaUI;
					//if (!ui.IsAvailable) ui = WinFormsUI;
					if (!ui.IsAvailable) ui = ConsoleUI;
					if (!ui.IsAvailable) ui = naUI;
					current = ui;
				}
				return current;
			}
			protected set
			{
				current = value;
			}
		}

		public static UI Set(string name)
		{
			switch (name)
			{
				case nameof(AvaloniaUI): Current = AvaloniaUI; break;
				case nameof(WinFormsUI): Current = WinFormsUI; break;
				case nameof(NotAvailableUI): Current = new NotAvailableUI(); break;
				default:
				case nameof(ConsoleUI): Current = ConsoleUI; break;
			}
			return Current;
		}

		public abstract bool IsAvailable { get; }
		public static UI WinFormsUI
		{
			get
			{
				var type = Type.GetType("SolidCP.UniversalInstaller.WinFormsUI, SolidCP.UniversalInstaller.UI.WinForms");
				if (type != null) return Activator.CreateInstance(type) as UI;
				else return NotAvailableUI;
			}
		}

		static UI consoleUI = null;
		public static UI ConsoleUI => consoleUI ??= new ConsoleUI();

		static UI notAvailableUI = null;
		public static UI NotAvailableUI => notAvailableUI ??= new NotAvailableUI();

		public static UI AvaloniaUI {
			get {
				var type = Type.GetType("SolidCP.UniversalInstaller.AvaloniaUI, SolidCP.UniversalInstaller.UI.Avalonia");
				if (type != null) return Activator.CreateInstance(type) as UI;
				else return NotAvailableUI;
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
			public virtual SetupWizard CheckPrerequisites() => this;
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
			public abstract bool Show();
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
			Console.WriteLine($"SolidCP UniversalInstaller {version}");
			Log.WriteLine($"SolidCP UniversalInstaller {version}");
		}

		public abstract void CheckPrerequisites();

		public abstract void ShowWarning(string msg);
		public abstract bool DownloadSetup(string fileName);
		public abstract bool ExecuteSetup(string path, string installerType, string method, object[] args);
		public object MainForm { get; }
		public bool IsConsole => this == ConsoleUI;
	}
}