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

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.SkinControls
{
    public partial class GlobalSearch : SolidCPControlBase
    {
        const string TYPE_WEBSITE = "WebSite";
        const string TYPE_DOMAIN = "Domain";
        const string TYPE_ORGANIZATION = "Organization";
        const string TYPE_EXCHANGEACCOUNT = "ExchangeAccount";
        const string TYPE_RDSCOLLECTION = "RDSCollection";
        const string TYPE_LYNC = "LyncAccount";
        const string TYPE_SFB = "SfBAccount";
        const string TYPE_FOLDER = "WebDAVFolder";
        const string TYPE_SHAREPOINT = "SharePointFoundationSiteCollection";
        const string TYPE_SHAREPOINTENTERPRISE = "SharePointEnterpriseSiteCollection";
        const string TYPE_VM = "VirtualMachine";

        const string PID_SPACE_WEBSITES = "SpaceWebSites";
        const string PID_SPACE_DIMAINS = "SpaceDomains";
        const string PID_SPACE_EXCHANGESERVER = "SpaceExchangeServer";
        const string PID_SPACE_VPS = "SpaceVPS";
        const string PID_SPACE_VPS2012 = "SpaceVPS2012";
        const string PID_SPACE_VPSFORPC = "SpaceVPSForPC";
        const string PID_SPACE_PROXMOS = "SpaceProxmox";

        class Tab
        {
            int index;
            string name;

            public Tab(int index, string name)
            {
                this.index = index;
                this.name = name;
            }

            public int Index
            {
                get { return this.index; }
                set { this.index = value; }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery",ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));
            cs.RegisterClientScriptInclude("jqueryui",ResolveUrl("~/JavaScript/jquery-ui-1.8.9.min.js"));
//            cs.RegisterClientScriptBlock(this.GetType(), "jquerycss",
//                "<link rel='stylesheet' type='text/css' href='" + ResolveUrl("~/App_Themes/Default/Styles/jquery-ui-1.8.9.css") + "' />");
            if (!IsPostBack)
            {
                BindItemTypes();
            }
        }

        private void BindItemTypes()
        {
/*            // bind item types
            DataTable dtItemTypes = ES.Services.Packages.GetSearchableServiceItemTypes().Tables[0];
			foreach (DataRow dr in dtItemTypes.Rows)
			{
				// Trying a well-known workaround to distinguish different service item types with the same name
				var localizedStr = PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "ServiceItemType." + dr["DisplayName"].ToString() + "_" + dr["ItemTypeID"].ToString());
				// Looking for localized text
				if (String.IsNullOrWhiteSpace(localizedStr))
				{
					localizedStr = PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "ServiceItemType." + dr["DisplayName"].ToString());
				}
				//
				ddlItemType.Items.Add(new ListItem(localizedStr, dr["ItemTypeID"].ToString()));
			} */
        }

        protected void btnSearchUsers_Click(object sender, EventArgs e)
        {
/*            Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetUsersSearchPageId(),
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "Query=" + Server.UrlEncode(txtUsersQuery.Text),
                "Criteria=" + ddlUserFields.SelectedValue)); */
        }

        protected void btnSearchSpaces_Click(object sender, EventArgs e)
        {
/*            Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetSpacesSearchPageId(),
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "Query=" + Server.UrlEncode(txtSpacesQuery.Text),
                "ItemTypeID=" + ddlItemType.SelectedValue)); */
        }

        public string GetItemPageUrl(string fullType, string itemType, int itemId, int spaceId, int accountId, string textSearch = "")
        {
            string res = "";
            if (fullType.Equals("AccountHome"))
            {
                res = PortalUtils.GetUserHomePageUrl(itemId);
            }
            else
            {
                switch (itemType)
                {
                    case TYPE_WEBSITE:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_WEBSITES, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_item",
                            "moduleDefId=websites");
                        break;
                    case TYPE_DOMAIN:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_DIMAINS, "DomainID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_item",
                            "moduleDefId=domains");
                        break;
                    case TYPE_ORGANIZATION:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_item",
                            "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_EXCHANGEACCOUNT:
                        if (fullType.Equals("Mailbox"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("Room"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("SharedMailbox"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("JournalingMailbox"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=JournalingMailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("Equipment"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=Mailbox", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("User"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=User", "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("Contact"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=contact_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("PublicFolder"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=public_folder_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("DistributionList"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=dlist_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("DefaultSecurityGroup"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=secur_group_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else if (fullType.Equals("SecurityGroup"))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=secur_group_settings",
                                "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        }
                        else
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                                PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=edit_user",
                                "AccountID=" + accountId, "Context=User", "moduleDefId=ExchangeServer");
                        }
                        break;
                    case TYPE_RDSCOLLECTION:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=rds_edit_collection",
                            "CollectionId=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_LYNC:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId.ToString(), "ctl=edit_lync_user",
                            "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_SFB:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId.ToString(), "ctl=edit_sfb_user",
                            "AccountID=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_FOLDER:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId.ToString(), "ctl=enterprisestorage_folder_settings",
                            "FolderID=" + textSearch, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_SHAREPOINT:
                    case TYPE_SHAREPOINTENTERPRISE:
                        res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                            PortalUtils.SPACE_ID_PARAM + "=" + spaceId, "ctl=" + (itemType == TYPE_SHAREPOINT ? "sharepoint_edit_sitecollection" : "sharepoint_enterprise_edit_sitecollection"),
                            "SiteCollectionID=" + accountId, "moduleDefId=ExchangeServer");
                        break;
                    case TYPE_VM:
                        PackageContext cntx = PackagesHelper.GetCachedPackageContext(spaceId);
                        if (cntx.Groups.ContainsKey(ResourceGroups.VPS))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_VPS, "SpaceID", spaceId.ToString(),
                                "ItemID=" + itemId.ToString(), "ctl=vps_general", "moduleDefId=VPS");
                        }
                        else if (cntx.Groups.ContainsKey(ResourceGroups.VPS2012))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_VPS2012, "SpaceID", spaceId.ToString(),
                                "ItemID=" + itemId.ToString(), "ctl=vps_general", "moduleDefId=VPS2012");
                        }
                        else if (cntx.Groups.ContainsKey(ResourceGroups.VPSForPC))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_VPSFORPC, "SpaceID", spaceId.ToString(),
                                "ItemID=" + itemId.ToString(), "ctl=vps_general", "moduleDefId=VPSForPC");
                        }
                        else if (cntx.Groups.ContainsKey(ResourceGroups.Proxmox))
                        {
                            res = PortalUtils.NavigatePageURL(PID_SPACE_PROXMOS, "SpaceID", spaceId.ToString(),
                                "ItemID=" + itemId.ToString(), "ctl=vps_general", "moduleDefId=Proxmox");
                        }
                        else res = PortalUtils.GetSpaceHomePageUrl(spaceId);
                        break;
                    default:
                        res = PortalUtils.GetSpaceHomePageUrl(spaceId);
                        break;
                }
            }

            return res;
        }

        //TODO START
        protected void btnSearchObject_Click(object sender, EventArgs e)
        {
            String strColumnType = tbSearchColumnType.Text;
            String strFullType = tbSearchFullType.Text;
            String strText = tbSearchText.Text;
            if (strText.Length > 0)
            {
                if (strFullType == "Users")
                {
                    if (tbObjectId.Text.Length > 0)
                    {
                        Response.Redirect(PortalUtils.GetUserHomePageUrl(Int32.Parse(tbObjectId.Text)));
                    }
                    else
                    {
                        Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetUsersSearchPageId(),
                           PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                            "Query=" + Server.UrlEncode(strText),
                            "Criteria=" + Server.UrlEncode(strColumnType)
                        ));
                    }
                }
                else if (strFullType == "Space")
                {
                    if (tbObjectId.Text.Length > 0)
                    {
                        Response.Redirect(GetItemPageUrl(strFullType, tbSearchColumnType.Text, Int32.Parse(tbObjectId.Text), Int32.Parse(tbPackageId.Text), Int32.Parse(tbAccountId.Text), tbSearchText.Text));
                    }
                    else
                    {
                        Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetSpacesSearchPageId(),
                            PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                            "Query=" + Server.UrlEncode(strText),
                            "Criteria=" + Server.UrlEncode(strColumnType)
                        ));
                    }
                }
                else
                {
                    if (tbObjectId.Text.Length > 0)
                    {
                        Response.Redirect(GetItemPageUrl(strFullType, tbSearchColumnType.Text, Int32.Parse(tbObjectId.Text), Int32.Parse(tbPackageId.Text), Int32.Parse(tbAccountId.Text), tbSearchText.Text));
                    }
                    else
                    {
                        Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetObjectSearchPageId(),
                            PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                            "Query=" + Server.UrlEncode(strText)));
                    }
                }
            }
            else
            {
                Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetObjectSearchPageId(),
                    PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                    "Query=" + Server.UrlEncode(tbSearch.Text)));
            }
        }
        //TODO END
    }
}
