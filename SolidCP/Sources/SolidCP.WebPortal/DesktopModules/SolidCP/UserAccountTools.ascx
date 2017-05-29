<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountTools.ascx.cs" Inherits="SolidCP.Portal.UserAccountTools" %>
<div class="widget">
							<div class="widget-header clearfix">
								<h3><i class="fa fa-user"></i> <span><asp:Localize id="ToolsHeader" runat="server" meta:resourcekey="ToolsHeader" Text="Account Tools"></asp:Localize></span></h3>
								<div class="btn-group widget-header-toolbar">
									<a href="#" title="Expand/Collapse" class="btn btn-link btn-toggle-expand"><i class="icon ion-ios-arrow-up"></i></a>
									<a href="#" title="Remove" class="btn btn-link btn-remove"><i class="icon ion-ios-close-empty"></i></a>
								</div>
							</div>
							<div class="widget-content">
<asp:Panel ID="ToolsPanel" runat="server" CssClass="Normal">
    <div class="ToolLink">
        <asp:HyperLink ID="lnkBackup" runat="server" meta:resourcekey="lnkBackup" Text="Backup"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkRestore" runat="server" meta:resourcekey="lnkRestore" Text="Restore"></asp:HyperLink>
    </div>
</asp:Panel>
</div></div>