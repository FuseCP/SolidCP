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
	partial class DatabasePage
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
			this.txtSqlServer = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.lblLogin = new System.Windows.Forms.Label();
			this.txtLogin = new System.Windows.Forms.TextBox();
			this.lblAuthentication = new System.Windows.Forms.Label();
			this.lblSqlServer = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblIntro = new System.Windows.Forms.Label();
			this.txtDatabase = new System.Windows.Forms.TextBox();
			this.lblDatabase = new System.Windows.Forms.Label();
			this.cbAuthentication = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// txtSqlServer
			// 
			this.txtSqlServer.Location = new System.Drawing.Point(147, 36);
			this.txtSqlServer.Name = "txtSqlServer";
			this.txtSqlServer.Size = new System.Drawing.Size(248, 20);
			this.txtSqlServer.TabIndex = 2;
			this.txtSqlServer.Text = "localhost\\SQLExpress";
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(55, 118);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(100, 23);
			this.lblPassword.TabIndex = 7;
			this.lblPassword.Text = "Password:";
			// 
			// lblLogin
			// 
			this.lblLogin.Location = new System.Drawing.Point(55, 92);
			this.lblLogin.Name = "lblLogin";
			this.lblLogin.Size = new System.Drawing.Size(100, 23);
			this.lblLogin.TabIndex = 5;
			this.lblLogin.Text = "Login name:";
			// 
			// txtLogin
			// 
			this.txtLogin.Enabled = false;
			this.txtLogin.Location = new System.Drawing.Point(187, 89);
			this.txtLogin.Name = "txtLogin";
			this.txtLogin.Size = new System.Drawing.Size(208, 20);
			this.txtLogin.TabIndex = 6;
			// 
			// lblAuthentication
			// 
			this.lblAuthentication.Location = new System.Drawing.Point(41, 65);
			this.lblAuthentication.Name = "lblAuthentication";
			this.lblAuthentication.Size = new System.Drawing.Size(100, 23);
			this.lblAuthentication.TabIndex = 3;
			this.lblAuthentication.Text = "Authentication:";
			// 
			// lblSqlServer
			// 
			this.lblSqlServer.Location = new System.Drawing.Point(41, 39);
			this.lblSqlServer.Name = "lblSqlServer";
			this.lblSqlServer.Size = new System.Drawing.Size(100, 23);
			this.lblSqlServer.TabIndex = 1;
			this.lblSqlServer.Text = "SQL Server:";
			// 
			// txtPassword
			// 
			this.txtPassword.Enabled = false;
			this.txtPassword.Location = new System.Drawing.Point(187, 115);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(208, 20);
			this.txtPassword.TabIndex = 8;
			// 
			// lblIntro
			// 
			this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIntro.Location = new System.Drawing.Point(0, 0);
			this.lblIntro.Name = "lblIntro";
			this.lblIntro.Size = new System.Drawing.Size(457, 33);
			this.lblIntro.TabIndex = 0;
			this.lblIntro.Text = "The connection information will be used by the Setup Wizard to install the databa" +
				"se objects only. ";
			// 
			// txtDatabase
			// 
			this.txtDatabase.Location = new System.Drawing.Point(147, 144);
			this.txtDatabase.MaxLength = 128;
			this.txtDatabase.Name = "txtDatabase";
			this.txtDatabase.Size = new System.Drawing.Size(248, 20);
			this.txtDatabase.TabIndex = 10;
			// 
			// lblDatabase
			// 
			this.lblDatabase.Location = new System.Drawing.Point(41, 147);
			this.lblDatabase.Name = "lblDatabase";
			this.lblDatabase.Size = new System.Drawing.Size(100, 23);
			this.lblDatabase.TabIndex = 9;
			this.lblDatabase.Text = "Database:";
			// 
			// cbAuthentication
			// 
			this.cbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAuthentication.FormattingEnabled = true;
			this.cbAuthentication.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
			this.cbAuthentication.Location = new System.Drawing.Point(147, 62);
			this.cbAuthentication.Name = "cbAuthentication";
			this.cbAuthentication.Size = new System.Drawing.Size(248, 21);
			this.cbAuthentication.TabIndex = 4;
			this.cbAuthentication.SelectedIndexChanged += new System.EventHandler(this.OnAuthenticationChanged);
			// 
			// DatabasePage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cbAuthentication);
			this.Controls.Add(this.txtDatabase);
			this.Controls.Add(this.lblDatabase);
			this.Controls.Add(this.txtSqlServer);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.lblLogin);
			this.Controls.Add(this.txtLogin);
			this.Controls.Add(this.lblAuthentication);
			this.Controls.Add(this.lblSqlServer);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.lblIntro);
			this.Name = "DatabasePage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtSqlServer;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Label lblLogin;
		private System.Windows.Forms.TextBox txtLogin;
		private System.Windows.Forms.Label lblAuthentication;
		private System.Windows.Forms.Label lblSqlServer;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblIntro;
		private System.Windows.Forms.TextBox txtDatabase;
		private System.Windows.Forms.Label lblDatabase;
		private System.Windows.Forms.ComboBox cbAuthentication;


	}
}
