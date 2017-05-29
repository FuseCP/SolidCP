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
using System.Net;
using System.ComponentModel;

using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace SolidCP.Providers.FTP
{
	[Serializable]
	public class FtpSite : ServiceProviderItem
	{
		private string siteId;
        private ServerBinding[] bindings = new ServerBinding[0];

		private bool allowAnonymous;
		private bool allowExecuteAccess;
		private bool allowScriptAccess;
		private bool allowSourceAccess;
		private bool allowReadAccess;
		private bool allowWriteAccess;
		private string anonymousUsername;
		private string anonymousUserPassword;
		private string contentPath;
		private string logFileDirectory;
		private bool anonymousOnly;

		public const string MSFTP7_SITE_ID = "MsFtp7_SiteId";
		public const string MSFTP7_LOG_EXT_FILE_FIELDS = "MsFtp7_LogExtFileFields";

		public FtpSite()
		{
		}

		[Persistent]
		public string SiteId
		{
			set { siteId = value; }
			get { return siteId; }
		}

        public ServerBinding[] Bindings
		{
			set { bindings = value; }
			get { return bindings; }
		}
			
		public bool AllowScriptAccess
		{
			get { return allowScriptAccess; }
			set { allowScriptAccess = value; }
		}

		public bool AllowSourceAccess
		{
			get { return allowSourceAccess; }
			set { allowSourceAccess = value; }
		}

	
		public bool AllowReadAccess
		{
			get { return allowReadAccess; }
			set { allowReadAccess = value; }
		}

		public bool AllowWriteAccess
		{
			get { return allowWriteAccess; }
			set { allowWriteAccess = value; }
		}

		public string LogFileDirectory
		{
			get { return logFileDirectory; }
			set { logFileDirectory = value; }
		}
		
		public string AnonymousUsername
		{
			get { return anonymousUsername; }
			set { anonymousUsername = value; }
		}

		public string AnonymousUserPassword
		{
			get { return anonymousUserPassword; }
			set { anonymousUserPassword = value; }
		}
	
		public bool AllowAnonymous
		{
			get { return allowAnonymous; }
			set { allowAnonymous = value; }
		}
		
		public bool AllowExecuteAccess
		{
			get { return allowExecuteAccess; }
			set { allowExecuteAccess = value; }
		}

		public string ContentPath
		{
			get { return contentPath; }
			set { contentPath = value; }
		}

		public bool AnonymousOnly
		{
			set { anonymousOnly = value; }
			get { return anonymousOnly; }
		}
	}
}
