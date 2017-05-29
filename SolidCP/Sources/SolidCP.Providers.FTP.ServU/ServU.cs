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
using System.Xml;
using Microsoft.Win32;

using SolidCP.Providers;
using SolidCP.Providers.FTP;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.FTP
{
    public class ServU : HostingServiceProviderBase, IFtpServer
    {
        #region Constants

        private const string SERVU_PATH_REG = @"SYSTEM\CurrentControlSet\Services\Serv-U";
        private const string SERVU_DAEMON_CONFIG_FILE = "ServUDaemon.ini";

        #endregion

        #region Properties

        protected string SERVU_DOMAINS_REG
        {
            get
            {
                return (IntPtr.Size == 8)
                ? @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\DomainList" // x64
                : @"SOFTWARE\Cat Soft\Serv-U\Domains\DomainList"; // x86
            }
        }

        protected virtual string ServUFolder
        {
            get
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(SERVU_PATH_REG);
                if (key == null)
                    throw new Exception("Serv-U service registry key was not found: " + SERVU_PATH_REG);

                return Path.GetDirectoryName((string)key.GetValue("ImagePath"));
            }
        }

        protected virtual string DomainId
        {
			get { return ProviderSettings["DomainId"]; }
        }
        #endregion

        #region Sites
        
        public virtual FtpSite[] GetSites()
        {
            List<FtpSite> sites = new List<FtpSite>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(SERVU_DOMAINS_REG);

            if (key == null)
                return sites.ToArray();

            foreach (string domainId in key.GetValueNames())
            {
                string[] parts = key.GetValue(domainId).ToString().Split('|');

                FtpSite site = new FtpSite();
                site.SiteId = domainId;
                site.Name = parts[3];
                sites.Add(site);
            }
            return sites.ToArray();
        }

        public virtual void ChangeSiteState(string siteId, ServerState state)
        {
            // not implemented
        }

        public virtual string CreateSite(FtpSite site)
        {
            // not implemented
            return null;
        }

        public virtual void DeleteSite(string siteId)
        {
            // not implemented
        }

        public virtual FtpSite GetSite(string siteId)
        {
            // not implemented
            return null;
        }

        public virtual bool SiteExists(string siteId)
        {
            // not implemented
            return false;
        }

        public virtual ServerState GetSiteState(string siteId)
        {
            // not implemented
            return ServerState.Started;
        }

        public virtual void UpdateSite(FtpSite site)
        {
            // not implemented
        }

        #endregion

        #region Accounts

        public virtual bool AccountExists(string accountName)
        {
            string keyName = @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserList";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName);
			if (key == null)
				return false;
            return (key.GetValue(accountName) != null);
        }

        public virtual FtpAccount[] GetAccounts()
        {
			List<FtpAccount> accounts = new List<FtpAccount>();

            string keyName = @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserList";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName);
            if (key == null)
				return accounts.ToArray();

            foreach (string name in key.GetValueNames())
            {
                string[] parts = key.GetValue(name).ToString().Split('|');

                FtpAccount acc = new FtpAccount();
                acc.Name = name;
                acc.Enabled = (parts[0] == "1");
                accounts.Add(acc);
            }
            return accounts.ToArray();
        }

        public virtual FtpAccount GetAccount(string accountName)
        {
            string keyName = @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserSettings\" + accountName;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName);
            if (key == null)
                return null;

            FtpAccount account = new FtpAccount();
            account.Name = accountName;

            // status
            RegistryKey usersKey = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserList");
            string[] parts = usersKey.GetValue(accountName).ToString().Split('|');
            account.Enabled = (parts[0] == "1");

            // path and permissions
            string path = (string)key.GetValue("Access1");
            if (path != null)
            {
                parts = path.Split('|');
                account.Folder = parts[0];
                account.CanRead = (parts[1].IndexOf("R") != -1);
                account.CanWrite = (parts[1].IndexOf("W") != -1);
            }

            // password
            account.Password = (string)key.GetValue("Password");

            return account;
        }

        public virtual void CreateAccount(FtpAccount account)
        {
            // add user to the list
            if (AccountExists(account.Name))
                return;

			RegistryKey domainKey = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId, true);

			RegistryKey usersKey = domainKey.OpenSubKey("UserList", true);
			if (usersKey == null)
				usersKey = domainKey.CreateSubKey("UserList");

            usersKey.SetValue(account.Name, BoolToString(account.Enabled) + "|0");

            // user details
			RegistryKey settingsKey = domainKey.OpenSubKey("UserSettings", true);
			if(settingsKey == null)
				settingsKey = domainKey.CreateSubKey("UserSettings");
			
			RegistryKey user = settingsKey.CreateSubKey(account.Name);

            // folder and permissions
            user.SetValue("Access1", account.Folder
                + "|" + BuildPermissionsString(account.CanRead, account.CanWrite));

            // enable
            if (!account.Enabled)
                user.SetValue("Enable", "0");

            // home folder
            user.SetValue("HomeDir", account.Folder);

            // password
            user.SetValue("Password", HashPassword(account.Password));
            user.SetValue("PasswordLastChange", "1170889819");

            // other props
            user.SetValue("RelPaths", "1");
            user.SetValue("SKEYValues", "");
            user.SetValue("TimeOut", "600");

            // reload config
            ReloadServUConfig();
        }

        public virtual void UpdateAccount(FtpAccount account)
        {
            // edit status in the list
            RegistryKey usersKey = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserList", true);
            usersKey.SetValue(account.Name, BoolToString(account.Enabled) + "|0");

            // edit details
            RegistryKey user = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserSettings\" + account.Name, true);

            if (user == null)
                return;

            // folder and permissions
            user.SetValue("Access1", account.Folder
                + "|" + BuildPermissionsString(account.CanRead, account.CanWrite));

            if (user.GetValue("Enable") != null)
                user.DeleteValue("Enable");

            // enable
            if (!account.Enabled)
                user.SetValue("Enable", "0");

            // home folder
            user.SetValue("HomeDir", account.Folder);

            // password
            if (!String.IsNullOrEmpty(account.Password))
            {
                user.SetValue("Password", HashPassword(account.Password));
                user.SetValue("PasswordLastChange", "1170889819");
            }

            // reload config
            ReloadServUConfig();
        }

        public virtual void DeleteAccount(string accountName)
        {
            // remove settings
            RegistryKey key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserSettings", true);
            key.DeleteSubKey(accountName);

            // remove from the list
            RegistryKey usersKey = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Wow6432Node\Cat Soft\Serv-U\Domains\" + DomainId + @"\UserList", true);
            usersKey.DeleteValue(accountName);

            // reload config
            ReloadServUConfig();
        }

        #endregion

		public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is FtpAccount)
				{
					try
					{
						// change FTP account state
						FtpAccount account = GetAccount(item.Name);
						account.Enabled = enabled;
						UpdateAccount(account);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error switching '{0}' {1}", item.Name, item.GetType().Name), ex);
					}
				}
			}
		}

		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is FtpAccount)
				{
					try
					{
						// delete FTP account
						DeleteAccount(item.Name);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
					}
				}
			}
		}

        #region Private Helpers
        private string BoolToString(bool val)
        {
            return val ? "1" : "0";
        }

        private string BuildPermissionsString(bool read, bool write)
        {
            string perms = read ? "R" : "";
            perms += write ? "WAM" : "";
            perms += read ? "L" : "";
            perms += write ? "CD" : "";
            perms += "P";
            return perms;
        }

        private string HashPassword(string str)
        {
            string chrs = "abcdefghijklmnopqrstuvwxyz";
            Random r = new Random();
            string prefix = chrs.Substring(r.Next(25), 1) + chrs.Substring(r.Next(25), 1);
            str = prefix + str;
            
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(str);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');

            return prefix + hashString.PadLeft(32, '0');
        }

        private void ReloadServUConfig()
        {
            string path = GetServUConfigPath();
            if (!File.Exists(path))
                throw new Exception("Can not find Serv-U config file: " + path);

            List<string> lines = new List<string>();
            lines.AddRange(File.ReadAllLines(path));

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].ToLower() == "[global]")
                {
                    lines.Insert(i + 1, "reloadsettings=true");
                    break;
                }
            }

            // save file
            File.WriteAllLines(path, lines.ToArray());
        }

        private string GetServUConfigPath()
        {
            return Path.Combine(ServUFolder, SERVU_DAEMON_CONFIG_FILE);
        }
        #endregion

        public override bool IsInstalled()
        {

            string name = null;
            string versionNumber = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Serv-U_is1");

            if (key != null)
            {
                name = (string)key.GetValue("DisplayName");
                string[] parts = name.Split(new char[] { ' ' });
                versionNumber = parts[1];
            }
            else
            {
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Serv-U_is1");
                if (key != null)
                {
                    name = (string)key.GetValue("DisplayName");
                    string[] parts = name.Split(new char[] { ' ' });
                    versionNumber = parts[1];
                }
                else
                {
                    return false;
                }
            }

            string[] split = versionNumber.Split(new char[] { '.' });

            return split[0].Equals("6");
                        
        }
    }
}
