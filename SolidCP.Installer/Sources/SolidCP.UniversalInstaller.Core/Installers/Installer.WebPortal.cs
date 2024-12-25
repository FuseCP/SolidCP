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
using System.Data;

namespace SolidCP.UniversalInstaller
{
	public abstract partial class Installer
	{
		public virtual void InstallWebPortalPrerequisites() { }
		public virtual void RemoveWebPortalPrerequisites() { }
		public virtual void SetWebPortalFilePermissions() => SetFilePermissions(PortalFolder);
		public virtual void SetWebPortalFileOwner() => SetFileOwner(PortalFolder, Settings.WebPortal.Username, SolidCP.ToLower());
		public virtual void InstallWebPortalWebsite()
		{
			InstallWebsite($"{SolidCP}WebPortal", Path.Combine(InstallWebRootPath, PortalFolder), Settings.WebPortal.Urls ?? "", "", "");
		}
		public virtual void InstallWebPortal()
		{
			InstallWebPortalPrerequisites();
			ReadWebPortalConfiguration();
			CopyWebPortal();
			SetWebPortalFilePermissions();
			SetWebPortalFileOwner();
			ConfigureWebPortal();
			InstallWebPortalWebsite();
		}
		public virtual void RemoveWebPortalWebsite() { }
		public virtual void UpdateWebPortal() {
			InstallWebPortalPrerequisites();
			ReadWebPortalConfiguration();
			CopyWebPortal(ConfigAndSetupFilter);
			SetWebPortalFilePermissions();
			SetWebPortalFileOwner();
			UpdateWebPortalConfig();
			ConfigureWebPortal();
			InstallWebPortalWebsite();
		}
		public virtual void RemoveWebPortal()
		{
			RemoveWebPortalWebsite();
			RemoveWebPortalFolder();
		}
		public virtual void ReadWebPortalConfiguration()
		{
			Settings.WebPortal = new WebPortalSettings();
		}
		public virtual void UpdateWebPortalConfig() { }
		public virtual void ConfigureWebPortal() { }
		public virtual void CopyWebPortal(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, PortalFolder);
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}
	}
}