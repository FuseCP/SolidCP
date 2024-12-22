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
		public virtual void InstallPortalPrerequisites() { }
		public virtual void RemovePortalPrerequisites() { }
		public virtual void SetPortalFilePermissions() => SetFilePermissions(PortalFolder);
		public virtual void SetPortalServerFileOwner() => SetFileOwner(PortalFolder, Settings.WebPortal.Username, SolidCP.ToLower());
		public virtual void InstallPortalWebsite()
		{
			InstallWebsite($"{SolidCP}WebPortal", Path.Combine(InstallWebRootPath, PortalFolder), Settings.WebPortal.Urls ?? "", "", "");
		}
		public virtual void InstallWebPortal()
		{
			InstallPortalPrerequisites();
			ReadWebPortalConfiguration();
			CopyPortal();
			SetPortalFilePermissions();
			SetPortalServerFileOwner();
			InstallPortalWebsite();
		}
		public virtual void RemovePortalWebsite() { }
		public virtual void ReadWebPortalConfiguration()
		{
			Settings.WebPortal = new WebPortalSettings();
		}
		public virtual void ConfigureWebPortal(WebPortalSettings settings)
		{

		}
		public virtual void CopyPortal(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, PortalFolder);
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}

	}
}