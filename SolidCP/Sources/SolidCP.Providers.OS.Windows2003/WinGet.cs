using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class WinGet : Installer
	{
		public override bool IsInstallerInstalled => Shell.Find("winget") != null;

		public override Shell AddSources(string sources)
		{
			throw new NotImplementedException();
		}

		public override Shell InstallAsync(string apps)
		{
			return Shell.ExecAsync($"winget install {apps.Replace(',', ' ').Replace(';', ' ')} --accept-source-agreements --accept-package-agreements");
		}

		public override bool IsInstalled(string apps)
		{
			throw new NotImplementedException();
		}

		public override Shell AddSourcesAsync(string sources)
		{
			throw new NotImplementedException();
		}
		public override Shell RemoveAsync(string apps)
		{
			throw new NotImplementedException();
		}
		public override Shell UpdateAsync()
		{
			throw new NotImplementedException();
		}
	}
}
