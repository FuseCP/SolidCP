<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrgIdPolicyEditor.ascx.cs" Inherits="SolidCP.Portal.UserControls.OrgIdPolicyEditor" %>
<asp:UpdatePanel runat="server" ID="OrgIdPolicyPanel" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:CheckBox id="enablePolicyCheckBox" runat="server" meta:resourcekey="enablePolicyCheckBox" Text="Enable Policy" CssClass="NormalBold" AutoPostBack="true" OnCheckedChanged="EnablePolicy_CheckedChanged"/>
        <table id="PolicyTable" runat="server" cellpadding="2">
            <tr>
                <td class="Normal" style="width:150px;">
                    <asp:Label ID="lblMaximumLength" runat="server" meta:resourcekey="lblMaximumLength" Text="Maximum OrgId length:"/>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtMaximumLength" runat="server" CssClass="form-control" Width="80px"/>
                    <asp:RequiredFieldValidator ID="valRequireMaxLength" runat="server" ControlToValidate="txtMaximumLength" meta:resourcekey="valRequireMaxLength" ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"/>
                    <asp:RegularExpressionValidator ID="valCorrectMaxLength" runat="server" ControlToValidate="txtMaximumLength" meta:resourcekey="valCorrectMaxLength" Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"/>
                </td>
            </tr>
        </table>
	</ContentTemplate>
</asp:UpdatePanel>