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
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Database;

namespace SolidCP.Portal
{
    public partial class SqlEditDatabase : SolidCPModuleBase
    {
        SqlDatabase item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
			// check for "View" mode
			if (String.Compare(Request["Mode"], "view", true) == 0)
			{
				// load database
				try
				{
					item = ES.Services.DatabaseServers.GetSqlDatabase(PanelRequest.ItemID);

					if (item.Users != null && item.Users.Length > 0)
					{
						DatabaseBrowserConfiguration config = ES.Services.DatabaseServers.GetDatabaseBrowserLogonScript(
							PanelSecurity.PackageId, item.GroupName, item.Users[0]);

						if (String.Compare(config.Method, "get", true) == 0)
						{
							Response.Redirect(config.GetData, true);
						}
						else
						{
							Response.Clear();
							Response.Write(config.PostData);
							Response.End();
						}
					}

				}
				catch (Exception ex)
				{
					ShowErrorMessage("SQL_GET_DATABASE", ex);
					return;
				}
			}



            bool editMode = (PanelRequest.ItemID > 0);
            btnDelete.Visible = editMode;

            BindItem();
        }

        private void BindUsers(int packageId)
        {
            SqlUser[] users = ES.Services.DatabaseServers.GetSqlUsers(packageId,
                SqlDatabases.GetDatabasesGroupName(Settings), false);

            dlUsers.DataSource = users;
            dlUsers.DataBind();
        }

        private void BindItem()
        {
            var policyName = "";
            if (SqlDatabases.GetDatabasesGroupName(Settings).ToLower().StartsWith("mssql")) { policyName = "MsSqlPolicy"; }
            else if (SqlDatabases.GetDatabasesGroupName(Settings).ToLower().StartsWith("mysql")) { policyName = "MySqlPolicy"; }
            else { policyName = "MariaDBPolicy"; }

            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        try
                        {
                            item = ES.Services.DatabaseServers.GetSqlDatabase(PanelRequest.ItemID);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("SQL_GET_DATABASE", ex);
                            return;
                        }

                        if (item != null)
                        {
                            
                            if (!string.IsNullOrEmpty(item.ExternalServerName))
                            {
                                lblDBExternalServer.Visible =litDBExternalServer.Visible = true;
                                litDBExternalServer.Text = item.ExternalServerName;
                            }

                            if (!string.IsNullOrEmpty(item.InternalServerName))
                            {
                                lblDBInternalServer.Visible = litDBInternalServer.Visible = true;
                                litDBInternalServer.Text = item.InternalServerName;
                            }

                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                            usernameControl.SetPackagePolicy(item.PackageId, policyName, "DatabaseNamePolicy");
                            BindUsers(item.PackageId);
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        lblDBExternalServer.Visible = lblDBInternalServer.Visible = false;
                        litDBExternalServer.Visible = litDBInternalServer.Visible = false;

                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        usernameControl.SetPackagePolicy(PanelSecurity.PackageId, policyName, "DatabaseNamePolicy");
                        BindUsers(PanelSecurity.PackageId);
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], SqlDatabases.GetDatabasesGroupName(Settings),
                    providerControl, "EditDatabase.ascx");

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        usernameControl.Text = item.Name;
                        usernameControl.EditMode = true;

                        foreach (string user in item.Users)
                        {
                            ListItem li = dlUsers.Items.FindByValue(user);
                            if (li != null)
                                li.Selected = true;
                        }

                        // other controls
                        IDatabaseEditDatabaseControl ctrl = (IDatabaseEditDatabaseControl)providerControl.Controls[0];
                        ctrl.BindItem(item);
                    }
                }
            }
            catch(Exception ex)
            {
                ShowErrorMessage("INIT_SERVICE_ITEM_FORM", ex);
                DisableFormControls(this, btnCancel);
                return;
            }
        }

        
        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            SqlDatabase item = new SqlDatabase();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = usernameControl.Text;
            
            List<string> users = new List<string>();
            foreach (ListItem li in dlUsers.Items)
            {
                if (li.Selected)
                    users.Add(li.Value);
            }
            item.Users = users.ToArray();

            // get other props
            IDatabaseEditDatabaseControl ctrl = (IDatabaseEditDatabaseControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.DatabaseServers.AddSqlDatabase(item, SqlDatabases.GetDatabasesGroupName(Settings));
					// Show an error message if the operation has failed to complete
                    if (result < 0)
                    {
                        ShowResultMessageWithContactForm(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SQL_ADD_DATABASE", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.DatabaseServers.UpdateSqlDatabase(item);
					// Show an error message if the operation has failed to complete
                    if (result < 0)
                    {
						ShowResultMessageWithContactForm(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SQL_UPDATE_DATABASE", ex);
                    return;
                }
            }

            // return
            RedirectSpaceHomePage();
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.DatabaseServers.DeleteSqlDatabase(PanelRequest.ItemID);
				// Show an error message if the operation has failed to complete
                if (result < 0)
                {
					ShowResultMessageWithContactForm(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SQL_DELETE_DATABASE", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
    }
}
