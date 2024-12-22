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
using SolidCP.UniversalInstaller.Core;

namespace SolidCP.Setup
{
	public class BaseSetup: ISetupInstaller
	{
		public InstallerSettings Settings => Installer.Current.Settings;
		static BaseSetup()
		{
#if Costura
			CosturaUtility.Initialize();
#endif
			//ResourceAssemblyLoader.Init();
			AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
		}
		static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.WriteError("Remote domain error", (Exception)e.ExceptionObject);
		}

		public void ParseArgs(string args)
		{
			Installer.Current.Settings = JsonConvert.DeserializeObject<InstallerSettings>(args, new VersionConverter());
		}

		public virtual bool Install(string args) => throw new NotImplementedException();
		public virtual bool Setup(string args) => throw new NotImplementedException();
		public virtual bool Update(string args) => throw new NotImplementedException();
		public virtual bool Uninstall(string args) => throw new NotImplementedException();
		public bool UpdateBase(string args, string minimalInstallerVersion, string versionToUpgrade, bool updateSql)
		{
			return UpdateBase(args, minimalInstallerVersion, versionToUpgrade, updateSql, null);
		}

		public bool UpdateBase(string args, string minimalInstallerVersion,
			string versionsToUpgrade, bool updateSql, Action versionSpecificAction)
		{
			ParseArgs(args);
			Version version = Settings.Installer.Version;
			if (version < new Version(minimalInstallerVersion))
			{
				UI.Current.ShowWarning($"SolidCP Installer {minimalInstallerVersion} or higher required.");
				return false;
			}
			
			#region Support for multiple versions to upgrade from
			// Find out whether the version(s) are supported in that upgrade
			var upgradeSupported = versionsToUpgrade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Any(x => Settings.Installer.Version == new Version(x.Trim()));
			// 
			if (!upgradeSupported)
			{
				Log.WriteInfo(
					String.Format("Could not find a suitable version to upgrade. Current version: {0}; Versions supported: {1};", Settings.Installer.Version, versionsToUpgrade));
				//
				UI.Current.ShowWarning(
					"Your current software version either is not supported or could not be upgraded at this time. Please send log file from the installer to the software vendor for further research on the issue.");
				//
				return false;
			}
			#endregion

			return true;
			/* return UI.Current.Wizard
				.Introduction()
				.LicenseAgreement()
				.Progress()
				.Finish()
				.Show(); */
		}
	}
}
