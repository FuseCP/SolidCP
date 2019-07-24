<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerVLANsControl.ascx.cs" Inherits="SolidCP.Portal.ServerVLANsControl" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<div class="FormButtonsBar">
    <asp:Button ID="btnAdd" runat="server" Text="Add" meta:resourcekey="btnAdd" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
</div>
<asp:GridView id="gvVLANs" runat="server" EnableViewState="False" AutoGenerateColumns="False"
	AllowSorting="True"
	CssSelectorClass="NormalGridView"
	AllowPaging="True" DataSourceID="odsVLANs" PageSize="5"
	EmptyDataText="gvVLANs">
	<Columns>
		<asp:TemplateField SortExpression="VLAN" HeaderText="gvVLANsVLAN">
			<ItemTemplate>
				<asp:hyperlink NavigateUrl='<%# EditModuleUrl("VlanID", Eval("VlanID").ToString(), "edit_vlan", "ReturnUrl", GetReturnUrl()) %>' runat="server" ID="lnkEdit">
					<%# Eval("Vlan") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="Comments" HeaderText="gvVLANsComments"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsVLANs" runat="server" EnablePaging="True"
	    SelectCountMethod="GetVLANsPagedCount"
	    SelectMethod="GetVLANsPaged"
	    SortParameterName="sortColumn"
	    TypeName="SolidCP.Portal.VLANsHelper">
    <SelectParameters>
	    <asp:QueryStringParameter Name="serverId" QueryStringField="ServerID" DefaultValue="0" />
        <asp:Parameter Name="filterColumn" DefaultValue="" />
        <asp:Parameter Name="filterValue" DefaultValue="" />
    </SelectParameters>
</asp:ObjectDataSource>