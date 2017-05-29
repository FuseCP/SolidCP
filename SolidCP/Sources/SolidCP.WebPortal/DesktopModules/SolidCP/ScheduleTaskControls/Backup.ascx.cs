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
using SolidCP.Portal.UserControls.ScheduleTaskView;

namespace SolidCP.Portal.ScheduleTaskControls
{
	public partial class Backup : EmptyView
	{
		private static readonly string BackupNameParameter = "BACKUP_FILE_NAME";
		private static readonly string StorePackageIdParameter = "STORE_PACKAGE_ID";
		private static readonly string StorePackageFolderParameter = "STORE_PACKAGE_FOLDER";
		private static readonly string StoreServerFolderParameter = "STORE_SERVER_FOLDER";
		private static readonly string DeleteTempBackupParameter = "DELETE_TEMP_BACKUP";

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Sets scheduler task parameters on view.
		/// </summary>
		/// <param name="parameters">Parameters list to be set on view.</param>
		public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
		{
			base.SetParameters(parameters);

			if (PanelSecurity.SelectedUser.Role == UserRole.Administrator
				&& PanelSecurity.PackageId < 2)
				ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("1"));

			if (PanelSecurity.LoggedUser.Role != UserRole.Administrator)
				ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("2"));

			if (PanelSecurity.SelectedUser.Role != UserRole.Administrator)
				chkDeleteBackup.Visible = false;

			string modeValue = "";
			string filePrefix = "";

            if (PanelSecurity.PackageId > 0)
			{
				// load a single package
				//PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                PackageInfo[] packages = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUser.UserId);

                foreach (PackageInfo package in packages)
                    ddlSpace.Items.Add(new ListItem(package.PackageName, package.PackageId.ToString()));

			    modeValue = ddlSpace.SelectedValue;
				filePrefix = "SpaceBackup";
			}
			else if (PanelRequest.ServiceId > 0)
			{
				ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("1"));

				ServiceInfo service = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);

				modeValue = service.ServiceName;
				filePrefix = "ServiceBackup";
			}
			else if (PanelRequest.ServerId > 0)
			{
				ddlDestination.Items.Remove(ddlDestination.Items.FindByValue("1"));

				ServerInfo server = ES.Services.Servers.GetServerById(PanelRequest.ServerId);

				modeValue = server.ServerName;
				filePrefix = "ServerBackup";
			}
			else if (PanelSecurity.SelectedUserId > 0)
			{
				// load user spaces
				PackageInfo[] packages = ES.Services.Packages.GetMyPackages(PanelSecurity.SelectedUserId);
				foreach (PackageInfo package in packages)
					ddlSpace.Items.Add(new ListItem(package.PackageName, package.PackageId.ToString()));

				modeValue = PanelSecurity.SelectedUser.Username;
				filePrefix = "UserBackup";
			}


			ToggleFormControls();
			InitFolderBrowser();


			this.SetParameter(this.txtBackupFileName, BackupNameParameter);
			if (String.IsNullOrEmpty(this.txtBackupFileName.Text))
			{
				// backup file
				txtBackupFileName.Text = String.Format("{0}-{1}-{2}.scpak", filePrefix,
					Regex.Replace(modeValue, "[^\\w]", "_"),
					DateTime.Now.ToString("ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture));
			}

			ScheduleTaskParameterInfo storePackageId = this.FindParameterById(StorePackageIdParameter);
			if (String.IsNullOrEmpty(storePackageId.ParameterValue))
			{
				// Select server folder as destination.
				this.ddlDestination.SelectedIndex = 1;
				this.ToggleFormControls();
			}

			Utils.SelectListItem(this.ddlSpace, storePackageId.ParameterValue);

			ScheduleTaskParameterInfo storePackageFolder = this.FindParameterById(StorePackageFolderParameter);
			this.spaceFolder.SelectedFile = storePackageFolder.ParameterValue;

			this.SetParameter(this.txtServerPath, StoreServerFolderParameter);
			this.SetParameter(this.chkDeleteBackup, DeleteTempBackupParameter);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public override ScheduleTaskParameterInfo[] GetParameters()
		{
			ScheduleTaskParameterInfo backupName = this.GetParameter(this.txtBackupFileName, BackupNameParameter);
			ScheduleTaskParameterInfo storePackageId = this.GetParameter(this.ddlSpace, StorePackageIdParameter);
			
			ScheduleTaskParameterInfo storePackageFolder = new ScheduleTaskParameterInfo();
			storePackageFolder.ParameterId = StorePackageFolderParameter;
			storePackageFolder.ParameterValue = this.spaceFolder.SelectedFile;

			ScheduleTaskParameterInfo storeServerFolder = this.GetParameter(this.txtServerPath, StoreServerFolderParameter);
			ScheduleTaskParameterInfo deleteTempBackup = this.GetParameter(this.chkDeleteBackup, DeleteTempBackupParameter);

			return new ScheduleTaskParameterInfo[5] { backupName, storePackageId, storePackageFolder, storeServerFolder, deleteTempBackup };
		}

		private void ToggleFormControls()
		{
			this.SpaceFolderPanel.Visible = (ddlDestination.SelectedValue == "1");
			this.ServerFolderPanel.Visible = (ddlDestination.SelectedValue == "2");
		}

		private void InitFolderBrowser()
		{
			int packageId = Utils.ParseInt(ddlSpace.SelectedValue, -1);
			if (packageId != -1)
				spaceFolder.PackageId = packageId;
		}

		protected void ddlDestination_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToggleFormControls();
		}

		protected void ddlSpace_SelectedIndexChanged(object sender, EventArgs e)
		{
			InitFolderBrowser();
		}
	}
}
