<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsernamePolicyEditor.ascx.cs" Inherits="SolidCP.Portal.UsernamePolicyEditor" %>

<asp:UpdatePanel runat="server" ID="UsernamePolicyPanel" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate> 

<asp:CheckBox id="chkEnabled" runat="server" meta:resourcekey="chkEnabled"
	Text="Enable Policy" CssClass="NormalBold" AutoPostBack="true" OnCheckedChanged="chkEnabled_CheckedChanged" />
<table id="PolicyTable" runat="server" cellpadding="2">
    <tr>
        <td class="Normal" valign="top" style="width:150px;"><asp:Label ID="lblAllowedSymbols" runat="server"
            meta:resourcekey="lblAllowedSymbols" Text="Allowed symbols:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtAllowedSymbols" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
            <br />
            <asp:Label ID="lblDefaultAllowedSymbols" runat="server"
                meta:resourcekey="lblDefaultAllowedSymbols" Text="Allowed symbols:"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="Normal"><asp:Label ID="lblMinimumLength" runat="server"
            meta:resourcekey="lblMinimumLength" Text="Minimum length:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtMinimumLength" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireMinLength" runat="server" ControlToValidate="txtMinimumLength" meta:resourcekey="valRequireMinLength"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectMinLength" runat="server" ControlToValidate="txtMinimumLength" meta:resourcekey="valCorrectMinLength"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="Normal"><asp:Label ID="lblMaximumLength" runat="server"
            meta:resourcekey="lblMaximumLength" Text="Maximum length:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtMaximumLength" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireMaxLength" runat="server" ControlToValidate="txtMaximumLength" meta:resourcekey="valRequireMaxLength"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectMaxLength" runat="server" ControlToValidate="txtMaximumLength" meta:resourcekey="valCorrectMaxLength"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="Normal" valign="top">
            <asp:Label ID="lblPrefix" runat="server"
                meta:resourcekey="lblPrefix" Text="Prefix:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:TextBox ID="txtPrefix" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="Normal" valign="top">
            <asp:Label ID="lblSuffix" runat="server"
                meta:resourcekey="lblSuffix" Text="Suffix:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:TextBox ID="txtSuffix" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
            <br />
            <asp:Label ID="lblSuffixVars" runat="server" meta:resourcekey="lblSuffixVars" Text="* [USER_NAME], [USER_ID] variables are allowed"></asp:Label>
        </td>
    </tr>
</table>

	</ContentTemplate>
</asp:UpdatePanel>