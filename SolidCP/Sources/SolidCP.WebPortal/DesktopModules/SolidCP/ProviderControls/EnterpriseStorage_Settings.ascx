<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStorage_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.EnterpriseStorage_Settings" %>
<table cellpadding="1" cellspacing="0" width="100%">
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label ID="lblSpacesFolder" runat="server" meta:resourcekey="lblSpacesFolder" Text="User Packages Path:"></asp:Label>
        </td>
        <td width="100%">
            <asp:TextBox runat="server" ID="txtFolder" Width="300px" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequiredFolder" runat="server" ControlToValidate="txtFolder"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valExpressionFolder"  runat="server"   ValidationExpression="[a-zA-Z]:\\((\w(\s)?)+(\\)?)+"  
                ControlToValidate="txtFolder"  ErrorMessage="*"></asp:RegularExpressionValidator>  
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label ID="lblDomain" runat="server" meta:resourcekey="lblDomain" Text="User domain:"></asp:Label>
        </td>
        <td width="100%">
            <asp:TextBox runat="server" ID="txtDomain" Width="300px" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequiredDomain" runat="server" ControlToValidate="txtDomain"
                ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valExpressionDomain"  runat="server"   ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"  
                ControlToValidate="txtDomain"  ErrorMessage="*"></asp:RegularExpressionValidator>  
        </td>
    </tr>
    <tr>
		<td class="SubHead" style="width:200px;">
		    <asp:Label ID="lblUseStorageSpaces" runat="server" meta:resourcekey="lblUseStorageSpaces" Text="Use storage sapces:"></asp:Label>
		</td>
		<td>
			<asp:CheckBox ID="chkUseStorageSpaces" runat="server" meta:resourcekey="chkUseStorageSpaces" Text="Yes" />
		</td>
	</tr>
</table>
