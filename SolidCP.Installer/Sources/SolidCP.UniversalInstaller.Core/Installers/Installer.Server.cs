using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;

namespace SolidCP.UniversalInstaller
{


	public abstract partial class Installer
	{
		public virtual void InstallServerPrerequisites() { }
		public virtual void RemoveServerPrerequisites() { }

		public virtual void SetServerFilePermissions() => SetFilePermissions(ServerFolder);
		public virtual void SetServerFileOwner() => SetFileOwner(ServerFolder, Settings.Server.Username, SolidCP.ToLower());
		public virtual void InstallServer()
		{
			InstallServerPrerequisites();
			CopyServer();
			SetServerFilePermissions();
			SetServerFileOwner();
			ConfigureServer();
			InstallServerWebsite();
		}
		public virtual void UpdateServer()
		{
			InstallServerPrerequisites();
			CopyServer(ConfigAndSetupFilter);
			SetServerFilePermissions();
			SetServerFileOwner();
			UpdateServerConfig();
			ConfigureServer();
			InstallServerWebsite();
		}

		public virtual void RemoveServer()
		{
			//RemoveServerPrerequisites();
			RemoveServerWebsite();
			RemoveServerFolder();
		}

		public virtual void InstallServerUser() { }
		public virtual void InstallServerApplicationPool() { }
		public virtual void InstallServerWebsite() { }
		public virtual void RemoveServerWebsite() { }
		public virtual void RemoveServerFolder() {
			Directory.Delete(Path.Combine(InstallWebRootPath, ServerFolder), true);
		}
		public virtual void RemoveServerUser() { }
		public virtual void RemoveServerApplicationPool() { }
		public virtual void ReadServerConfiguration() { }
		public virtual void UpdateServerConfig() { }
		public virtual void ConfigureServer() { }
		public virtual void CopyServer(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}
		public virtual int InstallServerMaxProgress => 100;
		public virtual int UninstallServerMaxProgress => 100;
		public virtual int SetupServerMaxProgress => 100;
		public virtual int UpdateServerMaxProgress => 100;
	}
}