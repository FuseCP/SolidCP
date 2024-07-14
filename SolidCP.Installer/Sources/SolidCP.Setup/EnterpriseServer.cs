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
using SolidCP.Setup.Actions;
using Data = SolidCP.EnterpriseServer.Data;
using SolidCP.UniversalInstaller.Core;

namespace SolidCP.Setup
{
	public class EnterpriseServer : BaseSetup
	{
		public static object Install(object obj)
		{
			ResourceAssemblyLoader.Init();
			return InstallBase(obj, "1.0.1");
		}
		public static object InstallBase(object obj, string minimalInstallerVersion)
		{
			ResourceAssemblyLoader.Init();
			return InstallBaseRaw(obj, minimalInstallerVersion);
		}
		static object InstallBaseRaw(object obj, string minimalInstallerVersion)
		{
			var args = Utils.GetSetupParameters(obj);
			//check CS version
			var shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
			var version = new Version(shellVersion);

			var setupVariables = new SetupVariables
			{
				ConnectionString = Global.EntServer.AspNetConnectionStringFormat,
				DatabaseServer = Global.EntServer.DefaultDbServer,
				Database = Global.EntServer.DefaultDatabase,
				CreateDatabase = true,
				WebSiteIP = Global.EntServer.DefaultIP,
				WebSitePort = Global.EntServer.DefaultPort,
				WebSiteDomain = String.Empty,
				NewWebSite = true,
				NewVirtualDirectory = false,
				ConfigurationFile = "web.config",
				UpdateServerAdminPassword = true,
				ServerAdminPassword = "",
			};

			//
			InitInstall(args, setupVariables);
			//
			var eam = new EntServerActionManager(setupVariables);
			//
			eam.PrepareDistributiveDefaults();
			//
			if (shellMode.Equals(Global.SilentInstallerShell, StringComparison.OrdinalIgnoreCase))
			{
				if (version < new Version(minimalInstallerVersion))
				{
					Utils.ShowConsoleErrorMessage(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion);
					//
					return false;
				}

				try
				{
					var success = true;
					//
					setupVariables.ServerAdminPassword = Utils.GetStringSetupParameter(args, Global.Parameters.ServerAdminPassword);
					setupVariables.Database = Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseName);
					setupVariables.DatabaseServer = Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseServer);
					setupVariables.DatabasePort = (int)Utils.GetSetupParameter(args, Global.Parameters.DatabasePort);
					Data.DbType dbType = Data.DbType.Unknown;
					Enum.TryParse(Utils.GetStringSetupParameter(args, Global.Parameters.DatabaseType), out dbType);
					setupVariables.DatabaseType = dbType;
					//
					switch (setupVariables.DatabaseType)
					{
						case Data.DbType.SqlServer:
							setupVariables.DbInstallConnectionString = Data.DatabaseUtils.BuildSqlServerMasterConnectionString(
								setupVariables.DatabaseServer,
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdmin),
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdminPassword)
							);
							break;
						case Data.DbType.MySql:
						case Data.DbType.MariaDb:
							setupVariables.DbInstallConnectionString = Data.DatabaseUtils.BuildMySqlMasterConnectionString(
								setupVariables.DatabaseServer,
								setupVariables.DatabasePort,
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdmin),
								Utils.GetStringSetupParameter(args, Global.Parameters.DbServerAdminPassword)
							);
							break;
						case Data.DbType.Sqlite:
						case Data.DbType.SqliteFX:
							setupVariables.DbInstallConnectionString =
								Data.DatabaseUtils.BuildSqliteMasterConnectionString(setupVariables.Database,
									setupVariables.InstallationFolder, setupVariables.EnterpriseServerPath, setupVariables.EmbedEnterpriseServer);
							break;
						default: break;
					}
					//
					eam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Utils.ShowConsoleErrorMessage(e.ErrorMessage);
						//
						Log.WriteError(e.ErrorMessage);
						//
						success = false;
					});
					//
					eam.Start();
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
				if (version < new Version(minimalInstallerVersion))
				{
					MessageBox.Show(string.Format(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion), "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return DialogResult.Cancel;
				}

				InstallerForm form = new InstallerForm();
				Wizard wizard = form.Wizard;
				wizard.SetupVariables = setupVariables;
				wizard.ActionManager = eam;

				//Unattended setup
				LoadSetupVariablesFromSetupXml(wizard.SetupVariables.SetupXml, wizard.SetupVariables);
				//create wizard pages
				var introPage = new IntroductionPage();
				var licPage = new LicenseAgreementPage();
				var page1 = new ConfigurationCheckPage();
				//
                ConfigurationCheck check1 = new ConfigurationCheck(CheckTypes.WindowsOperatingSystem, "Operating System Requirement") { SetupVariables = setupVariables };
                ConfigurationCheck check2 = new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement") { SetupVariables = setupVariables };
                ConfigurationCheck check3 = new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement") { SetupVariables = setupVariables };
				//
				page1.Checks.AddRange(new ConfigurationCheck[] { check1, check2, check3 });
				//
				var page2 = new InstallFolderPage();
				var page3 = new WebPage();
				var page4 = new InsecureHttpWarningPage();
				var page5 = new CertificatePage();
				var page6 = new UserAccountPage();
				var page7 = new DatabasePage();
				var passwordPage = new ServerAdminPasswordPage();
				//
				var page8 = new ExpressInstallPage2();
				//
				var page9 = new FinishPage();
				wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5, page6, page7, passwordPage, page8, page9 });
				wizard.LinkPages();
				wizard.SelectedPage = introPage;

				//show wizard
				IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
				return form.ShowModal(owner);
			}
		}

		public static DialogResult Uninstall(object obj)
		{
			ResourceAssemblyLoader.Init();
			return UninstallBase(obj);
		}

		public static DialogResult Setup(object obj)
		{
			ResourceAssemblyLoader.Init();
			return SetupRaw(obj);
		}
		static DialogResult SetupRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				ConfigurationFile = "web.config",
				IISVersion = Global.IISVersion,
				SetupAction = SetupActions.Setup
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			//IntroductionPage page1 = new IntroductionPage();
			WebPage page1 = new WebPage();
			var page2 = new InsecureHttpWarningPage();
			CertificatePage page3 = new CertificatePage();
			ServerAdminPasswordPage page4 = new ServerAdminPasswordPage();
			ExpressInstallPage page5 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.ConfigureLetsEncrypt);
			action.Description = "Configure Let's Encrypt...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateServerAdminPassword);
			action.Description = "Updating serveradmin password...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page5.Actions.Add(action);


            FinishPage page6 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4, page5, page6 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static DialogResult Update(object obj)
		{
			ResourceAssemblyLoader.Init();
			return UpdateRaw(obj);
		}
		static DialogResult UpdateRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);

			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Update,
				IISVersion = Global.IISVersion
			};

			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			ExpressInstallPage page2 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.Backup);
			action.Description = "Backing up...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.DeleteFiles);
			action.Description = "Deleting files...";
			action.Path = "setup\\delete.txt";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.ExecuteSql);
			action.Description = "Updating database...";
			action.Path = "setup\\update_db.sql";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page2.Actions.Add(action);

			FinishPage page3 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			Form parentForm = args[Global.Parameters.ParentForm] as Form;
			return form.ShowDialog(parentForm);
		}
	}
}
