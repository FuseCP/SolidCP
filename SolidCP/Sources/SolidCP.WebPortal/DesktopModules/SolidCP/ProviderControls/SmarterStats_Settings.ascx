<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterStats_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterStats_Settings" %>
<%@ Register Src="../UserControls/TimeZones.ascx" TagName="TimeZones" TagPrefix="uc1" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" nowrap width="200">
		    <asp:Label ID="lblSmarterUrl" runat="server" meta:resourcekey="lblSmarterUrl" Text="SmarterStats Web Services URL:"></asp:Label>
		</td>
		<td width="100%"><asp:TextBox Runat="server" ID="txtSmarterUrl" CssClass="form-control" Width="200px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblUsername" runat="server" meta:resourcekey="lblUsername" Text="Admin Username:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtUsername" CssClass="form-control" Width="100px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Admin Password:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password" Width="100px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblServer" runat="server" meta:resourcekey="lblServer" Text="SmarterStats Server:"></asp:Label>
		</td>
		<td>
            <asp:DropDownList ID="ddlServers" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="ServerId">
            </asp:DropDownList>
        </td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblLogFormat" runat="server" meta:resourcekey="lblLogFormat" Text="Logs Format:"></asp:Label>
		</td>
		<td>
            <asp:DropDownList ID="ddlLogFormat" runat="server" CssClass="form-control">
				<asp:ListItem value="W3Cex">IIS - W3Cex Log Format</asp:ListItem>
				<asp:ListItem value="IIS">IIS - Microsoft IIS Log Format</asp:ListItem>
				<asp:ListItem value="NCSA">IIS - NCSA Common Log Format</asp:ListItem>
				<asp:ListItem value="ApacheCLF">Apache - Common Log Format</asp:ListItem>
				<asp:ListItem value="ApacheNCSAEx">Apache - NCSA Extended Log Format</asp:ListItem>
				<asp:ListItem value="IPlanetCLF">IPlanet - Common Log Format</asp:ListItem>
				<asp:ListItem value="CLF">Other - Common Log Format</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblLogWildcard" runat="server" meta:resourcekey="lblLogWildcard" Text="Log Wildcard:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtLogWilcard" CssClass="form-control"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblLogAutoDeletion" runat="server" meta:resourcekey="lblLogAutoDeletion" Text="Web Site Logs Auto-Deletion:"></asp:Label>
		</td>
		<td class="Normal">
		    <asp:TextBox Runat="server" ID="txtLogDeleteDays" CssClass="form-control" Width="40"></asp:TextBox>
		    <asp:Label ID="lblDontDelete" runat="server" meta:resourcekey="lblDontDelete" Text="(0 - never delete)"></asp:Label>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblSmarterLogs" runat="server" meta:resourcekey="lblSmarterLogs" Text="Smarter Logs Location:"></asp:Label>
		</td>
		<td class="Normal">
		    <asp:TextBox Runat="server" ID="txtSmarterLogs" CssClass="form-control"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblSmarterLogAutoDeletion" runat="server" meta:resourcekey="lblSmarterLogAutoDeletion" Text="Smarter Logs Auto-Deletion:"></asp:Label>
		</td>
		<td class="Normal">
		    <asp:TextBox Runat="server" ID="txtSmarterLogDeleteMonths" CssClass="form-control" Width="40"></asp:TextBox>
		    <asp:Label ID="Label1" runat="server" meta:resourcekey="lblDontDelete" Text="(0 - never delete)"></asp:Label>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblTimeZone" runat="server" meta:resourcekey="lblTimeZone" Text="Log Import Time Zone:"></asp:Label>
		</td>
		<td class="Normal">
		    <uc1:TimeZones ID="timeZone" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblBuildUncLogsPath" runat="server" meta:resourcekey="lblBuildUncLogsPath" Text="Build UNC Path to Web Logs:"></asp:Label>
		</td>
		<td>
			<asp:CheckBox ID="chkBuildUncLogsPath" runat="server" meta:resourcekey="chkBuildUncLogsPath" Text="Yes" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblStatsUrl" runat="server" meta:resourcekey="lblStatsUrl" Text="Statistics Help URL:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtStatsUrl" CssClass="form-control" Width="100%"></asp:TextBox></td>
	</tr>
</table>