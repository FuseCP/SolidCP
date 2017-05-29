<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsAlertsLog.ascx.cs" Inherits="SolidCP.Portal.VPSForPC.VpsAlertsLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>


	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="AlertLog48" runat="server" />
                    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Alerts Log" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_alerts_log" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="Alarms Log is empty." AllowPaging="true" DataSourceID="odsLogEntries" PageSize="20"
                        CssSelectorClass="NormalGridView" EnableViewState="false">
                        <Columns>                        
                            <asp:BoundField DataField="Severity" HeaderText="gvEntriesSeverity" />
                            <asp:BoundField DataField="ResolutionState" HeaderText="gvEntriesResolutionState" />
                            <asp:BoundField DataField="Name" HeaderText="gvEntriesName" />
                            <asp:BoundField DataField="Description" HeaderText="gvEntriesDescription" />
                            <asp:BoundField DataField="Source" HeaderText="gvEntriesSource" Visible="false" />
                            <asp:BoundField DataField="Created" HeaderText="gvEntriesCreated" />
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsLogEntries" runat="server"
                            SelectMethod="GetMonitoringAlerts"
                            TypeName="SolidCP.Portal.VirtualMachinesForPCHelper">
                    </asp:ObjectDataSource>
			    </div>
            </div>
                    </div>
            </div>
	        <div class="Right">
		        <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
	        </div>