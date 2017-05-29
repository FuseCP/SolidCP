<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail50_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail50_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<table cellpadding="7" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" nowrap width="200">
		    <asp:Label ID="lblServiceUrl" runat="server" meta:resourcekey="lblServiceUrl" Text="Web Services URL:"></asp:Label>
		</td>
		<td width="100%"><asp:TextBox Runat="server" ID="txtServiceUrl" CssClass="form-control" Width="200px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblPublicIP" runat="server" meta:resourcekey="lblPublicIP" Text="Public IP Address:"></asp:Label>
		</td>
		<td>
            <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" UseAddressValueAsKey="true" />
        </td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblDomainsPath" runat="server" meta:resourcekey="lblDomainsPath" Text="Domains Root Folder:"></asp:Label>
		</td>
		<td>
		    <asp:TextBox Runat="server" ID="txtDomainsFolder" CssClass="form-control" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblAdminLogin" runat="server" meta:resourcekey="lblAdminLogin" Text="Admin Login:"></asp:Label>
		</td>
		<td>
		    <asp:TextBox Runat="server" ID="txtUsername" CssClass="form-control" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr id="rowPassword" runat="server">
		<td class="SubHead">
		    <asp:Label ID="lblCurrPassword" runat="server" meta:resourcekey="lblCurrPassword" Text="Current Admin Password:"></asp:Label>
		</td>
		<td class="Normal">*******
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblAdminPassword" runat="server" meta:resourcekey="lblAdminPassword" Text="Admin Password:"></asp:Label>
		</td>
		<td>
		    <asp:TextBox Runat="server" ID="txtPassword" CssClass="form-control" Width="200px" TextMode="Password"></asp:TextBox>
	    </td>	    
	</tr> 
	<tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblLicense" runat="server" meta:resourcekey="lblLicense" Text="License Type:"></asp:Label>
		</td>
		<td>
           <asp:DropDownList ID="ddlLicenseType" runat="server" CssClass="form-control">
				<asp:ListItem Value="ENT">Enterprise</asp:ListItem>
				<asp:ListItem Value="PRO">Professional</asp:ListItem>
            </asp:DropDownList>
        </td>
	</tr>
	    <td></td>
	    <td><asp:CheckBox runat="server" ID="cbImportDomainAdmin" meta:resourcekey="cbImportDomainAdmin"/></td>
	</tr>
	<tr>
	    <td></td>
	    <td><asp:CheckBox runat="server" ID="cbInheritDefaultLimits" meta:resourcekey="cbInheritDefaultLimits"/></td>
	</tr>
		
</table>

