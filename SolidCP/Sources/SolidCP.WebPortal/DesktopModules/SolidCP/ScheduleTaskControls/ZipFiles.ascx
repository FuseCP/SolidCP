<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ZipFiles.ascx.cs" Inherits="SolidCP.Portal.ScheduleTaskControls.ZipFiles" %>
	<table cellspacing="0" cellpadding="4" width="100%">
        <tr>
            <td class="SubHead" nowrap valign="top">
				<asp:Label ID="lblSpaceFolder" runat="server" meta:resourcekey="lblSpaceFolder">Space Folder:</asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtSpaceFolder" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox><br />
   				<asp:Label ID="lblSpaceFolderHint" runat="server" meta:resourcekey="lblSpaceFolderHint" Text="([date], [time] variables are supported)"></asp:Label>
        </tr>
        <tr>
            <td class="SubHead" nowrap valign="top">
				<asp:Label ID="lblZipFile" runat="server" meta:resourcekey="lblZipFile">Zip File: </asp:Label>
			</td>
            <td class="Normal" width="100%">
   				<asp:TextBox ID="txtZipFile" runat="server" Width="95%" CssClass="form-control" MaxLength="1000"></asp:TextBox><br />
   				<asp:Label ID="lblZipFileHint" runat="server" meta:resourcekey="lblZipFileHint" Text="([date], [time] variables are supported)"></asp:Label>
        </tr>     
   </table>
