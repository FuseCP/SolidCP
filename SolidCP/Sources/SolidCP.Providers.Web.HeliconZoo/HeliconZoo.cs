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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;
using SolidCP.Providers;
using SolidCP.Providers.HeliconZoo;



namespace SolidCP.Providers.Web.HeliconZoo
{
    public class HeliconZoo : HostingServiceProviderBase, IHeliconZooServer
    {
        const string ZooInstalledRegistryKey = @"SOFTWARE\Helicon\Zoo";
        const string ZooInstalledRegistryKeyWow6432 = @"SOFTWARE\Wow6432Node\Helicon\Zoo";
        private const int ZooCompatibleBuild = 52;

        public override bool IsInstalled()
        {
            // always return true;
            return true;

            /*
            RegistryKey HKLM = Registry.LocalMachine;
            string fullVersion = string.Empty;
            RegistryKey key = HKLM.OpenSubKey(ZooInstalledRegistryKey);
            if (key != null)
            {
                fullVersion = key.GetValue("Version", null) as string;
            }
            else
            {
                key = HKLM.OpenSubKey(ZooInstalledRegistryKeyWow6432);
                if (key != null)
                {
                    fullVersion = key.GetValue("Version", null) as string;
                }
            }

            if (string.IsNullOrEmpty(fullVersion))
            {
                return false;
            }

            Match match = Regex.Match(fullVersion, @"\.(\d+)$");
            if (match.Success)
            {
                int version = int.Parse(match.Groups[1].ToString());
                if (version >= ZooCompatibleBuild)
                {
                    return true;
                }
            }

            return false;
            */
        }

        public HeliconZooEngine[] GetEngines()
        {
            // Read applicationHost.config

            List<HeliconZooEngine> result = new List<HeliconZooEngine>();

            using (var srvman = new ServerManager())
            {
                Configuration appConfig = srvman.GetApplicationHostConfiguration();

                ConfigurationSection heliconZooServer;
                try
                {
                    heliconZooServer = appConfig.GetSection("system.webServer/heliconZooServer");
                }
                catch(Exception)
                {
                    // heliconZooServer is not found
                    // looks like zoo is not installed
                    return result.ToArray();
                }

                ConfigurationElement engines = heliconZooServer.GetChildElement("engines");
                ConfigurationElementCollection enginesCollection = engines.GetCollection();

                //switchboard
                ConfigurationElement switchboard = heliconZooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();


                bool switchboardDisabledDefault = true;
                foreach (ConfigurationElement switchboardElement in switchboardCollection)
                {
                    if ((string)switchboardElement.GetAttributeValue("name") == "*")
                    {
                        bool isEnabled = GetSwitchBoardValue(switchboardElement);

                        switchboardDisabledDefault =  !isEnabled ;
                        break;
                    }
                }

                //main engines

                foreach (ConfigurationElement item in enginesCollection)
                {
                    HeliconZooEngine newItem = ConvertElementToHeliconZooEngine(item);
                    newItem.disabled = switchboardDisabledDefault;
                    result.Add(newItem);
                }


                //userEngines

                ConfigurationElement userEngines = heliconZooServer.GetChildElement("userEngines");
                ConfigurationElementCollection userEnginesCollection = userEngines.GetCollection();
                foreach (ConfigurationElement item in userEnginesCollection)
                {
                    HeliconZooEngine newItem = ConvertElementToHeliconZooEngine(item);

                    //remove if exists
                    HeliconZooEngine serverItem = Collection_GetHeliconZooEngineByName(result, newItem.name);
                    if (serverItem != null)
                    {
                        result.Remove(serverItem);
                    }

                    //override settings
                    newItem.isUserEngine = true;
                    newItem.disabled = switchboardDisabledDefault;
                    result.Add(newItem);
                }


                //Web console
                HeliconZooEngine webConsole = new HeliconZooEngine
                    {
                        displayName = "Web console", 
                        name = "console"
                        
                    };

                result.Add(webConsole);

              

                foreach (ConfigurationElement switchboardElement in switchboardCollection)
                {
                    HeliconZooEngine item = Collection_GetHeliconZooEngineByName(result, (string)switchboardElement.GetAttributeValue("name"));
                    if (item != null)
                    {
                        bool isEnabled = GetSwitchBoardValue(switchboardElement);
                        item.disabled = !isEnabled;
                    }
                    else
                    {
                       //default value
                        //item.disabled = !switchboardEnabledDefaults;
                    }
                    
                }


            }

            return result.ToArray();
        }

        private bool GetSwitchBoardValue(ConfigurationElement switchboardElement)
        {
            return (0 == String.Compare((string)switchboardElement.GetAttributeValue("value"),
                                                                 "Enabled", StringComparison.OrdinalIgnoreCase));
        }
        
        private void SetSwitchBoardValue(ConfigurationElement switchboardElement, bool enabled)
        {
            switchboardElement.SetAttributeValue("value", enabled ? "Enabled" : "Disabled");
        }

        HeliconZooEngine Collection_GetHeliconZooEngineByName(List<HeliconZooEngine> collection, string name)
        {
            foreach (HeliconZooEngine r in collection)
            {
                if (r.name == name)
                {
                    return r;
                }
            }

            return null;
        }


        public void SetEngines(HeliconZooEngine[] userEngines)
        {
            // Write to applicationHost.config

            using (var srvman = new ServerManager())
            {

                Configuration appConfig = srvman.GetApplicationHostConfiguration();

                
                ConfigurationSection heliconZooServer = appConfig.GetSection("system.webServer/heliconZooServer");
                ConfigurationElement engines = heliconZooServer.GetChildElement("userEngines");
                ConfigurationElementCollection enginesCollection = engines.GetCollection();
                enginesCollection.Clear();


                ConfigurationElement switchboard = heliconZooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();
                switchboardCollection.Clear();
                
                
              
                foreach(HeliconZooEngine item in userEngines)
                {
                    if (item.isUserEngine)
                    {
                        ConfigurationElement engine = enginesCollection.CreateElement();
                        ConvertHeliconZooEngineToElement(item, engine);
                        enginesCollection.Add(engine);
                    }

                
                }

                srvman.CommitChanges();
            }
        }

        public bool IsEnginesEnabled()
        {
            bool isEnginesEnabled = true;

            using (var srvman = new ServerManager())
            {
                Configuration appConfig = srvman.GetApplicationHostConfiguration();

                ConfigurationSection heliconZooServer = appConfig.GetSection("system.webServer/heliconZooServer");

                //switchboard
                ConfigurationElement switchboard = heliconZooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();


                foreach (ConfigurationElement switchboardElement in switchboardCollection)
                {
                    if ((string) switchboardElement.GetAttributeValue("name") == "*")
                    {
                        isEnginesEnabled = GetSwitchBoardValue(switchboardElement);
                        break;
                    }
                }
            }

            return isEnginesEnabled;
        }

        public void SwithEnginesEnabled(bool enabled)
        {
            using (var srvman = new ServerManager())
            {
                Configuration appConfig = srvman.GetApplicationHostConfiguration();

                ConfigurationSection heliconZooServer = appConfig.GetSection("system.webServer/heliconZooServer");

                ConfigurationElement switchboard = heliconZooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();

                bool wildCardFound = false;
                foreach (ConfigurationElement switchboardElement in switchboardCollection)
                {
                    if ((string)switchboardElement.GetAttributeValue("name") == "*")
                    {
                        switchboardElement.SetAttributeValue("value", enabled ? "Enabed" : "Disabled");
                        wildCardFound = true;
                        break;
                    }
                }

                if (!wildCardFound)
                {
                    ConfigurationElement element = switchboardCollection.CreateElement();
                    element.SetAttributeValue("name", "*");
                    SetSwitchBoardValue(element, enabled);
                    switchboardCollection.Add(element);
                }

                srvman.CommitChanges();
            }
        }

        public string[] GetEnabledEnginesForSite(string siteId)
        {
            if (string.IsNullOrEmpty(siteId))
            {
                return new string[0];
            }

            List<string> engines = new List<string>();

            using (var srvman = new ServerManager())
            {
                Configuration appConfig = srvman.GetApplicationHostConfiguration();
                ConfigurationSection zooServer;
                try
                {
                    zooServer = appConfig.GetSection("system.webServer/heliconZooServer", siteId);
                }
                catch(Exception)
                {
                    // heliconZooServer is not found
                    // looks like zoo is not installed
                    return engines.ToArray();

                }
                ConfigurationElement switchboard = zooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();

                foreach (ConfigurationElement element in switchboardCollection)
                {
                    
                    bool isEnabled = GetSwitchBoardValue(element);
                    if (isEnabled)
                    {
                        engines.Add(element.GetAttributeValue("name").ToString());
                    }
                }
            }

            return engines.ToArray();
        }

        public void SetEnabledEnginesForSite(string siteId, string[] engineNames)
        {
            if (string.IsNullOrEmpty(siteId))
            {
                return;
            }

            using (var srvman = new ServerManager())
            {
                Configuration appConfig = srvman.GetApplicationHostConfiguration();
                ConfigurationSection zooServer = appConfig.GetSection("system.webServer/heliconZooServer", siteId);
                ConfigurationElement switchboard = zooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();

                switchboardCollection.Clear();

                // first disable all engines if "*" is not present in input engineNames
                if (!engineNames.Contains("*"))
                {
                    ConfigurationElement elementDisableAll = switchboardCollection.CreateElement();
                    elementDisableAll.SetAttributeValue("name", "*");
                    SetSwitchBoardValue(elementDisableAll, false);
                    switchboardCollection.Add(elementDisableAll);
                }

                foreach (string engineName in engineNames)
                {
                    ConfigurationElement element = switchboardCollection.CreateElement();
                    element.SetAttributeValue("name", engineName);
                    SetSwitchBoardValue(element, true);
                    switchboardCollection.Add(element);
                }

                RegisterZooPhpHandlers(siteId, engineNames, appConfig);

                srvman.CommitChanges();
            }

        }


        public bool IsWebCosoleEnabled()
        {
            bool isEnginesEnabled = true;

            using (var srvman = new ServerManager())
            {
                Configuration appConfig = srvman.GetApplicationHostConfiguration();

                ConfigurationSection heliconZooServer = appConfig.GetSection("system.webServer/heliconZooServer");

                //switchboard
                ConfigurationElement switchboard = heliconZooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();


                foreach (ConfigurationElement switchboardElement in switchboardCollection)
                {
                    if ((string)switchboardElement.GetAttributeValue("name") == "console")
                    {
                        isEnginesEnabled = GetSwitchBoardValue(switchboardElement);
                        break;
                    }
                }
            }

            return isEnginesEnabled;
        }

        public void SetWebCosoleEnabled(bool enabled)
        {
            using (var srvman = new ServerManager())
            {
                Configuration appConfig = srvman.GetApplicationHostConfiguration();

                ConfigurationSection heliconZooServer = appConfig.GetSection("system.webServer/heliconZooServer");

                ConfigurationElement switchboard = heliconZooServer.GetChildElement("switchboard");
                ConfigurationElementCollection switchboardCollection = switchboard.GetCollection();

                bool found = false;
                foreach (ConfigurationElement switchboardElement in switchboardCollection)
                {
                    if ((string)switchboardElement.GetAttributeValue("name") == "console")
                    {
                        SetSwitchBoardValue(switchboardElement, enabled);
                        
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ConfigurationElement element = switchboardCollection.CreateElement();
                    element.SetAttributeValue("name", "console");
                    SetSwitchBoardValue(element, enabled);
                    switchboardCollection.Add(element);
                }

                srvman.CommitChanges();
            }
        }

      

        #region private methods

        private void ConvertHeliconZooEngineToElement(HeliconZooEngine item, ConfigurationElement engine)
        {
            engine.SetAttributeValue("name", item.name);
            engine.SetAttributeValue("displayName", item.displayName);
            engine.SetAttributeValue("arguments", item.arguments);
            engine.SetAttributeValue("fullPath", item.fullPath);
            engine.SetAttributeValue("arguments", item.arguments);
            engine.SetAttributeValue("transport", item.transport);
            engine.SetAttributeValue("protocol", item.protocol);
            engine.SetAttributeValue("host", item.host);

            engine.SetAttributeValue("portLower", item.portLower);
            engine.SetAttributeValue("portUpper", item.portUpper);
            engine.SetAttributeValue("maxInstances", item.maxInstances);
            engine.SetAttributeValue("minInstances", item.minInstances);
            engine.SetAttributeValue("timeLimit", item.timeLimit);
            engine.SetAttributeValue("gracefulShutdownTimeout", item.gracefulShutdownTimeout);
            engine.SetAttributeValue("memoryLimit", item.memoryLimit);


            ConfigurationElementCollection envColl = engine.GetChildElement("environmentVariables").GetCollection();
            

            foreach(HeliconZooEnv env in item.environmentVariables)
            {
                ConfigurationElement envElement = envColl.CreateElement();
                envElement.SetAttributeValue("name", env.Name);
                envElement.SetAttributeValue("value", env.Value);
                envColl.Add(envElement);
            }
            
            
        }

        private HeliconZooEngine ConvertElementToHeliconZooEngine(ConfigurationElement item)
        {
            HeliconZooEngine result = new HeliconZooEngine();

            result.name = (string)item.GetAttributeValue("name");
            result.displayName = (string)item.GetAttributeValue("displayName");
            result.arguments = (string)item.GetAttributeValue("arguments");
            result.fullPath = (string)item.GetAttributeValue("fullPath");
            result.arguments = (string)item.GetAttributeValue("arguments");
            result.transport = (string)item.GetAttributeValue("transport");
            result.protocol = (string)item.GetAttributeValue("protocol");
            result.host = (string)item.GetAttributeValue("host");
            
            result.portLower = (long) item.GetAttributeValue("portLower");
            result.portUpper = (long) item.GetAttributeValue("portUpper");
            result.maxInstances = (long) item.GetAttributeValue("maxInstances");
            result.minInstances = (long) item.GetAttributeValue("minInstances");
            result.timeLimit = (long) item.GetAttributeValue("timeLimit");
            result.gracefulShutdownTimeout = (long) item.GetAttributeValue("gracefulShutdownTimeout");
            result.memoryLimit = (long) item.GetAttributeValue("memoryLimit");

            List<HeliconZooEnv> envList = new List<HeliconZooEnv>();
            ConfigurationElementCollection envColl = item.GetChildElement("environmentVariables").GetCollection();
            foreach (ConfigurationElement el in envColl)
            {
                envList.Add(ConvertElementToHeliconZooEnv(el));
            }
            result.environmentVariables = envList.ToArray();

            // TODO: fix this
            result.isUserEngine = false;

            // TODO: disabled

            return result;
            
        }

        private HeliconZooEnv ConvertElementToHeliconZooEnv(ConfigurationElement item)
        {
            HeliconZooEnv result = new HeliconZooEnv();
            result.Name = (string)item.GetAttributeValue("name");
            result.Value = (string)item.GetAttributeValue("value");
            return result;
        }

        private static void RegisterZooPhpHandlers(string siteId, string[] engineNames, Configuration appConfig)
        {
            // set up zoo php handler if php engine was enabled
            string enabledPhpEngine = string.Empty;
            foreach (string engineName in engineNames)
            {
                if (engineName.StartsWith("php", StringComparison.OrdinalIgnoreCase))
                {
                    enabledPhpEngine = engineName;
                }
            }

            if (!string.IsNullOrEmpty(enabledPhpEngine))
            {
                ConfigurationSection handlers = appConfig.GetSection("system.webServer/handlers", siteId);
                ConfigurationElementCollection handlerCollection = handlers.GetCollection();

                // remove native php handlers
                /*
                    ConfigurationElement removePhp53 = handlerCollection.CreateElement("remove");
                    removePhp53.SetAttributeValue("name", "PHP53_via_FastCGI");
                    handlerCollection.Add(removePhp53);

                    ConfigurationElement removePhp = handlerCollection.CreateElement("remove");
                    removePhp.SetAttributeValue("name", "PHP_via_FastCGI");
                    handlerCollection.Add(removePhp);
                    */

                // search native php handlers
                /*
                    List<ConfigurationElement> elementsToRemove = new List<ConfigurationElement>();
                    foreach (ConfigurationElement el in handlerCollection)
                    {
                        string name = el.GetAttributeValue("name") as string;
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (string.Equals(name, "PHP_via_FastCGI", StringComparison.OrdinalIgnoreCase)
                                ||
                                string.Equals(name, "PHP53_via_FastCGI", StringComparison.OrdinalIgnoreCase)
                            )
                            {
                                elementsToRemove.Add(el);
                            }
                        }
                    }

                    foreach (ConfigurationElement element in elementsToRemove)
                    {
                        //handlerCollection.Remove(element);
                    }
                    */


                // check zoo handlers exists
                List<ConfigurationElement> zooPhpHandlersToRemove = new List<ConfigurationElement>();
                foreach (ConfigurationElement el in handlerCollection)
                {
                    string name = el.GetAttributeValue("name") as string;
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (name.StartsWith("php.", StringComparison.OrdinalIgnoreCase))
                        {
                            string scriptProcessor = el.GetAttributeValue("scriptProcessor") as string;
                            if (!string.IsNullOrEmpty(scriptProcessor))
                            {
                                zooPhpHandlersToRemove.Add(el);
                            }
                        }
                    }
                }

                // remove existing zoo php handlers
                foreach (ConfigurationElement element in zooPhpHandlersToRemove)
                {
                    handlerCollection.Remove(element);
                }

                // add zoo php handlers
                ConfigurationElement zooPhpX86 = handlerCollection.CreateElement();
                zooPhpX86.SetAttributeValue("name", "php.pipe#x86");
                zooPhpX86.SetAttributeValue("scriptProcessor", enabledPhpEngine);
                zooPhpX86.SetAttributeValue("path", "*.php");
                zooPhpX86.SetAttributeValue("verb", "*");
                zooPhpX86.SetAttributeValue("modules", "HeliconZoo_x86");
                zooPhpX86.SetAttributeValue("preCondition", "bitness32");
                zooPhpX86.SetAttributeValue("resourceType", "Unspecified");
                zooPhpX86.SetAttributeValue("requireAccess", "Script");
                handlerCollection.AddAt(0, zooPhpX86);

                ConfigurationElement zooPhpX64 = handlerCollection.CreateElement();
                zooPhpX64.SetAttributeValue("name", "php.pipe#x64");
                zooPhpX64.SetAttributeValue("scriptProcessor", enabledPhpEngine);
                zooPhpX64.SetAttributeValue("path", "*.php");
                zooPhpX64.SetAttributeValue("verb", "*");
                zooPhpX64.SetAttributeValue("modules", "HeliconZoo_x64");
                zooPhpX64.SetAttributeValue("preCondition", "bitness64");
                zooPhpX64.SetAttributeValue("resourceType", "Unspecified");
                zooPhpX64.SetAttributeValue("requireAccess", "Script");
                handlerCollection.AddAt(1, zooPhpX64);

                // process index.php as default document
                ConfigurationSection defaultDocument = appConfig.GetSection("system.webServer/defaultDocument", siteId);
                RegisterPhpDefaultDocument(defaultDocument);
            }
        }

        private static void RegisterPhpDefaultDocument(ConfigurationSection defaultDocument)
        {
            ConfigurationElement defaultFiles = defaultDocument.GetChildElement("files");
            ConfigurationElementCollection filesCollection = defaultFiles.GetCollection();

            // search index.php in default documents
            bool indexPhpPresent = false;
            foreach (ConfigurationElement configurationElement in filesCollection)
            {
                string value = configurationElement.GetAttributeValue("value") as string;
                if (!string.IsNullOrEmpty(value))
                {
                    if (string.Equals(value, "index.php", StringComparison.OrdinalIgnoreCase))
                    {
                        indexPhpPresent = true;
                        break;
                    }
                }
            }

            if (!indexPhpPresent)
            {
                // add index.php
                ConfigurationElement indexPhp = filesCollection.CreateElement();
                indexPhp.SetAttributeValue("value", "index.php");
                filesCollection.AddAt(0, indexPhp);
            }
        }

        #endregion
    }
}
