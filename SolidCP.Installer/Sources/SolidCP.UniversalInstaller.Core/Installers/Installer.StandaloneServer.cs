using System;
using System.Reflection;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.EnterpriseServer.Data;
using System.Globalization;
using System.Security.Policy;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml.Linq;

namespace SolidCP.UniversalInstaller
{
	public abstract partial class Installer
	{
		public virtual void InstallStandaloneServerPrerequisites() { }
		public virtual void RemoveStandaloneServerPrerequisites() { }
		public virtual void SetStandaloneServerFilePermissions() {
			SetEnterpriseServerFilePermissions();
			SetServerFilePermissions();
			SetWebPortalFilePermissions();
			SetWebDavPortalFilePermissions();
		}
		public virtual void SetStandaloneServerFileOwner()
		{
			SetEnterpriseServerFileOwner();
			SetServerFileOwner();
			SetWebPortalFileOwner();
			SetWebDavPortalFileOwner();
		}
		public virtual void SetStandaloneServerSettings() { }
		public virtual void InstallStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountInstallDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(true, StandardInstallFilter);
			SetStandaloneServerFilePermissions();
			SetStandaloneServerFileOwner();
			InstallDatabase();
			ConfigureStandaloneServer();
			InstallServerWebsite();
			InstallWebPortalWebsite();
			if (OSInfo.IsWindows) InstallWebDavPortalWebsite();
			else RemoveWebDavPortalFolder();
		}
		public virtual void UpdateStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountUpdateDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(true, StandardUpdateFilter);
			SetStandaloneServerFilePermissions();
			SetStandaloneServerFileOwner();
			UpdateDatabase();
			UpdateStandaloneServerConfig();
			ConfigureStandaloneServer();
			InstallServerWebsite();
			InstallWebPortalWebsite();
			if (OSInfo.IsWindows) InstallWebDavPortalWebsite();
			else RemoveWebDavPortalFolder();
		}
		public virtual void UpdateStandaloneServerConfig() { }

		public virtual void InstallStandaloneServerWebsite() => InstallWebPortalWebsite();
		public virtual void RemoveSetupFolder()
		{
			Directory.Delete(Path.Combine(InstallWebRootPath, "Setup"), true);
		}
		public virtual void RemoveStandaloneServerFolder()
		{
			RemoveEnterpriseServerFolder();
			RemoveServerFolder();
			RemoveWebPortalFolder();
			RemoveWebDavPortalFolder();
			RemoveSetupFolder();
		}

		public virtual void RemoveStandaloneServer()
		{
			if (OSInfo.IsWindows) RemoveWebDavPortalWebsite();
			RemoveWebPortalWebsite();
			RemoveServerWebsite();
			RemoveStandaloneServerFolder();
			DeleteDatabase();
		}
		public virtual void ReadStandaloneServerConfiguration() { }

		public virtual string PathWithSpaces(string path)
		{
			return Regex.Replace(path, "(?<=[a-z])([A-Z])", " $1");
		}
		public virtual void ConfigureStandaloneServer() {
			Settings.WebPortal.EmbedEnterpriseServer = true;
			if (!EnterpriseServerFolder.Contains(' '))
			{
				EnterpriseServerFolder = PathWithSpaces(EnterpriseServerFolder);
			}
			Settings.WebPortal.EnterpriseServerPath = Path.Combine(Settings.EnterpriseServer.InstallFolder, EnterpriseServerFolder);
			ConfigureServer();
			ConfigureEnterpriseServer();
			ConfigureWebPortal();
			if (OSInfo.IsWindows) ConfigureWebDavPortal();
		}

		public virtual void CopyStandaloneServer(bool clearDestination = false, Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = InstallWebRootPath;
			CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
		}
	}
}