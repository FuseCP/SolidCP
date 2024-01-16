<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="vsftpd_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.vsftpd_Settings" %>
<table cellpadding="1" cellspacing="0" width="100%">
		<tr>
			<td class="SubHead" width="200" nowrap>
			    <asp:Label ID="lblConfigFile" runat="server" meta:resourcekey="lblConfigFile" Text="vsftpd Configuration File:"></asp:Label>
			</td>
			<td width="100%"><asp:TextBox Runat="server" ID="txtConfigFile" Width="300px" CssClass="form-control"></asp:TextBox></td>
		</tr>
</table>
