// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
// - Neither  the  name  of  SolidCP nor   the   names  of  its
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
﻿using SolidCP.EnterpriseServer.Base.HostedSolution;
﻿using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.SfB
{
    public partial class SfBUsers : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindStats();
        }

        private void BindStats()
        {            
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            int allocatedSfBUsers = stats.AllocatedSfBUsers;
            int usedUsers = stats.CreatedSfBUsers;
            usersQuota.QuotaUsedValue = usedUsers;
            usersQuota.QuotaValue = allocatedSfBUsers;

            if (stats.AllocatedSfBUsers != -1) usersQuota.QuotaAvailable = stats.AllocatedSfBUsers - stats.CreatedSfBUsers;
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_new_sfb_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetAccountImage()
        {

            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_sfb_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID);
        }



        protected void odsAccountsPaged_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {

        }

        protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                try
                {
                    ResultObject res  = ES.Services.SfB.DeleteSfBUser(PanelRequest.ItemID, Convert.ToInt32(e.CommandArgument));

                    messageBox.ShowMessage(res, "DELETE_SFB_USER", "SFB");
                    
                    // rebind grid
                    gvUsers.DataBind();
                    BindStats();
                    
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("DELETE_SFB_USERS", ex);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)   
        {   
            gvUsers.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);   
       
            // rebind grid   
            gvUsers.DataBind();   
       
            // bind stats   
            BindStats();   
        }



        public string GetOrganizationUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_user",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID,
                    "Context=User");
        }

    }
}
