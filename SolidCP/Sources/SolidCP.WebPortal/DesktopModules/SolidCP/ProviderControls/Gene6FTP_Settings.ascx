<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Gene6FTP_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Gene6FTP_Settings" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
	    <td class="SubHead" nowrap="nowrap">
	        <asp:Label ID="lblInstallFolder" runat="server" meta:resourcekey="lblInstallFolder" Text="Installation Folder:"></asp:Label>
	    </td>
	    <td class="Normal" valign="top">
            <asp:TextBox ID="txtInstallFolder" runat="server" CssClass="form-control" Width="300px"></asp:TextBox></td>
	</tr>
	<tr>
	    <td class="SubHead" nowrap="nowrap">
	        <asp:Label ID="lblLogsFolder" runat="server" meta:resourcekey="lblLogsFolder" Text="Logs Folder:"></asp:Label>
	    </td>
	    <td class="Normal" valign="top">
            <asp:TextBox ID="txtLogsFolder" runat="server" CssClass="form-control" Width="300px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead"  nowrap="nowrap">
		    <asp:Label ID="lblSite" runat="server" meta:resourcekey="lblSite" Text="FTP Accounts Site:"></asp:Label>
		</td>
		<td width="100%">
            <asp:DropDownList ID="ddlFtpSite" runat="server" CssClass="form-control"
                    DataValueField="SiteId" DataTextField="Name">
            </asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblBuildUncFilesPath" runat="server" meta:resourcekey="lblBuildUncFilesPath" Text="Build UNC Path to Space Files:"></asp:Label>
		</td>
		<td>
			<asp:CheckBox ID="chkBuildUncFilesPath" runat="server" meta:resourcekey="chkBuildUncFilesPath" Text="Yes" />
		</td>
	</tr>
</table>