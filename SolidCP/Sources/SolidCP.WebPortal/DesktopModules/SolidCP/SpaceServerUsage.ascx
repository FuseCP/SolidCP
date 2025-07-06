<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceServerUsage.ascx.cs" Inherits="SolidCP.Portal.SpaceServerUsage" %>
<%@ Register Src="UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="scp" %>
<%@ Import Namespace="SolidCP.Portal" %>

<asp:UpdatePanel ID="upUsage" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Timer ID="Timer1" runat="server" Interval="100" OnTick="Timer1_Tick" Enabled="false" />

        <asp:UpdateProgress ID="loadProgress" runat="server" AssociatedUpdatePanelID="upUsage" DisplayAfter="0">
            <ProgressTemplate>
                <div class="ProgressPanelArea" style="text-align: center; margin-top: 10px;">
                    <div class="MediumBold" style="white-space:nowrap; display: inline-flex; align-items: center; justify-content: center;">
                        <img src='<%= PortalUtils.GetThemedImage("indicator_medium.gif") %>' alt="Loading..." style="vertical-align: middle;" />&nbsp;
                        <span>Loading data...</span> 
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <div id="gaugeUsage" runat="server">
            <div class="panel-body form-horizontal">
                <ul class="list-group">
                <li>
                    <asp:Localize ID="locUsageCpu" runat="server" meta:resourcekey="locUsageCpu" Text="CPU load:" /> 
                    <scp:Gauge ID="cpuGauge" runat="server" Progress="0" Total="100" />
                    <asp:Label id="usageCpu" runat="server"/>%
                    of
                    <asp:Label id="totalCpu" runat="server"/>%
                </li>
            </ul>
                <ul class="list-group">
                    <li>
                        <asp:Localize ID="locUsageMemory" runat="server" meta:resourcekey="locUsageMemory" Text="RAM usage:" />
                        <scp:Gauge ID="ramGauge" runat="server" Progress="0" Total="100" />
                        <asp:Label id="freeMemory" runat="server"/> MB
                        of
                        <asp:Label id="totalMemory" runat="server"/> MB
                    </li>
                </ul>
            </div>
        </div>
        
    </ContentTemplate>
</asp:UpdatePanel>