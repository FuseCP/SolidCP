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
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SolidCP.Portal.ExchangeServer
{
	public partial class ExchangePublicFolderGeneralSettings : SolidCPModuleBase
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
				PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
				bool mailFoldersAllowed = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS)
					&& !cntx.Quotas[Quotas.EXCHANGE2007_MAILENABLEDPUBLICFOLDERS].QuotaExhausted;

                // get settings
                ExchangePublicFolder folder = ES.Services.ExchangeServer.GetPublicFolderGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID);

				litDisplayName.Text = folder.DisplayName;

				btnMailEnable.Visible = !folder.MailEnabled && mailFoldersAllowed;
				btnMailDisable.Visible = folder.MailEnabled && mailFoldersAllowed;

				tabs.MailEnabledFolder = folder.MailEnabled;

                // bind form
                txtName.Text = folder.Name;
                chkHideAddressBook.Checked = folder.HideFromAddressBook;
				List<ExchangeAccount> list = new List<ExchangeAccount>();
				
					foreach (ExchangeAccount ex in folder.Accounts)
					{ 
                      try
                        {
                            if (ex != null) 
                            { 
                            list.Add(ex);
                           
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
					}
				
                    ExchangeAccount[] accounts = list.ToArray();
                    allAccounts.SetAccounts(accounts);                   	
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_GET_PFOLDER_SETTINGS", ex);              
            }
        }
       
        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
               int result = ES.Services.ExchangeServer.SetPublicFolderGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    txtName.Text,
                    chkHideAddressBook.Checked,
					allAccounts.GetAccounts());
               
                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_PFOLDER_SETTINGS");

                // folder name
                string origName = litDisplayName.Text;
                origName = origName.Substring(0, origName.LastIndexOf("\\"));

                litDisplayName.Text = PortalAntiXSS.Encode(origName + txtName.Text);

				BindSettings();
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_UPDATE_PFOLDER_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

		protected void btnMailEnable_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "public_folder_mailenable",
				"SpaceID=" + PanelSecurity.PackageId.ToString(),
				"AccountID=" + PanelRequest.AccountID.ToString()));
		}

		protected void btnMailDisable_Click(object sender, EventArgs e)
		{
			// disable mail on folder
			try
			{

				int result = ES.Services.ExchangeServer.DisableMailPublicFolder(PanelRequest.ItemID,
					PanelRequest.AccountID);

				if (result < 0)
				{
					messageBox.ShowResultMessage(result);
					return;
				}

				// re-bind settings
				BindSettings();
			}
			catch (Exception ex)
			{
				messageBox.ShowErrorMessage("EXCHANGE_MAIL_DISABLE_PUBLIC_FOLDER", ex);
			}
		}
	}
}
