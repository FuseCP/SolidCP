<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceNestedSpaces.ascx.cs" Inherits="SolidCP.Portal.SpaceNestedSpaces" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="scp" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="scp" %>



<div class="FormButtonsBar">
    <div class="Left">
        <scp:SearchBox ID="searchBox" runat="server" />
    </div>
    <div class="Right">
        <asp:Label ID="lblStatus" runat="server" meta:resourcekey="lblStatus" Text="Status:" CssClass="Normal"></asp:Label>
        <asp:DropDownList ID="ddlStatus" runat="server" resourcekey="ddlStatus" CssClass="form-control" AutoPostBack="true">
            <asp:ListItem Value="0">All</asp:ListItem>
            <asp:ListItem Value="1">Active</asp:ListItem>
            <asp:ListItem Value="2">Suspended</asp:ListItem>
            <asp:ListItem Value="3">Cancelled</asp:ListItem>
        </asp:DropDownList>
    </div>
</div>

<asp:GridView ID="gvPackages" runat="server" AutoGenerateColumns="False"
    EmptyDataText="gvPackages" GridLines="both" CssSelectorClass="NormalGridView"
    AllowSorting="True" DataSourceID="odsNestedPackages" AllowPaging="True">
    <Columns>
        <asp:TemplateField SortExpression="PackageName" HeaderText="gvPackagesName">
            <ItemStyle Width="40%"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
		            <%# Eval("PackageName") %>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField SortExpression="Username" HeaderText="gvPackagesUsername" HeaderStyle-Wrap="false">
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
        <asp:TemplateField SortExpression="PlanName" HeaderText="gvPackagesHostingPlan" >
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# GetNestedSpacesPageUrl("PlanID", Eval("PlanID").ToString()) %>'>
					<%# Eval("PlanName") %>
				</asp:hyperlink>
			</ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="ServerName" HeaderText="gvPackagesServer">
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# GetNestedSpacesPageUrl("ServerID", Eval("ServerID").ToString()) %>'>
					<%# Eval("ServerName") %>
				</asp:hyperlink>
			</ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField SortExpression="PurchaseDate" DataField="PurchaseDate" HeaderText="gvPackagesCreationDate" DataFormatString="{0:d}" >
            <ItemStyle Wrap="False" />
            <HeaderStyle Wrap="False" />
        </asp:BoundField>
        <asp:TemplateField SortExpression="StatusID" HeaderText="gvPackagesStatus">
            <ItemTemplate>
		         <%# PanelFormatter.GetPackageStatusName((int)Eval("StatusID"))%>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate><scp:Comments id="Comments1" runat="server"
				    Comments='<%# Eval("Comments") %>'>
                </scp:Comments></ItemTemplate>
            <ItemStyle Width="20px" Wrap="False" />
		</asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsNestedPackages" runat="server" EnablePaging="True"
    SelectCountMethod="GetNestedPackagesPagedCount"
    SelectMethod="GetNestedPackagesPaged" SortParameterName="sortColumn"
    TypeName="SolidCP.Portal.PackagesHelper"
    OnSelected="odsNestedPackages_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" Type="Int32" />
        <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
        <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
        <asp:ControlParameter Name="statusId" ControlID="ddlStatus" PropertyName="SelectedValue" Type="Int32" />
        <asp:QueryStringParameter DefaultValue="0" Name="planId" QueryStringField="PlanID" Type="Int32" />
        <asp:QueryStringParameter DefaultValue="0" Name="serverId" QueryStringField="ServerID" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
