<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDistributionListMailFlowSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeDistributionListMailFlowSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/DistributionListTabs.ascx" TagName="DistributionListTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/AcceptedSenders.ascx" TagName="AcceptedSenders" TagPrefix="scp" %>
<%@ Register Src="UserControls/RejectedSenders.ascx" TagName="RejectedSenders" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeList48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Distribution List"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                        </h3>
                </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:DistributionListTabs id="tabs" runat="server" SelectedTab="dlist_mailflow" />	
                    </div>
                    <div class="panel panel-default tab-content">
					<scp:SimpleMessageBox id="messageBox" runat="server" />
					
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
					

				</div>
			</div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditMailbox"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
				    </div>