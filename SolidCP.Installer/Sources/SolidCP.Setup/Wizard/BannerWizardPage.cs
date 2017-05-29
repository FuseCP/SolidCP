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
using System.Windows.Forms;

namespace SolidCP.Setup
{
    //[Designer(typeof(WizardPageDesigner))]
    public class BannerWizardPage : WizardPageBase
    {
        public BannerWizardPage()
        {
            this.description = "Description.";
            this.proceedText = "";
            this.textColor = SystemColors.WindowText;
            this.descriptionColor = SystemColors.WindowText;
            this.Text = "Wizard Page";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (StringFormat format = new StringFormat(StringFormat.GenericDefault))
            {
                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    Rectangle rect = base.ClientRectangle;
                    rect.Inflate(-Control.DefaultFont.Height * 2, 0);
                    e.Graphics.DrawString(this.ProceedText, this.Font, brush, (RectangleF) rect, format);
                }
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                if (base.IsCurrentPage)
                {
                    ((Wizard) base.Parent).Redraw();
                }
            }
        }

        public Color DescriptionColor
        {
            get
            {
                return this.descriptionColor;
            }
            set
            {
                this.descriptionColor = value;
                if (base.IsCurrentPage)
                {
                    base.Parent.Invalidate();
                }
            }
        }

        [DefaultValue("")]
        public virtual string ProceedText
        {
            get
            {
                return this.proceedText;
            }
            set
            {
                this.proceedText = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "WindowText")]
        public Color TextColor
        {
            get
            {
                return this.textColor;
            }
            set
            {
                this.textColor = value;
                if (base.IsCurrentPage)
                {
                    base.Parent.Invalidate();
                }
            }
        }


        private string description;
        private string proceedText;
        private Color textColor;
        private Color descriptionColor;
    }
}

