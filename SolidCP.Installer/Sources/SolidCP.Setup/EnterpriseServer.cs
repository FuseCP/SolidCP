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
using System.Collections;
using System.Reflection;
using System.Text;
using Data = SolidCP.EnterpriseServer.Data;
using SolidCP.UniversalInstaller;
using SolidCP.UniversalInstaller.Core;
using SolidCP.Providers;

namespace SolidCP.Setup
{
	public class EnterpriseServer : BaseSetup
	{
		public bool Install(string args, string minimalInstallerVersion)
		{
			ParseArgs(args);
			return InstallBase(minimalInstallerVersion);
		}

		bool InstallBase(string minimalInstallerVersion)
		{
			var minVersion = Version.Parse(minimalInstallerVersion);
			if (Settings.Installer.Version >= minVersion)
			{
				var settings = Settings.EnterpriseServer;
				return UI.Current.Wizard
					.Introduction()
					.CheckPrerequisites()
					.LicenseAgreement()
					.InstallFolder(settings)
					.Web(settings)
					.InsecureHttpWarning(settings)
					.Certificate(settings)
					.UserAccount(settings)
					.Database()
					.RunWithProgress("Install Enterprise Server",
						Installer.Current.InstallEnterpriseServer)
					.Finish()
					.Show();
			}
			else
			{
				UI.Current.ShowWarning("You need to upgrade the Installer to install this component.");
				return false;
			}
		}


		public override bool Setup(string args)
		{
			ParseArgs(args);
			var settings = Settings.EnterpriseServer;
			return UI.Current.Wizard
				.Introduction()
				.Web(settings)
				.InsecureHttpWarning(settings)
				.Certificate(settings)
				.UserAccount(settings)
				.Database()
				.RunWithProgress("Setup Enterprise Server",
					() => Installer.Current.ConfigureEnterpriseServer())
				.Finish()
				.Show();
		}

		public override bool Update(string args)
		{
			ParseArgs(args);
			var settings = Settings.EnterpriseServer;
			return UI.Current.Wizard
				.Introduction()
				.Web(settings)
				.InsecureHttpWarning(settings)
				.Certificate(settings)
				.UserAccount(settings)
				.Database()
				.RunWithProgress("Update Enterprise Server",
					Installer.Current.UpdateEnterpriseServer)
				.Finish()
				.Show();
		}
		public override bool Uninstall(string args)
		{
			ParseArgs(args);
			return UI.Current.Wizard
				.Introduction()
				.ConfirmUninstall()
				.RunWithProgress("Uninstall Enterprise Server",
					Installer.Current.RemoveEnterpriseServer)
				.Finish()
				.Show();
		}

	}
}
