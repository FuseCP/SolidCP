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
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;


namespace SolidCP.Portal.SkinControls
{
    public partial class UserSpaceBreadcrumb : System.Web.UI.UserControl
    {
        public const string ORGANIZATION_CONTROL_KEY = "organization_home";
	    public const string PID_SPACE_EXCHANGE_SERVER = "SpaceExchangeServer";
        public const string EXCHANGE_SERVER_MODULE_DEFINTION_ID = "exchangeserver";
        public const string PAGE_NANE_KEY = "Text.PageName";
        public const string DM_FOLDER_VIRTUAL_PATH = "~/DesktopModules/";

        public bool CurrentNodeVisible
        {
            get { return CurrentNode.Visible; }
            set { CurrentNode.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindUsersPath();
            BindUserSpace();

        }

        private void BindUsersPath()
        {
            repUsersPath.DataSource = ES.Services.Users.GetUserParents(PanelSecurity.SelectedUserId);
            repUsersPath.DataBind();
        }

        private void BindUserSpace()
        {
            spanSpace.Visible = false;
            pnlViewSpace.Visible = false;
            pnlEditSpace.Visible = false;

            lnkCurrentPage.Text = PortalUtils.GetLocalizedPageName(PortalUtils.GetCurrentPageId());

            if (PanelSecurity.PackageId > 0)
            {
                // space
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package != null)
                {
                    spanSpace.Visible = true;
                    pnlViewSpace.Visible = true;

                    lnkSpace.Text = PortalAntiXSS.EncodeOld(package.PackageName);
                    lnkSpace.NavigateUrl = PortalUtils.GetSpaceHomePageUrl(package.PackageId);

                    cmdSpaceName.Text = PortalAntiXSS.EncodeOld(package.PackageName);
                    lblSpaceDescription.Text = PortalAntiXSS.EncodeOld(package.PackageComments);

                    UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
                    if (user != null)
                    {
                        lblUserAccountName.Text = PortalAntiXSS.EncodeOld(string.Format("{0} -",user.Username));
                    }

                    lnkCurrentPage.NavigateUrl = PortalUtils.NavigatePageURL(
                        PortalUtils.GetCurrentPageId(), "SpaceID", PanelSecurity.PackageId.ToString());
                }
            }
            else
            {
             //   pnlViewUser.Visible = true;

                // user
                UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
                if (user != null)
                {
              //      lblUsername.Text = user.Username;

                    lnkCurrentPage.NavigateUrl = PortalUtils.NavigatePageURL(
                        PortalUtils.GetCurrentPageId(), "UserID", PanelSecurity.SelectedUserId.ToString());
                }
            }

            // organization
            bool orgVisible = (PanelRequest.ItemID > 0 && Request[DefaultPage.PAGE_ID_PARAM].Equals(PID_SPACE_EXCHANGE_SERVER, StringComparison.InvariantCultureIgnoreCase));

            spanOrgn.Visible = orgVisible;

            if (orgVisible)
            {
                // load organization details
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);

                lnkOrgn.NavigateUrl = PortalUtils.EditUrl(
                    "ItemID", PanelRequest.ItemID.ToString(), ORGANIZATION_CONTROL_KEY,
                    "SpaceID=" + PanelSecurity.PackageId.ToString());
                lnkOrgn.Text = org.Name;

                string curCtrlKey = PanelRequest.Ctl.ToLower();
                string ctrlKey = PortalUtils.GetGeneralESControlKey(Request[DefaultPage.CONTROL_ID_PARAM].ToLower(System.Globalization.CultureInfo.InvariantCulture));

                if (curCtrlKey == "edit_user") ctrlKey = PanelRequest.Context.ToLower() == "user" ? "users" : "mailboxes";

                ModuleDefinition definition = PortalConfiguration.ModuleDefinitions[EXCHANGE_SERVER_MODULE_DEFINTION_ID];
                ModuleControl control = null;
                if (!String.IsNullOrEmpty(ctrlKey) && definition.Controls.ContainsKey(ctrlKey))
                    control = definition.Controls[ctrlKey];

                if (control != null)
                {
                    if (!String.IsNullOrEmpty(control.Src))
                    {
                        lnkOrgCurPage.Text = PortalUtils.GetLocalizedString(DM_FOLDER_VIRTUAL_PATH + control.Src, PAGE_NANE_KEY);
                        lnkOrgCurPage.NavigateUrl = PortalUtils.EditUrl(
                            "ItemID", PanelRequest.ItemID.ToString(), ctrlKey,
                            "SpaceID=" + PanelSecurity.PackageId.ToString());
                    }
                }
            }
        }

        protected void repUsersPath_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            UserInfo user = (UserInfo)e.Item.DataItem;

            HyperLink lnkUser = (HyperLink)e.Item.FindControl("lnkUser");
            if (lnkUser != null)
            {
                if (user.UserId == PanelSecurity.SelectedUserId && PanelSecurity.SelectedUserId != PanelSecurity.LoggedUserId)
                {
                    string imagePath = String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images", "/");

                    Image imgUserHome = new Image();
                    imgUserHome.ImageUrl = imagePath + "home_16_blk.png";

                    Label lblUserText = new Label();
                    lblUserText.Text = " " + user.Username;

                    lnkUser.Controls.Add(imgUserHome);
                    lnkUser.Controls.Add(lblUserText);
                }
                else
                {
                    lnkUser.Text = user.Username;
                }
                lnkUser.NavigateUrl = PortalUtils.GetUserHomePageUrl(user.UserId);
            }
        }

        protected void cmdChangeName_Click(object sender, EventArgs e)
        {
            pnlEditSpace.Visible = true;
            pnlViewSpace.Visible = false;

            txtName.Text = Server.HtmlDecode(cmdSpaceName.Text);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            pnlEditSpace.Visible = false;
            pnlViewSpace.Visible = true;
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            // update space
            int result = ES.Services.Packages.UpdatePackageName(PanelSecurity.PackageId,
                txtName.Text, lblSpaceDescription.Text);

            if (result < 0)
            {
                return;
            }

            // refresh page
            Response.Redirect(Request.Url.ToString());
        }

        
    }
}
