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
	public class DomainName : ProvisioningControllerBase, IProvisioningController
	{
		public const string REGISTRAR_ORDER_ID = "RegistrarOrderID";
		public const string REGISTRAR = "Registrar";
		public const string INTERNAL_DOMAIN_ID = "InternalDomainID";
		public const string DOMAIN_NAME = "DomainName";
		public const string DOMAIN_TLD = "DomainTLD";
		public const string RENEW_ORDER_ID = "RenewOrderID";
		public const string ROOT_SERVICE_ID = "RootServiceID";
		public const string PACKAGE_ID = "PackageID";
		private IDomainRegistrar registrar;
		/// <summary>
		/// Gets domain name
		/// </summary>
		public string Domain
		{
			get { return ServiceSettings[DOMAIN_NAME];  }
		}
		/// <summary>
		/// Gets domain name tld
		/// </summary>
		public string TLD
		{
			get { return ServiceSettings[DOMAIN_TLD]; }
		}
		/// <summary>
		/// Gets full qualified domain name
		/// </summary>
		public string FQDN
		{
			get { return Domain + "." + TLD; }
		}

		/// <summary>
		/// Gets a reference to the registrar instance
		/// </summary>
		protected IDomainRegistrar Registrar
		{
			get { return registrar; }
		}

		/// <summary>
		/// Gets registrar settings name
		/// </summary>
		protected string SettingsName
		{
			get { return Registrar.RegistrarName + "Settings"; }
		}

		public DomainName(Service serviceInfo)
			: base(serviceInfo)
		{
		}

		public override void LoadProvisioningSettings()
		{
			base.LoadProvisioningSettings();

			InitRegistrar();
		}

		private void InitRegistrar()
		{
			string tld = TLD;
			// check whether tld is already obtained
			if (String.IsNullOrEmpty(TLD))
				tld = RegistrarController.GetDomainTLD(TLD);
			// 
			registrar = RegistrarController.GetTLDDomainRegistrar(ServiceInfo.SpaceId, tld);
		}

		private void CreateSolidCPDomain(int packageId)
		{
			// try to register domain in panel
			DomainInfo domain = ServerController.GetDomain(FQDN);

			// domain not found thus create newly one
			if (domain == null)
			{
				domain = new DomainInfo();

				domain.DomainName = FQDN;
				domain.HostingAllowed = false;
				domain.PackageId = packageId;

				domain.DomainId = ServerController.AddDomain(domain);
			}

			// save domain id if it's created
			if (domain.DomainId > 0)
				ServiceSettings[INTERNAL_DOMAIN_ID] = domain.DomainId.ToString();
		}

		private void RenewDomainOnRegistrar()
		{
			// read service cycle
			ServiceLifeCycle lifeCycle = ServiceController.GetServiceLifeCycle(
				ServiceInfo.SpaceId,
				ServiceInfo.ServiceId
			);

			// copy renew arguments
			CommandParams args = new CommandParams();
			args[CommandParams.DOMAIN_NAME] = Domain;
			args[CommandParams.DOMAIN_TLD] = TLD;
			args[CommandParams.YEARS] = lifeCycle.CycleLength.ToString();

			// renew remote domain
			RenewDomainResult result = Registrar.RenewDomain(args);

			// save renew order id
			ServiceSettings[RENEW_ORDER_ID] = result[RenewDomainResult.RENEW_ORDER_NUMBER];
			ServiceSettings[REGISTRAR] = result[RenewDomainResult.REGISTRAR];

			// renew service life-cycle suspend date
			DateTime StartDate = ServiceController.GetServiceSuspendDate(
				ServiceInfo.SpaceId,
				ServiceInfo.ServiceId
			);
			
			// calculate end date
			DateTime EndDate = StartDate.AddYears(lifeCycle.CycleLength);

			// append life-cycle record
			ServiceController.SetServiceLifeCycleRecord(
				ServiceInfo.SpaceId,
				ServiceInfo.ServiceId,
				StartDate,
				EndDate
			);
		}

		private CommandParams PrepeareAccountParams()
		{
			CommandParams args = new CommandParams();

			args[CommandParams.USERNAME] = UserInfo.Username;
			args[CommandParams.PASSWORD] = UserInfo.Password;
			args[CommandParams.FIRST_NAME] = UserInfo.FirstName;
			args[CommandParams.LAST_NAME] = UserInfo.LastName;
			args[CommandParams.EMAIL] = UserInfo.Email;
			args[CommandParams.ADDRESS] = UserInfo.Address;
			args[CommandParams.CITY] = UserInfo.City;
			args[CommandParams.STATE] = UserInfo.State;
			args[CommandParams.COUNTRY] = UserInfo.Country;
			args[CommandParams.ZIP] = UserInfo.Zip;
			args[CommandParams.PHONE] = UserInfo.PrimaryPhone;
			args[CommandParams.FAX] = UserInfo.Fax;

			return args;
		}

		private AccountResult GetRegistrarAccountInfo()
		{
			string userAccount = UserInfo.Username;
			string emailAddress = UserInfo.Email;

			// check for user account
			bool exists = Registrar.CheckSubAccountExists(userAccount, emailAddress);

			// sub-account doesn't exist then create it
			if (!exists)
			{
				// create account params bunch
				CommandParams accountArgs = PrepeareAccountParams();
				// create sub-account
				return Registrar.CreateSubAccount(accountArgs);
			}

			// just get sub-account info on registrar's side
			return Registrar.GetSubAccount(userAccount, emailAddress);
		}

		private void EnsureUserSettings(CommandParams args)
		{
			// load user settings
			UserSettings regSettings = UserController.GetUserSettings(
				ServiceInfo.UserId,
				SettingsName
			);

			// settings are empty
			if (regSettings.SettingsArray == null || regSettings.SettingsArray.Length == 0)
			{
				// load registrar account info
				AccountResult rmResult = GetRegistrarAccountInfo();

				// update user settings locally
				regSettings.SettingsName = SettingsName;
				regSettings.UserId = UserInfo.UserId;

				// copy account data
				foreach (string key in rmResult.AllKeys)
				{
					regSettings[key] = rmResult[key];
					args[key] = rmResult[key];
				}

				// save sub-account settings
				UserController.UpdateUserSettings(regSettings);
			}
			else
			{
				// copy user settings
				foreach (string[] pair in regSettings.SettingsArray)
				{
					args[pair[0]] = pair[1];
				}
			}
		}

		private void CreateRegistrarDomain(int packageId)
		{
			// create command arguments
			CommandParams args = PrepeareAccountParams();
			// ensure user settings are ok & copy them
			EnsureUserSettings(args);

			// get package info
			PackageSettings nsSettings = PackageController.GetPackageSettings(
				packageId,
				PackageSettings.NAME_SERVERS
			);
			// read service cycle
			ServiceLifeCycle lifeCycle = ServiceController.GetServiceLifeCycle(
				ServiceInfo.SpaceId,
				ServiceInfo.ServiceId
			);
			// copy domain name related settings
			args[CommandParams.NAME_SERVERS] = nsSettings[PackageSettings.NAME_SERVERS];
			args[CommandParams.DOMAIN_NAME] = Domain;
			args[CommandParams.DOMAIN_TLD] = TLD;
			args[CommandParams.YEARS] = lifeCycle.CycleLength.ToString();

			// call registrar's API
			RegisterDomainResult rdResult = Registrar.RegisterDomain(args);
			// save registrar's order number
			ServiceSettings[REGISTRAR_ORDER_ID] = rdResult[RegisterDomainResult.ORDER_NUMBER];
			ServiceSettings[REGISTRAR] = Registrar.RegistrarName;
		}

		private void RemoveDomainOnPanel()
		{
			int domainId = Utils.ParseInt(ServiceSettings[INTERNAL_DOMAIN_ID], 0);

			if (domainId > 0)
				ServerController.DeleteDomain(domainId);
		}

		#region IProvisioningController Members

		public void Activate()
		{
			RenewDomainOnRegistrar();
		}

		public void Suspend() {}

		public void Cancel() {}

		public void Order()
		{
			int parentId = Convert.ToInt32(ServiceSettings[ROOT_SERVICE_ID]);

			KeyValueBunch rootSettings = ServiceController.GetServiceSettings(
				ServiceInfo.SpaceId,
				parentId
			);

			// check whether the parent service has been provisioned
			if (String.IsNullOrEmpty(rootSettings[PACKAGE_ID]))
			{
				throw new Exception(
					"Unable to provision service because parent service not provisioned yet"
				);
			}

			int packageId = Utils.ParseInt(rootSettings[PACKAGE_ID], -1);

			// create remote domain on registrar's side
			CreateRegistrarDomain(packageId);
			// create local domain on SolidCP's side
			CreateSolidCPDomain(packageId);
		}

		public void Rollback()
		{
			RemoveDomainOnPanel();
		}

		#endregion
	}
}
