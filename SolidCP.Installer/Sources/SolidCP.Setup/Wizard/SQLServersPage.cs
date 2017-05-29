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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using SolidCP.Setup.Web;
using SolidCP.Setup.Common;

namespace SolidCP.Setup
{
	public partial class SQLServersPage : BannerWizardPage
	{
		public SQLServersPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "SQL Servers";
			this.Description = "Specify SQL servers to manage with myLittleAdmin.";
			
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;

			PopulateServers();
		}

		private void PopulateServers()
		{
			try
			{
				Log.WriteStart("Populating SQL servers");
				ServerItem[] servers = null;

				if (Wizard.SetupVariables.SetupAction == SetupActions.Setup)
				{
					servers = LoadServersFromConfigFile();
				}
				else
				{
					if (Wizard.SetupVariables.SQLServers != null)
						servers = Wizard.SetupVariables.SQLServers;
				}
				if ( servers == null )
					servers = new ServerItem[] { };

				DataSet ds = new DataSet();
				DataTable dt = new DataTable("Servers");
				ds.Tables.Add(dt);
				DataColumn colServer = new DataColumn("Server", typeof(string));
				DataColumn colName = new DataColumn("Name", typeof(string));
				dt.Columns.AddRange(new DataColumn[]{colServer, colName});

				foreach (ServerItem item in servers)
				{
					dt.Rows.Add(item.Server, item.Name);
				}
				grdServers.DataSource = ds;
				grdServers.DataMember = "Servers";
				Log.WriteEnd("Populated SQL servers");
			}
			catch (Exception ex)
			{
				Log.WriteError("Configuration error", ex);
			}
		}

		private ServerItem[] LoadServersFromConfigFile()
		{
			string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, "config.xml");

			if (!File.Exists(path))
			{
				Log.WriteInfo(string.Format("File {0} not found", path));
				return null;
			}

			List<ServerItem> list = new List<ServerItem>();
			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			XmlNodeList servers = doc.SelectNodes("//myLittleAdmin/sqlservers/sqlserver");
			foreach (XmlElement serverNode in servers)
			{
				list.Add(
					new ServerItem(
						serverNode.GetAttribute("address"),
						serverNode.GetAttribute("name")));
			}
			return list.ToArray();
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			List<ServerItem> list = new List<ServerItem>();
			DataSet ds = grdServers.DataSource as DataSet;
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				list.Add(new ServerItem(row["Server"].ToString(), row["Name"].ToString()));
			}
			Wizard.SetupVariables.SQLServers = list.ToArray();
			base.OnBeforeMoveNext(e);
		}
	}
}
