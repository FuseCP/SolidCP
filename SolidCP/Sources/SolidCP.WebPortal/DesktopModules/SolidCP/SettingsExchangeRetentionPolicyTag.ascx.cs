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
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal
{
    public partial class SettingsExchangeRetentionPolicyTag : SolidCPControlBase, IUserSettingsEditorControl
    {


        public void BindSettings(UserSettings settings)
        {
            BindRetentionPolicy();


            string[] types = Enum.GetNames(typeof(ExchangeRetentionPolicyTagType));

            ddTagType.Items.Clear();
            for (int i = 0; i < types.Length; i++)
            {
                string name = GetSharedLocalizedString("Text." +types[i]);
                ddTagType.Items.Add(new ListItem(name, i.ToString()));
            }

            string[] action = Enum.GetNames(typeof(ExchangeRetentionPolicyTagAction));

            ddRetentionAction.Items.Clear();
            for (int i = 0; i < action.Length; i++)
            {
                string name = GetSharedLocalizedString("Text."+action[i]);
                ddRetentionAction.Items.Add(new ListItem(name, i.ToString()));
            }

            txtStatus.Visible = false;
        }


        private void BindRetentionPolicy()
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
                ExchangeRetentionPolicyTag[] list = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTags(orgs[0].Id);

                gvPolicy.DataSource = list;
                gvPolicy.DataBind();
            }

            btnUpdatePolicy.Enabled = (string.IsNullOrEmpty(txtPolicy.Text)) ? false : true;
        }


        public void btnAddPolicy_Click(object sender, EventArgs e)
        {
            Page.Validate("CreatePolicy");

            if (!Page.IsValid)
                return;

            ExchangeRetentionPolicyTag tag = new ExchangeRetentionPolicyTag();
            tag.TagName = txtPolicy.Text;
            tag.TagType = Convert.ToInt32(ddTagType.SelectedValue);
            tag.AgeLimitForRetention = ageLimitForRetention.QuotaValue;
            tag.RetentionAction = Convert.ToInt32(ddRetentionAction.SelectedValue);

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
                IntResult result = ES.Services.ExchangeServer.AddExchangeRetentionPolicyTag(orgs[0].Id, tag);

                if (!result.IsSuccess)
                {
                    messageBox.ShowMessage(result, "EXCHANGE_UPDATEPLANS", null);
                    return;
                }
                else
                {
                    messageBox.ShowSuccessMessage("EXCHANGE_UPDATEPLANS");
                }
            }

            BindRetentionPolicy();

        }

        protected void gvPolicy_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int mailboxPlanId = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            Providers.HostedSolution.Organization[] orgs = null;
            Providers.HostedSolution.ExchangeRetentionPolicyTag tag;

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

                        tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);

                        if (tag.ItemID != orgs[0].Id)
                        {
                            messageBox.ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                            BindRetentionPolicy();
                            return;
                        }


                        ResultObject result = ES.Services.ExchangeServer.DeleteExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);
                        if (!result.IsSuccess)
                        {
                            messageBox.ShowMessage(result, "EXCHANGE_DELETE_RETENTIONPOLICY", null);
                            return;
                        }
                        else
                        {
                            messageBox.ShowSuccessMessage("EXCHANGE_DELETE_RETENTIONPOLICY");
                        }

                        ViewState["PolicyID"] = null;

                        txtPolicy.Text = string.Empty;
                        ageLimitForRetention.QuotaValue = 0;

                        btnUpdatePolicy.Enabled = (string.IsNullOrEmpty(txtPolicy.Text)) ? false : true;

                    }
                    catch (Exception)
                    {
                        messageBox.ShowErrorMessage("EXCHANGE_DELETE_RETENTIONPOLICY");
                    }

                    BindRetentionPolicy();
                break;

                case "EditItem":
                        ViewState["PolicyID"] = mailboxPlanId;

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


                        tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);

                        txtPolicy.Text = tag.TagName;
                        Utils.SelectListItem(ddTagType, tag.TagType);
                        ageLimitForRetention.QuotaValue = tag.AgeLimitForRetention;
                        Utils.SelectListItem(ddRetentionAction, tag.RetentionAction);

                        btnUpdatePolicy.Enabled = (string.IsNullOrEmpty(txtPolicy.Text)) ? false : true;

                    break;
            }
        }


        public void SaveSettings(UserSettings settings)
        {
            settings["PolicyID"] = "";
        }


        protected void btnUpdatePolicy_Click(object sender, EventArgs e)
        {
            Page.Validate("CreatePolicy");

            if (!Page.IsValid)
                return;

            if (ViewState["PolicyID"] == null)
                return;

            int mailboxPlanId = (int)ViewState["PolicyID"];
            Providers.HostedSolution.Organization[] orgs = null;
            Providers.HostedSolution.ExchangeRetentionPolicyTag tag;


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

            tag = ES.Services.ExchangeServer.GetExchangeRetentionPolicyTag(orgs[0].Id, mailboxPlanId);

            if (tag.ItemID != orgs[0].Id)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UNABLE_USE_SYSTEMPLAN");
                BindRetentionPolicy();
                return;
            }


            tag.TagName = txtPolicy.Text;
            tag.TagType = Convert.ToInt32(ddTagType.SelectedValue);
            tag.AgeLimitForRetention = ageLimitForRetention.QuotaValue;
            tag.RetentionAction = Convert.ToInt32(ddRetentionAction.SelectedValue);

            if ((orgs != null) & (orgs.GetLength(0) > 0))
            {
                ResultObject result = ES.Services.ExchangeServer.UpdateExchangeRetentionPolicyTag(orgs[0].Id, tag);

                if (!result.IsSuccess)
                {
                    messageBox.ShowMessage(result, "EXCHANGE_UPDATEPLANS", null);
                }
                else
                {
                    messageBox.ShowSuccessMessage("EXCHANGE_UPDATEPLANS");
                }
            }

            BindRetentionPolicy();
        }

    }
}
