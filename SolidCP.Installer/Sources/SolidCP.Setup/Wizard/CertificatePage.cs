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
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using SolidCP.Setup.Web;
using SolidCP.Providers.OS;

namespace SolidCP.Setup
{
	public partial class CertificatePage : BannerWizardPage
	{
		public CertificatePage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();

			string component = SetupVariables.ComponentFullName;
			Description = string.Format("Specify {0} Let's Encrypt settings.", component);

			AllowMoveBack = true;
			AllowMoveNext = true;
			AllowCancel = true;

			// init fields
			txtLetsEncryptEmail.Text = SetupVariables.LetsEncryptEmail ?? "";
			txtCertFileFile.Text = SetupVariables.CertificateFile ?? "";
			txtCertFilePassword.Text = SetupVariables.CertificatePassword ?? "";
			txtStoreLocation.Text = SetupVariables.CertificateStoreLocation ?? "";
			txtStoreName.Text = SetupVariables.CertificateStore ?? "";
			txtStoreFindType.Text = SetupVariables.CertificateFindType ?? "";
			txtStoreFindValue.Text = SetupVariables.CertificateFindValue ?? "";
			manualCert.Checked = false;
			tabControl.Selected += SetAllowedMoveNext;
			manualCert.CheckedChanged += SetAllowedMoveNext;

			txtStoreLocation.Items.Clear();
			txtStoreLocation.Items.Add(StoreLocation.LocalMachine.ToString());
			txtStoreLocation.Items.Add(StoreLocation.CurrentUser.ToString());


			txtStoreName.Items.Clear();
			txtStoreName.Items.Add(StoreName.Root.ToString());
			txtStoreName.Items.Add(StoreName.My.ToString());
			txtStoreName.Items.Add(StoreName.TrustedPeople.ToString());

			txtStoreFindType.Items.Clear();
			txtStoreFindType.Items.Add(X509FindType.FindBySubjectName.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindByThumbprint.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindBySubjectDistinguishedName.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindBySubjectKeyIdentifier.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindBySerialNumber.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindByIssuerName.ToString());
			txtStoreFindType.Items.Add(X509FindType.FindByIssuerDistinguishedName.ToString());

			txtCertFileFile.Enabled = txtCertFilePassword.Enabled = btnOpenCertFile.Enabled = !OSInfo.IsWindows;

			if (OSInfo.IsWindows) tabControl.Controls.Remove(tabPageCertFile);

			Update();
		}

		private void SetAllowedMoveNext(object sender, EventArgs args)
		{
			AllowMoveNext = !IsHttps || (tabControl.SelectedTab != tabPageManual) || manualCert.Checked;
		}

		bool iis7 => SetupVariables.IISVersion.Major >= 7;

		bool IsHttps => (iis7 || !OSInfo.IsWindows) && Utils.IsHttps(SetupVariables.WebSiteIP, SetupVariables.WebSiteDomain);

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

			if (!Regex.IsMatch(email, "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,6})$"))
			{
				ShowWarning(String.Format("'{0}' is not a valid email address.", email));
				return false;
			}
			return true;
		}

		private bool CheckCertStore()
		{
			X509FindType findType;
			StoreLocation location;
			StoreName name;
			if (!Enum.TryParse<StoreLocation>(txtStoreLocation.Text, out location)) {
				ShowWarning("The entered Store Location is invalid.");
				return false;
			}
			if (!Enum.TryParse<StoreName>(txtStoreName.Text, out name)) {
				ShowWarning("The entered Store Name is invalid.");
				return false;
			}
			if (!Enum.TryParse<X509FindType>(txtStoreFindType.Text, out findType))
			{
				ShowWarning("The entered Find Type is invalid.");
				return false;
			}
			return true;
		}

		private bool CheckCertFile()
		{
			var file = txtCertFileFile.Text;
			if (!File.Exists(file))
			{
				ShowWarning("The entered Certificate File could not be found.");
				return false;
			}
			try
			{
				var cert2 = new X509Certificate2(file, txtCertFilePassword.Text);
			} catch
			{
				ShowWarning("The entered password is invalid.");
				return false;
			}
			return true;
		}
		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			SetupVariables.LetsEncryptEmail = null;
			SetupVariables.CertificateFile = null;
			SetupVariables.CertificatePassword = null;
			SetupVariables.CertificateStoreLocation = null;
			SetupVariables.CertificateStore = null;
			SetupVariables.CertificateFindType = null;
			SetupVariables.CertificateFindValue = null;

			if (IsHttps)
			{

				if (tabControl.SelectedTab == tabPageCertStore)
				{
					if (!CheckCertStore())
					{
						e.Cancel = true;
						return;
					}
					SetupVariables.CertificateStoreLocation = txtStoreLocation.Text;
					SetupVariables.CertificateStore = txtStoreName.Text;
					SetupVariables.CertificateFindType = txtStoreFindType.Text;
					SetupVariables.CertificateFindValue = txtStoreFindValue.Text;
				} else if (tabControl.SelectedTab == tabPageCertFile)
				{
					if (!CheckCertFile())
					{
						e.Cancel = true;
						return;
					}
					SetupVariables.CertificateFile = txtCertFileFile.Text;
					SetupVariables.CertificatePassword = txtCertFilePassword.Text;
				} else if (tabControl.SelectedTab == tabPageLetsEncrypt)
				{
					if (!CheckEmail())
					{
						e.Cancel = true;
						return;
					}
					SetupVariables.LetsEncryptEmail = txtLetsEncryptEmail.Text;
				} else if (tabControl.SelectedTab == tabPageManual)
				{
					if (!manualCert.Checked)
					{
						e.Cancel = true;
						return;
					}
				}
			}

			base.OnBeforeMoveNext(e);
		}

		private void btnOpenCertFile_Click(object sender, EventArgs e)
		{
			if (openCertFileDialog.ShowDialog() == DialogResult.OK)
			{
				txtCertFileFile.Text = openCertFileDialog.FileName;
			}
		}
	}
}
