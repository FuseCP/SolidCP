<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationUserSetupInstructions.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationUserSetupInstructions" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>
<%@ Register src="UserControls/UserTabs.ascx" tagname="UserTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/DaysBox.ascx" TagName="DaysBox" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>					
                </h3>
                        </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:UserTabs ID="UserTabs" runat="server" SelectedTab="organization_user_setup" />
                    <scp:MailboxTabs id="MailboxTabs" runat="server" SelectedTab="organization_user_setup" IsADUserTabs="true" />
                    </div>
                    <div class="panel panel-default tab-content">	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />

                    <scp:CollapsiblePanel id="secEmail" runat="server" IsCollapsed="true"
                        TargetControlID="EmailPanel" meta:resourcekey="secEmail" Text="Send via E-Mail">
                    </scp:CollapsiblePanel>
	                <asp:Panel ID="EmailPanel" runat="server" Height="0" style="overflow:hidden;">
                        <table id="tblEmail" runat="server" cellpadding="2">
                            <tr>
                                <td class="SubHead" width="30" nowrap>
                                    <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequireEmail" runat="server" ControlToValidate="txtTo" Display="Dynamic"
                                        ErrorMessage="Enter e-mail" ValidationGroup="SendEmail" meta:resourcekey="valRequireEmail"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="SubHead">
                                    <asp:Label ID="lblCC" runat="server" meta:resourcekey="lblCC" Text="CC:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:TextBox ID="txtCC" runat="server" CssClass="form-control" Width="300px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSend" runat="server" CssClass="Button2" meta:resourcekey="btnSend" Text="Send" OnClick="btnSend_Click" ValidationGroup="SendEmail" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
					
                    <div class="PreviewArea">
                        <asp:Literal ID="litContent" runat="server"></asp:Literal>
                    </div>
					</div>
				</div>