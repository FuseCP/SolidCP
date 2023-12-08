using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class FedoraInstaller : UnixInstaller
	{

		public override void InstallPKExec()
		{
			OSInstaller.Install("polkit");
		}

		public override void InstallNet8Runtime()
		{
			if (CheckNet8RuntimeInstalled()) return;

			throw new NotSupportedException();
		}

		public override void RemoveNet8NetRuntime()
		{
			throw new NotSupportedException();
		}
		public override void RemoveNet8AspRuntime()
		{
			throw new NotImplementedException();
		}
	}
}

