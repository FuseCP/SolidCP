#if NETFRAMEWORK

namespace SolidCP.UniversalInstaller {

    public class WinFormsUI: UI {

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

		public override string GetRootPassword()
		{
			try
			{
				var form = new RootPasswordForm();
				form.ShowDialog();
				return form.Password;
			} catch (Exception ex)
			{
				UI.Current = new ConsoleUI();
				return UI.Current.GetRootPassword();
			}
		}

		public override void ShowInstallationSuccess(Packages packages)
		{
			throw new NotImplementedException();
		}
	}
}

#endif