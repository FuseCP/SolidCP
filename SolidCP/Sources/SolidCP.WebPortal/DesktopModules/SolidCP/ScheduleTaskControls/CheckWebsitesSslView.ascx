<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckWebsitesSslView.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.CheckWebsitesSslView" %>
<div>
    <br />
	<table>
        <tr>
            <td class="SubHead" width="20%">
				<asp:Label runat="server" meta:resourcekey="lblMailFrom" Text="Mail From:"></asp:Label>
			</td>
            <td class="Normal" width="70%">
   				<asp:TextBox ID="txtMailFrom" runat="server" ReadOnly="true" CssClass="form-control" width="50%" MaxLength="1000"></asp:TextBox>
            </td>
        </tr>

		<tr>
            <td class="SubHead" width="20%">
                <asp:CheckBox ID="cbMailToCustomer" runat="server" meta:resourcekey="cbMailToCustomer" Checked="true" Text="Send mail to customer account" />
            </td>
            <td></td>
        </tr>

        <tr>
			<td class="SubHead">
				<asp:CheckBox ID="cbSendBcc" runat="server" meta:resourcekey="cbSendBcc" Text="Send BCC to:" />
   			</td>
   			<td class="Normal">
   				<asp:TextBox ID="txtBccMail" runat="server" CssClass="form-control" width="50%" MaxLength="1000"></asp:TextBox>
   			</td>
		</tr>

        <tr>
            <td class="SubHead" style="white-space: nowrap;">
				<asp:Label runat="server" meta:resourcekey="lblExpirationMailSubject" Text="Expiration Mail Subject:"></asp:Label>
			</td>
            <td class="Normal" width="70%">
   				<asp:TextBox ID="txtExpirationMailSubject" runat="server" CssClass="form-control" width="50%" MaxLength="1000"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td class="SubHead" style="white-space: nowrap;">
				<asp:Label runat="server" meta:resourcekey="lblExpirationMailBody" Text="Expiration Mail Body:"></asp:Label>
			</td>
			<td class="Normal" width="70%">
				<asp:TextBox ID="txtExpirationMailBody" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" MaxLength="2000"></asp:TextBox>
				<asp:Label runat="server" meta:resourcekey="lblExpirationMailBodyHint"
                    Text="([domain], [url], [issuer], [expires_in_days], [expires_on_date] variables are supported)"></asp:Label>
			</td>
        </tr>

        <tr>
            <td class="SubHead" width="20%">
                <asp:CheckBox ID="cbSend30DaysBeforeExpiration" runat="server" meta:resourcekey="cbSend30DaysBeforeExpiration" Checked="true" Text="Send notification 30 days before expiration" />
            </td>
            <td></td>
        </tr>

        <tr>
            <td class="SubHead" width="20%">
                <asp:CheckBox ID="cbSend14DaysBeforeExpiration" runat="server" meta:resourcekey="cbSend14DaysBeforeExpiration" Checked="true" Text="Send 14 days before expiration" />
            </td>
            <td></td>
        </tr>

        <tr>
            <td class="SubHead" width="20%">
                <asp:CheckBox ID="cbSendTodayExpired" runat="server" meta:resourcekey="cbSendTodayExpired" Checked="true" Text="Send today expired SSL notification" />
            </td>
            <td></td>
        </tr>

        <tr>
            <td class="SubHead" width="20%">
                <asp:CheckBox ID="cbSendSslError" runat="server" meta:resourcekey="cbSendSslError" Checked="true" Text="Send SSL error notification" />
            </td>
            <td></td>
        </tr>

        <tr>
            <td class="SubHead" style="white-space: nowrap;">
				<asp:Label runat="server" meta:resourcekey="lblErrorMailSubject" Text="SSL Error Mail Subject:"></asp:Label>
			</td>
            <td class="Normal" width="70%">
   				<asp:TextBox ID="txtErrorMailSubject" runat="server" CssClass="form-control" width="50%" MaxLength="1000"></asp:TextBox>
            </td>
        </tr>

        <tr>
			<td class="SubHead" style="white-space: nowrap;">
				<asp:Label runat="server" meta:resourcekey="lblErrorMailBody" Text="SSL Error Mail Body:"></asp:Label>
			</td>
			<td class="Normal" width="70%">
				<asp:TextBox ID="txtErrorMailBody" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" MaxLength="2000"></asp:TextBox>
				<asp:Label runat="server" meta:resourcekey="lblErrorMailBodyHint" Text="([domain], [url], [error] variables are supported)"></asp:Label>
			</td>
        </tr>
	</table>
</div>