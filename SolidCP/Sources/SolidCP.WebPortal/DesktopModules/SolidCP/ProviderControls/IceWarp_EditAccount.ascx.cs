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
using SolidCP.Providers.Mail;

namespace SolidCP.Portal.ProviderControls
{
    public partial class IceWarp_EditAccount : SolidCPControlBase, IMailEditAccountControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide some form items when creating a new account
            AutoresponderPanel.Visible = (PanelRequest.ItemID > 0);
            secAutoresponder.Visible = (PanelRequest.ItemID > 0);
            ForwardingPanel.Visible = (PanelRequest.ItemID > 0);
            secForwarding.Visible = (PanelRequest.ItemID > 0);
            OlderMailsPanel.Visible = (PanelRequest.ItemID > 0);
            secOlderMails.Visible = (PanelRequest.ItemID > 0);
            Utils.SelectListItem(ddlAccountType, "1");  // Set default account type to POP3 & IMAP
        }

        public void BindItem(MailAccount item)
        {
            txtFullName.Text = item.FullName;
            Utils.SelectListItem(ddlAccountType, item.IceWarpAccountType);
            Utils.SelectListItem(ddlAccountState, item.IceWarpAccountState);
            Utils.SelectListItem(ddlRespondType, item.IceWarpRespondType);
            chkRespondOnlyBetweenDates.Checked = item.RespondOnlyBetweenDates;

            // Set respond dates to something useful if they are null in IceWarp
            if (item.RespondFrom == DateTime.MinValue)
            {
                item.RespondFrom = DateTime.Today;
            }
            if (item.RespondTo == DateTime.MinValue)
            {
                item.RespondTo = DateTime.Today.AddDays(21);
            }
            calRespondFrom.SelectedDate = item.RespondFrom;
            calRespondTo.SelectedDate = item.RespondTo;

            chkRespondOnlyBetweenDates_CheckedChanged(this, null);

            txtRespondPeriodInDays.Text = item.RespondPeriodInDays.ToString();
            txtRespondWithReplyFrom.Text = item.RespondWithReplyFrom;
            txtSubject.Text = item.ResponderSubject;
            txtMessage.Text = item.ResponderMessage;
            txtForward.Text = item.ForwardingAddresses != null ? String.Join("; ", item.ForwardingAddresses) : "";
            cbDeleteOnForward.Checked = item.DeleteOnForward;
            cbDomainAdmin.Visible = item.IsDomainAdminEnabled;
            cbDomainAdmin.Checked = item.IsDomainAdmin;

            ddlRespondType_SelectedIndexChanged(this, null);

            cbForwardOlder.Checked = item.ForwardOlder;
            txtForwardOlderDays.Text = item.ForwardOlderDays.ToString();
            txtForwardOlderTo.Text = item.ForwardOlderTo;
            cbForwardOlder_CheckedChanged(this, null);

            cbDeleteOlder.Checked = item.DeleteOlder;
            txtDeleteOlderDays.Text = item.DeleteOlderDays.ToString();
            cbDeleteOlder_CheckedChanged(this, null);
        }

        public void SaveItem(MailAccount item)
        {
            item.FullName = txtFullName.Text;
            item.IceWarpAccountType = Convert.ToInt32(ddlAccountType.SelectedValue);
            item.IceWarpAccountState = Convert.ToInt32(ddlAccountState.SelectedValue);
            item.IceWarpRespondType = Convert.ToInt32(ddlRespondType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(txtRespondPeriodInDays.Text))
            {
                item.RespondPeriodInDays = Convert.ToInt32(txtRespondPeriodInDays.Text);
            }
            item.RespondOnlyBetweenDates = chkRespondOnlyBetweenDates.Checked;
            item.RespondFrom = calRespondFrom.SelectedDate;
            item.RespondTo = calRespondTo.SelectedDate;
            item.RespondWithReplyFrom = txtRespondWithReplyFrom.Text;
            item.ResponderSubject = txtSubject.Text;
            item.ResponderMessage = txtMessage.Text;
            item.ForwardingEnabled = !string.IsNullOrWhiteSpace(txtForward.Text);
            item.ForwardingAddresses = Utils.ParseDelimitedString(txtForward.Text, ';', ' ', ',');
            item.DeleteOnForward = cbDeleteOnForward.Checked;
            item.IsDomainAdmin = cbDomainAdmin.Checked;

            item.DeleteOlder = cbDeleteOlder.Checked;
            item.DeleteOlderDays = string.IsNullOrWhiteSpace(txtDeleteOlderDays.Text) ? 0 : Convert.ToInt32(txtDeleteOlderDays.Text);

            item.ForwardOlder = cbForwardOlder.Checked;
            item.ForwardOlderDays = string.IsNullOrWhiteSpace(txtForwardOlderDays.Text) ? 0 : Convert.ToInt32(txtForwardOlderDays.Text);
            item.ForwardOlderTo = txtForwardOlderTo.Text;
        }

        protected void ddlRespondType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RespondPeriod.Visible = ddlRespondType.SelectedValue == "3";
            RespondEnabled.Visible = Convert.ToInt32(ddlRespondType.SelectedValue) > 0;
        }

        protected void cbForwardOlder_CheckedChanged(object sender, EventArgs e)
        {
            ForwardOlderEnabled.Visible = cbForwardOlder.Checked;
        }

        protected void cbDeleteOlder_CheckedChanged(object sender, EventArgs e)
        {
            DeleteOlderEnabled.Visible = cbDeleteOlder.Checked;
        }

        protected void chkRespondOnlyBetweenDates_CheckedChanged(object sender, EventArgs e)
        {
            RespondFrom.Visible = chkRespondOnlyBetweenDates.Checked;
            RespondTo.Visible = chkRespondOnlyBetweenDates.Checked;
        }
    }
}
