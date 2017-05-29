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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class SpaceSettings : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // module visibility
            this.ContainerControl.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User && 
                PanelSecurity.PackageId > 1);

            lnkNameServers.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsNameServers",
                "edit_settings", "SettingsName=NameServers", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkInstantAlias.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsInstantAlias",
                "edit_settings", "SettingsName=InstantAlias", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkSharedSSL.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsSharedSslSites",
                "edit_settings", "SettingsName=SharedSslSites", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkPackagesFolder.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsSpacesFolder",
                "edit_settings", "SettingsName=ChildSpacesFolder", "SpaceID=" + PanelSecurity.PackageId.ToString());
			lnkExchangeServer.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsExchangeServer",
				"edit_settings", "SettingsName=ExchangeServer", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkVps.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsVPS",
                "edit_settings", "SettingsName=VirtualPrivateServers", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkVps2012.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsVPS2012",
                "edit_settings", "SettingsName=VirtualPrivateServers2012", "SpaceID=" + PanelSecurity.PackageId.ToString());
            lnkVpsForPC.NavigateUrl = EditUrl("SettingsControl", "SpaceSettingsVPSForPC",
                "edit_settings", "SettingsName=VirtualPrivateServersForPrivateCloud", "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkDnsRecords.NavigateUrl = EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_globaldns");

        }
    }
}
