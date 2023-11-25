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

        public override Packages GetPackagesToInstall() {
            throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

	}
}