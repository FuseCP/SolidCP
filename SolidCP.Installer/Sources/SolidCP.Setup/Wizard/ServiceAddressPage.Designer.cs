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
	partial class ServiceAddressPage
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
			this.grpSettings = new System.Windows.Forms.GroupBox();
			this.cbIP = new System.Windows.Forms.ComboBox();
			this.lblTcpPort = new System.Windows.Forms.Label();
			this.txtTcpPort = new System.Windows.Forms.TextBox();
			this.lblIP = new System.Windows.Forms.Label();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.grpSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.cbIP);
			this.grpSettings.Controls.Add(this.lblTcpPort);
			this.grpSettings.Controls.Add(this.txtTcpPort);
			this.grpSettings.Controls.Add(this.lblIP);
			this.grpSettings.Location = new System.Drawing.Point(30, 37);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Size = new System.Drawing.Size(396, 76);
			this.grpSettings.TabIndex = 1;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Address Settings";
			// 
			// cbIP
			// 
			this.cbIP.Location = new System.Drawing.Point(140, 19);
			this.cbIP.Name = "cbIP";
			this.cbIP.Size = new System.Drawing.Size(220, 21);
			this.cbIP.TabIndex = 3;
			this.cbIP.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblTcpPort
			// 
			this.lblTcpPort.Location = new System.Drawing.Point(30, 50);
			this.lblTcpPort.Name = "lblTcpPort";
			this.lblTcpPort.Size = new System.Drawing.Size(96, 16);
			this.lblTcpPort.TabIndex = 4;
			this.lblTcpPort.Text = "TCP Port:";
			// 
			// txtTcpPort
			// 
			this.txtTcpPort.Location = new System.Drawing.Point(140, 46);
			this.txtTcpPort.Name = "txtTcpPort";
			this.txtTcpPort.Size = new System.Drawing.Size(48, 20);
			this.txtTcpPort.TabIndex = 5;
			this.txtTcpPort.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblIP
			// 
			this.lblIP.Location = new System.Drawing.Point(30, 22);
			this.lblIP.Name = "lblIP";
			this.lblIP.Size = new System.Drawing.Size(104, 16);
			this.lblIP.TabIndex = 0;
			this.lblIP.Text = "IP Address:";
			// 
			// txtAddress
			// 
			this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtAddress.Location = new System.Drawing.Point(28, 11);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.ReadOnly = true;
			this.txtAddress.Size = new System.Drawing.Size(398, 13);
			this.txtAddress.TabIndex = 0;
			// 
			// ServiceAddressPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpSettings);
			this.Controls.Add(this.txtAddress);
			this.Name = "ServiceAddressPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpSettings.ResumeLayout(false);
			this.grpSettings.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpSettings;
		private System.Windows.Forms.Label lblTcpPort;
		private System.Windows.Forms.TextBox txtTcpPort;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.ComboBox cbIP;



	}
}
