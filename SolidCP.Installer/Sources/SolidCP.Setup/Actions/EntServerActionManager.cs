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
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using SolidCP.Providers.Common;
using SolidCP.UniversalInstaller.Core;
using Data = SolidCP.EnterpriseServer.Data;

namespace SolidCP.Setup.Actions
{
	public class SetEntServerWebSettingsAction : Action, IPrepareDefaultsAction
	{
		public const string LogStartMessage = "Retrieving default IP address of the component...";

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.WebSiteIP))
				vars.WebSiteIP = "127.0.0.1";
			//
			if (String.IsNullOrEmpty(vars.WebSitePort))
				vars.WebSitePort = "9002";
			//
			if (String.IsNullOrEmpty(vars.UserAccount))
				vars.UserAccount = "SCPEnterprise";
		}
	}

	public class SetEntServerCryptoKeyAction : Action, IInstallAction
	{
		public const string LogStartInstallMessage = "Updating web.config file (crypto key)";
		public const string LogEndInstallMessage = "Updated web.config file";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				OnInstallProgressChanged(LogStartInstallMessage, 0);
				Log.WriteStart(LogStartInstallMessage);
				var file = Path.Combine(vars.InstallationFolder, vars.ConfigurationFile);
				vars.CryptoKey = Utils.GetRandomString(20);
				var Xml = new XmlDocument();
				Xml.Load(file);
				var CryptoNode = Xml.SelectSingleNode("configuration/appSettings/add[@key='SolidCP.CryptoKey']") as XmlElement;
				if (CryptoNode != null)
					CryptoNode.SetAttribute("value", vars.CryptoKey);
				Xml.Save(file);
				// SolidCP.SchedulerService.exe.config
				var file1 = Path.Combine(vars.InstallationFolder, "Bin", "SolidCP.SchedulerService.exe.config");
				var Xml1 = new XmlDocument();
				Xml1.Load(file1);
				var CryptoNode1 = Xml1.SelectSingleNode("configuration/appSettings/add[@key='SolidCP.CryptoKey']") as XmlElement;
				if (CryptoNode1 != null)
					CryptoNode1.SetAttribute("value", vars.CryptoKey);
				Xml1.Save(file1);
				Log.WriteEnd(LogEndInstallMessage);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update web.config error", ex);
				throw;
			}
		}
	}

	public class InstallSchedulerServiceAction : Action, IInstallAction, IUninstallAction
	{
		public const string LogStartInstallMessage = "Installing Scheduler Windows Service...";
		public const string LogStartUninstallMessage = "Uninstalling Scheduler Windows Service...";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{

				Begin(LogStartInstallMessage);

				Log.WriteStart(LogStartInstallMessage);

				var ServiceName = Global.Parameters.SchedulerServiceName;
				var ServiceFile = Path.Combine(vars.InstallationFolder, "bin", Global.Parameters.SchedulerServiceFileName);

				Log.WriteInfo(String.Format("Scheduler Service Name: \"{0}\"", Global.Parameters.SchedulerServiceName));

				if (ServiceController.GetServices().Any(s => s.DisplayName.Equals(Global.Parameters.SchedulerServiceName, StringComparison.CurrentCultureIgnoreCase)))
				{
					Log.WriteEnd("Scheduler Service Already Installed.");
					InstallLog.AppendLine(String.Format("- Scheduler Service \"{0}\" Already Installed.", Global.Parameters.SchedulerServiceName));
					return;
				}

				ManagedInstallerClass.InstallHelper(new[] { "/i /LogFile=\"\" ", ServiceFile });
				Utils.StartService(Global.Parameters.SchedulerServiceName);

				AppConfig.EnsureComponentConfig(vars.ComponentId);
				AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ServiceName", ServiceName);
				AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ServiceFile", ServiceFile);
				AppConfig.SaveConfiguration();
			}
			catch (Exception ex)
			{
				UninstallService(vars);

				if (Utils.IsThreadAbortException(ex))
				{
					return;
				}

				Log.WriteError("Installing scheduler service error.", ex);
				throw;
			}
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart(LogStartUninstallMessage);
				UninstallService(vars);
				Log.WriteEnd("Scheduler Service Uninstalled.");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
				{
					return;
				}

				Log.WriteError("Uninstalling scheduler service error.", ex);
				throw;
			}
		}

		private void UninstallService(SetupVariables vars)
		{
			if (ServiceController.GetServices().Any(s => s.ServiceName.Equals(Global.Parameters.SchedulerServiceName, StringComparison.CurrentCultureIgnoreCase)))
			{
				ManagedInstallerClass.InstallHelper(new[] { "/u /LogFile=\"\" ", Path.Combine(vars.InstallationFolder, "bin", Global.Parameters.SchedulerServiceFileName) });
			}
		}
	}

	public class CreateDatabaseAction : Action, IInstallAction, IUninstallAction
	{
		public const string LogStartInstallMessage = "Creating database...";
		public const string LogStartUninstallMessage = "Deleting database";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Begin(LogStartInstallMessage);
				//
				var connectionString = vars.DbInstallConnectionString;
				var database = vars.Database;

				Log.WriteStart(LogStartInstallMessage);
				Log.WriteInfo(String.Format("Database Name: \"{0}\"", database));
				//
				if (Data.DatabaseUtils.DatabaseExists(connectionString, database))
				{
					throw new Exception(String.Format("Database \"{0}\" already exists", database));
				}
				Data.DatabaseUtils.CreateDatabase(connectionString, database);
				//
				Log.WriteEnd("Created database");
				//
				InstallLog.AppendLine(String.Format("- Created a new database \"{0}\"", database));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Create database error", ex);
				//
				throw;
			}
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart(LogStartUninstallMessage);
				//
				Log.WriteInfo(String.Format("Deleting database \"{0}\"", vars.Database));
				//
				if (Data.DatabaseUtils.DatabaseExists(vars.DbInstallConnectionString, vars.Database))
				{
					Data.DatabaseUtils.DeleteDatabase(vars.DbInstallConnectionString, vars.Database, vars.InstallationFolder);
					//
					Log.WriteEnd("Deleted database");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database delete error", ex);
				throw;
			}
		}
	}

	public class CreateDatabaseUserAction : Action, IInstallAction, IUninstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				//
				Log.WriteStart(String.Format("Creating database user {0}", vars.Database));
				//
				vars.DatabaseUserPassword = Utils.GetRandomString(20);
				//user name should be the same as database
				vars.NewDatabaseUser = Data.DatabaseUtils.CreateUser(vars.DbInstallConnectionString, vars.Database, vars.DatabaseUserPassword, vars.Database);
				//
				Log.WriteEnd("Created database user");
				InstallLog.AppendLine("- Created database user \"{0}\"", vars.Database);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create db user error", ex);
				throw;
			}
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart("Deleting database user");
				Log.WriteInfo(String.Format("Deleting database user \"{0}\"", vars.Database));
				//
				if (Data.DatabaseUtils.UserExists(vars.DbInstallConnectionString, vars.Database))
				{
					Data.DatabaseUtils.DeleteUser(vars.DbInstallConnectionString, vars.Database);
					//
					Log.WriteEnd("Deleted database user");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database user delete error", ex);
				throw;
			}
		}
	}

	public class ExecuteInstallSqlAction : Action, IInstallAction
	{
		public Data.DbType dbType = Data.DbType.Unknown;

		public string DbTypeId => dbType != Data.DbType.MariaDb ? dbType.ToString().ToLowerInvariant() : "mysql"; 
		public string SqlFilePath => $@"Setup\install.{DbTypeId}.sql";
		public const string ExecuteProgressMessage = "Creating database objects...";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				var component = vars.ComponentFullName;
				var componentId = vars.ComponentId;
				dbType = vars.DatabaseType;

				var path = Path.Combine(vars.InstallationFolder, SqlFilePath);

				if (!FileUtils.FileExists(path))
				{
					Log.WriteInfo(String.Format("File {0} not found", path));
					return;
				}
				//
				SqlProcess process = new SqlProcess(path, vars.DbInstallConnectionString, vars.Database);
				//
				process.ProgressChange += new EventHandler<ActionProgressEventArgs<int>>(process_ProgressChange);
				//
				process.Run();
				//
				InstallLog.AppendLine(string.Format("- Installed {0} database objects", component));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Run sql error", ex);
				//
				throw;
			}
		}

		void process_ProgressChange(object sender, ActionProgressEventArgs<int> e)
		{
			OnInstallProgressChanged(ExecuteProgressMessage, e.EventData);
		}
	}

	public class UpdateServeradminPasswAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart("Updating serveradmin password");
				//
				var path = Path.Combine(vars.InstallationFolder, vars.ConfigurationFile);
				var password = vars.ServerAdminPassword;

				if (!File.Exists(path))
				{
					Log.WriteInfo(String.Format("File {0} not found", path));
					return;
				}
				//
				bool encryptionEnabled = IsEncryptionEnabled(path);
				// Encrypt password
				if (encryptionEnabled)
				{
					password = Utils.Encrypt(vars.CryptoKey, password);
				}
				//
				Data.DatabaseUtils.SetServerAdminPassword(vars.DbInstallConnectionString, vars.Database, password);
				//
				Log.WriteEnd("Updated serveradmin password");
				//
				InstallLog.AppendLine("- Updated password for the serveradmin account");
				//
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Update error", ex);
				throw;
			}
		}

		private bool IsEncryptionEnabled(string path)
		{
			var doc = new XmlDocument();
			doc.Load(path);
			//encryption enabled
			string xPath = "configuration/appSettings/add[@key=\"SolidCP.EncryptionEnabled\"]";
			XmlElement encryptionNode = doc.SelectSingleNode(xPath) as XmlElement;
			bool encryptionEnabled = false;
			//
			if (encryptionNode != null)
			{
				bool.TryParse(encryptionNode.GetAttribute("value"), out encryptionEnabled);
			}
			//
			return encryptionEnabled;
		}
	}

	public class SaveAspNetDbConnectionStringAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			Log.WriteStart("Updating web.config file (connection string)");
			var file = Path.Combine(vars.InstallationFolder, vars.ConfigurationFile);
			vars.ConnectionString = Data.DatabaseUtils.BuildConnectionString(vars.DatabaseType, vars.DatabaseServer,
				vars.DatabasePort, vars.Database, vars.Database, vars.DatabaseUserPassword,
				vars.EnterpriseServerPath, vars.EmbedEnterpriseServer);
			var Xml = XDocument.Load(file);
			var connectionStrings = Xml
				.Element("configuration")
				?.Element("connectionStrings");
			var ConnNode = connectionStrings
				?.Elements("add")
				.FirstOrDefault(e => (string)e.Attribute("name") == "EnterpriseServer");
			if (ConnNode == null) connectionStrings.Add(ConnNode = new XElement("add", new XAttribute("name", "EnterpriseServer")));
			ConnNode.Attribute("connectionString").SetValue(vars.ConnectionString);
			ConnNode.Attribute("providerName")?.Remove();
			Xml.Save(file);
			Log.WriteEnd(String.Format("Updated {0} file", vars.ConfigurationFile));
			// Schedular
			var file1 = Path.Combine(vars.InstallationFolder, "Bin", "SolidCP.SchedulerService.exe.config");
			var Xml1 = XDocument.Load(file1);
			var connectionStrings1 = Xml1
				.Element("configuration")
				?.Element("connectionStrings");
			var ConnNode1 = connectionStrings1
				?.Elements("add")
				.FirstOrDefault(e => (string)e.Attribute("name") == "EnterpriseServer");
			if (ConnNode1 == null) connectionStrings1.Add(ConnNode1 = new XElement("add", new XAttribute("name", "EnterpriseServer")));
			ConnNode1.Attribute("connectionString").SetValue(vars.ConnectionString);
			ConnNode1.Attribute("providerName")?.Remove();
			Xml1.Save(file1);
			Log.WriteEnd(String.Format("Updated {0} file", vars.ConfigurationFile));
		}
	}

	public class SaveSchedulerServiceConnectionStringAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			Log.WriteStart(string.Format("Updating {0}.config file (connection string)", Global.Parameters.SchedulerServiceFileName));
			var file = Path.Combine(vars.InstallationFolder, "bin", string.Format("{0}.config", Global.Parameters.SchedulerServiceFileName));
			string content;

			using (var reader = new StreamReader(file))
			{
				content = reader.ReadToEnd();
			}

			vars.ConnectionString = Data.DatabaseUtils.BuildConnectionString(vars.DatabaseType, vars.DatabaseServer,
				vars.DatabasePort, vars.Database, vars.Database, vars.DatabaseUserPassword,
				vars.EnterpriseServerPath, vars.EmbedEnterpriseServer);
			content = Utils.ReplaceScriptVariable(content, "installer.connectionstring", vars.ConnectionString);

			using (var writer = new StreamWriter(file))
			{
				writer.Write(content);
			}

			Log.WriteEnd(string.Format("Updated {0}.config file (connection string)", Global.Parameters.SchedulerServiceFileName));
		}
	}

	public class SaveSchedulerServiceCryptoKeyAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			Log.WriteStart(string.Format("Updating {0}.config file (crypto key)", Global.Parameters.SchedulerServiceFileName));

			try
			{
				UpdateCryptoKey(vars.InstallationFolder);
			}
			catch (Exception)
			{
			}

			Log.WriteEnd(string.Format("Updated {0}.config file (connection string)", Global.Parameters.SchedulerServiceFileName));
		}

		private static void UpdateCryptoKey(string installFolder)
		{
			string path = Path.Combine(installFolder, "web.config");
			string cryptoKey = "0123456789";

			if (File.Exists(path))
			{
				using (var reader = new StreamReader(path))
				{
					string content = reader.ReadToEnd();
					var pattern = new Regex(@"(?<=<add key=""SolidCP.CryptoKey"" .*?value\s*=\s*"")[^""]+(?="".*?>)");
					Match match = pattern.Match(content);
					cryptoKey = match.Value;
				}
			}

			ChangeConfigString("installer.cryptokey", cryptoKey, installFolder);
		}

		private static void ChangeConfigString(string searchString, string replaceValue, string installFolder)
		{
			string path = Path.Combine(installFolder, "web.config");

			if (File.Exists(path))
			{
				string content;

				using (var reader = new StreamReader(path))
				{
					content = reader.ReadToEnd();
				}

				var re = new Regex("\\$\\{" + searchString + "\\}+", RegexOptions.IgnoreCase);
				content = re.Replace(content, replaceValue);

				using (var writer = new StreamWriter(path))
				{
					writer.Write(content);
				}
			}
		}
	}

	public class SaveEntServerConfigSettingsAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			Log.WriteStart("SaveEntServerConfigSettingsAction");
			AppConfig.EnsureComponentConfig(vars.ComponentId);
			//
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "Database", vars.Database);
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewDatabase", vars.CreateDatabase);
			//
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "DatabaseUser", vars.DatabaseUser);
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewDatabaseUser", vars.NewDatabaseUser);
			//
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, Global.Parameters.ConnectionString, vars.ConnectionString);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, Global.Parameters.DatabaseServer, vars.DatabaseServer);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, Global.Parameters.DatabasePort, vars.DatabasePort.ToString());
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, Global.Parameters.DatabaseServer, vars.DatabaseServer);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, Global.Parameters.InstallConnectionString, vars.DbInstallConnectionString);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, Global.Parameters.CryptoKey, vars.CryptoKey);
			//
			AppConfig.SaveConfiguration();
			Log.WriteEnd("SaveEntServerConfigSettingsAction");
		}
	}

	public class EntServerActionManager : BaseActionManager
	{
		public static readonly List<Action> InstallScenario = new List<Action>
		{
			new SetCommonDistributiveParamsAction(),
			new SetEntServerWebSettingsAction(),
			new EnsureServiceAccntSecured(),
			new CopyFilesAction(),
			new SetEntServerCryptoKeyAction(),
			new SetCertificateAction(),
			new CreateWindowsAccountAction(),
			new ConfigureAspNetTempFolderPermissionsAction(),
			new SetNtfsPermissionsAction(),
			new CreateWebApplicationPoolAction(),
			new CreateWebSiteAction(),
			new InstallLetsEncryptCertificateAction(),
			new SwitchAppPoolAspNetVersion(),
			new CreateDatabaseAction(),
			new CreateDatabaseUserAction(),
			new ExecuteInstallSqlAction(),
			new UpdateServeradminPasswAction(),
			new SaveAspNetDbConnectionStringAction(),
			new SaveComponentConfigSettingsAction(),
			new SaveEntServerConfigSettingsAction(),
			new SaveSchedulerServiceConnectionStringAction(),
			new SaveSchedulerServiceCryptoKeyAction(),
			new InstallSchedulerServiceAction()
		};

		public EntServerActionManager(SetupVariables sessionVars) : base(sessionVars)
		{
			Initialize += new EventHandler(EntServerActionManager_PreInit);
		}

		void EntServerActionManager_PreInit(object sender, EventArgs e)
		{
			//
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install: // Install
					LoadInstallationScenario();
					break;
			}
		}

		private void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}
}
