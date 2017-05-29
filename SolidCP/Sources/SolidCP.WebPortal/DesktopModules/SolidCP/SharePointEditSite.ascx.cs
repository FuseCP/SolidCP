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
using SolidCP.Providers.SharePoint;

namespace SolidCP.Portal
{
    public partial class SharePointEditSite : SolidCPModuleBase
    {
        SharePointSite item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool newItem = (PanelRequest.ItemID == 0);

            tblEditItem.Visible = newItem;
            tblViewItem.Visible = !newItem;

            btnUpdate.Visible = newItem;
            btnDelete.Visible = !newItem;
            btnUpdate.Text = newItem ? GetLocalizedString("Text.Add") : GetLocalizedString("Text.Update");
            btnBackup.Enabled = btnRestore.Enabled = btnWebParts.Enabled = !newItem;

            // bind item
            BindItem();
        }

        private void BindDatabaseVersions()
        {
            List<string> versions = new List<string>();
            versions.Add(ResourceGroups.MsSql2000);
            versions.Add(ResourceGroups.MsSql2005);
            versions.Add(ResourceGroups.MsSql2008);
            versions.Add(ResourceGroups.MsSql2012);
            versions.Add(ResourceGroups.MsSql2014);
            versions.Add(ResourceGroups.MsSql2016);

            FillDatabaseVersions(PanelSecurity.PackageId, ddlDatabaseVersion.Items, versions);
        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        item = ES.Services.SharePointServers.GetSharePointSite(PanelRequest.ItemID);
                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        databaseName.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.MSSQL_POLICY, "DatabaseNamePolicy");
                        databaseUser.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.MSSQL_POLICY, "UserNamePolicy");
                        databasePassword.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.MSSQL_POLICY, "UserPasswordPolicy");
                        BindDatabaseVersions();
                        BindWebSites(PanelSecurity.PackageId);
                        BindUsers(PanelSecurity.PackageId);
                    }
                }

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        litWebSite.Text = item.Name;
                        litLocaleID.Text = (item.LocaleID == 0) ? "1033" : item.LocaleID.ToString();
                        litSiteOwner.Text = item.OwnerLogin;
                        litOwnerEmail.Text = item.OwnerEmail;
                        litDatabaseName.Text = item.DatabaseName;
                        litDatabaseUser.Text = item.DatabaseUser;
                    }
                }

            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
                return;
            }
        }

        private void BindWebSites(int packageId)
        {
            ddlWebSites.DataSource = ES.Services.WebServers.GetWebSites(packageId, false);
            ddlWebSites.DataBind();
            ddlWebSites.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectSite"), ""));
        }

        private void BindUsers(int packageId)
        {
            ddlSiteOwner.DataSource = ES.Services.SharePointServers.GetSharePointUsers(packageId, false);
            ddlSiteOwner.DataBind();
            ddlSiteOwner.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectUser"), ""));
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            item = new SharePointSite();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = ddlWebSites.SelectedValue;
            item.LocaleID = Utils.ParseInt(txtLocaleID.Text.Trim(), 0);
            item.OwnerLogin = ddlSiteOwner.SelectedValue;
            item.OwnerEmail = txtOwnerEmail.Text;
            item.DatabaseGroupName = ddlDatabaseVersion.SelectedValue;
            item.DatabaseName = databaseName.Text;
            item.DatabaseUser = databaseUser.Text;
            item.DatabasePassword = databasePassword.Password;

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.SharePointServers.AddSharePointSite(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SHAREPOINT_ADD_SITE", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.SharePointServers.DeleteSharePointSite(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_DELETE_SITE", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "backup",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "restore",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnWebParts_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "webparts",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
