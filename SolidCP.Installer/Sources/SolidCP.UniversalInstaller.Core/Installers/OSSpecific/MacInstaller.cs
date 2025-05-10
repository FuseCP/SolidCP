using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
	public class MacInstaller : UnixInstaller
	{
		Brew Brew => (Brew)OSInfo.Unix.Brew;

		public override void InstallNet8Runtime()
		{
			if (CheckNet8RuntimeInstalled()) return;

			string tmp = null;

			if (OSInfo.Architecture == Architecture.X64) tmp = DownloadFile("https://builds.dotnet.microsoft.com/dotnet/Sdk/8.0.408/dotnet-sdk-8.0.408-osx-x64.pkg");
			else if (OSInfo.Architecture == Architecture.Arm64) tmp = DownloadFile("https://builds.dotnet.microsoft.com/dotnet/Sdk/8.0.408/dotnet-sdk-8.0.408-osx-arm64.pkg");
			else throw new PlatformNotSupportedException("Only x64 and Arm64 architectures supported.");

			Shell.Exec($"installer -pkg \"{tmp}\" -target /");
			Shell.Exec("brew update");
			Shell.Exec("brew install mono-libgdiplus");

			Net8RuntimeInstalled = true;

			ResetHasDotnet();
		}

		public override void RemoveNet8AspRuntime()
		{
			//throw new NotImplementedException();
		}
		public override void RemoveNet8NetRuntime()
		{
			//throw new NotImplementedException();
		}
	}
}
