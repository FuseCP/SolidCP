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

namespace SolidCP.UniversalInstaller.WinForms
{
	partial class EmbedEnterpriseServerPage
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
			this.lblIntro = new System.Windows.Forms.Label();
			this.chkBoxEmbed = new System.Windows.Forms.CheckBox();
			this.chkExpose = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// lblIntro
			// 
			this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIntro.Location = new System.Drawing.Point(0, 0);
			this.lblIntro.Name = "lblIntro";
			this.lblIntro.Size = new System.Drawing.Size(457, 58);
			this.lblIntro.TabIndex = 12;
			this.lblIntro.Text = "Please, specify if you want to embed EnterpriseServer into the Portal application" +
    " website. Click Next to continue.";
			// 
			// chkBoxEmbed
			// 
			this.chkBoxEmbed.AutoSize = true;
			this.chkBoxEmbed.Location = new System.Drawing.Point(0, 51);
			this.chkBoxEmbed.Name = "chkBoxEmbed";
			this.chkBoxEmbed.Size = new System.Drawing.Size(232, 17);
			this.chkBoxEmbed.TabIndex = 15;
			this.chkBoxEmbed.Text = "Embed Enterprise Server into Portal website";
			this.chkBoxEmbed.UseVisualStyleBackColor = true;
			this.chkBoxEmbed.CheckedChanged += new System.EventHandler(this.chkBoxEmbed_CheckedChanged);
			// 
			// chkExpose
			// 
			this.chkExpose.AutoSize = true;
			this.chkExpose.Location = new System.Drawing.Point(0, 88);
			this.chkExpose.Name = "chkExpose";
			this.chkExpose.Size = new System.Drawing.Size(210, 17);
			this.chkExpose.TabIndex = 19;
			this.chkExpose.Text = "Expose Enterprise Server Webservices";
			this.chkExpose.UseVisualStyleBackColor = true;
			// 
			// EmbedEnterpriseServerPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkExpose);
			this.Controls.Add(this.chkBoxEmbed);
			this.Controls.Add(this.lblIntro);
			this.Name = "EmbedEnterpriseServerPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.CheckBox chkBoxEmbed;
        private System.Windows.Forms.CheckBox chkExpose;
    }
}
