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
using System.IO;
using System.Text;

namespace SolidCP.Providers.Mail
{
	public class AmsDomain
	{
		private string domainName;
		private TreeNode domainConfig;

		public SolidCP.Providers.Mail.TreeNode DomainConfig
		{
			get { return this.domainConfig; }
		}

		public AmsDomain(string domainName)
		{
			this.domainName = domainName;
			this.domainConfig = new TreeNode();
		}

		public bool Load(Tree config)
		{
			foreach (TreeNode node in config.ChildNodes)
			{
				string amsDomain = node["domain"];
				if (String.Compare(amsDomain, domainName, true) == 0)
				{
					this.domainConfig = node;
					return true;
				}
			}
			return false;
		}

		public bool Save(Tree config)
		{
			if (!config.ChildNodes.Contains(domainConfig))
			{
                domainConfig["dir"] = Path.Combine(AMSHelper.AMSLocation, domainName);
				config.ChildNodes.Add(domainConfig);
			}

			return AMSHelper.SetDomainsConfig(config);
		}

		public bool Delete(Tree config)
		{
			if (config.ChildNodes.Contains(domainConfig))
				config.ChildNodes.Remove(domainConfig);

			Tree usersConfig = AMSHelper.GetUsersConfig();
			List<TreeNode> nodesToDelete = new List<TreeNode>();
			foreach (TreeNode node in usersConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				usersConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			Tree listsConfig = AMSHelper.GetMailListsConfig();
			foreach (TreeNode node in listsConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				listsConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			return AMSHelper.RemoveDomain(domainName) && 
				AMSHelper.SetUsersConfig(usersConfig) && 
				AMSHelper.SetMailListsConfig(listsConfig) && 
				AMSHelper.SetDomainsConfig(config);
		}

		public bool DeleteAlias(Tree config)
		{
			if (config.ChildNodes.Contains(domainConfig))
				config.ChildNodes.Remove(domainConfig);

			Tree usersConfig = AMSHelper.GetUsersConfig();
			List<TreeNode> nodesToDelete = new List<TreeNode>();
			foreach (TreeNode node in usersConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				usersConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			Tree listsConfig = AMSHelper.GetMailListsConfig();
			foreach (TreeNode node in listsConfig.ChildNodes)
			{
				if (string.Compare(node["domain"], domainName, true) == 0)
					nodesToDelete.Add(node);
			}

			while (nodesToDelete.Count > 0)
			{
				listsConfig.ChildNodes.Remove(nodesToDelete[0]);
				nodesToDelete.RemoveAt(0);
			}

			return AMSHelper.SetUsersConfig(usersConfig) &&
				AMSHelper.SetMailListsConfig(listsConfig) &&
				AMSHelper.SetDomainsConfig(config);
		}

		public void Read(MailDomain domain)
		{
			domainConfig["enabled"] = domain.Enabled ? "1" : "0";
			domainConfig["domain"] = domain.Name;
			domainConfig["mode"] = "0";

			domainConfig["usemaxusers"] = (domain.MaxDomainUsers == 0) ? "0" : "1";
			domainConfig["maxusers"] = (domain.MaxDomainUsers == 0) ? "0" : domain.MaxDomainUsers.ToString();

			domainConfig["usemaxmailinglists"] = (domain.MaxLists == 0) ? "0" : "1";
			domainConfig["maxmailinglists"] = (domain.MaxLists == 0) ? "0" : domain.MaxLists.ToString();
			
			if (!string.IsNullOrEmpty(domain.CatchAllAccount))
			{
				domainConfig["usecatchcalluser"] =  "1";
				domainConfig["catchalluser"] = domain.CatchAllAccount;
			}
			else
			{
				domainConfig["usecatchcalluser"] = "0";
				domainConfig["catchalluser"] = string.Empty;
			}
		}

		public MailDomain ToMailDomain()
		{
			if (domainConfig["mode"] == "0")
			{
				MailDomain domain = new MailDomain();

				domain.Enabled = domainConfig["enabled"] == "1" ? true : false;
				domain.Name = domainConfig["domain"];

				if (domainConfig["usemaxusers"] == "1")
					domain.MaxDomainUsers = Convert.ToInt32(domainConfig["maxusers"]);

				if (domainConfig["usemaxmailinglists"] == "1")
					domain.MaxLists = Convert.ToInt32(domainConfig["maxmailinglists"]);

				if (domainConfig["usecatchcalluser"] == "1")
					domain.CatchAllAccount = domainConfig["catchalluser"];

				return domain;
			}

			return null;
		}

		public static string[] GetDomainAliases(Tree config, string domainName)
		{
			List<string> list = new List<string>();

			foreach (TreeNode node in config.ChildNodes)
			{
				string mode = node["mode"];
				string convert = node["convertdomain"];

				if (String.Compare(convert, domainName, true) == 0 && mode == "1")
					list.Add(node["domain"]);
			}

			return list.ToArray();
		}

		public static string[] GetDomains(Tree config)
		{
			List<string> domains = new List<string>();

			foreach (TreeNode node in config.ChildNodes)
				domains.Add(node["domain"]);

			return domains.ToArray();
		}
	}
}
