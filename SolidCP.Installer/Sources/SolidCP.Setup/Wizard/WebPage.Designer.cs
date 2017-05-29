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
	partial class WebPage
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
			this.lblHint = new System.Windows.Forms.Label();
			this.cbWebSiteIP = new System.Windows.Forms.ComboBox();
			this.lblWebSiteTcpPort = new System.Windows.Forms.Label();
			this.txtWebSiteTcpPort = new System.Windows.Forms.TextBox();
			this.lblWebSiteIP = new System.Windows.Forms.Label();
			this.lblWebSiteDomain = new System.Windows.Forms.Label();
			this.txtWebSiteDomain = new System.Windows.Forms.TextBox();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.lblWarning = new System.Windows.Forms.Label();
			this.grpWebSiteSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpWebSiteSettings
			// 
			this.grpWebSiteSettings.Controls.Add(this.lblHint);
			this.grpWebSiteSettings.Controls.Add(this.cbWebSiteIP);
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteTcpPort);
			this.grpWebSiteSettings.Controls.Add(this.txtWebSiteTcpPort);
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteIP);
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteDomain);
			this.grpWebSiteSettings.Controls.Add(this.txtWebSiteDomain);
			this.grpWebSiteSettings.Location = new System.Drawing.Point(40, 33);
			this.grpWebSiteSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.grpWebSiteSettings.Name = "grpWebSiteSettings";
			this.grpWebSiteSettings.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.grpWebSiteSettings.Size = new System.Drawing.Size(528, 174);
			this.grpWebSiteSettings.TabIndex = 0;
			this.grpWebSiteSettings.TabStop = false;
			this.grpWebSiteSettings.Text = "Web Site Settings";
			// 
			// lblHint
			// 
			this.lblHint.Location = new System.Drawing.Point(40, 143);
			this.lblHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(440, 20);
			this.lblHint.TabIndex = 6;
			this.lblHint.Text = "Example: www.contoso.com or panel.contoso.com";
			// 
			// cbWebSiteIP
			// 
			this.cbWebSiteIP.Location = new System.Drawing.Point(44, 50);
			this.cbWebSiteIP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cbWebSiteIP.Name = "cbWebSiteIP";
			this.cbWebSiteIP.Size = new System.Drawing.Size(292, 24);
			this.cbWebSiteIP.TabIndex = 1;
			this.cbWebSiteIP.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblWebSiteTcpPort
			// 
			this.lblWebSiteTcpPort.Location = new System.Drawing.Point(352, 27);
			this.lblWebSiteTcpPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblWebSiteTcpPort.Name = "lblWebSiteTcpPort";
			this.lblWebSiteTcpPort.Size = new System.Drawing.Size(128, 20);
			this.lblWebSiteTcpPort.TabIndex = 2;
			this.lblWebSiteTcpPort.Text = "Port:";
			// 
			// txtWebSiteTcpPort
			// 
			this.txtWebSiteTcpPort.Location = new System.Drawing.Point(356, 50);
			this.txtWebSiteTcpPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.txtWebSiteTcpPort.Name = "txtWebSiteTcpPort";
			this.txtWebSiteTcpPort.Size = new System.Drawing.Size(63, 22);
			this.txtWebSiteTcpPort.TabIndex = 3;
			this.txtWebSiteTcpPort.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblWebSiteIP
			// 
			this.lblWebSiteIP.Location = new System.Drawing.Point(40, 27);
			this.lblWebSiteIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblWebSiteIP.Name = "lblWebSiteIP";
			this.lblWebSiteIP.Size = new System.Drawing.Size(139, 20);
			this.lblWebSiteIP.TabIndex = 0;
			this.lblWebSiteIP.Text = "IP address:";
			// 
			// lblWebSiteDomain
			// 
			this.lblWebSiteDomain.Location = new System.Drawing.Point(40, 91);
			this.lblWebSiteDomain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblWebSiteDomain.Name = "lblWebSiteDomain";
			this.lblWebSiteDomain.Size = new System.Drawing.Size(112, 20);
			this.lblWebSiteDomain.TabIndex = 4;
			this.lblWebSiteDomain.Text = "Host name:";
			// 
			// txtWebSiteDomain
			// 
			this.txtWebSiteDomain.Location = new System.Drawing.Point(44, 114);
			this.txtWebSiteDomain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.txtWebSiteDomain.Name = "txtWebSiteDomain";
			this.txtWebSiteDomain.Size = new System.Drawing.Size(435, 22);
			this.txtWebSiteDomain.TabIndex = 5;
			this.txtWebSiteDomain.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// txtAddress
			// 
			this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtAddress.Location = new System.Drawing.Point(40, 10);
			this.txtAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.ReadOnly = true;
			this.txtAddress.Size = new System.Drawing.Size(528, 16);
			this.txtAddress.TabIndex = 2;
			// 
			// lblWarning
			// 
			this.lblWarning.Location = new System.Drawing.Point(40, 210);
			this.lblWarning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblWarning.Name = "lblWarning";
			this.lblWarning.Size = new System.Drawing.Size(528, 44);
			this.lblWarning.TabIndex = 1;
			this.lblWarning.Text = "Make sure the specified host name is pointed to this web site; otherwise y" +
				"ou might not be able to access the application.\r\n";
			// 
			// WebPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblWarning);
			this.Controls.Add(this.grpWebSiteSettings);
			this.Controls.Add(this.txtAddress);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "WebPage";
			this.Size = new System.Drawing.Size(609, 281);
			this.grpWebSiteSettings.ResumeLayout(false);
			this.grpWebSiteSettings.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpWebSiteSettings;
		private System.Windows.Forms.Label lblWebSiteTcpPort;
		private System.Windows.Forms.TextBox txtWebSiteTcpPort;
		private System.Windows.Forms.Label lblWebSiteIP;
		private System.Windows.Forms.Label lblWebSiteDomain;
		private System.Windows.Forms.TextBox txtWebSiteDomain;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Label lblWarning;
		private System.Windows.Forms.ComboBox cbWebSiteIP;
		private System.Windows.Forms.Label lblHint;



	}
}
