<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServU_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.ServU_Settings" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" width="200" nowrap>
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