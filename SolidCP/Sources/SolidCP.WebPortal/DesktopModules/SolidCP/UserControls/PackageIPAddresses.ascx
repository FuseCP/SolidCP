<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackageIPAddresses.ascx.cs" Inherits="SolidCP.Portal.UserControls.PackageIPAddresses" %>
<%@ Register Src="SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="SearchBox.ascx" TagName="SearchBox" TagPrefix="scp" %>


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

<div class="FormButtonsBarClean">
    <div class="FormButtonsBarCleanLeft">
        <CPCC:StyleButton id="btnAllocateAddress" CssClass="btn btn-primary" runat="server" OnClick="btnAllocateAddress_Click" CausesValidation="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAllocateAddressText"/> </CPCC:StyleButton>
    </div>
    <div class="FormButtonsBarCleanRight">
		<div style="float: right;"> <!-- In the future, make it more elegant way -->
			<scp:SearchBox ID="searchBox" runat="server" />	 
		</div>
		<div style="float: right;">
			<asp:Label runat="server" Text="Page size:" CssClass="Normal"></asp:Label>
			<asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"
				onselectedindexchanged="ddlPageSize_SelectedIndexChanged">   
				<asp:ListItem>10</asp:ListItem>   
				<asp:ListItem Selected="True">20</asp:ListItem>   
				<asp:ListItem>50</asp:ListItem>   
				<asp:ListItem>100</asp:ListItem>   
			</asp:DropDownList> 
		</div>
	</div>
</div>

<asp:GridView ID="gvAddresses" runat="server" AutoGenerateColumns="False"
    Width="100%" EmptyDataText="gvAddresses" CssSelectorClass="NormalGridView"
    AllowPaging="True" AllowSorting="True" DataSourceID="odsExternalAddressesPaged" PageSize="20"
    onrowdatabound="gvAddresses_RowDataBound" DataKeyNames="PackageAddressID" >
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" onclick="unCheckSelectAll(this);" />&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField HeaderText="gvAddressesIPAddress" meta:resourcekey="gvAddressesIPAddress"
            DataField="ExternalIP" SortExpression="ExternalIP" />
            
        <asp:BoundField HeaderText="gvAddressesNATAddress" meta:resourcekey="gvAddressesNATAddress"
            DataField="InternalIP" SortExpression="InternalIP" />

        <asp:BoundField HeaderText="gvAddressesDefaultGateway" meta:resourcekey="gvAddressesDefaultGateway"
            DataField="DefaultGateway" SortExpression="DefaultGateway" />

        <asp:BoundField HeaderText="gvAddressesVLAN" meta:resourcekey="gvAddressesVLAN"
            DataField="VLAN" SortExpression="VLAN" />

        <asp:TemplateField HeaderText="gvAddressesItemName" meta:resourcekey="gvAddressesItemName" SortExpression="ItemName">						        						        
	        <ItemTemplate>
		         <asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# GetItemEditUrl(Eval("ItemID").ToString()) %>'>
			        <%# Eval("ItemName") %>
		        </asp:hyperlink>&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesPrimary" meta:resourcekey="gvAddressesPrimary" SortExpression="IsPrimary">						        						        
	        <ItemTemplate>						        
		        <asp:Image ID="imgPrimary" runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsPrimary") %>' />&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesSpace" meta:resourcekey="gvAddressesSpace" SortExpression="PackageName" >
	        <ItemTemplate>
		        <asp:hyperlink id="lnkSpace" runat="server" NavigateUrl='<%# GetSpaceHomeUrl(Eval("PackageID").ToString()) %>'>
			        <%# Eval("PackageName") %>
		        </asp:hyperlink>
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesUser" meta:resourcekey="gvAddressesUser" SortExpression="Username"  >						        
	        <ItemTemplate>
		        <%# Eval("UserName") %>
	        </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsExternalAddressesPaged" runat="server" EnablePaging="True"
	    SelectCountMethod="GetPackageIPAddressesCount"
	    SelectMethod="GetPackageIPAddresses"
	    SortParameterName="sortColumn"
	    TypeName="SolidCP.Portal.VirtualMachinesHelper"
	    OnSelected="odsExternalAddressesPaged_Selected" 
    onselecting="odsExternalAddressesPaged_Selecting">
    <SelectParameters>
	    <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="0" />						    
	    <asp:Parameter Name="pool" DefaultValue="0" />
        <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
        <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
    </SelectParameters>
</asp:ObjectDataSource>

<div style="margin-top:4px;">
    <asp:Button ID="btnDeallocateAddresses" runat="server" meta:resourcekey="btnDeallocateAddresses"
            Text="Deallocate selected" CssClass="SmallButton" CausesValidation="False" 
        onclick="btnDeallocateAddresses_Click" />
</div>