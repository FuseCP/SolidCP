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
	partial class FindForm
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
			this.btnFindNext = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.findTabControl = new System.Windows.Forms.TabControl();
			this.findPage = new System.Windows.Forms.TabPage();
			this.chkMatchWord = new System.Windows.Forms.CheckBox();
			this.chkMatchCase = new System.Windows.Forms.CheckBox();
			this.lblSearch = new System.Windows.Forms.Label();
			this.cbSearch = new System.Windows.Forms.ComboBox();
			this.cbReplace = new System.Windows.Forms.ComboBox();
			this.lblReplace = new System.Windows.Forms.Label();
			this.txtFind = new System.Windows.Forms.TextBox();
			this.lblFind = new System.Windows.Forms.Label();
			this.findTabControl.SuspendLayout();
			this.findPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnFindNext
			// 
			this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindNext.Location = new System.Drawing.Point(341, 166);
			this.btnFindNext.Name = "btnFindNext";
			this.btnFindNext.Size = new System.Drawing.Size(75, 23);
			this.btnFindNext.TabIndex = 1;
			this.btnFindNext.Text = "&Find Next";
			this.btnFindNext.UseVisualStyleBackColor = true;
			this.btnFindNext.Click += new System.EventHandler(this.OnFindNext);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(422, 166);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.OnCloseClick);
			// 
			// findTabControl
			// 
			this.findTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.findTabControl.Controls.Add(this.findPage);
			this.findTabControl.Location = new System.Drawing.Point(8, 8);
			this.findTabControl.Name = "findTabControl";
			this.findTabControl.SelectedIndex = 0;
			this.findTabControl.Size = new System.Drawing.Size(489, 152);
			this.findTabControl.TabIndex = 0;
			this.findTabControl.TabStop = false;
			// 
			// findPage
			// 
			this.findPage.Controls.Add(this.chkMatchWord);
			this.findPage.Controls.Add(this.chkMatchCase);
			this.findPage.Controls.Add(this.lblSearch);
			this.findPage.Controls.Add(this.cbSearch);
			this.findPage.Controls.Add(this.cbReplace);
			this.findPage.Controls.Add(this.lblReplace);
			this.findPage.Controls.Add(this.txtFind);
			this.findPage.Controls.Add(this.lblFind);
			this.findPage.Location = new System.Drawing.Point(4, 22);
			this.findPage.Name = "findPage";
			this.findPage.Padding = new System.Windows.Forms.Padding(5);
			this.findPage.Size = new System.Drawing.Size(481, 126);
			this.findPage.TabIndex = 0;
			this.findPage.Text = "Find";
			this.findPage.UseVisualStyleBackColor = true;
			// 
			// chkMatchWord
			// 
			this.chkMatchWord.AutoSize = true;
			this.chkMatchWord.Location = new System.Drawing.Point(192, 94);
			this.chkMatchWord.Name = "chkMatchWord";
			this.chkMatchWord.Size = new System.Drawing.Size(135, 17);
			this.chkMatchWord.TabIndex = 7;
			this.chkMatchWord.Text = "Match &whole word only";
			this.chkMatchWord.UseVisualStyleBackColor = true;
			// 
			// chkMatchCase
			// 
			this.chkMatchCase.AutoSize = true;
			this.chkMatchCase.Location = new System.Drawing.Point(192, 71);
			this.chkMatchCase.Name = "chkMatchCase";
			this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
			this.chkMatchCase.TabIndex = 6;
			this.chkMatchCase.Text = "Match &case";
			this.chkMatchCase.UseVisualStyleBackColor = true;
			// 
			// lblSearch
			// 
			this.lblSearch.Location = new System.Drawing.Point(6, 74);
			this.lblSearch.Name = "lblSearch";
			this.lblSearch.Size = new System.Drawing.Size(68, 23);
			this.lblSearch.TabIndex = 4;
			this.lblSearch.Text = "&Search:";
			// 
			// cbSearch
			// 
			this.cbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSearch.FormattingEnabled = true;
			this.cbSearch.Items.AddRange(new object[] {
            "By Rows",
            "By Columns"});
			this.cbSearch.Location = new System.Drawing.Point(80, 71);
			this.cbSearch.Name = "cbSearch";
			this.cbSearch.Size = new System.Drawing.Size(97, 21);
			this.cbSearch.TabIndex = 5;
			// 
			// cbReplace
			// 
			this.cbReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbReplace.FormattingEnabled = true;
			this.cbReplace.Location = new System.Drawing.Point(99, 35);
			this.cbReplace.Name = "cbReplace";
			this.cbReplace.Size = new System.Drawing.Size(374, 21);
			this.cbReplace.TabIndex = 3;
			this.cbReplace.Visible = false;
			// 
			// lblReplace
			// 
			this.lblReplace.Location = new System.Drawing.Point(6, 36);
			this.lblReplace.Name = "lblReplace";
			this.lblReplace.Size = new System.Drawing.Size(87, 23);
			this.lblReplace.TabIndex = 2;
			this.lblReplace.Text = "R&eplace with:";
			this.lblReplace.Visible = false;
			// 
			// txtFind
			// 
			this.txtFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFind.Location = new System.Drawing.Point(99, 8);
			this.txtFind.Name = "txtFind";
			this.txtFind.Size = new System.Drawing.Size(374, 20);
			this.txtFind.TabIndex = 1;
			this.txtFind.TextChanged += new System.EventHandler(this.OnSearchTextChanged);
			// 
			// lblFind
			// 
			this.lblFind.Location = new System.Drawing.Point(6, 9);
			this.lblFind.Name = "lblFind";
			this.lblFind.Size = new System.Drawing.Size(87, 23);
			this.lblFind.TabIndex = 0;
			this.lblFind.Text = "Fi&nd what:";
			// 
			// FindForm
			// 
			this.AcceptButton = this.btnFindNext;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(505, 197);
			this.ControlBox = false;
			this.Controls.Add(this.findTabControl);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnFindNext);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindForm";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Find";
			this.VisibleChanged += new System.EventHandler(this.OnFormVisibleChanged);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			this.findTabControl.ResumeLayout(false);
			this.findPage.ResumeLayout(false);
			this.findPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnFindNext;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TabControl findTabControl;
		private System.Windows.Forms.TabPage findPage;
		private System.Windows.Forms.TextBox txtFind;
		private System.Windows.Forms.Label lblFind;
		private System.Windows.Forms.ComboBox cbReplace;
		private System.Windows.Forms.Label lblReplace;
		private System.Windows.Forms.ComboBox cbSearch;
		private System.Windows.Forms.Label lblSearch;
		private System.Windows.Forms.CheckBox chkMatchCase;
		private System.Windows.Forms.CheckBox chkMatchWord;
	}
}
