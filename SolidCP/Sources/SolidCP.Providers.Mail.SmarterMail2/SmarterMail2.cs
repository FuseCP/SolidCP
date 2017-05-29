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
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Xml.XPath;
using SolidCP.Providers.Common;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;
using Microsoft.Win32;

namespace SolidCP.Providers.Mail
{
    public class SmarterMail2 : HostingServiceProviderBase, IMailServer
    {
        #region Constants
        public const string SYSTEM_DOMAIN_ADMIN = "system.domain.admin";
        public const string SYSTEM_CATCH_ALL = "system.catch.all";
        #endregion

        #region Public Properties
        protected string AdminUsername
        {
            get { return ProviderSettings["AdminUsername"]; }
        }

        protected string AdminPassword
        {
            get { return ProviderSettings["AdminPassword"]; }
        }

        protected bool ImportDomainAdmin
        {
            get
            {
                bool res;
                bool.TryParse(ProviderSettings[Constants.ImportDomainAdmin], out res);
                return res;
            }
        }

        protected bool InheritDomainDefaultLimits
        {
            get
            {
                bool res;
                bool.TryParse(ProviderSettings[Constants.InheritDomainDefaultLimits], out res);
                return res;
            }
        }

        protected string DomainsPath
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["DomainsPath"]); }
        }

        protected string ServerIP
        {
            get
            {
                string val = ProviderSettings["ServerIPAddress"];
                if(String.IsNullOrEmpty(val))
                    return "127.0.0.1";

				string ip = val.Trim();
				if (ip.IndexOf(";") > -1)
				{
					string[] ips = ip.Split(';');
					ip = String.IsNullOrEmpty(ips[1]) ? ips[0] : ips[1]; // get internal IP part
				}
				return ip;
            }
        }

        protected string ServiceUrl
        {
            get { return ProviderSettings["ServiceUrl"]; }
        }
        #endregion

        #region Mail domains

        /// <summary>
        /// Returns domain info
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <returns>Domain info</returns>
        public virtual MailDomain GetDomain(string domainName)
        {
            try
            {
                svcDomainAdmin domains = new svcDomainAdmin();
                PrepareProxy(domains);

                DomainSettingsResult result = domains.GetDomainSettings(AdminUsername, AdminPassword, domainName);
                if (!result.Result)
                    throw new Exception(result.Message);

                // fill domain properties
                MailDomain domain = new MailDomain();
                domain.Name = domainName;
                domain.Path = result.Path;
                domain.ServerIP = result.ServerIP;
                domain.ImapPort = result.ImapPort;
                domain.SmtpPort = result.SmtpPort;
                domain.PopPort = result.PopPort;
                domain.MaxAliases = result.MaxAliases;
                domain.MaxDomainAliases = result.MaxDomainAliases;
                domain.MaxLists = result.MaxLists;
                domain.MaxDomainSizeInMB = result.MaxDomainSizeInMB;
                domain.MaxDomainUsers = result.MaxDomainUsers;
                domain.MaxMailboxSizeInMB = result.MaxMailboxSizeInMB;
                domain.MaxMessageSize = result.MaxMessageSize;
                domain.MaxRecipients = result.MaxRecipients;
                domain.RequireSmtpAuthentication = result.RequireSmtpAuthentication;
                domain.ListCommandAddress = result.ListCommandAddress;
                domain.ShowContentFilteringMenu = result.ShowContentFilteringMenu;
                domain.ShowDomainAliasMenu = result.ShowDomainAliasMenu;
                domain.ShowListMenu = result.ShowListMenu;
                domain.ShowSpamMenu = result.ShowSpamMenu;
                domain.ShowsStatsMenu = result.ShowStatsMenu;
				// get additional domain settings
                string[] requestedSettings = new string[]
                {
                    "catchall",
                    "isenabled",
                    "ldapport",
                    "altsmtpport",
                    "sharedcalendar",
                    "sharedcontact",
                    "sharedfolder",
                    "sharednotes",
                    "sharedtasks",
                    "sharedgal",
                    "bypassforwardblacklist",
					
                };

                SettingsRequestResult addResult = domains.GetRequestedDomainSettings(AdminUsername, AdminPassword, domainName, requestedSettings);
                if (!addResult.Result)
                    throw new Exception(addResult.Message);

                FillMailDomainFields(domain, addResult);
                
                // get catch-all address
                if (!String.IsNullOrEmpty(domain.CatchAllAccount))
                {
                    // get catch-all group
                    string groupName = SYSTEM_CATCH_ALL + "@" + domain.Name;
                    if (GroupExists(groupName))
                    {
                        // get the first member of this group
                        MailGroup group = GetGroup(groupName);
                        domain.CatchAllAccount = GetAccountName(group.Members[0]);
                    }
                }

                /* 
				//get license information
				if (GetProductLicenseInfo() == "PRO")
				{
					domain[MailDomain.SMARTERMAIL_LICENSE_TYPE] = "PRO";
				}
				if (GetProductLicenseInfo() == "ENT")
				{
					domain[MailDomain.SMARTERMAIL_LICENSE_TYPE] = "ENT";
				}
				if (GetProductLicenseInfo() == "FREE")
				{
					domain[MailDomain.SMARTERMAIL_LICENSE_TYPE] = "FREE";
				}
                */
                 
                return domain;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get mail domain", ex);
            }
        }

        
        private static void FillMailDomainFields(MailDomain domain, SettingsRequestResult addResult)
        {
            foreach (string pair in addResult.settingValues)
            {
                string[] parts = pair.Split('=');
                switch (parts[0])
                {
                    case "catchall":
                        domain.CatchAllAccount = parts[1];
                        break;
                    case "isenabled":
                        domain.Enabled = Boolean.Parse(parts[1]);
                        break;
                    case "ldapport":
                        domain.LdapPort = int.Parse(parts[1]);
                        break;
                    case "altsmtpport":
                        domain.SmtpPortAlt = int.Parse(parts[1]);
                        break;
                    case "sharedcalendar":
                        domain.SharedCalendars = Boolean.Parse(parts[1]);
                        break;
                    case "sharedcontact":
                        domain.SharedContacts = Boolean.Parse(parts[1]);
                        break;
                    case "sharedfolder":
                        domain.SharedFolders = Boolean.Parse(parts[1]);
                        break;
                    case "sharednotes":
                        domain.SharedNotes = Boolean.Parse(parts[1]);
                        break;
                    case "sharedtasks":
                        domain.SharedTasks = Boolean.Parse(parts[1]);
                        break;
                    case "sharedgal":
                        domain.IsGlobalAddressList = Boolean.Parse(parts[1]);
                        break;
                    case "bypassforwardblacklist":
                        domain.BypassForwardBlackList = Boolean.Parse(parts[1]);
                        break;     
					
                }
            }
        }
        
        /// <summary>
        /// Returns a list of all domain names
        /// </summary>
        /// <returns>Array with domain names</returns>
        public virtual string[] GetDomains()
        {
            try
            {
                svcDomainAdmin domains = new svcDomainAdmin();
                PrepareProxy(domains);

                DomainListResult result = domains.GetAllDomains(AdminUsername, AdminPassword);
                if (!result.Result)
                    throw new Exception(result.Message);

                return result.DomainNames;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get the list of mail domains", ex);
            }
        }

        /// <summary>
        /// Checks whether the specified domain exists
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <returns>true if the specified domain exists, otherwise false</returns>
        public virtual bool DomainExists(string domainName)
        {
            try
            {
                svcDomainAdmin domains = new svcDomainAdmin();
                PrepareProxy(domains);

                DomainSettingsResult result = domains.GetDomainSettings(AdminUsername, AdminPassword, domainName);
                return result.Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not check whether mail domain exists", ex);
            }
        }

        /// <summary>
        /// Creates a new domain in the specified folder 
        /// </summary>
        /// <param name="domain">Domain info</param>
        public virtual void CreateDomain(MailDomain domain)
        {
            try
            {
                svcDomainAdmin domains = new svcDomainAdmin();
                PrepareProxy(domains);

				
                DomainSettingsResult defaultDomainSettings = domains.GetDomainDefaults(AdminUsername, AdminPassword);

                SettingsRequestResult defaultRequestedSettings =
                    domains.GetRequestedDomainDefaults(AdminUsername, AdminPassword, new string[] { 
                        "defaultaltsmtpport",
						"defaultimapport",
						"defaultmaxaliases",
						"defaultmaxdomainaliases",
						"defaultmaxdomainsize",
						"defaultmaxdomainusers",
						"defaultmaxlists",
						"defaultmaxmailboxsize",
						"defaultmaxmessagesize",
						"defaultmaxrecipients",
						"defaultpopport",
						"defaultshowcontentfilteringmenu",
						"defaultshowdomainaliasmenu",
						"defaultshowlistmenu",
						"defaultshowspammenu",
						"defaultshowstatmenu",
						"defaultsmtpauthenticationrequired",
						"defaultsmtpport",
						"defaultbypassforwardblacklist",
						"defaultldapport",
						"defaultldapdisallowoptout",
						"defaultsharedcalendar",
						"defaultsharedcontact",
						"defaultsharedfolder",
						"defaultsharedtasks",
						"defaultsharedgal"
						});


            	string[] requestedDomainDefaults = defaultRequestedSettings.settingValues;

				//domain Path is taken from SolidCP Service settings

                GenericResult1 result = null;

                if (!InheritDomainDefaultLimits)
                {
                    result = domains.AddDomain(AdminUsername, AdminPassword,
                                                             domain.Name,
                                                             Path.Combine(DomainsPath, domain.Name),
                                                             SYSTEM_DOMAIN_ADMIN, // admin username
                                                             Guid.NewGuid().ToString("P"), // admin password
                                                             "Domain", // admin first name
                                                             "Administrator", // admin last name
                                                             ServerIP,
                                                             defaultDomainSettings.ImapPort,
                                                             defaultDomainSettings.PopPort,
                                                             defaultDomainSettings.SmtpPort,
                                                             domain.MaxAliases,
                                                             domain.MaxDomainSizeInMB,
                                                             domain.MaxDomainUsers,
                                                             domain.MaxMailboxSizeInMB,
                                                             domain.MaxMessageSize,
                                                             domain.MaxRecipients,
                                                             domain.MaxDomainAliases,
                                                             domain.MaxLists,
                                                             defaultDomainSettings.ShowDomainAliasMenu,// ShowDomainAliasMenu
                                                             defaultDomainSettings.ShowContentFilteringMenu,// ShowContentFilteringMenu
                                                             defaultDomainSettings.ShowSpamMenu, // ShowSpamMenu
                                                             defaultDomainSettings.ShowStatsMenu, // ShowStatsMenu
                                                             defaultDomainSettings.RequireSmtpAuthentication,
                                                             defaultDomainSettings.ShowListMenu, // ShowListMenu
                                                             defaultDomainSettings.ListCommandAddress);
                }
                else
                {
                    result = domains.AddDomain(AdminUsername, AdminPassword,
                                                             domain.Name,
                                                             Path.Combine(DomainsPath, domain.Name),
                                                             SYSTEM_DOMAIN_ADMIN, // admin username
                                                             Guid.NewGuid().ToString("P"), // admin password
                                                             "Domain", // admin first name
                                                             "Administrator", // admin last name
                                                             ServerIP,
                                                             defaultDomainSettings.ImapPort,
                                                             defaultDomainSettings.PopPort,
                                                             defaultDomainSettings.SmtpPort,
                                                             defaultDomainSettings.MaxAliases,
                                                             defaultDomainSettings.MaxDomainSizeInMB,
                                                             defaultDomainSettings.MaxDomainUsers,
                                                             defaultDomainSettings.MaxMailboxSizeInMB,
                                                             defaultDomainSettings.MaxMessageSize,
                                                             defaultDomainSettings.MaxRecipients,
                                                             defaultDomainSettings.MaxDomainAliases,
                                                             defaultDomainSettings.MaxLists,
                                                             defaultDomainSettings.ShowDomainAliasMenu,// ShowDomainAliasMenu
                                                             defaultDomainSettings.ShowContentFilteringMenu,// ShowContentFilteringMenu
                                                             defaultDomainSettings.ShowSpamMenu, // ShowSpamMenu
                                                             defaultDomainSettings.ShowStatsMenu, // ShowStatsMenu
                                                             defaultDomainSettings.RequireSmtpAuthentication,
                                                             defaultDomainSettings.ShowListMenu, // ShowListMenu
                                                             defaultDomainSettings.ListCommandAddress);
                }

                if (!result.Result)
                    throw new Exception(result.Message);
				
                // update additional settings
                result = domains.SetRequestedDomainSettings(AdminUsername, AdminPassword, domain.Name, requestedDomainDefaults);

				
                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create mail domain", ex);
            }
           
        }

        /// <summary>
        /// Updates the settings for the specified domain 
        /// </summary>
        /// <param name="domain">Domain info</param>
        public virtual void UpdateDomain(MailDomain domain)
        {
            try
            {
                // load original domain
                MailDomain origDomain = GetDomain(domain.Name);

                svcDomainAdmin domains = new svcDomainAdmin();
                PrepareProxy(domains);

                GenericResult1 result = domains.UpdateDomain(AdminUsername, AdminPassword,
                    domain.Name,
                    origDomain.ServerIP,
                    domain.ImapPort,
                    domain.PopPort,
                    domain.SmtpPort,
					domain.MaxAliases,
                    domain.MaxDomainSizeInMB,
					domain.MaxDomainUsers,
                    domain.MaxMailboxSizeInMB,
					domain.MaxMessageSize,
					domain.MaxRecipients,
					domain.MaxDomainAliases,
					domain.MaxLists,
                    domain.ShowDomainAliasMenu, // ShowDomainAliasMenu
                    domain.ShowContentFilteringMenu, // ShowContentFilteringMenu
                    domain.ShowSpamMenu, // ShowSpamMenu
                    domain.ShowsStatsMenu, // ShowStatsMenu
                    origDomain.RequireSmtpAuthentication,
                    domain.ShowListMenu, // ShowListMenu
                    origDomain.ListCommandAddress);

                if (!result.Result)
                    throw new Exception(result.Message);

                // update catch-all group
                UpdateDomainCatchAllGroup(domain.Name, domain.CatchAllAccount);

                // update additional settings
                result = domains.SetRequestedDomainSettings(AdminUsername, AdminPassword, domain.Name,
                    new string[] {
                        "isenabled=" + domain.Enabled,
                        "catchall=" + (!String.IsNullOrEmpty(domain.CatchAllAccount) ? SYSTEM_CATCH_ALL : ""),
                        "altsmtpport=" + domain.SmtpPortAlt,
                        "ldapport=" + domain.LdapPort,
                        "sharedcalendar=" + domain.SharedCalendars,
                        "sharedcontact=" + domain.SharedContacts,
                        "sharedfolder=" + domain.SharedFolders,
                        "sharednotes=" + domain.SharedNotes,
                        "sharedtasks=" + domain.SharedTasks,
                        "sharedgal=" + domain.IsGlobalAddressList,
                        "bypassforwardblacklist=" + domain.BypassForwardBlackList,
				});
				

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update mail domain", ex);
            }
        }

        /// <summary>
        /// Deletes the specified domain
        /// </summary>
        /// <param name="domainName"></param>
        public virtual void DeleteDomain(string domainName)
        {
            try
            {
                svcDomainAdmin domains = new svcDomainAdmin();
                PrepareProxy(domains);

                GenericResult1 result = domains.DeleteDomain(AdminUsername, AdminPassword,
                    domainName,
                    true // delete files
                    );

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete mail domain", ex);
            }
        }

        private void UpdateDomainCatchAllGroup(string domainName, string mailboxName)
        {
            // check if system catch all group exists
            string groupName = SYSTEM_CATCH_ALL + "@" + domainName;
            if (GroupExists(groupName))
            {
                // delete group
                DeleteGroup(groupName);
            }

            if (!String.IsNullOrEmpty(mailboxName))
            {
                // create catch-all group
                MailGroup group = new MailGroup();
                group.Name = groupName;
                group.Enabled = true;
                group.Members = new string[] { mailboxName + "@" + domainName };

                // create
                CreateGroup(group);
            }
        }

        #endregion

        #region Domain aliases

        public virtual bool DomainAliasExists(string domainName, string aliasName)
        {
            try
            {
                string[] aliases = GetDomainAliases(domainName);
                foreach (string alias in aliases)
                {
                    if (String.Compare(alias, aliasName, true) == 0)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not check whether mail domain alias exists", ex);
            }
        }

        public virtual void AddDomainAlias(string domainName, string aliasName)
        {
            try
            {
                svcDomainAliasAdmin aliases = new svcDomainAliasAdmin();
                PrepareProxy(aliases);

                GenericResult1 result = aliases.AddDomainAlias(AdminUsername, AdminPassword,
                    domainName, aliasName);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add mail domain alias", ex);
            }
        }

        public virtual void DeleteDomainAlias(string domainName, string aliasName)
        {
            try
            {
                svcDomainAliasAdmin aliases = new svcDomainAliasAdmin();
                PrepareProxy(aliases);

                GenericResult1 result = aliases.DeleteDomainAlias(AdminUsername, AdminPassword,
                    domainName, aliasName);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete mail domain alias", ex);
            }
        }

        /// <summary>
        /// Returns all domain aliases that belong to the specified domain
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <returns>Array with domain names</returns>
        public virtual string[] GetDomainAliases(string domainName)
        {
            try
            {
                svcDomainAliasAdmin aliases = new svcDomainAliasAdmin();
                PrepareProxy(aliases);

                DomainAliasInfoListResult result = aliases.GetAliases(AdminUsername, AdminPassword, domainName);

                if (!result.Result)
                    throw new Exception(result.Message);

                return result.DomainAliasNames;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get the list of mail domain aliases", ex);
            }
        }

        #endregion

        #region Domain Groups (Aliases)

        public virtual bool GroupExists(string groupName)
        {
            try
            {
                svcAliasAdmin svcGroups = new svcAliasAdmin();
                PrepareProxy(svcGroups);

                AliasInfoResult result = svcGroups.GetAlias(AdminUsername, AdminPassword,
                    GetDomainName(groupName), groupName);

                return (result.Result
                    && result.AliasInfo.Name != "Empty");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not check whether mail domain group exists", ex);
            }
        }

        public virtual MailGroup[] GetGroups(string domainName)
        {
            try
            {
                svcAliasAdmin svcGroups = new svcAliasAdmin();
                PrepareProxy(svcGroups);

                AliasInfoListResult result = svcGroups.GetAliases(AdminUsername, AdminPassword, domainName);

                if (!result.Result)
                    throw new Exception(result.Message);

                MailGroup[] groups = new MailGroup[result.AliasInfos.Length];
                for (int i = 0; i < groups.Length; i++)
                {
                    groups[i] = new MailGroup();
                    groups[i].Name = result.AliasInfos[i].Name + "@" + domainName;
                    groups[i].Members = result.AliasInfos[i].Addresses;
                    groups[i].Enabled = true; // by default
                }

                return groups;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get the list of mail domain groups", ex);
            }
        }

        public virtual MailGroup GetGroup(string groupName)
        {
            try
            {
                svcAliasAdmin svcGroups = new svcAliasAdmin();
                PrepareProxy(svcGroups);

                AliasInfoResult result = svcGroups.GetAlias(AdminUsername, AdminPassword,
                    GetDomainName(groupName), groupName);

                if (!result.Result)
                    throw new Exception(result.Message);

                MailGroup group = new MailGroup();
                group.Name = groupName;
                group.Members = result.AliasInfo.Addresses;
                group.Enabled = true; // by default
                return group;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get mail domain group", ex);
            }
        }


        public virtual void CreateGroup(MailGroup group)
        {
            try
            {
                svcAliasAdmin svcGroups = new svcAliasAdmin();
                PrepareProxy(svcGroups);

                GenericResult1 result = svcGroups.AddAlias(AdminUsername, AdminPassword,
                    GetDomainName(group.Name), group.Name, group.Members);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create mail domain group", ex);
            }
        }

        public virtual void UpdateGroup(MailGroup group)
        {
            try
            {
                svcAliasAdmin svcGroups = new svcAliasAdmin();
                PrepareProxy(svcGroups);

                GenericResult1 result = svcGroups.UpdateAlias(AdminUsername, AdminPassword,
                    GetDomainName(group.Name), group.Name, group.Members);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update mail domain group", ex);
            }
        }

        public virtual void DeleteGroup(string groupName)
        {
            try
            {
                svcAliasAdmin svcGroups = new svcAliasAdmin();
                PrepareProxy(svcGroups);

                GenericResult1 result = svcGroups.DeleteAlias(AdminUsername, AdminPassword,
                    GetDomainName(groupName), groupName);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete mail domain group", ex);
            }
        }

        #endregion

        #region Mailboxes

        public virtual bool AccountExists(string mailboxName)
        {
            try
            {
                svcUserAdmin users = new svcUserAdmin();
                PrepareProxy(users);

                UserInfoResult result = users.GetUser(AdminUsername, AdminPassword, mailboxName);

                return result.Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not check whether mailbox exists", ex);
            }
        }

        /// <summary>
        /// Returns all users that belong to the specified domain
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <returns>Array with user names</returns>
        public virtual MailAccount[] GetAccounts(string domainName)
        {
            try
            {
                svcUserAdmin users = new svcUserAdmin();
                PrepareProxy(users);

                UserInfoListResult result = users.GetUsers(AdminUsername, AdminPassword, domainName);

                if (!result.Result)
                    throw new Exception(result.Message);

				List<MailAccount> accounts = new List<MailAccount>();
                
                
                foreach (UserInfo user in result.Users)
                {
                    if (user.IsDomainAdmin && !ImportDomainAdmin)
                        continue;
                                        
                    MailAccount account = new MailAccount();
					account.Name = user.UserName;
					account.Password = user.Password;
					accounts.Add(account);
                }
				return accounts.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get the list of domain mailboxes", ex);
            }
        }

        public virtual MailAccount GetAccount(string mailboxName)
        {
            try
            {
                svcUserAdmin users = new svcUserAdmin();
                PrepareProxy(users);

                UserInfoResult result = users.GetUser(AdminUsername, AdminPassword, mailboxName);

                if (!result.Result)
                    throw new Exception(result.Message);

                MailAccount mailbox = new MailAccount();
                mailbox.Name = result.UserInfo.UserName;
                mailbox.Password = result.UserInfo.Password;
                mailbox.FirstName = result.UserInfo.FirstName;
                mailbox.LastName = result.UserInfo.LastName;
                mailbox.IsDomainAdmin = result.UserInfo.IsDomainAdmin;

                // get additional settings
                string[] requestedSettings = new string[]
                {
                    "isenabled",
                    "maxsize",
                    "passwordlocked",
                    "replytoaddress",
                    "signature"
                };

                SettingsRequestResult addResult = users.GetRequestedUserSettings(AdminUsername, AdminPassword,
                    mailboxName, requestedSettings);

                if (!addResult.Result)
                    throw new Exception(addResult.Message);

                foreach (string pair in addResult.settingValues)
                {
                    string[] parts = pair.Split('=');
                    if (parts[0] == "isenabled") mailbox.Enabled = Boolean.Parse(parts[1]);
                    else if (parts[0] == "maxsize") mailbox.MaxMailboxSize = Int32.Parse(parts[1]);
                    else if (parts[0] == "passwordlocked") mailbox.PasswordLocked = Boolean.Parse(parts[1]);
                    else if (parts[0] == "replytoaddress") mailbox.ReplyTo = parts[1];
                    else if (parts[0] == "signature") mailbox.Signature = parts[1];
                }

                // get forwardings info
                UserForwardingInfoResult forwResult = users.GetUserForwardingInfo(AdminUsername, AdminPassword, mailboxName);

                if (!forwResult.Result)
                    throw new Exception(forwResult.Message);

                string[] forwAddresses = forwResult.ForwardingAddress.Split(';', ',');
                List<string> listForAddresses = new List<string>();
                foreach (string forwAddress in forwAddresses)
                {
                    if (!String.IsNullOrEmpty(forwAddress.Trim()))
                        listForAddresses.Add(forwAddress.Trim());
                }

                mailbox.ForwardingAddresses = listForAddresses.ToArray();
                mailbox.DeleteOnForward = forwResult.DeleteOnForward;

                // get autoresponder info
                UserAutoResponseResult respResult = users.GetUserAutoResponseInfo(AdminUsername, AdminPassword, mailboxName);

                if (!respResult.Result)
                    throw new Exception(respResult.Message);

                mailbox.ResponderEnabled = respResult.Enabled;
                mailbox.ResponderSubject = respResult.Subject;
                mailbox.ResponderMessage = respResult.Body;

                return mailbox;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get mailbox", ex);
            }
        }

        public virtual void CreateAccount(MailAccount mailbox)
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
                    false // domain admin is false
                    );

                if (!result.Result)
                    throw new Exception(result.Message);

                // set additional settings
                result = users.SetRequestedUserSettings(AdminUsername, AdminPassword,
                    mailbox.Name,
                    new string[]
                    {
                        "isenabled=" + mailbox.Enabled,
                        "maxsize=" + (mailbox.MaxMailboxSize),
                        "passwordlocked=" + mailbox.PasswordLocked,
                        "replytoaddress=" + (mailbox.ReplyTo ?? ""),
                        "signature=" + (mailbox.Signature ?? "")
                    });

                if (!result.Result)
                    throw new Exception(result.Message);

                // set forwarding settings
                result = users.UpdateUserForwardingInfo(AdminUsername, AdminPassword,
                    mailbox.Name, mailbox.DeleteOnForward,
                    (mailbox.ForwardingAddresses != null ? String.Join(", ", mailbox.ForwardingAddresses) : ""));

                if (!result.Result)
                    throw new Exception(result.Message);

                // set autoresponder settings
                result = users.UpdateUserAutoResponseInfo(AdminUsername, AdminPassword,
                    mailbox.Name,
                    mailbox.ResponderEnabled,
                    (mailbox.ResponderSubject ?? ""),
                    (mailbox.ResponderMessage ?? ""));

                if (!result.Result)
                    throw new Exception(result.Message);

            }
            catch (Exception ex)
            {
                throw new Exception("Could not create mailbox", ex);
            }
        }

        public virtual void UpdateAccount(MailAccount mailbox)
        {
            try
            {
                //get original account
                MailAccount account = GetAccount(mailbox.Name);

                svcUserAdmin users = new svcUserAdmin();
                PrepareProxy(users);

                GenericResult1 result = users.UpdateUser(AdminUsername, AdminPassword,
                    mailbox.Name,
                    mailbox.Password,
                    mailbox.FirstName,
                    mailbox.LastName,
                    account.IsDomainAdmin
                    );

                if (!result.Result)
                    throw new Exception(result.Message);

                // set additional settings
                result = users.SetRequestedUserSettings(AdminUsername, AdminPassword,
                    mailbox.Name,
                    new string[]
                    {
                        "isenabled=" + mailbox.Enabled,
                        "maxsize=" + (mailbox.MaxMailboxSize),
                        "passwordlocked=" + mailbox.PasswordLocked,
                        "replytoaddress=" + (mailbox.ReplyTo ?? ""),
                        "signature=" + (mailbox.Signature ?? "")
                    });

                if (!result.Result)
                    throw new Exception(result.Message);

                // set forwarding settings
                result = users.UpdateUserForwardingInfo(AdminUsername, AdminPassword,
                    mailbox.Name, mailbox.DeleteOnForward,
                    (mailbox.ForwardingAddresses != null ? String.Join(", ", mailbox.ForwardingAddresses) : ""));

                if (!result.Result)
                    throw new Exception(result.Message);

                // set autoresponder settings
                result = users.UpdateUserAutoResponseInfo(AdminUsername, AdminPassword,
                    mailbox.Name,
                    mailbox.ResponderEnabled,
                    (mailbox.ResponderSubject ?? ""),
                    (mailbox.ResponderMessage ?? ""));

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update mailbox", ex);
            }
        }

        public virtual void DeleteAccount(string mailboxName)
        {
            try
            {
                svcUserAdmin users = new svcUserAdmin();
                PrepareProxy(users);

                GenericResult1 result = users.DeleteUser(AdminUsername, AdminPassword,
                    mailboxName, GetDomainName(mailboxName));

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete mailbox", ex);
            }
        }

#endregion 

        #region Mail Aliases

        public bool MailAliasExists(string mailAliasName)
        {
            try
            {
                svcAliasAdmin aliases = new svcAliasAdmin();
                PrepareProxy(aliases);

                AliasInfoResult result = aliases.GetAlias(AdminUsername, AdminPassword, GetDomainName(mailAliasName), mailAliasName);

                if ((result.AliasInfo.Name.Equals("Empty")) && (result.AliasInfo.Addresses.Length == 0))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not check whether mail alias exists", ex);
            }
        }

        public MailAlias[] GetMailAliases(string domainName)
        {
            try
            {

                svcAliasAdmin aliases = new svcAliasAdmin();
                PrepareProxy(aliases);

                AliasInfoListResult result = aliases.GetAliases(AdminUsername, AdminPassword, domainName);

                if (!result.Result)
                    throw new Exception(result.Message);

                List<MailAlias> aliasesList = new List<MailAlias>();


                foreach (AliasInfo alias in result.AliasInfos)
                {
                    if (alias.Addresses.Length == 1)
                    {
                        MailAlias mailAlias = new MailAlias();
                        mailAlias.Name = alias.Name + "@" + domainName;
                        mailAlias.ForwardTo = alias.Addresses[0];
                        aliasesList.Add(mailAlias);
                    }
                }
                return aliasesList.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get the list of mail aliases", ex);
            }
        }

        public MailAlias GetMailAlias(string mailAliasName)
        {
            svcAliasAdmin aliases = new svcAliasAdmin();
            PrepareProxy(aliases);


            MailAlias alias = new MailAlias();
            MailAlias newAlias = new MailAlias();

            //convert old alliases created as mailboxes
            if (!MailAliasExists(mailAliasName))
            {
                MailAccount account = GetAccount(mailAliasName);
                newAlias.Name = account.Name;
                newAlias.ForwardTo = account.ForwardingAddresses[0];
                DeleteAccount(mailAliasName);
                CreateMailAlias(newAlias);
                return newAlias;
            }

            AliasInfoResult result = aliases.GetAlias(AdminUsername, AdminPassword, GetDomainName(mailAliasName), mailAliasName);
            alias.Name = result.AliasInfo.Name;
            alias.ForwardTo = result.AliasInfo.Addresses[0];
            return alias;
        }

        public void CreateMailAlias(MailAlias mailAlias)
        {
            try
            {
                svcAliasAdmin aliases = new svcAliasAdmin();
                PrepareProxy(aliases);

                GenericResult1 result = aliases.AddAlias(AdminUsername, AdminPassword,
                                                        GetDomainName(mailAlias.Name), mailAlias.Name,
                                                        new string[] { mailAlias.ForwardTo });


                if (!result.Result)
                    throw new Exception(result.Message);
            }

            catch (Exception ex)
            {
                if (MailAliasExists(mailAlias.Name))
                {
                    DeleteMailAlias(mailAlias.Name);
                }
                Log.WriteError(ex);
                throw new Exception("Could not create mail alias", ex);

            }

        }

        public void UpdateMailAlias(MailAlias mailAlias)
        {
            try
            {
                svcAliasAdmin aliases = new svcAliasAdmin();
                PrepareProxy(aliases);

                GenericResult1 result = aliases.UpdateAlias(AdminUsername, AdminPassword, GetDomainName(mailAlias.Name),
                                                             mailAlias.Name,
                                                             new string[] { mailAlias.ForwardTo });

                if (!result.Result)
                    throw new Exception(result.Message);

            }
            catch (Exception ex)
            {
                throw new Exception("Could not update mailAlias", ex);
            }

        }

        public void DeleteMailAlias(string mailAliasName)
        {
            try
            {
                svcAliasAdmin aliases = new svcAliasAdmin();
                PrepareProxy(aliases);

                GenericResult1 result = aliases.DeleteAlias(AdminUsername, AdminPassword, GetDomainName(mailAliasName),
                    mailAliasName);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete mailAlias", ex);
            }
        }
        #endregion

        #region Mailing lists

        public virtual bool ListExists(string listName)
        {
            try
            {
                SolidCPMailListAdmin svcLists = new SolidCPMailListAdmin();
                PrepareProxy(svcLists);

                GenericResult result = svcLists.MailingListExists(AdminUsername, AdminPassword, listName);

                return result.Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not check whether mailing list exists", ex);
            }
        }

        public virtual MailList[] GetLists(string domainName)
        {
            try
            {
                SolidCPMailListAdmin svcLists = new SolidCPMailListAdmin();
                PrepareProxy(svcLists);

                MailingListsResult result = svcLists.GetMailingLists(AdminUsername, AdminPassword, domainName);

                if (!result.Result)
                    throw new Exception(result.Message);

                List<MailList> items = new List<MailList>();
                foreach (MailingListInfo listInfo in result.MailingLists)
                {
                    MailList item = new MailList();
                    item.Name = listInfo.Name;
                    item.Description = listInfo.Description;
                }

                return items.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get mail list", ex);
            }
        }

        public virtual MailList GetList(string listName)
        {
            try
            {
                SolidCPMailListAdmin svcLists = new SolidCPMailListAdmin();
                PrepareProxy(svcLists);

                MailingListResult result = svcLists.GetMailingList(AdminUsername, AdminPassword, listName);

                if (!result.Result)
                    throw new Exception(result.Message);

                MailList item = new MailList();
                item.Description = result.MailingList.Description;
                item.EnableSubjectPrefix = result.MailingList.EnableSubjectPrefix;
                item.SubjectPrefix = result.MailingList.SubjectPrefix;
                item.Enabled = true;
                item.MaxMessageSize = result.MailingList.MaxMessageSize;
                item.MaxRecipientsPerMessage = result.MailingList.MaxRecipientsPerMessage;
                item.Members = result.MailingList.Members ?? new string[] { };
                item.Moderated = !String.IsNullOrEmpty(result.MailingList.ModeratorAddress);
                item.ModeratorAddress = result.MailingList.ModeratorAddress;
                item.Name = result.MailingList.Name;
                item.Password = result.MailingList.Password;
                item.RequirePassword = result.MailingList.RequirePassword;

                // post mode
                PostingMode postMode = PostingMode.AnyoneCanPost;
                if (result.MailingList.PostingMode == MailListPostOptions.ModeratorOnly)
                    postMode = PostingMode.ModeratorCanPost;
                else if (result.MailingList.PostingMode == MailListPostOptions.SubscribersOnly)
                    postMode = PostingMode.MembersCanPost;
                item.PostingMode = postMode;
                item.ReplyToMode = result.MailingList.ReplyToList ? ReplyTo.RepliesToList : ReplyTo.RepliesToSender;

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get mail list", ex);
            }
        }

        public virtual void CreateList(MailList list)
        {
            try
            {
                SolidCPMailListAdmin svcLists = new SolidCPMailListAdmin();
                PrepareProxy(svcLists);

                MailListPostOptions postMode = MailListPostOptions.Anyone;
                if (list.PostingMode == PostingMode.MembersCanPost)
                    postMode = MailListPostOptions.SubscribersOnly;
                if (list.PostingMode == PostingMode.ModeratorCanPost)
                    postMode = MailListPostOptions.ModeratorOnly;

                GenericResult result = svcLists.AddMailingList(AdminUsername, AdminPassword,
                    GetDomainName(list.Name),
                    GetAccountName(list.Name),
                    list.ModeratorAddress,
                    list.Description,
                    list.MaxMessageSize,
                    list.MaxRecipientsPerMessage,
                    list.EnableSubjectPrefix,
                    list.SubjectPrefix,
                    list.Members,
                    postMode,
                    (list.ReplyToMode == ReplyTo.RepliesToList),
                    list.Password,
                    list.RequirePassword);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create mail list", ex);
            }
        }

        public virtual void UpdateList(MailList list)
        {
            try
            {
                SolidCPMailListAdmin svcLists = new SolidCPMailListAdmin();
                PrepareProxy(svcLists);

                MailListPostOptions postMode = MailListPostOptions.Anyone;
                if (list.PostingMode == PostingMode.MembersCanPost)
                    postMode = MailListPostOptions.SubscribersOnly;
                if (list.PostingMode == PostingMode.ModeratorCanPost)
                    postMode = MailListPostOptions.ModeratorOnly;

                GenericResult result = svcLists.UpdateMailingList(AdminUsername, AdminPassword,
                    list.Name,
                    list.ModeratorAddress,
                    list.Description,
                    list.MaxMessageSize,
                    list.MaxRecipientsPerMessage,
                    list.EnableSubjectPrefix,
                    list.SubjectPrefix,
                    list.Members,
                    postMode,
                    (list.ReplyToMode == ReplyTo.RepliesToList),
                    list.Password,
                    list.RequirePassword);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update mail list", ex);
            }
        }


        /// <summary>
        /// Deletes specified mail list.
        /// </summary>
        /// <param name="listName">Mail list name.</param>
        public virtual void DeleteList(string listName)
        {
            try
            {
                SolidCPMailListAdmin svcLists = new SolidCPMailListAdmin();
                PrepareProxy(svcLists);

                GenericResult result = svcLists.DeleteMailingList(AdminUsername, AdminPassword, listName);

                if (!result.Result)
                    throw new Exception(result.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete mail list", ex);
            }
        }
        #endregion

        #region IHostingServiceProvier methods

        public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is MailDomain)
                {
                    try
                    {
                        // enable/disable mail domain
                        if (DomainExists(item.Name))
                        {
                            MailDomain mailDomain = GetDomain(item.Name);
                            mailDomain.Enabled = enabled;
                            UpdateDomain(mailDomain);
                        }
                    }
                    catch(Exception ex)
                    {
                        Log.WriteError(String.Format("Error switching '{0}' SmarterMail domain", item.Name), ex);
                    }
                }
            }
        }

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
                        Log.WriteError(String.Format("Error deleting '{0}' SmarterMail domain", item.Name), ex);
                    }
                }
            }
        }

        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();

            // update items with diskspace
            foreach (ServiceProviderItem item in items)
            {
                if (item is MailAccount)
                {
                    try
                    {

                        // get mailbox size
                        string name = item.Name;

                        // try to get SmarterMail postoffices path
                        string poPath = DomainsPath;
                        if (poPath == null)
                            continue;

                        string mailboxName = name.Substring(0, name.IndexOf("@"));
                        string domainName = name.Substring(name.IndexOf("@") + 1);

                        string mailboxPath = Path.Combine(DomainsPath, String.Format("{0}\\Users\\{1}", domainName, mailboxName));

                        Log.WriteStart(String.Format("Calculating '{0}' folder size", mailboxPath));

                        // calculate disk space
                        ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
                        diskspace.ItemId = item.Id;
                        //diskspace.DiskSpace = 0;
                        diskspace.DiskSpace = FileUtils.CalculateFolderSize(mailboxPath);
                        itemsDiskspace.Add(diskspace);
                        Log.WriteEnd(String.Format("Calculating '{0}' folder size", mailboxPath));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }
            return itemsDiskspace.ToArray();
        }

        public override ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since)
        {
            ServiceProviderItemBandwidth[] itemsBandwidth = new ServiceProviderItemBandwidth[items.Length];

            // update items with diskspace
            for (int i = 0; i < items.Length; i++)
            {
                ServiceProviderItem item = items[i];

                // create new bandwidth object
                itemsBandwidth[i] = new ServiceProviderItemBandwidth();
                itemsBandwidth[i].ItemId = item.Id;
                itemsBandwidth[i].Days = new DailyStatistics[0];

                if (item is MailDomain)
                {
                    try
                    {
                        // get daily statistics
                        itemsBandwidth[i].Days = GetDailyStatistics(since, item.Name);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            }
             
            return itemsBandwidth;
        }

        public DailyStatistics[] GetDailyStatistics(DateTime since, string maildomainName)
        {

            ArrayList days = new ArrayList();
            // read statistics
            DateTime now = DateTime.Now;
            DateTime date = since;

            try
            {
                while (date < now)
                {
                    svcDomainAdmin domains = new svcDomainAdmin();
                    PrepareProxy(domains);
                    StatInfoResult result =
                        domains.GetDomainStatistics(AdminUsername, AdminPassword, maildomainName, date, date);

                    if (!result.Result)
                        throw new Exception(result.Message);

                    if (result.BytesReceived != 0 | result.BytesSent != 0)
                    {
                        DailyStatistics dailyStats = new DailyStatistics();
                        dailyStats.Year = date.Year;
                        dailyStats.Month = date.Month;
                        dailyStats.Day = date.Day;
                        dailyStats.BytesSent = result.BytesSent;
                        dailyStats.BytesReceived = result.BytesReceived;
                        days.Add(dailyStats);
                    }
                    
                    // advance day
                    date = date.AddDays(1);
                }
            }
            catch(Exception ex)
            {
                Log.WriteError("Could not get SmarterMail domain statistics", ex);
            }
            return (DailyStatistics[])days.ToArray(typeof(DailyStatistics));
        }

        #endregion

        #region Helper Members

		
        protected void PrepareProxy(SoapHttpClientProtocol proxy)
        {
            string smarterUrl = ServiceUrl;

            int idx = proxy.Url.LastIndexOf("/");

            // strip the last slash if any
            if (smarterUrl[smarterUrl.Length - 1] == '/')
                smarterUrl = smarterUrl.Substring(0, smarterUrl.Length - 1);

            proxy.Url = smarterUrl + proxy.Url.Substring(idx);
        }

		
        protected string GetDomainName(string email)
        {
            return email.Substring(email.IndexOf('@') + 1);
        }

        protected string GetAccountName(string email)
        {
            return email.Substring(0, email.IndexOf('@'));
        }

        #endregion

    
       ///<summary>
        ///
        /// Checks whether service is installed within the system. This method will be
        /// used by server creation wizard for automatic services detection and configuring.
        /// 
        ///</summary>
        ///
        ///<returns>
        ///True if service is installed; otherwise - false.
        ///</returns>
        ///
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
                    return split[0].Equals("2");
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
                    return split[0].Equals("2");
                }
            }
            return false;
        }

    }

    
}
