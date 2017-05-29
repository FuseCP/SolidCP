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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal.ProviderControls
{
    public partial class OCS_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
        public string EDGEServices
        {
            get
            {
                return ViewState[OCSConstants.EDGEServicesData] != null ? ViewState[OCSConstants.EDGEServicesData].ToString() : string.Empty;
            }
            set
            {
                ViewState[OCSConstants.EDGEServicesData] = value;
            }
        }

        public ServiceInfo[] GetEDGEServices()
        {
            List<ServiceInfo> list = new List<ServiceInfo>();
            string[] services = EDGEServices.Split(';');
            foreach (string current in services)
            {
                string[] data = current.Split(',');
                if (data.Length > 1)
                    list.Add(new ServiceInfo() {ServiceId = Utils.ParseInt(data[1]), ServiceName = data[0]});
            }


            return list.ToArray();
        }

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtServerName.Text = settings[OCSConstants.PoolFQDN];
            EDGEServices = settings[OCSConstants.EDGEServicesData];
            BindOCSEdgeServices(ddlEdgeServers);
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            gvEdgeServices.DataSource = GetEDGEServices();            
            gvEdgeServices.DataBind();   
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[OCSConstants.EDGEServicesData] = EDGEServices;
            settings[OCSConstants.PoolFQDN] = txtServerName.Text;
        }

        public void BindOCSEdgeServices(DropDownList ddl)
        {
            ddl.Items.Clear();
            DataView dvServices =
                ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.OCS).Tables[0].DefaultView;
            foreach (DataRowView dr in dvServices)
            {
                if (dr["ProviderName"].ToString() != OCSConstants.ProviderName)
                    continue;

                int serviceId = (int) dr["ServiceID"];
                ServiceInfo[] services = GetEDGEServices();
                bool exists = false;
                foreach (ServiceInfo current in services)
                {
                    if (current.ServiceId == serviceId)
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

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            EDGEServices += ddlEdgeServers.SelectedItem.Text + "," + ddlEdgeServers.SelectedItem.Value + ";";
            UpdateGrid();
            BindOCSEdgeServices(ddlEdgeServers);
            
        }

        protected void gvEdgeServices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveServer")
            {
                string str = string.Empty;
                ServiceInfo []services = GetEDGEServices();
                foreach(ServiceInfo current in services)
                {
                    if (current.ServiceId == Utils.ParseInt(e.CommandArgument.ToString()))                                          
                        continue;
                                        

                    str += current.ServiceName + "," + current.ServiceId + ";";
                }

                EDGEServices = str;
                UpdateGrid();
                BindOCSEdgeServices(ddlEdgeServers);
            }
        }

        
        
    }
}
