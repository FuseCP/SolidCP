<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainExpirationView.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.DomainExpirationView" %>


<table cellspacing="0" cellpadding="4" width="100%">
    <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblEnableNotify" runat="server" meta:resourcekey="lblEnableNotify" Text="Enable Client Notification:"></asp:Label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnableNotify" /><br/>
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblIncludeNonExistenDomains" runat="server" meta:resourcekey="lblIncludeNonExistenDomains" Text="Include Non-Existen Domains:"></asp:Label>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="cbIncludeNonExistenDomains" /><br/>
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblMailTo" runat="server" meta:resourcekey="lblMailTo" Text="Mail To:"></asp:Label>
        </td>
        <td class="Normal" width="100%">
            <asp:TextBox ID="txtMailTo" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
         </td>
    </tr>

    <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblDayBeforeNotify" runat="server" meta:resourcekey="lblDayBeforeNotify" Text="Notify before (days):"></asp:Label>
        </td>
        <td class="Normal" width="100%">
            <asp:TextBox ID="txtDaysBeforeNotify" runat="server" Width="95%" CssClass="form-control" MaxLength="1000" placeholder="Number of days before expiration date"></asp:TextBox>
        </td>
    </tr>
</table>