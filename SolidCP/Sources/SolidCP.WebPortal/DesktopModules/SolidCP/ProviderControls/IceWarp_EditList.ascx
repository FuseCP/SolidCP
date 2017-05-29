<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IceWarp_EditList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.IceWarp_EditList" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

<asp:Panel runat="server" ID="MainPanel">
        <table width="100%">
    	    <tr>
		        <td class="SubHead" width="150">
		            <asp:Label ID="lblDescription" runat="server" meta:resourcekey="lblDescription" Text="List Description:"></asp:Label>
		        </td>
		        <td class="Normal">
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Width="300px"></asp:TextBox>
		        </td>
	        </tr>
	        <tr>
		        <td class="SubHead">
		            <asp:Label ID="lblModeratorAddress" runat="server" meta:resourcekey="lblModeratorAddress" Text="List Moderator:"></asp:Label>
		        </td>
		        <td class="Normal">
			        <asp:DropDownList ID="ddlListModerators" DataTextField="Name" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqValModerator" runat="server" ControlToValidate="ddlListModerators" Display="Dynamic" ErrorMessage = "*" />
		        </td>
	        </tr>
        </table>
    <asp:UpdatePanel ID="MembersUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
	        <tr>
		        <td class="SubHead" width="150">
		            <asp:Label ID="lblMembersSource" runat="server" meta:resourcekey="lblMembersSource" Text="Members Source:"></asp:Label>
		        </td>
		        <td class="Normal">
			        <asp:DropDownList ID="ddlMembersSource" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMembersSource_SelectedIndexChanged">
			            <asp:ListItem Value="MembersInFile" Text="From members list on this page" meta:resourcekey="ddlMembersSourceInFile" />
                        <asp:ListItem Value="AllDomainUsers" Text="All Domains Users" meta:resourcekey="ddlMembersSourceAllDomainUsers" />
                        <asp:ListItem Value="AllDomainAdmins" Text="All Domains Admins" meta:resourcekey="ddlMembersSourceAllDomainAdmins" />
                    </asp:DropDownList>
		        </td>
	        </tr>
            <tr id="MembersRow" runat="server">
		        <td class="SubHead">
		            <asp:Label ID="lblMembers" runat="server" meta:resourcekey="lblMembers" Text="Mailing list members:"></asp:Label>
		        </td>
		        <td class="Normal">
			        <dnc:EditItemsList id="mailEditItems" runat="server"></dnc:EditItemsList>
		        </td>
            </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>    

<scp:CollapsiblePanel id="Message" runat="server" targetcontrolid="MessagePanel" meta:resourcekey="Message" Text="Message"></scp:CollapsiblePanel>
<asp:Panel runat="server" ID="MessagePanel">
    <asp:UpdatePanel ID="FromHeaderUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%">
	        <tr>
		        <td class="SubHead" width="150">
		            <asp:Label ID="lblFromHeader" runat="server" meta:resourcekey="lblFromHeader" Text="From Header:"></asp:Label>
		        </td>
		        <td class="Normal">
			        <asp:DropDownList id="ddlFromHeaderAction" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFromHeaderAction_SelectedIndexChanged">
			            <asp:ListItem Value="NoChange" Text="No change" meta:resourcekey="HeaderActionNoChange"></asp:ListItem>
				        <asp:ListItem Value="SetToSender" Text="Set to sender" meta:resourcekey="HeaderActionSetToSender"></asp:ListItem>
                        <asp:ListItem Value="SetToValue" Text="Set to value" meta:resourcekey="HeaderActionSetToValue"></asp:ListItem>
			        </asp:DropDownList>
		        </td>
	        </tr>
	        <tr runat="server" ID="rowFromHeaderValue" Visible="False">
		        <td class="SubHead">
		            <asp:Label ID="lblFromHeaderValue" runat="server" meta:resourcekey="lblFromHeaderValue" Text="From Header Value:"></asp:Label>
		        </td>
		        <td class="Normal">
			        <asp:TextBox id="txtFromHeaderValue" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValFromHeaderValue" runat="server" ControlToValidate="txtFromHeaderValue" Display="Dynamic" ErrorMessage = "*" />
		        </td>
	        </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="ReplyToHeaderUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%">
	        <tr>
		        <td class="SubHead" width="150">
		            <asp:Label ID="lblReplyToHeader" runat="server" meta:resourcekey="lblReplyToHeader" Text="Reply-To Header:"></asp:Label>
		        </td>
		        <td class="Normal">
			        <asp:DropDownList id="ddlReplyToHeaderAction" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlReplyToHeaderAction_SelectedIndexChanged">
			            <asp:ListItem Value="NoChange" Text="No change" meta:resourcekey="HeaderActionNoChange"></asp:ListItem>
				        <asp:ListItem Value="SetToSender" Text="Set to sender" meta:resourcekey="HeaderActionSetToSender"></asp:ListItem>
                        <asp:ListItem Value="SetToValue" Text="Set to value" meta:resourcekey="HeaderActionSetToValue"></asp:ListItem>
			        </asp:DropDownList>
		        </td>
	        </tr>
	        <tr runat="server" ID="rowReplyToHeaderValue" Visible="False">
		        <td class="SubHead">
		            <asp:Label ID="lblReplyToHeaderValue" runat="server" meta:resourcekey="lblReplyToHeaderValue" Text="Reply-To Header Value:"></asp:Label>
		        </td>
		        <td class="Normal">
			        <asp:TextBox id="txtReplyToHeaderValue" runat="server" CssClass="form-control"></asp:TextBox>
		            <asp:RequiredFieldValidator ID="reqValReplyToHeaderValue" runat="server" ControlToValidate="txtReplyToHeaderValue" Display="Dynamic" Text="*" />
		        </td>
	        </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
	    <tr>
		    <td colspan="2">
                <asp:CheckBox ID="chkSetRecipientToToHeader" runat="server" meta:resourcekey="chkSetRecipientToToHeader" Text="Set recipient to To header" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead" width="150">
		        <asp:Label ID="lblSubjectPrefix" runat="server" meta:resourcekey="lblSubjectPrefix" Text="Subject Prefix:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:TextBox id="txtSubjectPrefix" runat="server" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblOriginator" runat="server" meta:resourcekey="lblOriginator" Text="Originator:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:DropDownList id="ddllblOriginator" runat="server">
			        <asp:ListItem Value="Blank" Text="Blank" meta:resourcekey="OriginatorBlank"></asp:ListItem>
				    <asp:ListItem Value="Sender" Text="Sender" meta:resourcekey="OriginatorSender"></asp:ListItem>
                    <asp:ListItem Value="Owner" Text="Owner" Selected="True" meta:resourcekey="OriginatorOwner"></asp:ListItem>
			    </asp:DropDownList>
		    </td>
	    </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="Security" runat="server" targetcontrolid="SecurityPanel" meta:resourcekey="Security" Text="Security"></scp:CollapsiblePanel>
<asp:Panel runat="server" ID="SecurityPanel">
    <table width="100%">
	    <tr>
		    <td class="SubHead" width="150">
		        <asp:Label ID="lblPostingMode" runat="server" meta:resourcekey="lblPostingMode" Text="Posting mode:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:DropDownList id="ddlPostingMode" runat="server">
			        <asp:ListItem Value="AnyoneCanPost" meta:resourcekey="ddlPostingModeAnyone">Anyone</asp:ListItem>
				    <asp:ListItem Value="MembersCanPost" meta:resourcekey="ddlPostingModeMembers">MembersOnly</asp:ListItem>
			    </asp:DropDownList>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblPasswordProtection" runat="server" meta:resourcekey="lblPasswordProtection" Text="Password Protection:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:DropDownList id="ddlPasswordProtection" runat="server" resourcekey="ddlPasswordProtection" AutoPostBack="True" OnSelectedIndexChanged="ddlPasswordProtection_SelectedIndexChanged">
				    <asp:ListItem Value="NoProtection" Text="Not password protected" meta:resourcekey="PasswordProtectionNoProtection"></asp:ListItem>
				    <asp:ListItem Value="ServerModerated" Text="Server Moderated" meta:resourcekey="PasswordProtectionServerModerated"></asp:ListItem>
				    <asp:ListItem Value="ClientModerated" Text="Client Moderated" meta:resourcekey="PasswordProtectionClientModerated"></asp:ListItem>
			    </asp:DropDownList>
		    </td>
	    </tr>
	    <tr runat="server" ID="rowPostingPassword" Visible="False">
		    <td class="SubHead">
		        <asp:Label ID="lblPostingPassword" runat="server" meta:resourcekey="lblPostingPassword" Text="Posting password:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:TextBox id="txtPassword" runat="server" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
        <asp:UpdatePanel ID="ModeratedUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkModerated" runat="server" meta:resourcekey="chkModerated" Text="Moderated listserver" AutoPostBack="True" OnCheckedChanged="chkModerated_CheckedChanged"></asp:CheckBox>
            </td>
        </tr>
	    <tr runat="server" ID="rowCommandPassword" Visible="False">
		    <td class="SubHead">
		        <asp:Label ID="lblCommandPassword" runat="server" meta:resourcekey="lblCommandPassword" Text="Command password:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:TextBox id="txtCommandPassword" runat="server" CssClass="form-control"></asp:TextBox>
		    </td>
	    </tr>
        </ContentTemplate>
        </asp:UpdatePanel>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblDefaultRights" runat="server" meta:resourcekey="lblDefaultRights" Text="Default Rights:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:DropDownList id="ddlDefaultRights" runat="server" meta:resourcekey="ddlDefaultRights">
				    <asp:ListItem Value="0" Text="Receive and post" meta:resourcekey="DefaultRightsReceivePost"></asp:ListItem>
				    <asp:ListItem Value="7" Text="Digest, receive and post" meta:resourcekey="DefaultRightsDigestReceivePost"></asp:ListItem>
				    <asp:ListItem Value="1" Text="Receive only" meta:resourcekey="DefaultRightsReceive"></asp:ListItem>
                    <asp:ListItem Value="5" Text="Digest and receive" meta:resourcekey="DefaultRightsDigestReceive"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Post only" meta:resourcekey="DefaultRightsPost"></asp:ListItem>
			    </asp:DropDownList>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblMaxMessageSize" runat="server" meta:resourcekey="lblMaxMessageSize" Text="Max Message Size, kB:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:TextBox id="txtMaxMessageSize" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                <asp:RangeValidator Type="Integer" ID="txtMaxMessageSizeValidator" MinimumValue="0" runat="server" ControlToValidate="txtMaxMessageSize"
                    Display="Dynamic" meta:resourcekey="txtMaxMessageSizeValidator" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtMaxMessageSizeRequiredValidator" ControlToValidate="txtMaxMessageSize" Display="Dynamic" Text="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblMaxMembers" runat="server" meta:resourcekey="lblMaxMembers" Text="Max members count:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:TextBox id="txtMaxMembers" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                <asp:RangeValidator Type="Integer" ID="txtMaxMembersValidator" MinimumValue="0" runat="server" ControlToValidate="txtMaxMembers"
                    Display="Dynamic" meta:resourcekey="txtMaxMembersValidator" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtMaxMembersRequiredValidator" ControlToValidate="txtMaxMembers" Display="Dynamic" Text="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
		    </td>
	    </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="Options" runat="server" targetcontrolid="OptionsPanel" meta:resourcekey="Options" Text="Options"></scp:CollapsiblePanel>
<asp:Panel runat="server" ID="OptionsPanel">
   <table width="100%">
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkSendToSender" runat="server" meta:resourcekey="chkSendToSender" Text="Send to sender" Checked="True"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkDigestMailingList" runat="server" meta:resourcekey="chkDigestMailingList" Text="DigestMailingList"></asp:CheckBox>
            </td>
        </tr>
	    <tr>
		    <td class="SubHead" width="150">
		        <asp:Label ID="lblMaxMessagesPerMinute" runat="server" meta:resourcekey="lblMaxMessagesPerMinute" Text="Max # of messages to send out in 1 minute:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:TextBox id="txtMaxMessagesPerMinute" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                <asp:RangeValidator Type="Integer" ID="txtMaxMessagesPerMinuteValidator" MinimumValue="0" runat="server" ControlToValidate="txtMaxMessagesPerMinute"
                    Display="Dynamic" meta:resourcekey="txtMaxMessagesPerMinuteValidator" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtMaxMessagesPerMinuteRequiredValidator" ControlToValidate="txtMaxMessagesPerMinute" Display="Dynamic" Text="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
		    </td>
	    </tr>       
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkSendSubscribe" runat="server" meta:resourcekey="chkSendSubscribe" Text="Notify owner of join"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkSendUnSubscribe" runat="server" meta:resourcekey="chkSendUnSubscribe" Text="Notify owner of leave"></asp:CheckBox>
            </td>
        </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblConfirmSubscription" runat="server" meta:resourcekey="lblConfirmSubscription" Text="Subscription:"></asp:Label>
		    </td>
		    <td class="Normal">
			    <asp:DropDownList id="ddlConfirmSubscription" runat="server">
				    <asp:ListItem Value="None" Text="No confirmation" meta:resourcekey="ConfirmSubscriptionNone"></asp:ListItem>
				    <asp:ListItem Value="User" Text="User confirmed" meta:resourcekey="ConfirmSubscriptionUser"></asp:ListItem>
				    <asp:ListItem Value="Owner" Text="Owner confirmed" meta:resourcekey="ConfirmSubscriptionOwner"></asp:ListItem>
			    </asp:DropDownList>
		    </td>
	    </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkCommandInSubject" runat="server" meta:resourcekey="chkCommandInSubject" Text="Allow command in subject"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkEnableSubscribe" runat="server" meta:resourcekey="chkEnableSubscribe" Text="Enable join (subscribe) command" Checked="True"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkEnableUnsubscribe" runat="server" meta:resourcekey="chkEnableUnsubscribe" Text="Enable leave (unsubscribe) command" Checked="True"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkEnableLists" runat="server" meta:resourcekey="chkEnableLists" Text="Enable Lists command" Checked="True"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkEnableWhich" runat="server" meta:resourcekey="chkEnableWhich" Text="Enable Which command"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkEnableReview" runat="server" meta:resourcekey="chkEnableReview" Text="Enable Review command"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkEnableVacation" runat="server" meta:resourcekey="chkEnableVacation" Text="Enable Vacation/No vacation command" Checked="True"></asp:CheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="chkSuppressCommandResponses" runat="server" meta:resourcekey="chkSuppressCommandResponses" Text="Suppress command responses"></asp:CheckBox>
            </td>
        </tr>
 </table>
 </asp:Panel>