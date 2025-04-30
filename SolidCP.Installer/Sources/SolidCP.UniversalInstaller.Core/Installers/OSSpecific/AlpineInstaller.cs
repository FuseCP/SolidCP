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

            throw new NotSupportedException("NET 8 Runtime must be installed.");
        }

		public override void RemoveNet8AspRuntime()
		{
			throw new NotImplementedException();
		}
		public override void RemoveNet8NetRuntime()
		{
			throw new NotImplementedException();
		}
	}
}
