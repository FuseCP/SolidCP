<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuditLogReportView.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.AuditLogReportView" %>
<div>
    <br />
	<table>
        <tr>
            <td class="SubHead">
				<asp:Label ID="lblMailFrom" runat="server" meta:resourcekey="lblMailFrom" Text="Mail From:"></asp:Label>
			</td>
            <td class="Normal">
   				<asp:TextBox ID="txtMailFrom" runat="server" ReadOnly="true" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
				<asp:Label ID="lblMailTo" runat="server" meta:resourcekey="lblMailTo" Text="Mail To:"></asp:Label>
			</td>
            <td class="Normal">
   				<asp:TextBox ID="txtMailTo" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
				<asp:Label ID="lblAuditLogSeverity" runat="server" meta:resourcekey="lblAuditLogSeverity" Text="Audit Log Severity:"></asp:Label>
			</td>
            <td class="Normal">
                <asp:DropDownList ID="ddlAuditLogSeverity" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
				<asp:Label ID="lblAuditLogSource" runat="server" meta:resourcekey="lblAuditLogSource" Text="Audit Log Source:"></asp:Label>
			</td>
            <td class="Normal">
                <asp:DropDownList ID="ddlAuditLogSource" runat="server" CssClass="form-control"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlSource_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
				<asp:Label ID="lblAuditLogTask" runat="server" meta:resourcekey="lblAuditLogTask" Text="Audit Log Task:"></asp:Label>
			</td>
            <td class="Normal">
                <asp:DropDownList ID="ddlAuditLogTask" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
				<asp:Label ID="lblAuditLogDate" runat="server" meta:resourcekey="lblAuditLogDate" Text="Audit Log Date:"></asp:Label>
			</td>
            <td class="Normal">
                <asp:DropDownList ID="ddlAuditLogDate" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
				<asp:Label ID="lblExecutionLog" runat="server" meta:resourcekey="lblExecutionLog" Text="Show Execution Log:"></asp:Label>
			</td>
            <td class="Normal">
                <asp:DropDownList ID="ddlExecutionLog" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
	</table>
</div>
