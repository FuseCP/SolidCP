<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverusageReport.ascx.cs" Inherits="SolidCP.Portal.OverusageReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
	Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register TagPrefix="scp" TagName="CalendarControl" Src="UserControls/CalendarControl.ascx" %>

<!-- Our Toolbar -->
<div class="panel-body form-horizontal">

	<!-- Bandwidth search criteria -->
	<scp:CollapsiblePanel 
		ID="bandwidthCollapsiblePanel" runat="server"
		TargetControlID="bandwidthSearchCriteria"
		Text="Bandwidth Search Criteria" resourceKey="bandwidthCollapsiblePanel"
		IsCollapsed="true"
		>
	</scp:CollapsiblePanel>
	<asp:Panel ID="bandwidthSearchCriteria" runat="server" Height="0" style="overflow:hidden;">
		<div style="margin-left: 5pt">
			<p>
				<asp:Label
					ID="bandwidthCaption" runat="server"
					Text="Choose either a time frame or a month to see the bandwidth overusage for." meta:resourcekey="bandwidthCaption"
					/>
			</p>
			<table width="100%" cellpadding="0" cellspacing="0">
				<tr>
					<!-- Time frame -->
					<td class="Normal" valign="top" width="150px">
						<table>
							<tr>
								<td class="Normal">
									<asp:Label 
										ID="startDateLabel" runat="server" 
										Text="Start date:" meta:resourcekey="startDateLabel"
										/>
								</td>
								<td class="Normal">
									<scp:CalendarControl ID="startDateCalendar" runat="server" />
								</td>
							</tr>
							
							<tr>
								<td class="Normal">
									<asp:Label
										ID="endDateLabel" runat="server"
										Text="End date:" meta:resourcekey="endDateLabel"
										/>
								</td>
								<td class="Normal">
									<scp:CalendarControl ID="endDateCalendar" runat="server" />
								</td>
							</tr>
						</table>
						
						<br />
					</td>
					
					<!-- Monthes -->
					<td class="Normal" valign="top" align="left" width="300px">
						<table>
							<tr valign="middle" height="18px">
								<td class="Normal">
									<table>
										<tr valign="baseline">
											<td class="Normal">
												<a  ID="prevMonthLink" runat="server" meta:resourcekey="prevMonthLink"
													href="javascript:PreviousMonth()" style="text-decoration:none" 
													>< previous month</a>
											</td>
											<td width="70px" class="Normal Centered">
												<span id="currentMonth">current month</span>
											</td>
											<td class="Normal">
												<a  ID="nextMonthLink" meta:resourcekey="nextMonthLink" runat="server" 
													href="javascript:NextMonth()" style="text-decoration:none" 
													>next month ></a>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</div>
	</asp:Panel>
	
	
	<!-- Export functionality -->
	<scp:CollapsiblePanel
		ID="exportCollapsiblePanel" runat="server"
		TargetControlID="exportPanel"
		Text="Export" resourceKey="exportCollapsiblePanel"
		IsCollapsed="true"
		>
	</scp:CollapsiblePanel>
	<asp:Panel ID="exportPanel" runat="server" Height="0" style="overflow:hidden;">
		<div style="margin-left: 5pt">
			<asp:HyperLink 
				ID="exportToExcel" runat="server"
				Text="Export to Microsoft Excel" meta:resourcekey="exportToExcel"
				NavigateUrl="#" Target="_blank"
				SkinID="CommandButton"
			/>
			&nbsp;
			<asp:HyperLink 
				ID="exportToPdf" runat="server"
				Text="Export to Acrobat Reader (PDF)" meta:resourcekey="exportToPdf"
				NavigateUrl="#" Target="_blank"
				SkinID="CommandButton"
			/>
		</div>
	</asp:Panel>
</div>
<div class="FormButtonsBar">
	<CPCC:StyleButton id="refreshButton" CssClass="btn btn-success" runat="server" OnClick="OnRefreshButtonClick"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="refreshButtonText"/> </CPCC:StyleButton>
</div>

<script>
	<!-- 
	
	if (typeof(BindOverusageReportClientFunctionality) == "function")
	{
		BindOverusageReportClientFunctionality();
	}
	
	// -->
</script>

<!-- Report Viewer -->
<rsweb:ReportViewer ID="rvContent" runat="server" 
	EnableTheming="true"
	CssClass="Module"	
	Width="100%"
	InternalBorderWidth="0"
	Font-Names="Tahoma"
	Font-Size="8pt"
	ProcessingMode="Local"
	ShowToolbar="false"
	ShowBackButton="true"
	
	OnInit="rvContent_Init"
	OnReportRefresh="rvContent_ReportRefresh"
	OnReportError="rvContent_ReportError"
	OnDrillthrough="rvContent_Drillthrough"
	>
	<LocalReport EnableHyperlinks="true" EnableExternalImages="true">
	</LocalReport>
</rsweb:ReportViewer>
