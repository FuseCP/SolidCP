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
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.EnterpriseServer.Base.RDS;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;
using SolidCP.Providers.RemoteDesktopServices;
using SolidCP.WebPortal;

namespace SolidCP.Portal.RDS
{
    public partial class RDSCollections : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            if (!IsPostBack)
            {
                BindQuota(cntx);
            }
            
            if (cntx.Quotas.ContainsKey(Quotas.RDS_COLLECTIONS))
            {
                btnAddCollection.Enabled = (!(cntx.Quotas[Quotas.RDS_COLLECTIONS].QuotaAllocatedValue <= gvRDSCollections.Rows.Count) || (cntx.Quotas[Quotas.RDS_COLLECTIONS].QuotaAllocatedValue == -1));
            }

            var serviceId = ES.Services.RDS.GetRemoteDesktopServiceId(PanelRequest.ItemID);
            var settings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettingsRDS(serviceId));
            
            var allowImport = Convert.ToBoolean(settings[RdsServerSettings.ALLOWCOLLECTIONSIMPORT]);

            if (!allowImport)
            {
                btnImportCollection.Visible = false;
            }
        }



        private void BindQuota(PackageContext cntx)
        {            
            OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatisticsByOrganization(PanelRequest.ItemID);
            collectionsQuota.QuotaUsedValue = stats.CreatedRdsCollections;
            collectionsQuota.QuotaValue = stats.AllocatedRdsCollections;

            if (stats.AllocatedUsers != -1)
            {
                collectionsQuota.QuotaAvailable = stats.AllocatedRdsCollections - stats.CreatedRdsCollections;
            }            
        }

        public string GetServerName(string collectionId)
        {
            int id = int.Parse(collectionId);

            RdsServer[] servers =  ES.Services.RDS.GetCollectionRdsServers(id);

            return servers.FirstOrDefault().FqdName;
        }

        protected void btnAddCollection_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_create_collection",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void btnImportCollection_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "rds_import_collection",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        protected void gvRDSCollections_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                // delete RDS Collection
                int rdsCollectionId = int.Parse(e.CommandArgument.ToString());

                try
                {
                    RdsCollection collection = ES.Services.RDS.GetRdsCollection(rdsCollectionId);

                    ResultObject result = ES.Services.RDS.RemoveRdsCollection(PanelRequest.ItemID, collection);

                    if (!result.IsSuccess)
                    {
                        messageBox.ShowMessage(result, "REMOTE_DESKTOP_SERVICES_REMOVE_COLLECTION", "RDS");
                        return;
                    }

                    gvRDSCollections.DataBind();

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("REMOTE_DESKTOP_SERVICES_REMOVE_COLLECTION", ex);
                }
            }
            else if (e.CommandName == "EditCollection")
            {
                Response.Redirect(GetCollectionEditUrl(e.CommandArgument.ToString()));
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvRDSCollections.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);

            gvRDSCollections.DataBind();
        }

        public string GetCollectionEditUrl(string collectionId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_edit_collection", "CollectionId=" + collectionId, "ItemID=" + PanelRequest.ItemID);
        }

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }
    }
}
