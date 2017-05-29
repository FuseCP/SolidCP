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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class OrganizationSecurityGroupGeneralSettings : SolidCPModuleBase
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
                OrganizationSecurityGroup securityGroup = ES.Services.Organizations.GetSecurityGroupGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID);

                litDisplayName.Text = PortalAntiXSS.Encode(securityGroup.DisplayName);

                // bind form
                txtDisplayName.Text = securityGroup.DisplayName;
                lblGroupName.Text = securityGroup.AccountName;

                members.SetAccounts(securityGroup.MembersAccounts);

                txtNotes.Text = securityGroup.Notes;

                if (securityGroup.IsDefault)
                {
                    txtDisplayName.ReadOnly = true;
                    txtNotes.ReadOnly = true;
                    members.Enabled = false;

                    btnSave.Visible = false;
                    tabs.IsDefault = true;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_GET_SECURITY_GROUP_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.Organizations.SetSecurityGroupGeneralSettings(
                    PanelRequest.ItemID,
                    PanelRequest.AccountID,
                    txtDisplayName.Text,
                    members.GetAccounts(),
                    txtNotes.Text);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                litDisplayName.Text = PortalAntiXSS.Encode(txtDisplayName.Text);

                messageBox.ShowSuccessMessage("ORGANIZATION_UPDATE_SECURITY_GROUP_SETTINGS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_UPDATE_SECURITY_GROUP_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
