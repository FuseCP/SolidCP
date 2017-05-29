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
using System.Management;
using Microsoft.Win32;
using SolidCP.Setup.Web;
using Microsoft.Web.Management;
using Microsoft.Web.Administration;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace SolidCP.Setup
{
	/// <summary>
	/// Web utils class.
	/// </summary>
	public sealed class WebUtils
	{
		/// <summary>IIS_SERVICE_ID</summary>
		public const string IIS_SERVICE_ID = "W3SVC";

		

        public const string AspNet11 = "v1.1.4322";
        public const string AspNet20 = "v2.0.50727";
		public const string AspNet40 = "v4.0.30319";

        public const string SolidCP_POOL = "SolidCP Pool";
		public const string SolidCP_ADMIN_POOL = "SolidCP Admin Pool";

        internal static string[] aspNet11Maps = new string[] { ".asax", ".ascx", ".ashx",
        ".asmx", ".aspx", ".axd", ".config", ".cs", ".csproj", ".licx", ".rem", ".resources", ".resx",
        ".soap", ".vb", ".vbproj", ".vsdisco", ".webinfo"};

        internal static string[] aspNet20Maps = new string[] { ".ad", ".adprototype", ".asax", ".ascx", ".ashx",
        ".asmx", ".aspx", ".axd", ".browser", ".cd", ".compiled", ".config", ".cs", ".csproj", ".dd",
        ".exclude", ".java", ".jsl", ".ldb", ".ldd", ".lddprototype", ".ldf", ".licx", ".master",
        ".mdb", ".mdf", ".msgx", ".refresh", ".rem", ".resources", ".resx", ".sd", ".sdm", ".sdmDocument",
        ".sitemap", ".skin", ".soap", ".svc", ".vb", ".vbproj", ".vjsproj", ".vsdisco", ".webinfo"};

		internal static string[] aspNet40Maps = new string[] { ".ad", ".adprototype", ".asax", ".ascx", ".ashx",
        ".asmx", ".aspx", ".axd", ".browser", ".cd", ".compiled", ".config", ".cs", ".csproj", ".dd",
        ".exclude", ".java", ".jsl", ".ldb", ".ldd", ".lddprototype", ".ldf", ".licx", ".master",
        ".mdb", ".mdf", ".msgx", ".refresh", ".rem", ".resources", ".resx", ".sd", ".sdm", ".sdmDocument",
        ".sitemap", ".skin", ".soap", ".svc", ".vb", ".vbproj", ".vjsproj", ".vsdisco", ".webinfo"};
		
		private static WmiHelper wmi = new WmiHelper("root\\MicrosoftIISv2");

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private WebUtils()
		{
		}

		/// <summary>
		/// Retrieves web sites.
		/// </summary>
		/// <returns>Web sites.</returns>
		internal static WebSiteItem[] GetWebSites()
		{
			List<WebSiteItem> sites = new List<WebSiteItem>();

			// get all sites
			ManagementObjectCollection objSites = wmi.ExecuteQuery("SELECT * FROM IIsWebServerSetting");
			foreach(ManagementObject objSite in objSites)
			{
				WebSiteItem site = new WebSiteItem();
				FillWebSiteFromWmiObject(site, objSite);
				sites.Add(site);
			}

			return sites.ToArray();
		}
/*
		/// <summary>
		/// Creates virtual directory.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <param name="directoryName">Directory name.</param>
		/// <param name="contentPath">Content path.</param>
        /// <param name="aspNet">ASP.NET version</param>
		internal static void CreateVirtualDirectory(string siteId, string directoryName, string contentPath,
            AspNetVersion aspNet)
		{
			// set folder permissions
			SetWebFolderPermissions(contentPath, GetSiteAnonymousUserName(siteId));

			string dirId = GetVirtualDirectoryPath(siteId, directoryName);

			// create a new virtual directory
			ManagementObject objDir = wmi.GetClass("IIsWebVirtualDir").CreateInstance();
			objDir.Properties["Name"].EventData = dirId;
			objDir.Put();
			objDir.InvokeMethod("AppCreate",new Object[] {true});

			// update directory properties
			ManagementObject objDirSetting = wmi.GetClass("IIsWebVirtualDirSetting").CreateInstance();
			objDirSetting.Properties["Name"].EventData = dirId;
			objDirSetting.Properties["AppFriendlyName"].EventData = directoryName;
			objDirSetting.Properties["Path"].EventData = contentPath;

            // save object again
            objDirSetting.Put();

            // set ASP.NET
            objDirSetting = wmi.GetObject(
                String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, directoryName)));

            SetVirtualDirectoryAspNetMappings(objDirSetting, aspNet);

            // set correct default documents
            SetVirtualDirectoryDefaultDocs(objDirSetting);

            // save object again
			objDirSetting.Put();
		}
*/
		internal static void SetWebFolderPermissions(string path, string userDomain, string userAccount)
		{
			if(!FileUtils.DirectoryExists(path))
				FileUtils.CreateDirectory(path);

			SecurityUtils.GrantNtfsPermissions(path, userDomain, userAccount, NtfsPermission.Modify, true, true);
            SecurityUtils.GrantNtfsPermissionsBySid(path, SystemSID.NETWORK_SERVICE, NtfsPermission.Modify, true, true);
		}

		/// <summary>
		/// Checks if the site exists.
		/// </summary>
		/// <param name="siteId">SiteID</param>
		/// <returns></returns>
		internal static bool SiteIdExists(string siteId)
		{
			return (wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsWebServerSetting WHERE Name='{0}'", siteId)).Count > 0);
		}

		/// <summary>
		/// Checks if the site exists.
		/// </summary>
		/// <param name="siteId">SiteID</param>
		/// <returns></returns>
		internal static bool IIS7SiteExists(string siteId)
		{
			ServerManager serverManager = new ServerManager();
			bool ret = (serverManager.Sites[siteId] != null);
			return ret;
		}

		/// <summary>
		/// Retreives site by site id.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <returns>Site object.</returns>
		internal static WebSiteItem GetSite(string siteId)
		{
			WebSiteItem site = new WebSiteItem();

			// get web server settings object
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

			FillWebSiteFromWmiObject(site, objSite);

			// get ROOT vritual directory settings object
			ManagementObject objVirtDir = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, "")));

			FillVirtualDirectoryFromWmiObject(site, objVirtDir);
			FillVirtualDirectoryRestFromWmiObject(site, objVirtDir);
			return site;
		}


		private static void FillWebSiteFromWmiObject(WebSiteItem site,
			ManagementBaseObject obj)
		{
			site.SiteId = (string)obj.Properties["Name"].Value;
			site.Name = (string)obj.Properties["ServerComment"].Value;

			// get server bindings
			ManagementBaseObject[] objBindings =
				((ManagementBaseObject[])obj.Properties["ServerBindings"].Value);

			if(objBindings != null)
			{
				site.Bindings = new ServerBinding[objBindings.Length];
				for(int i = 0; i < objBindings.Length; i++)
				{
					site.Bindings[i] = new ServerBinding(
						(string)objBindings[i].Properties["IP"].Value,
						(string)objBindings[i].Properties["Port"].Value,
						(string)objBindings[i].Properties["Hostname"].Value);

				}
			}
		}

		private static void FillVirtualDirectoryFromWmiObject(WebVirtualDirectoryItem virtDir,
			ManagementBaseObject obj)
		{
			virtDir.AllowReadAccess = (bool)obj.Properties["AccessRead"].Value;
			virtDir.AllowScriptAccess = (bool)obj.Properties["AccessScript"].Value;
			virtDir.AllowSourceAccess = (bool)obj.Properties["AccessSource"].Value;
			virtDir.AllowWriteAccess = (bool)obj.Properties["AccessWrite"].Value;
			virtDir.AllowExecuteAccess = (bool)obj.Properties["AccessExecute"].Value;
			virtDir.AllowDirectoryBrowsingAccess = (bool)obj.Properties["EnableDirBrowsing"].Value;
			virtDir.AnonymousUsername = (string)obj.Properties["AnonymousUserName"].Value;
			virtDir.AnonymousUserPassword = (string)obj.Properties["AnonymousUserPass"].Value;
			virtDir.AuthWindows = (bool)obj.Properties["AuthNTLM"].Value;
			virtDir.AuthAnonymous = (bool)obj.Properties["AuthAnonymous"].Value;
			virtDir.AuthBasic = (bool)obj.Properties["AuthBasic"].Value;
			virtDir.DefaultDocs = (string)obj.Properties["DefaultDoc"].Value;
		}

		private static void FillVirtualDirectoryRestFromWmiObject(WebVirtualDirectoryItem virtDir,
			ManagementBaseObject obj)
		{
			virtDir.ContentPath = (string)obj.Properties["Path"].Value;
			virtDir.HttpRedirect = (string)obj.Properties["HttpRedirect"].Value;
		}

		internal static string GetVirtualDirectoryPath(string siteId, string directoryName)
		{
			string path = siteId + "/ROOT";
			if(directoryName != null && directoryName != string.Empty)
				path += "/" + directoryName;
			return path;
		}

		private static string GetSiteAnonymousUserName(string siteId)
		{
			// get ROOT virtual directory settings object
			ManagementObject obj = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, string.Empty)));
			return (string)obj.Properties["AnonymousUserName"].Value;
		}

		/// <summary>
		/// Checks site bindings.
		/// </summary>
		/// <param name="ip">IP address.</param>
		/// <param name="port">TCP port.</param>
		/// <param name="host">Host header value.</param>
		/// <returns>True if site binding exist, otherwise false.</returns>
		internal static bool CheckSiteBindings(string ip, string port, string host)
		{
			// check for server bindings
			ManagementObjectCollection objSites = wmi.ExecuteQuery("SELECT * FROM IIsWebServerSetting");
			foreach(ManagementObject objSite in objSites)
			{
				// check server bindings
				ManagementBaseObject[] objProbBinings = (ManagementBaseObject[])objSite.Properties["ServerBindings"].Value;
				if(objProbBinings != null)
				{
					// check this binding against provided ones
					foreach(ManagementBaseObject objProbBinding in objProbBinings)
					{
						string siteIP = (string)objProbBinding.Properties["IP"].Value;
						string sitePort = (string)objProbBinding.Properties["Port"].Value;
						string siteHost = (string)objProbBinding.Properties["Hostname"].Value;

						if(siteIP == ip && sitePort == port && host.ToLower() == siteHost.ToLower())
							return false;
					}
				}
			}
			return true;
		}
		

		/// <summary>
		/// Creates site.
		/// </summary>
		/// <param name="site">Site object.</param>
		/// <returns>Site id.</returns>
		internal static string CreateSite(WebSiteItem site)
		{
			//CheckWebServerBindings(site.Bindings);

			// set folder permissions
			//SetWebFolderPermissions(site.ContentPath, site.AnonymousUsername);

			// create Web site
			ManagementObject objService = wmi.GetObject(String.Format("IIsWebService='{0}'", IIS_SERVICE_ID));
            	
			ManagementBaseObject methodParams = objService.GetMethodParameters("CreateNewSite");

			// create server bindings
			ManagementClass clsBinding = wmi.GetClass("ServerBinding");
			ManagementObject[] objBindings = new ManagementObject[site.Bindings.Length];
	
			for(int i = 0; i < objBindings.Length; i++)
			{
				objBindings[i] = clsBinding.CreateInstance();
				objBindings[i]["Hostname"] = site.Bindings[i].Host;
				objBindings[i]["IP"] = site.Bindings[i].IP;
				objBindings[i]["Port"] = site.Bindings[i].Port;
			}
	
			methodParams["ServerBindings"] = objBindings;
			methodParams["ServerComment"] = site.Name;
			methodParams["PathOfRootVirtualDir"] = site.ContentPath;

			ManagementBaseObject objResult = objService.InvokeMethod("CreateNewSite", methodParams, new InvokeMethodOptions());

			// get WEB settings
			string siteId = ((string)objResult["returnValue"]).Remove(0, "IIsWebServer='".Length).Replace("'","");

			// update site properties
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));
			ManagementObject objVirtDir = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, "")));

			if(site.LogFileDirectory != null && site.LogFileDirectory != "")
				objSite.Properties["LogFileDirectory"].Value = site.LogFileDirectory;

			FillWmiObjectFromVirtualDirectory(objSite, site);
			objSite.Put();

			FillWmiObjectFromVirtualDirectory(objVirtDir, site);
			FillWmiObjectFromVirtualDirectoryRest(objVirtDir, site);

            // set correct default documents
            SetVirtualDirectoryDefaultDocs(objVirtDir);

            // set ASP.NET
            SetVirtualDirectoryAspNetMappings(objVirtDir, site.InstalledDotNetFramework);

            // save object
			objVirtDir.Put();

			// start site
			ChangeSiteState(siteId, ServerState.Started);

			return siteId;

		}


		/// <summary>
		/// Creates site.
		/// </summary>
		/// <param name="site">Site object.</param>
		/// <returns>Site id.</returns>
		internal static string CreateIIS7Site(WebSiteItem site)
		{
			ServerManager serverManager = new ServerManager();
			Site webSite = serverManager.Sites[site.Name];
			if ( webSite == null )
				webSite = serverManager.Sites.Add(site.Name, site.ContentPath, 80);
			
			// cleanup all bindings
			webSite.Bindings.Clear();
			//
			foreach (ServerBinding binding in site.Bindings)
			{
				//
				webSite.Bindings.Add(binding.IP + ":" + binding.Port + ":" + binding.Host,
					Uri.UriSchemeHttp);
			}
			//
			webSite.Applications[0].ApplicationPoolName = site.ApplicationPool;
			//
			//webSite.LogFile.Directory = site.LogFileDirectory;
			//
			site.SiteId = webSite.Name;
			//
			webSite.ServerAutoStart = true;
			//authentication
			Configuration cnfg = serverManager.GetApplicationHostConfiguration();
			ConfigurationSection section = cnfg.GetSection("system.webServer/security/authentication/anonymousAuthentication", site.Name);
			section["enabled"] = site.AuthAnonymous;
			section["userName"] = string.Empty;
			section["password"] = string.Empty;


			section = cnfg.GetSection("system.webServer/security/authentication/windowsAuthentication", site.Name);
			section["enabled"] = site.AuthWindows;

			//TODO: default documents
			serverManager.CommitChanges();

			return site.SiteId;

		}

		/// <summary>
		/// Updates site
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="contentPath"></param>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		/// <param name="host"></param>
        /// <param name="aspNet">ASP.NET version</param>
		internal static void UpdateSite(string siteId, string contentPath, string ip, string port, string host,
            AspNetVersion aspNet)
		{
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));
			ManagementObject objVirtDir = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, "")));

			// check if we need to add a server binding
			string existSiteId = GetSiteIdByBinding(ip, port, host);
			if(existSiteId == null)
			{
				// binding doesn't exist
				// add the binding to the web site
				ManagementBaseObject[] objProbBindings = (ManagementBaseObject[])objSite.Properties["ServerBindings"].Value;
				ManagementClass clsBinding = wmi.GetClass("ServerBinding");
				ManagementObject[] newBindings = new ManagementObject[objProbBindings.Length+1];
				// copy existing bindings
				for(int i = 0; i < objProbBindings.Length; i++)
				{
					newBindings[i] = clsBinding.CreateInstance();
					newBindings[i]["Hostname"] = (string)objProbBindings[i].Properties["Hostname"].Value;
					newBindings[i]["IP"] = (string)objProbBindings[i].Properties["IP"].Value;
					newBindings[i]["Port"] = (string)objProbBindings[i].Properties["Port"].Value;
				}
				//create new binding
				newBindings[objProbBindings.Length] = clsBinding.CreateInstance();
				newBindings[objProbBindings.Length]["Hostname"] = host;
				newBindings[objProbBindings.Length]["IP"] = ip;
				newBindings[objProbBindings.Length]["Port"] = port;

				objSite.Properties["ServerBindings"].Value = newBindings;
			}
			objSite.Put();

            // set path
            if (!String.IsNullOrEmpty(contentPath))
            {
                // set content path
                objVirtDir.Properties["Path"].Value = contentPath;

                // set correct default documents
                SetVirtualDirectoryDefaultDocs(objVirtDir);
            }

            // set ASP.NET
            SetVirtualDirectoryAspNetMappings(objVirtDir, aspNet);

            // save object
			objVirtDir.Put();
		}

	
		/// <summary>
		/// Changes site state.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <param name="state">Server state.</param>
		internal static void ChangeSiteState(string siteId, ServerState state)
		{
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServer='{0}'", siteId));
			string methodName = "Continue";
			switch(state)
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

		private static void FillWmiObjectFromVirtualDirectory(ManagementBaseObject obj, WebVirtualDirectoryItem virtDir)
		{
			obj.Properties["AppFriendlyName"].Value = virtDir.Name;
			obj.Properties["AccessRead"].Value = virtDir.AllowReadAccess;
			obj.Properties["AccessScript"].Value = virtDir.AllowScriptAccess;
			obj.Properties["AccessSource"].Value = virtDir.AllowSourceAccess;
			obj.Properties["AccessWrite"].Value = virtDir.AllowWriteAccess;
			obj.Properties["AccessExecute"].Value = virtDir.AllowExecuteAccess;
			obj.Properties["EnableDirBrowsing"].Value = virtDir.AllowDirectoryBrowsingAccess;
			obj.Properties["AuthNTLM"].Value = virtDir.AuthWindows;
			obj.Properties["AuthAnonymous"].Value = virtDir.AuthAnonymous;
			obj.Properties["AuthBasic"].Value = virtDir.AuthBasic;
			if(virtDir.DefaultDocs != null && virtDir.DefaultDocs != string.Empty)
				obj.Properties["DefaultDoc"].Value = virtDir.DefaultDocs;
			if(virtDir.AnonymousUsername != null && virtDir.AnonymousUsername != string.Empty)
			{
				obj.Properties["AnonymousUserName"].Value = virtDir.AnonymousUsername;
				obj.Properties["AnonymousUserPass"].Value = virtDir.AnonymousUserPassword;
			}
			obj.Properties["AppPoolId"].Value = virtDir.ApplicationPool;
		}

		private static void FillWmiObjectFromVirtualDirectoryRest(ManagementBaseObject obj,
			WebVirtualDirectoryItem virtDir)
		{
			obj.Properties["Path"].Value = virtDir.ContentPath;
			obj.Properties["HttpRedirect"].Value = (virtDir.HttpRedirect == null || virtDir.HttpRedirect == "") ?
				null : virtDir.HttpRedirect;
		}

        private static void SetVirtualDirectoryDefaultDocs(ManagementBaseObject obj)
        {
            string defaultDoc = obj.Properties["DefaultDoc"].Value.ToString();
            List<string> defaultDocs = new List<string>();
            defaultDocs.AddRange(defaultDoc.Split(','));

            bool contains = false;
            foreach (string doc in defaultDocs)
            {
                if (String.Compare(doc, "default.aspx", true) == 0)
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                defaultDocs.Add("Default.aspx");
                defaultDoc = String.Join(",", defaultDocs.ToArray());
                obj.Properties["DefaultDoc"].Value = defaultDoc;
            }
        }

        private static void SetVirtualDirectoryAspNetMappings(ManagementBaseObject obj, AspNetVersion aspNet)
        {
            if (aspNet != AspNetVersion.Unknown)
            {
                // configure ASP.NET
                // remove existing mappings
                List<string> aspNetMaps = new List<string>();
                aspNetMaps.AddRange(aspNet20Maps);

                List<ManagementBaseObject> scriptMaps = new List<ManagementBaseObject>();

                ManagementBaseObject[] objScriptMaps =
                    ((ManagementBaseObject[])obj.Properties["ScriptMaps"].Value);

                // get/filter existing maps
                foreach (ManagementBaseObject objScriptMap in objScriptMaps)
                {
                    string ext = (string)objScriptMap.Properties["Extensions"].Value;
                    if (!aspNetMaps.Contains(ext))
                        scriptMaps.Add(objScriptMap);
                }

                // add script maps

                string[] aspNetExtensions = aspNet11Maps;
                string aspNetVersionName = AspNet11;
                if (aspNet == AspNetVersion.AspNet20)
                {
                    aspNetExtensions = aspNet20Maps;
                    aspNetVersionName = AspNet20;
                }

                // add required script maps
                RegistryKey netFramework = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NetFramework", false);
                string installRoot = netFramework.GetValue("InstallRoot").ToString();
                string processorPath = Path.Combine(installRoot, aspNetVersionName + "\\aspnet_isapi.dll");
				// correct mappings for IIS 32-bit mode  
				if (Utils.IsWin64() && Utils.IIS32Enabled())
					processorPath = processorPath.Replace("Framework64", "Framework");

                foreach (string extension in aspNetExtensions)
                {
                    ManagementClass clsScriptMap = wmi.GetClass("ScriptMap");
                    ManagementObject objScriptMap = clsScriptMap.CreateInstance();

                    objScriptMap.Properties["Extensions"].Value = extension;
                    int flags = 5;
                    if (extension == ".soap" || extension == ".rem" || extension == ".vsdisco" ||
                        extension == ".axd" || extension == ".aspx" || extension == ".asmx" || extension == ".ashx")
                        flags = 1;

                    objScriptMap.Properties["Flags"].Value = flags;
                    objScriptMap.Properties["IncludedVerbs"].Value = "GET,HEAD,POST,DEBUG";
                    objScriptMap.Properties["ScriptProcessor"].Value = processorPath;
                    objScriptMap.Put();

                    scriptMaps.Add(objScriptMap);
                }

	            // set script maps
                obj.Properties["ScriptMaps"].Value = scriptMaps.ToArray();
            }
        }

		/// <summary>
		/// Checks whether virtual directory exists.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <param name="directoryName">Directory name.</param>
		/// <returns>True if viertual directory exists, otherwise false.</returns>
		internal static bool VirtualDirectoryExists(string siteId, string directoryName)
		{
			return (wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsWebVirtualDirSetting" +
				" WHERE Name='{0}'", GetVirtualDirectoryPath(siteId, directoryName))).Count > 0);
		}

		/// <summary>
		/// Deletes virtual directory
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="directoryName"></param>
		internal static void DeleteVirtualDirectory(string siteId, string directoryName)
		{
			try
			{ 
				ManagementObject objDir = wmi.GetObject(String.Format("IIsWebVirtualDir='{0}'",
					GetVirtualDirectoryPath(siteId, directoryName)));
				objDir.Delete();
			}
			catch(Exception ex)
			{
				throw new Exception("Can't delete virtual directory", ex);
			}
		}

		/// <summary>
		/// Deletes site
		/// </summary>
		/// <param name="siteId"></param>
		internal static void DeleteSite(string siteId)
		{
			try
			{ 
				ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServer='{0}'", siteId));
				objSite.Delete();
			}
			catch(Exception ex)
			{
				throw new Exception("Can't delete web site", ex);
			}
		}

		/// <summary>
		/// Deletes site
		/// </summary>
		/// <param name="siteId"></param>
		internal static void DeleteIIS7Site(string siteId)
		{
			try
			{
				ServerManager serverManager = new ServerManager();
				Site site = serverManager.Sites[siteId];
				if (site != null)
				{
					serverManager.Sites.Remove(site);
					serverManager.CommitChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Can't delete web site", ex);
			}
		}

		/// <summary>
		/// Check if there is already a web site with the specified server binding.
		/// </summary>
		/// <param name="ip">IP address</param>
		/// <param name="port">Port number</param>
		/// <param name="host">Host header value</param>
		/// <returns></returns>
		internal static string GetSiteIdByBinding(string ip, string port, string host)
		{
			// check for server bindings
			ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM IIsWebServerSetting");
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmi.WmiScope, objectQuery))
			{
				using (ManagementObjectCollection objSites = searcher.Get())
				{
					foreach (ManagementObject objProbSite in objSites)
					{
						string probSiteId = (string)objProbSite.Properties["Name"].Value;

						// check server bindings
						ManagementBaseObject[] objProbBinings = (ManagementBaseObject[])objProbSite.Properties["ServerBindings"].Value;
						if (objProbBinings != null)
						{
							// check this binding against provided ones
							foreach (ManagementBaseObject objProbBinding in objProbBinings)
							{
								string siteIP = (string)objProbBinding.Properties["IP"].Value;
								string sitePort = (string)objProbBinding.Properties["Port"].Value;
								string siteHost = (string)objProbBinding.Properties["Hostname"].Value;

								if ((siteIP == ip && sitePort == port && host.ToLower() == siteHost.ToLower()) ||
									(siteIP == "" && sitePort == port && host.ToLower() == siteHost.ToLower())) // (All unassigned)
									return probSiteId;

								objProbBinding.Dispose();
							}
						}
						objProbSite.Dispose();
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Check if there is already a web site with the specified server binding.
		/// </summary>
		/// <param name="ip">IP address</param>
		/// <param name="port">Port number</param>
		/// <param name="host">Host header value</param>
		/// <returns></returns>
		internal static string GetIIS7SiteIdByBinding(string ip, string port, string host)
		{
			ServerManager serverManager = new ServerManager();
			foreach (Site webSite in serverManager.Sites)
			{
				foreach (Binding binding in webSite.Bindings)
				{
                    if (binding.Protocol != Uri.UriSchemeHttp)
                        continue;

                    string bi = binding.BindingInformation;
					string[] tokens = bi.Split(':');
                    if (tokens.Length != 3)
                        continue;

					string siteIP = tokens[0];
					string sitePort = tokens[1];
					string siteHost = tokens[2];

					if ((siteIP == ip && sitePort == port && host.ToLower() == siteHost.ToLower()) ||
						(siteIP == "*" && sitePort == port && host.ToLower() == siteHost.ToLower())) // (All unassigned)
						return webSite.Name;
				}
			}
			return null;
		}

		/// <summary>
		/// Checks whether application pool exists
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool ApplicationPoolExists(string name)
		{
			WmiHelper wmi = new WmiHelper("root\\MicrosoftIISv2");
			return(wmi.ExecuteQuery(
				String.Format("SELECT * FROM IIsApplicationPool WHERE Name='W3SVC/AppPools/{0}'", name)).Count > 0);
		}

		/// <summary>
		/// Checks whether application pool exists
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool IIS7ApplicationPoolExists(string name)
		{
			ServerManager serverManager = new ServerManager();
			bool ret = (serverManager.ApplicationPools[name] != null);
			return ret;
		}

		/// <summary>
		/// Ctreates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		internal static void CreateApplicationPool(string name, string username, string password)
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
                // specified account
                objPool.Properties["AppPoolIdentityType"].Value = 3;
                objPool.Properties["WAMUserName"].Value = username;
                objPool.Properties["WAMUserPass"].Value = password;
            }
            else
            {
                // NETWORK SERVICE
                objPool.Properties["AppPoolIdentityType"].Value = 2;
            }
			objPool.Put();
		}

		internal static void StartApplicationPool(string name)
		{
			ManagementObject objPool = wmi.GetObject(String.Format("IIsApplicationPool='W3SVC/AppPools/{0}'", name));
			objPool.InvokeMethod("Start", null);
		}

		internal static void StopApplicationPool(string name)
		{
			ManagementObject objPool = wmi.GetObject(String.Format("IIsApplicationPool='W3SVC/AppPools/{0}'", name));
			objPool.InvokeMethod("Stop", null);
			
		}

		internal static void StartIIS7ApplicationPool(string name)
		{
			ServerManager serverManager = new ServerManager();
			ApplicationPool pool = serverManager.ApplicationPools[name];
			if (pool != null)
			{
				pool.Start();
				serverManager.CommitChanges();
			}
		}

		internal static void StopIIS7ApplicationPool(string name)
		{
			ServerManager serverManager = new ServerManager();
			ApplicationPool pool = serverManager.ApplicationPools[name];
			if (pool != null)
			{
				pool.Stop();
				serverManager.CommitChanges();
			}
		}

		/// <summary>
		/// Ctreates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		internal static void CreateIIS7ApplicationPool(string name, string username, string password)
		{
			ServerManager serverManager = new ServerManager();
			ApplicationPool pool = serverManager.ApplicationPools.Add(name);

            if (!String.IsNullOrEmpty(username))
            {
                pool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
                pool.ProcessModel.UserName = username;
                pool.ProcessModel.Password = password;
                pool.ProcessModel.LoadUserProfile = true;
            }
			else
			{
				pool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
			}
			pool.ManagedRuntimeVersion = "v2.0";
			pool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
			serverManager.CommitChanges();
		}

		/// <summary>
		/// Deletes application pool
		/// </summary>
		/// <param name="name"></param>
		internal static void DeleteApplicationPool(string name)
		{
			try
			{
				ManagementObject objPool = wmi.GetObject(String.Format("IIsApplicationPool='W3SVC/AppPools/{0}'",
					name));
				objPool.Delete();
			}
			catch(Exception ex)
			{
				throw new Exception("Can't delete application pool", ex);
			}
		}

		/// <summary>
		/// Deletes application pool
		/// </summary>
		/// <param name="name"></param>
		internal static void DeleteIIS7ApplicationPool(string name)
		{
			try
			{
				ServerManager serverManager = new ServerManager();
				ApplicationPool pool = serverManager.ApplicationPools[name];
				if (pool != null)
				{
					serverManager.ApplicationPools.Remove(pool);
					serverManager.CommitChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Can't delete IIS 7 application pool", ex);
			}
		}

		/// <summary>
		/// Updates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		internal static void UpdateApplicationPool(string name, string username, string password)
		{
			ManagementObject objPool = wmi.GetObject(String.Format("IIsApplicationPoolSetting='W3SVC/AppPools/{0}'",
				name));

            if (!String.IsNullOrEmpty(username))
            {
                // specified account
                objPool.Properties["AppPoolIdentityType"].Value = 3;
                objPool.Properties["WAMUserName"].Value = username;
                objPool.Properties["WAMUserPass"].Value = password;
            }
            else
            {
                // NETWORK SERVICE
                objPool.Properties["AppPoolIdentityType"].Value = 2;
            }
			objPool.Put();
		}

		/// <summary>
		/// Updates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		internal static void UpdateIIS7ApplicationPool(string name, string username, string password)
		{
			ServerManager serverManager = new ServerManager();
			ApplicationPool pool = serverManager.ApplicationPools[name];

			if (!String.IsNullOrEmpty(username))
			{
				pool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
				pool.ProcessModel.UserName = username;
				pool.ProcessModel.Password = password;
                pool.ProcessModel.LoadUserProfile = true;
			}
			else
			{
				pool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
			}
			serverManager.CommitChanges();
		}

		/// <summary>
		/// Updates virtual directory application pool
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="directoryName"></param>
		/// <param name="applicationPoolName"></param>
		internal static void UpdateVirtualDirectoryApplicationPool(string siteId, string directoryName, string applicationPoolName)
		{
			ManagementObject obj = wmi.GetObject(
				String.Format("IIsWebVirtualDirSetting='{0}'", GetVirtualDirectoryPath(siteId, directoryName)));
			obj.Properties["AppPoolId"].Value = applicationPoolName;
			obj.Put();
		}

		/// <summary>
		/// Returns number of sites/virtual directories in the specified application pool.
		/// </summary>
		/// <param name="applicationPoolName"></param>
		/// <returns></returns>
		internal static int GetApplicationPoolSitesCount(string applicationPoolName)
		{
			ManagementObjectCollection objSites = wmi.ExecuteQuery(
				string.Format("SELECT * FROM IIsWebVirtualDirSetting where AppPoolId = '{0}'", applicationPoolName));
			return objSites.Count;
		}

		/// <summary>
		/// Returns number of sites/virtual directories in the specified application pool.
		/// </summary>
		/// <param name="applicationPoolName"></param>
		/// <returns></returns>
		internal static int GetIIS7ApplicationPoolSitesCount(string applicationPoolName)
		{
			ServerManager serverManager = new ServerManager();
			int num = 0;
			foreach (Site site in serverManager.Sites)
			{
				try
				{

					foreach (Application application in site.Applications)
					{
						if (String.Equals(application.ApplicationPoolName, applicationPoolName,
							StringComparison.InvariantCultureIgnoreCase))
						{

							num++;
						}
					}
				}
				catch
				{
					continue;
				}
			}
			return num;
		}

		/*internal static ServerBinding[] GetSiteBindings(string siteId)
		{
			// get web server settings object
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

			WebSiteItem site = new WebSiteItem();
			FillWebSiteFromWmiObject(site, objSite);
			return site.Bindings;
		}*/


		internal static void UpdateSiteBindings(string siteId, ServerBinding[] bindings)
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

		internal static void UpdateIIS7SiteBindings(string siteId, ServerBinding[] bindings)
		{
			ServerManager serverManager = new ServerManager();
			Site webSite = serverManager.Sites[siteId];
			

			// cleanup all bindings
			webSite.Bindings.Clear();
			//
			foreach (ServerBinding binding in bindings)
			{
				//
				webSite.Bindings.Add(binding.IP + ":" + binding.Port + ":" + binding.Host,
					Uri.UriSchemeHttp);
			}
			//
			serverManager.CommitChanges();
		}

		internal static string[] GetIPs()
		{
			List<string> list = new List<string>();
			WmiHelper wmi = new WmiHelper("root\\cimv2");
			ManagementObjectCollection collection = wmi.ExecuteQuery("SELECT IPAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'True'");
			foreach (ManagementObject obj in collection)
			{
				string[] ips = obj["IPAddress"] as string[];
				if (ips != null)
				{
					foreach (string ip in ips)
					{
						if (!list.Contains(ip))
							list.Add(ip);
					}
				}
			}
			return list.ToArray();
		}
		
		internal static string[] GetIPv4Addresses()
		{
			List<string> list = new List<string>();
			WmiHelper wmi = new WmiHelper("root\\cimv2");
			ManagementObjectCollection collection = wmi.ExecuteQuery("SELECT IPAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'True'");
			foreach (ManagementObject obj in collection)
			{
				string[] ips = obj["IPAddress"] as string[];
				if (ips != null)
				{
					foreach (string ip in ips)
					{
						if (!list.Contains(ip) && IsValidIPv4(ip))
							list.Add(ip);
					}
				}
			}
			return list.ToArray();
		}

		private static bool IsValidIPv4(string address)
		{
			return Regex.IsMatch(address, @"^(?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(?=\.?\d)\.)){4}$");
		}

		internal static WebExtensionStatus CheckIIS6WebExtensions()
		{
			WebExtensionStatus status = WebExtensionStatus.NotInstalled;

			DirectoryEntry iis = new DirectoryEntry("IIS://LocalHost/W3SVC");
			foreach (string propertyName in iis.Properties.PropertyNames)
			{
				if (propertyName.Equals("WebSvcExtRestrictionList", StringComparison.InvariantCultureIgnoreCase))
				{
					PropertyValueCollection valueCollection = iis.Properties[propertyName];
					foreach (object objVal in valueCollection)
					{
						if (objVal != null && !string.IsNullOrEmpty(objVal.ToString()))
						{
							string strVal = objVal.ToString().ToLower();
							if (strVal.Contains(@"\v2.0.50727\aspnet_isapi.dll".ToLower()))
							{
								if (strVal[0] == '1')
								{
									status = WebExtensionStatus.Allowed;
								}
								else if (status == WebExtensionStatus.NotInstalled)
								{
									status = WebExtensionStatus.Prohibited;
								}
							}
						}
					}
				}
			}
			return status;
		}
	}
}


