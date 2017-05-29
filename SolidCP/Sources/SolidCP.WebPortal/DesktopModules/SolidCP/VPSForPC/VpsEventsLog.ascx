<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsEventsLog.ascx.cs" Inherits="SolidCP.Portal.VPSForPC.VpsEventsLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>

	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="EventLog48" runat="server" />
                    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Events Log" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_events_log" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="Event Log is empty." AllowPaging="true" DataSourceID="odsLogEntries" PageSize="20"
                        CssSelectorClass="NormalGridView" EnableViewState="false">
                        <Columns>
                            <asp:BoundField DataField="Number" HeaderText="gvEntriesNumber" />
                            <asp:BoundField DataField="Level" HeaderText="gvEntriesLevel" Visible="false" />
                            <asp:BoundField DataField="Category" HeaderText="gvEntriesCategory" Visible="false" />
                            <asp:BoundField DataField="Decription" HeaderText="gvEntriesDecription"/>
                            <asp:BoundField DataField="EventData" HeaderText="gvEntriesEventData" Visible="false" />
                            <asp:BoundField DataField="TimeGenerated" HeaderText="gvEntriesTimeGenerated" />
                        </Columns>
                    </asp:GridView>
        
                    <asp:ObjectDataSource ID="odsLogEntries" runat="server"
                            SelectMethod="GetMonitoredObjectEvents"
                            TypeName="SolidCP.Portal.VirtualMachinesForPCHelper">
                    </asp:ObjectDataSource>
			    </div>
            </div>
            </div>
            </div>
	        <div class="Right">
		        <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
	        </div>