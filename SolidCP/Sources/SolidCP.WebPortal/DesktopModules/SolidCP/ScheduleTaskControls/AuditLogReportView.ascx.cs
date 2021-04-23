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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Portal.UserControls.ScheduleTaskView;
using SCP = SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ScheduleTaskControls
{
    public partial class AuditLogReportView : EmptyView
    {
        private static readonly string MailToParameter = "MAIL_TO";
        private static readonly string AuditLogSeverityParameter = "AUDIT_LOG_SEVERITY";
        private static readonly string AuditLogSourceParameter = "AUDIT_LOG_SOURCE";
        private static readonly string AuditLogTaskParameter = "AUDIT_LOG_TASK";
        private static readonly string AuditLogDateParameter = "AUDIT_LOG_DATE";
        private static readonly string ShowExecutionLogParameter = "SHOW_EXECUTION_LOG";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets scheduler task parameters on view.
        /// </summary>
        /// <param name="parameters">Parameters list to be set on view.</param>
        public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
        {
            base.SetParameters(parameters);

            SCP.SystemSettings settings = ES.Services.System.GetSystemSettingsActive(SCP.SystemSettings.SMTP_SETTINGS, false);
            if (settings != null)
            {
                txtMailFrom.Text = settings["SmtpUsername"];
            }
            if (String.IsNullOrEmpty(txtMailFrom.Text))
            {
                txtMailFrom.Text = GetLocalizedString("SMTPWarning.Text");
            }
            
            SetParameter(txtMailTo, MailToParameter);
            BindSources();
            SetParameter(ddlAuditLogSource, AuditLogSourceParameter);
            BindSourceTasks();
            SetParameter(ddlAuditLogTask, AuditLogTaskParameter);
            base.SetParameter(ddlAuditLogDate, AuditLogDateParameter);
            base.SetParameter(ddlAuditLogSeverity, AuditLogSeverityParameter);
            base.SetParameter(ddlExecutionLog, ShowExecutionLogParameter);
            foreach (ListItem item in ddlAuditLogDate.Items)
            {
                item.Text = GetLocalizedString(item.Value + ".Text");
            }
            foreach (ListItem item in ddlAuditLogSeverity.Items)
            {
                item.Text = GetLocalizedString(item.Value + ".Severity");
            }
            foreach (ListItem item in ddlExecutionLog.Items)
            {
                item.Text = GetLocalizedString(item.Value + ".ShowExecutionLog");
            }
        }

        private new void SetParameter(DropDownList control, string parameterName)
        {
            ScheduleTaskParameterInfo parameter = FindParameterById(parameterName);
            Utils.SelectListItem(control, parameter.ParameterValue);
        }

        /// <summary>
        /// Gets scheduler task parameters from view.
        /// </summary>
        /// <returns>Parameters list filled  from view.</returns>
        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo mailTo = GetParameter(txtMailTo, MailToParameter);
            ScheduleTaskParameterInfo auditLogSeverity = GetParameter(ddlAuditLogSeverity, AuditLogSeverityParameter);
            ScheduleTaskParameterInfo auditLogSource = GetParameter(ddlAuditLogSource, AuditLogSourceParameter);
            ScheduleTaskParameterInfo auditLogTask = GetParameter(ddlAuditLogTask, AuditLogTaskParameter);
            ScheduleTaskParameterInfo auditLogDate = GetParameter(ddlAuditLogDate, AuditLogDateParameter);
            ScheduleTaskParameterInfo showExecutionLog = GetParameter(ddlExecutionLog, ShowExecutionLogParameter);

            return new ScheduleTaskParameterInfo[6] { mailTo, auditLogSeverity, auditLogSource, auditLogTask, auditLogDate, showExecutionLog };

        }

        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSourceTasks();
        }

        private string GetAuditLogTaskName(string sourceName, string taskName)
        {
            string localizedText = PortalUtils.GetSharedLocalizedString(Utils.ModuleName,
                "AuditLogTask." + sourceName + "_" + taskName);
            return (localizedText != null) ? localizedText : taskName;
        }

        private string GetAuditLogSourceName(string sourceName)
        {
            string localizedText = PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "AuditLogSource." + sourceName);
            return (localizedText != null) ? localizedText : sourceName;
        }

        private string GetLocalizedString(string resourceKey)
        {
            return (string)GetLocalResourceObject(resourceKey);
        }

        private void BindSourceTasks()
        {
            string sourceName = ddlAuditLogSource.SelectedValue;

            ddlAuditLogTask.Items.Clear();
            ddlAuditLogTask.Items.Add(new ListItem(GetLocalizedString("All.Text"), ""));
            DataTable dt = ES.Services.AuditLog.GetAuditLogTasks(sourceName).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string taskName = dr["TaskName"].ToString();
                ddlAuditLogTask.Items.Add(new ListItem(GetAuditLogTaskName(sourceName, taskName), taskName));
            }
        }

        private void BindSources()
        {
            ddlAuditLogSource.Items.Clear();
            ddlAuditLogSource.Items.Add(new ListItem(GetLocalizedString("All.Text"), ""));
            DataTable dt = ES.Services.AuditLog.GetAuditLogSources().Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string sourceName = dr["SourceName"].ToString();
                ddlAuditLogSource.Items.Add(new ListItem(GetAuditLogSourceName(sourceName), sourceName));
            }
        }
    }
}
