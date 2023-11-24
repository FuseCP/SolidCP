using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
    public class DebianInstaller : Installer.Installer
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

        public override async void InstallNet8Runtime()
        {
            if (OSInfo.OSVersion.Major < 11) throw PlatformNotSupportedException("Cannot install NET 8 on Debian below version 11.");

            if (Shell.Find("dotnet") == null)
            {
                // install dotnet from microsoft
                await Shell.Exec("apt update");
                Apt.Install("wget");
                await Shell.Exec($"wget https://packages.microsoft.com/config/debian/{OSInfo.OSVersion.Major}/packages-microsoft-prod.deb -O packages-microsoft-prod.deb");
                await Shell.Exec("dpkg -i packages-microsoft-prod.deb");
                await Shell.Exec("rm packages-microsoft-prod.deb");
                await Shell.Exec("apt update");
                Apt.Install("aspnetcore-runtime-8.0 ");
            }
            else
            {
                Apt.Install("aspnetcore-runtime-8.0");
            }
        }

        public override void InstallGlobalPrerequisites()
        {
            throw new NotImplementedException();
        }
    }
}
