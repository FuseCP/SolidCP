<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsMsSqlPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsMsSqlPolicy" %>
<%@ Register Src="UserControls/UsernamePolicyEditor.ascx" TagName="UsernamePolicyEditor" TagPrefix="uc2" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>


<scp:CollapsiblePanel id="secDatabase" runat="server"
    TargetControlID="DatabasePanel" meta:resourcekey="secDatabase" Text="Database Policy"/>
<asp:Panel ID="DatabasePanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName" Text="Database Name:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="databaseNamePolicy" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
    </table>
</asp:Panel>


<scp:CollapsiblePanel id="secUsername" runat="server"
    TargetControlID="UsernamePanel" meta:resourcekey="secUsername" Text="Username Policy"/>
<asp:Panel ID="UsernamePanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblUsername" runat="server" meta:resourcekey="lblUsername" Text="Database User:"></asp:Label>
            </td>
            <td class="Normal">
                <uc2:UsernamePolicyEditor id="userNamePolicy" runat="server">
                </uc2:UsernamePolicyEditor></td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Database Password:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:PasswordPolicyEditor id="userPasswordPolicy" runat="server">
                </uc1:PasswordPolicyEditor></td>
        </tr>
    </table>
</asp:Panel>