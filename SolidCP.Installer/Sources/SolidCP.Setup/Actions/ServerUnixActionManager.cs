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

namespace SolidCP.Setup.Actions
{
	#region Actions
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
				.Select(url => Utils.IsLocal(ip, domain) ? "http://" + url : "https://" + url);
			var installer = UniversalInstaller.Installer.Current;

			Begin(LogStartMessage);
			
			Log.WriteStart(LogStartMessage);
			installer.Shell.Log += (msg) =>
			{
				Log.Write(msg);
				InstallLog.Append(msg);
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

			Finish(LogStartMessage);

			//update install log
			InstallLog.AppendLine(string.Format("- Created a new web site named \"{0}\" ({1})", siteName, newSiteId));
			InstallLog.AppendLine("  You can access the application by the following URLs:");
			foreach (string url in urls)
			{
				InstallLog.AppendLine("  " + url);
			}
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			var iisVersion = vars.IISVersion;
			var iis7 = (iisVersion.Major >= 7);
			var siteId = vars.WebSiteId;
			//

			Log.WriteStart("Deleting web site");
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
			if (String.IsNullOrEmpty(vars.InstallationFolder))
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
			new SaveComponentConfigSettingsAction()
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
