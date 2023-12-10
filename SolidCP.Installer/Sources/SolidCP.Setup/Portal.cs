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

namespace SolidCP.Setup
{
	public class Portal : BaseSetup
	{
		public static object Install(object obj)
		{
			return InstallBase(obj, "1.0.1");
		}
		internal static object InstallBase(object obj, string minimalInstallerVersion)
		{
			ResourceAssemblyLoader.Init();
			return InstallBaseRaw(obj, minimalInstallerVersion);
		}
		static object InstallBaseRaw(object obj, string minimalInstallerVersion)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			//check CS version
			var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
			var version = new Version(Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion));
			var setupVariables = new SetupVariables
			{
				SetupAction = SetupActions.Install,
				ConfigurationFile = "web.config",
				WebSiteIP = Global.WebPortal.DefaultIP, //empty - to detect IP 
				WebSitePort = Global.WebPortal.DefaultPort,
				WebSiteDomain = String.Empty,
				NewWebSite = true,
				NewVirtualDirectory = false,
				EnterpriseServerURL = Global.WebPortal.DefaultEntServURL
			};
			//
			InitInstall(args, setupVariables);
			//
			var wam = new WebPortalActionManager(setupVariables);
			//
			wam.PrepareDistributiveDefaults();
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
					setupVariables.EnterpriseServerURL = Utils.GetStringSetupParameter(args, Global.Parameters.EnterpriseServerUrl);
					//
					wam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Utils.ShowConsoleErrorMessage(e.ErrorMessage);
						//
						Log.WriteError(e.ErrorMessage);
						//
						success = false;
					});
					//
					wam.Start();
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
					//
					MessageBox.Show(String.Format(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion), "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					//
					return DialogResult.Cancel;
				}
				//
				InstallerForm form = new InstallerForm();
				Wizard wizard = form.Wizard;
				wizard.SetupVariables = setupVariables;
				wizard.ActionManager = wam;
				//Unattended setup
				LoadSetupVariablesFromSetupXml(wizard.SetupVariables.SetupXml, wizard.SetupVariables);

				//create wizard pages
				var introPage = new IntroductionPage();
				var licPage = new LicenseAgreementPage();
				var page1 = new ConfigurationCheckPage();
                ConfigurationCheck check1 = new ConfigurationCheck(CheckTypes.WindowsOperatingSystem, "Operating System Requirement") { SetupVariables = setupVariables };
                ConfigurationCheck check2 = new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement") { SetupVariables = setupVariables };
                ConfigurationCheck check3 = new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement") { SetupVariables = setupVariables };
				page1.Checks.AddRange(new ConfigurationCheck[] { check1, check2, check3 });
				var page2 = new InstallFolderPage();
				var page3 = new WebPage();
				var page4 = new InsecureHttpWarningPage();
				var page5 = new LetsEncryptPage();
				var page6 = new UserAccountPage();
				var page7 = new UrlPage();
				var page8 = new ExpressInstallPage2();

				var page9 = new FinishPage();
				wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5, page6, page7, page8, page9 });
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
			return UninstallRaw(obj);
		}
		static DialogResult UninstallRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				IISVersion = Global.IISVersion,
				SetupAction = SetupActions.Uninstall
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);
			//
			IntroductionPage page1 = new IntroductionPage();
			ConfirmUninstallPage page2 = new ConfirmUninstallPage();
			UninstallPage page3 = new UninstallPage();
			//create uninstall currentScenario
			InstallAction action = new InstallAction(ActionTypes.DeleteShortcuts);
			action.Description = "Deleting shortcuts...";
			action.Log = "- Delete shortcuts";
			action.Name = "Login to SolidCP.url";
			page3.Actions.Add(action);
			page2.UninstallPage = page3;

			FinishPage page4 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static DialogResult Setup(object obj)
		{
			ResourceAssemblyLoader.Init();
			return SetupRaw(obj);
		}
		static DialogResult SetupRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Setup,
				IISVersion = Global.IISVersion
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);
			
			WebPage page1 = new WebPage();
			var page2 = new InsecureHttpWarningPage();
			LetsEncryptPage page3 = new LetsEncryptPage();
			UrlPage page4 = new UrlPage();
			ExpressInstallPage page5 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.ConfigureLetsEncrypt);
			action.Description = "Configure Let's Encrypt...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateEnterpriseServerUrl);
			action.Description = "Updating site settings...";
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
			//
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


			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page2.Actions.Add(action);

			FinishPage page3 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;
			
			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}
	}
}
