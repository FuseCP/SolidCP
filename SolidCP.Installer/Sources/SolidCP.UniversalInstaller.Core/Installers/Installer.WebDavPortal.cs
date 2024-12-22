using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public partial class Installer
	{
		public virtual void InstallWebDavPortalPrerequisites() { }
		public virtual void RemoveWebDavPortalPrerequisites() { }
		public virtual void SetWebDavPortalFilePermissions() => SetFilePermissions(WebDavPortalFolder);
		public virtual void SetWebDavPortalFileOwner() => SetFileOwner(WebDavPortalFolder, Settings.WebDavPortal.Username, SolidCP.ToLower());
		public virtual void InstallWebDavPortal()
		{
			InstallWebDavPortalPrerequisites();
			ReadWebDavPortalConfiguration();
			CopyWebDavPortal();
			SetWebDavPortalFilePermissions();
			SetWebDavPortalFileOwner();
			InstallWebDavPortalWebsite();
		}
		public virtual void InstallWebDavPortalWebsite()
		{
			InstallWebsite($"{SolidCP}WebDavPortal",
				Path.Combine(InstallWebRootPath, WebDavPortalFolder),
				Settings.WebDavPortal.Urls ?? "",
				"", "");
		}
		public virtual void RemoveWebDavPortalWebsite() { }
		public virtual void ReadWebDavPortalConfiguration() { }

		public virtual void ConfigureWebDavPortal(CommonSettings settings) { }

		public virtual void CopyWebDavPortal(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, WebDavPortalFolder);
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}
	}
}
