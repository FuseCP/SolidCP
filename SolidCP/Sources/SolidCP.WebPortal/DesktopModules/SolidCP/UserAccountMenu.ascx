<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountMenu.ascx.cs" Inherits="SolidCP.Portal.UserAccountMenu" %>

<asp:Localize ID="locMenuTitle" runat="server" meta:resourcekey="locMenuTitle" Visible="false"></asp:Localize>

<asp:Menu ID="menu" runat="server"
    Orientation="Horizontal"
    EnableViewState="false"
    CssSelectorClass="TopMenu-UserAccountMenu" OnMenuItemDataBound="menu_MenuItemDataBound" >
</asp:Menu>
