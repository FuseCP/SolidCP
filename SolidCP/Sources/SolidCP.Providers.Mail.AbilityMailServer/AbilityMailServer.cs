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

using SolidCP.Server.Utils;
using Microsoft.Win32;

namespace SolidCP.Providers.Mail
{
	public class AbilityMailServer : HostingServiceProviderBase, IMailServer
	{
		#region IMailServer Members

		public bool AccountExists(string mailboxName)
		{
			Tree users = AMSHelper.GetUsersConfig();
			AmsMailbox accnt = new AmsMailbox(mailboxName);

			return accnt.Load(users);
		}

		public void AddDomainAlias(string domainName, string aliasName)
		{
			Tree domains = AMSHelper.GetDomainsConfig();
			AmsDomain alias = new AmsDomain(aliasName);

			if (!alias.Load(domains))
			{
				alias.DomainConfig["enabled"] = "1";
				alias.DomainConfig["domain"] = aliasName;
				alias.DomainConfig["mode"] = "1"; // alias mode
				alias.DomainConfig["useconvertdomain"] = "1";
				alias.DomainConfig["convertdomain"] = domainName;

				if (!alias.Save(domains))
				{
					Log.WriteInfo("Couldn't save domains configuration.");
					throw new Exception("Couldn't add domain alias.");
				}
			}
			else
			{
				Log.WriteInfo("Alias already exists.");
				throw new Exception("Alias already exists.");
			}
		}

		public void CreateAccount(MailAccount mailbox)
		{
			Tree users = AMSHelper.GetUsersConfig();
			AmsMailbox accnt = new AmsMailbox(mailbox.Name);

			if (accnt.Load(users))
				throw new Exception("Mailbox is already registered.");

			accnt.Read(mailbox);

			if (!accnt.Save(users))
				throw new Exception("Couldn't create a mailbox.");
		}

		public void CreateDomain(MailDomain domain)
		{
			Tree domains = AMSHelper.GetDomainsConfig();
			AmsDomain amsDomain = new AmsDomain(domain.Name);

			if (amsDomain.Load(domains))
				throw new Exception("Domain is already registered.");

			amsDomain.Read(domain);

			if (!amsDomain.Save(domains))
				throw new Exception("Couldn't create a domain.");
		}

		public void CreateGroup(MailGroup group)
		{
			Tree users = AMSHelper.GetUsersConfig();
			AmsMailbox amsGroup = new AmsMailbox(group.Name);

			if (amsGroup.Load(users))
				throw new Exception("Mail group is already exists.");

			amsGroup.Read(group);

			if (!amsGroup.Save(users))
				throw new Exception("Couldn't create a mail group.");
		}

		public void CreateList(MailList maillist)
		{
			Tree config = AMSHelper.GetMailListsConfig();
			AmsMailList amsList = new AmsMailList(maillist.Name);

			if (amsList.Load(config))
				throw new Exception("Mail list is already exists.");

			amsList.Read(maillist);

			if (!amsList.Save(config))
				throw new Exception("Couldn't create a mail list.");
		}

		public void DeleteAccount(string mailboxName)
		{
			Tree config = AMSHelper.GetUsersConfig();
			AmsMailbox amsMailbox = new AmsMailbox(mailboxName);

			if (amsMailbox.Load(config))
			{
				if (!amsMailbox.Delete(config))
					throw new Exception("Couldn't delete a specified account.");
			}
			else
				throw new Exception("Couldn't load account settings.");
		}

	    public bool MailAliasExists(string mailAliasName)
	    {
	        throw new System.NotImplementedException();
	    }

	    public MailAlias[] GetMailAliases(string domainName)
	    {
	        throw new System.NotImplementedException();
	    }

	    public MailAlias GetMailAlias(string mailAliasName)
	    {
	        throw new System.NotImplementedException();
	    }

	    public void CreateMailAlias(MailAlias mailAlias)
	    {
	        throw new System.NotImplementedException();
	    }

	    public void UpdateMailAlias(MailAlias mailAlias)
	    {
	        throw new System.NotImplementedException();
	    }

	    public void DeleteMailAlias(string mailAliasName)
	    {
	        throw new System.NotImplementedException();
	    }

	    public void DeleteDomain(string domainName)
		{
			Tree config = AMSHelper.GetDomainsConfig();
			AmsDomain amsDomain = new AmsDomain(domainName);

			if (amsDomain.Load(config))
			{
				if (!amsDomain.Delete(config))
					throw new Exception("Couldn't delete specified domain.");
			}
			else
				throw new Exception("Couldn't find specified domain.");
		}

		public void DeleteDomainAlias(string domainName, string aliasName)
		{
			Tree config = AMSHelper.GetDomainsConfig();
			AmsDomain amsAlias = new AmsDomain(aliasName);

			if (amsAlias.Load(config))
			{
				string amsDomain = amsAlias.DomainConfig["convertdomain"];
				if (string.Compare(amsDomain, domainName, true) == 0)
				{
					if (!amsAlias.DeleteAlias(config))
						throw new Exception("Couldn't delete alias.");
				}
			}
			else
			{
				throw new Exception("Couldn't find specified alias.");
			}
		}

		public void DeleteGroup(string groupName)
		{
			Tree config = AMSHelper.GetUsersConfig();
			AmsMailbox amsGroup = new AmsMailbox(groupName);

			if (amsGroup.Load(config))
			{
				if (!amsGroup.Delete(config))
					throw new Exception("Couldn't delete specified mail group.");
			}
			else
			{
				throw new Exception("Couldn't find specified mail group.");
			}
		}

		public void DeleteList(string maillistName)
		{
			Tree config = AMSHelper.GetMailListsConfig();
			AmsMailList amsList = new AmsMailList(maillistName);

			if (amsList.Load(config))
			{
				if (!amsList.Delete(config))
					throw new Exception("Couldn't delete a mail list.");
			}
			else
			{
				throw new Exception("Couldn't find specified mail list.");
			}
		}

		public bool DomainAliasExists(string domainName, string aliasName)
		{
			Tree config = AMSHelper.GetDomainsConfig();
			AmsDomain amsAlias = new AmsDomain(aliasName);

			if (amsAlias.Load(config))
				if (string.Compare(amsAlias.DomainConfig["convertdomain"], domainName, true) == 0)
					return true;

			return false;
		}

		public bool DomainExists(string domainName)
		{
			Tree config = AMSHelper.GetDomainsConfig();
			AmsDomain amsDomain = new AmsDomain(domainName);

			return amsDomain.Load(config);
		}

		public MailAccount GetAccount(string mailboxName)
		{
			Tree config = AMSHelper.GetUsersConfig();
			AmsMailbox amsMailbox = new AmsMailbox(mailboxName);
			
			if (amsMailbox.Load(config))
			{
				amsMailbox.LoadAccountConfig();
				return amsMailbox.ToMailAccount();
			}

			return null;
		}

		public MailAccount[] GetAccounts(string domainName)
		{
			Tree config = AMSHelper.GetUsersConfig();
			List<MailAccount> accounts = new List<MailAccount>();

			AmsMailbox[] mbList = AmsMailbox.GetMailboxes(config, domainName);
			foreach (AmsMailbox mb in mbList)
				accounts.Add(mb.ToMailAccount());

			return accounts.ToArray();
		}

		public MailDomain GetDomain(string domainName)
		{
			Tree config = AMSHelper.GetDomainsConfig();
			AmsDomain amsDomain = new AmsDomain(domainName);

			if (amsDomain.Load(config))
				return amsDomain.ToMailDomain();

			return null;
		}

		public virtual string[] GetDomains()
		{
			Tree config = AMSHelper.GetDomainsConfig();

			return AmsDomain.GetDomains(config);
		}

		public string[] GetDomainAliases(string domainName)
		{
			Tree config = AMSHelper.GetDomainsConfig();

			return AmsDomain.GetDomainAliases(config, domainName);
		}

		public MailGroup GetGroup(string groupName)
		{
			Tree config = AMSHelper.GetUsersConfig();
			AmsMailbox amsGroup = new AmsMailbox(groupName);

			if (amsGroup.Load(config))
			{
				amsGroup.LoadAccountConfig();
				return amsGroup.ToMailGroup();
			}

			return null;
		}

		public MailGroup[] GetGroups(string domainName)
		{
			List<MailGroup> groups = new List<MailGroup>();
			Tree config = AMSHelper.GetUsersConfig();

			AmsMailbox[] amsGroups = AmsMailbox.GetMailGroups(config, domainName);

			foreach (AmsMailbox amsGroup in amsGroups)
				groups.Add(amsGroup.ToMailGroup());

			return groups.ToArray();
		}

		public MailList GetList(string maillistName)
		{
			Tree config = AMSHelper.GetMailListsConfig();
			AmsMailList amsList = new AmsMailList(maillistName);

			if (amsList.Load(config))
			{
				amsList.LoadListConfig();
				return amsList.ToMailList();
			}

			return null;
		}

		public MailList[] GetLists(string domainName)
		{
			List<MailList> lists = new List<MailList>();
			Tree config = AMSHelper.GetMailListsConfig();

			AmsMailList[] amsLists = AmsMailList.GetMailLists(config, domainName);

			foreach (AmsMailList amsList in amsLists)
				lists.Add(amsList.ToMailList());

			return lists.ToArray();
		}

		public bool GroupExists(string groupName)
		{
			Tree config = AMSHelper.GetUsersConfig();
			AmsMailbox amsGroup = new AmsMailbox(groupName);

			if (amsGroup.Load(config))
			{
				amsGroup.LoadAccountConfig();

				return amsGroup.IsMailGroup();
			}

			return false;
		}

		public bool ListExists(string maillistName)
		{
			Tree config = AMSHelper.GetMailListsConfig();
			AmsMailList amsList = new AmsMailList(maillistName);

			return amsList.Load(config);
		}

		public void UpdateAccount(MailAccount mailbox)
		{
			Tree config = AMSHelper.GetUsersConfig();
			AmsMailbox amsMailbox = new AmsMailbox(mailbox.Name);

			if (amsMailbox.Load(config))
			{
				amsMailbox.LoadAccountConfig();
				amsMailbox.Read(mailbox);

				if (!amsMailbox.Save(config))
					throw new Exception("Couldn't update specified mailbox.");
			}
			else
			{
				throw new Exception("Couldn't find specified mailbox.");
			}
		}

		public void UpdateDomain(MailDomain domain)
		{
			Tree config = AMSHelper.GetDomainsConfig();
			AmsDomain amsDomain = new AmsDomain(domain.Name);

			if (amsDomain.Load(config))
			{
				amsDomain.Read(domain);

				if (!amsDomain.Save(config))
					throw new Exception("Couldn't update specified domain.");
			}
			else
			{
				throw new Exception("Couldn't find specified domain.");
			}
		}

		public void UpdateGroup(MailGroup group)
		{
			Tree config = AMSHelper.GetUsersConfig();
			AmsMailbox amsGroup = new AmsMailbox(group.Name);

			if (amsGroup.Load(config))
			{
				amsGroup.LoadAccountConfig();
				amsGroup.Read(group);

				if (!amsGroup.Save(config))
					throw new Exception("Couldn't update specified mail group.");
			}
			else
			{
				throw new Exception("Couldn't find specified mail group.");
			}
		}

		public void UpdateList(MailList maillist)
		{
			Tree config = AMSHelper.GetMailListsConfig();
			AmsMailList amsList = new AmsMailList(maillist.Name);

			if (amsList.Load(config))
			{
				amsList.LoadListConfig();
				amsList.Read(maillist);

				if (!amsList.Save(config))
					throw new Exception("Couldn't update specified mail list.");
			}
			else
			{
				throw new Exception("Couldn't find specified mail list.");
			}
		}

		#endregion

		#region HostingServiceProviderBase members

		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is MailDomain)
				{
					try
					{
						// delete mail domain
						DeleteDomain(item.Name);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error deleting '{0}' mail domain", item.Name), ex);
					}
				}
			}
		}

		public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is MailDomain)
				{
					try
					{
						MailDomain domain = GetDomain(item.Name);
						domain.Enabled = enabled;
						// update mail domain
						UpdateDomain(domain);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error switching '{0}' mail domain", item.Name), ex);
					}
				}
			}
		}

		#endregion


        public override bool  IsInstalled()
        {
            string name = null;
            string versionNumber = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Ability Mail Server 2_is1");

            if (key != null)
            {
                name = (string) key.GetValue("DisplayName");
                string[] parts = name.Split(new char[] {' '});
                versionNumber = parts[3];
            }
            else
            {
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Ability Mail Server 2_is1");
                if (key != null)
                {
                    name = (string)key.GetValue("DisplayName");
                    string[] parts = name.Split(new char[] { ' ' });
                    versionNumber = parts[3];
                }
                else
                {
                    return false;
                }
            }

            string[] split = versionNumber.Split(new char[] {'.'});

            return split[0].Equals("2");
        }

	}
}
