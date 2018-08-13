<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CerberusFTP6_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.CerberusFTP6_Settings" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
	    <td class="SubHead" nowrap="nowrap">
            <asp:Label ID="lblServiceUrl" runat="server" meta:resourcekey="lblServiceUrl" Text="Web Services URL:"></asp:Label>
        </td>
	    <td class="Normal" valign="top">
            <asp:TextBox Runat="server" ID="txtServiceUrl" CssClass="form-control"></asp:TextBox></td>
	</tr>
	<tr>
	    <td class="SubHead" nowrap="nowrap">
            <asp:Label ID="lblAdminLogin" runat="server" meta:resourcekey="lblAdminLogin" Text="Admin Login:"></asp:Label>
        </td>
	    <td class="Normal" valign="top">
            <asp:TextBox Runat="server" ID="txtUsername" CssClass="form-control"></asp:TextBox></td>
	</tr>
	<tr>
	    <td class="SubHead" nowrap="nowrap">
            <asp:Label ID="lblAdminPassword" runat="server" meta:resourcekey="lblAdminPassword" Text="Admin Password:"></asp:Label>
       </td>
	    <td class="Normal" valign="top">
            <asp:TextBox Runat="server" ID="txtPassword" CssClass="form-control" TextMode="Password"></asp:TextBox>
      </td>
	</tr>
</table>