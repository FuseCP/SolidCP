<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="hMailServer_EditList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.hMailServer_EditList" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<table cellSpacing="0" cellPadding="3" width="100%">
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPostingMode" runat="server" meta:resourcekey="lblPostingMode" Text="Posting mode:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlPostingMode" runat="server" resourcekey="ddlPostingMode" CssClass="form-control" Width="150px">
				<asp:ListItem Value="MembersCanPost">MembersOnly</asp:ListItem>
				<asp:ListItem Value="AnyoneCanPost">Anyone</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead" vAlign="top">
		    <asp:Label ID="lblMembers" runat="server" meta:resourcekey="lblMembers" Text="Mailing list members:"></asp:Label>
		</td>
		<td vAlign="top">
			<dnc:EditItemsList id="mailEditItems" runat="server"></dnc:EditItemsList>
		</td>
	</tr>
</table>