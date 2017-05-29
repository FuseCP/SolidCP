<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignedInUser.ascx.cs" Inherits="SolidCP.Portal.SkinControls.SignedInUser" %>

<asp:Panel ID="AnonymousPanel" runat="server">
	<asp:HyperLink ID="lnkSignIn" runat="server" meta:resourcekey="lnkSignIn">Sign In</asp:HyperLink>
</asp:Panel>


<asp:Panel ID="LoggedPanel" runat="server">

    <ul class="nav navbar-nav navbar-right hidden-xs">
    <li>
    <asp:HyperLink ID="lnkEditUserDetails" runat="server">
        <i class="fa fa-user"></i>&nbsp;
        <span class="hidden-xs hidden-sm"><asp:Localize runat="server" meta:resourcekey="lnkEditUserDetails" /></span>
    </asp:HyperLink>
    </li>
    <li>
    <asp:LinkButton ID="cmdSignOut" runat="server" CausesValidation="false" OnClick="cmdSignOut_Click">
        <i class="fa fa-sign-out "></i>
        <asp:Localize runat="server" meta:resourcekey="cmdSignOut" />
    </asp:LinkButton>
    </li>
        </ul>
</asp:Panel>


<asp:Panel ID="LoggedPanelSm" runat="server" CssClass="visible-xs-block">
    <ul class="nav navbar-sm navbar-right">
    <li>
     <a href="#" class="show-search">
        <i class="fa fa-search fa-2x">&nbsp;</i>
     </a>
    </li>
    <li>
    <asp:HyperLink ID="lnkEditUserDetailsSm" runat="server"><span><i class="fa fa-user fa-2x">&nbsp;</i></span>
    </asp:HyperLink>
    </li>
    <li>
    <asp:LinkButton ID="cmdSignOutSm" runat="server" CausesValidation="false" OnClick="cmdSignOut_Click">
        <i class="fa fa-sign-out fa-2x"></i>
    </asp:LinkButton>
    </li>
        </ul>
</asp:Panel>