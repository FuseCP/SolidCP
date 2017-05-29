<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsVpsPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsVpsPolicy" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secAdministratorPassword" runat="server"
    TargetControlID="AdministratorPasswordPanel" meta:resourcekey="secAdministratorPassword" Text="Administrator Account Password Policy"/>
<asp:Panel ID="AdministratorPasswordPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" nowrap>
                <asp:Label ID="lblPasswordPolicy" runat="server" meta:resourcekey="lblPasswordPolicy" Text="Password Policy:"></asp:Label>
            </td>
            <td>
                <scp:PasswordPolicyEditor id="adminPasswordPolicy" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel>