<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail60_EditDomain_Throttling.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail60_EditDomain_Throttling" %>
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
         <td align="left" width = "10">
            <asp:CheckBox runat="server" ID="cbMessagesPerHour"  />
        </td>
        <td width = "50" align="left"><asp:Label ID="lbMessagesPerHourEnabled" runat="server" meta:resourcekey="lbEnabled" /></td>
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
         <td align="left" width = "10">
            <asp:CheckBox runat="server" ID="cbBandwidthPerHour"/>
        </td>
        <td width = "50" align="left"><asp:Label ID="lbBandwidthPerHourEnabled" runat="server" meta:resourcekey="lbEnabled" /></td>
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
         <td align="left" width = "10">
            <asp:CheckBox runat="server" ID="cbBouncesPerHour" />
        </td>
        <td width = "50" align="left"><asp:Label ID="lbBouncesPerHourEnabled"  runat="server" meta:resourcekey="lbEnabled" /></td>
    </tr>
</table>