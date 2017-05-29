<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPAddressesAddIPAddress.ascx.cs" Inherits="SolidCP.Portal.IPAddressesAddIPAddress" %>
<%@ Register Src="UserControls/EditIPAddressControl.ascx" TagName="EditIPAddressControl" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<div class="panel-body form-horizontal">

    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <asp:ValidationSummary ID="validatorsSummary" runat="server"  ValidationGroup="EditAddress" ShowMessageBox="True" ShowSummary="False" />
	<asp:CustomValidator ID="consistentAddresses" runat="server" ErrorMessage="You must not mix IPv4 and IPv6 addresses." ValidationGroup="EditAddress" Display="dynamic" ServerValidate="CheckIPAddresses" /> 
	<table cellspacing="0" cellpadding="3">
	    <tr>
		    <td style="width:150px;">
		        <asp:Localize ID="locPool" runat="server" meta:resourcekey="locPool" Text="Pool:"></asp:Localize>
		    </td>
		    <td>
		        <asp:DropDownList ID="ddlPools" runat="server" CssClass="form-control" AutoPostBack="true" onselectedindexchanged="ddlPools_SelectedIndexChanged">
		            <asp:ListItem Value="General" meta:resourcekey="ddlPoolsGeneral">General</asp:ListItem>
		            <asp:ListItem Value="WebSites" meta:resourcekey="ddlPoolsWebSites">WebSites</asp:ListItem>
		            <asp:ListItem Value="VpsExternalNetwork" meta:resourcekey="ddlPoolsVpsExternalNetwork">VpsExternalNetwork</asp:ListItem>
		            <asp:ListItem Value="VpsManagementNetwork" meta:resourcekey="ddlPoolsVpsManagementNetwork">VpsManagementNetwork</asp:ListItem>
		        </asp:DropDownList>
            </td>
	    </tr>
	    <tr>
		    <td>
                <asp:Localize ID="locServer" runat="server" meta:resourcekey="locServer" Text="Server:"></asp:Localize>
		    </td>
		    <td>
		        <asp:dropdownlist id="ddlServer" CssClass="form-control" runat="server" DataTextField="ServerName" DataValueField="ServerID"></asp:dropdownlist>
		    </td>
	    </tr>
	    <tr>
		    <td>
                <asp:Localize ID="lblExternalIP" runat="server" meta:resourcekey="lblExternalIP" Text="IP Address:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="startIP" runat="server" ValidationGroup="EditAddress" Required="true" AllowSubnet="true" />
			    &nbsp;
                <asp:Localize ID="locTo" runat="server" meta:resourcekey="locTo" Text="to"></asp:Localize>
                &nbsp;
		        <scp:EditIPAddressControl id="endIP" runat="server" ValidationGroup="EditAddress" />
		    </td>
	    </tr>
	    <tr id="InternalAddressRow" runat="server">
		    <td>
		        <asp:Localize ID="lblInternalIP" runat="server" meta:resourcekey="lblInternalIP" Text="NAT Address:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditIPAddressControl id="internalIP" runat="server" ValidationGroup="EditAddress"  />
            </td>
	    </tr>
        <tr id="SubnetRow" runat="server">
	        <td>
                <asp:Localize ID="locSubnetMask" runat="server" meta:resourcekey="locSubnetMask" Text="Subnet Mask:"></asp:Localize>
	        </td>
	        <td class="NormalBold">
	            <scp:EditIPAddressControl id="subnetMask" runat="server" ValidationGroup="EditAddress" Required="true" AllowSubnet="true"  />
            </td>
        </tr>
        <tr id="GatewayRow" runat="server">
	        <td>
                <asp:Localize ID="locDefaultGateway" runat="server" meta:resourcekey="locDefaultGateway" Text="Default Gateway:"></asp:Localize>
	        </td>
	        <td class="NormalBold">
	            <scp:EditIPAddressControl id="defaultGateway" runat="server" ValidationGroup="EditAddress" Required="true"  />
            </td>
        </tr>
        <tr id="VLANRow" runat="server">
	        <td><asp:Localize ID="locVLAN" runat="server" meta:resourcekey="locVLAN" Text="VLAN:"></asp:Localize></td>
	        <td class="NormalBold">
	            <scp:EditIPAddressControl id="VLAN" runat="server" Required="true" Text="" />
            </td>
        </tr>
	    <tr>
		    <td><asp:Localize ID="lblComments" runat="server" meta:resourcekey="lblComments" Text="Comments:"></asp:Localize></td>
		    <td class="NormalBold">
                <asp:textbox id="txtComments" CssClass="form-control" runat="server" Rows="3" TextMode="MultiLine"></asp:textbox>
		    </td>
	    </tr>
    </table>
    
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" ValidationGroup="EditAddress">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAdd"/>
    </CPCC:StyleButton>
</div>