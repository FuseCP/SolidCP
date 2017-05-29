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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Portal.UserControls.ScheduleTaskView;

namespace SolidCP.Portal.ScheduleTaskControls
{
    public partial class DomainLookupView : EmptyView
    {
        private static readonly string DnsServersParameter = "DNS_SERVERS";
        private static readonly string MailToParameter = "MAIL_TO";
        private static readonly string ServerNameParameter = "SERVER_NAME";
        private static readonly string PauseBetweenQueriesParameter = "PAUSE_BETWEEN_QUERIES";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets scheduler task parameters on view.
        /// </summary>
        /// <param name="parameters">Parameters list to be set on view.</param>
        public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
        {
            base.SetParameters(parameters);

            this.SetParameter(this.txtDnsServers, DnsServersParameter);
            this.SetParameter(this.txtMailTo, MailToParameter);
            this.SetParameter(this.txtPause, PauseBetweenQueriesParameter);
            this.SetParameter(this.ddlServers, ServerNameParameter);

            var servers = ES.Services.Servers.GetAllServers();

            var osGroup = ES.Services.Servers.GetResourceGroups().First(x => x.GroupName == ResourceGroups.Os);
            var osProviders = ES.Services.Servers.GetProvidersByGroupId(osGroup.GroupId);

            var osServers = new List<ServerInfo>();

            foreach (var server in servers)
            {
                var services = ES.Services.Servers.GetServicesByServerId(server.ServerId);

                if (services.Any(x => osProviders.Any(p=>p.ProviderId  == x.ProviderId)))
                {
                    osServers.Add(server);
                }
            }

            ddlServers.DataSource = osServers.Select(x => new { Id = x.ServerName, Name = x.ServerName });
            ddlServers.DataTextField = "Name";
            ddlServers.DataValueField = "Id";
            ddlServers.DataBind();

            ScheduleTaskParameterInfo parameter = this.FindParameterById(ServerNameParameter);

            ddlServers.SelectedValue = parameter.ParameterValue;
        }

        /// <summary>
        /// Gets scheduler task parameters from view.
        /// </summary>
        /// <returns>Parameters list filled  from view.</returns>
        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo dnsServers = this.GetParameter(this.txtDnsServers, DnsServersParameter);
            ScheduleTaskParameterInfo mailTo = this.GetParameter(this.txtMailTo, MailToParameter);
            ScheduleTaskParameterInfo serverName = this.GetParameter(this.ddlServers, ServerNameParameter);
            ScheduleTaskParameterInfo pause = this.GetParameter(this.txtPause, PauseBetweenQueriesParameter);

            return new ScheduleTaskParameterInfo[4] { dnsServers, mailTo, serverName, pause };
        }
    }
}
