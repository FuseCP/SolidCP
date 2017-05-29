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
	/// <summary>
	/// Represents SharePoint site collection information.
	/// </summary>
	[Serializable]
	public class SharePointSiteCollection : ServiceProviderItem
	{
		private int organizationId;
		private string url;
		private string physicalAddress;
		private string ownerLogin;
		private string ownerName;
		private string ownerEmail;
		private int localeId;
		private string title;
		private string description;
		private long bandwidth;
		private long diskspace;
	    private long maxSiteStorage;
	    private long warningStorage;
        private string rootWebApplicationInteralIpAddress;
        private string rootWebApplicationFQDN;



	    [Persistent]
        public long MaxSiteStorage
	    {
	        get { return maxSiteStorage; }
	        set { maxSiteStorage = value; }
	    }

        [Persistent]
        public long WarningStorage
	    {
	        get { return warningStorage; }
	        set { warningStorage = value; }
	    }

	    /// <summary>
		/// Gets or sets service item name.
		/// </summary>
		public override string Name
		{
			get
			{
				return this.Url;
			}
			set
			{
				this.Url = value;
			}
		}

		/// <summary>
		/// Gets or sets id of organization which owns this site collection.
		/// </summary>
		[Persistent]
		public int OrganizationId
		{
			get
			{
				return this.organizationId;
			}
			set
			{
				this.organizationId = value;
			}
		}

		/// <summary>
		/// Gets or sets url of the host named site collection to be created. It must not contain port number.
		/// </summary>
		[Persistent]
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}

		/// <summary>
		/// Gets or sets physical address of the host named site collection. It contains scheme and port number.
		/// </summary>
		[Persistent]
		public string PhysicalAddress
		{
			get
			{
				return this.physicalAddress;
			}
			set
			{
				this.physicalAddress = value;
			}
		}

		/// <summary>
		/// Gets or sets login name of the site collection's owner/primary site administrator.
		/// </summary>
		[Persistent]
		public string OwnerLogin
		{
			get
			{
				return this.ownerLogin;
			}
			set
			{
				this.ownerLogin = value;
			}
		}

		/// <summary>
		/// Gets or sets display name of the site collection's owner/primary site administrator.
		/// </summary>
		[Persistent]
		public string OwnerName
		{
			get
			{
				return this.ownerName;
			}
			set
			{
				this.ownerName = value;
			}
		}

		/// <summary>
		/// Gets or sets display email of the site collection's owner/primary site administrator.
		/// </summary>
		[Persistent]
		public string OwnerEmail
		{
			get
			{
				return this.ownerEmail;
			}
			set
			{
				this.ownerEmail = value;
			}
		}

        /// <summary>
        /// Gets or sets the internal ip address
        /// </summary>
        [Persistent]
        public string RootWebApplicationInteralIpAddress
        {
            get
            {
                return this.rootWebApplicationInteralIpAddress;
            }
            set
            {
                this.rootWebApplicationInteralIpAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the internal ip address
        /// </summary>
        [Persistent]
        public string RootWebApplicationFQDN
        {
            get
            {
                return this.rootWebApplicationFQDN;
            }
            set
            {
                this.rootWebApplicationFQDN = value;
            }
        }


		/// <summary>
		/// Gets or sets locale id of the site collection to be created.
		/// </summary>
		[Persistent]
		public int LocaleId
		{
			get
			{
				return this.localeId;
			}
			set
			{
				this.localeId = value;
			}
		}

		/// <summary>
		/// Gets or sets title of the the site collection to be created.
		/// </summary>
		[Persistent]
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
			}
		}

		/// <summary>
		/// Gets or sets description of the the site collection to be created.
		/// </summary>
		[Persistent]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>
		/// Gets or sets bandwidth of the the site collection.
		/// </summary>
		[Persistent]
		public long Bandwidth
		{
			get
			{
				return this.bandwidth;
			}
			set
			{
				this.bandwidth = value;
			}
		}

		/// <summary>
		/// Gets or sets diskspace of the the site collection.
		/// </summary>
		[Persistent]
		public long Diskspace
		{
			get
			{
				return this.diskspace;
			}
			set
			{
				this.diskspace = value;
			}
		}
	
	}
}
