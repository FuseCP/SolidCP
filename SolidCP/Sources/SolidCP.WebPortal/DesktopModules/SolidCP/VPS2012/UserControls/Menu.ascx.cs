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
namespace SolidCP.Portal.VPS2012.UserControls
{
    public partial class Menu : SolidCPControlBase
    {
        public class MenuItem
        {
            private string url;
            private string text;
            private string key;
            public string Url
            {
                get { return url; }
                set { url = value; }
            }
            public string Text
            {
                get { return text; }
                set { text = value; }
            }
            public string Key
            {
                get { return key; }
                set { key = value; }
            }
        }
        private string selectedItem;
        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindMenu();
        }
        private void BindMenu()
        {
            bool isAdmin = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            // build the list of menu items
            List<MenuItem> items = new List<MenuItem>();
            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            // add items
            items.Add(CreateMenuItem("Vps", ""));
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED].QuotaExhausted
                || (PanelSecurity.PackageId == 1 && isAdmin))
                items.Add(CreateMenuItem("ExternalNetwork", "vdc_external_network"));
            if (isAdmin)
                items.Add(CreateMenuItem("ManagementNetwork", "vdc_management_network"));
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_PRIVATE_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_PRIVATE_NETWORK_ENABLED].QuotaExhausted)
                items.Add(CreateMenuItem("PrivateNetwork", "vdc_private_network"));
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_DMZ_NETWORK_ENABLED)
                && !cntx.Quotas[Quotas.VPS2012_DMZ_NETWORK_ENABLED].QuotaExhausted)
                items.Add(CreateMenuItem("DmzNetwork", "vdc_dmz_network"));
            //items.Add(CreateMenuItem("UserPermissions", "vdc_permissions"));
            items.Add(CreateMenuItem("AuditLog", "vdc_audit_log"));
            // selected menu item
            for (int i = 0; i < items.Count; i++)
            {
                if (String.Compare(items[i].Key, SelectedItem, true) == 0)
                {
                    MenuItems.SelectedIndex = i;
                    break;
                }
            }
            // bind items
            MenuItems.DataSource = items;
            MenuItems.DataBind();
        }
        private MenuItem CreateMenuItem(string text, string key)
        {
            MenuItem item = new MenuItem();
            item.Key = key;
            item.Text = GetLocalizedString("Text." + text);
            item.Url = HostModule.EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key);
            return item;
        }
    }
}