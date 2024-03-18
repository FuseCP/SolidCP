using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class Brew : Installer
	{
		public override bool IsInstallerInstalled => Shell.Find("brew") != null;

		public override Shell AddSourcesAsync(string sources)
		{
			throw new NotImplementedException();
		}

		public override Shell InstallAsync(string apps)
		{
			throw new NotImplementedException();
		}

		public override bool IsInstalled(string apps)
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
