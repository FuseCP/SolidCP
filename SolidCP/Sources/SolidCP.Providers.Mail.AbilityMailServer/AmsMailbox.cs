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
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.Mail
{
	public class AmsMailbox
	{
		private TreeNode userConfig;
		private Tree accountConfig;
		private Tree deliveryConfig;
		private string mailboxName;

		public TreeNode UserConfig
		{
			get { return userConfig; }
		}

		public Tree AccountConfig
		{
			get { return accountConfig;  }
			/*set { accountConfig = value; }*/
		}

		public Tree DeliveryConfig
		{
			get { return deliveryConfig;  }
			/*set { deliveryConfig = value; }*/
		}

		public AmsMailbox(string mailboxName)
		{
			this.mailboxName = mailboxName;

			this.accountConfig = new Tree();
			this.deliveryConfig = new Tree();

			this.userConfig = new TreeNode();
		}

		public bool Load(Tree userConfig)
		{
			string account = GetAccountName(mailboxName);
			string domain = GetDomainName(mailboxName);

			foreach (TreeNode node in userConfig.ChildNodes)
			{
				string amsUser = node["user"];
				string amsDomain = node["domain"];

				if (string.Compare(amsUser, account, true) == 0 && 
					string.Compare(amsDomain, domain, true) == 0)
				{
					this.userConfig = node;
					return true;
				}
			}

			return false;
		}

		public bool Load(TreeNode configNode)
		{
			string account = GetAccountName(mailboxName);
			string domain = GetDomainName(mailboxName);

			string amsUser = configNode["user"];
			string amsDomain = configNode["domain"];

			if (string.Compare(amsUser, account, true) == 0 &&
					string.Compare(amsDomain, domain, true) == 0)
			{
				this.userConfig = configNode;
				return true;
			}

			return false;
		}

		public void LoadAccountConfig()
		{
			this.accountConfig = AMSHelper.GetAccountConfig(mailboxName);
			this.deliveryConfig = AMSHelper.GetAccountDelivery(mailboxName);
		}

		public bool Save(Tree config)
		{
			if (!config.ChildNodes.Contains(userConfig))
			{
				userConfig["dir"] = Path.Combine(AMSHelper.AMSLocation, string.Format(@"{0}\{1}\", GetDomainName(mailboxName), GetAccountName(mailboxName)));
				config.ChildNodes.Add(userConfig);
			}

			return AMSHelper.SetUsersConfig(config) &&
				AMSHelper.SetAccountConfig(accountConfig, mailboxName) &&
				AMSHelper.SetAccountDelivery(deliveryConfig, mailboxName);
		}

		public bool Delete(Tree config)
		{
			if (config.ChildNodes.Contains(userConfig))
				config.ChildNodes.Remove(userConfig);

			return AMSHelper.RemoveAccount(mailboxName) &&
				AMSHelper.SetUsersConfig(config);
		}

		public MailAccount ToMailAccount()
		{
			MailAccount account = new MailAccount();

			account.Name = string.Concat(userConfig["user"], "@", userConfig["domain"]);
			account.Enabled = userConfig["enabled"] == "1" ? true : false;
			account.Password = userConfig["pass"];

			// read forwardings
			TreeNode redirection = deliveryConfig.ChildNodes["redirection"];
			if (redirection != null)
			{
				TreeNode redirections = redirection.ChildNodes["redirections"];

				if (redirections != null)
				{
					List<string> list = new List<string>();
					foreach (TreeNode node in redirections.ChildNodes)
						list.Add(node.NodeValue);

					account.ForwardingAddresses = list.ToArray();
				}
			}

			// read autoresponder
			TreeNode autoresponses = deliveryConfig.ChildNodes["autoresponses"];
			if (autoresponses != null)
			{
				account.ResponderEnabled = autoresponses["enabled"] == "1" ? true : false;
				account.ResponderSubject = autoresponses["subject"];
				account.ResponderMessage = autoresponses["body"];

				if (autoresponses["usereplyto"] == "1")
					account.ReplyTo = autoresponses["replyto"];
			}

			return account;
		}

		public MailGroup ToMailGroup()
		{
			MailGroup group = new MailGroup();

			group.Name = mailboxName;
			group.Enabled = userConfig["enabled"] == "1" ? true : false;

			TreeNode redirection = deliveryConfig.ChildNodes["redirection"];

			if (redirection != null)
			{
				TreeNode redirections = redirection.ChildNodes["redirections"];

				if (redirections != null)
				{
					List<string> list = new List<string>();
					
					foreach (TreeNode node in redirections.ChildNodes)
						list.Add(node.NodeValue);

					group.Members = list.ToArray();
				}
			}

			return group;
		}

		public void Read(MailAccount mailbox)
		{
			userConfig["domain"] = GetDomainName(mailbox.Name);
			userConfig["enabled"] = mailbox.Enabled ? "1" : "0";
			userConfig["user"] = GetAccountName(mailbox.Name);
			userConfig["pass"] = mailbox.Password;
			// forwardings
			if (mailbox.ForwardingAddresses != null)
				AddForwardingInfo(mailbox.ForwardingAddresses, mailbox.DeleteOnForward);

			AddAutoResponderInfo(mailbox);
		}

		private void AddAutoResponderInfo(MailAccount mailbox)
		{
			TreeNode autoresponses = deliveryConfig.ChildNodes["autoresponses"];

			if (autoresponses == null)
			{
				autoresponses = new TreeNode();
				autoresponses.NodeName = "autoresponses";
				deliveryConfig.ChildNodes.Add(autoresponses);
			}

			autoresponses["enabled"] = mailbox.ResponderEnabled ? "1" : "0";

			if (mailbox.ResponderEnabled)
			{
				autoresponses["subject"] = mailbox.ResponderSubject;
				autoresponses["body"] = mailbox.ResponderMessage;

				if (!string.IsNullOrEmpty(mailbox.ReplyTo))
				{
					autoresponses["usereplyto"] = "1";
					autoresponses["replyto"] = mailbox.ReplyTo;
				}
				else
				{
					autoresponses["usereplyto"] = "0";
					autoresponses["replyto"] = string.Empty;
				}
			}
			else
			{
				autoresponses["subject"] = string.Empty;
				autoresponses["body"] = string.Empty;
			}
		}

		private void AddForwardingInfo(string[] forwardings, bool removeCopies)
		{
			TreeNode redirection = deliveryConfig.ChildNodes["redirection"];

			if (redirection == null)
			{
				redirection = new TreeNode();
				redirection.NodeName = "redirection";
				deliveryConfig.ChildNodes.Add(redirection);
			}

			if (forwardings.Length > 0)
			{
				redirection["enabled"] = "1";
				redirection["stilldeliver"] = removeCopies ? "0" : "1";

				TreeNode redirections = redirection.ChildNodes["redirections"];

				if (redirections == null)
				{
					redirections = new TreeNode(redirection);
					redirections.NodeName = "redirections";
					redirection.ChildNodes.Add(redirections);
				}
				redirections.ChildNodes.Clear();

				foreach (string forwarding in forwardings)
				{
					TreeNode node = new TreeNode(redirections);
					node.NodeValue = forwarding;
					redirections.ChildNodes.Add(node);
				}
			}
		}

		public void Read(MailGroup group)
		{
			userConfig["domain"] = GetDomainName(group.Name);
			userConfig["enabled"] = group.Enabled ? "1" : "0";
			userConfig["user"] = GetAccountName(group.Name);

			AddForwardingInfo(group.Members, true);
		}

		public bool IsMailGroup()
		{
			TreeNode redirection = deliveryConfig.ChildNodes["redirection"];

			if (redirection != null)
				if (redirection["stilldeliver"] == "0")
					return true;

			return false;
		}

		public static AmsMailbox[] GetMailboxes(Tree config, string domainName)
		{
			List<AmsMailbox> list = new List<AmsMailbox>();

			foreach (TreeNode node in config.ChildNodes)
			{
				string account = node["user"];
				string amsDomain = node["domain"];
				if (string.Compare(amsDomain, domainName, true) == 0)
				{
					AmsMailbox mb = new AmsMailbox(string.Concat(account, "@", amsDomain));
					mb.Load(node);
					mb.LoadAccountConfig();
					list.Add(mb);
				}
			}

			return list.ToArray();
		}

		public static AmsMailbox[] GetMailGroups(Tree config, string domainName)
		{
			List<AmsMailbox> groups = new List<AmsMailbox>();

			foreach (TreeNode node in config.ChildNodes)
			{
				string account = node["user"];
				string amsDomain = node["domain"];

				if (string.Compare(amsDomain, domainName, true) == 0)
				{
					AmsMailbox mb = new AmsMailbox(string.Concat(account, "@", amsDomain));
					mb.Load(node);
					mb.LoadAccountConfig();

					if (mb.IsMailGroup())
						groups.Add(mb);
				}
			}

			return groups.ToArray();
		}

		public static string GetAccountName(string email)
		{
			if (email.IndexOf("@") == -1)
				return email;

			return email.Substring(0, email.IndexOf("@"));
		}

		public static string GetDomainName(string email)
		{
			return email.Substring(email.IndexOf("@") + 1);
		}
	}
}
