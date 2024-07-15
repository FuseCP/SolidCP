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
﻿using System.Collections.Specialized;

namespace SolidCP.Portal
{
    public class VirtualMachines2012Helper
    {
        public static bool IsVirtualMachineManagementAllowed(int packageId)
        {
            bool manageAllowed = false;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_MANAGING_ALLOWED))
                manageAllowed = !cntx.Quotas[Quotas.VPS2012_MANAGING_ALLOWED].QuotaExhausted;

            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                manageAllowed = true;
            else if (PanelSecurity.EffectiveUser.Role == UserRole.Reseller)
            {
                // check if the reseller is allowed to manage on its parent level
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package.UserId != PanelSecurity.EffectiveUserId)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(package.ParentPackageId);
                    if (cntx != null && cntx.Quotas.ContainsKey(Quotas.VPS2012_MANAGING_ALLOWED))
                        manageAllowed = !cntx.Quotas[Quotas.VPS2012_MANAGING_ALLOWED].QuotaExhausted;
                }
            }
            return manageAllowed;
        }
        public static bool IsReinstallAllowed(int packageId)
        {
            bool reinstallAllowed = false;
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_REINSTALL_ALLOWED))
                reinstallAllowed = !cntx.Quotas[Quotas.VPS2012_REINSTALL_ALLOWED].QuotaExhausted;

            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                reinstallAllowed = true;
            else if (PanelSecurity.EffectiveUser.Role == UserRole.Reseller)
            {
                // check if the reseller is allowed to manage on its parent level
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package.UserId != PanelSecurity.EffectiveUserId)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(package.ParentPackageId);
                    if (cntx != null && cntx.Quotas.ContainsKey(Quotas.VPS2012_REINSTALL_ALLOWED))
                        reinstallAllowed = !cntx.Quotas[Quotas.VPS2012_REINSTALL_ALLOWED].QuotaExhausted;
                }
            }
            return reinstallAllowed;
        }

        // TODO: Move this method to the corresponding extension later.
        public static VirtualMachine GetCachedVirtualMachine(int itemId)
        {
            return VirtualMachinesExtensions.GetCachedVirtualMachine<VirtualMachine>(
                itemId, () => ES.Services.VPS2012.GetVirtualMachineItem(itemId));
        }

        #region Virtual Machines
        VirtualMachineMetaItemsPaged vms;
        public VirtualMachineMetaItem[] GetVirtualMachines(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            vms = ES.Services.VPS2012.GetVirtualMachines(packageId, filterColumn, filterValue,
                    sortColumn, startRowIndex, maximumRows, true);
            return vms.Items;
        }

        public int GetVirtualMachinesCount(int packageId, string filterColumn, string filterValue)
        {
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

        public PackageIPAddress[] GetPackageIPAddresses(int packageId, int orgId, IPAddressPool pool, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            packageAddresses = ES.Services.Servers.GetPackageIPAddresses(packageId, orgId, pool,
                filterColumn, filterValue, sortColumn, startRowIndex, maximumRows, true);
            return packageAddresses.Items;
        }

        public int GetPackageIPAddressesCount(int packageId, int orgId, IPAddressPool pool, string filterColumn, string filterValue)
        {
            return packageAddresses.Count;
        }
        #endregion

        #region Package Private Network VLANs
        PackageVLANsPaged packageVLANs;
        public PackageVLAN[] GetPackageVLANs(int packageId, bool isDmz, string sortColumn, int maximumRows, int startRowIndex)
        {
            if (isDmz)
            {
                packageVLANs = ES.Services.Servers.GetPackageDmzNetworkVLANs(packageId, sortColumn, startRowIndex, maximumRows);
            }
            else
            {
                packageVLANs = ES.Services.Servers.GetPackagePrivateNetworkVLANs(packageId, sortColumn, startRowIndex, maximumRows);
            }
            return packageVLANs.Items;
        }

        public int GetPackageVLANsCount(int packageId, bool isDmz)
        {
            return packageVLANs.Count;
        }
        #endregion

        #region Package Private IP Addresses
        PrivateIPAddressesPaged privateAddresses;
        public PrivateIPAddress[] GetPackagePrivateIPAddresses(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            privateAddresses = ES.Services.VPS2012.GetPackagePrivateIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);
            return privateAddresses.Items;
        }

        public int GetPackagePrivateIPAddressesCount(int packageId, string filterColumn, string filterValue)
        {
            return privateAddresses.Count;
        }
        #endregion

        #region Package DMZ IP Addresses
        DmzIPAddressesPaged dmzAddresses;
        public DmzIPAddress[] GetPackageDmzIPAddresses(int packageId, string filterColumn, string filterValue,
            string sortColumn, int maximumRows, int startRowIndex)
        {
            dmzAddresses = ES.Services.VPS2012.GetPackageDmzIPAddressesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRowIndex, maximumRows);
            return dmzAddresses.Items;
        }

        public int GetPackageDmzIPAddressesCount(int packageId, string filterColumn, string filterValue)
        {
            return dmzAddresses.Count;
        }
        #endregion

        public static StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }

        //public static bool IsReplicationEnabled(int packageId)
        //{
        //    var vmsMeta = (new VirtualMachines2012Helper()).GetVirtualMachines(packageId, null, null, null, 1, 0);
        //    if (vmsMeta.Length == 0) return false;

        //    var packageVm = ES.Services.VPS2012.GetVirtualMachineItem(vmsMeta[0].ItemID);
        //    if (packageVm == null) return false;

        //    var serviceSettings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettings(packageVm.ServiceId));
        //    if (serviceSettings == null) return false;

        //    return serviceSettings["ReplicaMode"] == ReplicaMode.ReplicationEnabled.ToString();
        //}
    }
}
