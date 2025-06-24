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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolidCP.UniversalInstaller.WinForms
{
	public partial class FinishPage : BannerWizardPage
	{
		public FinishPage()
		{
			InitializeComponent();
		}

		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			if (Installer.Current.HasError)
			{
				this.Text = "Setup failed";
				txtLog.Text = Installer.Current.Error.SourceException.ToString();
				//ParentForm.DialogResult = DialogResult.Abort;
			} else
			{
				this.Text = "Setup complete";
				this.txtLog.Text = string.Join(Environment.NewLine,
					new[] { "The Installer has:" }
					.Concat(Installer.Current.InstallLogs.SelectMany(log =>
					{
						bool first = true;
						return log.Split('\n')
							.Where(line => !string.IsNullOrWhiteSpace(line))
							.Select(line =>
							{
								line = line.Trim();
								line = first ? $"- {line}" : $"  {line}";
								first = false;
								return line;
							});
					})));
				//ParentForm.DialogResult = DialogResult.OK;
			}
			this.Description = "Click Finish to exit the wizard.";
			this.AllowMoveBack = false;
			this.AllowCancel = false;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (Installer.Current.Settings.Installer.IsUnattended)
				Wizard.GoNext();
		}
		
		private void OnLinkClicked(object sender, LinkClickedEventArgs e)
		{
			var startInfo = new ProcessStartInfo(e.LinkText);
			startInfo.UseShellExecute = true;
			startInfo.Verb = "open";
			Process.Start(startInfo);
		}

		private void OnViewLogLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Log.ShowLogFile();
		}
	}
}
