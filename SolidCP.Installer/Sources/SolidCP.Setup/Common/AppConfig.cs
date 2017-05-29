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
using System.Xml;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SolidCP.Setup
{
	public sealed class AppConfig
	{
		public const string AppConfigFileNameWithoutExtension = "SolidCP.Installer.exe";
        static AppConfig()
        {
            ConfigurationPath = DefaultConfigurationPath;
        }
		private AppConfig()
		{
        
		}
		private static Configuration appConfig = null;
		private static XmlDocument xmlConfig = null;
        public static string ConfigurationPath { get; set; }
        public static string DefaultConfigurationPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfigFileNameWithoutExtension); } }
		public static void LoadConfiguration(ExeConfigurationFileMap FnMap = null, ConfigurationUserLevel CuLevel = ConfigurationUserLevel.None)
		{
            if (FnMap == null)
                appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationPath);
            else
                appConfig = ConfigurationManager.OpenMappedExeConfiguration(FnMap, CuLevel);
			ConfigurationSection section = appConfig.Sections["installer"];
			if (section == null)
				throw new ConfigurationErrorsException("installer section not found in " + appConfig.FilePath);
			string strXml = section.SectionInformation.GetRawXml();
			xmlConfig = new XmlDocument();
			xmlConfig.LoadXml(strXml);
		}

		public static XmlDocument Configuration
		{
			get
			{
				return xmlConfig;
			}
		}

		public static void EnsureComponentConfig(string componentId)
		{
			var xmlConfigNode = GetComponentConfig(componentId);
			//
			if (xmlConfigNode != null)
				return;
			//
			CreateComponentConfig(componentId);
		}

		public static XmlNode CreateComponentConfig(string componentId)
		{
			XmlNode components = Configuration.SelectSingleNode("//components");
			if (components == null)
			{
				components = Configuration.CreateElement("components");
				Configuration.FirstChild.AppendChild(components);
			}
			XmlElement componentNode = Configuration.CreateElement("component");
			componentNode.SetAttribute("id", componentId);
			components.AppendChild(componentNode);

			XmlElement settingsNode = Configuration.CreateElement("settings");
			componentNode.AppendChild(settingsNode);
			return componentNode;
		}
		
		public static XmlNode GetComponentConfig(string componentId)
		{
			string xPath = string.Format("//component[@id=\"{0}\"]", componentId);
			return Configuration.SelectSingleNode(xPath);
		}

		public static void SaveConfiguration()
		{
			if (appConfig != null && xmlConfig != null)
			{
				ConfigurationSection section = appConfig.Sections["installer"];
				section.SectionInformation.SetRawXml(xmlConfig.OuterXml);
				appConfig.Save();
			}
		}

		public static void SetComponentSettingStringValue(string componentId, string settingName, string value)
		{
			XmlNode componentNode = GetComponentConfig(componentId);
			XmlNode settings = componentNode.SelectSingleNode("settings");
			string xpath = string.Format("add[@key=\"{0}\"]", settingName);
			XmlNode settingNode = settings.SelectSingleNode(xpath);
			if (settingNode == null)
			{
				settingNode = Configuration.CreateElement("add");
				XmlUtils.SetXmlAttribute(settingNode, "key", settingName);
				settings.AppendChild(settingNode);
			}
			XmlUtils.SetXmlAttribute(settingNode, "value", value);
		}

		public static void SetComponentSettingBooleanValue(string componentId, string settingName, bool value)
		{
			XmlNode componentNode = GetComponentConfig(componentId);
			XmlNode settings = componentNode.SelectSingleNode("settings");
			string xpath = string.Format("add[@key=\"{0}\"]", settingName);
			XmlNode settingNode = settings.SelectSingleNode(xpath);
			if (settingNode == null)
			{
				settingNode = Configuration.CreateElement("add");
				XmlUtils.SetXmlAttribute(settingNode, "key", settingName);
				settings.AppendChild(settingNode);
			}
			XmlUtils.SetXmlAttribute(settingNode, "value", value.ToString());
		}

		public static void LoadComponentSettings(SetupVariables vars)
		{
			XmlNode componentNode = GetComponentConfig(vars.ComponentId);
			//
			if (componentNode != null)
			{
				var typeRef = vars.GetType();
				//
				XmlNodeList settingNodes = componentNode.SelectNodes("settings/add");
				//
				foreach (XmlNode item in settingNodes)
				{
					var sName = XmlUtils.GetXmlAttribute(item, "key");
					var sValue = XmlUtils.GetXmlAttribute(item, "value");
					//
					if (String.IsNullOrEmpty(sName))
						continue;
					//
					var objProperty = typeRef.GetProperty(sName);
					//
					if (objProperty == null)
						continue;
					// Set property value
					objProperty.SetValue(vars, Convert.ChangeType(sValue, objProperty.PropertyType), null);
				}
			}
		}

		public static string GetComponentSettingStringValue(string componentId, string settingName)
		{
			string ret = null;
			XmlNode componentNode = GetComponentConfig(componentId);
			if (componentNode != null)
			{
				string xpath = string.Format("settings/add[@key=\"{0}\"]", settingName);
				XmlNode settingNode = componentNode.SelectSingleNode(xpath);
				if (settingNode != null)
				{
					ret = XmlUtils.GetXmlAttribute(settingNode, "value");
				}
			}
			return ret;
		}

		internal static int GetComponentSettingInt32Value(string componentId, string settingName)
		{
			int ret = 0;
			XmlNode componentNode = GetComponentConfig(componentId);
			if (componentNode != null)
			{
				string xpath = string.Format("settings/add[@key=\"{0}\"]", settingName);
				XmlNode settingNode = componentNode.SelectSingleNode(xpath);
				string val = XmlUtils.GetXmlAttribute(settingNode, "value");
				Int32.TryParse(val, out ret);
			}
			return ret;
		}

		internal static bool GetComponentSettingBooleanValue(string componentId, string settingName)
		{
			bool ret = false;
			XmlNode componentNode = GetComponentConfig(componentId);
			if (componentNode != null)
			{
				string xpath = string.Format("settings/add[@key=\"{0}\"]", settingName);
				XmlNode settingNode = componentNode.SelectSingleNode(xpath);
				string val = XmlUtils.GetXmlAttribute(settingNode, "value");
				Boolean.TryParse(val, out ret);
			}
			return ret;
		}

		internal static string GetSettingStringValue(string settingName)
		{
			string ret = null;
			string xPath = string.Format("settings/add[@key=\"{0}\"]", settingName);
			XmlNode settingNode = Configuration.SelectSingleNode(xPath);
			if (settingNode != null)
			{
				ret = XmlUtils.GetXmlAttribute(settingNode, "value");
			}
			return ret;
		}
	}
}
