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
	partial class ConfigurationCheckPage
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationCheckPage));
			this.lvCheck = new System.Windows.Forms.ListView();
			this.colImage = new System.Windows.Forms.ColumnHeader();
			this.colAction = new System.Windows.Forms.ColumnHeader();
			this.colStatus = new System.Windows.Forms.ColumnHeader();
			this.colDetails = new System.Windows.Forms.ColumnHeader();
			this.smallImages = new System.Windows.Forms.ImageList(this.components);
			this.imgOk = new System.Windows.Forms.PictureBox();
			this.lblResult = new System.Windows.Forms.Label();
			this.imgError = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.imgOk)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgError)).BeginInit();
			this.SuspendLayout();
			// 
			// lvCheck
			// 
			this.lvCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvCheck.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colImage,
            this.colAction,
            this.colStatus,
            this.colDetails});
			this.lvCheck.FullRowSelect = true;
			this.lvCheck.GridLines = true;
			this.lvCheck.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvCheck.Location = new System.Drawing.Point(0, 55);
			this.lvCheck.MultiSelect = false;
			this.lvCheck.Name = "lvCheck";
			this.lvCheck.Size = new System.Drawing.Size(457, 145);
			this.lvCheck.SmallImageList = this.smallImages;
			this.lvCheck.TabIndex = 4;
			this.lvCheck.UseCompatibleStateImageBehavior = false;
			this.lvCheck.View = System.Windows.Forms.View.Details;
			// 
			// colImage
			// 
			this.colImage.Text = "";
			this.colImage.Width = 30;
			// 
			// colAction
			// 
			this.colAction.Text = "Action";
			this.colAction.Width = 186;
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			this.colStatus.Width = 99;
			// 
			// colDetails
			// 
			this.colDetails.Text = "Details";
			this.colDetails.Width = 135;
			// 
			// smallImages
			// 
			this.smallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallImages.ImageStream")));
			this.smallImages.TransparentColor = System.Drawing.Color.Transparent;
			this.smallImages.Images.SetKeyName(0, "Run.ico");
			this.smallImages.Images.SetKeyName(1, "Ok.ico");
			this.smallImages.Images.SetKeyName(2, "warning.ico");
			this.smallImages.Images.SetKeyName(3, "error.ico");
			// 
			// imgOk
			// 
			this.imgOk.Image = ((System.Drawing.Image)(resources.GetObject("imgOk.Image")));
			this.imgOk.Location = new System.Drawing.Point(12, 12);
			this.imgOk.Name = "imgOk";
			this.imgOk.Size = new System.Drawing.Size(32, 32);
			this.imgOk.TabIndex = 5;
			this.imgOk.TabStop = false;
			// 
			// lblResult
			// 
			this.lblResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblResult.Location = new System.Drawing.Point(50, 16);
			this.lblResult.Name = "lblResult";
			this.lblResult.Size = new System.Drawing.Size(404, 24);
			this.lblResult.TabIndex = 6;
			this.lblResult.Text = "Success";
			// 
			// imgError
			// 
			this.imgError.Image = ((System.Drawing.Image)(resources.GetObject("imgError.Image")));
			this.imgError.Location = new System.Drawing.Point(12, 12);
			this.imgError.Name = "imgError";
			this.imgError.Size = new System.Drawing.Size(32, 32);
			this.imgError.TabIndex = 7;
			this.imgError.TabStop = false;
			// 
			// ConfigurationCheckPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.imgError);
			this.Controls.Add(this.lblResult);
			this.Controls.Add(this.imgOk);
			this.Controls.Add(this.lvCheck);
			this.Name = "ConfigurationCheckPage";
			this.Size = new System.Drawing.Size(457, 228);
			((System.ComponentModel.ISupportInitialize)(this.imgOk)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgError)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvCheck;
		private System.Windows.Forms.ColumnHeader colAction;
		private System.Windows.Forms.ColumnHeader colStatus;
		private System.Windows.Forms.ColumnHeader colDetails;
		private System.Windows.Forms.ImageList smallImages;
		private System.Windows.Forms.ColumnHeader colImage;
		private System.Windows.Forms.PictureBox imgOk;
		private System.Windows.Forms.Label lblResult;
		private System.Windows.Forms.PictureBox imgError;
	}
}
