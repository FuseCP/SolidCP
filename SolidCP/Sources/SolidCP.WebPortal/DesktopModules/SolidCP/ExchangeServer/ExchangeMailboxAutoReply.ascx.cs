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
using System.Web.UI;
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxAutoReply : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSettings();
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                ExchangeMailboxAutoReplySettings autoReply = ES.Services.ExchangeServer.GetMailboxAutoReplySettings(PanelRequest.ItemID, PanelRequest.AccountID);

                // title
                litDisplayName.Text = mailbox.DisplayName;

                // auto reply settings
                rblSetAutoreply.SelectedIndex = (autoReply.AutoReplyState != OofState.Disabled) ? 1 : 0;
                chkAutoReplyTime.Checked = autoReply.AutoReplyState == OofState.Scheduled;
                chkOutsideOrganization.Checked = autoReply.ExternalAudience != ExternalAudience.None;
                txtIntReply.Text = autoReply.InternalMessage;
                txtExtReply.Text = autoReply.ExternalMessage;
                txtStartTime.Text = autoReply.StartTime.ToString("HH:mm");
                txtStartDate.Text = autoReply.StartTime.ToString("yyyy-MM-dd");
                txtEndTime.Text = autoReply.EndTime.ToString("HH:mm");
                txtEndDate.Text = autoReply.EndTime.ToString("yyyy-MM-dd");
                rblExternalAudience.SelectedIndex = (autoReply.ExternalAudience == ExternalAudience.Known) ? 0 : 1;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_AUTOREPLY_SETTINGS", ex);
            }
            ToggleControls();
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                ExchangeMailboxAutoReplySettings autoReply = new ExchangeMailboxAutoReplySettings();
                autoReply.AutoReplyState = (rblSetAutoreply.SelectedIndex == 0) ? OofState.Disabled : (chkAutoReplyTime.Checked) ? OofState.Scheduled : OofState.Enabled;
                autoReply.ExternalAudience = (!chkOutsideOrganization.Checked) ? ExternalAudience.None : (rblExternalAudience.SelectedIndex == 0) ? ExternalAudience.Known : ExternalAudience.All;
                autoReply.InternalMessage = txtIntReply.Text;
                autoReply.ExternalMessage = txtExtReply.Text;
                autoReply.StartTime = DateTime.Parse(txtStartDate.Text + " " + txtStartTime.Text);
                autoReply.EndTime = DateTime.Parse(txtEndDate.Text + " " + txtEndTime.Text);
                int result = ES.Services.ExchangeServer.SetMailboxAutoReplySettings(PanelRequest.ItemID, PanelRequest.AccountID, autoReply);
                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }
                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_AUTOREPLY_SETTINGS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_AUTOREPLY_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SaveSettings();

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        private void ToggleControls()
        {
            bool showAll = rblSetAutoreply.SelectedIndex == 1;
            bool showTime = chkAutoReplyTime.Checked;
            bool showExternal = chkOutsideOrganization.Checked;

            txtIntReply.Visible = showAll;
            locIntReply.Visible = showAll;
            chkAutoReplyTime.Visible = showAll;
            locStartTime.Visible = showAll && showTime;
            locEndTime.Visible = showAll && showTime;
            txtStartTime.Visible = showAll && showTime;
            txtEndTime.Visible = showAll && showTime;
            txtStartDate.Visible = showAll && showTime;
            txtEndDate.Visible = showAll && showTime;
            chkOutsideOrganization.Visible = showAll;
            rblExternalAudience.Visible = showAll && showExternal;
            txtExtReply.Visible = showAll && showExternal;
            locExtReply.Visible = showAll && showExternal;
        }

        protected void rblSetAutoreply_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void chkAutoReplyTime_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void chkOutsideOrganization_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}