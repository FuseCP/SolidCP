<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringPage.aspx.cs"
	Inherits="SolidCP.Portal.VPSForPC.MonitoringPage" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="../UserControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
	TagPrefix="uc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title></title>
	<link href="/App_Themes/Default/Styles/jquery-ui-1.8.9.css" rel="stylesheet" type="text/css" />
</head>
<body>
	<form id="form1" runat="server" style="width: 590px; height: 700px">
	<asp:ScriptManager ID="scriptManager" runat="server" EnablePartialRendering="true"
		EnableScriptGlobalization="true" EnableScriptLocalization="true">
	</asp:ScriptManager>
	<asp:Timer runat="server" Interval="10000" ID="operationTimer" OnTick="operationTimer_Tick" />
	<div id="testClass" style="width: 590px; height: 700px">
		<table cellpadding="3" width="100%">
			<tr>
				<td>
					<asp:Label ID="lblStartPeriod" runat="server" AssociatedControlID="txtStartPeriod"
						meta:resourcekey="lblStartPeriod" Text="Start day" CssClass="MediumBold" />
				</td>
				<td>
					<asp:TextBox ID="txtStartPeriod" runat="server" CssClass="form-control txtDateTimePeriod"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label ID="lblEndPeriod" runat="server" AssociatedControlID="txtEndPeriod" meta:resourcekey="lblEndPeriod"
						Text="End day" CssClass="MediumBold" />
				</td>
				<td>
					<asp:TextBox ID="txtEndPeriod" runat="server" CssClass="form-control txtDateTimePeriod"></asp:TextBox>
				</td>
			</tr>
		</table>
		<asp:UpdatePanel runat="server" ID="UpdatePanelCharts" UpdateMode="Conditional">
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="operationTimer" EventName="Tick" />
			</Triggers>
			<ContentTemplate>
				<div id="monitoringWrapper">
					<asp:Chart ID="ChartCounter" runat="server" ImageLocation="TempImages/ChartPic_#SEQ(300,3)"
						Width="584px" Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom"
						BorderWidth="2px" BorderColor="#B54001" IsSoftShadows="False">
						<Series>
							<asp:Series MarkerSize="8" BorderWidth="3" XValueType="DateTime" Name="series" ChartType="StackedArea"
								MarkerStyle="Circle" ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="#33CC33"
								ShadowOffset="2" YValueType="Double">
							</asp:Series>
						</Series>
						<ChartAreas>
							<asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
								BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								<Area3DStyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="40"
									IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								<AxisY LineColor="64, 64, 64, 64">
									<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									<MajorGrid LineColor="64, 64, 64, 64" />
								</AxisY>
								<AxisX LineColor="64, 64, 64, 64">
									<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									<MajorGrid LineColor="64, 64, 64, 64" />
								</AxisX>
							</asp:ChartArea>
						</ChartAreas>
					</asp:Chart>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	</form>
	<script type="text/javascript">
		$(document).ready(function () {
			$(".txtDateTimePeriod").datepicker();
		});
	</script>
</body>
</html>