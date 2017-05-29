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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;
using SolidCP.Providers.Utils.LogParser;
using Microsoft.Win32;

namespace SolidCP.Providers.FTP
{
    public class Gene6 : HostingServiceProviderBase, IFtpServer
    {
        #region Properties
        protected string SiteId
        {
            get { return ProviderSettings["SiteId"]; }
        }

        protected string InstallFolder
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["InstallFolder"]); }
        }

        protected string LogsFolder
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["LogsFolder"]); }
        }
        #endregion

        #region Sites
        public virtual bool SiteExists(string siteId)
        {
            return Directory.Exists(GetDomainPath(siteId));
        }

        public virtual FtpSite[] GetSites()
        {
            List<FtpSite> sites = new List<FtpSite>();

            // get all domain directories
            string[] domainNames = Directory.GetDirectories(GetAccountsPath());
            foreach (string domainName in domainNames)
            {
                sites.Add(GetSite(Path.GetFileName(domainName)));
            }

            return sites.ToArray();
        }

        public virtual FtpSite GetSite(string siteId)
        {
            // load domain
            ServerState state = ServerState.Started;
            return LoadDomain(siteId, out state);
        }

        public virtual string CreateSite(FtpSite site)
        {
            // create site
            SaveDomain(site, ServerState.Started);

            // create "users" directory
            Directory.CreateDirectory(GetDomainPath(site.Name) + "\\users");

            // create "groups" directory
            Directory.CreateDirectory(GetDomainPath(site.Name) + "\\groups");

            return site.Name;
        }

        public virtual void UpdateSite(FtpSite site)
        {
            // get original site state
            ServerState state = ServerState.Started;
            LoadDomain(site.Name, out state);

            // create site
            SaveDomain(site, state);
        }

        public virtual void DeleteSite(string siteId)
        {
            // delete domain directory
            Directory.Delete(GetDomainPath(siteId), true);
        }

        public virtual void ChangeSiteState(string siteId, ServerState state)
        {
            // get original site state
            ServerState origState = ServerState.Started;
            FtpSite site = LoadDomain(siteId, out origState);

            // create site
            SaveDomain(site, state);
        }

        public virtual ServerState GetSiteState(string siteId)
        {
            // load domain
            ServerState state = ServerState.Started;
            LoadDomain(siteId, out state);

            return state;
        }
        #endregion

        #region Accounts
        public virtual bool AccountExists(string accountName)
        {
            string accountPath = GetUserSettingsPath(SiteId, accountName);
            return File.Exists(accountPath);
        }

        public virtual FtpAccount[] GetAccounts()
        {
            List<FtpAccount> accounts = new List<FtpAccount>();

            // get all settings files in directory
            string usersPath = Path.Combine(GetDomainPath(SiteId), "users");
            string[] files = Directory.GetFiles(usersPath, "*.ini");
            foreach (string file in files)
            {
                accounts.Add(GetAccount(Path.GetFileNameWithoutExtension(Path.GetFileName(file))));
            }

            return accounts.ToArray();
        }

        public virtual FtpAccount GetAccount(string accountName)
        {
                string accountPath = GetUserSettingsPath(SiteId, accountName);
                if (!File.Exists(accountPath))
                    return null;

                // load account settings
                NameValueCollection dict = ReadSettingsFile(accountPath);
                if (dict == null)
                    return null;

                FtpAccount account = new FtpAccount();
                account.Name = accountName;

                // acccess rights
                string acr = dict["AccessList0"];
                if (acr != null)
                {
                    string[] acrParts = acr.Split(',');
                    account.Folder = acrParts[1];
                    account.CanRead = (acrParts[2][0] == 'R' && acrParts[3][0] == 'F');
                    account.CanWrite = (acrParts[2][1] == 'W');
                }
                account.Enabled = (dict["Enabled"] == "-1");
                return account;
        }

        public virtual void CreateAccount(FtpAccount account)
        {
            // get default account settings
            DateTime created = DateTime.Now;

            NameValueCollection dict = GetAccountDefaultSettings();
            dict["AccessList0"] = BildAccountAccessList(account);
            dict["CreationDate"] = created.ToString("yyyy/MM/dd HH:mm:ss");
            dict["ExpirationDate"] = created.ToString("yyyy/MM/dd");
            dict["Password"] = "MD5:" + MD5(account.Password);
            dict["Enabled"] = account.Enabled ? "-1" : "0";

            // save account settings
            SaveSettingsFile(GetUserSettingsPath(SiteId, account.Name), "Account", dict);

            // fluch cache
            FlushCache();
        }

        public virtual void UpdateAccount(FtpAccount account)
        {
            // load account settings
            NameValueCollection dict = ReadSettingsFile(GetUserSettingsPath(SiteId, account.Name));
            if (dict == null)
                return;

            // update account properties
            dict["AccessList0"] = BildAccountAccessList(account);

            // change password if required
            if (!String.IsNullOrEmpty(account.Password))
                dict["Password"] = "MD5:" + MD5(account.Password);

            dict["Enabled"] = account.Enabled ? "-1" : "0";

            // save account settings
            SaveSettingsFile(GetUserSettingsPath(SiteId, account.Name), "Account", dict);

            // fluch cache
            FlushCache();
        }

        public virtual void DeleteAccount(string accountName)
        {
            // delete account file
            string accountPath = GetUserSettingsPath(SiteId, accountName);
            if (File.Exists(accountPath))
                File.Delete(accountPath);

            // fluch cache
            FlushCache();
        }
        #endregion

        #region HostingServiceProvier methods
        public override string[] Install()
        {
            List<string> messages = new List<string>();

            // check Gene6 folder exists
            try
            {
                if (!Directory.Exists(InstallFolder))
                    messages.Add(String.Format("Specified '{0}' installation folder does not exist", InstallFolder));
            }
            catch (Exception ex)
            {
                messages.Add(String.Format("Could not check existance of the installation folder: {0}", ex.Message));
                return messages.ToArray();
            }

            // check if siteId is selected
            if (String.IsNullOrEmpty(SiteId))
                messages.Add("FTP Site should be selected");

            return messages.ToArray();
        }

        public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is FtpAccount)
                {
                    try
                    {
                        // make FTP account read-only
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
                        // delete FTP account from default FTP site
                        DeleteAccount(item.Name);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                    }
                }
            }
        }

        public override ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since)
        {
            ServiceProviderItemBandwidth[] itemsBandwidth = new ServiceProviderItemBandwidth[items.Length];

            // create parser object
            // and update statistics
            LogParser parser = new LogParser("Gene6Ftp", SiteId, GetLogsPath(), "cs-username");
            parser.ParseLogs();

            // update items with diskspace
            for (int i = 0; i < items.Length; i++)
            {
                ServiceProviderItem item = items[i];

                // create new bandwidth object
                itemsBandwidth[i] = new ServiceProviderItemBandwidth();
                itemsBandwidth[i].ItemId = item.Id;
                itemsBandwidth[i].Days = new DailyStatistics[0];

                if (item is FtpAccount)
                {
                    try
                    {
                        // get daily statistics
                        itemsBandwidth[i].Days = parser.GetDailyStatistics(since, new string[] { item.Name });
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }
            return itemsBandwidth;
        }
        #endregion

        #region Private helper methods
        private string BildAccountAccessList(FtpAccount account)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/,")
            .Append(account.Folder).Append(",");

            // file permissions
            sb.Append(account.CanRead ? "R" : "-")
            .Append(account.CanWrite ? "W" : "-")
            .Append(account.CanWrite ? "D" : "-")
            .Append(account.CanWrite ? "A" : "-")
            .Append(",");

            // folder permissions
            sb.Append(account.CanRead ? "F" : "-")
            .Append(account.CanRead ? "D" : "-")
            .Append(account.CanWrite ? "M" : "-")
            .Append(account.CanWrite ? "R" : "-")
            .Append(account.CanRead ? "I" : "-")
            .Append("---,");

            return sb.ToString();
        }

        private void SaveDomain(FtpSite site, ServerState state)
        {
            StringBuilder sb = new StringBuilder();

            // append header
            sb.Append("[Domain]\n");

            // build IP bindings
            if (site.Bindings != null)
            {
                for (int i = 0; i < site.Bindings.Length; i++)
                {
                    ServerBinding binding = site.Bindings[i];
                    sb.Append("IP").Append(i).Append("=")
                        .Append((binding.IP == "" || binding.IP == "*") ? "*" : binding.IP).Append(",")
                        .Append(binding.Port).Append(",\n");
                }
            }

            // site state
            string domainStatus = "0"; // online "1" - closed, "2" - offline
            switch (state)
            {
                case ServerState.Continuing: domainStatus = "0"; break; // online
                case ServerState.Paused: domainStatus = "1"; break; // closed
                case ServerState.Started: domainStatus = "0"; break; // online
                case ServerState.Stopped: domainStatus = "2"; break; // offline
                default: domainStatus = "0"; break; // online
            }

            sb.Append("Status=").Append(domainStatus).Append("\n");

            // footer (default settings)
            sb.Append(@"LogList0=Default,W3C,""-1,$DOM_NAME-$year-$mm-$dd.log,0""
LogList1=Transfers,W3C,""-1,$DOM_NAME-$year-$mm-$dd.log,0""
LogList2=Bandwidth,W3C,""-1,$DOM_NAME-$year-$mm-$dd.log,0""
MaxClients=0
MaxConnectionsPerIP=0
Running=1392
StatsCurrentlyLogged=0
CanDeleteReadOnly=-1
ChangeDataIPDependingOnClientIP=-1
DontLimitSpeedForLAN=-1
DontLimitTransferForLAN=-1
HammerResetOnLogin=-1
LogCacheEnabled=-1
LogEnabled=-1
NoCompressionForLAN=-1
ResolveIP=-1
TransferLimitType=never");

            // save domain file
            WriteTextFile(GetDomainSettingsPath(site.Name), sb.ToString());

            // flush cache
            FlushCache();
        }

        private FtpSite LoadDomain(string domainName, out ServerState siteState)
        {
            siteState = ServerState.Started;

            NameValueCollection dict = ReadSettingsFile(GetDomainSettingsPath(domainName));
            if (dict == null)
                return null;

            // process domain settings
            FtpSite site = new FtpSite();
            site.Name = domainName;
            site.SiteId = domainName;

            ArrayList bindings = new ArrayList();
            foreach (string key in dict.Keys)
            {
                string val = dict[key];

                // check for binding
                if (key.ToUpper().StartsWith("IP"))
                {
                    bool numbers = true;
                    for (int i = 2; i < key.Length; i++)
                    {
                        if (!Char.IsNumber(key[i]))
                        {
                            numbers = false;
                            break;
                        }
                    }

                    if (numbers)
                    {
                        // process binding
                        string[] bindParts = val.Split(',');
                        bindings.Add(new ServerBinding(((bindParts[0] == "*") ? "" : bindParts[0]),
                            bindParts[1], ""));
                    }
                }
            }

            // set bindings
            site.Bindings = (ServerBinding[])bindings.ToArray(typeof(ServerBinding));

            // status
            string domainStatus = dict["Status"];
            switch (domainStatus)
            {
                case "0": siteState = ServerState.Started; break;
                case "1": siteState = ServerState.Paused; break;
                case "2": siteState = ServerState.Stopped; break;
                default: siteState = ServerState.Started; break;
            }

            return site;
        }

        private NameValueCollection GetAccountDefaultSettings()
        {
            NameValueCollection dict = new NameValueCollection();
            dict.Add("AccessList0", "/,C:\\2,RWDA,FDMRI---");
            dict.Add("CreationDate", "2005/10/14 11:00:30");
            dict.Add("Enabled", "-1");
            dict.Add("Password", "MD5:827CCB0EEA8A706C4C34A16891F84E7B");
            dict.Add("PasswordEnabled", "-1");
            dict.Add("ExpirationDate", "2005/10/14");
            dict.Add("IncomingFXPAllowed", "-1");
            dict.Add("LogEnabled", "-1");
            dict.Add("OutgoingFXPAllowed", "-1");
            dict.Add("QuotaCheckOnLogin", "-1");
            dict.Add("TimeOutEnabled", "-1");
            dict.Add("TransferLimitType", "never");
            return dict;
        }

        private void FlushCache()
        {
            // just create an ampty file
            WriteTextFile(GetReloadCachePath(), "");
        }

        private string GetInstallFolder()
        {
            string folder = InstallFolder.Replace("/", "\\");
            if (folder[folder.Length - 1] != '\\')
                folder += "\\";
            return folder;
        }

        private NameValueCollection ReadSettingsFile(string path)
        {
            NameValueCollection dict = new NameValueCollection();

            if (!File.Exists(path))
                return null;

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path);
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("[") || line.StartsWith(";"))
                        continue; // skip header and comments

                    // parse line
                    string[] parts = line.Trim().Split('=');
                    dict.Add(parts[0], parts[1]);
                }
            }
            finally
            {
                reader.Close();
            }

            return dict;
        }

        private void SaveSettingsFile(string path, string settingsName, NameValueCollection dict)
        {
            string fullPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            StreamWriter writer = new StreamWriter(path);
            try
            {
                // write header
                writer.WriteLine("[" + settingsName + "]");

                foreach (string key in dict.Keys)
                    writer.WriteLine(key + "=" + dict[key]);
            }
            finally
            {
                writer.Close();
            }
        }

        private string GetAccountsPath()
        {
            return GetInstallFolder() + "Accounts";
        }

        private string GetServerSettingsPath()
        {
            return GetInstallFolder() + "Accounts\\settings.ini";
        }

        private string GetDomainPath(string domainName)
        {
            return GetInstallFolder() + "Accounts\\" + domainName;
        }

        private string GetDomainSettingsPath(string domainName)
        {
            return GetInstallFolder() + "Accounts\\" + domainName + "\\settings.ini";
        }

        private string GetUserSettingsPath(string domainName, string userName)
        {
            return GetInstallFolder() + "Accounts\\" + domainName + "\\users\\" + userName + ".ini";
        }

        private string GetReloadCachePath()
        {
            return GetInstallFolder() + "reload";
        }

        private string GetLogsPath()
        {
            if (String.IsNullOrEmpty(LogsFolder))
                return GetInstallFolder() + "Log";
            else
            {
                string folder = LogsFolder.Replace("/", "\\");
                return folder.TrimEnd('\\');
            }
        }

        private void WriteTextFile(string path, string content)
        {
            string fullPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            StreamWriter writer = new StreamWriter(path);
            try
            {
                writer.Write(content);
            }
            finally
            {
                writer.Close();
            }
        }

        private string ReadTextFile(string path)
        {
            if (!File.Exists(path))
                return null;

            string content = null;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path);
                content = reader.ReadToEnd();
            }
            finally
            {
                reader.Close();
            }
            return content;
        }

        private string MD5(string str)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(str);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0').ToUpper();
        }
        #endregion

        public override bool IsInstalled()
        {
            string versionNumber = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Gene6 FTP Server_is1");

            if (key != null)
            {
                versionNumber = (string)key.GetValue("DisplayVersion");
            }
            else
            {
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Gene6 FTP Server_is1");
                if (key != null)
                {
                    versionNumber = (string)key.GetValue("DisplayVersion");
                }
                else
                {
                    return false;
                }
            }

            string[] split = versionNumber.Split(new char[] { '.' });

            return split[0].Equals("3");

            
        }
    }
}
