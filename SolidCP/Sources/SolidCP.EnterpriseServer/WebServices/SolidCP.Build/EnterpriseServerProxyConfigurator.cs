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
using System.Text;

namespace SolidCP.EnterpriseServer
{
    public class EnterpriseServerProxyConfigurator
    {
        public const bool UseNetHttpAsDefaultProtocol = true;
        public const bool UseMessageSecurityOverHttp = true;

        private string enterpriseServerUrl;
        private string username;
        private string password;

        static bool? isCore = null;
        public bool IsCore
        {
            get
            {
                if (isCore == null)
                {
                    if (!string.IsNullOrEmpty(enterpriseServerUrl))
                    {
                        var test = new Client.esTest();
                        test.Url = enterpriseServerUrl;
                        isCore = test.OSPlatform().IsCore;
                    }
                    else throw new ArgumentNullException("EnterpriseServerUrl not set.");
                }
                return isCore.Value;
            }
        }
        public string EnterpriseServerUrl
        {
            get { return this.enterpriseServerUrl; }
            set { this.enterpriseServerUrl = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public void Configure(SolidCP.Web.Client.ClientBase proxy)
        {
            // set proxy URL
            string serverUrl = enterpriseServerUrl.Trim();
            if (serverUrl.Length == 0)
                throw new Exception("Enterprise Server URL could not be empty");

            proxy.Url = serverUrl;

            // set timeout
            proxy.Timeout = TimeSpan.FromMinutes(15); //15 minutes // System.Threading.Timeout.Infinite;

            if (proxy.IsDefaultApi)
            {
                if (UseMessageSecurityOverHttp && proxy.IsHttp && proxy.IsEncrypted && !IsCore)
                {
                    proxy.Protocol = Web.Client.Protocols.WSHttp;
                }
                else if (UseNetHttpAsDefaultProtocol)
                {
                    // use NetHttp protocol as default
                    if (proxy.IsHttp) proxy.Protocol = Web.Client.Protocols.NetHttp;
                    else if (proxy.IsHttps) proxy.Protocol = Web.Client.Protocols.NetHttps;
                }
            }

            if (!String.IsNullOrEmpty(username) && proxy.IsAuthenticated)
            {
                proxy.Credentials.UserName = username;
                proxy.Credentials.Password = password;
            }
        }
    }
}     