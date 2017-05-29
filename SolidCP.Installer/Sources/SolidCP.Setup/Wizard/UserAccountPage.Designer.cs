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
	partial class UserAccountPage
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
			this.chkUseActiveDirectory = new System.Windows.Forms.CheckBox();
			this.txtDomain = new System.Windows.Forms.TextBox();
			this.lblDomain = new System.Windows.Forms.Label();
			this.lblWarning = new System.Windows.Forms.Label();
			this.txtConfirmPassword = new System.Windows.Forms.TextBox();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblConfirmPassword = new System.Windows.Forms.Label();
			this.lblUserName = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// chkUseActiveDirectory
			// 
			this.chkUseActiveDirectory.AutoSize = true;
			this.chkUseActiveDirectory.Location = new System.Drawing.Point(62, 41);
			this.chkUseActiveDirectory.Name = "chkUseActiveDirectory";
			this.chkUseActiveDirectory.Size = new System.Drawing.Size(177, 17);
			this.chkUseActiveDirectory.TabIndex = 11;
			this.chkUseActiveDirectory.Text = "Create Active Directory account";
			this.chkUseActiveDirectory.UseVisualStyleBackColor = true;
			this.chkUseActiveDirectory.CheckedChanged += new System.EventHandler(this.OnActiveDirectoryChanged);
			// 
			// txtDomain
			// 
			this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDomain.Location = new System.Drawing.Point(188, 63);
			this.txtDomain.Name = "txtDomain";
			this.txtDomain.Size = new System.Drawing.Size(201, 20);
			this.txtDomain.TabIndex = 13;
			// 
			// lblDomain
			// 
			this.lblDomain.Location = new System.Drawing.Point(59, 67);
			this.lblDomain.Name = "lblDomain";
			this.lblDomain.Size = new System.Drawing.Size(123, 16);
			this.lblDomain.TabIndex = 12;
			this.lblDomain.Text = "Domain:";
			// 
			// lblWarning
			// 
			this.lblWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblWarning.Location = new System.Drawing.Point(3, 0);
			this.lblWarning.Name = "lblWarning";
			this.lblWarning.Size = new System.Drawing.Size(451, 38);
			this.lblWarning.TabIndex = 10;
			this.lblWarning.Text = "Please specify a new Windows user account for the web site anonymous access and a" +
				"pplication pool identity.";
			// 
			// txtConfirmPassword
			// 
			this.txtConfirmPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtConfirmPassword.Location = new System.Drawing.Point(188, 141);
			this.txtConfirmPassword.Name = "txtConfirmPassword";
			this.txtConfirmPassword.PasswordChar = '*';
			this.txtConfirmPassword.Size = new System.Drawing.Size(201, 20);
			this.txtConfirmPassword.TabIndex = 19;
			// 
			// txtUserName
			// 
			this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtUserName.Location = new System.Drawing.Point(188, 89);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(201, 20);
			this.txtUserName.TabIndex = 15;
			// 
			// lblConfirmPassword
			// 
			this.lblConfirmPassword.Location = new System.Drawing.Point(59, 145);
			this.lblConfirmPassword.Name = "lblConfirmPassword";
			this.lblConfirmPassword.Size = new System.Drawing.Size(123, 16);
			this.lblConfirmPassword.TabIndex = 18;
			this.lblConfirmPassword.Text = "Confirm Password:";
			// 
			// lblUserName
			// 
			this.lblUserName.Location = new System.Drawing.Point(59, 93);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(123, 16);
			this.lblUserName.TabIndex = 14;
			this.lblUserName.Text = "User Name:";
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(59, 119);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(123, 16);
			this.lblPassword.TabIndex = 16;
			this.lblPassword.Text = "Password:";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(188, 115);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(201, 20);
			this.txtPassword.TabIndex = 17;
			// 
			// WebSecurityPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkUseActiveDirectory);
			this.Controls.Add(this.txtDomain);
			this.Controls.Add(this.lblDomain);
			this.Controls.Add(this.lblWarning);
			this.Controls.Add(this.txtConfirmPassword);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.lblConfirmPassword);
			this.Controls.Add(this.lblUserName);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.txtPassword);
			this.Name = "WebSecurityPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkUseActiveDirectory;
		private System.Windows.Forms.TextBox txtDomain;
		private System.Windows.Forms.Label lblDomain;
		private System.Windows.Forms.Label lblWarning;
		private System.Windows.Forms.TextBox txtConfirmPassword;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label lblConfirmPassword;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;




	}
}
