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

using SolidCP.Providers.Statistics;

namespace SolidCP.Portal
{
    public partial class StatisticsEditStatistics : SolidCPModuleBase
    {
        StatsSite item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ItemID > 0);
            ddlWebSites.Visible = (PanelRequest.ItemID == 0);
            valRequireWebSite.Enabled = (PanelRequest.ItemID == 0);
            btnUpdate.Text = (PanelRequest.ItemID > 0) ? GetLocalizedString("Text.Update") : GetLocalizedString("Text.Add");

            // bind item
            BindItem();
        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    // load item if required
                    if (PanelRequest.ItemID > 0)
                    {
                        // existing item
                        item = ES.Services.StatisticsServers.GetSite(PanelRequest.ItemID);
                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        BindWebSites(PanelSecurity.PackageId);
                    }
                }

                // load provider control
                LoadProviderControl((int)ViewState["PackageId"], "Statistics", providerControl, "EditSite.ascx");

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        lblDomainName.Text = item.Name;

						if (String.Compare(Request["Mode"], "view", true) == 0
							&& !String.IsNullOrEmpty(item.StatisticsUrl))
						{
							// view mode
							Response.Redirect(item.StatisticsUrl);
						}

                        // other controls
                        IStatsEditInstallationControl ctrl = (IStatsEditInstallationControl)providerControl.Controls[0];
                        ctrl.BindItem(item);
                    }
                }

            }
            catch
            {
                ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                DisableFormControls(this, btnCancel);
                return;
            }
        }

        private void BindWebSites(int packageId)
        {
            ddlWebSites.DataSource = ES.Services.WebServers.GetWebSites(packageId, false);
            ddlWebSites.DataBind();
            ddlWebSites.Items.Insert(0, new ListItem("<Select Web Site>", ""));
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
                return;

            // get form data
            StatsSite item = new StatsSite();
            item.Id = PanelRequest.ItemID;
            item.PackageId = PanelSecurity.PackageId;
            item.Name = ddlWebSites.SelectedValue;

            // get other props
            IStatsEditInstallationControl ctrl = (IStatsEditInstallationControl)providerControl.Controls[0];
            ctrl.SaveItem(item);

            if (PanelRequest.ItemID == 0)
            {
                // new item
                try
                {
                    int result = ES.Services.StatisticsServers.AddSite(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("STATS_ADD_STAT", ex);
                    return;
                }
            }
            else
            {
                // existing item
                try
                {
                    int result = ES.Services.StatisticsServers.UpdateSite(item);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("STATS_UPDATE_STAT", ex);
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
                int result = ES.Services.StatisticsServers.DeleteSite(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("STATS_DELETE_STAT", ex);
                return;
            }

            // return
            RedirectSpaceHomePage();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveItem();
        }
    }
}
