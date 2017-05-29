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

using SolidCP.Providers.Web;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class SharedSSLEditFolder : SolidCPModuleBase
    {
        class Tab
        {
            string name;
            bool enabled;

            public Tab(string name, bool enabled)
            {
                this.name = name;
                this.enabled = enabled;
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }

            public bool Enabled
            {
                get { return this.enabled; }
                set { this.enabled = value; }
            }
        }

        private int PackageId
        {
            get { return (int)ViewState["PackageId"]; }
            set { ViewState["PackageId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVirtualDir();
            }
        }

        private void BindTabs()
        {
            Tab[] tabsArray = new Tab[]
            {
                new Tab(GetLocalizedString("Tab.HomeFolder"), true),
                new Tab(GetLocalizedString("Tab.Extensions"), true),
                new Tab(GetLocalizedString("Tab.CustomErrors"),
                    PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_ERRORS)),
                new Tab(GetLocalizedString("Tab.CustomHeaders"),
                    PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_HEADERS)),
                new Tab(GetLocalizedString("Tab.MIMETypes"),
                    PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_MIME))
            };

            if (dlTabs.SelectedIndex == -1)
                dlTabs.SelectedIndex = 0;

            dlTabs.DataSource = tabsArray;
            dlTabs.DataBind();

            tabs.ActiveViewIndex = dlTabs.SelectedIndex;
        }

        protected void dlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindVirtualDir()
        {
            SharedSSLFolder vdir = null;
            try
            {
                vdir = ES.Services.WebServers.GetSharedSSLFolder(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_GET_VDIR", ex);
                return;
            }

            if (vdir == null)
                RedirectSpaceHomePage();

            PackageId = vdir.PackageId;

            // bind site
            string fullName = vdir.Name;
            lnkSiteName.Text = "https://" + fullName;
            lnkSiteName.NavigateUrl = "https://" + fullName;

            // bind controls
            webSitesHomeFolderControl.BindWebItem(PackageId, vdir);
            webSitesExtensionsControl.BindWebItem(PackageId, vdir);
            webSitesMimeTypesControl.BindWebItem(vdir);
            webSitesCustomHeadersControl.BindWebItem(vdir);
            webSitesCustomErrorsControl.BindWebItem(vdir);

			// bind tabs
			BindTabs();
        }

        private void SaveVirtualDir()
        {
            if (!Page.IsValid)
                return;

            // load original web site item
            SharedSSLFolder vdir = ES.Services.WebServers.GetSharedSSLFolder(PanelRequest.ItemID);

            // other controls
            webSitesExtensionsControl.SaveWebItem(vdir);
            webSitesHomeFolderControl.SaveWebItem(vdir);
            webSitesMimeTypesControl.SaveWebItem(vdir);
            webSitesCustomHeadersControl.SaveWebItem(vdir);
            webSitesCustomErrorsControl.SaveWebItem(vdir);

            // update web site
            try
            {
                int result = ES.Services.WebServers.UpdateSharedSSLFolder(vdir);
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

            RedirectSpaceHomePage();
        }

        private void DeleteVirtualDir()
        {
            try
            {
                int result = ES.Services.WebServers.DeleteSharedSSLFolder(PanelRequest.ItemID);
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

            RedirectSpaceHomePage();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveVirtualDir();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteVirtualDir();
        }
    }
}
