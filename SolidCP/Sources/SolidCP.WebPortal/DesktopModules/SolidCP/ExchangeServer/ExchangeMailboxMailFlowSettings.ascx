<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxMailFlowSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxMailFlowSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/AcceptedSenders.ascx" TagName="AcceptedSenders" TagPrefix="scp" %>
<%@ Register Src="UserControls/RejectedSenders.ascx" TagName="RejectedSenders" TagPrefix="scp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeMailbox48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                    </h3>
                </div>
				<div class="panel-body form-horizontal">
                     <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_mailflow" />	
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
					<scp:CollapsiblePanel id="secForwarding" runat="server"
                        TargetControlID="Forwarding" meta:resourcekey="secForwarding" Text="Forwarding Address">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="Forwarding" runat="server" Height="0" style="overflow:hidden;">
						<asp:UpdatePanel ID="ForwardingUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>
							
					    <table>
							<tr>
								<td>
									<asp:CheckBox ID="chkEnabledForwarding" runat="server" meta:resourcekey="chkEnabledForwarding" Text="Enable Forwarding" AutoPostBack="true" OnCheckedChanged="chkEnabledForwarding_CheckedChanged" />
								</td>
							</tr>
						</table>
						<table id="ForwardSettingsPanel" runat="server">
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locForwardTo" runat="server" meta:resourcekey="locForwardTo" Text="Forward To:"></asp:Localize></td>
							    <td>
									<scp:MailboxSelector id="forwardingAddress" runat="server"
											MailboxesEnabled="true"
											ContactsEnabled="true"
											DistributionListsEnabled="true" />
								</td>
						    </tr>
						    <tr>
								<td></td>
								<td>
									<asp:CheckBox ID="chkDoNotDeleteOnForward" runat="server" meta:resourcekey="chkDoNotDeleteOnForward" Text="Deliver messages to both forwarding address and mailbox" />
								</td>
						    </tr>
						</table>
						
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>


					<scp:CollapsiblePanel id="secSendOnBehalf" runat="server"
                        TargetControlID="SendOnBehalf" meta:resourcekey="secSendOnBehalf" Text="Send On Behalf">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="SendOnBehalf" runat="server" Height="0" style="overflow:hidden;">
					    <table>
							<tr>
								<td>
									<asp:Localize ID="locGrantAccess" runat="server" meta:resourcekey="locGrantAccess" Text="Grant this permission to:"></asp:Localize>
								</td>
							</tr>
							<tr>
								<td>
									<scp:AccountsList id="accessAccounts" runat="server"
											MailboxesEnabled="true" />
								</td>
							</tr>
					    </table>
					</asp:Panel>
					
					
					<scp:CollapsiblePanel id="secAcceptMessagesFrom" runat="server"
                        TargetControlID="AcceptMessagesFrom" meta:resourcekey="secAcceptMessagesFrom" Text="Accept Messages From">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="AcceptMessagesFrom" runat="server" Height="0" style="overflow:hidden;">
					    <scp:AcceptedSenders id="acceptAccounts" runat="server" />
					    <asp:CheckBox ID="chkSendersAuthenticated" runat="server" meta:resourcekey="chkSendersAuthenticated" Text="Require that all senders are authenticated" />
					</asp:Panel>
					
					
					<scp:CollapsiblePanel id="secRejectMessagesFrom" runat="server"
                        TargetControlID="RejectMessagesFrom" meta:resourcekey="secRejectMessagesFrom" Text="Reject Messages From">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="RejectMessagesFrom" runat="server" Height="0" style="overflow:hidden;">
					    <scp:RejectedSenders id="rejectAccounts" runat="server" />
					</asp:Panel>
					
					<table style="width:100%;margin-top:10px;">
					    <tr>
					        <td align="center">
					            <asp:CheckBox ID="chkPmmAllowed" runat="server" meta:resourcekey="chkPmmAllowed" AutoPostBack="true" Visible="false"
					                Text="Allow these settings to be managed from Personal Mailbox Manager" OnCheckedChanged="chkPmmAllowed_CheckedChanged" />
					        </td>
					    </tr>
					</table>
					

				</div>
                    </div>
				    <div class="panel-footer text-right">
                        <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="EditMailbox" 
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
				    </div>