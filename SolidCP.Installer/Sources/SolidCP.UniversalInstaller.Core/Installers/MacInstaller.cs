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

			string? tmp = null;

			if (OSInfo.Architecture == Architecture.X64) tmp = DownloadFile("https://download.visualstudio.microsoft.com/download/pr/27a7ece8-f6cd-4cab-89cf-987e85ae6805/2c9ab2cb294143b0533f005640c393da/dotnet-sdk-8.0.100-osx-x64.pkg");
			else if (OSInfo.Architecture == Architecture.Arm64) tmp = DownloadFile("https://download.visualstudio.microsoft.com/download/pr/cf196f2f-f1e2-4f9a-a7ac-546242c431e2/8c386932f4a2f96c3e95c433e4899ec2/dotnet-sdk-8.0.100-osx-arm64.pkg");
			else throw new PlatformNotSupportedException("Only x64 and Arm64 architectures allowed.");

			Shell.Exec($"installer -pkg \"{tmp}\" -target /");
			
			ResetHasDotnet();
		}

		public override void RemoveNet8AspRuntime()
		{
			throw new NotImplementedException();
		}
		public override void RemoveNet8NetRuntime()
		{
			throw new NotImplementedException();
		}

		public override void InstallServerPrerequisites()
		{
			InstallNet8Runtime();
		}

	}
}
