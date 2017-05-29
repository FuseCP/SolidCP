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
using System.Linq;

namespace SolidCP.Portal.ProviderControls
{
    public partial class HyperVForPrivateCloud_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private StringDictionary localsettings;
        private static LibraryItem[] hosts;
        private LibraryItem[] Hosts
        {
            get
            {
                try
                {
                    if (radioServer.SelectedValue.Equals("host"))
                    {
                        hosts = ES.Services.VPSPC.GetHosts(PanelRequest.ServiceId);
                    }
                    else
                    {
                        hosts = ES.Services.VPSPC.GetClusters(PanelRequest.ServiceId);
                    }
                }
                catch
                {
                    hosts = null;
                }

                return hosts;
            }
        }

        void BindHosts()
        {
            string selectedItem = (localsettings != null ? localsettings["ServerName"] : String.Empty);

            if (!String.IsNullOrEmpty(listHosts.SelectedValue))
            {
                selectedItem = listHosts.SelectedItem.Text;
            }

            listHosts.Items.Clear();
            listHosts.DataSource = Hosts;
            listHosts.DataBind();
            listHosts.Items.Insert(0, new ListItem(GetLocalizedString("listHosts.Text"), ""));

            if (!String.IsNullOrEmpty(selectedItem))
            {
                ListItem selItem = listHosts.Items.FindByText(selectedItem);

                if (selItem != null)
                {
                    selItem.Selected = true;
                }
            }
        }

        void IHostingServiceProviderSettings.BindSettings(StringDictionary settings)
        {
            localsettings = settings;

            radioServer.SelectedValue = localsettings["ServerType"];

            if (String.IsNullOrEmpty(radioServer.SelectedValue))
            {
                radioServer.SelectedValue = "cluster";
            }

            BindHosts();
            // bind networks
            BindNetworksList();

            // CPU
            txtCpuLimit.Text = settings["CpuLimit"];
            txtCpuReserve.Text = settings["CpuReserve"];
            txtCpuWeight.Text = settings["CpuWeight"];
            txtLibraryPath.Text = settings["LibraryPath"];
            txtMonitoringServerName.Text = settings["MonitoringServerName"];

            chkUseSPNSCVMM.Checked = (String.IsNullOrEmpty(settings["UseSPNSCVMM"]) ? false : Convert.ToBoolean(settings["UseSPNSCVMM"]));
            chkUseSPNSCOM.Checked = (String.IsNullOrEmpty(settings["UseSPNSCOM"]) ? false : Convert.ToBoolean(settings["UseSPNSCOM"]));
            //// DVD library
            //txtDvdLibraryPath.Text = settings["DvdLibraryPath"];

            // VHD type
            radioVirtualDiskType.SelectedValue = settings["VirtualDiskType"];

            // External network
            ddlExternalNetworks.SelectedValue = settings["ExternalNetworkName"];

            // Private network
            ddlPrivateNetworks.SelectedValue = settings["PrivateNetworkName"];

            // host name
            txtHostnamePattern.Text = settings["HostnamePattern"];

            // start action
            radioStartAction.SelectedValue = settings["StartAction"];
            txtStartupDelay.Text = settings["StartupDelay"];

            // stop
            radioStopAction.SelectedValue = settings["StopAction"];

            //HyperVCloud
            txtSCVMMServer.Text = settings[VMForPCSettingsName.SCVMMServer.ToString()];
            txtSCVMMPrincipalName.Text = settings[VMForPCSettingsName.SCVMMPrincipalName.ToString()];

            txtSCOMServer.Text = settings[VMForPCSettingsName.SCOMServer.ToString()];
            txtSCOMPrincipalName.Text = settings[VMForPCSettingsName.SCOMPrincipalName.ToString()];

            //Check State 
            CheckServerAndSetState(btnSCVMMServer, VMForPCSettingsName.SCVMMServer, txtSCVMMServer.Text, txtSCVMMPrincipalName.Text);
            CheckServerAndSetState(btnSCOMServer, VMForPCSettingsName.SCOMServer, txtSCOMServer.Text, txtSCOMPrincipalName.Text);
        }

        void IHostingServiceProviderSettings.SaveSettings(StringDictionary settings)
        {

            settings["ServerType"] = radioServer.SelectedValue;
            settings["ServerName"] = listHosts.SelectedItem.Text.Trim();

            // CPU
            settings["CpuLimit"] = txtCpuLimit.Text.Trim();
            settings["CpuReserve"] = txtCpuReserve.Text.Trim();
            settings["CpuWeight"] = txtCpuWeight.Text.Trim();

            settings["MonitoringServerName"] = txtMonitoringServerName.Text.Trim();
            settings["LibraryPath"] = txtLibraryPath.Text.Trim();

            settings["UseSPNSCVMM"] = chkUseSPNSCVMM.Checked.ToString();
            settings["UseSPNSCOM"] = chkUseSPNSCOM.Checked.ToString();

            // VHD type
            settings["VirtualDiskType"] = radioVirtualDiskType.SelectedValue;

            // External network
            settings["ExternalNetworkName"] = ddlExternalNetworks.SelectedValue;

            // Private network
            settings["PrivateNetworkName"] = ddlPrivateNetworks.SelectedValue;

            // host name
            settings["HostnamePattern"] = txtHostnamePattern.Text.Trim();

            // start action
            settings["StartAction"] = radioStartAction.SelectedValue;
            settings["StartupDelay"] = Utils.ParseInt(txtStartupDelay.Text.Trim(), 0).ToString();

            // stop
            settings["StopAction"] = radioStopAction.SelectedValue;

            //HyperVCloud
            settings[VMForPCSettingsName.SCVMMServer.ToString()] = txtSCVMMServer.Text.Trim();
            settings[VMForPCSettingsName.SCVMMPrincipalName.ToString()] = txtSCVMMPrincipalName.Text.Trim();

            settings[VMForPCSettingsName.SCOMServer.ToString()] = txtSCOMServer.Text.Trim();
            settings[VMForPCSettingsName.SCOMPrincipalName.ToString()] = txtSCOMPrincipalName.Text.Trim();
        }

        protected void radioServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindHosts();
        }

        protected void listHosts_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        private void BindNetworksList()
        {
            try
            {
                if (ddlExternalNetworks.Items != null)
                {
                    ddlExternalNetworks.Items.Clear();
                }

                if (ddlPrivateNetworks.Items != null)
                {
                    ddlPrivateNetworks.Items.Clear();
                }

                LibraryItem[] localHosts = hosts ?? Hosts;

                VirtualNetworkInfo[] networks = localHosts.Where(item => item.Path == listHosts.SelectedValue).Select(item => item.Networks).FirstOrDefault();

                ddlExternalNetworks.DataSource = networks ?? new VirtualNetworkInfo[] {};
                ddlExternalNetworks.DataBind();

                ddlPrivateNetworks.DataSource = networks ?? new VirtualNetworkInfo[] { };
                ddlPrivateNetworks.DataBind();
            }
            catch
            {
                ddlExternalNetworks.Items.Add(new ListItem(GetLocalizedString("ErrorReadingNetworksList.Text"), ""));
                ddlPrivateNetworks.Items.Add(new ListItem(GetLocalizedString("ErrorReadingNetworksList.Text"), ""));
            }
        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        protected void btnSCVMMServer_Click(object sender, EventArgs e)
        {
            if (CheckServerAndSetState(sender, VMForPCSettingsName.SCVMMServer, txtSCVMMServer.Text, txtSCVMMPrincipalName.Text.Trim()))
            {
                BindHosts();
            }
        }

        protected void btnSCOMServer_Click(object sender, EventArgs e)
        {
            CheckServerAndSetState(sender, VMForPCSettingsName.SCOMServer, txtSCOMServer.Text, txtSCOMPrincipalName.Text.Trim());
        }

        private bool CheckServerAndSetState(object obj, VMForPCSettingsName control, string conn, string name)
        {
            bool temp = false;
            try
            {
                temp = ES.Services.VPSPC.CheckServerState(control, conn, name, PanelRequest.ServiceId);
            }
            catch (Exception ex)
            {
                messageBoxError.ShowErrorMessage("Server Error", ex);
            }
            finally
            {
                ((WebControl)obj).CssClass = temp ? "enabled" : "disabled";
            }
            return temp;
        }
    }
}
