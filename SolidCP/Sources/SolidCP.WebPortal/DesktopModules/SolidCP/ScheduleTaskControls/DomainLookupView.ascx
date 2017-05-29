<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainLookupView.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.DomainLookupView" %>

<table cellspacing="0" cellpadding="4" width="100%">
    <tr>
        <td class="SubHead" nowrap valign="top">
			<asp:Label ID="lblServerName" runat="server" meta:resourcekey="lblServerName">Server Name: </asp:Label>
		</td>
        <td class="Normal" width="100%">
            <asp:DropDownList ID="ddlServers" runat="server" CssClass="form-control" Width="150px" style="vertical-align: middle;" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblDnsServers" runat="server" meta:resourcekey="lblDnsServers" Text="DNS Servers:"></asp:Label>
        </td>
        <td class="Normal" width="100%">
            <asp:TextBox ID="txtDnsServers" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblMailTo" runat="server" meta:resourcekey="lblMailTo" Text="Mail To:"></asp:Label>
        </td>
        <td class="Normal" width="100%">
            <asp:TextBox ID="txtMailTo" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
         </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblPause" runat="server" meta:resourcekey="lblPause" Text="Pause between queries (ms):"></asp:Label>
        </td>
        <td class="Normal" width="100%">
            <asp:TextBox ID="txtPause" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
         </td>
    </tr>
</table>
