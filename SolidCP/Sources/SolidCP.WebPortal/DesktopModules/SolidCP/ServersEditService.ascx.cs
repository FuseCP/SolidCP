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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading.Tasks;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
	public partial class ServersEditService : SolidCPModuleBase
	{

		int ServiceId;
		Task<ServiceInfo> service = null;
		Task<ProviderInfo> provider = null;
		Task<ResourceGroupInfo> resourceGroup = null;

		async Task<ServiceInfo> Service()
		{
			lock (this)
			{
				if (service == null)
				{
					service = ES.Services.Servers.GetServiceInfoAsync(ServiceId);
				}
			}
			return await service;
		}

		async Task<ProviderInfo> Provider()
		{
			var service = await Service();
			lock (this)
			{
				if (provider == null)
					provider = ES.Services.Servers.GetProviderAsync(service.ProviderId);
			}
			return await provider;
		}
		async Task<ResourceGroupInfo> ResourceGroup()
		{
			var provider = await Provider();
			lock (this)
			{
				if (resourceGroup == null)
				{
					resourceGroup = ES.Services.Servers.GetResourceGroupAsync(provider.GroupId);
				}
			}
			return await resourceGroup;
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			Page.Load += PageLoadAsync;
		}

		Task loadService, loadSettingsControl;
		protected async void PageLoadAsync(object sender, EventArgs e)
		{
			try
			{
				ServiceId = PanelRequest.ServiceId;
				// load service settings control
				loadService = LoadService();
				loadSettingsControl = LoadSettingsControl();

				rowInstallResults.Visible = false;

				if (!IsPostBack)
				{
					await Task.WhenAll(
						BindClusters(),
						BindService(),
						BindServiceProperties(),
						BindServiceQuota(),
						ToggleGlobalDNS());
				}

				await Task.WhenAll(loadService, loadSettingsControl);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_GET_SERVICE", ex);
				return;
			}
		}

		private async Task LoadService()
		{
			var service = await Service();

			if (service == null)
				// return
				RedirectBack();

			// load provider details
			var provider = await Provider();

			// load resource group details
			var resourceGroup = await ResourceGroup();
		}

		private async Task BindService()
		{
			var resourceGroup = await ResourceGroup();
			var provider = await Provider();

			litGroup.Text = PanelFormatter.GetLocalizedResourceGroupName(resourceGroup.GroupName);

			if (ResourceGroups.VPS2012 == resourceGroup.GroupName || ResourceGroups.Os == resourceGroup.GroupName)
			{
				textProvider.Visible = false;
				ddlProviders.DataSource = ES.Services.Servers.GetProvidersByGroupId(provider.GroupId);
				ddlProviders.DataBind();
				ddlProviders.SelectedValue = provider.ProviderId.ToString();
			}
			else
			{
				selectProvider.Visible = false;
				litProvider.Text = provider.DisplayName;
			}

			var service = await Service();

			txtServiceName.Text = service.ServiceName;
			txtQuotaValue.Text = service.ServiceQuotaValue.ToString();
			Utils.SelectListItem(ddlClusters, service.ClusterId);
			txtComments.Text = service.Comments;
		}

		private async Task BindServiceQuota()
		{
			var provider = await Provider();

			QuotaInfo quota = await ES.Services.Servers.GetProviderServiceQuotaAsync(provider.ProviderId);
			if (quota != null)
			{
				lblQuotaName.Text = GetSharedLocalizedString(Utils.ModuleName, "Quota." + quota.QuotaName);
			}
			else
			{
				pnlQuota.Visible = false;
			}
		}

		private async Task LoadSettingsControl()
		{
			try
			{
				var provider = await Provider();

				// try to locate suitable control
				string currPath = this.AppRelativeVirtualPath;
				currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
				string ctrlPath = currPath + "/ProviderControls/" + provider.EditorControl + "_Settings.ascx";

				IHostingServiceProviderSettings ctrl =
					 (IHostingServiceProviderSettings)Page.LoadControl(ctrlPath);

				// add control to the placeholder
				serviceProps.Controls.Add((Control)ctrl);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_LOAD_SERVICE_CONTROL", ex);
				return;
			}
		}

		private async Task BindServiceProperties()
		{
			await loadSettingsControl;

			// find control
			IHostingServiceProviderSettings ctrl = serviceProps.Controls[0] as IHostingServiceProviderSettings;
			if (ctrl == null)
				return;

			// load service properties and bind them
			string[] settings = await ES.Services.Servers.GetServiceSettingsAsync(ServiceId);

			// bind
			ctrl.BindSettings(ConvertArrayToDictionary(settings));
		}


		private async Task ToggleGlobalDNS()
		{
			var resourceGroup = await ResourceGroup();

			DnsRecrodsPanel.Visible = DnsRecrodsHeader.Visible = ((resourceGroup.GroupName == ResourceGroups.BlackBerry) |
																				 (resourceGroup.GroupName == ResourceGroups.OCS) |
																				 (resourceGroup.GroupName == ResourceGroups.HostedCRM) |
																				 (resourceGroup.GroupName == ResourceGroups.Os) |
																				 (resourceGroup.GroupName == ResourceGroups.HostedOrganizations) |
																				 (resourceGroup.GroupName == ResourceGroups.SharepointFoundationServer) |
																				 (resourceGroup.GroupName == ResourceGroups.SharepointEnterpriseServer) |
																				 (resourceGroup.GroupName == ResourceGroups.Mail) |
																				 (resourceGroup.GroupName == ResourceGroups.Lync) |
																				 (resourceGroup.GroupName == ResourceGroups.SfB) |
																				 (resourceGroup.GroupName == ResourceGroups.Exchange) |
																				 (resourceGroup.GroupName == ResourceGroups.Web) |
																				 (resourceGroup.GroupName == ResourceGroups.Dns) |
																				 (resourceGroup.GroupName == ResourceGroups.Ftp) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2000) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2005) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2008) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2012) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2014) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2016) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2017) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2019) |
																				 (resourceGroup.GroupName == ResourceGroups.MsSql2022) |
																				 (resourceGroup.GroupName == ResourceGroups.MySql4) |
																				 (resourceGroup.GroupName == ResourceGroups.MySql5) |
																				 (resourceGroup.GroupName == ResourceGroups.MySql8) |
																				 (resourceGroup.GroupName == ResourceGroups.MariaDB) |
																				 (resourceGroup.GroupName == ResourceGroups.Statistics) |
																				 (resourceGroup.GroupName == ResourceGroups.VPS) |
																				 (resourceGroup.GroupName == ResourceGroups.VPS2012) |
																				 (resourceGroup.GroupName == ResourceGroups.VPSForPC) |
																				 (resourceGroup.GroupName == ResourceGroups.RDS) |
																				 (resourceGroup.GroupName == ResourceGroups.EnterpriseStorage) |
																				 (resourceGroup.GroupName == ResourceGroups.Filters) |
																				 (resourceGroup.GroupName == ResourceGroups.SharePoint) |
																				 (resourceGroup.GroupName == ResourceGroups.SharepointServer) |
																				 (resourceGroup.GroupName == ResourceGroups.StorageSpaces)
																				 );
		}


		private void SaveServiceProperties()
		{
			// find control
			try
			{
				IHostingServiceProviderSettings ctrl = serviceProps.Controls[0] as IHostingServiceProviderSettings;
				if (ctrl == null)
					return;

				// grab settings
				StringDictionary settings = new StringDictionary();
				ctrl.SaveSettings(settings);

				// save settings
				int result = ES.Services.Servers.UpdateServiceSettings(PanelRequest.ServiceId,
					 ConvertDictionaryToArray(settings));

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_UPDATE_SERVICE_PROPS", ex);
				return;
			}
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			// validate input
			if (!Page.IsValid)
				return;

			var service = new ServiceInfo();
			service.ServiceId = PanelRequest.ServiceId;
			service.ServiceName = txtServiceName.Text.Trim();
			if (ddlProviders.Items.Count > 0)
				service.ProviderId = Utils.ParseInt(ddlProviders.SelectedValue, 0);
			else
				service.ProviderId = 0; //just to be sure that here is 0
			service.ServiceQuotaValue = Utils.ParseInt(txtQuotaValue.Text, 0);
			service.ClusterId = Utils.ParseInt(ddlClusters.SelectedValue, 0);
			service.Comments = txtComments.Text;

			// update service
			try
			{
				int result = ES.Services.Servers.UpdateService(service);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_UPDATE_SERVICE", ex);
				return;
			}

			// save properties
			SaveServiceProperties();

			// install service
			string[] installResults = null;
			try
			{
				installResults = ES.Services.Servers.InstallService(PanelRequest.ServiceId);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_INSTALL_SERVICE", ex);
				return;
			}

			// check results
			if (installResults != null && installResults.Length > 0)
			{
				rowInstallResults.Visible = true;
				blInstallResults.Items.Clear();
				foreach (string installResult in installResults)
					blInstallResults.Items.Add(installResult);

				return;
			}
			// save quotas
			//SaveServiceQuotas();

			// return
			RedirectBack();
		}
		protected void btnCancel_Click(object sender, EventArgs e)
		{
			// return
			RedirectBack();
		}
		protected void btnDelete_Click(object sender, EventArgs e)
		{
			if (PanelRequest.ServiceId != 0)
			{
				// delete service
				try
				{
					int result = ES.Services.Servers.DeleteService(PanelRequest.ServiceId);
					if (result < 0)
					{
						ShowResultMessage(result);
						return;
					}
				}
				catch (Exception ex)
				{
					ShowErrorMessage("SERVER_DELETE_SERVICE", ex);
					return;
				}
			}

			// return
			RedirectBack();
		}

		private void RedirectBack()
		{
			// redirect to the previous page
			Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server"));
		}

		#region Cluster methods
		private async Task BindClusters()
		{
			try
			{
				ddlClusters.DataSource = await ES.Services.Servers.GetClustersAsync();
				ddlClusters.DataBind();

				ddlClusters.Items.Insert(0, new ListItem("<Not Included>", ""));
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_GET_CLUSTER", ex);
				return;
			}
		}
		protected void cmdAddCluster_Click(object sender, EventArgs e)
		{
			ClusterInfo cluster = new ClusterInfo();
			cluster.ClusterName = txtClusterName.Text.Trim();

			try
			{
				int result = ES.Services.Servers.AddCluster(cluster);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_ADD_CLUSTER", ex);
				return;
			}

			// rebind
			BindClusters();
			txtClusterName.Text = "";
		}
		protected void cmdDeleteCluster_Click(object sender, EventArgs e)
		{
			try
			{
				int result = ES.Services.Servers.DeleteCluster(Utils.ParseInt(ddlClusters.SelectedValue, 0));
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SERVER_DELETE_CLUSTER", ex);
				return;
			}

			// rebind
			BindClusters();
		}
		#endregion

		#region Helper methods
		private string[] ConvertDictionaryToArray(StringDictionary settings)
		{
			List<string> r = new List<string>();
			foreach (string key in settings.Keys)
				r.Add(key + "=" + settings[key]);
			return r.ToArray();
		}

		private StringDictionary ConvertArrayToDictionary(string[] settings)
		{
			StringDictionary r = new StringDictionary();
			foreach (string setting in settings)
			{
				int idx = setting.IndexOf('=');
				r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
			}
			return r;
		}
		#endregion
	}
}
