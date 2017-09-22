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

﻿using System;
using System.Collections.Generic;
using System.Text;
using SolidCP.Providers.Web.Iis.Common;
using SolidCP.Providers.Web.Iis.Utility;
using Microsoft.Web.Administration;

namespace SolidCP.Providers.Web.WebObjects
{
	public class CustomHttpHeadersModuleService : ConfigurationModuleService
	{
		public const string NameAttribute = "name";
		public const string ValueAttribute = "value";

		public void GetCustomHttpHeaders(ServerManager srvman, WebAppVirtualDirectory virtualDir)
		{
			var config = srvman.GetApplicationHostConfiguration();
			//
			var httpProtocolSection = config.GetSection(Constants.HttpProtocolSection, virtualDir.FullQualifiedPath);
			//
			if (httpProtocolSection == null)
				return;
			//
			var headersCollection = httpProtocolSection.GetCollection("customHeaders");
			//
			var headers = new List<HttpHeader>();
			//
			foreach (var item in headersCollection)
			{
				var item2Get = GetCustomHttpHeader(item);
				//
				if (item2Get == null)
					continue;
				//
				headers.Add(item2Get);
			}
			//
			virtualDir.HttpHeaders = headers.ToArray();
		}

		public void SetCustomHttpHeaders(WebAppVirtualDirectory virtualDir)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var section = config.GetSection(Constants.HttpProtocolSection, virtualDir.FullQualifiedPath);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			// Ensure virtual directory has Custom HTTP Headers set
			if (virtualDir.HttpHeaders == null || virtualDir.HttpHeaders.Length == 0)
				return;

			#region Put the change in effect
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var section = config.GetSection(Constants.HttpProtocolSection, virtualDir.FullQualifiedPath);
				//
				var headersCollection = section.GetCollection("customHeaders");
				// Clean the collection to avoid duplicates collision on the root and nested levels
				headersCollection.Clear();
				// Iterate over custom http headers are being set
				foreach (var item in virtualDir.HttpHeaders)
				{
					// Trying to find out whether the header is being updated
					int indexOf = FindCustomHttpHeader(headersCollection, item);
					//
					if (indexOf > -1)
					{
						// Obtain the custom http header to update
						var item2Renew = headersCollection[indexOf];
						// Apply changes to the element
						FillConfigurationElementWithData(item2Renew, item);
						// Loop the next item
						continue;
					}
					// Creating a new custom http header
					var item2Add = CreateCustomHttpHeader(headersCollection, item);
					// Checking results of the create operation
					if (item2Add == null)
						continue;
					// Adding the newly created custom http header
					headersCollection.Add(item2Add);
				}
				// Commit changes
				srvman.CommitChanges();
			} 
			#endregion
		}

		private HttpHeader GetCustomHttpHeader(ConfigurationElement element)
		{
			if (element == null)
				return null;
			//
			return new HttpHeader
			{
				Key		= Convert.ToString(element.GetAttributeValue(NameAttribute)),
				Value	= Convert.ToString(element.GetAttributeValue(ValueAttribute))
			};
		}
		
		private ConfigurationElement CreateCustomHttpHeader(ConfigurationElementCollection collection, HttpHeader header)
		{
			// Skip elements either empty or with empty data
			if (header == null || String.IsNullOrEmpty(header.Key))
				return null;

			//
			ConfigurationElement header2Add = collection.CreateElement("add");
			//
			FillConfigurationElementWithData(header2Add, header);
			//
			return header2Add;
		}

		private void FillConfigurationElementWithData(ConfigurationElement item2Fill, HttpHeader header)
		{
			//
			item2Fill.SetAttributeValue(NameAttribute, header.Key);
			item2Fill.SetAttributeValue(ValueAttribute, header.Value);
		}
		
		private int FindCustomHttpHeader(ConfigurationElementCollection collection, HttpHeader header)
		{
			for (int i = 0; i < collection.Count; i++)
			{
				var item = collection[i];
				//
				if (String.Equals(item.GetAttributeValue(NameAttribute), header.Key))
				{
					return i;
				}
			}
			//
			return -1;
		}
	}
}
