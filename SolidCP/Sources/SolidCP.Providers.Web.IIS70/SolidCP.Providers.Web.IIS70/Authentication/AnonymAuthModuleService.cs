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

namespace SolidCP.Providers.Web.Iis.Authentication
{
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using Common;
	using Utility;
    using System;
    using System.Globalization;

    internal sealed class AnonymAuthModuleService : ConfigurationModuleService
    {
		public const string EnabledAttribute = "enabled";
		public const string UserNameAttribute = "userName";
		public const string PasswordAttribute = "password";

		public PropertyBag GetAuthenticationSettings(ServerManager srvman, string siteId)
        {
			var config = srvman.GetApplicationHostConfiguration();
			//
			var section = config.GetSection(Constants.AnonymousAuthenticationSection, siteId);
			//
			PropertyBag bag = new PropertyBag();
			//
			bag[AuthenticationGlobals.AnonymousAuthenticationUserName] = Convert.ToString(section.GetAttributeValue(UserNameAttribute));
			bag[AuthenticationGlobals.AnonymousAuthenticationPassword] = Convert.ToString(section.GetAttributeValue(PasswordAttribute));
			bag[AuthenticationGlobals.Enabled] = Convert.ToBoolean(section.GetAttributeValue(EnabledAttribute));
			bag[AuthenticationGlobals.IsLocked] = section.IsLocked;
			//
			return bag;
        }

        public void SetAuthenticationSettings(string virtualPath, string userName, 
			string password, bool enabled)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var section = config.GetSection(Constants.AnonymousAuthenticationSection, virtualPath);
				//
				section.SetAttributeValue(EnabledAttribute, enabled);
				section.SetAttributeValue(UserNameAttribute, userName);
				section.SetAttributeValue(PasswordAttribute, password);
				//
				srvman.CommitChanges();
			}
        }

        public void RemoveAuthenticationSettings(string virtualPath)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				config.RemoveLocationPath(virtualPath);
				//
				srvman.CommitChanges();
			}
        }
    }
}
