<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPAddresses.ascx.cs" Inherits="SolidCP.Portal.IPAddresses" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<script type="text/javascript">
                function checkAll(selectAllCheckbox) {
                    //get all checkbox and select it
                    $('td :checkbox').prop("checked", selectAllCheckbox.checked);
                }
                function unCheckSelectAll(selectCheckbox) {
                    //if any item is unchecked, uncheck header checkbox as also
                    if (!selectCheckbox.checked)
                        $('th :checkbox').prop("checked", false);
                }
</script>

    <scp:SimpleMessageBox id="messageBox" runat="server" />


<div class="FormButtonsBar right">
    <CPCC:StyleButton ID="btnAddItem" runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click" >
        <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem"/>
    </CPCC:StyleButton>
</div>

<div class="panel-body row">
	<div class="col-md-6">
        <div class="form-group">
		<asp:Label ID="lblPool" runat="server" meta:resourcekey="lblPool" Text="Pool:" CssClass="col-sm-2"></asp:Label>
            <div class="col-sm-10">
		<asp:DropDownList ID="ddlPools" runat="server" CssClass="form-control" AutoPostBack="true">
		    <asp:ListItem Value="General" meta:resourcekey="ddlPoolsGeneral">General</asp:ListItem>
		    <asp:ListItem Value="WebSites" meta:resourcekey="ddlPoolsWebSites">WebSites</asp:ListItem>
		    <asp:ListItem Value="VpsExternalNetwork" meta:resourcekey="ddlPoolsVpsExternalNetwork">VpsExternalNetwork</asp:ListItem>
		    <asp:ListItem Value="VpsManagementNetwork" meta:resourcekey="ddlPoolsVpsManagementNetwork">VpsManagementNetwork</asp:ListItem>
		</asp:DropDownList>
                 </div>
            </div>
	</div>
	<div class="col-md-6">
		<scp:SearchBox ID="searchBox" runat="server" />
	</div>
</div>
    
<asp:GridView id="gvIPAddresses" runat="server" AutoGenerateColumns="False"
	AllowSorting="True" EmptyDataText="gvIPAddresses"
	CssSelectorClass="NormalGridView" DataKeyNames="AddressID"
	AllowPaging="True" DataSourceID="odsIPAddresses">
	<Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" onclick="unCheckSelectAll(this);" Enabled='<%# ((int)Eval("ItemID") == 0) %>'  />
            </ItemTemplate>
            <ItemStyle Width="10px" />
        </asp:TemplateField>
		<asp:TemplateField SortExpression="ExternalIP" HeaderText="gvIPAddressesExternalIP">
			<ItemTemplate>
				<asp:hyperlink NavigateUrl='<%# EditUrl("AddressID", DataBinder.Eval(Container.DataItem, "AddressID").ToString(), "edit_ip", "ReturnUrl=" + GetReturnUrl()) %>' runat="server" ID="Hyperlink2">
					<%# Eval("ExternalIP") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="InternalIP" SortExpression="InternalIP" HeaderText="gvIPAddressesInternalIP"></asp:BoundField>
        <asp:BoundField DataField="DefaultGateway" SortExpression="DefaultGateway" HeaderText="gvIPAddressesGateway"></asp:BoundField>
        <asp:BoundField DataField="VLAN" SortExpression="VLAN" HeaderText="VLAN"></asp:BoundField>
		<asp:BoundField DataField="ServerName" SortExpression="ServerName" HeaderText="gvIPAddressesServer" ItemStyle-Wrap="false"></asp:BoundField>
		<asp:TemplateField HeaderText="gvAddressesUser" meta:resourcekey="gvAddressesUser" SortExpression="Username"  >						        
	        <ItemTemplate>
		        <%# Eval("UserName") %>&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesSpace" meta:resourcekey="gvAddressesSpace" SortExpression="PackageName" >
	        <ItemTemplate>
		        <asp:hyperlink id="lnkSpace" runat="server" NavigateUrl='<%# GetSpaceHomeUrl((int)Eval("PackageID")) %>'>
			        <%# Eval("PackageName") %>
		        </asp:hyperlink>&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField HeaderText="gvAddressesItemName" meta:resourcekey="gvAddressesItemName" DataField="ItemName" SortExpression="ItemName" />
        <asp:BoundField DataField="Comments" HeaderText="gvIPAddressesComments"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsIPAddresses" runat="server" EnablePaging="True"
	    SelectCountMethod="GetIPAddressesPagedCount"
	    SelectMethod="GetIPAddressesPaged"
	    SortParameterName="sortColumn"
	    TypeName="SolidCP.Portal.IPAddressesHelper"
	    OnSelected="odsIPAddresses_Selected">
    <SelectParameters>
	    <asp:ControlParameter Name="pool" ControlID="ddlPools" PropertyName="SelectedValue" />
	    <asp:Parameter Name="serverId" DefaultValue="0" />
        <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
        <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
    </SelectParameters>
</asp:ObjectDataSource>

<div class="panel-footer">
    <div class="row">
    <div class="col-md-9">
        <asp:Button id="btnEditSelected" runat="server" Text="Edit Selected..."
            meta:resourcekey="btnEditSelected" CssClass="SmallButton" 
            CausesValidation="false" onclick="btnEditSelected_Click"></asp:Button>
        <asp:Button id="btnDeleteSelected" runat="server" Text="Delete Selected"
            meta:resourcekey="btnDeleteSelected" CssClass="SmallButton" 
            CausesValidation="false" onclick="btnDeleteSelected_Click"></asp:Button>
    </div>
    <div class="col-md-3">
        <div class="form-group">
        <asp:Label ID="lblItemsPerPage" runat="server" meta:resourcekey="lblItemsPerPage" Text="Page size:" CssClass="col-sm-4"></asp:Label>
            <div class="col-sm-8">
        <asp:DropDownList ID="ddlItemsPerPage" runat="server" CssClass="form-control" 
            AutoPostBack="True" onselectedindexchanged="ddlItemsPerPage_SelectedIndexChanged">
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="20">20</asp:ListItem>
            <asp:ListItem Value="50">50</asp:ListItem>
            <asp:ListItem Value="100">100</asp:ListItem>
        </asp:DropDownList>
                </div>
  </div> </div>
    </div>
</div>