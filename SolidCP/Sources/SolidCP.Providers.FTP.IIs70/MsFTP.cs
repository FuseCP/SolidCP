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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using SolidCP.Providers.FTP.IIs70;
using SolidCP.Providers.FTP.IIs70.Authorization;
using SolidCP.Providers.FTP.IIs70.Config;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.Providers.Utils.LogParser;
using SolidCP.Server.Utils;
using Microsoft.Web.Management.Server;
using Microsoft.Win32;
using IisFtpSite = SolidCP.Providers.FTP.IIs70.Config.FtpSite;
using IisSite = Microsoft.Web.Administration.Site;

namespace SolidCP.Providers.FTP
{
    public class MsFTP : HostingServiceProviderBase, IFtpServer
    {
    	private SitesModuleService ftpSitesService;
		private static readonly string DefaultFtpSiteFolder = @"%SystemDrive%\inetpub\ftproot";
		private static readonly string DefaultFtpSiteLogsFolder = @"%SystemDrive%\inetpub\logs\LogFiles";
		public const string DEFAULT_LOG_EXT_FILE_FIELDS = @"Date,Time,ClientIP,UserName,SiteName,ComputerName,
			ServerIP,Method,UriStem,FtpStatus,Win32Status,BytesSent,BytesRecv,TimeTaken,ServerPort,Host,FtpSubStatus,
			Session,FullPath,Info,ClientPort";

		private static string[] FTP70_SERVICE_CMD_LIST = new string[] {
			"DataChannelClosed",
			"DataChannelOpened",
			"ControlChannelOpened"
		};

		public const string EMPTY_LOG_FIELD = "-";

		/// <summary>
		/// Initializes a new instance of the <see cref="MsFTP"/> class.
		/// </summary>
		public MsFTP()
		{
            if (IsMsFTPInstalled())
            {
                this.ftpSitesService = new SitesModuleService();
            }
		}

    	#region Properties
        protected string SiteId
        {
            get { return ProviderSettings["SiteId"]; }
        }

		protected string SharedIP
		{
			get { return ProviderSettings["SharedIP"]; }
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

        protected string AdFtpRoot
        {
            get { return ProviderSettings["AdFtpRoot"]; }
        }

        protected Mode UserIsolationMode
        {
            get
            {
                var site = GetSite(ProviderSettings["SiteId"]);
                return (Mode)Enum.Parse(typeof(Mode), site["UserIsolationMode"]);
            }
        }
        #endregion

        #region IFtpServer Members

		/// <summary>
		/// Changes site's state.
		/// </summary>
		/// <param name="siteId">Site's id to change state for.</param>
		/// <param name="state">State to be set.</param>
		/// <exception cref="ArgumentException">Is thrown in case site name is null or empty.</exception>
        public void ChangeSiteState(string siteId, ServerState state)
        {
			if (String.IsNullOrEmpty(siteId))
			{
				throw new ArgumentException("Site name is null or empty.");
			}

			switch (state)
			{
				case ServerState.Continuing:
				case ServerState.Started:
					this.ftpSitesService.StartSite(siteId);
					break;
				case ServerState.Stopped:
				case ServerState.Paused:
					this.ftpSitesService.StopSite(siteId);
					break;
			}
        }

		/// <summary>
		/// Gets state for ftp site with supplied id.
		/// </summary>
		/// <param name="siteId">Site's id to get state for.</param>
		/// <returns>Ftp site's state.</returns>
		/// <exception cref="ArgumentException">Is thrown in case site name is null or empty.</exception>
		public ServerState GetSiteState(string siteId)
        {
			if (String.IsNullOrEmpty(siteId))
			{
				throw new ArgumentException("Site name is null or empty.");
			}

			return this.ftpSitesService.GetSiteState(siteId);
        }

		/// <summary>
		/// Checks whether site with given name exists.
		/// </summary>
		/// <param name="siteId">Site's name to check.</param>
		/// <returns>true - if it exists; false - otherwise.</returns>
		/// <exception cref="ArgumentException">Is thrown in case site name is null or empty.</exception>
		public bool SiteExists(string siteId)
        {
			if (String.IsNullOrEmpty(siteId))
			{
				throw new ArgumentException("Site name is null or empty.");
			}
			// In case site id doesn't contain default ftp site name we consider it as not existent.
			return this.ftpSitesService.SiteExists(siteId);
        }

		/// <summary>
		/// Gets list of available ftp sites.
		/// </summary>
		/// <returns>List of available ftp sites.</returns>
        public FtpSite[] GetSites()
        {
			List<FtpSite> ftpSites = new List<FtpSite>();

			foreach (string ftpSiteName in this.ftpSitesService.GetSitesNames())
			{
				ftpSites.Add(this.GetSite(ftpSiteName));
			}

			return ftpSites.ToArray();
        }

		/// <summary>
		/// Gets ftp site with given name.
		/// </summary>
		/// <param name="siteId">Ftp site's name to get.</param>
		/// <returns>Ftp site.</returns>
		/// <exception cref="ArgumentException"> Is thrown in case site name is null or empty. </exception>
		public FtpSite GetSite(string siteId)
        {
			if (String.IsNullOrEmpty(siteId))
			{
				throw new ArgumentException("Site name is null or empty.");
			}

            FtpSite ftpSite = new FtpSite();
			ftpSite.SiteId = siteId;
			ftpSite.Name = siteId;
			this.FillFtpSiteFromIis(ftpSite);
			
			return ftpSite;
        }

		/// <summary>
		/// Creates ftp site.
		/// </summary>
		/// <param name="site">Ftp site to be created.</param>
		/// <returns>Created site id.</returns>
		/// <exception cref="ArgumentNullException">Is thrown in case supplied argument is null.</exception>
		/// <exception cref="ArgumentException">
		/// Is thrown in case site id or its name is null or empty or if site id is not equal to default ftp site name.
		/// </exception>
        public string CreateSite(FtpSite site)
        {
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}

			if (String.IsNullOrEmpty(site.SiteId) || String.IsNullOrEmpty(site.Name))
			{
				throw new ArgumentException("Site id or name is null or empty.");
			}

			this.CheckFtpServerBindings(site);

			PropertyBag siteBag = this.ftpSitesService.GetSiteDefaults();
			// Set site name
			siteBag[FtpSiteGlobals.Site_Name] = site.Name;
			// Set site physical path
			siteBag[FtpSiteGlobals.AppVirtualDirectory_PhysicalPath] = site.ContentPath;
			PropertyBag ftpBinding = new PropertyBag();
			// Set site binding protocol
			ftpBinding[FtpSiteGlobals.BindingProtocol] = "ftp";
			// fill binding summary info
			ftpBinding[FtpSiteGlobals.BindingInformation] = site.Bindings[0].ToString();

			// Set site binding
			siteBag[FtpSiteGlobals.Site_SingleBinding] = ftpBinding;

			// Auto-start
			siteBag[FtpSiteGlobals.FtpSite_AutoStart] = true;

			// Set anonumous authentication
			siteBag[FtpSiteGlobals.Authentication_AnonymousEnabled] = true;
			siteBag[FtpSiteGlobals.Authentication_BasicEnabled] = true;

			this.ftpSitesService.AddSite(siteBag);

			AuthorizationRuleCollection rules =  this.ftpSitesService.GetAuthorizationRuleCollection(site.Name);
			rules.Add(AuthorizationRuleAccessType.Allow, "*", String.Empty, PermissionsFlags.Read);

			IisFtpSite iisFtpSite = this.ftpSitesService.GetIisFtpSite(site.Name);
			iisFtpSite.UserIsolation.Mode = Mode.StartInUsersDirectory;
			iisFtpSite.Security.Ssl.ControlChannelPolicy = ControlChannelPolicy.SslAllow;
			iisFtpSite.Security.Ssl.DataChannelPolicy = DataChannelPolicy.SslAllow;

			this.FillIisFromFtpSite(site);

			this.ftpSitesService.CommitChanges();

			// Do not start the site because it is started during creation.
			try
			{
				this.ChangeSiteState(site.Name, ServerState.Started);
			}
			catch
			{
				// Ignore the error if happened.
			}
			return site.Name;
        }

		/// <summary>
		/// Updates site with given information.
		/// </summary>
		/// <param name="site">Ftp site.</param>
        public void UpdateSite(FtpSite site)
        {
			// Check server bindings.
			CheckFtpServerBindings(site);

			this.FillIisFromFtpSite(site);

			this.ftpSitesService.CommitChanges();
        }

		/// <summary>
		/// Deletes site with specified name.
		/// </summary>
		/// <param name="siteId">Site's name to be deleted.</param>
        public void DeleteSite(string siteId)
        {
           this.ftpSitesService.DeleteSite(siteId);
        }

        /// <summary>
        /// Checks whether account with given name exists.
        /// </summary>
        /// <param name="accountName">Account name to check.</param>
        /// <returns>true - if it exists; false - otherwise.</returns>
        public bool AccountExists(string accountName)
        {
            if (String.IsNullOrEmpty(accountName))
            {
                return false;
            }

            switch (UserIsolationMode)
            {
                case Mode.ActiveDirectory:
                    return SecurityUtils.UserExists(accountName, ServerSettings, UsersOU);

                default:
                    // check acocunt on FTP server
                    bool ftpExists = this.ftpSitesService.AppVirtualDirectoryExists(this.SiteId, accountName);

                    // check account in the system
                    bool systemExists = SecurityUtils.UserExists(accountName, ServerSettings, UsersOU);
                    return (ftpExists || systemExists);
            }
        }

        /// <summary>
		/// Gets available ftp accounts.
		/// </summary>
		/// <returns>List of avaialble accounts.</returns>
        public FtpAccount[] GetAccounts()
        {
            switch (UserIsolationMode)
            {
                case Mode.ActiveDirectory:
                    return SecurityUtils.GetUsers(ServerSettings, UsersOU).Select(GetAccount).ToArray();
                default:
                    List<FtpAccount> accounts = new List<FtpAccount>();

                    foreach (string directory in this.ftpSitesService.GetAppVirtualDirectoriesNames(this.SiteId))
                    {
                        // Skip root virtual directory
                        if (String.Equals(directory, "/"))
                            continue;
                        //
                        accounts.Add(this.GetAccount(directory.Substring(1)));
                    }

                    return accounts.ToArray();
            }
        }

        /// <summary>
        /// Gets account with given name.
        /// </summary>
        /// <param name="accountName">Account's name to get.</param>
        /// <returns>Ftp account.</returns>
        public FtpAccount GetAccount(string accountName)
        {
            switch (UserIsolationMode)
            {
                case Mode.ActiveDirectory:
                    var user = SecurityUtils.GetUser(accountName, ServerSettings, UsersOU);

                    var path = Path.Combine(user.MsIIS_FTPRoot, user.MsIIS_FTPDir);
                    var permission = GetUserPermission(accountName, path);
                    var account = new FtpAccount()
                    {
                        CanRead = permission.Read,
                        CanWrite = permission.Write,
                        Enabled = !user.AccountDisabled,
                        Folder = path,
                        Name = accountName
                    };

                    return account;
                default:
                    FtpAccount acc = new FtpAccount();
                    acc.Name = accountName;
                    this.FillFtpAccountFromIis(acc);
                    return acc;
            }
        }

        protected UserPermission GetUserPermission(string accountName, string folder)
        {
            var userPermission = new UserPermission {AccountName = accountName};
            return SecurityUtils.GetGroupNtfsPermissions(folder, new[] {userPermission}, ServerSettings, UsersOU, GroupsOU)[0];
        }


        /// <summary>
		/// Creates ftp account under root ftp site.
		/// </summary>
		/// <param name="account">Ftp account to create.</param>
        public void CreateAccount(FtpAccount account)
        {
            switch (UserIsolationMode)
            {
                case Mode.ActiveDirectory:
                    SecurityUtils.EnsureOrganizationalUnitsExist(ServerSettings, UsersOU, GroupsOU);

                    var systemUser = SecurityUtils.GetUser(account.Name, ServerSettings, UsersOU);

                    if (systemUser == null)
                    {
                        systemUser = new SystemUser
                        {
                            Name = account.Name,
                            FullName = account.Name,
                            Password = account.Password,
                            PasswordCantChange = true,
                            PasswordNeverExpires = true,
                            System = true
                        };

                        SecurityUtils.CreateUser(systemUser, ServerSettings, UsersOU, GroupsOU);
                    }

                    UpdateAccount(account);

                    break;

                default:
                    // Create user account.
                    SystemUser user = new SystemUser();
                    user.Name = account.Name;
                    user.FullName = account.Name;
                    user.Description = "SolidCP System Account";
                    user.MemberOf = new string[] {FtpGroupName};
                    user.Password = account.Password;
                    user.PasswordCantChange = true;
                    user.PasswordNeverExpires = true;
                    user.AccountDisabled = !account.Enabled;
                    user.System = true;

                    // Create in the operating system.
                    if (SecurityUtils.UserExists(user.Name, ServerSettings, UsersOU))
                    {
                        SecurityUtils.DeleteUser(user.Name, ServerSettings, UsersOU);
                    }
                    SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);

                    // Prepare account's home folder.
                    this.EnsureUserHomeFolderExists(account.Folder, account.Name, account.CanRead, account.CanWrite);

                    // Future account will be given virtual directory under default ftp web site.
                    this.ftpSitesService.CreateFtpAccount(this.SiteId, account);
                    //
                    this.ftpSitesService.ConfigureConnectAs(account.Folder, this.SiteId, account.VirtualPath,
                        this.GetQualifiedAccountName(account.Name), account.Password, true);
                    break;
            }
        }

        /// <summary>
        /// Updates ftp account.
        /// </summary>
        /// <param name="account">Accoun to update.</param>
        public void UpdateAccount(FtpAccount account)
        {
            var user = SecurityUtils.GetUser(account.Name, ServerSettings, UsersOU);

            switch (UserIsolationMode)
            {
                case Mode.ActiveDirectory:
                    var ftpRoot = AdFtpRoot.ToLower();
                    var ftpDir = account.Folder.ToLower().Replace(ftpRoot, "");

                    var oldDir = user.MsIIS_FTPDir;

                    user.Password = account.Password;
                    user.PasswordCantChange = true;
                    user.PasswordNeverExpires = true;
                    user.Description = "SolidCP FTP Account with AD User Isolation";
                    user.MemberOf = new[] {FtpGroupName};
                    user.AccountDisabled = !account.Enabled;
                    user.MsIIS_FTPRoot = ftpRoot;
                    user.MsIIS_FTPDir = ftpDir;
                    user.System = true;

                    SecurityUtils.UpdateUser(user, ServerSettings, UsersOU, GroupsOU);

                    // Set NTFS permissions
                    var userPermission = GetUserPermission(account.Name, account.Folder);

                    // Do we need to change the NTFS permissions? i.e. is users home dir changed or are permissions changed?
                    if (oldDir != ftpDir || account.CanRead != userPermission.Read || account.CanWrite != userPermission.Write)
                    {
                        // First get sid of user account
                        var sid = SecurityUtils.GetAccountSid(account.Name, ServerSettings, UsersOU, GroupsOU);

                        // Remove the permissions set for this account on previous folder
                        SecurityUtils.RemoveNtfsPermissionsBySid(Path.Combine(ftpRoot, oldDir), sid);

                        // If no permissions is to be set, exit
                        if (!account.CanRead && !account.CanWrite)
                        {
                            return;
                        }

                        // Add the new permissions
                        var ntfsPermissions = account.CanRead ? NTFSPermission.Read : NTFSPermission.Write;
                        if (account.CanRead && account.CanWrite)
                        {
                            ntfsPermissions = NTFSPermission.Modify;
                        }

                        SecurityUtils.GrantNtfsPermissionsBySid(account.Folder, sid, ntfsPermissions, true, true);
                    }
                    break;

                default:

                    // Change user account state and password (if required).
                    user.Password = account.Password;
                    user.AccountDisabled = !account.Enabled;
                    SecurityUtils.UpdateUser(user, ServerSettings, UsersOU, GroupsOU);
                    // Update iis configuration.
                    this.FillIisFromFtpAccount(account);
                    break;
            }
        }

        /// <summary>
        /// Deletes account with given name.
        /// </summary>
        /// <param name="accountName">Account's name to be deleted.</param>
        public void DeleteAccount(string accountName)
        {
            switch (UserIsolationMode)
            {
                case Mode.ActiveDirectory:
                    var account = GetAccount(accountName);

                    // Remove the NTFS permissions first
                    SecurityUtils.RemoveNtfsPermissions(account.Folder, account.Name, ServerSettings, UsersOU, GroupsOU);

                    if (SecurityUtils.UserExists(accountName, ServerSettings, UsersOU))
                    {
                        SecurityUtils.DeleteUser(accountName, ServerSettings, UsersOU);
                    }
                    break;

                default:
                    string appVirtualDirectory = String.Format("/{0}", accountName);
                    string currentPhysicalPath = this.ftpSitesService.GetSitePhysicalPath(this.SiteId, appVirtualDirectory);

                    // Delete virtual directory
                    this.ftpSitesService.DeleteFtpAccount(this.SiteId, appVirtualDirectory);

                    this.ftpSitesService.CommitChanges();

                    // Remove permissions
                    RemoveFtpFolderPermissions(currentPhysicalPath, accountName);

                    // Delete system user account
                    if (SecurityUtils.UserExists(accountName, ServerSettings, UsersOU))
                    {
                        SecurityUtils.DeleteUser(accountName, ServerSettings, UsersOU);
                    }
                    break;
            }
        }

        /// <summary>
		/// Fills iis configuration  from ftp account.
		/// </summary>
		/// <param name="ftpAccount">Ftp account to fill from.</param>
		private void FillIisFromFtpAccount(FtpAccount ftpAccount)
		{
			// Remove permissions if required.
			string currentPhysicalPath = this.ftpSitesService.GetSitePhysicalPath(this.SiteId, String.Format("/{0}", ftpAccount.Name));
			if (String.Compare(currentPhysicalPath, ftpAccount.Folder, true) != 0)
			{
				RemoveFtpFolderPermissions(currentPhysicalPath, ftpAccount.Name);
			}

			// Set new permissions
			EnsureUserHomeFolderExists(ftpAccount.Folder, ftpAccount.Name, ftpAccount.CanRead, ftpAccount.CanWrite);
			// Update physical path.
			this.ftpSitesService.SetSitePhysicalPath(this.SiteId, ftpAccount.VirtualPath, ftpAccount.Folder);
			
			// Configure connect as feature.
			this.ftpSitesService.ConfigureConnectAs(ftpAccount.Folder, this.SiteId, ftpAccount.VirtualPath, 
				this.GetQualifiedAccountName(ftpAccount.Name), ftpAccount.Password, false);

			// Update authorization rules.
			AuthorizationRuleCollection authRulesCollection = this.ftpSitesService.GetAuthorizationRuleCollection(String.Format("{0}/{1}", this.SiteId, ftpAccount.Name));
			AuthorizationRule realtedRule = null;
			foreach(AuthorizationRule rule in authRulesCollection)
			{
				IList<string> users = rule.Users.Split(',');
				if (users.Contains(ftpAccount.Name))
				{
					realtedRule = rule;
				}
			}
			if (realtedRule != null)
			{
				PermissionsFlags permissions = 0;
				if (ftpAccount.CanRead)
				{
					permissions |= PermissionsFlags.Read;
				}
				if (ftpAccount.CanWrite)
				{
					permissions |= PermissionsFlags.Write;
				}
				if (ftpAccount.CanRead || ftpAccount.CanWrite)
				{
					realtedRule.Permissions = permissions;
				}
			}

			this.ftpSitesService.CommitChanges();
		}

    	/// <summary>
		/// Fills ftp account from iis configuration.
		/// </summary>
		/// <param name="ftpAccount">Ftp account to fill.</param>
		private void FillFtpAccountFromIis(FtpAccount ftpAccount)
		{
            //
			ftpAccount.Folder = this.ftpSitesService.GetSitePhysicalPath(this.SiteId, String.Format("/{0}", ftpAccount.Name));

			AuthorizationRuleCollection authRulesCollection = this.ftpSitesService.GetAuthorizationRuleCollection(String.Format("{0}/{1}", this.SiteId, ftpAccount.Name));
			ftpAccount.CanRead = false;
			ftpAccount.CanWrite = false;
			foreach(AuthorizationRule rule in authRulesCollection)
			{
				if (rule.AccessType == AuthorizationRuleAccessType.Allow)
				{
					foreach(string userName in rule.Users.Split(','))
					{
						if (String.Compare(userName, ftpAccount.Name, true) == 0)
						{
							ftpAccount.CanWrite = (rule.Permissions & PermissionsFlags.Write) == PermissionsFlags.Write;
							ftpAccount.CanRead = (rule.Permissions & PermissionsFlags.Read) == PermissionsFlags.Read;
						}
					}
				}
			}

			// Load user account.
			SystemUser user = SecurityUtils.GetUser(ftpAccount.Name, ServerSettings, UsersOU);
			if (user != null)
			{
				ftpAccount.Enabled = !user.AccountDisabled;
			}
		}

    	/// <summary>
		/// Fills ftp site with data from iis ftp site.
		/// </summary>
		/// <param name="ftpSite">Ftp site to fill.</param>
		private void FillFtpSiteFromIis(FtpSite ftpSite)
		{
			IisFtpSite iisFtpSite = this.ftpSitesService.GetIisFtpSite(ftpSite.SiteId);
			if (iisFtpSite != null)
			{
				// Security settings.
				ftpSite.AllowAnonymous = iisFtpSite.Security.Authentication.AnonymousAuthentication.Enabled;
				ftpSite.AnonymousUsername = iisFtpSite.Security.Authentication.AnonymousAuthentication.UserName;
				ftpSite.AnonymousUserPassword = iisFtpSite.Security.Authentication.AnonymousAuthentication.Password;
			    ftpSite["UserIsolationMode"] = iisFtpSite.UserIsolation.Mode.ToString();
				// Logging settings.
				ftpSite[FtpSite.MSFTP7_SITE_ID] = iisFtpSite.SiteServiceId;
				if (iisFtpSite.LogFile.Enabled)
				{
					ftpSite.LogFileDirectory = iisFtpSite.LogFile.Directory;
					ftpSite[FtpSite.MSFTP7_LOG_EXT_FILE_FIELDS] = iisFtpSite.LogFile.LogExtFileFlags.ToString();
				}
			}
			// Bindings
			ftpSite.Bindings = this.ftpSitesService.GetSiteBindings(ftpSite.SiteId);
			// Physical path
			ftpSite.ContentPath = this.ftpSitesService.GetSitePhysicalPath(ftpSite.SiteId, "/");
		}

		/// <summary>
		/// Fills iis configuration with information from ftp site.
		/// </summary>
		/// <param name="ftpSite">Ftp site that holds information.</param>
		private void FillIisFromFtpSite(FtpSite ftpSite)
		{
			IisFtpSite iisFtpSite = this.ftpSitesService.GetIisFtpSite(ftpSite.SiteId);
			string logExtFileFields = ftpSite[FtpSite.MSFTP7_LOG_EXT_FILE_FIELDS];
			
			if (iisFtpSite != null)
			{
				// Security settings.
				iisFtpSite.Security.Authentication.AnonymousAuthentication.Enabled = ftpSite.AllowAnonymous;
				iisFtpSite.Security.Authentication.AnonymousAuthentication.UserName = ftpSite.AnonymousUsername;
				iisFtpSite.Security.Authentication.AnonymousAuthentication.Password = ftpSite.AnonymousUserPassword;
				// enable logging
				iisFtpSite.LogFile.Enabled = true;
				// set logging fields
				if (!String.IsNullOrEmpty(logExtFileFields))
					iisFtpSite.LogFile.LogExtFileFlags = (FtpLogExtFileFlags)Enum.Parse(typeof(FtpLogExtFileFlags), logExtFileFields);
				// set log files directory
				if (!String.IsNullOrEmpty(ftpSite.LogFileDirectory))
					iisFtpSite.LogFile.Directory = ftpSite.LogFileDirectory;
			}
			// Set new bindings.
			this.CheckFtpServerBindings(ftpSite);
			this.ftpSitesService.SetSiteBindings(ftpSite.Name, ftpSite.Bindings);
			// Physical path
			this.ftpSitesService.SetSitePhysicalPath(ftpSite.SiteId, "/", ftpSite.ContentPath);
		}

		/// <summary>
		/// Ensures that home folder for ftp account exists.
		/// </summary>
		/// <param name="folder">Path to home folder.</param>
		/// <param name="accountName">Account name.</param>
		/// <param name="allowRead">A value which specifies whether read operation is allowed or not.</param>
		/// <param name="allowWrite">A value which specifies whether write operation is allowed or not.</param>
		private void EnsureUserHomeFolderExists(string folder, string accountName, bool allowRead, bool allowWrite)
		{
			// create folder
			if (!FileUtils.DirectoryExists(folder))
			{
				FileUtils.CreateDirectory(folder);
			}

			if (!allowRead && !allowWrite)
			{
				return;
			}

			NTFSPermission permissions = allowRead ? NTFSPermission.Read : NTFSPermission.Write;

			if (allowRead && allowWrite)
			{
				permissions = NTFSPermission.Modify;
			}

			// Set ntfs permissions
			SecurityUtils.GrantNtfsPermissions(folder, accountName, permissions, true, true,
				ServerSettings, UsersOU, GroupsOU);
		}

		/// <summary>
		/// Removes user specific permissions from folder.
		/// </summary>
		/// <param name="path">Folder to operate on.</param>
		/// <param name="accountName">User's name.</param>
		private void RemoveFtpFolderPermissions(string path, string accountName)
		{
			if (!FileUtils.DirectoryExists(path))
			{
				return;
			}

			// Anonymous account
			SecurityUtils.RemoveNtfsPermissions(path, accountName, ServerSettings, UsersOU, GroupsOU);
		}

		/// <summary>
		/// Checks if bindings listed in given site already in use.
		/// </summary>
		/// <param name="site">Site to check.</param>
		/// <exception cref="InvalidOperationException">Is thrown in case supplied site contains bindings that are already in use.</exception>
		private void CheckFtpServerBindings(FtpSite site)
		{
			if (this.IsFtpServerBindingsInUse(site))
			{
				throw new InvalidOperationException("Some of ftp site's bindings are already in use.");
			}
		}

		/// <summary>
		/// Gets a value which shows whether supplied site contains bindings that are already in use.
		/// </summary>
		/// <param name="site">Site to check.</param>
		/// <returns>true - if any of supplied bindinds is in use; false -otherwise.</returns>
		private bool IsFtpServerBindingsInUse(FtpSite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}

			// check for server bindings
			foreach (FtpSite existentSite in this.GetSites())
			{
				if (existentSite.Name != site.Name)
				{
					foreach (ServerBinding usedBinding in existentSite.Bindings)
					{
						foreach (ServerBinding requestedBinding in site.Bindings)
						{
							if (usedBinding.IP == requestedBinding.IP && usedBinding.Port == usedBinding.Port)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

    	/// <summary>
		/// Gets fully qualified name with respect to enabled active directory.
		/// </summary>
		/// <param name="accountName">Account name.</param>
		/// <returns>Fully qualified acount/domain name.</returns>
		private string GetQualifiedAccountName(string accountName)
		{
			if (!ServerSettings.ADEnabled)
			{
				return accountName;
			}

			if (accountName.IndexOf("\\") != -1)
			{
				return accountName; // already has domain information
			}

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

        #region IHostingServiceProvier methods
		/// <summary>
		/// Installs Ftp7 provider.
		/// </summary>
		/// <returns>Error messages.</returns>
        public override string[] Install()
        {
            List<string> messages = new List<string>();

			FtpSite site = null;
			string folder = FileUtils.EvaluateSystemVariables(DefaultFtpSiteFolder);
			string logsDirectory = FileUtils.EvaluateSystemVariables(DefaultFtpSiteLogsFolder);
			// Create site folder.
			if (!FileUtils.DirectoryExists(folder))
			{
				FileUtils.CreateDirectory(folder);
			}
			// Create logs folder.
			if (!FileUtils.DirectoryExists(logsDirectory))
			{
				FileUtils.CreateDirectory(logsDirectory);
			}

			site = new FtpSite();

			site.Name = this.SiteId;
			site.SiteId = this.SiteId;
			site.ContentPath = DefaultFtpSiteFolder;
			site.Bindings = new ServerBinding[1];
			// set default log directory
			site.LogFileDirectory = DefaultFtpSiteLogsFolder;
			// set default logging fields
			site[FtpSite.MSFTP7_LOG_EXT_FILE_FIELDS] = DEFAULT_LOG_EXT_FILE_FIELDS;

			if (!String.IsNullOrEmpty(this.SharedIP))
			{
				site.Bindings[0] = new ServerBinding(this.SharedIP, "21", String.Empty);
			}
			else
			{
				site.Bindings[0] = new ServerBinding("*", "21", "*");
				//// Get information on local server.
				//IPHostEntry localServerHostEntry = Dns.GetHostEntry(Dns.GetHostName());
				//foreach (IPAddress address in localServerHostEntry.AddressList)
				//{
				//    if (address.AddressFamily == AddressFamily.InterNetwork)
				//    {
				//        site.Bindings[0] = new ServerBinding(address.ToString(), "21", String.Empty);
				//    }
				//}
			}

			if (this.IsFtpServerBindingsInUse(site))
			{
				messages.Add("Cannot create ftp site because requested bindings are already in use.");
				return messages.ToArray();
			}

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

            	if (!this.ftpSitesService.SiteExists(this.SiteId))
				{
						this.CreateSite(site);
				}
				else
				{
					this.UpdateSite(site);
				}
            	
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
            FtpSite ftpSite = GetSite(SiteId);
			string siteId = String.Concat("FTPSVC", ftpSite[FtpSite.MSFTP7_SITE_ID]);
			string logsPath = Path.Combine(ftpSite.LogFileDirectory, siteId);

            // create parser object
            // and update statistics
            LogParser parser = new LogParser("Ftp", siteId, logsPath, "s-sitename", "cs-username");
			// Subscribe to the events because FTP 7.0 has several differences that should be taken into account
			// and processed in a specific way
			parser.ProcessKeyFields += new ProcessKeyFieldsEventHandler(LogParser_ProcessKeyFields);
			parser.CalculateStatisticsLine += new CalculateStatsLineEventHandler(LogParser_CalculateStatisticsLine);
			// 
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

		#region LogParser event handlers and helper routines

		private bool IsFtpServiceCommand(string command)
		{
			return (Array.IndexOf(FTP70_SERVICE_CMD_LIST, command) > -1);
		}

		private void LogParser_ProcessKeyFields(string[] key_fields, string[] key_values, string[] log_fields,
			string[] log_values)
		{
			int cs_method = Array.IndexOf(log_fields, "cs-method");
			int cs_uri_stem = Array.IndexOf(log_fields, "cs-uri-stem");
			int cs_username = Array.IndexOf(key_fields, "cs-username");
			//
			if (cs_username > -1)
			{
				string valueStr = EMPTY_LOG_FIELD;
				// this trick allows to calculate USER command bytes as well
				// in spite that "cs-username" field is empty for the command
				if (key_values[cs_username] != EMPTY_LOG_FIELD)
					valueStr = key_values[cs_username];
				else if (cs_method > -1 && cs_uri_stem > -1 && log_values[cs_method] == "USER")
					valueStr = log_values[cs_uri_stem];
				//
				key_values[cs_username] = valueStr.Substring(valueStr.IndexOf(@"\") + 1);
			}
		}

		private void LogParser_CalculateStatisticsLine(StatsLine line, string[] fields, string[] values)
		{
			int cs_method = Array.IndexOf(fields, "cs-method");
			// bandwidth calculation ignores FTP 7.0 serviced commands
			if (cs_method > -1 && !IsFtpServiceCommand(values[cs_method]))
			{
				int cs_bytes = Array.IndexOf(fields, "cs-bytes");
				int sc_bytes = Array.IndexOf(fields, "sc-bytes");
				// skip empty cs-bytes value processing
				if (cs_bytes > -1 && values[cs_bytes] != "0")
					line.BytesReceived += Int64.Parse(values[cs_bytes]);
				// skip empty sc-bytes value processing
				if (sc_bytes > -1 && values[sc_bytes] != "0")
					line.BytesSent += Int64.Parse(values[sc_bytes]);
			}
		}

		#endregion

        protected virtual bool IsMsFTPInstalled()
        {
            int value = 0;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\InetStp");
            if (rk != null)
            {
                value = (int)rk.GetValue("MajorVersion", null);
                rk.Close();
            }

            RegistryKey ftp = root.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\ftpsvc");
            bool res = (value == 7) && ftp != null;
            if (ftp != null)
                ftp.Close();

            return res;
        }
        
        public override bool IsInstalled()
        {
            return IsMsFTPInstalled();
        }
	}
}
