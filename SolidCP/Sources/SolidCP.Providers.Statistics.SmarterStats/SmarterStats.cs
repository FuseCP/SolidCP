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
using System.Text;
using System.Web.Services.Protocols;
using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using Microsoft.Win32;


namespace SolidCP.Providers.Statistics
{
    public class SmarterStats : HostingServiceProviderBase, IStatisticsServer
    {
        #region Properties
        protected string SmarterUrl
        {
            get { return ProviderSettings["SmarterUrl"]; }
        }

        protected string Username
        {
            get { return ProviderSettings["Username"]; }
        }

        protected string Password
        {
            get { return ProviderSettings["Password"]; }
        }

        protected int ServerId
        {
            get { try { return Int32.Parse(ProviderSettings["ServerID"]); } catch { return 1; } }
        }

        protected string LogFormat
        {
            get { return ProviderSettings["LogFormat"]; }
        }

        protected string LogWildcard
        {
            get { return ProviderSettings["LogWildcard"]; }
        }

        protected int LogDeleteDays
        {
            get { try { return Int32.Parse(ProviderSettings["LogDeleteDays"]); } catch { return 0; } }
        }

        protected string SmarterLogsPath
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["SmarterLogsPath"]); }
        }

        protected int SmarterLogDeleteMonths
        {
            get { try { return Int32.Parse(ProviderSettings["SmarterLogDeleteMonths"]); } catch { return 0; } }
        }

        protected int TimeZoneId
        {
            get { try { return Int32.Parse(ProviderSettings["TimeZoneId"]); } catch { return 1; } }
        }

		protected string StatisticsUrl
		{
			get { return ProviderSettings["StatisticsUrl"]; }
		}
        #endregion

        #region IStatistics methods
        public virtual StatsServer[] GetServers()
        {
            ServerAdmin srvAdmin = new ServerAdmin();
            PrepareProxy(srvAdmin);

            ServerInfoArrayResult result = srvAdmin.GetServers(Username, Password);
            if (result.Servers == null)
                return new StatsServer[] { };

            StatsServer[] servers = new StatsServer[result.Servers.Length];
            for (int i = 0; i < servers.Length; i++)
            {
                servers[i] = new StatsServer();
                servers[i].ServerId = result.Servers[i].ServerID;
                servers[i].Name = result.Servers[i].Name;
                servers[i].Ip = result.Servers[i].IP;
            }

            return servers;
        }

        public virtual string GetSiteId(string siteName)
        {
            SiteAdmin stAdmin = new SiteAdmin();
            PrepareProxy(stAdmin);

            SiteInfoArrayResult result = stAdmin.GetAllSites(Username, Password, false);
            if (result.Sites == null)
                return null;

            foreach (SiteInfo site in result.Sites)
            {
                if (String.Compare(siteName, site.DomainName, 0) == 0)
                    return site.SiteID.ToString();
            }

            return null;
        }

        public virtual string[] GetSites()
        {
            List<string> sites = new List<string>();

            SiteAdmin stAdmin = new SiteAdmin();
            PrepareProxy(stAdmin);

            SiteInfoArrayResult result = stAdmin.GetAllSites(Username, Password, false);
            if (result.Sites == null)
                return sites.ToArray();

            foreach (SiteInfo site in result.Sites)
                sites.Add(site.DomainName);

            return sites.ToArray();
        }

        public virtual StatsSite GetSite(string siteId)
        {
            SiteAdmin stAdmin = new SiteAdmin();
            PrepareProxy(stAdmin);

            int sid = Int32.Parse(siteId);
            SiteInfoResult result = stAdmin.GetSite(Username, Password, sid);
            if (result.Site == null)
                return null;

            StatsSite site = new StatsSite();
            site.Name = result.Site.DomainName;
            site.ExportPath = result.Site.ExportPath;
            site.ExportPathUrl = result.Site.ExportPathURL;
            site.LogDirectory = result.Site.LogDirectory;
            site.TimeZoneId = TimeZoneId;
            site.Status = result.Site.SiteStatus;

			// process stats URL
			string url = null;
			if (!String.IsNullOrEmpty(StatisticsUrl))
			{
				url = StringUtils.ReplaceStringVariable(StatisticsUrl, "domain_name", site.Name);
				url = StringUtils.ReplaceStringVariable(url, "site_id", siteId);
			}

            // get site users
            UserAdmin usrAdmin = new UserAdmin();
            PrepareProxy(usrAdmin);

            UserInfoResultArray usrResult = usrAdmin.GetUsers(Username, Password, sid);
            if (usrResult.user != null)
            {
                site.Users = new StatsUser[usrResult.user.Length];
                for (int i = 0; i < site.Users.Length; i++)
                {
                    site.Users[i] = new StatsUser();
                    site.Users[i].Username = usrResult.user[i].UserName;
                    site.Users[i].Password = usrResult.user[i].Password;
                    site.Users[i].FirstName = usrResult.user[i].FirstName;
                    site.Users[i].LastName = usrResult.user[i].LastName;
                    site.Users[i].IsAdmin = usrResult.user[i].IsAdmin;
                    site.Users[i].IsOwner = usrResult.user[i].IsSiteOwner;
                }

				if (site.Users.Length > 0 && !String.IsNullOrEmpty(url))
				{
					url = StringUtils.ReplaceStringVariable(url, "username", site.Users[0].Username);
					url = StringUtils.ReplaceStringVariable(url, "password", site.Users[0].Password);
				}
            }

			site.StatisticsUrl = url;

            return site;
        }


        public virtual string AddSite(StatsSite site)
        {
            // generate unique SiteID
            //int iSiteId = site.Name.GetHashCode();
            //if (iSiteId < 0)
            //     iSiteId *= -1;

            //string siteId = iSiteId.ToString();

            // create logs folder if not exists
            //if (!FileUtils.DirectoryExists(site.LogDirectory))
            //    FileUtils.CreateDirectory(site.LogDirectory);

            // add site
            SiteAdmin stAdmin = new SiteAdmin();
            PrepareProxy(stAdmin);

            if (site.Users == null || site.Users.Length == 0)
                throw new Exception("At least one user (site owner) should be specified when creating new statistics site");

            string ownerUsername = site.Users[0].Username.ToLower();
            GenericResult1 result = stAdmin.AddSite(Username, Password,
                site.Users[0].Username, site.Users[0].Password, site.Users[0].FirstName, site.Users[0].LastName,
                ServerId, 0, site.Name, site.LogDirectory, LogFormat, LogWildcard, LogDeleteDays,
                SmarterLogsPath, SmarterLogDeleteMonths, "", "", TimeZoneId);

            if (!result.Result)
                throw new Exception("Error creating statistics site: " + result.Message);

            string siteId = GetSiteId(site.Name);

            int iSiteId = Int32.Parse(siteId);

            // add other users
            UserAdmin usrAdmin = new UserAdmin();
            PrepareProxy(usrAdmin);
            foreach (StatsUser user in site.Users)
            {
                if (user.Username.ToLower() != ownerUsername)
                {
                    // add user
                    GenericResult2 r = usrAdmin.AddUser(Username, Password, iSiteId,
                        user.Username, user.Password, user.FirstName, user.LastName, user.IsAdmin);
                    if (!r.Result)
                        throw new Exception("Error adding site user: " + r.Message);
                }
            }

            return siteId;
        }

        public virtual void UpdateSite(StatsSite site)
        {
            // update site
            SiteAdmin stAdmin = new SiteAdmin();
            PrepareProxy(stAdmin);

            int siteId = Int32.Parse(site.SiteId);

            // get original site
            SiteInfoResult siteResult = stAdmin.GetSite(Username, Password, siteId);
            if (siteResult.Site == null)
                return;

            SiteInfo origSite = siteResult.Site;

            // update site with only required properties
            GenericResult1 result = stAdmin.UpdateSite(Username, Password, siteId, site.Name, origSite.LogDirectory,
                origSite.LogFormat, origSite.LogWildcard, origSite.LogDaysBeforeDelete,
                origSite.SmarterLogDirectory, origSite.SmarterLogMonthsBeforeDelete, origSite.ExportPath, origSite.ExportPathURL,
                origSite.TimeZoneID);

            if (!result.Result)
                throw new Exception("Error updating statistics site: " + result.Message);

            // update site users
            UserAdmin usrAdmin = new UserAdmin();
            PrepareProxy(usrAdmin);

            // get original users
            if (site.Users != null)
            {
                List<string> origUsers = new List<string>();
                List<string> newUsers = new List<string>();
                string ownerUsername = null;
                UserInfoResultArray usrResult = usrAdmin.GetUsers(Username, Password, siteId);
                foreach (UserInfo user in usrResult.user)
                {
                    // add to original users
                    origUsers.Add(user.UserName.ToLower());

                    // remember owner (he can't be deleted)
                    if (user.IsSiteOwner)
                        ownerUsername = user.UserName.ToLower();
                }

                // add, update users
                foreach (StatsUser user in site.Users)
                {
                    if (!origUsers.Contains(user.Username.ToLower()))
                    {
                        // add user
                        GenericResult2 r = usrAdmin.AddUser(Username, Password, siteId,
                            user.Username, user.Password, user.FirstName, user.LastName, user.IsAdmin);
                        if (!r.Result)
                            throw new Exception("Error adding site user: " + r.Message);
                    }
                    else
                    {
                        // update user
                        GenericResult2 r = usrAdmin.UpdateUser(Username, Password, siteId,
                            user.Username, user.Password, user.FirstName, user.LastName, user.IsAdmin);
                        if (!r.Result)
                            throw new Exception("Error updating site user: " + r.Message);
                    }

                    // add to new users list
                    newUsers.Add(user.Username.ToLower());
                }

                // delete users
                foreach (string username in origUsers)
                {
                    if (!newUsers.Contains(username) && username != ownerUsername)
                    {
                        // delete user
                        GenericResult2 r = usrAdmin.DeleteUser(Username, Password, siteId, username);
                    }
                }
            }
        }

        public virtual void DeleteSite(string siteId)
        {
            // delete site
            SiteAdmin stAdmin = new SiteAdmin();
            PrepareProxy(stAdmin);

            int sid = Int32.Parse(siteId);

            GenericResult1 result = stAdmin.DeleteSite(Username, Password, sid, true);
            if (!result.Result)
                throw new Exception("Error deleting statistics site: " + result.Message);
        }

        #endregion

        #region IHostingServiceProvider methods
        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is StatsSite)
                {
                    try
                    {
                        DeleteSite(((StatsSite)item).SiteId);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                    }
                }
            }
        }
        #endregion

        #region Helper Methods
        public void PrepareProxy(SoapHttpClientProtocol proxy)
        {
            string smarterUrl = SmarterUrl;

            int idx = proxy.Url.LastIndexOf("/");

            // strip the last slash if any
            if (smarterUrl[smarterUrl.Length - 1] == '/')
                smarterUrl = smarterUrl.Substring(0, smarterUrl.Length - 1);

            proxy.Url = smarterUrl + proxy.Url.Substring(idx);
        }
        #endregion

        public override bool IsInstalled()
        {
            string productName = null;
            string productVersion = null;
            String[] names = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + s);

                    if (subkey != null)
                    {
                        if (!String.IsNullOrEmpty((string) subkey.GetValue("DisplayName")))
                        {
                            productName = (string) subkey.GetValue("DisplayName");
                        }
                        if (productName != null)
                            if (productName.Equals("SmarterStats") || productName.Equals("SmarterStats Service"))
                            {
                                productVersion = (string) subkey.GetValue("DisplayVersion");
                                break;
                            }
                    }
                }

                if (!String.IsNullOrEmpty(productVersion))
                {
                    string[] split = productVersion.Split(new char[] {'.'});
                    return split[0].Equals("3");
                }
            }
            
                //checking x64 platform
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

                if (key != null)
                {
                    names = key.GetSubKeyNames();

                    foreach (string s in names)
                    {
                        RegistryKey subkey =
                            HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + s);

                        if (subkey != null)
                        {
                            if (!String.IsNullOrEmpty((string) subkey.GetValue("DisplayName")))
                            {
                                productName = (string) subkey.GetValue("DisplayName");
                            }
                            if (productName != null)
                                if (productName.Equals("SmarterStats") || productName.Equals("SmarterStats Service"))
                                {
                                    productVersion = (string) subkey.GetValue("DisplayVersion");
                                    break;
                                }
                        }
                    }

                    if (!String.IsNullOrEmpty(productVersion))
                    {
                        string[] split = productVersion.Split(new char[] {'.'});
                        return split[0].Equals("3");
                    }
                }
            
            return false;
        }

    }
}
