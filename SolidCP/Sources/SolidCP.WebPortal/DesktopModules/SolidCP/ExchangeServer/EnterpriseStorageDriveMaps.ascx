<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStorageDriveMaps.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.EnterpriseStorageDriveMaps" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="imgESDM" SkinID="EnterpriseStorageDriveMaps48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Drive Maps"></asp:Localize>
    </h3>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton id="btnAddDriveMap" CssClass="btn btn-success" runat="server" OnClick="btnAddDriveMap_Click">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddDriveMap"/>
    </CPCC:StyleButton>
</div>
<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="row">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-5 col-md-offset-7 text-right form-inline">
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
<asp:GridView ID="gvDriveMaps" runat="server" AutoGenerateColumns="False" EnableViewState="true"
    Width="100%" EmptyDataText="gvDriveMaps" CssSelectorClass="NormalGridView"
    OnRowCommand="gvDriveMaps_RowCommand" AllowPaging="True" AllowSorting="True"
    DataSourceID="odsEnterpriseDriveMapsPaged" PageSize="20">
    <Columns>
        <asp:TemplateField HeaderText="gvDrive">
            <ItemStyle Width="25%"></ItemStyle>
            <ItemTemplate>
                <asp:Image ID="img1" runat="server" ImageUrl='<%# GetDriveImage() %>' ImageAlign="AbsMiddle" />
                <asp:Literal id="litDrive" runat="server" Text='<%# string.Format("{0}:", Eval("DriveLetter")) %>'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvLabelAs">
            <ItemStyle Width="25%"></ItemStyle>
            <ItemTemplate>
                <asp:Literal id="litLabelAs" runat="server" Text='<%# Eval("LabelAs") %>'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvFolderUrl">
            <ItemStyle Width="50%"></ItemStyle>
            <ItemTemplate>
                <asp:Literal id="litFolderUrl" runat="server" Text='<%# (Eval("Folder.UncPath") ?? Eval("Folder.Url")).ToString()  %>'></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <CPCC:StyleButton id="imgDelDriveMap" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("Folder.Name") %>' OnClientClick="return confirm('Are you sure you want to delete selected map drive?')">
                    &nbsp;
                    <i class="fa fa-trash-o"></i>&nbsp;
                </CPCC:StyleButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsEnterpriseDriveMapsPaged" runat="server" EnablePaging="True"
    SelectCountMethod="GetEnterpriseDriveMapsPagedCount"
    SelectMethod="GetEnterpriseDriveMapsPaged"
    SortParameterName="sortColumn"
    TypeName="SolidCP.Portal.EnterpriseStorageHelper">
    <SelectParameters>
        <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>