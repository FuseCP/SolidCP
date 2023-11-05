using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
	public class Brew : Installer
	{
		public override bool IsInstalled => Shell.Find("brew") != null;

		public override void AddSources(string sources)
		{
			throw new NotImplementedException();
		}

		public override Shell InstallAsync(string apps)
		{
			throw new NotImplementedException();
		}
	}
}
