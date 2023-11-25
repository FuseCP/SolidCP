using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class CentOSInstaller : FedoraInstaller
	{

		public override void InstallNet8Runtime()
		{
			if (CheckNet8RuntimeInstalled()) return;

			throw new NotSupportedException();
		}

	}
}

