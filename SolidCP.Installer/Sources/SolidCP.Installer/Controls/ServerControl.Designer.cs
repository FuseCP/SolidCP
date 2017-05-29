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

namespace SolidCP.Installer.Controls
{
	partial class ServerControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerControl));
			this.grpConnectionSettings = new System.Windows.Forms.GroupBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.lblServer = new System.Windows.Forms.Label();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.grpConnectionSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpConnectionSettings
			// 
			this.grpConnectionSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpConnectionSettings.Controls.Add(this.txtPassword);
			this.grpConnectionSettings.Controls.Add(this.lblPassword);
			this.grpConnectionSettings.Controls.Add(this.txtPort);
			this.grpConnectionSettings.Controls.Add(this.lblPort);
			this.grpConnectionSettings.Controls.Add(this.txtServer);
			this.grpConnectionSettings.Controls.Add(this.lblServer);
			this.grpConnectionSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.grpConnectionSettings.Location = new System.Drawing.Point(14, 43);
			this.grpConnectionSettings.Name = "grpConnectionSettings";
			this.grpConnectionSettings.Size = new System.Drawing.Size(379, 122);
			this.grpConnectionSettings.TabIndex = 0;
			this.grpConnectionSettings.TabStop = false;
			this.grpConnectionSettings.Text = "Connection settings";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(122, 82);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(234, 21);
			this.txtPassword.TabIndex = 5;
			// 
			// lblPassword
			// 
			this.lblPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblPassword.Location = new System.Drawing.Point(16, 82);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(100, 21);
			this.lblPassword.TabIndex = 4;
			this.lblPassword.Text = "Password";
			// 
			// txtPort
			// 
			this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPort.Location = new System.Drawing.Point(122, 55);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(234, 21);
			this.txtPort.TabIndex = 3;
			// 
			// lblPort
			// 
			this.lblPort.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblPort.Location = new System.Drawing.Point(16, 55);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(100, 21);
			this.lblPort.TabIndex = 2;
			this.lblPort.Text = "Port";
			// 
			// txtServer
			// 
			this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtServer.Location = new System.Drawing.Point(122, 28);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(234, 21);
			this.txtServer.TabIndex = 1;
			// 
			// lblServer
			// 
			this.lblServer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblServer.Location = new System.Drawing.Point(16, 28);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new System.Drawing.Size(100, 21);
			this.lblServer.TabIndex = 0;
			this.lblServer.Text = "Server";
			// 
			// btnUpdate
			// 
			this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
			this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnUpdate.Location = new System.Drawing.Point(148, 171);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(128, 28);
			this.btnUpdate.TabIndex = 9;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			// 
			// btnTest
			// 
			this.btnTest.Image = ((System.Drawing.Image)(resources.GetObject("btnTest.Image")));
			this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnTest.Location = new System.Drawing.Point(14, 171);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(128, 28);
			this.btnTest.TabIndex = 8;
			this.btnTest.Text = "Test connection";
			this.btnTest.UseVisualStyleBackColor = true;
			// 
			// btnRemove
			// 
			this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
			this.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRemove.Location = new System.Drawing.Point(14, 9);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(128, 28);
			this.btnRemove.TabIndex = 10;
			this.btnRemove.Text = "Remove server";
			this.btnRemove.UseVisualStyleBackColor = true;
			// 
			// ServerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.grpConnectionSettings);
			this.Name = "ServerControl";
			this.Size = new System.Drawing.Size(406, 327);
			this.grpConnectionSettings.ResumeLayout(false);
			this.grpConnectionSettings.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpConnectionSettings;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.Label lblServer;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnRemove;
	}
}
