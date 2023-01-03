<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail100x_EditDomain_Features.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail100x_EditDomain_Features" %>
<table width="100%">
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label2" meta:resourcekey="lbShowdomainaliasmenu" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowdomainaliasmenu" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label3"  meta:resourcekey="lbShowlistmenu" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowlistmenu" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label4" meta:resourcekey="lbShowspammenu" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowspammenu" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label7" meta:resourcekey="lbEnableCatchAlls" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnableCatchAlls" />
        </td>
    </tr>
</table>