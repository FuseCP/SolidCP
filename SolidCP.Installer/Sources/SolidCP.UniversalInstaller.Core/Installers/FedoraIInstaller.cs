using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class FedoraInstaller : UnixInstaller
	{

		public override void InstallNet8Runtime()
		{
			if (CheckNet8RuntimeInstalled()) return;

			OSInstaller.Install("aspnetcore-runtime-8.0;dotnet-runtime-8.0");
			HasDotnet = Shell.Find("dotnet") != null;
		}

		public override void RemoveNet8NetRuntime()
		{
			OSInstaller.Remove("dotnet-runtime-8.0");
			HasDotnet = Shell.Find("dotnet") != null;
		}
		public override void RemoveNet8AspRuntime()
		{
			OSInstaller.Remove("aspnetcore-runtime-8.0");
			HasDotnet = Shell.Find("dotnet") != null;
		}
	}
}

