using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Renci.SshNet.Messages;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller {

    public class WinFormsUI: UI {

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
			public override UI.SetupWizard Introduction(CommonSettings settings)
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
			public override UI.SetupWizard RunWithProgress(string title, Action action, ComponentSettings settings, int maxProgress)
			{
				Add(new WinForms.ProgressPage() { Maximum = maxProgress, Settings = settings, Action = action });
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
				var result = Form.ShowModal(owner) == DialogResult.OK;
				if (result) Installer.Current.UpdateSettings();
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

		public override ServerSettings GetServerSettings()
        {
            throw new NotImplementedException();
        }
        
        public override EnterpriseServerSettings GetEnterpriseServerSettings()
        {
            throw new NotImplementedException();
        }
        
        public override WebPortalSettings GetWebPortalSettings() {
            throw new NotImplementedException();
        }

		public override void GetCommonSettings(CommonSettings settings)
		{
			throw new NotImplementedException();
		}

		public override Packages GetPackagesToInstall() {
			throw new NotImplementedException();
        }
        
        public override void ShowInstallationProgress(string title = null, int maxProgress = 100) {
            throw new NotImplementedException();
        }

		public override void CloseInstallationProgress()
		{
			throw new NotImplementedException();
		}

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

		public override void ShowInstallationSuccess(Packages packages)
		{
			throw new NotImplementedException();
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
				exitCalled = true;
				Application.Exit();
				Installer.Exit(0);
			}
		}

		public override void CheckPrerequisites()
		{
			throw new NotSupportedException();
		}

		public override void ShowLogFile()
		{
			throw new NotImplementedException();
		}

		public override void ShowWarning(string msg)
		{
			MessageBox.Show((Form)MainForm, msg, ((Form)MainForm).Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public override bool DownloadSetup(RemoteFile file)
		{
			Controls.Loader loader = new Controls.Loader(file, (e) => ShowError(e));
			DialogResult result = loader.ShowDialog();

			if (result == DialogResult.OK)
			{
				return true;
			}
			return false;
		}

		public override bool ExecuteSetup(string path, string installerType, string method, object[] args)
		{
			//run installer
			var res = (Result)Installer.Current.LoadContext.Execute(path, installerType, method, new object[] { args });
			Log.WriteInfo(string.Format("Installer returned {0}", res));
			Log.WriteEnd("Installer finished");
			(MainForm as Form)?.Update();
			if (res == Result.OK)
			{
				((ApplicationForm)MainForm).ReloadApplication();
				return true;
			}
			return false;
		}

		public override void ShowWaitCursor() => Cursor.Current = Cursors.WaitCursor;
		public override void EndWaitCursor() => Cursor.Current = Cursors.Default;
	}
}

