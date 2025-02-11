// Copyright (c) 2019, SolidCP
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

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Common;
using SolidCP.EnterpriseServer;
﻿using SolidCP.Portal.Code.Helpers;
using System.IO;

namespace SolidCP.Portal.VPS2012
{
    public partial class VpsDetailsEditConfiguration : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
            if (!manageAllowed) //block access for user if they don't have permission.
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));

            secHddQOS.Visible = QOSManag.Visible = PanelSecurity.EffectiveUser.Role != UserRole.User;

            // check snapshots
            VirtualMachineSnapshot[] snapshots = ES.Services.VPS2012.GetVirtualMachineSnapshots(PanelRequest.ItemID);
            if (snapshots.Length > 0)
            {
                messageBox.ShowWarningMessage("VPS_CHANGE_VM_CONFIGURATION_SNAPSHOT");
                btnUpdate.Enabled = false;
            }
            else
            {
                btnUpdate.Enabled = true;
            }

            if (!IsPostBack)
            {
                BindConfiguration();
            }
        }

        private void BindConfiguration()
        {
            VirtualMachine vm = null;

            try
            {
                // load machine
                vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);

                if (vm == null)
                {
                    messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM");
                    return;
                }

                // bind CPU cores
                int maxCores = ES.Services.VPS2012.GetMaximumCpuCoresNumber(vm.PackageId);
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                QuotaValueInfo cpuQuota2 = cntx.Quotas[Quotas.VPS2012_CPU_NUMBER];
                int cpuQuotausable = (cpuQuota2.QuotaAllocatedValue - cpuQuota2.QuotaUsedValue) + vm.CpuCores;

                // add VpsMaxCoresPerVM key to web.config appsettings section for a different cpu assignment behavior to take effect
                string vpsMaxCoresPerVM = WebConfigurationManager.AppSettings.Get("VpsMaxCoresPerVM");
                bool showCpuLabel = false;

                if (!String.IsNullOrEmpty(vpsMaxCoresPerVM))
                {
                    maxCores = Int32.Parse(vpsMaxCoresPerVM);
                    showCpuLabel = true;
                }

                string cpuRange = "0";
                int cpuAvailable = 0;

                if (maxCores > 0)
                {
                    if (cpuQuota2.QuotaAllocatedValue == -1)
                    {
                        cpuAvailable = maxCores;
                    }
                    else if (cpuQuota2.QuotaAllocatedValue > 0)
                    {
                        if (cpuQuota2.QuotaAllocatedValue > (cpuQuota2.QuotaUsedValue - vm.CpuCores))
                        {
                            if (cpuQuotausable > maxCores)
                            {
                                cpuAvailable = maxCores;
                            }
                            else
                            {
                                cpuAvailable = cpuQuotausable;
                            }
                        }
                    }
                }

                int cpuDisplayed = cpuAvailable;

                if (cpuDisplayed < vm.CpuCores)
                {
                    cpuDisplayed = vm.CpuCores;
                }

                if (cpuDisplayed <= 0)
                {
                    ddlCpu.Items.Add("0");
                }
                else
                {
                    for (int i = 1; i <= cpuDisplayed; i++)
                        ddlCpu.Items.Add(i.ToString());

                    if (cpuAvailable == 1) { cpuRange = "1"; } else { cpuRange = "1 - " + cpuAvailable; }
                }

                valCoresStatus.Text = String.Format(GetLocalizedString("valCoresStatus.Text"), cpuRange);
                valCoresStatus.Visible = showCpuLabel;

                // RAM
                if (cntx.Quotas.ContainsKey(Quotas.VPS2012_RAM))
                {

                    string vpsMinRamPerVM = WebConfigurationManager.AppSettings.Get("VpsMinRamPerVM");
                    string vpsMaxRamPerVM = WebConfigurationManager.AppSettings.Get("VpsMaxRamPerVM");

                    int minRam = 0;
                    int maxRam = 0;
                    string ramRange = "0";
                    bool showRamLabel = false;

                    if (!String.IsNullOrEmpty(vpsMinRamPerVM) && !String.IsNullOrEmpty(vpsMaxRamPerVM))
                    {
                        minRam = Int32.Parse(vpsMinRamPerVM);
                        maxRam = Int32.Parse(vpsMaxRamPerVM);
                        showRamLabel = true;
                    }

                    QuotaValueInfo ramQuota = cntx.Quotas[Quotas.VPS2012_RAM];

                    if (ramQuota.QuotaAllocatedValue == -1)
                    {
                        if (!showRamLabel)
                        {
                            ramRange = "unlimited";
                        }
                        else
                        {
                            ramRange = minRam + " - " + maxRam;
                        }
                    }
                    else if (ramQuota.QuotaAllocatedValue > 0)
                    {

                        if (ramQuota.QuotaAllocatedValue > (ramQuota.QuotaUsedValue - vm.RamSize))
                        {

                            int availSize = ramQuota.QuotaAllocatedValue - ramQuota.QuotaUsedValue + vm.RamSize;
                            int ramRemaining = 0;

                            if (availSize > 0)
                            {

                                if (!showRamLabel)
                                {
                                    ramRemaining = availSize;
                                }
                                else
                                {

                                    if (availSize >= maxRam)
                                    {
                                        ramRemaining = maxRam;
                                    }
                                    else if (availSize < maxRam && availSize >= minRam)
                                    {
                                        ramRemaining = availSize;
                                    }

                                }
                            }

                            if (ramRemaining > 0)
                            {
                                ramRange = minRam + " - " + ramRemaining;
                            }

                        }
                    }

                    valRamStatus.Text = String.Format(GetLocalizedString("valRamStatus.Text"), ramRange);
                    valRamStatus.Visible = showRamLabel;

                }

                // HDD
                if (cntx.Quotas.ContainsKey(Quotas.VPS2012_HDD))
                {

                    string vpsMinHddPerVM = WebConfigurationManager.AppSettings.Get("VpsMinHddPerVM");
                    string vpsMaxHddPerVM = WebConfigurationManager.AppSettings.Get("VpsMaxHddPerVM");

                    int minHdd = 0;
                    int maxHdd = 0;
                    //int maxHddAdds = 0;

                    int hddAddsSize = 0;
                    int hddTotalSize = 0;

                    string hddRange = "0";
                    string hddAdds = "0";
                    string hddTotal = "0";

                    bool showHddLabel = false;

                    if (!String.IsNullOrEmpty(vpsMinHddPerVM) && !String.IsNullOrEmpty(vpsMaxHddPerVM))
                    {
                        minHdd = Int32.Parse(vpsMinHddPerVM);
                        maxHdd = Int32.Parse(vpsMaxHddPerVM);
                        showHddLabel = true;
                    }

                    QuotaValueInfo hddQuota = cntx.Quotas[Quotas.VPS2012_HDD];
                    if (hddQuota.QuotaAllocatedValue == -1)
                    {

                        if (!showHddLabel)
                        {
                            hddRange = "unlimited";
                            hddAdds = "unlimited";
                        }
                        else
                        {
                            if (vm.HddSize[0] >= maxHdd)
                            {
                                hddRange = vm.HddSize[0].ToString();
                            }
                            else
                            {
                                hddRange = vm.HddSize[0] + " - " + maxHdd;
                            }

                            hddAdds = "1 - " + maxHdd;
                        }

                        hddTotal = "unlimited";

                    }
                    else
                    {

                        if (vm.HddSize.Length > 1)
                        {
                            for (int i = 1; i < vm.HddSize.Length; i++)
                            {
                                if (vm.HddSize[i] == 0 || String.IsNullOrEmpty(vm.VirtualHardDrivePath[i])) continue;
                                hddAddsSize += vm.HddSize[i];
                            }
                        }

                        hddTotalSize = vm.HddSize[0] + hddAddsSize;

                        if (hddQuota.QuotaAllocatedValue > 0)
                        {

                            if (hddQuota.QuotaAllocatedValue < hddQuota.QuotaUsedValue)
                            {

                                int hddQuotaUsed = hddQuota.QuotaUsedValue - hddTotalSize;

                                if (hddQuota.QuotaAllocatedValue > (hddQuotaUsed + hddAddsSize))
                                {
                                    int newMaxSize = hddQuota.QuotaAllocatedValue - hddQuotaUsed + hddAddsSize;
                                    newMaxSize = newMaxSize > maxHdd ? maxHdd : newMaxSize;
                                    hddRange = newMaxSize.ToString();
                                }

                                if (hddQuota.QuotaAllocatedValue > hddQuotaUsed)
                                {
                                    int newTotalSize = hddQuota.QuotaAllocatedValue - hddQuotaUsed;
                                    hddTotal = newTotalSize.ToString();
                                }

                            }
                            else if (hddQuota.QuotaAllocatedValue == hddQuota.QuotaUsedValue)
                            {
                                hddRange = vm.HddSize[0].ToString();
                                hddTotal = hddTotalSize.ToString();
                            }
                            else
                            {
                                int hddRemaining = hddQuota.QuotaAllocatedValue - hddQuota.QuotaUsedValue;
                                int hddMaxLimit;

                                if (vm.HddSize[0] >= maxHdd)
                                {
                                    hddRange = vm.HddSize[0].ToString();
                                }
                                else
                                {
                                    hddMaxLimit = vm.HddSize[0] + hddRemaining;
                                    hddMaxLimit = hddMaxLimit > maxHdd ? maxHdd : hddMaxLimit;
                                    hddRange = vm.HddSize[0] + " - " + hddMaxLimit;
                                }

                                int hddAddsMaxLimit = hddRemaining > maxHdd ? maxHdd : hddRemaining;
                                hddAdds = "1 - " + hddAddsMaxLimit;

                                int hddTotalMaxLimit = hddRemaining + hddTotalSize;
                                hddTotal = hddTotalMaxLimit.ToString();

                            }
                        }

                    }

                    valHddStatus.Text = String.Format(GetLocalizedString("valHddStatus.Text"), hddRange, hddAdds, hddTotal);
                    valHddStatus.Visible = showHddLabel;

                }

                // bind item
                ddlCpu.SelectedValue = vm.CpuCores.ToString();
                txtRam.Text = vm.RamSize.ToString();
                txtHdd.Text = vm.HddSize[0].ToString();
                hiddenTxtValHdd.Value = vm.HddSize[0].ToString();
                BindAdditionalHdd(vm);
                txtHddMinIOPS.Text = vm.HddMinimumIOPS.ToString();
                txtHddMaxIOPS.Text = vm.HddMaximumIOPS.ToString();
                txtSnapshots.Text = vm.SnapshotsNumber.ToString();

                chkDvdInstalled.Checked = vm.DvdDriveInstalled;
                chkBootFromCd.Checked = vm.BootFromCD;
                chkNumLock.Checked = vm.NumLockEnabled;
                chkSecureBoot.Checked = vm.EnableSecureBoot;
                if (vm.Generation == 1)
                {
                    chkSecureBoot.Checked = false;
                    chkSecureBoot.Enabled = false;
                }

                chkStartShutdown.Checked = vm.StartTurnOffAllowed;
                chkPauseResume.Checked = vm.PauseResumeAllowed;
                chkReset.Checked = vm.ResetAllowed;
                chkReboot.Checked = vm.RebootAllowed;
                chkReinstall.Checked = vm.ReinstallAllowed;

                chkExternalNetworkEnabled.Checked = vm.ExternalNetworkEnabled;
                chkPrivateNetworkEnabled.Checked = vm.PrivateNetworkEnabled;
                chkDmzNetworkEnabled.Checked = vm.DmzNetworkEnabled;

                chkIgnoreHddWarning.Visible = (PanelSecurity.EffectiveUser.Role != UserRole.User);

                // other quotas
                BindCheckboxOption(chkDvdInstalled, Quotas.VPS2012_DVD_ENABLED);
                chkBootFromCd.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_BOOT_CD_ALLOWED);

                BindCheckboxOption(chkStartShutdown, Quotas.VPS2012_START_SHUTDOWN_ALLOWED);
                BindCheckboxOption(chkPauseResume, Quotas.VPS2012_PAUSE_RESUME_ALLOWED);
                BindCheckboxOption(chkReset, Quotas.VPS2012_RESET_ALOWED);
                BindCheckboxOption(chkReboot, Quotas.VPS2012_REBOOT_ALLOWED);
                BindCheckboxOption(chkReinstall, Quotas.VPS2012_REINSTALL_ALLOWED);

                BindCheckboxOption(chkExternalNetworkEnabled, Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED);
                if (chkExternalNetworkEnabled.Enabled && !chkExternalNetworkEnabled.Checked)
                {
                    PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);
                    if (ips.Length == 0)
                    {
                        chkExternalNetworkEnabled.Enabled = false;
                        EmptyExternalAddressesMessage.Visible = true;
                    }
                }
                BindCheckboxOption(chkPrivateNetworkEnabled, Quotas.VPS2012_PRIVATE_NETWORK_ENABLED);
                BindCheckboxOption(chkDmzNetworkEnabled, Quotas.VPS2012_DMZ_NETWORK_ENABLED);

                this.BindSettingsControls(vm);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM", ex);
            }
        }

        private void BindCheckboxOption(CheckBox chk, string quotaName)
        {
            chk.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, quotaName);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectBack("cancel");
        }

        private void RedirectBack(string action)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_config",
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "action=" + action));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // check rights
                bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                if (!manageAllowed)
                {
                    return;
                }

                // check snapshots
                VirtualMachineSnapshot[] snapshots = ES.Services.VPS2012.GetVirtualMachineSnapshots(PanelRequest.ItemID);
                if (snapshots.Length > 0)
                {
                    return;
                }

                VirtualMachine virtualMachine = new VirtualMachine();
                VirtualMachine vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);

                if (!chkIgnoreHddWarning.Checked || PanelSecurity.EffectiveUser.Role == UserRole.User)
                {
                    if (Utils.ParseInt(hiddenTxtValHdd.Value) > Utils.ParseInt(txtHdd.Text.Trim()))
                    {
                        messageBox.ShowWarningMessage("VPS_CHANGE_HDD_SIZE");
                        return;
                    }
                    List<AdditionalHdd> hdds = GetAdditionalHdd();
                    foreach (AdditionalHdd hdd in hdds)
                    {
                        for (int i = 0; i < vm.VirtualHardDrivePath.Length; i++)
                        {
                            if (String.IsNullOrEmpty(vm.VirtualHardDrivePath[i])) continue;
                            if (Path.GetFileName(vm.VirtualHardDrivePath[i]).ToLower().Equals(Path.GetFileName(hdd.DiskPath).ToLower()) && hdd.DiskSize < vm.HddSize[i])
                            {
                                messageBox.ShowWarningMessage("VPS_CHANGE_HDD_SIZE");
                                return;
                            }
                        }
                    }
                }

                // the custom provider control
                this.SaveSettingsControls(ref virtualMachine);
                virtualMachine.CpuCores = Utils.ParseInt(ddlCpu.SelectedValue);
                virtualMachine.RamSize = Utils.ParseInt(txtRam.Text.Trim());
                List<int> hddSize = new List<int>();
                List<String> hddPath = new List<String>();
                hddSize.Add(Utils.ParseInt(txtHdd.Text.Trim()));
                hddPath.Add(vm.VirtualHardDrivePath[0]);
                List<AdditionalHdd> additionalHdd = GetAdditionalHdd();
                foreach (AdditionalHdd hdd in additionalHdd)
                {
                    int size = hdd.DiskSize;
                    if (size > 0)
                    {
                        hddSize.Add(size);
                        hddPath.Add(hdd.DiskPath);
                    }
                }
                virtualMachine.HddSize = hddSize.ToArray();
                virtualMachine.VirtualHardDrivePath = hddPath.ToArray();
                virtualMachine.SnapshotsNumber = Utils.ParseInt(txtSnapshots.Text.Trim());
                virtualMachine.HddMinimumIOPS = Utils.ParseInt(txtHddMinIOPS.Text.Trim());
                virtualMachine.HddMaximumIOPS = Utils.ParseInt(txtHddMaxIOPS.Text.Trim());
                virtualMachine.DvdDriveInstalled = chkDvdInstalled.Checked;
                virtualMachine.BootFromCD = chkBootFromCd.Checked;
                virtualMachine.NumLockEnabled = chkNumLock.Checked;
                virtualMachine.EnableSecureBoot = chkSecureBoot.Checked;
                virtualMachine.StartTurnOffAllowed = chkStartShutdown.Checked;
                virtualMachine.PauseResumeAllowed = chkPauseResume.Checked;
                virtualMachine.RebootAllowed = chkReboot.Checked;
                virtualMachine.ResetAllowed = chkReset.Checked;
                virtualMachine.ReinstallAllowed = chkReinstall.Checked;
                virtualMachine.ExternalNetworkEnabled = chkExternalNetworkEnabled.Checked;
                virtualMachine.PrivateNetworkEnabled = chkPrivateNetworkEnabled.Checked;
                virtualMachine.DmzNetworkEnabled = chkDmzNetworkEnabled.Checked;
                virtualMachine.NeedReboot = chkForceReboot.Checked;
                virtualMachine.defaultaccessvlan = vm.defaultaccessvlan;
                virtualMachine.PrivateNetworkVlan = vm.PrivateNetworkVlan;
                virtualMachine.DmzNetworkVlan = vm.DmzNetworkVlan;

                bool setupExternalNetwork = !vm.ExternalNetworkEnabled && chkExternalNetworkEnabled.Checked;
                bool setupPrivateNetwork = !vm.PrivateNetworkEnabled && chkPrivateNetworkEnabled.Checked;
                bool setupDmzNetwork = !vm.DmzNetworkEnabled && chkDmzNetworkEnabled.Checked;
                int[] ipId = new int[1];
                int privAdrCount = 0;
                int dmzAdrCount = 0;

                if (setupExternalNetwork)
                {
                    PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);
                    if (ips.Length > 0)
                    {
                        virtualMachine.defaultaccessvlan = ips[0].VLAN;
                        ipId[0] = ips[0].PackageAddressID;
                    }
                }

                if (setupPrivateNetwork)
                {
                    PackageVLANsPaged vlans = ES.Services.Servers.GetPackagePrivateNetworkVLANs(PanelSecurity.PackageId, "", 0, Int32.MaxValue);
                    if (vlans.Count > 0)
                    {
                        virtualMachine.PrivateNetworkVlan = vlans.Items[0].Vlan;
                    }

                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                    if (cntx.Quotas.ContainsKey(Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER))
                    {
                        QuotaValueInfo privQuota = cntx.Quotas[Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER];
                        if (privQuota.QuotaAllocatedValue > 0 || privQuota.QuotaAllocatedValue == -1) privAdrCount = 1;
                    }
                }

                if (setupDmzNetwork)
                {
                    PackageVLANsPaged vlans = ES.Services.Servers.GetPackageDmzNetworkVLANs(PanelSecurity.PackageId, "", 0, Int32.MaxValue);
                    if (vlans.Count > 0)
                    {
                        virtualMachine.DmzNetworkVlan = vlans.Items[0].Vlan;
                    }

                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                    if (cntx.Quotas.ContainsKey(Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER))
                    {
                        QuotaValueInfo dmzQuota = cntx.Quotas[Quotas.VPS2012_DMZ_IP_ADDRESSES_NUMBER];
                        if (dmzQuota.QuotaAllocatedValue > 0 || dmzQuota.QuotaAllocatedValue == -1) dmzAdrCount = 1;
                    }
                }

                ResultObject res = ES.Services.VPS2012.UpdateVirtualMachineResource(PanelRequest.ItemID, virtualMachine);

                if (res.IsSuccess)
                {
                    if (setupExternalNetwork && ipId[0] != 0)
                    {
                        ES.Services.VPS2012.AddVirtualMachineExternalIPAddresses(PanelRequest.ItemID, false, 1, ipId);
                    }

                    if (setupPrivateNetwork && privAdrCount > 0)
                    {
                        ES.Services.VPS2012.AddVirtualMachinePrivateIPAddresses(PanelRequest.ItemID, true, privAdrCount, new string[0], false, null, null, null, null);
                    }

                    if (setupDmzNetwork && dmzAdrCount > 0)
                    {
                        ES.Services.VPS2012.AddVirtualMachineDmzIPAddresses(PanelRequest.ItemID, true, dmzAdrCount, new string[0], false, null, null, null, null);
                    }

                    // redirect back
                    RedirectBack("changed");
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_CHANGE_VM_CONFIGURATION", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_CHANGE_VM_CONFIGURATION", ex);
            }
        }

        protected void btnAddHdd_Click(object sender, EventArgs e)
        {
            var hdds = GetAdditionalHdd();
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            int freeHddGb = 0;
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_HDD))
            {
                QuotaValueInfo hddQuota = cntx.Quotas[Quotas.VPS2012_HDD];
                if (hddQuota.QuotaAllocatedValue != -1)
                {
                    int availSize = hddQuota.QuotaAllocatedValue - hddQuota.QuotaUsedValue;
                    freeHddGb = availSize < 0 ? 0 : availSize;
                    foreach (AdditionalHdd hdd in hdds)
                    {
                        if (hdd.DiskSize > 0 && String.IsNullOrEmpty(hdd.DiskPath)) freeHddGb -= hdd.DiskSize;
                    }
                }
            }
            hdds.Add(new AdditionalHdd(freeHddGb, ""));
            RebindAdditionalHdd(hdds);
        }

        protected void btnRemoveHdd_OnCommand(object sender, CommandEventArgs e)
        {
            var hdds = GetAdditionalHdd();
            hdds.RemoveAt(Convert.ToInt32(e.CommandArgument));
            RebindAdditionalHdd(hdds);
        }

        private void BindAdditionalHdd(VirtualMachine vm)
        {
            CheckAdditionalHddQuota(vm.HddSize.Length - 1);
            List<AdditionalHdd> result = new List<AdditionalHdd>();
            if (vm.HddSize.Length > 1)
            {
                for (int i = 1; i < vm.HddSize.Length; i++)
                {
                    if (vm.HddSize[i] == 0 || String.IsNullOrEmpty(vm.VirtualHardDrivePath[i])) continue;
                    AdditionalHdd hdd = new AdditionalHdd(vm.HddSize[i], vm.VirtualHardDrivePath[i]);
                    result.Add(hdd);
                }
            }
            repHdd.DataSource = result;
            repHdd.DataBind();
        }

        private void RebindAdditionalHdd(List<AdditionalHdd> hdd)
        {
            CheckAdditionalHddQuota(hdd.Count);
            repHdd.DataSource = hdd;
            repHdd.DataBind();
        }

        private void CheckAdditionalHddQuota(int currCount)
        {
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_ADDITIONAL_VHD_COUNT))
            {
                QuotaValueInfo additionalHddQuota = cntx.Quotas[Quotas.VPS2012_ADDITIONAL_VHD_COUNT];
                int quotaHddCount = additionalHddQuota.QuotaAllocatedValue;
                int maxHddCount;
                VirtualMachine vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);
                if (vm != null && vm.Generation > 1)
                {
                    maxHddCount = 62;
                }
                else
                {
                    maxHddCount = 2;
                }
                if (quotaHddCount == -1 || quotaHddCount > maxHddCount) quotaHddCount = maxHddCount;
                btnAddHdd.Enabled = (currCount < quotaHddCount);
            }
            else
            {
                btnAddHdd.Enabled = false;
            }
        }

        private List<AdditionalHdd> GetAdditionalHdd()
        {
            var result = new List<AdditionalHdd>();

            foreach (RepeaterItem item in repHdd.Items)
            {
                AdditionalHdd hdd = new AdditionalHdd(Utils.ParseInt(GetTextBoxText(item, "txtAdditionalHdd")), GetHiddenFieldValue(item, "txtAdditionalHddPath"));
                result.Add(hdd);
            }

            return result;
        }

        private string GetTextBoxText(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as TextBox).Text;
        }

        private string GetHiddenFieldValue(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as HiddenField).Value;
        }
    }
}
