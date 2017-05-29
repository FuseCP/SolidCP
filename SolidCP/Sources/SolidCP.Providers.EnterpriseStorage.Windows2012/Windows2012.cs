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
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using SolidCP.Providers.OS;
using SolidCP.Providers.Web;
using System.Management.Automation.Runspaces;

namespace SolidCP.Providers.EnterpriseStorage
{
    public class Windows2012 : HostingServiceProviderBase, IEnterpriseStorage
    {
        #region Properties

        protected string UsersHome
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["UsersHome"]); }
        }

        protected string LocationDrive
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["LocationDrive"]); }
        }

        protected string UsersDomain
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["UsersDomain"]); }
        }

        #endregion

        #region Folders

        public SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings)
        {
            ArrayList items = new ArrayList();

            var webDavSettings = GetWebDavSettings(settings);

            foreach (var setting in webDavSettings)
            {
                string rootPath = string.Format("{0}:\\{1}\\{2}", setting.LocationDrive, setting.HomeFolder,
                    organizationId);

                var windows = new SolidCP.Providers.OS.Windows2012();

                if (Directory.Exists(rootPath))
                {
                    DirectoryInfo root = new DirectoryInfo(rootPath);
                    IWebDav webdav = new Web.WebDav(setting);

                    // get directories
                    DirectoryInfo[] dirs = root.GetDirectories();
                    var quotas = windows.GetQuotasForOrganization(rootPath, string.Empty, string.Empty);

                    foreach (DirectoryInfo dir in dirs)
                    {
                        string fullName = System.IO.Path.Combine(rootPath, dir.Name);

                        SystemFile folder = new SystemFile();

                        folder.Name = dir.Name;
                        folder.FullName = dir.FullName;
                        folder.IsDirectory = true;

                        if (quotas.ContainsKey(fullName))
                        {
                            folder.Size = quotas[fullName].Usage;

                            if (folder.Size == -1)
                            {
                                folder.Size = FileUtils.BytesToMb(FileUtils.CalculateFolderSize(dir.FullName));
                            }

                            folder.Url = string.Format("https://{0}/{1}/{2}", setting.Domain, organizationId, dir.Name);
                            folder.Rules = webdav.GetFolderWebDavRules(organizationId, dir.Name);
                            folder.FRSMQuotaMB = quotas[fullName].Size;
                            folder.FRSMQuotaGB = windows.ConvertMegaBytesToGB(folder.FRSMQuotaMB);
                            folder.FsrmQuotaType = quotas[fullName].QuotaType;

                            items.Add(folder);
                        }
                    }
                }
            }

            return (SystemFile[]) items.ToArray(typeof (SystemFile));
        }

        public SystemFile[] GetQuotasForOrganization(SystemFile[] folders)
        {
            var windows = new SolidCP.Providers.OS.Windows2012();

            var quotasArray = new Dictionary<string, Dictionary<string, Quota>>();

            foreach (var folder in folders)
            {
                var parentFolderPath = Directory.GetParent(folder.FullName).ToString();

                var quotas = quotasArray.ContainsKey(parentFolderPath) 
                    ? quotasArray[parentFolderPath] 
                    : windows.GetQuotasForOrganization(parentFolderPath, string.Empty, string.Empty);

                if (quotas.ContainsKey(folder.FullName) == false)
                {
                    continue;
                }

                var quota = quotas[folder.FullName];

                if (quota != null)
                {
                    folder.Size = quota.Usage;
                    folder.FsrmQuotaType = quota.QuotaType;

                    if (folder.Size == -1)
                    {
                        folder.Size = FileUtils.BytesToMb(FileUtils.CalculateFolderSize(folder.FullName));
                    }
                }
            }

            return folders;
        }

        public SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings)
        {
            ArrayList items = new ArrayList();

            var webDavSettings = GetWebDavSettings(settings);

            foreach (var setting in webDavSettings)
            {
                string rootPath = string.Format("{0}:\\{1}\\{2}", setting.LocationDrive, setting.HomeFolder,
                    organizationId);

                if (Directory.Exists(rootPath))
                {
                    DirectoryInfo root = new DirectoryInfo(rootPath);
                    IWebDav webdav = new Web.WebDav(setting);

                    // get directories
                    DirectoryInfo[] dirs = root.GetDirectories();

                    foreach (DirectoryInfo dir in dirs)
                    {
                        SystemFile folder = new SystemFile();

                        folder.Name = dir.Name;
                        folder.FullName = dir.FullName;
                        folder.IsDirectory = true;

                        if (folder.Size == -1)
                        {
                            folder.Size = FileUtils.BytesToMb(FileUtils.CalculateFolderSize(dir.FullName));
                        }

                        folder.Url = string.Format("https://{0}/{1}/{2}", setting.Domain, organizationId, dir.Name);
                        folder.Rules = webdav.GetFolderWebDavRules(organizationId, dir.Name);

                        items.Add(folder);
                    }
                }
            }

            return (SystemFile[])items.ToArray(typeof(SystemFile));
        }

        public SystemFile GetFolder(string organizationId, string folderName, WebDavSetting setting)
        {
            var webDavSetting = GetWebDavSetting(setting);

            string fullName = string.Format("{0}:\\{1}\\{2}\\{3}", webDavSetting.LocationDrive, webDavSetting.HomeFolder,
                organizationId, folderName);
            SystemFile folder = null;

            var windows = new SolidCP.Providers.OS.Windows2012();

            if (Directory.Exists(fullName))
            {
                DirectoryInfo root = new DirectoryInfo(fullName);

                folder = new SystemFile();

                folder.Name = root.Name;
                folder.FullName = root.FullName;
                folder.IsDirectory = true;

                Quota quota = windows.GetQuotaOnFolder(fullName, string.Empty, string.Empty);

                folder.Size = quota.Usage;

                if (folder.Size == -1)
                {
                    folder.Size = FileUtils.BytesToMb(FileUtils.CalculateFolderSize(root.FullName));
                }

                folder.Url = string.Format("https://{0}/{1}/{2}", webDavSetting.Domain, organizationId, folderName);
                folder.Rules = GetFolderWebDavRules(organizationId, folderName, webDavSetting);
                folder.FRSMQuotaMB = quota.Size;
                folder.FRSMQuotaGB = windows.ConvertMegaBytesToGB(folder.FRSMQuotaMB);
                folder.FsrmQuotaType = quota.QuotaType;
            }

            return folder;
        }

        public void CreateFolder(string organizationId, string folder, WebDavSetting setting)
        {
            var webDavSetting = GetWebDavSetting(setting);

            FileUtils.CreateDirectory(string.Format("{0}:\\{1}\\{2}\\{3}", webDavSetting.LocationDrive,
                webDavSetting.HomeFolder, organizationId, folder));
        }

        public SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            var webDavSetting = GetWebDavSetting(setting);

            var oldPath = string.Format("{0}:\\{1}\\{2}\\{3}", webDavSetting.LocationDrive, webDavSetting.HomeFolder,
                organizationId, originalFolder);
            var newPath = string.Format("{0}:\\{1}\\{2}\\{3}", webDavSetting.LocationDrive, webDavSetting.HomeFolder,
                organizationId, newFolder);

            FileUtils.MoveFile(oldPath, newPath);

            IWebDav webdav = new WebDav(webDavSetting);

            //deleting old folder rules
            webdav.DeleteAllWebDavRules(organizationId, originalFolder);

            return GetFolder(organizationId, newFolder, webDavSetting);
        }

        public void MoveFolder(string oldPath, string newPath)
        {
            FileUtils.CopyFile(oldPath, newPath);

            FileUtils.DeleteFile(oldPath);
        }

        public void DeleteFolder(string organizationId, string folder, WebDavSetting setting)
        {
            var webDavSetting = GetWebDavSetting(setting);

            string rootPath = string.Format("{0}:\\{1}\\{2}\\{3}", webDavSetting.LocationDrive, webDavSetting.HomeFolder,
                organizationId, folder);

            DirectoryInfo treeRoot = new DirectoryInfo(rootPath);

            if (treeRoot.Exists)
            {
                DirectoryInfo[] dirs = treeRoot.GetDirectories();
                while (dirs.Length > 0)
                {
                    foreach (DirectoryInfo dir in dirs)
                        DeleteFolder(organizationId,
                            folder != string.Empty ? string.Format("{0}\\{1}", folder, dir.Name) : dir.Name,
                            webDavSetting);

                    dirs = treeRoot.GetDirectories();
                }

                // DELETE THE FILES UNDER THE CURRENT ROOT
                string[] files = Directory.GetFiles(treeRoot.FullName);
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                IWebDav webdav = new WebDav(webDavSetting);

                webdav.DeleteAllWebDavRules(organizationId, folder);

                Directory.Delete(treeRoot.FullName, true);
            }
        }

        public bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting,
            WebDavFolderRule[] rules)
        {
            var users = new List<UserPermission>();

            foreach (var rule in rules)
            {
                foreach (var user in rule.Users)
                {
                    users.Add(new UserPermission
                    {
                        AccountName = user,
                        Read = rule.Read,
                        Write = rule.Write
                    });
                }

                foreach (var role in rule.Roles)
                {
                    users.Add(new UserPermission
                    {
                        AccountName = role,
                        Read = rule.Read,
                        Write = rule.Write
                    });
                }
            }

            var webDavSetting = GetWebDavSetting(setting);

            string path = string.Format("{0}:\\{1}\\{2}\\{3}", webDavSetting.LocationDrive, webDavSetting.HomeFolder,
                organizationId, folder);

            SecurityUtils.ResetNtfsPermissions(path);
            SecurityUtils.GrantGroupNtfsPermissions(path, users.ToArray(), false, new RemoteServerSettings(), null, null);
            //SecurityUtils.GrantGroupNtfsPermissions(path, users.ToArray(), false, ServerSettings, "*", "*");

            IWebDav webdav = new WebDav(webDavSetting);

            return webdav.SetFolderWebDavRules(organizationId, folder, rules);
        }

        public WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting)
        {
            var webDavSetting = GetWebDavSetting(setting);

            IWebDav webdav = new WebDav(webDavSetting);

            return webdav.GetFolderWebDavRules(organizationId, folder);
        }

        public bool CheckFileServicesInstallation()
        {
            return SolidCP.Server.Utils.OS.CheckFileServicesInstallation();
        }

        #endregion

        public SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            var settings = GetWebDavSetting(null);
            var result = new List<SystemFile>();
#pragma warning disable 0219
            var isRootSearch = false;
#pragma warning restore 0219

            if (searchPaths.Any(string.IsNullOrEmpty))
            {
                isRootSearch = true;
                searchPaths = searchPaths.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }

            {
                using (var conn = new OleDbConnection("Provider=Search.CollatorDSO;Extended Properties='Application=Windows';"))
                {
                    var rootFolder = Path.Combine(settings.LocationDrive + ":\\", settings.HomeFolder);
                    rootFolder = Path.Combine(rootFolder, organizationId);

                    var wsSql = string.Format(@"SELECT System.FileName, System.DateModified, System.Size, System.Kind, System.ItemPathDisplay, System.ItemType, System.Search.AutoSummary FROM SYSTEMINDEX WHERE System.FileName LIKE '%{0}%' AND ({1})",
                        searchText, string.Join(" OR ", searchPaths.Select(x => string.Format("{0} = '{1}'", recursive ? "SCOPE" : "DIRECTORY", Path.Combine(rootFolder, x))).ToArray()));

                    conn.Open();

                    var cmd = new OleDbCommand(wsSql, conn);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader!= null && reader.Read())
                        {
                            var file = new SystemFile {Name = reader[0] as string};

                            file.Changed = file.CreatedDate = reader[1] is DateTime ? (DateTime)reader[1] : new DateTime();
                            file.Size = reader[2] is Decimal ? Convert.ToInt64((Decimal) reader[2]) : 0;

                            var kind = reader[3] is IEnumerable ? ((IEnumerable)reader[3]).Cast<string>().ToList() : null;
                            var itemType = reader[5] as string ?? string.Empty;

                            if (kind != null && kind.Any() && itemType.ToLowerInvariant() != ".zip")
                            {
                                file.IsDirectory = kind.Any(x => x == "folder");
                            }

                            file.FullName = (reader[4] as string ?? string.Empty);

                            
                            file.RelativeUrl = file.FullName.Replace(rootFolder, "").Trim('\\');
                        

                            file.Summary = SanitizeXmlString(reader[6] as string);

                            result.Add(file);
                        }
                    }
                }
            }

            return result.ToArray();
        }


        public string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                return null;
            }

            var buffer = new StringBuilder(xml.Length);

            foreach (char c in xml.Where(c => IsLegalXmlChar(c)))
            {
                buffer.Append(c);
            }

            return buffer.ToString();
        }

        public bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        #region HostingServiceProvider methods
        
        public override string[] Install()
        {
            List<string> messages = new List<string>();

            // create folder if it not exists
            try
            {
                if (!FileUtils.DirectoryExists(UsersHome))
                {
                    FileUtils.CreateDirectory(UsersHome);
                }
            }
            catch (Exception ex)
            {
                messages.Add(String.Format("Folder '{0}' could not be created: {1}",
                    UsersHome, ex.Message));
            }
            return messages.ToArray();
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is HomeFolder)
                        // delete home folder
                        FileUtils.DeleteFile(item.Name);
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }
        }

        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
            foreach (ServiceProviderItem item in items)
            {
                if (item is HomeFolder)
                {
                    try
                    {
                        string path = item.Name;

                        Log.WriteStart(String.Format("Calculating '{0}' folder size", path));

                        // calculate disk space
                        ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
                        diskspace.ItemId = item.Id;
                        diskspace.DiskSpace = FileUtils.CalculateFolderSize(path);
                        itemsDiskspace.Add(diskspace);

                        Log.WriteEnd(String.Format("Calculating '{0}' folder size", path));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }
            return itemsDiskspace.ToArray();
        }

        #endregion

        public override bool IsInstalled()
        {
            Server.Utils.OS.WindowsVersion version = SolidCP.Server.Utils.OS.GetVersion();
            return version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012 ||
                   version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012R2 ||
                   version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2016;
        }

        protected WebDavSetting GetWebDavSetting(WebDavSetting setting)
        {
            if (setting == null || setting.IsEmpty())
            {
                return new WebDavSetting(LocationDrive, UsersHome, UsersDomain);
            }

            return setting;
        }

        protected WebDavSetting[] GetWebDavSettings(WebDavSetting[] settings)
        {
            // 06.09.2015 roland.breitschaft@x-company.de
            // Define a List as an temporary Storage-Object. It?s easier to handle.
            // var webDavSettings = new ArrayList();
            var webDavSettings = new List<WebDavSetting>();

            foreach (var setting in settings)
            {
                if (!setting.IsEmpty())
                {
                    webDavSettings.Add(setting);
                }
            }

            if (webDavSettings.Count == 0)
                return new WebDavSetting[] { GetWebDavSetting(new WebDavSetting()) };
            else
                // 06.09.2015 roland.breitschaft@x-company.de
                // Problem: Parts of settings are empty. But the Method returns the wrong Settings-Object
                // Fix: Return the Cleaned Settings-Object    
                return webDavSettings.ToArray();

            // return settings;
            

        }
    }
}
