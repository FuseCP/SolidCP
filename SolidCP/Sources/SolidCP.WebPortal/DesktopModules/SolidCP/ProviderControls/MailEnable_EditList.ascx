<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailEnable_EditList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MailEnable_EditList" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<%@ Register Src="../UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

<table cellSpacing="0" cellPadding="3" width="100%">
    <tr>
		<td class="SubHead" style="width:150px;">
		    <asp:Label ID="lblDescription" runat="server" meta:resourcekey="lblDescription" Text="List Description:"></asp:Label>
		</td>
		<td class="normal">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Width="300px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblReplyTo" runat="server" meta:resourcekey="lblReplyTo" Text="Subscribers reply to:"></asp:Label>
		</td>
		<td class="normal">
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
			<asp:DropDownList id="ddlPostingMode" runat="server" 
                resourcekey="ddlPostingMode" CssClass="form-control" Width="210px" 
                Height="22px">
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
			<asp:TextBox id="txtPassword" runat="server" CssClass="form-control"  TextMode="Password" Width="150px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="Label3" runat="server" meta:resourcekey="lblPrefixOptions" Text="Subject Prefix Options:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlPrefixOption" runat="server" 
                resourcekey="ddlPrefixOption" CssClass="form-control" Width="210px">
				<asp:ListItem Value="Default">Default Prefix</asp:ListItem>
				<asp:ListItem Value="Altered">Subject is not altered</asp:ListItem>
				<asp:ListItem Value="CustomPrefix">Custom Prefix</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="Label4" runat="server" meta:resourcekey="lblPrefix" Text="Prefix:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtSubjectPrefix" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
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
		<td class="SubHead" vAlign="top">
		    <asp:Label ID="lblMembers" runat="server" meta:resourcekey="lblMembers" Text="Mailing list members:"></asp:Label>
		</td>
		<td vAlign="top">
			<dnc:EditItemsList id="mailEditItems" runat="server"></dnc:EditItemsList>
		</td>
	</tr>
</table>

<scp:collapsiblepanel id="HeaderFooterSection" runat="server" targetcontrolid="pHeaderFooter"
    meta:resourcekey="HeaderFooterSection" ></scp:collapsiblepanel>
<asp:Panel runat="server" ID="pHeaderFooter">
   <table width="100%">
    <tr>
		<td>&nbsp;</td>
	</tr>
      <tr>
      	<td style="width:150px;">
		    <asp:Label ID="lblHeader" runat="server" meta:resourcekey="lblHeader" Text="Header:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="cbHeader" Runat="server" meta:resourcekey="cbHeader" Text="Attach header"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
	    <td></td>
	    <td class="SubHead">
		    <asp:Label ID="lblHeaderText" runat="server" meta:resourcekey="lblHeaderText" Text="Header for plain text messages"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
		 </td>
		<td class="normal">
			<asp:TextBox ID="txtHeaderText" runat="server" TextMode="MultiLine" Rows="7" Width="300px"></asp:TextBox>
		</td>
	</tr>
	<tr>
	    <td></td>
		<td class="SubHead">
		    <asp:Label ID="lblHeaderHtml" runat="server" meta:resourcekey="lblHeaderHtml" Text="Header for HTML messages"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
		 </td>
		<td class="normal">
			<asp:TextBox ID="txtHeaderHtml" runat="server" TextMode="MultiLine" Rows="7" Width="300px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td>
		    <asp:Label ID="lblFooter" runat="server" meta:resourcekey="lblFooter" Text="Footer:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="cbFooter" Runat="server" meta:resourcekey="cbFooter" Text="Attach Footer"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
	    <td></td>
	    <td class="SubHead">
		    <asp:Label ID="lblFooterText" runat="server" meta:resourcekey="lblFooterText" Text="Footer for plain text messages"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
		 </td>
		<td class="normal">
			<asp:TextBox ID="txtFooterText" runat="server" TextMode="MultiLine" Rows="7" Width="300px"></asp:TextBox>
		</td>
	</tr>
	<tr>
	    <td></td>
		<td class="SubHead">
		    <asp:Label ID="lblFooterHtml" runat="server" meta:resourcekey="lblFooterHtml" Text="Footer for HTML messages"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
		 </td>
		<td class="normal">
			<asp:TextBox ID="txtFooterHtml" runat="server" TextMode="MultiLine" Rows="7" Width="300px"></asp:TextBox>
		</td>
	</tr>
   </table>
 </asp:Panel>