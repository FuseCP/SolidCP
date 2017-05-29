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
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace SolidCP.Setup
{
	public class BaseSetup
	{
		public static void InitInstall(Hashtable args, SetupVariables vars)
		{
			AppConfig.LoadConfiguration();

			LoadSetupVariablesFromParameters(vars, args);

			vars.SetupAction = SetupActions.Install;
			vars.InstallationFolder = Path.Combine(Global.DefaultInstallPathRoot, vars.ComponentName);
			vars.ComponentId = Guid.NewGuid().ToString();
			vars.Instance = String.Empty;

			//create component settings node
			//vars.ComponentConfig = AppConfig.CreateComponentConfig(vars.ComponentId);
			//add default component settings
			//CreateComponentSettingsFromSetupVariables(vars, vars.ComponentId);
		}

		public static DialogResult UninstallBase(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, Global.Parameters.ShellVersion);
			var setupVariables = new SetupVariables
			{
				ComponentId = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentId),
				ComponentCode = Utils.GetStringSetupParameter(args, Global.Parameters.ComponentCode),
				SetupAction = SetupActions.Uninstall,
				IISVersion = Global.IISVersion
			};
			//
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables = setupVariables;
			//
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

		public static DialogResult SetupBase(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			string componentId = Utils.GetStringSetupParameter(args, "ComponentId");
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables.SetupAction = SetupActions.Setup;
			LoadSetupVariablesFromConfig(wizard.SetupVariables, componentId);
			wizard.SetupVariables.WebSiteId = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteId");
			wizard.SetupVariables.WebSiteIP = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteIP");
			wizard.SetupVariables.WebSitePort = AppConfig.GetComponentSettingStringValue(componentId, "WebSitePort");
			wizard.SetupVariables.WebSiteDomain = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteDomain");
			wizard.SetupVariables.NewWebSite = AppConfig.GetComponentSettingBooleanValue(componentId, "NewWebSite");
			wizard.SetupVariables.NewVirtualDirectory = AppConfig.GetComponentSettingBooleanValue(componentId, "NewVirtualDirectory");
			wizard.SetupVariables.VirtualDirectory = AppConfig.GetComponentSettingStringValue(componentId, "VirtualDirectory");
			wizard.SetupVariables.IISVersion = Utils.GetVersionSetupParameter(args, "IISVersion");
			//IntroductionPage page1 = new IntroductionPage();
			WebPage page2 = new WebPage();
			ExpressInstallPage page3 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page3.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page3.Actions.Add(action);

			FinishPage page4 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page2, page3, page4 });
			wizard.LinkPages();
			wizard.SelectedPage = page2;

			//show wizard
			IWin32Window owner = args["ParentForm"] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static DialogResult UpdateBase(object obj, string minimalInstallerVersion, string versionToUpgrade, bool updateSql)
		{
			return UpdateBase(obj, minimalInstallerVersion, versionToUpgrade, updateSql, null);
		}

		public static DialogResult UpdateBase(object obj, string minimalInstallerVersion,
			string versionsToUpgrade, bool updateSql, InstallAction versionSpecificAction)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");

			Version version = new Version(shellVersion);
			if (version < new Version(minimalInstallerVersion))
			{
				MessageBox.Show(
					string.Format("SolidCP Installer {0} or higher required.", minimalInstallerVersion),
					"Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return DialogResult.Cancel;
			}
			// Load application configuration
			AppConfig.LoadConfiguration();
			// 
			var setupVariables = new SetupVariables
			{
				SetupAction = SetupActions.Update,
				ComponentId = Utils.GetStringSetupParameter(args, "ComponentId"),
				IISVersion = Global.IISVersion,
			};
			// Load setup variables from app.config
			AppConfig.LoadComponentSettings(setupVariables);
			//
			InstallerForm form = new InstallerForm();
			form.Wizard.SetupVariables = setupVariables;
			Wizard wizard = form.Wizard;
			// Initialize setup variables with the data received from update procedure
			wizard.SetupVariables.BaseDirectory = Utils.GetStringSetupParameter(args, "BaseDirectory");
			wizard.SetupVariables.UpdateVersion = Utils.GetStringSetupParameter(args, "UpdateVersion");
			wizard.SetupVariables.InstallerFolder = Utils.GetStringSetupParameter(args, "InstallerFolder");
			wizard.SetupVariables.Installer = Utils.GetStringSetupParameter(args, "Installer");
			wizard.SetupVariables.InstallerType = Utils.GetStringSetupParameter(args, "InstallerType");
			wizard.SetupVariables.InstallerPath = Utils.GetStringSetupParameter(args, "InstallerPath");
			
			#region Support for multiple versions to upgrade from
			// Find out whether the version(s) are supported in that upgrade
			var upgradeSupported = versionsToUpgrade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Any(x => { return VersionEquals(wizard.SetupVariables.Version, x.Trim()); });
			// 
			if (upgradeSupported == false)
			{
				Log.WriteInfo(
					String.Format("Could not find a suitable version to upgrade. Current version: {0}; Versions supported: {1};", wizard.SetupVariables.Version, versionsToUpgrade));
				//
				MessageBox.Show(
					"Your current software version either is not supported or could not be upgraded at this time. Please send log file from the installer to the software vendor for further research on the issue.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				//
				return DialogResult.Cancel;
			} 
			#endregion

			//
			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			ExpressInstallPage page2 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.StopApplicationPool);
			action.Description = "Stopping IIS Application Pool...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.Backup);
			action.Description = "Backing up...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.DeleteFiles);
			action.Description = "Deleting files...";
			action.Path = "setup\\delete.txt";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page2.Actions.Add(action);

			if (versionSpecificAction != null)
				page2.Actions.Add(versionSpecificAction);

			if (updateSql)
			{
				action = new InstallAction(ActionTypes.ExecuteSql);
				action.Description = "Updating database...";
				action.Path = "setup\\update_db.sql";
				page2.Actions.Add(action);
			}

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.StartApplicationPool);
			action.Description = "Starting IIS Application Pool...";
			page2.Actions.Add(action);

			FinishPage page3 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			IWin32Window owner = args["ParentForm"] as IWin32Window;
			return form.ShowModal(owner);
		}

		protected static void LoadSetupVariablesFromSetupXml(string xml, SetupVariables setupVariables)
		{
			if (string.IsNullOrEmpty(xml))
				return;
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			XmlNodeList settings = doc.SelectNodes("settings/add");
			foreach (XmlElement node in settings)
			{
				string key = node.GetAttribute("key").ToLower();
				string value = node.GetAttribute("value");
				switch (key)
				{
					case "installationfolder":
						setupVariables.InstallationFolder = value;
						break;
					case "websitedomain":
						setupVariables.WebSiteDomain = value;
						break;
					case "websiteip":
						setupVariables.WebSiteIP = value;
						break;
					case "websiteport":
						setupVariables.WebSitePort = value;
						break;
					case "serveradminpassword":
						setupVariables.ServerAdminPassword = value;
						break;
					case "serverpassword":
						setupVariables.ServerPassword = value;
						break;
					case "useraccount":
						setupVariables.UserAccount = value;
						break;
					case "userpassword":
						setupVariables.UserPassword = value;
						break;
					case "userdomain":
						setupVariables.UserDomain = value;
						break;
					case "enterpriseserverurl":
						setupVariables.EnterpriseServerURL = value;
						break;
					case "licensekey":
						setupVariables.LicenseKey = value;
						break;
					case "dbinstallconnectionstring":
						setupVariables.DbInstallConnectionString = value;
						break;
				}
			}
		}

		public static void LoadSetupVariablesFromConfig(SetupVariables vars, string componentId)
		{
			vars.InstallationFolder = AppConfig.GetComponentSettingStringValue(componentId, "InstallFolder");
			vars.ComponentName = AppConfig.GetComponentSettingStringValue(componentId, "ComponentName");
			vars.ComponentCode = AppConfig.GetComponentSettingStringValue(componentId, "ComponentCode");
			vars.ComponentDescription = AppConfig.GetComponentSettingStringValue(componentId, "ComponentDescription");
			vars.ComponentId = componentId;
			vars.ApplicationName = AppConfig.GetComponentSettingStringValue(componentId, "ApplicationName");
			vars.Version = AppConfig.GetComponentSettingStringValue(componentId, "Release");
			vars.Instance = AppConfig.GetComponentSettingStringValue(componentId, "Instance");
		}

		public static void LoadSetupVariablesFromParameters(SetupVariables vars, Hashtable args)
		{
			vars.ApplicationName = Utils.GetStringSetupParameter(args, "ApplicationName");
			vars.ComponentName = Utils.GetStringSetupParameter(args, "ComponentName");
			vars.ComponentCode = Utils.GetStringSetupParameter(args, "ComponentCode");
			vars.ComponentDescription = Utils.GetStringSetupParameter(args, "ComponentDescription");
			vars.Version = Utils.GetStringSetupParameter(args, "Version");
			vars.InstallerFolder = Utils.GetStringSetupParameter(args, "InstallerFolder");
			vars.Installer = Utils.GetStringSetupParameter(args, "Installer");
			vars.InstallerType = Utils.GetStringSetupParameter(args, "InstallerType");
			vars.InstallerPath = Utils.GetStringSetupParameter(args, "InstallerPath");
			vars.IISVersion = Utils.GetVersionSetupParameter(args, "IISVersion");
			vars.SetupXml = Utils.GetStringSetupParameter(args, "SetupXml");

			// Add some extra variables if any, coming from SilentInstaller
			#region SilentInstaller CLI arguments
			var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
			//
			if (shellMode.Equals(Global.SilentInstallerShell, StringComparison.OrdinalIgnoreCase))
			{
				vars.WebSiteIP = Utils.GetStringSetupParameter(args, Global.Parameters.WebSiteIP);
				vars.WebSitePort = Utils.GetStringSetupParameter(args, Global.Parameters.WebSitePort);
				vars.WebSiteDomain = Utils.GetStringSetupParameter(args, Global.Parameters.WebSiteDomain);
				vars.UserDomain = Utils.GetStringSetupParameter(args, Global.Parameters.UserDomain);
				vars.UserAccount = Utils.GetStringSetupParameter(args, Global.Parameters.UserAccount);
				vars.UserPassword = Utils.GetStringSetupParameter(args, Global.Parameters.UserPassword);
			}
			#endregion
		}

		public static string GetDefaultDBName(string componentName)
		{
			return componentName.Replace(" ", string.Empty);
		}

		protected static bool VersionEquals(string version1, string version2)
		{
			Version v1 = new Version(version1);
			Version v2 = new Version(version2);
			return (v1.Major == v2.Major && v1.Minor == v2.Minor && v1.Build == v2.Build);
		}
	}
}
