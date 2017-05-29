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
    public partial class TasksTaskDetails : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindTask();
        }

        private void BindTask()
        {
            DateTime lastLogDate = DateTime.MinValue;
            if (ViewState["lastLogDate"] != null)
                lastLogDate = (DateTime)ViewState["lastLogDate"];

            BackgroundTask task = ES.Services.Tasks.GetTaskWithLogRecords(PanelRequest.TaskID, lastLogDate);
            if (task == null)
                RedirectToBrowsePage();

            // bind task details
            litTitle.Text = String.Format("{0} &quot;{1}&quot;",
                GetAuditLogTaskName(task.Source, task.TaskName),
                task.ItemName);
            litStep.Text = LocalizeActivityText(task.GetLogs().Count > 0 ? task.GetLogs()[0].Text : String.Empty);
            litStartTime.Text = task.StartDate.ToString();

            // progress
            int percent = 0;
            if (task.IndicatorMaximum > 0)
                percent = task.IndicatorCurrent * 100 / task.IndicatorMaximum;
            pnlProgressBar.Width = Unit.Percentage(percent);

            // duration
            litDuration.Text = GetDurationText(task.StartDate, DateTime.Now);

            // execution log
            StringBuilder log = new StringBuilder();
            if (task.GetLogs().Count > 0)
                ViewState["lastLogDate"] = task.GetLogs()[0].Date.AddTicks(1);



            foreach (BackgroundTaskLogRecord logRecord in task.GetLogs())
            {
                log.Append("[").Append(GetDurationText(task.StartDate, logRecord.Date)).Append("] ");
                log.Append(GetLogLineIdent(logRecord.TextIdent));
                log.Append(LocalizeActivityText(logRecord.Text));
                log.Append("<br>");
            }
            litLog.Text = log.ToString();//+ litLog.Text;

            if(task.Completed)
                btnStop.Visible = false;
        }

        private string LocalizeActivityText(string text)
        {
            // localize text
            string locText = GetSharedLocalizedString("TaskActivity." + text);
            if (locText != null)
                return locText;

            return text;
        }

        private string GetLogLineIdent(int ident)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ident; i++)
                sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
            return sb.ToString();
        }

        private string GetDurationText(DateTime startDate, DateTime endDate)
        {
            TimeSpan duration = (TimeSpan)(endDate - startDate);
            return String.Format("{0}:{1}:{2}",
                duration.Hours.ToString().PadLeft(2, '0'),
                duration.Minutes.ToString().PadLeft(2, '0'),
                duration.Seconds.ToString().PadLeft(2, '0'));
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            RedirectToBrowsePage();
        }

        protected void btnStop_Click(object sender, EventArgs e)
        {
            // stop task
            ES.Services.Tasks.StopTask(PanelRequest.TaskID);

            // hide stop button
            btnStop.Visible = false;
        }
    }
}
