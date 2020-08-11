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
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class OrganizationAddDomainName : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DomainInfo[] domains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId).Where(d => !Utils.IsIdnDomain(d.DomainName)).ToArray();

            Organization[] orgs = ES.Services.Organizations.GetOrganizations(PanelSecurity.PackageId, false);

            List<OrganizationDomainName> list = new List<OrganizationDomainName>();

            foreach (Organization o in orgs)
            {
                OrganizationDomainName[] tmpList = ES.Services.Organizations.GetOrganizationDomains(o.Id);

                foreach (OrganizationDomainName name in tmpList) list.Add(name);
            }

            foreach (DomainInfo d in domains)
            {
                if (!d.IsDomainPointer)
                {
                    bool bAdd = true;
                    foreach (OrganizationDomainName acceptedDomain in list)
                    {
                        if (d.DomainName.ToLower() == acceptedDomain.DomainName.ToLower())
                        {
                            bAdd = false;
                            break;
                        }

                    }
                    if (bAdd) ddlDomains.Items.Add(d.DomainName.ToLower());
                }
            }

            if (ddlDomains.Items.Count == 0)
            {
                ddlDomains.Visible= btnAdd.Enabled = false;
            }

            

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddDomain();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "org_domains", "SpaceID=" + PanelSecurity.PackageId));

        }


        private void AddDomain()
        {
            if (!Page.IsValid)
                return;

            try
            {

                int result = ES.Services.Organizations.AddOrganizationDomain(PanelRequest.ItemID,
                    ddlDomains.SelectedValue.Trim());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "org_domains",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_ADD_DOMAIN", ex);
            }
        }
    }
}
