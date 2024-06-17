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
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.Virtualization;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.ProviderControls
{
    public partial class HyperV2012R2_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public bool IsRemoteServer { get { return radioServer.SelectedIndex > 0; } }
        public string RemoteServerName { get { return IsRemoteServer ? txtServerName.Text.Trim() : ""; } }
        public string CertificateThumbprint { get { return IsRemoteServer ? txtCertThumbnail.Text.Trim() : ddlCertThumbnail.SelectedValue; } }
        public bool IsReplicaServer { get { return ReplicationModeList.SelectedValue == ReplicaMode.IsReplicaServer.ToString(); } }
        public bool EnabledReplica { get { return ReplicationModeList.SelectedValue == ReplicaMode.ReplicationEnabled.ToString(); } }
        public string ReplicaServerId { get; set; }
        private List<SecureBootTemplate> SecureBootTemplates { get; set; }
        private List<VirtualSwitch> ExternalSwitches { get; set; }

        void IHostingServiceProviderSettings.BindSettings(StringDictionary settings)
        {
            txtServerName.Text = settings["ServerName"];
            radioServer.SelectedIndex = (txtServerName.Text == "") ? 0 : 1;
            
            // bind networks
            BindNetworksList();

            // Maintenance Mode
            radioMaintenanceMode.SelectedValue = settings["MaintenanceMode"];
            radioMaintenanceMode.SelectedIndex = string.IsNullOrEmpty(radioMaintenanceMode.SelectedValue) 
                ? 0 : radioMaintenanceMode.SelectedIndex;

            // Guacamole
            txtGuacamoleConnectScript.Text = settings["GuacamoleConnectScript"];
            txtGuacamoleConnectPassword.Text = settings["GuacamoleConnectPassword"];
            txtGuacamoleHyperVDomain.Text = settings["GuacamoleHyperVDomain"];
            txtGuacamoleHyperVIP.Text = settings["GuacamoleHyperVIP"];
            ViewState["PWD"] = settings["GuacamoleHyperVAdministratorPassword"];
            rowPassword.Visible = ((string)ViewState["PWD"]) != "";
            if (!String.IsNullOrEmpty(settings["GuacamoleHyperVUser"])) txtGuacamoleHyperVUser.Text = settings["GuacamoleHyperVUser"];

            // general settings
            txtVpsRootFolder.Text = settings["RootFolder"];
            txtExportedVpsPath.Text = settings["ExportedVpsPath"];

            // CPU
            txtCpuLimit.Text = settings["CpuLimit"];
            txtCpuReserve.Text = settings["CpuReserve"];
            txtCpuWeight.Text = settings["CpuWeight"];

            // RAM
            txtRamReserve.Text = settings["RamReserve"];
            if (string.IsNullOrEmpty(txtRamReserve.Text))
                txtRamReserve.Text = "0"; //unlimited or disabled

            // Default Windows Configure Version
            BindVMConfigVersionList();
            ddlHyperVConfig.SelectedValue = settings["HyperVConfigurationVersion"];

            // OS Templates
            txtOSTemplatesPath.Text = settings["OsTemplatesPath"];
            repOsTemplates.DataSource = new ConfigFile(settings["OsTemplates"]).LibraryItems; //ES.Services.VPS2012.GetOperatingSystemTemplatesByServiceId(PanelRequest.ServiceId).ToList();
            repOsTemplates.DataBind();
            //onupdate select value

            // DVD library
            txtDvdLibraryPath.Text = settings["DvdLibraryPath"];
            repDvdLibrary.DataSource = new ConfigFile(settings["DvdLibrary"]).LibraryItems;
            repDvdLibrary.DataBind();

            // PS Script
            repPsScript.DataSource = new ConfigFile(settings["PsScript"]).LibraryItems;
            repPsScript.DataBind();

            // VHD type
            radioVirtualDiskType.SelectedValue = settings["VirtualDiskType"];

            // Switch Type
            radioSwitchType.SelectedValue = settings["SwitchType"];
            radioSwitchType.SelectedIndex = string.IsNullOrEmpty(radioSwitchType.SelectedValue) 
                ? 0 : radioSwitchType.SelectedIndex;

            chkGetSwitchesByPS.Checked = Utils.ParseBool(settings["UsePowerShellToGetExternalSW"], false);

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
            radioSwitchTypePrivateNetwork.SelectedValue = string.IsNullOrEmpty(settings["PrivateSwitchType"])
                ? "private" : settings["PrivateSwitchType"];
            ddlExternalNetworksPrivate.SelectedValue = settings["PrivateNetworkId"];
            ddlExternalNetworksPrivate.Enabled = "external".Equals(radioSwitchTypePrivateNetwork.SelectedValue);
            chkAssignVLANAutomatically.Checked = Utils.ParseBool(settings["AutoAssignVLAN"], true);
            chkAssignVLANAutomatically.Enabled = ddlExternalNetworksPrivate.Enabled;

            // DMZ network
            ddlDmzNetworkFormat.SelectedValue = settings["DmzNetworkFormat"];
            dmzIPAddress.Text = settings["DmzIPAddress"];
            dmzSubnetMask.Text = settings["DmzSubnetMask"];
            dmzDefaultGateway.Text = settings["DmzDefaultGateway"];
            dmzPreferredNameServer.Text = settings["DmzPreferredNameServer"];
            dmzAlternateNameServer.Text = settings["DmzAlternateNameServer"];
            radioSwitchTypeDmzNetwork.SelectedValue = string.IsNullOrEmpty(settings["DmzSwitchType"])
                ? "private" : settings["DmzSwitchType"];
            ddlExternalNetworksDmz.SelectedValue = settings["DmzNetworkId"];
            ddlExternalNetworksDmz.Enabled = "external".Equals(radioSwitchTypeDmzNetwork.SelectedValue);
            chkDmzAssignVLANAutomatically.Checked = Utils.ParseBool(settings["DmzAutoAssignVLAN"], true);
            chkDmzAssignVLANAutomatically.Enabled = ddlExternalNetworksDmz.Enabled;

            // Management network
            ddlManagementNetworks.SelectedValue = settings["ManagementNetworkId"];
            ddlManageNicConfig.SelectedValue = settings["ManagementNicConfig"];
            managePreferredNameServer.Text = settings["ManagementPreferredNameServer"];
            manageAlternateNameServer.Text = settings["ManagementAlternateNameServer"];

            // host name
            txtHostnamePattern.Text = settings["HostnamePattern"];
            if (string.IsNullOrEmpty(txtHostnamePattern.Text))
                txtHostnamePattern.Text = "[NetBIOSName].domain.local";

            // start action
            radioStartAction.SelectedValue = settings["StartAction"];
            txtStartupDelay.Text = settings["StartupDelay"];

            // stop
            radioStopAction.SelectedValue = settings["StopAction"];

            // replica
            ReplicationModeList.SelectedValue = settings["ReplicaMode"] ?? ReplicaMode.None.ToString();
            txtReplicaPath.Text = settings["ReplicaServerPath"];
            ReplicaServerId = settings["ReplicaServerId"];

            // Failover Cluster
            chkUseFailoverCluster.Checked = Utils.ParseBool(settings["UseFailoverCluster"], false);
            tbClusterName.Text = settings["ClusterName"];
            tbClusterName.Enabled = chkUseFailoverCluster.Checked;
            ClusterNameValidator.Enabled = chkUseFailoverCluster.Checked;

            ToggleControls();

            // replica
            txtCertThumbnail.Text = settings["ReplicaServerThumbprint"];
            ddlCertThumbnail.SelectedValue = settings["ReplicaServerThumbprint"];
            ddlReplicaServer.SelectedValue = settings["ReplicaServerId"];

            if (IsReplicaServer)
            {
                var realReplica = ES.Services.VPS2012.GetReplicaServer(PanelRequest.ServiceId, RemoteServerName);

                if (realReplica == null)
                    ReplicaErrorTr.Visible = true;
            }
        }

        void IHostingServiceProviderSettings.SaveSettings(StringDictionary settings)
        {            
            if (radioServer.SelectedIndex == 0)
                settings["ServerName"] = "";
            else
                settings["ServerName"] = txtServerName.Text.Trim();

            // MaintenanceMode
            settings["MaintenanceMode"] = radioMaintenanceMode.SelectedValue;

            // Guacamole
            settings["GuacamoleConnectScript"] = txtGuacamoleConnectScript.Text.Trim();
            settings["GuacamoleConnectPassword"] = txtGuacamoleConnectPassword.Text.Trim();
            settings["GuacamoleHyperVIP"] = txtGuacamoleHyperVIP.Text.Trim();
            settings["GuacamoleHyperVDomain"] = txtGuacamoleHyperVDomain.Text.Trim();
            settings["GuacamoleHyperVAdministratorPassword"] = (txtGuacamoleHyperVAdministratorPassword.Text.Length > 0) ? txtGuacamoleHyperVAdministratorPassword.Text : (string)ViewState["PWD"];
            settings["GuacamoleHyperVUser"] = txtGuacamoleHyperVUser.Text.Trim();

            // general settings
            settings["RootFolder"] = txtVpsRootFolder.Text.Trim();
            settings["ExportedVpsPath"] = txtExportedVpsPath.Text.Trim();

            // CPU
            settings["CpuLimit"] = txtCpuLimit.Text.Trim();
            settings["CpuReserve"] = txtCpuReserve.Text.Trim();
            settings["CpuWeight"] = txtCpuWeight.Text.Trim();

            // RAM
            if (string.IsNullOrEmpty(settings["ServerName"])) {
                settings["RamReserve"] = Utils.ParseInt(txtRamReserve.Text.Trim(), 0).ToString();
            } else {
                settings["RamReserve"] = "0";
            }
                

            // Default Windows Configure Version
            settings["HyperVConfigurationVersion"] = ddlHyperVConfig.SelectedValue;

            // OS Templates
            settings["OsTemplates"] = GetConfigXml(GetOsTemplates());
            settings["OsTemplatesPath"] = txtOSTemplatesPath.Text.Trim();

            // DVD library
            settings["DvdLibrary"] = GetConfigXml(GetDvds());
            settings["DvdLibraryPath"] = txtDvdLibraryPath.Text.Trim();

            // PS Script
            settings["PsScript"] = GetConfigXml(GetPsScripts());

            // VHD type
            settings["VirtualDiskType"] = radioVirtualDiskType.SelectedValue;

            // Switch Type
            settings["SwitchType"] = radioSwitchType.SelectedValue;
            settings["UsePowerShellToGetExternalSW"] = chkGetSwitchesByPS.Checked.ToString();

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
            settings["PrivateSwitchType"] = radioSwitchTypePrivateNetwork.SelectedValue;
            if ("external".Equals(radioSwitchTypePrivateNetwork.SelectedValue))
            {
                settings["PrivateNetworkId"] = ddlExternalNetworksPrivate.SelectedValue;
                settings["AutoAssignVLAN"] = chkAssignVLANAutomatically.Checked.ToString();
            }
            else
            {
                settings["PrivateNetworkId"] = "";
                settings["AutoAssignVLAN"] = "false";
            }

            // DMZ network
            settings["DmzNetworkFormat"] = ddlDmzNetworkFormat.SelectedValue;
            settings["DmzIPAddress"] = ddlDmzNetworkFormat.SelectedIndex == 0 ? dmzIPAddress.Text : "";
            settings["DmzSubnetMask"] = ddlDmzNetworkFormat.SelectedIndex == 0 ? dmzSubnetMask.Text : "";
            settings["DmzDefaultGateway"] = dmzDefaultGateway.Text;
            settings["DmzPreferredNameServer"] = dmzPreferredNameServer.Text;
            settings["DmzAlternateNameServer"] = dmzAlternateNameServer.Text;
            settings["DmzSwitchType"] = radioSwitchTypeDmzNetwork.SelectedValue;
            if ("external".Equals(radioSwitchTypeDmzNetwork.SelectedValue))
            {
                settings["DmzNetworkId"] = ddlExternalNetworksDmz.SelectedValue;
                settings["DmzAutoAssignVLAN"] = chkDmzAssignVLANAutomatically.Checked.ToString();
            }
            else
            {
                settings["DmzNetworkId"] = "";
                settings["DmzAutoAssignVLAN"] = "false";
            }

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

            // replication
            settings["ReplicaMode"] = ReplicationModeList.SelectedValue;
            settings["ReplicaServerId"] = ddlReplicaServer.SelectedValue;
            settings["ReplicaServerPath"] = txtReplicaPath.Text;
            settings["ReplicaServerThumbprint"] = CertificateThumbprint;

            // Failover Cluster
            settings["UseFailoverCluster"] = chkUseFailoverCluster.Checked.ToString();
            settings["ClusterName"] = tbClusterName.Text;

            SetUnsetReplication();
        }

        private void BindVMConfigVersionList()
        {
            List<VMConfigurationVersion> configurationVersions;
            configurationVersions = new List<VMConfigurationVersion>(ES.Services.VPS2012.GetVMConfigurationVersionSupportedList(PanelRequest.ServiceId));
            configurationVersions.RemoveAll(p => p.Version == "254.0" || p.Version == "255.0"); //Remove Experimental version
            ddlHyperVConfig.DataSource = configurationVersions;
            ddlHyperVConfig.DataBind();
        }

        private void BindNetworksList()
        {
            chkGetSwitchesByPS.Enabled = true;
            try
            {
                List<VirtualSwitch> switches;

                if (radioSwitchType.SelectedValue == "internal")
                {
                    switches = new List<VirtualSwitch>(ES.Services.VPS2012.GetInternalSwitches(PanelRequest.ServiceId, txtServerName.Text.Trim()));
                    chkGetSwitchesByPS.Enabled = false;
                }
                else switches = GetExternalSwitches();

                ddlExternalNetworks.DataSource = switches;
                ddlExternalNetworks.DataBind();

                List<VirtualSwitch> extSwitches = GetExternalSwitches();
                ddlExternalNetworksPrivate.DataSource = extSwitches;
                ddlExternalNetworksPrivate.DataBind();

                ddlExternalNetworksDmz.DataSource = extSwitches;
                ddlExternalNetworksDmz.DataBind();

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

        private void BindCertificates()
        {
            CertificateInfo[] certificates = ES.Services.VPS2012.GetCertificates(PanelRequest.ServiceId, RemoteServerName);

            if (certificates != null)
            {
                ddlCertThumbnail.Items.Clear();
                certificates.ToList().ForEach(c => ddlCertThumbnail.Items.Add(new ListItem(c.Title, c.Thumbprint)));
            }
        }


        private void BindReplicaServices()
        {
            ddlReplicaServer.Items.Clear();

            ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);
            DataView dvServices = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.VPS2012).Tables[0].DefaultView;

            List<ServiceInfo> services = GetServices(ReplicaServerId);

            foreach (DataRowView dr in dvServices)
            {
                int serviceId = (int)dr["ServiceID"];

                ServiceInfo currentServiceInfo = ES.Services.Servers.GetServiceInfo(serviceId);
                if (currentServiceInfo == null || currentServiceInfo.ProviderId != serviceInfo.ProviderId)
                    continue;

                var currentServiceSettings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettings(serviceId));
                if (currentServiceSettings["ReplicaMode"] != ReplicaMode.IsReplicaServer.ToString())
                    continue;

                var exists = false;
                if (services != null)
                    exists = services.Any(current => current != null && current.ServiceId == serviceId);

                var listItem = new ListItem(dr["FullServiceName"].ToString(), serviceId.ToString()) {Selected = exists};
                ddlReplicaServer.Items.Add(listItem);
            }
        }

        private List<ServiceInfo> GetServices(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            List<ServiceInfo> list = new List<ServiceInfo>();
            string[] servicesIds = data.Split(',');
            foreach (string current in servicesIds)
            {
                ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(Utils.ParseInt(current));
                list.Add(serviceInfo);
            }

            return list;
        }

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary r = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return r;
        }

        private void ToggleControls()
        {
            ServerNameRow.Visible = (radioServer.SelectedIndex == 1);

            txtRamReserve.Enabled = true;
            if (radioServer.SelectedIndex == 0)
            {
                txtServerName.Text = "";                
            }
            else
            {
                txtRamReserve.Text = "0";
                txtRamReserve.Enabled = false;
            }

            // private network
            PrivCustomFormatRow.Visible = (ddlPrivateNetworkFormat.SelectedIndex == 0);

            // dmz network
            DmzCustomFormatRow.Visible = (ddlDmzNetworkFormat.SelectedIndex == 0);

            // management network
            ManageNicConfigRow.Visible = (ddlManagementNetworks.SelectedIndex > 0);
            ManageAlternateNameServerRow.Visible = ManageNicConfigRow.Visible && (ddlManageNicConfig.SelectedIndex == 0);
            ManagePreferredNameServerRow.Visible = ManageNicConfigRow.Visible && (ddlManageNicConfig.SelectedIndex == 0);

            // Replica
            EnableReplicaRow.Visible = EnabledReplica;
            IsReplicaServerRow.Visible = IsReplicaServer;
            ddlCertThumbnail.Visible = CertificateDdlThumbnailValidator.Visible = !IsRemoteServer;
            txtCertThumbnail.Visible = CertificateThumbnailValidator.Visible = IsRemoteServer;
            ReplicaPathErrorTr.Visible = ReplicaErrorTr.Visible = false;
            if (IsReplicaServer) BindCertificates();
            if (EnabledReplica) BindReplicaServices();
         }

        protected string SetSelectedValueIfTimeZoneExis(object TimeID)
        {
            string str = (TimeID != null) ? TimeID.ToString() : "";
            if (string.IsNullOrEmpty(str) && !VirtualMachineTimeZoneList.IsTimeZoneExist(str))
                str = "";

            return str;
        }

        protected bool IsLegacyAdapterSupport(object val)
        {
            bool support = true;
            if (val != null && Utils.ParseInt(val, 0) > 1)
                support = false;
            return support;
        }

        protected bool IsSecureBootEnabled(object val)
        {
            bool enabled = false;
            if (val != null && Utils.ParseBool(val, false)) enabled = true;
            return enabled;
        }

        protected void radioServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void radioSwitchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        protected void radioSwitchTypePrivateNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlExternalNetworksPrivate.Enabled = "external".Equals(radioSwitchTypePrivateNetwork.SelectedValue);
            chkAssignVLANAutomatically.Enabled = ddlExternalNetworksPrivate.Enabled;
            if (ddlExternalNetworksPrivate.Enabled) BindNetworksList();
        }

        protected void radioSwitchTypeDmzNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlExternalNetworksDmz.Enabled = "external".Equals(radioSwitchTypeDmzNetwork.SelectedValue);
            chkDmzAssignVLANAutomatically.Enabled = ddlExternalNetworksDmz.Enabled;
            if (ddlExternalNetworksDmz.Enabled) BindNetworksList();
        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        protected void btnguacamolepassword_Click(object sender, EventArgs e)
        {
            string guacapassword = VPS2012.guacamole.Encryption.GenerateEncryptionKey();
            txtGuacamoleConnectPassword.Text = guacapassword;
        }

        protected void ddlPrivateNetworkFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void ddlDmzNetworkFormat_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void btnSetReplicaServer_Click(object sender, EventArgs e)
        {
            ToggleControls();
            SetUnsetReplication();
        }


        private void SetUnsetReplication()
        {
            if (!IsReplicaServer)
            {
                ES.Services.VPS2012.UnsetReplicaServer(PanelRequest.ServiceId, RemoteServerName);
                return;
            }

            if (txtReplicaPath.Text == "")
            {
                ReplicaPathErrorTr.Visible = true;
                return;
            }

            var thumbprint = IsRemoteServer ? txtCertThumbnail.Text : ddlCertThumbnail.SelectedValue;
            ResultObject result = ES.Services.VPS2012.SetReplicaServer(PanelRequest.ServiceId, RemoteServerName, thumbprint, txtReplicaPath.Text);

            if (!result.IsSuccess)
                ReplicaErrorTr.Visible = true;
        }

        // OS Templates
        protected void btnAddOsTemplate_Click(object sender, EventArgs e)
        {
            var templates = GetOsTemplates();

            templates.Add(new LibraryItem());

            RebindOsTemplate(templates);
        }

        protected void cbEnableSecureBoot_OnChecked(object sender, EventArgs e)
        {
            RebindOsTemplate(GetOsTemplates());
        }

        protected void btnRemoveOsTemplate_OnCommand(object sender, CommandEventArgs e)
        {
            var templates = GetOsTemplates();

            templates.RemoveAt(Convert.ToInt32(e.CommandArgument));

            RebindOsTemplate(templates);
        }

        private List<LibraryItem> GetOsTemplates()
        {
            var result = new List<LibraryItem>();
            
            foreach (RepeaterItem item in repOsTemplates.Items)
            {
                var template = new LibraryItem();
                int processVolume;

                template.Name = GetTextBoxText(item, "txtTemplateName");
                template.Path = GetTextBoxText(item, "txtTemplateFileName");

                int.TryParse(GetTextBoxText(item, "txtProcessVolume"), out processVolume);
                template.ProcessVolume = processVolume;
                template.Generation = GetDropDownListSelectedIndex(item, "ddlTemplateGeneration");
                template.SecureBootTemplate = GetDropDownListSelectedValue(item, "ddlSecureBootTemplate");
                template.LegacyNetworkAdapter = GetCheckBoxValue(item, "chkLegacyNetworkAdapter");

                if (template.Generation != 1)
                {
                    template.EnableSecureBoot = GetCheckBoxValue(item, "chkEnableSecureBoot");
                    template.LegacyNetworkAdapter = false; //Generation 2 does not support legacy adapter!
                }                    
                else
                    template.EnableSecureBoot = false;

                if (!template.EnableSecureBoot) template.SecureBootTemplate = "";

                template.RemoteDesktop = true; // obsolete
                template.ProvisionComputerName = GetCheckBoxValue(item, "chkCanSetComputerName");
                template.ProvisionAdministratorPassword = GetCheckBoxValue(item, "chkCanSetAdminPass");
                template.ProvisionNetworkAdapters = GetCheckBoxValue(item, "chkCanSetNetwork");

                var syspreps = GetTextBoxText(item, "txtSysprep").Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                template.SysprepFiles = syspreps.Select(s=>s.Trim()).ToArray();

                template.VhdBlockSizeBytes = GetBlockSizeBytes(item, "txtVhdBlockSizeBytes");

                int diskSize = 0;
                Int32.TryParse(GetTextBoxText(item, "txtDiskSize"), out diskSize);
                template.DiskSize = diskSize;

                string timeZone = GetDropDownListSelectedValue(item, "ddlTemplateTimeZone");
                template.TimeZoneId = string.IsNullOrEmpty(timeZone) ? GetTextBoxText(item, "txtManualTempplateTimeZone") : timeZone;
                template.CDKey = GetTextBoxText(item, "txtTemplateCDKey");


                result.Add(template);
            }

            return result;
        }

        private uint GetBlockSizeBytes(RepeaterItem item, string name)
        {
            uint VhdBlockSizeBytes;
            string blockSize = GetTextBoxText(item, name);
            if (Utils.IsDigitsOnly(blockSize))
            {
                VhdBlockSizeBytes = Convert.ToUInt32(blockSize);
            }
            else
            {
                string multiple = blockSize.Substring(blockSize.Length - 2);
                blockSize = blockSize.Remove(blockSize.Length - 2);

                try
                {
                    VhdBlockSizeBytes = Convert.ToUInt32(blockSize);
                    switch (multiple.ToUpper())
                    {
                        case "KB":
                            VhdBlockSizeBytes = Utils.ConvertKBytesToBytes(VhdBlockSizeBytes);
                            break;
                        case "MB":
                            VhdBlockSizeBytes = Utils.ConvertMBytesToBytes(VhdBlockSizeBytes);
                            break;
                        default:
                            VhdBlockSizeBytes = 0;
                            break;
                    }
                }
                catch
                {
                    VhdBlockSizeBytes = 0;
                }                
            }

            if (VhdBlockSizeBytes != 0 && VhdBlockSizeBytes < Utils.MinBlockSizeBytes)
                VhdBlockSizeBytes = Utils.MinBlockSizeBytes;

            return VhdBlockSizeBytes;
        }
        
        private void RebindOsTemplate(List<LibraryItem> templates)
        {
            repOsTemplates.DataSource = templates;
            repOsTemplates.DataBind();
        }

        public List<SecureBootTemplate> GetSecureBootTemplatesList()
        {
            if (SecureBootTemplates == null) SecureBootTemplates = new List<SecureBootTemplate>(ES.Services.VPS2012.GetSecureBootTemplates(PanelRequest.ServiceId, txtServerName.Text.Trim()));
            return SecureBootTemplates;
        }

        public List<VirtualSwitch> GetExternalSwitches()
        {
            if (ExternalSwitches == null)
            {
                if (chkGetSwitchesByPS.Checked)
                    ExternalSwitches = new List<VirtualSwitch>(ES.Services.VPS2012.GetExternalSwitches(PanelRequest.ServiceId, txtServerName.Text.Trim()));
                else
                    ExternalSwitches = new List<VirtualSwitch>(ES.Services.VPS2012.GetExternalSwitchesWMI(PanelRequest.ServiceId, txtServerName.Text.Trim()));

            }
            return ExternalSwitches;
        }

        public int GetSecureBootTemplateIndex(object val)
        {
            int index = 0;
            try
            {
                if (val != null)
                {
                    List<SecureBootTemplate> templates = GetSecureBootTemplatesList();
                    for (int i = 0; i < templates.Count; i++)
                    {
                        if (templates[i].Name.Equals(val))
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }
            catch{ }
            return index;
        }

        private string GetConfigXml(List<LibraryItem> items)
        {
            var templates = items.ToArray();
            return new ConfigFile(templates).Xml;
        }
        private int GetDropDownListSelectedIndex(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as DropDownList).SelectedIndex;
        }
        private string GetDropDownListSelectedValue(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as DropDownList).SelectedValue;
        }

        private string GetTextBoxText(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as TextBox).Text;
        }

        private bool GetCheckBoxValue(RepeaterItem item, string name)
        {
            return (item.FindControl(name) as CheckBox).Checked;
        }

        // DVD Library
        protected void btnAddDvd_Click(object sender, EventArgs e)
        {
            var dvds = GetDvds();

            dvds.Add(new LibraryItem());

            RebindDvds(dvds);
        }

        protected void btnRemoveDvd_OnCommand(object sender, CommandEventArgs e)
        {
            var dvds = GetDvds();

            dvds.RemoveAt(Convert.ToInt32(e.CommandArgument));

            RebindDvds(dvds);
        }

        private List<LibraryItem> GetDvds()
        {
            var result = new List<LibraryItem>();

            foreach (RepeaterItem item in repDvdLibrary.Items)
            {
                var dvd = new LibraryItem();

                dvd.Name = GetTextBoxText(item, "txtDvdName");
                dvd.Description = GetTextBoxText(item, "txtDvdDescription");
                dvd.Path = GetTextBoxText(item, "txtDvdFileName");

                result.Add(dvd);
            }

            return result;
        }
        
        private void RebindDvds(List<LibraryItem> dvds)
        {
            repDvdLibrary.DataSource = dvds;
            repDvdLibrary.DataBind();
        }

        //PS Script
        protected void btnAddPsScript_Click(object sender, EventArgs e)
        {
            var psscripts = GetPsScripts();

            psscripts.Add(new LibraryItem());

            RebindPsScripts(psscripts);
        }

        protected void btnRemovePsScript_OnCommand(object sender, CommandEventArgs e)
        {
            var psscripts = GetPsScripts();

            psscripts.RemoveAt(Convert.ToInt32(e.CommandArgument));

            RebindPsScripts(psscripts);
        }

        private List<LibraryItem> GetPsScripts()
        {
            var result = new List<LibraryItem>();

            foreach (RepeaterItem item in repPsScript.Items)
            {
                var psscripts = new LibraryItem();

                psscripts.Name = GetDropDownListSelectedValue(item, "ddlRunAt");
                psscripts.Description = HttpUtility.HtmlEncode(GetTextBoxText(item, "txtPsScript"));

                result.Add(psscripts);
            }

            return result;
        }

        public int GetPsScriptIndex(RepeaterItem item, Object val)
        {
            try
            {
                if (val != null && item != null)
                {
                    DropDownList ddl = (item.FindControl("ddlRunAt") as DropDownList);
                    int i = 0;
                    foreach (ListItem ddlItem in ddl.Items)
                    {
                        if (ddlItem.Value.Equals(val)) return i;
                        i++;
                    }
                }
            }
            catch { }
            return 0;
        }

        private void RebindPsScripts(List<LibraryItem> psscripts)
        {
            repPsScript.DataSource = psscripts;
            repPsScript.DataBind();
        }

        protected void chkGetSwitchesByPS_CheckedChanged(object sender, EventArgs e)
        {
            BindNetworksList();
        }

        protected void chkUseFailoverCluster_CheckedChanged(object sender, EventArgs e)
        {
            tbClusterName.Enabled = chkUseFailoverCluster.Checked;
            ClusterNameValidator.Enabled = chkUseFailoverCluster.Checked;
        }
    }
}
