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
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using System.Xml;
using System.Configuration;

namespace SolidCP.Portal.ProviderControls
{
	public partial class Exchange2010_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
	    public const string HubTransportsData = "HubTransportsData";
        public const string ClientAccessData = "ClientAccessData";
        public const string SEDestinations = "sedestinations";
        public const int EXCHANGE2010_PROVIDER_ID = 32;
        public const int EXCHANGE2010SP2_PROVIDER_ID = 90;
        public const int EXCHANGE2013_PROVIDER_ID = 91;
        public const int EXCHANGE2016_PROVIDER_ID = 92;

        private string[] ConvertDictionaryToArray(StringDictionary settings)
        {
            List<string> list = new List<string>();
            foreach (string key in settings.Keys)
                list.Add(key + "=" + settings[key]);
            return list.ToArray();
        }

        private StringDictionary ConvertArrayToDictionary(string[] settings)
        {
            StringDictionary list = new StringDictionary();
            foreach (string setting in settings)
            {
                int idx = setting.IndexOf('=');
                list.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String> sed = ViewState[SEDestinations] as List<String>;
            if (sed == null)
            {
                sed = new List<string>();

                StringDictionary settings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettings(PanelRequest.ServiceId));

                bool SpamExpertsEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["SpamExpertsEnable"]);
                if (SpamExpertsEnable == true)
                {
                    string strList = settings[SEDestinations];
                    if (strList != null)
                    {
                        string[] list = strList.Split(',');
                        sed.AddRange(list);
                    }
                    ViewState[SEDestinations] = sed;
                    gvSEDestinations.DataSource = sed;
                    gvSEDestinations.DataBind();
                }
                else
                {
                    SeDiv.Visible = false;
                }
            }

        }

        public string HubTransports
        {
            get
            {
                return ViewState[HubTransportsData] != null ? ViewState[HubTransportsData].ToString() : string.Empty;
            }
            set
            {
                ViewState[HubTransportsData] = value;
            }
        }

        public string ClientAccess
        {
            get
            {
                return ViewState[ClientAccessData] != null ? ViewState[ClientAccessData].ToString() : string.Empty;
            }
            set
            {
                ViewState[ClientAccessData] = value;
            }
        }

        public void BindSettings(StringDictionary settings)
        {            
                ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);
                if (serviceInfo != null)
                {
                    switch (serviceInfo.ProviderId)
                    {
                        case EXCHANGE2010_PROVIDER_ID:
                            clusteredMailboxServer.Visible = false;
                            txtMailboxClusterName.Text = "";
                        
                            storageGroup.Visible = false;
                            txtStorageGroup.Text = "";

                            locMailboxDAG.Visible = false;

                            powershellUrl1.Visible = powershellUrl2.Visible = false;

                            archivingGroup.Visible = false;
                            break;

                        case EXCHANGE2010SP2_PROVIDER_ID:
                            clusteredMailboxServer.Visible = false;
                            txtMailboxClusterName.Text = "";

                            storageGroup.Visible = false;
                            txtStorageGroup.Text = "";

                            locMailboxDatabase.Visible = false;
                            powershellUrl1.Visible = powershellUrl2.Visible = false;

                            archivingGroup.Visible = false;
                            break;

                        case EXCHANGE2013_PROVIDER_ID:
                            clusteredMailboxServer.Visible = false;
                            txtMailboxClusterName.Text = "";

                            storageGroup.Visible = false;
                            txtStorageGroup.Text = "";

                            locMailboxDatabase.Visible = false;
                            powershellUrl1.Visible = powershellUrl2.Visible = true;

                            archivingGroup.Visible = true;
                            break;

                    case EXCHANGE2016_PROVIDER_ID:
                        clusteredMailboxServer.Visible = false;
                        txtMailboxClusterName.Text = "";

                        storageGroup.Visible = false;
                        txtStorageGroup.Text = "";

                        locMailboxDatabase.Visible = false;
                        powershellUrl1.Visible = powershellUrl2.Visible = true;

                        archivingGroup.Visible = true;
                        break;

                    default:
                            storageGroup.Visible = true;
                            txtStorageGroup.Text = settings["StorageGroup"];
                            clusteredMailboxServer.Visible = true;
                            txtMailboxClusterName.Text = settings["MailboxCluster"];
                            locMailboxDAG.Visible = false;

                            archivingGroup.Visible = false;
                            break;
                    }
                }
            
                HubTransports = settings["HubTransportServiceID"];
                ClientAccess = settings["ClientAccessServiceID"];

                // bind exchange services
                BindExchangeServices(ddlHubTransport, true);

                BindExchangeServices(ddlClientAccess, false);

                Utils.SelectListItem(ddlHubTransport, settings["HubTransportServiceID"]);
                Utils.SelectListItem(ddlClientAccess, settings["ClientAccessServiceID"]);

                txtMailboxDatabase.Text = settings["MailboxDatabase"];
                

                txtKeepDeletedItems.Text = settings["KeepDeletedItemsDays"];
                txtKeepDeletedMailboxes.Text = settings["KeepDeletedMailboxesDays"];

                txtSmtpServers.Text = settings["SmtpServers"];
                txtAutodiscoverIP.Text = settings["AutodiscoverIP"];
                txtAutodiscoverDomain.Text = settings["AutodiscoverDomain"];
                txtOwaUrl.Text = settings["OwaUrl"];
                txtActiveSyncServer.Text = settings["ActiveSyncServer"];
                txtOABServer.Text = settings["OABServer"];
                txtPublicFolderServer.Text = settings["PublicFolderServer"];
                txtPowerShellUrl.Text = settings["PowerShellUrl"];

                txtArchivingDatabase.Text = settings["ArchivingDatabase"];

            UpdateHubTransportsGrid();
            UpdateClientAccessGrid();

        }

        public void SaveSettings(StringDictionary settings)
        {
			settings["HubTransportServiceID"] = HubTransports;
			settings["ClientAccessServiceID"] = ClientAccess;			
			settings["MailboxDatabase"] = txtMailboxDatabase.Text.Trim();
            settings["MailboxCluster"] = txtMailboxClusterName.Text.Trim();            
			settings["KeepDeletedItemsDays"] = Utils.ParseInt(txtKeepDeletedItems.Text.Trim(), 14).ToString();
			settings["KeepDeletedMailboxesDays"] = Utils.ParseInt(txtKeepDeletedMailboxes.Text.Trim(), 30).ToString();
            settings["SmtpServers"] = txtSmtpServers.Text;
            settings["AutodiscoverIP"] = txtAutodiscoverIP.Text.Trim();
            settings["AutodiscoverDomain"] = txtAutodiscoverDomain.Text.Trim();
            settings["OwaUrl"] = txtOwaUrl.Text.Trim();
            settings["ActiveSyncServer"] = txtActiveSyncServer.Text.Trim();
            settings["OABServer"] = txtOABServer.Text.Trim();
            settings["PublicFolderServer"] = txtPublicFolderServer.Text;
            settings["StorageGroup"] = txtStorageGroup.Text;
            settings["PowerShellUrl"] = txtPowerShellUrl.Text;
            settings["ArchivingDatabase"] = txtArchivingDatabase.Text;
        }

		public void BindExchangeServices(DropDownList ddl, bool isHubservice)
		{
			ddl.Items.Clear();

            ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);
            DataView dvServices = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.Exchange).Tables[0].DefaultView;

            foreach (DataRowView dr in dvServices)
            {             
                int serviceId = (int) dr["ServiceID"];
                ServiceInfo currentServiceInfo = ES.Services.Servers.GetServiceInfo(serviceId);
                
                if (currentServiceInfo == null || currentServiceInfo.ProviderId != serviceInfo.ProviderId)                
                    continue;
                                
                List<ServiceInfo> services = GetServices(isHubservice ? HubTransports : ClientAccess);
                bool exists = false;
                if (services != null)
                    foreach (ServiceInfo current in services)
                    {                    
                        if (current != null && current.ServiceId == serviceId)
                        {
                            exists = true;
                            break;
                        }
                    }
             
                if (!exists)
                    ddl.Items.Add(new ListItem(dr["FullServiceName"].ToString(), serviceId.ToString()));

            }
		    ddl.Visible = ddl.Items.Count != 0;
            btnAdd.Visible = ddl.Items.Count != 0;
            Button1.Visible = ddl.Items.Count != 0;

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(HubTransports))
                HubTransports += ",";

            HubTransports += ddlHubTransport.SelectedItem.Value;
            UpdateHubTransportsGrid();
            BindExchangeServices(ddlHubTransport, true);

        }

        private void UpdateHubTransportsGrid()
        {
            gvHubTransport.DataSource = GetServices(HubTransports);
            gvHubTransport.DataBind();
        }

        private void UpdateClientAccessGrid()
        {
            gvClients.DataSource = GetServices(ClientAccess);
            gvClients.DataBind();
        }

        public List<ServiceInfo> GetServices(string data)
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

        protected void gvHubTransport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveServer")
            {
                string str = string.Empty;
                List<ServiceInfo> services = GetServices(HubTransports);
                foreach (ServiceInfo current in services)
                {
                    if (current.ServiceId == Utils.ParseInt(e.CommandArgument.ToString()))
                        continue;


                    str += current.ServiceId + ",";
                }

                HubTransports = str.TrimEnd(','); 
                UpdateHubTransportsGrid();
                BindExchangeServices(ddlHubTransport, true);
            }
        }

        protected void btnAddClientAccess_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ClientAccess))
                ClientAccess += ",";
            
            ClientAccess += ddlClientAccess.SelectedItem.Value;
            
            UpdateClientAccessGrid();
            BindExchangeServices(ddlClientAccess, false);

        }

        protected void gvClientAccess_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveServer")
            {
                string str = string.Empty;
                List<ServiceInfo> services = GetServices(ClientAccess);
                foreach (ServiceInfo current in services)
                {
                    if (current.ServiceId == Utils.ParseInt(e.CommandArgument.ToString()))
                        continue;


                    str += current.ServiceId + ",";
                }

                ClientAccess = str.TrimEnd(',');
                UpdateClientAccessGrid();
                BindExchangeServices(ddlClientAccess, false);
            }
        }

        // RB Added for SpamExperts

        protected void gvSEDestinations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteItem":
                    try
                    {
                        string item = e.CommandArgument.ToString();

                        List<String> itemList = ViewState[SEDestinations] as List<String>;
                        if (itemList == null) return;

                        int i = itemList.FindIndex(x => x == item);
                        if (i >= 0) itemList.RemoveAt(i);

                        ViewState[SEDestinations] = itemList;
                        gvSEDestinations.DataSource = itemList;
                        gvSEDestinations.DataBind();
                        SaveSEDestinations();
                    }
                    catch (Exception)
                    {
                    }

                    break;
            }
        }

        protected void bntAddSEDestination_Click(object sender, EventArgs e)
        {
            List<String> res = ViewState[SEDestinations] as List<String>;
            if (res == null) res = new List<String>();

            res.Add(tbSEDestinations.Text);
            ViewState[SEDestinations] = res;
            gvSEDestinations.DataSource = res;
            gvSEDestinations.DataBind();
            SaveSEDestinations();
        }

        protected void SaveSEDestinations()
        {
            List<String> res = ViewState[SEDestinations] as List<String>;
            if (res == null) return;

            StringDictionary settings = new StringDictionary();
            settings.Add(SEDestinations, string.Join(",", res.ToArray()));

            int result = ES.Services.Servers.UpdateServiceSettings(PanelRequest.ServiceId,
                        ConvertDictionaryToArray(settings));
        }

    }
}
