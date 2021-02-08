<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeJournalingMailboxGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeJournalingMailboxGeneralSettings" %>
<%@ Register Src="UserControls/CountrySelector.ascx" TagName="CountrySelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxPlanSelector.ascx" TagName="MailboxPlanSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />


<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="Image1" SkinID="ExchangeJournalingMailbox48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
        -
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
        <asp:Image ID="imgVipUser" SkinID="VipUser16" runat="server" ToolTip="VIP user" Visible="false" />
        <asp:Label ID="litServiceLevel" runat="server" Style="float: right; padding-right: 8px;" Visible="false"></asp:Label>
    </h3>
</div>
<div class="panel-body form-horizontal">
    <div class="nav nav-tabs" style="padding-bottom: 7px !important;">
        <scp:MailboxTabs ID="tabs" runat="server" SelectedTab="journaling_mailbox_settings" />
    </div>
    <div class="panel panel-default tab-content">
        <scp:SimpleMessageBox ID="messageBox" runat="server" />
        <asp:UpdatePanel ID="GeneralUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>

                <scp:CollapsiblePanel ID="secGeneral" runat="server" TargetControlID="General" meta:ResourceKey="secGeneral" Text="General"></scp:CollapsiblePanel>
                <asp:Panel ID="General" runat="server" Height="0" Style="overflow: hidden;">
                    <table>
                        <tr>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="chkDisable" runat="server" meta:resourcekey="chkDisable" Text="Disable Mailbox" />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="Localize2" runat="server" meta:resourcekey="locMailboxplanName" Text="Mailbox plan: *"></asp:Localize></td>
                            <td>
                                <scp:MailboxPlanSelector ID="mailboxPlanSelector" runat="server" IsForJournaling="true" OnChanged="mailboxPlanSelector_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Mailbox Size:"></asp:Localize></td>
                            <td>
                                <scp:QuotaViewer ID="mailboxSize" runat="server" QuotaTypeId="2" DisplayGauge="true" />
                                MB
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <scp:CollapsiblePanel ID="secJournaling" runat="server" TargetControlID="Journaling" meta:ResourceKey="secJournaling" Text="Journal Rules"></scp:CollapsiblePanel>
                <asp:Panel ID="Journaling" runat="server" Height="0" Style="overflow: hidden;">
                    <table>
                        <tr>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="cbEnabled" runat="server" meta:resourcekey="locEnabled" Text="Enable journal rule" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locScope" runat="server" meta:resourcekey="locScope" Text="If the message is sent to or received from:"></asp:Localize></td>
                            <td>
                                <asp:DropDownList ID="ddlScope" runat="server" CssClass="form-control col-sm-4">
                                    <asp:ListItem Value="Global" meta:resourcekey="ddlScopeGlobal">All messages</asp:ListItem>
                                    <asp:ListItem Value="Internal" meta:resourcekey="ddlScopeInternal">Internal messages only</asp:ListItem>
                                    <asp:ListItem Value="External" meta:resourcekey="ddlScopeExternal">External messages only</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locRecipient" runat="server" meta:resourcekey="locRecipient" Text="Journal messages sent to or received from:"></asp:Localize></td>
                            <td>
                                <asp:DropDownList ID="ddlRecipient" runat="server" CssClass="form-control col-sm-4" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>

        <scp:CollapsiblePanel ID="secAdvancedInfo" runat="server" TargetControlID="AdvancedInfo" meta:ResourceKey="secAdvancedInfo" Text="Advanced Information" IsCollapsed="true"></scp:CollapsiblePanel>
        <asp:Panel ID="AdvancedInfo" runat="server" Height="0" Style="overflow: hidden;">
            <table>
                <tr>
                    <td class="FormLabel150">
                        <asp:Localize ID="locExchangeGuid" runat="server" meta:resourcekey="locExchangeGuid" Text="Exchange Guid:"></asp:Localize></td>
                    <td>
                        <asp:Label runat="server" ID="lblExchangeGuid" /></td>
                </tr>
            </table>
        </asp:Panel>


    </div>
</div>
<div class="panel-footer text-right">
    <scp:ItemButtonPanel ID="buttonPanel" runat="server" ValidationGroup="EditMailbox"
        OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
</div>
