<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail_EditList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail_EditList" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<%@ Register TagPrefix="scp" Namespace="SolidCP.WebPortal.Code.Controls" Assembly="SolidCP.WebPortal" %>
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
		    <asp:Label ID="lblModeratorAddress" runat="server" meta:resourcekey="lblModeratorAddress" Text="List Moderator:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList ID="ddlListModerators" DataTextField="Moderator" 
                runat="server" Width="200px">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqValModerator" runat="server" ControlToValidate="ddlListModerators"
                    Display="Dynamic" ErrorMessage = "*"/>
            <scp:DesktopContextValidator runat="server" ID="ctxValDomain" CssClass="QuickLabel" Display="Dynamic" EnableViewState="false"
			OnEvaluatingContext="ctxValDomain_EvaluatingContext" EnableClientScript="false" meta:resourcekey="ctxValDomain" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPostingPassword" runat="server" meta:resourcekey="lblPostingPassword" Text="Posting password:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtPassword" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
			<asp:CheckBox ID="chkPasswordEnabled" runat="server" meta:resourcekey="chkPasswordEnabled" Text="Enabled" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPostingMode" runat="server" resourcekey="lblPostingMode" Text="Posting mode:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlPostingMode" runat="server" resourcekey="ddlPostingMode" CssClass="form-control" Width="150px">
			    <asp:ListItem Value="AnyoneCanPost">Anyone</asp:ListItem>
				<asp:ListItem Value="MembersCanPost">MembersOnly</asp:ListItem>
				<asp:ListItem Value="ModeratorCanPost">ModeratorOnly</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblSubjectPrefix" runat="server" meta:resourcekey="lblSubjectPrefix" Text="Subject Prefix:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtSubjectPrefix" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
			<asp:CheckBox ID="chkSubjectPrefixEnabled" runat="server" meta:resourcekey="chkSubjectPrefixEnabled" Text="Enabled" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblMaxMessageSize" runat="server" meta:resourcekey="lblMaxMessageSize" Text="Max Message Size, KB:"></asp:Label>
		</td>
		<td class="normal">
			<asp:TextBox id="txtMaxMessageSize" runat="server" CssClass="form-control" Width="50px">0</asp:TextBox>
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
		<td class="Normal">&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead" valign="top">
		    <asp:Label ID="lblListOptions" runat="server" meta:resourcekey="lblListOptions" Text="List Options:"></asp:Label>
		</td>
		<td class="normal" valign="top">
            <table>
                <tr>
                    <td class="Normal" width="200">
                        <asp:CheckBox ID="chkReplyToList" runat="server" meta:resourcekey="chkReplyToList" Text="Reply To List" />
                    </td>
                    <td class="Normal"></td>
                </tr>
            </table>
		</td>
	</tr>
	<tr>
		<td class="Normal">&nbsp;</td>
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