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
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal
{
    public partial class SettingsLyncUserPlansPolicy : SolidCPControlBase, IUserSettingsEditorControl
    {

        internal static List<LyncUserPlan> list; // compiler indicates is never used, but it is

        protected void ddArchivingPolicyUpdate()
        {
            string[] archivePolicy = ES.Services.Lync.GetPolicyList(-1, LyncPolicyType.Archiving, null);
            if (archivePolicy != null)
            {
                foreach (string policy in archivePolicy)
                {
                    if (policy.ToLower() == "global") continue;
                    string txt = policy.Replace("Tag:", "");
                    if (ddArchivingPolicy.Items.FindByValue(policy)==null)
                        ddArchivingPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(txt, policy));
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            if (ddArchivingPolicy.Items.Count == 0)
                ddArchivingPolicyUpdate();

            chkEnterpriseVoice.Enabled = false;
            chkEnterpriseVoice.Checked = false;

            pnEnterpriseVoice.Visible = false;
            pnServerURI.Visible = false;

            switch (ddTelephony.SelectedIndex)
            {
                case 1:
                    break;
                case 2:
                    pnEnterpriseVoice.Visible = true;
                    chkEnterpriseVoice.Checked = true;
                    break;
                case 3:
                    pnServerURI.Visible = true;
                    break;
                case 4:
                    pnServerURI.Visible = true;
                    break;

            }

        }


        public void BindSettings(UserSettings settings)
        {
            BindPlans();

            txtStatus.Visible = false;
        }


        private void BindPlans()
        {
            Providers.HostedSolution.Organization[] orgs = null;

            if (PanelSecurity.SelectedUserId != 1)
            {
                PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                if ((Packages != null) & (Packages.GetLength(0) > 0))
                {
                    orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                }
            }
            else
            {
                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
            }

            if ((orgs != null) & (orgs.GetLength(0) > 0))
            {
                LyncUserPlan[] list = ES.Services.Lync.GetLyncUserPlans(orgs[0].Id);

                gvPlans.DataSource = list;
                gvPlans.DataBind();
            }

            btnUpdatePlan.Enabled = (string.IsNullOrEmpty(txtPlan.Text)) ? false : true;
        }



        public string IsChecked(bool val)
        {
            return val ? "checked" : "";
        }


        public void btnAddPlan_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (list != null)
            {
                foreach (LyncUserPlan p in list)
                {
                    p.LyncUserPlanId = count;
                    count++;
                }
            }

            Providers.HostedSolution.LyncUserPlan plan = new Providers.HostedSolution.LyncUserPlan();
            plan.LyncUserPlanName = txtPlan.Text;
            plan.IsDefault = false;

            plan.IM = true;
            plan.Mobility = chkMobility.Checked;
            plan.Federation = chkFederation.Checked;
            plan.Conferencing = chkConferencing.Checked;

            plan.EnterpriseVoice = chkEnterpriseVoice.Checked;

            plan.VoicePolicy = LyncVoicePolicyType.None;

            plan.RemoteUserAccess = chkRemoteUserAccess.Checked;

            plan.AllowOrganizeMeetingsWithExternalAnonymous = chkAllowOrganizeMeetingsWithExternalAnonymous.Checked;

            plan.Telephony = ddTelephony.SelectedIndex;

            plan.ServerURI = tbServerURI.Text;

            plan.ArchivePolicy = ddArchivingPolicy.SelectedValue;
            plan.TelephonyDialPlanPolicy = ddTelephonyDialPlanPolicy.SelectedValue;
            plan.TelephonyVoicePolicy = ddTelephonyVoicePolicy.SelectedValue;
            
            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator)
                plan.LyncUserPlanType = (int)LyncUserPlanType.Administrator;
            else
                if (PanelSecurity.SelectedUser.Role == UserRole.Reseller)
                    plan.LyncUserPlanType = (int)LyncUserPlanType.Reseller;


            Providers.HostedSolution.Organization[] orgs = null;

            if (PanelSecurity.SelectedUserId != 1)
            {
                PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                if ((Packages != null) & (Packages.GetLength(0) > 0))
                {
                    orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                }
            }
            else
            {
                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
            }


            if ((orgs != null) & (orgs.GetLength(0) > 0))
            {
                int result = ES.Services.Lync.AddLyncUserPlan(orgs[0].Id, plan);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }
            }

            BindPlans();
        }

        protected void gvPlan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int planId = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            Providers.HostedSolution.Organization[] orgs = null;
            Providers.HostedSolution.LyncUserPlan plan;
            int result = 0;


            switch (e.CommandName)
            {
                case "DeleteItem":
                    try
                    {

                        if (PanelSecurity.SelectedUserId != 1)
                        {
                            PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                            if ((Packages != null) & (Packages.GetLength(0) > 0))
                            {
                                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                            }
                        }
                        else
                        {
                            orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
                        }

                        plan = ES.Services.Lync.GetLyncUserPlan(orgs[0].Id, planId);

                        if (plan.ItemId != orgs[0].Id)
                        {
                            messageBox.ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                            BindPlans();
                            return;
                        }


                        result = ES.Services.Lync.DeleteLyncUserPlan(orgs[0].Id, planId);
                        if (result < 0)
                        {
                            messageBox.ShowResultMessage(result);
                            return;
                        }
                        ViewState["LyncUserPlanID"] = null; 

                        txtPlan.Text = string.Empty;

                        btnUpdatePlan.Enabled = (string.IsNullOrEmpty(txtPlan.Text)) ? false : true;

                    }
                    catch (Exception)
                    {
                        messageBox.ShowErrorMessage("LYNC_DELETE_PLAN");
                    }

                    BindPlans();

                    break;

                case "EditItem":
                    try
                    {

                        ViewState["LyncUserPlanID"] = planId;

                        if (PanelSecurity.SelectedUserId != 1)
                        {
                            PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                            if ((Packages != null) & (Packages.GetLength(0) > 0))
                            {
                                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                            }
                        }
                        else
                        {
                            orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
                        }

                        plan = ES.Services.Lync.GetLyncUserPlan(orgs[0].Id, planId);

                        txtPlan.Text = plan.LyncUserPlanName;
                        chkIM.Checked = plan.IM;
                        chkIM.Enabled = false;
                        chkFederation.Checked = plan.Federation;
                        chkConferencing.Checked = plan.Conferencing;
                        chkMobility.Checked = plan.Mobility;
                        chkEnterpriseVoice.Checked = plan.EnterpriseVoice;

                        chkRemoteUserAccess.Checked = plan.RemoteUserAccess;

                        chkAllowOrganizeMeetingsWithExternalAnonymous.Checked = plan.AllowOrganizeMeetingsWithExternalAnonymous;
                        ddTelephony.SelectedIndex = plan.Telephony;

                        tbServerURI.Text = plan.ServerURI;

                        string planArchivePolicy = "";
                        if (plan.ArchivePolicy != null) planArchivePolicy = plan.ArchivePolicy;
                        string planTelephonyDialPlanPolicy = "";
                        if (plan.TelephonyDialPlanPolicy != null) planTelephonyDialPlanPolicy = plan.TelephonyDialPlanPolicy;
                        string planTelephonyVoicePolicy = "";
                        if (plan.TelephonyVoicePolicy != null) planTelephonyVoicePolicy = plan.TelephonyVoicePolicy;

                        ddArchivingPolicyUpdate();
                        ListItem li = ddArchivingPolicy.Items.FindByValue(planArchivePolicy);
                        if (li == null)
                        {
                            li = new System.Web.UI.WebControls.ListItem(planArchivePolicy.Replace("Tag:", ""), planArchivePolicy);
                            ddArchivingPolicy.Items.Add(li);
                        }
                        ddArchivingPolicy.SelectedIndex = ddArchivingPolicy.Items.IndexOf(li);
                        
                        ddTelephonyDialPlanPolicy.Items.Clear();
                        ddTelephonyDialPlanPolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planTelephonyDialPlanPolicy.Replace("Tag:", ""), planTelephonyDialPlanPolicy));
                        ddTelephonyVoicePolicy.Items.Clear();
                        ddTelephonyVoicePolicy.Items.Add(new System.Web.UI.WebControls.ListItem(planTelephonyVoicePolicy.Replace("Tag:", ""), planTelephonyVoicePolicy));

                        btnUpdatePlan.Enabled  = (string.IsNullOrEmpty(txtPlan.Text)) ? false : true;

                        break;
                    }
                    catch (Exception)
                    {
                    }

                    BindPlans();

                    break;
                case "RestampItem":
                    RestampLyncUsers(planId, planId);
                    break;
            }
        }


        public string GetPlanType(int planType)
        {
            string imgName = string.Empty;

            LyncUserPlanType type = (LyncUserPlanType)planType;
            switch (type)
            {
                case LyncUserPlanType.Reseller:
                    imgName = "company24.png";
                    break;
                case LyncUserPlanType.Administrator:
                    imgName = "company24.png";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }


        public void SaveSettings(UserSettings settings)
        {
            settings["LyncUserPlansPolicy"] = "";
        }


        protected void btnUpdatePlan_Click(object sender, EventArgs e)
        {

            if (ViewState["LyncUserPlanID"] == null)
                return;

            int planId = (int)ViewState["LyncUserPlanID"];
            Providers.HostedSolution.Organization[] orgs = null;
            Providers.HostedSolution.LyncUserPlan plan;


            if (PanelSecurity.SelectedUserId != 1)
            {
                PackageInfo[] Packages = ES.Services.Packages.GetPackages(PanelSecurity.SelectedUserId);

                if ((Packages != null) & (Packages.GetLength(0) > 0))
                {
                    orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Packages[0].PackageId, false);
                }
            }
            else
            {
                orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(1, false);
            }

            plan = ES.Services.Lync.GetLyncUserPlan(orgs[0].Id, planId);

            if (plan.ItemId != orgs[0].Id)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                BindPlans();
                return;
            }

            plan = new Providers.HostedSolution.LyncUserPlan();
            plan.LyncUserPlanId = (int)ViewState["LyncUserPlanID"];
            plan.LyncUserPlanName = txtPlan.Text;
            plan.IsDefault = false;

            plan.IM = true;
            plan.Mobility = chkMobility.Checked;
            plan.Federation = chkFederation.Checked;
            plan.Conferencing = chkConferencing.Checked;

            plan.EnterpriseVoice = chkEnterpriseVoice.Checked;

            plan.VoicePolicy = LyncVoicePolicyType.None;

            plan.RemoteUserAccess = chkRemoteUserAccess.Checked;

            plan.AllowOrganizeMeetingsWithExternalAnonymous = chkAllowOrganizeMeetingsWithExternalAnonymous.Checked;

            plan.Telephony = ddTelephony.SelectedIndex;

            plan.ServerURI = tbServerURI.Text;

            plan.ArchivePolicy = ddArchivingPolicy.SelectedValue;
            plan.TelephonyDialPlanPolicy = ddTelephonyDialPlanPolicy.SelectedValue;
            plan.TelephonyVoicePolicy = ddTelephonyVoicePolicy.SelectedValue;

            
            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator)
                plan.LyncUserPlanType = (int)LyncUserPlanType.Administrator;
            else
                if (PanelSecurity.SelectedUser.Role == UserRole.Reseller)
                    plan.LyncUserPlanType = (int)LyncUserPlanType.Reseller;


            if ((orgs != null) & (orgs.GetLength(0) > 0))
            {
                int result = ES.Services.Lync.UpdateLyncUserPlan(orgs[0].Id, plan);

                if (result < 0)
                {
                    messageBox.ShowErrorMessage("LYNC_UPDATEPLANS");
                }
                else
                {
                    messageBox.ShowSuccessMessage("LYNC_UPDATEPLANS");
                }
            }

            BindPlans();
        }


        private bool PlanExists(LyncUserPlan plan, LyncUserPlan[] plans)
        {
            bool result = false;

            foreach (LyncUserPlan p in plans)
            {
                if (p.LyncUserPlanName.ToLower() == plan.LyncUserPlanName.ToLower())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        protected void txtMailboxPlan_TextChanged(object sender, EventArgs e)
        {
            btnUpdatePlan.Enabled = (string.IsNullOrEmpty(txtPlan.Text)) ? false : true;
        }

        private void RestampLyncUsers(int sourcePlanId, int destinationPlanId)
        {
            UserInfo[] UsersInfo = ES.Services.Users.GetUsers(PanelSecurity.SelectedUserId, true);

            try
            {
                foreach (UserInfo ui in UsersInfo)
                {
                    PackageInfo[] Packages = ES.Services.Packages.GetPackages(ui.UserId);

                    if ((Packages != null) & (Packages.GetLength(0) > 0))
                    {
                        foreach (PackageInfo Package in Packages)
                        {
                            Providers.HostedSolution.Organization[] orgs = null;

                            orgs = ES.Services.ExchangeServer.GetExchangeOrganizations(Package.PackageId, false);

                            if ((orgs != null) & (orgs.GetLength(0) > 0))
                            {
                                foreach (Organization org in orgs)
                                {
                                    if (!string.IsNullOrEmpty(org.LyncTenantId))
                                    {
                                        LyncUser[] Accounts = ES.Services.Lync.GetLyncUsersByPlanId(org.Id, sourcePlanId);

                                        foreach (LyncUser a in Accounts)
                                        {
                                            txtStatus.Text = "Completed";
                                            Providers.ResultObjects.LyncUserResult result = ES.Services.Lync.SetUserLyncPlan(org.Id, a.AccountID, destinationPlanId);
                                            if (!result.IsSuccess)
                                            {
                                                BindPlans();
                                                txtStatus.Text = "Error: " + a.DisplayName;
                                                messageBox.ShowErrorMessage("EXCHANGE_STAMPMAILBOXES");
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                messageBox.ShowSuccessMessage("EXCHANGE_STAMPMAILBOXES");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_FAILED_TO_STAMP", ex);
            }

            BindPlans();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {

            string name = tbTelephoneProvider.Text;

            if (string.IsNullOrEmpty(name)) return;

            ddTelephonyDialPlanPolicy.Items.Clear();
            string[] dialPlan = ES.Services.Lync.GetPolicyList(-1, LyncPolicyType.DialPlan, name);
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
            string[] voicePolicy = ES.Services.Lync.GetPolicyList(-1, LyncPolicyType.Voice, name);
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


    }
}
