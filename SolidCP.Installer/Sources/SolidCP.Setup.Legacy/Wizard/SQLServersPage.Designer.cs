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
	partial class SQLServersPage
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
			this.grdServers = new System.Windows.Forms.DataGridView();
			this.lblWarning = new System.Windows.Forms.Label();
			this.colServer = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.grdServers)).BeginInit();
			this.SuspendLayout();
			// 
			// grdServers
			// 
			this.grdServers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grdServers.BackgroundColor = System.Drawing.SystemColors.Window;
			this.grdServers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grdServers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colServer,
            this.colName});
			this.grdServers.Location = new System.Drawing.Point(39, 40);
			this.grdServers.MultiSelect = false;
			this.grdServers.Name = "grdServers";
			this.grdServers.RowHeadersWidth = 21;
			this.grdServers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.grdServers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdServers.Size = new System.Drawing.Size(379, 149);
			this.grdServers.TabIndex = 3;
			// 
			// lblWarning
			// 
			this.lblWarning.Location = new System.Drawing.Point(36, 15);
			this.lblWarning.Name = "lblWarning";
			this.lblWarning.Size = new System.Drawing.Size(396, 22);
			this.lblWarning.TabIndex = 2;
			this.lblWarning.Text = "Please specify SQL servers that will be accessible in myLittle Admin.";
			// 
			// colServer
			// 
			this.colServer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colServer.DataPropertyName = "Server";
			this.colServer.HeaderText = "Server";
			this.colServer.MinimumWidth = 60;
			this.colServer.Name = "colServer";
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colName.DataPropertyName = "Name";
			this.colName.HeaderText = "Name";
			this.colName.MinimumWidth = 60;
			this.colName.Name = "colName";
			this.colName.Width = 60;
			// 
			// SQLServersPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grdServers);
			this.Controls.Add(this.lblWarning);
			this.Name = "SQLServersPage";
			this.Size = new System.Drawing.Size(457, 228);
			((System.ComponentModel.ISupportInitialize)(this.grdServers)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView grdServers;
		private System.Windows.Forms.Label lblWarning;
		private System.Windows.Forms.DataGridViewTextBoxColumn colServer;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;



	}
}
