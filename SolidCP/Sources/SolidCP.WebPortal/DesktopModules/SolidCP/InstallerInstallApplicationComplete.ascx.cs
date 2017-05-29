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
using System.IO;
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
    public partial class InstallerInstallApplicationComplete : SolidCPModuleBase
    {
        ApplicationInfo app = null;
        int packageId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            packageId = PanelSecurity.PackageId;

            // load app info
            try
            {
                app = ES.Services.ApplicationsInstaller.GetApplication(packageId, PanelRequest.ApplicationID);
                if (app == null)
                    RedirectToBrowsePage();


                LoadApplicationSettingsControl();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("APPINSTALLER_INIT_FORM", ex);
                return;
            }
        }

        private void LoadApplicationSettingsControl()
        {
            string controlName = app.SettingsControl;
            if (!String.IsNullOrEmpty(controlName))
            {
                try
                {
                    controlName = controlName.Replace(".ascx", "Complete.ascx");
                    string currPath = this.AppRelativeVirtualPath;
                    currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                    string ctrlPath = currPath + "/ApplicationInstallerControls/" + controlName;

                    if (File.Exists(Server.MapPath(ctrlPath)))
                    {
                        Control ctrl = Page.LoadControl(ctrlPath);
                        completePanel.Controls.Add(ctrl);
                    }
                    else
                    {
                        LiteralControl litMsg = new LiteralControl(
							String.Format("<div style=\"text-align:center;font-weight:bold;\">{0}</div>",
                            GetLocalizedString("Text.SuccessInstalled")));
                        completePanel.Controls.Add(litMsg);
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("APPINSTALLER_LOAD_CONTROL", ex);
                    return;
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
    }
}
