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
using System.Text;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using Microsoft.Win32;

namespace SolidCP.Providers.Statistics
{
    public class AWStats : HostingServiceProviderBase, IStatisticsServer
    {
        #region Properties
        protected string AwStatsFolder
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["AwStatsFolder"]); }
        }

        protected string ConfigFileName
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ConfigFileName"]); }
        }

        protected string ConfigFileTemplate
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ConfigFileTemplate"]); }
        }

        protected string ConfigFileTemplatePath
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["ConfigFileTemplatePath"]); }
        }

        protected string BatchFileName
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["BatchFileName"]); }
        }

        protected string BatchLineTemplate
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["BatchLineTemplate"]); }
        }

		protected string StatisticsUrl
		{
			get { return ProviderSettings["StatisticsUrl"]; }
		}
        #endregion

        #region IStatisticsServer methods
        public virtual StatsServer[] GetServers()
        {
            return new StatsServer[] { };
        }

        public virtual string[] GetSites()
        {
            List<string> sites = new List<string>();
            // check for AWStats folder existance
            if (!Directory.Exists(AwStatsFolder))
                return sites.ToArray();

            string configFileName = ConfigFileName.ToLower();
            int idx = configFileName.IndexOf("[domain_name]");
            if (idx == -1)
                return sites.ToArray(); // wrong config file name pattern

            string configPrefix = configFileName.Substring(0, idx);
            string configSuffix = configFileName.Substring(idx + 13);

            // get all files in AWStats directory
            string[] files = Directory.GetFiles(AwStatsFolder,
                ConfigFileName.ToLower().Replace("[domain_name]", "*"));

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string site = fileName.Substring(configPrefix.Length,
                    fileName.Length - configPrefix.Length - configSuffix.Length);
                sites.Add(site);
            }
            return sites.ToArray();
        }

        public virtual string GetSiteId(string siteName)
        {
            return siteName;
        }

        public virtual StatsSite GetSite(string siteId)
        {
            string configFileName = ConfigFileName.Replace("[DOMAIN_NAME]", siteId);
            string configFilePath = Path.Combine(AwStatsFolder, configFileName);

            if (!File.Exists(configFilePath))
                return null;

            StatsSite site = new StatsSite();
            site.Name = siteId;
            site.SiteId = siteId;

			// process stats URL
			string url = null;
			if (!String.IsNullOrEmpty(StatisticsUrl))
			{
				url = StringUtils.ReplaceStringVariable(StatisticsUrl, "domain_name", site.Name);
				url = StringUtils.ReplaceStringVariable(url, "site_id", siteId);
			}

			site.StatisticsUrl = url;

            return site;
        }

        public virtual string AddSite(StatsSite site)
        {
                // check for AWStats folder existance
                string awFolder = AwStatsFolder;
                if (!Directory.Exists(awFolder))
                {
                    // try to create directory
                    Directory.CreateDirectory(awFolder);
                }

                // create a new configuration file
                string configFileName = ConfigFileName;

                // ...and substitute variables
                configFileName = configFileName.Replace("[DOMAIN_NAME]", site.Name);
                string configFilePath = Path.Combine(awFolder, configFileName);

                // check if the file already exists
                if (File.Exists(configFilePath))
                {
                    return site.Name; // nothing to create
                }

                // get config file template
                string configFileTemplate = ConfigFileTemplate;
                if (!String.IsNullOrEmpty(ConfigFileTemplatePath)
                    && File.Exists(ConfigFileTemplatePath))
                {
                    // read template from file
                    StreamReader reader = new StreamReader(ConfigFileTemplatePath);
                    configFileTemplate = reader.ReadToEnd();
                    reader.Close();
                }

                // ...and substitute variables
                configFileTemplate = configFileTemplate.Replace("[DOMAIN_NAME]", site.Name);
                configFileTemplate = site.DomainAliases.Length == 0 ? configFileTemplate.Replace("[DOMAIN_ALIASES]", "localhost 127.0.0.1") : configFileTemplate.Replace("[DOMAIN_ALIASES]", String.Join(" ", site.DomainAliases));
                configFileTemplate = configFileTemplate.Replace("[LOGS_FOLDER]", site.LogDirectory);

                // create config file
                StreamWriter writer = new StreamWriter(configFilePath);
                writer.Write(configFileTemplate);
                writer.Close();

                // add line to the batch file
                string batchFilePath = Path.Combine(awFolder, BatchFileName);

                // create file if not exists
                if (!File.Exists(batchFilePath))
                {
                    writer = new StreamWriter(batchFilePath);
                    writer.Close();
                }

                // read batch file
                List<string> lines = LoadBatchFile(batchFilePath);

                // check if the record is already added
                bool exists = false;
                foreach (string line in lines)
                {
                    if (line.IndexOf("=" + site.Name) != -1)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    // add new line to the batch
                    string line = BatchLineTemplate;
                    line = line.Replace("[DOMAIN_NAME]", site.Name);

                    lines.Add(line);

                    // save batch file
                    SaveBatchFile(batchFilePath, lines);
                }

                return site.Name;
        }

        public virtual void UpdateSite(StatsSite site)
        {
            // nope
        }

        public virtual void DeleteSite(string siteId)
        {
            // check for AWStats folder existance
            if (!Directory.Exists(AwStatsFolder))
            {
                return;
            }

            // create a new configuration file
            string configFileName = ConfigFileName;

            // ...and substitute variables
            configFileName = configFileName.Replace("[DOMAIN_NAME]", siteId);
            string configFilePath = Path.Combine(AwStatsFolder, configFileName);

            // check if the config file already exists
            if (File.Exists(configFilePath))
                File.Delete(configFilePath);

            // remove line from the batch file
            string batchFilePath = Path.Combine(AwStatsFolder, BatchFileName);

            // create file if not exists
            if (!File.Exists(batchFilePath))
                return;

            // read batch file
            List<string> lines = LoadBatchFile(batchFilePath);

            // check if the record is already added
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].IndexOf("=" + siteId) != -1)
                {
                    // remove line accurence
                    lines.RemoveAt(i);

                    // save batch
                    SaveBatchFile(batchFilePath, lines);
                    break;
                }
            }
        }

        private List<string> LoadBatchFile(string fileName)
        {
            List<string> lines = new List<string>();

            StreamReader reader = new StreamReader(fileName);
            string line = null;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);
            reader.Close();

            return lines;
        }

        private void SaveBatchFile(string fileName, List<string> lines)
        {
            StreamWriter writer = new StreamWriter(fileName);
            foreach (string line in lines)
                writer.WriteLine(line);
            writer.Close();
        }

        #endregion

        #region IHostingServiceProvider methods
        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is StatsSite)
                {
                    try
                    {
                        DeleteSite(((StatsSite)item).SiteId);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                    }
                }
            }
        }
        #endregion

        public override bool IsInstalled()
        {
            string versionNumber = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\AWStats");

            if (key != null)
            {
                versionNumber = (string)key.GetValue("DisplayVersion");
            }
            else
            {
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\AWStats");
                if (key != null)
                {
                    versionNumber = (string)key.GetValue("DisplayVersion");
                }
                else
                {
                    return false;
                }
            }

            string[] split = versionNumber.Split(new char[] { '.' });

            return split[0].Equals("6") || split[0].Equals("7");
        }

    }
}
