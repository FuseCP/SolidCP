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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;

namespace SolidCP.Portal.UserControls
{
    public partial class AllocatePackagePhoneNumbers : SolidCPModuleBase
    {
        private IPAddressPool pool;
        public IPAddressPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        private string listAddressesControl;
        public string ListAddressesControl
        {
            get { return listAddressesControl; }
            set { listAddressesControl = value; }
        }

        private string resourceGroup;
        public string ResourceGroup
        {
            get { return resourceGroup; }
            set { resourceGroup = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindIPAddresses();
                ToggleControls();
            }
        }

        private void BindIPAddresses()
        {
            // bind list
            IPAddressInfo[] ips = ES.Services.Servers.GetUnallottedIPAddresses(PanelSecurity.PackageId, ResourceGroup, Pool);
            foreach (IPAddressInfo ip in ips)
            {
                string txt = ip.ExternalIP;

                if (!String.IsNullOrEmpty(ip.InternalIP))
                    txt += "/" + ip.InternalIP;

                listExternalAddresses.Items.Add(new ListItem(txt, ip.AddressId.ToString()));
            }

            int quotaAllowed = -1;
            string quotaName = Quotas.LYNC_PHONE;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(quotaName))
            {
                int quotaAllocated = cntx.Quotas[quotaName].QuotaAllocatedValue;
                //int quotaUsed = cntx.Quotas[quotaName].QuotaUsedValue;

                int quotaUsed = ES.Services.Servers.GetPackageIPAddressesCount(PanelSecurity.PackageId, PanelRequest.ItemID, IPAddressPool.PhoneNumbers);

                if (quotaAllocated != -1)
                    quotaAllowed = quotaAllocated - quotaUsed;
            }

            // bind controls
            int max = quotaAllowed == -1 ? listExternalAddresses.Items.Count : Math.Min(quotaAllowed, listExternalAddresses.Items.Count);

            txtExternalAddressesNumber.Text = max.ToString();
            litMaxAddresses.Text = String.Format(GetLocalizedString("litMaxAddresses.Text"), max);

            if (max == 0)
            {
                AddressesTable.Visible = false;
                ErrorMessagesList.Visible = true;
                EmptyAddressesMessage.Visible = (listExternalAddresses.Items.Count == 0);
                QuotaReachedMessage.Visible = (quotaAllowed == 0);
                btnAdd.Enabled = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> ids = new List<int>();
                foreach (ListItem item in listExternalAddresses.Items)
                {
                    if (item.Selected)
                        ids.Add(Utils.ParseInt(item.Value));
                }

                ResultObject res = ES.Services.Servers.AllocatePackageIPAddresses(PanelSecurity.PackageId,
                                                     PanelRequest.ItemID,
                                                     ResourceGroup, Pool,
                                                     radioExternalRandom.Checked,
                                                     Utils.ParseInt(txtExternalAddressesNumber.Text),
                                                     ids.ToArray());
                if (res.IsSuccess)
                {
                    // return back
                    Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ListAddressesControl,
                                    PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
                }
                else
                {
                    // show message
                    messageBox.ShowMessage(res, "VPS_ALLOCATE_EXTERNAL_ADDRESSES_ERROR", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ALLOCATE_EXTERNAL_ADDRESSES_ERROR", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ListAddressesControl,
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId));
        }

        protected void radioExternalSelected_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            AddressesNumberRow.Visible = radioExternalRandom.Checked;
            AddressesListRow.Visible = radioExternalSelected.Checked;
        }

        protected void radioExternalRandom_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
