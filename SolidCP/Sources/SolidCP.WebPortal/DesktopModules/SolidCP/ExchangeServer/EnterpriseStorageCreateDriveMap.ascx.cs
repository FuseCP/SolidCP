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
using System.Linq;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class EnterpriseStorageCreateDriveMap : SolidCPModuleBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLetters();
                BindFolders();
            }

            if (ddlFolders.Items.Count < 1 || ddlLetters.Items.Count < 1)
            {
                btnCreate.Enabled = false;
            }
        }

        private void BindFolders()
        {
            ddlFolders.DataSource = ES.Services.EnterpriseStorage.GetNotMappedEnterpriseFolders(PanelRequest.ItemID).Select(x=> new {Name = x.Name, Url = x.UncPath ?? x.Url});
            ddlFolders.DataTextField = "Name";
            ddlFolders.DataValueField = "Url";
            ddlFolders.DataBind();

            if (ddlFolders.Items.Count > 0)
            {
                txtLabelAs.Text = ddlFolders.SelectedItem.Text;
                lbFolderUrl.Text = ddlFolders.SelectedItem.Value;
                txtFolderName.Value = ddlFolders.SelectedItem.Text;
            }
        }

        private void BindLetters()
        {
            //for (int i = 65; i < 91; i++) // increment from ASCII values for A-Z
            for (int i = 69; i < 91; i++) // E-Z
            {
                ddlLetters.Items.Add(new ListItem(Convert.ToChar(i).ToString() + ":", Convert.ToChar(i).ToString()));// Add uppercase letters to possible drive letters
            }
            
            //string[] usedLetters = ES.Services.EnterpriseStorage.GetUsedDriveLetters(PanelRequest.ItemID);

            //foreach (string elem in usedLetters)
            //{
            //    ListItem item = new ListItem(elem + ":", elem);
            //    if (ddlLetters.Items.Contains(item))
            //    {
            //        ddlLetters.Items.Remove(item);
            //    }
            //}
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            try
            {
                if (!ES.Services.EnterpriseStorage.CheckEnterpriseStorageInitialization(PanelSecurity.PackageId, PanelRequest.ItemID))
                {
                    ES.Services.EnterpriseStorage.CreateEnterpriseStorage(PanelSecurity.PackageId, PanelRequest.ItemID);
                }

                ResultObject result = ES.Services.EnterpriseStorage.CreateMappedDrive(
                    PanelSecurity.PackageId,
                    PanelRequest.ItemID,
                    ddlLetters.SelectedItem.Value,
                    txtLabelAs.Text,
                    txtFolderName.Value);

                if (!result.IsSuccess && result.ErrorCodes.Count > 0)
                {
                    messageBox.ShowMessage(result, "ENTERPRISE_STORAGE_CREATE_MAPPED_DRIVE", "Cloud Folders");
                    return;
                }

                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "enterprisestorage_drive_maps",
                "SpaceID=" + PanelSecurity.PackageId));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ENTERPRISE_STORAGE_CREATE_MAPPED_DRIVE", ex);
            }
        }
    }
}
