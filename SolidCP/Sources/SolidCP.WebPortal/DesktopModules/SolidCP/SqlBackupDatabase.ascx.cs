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

using SolidCP.Providers.Database;

namespace SolidCP.Portal
{
    public partial class SqlBackupDatabase : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDatabase();
            }
        }

        private void BindDatabase()
        {
            try
            {
                SqlDatabase database = ES.Services.DatabaseServers.GetSqlDatabase(PanelRequest.ItemID);
                litDatabaseName.Text = database.Name;
                if (database.GroupName.Contains("MySQL"))
                {
                    txtBackupName.Text = database.Name + ".sql";
                }
                if (database.GroupName.Contains("MariaDB"))
                {
                    txtBackupName.Text = database.Name + ".sql";
                }
                else
                {
                    txtBackupName.Text = database.Name + ".bak";
                }
                fileLookup.SelectedFile = "\\";
                fileLookup.PackageId = database.PackageId;

                BindBackupName();
                ToggleControls();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SQL_GET_DATABASE", ex);
                return;
            }
        }

        private void BindBackupName()
        {
            string backupName = Path.GetFileNameWithoutExtension(txtBackupName.Text);
            SqlDatabase database = ES.Services.DatabaseServers.GetSqlDatabase(PanelRequest.ItemID);
            if (database.GroupName.Contains("MySQL"))
            {
                txtBackupName.Text = backupName + (chkZipBackup.Checked ? ".zip" : ".sql");    
            }
            if (database.GroupName.Contains("MariaDB"))
            {
                txtBackupName.Text = backupName + (chkZipBackup.Checked ? ".zip" : ".sql");
            }
            else
            {
                txtBackupName.Text = backupName + (chkZipBackup.Checked ? ".zip" : ".bak");
            }
        }

        private void ToggleControls()
        {
            fileLookup.Visible = rbCopy.Checked;
        }

        private void BackupDatabase()
        {
            try
            {
                string bakFile = ES.Services.DatabaseServers.BackupSqlDatabase(PanelRequest.ItemID,
                    txtBackupName.Text, chkZipBackup.Checked, rbDownload.Checked, fileLookup.SelectedFile);

                if (rbDownload.Checked && !String.IsNullOrEmpty(bakFile))
                {
                    string fileName = bakFile;

                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileName));
                    //Response.AddHeader("Content-Length", files[0].Content.Length.ToString());
                    Response.ContentType = "application/octet-stream";

                    int FILE_BUFFER_LENGTH = 5000000;
                    byte[] buffer = null;
                    int offset = 0;
                    do
                    {
                        // read remote content
                        buffer = ES.Services.DatabaseServers.GetSqlBackupBinaryChunk(PanelRequest.ItemID, fileName, offset, FILE_BUFFER_LENGTH);

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
                ShowErrorMessage("SQL_BACKUP_DATABASE", ex);
                return;
            }
            RedirectBack();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            BackupDatabase();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
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
