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

using SolidCP.EnterpriseServer;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	public class OfflineRegistrar : SystemPluginBase, IDomainRegistrar
	{
		private string registrarName;

		public OfflineRegistrar()
		{
		}

		#region IDomainRegistrar Members

		public bool SubAccountRequired
		{
			get { return false; }
		}

		public string RegistrarName
		{
			get { return registrarName; }
			set { registrarName = value; }
		}

		public bool CheckSubAccountExists(string account, string emailAddress)
		{
			return true;
		}

		public AccountResult GetSubAccount(string account, string emailAddress)
		{
			AccountResult result = new AccountResult();

			return result;
		}

		public AccountResult CreateSubAccount(CommandParams args)
		{
			AccountResult result = new AccountResult();

			return result;
		}

		public DomainStatus CheckDomain(string domain)
		{
			return DomainStatus.NotFound;
		}

		public void RegisterDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers)
		{
			domainSvc["OrderID"] = DateTime.Now.ToString("yyyy-MM-dd") + "-" + domainSvc.Fqdn;
		}

		public void RenewDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers)
		{
			domainSvc["OrderID"] = DateTime.Now.ToString("yyyy-MM-dd") + "-" + domainSvc.Fqdn;
		}

		public TransferDomainResult TransferDomain(CommandParams args, DomainContacts contacts)
		{
			TransferDomainResult result = new TransferDomainResult();

			result[TransferDomainResult.TRANSFER_ORDER_NUMBER] = DateTime.Now.ToString("yyyy-MM-dd") + "-" + args[CommandParams.DOMAIN_NAME];

			return result;
		}

		#endregion
	}
}
