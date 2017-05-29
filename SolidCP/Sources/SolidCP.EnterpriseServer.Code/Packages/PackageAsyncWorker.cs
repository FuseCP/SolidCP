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
using System.Threading;
using SolidCP.Providers;

namespace SolidCP.EnterpriseServer
{
    public class PackageAsyncWorker
    {
        private int userId = -1;
        private List<PackageInfo> packages = new List<PackageInfo>();
        private PackageStatus itemsStatus = PackageStatus.Active;

        #region Public properties
        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public System.Collections.Generic.List<PackageInfo> Packages
        {
            get { return this.packages; }
            set { this.packages = value; }
        }

        public PackageStatus ItemsStatus
        {
            get { return this.itemsStatus; }
            set { this.itemsStatus = value; }
        }
        #endregion

        #region Delete Packages Service Items
        public void DeletePackagesServiceItemsAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(DeletePackagesServiceItems));
            t.Start();
        }

        public void DeletePackagesServiceItems()
        {
            // impersonate thread
            if(userId != -1)
                SecurityContext.SetThreadPrincipal(userId);


            // delete package by package
            foreach (PackageInfo package in packages)
            {
                TaskManager.StartTask("SPACE", "DELETE_ITEMS", package.PackageName);
                TaskManager.WriteParameter("User", package.UserId);

                // get package service items
                List<ServiceProviderItem> items = PackageController.GetServiceItemsForStatistics(
                    0, package.PackageId, false, false, false, true); // disposable items

                // order items by service
                Dictionary<int, List<ServiceProviderItem>> orderedItems =
                    PackageController.OrderServiceItemsByServices(items);

                // delete service items by service sets
                foreach (int serviceId in orderedItems.Keys)
                {
                    
                    ServiceInfo service = ServerController.GetServiceInfo(serviceId);
                    //Delete Exchange Organization 
                    if (service.ProviderId == 103 /*Organizations*/)
                    {
                        OrganizationController.DeleteOrganization(orderedItems[serviceId][0].Id);
                        //int exchangeId = PackageController.GetPackageServiceId(package.PackageId, ResourceGroups.Exchange2007);
                        //ExchangeServerController.DeleteOrganization(orderedItems[serviceId][0].Id);                                                                                                
                    }
                    else                
                        ProcessServiceItems(false, false, serviceId, orderedItems[serviceId] );
                }

                // delete package from database
                DataProvider.DeletePackage(SecurityContext.User.UserId, package.PackageId);
            }

            // add log record
            TaskManager.CompleteTask();
        }
        #endregion

        #region Change Packages Service Items State
        public void ChangePackagesServiceItemsStateAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(ChangePackagesServiceItemsState));
            t.Start();
        }

        public void ChangePackagesServiceItemsState()
        {
            // impersonate thread
            if (userId != -1)
                SecurityContext.SetThreadPrincipal(userId);

            TaskManager.StartTask("SPACE", "CHANGE_ITEMS_STATUS");

            // collect required service items
            List<ServiceProviderItem> items = new List<ServiceProviderItem>();
            foreach (PackageInfo package in packages)
            {
                items.AddRange(PackageController.GetServiceItemsForStatistics(
                    0, package.PackageId, false, false, true, false)); // suspendable items
            }

            // order items by service
            Dictionary<int, List<ServiceProviderItem>> orderedItems =
                PackageController.OrderServiceItemsByServices(items);

            // process items
            bool enabled = (ItemsStatus == PackageStatus.Active);
            foreach (int serviceId in orderedItems.Keys)
            {               
                ProcessServiceItems(true, enabled, serviceId, orderedItems[serviceId]);
            }

            // add log record
            TaskManager.CompleteTask();
        }
        #endregion

        private void ProcessServiceItems(bool changeState, bool enabled,
            int serviceId, List<ServiceProviderItem> items)
        {
            string methodName = changeState ? "ChangeServiceItemsState" : "DeleteServiceItems";

            int PACKET_SIZE = 10;
            int ATTEMPTS = 3;

            TaskManager.Write(String.Format("Start analyze {0} service ({1} items)",
                serviceId, items.Count));

            try
            {
                // convert items to SoapObjects by packets of 0
                int startItem = 0;
                List<SoapServiceProviderItem> objItems = new List<SoapServiceProviderItem>();

                for (int i = 0; i < items.Count; i++)
                {
                    // add to the packet
                    objItems.Add(SoapServiceProviderItem.Wrap(items[i]));

                    if (((i > 0) && (i % PACKET_SIZE == 0))
                        || i == (items.Count - 1)) // packet is ready
                    {
                        if (objItems.Count == 0)
                            continue;

                        int attempt = 0;
                        bool success = false;
                        while (attempt < ATTEMPTS)
                        {
                            // increment attempt
                            attempt++;

                            try
                            {
                                // send packet for calculation
                                // invoke service provider
                                TaskManager.Write(String.Format("Invoke {0} method ('{1}' - '{2}' items) - {3} attempt",
                                    methodName, items[startItem].Name, items[i].Name, attempt));
                                
                                ServiceProvider prov = new ServiceProvider();
                                ServiceProviderProxy.Init(prov, serviceId);

                                if (changeState)
                                    prov.ChangeServiceItemsState(objItems.ToArray(), enabled);
                                else
                                    prov.DeleteServiceItems(objItems.ToArray());

                                // exit from the loop
                                success = true;
                                break;
                            }
                            catch (Exception ex)
                            {
                                TaskManager.WriteWarning(ex.ToString());
                            }
                        }

                        if (!success)
                            throw new Exception("The number of attemtps has been reached. The whole operation aborted.");

                        // reset packet counter
                        startItem = i + 1;
                        objItems.Clear();
                    }
                } // end for items
            }
            catch (Exception ex)
            {
                // log exception
                TaskManager.WriteWarning(ex.ToString());
            }

            TaskManager.Write(String.Format("End analyze {0} service ({1} items)",
                serviceId, items.Count));
        }
    }
}
