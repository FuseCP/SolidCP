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
using System.Text;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SolidCP.Setup.Common;
using SolidCP.UniversalInstaller.Core;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.Setup.Actions
{
    public class SetWebPortalWebSettingsAction : Action, IPrepareDefaultsAction
    {
        public const string LogStartMessage = "Retrieving default IP address of the component...";

        void IPrepareDefaultsAction.Run(SetupVariables vars)
        {
            //
            if (String.IsNullOrEmpty(vars.WebSitePort))
                vars.WebSitePort = Global.WebPortal.DefaultPort;
            //
            if (String.IsNullOrEmpty(vars.UserAccount))
                vars.UserAccount = Global.WebPortal.ServiceAccount;

            // By default we use public ip for the component
            if (String.IsNullOrEmpty(vars.WebSiteIP))
            {
                var serverIPs = WebUtils.GetIPv4Addresses();
                //
                if (serverIPs != null && serverIPs.Length > 0)
                {
                    vars.WebSiteIP = serverIPs[0];
                }
                else
                {
                    vars.WebSiteIP = Global.LoopbackIPv4;
                }
            }
        }
    }

    public class UpdateEnterpriseServerUrlAction : Action, IInstallAction
    {
        public const string LogStartInstallMessage = "Updating site settings...";

        void IInstallAction.Run(SetupVariables vars)
        {
            try
            {
                Begin(LogStartInstallMessage);
                //
                Log.WriteStart(LogStartInstallMessage);
                //
                var path = Path.Combine(vars.InstallationFolder, @"App_Data\SiteSettings.config");
                //
                if (!File.Exists(path))
                {
                    Log.WriteInfo(String.Format("File {0} not found", path));
                    //
                    return;
                }
                //
                var doc = new XmlDocument();
                doc.Load(path);
                //
                var urlNode = doc.SelectSingleNode("SiteSettings/EnterpriseServer") as XmlElement;
                if (urlNode == null)
                {
                    Log.WriteInfo("EnterpriseServer setting not found");
                    return;
                }

                urlNode.InnerText = vars.EnterpriseServerURL;
                doc.Save(path);
                //
                Log.WriteEnd("Updated site settings");
                //
                InstallLog.AppendLine("- Updated site settings");
            }
            catch (Exception ex)
            {
                if (Utils.IsThreadAbortException(ex))
                    return;
                //
                Log.WriteError("Site settigs error", ex);
                //
                throw;
            }
        }
    }

    public class GenerateSessionValidationKeyAction : Action, IInstallAction
    {
        public const string LogStartInstallMessage = "Generating session validation key...";

        void IInstallAction.Run(SetupVariables vars)
        {
            try
            {
                Begin(LogStartInstallMessage);

                Log.WriteStart(LogStartInstallMessage);

                string path = Path.Combine(vars.InstallationFolder, "web.config");

                if (!File.Exists(path))
                {
                    Log.WriteInfo(string.Format("File {0} not found", path));
                    return;
                }

                Log.WriteStart("Updating configuration file (session validation key)");
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlElement sessionKey = doc.SelectSingleNode("configuration/appSettings/add[@key='SessionValidationKey']") as XmlElement;
                if (sessionKey == null)
                {
                    Log.WriteInfo("SessionValidationKey setting not found");
                    return;
                }

                sessionKey.SetAttribute("value", StringUtils.GenerateRandomString(16));
                doc.Save(path);

                Log.WriteEnd("Generated session validation key");
                InstallLog.AppendLine("- Generated session validation key");
            }
            catch (Exception ex)
            {
                if (Utils.IsThreadAbortException(ex))
                    return;
                //
                Log.WriteError("Site settigs error", ex);
                //
                throw;
            }
        }
    }

    public class CreateDesktopShortcutsAction : Action, IInstallAction
    {
        public const string LogStartInstallMessage = "Creating shortcut...";
        public const string ApplicationUrlNotFoundMessage = "Application url not found";
        public const string Path2 = "SolidCP Software";

        void IInstallAction.Run(SetupVariables vars)
        {
            //
            try
            {
                Begin(LogStartInstallMessage);
                //
                Log.WriteStart(LogStartInstallMessage);
                //
                var urls = Utils.GetApplicationUrls(vars.WebSiteIP, vars.WebSiteDomain, vars.WebSitePort, null);
                string url = null;

                if (urls.Length == 0)
                {
                    Log.WriteInfo(ApplicationUrlNotFoundMessage);
                    //
                    return;
                }
                // Retrieve top-most url from the list
                url = "http://" + urls[0];
                //
                Log.WriteStart("Creating menu shortcut");
                //
                string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                string fileName = "Login to SolidCP.url";
                string path = Path.Combine(programs, Path2);
                //
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //
                WriteShortcutData(Path.Combine(path, fileName), url);
                //
                Log.WriteEnd("Created menu shortcut");
                //
                Log.WriteStart("Creating desktop shortcut");
                //
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                WriteShortcutData(Path.Combine(desktop, fileName), url);
                //
                Log.WriteEnd("Created desktop shortcut");
                //
                InstallLog.AppendLine("- Created application shortcuts");
            }
            catch (Exception ex)
            {
                if (Utils.IsThreadAbortException(ex))
                    return;
                //
                Log.WriteError("Create shortcut error", ex);
            }
        }

        private static void WriteShortcutData(string filePath, string url)
        {
            string iconFile = Path.Combine(Environment.SystemDirectory, "url.dll");
            //
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine("[InternetShortcut]");
                sw.WriteLine("URL=" + url);
                sw.WriteLine("IconFile=" + iconFile);
                sw.WriteLine("IconIndex=0");
                sw.WriteLine("HotKey=0");
                //
                Log.WriteInfo(String.Format("Shortcut url: {0}", url));
            }
        }
    }

    public class CopyWebConfigAction : Action, IInstallAction
    {
        void IInstallAction.Run(SetupVariables vars)
        {
            try
            {
                Log.WriteStart("Copying web.config");
                string configPath = Path.Combine(vars.InstallationFolder, "web.config");
                string config6Path = Path.Combine(vars.InstallationFolder, "web6.config");

                bool iis6 = (vars.IISVersion.Major == 6);
                if (!File.Exists(config6Path))
                {
                    Log.WriteInfo(string.Format("File {0} not found", config6Path));
                    return;
                }

                if (iis6)
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
                //
                Log.WriteError("Copy web.config error", ex);
                //
                throw;
            }
        }
    }

    public class ConfigureEmbeddedEnterpriseServerAction : Action, IInstallAction
    {

        private void AddOrSetAppSettings(XElement appSettings, string key, string value, bool overwrite = true, string developValue = null)
        {
            var setting = appSettings.Elements().FirstOrDefault(e => e.Attribute("key")?.Value == key);
            if (setting == null)
            {
                setting = new XElement("add", new XAttribute("key", key), new XAttribute("value", value));
                appSettings.Add(setting);
            }
            else if (overwrite)
            {
                var valueAttribute = setting.Attribute("value");
                if (valueAttribute == null) setting.Add(new XAttribute("value", value));
                else if (developValue == null || valueAttribute.Value == developValue) valueAttribute.Value = value;
            }
        }

        void IInstallAction.Run(SetupVariables vars)
        {
            if (!vars.EmbedEnterpriseServer) return;
            try
            {
                Log.WriteStart("Configure embedded EnterpriseServer");
                string configPath = Path.Combine(vars.InstallationFolder, "web.config");

                if (!File.Exists(configPath))
                {
                    Log.WriteInfo(string.Format("File {0} not found", configPath));
                    return;
                }

                var probingPaths = string.Join(";", new string[] { "bin", "bin\\Code", "bin\\netstandard" }
                    .Select(path => Path.Combine(vars.EnterpriseServerPath, path)));
                var webConfig = XElement.Load(configPath);
                var appSettings = webConfig.Element("appSettings");
                if (appSettings == null)
                {
                    appSettings = new XElement("appSettings");
                    webConfig.Add(appSettings);
                }
                
                AddOrSetAppSettings(appSettings, "SolidCP.CryptoKey", Utils.GetRandomString(20), true, "1234567890");
                AddOrSetAppSettings(appSettings, "SolidCP.EncryptionEnabled", "true");
                AddOrSetAppSettings(appSettings, "SolidCP.EnterpriseServer.WebApplicationsPath", "~/WebApplications", true, "~/WebApplications");
                AddOrSetAppSettings(appSettings, "SolidCP.EnterpriseServer.ServerRequestTimeout", "3600", true, "3600");
                AddOrSetAppSettings(appSettings, "SolidCP.AltConnectionString", "ConnectionString", true, "ConnectionString");
                AddOrSetAppSettings(appSettings, "SolidCP.AltCryptoKey", "CryptoKey", true, "CryptoKey");
                AddOrSetAppSettings(appSettings, "ExternalProbingPaths", probingPaths, true);
                AddOrSetAppSettings(appSettings, "ExposeWebServices", vars.ExposeEnterpriseServerWebservices ? "EnterpriseServer" : "none", true);

                // Set connection string

                // Read EnterpriseServer web.config's connection string
                var enterpriseServerPath = vars.EnterpriseServerPath;
                var esWebConfigFile = Path.Combine(enterpriseServerPath, "web.config");
                var esWebConfig = XElement.Load(esWebConfigFile);
                var esConnectionString = esWebConfig
                    .Element("configuration")
                    .Element("connectionStrings")
                    ?.Elements("add")
                    .Where(e => (string)e.Attribute("name") == "EnterpriseServer")
                    .Select(e => (string)e.Attribute("connectionString"))
                    .FirstOrDefault();

                var connectionString = esConnectionString;
                var csb = new SolidCP.Providers.Common.ConnectionStringBuilder(connectionString);
                var dbType = csb["DbType"] as string;
                if (dbType.Equals("sqlite", StringComparison.OrdinalIgnoreCase) ||
                    dbType.Equals("sqlitefx", StringComparison.OrdinalIgnoreCase))
                {
                    // adjust SQLite data source
                    var database = csb["data source"] as string;
                    if (!Path.IsPathRooted(database))
                    {
                        if (!database.Contains(Path.DirectorySeparatorChar) && !database.Contains('.')) database = $"{database}.sqlite";
						database = Path.Combine(enterpriseServerPath, database);
                        connectionString = DatabaseUtils.BuildSqliteConnectionString(database);
                    }
                }

				var connectionStrings = webConfig
					.Element("configuration")
					?.Element("connectionStrings");
				var ConnNode = connectionStrings
					?.Elements("add")
					.FirstOrDefault(e => (string)e.Attribute("name") == "EnterpriseServer");
				if (ConnNode == null) connectionStrings.Add(ConnNode = new XElement("add", new XAttribute("name", "EnterpriseServer")));
				ConnNode.Attribute("connectionString").SetValue(connectionString);
				ConnNode.Attribute("providerName")?.Remove();
				webConfig.Save(configPath);
            }
            catch (Exception ex)
            {
                if (Utils.IsThreadAbortException(ex))
                    return;
                //
                Log.WriteError("Copy web.config error", ex);
                //
                throw;
            }
        }
    }

    public class WebPortalActionManager : BaseActionManager
    {
        public static readonly List<Action> InstallScenario = new List<Action>
        {
            new SetCommonDistributiveParamsAction(),
            new SetWebPortalWebSettingsAction(),
            new EnsureServiceAccntSecured(),
            new CopyFilesAction(),
            new CopyWebConfigAction(),
            new ConfigureEmbeddedEnterpriseServerAction(),
            new CreateWindowsAccountAction(),
            new ConfigureAspNetTempFolderPermissionsAction(),
            new SetNtfsPermissionsAction(),
            new CreateWebApplicationPoolAction(),
            new CreateWebSiteAction(),
            new InstallLetsEncryptCertificateAction(),
            new SwitchAppPoolAspNetVersion(),
            new UpdateEnterpriseServerUrlAction(),
            new GenerateSessionValidationKeyAction(),
            new SaveComponentConfigSettingsAction(),
            new CreateDesktopShortcutsAction()
        };

        public WebPortalActionManager(SetupVariables sessionVars)
            : base(sessionVars)
        {
            Initialize += new EventHandler(WebPortalActionManager_Initialize);
        }

        void WebPortalActionManager_Initialize(object sender, EventArgs e)
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
