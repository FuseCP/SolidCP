<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailAddress.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.EmailAddress" %>
<%@ Register Src="DomainSelector.ascx" TagName="DomainSelector" TagPrefix="scp" %>
<div class="input-group">
    <span class="input-group-addon"><i class="fa fa-envelope-o" aria-hidden="true"></i></span>
    <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control" MaxLength="64" onkeyup="this.value = this.value.toLowerCase();" Style="text-transform: lowercase;"></asp:TextBox>
    <scp:DomainSelector id="domain" runat="server"></scp:DomainSelector>
</div>
<asp:RequiredFieldValidator ID="valRequireAccount" runat="server" meta:resourcekey="valRequireAccount" ControlToValidate="txtAccount" ErrorMessage="Enter E-mail address" ValidationGroup="CreateMailbox" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="valRequireCorrectEmail" runat="server" ErrorMessage="Enter valid e-mail address" ControlToValidate="txtAccount" Display="Dynamic" meta:resourcekey="valRequireCorrectEmail" ValidationExpression="^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+$" SetFocusOnError="True"></asp:RegularExpressionValidator>