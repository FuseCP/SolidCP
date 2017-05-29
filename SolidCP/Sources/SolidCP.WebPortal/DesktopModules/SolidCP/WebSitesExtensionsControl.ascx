<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesExtensionsControl.ascx.cs" Inherits="SolidCP.Portal.WebSitesExtensionsControl" %>
<div style="padding: 20;">
<table cellpadding="4">
    <tr id="rowAsp" runat="server">
        <td class="SubHead">
            <asp:Label ID="lblAsp" runat="server" meta:resourcekey="lblAsp" Text="ASP:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:CheckBox ID="chkAsp" runat="server" Text="Enabled" meta:resourcekey="chkEnabled" /></td>
    </tr>
    <tr id="rowAspNet" runat="server">
        <td class="SubHead">
            <asp:Label ID="lblAspNet" runat="server" meta:resourcekey="lblAspNet" Text="ASP.NET:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:DropDownList ID="ddlAspNet" runat="server" CssClass="form-control" resourcekey="ddlAspNet">
                <asp:ListItem Value="">None</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="2I">2I</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="4I">4I</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="rowPhp" runat="server">
        <td class="SubHead">
            <asp:Label ID="lblPhp" runat="server" meta:resourcekey="lblPhp" Text="PHP:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:DropDownList ID="ddlPhp" runat="server" CssClass="form-control" resourcekey="ddlPhp">
                <asp:ListItem Value="">None</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="rowPerl" runat="server">
        <td class="SubHead">
            <asp:Label ID="lblPerl" runat="server" meta:resourcekey="lblPerl" Text="Perl:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:CheckBox ID="chkPerl" runat="server" Text="Enabled" meta:resourcekey="chkEnabled" /></td>
    </tr>
    <tr id="rowPython" runat="server">
        <td class="SubHead">
            <asp:Label ID="lblPython" runat="server" meta:resourcekey="lblPython" Text="Python:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:CheckBox ID="chkPython" runat="server" Text="Enabled" meta:resourcekey="chkEnabled" /></td>
    </tr>
    <tr id="rowCgiBin" runat="server">
        <td class="SubHead">
            <asp:Label ID="lblCgiBin" runat="server" meta:resourcekey="lblCgiBin" Text="CGI-BIN:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:CheckBox ID="chkCgiBin" runat="server" Text="Installed" meta:resourcekey="chkInstalled" /></td>
    </tr>
</table>
</div>