using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;


namespace SolidCP.Providers.OS
{
    public class Apk: Installer
	{

		public override bool IsInstallerInstalled => Shell.Find("apk") != null;
		public override Shell InstallAsync(string apps)
		{
			throw new NotSupportedException();
		}

		public override Shell AddSourcesAsync(string sources)
		{
			throw new NotSupportedException();
		}
		public override bool IsInstalled(string apps)
		{
			throw new NotSupportedException();
		}

		public override Shell RemoveAsync(string apps)
		{
			throw new NotSupportedException();
		}

		public override Shell UpdateAsync()
		{
			throw new NotSupportedException();
		}
	}
}
