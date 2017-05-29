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

namespace SolidCP.Providers.HostedSolution
{
    [LogParentProperty("Id", NameInLog = "Org ID")]
    [LogParentProperty("Name", NameInLog = "Org Name")]
    public class Organization : ServiceProviderItem
    {
        #region Fields

        private string distinguishedName;
        private string organizationId;
        private string defaultDomain;
        private string offlineAddressBook;
        private string addressList;
        private string roomsAddressList;
        private string globalAddressList;
        private string addressBookPolicy;

        private string database;
        private string securityGroup;
        private string lyncTenantId;
        private string sfbTenantId;
        private int diskSpace;
        private int keepDeletedItemsDays;


        private Guid crmOrganizationId;
        private int crmOrgState;
        private int crmAdministratorId;
        private string crmLanguadgeCode;
        private string crmCollation;
        private string crmCurrency;
        private string crmUrl;

        private int maxSharePointStorage;
        private int warningSharePointStorage;

        private int maxSharePointEnterpriseStorage;
        private int warningSharePointEnterpriseStorage;

        #endregion
 
        [Persistent]
        public bool IsDefault { get; set; }

        [Persistent]
        public int MaxSharePointStorage
        {
            get { return maxSharePointStorage; }
            set { maxSharePointStorage = value; }
        }

        [Persistent]
        public int WarningSharePointStorage
        {
            get { return warningSharePointStorage; }
            set { warningSharePointStorage = value; }
        }

        [Persistent]
        public int MaxSharePointEnterpriseStorage
        {
            get { return maxSharePointEnterpriseStorage; }
            set { maxSharePointEnterpriseStorage = value; }
        }

        [Persistent]
        public int WarningSharePointEnterpriseStorage
        {
            get { return warningSharePointEnterpriseStorage; }
            set { warningSharePointEnterpriseStorage = value; }
        }

        [Persistent]
        public string CrmUrl
        {
            get { return crmUrl; }
            set { crmUrl = value; }
        }

        [Persistent]
        public Guid CrmOrganizationId
        {
            get { return crmOrganizationId; }
            set { crmOrganizationId = value; }
        }

        [Persistent]
        public int CrmOrgState
        {
            get { return crmOrgState; }
            set { crmOrgState = value; }
        }

        [Persistent]
        public int CrmAdministratorId
        {
            get { return crmAdministratorId; }
            set { crmAdministratorId = value; }
        }

        [Persistent]
        public string CrmLanguadgeCode
        {
            get { return crmLanguadgeCode; }
            set { crmLanguadgeCode = value; }
        }

        [Persistent]
        public string CrmCollation
        {
            get { return crmCollation; }
            set { crmCollation = value; }
        }

        [Persistent]
        public string CrmCurrency
        {
            get { return crmCurrency; }
            set { crmCurrency = value; }
        }

        [Persistent]
        public string DistinguishedName
        {
            get { return distinguishedName; }
            set { distinguishedName = value; }
        }

        [Persistent]
        public string OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }

        [Persistent]
        public string DefaultDomain
        {
            set
            {
                defaultDomain = value;
            }
            get
            {
                return defaultDomain;
            }
        }


        [Persistent]
        public string OfflineAddressBook
        {
            get { return offlineAddressBook; }
            set { offlineAddressBook = value; }
        }

        [Persistent]
        public string AddressList
        {
            get { return addressList; }
            set { addressList = value; }
        }

        [Persistent]
        public string RoomsAddressList
        {
            get { return roomsAddressList; }
            set { roomsAddressList = value; }
        }

        [Persistent]
        public string GlobalAddressList
        {
            get { return globalAddressList; }
            set { globalAddressList = value; }
        }

        [Persistent]
        public string AddressBookPolicy
        {
            get { return addressBookPolicy; }
            set { addressBookPolicy = value; }
        }

        [Persistent]
        public string Database
        {
            get { return database; }
            set { database = value; }
        }

        [Persistent]
        public string SecurityGroup
        {
            get { return securityGroup; }
            set { securityGroup = value; }
        }

        [Persistent]
        public int DiskSpace
        {
            get { return diskSpace; }
            set { diskSpace = value; }
        }

        [Persistent]
        public int KeepDeletedItemsDays
        {
            get { return keepDeletedItemsDays; }
            set { keepDeletedItemsDays = value; }
        }

        [Persistent]
        public bool IsOCSOrganization { get; set; }


        [Persistent]
        public string LyncTenantId
        {
            get { return lyncTenantId; }
            set { lyncTenantId = value; }
        }
        [Persistent]
        public string SfBTenantId
        {
            get { return sfbTenantId; }
            set { sfbTenantId = value; }
        }


    }
}
