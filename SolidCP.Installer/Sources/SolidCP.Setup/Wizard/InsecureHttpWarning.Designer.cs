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
	partial class InsecureHttpWarningPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsecureHttpWarningPage));
			this.grpWebSiteSettings = new System.Windows.Forms.GroupBox();
			this.lblWebSiteDomain = new System.Windows.Forms.Label();
			this.grpWebSiteSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpWebSiteSettings
			// 
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteDomain);
			this.grpWebSiteSettings.Location = new System.Drawing.Point(30, 27);
			this.grpWebSiteSettings.Name = "grpWebSiteSettings";
			this.grpWebSiteSettings.Size = new System.Drawing.Size(396, 141);
			this.grpWebSiteSettings.TabIndex = 0;
			this.grpWebSiteSettings.TabStop = false;
			this.grpWebSiteSettings.Text = "Insecure Http Warning";
			// 
			// lblWebSiteDomain
			// 
			this.lblWebSiteDomain.Location = new System.Drawing.Point(30, 30);
			this.lblWebSiteDomain.Name = "lblWebSiteDomain";
			this.lblWebSiteDomain.Size = new System.Drawing.Size(340, 85);
			this.lblWebSiteDomain.TabIndex = 4;
			this.lblWebSiteDomain.Text = resources.GetString("lblWebSiteDomain.Text");
			// 
			// InsecureHttpWarningPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpWebSiteSettings);
			this.Name = "InsecureHttpWarningPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpWebSiteSettings.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpWebSiteSettings;
		private System.Windows.Forms.Label lblWebSiteDomain;
	}
}
