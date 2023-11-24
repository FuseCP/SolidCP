using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class Yum : Installer
    {
        public override bool IsInstallerInstalled => Shell.Find("yum") != null;

        public override void AddSources(string sources)
        {
            throw new NotImplementedException();
        }

        public override Shell Install(string apps)
        {
            throw new NotImplementedException();
        }

		public override bool IsInstalled(string apps)
		{
			throw new NotImplementedException();
		}
	}
}
