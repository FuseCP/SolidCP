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
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.CRM
{
    public partial class CRMUserRoles : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                {
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseProfessional"), CRMUserLycenseTypes.PROFESSIONAL.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseBasic"), CRMUserLycenseTypes.BASIC.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseEssential"), CRMUserLycenseTypes.ESSENTIAL.ToString()));
                }
                else
                {
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseFull"), CRMUserLycenseTypes.FULL.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseLimited"), CRMUserLycenseTypes.LIMITED.ToString()));
                    ddlLicenseType.Items.Add(new System.Web.UI.WebControls.ListItem(
                        GetSharedLocalizedString("HostedCRM.LicenseESS"), CRMUserLycenseTypes.ESS.ToString()));
                }


                try
                {
                    OrganizationUser user =
                        ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                    CrmUserResult userResult = ES.Services.CRM.GetCrmUser(PanelRequest.ItemID, PanelRequest.AccountID);

                    if (userResult.IsSuccess)
                    {
                        btnActive.Visible = userResult.Value.IsDisabled;
                        locEnabled.Visible = !userResult.Value.IsDisabled;

                        btnDeactivate.Visible = !userResult.Value.IsDisabled;
                        locDisabled.Visible = userResult.Value.IsDisabled;
                        lblDisplayName.Text = user.DisplayName;
                        lblEmailAddress.Text = user.PrimaryEmailAddress;
                        lblDomainName.Text = user.DomainUserName;

                        int cALType = userResult.Value.CALType + ((int)userResult.Value.ClientAccessMode) * 10;

                        Utils.SelectListItem(ddlLicenseType, cALType);
                    }
                    else
                    {
                        messageBox.ShowMessage(userResult, "GET_CRM_USER", "HostedCRM");
                        return;
                    }
                    
                    CrmRolesResult res =
                        ES.Services.CRM.GetCrmRoles(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId);

                    if (res.IsSuccess)
                    {
                        gvRoles.DataSource = res.Value;
                        gvRoles.DataBind();
                    }
                    else
                    {
                        messageBox.ShowMessage(res, "GET_CRM_USER_ROLES", "HostedCRM");
                    }
                }
                catch(Exception ex)
                {
                    messageBox.ShowErrorMessage("GET_CRM_USER_ROLES", ex);
                }
            }
        }

        protected bool SaveSettings()
        {
            try
            {
                List<Guid> roles = new List<Guid>();
                foreach (GridViewRow row in gvRoles.Rows)
                {
                    CheckBox cbSelected = row.FindControl("cbSelected") as CheckBox;
                    string str = gvRoles.DataKeys[row.DataItemIndex].Value.ToString();
                    if (cbSelected != null && cbSelected.Checked)
                        roles.Add(new Guid(str));
                }


                ResultObject res =
                    ES.Services.CRM.SetUserRoles(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId,
                                                 roles.ToArray());


                int CALType = 0;
                int.TryParse(ddlLicenseType.SelectedValue, out CALType);

                ResultObject res2 =
                    ES.Services.CRM.SetUserCALType(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId,
                                                CALType);

                if (!res2.IsSuccess)
                    messageBox.ShowMessage(res2, "UPDATE_CRM_USER_ROLES", "HostedCRM");
                else if (!res.IsSuccess)
                    messageBox.ShowMessage(res, "UPDATE_CRM_USER_ROLES", "HostedCRM");
                else
                    messageBox.ShowMessage(res, "UPDATE_CRM_USER_ROLES", "HostedCRM");

                return res.IsSuccess && res2.IsSuccess;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("UPDATE_CRM_USER_ROLES", ex);
                return false;
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                    "CRMUsers",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
        }


        private void ActivateUser()
        {
            ResultObject res = ES.Services.CRM.ChangeUserState(PanelRequest.ItemID, PanelRequest.AccountID, false);
            messageBox.ShowMessage(res, "CHANGE_USER_STATE", "HostedCRM");
            locDisabled.Visible = false;
            btnDeactivate.Visible = true;
            btnActive.Visible = false;
            locEnabled.Visible = true;
        }

        private void DeactivateUser()
        {
            ResultObject res = ES.Services.CRM.ChangeUserState(PanelRequest.ItemID, PanelRequest.AccountID, true);
            messageBox.ShowMessage(res, "CHANGE_USER_STATE", "HostedCRM");
            locDisabled.Visible = true;
            btnDeactivate.Visible = false;
            btnActive.Visible = true;
            locEnabled.Visible = false;
        }
        
        
        protected void btnActive_Click(object sender, EventArgs e)
        {
            ActivateUser();
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            DeactivateUser();
        }


        
    }
}
