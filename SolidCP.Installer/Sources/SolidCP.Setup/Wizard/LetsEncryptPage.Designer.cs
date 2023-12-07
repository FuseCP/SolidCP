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
	partial class LetsEncryptPage
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
			this.grpWebSiteSettings = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.manualCert = new System.Windows.Forms.CheckBox();
			this.lblWebSiteDomain = new System.Windows.Forms.Label();
			this.txtLetsEncryptEmail = new System.Windows.Forms.TextBox();
			this.grpWebSiteSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpWebSiteSettings
			// 
			this.grpWebSiteSettings.Controls.Add(this.label1);
			this.grpWebSiteSettings.Controls.Add(this.manualCert);
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteDomain);
			this.grpWebSiteSettings.Controls.Add(this.txtLetsEncryptEmail);
			this.grpWebSiteSettings.Location = new System.Drawing.Point(30, 27);
			this.grpWebSiteSettings.Name = "grpWebSiteSettings";
			this.grpWebSiteSettings.Size = new System.Drawing.Size(396, 141);
			this.grpWebSiteSettings.TabIndex = 0;
			this.grpWebSiteSettings.TabStop = false;
			this.grpWebSiteSettings.Text = "Let\'s Encrypt Settings";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(30, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(261, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Provide a Certificate for your server with Let\'s Encrypt:";
			// 
			// manualCert
			// 
			this.manualCert.AutoSize = true;
			this.manualCert.Location = new System.Drawing.Point(33, 109);
			this.manualCert.Name = "manualCert";
			this.manualCert.Size = new System.Drawing.Size(193, 17);
			this.manualCert.TabIndex = 6;
			this.manualCert.Text = "I\'ll configure the certificate manually";
			this.manualCert.UseVisualStyleBackColor = true;
			// 
			// lblWebSiteDomain
			// 
			this.lblWebSiteDomain.Location = new System.Drawing.Point(30, 64);
			this.lblWebSiteDomain.Name = "lblWebSiteDomain";
			this.lblWebSiteDomain.Size = new System.Drawing.Size(84, 16);
			this.lblWebSiteDomain.TabIndex = 4;
			this.lblWebSiteDomain.Text = "Email address:";
			// 
			// txtLetsEncryptEmail
			// 
			this.txtLetsEncryptEmail.Location = new System.Drawing.Point(33, 83);
			this.txtLetsEncryptEmail.Name = "txtLetsEncryptEmail";
			this.txtLetsEncryptEmail.Size = new System.Drawing.Size(327, 20);
			this.txtLetsEncryptEmail.TabIndex = 5;
			// 
			// LetsEncryptPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpWebSiteSettings);
			this.Name = "LetsEncryptPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpWebSiteSettings.ResumeLayout(false);
			this.grpWebSiteSettings.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpWebSiteSettings;
		private System.Windows.Forms.Label lblWebSiteDomain;
		private System.Windows.Forms.TextBox txtLetsEncryptEmail;
		private System.Windows.Forms.CheckBox manualCert;
		private System.Windows.Forms.Label label1;
	}
}
