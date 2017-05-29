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
using System.Text;
using System.DirectoryServices;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;

using SolidCP.Providers.OS;
using SolidCP.Server.Utils;


namespace SolidCP.Providers.Utils
{

    #region Enums
    [Flags]
    internal enum ADAccountOptions
    {
        UF_TEMP_DUPLICATE_ACCOUNT = 256,
        UF_NORMAL_ACCOUNT = 512,
        UF_INTERDOMAIN_TRUST_ACCOUNT = 2048,
        UF_WORKSTATION_TRUST_ACCOUNT = 4096,
        UF_SERVER_TRUST_ACCOUNT = 8192,
        UF_DONT_EXPIRE_PASSWD = 65536,
        UF_SCRIPT = 1,
        UF_ACCOUNTDISABLE = 2,
        UF_HOMEDIR_REQUIRED = 8,
        UF_LOCKOUT = 16,
        UF_PASSWD_NOTREQD = 32,
        UF_PASSWD_CANT_CHANGE = 64,
        UF_ACCOUNT_LOCKOUT = 16,
        UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 128
    }
    #endregion

    /// <summary>
    /// Summary description for SecurityUtils.
    /// </summary>
    public class SecurityUtils
    {
        private static WmiHelper wmi = new WmiHelper("root\\cimv2");

        #region NTFS permissions
        public static void GrantNtfsPermissions(string path, string accountName,
            NTFSPermission permissions, bool inheritParentPermissions,
            bool preserveOriginalPermissions, RemoteServerSettings serverSettings)
        {
            GrantNtfsPermissions(path, accountName, permissions, inheritParentPermissions, preserveOriginalPermissions, serverSettings, null, null);
        }

        public static void GrantNtfsPermissions(string path, string accountName,
            NTFSPermission permissions, bool inheritParentPermissions,
            bool preserveOriginalPermissions, RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            string sid = GetAccountSid(accountName, serverSettings, usersOU, groupsOU);
            // Check if loaded sid is not empty - because user can manually set up non existent account name.
            if (!String.IsNullOrEmpty(sid))
            {
                GrantNtfsPermissionsBySid(path, sid, permissions, inheritParentPermissions,
                    preserveOriginalPermissions);
            }
        }

        public static void GrantNtfsPermissionsBySid(string path, string sid,
            NTFSPermission permissions, bool inheritParentPermissions,
            bool preserveOriginalPermissions)
        {
            // get file or directory security object
            FileSystemSecurity security = GetFileSystemSecurity(path);
            if (security == null)
                return;

            FileSystemRights rights = FileSystemRights.Read;
            if (permissions == NTFSPermission.FullControl)
                rights = FileSystemRights.FullControl;
            else if (permissions == NTFSPermission.Modify)
                rights = FileSystemRights.Modify;
            else if (permissions == NTFSPermission.Write)
                rights = FileSystemRights.Write;
            else if (permissions == NTFSPermission.Read && security is DirectorySecurity)
                rights = FileSystemRights.ReadAndExecute;
            else if (permissions == NTFSPermission.Read && security is FileSecurity)
                rights = FileSystemRights.Read;

            SecurityIdentifier identity = new SecurityIdentifier(sid);

            if (!preserveOriginalPermissions)
                security = CreateFileSystemSecurity(path);
            else
                security.RemoveAccessRuleAll(new FileSystemAccessRule(identity,
                    FileSystemRights.Read, AccessControlType.Allow));

            if (!inheritParentPermissions)
                security.SetAccessRuleProtection(true, inheritParentPermissions);
            else
                security.SetAccessRuleProtection(false, true);

            InheritanceFlags flags = security is FileSecurity ? InheritanceFlags.None
                : InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

            // change DACL
            FileSystemAccessRule rule = new FileSystemAccessRule(
                identity, rights,
                flags,
                PropagationFlags.None,
                AccessControlType.Allow);

            // add/modify rule
            security.AddAccessRule(rule);

            // set security object
            SetFileSystemSecurity(path, security);
        }

        public static UserPermission[] GetGroupNtfsPermissions(
            string path, UserPermission[] users,
            RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            // get file or directory security object
            FileSystemSecurity security = GetFileSystemSecurity(path);
            if (security == null)
                return users;

            // get all explicit rules
            AuthorizationRuleCollection rules = security.GetAccessRules(true, true, typeof(SecurityIdentifier));

            // iterate through each account
            foreach (UserPermission permission in users)
            {
                SecurityIdentifier identity = null;
                if (String.Compare(permission.AccountName, "network service", true) == 0)
                    identity = new SecurityIdentifier(SystemSID.NETWORK_SERVICE);
                else
                    identity = new SecurityIdentifier(GetAccountSid(permission.AccountName, serverSettings, usersOU, groupsOU));

                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.IdentityReference == identity
                        && rule.AccessControlType == AccessControlType.Allow
                        && (rule.FileSystemRights & FileSystemRights.Read) == FileSystemRights.Read)
                        permission.Read = true;

                    if (rule.IdentityReference == identity
                        && rule.AccessControlType == AccessControlType.Allow
                        && (rule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
                        permission.Write = true;
                }
            }

            return users;
        }

        public static void GrantGroupNtfsPermissions(
            string path, UserPermission[] users, bool resetChildPermissions,
            RemoteServerSettings serverSettings, string usersOU, string groupsOU, bool isProteted = false, bool inheritePermissions = false)
        {
            // get file or directory security object
            FileSystemSecurity security = GetFileSystemSecurity(path);
            if (security == null)
                return;

            // iterate through each account
            foreach (UserPermission permission in users)
            {
                SecurityIdentifier identity = null;
                if (String.Compare(permission.AccountName, "network service", true) == 0)
                    identity = new SecurityIdentifier(SystemSID.NETWORK_SERVICE);
                else
                    identity = new SecurityIdentifier(GetAccountSid(permission.AccountName, serverSettings, usersOU, groupsOU));

                // remove explicit permissions
                security.RemoveAccessRuleAll(new FileSystemAccessRule(identity,
                    FileSystemRights.Read, AccessControlType.Allow));

                if (!permission.Read && !permission.Write)
                    continue;

                FileSystemRights rights = 0;
                if (permission.Write)
                    rights |= FileSystemRights.Write;
                if (permission.Read && security is DirectorySecurity)
                    rights |= FileSystemRights.ReadAndExecute;
                if (permission.Read && security is FileSecurity)
                    rights |= FileSystemRights.Read;
                if (permission.Read && permission.Write)
                    rights |= FileSystemRights.Modify;

                InheritanceFlags flags = security is FileSecurity ? InheritanceFlags.None
                    : InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

                // change DACL
                FileSystemAccessRule rule = new FileSystemAccessRule(
                    identity, rights,
                    flags,
                    PropagationFlags.None,
                    AccessControlType.Allow);

                // add/modify rule
                security.AddAccessRule(rule);
            }

            // set security object
            SetFileSystemSecurity(path, security);

            // reset child permissions if required
            if (resetChildPermissions && Directory.Exists(path))
                ResetChildNtfsPermissions(path);
        }

        public static void RemoveNtfsPermissions(string path, string accountName,
            RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            string sid = GetAccountSid(accountName, serverSettings, usersOU, groupsOU);
            // Check if got non empty sid as long as user can manually supply non existent account.
            if (!String.IsNullOrEmpty(sid))
            {
                RemoveNtfsPermissionsBySid(path, sid);
            }
        }

        public static void RemoveNtfsPermissionsBySid(string path, string sid)
        {
            // get file or directory security object
            FileSystemSecurity security = GetFileSystemSecurity(path);
            if (security == null)
                return;

            // modify DACL
            security.RemoveAccessRuleAll(new FileSystemAccessRule(new SecurityIdentifier(sid),
                FileSystemRights.Read, AccessControlType.Allow));

            // set security object
            SetFileSystemSecurity(path, security);
        }

        public static bool CheckWriteAccessEnabled(string path, string accountName,
            RemoteServerSettings serverSettings)
        {
            return CheckWriteAccessEnabled(path, accountName, serverSettings);
        }

        public static bool CheckWriteAccessEnabled(string path, string accountName,
            RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            return CheckWriteAccessEnabled(path, GetAccountSid(accountName, serverSettings, usersOU, groupsOU));
        }

        public static bool CheckWriteAccessEnabled(string path, string sid)
        {
            // get file or directory security object
            FileSystemSecurity security = GetFileSystemSecurity(path);
            if (security == null)
                return false;

            if (sid == null)
                return false;

            AuthorizationRuleCollection rules = security.GetAccessRules(true, true, typeof(SecurityIdentifier));
            SecurityIdentifier identity = new SecurityIdentifier(sid);
            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.IdentityReference == identity
                    && rule.AccessControlType == AccessControlType.Allow
                    && (rule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
                    return true;
            }
            return false;
        }

        public static void ResetNtfsPermissions(string path)
        {
            // create a new descriptor
            FileSystemSecurity newSecurity = CreateFileSystemSecurity(path);
            if (newSecurity == null)
                return;

            // allow inheritance
            newSecurity.SetAccessRuleProtection(false, true);

            // set security object
            SetFileSystemSecurity(path, newSecurity);
        }

        public static void ResetChildNtfsPermissions(string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            DirectoryInfo[] dirs = root.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // process inner folders
                ResetChildNtfsPermissions(dir.FullName);

                // reset folder permissions
                ResetNtfsPermissions(dir.FullName);
            }

            // reset files indside directory
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo file in files)
            {
                // reset file permissions
                ResetNtfsPermissions(file.FullName);
            }
        }

        private static FileSystemSecurity CreateFileSystemSecurity(string path)
        {
            if (Directory.Exists(path))
                return new DirectorySecurity();
            else if (File.Exists(path))
                return new FileSecurity();
            else
                return null;
        }

        private static FileSystemSecurity GetFileSystemSecurity(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                var directorySecurity = directoryInfo.GetAccessControl(AccessControlSections.Access);
                CanonicalizeDacl(directorySecurity);
                directoryInfo.SetAccessControl(directorySecurity);
                return directoryInfo.GetAccessControl();
            }
            else if (File.Exists(path))
                return new FileInfo(path).GetAccessControl();
            else
                return null;

        }

        private static void CanonicalizeDacl(NativeObjectSecurity objectSecurity)
        {
            if (objectSecurity == null) { throw new ArgumentNullException("objectSecurity"); }
            if (objectSecurity.AreAccessRulesCanonical) { return; }

            // A canonical ACL must have ACES sorted according to the following order:
            // 1. Access-denied on the object
            // 2. Access-denied on a child or property
            // 3. Access-allowed on the object
            // 4. Access-allowed on a child or property
            // 5. All inherited ACEs 
            RawSecurityDescriptor descriptor = new RawSecurityDescriptor(objectSecurity.GetSecurityDescriptorSddlForm(AccessControlSections.Access));

            List<CommonAce> implicitDenyDacl = new List<CommonAce>();
            List<CommonAce> implicitDenyObjectDacl = new List<CommonAce>();
            List<CommonAce> inheritedDacl = new List<CommonAce>();
            List<CommonAce> implicitAllowDacl = new List<CommonAce>();
            List<CommonAce> implicitAllowObjectDacl = new List<CommonAce>();

            foreach (CommonAce ace in descriptor.DiscretionaryAcl)
            {
                if ((ace.AceFlags & AceFlags.Inherited) == AceFlags.Inherited) { inheritedDacl.Add(ace); }
                else
                {
                    switch (ace.AceType)
                    {
                        case AceType.AccessAllowed:
                            implicitAllowDacl.Add(ace);
                            break;

                        case AceType.AccessDenied:
                            implicitDenyDacl.Add(ace);
                            break;

                        case AceType.AccessAllowedObject:
                            implicitAllowObjectDacl.Add(ace);
                            break;

                        case AceType.AccessDeniedObject:
                            implicitDenyObjectDacl.Add(ace);
                            break;
                    }
                }
            }

            Int32 aceIndex = 0;
            RawAcl newDacl = new RawAcl(descriptor.DiscretionaryAcl.Revision, descriptor.DiscretionaryAcl.Count);
            implicitDenyDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            implicitDenyObjectDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            implicitAllowDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            implicitAllowObjectDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));
            inheritedDacl.ForEach(x => newDacl.InsertAce(aceIndex++, x));

            if (aceIndex != descriptor.DiscretionaryAcl.Count)
            {
                System.Diagnostics.Debug.Fail("The DACL cannot be canonicalized since it would potentially result in a loss of information");
                return;
            }

            descriptor.DiscretionaryAcl = newDacl;
            objectSecurity.SetSecurityDescriptorSddlForm(descriptor.GetSddlForm(AccessControlSections.Access), AccessControlSections.Access);
        }

        private static void SetFileSystemSecurity(string path, FileSystemSecurity security)
        {
            if (Directory.Exists(path))
                new DirectoryInfo(path).SetAccessControl((DirectorySecurity)security);
            else if (File.Exists(path))
                new FileInfo(path).SetAccessControl((FileSecurity)security);
        }
        #endregion

        #region Users
        public static string[] GetUsers(RemoteServerSettings serverSettings, string usersOU)
        {
            List<string> users = new List<string>();
            if (serverSettings.ADEnabled)
            {
                // AD mode
                // root entry
                DirectoryEntry objRoot = GetUsersRoot(serverSettings, usersOU);

                //create instance fo the direcory searcher
                DirectorySearcher deSearch = new DirectorySearcher();

                deSearch.SearchRoot = objRoot;
                deSearch.Filter = "(objectClass=user)";
                deSearch.SearchScope = SearchScope.Subtree;

                //find the first instance
                SearchResultCollection results = deSearch.FindAll();

                foreach (SearchResult result in results)
                {
                    DirectoryEntry objUser = GetDirectoryObject(result.Path, serverSettings);
                    users.Add(GetObjectProperty(objUser, "cn").ToString());
                }
            }
            else
            {
                ManagementObjectCollection objUsers = wmi.ExecuteQuery("SELECT * FROM Win32_UserAccount");
                foreach (ManagementObject objUser in objUsers)
                    users.Add((string)objUser.Properties["Name"].Value);
            }
            return users.ToArray();
        }

        public static bool UserExists(string username, RemoteServerSettings serverSettings, string usersOU)
        {
            if (serverSettings.ADEnabled)
            {
                // AD mode
                return (GetUserObject(username, serverSettings, usersOU) != null);
            }
            else
            {
                // LOCAL mode
                return (wmi.ExecuteQuery(
                    String.Format("SELECT * FROM Win32_UserAccount WHERE Name='{0}'", username))).Count > 0;
            }
        }

        public static SystemUser GetUser(string username, RemoteServerSettings serverSettings, string usersOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // get user entry
                    //DirectoryEntry objUser = FindUserObject(username, serverSettings, usersOU);
                    DirectoryEntry objUser = GetUserObject(username, serverSettings, usersOU);
                    if (objUser == null)
                        return null;

                    // fill user
                    SystemUser user = new SystemUser();
                    user.Name = GetObjectProperty(objUser, "cn").ToString();
                    user.FullName = (GetObjectProperty(objUser, "givenName").ToString() + " " +
                        GetObjectProperty(objUser, "sn").ToString()).Trim();
                    user.Description = GetObjectProperty(objUser, "description").ToString();

                    ADAccountOptions userFlags = (ADAccountOptions)objUser.Properties["userAccountControl"].Value;
                    user.PasswordCantChange = ((userFlags & ADAccountOptions.UF_PASSWD_CANT_CHANGE) != 0);
                    user.PasswordNeverExpires = ((userFlags & ADAccountOptions.UF_DONT_EXPIRE_PASSWD) != 0);
                    user.AccountDisabled = ((userFlags & ADAccountOptions.UF_ACCOUNTDISABLE) != 0);
                    user.MsIIS_FTPDir = GetObjectProperty(objUser, "msIIS-FTPDir").ToString();
                    user.MsIIS_FTPRoot = GetObjectProperty(objUser, "msIIS-FTPRoot").ToString();

                    // get user groups
                    user.MemberOf = GetUserGroups(objUser);

                    return user;
                }
                else
                {
                    // LOCAL mode
                    SystemUser userInfo = null;
                    DirectoryEntry computer = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    // get user entry
                    DirectoryEntry user = null;

                    try
                    {
                        user = computer.Children.Find(username, "user");
                    }
                    catch
                    {
                        return userInfo; // user doesn't exist
                    }

                    if (user == null)
                        return userInfo; // user doesn't exist

                    // get user properties
                    userInfo = new SystemUser();

                    userInfo.Name = username;
                    userInfo.FullName = (string)user.Properties["FullName"].Value;
                    userInfo.Description = (string)user.Properties["Description"].Value;

                    ADAccountOptions userFlags = (ADAccountOptions)user.Properties["UserFlags"].Value;
                    userInfo.PasswordCantChange = ((userFlags & ADAccountOptions.UF_PASSWD_CANT_CHANGE) != 0);
                    userInfo.PasswordNeverExpires = ((userFlags & ADAccountOptions.UF_DONT_EXPIRE_PASSWD) != 0);
                    userInfo.AccountDisabled = ((userFlags & ADAccountOptions.UF_ACCOUNTDISABLE) != 0);

                    // get user groups
                    List<string> userGroups = new List<string>();
                    object groups = user.Invoke("Groups", null);
                    foreach (object nGroup in (IEnumerable)groups)
                    {
                        DirectoryEntry objGroup = new DirectoryEntry(nGroup);
                        userGroups.Add(objGroup.Name);
                    }

                    userInfo.MemberOf = userGroups.ToArray();

                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get system user properties", ex);
            }
        }

        public static void CreateUser(SystemUser user, RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    if (user.Name.IndexOf("\\") != -1)
                    {
                        string[] tmpStr = user.Name.Split('\\');
                        user.Name = tmpStr[1];
                    }

                    //check is user name less than 20 symbols
                    if (user.Name.Length > 20)
                    {
                        int separatorPlace = user.Name.IndexOf("_");
                        user.Name = user.Name.Remove(separatorPlace - (user.Name.Length - 20), user.Name.Length - 20);

                    }

                    // AD mode
                    // root entry
                    DirectoryEntry objRoot = GetUsersRoot(serverSettings, usersOU);

                    // add user
                    DirectoryEntry objUser = objRoot.Children.Add("CN=" + user.Name, "user");

                    int spaceIdx = user.FullName.IndexOf(' ');
                    if (spaceIdx == -1)
                    {
                        SetObjectProperty(objUser, "givenName", user.FullName);
                        SetObjectProperty(objUser, "sn", user.FullName);
                    }
                    else
                    {
                        SetObjectProperty(objUser, "givenName", user.FullName.Substring(0, spaceIdx));
                        SetObjectProperty(objUser, "sn", user.FullName.Substring(spaceIdx + 1));
                    }
                    SetObjectProperty(objUser, "description", user.Description);
                    SetObjectProperty(objUser, "UserPrincipalName", user.Name);
                    SetObjectProperty(objUser, "sAMAccountName", user.Name);
                    SetObjectProperty(objUser, "UserPassword", user.Password);

                    if (user.MsIIS_FTPDir != string.Empty)
                    {
                        SetObjectProperty(objUser, "msIIS-FTPDir", user.MsIIS_FTPDir);
                        SetObjectProperty(objUser, "msIIS-FTPRoot", user.MsIIS_FTPRoot);
                    }

                    objUser.Properties["userAccountControl"].Value =
                        ADAccountOptions.UF_NORMAL_ACCOUNT | ADAccountOptions.UF_PASSWD_NOTREQD;
                    objUser.CommitChanges();
                    //myDirectoryEntry = GetUser(UserName);

                    // set password
                    objUser.Invoke("SetPassword", new object[] { user.Password });

                    ADAccountOptions userFlags = ADAccountOptions.UF_NORMAL_ACCOUNT;

                    if (user.PasswordCantChange)
                        userFlags |= ADAccountOptions.UF_PASSWD_CANT_CHANGE;

                    if (user.PasswordNeverExpires)
                        userFlags |= ADAccountOptions.UF_DONT_EXPIRE_PASSWD;

                    if (user.AccountDisabled)
                        userFlags |= ADAccountOptions.UF_ACCOUNTDISABLE;

                    objUser.Properties["userAccountControl"].Value = userFlags;
                    objUser.CommitChanges();

                    // add user to groups
                    foreach (string groupName in user.MemberOf)
                        AddUserToGroup(objUser, groupName, serverSettings, groupsOU);

                    objUser.CommitChanges();
                    objUser.Close();
                }
                else
                {
                    // LOCAL mode
                    DirectoryEntry computer = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    //check is user name less than 20 symbols
                    if (user.Name.Length > 20)
                    {
                        int separatorPlace = user.Name.IndexOf("_");
                        user.Name = user.Name.Remove(separatorPlace - (user.Name.Length - 20), user.Name.Length - 20);

                    }

                    // create user
                    DirectoryEntry objUser = computer.Children.Add(user.Name, "user");
                    objUser.Invoke("SetPassword", new object[] { user.Password });
                    objUser.Properties["FullName"].Add(user.FullName);
                    objUser.Properties["Description"].Add(user.Description);
                    objUser.Properties["UserFlags"].Add(BuildUserFlags(
                        user.PasswordCantChange,
                        user.PasswordNeverExpires,
                        user.AccountDisabled));

                    // save account
                    objUser.CommitChanges();

                    // add user to groups
                    foreach (String groupName in user.MemberOf)
                    {
                        DirectoryEntry group = computer.Children.Find(groupName, "group");
                        if (group != null)
                            group.Invoke("Add", new object[] { objUser.Path.ToString() });
                        group.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create system user", ex);
            }
        }

        public static void UpdateUser(SystemUser user, RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // AD mode
                    // get user entry
                    //DirectoryEntry objUser = FindUserObject(user.Name, serverSettings, usersOU);
                    DirectoryEntry objUser = GetUserObject(user.Name, serverSettings, usersOU);
                    if (objUser == null)
                        return;

                    // get original user groups
                    string[] origGroups = GetUserGroups(objUser);

                    // remove user from original groups
                    foreach (string origGroupName in origGroups)
                        RemoveUserFromGroup(objUser, origGroupName, serverSettings, groupsOU);

                    // change properties
                    int spaceIdx = user.FullName.IndexOf(' ');
                    if (spaceIdx == -1)
                    {
                        objUser.Properties["givenName"].Value = user.FullName;
                        objUser.Properties["sn"].Value = user.FullName;
                    }
                    else
                    {
                        objUser.Properties["givenName"].Value = user.FullName.Substring(0, spaceIdx);
                        objUser.Properties["sn"].Value = user.FullName.Substring(spaceIdx + 1);
                    }

                    objUser.Properties["description"].Value = String.IsNullOrEmpty(user.Description) ? "SolidCP System Account" : user.Description;

                    if (user.MsIIS_FTPDir != string.Empty)
                    {
                        SetObjectProperty(objUser, "msIIS-FTPDir", user.MsIIS_FTPDir);
                        SetObjectProperty(objUser, "msIIS-FTPRoot", user.MsIIS_FTPRoot);
                    }

                    ADAccountOptions userFlags = ADAccountOptions.UF_NORMAL_ACCOUNT;

                    if (user.PasswordCantChange)
                        userFlags |= ADAccountOptions.UF_PASSWD_CANT_CHANGE;

                    if (user.PasswordNeverExpires)
                        userFlags |= ADAccountOptions.UF_DONT_EXPIRE_PASSWD;

                    if (user.AccountDisabled)
                        userFlags |= ADAccountOptions.UF_ACCOUNTDISABLE;

                    objUser.Properties["userAccountControl"].Value = userFlags;

                    objUser.CommitChanges();

                    // add user to groups
                    foreach (string groupName in user.MemberOf)
                        AddUserToGroup(objUser, groupName, serverSettings, groupsOU);

                    // set password if required
                    if (!String.IsNullOrEmpty(user.Password))
                        objUser.Invoke("SetPassword", new object[] { user.Password });

                    objUser.Close();
                }
                else
                {
                    // LOCAL mode
                    // get user entry
                    DirectoryEntry computer = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    // get group entry
                    DirectoryEntry objUser = computer.Children.Find(user.Name, "user");

                    // change user properties
                    objUser.Properties["FullName"].Add(user.FullName);
                    objUser.Properties["Description"].Add(user.Description);
                    objUser.Properties["UserFlags"].Add(BuildUserFlags(
                        user.PasswordCantChange,
                        user.PasswordNeverExpires,
                        user.AccountDisabled));

                    // save account
                    objUser.CommitChanges();

                    // remove user from all assigned groups
                    object groups = objUser.Invoke("Groups", null);
                    foreach (object nGroup in (IEnumerable)groups)
                    {
                        DirectoryEntry objGroup = new DirectoryEntry(nGroup);
                        objGroup.Invoke("Remove", new object[] { objUser.Path });
                    }

                    // add user to groups
                    foreach (String groupName in user.MemberOf)
                    {
                        DirectoryEntry group = computer.Children.Find(groupName, "group");
                        if (group != null)
                            group.Invoke("Add", new object[] { objUser.Path.ToString() });
                        group.CommitChanges();

                    }

                    // change password if required
                    if (!String.IsNullOrEmpty(user.Password))
                        objUser.Invoke("SetPassword", new object[] { user.Password });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update system user", ex);
            }
        }

        public static void ChangeUserPassword(string username, string password,
            RemoteServerSettings serverSettings, string usersOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // AD mode
                    // get user entry
                    //DirectoryEntry objUser = FindUserObject(username, serverSettings, usersOU);
                    DirectoryEntry objUser = GetUserObject(username, serverSettings, usersOU);
                    if (objUser == null)
                        return;

                    // change password
                    objUser.Invoke("SetPassword", new object[] { password });
                    objUser.Close();
                }
                else
                {
                    // LOCAL mode
                    // find user entry
                    DirectoryEntry machine = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    DirectoryEntry objUser = machine.Children.Find(username, "user");

                    // change user password
                    objUser.Invoke("SetPassword", new object[] { password });
                    objUser.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not change user password", ex);
            }
        }

        public static void DeleteUser(string username, RemoteServerSettings serverSettings, string usersOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // AD mode
                    // get user entry
                    //DirectoryEntry objUser = FindUserObject(username, serverSettings, usersOU);
                    DirectoryEntry objUser = GetUserObject(username, serverSettings, usersOU);
                    if (objUser == null)
                        return;

                    objUser.DeleteTree();
                    objUser.CommitChanges();
                }
                else
                {
                    // LOCAL mode
                    DirectoryEntry machine = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    DirectoryEntry objUser = machine.Children.Find(username, "user");
                    machine.Children.Remove(objUser);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete system user", ex);
            }
        }

        public static string GetAccountSid(string accountName, RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            if (serverSettings.ADEnabled)
            {
                // try to get user entry
                byte[] sid = null;
                //DirectoryEntry objUser = FindUserObject(accountName, serverSettings, usersOU);
                DirectoryEntry objUser = GetUserObject(accountName, serverSettings, usersOU);

                if (objUser == null)
                {
                    // try to get group entry
                    DirectoryEntry objGroup = FindGroupObject(accountName, serverSettings, groupsOU);
                    if (objGroup == null)
                        return null;

                    sid = (byte[])GetObjectProperty(objGroup, "objectSid");
                }
                else
                {
                    sid = (byte[])GetObjectProperty(objUser, "objectSid");
                }

                return ConvertByteToStringSid(sid);
            }
            else
            {
                // try to get user account
                string sid = null;
                try
                {
                    string normAccountName = accountName;
                    int idx = normAccountName.LastIndexOf("\\");
                    if (idx != -1)
                        normAccountName = normAccountName.Substring(idx + 1);

                    ManagementObjectCollection accounts = wmi.ExecuteQuery(String.Format(
                        "SELECT * FROM Win32_Account WHERE Name='{0}'",
                        normAccountName));

                    foreach (ManagementObject objAccount in accounts)
                    {
                        sid = objAccount.Properties["SID"].Value.ToString();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not get account sid", ex);
                }

                return sid;
            }
        }

        private static string[] GetUserGroups(DirectoryEntry objUser)
        {
            List<string> userGroups = new List<string>();
            object groups = objUser.Invoke("Groups", null);
            foreach (object group in (IEnumerable)groups)
            {
                // Get the Directory Entry.
                DirectoryEntry objGroup = new DirectoryEntry(group);
                string groupFullName = GetObjectProperty(objGroup, "distinguishedName").ToString();
                int startPos = groupFullName.IndexOf("CN=") + 3;
                int endPos = groupFullName.IndexOf(",", startPos);
                userGroups.Add(groupFullName.Substring(startPos, endPos - startPos));
            }
            return userGroups.ToArray();
        }

        private static void AddUserToGroup(DirectoryEntry objUser, string groupName,
            RemoteServerSettings serverSettings, string groupsOU)
        {
            DirectoryEntry objGroup = FindGroupObject(groupName, serverSettings, groupsOU);
            //
            if (objGroup != null)
            {
                objGroup.Invoke("Add", new Object[] { objUser.Path.ToString() });
                objGroup.Close();
            }
        }

        private static void RemoveUserFromGroup(DirectoryEntry objUser, string groupName,
            RemoteServerSettings serverSettings, string groupsOU)
        {
            DirectoryEntry objGroup = FindGroupObject(groupName, serverSettings, groupsOU);
            if (objGroup != null)
            {
                objGroup.Invoke("Remove", new Object[] { objUser.Path.ToString() });
                objGroup.Close();
            }
        }

        /*private static DirectoryEntry FindUserObject(string userName, RemoteServerSettings serverSettings, string usersOU)
		{
			StringBuilder sb = new StringBuilder();
			//
			AppendProviderPath(sb, serverSettings);
			AppendDomainPath(sb, serverSettings);
			// root entry
			try
			{
				DirectoryEntry srchRoot = GetDirectoryObject(sb.ToString(), serverSettings);
				//
				return GetUserObject(srchRoot, userName, serverSettings);
			}
			catch
			{
				// TO-DO: Add log actions here
			}
			return null;
		}*/

        private static string GetUserName(string userName, RemoteServerSettings serverSettings)
        {
            if (userName.Contains("\\"))
            {
                string[] tmp = userName.Split('\\');
                if (tmp.Length > 1)
                    return tmp[1];
                else
                    return tmp[0];
            }
            else
                return userName;
        }

        private static DirectoryEntry GetUserObject(DirectoryEntry objRoot, string userName,
            RemoteServerSettings serverSettings)
        {
            //create instance fo the direcory searcher
            DirectorySearcher deSearch = new DirectorySearcher();
            // get user name without domain name
            string accountName = GetUserName(userName, serverSettings);
            deSearch.SearchRoot = objRoot;
            deSearch.Filter = "(&(objectClass=user)(cn=" + accountName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            //find the first instance
            SearchResult results = deSearch.FindOne();

            //if found then return, otherwise return Null
            if (results != null)
            {
                return GetDirectoryObject(results.Path, serverSettings);
            }
            else
            {
                return null;
            }
        }

        private static DirectoryEntry GetUserObject(string userName, RemoteServerSettings serverSettings, string usersOU)
        {
            // root entry
            DirectoryEntry objRoot = GetUsersRoot(serverSettings, usersOU);

            return GetUserObject(objRoot, userName, serverSettings);
        }

        private static int BuildUserFlags(
            bool passwordCantChange,
            bool passwordNeverExpires,
            bool accountDisabled)
        {
            ADAccountOptions flags = ADAccountOptions.UF_NORMAL_ACCOUNT;// | ADAccountOptions.UF_SCRIPT;

            if (passwordCantChange)
                flags |= ADAccountOptions.UF_PASSWD_CANT_CHANGE;

            if (passwordNeverExpires)
                flags |= ADAccountOptions.UF_DONT_EXPIRE_PASSWD;

            if (accountDisabled)
                flags |= ADAccountOptions.UF_ACCOUNTDISABLE;

            return (int)flags;
        }
        #endregion

        #region Groups
        /// <summary>
        /// Grants local group membership to both local and Active Directory user accounts.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <param name="serverSettings"></param>
        public static void GrantLocalGroupMembership(string userName, string groupName, RemoteServerSettings serverSettings)
        {
            using (DirectoryEntry computer = new DirectoryEntry(String.Format("WinNT://{0}", Environment.MachineName)))
            {
                // get group entry
                using (DirectoryEntry group = computer.Children.Find(groupName, "group"))
                {
                    string userObjPath = "WinNT://{0}/{1}";
                    //
                    if (serverSettings.ADEnabled)
                        userObjPath = String.Format(userObjPath, serverSettings.ADRootDomain, userName);
                    else
                        userObjPath = String.Format(userObjPath, Environment.MachineName, userName);
                    //
                    try
                    {
                        group.Invoke("Add", new object[] { userObjPath });
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError("SecurityUtils.GrantLocalGroupMembership has failed to succeed", ex);
                    }
                    //
                    group.Close();
                }
                //
                computer.Close();
            }
        }

        /// <summary>
        /// Checks whether a user has membership in local group to both local and Active Directory user accounts.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <param name="serverSettings"></param>
        /// <param name="usersOU"></param>
        public static bool HasLocalGroupMembership(string userName, string groupName, RemoteServerSettings serverSettings, string usersOU)
        {
            using (DirectoryEntry computer = new DirectoryEntry(String.Format("WinNT://{0}", Environment.MachineName)))
            {
                // get group entry
                using (DirectoryEntry group = computer.Children.Find(groupName, "group"))
                {
                    using (DirectoryEntry userObj = serverSettings.ADEnabled ? GetUserObject(userName, serverSettings, usersOU) :
                        computer.Children.Find(userName, "user"))
                    {
                        string accountSid = ConvertByteToStringSid((byte[])GetObjectProperty(userObj, "objectSid"));
                        //
                        try
                        {
                            //
                            object members = group.Invoke("Members", null);
                            //
                            foreach (object member in (IEnumerable)members)
                            {
                                // Get the Directory Entry.
                                using (DirectoryEntry memberObj = new DirectoryEntry(member))
                                {
                                    //
                                    string memberSid = ConvertByteToStringSid((byte[])GetObjectProperty(memberObj, "objectSid"));
                                    //
                                    if (String.Equals(accountSid, memberSid, StringComparison.OrdinalIgnoreCase))
                                    {
                                        return true;
                                    }
                                    //
                                    memberObj.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError("SecurityUtils.HasLocalGroupMembership has failed to succeed", ex);
                        }
                        //
                        userObj.Close();
                    }
                    //
                    group.Close();
                }
                //
                computer.Close();
            }
            //
            return false;
        }

        /// <summary>
        /// Checks whether a user has membership in local group to both local and Active Directory user accounts.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <param name="serverSettings"></param>
        public static void RevokeLocalGroupMembership(string userName, string groupName, RemoteServerSettings serverSettings)
        {
            using (DirectoryEntry computer = new DirectoryEntry(String.Format("WinNT://{0}", Environment.MachineName)))
            {
                // get group entry
                using (DirectoryEntry group = computer.Children.Find(groupName, "group"))
                {
                    string userObjPath = "WinNT://{0}/{1}";
                    //
                    if (serverSettings.ADEnabled)
                        userObjPath = String.Format(userObjPath, serverSettings.ADRootDomain, userName);
                    else
                        userObjPath = String.Format(userObjPath, Environment.MachineName, userName);
                    //
                    try
                    {
                        group.Invoke("Remove", new object[] { userObjPath });
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError("SecurityUtils.RevokeLocalGroupMembership has failed to succeed", ex);
                    }
                    //
                    group.Close();
                }
                //
                computer.Close();
            }
        }

        public static bool GroupExists(string groupName, RemoteServerSettings serverSettings, string groupsOU)
        {
            if (serverSettings.ADEnabled)
            {
                // AD mode
                return (FindGroupObject(groupName, serverSettings, groupsOU) != null);
            }
            else
            {
                // LOCAL mode
                return (wmi.ExecuteQuery(
                    String.Format("SELECT * FROM Win32_Group WHERE Name='{0}'", groupName))).Count > 0;
            }
        }

        public static string[] GetGroups(RemoteServerSettings serverSettings, string groupsOU)
        {
            List<string> groups = new List<string>();
            if (serverSettings.ADEnabled)
            {
                // AD mode
                // root entry
                DirectoryEntry objRoot = GetGroupsRoot(serverSettings, groupsOU);

                //create instance fo the direcory searcher
                DirectorySearcher deSearch = new DirectorySearcher();

                deSearch.SearchRoot = objRoot;
                deSearch.Filter = "(objectClass=group)";
                deSearch.SearchScope = SearchScope.Subtree;

                //find the first instance
                SearchResultCollection results = deSearch.FindAll();

                foreach (SearchResult result in results)
                {
                    DirectoryEntry objGroup = GetDirectoryObject(result.Path, serverSettings);
                    groups.Add(GetObjectProperty(objGroup, "cn").ToString());
                }
            }
            else
            {
                ManagementObjectCollection objUsers = wmi.ExecuteQuery("SELECT * FROM Win32_Group");
                foreach (ManagementObject objUser in objUsers)
                    groups.Add((string)objUser.Properties["Name"].Value);
            }
            return groups.ToArray();
        }

        public static SystemGroup GetGroup(string groupName, RemoteServerSettings serverSettings, string groupsOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // AD mode
                    // get group entry
                    DirectoryEntry objGroup = FindGroupObject(groupName, serverSettings, groupsOU);
                    if (objGroup == null)
                        return null;

                    // fill group
                    SystemGroup group = new SystemGroup();
                    group.Name = GetObjectProperty(objGroup, "cn").ToString();
                    group.Description = GetObjectProperty(objGroup, "description").ToString();

                    // get group users
                    group.Members = GetGroupUsers(objGroup);

                    return group;
                }
                else
                {
                    // LOCAL mode

                    SystemGroup groupInfo = null;

                    DirectoryEntry computer = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    // get group entry
                    DirectoryEntry group = computer.Children.Find(groupName, "group");

                    // get group properties
                    groupInfo = new SystemGroup();

                    groupInfo.Name = groupName;
                    groupInfo.Description = (string)group.Properties["Description"].Value;

                    // get group members
                    List<string> groupMembers = new List<string>();
                    object users = group.Invoke("Members", null);
                    foreach (object nUser in (IEnumerable)users)
                    {
                        // Get the Directory Entry.
                        DirectoryEntry objUser = new DirectoryEntry(nUser);
                        groupMembers.Add(objUser.Name);
                    }

                    groupInfo.Members = groupMembers.ToArray();

                    return groupInfo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get system group properties", ex);
            }
        }

        public static void CreateGroup(SystemGroup group, RemoteServerSettings serverSettings,
            string usersOU, string groupsOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // AD mode
                    // root entry
                    DirectoryEntry objRoot = GetGroupsRoot(serverSettings, groupsOU);

                    // add group
                    DirectoryEntry objGroup = objRoot.Children.Add("CN=" + group.Name, "group");
                    SetObjectProperty(objGroup, "description", group.Description);
                    SetObjectProperty(objGroup, "sAMAccountName", group.Name);
                    objGroup.CommitChanges();

                    // add users to group
                    foreach (string userName in group.Members)
                    {
                        //DirectoryEntry objUser = FindUserObject(userName, serverSettings, usersOU);
                        DirectoryEntry objUser = GetUserObject(userName, serverSettings, usersOU);
                        if (objUser != null)
                        {
                            objGroup.Invoke("Add", new Object[] { objUser.Path.ToString() });
                            objUser.Close();
                        }
                    }

                    objGroup.Close();
                }
                else
                {
                    // LOCAL mode
                    DirectoryEntry computer = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    // create group
                    DirectoryEntry objGroup = computer.Children.Add(group.Name, "group");
                    objGroup.Properties["Description"].Add(group.Description);

                    // save group
                    objGroup.CommitChanges();

                    // add group members
                    foreach (string user in group.Members)
                    {
                        try
                        {
                            objGroup.Invoke("Add", new object[] { String.Format("WinNT://{0}/{1},user", Environment.MachineName, user) });
                        }
                        catch { /* skip */ }
                    }

                    // save group
                    objGroup.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not create system group", ex);
            }
        }

        public static void UpdateGroup(SystemGroup group, RemoteServerSettings serverSettings,
            string usersOU, string groupsOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // AD mode
                    DirectoryEntry objGroup = FindGroupObject(group.Name, serverSettings, groupsOU);

                    if (objGroup == null)
                        return;

                    // update group properties
                    objGroup.Properties["description"].Value = group.Description;
                    objGroup.CommitChanges();

                    // remove all users from the group
                    string[] groupUsers = GetGroupUsers(objGroup);
                    foreach (string userName in groupUsers)
                    {
                        //DirectoryEntry objUser = FindUserObject(userName, serverSettings, usersOU);
                        DirectoryEntry objUser = GetUserObject(userName, serverSettings, usersOU);
                        if (objUser != null)
                        {
                            objGroup.Invoke("Remove", new Object[] { objUser.Path.ToString() });
                            objUser.Close();
                        }
                    }

                    // add users to group
                    foreach (string userName in group.Members)
                    {
                        //DirectoryEntry objUser = FindUserObject(userName, serverSettings, usersOU);
                        DirectoryEntry objUser = GetUserObject(userName, serverSettings, usersOU);
                        if (objUser != null)
                        {
                            objGroup.Invoke("Add", new Object[] { objUser.Path.ToString() });
                            objUser.Close();
                        }
                    }

                    objGroup.Close();
                }
                else
                {
                    // LOCAL mode
                    DirectoryEntry computer = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    // get group entry
                    DirectoryEntry objGroup = computer.Children.Find(group.Name, "group");

                    // change group
                    objGroup.Properties["Description"].Add(group.Description);

                    // save group
                    objGroup.CommitChanges();

                    // remove all group members
                    object users = objGroup.Invoke("Members", null);
                    foreach (object nUser in (IEnumerable)users)
                    {
                        DirectoryEntry objUser = new DirectoryEntry(nUser);
                        objGroup.Invoke("Remove", new object[] { objUser.Path });
                    }

                    // add required group members
                    foreach (string user in group.Members)
                    {
                        try
                        {
                            objGroup.Invoke("Add", new object[] { String.Format("WinNT://{0}/{1},user", Environment.MachineName, user) });
                        }
                        catch { /* skip */ }
                    }

                    // save group
                    objGroup.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not update system group", ex);
            }
        }

        public static void DeleteGroup(string groupName, RemoteServerSettings serverSettings, string groupsOU)
        {
            try
            {
                if (serverSettings.ADEnabled)
                {
                    // AD mode
                    // get group entry
                    DirectoryEntry objGroup = FindGroupObject(groupName, serverSettings, groupsOU);
                    if (objGroup == null)
                        return;

                    objGroup.DeleteTree();
                    objGroup.CommitChanges();
                }
                else
                {
                    // LOCAL mode
                    DirectoryEntry machine = new DirectoryEntry(
                        String.Format("WinNT://{0}", Environment.MachineName));

                    DirectoryEntry objGroup = machine.Children.Find(groupName, "group");
                    machine.Children.Remove(objGroup);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete system group", ex);
            }
        }

        private static string[] GetGroupUsers(DirectoryEntry objGroup)
        {
            PropertyValueCollection members = objGroup.Properties["member"];
            string[] users = new string[members.Count];
            for (int i = 0; i < members.Count; i++)
            {
                string userFullName = members[i].ToString();
                int startPos = userFullName.IndexOf("CN=") + 3;
                int endPos = userFullName.IndexOf(",", startPos);
                users[i] = userFullName.Substring(startPos, endPos - startPos);
                Debug.WriteLine(users[i]);
            }
            return users;
        }

        private static DirectoryEntry FindGroupObject(string groupName, RemoteServerSettings serverSettings, string groupsOU)
        {
            StringBuilder sb = new StringBuilder();
            //
            AppendProviderPath(sb, serverSettings);
            AppendDomainPath(sb, serverSettings);
            // root entry
            try
            {
                DirectoryEntry srchRoot = GetDirectoryObject(sb.ToString(), serverSettings);
                //
                return GetGroupObject(srchRoot, groupName, serverSettings);
            }
            catch
            {
                // TO-DO: Add log actions here
            }
            return null;
        }

        private static DirectoryEntry GetGroupObject(string groupName, RemoteServerSettings serverSettings, string groupsOU)
        {
            // root entry
            DirectoryEntry objRoot = GetGroupsRoot(serverSettings, groupsOU);

            DirectoryEntry result = null;
            try
            {
                result = GetGroupObject(objRoot, groupName, serverSettings);
            }
            catch
            {
                objRoot = GetUsersRoot(serverSettings, groupsOU);
                result = GetGroupObject(objRoot, groupName, serverSettings);
            }
            return result;
        }

        private static DirectoryEntry GetGroupObject(DirectoryEntry objRoot, string groupName,
            RemoteServerSettings serverSettings)
        {
            //create instance fo the direcory searcher
            DirectorySearcher deSearch = new DirectorySearcher();

            deSearch.SearchRoot = objRoot;
            deSearch.Filter = "(&(objectClass=group)(cn=" + groupName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            //find the first instance
            SearchResult results = deSearch.FindOne();

            //if found then return, otherwise return Null
            if (results != null)
            {
                return GetDirectoryObject(results.Path, serverSettings);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Organizational Units
        public static void EnsureOrganizationalUnitsExist(RemoteServerSettings serverSettings, string usersOU, string groupsOU)
        {
            if (serverSettings.ADEnabled)
            {
                // Users OU
                EnsureOrganizationalUnitExists(usersOU, serverSettings);

                // Groups OU
                EnsureOrganizationalUnitExists(groupsOU, serverSettings);
            }
        }

        public static void EnsureOrganizationalUnitExists(string ouName, RemoteServerSettings serverSettings)
        {
            // perform lookup from the upper one
            if (ouName == null || ouName == "")
                return;

            ouName = ouName.Replace("/", "\\");
            string[] parts = ouName.Split('\\');

            DirectoryEntry objUpperOU = GetOURoot("", serverSettings);

            for (int i = 0; i < parts.Length; i++)
            {
                // find OU
                //create instance fo the direcory searcher
                DirectorySearcher deSearch = new DirectorySearcher();

                deSearch.SearchRoot = objUpperOU;
                //set the search filter
                deSearch.Filter = "(&(objectClass=organizationalUnit)(ou=" + parts[i] + "))";
                deSearch.SearchScope = SearchScope.Subtree;

                //find the first instance
                SearchResult results = deSearch.FindOne();

                if (results == null)
                {
                    // OU not found
                    // create it
                    DirectoryEntry objOU = objUpperOU.Children.Add("OU=" + parts[i], "organizationalUnit");
                    objOU.CommitChanges();
                    objUpperOU = objOU;
                }
                else
                {
                    objUpperOU = GetDirectoryObject(results.Path, serverSettings);
                }
            }
        }
        #endregion

        #region Helper Methods
        internal static void SetObjectProperty(DirectoryEntry oDE, string PropertyName, string PropertyValue)
        {
            if ((PropertyValue != string.Empty) && (PropertyValue != null))
            {
                if (oDE.Properties.Contains(PropertyName))
                {
                    oDE.Properties[PropertyName][0] = PropertyValue;
                }
                else
                {
                    oDE.Properties[PropertyName].Add(PropertyValue);
                }
            }
        }

        internal static object GetObjectProperty(DirectoryEntry entry, string propertyName)
        {
            if (entry.Properties.Contains(propertyName))
                return entry.Properties[propertyName][0];
            else
                return String.Empty;
        }

        internal static DirectoryEntry GetDirectoryObject(string path, RemoteServerSettings serverSettings)
        {
            return (serverSettings.ADUsername == null || serverSettings.ADUsername == "")
                ? new DirectoryEntry(path)
                : new DirectoryEntry(path, serverSettings.ADUsername, serverSettings.ADPassword, serverSettings.ADAuthenticationType);
        }

        private static DirectoryEntry GetOURoot(string ouName, RemoteServerSettings serverSettings)
        {
            StringBuilder sb = new StringBuilder();

            // append provider
            AppendProviderPath(sb, serverSettings);
            AppendOUPath(sb, ouName);
            AppendDomainPath(sb, serverSettings);

            return GetDirectoryObject(sb.ToString(), serverSettings);
        }

        private static DirectoryEntry GetGroupsRoot(RemoteServerSettings serverSettings, string groupsOU)
        {
            StringBuilder sb = new StringBuilder();

            // append provider
            AppendProviderPath(sb, serverSettings);
            AppendGroupsPath(sb, serverSettings, groupsOU);
            AppendDomainPath(sb, serverSettings);

            return GetDirectoryObject(sb.ToString(), serverSettings);
        }

        private static DirectoryEntry GetUsersRoot(RemoteServerSettings serverSettings, string usersOU)
        {
            StringBuilder sb = new StringBuilder();

            // append provider
            AppendProviderPath(sb, serverSettings);
            AppendUsersPath(sb, serverSettings, usersOU);
            AppendDomainPath(sb, serverSettings);

            return GetDirectoryObject(sb.ToString(), serverSettings);
        }

        private static void AppendProviderPath(StringBuilder sb, RemoteServerSettings serverSettings)
        {
            sb.Append("LDAP://");
        }

        private static void AppendUsersPath(StringBuilder sb, RemoteServerSettings serverSettings, string usersOU)
        {
            // append OU location
            if (String.IsNullOrEmpty(usersOU))
                sb.Append("CN=Users,"); // default user accounts location
            else
                AppendOUPath(sb, usersOU);
        }

        private static void AppendGroupsPath(StringBuilder sb, RemoteServerSettings serverSettings, string groupsOU)
        {
            // append OU location
            if (String.IsNullOrEmpty(groupsOU))
                sb.Append("CN=Users,"); // default user accounts location
            else
                AppendOUPath(sb, groupsOU);
        }

        private static void AppendOUPath(StringBuilder sb, string ou)
        {
            if (ou == null || ou == "")
                return;

            ou = ou.Replace("/", "\\");
            string[] parts = ou.Split('\\');
            for (int i = parts.Length - 1; i != -1; i--)
                sb.Append("OU=").Append(parts[i]).Append(",");
        }

        private static void AppendDomainPath(StringBuilder sb, RemoteServerSettings serverSettings)
        {
            string[] parts = serverSettings.ADRootDomain.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append("DC=").Append(parts[i]);

                if (i < (parts.Length - 1))
                    sb.Append(",");
            }
        }

        private static string ConvertByteToStringSid(Byte[] sidBytes)
        {
            StringBuilder strSid = new StringBuilder();
            strSid.Append("S-");
            try
            {
                // Add SID revision.
                strSid.Append(sidBytes[0].ToString());
                // Next six bytes are SID authority value.
                if (sidBytes[6] != 0 || sidBytes[5] != 0)
                {
                    string strAuth = String.Format
                        ("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
                        (Int16)sidBytes[1],
                        (Int16)sidBytes[2],
                        (Int16)sidBytes[3],
                        (Int16)sidBytes[4],
                        (Int16)sidBytes[5],
                        (Int16)sidBytes[6]);
                    strSid.Append("-");
                    strSid.Append(strAuth);
                }
                else
                {
                    Int64 iVal = (Int32)(sidBytes[1]) +
                        (Int32)(sidBytes[2] << 8) +
                        (Int32)(sidBytes[3] << 16) +
                        (Int32)(sidBytes[4] << 24);
                    strSid.Append("-");
                    strSid.Append(iVal.ToString());
                }

                // Get sub authority count...
                int iSubCount = Convert.ToInt32(sidBytes[7]);
                int idxAuth = 0;
                for (int i = 0; i < iSubCount; i++)
                {
                    idxAuth = 8 + i * 4;
                    UInt32 iSubAuth = BitConverter.ToUInt32(sidBytes, idxAuth);
                    strSid.Append("-");
                    strSid.Append(iSubAuth.ToString());
                }
            }
            catch
            {
                return "";
            }
            return strSid.ToString();
        }

        /// <summary>
        /// Converts plain text to secure string
        /// </summary>
        /// <param name="plainText">Plain text</param>
        /// <returns>Secure string</returns>
        public static SecureString ConvertToSecureString(string plainText)
        {
            SecureString secureString = new SecureString();
            foreach (char ch in plainText.ToCharArray())
            {
                secureString.AppendChar(ch);
            }
            return secureString;
        }

        #endregion
    }
}