<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CerberusFTP6_EditAccount.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.CerberusFTP6_EditAccount" %>
<table cellSpacing="0" cellPadding="4" width="100%">
	<tr>
		<td class="SubHead" valign="top" style="width:150px;">
            <asp:Label ID="lblAccessRights" runat="server" meta:resourcekey="lblAccessRights" Text="Access rights:"></asp:Label>
       </td>
		<td class="Normal">
            <asp:CheckBox ID="chkRead" runat="server" meta:resourcekey="chkRead" Text="Read" Checked="True" />
            <br/>
            <asp:CheckBox ID="chkWrite" runat="server" meta:resourcekey="chkWrite" Text="Write" Checked="True" />
        </td>
	</tr>
</table>