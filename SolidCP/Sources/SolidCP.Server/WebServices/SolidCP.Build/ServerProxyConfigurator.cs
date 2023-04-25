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
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Microsoft.Web.Services3;
//using Microsoft.Web.Services3.Design;

using SolidCP.Providers;

namespace SolidCP.Server.Client
{
    public class ServerProxyConfigurator
    {
        private int timeout = -1;
        private string serverUrl = string.Empty;
        private string serverPassword = string.Empty;
        RemoteServerSettings serverSettings = new RemoteServerSettings();
        ServiceProviderSettings providerSettings = new ServiceProviderSettings();

        public SolidCP.Providers.RemoteServerSettings ServerSettings
        {
            get { return this.serverSettings; }
            set { this.serverSettings = value; }
        }

        public SolidCP.Providers.ServiceProviderSettings ProviderSettings
        {
            get { return this.providerSettings; }
            set { this.providerSettings = value; }
        }

        public string ServerUrl
        {
            get { return this.serverUrl; }
            set { this.serverUrl = value; }
        }

        public string ServerPassword
        {
            get { return this.serverPassword; }
            set { this.serverPassword = value; }
        }

        public int Timeout
        {
            get { return this.timeout; }
            set { this.timeout = value; }
        }

        public void Configure(SolidCP.Web.Client.ClientBase proxy)
        {
            // configure proxy URL
            if (!String.IsNullOrEmpty(serverUrl))
            {
                if (serverUrl.EndsWith("/"))
                    serverUrl = serverUrl.Substring(0, serverUrl.Length - 1);

                proxy.Url = serverUrl; // + proxy.Url.Substring(proxy.Url.LastIndexOf('/'));
            }

            // set proxy timeout
            proxy.Timeout = (timeout == -1) ? null : TimeSpan.FromMilliseconds(timeout * 1000);

            // setup security assertion
            if (!String.IsNullOrEmpty(serverPassword) && proxy.IsAuthenticated)
            {
                //ServerUsernameAssertion assert
                //     = new ServerUsernameAssertion(ServerSettings.ServerId, serverPassword);

                // create policy
                //Policy policy = new Policy();
                //policy.Assertions.Add(assert);

                //proxy.SetPolicy(policy);
                proxy.Credentials.Password = serverPassword;
            }

            // provider settings
            ServiceProviderSettingsSoapHeader settingsHeader = new ServiceProviderSettingsSoapHeader();
            List<string> settings = new List<string>();
            
            // AD Settings
            settings.Add("AD:Enabled=" + ServerSettings.ADEnabled.ToString());
            settings.Add("AD:AuthenticationType=" + ServerSettings.ADAuthenticationType.ToString());
            settings.Add("AD:ParentDomain=" + ServerSettings.ADParentDomain);
            settings.Add("AD:ParentDomainController=" + ServerSettings.ADParentDomainController);
            settings.Add("AD:RootDomain=" + ServerSettings.ADRootDomain);
            settings.Add("AD:Username=" + ServerSettings.ADUsername);
            settings.Add("AD:Password=" + ServerSettings.ADPassword);

            // Server Settings
            settings.Add("Server:ServerId=" + ServerSettings.ServerId);
            settings.Add("Server:ServerName=" + ServerSettings.ServerName);

            // Provider Settings
            settings.Add("Provider:ProviderGroupID=" + ProviderSettings.ProviderGroupID.ToString());
            settings.Add("Provider:ProviderCode=" + ProviderSettings.ProviderCode);
            settings.Add("Provider:ProviderName=" + ProviderSettings.ProviderName);
            settings.Add("Provider:ProviderType=" + ProviderSettings.ProviderType);

            // Custom Provider Settings
            foreach (string settingName in ProviderSettings.Settings.Keys)
            {
                settings.Add(settingName + "=" + ProviderSettings.Settings[settingName]);
            }

            // set header
            settingsHeader.Settings = settings.ToArray();

            if (proxy.IsAuthenticated && proxy.HasSoapHeaders)
            {
                proxy.SoapHeader = settingsHeader;
            }
            //FieldInfo field = proxy.GetType().GetField("ServiceProviderSettingsSoapHeaderValue");
            //if (field != null)
            //    field.SetValue(proxy, settingsHeader);
        }
    }
}
