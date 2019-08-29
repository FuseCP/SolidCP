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

namespace SolidCP.Portal.VPS2012
{
    public partial class VdcCreate : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
            if (!manageAllowed) //block access for user if they don't have permission.
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));

            secHddQOS.Visible = QOSManag.Visible = PanelSecurity.EffectiveUser.Role != UserRole.User;

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
            if (!PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED))
            {
                wizard.WizardSteps.Remove(stepExternalNetwork);
                chkExternalNetworkEnabled.Checked = false;
            }

            //KD FSJ
            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            QuotaValueInfo cpuQuota2 = cntx.Quotas[Quotas.VPS2012_CPU_NUMBER];
            if (cpuQuota2.QuotaAllocatedValue > cpuQuota2.QuotaUsedValue | cpuQuota2.QuotaAllocatedValue == -1)
            {
                wizard.Visible = true;

            }
            else
            {
                wizard.Visible = false;
                messageBox.ShowErrorMessage("NO_CPU_CORES");
            }

            // private network
            if (!PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_PRIVATE_NETWORK_ENABLED))
            {
                wizard.WizardSteps.Remove(stepPrivateNetwork);
                chkPrivateNetworkEnabled.Checked = false;
            }
        }

        private bool IsMailConfigured(UserInfo user)
        {
            UserSettings userSettings = ES.Services.Users.GetUserSettings(user.UserId, UserSettings.VPS_SUMMARY_LETTER);
            bool isPossibleSendMail = 
                (
                    !string.IsNullOrEmpty(userSettings["From"]) &&
                    !string.IsNullOrEmpty(userSettings["Subject"]) &&
                        (
                            !string.IsNullOrEmpty(userSettings["HtmlBody"]) ||
                            !string.IsNullOrEmpty(userSettings["TextBody"])
                        )
                );
            return isPossibleSendMail;
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
            listOperatingSystems.DataSource = ES.Services.VPS2012.GetOperatingSystemTemplates(PanelSecurity.PackageId);
            listOperatingSystems.DataBind();
            listOperatingSystems.Items.Insert(0, new ListItem(GetLocalizedString("SelectOsTemplate.Text"), ""));

            // summary letter e-mail
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package != null)
            {
                UserInfo user = ES.Services.Users.GetUserById(package.UserId);                
                if (user != null)
                {                    
                    chkSendSummary.Checked = IsMailConfigured(user);
                    txtSummaryEmail.Text = user.Email;
                }
            }

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            // bind CPU cores
            int maxCores = ES.Services.VPS2012.GetMaximumCpuCoresNumber(PanelSecurity.PackageId);

            QuotaValueInfo cpuQuota2 = cntx.Quotas[Quotas.VPS2012_CPU_NUMBER];

            if (cpuQuota2.QuotaAllocatedValue == -1)
            {
                for (int i = 1; i < maxCores + 1; i++)
                    ddlCpu.Items.Add(i.ToString());

                ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item
            }
            else if (cpuQuota2.QuotaAllocatedValue >= cpuQuota2.QuotaUsedValue)
            {
                if ((cpuQuota2.QuotaAllocatedValue + 1 - cpuQuota2.QuotaUsedValue) > maxCores)
                {
                    for (int i = 1; i < maxCores + 1; i++)
                        ddlCpu.Items.Add(i.ToString());

                    ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item
                }
                else
                {
                    for (int i = 1; i < (cpuQuota2.QuotaAllocatedValue - cpuQuota2.QuotaUsedValue) + 1; i++)
                        ddlCpu.Items.Add(i.ToString());

                    ddlCpu.SelectedIndex = ddlCpu.Items.Count - 1; // select last (maximum) item
                }
            }
            else
            {
                ddlCpu.Items.Add("0");

            }

            // external network details
            if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_EXTERNAL_NETWORK_ENABLED))
            {
                List<int> dupevlans = new List<int>();
                List<int> vlans = new List<int>();

                bool isUnassignedPackageIPs = false;
                // bind vlan list
                PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);
                if (ips.Length > 0)
                {
                    foreach (PackageIPAddress ip in ips)
                    {
                        dupevlans.Add(ip.VLAN);
                    }
                }
                else
                {
                    if (PanelSecurity.EffectiveUser.Role != UserRole.User)
                    {
                        IPAddressInfo[] uips = ES.Services.Servers.GetUnallottedIPAddresses(PanelSecurity.PackageId, ResourceGroups.VPS2012, IPAddressPool.VpsExternalNetwork);
                        isUnassignedPackageIPs = true;
                        foreach (IPAddressInfo ip in uips)
                        {
                            dupevlans.Add(ip.VLAN);
                        }
                    }
                }

                // return vlan list without dupes
                vlans = dupevlans.Distinct().ToList();

                //List<int> vlans = ES.Services.VPS2012.GetAvailableVLANs(PanelSecurity.PackageId).vlans;
                listVlanLists.Items.Clear();
                //listVlanLists.Items.Add(new ListItem("External network disabled", "-1"));
                listVlanLists.Items.Add(new ListItem("Select a VLAN", "-1"));
                foreach (int vlan in vlans)
                {
                    listVlanLists.Items.Add(new ListItem(String.Format("VLAN {0}", vlan.ToString()), vlan.ToString()));
                }

                // bind external network ips 4 selected vlan
                if (isUnassignedPackageIPs)
                    BindExternalUnallottedIps();
                else
                    BindExternalIps();

                //GenerateMac
                txtExternalMACAddress.Text = ES.Services.VPS2012.GenerateMacAddress();
            }

            // private network
            if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_PRIVATE_NETWORK_ENABLED))
            {
                NetworkAdapterDetails nic = ES.Services.VPS2012.GetPrivateNetworkDetails(PanelSecurity.PackageId);
                litPrivateNetworkFormat.Text = nic.NetworkFormat;
                litPrivateSubnetMask.Text = nic.SubnetMask;

                // set max number
                QuotaValueInfo privQuota = cntx.Quotas[Quotas.VPS2012_PRIVATE_IP_ADDRESSES_NUMBER];
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

                listPrivateNetworkVLAN.Items.Clear();
                PackageVLANsPaged vlans = ES.Services.Servers.GetPackagePrivateNetworkVLANs(PanelSecurity.PackageId, "", 0, Int32.MaxValue);
                if (vlans != null && vlans.Count > 0)
                {
                    foreach (PackageVLAN vlan in vlans.Items)
                    {
                        listPrivateNetworkVLAN.Items.Add(new ListItem(String.Format("VLAN {0}", vlan.Vlan.ToString()), vlan.Vlan.ToString()));
                    }
                }
            }



            // RAM size
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_RAM))
            {
                QuotaValueInfo ramQuota = cntx.Quotas[Quotas.VPS2012_RAM];
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
                        virtualMachine.DynamicMemory.Minimum = availSize / 2;
                        virtualMachine.DynamicMemory.Maximum = availSize;
                    }
                }
            }

            // HDD size
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_HDD))
            {
                QuotaValueInfo hddQuota = cntx.Quotas[Quotas.VPS2012_HDD];
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

            // IOPS number
            // TODO: checke
            txtHddMinIOPS.Text = "0";
            txtHddMaxIOPS.Text = "0";

            // snapshots number
            if (cntx.Quotas.ContainsKey(Quotas.VPS2012_SNAPSHOTS_NUMBER))
            {
                int snapsNumber = cntx.Quotas[Quotas.VPS2012_SNAPSHOTS_NUMBER].QuotaAllocatedValue;
                txtSnapshots.Text = (snapsNumber != -1) ? snapsNumber.ToString() : "";
                txtSnapshots.Enabled = (snapsNumber != 0);
            }

            // toggle controls
            BindCheckboxOption(chkDvdInstalled, Quotas.VPS2012_DVD_ENABLED);
            chkBootFromCd.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_BOOT_CD_ALLOWED);

            BindCheckboxOption(chkStartShutdown, Quotas.VPS2012_START_SHUTDOWN_ALLOWED);
            BindCheckboxOption(chkPauseResume, Quotas.VPS2012_PAUSE_RESUME_ALLOWED);
            BindCheckboxOption(chkReset, Quotas.VPS2012_RESET_ALOWED);
            BindCheckboxOption(chkReboot, Quotas.VPS2012_REBOOT_ALLOWED);
            BindCheckboxOption(chkReinstall, Quotas.VPS2012_REINSTALL_ALLOWED);

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
            txtHostname.Enabled = txtDomain.Enabled = true;
            string defaultHostname = "The-name-will-be";
            if (chkAutoHostName.Checked)
            {
                txtHostname.Text = defaultHostname;
                txtDomain.Text = "generated.automatically";
                txtHostname.Enabled = txtDomain.Enabled = false;
            }else if(string.Equals(txtHostname.Text, defaultHostname))
            {
                txtHostname.Text = "";
                txtDomain.Text = "";
            }

            if (ViewState["Password"] != null)
                password.Password = ViewState["Password"].ToString();

            // send letter
            txtSummaryEmail.Enabled = chkSendSummary.Checked;
            SummaryEmailValidator.Enabled = chkSendSummary.Checked;

            // external network
            bool emptyIps = listExternalAddresses.Items.Count == 0;
            EmptyExternalAddressesMessage.Visible = emptyIps;
            listVlanLists.Visible = !emptyIps && (listVlanLists.Items.Count > 2); //First is fake ("Select Vlan")
            if (!emptyIps && (listVlanLists.Items.Count > 2))
            {
                tableExternalNetwork.Visible = !emptyIps && (Convert.ToInt32(listVlanLists.SelectedValue) >= 0);
                //TODO: set the first possible VLAN?
            }
            else
            {
                tableExternalNetwork.Visible = chkExternalNetworkEnabled.Checked && !emptyIps;
                chkExternalNetworkEnabled.Enabled = !emptyIps;
                chkExternalNetworkEnabled.Checked = chkExternalNetworkEnabled.Checked && !emptyIps;
                listVlanLists.SelectedIndex = listVlanLists.Items.Count - 1;
            }

            ExternalMACAddressRow.Visible = !emptyIps && (PanelSecurity.LoggedUser.Role != UserRole.User);
            ExternalAddressesNumberRow.Visible = radioExternalRandom.Checked;
            ExternalAddressesListRow.Visible = radioExternalSelected.Checked;

            // private network
            tablePrivateNetwork.Visible = chkPrivateNetworkEnabled.Checked && (ViewState["DHCP"] == null);
            PrivateAddressesNumberRow.Visible = radioPrivateRandom.Checked;
            PrivateAddressesListRow.Visible = radioPrivateSelected.Checked;
            listPrivateNetworkVLAN.Visible = listPrivateNetworkVLAN.Items.Count > 1;
        }

        private void BindExternalIps()
        {
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
        }

        private void BindExternalUnallottedIps()
        {
            // bind list
            IPAddressInfo[] ips = ES.Services.Servers.GetUnallottedIPAddresses(PanelSecurity.PackageId, ResourceGroups.VPS2012, IPAddressPool.VpsExternalNetwork);

            listExternalAddresses.Items.Clear();
            foreach (IPAddressInfo ip in ips)
            {
                if ((listVlanLists.SelectedValue == "-1") || ip.VLAN.ToString() == listVlanLists.SelectedValue)
                {
                    string txt = ip.ExternalIP;
                    if (!String.IsNullOrEmpty(ip.DefaultGateway))
                        txt += "/" + ip.DefaultGateway + " [VLAN " + ip.VLAN + "]";
                    listExternalAddresses.Items.Add(new ListItem(txt, ip.AddressId.ToString()));
                }
            }

            // toggle controls
            int maxAddresses = listExternalAddresses.Items.Count;
            litMaxExternalAddresses.Text = String.Format(GetLocalizedString("litMaxExternalAddresses.Text"), maxAddresses);
            if (maxAddresses > 0)
                txtExternalAddressesNumber.Text = "1";
        }


        private void BindSummary()
        {
            var resultVm = new VirtualMachine();

            // the user controls
            this.SaveSettingsControls(ref resultVm);
            this.BindSettingsControls(resultVm);

            // general
            litHostname.Text = PortalAntiXSS.Encode(String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim()));
            litOperatingSystem.Text = listOperatingSystems.SelectedItem.Text;

            litSummaryEmail.Text = PortalAntiXSS.Encode(txtSummaryEmail.Text.Trim());
            SummSummaryEmailRow.Visible = chkSendSummary.Checked;

            // config
            litCpu.Text = PortalAntiXSS.Encode(ddlCpu.SelectedValue);
            litRam.Text = PortalAntiXSS.Encode(txtRam.Text.Trim());
            litHdd.Text = PortalAntiXSS.Encode(txtHdd.Text.Trim());
            litHddIOPSmin.Text = PortalAntiXSS.Encode(txtHddMinIOPS.Text.Trim());
            litHddIOPSmax.Text = PortalAntiXSS.Encode(txtHddMaxIOPS.Text.Trim());
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
            bool isSelectedAndChecked = chkExternalNetworkEnabled.Checked && (Convert.ToInt32(listVlanLists.SelectedValue) >= 0);
            optionExternalNetwork.Value = isSelectedAndChecked;
            SummExternalAddressesNumberRow.Visible = radioExternalRandom.Checked && isSelectedAndChecked;
            litExternalAddressesNumber.Text = PortalAntiXSS.Encode(txtExternalAddressesNumber.Text.Trim());
            SummExternalAddressesListRow.Visible = radioExternalSelected.Checked && isSelectedAndChecked;
            SummExternalAddressMAC.Visible = isSelectedAndChecked && (PanelSecurity.LoggedUser.Role != UserRole.User);
            litSummExternalAddressMAC.Text = PortalAntiXSS.Encode(txtExternalMACAddress.Text.Trim()).Replace(" ", "").Replace(":", "").Replace("-", "");

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
                virtualMachine.Name = chkAutoHostName.Checked ? "" : String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim());
                virtualMachine.PackageId = PanelSecurity.PackageId;
                virtualMachine.CpuCores = Utils.ParseInt(ddlCpu.SelectedValue);
                virtualMachine.RamSize = Utils.ParseInt(txtRam.Text.Trim());
                virtualMachine.HddSize = Utils.ParseInt(txtHdd.Text.Trim());
                virtualMachine.HddMinimumIOPS = Utils.ParseInt(txtHddMinIOPS.Text.Trim());
                virtualMachine.HddMaximumIOPS = Utils.ParseInt(txtHddMaxIOPS.Text.Trim());
                virtualMachine.SnapshotsNumber = Utils.ParseInt(txtSnapshots.Text.Trim());
                virtualMachine.DvdDriveInstalled = chkDvdInstalled.Checked;
                virtualMachine.BootFromCD = chkBootFromCd.Checked;
                virtualMachine.NumLockEnabled = chkNumLock.Checked;
                virtualMachine.StartTurnOffAllowed = chkStartShutdown.Checked;
                virtualMachine.PauseResumeAllowed = chkPauseResume.Checked;
                virtualMachine.RebootAllowed = chkReboot.Checked;
                virtualMachine.ResetAllowed = chkReset.Checked;
                virtualMachine.ReinstallAllowed = chkReinstall.Checked;
                virtualMachine.ExternalNetworkEnabled = false; //setting up after
                virtualMachine.ExternalNicMacAddress = txtExternalMACAddress.Text.Trim().Replace(" ", "").Replace(":", "").Replace("-", "");
                virtualMachine.PrivateNetworkEnabled = chkPrivateNetworkEnabled.Checked;


                string adminPassword = (string)ViewState["Password"];

                // external IPs
                List<int> extIps = new List<int>();
                foreach (ListItem li in listExternalAddresses.Items)
                    if (li.Selected) extIps.Add(Utils.ParseInt(li.Value));

                // private IPs
                string[] privIps = Utils.ParseDelimitedString(txtPrivateAddressesList.Text, '\n', '\r', ' ', '\t');

                string summaryEmail = chkSendSummary.Checked ? txtSummaryEmail.Text.Trim() : null;

                //virtualMachine.ExternalNetworkEnabled = false;
                if ((Convert.ToInt32(listVlanLists.SelectedValue) >= 0) && chkExternalNetworkEnabled.Checked)
                    virtualMachine.ExternalNetworkEnabled = true;


                // set default selected vlan
                virtualMachine.defaultaccessvlan = Convert.ToInt32(listVlanLists.SelectedValue);//external network vlan

                // set private network vlan
                if (listPrivateNetworkVLAN.Items.Count > 0)
                {
                    virtualMachine.PrivateNetworkVlan = Convert.ToInt32(listPrivateNetworkVLAN.SelectedValue);
                }

                // create virtual machine
                IntResult res = ES.Services.VPS2012.CreateNewVirtualMachine(virtualMachine,
                    listOperatingSystems.SelectedValue, adminPassword, summaryEmail,
                    Utils.ParseInt(txtExternalAddressesNumber.Text.Trim()), radioExternalRandom.Checked, extIps.ToArray(),
                    Utils.ParseInt(txtPrivateAddressesNumber.Text.Trim()), radioPrivateRandom.Checked, privIps
                    );
                //IntResult res = ES.Services.VPS2012.CreateVirtualMachine(PanelSecurity.PackageId,
                //    hostname, listOperatingSystems.SelectedValue, adminPassword, summaryEmail,
                //    Utils.ParseInt(ddlCpu.SelectedValue), Utils.ParseInt(txtRam.Text.Trim()),
                //    Utils.ParseInt(txtHdd.Text.Trim()), Utils.ParseInt(txtSnapshots.Text.Trim()),
                //    chkDvdInstalled.Checked, chkBootFromCd.Checked, chkNumLock.Checked,
                //    chkStartShutdown.Checked, chkPauseResume.Checked, chkReboot.Checked, chkReset.Checked, chkReinstall.Checked,
                //    externalenabled, Utils.ParseInt(txtExternalAddressesNumber.Text.Trim()), radioExternalRandom.Checked, extIps.ToArray(),
                //    chkPrivateNetworkEnabled.Checked, Utils.ParseInt(txtPrivateAddressesNumber.Text.Trim()), radioPrivateRandom.Checked, privIps,
                //    virtualMachine);
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
            PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);
            if (ips.Length > 0)
                BindExternalIps();
            else
                if (PanelSecurity.EffectiveUser.Role != UserRole.User) BindExternalUnallottedIps();

        }


        protected void wizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            // save password
            if (wizard.ActiveStepIndex == 0)
                ViewState["Password"] = password.Password;
        }
    }
}
