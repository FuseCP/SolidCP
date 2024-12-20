#if NETFRAMEWORK

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller {

    public class WinFormsUI: UI {

		public class SetupWizard : UI.SetupWizard
		{
			public SetupWizard(UI ui): base(ui) { }
			public override void Show()
			{
				throw new NotImplementedException();
			}
		}

		public override bool IsAvailable => true;
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
			UI.Current = new ConsoleUI();
			return UI.Current.GetPackagesToInstall();
        }
        
        public override void ShowInstallationProgress() {
            throw new NotImplementedException();
        }

		public override void CloseInstallationProgress()
		{
			throw new NotImplementedException();
		}

		public override void ShowError(Exception ex)
		{
			throw new NotImplementedException();
		}

		public override void RunMainUI()
		{
			try
			{
				Application.ApplicationExit += new EventHandler(OnApplicationExit);
				Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				var mainForm = new ApplicationForm();
				MainForm = MainForm;
				mainForm.InitializeApplication();
				Application.Run(mainForm);
			}
			catch (Exception ex)
			{
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
				form.ShowDialog();
				return form.Password;
			} catch (Exception ex)
			{
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

		public override void Init()
		{
		}
		public override void Exit()
		{
		}

		public override void CheckPrerequisites()
		{
			UI.Current = new ConsoleUI();
			UI.Current.CheckPrerequisites();			
		}

		public override void ShowLogFile()
		{
			throw new NotImplementedException();
		}

		public object MainForm { get; set; }
		public override void ShowWarning(string msg)
		{
			MessageBox.Show((Form)MainForm, msg, ((Form)MainForm).Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public override bool DownloadSetup(string fileName)
		{
			Controls.Loader form = new Controls.Loader(fileName, (e) => ShowError(e));
			DialogResult result = form.ShowDialog((Form)MainForm);

			if (result == DialogResult.OK)
			{
				((Form)MainForm).Update();
				return true;
			}
			return false;
		}

		public override bool ExecuteSetup(string path, string installerType, string method, object[] args)
		{
			//run installer
			DialogResult? res = AssemblyLoader.Execute(path, installerType, method, new object[] { args }) as DialogResult?;
			Log.WriteInfo(string.Format("Installer returned {0}", res));
			Log.WriteEnd("Installer finished");
			((Form)MainForm).Update();
			if (res != null && res.Value == DialogResult.OK)
			{
				((ApplicationForm)MainForm).ReloadApplication();
				return true;
			}
			return false;
		}
	}
}

#endif