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

namespace SolidCP.Setup
{
	partial class InstallFolderPage
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpFolder = new System.Windows.Forms.GroupBox();
			this.txtFolder = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lblIntro = new System.Windows.Forms.Label();
			this.lblSpaceRequired = new System.Windows.Forms.Label();
			this.lblSpaceAvailable = new System.Windows.Forms.Label();
			this.lblSpaceAvailableValue = new System.Windows.Forms.Label();
			this.lblSpaceRequiredValue = new System.Windows.Forms.Label();
			this.grpFolder.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpFolder
			// 
			this.grpFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpFolder.Controls.Add(this.txtFolder);
			this.grpFolder.Controls.Add(this.btnBrowse);
			this.grpFolder.Location = new System.Drawing.Point(0, 82);
			this.grpFolder.Name = "grpFolder";
			this.grpFolder.Size = new System.Drawing.Size(454, 64);
			this.grpFolder.TabIndex = 4;
			this.grpFolder.TabStop = false;
			this.grpFolder.Text = "Destination Folder";
			// 
			// txtFolder
			// 
			this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFolder.Location = new System.Drawing.Point(24, 24);
			this.txtFolder.Name = "txtFolder";
			this.txtFolder.Size = new System.Drawing.Size(332, 20);
			this.txtFolder.TabIndex = 1;
			this.txtFolder.TextChanged += new System.EventHandler(this.OnFolderChanged);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(362, 22);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 0;
			this.btnBrowse.Text = "B&rowse...";
			this.btnBrowse.Click += new System.EventHandler(this.OnBrowseClick);
			// 
			// lblIntro
			// 
			this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIntro.Location = new System.Drawing.Point(0, 0);
			this.lblIntro.Name = "lblIntro";
			this.lblIntro.Size = new System.Drawing.Size(457, 56);
			this.lblIntro.TabIndex = 5;
			this.lblIntro.Text = "Setup will install SolidCP in the following folder. To install in a different" +
				" folder, click Browse and select another folder. Click Next to continue.";
			// 
			// lblSpaceRequired
			// 
			this.lblSpaceRequired.Location = new System.Drawing.Point(3, 164);
			this.lblSpaceRequired.Name = "lblSpaceRequired";
			this.lblSpaceRequired.Size = new System.Drawing.Size(144, 16);
			this.lblSpaceRequired.TabIndex = 6;
			this.lblSpaceRequired.Text = "Total disk space required:";
			// 
			// lblSpaceAvailable
			// 
			this.lblSpaceAvailable.Location = new System.Drawing.Point(3, 183);
			this.lblSpaceAvailable.Name = "lblSpaceAvailable";
			this.lblSpaceAvailable.Size = new System.Drawing.Size(144, 16);
			this.lblSpaceAvailable.TabIndex = 7;
			this.lblSpaceAvailable.Text = "Space available on disk:";
			// 
			// lblSpaceAvailableValue
			// 
			this.lblSpaceAvailableValue.Location = new System.Drawing.Point(153, 183);
			this.lblSpaceAvailableValue.Name = "lblSpaceAvailableValue";
			this.lblSpaceAvailableValue.Size = new System.Drawing.Size(134, 16);
			this.lblSpaceAvailableValue.TabIndex = 9;
			this.lblSpaceAvailableValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblSpaceRequiredValue
			// 
			this.lblSpaceRequiredValue.Location = new System.Drawing.Point(153, 164);
			this.lblSpaceRequiredValue.Name = "lblSpaceRequiredValue";
			this.lblSpaceRequiredValue.Size = new System.Drawing.Size(134, 16);
			this.lblSpaceRequiredValue.TabIndex = 8;
			this.lblSpaceRequiredValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// InstallFolderPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblSpaceAvailableValue);
			this.Controls.Add(this.lblSpaceRequiredValue);
			this.Controls.Add(this.lblSpaceAvailable);
			this.Controls.Add(this.lblSpaceRequired);
			this.Controls.Add(this.grpFolder);
			this.Controls.Add(this.lblIntro);
			this.Name = "InstallFolderPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpFolder.ResumeLayout(false);
			this.grpFolder.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpFolder;
		private System.Windows.Forms.TextBox txtFolder;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label lblIntro;
		private System.Windows.Forms.Label lblSpaceRequired;
		private System.Windows.Forms.Label lblSpaceAvailable;
		private System.Windows.Forms.Label lblSpaceAvailableValue;
		private System.Windows.Forms.Label lblSpaceRequiredValue;



	}
}
