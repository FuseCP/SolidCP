<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="hMailServer_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.hMailServer_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<table cellpadding="7" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblPublicIP" runat="server" meta:resourcekey="lblPublicIP" Text="Public IP Address:"></asp:Label>
		</td>
		<td width="100%">
            <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" />
        </td>
	</tr>
</table>