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
using SolidCP.Providers.Web;

namespace SolidCP.Portal
{
    public partial class VirtualDirectoryHomeFolderControl : SolidCPControlBase
    {
        private bool isVirtualDirectory;
        public bool IsVirtualDirectory
        {
            get { return isVirtualDirectory; }
            set { isVirtualDirectory = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadDirectoryContentPath(int packageId, WebVirtualDirectory item)
        {
            item = ES.Services.WebServers.GetVirtualDirectory(PanelRequest.ItemID, PanelRequest.VirtDir);
            fileLookup.SelectedFile = item.ContentPath;
        }

        public void BindWebItem(int packageId, WebVirtualDirectory item)
        {
            fileLookup.PackageId = item.PackageId;
            fileLookup.SelectedFile = item.ContentPath;

            string resSuffix = item.IIs7 ? "IIS7" : "";

            chkAuthAnonymous.Checked = item.EnableAnonymousAccess;
            chkAuthWindows.Checked = item.EnableWindowsAuthentication;
            chkAuthBasic.Checked = item.EnableBasicAuthentication;

            // toggle controls by quotas
            fileLookup.Enabled = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_HOMEFOLDERS);

            UserSettings settings = ES.Services.Users.GetUserSettings(PanelSecurity.SelectedUserId, "WebPolicy");
        }

        public void SaveWebItem(WebVirtualDirectory item)
        {
            item.ContentPath = fileLookup.SelectedFile;

            item.EnableAnonymousAccess = chkAuthAnonymous.Checked;
            item.EnableWindowsAuthentication = chkAuthWindows.Checked;
            item.EnableBasicAuthentication = chkAuthBasic.Checked;

        }

 
    }
}
