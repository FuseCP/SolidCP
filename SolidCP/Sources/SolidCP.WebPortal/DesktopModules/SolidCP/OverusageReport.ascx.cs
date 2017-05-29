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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;

using Microsoft.Reporting.WebForms;

using SolidCP.EnterpriseServer;
using SolidCP.Portal;
using SolidCP.Portal.ReportingServices;
using System.Text;
using System.Globalization;

namespace SolidCP.Portal
{
	public partial class OverusageReport : SolidCPModuleBase
	{
		#region Data
		public const string OverusageReportName = "OverusageReport";
		public const string DiskspaceDetailsReportName = "HostingSpaceDiskspaceOverusageDetails";
		public const string BandwidthDetailsReportName = "HostingSpaceBandwidthOverusageDetails";
		
		public const string ExcelExportTypeName = "Excel";
		public const string PDFExportTypeName = "PDF";

		public const string ParameterBandwidthStartDate = "BandwidthStartDate";
		public const string ParameterBandwidthEndDate = "BandwidthEndDate";

		public const int NonExistingPackage = -1;
		#endregion

		/// <summary>
		/// Loads client JavaScript that binds toolbar and <see cref="ReportViewer"/>
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			RenderToolbarClientScript();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (!IsPostBack)
				{
					BindToolbar();
					BindReport(OverusageReportName, rvContent.LocalReport);
					BindExportButtons(rvContent.LocalReport);
				}
			}
			catch (Exception ex)
			{
				ShowWarningMessage(ex.Message);
			}
		}

		/// <summary>
		/// Initiates report refresh with changes parameters from toolbar.
		/// </summary>
		/// <param name="sender">Report refresh button</param>
		/// <param name="e">Corresponding event arguments</param>
		protected void OnRefreshButtonClick(object sender, EventArgs e)
		{
		    DateTime startDate = startDateCalendar.SelectedDate;
		    DateTime endDate = endDateCalendar.SelectedDate;
            if (startDate > endDate)
            {
                ShowWarningMessage("START_END_DATE_VALIDATION");
                return;
            }
			BindReport(rvContent.LocalReport.DisplayName, rvContent.LocalReport);
			BindExportButtons(rvContent.LocalReport);
		}

		#region Report Viewer Events
		protected void rvContent_Init(object sender, EventArgs e)
		{
		}

		protected void rvContent_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
		{
			BindReport(
				  (sender as ReportViewer).LocalReport.DisplayName
				, (sender as ReportViewer).LocalReport
			);
			BindExportButtons((sender as ReportViewer).LocalReport);
		}

		protected void rvContent_Drillthrough(object sender, DrillthroughEventArgs e)
		{
			BindReport(e.ReportPath, e.Report as LocalReport);
			BindExportButtons(e.Report as LocalReport);
		}

		protected void rvContent_ReportError(object sender, ReportErrorEventArgs e)
		{
			ShowWarningMessage(e.Exception.Message);

			e.Handled = true;
		}
		#endregion

		#region Toolbar binding functionality
		/// <summary>
		/// Generates start and end dates for the bandwidht report.
		/// </summary>
		protected void BindToolbar()
		{
			startDateCalendar.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			endDateCalendar.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
		}

		/// <summary>
		/// Assigns export URLs for buttons
		/// </summary>
		protected void BindExportButtons(LocalReport report)
		{
			exportToExcel.NavigateUrl = ReportingServicesUtils
										.GetReportExportUrl(
											  report
											, rvContent
											, rvContent.ExportContentDisposition
										) + ExcelExportTypeName;

			exportToPdf.NavigateUrl = ReportingServicesUtils
										.GetReportExportUrl(
											  report
											, rvContent
											, rvContent.ExportContentDisposition
										) + PDFExportTypeName;
		}
		#endregion

		#region Load Reports Functionality
		/// <summary>
		/// Used to load all reports on the page.
		/// </summary>
		/// <param name="reportName">Name or the report to load.</param>
		/// <param name="localReport">
		/// Instance or <see cref="LocalReport"/> class. 
		/// This instance serves as a container for report being loaded.
		/// </param>
		protected void BindReport(string reportName, LocalReport report)
		{
			switch (reportName)
			{
				case OverusageReportName:
					BindOverusageSummaryReport(reportName, report);
					break;

				case DiskspaceDetailsReportName:
					BindHostingSpaceDiskspaceOverusageDetailsReport(reportName, report);
					break;

				case BandwidthDetailsReportName:
					BindHostingSpaceBandwidthOverusageDetailsReport(reportName, report);
					break;

				default:
					throw new InvalidOperationException(
						String.Format("Unable to load report '{0}'. Corresponding functionality does not exist.", reportName)
						);
			}
		}

		/// <summary>
		/// Loads Overusage summary report.
		/// </summary>
		/// <param name="reportName">Name of overusage summary report.</param>
		/// <param name="localReport">
		/// Instance or <see cref="LocalReport"/> class. 
		/// This instance serves as a container for report being loaded.
		/// </param>
		protected void BindOverusageSummaryReport(string reportName, LocalReport localReport)
		{
			// 1. Localize report
			localReport.DisplayName = reportName;
			localReport.LoadReportDefinition(
					ReportingServicesUtils.LoadReportFromFile(
						  GetReportFullPath(reportName)
						, reportName
						, this
					)
				);

			// 2. Update parameters
			List<ReportParameter> parameters = new List<ReportParameter>();
			parameters.Add(
					new ReportParameter(
						  ParameterBandwidthStartDate
						, startDateCalendar.SelectedDate.ToString()
					)
				);
			parameters.Add(
					new ReportParameter(
						  ParameterBandwidthEndDate
						, endDateCalendar.SelectedDate.ToString()
					)
				);

			localReport.SetParameters(parameters);

			// 3. Update DataSet
			DataSet report = ES.Services.Packages
							 .GetOverusageSummaryReport(
								  PanelSecurity.SelectedUserId
								, PanelSecurity.PackageId
								, startDateCalendar.SelectedDate
								, endDateCalendar.SelectedDate
							 );

			localReport.DataSources.Clear();

			TranslateStatusField(report.Tables["HostingSpace"]);

			// If you open reports DataSet file in XML and file <DataSets> node
			// you will see the same names as applied to ReportDataSource(name, value) instances below
			LoadDiskspaceOverusageData(report.Tables["HostingSpace"], report.Tables["DiskspaceOverusage"]);
			LoadBandwidthOverusageData(report.Tables["HostingSpace"], report.Tables["BandwidthOverusage"]);
			//
			BindDataTableToReport(localReport, "OverusageReport_HostingSpace", report.Tables["HostingSpace"]);
			BindDataTableToReport(localReport, "OverusageReport_DiskspaceOverusage", report.Tables["DiskspaceOverusage"]);
			BindDataTableToReport(localReport, "OverusageReport_BandwidthOverusage", report.Tables["BandwidthOverusage"]);
			BindDataTableToReport(localReport, "OverusageReport_OverusageDetails", report.Tables["OverusageDetails"]);

			localReport.Refresh();
		}

		/// <summary>
		/// Load a detailed diskspace report.
		/// </summary>
		/// <param name="reportName">Name of detailed diskspace report.</param>
		/// <param name="localReport">
		/// Instance or <see cref="LocalReport"/> class. 
		/// This instance serves as a container for report being loaded.
		/// </param>
		protected void BindHostingSpaceDiskspaceOverusageDetailsReport(string reportName, LocalReport localReport)
		{
			// 1. Localize report
			localReport.DisplayName = reportName;
			localReport.LoadReportDefinition(
					ReportingServicesUtils.LoadReportFromFile(
						  GetReportFullPath(reportName)
						, reportName
						, this
					)
				);

			// 2. Update parameters
			//    Note: here we are always in Drill-through mode.
			localReport.SetParameters(localReport.OriginalParametersToDrillthrough);
			string hostingSpaceId = localReport.GetParameters()["HostingSpaceId"].Values[0];


			// 3. Update DataSet
			DataSet report = ES.Services.Packages
							 .GetDiskspaceOverusageDetailsReport(
								  PanelSecurity.SelectedUserId
								, int.Parse(hostingSpaceId)
							 );

			localReport.DataSources.Clear();

			TranslateStatusField(report.Tables["HostingSpace"]);

			BindDataTableToReport(localReport, "OverusageReport_HostingSpace", report.Tables["HostingSpace"]);
			BindDataTableToReport(localReport, "OverusageReport_DiskspaceOverusage", report.Tables["DiskspaceOverusage"]);
			BindDataTableToReport(localReport, "OverusageReport_OverusageDetails", report.Tables["OverusageDetails"]);

			localReport.Refresh();
		}

		protected void BindHostingSpaceBandwidthOverusageDetailsReport(string reportName, LocalReport localReport)
		{
			// 1. Localize report
			localReport.DisplayName = reportName;
			localReport.LoadReportDefinition(
					ReportingServicesUtils.LoadReportFromFile(
						  GetReportFullPath(reportName)
						, reportName
						, this
					)
				);

			// 2. Update parameters
			//    Note: here we are always in Drill-through mode.
			localReport.SetParameters(localReport.OriginalParametersToDrillthrough);
			string hostingSpaceId = localReport.GetParameters()["HostingSpaceId"].Values[0];

			List<ReportParameter> parameters = new List<ReportParameter>();
			parameters.Add(
					new ReportParameter(
						  ParameterBandwidthStartDate
						, startDateCalendar.SelectedDate.ToString()
					)
				);
			parameters.Add(
					new ReportParameter(
						  ParameterBandwidthEndDate
						, endDateCalendar.SelectedDate.ToString()
					)
				);

			localReport.SetParameters(parameters);

			//3. Update data
			DataSet ds = ES.Services.Packages
						 .GetBandwidthOverusageDetailsReport(
							  PanelSecurity.SelectedUserId
							, int.Parse(hostingSpaceId)
							, startDateCalendar.SelectedDate
							, endDateCalendar.SelectedDate
						 );

			localReport.DataSources.Clear();

			TranslateStatusField(ds.Tables["HostingSpace"]);

			BindDataTableToReport(localReport, "OverusageReport_HostingSpace", ds.Tables["HostingSpace"]);
			BindDataTableToReport(localReport, "OverusageReport_BandwidthOverusage", ds.Tables["BandwidthOverusage"]);
			BindDataTableToReport(localReport, "OverusageReport_OverusageDetails", ds.Tables["OverusageDetails"]);

			localReport.Refresh();
		}
		#endregion

		#region Facility Functions
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostingSpace"></param>
		/// <param name="bwOverusage"></param>
		private void LoadBandwidthOverusageData(DataTable hostingSpace, DataTable bwOverusage)
		{
			if (hostingSpace != null)
			{
				hostingSpace.Columns.Add("BandwidthUsed", typeof(long));
				hostingSpace.Columns.Add("BandwidthAllotted", typeof(long));
				//
				if (bwOverusage != null)
				{
					foreach (DataRow drow in hostingSpace.Rows)
					{
						DataRow[] nqResults = bwOverusage.Select(String.Format("HostingSpaceId={0}", drow["HostingSpaceId"]));
						if (nqResults != null && nqResults.Length == 1)
						{
							drow["BandwidthUsed"] = nqResults[0]["Used"];
							drow["BandwidthAllotted"] = nqResults[0]["Allocated"];
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hostingSpace"></param>
		/// <param name="diskspaceOverusage"></param>
		private void LoadDiskspaceOverusageData(DataTable hostingSpace, DataTable dsOverusage)
		{
			if (hostingSpace != null)
			{
				hostingSpace.Columns.Add("DiskspaceUsed", typeof(long));
				hostingSpace.Columns.Add("DiskspaceAllotted", typeof(long));
				//
				if (dsOverusage != null)
				{
					foreach (DataRow drow in hostingSpace.Rows)
					{
						DataRow[] nqResults = dsOverusage.Select(String.Format("HostingSpaceId={0}", drow["HostingSpaceId"]));
						if (nqResults != null && nqResults.Length == 1)
						{
							drow["DiskspaceUsed"] = nqResults[0]["Used"];
							drow["DiskspaceAllotted"] = nqResults[0]["Allocated"];
						}
					}
				}
			}
		}
		/// <summary>
		/// Adds a <paramref name="datasource"/> to the reports' <see cref="DataSet"/>.
		/// </summary>
		/// <param name="localReport">A reference to a local report instance.</param>
		/// <param name="datasourceName">Name of the data set report uses.</param>
		/// <param name="datasource">The datasource representing <paramref name="datasourceName"/> data set.</param>
		private void BindDataTableToReport(LocalReport localReport, string datasourceName, DataTable datasource)
		{
			localReport.DataSources.Add(
					new ReportDataSource(datasourceName, datasource)
				);
		}

		/// <summary>
		/// Determines the full path to the Report on the disk
		/// </summary>
		/// <param name="reportName">Name of report without extension and relative path</param>
		/// <returns>Full path to the report file</returns>
		protected string GetReportFullPath(string reportName)
		{
			return HttpContext.Current.Server.MapPath(
				String.Format("~/DesktopModules/SolidCP/Reports/{0}.rdlc", reportName)
				);
		}

		/// <summary>
		/// For each row in the table translates status from Id to Value. For example from 1 to Active and so on.
		/// It uses <see cref="PanelFormatter"/>s GetPackageStatusName method for translation purposes.
		/// </summary>
		/// <param name="dt">Table containing rows with Hosting Space information.</param>
		protected void TranslateStatusField(DataTable dt)
		{
			foreach (DataRow row in dt.Rows)
			{
				int statusId = 0;
				if (int.TryParse(row["Status"].ToString(), out statusId))
				{
					row["Status"] = PanelFormatter.GetPackageStatusName(statusId);
				}
			}
		}

		#endregion

		#region JavaScript functionality
		/// <summary>
		/// This function renders client JavaScript that utilizes 
		/// Overusage Report toolbar functionality
		/// </summary>
		protected void RenderToolbarClientScript()
		{
			if (!Page.ClientScript.IsClientScriptBlockRegistered("overusageReportToolbarScript"))
			{
				StringBuilder jsFunctionality = new StringBuilder();

				jsFunctionality.AppendLine("<script>");
				jsFunctionality.AppendLine("<!-- ");
				jsFunctionality.AppendLine("/* Begin: Client Script for Overusage Report */");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("var monthes = {");
				string delimiter = " ";
				int monthNumber = 0;
				foreach (string monthName in CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames)
				{
					if (!String.IsNullOrEmpty(monthName))
					{
						jsFunctionality.AppendFormat("{0}{1} : '{2}'", delimiter, monthNumber, monthName);
						jsFunctionality.AppendLine(String.Empty);

						monthNumber++;
					}

					delimiter = ",";
				}
				jsFunctionality.AppendLine("};");
				jsFunctionality.AppendLine(String.Empty);

				jsFunctionality.AppendLine("function GetMonthNameFromNumber(currentMonth) {");
				jsFunctionality.AppendLine("  return monthes[currentMonth];");
				jsFunctionality.AppendLine("}");
				jsFunctionality.AppendLine(String.Empty);

				jsFunctionality.AppendLine("function SetCurrentMonthText(currentDate) {");
				jsFunctionality.AppendFormat("  $get('currentMonth').setAttribute('year', currentDate.getFullYear()); ");
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendFormat("  $get('currentMonth').setAttribute('month', currentDate.getMonth()); ");
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendFormat("  $get('currentMonth').setAttribute('text', GetMonthNameFromNumber(currentDate.getMonth())); ");
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendFormat("  $get('currentMonth').setAttribute('innerHTML', GetMonthNameFromNumber(currentDate.getMonth()));");
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("}");
				jsFunctionality.AppendLine(String.Empty);

				jsFunctionality.AppendLine("function GetReportViewerControllerInstance() {");
				jsFunctionality.AppendFormat("  return ClientController{0};", rvContent.ClientID);
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("}");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("function RefreshReport() {");
				jsFunctionality.AppendLine("  GetReportViewerControllerInstance().ActionHandler('Refresh', '');");
				jsFunctionality.AppendLine("}");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("function OnStartDateChanged(sender, e) {");
				jsFunctionality.AppendLine("  SetCurrentMonthText( sender.get_selectedDate() );");
				jsFunctionality.AppendLine("}");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("function GetStartDate(year, month) {");
				jsFunctionality.AppendLine("  return new Date(year, month, 1);");
				jsFunctionality.AppendLine("}");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("function GetEndDate(year, month) {");
				jsFunctionality.AppendLine("  var date = GetStartDate(year, month);");
				jsFunctionality.AppendLine("  var thisMonth = date.getMonth();");
				jsFunctionality.AppendLine("  while(date.getMonth() == thisMonth) {");
				jsFunctionality.AppendLine("    date = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);");
				jsFunctionality.AppendLine("  }");
				jsFunctionality.AppendLine("  return new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1);");
				jsFunctionality.AppendLine("}");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("function PreviousMonth() {");
				jsFunctionality.AppendLine("  var startDate = GetStartDate( $get('currentMonth').getAttribute('year'), $get('currentMonth').getAttribute('month') - 1 );");
				jsFunctionality.AppendLine("  var endDate = GetEndDate( $get('currentMonth').getAttribute('year'), $get('currentMonth').getAttribute('month') - 1 );");
                jsFunctionality.AppendFormat("  $find('{0}').set_selectedDate(startDate);", startDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
                jsFunctionality.AppendFormat("  $find('{0}').raiseDateSelectionChanged();", startDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
                jsFunctionality.AppendFormat("  $find('{0}').set_selectedDate(endDate);", endDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("}");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("function NextMonth() {");
				jsFunctionality.AppendLine("  var startDate = GetStartDate( $get('currentMonth').getAttribute('year'), $get('currentMonth').getAttribute('month') + 1 );");
				jsFunctionality.AppendLine("  var endDate = GetEndDate( $get('currentMonth').getAttribute('year'), $get('currentMonth').getAttribute('month') + 1 );");
                jsFunctionality.AppendFormat("  $find('{0}').set_selectedDate(startDate);", startDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
                jsFunctionality.AppendFormat("  $find('{0}').raiseDateSelectionChanged();", startDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
                jsFunctionality.AppendFormat("  $find('{0}').set_selectedDate(endDate);", endDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("}");

				//Add Event handlers
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("function BindOverusageReportClientFunctionality() {");
				jsFunctionality.AppendLine("  Sys.Application.add_load(");
				jsFunctionality.AppendLine("    function () {");
				jsFunctionality.AppendFormat("      $find('{0}').add_dateSelectionChanged(OnStartDateChanged);", startDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
                jsFunctionality.AppendFormat("      SetCurrentMonthText($find('{0}').get_selectedDate());", startDateCalendar.CalendarCtrlClientID);
				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("    }");
				jsFunctionality.AppendLine("  );");
				jsFunctionality.AppendLine("}");

				jsFunctionality.AppendLine(String.Empty);
				jsFunctionality.AppendLine("// -->");
				jsFunctionality.AppendLine("</script>");

				Page.ClientScript.RegisterClientScriptBlock(
					  typeof(OverusageReport)
					, "overusageReportToolbarScript"
					, jsFunctionality.ToString()
					, false
				);
			}
		}

		#endregion
	}
}
