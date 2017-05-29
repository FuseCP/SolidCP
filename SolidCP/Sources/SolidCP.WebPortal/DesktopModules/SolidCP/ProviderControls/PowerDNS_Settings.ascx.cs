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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ProviderControls
{
    public partial class PowerDNS_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        #region Constants

        //pdns mysql db settings
        const string PDNSDbServer = "PDNSDbServer";
        const string PDNSDbPort = "PDNSDbPort";
        const string PDNSDbName = "PDNSDbName";
        const string PDNSDbUser = "PDNSDbUser";
        const string PDNSDbPassword = "PDNSDbPassword";

        //soa record settings
        const string ResponsiblePerson = "ResponsiblePerson";
        const string RefreshInterval = "RefreshInterval";
        const string RetryDelay = "RetryDelay";
        const string ExpireLimit = "ExpireLimit";
        const string MinimumTTL = "MinimumTTL";

        //name servers
        const string cNameServers = "NameServers";

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            RenderValidationJavaScrip();
        }

        private void RenderValidationJavaScrip()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("pdnsValidationFunctions"))
            {
                StringBuilder jsFunctions = new StringBuilder();

                jsFunctions.Append("<script type=\"text/javascript\"> ");
                jsFunctions.Append("function pdnsComparePasswordFields(source, args) {");
                jsFunctions.Append(" var txtPwd = document.getElementById('" + txtPassword.ClientID + "');");
                jsFunctions.Append(" var txtCPwd = document.getElementById('" + txtConfirmPassword.ClientID + "');");
                jsFunctions.Append(" var result = true;");
                jsFunctions.Append(" if (txtPwd.value != '' && txtCPwd.value == '') {");
                jsFunctions.Append("  result = false;");
                jsFunctions.Append(" }");
                jsFunctions.Append(" args.IsValid = result;");
                jsFunctions.Append("} ");
                jsFunctions.Append("</");
                jsFunctions.Append("script>");

                Page.ClientScript.RegisterClientScriptBlock(
                      typeof(PowerDNS_Settings)
                    , "pdnsValidationFunctions"
                    , jsFunctions.ToString()
                    , false
                );
            }
        }

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
            //server settings
            txtServerAddress.Text = settings[PDNSDbServer];

            txtServerPort.Text = settings[PDNSDbPort];

            txtDatabase.Text = settings[PDNSDbName];
            txtUsername.Text = settings[PDNSDbUser];

            ViewState[PDNSDbPassword] = settings[PDNSDbPassword];

            if (!string.IsNullOrEmpty((string)ViewState[PDNSDbPassword]))
            {
                trCurrentPassword.Visible = true;
                varRequirePassword.Enabled = false;
            }
            else
            {
                varRequirePassword.Enabled = true;
                trCurrentPassword.Visible = false;
            }

            //soa record settings
            txtResponsiblePerson.Text = settings[ResponsiblePerson];
            intRefresh.Interval = Utils.ParseInt(settings[RefreshInterval], 0);
            intRetry.Interval = Utils.ParseInt(settings[RetryDelay], 0);
            intExpire.Interval = Utils.ParseInt(settings[ExpireLimit], 0);
            intTtl.Interval = Utils.ParseInt(settings[MinimumTTL], 0);


            //name servers
            nameServers.Value = settings[cNameServers];


            //ip address settings
            secondaryDNSServers.BindSettings(settings);
            iPAddressesList.BindSettings(settings);
        }

        public void SaveSettings(StringDictionary settings)
        {
            //server settings
            settings[PDNSDbServer] = txtServerAddress.Text;

            int port = 3306;
            if (!Int32.TryParse(txtServerPort.Text, out port))
            {
                port = 3306;
            }
            settings[PDNSDbPort] = port.ToString();


            settings[PDNSDbName] = txtDatabase.Text;
            settings[PDNSDbUser] = txtUsername.Text;
            settings[PDNSDbPassword] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState[PDNSDbPassword];


            
            //soa record settings
            settings[ResponsiblePerson] = txtResponsiblePerson.Text;
            settings[RefreshInterval] = intRefresh.Interval.ToString();
            settings[RetryDelay] = intRetry.Interval.ToString();
            settings[ExpireLimit] = intExpire.Interval.ToString();
            settings[MinimumTTL] = intTtl.Interval.ToString();


            //ip address settings
            secondaryDNSServers.SaveSettings(settings);
            iPAddressesList.SaveSettings(settings);  


            //name servers
            settings[cNameServers] = nameServers.Value;
        }
    }
}
