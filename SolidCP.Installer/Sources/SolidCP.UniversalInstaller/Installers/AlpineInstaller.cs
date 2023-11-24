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

            throw new NotSupportedException();
        }

        public override void RemoveNet8Runtime()
        {
            if (!Net8RuntimeAllreadyInstalled)
            {

            }
            if (!Net8AspRuntimeAllreadyInstalled)
            {

            }
        }
    }
}
