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
using SolidCP.Portal.Code.UserControls;
using SolidCP.WebPortal;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class MailboxTabs : SolidCPControlBase
    {
        private string selectedTab;
        public string SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindTabs()
        {
            bool hideItems = false;

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, cntx)))
                    hideItems = true;
            }

            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("edit_user", "Tab.General"));
            tabsList.Add(CreateTab("mailbox_settings", "Tab.Settings"));
            if (!hideItems) tabsList.Add(CreateTab("mailbox_addresses", "Tab.Addresses"));
            if (!hideItems) tabsList.Add(CreateTab("mailbox_mailflow", "Tab.Mailflow"));
            if (!hideItems) tabsList.Add(CreateTab("mailbox_permissions", "Tab.Permissions"));

            string instructions = ES.Services.ExchangeServer.GetMailboxSetupInstructions(PanelRequest.ItemID, PanelRequest.AccountID, false, false, false, " ");
            if (!string.IsNullOrEmpty(instructions))
                tabsList.Add(CreateTab("mailbox_setup", "Tab.Setup"));

            if (!hideItems) tabsList.Add(CreateTab("mailbox_mobile", "Tab.Mobile"));
            //tabsList.Add(CreateTab("mailbddox_spam", "Tab.Spam"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISTRIBUTIONLISTS, cntx))
                tabsList.Add(CreateTab("mailbox_memberof", "Tab.MemberOf"));

            // find selected menu item
            int idx = 0;
            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                    break;
                idx++;
            }
            dlTabs.SelectedIndex = idx;

            dlTabs.DataSource = tabsList;
            dlTabs.DataBind();
        }

        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("AccountID", PanelRequest.AccountID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ItemID=" + PanelRequest.ItemID.ToString(),
                "Context=Mailbox"));
        }


    }
}
