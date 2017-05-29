<%@ Control Language="C#" AutoEventWireup="true" Codebehind="NetticaDNS_Settings.ascx.cs"
    Inherits="SolidCP.Portal.ProviderControls.NetticaDNS_Settings" %>
<%@ Register Src="Common_IPAddressesList.ascx" TagName="IPAddressesList" TagPrefix="uc2" %>
<%@ Register Src="Common_SecondaryDNSServers.ascx" TagName="SecondaryDNSServers"
    TagPrefix="uc1" %>
<table cellspacing="7" width="100%">
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblUserName" meta:resourcekey="lblUserName" /></td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtUserName" MaxLength="1000" />
            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ControlToValidate="txtUserName" Display="Dynamic" />    
        </td>
    </tr>
    <tr id="rowPassword" runat="server">
		<td class="SubHead">
		    <asp:Label ID="lblCurrPassword" runat="server" meta:resourcekey="lblCurrPassword" Text="Current User Password:"></asp:Label>
		</td>
		<td class="Normal">*******
		</td>
	</tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblPassword" meta:resourcekey="lblPassword" /></td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtPassword"  TextMode="Password" 
                MaxLength="1000" Width="150px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtUserName" Display="Dynamic" />                
            </td>
    </tr>
    <tr>
			<td class="SubHead" width="200" nowrap valign="top">
			    <asp:Label ID="lblIPAddresses" runat="server" meta:resourcekey="lblIPAddresses" Text="Listening IP Addresses:"></asp:Label>
			</td>
			<td width="100%" valign="top">
                <uc2:IPAddressesList id="iPAddressesList" runat="server">
                </uc2:IPAddressesList></td>
		</tr>
    <tr>
		    <td class="SubHead" valign="top">
		        <asp:Label ID="lblSecondaryDNS" runat="server" meta:resourcekey="lblSecondaryDNS" Text="Secondary DNS Services:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <uc1:SecondaryDNSServers ID="secondaryDNSServers" runat="server" />
            </td>
		</tr>
	<tr>
	    <td></td>
	    <td><asp:CheckBox runat="server" ID="cbApplyDefaultTemplate" meta:resourcekey="cbApplyDefaultTemplate"/></td>
	</tr>		        
</table>
