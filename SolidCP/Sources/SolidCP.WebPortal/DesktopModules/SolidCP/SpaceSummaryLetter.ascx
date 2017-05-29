<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSummaryLetter.ascx.cs" Inherits="SolidCP.Portal.SpaceSummaryLetter" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">

    <scp:CollapsiblePanel id="secEmail" runat="server"
        TargetControlID="EmailPanel" meta:resourcekey="secEmail" Text="Send via E-Mail">
    </scp:CollapsiblePanel>
	<asp:Panel ID="EmailPanel" runat="server" Height="0" style="overflow:hidden;">
        <table id="tblEmail" runat="server" cellpadding="2">
            <tr>
                <td class="SubHead" width="30" nowrap>
                    <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To:"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtTo" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireEmail" runat="server" ControlToValidate="txtTo" Display="Dynamic"
                        ErrorMessage="Enter e-mail" ValidationGroup="SendEmail" meta:resourcekey="valRequireEmail"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblCC" runat="server" meta:resourcekey="lblCC" Text="CC:"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtCC" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnSend" runat="server" CssClass="Button2" meta:resourcekey="btnSend" Text="Send" OnClick="btnSend_Click" ValidationGroup="SendEmail" OnClientClick="ShowProgressDialog('Sending...');" /></td>
            </tr>
        </table>
    </asp:Panel>

    <div class="PreviewArea">
        <asp:Literal ID="litContent" runat="server"></asp:Literal>
    </div>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnReturn" CssClass="btn btn-warning" runat="server" OnClick="btnReturn_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnReturnText"/> </CPCC:StyleButton>
</div>