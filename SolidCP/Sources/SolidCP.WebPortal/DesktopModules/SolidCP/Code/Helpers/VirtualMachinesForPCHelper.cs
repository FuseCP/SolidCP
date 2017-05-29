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

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Virtualization;
using System.Web;
using System;

namespace SolidCP.Portal
{
    public class VirtualMachinesForPCHelper
    {
        public static bool IsVirtualMachineManagementAllowed(int packageId)
        {
            bool manageAllowed = false;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPSForPC_MANAGING_ALLOWED))
                manageAllowed = !cntx.Quotas[Quotas.VPSForPC_MANAGING_ALLOWED].QuotaExhausted;

            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                manageAllowed = true;
            else if (PanelSecurity.EffectiveUser.Role == UserRole.Reseller)
            {
                // check if the reseller is allowed to manage on its parent level
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package.UserId != PanelSecurity.EffectiveUserId)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(package.ParentPackageId);
                    if (cntx != null && cntx.Quotas.ContainsKey(Quotas.VPSForPC_MANAGING_ALLOWED))
                        manageAllowed = !cntx.Quotas[Quotas.VPSForPC_MANAGING_ALLOWED].QuotaExhausted;
                }
            }
            return manageAllowed;
        }

        public static VMInfo GetCachedVirtualMachineForPC(int itemId)
        {
            if (itemId == 0)
            {
                return new VMInfo();
            }

            string key = "CachedVirtualMachine" + itemId;
            if (HttpContext.Current.Items[key] != null)
                return (VMInfo)HttpContext.Current.Items[key];

            // load virtual machine
            VMInfo vm = ES.Services.VPSPC.GetVirtualMachineItem(itemId);

            // place to cache
            if (vm != null)
                HttpContext.Current.Items[key] = vm;

            vm.HostName = vm.HostName ?? String.Empty;

            return vm;
        }

        #region Virtual Machines
        VirtualMachineMetaItemsPaged vms;
        public VirtualMachineMetaItem[] GetVirtualMachines(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            vms = ES.Services.VPSPC.GetVirtualMachines(packageId, filterColumn, filterValue,
                    sortColumn, startRowIndex, maximumRows, true);
            return vms.Items;
        }

        public int GetVirtualMachinesCount(int packageId, string filterColumn, string filterValue)
        {
            if (vms == null)
            {
                vms = ES.Services.VPSPC.GetVirtualMachines(packageId, filterColumn, filterValue,
                        String.Empty, 0, 10, true);
            }

            return vms.Count;
        }
        #endregion

        #region Package IP Addresses
        PackageIPAddressesPaged packageAddresses;
        public PackageIPAddress[] GetPackageIPAddresses(int packageId, IPAddressPool pool, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            packageAddresses = ES.Services.Servers.GetPackageIPAddresses(packageId, 0, pool,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows, true);
            return packageAddresses.Items;
        }

        public int GetPackageIPAddressesCount(int packageId, IPAddressPool pool, string filterColumn, string filterValue)
        {
            return packageAddresses.Count;
        }
        #endregion

        #region Package Private IP Addresses
        PrivateIPAddressesPaged privateAddresses;
        public PrivateIPAddress[] GetPackagePrivateIPAddresses(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            privateAddresses = ES.Services.VPS.GetPackagePrivateIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);
            return privateAddresses.Items;
        }

        public int GetPackagePrivateIPAddressesCount(int packageId, string filterColumn, string filterValue)
        {
            return privateAddresses.Count;
        }
        #endregion

        #region Monitoring
        /// <summary>
        /// Get collection of MonitoredObjectEvent to selected VM
        /// </summary>
        /// <returns></returns>
        public MonitoredObjectEvent[] GetMonitoredObjectEvents()
        {
            return ES.Services.VPSPC.GetDeviceEvents(PanelRequest.ItemID);
        }

        /// <summary>
        /// Get collection of MonitoredObjectAlert to selected VM
        /// </summary>
        /// <returns></returns
        public MonitoredObjectAlert[] GetMonitoringAlerts()
        {
            return ES.Services.VPSPC.GetMonitoringAlerts(PanelRequest.ItemID);
        }                                         
        #endregion

    }
}
