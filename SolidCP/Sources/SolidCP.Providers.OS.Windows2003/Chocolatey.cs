using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class Chocolatey: Installer
	{
		public override bool IsInstalled => Shell.Find("choco") != null;

		public override void AddSources(string sources)
		{
			throw new NotImplementedException();
		}

		public override Shell InstallAsync(string apps)
		{
			return Shell.ExecAsync($"choco install {apps}");
		}
	}
}
