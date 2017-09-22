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

using Microsoft.Web.Administration;

namespace SolidCP.Providers.Web.Iis.DefaultDocuments
{
	using Common;
	using Microsoft.Web.Administration;
	using Microsoft.Web.Management.Server;
	using System;
	using System.Text;
	using System.Collections.Generic;
	using System.Collections;
	using SolidCP.Providers.Web.Iis.Utility;

	internal sealed class DefaultDocsModuleService : ConfigurationModuleService
	{
		public const string ValueAttribute = "value";

		public string GetDefaultDocumentSettings(ServerManager srvman, string siteId)
		{
			// Load web site configuration
			var config = srvman.GetWebConfiguration(siteId);
			// Load corresponding section
			var section = config.GetSection(Constants.DefaultDocumentsSection);
			//
			var filesCollection = section.GetCollection("files");
			// Build default documents
			var defaultDocs = new List<String>();
			//
			foreach (var item in filesCollection)
			{
				var item2Get = GetDefaultDocument(item);
				//
				if (String.IsNullOrEmpty(item2Get))
					continue;
				//
				defaultDocs.Add(item2Get);
			}
			//
			return String.Join(",", defaultDocs.ToArray());
		}

		public void SetDefaultDocumentsEnabled(string siteId, bool enabled)
		{
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(siteId);
				// Load corresponding section
				var section = config.GetSection(Constants.DefaultDocumentsSection);
				//
				section.SetAttributeValue("enabled", enabled);
				//
				srvman.CommitChanges();
			}
		}

		public void SetDefaultDocumentSettings(string siteId, string defaultDocs)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(siteId);
				// Load corresponding section
				var section = config.GetSection(Constants.DefaultDocumentsSection);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			// Exit if no changes have been made
			if (String.IsNullOrEmpty(defaultDocs))
				return;

			// Update default documents list
			var docs2Add = defaultDocs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			#region Put changes in effect
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(siteId);
				// Load corresponding section
				var section = config.GetSection(Constants.DefaultDocumentsSection);
				//
				var filesCollection = section.GetCollection("files");
				// The only solution to override inherited default documents is to use <clear/> element
				filesCollection.Clear();
				//
				foreach (var item in docs2Add)
				{
					// The default document specified exists
					if (FindDefaultDocument(filesCollection, item) > -1)
						continue;
					//
					var item2Add = CreateDefaultDocument(filesCollection, item);
					//
					if (item2Add == null)
						continue;
					//
					filesCollection.Add(item2Add);
				}
				//
				srvman.CommitChanges();
			} 
			#endregion
		}

		private string GetDefaultDocument(ConfigurationElement element)
		{
			if (element == null)
				return null;
			//
			return Convert.ToString(element.GetAttributeValue(ValueAttribute));
		}

		private ConfigurationElement CreateDefaultDocument(ConfigurationElementCollection collection, string valueStr)
		{
			if (valueStr == null)
				return null;
			//
			valueStr = valueStr.Trim();
			//
			if (String.IsNullOrEmpty(valueStr))
				return null;

			//
			ConfigurationElement file2Add = collection.CreateElement("add");
			file2Add.SetAttributeValue(ValueAttribute, valueStr);
			//
			return file2Add;
		}

		private int FindDefaultDocument(ConfigurationElementCollection collection, string valueStr)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				var item = collection[i];
				//
				var valueObj = item.GetAttributeValue(ValueAttribute);
				//
				if (String.Equals((String)valueObj, valueStr, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			//
			return -1;
		}
	}
}
