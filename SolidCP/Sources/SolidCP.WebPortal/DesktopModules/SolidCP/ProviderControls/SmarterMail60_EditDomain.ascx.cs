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
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Mail;

namespace SolidCP.Portal.ProviderControls
{
    public partial class SmarterMail60_EditDomain : SolidCPControlBase, IMailEditDomainControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PackageInfo info = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

            AdvancedSettingsPanel.Visible = PanelSecurity.EffectiveUser.Role == UserRole.Administrator;
            InitValidators();
        }

        public void BindItem(MailDomain item)
        {
            BindMailboxes(item);
            BindQuotas(item);
            
            featuresSection.BindItem(item);
            sharingSection.BindItem(item);
            throttlingSection.BindItem(item);

            
            if (item[MailDomain.SMARTERMAIL_LICENSE_TYPE] == "PRO")
            {
                secSharing.Visible = false;
                sharingSection.Visible = false;
                secThrottling.Visible = false;
                throttlingSection.Visible = false;
            }
            else
            {
                sharingSection.BindItem(item);
                throttlingSection.BindItem(item);
            }
            
        }

        public void SaveItem(MailDomain item)
        {
            item.CatchAllAccount = ddlCatchAllAccount.SelectedValue;
            SaveQuotas(item);

            featuresSection.SaveItem(item);
            sharingSection.SaveItem(item);
            throttlingSection.SaveItem(item);

            
            if (item[MailDomain.SMARTERMAIL_LICENSE_TYPE] == "PRO")
            {
                secSharing.Visible = false;
                sharingSection.Visible = false;
                secThrottling.Visible = false;
                throttlingSection.Visible = false;
            }
            else
            {
                sharingSection.SaveItem(item);
                throttlingSection.SaveItem(item);
            }
        }

        private void SaveQuotas(MailDomain item)
        {
            item.MaxDomainSizeInMB = Utils.ParseInt(txtSize.Text);
            item.MaxDomainAliases = Utils.ParseInt(txtDomainAliases.Text);
            item.MaxDomainUsers = Utils.ParseInt(txtUser.Text);
            item.MaxAliases = Utils.ParseInt(txtUserAliases.Text);
            item.MaxLists = Utils.ParseInt(txtMailingLists.Text);
            item[MailDomain.SMARTERMAIL5_POP_RETREIVAL_ACCOUNTS] = txtPopRetreivalAccounts.Text;
            item.MaxRecipients = Utils.ParseInt(txtRecipientsPerMessage.Text);
            item.MaxMessageSize = Utils.ParseInt(txtMessageSize.Text);
        }

        private void BindQuotas(MailDomain item)
        {
            txtSize.Text = item.MaxDomainSizeInMB.ToString();
            txtDomainAliases.Text = item.MaxDomainAliases.ToString();
            txtUser.Text = item.MaxDomainUsers.ToString();
            txtUserAliases.Text = item.MaxAliases.ToString();
            txtMailingLists.Text = item.MaxLists.ToString();
            txtPopRetreivalAccounts.Text = item[MailDomain.SMARTERMAIL5_POP_RETREIVAL_ACCOUNTS];
            txtRecipientsPerMessage.Text = item.MaxRecipients.ToString();
            txtMessageSize.Text = item.MaxMessageSize.ToString();
        }

        private void BindMailboxes(MailDomain item)
        {
            MailAccount[] accounts = ES.Services.MailServers.GetMailAccounts(item.PackageId, false);
            MailAlias[] forwardings = ES.Services.MailServers.GetMailForwardings(item.PackageId, false);

            BindAccounts(item, ddlCatchAllAccount, accounts);
            BindAccounts(item, ddlCatchAllAccount, forwardings);
            Utils.SelectListItem(ddlCatchAllAccount, item.CatchAllAccount);

        }

        private void BindAccounts(MailDomain item, DropDownList ddl, MailAccount[] accounts)
        {
            if (ddl.Items.Count == 0)
                ddl.Items.Add(new ListItem(GetLocalizedString("Text.NotSelected"), ""));

            foreach (MailAccount account in accounts)
            {
                int idx = account.Name.IndexOf("@");
                string accountName = account.Name.Substring(0, idx);
                string accountDomain = account.Name.Substring(idx + 1);

                if (String.Compare(accountDomain, item.Name, true) == 0)
                    ddl.Items.Add(new ListItem(account.Name, accountName));
            }
        }

        private void InitValidators()
        {
            string message = "*";
            reqValRecipientsPerMessage.ErrorMessage = message;
            valRecipientsPerMessage.ErrorMessage = message;
            valRecipientsPerMessage.MaximumValue = int.MaxValue.ToString();

            reqValMessageSize.ErrorMessage = message;
            valMessageSize.ErrorMessage = message;
            valMessageSize.MaximumValue = int.MaxValue.ToString();

            reqValMailingLists.ErrorMessage = message;
            valMailingLists.ErrorMessage = message;
            valMailingLists.MaximumValue = int.MaxValue.ToString();

            reqPopRetreivalAccounts.ErrorMessage = message;
            valPopRetreivalAccounts.ErrorMessage = message;
            valPopRetreivalAccounts.MaximumValue = int.MaxValue.ToString();

            reqValUser.ErrorMessage = message;
            valUser.ErrorMessage = message;
            valUser.MaximumValue = int.MaxValue.ToString();

            reqValUserAliases.ErrorMessage = message;
            valUserAliases.ErrorMessage = message;
            valUserAliases.MaximumValue = int.MaxValue.ToString();

            reqValDomainAliases.ErrorMessage = message;
            valDomainAliases.ErrorMessage = message;
            valDomainAliases.MaximumValue = int.MaxValue.ToString();

            reqValDiskSpace.ErrorMessage = message;
            valDomainDiskSpace.ErrorMessage = message;
            valDomainDiskSpace.MaximumValue = int.MaxValue.ToString();

        }
    }
}
