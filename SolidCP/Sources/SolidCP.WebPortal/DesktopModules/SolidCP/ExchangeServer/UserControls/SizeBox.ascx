<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SizeBox.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.SizeBox" %>
<div class="form-inline">
<asp:TextBox ID="txtValue" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
<asp:Localize ID="locKB" runat="server" meta:resourcekey="locKB" Text="KB"></asp:Localize>
<asp:Localize ID="locMB" runat="server" meta:resourcekey="locMB" Text="MB"></asp:Localize>
<asp:Localize ID="locPct" runat="server" meta:resourcekey="locPct" Text="%"></asp:Localize>

<ajaxtoolkit:ValidatorCalloutExtender ID="_ValidatorCalloutExtender" runat="server"
                            TargetControlID="valRequireNumber"
                             HighlightCssClass="validatorCalloutHighlight" Width="125px">
                       </ajaxtoolkit:ValidatorCalloutExtender>

<ajaxtoolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                            TargetControlID="valRequireCorrectNumber"
                             HighlightCssClass="validatorCalloutHighlight" Width="200px">
                       </ajaxtoolkit:ValidatorCalloutExtender>
</div>

<asp:RequiredFieldValidator ID="valRequireNumber" runat="server" meta:resourcekey="valRequireNumber" Enabled="false"
    ErrorMessage="Please enter value" ControlToValidate="txtValue" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="valRequireCorrectNumber" runat="server" meta:resourcekey="valRequireCorrectNumber"
	ErrorMessage="Enter correct number" ControlToValidate="txtValue" Display="None" SetFocusOnError="True"></asp:RegularExpressionValidator>
