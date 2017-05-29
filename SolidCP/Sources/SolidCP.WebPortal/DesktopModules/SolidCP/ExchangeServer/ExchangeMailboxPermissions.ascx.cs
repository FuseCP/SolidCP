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
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxPermissions : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPermissions();

                if (GetLocalizedString("buttonPanel.OnSaveClientClick") != null)
                    buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePermissions();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SavePermissions();

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
        }


        private void BindPermissions()
        {
            try
            {
                ExchangeMailbox mailbox =
                    ES.Services.ExchangeServer.GetMailboxPermissions(PanelRequest.ItemID, PanelRequest.AccountID);

                litDisplayName.Text = mailbox.DisplayName;
                sendAsPermission.SetAccounts(mailbox.SendAsAccounts);
                fullAccessPermission.SetAccounts(mailbox.FullAccessAccounts);
                onBehalfOfPermissions.SetAccounts(mailbox.OnBehalfOfAccounts);
                calendarPermissions.SetAccounts(mailbox.CalendarAccounts);
                contactsPermissions.SetAccounts(mailbox.ContactAccounts);

                // get account meta
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);

                if (account.AccountType == ExchangeAccountType.SharedMailbox)
                    litDisplayName.Text += GetSharedLocalizedString("SharedMailbox.Text");

                if (account.AccountType == ExchangeAccountType.Room)
                    litDisplayName.Text += GetSharedLocalizedString("RoomMailbox.Text");

                if (account.AccountType == ExchangeAccountType.Equipment)
                    litDisplayName.Text += GetSharedLocalizedString("EquipmentMailbox.Text");

            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_PERMISSIONS", ex);
            }
        }


        private void SavePermissions()
        {
            try
            {
                string[] fullAccess = fullAccessPermission.GetAccounts();
                string[] sendAs = sendAsPermission.GetAccounts();
                string[] onBehalf = onBehalfOfPermissions.GetAccounts();
                var calendar = calendarPermissions.GetAccounts();
                var contacts = contactsPermissions.GetAccounts();
                
                int result =
                    ES.Services.ExchangeServer.SetMailboxPermissions(PanelRequest.ItemID, PanelRequest.AccountID, sendAs, fullAccess, onBehalf, calendar, contacts);


                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }



                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_PERMISSIONS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_PERMISSIONS", ex);
            }
        }
    }
}
