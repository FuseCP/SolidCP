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
using SolidCP.Providers.Mail;

namespace SolidCP.Portal
{
    public partial class MailAccountsEditAccount : SolidCPModuleBase
    {
        MailAccount item = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);
            // bind item
            BindItem();
        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        try
                        {
                            item = ES.Services.MailServers.GetMailAccount(PanelRequest.ItemID);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("MAIL_GET_ACCOUNT", ex);
                            return;
                        }

                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                            mailEditAddress.PackageId = item.PackageId;
                            passwordControl.SetPackagePolicy(item.PackageId, UserSettings.MAIL_POLICY, "AccountPasswordPolicy");
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        mailEditAddress.PackageId = PanelSecurity.PackageId;
                        passwordControl.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.MAIL_POLICY, "AccountPasswordPolicy");
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], "Mail", providerControl, "EditAccount.ascx");
                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext((int)ViewState["PackageId"]);

                if (!IsPostBack)
                {
					// set messagebox size textbox visibility
					HandleMaxMailboxSizeLimitDisplay(cntx);
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        mailEditAddress.Email = item.Name;
                        mailEditAddress.EditMode = true;
                        passwordControl.EditMode = true;
						// Display currently set max mailbox size limit
						SetMaxMailboxSizeLimit(item.MaxMailboxSize);
                        // other controls
                        IMailEditAccountControl ctrl = (IMailEditAccountControl)providerControl.Controls[0];
                        ctrl.BindItem(item);
                    }
                }
            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
                return;
            }
        }

		private void HandleMaxMailboxSizeLimitDisplay(PackageContext cntx)
		{
			if (cntx.Quotas.ContainsKey(Quotas.MAIL_DISABLESIZEEDIT))
			{
				// Obtain quotas from the plan assigned
				bool maxMailboxSizeChangeable = (cntx.Quotas[Quotas.MAIL_DISABLESIZEEDIT].QuotaAllocatedValue == 0);
				int maxMailboxSizeLimit = cntx.Quotas[Quotas.MAIL_MAXBOXSIZE].QuotaAllocatedValue;
				// Ensure all validation controls, markup and layout is rendered consistently
				if (maxMailboxSizeLimit == -1 && maxMailboxSizeChangeable == false)
				{
					lblMaxMailboxSizeLimit.Visible = true;
					txtMailBoxSizeLimit.Visible = false;
				}
				// 
				else if (maxMailboxSizeLimit >= 0 && maxMailboxSizeChangeable == false)
				{
					lblMaxMailboxSizeLimit.Visible = true;
					txtMailBoxSizeLimit.Visible = false;
				}
				else if(maxMailboxSizeLimit == -1 && maxMailboxSizeChangeable == true)
				{
					lblMaxMailboxSizeLimit.Visible = false;
					txtMailBoxSizeLimit.Visible = true;
				}
				else // this is the cue for the fallback clause: if (maxMailboxSizeLimit >= 0 && maxMailboxSizeChangeable == true)
				{
					lblMaxMailboxSizeLimit.Visible = false;
					txtMailBoxSizeLimit.Visible = true;
				}
				// Set the value being displayed for both controls in either case, as the logic above addresses all rendering concerns.
				SetMaxMailboxSizeLimit(maxMailboxSizeLimit);
				// Configure required field & range validators appropriately
				RequiredFieldValidator1.Enabled = txtMailBoxSizeLimit.Visible;
				MaxMailboxSizeLimitValidator.Enabled = txtMailBoxSizeLimit.Visible;
				CompareValidator1.Enabled = txtMailBoxSizeLimit.Visible;
				// Ensure the validator has its mimimun & maximum values adjusted correspondingly
				if (maxMailboxSizeLimit == -1 || maxMailboxSizeLimit == 0)
				{
					MaxMailboxSizeLimitValidator.Enabled = false;
				    CompareValidator1.Enabled = false;
				}
				else
				{
					MaxMailboxSizeLimitValidator.MinimumValue = "1";
					MaxMailboxSizeLimitValidator.MaximumValue = maxMailboxSizeLimit.ToString();
				}
				// Format the validator's error message
				MaxMailboxSizeLimitValidator.ErrorMessage = String.Format(MaxMailboxSizeLimitValidator.ErrorMessage,
					MaxMailboxSizeLimitValidator.MinimumValue, MaxMailboxSizeLimitValidator.MaximumValue);
			}
		}

		private void SetMaxMailboxSizeLimit(int sizeLimit)
		{
			txtMailBoxSizeLimit.Text = sizeLimit.ToString();
			// Ensure we use correct wording when the mailbox size limit is disabled for editing
			if (sizeLimit == -1 || sizeLimit == 0)
				lblMaxMailboxSizeLimit.Text = GetSharedLocalizedString("Text.Unlimited");
			else
				lblMaxMailboxSizeLimit.Text = sizeLimit.ToString();
		}

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            MailAccount item = new MailAccount();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = mailEditAddress.Email;
            item.Password = passwordControl.Password;
            item.MaxMailboxSize = Utils.ParseInt(txtMailBoxSizeLimit.Text);

            // Only check for conflicting names if creating new item
            if (PanelRequest.ItemID == 0)
            {
                //checking if account name is different from existing e-mail lists
                MailList[] lists = ES.Services.MailServers.GetMailLists(PanelSecurity.PackageId, true);
                foreach (MailList list in lists)
                {
                    if (item.Name == list.Name)
                    {
                        ShowWarningMessage("MAIL_ACCOUNT_NAME");
                        return;
                    }
                }

                //checking if account name is different from existing e-mail groups
                MailGroup[] mailgroups = ES.Services.MailServers.GetMailGroups(PanelSecurity.PackageId, true);
                foreach (MailGroup group in mailgroups)
                {
                    if (item.Name == group.Name)
                    {
                        ShowWarningMessage("MAIL_ACCOUNT_NAME");
                        return;
                    }
                }

                //checking if account name is different from existing forwardings
                MailAlias[] forwardings = ES.Services.MailServers.GetMailForwardings(PanelSecurity.PackageId, true);
                foreach (MailAlias forwarding in forwardings)
                {
                    if (item.Name == forwarding.Name)
                    {
                        ShowWarningMessage("MAIL_ACCOUNT_NAME");
                        return;
                    }
                }
            }

            // get other props
            IMailEditAccountControl ctrl = (IMailEditAccountControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.MailServers.AddMailAccount(item);
                    if (result == BusinessErrorCodes.ERROR_MAIL_ACCOUNT_PASSWORD_NOT_COMPLEXITY)
                    {
                        ShowErrorMessage("MAIL_ACCOUNT_PASSWORD_NOT_COMPLEXITY");
                        return;
                    }
                    if (result == BusinessErrorCodes.ERROR_MAIL_LICENSE_DOMAIN_QUOTA)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                    if (result == BusinessErrorCodes.ERROR_MAIL_LICENSE_USERS_QUOTA)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("MAIL_ADD_ACCOUNT", ex);
                    return;
                }
                //Add Mail Cleaner
                Knom.Helpers.Net.APIMailCleanerHelper.DomainAdd(mailEditAddress.Domain);
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.MailServers.UpdateMailAccount(item);
                    if (result == BusinessErrorCodes.ERROR_MAIL_ACCOUNT_PASSWORD_NOT_COMPLEXITY)
                    {
                        ShowErrorMessage("MAIL_ACCOUNT_PASSWORD_NOT_COMPLEXITY");
                        return;
                    }
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("MAIL_UPDATE_ACCOUNT", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.MailServers.DeleteMailAccount(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("MAIL_DELETE_ACCOUNT", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
    }
}
