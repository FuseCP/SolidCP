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
	/// Represents <connections> configuration element containing a collection of child elements.
	/// </summary>
	public class ComponentCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// Creates a new instance of the ConnectionCollection class.
		/// </summary>
		public ComponentCollection()
		{
			AddElementName = "component";
		}

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
			return new ComponentConfigElement();
		}


		/// <summary>
		/// Creates a new ConfigurationElement. 
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement(string id)
		{
			return new ComponentConfigElement(id);
		}


		/// <summary>
		///  
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override Object GetElementKey(ConfigurationElement element)
		{
			ComponentConfigElement componentConfigElement = element as ComponentConfigElement;
			return componentConfigElement.ID;
		}


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
		/// Gets or sets a child element of this ConnectionCollection object.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ComponentConfigElement this[int index]
		{
			get
			{
				return (ComponentConfigElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		/// <summary>
		/// Gets or sets a child element of this ConnectionCollection object.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public new ComponentConfigElement this[string id]
		{
			get
			{
				return (ComponentConfigElement)BaseGet(id);
			}
		}

		/// <summary>
		/// The index of the specified PluginConfigElement.
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public int IndexOf(ComponentConfigElement element)
		{
			return BaseIndexOf(element);
		}

		/// <summary>
		/// Adds a PluginConfigElement to the ConnectionCollection instance. 
		/// </summary>
		/// <param name="c"></param>
		public void Add(ComponentConfigElement c)
		{
			BaseAdd(c);

			// Add custom code here.
		}

		public void Remove(ComponentConfigElement c)
		{
			if (BaseIndexOf(c) >= 0)
				BaseRemove(c.ID);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(string id)
		{
			BaseRemove(id);
		}

		public void Clear()
		{
			BaseClear();
			// Add custom code here.
		}
	}
}
