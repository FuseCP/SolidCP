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
using System.Threading;
using System.Threading.Tasks;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.OS;
using SolidCP.Server;
using System.Linq;

namespace SolidCP.Portal
{
	public partial class ServersEditServer : SolidCPModuleBase
	{
		int ServerId;
		Task<ServerInfo> serverInfo = null;
		async Task<ServerInfo> ServerInfo()
		{
			try
			{
				lock (this)
				{
					if (serverInfo == null)
					{
						serverInfo = ES.Services.Servers.GetServerByIdAsync(ServerId);
					}
				}
				return await serverInfo;
			}
			catch (Exception ex)
			{
				throw;

			}
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			Page.Load += PageLoadAsync;
		}
		protected async void PageLoadAsync(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					ServerId = PanelRequest.ServerId;
					await Task.WhenAll(
						BindTools(),
						BindServer(),
						BindServerMemory(),
						BindServerVersion(),
						BindServerFilepath());
				}
				catch (Exception ex)
				{
					ShowErrorMessage("SERVER_GET_SERVER", ex);
					return;
				}

				IPAddressesHeader.IsCollapsed = IsIpAddressesCollapsed;
			}
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
		private async Task BindTools()
		{
			try
			{
				lnkTerminalSessions.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "edit_termservices");

				lnkWindowsServices.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "edit_winservices");
				lnkUnixServices.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "edit_winservices");
				lnkWindowsProcesses.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "edit_processes");
				lnkEventViewer.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "edit_eventviewer");
				lnkPlatformInstaller.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "edit_platforminstaller");
				lnkServerReboot.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "edit_reboot");

				lnkBackup.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "backup");
				lnkRestore.NavigateUrl = EditUrl("ServerID", ServerId.ToString(), "restore");

				lnkBackup.Visible = lnkRestore.Visible = PortalUtils.PageExists("Backup");

				var serverInfo = await ServerInfo();
				pnPlatformPanel.Visible = pnTerminalPanel.Visible = pnWindowsServices.Visible = serverInfo.OSPlatform == OSPlatform.Windows;
				pnUnixServices.Visible = serverInfo.OSPlatform != OSPlatform.Windows;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private async Task BindServer()
		{
			ServerInfo server = await ServerInfo();

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

		private async Task BindServerVersion()
		{
			scpVersion.Text = await ES.Services.Servers.GetServerVersionAsync(ServerId);
		}

		private async Task BindServerMemory()
		{
			try
			{
				//Memory memory = await ES.Services.Servers.GetMemoryAsync(ServerId);
                Memory memory = null;
                // We need to get the ServiceInfo for VPS2012 servers, because only this way allows access to the Remote Hyper-V API.
                // Otherwise, it will return information about the local server.
                ServiceInfo ServiceInfo = await ES.Services.Servers.GetServicesByServerIdGroupNameAsync(PanelRequest.ServerId, ResourceGroups.VPS2012).FirstOrDefault();
                if (ServiceInfo != null)
                    memory = await ES.Services.VPS2012.GetMemoryAsync(ServiceInfo.ServiceId); //this is only immportant for Remote Hyper-V
                else
                    memory = await ES.Services.Servers.GetMemoryAsync(PanelRequest.ServerId);

				freeMemory.Text = (memory.FreePhysicalMemoryKB / 1024).ToString();
				totalMemory.Text = (memory.TotalVisibleMemorySizeKB / 1024).ToString();
				ramGauge.Total = (int)memory.TotalVisibleMemorySizeKB / 1024;
				ramGauge.Progress = (int)((memory.TotalVisibleMemorySizeKB / 1024) - (memory.FreePhysicalMemoryKB / 1024));
			}
			catch (Exception ex)
			{
				freeMemory.Text = "N/A";
				totalMemory.Text = "N/A";
			}
		}

		private async Task BindServerFilepath()
		{
			scpFilepath.Text = await ES.Services.Servers.GetServerFilePathAsync(ServerId);
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

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectToBrowsePage();
		}

		protected void btnDiscoverServices_Click(object sender, EventArgs e)
		{
			try
			{
				int result = ES.Services.Servers.DiscoverAndAddServices(PanelRequest.ServerId);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				} else
				{
					ServerServicesControl.BindServices();
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_DISCOVER_SERVICES", ex);
				return;
			}
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
	}
}
