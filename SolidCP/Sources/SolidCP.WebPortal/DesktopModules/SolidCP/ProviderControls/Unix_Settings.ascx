<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Unix_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Unix_Settings" %>
<table cellpadding="1" cellspacing="0" width="100%">
		<tr>
			<td class="SubHead" width="200" nowrap>
			    <asp:Label ID="lblUsersHome" runat="server" meta:resourcekey="lblUsersHome" Text="Users Home Directory:"></asp:Label>
			</td>
			<td width="100%"><asp:TextBox Runat="server" ID="txtUsersHome" Width="300px" CssClass="form-control"></asp:TextBox></td>
		</tr>
		<tr>
			<td class="SubHead" width="200" nowrap>
			    <asp:Label ID="lblLogDir" runat="server" meta:resourcekey="lblLogDir" Text="Log Directory:"></asp:Label>
			</td>
			<td width="100%"><asp:TextBox Runat="server" ID="txtLogDir" Width="300px" CssClass="form-control"></asp:TextBox></td>
		</tr>
</table>
