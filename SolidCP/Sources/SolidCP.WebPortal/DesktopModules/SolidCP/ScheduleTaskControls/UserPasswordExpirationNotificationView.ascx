<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserPasswordExpirationNotificationView.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.UserPasswordExpirationNotificationView" %>


<table cellspacing="0" cellpadding="4" width="100%">
   <tr>
        <td class="SubHead" nowrap>
            <asp:Label ID="lblDayBeforeNotify" runat="server" meta:resourcekey="lblDayBeforeNotify" Text="Notify before (days):"></asp:Label>
        </td>
        <td class="Normal" width="100%">
            <asp:TextBox ID="txtDaysBeforeNotify" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </td>
    </tr>
</table>
