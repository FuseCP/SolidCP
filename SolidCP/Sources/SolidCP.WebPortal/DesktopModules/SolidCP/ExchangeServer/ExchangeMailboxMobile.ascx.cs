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
using System.Drawing;
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxMobile : SolidCPModuleBase
    {
        public const string PendingWipe = "PendingWipe";
        public const string WipeSuccessful = "WipeSuccessful";
        public const string OK = "OK";
        public const string Unknown = "Unknown";


        private void BindGrid()
        {
            ExchangeMobileDevice[] devices = ES.Services.ExchangeServer.GetMobileDevices(PanelRequest.ItemID, PanelRequest.AccountID);

            gvMobile.DataSource = devices;
            gvMobile.DataBind();

            // form title
            ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
            litDisplayName.Text = account.DisplayName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                // if (mailbox != null)
                //   litDisplayName.Text = mailbox.DisplayName;

                BindGrid();
            }

        }

        protected void gvMobile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                ES.Services.ExchangeServer.RemoveDevice(PanelRequest.ItemID, e.CommandArgument.ToString());
                BindGrid();
            }
        }


        protected string GetEditUrl(string id)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "mailbox_mobile_details",
                        "DeviceID=" + id,
                        "ItemID=" + PanelRequest.ItemID, "AccountID=" + PanelRequest.AccountID);
        }

        protected void gvMobile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ExchangeMobileDevice current = e.Row.DataItem as ExchangeMobileDevice;
                if (current != null)
                {
                    Label lblDate = e.Row.FindControl("lblLastSyncTime") as Label;
                    if (lblDate != null)
                    {
                        lblDate.Text = current.LastSuccessSync == DateTime.MinValue ? string.Empty : current.LastSuccessSync.ToString("g");
                    }

                    Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                    if (lblStatus != null)
                    {
                        switch (current.Status)
                        {
                            case MobileDeviceStatus.PendingWipe:
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = GetLocalizedString(PendingWipe);
                                break;
                            case MobileDeviceStatus.WipeSuccessful:
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = GetLocalizedString(WipeSuccessful);
                                break;
                            default:
                                lblStatus.Text = GetLocalizedString(OK);
                                lblStatus.ForeColor = Color.Black;
                                break;
                        }
                    }

                    if (string.IsNullOrEmpty(current.DeviceUserAgent))
                    {
                        HyperLink lnkDeviceUserAgent = e.Row.FindControl("lnkDeviceUserAgent") as HyperLink;
                        if (lnkDeviceUserAgent != null)
                            lnkDeviceUserAgent.Text = GetLocalizedString(Unknown);
                    }
                }

            }
        }

        protected void gvMobile_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvMobile_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}
