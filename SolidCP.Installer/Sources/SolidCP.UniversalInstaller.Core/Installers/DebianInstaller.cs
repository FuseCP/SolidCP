using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public class DebianInstaller : UnixInstaller
	{

		Providers.OS.Installer? apt = null;
		public Providers.OS.Installer Apt
		{
			get
			{
				if (apt == null)
				{
					apt = OSInfo.Unix.Apt;
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
				Apt.Install("aspnetcore-runtime-8.0 netcore-runtime-8.0");
			}
			else
			{
				Apt.Install("aspnetcore-runtime-8.0 netcore-runtime-8.0");
			}

			ResetHasDotnet();
		}

		public override void RemoveNet8NetRuntime()
		{
			Apt.Remove("netcore-runtime-8.0");
			ResetHasDotnet();
		}
		public override void RemoveNet8AspRuntime()
		{
			Apt.Remove("aspnetcore-runtime-8.0");
			ResetHasDotnet();
		}
		public override void InstallServerPrerequisites()
		{
			InstallNet8Runtime();
		}
	}
}
