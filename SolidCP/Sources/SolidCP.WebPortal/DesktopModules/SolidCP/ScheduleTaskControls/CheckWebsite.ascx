<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckWebsite.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.CheckWebsite" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="clpSendMessageIfHeader" runat="server"
    TargetControlID="pnlSendMessageIf" resourcekey="clpSendMessageIfHeader" Text="Send message if">
</scp:CollapsiblePanel>
<asp:Panel ID="pnlSendMessageIf" runat="server" CssClass="Normal">
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap>
					<asp:CheckBox ID="cbxResponseStatus" runat="server" meta:resourcekey="cbxResponseStatus" Text="Response status equal to" />
   				</td>
   				<td class="SubHead" width="100%">
   					<asp:TextBox ID="txtResponseStatus" runat="server" Width="30" CssClass="form-control" MaxLength="1000"></asp:TextBox>
   					<asp:CompareValidator ID="valResponseStatus" runat="server" ErrorMessage="*" Display="Static" ControlToValidate="txtResponseStatus" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator>
   				</td>
			</tr>
		</table>
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap style="padding-left: 40px;">
					<asp:CheckBox ID="cbxResponseContains" runat="server" meta:resourcekey="cbxResponseContains" Text="Response content contains" />
   				</td>
   				<td class="SubHead" width="100%">
   					<asp:TextBox ID="txtResponseContains" runat="server" Width="200" CssClass="form-control" MaxLength="1000"></asp:TextBox>
   				</td>
			</tr>
		</table>
		<table cellspacing="0" cellpadding="4" width="100%">
			<tr>
				<td class="SubHead" nowrap style="padding-left: 80px;">
					<asp:CheckBox ID="cbxResponseDoesntContain" runat="server" meta:resourcekey="cbxResponseDoesntContain" Text="Response content doesn't contain" />
   				</td>
   				<td class="SubHead" width="100%">
   					<asp:TextBox ID="txtResponseDoesntContain" runat="server" Width="200" CssClass="form-control" MaxLength="1000"></asp:TextBox>
   				</td>
			</tr>
		</table>
</asp:Panel>

	<br />	

<scp:CollapsiblePanel id="clpMessageHeader" runat="server"
    TargetControlID="pnlMessage" resourcekey="clpMessageHeader" Text="Configure target site and message as">
</scp:CollapsiblePanel>
<asp:Panel ID="pnlMessage" runat="server" CssClass="Normal">
	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblUrl" runat="server" meta:resourcekey="lblUrl" Text="URL:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtUrl" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblAccessUsername" runat="server" meta:resourcekey="lblAccessUsername" Text="Access Username:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtAccessUsername" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblAccessPassword" runat="server" meta:resourcekey="lblAccessPassword" Text="Access Password:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtAccessPassword" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblMailFrom" runat="server" meta:resourcekey="lblMailFrom" Text="Mail From:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtMailFrom" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
   				<asp:RegularExpressionValidator ID="mailFromRegExValidator" runat="server" ControlToValidate="txtMailFrom" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblMailTo" runat="server" meta:resourcekey="lblMailTo" Text="Mail To:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtMailTo" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
   				<asp:RegularExpressionValidator ID="mailToRegExValidator" runat="server" ControlToValidate="txtMailTo" ValidationExpression='^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$' Display="Static" ErrorMessage="*" />
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblMailSubject" runat="server" meta:resourcekey="lblMailSubject" Text="Mail Subject:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtMailSubject" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
			<td colspan="2" class="SubHead" nowrap>
				<asp:Label ID="lblMailBody" runat="server" meta:resourcekey="lblMailBody" Text="Mail Body:"></asp:Label>
			</td>
        </tr>
        <tr>
			<td colspan="2">
				<asp:TextBox ID="txtMailBody" runat="server" Width="95%" MaxLength="1000" CssClass="form-control" TextMode="MultiLine" Rows="10" ></asp:TextBox>
				<br />
				<asp:Label ID="lblMailBodyHint" runat="server" meta:resourcekey="lblMailBodyHint">
					([url], [content] variables are supported for "Check Web Site Availability Task")
				</asp:Label>
			</td>
        </tr>
	</table>
</asp:Panel>

	<br />	
	
	