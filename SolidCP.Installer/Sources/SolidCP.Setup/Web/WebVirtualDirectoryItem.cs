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

namespace SolidCP.Setup.Web
{
	/// <summary>
	/// Virtual directory item.
	/// </summary>
	[Serializable]
	public class WebVirtualDirectoryItem
	{
		private bool allowExecuteAccess;
		private bool allowScriptAccess;
		private bool allowSourceAccess;
		private bool allowReadAccess;
		private bool allowWriteAccess;
		private string anonymousUsername;
		private string anonymousUserPassword;
		private string contentPath;
		private bool allowDirectoryBrowsingAccess;	
		private bool authAnonymous;
		private bool authWindows;
		private bool authBasic;
		private string defaultDocs;
		private string httpRedirect;
		private string name;
        private AspNetVersion installedDotNetFramework;
		private string applicationPool;

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public WebVirtualDirectoryItem()
		{
		}

		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
			
		/// <summary>
		/// Allow script access
		/// </summary>
		public bool AllowScriptAccess
		{
			get { return allowScriptAccess; }
			set { allowScriptAccess = value; }
		}

		/// <summary>
		/// Allow source access
		/// </summary>
		public bool AllowSourceAccess
		{
			get { return allowSourceAccess; }
			set { allowSourceAccess = value; }
		}

	
		/// <summary>
		/// Allow read access
		/// </summary>
		public bool AllowReadAccess
		{
			get { return allowReadAccess; }
			set { allowReadAccess = value; }
		}

		/// <summary>
		/// Allow write access
		/// </summary>
		public bool AllowWriteAccess
		{
			get { return allowWriteAccess; }
			set { allowWriteAccess = value; }
		}
		
		/// <summary>
		/// Anonymous user name
		/// </summary>
		public string AnonymousUsername
		{
			get { return anonymousUsername; }
			set { anonymousUsername = value; }
		}

		/// <summary>
		/// Anonymous user password
		/// </summary>
		public string AnonymousUserPassword
		{
			get { return anonymousUserPassword; }
			set { anonymousUserPassword = value; }
		}
		
		/// <summary>
		/// Allow execute access
		/// </summary>
		public bool AllowExecuteAccess
		{
			get { return allowExecuteAccess; }
			set { allowExecuteAccess = value; }
		}

		/// <summary>
		/// Content path
		/// </summary>
		public string ContentPath
		{
			get { return contentPath; }
			set { contentPath = value; }
		}
	
		/// <summary>
		/// Http redirect
		/// </summary>
		public string HttpRedirect
		{
			get { return httpRedirect; }
			set { httpRedirect = value; }
		}

		/// <summary>
		/// Default documents
		/// </summary>
		public string DefaultDocs
		{
			get { return defaultDocs; }
			set { defaultDocs = value; }
		}

		/// <summary>
		/// Allow directory browsing access
		/// </summary>
		public bool AllowDirectoryBrowsingAccess
		{
			get { return allowDirectoryBrowsingAccess; }
			set { allowDirectoryBrowsingAccess = value; }
		}

		/// <summary>
		/// Anonymous access.
		/// </summary>
		public bool AuthAnonymous
		{
			get { return this.authAnonymous; }
			set { this.authAnonymous = value; }
		}

		/// <summary>
		/// Basic authentication.
		/// </summary>
		public bool AuthBasic
		{
			get { return this.authBasic; }
			set { this.authBasic = value; }
		}

		/// <summary>
		/// Integrated Windows authentication.
		/// </summary>
		public bool AuthWindows
		{
			get { return this.authWindows; }
			set { this.authWindows = value; }
		}

        /// <summary>
        /// Installed ASP.NET version
        /// </summary>
        public AspNetVersion InstalledDotNetFramework
        {
            get { return this.installedDotNetFramework; }
            set { this.installedDotNetFramework = value; }
        }

		/// <summary>
		/// Application pool
		/// </summary>
		public string ApplicationPool
		{
			get { return applicationPool; }
			set { applicationPool = value; }
		}
	}
}

