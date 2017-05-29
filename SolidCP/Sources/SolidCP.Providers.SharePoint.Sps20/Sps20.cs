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
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.SharePoint
{
    public class Sps20 : HostingServiceProviderBase, ISharePointServer
    {
        #region Classes
        class ProcessExecutionResults
        {
            public int ExitCode;
            public string Output;
        }
        #endregion

        #region Constants
        private const string SHAREPOINT_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\6.0";
        #endregion

        #region Properties
        protected string UsersOU
        {
            get { return ProviderSettings["ADUsersOU"]; }
        }

        protected string GroupsOU
        {
            get { return ProviderSettings["ADGroupsOU"]; }
        }

        protected bool ExclusiveNTLM
        {
            get { return ProviderSettings.GetBool("ExclusiveNTLM"); }
        }
        #endregion

        #region Sites
        public virtual void ExtendVirtualServer(SharePointSite site)
        {
            // install SharePoint
            string cmdPath = GetAdminToolPath();
            string cmdArgs = String.Format(@"-o extendvs -url http://{0} -ownerlogin {1}\{2} -owneremail {3} -databaseserver {4} -databasename {5} -databaseuser {6} -databasepassword {7}",
                site.Name, Environment.MachineName, site.OwnerLogin, site.OwnerEmail,
                site.DatabaseServer, site.DatabaseName, site.DatabaseUser, site.DatabasePassword);

            ProcessExecutionResults result = ExecuteSystemCommand(cmdPath, cmdArgs);
            if (result.ExitCode < 0)
                throw new Exception("Error while installing SharePoint: " + result.Output);
        }

        public virtual void UnextendVirtualServer(string url, bool deleteContent)
        {
            // uninstall SharePoint
            string cmdPath = GetAdminToolPath();
            string cmdArgs = String.Format(@"-o unextendvs -url http://{0}{1}", url,
                (deleteContent ? " -deletecontent" : ""));

            ProcessExecutionResults result = ExecuteSystemCommand(cmdPath, cmdArgs);
            if (result.ExitCode < 0)
                throw new Exception("Error while uninstalling SharePoint: " + result.Output);
        }
        #endregion

        #region Backup/Restore
        public virtual string BackupVirtualServer(string url, string fileName, bool zipBackup)
        {
            string tempPath = Path.GetTempPath();
            string bakFile = Path.Combine(tempPath, (zipBackup
				? StringUtils.CleanIdentifier(url) + ".bsh"
				: StringUtils.CleanIdentifier(fileName)));

            // backup portal
            string cmdPath = GetAdminToolPath();
            string cmdArgs = String.Format(@"-o backup -url http://{0} -filename {1} -overwrite",
                url, bakFile);

            ProcessExecutionResults result = ExecuteSystemCommand(cmdPath, cmdArgs);
            if (result.ExitCode < 0)
                throw new Exception("Error while backing up SharePoint site: " + result.Output);

            // zip backup file
            if (zipBackup)
            {
                string zipFile = Path.Combine(tempPath, fileName);
                string zipRoot = Path.GetDirectoryName(bakFile);

                // zip files
                FileUtils.ZipFiles(zipFile, zipRoot, new string[] { Path.GetFileName(bakFile) });

                // delete data files
                FileUtils.DeleteFile(bakFile);

                bakFile = zipFile;
            }

            return bakFile;
        }

        public virtual void RestoreVirtualServer(string url, string fileName)
        {
            string tempPath = Path.GetTempPath();

            // unzip uploaded files if required
            string expandedFile = fileName;
            if (Path.GetExtension(fileName).ToLower() == ".zip")
            {
                // unpack file
                expandedFile = FileUtils.UnzipFiles(fileName, tempPath)[0];

                // delete zip archive
                FileUtils.DeleteFile(fileName);
            }

            // restore portal
            string cmdPath = GetAdminToolPath();
            string cmdArgs = String.Format(@"-o restore -url http://{0} -filename {1} -overwrite",
                url, expandedFile);

            ProcessExecutionResults result = ExecuteSystemCommand(cmdPath, cmdArgs);
            if (result.ExitCode < 0)
                throw new Exception("Error while restoring SharePoint site: " + result.Output);

            // delete expanded file
            FileUtils.DeleteFile(expandedFile);
        }

        public virtual byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            byte[] buffer = FileUtils.GetFileBinaryChunk(path, offset, length);

            // delete temp file
            if (buffer.Length < length)
                FileUtils.DeleteFile(path);
            return buffer;
        }

        public virtual string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            if (path == null)
            {
                path = Path.Combine(Path.GetTempPath(), fileName);
                if (FileUtils.FileExists(path))
                    FileUtils.DeleteFile(path);
            }

            FileUtils.AppendFileBinaryContent(path, chunk);

            return path;
        }
        #endregion

        #region Web Parts
        public virtual string[] GetInstalledWebParts(string url)
        {
            string cmdPath = GetAdminToolPath();
            string cmdArgs = String.Format(@"-o enumwppacks -url http://{0}", url);

            ProcessExecutionResults result = ExecuteSystemCommand(cmdPath, cmdArgs);

            List<string> list = new List<string>();
            string line = null;
            StringReader reader = new StringReader(result.Output);
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                int commaIdx = line.IndexOf(",");
                if (!String.IsNullOrEmpty(line) && commaIdx != -1)
                    list.Add(line.Substring(0, commaIdx));
            }

            return list.ToArray();
        }

        public virtual void InstallWebPartsPackage(string url, string fileName)
        {
            string tempPath = Path.GetTempPath();

            // unzip uploaded files if required
            string expandedFile = fileName;
            if (Path.GetExtension(fileName).ToLower() == ".zip")
            {
                // unpack file
                expandedFile = FileUtils.UnzipFiles(fileName, tempPath)[0];

                // delete zip archive
                FileUtils.DeleteFile(fileName);
            }

            // install webparts
            string cmdPath = GetAdminToolPath();
            string cmdArgs = String.Format(@"-o addwppack -url http://{0} -filename {1} -force",
                url, expandedFile);

            ProcessExecutionResults result = ExecuteSystemCommand(cmdPath, cmdArgs);
            if (result.ExitCode < 0)
                throw new Exception("Error while installing WebParts package: " + result.Output);

            // delete expanded file
            FileUtils.DeleteFile(expandedFile);
        }

        public virtual void DeleteWebPartsPackage(string url, string packageName)
        {
            // uninstall webparts
            string cmdPath = GetAdminToolPath();
            string cmdArgs = String.Format(@"-o deletewppack -url http://{0} -name {1}",
                url, packageName);

            ProcessExecutionResults result = ExecuteSystemCommand(cmdPath, cmdArgs);
            if (result.ExitCode < 0)
                throw new Exception("Error while installing WebParts package: " + result.Output);
        }
        #endregion

        #region Users and Groups

        public virtual bool UserExists(string username)
        {
            return SecurityUtils.UserExists(username, ServerSettings, UsersOU);
        }

        public virtual string[] GetUsers()
        {
            return SecurityUtils.GetUsers(ServerSettings, UsersOU);
        }

        public virtual SystemUser GetUser(string username)
        {
            return SecurityUtils.GetUser(username, ServerSettings, UsersOU);
        }

        public virtual void CreateUser(SystemUser user)
        {
            SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);
        }

        public virtual void UpdateUser(SystemUser user)
        {
            SecurityUtils.UpdateUser(user, ServerSettings, UsersOU, GroupsOU);
        }

        public virtual void ChangeUserPassword(string username, string password)
        {
            SecurityUtils.ChangeUserPassword(username, password, ServerSettings, UsersOU);
        }

        public virtual void DeleteUser(string username)
        {
            SecurityUtils.DeleteUser(username, ServerSettings, UsersOU);
        }

        public virtual bool GroupExists(string groupName)
        {
            return SecurityUtils.GroupExists(groupName, ServerSettings, GroupsOU);
        }

        public virtual string[] GetGroups()
        {
            return SecurityUtils.GetGroups(ServerSettings, GroupsOU);
        }

        public virtual SystemGroup GetGroup(string groupName)
        {
            return SecurityUtils.GetGroup(groupName, ServerSettings, GroupsOU);
        }

        public virtual void CreateGroup(SystemGroup group)
        {
            SecurityUtils.CreateGroup(group, ServerSettings, UsersOU, GroupsOU);
        }

        public virtual void UpdateGroup(SystemGroup group)
        {
            SecurityUtils.UpdateGroup(group, ServerSettings, UsersOU, GroupsOU);
        }

        public virtual void DeleteGroup(string groupName)
        {
            SecurityUtils.DeleteGroup(groupName, ServerSettings, GroupsOU);
        }
        #endregion

        #region HostingServiceProvider
        public override string[] Install()
        {
            List<string> messages = new List<string>();

            try
            {
                SecurityUtils.EnsureOrganizationalUnitsExist(ServerSettings, UsersOU, GroupsOU);
            }
            catch (Exception ex)
            {
                messages.Add(String.Format("Could not check/create Organizational Units: {0}", ex.Message));
                return messages.ToArray();
            }

            // check if SharePoint is installed
            if (!IsSharePointInstalled())
            {
                messages.Add("Most probably Windows SharePoint Services is not installed on this server.");
                return messages.ToArray();
            }
            return messages.ToArray();
        }

        public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is SystemUser)
                    {
                        // enable/disable user account
                        if (UserExists(item.Name))
                        {
                            SystemUser user = GetUser(item.Name);
                            user.AccountDisabled = !enabled;
                            UpdateUser(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error switching '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is SharePointSite)
                {
                    // delete SP site
                    try
                    {
                        UnextendVirtualServer(item.Name, true);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                    }
                }
                else if (item is SystemUser)
                {
                    // delete user
                    try
                    {
                        DeleteUser(item.Name);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                    }
                }
                else if (item is SystemGroup)
                {
                    // delete user
                    try
                    {
                        DeleteGroup(item.Name);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                    }
                }
            }
        }
        #endregion

        #region Private Helpers
        protected virtual string GetAdminToolPath()
        {
            RegistryKey spKey = Registry.LocalMachine.OpenSubKey(SHAREPOINT_REGLOC);
            if (spKey == null)
                throw new Exception("SharePoint Services is not installed on the system");

            return ((string)spKey.GetValue("Location")) + @"\bin\stsadm.exe";
        }

        protected virtual bool IsSharePointInstalled()
        {
            RegistryKey spKey = Registry.LocalMachine.OpenSubKey(SHAREPOINT_REGLOC);
            if (spKey == null)
                return false;

            string spVal = (string)spKey.GetValue("SharePoint");
            return (String.Compare(spVal, "installed", true) == 0);
        }

        private ProcessExecutionResults ExecuteSystemCommand(string cmd, string args)
        {
            ProcessExecutionResults result = new ProcessExecutionResults();

            // launch system process
            ProcessStartInfo startInfo = new ProcessStartInfo(cmd, args);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            Process proc = Process.Start(startInfo);

            // analyze results
            StreamReader reader = proc.StandardOutput;
            result.Output = reader.ReadToEnd();
            result.ExitCode = proc.ExitCode;
            reader.Close();

            return result;
        }
        #endregion

        public override bool IsInstalled()
        {
            return false;
        }
    }
}
