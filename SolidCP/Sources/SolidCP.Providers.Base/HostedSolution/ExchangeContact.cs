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

namespace SolidCP.Providers.HostedSolution
{
    public class ExchangeContact
    {
        string displayName;
        string accountName;
        string emailAddress;
        bool hideFromAddressBook;

        string firstName;
        string initials;
        string lastName;

        string jobTitle;
        string company;
        string department;
        string office;
        ExchangeAccount managerAccount;

        string businessPhone;
        string fax;
        string homePhone;
        string mobilePhone;
        string pager;
        string webPage;

        string address;
        string city;
        string state;
        string zip;
        string country;

        string notes;
        string sAMAccountName;
        private int useMapiRichTextFormat;

        ExchangeAccount[] acceptAccounts;
        ExchangeAccount[] rejectAccounts;
        bool requireSenderAuthentication;

        [LogProperty]
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }

        [LogProperty]
        public string AccountName
        {
            get { return this.accountName; }
            set { this.accountName = value; }
        }

        [LogProperty]
        public string EmailAddress
        {
            get { return this.emailAddress; }
            set { this.emailAddress = value; }
        }

        public bool HideFromAddressBook
        {
            get { return this.hideFromAddressBook; }
            set { this.hideFromAddressBook = value; }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        public string Initials
        {
            get { return this.initials; }
            set { this.initials = value; }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }

        public string JobTitle
        {
            get { return this.jobTitle; }
            set { this.jobTitle = value; }
        }

        public string Company
        {
            get { return this.company; }
            set { this.company = value; }
        }

        public string Department
        {
            get { return this.department; }
            set { this.department = value; }
        }

        public string Office
        {
            get { return this.office; }
            set { this.office = value; }
        }

        public ExchangeAccount ManagerAccount
        {
            get { return this.managerAccount; }
            set { this.managerAccount = value; }
        }

        public string BusinessPhone
        {
            get { return this.businessPhone; }
            set { this.businessPhone = value; }
        }

        public string Fax
        {
            get { return this.fax; }
            set { this.fax = value; }
        }

        public string HomePhone
        {
            get { return this.homePhone; }
            set { this.homePhone = value; }
        }

        public string MobilePhone
        {
            get { return this.mobilePhone; }
            set { this.mobilePhone = value; }
        }

        public string Pager
        {
            get { return this.pager; }
            set { this.pager = value; }
        }

        public string WebPage
        {
            get { return this.webPage; }
            set { this.webPage = value; }
        }

        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }

        public string State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        public string Zip
        {
            get { return this.zip; }
            set { this.zip = value; }
        }

        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        public string Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        public ExchangeAccount[] AcceptAccounts
        {
            get { return this.acceptAccounts; }
            set { this.acceptAccounts = value; }
        }

        public ExchangeAccount[] RejectAccounts
        {
            get { return this.rejectAccounts; }
            set { this.rejectAccounts = value; }
        }

        public bool RequireSenderAuthentication
        {
            get { return requireSenderAuthentication; }
            set { requireSenderAuthentication = value; }
        }

        public int UseMapiRichTextFormat
        {
            get { return useMapiRichTextFormat; }
            set { useMapiRichTextFormat = value; }
        }

        [LogProperty]
        public string SAMAccountName
        {
            get { return sAMAccountName; }
            set { sAMAccountName = value; }
        }



    }
}
