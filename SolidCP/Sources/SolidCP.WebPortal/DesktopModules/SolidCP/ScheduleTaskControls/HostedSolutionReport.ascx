<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostedSolutionReport.ascx.cs"
    Inherits="SolidCP.Portal.ScheduleTaskControls.HostedSolutionReport" %>
<table width="100%">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblMail" meta:resourcekey="lblMail" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtMail" meta:resourcekey="txtMail" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="cbExchange" meta:resourcekey="cbExchange" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="cbSharePoint" meta:resourcekey="cbSharePoint" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="cbLync" meta:resourcekey="cbLync" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="cbSfB" meta:resourcekey="cbSfB" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="cbCRM" meta:resourcekey="cbCRM" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="cbOrganization" meta:resourcekey="cbOrganization" />
        </td>
    </tr>
</table>
