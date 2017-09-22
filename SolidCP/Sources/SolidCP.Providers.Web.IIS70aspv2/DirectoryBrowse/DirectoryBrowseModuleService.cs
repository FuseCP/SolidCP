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

namespace SolidCP.Providers.Web.Iis.DirectoryBrowse
{
	using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using Common;
    using System;

    internal sealed class DirectoryBrowseModuleService : ConfigurationModuleService
    {
        public PropertyBag GetDirectoryBrowseSettings(ServerManager srvman, string siteId)
        {
			var config = srvman.GetWebConfiguration(siteId);
			//
			DirectoryBrowseSection directoryBrowseSection = (DirectoryBrowseSection)config.GetSection(Constants.DirectoryBrowseSection, typeof(DirectoryBrowseSection));
			//
			PropertyBag bag = new PropertyBag();
			bag[DirectoryBrowseGlobals.Enabled] = directoryBrowseSection.Enabled;
			bag[DirectoryBrowseGlobals.ShowFlags] = (int)directoryBrowseSection.ShowFlags;
			bag[DirectoryBrowseGlobals.ReadOnly] = directoryBrowseSection.IsLocked;
			return bag;
        }

        public void SetDirectoryBrowseEnabled(string siteId, bool enabled)
        {
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(siteId);
				//
				var section = config.GetSection("system.webServer/directoryBrowse");
				//
				section.SetAttributeValue("enabled", enabled);
				//
				srvman.CommitChanges();
			}
        }

        public void SetDirectoryBrowseSettings(string siteId, PropertyBag updatedBag)
        {
			if (updatedBag == null)
				return;

			using (var srvman = GetServerManager())
			{
				var config = srvman.GetWebConfiguration(siteId);
				//
				DirectoryBrowseSection section = (DirectoryBrowseSection)config.GetSection(Constants.DirectoryBrowseSection, typeof(DirectoryBrowseSection));
				//
				section.Enabled = (bool)updatedBag[DirectoryBrowseGlobals.Enabled];
				section.ShowFlags = (DirectoryBrowseShowFlags)updatedBag[DirectoryBrowseGlobals.ShowFlags];
				//
				srvman.CommitChanges();
			}
        }
    }
}
