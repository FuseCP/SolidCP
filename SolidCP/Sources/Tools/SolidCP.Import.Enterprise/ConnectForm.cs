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
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SolidCP.EnterpriseServer;

namespace SolidCP.Import.Enterprise
{
	public partial class ConnectForm : Form
	{
		private Thread connectThread;

		public ConnectForm()
		{
			InitializeComponent();
			if (DesignMode)
			{
				return;
			}
			InitializeForm();
			UpdateFormState();
		}

		private void InitializeForm()
		{
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage1);
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage2);
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage3);
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage4);
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage5);
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage6);
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage7);
			animatedIcon.Images.Add(SolidCP.Import.Enterprise.Properties.Resources.ProgressImage8);
			animatedIcon.LastFrame = 8;
		}

		private void OnConnectClick(object sender, EventArgs e)
		{
			DisableForm();
			animatedIcon.StartAnimation();
			ThreadStart threadDelegate = new ThreadStart(Connect);
			connectThread = new Thread(threadDelegate);
			connectThread.Start();
		}

		

		public string Username
		{
			get { return txtUserName.Text; }
		}
		
		public string Password
		{
			get { return txtPassword.Text; }
		}

		private void Connect()
		{
			int status = -1;
			try
			{
				status = UserController.AuthenticateUser(txtUserName.Text, txtPassword.Text, string.Empty);
				if (status == 0)
				{
					UserInfo userInfo = UserController.GetUser(txtUserName.Text);
					SecurityContext.SetThreadPrincipal(userInfo);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Authentication error", ex);
				status = -1;
			}
			finally
			{
				animatedIcon.StopAnimation();
			}

			string errorMessage = "Check configuration settings.";
			if (status != 0)
			{
				switch (status)
				{
					case BusinessErrorCodes.ERROR_USER_WRONG_USERNAME:
					case BusinessErrorCodes.ERROR_USER_WRONG_PASSWORD:
						errorMessage = "Wrong username or password.";
						break;
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_CANCELLED:
						errorMessage = "Account cancelled.";
						break;
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_PENDING:
						errorMessage = "Account pending.";
						break;
					case -1:
						errorMessage = "Authentication error.";
						break;
				}
				MessageBox.Show(this,
					string.Format("Cannot connect to the Enterprise Server.\n{0}", errorMessage),
					Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				EnableForm();
				return;
			}
			else
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void DisableForm()
		{
			foreach(Control ctrl in Controls)
			{
				ctrl.Enabled = false;
			}
			btnCancel.Enabled = true;
		}

		private void EnableForm()
		{
			foreach (Control ctrl in Controls)
			{
				ctrl.Enabled = true;
			}
		}

		private void UpdateFormState()
		{
			btnConnect.Enabled = (txtUserName.Text.Length > 0 );
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			if (connectThread != null)
			{
				connectThread.Abort();
			}
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void OnServerNameTextChanged(object sender, EventArgs e)
		{
			UpdateFormState();
		}

		private void OnUserNameTextChanged(object sender, EventArgs e)
		{
			UpdateFormState();
		}
	}
}
