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
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.Common;
using SolidCP.Portal.Code.Helpers;


namespace SolidCP.Portal.SfB
{
    public partial class SfBAddSfBUserPlan : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                string[] archivePolicy = ES.Services.SfB.GetPolicyList(PanelRequest.ItemID, SfBPolicyType.Archiving, null);
                if (archivePolicy != null)
                {
                    foreach (string policy in archivePolicy)
                    {
                        if (policy.ToLower()=="global") continue;
                        string txt = policy.Replace("Tag:","");
                        ddArchivingPolicy.Items.Add( new System.Web.UI.WebControls.ListItem( txt, policy) );
                    }
                }

                if (PanelRequest.GetInt("SfBUserPlanId") != 0)
                {
                    Providers.HostedSolution.SfBUserPlan plan = ES.Services.SfB.GetSfBUserPlan(PanelRequest.ItemID, PanelRequest.GetInt("SfBUserPlanId"));

                    txtPlan.Text = plan.SfBUserPlanName;
                    chkIM.Checked = plan.IM;
                    chkIM.Enabled = false;
                    chkFederation.Checked = plan.Federation;
                    chkConferencing.Checked = plan.Conferencing;
                    chkMobility.Checked = plan.Mobility;
                    chkEnterpriseVoice.Checked = plan.EnterpriseVoice;

                    /* because not used
                    switch (plan.VoicePolicy)
                    {
                        case SfBVoicePolicyType.None:
                            break;
                        case SfBVoicePolicyType.Emergency:
                            chkEmergency.Checked = true;
                            break;
                        case SfBVoicePolicyType.National:
                            chkNational.Checked = true;
                            break;
                        case SfBVoicePolicyType.Mobile:
                            chkMobile.Checked = true;
                            break;
                        case SfBVoicePolicyType.International:
                            chkInternational.Checked = true;
                            break;
                        default:
                            chkNone.Checked = true;
                            break;
                    }
                     */

	                chkRemoteUserAccess.Checked = plan.RemoteUserAccess;
                    chkAllowOrganizeMeetingsWithExternalAnonymous.Checked = plan.AllowOrganizeMeetingsWithExternalAnonymous;

                    Utils.SelectListItem(ddTelephony, plan.Telephony);

	                tbServerURI.Text = plan.ServerURI;

                    string planArchivePolicy = "";              if (plan.ArchivePolicy != null) planArchivePolicy = plan.ArchivePolicy;
                    string planTelephonyDialPlanPolicy = "";    if (plan.TelephonyDialPlanPolicy != null) planTelephonyDialPlanPolicy = plan.TelephonyDialPlanPolicy;
                    string planTelephonyVoicePolicy = "";       if (plan.TelephonyVoicePolicy != null) planTelephonyVoicePolicy = plan.TelephonyVoicePolicy;

                    ddArchivingPolicy.Items.Clear();
                    ddArchivingPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planArchivePolicy.Replace("Tag:", ""), planArchivePolicy));
                    ddTelephonyDialPlanPolicy.Items.Clear();
                    ddTelephonyDialPlanPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planTelephonyDialPlanPolicy.Replace("Tag:", ""), planTelephonyDialPlanPolicy));
                    ddTelephonyVoicePolicy.Items.Clear();
                    ddTelephonyVoicePolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planTelephonyVoicePolicy.Replace("Tag:", ""), planTelephonyVoicePolicy));

                    locTitle.Text = plan.SfBUserPlanName;
                    this.DisableControls = true;
                }
                else
                {
                    chkIM.Checked = true;
                    chkIM.Enabled = false;

                    // chkNone.Checked = true; because not used

                    if (cntx != null)
                    {
                        foreach (QuotaValueInfo quota in cntx.QuotasArray)
                        {
                            switch (quota.QuotaId)
                            {
                                case 371:
                                    chkFederation.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkFederation.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                                case 372:
                                    chkConferencing.Checked = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    chkConferencing.Enabled = Convert.ToBoolean(quota.QuotaAllocatedValue);
                                    break;
                            }
                        }
                    }
                    else
                        this.DisableControls = true;
                }
            }

            bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.SFB_ENTERPRISEVOICE, cntx);

            PlanFeaturesTelephony.Visible = enterpriseVoiceQuota;
            secPlanFeaturesTelephony.Visible = enterpriseVoiceQuota;
            if (!enterpriseVoiceQuota) Utils.SelectListItem(ddTelephony, "0");

            bool enterpriseVoice = enterpriseVoiceQuota && (ddTelephony.SelectedValue == "2");

            chkEnterpriseVoice.Enabled = false;
            chkEnterpriseVoice.Checked = enterpriseVoice;
            pnEnterpriseVoice.Visible = enterpriseVoice;

            switch (ddTelephony.SelectedValue)
            {
                case "3":
                case "4":
                    pnServerURI.Visible = true;
                    break;
                default:
                    pnServerURI.Visible = false;
                    break;
            }


        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AddPlan();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            string name = tbTelephoneProvider.Text;

            if (string.IsNullOrEmpty(name)) return;

            ddTelephonyDialPlanPolicy.Items.Clear();
            string[] dialPlan = ES.Services.SfB.GetPolicyList(PanelRequest.ItemID, SfBPolicyType.DialPlan, name);
            if (dialPlan != null)
            {
                foreach (string policy in dialPlan)
                {
                    if (policy.ToLower() == "global") continue;
                    string txt = policy.Replace("Tag:", "");
                    ddTelephonyDialPlanPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(txt, policy));
                }
            }

            ddTelephonyVoicePolicy.Items.Clear();
            string[] voicePolicy = ES.Services.SfB.GetPolicyList(PanelRequest.ItemID, SfBPolicyType.Voice, name);
            if (voicePolicy != null)
            {
                foreach (string policy in voicePolicy)
                {
                    if (policy.ToLower() == "global") continue;
                    string txt = policy.Replace("Tag:", "");
                    ddTelephonyVoicePolicy.Items.Add(new System.Web.UI.WebControls.ListItem(txt, policy));
                }
            }
        }

        private void AddPlan()
        {
            try
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                Providers.HostedSolution.SfBUserPlan plan = new Providers.HostedSolution.SfBUserPlan();
                plan.SfBUserPlanName = txtPlan.Text;
                plan.IsDefault = false;

                plan.IM = true;
                plan.Mobility = chkMobility.Checked;
                plan.Federation = chkFederation.Checked;
                plan.Conferencing = chkConferencing.Checked;

                bool enterpriseVoiceQuota = Utils.CheckQouta(Quotas.SFB_ENTERPRISEVOICE, cntx);
                bool enterpriseVoice = enterpriseVoiceQuota && (ddTelephony.SelectedValue == "2");

                plan.EnterpriseVoice = enterpriseVoice;

                plan.VoicePolicy = SfBVoicePolicyType.None;

                /* because not used
                if (!plan.EnterpriseVoice)
                {
                    plan.VoicePolicy = SfBVoicePolicyType.None;
                }
                else
                {
                    if (chkEmergency.Checked)
                        plan.VoicePolicy = SfBVoicePolicyType.Emergency;
                    else if (chkNational.Checked)
                        plan.VoicePolicy = SfBVoicePolicyType.National;
                    else if (chkMobile.Checked)
                        plan.VoicePolicy = SfBVoicePolicyType.Mobile;
                    else if (chkInternational.Checked)
                        plan.VoicePolicy = SfBVoicePolicyType.International;
                    else
                        plan.VoicePolicy = SfBVoicePolicyType.None;

                } 
                */

	            plan.RemoteUserAccess = chkRemoteUserAccess.Checked;

	            plan.AllowOrganizeMeetingsWithExternalAnonymous = chkAllowOrganizeMeetingsWithExternalAnonymous.Checked;

                int telephonyId = -1;
                int.TryParse(ddTelephony.SelectedValue, out telephonyId);
	            plan.Telephony = telephonyId;

	            plan.ServerURI = tbServerURI.Text;

                plan.ArchivePolicy = ddArchivingPolicy.SelectedValue;
                plan.TelephonyDialPlanPolicy = ddTelephonyDialPlanPolicy.SelectedValue;
                plan.TelephonyVoicePolicy = ddTelephonyVoicePolicy.SelectedValue;

                int result = ES.Services.SfB.AddSfBUserPlan(PanelRequest.ItemID,
                                                                                plan);


                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    messageBox.ShowErrorMessage("SFB_UNABLE_TO_ADD_PLAN");
                    return;
                }

                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "sfb_userplans",
                    "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("SFB_ADD_PLAN", ex);
            }
        }
    }
}
