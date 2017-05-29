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
    public partial class SpaceSettingsEditor : SolidCPModuleBase
    {
        private string SettingsName
        {
            get { return Request["SettingsName"]; }
        }

        IPackageSettingsEditorControl ctlSettings;

        protected void Page_Load(object sender, EventArgs e)
        {
            // load settings control
            LoadSettingsControl();

            // entry point
            try
            {
                if (!IsPostBack)
                {
                    BindSettings();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_SETTINGS_GET", ex);
                return;
            }
        }

        private void BindSettings()
        {
            // load user settings
            PackageSettings settings = ES.Services.Packages.GetPackageSettings(PanelSecurity.PackageId, SettingsName);

            ddlOverride.SelectedIndex = (settings.PackageId == PanelSecurity.PackageId) ? 1 : 0;
            ToggleControls();

            // bind settings
            ctlSettings.BindSettings(settings);
        }

        protected void ddlOverride_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOverride.SelectedIndex == 0) // use host settings
            {
                // delete current settings
                PackageSettings settings = new PackageSettings();
                settings.PackageId = PanelSecurity.PackageId;
                settings.SettingsName = SettingsName;
                ES.Services.Packages.UpdatePackageSettings(settings);

                // rebind settings
                BindSettings();
            }
            else
            {
                ToggleControls();
            }
        }

        private void ToggleControls()
        {
            // check if we should enable controls
            bool enabled = (ddlOverride.SelectedIndex == 1);

            // enable/disable controls
            EnableControlRecursively((Control)ctlSettings, enabled);
        }

        private void EnableControlRecursively(Control ctrl, bool enabled)
        {
            WebControl wc = ctrl as WebControl;
            if (wc != null && !(wc is Label))
                wc.Enabled = enabled;

            // process children
            foreach (Control childCtrl in ctrl.Controls)
                EnableControlRecursively(childCtrl, enabled);
        }

        private void SaveSettings()
        {
            try
            {
                PackageSettings settings = new PackageSettings();
                settings.PackageId = PanelSecurity.PackageId;
                settings.SettingsName = SettingsName;

                // set properties
                if (ddlOverride.SelectedIndex == 1)
                {
                    // gather settings from the control
                    // if overriden
                    ctlSettings.SaveSettings(settings);
                }

                int result = ES.Services.Packages.UpdatePackageSettings(settings);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ReturnBack();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_SETTINGS_UPDATE", ex);
                return;
            }
        }

        private void LoadSettingsControl()
        {
            string controlName = Request["SettingsControl"];
            if (!String.IsNullOrEmpty(controlName))
            {
                string currPath = this.AppRelativeVirtualPath;
                currPath = currPath.Substring(0, currPath.LastIndexOf("/"));
                string ctrlPath = currPath + "/" + controlName + ".ascx";

                Control ctrl = Page.LoadControl(ctrlPath);
                ctlSettings = (IPackageSettingsEditorControl)ctrl;
                settingsPlace.Controls.Add(ctrl);
            }
            else
            {
                ReturnBack();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            RedirectSpaceHomePage();
        }
    }
}
