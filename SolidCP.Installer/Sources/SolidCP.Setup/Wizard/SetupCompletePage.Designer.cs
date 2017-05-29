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
	partial class SetupCompletePage
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
			this.lblViewLog = new System.Windows.Forms.Label();
			this.lnkViewSetupLog = new System.Windows.Forms.LinkLabel();
			this.lblAccounts = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.lblServeradmin = new System.Windows.Forms.Label();
			this.lblAdmin = new System.Windows.Forms.Label();
			this.lblServeradminDescription = new System.Windows.Forms.Label();
			this.lblAdminDescription = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblViewLog
			// 
			this.lblViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblViewLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblViewLog.Location = new System.Drawing.Point(0, 4);
			this.lblViewLog.Name = "lblViewLog";
			this.lblViewLog.Size = new System.Drawing.Size(457, 33);
			this.lblViewLog.TabIndex = 1;
			this.lblViewLog.Text = "Refer to the setup log for information describing any failure(s) that occurred du" +
				"ring setup. Click Finish to exit the Setup Wizard.\r\n\r\n";
			// 
			// lnkViewSetupLog
			// 
			this.lnkViewSetupLog.AutoSize = true;
			this.lnkViewSetupLog.Location = new System.Drawing.Point(4, 37);
			this.lnkViewSetupLog.Name = "lnkViewSetupLog";
			this.lnkViewSetupLog.Size = new System.Drawing.Size(82, 13);
			this.lnkViewSetupLog.TabIndex = 2;
			this.lnkViewSetupLog.TabStop = true;
			this.lnkViewSetupLog.Text = "View Setup Log";
			this.lnkViewSetupLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnViewLogLinkClicked);
			// 
			// lblAccounts
			// 
			this.lblAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAccounts.Location = new System.Drawing.Point(0, 60);
			this.lblAccounts.Name = "lblAccounts";
			this.lblAccounts.Size = new System.Drawing.Size(457, 22);
			this.lblAccounts.TabIndex = 3;
			this.lblAccounts.Text = "Use one of the following administrator accounts to log on to SolidCP:\r\n";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(3, 206);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(116, 13);
			this.linkLabel1.TabIndex = 4;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Log on to SolidCP";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLoginClicked);
			// 
			// lblServeradmin
			// 
			this.lblServeradmin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblServeradmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblServeradmin.Location = new System.Drawing.Point(4, 82);
			this.lblServeradmin.Name = "lblServeradmin";
			this.lblServeradmin.Size = new System.Drawing.Size(113, 22);
			this.lblServeradmin.TabIndex = 5;
			this.lblServeradmin.Text = "serveradmin";
			// 
			// lblAdmin
			// 
			this.lblAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAdmin.Location = new System.Drawing.Point(3, 137);
			this.lblAdmin.Name = "lblAdmin";
			this.lblAdmin.Size = new System.Drawing.Size(113, 22);
			this.lblAdmin.TabIndex = 6;
			this.lblAdmin.Text = "admin";
			// 
			// lblServeradminDescription
			// 
			this.lblServeradminDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblServeradminDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblServeradminDescription.Location = new System.Drawing.Point(0, 103);
			this.lblServeradminDescription.Name = "lblServeradminDescription";
			this.lblServeradminDescription.Size = new System.Drawing.Size(457, 34);
			this.lblServeradminDescription.TabIndex = 7;
			this.lblServeradminDescription.Text = "The built-in serveradmin user account has a high level of access privileges. It i" +
				"s used for top-level administration and configuring.";
			// 
			// lblAdminDescription
			// 
			this.lblAdminDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblAdminDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAdminDescription.Location = new System.Drawing.Point(4, 159);
			this.lblAdminDescription.Name = "lblAdminDescription";
			this.lblAdminDescription.Size = new System.Drawing.Size(453, 29);
			this.lblAdminDescription.TabIndex = 8;
			this.lblAdminDescription.Text = "Admin user account is used for managing hosting resources.";
			// 
			// SetupCompletePage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblAdminDescription);
			this.Controls.Add(this.lblServeradminDescription);
			this.Controls.Add(this.lblAdmin);
			this.Controls.Add(this.lblServeradmin);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.lblAccounts);
			this.Controls.Add(this.lnkViewSetupLog);
			this.Controls.Add(this.lblViewLog);
			this.Name = "SetupCompletePage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblViewLog;
		private System.Windows.Forms.LinkLabel lnkViewSetupLog;
		private System.Windows.Forms.Label lblAccounts;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label lblServeradmin;
		private System.Windows.Forms.Label lblAdmin;
		private System.Windows.Forms.Label lblServeradminDescription;
		private System.Windows.Forms.Label lblAdminDescription;

	}
}
