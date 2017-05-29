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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SolidCP.Setup
{
	public partial class DatabasePage : BannerWizardPage
	{
		public DatabasePage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "Database Settings";
			string component = SetupVariables.ComponentFullName;
			this.Description = string.Format("Enter the connection information for the {0} database.", component);
			this.lblIntro.Text = "The connection information will be used by the Setup Wizard to install the database objects only. Click Next to continue.";
			this.txtDatabase.Text = SetupVariables.Database;
			this.txtSqlServer.Text = SetupVariables.DatabaseServer;
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
			cbAuthentication.SelectedIndex = 0;
			ParseConnectionString();
		}

		private void ParseConnectionString()
		{
			bool windowsAuthentication = false;
			if ( !string.IsNullOrEmpty(SetupVariables.DbInstallConnectionString ))
			{
				string[] pairs = SetupVariables.DbInstallConnectionString.Split(';');
				foreach (string pair in pairs)
				{
					string[] keyValue = pair.Split('=');
					if (keyValue.Length == 2)
					{
						string key = keyValue[0].Trim().ToLower();
						string value = keyValue[1];
						switch (key)
						{
							case "server":
								this.txtSqlServer.Text = value;
								break;
							case "database":
								this.txtDatabase.Text = value;
								break;
							case "integrated security":
								if (value.Trim().ToLower() == "sspi")
									windowsAuthentication = true;
								break;
							case "user":
							case "user id":
								txtLogin.Text = value;
								break;
							case "password":
								txtPassword.Text = value;
								break;
						}
					}
				}
				cbAuthentication.SelectedIndex = windowsAuthentication ? 0 : 1;
			}
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			//unattended setup
			if (!string.IsNullOrEmpty(SetupVariables.SetupXml))
				Wizard.GoNext();
		}

		protected internal override void OnBeforeMoveNext(CancelEventArgs e)
		{
			try
			{
				if (!CheckFields())
				{
					e.Cancel = true;
					return;
				}
				string connectionString = CreateConnectionString();
				string component = SetupVariables.ComponentFullName;

				if (CheckConnection(connectionString))
				{
					// check SQL server version
					string sqlVersion = GetSqlServerVersion(connectionString);
                    if (!sqlVersion.StartsWith("9.") && !sqlVersion.StartsWith("10.") && !sqlVersion.StartsWith("11.") && !sqlVersion.StartsWith("12.") && !sqlVersion.StartsWith("13."))
					{
						// SQL Server 2005 engine required
						e.Cancel = true;
						ShowWarning("This program can be installed on SQL Server 2005/2008/2012/2014/2016 only.");
						return;
					}
					int securityMode = GetSqlServerSecurityMode(connectionString);
					if (securityMode != 0)
					{
						// mixed mode required
						e.Cancel = true;
						ShowWarning("Please switch SQL Server authentication to mixed SQL Server and Windows Authentication mode.");
						return;
					}
				}
				else
				{
					e.Cancel = true;
					ShowWarning("SQL Server does not exist or access denied");
					return;
				}
				string database = this.txtDatabase.Text;
				if (SqlUtils.DatabaseExists(connectionString, database))
				{
					e.Cancel = true;
					ShowWarning(string.Format("{0} database already exists.", database));
					return;
				}

				string server = this.txtSqlServer.Text;
				Log.WriteInfo(string.Format("Sql server \"{0}\" selected for {1}", server, component));
				SetupVariables.Database = database;
				SetupVariables.DatabaseServer = server;
				SetupVariables.DbInstallConnectionString = connectionString;

				//AppConfig.SetComponentSettingStringValue(SetupVariables.ComponentId, "Database", database);
				//AppConfig.SetComponentSettingStringValue(SetupVariables.ComponentId, "DatabaseServer", server);
				//AppConfig.SetComponentSettingStringValue(SetupVariables.ComponentId, "InstallConnectionString", connectionString);
			}
			catch
			{
				e.Cancel = true;
				ShowError("Unable to configure the database server.");
				return;
			}
			base.OnBeforeMoveNext(e);
		}

		private void OnAuthenticationChanged(object sender, System.EventArgs e)
		{
			UpdateFields();
		}

		private void UpdateFields()
		{
			bool winAuthentication = (cbAuthentication.SelectedIndex == 0);
			txtLogin.Enabled = !winAuthentication;
			txtPassword.Enabled = !winAuthentication;
			lblLogin.Enabled = !winAuthentication;
			lblPassword.Enabled = !winAuthentication;
			Update();
		}

		private bool CheckFields()
		{
			if (txtSqlServer.Text.Trim().Length == 0)
			{
				ShowWarning("Please enter valid SQL Server name.");
				return false;
			}
			if (cbAuthentication.SelectedIndex == 1)
			{
				if (txtLogin.Text.Trim().Length == 0)
				{
					ShowWarning("Please enter valid Login name.");
					return false;
				}
			}
			string database = txtDatabase.Text;
			if (database.Trim().Length == 0 || !SqlUtils.IsValidDatabaseName(database))
			{
				ShowWarning("Please enter valid database name.");
				return false;
			}
			
			return true;
		}

		private bool CheckConnection(string connectionString)
		{
			return SqlUtils.CheckSqlConnection(connectionString);
		}

		private string GetSqlServerVersion(string connectionString)
		{
			return SqlUtils.GetSqlServerVersion(connectionString);
		}

		private int GetSqlServerSecurityMode(string connectionString)
		{
			return SqlUtils.GetSqlServerSecurityMode(connectionString);
		}

		private string CreateConnectionString()
		{
			if (cbAuthentication.SelectedIndex == 0)
			{
				return SqlUtils.BuildDbServerMasterConnectionString(txtSqlServer.Text, null, null);
			}
			else
			{
				return SqlUtils.BuildDbServerMasterConnectionString(txtSqlServer.Text, txtLogin.Text, txtPassword.Text);
			}
		}

	}
}
