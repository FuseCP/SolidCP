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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class HostingPlansEditPlan : SolidCPModuleBase
    {
		protected bool ShouldCopyCurrentHostingPlan()
		{
			return (HttpContext.Current.Request["TargetAction"] == "Copy");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			btnDelete.Visible = (PanelRequest.PlanID > 0) && (!ShouldCopyCurrentHostingPlan());

            if (!IsPostBack)
            {
                try
                {
                    BindPlan();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PLAN_GET_PLAN", ex);
                    return;
                }
            }
        }

        private void BindServers()
        {
            ddlServer.DataSource = ES.Services.Servers.GetRawAllServers();
            ddlServer.DataBind();
            ddlServer.Items.Insert(0, new ListItem("<Select Server>", ""));
        }

        private void BindSpaces()
        {
            ddlSpace.DataSource = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUserId);
            ddlSpace.DataBind();
            ddlSpace.Items.Insert(0, new ListItem("<Select Space>", ""));
        }

        private void BindPlan()
        {
            // hide "target server" section for non-admins
            bool isUserAdmin = PanelSecurity.SelectedUser.Role == UserRole.Administrator;
            rowTargetServer.Visible = isUserAdmin;
            rowTargetSpace.Visible = !isUserAdmin;

            if(isUserAdmin)
                BindServers();
            else
                BindSpaces();

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

			if (ShouldCopyCurrentHostingPlan())
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

            Utils.SelectListItem(ddlServer, plan.ServerId);
            Utils.SelectListItem(ddlSpace, plan.PackageId);

            // bind quotas
            BindQuotas();
        }

        private void BindQuotas()
        {
            int serverId = Utils.ParseInt(ddlServer.SelectedValue, 0);
            int packageId = Utils.ParseInt(ddlSpace.SelectedValue, -1);
            hostingPlansQuotas.BindPlanQuotas(packageId, PanelRequest.PlanID, serverId);
        }

        private void SavePlan()
        {
            if (!Page.IsValid)
                return;

            // gather form info
            HostingPlanInfo plan = new HostingPlanInfo();
            plan.UserId = PanelSecurity.SelectedUserId;
            plan.PlanId = PanelRequest.PlanID;
            plan.IsAddon = false;
            plan.PlanName = txtPlanName.Text;
            plan.PlanDescription = txtPlanDescription.Text;
            plan.Available = true; // always available

            plan.SetupPrice = 0;
            plan.RecurringPrice = 0;
            plan.RecurrenceLength = 1;
            plan.RecurrenceUnit = 2; // month

            plan.PackageId = Utils.ParseInt(ddlSpace.SelectedValue, 0);
            plan.ServerId = Utils.ParseInt(ddlServer.SelectedValue, 0);
            // if this is non-admin
            // get server info from parent package
            if (PanelSecurity.EffectiveUser.Role != UserRole.Administrator)
            {
                try
                {
                    PackageInfo package = ES.Services.Packages.GetPackage(plan.PackageId);
                    if (package != null)
                        plan.ServerId = package.ServerId;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_GET_PACKAGE", ex);
                    return;
                }
            }

            plan.Groups = hostingPlansQuotas.Groups;
            plan.Quotas = hostingPlansQuotas.Quotas;

            int planId = PanelRequest.PlanID;
            if ((PanelRequest.PlanID == 0) || ShouldCopyCurrentHostingPlan())
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
                    ShowErrorMessage("PLAN_ADD_PLAN", ex);
                    return;
                }
            }
            else
            {
                // update plan
                try
                {
                    PackageResult result = ES.Services.Packages.UpdateHostingPlan(plan);
                    if (result.Result < 0)
                    {
                        ShowResultMessage(result.Result);
                        lblMessage.Text = PortalAntiXSS.Encode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PLAN_UPDATE_PLAN", ex);
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
                ShowErrorMessage("PLAN_DELETE_PLAN", ex);
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

        protected void planTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindQuotas();
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }
    }
}
