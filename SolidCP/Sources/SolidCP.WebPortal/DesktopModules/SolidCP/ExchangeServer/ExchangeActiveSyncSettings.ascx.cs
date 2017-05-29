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
    public partial class ExchangeActiveSyncSettings : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindPolicy();
            }

            

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePolicy();
        }

        private void BindPolicy()
        {
            try
            {
                // read limits
                ExchangeActiveSyncPolicy policy = ES.Services.ExchangeServer.GetActiveSyncPolicy(PanelRequest.ItemID);

                // bind data
                chkAllowNonProvisionable.Checked = policy.AllowNonProvisionableDevices;

                chkAllowAttachments.Checked = policy.AttachmentsEnabled;
                sizeMaxAttachmentSize.ValueKB = policy.MaxAttachmentSizeKB;

                chkWindowsFileShares.Checked = policy.UNCAccessEnabled;
                chkWindowsSharePoint.Checked = policy.WSSAccessEnabled;

                chkRequirePasword.Checked = policy.DevicePasswordEnabled;
                chkRequireAlphaNumeric.Checked = policy.AlphanumericPasswordRequired;
                chkEnablePasswordRecovery.Checked = policy.PasswordRecoveryEnabled;
                chkRequireEncryption.Checked = policy.DeviceEncryptionEnabled;
                chkAllowSimplePassword.Checked = policy.AllowSimplePassword;

                sizeNumberAttempts.ValueKB = policy.MaxPasswordFailedAttempts;
                sizeMinimumPasswordLength.ValueKB = policy.MinPasswordLength;
                sizeTimeReenter.ValueKB = policy.InactivityLockMin;
                sizePasswordExpiration.ValueKB = policy.PasswordExpirationDays;
                sizePasswordHistory.ValueKB = policy.PasswordHistory;
                hoursRefreshInterval.ValueHours = policy.RefreshInterval;
                ToggleControls();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_ACTIVESYNC_POLICY", ex);
            }
        }

        private void SavePolicy()
        {
            if (!Page.IsValid)
                return;

            try
            {
                // set limits
                int result = ES.Services.ExchangeServer.SetActiveSyncPolicy(PanelRequest.ItemID,
                    chkAllowNonProvisionable.Checked,
                    chkAllowAttachments.Checked,
                    sizeMaxAttachmentSize.ValueKB,

                    chkWindowsFileShares.Checked,
                    chkWindowsSharePoint.Checked,

                    chkRequirePasword.Checked,
                    chkRequireAlphaNumeric.Checked,
                    chkEnablePasswordRecovery.Checked,
                    chkRequireEncryption.Checked,
                    chkAllowSimplePassword.Checked,

                    sizeNumberAttempts.ValueKB,
                    sizeMinimumPasswordLength.ValueKB,
                    sizeTimeReenter.ValueKB,
                    sizePasswordExpiration.ValueKB,
                    sizePasswordHistory.ValueKB,
                    hoursRefreshInterval.ValueHours);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_SET_ACTIVESYNC_POLICY");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_SET_ACTIVESYNC_POLICY", ex);
            }
        }

        private void ToggleControls()
        {
            PasswordSettingsRow.Visible = chkRequirePasword.Checked;
        }

        protected void chkRequirePasword_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
