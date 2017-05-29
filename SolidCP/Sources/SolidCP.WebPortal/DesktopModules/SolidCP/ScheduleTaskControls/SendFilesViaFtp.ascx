<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendFilesViaFtp.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.SendFilesViaFtp" %>
	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap valign="top">
				<asp:Label ID="lblFilePath" runat="server" meta:resourcekey="lblFilePath">Space File: </asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtFilePath" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox><br />
   				<asp:Label ID="lblFilePathHint" runat="server" meta:resourcekey="lblFilePathHint" Text="([date], [time] variables are supported)"></asp:Label>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblServer" runat="server" meta:resourcekey="lblServer" Text="FTP Server:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtServer" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="FTP Username:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtUserName" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="FTP Password:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtPassword" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblRemoteFolder" runat="server" meta:resourcekey="lblRemoteFolder" Text="FTP Remote Folder:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtRemoteFolder" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
	</table>