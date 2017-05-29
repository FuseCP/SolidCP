<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBox.ascx.cs" EnableViewState="false" Inherits="SolidCP.Portal.MessageBox" %>
<%@ Register Src="CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<div style="height:3px;"></div>
<div id="tblMessageBox" runat="server" class="MessageBox">
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
    <asp:Literal ID="litDescription" runat="server"></asp:Literal>
    
     <div id="rowTechnicalDetails" runat="server" class="TechnicalDetails">
    
        <scp:CollapsiblePanel id="secTechnicalDetails" runat="server" IsCollapsed="true"
            TargetControlID="TechnicalDetailsPanel" resourcekey="secTechnicalDetails" Text="Technical Details">
        </scp:CollapsiblePanel>
        <asp:Panel ID="TechnicalDetailsPanel" runat="server" Height="0" style="overflow:hidden;">
            <table id="tblTechnicalDetails" runat="server" class="TechnicalDetailsTable" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="3">
                            <tr>
                                <td class="NormalBold" width="150" nowrap>
                                    <asp:Label ID="lblPageUrl" runat="server" meta:resourcekey="lblPageUrl" Text="Page URL:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:Literal ID="litPageUrl" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBold">
                                    <asp:Label ID="lblLoggedUser" runat="server" meta:resourcekey="lblLoggedUser" Text="Logged User:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:Literal ID="litLoggedUser" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBold">
                                    <asp:Label ID="lblWorkOnBehalf" runat="server" meta:resourcekey="lblWorkOnBehalf" Text="Work On Behalf:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:Literal ID="litSelectedUser" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBold">
                                    <asp:Label ID="lblSpace" runat="server" meta:resourcekey="lblSpace" Text="Active Space:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:Literal ID="litPackageName" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td class="NormalBold" valign="top">
                                    <asp:Label ID="lblStackTrace" runat="server" meta:resourcekey="lblStackTrace" Text="Stack Trace:"></asp:Label>
                                </td>
                                <td class="WrapText" valign="top">
                                    <asp:Literal ID="litStackTrace" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        
        <scp:CollapsiblePanel id="secSendReport" runat="server" IsCollapsed="true"
            TargetControlID="SendReportPanel" resourcekey="secSendReport" Text="Send Report to Host">
        </scp:CollapsiblePanel>
        <asp:Panel ID="SendReportPanel" runat="server" Height="0" style="overflow:hidden;">
            <table cellspacing="0" cellpadding="3">
                <tr>
                    <td class="NormalBold" width="150" nowrap>
                        <asp:Label ID="lblFrom" runat="server" meta:resourcekey="lblFrom" Text="From:"></asp:Label>
                    </td>
                    <td class="Normal">
                        <asp:Literal ID="litSendFrom" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="NormalBold">
                        <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To:"></asp:Label>
                    </td>
                    <td class="Normal">
                        <asp:Literal ID="litSendTo" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="NormalBold">
                        <asp:Label ID="lblCC" runat="server" meta:resourcekey="lblCC" Text="CC:"></asp:Label>
                    </td>
                    <td class="Normal">
                        <asp:Literal ID="litSendCC" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="NormalBold">
                        <asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label>
                    </td>
                    <td class="Normal">
                        <asp:Literal ID="litSendSubject" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="NormalBold" valign="top">
                        <asp:Label ID="lblComments" runat="server" meta:resourcekey="lblComments" Text="Personal Comments:"></asp:Label>
                    </td>
                    <td class="Normal" valign="top">
                        <asp:TextBox ID="txtSendComments" runat="server" CssClass="LogArea TechnicalDetailsTable" Rows="5" TextMode="MultiLine"
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSend" runat="server" meta:resourcekey="btnSend" Text="Send Report" CssClass="Button2" CausesValidation="false" OnClick="btnSend_Click" />
                        <asp:Label ID="lblSentMessage" runat="server" meta:resourcekey="lblSentMessage"
                            Text="Message has been sent" CssClass="NormalBold" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</div>