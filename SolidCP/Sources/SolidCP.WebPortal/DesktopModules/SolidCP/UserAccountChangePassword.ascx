<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountChangePassword.ascx.cs"
    Inherits="SolidCP.Portal.UserAccountChangePassword" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<asp:Panel ID="PasswordPanel" runat="server" DefaultButton="cmdChangePassword">
    <div class="panel-body form-horizontal">
        <table cellspacing="0" cellpadding="2" width="100%">
            <tr>
                <td class="col-sm-2 control-label">
                    <asp:Label ID="lblUsername2" runat="server" meta:resourcekey="lblUsername" Text="Username:"></asp:Label>
                </td>
                <td>
                    <strong><asp:Literal ID="lblUsername" runat="server"></asp:Literal></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <scp:PasswordControl ID="userPassword" runat="server" />
                </td>
            </tr>
            <tr id="trChangePasswordWarning" runat="server" visible="false">
                <td>
                </td>
                <td>
                    <asp:Label ID="lblChangePasswordWarning" runat="server" CssClass="ErrorText">Warning: This will end the current session.</asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
        <CPCC:StyleButton id="cmdChangePassword" CssClass="btn btn-success" runat="server" OnClick="cmdChangePassword_Click" ValidationGroup="NewPassword"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="cmdChangePassword"/> </CPCC:StyleButton>
    </div>
</asp:Panel>
