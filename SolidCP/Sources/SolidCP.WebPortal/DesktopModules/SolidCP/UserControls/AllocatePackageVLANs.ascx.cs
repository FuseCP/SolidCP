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
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;

namespace SolidCP.Portal.UserControls
{
    public partial class AllocatePackageVLANs : SolidCPControlBase
    {
        private string listVLANsControl;
        public string ListVLANsControl
        {
            get { return listVLANsControl; }
            set { listVLANsControl = value; }
        }

        private string resourceGroup;
        public string ResourceGroup
        {
            get { return resourceGroup; }
            set { resourceGroup = value; }
        }

        private bool isDmz;
        public bool IsDmz
        {
            get { return isDmz; }
            set { isDmz = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVLANs();
                ToggleControls();
            }
        }

        private void BindVLANs()
        {
            // bind list
            VLANInfo[] vlans = ES.Services.Servers.GetUnallottedVLANs(PanelSecurity.PackageId, ResourceGroup);
            foreach (VLANInfo vlan in vlans)
            {
                listVLANs.Items.Add(new ListItem(vlan.Vlan.ToString(), vlan.VlanId.ToString()));
            }

            int quotaAllowed = -1;
            string quotaName;
            if (isDmz)
            {
                quotaName = Quotas.VPS2012_DMZ_VLANS_NUMBER;
            }
            else
            {
                quotaName = Quotas.VPS2012_PRIVATE_VLANS_NUMBER;
            }

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(quotaName))
            {
                int quotaAllocated = cntx.Quotas[quotaName].QuotaAllocatedValue;
                int quotaUsed = cntx.Quotas[quotaName].QuotaUsedValue;

                if (quotaAllocated != -1)
                    quotaAllowed = quotaAllocated - quotaUsed;
            }

            // bind controls
            int max = quotaAllowed == -1 ? listVLANs.Items.Count : quotaAllowed;

            txtVLANsNumber.Text = max.ToString();
            listVLANs.Text = String.Format(GetLocalizedString("litMaxVLANs.Text"), max);

            if (max <= 0)
            {
                VLANsTable.Visible = false;
                ErrorMessagesList.Visible = true;
                EmptyVLANsMessage.Visible = (listVLANs.Items.Count == 0);
                QuotaReachedMessage.Visible = (quotaAllowed <= 0);
                btnAdd.Enabled = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> ids = new List<int>();
                foreach (ListItem item in listVLANs.Items)
                {
                    if (item.Selected)
                        ids.Add(Utils.ParseInt(item.Value));
                }

                ResultObject res = ES.Services.Servers.AllocatePackageVLANs(PanelSecurity.PackageId, ResourceGroup, 
                    radioVLANRandom.Checked, Utils.ParseInt(txtVLANsNumber.Text), ids.ToArray(), isDmz);

                if (res.IsSuccess)
                {
                    // return back
                    if (isDmz)
                    {
                        Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_dmz_network"));
                    }
                    else
                    {
                        Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_private_network"));
                    }
                }
                else
                {
                    // show message
                    messageBox.ShowMessage(res, "VPS_ALLOCATE_PRIVATE_VLANS_ERROR", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ALLOCATE_PRIVATE_VLANS_ERROR", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (isDmz)
            {
                Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_dmz_network"));
            }
            else
            {
                Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "vdc_private_network"));
            }
        }

        protected void radioVLANSelected_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            VLANsNumberRow.Visible = radioVLANRandom.Checked;
            VLANsListRow.Visible = radioVLANSelected.Checked;
        }

        protected void radioVLANRandom_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}