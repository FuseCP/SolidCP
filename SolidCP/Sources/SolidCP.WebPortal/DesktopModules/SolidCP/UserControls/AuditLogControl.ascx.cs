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
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using SolidCP.EnterpriseServer;
using SolidCP.Portal;

namespace SolidCP.Portal.UserControls
{
    public partial class AuditLogControl : SolidCPControlBase
    {
        private string logSource;
        public string LogSource
        {
            get { return logSource; }
            set { logSource = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //modalTaskDetailsProperties.Hide();

            // set display preferences
            gvLog.PageSize = UsersHelper.GetDisplayItemsPerPage();

            // grid columns
            gvLog.Columns[4].Visible = String.IsNullOrEmpty(logSource);
            gvLog.Columns[6].Visible = PanelRequest.ItemID == 0;


            if (!IsPostBack)
            {
                try
                {
                    btnClearLog.Visible
                        = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

                    // bind
                    BindPeriod();
                    BindSources();

                    // hide source if required
                    if (!String.IsNullOrEmpty(logSource))
                    {
                        ddlSource.SelectedValue = logSource;
                        SourceRow.Visible = false;
                    }

                    // tasks
                    BindSourceTasks();

                    // hide item name if required
                    if (PanelRequest.ItemID > 0)
                    {
                        ItemNameRow.Visible = false;
                        FilterButtonsRow.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    //ShowErrorMessage("AUDIT_INIT_FORM", ex);
                    HostModule.ProcessException(ex);
                    //this.DisableControls = true;
                    return;
                }
            }
        }

        public string GetIconUrl(int severityID)
        {
            if (severityID == 1)
                return PortalUtils.GetThemedImage("warning_icon_small.gif");
            else if (severityID == 2)
                return PortalUtils.GetThemedImage("error_icon_small.gif");
            else
                return PortalUtils.GetThemedImage("information_icon_small.gif");
        }

        private void BindSources()
        {
            ddlSource.Items.Clear();
            ddlSource.Items.Add(new ListItem(GetLocalizedString("All.Text"), ""));
            DataTable dt = ES.Services.AuditLog.GetAuditLogSources().Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string sourceName = dr["SourceName"].ToString();
                ddlSource.Items.Add(new ListItem(GetAuditLogSourceName(sourceName), sourceName));
            }
        }

        private void BindSourceTasks()
        {
            string sourceName = ddlSource.SelectedValue;

            ddlTask.Items.Clear();
            ddlTask.Items.Add(new ListItem(GetLocalizedString("All.Text"), ""));
            DataTable dt = ES.Services.AuditLog.GetAuditLogTasks(sourceName).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string taskName = dr["TaskName"].ToString();
                ddlTask.Items.Add(new ListItem(GetAuditLogTaskName(sourceName, taskName), taskName));
            }
        }

        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSourceTasks();
        }

        private void BindPeriod()
        {
            if (calPeriod.SelectedDates.Count == 0)
                calPeriod.SelectedDate = DateTime.Now;

            DateTime startDate = calPeriod.SelectedDates[0];
            DateTime endDate = calPeriod.SelectedDates[calPeriod.SelectedDates.Count - 1];

            litPeriod.Text = startDate.ToString("MMM dd, yyyy") +
                " - " + endDate.ToString("MMM dd, yyyy");

            litStartDate.Text = startDate.ToString();
            litEndDate.Text = endDate.ToString();
        }

        private void ExportLog()
        {
            // build HTML
            DataTable dtRecords = ES.Services.AuditLog.GetAuditLogRecordsPaged(PanelSecurity.SelectedUserId,
                PanelSecurity.PackageId, PanelRequest.ItemID, txtItemName.Text.Trim(),
                DateTime.Parse(litStartDate.Text),
                DateTime.Parse(litEndDate.Text),
                Utils.ParseInt(ddlSeverity.SelectedValue, 0),
                ddlSource.SelectedValue, ddlTask.SelectedValue,
                "StartDate ASC", 0, Int32.MaxValue).Tables[1];

            StringBuilder sb = new StringBuilder();

            // header
            sb.AppendLine("Started,Finished,Severity,User-ID,Username,Source,Task,Item-Name,Execution-Log");

            foreach (DataRow dr in dtRecords.Rows)
            {
				// Started
                sb.AppendFormat("\"{0}\",", dr["StartDate"].ToString());
				// Finished
                sb.AppendFormat("\"{0}\",", dr["FinishDate"].ToString());
				// Severity
                sb.AppendFormat("\"{0}\",", 
					GetAuditLogRecordSeverityName((int)dr["SeverityID"]));
				// User-ID
				sb.AppendFormat("\"{0}\",", dr["UserID"]);
				// Username
                sb.AppendFormat("\"{0}\",", dr["Username"]);
                // Source
				sb.AppendFormat("\"{0}\",", 
					GetAuditLogSourceName((string)dr["SourceName"]));
                // Task
				sb.AppendFormat("\"{0}\",", 
					PortalAntiXSS.Encode(GetAuditLogTaskName((string)dr["SourceName"], (string)dr["TaskName"])));
				// Item-Name
                sb.AppendFormat("\"{0}\",", PortalAntiXSS.Encode(dr["ItemName"].ToString()));
				// Execution-Log
				string executionLog = FormatPlainTextExecutionLog(
					dr["ExecutionLog"].ToString(), DateTime.Parse(dr["StartDate"].ToString()));
				//
				executionLog = executionLog.Replace("\"", "\"\"");
				//
				sb.AppendFormat("\"{0}\"", executionLog);
				sb.AppendLine();
            }

            string cleanedPeriod = litPeriod.Text.Replace(" ", "").Replace("/", "-").Replace(",", "-");
            string fileName = "SCP-AuditLog-" + cleanedPeriod + ".csv";

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/ms-excel";

            Response.Write(sb.ToString());

            Response.End();
        }

        private void ClearLog()
        {
            try
            {
                int result = ES.Services.AuditLog.DeleteAuditLogRecords(PanelSecurity.SelectedUserId,
                    0, txtItemName.Text.Trim(),
                    DateTime.Parse(litStartDate.Text),
                    DateTime.Parse(litEndDate.Text),
                    Utils.ParseInt(ddlSeverity.SelectedValue, 0),
                    ddlSource.SelectedValue, ddlTask.SelectedValue);

                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                HostModule.ShowErrorMessage("AUDIT_CLEAR", ex);
                return;
            }
        }

        private void BindRecordDetails(string recordId)
        {
            // load task
            LogRecord record = ES.Services.AuditLog.GetAuditLogRecord(recordId);

            litUsername.Text = record.Username;
            litTaskName.Text = GetAuditLogTaskName(record.SourceName, record.TaskName);
            litSourceName.Text = GetAuditLogSourceName(record.SourceName);
            litItemName.Text = record.ItemName;
            litStarted.Text = record.StartDate.ToString();
            litFinished.Text = record.FinishDate.ToString();

            litDuration.Text = GetDurationText(record.StartDate, record.FinishDate);

            litSeverity.Text = GetAuditLogRecordSeverityName(record.SeverityID);
            litLog.Text = FormatExecutionLog(record.ExecutionLog, record.StartDate);
        }

		private string FormatPlainTextExecutionLog(string xmlLog, DateTime startDate)
		{
			StringBuilder sb = new StringBuilder();
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlLog);

			XmlNodeList nodeRecords = doc.SelectNodes("/log/records/record");

			foreach (XmlNode nodeRecord in nodeRecords)
			{
				// read attributes
				DateTime date = DateTime.MinValue;
				int severity = 0;
				int ident = 0;

				if (nodeRecord.Attributes["date"] != null)
					date = DateTime.Parse(nodeRecord.Attributes["date"].Value,
						System.Globalization.CultureInfo.InvariantCulture);

				if (nodeRecord.Attributes["severity"] != null)
					severity = Int32.Parse(nodeRecord.Attributes["severity"].Value);

				if (nodeRecord.Attributes["ident"] != null)
					ident = Int32.Parse(nodeRecord.Attributes["ident"].Value);

				// Begin audit record
				sb.Append('\t', ident);
				sb.Append("......................");
				sb.AppendLine();
				// Timestamp
				sb.Append('\t', ident);
				sb.AppendFormat("Timestamp: {0}", GetDurationText(startDate, date));
				sb.AppendLine();

				// text
				XmlNode nodeText = nodeRecord.SelectSingleNode("text");

				// text parameters
				string[] prms = new string[0];
				XmlNodeList nodePrms = nodeRecord.SelectNodes("textParameters/value");
				if (nodePrms != null)
				{
					prms = new string[nodePrms.Count];
					for (int i = 0; i < nodePrms.Count; i++)
						prms[i] = nodePrms[i].InnerText;
				}

				// write text
				string recordClass = "Information";
				if (severity == 1)
					recordClass = "Warning";
				else if (severity == 2)
					recordClass = "Error";

				string text = nodeText.InnerText;

				// localize text
				string locText = GetSharedLocalizedString("TaskActivity." + text);
				if (locText != null)
					text = locText;

				// format parameters
				if (prms.Length > 0)
					text = String.Format(text, prms);
				// Severity
				sb.Append('\t', ident);
				sb.AppendFormat(String.Format("Severity: {0}", recordClass));
				sb.AppendLine();
				// Record text
				if (!String.IsNullOrEmpty(text))
				{
					sb.Append('\t', ident);
					sb.Append(text);
					sb.AppendLine();	
				}
				//
				XmlNode nodeStackTrace = nodeRecord.SelectSingleNode("stackTrace");
				// Record stack trace
				if (!String.IsNullOrEmpty(nodeStackTrace.InnerText))
				{
					sb.Append('\t', ident);
					sb.Append(nodeStackTrace.InnerText);
					sb.AppendLine();
				}
				// End audit record
				sb.Append('\t', ident);
				sb.AppendLine();
			}
			// Replace each double-quote with 2*double-quote as per CSV specification.
			// See "http://en.wikipedia.org/wiki/Comma-separated_values#Basic_Rules" for further reference
			return sb.ToString();
		}

        private string FormatExecutionLog(string xmlLog, DateTime startDate)
        {
            StringBuilder sb = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlLog);

            XmlNodeList nodeRecords = doc.SelectNodes("/log/records/record");

            foreach (XmlNode nodeRecord in nodeRecords)
            {
                sb.Append("<div class=\"LogRecord\">");

                // read attributes
                DateTime date = DateTime.MinValue;
                int severity = 0;
                int ident = 0;

                if (nodeRecord.Attributes["date"] != null)
                    date = DateTime.Parse(nodeRecord.Attributes["date"].Value,
                        System.Globalization.CultureInfo.InvariantCulture);

                if (nodeRecord.Attributes["severity"] != null)
                    severity = Int32.Parse(nodeRecord.Attributes["severity"].Value);

                if (nodeRecord.Attributes["ident"] != null)
                    ident = Int32.Parse(nodeRecord.Attributes["ident"].Value);

                // date div
                sb.Append("<div class=\"Time\">");
                sb.Append(GetDurationText(startDate, date));
                sb.Append("</div>");

                // text
                XmlNode nodeText = nodeRecord.SelectSingleNode("text");

                // text parameters
                string[] prms = new string[0];
                XmlNodeList nodePrms = nodeRecord.SelectNodes("textParameters/value");
                if (nodePrms != null)
                {
                    prms = new string[nodePrms.Count];
                    for (int i = 0; i < nodePrms.Count; i++)
                        prms[i] = nodePrms[i].InnerText;
                }

                // write text
                int padding = 80 + ident * 20;
                string recordClass = "Information";
                if (severity == 1)
                    recordClass = "Warning";
                else if (severity == 2)
                    recordClass = "Error";

                string text = nodeText.InnerText;

                // localize text
                string locText = GetSharedLocalizedString("TaskActivity." + text);
                if (locText != null)
                    text = locText;

                if (!String.IsNullOrEmpty(text))
                    text = text.Replace("\n", "<br/>");

                // format parameters
                if (prms.Length > 0)
                    text = String.Format(text, prms);

                sb.Append("<div class=\"").Append(recordClass).Append("\" style=\"padding-left:");
                sb.Append(padding).Append("px;\">").Append(text);

                XmlNode nodeStackTrace = nodeRecord.SelectSingleNode("stackTrace");
                sb.Append("<br/>");
                sb.Append(nodeStackTrace.InnerText.Replace("\n", "<br>"));

                sb.Append("</div></div>");
            }

            return sb.ToString();
        }

        private string GetDurationText(DateTime startDate, DateTime endDate)
        {
            TimeSpan duration = endDate - startDate;
            return String.Format("{0}:{1}:{2}",
                duration.Hours.ToString().PadLeft(2, '0'),
                duration.Minutes.ToString().PadLeft(2, '0'),
                duration.Seconds.ToString().PadLeft(2, '0'));
        }

        protected void calPeriod_SelectionChanged(object sender, EventArgs e)
        {
            BindPeriod();
        }
        protected void odsLog_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                //ShowError(e.Exception.ToString());
                HostModule.ProcessException(e.Exception);
                //this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void btnExportLog_Click(object sender, EventArgs e)
        {
            ExportLog();
        }

        protected void btnClearLog_Click(object sender, EventArgs e)
        {
            ClearLog();

            // rebind grid
            gvLog.DataBind();
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            gvLog.DataBind();
        }

        protected void gvLog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string recordId = (string)e.CommandArgument;
                modalTaskDetailsProperties.Show();
                BindRecordDetails(recordId);
            }
        }
    }
}
