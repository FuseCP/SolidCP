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

using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.RemoteDesktopServices;

namespace SolidCP.Portal.RDS
{
    public partial class RDSEditApplicationUsers : SolidCPModuleBase
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            users.OnRefreshClicked -= OnRefreshClicked;
            users.OnRefreshClicked += OnRefreshClicked;
            users.Module = Module;
            WriteScriptBlock();

            if (!IsPostBack)
            {                
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                var applications = ES.Services.RDS.GetCollectionRemoteApplications(PanelRequest.ItemID, collection.Name);
                var remoteApp = applications.Where(x => x.Alias.Equals(PanelRequest.ApplicationID, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var organizationUsers = ES.Services.Organizations.GetOrganizationUsersPaged(PanelRequest.ItemID, null, null, null, 0, Int32.MaxValue).PageUsers;
                var applicationUsers = ES.Services.RDS.GetApplicationUsers(PanelRequest.ItemID, PanelRequest.CollectionID, remoteApp);

                litCollectionName.Text = collection.Name;                
                txtApplicationName.Text = remoteApp.DisplayName;
                txtCommandLine.Text = remoteApp.RequiredCommandLine;
                //var remoteAppUsers = organizationUsers.Where(x => applicationUsers.Contains(x.AccountName));
                var remoteAppUsers = organizationUsers.Where(x => applicationUsers.Select(a => a.Split('\\').Last().ToLower()).Contains(x.SamAccountName.Split('\\').Last().ToLower()));
                var localAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);

                switch(remoteApp.CommandLineSettings)
                {
                    case CommandLineSettings.Allow:
                        chAllowAny.Checked = true;
                        txtCommandLine.Enabled = false;
                        break;
                    case CommandLineSettings.DoNotAllow:
                        chNotAllow.Checked = true;
                        txtCommandLine.Enabled = false;
                        break;
                    default:
                        chAllow.Checked = true;
                        txtCommandLine.Enabled = true;
                        break;
                }

                foreach(var user in remoteAppUsers)
                {
                    if (localAdmins.Select(l => l.AccountName).Contains(user.AccountName))
                    {
                        user.IsVIP = true;
                    }
                    else
                    {
                        user.IsVIP = false;
                    }
                }

                users.SetUsers(remoteAppUsers.ToArray());
            }
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();            
        }

        private bool SaveApplicationUsers()
        {
            try
            {
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                var applications = ES.Services.RDS.GetCollectionRemoteApplications(PanelRequest.ItemID, collection.Name);
                var remoteApp = applications.Where(x => x.Alias.Equals(PanelRequest.ApplicationID, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                remoteApp.DisplayName = txtApplicationName.Text;
                remoteApp.RequiredCommandLine = txtCommandLine.Text;

                if (chAllowAny.Checked)
                {
                    remoteApp.CommandLineSettings = CommandLineSettings.Allow;
                }
                else if (chAllow.Checked)
                {
                    remoteApp.CommandLineSettings = CommandLineSettings.Require;
                }
                else
                {
                    remoteApp.CommandLineSettings = CommandLineSettings.DoNotAllow;
                }

                //ES.Services.RDS.SetApplicationUsers(PanelRequest.ItemID, PanelRequest.CollectionID, remoteApp, users.GetUsers().Select(x => x.AccountName).ToArray());
                ES.Services.RDS.SetApplicationUsers(PanelRequest.ItemID, PanelRequest.CollectionID, remoteApp, users.GetUsers().Select(x => x.SamAccountName.Split('\\').Last()).ToArray());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("REMOTEAPPUSERS_NOT_UPDATED", ex);

                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            SaveApplicationUsers();            
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveApplicationUsers())
            {
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_collection_edit_apps", "CollectionId=" + PanelRequest.CollectionID, "ItemID=" + PanelRequest.ItemID));
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_collection_edit_apps", "CollectionId=" + PanelRequest.CollectionID, "ItemID=" + PanelRequest.ItemID));
        }

        protected override void OnPreRender(EventArgs e)
        {
            chNotAllow.Attributes["onclick"] = String.Format("EnableCommandLineTextBox('{0}', false);", txtCommandLine.ClientID);
            chAllowAny.Attributes["onclick"] = String.Format("EnableCommandLineTextBox('{0}', false);", txtCommandLine.ClientID);
            chAllow.Attributes["onclick"] = String.Format("EnableCommandLineTextBox('{0}', true);", txtCommandLine.ClientID);
            base.OnPreRender(e);
        }

        private void WriteScriptBlock()
        {
            string scriptKey = "CommandLineScript";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptKey, @"<script language='javascript' type='text/javascript'>                        
                        function EnableCommandLineTextBox(textBoxId, enabled)
                        {                            
                            var textBox = document.getElementById(textBoxId);                            
                            textBox.disabled = !enabled;
                        }

                        </script>");
            }
        }
    }
}
