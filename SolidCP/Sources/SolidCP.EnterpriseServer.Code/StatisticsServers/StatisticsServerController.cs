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
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Web;

using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.Statistics;

namespace SolidCP.EnterpriseServer
{
    public class StatisticsServerController : IImportController, IBackupController
    {
        public static StatisticsServer GetStatisticsServer(int serviceId)
        {
            StatisticsServer stats = new StatisticsServer();
            ServiceProviderProxy.Init(stats, serviceId);
            return stats;
        }

        public static DataSet GetRawStatisticsSitesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return PackageController.GetRawPackageItemsPaged(packageId, typeof(StatsSite),
                true, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<StatsSite> GetStatisticsSites(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(StatsSite), recursive);

            return items.ConvertAll<StatsSite>(
                new Converter<ServiceProviderItem, StatsSite>(ConvertItemToStatisticsSiteItem));
        }

        private static StatsSite ConvertItemToStatisticsSiteItem(ServiceProviderItem item)
        {
            return (StatsSite)item;
        }

        public static StatsServer[] GetServers(int serviceId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return null;

            StatisticsServer stats = new StatisticsServer();
            ServiceProviderProxy.Init(stats, serviceId);
            return stats.GetServers();
        }

        public static StatsSite GetSite(int itemId)
        {
            // load meta item
            StatsSite item = (StatsSite)PackageController.GetPackageItem(itemId);
            if (item == null)
                return null;

            // load item from service
            StatisticsServer stats = new StatisticsServer();
            ServiceProviderProxy.Init(stats, item.ServiceId);
            StatsSite site = stats.GetSite(item.SiteId);

            if (site == null)
                return null;

            site.Id = item.Id;
            site.Name = item.Name;
            site.ServiceId = item.ServiceId;
            site.PackageId = item.PackageId;
            site.SiteId = item.SiteId;

            // update statistics URL
            if (!String.IsNullOrEmpty(site.StatisticsUrl))
            {
				// load space owner
				UserInfo user = PackageController.GetPackageOwner(item.PackageId);
				if (user != null)
				{
                    UserInfoInternal userInternal = UserController.GetUserInternally(user.UserId);

                    site.StatisticsUrl = Utils.ReplaceStringVariable(site.StatisticsUrl, "username",
                        HttpUtility.UrlEncode(userInternal.Username));
					site.StatisticsUrl = Utils.ReplaceStringVariable(site.StatisticsUrl, "password",
                        HttpUtility.UrlEncode(userInternal.Password));
				}
            }

            return site;
        }

        public static int AddSite(StatsSite item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(item.PackageId, Quotas.STATS_SITES);
            if (quota.QuotaExhausted)
                return BusinessErrorCodes.ERROR_STATS_RESOURCE_QUOTA_LIMIT;

            // check if stats resource is available
            int serviceId = PackageController.GetPackageServiceId(item.PackageId, ResourceGroups.Statistics);
            if (serviceId == 0)
                return BusinessErrorCodes.ERROR_STATS_RESOURCE_UNAVAILABLE;

            // check package items
            if (PackageController.GetPackageItemByName(item.PackageId, item.Name, typeof(StatsSite)) != null)
                return BusinessErrorCodes.ERROR_STATS_PACKAGE_ITEM_EXISTS;

            // place log record
            TaskManager.StartTask("STATS_SITE", "ADD", item.Name);

            try
            {
                // load web site
                WebSite siteItem = (WebSite)PackageController.GetPackageItemByName(item.PackageId,
                    item.Name, typeof(WebSite));

                if (siteItem == null)
                    return BusinessErrorCodes.ERROR_WEB_SITE_SERVICE_UNAVAILABLE;

                // get service web site
                WebServer web = new WebServer();
                ServiceProviderProxy.Init(web, siteItem.ServiceId);
                WebSite site = web.GetSite(siteItem.SiteId);

                List<DomainInfo> pointers = WebServerController.GetWebSitePointers(siteItem.Id);
                List<string> aliases = new List<string>();

                foreach(DomainInfo pointer in pointers)
                    aliases.Add(pointer.DomainName);

                StatisticsServer stats = new StatisticsServer();
                ServiceProviderProxy.Init(stats, serviceId);
                string siteNumber = (site.IIs7) ? site[WebSite.IIS7_SITE_ID] : siteItem.SiteId.Replace("/", "");
                string logsCommonFolder = site.LogsPath;
                string logsFolder = Path.Combine(logsCommonFolder, siteNumber);

				// get service settings
				StringDictionary settings = ServerController.GetServiceSettings(serviceId);
				if (Utils.ParseBool(settings["BuildUncLogsPath"], false))
				{
					logsFolder = FilesController.ConvertToUncPath(siteItem.ServiceId, logsFolder);
				}

                item.LogDirectory = logsFolder;
                item.DomainAliases = aliases.ToArray();

                // install statistics
                item.SiteId = stats.AddSite(item);

                // save statistics item
                item.ServiceId = serviceId;
                int itemId = PackageController.AddPackageItem(item);

                TaskManager.ItemId = itemId;

                return itemId;
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

        public static int UpdateSite(StatsSite item)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original meta item
            StatsSite origItem = (StatsSite)PackageController.GetPackageItem(item.Id);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_STATS_PACKAGE_ITEM_NOT_FOUND;

            // check package
            int packageCheck = SecurityContext.CheckPackage(origItem.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // update statistics site
            item.Name = origItem.Name;
            item.SiteId = origItem.SiteId;

            // place log record
            TaskManager.StartTask("STATS_SITE", "UPDATE", origItem.Name, origItem.Id);

            try
            {
                StatisticsServer stats = new StatisticsServer();
                ServiceProviderProxy.Init(stats, origItem.ServiceId);
                stats.UpdateSite(item);

                // update service item
                PackageController.UpdatePackageItem(item);

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

        public static int DeleteSite(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;


            // load original meta item
            StatsSite origItem = (StatsSite)PackageController.GetPackageItem(itemId);
            if (origItem == null)
                return BusinessErrorCodes.ERROR_STATS_PACKAGE_ITEM_NOT_FOUND;

            // place log record
            TaskManager.StartTask("STATS_SITE", "DELETE", origItem.Name, itemId);

            try
            {
                // get service
                StatisticsServer stats = new StatisticsServer();
                ServiceProviderProxy.Init(stats, origItem.ServiceId);

                // delete service item
                stats.DeleteSite(origItem.SiteId);

                // delete meta item
                PackageController.DeletePackageItem(origItem.Id);

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

        #region IImportController Members

        public List<string> GetImportableItems(int packageId, int itemTypeId, Type itemType, ResourceGroupInfo group)
        {
            List<string> items = new List<string>();

            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return items;

            // Mail provider
            StatisticsServer stats = new StatisticsServer();
            ServiceProviderProxy.Init(stats, serviceId);

            if (itemType == typeof(StatsSite))
                items.AddRange(stats.GetSites());

            return items;
        }

        public void ImportItem(int packageId, int itemTypeId, Type itemType,
			ResourceGroupInfo group, string itemName)
        {
            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return;

            StatisticsServer stats = new StatisticsServer();
            ServiceProviderProxy.Init(stats, serviceId);

            if (itemType == typeof(StatsSite))
            {
                // import statistics site
                StatsSite site = new StatsSite();
                site.ServiceId = serviceId;
                site.PackageId = packageId;
                site.Name = itemName;
                site.GroupName = group.GroupName;

                // load site id
                site.SiteId = stats.GetSiteId(itemName);

                PackageController.AddPackageItem(site);
            }
        }

        #endregion

        #region IBackupController Members

        public int BackupItem(string tempFolder, System.Xml.XmlWriter writer, ServiceProviderItem item, ResourceGroupInfo group)
        {
            if (item is StatsSite)
            {
                // backup stats site
                StatisticsServer stats = GetStatisticsServer(item.ServiceId);

                // read site info
                StatsSite itemSite = item as StatsSite;
                StatsSite site = stats.GetSite(itemSite.SiteId);

                XmlSerializer serializer = new XmlSerializer(typeof(StatsSite));
                serializer.Serialize(writer, site);
            }
            return 0;
        }

        public int RestoreItem(string tempFolder, System.Xml.XmlNode itemNode, int itemId, Type itemType, string itemName, int packageId, int serviceId, ResourceGroupInfo group)
        {
            if (itemType == typeof(StatsSite))
            {
                StatisticsServer stats = GetStatisticsServer(serviceId);

                // extract meta item
                XmlSerializer serializer = new XmlSerializer(typeof(StatsSite));
                StatsSite site = (StatsSite)serializer.Deserialize(
                    new XmlNodeReader(itemNode.SelectSingleNode("StatsSite")));

                // create site if required
                if (stats.GetSite(site.SiteId) == null)
                {
                    stats.AddSite(site);
                }

                // add meta-item if required
                if (PackageController.GetPackageItemByName(packageId, itemName, typeof(StatsSite)) == null)
                {
                    site.PackageId = packageId;
                    site.ServiceId = serviceId;
                    PackageController.AddPackageItem(site);
                }
            }

            return 0;
        }

        #endregion
    }
}
