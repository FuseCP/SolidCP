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

namespace SolidCP.Installer
{
	partial class ApplicationForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationForm));
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.scopeTree = new System.Windows.Forms.TreeView();
            this.smallImages = new System.Windows.Forms.ImageList(this.components);
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlResultView = new System.Windows.Forms.Panel();
            this.pnlDescription = new System.Windows.Forms.Panel();
            this.lineBox2 = new SolidCP.Installer.LineBox();
            this.lblResultViewPath = new System.Windows.Forms.Label();
            this.lblResultViewTitle = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.topLogoControl = new SolidCP.Installer.TopLogoControl();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 431);
            this.statusBar.Name = "statusBar";
            this.statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusBar.Size = new System.Drawing.Size(632, 22);
            this.statusBar.TabIndex = 1;
            // 
            // statusBarLabel
            // 
            this.statusBarLabel.Name = "statusBarLabel";
            this.statusBarLabel.Size = new System.Drawing.Size(39, 17);
            this.statusBarLabel.Text = "Ready";
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 63);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.scopeTree);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.pnlRight);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.splitContainer.Size = new System.Drawing.Size(632, 368);
            this.splitContainer.SplitterDistance = 250;
            this.splitContainer.SplitterWidth = 2;
            this.splitContainer.TabIndex = 2;
            // 
            // scopeTree
            // 
            this.scopeTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scopeTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scopeTree.HideSelection = false;
            this.scopeTree.ImageIndex = 0;
            this.scopeTree.ImageList = this.smallImages;
            this.scopeTree.ItemHeight = 19;
            this.scopeTree.Location = new System.Drawing.Point(0, 1);
            this.scopeTree.Name = "scopeTree";
            this.scopeTree.SelectedImageIndex = 0;
            this.scopeTree.ShowLines = false;
            this.scopeTree.Size = new System.Drawing.Size(250, 367);
            this.scopeTree.TabIndex = 0;
            this.scopeTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnScopeTreeBeforeExpand);
            this.scopeTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnScopeTreeAfterSelect);
            // 
            // smallImages
            // 
            this.smallImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.smallImages.ImageSize = new System.Drawing.Size(16, 16);
            this.smallImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlRight
            // 
            this.pnlRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlRight.Controls.Add(this.pnlResultView);
            this.pnlRight.Controls.Add(this.pnlDescription);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(0, 1);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(380, 367);
            this.pnlRight.TabIndex = 0;
            // 
            // pnlResultView
            // 
            this.pnlResultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResultView.Location = new System.Drawing.Point(0, 63);
            this.pnlResultView.Name = "pnlResultView";
            this.pnlResultView.Size = new System.Drawing.Size(376, 300);
            this.pnlResultView.TabIndex = 14;
            // 
            // pnlDescription
            // 
            this.pnlDescription.Controls.Add(this.lineBox2);
            this.pnlDescription.Controls.Add(this.lblResultViewPath);
            this.pnlDescription.Controls.Add(this.lblResultViewTitle);
            this.pnlDescription.Controls.Add(this.pictureBox);
            this.pnlDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDescription.Location = new System.Drawing.Point(0, 0);
            this.pnlDescription.Name = "pnlDescription";
            this.pnlDescription.Size = new System.Drawing.Size(376, 63);
            this.pnlDescription.TabIndex = 13;
            // 
            // lineBox2
            // 
            this.lineBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lineBox2.Location = new System.Drawing.Point(6, 51);
            this.lineBox2.Name = "lineBox2";
            this.lineBox2.Size = new System.Drawing.Size(366, 2);
            this.lineBox2.TabIndex = 16;
            this.lineBox2.TabStop = false;
            this.lineBox2.Text = "lineBox2";
            // 
            // lblResultViewPath
            // 
            this.lblResultViewPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultViewPath.AutoEllipsis = true;
            this.lblResultViewPath.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResultViewPath.Location = new System.Drawing.Point(47, 28);
            this.lblResultViewPath.Name = "lblResultViewPath";
            this.lblResultViewPath.Size = new System.Drawing.Size(251, 21);
            this.lblResultViewPath.TabIndex = 15;
            this.lblResultViewPath.Text = "Path";
            // 
            // lblResultViewTitle
            // 
            this.lblResultViewTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultViewTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblResultViewTitle.Location = new System.Drawing.Point(47, 8);
            this.lblResultViewTitle.Name = "lblResultViewTitle";
            this.lblResultViewTitle.Size = new System.Drawing.Size(318, 20);
            this.lblResultViewTitle.TabIndex = 14;
            this.lblResultViewTitle.Text = "Title";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(9, 8);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(32, 32);
            this.pictureBox.TabIndex = 13;
            this.pictureBox.TabStop = false;
            // 
            // topLogoControl
            // 
            this.topLogoControl.BackColor = System.Drawing.Color.White;
            this.topLogoControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.topLogoControl.Location = new System.Drawing.Point(0, 0);
            this.topLogoControl.Name = "topLogoControl";
            this.topLogoControl.Size = new System.Drawing.Size(632, 63);
            this.topLogoControl.TabIndex = 3;
            // 
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.topLogoControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "ApplicationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SolidCP Installer";
            this.Shown += new System.EventHandler(this.OnApplicationFormShown);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.Panel pnlRight;
		private TopLogoControl topLogoControl;
		private System.Windows.Forms.TreeView scopeTree;
		private System.Windows.Forms.Panel pnlDescription;
		private LineBox lineBox2;
		private System.Windows.Forms.Label lblResultViewPath;
		private System.Windows.Forms.Label lblResultViewTitle;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.ToolStripStatusLabel statusBarLabel;
		private System.Windows.Forms.ImageList smallImages;
		private System.Windows.Forms.Panel pnlResultView;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
	}
}
