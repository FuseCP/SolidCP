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
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers;
using System.Threading;
using System.DirectoryServices;


namespace SolidCP.Import.Enterprise
{
	public class OrganizationImporter
	{
		public const int EXCHANGE_CONTAINER_ERROR = -2790;
		
		private Label lblProcess;
		private ProgressBar progressBar;
		private ApplicationForm appForm;
		private Button btnImport;
		private Thread thread;


		public OrganizationImporter()
		{

		}

		public void Initialize(string username, ApplicationForm appForm)
		{
			this.appForm = appForm;
			this.lblProcess = appForm.lblMessage;
			this.progressBar = appForm.progressBar;
			this.btnImport = appForm.btnStart;
			try
			{
				UserInfo info = UserController.GetUser(username);
				SecurityContext.SetThreadPrincipal(info);
			}
			catch (Exception ex)
			{
				ShowError("Unable to authenticate user", ex);
				Cancel();
			}
		}

		public void Start()
		{
			Log.WriteInfo("Import started");
			btnImport.Enabled = false;
			appForm.ImportStarted = true;
			thread = new Thread(new ThreadStart(this.StartThread));
			thread.Start();
		}

		public void Cancel()
		{
			Log.WriteInfo("Import canceled");
			appForm.ImportStarted = false;
			btnImport.Enabled = true;
			this.lblProcess.Text = string.Empty;
			this.progressBar.Value = 0;
		}

		public void Abort()
		{
			Cancel();
			AbortThread();

		}

		private void AbortThread()
		{
			if (this.thread != null)
			{
				if (this.thread.IsAlive)
				{
					this.thread.Abort();
				}
				this.thread.Join();
			}
		}

		public DialogResult ShowError(string text, Exception ex)
		{
			Log.WriteError(text, ex);
			return MessageBox.Show(appForm, text, appForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void StartThread()
		{
			this.progressBar.Value = 0;

			try
			{
				this.lblProcess.Text = "Creating import script...";
				appForm.Update();

				//default actions
				List<ImportAction> actions = GetImportActions();

				//process actions
				for (int i = 0; i < actions.Count; i++)
				{
					ImportAction action = actions[i];
					this.lblProcess.Text = action.Description;
					this.progressBar.Value = i * 100 / actions.Count;
					appForm.Update();

					switch (action.ActionType)
					{
						case ActionTypes.ImportOrganization:
							ImportOrganization();
							break;

						case ActionTypes.ImportOrganizationDomain:
							ImportDomain(action.Name);
							break;

						case ActionTypes.ImportMailbox:
							ImportMailbox(action.DirectoryEntry);
							break;

						case ActionTypes.ImportContact:
							ImportContact(action.DirectoryEntry);
							break;

						case ActionTypes.ImportGroup:
							ImportGroup(action.DirectoryEntry);
							break;

					}
				}
				this.progressBar.Value = 100;

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				string message = Global.ErrorMessage;
				if ( string.IsNullOrEmpty(message))
					message = "An unexpected error has occurred during import.";
				ShowError(message, ex);
				Cancel();
				return;
			}

			this.lblProcess.Text = string.Empty;
			DialogResult dialogResult = DialogResult.No;  
			if (Global.HasErrors)
			{
				dialogResult = MessageBox.Show(appForm, "Import completed with errors. Would you like to see import log?", appForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			}
			else
			{
				dialogResult = MessageBox.Show(appForm, "Import completed successfully. Would you like to see import log?", appForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
			}
			if (dialogResult == DialogResult.Yes)
			{
				Log.ShowLogFile();
			}
			
			btnImport.Enabled = true;
			appForm.ImportStarted = false;
			this.progressBar.Value = 0;
		}

		public static bool IsThreadAbortException(Exception ex)
		{
			Exception innerException = ex;
			while (innerException != null)
			{
				if (innerException is System.Threading.ThreadAbortException)
					return true;
				innerException = innerException.InnerException;
			}

			string str = ex.ToString();
			return str.Contains("System.Threading.ThreadAbortException");
		}

		private List<ImportAction> GetImportActions()
		{
			List<ImportAction> list = new List<ImportAction>();
			ImportAction action = null;

			if (Global.ImportAccountsOnly)
			{
				ServiceProviderItem item = PackageController.GetPackageItemByName(Global.Space.PackageId, ResourceGroups.HostedOrganizations, Global.OrganizationName, typeof(Organization));
				if (item == null)
				{
					Global.ErrorMessage = string.Format("Organization {0} not found.", Global.OrganizationName);
					throw new Exception(Global.ErrorMessage);
				}
				Global.ItemId = item.Id;
			}
			else
			{
				action = new ImportAction(ActionTypes.ImportOrganization);
				action.Description = "Importing organization...";
				list.Add(action);

				DirectoryEntry org = Global.OrgDirectoryEntry;
				PropertyValueCollection props = org.Properties["uPNSuffixes"];
				if (props != null)
				{
					foreach (string domainName in props)
					{
						action = new ImportAction(ActionTypes.ImportOrganizationDomain);
						action.Description = "Importing organization domains...";
						action.Name = domainName;
						list.Add(action);
					}
				}
			}

			if (Global.SelectedAccounts != null)
			{
				foreach (DirectoryEntry entry in Global.SelectedAccounts)
				{
					switch (entry.SchemaClassName)
					{
						case "user":
							action = new ImportAction(ActionTypes.ImportMailbox);
							action.Description = "Importing mailbox...";
							action.DirectoryEntry = entry;
							list.Add(action);
							break;
						case "contact":
							action = new ImportAction(ActionTypes.ImportContact);
							action.Description = "Importing contact...";
							action.DirectoryEntry = entry;
							list.Add(action);
							break;
						case "group":
							action = new ImportAction(ActionTypes.ImportGroup);
							action.Description = "Importing group...";
							action.DirectoryEntry = entry;
							list.Add(action);
							break;
					}
				}
			}


			return list;
		}

		private void ImportOrganization()
		{

			PackageInfo packageInfo = Global.Space;
			int serviceId = PackageController.GetPackageServiceId(packageInfo.PackageId, ResourceGroups.HostedOrganizations);
			ServiceInfo serviceInfo = ServerController.GetServiceInfo(serviceId);
			StringDictionary serviceSettings = ServerController.GetServiceSettingsAdmin(serviceId);

			string tempDomain = serviceSettings["TempDomain"];
			ServerInfo serverInfo = ServerController.GetServerById(serviceInfo.ServerId);

			serviceId = PackageController.GetPackageServiceId(packageInfo.PackageId, ResourceGroups.Exchange);
			serviceSettings = ServerController.GetServiceSettingsAdmin(serviceId);
			Global.MailboxCluster = serviceSettings["MailboxCluster"];
			Global.StorageGroup = serviceSettings["StorageGroup"];
			Global.MailboxDatabase = serviceSettings["MailboxDatabase"];
			Global.KeepDeletedMailboxesDays = serviceSettings["KeepDeletedMailboxesDays"];
			Global.KeepDeletedItemsDays = serviceSettings["KeepDeletedItemsDays"];

			int ret = CreateOrganization(Global.Space.PackageId, Global.OrganizationId, Global.OrganizationName);
			if (ret > 0)
			{
				Global.ItemId = ret;
			}
			else
			{
				switch (ret)
				{
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_DEMO:
						Global.ErrorMessage = "You cannot import organization in demo account";
						break;
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_PENDING:
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_SUSPENDED:
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_CANCELLED:
					case BusinessErrorCodes.ERROR_USER_ACCOUNT_SHOULD_BE_ADMINISTRATOR:
						Global.ErrorMessage = "User account is disabled or does not have enough permissions";
						break;
					case BusinessErrorCodes.ERROR_PACKAGE_NOT_FOUND:
						Global.ErrorMessage = "Space not found";
						break;


					case BusinessErrorCodes.ERROR_PACKAGE_CANCELLED:
					case BusinessErrorCodes.ERROR_PACKAGE_SUSPENDED:
						Global.ErrorMessage = "Space is cancelled or suspended";
						break;

					case BusinessErrorCodes.ERROR_ORGS_RESOURCE_QUOTA_LIMIT:
						Global.ErrorMessage = "Organizations quota limit";
						break;

					case BusinessErrorCodes.ERROR_DOMAIN_QUOTA_LIMIT:
					case BusinessErrorCodes.ERROR_SUBDOMAIN_QUOTA_LIMIT:
						Global.ErrorMessage = "Domains quota limit";
						break;

					case BusinessErrorCodes.ERROR_ORG_ID_EXISTS:
						Global.ErrorMessage = "Organization with specified Id already exists";
						break;

					case BusinessErrorCodes.ERROR_ORGANIZATION_TEMP_DOMAIN_IS_NOT_SPECIFIED:
						Global.ErrorMessage = "Organization temp domain name is not specified";
						break;
					case BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS:
					case BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE:
						Global.ErrorMessage = "Domain already exists or is in use";
						break;
				}
				throw new Exception(string.Format("Unable to create organization. Error code: {0}", ret));
			}
		}



		public static int CreateOrganization(int packageId, string organizationId, string organizationName)
		{
			int itemId;
			int errorCode;

			// place log record
			Log.WriteStart(string.Format("Importing organization {0}...", organizationName));
			if (!CheckQuotas(packageId, out errorCode))
				return errorCode;
			
			string addressListsContainer = null;
			try
			{
				addressListsContainer = ADUtils.GetAddressListsContainer();
			}
			catch (Exception ex)
			{
				Log.WriteError("Cannot load Exchange 2007 Address Lists Container", ex);
				return EXCHANGE_CONTAINER_ERROR;
			}

			Organization org = PopulateOrganization(packageId, organizationId, addressListsContainer);

			// Check if organization exitsts.                
			if (OrganizationIdentifierExists(organizationId))
				return BusinessErrorCodes.ERROR_ORG_ID_EXISTS;


			int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedOrganizations);

			//create temprory domain name;
			
			string domainName = CreateTemporyDomainName(serviceId, organizationId);

			Log.WriteInfo(string.Format("Importing temp domain {0}", domainName));
			if (string.IsNullOrEmpty(domainName))
				return BusinessErrorCodes.ERROR_ORGANIZATION_TEMP_DOMAIN_IS_NOT_SPECIFIED;

			bool domainCreated;
			int domainId = CreateDomain(domainName, packageId, out domainCreated);
			//create domain 
			if (domainId < 0)
				return domainId;


			//add organization to package items                
			itemId = AddOrganizationToPackageItems(org, serviceId, packageId, organizationName, organizationId, domainName);

			// register org ID

			DataProvider.AddExchangeOrganization(itemId, organizationId);

			// register domain                
			DataProvider.AddExchangeOrganizationDomain(itemId, domainId, true);

			// register organization domain service item
			OrganizationDomain orgDomain = new OrganizationDomain();
			orgDomain.Name = domainName;
			orgDomain.PackageId = packageId;
			orgDomain.ServiceId = serviceId;

			PackageController.AddPackageItem(orgDomain);
			Log.WriteEnd("Organization imported");
			return itemId;
		}

		private static Organization PopulateOrganization(int packageId, string organizationId, string addressListsContainer)
		{
			Organization org = new Organization();
			org.OrganizationId = organizationId;
			org.AddressList = string.Format("CN={0} Address List,CN=All Address Lists,{1}", organizationId, addressListsContainer);
			org.GlobalAddressList = string.Format("CN={0} Global Address List,CN=All Global Address Lists,{1}", organizationId, addressListsContainer);
			org.OfflineAddressBook = string.Format("CN={0} Offline Address Book,CN=Offline Address Lists,{1}", organizationId, addressListsContainer);
			org.Database = GetDatabaseName();
			org.DistinguishedName = ADUtils.RemoveADPrefix(Global.OrgDirectoryEntry.Path);
			org.SecurityGroup = string.Format("CN={0},{1}", organizationId, org.DistinguishedName);


			PackageContext cntx = PackageController.GetPackageContext(packageId);
			// organization limits
            /*
			org.IssueWarningKB = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue;
			if (org.IssueWarningKB > 0) org.IssueWarningKB *= 1024;
			org.ProhibitSendKB = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue;
			if (org.ProhibitSendKB > 0) org.ProhibitSendKB *= 1024;
			org.ProhibitSendReceiveKB = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue;
			if (org.ProhibitSendReceiveKB > 0) org.ProhibitSendReceiveKB *= 1024;
             */

			PackageSettings settings = PackageController.GetPackageSettings(packageId, PackageSettings.EXCHANGE_SERVER);
			org.KeepDeletedItemsDays = Utils.ParseInt(settings["KeepDeletedItemsDays"], 14);
			return org;
		}

		private static string GetDatabaseName()
		{
			string ret;
			if (String.IsNullOrEmpty(Global.MailboxCluster))
			{
				ret = string.Format("{0}\\{1}", Global.StorageGroup, Global.MailboxDatabase);
			}
			else
			{
				ret = string.Format("{0}\\{1}\\{2}", Global.MailboxCluster, Global.StorageGroup, Global.MailboxDatabase);
			}
			return ret;
		}

		private static bool CheckQuotas(int packageId, out int errorCode)
		{

			// check account
			errorCode = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (errorCode < 0) return false;

			// check package
			errorCode = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
			if (errorCode < 0) return false;

			// check organizations quota
			QuotaValueInfo quota = PackageController.GetPackageQuota(packageId, Quotas.ORGANIZATIONS);
			if (quota.QuotaExhausted)
			{
				errorCode = BusinessErrorCodes.ERROR_ORGS_RESOURCE_QUOTA_LIMIT;
				return false;
			}


			// check sub-domains quota (for temporary domain)
			quota = PackageController.GetPackageQuota(packageId, Quotas.OS_SUBDOMAINS);
			if (quota.QuotaExhausted)
			{
				errorCode = BusinessErrorCodes.ERROR_SUBDOMAIN_QUOTA_LIMIT;
				return false;
			}
			return true;
		}

		private static bool OrganizationIdentifierExists(string organizationId)
		{
			return DataProvider.ExchangeOrganizationExists(organizationId);
		}

		private static string CreateTemporyDomainName(int serviceId, string organizationId)
		{
			// load service settings
			StringDictionary serviceSettings = ServerController.GetServiceSettingsAdmin(serviceId);

			string tempDomain = serviceSettings["TempDomain"];
			return String.IsNullOrEmpty(tempDomain) ? null : organizationId + "." + tempDomain;
		}

		private static int CreateDomain(string domainName, int packageId, out bool domainCreated)
		{
			// trying to locate (register) temp domain
			DomainInfo domain = null;
			int domainId = 0;
			domainCreated = false;

			// check if the domain already exists
			int checkResult = ServerController.CheckDomain(domainName);
			if (checkResult == BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS)
			{
				// domain exists
				// check if it belongs to the same space
				domain = ServerController.GetDomain(domainName);
				if (domain == null)
					return checkResult;

				if (domain.PackageId != packageId)
					return checkResult;

				if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
					return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;

				domainId = domain.DomainId;
			}
			else if (checkResult < 0)
			{
				return checkResult;
			}

			// create domain if required
			if (domain == null)
			{
				domain = CreateNewDomain(packageId, domainName);
				// add domain
				domainId = ServerController.AddDomain(domain);

				if (domainId < 0)
					return domainId;

				domainCreated = true;
			}

			return domainId;
		}

		private static DomainInfo CreateNewDomain(int packageId, string domainName)
		{
			// new domain
			DomainInfo domain = new DomainInfo();
			domain.PackageId = packageId;
			domain.DomainName = domainName;
			domain.IsInstantAlias = true;
			domain.IsSubDomain = true;

			return domain;
		}

		private static int AddOrganizationToPackageItems(Organization org, int serviceId, int packageId, string organizationName, string organizationId, string domainName)
		{
			org.ServiceId = serviceId;
			org.PackageId = packageId;
			org.Name = organizationName;
			org.OrganizationId = organizationId;
			org.DefaultDomain = domainName;

			int itemId = PackageController.AddPackageItem(org);
			return itemId;
		}

		private void ImportDomain(string domainName)
		{
			int ret = -1;
			try
			{
				ret = AddOrganizationDomain(Global.ItemId, domainName);
				if (ret < 0)
				{
					Log.WriteError(string.Format("Unable to import domain {0}. Error code: {1}", domainName, ret));
					Global.HasErrors = true;
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError(string.Format("Unable to import domain {0}. Error code: {1}", domainName, ret), ex);
				Global.HasErrors = true;
			}
		}

		private static int AddOrganizationDomain(int itemId, string domainName)
		{
			Log.WriteStart(string.Format("Importing domain {0}...", domainName)); 

			// load organization
			Organization org = (Organization)PackageController.GetPackageItem(itemId);
			if (org == null)
				return -1;

			// check package
			int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			DomainInfo domain = null;

			// check if the domain already exists
			int checkResult = ServerController.CheckDomain(domainName);
			if (checkResult == BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS)
			{
				// domain exists
				// check if it belongs to the same space
				domain = ServerController.GetDomain(domainName);
				if (domain == null)
					return checkResult;

				if (domain.PackageId != org.PackageId)
					return checkResult;

				if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
					return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;
			}
			else if (checkResult == BusinessErrorCodes.ERROR_RESTRICTED_DOMAIN)
			{
				return checkResult;
			}

			// create domain if required
			if (domain == null)
			{
				domain = new DomainInfo();
				domain.PackageId = org.PackageId;
				domain.DomainName = domainName;
				domain.IsInstantAlias = false;
				domain.IsSubDomain = false;

				// add domain
				domain.DomainId = ServerController.AddDomain(domain);
			}



			// register domain
			DataProvider.AddExchangeOrganizationDomain(itemId, domain.DomainId, false);

			// register service item
			OrganizationDomain exchDomain = new OrganizationDomain();
			exchDomain.Name = domainName;
			exchDomain.PackageId = org.PackageId;
			exchDomain.ServiceId = org.ServiceId;
			PackageController.AddPackageItem(exchDomain);
			Log.WriteEnd("Domain imported");
			return 0;

		}

		private static void ImportMailbox(DirectoryEntry directoryEntry)
		{
			int ret = -1;
			string name = null;
			try
			{
				name = (string)directoryEntry.Properties["name"].Value;
				ret = AddMailbox(Global.ItemId, directoryEntry);
				if (ret < 0)
				{
					Log.WriteError(string.Format("Unable to import mailbox {0}. Error code: {1}", name, ret));
					Global.HasErrors = true;
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError(string.Format("Unable to import mailbox {0}. Error code: {1}", name, ret), ex);
				Global.HasErrors = true;
			}
		}

		private static void ImportContact(DirectoryEntry directoryEntry)
		{
			int ret = -1;
			string name = null;
			try
			{
				name = (string)directoryEntry.Properties["name"].Value;
				ret = AddContact(Global.ItemId, directoryEntry);
				if (ret < 0)
				{
					Log.WriteError(string.Format("Unable to import contact {0}. Error code: {1}", name, ret));
					Global.HasErrors = true;
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError(string.Format("Unable to import contact {0}. Error code: {1}", name, ret), ex);
				Global.HasErrors = true;
			}
		}

		private static void ImportGroup(DirectoryEntry directoryEntry)
		{
			int ret = -1;
			string name = null;
			try
			{
				name = (string)directoryEntry.Properties["name"].Value;
				ret = AddGroup(Global.ItemId, directoryEntry);
				if (ret < 0)
				{
					Log.WriteError(string.Format("Unable to import group {0}. Error code: {1}", name, ret));
					Global.HasErrors = true;
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError(string.Format("Unable to import group {0}. Error code: {1}", name, ret), ex);
				Global.HasErrors = true;
			}
		}

		private static int AddMailbox(int itemId, DirectoryEntry entry)
		{
			string accountName = (string)entry.Properties["name"].Value;
			Log.WriteStart(string.Format("Importing user {0}...", accountName)); 

			Organization org = (Organization)PackageController.GetPackageItem(itemId);
			if (org == null)
				return -1;

			// e-mail
			string email = (string)entry.Properties["userPrincipalName"].Value;
			if (string.IsNullOrEmpty(email))
				throw new Exception("UPN is not specified");

			if (EmailAddressExists(email))
				return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

			if (AccountExists(accountName))
				throw new Exception(string.Format("Account {0} already exists", accountName));

			string displayName = (string)entry.Properties["displayName"].Value;

            string samName = (string)entry.Properties["sAMAccountName"].Value;
            // this should really NEVER happen - an AD account without sAMAccountName?!
            if (string.IsNullOrEmpty(samName))
                throw new Exception("SAMAccountName is not specified");
            // add Netbios-Domainname before samAccountName - format in the database
            samName = Global.NetBiosDomain + "\\" + samName;

			int userId = AddOrganizationUser(itemId, accountName, displayName, email, samName, string.Empty);
			AddAccountEmailAddress(userId, email);
			
			//account type
			PropertyValueCollection type = entry.Properties["msExchRecipientDisplayType"];
			if (type == null || type.Value == null)
			{
				Log.WriteInfo("Account type : user");
				return userId;
			}
			int mailboxType = (int)type.Value;

            int mailboxTypeDetails = 0;
            PropertyValueCollection typeDetails = entry.Properties["msExchRecipientTypeDetails"];
            if (typeDetails!=null)
            {
                if (typeDetails.Value != null)
                {
                    try
                    {
                        object adsLargeInteger = typeDetails.Value;
                        mailboxTypeDetails = (Int32)adsLargeInteger.GetType().InvokeMember("LowPart", System.Reflection.BindingFlags.GetProperty, null, adsLargeInteger, null);
                    }
                    catch { } // just skip

                }
            }

			ExchangeAccountType accountType = ExchangeAccountType.Undefined;

            if (mailboxTypeDetails == 4)
            {
                Log.WriteInfo("Account type : shared mailbox");
                accountType = ExchangeAccountType.SharedMailbox;
            }
            else
            {
                switch (mailboxType)
                {
                    case 1073741824:
                        Log.WriteInfo("Account type : mailbox");
                        accountType = ExchangeAccountType.Mailbox;
                        break;
                    case 7:
                        Log.WriteInfo("Account type : room");
                        accountType = ExchangeAccountType.Room;
                        break;
                    case 8:
                        Log.WriteInfo("Account type : equipment");
                        accountType = ExchangeAccountType.Equipment;
                        break;
                    default:
                        Log.WriteInfo("Account type : unknown");
                        return userId;
                }
            }

			UpdateExchangeAccount(userId, accountName, accountType, displayName, email, false, string.Empty, samName, string.Empty, Global.defaultMailboxPlanId);

			string defaultEmail = (string)entry.Properties["extensionAttribute3"].Value;

			PropertyValueCollection emails = entry.Properties["proxyAddresses"];
			if (emails != null)
			{
				foreach (string mail in emails)
				{
					string emailAddress = mail;
					if (emailAddress.ToLower().StartsWith("smtp:"))
						emailAddress = emailAddress.Substring(5);

					if (EmailAddressExists(emailAddress))
					{
                        if ((!emailAddress.Equals(defaultEmail, StringComparison.InvariantCultureIgnoreCase)) && (!emailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase)))
                            Log.WriteInfo(string.Format("Email address {0} already exists. Skipped", emailAddress));

                        continue;
					}
					// register email address
					Log.WriteInfo(string.Format("Importing email {0}", emailAddress));
					AddAccountEmailAddress(userId, emailAddress);
				}
			}
			Log.WriteEnd("User imported");
			return userId;


		}

		private static int AddContact(int itemId, DirectoryEntry entry)
		{
			string accountName = (string)entry.Properties["name"].Value;
			Log.WriteStart(string.Format("Importing contact {0}...", accountName));

			Organization org = (Organization)PackageController.GetPackageItem(itemId);
			if (org == null)
				return -1;


			if (AccountExists(accountName))
				throw new Exception(string.Format("Account {0} already exists", accountName));

			string displayName = (string)entry.Properties["displayName"].Value;
			string email = (string)entry.Properties["targetAddress"].Value;
			if (email != null && email.ToLower().StartsWith("smtp:"))
				email = email.Substring(5);

            // no sAMAccountName for contacts - so String.Empty is OK
			int accountId = AddAccount(itemId, ExchangeAccountType.Contact, accountName, displayName, email, false, 0, string.Empty, null);
			
			Log.WriteEnd("Contact imported");
			return accountId;
		}

		private static int AddGroup(int itemId, DirectoryEntry entry)
		{
			string accountName = (string)entry.Properties["name"].Value;
			Log.WriteStart(string.Format("Importing group {0}...", accountName));

			Organization org = (Organization)PackageController.GetPackageItem(itemId);
			if (org == null)
				return -1;

			if (AccountExists(accountName))
				throw new Exception(string.Format("Account {0} already exists", accountName));

			string displayName = (string)entry.Properties["displayName"].Value;
			string email = null;
			PropertyValueCollection proxyAddresses = entry.Properties["proxyAddresses"];
			if (proxyAddresses != null)
			{
				foreach (string address in proxyAddresses)
				{
					if (address != null && address.StartsWith("SMTP:"))
					{
						email = address.Substring(5);
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(email))
				throw new Exception("Email is not specified");
			
			if (EmailAddressExists(email))
				return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

            string samName = (string)entry.Properties["sAMAccountName"].Value;
            // this should really NEVER happen - an AD group without sAMAccountName?!
            if (string.IsNullOrEmpty(samName))
                throw new Exception("SAMAccountName is not specified");
            // add Netbios-Domainname before samAccountName - format in the database
            samName = Global.NetBiosDomain + "\\" + samName;

			int accountId = AddAccount(itemId, ExchangeAccountType.DistributionList, accountName, displayName, email, false, 0, samName, null);
			AddAccountEmailAddress(accountId, email);

			string defaultEmail = (string)entry.Properties["extensionAttribute3"].Value;

			PropertyValueCollection emails = entry.Properties["proxyAddresses"];
			if (emails != null)
			{
				foreach (string mail in emails)
				{
					string emailAddress = mail;
					if (emailAddress.ToLower().StartsWith("smtp:"))
						emailAddress = emailAddress.Substring(5);


					if (!emailAddress.Equals(defaultEmail, StringComparison.InvariantCultureIgnoreCase))
					{
						if (EmailAddressExists(emailAddress))
						{
							Log.WriteInfo(string.Format("Email address {0} already exists. Skipped", emailAddress));
							continue;
						}
						// register email address
						Log.WriteInfo(string.Format("Importing email {0}", emailAddress));
						AddAccountEmailAddress(accountId, emailAddress);
					}
				}
			}

			Log.WriteEnd("Group imported");
			return accountId;
		}

		private static bool EmailAddressExists(string emailAddress)
		{
			return DataProvider.ExchangeAccountEmailAddressExists(emailAddress);
		}

		private static bool AccountExists(string accountName)
		{
			return DataProvider.ExchangeAccountExists(accountName);
		}

		private static int AddAccount(int itemId, ExchangeAccountType accountType,
			string accountName, string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
			MailboxManagerActions mailboxManagerActions, string samAccountName, string accountPassword)
		{
			return DataProvider.AddExchangeAccount(itemId, (int)accountType,
				accountName, displayName, primaryEmailAddress, mailEnabledPublicFolder,
				mailboxManagerActions.ToString(), samAccountName,0, string.Empty);
		}

		private static int AddOrganizationUser(int itemId, string accountName, string displayName, string email, string samAccountName, string accountPassword)
		{
			return DataProvider.AddExchangeAccount(itemId, (int)ExchangeAccountType.User, accountName, displayName, email, false, string.Empty,
                                            samAccountName, 0 , string.Empty);

		}

		private static void AddAccountEmailAddress(int accountId, string emailAddress)
		{
			DataProvider.AddExchangeAccountEmailAddress(accountId, emailAddress);
		}
		
		private static void UpdateExchangeAccount(int  accountId, string accountName, ExchangeAccountType accountType,
            string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            string mailboxManagerActions, string samAccountName, string accountPassword, int mailboxPlanId)
		{
            DataProvider.UpdateExchangeAccount(accountId, 
                accountName, 
                accountType, 
                displayName, 
                primaryEmailAddress, 
                mailEnabledPublicFolder, 
                mailboxManagerActions,
                samAccountName,
                mailboxPlanId , -1, string.Empty, false);
        }
	}
}
