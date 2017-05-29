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
using SolidCP.Providers.ResultObjects;
using SolidCP.EnterpriseServer;

using SolidCP.Providers.HostedSolution;

using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace SolidCP.Portal.Lync
{
    public partial class CreateLyncUser : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SolidCP.Providers.HostedSolution.LyncUserPlan[] plans = ES.Services.Lync.GetLyncUserPlans(PanelRequest.ItemID);

                BindPhoneNumbers();

                if (plans.Length == 0)
                    btnCreate.Enabled = false;
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
                    ddlPhoneNumber.Items.Add(new ListItem(phone, ip.PackageAddressID.ToString()));
                }
            }

        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.LYNC_ENTERPRISEVOICE, cntx);

            bool enterpriseVoice = false;

            SolidCP.Providers.HostedSolution.LyncUserPlan plan = planSelector.plan;
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
                string[] pinPolicy = ES.Services.Lync.GetPolicyList(PanelRequest.ItemID, LyncPolicyType.Pin, "MinPasswordLength");
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int accountId = userSelector.GetAccountId();
            LyncUserResult res = ES.Services.Lync.CreateLyncUser(PanelRequest.ItemID, accountId, Convert.ToInt32(planSelector.planId));
            if (res.IsSuccess && res.ErrorCodes.Count == 0)
            {

                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.LYNC_ENTERPRISEVOICE, cntx);

                string lineUri = "";
                if ((enterpriseVoiceQuota) & (ddlPhoneNumber.Items.Count != 0)) lineUri = ddlPhoneNumber.SelectedItem.Text + ":" + tbPin.Text;

                //#1
                LyncUser lyncUser = ES.Services.Lync.GetLyncUserGeneralSettings(PanelRequest.ItemID, accountId);
                ES.Services.Lync.SetLyncUserGeneralSettings(PanelRequest.ItemID, accountId, lyncUser.SipAddress, lineUri);

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "edit_lync_user",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID));
            }
            else
            {
                messageBox.ShowMessage(res, "CREATE_LYNC_USER", "LYNC");
            }
        }
    }
}
