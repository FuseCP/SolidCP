<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettings.ascx.cs" Inherits="SolidCP.Portal.SpaceSettings" %>
<div class="widget">
							<div class="widget-header clearfix">
								<h3><i class="fa fa-cogs"></i> <span><asp:Localize id="SettingsHeader" runat="server" meta:resourcekey="SettingsHeader" Text="Settings"></asp:Localize></span></h3>
								<div class="btn-group widget-header-toolbar">
									<a href="#" title="Expand/Collapse" class="btn btn-link btn-toggle-expand"><i class="icon ion-ios-arrow-up"></i></a>
									<a href="#" title="Remove" class="btn btn-link btn-remove"><i class="icon ion-ios-close-empty"></i></a>
								</div>
							</div>
							<div class="widget-content">
<asp:Panel ID="SettingsPanel" runat="server">
    <div class="Normal">
        <div class="ToolLink">
            <asp:HyperLink ID="lnkNameServers" runat="server" meta:resourcekey="lnkNameServers" Text="Name Servers"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkInstantAlias" runat="server" meta:resourcekey="lnkInstantAlias" Text="Instant Alias"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkSharedSSL" runat="server" meta:resourcekey="lnkSharedSSL" Text="Shared SSL Sites"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkPackagesFolder" runat="server" meta:resourcekey="lnkPackagesFolder" Text="Child Spaces Location"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkDnsRecords" runat="server" meta:resourcekey="lnkDnsRecords" Text="Global DNS Records"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkExchangeServer" runat="server" meta:resourcekey="lnkExchangeServer" Text="Exchange Server"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkVps" runat="server" meta:resourcekey="lnkVps" Text="Virtual Private Servers"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkVps2012" runat="server" meta:resourcekey="lnkVps2012" Text="Virtual Private Servers 2012"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkVpsForPC" runat="server" meta:resourcekey="lnkVpsForPC" Text="Virtual Private Servers for Private Cloud"></asp:HyperLink>
        </div>
    </div>
</asp:Panel>
                                </div></div>