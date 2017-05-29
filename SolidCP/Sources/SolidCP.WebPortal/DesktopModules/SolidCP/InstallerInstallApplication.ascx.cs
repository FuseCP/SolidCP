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
using System.IO;
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

namespace SolidCP.Portal
{
    public partial class InstallerInstallApplication : SolidCPModuleBase
    {
        ApplicationInfo app = null;
        IWebInstallerSettings settings = null;
        int packageId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            packageId = PanelSecurity.PackageId;

            // load app info
            try
            {
                app = ES.Services.ApplicationsInstaller.GetApplication(packageId, PanelRequest.ApplicationID);
                if (app == null)
                    RedirectToBrowsePage();


                LoadApplicationSettingsControl();

                if (!IsPostBack)
                {
                    BindWebSites();
                    BindDatabaseVersions();
                    ToggleControls();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("APPINSTALLER_INIT_FORM", ex);
                return;
            }
        }

        private void BindWebSites()
        {
            ddlWebSite.DataSource = ES.Services.WebServers.GetWebSites(packageId, false);
            ddlWebSite.DataBind();
            ddlWebSite.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectWebSite"), ""));

            // apply policy to virtual dirs
            directoryName.SetPackagePolicy(packageId, UserSettings.WEB_POLICY, "VirtDirNamePolicy");
        }

        private void BindDatabaseVersions()
        {
            // extract required databases
            List<string> versions = new List<string>();
            foreach (ApplicationRequirement req in app.Requirements)
            {
                if (req.Groups != null)
                    versions.AddRange(req.Groups);
            }

            // fill databases box
            FillDatabaseVersions(packageId, ddlDatabaseGroup.Items, versions);

            // hide module if required
            divDatabase.Visible = (ddlDatabaseGroup.Items.Count > 0);

            BindDatabases();
            BindDatabaseUsers();
            ApplyDatabasePolicy();
        }

        private void BindDatabases()
        {
            if (ddlDatabaseGroup.Items.Count == 0)
                return; // no database required

            ddlDatabase.DataSource = ES.Services.DatabaseServers.GetSqlDatabases(packageId, ddlDatabaseGroup.SelectedValue, false);
            ddlDatabase.DataBind();
            ddlDatabase.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectDatabase"), ""));
        }

        private void BindDatabaseUsers()
        {
            if (ddlDatabaseGroup.Items.Count == 0)
                return; // no database required

            ddlUser.DataSource = ES.Services.DatabaseServers.GetSqlUsers(packageId, ddlDatabaseGroup.SelectedValue, false);
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectUser"), ""));
        }

        private void ApplyDatabasePolicy()
        {
            string groupName = ddlDatabaseGroup.SelectedValue;
            if (groupName == null)
                return;

            string settingsName = UserSettings.MYSQL_POLICY;
            if(groupName.ToLower().StartsWith("mssql"))
                settingsName = UserSettings.MSSQL_POLICY;
            if (groupName.ToLower().StartsWith("mariadb"))
                settingsName = UserSettings.MARIADB_POLICY;
            databaseName.SetPackagePolicy(packageId, settingsName, "DatabaseNamePolicy");
            databaseUser.SetPackagePolicy(packageId, settingsName, "UserNamePolicy");
            databasePassword.SetPackagePolicy(packageId, settingsName, "UserPasswordPolicy");
        }

        private void ToggleControls()
        {
            tblNewDatabase.Visible = (rblDatabase.SelectedIndex == 0);
            tblExistingDatabase.Visible = (rblDatabase.SelectedIndex != 0);
            rowNewUser.Visible = (rblUser.SelectedIndex == 0);
            rowExistingUser.Visible = (rblUser.SelectedIndex != 0);
        }

        private void LoadApplicationSettingsControl()
        {
            string controlName = app.SettingsControl;
            if (!String.IsNullOrEmpty(controlName))
            {
                try
                {
                    string currPath = this.AppRelativeVirtualPath;
                    currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                    string ctrlPath = currPath + "/ApplicationInstallerControls/" + controlName;

                    if (File.Exists(Server.MapPath(ctrlPath)))
                    {
                        Control ctrl = Page.LoadControl(ctrlPath);
                        settings = (IWebInstallerSettings)ctrl;
                        appSettings.Controls.Add(ctrl);
                    }
                    else
                    {
                        divSettings.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("APPINSTALLER_LOAD_CONTROL", ex);
                    return;
                }
            }
            else
            {
                divSettings.Visible = false;
            }
        }

        private void InstallApplication()
        {
            if (!Page.IsValid)
                return;

            InstallationInfo inst = new InstallationInfo();
            inst.PackageId = packageId;
            inst.ApplicationId = PanelRequest.ApplicationID;
            inst.WebSiteId = Utils.ParseInt(ddlWebSite.SelectedValue, 0);
            inst.VirtualDir = directoryName.Text;
            inst.DatabaseGroup = ddlDatabaseGroup.SelectedValue;
            inst.DatabaseId = Utils.ParseInt(ddlDatabase.SelectedValue, 0);
            inst.DatabaseName = databaseName.Text;
            inst.UserId = Utils.ParseInt(ddlUser.SelectedValue, 0);
            inst.Username = databaseUser.Text;
            inst.Password = databasePassword.Password;

            // get app settings
            try
            {
                if (settings != null)
                {
                    settings.GetSettings(inst);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("APPINSTALLER_GET_SETTINGS", ex);
            }

            // install application
            try
            {
                int result = ES.Services.ApplicationsInstaller.InstallApplication(inst);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("APPINSTALLER_INSTALL_APP", ex);
                return;
            }

            Response.Redirect(EditUrl("ApplicationID", PanelRequest.ApplicationID, "complete",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void ddlDatabaseGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDatabases();
            BindDatabaseUsers();
            ApplyDatabasePolicy();
        }

        protected void rblDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            InstallApplication();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
    }
}
