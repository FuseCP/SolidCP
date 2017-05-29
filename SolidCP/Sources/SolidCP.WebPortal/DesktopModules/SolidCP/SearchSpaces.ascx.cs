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

using SolidCP.WebPortal;
using SolidCP.Portal.UserControls;

namespace SolidCP.Portal
{
    public partial class SearchSpaces : SolidCPModuleBase
    {

        string ItemTypeName;

        const string type_WebSite = "WebSite";
        const string type_Domain = "Domain";
        const string type_Organization = "Organization";

        List<string> linkTypes = new List<string>(new string[] {type_WebSite, type_Domain, type_Organization});

        const string PID_SPACE_WEBSITES = "SpaceWebSites";
        const string PID_SPACE_DIMAINS = "SpaceDomains";
        const string PID_SPACE_EXCHANGESERVER = "SpaceExchangeServer";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind item types
                DataTable dtItemTypes = ES.Services.Packages.GetSearchableServiceItemTypes().Tables[0];
                foreach (DataRow dr in dtItemTypes.Rows)
                {
                    string displayName = dr["DisplayName"].ToString();
                    ddlItemType.Items.Add(new ListItem(
                        GetSharedLocalizedString("ServiceItemType." + displayName),
                        dr["ItemTypeID"].ToString()));

                    if (Request["ItemTypeID"] == dr["ItemTypeID"].ToString())
                        ItemTypeName = displayName;
                }

                // bind filter
                Utils.SelectListItem(ddlItemType, Request["ItemTypeID"]);
                tbSearch.Text = Request["Query"];
            }
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }

        public string GetItemPageUrl(int itemId, int spaceId)
        {
            string res = "";

            switch(ItemTypeName)
            {
                case type_WebSite:
                    res = PortalUtils.NavigatePageURL(PID_SPACE_WEBSITES, "ItemID", itemId.ToString(),
                        PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "edit_item",
                        "moduleDefId=websites");
                    break;
                case type_Domain:
                    res = PortalUtils.NavigatePageURL(PID_SPACE_DIMAINS, "DomainID", itemId.ToString(),
                        PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "edit_item",
                        "moduleDefId=domains");
                    break;
                case type_Organization:
                    res = PortalUtils.NavigatePageURL(PID_SPACE_EXCHANGESERVER, "ItemID", itemId.ToString(),
                        PortalUtils.SPACE_ID_PARAM + "=" + spaceId, DefaultPage.CONTROL_ID_PARAM + "=" + "organization_home",
                        "moduleDefId=ExchangeServer");
                    break;
            }

            return res;
        }

        protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
        {
            string query = tbSearchText.Text.Trim().Replace("%", "");
            if (query.Length == 0)
                query = tbSearch.Text.Trim().Replace("%", "");

            Response.Redirect(NavigateURL(
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "ItemTypeID=" + ddlItemType.SelectedValue,
                "Query=" + Server.UrlEncode(query)));
        }

        protected void odsPackagesPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception.InnerException);
                e.ExceptionHandled = true;
            }
        }

        public bool AllowItemLink()
        {
            bool res = linkTypes.Exists(x => x == ItemTypeName);

            return res;
        }
    }
}
