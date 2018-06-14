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
            if (!manageAllowed) //block access for user if they don't have permission.
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), ""));

            if (!IsPostBack)
            {
                BindConfiguration();
            }
        }

        private void BindConfiguration()
        {
            VirtualMachine vm = null;
            // load machine
            //vm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);
            vm = VirtualMachines2012Helper.GetCachedVirtualMachine(PanelRequest.ItemID);
            if (vm == null)
            {
                messageBox.ShowErrorMessage("VPS_LOAD_VM_META_ITEM");
                return;
            }
            if (!String.IsNullOrEmpty(vm.CurrentTaskId))
            {
                messageBox.ShowWarningMessage("VPS_PROVISIONING_PROCESS");
                btnReinstall.Enabled = false;
                return;
            }

            Session["VirtualMachine"] = vm;
            // check package quotas
            bool manageAllowed = VirtualMachines2012Helper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);

            //bind hostname
            VirtualMachineSettingsPanel.Visible = manageAllowed;
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
            else if(manageAllowed)
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
            if (nic.IPAddresses != null && nic.IPAddresses.GetLength(0)> 0)
            {
                List<string> ipAddresses = new List<string>();
                foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                    ipAddresses.Add(ip.IPAddress);
                litExternalAddresses.Text = PortalAntiXSS.Encode(String.Join(", ", ipAddresses.ToArray()));
                ExternalAddressesRow.Visible = true;
                Session["IpAddresses"] = ipAddresses;
            }            
        }

        private List<int> GetExternalAddressesID() // call only after deleting VM
        {
            List<int> extIps = new List<int>();
            PackageIPAddress[] uips = ES.Services.Servers.GetPackageUnassignedIPAddresses(PanelSecurity.PackageId, 0, IPAddressPool.VpsExternalNetwork);
            List<string> ipAddresses = (List<string>)Session["IpAddresses"];
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
            else if(nic.IPAddresses != null && nic.IPAddresses.GetLength(0) > 0)
            {
                List<string> ipPrivateAddresses = new List<string>();
                foreach (NetworkAdapterIPAddress ip in nic.IPAddresses)
                    ipPrivateAddresses.Add(ip.IPAddress);
                litPrivateAddresses.Text = PortalAntiXSS.Encode(String.Join(", ", ipPrivateAddresses.ToArray()));
            }
        }
        private void TryToCreateServer()
        {
            try
            {
                VirtualMachine vm = (VirtualMachine)Session["VirtualMachine"];
                // create virtual machine
                vm.Name = String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim());
                vm.PackageId = PanelSecurity.PackageId;
                string adminPassword = password.Password;
                string[] privIps = Utils.ParseDelimitedString(litPrivateAddresses.Text, '\n', '\r', ' ', '\t'); //possible doesn't work :)
                IntResult createResult = ES.Services.VPS2012.CreateNewVirtualMachine(vm,
                    listOperatingSystems.SelectedValue, adminPassword, null,
                    Utils.ParseInt(hiddenTxtExternalAddressesNumber.Value.Trim()), false, GetExternalAddressesID().ToArray(),
                    Utils.ParseInt(hiddenTxtPrivateAddressesNumber.Value.Trim()), false, privIps
                    );
                if (createResult.IsSuccess)
                {
                    Session.Abandon();
                    Response.Redirect(EditUrl("ItemID", createResult.Value.ToString(), "vps_general",
                        "SpaceID=" + PanelSecurity.PackageId.ToString()));
                }
                else
                {
                    messageBox.ShowMessage(createResult, "VPS_ERROR_CREATE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_CREATE", ex);
            }
        }

        private bool TryToDeleteServerIsSuccess()
        {
            // delete machine
            bool isOK = false;
            try
            {
                bool saveFiles = false, exportFiles = false; //not today
                ResultObject res = ES.Services.VPS2012.DeleteVirtualMachine(PanelRequest.ItemID,
                    saveFiles, exportFiles, "");

                if (res.IsSuccess)
                {
                    System.Threading.Thread.Sleep(1000); //give a little time to delete, just for sure.
                    // ready for creating machine
                    isOK = true;                    
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETE", ex);
            }
            return isOK;
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
                    if (TryToDeleteServerIsSuccess())
                    {
                        TryToCreateServer();
                    }
                }          
            }
        }
    }
}
