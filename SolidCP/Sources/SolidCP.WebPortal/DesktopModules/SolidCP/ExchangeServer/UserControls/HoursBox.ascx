<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HoursBox.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.HoursBox" %>
<div class="form-inline">
<asp:TextBox ID="txtValue" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
<asp:Localize ID="locHours" runat="server" meta:resourcekey="locHours" Text="Hours"></asp:Localize>
<asp:RangeValidator  ErrorMessage="*"  ID="valRangeHours" runat="server"  Display="Dynamic"
Type="Integer" MinimumValue="0" MaximumValue="596523" meta:resourcekey="valRangeHours" ControlToValidate="txtValue" SetFocusOnError="true" />
</div>
