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
using System.Text;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;
using SolidCP.WebPortal;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageFolders : SolidCPModuleBase
    {
        #region Constants

        private const int OneGb = 1024;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ES.Services.EnterpriseStorage.CheckUsersDomainExists(PanelRequest.ItemID))
                {
                    BindEnterpriseStorageStats();
                    RegisterStatusScript();
                    hdnItemId.Value = PanelRequest.ItemID.ToString();

                    gvFolders.DataBound -= OnDataBound;
                    gvFolders.DataBound += OnDataBound;
                }
                else
                {
                    btnAddFolder.Enabled = false;

                    messageBox.ShowWarningMessage("WEB_SITE_IS_NOT_CREATED");
                }
            }
        }

        private void RegisterStatusScript()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ESAjaxQuery"))
            {
                var builder = new StringBuilder();
                builder.AppendLine("function getFolderData() {");
                builder.AppendFormat("var hidden = document.getElementById('{0}').value;", hdnGridState.ClientID);
                builder.AppendFormat("var grid = document.getElementById('{0}');", gvFolders.ClientID);
                builder.AppendFormat("var itemId = document.getElementById('{0}').value;", hdnItemId.ClientID);
                builder.AppendLine("if (hidden === 'True'){");
                builder.AppendFormat("$('#{0}').val('false');", hdnGridState.ClientID);
                builder.AppendLine("for (i = 1; i < grid.rows.length; i++) {");
                builder.AppendLine("var folderName = grid.rows[i].cells[0].children[0].value;");
                builder.AppendLine("$.ajax({");
                builder.AppendLine("type: 'post',");
                builder.AppendLine("dataType: 'json',");
                builder.AppendLine("data: { folderName: folderName, itemIndex: i, itemId: itemId },");
                builder.AppendLine("url: 'EnterpriseFolderDataHandler.ashx',");
                builder.AppendLine("success: function (data) {");
                builder.AppendLine("var array = data.split(':');");
                builder.AppendLine("var usage = array[0];");
                builder.AppendLine("var index = array[1];");
                builder.AppendLine("var driveLetter = array[2];");
                builder.AppendLine("grid.rows[index].cells[3].childNodes[0].data = usage;");
                builder.AppendLine("var driveImage = grid.rows[index].cells[5].children[0];");
                builder.AppendLine("driveImage.style.display = driveLetter.length < 3 ? 'inline' : 'none';");
                builder.AppendLine("grid.rows[index].cells[5].childNodes[2].data = ' ' + driveLetter +':';");
                builder.AppendLine("}");
                builder.AppendLine("}");
                builder.AppendLine(")}");
                builder.AppendLine("}");
                builder.AppendLine("}");

                Page.ClientScript.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));
                Page.ClientScript.RegisterClientScriptBlock(typeof(EnterpriseStorageFolders), "ESAjaxQuery", builder.ToString(), true);
            }
        }


        private void OnDataBound(object sender, EventArgs e)
        {
            if (gvFolders.Rows.Count > 0)
            {
                hdnGridState.Value = true.ToString();
            }
        }

        public string GetFolderEditUrl(string folderName)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "enterprisestorage_folder_settings",
                    "FolderID=" + folderName,
                    "ItemID=" + PanelRequest.ItemID);
        }

        public static decimal ConvertMBytesToGB(object size)
        {
            return Math.Round(Convert.ToDecimal(size) / OneGb, 2);
        }

        protected void BindEnterpriseStorageStats()
        {
            btnAddFolder.Enabled = true;

            OrganizationStatistics organizationStats = ES.Services.EnterpriseStorage.GetStatisticsByOrganization(PanelRequest.ItemID);

            foldersQuota.QuotaUsedValue = organizationStats.CreatedEnterpriseStorageFolders;
            foldersQuota.QuotaValue = organizationStats.AllocatedEnterpriseStorageFolders;

            spaceAvailableQuota.QuotaUsedValue = organizationStats.UsedEnterpriseStorageSpace;
            spaceAvailableQuota.QuotaValue = organizationStats.AllocatedEnterpriseStorageSpace;

            spaceQuota.QuotaValue = (int)Math.Round(ConvertMBytesToGB(organizationStats.UsedEnterpriseStorageSpace), 0);

            if (organizationStats.AllocatedEnterpriseStorageFolders != -1)
            {
                int folderAvailable = foldersQuota.QuotaAvailable = organizationStats.AllocatedEnterpriseStorageFolders - organizationStats.CreatedEnterpriseStorageFolders;

                if (folderAvailable <= 0)
                {
                    btnAddFolder.Enabled = false;
                }
            }

            if (organizationStats.AllocatedEnterpriseStorageSpace != -1)
            {
                int spaceAvailable = spaceAvailableQuota.QuotaAvailable = organizationStats.AllocatedEnterpriseStorageSpace - organizationStats.UsedEnterpriseStorageSpace;

                if (spaceAvailable <= 0)
                {
                    btnAddFolder.Enabled = false;
                }
            }
        }

        protected void btnAddFolder_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_enterprisestorage_folder",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvFolders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete folder
                string folderName = e.CommandArgument.ToString();

                try
                {
                    ResultObject result = ES.Services.EnterpriseStorage.DeleteEnterpriseFolder(PanelRequest.ItemID, folderName);
                    if (!result.IsSuccess)
                    {
                        messageBox.ShowMessage(result, "ENTERPRISE_STORAGE_FOLDER", "EnterpriseStorage");
                        return;
                    }

                    gvFolders.DataBind();
                    
                    BindEnterpriseStorageStats();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ENTERPRISE_STORAGE_DELETE_FOLDER", ex);
                }
            }
        }

        protected void odsSecurityGroupsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_GET_SECURITY_GROUP", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvFolders.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);

            gvFolders.DataBind();
        }

        protected string GetDriveImage()
        {
            return String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images/Exchange", "/", "net_drive16.png");
        }
    }
}
