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
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using SolidCP.EnterpriseServer;

namespace SolidCP.Ecommerce.EnterpriseServer
{
	public class EnomRegistrar : SystemPluginBase, IDomainRegistrar
	{
		public const string DEMO_SERVICE_URL = "http://resellertest.enom.com/interface.asp";
		public const string LIVE_SERVICE_URL = "http://reseller.enom.com/interface.asp";

		public const int PAGE_SIZE = 25;

		public override string[] SecureSettings
		{
			get
			{
				return new string[] { EnomSettings.PASSWORD };
			}
		}

		/// <summary>
		/// Gets service username
		/// </summary>
		public string Username
		{
			get { return PluginSettings[EnomSettings.USERNAME]; }
		}
		/// <summary>
		/// Gets service password
		/// </summary>
		public string Password
		{
			get { return PluginSettings[EnomSettings.PASSWORD]; }
		}
		/// <summary>
		/// Gets whether plug-in running in live mode
		/// </summary>
		public bool LiveMode
		{
			get
			{
				return Convert.ToBoolean(PluginSettings[EnomSettings.LIVE_MODE]);
			}
		}
		/// <summary>
		/// Gets service url
		/// </summary>
		public string ServiceUrl
		{
			get
			{
				if (LiveMode)
					return LIVE_SERVICE_URL;

				return DEMO_SERVICE_URL;
			}
		}

		public bool SubAccountRequired
		{
			get { return false; }
		}

		private NameValueCollection queryString;

		public EnomRegistrar()
		{
			queryString = new NameValueCollection();
		}

		#region Helper routines

		private void RaiseRegistrarException(EnomResult result)
		{
			string[] errors = new string[result.ErrorsCount];

			for (int i = 0; i < result.ErrorsCount; i++)
				errors[i] = result["Err" + (i + 1)];

			throw new Exception(String.Join(Environment.NewLine, errors));
		}

		/// <summary>
		/// Converts custom phone number to enom format: +AreaCode.PhoneNumber
		/// </summary>
		/// <param name="phone"></param>
		/// <returns></returns>
		private string ConvertToEnomPhoneFormat(string phone)
		{
			StringBuilder result = new StringBuilder();

			IEnumerator iterator = phone.ToCharArray().GetEnumerator();
			bool separatorAdded = false;

			while (iterator.MoveNext())
			{
				char symbol = (char)iterator.Current;

				if (Char.IsDigit(symbol))
				{
					result.Append(symbol);
				}
				else if (result.Length == 3)
				{
					result.Append(".");
					separatorAdded = true;
				}
			}

			if (result.Length == 0)
			{
				result.Append("+000.00000000");
			}
			else
			{
				if (!separatorAdded)
					result.Insert(2, ".");

				result.Insert(0, "+");
			}

			return result.ToString();
		}

		private void AddParam(string key, string value)
		{
			queryString.Add(key, value);
		}

		private string ExecuteCommand()
		{
			return ExecuteCommand(Username, Password);
		}

		private string ExecuteCommand(string username, string password)
		{
			try
			{
				// set enom credentials
				AddParam("UID", username);
				AddParam("PW", password);

				WebClient enom = new WebClient();
				enom.QueryString = queryString;

				return enom.DownloadString(ServiceUrl);
			}
			finally
			{
				queryString.Clear();
			}
		}

		private void AddCustomerContacts(DomainContacts contacts)
		{
			if (contacts != null)
			{
				foreach (string key in contacts.Keys)
				{
					DomainContact contact = contacts[key];

					string contactKey = key;

					if (key == "Billing")
						contactKey = "AuxBilling";

					AddParam(contactKey + "EmailAddress", contact["Email"]);
					AddParam(contactKey + "Fax", contact["Fax"]);
					AddParam(contactKey + "Phone", contact["Phone"]);
					AddParam(contactKey + "Country", contact["Country"]);
					AddParam(contactKey + "PostalCode", contact["Zip"]);

					// add state
					if (contact.HasKey("State"))
					{
						AddParam(contactKey + "StateProvinceChoice", "S");
						AddParam(contactKey + "StateProvince", contact["State"]);
					}
					// add province
					else if (contact.HasKey("Province"))
					{
						AddParam(contactKey + "StateProvinceChoice", "Province");
						AddParam(contactKey + "StateProvince", contact["Province"]);
					}
					// add blank
					else
					{
						AddParam(contactKey + "StateProvinceChoice", "Blank");
						AddParam(contactKey + "StateProvince", "");
					}

					AddParam(contactKey + "City", contact["City"]);
					AddParam(contactKey + "Address1", contact["Address"]);
					AddParam(contactKey + "Address2", contact["Address1"]);
					AddParam(contactKey + "LastName", contact["LastName"]);
					AddParam(contactKey + "FirstName", contact["FirstName"]);
					AddParam(contactKey + "JobTitle", contact["JobTitle"]);
					AddParam(contactKey + "OrganizationName", contact["Company"]);
				}
			}
		}

		private EnomResult PushDomainToSubAccount(CommandParams args)
		{
			AddParam("Command", "PushDomain");

			AddParam("SLD", args[CommandParams.DOMAIN_NAME]);
			AddParam("TLD", args[CommandParams.DOMAIN_TLD]);

			AddParam("AccountID", args[AccountResult.ACCOUNT_LOGIN_ID]);

			EnomResult enomResult = new EnomResult(
				ExecuteCommand()
			);

			// throws an exception
			if (!enomResult.Succeed)
				RaiseRegistrarException(enomResult);

			return enomResult;
		}

		public EnomResult ExecuteCommand(string commandName, KeyValueBunch commandArgs)
		{
			AddParam("Command", commandName);
			// copy attributes
			foreach (string keyName in commandArgs.GetAllKeys())
			{
				AddParam(keyName, commandArgs[keyName]);
			}
			//
			return new EnomResult(ExecuteCommand());
		}

		#endregion

		#region IDomainRegistrar Members

		private string registrarName;

		public string RegistrarName
		{
			get { return registrarName; }
			set { registrarName = value; }
		}

		public bool CheckSubAccountExists(string account, string emailAddress)
		{
			if (String.IsNullOrEmpty(account))
				throw new ArgumentNullException("account");

			bool userFound = false;

			int startPosition = 1;

			while (startPosition > -1)
			{
				AddParam("Command", "GetSubAccounts");

				AddParam("ListBy", "LoginID");
				AddParam("StartLetter", account.Substring(0, 1));
				AddParam("StartPosition", startPosition.ToString());

				EnomResult enomResult = new EnomResult(
					ExecuteCommand()
				);

				// throws an exception
				if (!enomResult.Succeed)
					RaiseRegistrarException(enomResult);

				int count = Convert.ToInt32(enomResult["Count"]);

				for (int i = 1; i <= PAGE_SIZE; i++)
				{
					string enomLogin = enomResult["LoginID" + i];

					if (String.Compare(enomLogin, account, true) == 0)
					{
						userFound = true;
						startPosition = -1;
						break;
					}
				}

				// perform next iteration
				if (!userFound)
				{
					startPosition += PAGE_SIZE;

					if (startPosition >= count)
						startPosition = -1;
				}
			}

			return userFound;
		}

		public AccountResult GetSubAccount(string account, string emailAddress)
		{
			if (String.IsNullOrEmpty(account))
				throw new ArgumentNullException("account");

			AccountResult result = new AccountResult();

			int startPosition = 1;

			bool userFound = false;

			while (startPosition > -1)
			{
				AddParam("Command", "GetSubAccounts");

				AddParam("ListBy", "LoginID");
				AddParam("StartLetter", account.Substring(0, 1));
				AddParam("StartPosition", startPosition.ToString());

				EnomResult enomResult = new EnomResult(
					ExecuteCommand()
				);

				// throws an exception
				if (!enomResult.Succeed)
					RaiseRegistrarException(enomResult);

				int count = Convert.ToInt32(enomResult["Count"]);

				for (int i = 1; i <= PAGE_SIZE; i++)
				{
					string enomLogin = enomResult["LoginID" + i];

					// copy values to result
					if (String.Compare(enomLogin, account, true) == 0)
					{
						userFound = true;

						result[AccountResult.ACCOUNT_LOGIN_ID] = enomLogin;
						result[AccountResult.ACCOUNT_ID] = enomResult["Account" + i];
						result[AccountResult.ACCOUNT_PARTY_ID] = enomResult["PartyID" + i];
						
						startPosition = -1;

						break;
					}
				}

				// perform next iteration
				if (userFound)
				{
					startPosition += PAGE_SIZE;

					if (startPosition >= count)
						startPosition = -1;
				}
			}

			return result;
		}

		public AccountResult CreateSubAccount(CommandParams args)
		{
			AccountResult result = new AccountResult();

			AddParam("Command", "CreateSubAccount");

			AddParam("NewUID", args["Username"]);
			AddParam("NewPW", args["Password"]);
			AddParam("ConfirmPW", args["Password"]);

			AddParam("RegistrantAddress1", args["Address"]);
			AddParam("RegistrantCity", args["City"]);
			AddParam("RegistrantCountry", args["Country"]);
			AddParam("RegistrantEmailAddress", args["Email"]);
			AddParam("RegistrantEmailAddress_Contact", args["Email"]);
			AddParam("RegistrantFax", ConvertToEnomPhoneFormat(args["Fax"]));
			AddParam("RegistrantFirstName", args["FirstName"]);
			AddParam("RegistrantLastName", args["LastName"]);
			AddParam("RegistrantOrganizationName", "For Personal Usage Only");
			AddParam("RegistrantPhone", ConvertToEnomPhoneFormat(args["Phone"]));
			AddParam("RegistrantPostalCode", args["Zip"]);

			// create enom
			EnomResult enomResult = new EnomResult(
				ExecuteCommand()
			);

			// raise an exception
			if (!enomResult.Succeed)
				RaiseRegistrarException(enomResult);

			// check customer status info
			if (enomResult["StatusCustomerInfo"] != "Successful")
				throw new Exception(enomResult["StatusCustomerInfo"]);

			result[AccountResult.ACCOUNT_LOGIN_ID] = args["Username"];
			result[AccountResult.ACCOUNT_ID] = enomResult["Account"];
			result[AccountResult.ACCOUNT_PARTY_ID] = enomResult["PartyID"];

			return result;
		}

		public DomainStatus CheckDomain(string domain)
		{
			AddParam("Command", "Check");

			//AddDomainParam(domain, false);

			// gets a result from API
			EnomResult enomResult = new EnomResult(
				ExecuteCommand()
			);
			// domain available to registration
			if (enomResult["RRPCode"] == "210")
				return DomainStatus.NotFound;

			return DomainStatus.Registered;
		}

		public void RegisterDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers)
		{
			AddParam("Command", "Purchase");
			// as per Enom API reference this parameter use is recommended in production mode
			// as it uses a queued order peorcessing instead of real-time.
			AddParam("QueueOrder", "1");

			string domainName = GetDomainName(domainSvc.Fqdn);
			string domainTld = GetDomainTLD(domainSvc.Fqdn);

			// add domain
			AddParam("SLD", domainName);
			AddParam("TLD", domainTld);
			AddParam("NumYears", domainSvc.PeriodLength.ToString());

			if (LiveMode && (nameServers != null && nameServers.Length > 0))
			{
				// load name servers
				for (int i = 0; i < nameServers.Length; i++)
					AddParam(String.Concat("NS", i + 1), nameServers[i].Trim());
			}
			else
			{
				// use Enom's name servers
				AddParam("UseDNS", "default");
			}

			if (domainSvc.Fqdn.EndsWith(".uk"))
			{
				// special stub for org.uk and org.uk domains
				// org.uk and co.uk tlds should have at least 2 NS
				if (nameServers != null && nameServers.Length == 1)
				{
					// we already have first ns added
					// so push the second ns
					AddParam("NS2", nameServers[0].Trim());
				}
				AddParam("registered_for", domainSvc["RegisteredFor"]);
				AddParam("uk_legal_type", domainSvc["UK_LegalType"]);
				AddParam("uk_reg_co_no", domainSvc["UK_CompanyIdNumber"]);
				AddParam("uk_reg_opt_out", domainSvc["HideWhoisInfo"]);
			}
			// us TLDs extensions
			else if (domainSvc.Fqdn.EndsWith(".us"))
			{
                AddParam("global_cc_us", accountInfo[ContractAccount.COUNTRY]);
				AddParam("us_nexus", domainSvc["NexusCategory"]);
				AddParam("us_purpose", domainSvc["ApplicationPurpose"]);
			}
			// eu TLDs extensions
			else if (domainSvc.Fqdn.EndsWith(".eu"))
			{
				AddParam("eu_whoispolicy", domainSvc["EU_WhoisPolicy"]);
				AddParam("eu_agreedelete", domainSvc["EU_AgreeDelete"]);
				AddParam("eu_adr_lang", domainSvc["EU_ADRLang"]);
				//
				AddParam("AdminOrganizationName", accountInfo[ContractAccount.COMPANY_NAME]);
                AddParam("AdminFirstName", accountInfo[ContractAccount.FIRST_NAME]);
                AddParam("AdminLastName", accountInfo[ContractAccount.LAST_NAME]);
                AddParam("AdminAddress1", accountInfo[ContractAccount.ADDRESS]);
                AddParam("AdminAddress2", accountInfo[ContractAccount.ADDRESS]);
                AddParam("AdminCity", accountInfo[ContractAccount.CITY]);
				AddParam("AdminStateProvinceChoice", "P");
                AddParam("AdminProvince", accountInfo[ContractAccount.STATE]);
                AddParam("AdminPostalCode", accountInfo[ContractAccount.ZIP]);
                AddParam("AdminCountry", accountInfo[ContractAccount.COUNTRY]);
                AddParam("AdminEmailAddress", accountInfo[ContractAccount.EMAIL]);
                AddParam("AdminPhone", accountInfo[ContractAccount.PHONE_NUMBER]);
                AddParam("AdminFax", accountInfo[ContractAccount.FAX_NUMBER]);
				AddParam("AdminJobTitle", "Administrator");
				AddParam("RegistrantJobTitle", "Registrant");
			}

			AddParam("UseCreditCard", "no");
            AddParam("RegistrantFirstName", accountInfo[ContractAccount.FIRST_NAME]);
            AddParam("RegistrantLastName", accountInfo[ContractAccount.LAST_NAME]);
            AddParam("RegistrantAddress1", accountInfo[ContractAccount.ADDRESS]);
            AddParam("RegistrantCity", accountInfo[ContractAccount.CITY]);
            AddParam("RegistrantEmailAddress", accountInfo[ContractAccount.EMAIL]);
            AddParam("RegistrantPhone", accountInfo[ContractAccount.PHONE_NUMBER]);
            AddParam("RegistrantCountry", accountInfo[ContractAccount.COUNTRY]);
            AddParam("RegistrantStateProvince", accountInfo[ContractAccount.STATE]);
            AddParam("RegistrantPostalCode", accountInfo[ContractAccount.ZIP]);
			
			//}

			// unlock registrar
			/*if (domainSvc["LockRegistrar"] == "0")
				AddParam("UnLockRegistrar", "1");*/

			// load contacts
			//AddCustomerContacts(contacts);

			// return enom result
			EnomResult enomResult = new EnomResult(
				ExecuteCommand()
			);

			// throws an exception
			if (!enomResult.Succeed)
				RaiseRegistrarException(enomResult);

			// if something wrong was happend then throws an exception
			if (enomResult["RRPCode"] != "200")
			{
				throw new Exception(
					"Reason Code: " + enomResult["RRPCode"] + "; Description: " + enomResult["RRPText"]
				);
			}
			// copy order if
			domainSvc["OrderID"] = enomResult["OrderID"];

			//PushDomainToSubAccount(args);
		}

		public void RenewDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers)
		{
			string domainName = GetDomainName(domainSvc.Fqdn);
			string domainTld = GetDomainTLD(domainSvc.Fqdn);

			AddParam("Command", "Extend");

			AddParam("SLD1", domainName);
			AddParam("TLD1", domainTld);

			AddParam("NumYears", domainSvc.PeriodLength.ToString());

			EnomResult enomResult = new EnomResult(
				ExecuteCommand()
			);

			// throws an exception
			if (!enomResult.Succeed)
				RaiseRegistrarException(enomResult);

			// something wrong was happend we should throw an exception
			if (enomResult["RRPCode"] != "200")
			{
				throw new Exception(
					"Reason Code: " + enomResult["RRPCode"] + "; Description: " + enomResult["RRPText"]
				);
			}

			domainSvc["OrderID"] = enomResult["OrderID"];
		}

		public TransferDomainResult TransferDomain(CommandParams args, DomainContacts contacts)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}

	public class EnomResult
	{
		private NameValueCollection commandKeys;
		private string rawResponse;
		private bool succeed;
		private int errorsCount;

		public int ErrorsCount
		{
			get { return errorsCount; }
		}

		public bool Succeed
		{
			get { return succeed; }
		}

		internal EnomResult(string rawResponse)
		{
			this.rawResponse = rawResponse;

			ParseCommandResponse();
		}

		private void ParseCommandResponse()
		{
			// split raw respose to key/value pairs
			string[] lines = rawResponse.Split(
				new string[] { Environment.NewLine },
				StringSplitOptions.RemoveEmptyEntries
			);

			commandKeys = new NameValueCollection();

			// fill command keys with values
			foreach (string line in lines)
			{
				// check for enom API auto-comment line
				if (line.StartsWith(";")) continue; // EXIT
				// check for empty line
				if (String.IsNullOrEmpty(line)) continue; // EXIT
				// check line is header
				if (line.StartsWith("<!--")) continue; // EXIT

				// parse command response line
				string[] bunch = line.Split('=');
				commandKeys.Add(bunch[0], bunch[1]);
			}

			// set errors count
			errorsCount = Convert.ToInt32(commandKeys["ErrCount"]);

			// set command succeed status
			if (errorsCount == 0)
				succeed = true;
		}

		public string this[string keyName]
		{
			get { return commandKeys[keyName]; }
		}
	}
}
