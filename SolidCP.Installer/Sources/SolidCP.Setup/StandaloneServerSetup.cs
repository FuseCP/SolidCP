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
using System.IO;
using System.Xml;
using System.Configuration;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Policy;
using SolidCP.Setup.Web;
using SolidCP.Setup.Actions;
using Data = SolidCP.EnterpriseServer.Data;
using SolidCP.UniversalInstaller.Core;

namespace SolidCP.Setup
{
	public class StandaloneServerSetup : BaseSetup
	{
		public static object Install(object obj)
		{
			return InstallBase(obj, "1.0.6");
		}

		internal static object InstallBase(object obj, string minimalInstallerVersion)
		{
			ResourceAssemblyLoader.Init();
			return InstallBaseRaw(obj, minimalInstallerVersion);
		}
		static object InstallBaseRaw(object obj, string minimalInstallerVersion)
		{
			ResourceAssemblyLoader.Init();

			Hashtable args = Utils.GetSetupParameters(obj);

			//check CS version
			string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
			Version version = new Version(shellVersion);

			//********************  Server ****************
			var serverSetup = new SetupVariables
			{
				ComponentId = Guid.NewGuid().ToString(),
				Instance = String.Empty,
				ComponentName = Global.Server.ComponentName,
				ComponentCode = Global.Server.ComponentCode,
				ComponentDescription = Global.Server.ComponentDescription,
				//
				ServerPassword = Guid.NewGuid().ToString("N").Substring(0, 10),
				//
				SetupAction = SetupActions.Install,
				IISVersion = Global.IISVersion,
				ApplicationName = Utils.GetStringSetupParameter(args, Global.Parameters.ApplicationName),
				Version = Utils.GetStringSetupParameter(args, Global.Parameters.Version),
				Installer = Utils.GetStringSetupParameter(args, Global.Parameters.Installer),
				InstallerPath = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerPath),
				SetupXml = Utils.GetStringSetupParameter(args, Global.Parameters.SetupXml),
				//
				InstallerFolder = Path.Combine(Utils.GetStringSetupParameter(args, Global.Parameters.InstallerFolder), Global.Server.ComponentName),
				InstallerType = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerType).Replace(Global.StandaloneServer.SetupController, Global.Server.SetupController),
				InstallationFolder = Path.Combine(Path.Combine(Utils.GetSystemDrive(), "SolidCP"), Global.Server.ComponentName),
				ConfigurationFile = "web.config",
			};
			// Load config file
			AppConfig.LoadConfiguration();
			//
			LoadComponentVariablesFromSetupXml(serverSetup.ComponentCode, serverSetup.SetupXml, serverSetup);
			//
			//serverSetup.ComponentConfig = AppConfig.CreateComponentConfig(serverSetup.ComponentId);
			//serverSetup.RemoteServerUrl = GetUrl(serverSetup.WebSiteDomain, serverSetup.WebSiteIP, serverSetup.WebSitePort);
			//
			//CreateComponentSettingsFromSetupVariables(serverSetup, serverSetup.ComponentId);

			//********************  Enterprise Server ****************
			var esServerSetup = new SetupVariables
			{
				ComponentId = Guid.NewGuid().ToString(),
				SetupAction = SetupActions.Install,
				IISVersion = Global.IISVersion,
				//
				Instance = String.Empty,
				ComponentName = Global.EntServer.ComponentName,
				ComponentCode = Global.EntServer.ComponentCode,
				ApplicationName = Utils.GetStringSetupParameter(args, Global.Parameters.ApplicationName),
				Version = Utils.GetStringSetupParameter(args, Global.Parameters.Version),
				ComponentDescription = Global.EntServer.ComponentDescription,
				Installer = Utils.GetStringSetupParameter(args, Global.Parameters.Installer),
				InstallerFolder = Path.Combine(Utils.GetStringSetupParameter(args, Global.Parameters.InstallerFolder), Global.EntServer.ComponentName),
				InstallerType = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerType).Replace(Global.StandaloneServer.SetupController, Global.EntServer.SetupController),
				InstallationFolder = Path.Combine(Path.Combine(Utils.GetSystemDrive(), "SolidCP"), Global.EntServer.ComponentName),
				InstallerPath = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerPath),
				SetupXml = Utils.GetStringSetupParameter(args, Global.Parameters.SetupXml),
				//
				ConfigurationFile = "web.config",
				ConnectionString = Global.EntServer.AspNetConnectionStringFormat,
				DatabaseServer = Global.EntServer.DefaultDbServer,
				Database = Global.EntServer.DefaultDatabase,
				CreateDatabase = true,
				UpdateServerAdminPassword = true,
				//
				WebSiteIP = Global.EntServer.DefaultIP,
				WebSitePort = Global.EntServer.DefaultPort,
				WebSiteDomain = String.Empty,
			};
			//
			LoadComponentVariablesFromSetupXml(esServerSetup.ComponentCode, esServerSetup.SetupXml, esServerSetup);
			//
			//esServerSetup.ComponentConfig = AppConfig.CreateComponentConfig(esServerSetup.ComponentId);
			//
			//CreateComponentSettingsFromSetupVariables(esServerSetup, esServerSetup.ComponentId);

			//********************  Portal ****************
			#region Portal Setup Variables
			var portalSetup = new SetupVariables
				{
					ComponentId = Guid.NewGuid().ToString(),
					SetupAction = SetupActions.Install,
					IISVersion = Global.IISVersion,
					//
					Instance = String.Empty,
					ComponentName = Global.WebPortal.ComponentName,
					ComponentCode = Global.WebPortal.ComponentCode,
					ApplicationName = Utils.GetStringSetupParameter(args, Global.Parameters.ApplicationName),
					Version = Utils.GetStringSetupParameter(args, Global.Parameters.Version),
					ComponentDescription = Global.WebPortal.ComponentDescription,
					Installer = Utils.GetStringSetupParameter(args, Global.Parameters.Installer),
					InstallerFolder = Path.Combine(Utils.GetStringSetupParameter(args, Global.Parameters.InstallerFolder), Global.WebPortal.ComponentName),
					InstallerType = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerType).Replace(Global.StandaloneServer.SetupController, Global.WebPortal.SetupController),
					InstallationFolder = Path.Combine(Path.Combine(Utils.GetSystemDrive(), "SolidCP"), Global.WebPortal.ComponentName),
					InstallerPath = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerPath),
					SetupXml = Utils.GetStringSetupParameter(args, Global.Parameters.SetupXml),
					//
					ConfigurationFile = "web.config",
					EnterpriseServerURL = Global.WebPortal.DefaultEntServURL,
				};
			//
			LoadComponentVariablesFromSetupXml(portalSetup.ComponentCode, portalSetup.SetupXml, portalSetup);
			//
			//portalSetup.ComponentConfig = AppConfig.CreateComponentConfig(portalSetup.ComponentId);
			//
			//CreateComponentSettingsFromSetupVariables(portalSetup, portalSetup.ComponentId); 
			#endregion

			//
			var stdssam = new StandaloneServerActionManager(serverSetup, esServerSetup, portalSetup);
			//
			stdssam.PrepareDistributiveDefaults();

			//
			if (shellMode.Equals(Global.SilentInstallerShell, StringComparison.OrdinalIgnoreCase))
			{
				// Validate the setup controller's bootstrapper version
				if (version < new Version(minimalInstallerVersion))
				{
					Utils.ShowConsoleErrorMessage(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion);
					//
					return false;
				}

				try
				{
					var success = true;
					
					// Retrieve SolidCP Enterprise Server component's settings from the command-line
					var adminPassword = Utils.GetStringSetupParameter(args, Global.Parameters.ServerAdminPassword);
					// This has been designed to make an installation process via Web PI more secure
					if (String.IsNullOrEmpty(adminPassword))
					{
						// Set serveradmin password
						esServerSetup.ServerAdminPassword = Guid.NewGuid().ToString();
						// Set peer admin password
						esServerSetup.PeerAdminPassword = Guid.NewGuid().ToString();
						// Instruct provisioning scenario to enter the application in SCPA mode (Setup Control Panel Acounts)
						esServerSetup.EnableScpaMode = true;
					}
					else
					{
						esServerSetup.ServerAdminPassword = esServerSetup.PeerAdminPassword = adminPassword;
					}
					//
					esServerSetup.Database = Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseName);
					Data.DbType dbType = Data.DbType.Unknown;
					Enum.TryParse(Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseType), out dbType);
					esServerSetup.DatabaseType = dbType;
					esServerSetup.DatabasePort = (int)(Utils.GetSetupParameter(args, Global.Parameters.DatabasePort) ?? 0);
					esServerSetup.DatabaseServer = Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseServer);

					switch (dbType)
					{
						case Data.DbType.SqlServer:
							// DB_LOGIN, DB_PASSWORD.
							bool WinAuth = Utils.GetStringSetupParameter(args, "DbAuth").ToLowerInvariant().Equals("Windows Authentication".ToLowerInvariant());
							esServerSetup.DbInstallConnectionString = SqlUtils.BuildMsSqlServerMasterConnectionString(
														esServerSetup.DatabaseServer,
														WinAuth ? null : Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdmin),
														WinAuth ? null : Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdminPassword));
							break;
						case Data.DbType.MySql:
						case Data.DbType.MariaDb:
							esServerSetup.DbInstallConnectionString = SqlUtils.BuildMySqlServerMasterConnectionString(
														esServerSetup.DatabaseServer,
														esServerSetup.DatabasePort,
														Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdmin),
														Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdminPassword));
							break;
						case Data.DbType.Sqlite:
						case Data.DbType.SqliteFX:
							esServerSetup.DbInstallConnectionString = SqlUtils.BuildSqliteMasterConnectionString(esServerSetup.Database, esServerSetup.InstallationFolder, esServerSetup.EnterpriseServerPath, esServerSetup.EmbedEnterpriseServer);
							break;
						default: throw new NotSupportedException("This database type is not supported.");
					}

					//
					stdssam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Utils.ShowConsoleErrorMessage(e.ErrorMessage);
						//
						Log.WriteError(e.ErrorMessage);
						//
						success = false;
					});
					//
					stdssam.Start();
					//
					return success;
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to install the component", ex);
					//
					return false;
				}
			}
			else
			{
				// Validate the setup controller's bootstrapper version
				if (version < new Version(minimalInstallerVersion))
				{
					MessageBox.Show(String.Format(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion),
						"Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					//
					return DialogResult.Cancel;
				}

				// NOTE: there is no assignment to SetupVariables property of the wizard as usually because we have three components 
				// to setup here and thus we have created SwapSetupVariablesAction setup action to swap corresponding variables 
				// back and forth while executing the installation scenario.
				InstallerForm form = new InstallerForm();
				Wizard wizard = form.Wizard;
				wizard.SetupVariables = serverSetup;
				// Assign corresponding action manager to the wizard.
				wizard.ActionManager = stdssam;
				// Initialize wizard pages and their properties
				var introPage = new IntroductionPage();
				var licPage = new LicenseAgreementPage();
				var page2 = new ConfigurationCheckPage();			
				// Setup prerequisites validation
				page2.Checks.AddRange(new ConfigurationCheck[] { 
					new ConfigurationCheck(CheckTypes.WindowsOperatingSystem, "Operating System Requirement"){ SetupVariables = serverSetup }, 
					new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement"){ SetupVariables = serverSetup }, 
					new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement"){ SetupVariables = serverSetup }, 
					// Validate Server installation prerequisites
					new ConfigurationCheck(CheckTypes.SCPServer, "SolidCP Server Requirement") { SetupVariables = serverSetup }, 
					// Validate EnterpriseServer installation prerequisites
					new ConfigurationCheck(CheckTypes.SCPEnterpriseServer, "SolidCP Enterprise Server Requirement") { SetupVariables = esServerSetup }, 
					// Validate WebPortal installation prerequisites
					new ConfigurationCheck(CheckTypes.SCPPortal, "SolidCP Portal Requirement") { SetupVariables = portalSetup }
				});
				// Assign WebPortal setup variables set to acquire corresponding settings
				var page3 = new WebPage { SetupVariables = portalSetup };
				var page4 = new InsecureHttpWarningPage() { SetupVariables = portalSetup };
				var page5 = new CertificatePage { SetupVariables = portalSetup };
				var page6 = new EmbedEnterpriseServerPage { SetupVariables = portalSetup };
				// Assign EnterpriseServer setup variables set to acquire corresponding settings
				var page7 = new DatabasePage { SetupVariables = esServerSetup };
				// Assign EnterpriseServer setup variables set to acquire corresponding settings
				var page8 = new ServerAdminPasswordPage
				{
					SetupVariables = esServerSetup,
					NoteText = "Note: Both serveradmin and admin accounts will use this password. You can always change password for serveradmin or admin accounts through control panel."
				};
				//
				var page9 = new ExpressInstallPage2();
				// Assign WebPortal setup variables set to acquire corresponding settings
				var page10 = new SetupCompletePage { SetupVariables = portalSetup };
				//
				wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3, page4, page5, page6, page7, page8, page9, page10 });
				wizard.LinkPages();
				wizard.SelectedPage = introPage;
				// Run wizard
				IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
				return form.ShowModal(owner);
			}
		}

		public static DialogResult Uninstall(object obj)
		{
			ResourceAssemblyLoader.Init();
			MessageBox.Show("Functionality is not supported.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return DialogResult.Cancel;
		}

		public static DialogResult Setup(object obj)
		{
			ResourceAssemblyLoader.Init();
			MessageBox.Show("Functionality is not supported.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return DialogResult.Cancel;
		}

		public static DialogResult Update(object obj)
		{
			ResourceAssemblyLoader.Init();
			MessageBox.Show("Functionality is not supported.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return DialogResult.Cancel;
		}

		protected static void LoadComponentVariablesFromSetupXml(string componentCode, string xml, SetupVariables setupVariables)
		{
			if (string.IsNullOrEmpty(componentCode))
				return;

			if (string.IsNullOrEmpty(xml))
				return;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xml);

				string xpath = string.Format("components/component[@code=\"{0}\"]", componentCode);

				XmlNode componentNode = doc.SelectSingleNode(xpath);
				if (componentNode != null)
				{
					LoadSetupVariablesFromSetupXml(componentNode.InnerXml, setupVariables);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Unattended setup error", ex);
				throw;
			}
		}

		/*private static string GetUrl(string domain, string ip, string port)
		{
			// TODO https or http?
			string address = "https://";
			string server = string.Empty;
			string ipPort = string.Empty;
			//server 
			if (domain != null && domain.Trim().Length > 0)
			{
				//domain 
				server = domain.Trim();
			}
			else
			{
				//ip
				if (ip != null && ip.Trim().Length > 0)
				{
					server = ip.Trim();
				}
			}
			//port
			if (server.Length > 0 &&
				ip.Trim().Length > 0 &&
				ip.Trim() != "80")
			{
				ipPort = ":" + port.Trim();
			}

			//address string
			address += server + ipPort;
			return address;
		}*/

		

	}
}
 
