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
    public partial class ScheduleInterval : SolidCPControlBase
    {
        public int Interval
        {
            get
            {
                int multiplier = 1;
                if (ddlUnits.SelectedIndex == 0)
                    multiplier = 86400;
                else if (ddlUnits.SelectedIndex == 1)
                    multiplier = 3600;
                else if (ddlUnits.SelectedIndex == 2)
                    multiplier = 60;
                else if (ddlUnits.SelectedIndex == 3)
                    multiplier = 1;

                return Utils.ParseInt(txtInterval.Text.Trim(), 0) * multiplier;
            }
            set
            {
                ListItem item = ddlUnits.SelectedItem;
                if (item != null)
                    item.Selected = false;

                int s = value;
                if (s % 86400 == 0)
                {
                    // days
                    ddlUnits.SelectedIndex = 0;
                    txtInterval.Text = ((int)(s / 86400)).ToString();
                }
                else if (s % 3600 == 0)
                {
                    // hours
                    ddlUnits.SelectedIndex = 1;
                    txtInterval.Text = ((int)(s / 3600)).ToString();
                }
                else if (s % 60 == 0)
                {
                    // minutes
                    ddlUnits.SelectedIndex = 2;
                    txtInterval.Text = ((int)(s / 60)).ToString();
                }
                else
                {
                    // seconds
                    ddlUnits.SelectedIndex = 3;
                    txtInterval.Text = s.ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
