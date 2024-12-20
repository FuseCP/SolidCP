using System;

namespace SolidCP.UniversalInstaller {

	public class NotAvailableUI : UI {

		public override bool IsAvailable => false;

		public new class SetupWizard : UI.SetupWizard
		{
			public SetupWizard(UI ui) : base(ui) { }
			public override void Show() => throw new NotImplementedException();
		}

		public override UI.SetupWizard Wizard => new SetupWizard(this);

		public override void RunMainUI() => throw new NotImplementedException();
		public override ServerSettings GetServerSettings() => throw new NotImplementedException();
		public override EnterpriseServerSettings GetEnterpriseServerSettings() => throw new NotImplementedException();
		public override WebPortalSettings GetWebPortalSettings() => throw new NotImplementedException();
		public override void GetCommonSettings(CommonSettings settings) => throw new NotImplementedException();
		public override Packages GetPackagesToInstall() => throw new NotImplementedException();
		public override void ShowInstallationProgress() => throw new NotImplementedException();
		public override void CloseInstallationProgress() => throw new NotImplementedException();
		public override void ShowLogFile() => throw new NotImplementedException();
		public override string GetRootPassword() => throw new NotImplementedException();
		public override void ShowInstallationSuccess(Packages packages) => throw new NotImplementedException();
		public override void Init() => throw new NotImplementedException();
		public override void Exit() => throw new NotImplementedException();
		public override void CheckPrerequisites() => throw new NotImplementedException();
		public override void ShowWarning(string msg) => throw new NotImplementedException();
		public override void ShowError(Exception ex) => throw new NotImplementedException();
		public override bool DownloadSetup(string fileName) => throw new NotImplementedException();
		public override bool ExecuteSetup(string path, string installerType, string method, object[] args)
			=> throw new NotSupportedException();

	}
}
