<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Merak_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Merak_Settings" %>
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
	<tr>
		<td class="SubHead" nowrap>
		    <asp:Label ID="lblAccountType" runat="server" meta:resourcekey="lblAccountType" Text="Mail Account Type:"></asp:Label>
		</td>
		<td width="100%">
            <asp:DropDownList runat="server" id="ddlAccountType" CssClass="form-control">
				<asp:listitem value="0" text="POP3" />
				<asp:listitem value="1" text="IMAP & POP3" />
				<asp:listitem value="2" text="IMAP" />
            </asp:DropDownList>
        </td>
	</tr>
</table>