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

namespace SolidCP.Providers.HostedSolution
{
	public class ExchangePublicFolder
	{
		string name;
        string netbios;
		string displayName;
        string sAMAccountName;
		bool hideFromAddressBook;
		bool mailEnabled;

		ExchangeAccount[] accounts;	
		
		ExchangeAccount[] acceptAccounts;
		ExchangeAccount[] rejectAccounts;
		bool requireSenderAuthentication;

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public bool HideFromAddressBook
		{
			get { return this.hideFromAddressBook; }
			set { this.hideFromAddressBook = value; }
		}

		public bool MailEnabled
		{
			get { return this.mailEnabled; }
			set { this.mailEnabled = value; }
		}

		        
		public SolidCP.Providers.HostedSolution.ExchangeAccount[] Accounts
        {
            get { return this.accounts; }
            set { this.accounts = value; }
        }
        
		public SolidCP.Providers.HostedSolution.ExchangeAccount[] AcceptAccounts
		{
			get { return this.acceptAccounts; }
			set { this.acceptAccounts = value; }
		}

		public SolidCP.Providers.HostedSolution.ExchangeAccount[] RejectAccounts
		{
			get { return this.rejectAccounts; }
			set { this.rejectAccounts = value; }
		}

		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public bool RequireSenderAuthentication
		{
			get { return requireSenderAuthentication; }
			set { requireSenderAuthentication = value; }
		}

        public string SAMAccountName
        {
            get { return this.sAMAccountName; }
            set { this.sAMAccountName = value; }
        }

        public string NETBIOS
        {
            get { return this.netbios; }
            set { this.netbios = value; }
        }


	}
}
