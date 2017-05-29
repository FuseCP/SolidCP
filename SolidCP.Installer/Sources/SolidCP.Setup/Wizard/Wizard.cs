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
using System.Configuration;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using SolidCP.Setup.Actions;

namespace SolidCP.Setup
{
    //[Designer(typeof(WizardDesigner)), ToolboxBitmap(typeof(Wizard)), DefaultEvent("Cancel")]
    public class Wizard : ContainerControl
    {
		// Events
		public event EventHandler Cancel;
		public event EventHandler Finish;
		
		private string prevText;
		private string nextText;
		private string cancelText;
		private string finishText;
		private string helpText;
		private int bottomMargin;
		private int topMargin;
		private Size buttonSize;
		private Button prevButton;
		private Button nextButton;
		private Button cancelButton;
		private Button helpButton;
		private WizardPageBase selectedPage;
		private bool disabled;
		private bool helpVisible;
		private Image marginImage;
		private Image bannerImage;
		private Font textFont;

		delegate void VoidCallback();

        public Wizard()
        {
            this.prevText = "< &Back";
            this.nextText = "&Next >";
            this.cancelText = "&Cancel";
            this.finishText = "&Finish";
            this.helpText = "&Help";
            this.disabled = false;
            this.helpVisible = false;
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.FixedHeight | ControlStyles.FixedWidth, true);
            this.prevButton = new Button();
            this.prevButton.FlatStyle = FlatStyle.System;
            this.prevButton.Click += new EventHandler(this.OnPrevClick);
            this.prevButton.TabIndex = 0x3e8;
            this.nextButton = new Button();
            this.nextButton.FlatStyle = FlatStyle.System;
            this.nextButton.Click += new EventHandler(this.OnNextClick);
            this.nextButton.TabIndex = 0x3e9;
            this.cancelButton = new Button();
            this.cancelButton.FlatStyle = FlatStyle.System;
            this.cancelButton.Click += new EventHandler(this.OnCancelClick);
            this.cancelButton.TabIndex = 0x3ea;
            this.helpButton = new Button();
            this.helpButton.FlatStyle = FlatStyle.System;
            this.helpButton.Click += new EventHandler(this.OnHelpClick);
            this.helpButton.TabIndex = 0x3eb;
            this.helpButton.Visible = false;
            base.Controls.AddRange(new Control[] { this.prevButton, this.nextButton, this.cancelButton, this.helpButton });
            this.textFont = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point);
            this.RecalculateSize();
            this.SelectedPage = null;

			this.setupVariables = new SetupVariables();
        }

        private void RecalculateSize()
        {
            this.bottomMargin = (int) (Control.DefaultFont.Height * 3.5f);
            this.buttonSize = new Size((int) (Control.DefaultFont.Height * 5.8f), (int) (Control.DefaultFont.Height * 1.8f));
            this.topMargin = (int) (Control.DefaultFont.Height * 4.5f);
            this.Redraw();
        }

        private void DrawBannerPage(PaintEventArgs args)
        {
            Rectangle clientRect = base.ClientRectangle;
            clientRect.Height = this.topMargin;
            args.Graphics.FillRectangle(SystemBrushes.Window, clientRect);
            args.Graphics.DrawLine(SystemPens.ControlDark, clientRect.Left, clientRect.Bottom, clientRect.Right, clientRect.Bottom);
            args.Graphics.DrawLine(SystemPens.ControlLightLight, clientRect.Left, clientRect.Bottom + 1, clientRect.Right, clientRect.Bottom + 1);
            int margin = (this.topMargin - 0x31) / 2;
            Rectangle imageRect = clientRect;
            imageRect.X = imageRect.Right - (0x31 + margin);
            imageRect.Y += margin;
            imageRect.Size = new Size(0x31, 0x31);
            clientRect.Width -= 0x31 + margin;
            if (this.BannerImage != null)
            {
                args.Graphics.DrawImage(this.BannerImage, imageRect);
            }
            else
            {
                args.Graphics.FillRectangle(Brushes.DarkBlue, imageRect);
            }
            using (StringFormat format = new StringFormat(StringFormat.GenericDefault))
            {
                clientRect.X += 0x17;
                clientRect.Width -= 0x17 + (Control.DefaultFont.Height / 2);
                clientRect.Y += (int) (Control.DefaultFont.Height * 0.8f);
                SizeF stringSize = (SizeF) Size.Empty;
				
				using (Font font = new Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point))
                {
                    using (SolidBrush brush = new SolidBrush(((BannerWizardPage) this.SelectedPage).TextColor))
                    {
                        args.Graphics.DrawString(this.SelectedPage.Text, font, brush, (RectangleF) clientRect, format);
                    }
                    stringSize = args.Graphics.MeasureString(this.SelectedPage.Text, font, (SizeF) clientRect.Size, format);
                }
                clientRect.Y += ((int) Math.Ceiling((double) stringSize.Height)) + 1;
                clientRect.X += 0x17;
                clientRect.Width -= 0x17;
                using (SolidBrush brush = new SolidBrush(((BannerWizardPage) this.SelectedPage).DescriptionColor))
                {
                    args.Graphics.DrawString(((BannerWizardPage) this.SelectedPage).Description, this.SelectedPage.Font, brush, (RectangleF) clientRect, format);
                }
            }
        }

        private WizardPageBase[] GetConnectedPages(WizardPageBase currentPage, bool forwardDirection)
        {
            ArrayList list = new ArrayList();
            foreach (Control ctrl in base.Controls)
            {
                if (ctrl is WizardPageBase)
                {
                    WizardPageBase page = (WizardPageBase) ctrl;
                    if (((page.NextPage == currentPage) && forwardDirection) || ((page.PreviousPage == currentPage) && !forwardDirection))
                    {
                        list.Add(page);
                    }
                }
            }
            return (WizardPageBase[]) list.ToArray(typeof(WizardPageBase));
        }

        private void OnHelpClick(object sender, EventArgs args)
        {
            this.OnHelpRequested(new HelpEventArgs(Cursor.Position));
        }

        private WizardPageBase FindFirstWizardPage()
        {
            foreach (Control ctrl in base.Controls)
            {
                if (ctrl is IntroductionPage)
                {
                    return (WizardPageBase) ctrl;
                }
            }
            foreach (Control ctrl in base.Controls)
            {
                if (ctrl is WizardPageBase)
                {
                    return (WizardPageBase) ctrl;
                }
            }
            return null;
        }

        private void DrawMarginPage(PaintEventArgs args)
        {
            Rectangle clientRect = base.ClientRectangle;
            clientRect.Width = 0xa4;
            clientRect.Height -= this.bottomMargin + 2;
			if (this.MarginImage != null)
			{
				args.Graphics.DrawImage(this.MarginImage, new Rectangle(0, 0, this.MarginImage.Width, this.MarginImage.Height));
			}
			else
			{
				args.Graphics.FillRectangle(Brushes.DarkBlue, clientRect);
				
			}
            clientRect = base.ClientRectangle;
            clientRect.X += 0xa4;
            clientRect.Width -= 0xa4;
            clientRect.Height -= this.bottomMargin + 2;
            using (SolidBrush brush = new SolidBrush(this.SelectedPage.BackColor))
            {
                args.Graphics.FillRectangle(brush, clientRect);
            }
            clientRect = base.ClientRectangle;
            clientRect.X += 0xa4 + Control.DefaultFont.Height;
            clientRect.Width -= 0xa4 + (Control.DefaultFont.Height * 2);
            clientRect.Y += Control.DefaultFont.Height;
            using (StringFormat format = new StringFormat(StringFormat.GenericDefault))
            {
                using (SolidBrush brush = new SolidBrush(this.SelectedPage.ForeColor))
                {
                    args.Graphics.DrawString(this.SelectedPage.Text, this.textFont, brush, (RectangleF) clientRect, format);
                }
            }
        }

        private void OnCancelClick(object sender, EventArgs args)
        {
            this.OnCancel(EventArgs.Empty);
        }

        private void InitForm()
        {
            Form form = base.FindForm();
            if ((form != null) && !base.DesignMode)
            {
                form.AcceptButton = this.nextButton;
                form.CancelButton = this.cancelButton;
            }
        }

		private void OnFormShown(object sender, EventArgs e)
		{
			WizardPageBase firstPage = this.SelectedPage;
			if (firstPage != null)
			{
				firstPage.OnBeforeDisplay(EventArgs.Empty);
				firstPage.OnAfterDisplay(EventArgs.Empty);
			}
		}

		public void Close()
		{
			this.OnCancel(EventArgs.Empty);
		}

        private void OnNextClick(object source, EventArgs args)
        {
            this.GoNext();
        }

        internal void RedrawButtons()
        {
            this.prevButton.Text = this.prevText;
            this.nextButton.Text = ((this.SelectedPage == null) || (this.SelectedPage.NextPage != null)) ? this.nextText : this.finishText;
            this.cancelButton.Text = this.cancelText;
            this.helpButton.Text = this.helpText;
            if (this.disabled)
            {
                this.prevButton.Enabled = false;
                this.nextButton.Enabled = false;
                this.cancelButton.Enabled = false;
                this.helpButton.Enabled = false;
            }
            else
            {
                this.prevButton.Enabled = ((this.SelectedPage != null) && (this.SelectedPage.PreviousPage != null)) && (this.SelectedPage.AllowMoveBack || base.DesignMode);
                this.nextButton.Enabled = (this.SelectedPage != null) && (this.SelectedPage.AllowMoveNext || base.DesignMode);
                this.cancelButton.Enabled = (this.SelectedPage != null) && this.SelectedPage.AllowCancel;
                this.helpButton.Enabled = this.SelectedPage != null;
            }
        }

        private void OnPrevClick(object source, EventArgs args)
        {
            this.GoBack();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.textFont.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void Redraw()
        {
            if (this.SelectedPage != null)
            {
                this.SelectedPage.Visible = true;
            }
            foreach (Control ctrl in base.Controls)
            {
                if (ctrl is WizardPageBase)
                {
                    if (ctrl is MarginWizardPage)
                    {
                        ctrl.Bounds = this.CoverPageBounds;
                    }
                    else
                    {
                        ctrl.Bounds = this.ContentPageBounds;
                    }
                    ctrl.Visible = ( ctrl == this.SelectedPage );
                }
            }
            int y = (base.ClientRectangle.Bottom - (this.bottomMargin / 2)) - (this.buttonSize.Height / 2);
            int x = y - (base.ClientRectangle.Bottom - this.bottomMargin);
            Rectangle rect = new Rectangle((base.ClientRectangle.Right - x) - this.buttonSize.Width, y, this.buttonSize.Width, this.buttonSize.Height);
            if (this.HelpVisible)
            {
                this.helpButton.Bounds = rect;
                rect.Offset(-(this.buttonSize.Width + x), 0);
            }
            this.helpButton.Visible = this.HelpVisible;
            this.cancelButton.Bounds = rect;
            rect.Offset(-(this.buttonSize.Width + x), 0);
            this.nextButton.Bounds = rect;
            rect.Offset(-this.buttonSize.Width, 0);
            this.prevButton.Bounds = rect;
            base.Invalidate();
        }

        public WizardPageBase[] GetPagesWithNextPage(WizardPageBase nextPage)
        {
            return this.GetConnectedPages(nextPage, true);
        }

        public WizardPageBase[] GetPagesWithPreviousPage(WizardPageBase previousPage)
        {
            return this.GetConnectedPages(previousPage, false);
        }

        public virtual void GoBack()
        {
           	//thread safe call
			if (this.InvokeRequired)
			{
				VoidCallback callback = new VoidCallback(GoBack);
				this.Invoke(callback);
			}
			else
			{
				this.disabled = true;
				this.RedrawButtons();
				this.Cursor = Cursors.WaitCursor;
				try
				{
					WizardPageBase selectedPage = this.SelectedPage;
					CancelEventArgs args = new CancelEventArgs();
					selectedPage.OnBeforeMoveBack(args);
					if (!args.Cancel)
					{
						WizardPageBase prevPage = selectedPage.PreviousPage;
						this.SelectedPage = prevPage;
						if (prevPage != null)
						{
							prevPage.OnAfterDisplay(EventArgs.Empty);
						}
					}
				}
				finally
				{
					this.disabled = false;
					this.RedrawButtons();
					this.Cursor = Cursors.Default;
				}
			}
        }

        public virtual void GoNext()
        {
			//thread safe call
			if (this.InvokeRequired)
			{
				VoidCallback callback = new VoidCallback(GoNext);
				this.Invoke(callback);
			}
			else
			{
				if (this.SelectedPage != null)
				{
					this.disabled = true;
					this.RedrawButtons();
					this.Cursor = Cursors.WaitCursor;
					try
					{
						WizardPageBase selectedPage = this.SelectedPage;
						CancelEventArgs args = new CancelEventArgs();
						selectedPage.OnBeforeMoveNext(args);
						if (!args.Cancel)
						{
							if (selectedPage.NextPage == null)
							{
								this.OnFinish(EventArgs.Empty);
							}
							else
							{
								WizardPageBase nextPage = selectedPage.NextPage;
								if (nextPage != null)
								{
									nextPage.OnBeforeDisplay(EventArgs.Empty);
								}
								this.SelectedPage = nextPage;
								if (nextPage != null)
								{
									nextPage.OnAfterDisplay(EventArgs.Empty);
								}
							}
						}
					}
					finally
					{
						this.disabled = false;
						this.RedrawButtons();
						this.Cursor = Cursors.Default;
					}
				}
			}
        }

        protected virtual void OnCancel(EventArgs e)
        {
			Log.WriteInfo("Setup wizard was canceled by user");
			Form form = base.FindForm();
            if ((form != null) && form.Modal)
            {
                form.DialogResult = DialogResult.Cancel;
            }
			if (SelectedPage != null && SelectedPage.CustomCancelHandler)
				return;

            if (this.Cancel != null)
            {
                this.Cancel(this, e);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if ((e.Control is WizardPageBase) && (this.SelectedPage == null))
            {
                this.SelectedPage = (WizardPageBase) e.Control;
            }
            else
            {
                this.Redraw();
            }
            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (this.SelectedPage == e.Control)
            {
                if (base.Controls.Contains(this.SelectedPage.PreviousPage))
                {
                    this.SelectedPage = this.SelectedPage.PreviousPage;
                }
                else
                {
                    this.SelectedPage = this.FindFirstWizardPage();
                }
            }
            else
            {
                this.Redraw();
            }
            base.OnControlRemoved(e);
        }

        protected virtual void OnFinish(EventArgs e)
        {
            Form form = base.FindForm();
            if ((form != null) && form.Modal)
            {
                form.DialogResult = DialogResult.OK;
            }
            if (this.Finish != null)
            {
                this.Finish(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            Rectangle clientRect = base.ClientRectangle;
            args.Graphics.DrawLine(SystemPens.ControlLightLight, clientRect.Left, (clientRect.Bottom - this.bottomMargin) - 1, clientRect.Right, (clientRect.Bottom - this.bottomMargin) - 1);
            args.Graphics.DrawLine(SystemPens.ControlDark, clientRect.Left, (clientRect.Bottom - this.bottomMargin) - 2, clientRect.Right, (clientRect.Bottom - this.bottomMargin) - 2);
            if (this.SelectedPage is MarginWizardPage)
            {
                this.DrawMarginPage(args);
            }
            else if (this.SelectedPage is BannerWizardPage)
            {
                this.DrawBannerPage(args);
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.InitForm();
			Form form = base.FindForm();
			if ((form != null) && !base.DesignMode)
			{
				form.Shown += new EventHandler(OnFormShown);
			}
        }

        protected override void OnResize(EventArgs e)
        {
            this.RecalculateSize();
            base.OnResize(e);
        }

        public void SetPagePair(WizardPageBase firstPage, WizardPageBase secondPage)
        {
            firstPage.NextPage = secondPage;
            secondPage.PreviousPage = firstPage;
        }

		public void LinkPages()
		{
			WizardPageBase prevPage = null;
			foreach(Control ctrl in this.Controls)
			{
				WizardPageBase page = ctrl as WizardPageBase;
				if (page != null)
				{
					page.PreviousPage = prevPage;
					if (prevPage != null)
					{
						prevPage.NextPage = page;
					}
					prevPage = page;
				}
			}
		}

        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
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

        [AmbientValue(typeof(Image), null), DefaultValue(typeof(Image), null), Category("Appearance"), Description("The image displayed at the top right on content pages. The image must be 49x49 pixels.")]
        public Image BannerImage
        {
            get
            {
                return this.bannerImage;
            }
            set
            {
                if ((value != null) && ((value.Width != 0x31) || (value.Height != 0x31)))
                {
                    throw new ArgumentException(string.Concat(new object[] { "The banner image must be ", 0x31, " pixels wide and ", 0x31, " pixels high." }));
                }
                this.bannerImage = value;
                if (this.SelectedPage is BannerWizardPage)
                {
                    base.Invalidate();
                }
            }
        }

        [DefaultValue("Cancel")]
        public string CancelText
        {
            get
            {
                return this.cancelText;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.cancelText = value;
                this.RedrawButtons();
            }
        }

        protected virtual Rectangle ContentPageBounds
        {
            get
            {
                Rectangle clientRect = base.ClientRectangle;
                clientRect.Height -= this.bottomMargin + 2;
                clientRect.Y += this.topMargin + 2;
                clientRect.Height -= this.topMargin + 2;
                clientRect.Inflate((int) (-Control.DefaultFont.Height * 1.5), -Control.DefaultFont.Height);
                return clientRect;
            }
        }

        protected virtual Rectangle CoverPageBounds
        {
            get
            {
                Rectangle clientRect = base.ClientRectangle;
                clientRect.Height -= this.bottomMargin + 2;
                clientRect.X += 0xa4;
                clientRect.Width -= 0xa4;
                int num1 = (this.textFont.Height * 2) + Control.DefaultFont.Height;
                clientRect.Y += num1;
                clientRect.Height -= num1;
                clientRect.Inflate(-Control.DefaultFont.Height, -Control.DefaultFont.Height);
                return clientRect;
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(497, 360);
            }
        }

        [DefaultValue("Finish")]
        public string FinishText
        {
            get
            {
                return this.finishText;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.finishText = value;
                this.RedrawButtons();
            }
        }

        [DefaultValue("&Help")]
        public string HelpText
        {
            get
            {
                return this.helpText;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.helpText = value;
                this.RedrawButtons();
            }
        }

        [DefaultValue(false)]
        public bool HelpVisible
        {
            get
            {
                return this.helpVisible;
            }
            set
            {
                this.helpVisible = value;
                this.Redraw();
            }
        }

        [DefaultValue(typeof(Image), null), Description("The image displayed on the left of introduction and finish pages. The image must have a width of 164 pixels."), AmbientValue(typeof(Image), null), Category("Appearance")]
        public Image MarginImage
        {
            get
            {
                return this.marginImage;
            }
            set
            {
                if ((value != null) && (value.Width != 164))
                {
                    throw new ArgumentException("The margin image must be " + 164 + " pixels wide.");
                }
                this.marginImage = value;
                if (this.SelectedPage is MarginWizardPage)
                {
                    base.Invalidate();
                }
            }
        }

        internal Rectangle NextButtonBounds
        {
            get
            {
                return this.nextButton.Bounds;
            }
        }

        [Category("Buttons"), Localizable(true), DefaultValue("&Next >"), Description("Indicates the text that is used on the Next button.")]
        public string NextText
        {
            get
            {
                return this.nextText;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.nextText = value;
                this.RedrawButtons();
            }
        }

        internal Rectangle PreviousButtonBounds
        {
            get
            {
                return this.prevButton.Bounds;
            }
        }

        [Localizable(true), Description("Indicates the text that is used on the Previous button."), Category("Buttons"), DefaultValue("< &Back")]
        public string PreviousText
        {
            get
            {
                return this.prevText;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.prevText = value;
                this.RedrawButtons();
            }
        }

        //[Category("Paging"), Description("The active wizard page.")]
        public WizardPageBase SelectedPage
        {
            get
            {
                return this.selectedPage;
            }
            set
            {
                if ((value != null) && !base.Controls.Contains(value))
                {
                    throw new ArgumentException("The specified page does not belong to the wizard.");
                }
				
				if (value != null && this.selectedPage != value)
				{
					Log.WriteInfo(string.Format("{0} loaded.", value.GetType().Name));
				}

                this.selectedPage = value;
                this.Redraw();
                this.RedrawButtons();
                if (this.selectedPage != null)
                {
                    this.selectedPage.SelectNextControl(null, true, true, true, true);
					this.selectedPage.InitializePage();
                }
                if (base.ActiveControl != null)
                {
                    base.ActiveControl.Focus();
                }
                else if (this.nextButton.Enabled)
                {
                    this.nextButton.Focus();
                }
                this.InitForm();

            }
        }

        [Browsable(false)]
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

		#region Common
		private SetupVariables setupVariables;
		/// <summary>
		/// Installer variables collection
		/// </summary>
		internal SetupVariables SetupVariables
		{
			get { return setupVariables; }
			set { setupVariables = value; }
		}

		public IActionManager ActionManager { get; set; }
		#endregion

		internal void RollBack()
		{
			RollBackPage page = new RollBackPage();
			page.NextPage = null;
			page.PreviousPage = null;
			// Disable Cancel button
			page.AllowCancel = false;
			//
			this.Controls.Add(page);
			this.SelectedPage = page;
		}
	}
}

