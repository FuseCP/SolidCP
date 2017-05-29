<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerPasswordControl.ascx.cs" Inherits="SolidCP.Portal.ServerPasswordControl" %>
<div class="form-group">
    <asp:TextBox ID="txtPassword" runat="server" CssClass="hideContentOnBlur form-control" type="password" TextMode="Password" MaxLength="50"></asp:TextBox>
    <script type="text/javascript">$(".hideContentOnBlur").blur(function () { this.type = 'password'; }); $(".hideContentOnBlur").focus(function () { this.type = 'text'; });</script>
    <asp:RequiredFieldValidator ID="valRequirePassword" runat="server" meta:resourcekey="valRequirePassword" ErrorMessage="*" ControlToValidate="txtPassword" SetFocusOnError="True"></asp:RequiredFieldValidator>
</div>