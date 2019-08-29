<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackageVLANs.ascx.cs" Inherits="SolidCP.Portal.UserControls.PackageVLANs" %>
<%@ Register Src="SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>


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
        <CPCC:StyleButton id="btnAllocateVLAN" CssClass="btn btn-primary" runat="server" OnClick="btnAllocateVLAN_Click" CausesValidation="False"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAllocateVLANText"/> </CPCC:StyleButton>
    </div>
    <div class="FormButtonsBarCleanRight">
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

<asp:GridView ID="gvVLANs" runat="server" AutoGenerateColumns="False"
    Width="100%" EmptyDataText="gvVLANs" CssSelectorClass="NormalGridView"
    AllowPaging="True" AllowSorting="True" DataSourceID="odsVLANsPaged" PageSize="20"
    onrowdatabound="gvVLANs_RowDataBound" DataKeyNames="PackageVlanID" >
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" onclick="unCheckSelectAll(this);" />&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField HeaderText="gvVLANsVLAN" meta:resourcekey="gvVLANsVLAN"
            DataField="Vlan" SortExpression="Vlan" />

        <asp:TemplateField HeaderText="gvVLANsSpace" meta:resourcekey="gvVLANsSpace" SortExpression="PackageName" >
	        <ItemTemplate>
		        <asp:hyperlink id="lnkSpace" runat="server" NavigateUrl='<%# GetSpaceHomeUrl(Eval("PackageID").ToString()) %>'>
			        <%# Eval("PackageName") %>
		        </asp:hyperlink>
	        </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="gvVLANsUser" meta:resourcekey="gvVLANsUser" SortExpression="Username"  >						        
	        <ItemTemplate>
		        <%# Eval("UserName") %>
	        </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsVLANsPaged" runat="server" EnablePaging="True"
	    SelectCountMethod="GetPackageVLANsCount"
	    SelectMethod="GetPackageVLANs"
	    SortParameterName="sortColumn"
	    TypeName="SolidCP.Portal.VirtualMachines2012Helper"
	    OnSelected="odsVLANsPaged_Selected">
    <SelectParameters>
	    <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="0" />						    
    </SelectParameters>
</asp:ObjectDataSource>

<div style="margin-top:4px;">
    <asp:Button ID="btnDeallocateVLANs" runat="server" meta:resourcekey="btnDeallocateVLANs"
            Text="Deallocate selected" CssClass="SmallButton" CausesValidation="False" 
        onclick="btnDeallocateVLANs_Click" />
</div>