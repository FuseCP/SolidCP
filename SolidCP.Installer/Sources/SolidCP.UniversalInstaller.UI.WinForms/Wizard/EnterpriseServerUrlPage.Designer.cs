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

namespace SolidCP.UniversalInstaller.WinForms
{
	partial class EnterpriseServerUrlPage
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
			this.lblURL = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.lblIntro = new System.Windows.Forms.Label();
			this.chkBoxEmbed = new System.Windows.Forms.CheckBox();
			this.lblEnterpriseServerPath = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.chooseFolderButton = new System.Windows.Forms.Button();
			this.chkExpose = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// lblURL
			// 
			this.lblURL.Location = new System.Drawing.Point(3, 64);
			this.lblURL.Name = "lblURL";
			this.lblURL.Size = new System.Drawing.Size(139, 23);
			this.lblURL.TabIndex = 13;
			this.lblURL.Text = "Enterprise Server URL:";
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(148, 61);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(306, 20);
			this.txtURL.TabIndex = 14;
			// 
			// lblIntro
			// 
			this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIntro.Location = new System.Drawing.Point(0, 0);
			this.lblIntro.Name = "lblIntro";
			this.lblIntro.Size = new System.Drawing.Size(457, 58);
			this.lblIntro.TabIndex = 12;
			this.lblIntro.Text = "Please, specify the URL which will be used to access the Enterprise Server from t" +
    "he Portal. Click Next to continue.";
			// 
			// chkBoxEmbed
			// 
			this.chkBoxEmbed.AutoSize = true;
			this.chkBoxEmbed.Location = new System.Drawing.Point(6, 108);
			this.chkBoxEmbed.Name = "chkBoxEmbed";
			this.chkBoxEmbed.Size = new System.Drawing.Size(235, 17);
			this.chkBoxEmbed.TabIndex = 15;
			this.chkBoxEmbed.Text = "Embed Enterprise Server into Portal website:";
			this.chkBoxEmbed.UseVisualStyleBackColor = true;
			this.chkBoxEmbed.CheckedChanged += new System.EventHandler(this.chkBoxEmbed_CheckedChanged);
			// 
			// lblEnterpriseServerPath
			// 
			this.lblEnterpriseServerPath.AutoSize = true;
			this.lblEnterpriseServerPath.Location = new System.Drawing.Point(3, 143);
			this.lblEnterpriseServerPath.Name = "lblEnterpriseServerPath";
			this.lblEnterpriseServerPath.Size = new System.Drawing.Size(128, 13);
			this.lblEnterpriseServerPath.TabIndex = 16;
			this.lblEnterpriseServerPath.Text = "Path to Enterprise Server:";
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(148, 140);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(274, 20);
			this.txtPath.TabIndex = 17;
			// 
			// chooseFolderButton
			// 
			this.chooseFolderButton.Image = global::SolidCP.UniversalInstaller.Properties.Resources.Icons_MenuFileOpenIcon;
			this.chooseFolderButton.Location = new System.Drawing.Point(424, 138);
			this.chooseFolderButton.Name = "chooseFolderButton";
			this.chooseFolderButton.Size = new System.Drawing.Size(30, 23);
			this.chooseFolderButton.TabIndex = 18;
			this.chooseFolderButton.UseVisualStyleBackColor = true;
			this.chooseFolderButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
			// 
			// chkExpose
			// 
			this.chkExpose.AutoSize = true;
			this.chkExpose.Location = new System.Drawing.Point(6, 179);
			this.chkExpose.Name = "chkExpose";
			this.chkExpose.Size = new System.Drawing.Size(210, 17);
			this.chkExpose.TabIndex = 19;
			this.chkExpose.Text = "Expose Enterprise Server Webservices";
			this.chkExpose.UseVisualStyleBackColor = true;
			// 
			// UrlPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkExpose);
			this.Controls.Add(this.chooseFolderButton);
			this.Controls.Add(this.txtPath);
			this.Controls.Add(this.lblEnterpriseServerPath);
			this.Controls.Add(this.chkBoxEmbed);
			this.Controls.Add(this.lblURL);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.lblIntro);
			this.Name = "UrlPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblURL;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.CheckBox chkBoxEmbed;
        private System.Windows.Forms.Label lblEnterpriseServerPath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.CheckBox chkExpose;
    }
}
