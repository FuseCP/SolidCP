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
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Code.MailServers;
using SolidCP.Providers;
using SolidCP.Providers.Mail;

namespace SolidCP.EnterpriseServer
{
	public class MailServerController : IImportController, IBackupController
	{
		public static MailServer GetMailServer(int serviceId)
		{
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, serviceId);
			return mail;
		}

		#region Mail Accounts
		public static DataSet GetRawMailAccountsPaged(int packageId,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return PackageController.GetRawPackageItemsPaged(packageId, typeof(MailAccount),
				true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		public static List<MailAccount> GetMailAccounts(int packageId, bool recursive)
		{
			List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
				packageId, typeof(MailAccount), recursive);

			return items.ConvertAll(
				new Converter<ServiceProviderItem, MailAccount>(ConvertItemToMailAccount));
		}

		private static MailAccount ConvertItemToMailAccount(ServiceProviderItem item)
		{
			MailAccount account = (MailAccount)item;
			account.Password = CryptoUtils.Decrypt(account.Password);

			return account;
		}

		public static MailAccount GetMailAccount(int itemId)
		{
			// load meta item
			MailAccount item = (MailAccount)PackageController.GetPackageItem(itemId);

			// load service item
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, item.ServiceId);
			MailAccount account = mail.GetAccount(item.Name);

			// add common properties
			account.Id = item.Id;
			account.PackageId = item.PackageId;
			account.ServiceId = item.ServiceId;

			return account;
		}

		public static int AddMailAccount(MailAccount item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check package
			int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// check quota
			QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.MAIL_ACCOUNTS);
			if (quota.QuotaExhausted)
				return BusinessErrorCodes.ERROR_MAIL_ACCOUNTS_RESOURCE_QUOTA_LIMIT;

			// check if mail resource is available
			int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Mail);
			if (serviceId == 0)
				return BusinessErrorCodes.ERROR_MAIL_RESOURCE_UNAVAILABLE;

			// check package items
			if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(MailAccount)) != null)
				return BusinessErrorCodes.ERROR_MAIL_ACCOUNTS_PACKAGE_ITEM_EXISTS;

			// check mailbox account size limit
			if (item.MaxMailboxSize < -1)
				return BusinessErrorCodes.ERROR_MAIL_ACCOUNT_MAX_MAILBOX_SIZE_LIMIT;

			// place log record
			TaskManager.StartTask("MAIL_ACCOUNT", "ADD", item.Name);
			int itemId = 0;
			try
			{

				// check service items
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, serviceId);
				if (mail.AccountExists(item.Name))
					return BusinessErrorCodes.ERROR_MAIL_ACCOUNTS_SERVICE_ITEM_EXISTS;

				// add domain if not exists
				string domainName = item.Name.Substring(item.Name.IndexOf("@") + 1);
				int domainResult = AddMailDomain(item.PackageId, serviceId, domainName);
				if (domainResult < 0)
					return domainResult;

				// create service item
				item.MaxMailboxSize = GetMaxMailBoxSize(item.PackageId, item);

				// add service item
				mail.CreateAccount(item);

				// save item
				item.Password = CryptoUtils.Encrypt(item.Password);
				item.ServiceId = serviceId;
				itemId = PackageController.AddPackageItem(item);

				TaskManager.ItemId = itemId;

			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				if (ex.InnerException != null &&
					ex.InnerException.Message.Contains("The maximum number of domains allowed has been reached"))
				{
					return BusinessErrorCodes.ERROR_MAIL_LICENSE_DOMAIN_QUOTA;
				}
                if (ex.Message.Contains("Password doesn't meet complexity"))
                {
                    return BusinessErrorCodes.ERROR_MAIL_ACCOUNT_PASSWORD_NOT_COMPLEXITY;
                } 
                if (ex.Message.Contains("The maximum number of users for the server has been reached"))
				{
					return BusinessErrorCodes.ERROR_MAIL_LICENSE_USERS_QUOTA;
				}
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			return itemId;
		}

		public static int UpdateMailAccount(MailAccount item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailAccount origItem = (MailAccount)PackageController.GetPackageItem(item.Id);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_ACCOUNTS_PACKAGE_ITEM_NOT_FOUND;

			// check package
			int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// check mailbox account size limit
			if (item.MaxMailboxSize < -1)
				return BusinessErrorCodes.ERROR_MAIL_ACCOUNT_MAX_MAILBOX_SIZE_LIMIT;

			// place log record
            TaskManager.StartTask("MAIL_ACCOUNT", "UPDATE", origItem.Name, item.Id);

			try
			{
				// restore original props
				if (item.Password == "")
					item.Password = CryptoUtils.Decrypt(origItem.Password);

				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);
				item.Name = origItem.Name;

				item.MaxMailboxSize = GetMaxMailBoxSize(origItem.PackageId, item);

				if (String.IsNullOrEmpty(item.Password))
				{
					// get password from the service
					MailAccount origBox = mail.GetAccount(item.Name);
					item.Password = origBox.Password;
				}

				// update service item
				mail.UpdateAccount(item);

				// update meta item
				item.Password = CryptoUtils.Encrypt(item.Password);
				PackageController.UpdatePackageItem(item);
				return 0;
			}
			catch (Exception ex)
			{
                if (ex.Message.Contains("Password doesn't meet complexity") || 
                    ex.Message.Contains("IceWarp password policy denies use of this account") ||
                    ex.Message.Contains("Invalid characters in password"))
			    {
			        return BusinessErrorCodes.ERROR_MAIL_ACCOUNT_PASSWORD_NOT_COMPLEXITY;
			    }

				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int DeleteMailAccount(int itemId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailAccount origItem = (MailAccount)PackageController.GetPackageItem(itemId);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_ACCOUNTS_PACKAGE_ITEM_NOT_FOUND;

			// place log record
			TaskManager.StartTask("MAIL_ACCOUNT", "DELETE", origItem.Name, itemId);

			try
			{
				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);

				// delete service item
				mail.DeleteAccount(origItem.Name);

				// delete meta item
				PackageController.DeletePackageItem(origItem.Id);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		private static int GetMaxMailBoxSize(int packageId, MailAccount item)
		{
			// load package context
			int maxSize = 0; // unlimited
			PackageContext cntx = PackageController.GetPackageContext(packageId);
			if (cntx != null && cntx.Quotas.ContainsKey(Quotas.MAIL_MAXBOXSIZE))
			{
				maxSize = cntx.Quotas[Quotas.MAIL_MAXBOXSIZE].QuotaAllocatedValue;
				if (maxSize == -1)
					return item.MaxMailboxSize;
			}

			if (maxSize == 0)
			{
				if (cntx != null && cntx.Package != null)
					maxSize = GetMaxMailBoxSize(cntx.Package.ParentPackageId, item);
			}
			else if (item.MaxMailboxSize != 0)
			{
				maxSize = Math.Min(item.MaxMailboxSize, maxSize);
			}
			//
			return maxSize;
		}
		#endregion

		#region Mail Forwardings
		public static DataSet GetRawMailForwardingsPaged(int packageId,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return PackageController.GetRawPackageItemsPaged(packageId, typeof(MailAlias),
				true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		public static List<MailAlias> GetMailForwardings(int packageId, bool recursive)
		{
			List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
				packageId, typeof(MailAlias), recursive);

			return items.ConvertAll(
				new Converter<ServiceProviderItem, MailAlias>(ConvertItemToMailForwarding));
		}

		private static MailAlias ConvertItemToMailForwarding(ServiceProviderItem item)
		{
			return (MailAlias)item;
		}

		public static MailAlias GetMailForwarding(int itemId)
		{
			// load meta item
			MailAlias item = (MailAlias)PackageController.GetPackageItem(itemId);

			// load service item
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, item.ServiceId);
			MailAlias alias = mail.GetMailAlias(item.Name);

			// add common properties
			if (!String.IsNullOrEmpty(alias.ForwardTo))
			{
				item.ForwardTo = alias.ForwardTo;
			}
			else
			{
				item.ForwardTo = alias.ForwardingAddresses[0];
			}


			return item;
		}

		public static int AddMailForwarding(MailAlias item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check package
			int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// check quota
			QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.MAIL_FORWARDINGS);
			if (quota.QuotaExhausted)
				return BusinessErrorCodes.ERROR_MAIL_FORWARDINGS_RESOURCE_QUOTA_LIMIT;

			// check if mail resource is available
			int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Mail);
			if (serviceId == 0)
				return BusinessErrorCodes.ERROR_MAIL_RESOURCE_UNAVAILABLE;

			// check package items
			if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(MailAlias)) != null)
				return BusinessErrorCodes.ERROR_MAIL_FORWARDINGS_PACKAGE_ITEM_EXISTS;

			// place log record
			TaskManager.StartTask("MAIL_FORWARDING", "ADD", item.Name);

			try
			{
				// check service items
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, serviceId);
				if (mail.MailAliasExists(item.Name))
					return BusinessErrorCodes.ERROR_MAIL_FORWARDINGS_SERVICE_ITEM_EXISTS;

				// add domain if not exists
				string domainName = item.Name.Substring(item.Name.IndexOf("@") + 1);
				int domainResult = AddMailDomain(item.PackageId, serviceId, domainName);
				if (domainResult < 0)
					return domainResult;

				// create service item
				MailAlias alias = new MailAlias();
				alias.Name = item.Name;
				alias.ForwardTo = item.ForwardTo;
				//for MailEnable alias creation
				alias.DeleteOnForward = true;
				alias.ForwardingAddresses = new string[1];
				alias.ForwardingAddresses[0] = item.ForwardTo;
				alias.Password = Guid.NewGuid().ToString("N").Substring(0, 12);
				alias.Enabled = true;

				// add service item
				mail.CreateMailAlias(alias);

				// save item
				item.ServiceId = serviceId;
				int itemId = PackageController.AddPackageItem(item);

                TaskManager.ItemId = itemId;

				return itemId;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int UpdateMailForwarding(MailAlias item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailAlias origItem = (MailAlias)PackageController.GetPackageItem(item.Id);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_FORWARDINGS_PACKAGE_ITEM_NOT_FOUND;

			// check package
			int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// place log record
			TaskManager.StartTask("MAIL_FORWARDING", "UPDATE", origItem.Name, item.Id);

			try
			{
				// update forwarding
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);
				MailAlias alias = new MailAlias();
				alias.Name = origItem.Name;
				alias.ForwardTo = item.ForwardTo;

				//For MailEnable alias updating
				alias.DeleteOnForward = true;
				alias.ForwardingAddresses = new string[1];
				alias.ForwardingAddresses[0] = item.ForwardTo;

				//

				// update service item
				mail.UpdateMailAlias(alias);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int DeleteMailForwarding(int itemId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailAlias origItem = (MailAlias)PackageController.GetPackageItem(itemId);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_FORWARDINGS_PACKAGE_ITEM_NOT_FOUND;

			// place log record
			TaskManager.StartTask("MAIL_FORWARDING", "DELETE", origItem.Name, itemId);

			try
			{
				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);

				// delete service item
				mail.DeleteMailAlias(origItem.Name);

				// delete meta item
				PackageController.DeletePackageItem(origItem.Id);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}
		#endregion

		#region Mail Groups
		public static DataSet GetRawMailGroupsPaged(int packageId,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return PackageController.GetRawPackageItemsPaged(packageId, typeof(MailGroup),
				true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		public static List<MailGroup> GetMailGroups(int packageId, bool recursive)
		{
			List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
				packageId, typeof(MailGroup), recursive);

			return items.ConvertAll(
				new Converter<ServiceProviderItem, MailGroup>(ConvertItemToMailGroup));
		}

		private static MailGroup ConvertItemToMailGroup(ServiceProviderItem item)
		{
			return (MailGroup)item;
		}

		public static MailGroup GetMailGroup(int itemId)
		{
			// load meta item
			MailGroup item = (MailGroup)PackageController.GetPackageItem(itemId);

			// load service item
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, item.ServiceId);
			MailGroup group = mail.GetGroup(item.Name);

			// add common properties
			group.Id = item.Id;
			group.PackageId = item.PackageId;
			group.ServiceId = item.ServiceId;

			return group;
		}

		public static int AddMailGroup(MailGroup item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check package
			int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// check quota
			QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.MAIL_GROUPS);
			if (quota.QuotaExhausted)
				return BusinessErrorCodes.ERROR_MAIL_GROUPS_RESOURCE_QUOTA_LIMIT;

			// check recipients number
			if (!CheckRecipientsAllowedNumber(item.PackageId, Quotas.MAIL_MAXGROUPMEMBERS, item.Members))
				return BusinessErrorCodes.ERROR_MAIL_GROUPS_RECIPIENTS_LIMIT;

			// check if mail resource is available
			int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Mail);
			if (serviceId == 0)
				return BusinessErrorCodes.ERROR_MAIL_RESOURCE_UNAVAILABLE;

			// check package items
			if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(MailGroup)) != null)
				return BusinessErrorCodes.ERROR_MAIL_GROUPS_PACKAGE_ITEM_EXISTS;

			// place log record
			TaskManager.StartTask("MAIL_GROUP", "ADD", item.Name);

			try
			{
				// check service items
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, serviceId);
				if (mail.GroupExists(item.Name))
					return BusinessErrorCodes.ERROR_MAIL_GROUPS_SERVICE_ITEM_EXISTS;

				// add domain if not exists
				string domainName = item.Name.Substring(item.Name.IndexOf("@") + 1);
				int domainResult = AddMailDomain(item.PackageId, serviceId, domainName);
				if (domainResult < 0)
					return domainResult;

				// create service item
				item.Enabled = true;
				item.Members = RemoveItemNameFromMembersList(
						  item.Name
						, item.Members.Clone() as string[]
					);

				// add service item
				mail.CreateGroup(item);

				// save item
				item.ServiceId = serviceId;
				int itemId = PackageController.AddPackageItem(item);

				TaskManager.ItemId = itemId;

				return itemId;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int UpdateMailGroup(MailGroup item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailGroup origItem = (MailGroup)PackageController.GetPackageItem(item.Id);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_GROUPS_PACKAGE_ITEM_NOT_FOUND;


			// check package
			int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// check recipients number
			if (!CheckRecipientsAllowedNumber(origItem.PackageId, Quotas.MAIL_MAXGROUPMEMBERS, item.Members))
				return BusinessErrorCodes.ERROR_MAIL_GROUPS_RECIPIENTS_LIMIT;

			// place log record
            TaskManager.StartTask("MAIL_GROUP", "UPDATE", origItem.Name, item.Id);

			try
			{
				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);
				item.Name = origItem.Name;
				item.Enabled = true;
				item.Members = RemoveItemNameFromMembersList(
						  item.Name
						, item.Members.Clone() as string[]
					);

				// update service item
				mail.UpdateGroup(item);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int DeleteMailGroup(int itemId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailGroup origItem = (MailGroup)PackageController.GetPackageItem(itemId);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_GROUPS_PACKAGE_ITEM_NOT_FOUND;

			// place log record
			TaskManager.StartTask("MAIL_GROUP", "DELETE", origItem.Name, itemId);

			try
			{
				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);

				// delete service item
				mail.DeleteGroup(origItem.Name);

				// delete meta item
				PackageController.DeletePackageItem(origItem.Id);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		/// <summary>
		/// Searches the <paramref name="itemName"/> in <paramref name="membersList"/> and removes all occurencies.
		/// </summary>
		/// <param name="itemName">Name of the item to search for.</param>
		/// <param name="membersList">List of item members.</param>
		/// <returns><paramref name="membersList"/> with no occurencies of <paramref name="itemName"/> in it.</returns>
		private static string[] RemoveItemNameFromMembersList(string itemName, string[] membersList)
		{
			if (membersList == null)
			{
				return new string[] { String.Empty };
			}

			if (membersList.Length == 0)
			{
				return membersList;
			}

			if (String.IsNullOrEmpty(itemName))
			{
				return membersList;
			}

			List<string> members = new List<string>();

			members.AddRange(membersList);
			members.RemoveAll(
					delegate(string member)
					{
						return (String.Compare(member, itemName, StringComparison.OrdinalIgnoreCase) == 0);
					}
				);

			return members.ToArray();
		}

		private static bool CheckRecipientsAllowedNumber(int packageId, string quotaName, string[] members)
		{
			// load package context
			PackageContext cntx = PackageController.GetPackageContext(packageId);
			if (cntx == null || !cntx.Quotas.ContainsKey(quotaName))
				return false;

			if (members == null || cntx.Quotas[quotaName].QuotaAllocatedValue == -1)
				return true;

			return (members.Length <= cntx.Quotas[quotaName].QuotaAllocatedValue);
		}
		#endregion

		#region Mail Lists
		public static DataSet GetRawMailListsPaged(int packageId,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return PackageController.GetRawPackageItemsPaged(packageId, typeof(MailList),
				true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		public static List<MailList> GetMailLists(int packageId, bool recursive)
		{
			List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
				packageId, typeof(MailList), recursive);

			return items.ConvertAll(
				new Converter<ServiceProviderItem, MailList>(ConvertItemToMailList));
		}

		private static MailList ConvertItemToMailList(ServiceProviderItem item)
		{
			return (MailList)item;
		}

		public static MailList GetMailList(int itemId)
		{
			// load meta item
			MailList item = (MailList)PackageController.GetPackageItem(itemId);

			// load service item
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, item.ServiceId);
			MailList list = mail.GetList(item.Name);

			// add common properties
			list.Id = item.Id;
			list.PackageId = item.PackageId;
			list.ServiceId = item.ServiceId;

			return list;
		}

		public static int AddMailList(MailList item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check package
			int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// check quota
			QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.MAIL_LISTS);
			if (quota.QuotaExhausted)
				return BusinessErrorCodes.ERROR_MAIL_LISTS_RESOURCE_QUOTA_LIMIT;

			// check recipients number
			if (!CheckRecipientsAllowedNumber(item.PackageId, Quotas.MAIL_MAXLISTMEMBERS, item.Members))
				return BusinessErrorCodes.ERROR_MAIL_LISTS_RECIPIENTS_LIMIT;

			// check if mail resource is available
			int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Mail);
			if (serviceId == 0)
				return BusinessErrorCodes.ERROR_MAIL_RESOURCE_UNAVAILABLE;

			// check package items
			if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(MailList)) != null)
				return BusinessErrorCodes.ERROR_MAIL_LISTS_PACKAGE_ITEM_EXISTS;

			// place log record
			TaskManager.StartTask("MAIL_LIST", "ADD", item.Name);

			try
			{
				// check service items
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, serviceId);
				if (mail.ListExists(item.Name))
					return BusinessErrorCodes.ERROR_MAIL_LISTS_SERVICE_ITEM_EXISTS;

				// add domain if not exists
				string domainName = item.Name.Substring(item.Name.IndexOf("@") + 1);
				int domainResult = AddMailDomain(item.PackageId, serviceId, domainName);
				if (domainResult < 0)
					return domainResult;

				// create service item
				item.Enabled = true;
				item.Members = RemoveItemNameFromMembersList(
						  item.Name
						, item.Members.Clone() as string[]
					);

				// add service item
				mail.CreateList(item);

				// save item
				item.ServiceId = serviceId;
				item.Password = CryptoUtils.Encrypt(item.Password);
				int itemId = PackageController.AddPackageItem(item);

				TaskManager.ItemId = itemId;

				return itemId;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int UpdateMailList(MailList item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailList origItem = (MailList)PackageController.GetPackageItem(item.Id);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_LISTS_PACKAGE_ITEM_NOT_FOUND;

			// check package
			int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// check recipients number
			if (!CheckRecipientsAllowedNumber(origItem.PackageId, Quotas.MAIL_MAXLISTMEMBERS, item.Members))
				return BusinessErrorCodes.ERROR_MAIL_LISTS_RECIPIENTS_LIMIT;

			// place log record
            TaskManager.StartTask("MAIL_LIST", "UPDATE", origItem.Name, item.Id);

			try
			{
				// restore original props
				if (item.Password == "")
					item.Password = CryptoUtils.Decrypt(origItem.Password);

				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);
				item.Name = origItem.Name;
				item.Enabled = true;
				item.Members = RemoveItemNameFromMembersList(
						  item.Name
						, item.Members.Clone() as string[]
					);

				// update service item
				mail.UpdateList(item);

				// update meta item
				item.Password = CryptoUtils.Encrypt(item.Password);
				PackageController.UpdatePackageItem(item);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int DeleteMailList(int itemId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailList origItem = (MailList)PackageController.GetPackageItem(itemId);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_LISTS_PACKAGE_ITEM_NOT_FOUND;

			// place log record
            TaskManager.StartTask("MAIL_LIST", "DELETE", origItem.Name, itemId);

			try
			{
				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);

				// delete service item
				mail.DeleteList(origItem.Name);

				// delete meta item
				PackageController.DeletePackageItem(origItem.Id);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}
		#endregion

		#region Mail Domains
		public static DataSet GetRawMailDomainsPaged(int packageId,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return PackageController.GetRawPackageItemsPaged(packageId, typeof(MailDomain),
				true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		public static List<MailDomain> GetMailDomains(int packageId, bool recursive)
		{
			List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
				packageId, typeof(MailDomain), recursive);

			return items.ConvertAll(
				new Converter<ServiceProviderItem, MailDomain>(ConvertItemToMailDomain));
		}

		private static MailDomain ConvertItemToMailDomain(ServiceProviderItem item)
		{
			return (MailDomain)item;
		}

		public static MailDomain GetMailDomain(int packageId, string mailDomainName)
		{
			ServiceProviderItem mailDomain = PackageController.GetPackageItemByName(packageId, mailDomainName, typeof(MailDomain));
			if (mailDomain != null)
				return GetMailDomain(mailDomain.Id);
			else
				return null;
		}

		public static MailDomain GetMailDomain(int itemId)
		{
			// load meta item
			MailDomain item = (MailDomain)PackageController.GetPackageItem(itemId);

			// load service item
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, item.ServiceId);
			MailDomain domain = mail.GetDomain(item.Name);

			// add common properties
			domain.Id = item.Id;
			domain.PackageId = item.PackageId;
			domain.ServiceId = item.ServiceId;

			return domain;
		}

		public static int AddMailDomain(int packageId, int serviceId, string domainName)
		{
			MailDomain domain = new MailDomain();
			domain.Name = domainName;
			domain.PackageId = packageId;
			domain.ServiceId = serviceId;

			return AddMailDomain(domain);
		}

		public static int AddMailDomain(MailDomain item)
		{
			// check package items
			if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(MailDomain)) != null)
				return 0; // OK, domain already exists

			// place log record
			TaskManager.StartTask("MAIL_DOMAIN", "ADD", item.Name);

			// create domain
			try
			{
				// check service items
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, item.ServiceId);
				if (mail.DomainExists(item.Name))
					return BusinessErrorCodes.ERROR_MAIL_DOMAIN_EXISTS;

				item.Enabled = true;

				// add service item
				mail.CreateDomain(item);

				// save domain item
				int itemId = PackageController.AddPackageItem(item);

				// update related domain with a new pointer
                DomainInfo domain = ServerController.GetDomain(item.Name, true, false);
				if (domain != null)
				{
					domain.MailDomainId = itemId;
					ServerController.UpdateDomain(domain);

                    domain = ServerController.GetDomain(domain.DomainId);
                    ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Mail, domain, "");
				}

				// check if instant alias must be added
				if (!String.IsNullOrEmpty(domain.InstantAliasName))
				{
					// load instant alias
					DomainInfo instantAlias = ServerController.GetDomain(domain.InstantAliasId);
					if (instantAlias != null)
					{
						AddMailDomainPointer(itemId, instantAlias.DomainId);
					}
				}

				TaskManager.ItemId = itemId;
				
                return itemId;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int UpdateMailDomain(MailDomain item)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailDomain origItem = (MailDomain)PackageController.GetPackageItem(item.Id);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

			// check package
			int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// place log record
            TaskManager.StartTask("MAIL_DOMAIN", "UPDATE", origItem.Name, item.Id);

			// get service
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, origItem.ServiceId);
			item.Name = origItem.Name;
			item.Enabled = true;

			try
			{
				// update service item
				mail.UpdateDomain(item);

				// update meta item
				PackageController.UpdatePackageItem(item);
				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int DeleteMailDomain(int itemId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// load original meta item
			MailDomain origItem = (MailDomain)PackageController.GetPackageItem(itemId);
			if (origItem == null)
				return BusinessErrorCodes.ERROR_MAIL_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

			// find if account exists under this mail domain
			List<ServiceProviderItem> accounts = PackageController.GetPackageItemsByName(
				origItem.PackageId, "%@" + origItem.Name);

			if (accounts.Count > 0)
				return BusinessErrorCodes.ERROR_MAIL_DOMAIN_IS_NOT_EMPTY; // mail domain is not empty

			// place log record
            TaskManager.StartTask("MAIL_DOMAIN", "DELETE", origItem.Name, itemId);

			try
			{
				// get service
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, origItem.ServiceId);

				// delete service item
				mail.DeleteDomain(origItem.Name);

				// delete meta item
				PackageController.DeletePackageItem(origItem.Id);

				// update related domain with a new pointer
				DomainInfo domain = ServerController.GetDomain(origItem.Name, true, false);
				if (domain != null)
				{
					domain.MailDomainId = 0;
					ServerController.UpdateDomain(domain);
				}

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static List<DomainInfo> GetMailDomainPointers(int itemId)
		{
			List<DomainInfo> pointers = new List<DomainInfo>();

			// load site item
			MailDomain mailDomain = (MailDomain)PackageController.GetPackageItem(itemId);
			if (mailDomain == null)
				return pointers;

			// get the list of all domains
			List<DomainInfo> myDomains = ServerController.GetMyDomains(mailDomain.PackageId);

			foreach (DomainInfo domain in myDomains)
			{
				if (domain.MailDomainId == itemId &&
					domain.DomainName != mailDomain.Name)
					pointers.Add(domain);
			}

			return pointers;
		}

		public static int AddMailDomainPointer( int itemId, int domainId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// load site item
			MailDomain mailDomain = (MailDomain)PackageController.GetPackageItem(itemId);
			if (mailDomain == null)
				return BusinessErrorCodes.ERROR_MAIL_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

			// load domain item
			DomainInfo domain = ServerController.GetDomain(domainId);
			if (domain == null)
				return BusinessErrorCodes.ERROR_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

			// place log record
            TaskManager.StartTask("MAIL_DOMAIN", "ADD_POINTER", mailDomain.Name, itemId, new BackgroundTaskParameter("Domain ID", domain.DomainId));
			
            TaskManager.WriteParameter("Domain pointer", domain.DomainName);

			try
			{
				// update mail aliases
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, mailDomain.ServiceId);
				mail.AddDomainAlias(mailDomain.Name, domain.DomainName);


                if (domain != null)
                {
                    if (domain.ZoneItemId != 0)
                    {
                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Mail, domain, "");
                    }
                }


				// update domain
				domain.MailDomainId = itemId;
				ServerController.UpdateDomain(domain);

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static int DeleteMailDomainPointer(int itemId, int domainId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// load site item
			MailDomain mailDomain = (MailDomain)PackageController.GetPackageItem(itemId);
			if (mailDomain == null)
				return BusinessErrorCodes.ERROR_MAIL_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

			// load domain item
			DomainInfo domain = ServerController.GetDomain(domainId);
			if (domain == null)
				return BusinessErrorCodes.ERROR_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

			// place log record

            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            parameters.Add(new BackgroundTaskParameter("Domain ID", domain.DomainId));
            parameters.Add(new BackgroundTaskParameter("Domain pointer", domain.DomainName));

			TaskManager.StartTask("MAIL_DOMAIN", "DELETE_POINTER", mailDomain.Name, itemId, parameters);

			try
			{
				// update mail aliases
				MailServer mail = new MailServer();
				ServiceProviderProxy.Init(mail, mailDomain.ServiceId);
				mail.DeleteDomainAlias(mailDomain.Name, domain.DomainName);

				// update domain
				domain.MailDomainId = 0;
				ServerController.UpdateDomain(domain);

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}
		#endregion

		#region IImportController Members

		public List<string> GetImportableItems(int packageId, int itemTypeId, Type itemType, ResourceGroupInfo group)
		{
			List<string> items = new List<string>();

			// get service id
			int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
			if (serviceId == 0)
				return items;

			// Mail provider
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, serviceId);

			if (itemType == typeof(MailDomain))
				items.AddRange(mail.GetDomains());

			return items;
		}

		public void ImportItem(int packageId, int itemTypeId, Type itemType,
			ResourceGroupInfo group, string itemName)
		{
			// get service id
			int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
			if (serviceId == 0)
				return;

			// Mail provider
			MailServer mail = new MailServer();
			ServiceProviderProxy.Init(mail, serviceId);

			if (itemType == typeof(MailDomain))
			{
				QuotaLimit result = IsQuotasWillExceed(packageId, mail, itemName);
				//in case quotas will exceed the space - exit the import function
				if (result.IsExceeded)
				{
					if (result.Message.Equals("Mail_Domain"))
					{
						const string exceededQuota = "number of domains";
						TaskManager.WriteWarning(
							String.Format(
								"Unable to import mail domain '{0}'.\r\nHosting Plan quotas will be exceeded. \r\nVerify the following quotas before importing mail domain: {1}."
								, itemName, exceededQuota
								)
							);
						return;
					}
					if (result.Message.Equals("Mail_Account"))
					{
						const string exceededQuota = "number of mail accounts";
						TaskManager.WriteWarning(
							String.Format(
								"Unable to import mail domain '{0}'.\r\nHosting Plan quotas will be exceeded. \r\nVerify the following quotas before importing mail domain: {1}."
								, itemName, exceededQuota
								)
							);
						return;
					}
					if (result.Message.Equals("Mail_Group"))
					{
						const string exceededQuota = "number of groups";
						TaskManager.WriteWarning(
							String.Format(
								"Unable to import mail domain '{0}'.\r\nHosting Plan quotas will be exceeded. \r\nVerify the following quotas before importing mail domain: {1}."
								, itemName, exceededQuota
								)
							);
						return;
					}
					if (result.Message.Equals("Mail_List"))
					{
						const string exceededQuota = "number of mail lists";
						TaskManager.WriteWarning(
							String.Format(
								"Unable to import mail domain '{0}'.\r\nHosting Plan quotas will be exceeded. \r\nVerify the following quotas before importing mail domain: {1}."
								, itemName, exceededQuota
								)
							);
						return;
					}

				}

				List<string> domains = new List<string>();
				domains.Add(itemName);
				try
				{
					domains.AddRange(mail.GetDomainAliases(itemName));
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Error importing mail domain - skipped");
					return;
				}

				MailDomain mailDomain = new MailDomain();
				mailDomain.Name = itemName;
				mailDomain.ServiceId = serviceId;
				mailDomain.PackageId = packageId;
				int mailDomainId = PackageController.AddPackageItem(mailDomain);

				// restore domains
				RestoreDomainsByMail(domains, packageId, mailDomainId);

				// add mail accounts
				try
				{

					MailAccount[] accounts = mail.GetAccounts(itemName);
					foreach (MailAccount account in accounts)
					{
						account.ServiceId = serviceId;
						account.PackageId = packageId;
						//get mail account password
						account.Password = CryptoUtils.Encrypt(account.Password);
						PackageController.AddPackageItem(account);
					}
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Error importing mail account");
				}

				//add mail aliases (forwardings)
				try
				{
					MailAlias[] aliases = mail.GetMailAliases(itemName);
					foreach (MailAlias alias in aliases)
					{
						alias.ServiceId = serviceId;
						alias.PackageId = packageId;
						PackageController.AddPackageItem(alias);
					}
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Error importing mail aliases");
				}

				// add mail groups
				try
				{
					MailGroup[] groups = mail.GetGroups(itemName);
					foreach (MailGroup mailGroup in groups)
					{
						mailGroup.ServiceId = serviceId;
						mailGroup.PackageId = packageId;
						PackageController.AddPackageItem(mailGroup);
					}
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Error importing mail group");
				}

				// add mail lists
				try
				{
					MailList[] lists = mail.GetLists(itemName);
					foreach (MailList list in lists)
					{
						list.ServiceId = serviceId;
						list.PackageId = packageId;
						PackageController.AddPackageItem(list);
					}
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Error importing mail list");
				}
			}
		}

		private void RestoreDomainsByMail(List<string> domains, int packageId, int mailDomainId)
		{
			// add/update domains/pointers
			foreach (string domainName in domains)
			{
				DomainInfo domain = ServerController.GetDomain(domainName, true, false);
				if (domain == null)
				{
					domain = new DomainInfo();
					domain.DomainName = domainName;
					domain.PackageId = packageId;
					domain.MailDomainId = mailDomainId;
					ServerController.AddDomainItem(domain);
				}
				else
				{
					domain.MailDomainId = mailDomainId;
					ServerController.UpdateDomain(domain);
				}
			}
		}

		/// <summary>
		/// Verify is quota will exceed if we will add a number of new items into hosting space.
		/// </summary>
		/// <param name="packageId">Hosting Space Id</param>
		/// <param name="quotaName">Name of quota to check</param>
		/// <param name="numberOfItemsToAdd">The number of items we are going to add.</param>
		/// <returns>True if quota will exceed. Otherwise, false.</returns>
		protected bool VerifyIfQuotaWillBeExceeded(int packageId, string quotaName, int numberOfItemsToAdd)
		{
            // Don't bother to check quota if the number of items to add is zero or less otherwise IsQuotasWillExceed 
            // will fail when quota is set to 0 on lists or groups and still thera are no items to import
		    if (numberOfItemsToAdd <= 0)
		    {
		        return false;
		    }

			bool result = false;

			QuotaValueInfo quotaInfo = PackageController.GetPackageQuota(packageId, quotaName);

			if (!quotaInfo.QuotaExhausted)
			{
				// -1 means Unlimited quota
				if ((quotaInfo.QuotaAllocatedValue != -1) &&
					(quotaInfo.QuotaUsedValue + numberOfItemsToAdd > quotaInfo.QuotaAllocatedValue))
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Verify if mail provider quotas will exceed if we will try to add Mail Domain into Hosting Space
		/// </summary>
		/// <param name="packageId">Hosting Space Id</param>
		/// <param name="mailServer">Mail Server instance which is used to get Mail Domain information from.</param>
		/// <param name="domainName">Name of the domain.</param>
		/// <returns>True, if a quota will exceed. Otherwise, false.</returns>
		protected QuotaLimit IsQuotasWillExceed(int packageId, MailServer mailServer, string domainName)
		{
			QuotaLimit result = new QuotaLimit();

			bool quotaExceeded = false, skipOsDomains = false;

			// Get OS domains that are already in Hosting Space
			List<DomainInfo> spaceDomains = ServerController.GetDomains(packageId);
			// Step #1 - do not count mail domain as OS domain if OS domain with such name as mail domain already present in Hosting Space
			foreach (DomainInfo domain in spaceDomains)
			{
				if (domain.DomainName.Equals(domainName))
				{
					skipOsDomains = true;
					break;
				}
			}

			// in current scenario of importing we import domains until the quota exceeded
			// in other words let's examine whether quota will be exeeded if we will add +1 domain.
			if (skipOsDomains == false)
			{
				quotaExceeded = VerifyIfQuotaWillBeExceeded(packageId, Quotas.OS_DOMAINS, 1);
				result.IsExceeded = quotaExceeded;
				result.Message = "Mail_Domain";
			}
			// Step #3
			if (quotaExceeded == false)
			{
				quotaExceeded = VerifyIfQuotaWillBeExceeded(packageId, Quotas.MAIL_ACCOUNTS, mailServer.GetAccounts(domainName).Length);
				result.IsExceeded = quotaExceeded;
				result.Message = "Mail_Account";
			}
			// Step #4
			if (quotaExceeded == false)
			{
				quotaExceeded = VerifyIfQuotaWillBeExceeded(packageId, Quotas.MAIL_GROUPS, mailServer.GetGroups(domainName).Length);
				result.IsExceeded = quotaExceeded;
				result.Message = "Mail_Group";
			}
			// Step #5
			if (quotaExceeded == false)
			{
				quotaExceeded = VerifyIfQuotaWillBeExceeded(packageId, Quotas.MAIL_LISTS, mailServer.GetLists(domainName).Length);
				result.IsExceeded = quotaExceeded;
				result.Message = "Mail_List";
			}
			// Return validation result
			return result;
		}

		#endregion

		#region IBackupController Members

		public int BackupItem(string tempFolder, XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
		{
			if (item is MailDomain)
			{
				// backup mail domain
				MailServer mail = GetMailServer(item.ServiceId);

				// read domain info
				MailDomain domain = mail.GetDomain(item.Name);
				XmlSerializer serializer = new XmlSerializer(typeof(MailDomain));
				serializer.Serialize(writer, domain);

				XmlSerializer accountSerializer = new XmlSerializer(typeof(MailAccount));
				XmlSerializer groupSerializer = new XmlSerializer(typeof(MailGroup));
				XmlSerializer listSerializer = new XmlSerializer(typeof(MailList));

				// get domain aliases
				string[] aliases = mail.GetDomainAliases(item.Name);
				foreach (string alias in aliases)
					writer.WriteElementString("Alias", alias);

				// get domain accounts
				List<ServiceProviderItem> accounts = PackageController.GetPackageItemsByName(
					item.PackageId, "%@" + item.Name);

				// backup accounts
				foreach (ServiceProviderItem domainItem in accounts)
				{
					if (domainItem is MailAccount ||
						domainItem is MailAlias)
					{
						MailAccount account = mail.GetAccount(domainItem.Name);
						account.Password = ((MailAccount)domainItem).Password;
						account.DeleteOnForward = domainItem is MailAlias;
						accountSerializer.Serialize(writer, account);
					}
					else if (domainItem is MailGroup)
					{
						MailGroup mailGroup = mail.GetGroup(domainItem.Name);
						groupSerializer.Serialize(writer, mailGroup);
					}
					else if (domainItem is MailList)
					{
						MailList list = mail.GetList(domainItem.Name);
						list.Password = ((MailList)domainItem).Password;
						listSerializer.Serialize(writer, list);
					}
				}
			}
			return 0;
		}

		public int RestoreItem(string tempFolder, XmlNode itemNode, int itemId, Type itemType,
			string itemName, int packageId, int serviceId, ResourceGroupInfo group)
		{
			if (itemType == typeof(MailDomain))
			{
				MailServer mail = GetMailServer(serviceId);

				// extract meta item
				XmlSerializer serializer = new XmlSerializer(typeof(MailDomain));
				MailDomain domain = (MailDomain)serializer.Deserialize(
					new XmlNodeReader(itemNode.SelectSingleNode("MailDomain")));

				// create mail domain if required
				List<string> domains = new List<string>();
				if (!mail.DomainExists(domain.Name))
				{
					mail.CreateDomain(domain);
					domains.Add(domain.Name);

					// add domain aliases
					foreach (XmlNode aliasNode in itemNode.SelectNodes("Alias"))
					{
						mail.AddDomainAlias(domain.Name, aliasNode.InnerText);
						domains.Add(aliasNode.InnerText);
					}
				}

				// add meta-item if required
				int mailDomainId = 0;
				MailDomain existDomain = (MailDomain)PackageController.GetPackageItemByName(packageId, itemName, typeof(MailDomain));
				if (existDomain == null)
				{
					domain.PackageId = packageId;
					domain.ServiceId = serviceId;
					mailDomainId = PackageController.AddPackageItem(domain);
				}
				else
				{
					mailDomainId = existDomain.Id;
				}

				// restore domains
				RestoreDomainsByMail(domains, packageId, mailDomainId);

				XmlSerializer accountSerializer = new XmlSerializer(typeof(MailAccount));
				XmlSerializer groupSerializer = new XmlSerializer(typeof(MailGroup));
				XmlSerializer listSerializer = new XmlSerializer(typeof(MailList));

				// restore accounts
				foreach (XmlNode accountNode in itemNode.SelectNodes("MailAccount"))
				{
					MailAccount account = (MailAccount)accountSerializer.Deserialize(new XmlNodeReader(accountNode));

					if (!mail.AccountExists(account.Name))
					{
						account.Password = CryptoUtils.Decrypt(account.Password);
						mail.CreateAccount(account);

						// restore password
						account.Password = CryptoUtils.Encrypt(account.Password);
					}

					// add meta-item if required
					if (account.DeleteOnForward
						&& PackageController.GetPackageItemByName(packageId, account.Name, typeof(MailAlias)) == null)
					{
						MailAlias forw = new MailAlias();
						forw.PackageId = packageId;
						forw.ServiceId = serviceId;
						forw.Name = account.Name;
						PackageController.AddPackageItem(forw);
					}
					else if (!account.DeleteOnForward
						&& PackageController.GetPackageItemByName(packageId, account.Name, typeof(MailAccount)) == null)
					{
						account.PackageId = packageId;
						account.ServiceId = serviceId;
						PackageController.AddPackageItem(account);
					}
				}

				// restore groups
				foreach (XmlNode groupNode in itemNode.SelectNodes("MailGroup"))
				{
					MailGroup mailGroup = (MailGroup)groupSerializer.Deserialize(new XmlNodeReader(groupNode));

					if (!mail.GroupExists(mailGroup.Name))
					{
						mail.CreateGroup(mailGroup);
					}

					// add meta-item if required
					if (PackageController.GetPackageItemByName(packageId, mailGroup.Name, typeof(MailGroup)) == null)
					{
						mailGroup.PackageId = packageId;
						mailGroup.ServiceId = serviceId;
						PackageController.AddPackageItem(mailGroup);
					}
				}

				// restore lists
				foreach (XmlNode listNode in itemNode.SelectNodes("MailList"))
				{
					MailList list = (MailList)listSerializer.Deserialize(new XmlNodeReader(listNode));

					if (!mail.ListExists(list.Name))
					{
						list.Password = CryptoUtils.Decrypt(list.Password);

						mail.CreateList(list);

						// restore password
						list.Password = CryptoUtils.Encrypt(list.Password);
					}

					// add meta-item if required
					if (PackageController.GetPackageItemByName(packageId, list.Name, typeof(MailList)) == null)
					{
						list.PackageId = packageId;
						list.ServiceId = serviceId;
						PackageController.AddPackageItem(list);
					}
				}
			}
			return 0;
		}

		#endregion
	}
}
