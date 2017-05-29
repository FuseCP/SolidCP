<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsSolidCPPolicy.ascx.cs" Inherits="SolidCP.Portal.SettingsSolidCPPolicy" %>
<%@ Register Src="UserControls/UsernamePolicyEditor.ascx" TagName="UsernamePolicyEditor" TagPrefix="uc2" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secPanelSettings" runat="server"
    TargetControlID="PanelSettingsPanel" meta:resourcekey="secPanelSettings" Text="SolidCP Settings">
</scp:CollapsiblePanel>
<asp:Panel ID="PanelSettingsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table class="SettingsPanel" width="100%">
        <tr>
            <td class="SubHead" width="150">
                <asp:Label ID="lblLogoImage" runat="server" meta:resourcekey="lblLogoImage" Text="Logo Image:"></asp:Label>
            </td>
            <td class="Normal" width="100%">
                <asp:TextBox ID="txtLogoImageURL" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblDemoMessage" runat="server" meta:resourcekey="lblDemoMessage" Text="Demo Message:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtDemoMessage" runat="server" Rows="5" TextMode="MultiLine" Width="100%" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Account Password:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:PasswordPolicyEditor id="passwordPolicy" runat="server">
                </uc1:PasswordPolicyEditor>
            </td>
        </tr>
    </table>
</asp:Panel>