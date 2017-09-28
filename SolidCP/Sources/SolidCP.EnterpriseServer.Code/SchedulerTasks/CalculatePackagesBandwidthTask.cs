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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

using SolidCP.Providers;

namespace SolidCP.EnterpriseServer
{
    public class CalculatePackagesBandwidthTask : SchedulerTask
    {
        private readonly bool suspendOverused = false;

        public override void DoWork()
        {
            // Input parameters:
            //  - SUSPEND_OVERUSED_PACKAGES

            CalculateBandwidth();
        }

        public void CalculateBandwidth()
        {
            // get all owned packages
            List<PackageInfo> packages = PackageController.GetPackagePackages(TaskManager.TopTask.PackageId, true);
            TaskManager.Write("Packages to calculate: " + packages.Count.ToString());

            foreach (PackageInfo package in packages)
            {
                // calculating package bandwidth
                CalculatePackage(package.PackageId);
            }
        }

        public void CalculatePackage(int packageId)
        {
            DateTime since = PackageController.GetPackageBandwidthUpdate(packageId);
            DateTime nextUpdate = DateTime.Now;

            try
            {
                // get all package items
                List<ServiceProviderItem> items = PackageController.GetServiceItemsForStatistics(
                    0, packageId, false, true, false, false);

                // order items by service
                Dictionary<int, List<ServiceProviderItem>> orderedItems =
                    PackageController.OrderServiceItemsByServices(items);

                // calculate statistics for each service set
                List<ServiceProviderItemBandwidth> itemsBandwidth = new List<ServiceProviderItemBandwidth>();
                foreach (int serviceId in orderedItems.Keys)
                {
                    ServiceProviderItemBandwidth[] serviceBandwidth = CalculateItems(serviceId,
                        orderedItems[serviceId], since);
                    if (serviceBandwidth != null)
                        itemsBandwidth.AddRange(serviceBandwidth);
                }

                // update info in the database
                string xml = BuildDiskBandwidthStatisticsXml(itemsBandwidth.ToArray());
                PackageController.UpdatePackageBandwidth(packageId, xml);

                // if everything is OK
                // update date
                PackageController.UpdatePackageBandwidthUpdate(packageId, nextUpdate);

                // suspend package if requested
                if (suspendOverused)
                {
                    // disk space
                    QuotaValueInfo dsQuota = PackageController.GetPackageQuota(packageId, Quotas.OS_BANDWIDTH);

                    if (dsQuota.QuotaExhausted)
                        PackageController.ChangePackageStatus(null, packageId, PackageStatus.Suspended, false);
                }
            }
            catch (Exception ex)
            {
                // load package details
                PackageInfo package = PackageController.GetPackage(packageId);

                // load user details
                UserInfo user = PackageController.GetPackageOwner(package.PackageId);

                // log error
                TaskManager.WriteError(String.Format("Error calculating bandwidth for '{0}' space of user '{1}': {2}",
                    package.PackageName, user.Username, ex.ToString()));
            }
        }

        public ServiceProviderItemBandwidth[] CalculateItems(int serviceId, List<ServiceProviderItem> items,
            DateTime since)
        {
            // convert items to SoapObjects
            List<SoapServiceProviderItem> objItems = new List<SoapServiceProviderItem>();
            foreach (ServiceProviderItem item in items)
                objItems.Add(SoapServiceProviderItem.Wrap(item));

            int attempt = 0;
            int ATTEMPTS = 3;
            while (attempt < ATTEMPTS)
            {
                // increment attempt
                attempt++;

                try
                {
                    // send packet for calculation
                    // invoke service provider
                    //TaskManager.Write(String.Format("{0} - Invoke GetServiceItemsDiskSpace method ('{1}' items) - {2} attempt",
                    //    DateTime.Now, objItems.Count, attempt));

                    ServiceProvider prov = new ServiceProvider();
                    ServiceProviderProxy.Init(prov, serviceId);
                    return prov.GetServiceItemsBandwidth(objItems.ToArray(), since);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex.ToString());
                }
            }

            throw new Exception("The number of attemtps has been reached. The package calculation has been aborted.");
        }

        private string BuildDiskBandwidthStatisticsXml(ServiceProviderItemBandwidth[] itemsBandwidth)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<items>");

			if (itemsBandwidth != null)
			{
				CultureInfo culture = CultureInfo.InvariantCulture;

				if (itemsBandwidth != null)
				{
                    foreach (ServiceProviderItemBandwidth item in itemsBandwidth)
                    {
                        if (item != null && item.Days != null)
                        {
                            foreach (DailyStatistics day in item.Days)
                            {
                                string dt = new DateTime(day.Year, day.Month, day.Day).ToString("MM/dd/yyyy", culture);
                                sb.Append("<item id=\"").Append(item.ItemId).Append("\"")
                                    .Append(" date=\"").Append(dt).Append("\"")
                                    .Append(" sent=\"").Append(day.BytesSent).Append("\"")
                                    .Append(" received=\"").Append(day.BytesReceived).Append("\"")
                                    .Append("></item>\n");
                            }
                        }
                    }
				}
			}

            sb.Append("</items>");
            return sb.ToString();
        }
    }
}
