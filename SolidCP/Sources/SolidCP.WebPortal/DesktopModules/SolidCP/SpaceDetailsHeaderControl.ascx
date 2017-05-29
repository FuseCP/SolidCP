<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceDetailsHeaderControl.ascx.cs" Inherits="SolidCP.Portal.SpaceDetailsHeaderControl" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="scp" %>
<table cellSpacing="0" cellPadding="5" width="100%">
	<tr>
		<td class="Huge" colspan="2">
			<asp:Literal ID="litPackageName" runat="server"></asp:Literal>
		</td>
	</tr>
	<tr>
		<td class="SubHead"><asp:Label ID="lblUser" runat="server" meta:resourcekey="lblUser" Text="User:"></asp:Label></td>
		<td class="Normal">
			<scp:UserDetails id="spaceUser" runat="server"/>
		</td>
	</tr>
	<tr>
		<td class="SubHead"><asp:Label ID="lblHostingPlan" runat="server" meta:resourcekey="lblHostingPlan" Text="Hosting Plan:"></asp:Label></td>
		<td class="Normal">
		    <asp:Literal ID="litHostingPlan" runat="server"></asp:Literal>
	    </td>
	</tr>
	<tr id="rowSpaceServer" runat="server">
		<td class="SubHead"><asp:Label ID="lblTargetServer" runat="server" meta:resourcekey="lblTargetServer" Text="Target Server:"></asp:Label></td>
		<td class="Normal">
            <scp:ServerDetails ID="serverDetails" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
            <asp:Label ID="lblCreationDate" runat="server" meta:resourcekey="lblCreationDate" Text="Creation Date:"></asp:Label></td>
		<td class="Normal">
			<asp:Literal ID="litPurchaseDate" runat="server"></asp:Literal>
		</td>
	</tr>
	<tr id="rowStatus" runat="Server">
		<td class="SubHead"><asp:Label ID="lblStatus" runat="server" meta:resourcekey="lblStatus" Text="Status:"></asp:Label></td>
		<td class="Normal">
			<asp:Literal ID="litStatus" runat="server"></asp:Literal>
		</td>
	</tr>
</table> 