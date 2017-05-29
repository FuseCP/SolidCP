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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using System.Collections.Generic;

namespace SolidCP.Portal
{
    public partial class BackupWizard : SolidCPModuleBase
    {
		private KeyValueBunch backupSetContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // save return page url
                if (Request.UrlReferrer != null)
                    ViewState["ReturnURL"] = Request.UrlReferrer.ToString();

                // bind form
                BindForm();
            }
        }

        private void BindForm()
        {
            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator
                && PanelSecurity.PackageId < 2)
                ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("1"));

            if (PanelSecurity.LoggedUser.Role != UserRole.Administrator)
                ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("2"));

			if (PanelSecurity.SelectedUser.Role != UserRole.Administrator)
				chkDeleteBackup.Visible = false;

            string modeText = "{0}";
            string modeValue = "";
            string filePrefix = "";

			int userId		= PanelSecurity.SelectedUserId, 
				packageId	= PanelSecurity.PackageId,
				serviceId	= PanelRequest.ServiceId,
				serverId	= PanelRequest.ServerId;

            if (PanelSecurity.PackageId > 0)
            {
                // load a single package
                PackageInfo backupPackage = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);

                // load "store" packages
                PackageInfo[] packages = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUser.UserId);
                foreach (PackageInfo package in packages)
                    ddlSpace.Items.Add(new ListItem(package.PackageName, package.PackageId.ToString()));
                ddlSpace.SelectedValue = PanelSecurity.PackageId.ToString();

                modeText = "Text.SpaceBackupMode";
                modeValue = backupPackage.PackageName;
                filePrefix = "SpaceBackup";
            }
            else if (PanelRequest.ServiceId > 0)
            {
                ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("1"));

                ServiceInfo service = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);

                modeText = "Text.ServiceBackupMode";
                modeValue = service.ServiceName;
                filePrefix = "ServiceBackup";
            }
            else if (PanelRequest.ServerId > 0)
            {
                ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("1"));

                ServerInfo server = ES.Services.Servers.GetServerById(PanelRequest.ServerId);

                modeText = "Text.ServerBackupMode";
                modeValue = server.ServerName;
                filePrefix = "ServerBackup";
            }
            else if (PanelSecurity.SelectedUserId > 0)
            {
                // load user spaces
                PackageInfo[] packages = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUserId);
                foreach (PackageInfo package in packages)
                    ddlSpace.Items.Add(new ListItem(package.PackageName, package.PackageId.ToString()));

                modeText = "Text.UserBackupMode";
                modeValue = PanelSecurity.SelectedUser.Username;
                filePrefix = "UserBackup";
            }
			//
			backupSetContent = ES.Services.Backup.GetBackupContentSummary(userId, packageId, serviceId, serverId);
			//
			rptBackupSetSummary.DataSource = backupSetContent.GetAllKeys();
			rptBackupSetSummary.DataBind();

            // backup type
            litBackupType.Text = String.Format(GetLocalizedString(modeText), modeValue);

            // backup file
            txtBackupFileName.Text = String.Format("{0}-{1}-{2}.scpak", filePrefix,
                Regex.Replace(modeValue, "[^\\w]", "_"),
                DateTime.Now.ToString("ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture));

            ToggleFormControls();
            InitFolderBrowser();
        }

        private void ToggleFormControls()
        {
            SpaceFolderPanel.Visible = (ddlDestination.SelectedValue == "1");
            ServerFolderPanel.Visible = (ddlDestination.SelectedValue == "2");
        }

        private void InitFolderBrowser()
        {
            int packageId = Utils.ParseInt(ddlSpace.SelectedValue, -1);
            if (packageId != -1)
                spaceFolder.PackageId = packageId;
        }

        private void Backup()
        {
            if(!Page.IsValid)
                return;

            int userId = 0;
            int packageId = 0;
            int serviceId = 0;
            int serverId = 0;

            packageId = PanelSecurity.PackageId;
            if (packageId == -1)
                userId = PanelSecurity.SelectedUserId;

            serviceId = PanelRequest.ServiceId;
            if (serviceId == 0)
                serverId = PanelRequest.ServerId;

            // perform backup
            try
            {
                int result = ES.Services.Backup.Backup(true, TaskID, userId, packageId, serviceId, serverId,
                    txtBackupFileName.Text.Trim(),
                    Utils.ParseInt(ddlSpace.SelectedValue, 0),
                    spaceFolder.SelectedFile,
                    txtServerPath.Text.Trim(),
                    chkDeleteBackup.Checked);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("BACKUP_WIZARD", ex);
                return;
            }

			// show progress dialog
			AsyncTaskID = TaskID;
			AsyncTaskTitle = GetLocalizedString("Text.BackupItems");
        }

		public List<string> GetResourceGroupServiceItems(string groupName)
		{
			// Ensure the group and its data exist
			if (String.IsNullOrEmpty(groupName)
				|| String.IsNullOrEmpty(backupSetContent[groupName]))
			{
				return new List<string>();
			}
			//
			return new List<string>(backupSetContent[groupName].Split(','));
		}

        protected void ddlDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleFormControls();
        }

        protected void ddlSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitFolderBrowser();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            Backup();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            if (ViewState["ReturnURL"] != null)
                Response.Redirect((string)ViewState["ReturnURL"]);
            else
                RedirectToBrowsePage();
        }
    }
}
