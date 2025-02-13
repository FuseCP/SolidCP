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
    public partial class SimpleDNS_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
				RenderFtuNote();
			}
        }

		private void RenderFtuNote()
		{
			string ftuNote = GetLocalizedString("FirsttimeUserNote");
			//
			ServerInfo serverInfo = ES.Services.Servers.GetServerById(PanelRequest.ServerId);
			//
			lblFirsttimeUserNote.InnerHtml = String.Format(ftuNote, serverInfo.ServerName);
		}

        public void BindSettings(StringDictionary settings)
        {
            txtUrl.Text = settings["SimpleDnsUrl"];
            txtLogin.Text = settings["AdminLogin"];
            ViewState["PWD"] = settings["Password"];
            curPassword.Visible = ((string)ViewState["PWD"]) != "";
			txtAllowZoneTransfers.Text = settings["AllowZoneTransfers"];
			txtResponsiblePerson.Text = settings["ResponsiblePerson"];
            intRefresh.Interval = Utils.ParseInt(settings["RefreshInterval"], 0);
            intRetry.Interval = Utils.ParseInt(settings["RetryDelay"], 0);
            intExpire.Interval = Utils.ParseInt(settings["ExpireLimit"], 0);
            intTtl.Interval = Utils.ParseInt(settings["MinimumTTL"], 0);

            //DNS RecordTTL
            txtRecordDefaultTTL.Text = settings["RecordDefaultTTL"] ?? "86400";
            txtRecordMinimumTTL.Text = settings["RecordMinimumTTL"] ?? "3600";

            iPAddressesList.BindSettings(settings);
            secondaryDNSServers.BindSettings(settings);
            nameServers.Value = settings["NameServers"];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["SimpleDnsUrl"] = txtUrl.Text;
            settings["AdminLogin"] = txtLogin.Text.Trim();
            settings["Password"] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
			settings["AllowZoneTransfers"] = txtAllowZoneTransfers.Text;
			settings["ResponsiblePerson"] = txtResponsiblePerson.Text;
            settings["RefreshInterval"] = intRefresh.Interval.ToString();
            settings["RetryDelay"] = intRetry.Interval.ToString();
            settings["ExpireLimit"] = intExpire.Interval.ToString();
            settings["MinimumTTL"] = intTtl.Interval.ToString();

            //DNS RecordTTL
            settings["RecordDefaultTTL"] = txtRecordDefaultTTL.Text;
            settings["RecordMinimumTTL"] = txtRecordMinimumTTL.Text;

            iPAddressesList.SaveSettings(settings);
            secondaryDNSServers.SaveSettings(settings);
            settings["NameServers"] = nameServers.Value;
        }
    }
}
