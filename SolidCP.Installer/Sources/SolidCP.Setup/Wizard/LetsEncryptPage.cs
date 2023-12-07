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
using System.Management;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using SolidCP.Setup.Web;

namespace SolidCP.Setup
{
	public partial class LetsEncryptPage : BannerWizardPage
	{
		public LetsEncryptPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();

			string component = SetupVariables.ComponentFullName;
			this.Description = string.Format("Specify {0} Let's Encrypt settings.", component);

			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
			manualCert.CheckedChanged += (sender, args) =>
			{
				txtLetsEncryptEmail.Enabled = !manualCert.Checked;
			};

			// init fields
			this.txtLetsEncryptEmail.Text = SetupVariables.LetsEncryptEmail;
			Update();
		}

		bool IsHttps => Utils.IsHttps(SetupVariables.WebSiteIP, SetupVariables.WebSiteDomain);

		public override bool Hidden => !IsHttps;
		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if ((!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) || !IsHttps) && AllowMoveNext)
				Wizard.GoNext();
		}

		private bool CheckEmail()
		{
			string email = txtLetsEncryptEmail.Text?.Trim() ?? "";

			if (!Regex.IsMatch(email, "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$"))
			{
				ShowWarning(String.Format("'{0}' is not a valid email address.", email));
				return false;
			}
			return true;
		}
		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			if (IsHttps && !manualCert.Checked && !CheckEmail())
			{
				e.Cancel = true;
				return;
			}

			if (IsHttps)
			{
				if (manualCert.Checked)
				{
					SetupVariables.LetsEncryptEmail = "";
				}
				else
				{
					SetupVariables.LetsEncryptEmail = this.txtLetsEncryptEmail.Text;
				}
			}
			else SetupVariables.LetsEncryptEmail = "";

			base.OnBeforeMoveNext(e);
		}
	}
}
