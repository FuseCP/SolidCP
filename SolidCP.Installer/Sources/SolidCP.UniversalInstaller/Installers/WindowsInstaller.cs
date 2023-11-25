using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Http;
using Newtonsoft.Json.Bson;
using SolidCP.Providers.Web;
using SolidCP.Providers;
using System.Security.Policy;

namespace SolidCP.UniversalInstaller
{
	public class WindowsInstaller : Installer
	{
		const bool Net8RuntimeNeeded = false;

		public override string InstallExeRootPath
		{
			get => base.InstallExeRootPath ??
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), SolidCP);
            set => base.InstallExeRootPath = value;
		}
		public override string InstallWebRootPath { get => base.InstallWebRootPath ?? InstallExeRootPath; set => base.InstallWebRootPath = value; }
		public override string WebsiteLogsPath => InstallExeRootPath;

		WinGet WinGet => (WinGet)((IWindowsOperatingSystem)OSInfo.Current).WinGet;
		public override void InstallNet8Runtime()
		{
			if (Net8RuntimeNeeded)
			{
				if (Shell.Find("dotnet") != null)
				{
					var output = Shell.Exec("dotnet --info").Output().Result;
					if (output.Contains("Microsoft.AspNetCore.App 8.") &&
						output.Contains("Microsoft.NETCore.App 8."))
						// NET 8 runtime allready installed
						Net8RuntimeAllreadyInstalled = true;
					return;
				}

				var ver = OSInfo.WindowsVersion;
				if (!(OSInfo.IsWindowsServer && ver >= WindowsVersion.WindowsServer2012 ||
					!OSInfo.IsWindowsServer && ver >= WindowsVersion.Windows10))
					throw new PlatformNotSupportedException("NET 8 is not supported on this OS.");

				WinGet.Install("Microsoft.DotNet.AspNetCore.8;Microsoft.DotNet.Runtime.8");
			}
		}

		public override void RemoveNet8Runtime()
		{
			if (!Net8RuntimeAllreadyInstalled)
			{
				WinGet.Remove("Microsoft.DotNet.AspNetCore.8;Microsoft.DotNet.Runtime.8");
			}
		}

		public void InstallNet48()
		{
			if (!OSInfo.IsNet48) {
				var file = DownloadFile("https://download.visualstudio.microsoft.com/ndp48-web.exe");
				if (file != null)
				{
					Shell.Exec($"\"{file}\" /q");
				}
			}
		}

		public override void InstallServerPrerequisites()
		{
			// NET 8 not needed, as server still runs on NET Framework on Windows.
			// InstallNet8Runtime(); 

			InstallNet48();
		}

		public override void InstallServerWebsite()
		{
			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
			InstallWebsite("SolidCP.Server", websitePath, ServerSettings.Urls);
		}
	}
}