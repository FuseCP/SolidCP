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
using System.Collections;
using SolidCP.Providers.Web.Iis.Utility;

namespace SolidCP.Providers.Web.HttpRedirect
{
	internal class HttpRedirectModuleService : ConfigurationModuleService
	{
		public const string EnabledAttribute = "enabled";
		public const string ExactDestinationAttribute = "exactDestination";
		public const string ChildOnlyAttribute = "childOnly";
		public const string DestinationAttribute = "destination";
		public const string HttpResponseStatusAttribute = "httpResponseStatus";

		public void GetHttpRedirectSettings(ServerManager srvman, WebAppVirtualDirectory virtualDir)
		{
			// Load web site configuration
			var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
			// Load corresponding section
			var section = config.GetSection(Constants.HttpRedirectSection);
			//
			if (!Convert.ToBoolean(section.GetAttributeValue(EnabledAttribute)))
				return;
			//
			virtualDir.RedirectExactUrl = Convert.ToBoolean(section.GetAttributeValue(ExactDestinationAttribute));
			virtualDir.RedirectDirectoryBelow = Convert.ToBoolean(section.GetAttributeValue(ChildOnlyAttribute));
			virtualDir.HttpRedirect = Convert.ToString(section.GetAttributeValue(DestinationAttribute));
			virtualDir.RedirectPermanent = String.Equals("301", Convert.ToString(section.GetAttributeValue(HttpResponseStatusAttribute)));		
		}

		public void SetHttpRedirectSettings(WebAppVirtualDirectory virtualDir)
		{
			#region Revert to parent settings (inherited)
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				// Load corresponding section
				var section = config.GetSection(Constants.HttpRedirectSection);
				//
				section.RevertToParent();
				//
				srvman.CommitChanges();
			} 
			#endregion

			// HttpRedirect property is not specified so defaults to the parent
			if (String.IsNullOrEmpty(virtualDir.HttpRedirect))
				return;

			#region Put changes in effect
			using (var srvman = GetServerManager())
			{
				// Load web site configuration
				var config = srvman.GetWebConfiguration(virtualDir.FullQualifiedPath);
				// Load corresponding section
				var section = config.GetSection(Constants.HttpRedirectSection);
				// Enable http redirect feature
				section.SetAttributeValue(EnabledAttribute, true);
				section.SetAttributeValue(ExactDestinationAttribute, virtualDir.RedirectExactUrl);
				section.SetAttributeValue(DestinationAttribute, virtualDir.HttpRedirect);
				section.SetAttributeValue(ChildOnlyAttribute, virtualDir.RedirectDirectoryBelow);
				// Configure HTTP Response Status
				if (virtualDir.RedirectPermanent)
					section.SetAttributeValue(HttpResponseStatusAttribute, "Permanent");
				else
					section.SetAttributeValue(HttpResponseStatusAttribute, "Found");
				//
				srvman.CommitChanges();
			} 
			#endregion
		}
	}
}
