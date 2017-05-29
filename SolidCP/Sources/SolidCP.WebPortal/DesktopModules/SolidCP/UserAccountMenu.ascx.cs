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

namespace SolidCP.Portal
{
    public partial class UserAccountMenu : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            MenuItem rootItem = new MenuItem(locMenuTitle.Text);
            rootItem.Selectable = false;
            rootItem.Value = "Account Menu";
            menu.Items.Add(rootItem);

            BindMenu(rootItem.ChildItems, PortalUtils.GetModuleMenuItems(this));
        }

        private void BindMenu(MenuItemCollection items, XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                string pageId = null;
                if (node.Attributes["pageID"] != null)
                    pageId = node.Attributes["pageID"].Value;

                if (!PortalUtils.PageExists(pageId))
                    continue;

                string url = null;
                if (node.Attributes["url"] != null)
                    url = node.Attributes["url"].Value;

                string title = null;
                if (node.Attributes["title"] != null)
                    title = node.Attributes["title"].Value;

                string target = null;
                if (node.Attributes["target"] != null)
                    target = node.Attributes["target"].Value;

                string roles = null;
                if (node.Attributes["roles"] != null)
                    roles = node.Attributes["roles"].Value;

                string selectedUserContext = null;
                if (node.Attributes["selectedUserContext"] != null)
                    selectedUserContext = node.Attributes["selectedUserContext"].Value;
                
                // get custom page parameters
                XmlNodeList xmlParameters = node.SelectNodes("Parameters/Add");
                List<string> parameters = new List<string>();
                foreach (XmlNode xmlParameter in xmlParameters)
                {
                    parameters.Add(xmlParameter.Attributes["name"].Value
                        + "=" + xmlParameter.Attributes["value"].Value);
                }

                bool display = true;
				// set user role visibility second
                if (!String.IsNullOrEmpty(selectedUserContext))
                {
                    display = false;
                    string[] arrRoles = selectedUserContext.Split(',');
                    string userRole = PanelSecurity.SelectedUser.Role.ToString();
                    foreach (string role in arrRoles)
                    {
                        if (String.Compare(userRole, role, true) == 0)
                        {
                            display = true;
                            break;
                        }
                    }
                }

                if ((!String.IsNullOrEmpty(roles)) & display)
                {
                    display = false;
                    string[] arrRoles = roles.Split(',');
                    string userRole = PanelSecurity.LoggedUser.Role.ToString();
                    foreach (string role in arrRoles)
                    {
                        if (String.Compare(userRole, role, true) == 0)
                        {
                            display = true;
                            break;
                        }
                    }
                }
                
                //Audit Log functionality is disabled when user is in Demo mode
                if ((pageId == "AuditLog") && (PanelSecurity.SelectedUser.IsDemo))
                {
                      display = false;
                }
                
                // add menu item
                if (display)
                {
                    string pageUrl = !String.IsNullOrEmpty(url) ? url : PortalUtils.NavigatePageURL(
                        pageId, PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(), parameters.ToArray());
                    string pageName = !String.IsNullOrEmpty(title) ? title : PortalUtils.GetLocalizedPageName(pageId);
                    MenuItem item = new MenuItem(pageName, "", "", pageUrl);

                    if (!String.IsNullOrEmpty(target))
                        item.Target = target;

                    //for Selected == added kuldeep 
                    if (Request.QueryString.Get("pid") != null)
                    {
                       

                        string pid = Request.QueryString.Get("pid").ToString();

                        //Parent Menu Replacement
                        if (pid.Equals("SpaceHome", StringComparison.CurrentCultureIgnoreCase))
                        { pid = "UserSpaces"; }

                        if (item.NavigateUrl.IndexOf(pid) >= 0)
                        {
                            item.Selected = true;
                        }
                    }

                    items.Add(item);

                    // process nested menu items
                    XmlNodeList xmlNestedNodes = node.SelectNodes("MenuItems/MenuItem");
                    BindMenu(item.ChildItems, xmlNestedNodes);
                }
            }
        }

        protected void menu_MenuItemDataBound(object sender, MenuEventArgs e)
        {   
          
           
        }
    }
}
