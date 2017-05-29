<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Breadcrumb.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.Breadcrumb" %>
<div class="Breadcrumb">
	<asp:HyperLink ID="lnkHome" runat="server" meta:resourcekey="lnkHome">Exchange Organizations</asp:HyperLink>
	<span id="spanOrg" runat="server">
		<asp:Image ID="imgSep1" runat="server" SkinID="PathSeparatorWhite" />
		<asp:HyperLink ID="lnkOrg" runat="server">Organization</asp:HyperLink>
	</span>
	<span id="spanPage" runat="server">
		<asp:Image ID="imgSep2" runat="server" SkinID="PathSeparatorWhite" />
		<asp:Literal id="litPage" runat="server">Page Name</asp:Literal>
	</span>
</div>