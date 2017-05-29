<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail_EditDomain_Features.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail_EditDomain_Features" %>
<table width="100%">
    <tr>
        <td style="width:150px;" align="right"><asp:Label runat="server" meta:resourcekey="cbShowcontentfilteringmenu" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowcontentfilteringmenu"  />
        </td>
    </tr>
    <tr>
        <td align="right"><asp:Label ID="Label1" meta:resourcekey="cbShowdomainaliasmenu" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowdomainaliasmenu" />
        </td>
    </tr>
    <tr>
        <td align="right"><asp:Label ID="Label2"  meta:resourcekey="cbShowlistmenu" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowlistmenu" />
        </td>
    </tr>
    <tr>
        <td align="right"><asp:Label ID="Label3" meta:resourcekey="cbShowspammenu" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowspammenu" />
        </td>
    </tr>
    <tr>
        <td align="right"><asp:Label ID="Label4" meta:resourcekey="cbShowDomainReports" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowDomainReports" />
        </td>
    </tr>
</table>