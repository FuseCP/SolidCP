<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditVLANControl.ascx.cs" Inherits="SolidCP.Portal.UserControls.EditVLANControl" %>
<asp:TextBox ID="txtVLAN" runat="server" Width="260px" MaxLength="45" CssClass="form-control"></asp:TextBox>
<asp:RequiredFieldValidator ID="requireVLANValidator" runat="server" meta:resourcekey="requireVLANValidator"
    ControlToValidate="txtVLAN" SetFocusOnError="true" Text="*" Enabled="false" Display="Dynamic" />
<asp:CustomValidator ID="vlanValidator" runat="server" ControlToValidate="txtVLAN" OnServerValidate="Validate" Text="*" meta:resourcekey="vlanValidator"/>