<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailControl.ascx.cs" Inherits="SolidCP.Portal.UserControls.EmailControl" %>
<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
<asp:RequiredFieldValidator ID="valRequireEmail" runat="server" ErrorMessage="*" meta:resourcekey="valRequireEmail"
    ControlToValidate="txtEmail" Display="Dynamic"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="valCorrectEmail" runat="server"
    ErrorMessage="Wrong e-mail" ControlToValidate="txtEmail" Display="Dynamic" meta:resourcekey="valCorrectEmail"
    ValidationExpression="^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,25}$"></asp:RegularExpressionValidator>
