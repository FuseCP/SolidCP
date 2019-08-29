<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VLANs.ascx.cs" Inherits="SolidCP.Portal.VLANs" %>
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

<scp:SimpleMessageBox ID="messageBox" runat="server" />


<div class="FormButtonsBar right">
    <CPCC:StyleButton ID="btnAddItem" runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click">
        <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem" />
    </CPCC:StyleButton>
</div>

<div class="panel-body form-horizontal">
    <div class="row">
        <div class="col-md-4">
        </div>
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-8 text-right form-inline">
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlItemsPerPage" runat="server" CssClass="form-control"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlItemsPerPage_SelectedIndexChanged">
                        <asp:ListItem Value="10">10</asp:ListItem>
                        <asp:ListItem Value="20">20</asp:ListItem>
                        <asp:ListItem Value="50">50</asp:ListItem>
                        <asp:ListItem Value="100">100</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlSearchColumn" runat="server" class="form-control">
                        <asp:ListItem Value="Vlan" meta:resourcekey="liVLAN">VLAN</asp:ListItem>
                        <asp:ListItem Value="ServerName" meta:resourcekey="liServer">Server</asp:ListItem>
                        <asp:ListItem Value="Username" meta:resourcekey="liUsername">Username</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="input-group-btn">
                        <CPCC:StyleButton ID="cmdSearch" runat="server" CausesValidation="false" CssClass="btn btn-primary">
                            <i class="fa fa-search" aria-hidden="true"></i>
                        </CPCC:StyleButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</div>

<asp:GridView ID="gvVLANs" runat="server" AutoGenerateColumns="False"
    AllowSorting="True" EmptyDataText="gvVLANs"
    CssSelectorClass="NormalGridView" DataKeyNames="VlanID"
    AllowPaging="True" DataSourceID="odsVLANs">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" onclick="unCheckSelectAll(this);" Enabled='<%# ((int)Eval("PackageID") == 0) %>' />
            </ItemTemplate>
            <ItemStyle Width="10px" />
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Vlan" HeaderText="gvVLANsVLAN">
            <ItemTemplate>
                <asp:HyperLink NavigateUrl='<%# EditUrl("VlanID", DataBinder.Eval(Container.DataItem, "VlanID").ToString(), "edit_vlan", "ReturnUrl=" + GetReturnUrl()) %>' runat="server" ID="Hyperlink2">
					<%# Eval("Vlan") %>
                </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ServerName" SortExpression="ServerName" HeaderText="gvVLANsServer" ItemStyle-Wrap="false"></asp:BoundField>
        <asp:TemplateField HeaderText="hVLANsUser" meta:resourcekey="hVLANsUser" SortExpression="Username">
            <ItemTemplate>
                <%# Eval("UserName") %>&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="hVLANsSpace" meta:resourcekey="hVLANsSpace" SortExpression="PackageName">
            <ItemTemplate>
                <asp:HyperLink ID="lnkSpace" runat="server" NavigateUrl='<%# GetSpaceHomeUrl((int)Eval("PackageID")) %>'>
			        <%# Eval("PackageName") %>
                </asp:HyperLink>&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Comments" HeaderText="gvVLANsComments"></asp:BoundField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsVLANs" runat="server" EnablePaging="True"
    SelectCountMethod="GetVLANsPagedCount"
    SelectMethod="GetVLANsPaged"
    SortParameterName="sortColumn"
    TypeName="SolidCP.Portal.VLANsHelper"
    OnSelected="odsVLANs_Selected">
    <SelectParameters>
        <asp:Parameter Name="serverId" DefaultValue="0" />
        <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>

<div class="panel-footer">
    <div class="row">
        <div class="col-md-9">
            <asp:Button ID="btnDeleteSelected" runat="server" Text="Delete Selected"
                meta:resourcekey="btnDeleteSelected" CssClass="SmallButton"
                CausesValidation="false" OnClick="btnDeleteSelected_Click"></asp:Button>
        </div>
    </div>
</div>
