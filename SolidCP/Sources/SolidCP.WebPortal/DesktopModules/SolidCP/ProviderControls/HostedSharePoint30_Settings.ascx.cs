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

namespace SolidCP.Portal.ProviderControls
{
    public partial class HostedSharePoint30_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindSettings(StringDictionary settings)
        {
            this.txtRootWebApplication.Text = settings["RootWebApplicationUri"];
            int selectedAddressid = this.FindAddressByText(settings["RootWebApplicationIpAddress"]);
            this.ddlRootWebApplicationIpAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0;
            chkLocalHostFile.Checked = Utils.ParseBool(settings["LocalHostFile"], false);
            this.txtSharedSSLRoot.Text = settings["SharedSSLRoot"];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["RootWebApplicationUri"] = this.txtRootWebApplication.Text;
            settings["LocalHostFile"] = chkLocalHostFile.Checked.ToString();
            settings["RootWebApplicationInteralIpAddress"] = String.Empty;
            settings["SharedSSLRoot"] = this.txtSharedSSLRoot.Text;

            if (ddlRootWebApplicationIpAddress.AddressId > 0)
            {
                IPAddressInfo address = ES.Services.Servers.GetIPAddress(ddlRootWebApplicationIpAddress.AddressId);
                if (String.IsNullOrEmpty(address.ExternalIP))
                {
                    settings["RootWebApplicationIpAddress"] = address.InternalIP;
                }
                else
                {
                    settings["RootWebApplicationIpAddress"] = address.ExternalIP;
                }

                if (!String.IsNullOrEmpty(address.InternalIP))
                    settings["RootWebApplicationInteralIpAddress"] = address.InternalIP;
            }
            else
            {
                settings["RootWebApplicationIpAddress"] = String.Empty;
            }

        }

        private int FindAddressByText(string address)
        {
            foreach (IPAddressInfo addressInfo in ES.Services.Servers.GetIPAddresses(IPAddressPool.General, PanelRequest.ServerId))
            {
                if (addressInfo.InternalIP == address || addressInfo.ExternalIP == address)
                {
                    return addressInfo.AddressId;
                }
            }
            return 0;
        }
    }
}
