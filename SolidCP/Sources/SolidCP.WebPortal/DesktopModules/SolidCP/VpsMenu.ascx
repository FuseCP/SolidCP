<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsMenu.ascx.cs" Inherits="SolidCP.Portal.VpsMenu" %>

<asp:Localize ID="locMenuTitle" runat="server" meta:resourcekey="locMenuTitle" Visible="false"></asp:Localize>
<div id="vpsMenu" runat="server">
    <asp:Menu ID="menu" runat="server"
        Orientation="Horizontal"
        EnableViewState="false"
        CssSelectorClass="TopMenu" >
    </asp:Menu>
</div>

