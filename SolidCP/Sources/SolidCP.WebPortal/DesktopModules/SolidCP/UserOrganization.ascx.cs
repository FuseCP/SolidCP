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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

using SolidCP.EnterpriseServer;
using SolidCP.WebPortal;
using SolidCP.Portal.UserControls;


namespace SolidCP.Portal
{
    public partial class UserOrganization : OrganizationMenuControl
    {

        int packageId = 0;
        override public int PackageId 
        {
            get 
            {
                // test
                //return 1; 
                return packageId; 
            }
            set
            {
                packageId = value;
            }
        }

        int itemID = 0;
        override public int ItemID
        {
            get 
            {
                // test
                //return 1;
                if (itemID != 0) return itemID;
                if (PackageId == 0) return 0;

                DataTable orgs = new OrganizationsHelper().GetOrganizations(PackageId, false);

                for (int j = 0; j < orgs.Rows.Count; j++)
                {
                    DataRow org = orgs.Rows[j];
                    int iId = (int)org["ItemID"];

                    if (itemID == 0)
                        itemID = iId;

                    object isDefault = org["IsDefault"];
                    if (isDefault is bool)
                    {
                        if ((bool)isDefault)
                        {
                            itemID = iId;
                            break;
                        }
                    }
                }

                return itemID; 
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ShortMenu = false;
            ShowImg = true;
            PutBlackBerryInExchange = true;


            if ((PackageId > 0) && (Cntx.Groups.ContainsKey(ResourceGroups.HostedOrganizations)))
            {
                MenuItemCollection items = new MenuItemCollection();

                OrganizationMenuRoot = new MenuItem(GetLocalizedString("Text.OrganizationGroup"), "", "", null);
                items.Add(OrganizationMenuRoot);

                if (ItemID > 0)
                {
                    OrganizationMenuRoot.ChildItems.Add(CreateMenuItem("OrganizationHome", "organization_home", @"Icons/organization_home_48.png"));
                    BindMenu(items);
                }
                else
                {
                    OrganizationMenuRoot.ChildItems.Add(CreateMenuItem("CreateOrganization", "create_organization", @"Icons/create_organization_48.png"));
                }


                UserOrgPanel.Visible = true;

                OrgList.DataSource = items;
                OrgList.DataBind();
            }
            else
                UserOrgPanel.Visible = false;

        }

        protected override MenuItem CreateMenuItem(string text, string key, string img)
        {
            string PID_SPACE_EXCHANGE_SERVER = "SpaceExchangeServer";

            MenuItem item = new MenuItem();

            item.Text = GetLocalizedString("Text." + text);
            item.NavigateUrl = PortalUtils.NavigatePageURL( PID_SPACE_EXCHANGE_SERVER, "ItemID", ItemID.ToString(),
                PortalUtils.SPACE_ID_PARAM + "=" + PackageId, DefaultPage.CONTROL_ID_PARAM + "=" + key,
                "moduleDefId=exchangeserver");

            if (img == null)
                item.ImageUrl = PortalUtils.GetThemedIcon("Icons/tool_48.png");
            else
                item.ImageUrl = PortalUtils.GetThemedIcon(img);

            return item;
        }

        public MenuItemCollection GetIconMenuItems(object menuItems)
        {
            return (MenuItemCollection)menuItems;
        }

        public bool IsIconMenuVisible(object menuItems)
        {
            return ((MenuItemCollection)menuItems).Count > 0;
        }

    }
}
