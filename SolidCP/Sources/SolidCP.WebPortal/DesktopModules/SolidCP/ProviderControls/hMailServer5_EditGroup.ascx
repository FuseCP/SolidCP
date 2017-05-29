<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="hMailServer5_EditGroup.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.hMailServer5_EditGroup" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<table cellSpacing="0" cellPadding="3" width="100%">
	<tr>
		<td class="SubHead" width="200" nowrap valign="top">
		    <asp:Label ID="lblGroupMembers" runat="server" meta:resourcekey="lblGroupMembers" Text="Group e-mails:"></asp:Label>
		</td>
		<td class="normal" width="100%" valign="top">
			<dnc:EditItemsList id="mailEditItems" runat="server"></dnc:EditItemsList>
		</td>
	</tr>
</table>