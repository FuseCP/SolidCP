<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail60_EditList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail60_EditList" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<%@ Register TagPrefix="scp" Namespace="SolidCP.WebPortal.Code.Controls" Assembly="SolidCP.WebPortal" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

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
		<td class="SubHead">
		    <asp:Label ID="lblModeratorAddress" runat="server" meta:resourcekey="lblModeratorAddress" Text="List Moderator:"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList ID="ddlListModerators" DataTextField="Moderator" 
                runat="server" Height="25px" Width="200px">
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
		<td width="200px" align="right>
		    <asp:Label ID="lblListOptions" runat="server" meta:resourcekey="lblListOptions" Text="List Options:"></asp:Label>
		</td>
		<td>
            <asp:CheckBox ID="chkReplyToList" runat="server" meta:resourcekey="chkReplyToList" Text="Reply To List" />
        </td>
        <td class="Normal"></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblListToAddress" runat="server" resourcekey="lblListToAddress"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlListToAddress" runat="server" resourcekey="ddlListToAddress" CssClass="form-control" Width="150px">
			    <asp:ListItem Value="DEFAULT">Default</asp:ListItem>
				<asp:ListItem Value="LISTADDRESS">List Address</asp:ListItem>
				<asp:ListItem Value="SUBSCRIBERADDRESS">Subscriber Address</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblListFromAddress" runat="server" resourcekey="lblListFromAddress"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlListFromAddress" runat="server" resourcekey="ddlListFromAddress" CssClass="form-control" Width="150px">
			    <asp:ListItem Value="LISTADDRESS">List Address</asp:ListItem>
				<asp:ListItem Value="POSTERADDRESS">Poster Address</asp:ListItem>
		    </asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblListReplyToAddress" runat="server" resourcekey="lblListReplyToAddress"></asp:Label>
		</td>
		<td class="normal">
			<asp:DropDownList id="ddlListReplyToAddress" runat="server" resourcekey="ddlListReplyToAddress" CssClass="form-control" Width="150px">
			    <asp:ListItem Value="LISTADDRESS">List Address</asp:ListItem>
				<asp:ListItem Value="POSTERADDRESS">Poster Address</asp:ListItem>
		    </asp:DropDownList>
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
&nbsp
<scp:collapsiblepanel id="AdditionalOptions" runat="server" targetcontrolid="pAdditionalOptions"
    meta:resourcekey="AdditionalOptions" ></scp:collapsiblepanel>
<asp:Panel runat="server" ID="pAdditionalOptions">
   <table width="100%">
    <tr>
        <td width="200px" align="right"><asp:Label ID="lbDigestMode" runat="server" meta:resourcekey="lbDigestMode" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbDigestMode"/>
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="lbSendSubscribe" runat="server" meta:resourcekey="lbSendSubscribe" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbSendSubcsribe"/>
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="lbSendUnsubscribe" runat="server" meta:resourcekey="lbSendUnsubscribe" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbSendUnsubscribe"/>
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="lbAllowUnsubscribe" runat="server" meta:resourcekey="lbAllowUnsubscribe" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbAllowUnsubscribe"/>
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="lbDisableListcommand" runat="server" meta:resourcekey="lbDisableListcommand" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbDisableListcommand"/>
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="lbDisableSubscribecommand" runat="server" meta:resourcekey="lbDisableSubscribecommand" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbDisableSubscribecommand"/>
        </td>
    </tr>
 </table>
 </asp:Panel>