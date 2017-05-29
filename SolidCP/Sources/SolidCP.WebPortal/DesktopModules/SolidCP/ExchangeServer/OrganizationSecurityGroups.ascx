<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationSecurityGroups.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationSecurityGroups" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    <div class="panel-heading">
        <h3 class="panel-title">
            <asp:Image ID="Image1" SkinID="OrganizationGroup48" runat="server" />
            <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Groups"></asp:Localize>
        </h3>
    </div>
    <div class="FormButtonsBar right">
        <CPCC:StyleButton id="btnCreateGroup" CssClass="btn btn-primary" runat="server" OnClick="btnCreateGroup_Click">
            <i class="fa fa-users">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnCreateGroupText"/>
        </CPCC:StyleButton>
    </div>
    <div class="panel-body form-horizontal">
        <scp:SimpleMessageBox id="messageBox" runat="server" />
        <div class="row">
            <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-7 col-md-offset-5 text-right form-inline">
                <asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
                <div class="form-group">
                    <div class="input-group">
                        <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlPageSize_SelectedIndexChanged">   
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem Selected="True">20</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>100</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="form-control">
                            <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
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
    <asp:GridView ID="gvGroups" runat="server" AutoGenerateColumns="False" EnableViewState="true"
    Width="100%" EmptyDataText="gvGroups" CssSelectorClass="NormalGridView"
    OnRowCommand="gvSecurityGroups_RowCommand" OnRowDataBound="gvSecurityGroups_RowDataBound" AllowPaging="True" AllowSorting="True"
    DataSourceID="odsSecurityGroupsPaged" PageSize="20">
        <Columns>
            <asp:TemplateField HeaderText="gvGroupsDisplayName" SortExpression="DisplayName">
                <ItemStyle Width="35%"></ItemStyle>
                <ItemTemplate>
                    <asp:hyperlink id="lnk1" runat="server" NavigateUrl='<%# GetListEditUrl(Eval("AccountId").ToString()) %>'>
                        <%# Eval("DisplayName") %>
                    </asp:hyperlink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="gvGroupsNotes" DataField="Notes" ItemStyle-Width="65%" />
            <asp:TemplateField>
                <ItemTemplate>
                    <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("AccountId") %>' Visible='<%# IsNotDefault(Eval("AccountType").ToString()) %>' OnClientClick="return confirm('Remove this item?');">
                        &nbsp;
                        <i class="fa fa-trash-o"></i>&nbsp;
                    </CPCC:StyleButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsSecurityGroupsPaged" runat="server" EnablePaging="True"
        SelectCountMethod="GetOrganizationSecurityGroupsPagedCount"
        SelectMethod="GetOrganizationSecurityGroupsPaged"
        SortParameterName="sortColumn"
        TypeName="SolidCP.Portal.OrganizationsHelper"
        OnSelected="odsSecurityGroupsPaged_Selected">
        <SelectParameters>
            <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
            <asp:Parameter Name="accountTypes" DefaultValue="8" />
            <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <div class="panel-footer">
        <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Groups Created:"></asp:Localize>
        &nbsp;&nbsp;&nbsp;
        <scp:QuotaViewer ID="groupsQuota" runat="server" QuotaTypeId="2" />
    </div>