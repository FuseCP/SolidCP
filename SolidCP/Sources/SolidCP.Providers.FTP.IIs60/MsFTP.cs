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
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Management;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.Providers.Utils.LogParser;
using SolidCP.Server.Utils;
using Microsoft.Win32;

namespace SolidCP.Providers.FTP
{
    public class MsFTP : HostingServiceProviderBase, IFtpServer
    {
        #region Properties
        protected string SiteId
        {
            get { return ProviderSettings["SiteId"]; }
        }

        protected string FtpGroupName
        {
            get { return ProviderSettings["FtpGroupName"]; }
        }

        protected string UsersOU
        {
            get { return ProviderSettings["ADUsersOU"]; }
        }

        protected string GroupsOU
        {
            get { return ProviderSettings["ADGroupsOU"]; }
        }
        #endregion

        // constants
	    public const string FTP_SERVICE_ID = "MSFTPSVC";

	    // private fields
	    private WmiHelper wmi = null;

        public MsFTP()
	    {

            if (IsInstalled())
            {
                // instantiate WMI helper
                wmi = new WmiHelper("root\\MicrosoftIISv2");
            }
	    }

	    #region Sites
	    public virtual bool SiteExists(string siteId)
	    {
		    return (wmi.ExecuteQuery(
			    String.Format("SELECT * FROM IIsFtpServerSetting WHERE Name='{0}'", siteId)).Count > 0);
	    }

	    public virtual FtpSite[] GetSites()
	    {
            List<FtpSite> sites = new List<FtpSite>();

		    // get all sites
		    ManagementObjectCollection objSites = wmi.ExecuteQuery("SELECT * FROM IIsFtpServerSetting");
		    foreach (ManagementObject objSite in objSites)
		    {
			    FtpSite site = new FtpSite();

			    // fill site properties
			    FillFtpSiteFromWmiObject(site, objSite);

			    sites.Add(site);
		    }
		    return sites.ToArray();
	    }

	    public virtual FtpSite GetSite(string siteId)
	    {
		    FtpSite site = new FtpSite();

		    ManagementObject objSite = wmi.GetObject(
			    String.Format("IIsFtpServerSetting='{0}'", siteId));

		    // fill properties
		    FillFtpSiteFromWmiObject(site, objSite);

		    return site;
	    }

	    public virtual string CreateSite(FtpSite site)
	    {
		    // create folder if nessesary
		    //if(!Directory.Exists(site.ContentPath))
		    //	Directory.CreateDirectory(site.ContentPath);

		    // create anonymous user account if required
		    // blah blah blah

		    // set account permissions
		    // blah blah blah

		    // check server bindings
		    CheckFtpServerBindings(site.Bindings);

		    // create FTP site
		    ManagementObject objService = wmi.GetObject(String.Format("IIsFtpService='{0}'", FTP_SERVICE_ID));

		    ManagementBaseObject methodParams = objService.GetMethodParameters("CreateNewSite");

		    // create server bindings
		    ManagementClass clsBinding = wmi.GetClass("ServerBinding");
		    ManagementObject[] objBinings = new ManagementObject[site.Bindings.Length];

		    for (int i = 0; i < objBinings.Length; i++)
		    {
			    objBinings[i] = clsBinding.CreateInstance();
			    objBinings[i]["Hostname"] = site.Bindings[i].Host;
			    objBinings[i]["IP"] = site.Bindings[i].IP;
			    objBinings[i]["Port"] = site.Bindings[i].Port;
		    }

		    methodParams["ServerBindings"] = objBinings;
		    methodParams["ServerComment"] = site.Name;
		    methodParams["PathOfRootVirtualDir"] = site.ContentPath;

		    ManagementBaseObject objSite = objService.InvokeMethod("CreateNewSite", methodParams, new InvokeMethodOptions());

		    // get FTP settings
		    string siteId = ((string)objSite["returnValue"]).Remove(0, "IIsFtpServer='".Length).Replace("'", "");

		    // update site properties
		    ManagementObject objSettings = wmi.GetObject(
			    String.Format("IIsFtpServerSetting='{0}'", siteId));

		    FillWmiObjectFromFtpSite(objSettings, site);

		    // start FTP site
		    ChangeSiteState(siteId, ServerState.Started);
		    return siteId;
	    }

	    public virtual void UpdateSite(FtpSite site)
	    {
		    // check server bindings
		    CheckFtpServerBindings(site.Bindings);

		    // update site properties
		    ManagementObject objSettings = wmi.GetObject(
			    String.Format("IIsFtpServerSetting='{0}'", site.SiteId));

		    FillWmiObjectFromFtpSite(objSettings, site);

		    // update server bindings
		    ManagementClass clsBinding = wmi.GetClass("ServerBinding");
		    ManagementObject[] objBinings = new ManagementObject[site.Bindings.Length];

		    for (int i = 0; i < objBinings.Length; i++)
		    {
			    objBinings[i] = clsBinding.CreateInstance();
			    objBinings[i]["Hostname"] = site.Bindings[i].Host;
			    objBinings[i]["IP"] = site.Bindings[i].IP;
			    objBinings[i]["Port"] = site.Bindings[i].Port;

		    }

		    objSettings.Properties["ServerBindings"].Value = objBinings;

		    // save object
		    objSettings.Put();
	    }

	    public virtual void DeleteSite(string siteId)
	    {
		    ManagementObject objSite = wmi.GetObject(String.Format("IIsFtpServer='{0}'", siteId));
		    objSite.Delete();
	    }

	    public virtual void ChangeSiteState(string siteId, ServerState state)
	    {
		    ManagementObject objSite = wmi.GetObject(String.Format("IIsFtpServer='{0}'", siteId));
		    string methodName = "Continue";
		    switch (state)
		    {
			    case ServerState.Started: methodName = "Start"; break;
			    case ServerState.Stopped: methodName = "Stop"; break;
			    case ServerState.Paused: methodName = "Pause"; break;
			    case ServerState.Continuing: methodName = "Continue"; break;
			    default: methodName = "Start"; break;
		    }

		    // invoke method
		    objSite.InvokeMethod(methodName, null);
	    }

	    public virtual ServerState GetSiteState(string siteId)
	    {
		    ManagementObject objSite = wmi.GetObject(String.Format("IIsFtpServer='{0}'", siteId));
		    return (ServerState)objSite.Properties["ServerState"].Value;
	    }
	    #endregion

	    #region Accounts
	    public virtual bool AccountExists(string accountName)
	    {
		    bool ftpExists = (wmi.ExecuteQuery(
			    String.Format("SELECT * FROM IIsFtpVirtualDirSetting" +
			    " WHERE Name='{0}'", GetAccountPath(SiteId, accountName))).Count > 0);
            bool systemExists = SecurityUtils.UserExists(accountName, ServerSettings, UsersOU);
		    return (ftpExists || systemExists);
	    }

	    public virtual FtpAccount[] GetAccounts()
	    {
            List<FtpAccount> accounts = new List<FtpAccount>();

		    ManagementObjectCollection objAccounts = wmi.ExecuteQuery(
			    String.Format("SELECT * FROM IIsFtpVirtualDirSetting" +
			    " WHERE Name LIKE '{0}%'", SiteId));

		    foreach (ManagementObject objAccount in objAccounts)
		    {
			    string rootName = GetAccountPath(SiteId, "");
			    string name = (string)objAccount.Properties["Name"].Value;

			    if (String.Compare(rootName, name, true) != 0)
			    {
				    FtpAccount acc = new FtpAccount();

				    acc.Name = name.Substring(rootName.Length + 1);
				    acc.Folder = (string)objAccount.Properties["Path"].Value;
				    acc.CanRead = (bool)objAccount.Properties["AccessRead"].Value;
				    acc.CanWrite = (bool)objAccount.Properties["AccessWrite"].Value;

				    accounts.Add(acc);
			    }
		    }

		    return accounts.ToArray();
	    }


	    public virtual FtpAccount GetAccount(string accountName)
	    {
		    FtpAccount acc = new FtpAccount();

		    ManagementObject objAccount = wmi.GetObject(
			    String.Format("IIsFtpVirtualDirSetting='{0}'", GetAccountPath(SiteId, accountName)));

		    acc.Name = accountName;
		    acc.Folder = (string)objAccount.Properties["Path"].Value;
		    acc.CanRead = (bool)objAccount.Properties["AccessRead"].Value;
		    acc.CanWrite = (bool)objAccount.Properties["AccessWrite"].Value;

            // load user account
            SystemUser user = SecurityUtils.GetUser(accountName, ServerSettings, UsersOU);
            if (user != null)
            {
                acc.Enabled = !user.AccountDisabled;
            }
		    return acc;
	    }

	    public virtual void CreateAccount(FtpAccount account)
	    {
		    // create user account
		    SystemUser user = new SystemUser();
		    user.Name = account.Name;
		    user.FullName = account.Name;
            if (user.FullName.Length > 20)
            {
				Exception ex = new Exception("SolidCP_ERROR@FTP_USERNAME_MAX_LENGTH_EXCEEDED@");
                throw ex;
            }
		    user.Description = "SolidCP System Account";
		    user.MemberOf = new string[] { FtpGroupName };
		    user.Password = account.Password;
		    user.PasswordCantChange = true;
		    user.PasswordNeverExpires = true;
		    user.AccountDisabled = !account.Enabled;
		    user.System = true;

		    // create in the system
		    SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);

		    // prepare home folder
		    EnsureUserHomeFolderExists(account.Folder, account.Name, account.CanRead, account.CanWrite);

		    // create account in FTP
		    ManagementObject objDir = wmi.GetClass("IIsFtpVirtualDir").CreateInstance();
		    ManagementObject objDirSetting = wmi.GetClass("IIsFtpVirtualDirSetting").CreateInstance();

		    string accId = GetAccountPath(SiteId, account.Name);

		    objDir.Properties["Name"].Value = accId;

		    objDirSetting.Properties["Name"].Value = accId;
		    objDirSetting.Properties["Path"].Value = account.Folder;
		    objDirSetting.Properties["AccessRead"].Value = account.CanRead;
		    objDirSetting.Properties["AccessWrite"].Value = account.CanWrite;
		    objDirSetting.Properties["AccessScript"].Value = false;
		    objDirSetting.Properties["AccessSource"].Value = false;
		    objDirSetting.Properties["AccessExecute"].Value = false;
			// UNC Path (Connect As)
			FillWmiObjectUNCFromFtpAccount(objDirSetting, account);

		    // save account
		    objDir.Put();
		    objDirSetting.Put();
	    }

	    private void EnsureUserHomeFolderExists(string folder, string accountName, bool allowRead, bool allowWrite)
	    {
		    // create folder
		    if (!FileUtils.DirectoryExists(folder))
			    FileUtils.CreateDirectory(folder);

            if (!allowRead && !allowWrite)
                return;

            NTFSPermission permissions = allowRead ? NTFSPermission.Read : NTFSPermission.Write;

            if (allowRead && allowWrite)
                permissions = NTFSPermission.Modify;

		    // set permissions
            SecurityUtils.GrantNtfsPermissions(folder, accountName, permissions, true, true,
                ServerSettings, UsersOU, GroupsOU);

	    }

	    public virtual void UpdateAccount(FtpAccount account)
	    {
		    ManagementObject objAccount = wmi.GetObject(
			    String.Format("IIsFtpVirtualDirSetting='{0}'", GetAccountPath(SiteId, account.Name)));

            // remove permissions of required
            string origPath = (string)objAccount.Properties["Path"].Value;
            if (String.Compare(origPath, account.Folder, true) != 0)
                RemoveFtpFolderPermissions(origPath, account.Name);

            // set new permissions
            EnsureUserHomeFolderExists(account.Folder, account.Name, account.CanRead, account.CanWrite);

		    //objDirSetting.Properties["Name"].Value = accId;
		    objAccount.Properties["Path"].Value = account.Folder;
		    objAccount.Properties["AccessRead"].Value = account.CanRead;
		    objAccount.Properties["AccessWrite"].Value = account.CanWrite;
		    objAccount.Properties["AccessScript"].Value = false;
		    objAccount.Properties["AccessSource"].Value = false;
		    objAccount.Properties["AccessExecute"].Value = false;

			// UNC Path (Connect As)
			FillWmiObjectUNCFromFtpAccount(objAccount, account);

		    // save account
		    objAccount.Put();

            // change user account state
            // and password (if required)
            SystemUser user = SecurityUtils.GetUser(account.Name, ServerSettings, UsersOU);
            user.Password = account.Password;
            user.AccountDisabled = !account.Enabled;
            SecurityUtils.UpdateUser(user, ServerSettings, UsersOU, GroupsOU);
	    }

	    public virtual void DeleteAccount(string accountName)
	    {
		    ManagementObject objAccount = wmi.GetObject(String.Format("IIsFtpVirtualDirSetting='{0}'",
			    GetAccountPath(SiteId, accountName)));
            string origPath = (string)objAccount.Properties["Path"].Value;
		    objAccount.Delete();

            // remove permissions
            RemoveFtpFolderPermissions(origPath, accountName);

		    // delete system user account
            if (SecurityUtils.UserExists(accountName, ServerSettings, UsersOU))
                SecurityUtils.DeleteUser(accountName, ServerSettings, UsersOU);
	    }

        private void RemoveFtpFolderPermissions(string path, string accountName)
        {
            if (!FileUtils.DirectoryExists(path))
                return;

            // anonymous account
            SecurityUtils.RemoveNtfsPermissions(path, accountName,
                ServerSettings, UsersOU, GroupsOU);
        }
	    #endregion

	    #region IHostingServiceProvier methods
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

		    // create folder if it not exists
		    if (String.IsNullOrEmpty(SiteId))
		    {
			    messages.Add("Please, select FTP site to create accounts on");
		    }
		    else
		    {
			    // create FTP group name
			    if (String.IsNullOrEmpty(FtpGroupName))
			    {
				    messages.Add("FTP Group can not be blank");
			    }
			    else
			    {
				    try
				    {
					    // create group
                        if (!SecurityUtils.GroupExists(FtpGroupName, ServerSettings, GroupsOU))
					    {
						    SystemGroup group = new SystemGroup();
                            group.Name = FtpGroupName;
						    group.Members = new string[] { };
						    group.Description = "SolidCP System Group";

                            SecurityUtils.CreateGroup(group, ServerSettings, UsersOU, GroupsOU);
					    }
				    }
				    catch (Exception ex)
				    {
					    messages.Add(String.Format("There was an error while adding '{0}' group: {1}",
                            FtpGroupName, ex.Message));
					    return messages.ToArray();
				    }
			    }

			    FtpSite site = GetSite(SiteId);
			    try
			    {
				    // set permissions on the site root
                    SecurityUtils.GrantNtfsPermissions(site.ContentPath, FtpGroupName,
                        NTFSPermission.Read, true, true, ServerSettings,
                        UsersOU, GroupsOU);
			    }
			    catch (Exception ex)
			    {
				    messages.Add(String.Format("Can not set permissions on '{0}' folder: {1}",
					    site.ContentPath, ex.Message));
				    return messages.ToArray();
			    }
		    }
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

		    // calculate bandwidth for Default FTP Site
		    FtpSite fptSite = GetSite(SiteId);
		    string siteId = SiteId.Replace("/", "");
            string logsPath = Path.Combine(fptSite.LogFileDirectory, siteId);

		    // create parser object
		    // and update statistics
		    LogParser parser = new LogParser("Ftp", siteId, logsPath, "s-sitename", "cs-username");
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
					    itemsBandwidth[i].Days = parser.GetDailyStatistics(since, new string[] { siteId, item.Name });
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

	    #region private helper methods
	    private void CheckFtpServerBindings(ServerBinding[] bindings)
	    {
		    if (bindings == null || bindings.Length == 0)
			    throw new ArgumentException("Provide FTP site server bindings", "FtpSite.Bindings");

		    // check for server bindings
		    ManagementObjectCollection objSites = wmi.ExecuteQuery("SELECT * FROM IIsFtpServerSetting");
		    foreach (ManagementObject objProbSite in objSites)
		    {
			    string probSiteId = (string)objProbSite.Properties["Name"].Value;
			    string probSiteComment = (string)objProbSite.Properties["ServerComment"].Value;

			    // check server bindings
			    ManagementBaseObject[] objProbBinings = (ManagementBaseObject[])objProbSite.Properties["ServerBindings"].Value;
			    if (objProbBinings != null)
			    {
				    // check this binding against provided ones
				    foreach (ManagementBaseObject objProbBinding in objProbBinings)
				    {
					    string siteIP = (string)objProbBinding.Properties["IP"].Value;
					    string sitePort = (string)objProbBinding.Properties["Port"].Value;

					    for (int i = 0; i < bindings.Length; i++)
					    {
						    if (siteIP == bindings[i].IP &&
							    sitePort == bindings[i].Port)
							    throw new Exception(String.Format("The FTP site '{0}' ({1}) already uses provided IP and port.",
								    probSiteComment, probSiteId));
					    }
				    }
			    }
		    }
	    }

	    private string GetAccountPath(string siteId, string accountName)
	    {
		    string path = siteId + "/ROOT";
		    if (accountName != null && accountName != "")
			    path += "/" + accountName;
		    return path;
	    }

	    private void FillFtpSiteFromWmiObject(FtpSite site, ManagementBaseObject obj)
	    {
		    site.SiteId = (string)obj.Properties["Name"].Value;
		    site.Name = (string)obj.Properties["ServerComment"].Value;
		    site.AllowReadAccess = (bool)obj.Properties["AccessRead"].Value;
		    site.AllowScriptAccess = (bool)obj.Properties["AccessScript"].Value;
		    site.AllowSourceAccess = (bool)obj.Properties["AccessSource"].Value;
		    site.AllowWriteAccess = (bool)obj.Properties["AccessWrite"].Value;
		    site.AllowExecuteAccess = (bool)obj.Properties["AccessExecute"].Value;
		    site.AnonymousUsername = (string)obj.Properties["AnonymousUserName"].Value;
		    site.AnonymousUserPassword = (string)obj.Properties["AnonymousUserPass"].Value;
		    site.AllowAnonymous = (bool)obj.Properties["AllowAnonymous"].Value;
		    site.AnonymousOnly = (bool)obj.Properties["AnonymousOnly"].Value;
		    site.LogFileDirectory = (string)obj.Properties["LogFileDirectory"].Value;

		    // get server bindings
		    ManagementBaseObject[] objBindings =
			    ((ManagementBaseObject[])obj.Properties["ServerBindings"].Value);

		    if (objBindings != null)
		    {
			    site.Bindings = new ServerBinding[objBindings.Length];
			    for (int i = 0; i < objBindings.Length; i++)
			    {
				    site.Bindings[i] = new ServerBinding(
					    (string)objBindings[i].Properties["IP"].Value,
					    (string)objBindings[i].Properties["Port"].Value,
					    (string)objBindings[i].Properties["Hostname"].Value);

			    }
		    }

		    ManagementObject objVDir = wmi.GetObject(
			    String.Format("IIsFtpVirtualDirSetting='{0}'", GetAccountPath(site.SiteId, "")));

		    // determine root content folder
		    site.ContentPath = (string)objVDir.Properties["Path"].Value; ;
	    }

	    private void FillWmiObjectFromFtpSite(ManagementBaseObject obj, FtpSite site)
	    {
		    obj.Properties["ServerComment"].Value = site.Name;
		    obj.Properties["AccessRead"].Value = site.AllowReadAccess;
		    obj.Properties["AccessScript"].Value = site.AllowScriptAccess;
		    obj.Properties["AccessSource"].Value = site.AllowSourceAccess;
		    obj.Properties["AccessWrite"].Value = site.AllowWriteAccess;
		    obj.Properties["AccessExecute"].Value = site.AllowExecuteAccess;
		    obj.Properties["AnonymousUserName"].Value = site.AnonymousUsername;
		    obj.Properties["AnonymousUserPass"].Value = site.AnonymousUserPassword;
		    obj.Properties["AllowAnonymous"].Value = site.AllowAnonymous;
		    obj.Properties["AnonymousOnly"].Value = site.AnonymousOnly;
		    obj.Properties["LogFileDirectory"].Value = site.LogFileDirectory;
	    }

		private void FillWmiObjectUNCFromFtpAccount(ManagementBaseObject obj, FtpAccount account)
		{
			if (account.Folder.StartsWith(@"\\")
				&& !String.IsNullOrEmpty(account.Name)
				&& !String.IsNullOrEmpty(account.Password))
			{
				//
				obj.SetPropertyValue("UNCUserName", GetQualifiedAccountName(account.Name));
				obj.SetPropertyValue("UNCPassword", account.Password);
			}
		}

		protected string GetQualifiedAccountName(string accountName)
		{
			if (!ServerSettings.ADEnabled)
				return accountName;

			if (accountName.IndexOf("\\") != -1)
				return accountName; // already has domain information

			// DO IT FOR ACTIVE DIRECTORY MODE ONLY
			string domainName = null;
			try
			{
				DirectoryContext objContext = new DirectoryContext(DirectoryContextType.Domain, ServerSettings.ADRootDomain);
				Domain objDomain = Domain.GetDomain(objContext);
				domainName = objDomain.Name;
			}
			catch (Exception ex)
			{
				Log.WriteError("Get domain name error", ex);
			}

			return domainName != null ? domainName + "\\" + accountName : accountName;
		}
	    #endregion

        public override bool IsInstalled()
        {
            int value = 0;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\InetStp");
            if (rk != null)
            {
                value = (int)rk.GetValue("MajorVersion", null);
                rk.Close();
            }

            RegistryKey ftp = root.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\MSFTPSVC");
            bool res = value == 6 && ftp != null;
            if (ftp != null)
                ftp.Close();

            return res;
                        
        }
    }
}
