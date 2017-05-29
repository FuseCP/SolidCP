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
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace SolidCP.Setup
{
	public partial class UrlPage : BannerWizardPage
	{
		public UrlPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Enterprise Server URL";
			this.Description = "Enter the Enterprise Server URL";

			if (Wizard.SetupVariables.SetupAction == SetupActions.Setup)
			{
				LoadUrl();
			}
			this.txtURL.Text = Wizard.SetupVariables.EnterpriseServerURL;

			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
				Wizard.GoNext();
		}

		private void LoadUrl()
		{
			try
			{
				Wizard.SetupVariables.EnterpriseServerURL = string.Empty;

				string installFolder = Wizard.SetupVariables.InstallationFolder;
				string path = Path.Combine(installFolder, @"App_Data\SiteSettings.config");

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement urlNode = doc.SelectSingleNode("SiteSettings/EnterpriseServer") as XmlElement;
				if (urlNode == null)
				{
					Log.WriteInfo("EnterpriseServer setting not found");
					return;
				}

				Wizard.SetupVariables.EnterpriseServerURL = urlNode.InnerText;
			}
			catch(Exception ex)
			{
				Log.WriteError("Site settings error", ex);
			}			
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				Wizard.SetupVariables.EnterpriseServerURL = txtURL.Text;
			}
			catch
			{
				this.AllowMoveNext = false;
				ShowError("Unable to set enterprise server URL.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private bool CheckFields()
		{
			string url = txtURL.Text;

			if (url.Trim().Length == 0)
			{
				return true;
			}

			Regex r = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
			Match m = r.Match(url);
			if (!m.Success)
			{
				ShowWarning("Please enter valid URL");
				return false;
			}

			return true;
		}
	}
}
