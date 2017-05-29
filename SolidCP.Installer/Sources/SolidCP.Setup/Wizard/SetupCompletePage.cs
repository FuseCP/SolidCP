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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SolidCP.Setup
{
	public partial class SetupCompletePage : BannerWizardPage
	{
		public SetupCompletePage()
		{
			InitializeComponent();
		}

		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			this.Text = "Completing SolidCP Setup";
			this.Description = "Setup has finished configuration of SolidCP";
			this.AllowMoveBack = false;
			this.AllowCancel = false;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(SetupVariables.SetupXml))
				Wizard.GoNext();
		}
		
		private void OnViewLogLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Log.ShowLogFile();
		}

		private void OnLoginClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string ip = SetupVariables.WebSiteIP;
			string domain = SetupVariables.WebSiteDomain;
			string port = SetupVariables.WebSitePort;
			string url = GetApplicationUrl(ip, domain, port);
			Process.Start(url);
		}

		private string GetApplicationUrl(string ip, string domain, string port)
		{
			string url = ip;
			if (String.IsNullOrEmpty(domain))
			{
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
			}
			else
			{
				url = domain;
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
			}
			return "http://"+url;
		}
	}
}
