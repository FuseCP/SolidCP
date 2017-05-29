<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RejectedSenders.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.RejectedSenders" %>
<%@ Register Src="AccountsList.ascx" TagName="AccountsList" TagPrefix="scp" %>

<asp:UpdatePanel ID="MainUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>

<table>
	<tr>
		<td>
			<asp:RadioButtonList ID="rblRejectMessages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblRejectMessages_SelectedIndexChanged">
				<asp:ListItem Text="No senders" meta:resourcekey="rblRejectMessagesNo"></asp:ListItem>
				<asp:ListItem Text="Senders in the following list" meta:resourcekey="rblRejectMessagesOnlyList"></asp:ListItem>
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td>
			<scp:AccountsList id="rejectAccounts" runat="server"
					MailboxesEnabled="true"
					ContactsEnabled="true"
					DistributionListsEnabled="false">
			</scp:AccountsList>
		</td>
	</tr>
</table>

	</ContentTemplate>
</asp:UpdatePanel>