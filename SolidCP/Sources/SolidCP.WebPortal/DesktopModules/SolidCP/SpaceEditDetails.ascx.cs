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
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class SpaceEditDetails : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSpace();
                BindSpaceAddons();
                BindRoles(PanelSecurity.EffectiveUserId);
            }
        }

        private void BindRoles(int userId)
        {
            // load selected user
            UserInfo user = UsersHelper.GetUser(userId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) | 
                    (PanelSecurity.LoggedUser.Role == UserRole.ResellerCSR) |
                    (PanelSecurity.LoggedUser.Role == UserRole.ResellerHelpdesk) | 
                    (PanelSecurity.LoggedUser.Role == UserRole.PlatformCSR) |
                    (PanelSecurity.LoggedUser.Role == UserRole.PlatformHelpdesk))
                    this.rbPackageQuotas.Enabled = this.rbPlanQuotas.Enabled = false;
            }
        }


        private void BindSpace()
        {
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package != null)
            {
                // bind plans
                BindHostingPlans();

                // bind space
                txtName.Text = PortalAntiXSS.DecodeOld(package.PackageName);
                txtComments.Text = PortalAntiXSS.DecodeOld(package.PackageComments);
                PurchaseDate.SelectedDate = package.PurchaseDate;
                serverDetails.ServerId = package.ServerId;
                Utils.SelectListItem(ddlPlan, package.PlanId);

                // bind quotas
                packageQuotas.BindQuotas(PanelRequest.PackageID);

                // bind override flag
                rbPlanQuotas.Checked = !package.OverrideQuotas;
                rbPackageQuotas.Checked = package.OverrideQuotas;

                // toggle quotas editor
                ToggleQuotasEditor();
            }
        }

        private void BindHostingPlans()
        {
            ddlPlan.DataSource = ES.Services.Packages.GetUserAvailableHostingPlans(PanelSecurity.SelectedUserId);
            ddlPlan.DataBind();

            ddlPlan.Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), ""));
        }

        private void BindSpaceAddons()
        {
            gvAddons.DataSource = ES.Services.Packages.GetPackageAddons(PanelSecurity.PackageId);
            gvAddons.DataBind();
        }

        private void ToggleQuotasEditor()
        {
            packageQuotas.Visible = rbPlanQuotas.Checked;
            editPackageQuotas.Visible = rbPackageQuotas.Checked;

            // bind quotas editor if required
            if (rbPackageQuotas.Checked)
                editPackageQuotas.BindPackageQuotas(PanelSecurity.PackageId);
            else
                packageQuotas.BindQuotas(PanelSecurity.PackageId);
        }

        private void SaveSpace()
        {
            if (!Page.IsValid)
                return;

            // gather form data
            PackageInfo package = new PackageInfo();

            // load package for update
            if (PanelSecurity.PackageId > 0)
                package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

            package.PackageId = PanelSecurity.PackageId;
            package.PackageName = txtName.Text;
            package.PackageComments = txtComments.Text;
            package.PlanId = Utils.ParseInt(ddlPlan.SelectedValue, 0);
            package.PurchaseDate = PurchaseDate.SelectedDate;

            package.OverrideQuotas = rbPackageQuotas.Checked;
            if (package.OverrideQuotas)
            {
                package.Groups = editPackageQuotas.Groups;
                package.Quotas = editPackageQuotas.Quotas;
            }

            try
            {
                // update existing package
                PackageResult result = ES.Services.Packages.UpdatePackage(package);
                if (result.Result < 0)
                {
                    ShowResultMessage(result.Result);
                    lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_UPDATE_PACKAGE", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSpace();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void rbPlanQuotas_CheckedChanged(object sender, EventArgs e)
        {
            ToggleQuotasEditor();
        }

        protected void btnAddAddon_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit_addon"));
        }
    }
}
