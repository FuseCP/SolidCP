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
using System.Management;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using SolidCP.Setup.Web;

namespace SolidCP.Setup
{
	public partial class WebPage : BannerWizardPage
	{
		public WebPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Web Settings";
			
			string component = SetupVariables.ComponentFullName;
			this.Description = string.Format("Specify {0} web settings.", component);
			
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			

			// init fields
			PopulateIPs();
			this.txtWebSiteDomain.Text = SetupVariables.WebSiteDomain;
			this.txtWebSiteTcpPort.Text = SetupVariables.WebSitePort;

			if (SetupVariables.NewVirtualDirectory)
			{
				this.txtWebSiteDomain.Enabled = false;
				this.cbWebSiteIP.Enabled = false;
				this.txtWebSiteTcpPort.Enabled = false;
			}
			UpdateApplicationAddress();
			Update();
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
				Wizard.GoNext();
		}

		private void PopulateIPs()
		{
			try
			{
				Log.WriteStart("Loading IPs");
				
				cbWebSiteIP.Items.Clear();
				string[] ips = WebUtils.GetIPv4Addresses();
				foreach (string ip in ips)
				{
					cbWebSiteIP.Items.Add(ip);
				}
				Log.WriteEnd("Loaded IPs");

				if (string.IsNullOrEmpty(SetupVariables.WebSiteIP))
				{
					//select first available
					if (cbWebSiteIP.Items.Count > 0)
					{
						cbWebSiteIP.Text = cbWebSiteIP.Items[0].ToString();
					}

				}
				else
				{
					//add 127.0.0.1 
					if (!cbWebSiteIP.Items.Contains(SetupVariables.WebSiteIP))
					{
						cbWebSiteIP.Items.Insert(0, SetupVariables.WebSiteIP);
					}
					cbWebSiteIP.Text = SetupVariables.WebSiteIP;
				}
				Update();

			}
			catch (Exception ex)
			{
				Log.WriteError("WMI error", ex);
			}
		}

		private void OnAddressChanged(object sender, System.EventArgs e)
		{
			UpdateApplicationAddress();
		}

		private void UpdateApplicationAddress()
		{
			/*if (SetupVariables.NewVirtualDirectory)
			{
				this.txtAddress.StatusMessage = string.Empty;
				return;
			}*/
			string address = "http://";
			string server = string.Empty;
			string port = string.Empty;
			string virtualDir = string.Empty;
			//server 
			if (txtWebSiteDomain.Text.Trim().Length > 0)
			{
				//domain 
				server = txtWebSiteDomain.Text.Trim();
			}
			else
			{
				//ip
				if (cbWebSiteIP.Text.Trim().Length > 0)
				{
					server = cbWebSiteIP.Text.Trim();
				}
			}
			//port
			if (server.Length > 0 &&
				txtWebSiteTcpPort.Text.Trim().Length > 0 &&
				txtWebSiteTcpPort.Text.Trim() != "80")
			{
				port = ":" + txtWebSiteTcpPort.Text.Trim();
			}
			
			//virtual dir
			if (server.Length > 0 &&
				(!string.IsNullOrEmpty(SetupVariables.VirtualDirectory)))
			{
				virtualDir = "/" + SetupVariables.VirtualDirectory;
			}
			//address string
			address += server + port + virtualDir;
			txtAddress.Text = address;

		}

		private bool CheckWebExtensions()
		{
			bool ret = true;
			try
			{
				if (SetupVariables.IISVersion.Major < 7)
				{
					DirectoryEntry iis = new DirectoryEntry("IIS://LocalHost/W3SVC");
					WebExtensionStatus status = WebExtensionStatus.NotInstalled;
					foreach (string propertyName in iis.Properties.PropertyNames)
					{
						if (propertyName.Equals("WebSvcExtRestrictionList", StringComparison.InvariantCultureIgnoreCase))
						{
							PropertyValueCollection valueCollection = iis.Properties[propertyName];
							foreach (object objVal in valueCollection)
							{
								if (objVal != null && !string.IsNullOrEmpty(objVal.ToString()))
								{
									string strVal = objVal.ToString().ToLower();
									if (strVal.Contains(@"\v2.0.50727\aspnet_isapi.dll".ToLower()))
									{
										if (strVal[0] == '1')
										{
											status = WebExtensionStatus.Allowed;
										}
										else if (status == WebExtensionStatus.NotInstalled)
										{
											status = WebExtensionStatus.Prohibited;
										}
									}
								}
							}
						}
					}
					if (status == WebExtensionStatus.NotInstalled)
					{
						ShowWarning("ASP.NET 2.0 is not installed in the Web Service Extensions in IIS. Please install ASP.NET 2.0 Web Service Extension and click Next button to continue with the installation.");
						ret = false;
					}
					else if (status == WebExtensionStatus.Prohibited)
					{
						ShowWarning("ASP.NET 2.0 is not allowed in the Web Service Extensions in IIS. Please allow ASP.NET 2.0 Web Service Extension and click Next button to continue with the installation.");
						ret = false;
					}
				}
			}
			catch (Exception ex)
			{
				// you cannot enumerate metabase properties unless you are using Windows XP Professional with Service Pack 2 or Windows Server 2003 with Service Pack 1.
				Log.WriteError("IIS metabase error", ex);
				ret = true;
			}
			return ret;
		}

		private bool IsValidIPv4(string address)
		{
			return Regex.IsMatch(address, @"^(?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(?=\.?\d)\.)){4}$");
		}

		private bool CheckWebSite()
		{
			string ip = cbWebSiteIP.Text;
			string port = txtWebSiteTcpPort.Text;
			string domain = txtWebSiteDomain.Text;

			if (ip.Trim().Length == 0)
			{
				ShowWarning("Please enter web site IP address");
				return false;
			}

			if (!IsValidIPv4(ip))
			{
				ShowWarning("Please enter valid IP address (for example, 192.168.1.42)");
				return false;
			}

			if (domain.Trim().Length > 0)
			{
				if (!Regex.IsMatch(domain, @"^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}$"))
				{
					ShowWarning("Please enter valid domain name (for example, mydomain.com)");
					return false;
				}
			}

			if (port.Trim().Length == 0)
			{
				ShowWarning("Please enter TCP port");
				return false;
			}

			for (int i = 0; i < port.Length; i++)
			{
				if (!Char.IsNumber(port, i))
				{
					ShowWarning("Please enter valid TCP port (for example, 80).");
					return false;
				}
			}
			return true;
		}


		private bool IsEqualString(string s1, string s2)
		{
			bool ret = false;
			if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
			{
				ret = true;
			}
			else
			{
				ret = (s1 == s2);
			}
			return ret;
		}

		private bool CheckServerBindings()
		{

			try
			{
				string newIP = cbWebSiteIP.Text;
				string newPort = txtWebSiteTcpPort.Text;
				string newDomain = txtWebSiteDomain.Text;

				if (SetupVariables.SetupAction == SetupActions.Setup)
				{
					SetupVariables.UpdateWebSite = true;
					//load old settings from config
					string componentId = SetupVariables.ComponentId;
					string ip = SetupVariables.WebSiteIP;
					string port = SetupVariables.WebSitePort;
					string domain = SetupVariables.WebSiteDomain;
					//
					/*string ip = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteIP");
					string port = AppConfig.GetComponentSettingStringValue(componentId, "WebSitePort");
					string domain = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteDomain");*/

					if (newIP == ip && newPort == port && IsEqualString(newDomain,domain))
					{
						//settings were not changed
						SetupVariables.UpdateWebSite = false;
						return true;
					}
				}
                bool iis7 = (SetupVariables.IISVersion.Major >= 7);
				string siteId = iis7 ? 
					WebUtils.GetIIS7SiteIdByBinding(newIP, newPort, newDomain) :
					WebUtils.GetSiteIdByBinding(newIP, newPort, newDomain);
				if (siteId == null)
					return true;

				// get site name
				string siteName = iis7 ? siteId : WebUtils.GetSite(siteId).Name;
				ShowWarning(String.Format("'{0}' web site already has server binding with specified IP, Port and Domain.\nPlease, provide another combination of IP, Port and Domain.",
					siteName));
				return false;

			}
			catch (Exception ex)
			{
				Log.WriteError("Web error", ex);
				ShowError("Unable to load IIS data.");
				return true;
			}
		}

		private bool ProcessWebSettings()
		{
			if (!CheckWebExtensions())
			{
				return false;
			}

			if (!CheckWebSite())
			{
				return false;
			}

			if (!CheckServerBindings())
			{
				return false;
			}
			return true;
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			if (SetupVariables.NewVirtualDirectory)
			{
				//virtual directory is not supported
				if (SetupVariables.SetupAction == SetupActions.Setup)
				{
					SetupVariables.UpdateWebSite = false;
				}
			}
			else
			{

				if (!ProcessWebSettings())
				{
					e.Cancel = true;
					return;
				}
				SetupVariables.WebSiteIP = this.cbWebSiteIP.Text;
				SetupVariables.WebSiteDomain = this.txtWebSiteDomain.Text;
				SetupVariables.WebSitePort = this.txtWebSiteTcpPort.Text;
			}
			base.OnBeforeMoveNext(e);
		}
	}
}
