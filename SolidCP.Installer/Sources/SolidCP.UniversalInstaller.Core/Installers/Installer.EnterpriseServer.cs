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
using System.Xml.Linq;

namespace SolidCP.UniversalInstaller
{
	public abstract partial class Installer
	{
		public virtual void InstallEnterpriseServerPrerequisites() { }
		public virtual void RemoveEnterpriseServerPrerequisites() { }
		public virtual void SetEnterpriseServerFilePermissions() => SetFilePermissions(EnterpriseServerFolder);
		public virtual void InstallEnterpriseServer()
		{
			InstallEnterpriseServerPrerequisites();
			ReadEnterpriseServerConfiguration();
			UnzipEnterpriseServer();
			InstallEnterpriseServerWebsite();
			SetEnterpriseServerFilePermissions();
		}
		public virtual void InstallEnterpriseServerWebsite()
		{
			InstallWebsite($"{SolidCP}EnterpriseServer",
				Path.Combine(InstallWebRootPath, EnterpriseServerFolder),
				Settings.EnterpriseServer.Urls ?? "",
				"", "");
		}
		public virtual void RemoveEnterpriseServerWebsite() { }
		public virtual void ReadEnterpriseServerConfiguration() { }

		public virtual void ConfigureEnterpriseServer(EnterpriseServerSettings settings) { }

		public virtual void UnzipEnterpriseServer()
		{
			var websitePath = Path.Combine(InstallWebRootPath, EnterpriseServerFolder);
			UnzipFromResource("SolidCP-EnterpriseServer.zip", websitePath, UnzipFilter);
		}
	}
}