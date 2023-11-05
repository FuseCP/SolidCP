using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
	public class Yum : Installer
	{
		public override bool IsInstalled => Shell.Find("yum") != null;

		public override void AddSources(string sources)
		{
			CheckInstalled();
			throw new NotImplementedException();
		}

		public override Shell InstallAsync(string apps)
		{
			CheckInstalled();
			throw new NotImplementedException();
		}
	}
}
