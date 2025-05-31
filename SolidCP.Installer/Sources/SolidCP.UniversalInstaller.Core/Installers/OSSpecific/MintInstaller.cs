using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class MintInstaller : UbuntuInstaller
	{

		public override void InstallNet8Runtime()
		{
			if (CheckNet8RuntimeInstalled()) return;

			if (OSInfo.OSVersion.Major < 20) throw new PlatformNotSupportedException("Cannot install NET 8 on MINT below version 20. Please install NET 8 runtime manually.");

			bool installFromMicrosoftFeed = false;

			if (!HasDotnet)
			{
				if (OSInfo.Architecture == Architecture.Arm64)
				{
					if (OSInfo.OSVersion.Major < 22) throw new PlatformNotSupportedException("NET 8 not supported on this platform. Arm64 is only supported on Ubuntu 23 and above. Please install NET 8 runtime manually.");
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

			Info("Installing .NET 8 Runtime...");

			if (installFromMicrosoftFeed)
			{
				// install dotnet from microsoft
				var version = OSInfo.OSVersion;
				var ubuntuVersion = version;
				switch (version.Major)
				{
					case 21: ubuntuVersion = new Version("22.04"); break;
					case 20: ubuntuVersion = new Version("20.04"); break;
					default: throw new PlatformNotSupportedException("Cannot install dotnet on this OS.");
				}
				Apt.Install("wget");
				Shell.ExecScript(@$"
# Download Microsoft signing key and repository
wget https://packages.microsoft.com/config/ubuntu/{ubuntuVersion}/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install Microsoft signing key and repository
dpkg -i packages-microsoft-prod.deb

# Clean up
rm packages-microsoft-prod.deb
");
				Apt.Update();
			}

			Apt.Install("aspnetcore-runtime-8.0 netcore-runtime-8.0");

			Net8RuntimeInstalled = true;

			InstallLog("Installed .NET 8 Runtime.");

			ResetHasDotnet();
		}

	}
}

