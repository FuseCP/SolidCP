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
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Collections.Specialized;

using SolidCP.Setup.Common;
using SolidCP.Setup.Web;
using SolidCP.Setup.Windows;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.OS;
using SolidCP.Setup.Actions;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.Setup
{
	public partial class ExpressInstallPage : BannerWizardPage
	{
		private Thread thread;
		private List<InstallAction> actions;

		public ExpressInstallPage()
		{
			InitializeComponent();
			//
			actions = new List<InstallAction>();
			//
			this.CustomCancelHandler = true;
		}

		public List<InstallAction> Actions
		{
			get
			{
				return actions;
			}
		}

		delegate void StringCallback(string value);
		delegate void IntCallback(int value);

		private void SetProgressValue(int value)
		{
			//thread safe call
			if (InvokeRequired)
			{
				IntCallback callback = new IntCallback(SetProgressValue);
				Invoke(callback, new object[] { value });
			}
			else
			{
				progressBar.Value = value;
				Update();
			}
		}

		private void SetProgressText(string text)
		{
			//thread safe call
			if (InvokeRequired)
			{
				StringCallback callback = new StringCallback(SetProgressText);
				Invoke(callback, new object[] { text });
			}
			else
			{
				lblProcess.Text = text;
				Update();
			}
		}
		
		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			string name = Wizard.SetupVariables.ComponentFullName;
			this.Text = string.Format("Installing {0}", name);
			this.Description = string.Format("Please wait while {0} is being installed.", name);
			this.AllowMoveBack = false;
			this.AllowMoveNext = false;
			this.AllowCancel = false;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			thread = new Thread(new ThreadStart(this.Start));
			thread.Start();
		}

		public void Start()
		{
			SetProgressValue(0);

			string component = Wizard.SetupVariables.ComponentFullName;
			string componentName = Wizard.SetupVariables.ComponentName;
			bool isUnattended = !string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml);

			try
			{
				SetProgressText("Creating installation script...");

				for (int i = 0; i < actions.Count; i++)
				{
					InstallAction action = actions[i];
					SetProgressText(action.Description);
					SetProgressValue(0);

					switch (action.ActionType)
					{
						case ActionTypes.SwitchWebPortal2AspNet40:
							SwitchWebPortal2AspNet40(action, Wizard.SetupVariables);
							break;
						case ActionTypes.SwitchEntServer2AspNet40:
							SwitchEntServer2AspNet40(action, Wizard.SetupVariables);
							break;
						case ActionTypes.SwitchServer2AspNet40:
							SwitchServer2AspNet40(action, Wizard.SetupVariables);
							break;
						case ActionTypes.CopyFiles:
							CopyFiles(
								Wizard.SetupVariables.InstallerFolder,
								Wizard.SetupVariables.InstallationFolder);
							break;
						case ActionTypes.CreateWebSite:
							if (OSInfo.IsWindows) CreateWebSite();
							else
							{
								var a = (IInstallAction)new InstallServerUnixAction();
								a.Run(Wizard.SetupVariables);
							}
							break;
						case ActionTypes.ConfigureLetsEncrypt:
							if (!ConfigureLetsEncrypt(isUnattended)) action.Log = "Failed to install Let's Encrypt certificate. Check the error log for details.";
							break;
						case ActionTypes.CryptoKey:
							SetCryptoKey();
							break;
						case ActionTypes.ServerPassword:
							SetServerPassword();
							break;
						case ActionTypes.UpdateServerPassword:
							UpdateServerPassword();
							break;
						case ActionTypes.UpdateConfig:
							UpdateSystemConfiguration();
							break;
						case ActionTypes.CreateDatabase:
							CreateDatabase();
							break;
						case ActionTypes.CreateDatabaseUser:
							CreateDatabaseUser();
							break;
						case ActionTypes.ExecuteSql:
							ExecuteSqlScript(action.Path);
							break;
						case ActionTypes.UpdateWebSite:
							UpdateWebSiteBindings();
							break;
						case ActionTypes.Backup:
							Backup();
							break;
						case ActionTypes.DeleteFiles:
							DeleteFiles(action.Path);
							break;
						case ActionTypes.UpdateEnterpriseServerUrl:
							UpdateEnterpriseServerUrl();
							break;
						case ActionTypes.CreateShortcuts:
							CreateShortcuts();
							break;
						case ActionTypes.UpdateServers:
							UpdateServers();
							break;
						case ActionTypes.CopyWebConfig:
							CopyWebConfig();
							break;
						case ActionTypes.UpdateWebConfigNamespaces:
							UpdateWebConfigNamespaces();
							break;
						case ActionTypes.StopApplicationPool:
							StopApplicationPool();
							break;
						case ActionTypes.StartApplicationPool:
							StartApplicationPool();
							break;
						case ActionTypes.UpdatePortal2811:
							UpdatePortal2811();
							break;
						case ActionTypes.UpdateEnterpriseServer2810:
						case ActionTypes.UpdateServer2810:
							UpdateWseSecuritySettings();
							break;
						case ActionTypes.CreateUserAccount:
							CreateAccount(action.Name);
							break;
						case ActionTypes.ServiceSettings:
							SetServiceSettings();
							break;
						case ActionTypes.RegisterWindowsService:
							RegisterWindowsService();
							break;
						case ActionTypes.StartWindowsService:
							StartWindowsService();
							break;
						case ActionTypes.StopWindowsService:
							StopWindowsService();
							break;
						case ActionTypes.InitSetupVariables:
							InitSetupVaribles(action.SetupVariables);
							break;
						case ActionTypes.UpdateServerAdminPassword:
							UpdateServerAdminPassword();
							break;
						case ActionTypes.UpdateLicenseInformation:
							UpdateLicenseInformation();
							break;
						case ActionTypes.ConfigureStandaloneServerData:
							ConfigureStandaloneServer(action.Url);
							break;
						case ActionTypes.CreateSCPServerLogin:
							CreateSCPServerLogin();
							break;
						case ActionTypes.FolderPermissions:
							ConfigureFolderPermissions();
							break;
						case ActionTypes.AddCustomErrorsPage:
							AddCustomErrorsPage();
							break;
                        case ActionTypes.ConfigureSecureSessionModuleInWebConfig:
                            ConfigureSecureSessionModuleInWebConfig();
                            break;
					}
				}
				this.progressBar.Value = 100;

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				ShowError();
				Rollback();
				return;
			}

			SetProgressText("Completed. Click Next to continue.");
			this.AllowMoveNext = true;
			this.AllowCancel = true;
			//unattended setup
			if (!string.IsNullOrEmpty(SetupVariables.SetupXml))
				Wizard.GoNext();
		}

        private void ConfigureSecureSessionModuleInWebConfig()
        {
            try
            {
                string webConfigPath = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");
                Log.WriteStart("Web.config file is being updated");
                // Ensure the web.config exists
                if (!File.Exists(webConfigPath))
                {
                    Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
                    return;
                }
                // Load web.config
                XmlDocument doc = new XmlDocument();
                doc.Load(webConfigPath);

                // add node:
                //<system.webServer>
                //  <modules>
                //    <add name="SecureSession" type="SolidCP.WebPortal.SecureSessionModule" />
                //  </modules>
                //</system.webServer>
                //
                //  ... or for IIS 6:
                //
                //<system.web>
                //  <httpModules>
                //    <add name="SecureSession" type="SolidCP.WebPortal.SecureSessionModule" />
                //  </httpModules>
                //</system.web>
                bool iis6 = false;
                XmlElement webServer = doc.SelectSingleNode("configuration/system.webServer") as XmlElement;
                if (webServer == null)
                {
                    // this is IIS 6
                    webServer = doc.SelectSingleNode("configuration/system.web") as XmlElement;
                    iis6 = true;
                }

                if (webServer != null)
                {
                    string modulesNodeName = iis6 ? "httpModules" : "modules";
                    if (webServer.SelectSingleNode(modulesNodeName + "/add[@name='SecureSession']") == null)
                    {
                        var modules = doc.CreateElement(modulesNodeName);
                        webServer.AppendChild(modules);
                        var sessionModule = doc.CreateElement("add");
                        sessionModule.SetAttribute("name", "SecureSession");
                        sessionModule.SetAttribute("type", "SolidCP.WebPortal.SecureSessionModule");
                        modules.AppendChild(sessionModule);
                    }
                }

                // update /system.web/httpRuntime element
                var httpRuntime = doc.SelectSingleNode("configuration/system.web/httpRuntime") as XmlElement;
                if (httpRuntime != null)
                    httpRuntime.SetAttribute("enableVersionHeader", "false");

                // add:
                //<appSettings>
                //    <add key="SessionValidationKey" value="XXXXXX" />
                //</appSettings>
                var appSettings = doc.SelectSingleNode("configuration/appSettings");
                if (appSettings != null && appSettings.SelectSingleNode("add[@key='SessionValidationKey']") == null)
                {
                    var sessionKey = doc.CreateElement("add");
                    sessionKey.SetAttribute("key", "SessionValidationKey");
                    sessionKey.SetAttribute("value", StringUtils.GenerateRandomString(16));
                    appSettings.AppendChild(sessionKey);
                }
                
                // save changes have been made
                doc.Save(webConfigPath);
                //
                Log.WriteEnd("Web.config has been updated");
            }
            catch (Exception ex)
            {
                if (Utils.IsThreadAbortException(ex))
                    return;
                Log.WriteError("Could not update web.config file", ex);
                throw;
            }
        }

		private void SwitchWebPortal2AspNet40(InstallAction action, Setup.SetupVariables setupVariables)
		{
			var sam = new WebPortalActionManager(setupVariables);
			sam.AddAction(new RegisterAspNet40Action());
			sam.AddAction(new EnableAspNetWebExtensionAction());
			sam.AddAction(new MigrateWebPortalWebConfigAction());
			sam.AddAction(new SwitchAppPoolAspNetVersion());
			sam.AddAction(new CleanupSolidCPModulesListAction());
			//
			sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
			{
				throw e.OriginalException;
			});
			//
			sam.Start();
		}

		private void SwitchEntServer2AspNet40(InstallAction action, Setup.SetupVariables setupVariables)
		{
			var sam = new EntServerActionManager(setupVariables);
			sam.AddAction(new RegisterAspNet40Action());
			sam.AddAction(new EnableAspNetWebExtensionAction());
			sam.AddAction(new MigrateEntServerWebConfigAction());
			sam.AddAction(new AdjustHttpRuntimeRequestLengthAction());
			sam.AddAction(new SwitchAppPoolAspNetVersion());
			//
			sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
			{
				throw e.OriginalException;
			});
			//
			sam.Start();
		}

		private void SwitchServer2AspNet40(InstallAction action, Setup.SetupVariables setupVariables)
		{
			var sam = new ServerActionManager(setupVariables);
			sam.AddAction(new RegisterAspNet40Action());
			sam.AddAction(new EnableAspNetWebExtensionAction());
			sam.AddAction(new MigrateServerWebConfigAction());
			sam.AddAction(new AdjustHttpRuntimeRequestLengthAction());
			sam.AddAction(new SwitchAppPoolAspNetVersion());
			//
			sam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
			{
				throw e.OriginalException;
			});
			//
			sam.Start();
		}

		private void MigrateServerWebConfigFile(Setup.SetupVariables setupVariables)
		{
			// Migrate web.config
			// IIS 6
			if (setupVariables.IISVersion.Major == 6)
			{
			}
			// IIS 7
			else
			{

			}
		}

		private void UpdatePortal2811()
		{
			try
			{
				string webConfigPath = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");
				Log.WriteStart("Web.config file is being updated");
				// Ensure the web.config exists
				if (!File.Exists(webConfigPath))
				{
					Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
					return;
				}
				// Load web.config
				XmlDocument doc = new XmlDocument();
				doc.Load(webConfigPath);
				// do Windows 2008 platform-specific changes
                bool iis7 = (Wizard.SetupVariables.IISVersion.Major >= 7);
				//
				#region Do IIS 7 and IIS 6 specific web.config file changes
				if (iis7)
				{
					// remove existing node:
					//<system.webServer>
					//	<handlers>
					//		<add name="WebChart" path="WebChart.axd" verb="GET,HEAD" type="blong.WebControls.WebChartImageHandler, WebChart" preCondition="integratedMode,runtimeVersionv2.0" />
					//	</handlers>
					//</system.webServer>
					XmlElement webChartWebServerHandler = doc.SelectSingleNode("configuration/system.webServer/handlers/add[@path='WebChart.axd']") as XmlElement;
					// ensure node is found
					if (webChartWebServerHandler != null)
					{
						var parentNode = webChartWebServerHandler.ParentNode;
						parentNode.RemoveChild(webChartWebServerHandler);
					}
				}
				else
				{
					// remove existing node:
					//<system.web>
					//	<httpHandlers>
					//		<add verb="GET,HEAD" path="WebChart.axd" type="blong.WebControls.WebChartImageHandler, WebChart" validate="false"/>
					//	</httpHandlers>
					//</system.web>
					XmlElement webChartWebServerHandler = doc.SelectSingleNode("configuration/system.web/httpHandlers/add[@path='WebChart.axd']") as XmlElement;
					// ensure node is found
					if (webChartWebServerHandler != null)
					{
						var parentNode = webChartWebServerHandler.ParentNode;
						parentNode.RemoveChild(webChartWebServerHandler);
					}
				} 
				#endregion

				#region CompareValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.CompareValidator" mappedTagType="Sample.Web.UI.Compatibility.CompareValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement compareValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.CompareValidator']") as XmlElement;
				// ensure node is found
				if (compareValidatorMapping != null)
				{
					var parentNode = compareValidatorMapping.ParentNode;
					parentNode.RemoveChild(compareValidatorMapping);
				} 
				#endregion

				#region CustomValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.CustomValidator" mappedTagType="Sample.Web.UI.Compatibility.CustomValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement customValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.CustomValidator']") as XmlElement;
				// ensure node is found
				if (customValidatorMapping != null)
				{
					var parentNode = customValidatorMapping.ParentNode;
					parentNode.RemoveChild(customValidatorMapping);
				} 
				#endregion

				#region RangeValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.RangeValidator" mappedTagType="Sample.Web.UI.Compatibility.RangeValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement rangeValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.RangeValidator']") as XmlElement;
				// ensure node is found
				if (rangeValidatorMapping != null)
				{
					var parentNode = rangeValidatorMapping.ParentNode;
					parentNode.RemoveChild(rangeValidatorMapping);
				} 
				#endregion

				#region RegularExpressionValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.RegularExpressionValidator" mappedTagType="Sample.Web.UI.Compatibility.RegularExpressionValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement regExpValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.RegularExpressionValidator']") as XmlElement;
				// ensure node is found
				if (regExpValidatorMapping != null)
				{
					var parentNode = regExpValidatorMapping.ParentNode;
					parentNode.RemoveChild(regExpValidatorMapping);
				} 
				#endregion

				#region RequiredFieldValidator
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.RequiredFieldValidator" mappedTagType="Sample.Web.UI.Compatibility.RequiredFieldValidator, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement requiredFieldValidatorMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.RequiredFieldValidator']") as XmlElement;
				// ensure node is found
				if (requiredFieldValidatorMapping != null)
				{
					var parentNode = requiredFieldValidatorMapping.ParentNode;
					parentNode.RemoveChild(requiredFieldValidatorMapping);
				} 
				#endregion

				#region ValidationSummary
				// remove existing node:
				//<system.web>
				//	<pages>
				//		<tagMapping>
				//			<add tagType="System.Web.UI.WebControls.ValidationSummary" mappedTagType="Sample.Web.UI.Compatibility.ValidationSummary, Validators, Release=1.0.0.0"/>
				//		</tagMapping>
				//	</pages>
				//</system.webServer>
				XmlElement validationSummaryMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping/add[@tagType='System.Web.UI.WebControls.ValidationSummary']") as XmlElement;
				// ensure node is found
				if (validationSummaryMapping != null)
				{
					var parentNode = validationSummaryMapping.ParentNode;
					parentNode.RemoveChild(validationSummaryMapping);
				} 
				#endregion

				#region tagMapping
				// remove existing node only if it does not have any siblings:
				//<system.web>
				//	<pages>
				//		<tagMapping />
				//	</pages>
				//</system.webServer>
				XmlElement tagMapping = doc.SelectSingleNode("configuration/system.web/pages/tagMapping") as XmlElement;
				// ensure node is found
				if (tagMapping != null && !tagMapping.HasChildNodes)
				{
					var parentNode = tagMapping.ParentNode;
					parentNode.RemoveChild(tagMapping);
				} 
				#endregion

				// save changes have been made
				doc.Save(webConfigPath);
				//
				Log.WriteEnd("Web.config has been updated");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Could not update web.config file", ex);
				throw;
			}
		}

		private void UpdateLicenseInformation()
		{
			try
			{
				if (string.IsNullOrEmpty(Wizard.SetupVariables.LicenseKey))
					return;

				Log.WriteStart("Updating license information");

				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, Wizard.SetupVariables.ConfigurationFile);
				string licenseKey = Wizard.SetupVariables.LicenseKey;

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				string connectionString = GetConnectionString(path);
				if (string.IsNullOrEmpty(connectionString))
				{
					Log.WriteError("Connection string setting not found");
					return;
				}

				string cryptoKey = GetCryptoKey(path);
				if (string.IsNullOrEmpty(cryptoKey))
				{
					Log.WriteError("CryptoKey setting not found");
					return;
				}

				bool encryptionEnabled = IsEncryptionEnabled(path);
				//encrypt password
				if (encryptionEnabled)
				{
					licenseKey = Utils.Encrypt(cryptoKey, licenseKey);
				}

				string query = string.Format("INSERT INTO Licenses ( SerialNumber ) VALUES ('{0}')", licenseKey);
				DatabaseUtils.ExecuteQuery(connectionString, query);

				Log.WriteEnd("Updated license information");
				InstallLog.AppendLine("- Updated license information");

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update error", ex);
				throw;
			}
		}

		private string GetConnectionString(string webConfigPath)
		{
			string ret = null;
			XmlDocument doc = new XmlDocument();
			doc.Load(webConfigPath);
			//connection string
			string xPath = "configuration/connectionStrings/add[@name=\"EnterpriseServer\"]";
			XmlElement connectionStringNode = doc.SelectSingleNode(xPath) as XmlElement;
			if (connectionStringNode != null)
			{
				ret = connectionStringNode.GetAttribute("connectionString");
			}
			return ret;
		}

		private string GetCryptoKey(string webConfigPath)
		{
			string ret = null;
			XmlDocument doc = new XmlDocument();
			doc.Load(webConfigPath);
			//crypto key
			string xPath = "configuration/appSettings/add[@key=\"SolidCP.CryptoKey\"]";
			XmlElement keyNode = doc.SelectSingleNode(xPath) as XmlElement;
			if (keyNode != null)
			{
				ret = keyNode.GetAttribute("value");;
			}
			return ret;
		}

		private bool IsEncryptionEnabled(string webConfigPath)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(webConfigPath);
			//encryption enabled
			string xPath = "configuration/appSettings/add[@key=\"SolidCP.EncryptionEnabled\"]";
			XmlElement encryptionNode = doc.SelectSingleNode(xPath) as XmlElement;
			bool encryptionEnabled = false;
			if (encryptionNode != null)
			{
				bool.TryParse(encryptionNode.GetAttribute("value"), out encryptionEnabled);
			}
			return encryptionEnabled;
		}
		#region SolidCP providioning
		private void ConfigureStandaloneServer(string enterpriseServerUrl)
		{
			try
			{
				Log.WriteStart("Configuring SolidCP");
				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolvePortalAssembly);

				SetProgressText("Configuring SolidCP...");
				if (!ConnectToEnterpriseServer(enterpriseServerUrl, "serveradmin", Wizard.SetupVariables.ServerAdminPassword))
				{
					Log.WriteError("Enterprise Server connection error");
					return;
				}
				SetProgressValue(10);

				//Add server
				int serverId = AddServer(Wizard.SetupVariables.RemoteServerUrl, "My Server", Wizard.SetupVariables.RemoteServerPassword);
				if (serverId < 0)
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serverId));
					return;
				}
				//Add IP address
				string portalIP = AppConfig.GetComponentSettingStringValue(Wizard.SetupVariables.PortalComponentId, "WebSiteIP");
				int ipAddressId = AddIpAddress(portalIP, serverId);
				SetProgressValue(20);
				//Add OS service
				int osServiceId = AddOSService(serverId);
				SetProgressValue(30);
				//Add Web service
				int webServiceId = AddWebService(serverId, ipAddressId);
				SetProgressValue(40);
				//Add Sql service
				int sqlServiceId = AddSqlService(serverId);
				SetProgressValue(50);
				//Add Dns service
				int dnsServiceId = AddDnsService(serverId, ipAddressId);
				SetProgressValue(60);
				//Add virtual server
				int virtualServerId = AddVirtualServer("My Server Resources", serverId, new int[] { osServiceId, webServiceId, sqlServiceId, dnsServiceId });
				SetProgressValue(70);
				//Add user
				int userId = AddUser("admin", Wizard.SetupVariables.ServerAdminPassword, "Server", "Administrator", "admin@myhosting.com");
				SetProgressValue(80);
				//Add plan
				int planId = -1;
				if (virtualServerId > 0)
				{
					planId = AddHostingPlan("My Server", virtualServerId);
				}
				SetProgressValue(90);
				//Add package
				if (userId > 0 && planId > 0)
				{
					int packageId = AddPackage("My Server", userId, planId);
				}
				SetProgressValue(95);
				ConfigureWebPolicy(1);
				SetProgressValue(100);
				Log.WriteEnd("Server configured");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Server configuration error", ex);
			}
		}

		private void ConfigureWebPolicy(int userId)
		{
			try
			{
				Log.WriteStart("Configuring Web Policy");
				UserSettings settings = ES.Services.Users.GetUserSettings(userId, "WebPolicy");
				settings["AspNetInstalled"] = "2I";
				if (SetupVariables.IISVersion.Major == 6)
					settings["AspNetInstalled"] = "2";
				ES.Services.Users.UpdateUserSettings(settings);
				Log.WriteEnd("Configured Web Policy");
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Web policy configuration error", ex);
			}
		}

		private int AddOSService(int serverId)
		{
			try
			{
				Log.WriteStart("Adding OS service");
				ServiceInfo serviceInfo = new ServiceInfo();
				serviceInfo.ServerId = serverId;
				serviceInfo.ServiceName = "OS";
				serviceInfo.Comments = string.Empty;

				//check OS version
				OS.WindowsVersion version = OS.GetVersion();
				if (version == OS.WindowsVersion.WindowsServer2003)
				{
					serviceInfo.ProviderId = 1;
				}
				else if (version == OS.WindowsVersion.WindowsServer2008)
				{
					serviceInfo.ProviderId = 100;
				}
				int serviceId = ES.Services.Servers.AddService(serviceInfo);
				if (serviceId > 0)
				{
					InstallService(serviceId);
					Log.WriteEnd("Added OS service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("OS service configuration error", ex);
				return -1;
			}
		}

		private int AddWebService(int serverId, int ipAddressId)
		{
			try
			{
				Log.WriteStart("Adding Web service");
				ServiceInfo serviceInfo = new ServiceInfo();
				serviceInfo.ServerId = serverId;
				serviceInfo.ServiceName = "Web";
				serviceInfo.Comments = string.Empty;

				//check IIS version
                if (Wizard.SetupVariables.IISVersion.Major >= 7)
				{
					serviceInfo.ProviderId = 101;
				}
				else if (Wizard.SetupVariables.IISVersion.Major == 6)
				{
					serviceInfo.ProviderId = 2;
				}
				int serviceId = ES.Services.Servers.AddService(serviceInfo);
				if (serviceId > 0)
				{
					StringDictionary settings = GetServiceSettings(serviceId);
					if (settings != null)
					{
						// set ip address						
						if (ipAddressId > 0)
							settings["sharedip"] = ipAddressId.ToString();

						// settings for win2003 x64
						if (Wizard.SetupVariables.IISVersion.Major == 6 &&
							Utils.IsWin64() && !Utils.IIS32Enabled())
						{
							settings["AspNet20Path"] = @"%SYSTEMROOT%\Microsoft.NET\Framework64\v2.0.50727\aspnet_isapi.dll";
							settings["Php4Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php.exe";
							settings["Php5Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
						}
						// settings for win2008 x64
						if (Wizard.SetupVariables.IISVersion.Major > 6 &&
							Utils.IsWin64())
						{
							settings["Php4Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php.exe";
							settings["Php5Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
							settings["phppath"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
						}

						UpdateServiceSettings(serviceId, settings);
					}
					InstallService(serviceId);
					Log.WriteEnd("Added Web service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Web service configuration error", ex);
				return -1;
			}
		}

		private int AddSqlService(int serverId)
		{
			int serviceId = -1;
			try
			{
				Log.WriteStart("Adding Sql service");

				SqlServerItem item = ParseConnectionString(Wizard.SetupVariables.DbInstallConnectionString);
				string serverName = item.Server.ToLower();
				if (serverName.StartsWith("(local)") ||
					serverName.StartsWith("localhost") ||
					serverName.StartsWith(System.Environment.MachineName.ToLower()))
				{
					ServiceInfo serviceInfo = new ServiceInfo();
					serviceInfo.ServerId = serverId;
					serviceInfo.ServiceName = "SQL Server";
					serviceInfo.Comments = string.Empty;

					string connectionString = Wizard.SetupVariables.DbInstallConnectionString;
					//check SQL version
					if (DatabaseUtils.CheckSqlConnection(connectionString))
					{
						// check SQL server version
						string sqlVersion = DatabaseUtils.GetSqlServerVersion(connectionString);
						if (sqlVersion.StartsWith("9."))
						{
							serviceInfo.ProviderId = 16;
						}
						else if (sqlVersion.StartsWith("10."))
						{
							serviceInfo.ProviderId = 202;
						}
						else if (sqlVersion.StartsWith("11."))
						{
							serviceInfo.ProviderId = 209;
						}
                        else if (sqlVersion.StartsWith("12."))
                        {
                            serviceInfo.ProviderId = 1203;
                        }
                        else if (sqlVersion.StartsWith("13."))
                        {
                            serviceInfo.ProviderId = 1701;
                        }
                        else if (sqlVersion.StartsWith("14."))
                        {
                            serviceInfo.ProviderId = 1704;
                        }
                        else if (sqlVersion.StartsWith("15."))
                        {
                            serviceInfo.ProviderId = 1705;
                        }
                        else if (sqlVersion.StartsWith("16."))
                        {
                            serviceInfo.ProviderId = 1706;
                        }
                        serviceId = ES.Services.Servers.AddService(serviceInfo);
					}
					else
						Log.WriteInfo("SQL Server connection error");
					//configure service
					if (serviceId > 0)
					{
						StringDictionary settings = GetServiceSettings(serviceId);
						if (settings != null)
						{
							settings["InternalAddress"] = item.Server;
							settings["ExternalAddress"] = string.Empty;
							settings["UseTrustedConnection"] = item.WindowsAuthentication.ToString();
							settings["SaLogin"] = item.User;
							settings["SaPassword"] = item.Password;
							UpdateServiceSettings(serviceId, settings);
						}
						InstallService(serviceId);
						Log.WriteEnd("Added Sql service");
					}
					else
					{
						Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
					}
				}
				else
				{
					Log.WriteError("Microsoft SQL Server was not found");
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Sql service configuration error", ex);
				return -1;
			}
		}

		private int AddDnsService(int serverId, int ipAddressId)
		{
			try
			{
				Log.WriteStart("Adding DNS service");
				int providerId = 7;
				int serviceId = -1;
				BoolResult result = ES.Services.Servers.IsInstalled(serverId, providerId);
				if (result.IsSuccess && result.Value)
				{
					ServiceInfo serviceInfo = new ServiceInfo();
					serviceInfo.ServerId = serverId;
					serviceInfo.ServiceName = "DNS";
					serviceInfo.Comments = string.Empty;
					serviceInfo.ProviderId = providerId;
					serviceId = ES.Services.Servers.AddService(serviceInfo);
				}
				else
				{
					Log.WriteInfo("Microsoft DNS was not found");
					return -1;
				}

				if (serviceId > 0)
				{
					StringDictionary settings = GetServiceSettings(serviceId);
					if (settings != null)
					{
						if (ipAddressId > 0)
							settings["listeningipaddresses"] = ipAddressId.ToString();
						UpdateServiceSettings(serviceId, settings);
					}
					InstallService(serviceId);
					Log.WriteEnd("Added DNS service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("DNS service configuration error", ex);
				return -1;
			}
		}

		private int AddServer(string url, string name, string password)
		{
			try
			{
				Log.WriteStart("Adding server");
				ServerInfo serverInfo = new ServerInfo()
				{
					ADAuthenticationType = null,
					ADPassword = null,
					ADEnabled = false,
					ADRootDomain = null,
					ADUsername = null,
					Comments = string.Empty,
					Password = password,
					ServerName = name,
					ServerUrl = url,
					VirtualServer = false
				};

				int serverId = ES.Services.Servers.AddServer(serverInfo, false);
				if (serverId > 0)
				{
					Log.WriteEnd("Added server");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serverId));
				}
				return serverId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Server configuration error", ex);
				return -1;
			}
		}

		private int AddIpAddress(string ip, int serverId)
		{
			try
			{
				Log.WriteStart("Adding IP address");
				IntResult res = ES.Services.Servers.AddIPAddress(IPAddressPool.General, serverId, ip, string.Empty, string.Empty, string.Empty, string.Empty, 0);
				if (res.IsSuccess && res.Value > 0)
				{
					Log.WriteEnd("Added IP address");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", res.Value));
				}
				return res.Value;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("IP address configuration error", ex);
				return -1;
			}
		}

		private int AddVirtualServer(string name, int serverId, int[] services)
		{
			Log.WriteStart("Adding virtual server");
			ServerInfo serverInfo = new ServerInfo()
			{
				Comments = string.Empty,
				ServerName = name,
				VirtualServer = true
			};

			int virtualServerId = ES.Services.Servers.AddServer(serverInfo, false);
			if (virtualServerId > 0)
			{
				List<int> allServices = new List<int>(services);
				List<int> validServices = new List<int>();
				foreach (int serviceId in allServices)
				{
					if (serviceId > 0)
						validServices.Add(serviceId);
				}
				ES.Services.Servers.AddVirtualServices(virtualServerId, validServices.ToArray());
				Log.WriteEnd("Added virtual server");
			}
			else
			{
				Log.WriteError(string.Format("Enterprise Server error: {0}", virtualServerId));
			}

			return virtualServerId;
		}

		private int AddUser(string loginName, string password, string firstName, string lastName, string email)
		{
			try
			{
				Log.WriteStart("Adding user account");
				UserInfo user = new UserInfo();
				user.UserId = 0;
				user.Role = UserRole.User;
				user.StatusId = 1;
				user.OwnerId = 1;
				user.IsDemo = false;
				user.IsPeer = false;
				user.HtmlMail = true;
				user.Username = loginName;
				user.FirstName = firstName;
				user.LastName = lastName;
				user.Email = email;

				int userId = ES.Services.Users.AddUser(user, false, password, new string[0]);
				if (userId > 0)
				{
					Log.WriteEnd("Added user account");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", userId));
				}
				return userId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("User configuration error", ex);
				return -1;
			}
		}

		private int AddHostingPlan(string name, int serverId)
		{
			try
			{
				Log.WriteStart("Adding hosting plan");
				// gather form info
				HostingPlanInfo plan = new HostingPlanInfo();
				plan.UserId = 1;
				plan.PlanId = 0;
				plan.IsAddon = false;
				plan.PlanName = name;
				plan.PlanDescription = "";
				plan.Available = true; // always available

				plan.SetupPrice = 0;
				plan.RecurringPrice = 0;
				plan.RecurrenceLength = 1;
				plan.RecurrenceUnit = 2; // month

				plan.PackageId = 0;
				plan.ServerId = serverId;
				List<HostingPlanGroupInfo> groups = new List<HostingPlanGroupInfo>();
				List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();

				DataSet ds = ES.Services.Packages.GetHostingPlanQuotas(-1, 0, serverId);

				foreach (DataRow groupRow in ds.Tables[0].Rows)
				{
					bool enabled = (bool)groupRow["ParentEnabled"];
					if (!enabled)
						continue; // disabled group

					int groupId = (int)groupRow["GroupId"]; ;

					HostingPlanGroupInfo group = new HostingPlanGroupInfo();
					group.GroupId = groupId;
					group.Enabled = true;
					group.CalculateDiskSpace = (bool)groupRow["CalculateDiskSpace"];
					group.CalculateBandwidth = (bool)groupRow["CalculateBandwidth"];
					groups.Add(group);

					DataView dvQuotas = new DataView(ds.Tables[1], "GroupID=" + group.GroupId.ToString(), "", DataViewRowState.CurrentRows);
					List<HostingPlanQuotaInfo> groupQuotas = GetGroupQuotas(groupId, dvQuotas);
					quotas.AddRange(groupQuotas);

				}

				plan.Groups = groups.ToArray();
				plan.Quotas = quotas.ToArray();

				int planId = ES.Services.Packages.AddHostingPlan(plan);
				if (planId > 0)
				{
					Log.WriteEnd("Added hosting plan");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", planId));
				}
				return planId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Hosting plan configuration error", ex);
				return -1;
			}
		}

		private List<HostingPlanQuotaInfo> GetGroupQuotas(int groupId, DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			//OS quotas
			if (groupId == 1)
				quotas = GetOSQuotas(dvQuotas);
			//Web quotas
			else if (groupId == 2)
				quotas = GetWebQuotas(dvQuotas);
			else
			{
				foreach (DataRowView quotaRow in dvQuotas)
				{
					int quotaTypeId = (int)quotaRow["QuotaTypeID"];
					HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
					quota.QuotaId = (int)quotaRow["QuotaID"];
					quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
					quotas.Add(quota);
				}
			}
			return quotas;
		}

		private List<HostingPlanQuotaInfo> GetOSQuotas(DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			foreach (DataRowView quotaRow in dvQuotas)
			{
				int quotaTypeId = (int)quotaRow["QuotaTypeID"];
				string quotaName = (string)quotaRow["QuotaName"];
				HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
				quota.QuotaId = (int)quotaRow["QuotaID"];
				quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
				if (quotaName == "OS.AppInstaller" ||
					quotaName == "OS.ExtraApplications")
					quota.QuotaValue = 0;

				quotas.Add(quota);
			}
			return quotas;
		}

		private List<HostingPlanQuotaInfo> GetWebQuotas(DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			foreach (DataRowView quotaRow in dvQuotas)
			{
				int quotaTypeId = (int)quotaRow["QuotaTypeID"];
				string quotaName = (string)quotaRow["QuotaName"];
				HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
				quota.QuotaId = (int)quotaRow["QuotaID"];
				quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
				if (quotaName == "Web.Asp" ||
					quotaName == "Web.AspNet11" ||
					quotaName == "Web.Php4" ||
					quotaName == "Web.Perl" ||
					quotaName == "Web.CgiBin" ||
					quotaName == "Web.SecuredFolders" ||
					quotaName == "Web.SharedSSL" ||
					quotaName == "Web.Python" ||
					quotaName == "Web.AppPools" ||
					quotaName == "Web.IPAddresses" ||
					quotaName == "Web.ColdFusion" ||
					quotaName == "Web.CFVirtualDirectories" ||
					quotaName == "Web.RemoteManagement")
					quota.QuotaValue = 0;

				quotas.Add(quota);
			}
			return quotas;
		}

		private int AddPackage(string name, int userId, int planId)
		{
			try
			{
				Log.WriteStart("Adding hosting space");
				// gather form info
				PackageResult res = ES.Services.Packages.AddPackageWithResources(userId, planId,
					name, 1, false, false, string.Empty, false, false, false, null, false,string.Empty);
				if (res.Result > 0)
					Log.WriteEnd("Added hosting space");
				else
					Log.WriteError(string.Format("Enterprise Server error: {0}", planId));
				return res.Result;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Hosting space configuration error", ex);
				return -1;
			}
		}

		private bool ConnectToEnterpriseServer(string url, string username, string password)
		{
			return ES.Connect(url, username, password);
		}

		private void UpdateServiceSettings(int serviceId, StringDictionary settings)
		{
			if (serviceId < 0 || settings == null)
				return;

			try
			{
				// save settings
				int result = ES.Services.Servers.UpdateServiceSettings(serviceId,
					ConvertDictionaryToArray(settings));

				if (result < 0)
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", result));
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Update service settings error", ex);
			}
		}

		private void InstallService(int serviceId)
		{
			if (serviceId < 0)
				return;

			string[] installResults = null;

			try
			{
				installResults = ES.Services.Servers.InstallService(serviceId);
				foreach (string result in installResults)
				{
					Log.WriteInfo(result);
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Install service error", ex);
			}
		}

		private StringDictionary GetServiceSettings(int serviceId)
		{
			StringDictionary ret = null;
			try
			{
				if (serviceId > 0)
				{
					// load service properties and bind them
					string[] settings = ES.Services.Servers.GetServiceSettings(serviceId);
					ret = ConvertArrayToDictionary(settings);
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Get service settings error", ex);
			}
			return ret;
		}

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

		private SqlServerItem ParseConnectionString(string connectionString)
		{
			SqlServerItem ret = new SqlServerItem();

			ret.WindowsAuthentication = false;
			string[] pairs = connectionString.Split(';');
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
							ret.Server = value;
							break;
						case "database":
							ret.Database = value;
							break;
						case "integrated security":
							if (value.Trim().ToLower() == "sspi")
								ret.WindowsAuthentication = true;
							break;
						case "user":
						case "user id":
							ret.User = value;
							break;
						case "password":
							ret.Password = value;
							break;
					}
				}
			}
			return ret;
		}

		private Assembly ResolvePortalAssembly(object sender, ResolveEventArgs args)
		{
			Assembly ret = null;

			string portalPath = AppConfig.GetComponentSettingStringValue(Wizard.SetupVariables.PortalComponentId, "InstallFolder"); 
			string binPath = Path.Combine(portalPath, "bin");
			string path = Path.Combine(binPath, args.Name.Split(',')[0] + ".dll");
			Log.WriteInfo("Assembly to resolve: " + path);
			if (File.Exists(path))
			{
				ret = Assembly.LoadFrom(path);
				Log.WriteInfo("Assembly resolved: " + path);
			}
			return ret;
		}
		#endregion

		private void CreateSCPServerLogin()
		{
			try
			{
				Log.WriteStart("Creating SolidCP login");
				string query = string.Empty;

				string connectionString = AppConfig.GetComponentSettingStringValue(
					Wizard.SetupVariables.EnterpriseServerComponentId,
					"InstallConnectionString");

				SqlServerItem item = ParseConnectionString(connectionString);
				string serverName = item.Server.ToLower();
				if (serverName.StartsWith("(local)") ||
					serverName.StartsWith("localhost") ||
					serverName.StartsWith(System.Environment.MachineName.ToLower()))
				{

					string domain = AppConfig.GetComponentSettingStringValue(
						Wizard.SetupVariables.ServerComponentId,
						"Domain");
					if (string.IsNullOrEmpty(domain))
						domain = System.Environment.MachineName;

					string userAccount = AppConfig.GetComponentSettingStringValue(
							Wizard.SetupVariables.ServerComponentId,
							"UserAccount");

					string loginName = string.Format("{0}\\{1}", domain, userAccount);

					if (!DatabaseUtils.LoginExists(connectionString, loginName))
					{
						query = string.Format("CREATE LOGIN [{0}] FROM WINDOWS WITH DEFAULT_DATABASE=[master]", loginName);
						DatabaseUtils.ExecuteQuery(connectionString, query);
					}
					query = string.Format("EXEC master..sp_addsrvrolemember @loginame = N'{0}', @rolename = N'sysadmin'", loginName);
					DatabaseUtils.ExecuteQuery(connectionString, query);
					
					AppConfig.SetComponentSettingStringValue(Wizard.SetupVariables.EnterpriseServerComponentId, "DatabaseLogin", loginName);
					
					Log.WriteEnd("Created SolidCP login");
				}
				else
				{
					Log.WriteInfo("Microsoft SQL Server is not located on the local server.");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("SQL error", ex);
			}
		}

		private void UpdateServerAdminPassword()
		{
			try
			{
				if (!Wizard.SetupVariables.UpdateServerAdminPassword)
					return;

				Log.WriteStart("Updating serveradmin password");

				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, Wizard.SetupVariables.ConfigurationFile);
				string password = Wizard.SetupVariables.ServerAdminPassword;

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				string connectionString = GetConnectionString(path);
				if (string.IsNullOrEmpty(connectionString))
				{
					Log.WriteError("Connection string setting not found");
					return;
				}

				string cryptoKey = GetCryptoKey(path);
				if (string.IsNullOrEmpty(cryptoKey))
				{
					Log.WriteError("CryptoKey setting not found");
					return;
				}

				bool encryptionEnabled = IsEncryptionEnabled(path);
				//encrypt password
				if (encryptionEnabled)
				{
					password = Utils.Encrypt(cryptoKey, password);
				}

				string query = string.Format("UPDATE Users SET Password = '{0}' WHERE UserID = 1", password);
				DatabaseUtils.ExecuteQuery(connectionString, query);

				Log.WriteEnd("Updated serveradmin password");
				InstallLog.AppendLine("- Updated password for the serveradmin account");
		
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update error", ex);
				throw;
			}
		}

		private void InitSetupVaribles(SetupVariables setupVariables)
		{
			try
			{
				Wizard.SetupVariables = setupVariables.Clone();
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
			}
		}

		private void RegisterWindowsService()
		{
			try
			{
				string componentId = Wizard.SetupVariables.ComponentId;
				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, Wizard.SetupVariables.ServiceFile);
				string service = Wizard.SetupVariables.ServiceName;
				if (!File.Exists(path))
				{
					Log.WriteError(string.Format("File {0} not found", path), null);
					return;
				}

				Log.WriteStart(string.Format("Registering \"{0}\" windows service", service));
				string domain = Wizard.SetupVariables.UserDomain;
				if ( string.IsNullOrEmpty(domain))
					domain = ".";
				string arguments = string.Format("/i /user={0}\\{1} /password={2}",
					domain, Wizard.SetupVariables.UserAccount, Wizard.SetupVariables.UserPassword); 
				int exitCode = Utils.RunProcess(path, arguments);
				if (exitCode == 0)
				{
					//add rollback action
					RollBack.RegisterWindowsService(path, service);

					//update log
					Log.WriteEnd("Registered windows service");
					//update install log
					InstallLog.AppendLine(string.Format("- Registered \"{0}\" Windows service ", service));
				}
				else
				{
					Log.WriteError(string.Format("Unable to register \"{0}\" Windows service. Error code: {1}", service, exitCode), null);
					InstallLog.AppendLine(string.Format("- Failed to register \"{0}\" windows service ", service));
				}
				// update config setings
				AppConfig.SetComponentSettingStringValue(componentId, "ServiceName", Wizard.SetupVariables.ServiceName);
				AppConfig.SetComponentSettingStringValue(componentId, "ServiceFile", Wizard.SetupVariables.ServiceFile);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Windows service error", ex);
			}
		}

		private void StartWindowsService()
		{
			try
			{
				string service = Wizard.SetupVariables.ServiceName;
				Log.WriteStart(string.Format("Starting \"{0}\" Windows service", service));
				Utils.StartService(service);
				//update log
				Log.WriteEnd("Started Windows service");
				InstallLog.AppendLine(string.Format("- Started \"{0}\" Windows service ", service));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Windows service start error", ex);
			}
		}

		private void StopWindowsService()
		{
			try
			{
				string service = Wizard.SetupVariables.ServiceName;
				Log.WriteStart(string.Format("Stopping \"{0}\" Windows service", service));
				Utils.StopService(service);
				//update log
				Log.WriteEnd("Stopped Windows service");
				InstallLog.AppendLine(string.Format("- Stopped \"{0}\" Windows service ", service));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Windows service stop error", ex);
			}
		}

		private void StopApplicationPool()
		{
			try
			{
				string componentId = Wizard.SetupVariables.ComponentId;
				string appPool = AppConfig.GetComponentSettingStringValue(componentId, "ApplicationPool");
				if (string.IsNullOrEmpty(appPool))
					return;
				
				Version iisVersion = Wizard.SetupVariables.IISVersion;
                bool iis7 = (iisVersion.Major >= 7); 

				Log.WriteStart("Stopping IIS Application Pool");
				Log.WriteInfo(string.Format("Stopping \"{0}\"", appPool));
				if (iis7)
					WebUtils.StopIIS7ApplicationPool(appPool);
				else
					WebUtils.StopApplicationPool(appPool);
				
				Log.WriteEnd("Stopped IIS Application Pool");
				// rollback
				if (iis7)
					RollBack.RegisterStopIIS7ApplicationPool(appPool);
				else
					RollBack.RegisterStopApplicationPool(appPool);

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Application Pool stop error", ex);
			}
		}

		private void StartApplicationPool()
		{
			try
			{
				string componentId = Wizard.SetupVariables.ComponentId;
				string appPool = AppConfig.GetComponentSettingStringValue(componentId, "ApplicationPool");
				if (string.IsNullOrEmpty(appPool))
					return;

				Version iisVersion = Wizard.SetupVariables.IISVersion;
                bool iis7 = (iisVersion.Major >= 7);

				Log.WriteStart("Starting IIS Application Pool");
				Log.WriteInfo(string.Format("Starting \"{0}\"", appPool));
				if (iis7)
					WebUtils.StartIIS7ApplicationPool(appPool);
				else
					WebUtils.StartApplicationPool(appPool);

				Log.WriteEnd("Started IIS Application Pool");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Application Pool start error", ex);
			}
		}

		private void UpdateServers()
		{
			try
			{
				if (Wizard.SetupVariables.SQLServers == null)
					return;

				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, "config.xml");

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Updating config.xml file");
				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlNode serversNode = doc.SelectSingleNode("//myLittleAdmin/sqlservers");
				if (serversNode == null)
				{
					Log.WriteInfo("sql server setting not found");
					return;
				}
				
				if ( serversNode.HasChildNodes )
					serversNode.RemoveAll();

				foreach (ServerItem item in Wizard.SetupVariables.SQLServers)
				{
					XmlElement serverNode = doc.CreateElement("sqlserver");
					serverNode.SetAttribute("address", item.Server);
					serverNode.SetAttribute("name", item.Name);
					serversNode.AppendChild(serverNode);
				}
				doc.Save(path);
				Log.WriteEnd("Updated config.xml file");
				InstallLog.AppendLine("- Updated config.xml file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update config.xml error", ex);
				throw;
			}
		}

		public void InstallInstaller()
		{

		}
		private void CreateShortcuts()
		{
			try
			{
				string ip = Wizard.SetupVariables.WebSiteIP;
				string domain = Wizard.SetupVariables.WebSiteDomain;
				string port = Wizard.SetupVariables.WebSitePort;

				string[] urls = GetApplicationUrls(ip, domain, port, null);
				string url = null;
				if (urls.Length > 0)
					url = Utils.IsHttps(ip, domain) ? "https://" + url[0] : "http://" + url[0];
				else
				{
					Log.WriteInfo("Application url not found");
					return;
				}

				Log.WriteStart("Creating menu shortcut");
				string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
				string fileName = "Login to SolidCP.url";
				string path = Path.Combine(programs, "SolidCP Software");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				path = Path.Combine(path, fileName);
				using (StreamWriter sw = File.CreateText(path))
				{
					WriteShortcutData(url, sw);
				}
				Log.WriteEnd("Created menu shortcut");

				Log.WriteStart("Creating desktop shortcut");
				string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				path = Path.Combine(desktop, fileName);
				using (StreamWriter sw = File.CreateText(path))
				{
					WriteShortcutData(url, sw);
				}
				Log.WriteEnd("Created desktop shortcut");

				InstallLog.AppendLine("- Created application shortcuts");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Create shortcut error", ex);
				//throw;
			}
		}

		private static void WriteShortcutData(string url, StreamWriter sw)
		{
			sw.WriteLine("[InternetShortcut]");
			sw.WriteLine("URL=" + url);
			string iconFile = Path.Combine(Environment.SystemDirectory, "url.dll");
			sw.WriteLine("IconFile=" + iconFile);
			sw.WriteLine("IconIndex=0");
			sw.WriteLine("HotKey=0");
			Log.WriteInfo(string.Format("Shortcut url: {0}", url));
		}

		/// <summary>
		/// Tighten WSE security for Server
		/// </summary>
		private void UpdateWseSecuritySettings()
		{
			try
			{
				string webConfigPath = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");
				Log.WriteStart("Web.config file is being updated");
				// Ensure the web.config exists
				if (!File.Exists(webConfigPath))
				{
					Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
					return;
				}
				// Load web.config
				XmlDocument doc = new XmlDocument();
				doc.Load(webConfigPath);

				// Tighten WSE security on local machine
				XmlElement httpPostLocalhost = doc.SelectSingleNode("configuration/system.web/webServices/protocols/remove[@name='HttpPostLocalhost']") as XmlElement;
				// ensure node is found
				if (httpPostLocalhost == null)
				{
					var protocolsNode = doc.SelectSingleNode("configuration/system.web/webServices/protocols");
					//
					if (protocolsNode != null)
					{
						protocolsNode.InnerXml += "<remove name=\"HttpPostLocalhost\"/>";
					}
				}

				// save changes have been made
				doc.Save(webConfigPath);
				//
				Log.WriteEnd("Web.config has been updated");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Could not update web.config file", ex);
				throw;
			}
		}

		private void CopyWebConfig()
		{
			try
			{
				Log.WriteStart("Copying web.config");
				string configPath = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");
				string config6Path = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web6.config");

				bool iis7 = (Wizard.SetupVariables.IISVersion.Major == 6);
				if (!File.Exists(config6Path))
				{
					Log.WriteInfo(string.Format("File {0} not found", config6Path));
					return;
				}

				if (iis7)
				{
					if (!File.Exists(configPath))
					{
						Log.WriteInfo(string.Format("File {0} not found", configPath));
						return;
					}

					FileUtils.DeleteFile(configPath);
					File.Move(config6Path, configPath);
				}
				else
				{
					FileUtils.DeleteFile(config6Path);
				}
				Log.WriteEnd("Copied web.config");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Copy web.config error", ex);
				throw;
			}
		}

		private void LoadPortal153Settings()
		{
			try
			{
				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Loading portal settings");
				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				string xPath = "configuration/connectionStrings/add[@name=\"SiteSqlServer\"]";
				XmlElement connectionNode = doc.SelectSingleNode(xPath) as XmlElement;
				if (connectionNode != null)
				{
					string connectionString = connectionNode.GetAttribute("connectionString");
					Wizard.SetupVariables.ConnectionString = connectionString;
					Log.WriteInfo(string.Format("Connection string loaded: {0}", connectionString));
				}
				else
				{
					Wizard.SetupVariables.ConnectionString = null;
					Log.WriteError("Connection string not found!", null);
				}
				
				Log.WriteEnd("Loaded portal settings");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Loading portal settings error", ex);
				throw;
			}
		}

		private void UpdateWebConfigNamespaces()
		{
			try
			{
				// find all .config files in the installation directory
				string[] configFiles = Directory.GetFiles(Wizard.SetupVariables.InstallationFolder,
					"*.config", SearchOption.TopDirectoryOnly);

				if (configFiles != null && configFiles.Length > 0)
				{
					foreach (string path in configFiles)
					{
						try
						{
							Log.WriteStart(String.Format("Updating '{0}' file", path));

							// load configuration file in memory
							string content = File.ReadAllText(path);

							// replace DotNetPark. to empty strings
							content = Regex.Replace(content, "dotnetpark\\.", "", RegexOptions.IgnoreCase);

							// save updated config
							File.WriteAllText(path, content);

							Log.WriteEnd(String.Format("Updated '{0}' file", path));
							InstallLog.AppendLine(String.Format("- Updated {0} file", path));
						}
						catch (Exception ex)
						{
							if (Utils.IsThreadAbortException(ex))
								return;
							Log.WriteError(String.Format("Error updating '{0}' file", path), ex);
							throw;
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Error listing *.config files", ex);
				throw;
			}
		}

		private void UpdateEnterpriseServerUrl()
		{
			try
			{
				string url = Wizard.SetupVariables.EnterpriseServerURL;
				string installFolder = Wizard.SetupVariables.InstallationFolder;
				string file = @"App_Data\SiteSettings.config";

				string path = Path.Combine(installFolder, file);

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}
					
				Log.WriteStart("Updating site settings");
				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement urlNode = doc.SelectSingleNode("SiteSettings/EnterpriseServer") as XmlElement;
				if (urlNode == null)
				{
					Log.WriteInfo("EnterpriseServer setting not found");
					return;
				}

				urlNode.InnerText = url;
				doc.Save(path);
				Log.WriteEnd("Updated site settings");
				InstallLog.AppendLine("- Updated site settings");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Site settigs error", ex);
				throw;
			}
		}

		private void DeleteFiles(string file)
		{
			try
			{
				string component = Wizard.SetupVariables.ComponentFullName;
				string installerFolder = Wizard.SetupVariables.InstallerFolder;
				string installFolder = Wizard.SetupVariables.InstallationFolder;
				
				//file with list of files to delete
				string path = Path.Combine(installerFolder, file);

				if (!File.Exists(path))
					return;

				Log.WriteStart("Deleting files");
				long count = 0;

				using (StreamReader reader = new StreamReader(path))
				{
					string fileName;
					string filePath;
					// Read and display lines from the file until the end of the file is reached.
					while ((fileName = reader.ReadLine()) != null)
					{
						if (!string.IsNullOrEmpty(fileName))
						{
							filePath = Path.Combine(installFolder, fileName);
							if (Directory.Exists(filePath))
							{
								FileUtils.DeleteDirectory(filePath);
								count++;
							}
							else if (File.Exists(filePath))
							{
								FileUtils.DeleteFile(filePath);
								count++;
							}
						}
					}
				}
				Log.WriteEnd(string.Format("Deleted {0} files", count));
				InstallLog.AppendLine(string.Format("- Deleted {0} files", count));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("File delete error", ex);
				throw;
			}
		}

		private void Backup()
		{
			try
			{
				string componentId = Wizard.SetupVariables.ComponentId;
				string componentName = Wizard.SetupVariables.ComponentFullName;
				string version = Wizard.SetupVariables.Version;
				List<InstallAction> actions = GenerateBackupActions(componentId);

				Log.WriteStart("Creating backup directory");
				string backupDirectory = Path.Combine(Wizard.SetupVariables.BaseDirectory, "Backup");
				if (!Directory.Exists(backupDirectory))
				{
					Directory.CreateDirectory(backupDirectory);
				}
				
				string destinationDirectory = Path.Combine(backupDirectory,
					string.Format("{0}{1}{2} {3}",
					DateTime.Now.ToString("yyyy-MM-dd"),
					Path.DirectorySeparatorChar,
					componentName,
					version));

				if (Directory.Exists(destinationDirectory))
				{
					//clear existing dir
					FileUtils.DeleteDirectory(destinationDirectory);
				}

				Directory.CreateDirectory(destinationDirectory);
				Log.WriteEnd("Created backup directory");
				
				for (int i = 0; i < actions.Count; i++)
				{
					InstallAction action = actions[i];
					SetProgressText(action.Description);
					SetProgressValue(i * 100 / actions.Count);

					switch (action.ActionType)
					{
						case ActionTypes.BackupDirectory:
							BackupDirectory(action.Path, destinationDirectory);
							break;
						case ActionTypes.BackupDatabase:
							BackupDatabase( action.ConnectionString, action.Name);
							break;
						case ActionTypes.BackupConfig:
							BackupConfig(action.Path, destinationDirectory);
							break;
					}
				}
				this.progressBar.Value = 100;

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}

		private void BackupConfig(string path, string backupDirectory)
		{
			try
			{
				Log.WriteStart("Backing up system configuration");
				
				string destination = Path.Combine(backupDirectory, "Config");
				if (!Directory.Exists(destination))
				{
					Log.WriteStart(string.Format("Creating directory {0}", destination));
					Directory.CreateDirectory(destination);
					Log.WriteEnd("Created directory");
				}

				string[] files = Directory.GetFiles(path, "*.config", SearchOption.TopDirectoryOnly);
				foreach (string file in files)
				{
					FileUtils.CopyFileToFolder(file, destination);
				}
				Log.WriteEnd("Backed up system configuration");
				InstallLog.AppendLine("- Backed up system configuration");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}

		private void BackupDatabase(string connectionString, string database)
		{
			try
			{
				Log.WriteStart(string.Format("Backing up database \"{0}\"", database));
				string bakFile;
				string position;
				DatabaseUtils.BackupDatabase(connectionString, database, out bakFile, out position);
				Log.WriteEnd("Backed up database");
				InstallLog.AppendLine(string.Format("- Backed up {0} database", database));
				RollBack.RegisterDatabaseBackupAction(connectionString, database, bakFile, position);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}

		private void BackupDirectory(string source, string backupDirectory)
		{
			try
			{
				string componentName = Wizard.SetupVariables.ComponentFullName;
				string destination = Path.Combine(backupDirectory, "App");

				if (Directory.Exists(destination))
				{
					try
					{
						Log.WriteStart(string.Format("Deleting directory {0}", destination));
						FileUtils.DeleteDirectory(destination);
						Log.WriteEnd("Deleted directory");
					}
					catch (Exception ex)
					{
						Log.WriteError("Backup error", ex);
					}
				}

				if (!Directory.Exists(destination))
				{
					Log.WriteStart(string.Format("Creating directory {0}", destination));
					Directory.CreateDirectory(destination);
					Log.WriteEnd("Created directory");
				}
				string zipFile = Path.Combine(destination, "app.zip");

				Log.WriteStart("Backing up files");
				Log.WriteInfo(string.Format("Zipping files from \"{0}\" to \"{1}\"", source, zipFile));
				//showing process
				ZipIndicator process = new ZipIndicator(progressBar, source, zipFile);
				//CopyProcess process = new CopyProcess(progressBar, source, destination);
				process.Start();
				Log.WriteEnd("Backed up files");
				InstallLog.AppendLine(string.Format("- Backed up {0} files", componentName));
				RollBack.RegisterDirectoryBackupAction(source, zipFile);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Backup error", ex);
				throw;
			}
		}

		private List<InstallAction> GenerateBackupActions(string componentId)
		{
			List<InstallAction> list = new List<InstallAction>();
			InstallAction action = null;

			//database
			string connectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
			if (!String.IsNullOrEmpty(connectionString))
			{
				string database = AppConfig.GetComponentSettingStringValue(componentId, "Database");
				action = new InstallAction(ActionTypes.BackupDatabase);
				action.ConnectionString = connectionString;
				action.Name = database;
				action.Description = string.Format("Backing up database {0}...", database);
				list.Add(action);
			}
			
			//directory
			string path = AppConfig.GetComponentSettingStringValue(componentId, "InstallFolder");
			if (!string.IsNullOrEmpty(path))
			{
				action = new InstallAction(ActionTypes.BackupDirectory);
				action.Path = path;
				action.Description = string.Format("Backing up directory {0}...", path);
				list.Add(action);
			}

			//config
			action = new InstallAction(ActionTypes.BackupConfig);
			action.Description = "Backing up configuration settings...";
			action.Path = Wizard.SetupVariables.BaseDirectory;
			list.Add(action);

			return list;

		}

		private void UpdateWebSiteBindings()
		{
			string componentId = Wizard.SetupVariables.ComponentId;
			string component = Wizard.SetupVariables.ComponentFullName;
			string componenCode = Wizard.SetupVariables.ComponentCode;
			string siteId = Wizard.SetupVariables.WebSiteId;
			string ip = Wizard.SetupVariables.WebSiteIP;
			string port = Wizard.SetupVariables.WebSitePort;
			string domain = Wizard.SetupVariables.WebSiteDomain;
			bool update = Wizard.SetupVariables.UpdateWebSite;
			Version iisVersion = Wizard.SetupVariables.IISVersion;
            bool iis7 = (iisVersion.Major >= 7); 
			
			if (!update)
				return;
			
			//updating web site
			try
			{
				Log.WriteStart("Updating web site");
				Log.WriteInfo(string.Format("Updating web site \"{0}\" ( IP: {1}, Port: {2}, Domain: {3} )", siteId, ip, port, domain));

				if (OSInfo.IsWindows)
				{
					//check for existing site
					var oldSiteId = iis7 ? WebUtils.GetIIS7SiteIdByBinding(ip, port, domain) : WebUtils.GetSiteIdByBinding(ip, port, domain);
					// We found out that other web site has this combination of {IP:Port:Host Header} already assigned
					if (oldSiteId != null && !oldSiteId.Equals(Wizard.SetupVariables.WebSiteId))
					{
						// get site name
						string oldSiteName = iis7 ? oldSiteId : WebUtils.GetSite(oldSiteId).Name;
						throw new Exception(
							String.Format("'{0}' web site already has server binding ( IP: {1}, Port: {2}, Domain: {3} )",
							oldSiteName, ip, port, domain));
					}

					// Assign the binding only if is not defined
					if (String.IsNullOrEmpty(oldSiteId))
					{
						ServerBinding newBinding = new ServerBinding(ip, port, domain, null, componentId);
						if (iis7)
							WebUtils.UpdateIIS7SiteBindings(siteId, new ServerBinding[] { newBinding });
						else
							WebUtils.UpdateSiteBindings(siteId, new ServerBinding[] { newBinding });
					}
				} else if (componenCode == Global.Server.ComponentCode)
				{
					var installer = UniversalInstaller.Installer.Current;
					var isHttps = Utils.IsHttps(ip, domain);
					port = (isHttps && port == "443" || !isHttps && port == "80") ? "" : $":{port}";
					installer.ReadServerConfiguration();
					installer.ServerSettings.Urls = $"{(isHttps ? "https" : "http")}://{(!string.IsNullOrWhiteSpace(domain) ? domain : "localhost")}{port}";
					installer.ConfigureServer();
				}

				// update config setings
				AppConfig.SetComponentSettingStringValue(componentId, "WebSiteIP", ip);
				AppConfig.SetComponentSettingStringValue(componentId, "WebSitePort", port);
				AppConfig.SetComponentSettingStringValue(componentId, "WebSiteDomain", domain);

				//update log
				Log.WriteEnd("Updated web site");

				//update install log
				InstallLog.AppendLine("- Updated web site");
				InstallLog.AppendLine("  You can access the application by the following URLs:");
				string[] urls = GetApplicationUrls(ip, domain, port, null);
				//
				foreach (string url in urls)
				{
					InstallLog.AppendLine(Utils.IsHttpsAndNotWindows(ip, domain) ? "  https://" + url : "  http://" + url);
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Update web site error", ex);
				throw;
			}

			//opening windows firewall ports
			try
			{
				Utils.OpenFirewallPort(component, port, SetupVariables.IISVersion);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Open windows firewall port error", ex);
			}
		}

		private void CreateDatabaseUser()
		{
			try
			{
				string connectionString = Wizard.SetupVariables.DbInstallConnectionString;
				string database = Wizard.SetupVariables.Database;
				string component = Wizard.SetupVariables.ComponentFullName;
				//user name should be the same as database
				string userName = Wizard.SetupVariables.Database;
				string password = Utils.GetRandomString(20);

				CreateDbUser(connectionString, database, userName, password);
				UpdateWebConfigConnection(userName, password);

				InstallLog.AppendLine(string.Format("- Created database user \"{0}\"", userName));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create db user error", ex);
				throw;
			}
		}

		private void CreateDbUser(string connectionString, string database, string userName, string password)
		{
			Log.WriteStart(string.Format("Creating database user {0}", userName));

			if (DatabaseUtils.UserExists(connectionString, userName))
				throw new Exception(string.Format("Database user {0} already exists", userName)); 
			
			bool userCreated = DatabaseUtils.CreateUser(connectionString, userName, password, database);

			// save user details
			string componentId = Wizard.SetupVariables.ComponentId;
			AppConfig.SetComponentSettingStringValue(componentId, "DatabaseUser", userName);
			AppConfig.SetComponentSettingBooleanValue(componentId, "NewDatabaseUser", userCreated);

			// roll-back support
			if (userCreated)
				RollBack.RegisterDatabaseUserAction(connectionString, userName);

			Log.WriteEnd("Created database user");
		}

		private void UpdateWebConfigConnection(string userName, string password)
		{
			Log.WriteStart("Updating web.config file (connection string)");
			
			string file = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");

			string content = string.Empty;
			// load file
			using (StreamReader reader = new StreamReader(file))
			{
				content = reader.ReadToEnd();
			}
			
			string connectionString = string.Format("server={0};database={1};uid={2};pwd={3};",
					Wizard.SetupVariables.DatabaseServer, Wizard.SetupVariables.Database, userName, password);

			// expand variables
			content = Utils.ReplaceScriptVariable(content, "installer.connectionstring", connectionString);

			// save file
			using (StreamWriter writer = new StreamWriter(file))
			{
				writer.Write(content);
			}
			Log.WriteEnd("Updated web.config file");

			//update settings
			string componentId = Wizard.SetupVariables.ComponentId;
			AppConfig.SetComponentSettingStringValue(componentId, "ConnectionString", connectionString);
		}

		private void ExecuteSqlScript(string file)
		{
			try
			{
				string component = Wizard.SetupVariables.ComponentFullName;
				string componentId = Wizard.SetupVariables.ComponentId;

				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, file);
				if (Wizard.SetupVariables.SetupAction == SetupActions.Update)
				{
					path = Path.Combine(Wizard.SetupVariables.InstallerFolder, file);
					Wizard.SetupVariables.DbInstallConnectionString = AppConfig.GetComponentSettingStringValue(componentId, "InstallConnectionString");
					Wizard.SetupVariables.Database = AppConfig.GetComponentSettingStringValue(componentId, "Database");
				}

				if (!FileUtils.FileExists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}
				
				string connectionString = Wizard.SetupVariables.DbInstallConnectionString;
				string database = Wizard.SetupVariables.Database;
				
				//if (Wizard.SetupVariables.SetupAction == SetupActions.Install)
				//{
				//    UpdateSqlScript(path);
				//}
				RunSqlScript(connectionString, database, path);

				InstallLog.AppendLine(string.Format("- Installed {0} database objects", component));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Run sql error", ex);
				throw;
			}
		}

		private void CreateDatabase()
		{
			try
			{
				string connectionString = Wizard.SetupVariables.DbInstallConnectionString;
				string database = Wizard.SetupVariables.Database;

				Log.WriteStart("Creating SQL Server database");
				Log.WriteInfo(string.Format("Creating SQL Server database \"{0}\"", database));
				if (DatabaseUtils.DatabaseExists(connectionString, database))
				{
					throw new Exception(string.Format("SQL Server database \"{0}\" already exists", database));
				}
				DatabaseUtils.CreateDatabase(connectionString, database);
				Log.WriteEnd("Created SQL Server database");
				
				// rollback
				RollBack.RegisterDatabaseAction(connectionString, database);

				string componentId = Wizard.SetupVariables.ComponentId;
				AppConfig.SetComponentSettingStringValue(componentId, "Database", database);
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewDatabase", true);

				InstallLog.AppendLine(string.Format("- Created a new SQL Server database \"{0}\"", database));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Create database error", ex);
				throw;
			}
		}

		private void SetServerPassword()
		{
			if (!OSInfo.IsWindows)
			{
				SetServerPasswordUnix();
				return;
			}

			try
			{
				Log.WriteStart("Updating configuration file (server password)");

				string file = Path.Combine(Wizard.SetupVariables.InstallationFolder, Wizard.SetupVariables.ConfigurationFile);
				string hash = Wizard.SetupVariables.ServerPassword;

				// load file
				string content = string.Empty;
				using (StreamReader reader = new StreamReader(file))
				{
					content = reader.ReadToEnd();
				}
				
				// expand variables
				content = Utils.ReplaceScriptVariable(content, "installer.server.password", hash);

				// save file
				using ( StreamWriter writer = new StreamWriter(file))
				{
					writer.Write(content);
				}
				//update log
				Log.WriteEnd("Updated configuration file");

				//string component = Wizard.SetupVariables.ComponentFullName;
				//InstallLog.AppendLine(string.Format("- Updated {0} web.config file", component));

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void SetServerPasswordUnix()
		{
			try
			{
				Log.WriteStart("Updating configuration file (server password)");
				var installer = UniversalInstaller.Installer.Current;
				installer.InstallWebRootPath = Wizard.SetupVariables.InstallationFolder;
				installer.ReadServerConfiguration();
				installer.ServerSettings.ServerPasswordSHA = Wizard.SetupVariables.ServerPassword;
				installer.ConfigureServer();
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated password in the configuration file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void UpdateServerPassword()
		{
			if (!OSInfo.IsWindows)
			{
				UpdateServerPasswordUnix();
				return;
			}

			try
			{
				if (!Wizard.SetupVariables.UpdateServerPassword)
					return;

				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, Wizard.SetupVariables.ConfigurationFile);
				string hash = Wizard.SetupVariables.ServerPassword;

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Updating configuration file (server password)");
				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement passwordNode = doc.SelectSingleNode("//SolidCP.server/security/password") as XmlElement;
				if (passwordNode == null)
				{
					Log.WriteInfo("server password setting not found");
					return;
				}

				passwordNode.SetAttribute("value", hash);
				doc.Save(path);
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated password in the configuration file");

				//string component = Wizard.SetupVariables.ComponentFullName;
				//InstallLog.AppendLine(string.Format("- Updated {0} web.config file", component));

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void UpdateServerPasswordUnix()
		{
			try
			{
				Log.WriteStart("Updating configuration file (server password)");
				var installer = UniversalInstaller.Installer.Current;
				installer.InstallWebRootPath = Wizard.SetupVariables.InstallationFolder;
				installer.ReadServerConfiguration();
				installer.ServerSettings.ServerPassword = Wizard.SetupVariables.ServerPassword;
				installer.ConfigureServer();
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated password in the configuration file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void SetServiceSettings()
		{
			try
			{
				string path = Path.Combine(Wizard.SetupVariables.InstallationFolder, Wizard.SetupVariables.ConfigurationFile);
				string ip = Wizard.SetupVariables.ServiceIP;
				string port = Wizard.SetupVariables.ServicePort;

				if (!File.Exists(path))
				{
					Log.WriteInfo(string.Format("File {0} not found", path));
					return;
				}

				Log.WriteStart("Updating configuration file (service settings)");
				XmlDocument doc = new XmlDocument();
				doc.Load(path);

				XmlElement ipNode = doc.SelectSingleNode("//configuration/appSettings/add[@key='SolidCP.HostIP']") as XmlElement;
				if (ipNode == null)
				{
					Log.WriteInfo("Service host IP setting not found");
					return;
				}
				ipNode.SetAttribute("value", ip);
				XmlElement portNode = doc.SelectSingleNode("//configuration/appSettings/add[@key='SolidCP.HostPort']") as XmlElement;
				if (portNode == null)
				{
					Log.WriteInfo("Service host port setting not found");
					return;
				}
				portNode.SetAttribute("value", port);
				doc.Save(path);
				Log.WriteEnd("Updated configuration file");
				InstallLog.AppendLine("- Updated service settings in the configuration file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Configuration file update error", ex);
				throw;
			}
		}

		private void SetCryptoKey()
		{
			try
			{
				Log.WriteStart("Updating web.config file (crypto key)");

				string file = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");
				string cryptoKey = Utils.GetRandomString(20);

				// load file
				string content = string.Empty;
				using (StreamReader reader = new StreamReader(file))
				{
					content = reader.ReadToEnd();
				}

				// expand variables
				content = Utils.ReplaceScriptVariable(content, "installer.cryptokey", cryptoKey);

				// save file
				using (StreamWriter writer = new StreamWriter(file))
				{
					writer.Write(content);
				}
				//update log
				Log.WriteEnd("Updated web.config file");

				string componentId = Wizard.SetupVariables.ComponentId;
				Wizard.SetupVariables.CryptoKey = cryptoKey;
				AppConfig.SetComponentSettingStringValue(componentId, "CryptoKey", cryptoKey);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Update web.config error", ex);
				throw;
			}
		}

		private void CopyFiles(string source, string destination)
		{
			try
			{
				string component = Wizard.SetupVariables.ComponentFullName;
				Log.WriteStart("Copying files");
				Log.WriteInfo(string.Format("Copying files from \"{0}\" to \"{1}\"", source, destination));
				//showing copy process
				CopyProcess process = new CopyProcess(progressBar, source, destination);
				process.Run();
				Log.WriteEnd("Copied files");
				InstallLog.AppendLine(string.Format("- Copied {0} files", component));
				// rollback
				RollBack.RegisterDirectoryAction(destination);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Copy error", ex);
				throw;
			}
		}

		private void CreateWebSite()
		{
			string component = Wizard.SetupVariables.ComponentFullName;
			string ip = Wizard.SetupVariables.WebSiteIP;
			string port = Wizard.SetupVariables.WebSitePort;
			string domain = Wizard.SetupVariables.WebSiteDomain;
			string contentPath = Wizard.SetupVariables.InstallationFolder;
			
			//creating user account
			string userName = Wizard.SetupVariables.UserAccount;
			string password = Wizard.SetupVariables.UserPassword;
			string userDomain = Wizard.SetupVariables.UserDomain;
			string userDescription = component + " account for anonymous access to Internet Information Services";
			string[] memberOf = Wizard.SetupVariables.UserMembership;
			string identity = userName;
			string netbiosDomain = userDomain;
			Version iisVersion = Wizard.SetupVariables.IISVersion;
            bool iis7 = (iisVersion.Major >= 7);
			
			try
			{
				CreateUserAccount(userDomain, userName, password, userDescription, memberOf);
				if (!string.IsNullOrEmpty(userDomain))
				{
					netbiosDomain = SecurityUtils.GetNETBIOSDomainName(userDomain);
					if (iis7)
					{
						//for iis7 we use fqdn\user
						identity = string.Format("{0}\\{1}", userDomain, userName);
					}
					else 
					{
						//for iis6 we use netbiosdomain\user
						identity = string.Format("{0}\\{1}", netbiosDomain, userName);
					}					
				}
				WebUtils.SetWebFolderPermissions(contentPath, netbiosDomain, userName);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create user account error", ex);
				throw;
			}
			progressBar.Value = 30;

			//creating app pool
			string appPool = component + " Pool";
			try
			{
				CreateAppPool(appPool, identity, password);
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError(string.Format("Create application pool \"{0}\" error", appPool), ex);
				throw;
			}
			progressBar.Value = 60;

			//creating web site
			string siteName = component;
			try
			{
				CreateSite(siteName, ip, port, domain, contentPath, identity, password, appPool);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create web site error", ex);
				throw;
			}

			progressBar.Value = 90;
			
			//opening windows firewall ports
			try
			{
				Utils.OpenFirewallPort(component, port, SetupVariables.IISVersion);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Open windows firewall port error", ex);
			}

		}
		private bool ConfigureLetsEncrypt(bool isUnattended)
		{
			string ip = Wizard.SetupVariables.WebSiteIP;
			string siteId = Wizard.SetupVariables.WebSiteId;
			string domain = Wizard.SetupVariables.WebSiteDomain;
			string email = Wizard.SetupVariables.LetsEncryptEmail;
			var componentId = Wizard.SetupVariables.ComponentId;
			bool updateWCF = componentId == "enterpriseserver" || componentId == "server";
			var iisVersion = Wizard.SetupVariables.IISVersion;
			var iis7 = (iisVersion.Major >= 7);
			bool success = true;

			if ((iis7 || !OSInfo.IsWindows) && Utils.IsHttps(ip, domain) && !string.IsNullOrEmpty(email))
			{
				if (OSInfo.IsWindows)
				{
					success = WebUtils.LEInstallCertificate(siteId, domain, email, updateWCF);
				}
			}
			if (!success)
			{
				Log.WriteError("Error creating Let's Encrypt certificate. Check the error log for details.");
				if (!isUnattended) MessageBox.Show("Error creating Let's Encrypt certificate. Check the error log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return success;
		}
		private void ConfigureFolderPermissions()
		{
			try
			{
				string path; 
				if (Wizard.SetupVariables.IISVersion.Major == 6)
				{
					// IIS_WPG -> C:\WINDOWS\Temp
					path = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine);
					SetFolderPermission(path, "IIS_WPG", NtfsPermission.Modify);
					
					// IIS_WPG - > C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Temporary ASP.NET Files
					path = Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(),
						"Temporary ASP.NET Files");
					if (Utils.IsWin64() && Utils.IIS32Enabled())
						path = path.Replace("Framework64", "Framework");
					SetFolderPermission(path, "IIS_WPG", NtfsPermission.Modify);
				}
				// NETWORK_SERVICE -> C:\WINDOWS\Temp
				path = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine);
				SetFolderPermissionBySid(path, SystemSID.NETWORK_SERVICE, NtfsPermission.Modify);
				
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}

		private void SetFolderPermission(string path, string account, NtfsPermission permission)
		{
			try
			{
				if (!FileUtils.DirectoryExists(path))
				{
					FileUtils.CreateDirectory(path);
					Log.WriteInfo(string.Format("Created {0} folder", path));
				}

				Log.WriteStart(string.Format("Setting '{0}' permission for '{1}' folder for '{2}' account", permission, path, account));
				SecurityUtils.GrantNtfsPermissions(path, null, account, permission, true, true);
				Log.WriteEnd("Set security permissions");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}

		private void SetFolderPermissionBySid(string path, string account, NtfsPermission permission)
		{
			try
			{
				if (!FileUtils.DirectoryExists(path))
				{
					FileUtils.CreateDirectory(path);
					Log.WriteInfo(string.Format("Created {0} folder", path));
				}

				Log.WriteStart(string.Format("Setting '{0}' permission for '{1}' folder for '{2}' account", permission, path, account));
				SecurityUtils.GrantNtfsPermissionsBySid(path, account, permission, true, true);
				Log.WriteEnd("Set security permissions");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}
	
		private void CreateSite(string siteName, string ip, string port, string domain, string contentPath, string userName, string userPassword, string appPool)
		{
			SetProgressText("Creating web site...");

			Log.WriteStart("Creating web site");
			Log.WriteInfo(string.Format("Creating web site \"{0}\" ( IP: {1}, Port: {2}, Domain: {3} )", siteName, ip, port, domain));
			Version iisVersion = Wizard.SetupVariables.IISVersion;
			var componentId = Wizard.SetupVariables.ComponentId;
            bool iis7 = (iisVersion.Major >= 7);

			//check for existing site
			string oldSiteId = iis7 ? WebUtils.GetIIS7SiteIdByBinding(ip, port, domain) : WebUtils.GetSiteIdByBinding(ip, port, domain);
			if (oldSiteId != null)
			{
				// get site name
				string oldSiteName = iis7 ? oldSiteId : WebUtils.GetSite(oldSiteId).Name;
				throw new Exception(
					String.Format("'{0}' web site already has server binding ( IP: {1}, Port: {2}, Domain: {3} )",
					oldSiteName, ip, port, domain));
			}

			// create site
			WebSiteItem site = new WebSiteItem();
			site.Name = siteName;
			site.SiteIPAddress = ip;

			site.ContentPath = contentPath;
			//site.LogFileDirectory = logsPath;

			//set bindings
			ServerBinding binding = new ServerBinding(ip, port, domain, null, componentId);
			site.Bindings = new ServerBinding[] { binding };

			// set other properties
			site.AllowExecuteAccess = false;
			site.AllowScriptAccess = true;
			site.AllowSourceAccess = false;
			site.AllowReadAccess = true;
			site.AllowWriteAccess = false;
			site.AnonymousUsername = userName;
			site.AnonymousUserPassword = userPassword;
			site.AllowDirectoryBrowsingAccess = false;

			site.AuthAnonymous = true;
			site.AuthWindows = true;

			site.DefaultDocs = null; // inherit from service
			site.HttpRedirect = "";

			site.InstalledDotNetFramework = AspNetVersion.AspNet20;
			site.ApplicationPool = appPool;

			// create site
			string newSiteId = iis7 ? WebUtils.CreateIIS7Site(site) : WebUtils.CreateSite(site);
			
			//add rollback action
			if (iis7)
				RollBack.RegisterIIS7WebSiteAction(newSiteId);
			else
				RollBack.RegisterWebSiteAction(newSiteId);
			
			
			// update config setings
			AppConfig.SetComponentSettingStringValue(componentId, "WebSiteId", newSiteId);
			AppConfig.SetComponentSettingStringValue(componentId, "WebSiteIP", ip);
			AppConfig.SetComponentSettingStringValue(componentId, "WebSitePort", port);
			AppConfig.SetComponentSettingStringValue(componentId, "WebSiteDomain", domain);
			AppConfig.SetComponentSettingStringValue(componentId, "VirtualDirectory", string.Empty);
			AppConfig.SetComponentSettingBooleanValue(componentId, "NewWebSite", true);
			AppConfig.SetComponentSettingBooleanValue(componentId, "NewVirtualDirectory", false);

			// update setup variables
			Wizard.SetupVariables.WebSiteId = newSiteId;

			//update log
			Log.WriteEnd("Created web site");
			
			//update install log
			InstallLog.AppendLine(string.Format("- Created a new web site named \"{0}\" ({1})", siteName, newSiteId));
			InstallLog.AppendLine("  You can access the application by the following URLs:");
			string[] urls = GetApplicationUrls(ip, domain, port, null);
			foreach (string url in urls)
			{
				InstallLog.AppendLine(Utils.IsHttps(ip, domain) ? "  https://" + url : "  http://" + url);
			}
		}

		private void CreateAccount(string description)
		{
			string component = Wizard.SetupVariables.ComponentFullName;

			//creating user account
			string userName = Wizard.SetupVariables.UserAccount;
			string password = Wizard.SetupVariables.UserPassword;
			string userDomain = Wizard.SetupVariables.UserDomain;
			string userDescription = description;
			string[] memberOf = Wizard.SetupVariables.UserMembership;

			try
			{
				CreateUserAccount(userDomain, userName, password, userDescription, memberOf);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create user account error", ex);
				throw;
			}
		}

		private void CreateUserAccount(string domain, string userName, string password, string description, string[] memberOf)
		{
			SetProgressText("Creating windows user account...");
			this.Update();
			string componentId = Wizard.SetupVariables.ComponentId;

			if (!SecurityUtils.UserExists(domain, userName))
			{
				Log.WriteStart("Creating Windows user account");
				Log.WriteInfo(string.Format("Creating Windows user account \"{0}\"", userName));
				
				// create account
				SystemUserItem user = new SystemUserItem();
				user.Domain = domain;
				user.Name = userName;
				user.FullName = userName;
				user.Description = description;
				user.MemberOf = memberOf;
				user.Password = password;
				user.PasswordCantChange = true;
				user.PasswordNeverExpires = true;
				user.AccountDisabled = false;
				user.System = true;
				SecurityUtils.CreateUser(user);
				
				//add rollback action
				RollBack.RegisterUserAccountAction(domain, userName);
				
				// update config setings
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewUserAccount", true);
				AppConfig.SetComponentSettingStringValue(componentId, "UserAccount", userName);
				AppConfig.SetComponentSettingStringValue(componentId, "Domain", domain);
				
				//update log
				Log.WriteEnd("Created windows user account");				
				
				//update install log
				if ( string.IsNullOrEmpty(domain))
					InstallLog.AppendLine(string.Format("- Created a new windows user account \"{0}\"", userName));
				else
					InstallLog.AppendLine(string.Format("- Created a new windows user account \"{0}\" in \"{1}\" domain", userName, domain));
			}
			else
			{
				throw new Exception("Account already exists");
			}
		}

		private void CreateAppPool(string name, string userName, string userPassword)
		{
			SetProgressText("Creating local account...");
			string componentId = Wizard.SetupVariables.ComponentId;
			Version iisVersion = Wizard.SetupVariables.IISVersion;
            bool poolExists = (iisVersion.Major >= 7) ? 
				WebUtils.IIS7ApplicationPoolExists(name) :
				WebUtils.ApplicationPoolExists(name);
		
			if (poolExists)
			{
				//update app pool
				Log.WriteStart("Updating application pool");
				Log.WriteInfo(string.Format("Updating application pool \"{0}\"", name));
                if (iisVersion.Major >= 7)
					WebUtils.UpdateIIS7ApplicationPool(name, userName, userPassword);
				else
					WebUtils.UpdateApplicationPool(name, userName, userPassword);

				// update config setings
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewApplicationPool", false);
				AppConfig.SetComponentSettingStringValue(componentId, "ApplicationPool", name);
				
				//update log
				Log.WriteEnd("Updated application pool");
				
				//update install log
				InstallLog.AppendLine(string.Format("- Updated application pool named \"{0}\"", name));
			}
			else
			{
				// create app pool
				Log.WriteStart("Creating application pool");
				Log.WriteInfo(string.Format("Creating application pool \"{0}\"", name));
                if (iisVersion.Major >= 7)
					WebUtils.CreateIIS7ApplicationPool(name, userName, userPassword);
				else
					WebUtils.CreateApplicationPool(name, userName, userPassword);

				//register rollback action
                if (iisVersion.Major >= 7)
					RollBack.RegisterIIS7ApplicationPool(name);
				else
					RollBack.RegisterApplicationPool(name);
				

				// update config setings
				AppConfig.SetComponentSettingBooleanValue(componentId, "NewApplicationPool", true);
				AppConfig.SetComponentSettingStringValue(componentId, "ApplicationPool", name);

				//update log
				Log.WriteEnd("Created application pool");

				//update install log
				InstallLog.AppendLine(string.Format("- Created a new application pool named \"{0}\"", name));
			}
		}

		/// <summary>
		/// Returns the list of all possible application URLs
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="domain"></param>
		/// <param name="port"></param>
		/// <param name="virtualDir"></param>
		/// <returns></returns>
		protected string[] GetApplicationUrls(string ip, string domain, string port, string virtualDir)
		{
			List<string> urls = new List<string>();

			// IP address, [port] and [virtualDir]
			string url = ip;
			if (String.IsNullOrEmpty(domain))
			{
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			// domain, [port] and [virtualDir]
			if (!String.IsNullOrEmpty(domain))
			{
				url = domain;
				if (!String.IsNullOrEmpty(port) && port != "80")
					url += ":" + port;
				if (!String.IsNullOrEmpty(virtualDir))
					url += "/" + virtualDir;
				urls.Add(url);
			}

			// add "localhost" by default
			/*
			url = "localhost";
			if(port != "" && port != "80" && port != null)
				url += ":" + port;
			if(virtualDir != "" && virtualDir != null)
				url += "/" + virtualDir;
			urls.Add(url);
			 * */

			return urls.ToArray();
		}

		private void UpdateSystemConfiguration()
		{
			try
			{
				Log.WriteStart("Updating system configuration");
				string componentId = Wizard.SetupVariables.ComponentId;
				if (Wizard.SetupVariables.SetupAction == SetupActions.Update)
				{
					//update settings
					AppConfig.SetComponentSettingStringValue(componentId, "Release", Wizard.SetupVariables.UpdateVersion);
					AppConfig.SetComponentSettingStringValue(componentId, "Installer", Wizard.SetupVariables.Installer);
					AppConfig.SetComponentSettingStringValue(componentId, "InstallerType", Wizard.SetupVariables.InstallerType);
					AppConfig.SetComponentSettingStringValue(componentId, "InstallerPath", Wizard.SetupVariables.InstallerPath);
				}

				Log.WriteInfo("Saving system configuration");
				//save
				AppConfig.SaveConfiguration();
				Log.WriteEnd("Updated system configuration");
				InstallLog.AppendLine("- Updated system configuration");

				if (Wizard.SetupVariables.SetupAction == SetupActions.Install)
				{
					RollBack.RegisterConfigAction(Wizard.SetupVariables.ComponentId, Wizard.SetupVariables.ComponentName);
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Config error", ex);
				throw;
			}			
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			if (this.Wizard != null)
			{
				this.Wizard.Cancel += new EventHandler(OnWizardCancel);
			}
			Form parentForm = FindForm();
			parentForm.FormClosing += new FormClosingEventHandler(OnFormClosing);
		}

		void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			AbortProcess();
		}

		private void OnWizardCancel(object sender, EventArgs e)
		{
			AbortProcess();
			this.CustomCancelHandler = false;
			Wizard.Close();
		}

		private void AbortProcess()
		{
			if (this.thread != null)
			{
				if (this.thread.IsAlive)
				{
					this.thread.Abort();
				}
				this.thread.Join();
			}
		}

		private void UpdateSqlScript(string file)
		{
			// get ES crypto key from the registry
			string cryptoKey = Wizard.SetupVariables.CryptoKey;
			if (String.IsNullOrEmpty(cryptoKey))
				return;

			Log.WriteStart("Updating SQL script file");
			Log.WriteInfo(string.Format("Updating SQL script file \"{0}\"", file));

			//update 'Users' table
			string text = GetServerAdminPasswordScript(cryptoKey);

			using (StreamWriter sw = File.AppendText(file))
			{
				sw.Write(text);
			}
			Log.WriteEnd("Updated SQL script file");
		}

		private string GetServerAdminPasswordScript(string cryptoKey)
		{
			// encrypt default password
			string password = Utils.Encrypt(cryptoKey, "serveradmin");

			// build script
			StringBuilder sb = new StringBuilder();

			sb.Append("\n\nIF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[ResourceGroupDnsRecords]') AND type in (N'U'))\n");
			sb.Append("BEGIN\n\n");

			sb.Append("\nUPDATE Users SET Password = '");
			sb.Append(password).Append("' WHERE Username = 'serveradmin'\n");

			sb.Append("END\nGO\n\n");

			return sb.ToString();
		}

		/// <summary>
		/// Runs sql script.
		/// </summary>
		/// <param name="connectionString">Sql server connection string.</param>
		/// <param name="database">Sql server database name.</param>
		/// <param name="fileName">Sql script file</param>
		private void RunSqlScript(string connectionString, string database, string fileName)
		{
			if (!File.Exists(fileName))
			{
				Log.WriteInfo(string.Format("File {0} not found", fileName));
				return;
			}

			Log.WriteStart("Installing database objects");

			//showing process
			SqlProcess process = new SqlProcess(fileName, connectionString, database);
			// Update progress change
			process.ProgressChange += new EventHandler<ActionProgressEventArgs<int>>((object sender, ActionProgressEventArgs<int> e) =>
			{
				this.progressBar.Value = e.EventData;
			});
			//
			process.Run();

			Log.WriteEnd("Installed database objects");
		}
		/// <summary>
		/// Add custom error page to the web.config (Microsoft Security Advisory (2416728))
		/// </summary>
		private void AddCustomErrorsPage()
		{
			try
			{
				string webConfigPath = Path.Combine(Wizard.SetupVariables.InstallationFolder, "web.config");
				Log.WriteStart("Web.config file is being updated");
				// Ensure the web.config exists
				if (!File.Exists(webConfigPath))
				{
					Log.WriteInfo(string.Format("File {0} not found", webConfigPath));
					return;
				}
				// Load web.config
				XmlDocument doc = new XmlDocument();
				doc.Load(webConfigPath);

				// replace existing node:
				// <system.web>
				//	 <customErrors mode="Off" />
				// </system.web>
				// with:
				// <system.web>
				//	 <customErrors mode="RemoteOnly" defaultRedirect="~/error.htm" />
				// </system.web>
				//
				XmlElement customErrors = doc.SelectSingleNode("configuration/system.web/customErrors[@mode='Off']") as XmlElement;
				// ensure node is found
				if (customErrors != null)
				{
					XmlUtils.SetXmlAttribute(customErrors, "mode", "RemoteOnly");
					XmlUtils.SetXmlAttribute(customErrors, "defaultRedirect", "~/error.htm");
				}
				// save changes have been made
				doc.Save(webConfigPath);
				//
				Log.WriteEnd("Web.config has been updated");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Could not update web.config file", ex);
				throw;
			}
		}
	}
}
