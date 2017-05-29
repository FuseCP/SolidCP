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
using System.Web;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class HostingAddonsEditAddon : SolidCPModuleBase
    {
		protected bool ShouldCopyCurrentHostingAddon()
		{
			return (HttpContext.Current.Request["TargetAction"] == "Copy");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			btnDelete.Visible = (PanelRequest.PlanID > 0) && (!ShouldCopyCurrentHostingAddon());

            if (!IsPostBack)
            {
                try
                {
                    BindPlan();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ADDON_GET_ADDON", ex);
                    return;
                }
            }
        }

        private void BindPlan()
        {
            if (PanelRequest.PlanID == 0)
            {
                // new plan
                BindQuotas();
                return;
            }

            HostingPlanInfo plan = ES.Services.Packages.GetHostingPlan(PanelRequest.PlanID);
            if (plan == null)
                // plan not found
                RedirectBack();

			if (ShouldCopyCurrentHostingAddon())
			{
				plan.PlanId = 0;
				plan.PlanName = "Copy of " + plan.PlanName;
			}

            // bind plan
            txtPlanName.Text = PortalAntiXSS.DecodeOld(plan.PlanName);
            txtPlanDescription.Text = PortalAntiXSS.DecodeOld(plan.PlanDescription);
            //chkAvailable.Checked = plan.Available;

            //txtSetupPrice.Text = plan.SetupPrice.ToString("0.00");
            //txtRecurringPrice.Text = plan.RecurringPrice.ToString("0.00");
            //txtRecurrenceLength.Text = plan.RecurrenceLength.ToString();
            //Utils.SelectListItem(ddlRecurrenceUnit, plan.RecurrenceUnit);

            // bind quotas
            BindQuotas();
        }

        private void BindQuotas()
        {
            hostingPlansQuotas.BindPlanQuotas(-1, PanelRequest.PlanID, -1);
        }

        private void SavePlan()
        {
            if (!Page.IsValid)
                return;

            // gather form info
            HostingPlanInfo plan = new HostingPlanInfo();
            plan.UserId = PanelSecurity.SelectedUserId;
            plan.PlanId = PanelRequest.PlanID;
            plan.IsAddon = true;
            plan.PlanName = txtPlanName.Text;
            plan.PlanDescription = txtPlanDescription.Text;
            plan.Available = true; // always available

            plan.SetupPrice = 0;
            plan.RecurringPrice = 0;
            plan.RecurrenceLength = 1;
            plan.RecurrenceUnit = 2; // month

            plan.Groups = hostingPlansQuotas.Groups;
            plan.Quotas = hostingPlansQuotas.Quotas;

            int planId = PanelRequest.PlanID;
			if ((PanelRequest.PlanID == 0) || ShouldCopyCurrentHostingAddon())
            {
                // new plan
                try
                {
                    planId = ES.Services.Packages.AddHostingPlan(plan);
                    if (planId < 0)
                    {
                        ShowResultMessage(planId);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ADDON_ADD_ADDON", ex);
                    return;
                }
            }
            else
            {
                // update plan
                try
                {
                    PackageResult result = ES.Services.Packages.UpdateHostingPlan(plan);
                    lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                    if (result.Result < 0)
                    {
                        ShowResultMessage(result.Result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ADDON_UPDATE_ADDON", ex);
                    return;
                }
            }

            // redirect
            RedirectBack();
        }

        private void DeletePlan()
        {
            try
            {
                int result = ES.Services.Packages.DeleteHostingPlan(PanelRequest.PlanID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("ADDON_DELETE_ADDON", ex);
                return;
            }

            // redirect
            RedirectBack();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePlan();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeletePlan();
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }
    }
}
