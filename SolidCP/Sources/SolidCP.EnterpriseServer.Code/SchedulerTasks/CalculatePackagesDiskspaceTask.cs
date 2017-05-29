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
using SolidCP.EnterpriseServer.Code.SharePoint;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.SharePoint;

namespace SolidCP.EnterpriseServer
{
    public class CalculatePackagesDiskspaceTask : SchedulerTask
    {
        private readonly bool suspendOverused = false;

        public override void DoWork()
        {
            // Input parameters:
            //  - SUSPEND_OVERUSED_PACKAGES

            CalculateDiskspace();
        }

        public void CalculateDiskspace()
        {
            // get all owned packages
            List<PackageInfo> packages = PackageController.GetPackagePackages(TaskManager.TopTask.PackageId, true);
            TaskManager.Write("Packages to calculate: " + packages.Count.ToString());

            foreach (PackageInfo package in packages)
            {
                // calculating package diskspace
                CalculatePackage(package.PackageId);
            }
        }

        public void CalculatePackage(int packageId)
        {
            try
            {
                // get all package items
                List<ServiceProviderItem> items = PackageController.GetServiceItemsForStatistics(
                    0, packageId, true, false, false, false);

                //TaskManager.Write("Items: " + items.Count);

                // order items by service
                Dictionary<int, List<ServiceProviderItem>> orderedItems =
                    PackageController.OrderServiceItemsByServices(items);

                // calculate statistics for each service set
                List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
                foreach (int serviceId in orderedItems.Keys)
                {
                    ServiceProviderItemDiskSpace[] serviceDiskspace = CalculateItems(serviceId, orderedItems[serviceId]);
                    if (serviceDiskspace != null)
                        itemsDiskspace.AddRange(serviceDiskspace);
                }

                // update info in the database
                string xml = BuildDiskSpaceStatisticsXml(itemsDiskspace.ToArray());
                PackageController.UpdatePackageDiskSpace(packageId, xml);
                //TaskManager.Write("XML: " + xml);

                // suspend package if requested
                if (suspendOverused)
                {
                    // disk space
                    QuotaValueInfo dsQuota = PackageController.GetPackageQuota(packageId, Quotas.OS_DISKSPACE);

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
                TaskManager.WriteError(String.Format("Error calculating diskspace for '{0}' space of user '{1}': {2}",
                    package.PackageName, user.Username, ex.ToString()));
            }
        }

        private static int GetExchangeServiceID(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.Exchange);
        }


        public ServiceProviderItemDiskSpace[] CalculateItems(int serviceId, List<ServiceProviderItem> items)
        {
            // convert items to SoapObjects
            List<SoapServiceProviderItem> objItems = new List<SoapServiceProviderItem>();
            
            //hack for organization... Refactoring!!!

           
            List<ServiceProviderItemDiskSpace> organizationDiskSpaces = new List<ServiceProviderItemDiskSpace>();
            foreach (ServiceProviderItem item in items)
            {
                long size = 0;
                if (item is Organization)
                {
                    Organization org = (Organization) item;

                    //Exchange DiskSpace
                    if (!string.IsNullOrEmpty(org.GlobalAddressList))
                    {
                        int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                        if (exchangeServiceId > 0)
                        {
                            ServiceProvider exchangeProvider = ExchangeServerController.GetExchangeServiceProvider(exchangeServiceId, item.ServiceId);

                            SoapServiceProviderItem soapOrg = SoapServiceProviderItem.Wrap(org);
                            ServiceProviderItemDiskSpace[] itemsDiskspace =
                                exchangeProvider.GetServiceItemsDiskSpace(new SoapServiceProviderItem[] { soapOrg });

                            if (itemsDiskspace != null && itemsDiskspace.Length > 0)
                            {
                                size += itemsDiskspace[0].DiskSpace;
                            }
                        }
                    }

                    // Crm DiskSpace
                    if (org.CrmOrganizationId != Guid.Empty)
                    {
                        //CalculateCrm DiskSpace
                    }

                    //SharePoint DiskSpace

                    int res;

                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

                    if (cntx.Groups.ContainsKey(ResourceGroups.SharepointFoundationServer))
                    {
                        SharePointSiteDiskSpace[] sharePointSiteDiskSpaces =
                            HostedSharePointServerController.CalculateSharePointSitesDiskSpace(org.Id, out res);
                        if (res == 0)
                        {
                            foreach (SharePointSiteDiskSpace currecnt in sharePointSiteDiskSpaces)
                            {
                                size += currecnt.DiskSpace;
                            }
                        }
                    }

                    if (cntx.Groups.ContainsKey(ResourceGroups.SharepointEnterpriseServer))
                    {
                        SharePointSiteDiskSpace[] sharePointSiteDiskSpaces =
                            HostedSharePointServerEntController.CalculateSharePointSitesDiskSpace(org.Id, out res);
                        if (res == 0)
                        {
                            foreach (SharePointSiteDiskSpace currecnt in sharePointSiteDiskSpaces)
                            {
                                size += currecnt.DiskSpace;
                            }
                        }
                    }

                    ServiceProviderItemDiskSpace tmp = new ServiceProviderItemDiskSpace();
                    tmp.ItemId = item.Id;
                    tmp.DiskSpace = size;
                    organizationDiskSpaces.Add(tmp);
                }
                else
                    objItems.Add(SoapServiceProviderItem.Wrap(item));
            }
            
            
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

                    if (objItems.Count > 0)
                    {
                        ServiceProvider prov = new ServiceProvider();
                        ServiceProviderProxy.Init(prov, serviceId);
                        ServiceProviderItemDiskSpace[] itemsDiskSpace = prov.GetServiceItemsDiskSpace(objItems.ToArray());
                        if (itemsDiskSpace != null && itemsDiskSpace.Length > 0)
                            organizationDiskSpaces.AddRange(itemsDiskSpace);
                    }
                    
                    return organizationDiskSpaces.ToArray();
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex.ToString());
                }
            }

            throw new Exception("The number of attemtps has been reached. The package calculation has been aborted.");
        }

        private string BuildDiskSpaceStatisticsXml(ServiceProviderItemDiskSpace[] itemsDiskspace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<items>");

			if (itemsDiskspace != null)
			{
				foreach (ServiceProviderItemDiskSpace item in itemsDiskspace)
				{
					sb.Append("<item id=\"").Append(item.ItemId).Append("\"")
						.Append(" bytes=\"").Append(item.DiskSpace).Append("\"></item>\n");
				}
			}

            sb.Append("</items>");
            return sb.ToString();
        }
    }
}
