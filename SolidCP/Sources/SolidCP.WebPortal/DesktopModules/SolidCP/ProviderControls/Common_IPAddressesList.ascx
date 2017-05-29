<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Common_IPAddressesList.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Common_IPAddressesList" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<table>
    <tr>
        <td valign="top">
            <asp:ListBox ID="lbAddresses" runat="server" Width="200px" Rows="3" CssClass="form-control"></asp:ListBox></td>
        <td valign="top" width="100%">
            <asp:ImageButton ID="btnRemove" runat="server" SkinID="DeleteSmall" meta:resourcekey="btnRemove"
				OnClick="btnRemove_Click" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" />
            <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
        </td>
    </tr>
</table>
