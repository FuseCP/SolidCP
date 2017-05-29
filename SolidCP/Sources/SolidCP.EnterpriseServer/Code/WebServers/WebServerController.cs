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
using System.Data;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.DNS;
using OS = SolidCP.Providers.OS;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using System.Resources;
using System.Threading;
using System.Reflection;
using SolidCP.Templates;
using SolidCP.Providers.Database;
using SolidCP.Providers.FTP;
using System.Collections;

namespace SolidCP.EnterpriseServer
{
    public class WebServerController : IImportController, IBackupController
    {
        private const string LOG_SOURCE_WEB = "WEB";

        private const int DOMAIN_RANDOM_LENGTH = 8;
        private const int MAX_ACCOUNT_LENGTH = 20;
        private const string ANON_ACCOUNT_SUFFIX = "_web";
        private const string FRONTPAGE_ACCOUNT_SUFFIX = "_fp";

        private const string WEBSITE_ROOT_FOLDER_PATTERN = "\\[DOMAIN_NAME]";
        private const string WEBSITE_LOGS_FOLDER_PATTERN = "\\WebLogs\\[DOMAIN_NAME]";
        private const string WEBSITE_DATA_FOLDER_PATTERN = "\\WebData\\[DOMAIN_NAME]";

        public static WebServer GetWebServer(int serviceId)
        {
            WebServer ws = new WebServer();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }

        #region Web Sites
        public static DataSet GetRawWebSitesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, typeof(WebSite),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<WebSite> GetWebSites(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(WebSite), recursive);

            List<WebSite> sites = items.ConvertAll<WebSite>(new Converter<ServiceProviderItem, WebSite>(ConvertItemToWebSite));
			foreach (WebSite site in sites)
			{
				IPAddressInfo ip = ServerController.GetIPAddress(site.SiteIPAddressId);
				if(ip != null)
					site.SiteIPAddress = ip.ExternalIP;
			}
			return sites;
        }

        private static WebSite ConvertItemToWebSite(ServiceProviderItem item)
        {
            return (WebSite)item;
        }

        public static WebSite GetWebSite(int packageId, string siteName)
        {
            ServiceProviderItem siteItem = PackageController.GetPackageItemByName(packageId, siteName, typeof(WebSite));
            if (siteItem == null)
                return null;

            return GetWebSite(siteItem.Id);
        }

        public static WebSite GetWebSite(int siteItemId)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // load live site from service
            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);
            WebSite site = web.GetSite(siteItem.SiteId);
            
            // set other properties
            site.Id = siteItem.Id;
            site.Name = siteItem.Name;
            site.ServiceId = siteItem.ServiceId;
            site.PackageId = siteItem.PackageId;

            // load IP address
            site.SiteIPAddressId = siteItem.SiteIPAddressId;
            IPAddressInfo ip = ServerController.GetIPAddress(siteItem.SiteIPAddressId);
            if(ip != null)
                site.SiteIPAddress = ip.ExternalIP;

            // check if site has dedicated IP assigned
            var siteIpAddresses = ServerController.GetItemIPAddresses(siteItemId, IPAddressPool.None);
            foreach (var siteIp in siteIpAddresses)
            {
                var packageIpAddress = ServerController.GetPackageIPAddress(siteIp.AddressID);
                if (packageIpAddress != null && packageIpAddress.ExternalIP == site.SiteIPAddress)
                {
                    site.IsDedicatedIP = true;
                    break;
                }
            }

            // truncate home folder
            site.ContentPath = FilesController.GetVirtualPackagePath(siteItem.PackageId, site.ContentPath);

            //check if Coldfusion is available
            //site.ColdFusionAvailable = siteItem.ColdFusionAvailable;
            
            // set FrontPage account
            site.FrontPageAccount = siteItem.FrontPageAccount;
            if (String.IsNullOrEmpty(site.FrontPageAccount))
                site.FrontPageAccount = GetFrontPageUsername(site.Name);

			// Set Web Deploy publishing account
			site.WebDeployPublishingAccount = siteItem.WebDeployPublishingAccount;
			// Set Web Deploy site publishing enabled
			site.WebDeploySitePublishingEnabled = siteItem.WebDeploySitePublishingEnabled;
			// Set Web Deploy site publishing profile
			site.WebDeploySitePublishingProfile = siteItem.WebDeploySitePublishingProfile;

            return site;
        }

        public static int AddWebSite(int packageId, string hostName, int domainId, int ipAddressId)
        {
            return AddWebSite(packageId, hostName, domainId, ipAddressId, false, true);
        }


        private static bool IsValidIPAdddress(string addr)
        {
            System.Net.IPAddress ip;
            if (System.Net.IPAddress.TryParse(addr, out ip)) 
            {
                return ((ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) |
                        (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            }
            else 
            {
                return false;
            }
        }

        public static int AddWebSite(int packageId, string hostName, int domainId, int packageAddressId,
            bool addInstantAlias, bool ignoreGlobalDNSRecords)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo sitesQuota = PackageController.GetPackageQuota(packageId, Quotas.WEB_SITES);
            if (sitesQuota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_WEB_SITES_QUOTA_LIMIT;

            // load domain name
            DomainInfo domain = ServerController.GetDomain(domainId);
            string domainName = domain.DomainName;

            string siteName = string.IsNullOrEmpty(hostName) ? domainName : hostName + "." + domainName; 

            // check if the web site already exists (legacy)
            if (PackageController.GetPackageItemByName(packageId, siteName, typeof(WebSite)) != null)
                return BusinessErrorCodes.ERROR_WEB_SITE_ALREADY_EXISTS;

            if (DataProvider.CheckDomain(domain.PackageId, siteName, true) != 0)
                return BusinessErrorCodes.ERROR_WEB_SITE_ALREADY_EXISTS;


            // place log record
            TaskManager.StartTask("WEB_SITE", "ADD", siteName);

            try
            {

                // get service
                int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
                if (serviceId == 0)
                    return BusinessErrorCodes.ERROR_WEB_SITE_SERVICE_UNAVAILABLE;


				// Initialize IIS provider webservice proxy
				WebServer web = new WebServer();
				ServiceProviderProxy.Init(web, serviceId);

				// Ensure the web site is being created doesn't exist on the server
                if (web.SiteExists(siteName))
				{
					//
					PackageInfo packageInfo = PackageController.GetPackage(packageId);
					//
					ServerInfo serverInfo = ServerController.GetServerById(packageInfo.ServerId);
					// Give as much clues for the issue to an administrator as possible
					TaskManager.WriteError("Web site '{0}' could not be created because site with the name requested already " +
                        "exists on '{1}' server.", siteName, serverInfo.ServerName);
					// Return generic operation failed error
					return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
				} 

                // load web settings
                StringDictionary webSettings = ServerController.GetServiceSettings(serviceId);
                int addressId = Utils.ParseInt(webSettings["SharedIP"], 0);

                bool dedicatedIp = false;
                if (packageAddressId != 0)
                {
                    // dedicated IP
                    PackageIPAddress packageIp = ServerController.GetPackageIPAddress(packageAddressId);
                    if (packageIp != null)
                    {
                        addressId = packageIp.AddressID;
                        dedicatedIp = true;
                    }
                }

                // load assigned IP address
                string ipAddr = "*";
                IPAddressInfo ip = ServerController.GetIPAddress(addressId);
                if (ip != null)
                    ipAddr = !String.IsNullOrEmpty(ip.InternalIP) ? ip.InternalIP : ip.ExternalIP;


                // load web DNS records
                List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(serviceId);

                if (dedicatedIp)
                {
                    foreach (GlobalDnsRecord d in dnsRecords)
                    {
                        if (!string.IsNullOrEmpty(d.ExternalIP))
                        {
                            if (!IsValidIPAdddress(d.ExternalIP)) return BusinessErrorCodes.ERROR_GLOBALDNS_FOR_DEDICATEDIP;
                        }
                    }
                }
                else
                {
                    if (domain.ZoneItemId > 0)
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(serviceId);
                        if (string.IsNullOrEmpty(settings["PublicSharedIP"]))
                            return BusinessErrorCodes.ERROR_PUBLICSHAREDIP_FOR_SHAREDIP;
                    }
                }

                // prepare site bindings
                List<ServerBinding> bindings = new List<ServerBinding>();

                // SHARED IP
                // fill main domain bindings
                FillWebServerBindings(bindings, dnsRecords, ipAddr, hostName, domain.DomainName, ignoreGlobalDNSRecords);

                //double check all bindings
                foreach (ServerBinding b in bindings)
                {
                    if (DataProvider.CheckDomain(domain.PackageId, b.Host, true) != 0)
                        return BusinessErrorCodes.ERROR_WEB_SITE_ALREADY_EXISTS;
                }

                if (dedicatedIp)
                {
                    // DEDICATED IP
                    bindings.Add(new ServerBinding(ipAddr, "80", ""));
                }

                UserInfo user = PackageController.GetPackageOwner(packageId);
                UserSettings webPolicy = UserController.GetUserSettings(user.UserId, UserSettings.WEB_POLICY);

                // craete web site
                string siteId = null;
                WebSite site = new WebSite();

                // web site name and bindings
                site.Name = siteName;
                site.Bindings = bindings.ToArray();

                site.AnonymousUsername = GetWebSiteUsername(webPolicy, siteName);
                site.AnonymousUserPassword = Guid.NewGuid().ToString("P");

                // folders
                string packageHome = FilesController.GetHomeFolder(packageId);

                // add random string to the domain if specified
                string randDomainName = siteName;
                if (!String.IsNullOrEmpty(webPolicy["AddRandomDomainString"])
                    && Utils.ParseBool(webPolicy["AddRandomDomainString"], false))
                    randDomainName += "_" + Utils.GetRandomString(DOMAIN_RANDOM_LENGTH);

                // ROOT folder
                site.ContentPath = GetWebFolder(packageId, WEBSITE_ROOT_FOLDER_PATTERN, randDomainName);
                if (!String.IsNullOrEmpty(webPolicy["WebRootFolder"]))
                    site.ContentPath = GetWebFolder(packageId, webPolicy["WebRootFolder"], randDomainName);

                // LOGS folder
                site.LogsPath = GetWebFolder(packageId, WEBSITE_LOGS_FOLDER_PATTERN, randDomainName);
                if (!String.IsNullOrEmpty(webPolicy["WebLogsFolder"]))
                    site.LogsPath = GetWebFolder(packageId, webPolicy["WebLogsFolder"], randDomainName);

                // DATA folder
                site.DataPath = GetWebFolder(packageId, WEBSITE_DATA_FOLDER_PATTERN, randDomainName);
                if (!String.IsNullOrEmpty(webPolicy["WebDataFolder"]))
                    site.DataPath = GetWebFolder(packageId, webPolicy["WebDataFolder"], randDomainName);

                // default documents
                site.DefaultDocs = null; // inherit from service
                if (!String.IsNullOrEmpty(webPolicy["DefaultDocuments"]))
                    site.DefaultDocs = webPolicy["DefaultDocuments"];

				if (!String.IsNullOrEmpty(webPolicy["EnableWritePermissions"]))
                {
                    // security settings
                    site.EnableWritePermissions = Utils.ParseBool(webPolicy["EnableWritePermissions"], false);
                    site.EnableDirectoryBrowsing = Utils.ParseBool(webPolicy["EnableDirectoryBrowsing"], false);
                    site.EnableParentPaths = Utils.ParseBool(webPolicy["EnableParentPaths"], false);
					site.DedicatedApplicationPool = Utils.ParseBool(webPolicy["EnableDedicatedPool"], false);
                    

					// Ensure the website meets hosting plan quotas
					QuotaValueInfo quotaInfo = PackageController.GetPackageQuota(packageId, Quotas.WEB_APPPOOLS);
					site.DedicatedApplicationPool = site.DedicatedApplicationPool && (quotaInfo.QuotaAllocatedValue > 0);

                    site.EnableAnonymousAccess = Utils.ParseBool(webPolicy["EnableAnonymousAccess"], false);
                    site.EnableWindowsAuthentication = Utils.ParseBool(webPolicy["EnableWindowsAuthentication"], false);
                    site.EnableBasicAuthentication = Utils.ParseBool(webPolicy["EnableBasicAuthentication"], false);

                    // extensions
                    site.AspInstalled = Utils.ParseBool(webPolicy["AspInstalled"], false);
                    site.AspNetInstalled = webPolicy["AspNetInstalled"];
                    site.PhpInstalled = webPolicy["PhpInstalled"];
                    site.PerlInstalled = Utils.ParseBool(webPolicy["PerlInstalled"], false);
                    site.PythonInstalled = Utils.ParseBool(webPolicy["PythonInstalled"], false);
                    site.CgiBinInstalled = Utils.ParseBool(webPolicy["CgiBinInstalled"], false);
					QuotaValueInfo quotaInfoCF = PackageController.GetPackageQuota(packageId, Quotas.WEB_COLDFUSION);
                    site.ColdFusionInstalled = (quotaInfoCF.QuotaAllocatedValue > 0) && Utils.ParseBool(webPolicy["ColdFusionInstalled"], false);
					QuotaValueInfo quotaInfoCFV = PackageController.GetPackageQuota(packageId, Quotas.WEB_CFVIRTUALDIRS);
					site.CreateCFVirtualDirectoriesPol = (quotaInfoCFV.QuotaAllocatedValue > 0) && Utils.ParseBool(webPolicy["CreateCFVirtualDirectoriesPol"], false);
                    
                }
                else
                {
                    // security settings
                    site.EnableWritePermissions = false;
                    site.EnableDirectoryBrowsing = false;
                    site.EnableParentPaths = false;
                    site.DedicatedApplicationPool = false;

                    site.EnableAnonymousAccess = true;
                    site.EnableWindowsAuthentication = true;
                    site.EnableBasicAuthentication = false;

                    // extensions
                    site.AspInstalled = true;
                    site.AspNetInstalled = "1";
                    site.PhpInstalled = "";
                    site.PerlInstalled = false;
                    site.PythonInstalled = false;
                    site.CgiBinInstalled = false;
                    site.ColdFusionInstalled = false;
					site.CreateCFVirtualDirectoriesPol = false;
                }

                site.HttpRedirect = "";
                site.HttpErrors = null;
                site.MimeMaps = null;

                // CREATE WEB SITE
                siteId = web.CreateSite(site);

                // register item
                site.ServiceId = serviceId;
                site.PackageId = packageId;
                site.Name = siteName;
                site.SiteIPAddressId = addressId;
                site.SiteId = siteId;

                int siteItemId = PackageController.AddPackageItem(site);

                // associate IP with web site
                if (packageAddressId != 0)
                    ServerController.AddItemIPAddress(siteItemId, packageAddressId);

                // update domain
                // add main pointer
                AddWebSitePointer(siteItemId, hostName, domain.DomainId, false, ignoreGlobalDNSRecords, false);

               
                // add parking page
                // load package
                if (webPolicy["AddParkingPage"] != null)
                {
                    bool addParkingPage = Utils.ParseBool(webPolicy["AddParkingPage"], false);
                    if (addParkingPage)
                    {
                        // add page
                        string pageName = webPolicy["ParkingPageName"];
                        string pageContent = webPolicy["ParkingPageContent"];

                        if (!String.IsNullOrEmpty(pageName)
                            && pageContent != null)
                        {

                            if (Utils.ParseBool(webPolicy["EnableParkingPageTokens"], false))
                            {
                                pageContent = pageContent.Replace("[DOMAIN_NAME]", site.Name);
                                pageContent = pageContent.Replace("[SITE_IP]", site.SiteIPAddress);

                            }
                            string path = Path.Combine(
                                 FilesController.GetVirtualPackagePath(packageId, site.ContentPath), pageName);

                            if (!FilesController.FileExists(packageId, path))
                            {
                                FilesController.CreateFile(packageId, path);

								byte[] content = Encoding.UTF8.GetBytes(pageContent);
								byte[] fileContent = new byte[content.Length + 3];
								fileContent[0] = 0xEF;
								fileContent[1] = 0xBB;
								fileContent[2] = 0xBF;
								content.CopyTo(fileContent, 3);
                                FilesController.UpdateFileBinaryContent(packageId, path, fileContent);
                            }
                        }
                    }
                }

                TaskManager.ItemId = siteItemId;

                return siteItemId;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int UpdateWebSite(WebSite site)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load web site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(site.Id);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE", siteItem.Name);
            TaskManager.ItemId = site.Id;

            try
            {
                // update home folder
                string origPath = site.ContentPath;
                site.ContentPath = FilesController.GetFullPackagePath(site.PackageId, site.ContentPath);

                // build data folder path
                site.DataPath = siteItem.DataPath;

                // update site on the service
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                web.UpdateSite(site);
				// Restore settings back
				#region Web Deploy Settings
				site.WebDeployPublishingAccount = siteItem.WebDeployPublishingAccount;
				site.WebDeployPublishingPassword = siteItem.WebDeployPublishingPassword;
				site.WebDeploySitePublishingEnabled = siteItem.WebDeploySitePublishingEnabled;
				site.WebDeploySitePublishingProfile = siteItem.WebDeploySitePublishingProfile;
				#endregion

                // update service item
                PackageController.UpdatePackageItem(site);

                // set origpath
                site.ContentPath = origPath;

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int RepairWebSite(int siteItemId)
        {
            return 0;
        }

        public static int ChangeSiteState(int siteItemId, ServerState state)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(siteItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("WEB_SITE", "CHANGE_STATE", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("New state", state);

            try
            {

                // change state
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                web.ChangeSiteState(siteItem.SiteId, state);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        // AppPool
        public static int ChangeAppPoolState(int siteItemId, AppPoolState state)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(siteItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("WEB_SITE", "CHANGE_STATE", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("New state", state);

            try
            {

                // change state
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                web.ChangeAppPoolState(siteItem.SiteId, state);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static AppPoolState GetAppPoolState(int siteItemId)
        {
            AppPoolState state = AppPoolState.Unknown;

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return state;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return state;

            // check package
            int packageCheck = SecurityContext.CheckPackage(siteItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return state;

            // place log record
            TaskManager.StartTask("WEB_SITE", "GET_STATE", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            try
            {
                // get state
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                state = web.GetAppPoolState(siteItem.SiteId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return state;
        }



        public static int DeleteWebSite(int siteItemId, bool deleteWebsiteDirectory)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load web site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            // delete web site
            try
            {
				//cleanup certificates
                //cleanup certificates
                List<SSLCertificate> certificates = GetCertificatesForSite(siteItemId);
                foreach (SSLCertificate c in certificates)
                {
                    DeleteCertificate(siteItemId, c);
                }
                
                // remove all web site pointers
				List<DomainInfo> pointers = GetWebSitePointers(siteItemId);
				foreach (DomainInfo pointer in pointers)
					DeleteWebSitePointer(siteItemId, pointer.DomainId, false, true, true);

				// remove web site main pointer
				DomainInfo domain = ServerController.GetDomain(siteItem.Name);
				if(domain != null)
					DeleteWebSitePointer(siteItemId, domain.DomainId, false, true, true);

                List<WebSite> sites = WebServerController.GetWebSites(domain.PackageId, false);
                bool oneSiteOnly = (sites.Count == 1);

                if (oneSiteOnly)
                {
                    // load site item
                    IPAddressInfo ip = ServerController.GetIPAddress(sites[0].SiteIPAddressId);

                    string serviceIp = (ip != null) ? ip.ExternalIP : null;

                    if (string.IsNullOrEmpty(serviceIp))
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(sites[0].ServiceId);
                        if (settings["PublicSharedIP"] != null)
                            serviceIp = settings["PublicSharedIP"].ToString();
                    }

                    ServerController.RemoveServiceDNSRecords(domain.PackageId, ResourceGroups.Web, domain, serviceIp, true);
                }




				// delete web site
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);

				//
				if (web.IsFrontPageSystemInstalled() && web.IsFrontPageInstalled(siteItem.SiteId))
				{
					web.UninstallFrontPage(siteItem.SiteId, siteItem.FrontPageAccount);
				} 

				//
                web.DeleteSite(siteItem.SiteId);

                // Delete WebManagementAccess Account
                WebServerController.RevokeWebManagementAccess(siteItemId);

                if (deleteWebsiteDirectory)
                {
                    // Delete website directory from file server
                    // This will remove the hard quota as well
                    FilesController.DeleteDirectoryRecursive(siteItem.PackageId, new DirectoryInfo(siteItem.DataPath).Parent.FullName);

                }
                // delete service item
                PackageController.DeletePackageItem(siteItemId);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int SwitchWebSiteToDedicatedIP(int siteItemId, int ipAddressId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load web site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            
            int addressId = 0;
            PackageIPAddress packageIp = ServerController.GetPackageIPAddress(ipAddressId);
            if (packageIp != null)
            {
                addressId = packageIp.AddressID;
            }

            List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(siteItem.ServiceId);

            foreach (GlobalDnsRecord d in dnsRecords)
            {
                if (!string.IsNullOrEmpty(d.ExternalIP))
                {
                    if (!IsValidIPAdddress(d.ExternalIP)) return BusinessErrorCodes.ERROR_GLOBALDNS_FOR_DEDICATEDIP;
                }
            }

            // place log record
            TaskManager.StartTask("WEB_SITE", "SWITCH_TO_DEDICATED_IP", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            try
            {
                IPAddressInfo ip;

                // remove all web site pointers
                DomainInfo domain = ServerController.GetDomain(siteItem.Name);
                DomainInfo ZoneInfo = ServerController.GetDomain(domain.DomainItemId);

                if (ZoneInfo == null)
                    throw new Exception("Parent zone not found");

                //cleanup certificates
                List<SSLCertificate> certificates = GetCertificatesForSite(siteItemId);
                foreach (SSLCertificate c in certificates)
                {
                    DeleteCertificate(siteItemId, c);
                }

                certificates = GetPendingCertificates(siteItemId);
                foreach (SSLCertificate c in certificates)
                {
                    DeleteCertificateRequest(siteItemId, c.id);
                }
                
                List<DomainInfo> pointers = GetWebSitePointers(siteItemId);
                foreach (DomainInfo pointer in pointers)
                    DeleteWebSitePointer(siteItemId, pointer.DomainId, true, true, false);

                // remove web site main pointer
                if (domain != null)
                    DeleteWebSitePointer(siteItemId, domain.DomainId, true, true, false);

                // clear binding left overs
                List<ServerBinding> newBindings = new List<ServerBinding>();
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                try
                {
                    web.UpdateSiteBindings(siteItem.SiteId, newBindings.ToArray(), true);
                }
                catch (Exception)
                {

                }

                List<WebSite> sites = WebServerController.GetWebSites(domain.PackageId, false);
                bool oneSiteOnly = (sites.Count == 1);

                if (oneSiteOnly)
                {
                    // load site item
                    ip = ServerController.GetIPAddress(sites[0].SiteIPAddressId);

                    string serviceIp = (ip != null) ? ip.ExternalIP : null;

                    if (string.IsNullOrEmpty(serviceIp))
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(sites[0].ServiceId);
                        if (settings["PublicSharedIP"] != null)
                            serviceIp = settings["PublicSharedIP"].ToString();
                    }

                    ServerController.RemoveServiceDNSRecords(domain.PackageId, ResourceGroups.Web, domain, serviceIp, true);
                }


                // update site item
                siteItem.SiteIPAddressId = addressId;
                PackageController.UpdatePackageItem(siteItem);

                // associate IP with web site
                ServerController.AddItemIPAddress(siteItemId, ipAddressId);


                string parentZone = domain.ZoneName;
                if (string.IsNullOrEmpty(parentZone))
                {
                    DomainInfo parentDomain = ServerController.GetDomain(domain.DomainItemId);
                    parentZone = parentDomain.DomainName;
                }


                AddWebSitePointer(siteItemId,
                    ((domain.DomainName.Replace("." + parentZone, "") == parentZone) |
                    (domain.DomainName == parentZone))
                    ? "" : domain.DomainName.Replace("." + parentZone, "")
                    , ZoneInfo.DomainId, true, true, true);

                foreach (DomainInfo pointer in pointers)
                {
                    string pointerParentZone = pointer.ZoneName;
                    if (string.IsNullOrEmpty(pointerParentZone))
                    {
                        DomainInfo parentDomain = ServerController.GetDomain(pointer.DomainItemId);
                        pointerParentZone = parentDomain.DomainName;
                    }


                    ZoneInfo = ServerController.GetDomain(pointerParentZone);

                    AddWebSitePointer(siteItemId,
                        ((pointer.DomainName.Replace("." + pointerParentZone, "") == pointerParentZone)  |
                        (pointer.DomainName == pointerParentZone))
                        ? "" : pointer.DomainName.Replace("." + pointerParentZone, "")
                        , ZoneInfo.DomainId, true, true, true);
                }

                // load web site IP address
                ip = ServerController.GetIPAddress(siteItem.SiteIPAddressId);
                string ipAddr = "*";
                if (ip != null)
                    ipAddr = !String.IsNullOrEmpty(ip.InternalIP) ? ip.InternalIP : ip.ExternalIP;

                newBindings = new List<ServerBinding>();

                ServerBinding srvBinding = new ServerBinding(ipAddr, "80", "");
                newBindings.Add(srvBinding);

                foreach (ServerBinding b in web.GetSiteBindings(siteItem.SiteId))
                {
                    if (!((b.Host == srvBinding.Host) &
                        (b.IP == srvBinding.IP) &
                        (b.Port == srvBinding.Port)))
                        newBindings.Add(b);
                }
               
                web.UpdateSiteBindings(siteItem.SiteId, newBindings.ToArray(), false);

                if (oneSiteOnly)
                {
                    sites = WebServerController.GetWebSites(domain.PackageId, false);

                    // load site item
                    ip = ServerController.GetIPAddress(sites[0].SiteIPAddressId);

                    string serviceIp = (ip != null) ? ip.ExternalIP : null;

                    if (string.IsNullOrEmpty(serviceIp))
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(sites[0].ServiceId);
                        if (settings["PublicSharedIP"] != null)
                            serviceIp = settings["PublicSharedIP"].ToString();
                    }

                    ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Web, domain, serviceIp, true);
                }
                
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int SwitchWebSiteToSharedIP(int siteItemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load web site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;



            // place log record
            TaskManager.StartTask("WEB_SITE", "SWITCH_TO_SHARED_IP", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            try
            {
                IPAddressInfo ip;

                DomainInfo domain = ServerController.GetDomain(siteItem.Name);
                DomainInfo ZoneInfo = ServerController.GetDomain(domain.DomainItemId);

                if (ZoneInfo == null)
                    throw new Exception("Parent zone not found");


                if (ZoneInfo.ZoneItemId > 0)
                {
                    StringDictionary settings = ServerController.GetServiceSettings(siteItem.ServiceId);
                    if (string.IsNullOrEmpty(settings["PublicSharedIP"]))
                        return BusinessErrorCodes.ERROR_PUBLICSHAREDIP_FOR_SHAREDIP;

                }

                //cleanup certificates
                List<SSLCertificate> certificates = GetCertificatesForSite(siteItemId);
                foreach (SSLCertificate c in certificates)
                {
                    DeleteCertificate(siteItemId, c);
                }

                certificates = GetPendingCertificates(siteItemId);
                foreach (SSLCertificate c in certificates)
                {
                    DeleteCertificateRequest(siteItemId, c.id);
                }

                // remove all web site pointers
                List<DomainInfo> pointers = GetWebSitePointers(siteItemId);
                foreach (DomainInfo pointer in pointers)
                    DeleteWebSitePointer(siteItemId, pointer.DomainId, true, true, false);

                // remove web site main pointer
                if (domain != null)
                    DeleteWebSitePointer(siteItemId, domain.DomainId, true, true, false);

                // clear binding left overs
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                List<ServerBinding> newBindings = new List<ServerBinding>();

                try
                {
                    web.UpdateSiteBindings(siteItem.SiteId, newBindings.ToArray(), true);
                }
                catch (Exception)
                {

                }


                List<WebSite> sites = WebServerController.GetWebSites(domain.PackageId, false);
                bool oneSiteOnly = (sites.Count == 1);

                if (oneSiteOnly)
                {
                    // load site item
                    ip = ServerController.GetIPAddress(sites[0].SiteIPAddressId);

                    string serviceIp = (ip != null) ? ip.ExternalIP : null;

                    if (string.IsNullOrEmpty(serviceIp))
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(sites[0].ServiceId);
                        if (settings["PublicSharedIP"] != null)
                            serviceIp = settings["PublicSharedIP"].ToString();
                    }

                    ServerController.RemoveServiceDNSRecords(domain.PackageId, ResourceGroups.Web, domain, serviceIp, true);
                }


                //figure out the PackageAddressId
                PackageIPAddress packageIpAddress = null;
                var siteIpAddresses = ServerController.GetItemIPAddresses(siteItemId, IPAddressPool.None);
                foreach (var siteIp in siteIpAddresses)
                {
                    packageIpAddress = ServerController.GetPackageIPAddress(siteIp.AddressID);
                    if (packageIpAddress != null && packageIpAddress.AddressID == siteItem.SiteIPAddressId)
                    {
                        break;
                    }
                }
                
                //Deallocate IP Address
                if (packageIpAddress != null)
                    ServerController.DeleteItemIPAddress(siteItemId, packageIpAddress.PackageAddressID);

                // update site item
                siteItem.SiteIPAddressId = 0;
                PackageController.UpdatePackageItem(siteItem);

                string parentZone = domain.ZoneName;
                if (string.IsNullOrEmpty(parentZone))
                {
                    DomainInfo parentDomain = ServerController.GetDomain(domain.DomainItemId);
                    parentZone = parentDomain.DomainName;
                }

                AddWebSitePointer(siteItemId,
                    ((domain.DomainName.Replace("." + parentZone, "") == parentZone) |
                    (domain.DomainName == parentZone))
                    ? "" : domain.DomainName.Replace("." + parentZone, "")
                    , ZoneInfo.DomainId, true, true, true);

                foreach (DomainInfo pointer in pointers)
                {
                    string pointerParentZone = pointer.ZoneName;
                    if (string.IsNullOrEmpty(pointerParentZone))
                    {
                        DomainInfo parentDomain = ServerController.GetDomain(pointer.DomainItemId);
                        pointerParentZone = parentDomain.DomainName;
                    }


                    ZoneInfo = ServerController.GetDomain(pointerParentZone);

                    AddWebSitePointer(siteItemId,
                        ((pointer.DomainName.Replace("." + pointerParentZone, "") == pointerParentZone) |
                        (pointer.DomainName == pointerParentZone))
                        ? "" : pointer.DomainName.Replace("." + pointerParentZone, "")
                        , ZoneInfo.DomainId, true, true, true);
                }

                if (oneSiteOnly)
                {
                    sites = WebServerController.GetWebSites(domain.PackageId, false);

                    // load site item
                    ip = ServerController.GetIPAddress(sites[0].SiteIPAddressId);

                    string serviceIp = (ip != null) ? ip.ExternalIP : null;

                    if (string.IsNullOrEmpty(serviceIp))
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(sites[0].ServiceId);
                        if (settings["PublicSharedIP"] != null)
                            serviceIp = settings["PublicSharedIP"].ToString();
                    }

                    ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Web, domain, serviceIp, true);
                }


                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static void FillWebServerBindings(List<ServerBinding> bindings, List<GlobalDnsRecord> dnsRecords,
            string ipAddr, string hostName, string domainName, bool ignoreGlobalDNSRecords)
        // TODO test if IPv6 works
        {
            int bindingsCount = bindings.Count;
            foreach (GlobalDnsRecord dnsRecord in dnsRecords)
            {
				if ((dnsRecord.RecordType == "A" || dnsRecord.RecordType == "AAAA" || dnsRecord.RecordType == "CNAME") &&
                    dnsRecord.RecordName != "*")
                {
                    string recordData = Utils.ReplaceStringVariable(dnsRecord.RecordName, "host_name", hostName, true);

                    if (!string.IsNullOrEmpty(domainName))
                        recordData = recordData + ((string.IsNullOrEmpty(recordData)) ? "" : ".") + domainName;
                    //otherwise full recordData is supplied by hostName
                    
                    if (ignoreGlobalDNSRecords)
                    {
                        //only look for the host_nanme record, ignore all others
                        if (dnsRecord.RecordName == "[host_name]")
                        {
                            AddBinding(bindings, new ServerBinding(ipAddr, "80", recordData));
                            break;
                        }
                    }
                    else
                    {
                        AddBinding(bindings, new ServerBinding(ipAddr, "80", recordData));
                    }
                }
            }
            
            if ((bindings.Count == bindingsCount) | (bindings.Count == 0))
            {
                AddBinding(bindings, new ServerBinding(ipAddr, "80", string.IsNullOrEmpty(hostName) ? domainName : string.IsNullOrEmpty(domainName) ? hostName : hostName + "." + domainName));
            }
        }

        private static void AddBinding(List<ServerBinding> bindings, ServerBinding binding)
        {
            foreach (ServerBinding b in bindings)
            {
                if (string.Compare(b.Host, binding.Host, true) == 0)
                    return;
            }

            bindings.Add(binding);
        }


        private static string GetWebSiteUsername(UserSettings webPolicy, string domainName)
        {
            UsernamePolicy policy = new UsernamePolicy(webPolicy["AnonymousAccountPolicy"]);

            // remove dots from the domain
            domainName = Regex.Replace(domainName, "\\W+", "", RegexOptions.Compiled);

            if (!policy.Enabled)
            {
                // default algorythm
                int maxLength = MAX_ACCOUNT_LENGTH - ANON_ACCOUNT_SUFFIX.Length - 2;
                string username = (domainName.Length > maxLength) ? domainName.Substring(0, maxLength) : domainName;
                return (username + ANON_ACCOUNT_SUFFIX); // suffix
            }
            else
            {
                // policy-enabled
                // use prefix and suffix only

                // adjust maximum length
                int maxLength = MAX_ACCOUNT_LENGTH - 2; // 2 symbols for number
                if (policy.Prefix != null)
                    maxLength -= policy.Prefix.Length;

                if (policy.Suffix != null)
                    maxLength -= policy.Suffix.Length;

                string username = (domainName.Length > maxLength) ? domainName.Substring(0, maxLength) : domainName;
                return ((policy.Prefix != null) ? policy.Prefix : "")
                    + username
                    + ((policy.Suffix != null) ? policy.Suffix : "");
            }
        }

        private static string GetWebFolder(int packageId, string pattern, string domainName)
        {
            string path = Utils.ReplaceStringVariable(pattern, "domain_name", domainName);
            return FilesController.GetFullPackagePath(packageId, path);
        }

        private static string GetFrontPageUsername(string domainName)
        {
            int maxLength = MAX_ACCOUNT_LENGTH - FRONTPAGE_ACCOUNT_SUFFIX.Length - 1;
            string username = (domainName.Length > maxLength) ? domainName.Substring(0, maxLength) : domainName;
            return (username + FRONTPAGE_ACCOUNT_SUFFIX); // suffix
        }

        public static string GetWebUsersOU(int packageId)
        {
            int webServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
            if (webServiceId > 0)
            {
                // get users OU defined on web server
                StringDictionary webSettings = ServerController.GetServiceSettings(webServiceId);
                return webSettings["ADUsersOU"];
            }
            return null;
        }
        #endregion

        #region Web Site Pointers
        public static List<DomainInfo> GetWebSitePointers(int siteItemId)
        {
            List<DomainInfo> pointers = new List<DomainInfo>();

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return pointers;

            // get the list of all domains
            List<DomainInfo> myDomains = ServerController.GetMyDomains(siteItem.PackageId);

            foreach (DomainInfo domain in myDomains)
            {
                if (domain.WebSiteId == siteItemId &&
                    String.Compare(domain.DomainName, siteItem.Name, true) != 0)
                    pointers.Add(domain);
            }

            return pointers;
        }

        public static int AddWebSitePointer(int siteItemId, string hostName, int domainId)
        {
            return AddWebSitePointer(siteItemId, hostName, domainId, true, true, false);
        }

        internal static int AddWebSitePointer(int siteItemId, string hostName, int domainId, bool updateWebSite)
        {
            return AddWebSitePointer(siteItemId, hostName, domainId, updateWebSite, false, false);
        }

        internal static int AddWebSitePointer(int siteItemId, string hostName, int domainId, bool updateWebSite, bool ignoreGlobalDNSRecords, bool rebuild)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // load domain item
            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain == null)
                return BusinessErrorCodes.ERROR_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

            // check if the web site already exists
            if (!rebuild)
            {
                if (DataProvider.CheckDomain(domain.PackageId, string.IsNullOrEmpty(hostName) ? domain.DomainName : hostName + "." + domain.DomainName, true) != 0)
                    return BusinessErrorCodes.ERROR_WEB_SITE_ALREADY_EXISTS;
            }

            // get zone records for the service
            List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(siteItem.ServiceId);

            // load web site IP address
            IPAddressInfo ip = ServerController.GetIPAddress(siteItem.SiteIPAddressId);

            // place log record
            TaskManager.StartTask("WEB_SITE", "ADD_POINTER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Domain pointer", domain.DomainName);
            TaskManager.WriteParameter("Host name", hostName);
            TaskManager.WriteParameter("updateWebSite", updateWebSite.ToString());

            try
            {

               
                // load appropriate zone
                DnsZone zone = (DnsZone)PackageController.GetPackageItem(domain.ZoneItemId);
                               

                if (zone != null)
                {
                    // change DNS zone
                    List<GlobalDnsRecord> tmpDnsRecords = new List<GlobalDnsRecord>();

                    string serviceIp = (ip != null) ? ip.ExternalIP : null;

                    if (string.IsNullOrEmpty(serviceIp))
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(siteItem.ServiceId);
                        if (settings["PublicSharedIP"] != null)
                            serviceIp = settings["PublicSharedIP"].ToString();
                    }

                    //filter initiat GlobaDNSRecords list
                    if (ignoreGlobalDNSRecords)
                    {
                        //ignore all other except the host_name record
                        foreach (GlobalDnsRecord r in dnsRecords)
                        {
                            if (r.RecordName == "[host_name]")
                            {
                                tmpDnsRecords.Add(r);
                                break;
                            }
                        }
                    }
                    else
                        tmpDnsRecords = dnsRecords;


                    List<DnsRecord> resourceRecords = DnsServerController.BuildDnsResourceRecords(tmpDnsRecords, hostName, domain.DomainName, serviceIp);

                    if (!rebuild)
                    {
                        foreach (DnsRecord r in resourceRecords)
                        {
                            if (r.RecordName != "*")
                            {
                                // check if the web site already exists
                                if (DataProvider.CheckDomain(domain.PackageId, string.IsNullOrEmpty(r.RecordName) ? domain.DomainName : r.RecordName + "." + domain.DomainName, true) != 0)
                                    return BusinessErrorCodes.ERROR_WEB_SITE_ALREADY_EXISTS;
                            }
                        }
                    }

                    try
                    {
                        DNSServer dns = new DNSServer();
                        ServiceProviderProxy.Init(dns, zone.ServiceId);

                        DnsRecord[] domainRecords = dns.GetZoneRecords(zone.Name);
                        var duplicateRecords = (from zoneRecord in domainRecords
                                                from resRecord in resourceRecords
                                                where zoneRecord.RecordName == resRecord.RecordName
                                                where zoneRecord.RecordType == resRecord.RecordType
                                                select zoneRecord).ToArray();
                        if (duplicateRecords != null && duplicateRecords.Count() > 0)
                        {
                            dns.DeleteZoneRecords(zone.Name, duplicateRecords);
                        }

                        // add new resource records
                        dns.AddZoneRecords(zone.Name, resourceRecords.ToArray());
                    }
                    catch (Exception ex1)
                    {
                        TaskManager.WriteError(ex1, "Error updating DNS records");
                    }
                }

                // update host headers
                List<ServerBinding> bindings = new List<ServerBinding>();

                // get existing web site bindings
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                    
                bindings.AddRange(web.GetSiteBindings(siteItem.SiteId));

                // check if web site has dedicated IP assigned
                bool dedicatedIp = bindings.Exists(binding => { return String.IsNullOrEmpty(binding.Host) && binding.IP != "*"; });

                // update binding only for "shared" ip addresses
                // add new host headers
                string ipAddr = "*";
                if (ip != null)
                    ipAddr = !String.IsNullOrEmpty(ip.InternalIP) ? ip.InternalIP : ip.ExternalIP;

                // fill bindings
                FillWebServerBindings(bindings, dnsRecords, ipAddr, hostName, domain.DomainName, ignoreGlobalDNSRecords);

                //for logging purposes
                foreach (ServerBinding b in bindings)
                {
                    string header = string.Format("{0} {1} {2}", b.Host, b.IP, b.Port);
                    TaskManager.WriteParameter("Add Binding", header);
                }

                // update bindings
                if (updateWebSite)
                    web.UpdateSiteBindings(siteItem.SiteId, bindings.ToArray(), false);

                // update domain
                if (!rebuild)
                {
                    domain.WebSiteId = siteItemId;
                    domain.IsDomainPointer = true;
                    foreach (ServerBinding b in bindings)
                    {
                        //add new domain record
                        if (!string.IsNullOrEmpty(b.Host))
                        {
                            domain.DomainName = b.Host;

                            DomainInfo domainTmp = ServerController.GetDomain(domain.DomainName);
                            if (!((domainTmp != null) && (domainTmp.WebSiteId == siteItemId)))
                            {
                                int domainID = ServerController.AddDomain(domain, domain.IsInstantAlias, false);
                                domainTmp = ServerController.GetDomain(domainID);
                                if (domainTmp != null)
                                {
                                    domainTmp.WebSiteId = siteItemId;
                                    domainTmp.ZoneItemId = domain.ZoneItemId;
                                    domainTmp.DomainItemId = domainId;

                                    ServerController.UpdateDomain(domainTmp);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (domain.ZoneItemId > 0)
                    {
                        DomainInfo domainTmp = ServerController.GetDomain(string.IsNullOrEmpty(hostName) ? domain.DomainName : hostName + "." + domain.DomainName, true, true);
                        if (domainTmp != null)
                        {
                            domainTmp.ZoneItemId = domain.ZoneItemId;
                            ServerController.UpdateDomain(domainTmp);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

		public static int DeleteWebSitePointer(int siteItemId, int domainId)
		{
			return DeleteWebSitePointer(siteItemId, domainId, true, true, true);
		}

        public static int DeleteWebSitePointer(int siteItemId, int domainId, bool updateWebSite, bool ignoreGlobalDNSRecords, bool deleteDomainsRecord)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // load domain item
            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain == null)
                return BusinessErrorCodes.ERROR_DOMAIN_PACKAGE_ITEM_NOT_FOUND;

            // load appropriate zone
            DnsZone zone = (DnsZone)PackageController.GetPackageItem(domain.ZoneItemId);

            // get zone records for the service
            List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(siteItem.ServiceId);

            // load web site IP address
            IPAddressInfo ip = ServerController.GetIPAddress(siteItem.SiteIPAddressId);

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_POINTER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Domain pointer", domain.DomainName);
            TaskManager.WriteParameter("updateWebSite", updateWebSite.ToString());

            try
            {
                if (zone != null)
                {
                    // change DNS zone
                    List<GlobalDnsRecord> tmpDnsRecords = new List<GlobalDnsRecord>();

                    string serviceIp = (ip != null) ? ip.ExternalIP : null;

                    if (string.IsNullOrEmpty(serviceIp))
                    {
                        StringDictionary settings = ServerController.GetServiceSettings(siteItem.ServiceId);
                        if (settings["PublicSharedIP"] != null)
                            serviceIp = settings["PublicSharedIP"].ToString();
                    }

                    if (ignoreGlobalDNSRecords)
                    {
                        foreach (GlobalDnsRecord r in dnsRecords)
                        {
                            if ((r.RecordName == "[host_name]") | ((r.RecordName + (string.IsNullOrEmpty(r.RecordName) ? domain.ZoneName : "." + domain.ZoneName)) == domain.DomainName))
                                tmpDnsRecords.Add(r);
                        }
                    }
                    else tmpDnsRecords = dnsRecords;

                    List<DnsRecord> resourceRecords = DnsServerController.BuildDnsResourceRecords(
                        tmpDnsRecords, domain.DomainName, "", serviceIp);

                    try
                    {
                        DNSServer dns = new DNSServer();
                        ServiceProviderProxy.Init(dns, zone.ServiceId);
                        dns.DeleteZoneRecords(zone.Name, resourceRecords.ToArray());
                    }
                    catch(Exception ex1)
                    {
                        TaskManager.WriteError(ex1, "Error deleting DNS records");
                    }
                }

                // get existing web site bindings
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);

                List<ServerBinding> bindings = new List<ServerBinding>();
                bindings.AddRange(web.GetSiteBindings(siteItem.SiteId));

                // check if web site has dedicated IP assigned
                bool dedicatedIp = bindings.Exists(binding => { return String.IsNullOrEmpty(binding.Host) && binding.IP != "*"; });

                // update binding only for "shared" ip addresses

                // remove host headers
                List<ServerBinding> domainBindings = new List<ServerBinding>();
                FillWebServerBindings(domainBindings, dnsRecords, "", domain.DomainName, "", ignoreGlobalDNSRecords);

                // fill to remove list
                List<string> headersToRemove = new List<string>();
                foreach (ServerBinding domainBinding in domainBindings)
                    headersToRemove.Add(domainBinding.Host);

                // remove bndings
                bindings.RemoveAll(b => { return headersToRemove.Contains(b.Host) && b.Port == "80"; } );

                // update bindings
                if (updateWebSite)
                    web.UpdateSiteBindings(siteItem.SiteId, bindings.ToArray(), true);

                // update domain
                if (deleteDomainsRecord)
                {
                    if ((domain.WebSiteId > 0) && (!domain.IsDomainPointer))
                    {
                        domain.WebSiteId = 0;
                        ServerController.UpdateDomain(domain);
                    }
                    else
                    {
                        domain.WebSiteId = 0;
                        ServerController.UpdateDomain(domain);
                        ServerController.DeleteDomain(domain.DomainId);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region Virtual Directories
        public static List<WebVirtualDirectory> GetVirtualDirectories(int siteItemId)
        {
            List<WebVirtualDirectory> dirs = new List<WebVirtualDirectory>();

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return dirs;

            // truncate home folders
            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);
            WebVirtualDirectory[] vdirs = web.GetVirtualDirectories(siteItem.SiteId);

            foreach (WebVirtualDirectory vdir in vdirs)
            {
                vdir.ContentPath = FilesController.GetVirtualPackagePath(siteItem.PackageId, vdir.ContentPath);
                dirs.Add(vdir);
            }

            return dirs;
        }

        public static WebVirtualDirectory GetVirtualDirectory(int siteItemId, string vdirName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // create directory
            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);
            WebVirtualDirectory vdir = web.GetVirtualDirectory(siteItem.SiteId, vdirName);
            // truncate home folder
            vdir.ContentPath = FilesController.GetVirtualPackagePath(siteItem.PackageId, vdir.ContentPath);

            // set name
            vdir.ParentSiteName = siteItem.Name;
            vdir.PackageId = siteItem.PackageId;
            return vdir;
        }

        public static int AddVirtualDirectory(int siteItemId, string vdirName, string vdirPath)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(siteItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("WEB_SITE", "ADD_VDIR", vdirName);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Web site", siteItem.Name);

            try
            {
                // create virtual directory
                WebVirtualDirectory dir = new WebVirtualDirectory();
                dir.Name = vdirName;
                dir.ContentPath = FilesController.GetFullPackagePath(siteItem.PackageId, vdirPath);

                dir.EnableAnonymousAccess = true;
                dir.EnableWindowsAuthentication = true;
                dir.EnableBasicAuthentication = false;

                //dir.InstalledDotNetFramework = aspNet;

                dir.DefaultDocs = null; // inherit from service
                dir.HttpRedirect = "";
                dir.HttpErrors = null;
                dir.MimeMaps = null;

                // create directory
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                if (web.VirtualDirectoryExists(siteItem.SiteId, vdirName))
                    return BusinessErrorCodes.ERROR_VDIR_ALREADY_EXISTS;

                web.CreateVirtualDirectory(siteItem.SiteId, dir);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int UpdateVirtualDirectory(int siteItemId, WebVirtualDirectory vdir)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_VDIR", vdir.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Web site", siteItem.Name);

            try
            {
                // normalize path
                vdir.ContentPath = FilesController.GetFullPackagePath(siteItem.PackageId, vdir.ContentPath);

                // create directory
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                web.UpdateVirtualDirectory(siteItem.SiteId, vdir);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteVirtualDirectory(int siteItemId, string vdirName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_VDIR", vdirName);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Web site", siteItem.Name);

            try
            {
                // create directory
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                web.DeleteVirtualDirectory(siteItem.SiteId, vdirName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region FrontPage
        public static int InstallFrontPage(int siteItemId, string username, string password)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(siteItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("WEB_SITE", "INSTALL_FP", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("FrontPage username", username);

            try
            {

                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);

				// load original site
				//WebSite origSite = GetWebSite(siteItemId);

				// install FP
                bool success = web.InstallFrontPage(siteItem.SiteId, username, password);
                if (!success)
                {
                    TaskManager.WriteWarning("Account exists");
                    return BusinessErrorCodes.ERROR_FP_ACCOUNT_EXISTS;
                }

                // update site with FP account
                siteItem.FrontPageAccount = username;
                siteItem.FrontPagePassword = CryptoUtils.Encrypt(password);
                PackageController.UpdatePackageItem(siteItem);

				// restore original site
				//UpdateWebSite(origSite);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int UninstallFrontPage(int siteItemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UNINSTALL_FP", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("FrontPage username", siteItem.FrontPageAccount);

            try
            {
                if (String.IsNullOrEmpty(siteItem.FrontPageAccount))
                    siteItem.FrontPageAccount = GetFrontPageUsername(siteItem.Name);

                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                web.UninstallFrontPage(siteItem.SiteId, siteItem.FrontPageAccount);

                // update site with FP account
                siteItem.FrontPageAccount = "";
                PackageController.UpdatePackageItem(siteItem);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int ChangeFrontPagePassword(int siteItemId, string password)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "CHANGE_FP_PASSWORD", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("FrontPage username", siteItem.FrontPageAccount);

            try
            {
                if (String.IsNullOrEmpty(siteItem.FrontPageAccount))
                    siteItem.FrontPageAccount = GetFrontPageUsername(siteItem.Name);

                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                web.ChangeFrontPagePassword(siteItem.FrontPageAccount, password);

                // update site with FP account
                siteItem.FrontPagePassword = CryptoUtils.Encrypt(password);
                PackageController.UpdatePackageItem(siteItem);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region Secured Folders
        public static int InstallSecuredFolders(int siteItemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "INSTALL_SECURED_FOLDERS", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            try
            {

                // install folders
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.InstallSecuredFolders(siteItem.SiteId);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int UninstallSecuredFolders(int siteItemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UNINSTALL_SECURED_FOLDERS", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            try
            {
                // install folders
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.UninstallSecuredFolders(siteItem.SiteId);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static WebFolder[] GetFolders(int siteItemId)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get folders
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetFolders(siteItem.SiteId);
        }

        public static WebFolder GetFolder(int siteItemId, string folderPath)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get folder
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetFolder(siteItem.SiteId, folderPath);
        }

        public static int UpdateFolder(int siteItemId, WebFolder folder)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            folder.Path = FilesController.CorrectRelativePath(folder.Path);

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_SECURED_FOLDER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Folder", folder.Path);

            try
            {
                // update folder
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.UpdateFolder(siteItem.SiteId, folder);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteFolder(int siteItemId, string folderPath)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_SECURED_FOLDER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Folder", folderPath);

            try
            {
                // delete folder
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.DeleteFolder(siteItem.SiteId, folderPath);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region Secured Users
        public static WebUser[] GetUsers(int siteItemId)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get users
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetUsers(siteItem.SiteId);
        }

        public static WebUser GetUser(int siteItemId, string userName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get user
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetUser(siteItem.SiteId, userName);
        }

        public static int UpdateUser(int siteItemId, WebUser user)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_SECURED_USER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("User", user.Name);

            try
            {
                // update user
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.UpdateUser(siteItem.SiteId, user);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteUser(int siteItemId, string userName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_SECURED_USER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("User", userName);

            try
            {
                // delete user
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.DeleteUser(siteItem.SiteId, userName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region Secured Groups
        public static WebGroup[] GetGroups(int siteItemId)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get groups
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetGroups(siteItem.SiteId);
        }

        public static WebGroup GetGroup(int siteItemId, string groupName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get group
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetGroup(siteItem.SiteId, groupName);
        }

        public static int UpdateGroup(int siteItemId, WebGroup group)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_SECURED_GROUP", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Group", group.Name);

            try
            {
                // update group
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.UpdateGroup(siteItem.SiteId, group);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteGroup(int siteItemId, string groupName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_SECURED_GROUP", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Group", groupName);

            try
            {
                // delete group
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.DeleteGroup(siteItem.SiteId, groupName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region Shared SSL Folders
        public static List<string> GetSharedSSLDomains(int packageId)
        {
            List<string> domains = new List<string>();

            PackageSettings settings = PackageController.GetPackageSettings(packageId, PackageSettings.SHARED_SSL_SITES);
            if (settings != null && !String.IsNullOrEmpty(settings["SharedSslSites"]))
                domains.AddRange(settings["SharedSslSites"].Split(';'));

            return domains;
        }

        public static DataSet GetRawSSLFoldersPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, typeof(SharedSSLFolder),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<SharedSSLFolder> GetSharedSSLFolders(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(SharedSSLFolder), recursive);

            return items.ConvertAll<SharedSSLFolder>(
                new Converter<ServiceProviderItem, SharedSSLFolder>(ConvertItemToSharedSSLFolder));
        }

        private static SharedSSLFolder ConvertItemToSharedSSLFolder(ServiceProviderItem item)
        {
            return (SharedSSLFolder)item;
        }

        public static SharedSSLFolder GetSharedSSLFolder(int itemId)
        {
            // load original item
            SharedSSLFolder vdirItem = (SharedSSLFolder)PackageController.GetPackageItem(itemId);
            if (vdirItem == null)
                return null;

            // load shared SSL site
            int idx = vdirItem.Name.LastIndexOf("/");
            string domainName = vdirItem.Name.Substring(0, idx);
            string vdirName = vdirItem.Name.Substring(idx + 1);

            WebServer web = GetWebServer(vdirItem.ServiceId);
            string siteId = web.GetSiteId(domainName);
            if (siteId == null)
                return null;

            // get virtual directory
            WebVirtualDirectory vdir = web.GetVirtualDirectory(siteId, vdirName);
            if (vdir != null)
            {
                vdir.ContentPath = FilesController.GetVirtualPackagePath(vdirItem.PackageId, vdir.ContentPath);
            }
            vdir.Id = itemId;
            vdir.Name = vdirItem.Name;
            vdir.PackageId = vdirItem.PackageId;
            return ObjectUtils.ConvertObject<WebVirtualDirectory, SharedSSLFolder>(vdir);
        }

        public static int AddSharedSSLFolder(int packageId, string sslDomain, int webSiteId, string vdirName, string vdirPath)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo sitesQuota = PackageController.GetPackageQuota(packageId, Quotas.WEB_SHAREDSSL);
            if (sitesQuota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_WEB_SHARED_SSL_QUOTA_LIMIT;

            //get site
            WebSite site = GetWebSite(webSiteId);
            bool updateRequired = (site != null) && (site.Name.Equals(sslDomain, StringComparison.InvariantCulture) != true);

            // place log record
            TaskManager.StartTask("WEB_SITE", "ADD_SSL_FOLDER", sslDomain);
            TaskManager.WriteParameter("Directory name", vdirName);
            TaskManager.WriteParameter("Path", vdirPath);

            try
            {
                // create virtual directory
                SharedSSLFolder dir = new SharedSSLFolder();
                dir.Name = vdirName;
                dir.ContentPath = FilesController.GetFullPackagePath(packageId, vdirPath);

                dir.EnableAnonymousAccess = true;
                dir.EnableWindowsAuthentication = true;
                dir.EnableBasicAuthentication = false;

                //dir.InstalledDotNetFramework = aspNet;

                dir.DefaultDocs = null; // inherit from service
                dir.HttpRedirect = "";
                dir.HttpErrors = null;
                dir.MimeMaps = null;

                // create directory
                int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
                WebServer web = GetWebServer(serviceId);

                // get site id
                string siteId = web.GetSiteId(sslDomain);
                if (siteId == null)
                    return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

                // check if vdir exists
                if (web.VirtualDirectoryExists(siteId, vdirName))
                    return BusinessErrorCodes.ERROR_VDIR_ALREADY_EXISTS;

                if (updateRequired)
                {
                    dir.ApplicationPool = site.ApplicationPool;
                    dir.AnonymousUsername = site.AnonymousUsername;
                    dir.AnonymousUserPassword = site.AnonymousUserPassword;
                    dir.DedicatedApplicationPool = site.DedicatedApplicationPool;
					dir.AspInstalled = site.AspInstalled;
					dir.AspNetInstalled = site.AspNetInstalled;
					dir.CgiBinInstalled = site.CgiBinInstalled;
                }

                // create vdir
                WebVirtualDirectory virtDir = ObjectUtils.ConvertObject<SharedSSLFolder, WebVirtualDirectory>(dir);
                web.CreateVirtualDirectory(siteId, virtDir);

                //anonymous user and applcation pool
                if (updateRequired)
                {
                    web.UpdateVirtualDirectory(siteId, virtDir);
                }                

                // save item
                dir.Name = sslDomain + "/" + vdirName;
                dir.PackageId = packageId;
                dir.ServiceId = serviceId;
                TaskManager.ItemId = PackageController.AddPackageItem(dir);

                return TaskManager.ItemId;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int UpdateSharedSSLFolder(SharedSSLFolder vdir)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original item
            SharedSSLFolder origVdir = (SharedSSLFolder)PackageController.GetPackageItem(vdir.Id);
            if (origVdir == null)
                return 0;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_SSL_FOLDER", origVdir.Name);
            TaskManager.ItemId = vdir.Id;

            try
            {
                int idx = origVdir.Name.LastIndexOf("/");
                string domainName = origVdir.Name.Substring(0, idx);
                string vdirName = origVdir.Name.Substring(idx + 1);

                WebServer web = GetWebServer(origVdir.ServiceId);
                string siteId = web.GetSiteId(domainName);
                if (siteId == null)
                    return 0;

                // normalize path
                vdir.ContentPath = FilesController.GetFullPackagePath(origVdir.PackageId, vdir.ContentPath);
                vdir.Name = vdirName;

                // copy object
                WebVirtualDirectory virtDir = ObjectUtils.ConvertObject<SharedSSLFolder, WebVirtualDirectory>(vdir);

                // update directory
                web.UpdateVirtualDirectory(siteId, virtDir);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteSharedSSLFolder(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original item
            SharedSSLFolder origVdir = (SharedSSLFolder)PackageController.GetPackageItem(itemId);
            if (origVdir == null)
                return 0;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_SSL_FOLDER", origVdir.Name);
            TaskManager.ItemId = itemId;

            try
            {
                int idx = origVdir.Name.LastIndexOf("/");
                string domainName = origVdir.Name.Substring(0, idx);
                string vdirName = origVdir.Name.Substring(idx + 1);

                WebServer web = GetWebServer(origVdir.ServiceId);
                string siteId = web.GetSiteId(domainName);
                if (siteId == null)
                    return 0;

                // delete directory
                web.DeleteVirtualDirectory(siteId, vdirName);

                // delete item
                PackageController.DeletePackageItem(itemId);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

		#region Web Deploy Publishing Access

		public static ResultObject SaveWebDeployPublishingProfile(int siteItemId, int[] serviceItemIds)
		{
			ResultObject result = new ResultObject { IsSuccess = true };

			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "SaveWebDeployPublishingProfile");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				// load site item
				var item = (WebSite)PackageController.GetPackageItem(siteItemId);

				//
				if (item == null)
				{
					TaskManager.WriteError("Web site not found");
					//
					result.AddError("WEBSITE_NOT_FOUND", null);
					result.IsSuccess = false;
					return result;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");
					//
					result.AddError("DEMO_USER", null);
					result.IsSuccess = false;
					return result;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");
					//
					result.AddError("NOT_ALLOWED", null);
					result.IsSuccess = false;
					return result;
				}

				// Ensure all service items specified are within the same hosting space
				// This is a logcally correct and secure statement than the other one
				// TO-DO: Uncomment this line before demo!
				//var profileIntegritySucceeded = false;

				var profileIntegritySucceeded = true;
				//
				foreach (int itemId in serviceItemIds)
				{
					try
					{
						//
						PackageController.GetPackageItem(itemId);
						//
						profileIntegritySucceeded = true;
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex);
						//
						profileIntegritySucceeded = false;
						// Exit from the loop
						break;
					}
				}

				//
				if (profileIntegritySucceeded == true)
				{
					// Build service items list
					item.WebDeploySitePublishingProfile = String.Join(",", Array.ConvertAll<int, string>(serviceItemIds, (int x) => { return x.ToString(); }));
					// Put changes in effect
					PackageController.UpdatePackageItem(item);
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				result.IsSuccess = false;
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static ResultObject GrantWebDeployPublishingAccess(int siteItemId, string accountName, string accountPassword)
		{
			ResultObject result = new ResultObject { IsSuccess = true };

			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "GrantWeDeployPublishingAccess");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				// load site item
				var item = (WebSite)PackageController.GetPackageItem(siteItemId);

				//
				if (item == null)
				{
					TaskManager.WriteError("Web site not found");
					//
					result.AddError("WEBSITE_NOT_FOUND", null);
					result.IsSuccess = false;
					return result;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");
					//
					result.AddError("DEMO_USER", null);
					result.IsSuccess = false;
					return result;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");
					//
					result.AddError("NOT_ALLOWED", null);
					result.IsSuccess = false;
					return result;
				}

				//
				WebServer server = GetWebServer(item.ServiceId);

                StringDictionary webSettings = ServerController.GetServiceSettings(item.ServiceId);
                if (!String.IsNullOrEmpty(webSettings["WmSvc.NETBIOS"]))
                {
                    accountName = webSettings["WmSvc.NETBIOS"].ToString() + "\\" + accountName;
                }

				// Most part of the functionality used to enable Web Deploy publishing correspond to those created for Web Management purposes,
				// so we can re-use the existing functionality to deliver seamless development experience.
				if (server.CheckWebManagementAccountExists(accountName))
				{
					TaskManager.WriteWarning("Account name specified already exists");
					//
					result.AddError("ACCOUNTNAME_PROHIBITED", null);
					result.IsSuccess = false;
					return result;
				}

				// Most part of the functionality used to enable Web Deploy publishing correspond to those created for Web Management purposes,
				// so we can re-use the existing functionality to deliver seamless development experience.
				ResultObject passwResult = server.CheckWebManagementPasswordComplexity(accountPassword);
				if (!passwResult.IsSuccess)
				{
					TaskManager.WriteWarning("Account password does not meet complexity requirements");
					//
					result.ErrorCodes.AddRange(passwResult.ErrorCodes);
					result.IsSuccess = false;
					//
					return result;
				}

				// Execute a call to remote server to enable Web Deploy publishing access for the specified user account
				server.GrantWebDeployPublishingAccess(item.SiteId, accountName, accountPassword);
				// Enable Web Deploy flag for the web site
				item.WebDeploySitePublishingEnabled = true;
				// Remember Web Deploy publishing account
				item.WebDeployPublishingAccount = accountName;
				// Remember Web Deploy publishing password
				item.WebDeployPublishingPassword = CryptoUtils.Encrypt(accountPassword);
				// Put changes in effect
				PackageController.UpdatePackageItem(item);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				result.IsSuccess = false;
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static void RevokeWebDeployPublishingAccess(int siteItemId)
		{
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "RevokeWebDeployPublishingAccess");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				// load site item
				var item = (WebSite)PackageController.GetPackageItem(siteItemId);

				//
				if (item == null)
				{
					TaskManager.WriteWarning("Web site not found");
					return;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");
					return;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");
					return;
				}

				//
				string accountName = item.WebDeployPublishingAccount;
				//
				if (String.IsNullOrEmpty(accountName))
				{
					TaskManager.WriteWarning("Web Deploy Publishing Access account name is either not set or empty");
					return;
				}

				//
				WebServer server = GetWebServer(item.ServiceId);
				// Revoke 
				server.RevokeWebDeployPublishingAccess(item.SiteId, accountName);
				// Cleanup web site properties
				item.WebDeployPublishingAccount = String.Empty;
				item.WebDeploySitePublishingEnabled = false;
				item.WebDeploySitePublishingProfile = String.Empty;
				item.WebDeployPublishingPassword = String.Empty;
				// Put changes into effect
				PackageController.UpdatePackageItem(item);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static ResultObject ChangeWebDeployPublishingPassword(int siteItemId, string newAccountPassword)
		{
			ResultObject result = new ResultObject { IsSuccess = true };
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "ChangeWebDeployPublishingPassword");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				//
				var item = (WebSite)PackageController.GetPackageItem(siteItemId);

				//
				if (item == null)
				{
					TaskManager.WriteWarning("Web site not found");

					//
					result.AddError("WEBSITE_NOT_FOUND", null);
					result.IsSuccess = false;

					return result;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");

					result.AddError("DEMO_USER", null);
					result.IsSuccess = false;

					return result;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");

					//
					result.AddError("NOT_ALLOWED", null);
					result.IsSuccess = false;

					return result;
				}

				// Revoke access first
				RevokeWebDeployPublishingAccess(siteItemId);
				//
				result = GrantWebDeployPublishingAccess(siteItemId, item.WebDeployPublishingAccount, newAccountPassword);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static BytesResult GetWebDeployPublishingProfile(int siteItemId)
		{
			var result = new BytesResult();
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "GetWebDeployPublishingProfile");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				// load site item
				WebSite item = (WebSite)PackageController.GetPackageItem(siteItemId);
				//
				var siteItem = GetWebSite(siteItemId);

				//
				if (item == null)
				{
					TaskManager.WriteWarning("Web site not found");
					return result;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");
					return result;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");
					return result;
				}

				//
				string accountName = item.WebDeployPublishingAccount;
				//
				if (String.IsNullOrEmpty(accountName))
				{
					TaskManager.WriteWarning("Web Deploy Publishing Access account name is either not set or empty");
					return result;
				}
				//
				var packageInfo = PackageController.GetPackage(item.PackageId);
				//
				var userInfo = UserController.GetUser(packageInfo.UserId);
				//
				var items = new Hashtable()
				{
					{ "WebSite", siteItem },
					{ "User",  userInfo },
				};

				// Retrieve service item ids from the profile
				var serviceItemIds = Array.ConvertAll<string, int>(
					item.WebDeploySitePublishingProfile.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
					(string x) => { return Convert.ToInt32(x.Trim()); });

				//
				foreach (var serviceItemId in serviceItemIds)
				{
					var packageItem = PackageController.GetPackageItem(serviceItemId);
					// Handle SQL databases
					if (packageItem is SqlDatabase)
					{
						var dbItemKey = packageItem.GroupName.StartsWith("MsSQL") ? "MsSqlDatabase" : "MySqlDatabase";
						var dbServerKeyExt = packageItem.GroupName.StartsWith("MsSQL") ? "MsSqlServerExternalAddress" : "MySqlServerExternalAddress";
						var dbServerKeyInt = packageItem.GroupName.StartsWith("MsSQL") ? "MsSqlServerInternalAddress" : "MySqlServerInternalAddress";
						//
						items.Add(dbItemKey, DatabaseServerController.GetSqlDatabase(serviceItemId));
						// Retrieve settings
						var sqlSettings = ServerController.GetServiceSettings(packageItem.ServiceId);
						items[dbServerKeyExt] = sqlSettings["ExternalAddress"];
						items[dbServerKeyInt] = sqlSettings["InternalAddress"];
					}
					else if (packageItem is SqlUser)
					{
						var itemKey = packageItem.GroupName.StartsWith("MsSQL") ? "MsSqlUser" : "MySqlUser";
						//
						items.Add(itemKey, DatabaseServerController.GetSqlUser(serviceItemId));
					}
					else if (packageItem is FtpAccount)
					{
						//
						items.Add("FtpAccount", FtpServerController.GetFtpAccount(serviceItemId));
						// Get FTP DNS records
						List<GlobalDnsRecord> ftpRecords = ServerController.GetDnsRecordsByService(packageItem.ServiceId);
						if (ftpRecords.Count > 0)
						{
							GlobalDnsRecord ftpRecord = ftpRecords[0];
							string ftpIp = ftpRecord.ExternalIP;
							if (String.IsNullOrEmpty(ftpIp))
								ftpIp = ftpRecord.RecordData;
							// Assign FTP service address variable
							items["FtpServiceAddress"] = ftpIp;
						}
					}
				}

				// Decrypt publishing password & copy related settings
				siteItem.WebDeployPublishingPassword = CryptoUtils.Decrypt(item.WebDeployPublishingPassword);
				siteItem.WebDeployPublishingAccount = item.WebDeployPublishingAccount;
				siteItem.WebDeploySitePublishingEnabled = item.WebDeploySitePublishingEnabled;
				siteItem.WebDeploySitePublishingProfile = item.WebDeploySitePublishingProfile;

				// Retrieve publishing profile template from the Web Policy settings
				var webPolicy = UserController.GetUserSettings(packageInfo.UserId, UserSettings.WEB_POLICY);
				// Instantiate template, it's content and related items and then evaluate it
				var template = new Template(webPolicy["PublishingProfile"]);
				// Receive bytes for the evaluated template
				result.Value = Encoding.UTF8.GetBytes(template.Evaluate(items));
				//
				result.IsSuccess = true;
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				result.IsSuccess = false;
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		#endregion

        #region Helicon Ape
        public static int EnableHeliconApe(int siteItemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "ENABLE_HELICON_APE", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            try
            {

                // install folders
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.EnableHeliconApe(siteItem.SiteId);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

        }

        public static int DisableHeliconApe(int siteItemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DISABLE_HELICON_APE", siteItem.Name);
            TaskManager.ItemId = siteItemId;

            try
            {
                // install folders
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.DisableHeliconApe(siteItem.SiteId);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        /// <summary>
        /// Enables Helicon Ape on the server globally. Requires Administrator role to execute.
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        public static int EnableHeliconApeGlobally(int ServiceId)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive | DemandAccount.NotDemo);
            if (accountCheck < 0)
            {
                return accountCheck;
            }

            WebServer web = GetWebServer(ServiceId);
            web.EnableHeliconApe("");

            return 0;
        }


        /// <summary>
        /// Disables Helicon Ape on the server globally. Requires Administrator role to execute.
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        public static int DisableHeliconApeGlobally(int ServiceId)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive | DemandAccount.NotDemo);
            //
            if (accountCheck < 0)
            {
                return accountCheck;
            }

            WebServer web = GetWebServer(ServiceId);
            web.DisableHeliconApe("");

            return 0;
        }


        /// <summary>
		/// Retrieves Helicon Ape extension status from the server. Requires Administrator role to execute.
		/// </summary>
		/// <param name="ServiceId"></param>
		/// <returns></returns>
        public static HeliconApeStatus GetHeliconApeStatus(int ServiceId)
        {
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive | DemandAccount.NotDemo);
			//
			if (accountCheck < 0)
			{
				return HeliconApeStatus.Empty;
			}

            WebServer web = GetWebServer(ServiceId);

            return web.GetHeliconApeStatus("");
        }

		/// <summary>
		/// Installs Helicon Ape on the target server. Requires Administrator role to execute.
		/// </summary>
		/// <param name="ServiceId"></param>
		/// <returns></returns>
        public static int InstallHeliconApe(int ServiceId)
        {
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive | DemandAccount.NotDemo);
			//
			if (accountCheck < 0)
			{
				return accountCheck;
			}
			//
            WebServer web = GetWebServer(ServiceId);
			//
            web.InstallHeliconApe(String.Empty);
			//
			return 0;
        }

        public static HtaccessFolder[] GetHeliconApeFolders(int siteItemId)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get folders
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetHeliconApeFolders(siteItem.SiteId);
        }

        public static HtaccessFolder GetHeliconApeHttpdFolder(int serviceId)
        {
            // get folder
            WebServer web = GetWebServer(serviceId);
            return web.GetHeliconApeHttpdFolder();
        }

        public static HtaccessFolder GetHeliconApeFolder(int siteItemId, string folderPath)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get folder
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetHeliconApeFolder(siteItem.SiteId, folderPath);
        }

        public static int UpdateHeliconApeFolder(int siteItemId, HtaccessFolder folder)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            folder.Path = FilesController.CorrectRelativePath(folder.Path);

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_HELICON_APE_FOLDER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Folder", folder.Path);

            try
            {
                // update folder
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.UpdateHeliconApeFolder(siteItem.SiteId, folder);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int UpdateHeliconApeHttpdFolder(int serviceId, HtaccessFolder folder)
        {
            folder.Path = FilesController.CorrectRelativePath(folder.Path);

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_HELICON_APE_HTTPD_FOLDER");
            TaskManager.WriteParameter("Folder", folder.Path);

            try
            {
                // update folder
                WebServer web = GetWebServer(serviceId);
                web.UpdateHeliconApeHttpdFolder(folder);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteHeliconApeFolder(int siteItemId, string folderPath)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_HELICON_APE_FOLDER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Folder", folderPath);

            try
            {
                // delete folder
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.DeleteHeliconApeFolder(siteItem.SiteId, folderPath);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        #endregion

        #region Helicon Ape Users
        public static HtaccessUser[] GetHeliconApeUsers(int siteItemId)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get users
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetHeliconApeUsers(siteItem.SiteId);
        }

        public static HtaccessUser GetHeliconApeUser(int siteItemId, string userName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get user
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetHeliconApeUser(siteItem.SiteId, userName);
        }

        public static int UpdateHeliconApeUser(int siteItemId, HtaccessUser user)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_HELICON_APE_USER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("User", user.Name);

            try
            {
                // update user
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.UpdateHeliconApeUser(siteItem.SiteId, user);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteHeliconApeUser(int siteItemId, string userName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_HELICON_APE_USER", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("User", userName);

            try
            {
                // delete user
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.DeleteHeliconApeUser(siteItem.SiteId, userName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region Helicon Ape Groups
        public static WebGroup[] GetHeliconApeGroups(int siteItemId)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get groups
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetHeliconApeGroups(siteItem.SiteId);
        }

        public static WebGroup GetHeliconApeGroup(int siteItemId, string groupName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return null;

            // get group
            WebServer web = GetWebServer(siteItem.ServiceId);
            return web.GetHeliconApeGroup(siteItem.SiteId, groupName);
        }

        public static int UpdateHeliconApeGroup(int siteItemId, WebGroup group)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "UPDATE_HELICON_APE_GROUP", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Group", group.Name);

            try
            {
                // update group
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.UpdateHeliconApeGroup(siteItem.SiteId, group);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeleteHeliconApeGroup(int siteItemId, string groupName)
        {
            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return BusinessErrorCodes.ERROR_WEB_SITE_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("WEB_SITE", "DELETE_HELICON_APE_GROUP", siteItem.Name);
            TaskManager.ItemId = siteItemId;
            TaskManager.WriteParameter("Group", groupName);

            try
            {
                // delete group
                WebServer web = GetWebServer(siteItem.ServiceId);
                web.DeleteHeliconApeGroup(siteItem.SiteId, groupName);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion

        #region Helicon Zoo

        public static List<WebVirtualDirectory> GetZooApplications(int siteItemId)
        {
            List<WebVirtualDirectory> dirs = new List<WebVirtualDirectory>();

            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return dirs;

            // truncate home folders
            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);
            WebVirtualDirectory[] vdirs = web.GetZooApplications(siteItem.SiteId);

            foreach (WebVirtualDirectory vdir in vdirs)
            {
                vdir.ContentPath = FilesController.GetVirtualPackagePath(siteItem.PackageId, vdir.ContentPath);
                dirs.Add(vdir);
            }

            return dirs;
        }

        public static StringResultObject SetZooEnvironmentVariable(int siteItemId, string appName, string envName, string envValue)
        {
            StringResultObject result = new StringResultObject {IsSuccess = false};


            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return result;


            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);
            return web.SetZooEnvironmentVariable(siteItem.SiteId, appName, envName, envValue);
        }

        public static StringResultObject SetZooConsoleEnabled(int siteItemId, string appName)
        {
            StringResultObject result = new StringResultObject { IsSuccess = false };


            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return result;


            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);
            return web.SetZooConsoleEnabled(siteItem.SiteId, appName);
        }


        public static StringResultObject SetZooConsoleDisabled(int siteItemId, string appName)
        {
            StringResultObject result = new StringResultObject { IsSuccess = false };


            // load site item
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteItemId);
            if (siteItem == null)
                return result;


            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);
            return web.SetZooConsoleDisabled(siteItem.SiteId, appName);
        }

        #endregion

        #region WebManagement Access

		public static ResultObject GrantWebManagementAccess(int siteItemId, string accountName, string accountPassword)
		{
			ResultObject result = new ResultObject { IsSuccess = true };

			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "GrantWebManagementAccess");
				TaskManager.WriteParameter("SiteItemId", siteItemId);
				
				//
				WebSite item = GetWebSite(siteItemId);
				
				//
				if (item == null)
				{
					TaskManager.WriteError("Web site not found");
					//
					result.AddError("WEBSITE_NOT_FOUND", null);
					result.IsSuccess = false;
					return result;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");
					//
					result.AddError("DEMO_USER", null);
					result.IsSuccess = false;
					return result;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");
					//
					result.AddError("NOT_ALLOWED", null);
					result.IsSuccess = false;
					return result;
				}
				
				//
				WebServer server = GetWebServer(item.ServiceId);

                StringDictionary webSettings = ServerController.GetServiceSettings(item.ServiceId);
                if (!String.IsNullOrEmpty(webSettings["WmSvc.NETBIOS"]))
                {
                    accountName = webSettings["WmSvc.NETBIOS"].ToString() + "\\" + accountName;
                }

				//
				if (server.CheckWebManagementAccountExists(accountName))
				{
					TaskManager.WriteWarning("Account name specified already exists");
					//
					result.AddError("ACCOUNTNAME_PROHIBITED", null);
					result.IsSuccess = false;
					return result;
				}

				//
				ResultObject passwResult = server.CheckWebManagementPasswordComplexity(accountPassword);
				if (!passwResult.IsSuccess)
				{
					TaskManager.WriteWarning("Account password does not meet complexity requirements");
					//
					result.ErrorCodes.AddRange(passwResult.ErrorCodes);
					result.IsSuccess = false;
					//
					return result;
				}
				
				//
				server.GrantWebManagementAccess(item.SiteId, accountName, accountPassword);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static void RevokeWebManagementAccess(int siteItemId)
		{
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "RevokeWebManagementAccess");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				//
				WebSite item = GetWebSite(siteItemId);
				
				//
				if (item == null)
				{
					TaskManager.WriteWarning("Web site not found");
					return;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");
					return;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");
					return;
				}

				//
				string accountName = item.GetValue<string>(WebSite.WmSvcAccountName);
				//
				if (String.IsNullOrEmpty(accountName))
				{
					TaskManager.WriteWarning("WebManagement Access account name is either not set or empty");
					return;
				}

				//
				WebServer server = GetWebServer(item.ServiceId);
				//
				server.RevokeWebManagementAccess(item.SiteId, accountName);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static ResultObject ChangeWebManagementAccessPassword(int siteItemId, string accountPassword)
		{
			ResultObject result = new ResultObject { IsSuccess = true };
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "ChangeWebManagementAccessPassword");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				//
				WebSite item = GetWebSite(siteItemId) as WebSite;

				//
				if (item == null)
				{
					TaskManager.WriteWarning("Web site not found");

					//
					result.AddError("WEBSITE_NOT_FOUND", null);
					result.IsSuccess = false;
					
					return result;
				}

				//
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either demo or inactive");

					result.AddError("DEMO_USER", null);
					result.IsSuccess = false;

					return result;
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteWarning("Current user is either not allowed to access the package or the package inactive");

					//
					result.AddError("NOT_ALLOWED", null);
					result.IsSuccess = false;

					return result;
				}

				//
				string accountName = item.GetValue<string>(WebSite.WmSvcAccountName);
				//
				if (String.IsNullOrEmpty(accountName))
				{
					TaskManager.WriteWarning("WebManagement Access account name is either not set or empty");

					result.AddError("EMPTY_WMSVC_ACCOUNT", null);
					result.IsSuccess = false;

					return result;
				}

				//
				WebServer server = GetWebServer(item.ServiceId);
				
				//
				ResultObject passwResult = server.CheckWebManagementPasswordComplexity(accountPassword);
				if (!passwResult.IsSuccess)
				{
					TaskManager.WriteWarning("Account password does not meet complexity requirements");
					//
					result.ErrorCodes.AddRange(passwResult.ErrorCodes);
					result.IsSuccess = false;
					//
					return result;
				}
				//
				server.ChangeWebManagementAccessPassword(accountName, accountPassword);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}
		
		#endregion

        #region IImportController Members

        public List<string> GetImportableItems(int packageId, int itemTypeId, Type itemType, ResourceGroupInfo group)
        {
            List<string> items = new List<string>();

            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return items;

            WebServer web = GetWebServer(serviceId);
            if (itemType == typeof(WebSite))
                items.AddRange(web.GetSites());

            return items;
        }

        public void ImportItem(int packageId, int itemTypeId, Type itemType,
			ResourceGroupInfo group, string itemName)
        {
			// Controller supports web sites only
			if (itemType != typeof(WebSite))
				return;
            // Get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return;
            //
            WebServer web = GetWebServer(serviceId);
			// Obtaining web site unique id
			string siteId = web.GetSiteId(itemName);
			if (siteId == null)
				return;
            // Obtaining OperatingSystem service provider id within the context of package.
            int osServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Os);
            if (osServiceId == 0)
                return;
			//
            OS.OperatingSystem os = new OS.OperatingSystem();
            ServiceProviderProxy.Init(os, osServiceId);

            // get site info
            WebSite site = web.GetSite(siteId);
			PackageIPAddress ipMatch = default(PackageIPAddress);

			#region Resolving web site ip
			// Loading package context to evaluate IP Addresses quota
			PackageContext packageCtx = PackageController.GetPackageContext(packageId);

            // We are unable to step further because there are 
            // no bindings on the web site to choose from
            if (site.Bindings == null || site.Bindings.Length == 0)
            {
                TaskManager.WriteError("Could not import the web site because it has no bindings assigned.");
                return;
            }

			// Loading service provider settings
			StringDictionary webSettings = ServerController.GetServiceSettings(serviceId);
			int sharedIpId = Utils.ParseInt(webSettings["SharedIP"], 0);
			IPAddressInfo sharedIp = ServerController.GetIPAddress(sharedIpId);

			// Trying to match site's bindings to against either 
			// external or internal address of the shared ip
			bool sharedIpMatch = Array.Exists(site.Bindings, 
				x => sharedIp != null && (x.IP.Equals(sharedIp.ExternalIP) || x.IP.Equals(sharedIp.InternalIP)));

			// Quering dedicated ips package quota allotted
			bool dedicatedIpsAllotted = Array.Exists(packageCtx.QuotasArray,
				x => x.QuotaName == Quotas.WEB_IP_ADDRESSES && x.QuotaAllocatedValue != 0);

			// By default we fallback to the service provider's shared ip,
			// so the web site being imported is "hooked up" to the proper ip,
			// even if current bindings match different ip for some reason
			if (!dedicatedIpsAllotted || sharedIpMatch)
			{
				site.SiteIPAddressId = sharedIpId;
			}

			// Trying to find a match in dedicated ips list if any.
			if (dedicatedIpsAllotted && !sharedIpMatch)
			{
				// Obtaining first binding with non-empty ip
				ServerBinding binding = Array.Find<ServerBinding>(site.Bindings, x => !String.IsNullOrEmpty(x.IP));
				// No bindings were found - throw an exception
				if (binding == null)
				{
					TaskManager.WriteError(@"Could not import the web site because IP address field of all of its bindings is empty. 
Please ensure the web site has been assigned bindings in the appropriate format and then try to run import procedure once again.");
					return;
				}
				//
				ServiceInfo webService = ServerController.GetServiceInfo(serviceId);
				// Loading ip addresses from Web Sites address pool related to the package
				List<PackageIPAddress> ips = ServerController.GetPackageUnassignedIPAddresses(packageId, IPAddressPool.WebSites);
				// Looking for an entry matching by package and external/internal ip
				ipMatch = Array.Find<PackageIPAddress>(ips.ToArray(),
					x => x.ExternalIP == binding.IP || x.InternalIP == binding.IP);
				// No match has been found - we are in a fault state
				if (ipMatch == null)
				{
					TaskManager.WriteError(@"Could not import the web site because no dedicated IP address match in the target space has been found. 
Please ensure the space has been allocated {0} IP address as a dedicated one and it is free. Then try to run import procedure once again.", binding.IP);
					return;
				}
				//
				site.SiteIPAddressId = ipMatch.AddressID;
			}
			#endregion

            // folders
            UserInfo user = PackageController.GetPackageOwner(packageId);
            UserSettings webPolicy = UserController.GetUserSettings(user.UserId, UserSettings.WEB_POLICY);
            string packageHome = FilesController.GetHomeFolder(packageId);

            // add random string to the domain if specified
            string randDomainName = itemName;
            if (!String.IsNullOrEmpty(webPolicy["AddRandomDomainString"])
                && Utils.ParseBool(webPolicy["AddRandomDomainString"], false))
                randDomainName += "_" + Utils.GetRandomString(DOMAIN_RANDOM_LENGTH);

            // ROOT folder
            string contentPath = GetWebFolder(packageId, WEBSITE_ROOT_FOLDER_PATTERN, randDomainName);
            if (!String.IsNullOrEmpty(webPolicy["WebRootFolder"]))
                contentPath = GetWebFolder(packageId, webPolicy["WebRootFolder"], randDomainName);

            // LOGS folder
            string logsPath = GetWebFolder(packageId, WEBSITE_LOGS_FOLDER_PATTERN, randDomainName);
            if (!String.IsNullOrEmpty(webPolicy["WebLogsFolder"]))
                logsPath = GetWebFolder(packageId, webPolicy["WebLogsFolder"], randDomainName);

            // DATA folder
            string dataPath = GetWebFolder(packageId, WEBSITE_DATA_FOLDER_PATTERN, randDomainName);
            if (!String.IsNullOrEmpty(webPolicy["WebDataFolder"]))
                dataPath = GetWebFolder(packageId, webPolicy["WebDataFolder"], randDomainName);

            // copy site files
			try
			{
				os.CopyFile(site.ContentPath, contentPath);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex, "Can't copy web site files");
			}

            // copy site logs
            try
            {
                string logFolder = (site.IIs7) ? site[WebSite.IIS7_SITE_ID] : "W3SVC" + siteId;
                string logsSrcFolder = Path.Combine(site.LogsPath, logFolder);
                if (os.DirectoryExists(logsSrcFolder))
                    os.CopyFile(logsSrcFolder, Path.Combine(logsPath, logFolder));
            }
			catch (Exception ex)
			{
				TaskManager.WriteError(ex, "Can't copy web site log files");
			}

            // set new folders
            site.ContentPath = contentPath;
            site.LogsPath = logsPath;
            site.DataPath = dataPath;

            // update web site
            web.UpdateSite(site);

            // process virtual directories
            WebVirtualDirectory[] virtDirs = web.GetVirtualDirectories(siteId);
            foreach (WebVirtualDirectory virtDirPointer in virtDirs)
            {
                try
                {
                    // load virtual directory
                    WebVirtualDirectory virtDir = web.GetVirtualDirectory(siteId, virtDirPointer.Name);

                    // copy directory files
                    string vdirPath = Path.Combine(contentPath, virtDir.Name);
                    os.CopyFile(virtDir.ContentPath, vdirPath);

                    virtDir.ContentPath = vdirPath;

                    // update directory
                    web.UpdateVirtualDirectory(siteId, virtDir);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex, String.Format("Error importing '{0}' virtual directory",
                        virtDirPointer.Name));
                    continue;
                }
            }

            // import web site
            site.ServiceId = serviceId;
            site.PackageId = packageId;
            site.Name = itemName;
            site.SiteId = siteId;
            int webSiteId = PackageController.AddPackageItem(site);
			// Assign dedicated IP address to the web site from package context.
			if (ipMatch != null)
			{
				ServerController.AddItemIPAddress(webSiteId, ipMatch.PackageAddressID);
			}

            // restore/update domains
            RestoreDomainsByWebSite(site.Bindings, packageId, webSiteId, itemName);
        }

        private void RestoreDomainsByWebSite(ServerBinding[] bindings, int packageId, int webSiteId, string itemName)
        {
            // detect all web site related domains
            List<string> domainNames = new List<string>();
            foreach (ServerBinding binding in bindings)
            {
                string pointerName = binding.Host;
                if (pointerName == null)
                    continue;
                pointerName = pointerName.ToLower();
                if (pointerName.StartsWith("www."))
                    pointerName = pointerName.Substring(4);

                if (!domainNames.Contains(pointerName) && !String.IsNullOrEmpty(pointerName))
                    domainNames.Add(pointerName);
            }

            string siteName = itemName.ToLower();
            if (siteName.StartsWith("www."))
                siteName = siteName.Substring(4);
            if (!domainNames.Contains(siteName))
                domainNames.Add(siteName);

            foreach (string domainName in domainNames)
            {
                DomainInfo domain = ServerController.GetDomain(domainName);
                if (domain == null)
                {
                    domain = new DomainInfo();
                    domain.DomainName = domainName;
                    domain.PackageId = packageId;
                    domain.WebSiteId = webSiteId;
                    ServerController.AddDomainItem(domain);
                }
                else
                {
                    domain.WebSiteId = webSiteId;
                    ServerController.UpdateDomain(domain);
                }
            }
        }


        public static int ImporHostHeader(int userId, int packageId, int siteId)
        {
            WebSite siteItem = (WebSite)PackageController.GetPackageItem(siteId);
            if (siteItem == null)
                return -1;

            // load live site from service
            WebServer web = new WebServer();
            ServiceProviderProxy.Init(web, siteItem.ServiceId);

            // Verify if already exists
            List<DomainInfo> domains = ServerController.GetDomains(packageId);

            // Get hostheader
            foreach (ServerBinding b in web.GetSiteBindings(siteItem.SiteId))
            {
                if ((!DoesHeaderExistInDomains(b.Host.ToLower(), domains)) && (!string.IsNullOrEmpty(b.Host)))
                {
                    // If not get domain info and add to domains
                    int domainId = FindDomainForHeader(b.Host.ToLower(), domains);
                    if (domainId > 0)
                    {
                        DomainInfo domain = ServerController.GetDomain(domainId);
                        DomainInfo newDomain = new DomainInfo();
                        newDomain.DomainName = b.Host.ToLower();
                        newDomain.PackageId = domain.PackageId;
                        newDomain.IsDomainPointer = true;

                        int newDomainID = ServerController.AddDomain(newDomain, domain.IsInstantAlias, false);
                        if (newDomainID > 0)
                        {
                            newDomain = ServerController.GetDomain(newDomainID);
                            if (newDomain != null)
                            {
                                newDomain.WebSiteId = siteId;
                                newDomain.ZoneItemId = domain.ZoneItemId;
                                newDomain.DomainItemId = domain.DomainId;
                                ServerController.UpdateDomain(newDomain);
                            }
                        }
                    }
                }
            }

            return 0;
        }

        private static bool DoesHeaderExistInDomains(string header, List<DomainInfo> domains)
        {
            bool bExist = false;

            if (!string.IsNullOrEmpty(header))
            {
                foreach (DomainInfo d in domains)
                {
                    if ((header == d.DomainName.ToLower()) &&
                        (d.IsDomainPointer))
                    {
                        bExist = true;
                        break;
                    }
                }
            }
            return bExist;
        }


        private static int FindDomainForHeader(string header, List<DomainInfo> domains)
        {
            int domainId = 0;
            int counter = 0;
            while ((header.IndexOf(".") != -1) & (counter < 2))
            {

                foreach (DomainInfo d in domains)
                {
                    if ((header == d.DomainName.ToLower()) && (!d.IsDomainPointer))
                    {
                        return d.DomainId;
                    }
                }

                header = header.Substring(header.IndexOf(".") + 1);
                counter++;

            }

            return domainId;
        }



        #endregion

        #region IBackupController Members

        public int BackupItem(string tempFolder, System.Xml.XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
        {
            if (item is WebSite)
            {
                WebServer web = GetWebServer(item.ServiceId);

                // read web site
                WebSite itemSite = item as WebSite;
                string siteId = itemSite.SiteId;
                WebSite site = web.GetSite(siteId);
                site.SiteId = itemSite.SiteId;
                site.SiteIPAddressId = itemSite.SiteIPAddressId;
                site.DataPath = itemSite.DataPath;
                site.FrontPageAccount = itemSite.FrontPageAccount;
                site.FrontPagePassword = itemSite.FrontPagePassword;

                // serialize web site
                XmlSerializer serializer = new XmlSerializer(typeof(WebSite));
                serializer.Serialize(writer, site);

                // process virtual directories
                WebVirtualDirectory[] vdirs = web.GetVirtualDirectories(siteId);
                foreach (WebVirtualDirectory vdirShort in vdirs)
                {
                    WebVirtualDirectory vdir = web.GetVirtualDirectory(siteId, vdirShort.Name);

                    // serialize vdir
                    serializer = new XmlSerializer(typeof(WebVirtualDirectory));
                    serializer.Serialize(writer, vdir);
                }
            }
            else if (item is SharedSSLFolder)
            {
                SharedSSLFolder sslFolder = GetSharedSSLFolder(item.Id);

                // convert content path to physical
                sslFolder.ContentPath = FilesController.GetFullPackagePath(item.PackageId, sslFolder.ContentPath);

                XmlSerializer serializer = new XmlSerializer(typeof(SharedSSLFolder));
                serializer.Serialize(writer, sslFolder);
            }

            return 0;
        }

        public int RestoreItem(string tempFolder, System.Xml.XmlNode itemNode, int itemId, Type itemType, string itemName, int packageId, int serviceId, ResourceGroupInfo group)
        {
            if (itemType == typeof(WebSite))
            {
                WebServer web = GetWebServer(serviceId);

                // restore web site
                XmlSerializer serializer = new XmlSerializer(typeof(WebSite));
                WebSite site = (WebSite)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("WebSite")));

                // create site if required
                if (!web.SiteExists(site.SiteId))
                {
                    web.CreateSite(site);

                    // install FPSE if required
                    if (site.FrontPageInstalled && !web.IsFrontPageInstalled(site.SiteId))
                    {
                        web.InstallFrontPage(site.SiteId, site.FrontPageAccount,
                            CryptoUtils.Decrypt(site.FrontPagePassword));
                    }
                }

                // restore virtual directories
                foreach (XmlNode vdirNode in itemNode.SelectNodes("WebVirtualDirectory"))
                {
                    // deserialize vdir
                    serializer = new XmlSerializer(typeof(WebVirtualDirectory));
                    WebVirtualDirectory vdir = (WebVirtualDirectory)serializer.Deserialize(
                        new XmlNodeReader(vdirNode));

                    if (!web.VirtualDirectoryExists(site.SiteId, vdir.Name))
                    {
                        web.CreateVirtualDirectory(site.SiteId, vdir);
                    }
                }

                // add meta-item if required
                int webSiteId = 0;
                WebSite existItem = (WebSite)PackageController.GetPackageItemByName(packageId, itemName, typeof(WebSite));
                if (existItem == null)
                {
                    site.PackageId = packageId;
                    site.ServiceId = serviceId;
                    webSiteId = PackageController.AddPackageItem(site);
                }
                else
                {
                    webSiteId = existItem.Id;
                }

                // restore/update domains
                RestoreDomainsByWebSite(site.Bindings, packageId, webSiteId, itemName);
            }
            else if (itemType == typeof(SharedSSLFolder))
            {
                WebServer web = GetWebServer(serviceId);

                // extract meta item
                XmlSerializer serializer = new XmlSerializer(typeof(SharedSSLFolder));
                SharedSSLFolder sslFolder = (SharedSSLFolder)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("SharedSSLFolder")));

                // create vdir if required
                int idx = sslFolder.Name.LastIndexOf("/");
                string domainName = sslFolder.Name.Substring(0, idx);
                string vdirName = sslFolder.Name.Substring(idx + 1);

                string siteId = web.GetSiteId(domainName);
                if (siteId == null)
                    return -1;

                if (!web.VirtualDirectoryExists(siteId, vdirName))
                {
                    web.CreateVirtualDirectory(siteId, sslFolder);
                }

                // add meta-item if required
                if (PackageController.GetPackageItemByName(packageId, itemName, typeof(SharedSSLFolder)) == null)
                {
                    sslFolder.PackageId = packageId;
                    sslFolder.ServiceId = serviceId;
                    PackageController.AddPackageItem(sslFolder);
                }
            }

            return 0;
        }

        #endregion

		#region SSL
		public static SSLCertificate CertificateRequest(SSLCertificate certificate, int siteItemId)
		{

			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "certificateRequest");
				TaskManager.WriteParameter("SiteItemId", siteItemId);
				TaskManager.WriteParameter("Hostname", certificate.Hostname);


				WebSite item = GetWebSite(siteItemId) as WebSite;
				PackageInfo service = PackageController.GetPackage(item.PackageId);
				TaskManager.WriteParameter("WebSite.Name", item.Name);
				WebServer server = GetWebServer(item.ServiceId);
				TaskManager.WriteParameter("item.ServiceId", item.ServiceId);
				certificate.UserID = service.UserId;
				// We want to have a unique identifier for the FriendlyName to help us to identify it easily
				long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
				ticks /= 10000000; // Convert windows ticks to seconds

                //clean up bindings



				certificate.FriendlyName = String.Format("{0}_{1}", certificate.Hostname, ticks.ToString());
				certificate = server.generateCSR(certificate);
				certificate.id = DataProvider.AddSSLRequest(SecurityContext.User.UserId, item.PackageId,
					certificate.SiteID, certificate.UserID, certificate.FriendlyName, certificate.Hostname,
					certificate.CSR, certificate.CSRLength, certificate.DistinguishedName, certificate.IsRenewal,
					certificate.PreviousId);

			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);

			}
			finally
			{
				TaskManager.CompleteTask();
			}
			return certificate;

		}

		public static ResultObject InstallCertificate(SSLCertificate certificate, int siteItemId)
		{
			ResultObject result = new ResultObject { IsSuccess = true };
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "installCertificate");
				TaskManager.WriteParameter("SiteItemId", siteItemId);
				TaskManager.WriteParameter("Hostname", certificate.Hostname);


				WebSite item = GetWebSite(siteItemId) as WebSite;
				TaskManager.WriteParameter("WebSite.Name", item.Name);
				WebServer server = GetWebServer(item.ServiceId);
				TaskManager.WriteParameter("item.ServiceId", item.ServiceId);

                IPAddressInfo ip = ServerController.GetIPAddress(item.SiteIPAddressId);

                if (ip != null)
                    item.SiteIPAddress = !String.IsNullOrEmpty(ip.InternalIP) ? ip.InternalIP : ip.ExternalIP;

				certificate = server.installCertificate(certificate, item);
				if (certificate.SerialNumber == null)
				{
					result.AddError("Error_Installing_certificate", null);
					result.IsSuccess = false;
				}


				DataProvider.CompleteSSLRequest(SecurityContext.User.UserId, item.PackageId,
												certificate.id, certificate.Certificate,
												certificate.DistinguishedName, certificate.SerialNumber,
												certificate.Hash, certificate.ValidFrom, certificate.ExpiryDate);
				if (certificate.IsRenewal)
				{
					DataProvider.DeleteCertificate(SecurityContext.User.UserId, item.PackageId, certificate.PreviousId);
				}

			}
			catch (Exception ex)
			{
				result.AddError("0", ex);
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			return result;
		}

		public static ResultObject InstallPfx(byte[] pfx, int siteItemId, string password)
		{
			ResultObject result = new ResultObject { IsSuccess = true };
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "installPFX");
				TaskManager.WriteParameter("SiteItemId", siteItemId);

				WebSite item = GetWebSite(siteItemId) as WebSite;
				PackageInfo service = PackageController.GetPackage(item.PackageId);

				TaskManager.WriteParameter("WebSite.Name", item.Name);
				WebServer server = GetWebServer(item.ServiceId);
				TaskManager.WriteParameter("item.ServiceId", item.ServiceId);

                // remove all web site pointers
                List<DomainInfo> pointers = GetWebSitePointers(siteItemId);
                foreach (DomainInfo pointer in pointers)
                    DeleteWebSitePointer(siteItemId, pointer.DomainId, true, true, true);

				SSLCertificate certificate = server.installPFX(pfx, password, item);
				if (certificate.SerialNumber == null)
				{
					result.AddError("Error_Installing_certificate", null);
					result.IsSuccess = false;
				}
				DataProvider.AddPFX(SecurityContext.User.UserId, item.PackageId, item.Id, service.UserId, certificate.Hostname,
				   certificate.FriendlyName, certificate.DistinguishedName, certificate.CSRLength, certificate.SerialNumber,
				   certificate.ValidFrom, certificate.ExpiryDate);


			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.AddError("Error_Installing_certificate", ex);
				TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			return result;
		}

		public static List<SSLCertificate> GetPendingCertificates(int siteItemId)
		{
			WebSite item = GetWebSite(siteItemId) as WebSite;
			List<SSLCertificate> certificates = new List<SSLCertificate>();
			return ObjectUtils.CreateListFromDataSet<SSLCertificate>(
				DataProvider.GetPendingCertificates(SecurityContext.User.UserId, item.PackageId, item.Id, false));

		}

		public static SSLCertificate GetSslCertificateById(int iD)
		{
			return ObjectUtils.FillObjectFromDataReader<SSLCertificate>(
				DataProvider.GetSSLCertificateByID(SecurityContext.User.UserId, iD));
		}

		public static int CheckSSL(int siteID, bool renewal)
		{
			return DataProvider.CheckSSL(siteID, renewal);
		}

		public static ResultObject CheckSSLForDomain(string domain, int siteID)
		{
			ResultObject result = new ResultObject { IsSuccess = true };
			return result;
		}

		public static SSLCertificate GetSiteCert(int siteid)
		{
			return ObjectUtils.FillObjectFromDataReader<SSLCertificate>(
				DataProvider.GetSiteCert(SecurityContext.User.UserId, siteid));
		}

		public static List<SSLCertificate> GetCertificatesForSite(int siteId)
		{
			WebSite item = GetWebSite(siteId) as WebSite;
			List<SSLCertificate> certificates = new List<SSLCertificate>();
			return ObjectUtils.CreateListFromDataSet<SSLCertificate>(
				DataProvider.GetCertificatesForSite(SecurityContext.User.UserId, item.PackageId, item.Id));
		}

		public static byte[] ExportCertificate(int siteId, string serialNumber, string password)
		{
			WebSite item = GetWebSite(siteId) as WebSite;

			WebServer server = GetWebServer(item.ServiceId);
			return server.exportCertificate(serialNumber, password);
		}

		public static ResultObject DeleteCertificate(int siteId, SSLCertificate certificate)
		{

			ResultObject result = new ResultObject { IsSuccess = true };
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "DeleteCertificate");
				WebSite item = GetWebSite(siteId) as WebSite;
				WebServer server = GetWebServer(item.ServiceId);
				result = server.DeleteCertificate(certificate, item);
				if (result.IsSuccess)
				{
					DataProvider.DeleteCertificate(SecurityContext.User.UserId, item.PackageId, certificate.id);
				}
				else
				{
					List<string> exceptions = result.ErrorCodes;
					foreach (string s in exceptions)
					{
						TaskManager.WriteError(s.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				result.IsSuccess = false;
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			return result;
		}

		public static ResultObject ImportCertificate(int siteId)
		{
			ResultObject result = new ResultObject { IsSuccess = true };
			try
			{
				TaskManager.StartTask(LOG_SOURCE_WEB, "ImportCertificate");

				WebSite item = GetWebSite(siteId) as WebSite;
				WebServer server = GetWebServer(item.ServiceId);
				PackageInfo service = PackageController.GetPackage(item.PackageId);
				SSLCertificate certificate = server.ImportCertificate(item);
				if (!certificate.Success)
				{
					result.IsSuccess = false;
					TaskManager.WriteError(certificate.Certificate.ToString());
				}
				else
				{
					DataProvider.AddPFX(SecurityContext.User.UserId, item.PackageId, item.Id, service.UserId, certificate.Hostname,
				   certificate.FriendlyName, certificate.DistinguishedName, certificate.CSRLength, certificate.SerialNumber,
				   certificate.ValidFrom, certificate.ExpiryDate);
				}

			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				result.IsSuccess = false;

			}
			finally
			{
				TaskManager.CompleteTask();
			}
			return result;
		}

		public static ResultObject CheckCertificate(int siteId)
		{
			ResultObject result = new ResultObject { IsSuccess = false };
			bool serverResult = false;

			try
			{
				WebSite item = GetWebSite(siteId) as WebSite;
				WebServer server = GetWebServer(item.ServiceId);
				serverResult = server.CheckCertificate(item);
				if (serverResult && !DataProvider.CheckSSLExistsForWebsite(siteId))
				{
					result.IsSuccess = true;
				}
			}
			catch (Exception ex)
			{

				result.IsSuccess = false;
				result.AddError("610", ex);
			}
			return result;
		}

		public static ResultObject DeleteCertificateRequest(int siteId, int csrID)
		{
			ResultObject result = new ResultObject { IsSuccess = true };

			try
			{
				WebSite item = GetWebSite(siteId);
				PackageInfo service = PackageController.GetPackage(item.PackageId);

				DataProvider.DeleteCertificate(SecurityContext.User.UserId, service.PackageId, csrID);
			}
			catch (Exception ex)
			{

				result.IsSuccess = false;
				result.AddError("610", ex);
			}
			return result;
		}
		#endregion
    }
}
