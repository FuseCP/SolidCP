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
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.OCS
{
    public partial class EditOCSUser : SolidCPModuleBase
    {
        public const string UPDATE_OCS_USER = "UPDATE_OCS_USER";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindItems();

        }

        private void BindItems()
        {
            OCSUser user =
                       ES.Services.OCS.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.InstanceID);

            if (user != null)
            {
                lblDisplayName.Text = user.DisplayName;                
                lblSIP.Text = user.PrimaryUri;
                litDisplayName.Text = user.DisplayName;                

                cbEnableFederation.Checked = user.EnabledForFederation;
                cbEnablePublicConnectivity.Checked = user.EnabledForPublicIMConectivity;
                cbArchiveInternal.Checked = user.ArchiveInternalCommunications;
                cbArchiveFederation.Checked = user.ArchiveFederatedCommunications;
                cbEnablePresence.Checked = user.EnabledForEnhancedPresence;
                cbEnablePresence.Enabled = !user.EnabledForEnhancedPresence;

            }

            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            if (org != null)
            {

                cbEnableFederation.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                   ResourceGroups.OCS,
                                                                                   Quotas.OCS_Federation);

                cbEnablePublicConnectivity.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                           ResourceGroups.OCS,
                                                                                           Quotas.
                                                                                               OCS_PublicIMConnectivity);
                cbArchiveInternal.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                  ResourceGroups.OCS,
                                                                                  Quotas.OCS_ArchiveIMConversation);

                cbArchiveFederation.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                    ResourceGroups.OCS,
                                                                                    Quotas.
                                                                                        OCS_ArchiveFederatedIMConversation);

                cbEnablePresence.Visible = PackagesHelper.CheckGroupQuotaEnabled(org.PackageId,
                                                                                 ResourceGroups.OCS,
                                                                                 Quotas.OCS_PresenceAllowed);
                locNote.Visible = cbEnablePresence.Visible;

                secFedaration.Visible = cbEnableFederation.Visible || cbEnablePublicConnectivity.Visible;
                secArchiving.Visible = cbArchiveInternal.Visible || cbArchiveFederation.Visible;
                secPresence.Visible = cbEnablePresence.Visible;                               
            }            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ES.Services.OCS.SetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.InstanceID, cbEnableFederation.Checked, cbEnablePublicConnectivity.Checked, cbArchiveInternal.Checked, cbArchiveFederation.Checked, 
                    cbEnablePresence.Enabled && cbEnablePresence.Checked);
                
                messageBox.ShowSuccessMessage(UPDATE_OCS_USER);
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage(UPDATE_OCS_USER, ex);
            }
        }
    }
}
