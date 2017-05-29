<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExecuteSystemCommand.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.ExecuteSystemCommand" %>
	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap valign="top">
				<asp:Label ID="lblServerName" runat="server" meta:resourcekey="lblServerName">Server Name: </asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtServerName" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblExecutablePath" runat="server" meta:resourcekey="lblExecutablePath" Text="Executable Path:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtExecutablePath" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblExecutableParameters" runat="server" meta:resourcekey="lblExecutableParameters" Text="Executable Parameters:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtExecutableParameters" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
	</table>