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
using System.Xml;
using SolidCP.Setup.Common;

namespace SolidCP.Setup.Actions
{
	public class SetWebDavPortalWebSettingsAction : Action, IPrepareDefaultsAction
	{
		public const string LogStartMessage = "Retrieving default IP address of the component...";

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.WebSitePort))
				vars.WebSitePort = Global.WebDavPortal.DefaultPort;
			//
			if (String.IsNullOrEmpty(vars.UserAccount))
				vars.UserAccount = Global.WebDavPortal.ServiceAccount;

			// By default we use public ip for the component
			if (String.IsNullOrEmpty(vars.WebSiteIP))
			{
				var serverIPs = WebUtils.GetIPv4Addresses();
				//
				if (serverIPs != null && serverIPs.Length > 0)
				{
					vars.WebSiteIP = serverIPs[0];
				}
				else
				{
					vars.WebSiteIP = Global.LoopbackIPv4;
				}
			}
		}
	}

	public class WebDavPortalActionManager : BaseActionManager
	{
		public static readonly List<Action> InstallScenario = new List<Action>
		{
			new SetCommonDistributiveParamsAction(),
			new SetWebDavPortalWebSettingsAction(),
			new EnsureServiceAccntSecured(),
			new CopyFilesAction(),
			new CopyWebConfigAction(),
			new CreateWindowsAccountAction(),
			new ConfigureAspNetTempFolderPermissionsAction(),
			new SetNtfsPermissionsAction(),
			new CreateWebApplicationPoolAction(),
			new CreateWebSiteAction(),
			new SwitchAppPoolAspNetVersion(),
			new UpdateEnterpriseServerUrlAction(),
            new GenerateSessionValidationKeyAction(),
			new SaveComponentConfigSettingsAction()
        };

        public WebDavPortalActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(WebDavPortalActionManager_Initialize);
		}

		void WebDavPortalActionManager_Initialize(object sender, EventArgs e)
		{
			//
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install: // Install
					LoadInstallationScenario();
					break;
			}
		}

		private void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}
}
