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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class InstallerApplications : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindCategories();
                }
            }
            catch(Exception ex)
            {
                ProcessException(ex);
                //this.DisableControls = true;
                //ShowErrorMessage("USERWIZARD_INIT_FORM", ex);
                return;
            }
        }

        private void BindCategories()
        {
            ddlCategory.DataSource = ES.Services.ApplicationsInstaller.GetCategories();
            ddlCategory.DataBind();

            // add empty
            ddlCategory.Items.Insert(0, new ListItem(GetLocalizedString("SelectCategory.Text"), ""));
        }

        public string FormatRequirements(object objApp)
        {
            ApplicationInfo app = (ApplicationInfo)objApp;
            List<string> reqParts = new List<string>();
            if (app.Requirements != null)
            {
                foreach (ApplicationRequirement req in app.Requirements)
                {
                    if (!req.Display)
                        continue;

                    // process groups
                    if(req.Groups != null)
                    {
                        string[] groupsArray = new string[req.Groups.Length];
                        for(int i = 0; i < groupsArray.Length; i++)
							groupsArray[i] = GetSharedLocalizedString(Utils.ModuleName, "ResourceGroup." + req.Groups[i]);

                        reqParts.Add(String.Join(GetLocalizedString("Or.Text"), groupsArray));
                    }

                    // process quotas
                    if(req.Quotas != null)
                    {
                        string[] quotasArray = new string[req.Quotas.Length];
                        for(int i = 0; i < quotasArray.Length; i++)
							quotasArray[i] = GetSharedLocalizedString(Utils.ModuleName, "Quota." + req.Quotas[i]);

                        reqParts.Add(String.Join(GetLocalizedString("Or.Text"), quotasArray));
                    }
                }
            }
         
            return (reqParts.Count == 0) ? GetLocalizedString("None.Text") : String.Join(", ", reqParts.ToArray());
        }

        protected void odsApplications_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                //this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Install")
                Response.Redirect(EditUrl("ApplicationID", e.CommandArgument.ToString(), "edit",
                    "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }
    }
}
