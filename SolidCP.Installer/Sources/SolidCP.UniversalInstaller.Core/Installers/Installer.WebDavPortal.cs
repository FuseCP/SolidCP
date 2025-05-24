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
		public virtual void CreateWebDavPortalUser() => CreateUser(Settings.WebDavPortal);
		public virtual void RemoveWebDavPortalUser() => RemoveUser(Settings.WebDavPortal.Username);
		public virtual void SetWebDavPortalFilePermissions() => SetFilePermissions(Settings.WebDavPortal.InstallPath);
		public virtual void SetWebDavPortalFileOwner() => SetFileOwner(Settings.WebDavPortal.InstallPath, Settings.WebDavPortal.Username, SolidCPGroup);
		public virtual void InstallWebDavPortal()
		{
			InstallWebDavPortalPrerequisites();
			ReadWebDavPortalConfiguration();
			CopyWebDavPortal(true);//, //this.StandardInstallFilter);
			CreateWebDavPortalUser();
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
			var web = Settings.WebDavPortal.InstallPath;
			var dll = Path.Combine(web, "bin_dotnet", "SolidCP.WebDavPortal.dll");
			InstallWebsite(WebDavPortalSiteId,
				web,
				Settings.WebDavPortal,
				SolidCPUnixGroup,
				dll,
				"SolidCP.WebDavPortal service, the WebDavPortal for the SolidCP control panel.",
				UnixWebDavPortalServiceId);
		}
		public virtual void SetupWebDavPortal()
		{
			RemoveWebDavPortalWebsite();
			ConfigureWebDavPortal();
			InstallWebDavPortalWebsite();
		}
		public virtual void RemoveWebDavPortalWebsite() {
			RemoveWebsite(WebDavPortalSiteId, Settings.WebDavPortal);
		}
		public virtual void ReadWebDavPortalConfiguration() { }
		public virtual void RemoveWebDavPortal()
		{
			RemoveWebDavPortalWebsite();
			RemoveWebDavPortalFolder();
			RemoveWebDavPortalUser();
		}
		public virtual void RemoveWebDavPortalFolder()
		{
			var dir = Settings.WebDavPortal.InstallPath;
			if (Directory.Exists(dir)) Directory.Delete(dir, true);
			InstallLog("Removed WebDavPortal files");
		}

		public virtual void UpdateWebDavPortalConfig() { }
		public virtual void ConfigureWebDavPortal() { }

		public virtual void CopyWebDavPortal(bool clearDestination = false, Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Settings.WebDavPortal.InstallPath;
			InstallWebRootPath = Path.GetDirectoryName(websitePath);
			CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
		}
	}
}
