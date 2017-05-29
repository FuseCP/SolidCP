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
using System.Collections;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Reflection;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Generic;
using System.Text;
using System.Security.Policy;
using System.Xml;
using System.Web;
using System.IO;
using Microsoft.Win32;
using Microsoft.Web.Deployment;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Web.WPIWebApplicationGallery;
using SolidCP.Server.Utils;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.Providers.Utils.LogParser;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;

using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using SolidCP.Providers.Common;
using System.Collections.Specialized;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace SolidCP.Providers.Web
{
	public class IIs60 : HostingServiceProviderBase, IWebServer
	{
		// Empty remote server settings are intended to emulate localhost settings.
		// To help solve issues with AD accounts membership in local groups such as IIS_WPG or IIS_IUSRS and etc.
		public static readonly RemoteServerSettings Localhost = new RemoteServerSettings();

		#region Constants
		public const string APP_POOL_NAME_FORMAT_STRING = "#SITE-NAME# Pool #IIS6-ASPNET-VERSION#";
		public const string IIS_SERVICE_ID = "W3SVC";

		public const string REDIRECT_EXACT_URL = "EXACT_DESTINATION";
		public const string REDIRECT_DIRECTORY_BELOW = "CHILD_ONLY";
		public const string REDIRECT_PERMANENT = "PERMANENT";

		public const string CGI_BIN_FOLDER = "cgi-bin";

		public const string DEDICATED_POOL_SUFFIX_ASPNET1 = " Pool";
		public const string DEDICATED_POOL_SUFFIX_ASPNET2 = " Pool 2.0";
		public const string DEDICATED_POOL_SUFFIX_ASPNET4 = " Pool 4.0";
		public const string IIS_WPG_GROUP = "IIS_WPG";

		public const string PHP_4 = "4";
		public const string PHP_5 = "5";

		public const string ASPNET_11 = "1";
		public const string ASPNET_20 = "2";
		public const string ASPNET_40 = "4";

		protected string[] ASP_EXTENSIONS = new string[] { ".asa,5", ".asp,5", ".cdx,5", ".cer,5", ".htr,5" };

		protected string[] ASPNET_11_EXTENSIONS = new string[] { ".asax,5", ".ascx,5", ".ashx,1",
        ".asmx,1", ".aspx,1", ".axd,1", ".config,5", ".cs,5", ".csproj,5", ".licx,5", ".rem,1", ".resources,5", ".resx,5",
        ".soap,1", ".vb,5", ".vbproj,5", ".vsdisco,1", ".webinfo,5"};

		protected string[] ASPNET_20_EXTENSIONS = new string[] { ".ad,5", ".adprototype,5", ".asax,5", ".ascx,5", ".ashx,1",
        ".asmx,1", ".aspx,1", ".axd,1", ".browser,5", ".cd,5", ".compiled,5", ".config,5", ".cs,5", ".csproj,5", ".dd,5",
        ".exclude,5", ".java,5", ".jsl,5", ".ldb,5", ".ldd,5", ".lddprototype,5", ".ldf,5", ".licx,5", ".master,5",
        ".mdb,5", ".mdf,5", ".msgx,5", ".refresh,5", ".rem,1", ".resources,5", ".resx,5", ".sd,5", ".sdm,5", ".sdmDocument,5",
        ".sitemap,5", ".skin,5", ".soap,1", ".svc,5", ".vb,5", ".vbproj,5", ".vjsproj,5", ".vsdisco,1", ".webinfo,5"};

		protected string[] ASPNET_40_EXTENSIONS = new string[] {".asax,5", ".ascx,5", ".ashx,1", ".asmx,1", ".aspx,1", ".axd,1",
			".vsdisco,1", ".rem,1", ".soap,1", ".config,5", ".cs,5", ".csproj,5", ".vb,5", ".vbproj,5", ".webinfo,5", ".licx,5",
			".resx,5", ".resources,5", ".master,5", ".skin,5", ".compiled,5", ".browser,5", ".mdb,5", ".jsl,5", ".vjsproj,5",
			".sitemap,5", ".msgx,1", ".ad,5", ".dd,5", ".ldd,5", ".sd,5", ".cd,5", ".adprototype,5", ".lddprototype,5",
			".sdm,5", ".sdmDocument,5", ".ldb,5", ".mdf,5", ".ldf,5", ".java,5", ".exclude,5", ".refresh,5", ".xamlx,1",
			".cshtm,5", ".cshtml,5", ".vbhtm,5", ".vbhtml,5", ".svc,1", ".xoml,1", ".rules,5"};

		protected string[] PERL_EXTENSIONS = new string[] { ".pl,5", ".cgi,5" };
		protected string[] PHP_EXTENSIONS = new string[] { ".php,5" };
		protected string[] PYTHON_EXTENSIONS = new string[] { ".py,5" };

		protected string[] COLDFUSION_EXTENSIONS = new string[] { ".cfc,5", ".cfm,5", ".cfml,5", ".cfr,5", ".cfswf,5", ".jws,5" };

		protected string[] CUSTOM_ERRORS_TYPE1 = new string[] { "400", "403.1", "403.2", "403.3", "403.4", "403.5", "403.6", "403.7", 
        "403.8" , "403.9", "403.10", "403.11", "403.12", "403.13", "403.14", "403.15", "403.16", "403.17", "403.18", "403.19", "403.20", "404", "404.2", "404.3", "405", "406",
        "412", "414", "415", "500", "500.12", "500.13", "500.14", "500.15", "500.16", "500.17", "500.18", "500.19",
        "500.100", "501"};

		protected string[] CUSTOM_ERRORS_TYPE2 = new string[] { "401.1", "401.2", "401.3", "401.4", "401.5", "401.7", "407", "502" };

		// website root and ftp site root constants
		public const string FRONTPAGE_PATH = "W3SVC/Filters/fpexedll.dll";
		public const string FRONTPAGE_2002_INSTALLED = "Setup Packages";
		public const string SHAREPOINT_INSTALLED = "SharePoint";

		public const string IIS_PASSWORD_FILTER = "IISPassword";

		// Front Page related constants
		public const string SHARED_TOOLS_REGLOC = @"SOFTWARE\Microsoft\Shared Tools";
		public const string FRONTPAGE_2000_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\4.0";
		public const string FRONTPAGE_2002_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\5.0";
		public const string FRONTPAGE_PORT_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\Ports\";
		public const string SHAREPOINT_PORT_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\Ports\";
		public const string FRONTPAGE_ALLPORTS_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\All Ports\";

		//Front Page related constants (x64)
		public const string SHARED_TOOLS_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools";
		public const string FRONTPAGE_2000_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\4.0";
		public const string FRONTPAGE_2002_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\5.0";
		public const string FRONTPAGE_PORT_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\Ports\";
		public const string SHAREPOINT_PORT_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\Ports\";
		public const string FRONTPAGE_ALLPORTS_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\All Ports\";

		//ColdFusion related constants
		public const string COLDFUSION_2016_REGLOC = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 2016";
		public const string COLDFUSION_2016_REGLOC_X64 = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 2016";
		public const string COLDFUSION_11_REGLOC = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 11";
		public const string COLDFUSION_11_REGLOC_X64 = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 11";		
		public const string COLDFUSION_10_REGLOC = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 10";
		public const string COLDFUSION_10_REGLOC_X64 = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 10";
		public const string COLDFUSION_9_REGLOC = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 9";
		public const string COLDFUSION_9_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Adobe\Install Data\Adobe ColdFusion 9";
		public const string COLDFUSION_8_REGLOC = @"SOFTWARE\Adobe\Install Data\Adobe ColdFusion 8";
		public const string COLDFUSION_8_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Adobe\Install Data\Adobe ColdFusion 8";
		public const string COLDFUSION_7_REGLOC = @"SOFTWARE\Macromedia\Install Data\ColdFusion MX 7";
		public const string COLDFUSION_7_REGLOC_X64 = @"SOFTWARE\Wow6432Node\Macromedia\Install Data\ColdFusion MX 7";
		public const string COLDFUSION_ROOT_PATH = "CFMXRoot";
		public const string COLDFUSION_WEB_ROOT_PATH = "WebRoot";

		// IISPassword
		public const string AUTH_NAME_DIRECTIVE = "AuthName ";
		public const string ProtectedUsersFile_DIRECTIVE = "AuthUserFile ";
		public const string ProtectedGroupsFile_DIRECTIVE = "AuthGroupFile ";
		public const string REQUIRE_USER_DIRECTIVE = "Require user ";
		public const string REQUIRE_GROUP_DIRECTIVE = "Require group ";

		public const string WEB_PI_USER_AGENT_HEADER = "Platform-Installer/2.0.0.0({0})";
		public const string WEB_PI_APP_PACK_ROOT_INSTALLER_ITEM_MISSING = "Root installer item for the {0} application could not be found. Please contact your Web Application Gallery feed provider to resolve the error.";
		public const string WEB_PI_APP_PACK_DISPLAY_URL_MISSING = "Web application '{0}' could not be downloaded as installer displayURL is empty or missing.";

		// web application gallery
		public const string WAG_APPLICATIONS_CACHE_KEY = "WAG_APPLICATIONS_CACHE_KEY";
		public const int WEB_APPLICATIONS_CACHE_STORE_MINUTES = 60;
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

		protected string WebGroupName
		{
			get { return ProviderSettings["WebGroupName"]; }
		}

		protected string Asp11Pool
		{
			get { return ProviderSettings["AspNet11Pool"]; }
		}

		protected string Asp20Pool
		{
			get { return ProviderSettings["AspNet20Pool"]; }
		}

		protected string Asp40Pool
		{
			get { return ProviderSettings["AspNet40Pool"]; }
		}

		protected string AspPath
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["AspPath"]); }
		}

		protected string AspNet11Path
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["AspNet11Path"]); }
		}

		protected string AspNet20Path
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["AspNet20Path"]); }
		}

		protected string AspNet40Path
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["AspNet40Path"]); }
		}

		protected string Php4Path
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["Php4Path"]); }
		}

		protected string Php5Path
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["Php5Path"]); }
		}

		protected string PerlPath
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["PerlPath"]); }
		}

		protected string PythonPath
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["PythonPath"]); }
		}

		protected string ColdFusionPath
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ColdFusionPath"]); }
		}

		protected string CFScriptsDirectoryPath
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["CFScriptsDirectory"]); }
		}

		protected string CFFlashRemotingDirPath
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["CFFlashRemotingDirectory"]); }
		}

		protected string SecuredFoldersFilterPath
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["SecuredFoldersFilterPath"]); }
		}

		protected virtual string ProtectedAccessFile
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ProtectedAccessFile"]); }
		}

		protected string ProtectedUsersFile
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ProtectedUsersFile"]); }
		}

		protected string ProtectedGroupsFile
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ProtectedGroupsFile"]); }
		}

		protected virtual string ProtectedFoldersFile
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ProtectedFoldersFile"]); }
		}

        /*
        protected string GalleryXmlFeedUrl
		{
			get
			{
				string ret = ProviderSettings["GalleryXmlFeedUrl"];
				if (String.IsNullOrEmpty(ret))
					ret = WebApplicationGallery.WAG_DEFAULT_FEED_URL;
				return ret;
			}
		}
        */
		#endregion

		private WmiHelper wmi = null;

		private object lockObject = new object();

		public IIs60()
		{
			if (IsIISInstalled())
			{
				// instantiate WMI helper
				wmi = new WmiHelper("root\\MicrosoftIISv2");
			}
		}

		#region Web Sites
		public virtual void ChangeSiteState(string siteId, ServerState state)
		{
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServer='{0}'", siteId));
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
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServer='{0}'", siteId));
			return (ServerState)objSite.Properties["ServerState"].Value;
		}

		public virtual bool SiteExists(string siteId)
		{
			return (wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsWebServerSetting WHERE Name='{0}'", siteId)).Count > 0);
		}

		public virtual string[] GetSites()
		{
			List<string> sites = new List<string>();

			// get all sites
			ManagementObjectCollection objSites = wmi.ExecuteQuery("SELECT * FROM IIsWebServerSetting");
			foreach (ManagementObject objSite in objSites)
				sites.Add((string)objSite.Properties["ServerComment"].Value);

			return sites.ToArray();
		}

		public string GetSiteId(string siteName)
		{
			string siteId = null;
			ManagementObjectCollection objSites = wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsWebServerSetting WHERE ServerComment='{0}'", siteName));
			foreach (ManagementObject objSite in objSites)
				siteId = (string)objSite.Properties["Name"].Value;
			return siteId;
		}

		public string[] GetSitesAccounts(string[] siteIds)
		{
			List<string> accounts = new List<string>();
			for (int i = 0; i < siteIds.Length; i++)
			{
				try
				{
					ManagementObject objVirtDir = wmi.GetObject(
						String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteIds[i], "")));
					accounts.Add(GetNonQualifiedAccountName((string)objVirtDir.Properties["AnonymousUserName"].Value));
				}
				catch (Exception ex)
				{
					Log.WriteError(String.Format("Web site {0} is either deleted or doesn't exist", siteIds[i]), ex);
				}
			}
			//
			return accounts.ToArray();
		}

		public virtual WebSite GetSite(string siteId)
		{
			WebSite site = new WebSite();

			// get web server settings object
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

			FillWebSiteFromWmiObject(site, objSite);

			// get ROOT vritual directory settings object
			ManagementObject objVirtDir = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, "")));

			FillAppVirtualDirectoryFromWmiObject(site, objVirtDir);
			FillAppVirtualDirectoryRestFromWmiObject(site, objVirtDir);

			// check frontpage
			site.FrontPageAvailable = IsFrontPageSystemInstalled();
			site.FrontPageInstalled = IsFrontPageInstalled(siteId);

			// check coldfusion
			if (IsColdFusionSystemInstalled())
			{
				if (IsColdFusion7Installed())
				{
					site.ColdFusionVersion = "7";
					site.ColdFusionAvailable = true;
				}
				else
				{
					if (IsColdFusion8Installed())
					{
						site.ColdFusionVersion = "8";
						site.ColdFusionAvailable = true;
					}
				}

				if (IsColdFusion9Installed())
				{
					site.ColdFusionVersion = "9";
					site.ColdFusionAvailable = true;
				}
				
				if (IsColdFusion10Installed())
				{
					site.ColdFusionVersion = "10";
					site.ColdFusionAvailable = true;
				}

				if (IsColdFusion11Installed())
				{
					site.ColdFusionVersion = "11";
					site.ColdFusionAvailable = true;
				}
				
				if (IsColdFusion2016Installed())
				{
					site.ColdFusionVersion = "2016";
					site.ColdFusionAvailable = true;
				}
				
			}
			else
			{
				site.ColdFusionAvailable = false;
			}

			WebAppVirtualDirectory[] appdirs = GetAppVirtualDirectories(siteId);
            WebVirtualDirectory[] virtdirs = GetVirtualDirectories(siteId);

            if (IsColdFusion10Installed() || IsColdFusion11Installed() || IsColdFusion2016Installed())
				{
					if (AppVirtualDirectoryExists(siteId, "CFIDE") && AppVirtualDirectoryExists(siteId, "jakarta"));
					site.CreateCFAppVirtualDirectories = true;
				}
			else
				{
					if (AppVirtualDirectoryExists(siteId, "CFIDE") && AppVirtualDirectoryExists(siteId, "JRunScripts"));
					site.CreateCFAppVirtualDirectories = true;
				}
			{
				site.CreateCFAppVirtualDirectories = false;
			}

			// check sharepoint
			site.SharePointInstalled = IsSharePointInstalledOnWebSite(siteId);

			// check write permissions
			site.EnableWritePermissions = CheckWriteAccessEnabled(site.ContentPath,
				GetNonQualifiedAccountName(site.AnonymousUsername));

			// check CGI-BIN
			site.CgiBinInstalled = CheckCgiBinEnabled(siteId);

			// check if dedicated pool is enabled
			site.DedicatedApplicationPool = CheckIsDedicatedPoolEnabled(site.Name, site.ApplicationPool);

			// check secured folders
			site.SecuredFoldersInstalled = IsSecuredFoldersInstalled(siteId);

			// check Helicon Ape support
			HeliconApeStatus heliconApeStatus = GetHeliconApeStatus(null);
			site.HeliconApeInstalled = heliconApeStatus.IsInstalled;
			site.HeliconApeEnabled = heliconApeStatus.IsEnabled;

			site.SiteState = GetSiteState(siteId);

			return site;
		}

		public virtual ServerBinding[] GetSiteBindings(string siteId)
		{
			// get web server settings object
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

			WebSite site = new WebSite();
			FillWebSiteFromWmiObject(site, objSite);
			return site.Bindings;
		}

		public virtual string CreateSite(WebSite site)
		{
			// anonymous user groups
			List<string> webGroups = new List<string>();
			webGroups.Add(WebGroupName);

			// create web site anonymous account
			SystemUser user = new SystemUser();
			user.Name = site.AnonymousUsername;
			user.FullName = site.AnonymousUsername;
			user.Description = "SolidCP System Account";
			user.MemberOf = webGroups.ToArray();
			user.Password = site.AnonymousUserPassword;
			user.PasswordCantChange = true;
			user.PasswordNeverExpires = true;
			user.AccountDisabled = false;
			user.System = true;

			// create in the system
			try
			{
				SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);
			}
			catch (Exception ex)
			{
				// the possible reason the account already exists
				// check this
				if (SecurityUtils.UserExists(user.Name, ServerSettings, UsersOU))
				{
					// yes
					// try to give it original name
					for (int i = 2; i < 99; i++)
					{
						string username = user.Name + i.ToString();
						if (!SecurityUtils.UserExists(username, ServerSettings, UsersOU))
						{
							user.Name = username;
							site.AnonymousUsername = username;

							// try to create again
							SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);
							break;
						}
					}
				}
				else
				{
					throw ex;
				}
			}

			// Grant IIS_WPG group membership to site's anonymous account
			SecurityUtils.GrantLocalGroupMembership(site.AnonymousUsername, IIS_WPG_GROUP, ServerSettings);

			// Build names for dedicated pools
			string poolName1 = site.Name + DEDICATED_POOL_SUFFIX_ASPNET1;
			string poolName2 = site.Name + DEDICATED_POOL_SUFFIX_ASPNET2;
			string poolName4 = site.Name + DEDICATED_POOL_SUFFIX_ASPNET4;

			//
			bool dedicatedPool = site.DedicatedApplicationPool;

			// Check if we need to create a separate application pool
			if (dedicatedPool)
			{
				// Create dedicated pools
				CreateApplicationPool(poolName1, site.AnonymousUsername, site.AnonymousUserPassword);
				CreateApplicationPool(poolName2, site.AnonymousUsername, site.AnonymousUserPassword);
				CreateApplicationPool(poolName4, site.AnonymousUsername, site.AnonymousUserPassword);
			}

			// Assign application pool
			switch (site.AspNetInstalled)
			{
				case ASPNET_11:
					site.ApplicationPool = (dedicatedPool) ? poolName1 : Asp11Pool;
					break;
				case ASPNET_20:
					site.ApplicationPool = (dedicatedPool) ? poolName2 : Asp20Pool;
					break;
				case ASPNET_40:
					site.ApplicationPool = (dedicatedPool) ? poolName4 : Asp40Pool;
					break;
				default:
					break;
			}

			// set folder permissions
			SetWebFolderPermissions(site.ContentPath, site.AnonymousUsername,
				site.EnableWritePermissions, site.DedicatedApplicationPool);

			// set DATA folder permissions
			SetWebFolderPermissions(site.DataPath, site.AnonymousUsername,
				true, site.DedicatedApplicationPool);

			// create logs folder if not exists
			if (!FileUtils.DirectoryExists(site.LogsPath))
				FileUtils.CreateDirectory(site.LogsPath);

			//SecurityUtils.GrantNtfsPermissionsBySid(site.LogFileDirectory,
			//    SystemSID.NETWORK_SERVICE, NTFSPermission.Modify, true, true);

			// create Web site
			ManagementObject objService = wmi.GetObject(String.Format("IIsWebService='{0}'", IIS_SERVICE_ID));

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

			ManagementBaseObject objResult = objService.InvokeMethod("CreateNewSite", methodParams, new InvokeMethodOptions());

			// get WEB settings
			string siteId = ((string)objResult["returnValue"]).Remove(0, "IIsWebServer='".Length).Replace("'", "");

			// update site properties
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));
			ManagementObject objVirtDir = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, "")));

			if (site.LogsPath != null && site.LogsPath != "")
				objSite.Properties["LogFileDirectory"].Value = site.LogsPath;

			FillWmiObjectFromAppVirtualDirectory(objSite, site, false);
			objSite.Put();

			FillWmiObjectFromAppVirtualDirectory(objVirtDir, site, false);
			FillWmiObjectFromAppVirtualDirectoryRest(objVirtDir, site);
			// UNC Share
			ManagementObject objVirtDirUnc = wmi.GetObject(
				String.Format("IIsWebVirtualDir='{0}'", GetAppVirtualDirectoryPath(siteId, "")));
			FillWmiObjectUNCSettingsFromAppVirtualDirectory(objVirtDir, site);
			objVirtDirUnc.Put();

			objVirtDir.Put();

			// CGI-BIN folder
			UpdateCgiBinFolder(siteId, site.ContentPath, site.CgiBinInstalled);

			// start web site
			try
			{
				ChangeSiteState(siteId, ServerState.Started);
			}
			catch
			{
				// just skip an error
			}

			return siteId;
		}

		public virtual void UpdateSite(WebSite site)
		{
			// remove unnecessary permissions
			WebSite origSite = GetSite(site.SiteId);

			// Get non-qualified anonymous account user name (eq. without domain name or machine name)
			string anonymousAccount = GetNonQualifiedAccountName(site.AnonymousUsername);
			string origAnonymousAccount = GetNonQualifiedAccountName(origSite.AnonymousUsername);

			// if folder has been changed
			if (String.Compare(origSite.ContentPath, site.ContentPath, true) != 0)
				RemoveWebFolderPermissions(origSite.ContentPath, origAnonymousAccount);

			// dedicated app pool
			string poolName1 = origSite.Name + DEDICATED_POOL_SUFFIX_ASPNET1;
			string poolName2 = origSite.Name + DEDICATED_POOL_SUFFIX_ASPNET2;
			string poolName4 = origSite.Name + DEDICATED_POOL_SUFFIX_ASPNET4;

			//
			bool dedicatedPool = site.DedicatedApplicationPool;

			// set pool accordingly to ASP.NET
			switch (site.AspNetInstalled)
			{
				case ASPNET_11:
					site.ApplicationPool = (dedicatedPool) ? poolName1 : Asp11Pool;
					break;
				case ASPNET_20:
					site.ApplicationPool = (dedicatedPool) ? poolName2 : Asp20Pool;
					break;
				case ASPNET_40:
					site.ApplicationPool = (dedicatedPool) ? poolName4 : Asp40Pool;
					break;
				default:
					// Defaults to .NET 1.1
					site.ApplicationPool = (dedicatedPool) ? poolName1 : Asp11Pool;
					break;
			}

			bool deleteDedicatedPools = false;

			// add anonymous to IIS_WPG
			if (!SecurityUtils.HasLocalGroupMembership(anonymousAccount, IIS_WPG_GROUP, ServerSettings, UsersOU))
			{
				SecurityUtils.GrantLocalGroupMembership(anonymousAccount, IIS_WPG_GROUP, ServerSettings);
			}

			// check if DedicatedApplicationPool property
			// has been changed
			bool dedicatedPoolFlagChanged = (origSite.DedicatedApplicationPool != site.DedicatedApplicationPool);
			//
			if (site.DedicatedApplicationPool)
			{
				// CREATE dedicated pool
				if (!ApplicationPoolExists(poolName1))
				{
					CreateApplicationPool(poolName1, anonymousAccount, site.AnonymousUserPassword);
				}

				if (!ApplicationPoolExists(poolName2))
				{
					CreateApplicationPool(poolName2, anonymousAccount, site.AnonymousUserPassword);
				}

				if (!ApplicationPoolExists(poolName4))
				{
					CreateApplicationPool(poolName4, anonymousAccount, site.AnonymousUserPassword);
				}
			}
			else
			{
				// REMOVE dedicated pool
				deleteDedicatedPools = true;
			}

			// set WEB folder permissions
			SetWebFolderPermissions(site.ContentPath, anonymousAccount, site.EnableWritePermissions, site.DedicatedApplicationPool);

			// set DATA folder permissions
			SetWebFolderPermissions(site.DataPath, anonymousAccount, true, site.DedicatedApplicationPool);

			// set logs folder permissions
			if (!FileUtils.DirectoryExists(site.LogsPath))
				FileUtils.CreateDirectory(site.LogsPath);

			//SecurityUtils.GrantNtfsPermissionsBySid(site.LogFileDirectory,
			//    SystemSID.NETWORK_SERVICE, NTFSPermission.Modify, true, true);

			// update site properties
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", site.SiteId));
			ManagementObject objVirtDir = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(site.SiteId, "")));

			if (site.LogsPath != null && site.LogsPath != "")
				objSite.Properties["LogFileDirectory"].Value = site.LogsPath;

			// delete existing bindings


			// update bindings
			ManagementClass clsBinding = wmi.GetClass("ServerBinding");
			ManagementObject[] objBinings = new ManagementObject[site.Bindings.Length];

			for (int i = 0; i < objBinings.Length; i++)
			{
				objBinings[i] = clsBinding.CreateInstance();
				objBinings[i]["Hostname"] = site.Bindings[i].Host;
				objBinings[i]["IP"] = site.Bindings[i].IP;
				objBinings[i]["Port"] = site.Bindings[i].Port;
			}
			objSite.Properties["ServerBindings"].Value = objBinings;

			FillWmiObjectFromAppVirtualDirectory(objSite, site, true);
			objSite.Put();

			FillWmiObjectFromAppVirtualDirectory(objVirtDir, site, true);
			FillWmiObjectFromAppVirtualDirectoryRest(objVirtDir, site);
			objVirtDir.Put();

			// UNC Share
			FillWmiObjectUNCSettingsFromAppVirtualDirectory(objVirtDir, site);

			// CGI-BIN folder
			UpdateCgiBinFolder(site.SiteId, site.ContentPath, site.CgiBinInstalled);

			// update all child virtual directories to use new pool
			if (dedicatedPoolFlagChanged)
			{
				WebAppVirtualDirectory[] dirs = GetAppVirtualDirectories(site.SiteId, false);
				foreach (WebAppVirtualDirectory dir in dirs)
				{
					// set dedicated pool flag
					//dir.DedicatedApplicationPool = site.DedicatedApplicationPool;
					WebAppVirtualDirectory vdir = GetAppVirtualDirectory(site.SiteId, dir.Name);

					// update directory
					UpdateAppVirtualDirectory(site.SiteId, vdir);
				}
			}

			#region Commented ColdFusion code
			//enable ColdFusion on site through wsconfig.exe utility
			/*if (IsColdFusionSystemInstalled())
			{
				if (IsColdFusionEnabledOnSite(origSite.SiteId))
				{
					if (!site.ColdFusionInstalled)
					{
						DisableColdFusionScripting(site.SiteId, site.Name);
						site.ColdFusionInstalled = false;
					}
				}
				else
				{
					if (site.ColdFusionInstalled)
					{
						EnableColdFusionScripting(site.Name);
						site.ColdFusionInstalled = true;
					}
				}
			}*/

			#endregion

			#region ColdFusion Virtual Directories
			WebAppVirtualDirectory[] virtdirs = GetAppVirtualDirectories(site.SiteId);
			bool cfDirsinstalled = false;

			if (IsColdFusion10Installed() || IsColdFusion11Installed() || IsColdFusion2016Installed())
				{
					if (AppVirtualDirectoryExists(site.SiteId, "CFIDE") && AppVirtualDirectoryExists(site.SiteId, "jakarta"));
				}
			else
				{
					if (AppVirtualDirectoryExists(site.SiteId, "CFIDE") && AppVirtualDirectoryExists(site.SiteId, "JRunScripts"));
				}
			{
				cfDirsinstalled = true;
			}

			if (cfDirsinstalled)
			{
				if (!site.CreateCFAppVirtualDirectories)
				{
					DeleteCFAppVirtualDirectories(site.SiteId);
					site.CreateCFAppVirtualDirectories = false;
				}
			}
			else
			{
				if (site.CreateCFAppVirtualDirectories)
				{
					CreateCFAppVirtualDirectories(site.SiteId);
					site.CreateCFAppVirtualDirectories = true;
				}
			}
			#endregion

			// delete dedicated pool if required
			if (deleteDedicatedPools)
			{
				if (ApplicationPoolExists(poolName1))
				{
					DeleteApplicationPool(poolName1);
				}

				if (ApplicationPoolExists(poolName2))
				{
					DeleteApplicationPool(poolName2);
				}

				if (ApplicationPoolExists(poolName4))
				{
					DeleteApplicationPool(poolName4);
				}
			}
		}

        // AppPool
        public void ChangeAppPoolState(string siteId, AppPoolState state)
        {
        }

        public AppPoolState GetAppPoolState(string siteId)
        {
            return AppPoolState.Unknown;
        }

        public virtual void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
		{
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

			// update bindings
			ManagementClass clsBinding = wmi.GetClass("ServerBinding");
			ManagementObject[] objBinings = new ManagementObject[bindings.Length];

			for (int i = 0; i < objBinings.Length; i++)
			{
				objBinings[i] = clsBinding.CreateInstance();
				objBinings[i]["Hostname"] = bindings[i].Host;
				objBinings[i]["IP"] = bindings[i].IP;
				objBinings[i]["Port"] = bindings[i].Port;
			}
			objSite.Properties["ServerBindings"].Value = objBinings;
			objSite.Put();
		}

		public virtual void DeleteSite(string siteId)
		{
			// load web site
			WebSite site = GetSite(siteId);

			//
			string anonymousAccount = GetNonQualifiedAccountName(site.AnonymousUsername);

			// remove unnecessary permissions
			RemoveWebFolderPermissions(site.ContentPath, anonymousAccount);

			// delete IIS object
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServer='{0}'", siteId));
			//stop Website before delete
			objSite.InvokeMethod("Stop", new object[0]);
			Log.WriteInfo(String.Format("Site {0} was stopped before deleting.", site.Name));
			objSite.Delete();

			// delete dedicated pool if required
			if (site.DedicatedApplicationPool)
			{
				string poolName1 = site.Name + DEDICATED_POOL_SUFFIX_ASPNET1;
				string poolName2 = site.Name + DEDICATED_POOL_SUFFIX_ASPNET2;
				string poolName4 = site.Name + DEDICATED_POOL_SUFFIX_ASPNET4;

				if (ApplicationPoolExists(poolName1))
					DeleteApplicationPool(poolName1);

				if (ApplicationPoolExists(poolName2))
					DeleteApplicationPool(poolName2);

				if (ApplicationPoolExists(poolName4))
					DeleteApplicationPool(poolName4);
			}

			// 
			if (!anonymousAccount.StartsWith("IUSR_"))
			{
				// Revoke IIS_WPG membership first
				if (SecurityUtils.HasLocalGroupMembership(anonymousAccount, IIS_WPG_GROUP, ServerSettings, UsersOU))
				{
					SecurityUtils.RevokeLocalGroupMembership(anonymousAccount, IIS_WPG_GROUP, ServerSettings);
				}

				// Delete anonymous user account
				if (SecurityUtils.UserExists(anonymousAccount, ServerSettings, UsersOU))
				{
					SecurityUtils.DeleteUser(GetNonQualifiedAccountName(site.AnonymousUsername), ServerSettings, UsersOU);
				}
			}
		}

		private bool CheckCgiBinEnabled(string siteId)
		{
			return (wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsWebDirectorySetting WHERE Name='{0}' AND AccessExecute=True",
				GetAppVirtualDirectoryPath(siteId, CGI_BIN_FOLDER))).Count > 0);
		}

		private void UpdateCgiBinFolder(string siteId, string contentPath, bool cgiBinInstalled)
		{
			string cgiBinId = GetAppVirtualDirectoryPath(siteId, CGI_BIN_FOLDER);
			ManagementObjectCollection objCgiBin = wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsWebDirectorySetting WHERE Name='{0}' AND AccessExecute=True",
					cgiBinId));

			if (cgiBinInstalled)
			{
				// create folder if not exists
				string cgiBinPath = Path.Combine(contentPath, CGI_BIN_FOLDER);
				if (!FileUtils.DirectoryExists(cgiBinPath))
					FileUtils.CreateDirectory(cgiBinPath);

				if (objCgiBin.Count == 0)
				{
					// add
					ManagementObject objDir = wmi.GetClass("IIsWebDirectorySetting").CreateInstance();
					objDir.Properties["Name"].Value = cgiBinId;
					objDir.Properties["AccessExecute"].Value = true;
					objDir.Put();
				}
			}
			else
			{
				// remove CGi-BIN
				if (objCgiBin.Count > 0)
					foreach (ManagementObject obj in objCgiBin)
						obj.Delete();
			}
		}

		protected bool CheckIsDedicatedPoolEnabled(string siteName, string appPoolName)
		{
			string poolName1 = siteName + DEDICATED_POOL_SUFFIX_ASPNET1;
			string poolName2 = siteName + DEDICATED_POOL_SUFFIX_ASPNET2;
			string poolName4 = siteName + DEDICATED_POOL_SUFFIX_ASPNET4;

			return (String.Compare(poolName1, appPoolName, true) == 0
				|| String.Compare(poolName2, appPoolName, true) == 0
				|| String.Compare(poolName4, appPoolName, true) == 0);
		}
		#endregion

		#region Virtual Directories
		

        public virtual bool AppPoolExists(string siteId, string appPool)
        {
            return DirectoryEntry.Exists(GetAppVirtualDirectoryADSIPath(siteId, appPool));
        }



		public virtual void CreateCFAppVirtualDirectories(string siteId)
		{
			WebAppVirtualDirectory scriptsDirectory = new WebAppVirtualDirectory();
			WebSite site = GetSite(siteId);
			scriptsDirectory.Name = "CFIDE";
			scriptsDirectory.ContentPath = CFScriptsDirectoryPath;
			scriptsDirectory.EnableAnonymousAccess = true;
			scriptsDirectory.EnableWindowsAuthentication = true;
			scriptsDirectory.EnableBasicAuthentication = false;
			scriptsDirectory.DefaultDocs = null; // inherit from service
			scriptsDirectory.HttpRedirect = "";
			scriptsDirectory.HttpErrors = null;
			scriptsDirectory.MimeMaps = null;

			if (!AppVirtualDirectoryExists(siteId, scriptsDirectory.Name))
			{
				CreateAppVirtualDirectoryNonApplication(siteId, scriptsDirectory);
			}

			WebAppVirtualDirectory flashRemotingDir = new WebAppVirtualDirectory();
			if (IsColdFusion10Installed() || IsColdFusion11Installed() || IsColdFusion2016Installed())
				{
					flashRemotingDir.Name = "jakarta";
				}
			else
				{
					flashRemotingDir.Name = "JRunScripts";
				}
			flashRemotingDir.ContentPath = CFFlashRemotingDirPath;
			flashRemotingDir.EnableAnonymousAccess = true;
			flashRemotingDir.EnableWindowsAuthentication = true;
			flashRemotingDir.EnableBasicAuthentication = false;
			flashRemotingDir.DefaultDocs = null; // inherit from service
			flashRemotingDir.HttpRedirect = "";
			flashRemotingDir.HttpErrors = null;
			flashRemotingDir.MimeMaps = null;

			if (!AppVirtualDirectoryExists(siteId, flashRemotingDir.Name))
			{
				CreateAppVirtualDirectoryNonApplication(siteId, flashRemotingDir);
			}
		}

		public virtual void DeleteCFAppVirtualDirectories(string siteId)
		{
			if (IsColdFusion10Installed() || IsColdFusion11Installed() || IsColdFusion2016Installed())
				{
					DeleteAppVirtualDirectory(siteId, "CFIDE");
					DeleteAppVirtualDirectory(siteId, "jakarta");
				}
			else
				{
					DeleteAppVirtualDirectory(siteId, "CFIDE");
					DeleteAppVirtualDirectory(siteId, "JRunScripts");
				}

		}
        public virtual void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {

        }

        public virtual bool VirtualDirectoryExists(string siteId, string directoryName)
        {
            return DirectoryEntry.Exists(GetVirtualDirectoryADSIPath(siteId, directoryName));
        }

        public virtual WebVirtualDirectory[] GetVirtualDirectories(string siteId)
        {
            return GetVirtualDirectories(siteId, false);
        }

        private WebVirtualDirectory[] GetVirtualDirectories(string siteId, bool includeSystemDirectories)
        {
            List<WebVirtualDirectory> dirs = new List<WebVirtualDirectory>();

            // MS SharedTools folder
            string sharedToolsFolder = GetMicrosoftSharedFolderPath();

            DirectoryEntry objSite = new DirectoryEntry(GetVirtualDirectoryADSIPath(siteId, ""));
            foreach (DirectoryEntry objVirtDir in objSite.Children)
            {
                if (objVirtDir.SchemaClassName == "IIsWebVirtualDir" &&
                    String.Compare(objVirtDir.Name, "root", true) != 0)
                {
                    // this is virtual directory
                    WebVirtualDirectory dir = new WebVirtualDirectory();
                    dir.Name = objVirtDir.Name;
                    dir.ContentPath = (string)objVirtDir.Properties["Path"].Value;

                    //do not show ColdFusion virtual directories
                    if (dir.ContentPath.Equals(CFScriptsDirectoryPath) || dir.ContentPath.Equals(CFFlashRemotingDirPath))
                    {
                        continue;
                    }

                    // check if this is a system (FrontPage or SharePoint) virtual directory
                    if (!includeSystemDirectories
                        && !String.IsNullOrEmpty(sharedToolsFolder)
                        && dir.ContentPath.ToLower().StartsWith(sharedToolsFolder.ToLower()))
                        continue;

                    // add to the collection
                    dirs.Add(dir);

                    // fill properties
                    //FillVirtualDirectoryFromWmiObject(dir, objVirtDir);
                    //FillVirtualDirectoryRestFromWmiObject(dir, objVirtDir);
                }
            }

            return dirs.ToArray();
        }

        public virtual WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            WebVirtualDirectory dir = new WebVirtualDirectory();
            ManagementObject objDir = wmi.GetObject(
                String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, directoryName)));

            dir.Name = directoryName;

            FillVirtualDirectoryFromWmiObject(dir, objDir);


            // load parent site settings
            ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

            // check write permissions
            dir.EnableWritePermissions = CheckWriteAccessEnabled(dir.ContentPath,
                GetNonQualifiedAccountName(dir.AnonymousUsername));

            return dir;
        }

        public virtual void CreateVirtualDirectoryNonApplication(string siteId, WebVirtualDirectory directory)
        {
            // create directory folder if not exists
            if (!FileUtils.DirectoryExists(directory.ContentPath))
                FileUtils.CreateDirectory(directory.ContentPath);

            string dirId = GetVirtualDirectoryPath(siteId, directory.Name);

            // create a new virtual directory
            ManagementObject objDir = wmi.GetClass("IIsWebVirtualDir").CreateInstance();
            objDir.Properties["Name"].Value = dirId;
            objDir.Put();

            // update directory properties
            ManagementObject objDirSetting = wmi.GetClass("IIsWebVirtualDirSetting").CreateInstance();
            objDirSetting.Properties["Name"].Value = dirId;
            objDirSetting.Properties["FriendlyName"].Value = directory.Name;
            objDirSetting.Properties["Path"].Value = directory.ContentPath;
            objDirSetting.Put();

            // check if site has write permissions enabled
            ManagementObject objSiteSetting = wmi.GetObject(String.Format(
                "IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, "")));

            directory.EnableWritePermissions = CheckWriteAccessEnabled(
                (string)objSiteSetting.Properties["Path"].Value,
                GetNonQualifiedAccountName((string)objSiteSetting.Properties["AnonymousUserName"].Value));

            // update directory
            UpdateVirtualDirectory(siteId, directory, false);
        }

        public virtual void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {














            // create directory folder if not exists
            if (!FileUtils.DirectoryExists(directory.ContentPath))
                FileUtils.CreateDirectory(directory.ContentPath);

            string dirId = GetVirtualDirectoryPath(siteId, directory.Name);

            // create a new virtual directory
            ManagementObject objDir = wmi.GetClass("IIsWebVirtualDir").CreateInstance();
            objDir.Properties["Name"].Value = dirId;
            objDir.Put();
            objDir.InvokeMethod("AppCreate", new Object[] { true });

            // update directory properties
            ManagementObject objDirSetting = wmi.GetClass("IIsWebVirtualDirSetting").CreateInstance();
            objDirSetting.Properties["Name"].Value = dirId;
            objDirSetting.Properties["FriendlyName"].Value = directory.Name;
            objDirSetting.Properties["Path"].Value = directory.ContentPath;
            objDirSetting.Put();

            // check if site has write permissions enabled
            ManagementObject objSiteSetting = wmi.GetObject(String.Format(
                "IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, "")));

            directory.EnableWritePermissions = CheckWriteAccessEnabled(
                (string)objSiteSetting.Properties["Path"].Value,
                GetNonQualifiedAccountName((string)objSiteSetting.Properties["AnonymousUserName"].Value));

            // update directory
            UpdateVirtualDirectory(siteId, directory, false);
        }

        public virtual void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            UpdateVirtualDirectory(siteId, directory, true);
        }

        private void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory, bool updateProperties)
        {
            // load parent site settings
            ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));
            ManagementObject objSiteSetting = wmi.GetObject(String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, "")));
            string siteName = (string)objSite.Properties["ServerComment"].Value;


            if (String.IsNullOrEmpty(directory.AnonymousUsername) ||
                String.IsNullOrEmpty(directory.AnonymousUserPassword))
            {
                directory.AnonymousUsername = GetNonQualifiedAccountName((string)objSiteSetting.Properties["AnonymousUserName"].Value);
                directory.AnonymousUserPassword = (string)objSiteSetting.Properties["AnonymousUserPass"].Value;
            }


            // load original virt dir
            ManagementObject objDir = wmi.GetObject(String.Format("IIsWebVirtualDirSetting='{0}'",
                GetVirtualDirectoryPath(siteId, directory.Name)));

            string origPath = (string)objDir.Properties["Path"].Value;

            string sharedToolsFolder = GetMicrosoftSharedFolderPath();
            // remove unnecessary permissions
            // if original folder has been changed
            if (!String.IsNullOrEmpty(sharedToolsFolder) &&
                !origPath.ToLower().StartsWith(sharedToolsFolder.ToLower()))
            {
                if (String.Compare(origPath, directory.ContentPath, true) != 0)
                    RemoveWebFolderPermissions(origPath, directory.AnonymousUsername);
            }



            if (updateProperties)
            {

                FillWmiObjectFromVirtualDirectory(objDir, directory, true);
                FillWmiObjectFromVirtualDirectoryRest(objDir, directory);

                // UNC Share
                ManagementObject objVirtDirUnc = wmi.GetObject(
                    String.Format("IIsWebVirtualDir='{0}'", GetVirtualDirectoryPath(siteId, directory.Name)));
                FillWmiObjectUNCSettingsFromVirtualDirectory(objVirtDirUnc, directory);
                objVirtDirUnc.Put();

                // save account
                objDir.Put();
            }
        }

        public virtual void DeleteVirtualDirectory(string siteId, string directoryName)
        {

            // load virtual directory
            ManagementObject objOrigDir = wmi.GetObject(String.Format("IIsWebVirtualDirSetting='{0}'",
                GetVirtualDirectoryPath(siteId, directoryName)));

            string path = (string)objOrigDir.Properties["Path"].Value;
            string anonymousUsername = (string)objOrigDir.Properties["AnonymousUserName"].Value;

            // remove unnecessary permissions
            RemoveWebFolderPermissions(path, GetNonQualifiedAccountName(anonymousUsername));

            // delete directory
            ManagementObject objDir = wmi.GetObject(String.Format("IIsWebVirtualDir='{0}'",
                GetVirtualDirectoryPath(siteId, directoryName)));
            objDir.Delete();
        }


        public virtual bool AppVirtualDirectoryExists(string siteId, string directoryName)
        {
            return DirectoryEntry.Exists(GetAppVirtualDirectoryADSIPath(siteId, directoryName));
        }

        public virtual WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
        {
            return GetAppVirtualDirectories(siteId, false);
        }

        private WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId, bool includeSystemDirectories)
        {
            List<WebAppVirtualDirectory> dirs = new List<WebAppVirtualDirectory>();

            // MS SharedTools folder
            string sharedToolsFolder = GetMicrosoftSharedFolderPath();

            DirectoryEntry objSite = new DirectoryEntry(GetAppVirtualDirectoryADSIPath(siteId, ""));
            foreach (DirectoryEntry objVirtDir in objSite.Children)
            {
                if (objVirtDir.SchemaClassName == "IIsWebVirtualDir" &&
                    String.Compare(objVirtDir.Name, "root", true) != 0)
                {
                    // this is virtual directory
                    WebAppVirtualDirectory dir = new WebAppVirtualDirectory();
                    dir.Name = objVirtDir.Name;
                    dir.ContentPath = (string)objVirtDir.Properties["Path"].Value;

                    //do not show ColdFusion virtual directories
                    if (dir.ContentPath.Equals(CFScriptsDirectoryPath) || dir.ContentPath.Equals(CFFlashRemotingDirPath))
                    {
                        continue;
                    }

                    // check if this is a system (FrontPage or SharePoint) virtual directory
                    if (!includeSystemDirectories
                        && !String.IsNullOrEmpty(sharedToolsFolder)
                        && dir.ContentPath.ToLower().StartsWith(sharedToolsFolder.ToLower()))
                        continue;

                    // add to the collection
                    dirs.Add(dir);

                    // fill properties
                    //FillAppVirtualDirectoryFromWmiObject(dir, objVirtDir);
                    //FillAppVirtualDirectoryRestFromWmiObject(dir, objVirtDir);
                }
            }

            return dirs.ToArray();
        }

        public virtual WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
        {
            WebAppVirtualDirectory dir = new WebAppVirtualDirectory();
            ManagementObject objDir = wmi.GetObject(
                String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, directoryName)));

            dir.Name = directoryName;

            FillAppVirtualDirectoryFromWmiObject(dir, objDir);
            FillAppVirtualDirectoryRestFromWmiObject(dir, objDir);

            // load parent site settings
            ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

            // check write permissions
            dir.EnableWritePermissions = CheckWriteAccessEnabled(dir.ContentPath,
                GetNonQualifiedAccountName(dir.AnonymousUsername));

            return dir;
        }

        public virtual void CreateAppVirtualDirectoryNonApplication(string siteId, WebAppVirtualDirectory directory)
		{
			// create directory folder if not exists
			if (!FileUtils.DirectoryExists(directory.ContentPath))
				FileUtils.CreateDirectory(directory.ContentPath);

			string dirId = GetAppVirtualDirectoryPath(siteId, directory.Name);

			// create a new virtual directory
			ManagementObject objDir = wmi.GetClass("IIsWebVirtualDir").CreateInstance();
			objDir.Properties["Name"].Value = dirId;
			objDir.Put();

			// update directory properties
			ManagementObject objDirSetting = wmi.GetClass("IIsWebVirtualDirSetting").CreateInstance();
			objDirSetting.Properties["Name"].Value = dirId;
			objDirSetting.Properties["AppFriendlyName"].Value = directory.Name;
			objDirSetting.Properties["Path"].Value = directory.ContentPath;
			objDirSetting.Put();

			// check if site has write permissions enabled
			ManagementObject objSiteSetting = wmi.GetObject(String.Format(
				"IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, "")));

			directory.EnableWritePermissions = CheckWriteAccessEnabled(
				(string)objSiteSetting.Properties["Path"].Value,
				GetNonQualifiedAccountName((string)objSiteSetting.Properties["AnonymousUserName"].Value));

			// update directory
			UpdateAppVirtualDirectory(siteId, directory, false);
		}

		public virtual void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			// create directory folder if not exists
			if (!FileUtils.DirectoryExists(directory.ContentPath))
				FileUtils.CreateDirectory(directory.ContentPath);

			string dirId = GetAppVirtualDirectoryPath(siteId, directory.Name);

			// create a new virtual directory
			ManagementObject objDir = wmi.GetClass("IIsWebVirtualDir").CreateInstance();
			objDir.Properties["Name"].Value = dirId;
			objDir.Put();
			objDir.InvokeMethod("AppCreate", new Object[] { true });

			// update directory properties
			ManagementObject objDirSetting = wmi.GetClass("IIsWebVirtualDirSetting").CreateInstance();
			objDirSetting.Properties["Name"].Value = dirId;
			objDirSetting.Properties["AppFriendlyName"].Value = directory.Name;
			objDirSetting.Properties["Path"].Value = directory.ContentPath;
			objDirSetting.Put();

			// check if site has write permissions enabled
			ManagementObject objSiteSetting = wmi.GetObject(String.Format(
				"IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, "")));

			directory.EnableWritePermissions = CheckWriteAccessEnabled(
				(string)objSiteSetting.Properties["Path"].Value,
				GetNonQualifiedAccountName((string)objSiteSetting.Properties["AnonymousUserName"].Value));

			// update directory
			UpdateAppVirtualDirectory(siteId, directory, false);
		}

		public virtual void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
		{
			UpdateAppVirtualDirectory(siteId, directory, true);
		}

		private void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory, bool updateProperties)
		{
			// load parent site settings
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));
			ManagementObject objSiteSetting = wmi.GetObject(String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, "")));
			string siteName = (string)objSite.Properties["ServerComment"].Value;

			string poolName1 = siteName + DEDICATED_POOL_SUFFIX_ASPNET1;
			string poolName2 = siteName + DEDICATED_POOL_SUFFIX_ASPNET2;
			string poolName4 = siteName + DEDICATED_POOL_SUFFIX_ASPNET4;

			string sitePoolName = (string)objSiteSetting.Properties["AppPoolId"].Value;

			if (String.IsNullOrEmpty(directory.AnonymousUsername) ||
				String.IsNullOrEmpty(directory.AnonymousUserPassword))
			{
				directory.AnonymousUsername = GetNonQualifiedAccountName((string)objSiteSetting.Properties["AnonymousUserName"].Value);
				directory.AnonymousUserPassword = (string)objSiteSetting.Properties["AnonymousUserPass"].Value;
			}

			bool dedicatedPool = CheckIsDedicatedPoolEnabled(siteName, sitePoolName);

			// load original virt dir
			ManagementObject objDir = wmi.GetObject(String.Format("IIsWebVirtualDirSetting='{0}'",
				GetAppVirtualDirectoryPath(siteId, directory.Name)));

			string origPath = (string)objDir.Properties["Path"].Value;

			string sharedToolsFolder = GetMicrosoftSharedFolderPath();
			// remove unnecessary permissions
			// if original folder has been changed
			if (!String.IsNullOrEmpty(sharedToolsFolder) &&
				!origPath.ToLower().StartsWith(sharedToolsFolder.ToLower()))
			{
				if (String.Compare(origPath, directory.ContentPath, true) != 0)
					RemoveWebFolderPermissions(origPath, directory.AnonymousUsername);
			}

			// set folder permissions
			if (!String.IsNullOrEmpty(sharedToolsFolder) &&
				!directory.ContentPath.ToLower().StartsWith(sharedToolsFolder.ToLower()))
			{
				SetWebFolderPermissions(directory.ContentPath, directory.AnonymousUsername,
					directory.EnableWritePermissions, dedicatedPool);
			}

			if (updateProperties)
			{
				string customAppPool = directory.ApplicationPool;

				// set pool accordingly to ASP.NET
				switch (directory.AspNetInstalled)
				{
					case ASPNET_11:
						directory.ApplicationPool = (dedicatedPool) ? poolName1 : Asp11Pool;
						break;
					case ASPNET_20:
						directory.ApplicationPool = (dedicatedPool) ? poolName2 : Asp20Pool;
						break;
					case ASPNET_40:
						directory.ApplicationPool = (dedicatedPool) ? poolName4 : Asp40Pool;
						break;
				}

				bool standardPool = (String.Compare(customAppPool, Asp11Pool, true) == 0
					|| String.Compare(customAppPool, Asp20Pool, true) == 0
					|| String.Compare(customAppPool, Asp40Pool, true) == 0
					|| String.Compare(customAppPool, poolName1, true) == 0
					|| String.Compare(customAppPool, poolName2, true) == 0
					|| String.Compare(customAppPool, poolName4, true) == 0);

				if (!standardPool && !String.IsNullOrEmpty(customAppPool))
					directory.ApplicationPool = customAppPool;

				FillWmiObjectFromAppVirtualDirectory(objDir, directory, true);
				FillWmiObjectFromAppVirtualDirectoryRest(objDir, directory);

				// UNC Share
				ManagementObject objVirtDirUnc = wmi.GetObject(
					String.Format("IIsWebVirtualDir='{0}'", GetAppVirtualDirectoryPath(siteId, directory.Name)));
				FillWmiObjectUNCSettingsFromAppVirtualDirectory(objVirtDirUnc, directory);
				objVirtDirUnc.Put();

				// save account
				objDir.Put();
			}
		}

		public virtual void DeleteAppVirtualDirectory(string siteId, string directoryName)
		{
			// load virtual directory
			ManagementObject objOrigDir = wmi.GetObject(String.Format("IIsWebVirtualDirSetting='{0}'",
				GetAppVirtualDirectoryPath(siteId, directoryName)));

			string path = (string)objOrigDir.Properties["Path"].Value;
			string anonymousUsername = (string)objOrigDir.Properties["AnonymousUserName"].Value;

			// remove unnecessary permissions
			RemoveWebFolderPermissions(path, GetNonQualifiedAccountName(anonymousUsername));

			// delete directory
			ManagementObject objDir = wmi.GetObject(String.Format("IIsWebVirtualDir='{0}'",
				GetAppVirtualDirectoryPath(siteId, directoryName)));
			objDir.Delete();
		}

		protected string GetMicrosoftSharedFolderPath()
		{
			string sharedToolsFolder = null;
			try
			{
				RegistryKey stKey = Registry.LocalMachine.OpenSubKey(SHARED_TOOLS_REGLOC);
				if (stKey != null)
				{
					string sharedFilesDir = stKey.GetValue("SharedFilesDir") as string;
					//
					if (!String.IsNullOrEmpty(sharedFilesDir))
						sharedToolsFolder = sharedFilesDir.ToLower();
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("GetMicrosoftSharedFolderPath: Could not read SharedTools registry key location", ex);
			}
			return sharedToolsFolder;
		}
		#endregion

		#region ColdFusion

		public virtual bool IsColdFusionSystemInstalled()
		{
			return (IsColdFusion8Installed() || IsColdFusion7Installed() || IsColdFusion9Installed() || IsColdFusion10Installed() || IsColdFusion11Installed() || IsColdFusion2016Installed());
		}

		protected bool IsColdFusion2016Installed()
		{
			RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_2016_REGLOC);
			if (keyColdFusion == null)
			{
				keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_2016_REGLOC_X64);
				if (keyColdFusion == null)
					return false;
			}

			if (!String.IsNullOrEmpty((string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH)))
			{
				return true;
			}
			return false;
		}
		
		protected bool IsColdFusion11Installed()
		{
			RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_11_REGLOC);
			if (keyColdFusion == null)
			{
				keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_11_REGLOC_X64);
				if (keyColdFusion == null)
					return false;
			}

			if (!String.IsNullOrEmpty((string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH)))
			{
				return true;
			}
			return false;
		}
		
		protected bool IsColdFusion10Installed()
		{
			RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_10_REGLOC);
			if (keyColdFusion == null)
			{
				keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_10_REGLOC_X64);
				if (keyColdFusion == null)
					return false;
			}

			if (!String.IsNullOrEmpty((string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH)))
			{
				return true;
			}
			return false;
		}
		
		protected bool IsColdFusion9Installed()
		{
			RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_9_REGLOC);
			if (keyColdFusion == null)
			{
				keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_9_REGLOC_X64);
				if (keyColdFusion == null)
					return false;
			}

			if (!String.IsNullOrEmpty((string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH)))
			{
				return true;
			}
			return false;
		}


		protected bool IsColdFusion8Installed()
		{
			RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_8_REGLOC);
			if (keyColdFusion == null)
			{
				keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_8_REGLOC_X64);
				if (keyColdFusion == null)
					return false;
			}

			if (!String.IsNullOrEmpty((string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH)))
			{
				return true;
			}
			return false;
		}

		protected bool IsColdFusion7Installed()
		{
			RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_7_REGLOC);
			if (keyColdFusion == null)
			{
				keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_7_REGLOC_X64);
				if (keyColdFusion == null)
					return false;
			}
			if (!String.IsNullOrEmpty((string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH)))
			{
				return true;
			}
			return false;
		}

		protected string GetColdFusionRootPath()
		{
			if (IsColdFusion8Installed())
			{
				RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_8_REGLOC);
				if (keyColdFusion == null)
				{
					keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_8_REGLOC_X64);
					if (keyColdFusion == null)
						return String.Empty;
				}
				return (string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH);
			}
			if (IsColdFusion7Installed())
			{
				RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_7_REGLOC);
				if (keyColdFusion == null)
				{
					keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_7_REGLOC_X64);
					if (keyColdFusion == null)
						return String.Empty;
				}
				return (string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH);
			}

			if (IsColdFusion9Installed())
			{
				RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_9_REGLOC);
				if (keyColdFusion == null)
				{
					keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_9_REGLOC_X64);
					if (keyColdFusion == null)
						return String.Empty;
				}
				return (string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH);
			}

			return String.Empty;
			
			if (IsColdFusion10Installed())
			{
				RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_10_REGLOC);
				if (keyColdFusion == null)
				{
					keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_10_REGLOC_X64);
					if (keyColdFusion == null)
						return String.Empty;
				}
				return (string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH);
			}

			return String.Empty;
			
			if (IsColdFusion11Installed())
			{
				RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_11_REGLOC);
				if (keyColdFusion == null)
				{
					keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_11_REGLOC_X64);
					if (keyColdFusion == null)
						return String.Empty;
				}
				return (string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH);
			}

			return String.Empty;

			if (IsColdFusion2016Installed())
			{
				RegistryKey keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_2016_REGLOC);
				if (keyColdFusion == null)
				{
					keyColdFusion = Registry.LocalMachine.OpenSubKey(COLDFUSION_2016_REGLOC_X64);
					if (keyColdFusion == null)
						return String.Empty;
				}
				return (string)keyColdFusion.GetValue(COLDFUSION_ROOT_PATH);
			}

			return String.Empty;		
			
		}

		protected void EnableColdFusionScripting(string siteName)
		{
			//cf_root/runtime/bin/wsconfig.exe -server coldfusion -ws iis -site "web31" -coldfusion -v
			string pathWs = Path.Combine(GetColdFusionRootPath(), @"runtime\bin\wsconfig.exe");
			string enableCF = String.Format("{0} -server coldfusion -ws iis -site \"{1}\" -coldfusion -v -norestart", pathWs, siteName);
			try
			{
				FileUtils.ExecuteCmdCommand(enableCF);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
			}

		}

		public bool IsColdFusionEnabled(string siteId)
		{
			return IsColdFusionEnabledOnSite(siteId);
		}

		protected virtual bool IsColdFusionEnabledOnSite(string siteId)
		{
			bool isCFenabled = false;

			string[] split = siteId.Split(new char[] { '/' });
			string Id = split[1];

			if (IsColdFusionSystemInstalled())
			{
				string pathWsConfigSettings = Path.Combine(GetColdFusionRootPath(), @"runtime\lib\wsconfig\wsconfig.properties");
				StreamReader file = new StreamReader(pathWsConfigSettings);
				string line = String.Empty;
				int counter = 0;
				while ((line = file.ReadLine()) != null)
				{
					if (line.Contains(Id))
					{
						isCFenabled = true;
						break;
					}
					counter++;
				}
				file.Close();
			}

			return isCFenabled;
		}

		protected void DisableColdFusionScripting(string siteId, string siteName)
		{

			//wsconfig.exe -remove -ws iis -site "tube.com" -v
			string pathWs = Path.Combine(GetColdFusionRootPath(), @"runtime\bin");
			//string command = String.Format("wsconfig.exe -remove -ws iis -site \"{0}\" -v", siteName);

			string execpath = Path.Combine(GetColdFusionRootPath(), @"runtime\bin\wsconfig.exe");
			string command = String.Format("{0} -remove -ws iis -site \"{1}\" -v", execpath, siteName);

			if (IsColdFusionEnabledOnSite(siteId))
			{
				try
				{
					FileUtils.ExecuteCmdCommand(command);
				}
				catch (Exception ex)
				{
					Log.WriteError(ex);
				}
			}
		}

		#endregion

		#region FrontPage
		public virtual bool IsFrontPageSystemInstalled()
		{
			return (IsFrontPage2000Installed() || IsFrontPage2002Installed());
		}

		private bool IsFrontPage2000Installed()
		{
			// query IIS filters to determine if FPSE 2000 is installed
			ManagementObject objFilter = wmi.GetObject(String.Format("IIsFilter.Name='{0}'",
				FRONTPAGE_PATH));

			try
			{
				object test = objFilter.Properties["Name"].Value;
				return true;
			}
			catch
			{
				return false;
			}
		}

		protected bool IsFrontPage2002Installed()
		{
			// we will lookup in the registry for the required information
			// check for FPSE 2002
			RegistryKey keyFrontPage = Registry.LocalMachine.OpenSubKey(FRONTPAGE_2002_REGLOC);
			if (keyFrontPage == null)
			{
				keyFrontPage = Registry.LocalMachine.OpenSubKey(FRONTPAGE_2002_REGLOC_X64);
				if (keyFrontPage == null)
				{
					return false;
				}
			}

			string[] subKeys = keyFrontPage.GetSubKeyNames();
			if (subKeys != null && subKeys.Length > 0)
			{
				foreach (string key in subKeys)
				{
					if (key == FRONTPAGE_2002_INSTALLED || key == SHAREPOINT_INSTALLED)
						return true;
				}
			}

			return false;
		}

		public virtual bool IsFrontPageInstalled(string siteId)
		{
			return (IsFrontPage2000InstalledOnWebSite(siteId) ||
				IsFrontPage2002InstalledOnWebSite(siteId));
		}

		private bool IsFrontPage2000InstalledOnWebSite(string siteId)
		{
			// site port
			RegistryKey sitePortKey = Registry.LocalMachine.OpenSubKey(String.Format("{0}Port /LM/{1}:",
				FRONTPAGE_PORT_REGLOC, siteId));

			if (sitePortKey == null)
				return false;

			// get required keys
			string keyAuthoring = (string)sitePortKey.GetValue("authoring");
			string keyFrontPageRoot = (string)sitePortKey.GetValue("frontpageroot");

			return (keyAuthoring != null && keyAuthoring.ToUpper() == "ENABLED" &&
				keyFrontPageRoot != null && keyFrontPageRoot.IndexOf("\\40") != -1);
		}

		private bool IsFrontPage2002InstalledOnWebSite(string siteId)
		{
			// site port
			RegistryKey sitePortKey = Registry.LocalMachine.OpenSubKey(String.Format("{0}Port /LM/{1}:",
				FRONTPAGE_PORT_REGLOC, siteId));

			if (sitePortKey == null)
			{
				sitePortKey = Registry.LocalMachine.OpenSubKey(String.Format("{0}Port /LM/{1}:",
				FRONTPAGE_PORT_REGLOC_X64, siteId));
				if (sitePortKey == null)
				{
					return false;
				}
			}

			// get required keys
			string keyAuthoring = (string)sitePortKey.GetValue("authoring");
			string keyFrontPageRoot = (string)sitePortKey.GetValue("frontpageroot");

			return (keyAuthoring != null && keyAuthoring.ToUpper() == "ENABLED" &&
				keyFrontPageRoot != null && keyFrontPageRoot.IndexOf("\\50") != -1);
		}

		public virtual bool InstallFrontPage(string siteId, string username, string password)
		{
			if (SecurityUtils.UserExists(username, ServerSettings, UsersOU))
				return false;

			// create user account
			SystemUser user = new SystemUser();
			user.Name = username;
			user.FullName = username;
			user.Description = "SolidCP System Account";
			user.Password = password;
			user.PasswordCantChange = true;
			user.PasswordNeverExpires = true;
			user.AccountDisabled = false;
			user.System = true;

			// create in the system
			SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);

			string cmdPath = null;
			string cmdArgs = null;

			// try to install FPSE2002 first
			if (IsFrontPage2002Installed())
			{
				// add registry key for anonymous group if not exists
				RegistryKey portsKey = Registry.LocalMachine.OpenSubKey(FRONTPAGE_ALLPORTS_REGLOC, true) ??
									   Registry.LocalMachine.OpenSubKey(FRONTPAGE_ALLPORTS_REGLOC_X64, true);

				if (portsKey != null) portsKey.SetValue("anonusergroupprefix", "anonfp");

				// create anonymous group
				string groupName = "anonfp_" + siteId.Substring(siteId.IndexOf("/") + 1);

				int numberOfatempts = 0;

				while (!SecurityUtils.GroupExists(groupName, ServerSettings, GroupsOU) && numberOfatempts < 5)
				{
					SystemGroup fpseGroup = new SystemGroup();
					fpseGroup.Name = groupName;
					fpseGroup.Description = "Anonymous FPSE group for " + siteId + " web site";
					fpseGroup.Members = new string[] { username };
					SecurityUtils.CreateGroup(fpseGroup, ServerSettings, UsersOU, GroupsOU);
					numberOfatempts++;
				}

				// install FPSE 2002
				RegistryKey fpKey = Registry.LocalMachine.OpenSubKey(FRONTPAGE_2002_REGLOC) ??
									Registry.LocalMachine.OpenSubKey(FRONTPAGE_2002_REGLOC_X64);

				if (fpKey != null)
				{
					string location = (string)fpKey.GetValue("Location");
					cmdPath = location + @"\bin\owsadm.exe";
				}
				cmdArgs = String.Format("-o install -p /LM/{0} -type msiis -u {1}",
					siteId, username);
			}
			else if (IsFrontPage2000Installed())
			{
				// install FPSE 2000
				RegistryKey fpKey = Registry.LocalMachine.OpenSubKey(FRONTPAGE_2000_REGLOC) ??
								   Registry.LocalMachine.OpenSubKey(FRONTPAGE_2000_REGLOC_X64);

				if (fpKey != null)
				{
					string location = (string)fpKey.GetValue("Location");
					cmdPath = location + @"\bin\fpsrvadm.exe";
				}
				cmdArgs = String.Format("-o install -p /LM/{0} -type msiis -u {1}",
					siteId, username);
			}

			if (cmdPath != null)
			{
				// launch system process
				string result = FileUtils.ExecuteSystemCommand(cmdPath, cmdArgs);
			}

			// update web site
			WebSite site = GetSite(siteId);
			if (site != null)
			{
				site.EnableWindowsAuthentication = true;
				UpdateSite(site);
			}

			return true;
		}

		public virtual void UninstallFrontPage(string siteId, string username)
		{
			string cmdPath = null;
			string cmdArgs = null;

			// try to install FPSE2002 first
			if (IsFrontPage2002InstalledOnWebSite(siteId))
			{
				// uninstall FPSE 2002
				RegistryKey fpKey = Registry.LocalMachine.OpenSubKey(FRONTPAGE_2002_REGLOC) ??
									Registry.LocalMachine.OpenSubKey(FRONTPAGE_2002_REGLOC_X64);

				if (fpKey != null)
				{
					string location = (string)fpKey.GetValue("Location");
					cmdPath = location + @"\bin\owsadm.exe";
				}
				cmdArgs = "-o uninstall -p /LM/" + siteId;

				// remove anonymous group
				string groupName = "anonfp_" + siteId.Substring(siteId.IndexOf("/") + 1);
				if (SecurityUtils.GroupExists(groupName, ServerSettings, GroupsOU))
					SecurityUtils.DeleteGroup(groupName, ServerSettings, GroupsOU);
			}
			else if (IsFrontPage2000InstalledOnWebSite(siteId))
			{
				// uninstall FPSE 2000
				RegistryKey fpKey = Registry.LocalMachine.OpenSubKey(FRONTPAGE_2000_REGLOC);
				if (fpKey != null)
				{
					string location = (string)fpKey.GetValue("Location");
					cmdPath = location + @"\bin\fpsrvadm.exe";
				}
				cmdArgs = "-o uninstall -p /LM/" + siteId;
			}

			if (cmdPath != null)
			{
				// launch system process
				string result = FileUtils.ExecuteSystemCommand(cmdPath, cmdArgs);
			}

			// delete user account
			if (SecurityUtils.UserExists(username, ServerSettings, UsersOU))
				SecurityUtils.DeleteUser(username, ServerSettings, UsersOU);
		}

		public virtual void ChangeFrontPagePassword(string username, string password)
		{
			// change password
			if (SecurityUtils.UserExists(username, ServerSettings, UsersOU))
				SecurityUtils.ChangeUserPassword(username, password, ServerSettings, UsersOU);
		}

		private bool IsSharePointInstalledOnWebSite(string siteId)
		{
			// site port
			RegistryKey sitePortKey = Registry.LocalMachine.OpenSubKey(String.Format("{0}Port /LM/{1}:",
																					SHAREPOINT_PORT_REGLOC, siteId)) ??
									 Registry.LocalMachine.OpenSubKey(String.Format("{0}Port /LM/{1}:", SHAREPOINT_PORT_REGLOC_X64, siteId));


			if (sitePortKey == null)
				return false;

			// get required keys
			byte[] serverId = (byte[])sitePortKey.GetValue("virtualserverid");
			return (serverId != null);
		}
		#endregion

		#region Application Pools
		private bool ApplicationPoolExists(string name)
		{
			return (wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsApplicationPool WHERE Name='W3SVC/AppPools/{0}'", name)).Count > 0);
		}

		private void CreateApplicationPool(string name, string username, string password)
		{
			// create pool
			ManagementObject objPool = wmi.GetClass("IIsApplicationPool").CreateInstance();
			objPool.Properties["Name"].Value = "W3SVC/AppPools/" + name;
			objPool.Put();

			// specify pool properties
			objPool = wmi.GetClass("IIsApplicationPoolSetting").CreateInstance();
			objPool.Properties["Name"].Value = "W3SVC/AppPools/" + name;

			if (!String.IsNullOrEmpty(username))
			{
				objPool.Properties["WAMUserName"].Value = GetQualifiedAccountName(username);
				objPool.Properties["WAMUserPass"].Value = password;
				objPool.Properties["AppPoolIdentityType"].Value = 3;
			}
			else
			{
				objPool.Properties["AppPoolIdentityType"].Value = 2;
			}
			objPool.Put();
		}

		private void DeleteApplicationPool(string name)
		{
			ManagementObject objPool = wmi.GetObject(String.Format("IIsApplicationPool='W3SVC/AppPools/{0}'",
				name));
			objPool.Delete();
		}
		#endregion

		#region Permissions
		public void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
		{
			// TODO
		}
		#endregion

		#region Secured Folders
		protected virtual bool IsSecuredFoldersInstalled(string siteId)
		{
			return (wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsFilterSetting WHERE Name='{0}'",
				GetFilterPath(siteId, IIS_PASSWORD_FILTER))).Count > 0);
		}

		public virtual void InstallSecuredFolders(string siteId)
		{
			if (IsSecuredFoldersInstalled(siteId)
				|| String.IsNullOrEmpty(SecuredFoldersFilterPath))
				return;

			string wmiPath = GetFilterPath(siteId, IIS_PASSWORD_FILTER);

			// add filter object
			ManagementObject objFilter = wmi.GetClass("IIsFilter").CreateInstance();
			objFilter.Properties["Name"].Value = wmiPath;
			objFilter.Put();

			// add filter settings
			ManagementObject objFilterSetting = wmi.GetClass("IIsFilterSetting").CreateInstance();
			objFilterSetting.Properties["Name"].Value = wmiPath;
			objFilterSetting.Properties["FilterEnabled"].Value = true;
			//objFilterSetting.Properties["FilterFlags"].Value = 147459;
			objFilterSetting.Properties["FilterPath"].Value = SecuredFoldersFilterPath;
			objFilterSetting.Put();

			// change filters order
			ChangeFiltersOrder(siteId, IIS_PASSWORD_FILTER, false);
		}

		public virtual void UninstallSecuredFolders(string siteId)
		{
			ManagementObjectCollection objFilters = wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsFilterSetting WHERE Name='{0}'",
				GetFilterPath(siteId, IIS_PASSWORD_FILTER)));

			if (objFilters.Count > 0)
				foreach (ManagementObject objFilter in objFilters)
					objFilter.Delete();

			// change filters order
			ChangeFiltersOrder(siteId, IIS_PASSWORD_FILTER, true);
		}

		private void ChangeFiltersOrder(string siteId, string filterName, bool remove)
		{
			// load filters object
			ManagementObject objFilters = wmi.GetObject(String.Format("IIsFiltersSetting='{0}'",
				GetFilterPath(siteId, "")));

			string filtersOrder = (string)objFilters.Properties["FilterLoadOrder"].Value;

			List<string> updatedFilters = new List<string>();
			if (!String.IsNullOrEmpty(filtersOrder))
			{
				string[] existingFilters = filtersOrder.Split(',');
				foreach (string existingFilter in existingFilters)
				{
					if (String.Compare(filterName, existingFilter, true) != 0)
						updatedFilters.Add(existingFilter);
				}
			}

			if (!remove)
				updatedFilters.Add(filterName);

			// update filters order
			objFilters.Properties["FilterLoadOrder"].Value = (updatedFilters.Count > 0)
				? String.Join(",", updatedFilters.ToArray()) : null;
			objFilters.Put();
		}

		public List<WebFolder> GetFolders(string siteId)
		{
			string rootPath = GetSiteContentPath(siteId);
			List<WebFolder> folders = new List<WebFolder>();

			string foldersFile = Path.Combine(rootPath, ProtectedFoldersFile);
			if (File.Exists(foldersFile))
			{
				List<string> list = ReadFile(foldersFile);

				// read the list of secured folders
				foreach (string f in list)
				{
					WebFolder folder = new WebFolder();
					folder.Title = "";
					folder.Path = f;
					folder.Groups = new string[] { };
					folder.Users = new string[] { };
					folders.Add(folder);
				}
			}

			return folders;
		}

		public WebFolder GetFolder(string siteId, string folderPath)
		{
			// read folder file
			string rootPath = GetSiteContentPath(siteId);
			string normalizedPath = NormalizeFolderPath(folderPath);
			string path = Path.Combine(rootPath, normalizedPath);
			path = Path.Combine(path, ProtectedAccessFile);

			List<string> folderLines = ReadFile(path);
			if (folderLines.Count == 0)
				return null;

			// parse file
			WebFolder folder = new WebFolder();
			folder.Path = "\\" + normalizedPath;
			folder.Title = "";
			folder.Users = new string[] { };
			folder.Groups = new string[] { };

			foreach (string line in folderLines)
			{
				string lowerLine = line.ToLower();
				if (lowerLine.StartsWith(AUTH_NAME_DIRECTIVE.ToLower()))
				{
					folder.Title = line.Substring(AUTH_NAME_DIRECTIVE.Length);
					continue;
				}
				else if (lowerLine.StartsWith(REQUIRE_USER_DIRECTIVE.ToLower()))
				{
					string users = line.Substring(REQUIRE_USER_DIRECTIVE.Length);
					folder.Users = users.Split(' ');
					continue;
				}
				else if (lowerLine.StartsWith(REQUIRE_GROUP_DIRECTIVE.ToLower()))
				{
					string groups = line.Substring(REQUIRE_GROUP_DIRECTIVE.Length);
					folder.Groups = groups.Split(' ');
					continue;
				}
			}

			return folder;
		}

		public void UpdateFolder(string siteId, WebFolder folder)
		{
			UpdateFolder(siteId, folder, false);
		}

		private void UpdateFolder(string siteId, WebFolder folder, bool deleteFolder)
		{
			// file path
			string rootPath = GetSiteContentPath(siteId);
			string normalizedPath = NormalizeFolderPath(folder.Path);

			string path = Path.Combine(rootPath, normalizedPath);
			path = Path.Combine(path, ProtectedAccessFile);

			if (!deleteFolder)
			{
				// create .htaccess file
				List<string> accessLines = new List<string>();

				// title
				if (!String.IsNullOrEmpty(folder.Title))
					accessLines.Add(AUTH_NAME_DIRECTIVE + folder.Title);

				// link to users file
				string usersFile = Path.Combine(rootPath, ProtectedUsersFile);
				if (!File.Exists(usersFile))
					WriteFile(usersFile, new List<string>());
				accessLines.Add(ProtectedUsersFile_DIRECTIVE + usersFile);

				// link to groups file
				string groupsFile = Path.Combine(rootPath, ProtectedGroupsFile);
				if (!File.Exists(groupsFile))
					WriteFile(groupsFile, new List<string>());
				accessLines.Add(ProtectedGroupsFile_DIRECTIVE + groupsFile);

				// require users
				if (folder.Users != null && folder.Users.Length > 0)
					accessLines.Add(REQUIRE_USER_DIRECTIVE + String.Join(" ", folder.Users));

				// require groups
				if (folder.Groups != null && folder.Groups.Length > 0)
					accessLines.Add(REQUIRE_GROUP_DIRECTIVE + String.Join(" ", folder.Groups));

				// write access file
				WriteFile(path, accessLines);
			}
			else
			{
				if (File.Exists(path))
					File.Delete(path);
			}

			// update folders list
			List<WebFolder> folders = GetFolders(siteId);
			List<string> updatedFolders = new List<string>();
			bool exists = false;
			string folderName = "\\" + normalizedPath;
			foreach (WebFolder f in folders)
			{
				if (String.Compare(f.Path, folderName, true) == 0)
				{
					// exists
					exists = true;

					if (deleteFolder)
						continue;
				}

				updatedFolders.Add(f.Path);
			}

			if (!exists && !deleteFolder)
				updatedFolders.Add(folderName);

			// save folders list
			string foldersFile = Path.Combine(rootPath, ProtectedFoldersFile);
			WriteFile(foldersFile, updatedFolders);
		}

		public void DeleteFolder(string siteId, string folderPath)
		{
			WebFolder folder = new WebFolder();
			folder.Path = folderPath;

			// delete folder
			UpdateFolder(siteId, folder, true);
		}

		private string NormalizeFolderPath(string str)
		{
			if (str.StartsWith("\\"))
				return str.Substring(1);

			return str;
		}
		#endregion

		#region Secured Users
		public List<WebUser> GetUsers(string siteId)
		{
			string rootPath = GetSiteContentPath(siteId);
			List<WebUser> users = new List<WebUser>();

			// load users file
			string usersPath = Path.Combine(rootPath, ProtectedUsersFile);
			List<string> lines = ReadFile(usersPath);

			// iterate through all lines
			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i];

				int colonIdx = line.IndexOf(":");
				if (colonIdx != -1)
				{
					string username = line.Substring(0, colonIdx);

					// add it to the return collection
					users.Add(GetUser(siteId, username));
				}
			}

			return users;
		}

		public WebUser GetUser(string siteId, string userName)
		{
			// load users file
			string rootPath = GetSiteContentPath(siteId);
			string usersPath = Path.Combine(rootPath, ProtectedUsersFile);
			List<string> lines = ReadFile(usersPath);

			// iterate through all lines
			WebUser user = null;
			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i];

				int colonIdx = line.IndexOf(":");
				if (colonIdx != -1)
				{
					string username = line.Substring(0, colonIdx);
					string password = line.Substring(colonIdx + 1);
					if (String.Compare(username, userName, true) == 0)
					{
						// exists
						user = new WebUser();
						user.Name = username;
						user.Password = password;
						break;
					}
				}
			}

			if (user == null)
				return null; // user doesn't exist

			List<string> userGroups = new List<string>();

			// read groups information
			// open groups file
			string groupsPath = Path.Combine(rootPath, ProtectedGroupsFile);
			List<string> groupLines = ReadFile(groupsPath);

			for (int i = 0; i < groupLines.Count; i++)
			{
				string groupLine = groupLines[i];
				int colonIdx = groupLine.IndexOf(":");
				if (colonIdx != -1)
				{
					string groupName = groupLine.Substring(0, colonIdx);
					string[] groupMembers = groupLine.Substring(colonIdx + 1).Split(' ');

					// check group members
					for (int j = 0; j < groupMembers.Length; j++)
					{
						if (String.Compare(groupMembers[j], user.Name, true) == 0)
						{
							userGroups.Add(groupName);
							break;
						}
					}
				}
			} // end iterating groups
			user.Groups = userGroups.ToArray();

			return user;
		}

		public void UpdateUser(string siteId, WebUser user)
		{
			UpdateUser(siteId, user, false);
		}

		private void UpdateUser(string siteId, WebUser user, bool deleteUser)
		{
			string rootPath = GetSiteContentPath(siteId);
			string usersPath = Path.Combine(rootPath, ProtectedUsersFile);

			// load users file
			List<string> lines = ReadFile(usersPath);

			// check if the user already exists
			List<string> updatedLines = new List<string>();
			bool exists = false;
			for (int i = 0; i < lines.Count; i++)
			{
				string line = lines[i];
				string updatedLine = line;

				int colonIdx = line.IndexOf(":");
				if (colonIdx != -1)
				{
					string username = line.Substring(0, colonIdx);
					string password = line.Substring(colonIdx + 1);
					if (String.Compare(username, user.Name, true) == 0)
					{
						// already exists
						exists = true;

						// check if we need to delete this user
						if (deleteUser)
							continue;

						// change password if required
						if (!String.IsNullOrEmpty(user.Password))
						{
							// change password
							BsdDES des = new BsdDES();
							password = des.Crypt(user.Password);

							// update line
							updatedLine = username + ":" + password;
						}
					}
				}

				updatedLines.Add(updatedLine);
			}

			if (!exists && !deleteUser)
			{
				// new user has been added
				BsdDES des = new BsdDES();
				updatedLines.Add(user.Name + ":" + des.Crypt(user.Password));
			}

			// save users file
			WriteFile(usersPath, updatedLines);

			if (user.Groups == null)
				user.Groups = new string[] { };

			// update groups
			// open groups file
			string groupsPath = Path.Combine(rootPath, ProtectedGroupsFile);
			List<string> groupLines = ReadFile(groupsPath);

			for (int i = 0; i < groupLines.Count; i++)
			{
				string groupLine = groupLines[i];
				int colonIdx = groupLine.IndexOf(":");
				if (colonIdx != -1)
				{
					string groupName = groupLine.Substring(0, colonIdx);
					string[] groupMembers = groupLine.Substring(colonIdx + 1).Split(' ');

					// check if user is assigned to this group
					bool assigned = false;
					for (int j = 0; j < user.Groups.Length; j++)
					{
						if (String.Compare(user.Groups[j], groupName, true) == 0)
						{
							assigned = true;
							break;
						}
					}

					// remove current user
					List<string> updatedMembers = new List<string>();
					for (int j = 0; j < groupMembers.Length; j++)
					{
						// user exists in the members
						// check if he should be really added to this group
						if (String.Compare(groupMembers[j], user.Name, true) == 0)
							continue;

						updatedMembers.Add(groupMembers[j]);
					}

					if (assigned)
						updatedMembers.Add(user.Name);

					// modify group line
					groupLines[i] = groupName + ":" + String.Join(" ", updatedMembers.ToArray());
				}
			} // end iterating groups

			// save group file
			WriteFile(groupsPath, groupLines);
		}

		public void DeleteUser(string siteId, string userName)
		{
			string rootPath = GetSiteContentPath(siteId);
			WebUser user = new WebUser();
			user.Name = userName;

			// update users and groups
			UpdateUser(siteId, user, true);

			// update foleds
			DeleteNonexistentUsersAndGroups(rootPath);
		}
		#endregion

		#region Secured Groups
		public List<WebGroup> GetGroups(string siteId)
		{
			string rootPath = GetSiteContentPath(siteId);
			List<WebGroup> groups = new List<WebGroup>();

			// open groups file
			string groupsPath = Path.Combine(rootPath, ProtectedGroupsFile);
			List<string> groupLines = ReadFile(groupsPath);

			for (int i = 0; i < groupLines.Count; i++)
			{
				string groupLine = groupLines[i];
				int colonIdx = groupLine.IndexOf(":");
				if (colonIdx != -1)
				{
					string name = groupLine.Substring(0, colonIdx);

					// add group to the collection
					groups.Add(GetGroup(siteId, name));
				}
			} // end iterating groups

			return groups;
		}

		public WebGroup GetGroup(string siteId, string groupName)
		{
			string rootPath = GetSiteContentPath(siteId);
			// open groups file
			string groupsPath = Path.Combine(rootPath, ProtectedGroupsFile);
			List<string> groupLines = ReadFile(groupsPath);

			WebGroup group = null;
			for (int i = 0; i < groupLines.Count; i++)
			{
				string groupLine = groupLines[i];
				int colonIdx = groupLine.IndexOf(":");
				if (colonIdx != -1)
				{
					string name = groupLine.Substring(0, colonIdx);
					string[] members = groupLine.Substring(colonIdx + 1).Split(' ');

					if (String.Compare(groupName, name, true) == 0)
					{
						group = new WebGroup();
						group.Name = groupName;
						group.Users = members;
					}
				}
			} // end iterating groups

			return group;
		}

		public void UpdateGroup(string siteId, WebGroup group)
		{
			UpdateGroup(siteId, group, false);
		}

		private void UpdateGroup(string siteId, WebGroup group, bool deleteGroup)
		{
			string rootPath = GetSiteContentPath(siteId);

			if (group.Users == null)
				group.Users = new string[] { };

			List<string> updatedGroups = new List<string>();

			// open groups file
			string groupsPath = Path.Combine(rootPath, ProtectedGroupsFile);
			List<string> groupLines = ReadFile(groupsPath);

			bool exists = false;
			for (int i = 0; i < groupLines.Count; i++)
			{
				string groupLine = groupLines[i];
				int colonIdx = groupLine.IndexOf(":");
				if (colonIdx != -1)
				{
					string name = groupLine.Substring(0, colonIdx);

					// add group to the collection
					if (String.Compare(group.Name, name, true) == 0)
					{
						exists = true;

						if (deleteGroup)
							continue;

						// update group members
						groupLine = group.Name + ":" + String.Join(" ", group.Users);
					}
				}

				updatedGroups.Add(groupLine);
			} // end iterating groups

			if (!exists && !deleteGroup)
				updatedGroups.Add(group.Name + ":" + String.Join(" ", group.Users));

			// save groups
			WriteFile(groupsPath, updatedGroups);
		}

		public void DeleteGroup(string siteId, string groupName)
		{
			string rootPath = GetSiteContentPath(siteId);

			// delete group
			WebGroup group = new WebGroup();
			group.Name = groupName;
			UpdateGroup(siteId, group, true);

			// update foleds
			DeleteNonexistentUsersAndGroups(rootPath);
		}

		#endregion

		#region Helicon Ape

		public virtual HeliconApeStatus GetHeliconApeStatus(string siteId)
		{
			// Helicon Ape does not work on iis 6
			return new HeliconApeStatus { IsEnabled = false, IsInstalled = false };
		}

		public virtual void InstallHeliconApe(string ServiceId)
		{
			throw new NotImplementedException();
		}

		public virtual void EnableHeliconApe(string siteId)
		{
			throw new NotImplementedException();
		}

		public virtual void DisableHeliconApe(string siteId)
		{
			throw new NotImplementedException();
		}

		public virtual List<HtaccessFolder> GetHeliconApeFolders(string siteId)
		{
			throw new NotImplementedException();
		}

		public virtual HtaccessFolder GetHeliconApeHttpdFolder()
		{
			throw new NotImplementedException();
		}

		public virtual HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public virtual void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder)
		{
			throw new NotImplementedException();
		}

		public virtual void UpdateHeliconApeHttpdFolder(HtaccessFolder folder)
		{
			throw new NotImplementedException();
		}

		public virtual void DeleteHeliconApeFolder(string siteId, string folderPath)
		{
			throw new NotImplementedException();
		}

		public virtual List<HtaccessUser> GetHeliconApeUsers(string siteId)
		{
			throw new NotImplementedException();
		}

		public virtual HtaccessUser GetHeliconApeUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public virtual void UpdateHeliconApeUser(string siteId, HtaccessUser user)
		{
			throw new NotImplementedException();
		}

		public virtual void DeleteHeliconApeUser(string siteId, string userName)
		{
			throw new NotImplementedException();
		}

		public virtual List<WebGroup> GetHeliconApeGroups(string siteId)
		{
			throw new NotImplementedException();
		}

		public virtual WebGroup GetHeliconApeGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

		public virtual void UpdateHeliconApeGroup(string siteId, WebGroup group)
		{
			throw new NotImplementedException();
		}

		public void GrantWebDeployPublishingAccess(string siteName, string accountName, string accountPassword)
		{
			throw new NotSupportedException();
		}

		public void RevokeWebDeployPublishingAccess(string siteName, string accountName)
		{
			throw new NotSupportedException();
		}

		public virtual void DeleteHeliconApeGroup(string siteId, string groupName)
		{
			throw new NotImplementedException();
		}

	   

	    #endregion

        #region Helicon Zoo
        public virtual WebAppVirtualDirectory[] GetZooApplications(string siteId)
        {
            return new WebAppVirtualDirectory[] { };
        }

        public virtual StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
	    {
	        //pass
            return new StringResultObject();
            
	    }

        public virtual StringResultObject SetZooConsoleEnabled(string siteId, string appName)
	    {
            return new StringResultObject();
	    }

	    public virtual StringResultObject SetZooConsoleDisabled(string siteId, string appName)
	    {
            return new StringResultObject();
	    }

        #endregion

        #region Private Helper Methods
        protected string GetVirtualDirectoryPath(string siteId, string directoryName)
        {
            string path = siteId + "/ROOT";
            if (!String.IsNullOrEmpty(directoryName))
                path += "/" + directoryName;
            return path;
        }

        private string GetVirtualDirectoryADSIPath(string siteId, string directoryName)
        {
            string path = "IIS://localhost/" + siteId + "/ROOT";
            if (!String.IsNullOrEmpty(directoryName))
                path += "/" + directoryName;
            return path;
        }
        protected string GetAppVirtualDirectoryPath(string siteId, string directoryName)
		{
			string path = siteId + "/ROOT";
			if (!String.IsNullOrEmpty(directoryName))
				path += "/" + directoryName;
			return path;
		}

		private string GetAppVirtualDirectoryADSIPath(string siteId, string directoryName)
		{
			string path = "IIS://localhost/" + siteId + "/ROOT";
			if (!String.IsNullOrEmpty(directoryName))
				path += "/" + directoryName;
			return path;
		}

		private string GetFilterPath(string siteId, string filterName)
		{
			string path = siteId + "/Filters";
			if (!String.IsNullOrEmpty(filterName))
				path += "/" + filterName;
			return path;
		}

		protected virtual string GetSiteContentPath(string siteId)
		{
			ManagementObject objVirtDir = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, "")));
			return (string)objVirtDir.Properties["Path"].Value;
		}

        private void FillVirtualDirectoryFromWmiObject(WebVirtualDirectory virtDir,
            ManagementBaseObject obj)
        {
            virtDir.EnableDirectoryBrowsing = (bool)obj.Properties["EnableDirBrowsing"].Value;
            virtDir.AnonymousUsername = (string)obj.Properties["AnonymousUserName"].Value;
            virtDir.AnonymousUserPassword = (string)obj.Properties["AnonymousUserPass"].Value;
            virtDir.EnableWindowsAuthentication = (bool)obj.Properties["AuthNTLM"].Value;
            virtDir.EnableAnonymousAccess = (bool)obj.Properties["AuthAnonymous"].Value;
            virtDir.EnableBasicAuthentication = (bool)obj.Properties["AuthBasic"].Value;
            virtDir.EnableDynamicCompression = (bool)obj.Properties["DoDynamicCompression"].Value;
            virtDir.EnableStaticCompression = (bool)obj.Properties["DoStaticCompression"].Value;
            //virtDir.DefaultDocs = (string)obj.Properties["DefaultDoc"].Value;
            virtDir.EnableParentPaths = (bool)obj.Properties["AspEnableParentPaths"].Value;
        }

        private void FillAppVirtualDirectoryFromWmiObject(WebAppVirtualDirectory virtDir,
			ManagementBaseObject obj)
		{
			virtDir.EnableDirectoryBrowsing = (bool)obj.Properties["EnableDirBrowsing"].Value;
			virtDir.AnonymousUsername = (string)obj.Properties["AnonymousUserName"].Value;
			virtDir.AnonymousUserPassword = (string)obj.Properties["AnonymousUserPass"].Value;
			virtDir.EnableWindowsAuthentication = (bool)obj.Properties["AuthNTLM"].Value;
			virtDir.EnableAnonymousAccess = (bool)obj.Properties["AuthAnonymous"].Value;
			virtDir.EnableBasicAuthentication = (bool)obj.Properties["AuthBasic"].Value;
            virtDir.EnableDynamicCompression = (bool)obj.Properties["DoDynamicCompression"].Value;
            virtDir.EnableStaticCompression = (bool)obj.Properties["DoStaticCompression"].Value;
			virtDir.DefaultDocs = (string)obj.Properties["DefaultDoc"].Value;
			virtDir.EnableParentPaths = (bool)obj.Properties["AspEnableParentPaths"].Value;
		}

        private void FillWmiObjectFromVirtualDirectory(ManagementBaseObject obj, WebVirtualDirectory virtDir,
    bool update)
        {
            if (!update)
            {
                obj.Properties["AppFriendlyName"].Value = virtDir.Name;
                obj.Properties["AccessRead"].Value = true;// virtDir.AllowReadAccess;
                obj.Properties["AccessScript"].Value = true;
            }

            obj.Properties["EnableDirBrowsing"].Value = virtDir.EnableDirectoryBrowsing;
            obj.Properties["AuthNTLM"].Value = virtDir.EnableWindowsAuthentication;
            obj.Properties["AuthAnonymous"].Value = virtDir.EnableAnonymousAccess;
            obj.Properties["AuthBasic"].Value = virtDir.EnableBasicAuthentication;
            obj.Properties["DoDynamicCompression"].Value = virtDir.EnableDynamicCompression;
            obj.Properties["DoStaticCompression"].Value = virtDir.EnableStaticCompression;

            obj.Properties["AspEnableParentPaths"].Value = virtDir.EnableParentPaths;
            
            if (!String.IsNullOrEmpty(virtDir.AnonymousUsername))
            {
                obj.Properties["AnonymousUserName"].Value = GetQualifiedAccountName(virtDir.AnonymousUsername);
                obj.Properties["AnonymousUserPass"].Value = virtDir.AnonymousUserPassword;
            }
        }
        private void FillWmiObjectFromAppVirtualDirectory(ManagementBaseObject obj, WebAppVirtualDirectory virtDir,
			bool update)
		{
			if (!update)
			{
				obj.Properties["AppFriendlyName"].Value = virtDir.Name;
				obj.Properties["AccessRead"].Value = true;// virtDir.AllowReadAccess;
				obj.Properties["AccessScript"].Value = true;
			}

			obj.Properties["EnableDirBrowsing"].Value = virtDir.EnableDirectoryBrowsing;
			obj.Properties["AuthNTLM"].Value = virtDir.EnableWindowsAuthentication;
			obj.Properties["AuthAnonymous"].Value = virtDir.EnableAnonymousAccess;
			obj.Properties["AuthBasic"].Value = virtDir.EnableBasicAuthentication;
            obj.Properties["DoDynamicCompression"].Value = virtDir.EnableDynamicCompression;
            obj.Properties["DoStaticCompression"].Value = virtDir.EnableStaticCompression;

			obj.Properties["AspEnableParentPaths"].Value = virtDir.EnableParentPaths;
			if (virtDir.DefaultDocs != null && virtDir.DefaultDocs != "")
				obj.Properties["DefaultDoc"].Value = virtDir.DefaultDocs;
			if (!String.IsNullOrEmpty(virtDir.AnonymousUsername))
			{
				obj.Properties["AnonymousUserName"].Value = GetQualifiedAccountName(virtDir.AnonymousUsername);
				obj.Properties["AnonymousUserPass"].Value = virtDir.AnonymousUserPassword;
			}
		}

        private void FillWmiObjectUNCSettingsFromVirtualDirectory(ManagementBaseObject obj, WebVirtualDirectory virtDir)
        {
            // UNC access
            if (!String.IsNullOrEmpty(virtDir.AnonymousUsername)
                && virtDir.ContentPath.StartsWith(@"\\"))
            {
                //Log.WriteError(virtDir.ContentPath, new Exception());
                ExecuteIgnorantly(delegate
                {
                    obj.Properties.Remove("UNCUserName");
                    obj.Properties.Remove("UNCPassword");

                    obj.SetPropertyValue("UNCUserName", GetQualifiedAccountName(virtDir.AnonymousUsername));
                    obj.SetPropertyValue("UNCPassword", virtDir.AnonymousUserPassword);
                });
                //
                //
                //obj.Properties["UNCUserName"].Value = GetQualifiedAccountName(virtDir.AnonymousUsername);
                //obj.Properties["UNCPassword"].Value = virtDir.AnonymousUserPassword;
            }
        }
        private void FillWmiObjectUNCSettingsFromAppVirtualDirectory(ManagementBaseObject obj, WebAppVirtualDirectory virtDir)
		{
			// UNC access
			if (!String.IsNullOrEmpty(virtDir.AnonymousUsername)
				&& virtDir.ContentPath.StartsWith(@"\\"))
			{
				//Log.WriteError(virtDir.ContentPath, new Exception());
				ExecuteIgnorantly(delegate
									{
										obj.Properties.Remove("UNCUserName");
										obj.Properties.Remove("UNCPassword");

										obj.SetPropertyValue("UNCUserName", GetQualifiedAccountName(virtDir.AnonymousUsername));
										obj.SetPropertyValue("UNCPassword", virtDir.AnonymousUserPassword);
									});
				//
				//
				//obj.Properties["UNCUserName"].Value = GetQualifiedAccountName(virtDir.AnonymousUsername);
				//obj.Properties["UNCPassword"].Value = virtDir.AnonymousUserPassword;
			}
		}

		private delegate void AnyAction();

		private static void ExecuteIgnorantly(AnyAction action)
		{
			try
			{
				action();
			}
			catch
			{
				// Ignore any exceptions thrown. This means execute ignorantly.
			}
		}

        private void FillVirtualDirectoryRestFromWmiObject(WebVirtualDirectory virtDir,
            ManagementBaseObject obj)
        {
            virtDir.ContentPath = (string)obj.Properties["Path"].Value;

            //string httpRedirect = (string)obj.Properties["HttpRedirect"].Value;
            //if (!String.IsNullOrEmpty(httpRedirect))
            //{
            //    virtDir.RedirectExactUrl = httpRedirect.Contains(REDIRECT_EXACT_URL);
            //    virtDir.RedirectDirectoryBelow = httpRedirect.Contains(REDIRECT_DIRECTORY_BELOW);
            //    virtDir.RedirectPermanent = httpRedirect.Contains(REDIRECT_PERMANENT);
            //    virtDir.HttpRedirect = httpRedirect.Split(',')[0].Trim();
            //}


            // HTTP headers
            //ManagementBaseObject[] objHttpHeaders =
            //    ((ManagementBaseObject[])obj.Properties["HttpCustomHeaders"].Value);

            //if (objHttpHeaders != null)
            //{
            //    virtDir.HttpHeaders = new HttpHeader[objHttpHeaders.Length];
            //    for (int i = 0; i < objHttpHeaders.Length; i++)
            //    {
            //        virtDir.HttpHeaders[i] = new HttpHeader();
            //        string headerVal = (string)objHttpHeaders[i].Properties["Keyname"].Value;
            //        if (String.IsNullOrEmpty(headerVal))
            //            continue;

            //        int sepIdx = headerVal.IndexOf(": ");
            //        if (sepIdx == -1)
            //            continue;

            //        virtDir.HttpHeaders[i].Key = headerVal.Substring(0, sepIdx);
            //        virtDir.HttpHeaders[i].Value = headerVal.Substring(sepIdx + 2);
            //    }
            //}

            //// HTTP errors (Skip inherited definitions)
            //virtDir.HttpErrors = GetCustomHttpErrors(obj, virtDir, true).ToArray();

            // MIME mappings
            //ManagementBaseObject[] objMimeMaps =
            //    ((ManagementBaseObject[])obj.Properties["MimeMap"].Value);

            //if (objMimeMaps != null)
            //{
            //    List<MimeMap> mimes = new List<MimeMap>();
            //    for (int i = 0; i < objMimeMaps.Length; i++)
            //    {
            //        string mimeExt = (string)objMimeMaps[i].Properties["Extension"].Value;

            //        if (String.IsNullOrEmpty(mimeExt))
            //            continue;

            //        MimeMap mime = new MimeMap();
            //        mime.Extension = mimeExt;
            //        mime.MimeType = (string)objMimeMaps[i].Properties["MimeType"].Value;
            //        mimes.Add(mime);
            //    }

            //    virtDir.MimeMaps = mimes.ToArray();
            //}

            
        }

        private void FillAppVirtualDirectoryRestFromWmiObject(WebAppVirtualDirectory virtDir,
			ManagementBaseObject obj)
		{
			virtDir.ContentPath = (string)obj.Properties["Path"].Value;

			string httpRedirect = (string)obj.Properties["HttpRedirect"].Value;
			if (!String.IsNullOrEmpty(httpRedirect))
			{
				virtDir.RedirectExactUrl = httpRedirect.Contains(REDIRECT_EXACT_URL);
				virtDir.RedirectDirectoryBelow = httpRedirect.Contains(REDIRECT_DIRECTORY_BELOW);
				virtDir.RedirectPermanent = httpRedirect.Contains(REDIRECT_PERMANENT);
				virtDir.HttpRedirect = httpRedirect.Split(',')[0].Trim();
			}


			// HTTP headers
			ManagementBaseObject[] objHttpHeaders =
				((ManagementBaseObject[])obj.Properties["HttpCustomHeaders"].Value);

			if (objHttpHeaders != null)
			{
				virtDir.HttpHeaders = new HttpHeader[objHttpHeaders.Length];
				for (int i = 0; i < objHttpHeaders.Length; i++)
				{
					virtDir.HttpHeaders[i] = new HttpHeader();
					string headerVal = (string)objHttpHeaders[i].Properties["Keyname"].Value;
					if (String.IsNullOrEmpty(headerVal))
						continue;

					int sepIdx = headerVal.IndexOf(": ");
					if (sepIdx == -1)
						continue;

					virtDir.HttpHeaders[i].Key = headerVal.Substring(0, sepIdx);
					virtDir.HttpHeaders[i].Value = headerVal.Substring(sepIdx + 2);
				}
			}

			// HTTP errors (Skip inherited definitions)
			virtDir.HttpErrors = GetCustomHttpErrors(obj, virtDir, true).ToArray();

			// MIME mappings
			ManagementBaseObject[] objMimeMaps =
				((ManagementBaseObject[])obj.Properties["MimeMap"].Value);

			if (objMimeMaps != null)
			{
				List<MimeMap> mimes = new List<MimeMap>();
				for (int i = 0; i < objMimeMaps.Length; i++)
				{
					string mimeExt = (string)objMimeMaps[i].Properties["Extension"].Value;

					if (String.IsNullOrEmpty(mimeExt))
						continue;

					MimeMap mime = new MimeMap();
					mime.Extension = mimeExt;
					mime.MimeType = (string)objMimeMaps[i].Properties["MimeType"].Value;
					mimes.Add(mime);
				}

				virtDir.MimeMaps = mimes.ToArray();
			}

			// script mappings
			ManagementBaseObject[] objScriptMaps =
				((ManagementBaseObject[])obj.Properties["ScriptMaps"].Value);

			virtDir.AspInstalled = false; // not installed
			virtDir.AspNetInstalled = ""; // none
			virtDir.PhpInstalled = ""; // none
			virtDir.PerlInstalled = false; // not installed
			virtDir.PythonInstalled = false; // not installed
			virtDir.ColdFusionInstalled = false; //not installed

			foreach (ManagementBaseObject objScriptMap in objScriptMaps)
			{
				string processor = (string)objScriptMap.Properties["ScriptProcessor"].Value;
				string extension = (string)objScriptMap.Properties["Extensions"].Value;
				if (String.Compare(AspPath, processor, true) == 0)
					virtDir.AspInstalled = true;
				else if (String.Compare(AspNet11Path, processor, true) == 0)
					virtDir.AspNetInstalled = ASPNET_11;
				else if (String.Compare(AspNet20Path, processor, true) == 0)
					virtDir.AspNetInstalled = ASPNET_20;
				else if (String.Compare(AspNet40Path, processor, true) == 0)
					virtDir.AspNetInstalled = ASPNET_40;
				else if (String.Compare(Php4Path, processor, true) == 0)
					virtDir.PhpInstalled = PHP_4;
				else if (String.Compare(Php5Path, processor, true) == 0)
					virtDir.PhpInstalled = PHP_5;
				else if (String.Compare(PerlPath, processor, true) == 0)
					virtDir.PerlInstalled = true;
				else if (String.Compare(PythonPath, processor, true) == 0)
					virtDir.PythonInstalled = true;
				else if (String.Compare(ColdFusionPath, processor, true) == 0 && String.Compare(".cfm", extension, true) == 0)
					virtDir.ColdFusionInstalled = true;
			}

			// application pool
			virtDir.ApplicationPool = obj.Properties["AppPoolId"].Value.ToString();
		}

		/// <summary>
		/// return Custom error type:
		/// 1 - if Custom Error allows 3 handler types: File, URL, Default
		/// 2 - if Custom Error allows only 2 handler types: File, Default
		/// 0 - if Custom Error code is not within list of Custom Errors in IIS 6.0
		/// </summary>
		/// <param name="code">Contains Custom error code</param>
		/// <param name="subcode">Contains Custom error subcode</param>
		/// <returns>int value</returns>

		private int GetCustomErrorType(string code, string subcode)
		{
			int type = 0;
			string combinedCode;

			if (subcode == "*" | subcode == "")
				combinedCode = code;
			else
				combinedCode = code + "." + subcode;

			for (int i = 0; i < CUSTOM_ERRORS_TYPE1.Length; i++)
			{
				if (combinedCode == CUSTOM_ERRORS_TYPE1[i])
				{
					type = 1;
					return type;
				}
			}
			for (int i = 0; i < CUSTOM_ERRORS_TYPE2.Length; i++)
			{
				if (combinedCode == CUSTOM_ERRORS_TYPE2[i])
				{
					type = 2;
					return type;
				}
			}
			return type;
		}

        private void FillWmiObjectFromVirtualDirectoryRest(ManagementBaseObject obj,
            WebVirtualDirectory virtDir)
        {
            obj.Properties["Path"].Value = virtDir.ContentPath;
            //obj.Properties["HttpRedirect"].Value = null;
            //if (!String.IsNullOrEmpty(virtDir.HttpRedirect))
            //{
            //    string httpRedirect = virtDir.HttpRedirect;
            //    if (virtDir.RedirectExactUrl)
            //        httpRedirect += ", " + REDIRECT_EXACT_URL;
            //    if (virtDir.RedirectDirectoryBelow)
            //        httpRedirect += ", " + REDIRECT_DIRECTORY_BELOW;
            //    if (virtDir.RedirectPermanent)
            //        httpRedirect += ", " + REDIRECT_PERMANENT;
            //    obj.Properties["HttpRedirect"].Value = httpRedirect;
            //}

            // HTTP headers
            //if (virtDir.HttpHeaders != null)
            //{
            //    ManagementClass clsHttpHeader = wmi.GetClass("HttpCustomHeader");
            //    ManagementObject[] httpHeaders = new ManagementObject[virtDir.HttpHeaders.Length];
            //    for (int i = 0; i < virtDir.HttpHeaders.Length; i++)
            //    {
            //        httpHeaders[i] = clsHttpHeader.CreateInstance();
            //        httpHeaders[i].Properties["Keyname"].Value =
            //            virtDir.HttpHeaders[i].Key + ": " + virtDir.HttpHeaders[i].Value;
            //        //httpHeaders[i].Properties["Value"].Value = virtDir.HttpHeaders[i].Value;
            //        httpHeaders[i].Put();
            //    }
            //    obj.Properties["HttpCustomHeaders"].Value = httpHeaders;
            //}

            // HTTP errors
            // load global settings for all of websites
            //ManagementBaseObject objSitesSetting = wmi.GetObject(String.Format("IISWebServiceSetting='{0}'", IIS_SERVICE_ID));
            //ManagementClass clsHttpError = wmi.GetClass("HttpError");
            //// get all of custom http errors including inherited ones
            //List<HttpError> httpErrors = GetCustomHttpErrors(objSitesSetting, virtDir, false);
            //List<ManagementObject> objHttpErrors = new List<ManagementObject>();
            //// merge both inherited and customized records
            //if (virtDir.HttpErrors != null)
            //{
            //    foreach (HttpError errorA in virtDir.HttpErrors)
            //    {
            //        // skip empty entries
            //        if (String.IsNullOrEmpty(errorA.ErrorContent))
            //        {
            //            continue;
            //        }


            //        // if error is not within list of Custom Errors in IIS 6.0 - just skip it
            //        if (GetCustomErrorType(errorA.ErrorCode, errorA.ErrorSubcode) == 0)
            //        {
            //            continue;
            //        }
            //        else
            //        {
            //            //if error type is 2, it can't be with handler type "URL" - skip it
            //            if ((GetCustomErrorType(errorA.ErrorCode, errorA.ErrorSubcode) == 2) && (String.Compare(errorA.HandlerType, "URL", true) == 0))
            //            {
            //                continue;
            //            }
            //        }

            //        foreach (HttpError errorB in httpErrors)
            //        {
            //            if (String.Equals(errorA.ErrorCode, errorB.ErrorCode)
            //                && String.Equals(errorA.ErrorSubcode, errorB.ErrorSubcode))
            //            {
            //                httpErrors.Remove(errorB);
            //                break;
            //            }
            //        }
            //        if (String.Compare(errorA.HandlerType, "file", true) == 0)
            //        {
            //            errorA.ErrorContent = Path.Combine(virtDir.ContentPath,
            //                FileUtils.CorrectRelativePath(errorA.ErrorContent));
            //        }
            //        //
            //        httpErrors.Add(errorA);
            //    }
            //}
            //// put all of records to IIS
            //foreach (HttpError httpError in httpErrors)
            //{
            //    ManagementObject error = clsHttpError.CreateInstance();
            //    error.Properties["HttpErrorCode"].Value = httpError.ErrorCode;
            //    error.Properties["HttpErrorSubcode"].Value = httpError.ErrorSubcode;
            //    error.Properties["HandlerType"].Value = httpError.HandlerType;
            //    error.Properties["HandlerLocation"].Value = httpError.ErrorContent;
            //    error.Put();
            //    //
            //    objHttpErrors.Add(error);
            //}
            ////
            //obj.Properties["HttpErrors"].Value = objHttpErrors.ToArray();


            // MIME mappings
            //if (virtDir.MimeMaps != null)
            //{
            //    ManagementClass clsMimeMap = wmi.GetClass("MimeMap");
            //    ManagementObject[] mimeMaps = new ManagementObject[virtDir.MimeMaps.Length];
            //    for (int i = 0; i < virtDir.MimeMaps.Length; i++)
            //    {
            //        mimeMaps[i] = clsMimeMap.CreateInstance();
            //        mimeMaps[i].Properties["Extension"].Value = virtDir.MimeMaps[i].Extension;
            //        mimeMaps[i].Properties["MimeType"].Value = virtDir.MimeMaps[i].MimeType;
            //        mimeMaps[i].Put();
            //    }
            //    obj.Properties["MimeMap"].Value = mimeMaps;
            //}

            

            
        }

        private void FillWmiObjectFromAppVirtualDirectoryRest(ManagementBaseObject obj,
			WebAppVirtualDirectory virtDir)
		{
			obj.Properties["Path"].Value = virtDir.ContentPath;
			obj.Properties["HttpRedirect"].Value = null;
			if (!String.IsNullOrEmpty(virtDir.HttpRedirect))
			{
				string httpRedirect = virtDir.HttpRedirect;
				if (virtDir.RedirectExactUrl)
					httpRedirect += ", " + REDIRECT_EXACT_URL;
				if (virtDir.RedirectDirectoryBelow)
					httpRedirect += ", " + REDIRECT_DIRECTORY_BELOW;
				if (virtDir.RedirectPermanent)
					httpRedirect += ", " + REDIRECT_PERMANENT;
				obj.Properties["HttpRedirect"].Value = httpRedirect;
			}

			// HTTP headers
			if (virtDir.HttpHeaders != null)
			{
				ManagementClass clsHttpHeader = wmi.GetClass("HttpCustomHeader");
				ManagementObject[] httpHeaders = new ManagementObject[virtDir.HttpHeaders.Length];
				for (int i = 0; i < virtDir.HttpHeaders.Length; i++)
				{
					httpHeaders[i] = clsHttpHeader.CreateInstance();
					httpHeaders[i].Properties["Keyname"].Value =
						virtDir.HttpHeaders[i].Key + ": " + virtDir.HttpHeaders[i].Value;
					//httpHeaders[i].Properties["Value"].Value = virtDir.HttpHeaders[i].Value;
					httpHeaders[i].Put();
				}
				obj.Properties["HttpCustomHeaders"].Value = httpHeaders;
			}

			// HTTP errors
			// load global settings for all of websites
			ManagementBaseObject objSitesSetting = wmi.GetObject(String.Format("IISWebServiceSetting='{0}'", IIS_SERVICE_ID));
			ManagementClass clsHttpError = wmi.GetClass("HttpError");
			// get all of custom http errors including inherited ones
			List<HttpError> httpErrors = GetCustomHttpErrors(objSitesSetting, virtDir, false);
			List<ManagementObject> objHttpErrors = new List<ManagementObject>();
			// merge both inherited and customized records
			if (virtDir.HttpErrors != null)
			{
				foreach (HttpError errorA in virtDir.HttpErrors)
				{
					// skip empty entries
					if (String.IsNullOrEmpty(errorA.ErrorContent))
					{
						continue;
					}


					// if error is not within list of Custom Errors in IIS 6.0 - just skip it
					if (GetCustomErrorType(errorA.ErrorCode, errorA.ErrorSubcode) == 0)
					{
						continue;
					}
					else
					{
						//if error type is 2, it can't be with handler type "URL" - skip it
						if ((GetCustomErrorType(errorA.ErrorCode, errorA.ErrorSubcode) == 2) && (String.Compare(errorA.HandlerType, "URL", true) == 0))
						{
							continue;
						}
					}

					foreach (HttpError errorB in httpErrors)
					{
						if (String.Equals(errorA.ErrorCode, errorB.ErrorCode)
							&& String.Equals(errorA.ErrorSubcode, errorB.ErrorSubcode))
						{
							httpErrors.Remove(errorB);
							break;
						}
					}
					if (String.Compare(errorA.HandlerType, "file", true) == 0)
					{
						errorA.ErrorContent = Path.Combine(virtDir.ContentPath,
							FileUtils.CorrectRelativePath(errorA.ErrorContent));
					}
					//
					httpErrors.Add(errorA);
				}
			}
			// put all of records to IIS
			foreach (HttpError httpError in httpErrors)
			{
				ManagementObject error = clsHttpError.CreateInstance();
				error.Properties["HttpErrorCode"].Value = httpError.ErrorCode;
				error.Properties["HttpErrorSubcode"].Value = httpError.ErrorSubcode;
				error.Properties["HandlerType"].Value = httpError.HandlerType;
				error.Properties["HandlerLocation"].Value = httpError.ErrorContent;
				error.Put();
				//
				objHttpErrors.Add(error);
			}
			//
			obj.Properties["HttpErrors"].Value = objHttpErrors.ToArray();


			// MIME mappings
			if (virtDir.MimeMaps != null)
			{
				ManagementClass clsMimeMap = wmi.GetClass("MimeMap");
				ManagementObject[] mimeMaps = new ManagementObject[virtDir.MimeMaps.Length];
				for (int i = 0; i < virtDir.MimeMaps.Length; i++)
				{
					mimeMaps[i] = clsMimeMap.CreateInstance();
					mimeMaps[i].Properties["Extension"].Value = virtDir.MimeMaps[i].Extension;
					mimeMaps[i].Properties["MimeType"].Value = virtDir.MimeMaps[i].MimeType;
					mimeMaps[i].Put();
				}
				obj.Properties["MimeMap"].Value = mimeMaps;
			}

			// delete all well-known script maps
			List<ManagementBaseObject> scriptMaps = new List<ManagementBaseObject>();
			ManagementBaseObject[] objScriptMaps = ((ManagementBaseObject[])obj.Properties["ScriptMaps"].Value);

			List<string> allExtensions = new List<string>();
			AddExtensions(allExtensions, ASP_EXTENSIONS);
			AddExtensions(allExtensions, ASPNET_11_EXTENSIONS);
			AddExtensions(allExtensions, ASPNET_20_EXTENSIONS);
			AddExtensions(allExtensions, ASPNET_40_EXTENSIONS);
			AddExtensions(allExtensions, PHP_EXTENSIONS);
			AddExtensions(allExtensions, PERL_EXTENSIONS);
			AddExtensions(allExtensions, PYTHON_EXTENSIONS);
			AddExtensions(allExtensions, COLDFUSION_EXTENSIONS);

			foreach (ManagementBaseObject objScriptMap in objScriptMaps)
			{
				if (allExtensions.Contains(objScriptMap.Properties["Extensions"].Value.ToString().ToLower()))
					continue;

				scriptMaps.Add(objScriptMap);
			}

			// add required script maps
			if (virtDir.AspInstalled)
				AddScriptMaps(scriptMaps, ASP_EXTENSIONS, AspPath);
			if (virtDir.AspNetInstalled == ASPNET_11)
				AddScriptMaps(scriptMaps, ASPNET_11_EXTENSIONS, AspNet11Path);
			if (virtDir.AspNetInstalled == ASPNET_20)
				AddScriptMaps(scriptMaps, ASPNET_20_EXTENSIONS, AspNet20Path);
			if (virtDir.AspNetInstalled == ASPNET_40)
				AddScriptMaps(scriptMaps, ASPNET_40_EXTENSIONS, AspNet40Path);
			if (virtDir.PhpInstalled == PHP_4)
				AddScriptMaps(scriptMaps, PHP_EXTENSIONS, Php4Path);
			if (virtDir.PhpInstalled == PHP_5)
				AddScriptMaps(scriptMaps, PHP_EXTENSIONS, Php5Path);
			if (virtDir.PerlInstalled)
				AddScriptMaps(scriptMaps, PERL_EXTENSIONS, PerlPath);
			if (virtDir.PythonInstalled)
				AddScriptMaps(scriptMaps, PYTHON_EXTENSIONS, PythonPath);
			if (virtDir.ColdFusionInstalled)
				AddScriptMaps(scriptMaps, COLDFUSION_EXTENSIONS, ColdFusionPath);

			// set script maps
			obj.Properties["ScriptMaps"].Value = scriptMaps.ToArray();

			// app pool
			obj.Properties["AppPoolId"].Value = virtDir.ApplicationPool;
		}

		protected void AddExtensions(List<string> allExtensions, string[] extensions)
		{
			foreach (string extension in extensions)
			{
				string ext = extension.Split(',')[0].ToLower();
				if (!allExtensions.Contains(ext))
					allExtensions.Add(ext);
			}
		}

		private void AddScriptMaps(List<ManagementBaseObject> maps, string[] extensions, string processor)
		{
			if (String.IsNullOrEmpty(processor))
				return;

			foreach (string extension in extensions)
			{
				ManagementClass clsScriptMap = wmi.GetClass("ScriptMap");
				ManagementObject objScriptMap = clsScriptMap.CreateInstance();

				string[] extParts = extension.Split(',');
				objScriptMap.Properties["Extensions"].Value = extParts[0];
				objScriptMap.Properties["Flags"].Value = Int32.Parse(extParts[1]);
				objScriptMap.Properties["IncludedVerbs"].Value = "GET,HEAD,POST,DEBUG";
				objScriptMap.Properties["ScriptProcessor"].Value = processor;
				objScriptMap.Put();

				maps.Add(objScriptMap);
			}
		}

		private void FillWebSiteFromWmiObject(WebSite site,
			ManagementBaseObject obj)
		{
			site.SiteId = (string)obj.Properties["Name"].Value;
			site.Name = (string)obj.Properties["ServerComment"].Value;
			site.LogsPath = (string)obj.Properties["LogFileDirectory"].Value;

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
		}

		protected bool CheckWriteAccessEnabled(string path, string anonAccount)
		{
			if (!Directory.Exists(path))
				return false;

			return SecurityUtils.CheckWriteAccessEnabled(path, anonAccount, ServerSettings, UsersOU, GroupsOU);
		}

        protected void SetWebFolderPermissionsNonApp(string path, string anonAccount, bool enableWriteAccess)
        {
            if (String.IsNullOrEmpty(path))
                return;

            if (!FileUtils.DirectoryExists(path))
                FileUtils.CreateDirectory(path);

            NTFSPermission permissions = enableWriteAccess ? NTFSPermission.Modify : NTFSPermission.Read;

            SecurityUtils.GrantNtfsPermissions(path, anonAccount, permissions, true, true,
                ServerSettings, UsersOU, GroupsOU);

                SecurityUtils.GrantNtfsPermissionsBySid(path, SystemSID.NETWORK_SERVICE, permissions, true, true);
        }

        protected void SetWebFolderPermissions(string path, string anonAccount,
			bool enableWriteAccess, bool dedicatedPool)
		{
			if (String.IsNullOrEmpty(path))
				return;

			if (!FileUtils.DirectoryExists(path))
				FileUtils.CreateDirectory(path);

			NTFSPermission permissions = enableWriteAccess ? NTFSPermission.Modify : NTFSPermission.Read;

			SecurityUtils.GrantNtfsPermissions(path, anonAccount, permissions, true, true,
				ServerSettings, UsersOU, GroupsOU);

			if (dedicatedPool)
				SecurityUtils.RemoveNtfsPermissionsBySid(path, SystemSID.NETWORK_SERVICE);
			else
				SecurityUtils.GrantNtfsPermissionsBySid(path, SystemSID.NETWORK_SERVICE, permissions, true, true);
		}

		protected void RemoveWebFolderPermissions(string path, string anonymousUsername)
		{
			if (String.IsNullOrEmpty(path))
				return;

			if (!FileUtils.DirectoryExists(path))
				return;

			// anonymous account
			SecurityUtils.RemoveNtfsPermissions(path, anonymousUsername,
				ServerSettings, UsersOU, GroupsOU);

			// NETWORK SERVICE
			SecurityUtils.RemoveNtfsPermissionsBySid(path, SystemSID.NETWORK_SERVICE);
		}

		private void DeleteNonexistentUsersAndGroups(string rootPath)
		{
			// nothing to do for now
		}

		private List<string> ReadFile(string path)
		{
			List<string> lines = new List<string>();

			if (!File.Exists(path))
				return lines;

			StreamReader reader = new StreamReader(path);
			string line = null;
			while ((line = reader.ReadLine()) != null)
				lines.Add(line.Trim());
			reader.Close();
			return lines;
		}

		private void WriteFile(string path, List<string> lines)
		{
			// check if folder exists
			string folder = Path.GetDirectoryName(path);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			// write file
			StreamWriter writer = new StreamWriter(path);
			foreach (string line in lines)
				writer.WriteLine(line);
			writer.Close();
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

		protected string GetNonQualifiedAccountName(string accountName)
		{
			int idx = accountName.LastIndexOf("\\");
			return (idx != -1) ? accountName.Substring(idx + 1) : accountName;
		}

		protected List<HttpError> GetCustomHttpErrors(ManagementBaseObject obj, WebAppVirtualDirectory virtDir, bool skipInherited)
		{
			List<HttpError> httpErrors = new List<HttpError>();
			// load inherited definitions
			ManagementBaseObject objSitesSetting = wmi.GetObject(String.Format("IISWebServiceSetting='{0}'", IIS_SERVICE_ID));
			//
			ManagementBaseObject[] objHttpErrorsA =
				((ManagementBaseObject[])obj.Properties["HttpErrors"].Value);
			ManagementBaseObject[] objHttpErrorsB =
				((ManagementBaseObject[])objSitesSetting.Properties["HttpErrors"].Value);

			if (objHttpErrorsA != null)
			{
				foreach (ManagementBaseObject objErrorA in objHttpErrorsA)
				{
					//
					string errorCodeA = (string)objErrorA.Properties["HttpErrorCode"].Value;
					// Skip empty entries
					if (String.IsNullOrEmpty(errorCodeA))
						continue;
					//
					string errorSubcodeA = (string)objErrorA.Properties["HttpErrorSubcode"].Value;
					string handlerA = (string)objErrorA.Properties["HandlerType"].Value;
					string contentA = (string)objErrorA.Properties["HandlerLocation"].Value;

					// Create customized ones
					HttpError error = new HttpError();
					error.ErrorCode = errorCodeA;
					error.ErrorSubcode = errorSubcodeA;
					error.HandlerType = handlerA;
					error.ErrorContent = contentA;
					// Custom errors with file handler are allowed
					// to use hosting space relative path only.
					if (String.Compare(error.HandlerType, "file", true) == 0)
					{
						if (skipInherited)
						{
							//
							bool inherited = false;
							// Loop thru iherited definitions
							foreach (ManagementBaseObject objErrorB in objHttpErrorsB)
							{
								//
								string errorCodeB = (string)objErrorB.Properties["HttpErrorCode"].Value;
								string errorSubcodeB = (string)objErrorB.Properties["HttpErrorSubcode"].Value;
								string handlerB = (string)objErrorB.Properties["HandlerType"].Value;
								string contentB = (string)objErrorB.Properties["HandlerLocation"].Value;
								// compare
								if (String.Equals(errorCodeA, errorCodeB)
									&& String.Equals(errorSubcodeA, errorSubcodeB)
									&& String.Equals(handlerA, handlerB)
									&& String.Equals(contentA, contentB))
								{
									inherited = true;
									break;
								}
							}
							// Skip inherited records
							if (inherited)
								continue;
						}
						//
						if (error.ErrorContent.StartsWith(virtDir.ContentPath, StringComparison.InvariantCultureIgnoreCase))
						{
							error.ErrorContent = error.ErrorContent.Substring(virtDir.ContentPath.Length);
						}
					}
					//
					httpErrors.Add(error);
				}
			}
			//
			return httpErrors;
		}

		#endregion

		#region HostingServiceProvider methods
		public override string[] Install()
		{
			List<string> messages = new List<string>();

			try
			{
				SecurityUtils.EnsureOrganizationalUnitsExist(ServerSettings, UsersOU, GroupsOU);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				messages.Add(String.Format("Could not check/create Organizational Units: {0}", ex.Message));
				return messages.ToArray();
			}

			// create web group name
			if (String.IsNullOrEmpty(WebGroupName))
			{
				messages.Add("Web Group can not be blank");
			}
			else
			{
				try
				{
					// create group
					if (!SecurityUtils.GroupExists(WebGroupName, ServerSettings, GroupsOU))
					{
						SystemGroup group = new SystemGroup();
						group.Name = WebGroupName;
						group.Members = new string[] { };
						group.Description = "SolidCP System Group";

						SecurityUtils.CreateGroup(group, ServerSettings, UsersOU, GroupsOU);
					}
				}
				catch (Exception ex)
				{
					Log.WriteError(ex);
					messages.Add(String.Format("There was an error while adding '{0}' group: {1}",
						WebGroupName, ex.Message));
				}
			}

			#region Create ASP.NET application pools
			try
			{
				if (!ApplicationPoolExists(Asp11Pool))
					CreateApplicationPool(Asp11Pool, "", "");
			}
			catch (Exception ex)
			{
				messages.Add(String.Format("There was an error while creating '{0}' pool: {1}",
					Asp11Pool, ex.Message));
			}

			try
			{
				if (!ApplicationPoolExists(Asp20Pool))
					CreateApplicationPool(Asp20Pool, "", "");
			}
			catch (Exception ex)
			{
				messages.Add(String.Format("There was an error while creating '{0}' pool: {1}",
					Asp20Pool, ex.Message));
			}

			try
			{
				if (!ApplicationPoolExists(Asp40Pool))
					CreateApplicationPool(Asp40Pool, "", "");
			}
			catch (Exception ex)
			{
				messages.Add(String.Format("There was an error while creating '{0}' pool: {1}",
					Asp40Pool, ex.Message));
			}
			#endregion

			// check script processors
			string aspPath = FileUtils.GetExecutablePathWithoutParameters(AspPath);
			string aspNet11Path = FileUtils.GetExecutablePathWithoutParameters(AspNet11Path);
			string aspNet20Path = FileUtils.GetExecutablePathWithoutParameters(AspNet20Path);
			string aspNet40Path = FileUtils.GetExecutablePathWithoutParameters(AspNet40Path);
			string php4Path = FileUtils.GetExecutablePathWithoutParameters(Php4Path);
			string php5Path = FileUtils.GetExecutablePathWithoutParameters(Php5Path);
			string perlPath = FileUtils.GetExecutablePathWithoutParameters(PerlPath);
			string pythonPath = FileUtils.GetExecutablePathWithoutParameters(PythonPath);
			string securedFoldersFilterPath = FileUtils.GetExecutablePathWithoutParameters(SecuredFoldersFilterPath);
			string coldfusionPath = FileUtils.GetExecutablePathWithoutParameters(ColdFusionPath);
			string cfscriptsdirPath = FileUtils.GetExecutablePathWithoutParameters(CFScriptsDirectoryPath);
			string cfflashdirectoryPath = FileUtils.GetExecutablePathWithoutParameters(CFFlashRemotingDirPath);

			if (!String.IsNullOrEmpty(aspPath) && !FileUtils.FileExists(aspPath))
				messages.Add(String.Format("\"{0}\" file could not be found", aspPath));

			if (!String.IsNullOrEmpty(aspNet11Path) && !FileUtils.FileExists(aspNet11Path))
				messages.Add(String.Format("\"{0}\" file could not be found", aspNet11Path));

			if (!String.IsNullOrEmpty(aspNet20Path) && !FileUtils.FileExists(aspNet20Path))
				messages.Add(String.Format("\"{0}\" file could not be found", aspNet20Path));

			if (!String.IsNullOrEmpty(aspNet40Path) && !FileUtils.FileExists(aspNet40Path))
				messages.Add(String.Format("\"{0}\" file could not be found", aspNet40Path));

			if (!String.IsNullOrEmpty(php4Path) && !FileUtils.FileExists(php4Path))
				messages.Add(String.Format("\"{0}\" file could not be found", php4Path));

			if (!String.IsNullOrEmpty(php5Path) && !FileUtils.FileExists(php5Path))
				messages.Add(String.Format("\"{0}\" file could not be found", php5Path));

			if (!String.IsNullOrEmpty(perlPath) && !FileUtils.FileExists(perlPath))
				messages.Add(String.Format("\"{0}\" file could not be found", perlPath));

			if (!String.IsNullOrEmpty(pythonPath) && !FileUtils.FileExists(pythonPath))
				messages.Add(String.Format("\"{0}\" file could not be found", pythonPath));

			if (!String.IsNullOrEmpty(securedFoldersFilterPath) && !FileUtils.FileExists(securedFoldersFilterPath))
				messages.Add(String.Format("\"{0}\" file could not be found", securedFoldersFilterPath));

			if (!String.IsNullOrEmpty(cfscriptsdirPath) && !FileUtils.DirectoryExists(cfscriptsdirPath))
				messages.Add(String.Format("\"{0}\" directory could not be found", cfscriptsdirPath));

			if (!String.IsNullOrEmpty(cfflashdirectoryPath) && !FileUtils.DirectoryExists(cfflashdirectoryPath))
				messages.Add(String.Format("\"{0}\" directory could not be found", cfflashdirectoryPath));


			if (!String.IsNullOrEmpty(coldfusionPath) && !FileUtils.FileExists(coldfusionPath))
				messages.Add(String.Format("\"{0}\" file could not be found", coldfusionPath));

			return messages.ToArray();
		}

		public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is WebSite)
				{
					try
					{
						// start/stop web site
						ChangeSiteState(((WebSite)item).SiteId,
							(enabled ? ServerState.Started : ServerState.Stopped));
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
				if (item is WebSite)
				{
					try
					{
						// delete web site
						DeleteSite(((WebSite)item).SiteId);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
					}
				}
				else if (item is SharedSSLFolder)
				{
					try
					{
						// delete shared SSL folder
						int idx = item.Name.LastIndexOf("/");
						string domainName = item.Name.Substring(0, idx);
						string vdirName = item.Name.Substring(idx + 1);

						string siteId = GetSiteId(domainName);
						if (siteId != null)
						{
							// delete directory
							DeleteAppVirtualDirectory(siteId, vdirName);
                            DeleteVirtualDirectory(siteId, vdirName);
                        }
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
					}
				}
			}
		}

		public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
		{
			List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();

			// update items with diskspace
			foreach (ServiceProviderItem item in items)
			{
				if (item is WebSite)
				{
					try
					{
						Log.WriteStart(String.Format("Calculating '{0}' site logs size", item.Name));

						WebSite site = GetSite(((WebSite)item).SiteId);
						//string sitePath = site.ContentPath;
						string siteId = ((WebSite)item).SiteId.Replace("/", "");
						string logsPath = Path.Combine(site.LogsPath, siteId);

						// calculate disk space
						ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
						diskspace.ItemId = item.Id;
						diskspace.DiskSpace = -1 * FileUtils.CalculateFolderSize(logsPath);
						itemsDiskspace.Add(diskspace);

						Log.WriteEnd(String.Format("Calculating '{0}' site logs size", item.Name));
					}
					catch (Exception ex)
					{
						Log.WriteError(ex);
					}
				}
			}
			return itemsDiskspace.ToArray();
		}

		public override ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since)
		{
			ServiceProviderItemBandwidth[] itemsBandwidth = new ServiceProviderItemBandwidth[items.Length];

			// update items with diskspace
			for (int i = 0; i < items.Length; i++)
			{
				ServiceProviderItem item = items[i];

				// create new bandwidth object
				itemsBandwidth[i] = new ServiceProviderItemBandwidth();
				itemsBandwidth[i].ItemId = item.Id;
				itemsBandwidth[i].Days = new DailyStatistics[0];

				if (item is WebSite)
				{
					try
					{
						WebSite site = GetSite(((WebSite)item).SiteId);
						string siteId = ((WebSite)item).SiteId.Replace("/", "");
						string logsPath = Path.Combine(site.LogsPath, siteId);

						if (!Directory.Exists(logsPath))
							continue;

						// create parser object
						// and update statistics
						LogParser parser = new LogParser("Web", siteId, logsPath, "s-sitename");
						parser.ParseLogs();

						// get daily statistics
						itemsBandwidth[i].Days = parser.GetDailyStatistics(since, new string[] { siteId });
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

        #region Directory Browsing

        public virtual bool GetDirectoryBrowseEnabled(string siteId)
        {
            ManagementObject objVirtDir = wmi.GetObject(String.Format("IIsWebVirtualDirSetting='{0}'", GetAppVirtualDirectoryPath(siteId, "")));
            return objVirtDir.Properties["EnableDirBrowsing"].Value != null ? (bool)objVirtDir.Properties["EnableDirBrowsing"].Value : false;
        }

        public virtual void SetDirectoryBrowseEnabled(string siteId, bool enabled)
        {
            ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

            WebSite site = GetSite(siteId);
            site.EnableDirectoryBrowsing = enabled;

            FillWmiObjectFromAppVirtualDirectory(objSite, site, false);
            objSite.Put();
        }

        #endregion

        public virtual bool IsIISInstalled()
		{
			int value = 0;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\InetStp");
			if (rk != null)
			{
				value = (int)rk.GetValue("MajorVersion", 0);
				rk.Close();
			}

			return value == 6;
		}

		public override bool IsInstalled()
		{
			return IsIISInstalled();
		}

		
        #region Microsoft Web Application Gallery

        private const string MS_DEPLOY_ASSEMBLY_NAME = "Microsoft.Web.Deployment";
        private const string WPI_INSTANCE_VIEWER = "viewer";
	    private const string WPI_INSTANCE_INSTALLER = "installer";

        virtual public bool CheckLoadUserProfile()
        {
            //throw new NotImplementedException("LoadUserProfile option valid only on IIS7 or higer");
            return false;
        }

        virtual public void EnableLoadUserProfile()
        {
            throw new NotImplementedException("LoadUserProfile option valid only on IIS7 or higer");
        }

        

	    public void InitFeeds(int UserId, string[] feeds)
        {
            //need to call InitFeeds() before any operation with WPIApplicationGallery()
            WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_VIEWER);
            module.InitFeeds(UserId, feeds);
        }

        public void SetResourceLanguage(int UserId, string resourceLanguage)
        {
            WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_VIEWER);
            module.SetResourceLanguage(UserId, resourceLanguage);
        }

	    public bool IsMsDeployInstalled()
        {
            // TO-DO: Implement Web Deploy detection (x64/x86)
            var isInstalled = false;
            //
            try
            {
                var msdeployRegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\IIS Extensions\MSDeploy\3");
                //
                var keyValue = msdeployRegKey.GetValue("Install");
                // We have found the required key in the registry hive
                if (keyValue != null && keyValue.Equals(1))
                {
                    isInstalled = true;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Could not retrieve Web Deploy key from the registry", ex);
            }
            //
            return isInstalled;


        }

	    public GalleryLanguagesResult GetGalleryLanguages(int UserId)
        {
            GalleryLanguagesResult result = new GalleryLanguagesResult();
            WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_VIEWER);
            try
            {
                result.Value = module.GetLanguages(UserId);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.AddError(GalleryErrors.GetLanguagesError, ex);
            }

            return result;
        }

        public GalleryCategoriesResult GetGalleryCategories(int UserId)
        {
            GalleryCategoriesResult result = new GalleryCategoriesResult();

            //try
            //{
            WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_VIEWER);
            //
            result.Value = module.GetCategories(UserId);
            result.IsSuccess = true;
            //}
            //catch (Exception ex)
            //{
            //    result.IsSuccess = false;
            //    result.AddError(GalleryErrors.ProcessingFeedXMLError, ex);
            //}
            ////
            return result;
        }

        public GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
        {
            GalleryApplicationsResult result = new GalleryApplicationsResult();

            try
            {
                WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_VIEWER);
                //
                result.Value = module.GetApplications(UserId, categoryId);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.AddError(GalleryErrors.ProcessingFeedXMLError, ex);
            }
            //
            return result;
        }

        public GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            GalleryApplicationsResult result = new GalleryApplicationsResult();

            try
            {
                WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_VIEWER);

                result.Value = module.GetGalleryApplicationsFiltered(UserId, pattern);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.AddError(ex.Message, ex);
            }


            return result;
        }


        public GalleryApplicationResult GetGalleryApplication(int UserId, string id)
        {
            GalleryApplicationResult result = new GalleryApplicationResult();
            //
            try
            {
                WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_VIEWER);
                //
                result.Value = module.GetApplicationByProductId(UserId, id);
                result.IsSuccess = true;
                result.ErrorCodes.AddRange(module.GetMissingDependenciesForApplicationById(UserId, id));
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.AddError(GalleryErrors.ProcessingFeedXMLError, ex);
            }
            //
            return result;
        }

        public GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
        {
            return GetGalleryApplicationStatus(UserId, id);
        }

        public GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
        {
            try
            {
                WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_INSTALLER);

                return module.DownloadAppAndGetStatus(UserId, id);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.WriteError(ex);
                return GalleryWebAppStatus.UnauthorizedAccessException;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                return GalleryWebAppStatus.Failed;
            }
        }

        public DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
        {
            DeploymentParametersResult result = new DeploymentParametersResult();

            try
            {
                WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_INSTALLER);
                //
                result.Value = module.GetApplicationParameters(UserId, id);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.AddError(GalleryErrors.ProcessingPackageError, ex);
            }
            //
            return result;
        }


        public StringResultObject InstallGalleryApplication(int UserId, string webAppId, List<DeploymentParameter> updatedValues, string languageId)
        {
            StringResultObject result = new StringResultObject();

            try
            {
                WPIApplicationGallery module = new WPIApplicationGallery(WPI_INSTANCE_INSTALLER);
                //
                module.InstallApplication(UserId, webAppId, updatedValues, languageId, ref result);

                if (result.IsSuccess)
                {
                    module.DeleteWpiHelper(UserId);
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.AddError(GalleryErrors.PackageInstallationError, ex);
            }
            //
            return result;
        }

		#endregion

		#region Remote Management Access
		public virtual void GrantWebManagementAccess(string siteName, string accountName, string accountPassword)
		{
			throw new NotSupportedException();
		}

        public virtual void RevokeWebManagementAccess(string siteName, string accountName)
		{
			throw new NotSupportedException();
		}

        public virtual void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
		{
			throw new NotSupportedException();
		}

        public virtual bool CheckWebManagementAccountExists(string accountName)
		{
			throw new NotSupportedException();
		}

        public virtual ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
		{
			throw new NotSupportedException();
		}
        #endregion

        #region SSL

        public virtual SSLCertificate installCertificate(SSLCertificate certificate, WebSite website)
        {
            throw new NotSupportedException();
        }

        public virtual SSLCertificate getCertificate(WebSite site)
        {
            throw new NotSupportedException();
        }

        public virtual SSLCertificate installPFX(byte[] certificate, string password, WebSite website)
        {
            throw new NotSupportedException();
        }

        public virtual byte[] exportCertificate(string serialNumber, string password)
        {
            throw new NotSupportedException();
        }

        public virtual SSLCertificate generateCSR(SSLCertificate certificate)
        {
            throw new NotSupportedException();
        }
        public virtual SSLCertificate generateRenewalCSR(SSLCertificate certificate)
        {
            throw new NotSupportedException();
        }
        public virtual List<SSLCertificate> getServerCertificates()
        {
            throw new NotSupportedException();
        }
        public virtual ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
        {
            throw new NotSupportedException();
        }
        public virtual SSLCertificate ImportCertificate(WebSite website)
        {
            throw new NotSupportedException();
        }
        public virtual bool CheckCertificate(WebSite webSite)
        {
            throw new NotSupportedException();
        }
        #endregion

    }
}
