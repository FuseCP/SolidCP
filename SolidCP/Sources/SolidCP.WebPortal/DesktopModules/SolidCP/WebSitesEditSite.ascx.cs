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
using System.Linq;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.Common;
using SolidCP.Portal.Code.Helpers;
using SolidCP.Providers.ResultObjects;
using System.Collections.Specialized;

namespace SolidCP.Portal
{
	public partial class WebSitesEditSite : SolidCPModuleBase
	{
		/// <summary>
		/// Use this variable to define additional tabs to in the control
		/// </summary>
		private List<Tab> TabsList = new List<Tab>()
		{
			new Tab { Id = "home", ResourceKey = "Tab.HomeFolder", ViewId = "tabHomeFolder" },
			new Tab { Id = "vdirs", ResourceKey = "Tab.VirtualDirs", Quota = Quotas.WEB_VIRTUALDIRS, ViewId = "tabVirtualDirs" },
			new Tab { Id = "securedfolders", ResourceKey = "Tab.SecuredFolders", Quota = Quotas.WEB_SECUREDFOLDERS, ViewId = "tabSecuredFolders" },
			new Tab { Id = "htaccessfolders", ResourceKey = "Tab.Htaccess", Quota = Quotas.WEB_HTACCESS, ViewId = "tabHeliconApe" },
			new Tab { Id = "frontpage", ResourceKey = "Tab.FrontPage", Quota = Quotas.WEB_FRONTPAGE, ViewId = "tabFrontPage" },
			new Tab { Id = "extensions", ResourceKey = "Tab.Extensions", ViewId = "tabExtensions" },
			new Tab { Id = "HeliconZoo", ResourceKey = "Tab.HeliconZoo", Quota = Quotas.HELICON_ZOO, ResourceGroup = "HeliconZoo", ViewId = "tabHeliconZoo" },
			new Tab { Id = "errors", ResourceKey = "Tab.CustomErrors", Quota = Quotas.WEB_ERRORS, ViewId = "tabErrors" },
			new Tab { Id = "headers", ResourceKey = "Tab.CustomHeaders", Quota = Quotas.WEB_HEADERS, ViewId = "tabHeaders" },
			new Tab { Id = "webpub", ResourceKey = "Tab.WebDeployPublishing", Quota = Quotas.WEB_REMOTEMANAGEMENT, ViewId = "tabWebDeployPublishing" },
			new Tab { Id = "mime", ResourceKey = "Tab.MIMETypes", Quota = Quotas.WEB_MIME, ViewId = "tabMimes" },
			new Tab { Id = "coldfusion", ResourceKey = "Tab.ColdFusion", Quota = Quotas.WEB_COLDFUSION, ViewId = "tabCF" },
			new Tab { Id = "webman", ResourceKey = "Tab.WebManagement", Quota = Quotas.WEB_REMOTEMANAGEMENT, ViewId = "tabWebManagement" },
			new Tab { Id = "SSL", ResourceKey = "Tab.SSL", Quota = Quotas.WEB_SSL, ViewId = "SSL" },
		};
		protected string SharedIdAddres { get; set; }
		private int PackageId
		{
			get { return (int)ViewState["PackageId"]; }
			set { ViewState["PackageId"] = value; }
		}

		private bool AllowSsl
		{
			get { return (bool)ViewState["AllowSsl"]; }
			set { ViewState["AllowSsl"] = value; }
		}

		private bool IIs7
		{
			get { return (bool)ViewState["IIs7"]; }
			set { ViewState["IIs7"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// Retrieve localized tab names as per file resources
			// and assign Web resource group by default if otherwise specified
			TabsList.ForEach((x) =>
			{
				x.Name = GetLocalizedString(x.ResourceKey);
				x.ResourceGroup = x.ResourceGroup ?? ResourceGroups.Web;
			});

			//
			if (!IsPostBack)
			{
				BindWebSite();

				if (GetLocalizedString("buttonPanel.OnSaveClientClick")!=null)
					buttonPanel.OnSaveClientClick = GetLocalizedString("buttonPanel.OnSaveClientClick");
			}
		}

		private void BindTabs()
		{
			var filteredTabs = TabsList.FilterTabsByHostingPlanQuotas(PackageId).ToList();

			// remove "SSL" tab for a site with dynamic IP and not SNI enabled
			var sslTab = filteredTabs.SingleOrDefault(t => t.Id == "SSL");
			if (!AllowSsl && sslTab != null)
				filteredTabs.Remove(sslTab);


			var selectedValue = dlTabs.SelectedValue;

			if (dlTabs.SelectedIndex == -1)
			{
				if (!IsPostBack && String.IsNullOrEmpty(Request["MenuID"]) == false)
				{
					// Find menu item requested
					var st = filteredTabs.SingleOrDefault(x => x.Id.Equals(Request["MenuID"]));
					//
					if (st != null)
					{
						selectedValue = st.ViewId;
						//
						dlTabs.SelectedIndex = Array.IndexOf(filteredTabs.ToArray(), st);
					}
				}
				else
				{
					// Select "Home Folder" tab by default
					dlTabs.SelectedIndex = 0;
				}
			}

			// Bind data to the view
			dlTabs.DataSource = filteredTabs;
			dlTabs.DataBind();
			// Select active view corresponding to the tab selected
			tabs.SetActiveViewById(selectedValue as string);
		}

		protected void dlTabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindTabs();
		}

		private void BindWebSite()
		{
			WebSite site = null;
			try
			{
				site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_GET_SITE", ex);
				return;
			}

			if (site == null)
				RedirectToBrowsePage();

			// IIS 7.0 mode
			IIs7 = site.IIs7;



			PackageId = site.PackageId;

			// bind site
			lnkSiteName.Text = site.Name;
			lnkSiteName.NavigateUrl = "http://" + site.Name;

			// bind unassigned IP addresses
			ddlIpAddresses.Items.Clear();
			PackageIPAddress[] ips = ES.Services.Servers.GetPackageUnassignedIPAddresses(site.PackageId, 0, IPAddressPool.WebSites);
			foreach (PackageIPAddress ip in ips)
			{
				string fullIP = ip.ExternalIP;
				if (ip.InternalIP != null &&
					ip.InternalIP != "" &&
					ip.InternalIP != ip.ExternalIP)
					fullIP += " (" + ip.InternalIP + ")";

				ddlIpAddresses.Items.Add(new ListItem(fullIP, ip.PackageAddressID.ToString()));
			}

			if (site.IsDedicatedIP)
			{
				litIPAddress.Text = site.SiteIPAddress;
			}
			else
			{
                    //The line below was not showing the correct Public Shared IP Address for websites.
                    //lblSharedIP.Text = string.Format("({0})", ipsGeneral[0].ExternalIP);
                    StringDictionary settings = ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettingsRDS(site.ServiceId));
                    if (settings["PublicSharedIP"] != null)
                        lblSharedIP.Text = settings["PublicSharedIP"].ToString();
            }

			dedicatedIP.Visible = site.IsDedicatedIP;
			sharedIP.Visible = !site.IsDedicatedIP;

			PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
			if (Utils.CheckQouta(Quotas.WEB_ALLOWIPADDRESSMODESWITCH, cntx))
            { 
				cmdSwitchToDedicatedIP.Visible = (ddlIpAddresses.Items.Count > 0);
            }
            else
            { 
				cmdSwitchToDedicatedIP.Visible = cmdSwitchToSharedIP.Visible = false;
            }

            litFrontPageUnavailable.Visible = false;
			tblSharePoint.Visible = site.SharePointInstalled;
			tblFrontPage.Visible = !site.SharePointInstalled;


			if (!site.ColdFusionAvailable)
			{
				litCFUnavailable.Text = GetLocalizedString("Text.COLDFUSION_UNAVAILABLE");
				litCFUnavailable.Visible = true;
				rowCF.Visible = false;
				rowVirtDir.Visible = false;

			}
			else
			{
				if (site.ColdFusionVersion.Equals("7"))
				{
					litCFUnavailable.Text = "ColdFusion 7.x is installed";
					litCFUnavailable.Visible = true;
				}
				else
				{
					if (site.ColdFusionVersion.Equals("8"))
						litCFUnavailable.Text = "ColdFusion 8.x is installed";
					litCFUnavailable.Visible = true;
				}

				if (site.ColdFusionVersion.Equals("9"))
					litCFUnavailable.Text = "ColdFusion 9.x is installed";
				litCFUnavailable.Visible = true;

				if (site.ColdFusionVersion.Equals("10"))
					litCFUnavailable.Text = "ColdFusion 10.x is installed";
				litCFUnavailable.Visible = true;

				if (site.ColdFusionVersion.Equals("11"))
					litCFUnavailable.Text = "ColdFusion 11.x is installed";
				litCFUnavailable.Visible = true;

				if (site.ColdFusionVersion.Equals("2016"))
					litCFUnavailable.Text = "ColdFusion 2016.x is installed";
				litCFUnavailable.Visible = true;

			}

			if (!PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_CFVIRTUALDIRS))
			{
				//virtual directories are not implemented for IIS 7
				rowVirtDir.Visible = false;
			}

			chkCfExt.Checked = site.ColdFusionInstalled;
			chkVirtDir.Checked = site.CreateCFAppVirtualDirectories;

            

			// bind FrontPage
			if (!site.FrontPageAvailable)
			{
				litFrontPageUnavailable.Text = GetLocalizedString("Text.FPSE_UNAVAILABLE");
				litFrontPageUnavailable.Visible = true;
				tblFrontPage.Visible = false;
                
            }
			else
			{
				// set control policies
				frontPageUsername.SetPackagePolicy(site.PackageId, UserSettings.WEB_POLICY, "FrontPageAccountPolicy");
				frontPagePassword.SetPackagePolicy(site.PackageId, UserSettings.WEB_POLICY, "FrontPagePasswordPolicy");

                // set default account name
                VirtualDirectoriesCol.Visible = false;
                frontPageUsername.Text = site.FrontPageAccount;
				ToggleFrontPageControls(site.FrontPageInstalled);
			}
            

            AppPoolRestartPanel.Visible = Utils.CheckQouta(Quotas.WEB_APPPOOLSRESTART, cntx);

			// bind controls
			webSitesHomeFolderControl.BindWebItem(PackageId, site);
			webSitesSecuredFoldersControl.BindSecuredFolders(site);
			webSitesHeliconApeControl.BindHeliconApe(site);
			webSitesExtensionsControl.BindWebItem(PackageId, site);
			webSitesMimeTypesControl.BindWebItem(site);
			webSitesCustomHeadersControl.BindWebItem(site);
			webSitesCustomErrorsControl.BindWebItem(site);
			webSitesHeliconZooControl.BindWebItem(site);

			// If SNI is enabled on the server, we do allow for SSL even if site not has dedicated Ip
			if (site.IsDedicatedIP || site.SniEnabled)
			{
				AllowSsl = true;
				WebsitesSSLControl.Visible = true;
				WebsitesSSLControl.BindWebItem(site);
			}
			else
			{
				AllowSsl = false;
				WebsitesSSLControl.Visible = false;
			}

			BindVirtualDirectories();
            BindAppVirtualDirectories();

            // bind state
            BindSiteState(site.SiteState);
			// AppPool
			AppPoolState appPoolState = ES.Services.WebServers.GetAppPoolState(PanelRequest.ItemID);
			BindAppPoolState(appPoolState);

			// bind pointers
			BindPointers();

			// save packageid
			ViewState["PackageID"] = site.PackageId;

			//
			ToggleWmSvcControls(site);

			//
			if (!site.GetValue<bool>(WebAppVirtualDirectory.WmSvcSiteEnabled))
			{
				txtWmSvcAccountName.Text = AutoSuggestWmSvcAccontName(site, "_admin");
			}

			ToggleWmSvcConnectionHint(site);

			// Web Deploy Publishing
			ToggleWebDeployPublishingControls(site);
			BindWebPublishingProfileDatabases();
			BindWebPublishingProfileDatabaseUsers();
			BindWebPublishingProfileFtpAccounts(site);

			// bind tabs
			BindTabs();
		}

		#region Web Deploy Publishing

		protected void WDeployEnabePublishingButton_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;
			//
			GrantWebDeployPublishingAccess(WDeployPublishingAccountTextBox.Text.Trim(), WDeployPublishingPasswordTextBox.Text);
			//
			BindWebSite();
		}

		protected void MyDatabaseList_SelectedIndexChanged(object sender, EventArgs e)
		{
			//
			BindWebPublishingProfileDatabaseUsers();
		}

		private void GrantWebDeployPublishingAccess(string accountName, string accountPassword)
		{
			//
			ResultObject result = ES.Services.WebServers.GrantWebDeployPublishingAccess(PanelRequest.ItemID, accountName, accountPassword);
			//
			if (!result.IsSuccess)
			{
				messageBox.ShowMessage(result, "WEB_PUB_ENABLE", "IIS7");
				return;
			}
			//
			messageBox.ShowSuccessMessage("WEB_PUB_ENABLE");
		}

		protected void WDeployChangePublishingPasswButton_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid || WDeployPublishingPasswordTextBox.Text.Equals(PasswordControl.EMPTY_PASSWORD))
				return;
			//
			ChangeWDeployAccountPassword(PanelRequest.ItemID, WDeployPublishingPasswordTextBox.Text);
		}

		private void ChangeWDeployAccountPassword(int siteItemId, string newAccountPassword)
		{
			try
			{
				//
				var result = ES.Services.WebServers.ChangeWebDeployPublishingPassword(siteItemId, newAccountPassword);
				//
				if (result.IsSuccess == false)
				{
					messageBox.ShowErrorMessage("WPUB_PASSW_CHANGE");
					return;
				}
				//
				messageBox.ShowSuccessMessage("WPUB_PASSW_CHANGE");
			}
			catch (Exception ex)
			{
				messageBox.ShowErrorMessage("WPUB_PASSW_CHANGE", ex);
			}
		}

		protected void WDeployDownloadPubProfileLink_Command(object sender, CommandEventArgs e)
		{
			DownloadWDeployPublishingProfile((string)e.CommandArgument);
		}

		private void DownloadWDeployPublishingProfile(string siteName)
		{
			// download file
			Response.Clear();
			Response.AddHeader("Content-Disposition", "attachment; filename=" + String.Format("{0}.publishsettings", siteName));
			Response.ContentType = "application/octet-stream";

			var result = default(BytesResult);

			try
			{
				// read remote content
				result = ES.Services.WebServers.GetWebDeployPublishingProfile(PanelRequest.ItemID);
				//
				if (result.IsSuccess == false)
				{
					messageBox.ShowErrorMessage("WDEPLOY_GET_PROFILE");
					//
					return;
				}
			}
			catch (Exception ex)
			{
				messageBox.ShowErrorMessage("FILES_READ_FILE", ex);
				return;
			}

			// write to stream
			Response.BinaryWrite(result.Value);
			//
			Response.End();
		}

		protected void WDeployDisablePublishingButton_Click(object sender, EventArgs e)
		{
			DisableWebDeployPublishing();
			//
			BindWebSite();
		}

		private void DisableWebDeployPublishing()
		{
			try
			{
				ES.Services.WebServers.RevokeWebDeployPublishingAccess(PanelRequest.ItemID);
				//
				messageBox.ShowSuccessMessage("WEB_PUB_DISABLE");
			}
			catch (Exception ex)
			{
				messageBox.ShowErrorMessage("WEB_PUB_DISABLE", ex);
			}
		}

		protected void PubProfileWizardOkButton_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;
			//
			SaveWebDeployPublishingProfile();
			//
			BindWebSite();
		}

		private void SaveWebDeployPublishingProfile()
		{
			var ids = new List<int>();
			// Add FTP account to profile
			if (String.IsNullOrEmpty(MyFtpAccountList.SelectedValue) != true)
			{
				ids.Add(Convert.ToInt32(MyFtpAccountList.SelectedValue));
			}
			// Add database to profile
			if (String.IsNullOrEmpty(MyDatabaseList.SelectedValue) != true)
			{
				ids.Add(Convert.ToInt32(MyDatabaseList.SelectedValue));
			}
			// Add database user to profile
			if (String.IsNullOrEmpty(MyDatabaseUserList.SelectedValue) != true)
			{
				ids.Add(Convert.ToInt32(MyDatabaseUserList.SelectedValue));
			}
			//
			var result = ES.Services.WebServers.SaveWebDeployPublishingProfile(PanelRequest.ItemID, ids.ToArray());
			//
			if (!result.IsSuccess)
			{
				messageBox.ShowMessage(result, "WPUB_PROFILE_SAVE", "IIS7");
				return;
			}
			//
			messageBox.ShowSuccessMessage("WPUB_PROFILE_SAVE");
		}

		private void DisableChildControlsOfType(Control ctl, params Type[] ctlTypes)
		{
			foreach (Control cc in ctl.Controls)
			{
				if (Array.Exists(ctlTypes, x =>
				{
					return cc.GetType().Equals(x);
				}))
				{
					cc.Visible = false;
				}
				// Disable child controls recursively if any
				if (cc.Controls.Count > 0)
				{
					DisableChildControlsOfType(cc, ctlTypes);
				}
			}
		}

		private void EnableControlsInBulk(params Control[] ctls)
		{
			foreach (var item in ctls)
			{
				item.Visible = true;
			}
		}

		private void ToggleWebDeployPublishingControls(WebAppVirtualDirectory item)
		{
			// Disable all child controls
			DisableChildControlsOfType(tabWebDeployPublishing, typeof(PlaceHolder), typeof(Button), typeof(TextBox), typeof(Literal));

			// Cleanup password text boxes
			WDeployPublishingPasswordTextBox.Text = WDeployPublishingPasswordTextBox.Attributes["value"] = String.Empty;
			WDeployPublishingConfirmPasswordTextBox.Text = WDeployPublishingConfirmPasswordTextBox.Attributes["value"] = String.Empty;

			// Step 1: Web Deploy feature is not installed on the server
			if (item.WebDeployPublishingAvailable == false)
			{
				// Enable panels
				EnableControlsInBulk(PanelWDeployNotInstalled);
				//
				return;
			}

			// Step 2: Web Deploy feature is available but not publishing enabled for the web site yet
			if (item.WebDeploySitePublishingEnabled == false)
			{
				// Enable controls
				EnableControlsInBulk(
					PanelWDeploySitePublishingDisabled,
					PanelWDeployPublishingCredentials,
					WDeployEnabePublishingButton,
					WDeployPublishingAccountTextBox,
					WDeployPublishingPasswordTextBox,
					WDeployPublishingConfirmPasswordTextBox,
					WDeployPublishingAccountRequiredFieldValidator);

				WDeployPublishingAccountTextBox.Text = AutoSuggestWmSvcAccontName(item, "_dploy");
				//
				WDeployPublishingAccountRequiredFieldValidator.Enabled = true;
				//
				return;
			}
			// Step 3: Publishing has been enabled for the web site
			if (item.WebDeploySitePublishingEnabled == true)
			{
				// Enable controls
				EnableControlsInBulk(
					PanelWDeployPublishingCredentials,
					WDeployChangePublishingPasswButton,
					WDeployDisablePublishingButton,
					WDeployPublishingAccountLiteral,
					WDeployPublishingPasswordTextBox,
					WDeployPublishingConfirmPasswordTextBox);
				// Disable user name validation
				WDeployPublishingAccountRequiredFieldValidator.Enabled = false;
				// Display plain-text publishing account name
				WDeployPublishingAccountLiteral.Text = item.WebDeployPublishingAccount;
				// Miscellaneous
				// Enable empty publishing password for stylistic purposes
				WDeployPublishingPasswordTextBox.Text = PasswordControl.EMPTY_PASSWORD;
				WDeployPublishingPasswordTextBox.Attributes["value"] = PasswordControl.EMPTY_PASSWORD;
				// Enable empty publishing password confirmation for stylistic purposes
				WDeployPublishingConfirmPasswordTextBox.Text = PasswordControl.EMPTY_PASSWORD;
				WDeployPublishingConfirmPasswordTextBox.Attributes["value"] = PasswordControl.EMPTY_PASSWORD;
			}
			// Step 4: Publishing has been enabled and publishing profile has been built
			if (item.WebDeploySitePublishingEnabled == true)
			{
				// Enable controls
				EnableControlsInBulk(PanelWDeployManagePublishingProfile);
				// Save web site name as a command argument for the link
				WDeployDownloadPubProfileLink.CommandArgument = item.Name;
			}
		}

		private void BindWebPublishingProfileDatabases()
		{
			MyDatabaseList.DataSource = ES.Services.DatabaseServers.GetSqlDatabases(PanelSecurity.PackageId, null, false);
			MyDatabaseList.DataBind();
			//
			MyDatabaseList.Items.Insert(0, new ListItem(GetLocalizedString("WebPublishing.ChooseDatabasePrompt"), String.Empty));
		}

		private void BindWebPublishingProfileDatabaseUsers()
		{
			//
			if (String.IsNullOrEmpty(MyDatabaseList.SelectedValue) == false)
			{
				var dbItem = ES.Services.DatabaseServers.GetSqlDatabase(Convert.ToInt32(MyDatabaseList.SelectedValue));
				//
				var sqlUsers = ES.Services.DatabaseServers.GetSqlUsers(PanelSecurity.PackageId, dbItem.GroupName, false);
				//
				MyDatabaseUserList.DataSource = Array.FindAll(sqlUsers, x => Array.Exists(dbItem.Users, y => y.Equals(x.Name)));
				MyDatabaseUserList.DataBind();
			}
			else
			{
				MyDatabaseUserList.Items.Clear();
			}
			//
			MyDatabaseUserList.Items.Insert(0, new ListItem(GetLocalizedString("WebPublishing.ChooseDatabaseUserPrompt"), String.Empty));
		}

		private void BindWebPublishingProfileFtpAccounts(WebAppVirtualDirectory item)
		{
			var ftpAccounts = ES.Services.FtpServers.GetFtpAccounts(PanelSecurity.PackageId, false);
			//
			MyFtpAccountList.DataSource = Array.FindAll(ftpAccounts, x => x.Folder.Equals(item.ContentPath));
			MyFtpAccountList.DataBind();
			//
			MyFtpAccountList.Items.Insert(0, new ListItem(GetLocalizedString("WebPublishing.ChooseFtpAccountPrompt"), String.Empty));
		}

		#endregion

		#region WmSvc Management

		private string  AutoSuggestWmSvcAccontName(WebAppVirtualDirectory item, string suffix)
		{
			string autoSuggestedPart = item.Name;
			//
			if (autoSuggestedPart.Length > 14)
			{
				autoSuggestedPart = autoSuggestedPart.Substring(0, 14);
				//
				while (!String.IsNullOrEmpty(autoSuggestedPart) &&
					!Char.IsLetterOrDigit(autoSuggestedPart[autoSuggestedPart.Length - 1]))
				{
					autoSuggestedPart = autoSuggestedPart.Substring(0, autoSuggestedPart.Length - 1);
				}
			}
			//
			return autoSuggestedPart + suffix;
		}

		private void ToggleWmSvcControls(WebAppVirtualDirectory item)
		{
			if (!item.GetValue<bool>(WebAppVirtualDirectory.WmSvcAvailable))
			{
				pnlWmcSvcManagement.Visible = false;
				pnlNotInstalled.Visible = true;
				//
				return;
			}
			//
			pnlWmcSvcManagement.Visible = true;
			pnlNotInstalled.Visible = false;

			//
			string wmSvcAccountName = item.GetValue<string>(WebAppVirtualDirectory.WmSvcAccountName);
			bool wmcSvcSiteEnabled = item.GetValue<bool>(WebAppVirtualDirectory.WmSvcSiteEnabled);

			btnWmSvcSiteEnable.Visible = true;
			txtWmSvcAccountName.Visible = true;

			//
			txtWmSvcAccountPassword.Text = txtWmSvcAccountPassword.Attributes["value"] = String.Empty;
			//
			txtWmSvcAccountPasswordC.Text = txtWmSvcAccountPasswordC.Attributes["value"] = String.Empty;

			// Disable edit mode if WmSvc account name is set
			if (wmcSvcSiteEnabled)
			{
				btnWmSvcSiteEnable.Visible = false;
				txtWmSvcAccountName.Visible = false;

				//
				txtWmSvcAccountPassword.Text = PasswordControl.EMPTY_PASSWORD;
				txtWmSvcAccountPassword.Attributes["value"] = PasswordControl.EMPTY_PASSWORD;

				//
				txtWmSvcAccountPasswordC.Text = PasswordControl.EMPTY_PASSWORD;
				txtWmSvcAccountPasswordC.Attributes["value"] = PasswordControl.EMPTY_PASSWORD;
			}

			//
			litWmSvcAccountName.Visible = wmcSvcSiteEnabled;
			btnWmSvcSiteDisable.Visible = wmcSvcSiteEnabled;
			btnWmSvcChangePassw.Visible = wmcSvcSiteEnabled;
			pnlWmSvcSiteDisabled.Visible = !wmcSvcSiteEnabled;
			pnlWmSvcSiteEnabled.Visible = wmcSvcSiteEnabled;

			//
			txtWmSvcAccountName.Text = wmSvcAccountName;
			litWmSvcAccountName.Text = wmSvcAccountName;
		}

		private void ToggleWmSvcConnectionHint(WebAppVirtualDirectory item)
		{
			bool wmcSvcSiteEnabled = item.GetValue<bool>(WebSite.WmSvcSiteEnabled);
			//
			if (wmcSvcSiteEnabled)
			{
				//
				string wmSvcServicePort = item.GetValue<String>(WebSite.WmSvcServicePort);
				string wmSvcServiceUrl = item.GetValue<String>(WebSite.WmSvcServiceUrl);
				//
				if (!String.IsNullOrEmpty(wmSvcServiceUrl))
				{
					if (!String.IsNullOrEmpty(wmSvcServicePort)
						&& !String.Equals(wmSvcServicePort, WebSite.WmSvcDefaultPort))
						lclWmSvcConnectionHint.Text = String.Format(
							lclWmSvcConnectionHint.Text, String.Format("{0}:{1}", wmSvcServiceUrl, wmSvcServicePort), item.Name);
					else
						lclWmSvcConnectionHint.Text = String.Format(
							lclWmSvcConnectionHint.Text, wmSvcServiceUrl, item.Name);
				}
				else
					lclWmSvcConnectionHint.Visible = false;
			}
		}

		protected void btnWmSvcSiteEnable_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			//
			string accountName = txtWmSvcAccountName.Text.Trim();
			string accountPassword = txtWmSvcAccountPassword.Text;

			//
			ResultObject result = ES.Services.WebServers.GrantWebManagementAccess(PanelRequest.ItemID, accountName, accountPassword);
			//
			if (!result.IsSuccess)
			{
				messageBox.ShowMessage(result, "IIS7_WMSVC", "IIS7");
				return;
			}
			//
			messageBox.ShowSuccessMessage("Iis7WmSvc_Enabled");
			//
			BindWebSite();
		}

		protected void btnWmSvcChangePassw_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			//
			string accountPassword = txtWmSvcAccountPassword.Text;

			//
			ResultObject result = ES.Services.WebServers.ChangeWebManagementAccessPassword(
				PanelRequest.ItemID, accountPassword);
			//
			if (!result.IsSuccess)
			{
				messageBox.ShowMessage(result, "IIS7_WMSVC", "IIS7");
				return;
			}
			//
			messageBox.ShowSuccessMessage("Iis7WmSvc_PasswordChanged");
			//
			BindWebSite();
		}

		protected void btnWmSvcSiteDisable_Click(object sender, EventArgs e)
		{
			//
			string accountName = txtWmSvcAccountName.Text.Trim();

			//
			ES.Services.WebServers.RevokeWebManagementAccess(PanelRequest.ItemID);
			//
			messageBox.ShowSuccessMessage("Iis7WmSvc_Disabled");
			//
			BindWebSite();
		}

		#endregion

		#region FrontPage
		private void ToggleFrontPageControls(bool installed)
		{
			// status
			litFrontPageStatus.Text = installed ? GetLocalizedString("Text.FPSE_INSTALLED") : GetLocalizedString("Text.FPSE_NOT_INSTALLED");

			if (!installed)
				frontPageUsername.Text = "";

			frontPageUsername.EditMode = installed;

			// toggle buttons
			btnInstallFrontPage.Visible = !installed;
			btnUninstallFrontPage.Visible = installed;
			btnChangeFrontPagePassword.Visible = installed;
			pnlFrontPage.DefaultButton = installed ? "btnChangeFrontPagePassword" : "btnInstallFrontPage";
		}

		protected void btnInstallFrontPage_Click(object sender, EventArgs e)
		{
			try
			{
				int result = ES.Services.WebServers.InstallFrontPage(PanelRequest.ItemID,
					frontPageUsername.Text, frontPagePassword.Password);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				ShowSuccessMessage("WEB_FP_INSTALL");
				frontPagePassword.Password = "";
				frontPageUsername.Text = frontPageUsername.Text;
				ToggleFrontPageControls(true);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_FP_INSTALL", ex);
				return;
			}
		}
		protected void btnChangeFrontPagePassword_Click(object sender, EventArgs e)
		{
			try
			{
				int result = ES.Services.WebServers.ChangeFrontPagePassword(PanelRequest.ItemID,
					frontPagePassword.Password);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				ShowSuccessMessage("WEB_FP_CHANGE_PASSWORD");
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_FP_CHANGE_PASSWORD", ex);
				return;
			}
		}
		protected void btnUninstallFrontPage_Click(object sender, EventArgs e)
		{
			try
			{
				int result = ES.Services.WebServers.UninstallFrontPage(PanelRequest.ItemID);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				ShowSuccessMessage("WEB_FP_UNINSTALL");
				ToggleFrontPageControls(false);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_FP_UNINSTALL", ex);
				return;
			}
		}
        #endregion

        #region Virtual Directories
        private void BindVirtualDirectories()
        {
            gvVirtualDirectories.DataSource = ES.Services.WebServers.GetVirtualDirectories(PanelRequest.ItemID);
            gvVirtualDirectories.DataBind();
        }

        private void BindAppVirtualDirectories()
        {
            gvAppVirtualDirectories.DataSource = ES.Services.WebServers.GetAppVirtualDirectories(PanelRequest.ItemID);
            gvAppVirtualDirectories.DataBind();
        }


        protected void btnAddVirtualDirectory_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_vdir",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnAddAppVirtualDirectory_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_appdir",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
        #endregion

        private void BindPointers()
		{
			gvPointers.DataSource = ES.Services.WebServers.GetWebSitePointers(PanelRequest.ItemID);
			gvPointers.DataBind();
		}

		private void SaveWebSite()
		{
			if (!Page.IsValid)
				return;

			// load original web site item
			WebSite site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);

			// collect form data
			site.FrontPageAccount = frontPageUsername.Text;

			site.ColdFusionInstalled = chkCfExt.Checked;
			site.CreateCFAppVirtualDirectories = chkVirtDir.Checked;

			// other controls
			webSitesExtensionsControl.SaveWebItem(site);
			webSitesHomeFolderControl.SaveWebItem(site);
			webSitesMimeTypesControl.SaveWebItem(site);
			webSitesCustomHeadersControl.SaveWebItem(site);
			webSitesCustomErrorsControl.SaveWebItem(site);
			webSitesHeliconZooControl.SaveWebItem(site);

			// update web site
			try
			{
				int result = ES.Services.WebServers.UpdateWebSite(site);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				ShowSuccessMessage("WEB_UPDATE_SITE");
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_UPDATE_SITE", ex);
				return;
			}
		}

		private void DeleteWebSite()
		{
			try
			{
				int result = ES.Services.WebServers.DeleteWebSite(PanelRequest.ItemID, chkDeleteWebsiteDirectory.Checked);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_DELETE_SITE", ex);
				return;
			}

			RedirectSpaceHomePage();
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SaveWebSite();
		}

		protected void btnSaveExit_Click(object sender, EventArgs e)
		{
			SaveWebSite();
			RedirectSpaceHomePage();
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			DeleteWebSite();
		}


		#region Site State
		private void BindSiteState(ServerState state)
		{
			if (state == ServerState.Continuing)
				state = ServerState.Started;

			litStatus.Text = GetLocalizedString("SiteState." + state.ToString());
			cmdStart.Visible = (state == ServerState.Stopped);
			cmdContinue.Visible = (state == ServerState.Paused);
			cmdPause.Visible = (state == ServerState.Started);
			cmdStop.Visible = (state == ServerState.Started || state == ServerState.Paused);
		}

		protected void cmdChangeState_Click(object sender, EventArgs e)
		{
			string stateName = ((CPCC.StyleButton)sender).CommandName;
			ServerState state = (ServerState)Enum.Parse(typeof(ServerState), stateName, true);

			try
			{
				int result = ES.Services.WebServers.ChangeSiteState(PanelRequest.ItemID, state);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				BindSiteState(state);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_CHANGE_SITE_STATE", ex);
				return;
			}
		}

		// AppPool
		private void BindAppPoolState(AppPoolState state)
		{
			litAppPoolStatus.Text = GetLocalizedString("SiteState." + state.ToString());

			cmdAppPoolStart.Visible = (state == AppPoolState.Stopped || state == AppPoolState.Stopping);
			cmdAppPoolStop.Visible = (state == AppPoolState.Started || state == AppPoolState.Starting);
			cmdAppPoolRecycle.Visible = (state == AppPoolState.Started || state == AppPoolState.Starting);
		}


		protected void cmdAppPoolChangeState_Click(object sender, EventArgs e)
		{
			string stateName = ((CPCC.StyleButton)sender).CommandName;
			AppPoolState state = (AppPoolState)Enum.Parse(typeof(AppPoolState), stateName, true);

			try
			{
				int result = ES.Services.WebServers.ChangeAppPoolState(PanelRequest.ItemID, state);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				state = ES.Services.WebServers.GetAppPoolState(PanelRequest.ItemID);
				BindAppPoolState(state);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_CHANGE_SITE_STATE", ex);
				return;
			}
		}

		#endregion

		#region Pointers
		protected void gvPointers_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			int domainId = (int)gvPointers.DataKeys[e.RowIndex][0];

			try
			{
				int result = ES.Services.WebServers.DeleteWebSitePointer(PanelRequest.ItemID, domainId);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				ShowSuccessMessage("WEB_DELETE_SITE_POINTER");
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_DELETE_SITE_POINTER", ex);
				return;
			}

			// rebind pointers
			BindPointers();
		}

		protected void btnAddPointer_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_pointer",
				PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
		}
		#endregion

		protected void cmdSwitchToDedicatedIP_Click(object sender, EventArgs e)
		{

			sharedIP.Visible = false;
			switchToDedicatedIP.Visible = true;
			WebsitesSSLControl.InstalledCert = null;
		}

		protected void cmdSwitchToSharedIP_Click(object sender, EventArgs e)
		{
			// call web service
			try
			{
				int result = ES.Services.WebServers.SwitchWebSiteToSharedIP(PanelRequest.ItemID);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				ShowSuccessMessage("WEB_SWITCH_TO_SHARED_IP");

				dlTabs.SelectedIndex = 0;

				WebsitesSSLControl.InstalledCert = null;
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_SWITCH_TO_SHARED_IP", ex);
				return;
			}

			// rebind
			BindWebSite();
		}

		protected void cmdApplyDedicatedIP_Click(object sender, EventArgs e)
		{
			// call web service
			try
			{
				int addressId = Int32.Parse(ddlIpAddresses.SelectedValue);
				int result = ES.Services.WebServers.SwitchWebSiteToDedicatedIP(PanelRequest.ItemID, addressId);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				ShowSuccessMessage("WEB_SWITCH_TO_DEDICATED_IP");
			}
			catch (Exception ex)
			{
				ShowErrorMessage("WEB_SWITCH_TO_DEDICATED_IP", ex);
				return;
			}

			// rebind
			HideDedicatedIPPanel();
			BindWebSite();
		}

		protected void cmdCancelDedicatedIP_Click(object sender, EventArgs e)
		{
			HideDedicatedIPPanel();
		}

		private void HideDedicatedIPPanel()
		{
			switchToDedicatedIP.Visible = false;
			sharedIP.Visible = true;
		}

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
