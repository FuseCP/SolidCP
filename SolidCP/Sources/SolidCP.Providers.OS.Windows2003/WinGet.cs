using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class WinGet : Installer
	{
		public override bool IsInstallerInstalled => Shell.Find("winget") != null;

		public override void AddSources(string sources)
		{
			throw new NotImplementedException();
		}

		public override Shell Install(string apps)
		{
			Shell.Exec($"winget install {apps}").Task().Wait();
			return Shell;
		}

		public override bool IsInstalled(string apps)
		{
			throw new NotImplementedException();
		}
	}
}
