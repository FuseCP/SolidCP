// Copyright (c) 2018, SolidCP
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
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Virtualization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolidCP.Portal.VPS2012
{
    public partial class VpsToolsReinstallServer : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
            bool reinstallAlloew = VirtualMachines2012Helper.IsReinstallAllowed(PanelSecurity.PackageId);

            if (!manageAllowed && !reinstallAlloew) //block access for user if they don't have permission.
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));

            if (!IsPostBack)
            {
                BindConfiguration();
            }
        }

        //private readonly string sessionVMsettings = "VirtualMachine" + PanelRequest.ItemID;
        private readonly string sessionIpAddresses = "IpAddresses" + PanelRequest.ItemID;   //TODO: Add the ability to change IP?
        private void BindConfiguration()
        {
            Session.Timeout = 10;
            VirtualMachine vm = null;
            // load machine
            //vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);
            vm = VirtualMachines2012Helper.GetCachedVirtualMachine(PanelRequest.ItemID);
            if (vm == null)
            {
                messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM");
                reinstallForms.Visible = false;
                return;
            }
            if (!String.IsNullOrEmpty(vm.CurrentTaskId))
            {
                messageBox.ShowWarningMessage("VPS_PROVISIONING_PROCESS");
                reinstallForms.Visible = false;
                btnReinstall.Enabled = false;
                return;
            }

            // check package quotas
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);

            vm.CreationTime = string.IsNullOrEmpty(vm.CreationTime) ? DateTime.Now.ToString() : vm.CreationTime;
            DateTime dateTimePlusHours = DateTime.Parse(vm.CreationTime).AddHours(9);
            bool IsNotReinstallPossible = !(dateTimePlusHours <= DateTime.Now); //TODO: add possible to change that check.
            if (IsNotReinstallPossible && !manageAllowed)
            {
                messageBox.ShowWarningMessage("VPS_REINSTALL_LIMIT", 
                    "You will be able to reinstall the server after - " + dateTimePlusHours.ToUniversalTime().ToString() + " UTC");
                reinstallForms.Visible = false;
                btnReinstall.Enabled = false;
                return;
            }

            //Session[sessionVMsettings] = vm;          

            //bind hostname
            hostnameSetting.Visible = manageAllowed;
            int dotIdx = vm.Name.IndexOf(".");
            if (dotIdx > -1)
            {
                txtHostname.Text = vm.Name.Substring(0, dotIdx);
                txtDomain.Text = vm.Name.Substring(dotIdx + 1);
            }
            else if (!manageAllowed)
            {
                btnReinstall.Enabled = false;
                messageBox.ShowWarningMessage("VPS_REINSTALL_NO_HOSTNAME");
            }


            //bind password policy
            password.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.VPS_POLICY, "AdministratorPasswordPolicy");

            // OS templates
            //bool isNotUser = ((PanelSecurity.LoggedUser.Role != UserRole.User));
            LibraryItem[] libraryItems = ES.Services.VPS2012.GetOperatingSystemTemplates(PanelSecurity.PackageId);
            listOperatingSystems.Visible = manageAllowed;
            listOperatingSystems.DataSource = libraryItems;
            listOperatingSystems.DataBind();
            string operatingSystemTemplatePath = Path.GetFileName(vm.OperatingSystemTemplatePath);
            if (Array.Exists(libraryItems, item => item.Path == operatingSystemTemplatePath))
            {
                listOperatingSystems.SelectedValue = operatingSystemTemplatePath;
            }
            else if (manageAllowed)
            {
                listOperatingSystems.Items.Insert(0, new ListItem(GetLocalizedString("SelectOsTemplate.Text"), ""));
            }
            else
            {
                btnReinstall.Enabled = false;
                messageBox.ShowWarningMessage("VPS_REINSTALL_NO_TEMPLATE");
            }

            //show summary
            litCpu.Text = vm.CpuCores.ToString();
            litRam.Text = vm.RamSize.ToString();
            litHdd.Text = vm.HddSize.ToString();

            litHddIOPSmin.Visible = litHddIOPSmax.Visible = manageAllowed; //Technical information, it makes no sense to show the user if it can not create the server
            litHddIOPSmin.Text = vm.HddMinimumIOPS.ToString();
            litHddIOPSmax.Text = vm.HddMaximumIOPS.ToString();

            ExternalAddressesRow.Visible = PrivateAddressesRow.Visible = false;
            hiddenTxtExternalAddressesNumber.Value = hiddenTxtPrivateAddressesNumber.Value = "";
            if (vm.ExternalNetworkEnabled)
                BindExternalAddresses();
            if (vm.PrivateNetworkEnabled)
                BindPrivateAddresses();
        }


        private void BindExternalAddresses()
        {
            //PackageIPAddress[] ips = ES.Services.Servers.GetPackageIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork, "","","",0,255,true).Items;
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetExternalNetworkAdapterDetails(PanelRequest.ItemID);
            hiddenTxtExternalAddressesNumber.Value = "1";
            if (nic.IPAddresses != null && nic.IPAddresses.GetLength(0) > 0)
            {
                List<string> ipAddresses = new List<string>();
                foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                    ipAddresses.Add(ip.IPAddress);
                litExternalAddresses.Text = PortalAntiXSS.Encode(String.Join(", ", ipAddresses.ToArray()));
                ExternalAddressesRow.Visible = true;
                Session[sessionIpAddresses] = ipAddresses;
            }
        }

        private List<int> GetExternalAddressesID() // call only after deleting VM
        {
            List<int> extIps = new List<int>();
            PackageIPAddress[] uips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);
            List<string> ipAddresses = (List<string>)Session[sessionIpAddresses];
            foreach (PackageIPAddress uip in uips)
            {
                foreach (string ip in ipAddresses)
                    if (ip.Equals(uip.ExternalIP))
                    {
                        extIps.Add(uip.PackageAddressID);
                        break;
                    }
            }
            return extIps;
        }

        private void BindPrivateAddresses() //possible doesn't work
        {
            PrivateAddressesRow.Visible = true;
            hiddenTxtPrivateAddressesNumber.Value = "1";
            NetworkAdapterDetails nic = ES.Services.VPS2012.GetPrivateNetworkAdapterDetails(PanelRequest.ItemID);
            if (nic.IsDHCP)
            {
                litPrivateAddresses.Text = GetLocalizedString("Automatic.Text");
            }
            else if (nic.IPAddresses != null && nic.IPAddresses.GetLength(0) > 0)
            {
                List<string> ipPrivateAddresses = new List<string>();
                foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                    ipPrivateAddresses.Add(ip.IPAddress);
                litPrivateAddresses.Text = PortalAntiXSS.Encode(String.Join(", ", ipPrivateAddresses.ToArray()));
            }
        }

        private void TryToReinstall()
        {
            try
            {
                VirtualMachine vm = VirtualMachines2012Helper.GetCachedVirtualMachine(PanelRequest.ItemID);

                // create virtual machine
                vm.OperatingSystemTemplate = listOperatingSystems.SelectedValue;
                vm.Name = String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim());
                //vm.PackageId = PanelSecurity.PackageId; //TODO: An idea to change HyperV logic of showing VMs (maybe in 2019?).
                string adminPassword = password.Password;
                string[] privIps = Utils.ParseDelimitedString(litPrivateAddresses.Text, '\n', '\r', ' ', '\t'); //possible doesn't work :)
                IntResult reinstallResult = ES.Services.VPS2012.ReinstallVirtualMachine(PanelRequest.ItemID, vm, adminPassword, privIps, false, false, "");
                
                if (reinstallResult.IsSuccess)
                {
                    //Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
                    Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_general",
                        "SpaceID=" + PanelSecurity.PackageId.ToString()));
                    return;
                }
                else
                {
                    messageBox.ShowMessage(reinstallResult, "VPS_ERROR_CREATE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_CREATE", ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_tools",
            //    "SpaceID=" + PanelSecurity.PackageId.ToString()));
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!chkConfirmReinstall.Checked)
            {
                messageBox.ShowWarningMessage("VPS_REINSTALL_CONFIRM");
            }
            else
            {
                Page.Validate("Vps");
                if (Page.IsValid)
                {
                    btnReinstall.Enabled = false;
                    TryToReinstall();
                    //if (TryToDeleteServerIsSuccess())
                    //{
                    //    TryToCreateServer();
                    //}

                }
            }
        }
    }
}
