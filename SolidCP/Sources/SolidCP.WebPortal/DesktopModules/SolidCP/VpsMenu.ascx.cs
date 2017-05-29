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
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.WebPortal;
namespace SolidCP.Portal
{
    public partial class VpsMenu : SolidCPModuleBase
    {
        private const string PID_SPACE_VPS = "SpaceVPS2012";
        private const string PID_SPACE_PROXMOX = "SpaceProxmox";
        protected void Page_Load(object sender, EventArgs e)
        {
            // organization
            bool vpsVisible = (Request[DefaultPage.PAGE_ID_PARAM].Equals(PID_SPACE_VPS, StringComparison.InvariantCultureIgnoreCase) ||
                                Request[DefaultPage.PAGE_ID_PARAM].Equals(PID_SPACE_PROXMOX, StringComparison.InvariantCultureIgnoreCase));

            vpsMenu.Visible = vpsVisible;
            if (vpsVisible)
            {
                MenuItem rootItem = new MenuItem(locMenuTitle.Text);
                rootItem.Selectable = false;
                menu.Items.Add(rootItem);
                BindMenu(rootItem.ChildItems);
            }
        }
        virtual public int PackageId
        {
            get { return PanelSecurity.PackageId; }
            set { }
        }
        virtual public int ItemID
        {
            get { return PanelRequest.ItemID; }
            set { }
        }
        private PackageContext cntx = null;
        virtual public PackageContext Cntx
        {
            get
            {
                if (cntx == null) cntx = PackagesHelper.GetCachedPackageContext(PackageId);
                return cntx;
            }
        }
        public void BindMenu(MenuItemCollection items)
        {
            if (PackageId <= 0)
                return;
            // VPS Menu
            if (Cntx.Groups.ContainsKey(ResourceGroups.VPS2012) || Cntx.Groups.ContainsKey(ResourceGroups.Proxmox))
                PrepareVPS2012Menu(items);
        }
        private void PrepareVPS2012Menu(MenuItemCollection vpsItems)
        {
            bool isAdmin = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            // add items
            vpsItems.Add(CreateMenuItem("VPSHome", ""));
            if (((cntx.Quotas.ContainsKey(Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED].QuotaExhausted)
                || (cntx.Quotas.ContainsKey(Quotas.PROXMOX_EXTERNAL_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.PROXMOX_EXTERNAL_NETWORK_ENABLED].QuotaExhausted))
                || (PanelSecurity.PackageId == 1 && isAdmin))
                vpsItems.Add(CreateMenuItem("ExternalNetwork", "vdc_external_network"));
            if (isAdmin)
                vpsItems.Add(CreateMenuItem("ManagementNetwork", "vdc_management_network"));
            if ((cntx.Quotas.ContainsKey(Quotas.VPS2012_PRIVATE_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_PRIVATE_NETWORK_ENABLED].QuotaExhausted)
                || (cntx.Quotas.ContainsKey(Quotas.PROXMOX_PRIVATE_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.PROXMOX_PRIVATE_NETWORK_ENABLED].QuotaExhausted))
                vpsItems.Add(CreateMenuItem("PrivateNetwork", "vdc_private_network"));
            vpsItems.Add(CreateMenuItem("AuditLog", "vdc_audit_log"));
        }
        private MenuItem CreateMenuItem(string text, string key)
        {
            return CreateMenuItem(text, key, null);
        }
        protected virtual MenuItem CreateMenuItem(string text, string key, string img)
        {
            MenuItem item = new MenuItem();
            item.Text = GetLocalizedString("Text." + text);
            var hostModule = GetAllControlsOfType<SolidCPModuleBase>(this.Page);
            if (hostModule.Count > 0)
            {
                item.NavigateUrl = hostModule.LastOrDefault().EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key); // PortalUtils.EditUrl("ItemID", ItemID.ToString(), key, "SpaceID=" + PackageId);
            }
            //if (img == null)
            //    item.ImageUrl = PortalUtils.GetThemedIcon("Icons/tool_48.png");
            //else
            //    item.ImageUrl = PortalUtils.GetThemedIcon(img);
            return item;
        }
        public static List<T> GetAllControlsOfType<T>(Control parent) where T : Control
        {
            var result = new List<T>();
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                {
                    result.Add((T)control);
                }
                if (control.HasControls())
                {
                    result.AddRange(GetAllControlsOfType<T>(control));
                }
            }
            return result;
        }
    }
}