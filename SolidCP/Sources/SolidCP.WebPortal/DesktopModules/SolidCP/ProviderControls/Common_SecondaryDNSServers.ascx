<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Common_SecondaryDNSServers.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.Common_SecondaryDNSServers" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<table>
    <tr>
        <td valign="top">
            <asp:ListBox ID="lbServices" runat="server" Width="200px" Rows="3" CssClass="form-control"></asp:ListBox></td>
        <td valign="top">
            <asp:ImageButton ID="btnRemove" runat="server" SkinID="DeleteSmall"
				meta:resourcekey="btnRemove" OnClick="btnRemove_Click" /></td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlService" runat="server" CssClass="form-control" Width="100%">
            </asp:DropDownList></td>
        <td><CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton></td>
    </tr>
</table>