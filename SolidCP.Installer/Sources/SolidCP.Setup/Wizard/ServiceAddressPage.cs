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
using System.Xml;

using SolidCP.Setup.Web;

namespace SolidCP.Setup
{
	public partial class ServiceAddressPage : BannerWizardPage
	{
		public ServiceAddressPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Service Settings";
			
			string component = Wizard.SetupVariables.ComponentFullName;
			this.Description = string.Format("Specify {0} address settings.", component);
			
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			if (Wizard.SetupVariables.SetupAction == SetupActions.Setup)
			{
				LoadServiceConfigSettings();
			}
			
			// init fields
			PopulateIPs();
			this.txtTcpPort.Text = Wizard.SetupVariables.ServicePort;
			UpdateApplicationAddress();
		}

		private void LoadServiceConfigSettings()
		{
			try
			{
				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, Wizard.SetupVariables.ConfigurationFile);

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Loading configuration file (service settings)");
				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement ipNode = doc.SelectSingleNode("//configuration/appSettings/add[@key='SolidCP.HostIP']") as XmlElement;
				if (ipNode != null)
				{
					Wizard.SetupVariables.ServiceIP = ipNode.GetAttribute("value");
				}
				else
				{
					Log.WriteInfo("Service host IP setting not found");
				}
				
				XmlElement portNode = doc.SelectSingleNode("//configuration/appSettings/add[@key='SolidCP.HostPort']") as XmlElement;
				if (portNode != null)
				{
					Wizard.SetupVariables.ServicePort = portNode.GetAttribute("value");
				}
				else
				{
					Log.WriteInfo("Service host post setting not found");
				}
				
				Log.WriteEnd("Loaded configuration file");
			}
			catch (Exception ex)
			{
				Log.WriteError("Configuration file error", ex);
			}
		}

		private void PopulateIPs()
		{
			try
			{
				Log.WriteStart("Loading IPs");
				cbIP.Items.Clear();
				string[] ips = WebUtils.GetIPs();
				foreach (string ip in ips)
				{
					cbIP.Items.Add(ip);
				}
				Log.WriteEnd("Loaded IPs");

				if (string.IsNullOrEmpty(Wizard.SetupVariables.ServiceIP))
				{
					//select first available
					if (cbIP.Items.Count > 0)
					{
						cbIP.Text = cbIP.Items[0].ToString();
					}
				}
				else
				{
					//add 127.0.0.1 
					if (!cbIP.Items.Contains(Wizard.SetupVariables.ServiceIP))
					{
						cbIP.Items.Insert(0, Wizard.SetupVariables.ServiceIP);
					}
					cbIP.Text = Wizard.SetupVariables.ServiceIP;
				}

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
			string address = "soap.tcp://";
			string ip = string.Empty;
			string port = string.Empty;

				//ip
				if (cbIP.Text.Trim().Length > 0)
				{
					ip = cbIP.Text.Trim();
				}
			
			//port
			if (ip.Length > 0 && txtTcpPort.Text.Trim().Length > 0 )
			{
				port = ":" + txtTcpPort.Text.Trim();
			}
			
			//address string
			address += ip + port;
			txtAddress.Text = address;
		}

		private bool CheckAddress()
		{
			string ip = cbIP.Text;
			string port = txtTcpPort.Text;

			if (ip.Trim().Length == 0)
			{
				ShowWarning("Please enter IP address");
				return false;
			}

			if (!Regex.IsMatch(ip, @"^(?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(?=\.?\d)\.)){4}$"))
			{
				ShowWarning("Please enter valid IP address (for example, 192.168.1.42)");
				return false;
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

		private bool ProcessAddressSettings()
		{
			if (!CheckAddress())
			{
				return false;
			}
			return true;
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			if (!ProcessAddressSettings())
			{
				e.Cancel = true;
				return;
			}
			Wizard.SetupVariables.ServiceIP = this.cbIP.Text;
			Wizard.SetupVariables.ServicePort = this.txtTcpPort.Text;

			base.OnBeforeMoveNext(e);
		}
	}
}
