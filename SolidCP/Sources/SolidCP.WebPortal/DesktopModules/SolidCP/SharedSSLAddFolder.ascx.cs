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

using SolidCP.Providers.Web;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class SharedSSLAddFolder : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDomains();

                BindWebSites();

                fileLookup.PackageId = PanelSecurity.PackageId;
                virtDirName.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "VirtDirNamePolicy");
            }
        }

        private void BindWebSites()
        {
            WebSite[] webSites = ES.Services.WebServers.GetWebSites(PanelSecurity.PackageId, true);

            ddlWebSites.DataSource = webSites;
            ddlWebSites.DataTextField = "Name";
            ddlWebSites.DataValueField = "Id";
            ddlWebSites.DataBind();
            ddlWebSites.Items.Insert(0, new ListItem(GetLocalizedString("SelectWebSite.Text"), ""));
        }


        private void BindDomains()
        {
            string[] sslDomains = ES.Services.WebServers.GetSharedSSLDomains(PanelSecurity.PackageId);
            ddlDomains.DataSource = sslDomains;
            ddlDomains.DataBind();
            ddlDomains.Items.Insert(0, new ListItem(GetLocalizedString("SelectDomain.Text"), ""));
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string dirName = virtDirName.Text;

            int siteId = 0;
            if (!Int32.TryParse(ddlWebSites.SelectedValue, out siteId))
            {
                siteId = 0;
            }

            int result = 0;
            try
            {
                

                result = ES.Services.WebServers.AddSharedSSLFolder(PanelSecurity.PackageId, ddlDomains.SelectedValue,
                    siteId, dirName, fileLookup.SelectedFile);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_ADD_VDIR", ex);
                return;
            }

            // redirect to directory edit page
            Response.Redirect(EditUrl("ItemID", result.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
    }
}
