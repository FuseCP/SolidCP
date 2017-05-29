<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceNestedSpacesSummary.ascx.cs" Inherits="SolidCP.Portal.SpaceNestedSpacesSummary" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Import Namespace="SolidCP.Portal" %>

<div class="panel-body form-horizontal">
    <div class="FormRow">
        <asp:Panel ID="tblSearch" runat="server" DefaultButton="cmdSearch" CssClass="NormalBold">
            <asp:DropDownList ID="ddlFilterColumn" runat="server" CssClass="NormalTextBox" resourcekey="ddlFilterColumn" style="vertical-align: middle;">
                <asp:ListItem Value="Username">Username</asp:ListItem>
                <asp:ListItem Value="Email">Email</asp:ListItem>
                <asp:ListItem Value="FullName">FullName</asp:ListItem>
            </asp:DropDownList><asp:TextBox ID="txtFilterValue" runat="server" CssClass="NormalTextBox" Width="100" style="vertical-align: middle;"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" SkinID="SearchButton" meta:resourcekey="cmdSearch"
                CausesValidation="false" OnClick="cmdSearch_Click" style="vertical-align: middle;">
		        </asp:ImageButton>
        </asp:Panel>
    </div>
	<scp:CollapsiblePanel id="allSpaces" runat="server"
		TargetControlID="AllSpacesPanel" resourcekey="AllSpacesPanel" Text="All Spaces">
	</scp:CollapsiblePanel>
	<asp:Panel ID="AllSpacesPanel" runat="server" CssClass="FormRow">
	    <asp:HyperLink ID="lnkAllSpaces" runat="server" meta:resourcekey="lnkAllSpaces" Text="All spaces"></asp:HyperLink>
	</asp:Panel>
	
	<scp:CollapsiblePanel id="spacesbyStatus" runat="server"
		TargetControlID="SpacesByStatusPanel" resourcekey="SpacesByStatusPanel" Text="By Status">
	</scp:CollapsiblePanel>
	<asp:Panel ID="SpacesByStatusPanel" runat="server" CssClass="FormRow">
        <asp:Repeater ID="repSpaceStatuses" runat="server" EnableViewState="false">
            <ItemTemplate>
                <asp:HyperLink ID="lnkBrowseStatus" runat="server" NavigateUrl='<%# GetNestedSpacesPageUrl("StatusID", Eval("StatusID").ToString()) %>'>
                    <%# PanelFormatter.GetPackageStatusName((int)Eval("StatusID"))%> (<%# Eval("PackagesNumber")%>)
                </asp:HyperLink>
            </ItemTemplate>
            <SeparatorTemplate>&nbsp;&nbsp;</SeparatorTemplate>
        </asp:Repeater>
    </asp:Panel>
</div>