<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationMenu.ascx.cs" Inherits="SolidCP.Portal.OrganizationMenu" %>

<asp:Localize ID="locMenuTitle" runat="server" meta:resourcekey="locMenuTitle" Visible="false"></asp:Localize>
<div id="orgMenu" runat="server">
    <asp:Menu ID="menu" runat="server"
        Orientation="Horizontal"
        EnableViewState="false"
        CssSelectorClass="TopMenu" >
    </asp:Menu>
</div>

