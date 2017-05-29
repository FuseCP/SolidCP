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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Specialized;
using SolidCP.Providers.Virtualization;
using SolidCP.EnterpriseServer;
using System.Web.UI.MobileControls;
using System.Collections.Generic;

namespace SolidCP.Portal.ProviderControls
{
    public partial class HyperV_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void IHostingServiceProviderSettings.BindSettings(StringDictionary settings)
        {
            txtServerName.Text = settings["ServerName"];
            radioServer.SelectedIndex = (txtServerName.Text == "") ? 0 : 1;

            // bind networks
            BindNetworksList();

            // general settings
            txtVpsRootFolder.Text = settings["RootFolder"];
            txtOSTemplatesPath.Text = settings["OsTemplatesPath"];
            txtExportedVpsPath.Text = settings["ExportedVpsPath"];

            // CPU
            txtCpuLimit.Text = settings["CpuLimit"];
            txtCpuReserve.Text = settings["CpuReserve"];
            txtCpuWeight.Text = settings["CpuWeight"];

            // DVD library
            txtDvdLibraryPath.Text = settings["DvdLibraryPath"];

            // VHD type
            radioVirtualDiskType.SelectedValue = settings["VirtualDiskType"];

            // External network
            ddlExternalNetworks.SelectedValue = settings["ExternalNetworkId"];
            externalPreferredNameServer.Text = settings["ExternalPreferredNameServer"];
            externalAlternateNameServer.Text = settings["ExternalAlternateNameServer"];
            chkAssignIPAutomatically.Checked = Utils.ParseBool(settings["AutoAssignExternalIP"], true);

            // Private network
            ddlPrivateNetworkFormat.SelectedValue = settings["PrivateNetworkFormat"];
            privateIPAddress.Text = settings["PrivateIPAddress"];
            privateSubnetMask.Text = settings["PrivateSubnetMask"];
            privateDefaultGateway.Text = settings["PrivateDefaultGateway"];
            privatePreferredNameServer.Text = settings["PrivatePreferredNameServer"];
            privateAlternateNameServer.Text = settings["PrivateAlternateNameServer"];

            // Management network
            ddlManagementNetworks.SelectedValue = settings["ManagementNetworkId"];
            ddlManageNicConfig.SelectedValue = settings["ManagementNicConfig"];
            managePreferredNameServer.Text = settings["ManagementPreferredNameServer"];
            manageAlternateNameServer.Text = settings["ManagementAlternateNameServer"];

            // host name
            txtHostnamePattern.Text = settings["HostnamePattern"];

            // start action
            radioStartAction.SelectedValue = settings["StartAction"];
            txtStartupDelay.Text = settings["StartupDelay"];

            // stop
            radioStopAction.SelectedValue = settings["StopAction"];

            ToggleControls();
        }

        void IHostingServiceProviderSettings.SaveSettings(StringDictionary settings)
        {
            settings["ServerName"] = txtServerName.Text.Trim();

            // general settings
            settings["RootFolder"] = txtVpsRootFolder.Text.Trim();
            settings["OsTemplatesPath"] = txtOSTemplatesPath.Text.Trim();
            settings["ExportedVpsPath"] = txtExportedVpsPath.Text.Trim();

            // CPU
            settings["CpuLimit"] = txtCpuLimit.Text.Trim();
            settings["CpuReserve"] = txtCpuReserve.Text.Trim();
            settings["CpuWeight"] = txtCpuWeight.Text.Trim();

            // DVD library
            settings["DvdLibraryPath"] = txtDvdLibraryPath.Text.Trim();

            // VHD type
            settings["VirtualDiskType"] = radioVirtualDiskType.SelectedValue;

            // External network
            settings["ExternalNetworkId"] = ddlExternalNetworks.SelectedValue;
            settings["ExternalPreferredNameServer"] = externalPreferredNameServer.Text;
            settings["ExternalAlternateNameServer"] = externalAlternateNameServer.Text;
            settings["AutoAssignExternalIP"] = chkAssignIPAutomatically.Checked.ToString();

            // Private network
            settings["PrivateNetworkFormat"] = ddlPrivateNetworkFormat.SelectedValue;
            settings["PrivateIPAddress"] = ddlPrivateNetworkFormat.SelectedIndex == 0 ? privateIPAddress.Text : "";
            settings["PrivateSubnetMask"] = ddlPrivateNetworkFormat.SelectedIndex == 0 ? privateSubnetMask.Text : "";
            settings["PrivateDefaultGateway"] = privateDefaultGateway.Text;
            settings["PrivatePreferredNameServer"] = privatePreferredNameServer.Text;
            settings["PrivateAlternateNameServer"] = privateAlternateNameServer.Text;

            // Management network
            settings["ManagementNetworkId"] = ddlManagementNetworks.SelectedValue;
            settings["ManagementNicConfig"] = ddlManageNicConfig.SelectedValue;
            settings["ManagementPreferredNameServer"] = ddlManageNicConfig.SelectedIndex == 0 ? managePreferredNameServer.Text : "";
            settings["ManagementAlternateNameServer"] = ddlManageNicConfig.SelectedIndex == 0 ? manageAlternateNameServer.Text : "";

            // host name
            settings["HostnamePattern"] = txtHostnamePattern.Text.Trim();

            // start action
            settings["StartAction"] = radioStartAction.SelectedValue;
            settings["StartupDelay"] = Utils.ParseInt(txtStartupDelay.Text.Trim(), 0).ToString();

            // stop
            settings["StopAction"] = radioStopAction.SelectedValue;
        }

        private void BindNetworksList()
        {
            try
            {
                VirtualSwitch[] switches = ES.Services.VPS.GetExternalSwitches(PanelRequest.ServiceId, txtServerName.Text.Trim());

                ddlExternalNetworks.DataSource = switches;
                ddlExternalNetworks.DataBind();

                ddlManagementNetworks.DataSource = switches;
                ddlManagementNetworks.DataBind();
                ddlManagementNetworks.Items.Insert(0, new ListItem(GetLocalizedString("ddlManagementNetworks.Text"), ""));

                locErrorReadingNetworksList.Visible = false;
            }
            catch
            {
                ddlExternalNetworks.Items.Add(new ListItem(GetLocalizedString("ErrorReadingNetworksList.Text"), ""));
                ddlManagementNetworks.Items.Add(new ListItem(GetLocalizedString("ErrorReadingNetworksList.Text"), ""));
                locErrorReadingNetworksList.Visible = true;
            }
        }

        private void ToggleControls()
        {
            ServerNameRow.Visible = (radioServer.SelectedIndex == 1);

            if (radioServer.SelectedIndex == 0)
            {
                txtServerName.Text = "";
            }

            // private network
            PrivCustomFormatRow.Visible = (ddlPrivateNetworkFormat.SelectedIndex == 0);

            // management network
            ManageNicConfigRow.Visible = (ddlManagementNetworks.SelectedIndex > 0);
            ManageAlternateNameServerRow.Visible = ManageNicConfigRow.Visible && (ddlManageNicConfig.SelectedIndex == 0);
            ManagePreferredNameServerRow.Visible = ManageNicConfigRow.Visible && (ddlManageNicConfig.SelectedIndex == 0);
        }

        protected void radioServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        protected void ddlPrivateNetworkFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void ddlManageNicConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void ddlManagementNetworks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }
    }
}
