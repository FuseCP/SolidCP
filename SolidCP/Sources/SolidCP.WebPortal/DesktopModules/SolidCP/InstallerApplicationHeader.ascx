<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstallerApplicationHeader.ascx.cs" Inherits="SolidCP.Portal.InstallerApplicationHeader" %>
<table width="100%">
	<tr>
		<td colspan="2" class="Huge">
			<asp:Literal ID="litName" Runat="server"></asp:Literal>
			<hr size="1">
		</td>
	</tr>
	<tr>
		<td width="200" valign="top" nowrap>
			<table width="100%" cellpadding="3">
				<tr>
					<td>
						<asp:Image id="imgLogo" runat="server"></asp:Image></td>
				</tr>
				<tr>
					<td class="Normal">
						</td>
				</tr>
			</table>
		</td>
		<td width="100%" valign="top">
			<table width="100%" cellpadding="3">
				<tr>
					<td nowrap class="SubHead"><asp:Label ID="lblManufacturer" runat="server" meta:resourcekey="lblManufacturer" Text="Manufacturer:"></asp:Label></td>
					<td width="100%" class="Normal">
						<asp:Literal id="litManufacturer" Runat="server"></asp:Literal></td>
				</tr>
				<tr>
					<td nowrap class="SubHead"><asp:Label ID="lblVersion" runat="server" meta:resourcekey="lblVersion" Text="Version:"></asp:Label></td>
					<td class="Normal">
						<asp:Literal id="litVersion" Runat="server"></asp:Literal></td>
				</tr>
				<tr>
					<td nowrap class="SubHead"><asp:Label ID="lblLicense" runat="server" meta:resourcekey="lblLicense" Text="License:"></asp:Label></td>
					<td class="Normal">
						<asp:Literal id="litLicense" Runat="server"></asp:Literal></td>
				</tr>
				<tr>
					<td nowrap class="SubHead"><asp:Label ID="lblSize" runat="server" meta:resourcekey="lblSize" Text="Size, MB:"></asp:Label></td>
					<td class="Normal">
						<asp:Literal id="litSize" Runat="server"></asp:Literal></td>
				</tr>
				<tr>
					<td class="Normal">&nbsp;</td>
				</tr>
				<tr id="rowHomeSite" runat="server">
					<td nowrap class="SubHead"><asp:Label ID="lblWebSite" runat="server" meta:resourcekey="lblWebSite" Text="Web site:"></asp:Label></td>
					<td class="Normal">
						<asp:HyperLink id="lnkHomeSite" runat="server" Target="_blank"></asp:HyperLink></td>
				</tr>
				<tr id="rowSupportSite" runat="server">
					<td nowrap class="SubHead"><asp:Label ID="lblSupportSite" runat="server" meta:resourcekey="lblSupportSite" Text="Support site:"></asp:Label></td>
					<td class="Normal">
						<asp:HyperLink id="lnkSupportSite" runat="server" Target="_blank"></asp:HyperLink></td>
				</tr>
				<tr id="rowDocSite" runat="server">
					<td nowrap class="SubHead"><asp:Label ID="lblDocumentation" runat="server" meta:resourcekey="lblDocumentation" Text="Documentation:"></asp:Label></td>
					<td class="Normal">
						<asp:HyperLink id="lnkDocSite" runat="server" Target="_blank"></asp:HyperLink></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td></td>
		<td class="Normal">
			<asp:Literal id="litFullDescription" Runat="server"></asp:Literal>
		</td>
	</tr>
</table>
