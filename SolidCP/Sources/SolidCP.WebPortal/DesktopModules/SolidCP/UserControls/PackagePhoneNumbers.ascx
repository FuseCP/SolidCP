<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackagePhoneNumbers.ascx.cs" Inherits="SolidCP.Portal.UserControls.PackagePhoneNumbers" %>
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
        <scp:SearchBox ID="searchBox" runat="server" />
    </div>
</div>

<asp:GridView ID="gvAddresses" runat="server" AutoGenerateColumns="False"
    Width="100%" EmptyDataText="gvAddresses" CssSelectorClass="NormalGridView"
    AllowPaging="True" AllowSorting="True" DataSourceID="odsExternalAddressesPaged" 
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
            
        <asp:TemplateField HeaderText="gvAddressesItemName" meta:resourcekey="gvAddressesItemName" SortExpression="ItemName">						        						        
	        <ItemTemplate>
		         <asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# GetItemEditUrl(Eval("ItemID").ToString()) %>'>
			        <%# Eval("ItemName") %>
		        </asp:hyperlink>&nbsp;
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
        <asp:QueryStringParameter Name="orgId" QueryStringField="ItemID" DefaultValue="0" />						    
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