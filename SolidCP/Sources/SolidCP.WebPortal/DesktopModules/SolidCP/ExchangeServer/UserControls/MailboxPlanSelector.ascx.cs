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
using System.Web.UI.WebControls;

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class MailboxPlanSelector : SolidCPControlBase
    {
        private void UpdateMailboxPlanSelected()
        {
            foreach (ListItem li in ddlMailboxPlan.Items)
            {
                if (li.Value == mailboxPlanToSelect)
                {
                    ddlMailboxPlan.ClearSelection();
                    li.Selected = true;
                    break;
                }
            }

        }

        private string mailboxPlanToSelect = null;

        public string MailboxPlanId
        {
            get {
                if (ddlMailboxPlan.SelectedItem != null)
                    return ddlMailboxPlan.SelectedItem.Value;
                return mailboxPlanToSelect; 
            }
            set 
            { 
                mailboxPlanToSelect = value; 
                UpdateMailboxPlanSelected(); 
            }
        }

        public bool AddNone
        {
            get { return ViewState["AddNone"] != null ? (bool)ViewState["AddNone"] : false; }
            set { ViewState["AddNone"] = value; }
        }


        public int MailboxPlansCount
        {
            get
            {
                return this.ddlMailboxPlan.Items.Count;
            }
        }

        private bool archiving = false;
        public bool Archiving
        {
            get { return archiving; }
            set { archiving = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMailboxPlans();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ddlMailboxPlan.AutoPostBack = (Changed!=null);
        }

        private void BindMailboxPlans()
        {

            SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[] plans = ES.Services.ExchangeServer.GetExchangeMailboxPlans(PanelRequest.ItemID, Archiving);

            if (AddNone)
            {
                ListItem li = new ListItem();
                li.Text =  "None";
                li.Value = "-1";
                li.Selected = false;
                ddlMailboxPlan.Items.Add(li);
            }

            foreach (SolidCP.Providers.HostedSolution.ExchangeMailboxPlan plan in plans)
            {
                ListItem li = new ListItem();
                li.Text = plan.MailboxPlan;
                li.Value = plan.MailboxPlanId.ToString();
                li.Selected = plan.IsDefault;
                ddlMailboxPlan.Items.Add(li);
            }

            UpdateMailboxPlanSelected();

        }

        public event EventHandler Changed = null;
        protected void ddlMailboxPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }
    }
}
