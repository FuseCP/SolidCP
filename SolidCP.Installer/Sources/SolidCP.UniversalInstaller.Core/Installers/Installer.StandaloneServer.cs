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
		}
		public virtual void SetStandaloneServerFileOwner()
		{
			SetEnterpriseServerFileOwner();
			SetServerFileOwner();
			SetWebPortalFileOwner();
		}
		public virtual void SetStandaloneServerSettings() { }
		public virtual void InstallStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountInstallDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(StandardInstallFilter);
			SetStandaloneServerFilePermissions();
			SetStandaloneServerFileOwner();
			InstallDatabase();
			ConfigureStandaloneServer();
			InstallServerWebsite();
			InstallWebPortalWebsite();
		}
		public virtual void UpdateStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountUpdateDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(StandardUpdateFilter);
			SetStandaloneServerFilePermissions();
			SetStandaloneServerFileOwner();
			UpdateDatabase();
			UpdateStandaloneServerConfig();
			ConfigureStandaloneServer();
			InstallServerWebsite();
			InstallWebPortalWebsite();
		}
		public virtual void UpdateStandaloneServerConfig() { }

		public virtual void InstallStandaloneServerWebsite() => InstallWebPortalWebsite();
		public virtual void RemoveStandaloneServerFolder()
		{
			RemoveEnterpriseServerFolder();
			RemoveServerFolder();
			RemoveWebPortalFolder();
		}

		public virtual void RemoveStandaloneServer()
		{
			RemoveWebPortalWebsite();
			RemoveStandaloneServerFolder();
			DeleteDatabase();
		}
		public virtual void ReadStandaloneServerConfiguration() { }

		public virtual void ConfigureStandaloneServer() {
			ConfigureWebPortal();
			ConfigureEnterpriseServer();
			ConfigureServer();
			ConfigureEnterpriseServer(true);
			ConfigureServer(true);
		}

		public virtual void CopyStandaloneServer(Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = InstallWebRootPath;
			CopyFiles(Settings.Installer.TempPath, websitePath, filter);
		}
	}
}