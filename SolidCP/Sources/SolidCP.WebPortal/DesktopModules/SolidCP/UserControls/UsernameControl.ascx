<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsernameControl.ascx.cs" Inherits="SolidCP.Portal.UsernameControl" %>
<div class="input-group col-sm-12">
    <asp:Label ID="litPrefix" runat="server" CssClass="input-group-addon"></asp:Label>
    <asp:textbox id="txtName" runat="server" CssClass="form-control" MaxLength="100" style="text-transform: lowercase;" onkeyup="this.value = this.value.toLowerCase();" ></asp:textbox>
    <asp:textbox ID="lblName" runat="server" style="line-height: 22px;" CssClass="form-control" ReadOnly="true"></asp:textbox>
    <asp:Label ID="litSuffix" runat="server" CssClass="input-group-addon"></asp:Label>
</div>
<asp:RequiredFieldValidator id="valRequireUsername" runat="server" CssClass="input-form-addon" ErrorMessage="*" ControlToValidate="txtName" Display="Dynamic"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator id="valCorrectUsername" runat="server" Display="Dynamic" ControlToValidate="txtName" ErrorMessage="Only letters, numbers and '_', '-', '.' symbols are allowed" ValidationExpression="^[a-zA-Z0-9_\.\-]{1,20}$" CssClass="NormalBold"></asp:RegularExpressionValidator>
<asp:RegularExpressionValidator id="valCorrectMinLength" runat="server" Display="Dynamic" ControlToValidate="txtName" ErrorMessage="Min length" ValidationExpression="^.{3,}$" CssClass="NormalBold"></asp:RegularExpressionValidator>