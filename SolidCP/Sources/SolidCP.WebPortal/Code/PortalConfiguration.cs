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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace SolidCP.WebPortal
{
    /// <summary>
    /// Summary description for ModuleDefinitions
    /// </summary>
    public class PortalConfiguration
    {
        private const string ROLES_DELIMITERS = ";,";
        private const string APP_DATA_FOLDER = "~/App_Data";
        private const string THEMES_FOLDER = "~/App_Themes";
        private const string MODULES_PATTERN = "*_Modules.config";
        private const string PAGES_PATTERN = "*_Pages.config";
        private const string SITE_SETTINGS_FILE = "SiteSettings.config";
        private const string MODULE_DEFINITIONS_KEY = "PortalModuleDefinitionsCacheKey";
        private const string SITE_STRUCTURE_KEY = "SiteStructureCacheKey";
        private const string SITE_SETTINGS_KEY = "SiteSettingsCacheKey";

        public static Dictionary<string, ModuleDefinition> ModuleDefinitions
        {
            get
            {
                Dictionary<string, ModuleDefinition> modules =
                    (Dictionary<string, ModuleDefinition>)HttpContext.Current.Cache[MODULE_DEFINITIONS_KEY];
                if (modules == null)
                {
                    // create list
                    modules = new Dictionary<string, ModuleDefinition>();

                    // load modules
                    string appData = HttpContext.Current.Server.MapPath(APP_DATA_FOLDER);
                    FileInfo[] files = new DirectoryInfo(appData).GetFiles(MODULES_PATTERN);

                    foreach (FileInfo file in files)
                        LoadModulesFromXml(modules, file.FullName);

                    // place to cache
                    HttpContext.Current.Cache.Insert(MODULE_DEFINITIONS_KEY, modules,
                        new System.Web.Caching.CacheDependency(appData));

                }
                return modules;
            }
        }

        public static SiteStructure Site
        {
            get
            {
                SiteStructure site =
                    (SiteStructure)HttpContext.Current.Cache[SITE_STRUCTURE_KEY];
                if (site == null)
                {
                    // create list
                    site = new SiteStructure();

                    // load pages
                    string appData = HttpContext.Current.Server.MapPath(APP_DATA_FOLDER);
                    FileInfo[] files = new DirectoryInfo(appData).GetFiles(PAGES_PATTERN);

                    foreach (FileInfo file in files)
                        LoadPagesFromXml(site, file.FullName);

                    // place to cache
                    HttpContext.Current.Cache.Insert(SITE_STRUCTURE_KEY, site,
                        new System.Web.Caching.CacheDependency(appData));

                }
                return site;
            }
        }

        public static SiteSettings SiteSettings
        {
            get
            {
                SiteSettings settings =
                    (SiteSettings)HttpContext.Current.Cache[SITE_SETTINGS_KEY];
                if (settings == null)
                {
                    // create list
                    settings = new SiteSettings();

                    // load pages
                    string appData = HttpContext.Current.Server.MapPath(APP_DATA_FOLDER);
                    string path = Path.Combine(appData, SITE_SETTINGS_FILE);

                    // load site settings
                    XmlDocument xml = new XmlDocument();
                    xml.Load(path);

					XmlNodeList nodes = xml.SelectNodes("SiteSettings/*");

					foreach(XmlNode node in nodes)
					{
						settings[node.LocalName] = node.InnerText;
					}

                    // place to cache
                    HttpContext.Current.Cache.Insert(SITE_SETTINGS_KEY, settings,
                        new System.Web.Caching.CacheDependency(path));

                }
                return settings;
            }
        }

		public static bool SaveSiteSettings()
		{
			// load pages
			string appData = HttpContext.Current.Server.MapPath(APP_DATA_FOLDER);
			string path = Path.Combine(appData, SITE_SETTINGS_FILE);

			try
			{
				// build and save site settings
				XmlDocument xml = new XmlDocument();

				XmlElement root = xml.CreateElement("SiteSettings");
				xml.AppendChild(root);

				foreach (string keyName in SiteSettings.AllKeys)
				{
					XmlElement elem = xml.CreateElement(keyName);
					elem.InnerText = SiteSettings[keyName];

					root.AppendChild(elem);
				}

				xml.Save(path);
			}
			catch
			{
				return false;
			}

			return true;
		}

        private static void LoadModulesFromXml(Dictionary<string, ModuleDefinition> modules, string path)
        {
            // open xml document
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            // select nodes
            XmlNodeList xmlModules = xml.SelectNodes("ModuleDefinitions/ModuleDefinition");
            foreach (XmlNode xmlModule in xmlModules)
            {
                ModuleDefinition module = new ModuleDefinition();
                if (xmlModule.Attributes["id"] == null)
                    throw new Exception(String.Format("Module ID is not specified. File: {0}, Node: {1}",
                        path, xmlModule.OuterXml));

				module.Id = xmlModule.Attributes["id"].Value.ToLower(CultureInfo.InvariantCulture);
                modules.Add(module.Id, module);
                
                // controls
                XmlNodeList xmlControls = xmlModule.SelectNodes("Controls/Control");
                foreach (XmlNode xmlControl in xmlControls)
                {
                    ModuleControl control = new ModuleControl();
                    if (xmlControl.Attributes["icon"] != null)
                        control.IconFile = xmlControl.Attributes["icon"].Value;

                    if(xmlControl.Attributes["key"] != null)
						control.Key = xmlControl.Attributes["key"].Value.ToLower(CultureInfo.InvariantCulture);

                    if (xmlControl.Attributes["src"] == null)
                        throw new Exception(String.Format("Control 'src' is not specified. File: {0}, Node: {1}",
                            path, xmlControl.ParentNode.OuterXml));
                    control.Src = xmlControl.Attributes["src"].Value;

                    if (xmlControl.Attributes["title"] == null)
                        throw new Exception(String.Format("Control 'title' is not specified. File: {0}, Node: {1}",
                            path, xmlControl.ParentNode.OuterXml));
                    control.Title = xmlControl.Attributes["title"].Value;

                    if (xmlControl.Attributes["type"] != null)
                        control.ControlType = (ModuleControlType)Enum.Parse(typeof(ModuleControlType), xmlControl.Attributes["type"].Value, true);
                    else
                        control.ControlType = ModuleControlType.View;

                    if (String.IsNullOrEmpty(control.Key))
                        module.DefaultControl = control;

                    module.Controls.Add(control.Key, control);
                }
            }
        }

        private static void LoadPagesFromXml(SiteStructure site, string path)
        {
            // open xml document
            XmlDocument xml = new XmlDocument();
            xml.Load(path);

            // select nodes
            XmlNodeList xmlPages = xml.SelectNodes("Pages/Page");

            // process includes
            ProcessXmlIncludes(xml, path);

            // parse root nodes
            ParsePagesRecursively(path, site, xmlPages, null);
        }

        private static void ProcessXmlIncludes(XmlDocument xml, string parentPath)
        {
            // working dir
            string path = Path.GetDirectoryName(parentPath);

            // get all <includes>
            XmlNodeList nodes = xml.SelectNodes("//include");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["file"] == null)
                    continue;

                string incPath = Path.Combine(path, node.Attributes["file"].Value);
                XmlDocument inc = new XmlDocument();
                inc.Load(incPath);

                XmlElement incNode = xml.CreateElement(inc.DocumentElement.Name);
                incNode.InnerXml = inc.DocumentElement.InnerXml;

                // replace original node
                node.ParentNode.ReplaceChild(incNode, node);
            }
        }

        private static void ParsePagesRecursively(string path, SiteStructure site, XmlNodeList xmlPages, PortalPage parentPage)
        {
            foreach (XmlNode xmlPage in xmlPages)
            {
                PortalPage page = new PortalPage();
                page.ParentPage = parentPage;

                // page properties
                if (xmlPage.Attributes["name"] == null)
                    throw new Exception(String.Format("Page name is not specified. File: {0}, Node: {1}",
                        path, xmlPage.OuterXml));
                page.Name = xmlPage.Attributes["name"].Value;

                if (xmlPage.Attributes["roles"] == null)
                    page.Roles.Add("*");
                else
                    page.Roles.AddRange(xmlPage.Attributes["roles"].Value.Split(ROLES_DELIMITERS.ToCharArray()));

                if (xmlPage.Attributes["selectedUserContext"] != null)
                    page.Roles.AddRange(xmlPage.Attributes["selectedUserContext"].Value.Split(ROLES_DELIMITERS.ToCharArray()));

                page.Enabled = (xmlPage.Attributes["enabled"] != null) ? Boolean.Parse(xmlPage.Attributes["enabled"].Value) : true;
                page.Hidden = (xmlPage.Attributes["hidden"] != null) ? Boolean.Parse(xmlPage.Attributes["hidden"].Value) : false;
                page.Align = (xmlPage.Attributes["align"] != null) ? xmlPage.Attributes["align"].Value : null;
                page.SkinSrc = (xmlPage.Attributes["skin"] != null) ? xmlPage.Attributes["skin"].Value : null;
				page.AdminSkinSrc = (xmlPage.Attributes["adminskin"] != null) ? xmlPage.Attributes["adminskin"].Value : null;

                if (xmlPage.Attributes["url"] != null)
					page.Url = xmlPage.Attributes["url"].Value;

                if (xmlPage.Attributes["target"] != null)
                    page.Target = xmlPage.Attributes["target"].Value;

                // content panes
                XmlNodeList xmlContentPanes = xmlPage.SelectNodes("Content");
                foreach (XmlNode xmlContentPane in xmlContentPanes)
                {
                    ContentPane pane = new ContentPane();
                    if (xmlContentPane.Attributes["id"] == null)
                        throw new Exception(String.Format("ContentPane ID is not specified. File: {0}, Node: {1}",
                            path, xmlContentPane.ParentNode.OuterXml));
                    pane.Id = xmlContentPane.Attributes["id"].Value;
                    page.ContentPanes.Add(pane.Id, pane);

                    // page modules
                    XmlNodeList xmlModules = xmlContentPane.SelectNodes("Module");
                    foreach (XmlNode xmlModule in xmlModules)
                    {
                        PageModule module = new PageModule();
                        module.ModuleId = site.Modules.Count + 1;
                        module.Page = page;
                        site.Modules.Add(module.ModuleId, module);

                        if (xmlModule.Attributes["moduleDefinitionID"] == null)
                            throw new Exception(String.Format("ModuleDefinition ID is not specified. File: {0}, Node: {1}",
                                path, xmlModule.ParentNode.OuterXml));
						module.ModuleDefinitionID = xmlModule.Attributes["moduleDefinitionID"].Value.ToLower(CultureInfo.InvariantCulture);

                        if (xmlModule.Attributes["title"] != null)
                            module.Title = xmlModule.Attributes["title"].Value;

                        if (xmlModule.Attributes["icon"] != null)
                            module.IconFile = xmlModule.Attributes["icon"].Value;

                        if (xmlModule.Attributes["container"] != null)
                            module.ContainerSrc = xmlModule.Attributes["container"].Value;

						if (xmlModule.Attributes["admincontainer"] != null)
							module.AdminContainerSrc = xmlModule.Attributes["admincontainer"].Value;

                        if (xmlModule.Attributes["viewRoles"] == null)
                            module.ViewRoles.Add("*");
                        else
                            module.ViewRoles.AddRange(xmlModule.Attributes["viewRoles"].Value.Split(ROLES_DELIMITERS.ToCharArray()));

                        if (xmlModule.Attributes["readOnlyRoles"] != null)
                            module.ReadOnlyRoles.AddRange(xmlModule.Attributes["readOnlyRoles"].Value.Split(ROLES_DELIMITERS.ToCharArray()));


                        if (xmlModule.Attributes["editRoles"] == null)
                            module.EditRoles.Add("*");
                        else
                            module.EditRoles.AddRange(xmlModule.Attributes["editRoles"].Value.Split(ROLES_DELIMITERS.ToCharArray()));

                        // settings
                        XmlNodeList xmlSettings = xmlModule.SelectNodes("Settings/Add");
                        foreach (XmlNode xmlSetting in xmlSettings)
                        {
                            module.Settings[xmlSetting.Attributes["name"].Value] = xmlSetting.Attributes["value"].Value;
                        }

						XmlNode xmlModuleData = xmlModule.SelectSingleNode("ModuleData");
                        if (xmlModuleData != null)
                        {
                            // check reference
                            if (xmlModuleData.Attributes["ref"] != null)
                            {
                                // load referenced module data
                                xmlModuleData = xmlModule.OwnerDocument.SelectSingleNode(
                                    "Pages/ModulesData/ModuleData[@id='" + xmlModuleData.Attributes["ref"].Value + "']");
                            }
                            module.LoadXmlModuleData(xmlModuleData.OuterXml);
                        }

                        pane.Modules.Add(module);
                    }
                }

                // add page to te array
                if (parentPage != null)
                    parentPage.Pages.Add(page);

                site.Pages.Add(page);

                // process children
                XmlNodeList xmlChildPages = xmlPage.SelectNodes("Pages/Page");
                ParsePagesRecursively(path, site, xmlChildPages, page);
            }
        }
    }
}
