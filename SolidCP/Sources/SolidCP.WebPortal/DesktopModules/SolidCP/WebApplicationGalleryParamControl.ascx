<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGalleryParamControl.ascx.cs" Inherits="SolidCP.Portal.WebApplicationGalleryParamControl" %>
<fieldset>
    <%-- Friendly name --%>
    <legend>
        <asp:Literal ID="friendlyName" runat="server"></asp:Literal>
    </legend>

    <%-- Description --%>
    <div class="FormFieldDescription">
        <asp:Literal ID="description" runat="server"></asp:Literal>
    </div>


    <%-- Password --%>
    <div id="PasswordControl" runat="server" class="FormField">
        <asp:TextBox ID="password" runat="server" TextMode="Password" Width="600px"></asp:TextBox>
        <div>
            <asp:RequiredFieldValidator ID="requirePassword" runat="server" 
                ControlToValidate="password" Text="*" ValidationGroup="wag" 
                Display="Dynamic" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regexpPassword" runat="server" 
                ControlToValidate="password" Text="!" ValidationGroup="wag" 
                Display="Dynamic" SetFocusOnError="True"></asp:RegularExpressionValidator>
        </div>

        <div id="confirmPasswordControls" runat="server">
            <asp:Localize ID="locConfirm" meta:resourcekey="locConfirm" runat="server" Text="Confirm:"></asp:Localize><br />

            <asp:TextBox ID="confirmPassword" runat="server" TextMode="Password" Width="600px"></asp:TextBox>
            <div>
                <asp:RequiredFieldValidator ID="requireConfirmPassword" runat="server" meta:resourcekey="requireConfirmPassword"
                    ControlToValidate="confirmPassword" Text="*" ValidationGroup="wag" 
                    Display="Dynamic" SetFocusOnError="True"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="comparePasswords" runat="server" meta:resourcekey="comparePasswords"
                    ErrorMessage="CompareValidator" Text="!" ControlToCompare="password" 
                    ControlToValidate="confirmPassword" Display="Dynamic"
                    ValidationGroup="wag"></asp:CompareValidator>
            </div>
        </div>
    </div>


    <%-- Text value --%>
    <div id="TextControl" runat="server" class="FormField">
        <asp:Literal ID="valPrefix" runat="server" Text=""></asp:Literal>
        <asp:TextBox ID="textValue" runat="server" Width="600px"></asp:TextBox>
        <asp:Literal ID="valSuffix" runat="server" Text=""></asp:Literal>
        <div>
            <asp:RequiredFieldValidator ID="requireTextValue" runat="server" 
                ControlToValidate="textValue" ValidationGroup="wag" 
                Display="Dynamic" SetFocusOnError="True" Text="This parameter cannot be empty"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regexpTextValue" runat="server"
                ControlToValidate="textValue" Text="!" ValidationGroup="wag" 
                Display="Dynamic" SetFocusOnError="True"></asp:RegularExpressionValidator>
            <asp:CustomValidator runat="server" ID="MysqlUsernameLengthValidator"
                ControlToValidate="textValue" OnServerValidate="mysqlUsernameLen_OnServerValidate"
                Display="Dynamic" SetFocusOnError="True" ValidationGroup="wag" Enabled="False"
                Text="Mysql username can not be longer than 16 characters"></asp:CustomValidator>
            <asp:CustomValidator runat="server" ID="MariaDBUsernameLengthValidator"
                ControlToValidate="textValue" OnServerValidate="mariadbUsernameLen_OnServerValidate"
                Display="Dynamic" SetFocusOnError="True" ValidationGroup="wag" Enabled="False"
                Text="MariaDB username can not be longer than 16 characters"></asp:CustomValidator>
        </div>
    </div>


    <%-- Boolean value --%>
    <div id="BooleanControl" runat="server" class="FormField">
        <asp:CheckBox ID="boolValue" meta:resourcekey="boolValue" runat="server" Text="Yes" />
    </div>


    <%-- Enumeration value --%>
    <div id="EnumControl" runat="server" class="FormField">
        <asp:DropDownList ID="enumValue" runat="server" Width="600px"></asp:DropDownList>
        <div>
            <asp:RequiredFieldValidator ID="requireEnumValue" runat="server" 
                ControlToValidate="enumValue" Text="*" ValidationGroup="wag" 
                Display="Dynamic" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regexpEnumValue" runat="server" 
                ControlToValidate="enumValue" Text="!" ValidationGroup="wag" 
                Display="Dynamic" SetFocusOnError="True"></asp:RegularExpressionValidator>
        </div>
    </div>
</fieldset>