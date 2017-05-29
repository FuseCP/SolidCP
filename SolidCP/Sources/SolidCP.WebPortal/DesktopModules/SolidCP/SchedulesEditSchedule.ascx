<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchedulesEditSchedule.ascx.cs" Inherits="SolidCP.Portal.SchedulesEditSchedule" %>
<%@ Register Src="UserControls/ScheduleTime.ascx" TagName="ScheduleTime" TagPrefix="uc3" %>
<%@ Register Src="UserControls/ScheduleInterval.ascx" TagName="ScheduleInterval" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/UserLookup.ascx" TagName="UserLookup" TagPrefix="uc1" %>
<%@ Register Src="UserControls/ParameterEditor.ascx" TagName="ParameterEditor" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div class="FormBody">
	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" style="width:150px;"><asp:Label ID="lblTaskName" runat="server" meta:resourcekey="lblTaskName" Text="Task Name:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtTaskName" runat="server" Width="380px" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="txtTaskName"></asp:RequiredFieldValidator></td>
        </tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblTaskType" runat="server" meta:resourcekey="lblTaskType" Text="Task Type:"></asp:Label>
			</td>
			<td class="Normal">
                <asp:DropDownList ID="ddlTaskType" runat="server" Width="380px"   CssClass="NormalDropDown"
                    DataTextField="TaskId" DataValueField="TaskId" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlTaskType_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="ddlTaskType"></asp:RequiredFieldValidator></td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblTaskParameters" runat="server" meta:resourcekey="lblTaskParameters" Text="Task Parameters:"></asp:Label>
			</td>
			<td class="Normal" valign="top">
				<input id="ControlToLoad" type="hidden" value="" runat="server"/>
			    <asp:PlaceHolder runat="server" ID="TaskParametersPlaceHolder" />
                <asp:GridView id="gvTaskParameters" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="gvTaskParameters" CssSelectorClass="NormalGridView"
                    DataKeyNames="ParameterID" OnRowDataBound="gvTaskParameters_RowDataBound" Visible="false">
                    <Columns>
                        <asp:TemplateField HeaderText="gvTaskParametersName">
                            <ItemStyle CssClass="NormalBold" Width="150" Wrap="false" />
	                        <ItemTemplate>
		                        <%# GetSharedLocalizedString("SchedulerTaskParameter." + Eval("ParameterID").ToString()) %>
	                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="gvTaskParametersValue">
                            <ItemStyle Width="300px" />
	                        <ItemTemplate>
	                            <uc1:ParameterEditor ID="txtValue" runat="server" />
	                        </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
			</td>
		</tr>
		<tr>
		    <td class="Normal">&nbsp;</td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
                <asp:Label ID="lblSchedule" runat="server" meta:resourcekey="lblSchedule" Text="Schedule:"></asp:Label>
			</td>
			<td valign="top">
			    <asp:DropDownList ID="ddlSchedule" runat="server"   CssClass="NormalDropDown" resourcekey="ddlSchedule"
			        AutoPostBack="True" OnSelectedIndexChanged="ddlSchedule_SelectedIndexChanged">
			        <asp:ListItem Value="Daily">Daily</asp:ListItem>
			        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
			        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
			        <asp:ListItem Value="OneTime">Once</asp:ListItem>
			        <asp:ListItem Value="Interval">Interval</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Label ID="lblStartTime" runat="server" meta:resourcekey="lblStartTime" Text="Start Time:"></asp:Label>
                <uc3:ScheduleTime ID="timeStartTime" runat="server" />
                <table id="tblInterval" runat="server" cellpadding="5">
                    <tr>
                        <td class="SubHead" nowrap>
                            <asp:Label ID="lblRunEvery" runat="server" meta:resourcekey="lblRunEvery" Text="Run Every:"></asp:Label>
                        </td>
                        <td>
                            <uc2:ScheduleInterval ID="intInterval" runat="server" Interval="3600" />
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" nowrap>
                            <asp:Label ID="lblFrom" runat="server" meta:resourcekey="lblFrom" Text="From:"></asp:Label>
                        </td>
                        <td class="SubHead" nowrap>
                            <uc3:ScheduleTime ID="timeFromTime" runat="server" />
                            &nbsp;&nbsp;
                            <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To: "></asp:Label><uc3:ScheduleTime ID="timeToTime" runat="server" />
                        </td>
                    </tr>
                </table>
                <table id="tblOneTime" runat="server" cellpadding="5">
                    <tr>
                        <td class="SubHead" nowrap>
                            <asp:Label ID="lblRunOn" runat="server" meta:resourcekey="lblRunOn" Text="Run On:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartDate" runat="server" Width="100px">10/10/2006</asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtStartDate"
                                ErrorMessage="*"></asp:RequiredFieldValidator></td>
                    </tr>
                </table>
                <table id="tblWeekly" runat="server" cellpadding="5">
                    <tr>
                        <td class="SubHead" nowrap>
                            <asp:Label ID="lblWeekDay" runat="server" meta:resourcekey="lblWeekDay" Text="Day of the Week:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWeekDay" runat="server" Width="40px">1</asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTaskType"
                                ErrorMessage="*"></asp:RequiredFieldValidator></td>
                    </tr>
                </table>
                <table id="tblMonthly" runat="server" cellpadding="5">
                    <tr>
                        <td class="SubHead" nowrap>
                            <asp:Label ID="lblMonthDay" runat="server" meta:resourcekey="lblMonthDay" Text="Day of the Month:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMonthDay" runat="server" Width="40px">1</asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMonthDay"
                                ErrorMessage="*"></asp:RequiredFieldValidator></td>
                    </tr>
                </table>
            </td>
		</tr>
		<tr>
		    <td class="Normal">&nbsp;</td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblEnabled" runat="server" meta:resourcekey="lblEnabled" Text="Enabled:"></asp:Label>
			</td>
			<td valign="top">
                <asp:CheckBox ID="chkEnabled" runat="server" meta:resourcekey="chkEnabled" Text="Yes" Checked="true" /></td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblPriority" runat="server" meta:resourcekey="lblPriority" Text="Priority:"></asp:Label>
			</td>
			<td class="Normal" valign="top">
                <asp:DropDownList ID="ddlPriority" runat="server"   CssClass="NormalDropDown" resourcekey="ddlPriority">
                    <asp:ListItem Value="Highest">High</asp:ListItem>
                    <asp:ListItem Value="AboveNormal">AboveNormal</asp:ListItem>
                    <asp:ListItem Value="Normal" Selected="True">Normal</asp:ListItem>
                    <asp:ListItem Value="BelowNormal">BelowNormal</asp:ListItem>
                    <asp:ListItem Value="Lowest">Low</asp:ListItem>
                </asp:DropDownList></td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblMaxExecutionTime" runat="server" meta:resourcekey="lblMaxExecutionTime" Text="Max Execution Time:"></asp:Label>
			</td>
			<td class="Normal" valign="top">
                <uc2:ScheduleInterval ID="intMaxExecutionTime" runat="server" Interval="0" />
            </td>
		</tr>
    </table>
	<%--
    <scp:CollapsiblePanel id="secHistory" runat="server"
        TargetControlID="HistoryPanel" meta:resourcekey="secHistory" Text="History Log">
    </scp:CollapsiblePanel>
    <asp:Panel ID="HistoryPanel" runat="server" Height="0" style="overflow:hidden;">
	    <table width="100%" cellspacing="0" cellpadding="0">
		    <tr>
			    <td>
                    <table width="100%" cellpadding="3" cellspacing="1"
                        class="GridToolbox">
                        <tr>
                            <td class="GridToolboxCell">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td><asp:Button ID="btnClearLog" runat="server" meta:resourcekey="btnClearLog" Text="Clear History" CssClass="Button3" OnClick="btnClearLog_Click" CausesValidation="False" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView id="gvHistory" runat="server" AutoGenerateColumns="False"
                        Width="100%" EmptyDataText="gvHistory"
                        CellPadding="4" CellSpacing="1" GridLines="None" Border="1" BorderColor="#C4D6BB" CssClass="GridOutline" OnRowEditing="gvHistory_RowEditing" OnRowCancelingEdit="gvHistory_RowCancelingEdit">
                        <Columns>
                            <asp:TemplateField HeaderText="gvHistoryStartTime" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
	                            <ItemTemplate>
			                        
	                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="gvHistoryFinishTime" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
	                            <ItemTemplate>
		                            <%# GetHistoryFinishTime((DateTime)Eval("FinishTime"))%>
	                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="gvSchedulesResult">
	                            <ItemTemplate>
		                            <%# GetLocalizedString("Result." + (string)Eval("StatusID")) %>
	                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="gvSchedulesLog" ItemStyle-Width="100%">
	                            <ItemTemplate>
		                            <asp:LinkButton ID="cmdLog" runat="server" meta:resourcekey="cmdLog" Text="View Log" CommandName="edit" CausesValidation="false"></asp:LinkButton>
	                            </ItemTemplate>
	                            <EditItemTemplate>
	                                <asp:TextBox ID="txtLog" runat="server"
	                                    CssClass="LogArea"
	                                    TextMode="MultiLine" Rows="10" Width="400" Wrap="false"
	                                    Text='<%# GetHistoryLog((int)Eval("ScheduleHistoryID")) %>'></asp:TextBox>
	                                <asp:LinkButton ID="cmdClose" runat="server" meta:resourcekey="cmdClose" Text="Close" CommandName="cancel" CausesValidation="false"></asp:LinkButton>
	                            </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridHeader" HorizontalAlign="Left" />
                        <RowStyle CssClass="Normal" />
                        <PagerStyle CssClass="GridPager" />
                        <EmptyDataRowStyle CssClass="Normal" />
                    </asp:GridView>
			    </td>
		    </tr>
	    </table>
	</asp:Panel>
	--%>
</div>
<div class="FormFooter">
    <asp:Button id="btnUpdate" runat="server" meta:resourcekey="btnUpdate" CssClass="Button1" Text="Save" OnClick="btnUpdate_Click"></asp:Button>
	<asp:Button id="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1"  CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
	<asp:Button id="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1"  CausesValidation="False" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete task?');"></asp:Button>
</div>