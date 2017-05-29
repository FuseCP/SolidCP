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

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for ServerInfo.
	/// </summary>
	[Serializable]
	public class ServerInfo
	{
		private int serverId;
		private string serverName;
		private string serverUrl;
		private String password;
		private string comments;
        private bool virtualServer;
        private string instantDomainAlias;
        private bool adEnabled;
        private string adRootDomain;
        private string adAuthenticationType;
        private string adUsername;
        private string adPassword;
        private int primaryGroupId;

        public ServerInfo()
        {
        }

        public int PrimaryGroupId
        {
            get { return primaryGroupId; }
            set { primaryGroupId = value; }
        }

        public string ADRootDomain
        {
            get { return adRootDomain; }
            set { adRootDomain = value; }
        }

        public string ADAuthenticationType
        {
            get { return this.adAuthenticationType; }
            set { this.adAuthenticationType = value; }
        }

        public string ADUsername
        {
            get { return adUsername; }
            set { adUsername = value; }
        }

        public string ADPassword
        {
            get { return adPassword; }
            set { adPassword = value; }
        }

		public int ServerId
		{
			get { return serverId; }
			set { serverId = value; }
		}

		public string ServerName
		{
			get { return serverName; }
			set { serverName = value; }
		}

		public string ServerUrl
		{
			get { return serverUrl; }
			set { serverUrl = value; }
		}

		public string Comments
		{
			get { return comments; }
			set { comments = value; }
		}
		
		public String Password
		{
			get{ return password; }
			set{ password = value; }
		}

        public bool VirtualServer
        {
            get { return virtualServer; }
            set { virtualServer = value; }
        }

        public string InstantDomainAlias
        {
            get { return instantDomainAlias; }
            set { instantDomainAlias = value; }
        }

        public bool ADEnabled
        {
            get { return this.adEnabled; }
            set { this.adEnabled = value; }
        }
	}
}
