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
using SolidCP.Providers.Common;
using Data = SolidCP.EnterpriseServer.Data;
using SolidCP.UniversalInstaller.Core;

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
			this.txtSqlServerDatabase.Text = SetupVariables.Database;
			this.txtSqlServerServer.Text = SetupVariables.DatabaseServer;
			this.txtMySqlDatabase.Text = SetupVariables.Database;
			this.txtSqliteDatabase.Text = SetupVariables.Database;
			this.AllowMoveBack = true;
			this.AllowMoveNext = true;
			this.AllowCancel = true;
			cbSqlServerAuthentication.SelectedIndex = 0;
			ParseConnectionString();
		}

		private void ParseConnectionString()
		{
			bool windowsAuthentication = false;
			if (!string.IsNullOrEmpty(SetupVariables.DbInstallConnectionString))
			{
				var csb = new ConnectionStringBuilder();
				csb.ConnectionString = SetupVariables.DbInstallConnectionString;
				var dbType = (csb["dbtype"] as string)?.ToLower();

				switch (dbType)
				{
					case "mssql":
						txtSqlServerServer.Text = (string)(csb["server"] ?? "");
						windowsAuthentication = (csb["integrated security"] as string)?.Trim().ToLower() == "sspi";
						cbSqlServerAuthentication.SelectedIndex = windowsAuthentication ? 0 : 1;
						txtSqlServerLogin.Text = (string)(csb["user"] ?? csb["user id"] ?? csb["uid"] ?? "");
						txtSqlServerPassword.Text = (string)(csb["password"] ?? "");
						txtSqlServerDatabase.Text = (string)(csb["database"] ?? "");
						tabControl.SelectedIndex = 0;
						break;
					case "mysql":
					case "mariadb":
						txtMySqlServer.Text = (string)(csb["server"] ?? csb["host"] ?? csb["data source"] ??
							csb["datasource"] ?? csb["address"] ?? csb["addr"] ?? csb["network address"] ?? "");
						txtMySqlPort.Text = (string)(csb["port"] ?? "");
						txtMySqlUser.Text = (string)(csb["uid"] ?? csb["user"] ?? csb["user id"] ?? csb["userid"] ??
							csb["username"] ?? csb["user name"] ?? "");
						txtMySqlPassword.Text = (string)(csb["pwd"] ?? csb["password"] ?? "");
						txtMySqlDatabase.Text = (string)(csb["database"] ?? csb["inital catalog"] ?? "");
						tabControl.SelectedIndex = 1;
						break;
					case "sqlite":
					case "sqlitefx":
						txtSqliteDatabase.Text = (string)(csb["data source"] ?? "");
						tabControl.SelectedIndex = 2;
						break;
				}
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
				Data.DbType dbtype = Data.DbType.Unknown;
				string server;
				string database;
				string dbuser;
				string dbpassword;
				int? dbport = null;
				switch (tabControl.SelectedIndex)
				{
					case 0:
						dbtype = Data.DbType.SqlServer;
						server = txtSqlServerServer.Text.Trim();
						database = txtSqlServerDatabase.Text.Trim();
						if (cbSqlServerAuthentication.SelectedIndex != 0)
						{
							dbuser = txtSqlServerLogin.Text.Trim();
							dbpassword = txtSqlServerPassword.Text.Trim();
						} else
						{
							dbuser = dbpassword = null;
						}
						dbport = null;
						break;
					case 1:
						dbtype = Data.DbType.MySql;
						server = txtMySqlServer.Text.Trim();
						database = txtMySqlDatabase.Text.Trim();
						dbuser = txtMySqlUser.Text.Trim();
						dbpassword = txtMySqlPassword.Text.Trim();
						int port;
						if (int.TryParse(txtMySqlPort.Text.Trim(), out port)) dbport = port;
						else
						{
							e.Cancel = true;
							ShowWarning("Enter a valid port.");
							return;
						}
						break;
					case 2:
						dbtype = Data.DbType.Sqlite;
						server = "(local)";
						database = txtSqliteDatabase.Text.Trim();
						dbuser = dbpassword = null;
						dbport = null;
						break;
					default: throw new NotSupportedException();
				}
				string connectionString = CreateConnectionString(dbtype);
				string component = SetupVariables.ComponentFullName;

				if (CheckConnection(connectionString))
				{
					if (dbtype == Data.DbType.SqlServer)
					{
						// check SQL server version
						string sqlVersion = GetSqlServerVersion(connectionString);
						if (!sqlVersion.StartsWith("9.") && !sqlVersion.StartsWith("10.") && !sqlVersion.StartsWith("11.") && !sqlVersion.StartsWith("12.") && !sqlVersion.StartsWith("13.") && !sqlVersion.StartsWith("14.") && !sqlVersion.StartsWith("15.") && !sqlVersion.StartsWith("16."))
						{
							// SQL Server 2005 engine required
							e.Cancel = true;
							ShowWarning("This program can be installed on SQL Server 2005/2008/2012/2014/2016/2017/2016/2017/2019/2022 only.");
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
				}
				else
				{
					e.Cancel = true;
					ShowWarning("Database Server does not exist or access denied");
					return;
				}
				
				if (Data.DatabaseUtils.DatabaseExists(connectionString, database))
				{
					e.Cancel = true;
					ShowWarning(string.Format("{0} database already exists.", database));
					return;
				}

				Log.WriteInfo(string.Format("Server \"{0}\" selected for {1}", server, component));
				SetupVariables.Database = database;
				SetupVariables.DatabaseServer = server;
				SetupVariables.DatabaseType = dbtype;
				SetupVariables.DatabasePort = dbport ?? 0;
				//SetupVariables.DatabaseUser = dbuser;
				//SetupVariables.DatabaseUserPassword = dbpassword;
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
			bool winAuthentication = (cbSqlServerAuthentication.SelectedIndex == 0);
			txtSqlServerLogin.Enabled = !winAuthentication;
			txtSqlServerPassword.Enabled = !winAuthentication;
			lblLogin.Enabled = !winAuthentication;
			lblPassword.Enabled = !winAuthentication;
			Update();
		}

		private bool CheckFields()
		{
			switch (tabControl.SelectedIndex)
			{
				case 0: // MS SQL
					if (txtSqlServerServer.Text.Trim().Length == 0)
					{
						ShowWarning("Please enter valid SQL Server name.");
						return false;
					}
					if (cbSqlServerAuthentication.SelectedIndex == 1)
					{
						if (txtSqlServerLogin.Text.Trim().Length == 0)
						{
							ShowWarning("Please enter valid Login name.");
							return false;
						}
					}
					string database = txtSqlServerDatabase.Text;
					if (database.Trim().Length == 0 || !Data.DatabaseUtils.IsValidDatabaseName(database))
					{
						ShowWarning("Please enter valid database name.");
						return false;
					}

					return true;
				case 1: // MySQL
					if (txtMySqlServer.Text.Trim().Length == 0)
					{
						ShowWarning("Please enter valid server name.");
						return false;
					}
					database = txtMySqlDatabase.Text;
					if (database.Trim().Length == 0 || !Data.DatabaseUtils.IsValidDatabaseName(database))
					{
						ShowWarning("Please enter valid database name.");
						return false;
					}
					int port;
					if (!int.TryParse(txtMySqlPort.Text.Trim(), out port))
					{
						ShowWarning("Please enter valid server port.");
						return false;
					}
					return true;
				case 2: // SQlite
					database = txtMySqlDatabase.Text;
					if (database.Trim().Length == 0 ||
						!(Path.IsPathRooted(database) ||
						database.Contains(Path.DirectorySeparatorChar.ToString()) ||
						Data.DatabaseUtils.IsValidDatabaseName(database)))
					{
						ShowWarning("Please enter valid database name.");
						return false;
					}
					return true;
				default: return false;

			}
		}
		private bool CheckConnection(string connectionString)
		{
			return Data.DatabaseUtils.CheckSqlConnection(connectionString);
		}

		private string GetSqlServerVersion(string connectionString)
		{
			return Data.DatabaseUtils.GetSqlServerVersion(connectionString);
		}

		private int GetSqlServerSecurityMode(string connectionString)
		{
			return Data.DatabaseUtils.GetSqlServerSecurityMode(connectionString);
		}

		private string CreateConnectionString(Data.DbType dbtype)
		{
			switch (dbtype)
			{
				case Data.DbType.SqlServer:
					if (cbSqlServerAuthentication.SelectedIndex == 0)
					{
						return Data.DatabaseUtils.BuildSqlServerMasterConnectionString(txtSqlServerServer.Text.Trim(), null, null);
					}
					else
					{
						return Data.DatabaseUtils.BuildSqlServerMasterConnectionString(txtSqlServerServer.Text.Trim(), txtSqlServerLogin.Text.Trim(), txtSqlServerPassword.Text.Trim());
					}
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					int port = 3306;
					int.TryParse(txtMySqlPort.Text.Trim(), out port);
					return Data.DatabaseUtils.BuildMySqlMasterConnectionString(txtMySqlServer.Text.Trim(),
						port, txtMySqlUser.Text.Trim(), txtMySqlPassword.Text.Trim());
				case Data.DbType.Sqlite:
				case Data.DbType.SqliteFX:
					return Data.DatabaseUtils.BuildSqliteMasterConnectionString(txtSqliteDatabase.Text.Trim(), SetupVariables.InstallationFolder, SetupVariables.EnterpriseServerPath, SetupVariables.EmbedEnterpriseServer);
				default: return "";
			}
		}
	}
}
