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
			this.txtSqlServerServer = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.lblLogin = new System.Windows.Forms.Label();
			this.txtSqlServerLogin = new System.Windows.Forms.TextBox();
			this.lblAuthentication = new System.Windows.Forms.Label();
			this.lblSqlServer = new System.Windows.Forms.Label();
			this.txtSqlServerPassword = new System.Windows.Forms.TextBox();
			this.lblIntro = new System.Windows.Forms.Label();
			this.txtSqlServerDatabase = new System.Windows.Forms.TextBox();
			this.lblDatabase = new System.Windows.Forms.Label();
			this.cbSqlServerAuthentication = new System.Windows.Forms.ComboBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabSqlServer = new System.Windows.Forms.TabPage();
			this.tabMySql = new System.Windows.Forms.TabPage();
			this.txtMySqlPort = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtMySqlPassword = new System.Windows.Forms.TextBox();
			this.txtMySqlDatabase = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtMySqlUser = new System.Windows.Forms.TextBox();
			this.txtMySqlServer = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tabSqlite = new System.Windows.Forms.TabPage();
			this.txtSqliteDatabase = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabSqlServer.SuspendLayout();
			this.tabMySql.SuspendLayout();
			this.tabSqlite.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtSqlServerServer
			// 
			this.txtSqlServerServer.Location = new System.Drawing.Point(127, 17);
			this.txtSqlServerServer.Name = "txtSqlServerServer";
			this.txtSqlServerServer.Size = new System.Drawing.Size(248, 20);
			this.txtSqlServerServer.TabIndex = 2;
			this.txtSqlServerServer.Text = "localhost\\SQLExpress";
			// 
			// lblPassword
			// 
			this.lblPassword.Location = new System.Drawing.Point(35, 99);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(100, 23);
			this.lblPassword.TabIndex = 7;
			this.lblPassword.Text = "Password:";
			// 
			// lblLogin
			// 
			this.lblLogin.Location = new System.Drawing.Point(35, 73);
			this.lblLogin.Name = "lblLogin";
			this.lblLogin.Size = new System.Drawing.Size(100, 23);
			this.lblLogin.TabIndex = 5;
			this.lblLogin.Text = "Login name:";
			// 
			// txtSqlServerLogin
			// 
			this.txtSqlServerLogin.Enabled = false;
			this.txtSqlServerLogin.Location = new System.Drawing.Point(167, 70);
			this.txtSqlServerLogin.Name = "txtSqlServerLogin";
			this.txtSqlServerLogin.Size = new System.Drawing.Size(208, 20);
			this.txtSqlServerLogin.TabIndex = 6;
			// 
			// lblAuthentication
			// 
			this.lblAuthentication.Location = new System.Drawing.Point(21, 46);
			this.lblAuthentication.Name = "lblAuthentication";
			this.lblAuthentication.Size = new System.Drawing.Size(100, 23);
			this.lblAuthentication.TabIndex = 3;
			this.lblAuthentication.Text = "Authentication:";
			// 
			// lblSqlServer
			// 
			this.lblSqlServer.Location = new System.Drawing.Point(21, 20);
			this.lblSqlServer.Name = "lblSqlServer";
			this.lblSqlServer.Size = new System.Drawing.Size(100, 23);
			this.lblSqlServer.TabIndex = 1;
			this.lblSqlServer.Text = "SQL Server:";
			// 
			// txtSqlServerPassword
			// 
			this.txtSqlServerPassword.Enabled = false;
			this.txtSqlServerPassword.Location = new System.Drawing.Point(167, 96);
			this.txtSqlServerPassword.Name = "txtSqlServerPassword";
			this.txtSqlServerPassword.PasswordChar = '*';
			this.txtSqlServerPassword.Size = new System.Drawing.Size(208, 20);
			this.txtSqlServerPassword.TabIndex = 8;
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
			// txtSqlServerDatabase
			// 
			this.txtSqlServerDatabase.Location = new System.Drawing.Point(127, 125);
			this.txtSqlServerDatabase.MaxLength = 128;
			this.txtSqlServerDatabase.Name = "txtSqlServerDatabase";
			this.txtSqlServerDatabase.Size = new System.Drawing.Size(248, 20);
			this.txtSqlServerDatabase.TabIndex = 10;
			this.txtSqlServerDatabase.Text = "SolidCP";
			// 
			// lblDatabase
			// 
			this.lblDatabase.Location = new System.Drawing.Point(21, 128);
			this.lblDatabase.Name = "lblDatabase";
			this.lblDatabase.Size = new System.Drawing.Size(100, 23);
			this.lblDatabase.TabIndex = 9;
			this.lblDatabase.Text = "Database:";
			// 
			// cbSqlServerAuthentication
			// 
			this.cbSqlServerAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSqlServerAuthentication.FormattingEnabled = true;
			this.cbSqlServerAuthentication.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
			this.cbSqlServerAuthentication.Location = new System.Drawing.Point(127, 43);
			this.cbSqlServerAuthentication.Name = "cbSqlServerAuthentication";
			this.cbSqlServerAuthentication.Size = new System.Drawing.Size(248, 21);
			this.cbSqlServerAuthentication.TabIndex = 4;
			this.cbSqlServerAuthentication.SelectedIndexChanged += new System.EventHandler(this.OnAuthenticationChanged);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabSqlServer);
			this.tabControl.Controls.Add(this.tabMySql);
			this.tabControl.Controls.Add(this.tabSqlite);
			this.tabControl.Location = new System.Drawing.Point(0, 36);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(451, 189);
			this.tabControl.TabIndex = 11;
			// 
			// tabSqlServer
			// 
			this.tabSqlServer.BackColor = System.Drawing.SystemColors.Control;
			this.tabSqlServer.Controls.Add(this.lblSqlServer);
			this.tabSqlServer.Controls.Add(this.cbSqlServerAuthentication);
			this.tabSqlServer.Controls.Add(this.txtSqlServerPassword);
			this.tabSqlServer.Controls.Add(this.txtSqlServerDatabase);
			this.tabSqlServer.Controls.Add(this.lblAuthentication);
			this.tabSqlServer.Controls.Add(this.lblDatabase);
			this.tabSqlServer.Controls.Add(this.txtSqlServerLogin);
			this.tabSqlServer.Controls.Add(this.txtSqlServerServer);
			this.tabSqlServer.Controls.Add(this.lblLogin);
			this.tabSqlServer.Controls.Add(this.lblPassword);
			this.tabSqlServer.Location = new System.Drawing.Point(4, 22);
			this.tabSqlServer.Name = "tabSqlServer";
			this.tabSqlServer.Padding = new System.Windows.Forms.Padding(3);
			this.tabSqlServer.Size = new System.Drawing.Size(443, 163);
			this.tabSqlServer.TabIndex = 0;
			this.tabSqlServer.Text = "   SQL Server  ";
			// 
			// tabMySql
			// 
			this.tabMySql.BackColor = System.Drawing.SystemColors.Control;
			this.tabMySql.Controls.Add(this.txtMySqlPort);
			this.tabMySql.Controls.Add(this.label2);
			this.tabMySql.Controls.Add(this.label1);
			this.tabMySql.Controls.Add(this.txtMySqlPassword);
			this.tabMySql.Controls.Add(this.txtMySqlDatabase);
			this.tabMySql.Controls.Add(this.label3);
			this.tabMySql.Controls.Add(this.txtMySqlUser);
			this.tabMySql.Controls.Add(this.txtMySqlServer);
			this.tabMySql.Controls.Add(this.label4);
			this.tabMySql.Controls.Add(this.label5);
			this.tabMySql.Location = new System.Drawing.Point(4, 22);
			this.tabMySql.Name = "tabMySql";
			this.tabMySql.Padding = new System.Windows.Forms.Padding(3);
			this.tabMySql.Size = new System.Drawing.Size(443, 163);
			this.tabMySql.TabIndex = 1;
			this.tabMySql.Text = "  MySQL / MariaDB  ";
			// 
			// txtMySqlPort
			// 
			this.txtMySqlPort.Location = new System.Drawing.Point(132, 44);
			this.txtMySqlPort.MaxLength = 128;
			this.txtMySqlPort.Name = "txtMySqlPort";
			this.txtMySqlPort.Size = new System.Drawing.Size(248, 20);
			this.txtMySqlPort.TabIndex = 22;
			this.txtMySqlPort.Text = "3306";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(26, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 21;
			this.label2.Text = "Port:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(26, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 11;
			this.label1.Text = "Server:";
			// 
			// txtMySqlPassword
			// 
			this.txtMySqlPassword.Location = new System.Drawing.Point(172, 97);
			this.txtMySqlPassword.Name = "txtMySqlPassword";
			this.txtMySqlPassword.PasswordChar = '*';
			this.txtMySqlPassword.Size = new System.Drawing.Size(208, 20);
			this.txtMySqlPassword.TabIndex = 18;
			// 
			// txtMySqlDatabase
			// 
			this.txtMySqlDatabase.Location = new System.Drawing.Point(132, 126);
			this.txtMySqlDatabase.MaxLength = 128;
			this.txtMySqlDatabase.Name = "txtMySqlDatabase";
			this.txtMySqlDatabase.Size = new System.Drawing.Size(248, 20);
			this.txtMySqlDatabase.TabIndex = 20;
			this.txtMySqlDatabase.Text = "SolidCP";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(26, 129);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 23);
			this.label3.TabIndex = 19;
			this.label3.Text = "Database:";
			// 
			// txtMySqlUser
			// 
			this.txtMySqlUser.Location = new System.Drawing.Point(172, 71);
			this.txtMySqlUser.Name = "txtMySqlUser";
			this.txtMySqlUser.Size = new System.Drawing.Size(208, 20);
			this.txtMySqlUser.TabIndex = 16;
			// 
			// txtMySqlServer
			// 
			this.txtMySqlServer.Location = new System.Drawing.Point(132, 18);
			this.txtMySqlServer.Name = "txtMySqlServer";
			this.txtMySqlServer.Size = new System.Drawing.Size(248, 20);
			this.txtMySqlServer.TabIndex = 12;
			this.txtMySqlServer.Text = "localhost";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(40, 74);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 23);
			this.label4.TabIndex = 15;
			this.label4.Text = "Login name:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(40, 100);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 23);
			this.label5.TabIndex = 17;
			this.label5.Text = "Password:";
			// 
			// tabSqlite
			// 
			this.tabSqlite.BackColor = System.Drawing.SystemColors.Control;
			this.tabSqlite.Controls.Add(this.txtSqliteDatabase);
			this.tabSqlite.Controls.Add(this.label6);
			this.tabSqlite.Location = new System.Drawing.Point(4, 22);
			this.tabSqlite.Name = "tabSqlite";
			this.tabSqlite.Padding = new System.Windows.Forms.Padding(3);
			this.tabSqlite.Size = new System.Drawing.Size(443, 163);
			this.tabSqlite.TabIndex = 2;
			this.tabSqlite.Text = "  SQLite  ";
			// 
			// txtSqliteDatabase
			// 
			this.txtSqliteDatabase.Location = new System.Drawing.Point(129, 33);
			this.txtSqliteDatabase.MaxLength = 128;
			this.txtSqliteDatabase.Name = "txtSqliteDatabase";
			this.txtSqliteDatabase.Size = new System.Drawing.Size(248, 20);
			this.txtSqliteDatabase.TabIndex = 22;
			this.txtSqliteDatabase.Text = "SolidCP";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(23, 36);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 23);
			this.label6.TabIndex = 21;
			this.label6.Text = "Database:";
			// 
			// DatabasePage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.lblIntro);
			this.Name = "DatabasePage";
			this.Size = new System.Drawing.Size(457, 228);
			this.tabControl.ResumeLayout(false);
			this.tabSqlServer.ResumeLayout(false);
			this.tabSqlServer.PerformLayout();
			this.tabMySql.ResumeLayout(false);
			this.tabMySql.PerformLayout();
			this.tabSqlite.ResumeLayout(false);
			this.tabSqlite.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtSqlServerServer;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Label lblLogin;
		private System.Windows.Forms.TextBox txtSqlServerLogin;
		private System.Windows.Forms.Label lblAuthentication;
		private System.Windows.Forms.Label lblSqlServer;
		private System.Windows.Forms.TextBox txtSqlServerPassword;
		private System.Windows.Forms.Label lblIntro;
		private System.Windows.Forms.TextBox txtSqlServerDatabase;
		private System.Windows.Forms.Label lblDatabase;
		private System.Windows.Forms.ComboBox cbSqlServerAuthentication;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabSqlServer;
		private System.Windows.Forms.TabPage tabMySql;
		private System.Windows.Forms.TabPage tabSqlite;
		private System.Windows.Forms.TextBox txtMySqlPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMySqlPassword;
		private System.Windows.Forms.TextBox txtMySqlDatabase;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtMySqlUser;
		private System.Windows.Forms.TextBox txtMySqlServer;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtSqliteDatabase;
		private System.Windows.Forms.Label label6;
	}
}
