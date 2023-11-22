using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class WinGet : Installer
	{
		public override bool IsInstalled => Shell.Find("winget") != null;

		public override void AddSources(string sources)
		{
			throw new NotImplementedException();
		}

		public override Shell InstallAsync(string apps)
		{
			return Shell.Exec($"winget install {apps}");
		}
	}
}
