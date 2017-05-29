<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditIPAddressControl.ascx.cs" Inherits="SolidCP.Portal.UserControls.EditIPAddressControl" %>
<asp:TextBox ID="txtAddress" runat="server" Width="260px" MaxLength="45" CssClass="form-control"></asp:TextBox>
<asp:RequiredFieldValidator ID="requireAddressValidator" runat="server" meta:resourcekey="requireAddressValidator"
    ControlToValidate="txtAddress" SetFocusOnError="true" Text="*" Enabled="false" Display="Dynamic" />
<asp:CustomValidator ID="addressValidator" runat="server" ControlToValidate="txtAddress" OnServerValidate="Validate" Text="*" meta:resourcekey="addressValidator"/>