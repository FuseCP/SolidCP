<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AWStats_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.AWStats_Settings" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" nowrap width="200">
		    <asp:Label ID="lblCgiBinFolder" runat="server" meta:resourcekey="lblCgiBinFolder" Text="Cgi-Bin Folder:"></asp:Label>
		</td>
		<td width="100%"><asp:TextBox Runat="server" ID="txtAwStatsFolder" CssClass="form-control" Width="200px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblBatchFileName" runat="server" meta:resourcekey="lblBatchFileName" Text="Batch File Name:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtBatchFileName" CssClass="form-control" Width="200px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblLineTemplate" runat="server" meta:resourcekey="lblLineTemplate" Text="Batch File Line Template:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtBatchLineTemplate" CssClass="form-control" Width="100%"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead" noWrap>
            <asp:Label ID="lblConfigFileName" runat="server" meta:resourcekey="lblConfigFileName" Text="Configuration File Name:"></asp:Label>
        </td>
		<td vAlign="top"><asp:TextBox Runat="server" ID="txtConfigFileName" CssClass="form-control" Width="200px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead" vAlign="top">
		    <asp:Label ID="lblFileTemplate" runat="server" meta:resourcekey="lblFileTemplate" Text="Configuration File Template:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtConfigFileTemplate" CssClass="form-control" Width="100%"
				TextMode="MultiLine" Rows="10" Wrap="False"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblFileTemplatePath" runat="server" meta:resourcekey="lblFileTemplatePath" Text="Configuration Template Path:"></asp:Label>
		</td>
		<td><asp:TextBox Runat="server" ID="txtConfigFileTemplatePath" CssClass="form-control" Width="100%"></asp:TextBox></td>
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
