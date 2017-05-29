<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerIPAddressesControl.ascx.cs" Inherits="SolidCP.Portal.ServerIPAddressesControl" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<div class="FormButtonsBar">
    <asp:Button ID="btnAdd" runat="server" Text="Add" meta:resourcekey="btnAdd" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
</div>
<asp:GridView id="gvIPAddresses" runat="server" EnableViewState="False" AutoGenerateColumns="False"
	AllowSorting="True"
	CssSelectorClass="NormalGridView"
	AllowPaging="True" DataSourceID="odsIPAddresses" PageSize="5"
	EmptyDataText="gvIPAddresses">
	<Columns>
		<asp:TemplateField SortExpression="ExternalIP" HeaderText="gvIPAddressesExternalIP">
			<ItemTemplate>
				<asp:hyperlink NavigateUrl='<%# EditModuleUrl("AddressID", Eval("AddressID").ToString(), "edit_ip", "ReturnUrl", GetReturnUrl()) %>' runat="server" ID="lnkEdit">
					<%# Eval("ExternalIP") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="InternalIP" SortExpression="InternalIP" HeaderText="gvIPAddressesInternalIP"></asp:BoundField>
		<asp:BoundField DataField="DefaultGateway" SortExpression="DefaultGateway" HeaderText="gvIPAddressesDefaultGateway"></asp:BoundField>
		<asp:BoundField DataField="Comments" HeaderText="gvIPAddressesComments"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsIPAddresses" runat="server" EnablePaging="True"
	    SelectCountMethod="GetIPAddressesPagedCount"
	    SelectMethod="GetIPAddressesPaged"
	    SortParameterName="sortColumn"
	    TypeName="SolidCP.Portal.IPAddressesHelper">
    <SelectParameters>
    	<asp:Parameter Name="pool" DefaultValue="None" />
	    <asp:QueryStringParameter Name="serverId" QueryStringField="ServerID" DefaultValue="0" />
        <asp:Parameter Name="filterColumn" DefaultValue="" />
        <asp:Parameter Name="filterValue" DefaultValue="" />
    </SelectParameters>
</asp:ObjectDataSource>

