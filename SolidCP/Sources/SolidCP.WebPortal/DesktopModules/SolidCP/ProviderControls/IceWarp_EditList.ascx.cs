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
    public partial class IceWarp_EditList : SolidCPControlBase, IMailEditListControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var moderators = ES.Services.MailServers.GetMailAccounts(PanelSecurity.PackageId, true);
                ddlListModerators.DataSource = moderators;
                ddlListModerators.DataBind();
            }

            txtMaxMembersValidator.MaximumValue = int.MaxValue.ToString();
            txtMaxMessageSizeValidator.MaximumValue = int.MaxValue.ToString();
            txtMaxMessagesPerMinuteValidator.MaximumValue = int.MaxValue.ToString();
        }

        public void BindItem(MailList item)
        {
            txtDescription.Text = item.Description;
            Utils.SelectListItem(ddlListModerators, item.ModeratorAddress);
            Utils.SelectListItem(ddlMembersSource, item.MembersSource.ToString());
            mailEditItems.Items = item.Members;
            Utils.SelectListItem(ddlFromHeaderAction, item.FromHeader.ToString());
            Utils.SelectListItem(ddlReplyToHeaderAction, item.ReplyToHeader.ToString());
            txtFromHeaderValue.Text = item.ListFromAddress;
            txtReplyToHeaderValue.Text = item.ListReplyToAddress;
            txtSubjectPrefix.Text = item.SubjectPrefix;
            Utils.SelectListItem(ddllblOriginator, item.Originator.ToString());
            Utils.SelectListItem(ddlPostingMode, item.PostingMode.ToString());
            Utils.SelectListItem(ddlPasswordProtection, item.PasswordProtection.ToString());
            txtPassword.Text = item.Password;
            Utils.SelectListItem(ddlDefaultRights, ((int) item.DefaultRights).ToString());
            txtMaxMessageSize.Text = item.MaxMessageSize.ToString();
            txtMaxMembers.Text = item.MaxMembers.ToString();
            chkSendToSender.Checked = item.SendToSender;
            chkSetRecipientToToHeader.Checked = item.SetReceipientsToToHeader;
            chkDigestMailingList.Checked = item.DigestMode;
            txtMaxMessagesPerMinute.Text = item.MaxMessagesPerMinute.ToString();
            chkSendSubscribe.Checked = item.SendSubscribe;
            chkSendUnSubscribe.Checked = item.SendUnsubscribe;
            Utils.SelectListItem(ddlConfirmSubscription, item.ConfirmSubscription.ToString());
            chkCommandInSubject.Checked = item.CommandsInSubject;
            chkEnableSubscribe.Checked = !item.DisableSubscribecommand;
            chkEnableUnsubscribe.Checked = item.AllowUnsubscribe;
            chkEnableLists.Checked = !item.DisableListcommand;
            chkEnableWhich.Checked = !item.DisableWhichCommand;
            chkEnableReview.Checked = !item.DisableReviewCommand;
            chkEnableVacation.Checked = !item.DisableVacationCommand;
            chkModerated.Checked = item.Moderated;
            txtCommandPassword.Text = item.CommandPassword;
            chkSuppressCommandResponses.Checked = item.SuppressCommandResponses;

            ddlMembersSource_SelectedIndexChanged(this, null);
            ddlFromHeaderAction_SelectedIndexChanged(this, null);
            ddlReplyToHeaderAction_SelectedIndexChanged(this, null);
            chkModerated_CheckedChanged(this, null);
            ddlPasswordProtection_SelectedIndexChanged(this, null);
        }

        public void SaveItem(MailList item)
        {
            item.Description = txtDescription.Text;
            item.ModeratorAddress = ddlListModerators.SelectedValue;
            item.MembersSource = (IceWarpListMembersSource)Enum.Parse(typeof (IceWarpListMembersSource), ddlMembersSource.SelectedValue);
            item.Members = mailEditItems.Items;
            item.FromHeader = (IceWarpListFromAndReplyToHeader)Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlFromHeaderAction.SelectedValue);
            item.ReplyToHeader = (IceWarpListFromAndReplyToHeader)Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlReplyToHeaderAction.SelectedValue);
            item.ListFromAddress = txtFromHeaderValue.Text;
            item.ListReplyToAddress = txtReplyToHeaderValue.Text;
            item.SubjectPrefix = txtSubjectPrefix.Text;
            item.Originator = (IceWarpListOriginator)Enum.Parse(typeof (IceWarpListOriginator), ddllblOriginator.SelectedValue);
            item.PostingMode = (PostingMode)Enum.Parse(typeof (PostingMode), ddlPostingMode.SelectedValue);
            item.PasswordProtection = (PasswordProtection)Enum.Parse(typeof (PasswordProtection), ddlPasswordProtection.SelectedValue);
            item.Password = txtPassword.Text;
            item.DefaultRights = (IceWarpListDefaultRights)Enum.Parse(typeof (IceWarpListDefaultRights), ddlDefaultRights.SelectedValue);
            item.MaxMessageSize = Convert.ToInt32(txtMaxMessageSize.Text);
            item.MaxMembers = Convert.ToInt32(txtMaxMembers.Text);
            item.SetReceipientsToToHeader = chkSetRecipientToToHeader.Checked;
            item.SendToSender = chkSendToSender.Checked;
            item.DigestMode = chkDigestMailingList.Checked;
            item.MaxMessagesPerMinute = Convert.ToInt32(txtMaxMessagesPerMinute.Text);
            item.SendSubscribe = chkSendSubscribe.Checked;
            item.SendUnsubscribe = chkSendUnSubscribe.Checked;
            item.ConfirmSubscription = (IceWarpListConfirmSubscription)Enum.Parse(typeof (IceWarpListConfirmSubscription), ddlConfirmSubscription.SelectedValue);
            item.CommandsInSubject = chkCommandInSubject.Checked;
            item.DisableSubscribecommand = !chkEnableSubscribe.Checked;
            item.AllowUnsubscribe = chkEnableUnsubscribe.Checked;
            item.DisableListcommand = !chkEnableLists.Checked;
            item.DisableWhichCommand = !chkEnableWhich.Checked;
            item.DisableReviewCommand = !chkEnableReview.Checked;
            item.DisableVacationCommand = !chkEnableVacation.Checked;
            item.Moderated = chkModerated.Checked;
            item.CommandPassword = txtCommandPassword.Text;
            item.SuppressCommandResponses = chkSuppressCommandResponses.Checked;
        }


        protected void ddlMembersSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            MembersRow.Visible = (IceWarpListMembersSource) Enum.Parse(typeof (IceWarpListMembersSource), ddlMembersSource.SelectedValue) == IceWarpListMembersSource.MembersInFile;
        }

        protected void ddlFromHeaderAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var setToValueChoosen = (IceWarpListFromAndReplyToHeader) Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlFromHeaderAction.SelectedValue) == IceWarpListFromAndReplyToHeader.SetToValue;
            rowFromHeaderValue.Visible = setToValueChoosen;
            //reqValFromHeaderValue.Enabled = setToValueChoosen;
        }

        protected void ddlReplyToHeaderAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var setToValueChoosen = (IceWarpListFromAndReplyToHeader) Enum.Parse(typeof (IceWarpListFromAndReplyToHeader), ddlReplyToHeaderAction.SelectedValue) == IceWarpListFromAndReplyToHeader.SetToValue;
            rowReplyToHeaderValue.Visible = setToValueChoosen;
            //reqValReplyToHeaderValue.Enabled = setToValueChoosen;
        }

        protected void chkModerated_CheckedChanged(object sender, EventArgs e)
        {
            rowCommandPassword.Visible = chkModerated.Checked;
        }

        protected void ddlPasswordProtection_SelectedIndexChanged(object sender, EventArgs e)
        {
            rowPostingPassword.Visible = ddlPasswordProtection.SelectedValue == "NoProtection";
        }
    }
}

