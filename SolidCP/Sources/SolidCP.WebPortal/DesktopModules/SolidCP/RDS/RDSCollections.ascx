<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSCollections.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSCollections" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport ID="asyncTasks" runat="server" />


<div class="panel-heading">
    <asp:Image ID="imgRDSCollections" SkinID="EnterpriseRDSCollections48" runat="server" />
    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="RDS Collections"></asp:Localize>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton ID="btnAddCollection" CssClass="btn btn-primary" runat="server" OnClick="btnAddCollection_Click">
        <i class="fa fa-plus">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddCollectionText" />
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton ID="btnImportCollection" CssClass="btn btn-warning" runat="server" OnClick="btnImportCollection_Click">
        <i class="fa fa-download">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnImportCollectionText" />
    </CPCC:StyleButton>
</div>

<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox ID="messageBox" runat="server" />
    <div class="row" style="margin-bottom:15px;">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-7 col-md-offset-5 text-right form-inline">
            <asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem Selected="True">20</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
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
<asp:GridView ID="gvRDSCollections" runat="server" AutoGenerateColumns="False" EnableViewState="true" Width="100%"
    EmptyDataText="gvRDSCollections" CssSelectorClass="NormalGridView" OnRowCommand="gvRDSCollections_RowCommand" AllowPaging="True"
    AllowSorting="True" DataSourceID="odsRDSCollectionsPaged" PageSize="20">
    <Columns>
        <asp:TemplateField HeaderText="gvCollectionName" SortExpression="DisplayName">
            <ItemStyle Width="50%"></ItemStyle>
            <ItemTemplate>
                <asp:LinkButton ID="lnkCollectionName" meta:resourcekey="lnkCollectionName" runat="server" CommandName="EditCollection" CommandArgument='<%# Eval("Id") %>' OnClientClick="ShowProgressDialog('Loading ...');return true;"><%# Eval("DisplayName").ToString() %></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvServer">
            <ItemStyle Width="50%"></ItemStyle>
            <ItemTemplate>
                <asp:Literal ID="litServer" runat="server" Text='<%#GetServerName(Eval("Id").ToString())%>'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemStyle Width="65px" HorizontalAlign="Center" />
            <ItemTemplate>
                <asp:LinkButton ID="lnkRemove" runat="server" CssClass="btn btn-danger" CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Are you sure you want to remove selected rds collection?')">
                    &nbsp;<i class="fa fa-trash-o"></i>&nbsp;
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<div class="panel-footer">
    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Collections Created:"></asp:Localize>&nbsp;	
    <scp:QuotaViewer ID="collectionsQuota" runat="server" QuotaTypeId="2" DisplayGauge="true" />
</div>
<asp:ObjectDataSource ID="odsRDSCollectionsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetRDSCollectonsPagedCount"
    SelectMethod="GetRDSCollectonsPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.RDSHelper">
    <SelectParameters>
        <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>