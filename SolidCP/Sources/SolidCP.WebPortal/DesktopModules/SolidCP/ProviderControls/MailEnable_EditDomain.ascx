<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailEnable_EditDomain.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MailEnable_EditDomain" %>
<table width="100%">
    <tr>
        <td class="SubHead" style="width:200px;"><asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlCatchAllAccount" runat="server" CssClass="form-control">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblPostmaster" runat="server" meta:resourcekey="lblPostmaster" Text="Postmaster Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlPostmasterAccount" runat="server" CssClass="form-control">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblAbuse" runat="server" meta:resourcekey="lblAbuse" Text="Abuse Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlAbuseAccount" runat="server" CssClass="form-control">
            </asp:DropDownList></td>
    </tr>
</table>
<table width="100%">
	<tr>
		<td class="SubHead" style="width:200px;"><asp:Label ID="lblDomainSmartHostEnabled" runat="server" meta:resourcekey="lblDomainSmartHostEnabled" Text="Act as smart host:"></asp:Label></td>
		<td class="Normal">
			<asp:CheckBox ID="chkDomainSmartHostEnabled" Runat="server" meta:resourcekey="chkDomainSmartHostEnabled" Text="Yes"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">Only smart host email from authenticated senders:</td>
		<td class="Normal" vAlign="top">
			<asp:CheckBox ID="chkDomainSmartHostAuthSenders" Runat="server" meta:resourcekey="chkDomainSmartHostAuthSenders" Text="Yes" />
		</td>
	</tr>
	<tr>
        <td class="SubHead">Smart host mail to:</td>
        <td class="Normal" vAlign="top">
            <asp:TextBox ID="txtDestination" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
            (enter IP address or domain name of destination)</td>
    </tr>
</table>
