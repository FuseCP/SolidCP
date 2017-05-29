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
using SolidCP.Providers.Web;

namespace SolidCP.Portal
{
    public partial class WebSitesEditVirtualApps : SolidCPModuleBase
    {

        class Tab
        {
            int index;
            string id;
            string name;

            public Tab(int index, string id, string name)
            {
                this.index = index;
                this.id = id;
                this.name = name;
            }

            public int Index
            {
                get { return this.index; }
                set { this.index = value; }
            }

            public string Id
            {
                get { return this.id; }
                set { this.id = value; }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }
        }

        private int PackageId
        {
            get { return (int)ViewState["PackageId"]; }
            set { ViewState["PackageId"] = value; }
        }

        private bool IIs7
        {
            get { return (bool)ViewState["IIs7"]; }
            set { ViewState["IIs7"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVirtualDir();
                BindTabs();
            }
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(new Tab(0, "home", GetLocalizedString("Tab.HomeFolder")));
            tabsList.Add(new Tab(1, "extensions", GetLocalizedString("Tab.Extensions")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_ERRORS))
                tabsList.Add(new Tab(2, "errors", GetLocalizedString("Tab.CustomErrors")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_HEADERS))
                tabsList.Add(new Tab(3, "headers", GetLocalizedString("Tab.CustomHeaders")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_MIME))
                tabsList.Add(new Tab(4, "mime", GetLocalizedString("Tab.MIMETypes")));

            if (dlTabs.SelectedIndex == -1)
                dlTabs.SelectedIndex = 0;

            dlTabs.DataSource = tabsList.ToArray();
            dlTabs.DataBind();

            tabs.ActiveViewIndex = tabsList[dlTabs.SelectedIndex].Index;
        }

        protected void dlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindVirtualDir()
        {
            WebAppVirtualDirectory vdir = null;
            try
            {
                vdir = ES.Services.WebServers.GetAppVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_GET_VDIR", ex);
                return;
            }

            if (vdir == null)
                RedirectToBrowsePage();

            // IIS 7.0 mode
            IIs7 = vdir.IIs7;

            PackageId = vdir.PackageId;

            // bind site
            string fullName = vdir.ParentSiteName + "/" + vdir.Name;
            lnkSiteName.Text = fullName;
            lnkSiteName.NavigateUrl = "http://" + fullName;

            // bind controls
            webSitesHomeFolderControl.BindWebItem(PackageId, vdir);
            webSitesExtensionsControl.BindWebItem(PackageId, vdir);
            webSitesMimeTypesControl.BindWebItem(vdir);
            webSitesCustomHeadersControl.BindWebItem(vdir);
            webSitesCustomErrorsControl.BindWebItem(vdir);
        }

        private void SaveVirtualDir()
        {
            if (!Page.IsValid)
                return;

            // load original web site item
            WebAppVirtualDirectory vdir = ES.Services.WebServers.GetAppVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);

            // other controls
            webSitesExtensionsControl.SaveWebItem(vdir);
            webSitesHomeFolderControl.SaveWebItem(vdir);
            webSitesMimeTypesControl.SaveWebItem(vdir);
            webSitesCustomHeadersControl.SaveWebItem(vdir);
            webSitesCustomErrorsControl.SaveWebItem(vdir);

            // update web site
            try
            {
                int result = ES.Services.WebServers.UpdateAppVirtualDirectory(PanelRequest.ItemID, vdir);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_VDIR", ex);
                return;
            }

            ReturnBack();
        }

        private void DeleteVirtualDir()
        {
            try
            {
                int result = ES.Services.WebServers.DeleteAppVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_DELETE_VDIR", ex);
                return;
            }

            ReturnBack();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveVirtualDir();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteVirtualDir();
        }

        private void ReturnBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                "MenuID=vdirs",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
