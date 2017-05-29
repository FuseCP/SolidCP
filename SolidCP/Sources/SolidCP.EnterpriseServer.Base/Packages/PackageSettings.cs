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
using System.Xml.Serialization;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for PackageSettings.
    /// </summary>
    public class PackageSettings
    {
        public const string INSTANT_ALIAS = "InstantAlias";
        public const string SPACES_FOLDER = "ChildSpacesFolder";
        public const string NAME_SERVERS = "NameServers";
        public const string SHARED_SSL_SITES = "SharedSslSites";
		public const string EXCHANGE_SERVER = "ExchangeServer";
        public const string HOSTED_SOLLUTION = "HostedSollution";
        public const string VIRTUAL_PRIVATE_SERVERS = "VirtualPrivateServers";
        public const string VIRTUAL_PRIVATE_SERVERS_PROXMOX = "VirtualPrivateServersProxmox";
        public const string VIRTUAL_PRIVATE_SERVERS_2012 = "VirtualPrivateServers2012";

        public const string VIRTUAL_PRIVATE_SERVERS_FOR_PRIVATE_CLOUD = "VirtualPrivateServersForPrivateCloud";
        public int PackageId;
        public string SettingsName;

        private NameValueCollection settingsHash = null;
        public string[][] SettingsArray;

        [XmlIgnore]
        NameValueCollection Settings
        {
            get
            {
                if (settingsHash == null)
                {
                    // create new dictionary
                    settingsHash = new NameValueCollection();

                    // fill dictionary
                    if (SettingsArray != null)
                    {
                        foreach (string[] pair in SettingsArray)
                            settingsHash.Add(pair[0], pair[1]);
                    }
                }
                return settingsHash;
            }
        }

        [XmlIgnore]
        public string this[string settingName]
        {
            get
            {
                return Settings[settingName];
            }
            set
            {
                // set setting
                Settings[settingName] = value;

                // rebuild array
                SettingsArray = new string[Settings.Count][];
                for (int i = 0; i < Settings.Count; i++)
                {
                    SettingsArray[i] = new string[] { Settings.Keys[i], Settings[Settings.Keys[i]] };
                }
            }
        }

        public int GetInt(string settingName)
        {
            return Int32.Parse(Settings[settingName]);
        }

        public long GetLong(string settingName)
        {
            return Int64.Parse(Settings[settingName]);
        }

        public bool GetBool(string settingName)
        {
            return Boolean.Parse(Settings[settingName]);
        }
    }
}
