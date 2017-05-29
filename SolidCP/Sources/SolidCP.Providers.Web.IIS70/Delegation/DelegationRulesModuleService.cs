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
using SolidCP.Providers.Web.Iis.Common;
using Microsoft.Web.Administration;
using System.Reflection;

namespace SolidCP.Providers.Web.Delegation
{
	internal sealed class DelegationRulesModuleService : ConfigurationModuleService
	{
		public void RestrictRuleToUser(string providers, string path, string accountName)
		{
			var rulePredicate = new Predicate<ConfigurationElement>(x => { return x.Attributes["providers"].Value.Equals(providers) && x.Attributes["path"].Value.Equals(path); });
			//
			var userPredicate = new Predicate<ConfigurationElement>(x => { return x.Attributes["name"].Value.Equals(accountName); });
			//
			using (var srvman = new ServerManager())
			{
				var adminConfig = srvman.GetAdministrationConfiguration();

                // return if system.webServer/management/delegation section is not exist in config file 
                if (!HasDelegationSection(adminConfig))
			        return;
				
                var delegationSection = adminConfig.GetSection("system.webServer/management/delegation");
				//
				var rulesCollection = delegationSection.GetCollection();
				// Update rule if exists
				foreach (var rule in rulesCollection)
				{
					if (rulePredicate.Invoke(rule) == true)
					{
						var permissions = rule.GetCollection("permissions");
						//
						var user = default(ConfigurationElement);
						//
						foreach (var item in permissions)
						{
							if (userPredicate.Invoke(item))
							{
								user = item;
								//
								break;
							}
						}
						//
						if (user == null)
						{
							user = permissions.CreateElement("user");
							//
							user.SetAttributeValue("name", accountName);
							user.SetAttributeValue("isRole", false);
							//
							permissions.Add(user);
						}
						//
						if (user != null)
						{
							user.SetAttributeValue("accessType", "Deny");
							//
							srvman.CommitChanges();
						}
					}
				}
			}
		}

		public void AllowRuleToUser(string providers, string path, string accountName)
		{
			RemoveUserFromRule(providers, path, accountName);
		}

		public void RemoveUserFromRule(string providers, string path, string accountName)
		{
			var rulePredicate = new Predicate<ConfigurationElement>(x => { return x.Attributes["providers"].Value.Equals(providers) && x.Attributes["path"].Value.Equals(path); });
			//
			var userPredicate = new Predicate<ConfigurationElement>(x => { return x.Attributes["name"].Value.Equals(accountName); });
			//
			using (var srvman = new ServerManager())
			{
				var adminConfig = srvman.GetAdministrationConfiguration();
              
                // return if system.webServer/management/delegation section is not exist in config file 
                if (!HasDelegationSection(adminConfig))
                    return;

                var delegationSection = adminConfig.GetSection("system.webServer/management/delegation");
				//
				var rulesCollection = delegationSection.GetCollection();
				// Update rule if exists
				foreach (var rule in rulesCollection)
				{
					if (rulePredicate.Invoke(rule) == true)
					{
						var permissions = rule.GetCollection("permissions");
						//
						foreach (var user in permissions)
						{
							if (userPredicate.Invoke(user))
							{
								permissions.Remove(user);
								//
								srvman.CommitChanges();
								//
								break;
							}
						}
					}
				}
			}
		}

		public bool DelegationRuleExists(string providers, string path)
		{
			var exists = false;
			//
			var predicate = new Predicate<ConfigurationElement>(x =>
			{
				return x.Attributes["providers"].Value.Equals(providers) && x.Attributes["path"].Value.Equals(path);
			});
			//
			using (var srvman = new ServerManager())
			{
				var adminConfig = srvman.GetAdministrationConfiguration();

                // return if system.webServer/management/delegation section is not exist in config file 
                if (!HasDelegationSection(adminConfig))
                    return false;

                var delegationSection = adminConfig.GetSection("system.webServer/management/delegation");
				//
				var rulesCollection = delegationSection.GetCollection();
				// Update rule if exists
				foreach (var rule in rulesCollection)
				{
					if (predicate.Invoke(rule) == true)
					{
						exists = true;
						//
						break;
					}
				}
			}
			//
			return exists;
		}

		public void AddDelegationRule(string providers, string path, string pathType, string identityType, string userName, string userPassword)
		{
			var predicate = new Predicate<ConfigurationElement>(x =>
			{
				return x.Attributes["providers"].Value.Equals(providers) && x.Attributes["path"].Value.Equals(path);
			});
			//
			using (var srvman = GetServerManager())
			{
				var adminConfig = srvman.GetAdministrationConfiguration();

                // return if system.webServer/management/delegation section is not exist in config file 
                if (!HasDelegationSection(adminConfig))
                    return;

                var delegationSection = adminConfig.GetSection("system.webServer/management/delegation");
				//
				var rulesCollection = delegationSection.GetCollection();
				// Update rule if exists
				foreach (var rule in rulesCollection)
				{
					//
					if (predicate.Invoke(rule) == true)
					{
						if (identityType.Equals("SpecificUser"))
						{
							var runAsElement = rule.ChildElements["runAs"];
							//
							runAsElement.SetAttributeValue("userName", userName);
							runAsElement.SetAttributeValue("password", userPassword);
							// Ensure the rules is enabled
							if (rule.Attributes["enabled"].Equals(false))
							{
								rule.SetAttributeValue("enabled", true);
							}
							//
							srvman.CommitChanges();
						}
						//
						return; // Exit
					}
				}
				// Create new rule if none exists
				var newRule = rulesCollection.CreateElement("rule");
				newRule.SetAttributeValue("providers", providers);
				newRule.SetAttributeValue("actions", "*"); // Any
				newRule.SetAttributeValue("path", path);
				newRule.SetAttributeValue("pathType", pathType);
				newRule.SetAttributeValue("enabled", true);
				// Run rule as SpecificUser
				if (identityType.Equals("SpecificUser"))
				{
					var runAs = newRule.GetChildElement("runAs");
					//
					runAs.SetAttributeValue("identityType", "SpecificUser");
					runAs.SetAttributeValue("userName", userName);
					runAs.SetAttributeValue("password", userPassword);
				}
				else // Run rule as CurrentUser
				{
					var runAs = newRule.GetChildElement("runAs");
					//
					runAs.SetAttributeValue("identityType", "CurrentUser");
				}
				// Establish permissions
				var permissions = newRule.GetCollection("permissions");
				var user = permissions.CreateElement("user");
				user.SetAttributeValue("name", "*");
				user.SetAttributeValue("accessType", "Allow");
				user.SetAttributeValue("isRole", false);
				permissions.Add(user);
				//
				rulesCollection.Add(newRule);
				//
				srvman.CommitChanges();
			}
		}

		public void RemoveDelegationRule(string providers, string path)
		{
			var predicate = new Predicate<ConfigurationElement>(x =>
			{
				return x.Attributes["providers"].Value.Equals(providers) && x.Attributes["path"].Value.Equals(path);
			});
			//
			using (var srvman = GetServerManager())
			{
				var adminConfig = srvman.GetAdministrationConfiguration();

                // return if system.webServer/management/delegation section is not exist in config file 
                if (!HasDelegationSection(adminConfig))
                    return;

                var delegationSection = adminConfig.GetSection("system.webServer/management/delegation");
				//
				var rulesCollection = delegationSection.GetCollection();
				// Remove rule if exists
				foreach (var rule in rulesCollection)
				{
					// Match rule against predicate
					if (predicate.Invoke(rule) == true)
					{
						rulesCollection.Remove(rule);
						//
						srvman.CommitChanges();
						//
						return; // Exit
					}
				}
			}
		}

        private bool HasDelegationSection(Configuration adminConfig)
        {
            // try to get delegation section in config file (C:\Windows\system32\inetsrv\config\administration.config)
            try
            {
                adminConfig.GetSection("system.webServer/management/delegation");
            }
            catch (AmbiguousMatchException)
            {
                /* skip */
                return false;
            }

            return true;
        }
	}
}
