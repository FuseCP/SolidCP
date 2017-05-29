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
using System.Data;
using SolidCP.EnterpriseServer;
using SolidCP.WebPortal;
using SolidCP.Portal.UserControls;


namespace SolidCP.Portal
{
    public partial class OrganizationMenu : OrganizationMenuControl
    {
        DataSet myPackages;
        int currentPackage;
        // int l_CurrentItem;  --- compile warning - never used

        private PackageContext cntx = null;
        private const string PID_SPACE_EXCHANGE_SERVER = "SpaceExchangeServer";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator)
            {
                orgMenu.Visible = false;
                return;
            }

            if (PanelSecurity.PackageId == 0)
            {
                myPackages = new PackagesHelper().GetMyPackages();
                //For selectedUser have Packages or not then HIDE Menu
                if (myPackages.Tables[0].Rows.Count == 0)
                {
                    orgMenu.Visible = false;
                    return;
                }

                if (Session["currentPackage"] == null || ((int)Session["currentUser"]) != PanelSecurity.SelectedUserId)
                {
                    if (myPackages.Tables[0].Rows.Count > 0)
                    {
                        Session["currentPackage"] = myPackages.Tables[0].Rows[0][0].ToString();
                        Session["currentUser"] = PanelSecurity.SelectedUserId;
                    }
                }
                currentPackage = Convert.ToInt16(Session["currentPackage"]);
            }
            else
            {
                currentPackage = PanelSecurity.PackageId;
            }
            // load package context
            cntx = PackagesHelper.GetCachedPackageContext(currentPackage);

            ShortMenu = false;
            ShowImg = false;

            //if (currentPackage > 0 && PanelRequest.ItemID == 0)
            //{
            //    DataTable l_OrgTable;
            //    l_OrgTable = new OrganizationsHelper().GetOrganizations(currentPackage, false);
            //    if (l_OrgTable.Rows.Count > 0)
            //    {
            //        l_CurrentItem = Convert.ToInt32(l_OrgTable.Rows[0]["ItemID"]);
            //    }
            // }
            // else
            // {
            //     l_CurrentItem = PanelRequest.ItemID;
            // }


            // organization



            // if (l_CurrentItem > 0)
            // {

            //    if (!Request[DefaultPage.PAGE_ID_PARAM].Equals(PID_SPACE_EXCHANGE_SERVER, StringComparison.InvariantCultureIgnoreCase)) {
            //       MenuItem rootItem = new MenuItem(locMenuTitle.Text);
            //      rootItem.Selectable = false;

            //       menu.Items.Add(rootItem);
            //      //Add "Organization Home" menu item 
            //     MenuItem item = new MenuItem(
            //      GetLocalizedString("Text.OrganizationHome"),
            //       "",
            //       "",
            //      PortalUtils.EditUrl("ItemID", l_CurrentItem.ToString(), "organization_home", "SpaceID=" + currentPackage));//, "mid=135"
            //       makeSelectedMenu(item);
            //       rootItem.ChildItems.Add(item);
            //       this.ItemID = l_CurrentItem;
            //       this.PackageId = currentPackage;
            //       BindMenu(rootItem.ChildItems);
            //     }

            //   }
            if (cntx.Quotas.ContainsKey(Quotas.ORGANIZATIONS))
            {
                if ((cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue > 0) || (cntx.Quotas[Quotas.ORGANIZATIONS].QuotaAllocatedValue == -1))
                {
                    MenuItem rootItem = new MenuItem(locMenuTitle.Text);
                    rootItem.Value = "ORGANIZATION MENU";
                    rootItem.Selectable = false;

                    menu.Items.Add(rootItem);
                    MenuItem item = new MenuItem(
                       "Hosted Organizations",
                       "",
                       "",
                      "~/Default.aspx?pid=SpaceExchangeServer&SpaceID=" + currentPackage);
                    makeSelectedMenu(item);
                    rootItem.ChildItems.Add(item);
                    this.PackageId = currentPackage;
                    BindMenu(rootItem.ChildItems);
                }
            }
        }
    }
}
