using System;

namespace SolidCP.UniversalInstaller
{
	public partial class AvaloniaUI : UI
	{
		public new class SetupWizard: UI.SetupWizard
		{
			public SetupWizard(UI ui): base(ui) { }

			public override UI.SetupWizard Introduction(CommonSettings settings) => this;
			public override UI.SetupWizard Certificate(CommonSettings settings) => this;
			public override UI.SetupWizard CheckPrerequisites() => this;
			public override UI.SetupWizard ConfirmUninstall(ComponentSettings settings) => this;
			public override UI.SetupWizard Database() => this;
			public override UI.SetupWizard EmbedEnterpriseServer() => this;
			public override UI.SetupWizard Progress() => this;
			public override UI.SetupWizard Download() => this;
			public override UI.SetupWizard Finish() => this;
			public override UI.SetupWizard InsecureHttpWarning(CommonSettings settings) => this;
			public override UI.SetupWizard InstallFolder(ComponentSettings settings) => this;
			public override UI.SetupWizard LicenseAgreement() => this;
			public override UI.SetupWizard ServerAdminPassword() => this;
			public override UI.SetupWizard ServerPassword() => this;
	
			public override bool Show()
			{
				return true;
			}
		}

		bool isAvailable = false;
		public override bool IsAvailable => isAvailable;

		public override UI.SetupWizard Wizard => new SetupWizard(this);

		public override void CheckPrerequisites()
		{
			throw new NotImplementedException();
		}

		public override void CloseInstallationProgress()
		{
			throw new NotImplementedException();
		}

		public override void Exit()
		{
		}

		public override string GetRootPassword()
		{
			throw new NotImplementedException();
		}

		public override void Init()
		{
			try
			{
			} catch
			{

			}
		}

		public override void RunMainUI()
		{
			throw new NotImplementedException();
		}

		public override void ShowError(Exception ex)
		{
			throw new NotImplementedException();
		}

		public override void ShowInstallationProgress(string title = null, int maxProgress = 100)
		{
			throw new NotImplementedException();
		}

		public override void ShowInstallationSuccess(Packages packages)
		{
			throw new NotImplementedException();
		}

		public override void ShowLogFile()
		{
			throw new NotImplementedException();
		}
		public override void ShowWarning(string msg) => throw new NotImplementedException();

		public override bool DownloadSetup(RemoteFile file)
		{
			throw new NotImplementedException();
		}
	}
}
