<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail100_EditDomain_Features.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail100_EditDomain_Features" %>
<table width="100%">
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label1" runat="server" meta:resourcekey="lbShowcontentfilteringmenu" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowcontentfilteringmenu"  />
        </td>
    </tr>
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
        <td width="200px" align="right"><asp:Label ID="Label5" meta:resourcekey="lbShowDomainReports" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbShowDomainReports" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label6" meta:resourcekey="lbEnablePopRetreival" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnablePopRetreival" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label7" meta:resourcekey="lbEnableCatchAlls" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnableCatchAlls" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label8" meta:resourcekey="lbEnableIMAPRetreival" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnableIMAPRetreival" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label9" meta:resourcekey="lbEnableMailSigning" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnableEmailSigning" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label10" meta:resourcekey="lbEnableMailReports" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnableEmailReports" />
        </td>
    </tr>
    <tr>
        <td width="200px" align="right"><asp:Label ID="Label11" meta:resourcekey="lbEnableSyncML" runat="server" /></td>
        <td>
            <asp:CheckBox runat="server" ID="cbEnableSyncML" />
        </td>
    </tr>
</table>