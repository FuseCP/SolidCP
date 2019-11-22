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
using System.DirectoryServices;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers
{
    public class RemoteServerSettings
    {
        // Active Directory settings
        private bool adEnabled;
        private AuthenticationTypes adAuthenticationType;
        private string adRootDomain;
        private string adUsername;
        private string adPassword;
        private string adParentDomain;
        private string adParentDomainController;

        // Server settings
        private int serverId;
        private string serverName;

        public RemoteServerSettings()
        {
            // just do nothing
        }

        public RemoteServerSettings(string[] settings)
        {
            // parse settings array
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                string key = setting.Substring(0, idx);
                string val = setting.Substring(idx + 1);

                if (key == "AD:Enabled")
                    ADEnabled = Boolean.Parse(val);
                else if (key == "AD:AuthenticationType")
                    ADAuthenticationType = (AuthenticationTypes)Enum.Parse(typeof(AuthenticationTypes), val, true);
                else if (key == "AD:RootDomain")
                    ADRootDomain = val;
                else if (key == "AD:Username")
                    ADUsername = val;
                else if (key == "AD:Password")
                    ADPassword = val;
                else if (key == "Server:ServerId")
                    ServerId = Int32.Parse(val);
                else if (key == "Server:ServerName")
                    ServerName = val;
                else if (key == "AD:ParentDomain")
                    ADParentDomain = val;
                else if (key == "AD:ParentDomainController")
                    ADParentDomainController = val;
            }
        }

        #region Public Properties
        public bool ADEnabled
        {
            get { return this.adEnabled; }
            set { this.adEnabled = value; }
        }

        public AuthenticationTypes ADAuthenticationType
        {
            get { return this.adAuthenticationType; }
            set { this.adAuthenticationType = value; }
        }

        public string ADRootDomain
        {
            get { return this.adRootDomain; }
            set { this.adRootDomain = value; }
        }

        public string ADUsername
        {
            get { return this.adUsername; }
            set { this.adUsername = value; }
        }

        public string ADPassword
        {
            get { return this.adPassword; }
            set { this.adPassword = value; }
        }

        public int ServerId
        {
            get { return this.serverId; }
            set { this.serverId = value; }
        }

        public string ServerName
        {
            get { return this.serverName; }
            set { this.serverName = value; }
        }

        public string ADParentDomain
        {
            get { return this.adParentDomain; }
            set { this.adParentDomain = value; }
        }

        public string ADParentDomainController
        {
            get { return this.adParentDomainController; }
            set { this.adParentDomainController = value; }
        }
        #endregion
    }
}
