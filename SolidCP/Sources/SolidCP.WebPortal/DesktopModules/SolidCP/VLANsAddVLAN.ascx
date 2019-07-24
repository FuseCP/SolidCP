<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VLANsAddVLAN.ascx.cs" Inherits="SolidCP.Portal.VLANsAddVLANs" %>
<%@ Register Src="UserControls/EditVLANControl.ascx" TagName="EditVLANControl" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<div class="panel-body form-horizontal">

    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <asp:ValidationSummary ID="validatorsSummary" runat="server"  ValidationGroup="EditVLAN" ShowMessageBox="True" ShowSummary="False" />
	<asp:CustomValidator ID="consistentVLANs" runat="server" ErrorMessage="Wrong VLAN." ValidationGroup="EditVLAN" Display="dynamic" ServerValidate="CheckVLANs" /> 
	<table>
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
                <asp:Localize ID="lblVLAN" runat="server" meta:resourcekey="lblVLAN" Text="VLAN:"></asp:Localize>
		    </td>
		    <td>
		        <scp:EditVLANControl id="startVLAN" runat="server" ValidationGroup="EditVLAN" Required="true"/>
			    &nbsp;
                <asp:Localize ID="locTo" runat="server" meta:resourcekey="locTo" Text="to"></asp:Localize>
                &nbsp;
		        <scp:EditVLANControl id="endVLAN" runat="server" ValidationGroup="EditVLAN" />
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
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" ValidationGroup="EditVLAN">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAdd"/>
    </CPCC:StyleButton>
</div>