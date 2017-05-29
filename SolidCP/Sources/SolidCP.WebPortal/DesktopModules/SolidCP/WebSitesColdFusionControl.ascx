<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesColdFusionControl.ascx.cs" Inherits="SolidCP.Portal.WebSitesColdFusionControl" %>
<div style="padding: 20;">
<asp:Literal ID="litCFUnavailable" runat="server"></asp:Literal>
<table cellpadding="4">
    <tr id="rowAsp" runat="server">
        <td class="SubHead">
            <asp:Label ID="lblCF" runat="server" meta:resourcekey="lblAsp" Text="Enable ColdFusion Scripting:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:CheckBox ID="chkCF" runat="server" Text="Enabled" meta:resourcekey="chkEnabled" /></td>
       </tr>
</table>
	<asp:Label ID="Label1" runat="server"> * Enabling of ColdFusion scripting automatically restarts IIS</asp:Label>
</div>