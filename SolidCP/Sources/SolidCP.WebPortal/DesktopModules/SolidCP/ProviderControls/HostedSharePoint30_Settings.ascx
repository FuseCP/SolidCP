<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostedSharePoint30_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.HostedSharePoint30_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="scp" %>

<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblRootWebApplication" runat="server" meta:resourcekey="lblRootWebApplication" Text="SharePoint Web Application Url:"></asp:Label>
		</td>
		<td width="100%">
            <asp:TextBox ID="txtRootWebApplication" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
        </td>
	</tr>
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblRootWebApplicationIpAddress" runat="server" meta:resourcekey="lblRootWebApplicationIpAddress" Text="SharePoint Web Application IP:"></asp:Label>
		</td>
		<td width="100%">
			 <scp:SelectIPAddress ID="ddlRootWebApplicationIpAddress" runat="server" ServerIdParam="ServerID" AllowEmptySelection="false" />
        </td>
    </tr>
	<tr>
        <td colspan="2">
	        <asp:CheckBox ID="chkLocalHostFile" runat="server" meta:resourcekey="chkLocalHostFile" Text="Provision localhost file" />
	    </td>
    </tr>
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblSharedSSLRoot" runat="server" meta:resourcekey="lblSharedSSLRoot" Text="Shared SSL Root:"></asp:Label>
		</td>
		<td width="100%">
            <asp:TextBox ID="txtSharedSSLRoot" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
        </td>
	</tr>

</table>

<fieldset>
    <legend>
        <asp:Label ID="lblSharePointBackup" runat="server" meta:resourcekey="lblSharePointBackup" Text="SharePoint Backup" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">

		<tr>
		    <td class="Normal" valign="top" width="181">
		        <asp:Label ID="lblBackupTempFolder" runat="server" meta:resourcekey="lblBackupTempFolder" Text="SharePoint Backup Temporary Folder:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtBackupTempFolder" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
                <p style="text-align: justify;"><i><asp:Localize ID="Localize1" runat="server" meta:resourcekey="lclTempBackupNote" /></i></p>
			</td>
		</tr>
				
    </table>
</fieldset>

<br />