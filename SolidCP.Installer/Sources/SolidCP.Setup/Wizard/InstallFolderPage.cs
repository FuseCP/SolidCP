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
	public partial class InstallFolderPage : BannerWizardPage
	{
		public InstallFolderPage()
		{
			InitializeComponent();
		}

		private long spaceRequired = 0;

		protected override void InitializePageInternal()
		{
			this.Text = "Choose Install Location";
			string component = Wizard.SetupVariables.ComponentFullName;
			this.Description = string.Format("Choose the folder in which to install {0}.", component);
			this.lblIntro.Text = string.Format("Setup will install {0} in the following folder. To install in a different folder, click Browse and select another folder. Click Next to continue.", component);
			this.AllowMoveBack = false;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
			
			UpdateSpaceRequiredInformation();

			if ( !string.IsNullOrEmpty(Wizard.SetupVariables.InstallationFolder))
			{
				txtFolder.Text = Wizard.SetupVariables.InstallationFolder;
			}
		}

		private void UpdateSpaceRequiredInformation()
		{
			try
			{
				spaceRequired = 0;
				lblSpaceRequiredValue.Text = string.Empty;
				if (!string.IsNullOrEmpty(Wizard.SetupVariables.InstallerFolder))
				{
					spaceRequired = FileUtils.CalculateFolderSize(Wizard.SetupVariables.InstallerFolder);
					lblSpaceRequiredValue.Text = FileUtils.SizeToMB(spaceRequired);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("I/O error:", ex);
			}
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			////unattended setup
			if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
				Wizard.GoNext();
		}

		private void OnBrowseClick(object sender, System.EventArgs e)
		{
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.RootFolder = Environment.SpecialFolder.MyComputer;
			dialog.SelectedPath = txtFolder.Text;

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				txtFolder.Text = dialog.SelectedPath;
			
			}
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				string installFolder = this.txtFolder.Text;
				Log.WriteInfo(string.Format("Destination folder \"{0}\" selected", installFolder));

				if (!Directory.Exists(installFolder))
				{
					Log.WriteStart(string.Format("Creating a new folder \"{0}\"", installFolder));
					Directory.CreateDirectory(installFolder);
					Log.WriteStart("Created a new folder");
				}
				Wizard.SetupVariables.InstallationFolder = installFolder;

				base.OnBeforeMoveNext(e);
			}
			catch (Exception ex)
			{
				Log.WriteError("Folder create error", ex);
				e.Cancel = true;
				ShowError("Unable to create the folder.");
			}
		}

		private void OnFolderChanged(object sender, EventArgs e)
		{
			UpdateFreeSpaceInformation();
			
		}

		private void UpdateFreeSpaceInformation()
		{
			lblSpaceAvailableValue.Text = string.Empty;
			this.AllowMoveNext = false;
			try
			{
				if ( string.IsNullOrEmpty(txtFolder.Text))
					return;
				string path = Path.GetFullPath(txtFolder.Text);
				string drive = Path.GetPathRoot(path);
				ulong freeBytesAvailable, totalBytes, freeBytes;
				if (FileUtils.GetDiskFreeSpaceEx(drive, out freeBytesAvailable, out totalBytes, out freeBytes))
				{
					long freeSpace = Convert.ToInt64(freeBytesAvailable);
					lblSpaceAvailableValue.Text = FileUtils.SizeToMB(freeSpace);
					this.AllowMoveNext = (freeSpace >= spaceRequired);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("I/O error:", ex);
				this.AllowMoveNext = false;
			}
		}
	}
}
