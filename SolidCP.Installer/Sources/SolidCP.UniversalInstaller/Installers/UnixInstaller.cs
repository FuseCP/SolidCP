using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolidCP.UniversalInstaller
{
    public abstract class UnixInstaller : Installer
    {
		public override string InstallExeRootPath { get => base.InstallExeRootPath ?? "/user/local/SolidCP/bin"; set => base.InstallExeRootPath = value; }
		public override string InstallWebRootPath { get => base.InstallWebRootPath ?? "/var/www/SolidCP"; set => base.InstallWebRootPath = value; }
		public override string WebsiteLogsPath => "/var/log/SolidCP";
		public UnixInstaller(): base() { }

		public override void InstallServerWebsite()
		{
			// Run SolidCP.Server as a service on Unix

			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);

			var service = new ServiceDescription()
			{
				ServiceId = "SolidCPServer",
				Directory = websitePath,
				Description = "SolidCP.Server service, the server management service for the SolidCP control panel.",
				Executable = $"dotnet {websitePath}/SolidCP.Server.dll"
			};
			service.EnvironmentVariables.Add("ASPNETCORE_ENVIRONMENT", "Production");

			ServiceController.Install(service);
			ServiceController.Enable(service.ServiceId);
			ServiceController.Start(service.ServiceId);
		}

		public override void InstallServerPrerequisites()
		{
			InstallNet8Runtime();
		}
	}
}
