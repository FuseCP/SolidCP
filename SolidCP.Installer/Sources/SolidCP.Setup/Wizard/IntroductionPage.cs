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
    public class IntroductionPage : MarginWizardPage
    {
        public IntroductionPage()
        {
        }

		protected override void InitializePageInternal()
		{
			string product = Wizard.SetupVariables.Product;
			if (string.IsNullOrEmpty(product))
				product = "SolidCP";
			this.introductionText = string.Format("This wizard will guide you through the installation of {0} product.\n\n" +
				"It is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer.",
				product);
			this.Text = string.Format("Welcome to the {0} Setup Wizard", product);
			
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
				Wizard.GoNext();
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (StringFormat format = new StringFormat(StringFormat.GenericDefault))
            {
                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    e.Graphics.DrawString(this.IntroductionText, this.Font, brush, (RectangleF) base.ClientRectangle, format);
                }
            }
        }

        public string IntroductionText
        {
            get
            {
                return this.introductionText;
            }
            set
            {
                this.introductionText = value;
                base.Invalidate();
            }
        }


        private string introductionText;
    }
}

