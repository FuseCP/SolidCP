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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class SelectIPAddress : SolidCPControlBase
    {
        private bool allowEmptySelection = true;
        public bool AllowEmptySelection
        {
            get { return allowEmptySelection; }
            set { allowEmptySelection = value; }
        }

        public string SelectValueText { get; set; }

        private bool useAddressValueAsKey = false;
        public bool UseAddressValueAsKey
        {
            get { return useAddressValueAsKey; }
            set { useAddressValueAsKey = value; }
        }

        private int addressId;
        public int AddressId
        {
            get { return Utils.ParseInt(ddlIPAddresses.SelectedValue, 0); }
            set
            {
                addressId = value;
                ListItem li = ddlIPAddresses.Items.FindByValue(addressId.ToString());
                if (li != null)
                {
                    // deselect previous item
                    ddlIPAddresses.SelectedIndex = -1;
                    li.Selected = true;
                }
            }
        }

        private string addressValue;
        public string AddressValue
        {
            get { return ddlIPAddresses.SelectedValue; }
            set
            {
                addressValue = value;
                ListItem li = ddlIPAddresses.Items.FindByValue(addressValue);
                if (li != null)
                {
                    // deselect previous item
                    ddlIPAddresses.SelectedIndex = -1;
                    li.Selected = true;
                }
            }
        }

        private string serverIdParam;
        public string ServerIdParam
        {
            get { return serverIdParam; }
            set { serverIdParam = value; }
        }

        private int serverId = -1;
        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindIPAddresses();
            }
        }

        private void BindIPAddresses()
        {
            IPAddressInfo[] ips = null;

            if (serverIdParam != null || serverId != -1)
            {
                // get addresses by Server
                if (serverIdParam != null)
                    serverId = Utils.ParseInt(Request[serverIdParam], 0);

                ips = ES.Services.Servers.GetIPAddresses(IPAddressPool.General, serverId);
            }
            else
            {
                // get all IP addresses
                ips = ES.Services.Servers.GetIPAddresses(IPAddressPool.None, serverId);
            }

            // bind IP addresses
            ddlIPAddresses.Items.Clear();

            foreach (IPAddressInfo ip in ips)
            {
                string fullIP = ip.ExternalIP;
                if (ip.InternalIP != null &&
                    ip.InternalIP != "" &&
                    ip.InternalIP != ip.ExternalIP)
                    fullIP += " (" + ip.InternalIP + ")";

                string key = ip.AddressId.ToString();
                if (UseAddressValueAsKey)
                {
                    key = ip.ExternalIP + ";" + ip.InternalIP;
                }

                // add list item
                ddlIPAddresses.Items.Add(new ListItem(fullIP, key));
            }

            // add empty item if required
            if (AllowEmptySelection)
            {
                if (SelectValueText == null)
                    SelectValueText = GetLocalizedString("Text.SelectAddress");
                ddlIPAddresses.Items.Insert(0, new ListItem(SelectValueText, ""));
            }

            // select address by ID
            ListItem li = ddlIPAddresses.Items.FindByValue(addressId.ToString());
            if (li != null)
                li.Selected = true;

            // select address by Value
            li = ddlIPAddresses.Items.FindByValue(addressValue);
            if (li != null)
                li.Selected = true;
        }
    }
}
