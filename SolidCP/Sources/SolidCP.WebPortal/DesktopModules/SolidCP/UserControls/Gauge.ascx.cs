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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SolidCP.Portal
{
    public partial class Gauge : SolidCPControlBase
    {
        private int width = 100;
		private bool oneColour = false;

        public bool DisplayGauge
        {
            get { return (ViewState["DisplayGauge"] != null) ? (bool)ViewState["DisplayGauge"] : true; }
            set { ViewState["DisplayGauge"] = value; }
        }

        public bool DisplayText
        {
            get { return (ViewState["DisplayText"] != null) ? (bool)ViewState["DisplayText"] : true; }
            set { ViewState["DisplayText"] = value; }
        }

        public int Progress
        {
            get { return (ViewState["Progress"] != null) ? (int)ViewState["Progress"] : 0; }
            set { ViewState["Progress"] = value; }
        }

        public int Total
        {
            get { return (ViewState["Total"] != null) ? (int)ViewState["Total"] : 0; }
            set { ViewState["Total"] = value; }
        }

        public int Available
        {
            get { return (ViewState["Available"] != null) ? (int)ViewState["Available"] : -1; }
            set { ViewState["Available"] = value; }
        }
        
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

		public bool OneColour
		{
			get { return this.oneColour; }
			set { this.oneColour = value; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!DisplayGauge)
                return;

            string leftSideSrc = Page.ResolveUrl(PortalUtils.GetThemedImage("gauge_left.gif"));
            string rightSideSrc = Page.ResolveUrl(PortalUtils.GetThemedImage("gauge_right.gif"));
            string bkgSrc = Page.ResolveUrl(PortalUtils.GetThemedImage("gauge_bkg.gif"));

            // calculate the width of the gauge
            int fTotal = Total;
            int percent = (fTotal > 0) ? Convert.ToInt32(Math.Round((double)Progress / (double)fTotal * 100)) : 0;

            double fFilledWidth = (fTotal > 0) ? ((double)Progress / (double)fTotal * Width) : 0;
            int filledWidth = Convert.ToInt32(fFilledWidth);

			if (filledWidth > Width)
				filledWidth = Width;

            string fillSrc = "gauge_green.gif";
            if(percent > 60 && percent < 90 && !oneColour)
                fillSrc = "gauge_yellow.gif";
            else if (percent >= 90 && !oneColour)
                fillSrc = "gauge_red.gif";

            fillSrc = Page.ResolveUrl(PortalUtils.GetThemedImage(fillSrc));

            GaugeContent.Text = DrawImage(leftSideSrc, 1);
            GaugeContent.Text += DrawImage(fillSrc, filledWidth);
            GaugeContent.Text += DrawImage(bkgSrc, width - filledWidth);
            GaugeContent.Text += DrawImage(rightSideSrc, 1);
        }

        private string DrawImage(string src, int width)
        {
            return String.Format("<img src=\"{0}\" width=\"{1}\" height=\"11\" align=\"absmiddle\"/>",
                src, width);
        }
    }
}
