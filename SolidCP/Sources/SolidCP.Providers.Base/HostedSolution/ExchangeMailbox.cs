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
	public class ExchangeMailbox
	{	   	    
        string displayName;
		string accountName;
		bool hideFromAddressBook;
		bool disabled;

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

		bool enableForwarding;
        int saveSentItems;
        ExchangeAccount forwardingAccount;
		bool doNotDeleteOnForward;

		ExchangeAccount[] sendOnBehalfAccounts;
		ExchangeAccount[] acceptAccounts;
		ExchangeAccount[] rejectAccounts;

		ExchangeAccount[] fullAccessAccounts;
		ExchangeAccount[] sendAsAccounts;

		bool requireSenderAuthentication;
		int maxRecipients;
		int maxSendMessageSizeKB;
		int maxReceiveMessageSizeKB;

		bool enablePOP;
		bool enableIMAP;
		bool enableOWA;
		bool enableMAPI;
		bool enableActiveSync;
		
		long issueWarningKB;
        long prohibitSendKB;
        long prohibitSendReceiveKB;
		int keepDeletedItemsDays;

		private string domain;

		int totalItems;
		int totalSizeMB;
		DateTime lastLogon;
		DateTime lastLogoff;

        bool enableLitigationHold;
        long recoverabelItemsSpace;
        long recoverabelItemsWarning;

        string exchangeGuid;



        public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public string AccountName
		{
			get { return this.accountName; }
			set { this.accountName = value; }
		}

		public bool HideFromAddressBook
		{
			get { return this.hideFromAddressBook; }
			set { this.hideFromAddressBook = value; }
		}

		public bool Disabled
		{
			get { return this.disabled; }
			set { this.disabled = value; }
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

		public bool EnableForwarding
		{
			get { return this.enableForwarding; }
			set { this.enableForwarding = value; }
		}

        public int SaveSentItems
        {
            get { return this.saveSentItems; }
            set { this.saveSentItems = value; }
        }

        public ExchangeAccount ForwardingAccount
		{
			get { return this.forwardingAccount; }
			set { this.forwardingAccount = value; }
		}

		public bool DoNotDeleteOnForward
		{
			get { return this.doNotDeleteOnForward; }
			set { this.doNotDeleteOnForward = value; }
		}

		public ExchangeAccount[] SendOnBehalfAccounts
		{
			get { return this.sendOnBehalfAccounts; }
			set { this.sendOnBehalfAccounts = value; }
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

		public int MaxRecipients
		{
			get { return this.maxRecipients; }
			set { this.maxRecipients = value; }
		}

		public int MaxSendMessageSizeKB
		{
			get { return this.maxSendMessageSizeKB; }
			set { this.maxSendMessageSizeKB = value; }
		}

		public int MaxReceiveMessageSizeKB
		{
			get { return this.maxReceiveMessageSizeKB; }
			set { this.maxReceiveMessageSizeKB = value; }
		}

		public bool EnablePOP
		{
			get { return this.enablePOP; }
			set { this.enablePOP = value; }
		}

		public bool EnableIMAP
		{
			get { return this.enableIMAP; }
			set { this.enableIMAP = value; }
		}

		public bool EnableOWA
		{
			get { return this.enableOWA; }
			set { this.enableOWA = value; }
		}

		public bool EnableMAPI
		{
			get { return this.enableMAPI; }
			set { this.enableMAPI = value; }
		}

		public bool EnableActiveSync
		{
			get { return this.enableActiveSync; }
			set { this.enableActiveSync = value; }
		}

		public long IssueWarningKB
		{
			get { return this.issueWarningKB; }
			set { this.issueWarningKB = value; }
		}

		public long ProhibitSendKB
		{
			get { return this.prohibitSendKB; }
			set { this.prohibitSendKB = value; }
		}

		public long ProhibitSendReceiveKB
		{
			get { return this.prohibitSendReceiveKB; }
			set { this.prohibitSendReceiveKB = value; }
		}

		public int KeepDeletedItemsDays
		{
			get { return this.keepDeletedItemsDays; }
			set { this.keepDeletedItemsDays = value; }
		}

		public string Domain
		{
			get { return domain; }
			set { domain = value; }
		}

		public int TotalItems
		{
			get { return this.totalItems; }
			set { this.totalItems = value; }
		}

		public int TotalSizeMB
		{
			get { return this.totalSizeMB; }
			set { this.totalSizeMB = value; }
		}

		public DateTime LastLogon
		{
			get { return this.lastLogon; }
			set { this.lastLogon = value; }
		}

		public DateTime LastLogoff
		{
			get { return this.lastLogoff; }
			set { this.lastLogoff = value; }
		}

		public bool RequireSenderAuthentication
		{
			get { return requireSenderAuthentication; }
			set { requireSenderAuthentication = value; }
		}
		
		public ExchangeAccount[] SendAsAccounts
		{
			get { return sendAsAccounts; }
			set { sendAsAccounts = value; }
		}
		
		public ExchangeAccount[] FullAccessAccounts
		{
			get { return fullAccessAccounts; }
			set { fullAccessAccounts = value; }
		}

	    public ExchangeAccount[] OnBehalfOfAccounts { get; set; }
        public ExchangeAccount[] CalendarAccounts { get; set; }
        public ExchangeAccount[] ContactAccounts { get; set; }

        public bool EnableLitigationHold
        {
            get { return enableLitigationHold; }
            set { enableLitigationHold = value; }
        }


        public long RecoverabelItemsSpace
        {
            get { return this.recoverabelItemsSpace; }
            set { this.recoverabelItemsSpace = value; }
        }

        public long RecoverabelItemsWarning
        {
            get { return this.recoverabelItemsWarning; }
            set { this.recoverabelItemsWarning = value; }
        }

        public string ExchangeGuid
        {
            get { return this.exchangeGuid; }
            set { this.exchangeGuid = value; }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(accountName) ? accountName : base.ToString();
        }

	}
}
