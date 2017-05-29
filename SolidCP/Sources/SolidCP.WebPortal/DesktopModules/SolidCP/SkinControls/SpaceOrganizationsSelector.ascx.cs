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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.WebPortal;

namespace SolidCP.Portal.SkinControls
{
    public partial class SpaceOrganizationsSelector : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSpaceOrgs();
            }
        }

        private void BindSpaceOrgs()
        {
            // organization
            bool orgVisible = (PanelRequest.ItemID > 0 && Request[DefaultPage.PAGE_ID_PARAM].Equals(UserSpaceBreadcrumb.PID_SPACE_EXCHANGE_SERVER, StringComparison.InvariantCultureIgnoreCase));

            spanOrgsSelector.Visible = orgVisible;

            if (orgVisible)
            {
                OrganizationsHelper helper = new OrganizationsHelper();

                ddlSpaceOrgs.DataSource = helper.GetOrganizations(PanelSecurity.PackageId, false);
                ddlSpaceOrgs.DataTextField = "ItemName";
                ddlSpaceOrgs.DataValueField = "ItemID";
                ddlSpaceOrgs.DataBind();

                ddlSpaceOrgs.Items.FindByValue(PanelRequest.ItemID.ToString()).Selected = true; 

                lnkOrgnsList.NavigateUrl = PortalUtils.NavigatePageURL(
                        PortalUtils.GetCurrentPageId(), "SpaceID", PanelSecurity.PackageId.ToString());
            }
        }

        protected void ddlSpaceOrgs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(GetOrganizationEditUrl(ddlSpaceOrgs.SelectedValue));
        }

        private string GetOrganizationEditUrl(string itemId)
        {
            return PortalUtils.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "organization_home",
                    "ItemID=" + itemId);
        }
    }
}
