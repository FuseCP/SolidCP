<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteFooter.ascx.cs" Inherits="SolidCP.Portal.SkinControls.SiteFooter" %>
<%@ Register TagPrefix="scp" TagName="ProductVersion" Src="ProductVersion.ascx" %>
<div class="row">
    <asp:Panel runat="server" ID="Copyright" CssClass="col-md-2 Copyright">
        <asp:Localize ID="locPoweredBy" runat="server" meta:resourcekey="locPoweredBy" />
    </asp:Panel>
    <asp:Panel runat="server" ID="EntityFrameworkPanel" Visible="false" CssClass="col-md-7 UseEntityFramework">
        <asp:CheckBox runat="server" ID="chkUseEntityFramework" AutoPostBack="True" Text="Use EntityFramework for database access"
            OnCheckedChanged="chkUseEntityFramework_CheckedChanged" meta:resourcekey="chkUseEntityFramework" />
    </asp:Panel>
    <div class="col-md-3 Version">
        <asp:Localize ID="locVersion" runat="server" meta:resourcekey="locVersion" />
        <scp:ProductVersion ID="scpVersion" runat="server" AssemblyName="SolidCP.Portal.Modules" />
    </div>
</div>