<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingAddons.ascx.cs" Inherits="SolidCP.Portal.HostingAddons" %>
<%@ Import Namespace="SolidCP.Portal" %>
<div class="FormButtonsBar right">
	<CPCC:StyleButton ID="btnAddItem" runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click" >
        <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem"/>
	</CPCC:StyleButton>
</div>
<asp:GridView id="gvAddons" runat="server" AutoGenerateColumns="False"
	DataSourceID="odsAddons" AllowPaging="True" AllowSorting="True" EmptyDataText="gvAddons"
	CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField SortExpression="PlanName" HeaderText="gvAddonsName">
			<ItemStyle Width="100%"></ItemStyle>
			<ItemTemplate>
				<b><asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_addon", "UserID=" + Eval("UserID").ToString()) %>'>
					<%# PortalAntiXSS.EncodeOld((string) Eval("PlanName")) %>
				</asp:hyperlink></b><br />
				<%# PortalAntiXSS.EncodeOld((string) Eval("PlanDescription")) %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:hyperlink id="lnkCopy" meta:resourcekey="lnkCopy" runat="server" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_addon", "UserID=" + Eval("UserID").ToString(), "TargetAction=Copy") %>'>Copy</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="PackagesNumber" SortExpression="PackagesNumber" HeaderText="gvAddonsSpaces"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsAddons" runat="server" SelectMethod="GetRawHostingAddons"
    TypeName="SolidCP.Portal.HostingPlansHelper" OnSelected="odsAddons_Selected"></asp:ObjectDataSource>
