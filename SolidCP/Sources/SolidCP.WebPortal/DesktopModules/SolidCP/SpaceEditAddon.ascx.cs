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
    public partial class SpaceEditAddon : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.PackageAddonID != 0);

            if (!IsPostBack)
            {
                try
                {
                    BindPackageAddon();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_GET_ADDON", ex);
                    return;
                }
            }
        }

        private void BindAddons(int userId)
        {
            HostingPlanInfo[] hpi = ES.Services.Packages.GetUserAvailableHostingAddons(userId);

            // Next code is user for decoding incorectly stored plan names and descriptions with pre 1.2.2 installations
            for (int i = 0; i < hpi.Length; i++)
            {
                hpi[i].PlanDescription = PortalAntiXSS.DecodeOld(hpi[i].PlanDescription);
                hpi[i].PlanName = PortalAntiXSS.DecodeOld(hpi[i].PlanName);
            }

            ddlPlan.DataSource = hpi;
            ddlPlan.DataBind();

            ddlPlan.Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), ""));
        }

        private void BindPackageAddon()
        {
            try
            {
                int packageId = PanelSecurity.PackageId;

                PackageInfo package = null;
                PackageAddonInfo addon = null;

                if (PanelRequest.PackageAddonID != 0)
                {
                    // load package addon
                    addon = ES.Services.Packages.GetPackageAddon(PanelRequest.PackageAddonID);

                    if (addon == null)
                        // package not found
                        RedirectBack();

                    packageId = addon.PackageId;
                }

                // load addon package
                package = ES.Services.Packages.GetPackage(packageId);
                if (package == null)
                    RedirectBack();

                // bind addons list
                BindAddons(package.UserId);

                // init other fields
                PurchaseDate.SelectedDate = DateTime.Now;

                if (PanelRequest.PackageAddonID == 0)
                    return;

                Utils.SelectListItem(ddlPlan, addon.PlanId);

                txtComments.Text = addon.Comments;

                PurchaseDate.SelectedDate = addon.PurchaseDate;
                Utils.SelectListItem(ddlPlan, addon.PlanId);
                txtQuantity.Text = addon.Quantity.ToString();
                Utils.SelectListItem(ddlStatus, addon.StatusId);

            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_GET_ADDON", ex);
                return;
            }
        }

        private void SaveAddon()
        {
            if (!Page.IsValid)
                return;

            // gather form data
            PackageAddonInfo addon = new PackageAddonInfo();
            addon.PackageAddonId = PanelRequest.PackageAddonID;
            addon.PackageId = PanelSecurity.PackageId;
            addon.Comments = txtComments.Text;
            addon.PlanId = Utils.ParseInt(ddlPlan.SelectedValue, 0);
            addon.StatusId = Utils.ParseInt(ddlStatus.SelectedValue, 0);
            addon.PurchaseDate = PurchaseDate.SelectedDate;
            addon.Quantity = Utils.ParseInt(txtQuantity.Text, 1);

            if (PanelRequest.PackageAddonID == 0)
            {
                // add a new package addon
                try
                {
                    PackageResult result = ES.Services.Packages.AddPackageAddon(addon);
                    if (result.Result < 0)
                    {
                        ShowResultMessage(result.Result);
                        lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_ADD_ADDON", ex);
                    return;
                }
            }
            else
            {
                // update existing package addon
                try
                {
                    PackageResult result = ES.Services.Packages.UpdatePackageAddon(addon);
                    if (result.Result < 0)
                    {
                        ShowResultMessage(result.Result);
                        lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_UPDATE_ADDON", ex);
                    return;
                }
            }

            RedirectBack();
        }

        private void DeleteAddon()
        {
            try
            {
                int result = ES.Services.Packages.DeletePackageAddon(PanelRequest.PackageAddonID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_DELETE_ADDON", ex);
                return;
            }

            RedirectBack();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveAddon();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteAddon();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit_details"));
        }
    }
}
