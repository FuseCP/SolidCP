using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Renci.SshNet.Messages;
using SolidCP.Providers.OS;
using SolidCP.Updater;


namespace SolidCP.UniversalInstaller {

    public class WinFormsUI: UI {
		const string WaitCursorIncrementFileName = "WaitCursor.cancel";
		string WaitCursorIncrementFile => Path.Combine(Settings.Installer.TempPath, WaitCursorIncrementFileName);

		public new class SetupWizard : UI.SetupWizard
		{
			public WinForms.InstallerForm Form { get; private set; } = new WinForms.InstallerForm();	
			public WinForms.Wizard Wizard => Form.Wizard;
			public SetupWizard(UI ui): base(ui) { }

			void Add(Control page) => Wizard.Controls.Add(page);
			public override UI.SetupWizard Certificate(CommonSettings settings)
			{
				Add(new WinForms.CertificatePage(settings));
				return this;
			}
			public override UI.SetupWizard ConfirmUninstall(ComponentSettings settings)
			{
				Add(new WinForms.ConfirmUninstallPage() { Settings = settings });
				return this;
			}
			public override UI.SetupWizard CheckPrerequisites()
			{
				Add(new WinForms.ConfigurationCheckPage());
				return this;
			}
			public override UI.SetupWizard Database()
			{
				Add(new WinForms.DatabasePage());
				return this;
			}
			public override UI.SetupWizard EmbedEnterpriseServer()
			{
				Add(new WinForms.EmbedEnterpriseServerPage());
				return this;
			}
			public override UI.SetupWizard EnterpriseServerUrl()
			{
				Add(new WinForms.EnterpriseServerUrlPage());
				return this;
			}
			public override UI.SetupWizard Finish()
			{
				Add(new WinForms.FinishPage());
				return this;
			}
			public override UI.SetupWizard InsecureHttpWarning(CommonSettings settings)
			{
				Add(new WinForms.InsecureHttpWarningPage() { Settings = settings });
				return this;
			}
			public override UI.SetupWizard InstallFolder(ComponentSettings settings)
			{
				Add(new WinForms.InstallFolderPage() { Settings = settings });
				return this;
			}
			public override UI.SetupWizard Introduction(ComponentSettings settings)
			{
				Add(new WinForms.IntroductionPage() { Settings = settings });
				return this;
			}
			public override UI.SetupWizard LicenseAgreement()
			{
				Add(new WinForms.LicenseAgreementPage());
				return this;
			}
			public override UI.SetupWizard ServerAdminPassword()
			{
				Add(new WinForms.ServerAdminPasswordPage());
				return this;
			}
			public override UI.SetupWizard RunWithProgress(string title, Action action, ComponentSettings settings)
			{
				Add(new WinForms.ProgressPage() { Maximum = 1000, Settings = settings, Action = action });
				return this;
			}
			public override UI.SetupWizard ServerPassword()
			{
				Add(new WinForms.ServerPasswordPage());
				return this;
			}
			public override UI.SetupWizard UserAccount(CommonSettings settings)
			{
				Add(new WinForms.UserAccountPage() { Settings = settings });
				return this;
			}
			public override UI.SetupWizard Web(CommonSettings settings)
			{
				Add(new WinForms.WebPage() { Settings = settings });
				return this;
			}
			public override bool Show()
			{
				Form.Wizard.LinkPages();
				Form.Wizard.SelectedPage = Form.Wizard.Controls.OfType<WinForms.WizardPageBase>()
					.FirstOrDefault();
				UI.EndWaitCursor();
				IWin32Window owner = UI.MainForm as IWin32Window;
				var res = Form.ShowModal(owner);
				var result = res == DialogResult.OK;
				if (res == DialogResult.Cancel) Installer.Cancel.Cancel();
				Installer.Current.UpdateSettings();
				Installer.Current.Cleanup();
				return result;
			}
		}

		bool isAvailable = false;
		public override bool IsAvailable
		{
			get
			{
				if (OSInfo.IsWindows || OSInfo.IsMono)
				{
					Init();
					return isAvailable;
				}
				else return false;
			}
		}
		public override UI.SetupWizard Wizard => new SetupWizard(this);
        
		public override void ShowError(Exception ex)
		{
			MessageBox.Show($"Error: {ex}", "SolidCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public override void PassArguments(Hashtable args)
		{
			base.PassArguments(args);
			args["ParentForm"] = MainForm as IWin32Window;
		}
		public override void ReadArguments(Hashtable args)
		{
			base.ReadArguments(args);
			MainForm = args["ParentForm"];
		}

		public override bool CheckForInstallerUpdate(bool appSetup = false)
		{
			ComponentUpdateInfo component;
			if (appSetup && Installer.Settings.Installer.CheckForUpdate &&
				!Environment.GetCommandLineArgs().Any(arg => arg.Equals("nockech", StringComparison.OrdinalIgnoreCase)) &&
				Installer.Current.CheckForInstallerUpdate(out component))
			{
				if (MessageBox.Show("There is an update for SolidCP Installer available. Do you want to install the update?",
					"Update for Installer available", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					if (Installer.Current.DownloadInstallerUpdate(component))
					{
						Installer.Current.Exit();
					}
				}
				return true;
			}
			return false;
		}
		
		public override void RunMainUI()
		{
			try
			{
				var mainForm = new ApplicationForm();
				MainForm = mainForm;
				mainForm.InitializeApplication();

				Application.Run(mainForm);
			}
			catch (Exception ex)
			{
				isAvailable = false;

				try
				{
					UI.Current = UI.AvaloniaUI;
					UI.Current.RunMainUI();
				}
				catch (Exception ex2)
				{
					UI.Current = UI.ConsoleUI;
					UI.Current.RunMainUI();
				}
			}
		}

		/// <summary>
		/// Application thread exception handler 
		/// </summary>
		static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Log.WriteError("Fatal error occured.", e.Exception);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.";
			MessageBox.Show(message, "SolidCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Application.Exit();
		}

		/// <summary>
		/// Application exception handler
		/// </summary>
		static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.WriteError("Fatal error occured.", (Exception)e.ExceptionObject);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.";
			MessageBox.Show(message, "SolidCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Process.GetCurrentProcess().Kill();
		}

		/// <summary>
		/// Writes to log on application exit
		/// </summary>
		private static void OnApplicationExit(object sender, EventArgs e)
		{
			Log.WriteApplicationEnd();
		}

		public override string GetRootPassword()
		{
			try
			{
				var form = new RootPasswordForm();
				MainForm = form;
				form.ShowDialog();
				return form.Password;
			} catch (Exception ex)
			{
				isAvailable = false;
				try
				{
					UI.Current = UI.AvaloniaUI;
					return UI.Current.GetRootPassword();
				} catch (Exception ex2)
				{
					UI.Current = UI.ConsoleUI;
					return UI.Current.GetRootPassword();
				}
			}
		}

		bool initCalled = false;
		public override void Init()
		{
			if (!initCalled)
			{
				initCalled = true;
				try
				{
					Application.ApplicationExit += new EventHandler(OnApplicationExit);
					Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					isAvailable = true;
				}
				catch
				{
					isAvailable = false;
				}
			}
		}

		bool exitCalled = false;
		public override void Exit()
		{
			if (!exitCalled)
			{
				if (File.Exists(WaitCursorIncrementFile)) File.Delete(WaitCursorIncrementFile);

				exitCalled = true;
				Application.Exit();
				Installer.Exit(0);
			}
		}

		public override void ShowLogFile()
		{
			throw new NotImplementedException();
		}

		public override void ShowWarning(string msg)
		{
			MessageBox.Show((Form)MainForm, msg, ((Form)MainForm).Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public override bool DownloadSetup(RemoteFile file, bool setupOnly = false)
		{
			Controls.Loader loader = new Controls.Loader(file, ShowError, setupOnly);
			DialogResult result = loader.ShowDialog();

			if (result == DialogResult.OK)
			{
				return true;
			}

			Installer.Cleanup();
			
			return false;
		}

		public override bool ExecuteSetup(string path, string installerType, string method, object[] args)
		{
			//run installer
			var res = (Result)Installer.Current.LoadContext.Execute(path, installerType, method, new object[] { args });
			Log.WriteInfo(string.Format("Installer returned {0}", res));
			Log.WriteEnd("Installer finished");
			(MainForm as Form)?.Update();
			((ApplicationForm)MainForm).ReloadApplication();
			
			EndWaitCursor();

			return res == Result.OK;
		}

		public override void ShowWaitCursor()
		{
			if (string.IsNullOrEmpty(Settings.Installer.TempPath)) Settings.Installer.TempPath = FileUtils.GetTempDirectory();

			if (!Directory.Exists(Settings.Installer.TempPath)) Directory.CreateDirectory(Settings.Installer.TempPath);

			Cursor.Current = Cursors.WaitCursor;

			var instances = File.Exists(WaitCursorIncrementFile) ? int.Parse(File.ReadAllText(WaitCursorIncrementFile)) : 0;
			File.WriteAllText(WaitCursorIncrementFile, (++instances).ToString());
		}
		public override void EndWaitCursor()
		{
			var instances = File.Exists(WaitCursorIncrementFile) ? int.Parse(File.ReadAllText(WaitCursorIncrementFile)) : 0;
			File.WriteAllText(WaitCursorIncrementFile, (--instances).ToString());

			if (instances <= 0) Cursor.Current = Cursors.Default;
		}

		public override void DownloadInstallerUpdate()
		{
			Application.Run(new UpdaterForm());
		}
	}
}

