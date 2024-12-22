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
			InstallServerWebsite();
			ConfigureServer();
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
		public virtual void ConfigureServer() { }
		public virtual void CopyServer(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, ServerFolder);
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}
	}
}