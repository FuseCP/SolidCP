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
using System.Collections;

namespace SolidCP.Providers.Mail
{ 
	public interface IMailServer 
	{
		// mail domains
		bool DomainExists(string domainName);
		string[] GetDomains();
		MailDomain GetDomain(string domainName); 
		void CreateDomain(MailDomain domain); 
		void UpdateDomain(MailDomain domain); 
		void DeleteDomain(string domainName);
 
		// mail aliases
		bool DomainAliasExists(string domainName, string aliasName);
		string[] GetDomainAliases(string domainName);
		void AddDomainAlias(string domainName, string aliasName);
		void DeleteDomainAlias(string domainName, string aliasName);

		// mailboxes
		bool AccountExists(string mailboxName);
		MailAccount[] GetAccounts(string domainName);
        MailAccount GetAccount(string mailboxName);
        void CreateAccount(MailAccount mailbox);
        void UpdateAccount(MailAccount mailbox);
        void DeleteAccount(string mailboxName);

        //forwardings (mail aliases)
	    bool MailAliasExists(string mailAliasName);
	    MailAlias[] GetMailAliases(string domainName);
	    MailAlias GetMailAlias(string mailAliasName);
	    void CreateMailAlias(MailAlias mailAlias);
	    void UpdateMailAlias(MailAlias mailAlias);
        void DeleteMailAlias(string mailAliasName);


		// groups
		bool GroupExists(string groupName);
		MailGroup[] GetGroups(string domainName); 
		MailGroup GetGroup(string groupName); 
		void CreateGroup(MailGroup group); 
		void UpdateGroup(MailGroup group); 
		void DeleteGroup(string groupName);

		// mailing lists
		bool ListExists(string maillistName);
        MailList[] GetLists(string domainName);
        MailList GetList(string maillistName);
        void CreateList(MailList maillist);
        void UpdateList(MailList maillist);
        void DeleteList(string maillistName);
	} 
}
