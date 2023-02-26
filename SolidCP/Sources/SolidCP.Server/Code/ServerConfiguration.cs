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
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for ServerConfiguration
    /// </summary>
    public class ServerConfiguration : IConfigurationSectionHandler
    {
        #region Public Properties
        private static SecuritySettings security = null;

        public static SecuritySettings Security
        {
            get
            {
                return security;
            }
        }
        #endregion

        private ServerConfiguration()
        {
        }

        static ServerConfiguration()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            System.Configuration.ConfigurationManager.GetSection("SolidCP.server");
        }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // parse "security" section
            XmlNode nodeSecurity = section.SelectSingleNode("security");
            if (nodeSecurity == null)
                throw new Exception("'SolidCP/security' section is missing");

            security = new SecuritySettings();
            security.ParseSection(nodeSecurity);

            return null;
        }

        #region Inner Classes
        public class SecuritySettings
        {
            private bool securityEnabled;
            private string password;

            public bool SecurityEnabled
            {
                get { return this.securityEnabled; }
                set { this.securityEnabled = value; }
            }

            public string Password
            {
                get { return this.password; }
                set { this.password = value; }
            }

            public void ParseSection(XmlNode section)
            {
                // enabled
                XmlNode nodeEnabled = section.SelectSingleNode("enabled");
                if (nodeEnabled == null)
                    throw new Exception("'SolidCP/security/enabled' node is missing");

                if (nodeEnabled.Attributes["value"] == null)
                    throw new Exception("'SolidCP/security/enabled/@value' attribute is missing");

                securityEnabled = true;
                Boolean.TryParse(nodeEnabled.Attributes["value"].Value, out securityEnabled);

                // password
                XmlNode nodePassword = section.SelectSingleNode("password");
                if (nodePassword == null)
                    throw new Exception("'SolidCP/security/password' node is missing");

                if (nodePassword.Attributes["value"] == null)
                    throw new Exception("'SolidCP/security/password/@value' attribute is missing");

                password = nodePassword.Attributes["value"].Value;
            }
        }
        #endregion
    }
}
