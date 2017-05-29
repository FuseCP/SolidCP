<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceDetails.ascx.cs" Inherits="SolidCP.Portal.SpaceDetails" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="scp" %>
<div class="widget">
							<div class="widget-header clearfix">
								<h3><i class="fa fa-server"></i> <span><asp:Literal ID="litSpaceName" runat="server"></asp:Literal></span></h3>
								<div class="btn-group widget-header-toolbar">
									<a href="#" title="Expand/Collapse" class="btn btn-link btn-toggle-expand"><i class="icon ion-ios-arrow-up"></i></a>
									<a href="#" title="Remove" class="btn btn-link btn-remove"><i class="icon ion-ios-close-empty"></i></a>
								</div>
                           </div>
                                <div class="widget-content">
<div class="FormRightIcon">
    <asp:Image ID="Image1" runat="server" SkinID="Space128" />
</div>
<table class="RightBlockTable">
    <tr>
        <td class="SubHead"><asp:Label ID="lblCreated" runat="server" meta:resourcekey="lblCreated" Text="Created:"></asp:Label></td>
        <td class="Normal"><asp:Literal ID="litCreated" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblHostingPlan" runat="server" meta:resourcekey="lblHostingPlan" Text="Hosting Plan:"></asp:Label></td>
        <td class="Normal"><asp:Literal ID="litHostingPlan" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblServer" runat="server" meta:resourcekey="lblServer" Text="Server:"></asp:Label></td>
        <td class="Normal"><scp:ServerDetails ID="serverDetails" runat="server" /></td>
    </tr>
</table>
<div class="Normal">
    <div class="ToolLink">
        <asp:HyperLink ID="lnkSummaryLetter" runat="server" meta:resourcekey="lnkSummaryLetter" Text="View Summary Letter"></asp:HyperLink>
    </div>
    <div class="ToolLink" runat="server" id="OverusageReport">
        <asp:HyperLink ID="lnkOverusageReport" runat="server" meta:resourcekey="lnkOverusageReport" Text="Overusage Report"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkEditSpaceDetails" runat="server" meta:resourcekey="lnkEditSpaceDetails" Text="Edit Details"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkDelete" runat="server" meta:resourcekey="lnkDelete" Text="Delete"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:CheckBox ID="chkDefault" runat="server" meta:resourcekey="chkDefaultSpace" AutoPostBack="true" OnCheckedChanged="chkDefault_CheckedChanged" Text="Default space" />
    </div>
</div>
</div>
</div>
<br />
    <div class="widget">
							<div class="widget-header clearfix">
								<h3><i class="fa fa-heartbeat"></i> <span><asp:Localize id="StatusHeader" runat="server" meta:resourcekey="StatusHeader" Text="Space Status"></asp:Localize></span></h3>
								<div class="btn-group widget-header-toolbar">
									<a href="#" title="Expand/Collapse" class="btn btn-link btn-toggle-expand"><i class="icon ion-ios-arrow-up"></i></a>
									<a href="#" title="Remove" class="btn btn-link btn-remove"><i class="icon ion-ios-close-empty"></i></a>
								</div>
                                </div>
                                <div class="widget-content">
<asp:Panel ID="StatusPanel" runat="server">
	<table cellpadding="5" style="width:100%;">
		<tr>
			<td align="center">
				<div class="MediumBold" style="padding:5px;">
					<asp:Literal ID="litStatus" runat="server"></asp:Literal>
				</div>
				<div id="StatusBlock" runat="server" style="padding:5px;text-align:center;">
					<asp:ImageButton ID="cmdActive" runat="server" SkinID="Start" meta:resourcekey="cmdActive" AlternateText="Activate" CommandName="Active" OnClick="statusButton_Click" />
					<asp:ImageButton ID="cmdSuspend" runat="server" SkinID="Pause" meta:resourcekey="cmdSuspend" AlternateText="Suspend" CommandName="Suspended" OnClick="statusButton_Click" />
					<asp:ImageButton ID="cmdCancel" runat="server" SkinID="Stop" meta:resourcekey="cmdCancel" AlternateText="Cancel" CommandName="Cancelled" OnClick="statusButton_Click" />
				</div>
			</td>
		</tr>
	</table>
</asp:Panel>
                                    </div>
</div><br />