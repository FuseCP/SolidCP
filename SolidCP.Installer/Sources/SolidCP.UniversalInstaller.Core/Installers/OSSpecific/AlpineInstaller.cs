using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
    public class AlpineInstaller : UnixInstaller
    {
        public override void InstallNet8Runtime()
        {
            if (CheckNet8RuntimeInstalled()) return;

			OSInstaller.Install("dotnet8-runtime, aspnetcore8-runtime");
		}

		public override void RemoveNet8AspRuntime()
		{
			OSInstaller.Remove("aspnetcore8-runtime");
		}
		public override void RemoveNet8NetRuntime()
		{
			OSInstaller.Remove("dotnet8-runtime");
		}
	}
}
