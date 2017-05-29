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
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class DomainSelector : System.Web.UI.UserControl
    {
        public string DomainName
        {
            get { return ddlDomain.SelectedItem.Text; }
            set
            {
                foreach (ListItem li in ddlDomain.Items)
                {
                    if (li.Value == value)
                    {
                        ddlDomain.ClearSelection();
                        li.Selected = true;
                        break;
                    }
                }
            }
        }

        public int DomainId
        {
            get
            {
                return Convert.ToInt32(ddlDomain.SelectedValue);
            }
        }

        public int DomainsCount
        {
            get
            {
                return this.ddlDomain.Items.Count;
            }
        }

        public bool ShowAt
        {
            get
            {
                return this.litAt.Visible;
            }
            set
            {
                this.litAt.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDomains();
            }
        }

        private void BindDomains()
        {
            // get domains
            OrganizationDomainName[] domains = ES.Services.Organizations.GetOrganizationDomains(PanelRequest.ItemID);

            // bind domains
            foreach (OrganizationDomainName domain in domains)
            {
                ListItem li = new ListItem(domain.DomainName, domain.DomainId.ToString());
                li.Selected = domain.IsDefault;
                ddlDomain.Items.Add(li);
            }
        }
    }
}
