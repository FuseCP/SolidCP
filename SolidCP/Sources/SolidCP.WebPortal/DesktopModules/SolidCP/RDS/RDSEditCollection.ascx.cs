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
    public partial class RDSEditCollection : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            servers.Module = Module;
            servers.OnRefreshClicked -= OnRefreshClicked;
            servers.OnRefreshClicked += OnRefreshClicked;

            if (!Page.IsPostBack)
            {
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                litCollectionName.Text = collection.DisplayName;                
            }
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            var rdsServers = (List<RdsServer>)sender;

            foreach (var rdsServer in rdsServers)
            {
                rdsServer.Status = ES.Services.RDS.GetRdsServerStatus(PanelRequest.ItemID, rdsServer.FqdName);
            }

            servers.BindServers(rdsServers.ToArray());
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
        }

        private bool SaveRdsServers(bool exit = false)
        {
            try
            {
                if (servers.GetServers().Count < 1)
                {
                    messageBox.ShowErrorMessage("RDS_CREATE_COLLECTION_RDSSERVER_REQUAIRED");
                    return false;
                }

                RdsCollection collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                collection.Servers = servers.GetServers();

                ES.Services.RDS.EditRdsCollection(PanelRequest.ItemID, collection);

                if (!exit)
                {
                    foreach(var rdsServer in collection.Servers)
                    {
                        rdsServer.Status = ES.Services.RDS.GetRdsServerStatus(PanelRequest.ItemID, rdsServer.FqdName);
                    }

                    servers.BindServers(collection.Servers.ToArray());
                }
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage(ex.Message);
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            SaveRdsServers();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (SaveRdsServers())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}
