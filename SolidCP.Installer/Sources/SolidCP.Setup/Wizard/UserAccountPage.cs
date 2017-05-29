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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


using SolidCP.Setup.Web;
using SolidCP.Setup.Windows;

namespace SolidCP.Setup
{
	public partial class UserAccountPage : BannerWizardPage
	{
		public UserAccountPage()
		{
			InitializeComponent();
		}

		public string WarningText
		{
			get { return lblWarning.Text; }
			set { lblWarning.Text = value; }
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Security Settings";
			
			string component = Wizard.SetupVariables.ComponentFullName;
			this.Description = string.Format("Specify {0} security settings.", component);
			
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			if (!string.IsNullOrEmpty(Wizard.SetupVariables.UserAccount))
			{
				txtUserName.Text = Wizard.SetupVariables.UserAccount;
				txtDomain.Text = Wizard.SetupVariables.UserDomain;
				txtPassword.Text = Wizard.SetupVariables.UserPassword;
				txtConfirmPassword.Text = Wizard.SetupVariables.UserPassword;
				chkUseActiveDirectory.Checked = !string.IsNullOrEmpty(Wizard.SetupVariables.UserDomain);
			}
			else
			{

				//creating user account
				string userName = component.Replace(" ", string.Empty);
				userName = userName.Replace("SolidCP", "WP");

				txtUserName.Text = userName;
				txtDomain.Text = "mydomain.com";

				string password = Guid.NewGuid().ToString("P");
				this.txtPassword.Text = password;
				this.txtConfirmPassword.Text = password;

				if (Environment.UserDomainName != Environment.MachineName)
				{
					chkUseActiveDirectory.Checked = true;
					string domainName = SecurityUtils.GetFullDomainName(Environment.UserDomainName);
					if (!string.IsNullOrEmpty(domainName))
					{
						txtDomain.Text = domainName;
					}
				}
				else
				{
					chkUseActiveDirectory.Checked = false;
				}
			}
			UpdateControls();
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
				Wizard.GoNext();
		}

		private bool CheckSettings()
		{
			string name = txtUserName.Text;
			string password = txtPassword.Text;
			string confirm = txtConfirmPassword.Text;
			string domain = txtDomain.Text;

			if (chkUseActiveDirectory.Checked)
			{
				if (domain.Trim().Length == 0)
				{
					ShowWarning("Please enter domain name");
					return false;
				}

				string users = SecurityUtils.GetDomainUsersContainer(domain);
				if (string.IsNullOrEmpty(users))
				{
					ShowWarning("Domain not found or access denied.");
					return false;
				}

				if (!SecurityUtils.ADObjectExists(users))
				{
					ShowWarning("Domain not found or access denied.");
					return false;
				}
			}

			if (name.Trim().Length == 0)
			{
				ShowWarning("Please enter user name");
				return false;
			}

			if (password.Trim().Length == 0)
			{
				ShowWarning("Please enter password");
				return false;
			}

			if (password != confirm)
			{
				ShowWarning("The password was not correctly confirmed. Please ensure that the password and confirmation match exactly.");
				return false;
			}

			return true;
		}

		private bool ProcessSettings()
		{
			if (!CheckSettings())
			{
				return false;
			}

			if (!CheckUserAccount())
			{
				return false;
			}
			return true;
		}

		private bool CheckUserAccount()
		{
			string userName = txtUserName.Text;
			string password = txtPassword.Text;
			string domain = (chkUseActiveDirectory.Checked ? txtDomain.Text : null);
			
			if (SecurityUtils.UserExists(domain, userName))
			{
				ShowWarning(string.Format("{0} user account already exists.", userName));
				return false;
			}
			
			bool created = false;
			try
			{
				// create account
				Log.WriteStart(string.Format("Creating temp user account \"{0}\"", userName));
				SystemUserItem user = new SystemUserItem();
				user.Name = userName;
				user.FullName = userName;
				user.Description = string.Empty;
				user.MemberOf = null;
				user.Password = password;
				user.PasswordCantChange = true;
				user.PasswordNeverExpires = true;
				user.AccountDisabled = false;
				user.System = true;
				user.Domain = domain;
				SecurityUtils.CreateUser(user);
				//update log
				Log.WriteEnd("Created temp local user account");
				created = true;
			}
			catch (Exception ex)
			{
				System.Runtime.InteropServices.COMException e = ex.InnerException as System.Runtime.InteropServices.COMException;
				Log.WriteError("Create temp local user account error", ex);
				string errorMessage = "Unable to create Windows user account"; 
				if (e != null )
				{
					string errorCode = string.Format("{0:x}", e.ErrorCode);
					switch (errorCode)
					{
						case "8007089a":
							errorMessage = "Invalid username";
							break;
						case "800708c5":
							errorMessage = "The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements.";
							break;
						case "800708b0":
							errorMessage = "The account already exists.";
							break;
					}
				}
				ShowWarning(errorMessage);
				return false;
			}

			if (created)
			{
				Log.WriteStart(string.Format("Deleting temp local user account \"{0}\"", userName));
				try
				{
					SecurityUtils.DeleteUser(domain, userName);
				}
				catch (Exception ex)
				{
					Log.WriteError("Delete temp local user account error", ex);
				}
				Log.WriteEnd("Deleted temp local user account");
			}
			return true;
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			string userName = txtUserName.Text;
			string password = txtPassword.Text;
			string domain = (chkUseActiveDirectory.Checked ? txtDomain.Text : null);

			Log.WriteInfo(string.Format("Domain: {0}", domain));
			Log.WriteInfo(string.Format("User name: {0}", userName));


			if (!ProcessSettings())
			{
				e.Cancel = true;
				return;
			}
			Wizard.SetupVariables.UserAccount = userName;
			Wizard.SetupVariables.UserPassword = password;
			Wizard.SetupVariables.UserDomain = domain;

			base.OnBeforeMoveNext(e);
		}

		private void OnActiveDirectoryChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void UpdateControls()
		{
			bool useAD = chkUseActiveDirectory.Checked;
			lblDomain.Visible = useAD;
			txtDomain.Visible = useAD;
			Update();
		}
	}
}
