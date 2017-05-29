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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ProviderControls
{
    public partial class Common_SecondaryDNSServers : SolidCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindSettings(StringDictionary settings)
        {
            // bind DNS services
            DataView dvServices = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.Dns).Tables[0].DefaultView;
            foreach (DataRowView dr in dvServices)
            {
                int serviceId = (int)dr["ServiceID"];
                if (PanelRequest.ServiceId != serviceId)
                    ddlService.Items.Add(new ListItem(dr["FullServiceName"].ToString(), serviceId.ToString()));
            }

            // bind selected services
            string sids = settings["SecondaryDNSServices"];
            if (sids != null)
            {
                string[] ids = sids.Split(',');

                foreach (string id in ids)
                {
                    ListItem li = ddlService.Items.FindByValue(id);
                    if (li != null)
                    {
                        ddlService.Items.Remove(li);
                        lbServices.Items.Add(li);
                    }
                }
            }
        }

        public void SaveSettings(StringDictionary settings)
        {
            string[] ids = new string[lbServices.Items.Count];
            for (int i = 0; i < lbServices.Items.Count; i++)
                ids[i] = lbServices.Items[i].Value;

            settings["SecondaryDNSServices"] = String.Join(",", ids);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlService.SelectedIndex != -1)
            {
                ListItem li = ddlService.SelectedItem;
                li.Selected = false;
                ddlService.Items.Remove(li);
                lbServices.Items.Add(li);
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbServices.SelectedIndex != -1)
            {
                ListItem li = lbServices.Items[lbServices.SelectedIndex];
                li.Selected = false;
                lbServices.Items.Remove(li);
                ddlService.Items.Add(li);
            }
        }
    }
}
