<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpamExperts_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SpamExperts_Settings" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
	    <td class="SubHead" nowrap="nowrap">
            <asp:Label ID="lblHostname" runat="server" meta:resourcekey="lblHostname" Text="Hostname:"></asp:Label>
        </td>
	    <td class="Normal" valign="top">
            <asp:TextBox Runat="server" ID="txtServiceUrl" CssClass="form-control"></asp:TextBox></td>
	</tr>
	<tr>
	    <td class="SubHead" nowrap="nowrap">
            <asp:Label ID="lblAdminLogin" runat="server" meta:resourcekey="lblAdminLogin" Text="Admin Login:"></asp:Label>
        </td>
	    <td class="Normal" valign="top">
            <asp:TextBox Runat="server" ID="txtAdminUser" CssClass="form-control"></asp:TextBox></td>
	</tr>
	<tr>
	    <td class="SubHead" nowrap="nowrap">
            <asp:Label ID="lblAdminPassword" runat="server" meta:resourcekey="lblAdminPassword" Text="Admin Password:"></asp:Label>
       </td>
	    <td class="Normal" valign="top">
            <asp:TextBox Runat="server" ID="txtAdminPassword" CssClass="form-control" TextMode="Password"></asp:TextBox>
      </td>
	</tr>
</table>