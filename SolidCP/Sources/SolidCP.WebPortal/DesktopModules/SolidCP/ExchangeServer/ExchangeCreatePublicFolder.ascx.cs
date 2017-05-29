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
	public partial class ExchangeCreatePublicFolder : SolidCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
				BindParentFolders();
                MailEnablePublicFolder();
            }
		}

		private void BindParentFolders()
		{
			// get organization info
			Organization org = ES.Services.ExchangeServer.GetOrganization(PanelRequest.ItemID);

			string rootFolder = org.OrganizationId;
			ddlParentFolder.Items.Add("\\" + org.OrganizationId);

			// get public folder accounts
			ExchangeAccount[] folders = ES.Services.ExchangeServer.GetAccounts(PanelRequest.ItemID, ExchangeAccountType.PublicFolder);

			// add folders to the tree
			foreach (ExchangeAccount folder in folders)
			{
				ddlParentFolder.Items.Add(folder.DisplayName);
			}
		}

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreatePublicFolder();
        }

        private void CreatePublicFolder()
        {
            if (!Page.IsValid)
                return;

            try
            {

                int accountId = ES.Services.ExchangeServer.CreatePublicFolder(PanelRequest.ItemID,
                    ddlParentFolder.SelectedValue,
                    txtName.Text.Trim(),
                    chkMailEnabledFolder.Checked,
                    email.AccountName,
                    email.DomainName);

                if (accountId < 0)
                {
                    messageBox.ShowResultMessage(accountId);
                    return;
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "public_folder_settings",
                    "SpaceID=" + PanelSecurity.PackageId.ToString(),
                    "ItemID=" + PanelRequest.ItemID.ToString()));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_CREATE_PUBLIC_FOLDER", ex);
            }
        }

        private void MailEnablePublicFolder()
        {
			PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
			chkMailEnabledFolder.Visible = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS)
				&& !cntx.Quotas[Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS].QuotaExhausted;

            EmailRow.Visible = chkMailEnabledFolder.Checked;
        }

        protected void chkMailEnabledFolder_CheckedChanged(object sender, EventArgs e)
        {
            MailEnablePublicFolder();
        }
	}
}
