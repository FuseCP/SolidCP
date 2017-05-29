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
using System.Text;
using System.Xml.Serialization;

namespace SolidCP.Providers.Web
{
    public class WebAppVirtualDirectory : ServiceProviderItem
	{
		#region Web Management Service Constants

		public const string WmSvcAvailable = "WmSvcAvailable";
		public const string WmSvcSiteEnabled = "WmSvcSiteEnabled";
		public const string WmSvcAccountName = "WmSvcAccountName";
		public const string WmSvcAccountPassword = "WmSvcAccountPassword";
		public const string WmSvcServiceUrl = "WmSvcServiceUrl";
		public const string WmSvcServicePort = "WmSvcServicePort";
		public const string WmSvcDefaultPort = "8172";

		#endregion

        private string anonymousUsername;
        private string anonymousUserPassword;
        private string contentPath;
        private bool enableWritePermissions;
        private bool enableParentPaths;
        private bool enableDirectoryBrowsing;
        private bool enableAnonymousAccess;
        private bool enableWindowsAuthentication;
        private bool enableBasicAuthentication;
        private bool enableDynamicCompression;
        private bool enableStaticCompression;
        private string defaultDocs;
        private string httpRedirect;
        private HttpError[] httpErrors;
        private HttpErrorsMode errorMode;
        private HttpErrorsExistingResponse existingResponse;
        private MimeMap[] mimeMaps; 
        private HttpHeader[] httpHeaders;
        private bool aspInstalled;
        private string aspNetInstalled;
        private string phpInstalled;
        private bool perlInstalled;
        private bool pythonInstalled;
        private bool coldfusionInstalled;
        private bool cgiBinInstalled;
        private string applicationPool;
        private bool dedicatedApplicationPool;
        private string parentSiteName;
        private bool redirectExactUrl;
        private bool redirectDirectoryBelow;
        private bool redirectPermanent;
        private bool sharePointInstalled;
        private bool iis7;
        private string consoleUrl;
        private string php5VersionsInstalled;

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

        public string ContentPath
        {
            get { return contentPath; }
            set { contentPath = value; }
        }

        public string HttpRedirect
        {
            get { return httpRedirect; }
            set { httpRedirect = value; }
        }

        public string DefaultDocs
        {
            get { return defaultDocs; }
            set { defaultDocs = value; }
        }

        public MimeMap[] MimeMaps
        {
            get { return mimeMaps; }
            set { mimeMaps = value; }
        }

        public HttpError[] HttpErrors
        {
            get { return httpErrors; }
            set { httpErrors = value; }
        }

        public HttpErrorsMode ErrorMode
        {
            get { return errorMode; }
            set { errorMode = value; }
        }

        public HttpErrorsExistingResponse ExistingResponse
        {
            get { return existingResponse; }
            set { existingResponse = value; }
        }

        public string ApplicationPool
        {
            get { return this.applicationPool; }
            set { this.applicationPool = value; }
        }

        public bool EnableParentPaths
        {
            get { return this.enableParentPaths; }
            set { this.enableParentPaths = value; }
        }

        public HttpHeader[] HttpHeaders
        {
            get { return this.httpHeaders; }
            set { this.httpHeaders = value; }
        }

        public bool EnableWritePermissions
        {
            get { return this.enableWritePermissions; }
            set { this.enableWritePermissions = value; }
        }

        public bool EnableDirectoryBrowsing
        {
            get { return this.enableDirectoryBrowsing; }
            set { this.enableDirectoryBrowsing = value; }
        }

        public bool EnableAnonymousAccess
        {
            get { return this.enableAnonymousAccess; }
            set { this.enableAnonymousAccess = value; }
        }

        public bool EnableWindowsAuthentication
        {
            get { return this.enableWindowsAuthentication; }
            set { this.enableWindowsAuthentication = value; }
        }

        public bool EnableBasicAuthentication
        {
            get { return this.enableBasicAuthentication; }
            set { this.enableBasicAuthentication = value; }
        }

        public bool EnableDynamicCompression
        {
            get { return this.enableDynamicCompression; }
            set { this.enableDynamicCompression = value; }
        }
        public bool EnableStaticCompression
        {
            get { return this.enableStaticCompression; }
            set { this.enableStaticCompression = value; }
        }

        public bool AspInstalled
        {
            get { return this.aspInstalled; }
            set { this.aspInstalled = value; }
        }

        public string AspNetInstalled
        {
            get { return this.aspNetInstalled; }
            set { this.aspNetInstalled = value; }
        }

        public string PhpInstalled
        {
            get { return this.phpInstalled; }
            set { this.phpInstalled = value; }
        }

        public bool PerlInstalled
        {
            get { return this.perlInstalled; }
            set { this.perlInstalled = value; }
        }

        public bool PythonInstalled
        {
            get { return this.pythonInstalled; }
            set { this.pythonInstalled = value; }
        }

        public bool ColdFusionInstalled
        {
            get { return this.coldfusionInstalled; }
            set { this.coldfusionInstalled = value; }
        }

        public bool DedicatedApplicationPool
        {
            get { return this.dedicatedApplicationPool; }
            set { this.dedicatedApplicationPool = value; }
        }

        public string ParentSiteName
        {
            get { return this.parentSiteName; }
            set { this.parentSiteName = value; }
        }

        public bool RedirectExactUrl
        {
            get { return this.redirectExactUrl; }
            set { this.redirectExactUrl = value; }
        }

        public bool RedirectDirectoryBelow
        {
            get { return this.redirectDirectoryBelow; }
            set { this.redirectDirectoryBelow = value; }
        }

        public bool RedirectPermanent
        {
            get { return this.redirectPermanent; }
            set { this.redirectPermanent = value; }
        }

        public bool CgiBinInstalled
        {
            get { return this.cgiBinInstalled; }
            set { this.cgiBinInstalled = value; }
        }

        public bool SharePointInstalled
        {
            get { return this.sharePointInstalled; }
            set { this.sharePointInstalled = value; }
        }

        public bool IIs7
        {
            get { return this.iis7; }
            set { this.iis7 = value; }
        }
        
        public string ConsoleUrl
        {
            get { return consoleUrl; }
            set { consoleUrl = value; }
        }

        public string Php5VersionsInstalled
        {
            get { return php5VersionsInstalled; }
            set { php5VersionsInstalled = value; }
        }

        #region Web Deploy Publishing Properties
		/// <summary>
		/// Gets or sets Web Deploy publishing account name
		/// </summary>
		[Persistent]
		public string WebDeployPublishingAccount { get; set; }

		/// <summary>
		/// Gets or sets Web Deploy publishing password
		/// </summary>
		[Persistent]
		public string WebDeployPublishingPassword { get; set; }

		/// <summary>
		/// Gets or sets whether Web Deploy publishing is enabled on the server
		/// </summary>
		public bool WebDeployPublishingAvailable { get; set; }

		/// <summary>
		/// Gets or sets whether Web Deploy publishing is enabled for this particular web site
		/// </summary>
		[Persistent]
		public bool WebDeploySitePublishingEnabled { get; set; }

		/// <summary>
		/// Gets or sets Web Deploy publishing profile data for this particular web site
		/// </summary>
		[Persistent]
		public string WebDeploySitePublishingProfile { get; set; }

		#endregion

		/// <summary>
		/// Gets fully qualified name which consists of parent website name if present and virtual directory name.
		/// </summary>
		[XmlIgnore]
    	public string VirtualPath
    	{
    		get
    		{
                // virtual path is rooted
                if (String.IsNullOrEmpty(ParentSiteName))
                    return "/"; //
                else if (!Name.StartsWith("/"))
                    return "/" + Name;
                //
                return Name;
    		}
    	}

        /// <summary>
        /// Gets fully qualified name which consists of parent website name if present and virtual directory name.
        /// </summary>
        [XmlIgnore]
        public string FullQualifiedPath
        {
            get
            {
                if (String.IsNullOrEmpty(ParentSiteName))
                    return Name;
                else if (Name.StartsWith("/"))
                    return ParentSiteName + Name;
                else
                    return ParentSiteName + "/" + Name;
            }
        }
	}
}
