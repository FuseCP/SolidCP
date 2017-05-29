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
using SolidCP.WebPortal;
using SolidCP.EnterpriseServer;
namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class Menu : SolidCPControlBase
    {

        public class MenuGroup
        {
            private string text;
            private List<MenuItem> menuItems;
            private string imageUrl;

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public List<MenuItem> MenuItems
            {
                get { return menuItems; }
                set { menuItems = value; }
            }

            public string ImageUrl
            {
                get { return imageUrl; }
                set { imageUrl = value; }
            }

            public MenuGroup(string text, string imageUrl)
            {
                this.text = text;
                this.imageUrl = imageUrl;
                menuItems = new List<MenuItem>();
            }

        }

        public class MenuItem
        {
            private string url;
            private string text;
            private string key;

            public string Url
            {
                get { return url; }
                set { url = value; }
            }

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public string Key
            {
                get { return key; }
                set { key = value; }
            }


        }

        private string selectedItem;
        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }


        private void PrepareExchangeMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            bool hideItems = false;

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, cntx)))
                    hideItems = true;
            }

            MenuGroup exchangeGroup = new MenuGroup(GetLocalizedString("Text.ExchangeGroup"), imagePath + "exchange24.png");

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("Mailboxes", "mailboxes"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("ArchivingMailboxes", "archivingmailboxes"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_CONTACTS, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("Contacts", "contacts"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISTRIBUTIONLISTS, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("DistributionLists", "dlists"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_PUBLICFOLDERS, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("PublicFolders", "public_folders"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_ACTIVESYNCALLOWED, cntx))
                    exchangeGroup.MenuItems.Add(CreateMenuItem("ActiveSyncPolicy", "activesync_policy"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, cntx))
                    exchangeGroup.MenuItems.Add(CreateMenuItem("MailboxPlans", "mailboxplans"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, cntx))
                    exchangeGroup.MenuItems.Add(CreateMenuItem("RetentionPolicy", "retentionpolicy"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, cntx))
                    exchangeGroup.MenuItems.Add(CreateMenuItem("RetentionPolicyTag", "retentionpolicytag"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, cntx))
                    exchangeGroup.MenuItems.Add(CreateMenuItem("ExchangeDomainNames", "domains"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, cntx))
                    exchangeGroup.MenuItems.Add(CreateMenuItem("StorageUsage", "storage_usage"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISCLAIMERSALLOWED, cntx))
                    exchangeGroup.MenuItems.Add(CreateMenuItem("Disclaimers", "disclaimers"));

            if (exchangeGroup.MenuItems.Count > 0)
                groups.Add(exchangeGroup);

        }

        private void PrepareOrganizationMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            bool hideItems = false;

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, cntx)))
                    hideItems = true;
            }

            if (!hideItems)
            {
                MenuGroup organizationGroup = new MenuGroup(GetLocalizedString("Text.OrganizationGroup"), imagePath + "company24.png");

                if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, cntx) == false)
                {
                    if (Utils.CheckQouta(Quotas.ORGANIZATION_DOMAINS, cntx))
                        organizationGroup.MenuItems.Add(CreateMenuItem("DomainNames", "org_domains"));
                }
                
                if (Utils.CheckQouta(Quotas.ORGANIZATION_USERS, cntx))
                    organizationGroup.MenuItems.Add(CreateMenuItem("Users", "users"));

                if (Utils.CheckQouta(Quotas.ORGANIZATION_DELETED_USERS, cntx))
                    organizationGroup.MenuItems.Add(CreateMenuItem("DeletedUsers", "deleted_users"));

				if (Utils.CheckQouta(Quotas.ORGANIZATION_SECURITYGROUPS, cntx))
                    organizationGroup.MenuItems.Add(CreateMenuItem("SecurityGroups", "secur_groups"));
				
                if (organizationGroup.MenuItems.Count > 0)
                    groups.Add(organizationGroup);
            }

        }

        private void PrepareCRMMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup crmGroup = new MenuGroup(GetLocalizedString("Text.CRMGroup"), imagePath + "crm_16.png");

            crmGroup.MenuItems.Add(CreateMenuItem("CRMOrganization", "CRMOrganizationDetails"));
            crmGroup.MenuItems.Add(CreateMenuItem("CRMUsers", "CRMUsers"));
            crmGroup.MenuItems.Add(CreateMenuItem("StorageLimits", "crm_storage_settings"));

            if (crmGroup.MenuItems.Count > 0)
                groups.Add(crmGroup);

        }

        private void PrepareCRMMenu2013(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup crmGroup = new MenuGroup(GetLocalizedString("Text.CRMGroup2013"), imagePath + "crm_16.png");

            crmGroup.MenuItems.Add(CreateMenuItem("CRMOrganization", "CRMOrganizationDetails"));
            crmGroup.MenuItems.Add(CreateMenuItem("CRMUsers", "CRMUsers"));
            crmGroup.MenuItems.Add(CreateMenuItem("StorageLimits", "crm_storage_settings"));

            if (crmGroup.MenuItems.Count > 0)
                groups.Add(crmGroup);

        }

        private void PrepareBlackBerryMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup bbGroup = new MenuGroup(GetLocalizedString("Text.BlackBerryGroup"), imagePath + "blackberry16.png");

            bbGroup.MenuItems.Add(CreateMenuItem("BlackBerryUsers", "blackberry_users"));


            if (bbGroup.MenuItems.Count > 0)
                groups.Add(bbGroup);

        }

        private void PrepareSharePointMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath, string menuItemText)
        {
            MenuGroup sharepointGroup =
                    new MenuGroup(menuItemText, imagePath + "sharepoint24.png");
            sharepointGroup.MenuItems.Add(CreateMenuItem("SiteCollections", "sharepoint_sitecollections"));
            sharepointGroup.MenuItems.Add(CreateMenuItem("StorageUsage", "sharepoint_storage_usage"));
            sharepointGroup.MenuItems.Add(CreateMenuItem("StorageLimits", "sharepoint_storage_settings"));

            groups.Add(sharepointGroup);


        }


        private void PrepareSharePointEnterpriseMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath, string menuItemText)
        {
            MenuGroup sharepointGroup =
                    new MenuGroup(menuItemText, imagePath + "sharepoint24.png");
            sharepointGroup.MenuItems.Add(CreateMenuItem("SiteCollections", "sharepoint_enterprise_sitecollections"));
            sharepointGroup.MenuItems.Add(CreateMenuItem("StorageUsage", "sharepoint_enterprise_storage_usage"));
            sharepointGroup.MenuItems.Add(CreateMenuItem("StorageLimits", "sharepoint_enterprise_storage_settings"));

            groups.Add(sharepointGroup);


        }

        private void PrepareOCSMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup ocsGroup =
                   new MenuGroup(GetLocalizedString("Text.OCSGroup"), imagePath + "ocs16.png");
            ocsGroup.MenuItems.Add(CreateMenuItem("OCSUsers", "ocs_users"));


            groups.Add(ocsGroup);
        }

        private void PrepareSfBMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup sfbGroup =
                   new MenuGroup(GetLocalizedString("Text.SfBGroup"), imagePath + "SfB16.png");
            sfbGroup.MenuItems.Add(CreateMenuItem("SfBUsers", "sfb_users"));

            sfbGroup.MenuItems.Add(CreateMenuItem("SfBUserPlans", "sfb_userplans"));


            if (Utils.CheckQouta(Quotas.SFB_FEDERATION, cntx))
                sfbGroup.MenuItems.Add(CreateMenuItem("SfBFederationDomains", "sfb_federationdomains"));

            if (Utils.CheckQouta(Quotas.SFB_PHONE, cntx))
                sfbGroup.MenuItems.Add(CreateMenuItem("SfBPhoneNumbers", "sfb_phonenumbers"));

            groups.Add(sfbGroup);
        }

        private void PrepareLyncMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup lyncGroup =
                   new MenuGroup(GetLocalizedString("Text.LyncGroup"), imagePath + "lync16.png");
            lyncGroup.MenuItems.Add(CreateMenuItem("LyncUsers", "lync_users"));

            lyncGroup.MenuItems.Add(CreateMenuItem("LyncUserPlans", "lync_userplans"));


            if (Utils.CheckQouta(Quotas.LYNC_FEDERATION, cntx))
                lyncGroup.MenuItems.Add(CreateMenuItem("LyncFederationDomains", "lync_federationdomains"));

            if (Utils.CheckQouta(Quotas.LYNC_PHONE, cntx))
                lyncGroup.MenuItems.Add(CreateMenuItem("LyncPhoneNumbers", "lync_phonenumbers"));

            groups.Add(lyncGroup);
        }

        private void PrepareEnterpriseStorageMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup enterpriseStorageGroup =
                   new MenuGroup(GetLocalizedString("Text.EnterpriseStorageGroup"), imagePath + "spaces16.png");

            enterpriseStorageGroup.MenuItems.Add(CreateMenuItem("EnterpriseStorageFolders", "enterprisestorage_folders"));

            groups.Add(enterpriseStorageGroup);
        }


        private List<MenuGroup> PrepareMenu()
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            List<MenuGroup> groups = new List<MenuGroup>();

            string imagePath = String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images/Exchange", "/");

            //Organization menu group;
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedOrganizations))
                PrepareOrganizationMenu(cntx, groups, imagePath);


            //Exchange menu group;
            if (cntx.Groups.ContainsKey(ResourceGroups.Exchange))
                PrepareExchangeMenu(cntx, groups, imagePath);

            //BlackBerry Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.BlackBerry))
                PrepareBlackBerryMenu(cntx, groups, imagePath);

            //SharePoint menu group;
            if (cntx.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
            {
                PrepareSharePointMenu(cntx, groups, imagePath, GetLocalizedString("Text.SharePointFoundationServerGroup"));
            }

            if (cntx.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
            {
                PrepareSharePointEnterpriseMenu(cntx, groups, imagePath, GetLocalizedString("Text.SharePointEnterpriseServerGroup"));
            }

            //CRM Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                PrepareCRMMenu2013(cntx, groups, imagePath);
            else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM))
                PrepareCRMMenu(cntx, groups, imagePath);


            //OCS Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.OCS))
                PrepareOCSMenu(cntx, groups, imagePath);

            //Lync Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.Lync))
                PrepareLyncMenu(cntx, groups, imagePath);

            //SfB Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.SfB))
                PrepareSfBMenu(cntx, groups, imagePath);

            //EnterpriseStorage Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.EnterpriseStorage))
                PrepareEnterpriseStorageMenu(cntx, groups, imagePath);


            return groups;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<MenuGroup> groups = PrepareMenu();

            /*repMenu.SelectedIndex = -1;
			
            for(int i = 0; i < items.Count; i++)
            {
                if (String.Compare(SelectedItem, items[i].Key, true) == 0)
                {
                    repMenu.SelectedIndex = i;
                    break;
                }
            }*/

            // bind
            repMenu.DataSource = groups;
            repMenu.DataBind();
        }

        private MenuItem CreateMenuItem(string text, string key)
        {
            MenuItem item = new MenuItem();
            item.Key = key;
            item.Text = GetLocalizedString("Text." + text);
            item.Url = HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), key,
                "SpaceID=" + PanelSecurity.PackageId);
            return item;
        }
    }
}
