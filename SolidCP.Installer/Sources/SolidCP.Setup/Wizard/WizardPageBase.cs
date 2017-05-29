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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SolidCP.Setup
{
    //[DefaultEvent("BeforeDisplay"), ToolboxItem(false), Designer(typeof(WizardPageBaseDesigner))]
    public abstract class WizardPageBase : UserControl
    {
		// Events
		public event EventHandler AfterDisplay;
		public event EventHandler BeforeDisplay;
		public event CancelEventHandler BeforeMoveBack;
		public event CancelEventHandler BeforeMoveNext;
		
		
		// Fields
		private WizardPageBase previousPage;
		private WizardPageBase nextPage;
		private bool allowMoveBack;
		private bool allowMoveNext;
		private bool allowCancel;
		private bool customCancelHandler;
		private bool initialized = false;

        protected WizardPageBase()
        {
            this.allowMoveBack = true;
            this.allowMoveNext = true;
            this.allowCancel = true;
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ContainerControl, true);
			CheckForIllegalCrossThreadCalls = false;
        }

        protected internal virtual void OnAfterDisplay(EventArgs e)
        {
            if (this.AfterDisplay != null)
            {
                this.AfterDisplay(this, e);
            }
        }

        protected internal virtual void OnBeforeDisplay(EventArgs e)
        {
            if (this.BeforeDisplay != null)
            {
                this.BeforeDisplay(this, e);
            }
        }

        protected internal virtual void OnBeforeMoveBack(CancelEventArgs e)
        {
            if (this.BeforeMoveBack != null)
            {
                this.BeforeMoveBack(this, e);
            }
        }

        protected internal virtual void OnBeforeMoveNext(CancelEventArgs e)
        {
            if (this.BeforeMoveNext != null)
            {
                this.BeforeMoveNext(this, e);
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            if (this.IsCurrentPage)
            {
                ((Wizard) base.Parent).Redraw();
            }
            base.OnFontChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (this.IsCurrentPage)
            {
                ((Wizard) base.Parent).Redraw();
            }
            base.OnTextChanged(e);
        }


        [Category("Behavior"), Description("Indicates whether the user will be allowed to cancel the wizard from this page."), DefaultValue(true)]
        public bool AllowCancel
        {
            get
            {
                return this.allowCancel;
            }
            set
            {
                this.allowCancel = value;
                if (this.IsCurrentPage)
                {
                    ((Wizard) base.Parent).RedrawButtons();
                }
            }
        }

        [Description("Indicates whether the user will be allowed to move forwards from this page."), Category("Paging"), DefaultValue(true)]
        public bool AllowMoveNext
        {
            get
            {
                return this.allowMoveNext;
            }
            set
            {
                this.allowMoveNext = value;
                if (this.IsCurrentPage)
                {
                    ((Wizard) base.Parent).RedrawButtons();
                }
            }
        }

        [DefaultValue(true), Category("Paging"), Description("Indicates whether the user will be allowed to move backwards from the page.")]
        public bool AllowMoveBack
        {
            get
            {
                return this.allowMoveBack;
            }
            set
            {
                this.allowMoveBack = value;
                if (this.IsCurrentPage)
                {
                    ((Wizard) base.Parent).RedrawButtons();
                }
            }
        }

		[DefaultValue(false)]
		public bool CustomCancelHandler
		{
			get { return this.customCancelHandler; }
			set { this.customCancelHandler = value; }
		}

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false)]
        public bool IsCurrentPage
        {
            get
            {
                if (base.Parent is Wizard)
                {
                    return (((Wizard) base.Parent).SelectedPage == this);
                }
                return false;
            }
        }

        [Description("The page to move to when the user presses the Next button."), DefaultValue(typeof(WizardPageBase), null), Category("Paging")]
        public WizardPageBase NextPage
        {
            get
            {
                return this.nextPage;
            }
            set
            {
                if (value == this)
                {
                    throw new ArgumentException("Cannot navigate from one page to the same page.");
                }
                this.nextPage = value;
                if (this.IsCurrentPage)
                {
                    ((Wizard) base.Parent).RedrawButtons();
                }
            }
        }

        [Description("The page to move to when the user presses the Back button."), DefaultValue(typeof(WizardPageBase), null), Category("Paging")]
        public WizardPageBase PreviousPage
        {
            get
            {
                return this.previousPage;
            }
            set
            {
                if (value == this)
                {
                    throw new ArgumentException("Cannot navigate from one page to the same page.");
                }
                this.previousPage = value;
                if (this.IsCurrentPage)
                {
                    ((Wizard) base.Parent).RedrawButtons();
                }
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [Browsable(false)]
        public Wizard Wizard
        {
            get
            {
                return (base.Parent as Wizard);
            }
        }

		private SetupVariables setupVariables;
		[Browsable(false)]
		public SetupVariables SetupVariables
		{
			get
			{
				if (setupVariables == null)
				{
					if ( Wizard != null )
						setupVariables = Wizard.SetupVariables;
				}
				return setupVariables;
			}
			set
			{
				setupVariables = value;
			}
		}

		/// <summary>
		/// Displays an error message box with the specified text.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		protected void ShowError(string text)
		{
			MessageBox.Show(this, text, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		protected void ShowError()
		{
			ShowError("An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.");
		}

		/// <summary>
		/// Displays an warning message box with the specified text.
		/// </summary>
		/// <param name="text">The text to display in the message box.</param>
		protected void ShowWarning(string text)
		{
			MessageBox.Show(this, text, FindForm().Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		internal void InitializePage()
		{
			if (!this.initialized)
			{
				this.initialized = true;
				InitializePageInternal();
			}
		}

		protected virtual void InitializePageInternal()
		{
		}

		delegate void RollBackCallback();
		/// <summary>
		/// Thread safe application rollback
		/// </summary>
		protected void Rollback()
		{
			// InvokeRequired compares thread ID of the
			// calling thread to thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.InvokeRequired)
			{
				RollBackCallback callback = new RollBackCallback(Rollback);
				Invoke(callback, null);
			}
			else
			{
				Wizard.RollBack();
			}
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// WizardPageBase
			// 
			this.Name = "WizardPageBase";
			this.ResumeLayout(false);

		}
	}
}

