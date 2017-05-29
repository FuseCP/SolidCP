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
using SolidCP.EnterpriseServer;
using System.Xml;
using System.Collections.Generic;
using SolidCP.WebPortal;

namespace SolidCP.Portal.UserControls
{
    public class OrganizationMenuControl : SolidCPModuleBase
    {

        virtual public int PackageId
        {
            get { return PanelSecurity.PackageId; }
            set { }
        }

        virtual public int ItemID
        {
            get { return PanelRequest.ItemID; }
            set { }
        }


        private PackageContext cntx = null;
        virtual public PackageContext Cntx
        {
            get
            {
                int l_CurrentPackage = 0;
                if (PackageId == 0)
                {
                    l_CurrentPackage = Convert.ToInt32(Session["currentPackage"]);
                    if (cntx == null) cntx = PackagesHelper.GetCachedPackageContext(l_CurrentPackage);
                    return cntx;
                }
                else {
                    if (cntx == null) cntx = PackagesHelper.GetCachedPackageContext(PackageId);
                    return cntx;
                }
            }
        }

        public bool ShortMenu = false;
        public bool ShowImg = false;

        public MenuItem OrganizationMenuRoot = null;
        public MenuItem ExchangeMenuRoot = null;

        public bool PutBlackBerryInExchange = false;

        public void makeSelectedMenu(MenuItem f_MenuItem)
        {
            //for Selected == added kuldeep 

            if (Request.Url.AbsoluteUri.IndexOf(f_MenuItem.NavigateUrl.Replace("~", "")) >= 0)
            {
                f_MenuItem.Selected = true;
            }
            else
            {
                if (Request.QueryString.Get("pid") != null && Request.QueryString.Get("ctl") != null)
                {
                    string pid = Request.QueryString.Get("pid").ToString();
                    string ctl = Request.QueryString.Get("ctl").ToString();
                    string archiving = "archiving=" + ((!String.IsNullOrEmpty(Request.QueryString.Get("archiving"))) ? Request.QueryString.Get("archiving") : ""); 
                    //ctl Replacement for Keep Parent menu Open
                    if (ctl.Equals("secur_group_settings", StringComparison.CurrentCultureIgnoreCase) ||
                        ctl.Equals("create_secur_group", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "secur_groups";
                    }else if (ctl.Equals("create_user", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("user_memberof", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "users";
                    }
                    else if (ctl.Equals("organization_settings_general_settings", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "organization_settings_password_settings";
                    }
                    else if (ctl.Equals("create_mailbox", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("mailbox_settings", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("mailbox_addresses", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("mailbox_mailflow", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("mailbox_permissions", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("mailbox_setup", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("mailbox_mobile", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("mailbox_memberof", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("archivingmailboxes", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "mailboxes";
                    }
                    else if (ctl.Equals("edit_user", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Request.QueryString.Get("Context") != null)
                        {
                            string Context = Request.QueryString.Get("Context").ToString();
                            if (Context.Equals("Mailbox", StringComparison.CurrentCultureIgnoreCase))
                            {
                                ctl = "mailboxes";
                            } else {
                              ctl = "users";
                            }
                        }
                    }

                    else if (ctl.Equals("contact_settings", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("contact_mailflow", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("create_contact", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "contacts";
                    }

                    else if (ctl.Equals("create_dlist", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("dlist_addresses", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("dlist_permissions", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("dlist_memberof", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("dlist_mailflow", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("dlist_settings", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "dlists";
                    }


                    else if (ctl.Equals("create_public_folder", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("public_folder_settings", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("public_folder_addresses", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("public_folder_mailflow", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "public_folders";
                    }

                    else if (ctl.Equals("add_mailboxplan", StringComparison.CurrentCultureIgnoreCase) &&
                        archiving.Equals("archiving=false", StringComparison.CurrentCultureIgnoreCase))
                    {
                       // archiving = False
                        ctl = "mailboxplans";
                    }
                    else if (ctl.Equals("add_mailboxplan", StringComparison.CurrentCultureIgnoreCase) &&
                        archiving.Equals("archiving=true", StringComparison.CurrentCultureIgnoreCase))
                    {//archiving=True
                        ctl = "retentionpolicy";
                    }
                    else if (ctl.Equals("add_domain", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("check_domain", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("domain_records", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "domains";
                    }

                    else if (ctl.Equals("disclaimers_settings", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "disclaimers";
                    }

                    else if (ctl.Equals("enterprisestorage_folder_settings", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("enterprisestorage_folder_settings_folder_permissions", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("enterprisestorage_folder_settings_owa_editing", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("create_enterprisestorage_folder", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "enterprisestorage_folders";
                    }

                    else if (ctl.Equals("create_enterprisestorage_drive_map", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "enterprisestorage_drive_maps";
                    }
                    else if (ctl.Equals("rds_create_collection", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("rds_import_collection", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "rds_collections";
                    }
                    else if (ctl.Equals("rds_add_server", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "rds_servers";
                    }
                    else if (ctl.Equals("rds_edit_collection", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("rds_edit_collection_settings", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("rds_collection_user_experience", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("rds_collection_edit_apps", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("rds_collection_edit_users", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("rds_collection_user_sessions", StringComparison.CurrentCultureIgnoreCase) || ctl.Equals("rds_collection_local_admins", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ctl = "rds_collections";
                    }

                    if (f_MenuItem.NavigateUrl.IndexOf(pid) >= 0  && f_MenuItem.NavigateUrl.IndexOf(ctl) >= 0)
                    {
                        f_MenuItem.Selected = true;
                    }
                }
            }

        }

        public void BindMenu(MenuItemCollection items)
        {
            if (PackageId == 0)
            {
                PackageId = Convert.ToInt32(Session["currentPackage"]);
            }
            System.Data.DataTable l_OrgTable;
            if (PackageId > 0 && ItemID == 0)
            {
                l_OrgTable = new OrganizationsHelper().GetOrganizations(PackageId, false);
                if (l_OrgTable.Rows.Count > 0)
                {
                    ItemID = Convert.ToInt32(l_OrgTable.Rows[0]["ItemID"]);
                }
            }
            if ((PackageId <= 0) || (ItemID <= 0))
                return;

            //Organization menu group;
            if (Cntx.Groups.ContainsKey(ResourceGroups.HostedOrganizations))
                PrepareOrganizationMenuRoot(items);

            //Exchange menu group;
            if (Cntx.Groups.ContainsKey(ResourceGroups.Exchange))
                PrepareExchangeMenuRoot(items);

            //BlackBerry Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.BlackBerry))
                PrepareBlackBerryMenuRoot(items);

            //SharePoint menu group;
            if (Cntx.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
                PrepareSharePointMenuRoot(items);

            if (Cntx.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
                PrepareSharePointEnterpriseMenuRoot(items);

            //CRM Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013))
                PrepareCRM2013MenuRoot(items);
            else if (Cntx.Groups.ContainsKey(ResourceGroups.HostedCRM))
                PrepareCRMMenuRoot(items);

            //OCS Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.OCS))
                PrepareOCSMenuRoot(items);

            //Lync Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.Lync))
                PrepareLyncMenuRoot(items);

            //SfBMenu
            if (Cntx.Groups.ContainsKey(ResourceGroups.SfB))
                PrepareSfBMenuRoot(items);

            //EnterpriseStorage Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.EnterpriseStorage))
                PrepareEnterpriseStorageMenuRoot(items);

            //Remote Desktop Services Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.RDS))
                PrepareRDSMenuRoot(items);
        }

        private void PrepareOrganizationMenuRoot(MenuItemCollection items)
        {
            bool hideItems = false;

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, Cntx)))
                    hideItems = true;
            }

            if (!hideItems)
            {
                if (ShortMenu)
                {
                    PrepareOrganizationMenu(items);
                }
                else
                {
                    MenuItem item;

                    if (OrganizationMenuRoot != null) 
                        item = OrganizationMenuRoot;
                    else
                        item = new MenuItem(GetLocalizedString("Text.OrganizationGroup"), "", "", null);

                    item.Selectable = false;

                    PrepareOrganizationMenu(item.ChildItems);

                    if ((item.ChildItems.Count > 0) && (OrganizationMenuRoot == null))
                    {
                        items.Add(item);
                    }

                    OrganizationMenuRoot = item;
                }
            }
        }

        private void PrepareOrganizationMenu(MenuItemCollection items)
        {
            if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, Cntx) == false)
            {
                if (Utils.CheckQouta(Quotas.ORGANIZATION_DOMAINS, Cntx))
                    items.Add(CreateMenuItem("DomainNames", "org_domains"));
            }
            
            if (Utils.CheckQouta(Quotas.ORGANIZATION_USERS, Cntx))
                items.Add(CreateMenuItem("Users", "users", @"Icons/user_48.png")); // items.Add(CreateMenuItem("Users", "users"));//

            if (Utils.CheckQouta(Quotas.ORGANIZATION_DELETED_USERS, Cntx))
                items.Add(CreateMenuItem("DeletedUsers", "deleted_users", @"Icons/deleted_user_48.png")); //items.Add(CreateMenuItem("DeletedUsers", "deleted_users"));//

            if (Utils.CheckQouta(Quotas.ORGANIZATION_SECURITYGROUPS, Cntx))
               items.Add(CreateMenuItem("SecurityGroups", "secur_groups", @"Icons/group_48.png")); // items.Add(CreateMenuItem("SecurityGroups", "secur_groups"));//

            items.Add(CreateMenuItem("PasswordPolicy", "organization_settings_password_settings", @"Icons/password_policy_48.png"));
            //items.Add(CreateMenuItem("PasswordPolicy", "organization_settings_password_settings"));
        }

        private void PrepareExchangeMenuRoot(MenuItemCollection items)
        {
            bool hideItems = false;

            UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);

            if (user != null)
            {
                if ((user.Role == UserRole.User) & (Utils.CheckQouta(Quotas.EXCHANGE2007_ISCONSUMER, Cntx)))
                    hideItems = true;
            }

            if (ShortMenu)
            {
                PrepareExchangeMenu(items, hideItems);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.ExchangeGroup"), "", "", null);

                item.Selectable = false;

                PrepareExchangeMenu(item.ChildItems, hideItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }

                ExchangeMenuRoot = item;
            }
        }

        private void PrepareExchangeMenu(MenuItemCollection exchangeItems, bool hideItems)
        {
            if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, Cntx))
                exchangeItems.Add(CreateMenuItem("Mailboxes", "mailboxes", @"Icons/exchange_mailboxes_48.png"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_CONTACTS, Cntx))
                exchangeItems.Add(CreateMenuItem("Contacts", "contacts", @"Icons/exchange_contacts_48.png"));

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISTRIBUTIONLISTS, Cntx))
                exchangeItems.Add(CreateMenuItem("DistributionLists", "dlists", @"Icons/exchange_dlists_48.png"));

            //if (ShortMenu) return;

            if (Utils.CheckQouta(Quotas.EXCHANGE2007_PUBLICFOLDERS, Cntx))
                exchangeItems.Add(CreateMenuItem("PublicFolders", "public_folders", @"Icons/exchange_public_folders_48.png"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_ACTIVESYNCALLOWED, Cntx))
                    exchangeItems.Add(CreateMenuItem("ActiveSyncPolicy", "activesync_policy", @"Icons/exchange_activesync_policy_48.png"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, Cntx))
                    exchangeItems.Add(CreateMenuItem("MailboxPlans", "mailboxplans", @"Icons/exchange_mailboxplans_48.png"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, Cntx))
                    exchangeItems.Add(CreateMenuItem("RetentionPolicy", "retentionpolicy", @"Icons/exchange_retentionpolicy_48.png"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2013_ALLOWRETENTIONPOLICY, Cntx))
                    exchangeItems.Add(CreateMenuItem("RetentionPolicyTag", "retentionpolicytag", @"Icons/exchange_retentionpolicytag_48.png"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, Cntx))
                    exchangeItems.Add(CreateMenuItem("ExchangeDomainNames", "domains", @"Icons/exchange_accepted_domains_48.png"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, Cntx))
                    exchangeItems.Add(CreateMenuItem("StorageUsage", "storage_usage", @"Icons/exchange_storage_usages_48.png"));

            if (!hideItems)
                if (Utils.CheckQouta(Quotas.EXCHANGE2007_DISCLAIMERSALLOWED, Cntx))
                    exchangeItems.Add(CreateMenuItem("Disclaimers", "disclaimers", @"Icons/exchange_disclaimers_48.png"));

        }

        private void PrepareCRMMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareCRMMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.CRMGroup"), "", "", null);

                item.Selectable = false;

                PrepareCRMMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }

        private void PrepareCRMMenu(MenuItemCollection crmItems)
        {
            crmItems.Add(CreateMenuItem("CRMOrganization", "CRMOrganizationDetails", @"Icons/crm_orgs_48.png"));
            crmItems.Add(CreateMenuItem("CRMUsers", "CRMUsers", @"Icons/crm_users_48.png"));

            //if (ShortMenu) return;

            crmItems.Add(CreateMenuItem("StorageLimits", "crm_storage_settings", @"Icons/crm_storage_settings_48.png"));
        }

        private void PrepareCRM2013MenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareCRM2013Menu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.CRM2013Group"), "", "", null);

                item.Selectable = false;

                PrepareCRM2013Menu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }

        private void PrepareCRM2013Menu(MenuItemCollection crmItems)
        {
            crmItems.Add(CreateMenuItem("CRMOrganization", "CRMOrganizationDetails", @"Icons/crm_orgs_48.png"));
            crmItems.Add(CreateMenuItem("CRMUsers", "CRMUsers", @"Icons/crm_users_48.png"));

            //if (ShortMenu) return;

            crmItems.Add(CreateMenuItem("StorageLimits", "crm_storage_settings", @"Icons/crm_storage_settings_48.png"));
        }

        private void PrepareBlackBerryMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareBlackBerryMenu(items);
            }
            else
            {
                MenuItem item;
                bool additem = true;

                if (PutBlackBerryInExchange && (ExchangeMenuRoot != null))
                {
                    item = ExchangeMenuRoot;
                    additem = false;
                }
                else
                    item = new MenuItem(GetLocalizedString("Text.BlackBerryGroup"), "", "", null);

                item.Selectable = false;

                PrepareBlackBerryMenu(item.ChildItems);

                additem = additem && (item.ChildItems.Count > 0);

                if (additem)
                {
                    items.Add(item);
                }
            }

        }

        private void PrepareBlackBerryMenu(MenuItemCollection bbItems)
        {
            bbItems.Add(CreateMenuItem("BlackBerryUsers", "blackberry_users", @"Icons/blackberry_users_48.png"));
        }

        private void PrepareSharePointMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareSharePointMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.SharePointFoundationServerGroup"), "", "", null);

                item.Selectable = false;

                PrepareSharePointMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }

        private void PrepareSharePointMenu(MenuItemCollection spItems)
        {                        
            spItems.Add(CreateMenuItem("SiteCollections", "sharepoint_sitecollections", @"Icons/sharepoint_sitecollections_48.png"));
            spItems.Add(CreateMenuItem("StorageUsage", "sharepoint_storage_usage", @"Icons/sharepoint_storage_usage_48.png"));
            spItems.Add(CreateMenuItem("StorageLimits", "sharepoint_storage_settings", @"Icons/sharepoint_storage_settings_48.png"));
        }


        private void PrepareSharePointEnterpriseMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareSharePointEnterpriseMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.SharePointEnterpriseServerGroup"), "", "", null);

                item.Selectable = false;

                PrepareSharePointEnterpriseMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }


        private void PrepareSharePointEnterpriseMenu(MenuItemCollection spItems)
        {
            spItems.Add(CreateMenuItem("SiteCollections", "sharepoint_enterprise_sitecollections", @"Icons/sharepoint_sitecollections_48.png"));
            spItems.Add(CreateMenuItem("StorageUsage", "sharepoint_enterprise_storage_usage", @"Icons/sharepoint_storage_usage_48.png"));
            spItems.Add(CreateMenuItem("StorageLimits", "sharepoint_enterprise_storage_settings", @"Icons/sharepoint_storage_settings_48.png"));
        }


        private void PrepareOCSMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareOCSMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.OCSGroup"), "", "", null);

                item.Selectable = false;

                PrepareOCSMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }

        private void PrepareOCSMenu(MenuItemCollection osItems)
        {
            osItems.Add(CreateMenuItem("OCSUsers", "ocs_users"));
        }

        private void PrepareLyncMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareLyncMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.LyncGroup"), "", "", null);

                item.Selectable = false;

                PrepareLyncMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }
        private void PrepareLyncMenu(MenuItemCollection lyncItems)
        {
            lyncItems.Add(CreateMenuItem("LyncUsers", "lync_users", @"Icons/lync_users_48.png"));

            //if (ShortMenu) return;

            lyncItems.Add(CreateMenuItem("LyncUserPlans", "lync_userplans", @"Icons/lync_userplans_48.png"));


            if (Utils.CheckQouta(Quotas.LYNC_FEDERATION, Cntx))
                lyncItems.Add(CreateMenuItem("LyncFederationDomains", "lync_federationdomains", @"Icons/lync_federationdomains_48.png"));

            if (Utils.CheckQouta(Quotas.LYNC_PHONE, Cntx))
                lyncItems.Add(CreateMenuItem("LyncPhoneNumbers", "lync_phonenumbers", @"Icons/lync_phonenumbers_48.png"));
        }

        private void PrepareSfBMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareSfBMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.SfBGroup"), "", "", null);

                item.Selectable = false;

                PrepareSfBMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }

        private void PrepareSfBMenu(MenuItemCollection sfbItems)
        {
            sfbItems.Add(CreateMenuItem("SfBUsers", "sfb_users", @"Icons/SfB_users_48.png"));

            //if (ShortMenu) return;

            sfbItems.Add(CreateMenuItem("SfBUserPlans", "sfb_userplans", @"Icons/SfB_userplans_48.png"));


            if (Utils.CheckQouta(Quotas.SFB_FEDERATION, Cntx))
                sfbItems.Add(CreateMenuItem("SfBFederationDomains", "sfb_federationdomains", @"Icons/SfB_federationdomains_48.png"));

            if (Utils.CheckQouta(Quotas.SFB_PHONE, Cntx))
                sfbItems.Add(CreateMenuItem("SfBPhoneNumbers", "sfb_phonenumbers", @"Icons/SfB_phonenumbers_48.png"));
        }

        private void PrepareEnterpriseStorageMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareEnterpriseStorageMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.EnterpriseStorageGroup"), "", "", null);

                item.Selectable = false;

                PrepareEnterpriseStorageMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }

        private void PrepareEnterpriseStorageMenu(MenuItemCollection enterpriseStorageItems)
        {
            enterpriseStorageItems.Add(CreateMenuItem("EnterpriseStorageFolders", "enterprisestorage_folders", @"Icons/enterprisestorage_folders_48.png"));

            if (Utils.CheckQouta(Quotas.ENTERPRICESTORAGE_DRIVEMAPS, Cntx))
            {
                enterpriseStorageItems.Add(CreateMenuItem("EnterpriseStorageDriveMaps", "enterprisestorage_drive_maps", @"Icons/enterprisestorage_drive_maps_48.png"));
            }
        }

        private void PrepareRDSMenuRoot(MenuItemCollection items)
        {
            if (ShortMenu)
            {
                PrepareRDSMenu(items);
            }
            else
            {
                MenuItem item = new MenuItem(GetLocalizedString("Text.RDSGroup"), "", "", null);

                item.Selectable = false;

                PrepareRDSMenu(item.ChildItems);

                if (item.ChildItems.Count > 0)
                {
                    items.Add(item);
                }
            }
        }

        private void PrepareRDSMenu(MenuItemCollection rdsItems)
        {
            rdsItems.Add(CreateMenuItem("RDSCollections", "rds_collections", @"Icons/rds_collections_48.png"));

            if (Utils.CheckQouta(Quotas.RDS_SERVERS, Cntx))
            {
                rdsItems.Add(CreateMenuItem("RDSServers", "rds_servers", @"Icons/rds_servers_48.png"));
            }
        }

        private MenuItem CreateMenuItem(string text, string key)
        {
            return CreateMenuItem(text, key, null);
        }

        virtual protected MenuItem CreateMenuItem(string text, string key, string img)
        {
            MenuItem item = new MenuItem();

            item.Text = GetLocalizedString("Text." + text);
            if (item.Text.Trim() == "")
            {
                item.Text = text;
            }

            item.NavigateUrl = PortalUtils.EditUrl("ItemID", ItemID.ToString(), key, "SpaceID=" + PackageId);

            if (ShowImg)
            {
                if (img==null)
                    item.ImageUrl =  PortalUtils.GetThemedIcon("Icons/tool_48.png");
                else
                    item.ImageUrl =  PortalUtils.GetThemedIcon(img);
            }

            makeSelectedMenu(item);

            return item;
        }

    }
}
