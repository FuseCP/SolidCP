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
	partial class SettingsControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsControl));
			this.grpWebUpdate = new System.Windows.Forms.GroupBox();
			this.btnCheck = new System.Windows.Forms.Button();
			this.chkAutoUpdate = new System.Windows.Forms.CheckBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.grpProxy = new System.Windows.Forms.GroupBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblUsername = new System.Windows.Forms.Label();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.lblAddress = new System.Windows.Forms.Label();
			this.chkUseHTTPProxy = new System.Windows.Forms.CheckBox();
			this.btnViewLog = new System.Windows.Forms.Button();
			this.grpWebUpdate.SuspendLayout();
			this.grpProxy.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpWebUpdate
			// 
			this.grpWebUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpWebUpdate.Controls.Add(this.btnCheck);
			this.grpWebUpdate.Controls.Add(this.chkAutoUpdate);
			this.grpWebUpdate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.grpWebUpdate.Location = new System.Drawing.Point(14, 3);
			this.grpWebUpdate.Name = "grpWebUpdate";
			this.grpWebUpdate.Size = new System.Drawing.Size(379, 83);
			this.grpWebUpdate.TabIndex = 0;
			this.grpWebUpdate.TabStop = false;
			this.grpWebUpdate.Text = "Web update";
			// 
			// btnCheck
			// 
			this.btnCheck.Image = ((System.Drawing.Image)(resources.GetObject("btnCheck.Image")));
			this.btnCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCheck.Location = new System.Drawing.Point(19, 44);
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Size = new System.Drawing.Size(128, 28);
			this.btnCheck.TabIndex = 1;
			this.btnCheck.Text = "&Check For Updates";
			this.btnCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCheck.UseVisualStyleBackColor = true;
			this.btnCheck.Click += new System.EventHandler(this.OnCheckClick);
			// 
			// chkAutoUpdate
			// 
			this.chkAutoUpdate.Checked = true;
			this.chkAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoUpdate.Location = new System.Drawing.Point(19, 20);
			this.chkAutoUpdate.Name = "chkAutoUpdate";
			this.chkAutoUpdate.Size = new System.Drawing.Size(184, 18);
			this.chkAutoUpdate.TabIndex = 0;
			this.chkAutoUpdate.Text = "Automatically check for updates";
			this.chkAutoUpdate.UseVisualStyleBackColor = true;
			// 
			// btnUpdate
			// 
			this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
			this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnUpdate.Location = new System.Drawing.Point(14, 236);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(128, 28);
			this.btnUpdate.TabIndex = 2;
			this.btnUpdate.Text = "&Save Settings";
			this.btnUpdate.Click += new System.EventHandler(this.OnUpdateClick);
			// 
			// grpProxy
			// 
			this.grpProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpProxy.Controls.Add(this.txtPassword);
			this.grpProxy.Controls.Add(this.lblPassword);
			this.grpProxy.Controls.Add(this.txtUserName);
			this.grpProxy.Controls.Add(this.lblUsername);
			this.grpProxy.Controls.Add(this.txtAddress);
			this.grpProxy.Controls.Add(this.lblAddress);
			this.grpProxy.Controls.Add(this.chkUseHTTPProxy);
			this.grpProxy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.grpProxy.Location = new System.Drawing.Point(14, 91);
			this.grpProxy.Name = "grpProxy";
			this.grpProxy.Size = new System.Drawing.Size(379, 139);
			this.grpProxy.TabIndex = 1;
			this.grpProxy.TabStop = false;
			this.grpProxy.Text = "Proxy";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Enabled = false;
			this.txtPassword.Location = new System.Drawing.Point(125, 101);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(234, 21);
			this.txtPassword.TabIndex = 6;
			// 
			// lblPassword
			// 
			this.lblPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblPassword.Location = new System.Drawing.Point(19, 101);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(100, 21);
			this.lblPassword.TabIndex = 5;
			this.lblPassword.Text = "Password";
			// 
			// txtUserName
			// 
			this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtUserName.Enabled = false;
			this.txtUserName.Location = new System.Drawing.Point(125, 74);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(234, 21);
			this.txtUserName.TabIndex = 4;
			// 
			// lblUsername
			// 
			this.lblUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblUsername.Location = new System.Drawing.Point(19, 74);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(100, 21);
			this.lblUsername.TabIndex = 3;
			this.lblUsername.Text = "User name";
			// 
			// txtAddress
			// 
			this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAddress.Enabled = false;
			this.txtAddress.Location = new System.Drawing.Point(125, 47);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(234, 21);
			this.txtAddress.TabIndex = 2;
			// 
			// lblAddress
			// 
			this.lblAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAddress.Location = new System.Drawing.Point(19, 47);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(100, 21);
			this.lblAddress.TabIndex = 1;
			this.lblAddress.Text = "Address";
			// 
			// chkUseHTTPProxy
			// 
			this.chkUseHTTPProxy.Location = new System.Drawing.Point(19, 22);
			this.chkUseHTTPProxy.Name = "chkUseHTTPProxy";
			this.chkUseHTTPProxy.Size = new System.Drawing.Size(184, 18);
			this.chkUseHTTPProxy.TabIndex = 0;
			this.chkUseHTTPProxy.Text = "Use HTTP Proxy";
			this.chkUseHTTPProxy.UseVisualStyleBackColor = true;
			this.chkUseHTTPProxy.CheckedChanged += new System.EventHandler(this.OnUseHTTPProxyCheckedChanged);
			// 
			// btnViewLog
			// 
			this.btnViewLog.Image = ((System.Drawing.Image)(resources.GetObject("btnViewLog.Image")));
			this.btnViewLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnViewLog.Location = new System.Drawing.Point(148, 236);
			this.btnViewLog.Name = "btnViewLog";
			this.btnViewLog.Size = new System.Drawing.Size(128, 28);
			this.btnViewLog.TabIndex = 3;
			this.btnViewLog.Text = "&View System Log";
			this.btnViewLog.Click += new System.EventHandler(this.OnViewLogClick);
			// 
			// SettingsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnViewLog);
			this.Controls.Add(this.grpProxy);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.grpWebUpdate);
			this.Name = "SettingsControl";
			this.Size = new System.Drawing.Size(406, 327);
			this.grpWebUpdate.ResumeLayout(false);
			this.grpProxy.ResumeLayout(false);
			this.grpProxy.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpWebUpdate;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.CheckBox chkAutoUpdate;
		private System.Windows.Forms.Button btnCheck;
		private System.Windows.Forms.GroupBox grpProxy;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.CheckBox chkUseHTTPProxy;
		private System.Windows.Forms.Button btnViewLog;
	}
}
