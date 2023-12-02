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
		public WebPortalSettings WebPortalSettings { get; set; } = new WebPortalSettings();
		public virtual void InstallPortalPrerequisites() { }
		public virtual void RemovePortalPrerequisites() { }
		public virtual void SetPortalFilePermissions() => SetFilePermissions(PortalFolder);
		public virtual void InstallPortalWebsite()
		{
			InstallWebsite($"{SolidCP}WebPortal", Path.Combine(InstallWebRootPath, PortalFolder), WebPortalSettings.Urls ?? "", "", "");
		}
		public virtual void InstallWebPortal()
		{
			InstallPortalPrerequisites();
			ReadWebPortalConfiguration();
			UnzipPortal();
			InstallPortalWebsite();
			SetPortalFilePermissions();
		}
		public virtual void ReadWebPortalConfiguration()
		{
			WebPortalSettings = new WebPortalSettings();
		}
		public void ConfigureWebPortal(WebPortalSettings settings)
		{

		}
		public virtual void UnzipPortal()
		{
			var websitePath = Path.Combine(InstallWebRootPath, PortalFolder);
			UnzipFromResource("SolidCP-Portal.zip", websitePath, Net48UnzipFilter);
		}

	}
}