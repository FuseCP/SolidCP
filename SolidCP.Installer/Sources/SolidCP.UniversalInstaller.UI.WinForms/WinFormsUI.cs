#if NETFRAMEWORK

using System;
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
				var form = new ApplicationForm();
				form.ShowDialog();
				return form.Password;
			}
			catch (Exception ex)
			{
				try
				{
					UI.Current = UI.AvaloniaUI;
					return UI.Current.GetRootPassword();
				}
				catch (Exception ex2)
				{
					UI.Current = UI.ConsoleUI;
					return UI.Current.GetRootPassword();
				}
			}
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
	}
}

#endif