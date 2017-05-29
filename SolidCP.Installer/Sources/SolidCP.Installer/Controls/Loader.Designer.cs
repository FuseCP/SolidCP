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
	partial class Loader
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpFiles = new System.Windows.Forms.GroupBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lblProcess = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpFiles
			// 
			this.grpFiles.Controls.Add(this.lblValue);
			this.grpFiles.Controls.Add(this.progressBar);
			this.grpFiles.Controls.Add(this.lblProcess);
			this.grpFiles.Location = new System.Drawing.Point(12, 9);
			this.grpFiles.Name = "grpFiles";
			this.grpFiles.Size = new System.Drawing.Size(448, 88);
			this.grpFiles.TabIndex = 4;
			this.grpFiles.TabStop = false;
			// 
			// lblValue
			// 
			this.lblValue.Location = new System.Drawing.Point(294, 24);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(138, 16);
			this.lblValue.TabIndex = 2;
			this.lblValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(16, 40);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(416, 23);
			this.progressBar.Step = 1;
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 1;
			// 
			// lblProcess
			// 
			this.lblProcess.Location = new System.Drawing.Point(16, 24);
			this.lblProcess.Name = "lblProcess";
			this.lblProcess.Size = new System.Drawing.Size(272, 16);
			this.lblProcess.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(385, 112);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// Loader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(473, 148);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.grpFiles);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Loader";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SolidCP Installer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnLoaderFormClosing);
			this.grpFiles.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpFiles;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lblProcess;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblValue;
	}
}
