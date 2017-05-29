<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BackupDatabase.ascx.cs" Inherits="SolidCP.Portal.UserControls.ScheduleTaskView.BackupDatabase" %>

	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblDatabaseType" runat="server" meta:resourcekey="lblDatabaseType" Text="Database Type:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:DropDownList ID="ddlDatabaseType" runat="server"   CssClass="NormalDropDown"></asp:DropDownList>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName" Text="Database Name:"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtDatabaseName" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox>
        </tr>
        <tr>
            <td class="SubHead" nowrap valign="top">
				<asp:Label ID="lblBackupFolder" runat="server" meta:resourcekey="lblBackupFolder">Backup Fodler: </asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtBackupFolder" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox><br />
   				<asp:Label ID="lblBackupFolderHint" runat="server" meta:resourcekey="lblBackupFolderHint" Text="([date], [time] variables are supported)"></asp:Label>
        </tr>
        <tr>
            <td class="SubHead" nowrap valign="top">
				<asp:Label ID="lblBackupName" runat="server" meta:resourcekey="lblBackupName">Backup File Name: </asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtBackupName" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox><br />
   				<asp:Label ID="lblBackupNameHint" runat="server" meta:resourcekey="lblBackupNameHint" Text="([date], [time] variables are supported)"></asp:Label>
        </tr>
        <tr>
            <td class="SubHead" nowrap>
				<asp:Label ID="lblZipBackup" runat="server" meta:resourcekey="lblZipBackup" Text="Zip Backup?"></asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:DropDownList ID="ddlZipBackup" runat="server"  CssClass="NormalDropDown"></asp:DropDownList>
        </tr>
	</table>