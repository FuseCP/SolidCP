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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;

namespace SolidCP.Portal.RDS
{
    public partial class RDSEditCollectionUsers : SolidCPModuleBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            users.Module = Module;
            users.OnRefreshClicked -= OnRefreshClicked;
            users.OnRefreshClicked += OnRefreshClicked;

            if (!IsPostBack)
            {
                var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
                litCollectionName.Text = collection.DisplayName;
                BindQuota();
                users.BindUsers();
            }
        }        

        private void BindQuota()
        {
            var quota = GetQuota();

            if (quota != null)
            {
                int rdsUsersCount = ES.Services.RDS.GetOrganizationRdsUsersCount(PanelRequest.ItemID);
                users.ButtonAddEnabled = (!(quota.QuotaAllocatedValue <= rdsUsersCount) || (quota.QuotaAllocatedValue == -1));
            }
        }

        private QuotaValueInfo GetQuota()
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            usersQuota.QuotaUsedValue = stats.CreatedRdsUsers;
            usersQuota.QuotaValue = stats.AllocatedRdsUsers;

            if (stats.AllocatedUsers != -1)
            {
                usersQuota.QuotaAvailable = stats.AllocatedRdsUsers - stats.CreatedRdsUsers;
            }
            
            if (cntx.Quotas.ContainsKey(Quotas.RDS_USERS))
            {
                return cntx.Quotas[Quotas.RDS_USERS];
            }

            return null;
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {           
            ((ModalPopupExtender)asyncTasks.FindControl("ModalPopupProperties")).Hide();
            var users = (List<string>)sender;

            if (users.Any())
            {
                messageBox.Visible = true;
                messageBox.ShowErrorMessage("RDS_USERS_NOT_DELETED", new Exception(string.Join(", ", users)));
            }
            else
            {
                messageBox.Visible = false;
            }
        }

        private bool SaveRdsUsers()
        {
            try
            {
                var quota = GetQuota();
                var rdsUsers = users.GetUsers();

                if (quota.QuotaAllocatedValue == -1 || quota.QuotaAllocatedValue >= rdsUsers.Count())
                {
                    messageBox.Visible = false;
                    ES.Services.RDS.SetUsersToRdsCollection(PanelRequest.ItemID, PanelRequest.CollectionID, users.GetUsers());                
                }   
                else
                {
                    throw new Exception("Too many RDS users added");
                }
            }
            catch (Exception ex)
            {
                messageBox.Visible = true;
                messageBox.ShowErrorMessage("RDS_USERS_NOT_UPDATED", ex);
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

            SaveRdsUsers();
            BindQuota();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (SaveRdsUsers())
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_collections", "SpaceID=" + PanelSecurity.PackageId));
            }
        }
    }
}
