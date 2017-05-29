<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsOperatingSystemPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsOperatingSystemPolicy" %>
<%@ Register Src="UserControls/UsernamePolicyEditor.ascx" TagName="UsernamePolicyEditor" TagPrefix="uc2" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secOdbc" runat="server"
    TargetControlID="OdbcPanel" meta:resourcekey="secOdbc" Text="ODBC DSN Policy"/>
<asp:Panel ID="OdbcPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblDsnName" runat="server" meta:resourcekey="lblDsnName" Text="ODBC DSN Name:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="dsnNamePolicy" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
    </table>
</asp:Panel>