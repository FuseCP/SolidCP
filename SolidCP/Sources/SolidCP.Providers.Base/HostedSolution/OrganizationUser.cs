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


    public class OrganizationUser
    {
        private int accountId;
        private int itemId;
        private int packageId;
        private string subscriberNumber;

        private string primaryEmailAddress;
        private string accountPassword;
        private string samAccountName;
        private string displayName;
        private string accountName;
        private string firstName;
        private string initials;
        private string lastName;
        private string jobTitle;
        private string company;
        private string department;
        private string office;
        private string businessPhone;
        private string fax;
        private string homePhone;
        private string mobilePhone;
        private string pager;
        private string webPage;
        private string address;
        private string city;
        private string state;
        private string zip;
        private string country;
        private string notes;
        private string domainUserName;
        private string userPrincipalName;

        private bool disabled;
        private bool locked;
        private bool isOCSUser;
        private bool isBlackBerryUser;
        private bool isLyncUser;
        private bool isSfBUser;

        ExchangeAccountType accountType;

        private OrganizationUser manager;
        private Guid crmUserId;

        private int levelId;
        private bool isVip;

        public Guid CrmUserId
        {
            get { return crmUserId; }
            set { crmUserId = value; }
        }


        public string DomainUserName
        {
            get { return domainUserName; }
            set { domainUserName = value; }
        }

        public ExchangeAccountType AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }

        public OrganizationUser Manager
        {
            get { return manager; }
            set { manager = value; }
        }

        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string Initials
        {
            get { return initials; }
            set { initials = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string JobTitle
        {
            get { return jobTitle; }
            set { jobTitle = value; }
        }

        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        public string Department
        {
            get { return department; }
            set { department = value; }
        }

        public string Office
        {
            get { return office; }
            set { office = value; }
        }

        public string BusinessPhone
        {
            get { return businessPhone; }
            set { businessPhone = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string HomePhone
        {
            get { return homePhone; }
            set { homePhone = value; }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        public string Pager
        {
            get { return pager; }
            set { pager = value; }
        }

        public string WebPage
        {
            get { return webPage; }
            set { webPage = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        [LogProperty]
        public int AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }

        [LogProperty]
        public string AccountName
        {
            get { return accountName; }
            set { accountName = value; }
        }

        [LogProperty]
        public string SamAccountName
        {
            get { return samAccountName; }
            set { samAccountName = value; }
        }

        [LogProperty]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        [LogProperty("Email Address")]
        public string PrimaryEmailAddress
        {
            get { return primaryEmailAddress; }
            set { primaryEmailAddress = value; }
        }


        //public string AccountPassword
        //{
        //    get { return accountPassword; }
        //    set { accountPassword = value; }
        //}

        public string ExternalEmail { get; set; }

        public string DistinguishedName { get; set; }

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        public bool IsOCSUser
        {
            get { return isOCSUser; }
            set { isOCSUser = value; }
        }

        public bool IsLyncUser
        {
            get { return isLyncUser; }
            set { isLyncUser = value; }
        }
        public bool IsSfBUser
        {
            get { return isSfBUser; }
            set { isSfBUser = value; }
        }

        public bool IsBlackBerryUser
        {
            get { return isBlackBerryUser; }
            set { isBlackBerryUser = value; }
        }

        public string SubscriberNumber
        {
            get { return subscriberNumber; }
            set { subscriberNumber = value; }
        }

        public string UserPrincipalName
        {
            get { return userPrincipalName; }
            set { userPrincipalName = value; }
        }


        public int LevelId
        {
            get { return levelId; }
            set { levelId = value; }
        }

        public bool IsVIP
        {
            get { return isVip; }
            set { isVip = value; }
        }

        private bool userMustChangePassword;

        public bool UserMustChangePassword
        {
            get { return userMustChangePassword; }
            set { userMustChangePassword = value; }
        }

        public DateTime PasswordExpirationDateTime { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(accountName) ? accountName : base.ToString();
        }
    }
}
