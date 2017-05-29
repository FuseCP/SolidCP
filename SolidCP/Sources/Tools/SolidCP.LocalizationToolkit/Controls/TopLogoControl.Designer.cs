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

namespace SolidCP.LocalizationToolkit
{
	partial class TopLogoControl
    {
		private System.Windows.Forms.Panel pnlLogo;
		private System.Windows.Forms.PictureBox imgLogo;
		private System.Windows.Forms.Label lblVersion;
		private LineBox line;
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TopLogoControl));
			this.pnlLogo = new System.Windows.Forms.Panel();
			this.progressIcon = new SolidCP.LocalizationToolkit.ProgressIcon();
			this.lblVersion = new System.Windows.Forms.Label();
			this.imgLogo = new System.Windows.Forms.PictureBox();
			this.line = new SolidCP.LocalizationToolkit.LineBox();
			this.pnlLogo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlLogo
			// 
			this.pnlLogo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlLogo.BackColor = System.Drawing.Color.White;
			this.pnlLogo.Controls.Add(this.progressIcon);
			this.pnlLogo.Controls.Add(this.lblVersion);
			this.pnlLogo.Controls.Add(this.imgLogo);
			this.pnlLogo.Location = new System.Drawing.Point(0, 0);
			this.pnlLogo.Name = "pnlLogo";
			this.pnlLogo.Size = new System.Drawing.Size(496, 63);
			this.pnlLogo.TabIndex = 2;
			// 
			// progressIcon
			// 
			this.progressIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.progressIcon.Location = new System.Drawing.Point(452, 15);
			this.progressIcon.Name = "progressIcon";
			this.progressIcon.Size = new System.Drawing.Size(30, 30);
			this.progressIcon.TabIndex = 4;
			this.progressIcon.Visible = false;
			// 
			// lblVersion
			// 
			this.lblVersion.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblVersion.ForeColor = System.Drawing.Color.Black;
			this.lblVersion.Location = new System.Drawing.Point(264, 36);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(93, 13);
			this.lblVersion.TabIndex = 2;
			this.lblVersion.Text = "v1.0";
			// 
			// imgLogo
			// 
			this.imgLogo.Image = ((System.Drawing.Image)(resources.GetObject("imgLogo.Image")));
			this.imgLogo.Location = new System.Drawing.Point(13, 7);
			this.imgLogo.Name = "imgLogo";
			this.imgLogo.Size = new System.Drawing.Size(251, 42);
			this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.imgLogo.TabIndex = 0;
			this.imgLogo.TabStop = false;
			// 
			// line
			// 
			this.line.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.line.Location = new System.Drawing.Point(0, 61);
			this.line.Name = "line";
			this.line.Size = new System.Drawing.Size(496, 2);
			this.line.TabIndex = 3;
			this.line.TabStop = false;
			// 
			// TopLogoControl
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.line);
			this.Controls.Add(this.pnlLogo);
			this.Name = "TopLogoControl";
			this.Size = new System.Drawing.Size(496, 64);
			this.pnlLogo.ResumeLayout(false);
			this.pnlLogo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private ProgressIcon progressIcon;


	}
}

