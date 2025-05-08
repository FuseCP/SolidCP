using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{
	public partial class Installer
	{
		public virtual string WebDavPortalSiteId => $"{SolidCP}WebDavPortal";
		public virtual string UnixWebDavPortalServiceId => "solidcp-webdavportal";
		public virtual void InstallWebDavPortalPrerequisites() { }
		public virtual void RemoveWebDavPortalPrerequisites() { }
		public virtual void SetWebDavPortalFilePermissions() => SetFilePermissions(WebDavPortalFolder);
		public virtual void SetWebDavPortalFileOwner() => SetFileOwner(WebDavPortalFolder, Settings.WebDavPortal.Username, SolidCP.ToLower());
		public virtual void InstallWebDavPortal()
		{
			InstallWebDavPortalPrerequisites();
			ReadWebDavPortalConfiguration();
			CopyWebDavPortal(true, StandardInstallFilter);
			SetWebDavPortalFilePermissions();
			SetWebDavPortalFileOwner();
			ConfigureWebDavPortal();
			InstallWebDavPortalWebsite();
		}
		public virtual void UpdateWebDavPortal()
		{
			InstallWebDavPortalPrerequisites();
			ReadWebDavPortalConfiguration();
			CopyWebDavPortal(true, StandardUpdateFilter);
			SetWebDavPortalFilePermissions();
			SetWebDavPortalFileOwner();
			UpdateWebDavPortalConfig();
			ConfigureWebDavPortal();
			InstallWebDavPortalWebsite();
		}
		public virtual void InstallWebDavPortalWebsite()
		{
			var web = Path.Combine(InstallWebRootPath, WebDavPortalFolder);
			var dll = Path.Combine(web, "bin_dotnet", "SolidCP.WebDavPortal.dll");
			InstallWebsite(WebDavPortalSiteId,
				web,
				Settings.WebDavPortal,
				UnixWebDavPortalServiceId,
				dll,
				"SolidCP.WebDavPortal service, the WebDavPortal for the SolidCP control panel.",
				UnixWebDavPortalServiceId);
		}
		public virtual void RemoveWebDavPortalWebsite() {
			RemoveWebsite(WebDavPortalSiteId, Settings.WebDavPortal);
		}
		public virtual void ReadWebDavPortalConfiguration() { }
		public virtual void RemoveWebDavPortal()
		{
			RemoveWebDavPortalWebsite();
			RemoveWebDavPortalFolder();
		}
		public virtual void RemoveWebDavPortalFolder()
		{
			Directory.Delete(Path.Combine(InstallWebRootPath, WebDavPortalFolder), true);
		}

		public virtual void UpdateWebDavPortalConfig() { }
		public virtual void ConfigureWebDavPortal() { }

		public virtual void CopyWebDavPortal(bool clearDestination = false, Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Path.Combine(InstallWebRootPath, WebDavPortalFolder);
			CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
		}
	}
}
