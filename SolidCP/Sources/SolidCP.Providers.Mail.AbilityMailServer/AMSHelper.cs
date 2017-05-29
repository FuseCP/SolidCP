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
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SolidCP.Server.Utils;
using Microsoft.Win32;

namespace SolidCP.Providers.Mail
{
	public class AMSHelper
	{
	   
	    public static string MailListsConfigFile
	    {
            get { return @"mailinglists.ini"; }
	    }
	    
        public static string DomainsConfigFile
	    {
            get { return @"domains.ini"; }

	    } 
        
	    public static string UsersConfigFile
	    {
            get { return @"users.ini"; }
	    }
        
	    public static string AccountConfigFile
        {
            get { return @"{0}\{1}\config.ini"; }
        }

	    public static string AccountDeliveryFile
	    {
            get { return @"{0}\{1}\delivery.ini"; }
	    }

	    public static string MailListConfigFile
	    {
            get { return @"{0}\{1}.ini"; }
	    }
	    
        public static string AccountFolder 
        {
            get { return @"{0}\{1}\"; }
        }

	    public static string DomainFolder
	    {
	        get {return @"{0}\"; }
	    }

	    public static string GroupsConfigFile
	    {
	        get
	        {
	            return @"config\accounts\groups.ini";
	        }
	    }

	    public static string AMSLocation
		{
			get
			{
				string serverLocation = null;

				RegistryKey HKLM = Registry.LocalMachine;

				RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Ability Mail Server 2_is1");

				if (key != null)
					serverLocation = (string)key.GetValue("InstallLocation");

                string amsMailConfig = serverLocation + @"config\mailserver.ini";
                string[] lines = File.ReadAllLines(amsMailConfig);
			    
                try
                {
                    foreach (string s in lines)
                    {
                        if (s.StartsWith("accountpath"))
                        {
                            string[] split = s.Split(new char[] { '=' });
                            if (Path.IsPathRooted(split[1]))
                            {
                                if (String.Equals(split[1], "\\config\\accounts\\"))
                                {
                                    if (serverLocation != null)
                                        return Path.Combine(serverLocation, "config\\accounts\\");
                                }
                                    serverLocation = split[1];
                                    DirectoryInfo Location = new DirectoryInfo(split[1]);
                                    if (!Location.Exists)
                                        Location.Create();
                                    string groupConfPath = Path.Combine(split[1], @"\groups.ini");
                                    if (!File.Exists(groupConfPath))
                                        if (key != null)
                                            File.Copy(
                                                Path.Combine((string) key.GetValue("InstallLocation"), GroupsConfigFile),
                                                groupConfPath);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex);
                }
			    return serverLocation;
			}
		}

        
       public static Tree GetDomainsConfig()
		{
			return ReadAmsConfigurationFile(DomainsConfigFile);
		}

		public static bool SetDomainsConfig(Tree config)
		{
			return SaveAmsConfigurationFile(config, DomainsConfigFile);
		}

		public static Tree GetUsersConfig()
		{
			return ReadAmsConfigurationFile(UsersConfigFile);
		}

		public static bool SetUsersConfig(Tree config)
		{
			return SaveAmsConfigurationFile(config, UsersConfigFile);
		}

		public static Tree GetAccountConfig(string mailboxName)
		{
			string account = AmsMailbox.GetAccountName(mailboxName);
			string domain = AmsMailbox.GetDomainName(mailboxName);
			string accountConfig = string.Format(AccountConfigFile, domain, account);

			return ReadAmsConfigurationFile(accountConfig);
		}

		public static bool SetAccountConfig(Tree config, string mailboxName)
		{
			string account = AmsMailbox.GetAccountName(mailboxName);
			string domain = AmsMailbox.GetDomainName(mailboxName);
			string accountConfig = string.Format(AccountConfigFile, domain, account);

			return SaveAmsConfigurationFile(config, accountConfig);
		}

		public static Tree GetMailListsConfig()
		{
			return ReadAmsConfigurationFile(MailListsConfigFile);
		}

		public static bool SetMailListsConfig(Tree config)
		{
			return SaveAmsConfigurationFile(config, MailListsConfigFile);
		}

		public static Tree GetMailingListConfig(string maillistName)
		{
			string domain = AmsMailbox.GetDomainName(maillistName);
			string account = AmsMailbox.GetAccountName(maillistName);
			string listConfig = Path.Combine(AMSLocation, string.Format(MailListConfigFile, domain, account));

			return ReadAmsConfigurationFile(listConfig);
		}

		public static bool SetMailingListConfig(Tree config, string maillistName)
		{
			string domain = AmsMailbox.GetDomainName(maillistName);
			string account = AmsMailbox.GetAccountName(maillistName);
			string listConfig = Path.Combine(AMSLocation, string.Format(MailListConfigFile, domain, account));

			return SaveAmsConfigurationFile(config, listConfig);
		}

		public static Tree GetAccountDelivery(string mailboxName)
		{
			string account = AmsMailbox.GetAccountName(mailboxName);
			string domain = AmsMailbox.GetDomainName(mailboxName);
			string deliveryConfig = string.Format(AccountDeliveryFile, domain, account);

			return ReadAmsConfigurationFile(deliveryConfig);
		}

		public static bool SetAccountDelivery(Tree config, string mailboxName)
		{
			string account = AmsMailbox.GetAccountName(mailboxName);
			string domain = AmsMailbox.GetDomainName(mailboxName);
			string deliveryConfig = string.Format(AccountDeliveryFile, domain, account);

			return SaveAmsConfigurationFile(config, deliveryConfig);
		}

		public static bool RemoveAccount(string mailboxName)
		{
			bool succeed = false;
			string account = AmsMailbox.GetAccountName(mailboxName);
			string domain = AmsMailbox.GetDomainName(mailboxName);
			string accountDir = Path.Combine(AMSLocation, string.Format(AccountFolder, domain, account));

			try
			{
				Directory.Delete(accountDir, true);
				succeed = true;
			}
			catch(Exception ex)
			{
                Log.WriteError(ex);
			}

			return succeed;
		}

		public static bool RemoveDomain(string domainName)
		{
			bool succeed = false;
			string domainFolder = Path.Combine(AMSLocation, string.Format(DomainFolder, domainName));

			try
			{
				Directory.Delete(domainFolder, true);
				succeed = true;
			}
			catch(Exception ex)
			{
                Log.WriteError(ex.Message, ex);
			}

			return succeed;
		}

		public static bool RemoveMailList(string maillistName)
		{
			bool succeed = false;
			string account = AmsMailbox.GetAccountName(maillistName);
			string domain = AmsMailbox.GetDomainName(maillistName);
			string maillistConfig = Path.Combine(AMSLocation, string.Format(MailListConfigFile, domain, account));

			try
			{
				File.Delete(maillistConfig);
				succeed = true;
			}
			catch(Exception ex)
			{
                Log.WriteError(ex.Message, ex);
			}

			return succeed;
		}

		protected static Tree ReadAmsConfigurationFile(string configFile)
		{
			Tree configTree = new Tree();

			try
			{
				string amsConfig = Path.Combine(AMSLocation, configFile);

                if (!File.Exists(amsConfig))
                {
                    FileStream stream = File.Create(amsConfig);
                    stream.Close();
                    stream.Dispose();
                    return configTree;
                }
                
			    string[] lines = File.ReadAllLines(amsConfig);

				TreeNode parent = null;

				foreach(string lineStr in lines)
				{
					bool isLeaf = false;
					string line = lineStr.Trim();
					TreeNode node = new TreeNode(parent);

					if (line.StartsWith("}"))
					{
					    if (parent != null) parent = parent.Parent;
					    continue;
					}

				    // fill node
					if (line.StartsWith("{"))
					{
						line = line.Replace("{", string.Empty).Trim();
						node.NodeName = line;
						isLeaf = true;
					}
					else
					{
						int splitIndex = line.IndexOf("=");
						node.NodeName = line.Substring(0, splitIndex);
						node.NodeValue = line.Substring(splitIndex + 1);
					}

					// add node
					if (parent != null)
						parent.ChildNodes.Add(node);
					else
						configTree.ChildNodes.Add(node);

					parent = isLeaf ? node : parent;
				}
			}
			catch (Exception ex)
			{
                Log.WriteError(ex);
                throw ex;
			}

			return configTree;
		}

		public static bool SaveAmsConfigurationFile(Tree config, string configFile)
		{
			string amsConfig = null;
			string amsBackup = null;
			bool succeed = false;

			try
			{
				StringBuilder builder = new StringBuilder();
				config.Serialize(builder);

				amsConfig = Path.Combine(AMSLocation, configFile);
                amsBackup = string.Concat(amsConfig, ".bak");

				string configFolder = Path.GetDirectoryName(amsConfig);

				if (!Directory.Exists(configFolder))
					Directory.CreateDirectory(configFolder);

				// create backup
				if (File.Exists(amsConfig))
					File.Move(amsConfig, amsBackup);
				
				File.WriteAllText(amsConfig, builder.ToString());
				succeed = true;
			}
			catch (Exception ex)
			{
                Log.WriteError(ex);
                throw ex;
			}
			finally
			{
				if (!succeed) // restore backup
				{
					if (File.Exists(amsBackup))
						File.Move(amsBackup, amsConfig);
				}
				else // remove backup
				{
					File.Delete(amsBackup);
				}
			}

			return succeed;
		}
	}
}
