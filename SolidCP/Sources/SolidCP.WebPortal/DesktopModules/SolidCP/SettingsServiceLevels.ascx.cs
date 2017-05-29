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
using System.Xml;
using System.Xml.Serialization;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Common;

namespace SolidCP.Portal
{
    public partial class SettingsServiceLevels : SolidCPControlBase, IUserSettingsEditorControl
    {


        public void BindSettings(UserSettings settings)
        {
            if (PanelSecurity.SelectedUser.Role == UserRole.Administrator)
                BindServiceLevels();
            
            txtStatus.Visible = false;
        
            try
            {
                //Change container title
                ((Label)this.Parent.Parent.Parent.Parent.Parent.FindControl(SolidCP.WebPortal.DefaultPage.MODULE_TITLE_CONTROL_ID)).Text = "Service Levels";
            }
            catch { /*to do*/ }
        }


        private void BindServiceLevels()
        {
            ServiceLevel[] array = ES.Services.Organizations.GetSupportServiceLevels();

            gvServiceLevels.DataSource = array;
            gvServiceLevels.DataBind();

            btnAddServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? true : false;
            btnUpdateServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? false : true;
        }


        public void btnAddServiceLevel_Click(object sender, EventArgs e)
        {
            Page.Validate("CreateServiceLevel");

            if (!Page.IsValid)
                return;

            ServiceLevel serviceLevel = new ServiceLevel();

            int res = ES.Services.Organizations.AddSupportServiceLevel(txtServiceLevelName.Text, txtServiceLevelDescr.Text);

            if (res < 0)
            {
                messageBox.ShowErrorMessage("ADD_SERVICE_LEVEL");

                return;
            }

            txtServiceLevelName.Text = string.Empty;
            txtServiceLevelDescr.Text = string.Empty;

            BindServiceLevels();
        }


        protected void gvServiceLevels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int levelID = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            
            switch (e.CommandName)
            {
                case "DeleteItem":

                    ResultObject result = ES.Services.Organizations.DeleteSupportServiceLevel(levelID);

                    if (!result.IsSuccess)
                    {
                        if (result.ErrorCodes.Contains("SERVICE_LEVEL_IN_USE:Service Level is being used; ")) messageBox.ShowErrorMessage("SERVICE_LEVEL_IN_USE");
                        else messageBox.ShowMessage(result, "DELETE_SERVICE_LEVEL", null);

                        return;
                    }

                    ViewState["ServiceLevelID"] = null;

                    txtServiceLevelName.Text = string.Empty;
                    txtServiceLevelDescr.Text = string.Empty;

                    BindServiceLevels();
                    break;

                case "EditItem":
                    ServiceLevel serviceLevel;

                    ViewState["ServiceLevelID"] = levelID;

                    serviceLevel = ES.Services.Organizations.GetSupportServiceLevel(levelID);

                    txtServiceLevelName.Text = serviceLevel.LevelName;
                    txtServiceLevelDescr.Text = serviceLevel.LevelDescription;

                    btnUpdateServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? false : true;
                    btnAddServiceLevel.Enabled = (string.IsNullOrEmpty(txtServiceLevelName.Text)) ? true : false;
                    break;
            }
        }


        public void SaveSettings(UserSettings settings)
        {
            settings["ServiceLevels"] = "";
        }


        protected void btnUpdateServiceLevel_Click(object sender, EventArgs e)
        {
            Page.Validate("CreateServiceLevel");

            if (!Page.IsValid)
                return;

            if (ViewState["ServiceLevelID"] == null)
                return;

            int levelID = (int)ViewState["ServiceLevelID"];

            ES.Services.Organizations.UpdateSupportServiceLevel(levelID, txtServiceLevelName.Text, txtServiceLevelDescr.Text);

            txtServiceLevelName.Text = string.Empty;
            txtServiceLevelDescr.Text = string.Empty;

            BindServiceLevels();
        }

    }
}
