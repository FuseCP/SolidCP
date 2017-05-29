<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDistributionLists.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeDistributionLists" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="Image1" SkinID="ExchangeList48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Lists"></asp:Localize>
    </h3>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton id="btnCreateList" CssClass="btn btn-primary" runat="server" OnClick="btnCreateList_Click">
        <i class="fa fa-users">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCreateList"/>
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
                        <asp:ListItem Value="PrimaryEmailAddress" meta:resourcekey="ddlSearchColumnEmail">Email</asp:ListItem>
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
<asp:GridView ID="gvLists" runat="server" AutoGenerateColumns="False" EnableViewState="true"
    Width="100%" EmptyDataText="gvLists" CssSelectorClass="NormalGridView"
    OnRowCommand="gvLists_RowCommand" AllowPaging="True" AllowSorting="True"
    DataSourceID="odsAccountsPaged" PageSize="20">
    <Columns>
        <asp:TemplateField HeaderText="gvListsDisplayName" SortExpression="DisplayName">
            <ItemStyle Width="50%"></ItemStyle>
            <ItemTemplate>
                <asp:hyperlink id="lnk1" runat="server" NavigateUrl='<%# GetListEditUrl(Eval("AccountId").ToString()) %>'>
                    <%# Eval("DisplayName") %>
                </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="gvListsEmail" DataField="PrimaryEmailAddress" SortExpression="PrimaryEmailAddress" ItemStyle-Width="50%" />
        <asp:TemplateField>
            <ItemTemplate>
                <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("AccountId") %>' OnClientClick="return confirm('Remove this item?');">
                    &nbsp;
                    <i class="fa fa-trash-o"></i>&nbsp;
                </CPCC:StyleButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetExchangeAccountsPagedCount"
    SelectMethod="GetExchangeAccountsPaged" SortParameterName="sortColumn"
    TypeName="SolidCP.Portal.ExchangeHelper" OnSelected="odsAccountsPaged_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
        <asp:Parameter Name="accountTypes" DefaultValue="3" />
        <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>
<div class="panel-footer">
    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Distribution Lists Created:"></asp:Localize>
    &nbsp;&nbsp;&nbsp;
    <scp:QuotaViewer ID="listsQuota" runat="server" QuotaTypeId="2" />
</div>