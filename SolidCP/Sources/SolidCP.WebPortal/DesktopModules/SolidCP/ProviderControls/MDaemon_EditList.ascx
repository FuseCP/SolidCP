<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MDaemon_EditList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MDaemon_EditList" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<table cellSpacing="0" cellPadding="3" width="100%">
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblReplyTo" runat="server" meta:resourcekey="lblReplyTo" Text="Subscribers reply to:"></asp:Label>
		</td>
		<td class="normal" width="100%">
			<asp:DropDownList id="ddlReplyTo" runat="server" resourcekey="ddlReplyTo" CssClass="form-control" Width="150px">
				<asp:ListItem Value="RepliesToList">List</asp:ListItem>
				<asp:ListItem Value="RepliesToSender">Sender</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPostingMode" runat="server" resourcekey="lblPostingMode" Text="Posting mode:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlPostingMode" runat="server" meta:resourcekey="ddlPostingMode" CssClass="form-control" Width="150px">
				<asp:ListItem Value="MembersCanPost">MembersOnly</asp:ListItem>
				<asp:ListItem Value="AnyoneCanPost">Anyone</asp:ListItem>
				<asp:ListItem Value="PasswordProtectedPosting">Password</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPostingPassword" runat="server" meta:resourcekey="lblPostingPassword" Text="Posting password:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Width="150px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblModerationEnabled" runat="server" meta:resourcekey="lblModerationEnabled" Text="Moderation is enabled:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="chkModerationEnabled" Runat="server" Text="Yes"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblModeratorAddress" runat="server" meta:resourcekey="lblModeratorAddress" Text="Moderator address:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtModeratorEmail" runat="server" Width="150px" CssClass="form-control"></asp:TextBox>
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