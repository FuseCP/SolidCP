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
using SolidCP.Providers.Mail.SM3;
using Microsoft.Win32;
using SM3 = SolidCP.Providers.Mail.SM3;

namespace SolidCP.Providers.Mail
{
    public class SmarterMail3 : SmarterMail2
    {
		static string[] sm3Settings = new string[] {
            "description",
			"disabled",
			"moderator",
			"password",
			"requirepassword",
			"whocanpost",
			"prependsubject",
			"maxmessagesize",
			"maxrecipients",
			"replytolist"
		};

        public SmarterMail3()
        {
        }

		public override void CreateAccount(MailAccount mailbox)
		{
			try
			{
				svcUserAdmin users = new svcUserAdmin();
				PrepareProxy(users);

				GenericResult1 result = users.AddUser(AdminUsername, AdminPassword,
					mailbox.Name,
					mailbox.Password,
					GetDomainName(mailbox.Name),
					mailbox.FirstName,
					mailbox.LastName,
					false //domain admin is false
					);

				if (!result.Result)
					throw new Exception(result.Message);

				// set forwarding settings
				result = users.UpdateUserForwardingInfo(AdminUsername, AdminPassword,
					mailbox.Name, mailbox.DeleteOnForward,
					(mailbox.ForwardingAddresses != null ? String.Join(", ", mailbox.ForwardingAddresses) : ""));

				if (!result.Result)
					throw new Exception(result.Message);

				// set additional settings
				result = users.SetRequestedUserSettings(AdminUsername, AdminPassword,
					mailbox.Name,
					new string[]
                    {
                        "isenabled=" + mailbox.Enabled.ToString(),
                        "maxsize=" + mailbox.MaxMailboxSize.ToString(),
                        "passwordlocked=" + mailbox.PasswordLocked.ToString(),
                        "replytoaddress=" + (mailbox.ReplyTo != null ? mailbox.ReplyTo : ""),
                        "signature=" + (mailbox.Signature != null ? mailbox.Signature : ""),
						"spamforwardoption=none"
                    });

				if (!result.Result)
					throw new Exception(result.Message);

				// set autoresponder settings
				result = users.UpdateUserAutoResponseInfo(AdminUsername, AdminPassword,
					mailbox.Name,
					mailbox.ResponderEnabled,
					(mailbox.ResponderSubject != null ? mailbox.ResponderSubject : ""),
					(mailbox.ResponderMessage != null ? mailbox.ResponderMessage : ""));

				if (!result.Result)
					throw new Exception(result.Message);

			}
			catch (Exception ex)
			{
				throw new Exception("Could not create mailbox", ex);
			}
		}

		public override void UpdateAccount(MailAccount mailbox)
		{
			try
			{
                //get original account
                MailAccount account = GetAccount(mailbox.Name);  
				               
                svcUserAdmin users = new svcUserAdmin();
				PrepareProxy(users);

                
                
                string strPassword = mailbox.Password;
                
                //Don't change password. Get it from mail server.
                if (!mailbox.ChangePassword)
                {                
                    strPassword = account.Password;                                                            
                }

                GenericResult1 result = users.UpdateUser(AdminUsername, AdminPassword,
                                                             mailbox.Name,
                                                             strPassword,
                                                             mailbox.FirstName,
                                                             mailbox.LastName,
                                                             account.IsDomainAdmin
                        );                    

                if (!result.Result)
					throw new Exception(result.Message);

				// set forwarding settings
				result = users.UpdateUserForwardingInfo(AdminUsername, AdminPassword,
					mailbox.Name, mailbox.DeleteOnForward,
					(mailbox.ForwardingAddresses != null ? String.Join(", ", mailbox.ForwardingAddresses) : ""));

				if (!result.Result)
					throw new Exception(result.Message);

				// set additional settings
				result = users.SetRequestedUserSettings(AdminUsername, AdminPassword,
					mailbox.Name,
					new string[]
                    {
                        "isenabled=" + mailbox.Enabled.ToString(),
                        "maxsize=" + mailbox.MaxMailboxSize.ToString(),
                        "passwordlocked=" + mailbox.PasswordLocked.ToString(),
                        "replytoaddress=" + (mailbox.ReplyTo != null ? mailbox.ReplyTo : ""),
                        "signature=" + (mailbox.Signature != null ? mailbox.Signature : ""),
						"spamforwardoption=none"
                    });

				if (!result.Result)
					throw new Exception(result.Message);

				// set autoresponder settings
				result = users.UpdateUserAutoResponseInfo(AdminUsername, AdminPassword,
					mailbox.Name,
					mailbox.ResponderEnabled,
					(mailbox.ResponderSubject != null ? mailbox.ResponderSubject : ""),
					(mailbox.ResponderMessage != null ? mailbox.ResponderMessage : ""));

				if (!result.Result)
					throw new Exception(result.Message);
			}
			catch (Exception ex)
			{
				throw new Exception("Could not update mailbox", ex);
			}
		}

		public override void AddDomainAlias(string domainName, string aliasName)
		{
			try
			{
				SM3.svcDomainAliasAdmin service = new SM3.svcDomainAliasAdmin();
				PrepareProxy(service);

				SM3.GenericResult result = service.AddDomainAliasWithoutMxCheck(
					AdminUsername,
					AdminPassword,
					domainName,
					aliasName
				);

				if (!result.Result)
					throw new Exception(result.Message);
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't create domain alias.", ex);
			}
		}

		public override void DeleteDomainAlias(string domainName, string aliasName)
		{
			try
			{
				SM3.svcDomainAliasAdmin service = new SM3.svcDomainAliasAdmin();
				PrepareProxy(service);

				SM3.GenericResult result = service.DeleteDomainAlias(
					AdminUsername,
					AdminPassword,
					domainName,
					aliasName
				);

				if (!result.Result)
					throw new Exception(result.Message);
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't delete domain alias.", ex);
			}
		}

		public override bool ListExists(string listName)
		{
			bool exists = false;

			try
			{
				string domain = GetDomainName(listName);
				string account = GetAccountName(listName);

				SM3.svcMailListAdmin lists = new SM3.svcMailListAdmin();
				PrepareProxy(lists);

				SM3.MailingListResult result = lists.GetMailingListsByDomain(AdminUsername, AdminPassword, domain);

				if (result.Result)
				{
					foreach (string member in result.listNames)
					{
						if (string.Compare(member, listName, true) == 0)
						{
							exists = true;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't obtain mail list.", ex);
			}

			return exists;
		}

		public override void CreateList(MailList list)
		{
			try
			{
				string domain = GetDomainName(list.Name);
				string account = GetAccountName(list.Name);

				SM3.svcMailListAdmin lists = new SM3.svcMailListAdmin();
				PrepareProxy(lists);

				SM3.GenericResult result = lists.AddList(AdminUsername, AdminPassword,
					domain,
					account,
					list.ModeratorAddress,
					list.Description
				);

				if (!result.Result)
					throw new Exception(result.Message);

				List<string> settings = new List<string>();
                settings.Add(string.Concat("description=", list.Description));
				settings.Add(string.Concat("disabled=", !list.Enabled));
				settings.Add(string.Concat("moderator=", list.ModeratorAddress));
				settings.Add(string.Concat("password=", list.Password));
				settings.Add(string.Concat("requirepassword=", list.RequirePassword));

				switch (list.PostingMode)
				{
					case PostingMode.AnyoneCanPost:
						settings.Add("whocanpost=anyone");
						break;
					case PostingMode.MembersCanPost:
						settings.Add("whocanpost=subscribersonly");
						break;
					case PostingMode.ModeratorCanPost:
						settings.Add("whocanpost=moderator");
						break;
				}

				settings.Add(string.Concat("prependsubject=", list.EnableSubjectPrefix));
				settings.Add(string.Concat("maxmessagesize=", list.MaxMessageSize));
				settings.Add(string.Concat("maxrecipients=", list.MaxRecipientsPerMessage));

				switch (list.ReplyToMode)
				{
					case ReplyTo.RepliesToList:
						settings.Add("replytolist=true");
						break;
				}

				result = lists.SetRequestedListSettings(AdminUsername, AdminPassword,
					domain,
					account,
					settings.ToArray()
				);

				if (!result.Result)
					throw new Exception(result.Message);

				if (list.Members.Length > 0)
				{
					result = lists.SetSubscriberList(AdminUsername, AdminPassword,
						domain,
						account,
						list.Members
					);

					if (!result.Result)
						throw new Exception(result.Message);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't create mail list.", ex);
			}
		}

		public override MailList GetList(string listName)
		{
			try
			{
				string domain = GetDomainName(listName);
				string account = GetAccountName(listName);

				SM3.svcMailListAdmin svcLists = new SM3.svcMailListAdmin();
				PrepareProxy(svcLists);

				SM3.SettingsRequestResult sResult = svcLists.GetRequestedListSettings(
					AdminUsername,
					AdminPassword,
					domain,
					account,
					sm3Settings
				);

				if (!sResult.Result)
					throw new Exception(sResult.Message);

				SM3.SubscriberListResult mResult = svcLists.GetSubscriberList(
					AdminUsername,
					AdminPassword,
					domain,
					account
				);

				if (!mResult.Result)
					throw new Exception(mResult.Message);

				MailList list = new MailList();
				list.Name = listName;

				SetMailListSettings(list, sResult.settingValues);
				SetMailListMembers(list, mResult.Subscribers);

				return list;
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't obtain mail list.", ex);
			}
		}

		public override MailList[] GetLists(string domainName)
		{
			try
			{
				SM3.svcMailListAdmin svcLists = new SM3.svcMailListAdmin();
				PrepareProxy(svcLists);

				SM3.MailingListResult mResult = svcLists.GetMailingListsByDomain(
					AdminUsername,
					AdminPassword,
					domainName
				);

				if (!mResult.Result)
					throw new Exception(mResult.Message);

				List<MailList> mailLists = new List<MailList>();
				foreach (string listName in mResult.listNames)
				{
					SM3.SettingsRequestResult sResult = svcLists.GetRequestedListSettings(
						AdminUsername,
						AdminPassword,
						domainName,
						listName,
						sm3Settings
					);

					if (!sResult.Result)
						throw new Exception(sResult.Message);

					SM3.SubscriberListResult rResult = svcLists.GetSubscriberList(
						AdminUsername,
						AdminPassword,
						domainName,
						listName
					);

					if (!rResult.Result)
						throw new Exception(rResult.Message);

					MailList list = new MailList();
					list.Name = string.Concat(listName, "@", domainName);
					SetMailListSettings(list, sResult.settingValues);
					SetMailListMembers(list, rResult.Subscribers);
					mailLists.Add(list);
				}

				return mailLists.ToArray();
			}
			catch(Exception ex)
			{
				throw new Exception("Couldn't obtain domain mail lists.", ex);
			}
		}

		public override void DeleteList(string listName)
		{
			try
			{
				SM3.svcMailListAdmin svcLists = new SM3.svcMailListAdmin();
				PrepareProxy(svcLists);

				string account = GetAccountName(listName);
				string domain = GetDomainName(listName);

				SM3.GenericResult gResult = svcLists.DeleteList(
					AdminUsername,
					AdminPassword,
					domain,
					listName
				);

				if (!gResult.Result)
					throw new Exception(gResult.Message);
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't delete a mail list.", ex);
			}
		}

		public override void UpdateList(MailList list)
		{
			try
			{
				string domain = GetDomainName(list.Name);
				string account = GetAccountName(list.Name);

				SM3.svcMailListAdmin lists = new SM3.svcMailListAdmin();
				PrepareProxy(lists);

				List<string> settings = new List<string>();
                settings.Add(string.Concat("description=", list.Description));
				settings.Add(string.Concat("disabled=", !list.Enabled));
				settings.Add(string.Concat("moderator=", list.ModeratorAddress));
				settings.Add(string.Concat("password=", list.Password));
				settings.Add(string.Concat("requirepassword=", list.RequirePassword));

				switch (list.PostingMode)
				{
					case PostingMode.AnyoneCanPost:
						settings.Add("whocanpost=anyone");
						break;
					case PostingMode.MembersCanPost:
						settings.Add("whocanpost=subscribersonly");
						break;
					case PostingMode.ModeratorCanPost:
						settings.Add("whocanpost=moderator");
						break;
				}

				settings.Add(string.Concat("prependsubject=", list.EnableSubjectPrefix));
				settings.Add(string.Concat("maxmessagesize=", list.MaxMessageSize));
				settings.Add(string.Concat("maxrecipients=", list.MaxRecipientsPerMessage));

				switch (list.ReplyToMode)
				{
					case ReplyTo.RepliesToList:
						settings.Add("replytolist=true");
						break;
                    case ReplyTo.RepliesToSender:
                        settings.Add("replytolist=false");
                        break;
				}

				SM3.GenericResult result = lists.SetRequestedListSettings(AdminUsername, AdminPassword,
					domain,
					account,
					settings.ToArray()
				);

				if (!result.Result)
					throw new Exception(result.Message);

				if (list.Members.Length > 0)
				{
					result = lists.SetSubscriberList(AdminUsername, AdminPassword,
						domain,
						account,
						list.Members
					);

					if (!result.Result)
						throw new Exception(result.Message);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Couldn't update mail list.", ex);
			}
		}

		protected void SetMailListSettings(MailList list, string[] smSettings)
		{
			foreach (string setting in smSettings)
			{
				string[] bunch = setting.Split(new char[] { '=' });

				switch (bunch[0])
				{
                    case "description":
				        list.Description = bunch[1];
                        break;
					case "disabled":
						list.Enabled = !Convert.ToBoolean(bunch[1]);
						break;
					case "moderator":
						list.ModeratorAddress = bunch[1];
						list.Moderated = !string.IsNullOrEmpty(bunch[1]);
						break;
					case "password":
						list.Password = bunch[1];
						break;
					case "requirepassword":
						list.RequirePassword = Convert.ToBoolean(bunch[1]);
						break;
					case "whocanpost":
						if (string.Compare(bunch[1], "anyone", true) == 0)
							list.PostingMode = PostingMode.AnyoneCanPost;
						else if (string.Compare(bunch[1], "moderatoronly", true) == 0)
							list.PostingMode = PostingMode.ModeratorCanPost;
						else
							list.PostingMode = PostingMode.MembersCanPost;
						break;
					case "prependsubject":
						list.EnableSubjectPrefix = Convert.ToBoolean(bunch[1]);
						break;
					case "maxmessagesize":
						list.MaxMessageSize = Convert.ToInt32(bunch[1]);
						break;
					case "maxrecipients":
						list.MaxRecipientsPerMessage = Convert.ToInt32(bunch[1]);
						break;
					case "replytolist":
						if (string.Compare(bunch[1], "true", true) == 0)
                            list.ReplyToMode = string.Compare(bunch[1], "true", true) == 0 ? ReplyTo.RepliesToList : ReplyTo.RepliesToSender;
						break;
				}
			}
		}

		protected void SetMailListMembers(MailList list, string[] subscribers)
		{
			List<string> members = new List<string>();

			foreach (string subscriber in subscribers)
				members.Add(subscriber);

			list.Members = members.ToArray();
		}


        public override bool IsInstalled()
        {
            string productName = null;
            string productVersion = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            String[] names = null;

            if (key != null)
            {
                names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + s);
                    if (subkey != null)
                        if (!String.IsNullOrEmpty((string)subkey.GetValue("DisplayName")))
                        {
                            productName = (string)subkey.GetValue("DisplayName");
                        }
                    if (productName != null)
                        if (productName.Equals("SmarterMail"))
                        {
                            if (subkey != null) productVersion = (string)subkey.GetValue("DisplayVersion");
                            break;
                        }
                }

                if (!String.IsNullOrEmpty(productVersion))
                {
                    string[] split = productVersion.Split(new char[] { '.' });
                    return split[0].Equals("3") | split[0].Equals("4");
                }
            }
            else
            {
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

                if (key == null)
                {
                    return false;
                }

                names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + s);
                    if (subkey != null)
                        if (!String.IsNullOrEmpty((string)subkey.GetValue("DisplayName")))
                        {
                            productName = (string)subkey.GetValue("DisplayName");
                        }
                    if (productName != null)
                        if (productName.Equals("SmarterMail"))
                        {
                            if (subkey != null) productVersion = (string)subkey.GetValue("DisplayVersion");
                            break;
                        }
                }

                if (!String.IsNullOrEmpty(productVersion))
                {
                    string[] split = productVersion.Split(new[] { '.' });
                    return split[0].Equals("3") | split[0].Equals("4");
                }
            }
            return false;
        }
    }
}
