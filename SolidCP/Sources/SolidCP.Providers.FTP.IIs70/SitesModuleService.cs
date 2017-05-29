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

using System.Collections.Generic;
using SolidCP.Providers;
using SolidCP.Providers.FTP;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.FTP.IIs70
{
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Ftp;
	using SolidCP.Providers.FTP.IIs70.Authorization;
	using SolidCP.Providers.FTP.IIs70.Config;
    using Microsoft.Web.Management.Server;
    using Microsoft.Web.Management.Utility;
    using System;
	using System.IO;
    using System.Collections;
    using System.Security.Cryptography.X509Certificates;

    internal sealed class SitesModuleService : IDisposable
    {
		private ServerManager ServerManager;

		public SitesModuleService()
		{
			ServerManager = new ServerManager();
		}

		#region Helper routines
		private void AddAuthorizationRules(string siteName, PropertyBag authBag)
        {
            PermissionsFlags permissions = (PermissionsFlags) authBag[3];
            string str = (string) authBag[1];
            string str2 = (string) authBag[2];
            AuthorizationRuleCollection authorizationRuleCollection = this.GetAuthorizationRuleCollection(siteName);
            if (!string.IsNullOrEmpty(str))
            {
                authorizationRuleCollection.Add(AuthorizationRuleAccessType.Allow, str, string.Empty, permissions);
            }
            if (!string.IsNullOrEmpty(str2))
            {
                authorizationRuleCollection.Add(AuthorizationRuleAccessType.Allow, string.Empty, str2, permissions);
            }
        }

		/// <summary>
		/// Gets authorization collection for a given site.
		/// </summary>
		/// <param name="siteName">Site's name to get authorization collection for.</param>
		/// <returns>Authorization collection.</returns>
		public AuthorizationRuleCollection GetAuthorizationRuleCollection(string siteName)
		{
			AuthorizationSection section = (AuthorizationSection)FtpHelper.GetAppHostSection(ServerManager, "system.ftpServer/security/authorization", typeof(AuthorizationSection), ManagementConfigurationPath.CreateSiteConfigurationPath(siteName));
			AuthorizationRuleCollection rules = section.Rules;
			if (rules == null)
				throw new Exception("ConfigurationError");
			return rules;
		}

        /// <summary>
        /// Removes authorization collection for a given site.
        /// </summary>
        /// <param name="siteName">Site's name to get authorization collection for.</param>
        /// <returns>Authorization collection.</returns>
        public void RemoveFtpAccountAuthSection(string accountPath)
        {
            //
            Configuration config = ServerManager.GetApplicationHostConfiguration();
            //
            config.RemoveLocationPath(accountPath);
        }
		#endregion

		public void AddSite(PropertyBag bag)
        {
			// ensure bag not empty
            if (bag == null)
                throw new ArgumentNullException("bag");
			
			// ensure site not exists
			string name = (string)bag[FtpSiteGlobals.Site_Name];
            if (ServerManager.Sites[name] != null)
				throw new Exception("SiteAlreadyExistsExceptionError");

			// ensure site path
			string directory = (string)bag[FtpSiteGlobals.AppVirtualDirectory_PhysicalPath];
			if (!Directory.Exists(FileUtils.EvaluateSystemVariables(directory)))
                throw new Exception("SiteDirectoryDoesNotExistExceptionError");
			
			// ensure site binding
            PropertyBag bag2 = (PropertyBag) bag[FtpSiteGlobals.Site_SingleBinding];
            if (bag2 == null)
				throw new ArgumentNullException("bindingBag");

			string bindingInformation = (string)bag2[FtpSiteGlobals.BindingInformation];
            
			SitesHelper.DeserializeSiteProperties(ServerManager.Sites.Add(name, "ftp", bindingInformation, directory), bag);
			// authorization
			PropertyBag authBag = (PropertyBag)bag[FtpSiteGlobals.Authorization_Rule];
            if (authBag != null)
                AddAuthorizationRules(name, authBag);

			ServerManager.CommitChanges();
            Site site = ServerManager.Sites[name];
            try
            {
                FtpSite ftpSiteElement = FtpHelper.GetFtpSiteElement(site);
				//
                if (ftpSiteElement.ServerAutoStart)
                    ftpSiteElement.Start();
            }
            catch
            {
            }
        }

        public ArrayList AddSiteBinding(string siteName, PropertyBag bindingBag)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentNullException("siteName");
            }
            if (bindingBag == null)
            {
                throw new ArgumentNullException("bindingBag");
            }
			//
            Site site = ServerManager.Sites[siteName];
            if (site == null)
                throw new Exception("SiteDoesNotExistCannotAddBindingExceptionError");

            string bindingProtocol = (string) bindingBag[0];
            string bindingInformation = (string) bindingBag[1];
            site.Bindings.Add(bindingInformation, bindingProtocol);
            ArrayList allBindings = SitesHelper.GetAllBindings(site.Bindings);

			ServerManager.CommitChanges();

            return allBindings;
        }

        public ArrayList EditSiteBinding(string siteName, PropertyBag originalBindingBag, PropertyBag newBindingBag)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentNullException("siteName");
            }
            if (originalBindingBag == null)
            {
                throw new ArgumentNullException("originalBindingBag");
            }
            if (newBindingBag == null)
            {
                throw new ArgumentNullException("newBindingBag");
            }
			//
            Site site = ServerManager.Sites[siteName];
            if (site == null)
				throw new Exception("SiteDoesNotExistCannotEditBindingExceptionError");

            string b = (string) originalBindingBag[1];
            int num = (int) originalBindingBag[2];
            string str2 = (string) newBindingBag[1];
            int num2 = 0;
            bool flag = false;
            foreach (Binding binding in site.Bindings)
            {
                if (((num == num2) && string.Equals(binding.Protocol, "ftp", StringComparison.OrdinalIgnoreCase)) && string.Equals(binding.BindingInformation, b, StringComparison.OrdinalIgnoreCase))
                {
                    binding.BindingInformation = str2;
                    flag = true;
                }
                num2++;
            }
            if (!flag)
				throw new Exception("SitesBindingDoesNotExistCannotEditBindingExceptionError");
            ArrayList allBindings = SitesHelper.GetAllBindings(site.Bindings);
            
			ServerManager.CommitChanges();
            
			return allBindings;
        }

        public void EditSiteDefaults(PropertyBag bag)
        {
            if (bag == null)
            {
                throw new ArgumentNullException("bag");
            }
            SitesHelper.DeserializeFtpSiteProperties(FtpHelper.GetFtpSiteDefaultElement(ServerManager.SiteDefaults), bag);
            ServerManager.CommitChanges();
        }

        public PropertyBag EditSiteProperties(PropertyBag bag)
        {
            if (bag == null)
            {
                throw new ArgumentNullException("bag");
            }
            string siteName = (string) bag[100];
            PropertyBag bindingBag = (PropertyBag) bag[0x68];
            if (bindingBag != null)
            {
                this.AddSiteBinding(siteName, bindingBag);
            }
            Site site = ServerManager.Sites[siteName];
            if (site == null)
				throw new Exception("SiteDoesNotExistCannotEditExceptionError");

            SitesHelper.DeserializeSiteProperties(site, bag);
            PropertyBag authBag = (PropertyBag) bag[0x1a6];
            if (authBag != null)
            {
                this.AddAuthorizationRules(siteName, authBag);
            }
			//
            ServerManager.CommitChanges();
			//
            site = ServerManager.Sites[siteName];
            try
            {
                FtpSite ftpSiteElement = FtpHelper.GetFtpSiteElement(site);
                if (ftpSiteElement.ServerAutoStart)
                {
                    ftpSiteElement.Start();
                }
            }
            catch
            {
            }
            return SitesHelper.SerializeSite(site);
        }

        public ArrayList GetCertificates()
        {
            ArrayList list = new ArrayList();
            X509Store store = null;
            try
            {
                store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.OpenExistingOnly);
                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if (FtpHelper.CanAuthenticateServer(certificate) && certificate.HasPrivateKey)
                    {
                        PropertyBag bag = new PropertyBag();
                        bag[0] = certificate.FriendlyName;
                        bag[1] = certificate.GetCertHashString();
                        bag[2] = FtpHelper.ConvertDistinguishedNameToString(certificate.IssuerName);
                        list.Add(bag);
                    }
                }
            }
            finally
            {
                store.Close();
            }
            return list;
        }

        public PropertyBag GetSite(string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentNullException(siteName);
            }
            Site site = ServerManager.Sites[siteName];
            if (site == null)
				throw new Exception("SiteDoesNotExistExceptionError");

            return SitesHelper.SerializeSite(site);
        }



        public PropertyBag GetSiteDefaults()
        {
            return SitesHelper.SerializeSiteDefaults(ManagementUnit.ReadOnlyServerManager);
        }

        public PropertyBag GetSiteProperties(string siteName)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentNullException(siteName);
            }
            Site site = ServerManager.Sites[siteName];
            if (site == null)
				throw new Exception("SiteDoesNotExistExceptionError");

            PropertyBag bag = SitesHelper.SerializeSite(site);
            SitesHelper.SerializeFtpSiteProperties(FtpHelper.GetFtpSiteElement(site), bag);
            return bag;
        }

        public ArrayList RemoveSiteBinding(string siteName, PropertyBag bindingBag)
        {
            if (string.IsNullOrEmpty(siteName))
            {
                throw new ArgumentNullException("siteName");
            }
            if (bindingBag == null)
            {
                throw new ArgumentNullException("bindingBag");
            }
            Site site = ServerManager.Sites[siteName];
            if (site == null)
				throw new Exception("SiteDoesNotExistCannotRemoveBindingExceptionError");

            string b = (string) bindingBag[1];
            int num = (int) bindingBag[2];
            int num2 = 0;
            Binding element = null;
            foreach (Binding binding2 in site.Bindings)
            {
                if (((num == num2) && string.Equals(binding2.Protocol, "ftp", StringComparison.OrdinalIgnoreCase)) && string.Equals(binding2.BindingInformation, b, StringComparison.OrdinalIgnoreCase))
                {
                    binding2.BindingInformation = b;
                    element = binding2;
                }
                num2++;
            }
            if (element == null)
				throw new Exception("SitesBindingDoesNotExistCannotRemoveBindingExceptionError");

            site.Bindings.Remove(element);
            ArrayList allBindings = SitesHelper.GetAllBindings(site.Bindings);
            //
			ServerManager.CommitChanges();

            return allBindings;
        }

		public void CommitChanges()
		{
			this.ServerManager.CommitChanges();
			this.ServerManager.Dispose();
			this.ServerManager = new ServerManager();
		}

        public void DeleteFtpAccount(string siteName, string appVirtualDirectory)
        {
            // Ensure virtual directory is in correct format
            if (!appVirtualDirectory.StartsWith("/"))
                appVirtualDirectory = String.Concat("/", appVirtualDirectory);
            //
            Site site = this.GetIisSite(siteName);
            if (site != null)
            {
                Application application = site.Applications["/"];
                if (application != null)
                {
                   VirtualDirectory directory = application.VirtualDirectories[appVirtualDirectory];
                    if (directory != null)
                    {
                        RemoveFtpAccountAuthSection(String.Format("{0}{1}", siteName, appVirtualDirectory));
                        //
                        application.VirtualDirectories.Remove(directory);
                    }
                }
            }
            //
            this.CommitChanges();
        }

		/// <summary>
		/// Deletes virtual directory with given name under site with given name.
		/// </summary>
		/// <param name="siteName">Site's name that owns virtual directory.</param>
		/// <param name="appVirtualDirectory">Virtual direcotry's name to be deleted.</param>
		public void DeleteAppVirtualDirectory(string siteName, string appVirtualDirectory)
		{
			Site site = this.GetIisSite(siteName);
			if (site != null)
			{
				Application application = site.Applications["/"];
				if (application != null)

				{
					VirtualDirectory directory = application.VirtualDirectories[appVirtualDirectory];
					if (directory != null)
					{
						application.VirtualDirectories.Remove(directory);
					}
				}
			}
			this.CommitChanges();
		}

		/// <summary>
		/// Configures connect as feature for supplied directory with specified user name and password.
		/// </summary>
		/// <param name="physicalPath">Physical path to impriove performance.</param>
		/// <param name="siteName">Site's id that owns the directory.</param>
		/// <param name="directoryName">Directory to configure connect as for.</param>
		/// <param name="username">User name.</param>
		/// <param name="password">Password.</param>
		/// <param name="commit">A value which shows whether changes should be commited.</param>
		public void ConfigureConnectAs(string physicalPath, string siteName, string directoryName, 
			string username, string password, bool commit)
		{
			if (physicalPath.StartsWith(@"\\")
				&& !String.IsNullOrEmpty(username)
				&& !String.IsNullOrEmpty(password))
			{
				Site site = this.GetIisSite(siteName);
				if (site != null)
				{
					//
					Application application = site.Applications["/"];
					if (application != null)
					{
						VirtualDirectory accountDirectory = application.VirtualDirectories[directoryName];
						if (accountDirectory != null)
						{
							accountDirectory.UserName = username;
							accountDirectory.Password = password;
							if (commit)
							{
								this.CommitChanges();
							}
						}
					}
				}
			}
		}

    	/// <summary>
		/// Creates virtual directory under site with given name and sets authorization rules.
		/// </summary>
		/// <param name="siteName">Site name.</param>
		/// <param name="account">Account information.</param>
		public void CreateFtpAccount(string siteName, FtpAccount account)
		{
			Site site = this.GetIisSite(siteName);
			if (site !=null)
			{
				Application application = site.Applications["/"];
				if (application != null)
				{
					var ftpVirtualDir = String.Format("/{0}", account.Name);
					//
					VirtualDirectory accountDirectory = application.VirtualDirectories[ftpVirtualDir];
					//
					if (accountDirectory != null)
					{
						application.VirtualDirectories.Remove(accountDirectory);
					}
					VirtualDirectory createdAppVirtualDirectory = application.VirtualDirectories.Add(ftpVirtualDir, account.Folder);


					AuthorizationRuleCollection authRulesCollection = this.GetAuthorizationRuleCollection(String.Format("{0}/{1}", siteName, account.Name));
					List<AuthorizationRule> rulesToRemove = new List<AuthorizationRule>();
					foreach (AuthorizationRule rule in authRulesCollection)
					{
						if (rule.AccessType == AuthorizationRuleAccessType.Allow && (rule.Users == "?" || rule.Users == "*"))
						{
							rulesToRemove.Add(rule);
						}
					}

					foreach(AuthorizationRule rule in rulesToRemove)
					{
						authRulesCollection.Remove(rule);
					}

					PermissionsFlags permissions = 0;
					if (account.CanRead)
					{
						permissions |= PermissionsFlags.Read;
					}
					if (account.CanWrite)
					{
						permissions |= PermissionsFlags.Write;
					}
					if (account.CanRead || account.CanWrite)
					{
						authRulesCollection.Add(AuthorizationRuleAccessType.Allow, account.Name, String.Empty, permissions);
					}
				}
			}
			this.CommitChanges();
		}

    	/// <summary>
		/// Gets list of virtual directories under site with given name.
		/// </summary>
		/// <param name="siteName">Site name under which to look for virtual directories.</param>
		/// <returns>List of virtual directories under site with given name.</returns>
		public IEnumerable<string> GetAppVirtualDirectoriesNames(string siteName)
		{
			List<string> virtualDirectoriesNames = new List<string>();
			Site site = this.GetIisSite(siteName);
			if (site !=null)
			{
				Application application = site.Applications["/"];
				if (application != null)
				{
					foreach(VirtualDirectory directory in application.VirtualDirectories)
					{
						virtualDirectoriesNames.Add(directory.Path);
					}
				}
			}

			return virtualDirectoriesNames;
		}

    	/// <summary>
		/// Checks whether virtual directory under given site exists.
		/// </summary>
		/// <param name="siteName">Site name.</param>
		/// <param name="appVirtualDirectory">Virtual directory.</param>
		/// <returns>true - if exists; false - otherwise.</returns>
		public bool AppVirtualDirectoryExists(string siteName, string appVirtualDirectory)
		{
			Site site = this.GetIisSite(siteName);
			if (site == null)
			{
				return false;
			}
			Application application = site.Applications["/"];
			if (application == null)
			{
				return false;
			}
    	    VirtualDirectory directory = appVirtualDirectory.StartsWith("/") 
                ? application.VirtualDirectories[appVirtualDirectory] : application.VirtualDirectories["/" + appVirtualDirectory];
    	    if (directory == null)
			{
				return false;
			}
			return true;
		}

    	/// <summary>
		/// Gets names of all ftp sites.
		/// </summary>
		/// <returns>Names of all ftp sites.</returns>
		public IEnumerable<string> GetSitesNames()
		{
			List<string> ftpSiteNames = new List<string>();
			// Add only ftp sites.
			foreach(Site site in this.ServerManager.Sites)
			{
				if (this.GetSiteBindings(site.Name).Length > 0)
				{
					ftpSiteNames.Add(site.Name);
				}
			}

			return ftpSiteNames;
		}

    	/// <summary>
		/// Gets physical path of the ftp site with given name.
		/// </summary>
		/// <param name="siteName">Site's name to get physical path for.</param>
		/// <param name="appVirtualDirectory">Virtual directory name.</param>
		/// <returns>physical path of the ftp site with given name.</returns>
		public string GetSitePhysicalPath(string siteName, string appVirtualDirectory)
		{
			if (this.SiteExists(siteName))
			{
				Site site = this.GetIisSite(siteName);
				if (site == null)
				{
					throw new ArgumentException("Site with given name doesn't exist.", "siteName");
				}
				Application application = site.Applications["/"];
				if (application == null)
				{
					throw new InvalidOperationException("Site with given name doesn't have root application.");
				}
				VirtualDirectory directory = application.VirtualDirectories[appVirtualDirectory];
				if (directory == null)
				{
					throw new InvalidOperationException("Site with given name doesn't have root virtual directory.");
				}
				return directory.PhysicalPath;
			}
			throw new ArgumentException("Site with given name doesn't exist.");
		}

		/// <summary>
		/// Sets physical path of the ftp site with given name.
		/// </summary>
		/// <param name="siteName">Site's name to get physical path for.</param>
		/// <param name="appVirtualDirectory">Virtual directory name.</param>
		/// <param name="physicalPath">New physical path.</param>
		public void SetSitePhysicalPath(string siteName, string appVirtualDirectory, string physicalPath)
		{
			if (this.SiteExists(siteName))
			{
				Site site = this.GetIisSite(siteName);
				if (site == null)
				{
					throw new ArgumentException("Site with given name doesn't exist.", "siteName");
				}
				Application application = site.Applications["/"];
				if (application == null)
				{
					throw new InvalidOperationException("Site with given name doesn't have root application.");
				}
				VirtualDirectory directory = application.VirtualDirectories[appVirtualDirectory];
				if (directory == null)
				{
					throw new InvalidOperationException("Site with given name doesn't have root virtual directory.");
				}
				directory.PhysicalPath = physicalPath;
			}
		}

    	/// <summary>
		/// Gets ftp site bindings.
		/// </summary>
		/// <param name="siteName">Site's name to get bindings for.</param>
		/// <returns>Ftp site bindings.</returns>
		/// <remarks>Site name must contain only default ftp site name.</remarks>
		public ServerBinding[] GetSiteBindings(string siteName)
		{
			List<ServerBinding> bindings = new List<ServerBinding>();
			if (this.SiteExists(siteName))
			{
				Site site = this.GetIisSite(siteName);
				foreach (Binding binding in site.Bindings)
				{
					// Add only ftp bindings
					if (string.Equals(binding.Protocol, "ftp", StringComparison.OrdinalIgnoreCase))
					{
						string[] parts = binding.BindingInformation.Split(':');
						bindings.Add(new ServerBinding(parts[0], parts[1], parts[2]));
					}
				}
			}
			else
			{
				throw new InvalidOperationException("Site doesn't exist.");
			}
    		return bindings.ToArray();
		}

		/// <summary>
		/// Sets ftp site bindings.
		/// </summary>
		/// <param name="siteName">Site's name to set bindings for.</param>
		/// <param name="bindings">Ftp bindings to set.</param>
		public void SetSiteBindings(string siteName, ServerBinding[] bindings)
		{
			if (this.SiteExists(siteName))
			{
				Site site = this.GetIisSite(siteName);
				List<Binding> originalBindingsToRemove = new List<Binding>();
				foreach (Binding binding in site.Bindings)
				{
					if (string.Equals(binding.Protocol, "ftp", StringComparison.OrdinalIgnoreCase))
					{
						originalBindingsToRemove.Add(binding);
					}
				}
				// Remove all ftp bindings.
				foreach (Binding binding in originalBindingsToRemove)
				{
					site.Bindings.Remove(binding);
				}
				// Add new ones.
				foreach(ServerBinding binding in bindings)
				{
					site.Bindings.Add(binding.ToString(), "ftp");
				}
			}
			else
			{
				throw new InvalidOperationException("Site doesn't exist.");
			}
		}

    	/// <summary>
		/// Checks whether site with given name exists.
		/// </summary>
		/// <param name="siteName">Site's name to check.</param>
		/// <returns>true - if it exists; false - otherwise.</returns>
		public bool SiteExists(string siteName)
		{
			return this.AppVirtualDirectoryExists(siteName, "/");
		}

		/// <summary>
		/// Deletes site with specified name.
		/// </summary>
		/// <param name="siteName">Site's name to be deleted.</param>
		public void DeleteSite(string siteName)
		{
			Site site = this.GetIisSite(siteName);
			if (site != null)
			{
				this.ServerManager.Sites.Remove(site);
				this.CommitChanges();
			}
		}

    	/// <summary>
		/// Gets state of the site with sepcified name.
		/// </summary>
		/// <param name="siteName">Site's name to get state for.</param>
		/// <returns>Site's state.</returns>
		public ServerState GetSiteState(string siteName)
		{
			FtpSite ftpSite = GetIisFtpSite(siteName);
			if (ftpSite == null)
			{
				return ServerState.Unknown;
			}
    		return ConvertSiteStateToServerState(ftpSite.State);
		}

		/// <summary>
		/// Starts site with given name.
		/// </summary>
		/// <param name="siteName">Site's name to start.</param>
		/// <returns>New site's name.</returns>
		public int StartSite(string siteName)
        {
			Site ftpSiteElement = this.GetIisSite(siteName);
			if (ftpSiteElement == null)
			{
				return (int)SiteState.Unknown;
			}
			ftpSiteElement.Start();
            return (int) ftpSiteElement.State;
        }

		/// <summary>
		/// Stops site with given name.
		/// </summary>
		/// <param name="siteName">Site's name to stop.</param>
		/// <returns>New site's name.</returns>
        public int StopSite(string siteName)
        {
			Site ftpSiteElement = this.GetIisSite(siteName);
			if (ftpSiteElement == null)
			{
				return (int)SiteState.Unknown;
			}
			ftpSiteElement.Stop();
            return (int) ftpSiteElement.State;
		}

		/// <summary>
		/// Gets ftp site with given name.
		/// </summary>
		/// <param name="siteName">Site's name.</param>
		/// <returns>Ftp site.</returns>
		public FtpSite GetIisFtpSite(string siteName)
		{
			Site site = this.GetIisSite(siteName);
			if (site == null)
			{
				throw new ArgumentException("Site with given name doesn't exist.", "siteName");
			}
			FtpSite ftpSiteElement = FtpHelper.GetFtpSiteElement(site);
			ftpSiteElement.SiteServiceId = Convert.ToString(site.Id);
			//
			return ftpSiteElement;
		}

		/// <summary>
		/// Gets site with given name.
		/// </summary>
		/// <param name="siteName">Site's name.</param>
		/// <returns>Iis site.</returns>
		public Site GetIisSite(string siteName)
		{
			if (string.IsNullOrEmpty(siteName))
			{
				throw new ArgumentNullException("siteName");
			}
			Site site = ServerManager.Sites[siteName];
			return site;
		}

		/// <summary>
		/// Converts site state to server state.
		/// </summary>
		/// <param name="siteState">Site state to convert.</param>
		/// <returns>Server state.</returns>
		private static ServerState ConvertSiteStateToServerState(SiteState siteState)
		{
			switch(siteState)
			{
				case SiteState.Started:
					return ServerState.Started;
				case SiteState.Starting:
					return ServerState.Starting;
				case SiteState.Stopped:
					return ServerState.Stopped;
				case SiteState.Stopping:
					return ServerState.Stopping;
				default:
					return ServerState.Unknown;
			}
		}


		/// <summary>
		/// Disposes underlying resources explicitly.
		/// </summary>
    	public void Dispose()
    	{
    		this.Dispose(true);
    	}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.ServerManager != null)
				{
					this.ServerManager.Dispose();
					this.ServerManager = null;
				}
			}
		}
    }
}

