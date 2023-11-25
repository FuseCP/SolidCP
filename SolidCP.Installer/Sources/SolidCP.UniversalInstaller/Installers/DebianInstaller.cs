using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
    public class DebianInstaller : UnixInstaller
    {

        Providers.OS.Installer apt;
        public Providers.OS.Installer Apt
        {
            get
            {
                if (apt == null)
                {
                    apt = ((IUnixOperatingSystem)OSInfo.Current).Apt;
                    apt.CheckInstallerInstalled();
                }
                return apt;
            }
        }

        public override void InstallNet8Runtime()
        {
            if (OSInfo.OSVersion.Major < 11) throw new PlatformNotSupportedException("Cannot install NET 8 on Debian below version 11.");

            CheckNet8RuntimeInstalled();

			if (!HasDotnet)
            {
                // install dotnet from microsoft
                var tmp = DownloadFile($"https://packages.microsoft.com/config/debian/{OSInfo.OSVersion.Major}/packages-microsoft-prod.deb");
                Shell.Exec($"dpkg -i \"{tmp}\"");
                File.Delete(tmp);
                Apt.Update();
                Apt.Install("aspnetcore-runtime-8.0 ");
            }
            else
            {
                Apt.Install("aspnetcore-runtime-8.0");
            }
        }

		public override void RemoveNet8Runtime()
		{
            if (!Net8RuntimeAllreadyInstalled) Apt.Remove("aspnetcore-runtime-8.0");
		}
		public override void InstallServerPrerequisites()
		{
            InstallNet8Runtime();
		}
	}
}
