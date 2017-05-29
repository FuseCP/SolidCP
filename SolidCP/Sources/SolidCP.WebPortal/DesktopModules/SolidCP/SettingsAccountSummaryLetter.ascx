<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsAccountSummaryLetter.ascx.cs" Inherits="SolidCP.Portal.SettingsAccountSummaryLetter" %>
<table>
    <tr>
        <td class="Normal" colspan="2">
            <asp:CheckBox ID="chkEnableLetter" runat="server" meta:resourcekey="chkEnableLetter" Text="Enable Account Summary Letter" /></td>
    </tr>
    <tr>
        <td class="SubHead" Width="150" colspan="1"><asp:Label ID="lblFrom" runat="server" meta:resourcekey="lblFrom" Text="From:"></asp:Label></td>
        <td class="Normal" Width="100%" colspan="1">
            <asp:TextBox ID="txtFrom" runat="server" Width="450px" CssClass="form-control"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="SubHead" colspan="1"><asp:Label ID="lblCC" runat="server" meta:resourcekey="lblCC" Text="CC:"></asp:Label></td>
        <td class="Normal" colspan="1">
            <asp:TextBox ID="txtCC" runat="server" Width="100%" CssClass="form-control"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="SubHead" colspan="1"><asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label></td>
        <td class="Normal" colspan="1">
            <asp:TextBox ID="txtSubject" runat="server" Width="100%" CssClass="form-control"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="SubHead" colspan="1"><asp:Label ID="lblPriority" runat="server" meta:resourcekey="lblPriority" Text="Priority"></asp:Label></td>
        <td class="Normal" colspan="1">
            <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control" resourcekey="ddlPriority">
				<asp:ListItem Value="High">High</asp:ListItem>
				<asp:ListItem Value="Normal">Normal</asp:ListItem>
				<asp:ListItem Value="Low">Low</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
	<tr>
		<td class="SubHead" colspan="2"><br /><br /><asp:Label ID="lblHtmlBody" runat="server" meta:resourcekey="lblHtmlBody" Text="HTML Body:"></asp:Label></td>
	</tr>
	<tr>
		<td class="Normal" colspan="2">
			<asp:TextBox ID="txtHtmlBody" runat="server" Rows="15" TextMode="MultiLine" Width="100%" CssClass="form-control" Wrap="false"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead" colspan="2"><br /><br /><asp:Label ID="lblTextBody" runat="server" meta:resourcekey="lblTextBody" Text="Text Body:"></asp:Label></td>
	</tr>
	<tr>
		<td class="Normal" colspan="2">
			<asp:TextBox ID="txtTextBody" runat="server" Rows="15" TextMode="MultiLine" Width="100%" CssClass="form-control" Wrap="false"></asp:TextBox></td>
	</tr>
</table>