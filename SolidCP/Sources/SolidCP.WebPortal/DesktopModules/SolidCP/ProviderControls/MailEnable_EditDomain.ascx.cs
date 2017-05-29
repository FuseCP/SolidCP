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

using SolidCP.Providers.Mail;


namespace SolidCP.Portal.ProviderControls
{
    public partial class MailEnable_EditDomain : SolidCPControlBase, IMailEditDomainControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindItem(MailDomain item)
        {
            BindMailboxes(item);

            chkDomainSmartHostEnabled.Checked = Convert.ToBoolean(item["MailEnable_SmartHostEnabled"]);
            chkDomainSmartHostAuthSenders.Checked = Convert.ToBoolean(item["MailEnable_SmartHostAuth"]);
            txtDestination.Text = item.RedirectionHosts;
        }

        public void SaveItem(MailDomain item)
        {
            item.AbuseAccount = ddlAbuseAccount.SelectedValue;
            item.PostmasterAccount = ddlPostmasterAccount.SelectedValue;

            // if we have a smarthost we need to clear the catchall
            if (chkDomainSmartHostEnabled.Checked)
                item.CatchAllAccount= "";
            else
                item.CatchAllAccount = ddlCatchAllAccount.SelectedValue;

            item["MailEnable_SmartHostEnabled"] = chkDomainSmartHostEnabled.Checked.ToString();
            item["MailEnable_SmartHostAuth"] = chkDomainSmartHostAuthSenders.Checked.ToString(); 
            item.RedirectionHosts = txtDestination.Text;
        }

        private void BindMailboxes(MailDomain item)
        {
            MailAccount[] accounts = ES.Services.MailServers.GetMailAccounts(item.PackageId, false);
            MailAlias[] forwardings = ES.Services.MailServers.GetMailForwardings(item.PackageId, false);

            BindAccounts(item, ddlAbuseAccount, accounts);
            BindAccounts(item, ddlAbuseAccount, forwardings);
            Utils.SelectListItem(ddlAbuseAccount, item.AbuseAccount);

            BindAccounts(item, ddlCatchAllAccount, accounts);
            BindAccounts(item, ddlCatchAllAccount, forwardings);
            Utils.SelectListItem(ddlCatchAllAccount, item.CatchAllAccount);

            BindAccounts(item, ddlPostmasterAccount, accounts);
            BindAccounts(item, ddlPostmasterAccount, forwardings);
            Utils.SelectListItem(ddlPostmasterAccount, item.PostmasterAccount);
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
    }
}
