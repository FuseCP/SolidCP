using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using Ionic.Zip;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data;

namespace SolidCP.UniversalInstaller
{


	public abstract partial class Installer
	{
		public ServerSettings ServerSettings { get; set; } = new ServerSettings();

		public virtual void InstallServerPrerequisites() { }
		public virtual void RemoveServerPrerequisites() { }

		public virtual void SetServerFilePermissions() => SetFilePermissions(ServerFolder);

		public virtual void ConfigureServer()
		{
		}

		public virtual void InstallServer()
		{
			InstallServerPrerequisites();
			UnzipServer();
			InstallServerWebsite();
			SetServerFilePermissions();
			ConfigureServer();
		}

		public virtual void RemoveServer()
		{
			RemoveServerPrerequisites();
			RemoveServerFolder();
			RemoveServerWebsite();
		}

		public virtual void InstallServerUser() { }
		public virtual void InstallServerApplicationPool() { }
		public virtual void InstallServerWebsite() { }
		public virtual void RemoveServerWebsite() { }
		public virtual void RemoveServerFolder() { }
		public virtual void RemoveServerUser() { }
		public virtual void RemoveServerApplicationPool() { }
		public virtual void ReadServerConfiguration()
		{
			ServerSettings = new ServerSettings();
		}
		public void ConfigureServer(ServerSettings settings)
		{
		}
		public virtual void UnzipServer()
		{
			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
			UnzipFromResource("SolidCP-Server.zip", websitePath, UnzipFilter);
		}
	}
}