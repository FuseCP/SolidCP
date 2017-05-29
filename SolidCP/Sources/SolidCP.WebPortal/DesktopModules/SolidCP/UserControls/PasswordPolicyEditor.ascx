<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordPolicyEditor.ascx.cs" Inherits="SolidCP.Portal.PasswordPolicyEditor" %>

<asp:UpdatePanel runat="server" ID="PasswordPolicyPanel" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate> 

<asp:CheckBox id="chkEnabled" runat="server" meta:resourcekey="chkEnabled"
	Text="Enable Policy" CssClass="NormalBold" AutoPostBack="true" OnCheckedChanged="chkEnabled_CheckedChanged" />
<table id="PolicyTable" runat="server" cellpadding="2">
    <tr>
        <td class="Normal" style="width:150px;"><asp:Label ID="lblMinimumLength" runat="server"
            meta:resourcekey="lblMinimumLength" Text="Minimum length:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtMinimumLength" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireMinLength" runat="server" ControlToValidate="txtMinimumLength" meta:resourcekey="valRequireMinLength"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectMinLength" runat="server" ControlToValidate="txtMinimumLength" meta:resourcekey="valCorrectMinLength"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator></td>
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
        <td class="Normal" style="width:150px;"><asp:Label ID="lblEnforcePasswordHistory" runat="server"
            meta:resourcekey="lblEnforcePasswordHistory" Text="Enforce Password History:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtEnforcePasswordHistory" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireEnforcePasswordHistory" runat="server" ControlToValidate="txtEnforcePasswordHistory" meta:resourcekey="valRequireEnforcePasswordHistory"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectEnforcePasswordHistory" runat="server" ControlToValidate="txtEnforcePasswordHistory" meta:resourcekey="valCorrectEnforcePasswordHistory"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <td class="Normal" style="width:150px;"><asp:Label ID="lblMaxPasswordAge" runat="server"
            meta:resourcekey="lblMaxPasswordAge" Text="Max Password Age (days):"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtMaxPasswordAge" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireMaxPasswordAge" runat="server" ControlToValidate="txtMaxPasswordAge" meta:resourcekey="valRequireMaxPasswordAge"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectMaxPasswordAge" runat="server" ControlToValidate="txtMaxPasswordAge" meta:resourcekey="valCorrectMaxPasswordAge"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator></td>
    </tr>
    <tr id="RowChkLockOutSettigns" runat="server">
        <td colspan="2" class="NormalBold">
            <asp:CheckBox id="chkLockOutSettigns" runat="server" meta:resourcekey="chkLockOutSettigns" 
            Text="Enable Lockout Settings" CssClass="NormalBold" AutoPostBack="true" OnCheckedChanged="chkLockOutSettigns_CheckedChanged" />
        </td>
    </tr>
    <tr id="RowAccountLockoutDuration" runat="server">
        <td class="Normal" style="width:150px;"><asp:Label ID="lblAccountLockoutDuration" runat="server"
            meta:resourcekey="lblAccountLockoutDuration" Text="Account Lockout Duration (minutes):"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtAccountLockoutDuration" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireAccountLockoutDuration" runat="server" ControlToValidate="txtAccountLockoutDuration" meta:resourcekey="valRequireAccountLockoutDuration"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectAccountLockoutDuration" runat="server" ControlToValidate="txtAccountLockoutDuration" meta:resourcekey="valCorrectAccountLockoutDuration"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator></td>
    </tr>
    <tr id="RowLockedOut" runat="server">
        <td class="Normal">
            <asp:Label ID="lblLockedOut" runat="server"
                meta:resourcekey="lblLockedOut" Text="Account Lockout threshold:"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtLockedOut" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RegularExpressionValidator ID="valCorrectLockedOut" runat="server" ControlToValidate="txtLockedOut" meta:resourcekey="valCorrectLockedOut"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,10}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr id="RowResetAccountLockout" runat="server">
        <td class="Normal">
            <asp:Label ID="lblResetAccountLockout" runat="server"
                meta:resourcekey="lblResetAccountLockout" Text="Reset account lockout counter after (minutes):"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtResetAccountLockout" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RegularExpressionValidator ID="valResetAccountLockout" runat="server" ControlToValidate="txtResetAccountLockout" meta:resourcekey="valResetAccountLockout"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,10}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
        </td>
    </tr>

    <tr>
        <td colspan="2" class="NormalBold">
            <asp:CheckBox id="chkPasswordComplexity" runat="server" meta:resourcekey="chkPasswordComplexity" 
            Text="Enable Password Complexity" CssClass="NormalBold" AutoPostBack="true" OnCheckedChanged="chkPasswordComplexity_CheckedChanged" />
        </td>
    </tr>
    <tr id="RowMinimumUppercase" runat="server">
        <td class="Normal">
            <asp:Label ID="lblMinimumUppercase" runat="server"
                meta:resourcekey="lblMinimumUppercase" Text="Uppercase letters:"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtMinimumUppercase" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireUppercase" runat="server" ControlToValidate="txtMinimumUppercase" meta:resourcekey="valRequireUppercase"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectUppercase" runat="server" ControlToValidate="txtMinimumUppercase" meta:resourcekey="valCorrectUppercase"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
            </td>
    </tr>
    <tr id="RowMinimumNumbers" runat="server">
        <td class="Normal">
            <asp:Label ID="lblMinimumNumbers" runat="server"
                meta:resourcekey="lblMinimumNumbers" Text="Numbers:"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtMinimumNumbers" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireNumbers" runat="server" ControlToValidate="txtMinimumNumbers" meta:resourcekey="valRequireNumbers"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectNumbers" runat="server" ControlToValidate="txtMinimumNumbers" meta:resourcekey="valCorrectNumbers"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
            </td>
    </tr>
    <tr id="RowMinimumSymbols" runat="server">
        <td class="Normal">
            <asp:Label ID="lblMinimumSymbols" runat="server"
                meta:resourcekey="lblMinimumSymbols" Text="Non-alphanumeric symbols:"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtMinimumSymbols" runat="server" CssClass="form-control" Width="60px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireSymbols" runat="server" ControlToValidate="txtMinimumSymbols" meta:resourcekey="valRequireSymbols"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectSymbols" runat="server" ControlToValidate="txtMinimumSymbols" meta:resourcekey="valCorrectSymbols"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr id="rowEqualUsername" runat="server" visible="false">
        <td class="Normal" colspan="2">
            <asp:CheckBox id="chkNotEqualUsername" runat="server" meta:resourcekey="chkNotEqualUsername" Text="Should not be equal to username" />
        </td>
    </tr>
</table>

	</ContentTemplate>
</asp:UpdatePanel>