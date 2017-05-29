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
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using System.Drawing;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxMobileDetails : SolidCPModuleBase
    {
        public const string Unknown = "Unknown";

        private void BindData()
        {
            ExchangeMobileDevice device = ES.Services.ExchangeServer.GetMobileDevice(PanelRequest.ItemID,
                PanelRequest.DeviceId);

            if (device != null)
            {
                lblStatus.Text = GetLocalizedString(device.Status.ToString());
                switch (device.Status)
                {
                    case MobileDeviceStatus.PendingWipe:
                        lblStatus.ForeColor = Color.Red;
                        break;
                    case MobileDeviceStatus.WipeSuccessful:
                        lblStatus.ForeColor = Color.Green;
                        break;
                    default:
                        lblStatus.ForeColor = Color.Black;
                        break;
                }
                lblDeviceModel.Text = device.DeviceModel;
                lblDeviceType.Text = device.DeviceType;
                lblFirstSyncTime.Text = DateTimeToString(device.FirstSyncTime);
                lblDeviceWipeRequestTime.Text = DateTimeToString(device.DeviceWipeRequestTime);
                lblDeviceAcnowledgeTime.Text = DateTimeToString(device.DeviceWipeAckTime);
                lblLastSync.Text = DateTimeToString(device.LastSyncAttemptTime);
                lblLastUpdate.Text = DateTimeToString(device.LastPolicyUpdateTime);
                lblLastPing.Text = device.LastPingHeartbeat == 0 ? string.Empty : device.LastPingHeartbeat.ToString();
                lblDeviceFriendlyName.Text = device.DeviceFriendlyName;
                lblDeviceId.Text = device.DeviceID;
                lblDeviceUserAgent.Text = device.DeviceUserAgent;
                lblDeviceOS.Text = device.DeviceOS;
                lblDeviceOSLanguage.Text = device.DeviceOSLanguage;
                lblDeviceIMEA.Text = device.DeviceIMEI;
                lblDevicePassword.Text = device.RecoveryPassword;

                UpdateButtons(device.Status);

                // form title
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
                litDisplayName.Text = account.DisplayName;
            }
        }

        private string DateTimeToString(DateTime dateTime)
        {
            return dateTime == DateTime.MinValue ? string.Empty : dateTime.ToString("g");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();

            
        }

        private void UpdateButtons(MobileDeviceStatus status)
        {
            if (status == MobileDeviceStatus.OK)
            {
                btnWipeAllData.Visible = true;
                btnCancel.Visible = false;
            }
            else
                if (status == MobileDeviceStatus.PendingWipe)
                {
                    btnWipeAllData.Visible = false;
                    btnCancel.Visible = true;
                }
                else
                {
                    btnWipeAllData.Visible = false;
                    btnCancel.Visible = false;
                }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string str = EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_mobile",
                        "ItemID=" + PanelRequest.ItemID, "AccountID=" + PanelRequest.AccountID);
            Response.Redirect(str);

        }

        protected void btnWipeAllData_Click(object sender, EventArgs e)
        {
            ES.Services.ExchangeServer.WipeDataFromDevice(PanelRequest.ItemID, PanelRequest.DeviceId);
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ES.Services.ExchangeServer.CancelRemoteWipeRequest(PanelRequest.ItemID, PanelRequest.DeviceId);
            BindData();
        }
    }
}
