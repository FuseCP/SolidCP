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
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SolidCP.UniversalInstaller;

namespace SolidCP.Setup
{
	public class BaseSetup
	{
		public InstallerSettings Settings => Installer.Current.Settings;

		static BaseSetup()
		{
#if Costura
			CosturaUtility.Initialize();
#endif
			AssemblyLoader.Init();
			AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
		}
		static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.WriteError("Remote domain error", (Exception)e.ExceptionObject);
		}

		bool argsParsed = false;
		public bool ParseArgs(object args)
		{
			if (!argsParsed)
			{
				argsParsed = true;
				if (args is string)
				{
					Installer.Current.Settings = JsonConvert.DeserializeObject<InstallerSettings>((string)args, new VersionConverter());
					return true;
				}
				else
				{
					UI.Current.ShowWarning("You need to upgrade the Installer to install this component.");
					return false;
				}
			}
			return true;
		}
		public virtual Version MinimalInstallerVersion => new Version("1.6.0");
		public virtual string VersionsToUpgrade => "";
		public bool CheckInstallerVersion()
		{
			if (Settings.Installer.Version < MinimalInstallerVersion)
			{
				UI.Current.ShowWarning("You need to upgrade the Installer to install this component.");
				return false;
			}
			else return true;
		}
		public virtual CommonSettings CommonSettings => null;
		public virtual ComponentSettings ComponentSettings => (CommonSettings as ComponentSettings) ?? Installer.Current.Settings.Standalone;
		public virtual ComponentInfo Component => null;
		public virtual UI.SetupWizard Wizard(object args, bool installFolder = true,
			bool urlWizard = true, bool userWizard = true)
		{
			if (ParseArgs(args) && CheckInstallerVersion())
			{
				var wizard = UI.Current.Wizard
					.Introduction(CommonSettings)
					.CheckPrerequisites()
					.LicenseAgreement();
				if (CommonSettings != null)
				{
					if (installFolder) wizard = wizard
						.InstallFolder(CommonSettings);

					if (urlWizard) wizard = wizard
						.Web(CommonSettings)
						.InsecureHttpWarning(CommonSettings)
						.Certificate(CommonSettings);

					if (userWizard) wizard = wizard
						.UserAccount(CommonSettings);
				} else
				{
					wizard = wizard
						.InstallFolder(Settings.Standalone)
						.Web(Settings.WebPortal)
						.InsecureHttpWarning(Settings.WebPortal)
						.Certificate(Settings.WebPortal)
						.UserAccount(Settings.WebPortal)
						.Web(Settings.EnterpriseServer)
						.InsecureHttpWarning(Settings.EnterpriseServer)
						.Certificate(Settings.EnterpriseServer)
						.UserAccount(Settings.EnterpriseServer)
						.Database()
						.Web(Settings.Server)
						.InsecureHttpWarning(Settings.Server)
						.Certificate(Settings.Server)
						.UserAccount(Settings.Server);
				}
				return wizard;
			}
			return null;
		}
		public virtual Result InstallOrSetup(object args, string title, Action installer, bool database = false, bool setup = false, int maxProgress = 100) {
			var wizard = Wizard(args, true, true, true);
			if (database) wizard = wizard.Database();
			if (setup) Installer.Current.Settings.Installer.Action = SetupActions.Setup;
			else Installer.Current.Settings.Installer.Action = SetupActions.Install;
			return wizard
				.RunWithProgress(title, installer, ComponentSettings, maxProgress)
				.Finish()
				.Show() ? Result.OK : Result.Cancel;
		}

		public bool CheckUpdate()
		{
			// Find out whether the version(s) are supported in that upgrade
			var upgradeSupported = VersionsToUpgrade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Any(x => Component?.Version == new Version(x.Trim()));
			// 
			if (!upgradeSupported)
			{
				Log.WriteInfo(
					String.Format("Could not find a suitable version to upgrade. Current version: {0}; Versions supported: {1};", Component?.Version.ToString() ?? "?", VersionsToUpgrade));
				//
				UI.Current.ShowWarning(
					"Your current software version either is not supported or could not be upgraded at this time. Please send log file from the installer to the software vendor for further research on the issue.");
				//
			}

			return upgradeSupported;
		}
		public virtual Result Update(object args, string title, Action installer, int maxProgress)
		{
			if (ParseArgs(args))
			{
				Installer.Current.Settings.Installer.Action = SetupActions.Update;

				return CheckUpdate() && Wizard(args, false, true, true)
					.RunWithProgress(title, installer, ComponentSettings, maxProgress)
					.Finish()
					.Show() ? Result.OK : Result.Cancel;
			}
			return Result.Cancel;
		}
				
		public virtual Result Uninstall(object args, string title, Action installer, int maxProgress)
		{
			if (ParseArgs(args))
			{
				Installer.Current.Settings.Installer.Action = SetupActions.Uninstall;

				if (CheckInstallerVersion())
				{
					return UI.Current.Wizard
						.Introduction(CommonSettings)
						.ConfirmUninstall(CommonSettings)
						.RunWithProgress(title, installer, ComponentSettings, maxProgress)
						.Finish()
						.Show() ? Result.OK : Result.Cancel;
				}
			}
			return Result.Cancel;
		}
	}
}
