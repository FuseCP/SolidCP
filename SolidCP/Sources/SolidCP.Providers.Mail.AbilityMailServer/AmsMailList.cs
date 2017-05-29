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

namespace SolidCP.Providers.Mail
{
	public class AmsMailList
	{
		private TreeNode nodeConfig;
		private Tree listConfig;
		private string maillistName;

		public TreeNode Config
		{
			get { return nodeConfig; }
		}

		public Tree ListConfig
		{
			get { return listConfig; }
		}

		public AmsMailList(string maillistName)
		{
			this.maillistName = maillistName;
			nodeConfig = new TreeNode();
			listConfig = new Tree();
		}

		public bool Load(Tree config)
		{
			string account = AmsMailbox.GetAccountName(maillistName);
			string domain = AmsMailbox.GetDomainName(maillistName);

			foreach (TreeNode node in config.ChildNodes)
			{
				string amsUser = node["user"];
				string amsDomain = node["domain"];

				if (string.Compare(amsUser, account, true) == 0 &&
					string.Compare(amsDomain, domain, true) == 0)
				{
					nodeConfig = node;
					return true;
				}
			}

			return false;
		}

		public bool Load(TreeNode configNode)
		{
			string account = AmsMailbox.GetAccountName(maillistName);
			string domain = AmsMailbox.GetDomainName(maillistName);

			string amsUser = configNode["user"];
			string amsDomain = configNode["domain"];

			if (string.Compare(amsUser, account, true) == 0 &&
					string.Compare(amsDomain, domain, true) == 0)
			{
				nodeConfig = configNode;
				return true;
			}

			return false;
		}

		public MailList ToMailList()
		{
			MailList list = new MailList();

			list.Name = maillistName;

			if (nodeConfig["enabled"] == "1")
				list.Enabled = true;

			// copy mail list members
			TreeNode addresses = listConfig.ChildNodes["addresses"];

			if (addresses != null)
			{
				List<string> members = new List<string>();

				foreach (TreeNode node in addresses.ChildNodes)
					members.Add(node.NodeValue);

				list.Members = members.ToArray();
			}

			return list;
		}

		public bool Save(Tree config)
		{
			if (!config.ChildNodes.Contains(nodeConfig))
			{
				nodeConfig["file"] = string.Format(AMSHelper.MailListConfigFile, AmsMailbox.GetDomainName(maillistName), AmsMailbox.GetAccountName(maillistName));
				config.ChildNodes.Add(nodeConfig);
			}

			return AMSHelper.SetMailListsConfig(config) &&
				AMSHelper.SetMailingListConfig(listConfig, maillistName);
		}

		public void LoadListConfig()
		{
			this.listConfig = AMSHelper.GetMailingListConfig(maillistName);
		}

		public void Read(MailList maillist)
		{
			nodeConfig["domain"] = AmsMailbox.GetDomainName(maillist.Name);
			nodeConfig["enabled"] = maillist.Enabled ? "1" : "0";
			nodeConfig["user"] = AmsMailbox.GetAccountName(maillist.Name);
			nodeConfig["triggertext"] = "TRIGGER";
			nodeConfig["usetriggeredreplyto"] = "0";
			nodeConfig["triggeredreplyto"] = string.Empty;
			nodeConfig["usetriggeredfrom"] = string.Empty;
			nodeConfig["triggeredfrom"] = string.Empty;
			nodeConfig["maxtriggersperday"] = "100";
			nodeConfig["useonlymemberscantrigger"] = "0";
			nodeConfig["maxaddresses"] = "5000";

			// copy mail list members
			TreeNode addresses = listConfig.ChildNodes["addresses"];

			if (addresses == null)
			{
				addresses = new TreeNode();
				addresses.NodeName = "addresses";
				listConfig.ChildNodes.Add(addresses);
			}
			addresses.ChildNodes.Clear();

			foreach (string member in maillist.Members)
			{
				TreeNode node = new TreeNode(addresses);
				node.NodeValue = member;
				addresses.ChildNodes.Add(node);
			}
		}

		public bool Delete(Tree config)
		{
			if (config.ChildNodes.Contains(nodeConfig))
				config.ChildNodes.Remove(nodeConfig);

			return AMSHelper.RemoveMailList(maillistName) &&
				AMSHelper.SetMailListsConfig(config);
		}

		public static AmsMailList[] GetMailLists(Tree config, string domainName)
		{
			List<AmsMailList> list = new List<AmsMailList>();

			foreach(TreeNode node in config.ChildNodes)
			{
				string user = node["user"];
				string domain = node["domain"];
				if (string.Compare(domain, domainName, true) == 0)
				{
					AmsMailList ml = new AmsMailList(string.Concat(user, "@", domain));
					ml.Load(node);
					ml.LoadListConfig();

					list.Add(ml);
				}
			}

			return list.ToArray();
		}
	}
}
