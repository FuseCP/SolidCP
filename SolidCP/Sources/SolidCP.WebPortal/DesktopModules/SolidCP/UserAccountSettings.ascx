<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountSettings.ascx.cs" Inherits="SolidCP.Portal.UserAccountSettings" %>
<div class="widget">
							<div class="widget-header clearfix">
								<h3><i class="fa fa-cogs"></i> <span><asp:Localize id="SettingsHeader" runat="server" meta:resourcekey="SettingsHeader" Text="Settings"></asp:Localize></span></h3>
								<div class="btn-group widget-header-toolbar">
									<a href="#" title="Expand/Collapse" class="btn btn-link btn-toggle-expand"><i class="icon ion-ios-arrow-up"></i></a>
									<a href="#" title="Remove" class="btn btn-link btn-remove"><i class="icon ion-ios-close-empty"></i></a>
								</div>
							</div>
							<div class="widget-content">
<asp:Panel ID="SettingsPanel" runat="server" CssClass="Normal">
    <div class="ToolLink">
        <asp:HyperLink ID="lnkMailTemplates" runat="server" meta:resourcekey="lnkMailTemplates" Text="Mail Templates"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkPolicies" runat="server" meta:resourcekey="lnkPolicies" Text="Policies"></asp:HyperLink>
    </div>
</asp:Panel>
                                </div>
    </div>
