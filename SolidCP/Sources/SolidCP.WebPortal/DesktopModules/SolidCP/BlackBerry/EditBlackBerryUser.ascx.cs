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
using System.Web.UI.WebControls;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.BlackBerry
{
    public partial class EditBlackBerryUser :  SolidCPModuleBase
    {
        public const string CANNOT_GET_BLACKBERRY_STATS = "CANNOT_GET_BLACKBERRY_STATS";

        public const string CANNOT_DELETE_BLACKBERRY_DATA = "CANNOT_DELETE_BLACKBERRY_DATA";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            BlackBerryUserStatsResult stats = ES.Services.BlackBerry.GetBlackBerryUserStats(PanelRequest.ItemID, PanelRequest.AccountID);
            if (stats.IsSuccess)
            {
                dvStats.Visible = true;
                dvStats.DataSource = stats.Value;
                dvStats.DataBind();
            }
            else
            {
                dvStats.Visible = false;
                messageBox.ShowWarningMessage(CANNOT_GET_BLACKBERRY_STATS);                
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {            
            ResultObject res = ES.Services.BlackBerry.DeleteBlackBerryUser(PanelRequest.ItemID, PanelRequest.AccountID);
            if (res.IsSuccess && res.ErrorCodes.Count == 0)
            {
                
                Response.Redirect(EditUrl("", "", "blackberry_users",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID));
            }
            else
            {
                messageBox.ShowMessage(res, "DELETE_BLACKBERRY_USER", "BlackBerry");
            }
        }

        protected void rbGeneratePassword_OnCheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;            
            if (button != null)
                tblPassword.Visible = !button.Checked;
        }

        protected void rbSpecifyPassword_OnCheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button != null)
                tblPassword.Visible = button.Checked;
        }

        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            ResultObject res;

            res = rbSpecifyPassword.Checked
                      ?
                          ES.Services.BlackBerry.SetActivationPasswordWithExpirationTime(PanelRequest.ItemID,
                                                                                         PanelRequest.AccountID,
                                                                                         txtPassword.Text,
                                                                                         Utils.ParseInt(txtTime.Text))
                      : ES.Services.BlackBerry.SetEmailActivationPassword(PanelRequest.ItemID,
                                                                          PanelRequest.AccountID);

            messageBox.ShowMessage(res, "SET_BLACKBERRY_USER_PASSWORD", "BlackBerry");
        }

        protected void btnDeleteData_Click(object sender, EventArgs e)
        {
           ResultObject res = ES.Services.BlackBerry.DeleteDataFromBlackBerryDevice(PanelRequest.ItemID,
                                                                  PanelRequest.AccountID);

           if (res.IsSuccess) 
                messageBox.ShowMessage(res, "DELETE_BLACKBERRY_DEVICE_DATA", "BlackBerry");
           else
           {
               messageBox.ShowWarningMessage(CANNOT_DELETE_BLACKBERRY_DATA);
           }
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "blackberry_users",
                "SpaceID=" + PanelSecurity.PackageId));
        }

    }
}
