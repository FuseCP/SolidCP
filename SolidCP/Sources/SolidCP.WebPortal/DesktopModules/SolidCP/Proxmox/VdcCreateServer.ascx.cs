﻿// Copyright (c) 2017, centron GmbH
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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Portal.Code.Helpers;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.Proxmox
{
    public partial class VdcCreate : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFormControls();
            }

            // remove non-required steps
            ToggleWizardSteps();

            // toggle
            ToggleControls();
        }

        private void ToggleWizardSteps()
        {
            // external network
            if (!PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.PROXMOX_EXTERNAL_NETWORK_ENABLED))
            {
                wizard.WizardSteps.Remove(stepExternalNetwork);
                //chkExternalNetworkEnabled.Checked = false;
            }

            // private network
            if (!PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.PROXMOX_PRIVATE_NETWORK_ENABLED))
            {
                wizard.WizardSteps.Remove(stepPrivateNetwork);
                chkPrivateNetworkEnabled.Checked = false;
            }
        }


        private void BindFormControls()
        {
            var virtualMachine = new VirtualMachine
            {
                DynamicMemory = new DynamicMemory
                {
                    Buffer = 20,
                    Priority = 50
                }
            };

            // bind password policy
            password.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.VPS_POLICY, "AdministratorPasswordPolicy");

            // OS templates
            listOperatingSystems.DataSource = ES.Services.Proxmox.GetOperatingSystemTemplates(PanelSecurity.PackageId);
            listOperatingSystems.DataBind();
            listOperatingSystems.Items.Insert(0, new ListItem(GetLocalizedString("SelectOsTemplate.Text"), ""));

            // summary letter e-mail
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package != null)
            {
                UserInfo user = ES.Services.Users.GetUserById(package.UserId);
                if (user != null)
                {
                    chkSendSummary.Checked = true;
                    txtSummaryEmail.Text = user.Email;
                }
            }

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            int maxCores = ES.Services.Proxmox.GetMaximumCpuCoresNumber(PanelSecurity.PackageId);

            if (cntx.Quotas.ContainsKey(Quotas.PROXMOX_CPU_NUMBER))
            {
                QuotaValueInfo cpuQuota = cntx.Quotas[Quotas.PROXMOX_CPU_NUMBER];

                if (cpuQuota.QuotaAllocatedValue != -1
                    && maxCores > cpuQuota.QuotaAllocatedValue)
                    maxCores = cpuQuota.QuotaAllocatedValue;
            }

            for (int i = 1; i < maxCores + 1; i++)
                ddlCpu.Items.Add(i.ToString());

            ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item


            // external network details
            if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.PROXMOX_EXTERNAL_NETWORK_ENABLED))
            {


                // bind vlan list
                PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);

                List<int> dupevlans = new List<int>();
                List<int> vlans = new List<int>();
                foreach (PackageIPAddress ip in ips)
                {
                    dupevlans.Add(ip.VLAN);
                }

                // return vlan list without dupes
                vlans = dupevlans.Distinct().ToList();

                //List<int> vlans = ES.Services.Proxmox.GetAvailableVLANs(PanelSecurity.PackageId).vlans;
                listVlanLists.Items.Clear();
                listVlanLists.Items.Add(new ListItem("External network disabled", "-1"));
                foreach (int vlan in vlans)
                {
                    listVlanLists.Items.Add(new ListItem(String.Format("VLAN {0}", vlan.ToString()), vlan.ToString()));
                }

                // bind external network ips 4 selected vlan
                BindExternalIps();
            }


            // private network
            if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.PROXMOX_PRIVATE_NETWORK_ENABLED))
            {
                NetworkAdapterDetails nic = ES.Services.Proxmox.GetPrivateNetworkDetails(PanelSecurity.PackageId);
                litPrivateNetworkFormat.Text = nic.NetworkFormat;
                litPrivateSubnetMask.Text = nic.SubnetMask;

                // set max number
                QuotaValueInfo privQuota = cntx.Quotas[Quotas.PROXMOX_PRIVATE_IP_ADDRESSES_NUMBER];
                int maxPrivate = privQuota.QuotaAllocatedValue;
                if (maxPrivate == -1)
                    maxPrivate = 10;

                // handle DHCP mode
                if (nic.IsDHCP)
                {
                    maxPrivate = 0;
                    ViewState["DHCP"] = true;
                }

                txtPrivateAddressesNumber.Text = "1";
                litMaxPrivateAddresses.Text = String.Format(GetLocalizedString("litMaxPrivateAddresses.Text"), maxPrivate);
            }

            // RAM size
            if (cntx.Quotas.ContainsKey(Quotas.PROXMOX_RAM))
            {
                QuotaValueInfo ramQuota = cntx.Quotas[Quotas.PROXMOX_RAM];
                if (ramQuota.QuotaAllocatedValue == -1)
                {
                    // unlimited RAM
                    txtRam.Text = "";
                }
                else
                {
                    int availSize = ramQuota.QuotaAllocatedValue - ramQuota.QuotaUsedValue;
                    txtRam.Text = availSize < 0 ? "" : availSize.ToString();

                    if (availSize > 0)
                    {
                        virtualMachine.DynamicMemory.Minimum = availSize/2;
                        virtualMachine.DynamicMemory.Maximum = availSize;
                    }
                }
            }

            // HDD size
            if (cntx.Quotas.ContainsKey(Quotas.PROXMOX_HDD))
            {
                QuotaValueInfo hddQuota = cntx.Quotas[Quotas.PROXMOX_HDD];
                if (hddQuota.QuotaAllocatedValue == -1)
                {
                    // unlimited HDD
                    txtHdd.Text = "";
                }
                else
                {
                    int availSize = hddQuota.QuotaAllocatedValue - hddQuota.QuotaUsedValue;
                    txtHdd.Text = availSize < 0 ? "" : availSize.ToString();
                }
            }

            // snapshots number
            if (cntx.Quotas.ContainsKey(Quotas.PROXMOX_SNAPSHOTS_NUMBER))
            {
                int snapsNumber = cntx.Quotas[Quotas.PROXMOX_SNAPSHOTS_NUMBER].QuotaAllocatedValue;
                txtSnapshots.Text = (snapsNumber != -1) ? snapsNumber.ToString() : "";
                txtSnapshots.Enabled = (snapsNumber != 0);
            }

            // toggle controls
            BindCheckboxOption(chkDvdInstalled, Quotas.PROXMOX_DVD_ENABLED);
            chkBootFromCd.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.PROXMOX_BOOT_CD_ALLOWED);

            BindCheckboxOption(chkStartShutdown, Quotas.PROXMOX_START_SHUTDOWN_ALLOWED);
            BindCheckboxOption(chkPauseResume, Quotas.PROXMOX_PAUSE_RESUME_ALLOWED);
            BindCheckboxOption(chkReset, Quotas.PROXMOX_RESET_ALOWED);
            BindCheckboxOption(chkReboot, Quotas.PROXMOX_REBOOT_ALLOWED);
            BindCheckboxOption(chkReinstall, Quotas.PROXMOX_REINSTALL_ALLOWED);

            // the settings user controls
            this.BindSettingsControls(virtualMachine);
        }

        private void BindCheckboxOption(CheckBox chk, string quotaName)
        {
            chk.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, quotaName);
            chk.Checked = chk.Enabled;
        }

        private void ToggleControls()
        {
            // send letter
            txtSummaryEmail.Enabled = chkSendSummary.Checked;
            SummaryEmailValidator.Enabled = chkSendSummary.Checked;

            // external network
            bool emptyIps = listExternalAddresses.Items.Count == 0;
            EmptyExternalAddressesMessage.Visible = emptyIps;
            tableExternalNetwork.Visible = !emptyIps && (Convert.ToInt32(listVlanLists.SelectedValue) >= 0);
            //tableExternalNetwork.Visible = chkExternalNetworkEnabled.Checked && !emptyIps;
            //chkExternalNetworkEnabled.Enabled = !emptyIps;
            //chkExternalNetworkEnabled.Checked = chkExternalNetworkEnabled.Checked && !emptyIps;
            ExternalAddressesNumberRow.Visible = radioExternalRandom.Checked;
            ExternalAddressesListRow.Visible = radioExternalSelected.Checked;

            // private network
            tablePrivateNetwork.Visible = chkPrivateNetworkEnabled.Checked && (ViewState["DHCP"] == null);
            PrivateAddressesNumberRow.Visible = radioPrivateRandom.Checked;
            PrivateAddressesListRow.Visible = radioPrivateSelected.Checked;
        }

        private void BindExternalIps()
        {
            //if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED))
            //{

            // bind list
            PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);

            listExternalAddresses.Items.Clear();
            foreach (PackageIPAddress ip in ips)
            {
                if ((listVlanLists.SelectedValue == "-1") || ip.VLAN.ToString() == listVlanLists.SelectedValue)
                {
                    string txt = ip.ExternalIP;
                    if (!String.IsNullOrEmpty(ip.DefaultGateway))
                        txt += "/" + ip.DefaultGateway + " [VLAN " + ip.VLAN + "]";
                    listExternalAddresses.Items.Add(new ListItem(txt, ip.PackageAddressID.ToString()));
                }
            }

            // toggle controls
            int maxAddresses = listExternalAddresses.Items.Count;
            litMaxExternalAddresses.Text = String.Format(GetLocalizedString("litMaxExternalAddresses.Text"), maxAddresses);
            if (maxAddresses > 0)
                txtExternalAddressesNumber.Text = "1";
            //}
        }

        private void BindSummary()
        {
            var resultVm = new VirtualMachine();

            // the user controls
            this.SaveSettingsControls(ref resultVm);
            this.BindSettingsControls(resultVm);
            
            // general
            litHostname.Text =  PortalAntiXSS.Encode(String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim()));
            litOperatingSystem.Text = listOperatingSystems.SelectedItem.Text;

            litSummaryEmail.Text = PortalAntiXSS.Encode(txtSummaryEmail.Text.Trim());
            SummSummaryEmailRow.Visible = chkSendSummary.Checked;

            // config
            litCpu.Text = PortalAntiXSS.Encode(ddlCpu.SelectedValue);
            litRam.Text = PortalAntiXSS.Encode(txtRam.Text.Trim());
            litHdd.Text = PortalAntiXSS.Encode(txtHdd.Text.Trim());
            litSnapshots.Text = PortalAntiXSS.Encode(txtSnapshots.Text.Trim());
            optionDvdInstalled.Value = chkDvdInstalled.Checked;
            optionBootFromCd.Value = chkBootFromCd.Checked;
            optionNumLock.Value = chkNumLock.Checked;
            optionStartShutdown.Value = chkStartShutdown.Checked;
            optionPauseResume.Value = chkPauseResume.Checked;
            optionReboot.Value = chkReboot.Checked;
            optionReset.Value = chkReset.Checked;
            optionReinstall.Value = chkReinstall.Checked;

            // external network
            optionExternalNetwork.Value = (Convert.ToInt32(listVlanLists.SelectedValue) >= 0);
            SummExternalAddressesNumberRow.Visible = radioExternalRandom.Checked && (Convert.ToInt32(listVlanLists.SelectedValue) >= 0);
            litExternalAddressesNumber.Text = PortalAntiXSS.Encode(txtExternalAddressesNumber.Text.Trim());
            SummExternalAddressesListRow.Visible = radioExternalSelected.Checked && (Convert.ToInt32(listVlanLists.SelectedValue) >= 0);

            // external network
            //optionExternalNetwork.Value = chkExternalNetworkEnabled.Checked;
            //SummExternalAddressesNumberRow.Visible = radioExternalRandom.Checked && chkExternalNetworkEnabled.Checked;
            //litExternalAddressesNumber.Text = PortalAntiXSS.Encode(txtExternalAddressesNumber.Text.Trim());
            //SummExternalAddressesListRow.Visible = radioExternalSelected.Checked && chkExternalNetworkEnabled.Checked;

            List<string> ipAddresses = new List<string>();
            foreach (ListItem li in listExternalAddresses.Items)
                if (li.Selected)
                    ipAddresses.Add(li.Text);
            litExternalAddresses.Text = PortalAntiXSS.Encode(String.Join(", ", ipAddresses.ToArray()));

            // private network
            optionPrivateNetwork.Value = chkPrivateNetworkEnabled.Checked;
            SummPrivateAddressesNumberRow.Visible = radioPrivateRandom.Checked && chkPrivateNetworkEnabled.Checked && (ViewState["DHCP"] == null);
            litPrivateAddressesNumber.Text = PortalAntiXSS.Encode(txtPrivateAddressesNumber.Text.Trim());
            SummPrivateAddressesListRow.Visible = radioPrivateSelected.Checked && chkPrivateNetworkEnabled.Checked && (ViewState["DHCP"] == null);

            string[] privIps = Utils.ParseDelimitedString(txtPrivateAddressesList.Text, '\n', '\r', ' ', '\t');
            litPrivateAddressesList.Text = PortalAntiXSS.Encode(String.Join(", ", privIps));

        }

        protected void wizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                VirtualMachine virtualMachine = new VirtualMachine();

                // the user controls
                this.SaveSettingsControls(ref virtualMachine);

                // collect and prepare data
                string hostname = String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim());

                string adminPassword = (string)ViewState["Password"];

                // external IPs
                List<int> extIps = new List<int>();
                foreach (ListItem li in listExternalAddresses.Items)
                    if (li.Selected) extIps.Add(Utils.ParseInt(li.Value));

                // private IPs
                string[] privIps = Utils.ParseDelimitedString(txtPrivateAddressesList.Text, '\n', '\r', ' ', '\t');

                string summaryEmail = chkSendSummary.Checked ? txtSummaryEmail.Text.Trim() : null;

                bool externalenabled = false;
                if (Convert.ToInt32(listVlanLists.SelectedValue) >= 0)
                    externalenabled = true;

                // set default selected vlan
                virtualMachine.DefaultAccessVlan = Convert.ToInt32(listVlanLists.SelectedValue);

                // create virtual machine
                IntResult res = ES.Services.Proxmox.CreateVirtualMachine(PanelSecurity.PackageId,
                    hostname, listOperatingSystems.SelectedValue, adminPassword, summaryEmail,
                    Utils.ParseInt(ddlCpu.SelectedValue), Utils.ParseInt(txtRam.Text.Trim()),
                    Utils.ParseInt(txtHdd.Text.Trim()), Utils.ParseInt(txtSnapshots.Text.Trim()),
                    chkDvdInstalled.Checked, chkBootFromCd.Checked, chkNumLock.Checked,
                    chkStartShutdown.Checked, chkPauseResume.Checked, chkReboot.Checked, chkReset.Checked, chkReinstall.Checked,
                    externalenabled, Utils.ParseInt(txtExternalAddressesNumber.Text.Trim()), radioExternalRandom.Checked, extIps.ToArray(),
                    chkPrivateNetworkEnabled.Checked, Utils.ParseInt(txtPrivateAddressesNumber.Text.Trim()), radioPrivateRandom.Checked, privIps,
                    virtualMachine);

                if (res.IsSuccess)
                {
                    Response.Redirect(EditUrl("ItemID", res.Value.ToString(), "vps_general",
                        "SpaceID=" + PanelSecurity.PackageId.ToString()));
                }
                else
                {
                    messageBox.ShowMessage(res, "VPS_ERROR_CREATE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_CREATE", ex);
            }
        }

        protected void wizard_SideBarButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex < e.CurrentStepIndex)
                return;

            // save password
            if (wizard.ActiveStepIndex == 0)
                ViewState["Password"] = password.Password;

            Page.Validate("Vps");

            if (!Page.IsValid)
                e.Cancel = true;
        }

        protected void wizard_ActiveStepChanged(object sender, EventArgs e)
        {
            BindSummary();
        }

        protected void VlanLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindExternalIps();
        }


        protected void wizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            // save password
            if (wizard.ActiveStepIndex == 0)
                ViewState["Password"] = password.Password;
        }
    }
}
