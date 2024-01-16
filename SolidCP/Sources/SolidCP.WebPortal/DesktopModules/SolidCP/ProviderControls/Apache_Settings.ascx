<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Apache_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Apache_Settings" %>
<table cellpadding="1" cellspacing="0" width="100%">
		<tr>
			<td class="SubHead" width="200" nowrap>
			    <asp:Label ID="lblConfigPath" runat="server" meta:resourcekey="lblConfigPath" Text="Apache Configuration Path:"></asp:Label>
			</td>
			<td width="100%"><asp:TextBox Runat="server" ID="txtApacheConfigPath" Width="300px" CssClass="form-control"></asp:TextBox></td>
		</tr>
		<tr>
			<td class="SubHead" width="200" nowrap>
			    <asp:Label ID="lblConfigFile" runat="server" meta:resourcekey="lblConfigFile" Text="Apache Configuration File:"></asp:Label>
			</td>
			<td width="100%"><asp:TextBox Runat="server" ID="txtApacheConfigFile" Width="300px" CssClass="form-control"></asp:TextBox></td>
		</tr>
		<tr>
			<td class="SubHead" width="200" nowrap>
			    <asp:Label ID="lblBinPath" runat="server" meta:resourcekey="lblBinPath" Text="Apache Bin Path:"></asp:Label>
			</td>
			<td width="100%"><asp:TextBox Runat="server" ID="txtApacheBinPath" Width="300px" CssClass="form-control"></asp:TextBox></td>
		</tr>
</table>
