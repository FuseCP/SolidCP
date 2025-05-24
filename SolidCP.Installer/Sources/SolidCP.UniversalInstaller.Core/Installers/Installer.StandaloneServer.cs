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
		public virtual void CreateStandaloneUsers()
		{
			CreateServerUser();
			if (!Settings.WebPortal.EmbedEnterpriseServer) CreateEnterpriseServerUser();
			CreateWebPortalUser();
			if (OSInfo.IsWindows) CreateWebDavPortalUser();
		}

		public virtual void RemoveStandaloneUsers()
		{
			RemoveServerUser();
			if (!Settings.WebPortal.EmbedEnterpriseServer) RemoveEnterpriseServerUser();
			RemoveWebPortalUser();
			if (OSInfo.IsWindows) RemoveWebDavPortalUser();
		}
		public virtual void SetStandaloneServerFilePermissions() {
			SetEnterpriseServerFilePermissions();
			SetServerFilePermissions();
			SetWebPortalFilePermissions();
			if (OSInfo.IsWindows) SetWebDavPortalFilePermissions();
		}
		public virtual void SetStandaloneServerFileOwner()
		{
			SetEnterpriseServerFileOwner();
			SetServerFileOwner();
			SetWebPortalFileOwner();
			if (OSInfo.IsWindows) SetWebDavPortalFileOwner();
		}
		public virtual void SetStandaloneServerSettings() { }
		public virtual void InstallStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountInstallDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(true, StandaloneInstallFilter);
			CreateStandaloneUsers();
			SetStandaloneServerFilePermissions();
			SetStandaloneServerFileOwner();
			InstallDatabase();
			ConfigureStandaloneServer();
			InstallServerWebsite();
			InstallWebPortalWebsite();
			if (OSInfo.IsWindows) InstallWebDavPortalWebsite();
			else RemoveWebDavPortalFolder();
		}
		public virtual void UpdateStandaloneServerConfig() {
			UpdateServerConfig();
			UpdateEnterpriseServerConfig();
			UpdateWebPortalConfig();
			if (OSInfo.IsWindows) UpdateWebDavPortalConfig();
		}
		public virtual void UpdateStandaloneServer()
		{
			ResetEstimatedOutputLines();
			CountUpdateDatabaseStatements();
			InstallStandaloneServerPrerequisites();
			CopyStandaloneServer(true, StandaloneUpdateFiler);
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

		public virtual void SetupStandaloneServer()
		{
			RemoveServerWebsite();
			RemoveWebPortalWebsite();
			if (OSInfo.IsWindows) RemoveWebDavPortalWebsite();
			ConfigureStandaloneServer();
			InstallServerWebsite();
			InstallWebPortalWebsite();
			if (OSInfo.IsWindows) InstallWebDavPortalWebsite();
		}
		public virtual void InstallStandaloneServerWebsite() => InstallWebPortalWebsite();
		public virtual void RemoveSetupFolder()
		{
			var dir = Path.Combine(Settings.Standalone.InstallPath, "Setup");
			if (Directory.Exists(dir)) Directory.Delete(dir, true);
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
			RemoveWebDavPortalWebsite();
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
			Settings.WebPortal.EnterpriseServerPath = $"..{Path.DirectorySeparatorChar}{Settings.EnterpriseServer.InstallFolder}";
			ConfigureServer();
			ConfigureEnterpriseServer();
			ConfigureWebPortal();
			if (OSInfo.IsWindows) ConfigureWebDavPortal();
		}

		public virtual void CopyStandaloneServer(bool clearDestination = false, Func<string, string> filter = null)
		{
			filter ??= SetupFilter;
			var websitePath = Settings.Standalone.InstallPath;
			InstallWebRootPath = Path.GetDirectoryName(websitePath);
			CopyFiles(ComponentTempPath, websitePath, clearDestination, filter);
		}
	}
}