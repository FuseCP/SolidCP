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
using SolidCP.Providers.Web.Iis.Common;
using Microsoft.Web.Administration;
using SolidCP.Providers.Web.Iis.Utility;

namespace SolidCP.Providers.Web.MimeTypes
{
	internal sealed class MimeTypesModuleService : ConfigurationModuleService
	{
		public const string FileExtensionAttribute = "fileExtension";
		public const string MimeTypeAttribute = "mimeType";

		/// <summary>
		/// Loads available mime maps into supplied virtual iisDirObject description.
		/// </summary>
		/// <param name="vdir">Virtual iisDirObject description.</param>
		public void GetMimeMaps(ServerManager srvman, WebAppVirtualDirectory virtualDir)
		{
			var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
			//
			var section = config.GetSection(Constants.StaticContentSection);
			//
			var mappings = new List<MimeMap>();
			//
			foreach (var item in section.GetCollection())
			{
				var item2Get = GetMimeMap(item);
				//
				if (item2Get == null)
					continue;
				//
				mappings.Add(item2Get);
			}
			//
			virtualDir.MimeMaps = mappings.ToArray();
		}

		/// <summary>
		/// Saves mime types from virtual iisDirObject description into configuration file.
		/// </summary>
		/// <param name="vdir">Virtual iisDirObject description.</param>
		public void SetMimeMaps(WebAppVirtualDirectory virtualDir)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				//
				var section = config.GetSection(Constants.StaticContentSection);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			// Ensure mime maps are set
			if (virtualDir.MimeMaps == null || virtualDir.MimeMaps.Length == 0)
				return;

			#region Put the change in effect
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				//
				var section = config.GetSection(Constants.StaticContentSection);
				//
				var typesCollection = section.GetCollection();
				//
				foreach (var item in virtualDir.MimeMaps)
				{
					// Make sure mime-type mapping file extension is formatted exactly as it should be
					if (!item.Extension.StartsWith("."))
						item.Extension = "." + item.Extension;
					//
					int indexOf = FindMimeMap(typesCollection, item);
					//
					if (indexOf > -1)
					{
						var item2Renew = typesCollection[indexOf];
						//
						FillConfigurationElementWithData(item2Renew, item);
						//
						continue;
					}
					//
					typesCollection.Add(CreateMimeMap(typesCollection, item));
				}
				//
				srvman.CommitChanges();
			}
			#endregion
		}

		private MimeMap GetMimeMap(ConfigurationElement element)
		{
			// skip inherited mime mappings
			if (element == null || !element.IsLocallyStored)
				return null;
			//
			return new MimeMap
			{
				Extension	= Convert.ToString(element.GetAttributeValue(FileExtensionAttribute)),
				MimeType	= Convert.ToString(element.GetAttributeValue(MimeTypeAttribute))
			};
		}

		private ConfigurationElement CreateMimeMap(ConfigurationElementCollection collection, MimeMap mapping)
		{
			if (mapping == null
				|| String.IsNullOrEmpty(mapping.MimeType)
					|| String.IsNullOrEmpty(mapping.Extension))
			{
				return null;
			}
			//
			var item2Add = collection.CreateElement("mimeMap");
			//
			FillConfigurationElementWithData(item2Add, mapping);
			//
			return item2Add;
		}

		private void FillConfigurationElementWithData(ConfigurationElement item2Fill, MimeMap mapping)
		{
			if (mapping == null
				|| item2Fill == null
					|| String.IsNullOrEmpty(mapping.MimeType)
						|| String.IsNullOrEmpty(mapping.Extension))
			{
				return;
			}
			//
			item2Fill.SetAttributeValue(MimeTypeAttribute, mapping.MimeType);
			item2Fill.SetAttributeValue(FileExtensionAttribute, mapping.Extension);
		}

		private int FindMimeMap(ConfigurationElementCollection collection, MimeMap mapping)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				var item = collection[i];
				//
				if (String.Equals(item.GetAttributeValue(FileExtensionAttribute), mapping.Extension))
				{
					return i;
				}
			}
			//
			return -1;
		}
	}
}
