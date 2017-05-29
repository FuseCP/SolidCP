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
    public partial class QuotaViewer : SolidCPControlBase
    {
        public bool DisplayGauge
        {
            get { return gauge.DisplayGauge; }
            set { gauge.DisplayGauge = value; UpdateControl(); }
        }

        public int QuotaTypeId
        {
            get { return (ViewState["QuotaTypeId"] != null) ? (int)ViewState["QuotaTypeId"] : 2; }
            set { ViewState["QuotaTypeId"] = value; UpdateControl(); }
        }

        public int QuotaUsedValue
        {
            set
            {
                // store value
                gauge.Progress = value;

                // upodate control
                UpdateControl();
            }
        }

        public int QuotaValue
        {
            set
            {
                // store value
                gauge.Total = value;

                // update control
                UpdateControl();
            }
        }

        public int QuotaAvailable
        {
            set
            {
                // store value
                gauge.Available = value;

                // update control
                UpdateControl();
            }
        }


        private void UpdateControl()
        {
            int total = gauge.Total;
            if (QuotaTypeId == 1)
            {
                litValue.Text = (total == 0) ? GetLocalizedString("Text.Disabled") : GetLocalizedString("Text.Enabled");
                litValue.CssClass = (total == 0) ? "NormalRed" : "NormalGreen";
                gauge.Visible = false;
            }
            else if (QuotaTypeId == 2)
            {
                string availableText = string.Empty;
                if (gauge.Available != -1) availableText = String.Format("({0} {1})", gauge.Available.ToString(), GetLocalizedString("Text.Available"));

                litValue.Text = String.Format("{0} {1} {2} {3}",
                    gauge.Progress, GetLocalizedString("Text.Of"), ((total == -1) ? GetLocalizedString("Text.Unlimited") : total.ToString()), availableText);

                if ((gauge.Progress < 0) & (total == -1))
                    litValue.Text = GetLocalizedString("Text.Unlimited");

                gauge.Visible = (total != -1);
                //litValue.Visible = (value == -1);
            }
            else if (QuotaTypeId == 3)
            {
                litValue.Text = (total == -1) ? GetLocalizedString("Text.Unlimited") : total.ToString();
                gauge.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
