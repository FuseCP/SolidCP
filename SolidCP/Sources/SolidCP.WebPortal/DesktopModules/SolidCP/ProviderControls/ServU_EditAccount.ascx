<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServU_EditAccount.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.ServU_EditAccount" %>
<table cellSpacing="0" cellPadding="4" width="100%">
	<tr>
		<td class="SubHead" valign="top" style="width:150px;">
		    <asp:Label ID="lblAccessRights" runat="server" meta:resourcekey="lblAccessRights" Text="Access rights:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:CheckBox id="chkRead" runat="server" meta:resourcekey="chkRead" Text="Read" Checked="True"></asp:CheckBox>
			<br/>
			<asp:CheckBox id="chkWrite" runat="server" meta:resourcekey="chkWrite" Text="Write" Checked="True"></asp:CheckBox>
		</td>
	</tr>
</table>