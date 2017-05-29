// Copyright (c) 2017, centron GmbH
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
    public partial class Proxmox_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
        void IHostingServiceProviderSettings.BindSettings(StringDictionary settings)
        {

            // Proxmox Cluster Settings
            txtProxmoxClusterServerHost.Text = settings["ProxmoxClusterServerHost"];
            txtProxmoxClusterServerPort.Text = settings["ProxmoxClusterServerPort"];
            txtProxmoxClusterAdminUser.Text = settings["ProxmoxClusterAdminUser"];
            txtProxmoxClusterRealm.Text = settings["ProxmoxClusterRealm"];
            ViewState["PWD"] = settings["ProxmoxClusterAdminPass"];
            rowPassword.Visible = ((string)ViewState["PWD"]) != "";

            // Proxmox SSH Settings
            txtDeploySSHServerHost.Text = settings["DeploySSHServerHost"];
            txtDeploySSHServerPort.Text = settings["DeploySSHServerPort"];
            txtDeploySSHUser.Text = settings["DeploySSHUser"];
            ViewState["SSHPWD"] = settings["DeploySSHPass"];
            rowSSHPassword.Visible = ((string)ViewState["SSHPWD"]) != "";
            txtDeploySSHKey.Text = settings["DeploySSHKey"];
            ViewState["SSHKEYPWD"] = settings["DeploySSHKeyPass"];
            rowSSHKEYPassword.Visible = ((string)ViewState["SSHKEYPWD"]) != "";
            txtDeploySSHScript.Text = settings["DeploySSHScript"];
            txtDeploySSHScriptParams.Text = settings["DeploySSHScriptParams"];

            // OS Templates
            //txtOSTemplatesPath.Text = settings["OsTemplatesPath"];
            repOsTemplates.DataSource = new ConfigFile(settings["OsTemplates"]).LibraryItems; //ES.Services.VPS2012.GetOperatingSystemTemplatesByServiceId(PanelRequest.ServiceId).ToList();
            repOsTemplates.DataBind();

            // DVD Path
            txtProxmoxIsosonStorage.Text = settings["ProxmoxIsosonStorage"];

            // host name
            txtHostnamePattern.Text = settings["HostnamePattern"];

        }

        void IHostingServiceProviderSettings.SaveSettings(StringDictionary settings)
        {

            // Proxmox Cluster Settings
            settings["ProxmoxClusterServerHost"] = txtProxmoxClusterServerHost.Text.Trim();
            settings["ProxmoxClusterServerPort"] = txtProxmoxClusterServerPort.Text.Trim();
            settings["ProxmoxClusterAdminUser"] = txtProxmoxClusterAdminUser.Text.Trim();
            settings["ProxmoxClusterRealm"] = txtProxmoxClusterRealm.Text.Trim();
            settings["ProxmoxClusterAdminPass"] = (txtProxmoxClusterAdminPass.Text.Length > 0) ? txtProxmoxClusterAdminPass.Text : (string)ViewState["PWD"];

            // Proxmox SSH Settings
            settings["DeploySSHServerHost"] = txtDeploySSHServerHost.Text.Trim();
            settings["DeploySSHServerPort"] = txtDeploySSHServerPort.Text.Trim();
            settings["DeploySSHUser"] = txtDeploySSHUser.Text.Trim();
            settings["DeploySSHPass"] = (txtDeploySSHPass.Text.Length > 0) ? txtDeploySSHPass.Text : (string)ViewState["SSHPWD"];
            if (chkdelsshpass.Checked == true)
                settings["DeploySSHPass"] = "";
            settings["DeploySSHKey"] = txtDeploySSHKey.Text.Trim();
            settings["DeploySSHKeyPass"] = (txtDeploySSHKeyPass.Text.Length > 0) ? txtDeploySSHKeyPass.Text : (string)ViewState["SSHKEYPWD"];
            if (chkdelsshkeypass.Checked == true)
                settings["DeploySSHKeyPass"] = "";
            settings["DeploySSHScript"] = txtDeploySSHScript.Text.Trim();
            settings["DeploySSHScriptParams"] = txtDeploySSHScriptParams.Text.Trim();

            // OS Templates
            settings["OsTemplates"] = GetConfigXml(GetOsTemplates());
            //settings["OsTemplatesPath"] = txtOSTemplatesPath.Text.Trim();

            // DVD Path
            settings["ProxmoxIsosonStorage"] = txtProxmoxIsosonStorage.Text.Trim();

            // host name
            settings["HostnamePattern"] = txtHostnamePattern.Text.Trim();

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

                template.DeployScriptParams = GetTextBoxText(item, "txtDeployScriptParams");

                int.TryParse(GetTextBoxText(item, "txtProcessVolume"), out processVolume);
                template.ProcessVolume = processVolume;

                template.LegacyNetworkAdapter = GetCheckBoxValue(item, "chkLegacyNetworkAdapter");
                template.RemoteDesktop = true; // obsolete
                template.ProvisionComputerName = GetCheckBoxValue(item, "chkCanSetComputerName");
                template.ProvisionAdministratorPassword = GetCheckBoxValue(item, "chkCanSetAdminPass");
                template.ProvisionNetworkAdapters = GetCheckBoxValue(item, "chkCanSetNetwork");
                

                var syspreps = GetTextBoxText(item, "txtSysprep").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                template.SysprepFiles = syspreps.Select(s => s.Trim()).ToArray();

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


    }
}
