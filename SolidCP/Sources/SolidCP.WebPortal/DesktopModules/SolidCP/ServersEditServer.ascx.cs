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

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SolidCP.Portal
{
    public partial class ServersEditServer : SolidCPModuleBase
    {
        private const int SERVER_INFO_TIMEOUT = 10000; //10 sec, there is no point to wait too long for just a server information
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //load infromation from the database only, check Timer1_Tick for the rest
                    BindTools();
                    BindServer();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SERVER_GET_SERVER", ex);
                    return;
                }
                Timer1.Enabled = true;
                IPAddressesHeader.IsCollapsed = IsIpAddressesCollapsed;
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e) //we use <asp:UpdatePanel> with <asp:Timer> to post update
        {
            try
            {
                //load information from the server
                BindServerVersion();
                BindServerFilepath();
                BindServerMemory();
            }
            catch (Exception ex)
            {
                divSimpleMessageBox.Visible = true;
                ShowSimpleErrorMessage("SERVER_GET_SERVER", ex);
                updSimpleMessageBox.Update();
            }

            Timer1.Enabled = false;
        }
        //protected void rbUsersCreationMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.rbUsersCreationMode.SelectedValue == "1")
        //    {
        //        this.trAuthType.Visible = true;
        //    }

        //    else
        //    {
        //        this.trAuthType.Visible = false;
        //    }
        //}
        //protected void ddlAdAuthType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.ddlAdAuthType.SelectedValue == "Secure")
        //    {
        //        this.trAddDomain.Visible = true;
        //        this.trAdUserName.Visible = true;
        //        this.trAdPassword.Visible = true;
        //        this.trAdButton.Visible = true;
        //    }
        //    if (this.ddlAdAuthType.SelectedValue == "Delegation")
        //    {
        //        this.trAddDomain.Visible = true;
        //        this.trAdUserName.Visible = true;
        //        this.trAdPassword.Visible = true;
        //        this.trAdButton.Visible = true;
        //    }
        //    else
        //    {
        //        this.trAddDomain.Visible = false;
        //        this.trAdUserName.Visible = false;
        //        this.trAdPassword.Visible = false;
        //        this.trAdButton.Visible = false;
        //    }
        //}
        private void BindTools()
        {
            lnkTerminalSessions.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_termservices");
            lnkWindowsServices.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_winservices");
            lnkWindowsProcesses.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_processes");
            lnkEventViewer.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_eventviewer");
            lnkPlatformInstaller.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_platforminstaller");
            lnkServerReboot.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_reboot");

            lnkBackup.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "backup");
            lnkRestore.NavigateUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "restore");

            lnkBackup.Visible = lnkRestore.Visible = PortalUtils.PageExists("Backup");
        }

        private void BindServer()
        {
            ServerInfo server = ES.Services.Servers.GetServerById(PanelRequest.ServerId);

			if (server == null)
				RedirectToBrowsePage();

            // header
            txtName.Text = PortalAntiXSS.DecodeOld(server.ServerName);
            txtComments.Text = PortalAntiXSS.DecodeOld(server.Comments);

            
            // connection
            txtUrl.Text = server.ServerUrl;

			// AD
            rbUsersCreationMode.SelectedIndex = server.ADEnabled ? 1 : 0;
            Utils.SelectListItem(ddlAdAuthType, server.ADAuthenticationType);
            txtDomainName.Text = server.ADRootDomain;
            txtAdUsername.Text = server.ADUsername;
            txtAdParentDomain.Text = server.ADParentDomain;
            txtAdParentDomainController.Text = server.ADParentDomainController;

            chkUseAdParentDomain.Checked = !string.IsNullOrEmpty(server.ADParentDomain);

            chkUseAdParentDomain_StateChanged(null, null);

            // Preview Domain
            txtPreviewDomain.Text = server.InstantDomainAlias;
        }

		private void BindServerVersion()
		{
            scpVersion.Text = ES.Services.WithTimeout(SERVER_INFO_TIMEOUT).Servers.GetServerVersion(PanelRequest.ServerId);
        }

        private void BindServerMemory()
        {
            freeMemory.Text = "N/A";
            totalMemory.Text = "N/A";

            SystemMemoryInfo memory = null;
            // We need to get the ServiceInfo for VPS2012 servers, because only this way allows access to the Remote Hyper-V API.
            // Otherwise, it will return information about the local server.
            ServiceInfo ServiceInfo = ES.Services.Servers.GetServicesByServerIdGroupName(PanelRequest.ServerId, ResourceGroups.VPS2012).FirstOrDefault();
            if (ServiceInfo != null)
                memory = ES.Services.WithTimeout(SERVER_INFO_TIMEOUT).VPS2012.GetSystemMemoryInfo(ServiceInfo.ServiceId); //this is only immportant for Remote Hyper-V
            else
                memory = ES.Services.WithTimeout(SERVER_INFO_TIMEOUT).Servers.GetSystemMemoryInfo(PanelRequest.ServerId);

            freeMemory.Text = (memory.FreePhysicalKB / 1024).ToString();
            totalMemory.Text = (memory.TotalVisibleSizeKB / 1024).ToString();
            ramGauge.Total = (int)memory.TotalVisibleSizeKB / 1024;
            ramGauge.Progress = (int)((memory.TotalVisibleSizeKB / 1024) - (memory.FreePhysicalKB / 1024));

        }

        private void BindServerFilepath() 
        {
            scpFilepath.Text = ES.Services.WithTimeout(SERVER_INFO_TIMEOUT).Servers.GetServerFilePath(PanelRequest.ServerId);
        }

        private void UpdateServer()
        {
            if (!Page.IsValid)
                return;

            ServerInfo server = new ServerInfo();

            // header
            server.ServerId = PanelRequest.ServerId;
            server.ServerName = txtName.Text;
            server.Comments = txtComments.Text;

            // connection
            server.ServerUrl = txtUrl.Text;

            // AD
            server.ADEnabled = (rbUsersCreationMode.SelectedIndex == 1);
            server.ADAuthenticationType = ddlAdAuthType.SelectedValue;
            server.ADRootDomain = txtDomainName.Text;
            server.ADUsername = txtAdUsername.Text;
            server.ADParentDomain = txtAdParentDomain.Text;

            // Preview Domain
            server.InstantDomainAlias = txtPreviewDomain.Text;

            try
            {
                int result = ES.Services.Servers.UpdateServer(server);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_UPDATE_SERVER", ex);
                return;
            }

            // return to browse page
            RedirectToBrowsePage();
        }

        private void DeleteServer()
        {
            try
            {
                int result = ES.Services.Servers.DeleteServer(PanelRequest.ServerId);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_DELETE_SERVER", ex);
                return;
            }

            RedirectToBrowsePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteServer();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateServer();
        }
        protected void btnUpdate_Click1(object sender, EventArgs e)
        {
            UpdateServer();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectToBrowsePage();
        }

        protected void btnChangeServerPassword_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.Servers.UpdateServerConnectionPassword(
                    PanelRequest.ServerId, serverPassword.Password);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("SERVER_UPDATE_SERVER_PSW");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_UPDATE_SERVER_PSW", ex);
                return;
            }
        }

        protected void btnChangeADPassword_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.Servers.UpdateServerADPassword(
                    PanelRequest.ServerId, adPassword.Password);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("SERVER_UPDATE_AD_PSW");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_UPDATE_AD_PSW", ex);
                return;
            }
        }

        protected bool IsIpAddressesCollapsed
        {
            get
            {
                return PanelRequest.GetBool("IpAddressesCollapsed", true);
            }
        }

        protected void chkUseAdParentDomain_StateChanged(object sender, EventArgs e)
        {
            //divParentDomain.Visible = chkUseAdParentDomain.Checked;
            //trParentDomainController.Visible = chkUseAdParentDomain.Checked;
            lblAdParentDomain.Visible = chkUseAdParentDomain.Checked;
            lblAdParentDomainController.Visible = chkUseAdParentDomain.Checked;
            txtAdParentDomain.Visible = chkUseAdParentDomain.Checked;
            txtAdParentDomainController.Visible = chkUseAdParentDomain.Checked;

            if (!chkUseAdParentDomain.Checked)
            {
                txtAdParentDomain.Text = null;
            }
        }

        private void ShowSimpleErrorMessage(string messageKey, Exception ex)
        {
            string exceptionKey = null;
            litStackTrace.Visible = false;
            if (ex != null)
            {
                if (!String.IsNullOrEmpty(ex.Message) && ex.Message.Contains("SolidCP_ERROR"))
                {
                    string[] messageParts = ex.Message.Split(new char[] { '@' });
                    if (messageParts.Length > 1)
                    {
                        exceptionKey = messageParts[1].TrimStart(new char[] { ' ' });
                    }
                }
                litStackTrace.Visible = true;
                litStackTrace.Text = ex.ToString().Replace("\n", "<br/>");
            }
            string localizedMessage = GetSharedLocalizedString(Utils.ModuleName, "Error." + exceptionKey);
            string localizedDescription = GetSharedLocalizedString(Utils.ModuleName, "ErrorDescription." + exceptionKey);

            if (localizedMessage == null)
            {
                localizedMessage = GetSharedLocalizedString(Utils.ModuleName, "Error." + messageKey);
                localizedDescription = GetSharedLocalizedString(Utils.ModuleName, messageKey);
                if (localizedMessage == null)
                    localizedMessage = messageKey;
            }

            string boxStyle = "MessageBox Red";
            divSimpleMessageBox.Attributes["class"] = boxStyle;
            litMessageBox.Text = localizedMessage;
            litDescriptionBox.Text = !String.IsNullOrEmpty(localizedDescription)
                ? String.Format("<br/><span class=\"description\">{0}</span>", localizedDescription) : "";
        }
    }
}
