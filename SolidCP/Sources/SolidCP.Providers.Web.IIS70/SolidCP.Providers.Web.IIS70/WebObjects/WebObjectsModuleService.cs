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

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SolidCP.Providers.Web.Iis.WebObjects
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using Common;
    using Utility;
    using SolidCP.Providers.Utils;

    using Microsoft.Web.Management;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
	using SolidCP.Providers.Web.Iis.Authentication;

    internal sealed class WebObjectsModuleService : ConfigurationModuleService
    {
		

		public void SetWebServerDefaultLoggingSettings(LogExtFileFlags svrLoggingFlags)
		{
			using (var srvman = GetServerManager())
			{
				// Update logging settings
				srvman.SiteDefaults.LogFile.LogExtFileFlags |= svrLoggingFlags;
				//
				srvman.CommitChanges();
			}
		}

		public void SetWebSiteLoggingSettings(WebSite webSite)
		{
			using (var srvman = GetServerManager())
			{
				var iisObject = srvman.Sites[webSite.SiteId];
				// Website logging is enabled by default
				iisObject.LogFile.Enabled = true;
				// Set website logs folder
				if (!String.IsNullOrEmpty(webSite.LogsPath))
					iisObject.LogFile.Directory = webSite.LogsPath;
				//
				srvman.CommitChanges();
			}
		}

		public string[] GrantConfigurationSectionAccess(string[] sections)
		{
			List<string> messages = new List<string>();
			//
			if (sections != null && sections.Length > 0)
			{
				foreach (string sectionName in sections)
				{
					try
					{
						string cmd = FileUtils.EvaluateSystemVariables(@"%windir%\system32\inetsrv\appcmd.exe");
						//
						FileUtils.ExecuteSystemCommand(cmd,
							String.Format("unlock config -section:{0}", sectionName));
					}
					catch (Exception ex)
					{
						messages.Add(String.Format("Could not unlock section '{0}'. Reason: {1}",
							sectionName, ex.StackTrace));
					}
				}
			}
			//
			return messages.ToArray();
		}

        public void ConfigureConnectAsFeature(WebVirtualDirectory virtualDir)
        {
            // read website
            using (var srvman = GetServerManager())
            {
                var webSite = String.IsNullOrEmpty(virtualDir.ParentSiteName) ? srvman.Sites[virtualDir.Name] : srvman.Sites[virtualDir.ParentSiteName];
                //
                if (webSite != null)
                {
                    // get root iisAppObject
                    var webApp = webSite.Applications[virtualDir.VirtualPath];
                    //
                    if (webApp != null)
                    {
                        var vdir = webApp.VirtualDirectories["/"];
                        //
                        if (vdir != null)
                        {
                            vdir.LogonMethod = AuthenticationLogonMethod.ClearText;
                            //
                            
                                vdir.UserName = virtualDir.AnonymousUsername;
                                vdir.Password = virtualDir.AnonymousUserPassword;

                            //
                            srvman.CommitChanges();
                        }
                    }
                }
            }
        }


        public void ConfigureConnectAsFeature(WebAppVirtualDirectory virtualDir)
		{
			// read website
			using (var srvman = GetServerManager())
			{
				var webSite = String.IsNullOrEmpty(virtualDir.ParentSiteName) ? srvman.Sites[virtualDir.Name] : srvman.Sites[virtualDir.ParentSiteName];
				//
				if (webSite != null)
				{
					// get root iisAppObject
					var webApp = webSite.Applications[virtualDir.VirtualPath];
					//
					if (webApp != null)
					{
						var vdir = webApp.VirtualDirectories["/"];
						//
						if (vdir != null)
						{
							vdir.LogonMethod = AuthenticationLogonMethod.ClearText;
							//
							if (virtualDir.DedicatedApplicationPool)
							{
								var appPool = GetApplicationPool(srvman, virtualDir);
								vdir.UserName = appPool.ProcessModel.UserName;
								vdir.Password = appPool.ProcessModel.Password;
							}
							else
							{
								vdir.UserName = virtualDir.AnonymousUsername;
								vdir.Password = virtualDir.AnonymousUserPassword;
							}
							//
							srvman.CommitChanges();
						}
					}
				}
			}
		}

        // AppPool
        public bool IsApplicationPoolExist(string poolName)
        {
            if (String.IsNullOrEmpty(poolName))
                throw new ArgumentNullException("poolName");
            //
            using (var srvman = GetServerManager())
            {
                return (srvman.ApplicationPools[poolName] != null);
            }
        }

        public void ForceEnableAppPoolWow6432Mode(string poolName)
        {
            using (var srvman = GetServerManager())
            {
                var appPool = srvman.ApplicationPools[poolName];
                //
                if (Constants.X64Environment && appPool.Enable32BitAppOnWin64 == false)
                {
                    appPool.Enable32BitAppOnWin64 = true;
                    srvman.CommitChanges();
                }
            }
        }

        public void ChangeAppPoolState(string siteId, AppPoolState state)
        {
            using (var srvman = GetServerManager())
            {
                var site = srvman.Sites[siteId];
                //
                if (site == null)
                    return;

                foreach (Application app in site.Applications)
                {
                    string AppPoolName = app.ApplicationPoolName;

                    if (string.IsNullOrEmpty(AppPoolName))
                        continue;

                    ApplicationPool pool = srvman.ApplicationPools[AppPoolName];
                    if (pool == null) continue;

                    //
                    switch (state)
                    {
                        case AppPoolState.Started:
                        case AppPoolState.Starting:
                            if ((pool.State != ObjectState.Started) && (pool.State != ObjectState.Starting))
                            {
                                pool.Start();
                                pool.AutoStart = true;
                            }
                            break;
                        case AppPoolState.Stopped:
                        case AppPoolState.Stopping:
                            if ((pool.State != ObjectState.Stopped) && (pool.State != ObjectState.Stopping))
                            {
                                pool.Stop();
                                pool.AutoStart = false;
                            }
                            break;
                        case AppPoolState.Recycle:
                            pool.Recycle();
                            pool.AutoStart = true;
                            break;
                    }

                    srvman.CommitChanges();

                }
            }
        }

        public AppPoolState GetAppPoolState(ServerManager srvman, string siteId)
        {
            Site site = srvman.Sites[siteId];

            // ensure website exists
            if (site == null)
                return AppPoolState.Unknown;

            string AppPoolName = site.ApplicationDefaults.ApplicationPoolName;
            foreach (Application app in site.Applications)
                AppPoolName = app.ApplicationPoolName;

            if (string.IsNullOrEmpty(AppPoolName))
                return AppPoolState.Unknown;

            ApplicationPool pool = srvman.ApplicationPools[AppPoolName];

            if (pool == null) return AppPoolState.Unknown;

            AppPoolState state = AppPoolState.Unknown;

            switch (pool.State)
            {
                case ObjectState.Started:
                    state = AppPoolState.Started;
                    break;
                case ObjectState.Starting:
                    state = AppPoolState.Starting;
                    break;
                case ObjectState.Stopped:
                    state = AppPoolState.Stopped;
                    break;
                case ObjectState.Stopping:
                    state = AppPoolState.Stopping;
                    break;
            }

            return state;
        }

        public ApplicationPool GetApplicationPool(ServerManager srvman, WebAppVirtualDirectory virtualDir)
        {
            if (virtualDir == null)
                throw new ArgumentNullException("vdir");
            // read app pool
            var appPool = srvman.ApplicationPools[virtualDir.ApplicationPool];
            //
            if (appPool == null)
                throw new ApplicationException("ApplicationPoolNotFound");
            //
            return appPool;
        }

        public void CreateApplicationPool(string appPoolName, string appPoolUsername,
            string appPoolPassword, string runtimeVersion, bool enable32BitOnWin64,
            ManagedPipelineMode pipelineMode)
        {
            // ensure app pool name specified
            if (String.IsNullOrEmpty(appPoolName))
                throw new ArgumentNullException("appPoolName");

            // Create iisAppObject pool
            using (var srvman = GetServerManager())
            {
                // ensure app pool unique
                if (srvman.ApplicationPools[appPoolName] != null)
                    throw new Exception("ApplicationPoolAlreadyExists");

                var element = srvman.ApplicationPools.Add(appPoolName);
                //
                element.ManagedPipelineMode = pipelineMode;
                // ASP.NET 2.0 by default
                if (!String.IsNullOrEmpty(runtimeVersion))
                    element.ManagedRuntimeVersion = runtimeVersion;
                //
                element.Enable32BitAppOnWin64 = enable32BitOnWin64;
                // set iisAppObject pool identity
                if (!String.IsNullOrEmpty(appPoolUsername))
                {
                    element.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
                    element.ProcessModel.UserName = appPoolUsername;
                    element.ProcessModel.Password = appPoolPassword;
                }
                else
                {
                    element.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
                }
                //
                element.AutoStart = true;
                //
                srvman.CommitChanges();
            }
        }

        public void DeleteApplicationPools(params string[] appPoolNames)
        {
            using (var srvman = GetServerManager())
            {
                //
                foreach (var poolName in appPoolNames)
                {
                    // Lookup for an app pool
                    int indexOf = srvman.ApplicationPools.IndexOf(srvman.ApplicationPools[poolName]);
                    // Remove app pool if it is found
                    if (indexOf > -1)
                        srvman.ApplicationPools.RemoveAt(indexOf);
                }
                //
                srvman.CommitChanges();
            }
        }

        public void DeleteApplicationPool(params string[] appPoolNames)
        {
            using (var srvman = GetServerManager())
            {
                foreach (var item in appPoolNames)
                {
                    var indexOf = srvman.ApplicationPools.IndexOf(srvman.ApplicationPools[item]);
                    //
                    if (indexOf > -1)
                        srvman.ApplicationPools.RemoveAt(indexOf);
                }
                //
                srvman.CommitChanges();
            }
        }

        // Site
        public string CreateSite(WebSite site)
        {
            // ensure site bindings
            if (site.Bindings == null || site.Bindings.Length == 0)
                throw new ApplicationException("SiteServerBindingsEmpty");
            // ensure site name
            if (String.IsNullOrEmpty(site.Name))
                throw new ApplicationException("SiteNameEmpty");
            // ensure physical site content path
            if (String.IsNullOrEmpty(site.ContentPath))
                throw new ApplicationException("SiteContentPathEmpty");

            using (var srvman = GetServerManager())
            {
                //
                var iisObject = srvman.Sites.Add(site.Name, site.ContentPath, 80);
                //
                iisObject.Applications[0].ApplicationPoolName = site.ApplicationPool;
                //
                site.SiteId = iisObject.Name;
                //
                iisObject.ServerAutoStart = true;
                //
                srvman.CommitChanges();
                //
                return iisObject.Name;
            }
        }

        public void UpdateSite(WebSite site)
        {
            // ensure physical site content path
            if (String.IsNullOrEmpty(site.ContentPath))
                throw new Exception("SiteContentPathEmpty");
            //
            using (var srvman = GetServerManager())
            {
                //
                var iisObject = srvman.Sites[site.Name];
                //
                iisObject.Applications[0].ApplicationPoolName = site.ApplicationPool;
                //
                iisObject.Applications[0].VirtualDirectories[0].PhysicalPath = site.ContentPath;
                //
                iisObject.ServerAutoStart = true;
                //
                srvman.CommitChanges();
            }
        }

        public void ChangeSiteState(string siteId, ServerState state)
        {
            using (var srvman = GetServerManager())
            {
                var webSite = srvman.Sites[siteId];
                //
                if (webSite == null)
                    return;
                //
                switch (state)
                {
                    case ServerState.Continuing:
                    case ServerState.Started:
                        webSite.Start();
                        webSite.ServerAutoStart = true;
                        break;
                    case ServerState.Stopped:
                    case ServerState.Paused:
                        webSite.Stop();
                        webSite.ServerAutoStart = false;
                        break;
                }
                //
                srvman.CommitChanges();
            }
        }

        public ServerState GetSiteState(ServerManager srvman, string siteId)
        {
            // ensure website exists
            if (srvman.Sites[siteId] == null)
                return ServerState.Unknown;
            //
            var siteState = ServerState.Unknown;
            //
            switch (srvman.Sites[siteId].State)
            {
                case ObjectState.Started:
                    siteState = ServerState.Started;
                    break;
                case ObjectState.Starting:
                    siteState = ServerState.Starting;
                    break;
                case ObjectState.Stopped:
                    siteState = ServerState.Stopped;
                    break;
                case ObjectState.Stopping:
                    siteState = ServerState.Stopping;
                    break;
            }
            //
            return siteState;
        }
        public bool SiteExists(ServerManager srvman, string siteId)
        {
            return (srvman.Sites[siteId] != null);
        }

        public string[] GetSites(ServerManager srvman)
        {
			var iisObjects = new List<string>();
			//
			foreach (var item in srvman.Sites)
				iisObjects.Add(item.Name);
			//
			return iisObjects.ToArray();
        }

        public string GetWebSiteNameFromIIS(ServerManager srvman, string siteName)
        {
			if (srvman.Sites[siteName] != null)
				return srvman.Sites[siteName].Name;
			//
			return null;
        }

        public string GetWebSiteIdFromIIS(ServerManager srvman, string siteId, string format)
		{
			var iisObject = srvman.Sites[siteId];
			// Format string is empty
			if (String.IsNullOrEmpty(format))
				return Convert.ToString(iisObject.Id);
			//
			return String.Format(format, iisObject.Id);
		}

        public WebSite GetWebSiteFromIIS(ServerManager srvman, string siteId)
        {
			var webSite = new WebSite();
			//
			var iisObject = srvman.Sites[siteId];
			//
			webSite.SiteId = webSite.Name = iisObject.Name;
			//
			if (iisObject.LogFile.Enabled)
			{
				webSite.LogsPath = iisObject.LogFile.Directory;
				webSite[WebSite.IIS7_LOG_EXT_FILE_FIELDS] = iisObject.LogFile.LogExtFileFlags.ToString();
			}
			// Read instant website id
			webSite[WebSite.IIS7_SITE_ID] = GetWebSiteIdFromIIS(srvman, siteId, "W3SVC{0}");
			// Read web site iisAppObject pool name
			webSite.ApplicationPool = iisObject.Applications["/"].ApplicationPoolName;
			//
			return webSite;
        }

        public ServerBinding[] GetSiteBindings(ServerManager srvman, string siteId)
        {
			var iisObject = srvman.Sites[siteId];
			// get server bingings
			var bindings = new List<ServerBinding>();
			//
			foreach (var bindingObj in iisObject.Bindings)
			{
                // return only "http" bindings
                if (String.Equals(bindingObj.Protocol, Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] parts = bindingObj.BindingInformation.Split(':');
                    // append binding
                    bindings.Add(new ServerBinding(bindingObj.Protocol, parts[0], parts[1], parts[2]));
                }
			}
			//
			return bindings.ToArray();
        }

		private void SyncWebSiteBindingsChanges(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
		{
			// ensure site bindings
            if (!emptyBindingsAllowed)
            {
                if (bindings == null || bindings.Length == 0)
                    throw new Exception("SiteServerBindingsEmpty");
            }
			
			using (var srvman = GetServerManager())
			{
				var iisObject = srvman.Sites[siteId];
				//
				lock (((ICollection)iisObject.ChildElements).SyncRoot)
				{
                    // remove all "http" bindings
                    int i = 0;
                    while (i < iisObject.Bindings.Count)
                    {
                        if ((String.Equals(iisObject.Bindings[i].Protocol, Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase)) | 
                            (bindings.Length == 0))
                        {
                            iisObject.Bindings.RemoveAt(i);
                            continue;
                        }
                        else
                        {
                            i++;
                        }
                    }

					// Create HTTP bindings received
					foreach (var serverBinding in bindings)
                        {
                            var bindingInformation = String.Format("{0}:{1}:{2}", serverBinding.IP, serverBinding.Port, serverBinding.Host);
                            iisObject.Bindings.Add(bindingInformation, Uri.UriSchemeHttp);
                        }
				}
				//
				srvman.CommitChanges();
			}
		}

		public void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
		{
            using (ServerManager srvman = GetServerManager())
            {
                // Ensure web site exists
                if (!SiteExists(srvman, siteId))
                    return;
            }
			//
            SyncWebSiteBindingsChanges(siteId, bindings, emptyBindingsAllowed);
		}

        public string GetPhysicalPathNonApp(ServerManager iisManager, WebVirtualDirectory virtualDir)
        {
            string siteId = (virtualDir.ParentSiteName == null)
                ? virtualDir.Name : virtualDir.ParentSiteName;
            //
            var iisSite = iisManager.Sites[siteId];

            var applicationRoot = iisSite.Applications["/"];
            var virtualRoot = applicationRoot.VirtualDirectories[virtualDir.VirtualPath];

            return virtualRoot.PhysicalPath;

            

        }

        public string GetPhysicalPath(ServerManager srvman, WebAppVirtualDirectory virtualDir)
        {
			string siteId = (virtualDir.ParentSiteName == null) 
				? virtualDir.Name : virtualDir.ParentSiteName;
			//
			var iisObject = srvman.Sites[siteId];
				
			if (iisObject == null)
				return null;

			//
			var iisAppObject = iisObject.Applications[virtualDir.VirtualPath];

			if (iisAppObject == null)
				return null;

			//
			var iisDirObject = iisAppObject.VirtualDirectories["/"];

			if (iisDirObject == null)
				return null;

			//
			return iisDirObject.PhysicalPath;
        }


    	public void DeleteSite(string siteId)
		{
			using (var srvman = GetServerManager())
			{
                if (!SiteExists(srvman, siteId))
                    return;

				//
				var indexOf = srvman.Sites.IndexOf(srvman.Sites[siteId]);
				srvman.Sites.RemoveAt(indexOf);
				//
				srvman.CommitChanges();
			}
		}

        public WebAppVirtualDirectory[] GetZooApplications(ServerManager srvman, string siteId)
        {
            if (!SiteExists(srvman, siteId))
                return new WebAppVirtualDirectory[] { };

            var vdirs = new List<WebAppVirtualDirectory>();
            var iisObject = srvman.Sites[siteId];
            //
            foreach (var item in iisObject.Applications)
            {
                Configuration cfg = item.GetWebConfiguration();
                string location = siteId + ConfigurationUtility.GetQualifiedVirtualPath(item.Path);
                ConfigurationSection section;
                try
                {
                    section = cfg.GetSection("system.webServer/heliconZoo", location);
                }
                catch(Exception)
                {
                    // looks like Helicon Zoo is not installed, return empty array
                    return vdirs.ToArray();
                }

                if (section.GetCollection().Count > 0)
                {
                    WebAppVirtualDirectory vdir = new WebAppVirtualDirectory
                        {
                            Name = ConfigurationUtility.GetNonQualifiedVirtualPath(item.Path),
                            ContentPath = item.VirtualDirectories[0].PhysicalPath
                        };

                    ConfigurationElement zooAppElement = section.GetCollection()[0];
                    ConfigurationElementCollection envColl = zooAppElement.GetChildElement("environmentVariables").GetCollection();
                        
                    foreach (ConfigurationElement env in  envColl)
                    {
                        if ((string) env.GetAttributeValue("name") == "CONSOLE_URL")
                        {
                            vdir.ConsoleUrl = ConfigurationUtility.GetQualifiedVirtualPath(item.Path);
                            if (!vdir.ConsoleUrl.EndsWith("/"))
                            {
                                vdir.ConsoleUrl += "/";
                            }
                            vdir.ConsoleUrl += (string)env.GetAttributeValue("value");
                        }
                    }
                        
                    vdirs.Add(vdir);
                        
                }
            }

            return vdirs.ToArray();
        }

        public void SetZooConsoleEnabled(ServerManager srvman, string siteId, string appName)
        {

            Random random = new Random((int) DateTime.Now.Ticks);
            byte[] bytes = new byte[8];
            random.NextBytes(bytes);
            string consoleUrl = "console_" + Convert.ToBase64String(bytes);
            SetZooEnvironmentVariable(srvman, siteId, appName, "CONSOLE_URL", consoleUrl);

        }
        
        public void SetZooConsoleDisabled(ServerManager srvman, string siteId, string appName)
        {
            SetZooEnvironmentVariable(srvman, siteId, appName, "CONSOLE_URL", null);
        }

        public void SetZooEnvironmentVariable(ServerManager srvman, string siteId, string appName, string envName, string envValue)
        {
            if (!SiteExists(srvman, siteId))
                return;

            
            var iisObject = srvman.Sites[siteId];
            //
            foreach (var item in iisObject.Applications)
            {
                

                if (appName == ConfigurationUtility.GetNonQualifiedVirtualPath(item.Path))
                {
                    Configuration cfg = item.GetWebConfiguration();
                    ConfigurationSection section = cfg.GetSection("system.webServer/heliconZoo");
                    ConfigurationElement zooAppElement = section.GetCollection()[0];
                    ConfigurationElementCollection envColl = zooAppElement.GetChildElement("environmentVariables").GetCollection();

                    //remove all CONSOLE_URLs
                    for (int i = 0; i < envColl.Count; )
                    {
                        if (String.Compare(envColl[i].GetAttributeValue("name").ToString(), envName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            envColl.RemoveAt(i);        
                        }
                        else
                        {
                            ++i;
                        }
                        
                    }

                    // do not set empty value
                    if (!string.IsNullOrEmpty(envValue))
                    {
                        ConfigurationElement el = envColl.CreateElement();
                        el.SetAttributeValue("name", envName);
                        el.SetAttributeValue("value", envValue);
                        envColl.Add(el);
                    }

                    
                    srvman.CommitChanges();
                }

              

            }
         
        }


        public WebVirtualDirectory[] GetVirtualDirectories(ServerManager srvman, string siteId)
        {

            if (!SiteExists(srvman, siteId))
                // if site does not exist return for add new directory
                return new WebVirtualDirectory[] { };
            // Create site variable to work with
            Site iisSite = srvman.Sites[siteId];
            // create variable for directory list
            var vdirs = new List<WebVirtualDirectory>();
            // Make sure site ID exists
            if (iisSite != null)
            {
                // Grab site root Application not sub applications
                Application application = iisSite.Applications["/"];
                if (application != null)
                {
                    // Make sure we grab only virtual directories, Not apps
                    foreach (VirtualDirectory directory in application.VirtualDirectories)
                    {
                        // Do not show Website it self as Virtual Directory
                        if (directory.Path == "/")
                            continue;
                        // Grab directory Information
                        vdirs.Add(new WebVirtualDirectory
                        {
                            Name = ConfigurationUtility.GetNonQualifiedVirtualPath(directory.Path),
                            ContentPath = directory.PhysicalPath,
                        });
                    }
                }
            }
            // Virtual directories collection ready for view
            return vdirs.ToArray();

        }

        public WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            //
            if (String.IsNullOrEmpty(siteId))
                throw new ArgumentNullException("siteId");
            //
            if (String.IsNullOrEmpty(directoryName))
                throw new ArgumentNullException("directoryName");
            //
            using (var iisManager = GetServerManager())
            {
                Application iisSiteObject = iisManager.Sites[siteId].Applications["/"];
                if (!SiteExists(iisManager, siteId))
                    return null;

                var vdir = new WebVirtualDirectory
                {
                    Name = directoryName,
                    ParentSiteName = siteId
                };

                return vdir;
            }
        }

        public void CreateVirtualDirectory(string siteId, string directoryName, string physicalPath)
        {
            using (var iisManager = GetServerManager())
            {
                Application iisSiteObject = iisManager.Sites[siteId].Applications["/"];
                if (!SiteExists(iisManager, siteId))
                    throw new ApplicationException();

                iisSiteObject.VirtualDirectories.Add(directoryName, physicalPath);
                iisManager.CommitChanges();
            }
        }

        public void UpdateVirtualDirectory(WebVirtualDirectory virtualDir)
        {

            // ensure physical site content path
            if (String.IsNullOrEmpty(virtualDir.ContentPath))
                throw new Exception("VirtualDirContentPathEmpty");
            //
            using (var iisManager = GetServerManager())
            {
                Application iisSiteObject = iisManager.Sites[virtualDir.ParentSiteName].Applications["/"];


                var v_dir = iisSiteObject.VirtualDirectories[virtualDir.VirtualPath];

                v_dir.PhysicalPath = virtualDir.ContentPath;
                //
                iisManager.CommitChanges();

                }
        }

        public bool VirtualDirectoryExists(string siteId, string directoryName)
        {
            using (var iisManager = GetServerManager())
            {
                //Get site Id and Path
                Application iisSiteObject = iisManager.Sites[siteId].Applications["/"];
                //Check if site exists
                if (!SiteExists(iisManager, siteId))
                    return false;

                var vdir = new WebVirtualDirectory
                {
                    Name = directoryName,
                    ParentSiteName = siteId
                };
                //
                return iisSiteObject.VirtualDirectories[vdir.VirtualPath] != null;
            }
        }

        public void DeleteVirtualDirectory(WebVirtualDirectory virtualDir)
        {

            using (var iisManager = GetServerManager())
            {

                if (!SiteExists(iisManager, virtualDir.ParentSiteName))
                    return;

                Site iisSiteRoot = iisManager.Sites[virtualDir.ParentSiteName];
                if (iisSiteRoot != null)
                {
                    Application application = iisSiteRoot.Applications["/"];
                    if (application != null)
                    {
                       VirtualDirectory directory = application.VirtualDirectories[virtualDir.VirtualPath];
                        if (directory != null)
                        {
                            //
                            application.VirtualDirectories.Remove(directory);
                        }
                    }
                }
                //
                iisManager.CommitChanges();
            }
        }

        public WebAppVirtualDirectory[] GetAppVirtualDirectories(ServerManager srvman, string siteId)
        {
            if (!SiteExists(srvman, siteId))
                return new WebAppVirtualDirectory[] { };

            var vdirs = new List<WebAppVirtualDirectory>();
            var iisObject = srvman.Sites[siteId];
            //
            foreach (var item in iisObject.Applications)
            {
                // Skip root application which is web site itself
                if (item.Path == "/")
                    continue;
                if (item.ApplicationPoolName == "Null")
                    continue;
                //
                vdirs.Add(new WebAppVirtualDirectory
                {
                    Name = ConfigurationUtility.GetNonQualifiedVirtualPath(item.Path),
                    ContentPath = item.VirtualDirectories[0].PhysicalPath
                });
            }
            //
            return vdirs.ToArray();
        }

        public WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
		{
			//
			if (String.IsNullOrEmpty(siteId))
				throw new ArgumentNullException("siteId");
			//
			if (String.IsNullOrEmpty(directoryName))
				throw new ArgumentNullException("directoryName");
			//
			using (var srvman = GetServerManager())
			{
                if (!SiteExists(srvman, siteId))
                    return null;

				var site = srvman.Sites[siteId];
				//
				var vdir = new WebAppVirtualDirectory
				{
					Name = directoryName,
					ParentSiteName = siteId
				};
				// We assume that we create only applications.
				vdir.ApplicationPool = site.Applications[vdir.VirtualPath].ApplicationPoolName;
				//
				return vdir;
			}
		}

		public void CreateAppVirtualDirectory(string siteId, string directoryName, string physicalPath)
		{
			using (var srvman = GetServerManager())
			{
                if (!SiteExists(srvman, siteId))
                    throw new ApplicationException();

				var iisSiteObject = srvman.Sites[siteId];
				var iisAppObject = iisSiteObject.Applications.Add(directoryName, physicalPath);
				//
				srvman.CommitChanges();
			}
		}

        public void UpdateAppVirtualDirectory(WebAppVirtualDirectory virtualDir)
        {
            // ensure physical site content path
            if (String.IsNullOrEmpty(virtualDir.ContentPath))
                throw new Exception("VirtualDirContentPathEmpty");
            //
            using (var srvman = GetServerManager())
            {
                // Obtain parent web site
                var webSite = srvman.Sites[virtualDir.ParentSiteName];
                // Ensure web site has been found
                if (webSite == null)
                    throw new ApplicationException("WebSiteNotFound");
                //
                var v_dir = webSite.Applications[virtualDir.VirtualPath];
                v_dir.ApplicationPoolName = virtualDir.ApplicationPool;
                v_dir.VirtualDirectories[0].PhysicalPath = virtualDir.ContentPath;
                //
                srvman.CommitChanges();
            }
        }

        public bool AppVirtualDirectoryExists(string siteId, string directoryName)
		{
			using (var srvman = GetServerManager())
			{
                if (!SiteExists(srvman, siteId))
                    return false;

				var vdir = new WebAppVirtualDirectory
				{
					Name = directoryName,
					ParentSiteName = siteId
				};
				//
				return (srvman.Sites[siteId].Applications[vdir.VirtualPath] != null);
			}
		}

    	public void DeleteAppVirtualDirectory(WebAppVirtualDirectory virtualDir)
		{
			using (var srvman = GetServerManager())
			{
                if (!SiteExists(srvman, virtualDir.ParentSiteName))
                    return;

				var iisSiteObject = srvman.Sites[virtualDir.ParentSiteName];
				var iisAppObject = iisSiteObject.Applications[virtualDir.VirtualPath];
				//
				if (iisAppObject != null)
					iisSiteObject.Applications.Remove(iisAppObject);
				//
				srvman.CommitChanges();
			}
        }
    }

    
}
