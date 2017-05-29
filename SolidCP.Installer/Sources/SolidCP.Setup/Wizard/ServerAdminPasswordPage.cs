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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SolidCP.Setup
{
	public partial class ServerAdminPasswordPage : BannerWizardPage
	{
		public ServerAdminPasswordPage()
		{
			InitializeComponent();
		}

		public string NoteText
		{
			get { return lblNote.Text; }
			set { lblNote.Text = value; }
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Set Administrator Password";
			this.Description = "Specify a new password for the serveradmin account.";
			
			chkChangePassword.Checked = true;
			txtPassword.Text = SetupVariables.ServerAdminPassword;
			txtConfirmPassword.Text = SetupVariables.ServerAdminPassword;

			if (SetupVariables.SetupAction == SetupActions.Setup)
			{
				chkChangePassword.Visible = true;
			}
			else
			{
				SetupVariables.UpdateServerAdminPassword = true;
				chkChangePassword.Visible = false;
			}

			if (!string.IsNullOrEmpty(SetupVariables.ServerAdminPassword))
			{
				txtPassword.Text = SetupVariables.ServerAdminPassword;
				txtConfirmPassword.Text = SetupVariables.ServerAdminPassword;
			}

		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(SetupVariables.SetupXml))
				Wizard.GoNext();
		}


		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);

			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				if (!chkChangePassword.Checked)
				{
					//if we don't want to change password during setup 
					SetupVariables.UpdateServerAdminPassword = false;
					e.Cancel = false;
					return;
				}

				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				// Use the same password for peer account
				SetupVariables.ServerAdminPassword = SetupVariables.PeerAdminPassword = txtPassword.Text;
				//
				SetupVariables.UpdateServerAdminPassword = true;
			}
			catch
			{
				this.AllowMoveNext = false;
				ShowError("Unable to reset password.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private bool CheckFields()
		{
			string password = txtPassword.Text;

			if (string.IsNullOrEmpty(password) || password.Trim().Length == 0)
			{
				ShowWarning("Please enter password");
				return false;
			}

			if (password != txtConfirmPassword.Text)
			{
				ShowWarning("The password was not correctly confirmed. Please ensure that the password and confirmation match exactly.");
				return false;
			}
			return true;
		}

		private void OnChangePasswordChecked(object sender, EventArgs e)
		{
			bool enabled = chkChangePassword.Checked;
			txtPassword.Enabled = enabled;
			txtConfirmPassword.Enabled = enabled;
		}
	}
}
