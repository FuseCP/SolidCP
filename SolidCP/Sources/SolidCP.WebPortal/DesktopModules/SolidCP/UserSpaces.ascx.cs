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
using SCP = SolidCP.EnterpriseServer;

using SolidCP.EnterpriseServer;
using System.Xml;
using System.Collections.Generic;

namespace SolidCP.Portal
{
    public partial class UserSpaces : SolidCPModuleBase
    {
        XmlNodeList xmlIcons = null;
        DataSet myPackages;
        int currentPackage;
        int currentUser;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));
            // check for user
            bool isUser = PanelSecurity.SelectedUser.Role == UserRole.User;

            // load icons data
            xmlIcons = this.Module.SelectNodes("Group");

            if (isUser && xmlIcons != null)
            {
                
                if(!IsPostBack) 
                {
                    myPackages = new PackagesHelper().GetMyPackages();
                    myPackages.Tables[0].DefaultView.Sort = "DefaultTopPackage DESC, PackageId ASC";
                    ddlPackageSelect.DataSource = myPackages.Tables[0].DefaultView;
                    ddlPackageSelect.DataTextField = myPackages.Tables[0].Columns[2].ColumnName;
                    ddlPackageSelect.DataValueField = myPackages.Tables[0].Columns[0].ColumnName;
                    ddlPackageSelect.DataBind();
                    if(Session["currentPackage"] == null || ((int)Session["currentUser"]) != PanelSecurity.SelectedUserId) {
                        if(ddlPackageSelect.Items.Count > 0) {
                            Session["currentPackage"] = ddlPackageSelect.Items[0].Value;
                            Session["currentUser"] = PanelSecurity.SelectedUserId;
                        }
                    } else {
                        currentPackage = int.Parse(Session["currentPackage"].ToString());
                        currentUser = int.Parse(Session["currentUser"].ToString());
                        ddlPackageSelect.SelectedValue = currentPackage.ToString();
                    }
                }
                // USER
                UserPackagesPanel.Visible = true;
                if(ddlPackageSelect.UniqueID != Page.Request.Params["__EVENTTARGET"]) { 
                    if(ddlPackageSelect.Items.Count == 0) {
                        litEmptyList.Text = GetLocalizedString("gvPackages.Empty");
                        EmptyPackagesList.Visible = true;
                    } else {
                        ddlPackageSelect.Visible = true;
                        myPackages = new PackagesHelper().GetMyPackage(int.Parse(Session["currentPackage"].ToString()));
                        if (myPackages.Tables.Count != 0)
                        {
                            PackagesList.DataSource = myPackages;
                            PackagesList.DataBind();
                        }
                    }
                }
            }
            else
            {
                // ADMINS, RESELLERS
                ResellerPackagesPanel.Visible = true;
                gvPackages.PageSize = UsersHelper.GetDisplayItemsPerPage();
                gvPackages.Columns[1].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            }

            // toggle button
            ButtonsPanel.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }
        public string GetOrgPageUrl(int spaceId)
        {
            string PID_SPACE_EXCHANGE_SERVER = "SpaceExchangeServer";
            return NavigatePageURL(PID_SPACE_EXCHANGE_SERVER, PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }


        protected void odsPackages_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "create_space"));
        }

        public MenuItemCollection GetIconsDataSource(int packageId)
        {
            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);

            // init collection
            MenuItemCollection items = new MenuItemCollection();

            // get icons list
            foreach (XmlNode xmlNode in xmlIcons)
            {
                // create icon item
                MenuItem iconItem = CreateMenuItem(cntx, xmlNode);
                if (iconItem == null)
                    continue;

                // add into list
                items.Add(iconItem);
            }

            return items;
        }



        public MenuItemCollection GetIconMenuItems(object menuItems)
        {
            return (MenuItemCollection)menuItems;
        }

        public bool IsIconMenuVisible(object menuItems)
        {
            return ((MenuItemCollection)menuItems).Count > 0;
        }

        public bool IsOrgPanelVisible(int packageId)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            return cntx.Groups.ContainsKey(ResourceGroups.HostedOrganizations);
        }


        private MenuItem CreateMenuItem(PackageContext cntx, XmlNode xmlNode)
        {
            string pageId = GetXmlAttribute(xmlNode, "pageID");

            if (!PortalUtils.PageExists(pageId))
                return null;

            string url = GetXmlAttribute(xmlNode, "url");
            string title = GetXmlAttribute(xmlNode, "title");
            string imageUrl = GetXmlAttribute(xmlNode, "imageUrl");
            string target = GetXmlAttribute(xmlNode, "target");
            string resourceGroup = GetXmlAttribute(xmlNode, "resourceGroup");
            string quota = GetXmlAttribute(xmlNode, "quota");
            bool disabled = Utils.ParseBool(GetXmlAttribute(xmlNode, "disabled"), false);

            string titleresourcekey = GetXmlAttribute(xmlNode, "titleresourcekey");
            if (!String.IsNullOrEmpty(titleresourcekey))
                title = GetLocalizedString(titleresourcekey + ".Text");

            // get custom page parameters
            XmlNodeList xmlParameters = xmlNode.SelectNodes("Parameters/Add");
            List<string> parameters = new List<string>();
            foreach (XmlNode xmlParameter in xmlParameters)
            {
                parameters.Add(xmlParameter.Attributes["name"].Value
                    + "=" + xmlParameter.Attributes["value"].Value);
            }

            // add menu item
            string pageUrl = !String.IsNullOrEmpty(url) ? url : PortalUtils.NavigatePageURL(
                pageId, PortalUtils.SPACE_ID_PARAM, cntx.Package.PackageId.ToString(), parameters.ToArray());
            string pageName = !String.IsNullOrEmpty(title) ? title : PortalUtils.GetLocalizedPageName(pageId);
            MenuItem item = new MenuItem(pageName, pageId, "", disabled ? null : pageUrl);
            item.ImageUrl = PortalUtils.GetThemedIcon(imageUrl);

            if (!String.IsNullOrEmpty(target))
                item.Target = target;
            item.Selectable = !disabled;

            // check groups/quotas
            bool display = true;
            if (cntx != null)
            {
                display = (String.IsNullOrEmpty(resourceGroup)
                    || cntx.Groups.ContainsKey(resourceGroup)) &&
                    (String.IsNullOrEmpty(quota)
                    || (cntx.Quotas.ContainsKey(quota) &&
                        cntx.Quotas[quota].QuotaAllocatedValue != 0));
            }

            // process nested menu items
            XmlNodeList xmlMenuNodes = xmlNode.SelectNodes("Icon");
            if (xmlMenuNodes.Count==0)
                xmlMenuNodes = xmlNode.SelectNodes("MenuItems/MenuItem");
            foreach (XmlNode xmlMenuNode in xmlMenuNodes)
            {
                MenuItem menuItem = CreateMenuItem(cntx, xmlMenuNode);
                if (menuItem != null)
                    item.ChildItems.Add(menuItem);
            }

            // test
            //return item;

            if (display && !(disabled && item.ChildItems.Count == 0))
                return item;

            return null;
        }

        private string GetXmlAttribute(XmlNode node, string name)
        {
            return node.Attributes[name] != null ? node.Attributes[name].Value : null;
        }

        public void openSelectedPackage(Object sender, EventArgs e) {
            Session["currentPackage"] = int.Parse(ddlPackageSelect.SelectedValue);
            Response.Redirect(Request.RawUrl);
        }
    }
}
