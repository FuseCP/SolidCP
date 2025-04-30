using System.Diagnostics;
using System.Reflection;
using System.Collections;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{	
	[Flags]
	public enum Packages { None = 0, Server = 1, EnterpriseServer = 2, WebPortal = 4, WebDavPortal = 8, All = 15 }

	public abstract class UI
	{
		public const bool UseWinForms = true;

		static UI current;
		public static UI Current
		{
			get
			{
				if (current == null)
				{
					var naUI = new NotAvailableUI();
					UI ui = naUI;
					if (!ui.IsAvailable && UseWinForms) ui = WinFormsUI;
					if (!ui.IsAvailable) ui = AvaloniaUI;
					if (!ui.IsAvailable) ui = ConsoleUI;
					if (!ui.IsAvailable) ui = naUI;
					current = ui;
					Installer.Current.Settings.Installer.UI = ui.GetType().Name;
				}
				return current;
			}
			set
			{
				current = value;
			}
		}

		public static UI SetCurrent(string name)
		{
			switch (name)
			{
				case nameof(AvaloniaUI): Current = AvaloniaUI; break;
				case nameof(WinFormsUI): Current = WinFormsUI; break;
				case nameof(NotAvailableUI): Current = new NotAvailableUI(); break;
				default:
				case nameof(ConsoleUI): Current = ConsoleUI; break;
			}
			Installer.Current.Settings.Installer.UI = name;
			return Current;
		}

		public abstract bool IsAvailable { get; }

		static UI winFormsUI = null;
		public static UI WinFormsUI
		{
			get
			{
				if (winFormsUI == null)
				{
					var type = Installer.Current.GetType($"SolidCP.UniversalInstaller.WinFormsUI, SolidCP.UniversalInstaller.UI.WinForms.{
						(OSInfo.IsNetFX ? "NetFX" : "NetCore")}");
					if (type != null) winFormsUI = Activator.CreateInstance(type) as UI;
					else winFormsUI = NotAvailableUI;
				}
				return winFormsUI;
			}
		}

		static UI consoleUI = null;
		public static UI ConsoleUI {
			get {
				if (consoleUI == null)
				{
					var type = Installer.Current.GetType("SolidCP.UniversalInstaller.ConsoleUI, SolidCP.UniversalInstaller.UI.Console");
					if (type != null) consoleUI = Activator.CreateInstance(type) as UI;
					else consoleUI = NotAvailableUI;
				}
				return consoleUI;
			}
		}

		static UI notAvailableUI = null;
		public static UI NotAvailableUI => notAvailableUI ??= new NotAvailableUI();

		static UI avaloniaUI = null;
		public static UI AvaloniaUI {
			get
			{
				if (avaloniaUI == null)
				{
					var type = Installer.Current.GetType("SolidCP.UniversalInstaller.AvaloniaUI, SolidCP.UniversalInstaller.UI.Avalonia");
					if (type != null) avaloniaUI = Activator.CreateInstance(type) as UI;
					else avaloniaUI = NotAvailableUI;
				}
				return avaloniaUI;
			}
		}
		public bool IsConsole => this == ConsoleUI;
		public bool IsWinForms => this == WinFormsUI;
		public bool IsAvalonia => this == AvaloniaUI;
		public bool IsGraphical => !IsConsole && this != NotAvailableUI;

		public void ShowRunningInstance()
		{
			if (OSInfo.IsWindows && IsGraphical)
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

			public virtual SetupWizard Introduction(ComponentSettings settings) => this;
			public virtual SetupWizard Certificate(CommonSettings settings) => this;
			public virtual SetupWizard CheckPrerequisites() => this;
			public virtual SetupWizard ConfirmUninstall(ComponentSettings settings) => this;
			public virtual SetupWizard Database() => this;
			public virtual SetupWizard EmbedEnterpriseServer() => this;
			public virtual SetupWizard EnterpriseServerUrl() => this;
			public virtual SetupWizard Progress() => this;
			public virtual SetupWizard Download() => this;
			public virtual SetupWizard Finish() => this;
			public virtual SetupWizard InsecureHttpWarning(CommonSettings settings) => this;
			public virtual SetupWizard InstallFolder(ComponentSettings settings) => this;
			public virtual SetupWizard LicenseAgreement() => this;
			public virtual SetupWizard ServerAdminPassword() => this;
			public virtual SetupWizard ServerPassword() => this;
			public virtual SetupWizard RunWithProgress(string title, Action action, ComponentSettings settings) => this;
			public virtual SetupWizard UserAccount(CommonSettings settings) => this;
			public virtual SetupWizard Web(CommonSettings settings) => this;
			public abstract bool Show();
		}

		public Installer Installer => Installer.Current;
		public InstallerSettings Settings => Installer.Settings;
		public abstract SetupWizard Wizard { get; }
		public abstract void Init();
		public abstract void Exit();
		public abstract void RunMainUI();
		public abstract string GetRootPassword();
		public abstract void ShowError(Exception ex);
		public abstract void ShowLogFile();
		public virtual void ShowWaitCursor() { }
		public virtual void EndWaitCursor() { }
		public virtual void PassArguments(Hashtable args) { }
		public virtual void ReadArguments(Hashtable args) { }
		public virtual void PrintInstallerVersion()
		{
			var version = Installer.Current.Version;
			Console.WriteLine($"SolidCP UniversalInstaller {version}");
			Log.WriteLine($"SolidCP UniversalInstaller {version}");
		}

		public abstract void ShowWarning(string msg);
		public abstract bool DownloadSetup(RemoteFile file, bool setupOnly = false);
		public virtual bool ExecuteSetup(string path, string installerType, string method, object[] args)
		{
			var res = (Result)Installer.Current.LoadContext.Execute(path, installerType, method, new object[] { args });
			Log.WriteInfo(string.Format("Installer returned {0}", res));
			Log.WriteEnd("Installer finished");
			
			EndWaitCursor();
			
			return res == Result.OK;
		}
		public abstract void DownloadInstallerUpdate();
		public abstract bool CheckForInstallerUpdate(bool appStartup = false);
		public virtual object MainForm { get; set; }
	}
}