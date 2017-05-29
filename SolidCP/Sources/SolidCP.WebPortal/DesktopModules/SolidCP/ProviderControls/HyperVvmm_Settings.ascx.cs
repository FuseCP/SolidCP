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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.ProviderControls
{
    public partial class HyperVvmm_Settings : SolidCPControlBase, IHostingServiceProviderSettings
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

        void IHostingServiceProviderSettings.BindSettings(StringDictionary settings)
        {
            txtServerName.Text = settings["ServerName"];
            radioServer.SelectedIndex = (txtServerName.Text == "") ? 0 : 1;

            // bind networks
            BindNetworksList();

            // general settings
            txtVpsRootFolder.Text = settings["RootFolder"];
            txtExportedVpsPath.Text = settings["ExportedVpsPath"];

            // CPU
            txtCpuLimit.Text = settings["CpuLimit"];
            txtCpuReserve.Text = settings["CpuReserve"];
            txtCpuWeight.Text = settings["CpuWeight"];

            // OS Templates
            txtOSTemplatesPath.Text = settings["OsTemplatesPath"];
            repOsTemplates.DataSource = new ConfigFile(settings["OsTemplates"]).LibraryItems; //ES.Services.VPS2012.GetOperatingSystemTemplatesByServiceId(PanelRequest.ServiceId).ToList();
            repOsTemplates.DataBind();

            // DVD library
            txtDvdLibraryPath.Text = settings["DvdLibraryPath"];
            repDvdLibrary.DataSource = new ConfigFile(settings["DvdLibrary"]).LibraryItems;
            repDvdLibrary.DataBind();

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

            // replica
            ReplicationModeList.SelectedValue = settings["ReplicaMode"] ?? ReplicaMode.None.ToString();
            txtReplicaPath.Text = settings["ReplicaServerPath"];
            ReplicaServerId = settings["ReplicaServerId"];

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
            settings["ServerName"] = txtServerName.Text.Trim();

            // general settings
            settings["RootFolder"] = txtVpsRootFolder.Text.Trim();
            settings["ExportedVpsPath"] = txtExportedVpsPath.Text.Trim();

            // CPU
            settings["CpuLimit"] = txtCpuLimit.Text.Trim();
            settings["CpuReserve"] = txtCpuReserve.Text.Trim();
            settings["CpuWeight"] = txtCpuWeight.Text.Trim();

            // OS Templates
            settings["OsTemplates"] = GetConfigXml(GetOsTemplates());
            settings["OsTemplatesPath"] = txtOSTemplatesPath.Text.Trim();

            // DVD library
            settings["DvdLibrary"] = GetConfigXml(GetDvds());
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

            // replication
            settings["ReplicaMode"] = ReplicationModeList.SelectedValue;
            settings["ReplicaServerId"] = ddlReplicaServer.SelectedValue;
            settings["ReplicaServerPath"] = txtReplicaPath.Text;
            settings["ReplicaServerThumbprint"] = CertificateThumbprint;

            SetUnsetReplication();
        }

        private void BindNetworksList()
        {
            try
            {
                VirtualSwitch[] switches = ES.Services.VPS2012.GetExternalSwitches(PanelRequest.ServiceId, txtServerName.Text.Trim());

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

            // Replica
            EnableReplicaRow.Visible = EnabledReplica;
            IsReplicaServerRow.Visible = IsReplicaServer;
            ddlCertThumbnail.Visible = CertificateDdlThumbnailValidator.Visible = !IsRemoteServer;
            txtCertThumbnail.Visible = CertificateThumbnailValidator.Visible = IsRemoteServer;
            ReplicaPathErrorTr.Visible = ReplicaErrorTr.Visible = false;
            if (IsReplicaServer) BindCertificates();
            if (EnabledReplica) BindReplicaServices();
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

                template.LegacyNetworkAdapter = GetCheckBoxValue(item, "chkLegacyNetworkAdapter");
                template.RemoteDesktop = true; // obsolete
                template.ProvisionComputerName = GetCheckBoxValue(item, "chkCanSetComputerName");
                template.ProvisionAdministratorPassword = GetCheckBoxValue(item, "chkCanSetAdminPass");
                template.ProvisionNetworkAdapters = GetCheckBoxValue(item, "chkCanSetNetwork");

                var syspreps = GetTextBoxText(item, "txtSysprep").Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                template.SysprepFiles = syspreps.Select(s=>s.Trim()).ToArray();

                result.Add(template);
            }

            return result;
        }
        private void RebindOsTemplate(List<LibraryItem> templates)
        {
            repOsTemplates.DataSource = templates;
            repOsTemplates.DataBind();
        }

        private string GetConfigXml(List<LibraryItem> items)
        {
            var templates = items.ToArray();
            return new ConfigFile(templates).Xml;
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
    }
}
