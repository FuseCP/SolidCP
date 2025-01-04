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
//using System.Management;
using Microsoft.Win32;
using Microsoft.Web.Management;
using Microsoft.Web.Administration;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using System.Linq;
using SolidCP.Providers.OS;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using SolidCP.UniversalInstaller.Web;

namespace SolidCP.UniversalInstaller
{
	/// <summary>
	/// Web utils class.
	/// </summary>
	public abstract class WebUtils
	{
		/// <summary>IIS_SERVICE_ID</summary>
		public const string IIS_SERVICE_ID = "W3SVC";

        public const string AspNet11 = "v1.1.4322";
        public const string AspNet20 = "v2.0.50727";
		public const string AspNet40 = "v4.0.30319";

        public const string SolidCP_POOL = "SolidCP Pool";
		public const string SolidCP_ADMIN_POOL = "SolidCP Admin Pool";

        public static string[] aspNet11Maps = new string[] { ".asax", ".ascx", ".ashx",
        ".asmx", ".aspx", ".axd", ".config", ".cs", ".csproj", ".licx", ".rem", ".resources", ".resx",
        ".soap", ".vb", ".vbproj", ".vsdisco", ".webinfo"};

        public static string[] aspNet20Maps = new string[] { ".ad", ".adprototype", ".asax", ".ascx", ".ashx",
        ".asmx", ".aspx", ".axd", ".browser", ".cd", ".compiled", ".config", ".cs", ".csproj", ".dd",
        ".exclude", ".java", ".jsl", ".ldb", ".ldd", ".lddprototype", ".ldf", ".licx", ".master",
        ".mdb", ".mdf", ".msgx", ".refresh", ".rem", ".resources", ".resx", ".sd", ".sdm", ".sdmDocument",
        ".sitemap", ".skin", ".soap", ".svc", ".vb", ".vbproj", ".vjsproj", ".vsdisco", ".webinfo"};

		public static string[] aspNet40Maps = new string[] { ".ad", ".adprototype", ".asax", ".ascx", ".ashx",
        ".asmx", ".aspx", ".axd", ".browser", ".cd", ".compiled", ".config", ".cs", ".csproj", ".dd",
        ".exclude", ".java", ".jsl", ".ldb", ".ldd", ".lddprototype", ".ldf", ".licx", ".master",
        ".mdb", ".mdf", ".msgx", ".refresh", ".rem", ".resources", ".resx", ".sd", ".sdm", ".sdmDocument",
        ".sitemap", ".skin", ".soap", ".svc", ".vb", ".vbproj", ".vjsproj", ".vsdisco", ".webinfo"};

		/// <summary>
		/// Retrieves web sites.
		/// </summary>
		/// <returns>Web sites.</returns>
		public abstract WebSiteItem[] GetWebSites();

		/*
				/// <summary>
				/// Creates virtual directory.
				/// </summary>
				/// <param name="siteId">Site id.</param>
				/// <param name="directoryName">Directory name.</param>
				/// <param name="contentPath">Content path.</param>
				/// <param name="aspNet">ASP.NET version</param>
				public static void CreateVirtualDirectory(string siteId, string directoryName, string contentPath,
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
		public abstract void SetWebFolderPermissions(string path, string userDomain, string userAccount);

		/// <summary>
		/// Checks if the site exists.
		/// </summary>
		/// <param name="siteId">SiteID</param>
		/// <returns></returns>
		public abstract bool SiteIdExists(string siteId);

		/// <summary>
		/// Checks if the site exists.
		/// </summary>
		/// <param name="siteId">SiteID</param>
		/// <returns></returns>
		public abstract bool IIS7SiteExists(string siteId);

		/// <summary>
		/// Retreives site by site id.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <returns>Site object.</returns>
		public abstract WebSiteItem GetSite(string siteId);

		public abstract string GetVirtualDirectoryPath(string siteId, string directoryName);

		/// <summary>
		/// Checks site bindings.
		/// </summary>
		/// <param name="ip">IP address.</param>
		/// <param name="port">TCP port.</param>
		/// <param name="host">Host header value.</param>
		/// <returns>True if site binding exist, otherwise false.</returns>
		public abstract bool CheckSiteBindings(string ip, string port, string host);

		/// <summary>
		/// Creates site.
		/// </summary>
		/// <param name="site">Site object.</param>
		/// <returns>Site id.</returns>
		public abstract string CreateSite(WebSiteItem site);

		/// <summary>
		/// Creates site.
		/// </summary>
		/// <param name="site">Site object.</param>
		/// <returns>Site id.</returns>
		public abstract string CreateIIS7Site(WebSiteItem site);

		/// <summary>
		/// Updates site
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="contentPath"></param>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		/// <param name="host"></param>
		/// <param name="aspNet">ASP.NET version</param>
		public abstract void UpdateSite(string siteId, string contentPath, string ip, string port, string host,
			AspNetVersion aspNet);

		/// <summary>
		/// Changes site state.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <param name="state">Server state.</param>
		public abstract void ChangeSiteState(string siteId, ServerState state);

		/// <summary>
		/// Checks whether virtual directory exists.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <param name="directoryName">Directory name.</param>
		/// <returns>True if viertual directory exists, otherwise false.</returns>
		public abstract bool VirtualDirectoryExists(string siteId, string directoryName);

		/// <summary>
		/// Deletes virtual directory
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="directoryName"></param>
		public abstract void DeleteVirtualDirectory(string siteId, string directoryName);

		/// <summary>
		/// Deletes site
		/// </summary>
		/// <param name="siteId"></param>
		public abstract void DeleteSite(string siteId);

		/// <summary>
		/// Deletes site
		/// </summary>
		/// <param name="siteId"></param>
		public abstract void DeleteIIS7Site(string siteId);

		/// <summary>
		/// Check if there is already a web site with the specified server binding.
		/// </summary>
		/// <param name="ip">IP address</param>
		/// <param name="port">Port number</param>
		/// <param name="host">Host header value</param>
		/// <returns></returns>
		public abstract string GetSiteIdByBinding(string ip, string port, string host);

		/// <summary>
		/// Check if there is already a web site with the specified server binding.
		/// </summary>
		/// <param name="ip">IP address</param>
		/// <param name="port">Port number</param>
		/// <param name="host">Host header value</param>
		/// <returns></returns>
		public abstract string GetIIS7SiteIdByBinding(string ip, string port, string host);

		/// <summary>
		/// Checks whether application pool exists
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public abstract bool ApplicationPoolExists(string name);

		/// <summary>
		/// Checks whether application pool exists
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public abstract bool IIS7ApplicationPoolExists(string name);

		/// <summary>
		/// Creates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public abstract void CreateApplicationPool(string name, string username, string password);
		public abstract void StartApplicationPool(string name);

		public abstract void StopApplicationPool(string name);

		public abstract void StartIIS7ApplicationPool(string name);
		public abstract void StopIIS7ApplicationPool(string name);

		/// <summary>
		/// Creates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public abstract void CreateIIS7ApplicationPool(string name, string username, string password);

		/// <summary>
		/// Deletes application pool
		/// </summary>
		/// <param name="name"></param>
		public abstract void DeleteApplicationPool(string name);

		/// <summary>
		/// Deletes application pool
		/// </summary>
		/// <param name="name"></param>
		public abstract void DeleteIIS7ApplicationPool(string name);
		/// <summary>
		/// Updates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public abstract void UpdateApplicationPool(string name, string username, string password);

		/// <summary>
		/// Updates application pool
		/// </summary>
		/// <param name="name"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public abstract void UpdateIIS7ApplicationPool(string name, string username, string password);

		/// <summary>
		/// Updates virtual directory application pool
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="directoryName"></param>
		/// <param name="applicationPoolName"></param>
		public abstract void UpdateVirtualDirectoryApplicationPool(string siteId, string directoryName, string applicationPoolName);

		/// <summary>
		/// Returns number of sites/virtual directories in the specified application pool.
		/// </summary>
		/// <param name="applicationPoolName"></param>
		/// <returns></returns>
		public abstract int GetApplicationPoolSitesCount(string applicationPoolName);

		/// <summary>
		/// Returns number of sites/virtual directories in the specified application pool.
		/// </summary>
		/// <param name="applicationPoolName"></param>
		/// <returns></returns>
		public abstract int GetIIS7ApplicationPoolSitesCount(string applicationPoolName);

		/*public static ServerBinding[] GetSiteBindings(string siteId)
		{
			// get web server settings object
			ManagementObject objSite = wmi.GetObject(String.Format("IIsWebServerSetting='{0}'", siteId));

			WebSiteItem site = new WebSiteItem();
			FillWebSiteFromWmiObject(site, objSite);
			return site.Bindings;
		}*/

		public abstract void UpdateSiteBindings(string siteId, ServerBinding[] bindings);

		public abstract void UpdateIIS7SiteBindings(string siteId, ServerBinding[] bindings);
		public abstract string[] GetIPs();
		public abstract string[] GetIPv4Addresses();
		public abstract string[] GetIPv4AddressesWindows();

		public abstract WebExtensionStatus CheckIIS6WebExtensions();

#if DEBUG
		const bool Debug = true;
#else
		const bool Debug = false;
#endif

		public abstract bool LEInstallCertificate(string site, string domain, string email, bool updateWCF = true, bool updateIIS = true);
	}
}


