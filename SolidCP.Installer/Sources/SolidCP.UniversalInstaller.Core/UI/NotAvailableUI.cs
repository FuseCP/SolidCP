using System;

namespace SolidCP.UniversalInstaller {

	public class NotAvailableUI : UI {

		public override bool IsAvailable => false;

		public new class SetupWizard : UI.SetupWizard
		{
			public SetupWizard(UI ui) : base(ui) { }
			public override bool Show() => throw new NotImplementedException();
		}

		public override UI.SetupWizard Wizard => new SetupWizard(this);

		public override void RunMainUI() => throw new NotImplementedException();
		public override void ShowLogFile() => throw new NotImplementedException();
		public override string GetRootPassword() => throw new NotImplementedException();
		public override void Init() { }
		public override void Exit() => throw new NotImplementedException();
		public override void ShowWarning(string msg) => throw new NotImplementedException();
		public override void ShowError(Exception ex) => throw new NotImplementedException();
		public override void Update() => throw new NotImplementedException();
		public override bool DownloadSetup(RemoteFile file, bool setupOnly = false) => throw new NotImplementedException();
	}
}
