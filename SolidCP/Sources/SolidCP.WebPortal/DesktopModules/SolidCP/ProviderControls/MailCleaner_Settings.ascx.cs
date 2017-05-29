// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Filters;

namespace SolidCP.Portal.ProviderControls
{
    public partial class MailCleaner_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {

        public const string MailCleanerServersData = "MailCleanerServersData";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        { 
            txtServerName.Text = settings[MailCleanerContants.APITitle];
            txtSimpleUrlBase.Text = settings[MailCleanerContants.APIUrl];


            MailCleanerServers = settings["MailCleanerServiceID"];
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[MailCleanerContants.APITitle] = txtServerName.Text.Trim();
            settings[MailCleanerContants.APIUrl] = txtSimpleUrlBase.Text.Trim();

            settings["MailCleanerServiceID"] = MailCleanerServers;			
        }


        public string MailCleanerServers
        {
            get
            {
                return ViewState[MailCleanerServersData] != null ? ViewState[MailCleanerServersData].ToString() : string.Empty;
            }
            set
            {
                ViewState[MailCleanerServersData] = value;
            }
        }


        protected void btnAddMailCleanerServer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MailCleanerServers))
                MailCleanerServers += ",";

            MailCleanerServers += ddlMailCleanerServers.SelectedItem.Value;

            BindMailCleanerServices(ddlMailCleanerServers);

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

        private void UpdateMailCleanerServersGrid()
        {
            gvMailCleanerServers.DataSource = GetServices(MailCleanerServers);
            gvMailCleanerServers.DataBind();
        }


        public void BindMailCleanerServices(DropDownList ddl)
        {
            ddl.Items.Clear();

            ServiceInfo serviceInfo = ES.Services.Servers.GetServiceInfo(PanelRequest.ServiceId);
            DataView dvServices = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.Filters).Tables[0].DefaultView;

            foreach (DataRowView dr in dvServices)
            {
                int serviceId = (int)dr["ServiceID"];
                ServiceInfo currentServiceInfo = ES.Services.Servers.GetServiceInfo(serviceId);

                if (currentServiceInfo == null || currentServiceInfo.ProviderId != serviceInfo.ProviderId)
                    continue;

                List<ServiceInfo> services = GetServices(MailCleanerServers);
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
            btnAddMailCleanerServer.Visible = ddl.Items.Count != 0;

        }

        protected void gvMailCleanerServers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveServer")
            {
                string str = string.Empty;
                List<ServiceInfo> services = GetServices(MailCleanerServers);
                foreach (ServiceInfo current in services)
                {
                    if (current.ServiceId == Utils.ParseInt(e.CommandArgument.ToString()))
                        continue;


                    str += current.ServiceId + ",";
                }

                MailCleanerServers = str.TrimEnd(',');
                UpdateMailCleanerServersGrid();
                BindMailCleanerServices(ddlMailCleanerServers);
            }
        }

    }
}
