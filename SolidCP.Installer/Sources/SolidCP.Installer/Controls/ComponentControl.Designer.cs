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
	partial class ComponentControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentControl));
			this.grpInfo = new System.Windows.Forms.GroupBox();
			this.txtVersion = new System.Windows.Forms.TextBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.txtComponent = new System.Windows.Forms.TextBox();
			this.lblComponent = new System.Windows.Forms.Label();
			this.txtApplication = new System.Windows.Forms.TextBox();
			this.lblApplication = new System.Windows.Forms.Label();
			this.grpDescription = new System.Windows.Forms.GroupBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.btnCheckUpdate = new System.Windows.Forms.Button();
			this.btnSettings = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.grpInfo.SuspendLayout();
			this.grpDescription.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpInfo
			// 
			this.grpInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpInfo.Controls.Add(this.txtVersion);
			this.grpInfo.Controls.Add(this.lblVersion);
			this.grpInfo.Controls.Add(this.txtComponent);
			this.grpInfo.Controls.Add(this.lblComponent);
			this.grpInfo.Controls.Add(this.txtApplication);
			this.grpInfo.Controls.Add(this.lblApplication);
			this.grpInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.grpInfo.Location = new System.Drawing.Point(14, 43);
			this.grpInfo.Name = "grpInfo";
			this.grpInfo.Size = new System.Drawing.Size(379, 122);
			this.grpInfo.TabIndex = 3;
			this.grpInfo.TabStop = false;
			this.grpInfo.Text = "Summary";
			// 
			// txtVersion
			// 
			this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtVersion.Location = new System.Drawing.Point(122, 82);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.ReadOnly = true;
			this.txtVersion.Size = new System.Drawing.Size(234, 21);
			this.txtVersion.TabIndex = 5;
			// 
			// lblVersion
			// 
			this.lblVersion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblVersion.Location = new System.Drawing.Point(16, 82);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(100, 21);
			this.lblVersion.TabIndex = 4;
			this.lblVersion.Text = "Version";
			// 
			// txtComponent
			// 
			this.txtComponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtComponent.Location = new System.Drawing.Point(122, 55);
			this.txtComponent.Name = "txtComponent";
			this.txtComponent.ReadOnly = true;
			this.txtComponent.Size = new System.Drawing.Size(234, 21);
			this.txtComponent.TabIndex = 3;
			// 
			// lblComponent
			// 
			this.lblComponent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblComponent.Location = new System.Drawing.Point(16, 55);
			this.lblComponent.Name = "lblComponent";
			this.lblComponent.Size = new System.Drawing.Size(100, 21);
			this.lblComponent.TabIndex = 2;
			this.lblComponent.Text = "Component";
			// 
			// txtApplication
			// 
			this.txtApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtApplication.Location = new System.Drawing.Point(122, 28);
			this.txtApplication.Name = "txtApplication";
			this.txtApplication.ReadOnly = true;
			this.txtApplication.Size = new System.Drawing.Size(234, 21);
			this.txtApplication.TabIndex = 1;
			// 
			// lblApplication
			// 
			this.lblApplication.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblApplication.Location = new System.Drawing.Point(16, 28);
			this.lblApplication.Name = "lblApplication";
			this.lblApplication.Size = new System.Drawing.Size(100, 21);
			this.lblApplication.TabIndex = 0;
			this.lblApplication.Text = "Application";
			// 
			// grpDescription
			// 
			this.grpDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpDescription.Controls.Add(this.lblDescription);
			this.grpDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.grpDescription.Location = new System.Drawing.Point(14, 171);
			this.grpDescription.Name = "grpDescription";
			this.grpDescription.Size = new System.Drawing.Size(379, 117);
			this.grpDescription.TabIndex = 4;
			this.grpDescription.TabStop = false;
			this.grpDescription.Text = "Description";
			// 
			// lblDescription
			// 
			this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDescription.Location = new System.Drawing.Point(3, 17);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(373, 97);
			this.lblDescription.TabIndex = 0;
			// 
			// btnCheckUpdate
			// 
			this.btnCheckUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckUpdate.Image")));
			this.btnCheckUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCheckUpdate.Location = new System.Drawing.Point(14, 9);
			this.btnCheckUpdate.Name = "btnCheckUpdate";
			this.btnCheckUpdate.Size = new System.Drawing.Size(128, 28);
			this.btnCheckUpdate.TabIndex = 0;
			this.btnCheckUpdate.Text = " &Check For Updates";
			this.btnCheckUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCheckUpdate.UseVisualStyleBackColor = true;
			this.btnCheckUpdate.Click += new System.EventHandler(this.OnCheckUpdateClick);
			// 
			// btnSettings
			// 
			this.btnSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSettings.Image")));
			this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSettings.Location = new System.Drawing.Point(262, 9);
			this.btnSettings.Name = "btnSettings";
			this.btnSettings.Size = new System.Drawing.Size(108, 28);
			this.btnSettings.TabIndex = 2;
			this.btnSettings.Text = "&Settings";
			this.btnSettings.UseVisualStyleBackColor = true;
			this.btnSettings.Click += new System.EventHandler(this.OnSettingsClick);
			// 
			// btnRemove
			// 
			this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
			this.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRemove.Location = new System.Drawing.Point(148, 9);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(108, 28);
			this.btnRemove.TabIndex = 1;
			this.btnRemove.Text = "&Uninstall";
			this.btnRemove.Click += new System.EventHandler(this.OnRemoveClick);
			// 
			// ComponentControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpDescription);
			this.Controls.Add(this.grpInfo);
			this.Controls.Add(this.btnCheckUpdate);
			this.Controls.Add(this.btnSettings);
			this.Controls.Add(this.btnRemove);
			this.Name = "ComponentControl";
			this.Size = new System.Drawing.Size(406, 327);
			this.grpInfo.ResumeLayout(false);
			this.grpInfo.PerformLayout();
			this.grpDescription.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnSettings;
		private System.Windows.Forms.Button btnCheckUpdate;
		private System.Windows.Forms.GroupBox grpInfo;
		private System.Windows.Forms.TextBox txtVersion;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.TextBox txtComponent;
		private System.Windows.Forms.Label lblComponent;
		private System.Windows.Forms.TextBox txtApplication;
		private System.Windows.Forms.Label lblApplication;
		private System.Windows.Forms.GroupBox grpDescription;
		private System.Windows.Forms.Label lblDescription;
	}
}
