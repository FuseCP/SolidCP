using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class UbuntuInstaller : DebianInstaller
	{

		public override void InstallNet8Runtime()
		{
			if (CheckNet8RuntimeInstalled()) return;

			if (OSInfo.OSVersion.Major < 20) throw new PlatformNotSupportedException("Cannot install NET 8 on Ubuntu below version 20.4. Please install NET 8 runtime manually.");

			bool installFromMicrosoftFeed = false;

			if (!HasDotnet)
			{
				if (OSInfo.Architecture == Architecture.Arm64)
				{
					if (OSInfo.OSVersion.Major < 23) throw new PlatformNotSupportedException("NET 8 not supported on this platform. Arm64 is only supported on Ubuntu 23 and above. Please install NET 8 runtime manually.");
					// install from ubuntu
					installFromMicrosoftFeed = false;
				}
				else if (OSInfo.Architecture == Architecture.Arm) throw new PlatformNotSupportedException("NET 8 not supported on Arm platform. Please install NET 8 runtime manually.");
				else if (OSInfo.Architecture == Architecture.X86) throw new PlatformNotSupportedException("NET 8 not supported on this platform. Please install NET 8 runtime manually.");

				else
				{
					installFromMicrosoftFeed = true;

				}
			}
			else installFromMicrosoftFeed = false;

			if (installFromMicrosoftFeed)
			{
				// install dotnet from microsoft
				Apt.Install("wget");
				Shell.ExecScript(@"
# Get Ubuntu version
declare repo_version=$(if command -v lsb_release &> /dev/null; then lsb_release -r -s; else grep -oP '(?<=^VERSION_ID=).+' /etc/os-release | tr -d '""'; fi)

# Download Microsoft signing key and repository
wget https://packages.microsoft.com/config/ubuntu/$repo_version/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install Microsoft signing key and repository
dpkg -i packages-microsoft-prod.deb

# Clean up
rm packages-microsoft-prod.deb
");
				Apt.Update();
			}

			Apt.Install("aspnetcore-runtime-8.0 netcore-runtime-8.0");

			ResetHasDotnet();
		}
	}
}
