<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IceWarp_EditGroup.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.IceWarp_EditGroup" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<table cellSpacing="0" cellPadding="3" width="100%">
	<tr>
		<td class="SubHead" width="150">
		    <asp:Label ID="lblGroupMembers" runat="server" meta:resourcekey="lblGroupMembers" Text="Group e-mails:"></asp:Label>
		</td>
		<td class="Normal">
			<dnc:EditItemsList id="mailEditItems" runat="server"></dnc:EditItemsList>
		</td>
	</tr>
</table>