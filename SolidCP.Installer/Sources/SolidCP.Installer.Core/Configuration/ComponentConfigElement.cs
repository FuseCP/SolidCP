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
using System.Collections;
using System.Configuration;
using System.Text;

namespace SolidCP.Installer.Configuration
{
	/// <summary>
	/// Represents a configuration element containing component info.
	/// </summary>
	public class ComponentConfigElement : ConfigurationElement
	{
		/// <summary>
		/// Creates a new instance of the ServerConfigElement class.
		/// </summary>
		public ComponentConfigElement() : this(string.Empty)
		{
		}


		/// <summary>
		/// Creates a new instance of the ServerConfigElement class.
		/// </summary>
		public ComponentConfigElement(string id)
		{
			ID = id;
		}


		[ConfigurationProperty("id", IsRequired = true, IsKey = true)]
		public string ID
		{
			get
			{
				return (string)this["id"];
			}
			set
			{
				this["id"] = value;
			}
		}

		[ConfigurationProperty("settings", IsDefaultCollection = false)]
		public KeyValueConfigurationCollection Settings
		{
			get
			{
				return (KeyValueConfigurationCollection)base["settings"];
			}
		}

		public string GetStringSetting(string key)
		{
			string ret = null;
			if (Settings[key] != null)
			{
				ret = Settings[key].Value;
			}
			return ret;
		}

		public int GetInt32Setting(string key)
		{
			int ret = 0;
			if (Settings[key] != null)
			{
				string val = Settings[key].Value;
				Int32.TryParse(val, out ret);
			}
			return ret;
		}

		public bool GetBooleanSetting(string key)
		{
			bool ret = false;
			if (Settings[key] != null)
			{
				string val = Settings[key].Value;
				Boolean.TryParse(val, out ret);
			}
			return ret;
		}

		/*
		/// <summary>
		/// Gets the type of the ConfigurationElementCollection.
		/// </summary>
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}
		
		/// <summary>
		/// Creates a new ConfigurationElement. 
		/// </summary>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement()
		{
			//return new ModuleConfigElement();
			return null;
		}*/


		/// <summary>
		/// Creates a new ConfigurationElement. 
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		/*protected override ConfigurationElement CreateNewElement(string elementName)
		{
			return new ModuleConfigElement(elementName);
		}*/

		/*
		/// <summary>
		///  
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override Object GetElementKey(ConfigurationElement element)
		{
			//ModuleConfigElement moduleConfigElement = element as ModuleConfigElement;
			//return userConfigElement.Module;
			return null;
		}*/


		/*		public new string AddElementName
				{
					get
					{
						return base.AddElementName;
					}
					set
					{
						base.AddElementName = value; 
					}
				}

				public new string ClearElementName
				{
					get
					{ return base.ClearElementName; }

					set
					{ base.AddElementName = value; }

				}

				public new string RemoveElementName
				{
					get
					{ return base.RemoveElementName; }


				}

				public new int Count
				{

					get { return base.Count; }

				}

		*/
		/// <summary>
		/// Gets or sets a child element of this ServerConfigElement object.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/*public UserConfigElement this[int index]
		{
			get
			{
				return (UserConfigElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}*/

	/*	/// <summary>
		/// Gets or sets a child element of this ServerConfigElement object.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		new public UserConfigElement this[string name]
		{
			get
			{
				return (UserConfigElement)BaseGet(name);
			}
		}*/

		/// <summary>
		/// The index of the specified PluginConfigElement.
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		/*public int IndexOf(UserConfigElement connection)
		{
			return BaseIndexOf(connection);
		}*/

		/// <summary>
		/// Adds a PluginConfigElement to the ServerConfigElement instance. 
		/// </summary>
		/// <param name="c"></param>
		/*public void Add(UserConfigElement c)
		{
			BaseAdd(c);

			// Add custom code here.
		}*/
	}
}
