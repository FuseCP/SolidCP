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
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
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
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.HostedSolution;

using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace SolidCP.Portal.SfB
{
    public partial class EditSfBUser : SolidCPModuleBase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPhoneNumbers();
                BindItems();
            }
        }

        private void BindPhoneNumbers()
        {
            PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, PanelRequest.ItemID, IPAddressPool.PhoneNumbers);

            if (ips.Length > 0)
            {
                ddlPhoneNumber.Items.Add(new ListItem("<Select Phone>", ""));

                foreach (PackageIPAddress ip in ips)
                {
                    string phone = ip.ExternalIP;
                    ddlPhoneNumber.Items.Add(new ListItem(phone, phone));
                }
            }

        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.SFB_ENTERPRISEVOICE, cntx);

            bool enterpriseVoice = false;

            SolidCP.Providers.HostedSolution.SfBUserPlan plan = planSelector.plan;
            if (plan != null)
                enterpriseVoice = plan.EnterpriseVoice && enterpriseVoiceQuota && (ddlPhoneNumber.Items.Count > 0);

            pnEnterpriseVoice.Visible = enterpriseVoice;

            if (!enterpriseVoice)
            {
                ddlPhoneNumber.Text = "";
                tbPin.Text = "";
            }

            if (enterpriseVoice)
            {
                string[] pinPolicy = ES.Services.SfB.GetPolicyList(PanelRequest.ItemID, SfBPolicyType.Pin, "MinPasswordLength");
                if (pinPolicy != null)
                {
                    if (pinPolicy.Length > 0)
                    {
                        int MinPasswordLength = -1;
                        if (int.TryParse(pinPolicy[0], out MinPasswordLength))
                        {
                            PinRegularExpressionValidator.ValidationExpression = "^([0-9]){" + MinPasswordLength.ToString() + ",}$";
                            PinRegularExpressionValidator.ErrorMessage = "Must contain only numbers. Min. length " + MinPasswordLength.ToString();
                        }
                    }
                }
            }

        }

        private void BindItems()
        {
            // get settings
            SfBUser sfbUser = ES.Services.SfB.GetSfBUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

            // title
            litDisplayName.Text = sfbUser.DisplayName;

            planSelector.planId = sfbUser.SfBUserPlanId.ToString();
            sfbUserSettings.sipAddress = sfbUser.SipAddress;

            Utils.SelectListItem(ddlPhoneNumber, sfbUser.LineUri);

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

            if (user.LevelId > 0 && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels))
            {
                SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(user.LevelId);

                litServiceLevel.Visible = true;
                litServiceLevel.Text = serviceLevel.LevelName;
                litServiceLevel.ToolTip = serviceLevel.LevelDescription;

            }
            imgVipUser.Visible = user.IsVIP && cntx.Groups.ContainsKey(ResourceGroups.ServiceLevels);
        }

        protected bool SaveSettings()
        {
            if (!Page.IsValid)
                return false;
            try
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.SFB_ENTERPRISEVOICE, cntx);

                string lineUri = "";
                if ((enterpriseVoiceQuota) & (ddlPhoneNumber.Items.Count != 0)) lineUri = ddlPhoneNumber.SelectedItem.Text + ":" + tbPin.Text;

                SfBUserResult res = ES.Services.SfB.SetUserSfBPlan(PanelRequest.ItemID, PanelRequest.AccountID, Convert.ToInt32(planSelector.planId));
                if (res.IsSuccess && res.ErrorCodes.Count == 0)
                {
                    res = ES.Services.SfB.SetSfBUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID, sfbUserSettings.sipAddress, lineUri);
                }

                if (res.IsSuccess && res.ErrorCodes.Count == 0)
                {
                    messageBox.ShowSuccessMessage("UPDATE_SFB_USER");
                    return true;
                }
                else
                {
                    messageBox.ShowMessage(res, "UPDATE_SFB_USER", "SFB");
                    return false;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("UPDATE_SFB_USER", ex);
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                    "sfb_users",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
        }


    }
}
