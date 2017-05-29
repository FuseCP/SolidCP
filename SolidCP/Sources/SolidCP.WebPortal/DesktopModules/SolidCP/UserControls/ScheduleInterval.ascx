<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleInterval.ascx.cs" Inherits="SolidCP.Portal.ScheduleInterval" %>
<div class="form-inline">
    <asp:TextBox ID="txtInterval" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
    <asp:DropDownList ID="ddlUnits" runat="server" resourcekey="ddlUnits"  CssClass="form-control">
        <asp:ListItem Value="Days">Days</asp:ListItem>
        <asp:ListItem Value="Hours">Hours</asp:ListItem>
        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
        <asp:ListItem Value="Seconds">Seconds</asp:ListItem>
    </asp:DropDownList>
    <asp:RequiredFieldValidator id="valRequireInterval" runat="server" ErrorMessage="*" ControlToValidate="txtInterval" Display="Dynamic"></asp:RequiredFieldValidator>
</div>