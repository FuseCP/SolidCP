<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail100x_EditDomain_Throttling.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail100x_EditDomain_Throttling" %>
<table>
    <tr>
        <td align="right" width = "200"><asp:Label ID="lbMessagesPerHour" runat="server" meta:resourcekey="lbMessagesPerHour" /></td>
        <td width = "100">
                <asp:TextBox runat="server"  ID="txtMessagesPerHour" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" runat="server" ID="valMessagesPerHour" ControlToValidate="txtMessagesPerHour"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValMessagesPerHour" runat="server" ControlToValidate="txtMessagesPerHour"
                    Display="Dynamic" />
         </td>
    </tr>
    <tr>
        <td align="right" width = "200"><asp:Label ID="lblmessagesAction" meta:resourcekey="lblmessagesAction" Text="Message Throttling Action:" runat="server" /></td>
        <td>
            <asp:DropDownList ID="ddlMessagesAction" runat="server">
                <asp:ListItem Value="0" meta:resourcekey="ddlMessagesAction0">Reject</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ddlMessagesAction1">Delay</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ddlMessagesAction2">None</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" width = "200"><asp:Label ID="lbBandwidthPerHour" runat="server" meta:resourcekey="lbBandwidthPerHour" /></td>
        <td width = "100">
                <asp:TextBox runat="server"  ID="txtBandwidthPerHour" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" runat="server" ID="valBandwidthPerHour" ControlToValidate="txtBandwidthPerHour"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValBandwidth" runat="server" ControlToValidate="txtBandwidthPerHour"
                    Display="Dynamic" />
         </td>
    </tr>
    <tr>
        <td align="right" width = "200"><asp:Label ID="lblBandwidthAction" meta:resourcekey="lblBandwidthAction" Text="Bandwidth Throttling Action:" runat="server" /></td>
        <td>
            <asp:DropDownList ID="ddlBandwidthAction" runat="server">
                <asp:ListItem Value="0" meta:resourcekey="ddlBandwidthAction0">Reject</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ddlBandwidthAction1">Delay</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ddlBandwidthAction2">None</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" width = "200"><asp:Label ID="lbBouncesPerHour" runat="server" meta:resourcekey="lbBouncesPerHour" /></td>
        <td width = "100">
                <asp:TextBox runat="server"  ID="txtBouncesPerHour" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" runat="server" ID="valBouncesPerHour" ControlToValidate="txtBouncesPerHour"
                    MinimumValue="0" Display="None" />
                <asp:RequiredFieldValidator ID="reqValBouncesPerHour" runat="server" ControlToValidate="txtBouncesPerHour"
                    Display="None" />
         </td>
    </tr>
    <tr>
        <td align="right" width = "200"><asp:Label ID="lblBouncesPerHourAction" meta:resourcekey="lblBouncesPerHourAction" Text="Bounces Throttling Action:" runat="server" /></td>
        <td>
            <asp:DropDownList ID="ddlBouncesPerHourAction" runat="server">
                <asp:ListItem Value="0" meta:resourcekey="ddlBouncesPerHourAction0">Reject</asp:ListItem>
                <asp:ListItem Value="1" meta:resourcekey="ddlBouncesPerHourAction1">Delay</asp:ListItem>
                <asp:ListItem Value="2" meta:resourcekey="ddlBouncesPerHourAction2">None</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>