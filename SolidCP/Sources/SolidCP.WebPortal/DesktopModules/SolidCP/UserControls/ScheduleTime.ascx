<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleTime.ascx.cs" Inherits="SolidCP.Portal.ScheduleTime" %>

<asp:TextBox ID="txtHours" runat="server" CssClass="form-control" Width="100px">12</asp:TextBox>
:
<asp:TextBox ID="txtMinutes" runat="server" CssClass="form-control" Width="100px">00</asp:TextBox>

<asp:RequiredFieldValidator ID="valRequireHours" runat="server" ErrorMessage="*" ControlToValidate="txtHours" Display="Dynamic"></asp:RequiredFieldValidator>
<asp:RequiredFieldValidator ID="valRequireMinutes" runat="server" ErrorMessage="*" ControlToValidate="txtMinutes" Display="Dynamic"></asp:RequiredFieldValidator>
<asp:RangeValidator ID="valCorrectHours" runat="server" ErrorMessage="Incorrect hours value" ControlToValidate="txtHours" Display="Dynamic" MaximumValue="23" MinimumValue="0"></asp:RangeValidator>
<asp:RangeValidator ID="valCorrectMinutes" runat="server" ErrorMessage="Incorrect minutes value" ControlToValidate="txtMinutes" Display="Dynamic" MaximumValue="59" MinimumValue="0"></asp:RangeValidator>
