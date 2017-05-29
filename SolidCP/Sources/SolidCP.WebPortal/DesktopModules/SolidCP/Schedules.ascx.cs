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
using System.Text;

namespace SolidCP.Portal
{
    public partial class Schedules : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BindServerTime();

            // set display preferences
            gvSchedules.PageSize = UsersHelper.GetDisplayItemsPerPage();

            if (!IsPostBack)
            {
                
                chkRecursive.Visible = (PanelSecurity.EffectiveUser.Role != UserRole.User);
                // toggle controls
                //btnAddItem.Enabled = PackagesHelper.CheckGroupQuotaEnabled(
                 //   PanelSecurity.PackageId, ResourceGroups.Statistics, Quotas.STATS_SITES);

                searchBox.AddCriteria("ScheduleName", GetLocalizedString("Text.ScheduleName"));
                searchBox.AddCriteria("Username", GetLocalizedString("Text.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("Text.FullName"));
                searchBox.AddCriteria("Email", GetLocalizedString("Text.Email"));

                bool isUser = PanelSecurity.SelectedUser.Role == UserRole.User;
                gvSchedules.Columns[gvSchedules.Columns.Count - 1].Visible = !isUser;
                gvSchedules.Columns[gvSchedules.Columns.Count - 2].Visible = !isUser;
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        protected void odsSchedules_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                e.ExceptionHandled = true;
            }
        }

        /*
        private void BindServerTime()
        {
            try
            {
                litServerTime.Text = ES.Scheduler.GetSchedulerTime().ToString();
            }
            catch
            {
                // skip
            }
        }
         * */

        public string GetScheduleStatus(int statusId)
        {
			return GetSharedLocalizedString(Utils.ModuleName, "ScheduleStatus." + ((ScheduleStatus)statusId).ToString());
        }

        public bool IsScheduleActive(int statusId)
        {
            ScheduleStatus status = (ScheduleStatus)statusId;
            return (status == ScheduleStatus.Running);
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit"));
        }
        protected void gvSchedules_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int scheduleId = Utils.ParseInt(e.CommandArgument.ToString(), 0);
            if (e.CommandName == "start")
            {
                try
                {
                    int result = ES.Services.Scheduler.StartSchedule(scheduleId);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SCHEDULE_START_TASK", ex);
                    return;
                }
            }
            else if (e.CommandName == "stop")
            {
                try
                {
                    int result = ES.Services.Scheduler.StopSchedule(scheduleId);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SCHEDULE_STOP_TASK", ex);
                    return;
                }
            }

            // rebind grid
            gvSchedules.DataBind();
        }

        public string GetSearchBoxAjaxData()
        {
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'Schedules'");
            res.Append(", RedirectUrl: '" + EditUrl("ScheduleID", "{0}", "edit", "SpaceID=" + PanelSecurity.PackageId).Substring(2) + "'");
            res.Append(", PackageID: " + PanelSecurity.PackageId.ToString());
            res.Append(", Recursive: ($('#" + chkRecursive.ClientID + "').val() == 'on')");
            return res.ToString();
        }

        protected void tasksTimer_Tick(object sender, EventArgs e)
        {
            gvSchedules.DataBind();
        }
    }
}
