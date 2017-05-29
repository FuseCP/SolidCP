<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsMailPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsMailPolicy" %>
<%@ Register Src="UserControls/UsernamePolicyEditor.ascx" TagName="UsernamePolicyEditor" TagPrefix="uc2" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secGeneral" runat="server"
    TargetControlID="GeneralPanel" meta:resourcekey="secGeneral" Text="General Settings"/>
<asp:Panel ID="GeneralPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtCatchAll" runat="server" CssClass="form-control" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireCatchAll" runat="server" ControlToValidate="txtCatchAll" meta:resourcekey="valRequireCatchAll"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="SettingsEditor"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</asp:Panel>

<scp:CollapsiblePanel id="secAccountPolicy" runat="server"
    TargetControlID="AccountPolicyPanel" meta:resourcekey="secAccountPolicy" Text="Mail Accounts Policy"/>
<asp:Panel ID="AccountPolicyPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblAccount" runat="server" meta:resourcekey="lblAccount" Text="Account Policy:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="accountNamePolicy" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password Policy:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:PasswordPolicyEditor id="accountPasswordPolicy" runat="server">
                </uc1:PasswordPolicyEditor></td>
        </tr>
    </table>
</asp:Panel>