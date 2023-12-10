// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SolidCP.Setup.Web;
using SolidCP.Setup.Windows;
using Microsoft.Web.Management;
using Microsoft.Web.Administration;
using Ionic.Zip;
using System.Xml;
using System.Management;
using Microsoft.Win32;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using SolidCP.Providers.OS;
using System.Runtime.Remoting.Contexts;
using System.Net.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SolidCP.Setup.Actions
{
	#region Actions

	public class InstallUnixInstallerAction : Action, IInstallAction, IUninstallAction
	{
		public const string LogStartMessage = "Install SolidCP Installer...";
		public const string LogEndMessage = "";

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			Log.WriteStart("Installing installer");

			try
			{
				var installerDir = Path.Combine(Path.GetDirectoryName(vars.InstallationFolder), "Installer");
				if (!Directory.Exists(installerDir)) Directory.CreateDirectory(installerDir);

				var exePath = Path.Combine(installerDir, Path.GetFileName(AppConfig.ConfigurationPath));

				File.Copy(AppConfig.ConfigurationPath, exePath, true);
				File.Copy(AppConfig.ConfigurationPath + ".config", exePath + ".config", true);
				var sh = Shell.Default.Find("sh");
				File.WriteAllText("/usr/bin/solidcp", $"#!{sh}\nmono {exePath}");

				OSInfo.Unix.GrantUnixPermissions("/usr/bin/solidcp", UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite |
					UnixFileMode.GroupRead | UnixFileMode.GroupExecute | UnixFileMode.GroupWrite |
					UnixFileMode.OtherExecute | UnixFileMode.OtherRead);
				OSInfo.Unix.GrantUnixPermissions(exePath, UnixFileMode.GroupExecute | UnixFileMode.GroupRead | UnixFileMode.GroupWrite |
					UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
					UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
				OSInfo.Unix.GrantUnixPermissions(exePath + ".config", UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.UserRead | UnixFileMode.UserWrite |
					UnixFileMode.OtherRead | UnixFileMode.OtherWrite);

				// creat icon
				var rscAssembly = Assembly.GetExecutingAssembly();
				var resources = rscAssembly.GetManifestResourceNames();
				var iconFileName = Path.Combine(installerDir, "SolidCP.ico");
				var iconResourceName = resources.FirstOrDefault(res => res.EndsWith("SolidCP.ico"));
				if (iconResourceName != null)
				{
					using (var rscstream = rscAssembly.GetManifestResourceStream(iconResourceName))
					using (var file = new FileStream(iconFileName, FileMode.Create, FileAccess.Write))
					{
						rscstream.CopyTo(file);
					}
				}
				else iconFileName = exePath;

				// create .desktop file
				var appdir = Environment.GetEnvironmentVariable("XDG_DATA_DIRS")?.Split(Path.PathSeparator)
					.Select(dir => Path.Combine(dir, "applications"))
					.FirstOrDefault(dir => Directory.Exists(dir)) ??
					"/usr/share/applications";

				var deskfile = appdir + "/com.solidcp.installer.desktop";
				File.WriteAllText(deskfile, $@"[Desktop Entry]
Type=Application
Name=SolidCP Installer
Comment=The Server component of the SolidCP server control panel
Exec=/usr/bin/solidcp
Icon={iconFileName}
Version={vars.Version}
Terminal=false
Caregories=Network".Replace("\r\n", Environment.NewLine));

				OSInfo.Unix.GrantUnixPermissions(deskfile, UnixFileMode.OtherExecute | UnixFileMode.GroupExecute | UnixFileMode.UserExecute |
					UnixFileMode.OtherRead | UnixFileMode.GroupRead | UnixFileMode.UserRead | UnixFileMode.GroupWrite | UnixFileMode.UserWrite);

				Log.WriteEnd("Installer installed");

				InstallLog.AppendLine("- Installed SolidCP Installer. You can run the installer with the command \"solidcp\"");

			}
			catch (Exception ex)
			{
				Log.WriteError("Installing Installer failed: ", ex);
			}
		}
		void IUninstallAction.Run(SetupVariables vars)
		{
			Log.WriteStart("Deleting installer");

			try
			{
				var appdir = Environment.GetEnvironmentVariable("XDG_DATA_DIRS") ?? "/usr/share";
				appdir += "/applications";

				var deskfile = appdir + "/com.solidcp.SolidCP.desktop";
				if (File.Exists(deskfile)) File.Delete(deskfile);

				if (File.Exists("/usr/bin/solidcp")) File.Delete("/usr/bin/solidcp");

				var installerDir = Path.Combine(vars.InstallationFolder, "Installer");
				if (Directory.Exists(installerDir))
				{
					FileUtils.DeleteDirectory(installerDir);
				}
				Log.WriteEnd("Installer deleted");

				InstallLog.AppendLine("- Removed Installer");
			}
			catch (Exception ex)
			{
				Log.WriteError("Error removing installer", ex);
			}
		}
	}

	public class InstallServerUnixAction : Action, IInstallAction, IUninstallAction
	{
		public const string LogStartMessage = "Install SolidCP Server...";
		public const string LogEndMessage = "";

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			if (String.IsNullOrEmpty(vars.InstallationFolder) || vars.InstallationFolder.Contains('\\'))
				vars.InstallationFolder = Path.Combine("/var/www/SolidCP", vars.ComponentName);
			//
			if (String.IsNullOrEmpty(vars.WebSiteDomain))
				vars.WebSiteDomain = String.Empty;


			var siteName = vars.ComponentFullName;
			var ip = vars.WebSiteIP;
			var port = vars.WebSitePort;
			var domain = vars.WebSiteDomain;
			var contentPath = vars.InstallationFolder;
			var iisVersion = vars.IISVersion;
			var iis7 = (iisVersion.Major >= 7);
			var userName = CreateWebApplicationPoolAction.GetWebIdentity(vars);
			var userPassword = vars.UserPassword;
			var appPool = vars.WebApplicationPoolName;
			var componentId = vars.ComponentId;
			var newSiteId = String.Empty;
			var urls = Utils.GetApplicationUrls(ip, domain, port, null)
				.Select(url => Utils.IsHttps(ip, domain) ? "https://" + url : "http://" + url);
			var installer = UniversalInstaller.Installer.Current;

			Begin(LogStartMessage);

			Log.WriteStart(LogStartMessage);
			installer.Shell.Log += (msg) =>
			{
				Log.Write(msg);
			};

			installer.ReadServerConfiguration();

			var settings = installer.ServerSettings;
			settings.Urls = string.Join(";", urls.ToArray());
			settings.LetsEncryptCertificateDomains = domain;
			settings.LetsEncryptCertificateEmail = vars.LetsEncryptEmail;
			settings.CryptoKey = vars.CryptoKey;
			settings.ServerPassword = vars.ServerPassword;

			installer.InstallServerPrerequisites();
			installer.InstallServerWebsite();
			installer.SetServerFilePermissions();
			installer.ConfigureServer();

			vars.VirtualDirectory = String.Empty;
			vars.NewWebSite = true;
			vars.NewVirtualDirectory = false;

			Finish(LogStartMessage);

			//update install log
			InstallLog.AppendLine("- Created a new system service SolidCPServer running the website.");
			InstallLog.AppendLine("  You can access the application by the following URLs:");
			foreach (string url in urls)
			{
				InstallLog.AppendLine("  " + url);
			}
			InstallLog.AppendLine($"- Opened the firewall for port {vars.WebSitePort}.");
			InstallLog.AppendLine("- Set file permissions on the website folder.");
			InstallLog.AppendLine("- Configured the server.");
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			var iisVersion = vars.IISVersion;
			var iis7 = (iisVersion.Major >= 7);
			var siteId = vars.WebSiteId;
			//

			Log.WriteStart("Deleting web site");

			UniversalInstaller.Installer.Current.RemoveServer();

			Log.WriteInfo(String.Format("Deleting web site \"{0}\"", siteId));
		}
	}

	public class SetCommonDistributiveParamsUnixAction : Action, IPrepareDefaultsAction
	{
		public override bool Indeterminate
		{
			get { return true; }
		}

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			if (String.IsNullOrEmpty(vars.InstallationFolder) || vars.InstallationFolder.Contains('\\'))
				vars.InstallationFolder = String.Format(@"/var/www/SolidCP/{0}", vars.ComponentName);

			if (String.IsNullOrEmpty(vars.WebSiteDomain))
				vars.WebSiteDomain = String.Empty;

			if (String.IsNullOrEmpty(vars.ConfigurationFile))
				vars.ConfigurationFile = "bin_dotnet/appsettings.json";
		}
	}

	public class SetServerUnixDefaultInstallationSettingsAction : Action, IPrepareDefaultsAction
	{
		public override bool Indeterminate
		{
			get { return true; }
		}

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.WebSiteIP))
				vars.WebSiteIP = Global.Server.DefaultIP;
			//
			if (String.IsNullOrEmpty(vars.WebSitePort))
				vars.WebSitePort = Global.Server.DefaultPort;
			//
			if (string.IsNullOrEmpty(vars.UserAccount))
				vars.UserAccount = Global.Server.ServiceAccount;
		}
	}
	#endregion

	public class ServerUnixActionManager : BaseActionManager
	{
		public static readonly List<Action> InstallScenario = new List<Action>
		{
			new SetCommonDistributiveParamsUnixAction(),
			new SetServerUnixDefaultInstallationSettingsAction(),
			new CopyFilesAction(),
			new InstallServerUnixAction(),
			new SaveComponentConfigSettingsAction(),
			new InstallUnixInstallerAction()
		};

		public ServerUnixActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(ServerActionManager_Initialize);
		}

		void ServerActionManager_Initialize(object sender, EventArgs e)
		{
			//
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install: // Install
					LoadInstallationScenario();
					break;
				default:
					break;
			}
		}

		protected virtual void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}
}
