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
using SolidCP.Providers.OS;
using SolidCP.UniversalInstaller;
using System.Diagnostics;

namespace SolidCP.Setup
{
	public class Server : BaseSetup
	{
		public static object Install(object obj)
		{
			return InstallBase(obj, "1.0.1");
		}

		static bool IsWindows => OSInfo.IsWindows;

		internal static object InstallBase(object obj, string minimalInstallerVersion)
		{
			return InstallBaseRaw(obj, minimalInstallerVersion);
		}
		static object InstallBaseRaw(object obj, string minimalInstallerVersion)
		{
			Hashtable args = Utils.GetSetupParameters(obj);

			//check CS version
			string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
			Version version = new Version(shellVersion);
			//
			var setupVariables = new SetupVariables
			{
				SetupAction = SetupActions.Install,
				IISVersion = Global.IISVersion
			};
			//
			InitInstall(args, setupVariables);
			//Unattended setup
			LoadSetupVariablesFromSetupXml(setupVariables.SetupXml, setupVariables);
			//
			BaseActionManager sam = null;
			
			if (IsWindows) sam =	new ServerActionManager(setupVariables);
			else sam = new ServerUnixActionManager(setupVariables);

			// Prepare installation defaults
			sam.PrepareDistributiveDefaults();
			// Silent Installer Mode
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
					setupVariables.ServerPassword = Utils.GetStringSetupParameter(args, Global.Parameters.ServerPassword);
					//
					sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Utils.ShowConsoleErrorMessage(e.ErrorMessage);
						//
						Log.WriteError(e.ErrorMessage);
						//
						success = false;
					});
					//
					sam.Start();
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
					MessageBox.Show(String.Format(Global.Messages.InstallerVersionIsObsolete, minimalInstallerVersion), "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					//
					return DialogResult.Cancel;
				}

				var form = new InstallerForm();
				var wizard = form.Wizard;
				wizard.SetupVariables = setupVariables;
				//
				wizard.ActionManager = sam;

				//create wizard pages
				var introPage = new IntroductionPage();
				var licPage = new LicenseAgreementPage();
				//
				var page1 = new ConfigurationCheckPage();
				if (OSInfo.IsWindows)
				{
					page1.Checks.AddRange(new ConfigurationCheck[]
					{
						new ConfigurationCheck(CheckTypes.WindowsOperatingSystem, "Operating System Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement"){ SetupVariables = setupVariables }
					});
				} else
				{
					page1.Checks.AddRange(new ConfigurationCheck[]
					{
						new ConfigurationCheck(CheckTypes.OperatingSystem, "Operating System Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.Net8Runtime, ".NET 8 Runtime Requirement"){ SetupVariables = setupVariables },
						new ConfigurationCheck(CheckTypes.Systemd, "Systemd Requirement"){ SetupVariables = setupVariables },
					});
				}
				
				var page2 = new InstallFolderPage();
				var page3 = new WebPage();
				var page4 = new InsecureHttpWarningPage();
				var page5 = new CertificatePage();
				UserAccountPage page6 = null;
				if (OSInfo.IsWindows) page6 = new UserAccountPage();
				var page7 = new ServerPasswordPage();
				var page8 = new ExpressInstallPage2();
				var page9 = new FinishPage();
				
				if (OSInfo.IsWindows)
					wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5, page6, page7, page8, page9 });
				else
					wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5, page7, page8, page9 });

				wizard.LinkPages();
				wizard.SelectedPage = introPage;

				//show wizard
				IWin32Window owner = args["ParentForm"] as IWin32Window;
				return form.ShowModal(owner);
			}
		}

		public static object Uninstall(object obj)
		{
			return UninstallRaw(obj);
		}
		static object UninstallRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			//
			string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Uninstall,
				IISVersion = Global.IISVersion
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;

			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			IntroductionPage page1 = new IntroductionPage();
			ConfirmUninstallPage page2 = new ConfirmUninstallPage();
			UninstallPage page3 = new UninstallPage();
			page2.UninstallPage = page3;
			FinishPage page4 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static object Setup(object obj)
		{
			return SetupRaw(obj);
		}
		static object SetupRaw(object obj)
		{
			var args = Utils.GetSetupParameters(obj);
			var shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			//
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Setup,
				IISVersion = Global.IISVersion,
				ConfigurationFile = "web.config"
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			//
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			WebPage page1 = new WebPage();
			var page2 = new InsecureHttpWarningPage();
			var page3 = new CertificatePage();
			ServerPasswordPage page4 = new ServerPasswordPage();
			ExpressInstallPage page5 = new ExpressInstallPage();
			//create install actions
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.ConfigureLetsEncrypt);
			action.Description = "Configuring Let's Encrypt...";
			page5.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateServerPassword);
			action.Description = "Updating server password...";
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

		public static object Update(object obj)
		{
			return UpdateRaw(obj);
		}
		static object UpdateRaw(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);

			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				SetupAction = SetupActions.Update,
				BaseDirectory = Utils.GetStringSetupParameter(args, Global.Parameters.BaseDirectory),
				UpdateVersion = Utils.GetStringSetupParameter(args, "UpdateVersion"),
				InstallerFolder = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerFolder),
				Installer = Utils.GetStringSetupParameter(args, Global.Parameters.Installer),
				InstallerType = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerType),
				InstallerPath = Utils.GetStringSetupParameter(args, Global.Parameters.InstallerPath)
			};

			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			//
			wizard.SetupVariables = setupVariables;
			//
			AppConfig.LoadComponentSettings(wizard.SetupVariables);

			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			WebPage page1 = new WebPage();
			var page2 = new InsecureHttpWarningPage();
			var page3 = new CertificatePage();
			ExpressInstallPage page4 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.Backup);
			action.Description = "Backing up...";
			page4.Actions.Add(action);

			action = new InstallAction(ActionTypes.DeleteFiles);
			action.Description = "Deleting files...";
			action.Path = "setup\\delete.txt";
			page4.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page4.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page4.Actions.Add(action);

			action = new InstallAction(ActionTypes.ConfigureLetsEncrypt);
			action.Description = "Configuring Let's Encrypt...";
			page4.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page4.Actions.Add(action);

			FinishPage page5 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3, page4, page5 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			IWin32Window owner = args[Global.Parameters.ParentForm] as IWin32Window;
			return form.ShowModal(owner);
		}
	}
}
