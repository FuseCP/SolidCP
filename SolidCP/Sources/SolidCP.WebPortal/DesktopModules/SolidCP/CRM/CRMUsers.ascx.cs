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
using SolidCP.EnterpriseServer;
﻿using SolidCP.EnterpriseServer.Base.HostedSolution;
﻿using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.CRM
{
    public partial class CRMUsers : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            if (org.CrmOrganizationId == Guid.Empty)
            {
                messageBox.ShowErrorMessage("NOT_CRM_ORGANIZATION");
                btnCreateUser.Enabled = false;
                CRM2011Panel.Visible = false;
                CRM2013Panel.Visible = false;
            }
            else
            {
                OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                {
                    CRM2011Panel.Visible = false;
                    CRM2013Panel.Visible = true;

                    professionalusersQuota.QuotaUsedValue = stats.CreatedProfessionalCRMUsers;
                    professionalusersQuota.QuotaValue = stats.AllocatedProfessionalCRMUsers;

                    basicusersQuota.QuotaUsedValue = stats.CreatedBasicCRMUsers;
                    basicusersQuota.QuotaValue = stats.AllocatedBasicCRMUsers;

                    essentialusersQuota.QuotaUsedValue = stats.CreatedEssentialCRMUsers;
                    essentialusersQuota.QuotaValue = stats.AllocatedEssentialCRMUsers;
                }
                else
                {
                    CRM2011Panel.Visible = true;
                    CRM2013Panel.Visible = false;

                    usersQuota.QuotaUsedValue = stats.CreatedCRMUsers;
                    usersQuota.QuotaValue = stats.AllocatedCRMUsers;

                    limitedusersQuota.QuotaUsedValue = stats.CreatedLimitedCRMUsers;
                    limitedusersQuota.QuotaValue = stats.AllocatedLimitedCRMUsers;

                    essusersQuota.QuotaUsedValue = stats.CreatedESSCRMUsers;
                    essusersQuota.QuotaValue = stats.AllocatedESSCRMUsers;
                }

            }
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_crm_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetAccountImage(int accountTypeId)
        {
            
            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "CRMUserRoles",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID);
        }

      

        protected void odsAccountsPaged_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {

        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvUsers.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);

            // rebind grid
            gvUsers.DataBind();

        }

    }
}
