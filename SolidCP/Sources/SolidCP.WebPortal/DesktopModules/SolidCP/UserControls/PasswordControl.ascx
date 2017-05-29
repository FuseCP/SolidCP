<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordControl.ascx.cs" Inherits="SolidCP.Portal.PasswordControl" %>
<script src="<%= GetRandomPasswordUrl() %>" language="javascript" type="text/javascript"></script>

<div class="form-group">
    <label for="txtPassword" class="col-sm-2 control-label">
        <asp:Localize ID="locPassword" runat="server" meta:resourcekey="locPassword" Text="Password:" />
    </label>
    <div class="col-sm-8">
        <div class="input-group">
            <!-- Used to stop browsers auto-completing the username box --><input style="display:none" type="text" name="fakeusernameremembered" />
            <!-- Used to stop browsers auto-completing the password box --><input style="display:none" type="password" name="fakepasswordremembered" />
            <span class="input-group-addon"><i class="fa fa-lock fa-lg" aria-hidden="true"></i></span>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="hideContentOnBlur form-control" type="password" TextMode="Password" MaxLength="50" meta:resourcekey="loctxtPassword" placeholder="Enter your password"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="valRequirePassword" runat="server" meta:resourcekey="valRequirePassword" ErrorMessage="*" ControlToValidate="txtPassword" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group">
    <label for="txtConfirmPassword" class="col-sm-2 control-label">
        <asp:Localize ID="locConfirmPassword" runat="server" meta:resourcekey="locConfirmPassword" Text="Confirm Password:" />
    </label>
    <div class="col-sm-8">
        <div class="input-group">
            <span class="input-group-addon">
                <i class="fa fa-lock fa-lg" aria-hidden="true"></i>
            </span>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="hideContentOnBlur form-control" type="password" TextMode="Password" MaxLength="50" meta:resourcekey="loctxtConfirmPassword" placeholder="Confirm your password"></asp:TextBox>
            <script type="text/javascript">$(".hideContentOnBlur").blur(function () { this.type = 'password'; }); $(".hideContentOnBlur").focus(function () { this.type = 'text'; });</script>
        </div>
        <asp:RequiredFieldValidator ID="valRequireConfirmPassword" runat="server" meta:resourcekey="valRequireConfirmPassword" ErrorMessage="*" ControlToValidate="txtConfirmPassword" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group">
    <div class="col-sm-10 col-sm-offset-2">
        <ajaxToolkit:PasswordStrength ID="PS" runat="server" TargetControlID="txtPassword" DisplayPosition="RightSide" StrengthIndicatorType="Text"
            PreferredPasswordLength="10" PrefixText="Strength:" TextCssClass="TextIndicator_TextBox1" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
            RequiresUpperAndLowerCaseCharacters="true" TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
            TextStrengthDescriptionStyles="alert alert-danger;alert alert-warning;alert alert-default;alert alert-success;alert alert-success" CalculationWeightings="50;15;15;20" />
    </div>
    <div class="col-sm-2 col-sm-offset-2">
        <asp:HyperLink ID="lnkGenerate" runat="server" NavigateUrl="#" meta:resourcekey="lnkGenerate" CssClass="btn btn-primary btn-sm" Visible="true">
            Generate Random Password
        </asp:HyperLink>
    </div>
    <div class="col-sm-8">
        <asp:CompareValidator ID="valRequireEqualPassword" CssClass="alert alet-warning" runat="server" ControlToCompare="txtPassword" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtConfirmPassword" meta:resourcekey="valRequireEqualPassword"></asp:CompareValidator>
        <asp:CustomValidator ID="valCorrectLength" CssClass="alert alet-warning" runat="server" ControlToValidate="txtPassword" ErrorMessage="len" Display="Dynamic" Enabled="false" ClientValidationFunction="scpValidatePasswordLength" OnServerValidate="valCorrectLength_ServerValidate"></asp:CustomValidator>
        <asp:CustomValidator ID="valRequireNumbers" CssClass="alert alet-warning" runat="server" ControlToValidate="txtPassword" ErrorMessage="num" Display="Dynamic" Enabled="false" ClientValidationFunction="scpValidatePasswordNumbers" OnServerValidate="valRequireNumbers_ServerValidate"></asp:CustomValidator>
        <asp:CustomValidator ID="valRequireUppercase" CssClass="alert alet-warning" runat="server" ControlToValidate="txtPassword" ErrorMessage="upp" Display="Dynamic" Enabled="false" ClientValidationFunction="scpValidatePasswordUppercase" OnServerValidate="valRequireUppercase_ServerValidate"></asp:CustomValidator>
        <asp:CustomValidator ID="valRequireSymbols" CssClass="alert alet-warning" runat="server" ControlToValidate="txtPassword" ErrorMessage="sym" Display="Dynamic" Enabled="false" ClientValidationFunction="scpValidatePasswordSymbols" OnServerValidate="valRequireSymbols_ServerValidate"></asp:CustomValidator>
    </div>
</div>

<% if (ValidationEnabled)
    {%>
<div style="display: none;" id="password-hint-popup">
    <h3 class="popover-title">
        Password must meet the following requirements:
    </h3>
    <ul class="popover-content">
        <li><%= string.Format("Password should be at least {0} characters", MinimumLength) %></li>
        <li><%= string.Format("Password should be maximum {0} characters", MaximumLength) %></li>

        <% if (MinimumUppercase > 0)
            {%>
        <li><%= string.Format("Password should contain at least {0} UPPERCASE characters", MinimumUppercase) %></li>
        <% }%>
        <% if (MinimumNumbers > 0)
            {%>
        <li><%= string.Format("Password should contain at least {0} numbers", MinimumNumbers) %></li>
        <% }%>
        <% if (MinimumSymbols > 0)
            {%>
        <li><%= string.Format("Password should contain at least {0} non-alphanumeric symbols", MinimumSymbols) %></li>
        <% }%>
    </ul>
    <pre class="sh_javascript">$('#password-hint-popup').poshytip({
    className: 'tip-bluesimple',
    showOn: 'focus',
            alignTo: 'target',
            alignX: 'center',
            alignY: 'bottom',
            offsetX: 2,
    showTimeout: 100
});</pre>
</div>
<% }%>