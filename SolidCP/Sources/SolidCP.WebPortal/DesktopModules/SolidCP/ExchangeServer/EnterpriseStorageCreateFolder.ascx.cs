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
using System.Text.RegularExpressions;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageCreateFolder : SolidCPModuleBase
    {
        #region Constants

        private const int OneGb = 1024;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ES.Services.EnterpriseStorage.CheckUsersDomainExists(PanelRequest.ItemID))
                {
                    Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folders",
                        "ItemID=" + PanelRequest.ItemID));
                }

                OrganizationStatistics organizationStats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);

                if (organizationStats.AllocatedEnterpriseStorageSpace != -1)
                {
                    rangeFolderSize.MaximumValue = Math.Round((organizationStats.AllocatedEnterpriseStorageSpace - (decimal)organizationStats.UsedEnterpriseStorageSpace) / OneGb
                        + Utils.ParseDecimal(txtFolderSize.Text, 0), 2).ToString();
                    rangeFolderSize.ErrorMessage = string.Format("The quota you've entered exceeds the available quota for organization ({0}Gb)", rangeFolderSize.MaximumValue);
                }

                if (organizationStats.AllocatedGroups != -1)
                {
                    int groupsAvailable = organizationStats.AllocatedGroups - organizationStats.CreatedGroups;

                    if (groupsAvailable <= 0)
                    {
                        chkAddDefaultGroup.Enabled = false;
                    }
                }
            }
        }


        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            try
            {
                if (!EnterpriseStorageHelper.ValidateFolderName(txtFolderName.Text))
                {
                    messageBox.ShowErrorMessage("FILES_INCORRECT_FOLDER_NAME");

                    return;
                }

                if (!ES.Services.EnterpriseStorage.CheckEnterpriseStorageInitialization(PanelSecurity.PackageId, PanelRequest.ItemID))
                {
                    ES.Services.EnterpriseStorage.CreateEnterpriseStorage(PanelSecurity.PackageId, PanelRequest.ItemID);
                }

                ResultObject result = ES.Services.EnterpriseStorage.CreateEnterpriseFolder(
                    PanelRequest.ItemID,
                    txtFolderName.Text,
                    (int)(decimal.Parse(txtFolderSize.Text) * OneGb),
                    rbtnQuotaSoft.Checked ? QuotaType.Soft : QuotaType.Hard,
                    chkAddDefaultGroup.Checked);

                if (!result.IsSuccess && result.ErrorCodes.Count > 0)
                {
                    messageBox.ShowMessage(result, "ENTERPRISE_STORAGE_CREATE_FOLDER", "Cloud Folders");

                    return;
                }

                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folder_settings",
                    "FolderID=" + txtFolderName.Text,
                    "ItemID=" + PanelRequest.ItemID));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_CREATE_FOLDER", ex);
            }
        }
    }
}
