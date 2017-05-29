<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="SolidCP.Portal" %>
<script runat="server">
	void Page_Load()
	{
		this.DataBind();
	}
</script>
<div class="BrowseContainer">
    <div class="Top"><div class="Left"></div></div>
    <asp:Panel ID="HeaderPanel" runat="server" CssClass="Title" style="cursor:pointer;">
		<table cellpadding="0" cellspacing="0" style="width:100%;">
			<tr>
				<td style="width:100%;">
					<asp:Image ID="imgModuleIcon" runat="server" ImageAlign="AbsMiddle" Width="48px" Height="48px" />
					&nbsp;<asp:Label ID="lblModuleTitle" runat="server" CssClass="Head"></asp:Label>
				</td>
				<td style="padding-left:5px;padding-right:5px;"><asp:Image ID="ToggleImage" runat="server" ImageUrl='<%# PortalUtils.GetThemedImage("module_expand.gif") %>' /></td>
			</tr>
		</table>
    </asp:Panel>
    <asp:Panel ID="ContentPanel" runat="server" CssClass="Content">
        <asp:PlaceHolder ID="ContentPane" runat="server"/>
    </asp:Panel>
</div>
<ajaxToolkit:CollapsiblePanelExtender ID="cpe" runat="Server"
        TargetControlID="ContentPanel"
        ExpandControlID="HeaderPanel"
        CollapseControlID="HeaderPanel"
        Collapsed="True"        
        ExpandDirection="Vertical"
        ImageControlID="ToggleImage"
        ExpandedImage='<%# PortalUtils.GetThemedImage("module_collapse.gif") %>'
        ExpandedText="Collapse"
        CollapsedImage='<%# PortalUtils.GetThemedImage("module_expand.gif") %>'
        CollapsedText="Expand"
        SuppressPostBack="true" /> 