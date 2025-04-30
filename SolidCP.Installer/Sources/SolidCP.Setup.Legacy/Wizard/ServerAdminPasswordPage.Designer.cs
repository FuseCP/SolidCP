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
	partial class ServerAdminPasswordPage
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
			this.lblConfirmPassword = new System.Windows.Forms.Label();
			this.txtConfirmPassword = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.chkChangePassword = new System.Windows.Forms.CheckBox();
			this.lblNote = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblConfirmPassword
			// 
			this.lblConfirmPassword.Location = new System.Drawing.Point(53, 124);
			this.lblConfirmPassword.Name = "lblConfirmPassword";
			this.lblConfirmPassword.Size = new System.Drawing.Size(106, 23);
			this.lblConfirmPassword.TabIndex = 4;
			this.lblConfirmPassword.Text = "Confirm password:";
			// 
			// txtConfirmPassword
			// 
			this.txtConfirmPassword.Location = new System.Drawing.Point(165, 124);
			this.txtConfirmPassword.Name = "txtConfirmPassword";
			this.txtConfirmPassword.PasswordChar = '*';
			this.txtConfirmPassword.Size = new System.Drawing.Size(170, 20);
			this.txtConfirmPassword.TabIndex = 5;
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(53, 92);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(106, 23);
			this.lblPassword.TabIndex = 2;
			this.lblPassword.Text = "Password:";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(165, 92);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(170, 20);
			this.txtPassword.TabIndex = 3;
			// 
			// chkChangePassword
			// 
			this.chkChangePassword.Checked = true;
			this.chkChangePassword.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkChangePassword.Location = new System.Drawing.Point(56, 52);
			this.chkChangePassword.Name = "chkChangePassword";
			this.chkChangePassword.Size = new System.Drawing.Size(279, 25);
			this.chkChangePassword.TabIndex = 1;
			this.chkChangePassword.Text = "Reset Serveradmin Password";
			this.chkChangePassword.UseVisualStyleBackColor = true;
			this.chkChangePassword.CheckedChanged += new System.EventHandler(this.OnChangePasswordChecked);
			// 
			// lblNote
			// 
			this.lblNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblNote.Location = new System.Drawing.Point(0, 184);
			this.lblNote.Name = "lblNote";
			this.lblNote.Size = new System.Drawing.Size(457, 38);
			this.lblNote.TabIndex = 6;
			// 
			// ServerAdminPasswordPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblNote);
			this.Controls.Add(this.chkChangePassword);
			this.Controls.Add(this.lblConfirmPassword);
			this.Controls.Add(this.txtConfirmPassword);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.txtPassword);
			this.Name = "ServerAdminPasswordPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblConfirmPassword;
		private System.Windows.Forms.TextBox txtConfirmPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.CheckBox chkChangePassword;
		private System.Windows.Forms.Label lblNote;

	}
}
