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

namespace SolidCP.Providers.SharePoint
{
	[Serializable]
    public class SharePointSite : ServiceProviderItem
    {
        private string ownerLogin;
        private string ownerEmail;
        private string databaseServer;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string siteTemplate;
        private string databaseGroupName;
		private string applicationPool;
		private string rootFolder;
        private int localeID;

        [Persistent]
        public string OwnerLogin
        {
            get { return this.ownerLogin; }
            set { this.ownerLogin = value; }
        }

        [Persistent]
        public string OwnerEmail
        {
            get { return this.ownerEmail; }
            set { this.ownerEmail = value; }
        }

        [Persistent]
        public string DatabaseName
        {
            get { return this.databaseName; }
            set { this.databaseName = value; }
        }

        [Persistent]
        public string DatabaseUser
        {
            get { return this.databaseUser; }
            set { this.databaseUser = value; }
        }

        public string DatabaseServer
        {
            get { return this.databaseServer; }
            set { this.databaseServer = value; }
        }

        public string DatabasePassword
        {
            get { return this.databasePassword; }
            set { this.databasePassword = value; }
        }

        public string SiteTemplate
        {
            get { return this.siteTemplate; }
            set { this.siteTemplate = value; }
        }

        [Persistent]
        public string DatabaseGroupName
        {
            get { return this.databaseGroupName; }
            set { this.databaseGroupName = value; }
        }

		public string ApplicationPool
		{
			get { return this.applicationPool; }
			set { this.applicationPool = value; }
		}

		public string RootFolder
		{
			get { return this.rootFolder; }
			set { this.rootFolder = value; }
		}

        [Persistent]
        public int LocaleID
        {
            get { return this.localeID; }
            set { this.localeID = value; }
        }
    }
}
