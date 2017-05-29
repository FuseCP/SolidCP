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
using SolidCP.Providers;

namespace SolidCP.EnterpriseServer
{
    [Serializable]
    public class DomainInfo
    {
        private int domainId;
        private int packageId;
        private int zoneItemId;
        private int domainItemId;
        private string domainName;
        private bool hostingAllowed;
        private int webSiteId;
        private int mailDomainId;
        private string webSiteName;
        private string mailDomainName;
        private string zoneName;
        private bool isSubDomain;
        private bool isInstantAlias;
        private bool isDomainPointer;
        private int instantAliasId;
        private string instantAliasName;
        
        [LogProperty]
        public int DomainId
        {
            get { return domainId; }
            set { domainId = value; }
        }

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }

        public int ZoneItemId
        {
            get { return zoneItemId; }
            set { zoneItemId = value; }
        }

        public int DomainItemId
        {
            get { return domainItemId; }
            set { domainItemId = value; }
        }

        [LogProperty]
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        public bool HostingAllowed
        {
            get { return hostingAllowed; }
            set { hostingAllowed = value; }
        }

        public int WebSiteId
        {
            get { return webSiteId; }
            set { webSiteId = value; }
        }

        public int MailDomainId
        {
            get { return mailDomainId; }
            set { mailDomainId = value; }
        }

        public string WebSiteName
        {
            get { return this.webSiteName; }
            set { this.webSiteName = value; }
        }

        public string MailDomainName
        {
            get { return this.mailDomainName; }
            set { this.mailDomainName = value; }
        }

        public string ZoneName
        {
            get { return this.zoneName; }
            set { this.zoneName = value; }
        }

        public bool IsSubDomain
        {
            get { return this.isSubDomain; }
            set { this.isSubDomain = value; }
        }

        public bool IsInstantAlias
        {
            get { return this.isInstantAlias; }
            set { this.isInstantAlias = value; }
        }

        public bool IsDomainPointer
        {
            get { return this.isDomainPointer; }
            set { this.isDomainPointer = value; }
        }

        public int InstantAliasId
        {
            get { return this.instantAliasId; }
            set { this.instantAliasId = value; }
        }

        public string InstantAliasName
        {
            get { return this.instantAliasName; }
            set { this.instantAliasName = value; }
        }

        public DateTime? CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string RegistrarName { get; set; }
    }
}
