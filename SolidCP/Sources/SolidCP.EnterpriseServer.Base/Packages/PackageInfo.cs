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
	/// Summary description for PackageInfo.
	/// </summary>
	[Serializable]
	public class PackageInfo
	{
		int packageId;
		int userId;
		int parentPackageId;
		int statusId;
		int planId;
        int serverId;
		DateTime purchaseDate;
		string packageName;
		string packageComments;
        int domains;
        int diskSpace;
        int bandWidth;
        int domainsQuota;
        int diskSpaceQuota;
        int bandWidthQuota;
        bool overrideQuotas;
        bool defaultTopPackage;
        HostingPlanGroupInfo[] groups;
        HostingPlanQuotaInfo[] quotas;

		public PackageInfo()
		{
		}

		public int PackageId
		{
			get { return packageId; }
			set { packageId = value; }
		}

		public int UserId
		{
			get { return userId; }
			set { userId = value; }
		}

		public int ParentPackageId
		{
			get { return parentPackageId; }
			set { parentPackageId = value; }
		}

		public int StatusId
		{
			get { return statusId; }
			set { statusId = value; }
		}

		public int PlanId
		{
			get { return planId; }
			set { planId = value; }
		}

		public DateTime PurchaseDate
		{
			get { return purchaseDate; }
			set { purchaseDate = value; }
		}

		public string PackageName
		{
			get { return packageName; }
			set { packageName = value; }
		}

		public string PackageComments
		{
			get { return packageComments; }
			set { packageComments = value; }
		}

        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }

        public int DiskSpace
        {
            get { return diskSpace; }
            set { diskSpace = value; }
        }

        public int BandWidth
        {
            get { return bandWidth; }
            set { bandWidth = value; }
        }

        public int DiskSpaceQuota
        {
            get { return this.diskSpaceQuota; }
            set { this.diskSpaceQuota = value; }
        }

        public int BandWidthQuota
        {
            get { return this.bandWidthQuota; }
            set { this.bandWidthQuota = value; }
        }

        public int Domains
        {
            get { return this.domains; }
            set { this.domains = value; }
        }

        public int DomainsQuota
        {
            get { return this.domainsQuota; }
            set { this.domainsQuota = value; }
        }

        public bool OverrideQuotas
        {
            get { return this.overrideQuotas; }
            set { this.overrideQuotas = value; }
        }

        public bool DefaultTopPackage 
        {
            get { return this.defaultTopPackage;  }
            set { this.defaultTopPackage = value; }
        }

        public HostingPlanGroupInfo[] Groups
        {
            get { return this.groups; }
            set { this.groups = value; }
        }

        public HostingPlanQuotaInfo[] Quotas
        {
            get { return this.quotas; }
            set { this.quotas = value; }
        }
    }
}
