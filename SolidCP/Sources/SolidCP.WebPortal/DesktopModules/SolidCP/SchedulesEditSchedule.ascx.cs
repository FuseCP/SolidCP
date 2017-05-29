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
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.Scheduling;
using SolidCP.Portal.Code.Framework;

namespace SolidCP.Portal
{
    public partial class SchedulesEditSchedule : SolidCPModuleBase
    {
        private static readonly string ScheduleViewEnvironment = "ASP.NET";

        private ISchedulerTaskView configurationView;
        private string cachedTaskIdsToLoad = String.Empty;

        public int PackageId
        {
            get { return (int)ViewState["PackageId"]; }
            set { ViewState["PackageId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Visible = (PanelRequest.ScheduleID > 0);

            this.ControlToLoad.Value = this.cachedTaskIdsToLoad;
            if (!IsPostBack)
            {
                try
                {
                    // bind controls
                    BindTasks();

                    // bind schedule
                    BindSchedule();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SCHEDULE_INIT_FORM", ex);
                    return;
                }
            }
        }

        /// <summary>
        /// Overridden. Dynamically loads configuration view.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Make sure control is loaded before view state and post back data are loaded.
            string taskIdsToLoad = HttpContext.Current.Request.Params[this.ControlToLoad.Name];
            if (taskIdsToLoad == null)
            {
                taskIdsToLoad = String.Empty;
            }

            string selectedTaskId = HttpContext.Current.Request.Params[this.ddlTaskType.UniqueID];
            if (!IsPostBack)
            {
                if (PanelRequest.ScheduleID != 0)
                {
                    ScheduleInfo sc = ES.Services.Scheduler.GetSchedule(PanelRequest.ScheduleID);
                    if (sc != null)
                    {
                        selectedTaskId = sc.TaskId;
                    }
                }
            }

            List<string> tasksListToLoad = new List<string>(taskIdsToLoad.Split(new char[] { ';' }));
            if (!String.IsNullOrEmpty(selectedTaskId))
            {
                if (!tasksListToLoad.Contains(selectedTaskId))
                {
                    tasksListToLoad.Add(selectedTaskId);
                }
            }

            foreach (string taskId in tasksListToLoad)
            {
                ISchedulerTaskView view = LoadScheduleTaskConfigurationView(taskId, taskId == selectedTaskId);
                if (taskId == selectedTaskId)
                {
                    this.configurationView = view;
                }
            }

            cachedTaskIdsToLoad = String.Join(";", tasksListToLoad.ToArray());
        }

        /// <summary>
        /// Loads control that is intended to provide user ability to configure schedule task.
        /// </summary>
        /// <remarks>
        /// Returns loaded configuration view.
        /// </remarks>
        private ISchedulerTaskView LoadScheduleTaskConfigurationView(string taskId, bool visible)
        {
            //this.TaskParametersPlaceHolder.Controls.Clear();

            string selectedTaskId = taskId;

            if (!String.IsNullOrEmpty(selectedTaskId))
            {
                // Try to find view configuration
                ScheduleTaskViewConfiguration aspNetEnvironmentViewConfiguration = ES.Services.Scheduler.GetScheduleTaskViewConfiguration(selectedTaskId, ScheduleViewEnvironment);
                // If no configuration found ignore view 
                if (aspNetEnvironmentViewConfiguration == null)
                {
                    return null;
                }
                // Description contains relative path to control to be loaded.
                Control view = this.LoadControl(aspNetEnvironmentViewConfiguration.Description);
                if (!(view is ISchedulerTaskView))
                {
                    // The view does not provide ability to set and get parameters.
                    return null;
                }
                view.ID = taskId;
                view.Visible = visible;
                view.EnableTheming = true;
                this.TaskParametersPlaceHolder.Controls.Add(view);
                return (ISchedulerTaskView)view;
            }
            return null;
        }

        private void BindTasks()
        {
            ScheduleTaskInfo[] tasks = ES.Services.Scheduler.GetScheduleTasks();

            ddlTaskType.Items.Add(new ListItem("<Select Task>", ""));

            foreach (ScheduleTaskInfo task in tasks)
            {
                string localizedTaskName = GetSharedLocalizedString(Utils.ModuleName, "SchedulerTask." + task.TaskId);
                if (localizedTaskName == null)
                    localizedTaskName = task.TaskId;

                ddlTaskType.Items.Add(new ListItem(localizedTaskName, task.TaskId));
            }
        }

        private void BindSchedule()
        {
            txtStartDate.Text = DateTime.Now.ToString("d");
            timeFromTime.SelectedValue = new DateTime(2000, 1, 1, 0, 0, 0);
            timeToTime.SelectedValue = new DateTime(2000, 1, 1, 23, 59, 59);
            intMaxExecutionTime.Interval = 3600;

            if (PanelRequest.ScheduleID == 0)
            {
                ApplyPackageContextRestrictions(PanelSecurity.PackageId);
                PackageId = PanelSecurity.PackageId;
            }
            else
            {

                ScheduleInfo sc = ES.Services.Scheduler.GetSchedule(PanelRequest.ScheduleID);
                if (sc == null)
                    return;

                ApplyPackageContextRestrictions(sc.PackageId);
                PackageId = sc.PackageId;

                txtTaskName.Text = sc.ScheduleName;

                Utils.SelectListItem(ddlTaskType, sc.TaskId);

                Utils.SelectListItem(ddlSchedule, sc.ScheduleTypeId);
                timeFromTime.SelectedValue = sc.FromTime;
                timeToTime.SelectedValue = sc.ToTime;

                timeStartTime.SelectedValue = sc.StartTime;
                intInterval.Interval = sc.Interval;

                // run once
                if (ddlSchedule.SelectedIndex == 3)
                {
                    txtStartDate.Text = sc.StartTime.ToString("d");
                }

                txtWeekDay.Text = sc.WeekMonthDay.ToString();
                txtMonthDay.Text = sc.WeekMonthDay.ToString();

                chkEnabled.Checked = sc.Enabled;
                Utils.SelectListItem(ddlPriority, sc.PriorityId);
                intMaxExecutionTime.Interval = sc.MaxExecutionTime;
            }


            // bind schedule parameters
            BindScheduleParameters();

            // toggle
            ToggleControls();
        }

        private void ApplyPackageContextRestrictions(int packageId)
        {
            // load context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);

            bool intervalTasksAllowed = (cntx.Quotas.ContainsKey(Quotas.OS_SCHEDULEDINTERVALTASKS)
                && cntx.Quotas[Quotas.OS_SCHEDULEDINTERVALTASKS].QuotaAllocatedValue != 0);
            if (!intervalTasksAllowed)
                ddlSchedule.Items.Remove(ddlSchedule.Items.FindByValue("Interval"));

            // check if this an admin
            if (PanelSecurity.LoggedUser.Role != UserRole.Administrator)
            {
                // remove "high" priorities
                ddlPriority.Items.Remove(ddlPriority.Items.FindByValue("Highest"));
                ddlPriority.Items.Remove(ddlPriority.Items.FindByValue("AboveNormal"));
                ddlPriority.Items.Remove(ddlPriority.Items.FindByValue("Normal"));
            }
        }

        /// <summary>
        /// Binds schedule task parameters to configuration view.
        /// </summary>
        private void BindScheduleParameters()
        {
            ScheduleTaskParameterInfo[] parameters = ES.Services.Scheduler.GetScheduleParameters(ddlTaskType.SelectedValue,
                PanelRequest.ScheduleID);

            gvTaskParameters.DataSource = parameters;
            gvTaskParameters.DataBind();

            if (this.configurationView != null)
            {
                this.configurationView.SetParameters(parameters);
            }
        }

        protected void gvTaskParameters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ParameterEditor txtValue = (ParameterEditor)e.Row.FindControl("txtValue");
            if (txtValue == null)
                return;

            ScheduleTaskParameterInfo prm = (ScheduleTaskParameterInfo)e.Row.DataItem;
            txtValue.DataType = prm.DataTypeId;
            txtValue.DefaultValue = prm.DefaultValue;
            txtValue.Value = prm.ParameterValue;
        }

        public string GetHistoryFinishTime(DateTime dt)
        {
            return (dt == DateTime.MinValue) ? "" : dt.ToString();
        }

        private void ToggleControls()
        {
            tblWeekly.Visible = (ddlSchedule.SelectedIndex == 1);
            tblMonthly.Visible = (ddlSchedule.SelectedIndex == 2);
            tblOneTime.Visible = (ddlSchedule.SelectedIndex == 3);
            tblInterval.Visible = (ddlSchedule.SelectedIndex == 4);
            timeStartTime.Enabled = (ddlSchedule.SelectedIndex != 4);
        }

        protected void ddlSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void ddlTaskType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.configurationView = this.LoadScheduleTaskConfigurationView(this.ddlTaskType.SelectedValue);
            BindScheduleParameters();
        }

        private void SaveTask()
        {
            // gather form parameters
            ScheduleInfo sc = new ScheduleInfo();
            sc.ScheduleId = PanelRequest.ScheduleID;
            sc.ScheduleName = txtTaskName.Text.Trim();
            sc.TaskId = ddlTaskType.SelectedValue;

            sc.PackageId = PanelSecurity.PackageId;

            sc.ScheduleTypeId = ddlSchedule.SelectedValue;
            sc.FromTime = timeFromTime.SelectedValue;
            sc.ToTime = timeToTime.SelectedValue;

            sc.StartTime = timeStartTime.SelectedValue;
            sc.Interval = intInterval.Interval;

            // check maximum interval
            // load context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PackageId);
            if (cntx.Quotas.ContainsKey(Quotas.OS_MINIMUMTASKINTERVAL))
            {
                int minInterval = cntx.Quotas[Quotas.OS_MINIMUMTASKINTERVAL].QuotaAllocatedValue;
                if (minInterval != -1 && sc.Interval < (minInterval * 60))
                    sc.Interval = (minInterval * 60);
            }

            // run once
            if (ddlSchedule.SelectedIndex == 3)
            {
                DateTime tm = timeStartTime.SelectedValue;
                DateTime dt = DateTime.Parse(txtStartDate.Text);
                DateTime startTime = new DateTime(dt.Year, dt.Month, dt.Day, tm.Hour, tm.Minute, tm.Second);
                sc.StartTime = startTime;
            }

            sc.WeekMonthDay = Utils.ParseInt(txtWeekDay.Text, 0);
            if (ddlSchedule.SelectedIndex == 2)
                sc.WeekMonthDay = Utils.ParseInt(txtMonthDay.Text, 0);


            sc.Enabled = chkEnabled.Checked;
            sc.PriorityId = ddlPriority.SelectedValue;
            sc.HistoriesNumber = 0;
            sc.MaxExecutionTime = intMaxExecutionTime.Interval;

            // gather parameters
            List<ScheduleTaskParameterInfo> parameters = new List<ScheduleTaskParameterInfo>();
            foreach (GridViewRow row in gvTaskParameters.Rows)
            {
                ParameterEditor txtValue = (ParameterEditor)row.FindControl("txtValue");
                if (txtValue == null)
                    continue;

                string prmId = (string)gvTaskParameters.DataKeys[row.RowIndex][0];

                ScheduleTaskParameterInfo parameter = new ScheduleTaskParameterInfo();
                parameter.ParameterId = prmId;
                parameter.ParameterValue = txtValue.Value;
                parameters.Add(parameter);
            }

            sc.Parameters = parameters.ToArray();

            // Gather parameters from view.
            if (this.configurationView != null)
            {
                sc.Parameters = this.configurationView.GetParameters();
            }

            // save
            if (PanelRequest.ScheduleID == 0)
            {
                // add new schedule
                try
                {
                    int result = ES.Services.Scheduler.AddSchedule(sc);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SCHEDULE_ADD_TASK", ex);
                    return;
                }
            }
            else
            {
                // update existing
                try
                {
                    int result = ES.Services.Scheduler.UpdateSchedule(sc);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SCHEDULE_UPDATE_TASK", ex);
                    return;
                }
            }

            // redirect
            RedirectSpaceHomePage();
        }

        private void DeleteTask()
        {
            try
            {
                // delete
                if (PanelRequest.ScheduleID == 0)
                    return;

                // delete schedule
                int result = ES.Services.Scheduler.DeleteSchedule(PanelRequest.ScheduleID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                // redirect
                RedirectSpaceHomePage();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("SCHEDULE_DELETE_TASK", ex);
                return;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveTask();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteTask();
        }
    }
}