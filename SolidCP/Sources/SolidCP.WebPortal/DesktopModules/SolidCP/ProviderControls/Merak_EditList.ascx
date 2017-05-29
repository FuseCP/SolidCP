<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Merak_EditList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Merak_EditList" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<%@ Register Src="../UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>

<table cellSpacing="0" cellPadding="3" width="100%">
    <tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblDescription" runat="server" meta:resourcekey="lblDescription" Text="List Description:"></asp:Label>
		</td>
		<td class="normal" width="100%">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Width="300px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblReplyTo" runat="server" meta:resourcekey="lblReplyTo" Text="Subscribers reply to:"></asp:Label>
		</td>
		<td class="normal" width="100%">
			<asp:DropDownList id="ddlReplyTo" runat="server" resourcekey="ddlReplyTo" CssClass="form-control" Width="150px">
				<asp:ListItem Value="RepliesToList">List</asp:ListItem>
				<asp:ListItem Value="RepliesToSender">Sender</asp:ListItem>
				<asp:ListItem Value="RepliesToModerator">Moderator</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPostingMode" runat="server" meta:resourcekey="lblPostingMode" Text="Posting mode:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlPostingMode" runat="server" resourcekey="ddlPostingMode" CssClass="form-control" Width="150px">
				<asp:ListItem Value="AnyoneCanPost">Anyone</asp:ListItem> 
				<asp:ListItem Value="MembersCanPost">Members Only</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPasswordProtection" runat="server" meta:resourcekey="lblPasswordProtection" Text="Password Protection:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlPasswordProtection" runat="server" resourcekey="ddlPasswordProtection" CssClass="form-control" Width="150px">
				<asp:ListItem Value="NoProtection">Not password protected</asp:ListItem>
				<asp:ListItem Value="ClientModerated">Client Moderated</asp:ListItem>
				<asp:ListItem Value="ServerModerated">Server Moderated</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPostingPassword" runat="server" meta:resourcekey="lblPostingPassword" Text="Posting password:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtPassword" runat="server" CssClass="form-control" Width="150px" TextMode = "Password"></asp:TextBox>
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
		<td class="NormalBold">
                <uc2:EmailControl id="txtModeratorEmail" runat="server">
                </uc2:EmailControl>
	    </td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblMaxMessageSize" runat="server" meta:resourcekey="lblMaxMessageSize" Text="Max Message Size, KB:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtMaxMessageSize" runat="server" CssClass="form-control" Width="50px">0</asp:TextBox>
		    <asp:CheckBox ID="chkMaxMessageSizeEnabled" runat="server" meta:resourcekey="chkMaxMessageSizeEnabled" Text="Enabled" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblMaxRecipients" runat="server" meta:resourcekey="lblMaxRecipients" Text="Max Recipients per Message:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtMaxRecipients" runat="server" CssClass="form-control" Width="50px">10</asp:TextBox>
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