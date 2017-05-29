<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsMonitoring.ascx.cs" Inherits="SolidCP.Portal.VPSForPC.VpsMonitoring" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<asp:Timer runat="server" Interval="180000" ID="operationTimer" />

<link type="text/css" href="/App_Themes/Default/Styles/jquery-ui-1.8.9.css" rel="stylesheet" />	
<link type="text/css" href="/App_Themes/Default/Styles/jquery.window.css" rel="stylesheet" />	

<script src="JavaScript/jquery.window.js" type="text/javascript"></script>

<div runat="server" id="divWrapper">
<script type="text/javascript">
    function ShowASPanel(chartType) {
        var sUrl = "/DesktopModules/SolidCP/VPSForPC/MonitoringPage.aspx" + "?ItemID=" + $("#<%=hItemId.ClientID%>").val() + '&chartType=' + chartType;
        var win = $.window({
            title: "Performance Counter: " + chartType,
            url: sUrl,
            width: "590px"
        });

        win.getFrame().height(500);
        win.resize(600, 500);
        return false;
    };
</script>
</div>
        <asp:HiddenField ID="hItemId" runat="server"  />
	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Monitoring48" runat="server" />
                    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Monitoring" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                 <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_monitoring" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <h1>Monitoring</h1>
               <div id="testClass">
                    <div id="monitoringWrapper">
				        <asp:UpdatePanel runat="server" ID="UpdatePanelCharts" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="operationTimer" EventName="Tick" />
                            </Triggers>
                            <ContentTemplate>                
                            <asp:Chart ID="ChartProc" runat="server" Palette="BrightPastel" 
                                ImageType="Png" ImageLocation="TempImages/ChartPic_#SEQ(300,3)" 
                                Width="584px" Height="296px" BorderlineDashStyle="Solid" 
                                BorderWidth="1" BorderColor="181, 64, 1" IsSoftShadows="False">
                                <Series>
							        <asp:Series BorderWidth="1" XValueType="DateTime" Name="series" ChartType="Line" MarkerStyle="Circle" ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="0" YValueType="Double" IsValueShownAsLabel="false"></asp:Series>
                                </Series>
                                <ChartAreas>
							        <asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								        <area3dstyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="20" IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								        <axisy LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisy>
								        <axisx LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisx>
							        </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <div>
                                <CPCC:StyleButton id="btnShowProcessorAsPanel" CssClass="btn btn-success" runat="server" OnClientClick="return ShowASPanel('Processor')"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnShowProcessorAsPanelText"/> </CPCC:StyleButton>
			                </div>
                            <asp:Chart ID="ChartNetwork" runat="server" ImageLocation="TempImages/ChartPic_#SEQ(300,3)" 
                                Width="584px" Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" 
                                BorderWidth="1px" BorderColor="#B54001" IsSoftShadows="False">
                                <Series>
							        <asp:Series MarkerSize="8" BorderWidth="1" XValueType="DateTime" Name="series" 
                                        ChartType="Area" MarkerStyle="Circle" ShadowColor="Black" 
                                        BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="0" 
                                        YValueType="Double" IsValueShownAsLabel="false"></asp:Series>
                                </Series>
                                <ChartAreas>
							        <asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								        <area3dstyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="40" IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								        <axisy LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisy>
								        <axisx LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisx>
							        </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <div>
                                <CPCC:StyleButton id="btnShowNetworkAsPanel" CssClass="btn btn-success" runat="server" OnClientClick="return ShowASPanel('Network')"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnShowNetworkAsPanelText"/> </CPCC:StyleButton>
			                </div>
                            <asp:Chart ID="ChartMemory" runat="server" ImageLocation="TempImages/ChartPic_#SEQ(300,3)" 
                                Width="584px" Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" 
                                BorderWidth="1px" BorderColor="#B54001" IsSoftShadows="False">
                                <Series>
							        <asp:Series MarkerSize="8" BorderWidth="1" XValueType="DateTime" Name="series" 
                                        ChartType="StackedArea" MarkerStyle="Circle" ShadowColor="Black" 
                                        BorderColor="180, 26, 59, 105" Color="#33CC33" ShadowOffset="0" 
                                        YValueType="Double" IsValueShownAsLabel="false"></asp:Series>
                                </Series>
                                <ChartAreas>
							        <asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								        <area3dstyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="40" IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								        <axisy LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisy>
								        <axisx LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisx>
							        </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <div>
                                <CPCC:StyleButton id="btnShowMemoryAsPanel" CssClass="btn btn-success" runat="server" OnClientClick="return ShowASPanel('Memory')"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnShowMemoryAsPanelText"/> </CPCC:StyleButton>
			                </div>

                        </ContentTemplate>
                     </asp:UpdatePanel>
                            </div>
                       </div>
	                   <div class="Right">
		                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
	                </div>
                    </div>
            </div>
                    </div>
            </div>
<script type="text/javascript">
    $(document).ready(function () {
        $(".txtDateTimePeriod").datepicker();
    });
</script>