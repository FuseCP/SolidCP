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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.Providers.SharePoint;

namespace SolidCP.Portal
{
    public partial class SharePointBackupSite : SolidCPModuleBase
    {
        private const string BACKUP_EXTENSION = ".bsh";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSite();
            }
        }

        private void BindSite()
        {
            try
            {
                SharePointSite site = ES.Services.SharePointServers.GetSharePointSite(PanelRequest.ItemID);
                litSiteName.Text = site.Name;
                txtBackupName.Text = site.Name + BACKUP_EXTENSION;
                fileLookup.SelectedFile = "\\";
                fileLookup.PackageId = site.PackageId;

                BindBackupName();
                ToggleControls();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_GET_SITE", ex);
                return;
            }
        }

        private void BindBackupName()
        {
            string backupName = Path.GetFileNameWithoutExtension(txtBackupName.Text);
            txtBackupName.Text = backupName + (chkZipBackup.Checked ? ".zip" : BACKUP_EXTENSION);
        }

        private void ToggleControls()
        {
            fileLookup.Visible = rbCopy.Checked;
        }

        private void BackupSite()
        {
            try
            {
                string bakFile = ES.Services.SharePointServers.BackupVirtualServer(PanelRequest.ItemID,
                    txtBackupName.Text, chkZipBackup.Checked, rbDownload.Checked, fileLookup.SelectedFile);

                if (rbDownload.Checked && !String.IsNullOrEmpty(bakFile))
                {
                    string fileName = bakFile;

                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileName));
                    Response.ContentType = "application/octet-stream";

                    int FILE_BUFFER_LENGTH = 5000000;
                    byte[] buffer = null;
                    int offset = 0;
                    do
                    {
                        // read remote content
                        buffer = ES.Services.SharePointServers.GetSharePointBackupBinaryChunk(PanelRequest.ItemID, fileName, offset, FILE_BUFFER_LENGTH);

                        // write to stream
                        Response.BinaryWrite(buffer);

                        offset += FILE_BUFFER_LENGTH;
                    }
                    while (buffer.Length == FILE_BUFFER_LENGTH);

                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SHAREPOINT_BACKUP_SITE", ex);
                return;
            }
            RedirectBack();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            BackupSite();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void chkZipBackup_CheckedChanged(object sender, EventArgs e)
        {
            BindBackupName();
        }

        protected void rbDownload_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
