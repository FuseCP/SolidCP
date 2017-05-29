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
    public class FileZilla : HostingServiceProviderBase, IFtpServer
    {
        #region Constants

        private const string FILEZILLA_REG = @"SOFTWARE\FileZilla Server";
        private const string FILEZILLA_REG_X64 = @"SOFTWARE\Wow6432Node\FileZilla Server";
        private const string FILEZILLA_SERVER_FILE = "FileZilla Server.xml";

        #endregion

        #region Properties
        protected virtual string FileZillaFolder
        {
            get
            {
                RegistryKey fzKey = Registry.LocalMachine.OpenSubKey(FILEZILLA_REG) ??
                                    Registry.LocalMachine.OpenSubKey(FILEZILLA_REG_X64);

                if (fzKey == null)
                    throw new Exception("FileZilla registry key was not found: " + FILEZILLA_REG);

                return (string)fzKey.GetValue("Install_Dir");
            }
        }
        #endregion

        #region Sites

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

        public virtual FtpSite[] GetSites()
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
            XmlDocument doc = GetFileZillaConfig();
            XmlNode nodeUser = doc.SelectSingleNode("/FileZillaServer/Users/User[@Name='" + accountName + "']");
            return (nodeUser != null);
        }

        public virtual FtpAccount GetAccount(string accountName)
        {
            XmlDocument doc = GetFileZillaConfig();
            XmlNode nodeUser = doc.SelectSingleNode("/FileZillaServer/Users/User[@Name='" + accountName + "']");
            if (nodeUser == null)
                return null;

            return CreateAccountFromXmlNode(nodeUser, false);
        }

        public virtual FtpAccount[] GetAccounts()
        {
            XmlDocument doc = GetFileZillaConfig();
            XmlNodeList nodeUsers = doc.SelectNodes("/FileZillaServer/Users/User");

            List<FtpAccount> accounts = new List<FtpAccount>();
            foreach (XmlNode nodeUser in nodeUsers)
                accounts.Add(CreateAccountFromXmlNode(nodeUser, true));

            return accounts.ToArray();
        }

        public virtual void CreateAccount(FtpAccount account)
        {
            Log.WriteInfo("GetFileZillaConfig");
            XmlDocument doc = GetFileZillaConfig();


            Log.WriteInfo("Find users nodes");
            // find users node
            XmlNode fzServerNode = doc.SelectSingleNode("FileZillaServer");
            XmlNode fzAccountsNode = fzServerNode.SelectSingleNode("Users");
            if (fzAccountsNode == null)
            {
                fzAccountsNode = doc.CreateElement("Users");
                fzServerNode.AppendChild(fzAccountsNode);
            }

            XmlElement fzAccountNode = doc.CreateElement("User");
            fzAccountsNode.AppendChild(fzAccountNode);
            // set properties
            fzAccountNode.SetAttribute("Name", account.Name);
            SetOption(fzAccountNode, "Pass", MD5(account.Password));
            SetOption(fzAccountNode, "Group", "");
            SetOption(fzAccountNode, "Bypass server userlimit", "0");
            SetOption(fzAccountNode, "User Limit", "0");
            SetOption(fzAccountNode, "IP Limit", "0");
            SetOption(fzAccountNode, "Enabled", BoolToString(account.Enabled));
            SetOption(fzAccountNode, "Comments", "");
            SetOption(fzAccountNode, "ForceSsl", "0");

            // IP filter
            XmlElement nodeIPFilter = doc.CreateElement("IpFilter");
            fzAccountNode.AppendChild(nodeIPFilter);

            XmlElement nodeDisallowed = doc.CreateElement("Disallowed");
            nodeIPFilter.AppendChild(nodeDisallowed);

            XmlElement nodeAllowed = doc.CreateElement("Allowed");
            nodeIPFilter.AppendChild(nodeAllowed);

            // folder
            XmlElement nodePermissions = doc.CreateElement("Permissions");
            fzAccountNode.AppendChild(nodePermissions);

            XmlElement nodePermission = doc.CreateElement("Permission");
            nodePermissions.AppendChild(nodePermission);

            // folder settings
            nodePermission.SetAttribute("Dir", account.Folder);
            SetOption(nodePermission, "FileRead", BoolToString(account.CanRead));
            SetOption(nodePermission, "FileWrite", BoolToString(account.CanWrite));
            SetOption(nodePermission, "FileDelete", BoolToString(account.CanWrite));
            SetOption(nodePermission, "DirCreate", BoolToString(account.CanWrite));
            SetOption(nodePermission, "DirDelete", BoolToString(account.CanWrite));
            SetOption(nodePermission, "DirList", BoolToString(account.CanRead));
            SetOption(nodePermission, "DirSubdirs", BoolToString(account.CanRead));
            SetOption(nodePermission, "IsHome", "1");
            SetOption(nodePermission, "AutoCreate", "0");

            // speed limits
            XmlElement nodeSpeedLimits = doc.CreateElement("SpeedLimits");
            fzAccountNode.AppendChild(nodeSpeedLimits);
            nodeSpeedLimits.SetAttribute("DlType", "0");
            nodeSpeedLimits.SetAttribute("DlLimit", "10");
            nodeSpeedLimits.SetAttribute("ServerDlLimitBypass", "0");
            nodeSpeedLimits.SetAttribute("UlType", "0");
            nodeSpeedLimits.SetAttribute("UlLimit", "10");
            nodeSpeedLimits.SetAttribute("ServerUlLimitBypass", "0");

            XmlElement nodeDownload = doc.CreateElement("Download");
            nodeSpeedLimits.AppendChild(nodeDownload);

            XmlElement nodeUpload = doc.CreateElement("Upload");
            nodeSpeedLimits.AppendChild(nodeUpload);

            // save document
            doc.Save(GetFileZillaConfigPath());

            // reload config
            ReloadFileZillaConfig();
        }

        public virtual void UpdateAccount(FtpAccount account)
        {
            XmlDocument doc = GetFileZillaConfig();
            XmlNode nodeUser = doc.SelectSingleNode("/FileZillaServer/Users/User[@Name='" + account.Name + "']");
            if (nodeUser == null)
                return;

            // update user
            if(!String.IsNullOrEmpty(account.Password))
                SetOption(nodeUser, "Pass", MD5(account.Password));
            SetOption(nodeUser, "Enabled", BoolToString(account.Enabled));

            // update folder
            XmlNode nodePermission = nodeUser.SelectSingleNode("Permissions/Permission");
            if (nodePermission != null)
            {
                ((XmlElement)nodePermission).SetAttribute("Dir", account.Folder);
                SetOption(nodePermission, "FileRead", BoolToString(account.CanRead));
                SetOption(nodePermission, "FileWrite", BoolToString(account.CanWrite));
                SetOption(nodePermission, "FileDelete", BoolToString(account.CanWrite));
                SetOption(nodePermission, "DirCreate", BoolToString(account.CanWrite));
                SetOption(nodePermission, "DirDelete", BoolToString(account.CanWrite));
                SetOption(nodePermission, "DirList", BoolToString(account.CanRead));
                SetOption(nodePermission, "DirSubdirs", BoolToString(account.CanRead));
            }

            // save document
            doc.Save(GetFileZillaConfigPath());

            // reload config
            ReloadFileZillaConfig();
        }

        public virtual void DeleteAccount(string accountName)
        {
            XmlDocument doc = GetFileZillaConfig();
            XmlNode nodeUser = doc.SelectSingleNode("/FileZillaServer/Users/User[@Name='" + accountName + "']");
            if (nodeUser == null)
                return;

            // delete account
            nodeUser.ParentNode.RemoveChild(nodeUser);

            // save document
            doc.Save(GetFileZillaConfigPath());

            // reload config
            ReloadFileZillaConfig();
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

        private void SetOption(XmlNode parentNode, string name, string val)
        {
            XmlNode option = parentNode.SelectSingleNode("Option[@Name='" + name + "']");
            if (option == null)
            {
                option = parentNode.OwnerDocument.CreateElement("Option");
                parentNode.AppendChild(option);
                ((XmlElement)option).SetAttribute("Name", name);
            }
            option.InnerText = val;
        }

        private FtpAccount CreateAccountFromXmlNode(XmlNode nodeUser, bool excludeDetails)
        {
            FtpAccount account = new FtpAccount();
            account.Name = nodeUser.Attributes["Name"].Value;

            if (!excludeDetails)
            {
                account.Password = nodeUser.SelectSingleNode("Option[@Name='Pass']").InnerText;
                account.Enabled = (nodeUser.SelectSingleNode("Option[@Name='Enabled']").InnerText == "1");
                XmlNode nodeFolder = nodeUser.SelectSingleNode("Permissions/Permission");
                if (nodeFolder != null)
                {
                    account.Folder = nodeFolder.Attributes["Dir"].Value;
                    account.CanRead = (nodeFolder.SelectSingleNode("Option[@Name='FileRead']").InnerText == "1");
                    account.CanWrite = (nodeFolder.SelectSingleNode("Option[@Name='FileWrite']").InnerText == "1");
                }
            }

            return account;
        }

        private XmlDocument GetFileZillaConfig()
        {
            string path = GetFileZillaConfigPath();
            if (!File.Exists(path))
                throw new Exception("FileZilla configuration file was not found: " + path);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            return doc;
        }

        private string GetFileZillaConfigPath()
        {
            return Path.Combine(FileZillaFolder, FILEZILLA_SERVER_FILE);
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
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');

            return hashString.PadLeft(32, '0');
        }

        private void ReloadFileZillaConfig()
        {
            FileUtils.ExecuteSystemCommand(
                Path.Combine(FileZillaFolder, "FileZilla Server.exe"),
                "/reload-config");
        }
        #endregion

        public override bool IsInstalled()
        {
            string instPath = null;
            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\FileZilla Server");

            if (key != null)
            {
                instPath = (string)key.GetValue("Install_Dir");
                return instPath != null;
            }
            else
            {
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\FileZilla Server");
                if (key != null)
                {
                    instPath = (string)key.GetValue("Install_Dir");
                    return instPath != null;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
