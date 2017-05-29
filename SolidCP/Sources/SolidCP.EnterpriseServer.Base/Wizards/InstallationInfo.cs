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
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for InstallationInfo.
	/// </summary>
	[Serializable]
	public class InstallationInfo
	{
        private NameValueCollection propertiesHash = null;
        public string[][] PropertiesArray;

		private int packageId;
		private string applicationId;
		private int webSiteId;
		private string virtualDir;
        private int databaseId;
		private string databaseName;
        private string databaseGroup;
        private int userId;
		private string username;
		private string password;

		public InstallationInfo()
		{
		}

        [XmlIgnore]
        NameValueCollection Properties
        {
            get
            {
                if (propertiesHash == null)
                {
                    // create new dictionary
                    propertiesHash = new NameValueCollection();

                    // fill dictionary
                    if (PropertiesArray != null)
                    {
                        foreach (string[] pair in PropertiesArray)
                            propertiesHash.Add(pair[0], pair[1]);
                    }
                }
                return propertiesHash;
            }
        }

        [XmlIgnore]
        public string this[string propertyName]
        {
            get
            {
                return Properties[propertyName];
            }
            set
            {
                // set setting
                Properties[propertyName] = value;

                // rebuild array
                PropertiesArray = new string[Properties.Count][];
                for (int i = 0; i < Properties.Count; i++)
                {
                    PropertiesArray[i] = new string[] { Properties.Keys[i], Properties[Properties.Keys[i]] };
                }
            }
        }

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

        public string ApplicationId
        {
            get { return this.applicationId; }
            set { this.applicationId = value; }
        }

        public int WebSiteId
        {
            get { return this.webSiteId; }
            set { this.webSiteId = value; }
        }

        public string VirtualDir
        {
            get { return this.virtualDir; }
            set { this.virtualDir = value; }
        }

        public int DatabaseId
        {
            get { return this.databaseId; }
            set { this.databaseId = value; }
        }

        public string DatabaseName
        {
            get { return this.databaseName; }
            set { this.databaseName = value; }
        }

        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
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

        public string DatabaseGroup
        {
            get { return this.databaseGroup; }
            set { this.databaseGroup = value; }
        }
	}
}
