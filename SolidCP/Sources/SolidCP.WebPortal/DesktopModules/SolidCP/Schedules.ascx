<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Schedules.ascx.cs" Inherits="SolidCP.Portal.Schedules" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="uc4" %>
<%@ Import Namespace="SolidCP.Portal" %>

<asp:Timer runat="server" Interval="5000" ID="tasksTimer" OnTick="tasksTimer_Tick" />
<div class="FormButtonsBar right">
     <CPCC:StyleButton ID="btnAddItem" runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click" >
         <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem"/>
     </CPCC:StyleButton>
      
</div>
<div class="panel-body">
    <div class="row">
    <div class="col-md-8">
         &nbsp;<asp:CheckBox ID="chkRecursive" runat="server" AutoPostBack="True" CssClass="Normal"
            Text="Recursive" meta:resourcekey="chkRecursive" />
    </div>
    <div class="col-md-4">
        <uc1:SearchBox ID="searchBox" runat="server" />
    </div>
        </div>
</div>

<asp:UpdatePanel runat="server" ID="schedulesUpdatePanel" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="tasksTimer" EventName="Tick" />
    </Triggers>
    <ContentTemplate>
<asp:GridView id="gvSchedules" runat="server" AutoGenerateColumns="False"
	DataSourceID="odsSchedules" AllowPaging="True" AllowSorting="True" EmptyDataText="gvSchedules"
	OnRowCommand="gvSchedules_RowCommand" CssSelectorClass="NormalGridView" DataKeyNames="ScheduleID">
	<Columns>
		<asp:TemplateField SortExpression="ScheduleName" HeaderText="gvSchedulesName">
			<ItemStyle Width="100%"></ItemStyle>
			<HeaderStyle Wrap="false" />
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("ScheduleID", Eval("ScheduleID").ToString(), "edit", "SpaceID=" + PanelSecurity.PackageId) %>'>
					<%# PortalAntiXSS.Encode((string)Eval("ScheduleName")) %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField SortExpression="ScheduleTypeID" HeaderText="gvSchedulesType"
		    ItemStyle-Wrap="false" HeaderStyle-Wrap="false">
			<ItemTemplate>
    			<%# GetSharedLocalizedString("ScheduleType." + Eval("ScheduleTypeID").ToString()) %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="NextRun" SortExpression="NextRun" HeaderText="gvSchedulesNextRun"
		    ItemStyle-Wrap="false" HeaderStyle-Wrap="false"></asp:BoundField>
		<asp:BoundField DataField="LastRun" SortExpression="LastRun" HeaderText="gvSchedulesLastRun"
		    ItemStyle-Wrap="false" HeaderStyle-Wrap="false"></asp:BoundField>
		<asp:TemplateField HeaderText="gvSchedulesStatus" ItemStyle-Wrap="false">
			<ItemTemplate>
                <asp:ImageButton ID="cmdStart" runat="server" ToolTip="Start" SkinID="StartMedium" Visible='<%# !IsScheduleActive((int)Eval("StatusID")) %>'
                    CommandName="start" CommandArgument='<%# Eval("ScheduleID") %>' />
                <asp:ImageButton ID="cmdStop" runat="server" ToolTip="Stop" SkinID="StopMedium" Visible='<%# IsScheduleActive((int)Eval("StatusID")) %>'
                    CommandName="stop" CommandArgument='<%# Eval("ScheduleID") %>' />
                <%# GetScheduleStatus((int)Eval("StatusID")) %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="gvSchedulesResult" HeaderStyle-Wrap="false" ItemStyle-Width="150px">
			<ItemTemplate>
    			<%# GetAuditLogRecordSeverityName((int)Eval("LastResult"))%>
			</ItemTemplate>
		</asp:TemplateField>
        <asp:TemplateField SortExpression="PackageName" HeaderText="gvSchedulesSpace">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id="lnkSpace" runat="server"
	                NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
		            <%# Eval("PackageName") %>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField SortExpression="Username" HeaderText="gvSchedulesUser">
		    <ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<asp:hyperlink id="lnkUser" runat="server"
				    NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
            <HeaderStyle Wrap="False" />
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<div class="GridFooter">
    <asp:Label ID="lblScheduledTasks" runat="server" meta:resourcekey="lblScheduledTasks" Text="Scheduled Tasks:"></asp:Label>
    <uc4:Quota ID="quotaTasks" runat="server" QuotaName="OS.ScheduledTasks" />
</div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:ObjectDataSource ID="odsSchedules" runat="server" EnablePaging="True" SelectCountMethod="GetSchedulesPagedCount"
    SelectMethod="GetSchedulesPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.SchedulesHelper" OnSelected="odsSchedules_Selected">
    <SelectParameters>
        <asp:ControlParameter ControlID="chkRecursive" Name="recursive" PropertyName="Checked" />
        <asp:ControlParameter ControlID="searchBox" Name="filterColumn" PropertyName="FilterColumn" />
         <asp:ControlParameter ControlID="searchBox" Name="filterValue" PropertyName="FilterValue" />
    </SelectParameters>
</asp:ObjectDataSource>