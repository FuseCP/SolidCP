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
    public partial class MySQL_Settings : SolidCPControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
				//RenderFtuNote();
			}
        }

		/*private void RenderFtuNote()
		{
			string ftuNote = GetLocalizedString("FirsttimeUserNote");
			//
			ServerInfo serverInfo = ES.Services.Servers.GetServerById(PanelRequest.ServerId);
			//
			lblFirsttimeUserNote.InnerHtml = String.Format(ftuNote, serverInfo.ServerName);
		}*/

        public void BindSettings(StringDictionary settings)
        {
            txtInternalAddress.Text = settings["InternalAddress"];
            txtExternalAddress.Text = settings["ExternalAddress"];
            txtBinFolder.Text = settings["InstallFolder"];
			chkOldPassword.Checked = Utils.ParseBool(settings["OldPassword"], false);
            chkSslMode.Checked = Utils.ParseBool(settings["SslMode"], false);
            txtUserName.Text = settings["RootLogin"];
            ViewState["PWD"] = settings["RootPassword"];
            rowPassword.Visible = ((string)ViewState["PWD"]) != "";

			txtBrowseUrl.Text = settings["BrowseURL"];
			Utils.SelectListItem(ddlBrowseMethod, settings["BrowseMethod"]);
			txtBrowseParameters.Text = settings["BrowseParameters"];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["InternalAddress"] = txtInternalAddress.Text.Trim();
            settings["ExternalAddress"] = txtExternalAddress.Text.Trim();
            settings["InstallFolder"] = txtBinFolder.Text.Trim();
			settings["OldPassword"] = chkOldPassword.Checked.ToString();
            settings["SslMode"] = chkSslMode.Checked.ToString();
            settings["RootLogin"] = txtUserName.Text.Trim();
            settings["RootPassword"] = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
			settings["BrowseURL"] = txtBrowseUrl.Text.Trim();
			settings["BrowseMethod"] = ddlBrowseMethod.SelectedValue;
			settings["BrowseParameters"] = txtBrowseParameters.Text;
        }
    }
}
