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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;
using SolidCP.WebPortal;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageDriveMaps : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ES.Services.EnterpriseStorage.CheckUsersDomainExists(PanelRequest.ItemID))
                {
                    btnAddDriveMap.Enabled = false;

                    messageBox.ShowWarningMessage("WEB_SITE_IS_NOT_CREATED");
                }
            }
        }

        protected void btnAddDriveMap_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_enterprisestorage_drive_map",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvDriveMaps_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete drive map
                string driveLetter = e.CommandArgument.ToString();

                try
                {
                    ResultObject result = ES.Services.EnterpriseStorage.DeleteMappedDrive(PanelRequest.ItemID, driveLetter);
                    if (!result.IsSuccess)
                    {
                        messageBox.ShowMessage(result, "ENTERPRISE_STORAGE_DRIVE_MAP", "EnterpriseStorage");
                        return;
                    }

                    gvDriveMaps.DataBind();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("ENTERPRISE_STORAGE_DELETE_DRIVE_MAP", ex);
                }
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvDriveMaps.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);

            gvDriveMaps.DataBind();
        }

        protected string GetDriveImage()
        {
            return String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images/Exchange", "/", "net_drive16.png");
        }
    }
}
