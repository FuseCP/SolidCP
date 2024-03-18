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
	partial class CertificatePage
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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageCertStore = new System.Windows.Forms.TabPage();
			this.groupBoxStore = new System.Windows.Forms.GroupBox();
			this.lblStoreLocation = new System.Windows.Forms.Label();
			this.lblStoreFindValue = new System.Windows.Forms.Label();
			this.txtStoreLocation = new System.Windows.Forms.ComboBox();
			this.lblStoreFindType = new System.Windows.Forms.Label();
			this.txtStoreName = new System.Windows.Forms.ComboBox();
			this.lblStoreName = new System.Windows.Forms.Label();
			this.txtStoreFindType = new System.Windows.Forms.ComboBox();
			this.txtStoreFindValue = new System.Windows.Forms.TextBox();
			this.tabPageCertFile = new System.Windows.Forms.TabPage();
			this.groupBoxCertFile = new System.Windows.Forms.GroupBox();
			this.btnOpenCertFile = new System.Windows.Forms.Button();
			this.txtCertFileFile = new System.Windows.Forms.TextBox();
			this.txtCertFilePassword = new System.Windows.Forms.TextBox();
			this.lblCertFilePassword = new System.Windows.Forms.Label();
			this.lblCertFileFile = new System.Windows.Forms.Label();
			this.tabPageLetsEncrypt = new System.Windows.Forms.TabPage();
			this.grpLESettings = new System.Windows.Forms.GroupBox();
			this.labelProviderCertLE = new System.Windows.Forms.Label();
			this.lblLECertEmail = new System.Windows.Forms.Label();
			this.txtLetsEncryptEmail = new System.Windows.Forms.TextBox();
			this.tabPageManual = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.manualCert = new System.Windows.Forms.CheckBox();
			this.openCertFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.tabControl.SuspendLayout();
			this.tabPageCertStore.SuspendLayout();
			this.groupBoxStore.SuspendLayout();
			this.tabPageCertFile.SuspendLayout();
			this.groupBoxCertFile.SuspendLayout();
			this.tabPageLetsEncrypt.SuspendLayout();
			this.grpLESettings.SuspendLayout();
			this.tabPageManual.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageCertStore);
			this.tabControl.Controls.Add(this.tabPageCertFile);
			this.tabControl.Controls.Add(this.tabPageLetsEncrypt);
			this.tabControl.Controls.Add(this.tabPageManual);
			this.tabControl.Location = new System.Drawing.Point(3, 3);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(451, 225);
			this.tabControl.TabIndex = 1;
			// 
			// tabPageCertStore
			// 
			this.tabPageCertStore.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageCertStore.Controls.Add(this.groupBoxStore);
			this.tabPageCertStore.Location = new System.Drawing.Point(4, 22);
			this.tabPageCertStore.Name = "tabPageCertStore";
			this.tabPageCertStore.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCertStore.Size = new System.Drawing.Size(443, 199);
			this.tabPageCertStore.TabIndex = 0;
			this.tabPageCertStore.Text = "   Certificate Store   ";
			// 
			// groupBoxStore
			// 
			this.groupBoxStore.Controls.Add(this.lblStoreLocation);
			this.groupBoxStore.Controls.Add(this.lblStoreFindValue);
			this.groupBoxStore.Controls.Add(this.txtStoreLocation);
			this.groupBoxStore.Controls.Add(this.lblStoreFindType);
			this.groupBoxStore.Controls.Add(this.txtStoreName);
			this.groupBoxStore.Controls.Add(this.lblStoreName);
			this.groupBoxStore.Controls.Add(this.txtStoreFindType);
			this.groupBoxStore.Controls.Add(this.txtStoreFindValue);
			this.groupBoxStore.Location = new System.Drawing.Point(18, 18);
			this.groupBoxStore.Name = "groupBoxStore";
			this.groupBoxStore.Size = new System.Drawing.Size(401, 164);
			this.groupBoxStore.TabIndex = 8;
			this.groupBoxStore.TabStop = false;
			this.groupBoxStore.Text = "Certificate Store Settings";
			// 
			// lblStoreLocation
			// 
			this.lblStoreLocation.AutoSize = true;
			this.lblStoreLocation.Location = new System.Drawing.Point(33, 36);
			this.lblStoreLocation.Name = "lblStoreLocation";
			this.lblStoreLocation.Size = new System.Drawing.Size(51, 13);
			this.lblStoreLocation.TabIndex = 4;
			this.lblStoreLocation.Text = "Location:";
			// 
			// lblStoreFindValue
			// 
			this.lblStoreFindValue.AutoSize = true;
			this.lblStoreFindValue.Location = new System.Drawing.Point(33, 117);
			this.lblStoreFindValue.Name = "lblStoreFindValue";
			this.lblStoreFindValue.Size = new System.Drawing.Size(60, 13);
			this.lblStoreFindValue.TabIndex = 7;
			this.lblStoreFindValue.Text = "Find Value:";
			// 
			// txtStoreLocation
			// 
			this.txtStoreLocation.FormattingEnabled = true;
			this.txtStoreLocation.Location = new System.Drawing.Point(109, 33);
			this.txtStoreLocation.Name = "txtStoreLocation";
			this.txtStoreLocation.Size = new System.Drawing.Size(258, 21);
			this.txtStoreLocation.TabIndex = 0;
			// 
			// lblStoreFindType
			// 
			this.lblStoreFindType.AutoSize = true;
			this.lblStoreFindType.Location = new System.Drawing.Point(33, 90);
			this.lblStoreFindType.Name = "lblStoreFindType";
			this.lblStoreFindType.Size = new System.Drawing.Size(57, 13);
			this.lblStoreFindType.TabIndex = 6;
			this.lblStoreFindType.Text = "Find Type:";
			// 
			// txtStoreName
			// 
			this.txtStoreName.FormattingEnabled = true;
			this.txtStoreName.Location = new System.Drawing.Point(109, 60);
			this.txtStoreName.Name = "txtStoreName";
			this.txtStoreName.Size = new System.Drawing.Size(258, 21);
			this.txtStoreName.TabIndex = 1;
			// 
			// lblStoreName
			// 
			this.lblStoreName.AutoSize = true;
			this.lblStoreName.Location = new System.Drawing.Point(33, 63);
			this.lblStoreName.Name = "lblStoreName";
			this.lblStoreName.Size = new System.Drawing.Size(35, 13);
			this.lblStoreName.TabIndex = 5;
			this.lblStoreName.Text = "Store:";
			// 
			// txtStoreFindType
			// 
			this.txtStoreFindType.FormattingEnabled = true;
			this.txtStoreFindType.Location = new System.Drawing.Point(109, 87);
			this.txtStoreFindType.Name = "txtStoreFindType";
			this.txtStoreFindType.Size = new System.Drawing.Size(258, 21);
			this.txtStoreFindType.TabIndex = 2;
			// 
			// txtStoreFindValue
			// 
			this.txtStoreFindValue.Location = new System.Drawing.Point(109, 114);
			this.txtStoreFindValue.Name = "txtStoreFindValue";
			this.txtStoreFindValue.Size = new System.Drawing.Size(258, 20);
			this.txtStoreFindValue.TabIndex = 3;
			// 
			// tabPageCertFile
			// 
			this.tabPageCertFile.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageCertFile.Controls.Add(this.groupBoxCertFile);
			this.tabPageCertFile.Location = new System.Drawing.Point(4, 22);
			this.tabPageCertFile.Name = "tabPageCertFile";
			this.tabPageCertFile.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCertFile.Size = new System.Drawing.Size(443, 199);
			this.tabPageCertFile.TabIndex = 1;
			this.tabPageCertFile.Text = "   Certificate File   ";
			// 
			// groupBoxCertFile
			// 
			this.groupBoxCertFile.Controls.Add(this.btnOpenCertFile);
			this.groupBoxCertFile.Controls.Add(this.txtCertFileFile);
			this.groupBoxCertFile.Controls.Add(this.txtCertFilePassword);
			this.groupBoxCertFile.Controls.Add(this.lblCertFilePassword);
			this.groupBoxCertFile.Controls.Add(this.lblCertFileFile);
			this.groupBoxCertFile.Location = new System.Drawing.Point(21, 23);
			this.groupBoxCertFile.Name = "groupBoxCertFile";
			this.groupBoxCertFile.Size = new System.Drawing.Size(400, 147);
			this.groupBoxCertFile.TabIndex = 2;
			this.groupBoxCertFile.TabStop = false;
			this.groupBoxCertFile.Text = "Use a Certificate File";
			// 
			// btnOpenCertFile
			// 
			this.btnOpenCertFile.Image = global::SolidCP.Setup.Properties.Resources.Icons_MenuFileOpenIcon;
			this.btnOpenCertFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnOpenCertFile.Location = new System.Drawing.Point(234, 91);
			this.btnOpenCertFile.Name = "btnOpenCertFile";
			this.btnOpenCertFile.Size = new System.Drawing.Size(145, 32);
			this.btnOpenCertFile.TabIndex = 4;
			this.btnOpenCertFile.Text = "Choose Certificate File";
			this.btnOpenCertFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnOpenCertFile.UseVisualStyleBackColor = true;
			this.btnOpenCertFile.Click += new System.EventHandler(this.btnOpenCertFile_Click);
			// 
			// txtCertFileFile
			// 
			this.txtCertFileFile.Location = new System.Drawing.Point(69, 37);
			this.txtCertFileFile.Name = "txtCertFileFile";
			this.txtCertFileFile.Size = new System.Drawing.Size(310, 20);
			this.txtCertFileFile.TabIndex = 3;
			// 
			// txtCertFilePassword
			// 
			this.txtCertFilePassword.Location = new System.Drawing.Point(69, 65);
			this.txtCertFilePassword.Name = "txtCertFilePassword";
			this.txtCertFilePassword.Size = new System.Drawing.Size(310, 20);
			this.txtCertFilePassword.TabIndex = 2;
			// 
			// lblCertFilePassword
			// 
			this.lblCertFilePassword.AutoSize = true;
			this.lblCertFilePassword.Location = new System.Drawing.Point(6, 68);
			this.lblCertFilePassword.Name = "lblCertFilePassword";
			this.lblCertFilePassword.Size = new System.Drawing.Size(56, 13);
			this.lblCertFilePassword.TabIndex = 1;
			this.lblCertFilePassword.Text = "Password:";
			// 
			// lblCertFileFile
			// 
			this.lblCertFileFile.AutoSize = true;
			this.lblCertFileFile.Location = new System.Drawing.Point(6, 40);
			this.lblCertFileFile.Name = "lblCertFileFile";
			this.lblCertFileFile.Size = new System.Drawing.Size(26, 13);
			this.lblCertFileFile.TabIndex = 0;
			this.lblCertFileFile.Text = "File:";
			// 
			// tabPageLetsEncrypt
			// 
			this.tabPageLetsEncrypt.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageLetsEncrypt.Controls.Add(this.grpLESettings);
			this.tabPageLetsEncrypt.Location = new System.Drawing.Point(4, 22);
			this.tabPageLetsEncrypt.Name = "tabPageLetsEncrypt";
			this.tabPageLetsEncrypt.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageLetsEncrypt.Size = new System.Drawing.Size(443, 199);
			this.tabPageLetsEncrypt.TabIndex = 2;
			this.tabPageLetsEncrypt.Text = "   Let\'s Encrypt   ";
			// 
			// grpLESettings
			// 
			this.grpLESettings.Controls.Add(this.labelProviderCertLE);
			this.grpLESettings.Controls.Add(this.lblLECertEmail);
			this.grpLESettings.Controls.Add(this.txtLetsEncryptEmail);
			this.grpLESettings.Location = new System.Drawing.Point(21, 20);
			this.grpLESettings.Name = "grpLESettings";
			this.grpLESettings.Size = new System.Drawing.Size(396, 141);
			this.grpLESettings.TabIndex = 0;
			this.grpLESettings.TabStop = false;
			this.grpLESettings.Text = " Let\'s Encrypt Settings";
			// 
			// labelProviderCertLE
			// 
			this.labelProviderCertLE.AutoSize = true;
			this.labelProviderCertLE.Location = new System.Drawing.Point(30, 32);
			this.labelProviderCertLE.Name = "labelProviderCertLE";
			this.labelProviderCertLE.Size = new System.Drawing.Size(261, 13);
			this.labelProviderCertLE.TabIndex = 7;
			this.labelProviderCertLE.Text = "Provide a Certificate for your server with Let\'s Encrypt:";
			// 
			// lblLECertEmail
			// 
			this.lblLECertEmail.Location = new System.Drawing.Point(30, 64);
			this.lblLECertEmail.Name = "lblLECertEmail";
			this.lblLECertEmail.Size = new System.Drawing.Size(84, 16);
			this.lblLECertEmail.TabIndex = 4;
			this.lblLECertEmail.Text = "Email address:";
			// 
			// txtLetsEncryptEmail
			// 
			this.txtLetsEncryptEmail.Location = new System.Drawing.Point(33, 83);
			this.txtLetsEncryptEmail.Name = "txtLetsEncryptEmail";
			this.txtLetsEncryptEmail.Size = new System.Drawing.Size(327, 20);
			this.txtLetsEncryptEmail.TabIndex = 5;
			// 
			// tabPageManual
			// 
			this.tabPageManual.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageManual.Controls.Add(this.groupBox1);
			this.tabPageManual.Location = new System.Drawing.Point(4, 22);
			this.tabPageManual.Name = "tabPageManual";
			this.tabPageManual.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageManual.Size = new System.Drawing.Size(443, 199);
			this.tabPageManual.TabIndex = 3;
			this.tabPageManual.Text = "   Manual Configuration   ";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.manualCert);
			this.groupBox1.Location = new System.Drawing.Point(30, 20);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(373, 134);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Manual Certificate Configuration";
			// 
			// manualCert
			// 
			this.manualCert.AutoSize = true;
			this.manualCert.Location = new System.Drawing.Point(46, 75);
			this.manualCert.Name = "manualCert";
			this.manualCert.Size = new System.Drawing.Size(191, 17);
			this.manualCert.TabIndex = 0;
			this.manualCert.Text = "I\'ll configure my certificate manually";
			this.manualCert.UseVisualStyleBackColor = true;
			// 
			// openCertFileDialog
			// 
			this.openCertFileDialog.DefaultExt = "pfx";
			this.openCertFileDialog.FileName = "localhost.pfx";
			this.openCertFileDialog.Filter = "Certificate Files (*.pfx)|*.pfx|All files (*.*)|*.*";
			this.openCertFileDialog.Title = "Use Certificate File";
			// 
			// CertificatePage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl);
			this.Name = "CertificatePage";
			this.Size = new System.Drawing.Size(457, 228);
			this.tabControl.ResumeLayout(false);
			this.tabPageCertStore.ResumeLayout(false);
			this.groupBoxStore.ResumeLayout(false);
			this.groupBoxStore.PerformLayout();
			this.tabPageCertFile.ResumeLayout(false);
			this.groupBoxCertFile.ResumeLayout(false);
			this.groupBoxCertFile.PerformLayout();
			this.tabPageLetsEncrypt.ResumeLayout(false);
			this.grpLESettings.ResumeLayout(false);
			this.grpLESettings.PerformLayout();
			this.tabPageManual.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageCertStore;
		private System.Windows.Forms.TabPage tabPageCertFile;
		private System.Windows.Forms.GroupBox grpLESettings;
		private System.Windows.Forms.Label labelProviderCertLE;
		private System.Windows.Forms.Label lblLECertEmail;
		private System.Windows.Forms.TextBox txtLetsEncryptEmail;
		private System.Windows.Forms.TabPage tabPageLetsEncrypt;
		private System.Windows.Forms.Label lblStoreFindValue;
		private System.Windows.Forms.Label lblStoreFindType;
		private System.Windows.Forms.Label lblStoreName;
		private System.Windows.Forms.Label lblStoreLocation;
		private System.Windows.Forms.TextBox txtStoreFindValue;
		private System.Windows.Forms.ComboBox txtStoreFindType;
		private System.Windows.Forms.ComboBox txtStoreName;
		private System.Windows.Forms.ComboBox txtStoreLocation;
		private System.Windows.Forms.GroupBox groupBoxStore;
		private System.Windows.Forms.GroupBox groupBoxCertFile;
		private System.Windows.Forms.Label lblCertFilePassword;
		private System.Windows.Forms.Label lblCertFileFile;
		private System.Windows.Forms.Button btnOpenCertFile;
		private System.Windows.Forms.TextBox txtCertFileFile;
		private System.Windows.Forms.TextBox txtCertFilePassword;
		private System.Windows.Forms.OpenFileDialog openCertFileDialog;
		private System.Windows.Forms.TabPage tabPageManual;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox manualCert;
	}
}
