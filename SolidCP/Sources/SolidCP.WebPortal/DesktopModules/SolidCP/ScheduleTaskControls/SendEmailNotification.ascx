<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendEmailNotification.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.SendEmailNotification" %>
	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblMailFrom" runat="server" meta:resourcekey="lblMailFrom" Text="Mail From:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtMailFrom" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblMailTo" runat="server" meta:resourcekey="lblMailTo" Text="Mail To:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtMailTo" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblMailSubject" runat="server" meta:resourcekey="lblMailSubject" Text="Mail Subject:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtMailSubject" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
			<td colspan="2" class="SubHead" nowrap>
				<asp:Label ID="lblMailBody" runat="server" meta:resourcekey="lblMailBody" Text="Mail Body:"></asp:Label>
			</td>
        </tr>
        <tr>
			<td colspan="2">
				<asp:TextBox ID="txtMailBody" runat="server" Width="95%" CssClass="form-control" TextMode="MultiLine" Rows="10" MaxLength="1000"></asp:TextBox>
				<br />
				<asp:Label ID="lblMailBodyHint" runat="server" meta:resourcekey="lblMailBodyHint">
					([url], [message], [content] variables are supported for "Check Web Site Availability Task")
				</asp:Label>
			</td>
        </tr>
	</table>